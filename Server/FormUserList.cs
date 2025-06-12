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
            this.Text = "�b���M��";
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
            dgv.Columns.Add("Username", "�b��");
            dgv.Columns.Add("Password", "�K�X");
            dgv.Columns.Add("RegTime", "���U�ɶ�");

            // ���A�U����
            var statusCol = new DataGridViewComboBoxColumn
            {
                Name = "Status",
                HeaderText = "���A",
                DataSource = new string[] { "�b�u", "���u" }
            };
            dgv.Columns.Add(statusCol);

            // �R�����s��
            var delCol = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "�R���b��",
                Text = "�R��",
                UseColumnTextForButtonValue = true
            };
            dgv.Columns.Add(delCol);

            dgv.CellValueChanged += Dgv_CellValueChanged;
            dgv.CellClick += Dgv_CellClick;

            this.Controls.Add(dgv);

            LoadUserList();

            // ���U�϶�
            var txtUsername = new TextBox { PlaceholderText = "�b��", Top = 310, Left = 20, Width = 120 };
            var txtPassword = new TextBox { PlaceholderText = "�K�X", Top = 310, Left = 150, Width = 120 };
            var btnRegister = new Button { Text = "���U", Top = 310, Left = 280, Width = 80 };
            btnRegister.Click += (s, e) =>
            {
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text.Trim();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("�п�J�b���P�K�X", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (IsUserExists(username))
                {
                    MessageBox.Show("�b���w�Q�ϥ�", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var user = new UserInfo(username, password);
                File.AppendAllLines(_userDbFile, [user.ToString()]);
                userList.Add(user);
                string status = onlineUsers.Contains(user.Username) ? "�b�u" : "���u";
                dgv.Rows.Add(user.Username, user.Password, user.RegisterTime, status);
                txtUsername.Clear();
                txtPassword.Clear();
                MessageBox.Show("���U���\", "�T��", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        string status = onlineUsers.Contains(user.Username) ? "�b�u" : "���u";
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
        // ���A�U�Կ���ܧ�
        private void Dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != "Status") return;
            string username = dgv.Rows[e.RowIndex].Cells["Username"].Value?.ToString();
            string status = dgv.Rows[e.RowIndex].Cells["Status"].Value?.ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(status)) return;

            if (status == "�b�u")
            {
                onlineUsers.Add(username);
            }
            else // ���A�אּ���u
            {
                onlineUsers.Remove(username);
                // �q���ý�X
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
        // �R�����s
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != "Delete") return;
            string username = dgv.Rows[e.RowIndex].Cells["Username"].Value?.ToString();
            if (string.IsNullOrEmpty(username)) return;

            if (MessageBox.Show($"�T�w�n�R���b�� {username}�H", "�T�{", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // �q userList ����
                userList.RemoveAll(u => u.Username == username);
                // �q onlineUsers ����
                onlineUsers.Remove(username);
                // �q�ɮײ���
                File.WriteAllLines(_userDbFile, userList.Select(u => u.ToString()));
                // �q DataGridView ����
                dgv.Rows.RemoveAt(e.RowIndex);
                MessageBox.Show("�b���w�R��", "�T��", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
