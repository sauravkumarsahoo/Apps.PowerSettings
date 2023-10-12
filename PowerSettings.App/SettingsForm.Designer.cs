namespace PowerSettings
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.startWithWindowsToggle = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // startWithWindowsToggle
            // 
            this.startWithWindowsToggle.AutoSize = true;
            this.startWithWindowsToggle.Location = new System.Drawing.Point(24, 24);
            this.startWithWindowsToggle.Name = "startWithWindowsToggle";
            this.startWithWindowsToggle.Size = new System.Drawing.Size(128, 19);
            this.startWithWindowsToggle.TabIndex = 1;
            this.startWithWindowsToggle.Text = "Start with Windows";
            this.startWithWindowsToggle.UseVisualStyleBackColor = true;
            this.startWithWindowsToggle.CheckedChanged += new System.EventHandler(this.startWithWindowsToggle_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 71);
            this.Controls.Add(this.startWithWindowsToggle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(261, 110);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(261, 110);
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Power Settings Configuration";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CheckBox startWithWindowsToggle;
    }
}