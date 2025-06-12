using System;
using System.Windows.Forms;

namespace Server
{
    public partial class FormExitConfirm : Form
    {
        public FormExitConfirm()
        {
            //InitializeComponent();
            this.Text = "�O�_�������A��";
            Button btnYes = new Button() { Text = "�O", DialogResult = DialogResult.Yes, Left = 60, Width = 60, Top = 30 };
            Button btnNo = new Button() { Text = "�_", DialogResult = DialogResult.No, Left = 180, Width = 60, Top = 30 };
            this.Controls.Add(btnYes);
            this.Controls.Add(btnNo);
            this.AcceptButton = btnYes;
            this.CancelButton = btnNo;
            this.ClientSize = new System.Drawing.Size(280, 80);
            btnYes.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
            btnNo.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };
        }
    }
}
