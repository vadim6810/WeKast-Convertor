namespace WeCastConvertor.Forms
{
    partial class MdiConvert
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
            this.pnlDragDrop = new System.Windows.Forms.Panel();
            this.pctSelectFiles = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pctExit = new System.Windows.Forms.PictureBox();
            this.pctSettings = new System.Windows.Forms.PictureBox();
            this.pctLogo = new System.Windows.Forms.PictureBox();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.pgsStatusProgress = new System.Windows.Forms.ProgressBar();
            this.pnlStatus = new System.Windows.Forms.TableLayoutPanel();
            this.lblStatusSize = new System.Windows.Forms.Label();
            this.pctProgressCancel = new System.Windows.Forms.PictureBox();
            this.pnlDragDrop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctSelectFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctLogo)).BeginInit();
            this.pnlStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctProgressCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Gold;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Add files to transfer to the App";
            this.label1.UseMnemonic = false;
            // 
            // pnlDragDrop
            // 
            this.pnlDragDrop.Controls.Add(this.pctSelectFiles);
            this.pnlDragDrop.Controls.Add(this.label3);
            this.pnlDragDrop.Controls.Add(this.label2);
            this.pnlDragDrop.Location = new System.Drawing.Point(10, 75);
            this.pnlDragDrop.Name = "pnlDragDrop";
            this.pnlDragDrop.Size = new System.Drawing.Size(420, 180);
            this.pnlDragDrop.TabIndex = 4;
            this.pnlDragDrop.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pctSelectFiles
            // 
            this.pctSelectFiles.Image = global::WeCastConvertor.Properties.Resources.SelectButton;
            this.pctSelectFiles.Location = new System.Drawing.Point(160, 85);
            this.pctSelectFiles.Name = "pctSelectFiles";
            this.pctSelectFiles.Size = new System.Drawing.Size(100, 30);
            this.pctSelectFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctSelectFiles.TabIndex = 2;
            this.pctSelectFiles.TabStop = false;
            this.pctSelectFiles.Paint += new System.Windows.Forms.PaintEventHandler(this.pctSelectFiles_Paint);
            this.pctSelectFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctSelectFiles_MouseDown);
            this.pctSelectFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(190, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 30);
            this.label3.TabIndex = 1;
            this.label3.Text = "or";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(110, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "Drop Files Here";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pctExit
            // 
            this.pctExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pctExit.Image = global::WeCastConvertor.Properties.Resources.exit;
            this.pctExit.Location = new System.Drawing.Point(423, 0);
            this.pctExit.Name = "pctExit";
            this.pctExit.Size = new System.Drawing.Size(17, 17);
            this.pctExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pctExit.TabIndex = 2;
            this.pctExit.TabStop = false;
            this.pctExit.Click += new System.EventHandler(this.pctExit_Click);
            // 
            // pctSettings
            // 
            this.pctSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pctSettings.Image = global::WeCastConvertor.Properties.Resources.settings1;
            this.pctSettings.Location = new System.Drawing.Point(340, 34);
            this.pctSettings.Name = "pctSettings";
            this.pctSettings.Size = new System.Drawing.Size(24, 24);
            this.pctSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pctSettings.TabIndex = 1;
            this.pctSettings.TabStop = false;
            // 
            // pctLogo
            // 
            this.pctLogo.Image = global::WeCastConvertor.Properties.Resources.logo;
            this.pctLogo.Location = new System.Drawing.Point(363, 12);
            this.pctLogo.Name = "pctLogo";
            this.pctLogo.Size = new System.Drawing.Size(65, 65);
            this.pctLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pctLogo.TabIndex = 0;
            this.pctLogo.TabStop = false;
            this.pctLogo.Click += new System.EventHandler(this.pctLogo_Click);
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.AutoSize = true;
            this.lblStatusMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusMessage.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusMessage.ForeColor = System.Drawing.Color.Gold;
            this.lblStatusMessage.Location = new System.Drawing.Point(3, 0);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(193, 27);
            this.lblStatusMessage.TabIndex = 5;
            this.lblStatusMessage.Text = "We currently dont support this formats";
            this.lblStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pgsStatusProgress
            // 
            this.pgsStatusProgress.Location = new System.Drawing.Point(281, 3);
            this.pgsStatusProgress.Name = "pgsStatusProgress";
            this.pgsStatusProgress.Size = new System.Drawing.Size(112, 21);
            this.pgsStatusProgress.TabIndex = 6;
            this.pgsStatusProgress.Value = 20;
            // 
            // pnlStatus
            // 
            this.pnlStatus.ColumnCount = 4;
            this.pnlStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.pnlStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.pnlStatus.Controls.Add(this.pgsStatusProgress, 2, 0);
            this.pnlStatus.Controls.Add(this.lblStatusMessage, 0, 0);
            this.pnlStatus.Controls.Add(this.lblStatusSize, 1, 0);
            this.pnlStatus.Controls.Add(this.pctProgressCancel, 3, 0);
            this.pnlStatus.Location = new System.Drawing.Point(10, 260);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.RowCount = 1;
            this.pnlStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlStatus.Size = new System.Drawing.Size(420, 27);
            this.pnlStatus.TabIndex = 5;
            this.pnlStatus.Visible = false;
            // 
            // lblStatusSize
            // 
            this.lblStatusSize.AutoSize = true;
            this.lblStatusSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusSize.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusSize.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblStatusSize.Location = new System.Drawing.Point(202, 0);
            this.lblStatusSize.Name = "lblStatusSize";
            this.lblStatusSize.Size = new System.Drawing.Size(73, 27);
            this.lblStatusSize.TabIndex = 7;
            this.lblStatusSize.Text = "label5";
            this.lblStatusSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pctProgressCancel
            // 
            this.pctProgressCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pctProgressCancel.BackgroundImage = global::WeCastConvertor.Properties.Resources.exit;
            this.pctProgressCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pctProgressCancel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctProgressCancel.Location = new System.Drawing.Point(397, 0);
            this.pctProgressCancel.Margin = new System.Windows.Forms.Padding(0);
            this.pctProgressCancel.Name = "pctProgressCancel";
            this.pctProgressCancel.Size = new System.Drawing.Size(23, 27);
            this.pctProgressCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pctProgressCancel.TabIndex = 8;
            this.pctProgressCancel.TabStop = false;
            // 
            // MdiConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(440, 300);
            this.ControlBox = false;
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlDragDrop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pctExit);
            this.Controls.Add(this.pctSettings);
            this.Controls.Add(this.pctLogo);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MdiConvert";
            this.Text = "mdiConvert";
            this.Load += new System.EventHandler(this.MdiConvert_Load);
            this.pnlDragDrop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctSelectFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctLogo)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctProgressCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctLogo;
        private System.Windows.Forms.PictureBox pctSettings;
        private System.Windows.Forms.PictureBox pctExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlDragDrop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pctSelectFiles;
        private System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.ProgressBar pgsStatusProgress;
        private System.Windows.Forms.TableLayoutPanel pnlStatus;
        private System.Windows.Forms.Label lblStatusSize;
        private System.Windows.Forms.PictureBox pctProgressCancel;
    }
}