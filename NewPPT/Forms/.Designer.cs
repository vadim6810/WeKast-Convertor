namespace WeCastConvertor.Forms
{
    partial class FilesForm
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
            this.lblFiles = new System.Windows.Forms.Label();
            this.tableContent = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFiles.ForeColor = System.Drawing.Color.Gold;
            this.lblFiles.Location = new System.Drawing.Point(21, 19);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(54, 23);
            this.lblFiles.TabIndex = 0;
            this.lblFiles.Text = "Files";
            // 
            // tableContent
            // 
            this.tableContent.ColumnCount = 3;
            this.tableContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableContent.Location = new System.Drawing.Point(10, 60);
            this.tableContent.Name = "tableContent";
            this.tableContent.RowCount = 1;
            this.tableContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableContent.Size = new System.Drawing.Size(400, 80);
            this.tableContent.TabIndex = 1;
            // 
            // FilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(440, 572);
            this.ControlBox = false;
            this.Controls.Add(this.tableContent);
            this.Controls.Add(this.lblFiles);
            this.ForeColor = System.Drawing.Color.Gold;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FilesForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FilesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.TableLayoutPanel tableContent;
    }
}