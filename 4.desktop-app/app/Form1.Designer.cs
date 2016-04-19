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
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.debugCollectionTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.flowLayoutPanelRadioChex = new System.Windows.Forms.FlowLayoutPanel();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxData.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usbToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(587, 22);
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
            // debugTextBox
            // 
            this.debugTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugTextBox.Font = new System.Drawing.Font("Courier New", 8F);
            this.debugTextBox.Location = new System.Drawing.Point(3, 16);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.debugTextBox.Size = new System.Drawing.Size(557, 80);
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
            this.groupBox2.Location = new System.Drawing.Point(12, 350);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(563, 99);
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
            this.groupBoxData.Location = new System.Drawing.Point(12, 181);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(563, 163);
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
            this.listBoxData.Size = new System.Drawing.Size(554, 144);
            this.listBoxData.TabIndex = 0;
            this.listBoxData.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxData_DragDrop);
            this.listBoxData.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxData_DragEnter);
            // 
            // flowLayoutPanelRadioChex
            // 
            this.flowLayoutPanelRadioChex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelRadioChex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelRadioChex.Location = new System.Drawing.Point(12, 95);
            this.flowLayoutPanelRadioChex.Name = "flowLayoutPanelRadioChex";
            this.flowLayoutPanelRadioChex.Size = new System.Drawing.Size(563, 79);
            this.flowLayoutPanelRadioChex.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 488);
            this.Controls.Add(this.flowLayoutPanelRadioChex);
            this.Controls.Add(this.groupBoxData);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(603, 526);
            this.Name = "Form1";
            this.Text = "USB Generic HID reference application";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxData.ResumeLayout(false);
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
    }
    }

