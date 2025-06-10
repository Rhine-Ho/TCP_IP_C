using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Shared
{
    public static class FileTransferHelper
    {
        // 傳送訊息（MSGINFO協定）
        public static void SendMessage(NetworkStream stream, MessageInfo msg)
        {
            if (!msg.IsValid())
            {
                MessageBox.Show("無效的訊息內容", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string line = "MSGINFO|" + msg.ToString() + "\n";
            byte[] data = Encoding.UTF8.GetBytes(line);
            stream.Write(data, 0, data.Length);
        }
        // 傳送檔案（MSGINFO協定）- 修正版本
        public static void SendFile(NetworkStream stream, string filePath, string type, string sender)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"找不到檔案: {filePath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fileName = Path.GetFileName(filePath);
                byte[] fileBytes = File.ReadAllBytes(filePath);

                if (fileBytes.Length == 0)
                {
                    MessageBox.Show($"檔案內容為空: {fileName}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 建立 MessageInfo 物件
                var msgInfo = new MessageInfo
                {
                    Sender = sender,
                    Type = type,
                    FileName = fileName,
                    Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                };

                // 關鍵修改：根據類型正確設置 Content 或 FileContent
                if (type == "TEXT")
                {
                    // 對於文字檔，嘗試讀取為文字
                    try
                    {
                        msgInfo.Content = File.ReadAllText(filePath, Encoding.UTF8);
                    }
                    catch
                    {
                        // 如果無法讀取為文字，就使用 FileContent
                        msgInfo.FileContent = fileBytes;
                    }
                }
                else // IMAGE 或其他類型
                {
                    // 關鍵修改：直接使用 FileContent 存儲原始檔案內容
                    msgInfo.FileContent = fileBytes;
                    msgInfo.Content = null; // 確保 Content 為 null
                }

                // 傳送訊息
                SendMessage(stream, msgInfo);

                // 除錯訊息
                Console.WriteLine($"已傳送檔案: {fileName}, 類型: {type}, 大小: {fileBytes.Length} bytes");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"傳送檔案錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Client/Server端選擇檔案並傳送（for MenuItemTxt/MenuItemPic）
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
                        logAction?.Invoke($"[於 {DateTime.Now:yyyy/MM/dd HH:mm:ss}]：已傳送{(type == "TEXT" ? "文字檔" : "圖片檔")} {Path.GetFileName(ofd.FileName)}\n");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"傳送檔案失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logAction?.Invoke($"[於 {DateTime.Now:yyyy/MM/dd HH:mm:ss}]：傳送{(type == "TEXT" ? "文字檔" : "圖片檔")}失敗 - {ex.Message}\n");
                    }
                }
            }
        }
    }
}
