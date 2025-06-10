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
        private string loginMarqueeText = "�n�J���\     ";

        public TCP_Client()
        {
            InitializeComponent();
            textBox1.Text = "127.0.0.1";
            textBox2.Text = "8888";
            button3.Click += button3_Click; // �ǰe�T��
            button4.Click += button4_Click; // �M���T��
            button7.Click += button7_Click; // �����s
            MenuItemExit.Click += MenuItemExit_Click;
            richTextBox1.Font = new Font("Segoe UI Emoji", 12);
            btnHi.Click += (s, e) => SendTextMessage("Hi");
            btnBye.Click += (s, e) => SendTextMessage("Bye");
            btnOMG.Click += (s, e) => SendTextMessage("OMG");
            //�]���O
            loginMarqueeTimer = new System.Windows.Forms.Timer();//�ޥέp�ɾ�
            loginMarqueeTimer.Interval = 200;
            loginMarqueeTimer.Tick += LoginMarqueeTimer_Tick;
        }
        // ���U��k�G�ھ��ɮ��������o����y�z
        private string GetFileTypeText(string fileType)
        {
            switch (fileType)
            {
                case "TEXT": return "��r��";
                case "IMAGE": return "�Ϥ���";
                case "FILE": return "�ɮ�";
                default: return "�ɮ�";
            }
        }
        // �Ұʱ��������
        public void StartReceive()
        {
            receiveThread = new Thread(ReceiveMessage);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        private void LoginMarqueeTimer_Tick(object sender, EventArgs e)
        {
            // �����r�G�����Ĥ@�Ӧr�A�[��᭱
            loginMarqueeText = loginMarqueeText.Substring(1) + loginMarqueeText[0];
            label3.Text = loginMarqueeText;
        }        
        public void ReceiveMessage()
        {
            try
            {
                while (client != null && client.Connected)
                {
                    // 1. �v�r��Ū header ���� \n
                    StringBuilder headerBuilder = new StringBuilder();
                    while (true)
                    {
                        int b = stream.ReadByte();
                        if (b == -1) return; // �s�u���_
                        if (b == '\n') break; // Ū���촫��šA��� header ����
                        headerBuilder.Append((char)b);
                    }
                    string header = headerBuilder.ToString();

                    // 2. �P�_��w���O
                    if (header.StartsWith("MSGINFO|"))
                    {
                        var msgInfo = MessageInfo.Parse(header.Substring(8));
                        if (lastSentMessageId.HasValue && msgInfo.MessageId == lastSentMessageId.Value)
                        {
                            continue; // ���L�ۤv��e�X���T��
                        }

                        if (msgInfo.Type == "IMAGE")
                        {
                            // ��ܹϤ�
                            try
                            {
                                // �T�{ FileContent �����e
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

                                    string display = $"[{msgInfo.Sender} {msgInfo.Time}]�G�ǰe�F�@�i�Ϥ� ({msgInfo.FileName})\n";
                                    if (richTextBox1.InvokeRequired)
                                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                                    else
                                        richTextBox1.AppendText(display);

                                    // �i�H�[�W�����T��
                                    Console.WriteLine($"��ܹϤ����\: {msgInfo.FileName}, �j�p: {msgInfo.FileContent.Length} bytes");

                                    // �Y���ɦW�A�߰ݬO�_�x�s
                                    if (!string.IsNullOrEmpty(msgInfo.FileName))
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            using (SaveFileDialog sfd = new SaveFileDialog())
                                            {
                                                sfd.FileName = msgInfo.FileName;
                                                sfd.Filter = "�Ϥ���|*.jpg;*.jpeg;*.png;*.bmp;*.gif|�Ҧ��ɮ�|*.*";
                                                if (sfd.ShowDialog() == DialogResult.OK)
                                                {
                                                    try
                                                    {
                                                        File.WriteAllBytes(sfd.FileName, msgInfo.FileContent);
                                                        MessageBox.Show($"�Ϥ��w���\�x�s��: {sfd.FileName}", "�x�s���\", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show($"�x�s�Ϥ��ɵo�Ϳ��~: {ex.Message}", "�x�s����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    }
                                                }
                                            }
                                        }));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"�Ϥ����e����: {msgInfo.FileName}");
                                    string display = $"[{msgInfo.Sender} {msgInfo.Time}]�G�ǰe�F�@�i�Ϥ��A�����e����\n";
                                    if (richTextBox1.InvokeRequired)
                                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                                    else
                                        richTextBox1.AppendText(display);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"��ܹϤ��ɵo�Ϳ��~: {ex.Message}");
                                string display = $"[{msgInfo.Sender} {msgInfo.Time}]�G�ǰe�F�@�i�Ϥ��A����ܥ��� ({ex.Message})\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                                else
                                    richTextBox1.AppendText(display);
                            }
                        }
                        else if (msgInfo.Type == "TEXT")
                        {
                            string display = $"[{msgInfo.Sender} {msgInfo.Time}]�G{msgInfo.Content}\n";
                            if (richTextBox1.InvokeRequired)
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(display)));
                            else
                                richTextBox1.AppendText(display);

                            // �Y���ɦW�]�¤�r�ɡ^�A�߰ݬO�_�x�s
                            if (!string.IsNullOrEmpty(msgInfo.FileName))
                            {
                                this.Invoke(new Action(() =>
                                {
                                    using (SaveFileDialog sfd = new SaveFileDialog())
                                    {
                                        sfd.FileName = msgInfo.FileName;
                                        sfd.Filter = "��r��|*.txt|�Ҧ��ɮ�|*.*";
                                        if (sfd.ShowDialog() == DialogResult.OK)
                                        {
                                            try
                                            {
                                                File.WriteAllText(sfd.FileName, msgInfo.Content, Encoding.UTF8);
                                                MessageBox.Show($"��r�ɤw���\�x�s��: {sfd.FileName}", "�x�s���\", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show($"�x�s��r�ɮɵo�Ϳ��~: {ex.Message}", "�x�s����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                }));
                            }
                        }
                        else if (msgInfo.Type == "FILE")
                        {
                            // �B�z�@���ɮ�
                            string display = $"[{msgInfo.Sender} {msgInfo.Time}]�G�ǰe�F�@���ɮ� ({msgInfo.FileName})\n";
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
                                        sfd.Filter = "�Ҧ��ɮ�|*.*";
                                        if (sfd.ShowDialog() == DialogResult.OK)
                                        {
                                            try
                                            {
                                                File.WriteAllBytes(sfd.FileName, msgInfo.FileContent);
                                                MessageBox.Show($"�ɮפw���\�x�s��: {sfd.FileName}", "�x�s���\", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show($"�x�s�ɮ׮ɵo�Ϳ��~: {ex.Message}", "�x�s����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                }));
                            }
                        }
                    }
                    else if (header.StartsWith("TEXT|") || header.StartsWith("IMAGE|") || header.StartsWith("FILE|"))
                    {
                        // �ѪR header
                        string[] parts = header.Split('|');
                        if (parts.Length < 3) continue;
                        string fileType = parts[0];
                        string fileName = parts[1];
                        if (!int.TryParse(parts[2], out int fileLen) || fileLen <= 0) continue;

                        // 3. Ū���ɮפ��e
                        byte[] fileBuffer = new byte[fileLen];
                        int totalRead = 0;
                        int readAttempts = 0;
                        const int maxReadAttempts = 100; // �]�w�̤jŪ�����զ���

                        while (totalRead < fileLen && readAttempts < maxReadAttempts)
                        {
                            int read = stream.Read(fileBuffer, totalRead, fileLen - totalRead);
                            if (read == 0)
                            {
                                readAttempts++;
                                System.Threading.Thread.Sleep(10); // �y�L�Ȱ��@�U�A��
                                continue;
                            }
                            totalRead += read;
                            readAttempts = 0; // ���\Ū���A���m���խp��
                        }

                        if (totalRead < fileLen)
                        {
                            string errorMsg = $"[���~] �ɮױ���������: {fileName}, �ȱ��� {totalRead}/{fileLen} bytes\n";
                            if (richTextBox1.InvokeRequired)
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(errorMsg)));
                            else
                                richTextBox1.AppendText(errorMsg);
                            continue;
                        }

                        // �Y���Ϥ��A���������
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

                                // ��ܱ����T��
                                string displayMsg = $"[���A���ǰe�Ϥ�] {fileName} ({fileBuffer.Length} bytes)\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(displayMsg)));
                                else
                                    richTextBox1.AppendText(displayMsg);
                            }
                            catch (Exception ex)
                            {
                                string errorMsg = $"[���~] ��ܹϤ�����: {ex.Message}\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(errorMsg)));
                                else
                                    richTextBox1.AppendText(errorMsg);
                            }
                        }

                        // 4. �߰ݬO�_�x�s�ɮ�
                        string savePath = null;
                        this.Invoke(new Action(() =>
                        {
                            using (SaveFileDialog sfd = new SaveFileDialog())
                            {
                                sfd.FileName = fileName;
                                switch (fileType)
                                {
                                    case "TEXT":
                                        sfd.Filter = "��r��|*.txt|�Ҧ��ɮ�|*.*";
                                        break;
                                    case "IMAGE":
                                        sfd.Filter = "�Ϥ���|*.jpg;*.jpeg;*.png;*.bmp;*.gif|�Ҧ��ɮ�|*.*";
                                        break;
                                    default:
                                        sfd.Filter = "�Ҧ��ɮ�|*.*";
                                        break;
                                }

                                if (sfd.ShowDialog() == DialogResult.OK)
                                {
                                    savePath = sfd.FileName;
                                }
                            }
                        }));

                        // 5. �x�s�ɮר���ܰT��
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        if (!string.IsNullOrEmpty(savePath))
                        {
                            try
                            {
                                File.WriteAllBytes(savePath, fileBuffer);
                                string msg = $"[���A���� {time} �ǰe{GetFileTypeText(fileType)}]�G{fileName} �w�x�s�� {savePath}\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(msg)));
                                else
                                    richTextBox1.AppendText(msg);

                                // �Y���Ϥ��B�|����ܡA�h���
                                if (fileType == "IMAGE" && pictureBox1.Image == null)
                                {
                                    pictureBox1.Image = Image.FromFile(savePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                string errorMsg = $"[���~] �x�s�ɮץ���: {ex.Message}\n";
                                if (richTextBox1.InvokeRequired)
                                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(errorMsg)));
                                else
                                    richTextBox1.AppendText(errorMsg);
                            }
                        }
                        else
                        {
                            string msg = $"[���A���� {time}]�G����{GetFileTypeText(fileType)} {fileName}�A�����x�s�C\n";
                            if (richTextBox1.InvokeRequired)
                                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(msg)));
                            else
                                richTextBox1.AppendText(msg);
                        }
                    }
                    else
                    {
                        // ��L�����T��
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        string displayMsg = $"[���A���T�� {time}]�G{header}\n";
                        if (richTextBox1.InvokeRequired)
                            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(displayMsg)));
                        else
                            richTextBox1.AppendText(displayMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                // �s�u���_�Ψ�L�ҥ~���p
                Console.WriteLine($"�����T���ɵo�Ϳ��~: {ex.Message}");

                // �i�H��ܦb UI ��ܳs�u���_�T��
                try
                {
                    if (richTextBox1.InvokeRequired)
                        richTextBox1.Invoke(new Action(() => richTextBox1.AppendText($"[�t��] �s�u�w���_: {ex.Message}\n")));
                    else
                        richTextBox1.AppendText($"[�t��] �s�u�w���_: {ex.Message}\n");
                }
                catch { /* �����b UI ��s�ɥi��o�ͪ��ҥ~ */ }
            }
            finally
            {
                // �T�O�b���������������귽
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
                    catch { /* ���������s�u�ɥi��o�ͪ��ҥ~ */ }
                }
            }
        }        
        // �ǰe�T�����s�ƥ�
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

            // �O���̫�e�X���T��ID
            lastSentMessageId = msgInfo.MessageId;

            // �����c��MSGINFO�r��õo�e�A���ϥ�FileTransferHelper.SendMessage
            try
            {
                string line = "MSGINFO|" + msgInfo.ToString() + "\n";
                byte[] data = Encoding.UTF8.GetBytes(line);

                // �T�O�ۤv�]��ݨ�T��
                string selfDisplay = $"[{currentUser} {msgInfo.Time}]�G{msgInfo.Content}\n";
                richTextBox1.AppendText(selfDisplay);

                // �o�e����A��
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�ǰe�T������: {ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // �ǰe�T���]�temoji�^
        private void SendTextMessage(string message)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                MessageBox.Show("�Х��n�J�I");
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

            // �O���̫�e�X���T��ID
            lastSentMessageId = msgInfo.MessageId;

            try
            {
                // �����c��MSGINFO�r��õo�e
                string line = "MSGINFO|" + msgInfo.ToString() + "\n";
                byte[] data = Encoding.UTF8.GetBytes(line);

                // �T�O�ۤv�]��ݨ�T��
                string selfDisplay = $"[{currentUser} {msgInfo.Time}]�G{message}\n";
                richTextBox1.AppendText(selfDisplay);

                // �o�e����A��
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�ǰe�T������: {ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // �M���T����ܰ�
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
        // �����
        private void button7_Click(object sender, EventArgs e)
        {
            using (var picker = new EmojiPickerForm())
            {
                if (picker.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(picker.SelectedEmoji))
                {
                    // �����ǰeemoji�T��
                    int selectionIndex = textBox3.SelectionStart;
                    textBox3.Text = textBox3.Text.Insert(selectionIndex, picker.SelectedEmoji);
                    textBox3.SelectionStart = selectionIndex + picker.SelectedEmoji.Length;
                    textBox3.Focus();
                }
            }
        }
        // �s�u����A���]���U/�n�J�Ρ^
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
                    // ���o FormUser ���ϥΪ̦W�١A��ܨ� label4
                    label4.Text = form.UserName;
                    label3.Text = loginMarqueeText;
                    label3.BackColor = Color.LightGreen;
                    loginMarqueeTimer.Start(); // �Ұʶ]���O
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
                MessageBox.Show("�w�n�X");
                // ����]���O
                loginMarqueeTimer.Stop();
                label3.BackColor = Color.LightGray;
                label3.Text = "�w�n�X";
            }
            catch { }
        }
        private void MenuItemTxt_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected) return;
            Shared.FileTransferHelper.SendFileWithDialog(client.GetStream(), "��r��|*.txt", "TEXT", currentUser, msg => richTextBox1.AppendText(msg));
        }
        private void MenuItemPic_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected) return;
            Shared.FileTransferHelper.SendFileWithDialog(client.GetStream(), "�Ϥ���|*.jpg;*.jpeg;*.png;*.bmp;*.gif", "IMAGE", currentUser, msg => richTextBox1.AppendText(msg));
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

