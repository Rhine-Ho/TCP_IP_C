namespace TCP_Client
{
    partial class FormUser
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
            label1 = new Label();
            label2 = new Label();
            textBoxPwd = new TextBox();
            textBoxUser = new TextBox();
            btnSignUp = new Button();
            btnSignIn = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 39);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 1;
            label1.Text = "帳號: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(43, 78);
            label2.Name = "label2";
            label2.Size = new Size(37, 15);
            label2.TabIndex = 6;
            label2.Text = "密碼: ";
            // 
            // textBoxPwd
            // 
            textBoxPwd.Location = new Point(114, 75);
            textBoxPwd.Name = "textBoxPwd";
            textBoxPwd.Size = new Size(153, 23);
            textBoxPwd.TabIndex = 18;
            // 
            // textBoxUser
            // 
            textBoxUser.Location = new Point(114, 36);
            textBoxUser.Name = "textBoxUser";
            textBoxUser.Size = new Size(153, 23);
            textBoxUser.TabIndex = 17;
            // 
            // btnSignUp
            // 
            btnSignUp.Location = new Point(43, 123);
            btnSignUp.Name = "btnSignUp";
            btnSignUp.Size = new Size(75, 23);
            btnSignUp.TabIndex = 19;
            btnSignUp.Text = "註冊";
            btnSignUp.UseVisualStyleBackColor = true;
            btnSignUp.Click += btnSignUp_Click;
            // 
            // btnSignIn
            // 
            btnSignIn.Location = new Point(139, 123);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(75, 23);
            btnSignIn.TabIndex = 20;
            btnSignIn.Text = "登入";
            btnSignIn.UseVisualStyleBackColor = true;
            btnSignIn.Click += btnSignIn_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(231, 123);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 21;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FormUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 166);
            Controls.Add(btnCancel);
            Controls.Add(btnSignIn);
            Controls.Add(btnSignUp);
            Controls.Add(textBoxPwd);
            Controls.Add(textBoxUser);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "FormUser";
            Text = "FormUser";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox textBoxPwd;
        private TextBox textBoxUser;
        private Button btnSignUp;
        private Button btnSignIn;
        private Button btnCancel;
    }
}