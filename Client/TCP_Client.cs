using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TCP_Client;
using Shared;

namespace Client
{
    public partial class TCP_Client : Form
    {
        public TcpClient client;
        public NetworkStream stream;
        public Thread receiveThread;
        public string currentUser = "";
        private Guid? lastSentMessageId = null;
        private System.Windows.Forms.Timer loginMarqueeTimer;
        private string loginMarqueeText = "登入成功     ";

        public TCP_Client()
        {
            InitializeComponent();
            textBox1.Text = "127.0.0.1";
            textBox2.Text = "8888";
            button3.Click += button3_Click; // 傳送訊息
            button4.Click += button4_Click; // 清除訊息
            button7.Click += button7_Click; // 表情按鈕
            MenuItemExit.Click += MenuItemExit_Click;
            richTextBox1.Font = new Font("Segoe UI Emoji", 12);
            btnHi.Click += (s, e) => SendTextMessage("Hi");
            btnBye.Click += (s, e) => SendTextMessage("Bye");
            btnOMG.Click += (s, e) => SendTextMessage("OMG");
            //跑馬燈
            loginMarqueeTimer = new System.Windows.Forms.Timer();//引用計時器
            loginMarqueeTimer.Interval = 200;
            loginMarqueeTimer.Tick += LoginMarqueeTimer_Tick;
        }
        // 輔助方法：根據檔案類型取得中文描述
        private string GetFileTypeText(string fileType)
        {
            switch (fileType)
            {
                case "TEXT": return "文字檔";
                case "IMAGE": return "圖片檔";
                case "FILE": return "檔案";
                default: return "檔案";
            }
        }
        // 啟動接收執行緒
        public void StartReceive()
        {
            receiveThread = new Thread(ReceiveMessage);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        private void LoginMarqueeTimer_Tick(object sender, EventArgs e)
        {
            // 輪轉文字：移除第一個字，加到後面
            loginMarqueeText = loginMarqueeText.Substring(1) + loginMarqueeText[0];
            label3.Text = loginMarqueeText;
        }        
        public void ReceiveMessage()
        {
            try
            {
                while (client != null && client.Connected)
                {
                    // 1. 逐字元讀 header 直到 \n
                    StringBuilder headerBuilder = new StringBuilder();
                    while (true)
                    {
                        int b = stream.ReadByte();
                        if (b == -1) return; // 連線中斷
                        if (b == '\n') break; // 讀取到換行符，表示 header 結束
                        headerBuilder.Append((char)b);
                    }
                    string header = headerBuilder.ToString();

                    // 2. 判斷協定型別
                    if (header.StartsWith("MSGINFO|"))
                    {
                        var msgInfo = MessageInfo.Parse(header.Substring(8));
                        if (lastSentMessageId.HasValue && msgInfo.MessageId == lastSentMessageId.Value)
                        {
                            continue; // 跳過自己剛送出的訊息
                        }

                        if (msgInfo.Type == "IMAGE")
                        {
                            // 顯示圖片
                            try
                            {
                                // 確認 FileContent 有內容
                                if (msgInfo.FileContent != null && msgInfo.FileContent.Length > 0)
                                {
                                    using (var ms = new MemoryStream(msgInfo.FileContent))
                                    {
                                        Image img = Image.FromStream(ms);
                                        if (pictureBox1.InvokeRequired)
                                            pictureBox1.Invoke(new Action(() => pictureBox1.Image = img));
                                        else
                                            pictureBox1.Image = img;
                                    }

                                    string display = $"[{msgInfo.Sender} {msgInfo.Time}]：傳送了一張圖片 ({msgInfo.FileName})\n";
                                    if (richTextBox1.InvokeRequired)
                                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                                    else
                                        richTextBox1.AppendText(display);

                                    // 可以加上除錯訊息
                                    Console.WriteLine($"顯示圖片成功: {msgInfo.FileName}, 大小: {msgInfo.FileContent.Length} bytes");

                                    // 若有檔名，詢問是否儲存
                                    if (!string.IsNullOrEmpty(msgInfo.FileName))
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            using (SaveFileDialog sfd = new SaveFileDialog())
                                            {
                                                sfd.FileName = msgInfo.FileName;
                                                sfd.Filter = "圖片檔|*.jpg;*.jpeg;*.png;*.bmp;*.gif|所有檔案|*.*";
                                                if (sfd.ShowDialog() == DialogResult.OK)
                                                {
                                                    try
                                                    {
                                                        File.WriteAllBytes(sfd.FileName, msgInfo.FileContent);
                                                        MessageBox.Show($"圖片已成功儲存至: {sfd.FileName}", "儲存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show($"儲存圖片時發生錯誤: {ex.Message}", "儲存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    }
                                                }
                                            }
                                        }));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"圖片內容為空: {msgInfo.FileName}");
                                    string display = $"[{msgInfo.Sender} {msgInfo.Time}]：傳送了一張圖片，但內容為空\n";
                                    if (richTextBox1.InvokeRequired)
                                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                                    else
                                        richTextBox1.AppendText(display);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"顯示圖片時發生錯誤: {ex.Message}");
                                string display = $"[{msgInfo.Sender} {msgInfo.Time}]：傳送了一張圖片，但顯示失敗 ({ex.Message})\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                                else
                                    richTextBox1.AppendText(display);
                            }
                        }
                        else if (msgInfo.Type == "TEXT")
                        {
                            string display = $"[{msgInfo.Sender} {msgInfo.Time}]：{msgInfo.Content}\n";
                            if (richTextBox1.InvokeRequired)
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                            else
                                richTextBox1.AppendText(display);

                            // 若有檔名（純文字檔），詢問是否儲存
                            if (!string.IsNullOrEmpty(msgInfo.FileName))
                            {
                                this.Invoke(new Action(() =>
                                {
                                    using (SaveFileDialog sfd = new SaveFileDialog())
                                    {
                                        sfd.FileName = msgInfo.FileName;
                                        sfd.Filter = "文字檔|*.txt|所有檔案|*.*";
                                        if (sfd.ShowDialog() == DialogResult.OK)
                                        {
                                            try
                                            {
                                                File.WriteAllText(sfd.FileName, msgInfo.Content, Encoding.UTF8);
                                                MessageBox.Show($"文字檔已成功儲存至: {sfd.FileName}", "儲存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show($"儲存文字檔時發生錯誤: {ex.Message}", "儲存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                }));
                            }
                        }
                        else if (msgInfo.Type == "FILE")
                        {
                            // 處理一般檔案
                            string display = $"[{msgInfo.Sender} {msgInfo.Time}]：傳送了一個檔案 ({msgInfo.FileName})\n";
                            if (richTextBox1.InvokeRequired)
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                            else
                                richTextBox1.AppendText(display);

                            if (msgInfo.FileContent != null && msgInfo.FileContent.Length > 0 && !string.IsNullOrEmpty(msgInfo.FileName))
                            {
                                this.Invoke(new Action(() =>
                                {
                                    using (SaveFileDialog sfd = new SaveFileDialog())
                                    {
                                        sfd.FileName = msgInfo.FileName;
                                        sfd.Filter = "所有檔案|*.*";
                                        if (sfd.ShowDialog() == DialogResult.OK)
                                        {
                                            try
                                            {
                                                File.WriteAllBytes(sfd.FileName, msgInfo.FileContent);
                                                MessageBox.Show($"檔案已成功儲存至: {sfd.FileName}", "儲存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show($"儲存檔案時發生錯誤: {ex.Message}", "儲存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                }));
                            }
                        }
                    }
                    else if (header.StartsWith("TEXT|") || header.StartsWith("IMAGE|") || header.StartsWith("FILE|"))
                    {
                        // 解析 header
                        string[] parts = header.Split('|');
                        if (parts.Length < 3) continue;
                        string fileType = parts[0];
                        string fileName = parts[1];
                        if (!int.TryParse(parts[2], out int fileLen) || fileLen <= 0) continue;

                        // 3. 讀取檔案內容
                        byte[] fileBuffer = new byte[fileLen];
                        int totalRead = 0;
                        int readAttempts = 0;
                        const int maxReadAttempts = 100; // 設定最大讀取嘗試次數

                        while (totalRead < fileLen && readAttempts < maxReadAttempts)
                        {
                            int read = stream.Read(fileBuffer, totalRead, fileLen - totalRead);
                            if (read == 0)
                            {
                                readAttempts++;
                                System.Threading.Thread.Sleep(10); // 稍微暫停一下再試
                                continue;
                            }
                            totalRead += read;
                            readAttempts = 0; // 成功讀取，重置嘗試計數
                        }

                        if (totalRead < fileLen)
                        {
                            string errorMsg = $"[錯誤] 檔案接收不完整: {fileName}, 僅接收 {totalRead}/{fileLen} bytes\n";
                            if (richTextBox1.InvokeRequired)
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(errorMsg)));
                            else
                                richTextBox1.AppendText(errorMsg);
                            continue;
                        }

                        // 若為圖片，先直接顯示
                        if (fileType == "IMAGE")
                        {
                            try
                            {
                                using (var ms = new MemoryStream(fileBuffer))
                                {
                                    Image img = Image.FromStream(ms);
                                    if (pictureBox1.InvokeRequired)
                                        pictureBox1.Invoke(new Action(() => pictureBox1.Image = img));
                                    else
                                        pictureBox1.Image = img;
                                }

                                // 顯示接收訊息
                                string displayMsg = $"[伺服器傳送圖片] {fileName} ({fileBuffer.Length} bytes)\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(displayMsg)));
                                else
                                    richTextBox1.AppendText(displayMsg);
                            }
                            catch (Exception ex)
                            {
                                string errorMsg = $"[錯誤] 顯示圖片失敗: {ex.Message}\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(errorMsg)));
                                else
                                    richTextBox1.AppendText(errorMsg);
                            }
                        }

                        // 4. 詢問是否儲存檔案
                        string savePath = null;
                        this.Invoke(new Action(() =>
                        {
                            using (SaveFileDialog sfd = new SaveFileDialog())
                            {
                                sfd.FileName = fileName;
                                switch (fileType)
                                {
                                    case "TEXT":
                                        sfd.Filter = "文字檔|*.txt|所有檔案|*.*";
                                        break;
                                    case "IMAGE":
                                        sfd.Filter = "圖片檔|*.jpg;*.jpeg;*.png;*.bmp;*.gif|所有檔案|*.*";
                                        break;
                                    default:
                                        sfd.Filter = "所有檔案|*.*";
                                        break;
                                }

                                if (sfd.ShowDialog() == DialogResult.OK)
                                {
                                    savePath = sfd.FileName;
                                }
                            }
                        }));

                        // 5. 儲存檔案並顯示訊息
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        if (!string.IsNullOrEmpty(savePath))
                        {
                            try
                            {
                                File.WriteAllBytes(savePath, fileBuffer);
                                string msg = $"[伺服器於 {time} 傳送{GetFileTypeText(fileType)}]：{fileName} 已儲存至 {savePath}\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(msg)));
                                else
                                    richTextBox1.AppendText(msg);

                                // 若為圖片且尚未顯示，則顯示
                                if (fileType == "IMAGE" && pictureBox1.Image == null)
                                {
                                    pictureBox1.Image = Image.FromFile(savePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                string errorMsg = $"[錯誤] 儲存檔案失敗: {ex.Message}\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(errorMsg)));
                                else
                                    richTextBox1.AppendText(errorMsg);
                            }
                        }
                        else
                        {
                            string msg = $"[伺服器於 {time}]：收到{GetFileTypeText(fileType)} {fileName}，但未儲存。\n";
                            if (richTextBox1.InvokeRequired)
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(msg)));
                            else
                                richTextBox1.AppendText(msg);
                        }
                    }
                    else
                    {
                        // 其他未知訊息
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        string displayMsg = $"[伺服器訊息 {time}]：{header}\n";
                        if (richTextBox1.InvokeRequired)
                            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(displayMsg)));
                        else
                            richTextBox1.AppendText(displayMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                // 連線中斷或其他例外狀況
                Console.WriteLine($"接收訊息時發生錯誤: {ex.Message}");

                // 可以選擇在 UI 顯示連線中斷訊息
                try
                {
                    if (richTextBox1.InvokeRequired)
                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"[系統] 連線已中斷: {ex.Message}\n")));
                    else
                        richTextBox1.AppendText($"[系統] 連線已中斷: {ex.Message}\n");
                }
                catch { /* 忽略在 UI 更新時可能發生的例外 */ }
            }
            finally
            {
                // 確保在執行緒結束時釋放資源
                if (client != null)
                {
                    try
                    {
                        if (client.Connected)
                        {
                            stream?.Close();
                            client.Close();
                        }
                    }
                    catch { /* 忽略關閉連線時可能發生的例外 */ }
                }
            }
        }        
        // 傳送訊息按鈕事件
        private void button3_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected) return;
            if (string.IsNullOrWhiteSpace(textBox3.Text)) return;

            var msgInfo = new MessageInfo
            {
                Sender = currentUser,
                Type = "TEXT",
                Content = textBox3.Text,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            // 記錄最後送出的訊息ID
            lastSentMessageId = msgInfo.MessageId;

            // 直接構建MSGINFO字串並發送，不使用FileTransferHelper.SendMessage
            try
            {
                string line = "MSGINFO|" + msgInfo.ToString() + "\n";
                byte[] data = Encoding.UTF8.GetBytes(line);

                // 確保自己也能看到訊息
                string selfDisplay = $"[{currentUser} {msgInfo.Time}]：{msgInfo.Content}\n";
                richTextBox1.AppendText(selfDisplay);

                // 發送到伺服器
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"傳送訊息失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 傳送訊息（含emoji）
        private void SendTextMessage(string message)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                MessageBox.Show("請先登入！");
                return;
            }
            if (string.IsNullOrWhiteSpace(message))
                return;

            var msgInfo = new MessageInfo
            {
                Sender = currentUser,
                Type = "TEXT",
                Content = message,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            // 記錄最後送出的訊息ID
            lastSentMessageId = msgInfo.MessageId;

            try
            {
                // 直接構建MSGINFO字串並發送
                string line = "MSGINFO|" + msgInfo.ToString() + "\n";
                byte[] data = Encoding.UTF8.GetBytes(line);

                // 確保自己也能看到訊息
                string selfDisplay = $"[{currentUser} {msgInfo.Time}]：{message}\n";
                richTextBox1.AppendText(selfDisplay);

                // 發送到伺服器
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"傳送訊息失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 清除訊息顯示區
        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.Clear()));
            }
            else
            {
                richTextBox1.Clear();
            }
        }
        // 表情選擇
        private void button7_Click(object sender, EventArgs e)
        {
            using (var picker = new EmojiPickerForm())
            {
                if (picker.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(picker.SelectedEmoji))
                {
                    // 直接傳送emoji訊息
                    int selectionIndex = textBox3.SelectionStart;
                    textBox3.Text = textBox3.Text.Insert(selectionIndex, picker.SelectedEmoji);
                    textBox3.SelectionStart = selectionIndex + picker.SelectedEmoji.Length;
                    textBox3.Focus();
                }
            }
        }
        // 連線到伺服器（註冊/登入用）
        public void ConnectToServer()
        {
            if (client != null)
            {
                try { stream?.Close(); } catch { }
                try { client.Close(); } catch { }
                client = null;
                stream = null;
            }
            string ip = textBox1.Text.Trim();
            int port = int.Parse(textBox2.Text.Trim());
            client = new TcpClient(ip, port);
            stream = client.GetStream();
        }
        private void MenuItemSign_Click(object sender, EventArgs e)
        {
            using (var form = new FormUser(this))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 取得 FormUser 的使用者名稱，顯示到 label4
                    label4.Text = form.UserName;
                    label3.Text = loginMarqueeText;
                    label3.BackColor = Color.LightGreen;
                    loginMarqueeTimer.Start(); // 啟動跑馬燈
                }
            }
        }
        private void MenuItemLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    Byte[] data = Encoding.UTF8.GetBytes("disconnect");
                    stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    client.Close();
                }
                client = null;
                stream = null;
                currentUser = "";
                if (receiveThread != null && receiveThread.IsAlive)
                {
                    try { receiveThread.Interrupt(); } catch { }
                    try { receiveThread.Join(100); } catch { }
                    receiveThread = null;
                }
                label4.Text = "";
                MessageBox.Show("已登出");
                // 停止跑馬燈
                loginMarqueeTimer.Stop();
                label3.BackColor = Color.LightGray;
                label3.Text = "已登出";
            }
            catch { }
        }
        private void MenuItemTxt_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected) return;
            Shared.FileTransferHelper.SendFileWithDialog(client.GetStream(), "文字檔|*.txt", "TEXT", currentUser, msg => richTextBox1.AppendText(msg));
        }
        private void MenuItemPic_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected) return;
            Shared.FileTransferHelper.SendFileWithDialog(client.GetStream(), "圖片檔|*.jpg;*.jpeg;*.png;*.bmp;*.gif", "IMAGE", currentUser, msg => richTextBox1.AppendText(msg));
        }
        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            using (var confirm = new FormExitConfirm())
            {
                if (confirm.ShowDialog() == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
    }
}

