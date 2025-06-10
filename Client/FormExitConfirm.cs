using System;
using System.Windows.Forms;

namespace Client
{
    public class FormExitConfirm : Form
    {
        public FormExitConfirm()
        {
            this.Text = "�O�_�����{��";
            Button btnYes = new Button() { Text = "�O", DialogResult = DialogResult.Yes, Left = 30, Width = 60, Top = 30 };
            Button btnNo = new Button() { Text = "�_", DialogResult = DialogResult.No, Left = 110, Width = 60, Top = 30 };
            this.Controls.Add(btnYes);
            this.Controls.Add(btnNo);
            this.AcceptButton = btnYes;
            this.CancelButton = btnNo;
            this.ClientSize = new System.Drawing.Size(200, 80);
            btnYes.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
            btnNo.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };
        }
    }
}