using System;
using System.Text;

namespace Shared
{
    public class MessageInfo
    {
        public string Sender { get; set; }
        public string Type { get; set; } // "TEXT", "IMAGE", "FILE"
        public string Content { get; set; } // ��r�T��
        public byte[] FileContent { get; set; } // �ɮ�/�Ϥ�����l bytes�]�s�W�^
        public string FileName { get; set; }
        public string Time { get; set; }
        public Guid MessageId { get; set; } = Guid.NewGuid();

        public MessageInfo() { }

        public MessageInfo(string sender, string type, string content = null, byte[] fileContent = null, string fileName = null, string time = null, Guid? messageId = null)
        {
            Sender = sender;
            Type = type;
            Content = content;
            FileContent = fileContent;
            FileName = fileName;
            Time = time ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            MessageId = messageId ?? Guid.NewGuid();
        }

        // �ন�x�s�r��
        public override string ToString()
        {
            string dataPart;
            if (Type == "TEXT")
            {
                dataPart = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Content ?? ""));
            }
            else // IMAGE, FILE
            {
                // FileContent �w�g�O��l�줸�աA������ Base64 �Y�i�A���ݭn�����ഫ
                dataPart = Convert.ToBase64String(FileContent ?? Array.Empty<byte>());
            }
            //�i���`��ܤ���
            return $"{MessageId}|{Sender}|{Type}|{(FileName == null ? "" : Convert.ToBase64String(Encoding.UTF8.GetBytes(FileName)))}|{Time}|{dataPart}";
        }
        // ���x�s�r���٭�
        public static MessageInfo Parse(string line)
        {
            var parts = line.Split('|', 6);
            if (parts.Length < 6)
                throw new FormatException("MessageInfo �榡���~");

            var msg = new MessageInfo
            {
                MessageId = Guid.TryParse(parts[0], out var id) ? id : Guid.NewGuid(),
                Sender = parts[1],
                Type = parts[2],
                //�i���`��ܤ���
                FileName = string.IsNullOrEmpty(parts[3]) ? null : Encoding.UTF8.GetString(Convert.FromBase64String(parts[3])),
                Time = parts[4]
            };

            if (msg.Type == "TEXT")
            {
                msg.Content = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(parts[5]));
                msg.FileContent = null;
            }
            else // IMAGE, FILE
            {
                msg.FileContent = Convert.FromBase64String(parts[5]);
                msg.Content = null;
            }
            return msg;
        }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Sender))
                return false;

            if (string.IsNullOrEmpty(Type))
                return false;

            if (Type == "TEXT" && string.IsNullOrEmpty(Content))
                return false;

            if ((Type == "IMAGE" || Type == "FILE") && (FileContent == null || FileContent.Length == 0))
                return false;

            return true;
        }
    }
}