namespace IRC_Client
{
    partial class NewRoom
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.btnOkRoom = new System.Windows.Forms.Button();
            this.btnCancelRoom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Room:";
            // 
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(53, 20);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(197, 20);
            this.txtRoom.TabIndex = 1;
            // 
            // btnOkRoom
            // 
            this.btnOkRoom.Location = new System.Drawing.Point(15, 67);
            this.btnOkRoom.Name = "btnOkRoom";
            this.btnOkRoom.Size = new System.Drawing.Size(75, 23);
            this.btnOkRoom.TabIndex = 2;
            this.btnOkRoom.Text = "Join";
            this.btnOkRoom.UseVisualStyleBackColor = true;
            this.btnOkRoom.Click += new System.EventHandler(this.btnOkRoom_Click);
            // 
            // btnCancelRoom
            // 
            this.btnCancelRoom.Location = new System.Drawing.Point(175, 67);
            this.btnCancelRoom.Name = "btnCancelRoom";
            this.btnCancelRoom.Size = new System.Drawing.Size(75, 23);
            this.btnCancelRoom.TabIndex = 3;
            this.btnCancelRoom.Text = "Cancel";
            this.btnCancelRoom.UseVisualStyleBackColor = true;
            this.btnCancelRoom.Click += new System.EventHandler(this.btnCancelRoom_Click);
            // 
            // NewRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 105);
            this.Controls.Add(this.btnCancelRoom);
            this.Controls.Add(this.btnOkRoom);
            this.Controls.Add(this.txtRoom);
            this.Controls.Add(this.label1);
            this.Name = "NewRoom";
            this.Text = "NewRoom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.Button btnOkRoom;
        private System.Windows.Forms.Button btnCancelRoom;
    }
}