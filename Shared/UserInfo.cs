using System;

namespace Shared
{
    public class UserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RegisterTime { get; set; }

        public UserInfo() { }

        public UserInfo(string username, string password, string registerTime = null)
        {
            Username = username;
            Password = password;
            RegisterTime = registerTime ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // 轉成儲存字串
        public override string ToString()
        {
            return $"{Username}|{Password}|{RegisterTime}";
        }

        // 由儲存字串還原
        public static UserInfo Parse(string line)
        {
            var parts = line.Split('|');
            if (parts.Length == 3)
                return new UserInfo(parts[0], parts[1], parts[2]);
            if (parts.Length == 2)
                return new UserInfo(parts[0], parts[1], "（無註冊時間）");
            throw new FormatException("UserInfo 格式錯誤");
        }
    }
}
