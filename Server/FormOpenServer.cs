using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace TCP_server
{
    public partial class FormOpenServer : Form
    {
        public string ServerIP => textBox1.Text.Trim();
        public string ServerPort => textBox2.Text.Trim();

        public FormOpenServer(string defaultIP, string defaultPort)
        {
            InitializeComponent();

            // 設定標題與表單大小
            this.Text = "啟動伺服器";
            this.ClientSize = new System.Drawing.Size(350, 180);

            // 設定預設值
            textBox1.Text = defaultIP;   // 這裡改成用 defaultIP
            textBox2.Text = defaultPort;

            // 設定按鈕 DialogResult
            btnCheck.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            // 設定表單的 Accept/Cancel 按鈕
            this.AcceptButton = btnCheck;
            this.CancelButton = btnCancel;
        }

        private void buttonGetIP_Click_Click(object sender, EventArgs e)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipList = host.AddressList
                .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(ip => ip.ToString())
                .ToList();

            if (ipList.Count > 0)
            {
                // 取第一個非127.0.0.1的IP（通常是區網IP）
                string localIP = ipList.FirstOrDefault(ip => ip != "127.0.0.1") ?? "127.0.0.1";
                textBox1.Text = localIP;
                MessageBox.Show("本機可用IP:\n" + string.Join("\n", ipList), "本機IP");
            }
            else
            {
                MessageBox.Show("找不到本機IPv4位址");
            }
        }
    }
}
