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
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.components = new System.ComponentModel.Container();
            this.lblFiles = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tblTopMenu = new System.Windows.Forms.TableLayoutPanel();
            this.lblFolders = new System.Windows.Forms.Label();
            this.lblFavorites = new System.Windows.Forms.Label();
            this.tblContent = new System.Windows.Forms.TableLayoutPanel();
            this.tblTopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFiles
            // 
            this.lblFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFiles.AutoSize = true;
            this.lblFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFiles.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFiles.ForeColor = System.Drawing.Color.Gold;
            this.lblFiles.Location = new System.Drawing.Point(5, 5);
            this.lblFiles.Margin = new System.Windows.Forms.Padding(5);
            this.lblFiles.MinimumSize = new System.Drawing.Size(100, 20);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(136, 23);
            this.lblFiles.TabIndex = 0;
            this.lblFiles.Text = "Files";
            this.lblFiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFiles.Click += new System.EventHandler(this.lblFiles_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tblTopMenu
            // 
            this.tblTopMenu.AutoSize = true;
            this.tblTopMenu.ColumnCount = 3;
            this.tblTopMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblTopMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblTopMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblTopMenu.Controls.Add(this.lblFolders, 1, 0);
            this.tblTopMenu.Controls.Add(this.lblFavorites, 2, 0);
            this.tblTopMenu.Controls.Add(this.lblFiles, 0, 0);
            this.tblTopMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblTopMenu.Location = new System.Drawing.Point(0, 0);
            this.tblTopMenu.Name = "tblTopMenu";
            this.tblTopMenu.RowCount = 1;
            this.tblTopMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblTopMenu.Size = new System.Drawing.Size(440, 33);
            this.tblTopMenu.TabIndex = 1;
            // 
            // lblFolders
            // 
            this.lblFolders.AutoSize = true;
            this.lblFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFolders.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFolders.ForeColor = System.Drawing.Color.White;
            this.lblFolders.Location = new System.Drawing.Point(151, 5);
            this.lblFolders.Margin = new System.Windows.Forms.Padding(5);
            this.lblFolders.Name = "lblFolders";
            this.lblFolders.Size = new System.Drawing.Size(136, 23);
            this.lblFolders.TabIndex = 1;
            this.lblFolders.Text = "Folders";
            this.lblFolders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFavorites
            // 
            this.lblFavorites.AutoSize = true;
            this.lblFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFavorites.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFavorites.ForeColor = System.Drawing.Color.White;
            this.lblFavorites.Location = new System.Drawing.Point(297, 5);
            this.lblFavorites.Margin = new System.Windows.Forms.Padding(5);
            this.lblFavorites.Name = "lblFavorites";
            this.lblFavorites.Size = new System.Drawing.Size(138, 23);
            this.lblFavorites.TabIndex = 2;
            this.lblFavorites.Text = "Favorites";
            this.lblFavorites.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tblContent
            // 
            this.tblContent.AutoScroll = true;
            this.tblContent.ColumnCount = 4;
            this.tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblContent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tblContent.ForeColor = System.Drawing.Color.White;
            this.tblContent.Location = new System.Drawing.Point(0, 33);
            this.tblContent.Name = "tblContent";
            this.tblContent.RowCount = 1;
            this.tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 539F));
            this.tblContent.Size = new System.Drawing.Size(440, 539);
            this.tblContent.TabIndex = 2;
            // 
            // FilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(440, 572);
            this.ControlBox = false;
            this.Controls.Add(this.tblContent);
            this.Controls.Add(this.tblTopMenu);
            this.ForeColor = System.Drawing.Color.Gold;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FilesForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FilesForm_Load);
            this.tblTopMenu.ResumeLayout(false);
            this.tblTopMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tblTopMenu;
        private System.Windows.Forms.Label lblFavorites;
        private System.Windows.Forms.Label lblFolders;
        private System.Windows.Forms.TableLayoutPanel tblContent;
    }
}