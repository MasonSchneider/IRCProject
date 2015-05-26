namespace IRC_Client
{
    partial class clientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(clientForm));
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuServer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddServer = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSend = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnKick = new System.Windows.Forms.Button();
            this.btnBan = new System.Windows.Forms.Button();
            this.lblNick = new System.Windows.Forms.Label();
            this.lblNicks = new System.Windows.Forms.Label();
            this.lblServers = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnConnectServer = new System.Windows.Forms.Button();
            this.btnAddServer = new System.Windows.Forms.Button();
            this.lstServers = new System.Windows.Forms.ListBox();
            this.lstNicks = new System.Windows.Forms.ListBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.txtChatMain = new System.Windows.Forms.TextBox();
            this.tabChats = new System.Windows.Forms.TabControl();
            this.btnJoinRoom = new System.Windows.Forms.Button();
            this.btnLeave = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabChats.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(320, 587);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(797, 20);
            this.txtMsg.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuServer});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1221, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuServer
            // 
            this.menuServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddServer});
            this.menuServer.Name = "menuServer";
            this.menuServer.Size = new System.Drawing.Size(60, 20);
            this.menuServer.Text = "Server...";
            // 
            // menuAddServer
            // 
            this.menuAddServer.Name = "menuAddServer";
            this.menuAddServer.Size = new System.Drawing.Size(131, 22);
            this.menuAddServer.Text = "Add Server";
            this.menuAddServer.Click += new System.EventHandler(this.menuAddServer_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(1123, 587);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(86, 20);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnKick);
            this.panel1.Controls.Add(this.btnBan);
            this.panel1.Location = new System.Drawing.Point(1002, 506);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(207, 71);
            this.panel1.TabIndex = 5;
            // 
            // btnKick
            // 
            this.btnKick.Location = new System.Drawing.Point(4, 38);
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size(50, 29);
            this.btnKick.TabIndex = 1;
            this.btnKick.Text = "Kick";
            this.btnKick.UseVisualStyleBackColor = true;
            // 
            // btnBan
            // 
            this.btnBan.Location = new System.Drawing.Point(4, 3);
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size(50, 29);
            this.btnBan.TabIndex = 0;
            this.btnBan.Text = "Ban";
            this.btnBan.UseVisualStyleBackColor = true;
            // 
            // lblNick
            // 
            this.lblNick.Location = new System.Drawing.Point(232, 586);
            this.lblNick.Name = "lblNick";
            this.lblNick.Size = new System.Drawing.Size(87, 20);
            this.lblNick.TabIndex = 6;
            this.lblNick.Text = "Nickname:";
            this.lblNick.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNicks
            // 
            this.lblNicks.AutoSize = true;
            this.lblNicks.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNicks.Location = new System.Drawing.Point(998, 46);
            this.lblNicks.Name = "lblNicks";
            this.lblNicks.Size = new System.Drawing.Size(82, 20);
            this.lblNicks.TabIndex = 7;
            this.lblNicks.Text = "Nick List:";
            // 
            // lblServers
            // 
            this.lblServers.AutoSize = true;
            this.lblServers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServers.Location = new System.Drawing.Point(12, 46);
            this.lblServers.Name = "lblServers";
            this.lblServers.Size = new System.Drawing.Size(75, 20);
            this.lblServers.TabIndex = 9;
            this.lblServers.Text = "Servers:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnEdit);
            this.panel2.Controls.Add(this.btnDisconnect);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Controls.Add(this.btnConnectServer);
            this.panel2.Controls.Add(this.btnAddServer);
            this.panel2.Location = new System.Drawing.Point(12, 509);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(214, 100);
            this.panel2.TabIndex = 10;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(136, 33);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(4, 33);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(136, 62);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnConnectServer
            // 
            this.btnConnectServer.Location = new System.Drawing.Point(3, 3);
            this.btnConnectServer.Name = "btnConnectServer";
            this.btnConnectServer.Size = new System.Drawing.Size(75, 23);
            this.btnConnectServer.TabIndex = 1;
            this.btnConnectServer.Text = "Connect";
            this.btnConnectServer.UseVisualStyleBackColor = true;
            this.btnConnectServer.Click += new System.EventHandler(this.btnConnectServer_Click);
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point(136, 3);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(75, 23);
            this.btnAddServer.TabIndex = 0;
            this.btnAddServer.Text = "Add";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler(this.btnAddServer_Click);
            // 
            // lstServers
            // 
            this.lstServers.FormattingEnabled = true;
            this.lstServers.Location = new System.Drawing.Point(12, 69);
            this.lstServers.Name = "lstServers";
            this.lstServers.Size = new System.Drawing.Size(211, 433);
            this.lstServers.TabIndex = 1;
            // 
            // lstNicks
            // 
            this.lstNicks.FormattingEnabled = true;
            this.lstNicks.Items.AddRange(new object[] {
            "Must join a channel"});
            this.lstNicks.Location = new System.Drawing.Point(1002, 69);
            this.lstNicks.Name = "lstNicks";
            this.lstNicks.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstNicks.Size = new System.Drawing.Size(207, 433);
            this.lstNicks.TabIndex = 11;
            this.lstNicks.SelectedIndexChanged += new System.EventHandler(this.s);
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerName.Location = new System.Drawing.Point(231, 46);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(129, 20);
            this.lblServerName.TabIndex = 12;
            this.lblServerName.Text = "Not Connected";
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.txtChatMain);
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(741, 486);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "server";
            this.tabPageMain.UseVisualStyleBackColor = true;
            this.tabPageMain.Click += new System.EventHandler(this.tabPageMain_Click);
            // 
            // txtChatMain
            // 
            this.txtChatMain.Location = new System.Drawing.Point(3, 0);
            this.txtChatMain.Multiline = true;
            this.txtChatMain.Name = "txtChatMain";
            this.txtChatMain.ReadOnly = true;
            this.txtChatMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatMain.Size = new System.Drawing.Size(729, 474);
            this.txtChatMain.TabIndex = 0;
            this.txtChatMain.Text = "Connect to a server";
            this.txtChatMain.TextChanged += new System.EventHandler(this.txtChatMain_TextChanged);
            // 
            // tabChats
            // 
            this.tabChats.Controls.Add(this.tabPageMain);
            this.tabChats.Location = new System.Drawing.Point(235, 74);
            this.tabChats.Name = "tabChats";
            this.tabChats.SelectedIndex = 0;
            this.tabChats.Size = new System.Drawing.Size(749, 512);
            this.tabChats.TabIndex = 0;
            this.tabChats.SelectedIndexChanged += new System.EventHandler(this.tabChats_SelectedIndexChanged);
            this.tabChats.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabChats_Selected);
            this.tabChats.TabIndexChanged += new System.EventHandler(this.tabChats_TabIndexChanged);
            // 
            // btnJoinRoom
            // 
            this.btnJoinRoom.Location = new System.Drawing.Point(366, 46);
            this.btnJoinRoom.Name = "btnJoinRoom";
            this.btnJoinRoom.Size = new System.Drawing.Size(82, 22);
            this.btnJoinRoom.TabIndex = 13;
            this.btnJoinRoom.Text = "Join Channel";
            this.btnJoinRoom.UseVisualStyleBackColor = true;
            this.btnJoinRoom.Click += new System.EventHandler(this.btnJoinRoom_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.Location = new System.Drawing.Point(897, 48);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(87, 20);
            this.btnLeave.TabIndex = 14;
            this.btnLeave.Text = "Leave Channel";
            this.btnLeave.UseVisualStyleBackColor = true;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // clientForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1221, 619);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnJoinRoom);
            this.Controls.Add(this.lblServerName);
            this.Controls.Add(this.lstNicks);
            this.Controls.Add(this.lstServers);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblServers);
            this.Controls.Add(this.lblNicks);
            this.Controls.Add(this.lblNick);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.tabChats);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "clientForm";
            this.Text = "Rose IRC Client";
            this.Load += new System.EventHandler(this.clientForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabChats.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuServer;
        private System.Windows.Forms.ToolStripMenuItem menuAddServer;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnKick;
        private System.Windows.Forms.Button btnBan;
        private System.Windows.Forms.Label lblNick;
        private System.Windows.Forms.Label lblNicks;
        private System.Windows.Forms.Label lblServers;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnConnectServer;
        private System.Windows.Forms.Button btnAddServer;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListBox lstServers;
        private System.Windows.Forms.ListBox lstNicks;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TextBox txtChatMain;
        private System.Windows.Forms.TabControl tabChats;
        private System.Windows.Forms.Button btnJoinRoom;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnEdit;
    }
}

