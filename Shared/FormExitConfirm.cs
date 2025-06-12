using System;
using System.Windows.Forms;

namespace Shared
{
    public class FormExitConfirm : Form
    {
        public FormExitConfirm()
        {
            this.Text = "是否關閉程式";
            Button btnYes = new Button() { Text = "是", DialogResult = DialogResult.Yes, Left = 50, Width = 60, Top = 30 };
            Button btnNo = new Button() { Text = "否", DialogResult = DialogResult.No, Left = 150, Width = 60, Top = 30 };
            this.Controls.Add(btnYes);
            this.Controls.Add(btnNo);
            this.AcceptButton = btnYes;
            this.CancelButton = btnNo;
            this.ClientSize = new System.Drawing.Size(250, 80);
            btnYes.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
            btnNo.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };
        }
    }
}