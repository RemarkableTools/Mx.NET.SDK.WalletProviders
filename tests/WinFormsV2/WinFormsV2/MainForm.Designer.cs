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
            btnSendMultiple = new Button();
            btnSend = new Button();
            btnDisconnect = new Button();
            btnConnect = new Button();
            qrCodeImg = new PictureBox();
            tbReceiver = new TextBox();
            label1 = new Label();
            label2 = new Label();
            tbEGLD = new TextBox();
            tbConnectionStatus = new Label();
            btnSignMessage = new Button();
            lbSignature = new Label();
            label4 = new Label();
            label3 = new Label();
            tbMessageToSign = new TextBox();
            ((System.ComponentModel.ISupportInitialize)qrCodeImg).BeginInit();
            SuspendLayout();
            // 
            // btnSendMultiple
            // 
            btnSendMultiple.FlatStyle = FlatStyle.Flat;
            btnSendMultiple.Location = new Point(154, 697);
            btnSendMultiple.Name = "btnSendMultiple";
            btnSendMultiple.Size = new Size(127, 39);
            btnSendMultiple.TabIndex = 9;
            btnSendMultiple.Text = "Send Multiple";
            btnSendMultiple.UseVisualStyleBackColor = true;
            btnSendMultiple.Click += BtnSendMultiple_Click;
            // 
            // btnSend
            // 
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Location = new Point(26, 697);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(99, 39);
            btnSend.TabIndex = 8;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += BtnSend_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.Location = new Point(26, 513);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(99, 39);
            btnDisconnect.TabIndex = 4;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Visible = false;
            btnDisconnect.Click += BtnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Location = new Point(26, 513);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(99, 39);
            btnConnect.TabIndex = 3;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Visible = false;
            btnConnect.Click += BtnConnect_Click;
            // 
            // qrCodeImg
            // 
            qrCodeImg.BackgroundImageLayout = ImageLayout.Center;
            qrCodeImg.Location = new Point(26, 27);
            qrCodeImg.Name = "qrCodeImg";
            qrCodeImg.Size = new Size(593, 446);
            qrCodeImg.TabIndex = 5;
            qrCodeImg.TabStop = false;
            // 
            // tbReceiver
            // 
            tbReceiver.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            tbReceiver.Location = new Point(97, 610);
            tbReceiver.Name = "tbReceiver";
            tbReceiver.Size = new Size(522, 27);
            tbReceiver.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(26, 613);
            label1.Name = "label1";
            label1.Size = new Size(65, 20);
            label1.TabIndex = 0;
            label1.Text = "Receiver";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(46, 655);
            label2.Name = "label2";
            label2.Size = new Size(45, 20);
            label2.TabIndex = 13;
            label2.Text = "EGLD";
            // 
            // tbEGLD
            // 
            tbEGLD.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            tbEGLD.Location = new Point(97, 652);
            tbEGLD.Name = "tbEGLD";
            tbEGLD.Size = new Size(88, 27);
            tbEGLD.TabIndex = 1;
            // 
            // tbConnectionStatus
            // 
            tbConnectionStatus.AutoSize = true;
            tbConnectionStatus.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            tbConnectionStatus.Location = new Point(26, 557);
            tbConnectionStatus.Name = "tbConnectionStatus";
            tbConnectionStatus.Size = new Size(0, 20);
            tbConnectionStatus.TabIndex = 14;
            // 
            // btnSignMessage
            // 
            btnSignMessage.FlatStyle = FlatStyle.Flat;
            btnSignMessage.Location = new Point(26, 842);
            btnSignMessage.Name = "btnSignMessage";
            btnSignMessage.Size = new Size(131, 39);
            btnSignMessage.TabIndex = 24;
            btnSignMessage.Text = "Sign Message";
            btnSignMessage.UseVisualStyleBackColor = true;
            btnSignMessage.Click += BtnSignMessage_Click;
            // 
            // lbSignature
            // 
            lbSignature.AutoEllipsis = true;
            lbSignature.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            lbSignature.Location = new Point(97, 811);
            lbSignature.Name = "lbSignature";
            lbSignature.Size = new Size(522, 20);
            lbSignature.TabIndex = 23;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(18, 811);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 22;
            label4.Text = "Signature:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(26, 777);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 21;
            label3.Text = "Message";
            // 
            // tbMessageToSign
            // 
            tbMessageToSign.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            tbMessageToSign.Location = new Point(97, 774);
            tbMessageToSign.Name = "tbMessageToSign";
            tbMessageToSign.Size = new Size(468, 27);
            tbMessageToSign.TabIndex = 20;
            tbMessageToSign.TextChanged += TbMessageToSign_TextChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(645, 906);
            Controls.Add(btnSignMessage);
            Controls.Add(lbSignature);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(tbMessageToSign);
            Controls.Add(tbConnectionStatus);
            Controls.Add(label2);
            Controls.Add(tbEGLD);
            Controls.Add(label1);
            Controls.Add(tbReceiver);
            Controls.Add(btnSendMultiple);
            Controls.Add(btnSend);
            Controls.Add(qrCodeImg);
            Controls.Add(btnConnect);
            Controls.Add(btnDisconnect);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Wallet Connect - Example";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)qrCodeImg).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSendMultiple;
        private Button btnSend;
        private Button btnDisconnect;
        private Button btnConnect;
        private PictureBox qrCodeImg;
        private TextBox tbReceiver;
        private Label label1;
        private Label label2;
        private TextBox tbEGLD;
        private Label tbConnectionStatus;
        private Button btnSignMessage;
        private Label lbSignature;
        private Label label4;
        private Label label3;
        private TextBox tbMessageToSign;
    }
}