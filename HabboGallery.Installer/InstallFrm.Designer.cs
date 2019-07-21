namespace HabboGalleryInstaller
{
    partial class InstallFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallFrm));
            this.InstallationPathDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.CustomizeTgl = new System.Windows.Forms.CheckBox();
            this.CustomInstallationPnl = new System.Windows.Forms.Panel();
            this.DesktopShortTgl = new System.Windows.Forms.CheckBox();
            this.StartMenuTgl = new System.Windows.Forms.CheckBox();
            this.SelectCustomPathBtn = new System.Windows.Forms.Button();
            this.CustomPathTxt = new System.Windows.Forms.TextBox();
            this.MainContinueButton = new System.Windows.Forms.Button();
            this.MainInfoLbl = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LaunchAppTgl = new System.Windows.Forms.CheckBox();
            this.CustomInstallationPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomizeTgl
            // 
            this.CustomizeTgl.AutoSize = true;
            this.CustomizeTgl.Location = new System.Drawing.Point(541, 212);
            this.CustomizeTgl.Name = "CustomizeTgl";
            this.CustomizeTgl.Size = new System.Drawing.Size(126, 17);
            this.CustomizeTgl.TabIndex = 9;
            this.CustomizeTgl.Text = "Customize installation";
            this.CustomizeTgl.UseVisualStyleBackColor = true;
            this.CustomizeTgl.Visible = false;
            this.CustomizeTgl.CheckedChanged += new System.EventHandler(this.CustomizeTgl_CheckedChanged);
            // 
            // CustomInstallationPnl
            // 
            this.CustomInstallationPnl.Controls.Add(this.DesktopShortTgl);
            this.CustomInstallationPnl.Controls.Add(this.StartMenuTgl);
            this.CustomInstallationPnl.Controls.Add(this.SelectCustomPathBtn);
            this.CustomInstallationPnl.Controls.Add(this.CustomPathTxt);
            this.CustomInstallationPnl.Location = new System.Drawing.Point(4, 228);
            this.CustomInstallationPnl.Name = "CustomInstallationPnl";
            this.CustomInstallationPnl.Size = new System.Drawing.Size(796, 70);
            this.CustomInstallationPnl.TabIndex = 8;
            this.CustomInstallationPnl.Visible = false;
            // 
            // DesktopShortTgl
            // 
            this.DesktopShortTgl.AutoSize = true;
            this.DesktopShortTgl.Checked = true;
            this.DesktopShortTgl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DesktopShortTgl.Location = new System.Drawing.Point(425, 30);
            this.DesktopShortTgl.Name = "DesktopShortTgl";
            this.DesktopShortTgl.Size = new System.Drawing.Size(107, 17);
            this.DesktopShortTgl.TabIndex = 8;
            this.DesktopShortTgl.Text = "Desktop shortcut";
            this.DesktopShortTgl.UseVisualStyleBackColor = true;
            // 
            // StartMenuTgl
            // 
            this.StartMenuTgl.AutoSize = true;
            this.StartMenuTgl.Checked = true;
            this.StartMenuTgl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StartMenuTgl.Location = new System.Drawing.Point(266, 30);
            this.StartMenuTgl.Name = "StartMenuTgl";
            this.StartMenuTgl.Size = new System.Drawing.Size(109, 17);
            this.StartMenuTgl.TabIndex = 7;
            this.StartMenuTgl.Text = "Add to start menu";
            this.StartMenuTgl.UseVisualStyleBackColor = true;
            // 
            // SelectCustomPathBtn
            // 
            this.SelectCustomPathBtn.Location = new System.Drawing.Point(538, 3);
            this.SelectCustomPathBtn.Name = "SelectCustomPathBtn";
            this.SelectCustomPathBtn.Size = new System.Drawing.Size(83, 23);
            this.SelectCustomPathBtn.TabIndex = 6;
            this.SelectCustomPathBtn.Text = "Select Folder";
            this.SelectCustomPathBtn.UseVisualStyleBackColor = true;
            // 
            // CustomPathTxt
            // 
            this.CustomPathTxt.Location = new System.Drawing.Point(267, 4);
            this.CustomPathTxt.Name = "CustomPathTxt";
            this.CustomPathTxt.ReadOnly = true;
            this.CustomPathTxt.Size = new System.Drawing.Size(265, 20);
            this.CustomPathTxt.TabIndex = 5;
            // 
            // MainContinueButton
            // 
            this.MainContinueButton.BackColor = System.Drawing.Color.Transparent;
            this.MainContinueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainContinueButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MainContinueButton.Location = new System.Drawing.Point(270, 147);
            this.MainContinueButton.Name = "MainContinueButton";
            this.MainContinueButton.Size = new System.Drawing.Size(265, 82);
            this.MainContinueButton.TabIndex = 7;
            this.MainContinueButton.Text = "Cool, let\'s go!";
            this.MainContinueButton.UseVisualStyleBackColor = false;
            this.MainContinueButton.Click += new System.EventHandler(this.MainContinueButton_Click);
            // 
            // MainInfoLbl
            // 
            this.MainInfoLbl.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainInfoLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MainInfoLbl.Location = new System.Drawing.Point(7, 68);
            this.MainInfoLbl.Name = "MainInfoLbl";
            this.MainInfoLbl.Size = new System.Drawing.Size(793, 77);
            this.MainInfoLbl.TabIndex = 6;
            this.MainInfoLbl.Text = "This installer will guide you through the installation of HabboGallery.";
            this.MainInfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HabboGalleryInstaller.Properties.Resources.InstallerLogo;
            this.pictureBox1.Location = new System.Drawing.Point(4, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(796, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // LaunchAppTgl
            // 
            this.LaunchAppTgl.AutoSize = true;
            this.LaunchAppTgl.Checked = true;
            this.LaunchAppTgl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LaunchAppTgl.Location = new System.Drawing.Point(541, 212);
            this.LaunchAppTgl.Name = "LaunchAppTgl";
            this.LaunchAppTgl.Size = new System.Drawing.Size(129, 17);
            this.LaunchAppTgl.TabIndex = 10;
            this.LaunchAppTgl.Text = "Launch HabboGallery";
            this.LaunchAppTgl.UseVisualStyleBackColor = true;
            this.LaunchAppTgl.Visible = false;
            // 
            // InstallFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(804, 300);
            this.Controls.Add(this.LaunchAppTgl);
            this.Controls.Add(this.CustomizeTgl);
            this.Controls.Add(this.CustomInstallationPnl);
            this.Controls.Add(this.MainContinueButton);
            this.Controls.Add(this.MainInfoLbl);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstallFrm";
            this.Text = "HabboGallery - Installer";
            this.CustomInstallationPnl.ResumeLayout(false);
            this.CustomInstallationPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog InstallationPathDlg;
        private System.Windows.Forms.CheckBox CustomizeTgl;
        private System.Windows.Forms.Panel CustomInstallationPnl;
        private System.Windows.Forms.CheckBox DesktopShortTgl;
        private System.Windows.Forms.CheckBox StartMenuTgl;
        private System.Windows.Forms.Button SelectCustomPathBtn;
        private System.Windows.Forms.TextBox CustomPathTxt;
        private System.Windows.Forms.Button MainContinueButton;
        private System.Windows.Forms.Label MainInfoLbl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox LaunchAppTgl;
    }
}

