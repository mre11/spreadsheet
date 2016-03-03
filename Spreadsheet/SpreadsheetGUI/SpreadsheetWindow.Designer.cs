namespace SSGui
{
    partial class SpreadsheetWindow
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.container = new System.Windows.Forms.Panel();
            this.spreadsheetPanel = new SSGui.SpreadsheetPanel();
            this.valueBox = new System.Windows.Forms.TextBox();
            this.contentsBox = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.setButton = new System.Windows.Forms.Button();
            this.selectedCellNameLabel = new System.Windows.Forms.Label();
            this.selectedCellValueLabel = new System.Windows.Forms.Label();
            this.selectedCellContentsLabel = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save As";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "Contents";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.helpContentsToolStripMenuItem_Click);
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(56, 24);
            this.nameBox.Name = "nameBox";
            this.nameBox.ReadOnly = true;
            this.nameBox.Size = new System.Drawing.Size(38, 20);
            this.nameBox.TabIndex = 2;
            this.nameBox.TabStop = false;
            this.toolTip.SetToolTip(this.nameBox, "Name");
            // 
            // container
            // 
            this.container.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.container.Controls.Add(this.spreadsheetPanel);
            this.container.Location = new System.Drawing.Point(0, 50);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(1264, 628);
            this.container.TabIndex = 3;
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.AutoSize = true;
            this.spreadsheetPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel.Location = new System.Drawing.Point(0, 0);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(1264, 628);
            this.spreadsheetPanel.TabIndex = 6;
            this.spreadsheetPanel.TabStop = false;
            // 
            // valueBox
            // 
            this.valueBox.Location = new System.Drawing.Point(143, 24);
            this.valueBox.Name = "valueBox";
            this.valueBox.ReadOnly = true;
            this.valueBox.Size = new System.Drawing.Size(186, 20);
            this.valueBox.TabIndex = 4;
            this.valueBox.TabStop = false;
            this.toolTip.SetToolTip(this.valueBox, "Value");
            // 
            // contentsBox
            // 
            this.contentsBox.AcceptsReturn = true;
            this.contentsBox.Location = new System.Drawing.Point(388, 24);
            this.contentsBox.Name = "contentsBox";
            this.contentsBox.Size = new System.Drawing.Size(506, 20);
            this.contentsBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.contentsBox, "Formula");
            this.contentsBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.contentsBox_KeyPress);
            // 
            // toolTip
            // 
            this.toolTip.ToolTipTitle = "SpreadsheetToolTip";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "ss";
            this.openFileDialog.Filter = "Spreadsheet files|*.ss|All files|*.*";
            this.openFileDialog.Title = "Open";
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.DefaultExt = "ss";
            this.saveAsFileDialog.Filter = "Spreadsheet files|*.ss|All files|*.*";
            this.saveAsFileDialog.Title = "Save As";
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(900, 22);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 1;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // selectedCellNameLabel
            // 
            this.selectedCellNameLabel.AutoSize = true;
            this.selectedCellNameLabel.Location = new System.Drawing.Point(12, 27);
            this.selectedCellNameLabel.Name = "selectedCellNameLabel";
            this.selectedCellNameLabel.Size = new System.Drawing.Size(38, 13);
            this.selectedCellNameLabel.TabIndex = 7;
            this.selectedCellNameLabel.Text = "Name:";
            // 
            // selectedCellValueLabel
            // 
            this.selectedCellValueLabel.AutoSize = true;
            this.selectedCellValueLabel.Location = new System.Drawing.Point(100, 27);
            this.selectedCellValueLabel.Name = "selectedCellValueLabel";
            this.selectedCellValueLabel.Size = new System.Drawing.Size(37, 13);
            this.selectedCellValueLabel.TabIndex = 8;
            this.selectedCellValueLabel.Text = "Value:";
            // 
            // selectedCellContentsLabel
            // 
            this.selectedCellContentsLabel.AutoSize = true;
            this.selectedCellContentsLabel.Location = new System.Drawing.Point(335, 27);
            this.selectedCellContentsLabel.Name = "selectedCellContentsLabel";
            this.selectedCellContentsLabel.Size = new System.Drawing.Size(47, 13);
            this.selectedCellContentsLabel.TabIndex = 9;
            this.selectedCellContentsLabel.Text = "Formula:";
            // 
            // SpreadsheetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.selectedCellContentsLabel);
            this.Controls.Add(this.selectedCellValueLabel);
            this.Controls.Add(this.selectedCellNameLabel);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.contentsBox);
            this.Controls.Add(this.valueBox);
            this.Controls.Add(this.container);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SpreadsheetWindow";
            this.Text = "- Spreadsheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetWindow_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.container.ResumeLayout(false);
            this.container.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Panel container;
        private SSGui.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.TextBox valueBox;
        private System.Windows.Forms.TextBox contentsBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.Label selectedCellNameLabel;
        private System.Windows.Forms.Label selectedCellValueLabel;
        private System.Windows.Forms.Label selectedCellContentsLabel;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
    }
}

