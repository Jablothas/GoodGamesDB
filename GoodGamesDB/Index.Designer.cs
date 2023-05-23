namespace GoodGamesDB
{
    partial class Index
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Index));
            PnlHeader = new Panel();
            BtnReload = new Button();
            panel3 = new Panel();
            textBoxSearch = new TextBox();
            label5 = new Label();
            panel2 = new Panel();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            PicBoxLogo = new PictureBox();
            PnlHeader_RightNav = new Panel();
            BtnMiniApp = new PictureBox();
            BtnCloseApp = new PictureBox();
            button3 = new Button();
            button2 = new Button();
            PnlHeaderSplitter = new Panel();
            BtnAdd = new Button();
            PnlDataBody = new Panel();
            PnlBottomSpacer = new Panel();
            button1 = new Button();
            textBox1 = new TextBox();
            PnlBottom = new Panel();
            PnlHeader.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxLogo).BeginInit();
            PnlHeader_RightNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)BtnMiniApp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BtnCloseApp).BeginInit();
            PnlDataBody.SuspendLayout();
            SuspendLayout();
            // 
            // PnlHeader
            // 
            PnlHeader.BackColor = Color.FromArgb(12, 12, 12);
            PnlHeader.Controls.Add(BtnReload);
            PnlHeader.Controls.Add(panel3);
            PnlHeader.Controls.Add(panel2);
            PnlHeader.Controls.Add(label3);
            PnlHeader.Controls.Add(label2);
            PnlHeader.Controls.Add(PicBoxLogo);
            PnlHeader.Controls.Add(PnlHeader_RightNav);
            PnlHeader.Controls.Add(button3);
            PnlHeader.Controls.Add(button2);
            PnlHeader.Controls.Add(PnlHeaderSplitter);
            PnlHeader.Controls.Add(BtnAdd);
            PnlHeader.Dock = DockStyle.Top;
            PnlHeader.Location = new Point(0, 0);
            PnlHeader.Margin = new Padding(4);
            PnlHeader.Name = "PnlHeader";
            PnlHeader.Size = new Size(1490, 65);
            PnlHeader.TabIndex = 0;
            PnlHeader.MouseMove += Panel_Header_MouseMove;
            // 
            // BtnReload
            // 
            BtnReload.BackColor = Color.FromArgb(12, 12, 12);
            BtnReload.CausesValidation = false;
            BtnReload.FlatAppearance.BorderColor = Color.DimGray;
            BtnReload.FlatAppearance.MouseDownBackColor = Color.DimGray;
            BtnReload.FlatAppearance.MouseOverBackColor = Color.DimGray;
            BtnReload.FlatStyle = FlatStyle.Flat;
            BtnReload.Location = new Point(651, 19);
            BtnReload.Name = "BtnReload";
            BtnReload.Size = new Size(100, 25);
            BtnReload.TabIndex = 14;
            BtnReload.Text = "Reload";
            BtnReload.TextAlign = ContentAlignment.TopCenter;
            BtnReload.UseVisualStyleBackColor = false;
            BtnReload.Click += BtnReload_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(12, 12, 12);
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(textBoxSearch);
            panel3.Controls.Add(label5);
            panel3.Location = new Point(214, 19);
            panel3.Name = "panel3";
            panel3.Size = new Size(395, 25);
            panel3.TabIndex = 13;
            // 
            // textBoxSearch
            // 
            textBoxSearch.BackColor = Color.FromArgb(12, 12, 12);
            textBoxSearch.BorderStyle = BorderStyle.None;
            textBoxSearch.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxSearch.ForeColor = Color.DimGray;
            textBoxSearch.Location = new Point(8, 4);
            textBoxSearch.Margin = new Padding(0);
            textBoxSearch.MinimumSize = new Size(0, 25);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(395, 25);
            textBoxSearch.TabIndex = 0;
            textBoxSearch.TextChanged += txtBox_Search_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 4);
            label5.Name = "label5";
            label5.Size = new Size(0, 16);
            label5.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.DimGray;
            panel2.Controls.Add(label4);
            panel2.Location = new Point(145, 19);
            panel2.Name = "panel2";
            panel2.Size = new Size(67, 25);
            panel2.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 4);
            label4.Name = "label4";
            label4.Size = new Size(53, 16);
            label4.TabIndex = 0;
            label4.Text = "Search";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(187, 187, 187);
            label3.Location = new Point(58, 33);
            label3.Name = "label3";
            label3.Size = new Size(49, 16);
            label3.TabIndex = 11;
            label3.Text = "0.14.1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Verdana", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(58, 15);
            label2.Name = "label2";
            label2.Size = new Size(61, 18);
            label2.TabIndex = 10;
            label2.Text = "GG.DB";
            // 
            // PicBoxLogo
            // 
            PicBoxLogo.Image = (Image)resources.GetObject("PicBoxLogo.Image");
            PicBoxLogo.Location = new Point(12, 12);
            PicBoxLogo.Name = "PicBoxLogo";
            PicBoxLogo.Size = new Size(40, 40);
            PicBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            PicBoxLogo.TabIndex = 9;
            PicBoxLogo.TabStop = false;
            // 
            // PnlHeader_RightNav
            // 
            PnlHeader_RightNav.Controls.Add(BtnMiniApp);
            PnlHeader_RightNav.Controls.Add(BtnCloseApp);
            PnlHeader_RightNav.Dock = DockStyle.Right;
            PnlHeader_RightNav.Location = new Point(1374, 0);
            PnlHeader_RightNav.Name = "PnlHeader_RightNav";
            PnlHeader_RightNav.Size = new Size(116, 64);
            PnlHeader_RightNav.TabIndex = 8;
            // 
            // BtnMiniApp
            // 
            BtnMiniApp.Image = (Image)resources.GetObject("BtnMiniApp.Image");
            BtnMiniApp.Location = new Point(40, 15);
            BtnMiniApp.Name = "BtnMiniApp";
            BtnMiniApp.Size = new Size(30, 30);
            BtnMiniApp.SizeMode = PictureBoxSizeMode.Zoom;
            BtnMiniApp.TabIndex = 1;
            BtnMiniApp.TabStop = false;
            BtnMiniApp.Click += BtnMiniApp_Click;
            BtnMiniApp.MouseEnter += BtnMiniApp_MouseEnter;
            BtnMiniApp.MouseLeave += BtnMiniApp_MouseLeave;
            // 
            // BtnCloseApp
            // 
            BtnCloseApp.Image = (Image)resources.GetObject("BtnCloseApp.Image");
            BtnCloseApp.Location = new Point(74, 15);
            BtnCloseApp.Name = "BtnCloseApp";
            BtnCloseApp.Size = new Size(30, 30);
            BtnCloseApp.SizeMode = PictureBoxSizeMode.Zoom;
            BtnCloseApp.TabIndex = 0;
            BtnCloseApp.TabStop = false;
            BtnCloseApp.Click += BtnCloseApp_Click;
            BtnCloseApp.MouseEnter += BtnCloseApp_MouseEnter;
            BtnCloseApp.MouseLeave += BtnCloseApp_MouseLeave;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(12, 12, 12);
            button3.CausesValidation = false;
            button3.FlatAppearance.BorderColor = Color.DimGray;
            button3.FlatAppearance.MouseDownBackColor = Color.DimGray;
            button3.FlatAppearance.MouseOverBackColor = Color.DimGray;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(1022, 20);
            button3.Name = "button3";
            button3.Size = new Size(100, 25);
            button3.TabIndex = 3;
            button3.TabStop = false;
            button3.Text = "Settings";
            button3.TextAlign = ContentAlignment.TopCenter;
            button3.UseVisualStyleBackColor = false;
            button3.Visible = false;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(12, 12, 12);
            button2.CausesValidation = false;
            button2.FlatAppearance.BorderColor = Color.DimGray;
            button2.FlatAppearance.MouseDownBackColor = Color.DimGray;
            button2.FlatAppearance.MouseOverBackColor = Color.DimGray;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(916, 20);
            button2.Name = "button2";
            button2.Size = new Size(100, 25);
            button2.TabIndex = 2;
            button2.Text = "Numbers";
            button2.TextAlign = ContentAlignment.TopCenter;
            button2.UseVisualStyleBackColor = false;
            button2.Visible = false;
            // 
            // PnlHeaderSplitter
            // 
            PnlHeaderSplitter.BackColor = Color.White;
            PnlHeaderSplitter.Dock = DockStyle.Bottom;
            PnlHeaderSplitter.Location = new Point(0, 64);
            PnlHeaderSplitter.Name = "PnlHeaderSplitter";
            PnlHeaderSplitter.Size = new Size(1490, 1);
            PnlHeaderSplitter.TabIndex = 1;
            PnlHeaderSplitter.Visible = false;
            // 
            // BtnAdd
            // 
            BtnAdd.BackColor = Color.FromArgb(12, 12, 12);
            BtnAdd.CausesValidation = false;
            BtnAdd.FlatAppearance.BorderColor = Color.DimGray;
            BtnAdd.FlatAppearance.MouseDownBackColor = Color.DimGray;
            BtnAdd.FlatAppearance.MouseOverBackColor = Color.DimGray;
            BtnAdd.FlatStyle = FlatStyle.Flat;
            BtnAdd.Location = new Point(615, 19);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(30, 25);
            BtnAdd.TabIndex = 1;
            BtnAdd.Text = "+";
            BtnAdd.TextAlign = ContentAlignment.TopCenter;
            BtnAdd.UseVisualStyleBackColor = false;
            BtnAdd.Click += BtnAdd_Click;
            // 
            // PnlDataBody
            // 
            PnlDataBody.BackColor = Color.FromArgb(30, 33, 35);
            PnlDataBody.Controls.Add(PnlBottomSpacer);
            PnlDataBody.Dock = DockStyle.Left;
            PnlDataBody.Location = new Point(0, 65);
            PnlDataBody.Margin = new Padding(0);
            PnlDataBody.Name = "PnlDataBody";
            PnlDataBody.Size = new Size(775, 677);
            PnlDataBody.TabIndex = 1;
            // 
            // PnlBottomSpacer
            // 
            PnlBottomSpacer.BackColor = Color.FromArgb(30, 33, 35);
            PnlBottomSpacer.Dock = DockStyle.Bottom;
            PnlBottomSpacer.Location = new Point(0, 642);
            PnlBottomSpacer.Name = "PnlBottomSpacer";
            PnlBottomSpacer.Size = new Size(775, 35);
            PnlBottomSpacer.TabIndex = 4;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.BackColor = Color.Black;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.WhiteSmoke;
            button1.Location = new Point(234, 201);
            button1.Name = "button1";
            button1.Size = new Size(245, 32);
            button1.TabIndex = 3;
            button1.Text = "Please let me in!";
            button1.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.None;
            textBox1.Location = new Point(234, 167);
            textBox1.Margin = new Padding(4);
            textBox1.Name = "textBox1";
            textBox1.PasswordChar = '*';
            textBox1.Size = new Size(245, 23);
            textBox1.TabIndex = 0;
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // PnlBottom
            // 
            PnlBottom.Dock = DockStyle.Bottom;
            PnlBottom.Location = new Point(0, 742);
            PnlBottom.Name = "PnlBottom";
            PnlBottom.Size = new Size(1490, 25);
            PnlBottom.TabIndex = 2;
            // 
            // Index
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 12, 12);
            ClientSize = new Size(1490, 767);
            Controls.Add(PnlDataBody);
            Controls.Add(PnlBottom);
            Controls.Add(PnlHeader);
            Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = SystemColors.Control;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "Index";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GG.DB 3.1.0";
            ResizeEnd += ResizeWindow;
            Resize += Index_SizeChanged;
            PnlHeader.ResumeLayout(false);
            PnlHeader.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxLogo).EndInit();
            PnlHeader_RightNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)BtnMiniApp).EndInit();
            ((System.ComponentModel.ISupportInitialize)BtnCloseApp).EndInit();
            PnlDataBody.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel PnlHeader;
        private Panel PnlDataBody;
        private Button button1;
        private TextBox textBox1;
        private TextBox textBoxSearch;
        private Panel PnlBottom;
        private Panel PnlHeaderSplitter;
        private Button BtnAdd;
        private Button button3;
        private Button button2;
        private Panel PnlHeader_RightNav;
        private PictureBox BtnCloseApp;
        private PictureBox PicBoxLogo;
        private Label label3;
        private Label label2;
        private Panel panel2;
        private Label label4;
        private PictureBox BtnMiniApp;
        private Panel panel3;
        private Label label5;
        private Panel PnlBottomSpacer;
        private Button BtnReload;
    }
}