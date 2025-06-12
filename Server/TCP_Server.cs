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
using System.Threading;
using TCP_server;
using Shared;

namespace Server
{
    public partial class TCP_Server : Form
    {
        private volatile bool serverRunning = true;
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private object clientLock = new object();
        private Thread acceptThread;
        private Dictionary<string, string> userDb = new Dictionary<string, string>();
        private readonly string userDbFile = "users.txt";
        private Dictionary<string, TcpClient> loggedInUsers = new Dictionary<string, TcpClient>();
        private object loginLock = new object();
        private Guid? lastSentMessageId = null;
        private System.Windows.Forms.Timer marqueeTimer;
        private string marqueeText = "伺服器已啟動     "; // 建議加空格讓滾動更順暢


        public TCP_Server()
        {
            InitializeComponent();
            textBox1.Text = "127.0.0.1";
            textBox2.Text = "8888";
            button3.Click += button3_Click;
            button4.Click += button4_Click;
            button7.Click += button7_Click;
            MenuItemExit.Click += MenuItemExit_Click;
            richTextBox1.Font = new Font("Segoe UI Emoji", 12);
            btnHi.Click += (s, e) => SendTextMessage("Hi");
            btnBye.Click += (s, e) => SendTextMessage("Bye");
            btnOMG.Click += (s, e) => SendTextMessage("OMG");
        }
        private void AcceptClients()
        {
            while (serverRunning)
            {
                try
                {
                    var client = server.AcceptTcpClient();
                    lock (clientLock)
                    {
                        clients.Add(client);
                    }
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch (SocketException)
                {
                    // 伺服器關閉時會拋出例外，這裡可以忽略
                    break;
                }
            }
        }
        private void MarqueeTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(label3.Text))
            {
                label3.Text = label3.Text.Substring(1) + label3.Text[0];
            }
        }
        private void StartServer()
        {
            string ip = textBox1.Text.Trim();
            int port = 8888;
            if (!string.IsNullOrWhiteSpace(textBox2.Text) && int.TryParse(textBox2.Text.Trim(), out int userPort))
                port = userPort;

            server = new TcpListener(IPAddress.Parse(ip), port);
            try
            {
                server.Start();
                serverRunning = true;

                acceptThread = new Thread(AcceptClients);
                acceptThread.IsBackground = true;
                acceptThread.Start();

                label3.BackColor = Color.LightGreen;
                label3.Text = marqueeText;

                // ✅ 初始化跑馬燈 Timer
                if (marqueeTimer == null)
                {
                    marqueeTimer = new System.Windows.Forms.Timer();
                    marqueeTimer.Interval = 200; // 控制滾動速度 (ms)
                    marqueeTimer.Tick += MarqueeTimer_Tick;
                }
                marqueeTimer.Start();
            }
            catch (SocketException e)
            {
                MessageBox.Show("SocketException: " + e.Message);
            }
        }

