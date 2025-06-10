using System;
using System.Windows.Forms;

namespace Server
{
    public partial class EmojiPickerForm : Form
    {
        public string SelectedEmoji { get; private set; }

        public EmojiPickerForm()
        {
            //InitializeComponent();
            string[] emojis = { "😀", "😂", "😍", "😎", "😭", "😡", "👍", "🙏", "🎉", "❤️" };
            int x = 10, y = 10;
            foreach (var emoji in emojis)
            {
                var btn = new Button
                {
                    Text = emoji,
                    Width = 40,
                    Height = 40,
                    Left = x,
                    Top = y
                };
                btn.Click += (s, e) =>
                {
                    SelectedEmoji = emoji;
                    DialogResult = DialogResult.OK;
                    Close();
                };
                Controls.Add(btn);
                x += 45;
                if (x > 250) { x = 10; y += 45; }
            }
            this.Width = 300;
            this.Height = 150;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Text = "選擇表情";
        }
    }
}
