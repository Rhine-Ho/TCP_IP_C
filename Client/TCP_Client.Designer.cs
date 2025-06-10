namespace Client
{
    partial class TCP_Client
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCP_Client));
            richTextBox1 = new RichTextBox();
            button3 = new Button();
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            button4 = new Button();
            button7 = new Button();
            menuStrip1 = new MenuStrip();
            MenuItemConn = new ToolStripMenuItem();
            MenuItemSign = new ToolStripMenuItem();
            MenuItemLogOut = new ToolStripMenuItem();
            MenuItemExit = new ToolStripMenuItem();
            MenuItemOpt = new ToolStripMenuItem();
            MenuItemTxt = new ToolStripMenuItem();
            MenuItemPic = new ToolStripMenuItem();
            richTextBox2 = new RichTextBox();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            btnHi = new Button();
            btnBye = new Button();
            btnOMG = new Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.Location = new Point(490, 123);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(393, 337);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // button3
            // 
            button3.Location = new Point(889, 479);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 2;
            button3.Text = "傳送";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 42);
            label1.Name = "label1";
            label1.Size = new Size(26, 15);
            label1.TabIndex = 0;
            label1.Text = "IP:  ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 68);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 5;
            label2.Text = "Port :";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(108, 38);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 6;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(108, 68);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 7;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(490, 479);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(393, 23);
            textBox3.TabIndex = 8;
            // 
            // button4
            // 
            button4.Location = new Point(889, 362);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 9;
            button4.Text = "清除訊息";
            button4.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(889, 415);
            button7.Name = "button7";
            button7.Size = new Size(75, 23);
            button7.TabIndex = 12;
            button7.Text = "表情";
            button7.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { MenuItemConn, MenuItemOpt });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(989, 24);
            menuStrip1.TabIndex = 17;
            menuStrip1.Text = "menuStrip1";
            // 
            // MenuItemConn
            // 
            MenuItemConn.DropDownItems.AddRange(new ToolStripItem[] { MenuItemSign, MenuItemLogOut, MenuItemExit });
            MenuItemConn.Name = "MenuItemConn";
            MenuItemConn.Size = new Size(43, 20);
            MenuItemConn.Text = "連線";
            // 
            // MenuItemSign
            // 
            MenuItemSign.Name = "MenuItemSign";
            MenuItemSign.Size = new Size(127, 22);
            MenuItemSign.Text = "註冊/登入";
            MenuItemSign.Click += MenuItemSign_Click;
            // 
            // MenuItemLogOut
            // 
            MenuItemLogOut.Name = "MenuItemLogOut";
            MenuItemLogOut.Size = new Size(127, 22);
            MenuItemLogOut.Text = "登出";
            MenuItemLogOut.Click += MenuItemLogOut_Click;
            // 
            // MenuItemExit
            // 
            MenuItemExit.Name = "MenuItemExit";
            MenuItemExit.Size = new Size(127, 22);
            MenuItemExit.Text = "結束";
            MenuItemExit.Click += MenuItemExit_Click;
            // 
            // MenuItemOpt
            // 
            MenuItemOpt.DropDownItems.AddRange(new ToolStripItem[] { MenuItemTxt, MenuItemPic });
            MenuItemOpt.Name = "MenuItemOpt";
            MenuItemOpt.Size = new Size(43, 20);
            MenuItemOpt.Text = "功能";
            // 
            // MenuItemTxt
            // 
            MenuItemTxt.Name = "MenuItemTxt";
            MenuItemTxt.Size = new Size(134, 22);
            MenuItemTxt.Text = "傳送文字檔";
            MenuItemTxt.Click += MenuItemTxt_Click;
            // 
            // MenuItemPic
            // 
            MenuItemPic.Name = "MenuItemPic";
            MenuItemPic.Size = new Size(134, 22);
            MenuItemPic.Text = "傳送圖片";
            MenuItemPic.Click += MenuItemPic_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(30, 123);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(105, 337);
            richTextBox2.TabIndex = 18;
            richTextBox2.Text = "";
            richTextBox2.Visible = false;
            // 
            // label4
            // 
            label4.Font = new Font("Microsoft JhengHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 136);
            label4.Location = new Point(230, 51);
            label4.Name = "label4";
            label4.Size = new Size(304, 41);
            label4.TabIndex = 20;
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(30, 123);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(423, 337);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 21;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.Location = new Point(606, 62);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(118, 25);
            label3.TabIndex = 25;
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnHi
            // 
            btnHi.Location = new Point(889, 123);
            btnHi.Name = "btnHi";
            btnHi.Size = new Size(75, 23);
            btnHi.TabIndex = 26;
            btnHi.Text = "Hi";
            btnHi.UseVisualStyleBackColor = true;
            // 
            // btnBye
            // 
            btnBye.Location = new Point(889, 184);
            btnBye.Name = "btnBye";
            btnBye.Size = new Size(75, 23);
            btnBye.TabIndex = 27;
            btnBye.Text = "Bye";
            btnBye.UseVisualStyleBackColor = true;
            // 
            // btnOMG
            // 
            btnOMG.Location = new Point(889, 244);
            btnOMG.Name = "btnOMG";
            btnOMG.Size = new Size(75, 23);
            btnOMG.TabIndex = 28;
            btnOMG.Text = "OMG";
            btnOMG.UseVisualStyleBackColor = true;
            // 
            // TCP_Client
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(989, 541);
            Controls.Add(btnOMG);
            Controls.Add(btnBye);
            Controls.Add(btnHi);
            Controls.Add(label3);
            Controls.Add(pictureBox1);
            Controls.Add(label4);
            Controls.Add(richTextBox2);
            Controls.Add(button7);
            Controls.Add(button4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(richTextBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "TCP_Client";
            Text = "TCP_Client";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox richTextBox1;
        private Button button3;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Button button4;
        private Button button7;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem MenuItemConn;
        private ToolStripMenuItem MenuItemSign;
        private ToolStripMenuItem MenuItemLogOut;
        private ToolStripMenuItem MenuItemExit;
        private ToolStripMenuItem MenuItemOpt;
        private ToolStripMenuItem MenuItemTxt;
        private ToolStripMenuItem MenuItemPic;
        private RichTextBox richTextBox2;
        private Label label4;
        private PictureBox pictureBox1;
        private Label label3;
        private Button btnHi;
        private Button btnBye;
        private Button btnOMG;
    }
}