        private void AppendServerMessageInfo(MessageInfo msg)
        {
            string display;

            if (msg.Type == "TEXT")
            {
                display = $"[{msg.Sender} {msg.Time}]：{msg.Content}\n";
            }
            else if (msg.Type == "IMAGE")
            {
                display = $"[{msg.Sender} {msg.Time}]：傳送了一張圖片";
                if (!string.IsNullOrEmpty(msg.FileName))
                    display += $" ({msg.FileName})";
                display += "\n";

                // 如果是圖片，顯示在 pictureBox1 上
                if (msg.FileContent != null && msg.FileContent.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(msg.FileContent))
                        {
                            Image image = Image.FromStream(ms);
                            if (pictureBox1.InvokeRequired)
                            {
                                pictureBox1.Invoke(new Action(() => pictureBox1.Image = image));
                            }
                            else
                            {
                                pictureBox1.Image = image;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // 圖片載入失敗時的處理
                        Console.WriteLine($"圖片載入失敗: {ex.Message}");
                        display += $"[圖片載入失敗: {ex.Message}]\n";
                    }
                }
                else
                {
                    display += "[圖片內容為空]\n";
                }
            }
            else // 其他類型
            {
                display = $"[{msg.Sender} {msg.Time}]：傳送了一個檔案";
                if (!string.IsNullOrEmpty(msg.FileName))
                    display += $" ({msg.FileName})";
                display += "\n";
            }

            if (richTextBox1.InvokeRequired)
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
            else
                richTextBox1.AppendText(display);
        }
        private void SendTextMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var msgInfo = new MessageInfo
            {
                Sender = "Server",
                Type = "TEXT",
                Content = message,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            AppendServerMessageInfo(msgInfo);
            BroadcastMessageInfo(msgInfo);
        }
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            string currentUser = null;
            try
            {
                // 1. 先載入使用者資料庫
                LoadUserDb();

                // 2. 先處理登入/註冊
                byte[] buffer = new byte[1024];
                int len = stream.Read(buffer, 0, buffer.Length);
                if (len <= 0) return;
                string loginMsg = Encoding.UTF8.GetString(buffer, 0, len).Trim();
                var parts = loginMsg.Split('|');
                if (parts.Length < 3)
                {
                    stream.Write(Encoding.UTF8.GetBytes("LOGIN_FAIL"), 0, Encoding.UTF8.GetByteCount("LOGIN_FAIL"));
                    return;
                }

                if (parts[0] == "REGISTER")
                {
                    bool ok;
                    lock (loginLock)
                    {
                        ok = RegisterUser(parts[1], parts[2]);
                        if (ok && !loggedInUsers.ContainsKey(parts[1]))
                        {
                            loggedInUsers[parts[1]] = client;
                        }
                    }
                    string reply = ok ? "REGISTER_OK" : "REGISTER_DUP";
                    stream.Write(Encoding.UTF8.GetBytes(reply), 0, Encoding.UTF8.GetByteCount(reply));
                    if (!ok) return;
                    currentUser = parts[1];
                }
                else if (parts[0] == "LOGIN")
                {
                    lock (loginLock)
                    {
                        if (userDb.TryGetValue(parts[1], out string pwd) && pwd == parts[2])
                        {
                            if (loggedInUsers.ContainsKey(parts[1]))
                            {
                                string reply = "LOGIN_DUP";
                                stream.Write(Encoding.UTF8.GetBytes(reply), 0, Encoding.UTF8.GetByteCount(reply));
                                return;
                            }
                            else
                            {
                                string reply = "LOGIN_OK";
                                stream.Write(Encoding.UTF8.GetBytes(reply), 0, Encoding.UTF8.GetByteCount(reply));
                                currentUser = parts[1];
                                loggedInUsers[currentUser] = client;
                            }
                        }
                        else
                        {
                            string reply = "LOGIN_FAIL";
                            stream.Write(Encoding.UTF8.GetBytes(reply), 0, Encoding.UTF8.GetByteCount(reply));
                            return;
                        }
                    }
                }
                else
                {
                    stream.Write(Encoding.UTF8.GetBytes("LOGIN_FAIL"), 0, Encoding.UTF8.GetByteCount("LOGIN_FAIL"));
                    return;
                }

                // 3. 登入成功後，進入聊天室訊息/檔案處理迴圈
                while (serverRunning)
                {
                    // 先讀header直到遇到\n
                    StringBuilder headerBuilder = new StringBuilder();
                    while (true)
                    {
                        int b = stream.ReadByte();
                        if (b == -1) return;
                        if (b == '\n') break;
                        headerBuilder.Append((char)b);
                    }
                    string header = headerBuilder.ToString();

                    if (header.StartsWith("MSGINFO|"))
                    {
                        var msgInfo = MessageInfo.Parse(header.Substring(8));
                        AppendServerMessageInfo(msgInfo);
                        BroadcastMessageInfo(msgInfo);
                    }
                    else if (header.StartsWith("TEXT|") || header.StartsWith("IMAGE|"))
                    {
                        string[] hparts = header.Split('|');
                        if (hparts.Length < 3) continue;
                        string fileType = hparts[0];
                        string fileName = hparts[1];
                        if (!int.TryParse(hparts[2], out int fileLen) || fileLen <= 0) continue;

                        // 讀取檔案內容
                        byte[] fileBuffer = new byte[fileLen];
                        int totalRead = 0;
                        while (totalRead < fileLen)
                        {
                            int read = stream.Read(fileBuffer, totalRead, fileLen - totalRead);
                            if (read == 0) break;
                            totalRead += read;
                        }

                        // 廣播給所有Client（用MessageInfo格式，Content為Base64）
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        var msgInfo = new MessageInfo
                        {
                            Sender = currentUser,
                            Type = fileType,
                            FileContent = fileBuffer,
                            FileName = fileName,
                            Time = time
                        };
                        AppendServerMessageInfo(msgInfo);
                        BroadcastMessageInfo(msgInfo);
                    }
                    else
                    {
                        // 其他訊息（含表情）
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        var msgInfo = new MessageInfo
                        {
                            Sender = currentUser,
                            Type = "TEXT",
                            Content = header,
                            Time = time
                        };
                        AppendServerMessageInfo(msgInfo);
                        BroadcastMessageInfo(msgInfo);

                        if (header.Trim() == "disconnect")
                            break;
                    }
                }
            }
            catch { }
            finally
            {
                lock (clientLock)
                {
                    clients.Remove(client);
                }
                lock (loginLock)
                {
                    if (!string.IsNullOrEmpty(currentUser) && loggedInUsers.ContainsKey(currentUser) && loggedInUsers[currentUser] == client)
                        loggedInUsers.Remove(currentUser);
                }
                try { stream.Close(); } catch { }
                try { client.Close(); } catch { }
                string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                AppendServerMessage($"[系統 {time}]：有 Client 離線\n");
                if (clients.Count == 0)
                {
                    //label2.Invoke(new Action(() => label2.Text = "disconnected!"));
                }
                else
                {
                    //richTextBox2.Text = string.Join(Environment.NewLine, loggedInUsers.Keys);
                }
            }
        }

        private void BroadcastMessageInfo(MessageInfo msg)
        {
            string line = msg.ToString();
            byte[] data = Encoding.UTF8.GetBytes("MSGINFO|" + line + "\n");
            lock (clientLock)
            {
                foreach (var c in clients.ToList())
                {
                    if (c.Connected)
                    {
                        try { c.GetStream().Write(data, 0, data.Length); } catch { }
                    }
                }
            }
        }
        //private void BroadcastRaw(byte[] data, TcpClient fromClient)
        //{
        //    lock (clientLock)
        //    {
        //        foreach (var c in clients.ToList())
        //        {
        //            if (c.Connected && c != fromClient)
        //            {
        //                try
        //                {
        //                    c.GetStream().Write(data, 0, data.Length);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}
        private void button3_Click(object sender, EventArgs e)
        {
            string message = textBox3.Text;
            if (string.IsNullOrWhiteSpace(message))
                return;

            var msgInfo = new MessageInfo
            {
                Sender = "Server",
                Type = "TEXT",
                Content = message,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            lastSentMessageId = msgInfo.MessageId;
            AppendServerMessageInfo(msgInfo);
            BroadcastMessageInfo(msgInfo);
            textBox3.Clear();
        }
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
        private void button7_Click(object sender, EventArgs e)
        {
            using (var picker = new EmojiPickerForm())
            {
                if (picker.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(picker.SelectedEmoji))
                {
                    int selectionIndex = textBox3.SelectionStart;
                    textBox3.Text = textBox3.Text.Insert(selectionIndex, picker.SelectedEmoji);
                    textBox3.SelectionStart = selectionIndex + picker.SelectedEmoji.Length;
                    textBox3.Focus();
                }
            }
        }
        private void AppendServerMessage(string msg)
        {
            //if (msg.Sender == "Server")
            //    return;
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(msg)));
            }
            else
            {
                richTextBox1.AppendText(msg);
            }
        }
        private void LoadUserDb()
        {
            userDb.Clear();
            if (File.Exists(userDbFile))
            {
                foreach (var line in File.ReadAllLines(userDbFile))
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 2)
                        userDb[parts[0]] = parts[1];
                }
            }
        }
        private bool RegisterUser(string username, string password)
        {
            if (userDb.ContainsKey(username) || loggedInUsers.ContainsKey(username)) return false;
            string regTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            userDb[username] = password;
            //File.AppendAllText(userDbFile, $"{username}|{password}|{regTime}\n");
            File.AppendAllText(userDbFile, $"{username}|{password}|{regTime}\n");
            return true;
        }
        private void MenuItemRegister_Click(object sender, EventArgs e)
        {
            //模態顯示:子表單關閉才可以點選主表單
            //using (var form = new FormUserList(userDbFile, new HashSet<string>(loggedInUsers.Keys)))
            //{
            //    //form.ShowDialog();
            //}
            var form = new FormUserList(userDbFile, new HashSet<string>(loggedInUsers.Keys), loggedInUsers);
            form.Show();
        }
        private void MenuItemOpenServer_Click(object sender, EventArgs e)
        {
            using (var form = new FormOpenServer(textBox1.Text, textBox2.Text))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = form.ServerIP;
                    textBox2.Text = form.ServerPort;
                    if (server == null || !serverRunning)
                    {
                        if (serverRunning && server != null)
                            return;
                        StartServer();
                    }
                }
            }
        }
        private void MenuItemCloseServer_Click(object sender, EventArgs e)
        {
            serverRunning = false;
            try
            {
                server?.Stop();
                label3.BackColor = Color.LightGray; // 灰色 = 伺服器未啟動
                label3.Text = $"伺服器未啟動";
            }
            catch { }
            if (acceptThread != null && acceptThread.IsAlive)
            {
                acceptThread.Join(1000); // 最多等1秒
                acceptThread = null;
            }
            lock (clientLock)
            {
                foreach (var c in clients.ToList())
                {
                    try { c.Close(); } catch { }
                }
                clients.Clear();
            }
            //label2.Invoke(new Action(() => label2.Text = "disconnected!"));
            string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            AppendServerMessage($"[系統 {time}]：伺服器已停止\n");
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
        private void MenuItemTxt_Click(object sender, EventArgs e)
        {
            lock (clientLock)
            {
                foreach (var c in clients.ToList())
                {
                    if (c.Connected)
                    {
                        Shared.FileTransferHelper.SendFileWithDialog(c.GetStream(), "文字檔|*.txt", "TEXT", "Server", AppendServerMessage);
                    }
                }
            }
        }
        private void MenuItemPic_Click(object sender, EventArgs e)
        {
            lock (clientLock)
            {
                foreach (var c in clients.ToList())
                {
                    if (c.Connected)
                    {
                        Shared.FileTransferHelper.SendFileWithDialog(c.GetStream(), "圖片檔|*.jpg;*.jpeg;*.png;*.bmp;*.gif", "IMAGE", "Server", AppendServerMessage);
                    }
                }
            }
        }
        private void MenuItemMedia_Click(object sender, EventArgs e)
        {
            lock (clientLock)
            {
                foreach (var c in clients.ToList())
                {
                    if (c.Connected)
                    {
                        Shared.FileTransferHelper.SendFileWithDialog(c.GetStream(), "影音檔|*.mp4;*.mp3;*.avi;*.mov;*.wav;*.wma|所有檔案|*.*", "FILE", "Server", AppendServerMessage);
                    }
                }
            }
        }

        private void buttonGetIP_Click_Click(object sender, EventArgs e)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipList = host.AddressList
                .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(ip => ip.ToString())
                .ToList();

            string port = textBox2.Text.Trim();

            if (ipList.Count > 0)
            {
                // 取第一個非127.0.0.1的IP（通常是區網IP）
                string localIP = ipList.FirstOrDefault(ip => ip != "127.0.0.1") ?? "127.0.0.1";
                textBox1.Text = localIP;
                MessageBox.Show(
                    "本機可用IP:\n" + string.Join("\n", ipList) +
                    $"\n\n目前Port: {port}\n\n" +
                    $"連線範例：{localIP}:{port} 或 127.0.0.1:{port}",
                    "本機IP與Port"
                );
            }
            else
            {
                MessageBox.Show("找不到本機IPv4位址");
            }
        }
    }
}