using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace TCP_Client
{
    public partial class FormUser : Form
    {
        private Client.TCP_Client mainForm;
        public string UserName => textBoxUser.Text;

        public FormUser(Client.TCP_Client tCP_Client)
        {
            InitializeComponent();
            mainForm = tCP_Client;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string user = textBoxUser.Text.Trim();
            string pwd = textBoxPwd.Text.Trim();
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("請輸入帳號與密碼");
                return;
            }
            try
            {
                mainForm.ConnectToServer();
                string msg = $"REGISTER|{user}|{pwd}";
                mainForm.stream.Write(Encoding.UTF8.GetBytes(msg), 0, Encoding.UTF8.GetByteCount(msg));
                byte[] buffer = new byte[64];
                int len = mainForm.stream.Read(buffer, 0, buffer.Length);
                string reply = Encoding.UTF8.GetString(buffer, 0, len);
                if (reply == "REGISTER_OK")
                {
                    MessageBox.Show("註冊成功，已登入聊天室");
                    mainForm.currentUser = user;
                    mainForm.StartReceive();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (reply == "REGISTER_DUP")
                {
                    MessageBox.Show("註冊失敗，帳號已被使用或已在線");
                    mainForm.stream.Close(); mainForm.client.Close();
                }
                else
                {
                    MessageBox.Show("註冊失敗，未知錯誤");
                    mainForm.stream.Close(); mainForm.client.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("註冊失敗：" + ex.Message);
            }
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string user = textBoxUser.Text.Trim();
            string pwd = textBoxPwd.Text.Trim();
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("請輸入帳號與密碼");
                return;
            }
            try
            {
                mainForm.ConnectToServer();
                string msg = $"LOGIN|{user}|{pwd}";
                mainForm.stream.Write(Encoding.UTF8.GetBytes(msg), 0, Encoding.UTF8.GetByteCount(msg));
                byte[] buffer = new byte[64];
                int len = mainForm.stream.Read(buffer, 0, buffer.Length);
                string reply = Encoding.UTF8.GetString(buffer, 0, len);
                if (reply == "LOGIN_OK")
                {
                    MessageBox.Show("登入成功");
                    mainForm.currentUser = user;
                    mainForm.StartReceive();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (reply == "LOGIN_DUP")
                {
                    MessageBox.Show("帳號已被登入（在線）");
                    mainForm.stream.Close(); mainForm.client.Close();
                }
                else if (reply == "LOGIN_FAIL")
                {
                    MessageBox.Show("登入失敗，帳號或密碼錯誤");
                    mainForm.stream.Close(); mainForm.client.Close();
                }
                else
                {
                    MessageBox.Show("登入失敗，未知錯誤");
                    mainForm.stream.Close(); mainForm.client.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登入失敗：" + ex.Message);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            using (var confirm = new Shared.FormExitConfirm())
            {
                if (confirm.ShowDialog() == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
    }
}
