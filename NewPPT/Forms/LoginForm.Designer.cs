namespace WeCastConvertor.Forms
{
    partial class LoginForm
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.login_label = new System.Windows.Forms.Label();
            this.password_label = new System.Windows.Forms.Label();
            this.login_textbox = new System.Windows.Forms.TextBox();
            this.password_textbox = new System.Windows.Forms.TextBox();
            this.LoginGroup = new System.Windows.Forms.GroupBox();
            this.LoginGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(78, 77);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(49, 20);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(140, 77);
            this.cancel_button.Margin = new System.Windows.Forms.Padding(2);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(49, 20);
            this.cancel_button.TabIndex = 1;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // login_label
            // 
            this.login_label.AutoSize = true;
            this.login_label.Location = new System.Drawing.Point(36, 24);
            this.login_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.login_label.Name = "login_label";
            this.login_label.Size = new System.Drawing.Size(39, 13);
            this.login_label.TabIndex = 2;
            this.login_label.Text = "Login:";
            this.login_label.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(16, 54);
            this.password_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(59, 13);
            this.password_label.TabIndex = 3;
            this.password_label.Text = "Password:";
            // 
            // login_textbox
            // 
            this.login_textbox.Location = new System.Drawing.Point(78, 24);
            this.login_textbox.Margin = new System.Windows.Forms.Padding(2);
            this.login_textbox.Name = "login_textbox";
            this.login_textbox.Size = new System.Drawing.Size(112, 22);
            this.login_textbox.TabIndex = 4;
            // 
            // password_textbox
            // 
            this.password_textbox.Location = new System.Drawing.Point(78, 54);
            this.password_textbox.Margin = new System.Windows.Forms.Padding(2);
            this.password_textbox.Name = "password_textbox";
            this.password_textbox.Size = new System.Drawing.Size(112, 22);
            this.password_textbox.TabIndex = 5;
            // 
            // LoginGroup
            // 
            this.LoginGroup.Controls.Add(this.login_label);
            this.LoginGroup.Controls.Add(this.password_textbox);
            this.LoginGroup.Controls.Add(this.SaveButton);
            this.LoginGroup.Controls.Add(this.login_textbox);
            this.LoginGroup.Controls.Add(this.cancel_button);
            this.LoginGroup.Controls.Add(this.password_label);
            this.LoginGroup.Location = new System.Drawing.Point(9, 10);
            this.LoginGroup.Margin = new System.Windows.Forms.Padding(2);
            this.LoginGroup.Name = "LoginGroup";
            this.LoginGroup.Padding = new System.Windows.Forms.Padding(2);
            this.LoginGroup.Size = new System.Drawing.Size(194, 105);
            this.LoginGroup.TabIndex = 6;
            this.LoginGroup.TabStop = false;
            this.LoginGroup.Text = "Login";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 132);
            this.Controls.Add(this.LoginGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(229, 170);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(229, 170);
            this.Name = "LoginForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.LoginGroup.ResumeLayout(false);
            this.LoginGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label login_label;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.TextBox login_textbox;
        private System.Windows.Forms.TextBox password_textbox;
        private System.Windows.Forms.GroupBox LoginGroup;
    }
}