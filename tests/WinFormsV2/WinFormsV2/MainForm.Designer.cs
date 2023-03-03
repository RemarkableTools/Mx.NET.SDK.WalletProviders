namespace WinForms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnSendMultiple = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbReceiver = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbEGLD = new System.Windows.Forms.TextBox();
            this.tbConnectionStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSendMultiple
            // 
            this.btnSendMultiple.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendMultiple.Location = new System.Drawing.Point(154, 697);
            this.btnSendMultiple.Name = "btnSendMultiple";
            this.btnSendMultiple.Size = new System.Drawing.Size(127, 39);
            this.btnSendMultiple.TabIndex = 9;
            this.btnSendMultiple.Text = "Send Multiple";
            this.btnSendMultiple.UseVisualStyleBackColor = true;
            this.btnSendMultiple.Click += new System.EventHandler(this.BtnSendMultiple_Click);
            // 
            // btnSend
            // 
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Location = new System.Drawing.Point(26, 697);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(99, 39);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.Location = new System.Drawing.Point(154, 513);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(99, 39);
            this.btnDisconnect.TabIndex = 7;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Location = new System.Drawing.Point(26, 513);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(99, 39);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(26, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(593, 446);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // tbReceiver
            // 
            this.tbReceiver.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbReceiver.Location = new System.Drawing.Point(97, 610);
            this.tbReceiver.Name = "tbReceiver";
            this.tbReceiver.Size = new System.Drawing.Size(522, 27);
            this.tbReceiver.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(26, 613);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Receiver";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(46, 655);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "EGLD";
            // 
            // tbEGLD
            // 
            this.tbEGLD.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbEGLD.Location = new System.Drawing.Point(97, 652);
            this.tbEGLD.Name = "tbEGLD";
            this.tbEGLD.Size = new System.Drawing.Size(88, 27);
            this.tbEGLD.TabIndex = 12;
            // 
            // tbConnectionStatus
            // 
            this.tbConnectionStatus.AutoSize = true;
            this.tbConnectionStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbConnectionStatus.Location = new System.Drawing.Point(26, 557);
            this.tbConnectionStatus.Name = "tbConnectionStatus";
            this.tbConnectionStatus.Size = new System.Drawing.Size(0, 20);
            this.tbConnectionStatus.TabIndex = 14;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(645, 774);
            this.Controls.Add(this.tbConnectionStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbEGLD);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbReceiver);
            this.Controls.Add(this.btnSendMultiple);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wallet Connect - Example";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnSendMultiple;
        private Button btnSend;
        private Button btnDisconnect;
        private Button btnConnect;
        private PictureBox pictureBox1;
        private TextBox tbReceiver;
        private Label label1;
        private Label label2;
        private TextBox tbEGLD;
        private Label tbConnectionStatus;
    }
}