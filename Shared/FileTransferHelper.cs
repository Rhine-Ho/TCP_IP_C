using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Shared
{
    public static class FileTransferHelper
    {
        // �ǰe�T���]MSGINFO��w�^
        public static void SendMessage(NetworkStream stream, MessageInfo msg)
        {
            if (!msg.IsValid())
            {
                MessageBox.Show("�L�Ī��T�����e", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string line = "MSGINFO|" + msg.ToString() + "\n";
            byte[] data = Encoding.UTF8.GetBytes(line);
            stream.Write(data, 0, data.Length);
        }
        // �ǰe�ɮס]MSGINFO��w�^- �ץ�����
        public static void SendFile(NetworkStream stream, string filePath, string type, string sender)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"�䤣���ɮ�: {filePath}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fileName = Path.GetFileName(filePath);
                byte[] fileBytes = File.ReadAllBytes(filePath);

                if (fileBytes.Length == 0)
                {
                    MessageBox.Show($"�ɮפ��e����: {fileName}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // �إ� MessageInfo ����
                var msgInfo = new MessageInfo
                {
                    Sender = sender,
                    Type = type,
                    FileName = fileName,
                    Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                };

                // ����ק�G�ھ��������T�]�m Content �� FileContent
                if (type == "TEXT")
                {
                    // ����r�ɡA����Ū������r
                    try
                    {
                        msgInfo.Content = File.ReadAllText(filePath, Encoding.UTF8);
                    }
                    catch
                    {
                        // �p�G�L�kŪ������r�A�N�ϥ� FileContent
                        msgInfo.FileContent = fileBytes;
                    }
                }
                else // IMAGE �Ψ�L����
                {
                    // ����ק�G�����ϥ� FileContent �s�x��l�ɮפ��e
                    msgInfo.FileContent = fileBytes;
                    msgInfo.Content = null; // �T�O Content �� null
                }

                // �ǰe�T��
                SendMessage(stream, msgInfo);

                // �����T��
                Console.WriteLine($"�w�ǰe�ɮ�: {fileName}, ����: {type}, �j�p: {fileBytes.Length} bytes");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�ǰe�ɮ׿��~: {ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Client/Server�ݿ���ɮרöǰe�]for MenuItemTxt/MenuItemPic�^
        public static void SendFileWithDialog(NetworkStream stream, string filter, string type, string sender, Action<string> logAction = null)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = filter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SendFile(stream, ofd.FileName, type, sender);
                        logAction?.Invoke($"[�� {DateTime.Now:yyyy/MM/dd HH:mm:ss}]�G�w�ǰe{(type == "TEXT" ? "��r��" : "�Ϥ���")} {Path.GetFileName(ofd.FileName)}\n");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"�ǰe�ɮץ���: {ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logAction?.Invoke($"[�� {DateTime.Now:yyyy/MM/dd HH:mm:ss}]�G�ǰe{(type == "TEXT" ? "��r��" : "�Ϥ���")}���� - {ex.Message}\n");
                    }
                }
            }
        }
    }
}
