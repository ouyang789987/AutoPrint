namespace AutoPrint
{
	partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonPreviewExtended = new System.Windows.Forms.Button();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.listOfColors = new System.Windows.Forms.ListView();
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.checkboxResetPosition = new System.Windows.Forms.CheckBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.buttonMakeCmdFile = new System.Windows.Forms.Button();
            this.buttonSaveColors = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkboxUseSavedColors = new System.Windows.Forms.CheckBox();
            this.pictureboxSideLabel = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listboxPrinters = new System.Windows.Forms.ListBox();
            this.tooltipInstalledPrinters = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipColorsToPrint = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipButtonPreview = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipButtonPreviewExtended = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipResetPosition = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipButtonPrintSelected = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipButtonSaveColors = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipButtonUsedSavedColors = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipButtonMakeCmdFile = new System.Windows.Forms.ToolTip(this.components);
            this.tooltipButtonDone = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxSideLabel)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPrint
            // 
            this.buttonPrint.Enabled = false;
            this.buttonPrint.Location = new System.Drawing.Point(274, 100);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(112, 23);
            this.buttonPrint.TabIndex = 4;
            this.buttonPrint.Text = "Print";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Enabled = false;
            this.buttonPreview.Location = new System.Drawing.Point(274, 19);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(112, 23);
            this.buttonPreview.TabIndex = 1;
            this.buttonPreview.Text = "Preview...";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonPreviewExtended
            // 
            this.buttonPreviewExtended.Enabled = false;
            this.buttonPreviewExtended.Location = new System.Drawing.Point(274, 48);
            this.buttonPreviewExtended.Name = "buttonPreviewExtended";
            this.buttonPreviewExtended.Size = new System.Drawing.Size(112, 23);
            this.buttonPreviewExtended.TabIndex = 2;
            this.buttonPreviewExtended.Text = "Preview Extended...";
            this.buttonPreviewExtended.UseVisualStyleBackColor = true;
            this.buttonPreviewExtended.Click += new System.EventHandler(this.buttonPreviewExtended_Click);
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // listOfColors
            // 
            this.listOfColors.CheckBoxes = true;
            this.listOfColors.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listOfColors.Location = new System.Drawing.Point(36, 24);
            this.listOfColors.MultiSelect = false;
            this.listOfColors.Name = "listOfColors";
            this.listOfColors.Size = new System.Drawing.Size(137, 195);
            this.listOfColors.SmallImageList = this.imageListSmall;
            this.listOfColors.TabIndex = 0;
            this.listOfColors.UseCompatibleStateImageBehavior = false;
            this.listOfColors.View = System.Windows.Forms.View.List;
            this.listOfColors.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listOfColors_ItemChecked);
            // 
            // imageListSmall
            // 
            this.imageListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // checkboxResetPosition
            // 
            this.checkboxResetPosition.AutoSize = true;
            this.checkboxResetPosition.BackColor = System.Drawing.Color.Transparent;
            this.checkboxResetPosition.Location = new System.Drawing.Point(278, 77);
            this.checkboxResetPosition.Name = "checkboxResetPosition";
            this.checkboxResetPosition.Size = new System.Drawing.Size(93, 17);
            this.checkboxResetPosition.TabIndex = 3;
            this.checkboxResetPosition.Text = "Reset position";
            this.checkboxResetPosition.UseVisualStyleBackColor = false;
            this.checkboxResetPosition.CheckedChanged += new System.EventHandler(this.checkboxResetPosition_CheckedChanged);
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(147, 425);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(112, 23);
            this.buttonDone.TabIndex = 0;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // buttonMakeCmdFile
            // 
            this.buttonMakeCmdFile.Enabled = false;
            this.buttonMakeCmdFile.Location = new System.Drawing.Point(274, 181);
            this.buttonMakeCmdFile.Name = "buttonMakeCmdFile";
            this.buttonMakeCmdFile.Size = new System.Drawing.Size(112, 23);
            this.buttonMakeCmdFile.TabIndex = 7;
            this.buttonMakeCmdFile.Text = "Make Cmd File";
            this.buttonMakeCmdFile.UseVisualStyleBackColor = true;
            this.buttonMakeCmdFile.Click += new System.EventHandler(this.buttonMakeCmdFile_Click);
            // 
            // buttonSaveColors
            // 
            this.buttonSaveColors.Enabled = false;
            this.buttonSaveColors.Location = new System.Drawing.Point(274, 129);
            this.buttonSaveColors.Name = "buttonSaveColors";
            this.buttonSaveColors.Size = new System.Drawing.Size(112, 23);
            this.buttonSaveColors.TabIndex = 5;
            this.buttonSaveColors.Text = "Save Colors";
            this.buttonSaveColors.UseVisualStyleBackColor = true;
            this.buttonSaveColors.Click += new System.EventHandler(this.buttonSaveColors_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.checkboxUseSavedColors);
            this.groupBox1.Controls.Add(this.pictureboxSideLabel);
            this.groupBox1.Controls.Add(this.buttonMakeCmdFile);
            this.groupBox1.Controls.Add(this.buttonSaveColors);
            this.groupBox1.Controls.Add(this.listOfColors);
            this.groupBox1.Controls.Add(this.buttonPrint);
            this.groupBox1.Controls.Add(this.buttonPreview);
            this.groupBox1.Controls.Add(this.buttonPreviewExtended);
            this.groupBox1.Controls.Add(this.checkboxResetPosition);
            this.groupBox1.Location = new System.Drawing.Point(8, 167);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 252);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "For Currently Selected Printer";
            // 
            // checkboxUseSavedColors
            // 
            this.checkboxUseSavedColors.AutoSize = true;
            this.checkboxUseSavedColors.BackColor = System.Drawing.Color.Transparent;
            this.checkboxUseSavedColors.Location = new System.Drawing.Point(274, 158);
            this.checkboxUseSavedColors.Name = "checkboxUseSavedColors";
            this.checkboxUseSavedColors.Size = new System.Drawing.Size(108, 17);
            this.checkboxUseSavedColors.TabIndex = 6;
            this.checkboxUseSavedColors.Text = "Use saved colors";
            this.checkboxUseSavedColors.UseVisualStyleBackColor = false;
            // 
            // pictureboxSideLabel
            // 
            this.pictureboxSideLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureboxSideLabel.Location = new System.Drawing.Point(8, 24);
            this.pictureboxSideLabel.Margin = new System.Windows.Forms.Padding(0);
            this.pictureboxSideLabel.Name = "pictureboxSideLabel";
            this.pictureboxSideLabel.Size = new System.Drawing.Size(29, 195);
            this.pictureboxSideLabel.TabIndex = 14;
            this.pictureboxSideLabel.TabStop = false;
            this.pictureboxSideLabel.Click += new System.EventHandler(this.pictureboxSideLabel_Click);
            this.pictureboxSideLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureboxSideLabel_Paint);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listboxPrinters);
            this.groupBox2.Location = new System.Drawing.Point(8, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 87);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Installed Printers";
            // 
            // listboxPrinters
            // 
            this.listboxPrinters.FormattingEnabled = true;
            this.listboxPrinters.IntegralHeight = false;
            this.listboxPrinters.Location = new System.Drawing.Point(9, 19);
            this.listboxPrinters.Name = "listboxPrinters";
            this.listboxPrinters.Size = new System.Drawing.Size(377, 58);
            this.listboxPrinters.TabIndex = 0;
            this.listboxPrinters.SelectedIndexChanged += new System.EventHandler(this.checkedListPrinters_SelectedIndexChanged);
            // 
            // tooltipInstalledPrinters
            // 
            this.tooltipInstalledPrinters.AutoPopDelay = 10000;
            this.tooltipInstalledPrinters.InitialDelay = 500;
            this.tooltipInstalledPrinters.ReshowDelay = 100;
            // 
            // tooltipColorsToPrint
            // 
            this.tooltipColorsToPrint.AutoPopDelay = 15000;
            this.tooltipColorsToPrint.InitialDelay = 500;
            this.tooltipColorsToPrint.ReshowDelay = 100;
            // 
            // tooltipButtonMakeCmdFile
            // 
            this.tooltipButtonMakeCmdFile.Popup += new System.Windows.Forms.PopupEventHandler(this.tooltipButtonMakeCmdFile_Popup);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(409, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(409, 2);
            this.label1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(395, 42);
            this.label2.TabIndex = 0;
            this.label2.Text = "If the name of your printer contains any of the following characters: \" ! “ # $ %" +
    " & ‘ ( ) * + , - . / : ; < = > ? @ { | } ~ [ \\ ] ^** \" Please remove them, as it" +
    " may crash AutoPrint!";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(274, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 36);
            this.button1.TabIndex = 15;
            this.button1.Text = "Open ProgramData Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 460);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(425, 499);
            this.Name = "Form1";
            this.Text = "AutoPrint";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxSideLabel)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonPrint;
		private System.Windows.Forms.Button buttonPreview;
		private System.Windows.Forms.Button buttonPreviewExtended;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
		private System.Windows.Forms.ListView listOfColors;
		private System.Windows.Forms.ImageList imageListSmall;
		private System.Windows.Forms.CheckBox checkboxResetPosition;
		private System.Windows.Forms.Button buttonDone;
		private System.Windows.Forms.Button buttonMakeCmdFile;
		private System.Windows.Forms.Button buttonSaveColors;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pictureboxSideLabel;
		private System.Windows.Forms.CheckBox checkboxUseSavedColors;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox listboxPrinters;
		private System.Windows.Forms.ToolTip tooltipInstalledPrinters;
		private System.Windows.Forms.ToolTip tooltipColorsToPrint;
		private System.Windows.Forms.ToolTip tooltipButtonPreview;
		private System.Windows.Forms.ToolTip tooltipButtonPreviewExtended;
		private System.Windows.Forms.ToolTip tooltipResetPosition;
		private System.Windows.Forms.ToolTip tooltipButtonPrintSelected;
		private System.Windows.Forms.ToolTip tooltipButtonSaveColors;
		private System.Windows.Forms.ToolTip tooltipButtonUsedSavedColors;
		private System.Windows.Forms.ToolTip tooltipButtonMakeCmdFile;
		private System.Windows.Forms.ToolTip tooltipButtonDone;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}

