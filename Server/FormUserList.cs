using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using Shared;


namespace Server
{
    public partial class FormUserList : Form
    {
        private string _userDbFile;
        private DataGridView dgv;
        private List<UserInfo> userList = new();
        private HashSet<string> onlineUsers;
        private Dictionary<string, TcpClient> loggedInUsers;

        public FormUserList(string userDbFile, HashSet<string> onlineUsers = null, Dictionary<string, TcpClient> loggedInUsers = null)
        {
            _userDbFile = userDbFile;
            this.onlineUsers = onlineUsers ?? new HashSet<string>();
            this.loggedInUsers = loggedInUsers ?? new Dictionary<string, TcpClient>();
            this.Text = "帳號清單";
            this.ClientSize = new System.Drawing.Size(700, 400);

            dgv = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                ReadOnly = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            dgv.Columns.Add("Username", "帳號");
            dgv.Columns.Add("Password", "密碼");
            dgv.Columns.Add("RegTime", "註冊時間");

            // 狀態下拉欄
            var statusCol = new DataGridViewComboBoxColumn
            {
                Name = "Status",
                HeaderText = "狀態",
                DataSource = new string[] { "在線", "離線" }
            };
            dgv.Columns.Add(statusCol);

            // 刪除按鈕欄
            var delCol = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "刪除帳號",
                Text = "刪除",
                UseColumnTextForButtonValue = true
            };
            dgv.Columns.Add(delCol);

            dgv.CellValueChanged += Dgv_CellValueChanged;
            dgv.CellClick += Dgv_CellClick;

            this.Controls.Add(dgv);

            LoadUserList();

            // 註冊區塊
            var txtUsername = new TextBox { PlaceholderText = "帳號", Top = 310, Left = 20, Width = 120 };
            var txtPassword = new TextBox { PlaceholderText = "密碼", Top = 310, Left = 150, Width = 120 };
            var btnRegister = new Button { Text = "註冊", Top = 310, Left = 280, Width = 80 };
            btnRegister.Click += (s, e) =>
            {
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text.Trim();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("請輸入帳號與密碼", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (IsUserExists(username))
                {
                    MessageBox.Show("帳號已被使用", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var user = new UserInfo(username, password);
                File.AppendAllLines(_userDbFile, [user.ToString()]);
                userList.Add(user);
                string status = onlineUsers.Contains(user.Username) ? "在線" : "離線";
                dgv.Rows.Add(user.Username, user.Password, user.RegisterTime, status);
                txtUsername.Clear();
                txtPassword.Clear();
                MessageBox.Show("註冊成功", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnRegister);
        }
        private void LoadUserList()
        {
            dgv.Rows.Clear();
            userList.Clear();
            if (File.Exists(_userDbFile))
            {
                foreach (var line in File.ReadAllLines(_userDbFile))
                {
                    try
                    {
                        var user = UserInfo.Parse(line);
                        userList.Add(user);
                        string status = onlineUsers.Contains(user.Username) ? "在線" : "離線";
                        dgv.Rows.Add(user.Username, user.Password, user.RegisterTime, status);
                    }
                    catch { }
                }
            }
        }
        private bool IsUserExists(string username)
        {
            return userList.Any(u => u.Username == username);
        }
        // 狀態下拉選單變更
        private void Dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != "Status") return;
            string username = dgv.Rows[e.RowIndex].Cells["Username"].Value?.ToString();
            string status = dgv.Rows[e.RowIndex].Cells["Status"].Value?.ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(status)) return;

            if (status == "在線")
            {
                onlineUsers.Add(username);
            }
            else // 狀態改為離線
            {
                onlineUsers.Remove(username);
                // 通知並踢出
                if (loggedInUsers != null && loggedInUsers.TryGetValue(username, out var client))
                {
                    try
                    {
                        var stream = client.GetStream();
                        string kickMsg = "You have been logged out from Server \n";
                        byte[] data = Encoding.UTF8.GetBytes(kickMsg);
                        stream.Write(data, 0, data.Length);                        
                    }
                    catch { }
                    try { client.Close(); } catch { }
                    loggedInUsers.Remove(username);
                }
            }
        }
        // 刪除按鈕
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != "Delete") return;
            string username = dgv.Rows[e.RowIndex].Cells["Username"].Value?.ToString();
            if (string.IsNullOrEmpty(username)) return;

            if (MessageBox.Show($"確定要刪除帳號 {username}？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // 從 userList 移除
                userList.RemoveAll(u => u.Username == username);
                // 從 onlineUsers 移除
                onlineUsers.Remove(username);
                // 從檔案移除
                File.WriteAllLines(_userDbFile, userList.Select(u => u.ToString()));
                // 從 DataGridView 移除
                dgv.Rows.RemoveAt(e.RowIndex);
                MessageBox.Show("帳號已刪除", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
