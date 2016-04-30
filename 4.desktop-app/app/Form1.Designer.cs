using System.ComponentModel;
using System.Windows.Forms;

namespace USB_Generic_HID_reference_application
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.usbToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelScopeTriggerRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.debugCollectionTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.flowLayoutPanelRadioChex = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scopeTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clockedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pulsedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fastToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.slowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMemParams = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxData.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usbToolStripStatusLabel,
            this.toolStripStatusLabelScopeTriggerRate});
            this.statusStrip1.Location = new System.Drawing.Point(0, 507);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(451, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // usbToolStripStatusLabel
            // 
            this.usbToolStripStatusLabel.Name = "usbToolStripStatusLabel";
            this.usbToolStripStatusLabel.Size = new System.Drawing.Size(155, 17);
            this.usbToolStripStatusLabel.Text = "USB Device Status Unknown";
            // 
            // toolStripStatusLabelScopeTriggerRate
            // 
            this.toolStripStatusLabelScopeTriggerRate.Name = "toolStripStatusLabelScopeTriggerRate";
            this.toolStripStatusLabelScopeTriggerRate.Size = new System.Drawing.Size(156, 17);
            this.toolStripStatusLabelScopeTriggerRate.Text = "Scope trigger: Clocked/Slow";
            // 
            // debugTextBox
            // 
            this.debugTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugTextBox.Font = new System.Drawing.Font("Courier New", 8F);
            this.debugTextBox.Location = new System.Drawing.Point(3, 16);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.debugTextBox.Size = new System.Drawing.Size(421, 80);
            this.debugTextBox.TabIndex = 21;
            // 
            // debugCollectionTimer
            // 
            this.debugCollectionTimer.Enabled = true;
            this.debugCollectionTimer.Interval = 50;
            this.debugCollectionTimer.Tick += new System.EventHandler(this.debugCollectionTimer_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.debugTextBox);
            this.groupBox2.Location = new System.Drawing.Point(15, 388);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(427, 99);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Device Debug Output";
            // 
            // groupBoxData
            // 
            this.groupBoxData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxData.Controls.Add(this.listBoxData);
            this.groupBoxData.Location = new System.Drawing.Point(12, 225);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(430, 160);
            this.groupBoxData.TabIndex = 23;
            this.groupBoxData.TabStop = false;
            this.groupBoxData.Text = "Data";
            // 
            // listBoxData
            // 
            this.listBoxData.AllowDrop = true;
            this.listBoxData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxData.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.IntegralHeight = false;
            this.listBoxData.ItemHeight = 14;
            this.listBoxData.Location = new System.Drawing.Point(3, 16);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.Size = new System.Drawing.Size(421, 141);
            this.listBoxData.TabIndex = 0;
            this.listBoxData.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxData_DragDrop);
            this.listBoxData.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxData_DragEnter);
            // 
            // flowLayoutPanelRadioChex
            // 
            this.flowLayoutPanelRadioChex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelRadioChex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelRadioChex.Location = new System.Drawing.Point(12, 142);
            this.flowLayoutPanelRadioChex.Name = "flowLayoutPanelRadioChex";
            this.flowLayoutPanelRadioChex.Size = new System.Drawing.Size(430, 77);
            this.flowLayoutPanelRadioChex.TabIndex = 24;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(451, 24);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDataFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openDataFileToolStripMenuItem
            // 
            this.openDataFileToolStripMenuItem.Name = "openDataFileToolStripMenuItem";
            this.openDataFileToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.openDataFileToolStripMenuItem.Text = "&Open data file";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scopeTriggerToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // scopeTriggerToolStripMenuItem
            // 
            this.scopeTriggerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockedToolStripMenuItem,
            this.pulsedToolStripMenuItem});
            this.scopeTriggerToolStripMenuItem.Name = "scopeTriggerToolStripMenuItem";
            this.scopeTriggerToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.scopeTriggerToolStripMenuItem.Text = "Scope trigger";
            // 
            // clockedToolStripMenuItem
            // 
            this.clockedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fastToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.slowToolStripMenuItem});
            this.clockedToolStripMenuItem.Name = "clockedToolStripMenuItem";
            this.clockedToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.clockedToolStripMenuItem.Text = "Clocked";
            // 
            // fastToolStripMenuItem
            // 
            this.fastToolStripMenuItem.Name = "fastToolStripMenuItem";
            this.fastToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.fastToolStripMenuItem.Tag = "0";
            this.fastToolStripMenuItem.Text = "Fast";
            this.fastToolStripMenuItem.Click += new System.EventHandler(this.ClockedScopeTrigger_Click);
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.mediumToolStripMenuItem.Tag = "1";
            this.mediumToolStripMenuItem.Text = "Medium";
            this.mediumToolStripMenuItem.Click += new System.EventHandler(this.ClockedScopeTrigger_Click);
            // 
            // slowToolStripMenuItem
            // 
            this.slowToolStripMenuItem.Name = "slowToolStripMenuItem";
            this.slowToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.slowToolStripMenuItem.Tag = "2";
            this.slowToolStripMenuItem.Text = "Slow";
            this.slowToolStripMenuItem.Click += new System.EventHandler(this.ClockedScopeTrigger_Click);
            // 
            // pulsedToolStripMenuItem
            // 
            this.pulsedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fastToolStripMenuItem1,
            this.mediumToolStripMenuItem1,
            this.slowToolStripMenuItem1});
            this.pulsedToolStripMenuItem.Name = "pulsedToolStripMenuItem";
            this.pulsedToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.pulsedToolStripMenuItem.Text = "Pulsed";
            // 
            // fastToolStripMenuItem1
            // 
            this.fastToolStripMenuItem1.Name = "fastToolStripMenuItem1";
            this.fastToolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.fastToolStripMenuItem1.Tag = "0";
            this.fastToolStripMenuItem1.Text = "Fast";
            this.fastToolStripMenuItem1.Click += new System.EventHandler(this.PulsedScopeTrigger_Click);
            // 
            // mediumToolStripMenuItem1
            // 
            this.mediumToolStripMenuItem1.Name = "mediumToolStripMenuItem1";
            this.mediumToolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.mediumToolStripMenuItem1.Tag = "1";
            this.mediumToolStripMenuItem1.Text = "Medium";
            this.mediumToolStripMenuItem1.Click += new System.EventHandler(this.PulsedScopeTrigger_Click);
            // 
            // slowToolStripMenuItem1
            // 
            this.slowToolStripMenuItem1.Name = "slowToolStripMenuItem1";
            this.slowToolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.slowToolStripMenuItem1.Tag = "2";
            this.slowToolStripMenuItem1.Text = "Slow";
            this.slowToolStripMenuItem1.Click += new System.EventHandler(this.PulsedScopeTrigger_Click);
            // 
            // panelMemParams
            // 
            this.panelMemParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMemParams.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMemParams.Location = new System.Drawing.Point(12, 37);
            this.panelMemParams.Name = "panelMemParams";
            this.panelMemParams.Size = new System.Drawing.Size(430, 99);
            this.panelMemParams.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 529);
            this.Controls.Add(this.flowLayoutPanelRadioChex);
            this.Controls.Add(this.groupBoxData);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelMemParams);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(467, 567);
            this.Name = "Form1";
            this.Text = "Genie Tickler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxData.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel usbToolStripStatusLabel;
        private TextBox debugTextBox;
        private Timer debugCollectionTimer;
        private GroupBox groupBox2;
        private GroupBox groupBoxData;
        private ListBox listBoxData;
        private FlowLayoutPanel flowLayoutPanelRadioChex;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openDataFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem scopeTriggerToolStripMenuItem;
        private ToolStripMenuItem clockedToolStripMenuItem;
        private ToolStripMenuItem fastToolStripMenuItem;
        private ToolStripMenuItem mediumToolStripMenuItem;
        private ToolStripMenuItem slowToolStripMenuItem;
        private ToolStripMenuItem pulsedToolStripMenuItem;
        private ToolStripMenuItem fastToolStripMenuItem1;
        private ToolStripMenuItem mediumToolStripMenuItem1;
        private ToolStripMenuItem slowToolStripMenuItem1;
        private ToolStripStatusLabel toolStripStatusLabelScopeTriggerRate;
        private Panel panelMemParams;
    }
}

