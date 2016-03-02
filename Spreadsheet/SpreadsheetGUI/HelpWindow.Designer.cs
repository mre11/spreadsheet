using System.IO;

namespace SSGui
{
    partial class HelpWindow
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
            this.helpBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // helpBrowser
            // 
            this.helpBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpBrowser.Location = new System.Drawing.Point(0, 0);
            this.helpBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.helpBrowser.Name = "helpBrowser";
            this.helpBrowser.Size = new System.Drawing.Size(624, 681);
            this.helpBrowser.TabIndex = 0;
            string currentDirectory = Directory.GetCurrentDirectory();
            this.helpBrowser.Url = new System.Uri("file://" + currentDirectory + "/contents.html", System.UriKind.Absolute);
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 681);
            this.Controls.Add(this.helpBrowser);
            this.Name = "Help";
            this.Text = "Help Contents";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser helpBrowser;
    }
}