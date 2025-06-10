using System;
using System.Text;
using System.Windows.Forms;

namespace Shared
{
    public partial class EmojiPickerForm : Form
    {
        public string SelectedEmoji { get; private set; }

        // 常用 emoji，可自行擴充
        private readonly string[] emojis = new string[]
        {
            "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆",
            "😉", "😊", "😋", "😎", "😍", "😘", "🥰", "😗",
            "😙", "😚", "🙂", "🤗", "🤩", "🤔", "🤨", "😐",
            "😑", "😶", "🙄", "😏", "😣", "😥", "😮", "🤐",
            "😯", "😪", "😫", "🥱", "😴", "😌", "😛", "😜",
            "😝", "🤤", "😒", "😓", "😔", "😕", "🙃", "🤑",
            "😲", "☹️", "🙁", "😖", "😞", "😟", "😤", "😢",
            "😭", "😦", "😧", "😨", "😩", "🤯", "😬", "😰",
            "😱", "🥵", "🥶", "😳", "🤪", "😵", "😡", "😠",
            "🤬", "😷", "🤒", "🤕", "🤢", "🤮", "🥴", "😇"
        };

        public EmojiPickerForm()
        {
            this.Text = "選擇表情符號";
            this.Size = new Size(400, 300);

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            foreach (var emoji in emojis)
            {
                var btn = new Button
                {
                    Text = emoji,
                    Font = new Font("Segoe UI Emoji", 18),
                    Width = 48,
                    Height = 48,
                    Margin = new Padding(4)
                };
                btn.Click += (s, e) =>
                {
                    SelectedEmoji = emoji;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
                flow.Controls.Add(btn);
            }
            this.Controls.Add(flow);
        }
    }
}