namespace TCP_server
{
    partial class FormOpenServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label2 = new Label();
            label1 = new Label();
            btnCheck = new Button();
            btnCancel = new Button();
            buttonGetIP_Click = new Button();
            SuspendLayout();
            // 
            // textBox2
            // 
            textBox2.Location = new Point(119, 80);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 7;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(119, 40);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 83);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 5;
            label2.Text = "Port :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 44);
            label1.Name = "label1";
            label1.Size = new Size(23, 15);
            label1.TabIndex = 4;
            label1.Text = "IP :";
            // 
            // btnCheck
            // 
            btnCheck.Location = new Point(237, 39);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(75, 23);
            btnCheck.TabIndex = 8;
            btnCheck.Text = "確認";
            btnCheck.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(237, 79);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // buttonGetIP_Click
            // 
            buttonGetIP_Click.Location = new Point(119, 132);
            buttonGetIP_Click.Name = "buttonGetIP_Click";
            buttonGetIP_Click.Size = new Size(193, 23);
            buttonGetIP_Click.TabIndex = 10;
            buttonGetIP_Click.Text = "取得當地IP";
            buttonGetIP_Click.UseVisualStyleBackColor = true;
            buttonGetIP_Click.Click += buttonGetIP_Click_Click;
            // 
            // FormOpenServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(371, 214);
            Controls.Add(buttonGetIP_Click);
            Controls.Add(btnCancel);
            Controls.Add(btnCheck);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "FormOpenServer";
            Text = "FormOpenServer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox2;
        private TextBox textBox1;
        private Label label2;
        private Label label1;
        private Button btnCheck;
        private Button btnCancel;
        private Button buttonGetIP_Click;
    }
}