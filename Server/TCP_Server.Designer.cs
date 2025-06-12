namespace Server
{
    partial class TCP_Server
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Replace the problematic line in the Dispose method with the following:  
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCP_Server));
            label1 = new Label();
            label2 = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            richTextBox1 = new RichTextBox();
            textBox3 = new TextBox();
            button3 = new Button();
            button4 = new Button();
            button7 = new Button();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            MenuItemRegister = new ToolStripMenuItem();
            MenuItemOpenServer = new ToolStripMenuItem();
            MenuItemCloseServer = new ToolStripMenuItem();
            MenuItemExit = new ToolStripMenuItem();
            MenuItemOption = new ToolStripMenuItem();
            MenuItemTxt = new ToolStripMenuItem();
            MenuItemPic = new ToolStripMenuItem();
            MenuItemMedia = new ToolStripMenuItem();
            richTextBox2 = new RichTextBox();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            btnHi = new Button();
            btnBye = new Button();
            btnOMG = new Button();
            buttonGetIP_Click = new Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 136);
            label1.Location = new Point(30, 42);
            label1.Name = "label1";
            label1.Size = new Size(23, 15);
            label1.TabIndex = 0;
            label1.Text = "IP :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 136);
            label2.Location = new Point(30, 81);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 1;
            label2.Text = "Port :";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 136);
            textBox1.Location = new Point(108, 38);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 136);
            textBox2.Location = new Point(108, 78);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 3;
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.Location = new Point(485, 136);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(393, 333);
            richTextBox1.TabIndex = 6;
            richTextBox1.Text = "";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(485, 488);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(393, 23);
            textBox3.TabIndex = 9;
            // 
            // button3
            // 
            button3.Location = new Point(884, 488);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 10;
            button3.Text = "傳送";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 136);
            button4.Location = new Point(884, 371);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 11;
            button4.Text = "清除訊息";
            button4.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(884, 423);
            button7.Name = "button7";
            button7.Size = new Size(75, 23);
            button7.TabIndex = 14;
            button7.Text = "表情";
            button7.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, MenuItemOption });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1003, 24);
            menuStrip1.TabIndex = 15;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { MenuItemRegister, MenuItemOpenServer, MenuItemCloseServer, MenuItemExit });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(55, 20);
            toolStripMenuItem1.Text = "伺服器";
            // 
            // MenuItemRegister
            // 
            MenuItemRegister.Name = "MenuItemRegister";
            MenuItemRegister.Size = new Size(134, 22);
            MenuItemRegister.Text = "註冊表";
            MenuItemRegister.Click += MenuItemRegister_Click;
            // 
            // MenuItemOpenServer
            // 
            MenuItemOpenServer.Name = "MenuItemOpenServer";
            MenuItemOpenServer.Size = new Size(134, 22);
            MenuItemOpenServer.Text = "開啟伺服器";
            MenuItemOpenServer.Click += MenuItemOpenServer_Click;
            // 
            // MenuItemCloseServer
            // 
            MenuItemCloseServer.Name = "MenuItemCloseServer";
            MenuItemCloseServer.Size = new Size(134, 22);
            MenuItemCloseServer.Text = "關閉伺服器";
            MenuItemCloseServer.Click += MenuItemCloseServer_Click;
            // 
            // MenuItemExit
            // 
            MenuItemExit.Name = "MenuItemExit";
            MenuItemExit.Size = new Size(134, 22);
            MenuItemExit.Text = "結束";
            // 
            // MenuItemOption
            // 
            MenuItemOption.DropDownItems.AddRange(new ToolStripItem[] { MenuItemTxt, MenuItemPic, MenuItemMedia });
            MenuItemOption.Name = "MenuItemOption";
            MenuItemOption.Size = new Size(67, 20);
            MenuItemOption.Text = "廣播功能";
            // 
            // MenuItemTxt
            // 
            MenuItemTxt.Name = "MenuItemTxt";
            MenuItemTxt.Size = new Size(180, 22);
            MenuItemTxt.Text = "傳送文字檔";
            MenuItemTxt.Click += MenuItemTxt_Click;
            // 
            // MenuItemPic
            // 
            MenuItemPic.Name = "MenuItemPic";
            MenuItemPic.Size = new Size(180, 22);
            MenuItemPic.Text = "傳送圖片";
            MenuItemPic.Click += MenuItemPic_Click;
            // 
            // MenuItemMedia
            // 
            MenuItemMedia.Name = "MenuItemMedia";
            MenuItemMedia.Size = new Size(180, 22);
            MenuItemMedia.Text = "傳送影音";
            MenuItemMedia.Click += MenuItemMedia_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(30, 136);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(108, 333);
            richTextBox2.TabIndex = 16;
            richTextBox2.Text = "";
            richTextBox2.Visible = false;
            // 
            // label3
            // 
            label3.BackColor = SystemColors.Control;
            label3.Location = new Point(30, 488);
            label3.Name = "label3";
            label3.Size = new Size(108, 23);
            label3.TabIndex = 17;
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(30, 136);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(419, 333);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 18;
            pictureBox1.TabStop = false;
            // 
            // btnHi
            // 
            btnHi.Location = new Point(884, 136);
            btnHi.Name = "btnHi";
            btnHi.Size = new Size(75, 23);
            btnHi.TabIndex = 19;
            btnHi.Text = "Hi";
            btnHi.UseVisualStyleBackColor = true;
            // 
            // btnBye
            // 
            btnBye.Location = new Point(884, 192);
            btnBye.Name = "btnBye";
            btnBye.Size = new Size(75, 23);
            btnBye.TabIndex = 20;
            btnBye.Text = "Bye";
            btnBye.UseVisualStyleBackColor = true;
            // 
            // btnOMG
            // 
            btnOMG.Location = new Point(884, 251);
            btnOMG.Name = "btnOMG";
            btnOMG.Size = new Size(75, 23);
            btnOMG.TabIndex = 21;
            btnOMG.Text = "OMG";
            btnOMG.UseVisualStyleBackColor = true;
            // 
            // buttonGetIP_Click
            // 
            buttonGetIP_Click.Location = new Point(274, 50);
            buttonGetIP_Click.Name = "buttonGetIP_Click";
            buttonGetIP_Click.Size = new Size(75, 23);
            buttonGetIP_Click.TabIndex = 22;
            buttonGetIP_Click.Text = "取得當地IP";
            buttonGetIP_Click.UseVisualStyleBackColor = true;
            buttonGetIP_Click.Click += buttonGetIP_Click_Click;
            // 
            // TCP_Server
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1003, 533);
            Controls.Add(buttonGetIP_Click);
            Controls.Add(btnOMG);
            Controls.Add(btnBye);
            Controls.Add(btnHi);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(richTextBox2);
            Controls.Add(button7);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(textBox3);
            Controls.Add(richTextBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "TCP_Server";
            Text = "TCP_Server";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        // Add the missing Form1_Load method to resolve the CS1061 error.  
        private void Form1_Load(object sender, EventArgs e)
        {
            // Add any initialization logic here if needed.  
        }

        #endregion

        private Label label1;
        private Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TextBox textBox1;
        private TextBox textBox2;
        private RichTextBox richTextBox1;
        private TextBox textBox3;
        private Button button3;
        private Button button4;
        private Button button7;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem MenuItemRegister;
        private ToolStripMenuItem MenuItemOpenServer;
        private ToolStripMenuItem MenuItemCloseServer;
        private ToolStripMenuItem MenuItemExit;
        private ToolStripMenuItem MenuItemOption;
        private ToolStripMenuItem MenuItemTxt;
        private ToolStripMenuItem MenuItemPic;
        private RichTextBox richTextBox2;
        private Label label3;
        private PictureBox pictureBox1;
        private Button btnHi;
        private Button btnBye;
        private Button btnOMG;
        private Button buttonGetIP_Click;
        private ToolStripMenuItem MenuItemMedia;
    }
}
