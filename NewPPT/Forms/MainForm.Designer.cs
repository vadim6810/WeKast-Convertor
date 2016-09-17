namespace WeCastConvertor.Forms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.LogWindow = new System.Windows.Forms.ListBox();
            this.grid = new System.Windows.Forms.DataGridView();
            this.SourcePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Converted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uploaded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridData = new System.Windows.Forms.BindingSource(this.components);
            this.AddButton = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.AboutButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.GroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogWindow
            // 
            this.LogWindow.FormattingEnabled = true;
            this.LogWindow.Location = new System.Drawing.Point(4, 369);
            this.LogWindow.Name = "LogWindow";
            this.LogWindow.Size = new System.Drawing.Size(635, 56);
            this.LogWindow.TabIndex = 2;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AutoGenerateColumns = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SourcePath,
            this.Converted,
            this.Uploaded});
            this.grid.DataSource = this.gridData;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid.DefaultCellStyle = dataGridViewCellStyle2;
            this.grid.Location = new System.Drawing.Point(4, 32);
            this.grid.Margin = new System.Windows.Forms.Padding(2);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grid.RowTemplate.Height = 24;
            this.grid.Size = new System.Drawing.Size(634, 332);
            this.grid.TabIndex = 4;
            // 
            // SourcePath
            // 
            this.SourcePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SourcePath.DataPropertyName = "SourcePath";
            this.SourcePath.FillWeight = 7.312659F;
            this.SourcePath.HeaderText = "Name";
            this.SourcePath.MinimumWidth = 100;
            this.SourcePath.Name = "SourcePath";
            this.SourcePath.ReadOnly = true;
            // 
            // Converted
            // 
            this.Converted.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Converted.DataPropertyName = "Convert";
            this.Converted.FillWeight = 152.2843F;
            this.Converted.HeaderText = "Converted";
            this.Converted.MinimumWidth = 100;
            this.Converted.Name = "Converted";
            this.Converted.ReadOnly = true;
            this.Converted.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Uploaded
            // 
            this.Uploaded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Uploaded.DataPropertyName = "Upload";
            this.Uploaded.FillWeight = 140.4031F;
            this.Uploaded.HeaderText = "Uploaded";
            this.Uploaded.MinimumWidth = 100;
            this.Uploaded.Name = "Uploaded";
            this.Uploaded.ReadOnly = true;
            this.Uploaded.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(65, 8);
            this.AddButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(56, 19);
            this.AddButton.TabIndex = 5;
            this.AddButton.Text = "Add File";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(4, 8);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(2);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(56, 19);
            this.LoginButton.TabIndex = 6;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // AboutButton
            // 
            this.AboutButton.Location = new System.Drawing.Point(521, 8);
            this.AboutButton.Margin = new System.Windows.Forms.Padding(2);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(56, 19);
            this.AboutButton.TabIndex = 7;
            this.AboutButton.Text = "About";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(582, 8);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(2);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(56, 19);
            this.ExitButton.TabIndex = 8;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // GroupBox
            // 
            this.GroupBox.Controls.Add(this.LoginButton);
            this.GroupBox.Controls.Add(this.LogWindow);
            this.GroupBox.Controls.Add(this.grid);
            this.GroupBox.Controls.Add(this.ExitButton);
            this.GroupBox.Controls.Add(this.AddButton);
            this.GroupBox.Controls.Add(this.AboutButton);
            this.GroupBox.Location = new System.Drawing.Point(10, 10);
            this.GroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.GroupBox.Name = "GroupBox";
            this.GroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.GroupBox.Size = new System.Drawing.Size(643, 430);
            this.GroupBox.TabIndex = 9;
            this.GroupBox.TabStop = false;
            this.GroupBox.Visible = false;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(679, 495);
            this.Controls.Add(this.GroupBox);
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(679, 495);
            this.MinimumSize = new System.Drawing.Size(679, 495);
            this.Name = "MainForm";
            this.Text = "WeKast converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox LogWindow;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.BindingSource gridData;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourcePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn Converted;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uploaded;
        private System.Windows.Forms.GroupBox GroupBox;
    }
}

