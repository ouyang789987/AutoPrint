using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing; 
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace AutoPrint
{
	
	public partial class Form1 : Form
	{
		private VerticalLabel		m_vertLabel		= new VerticalLabel();
		private Bitmap				m_vertLabelBmp	= null;

		//--------------------------------------------------------------------------------
		/// <summary>
		///  Constructor
		/// </summary>
		public Form1()
		{
			InitializeComponent();

			// initialize our vertical label picture box
			m_vertLabel.Size			= new Size(this.pictureboxSideLabel.Height, this.pictureboxSideLabel.Width);
			m_vertLabel.Location		= new Point(0,0);
			m_vertLabel.Font			= new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			m_vertLabel.TextAlign		= System.Drawing.ContentAlignment.MiddleCenter;
			m_vertLabel.BackColor		= System.Drawing.SystemColors.MenuBar;
			m_vertLabel.ForeColor		= System.Drawing.SystemColors.MenuText;
			m_vertLabel.BorderStyle		= System.Windows.Forms.BorderStyle.Fixed3D;
			m_vertLabel.Text			= "Colors To Print";
			m_vertLabel.Visible			= true;
			m_vertLabel.CreateLabelBitmap();
			m_vertLabelBmp				= m_vertLabel.Bitmap;
			this.pictureboxSideLabel.Image = m_vertLabel.Bitmap;

			DiscoverPrinters();

		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the form loads.  Sets the tooltip text.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			this.tooltipInstalledPrinters.SetToolTip	(this.listboxPrinters,		"Select a printer from this list to populate the list\nof colors to print and enable buttons.");
			this.tooltipColorsToPrint.SetToolTip		(this.listOfColors,				"Select a color for each color cartridge in your printer\n (some are already selected).");
			this.tooltipButtonPreview.SetToolTip		(this.buttonPreview,			"Preview where the next nozzle check bitmap will be printed.");
			this.tooltipButtonPreviewExtended.SetToolTip(this.buttonPreviewExtended,	"Preview a full page of nozzl-check images.");
			this.tooltipResetPosition.SetToolTip		(this.checkboxResetPosition,	"Resets the position of the next nozzle check image to the \ntop-left corner of the printed page. Check \nthis BEFORE clicking the Print button if you want \nto reposition the image. After printing, \nthis checkbox will un-check itself.");
			this.tooltipButtonPrintSelected.SetToolTip	(this.buttonPrint,				"Print the nozzle check image on the selected printer.");
			this.tooltipButtonUsedSavedColors.SetToolTip(this.checkboxUseSavedColors,	"When the cmd file is created, it will run this application \nin such a way as to load the colors saved \nin the settings file.");
			this.tooltipButtonSaveColors.SetToolTip		(this.buttonSaveColors,			"Saves the selected colors, and keeps you from having to \nre-select them every time.");
			this.tooltipButtonMakeCmdFile.SetToolTip	(this.buttonMakeCmdFile,		"Creates a .cmd file for use by the Windows scheduler.");
			this.tooltipButtonDone.SetToolTip			(this.buttonDone,				"Terminates this program.");
		}

		#region Button handlers
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Preview button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonPreview_Click(object sender, EventArgs e)
		{
			PreviewCommon(false);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Preview Extended button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonPreviewExtended_Click(object sender, EventArgs e)
		{
			PreviewCommon(true);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Print button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonPrint_Click(object sender, EventArgs e)
		{
			if (this.listboxPrinters.SelectedIndex == -1)
			{
				MessageBox.Show("No printer selected. Print/preview action terminated.");
				return;
			}
			ColorBarFlags bars = GetSelectedColors();
			if (bars == ColorBarFlags.None)
			{
				MessageBox.Show("No colors have been selected. Print/preview action terminated.");
				return;
			}

			ProfileBitmap profile = (ProfileBitmap)this.listboxPrinters.SelectedItem;
			if (this.checkboxResetPosition.Checked)
			{
				profile.ResetPosition();
			}
			profile.ColorBars = bars;
			profile.PrintBitmap();
			this.checkboxResetPosition.Checked = false;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Save Colors button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonSaveColors_Click(object sender, EventArgs e)
		{
			ProfileBitmap profile = (ProfileBitmap)this.listboxPrinters.SelectedItem;
			if (profile != null)
			{
				profile.SaveSettings();
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Make Cmd File button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonMakeCmdFile_Click(object sender, EventArgs e)
		{
			string name		= this.listboxPrinters.SelectedItem.ToString();
			string colors	= Utility.MakeColorString(GetSelectedColors());
			string command	= "";
			string exeFile  = System.IO.Path.GetFileName(Application.ExecutablePath);

			// determine which commandline we need
			if (this.checkboxUseSavedColors.Checked)
			{
				command = string.Format("{0} config=\"{1}\"", exeFile, name.Replace(" ", "_"));
			}
			else
			{
				command = string.Format("{0} name=\"{1}\" colors=\"{2}\"", exeFile, name, colors);
			}

			// establish the path/name of the cmd file
			string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ProfileBitmap.GetDataFileName()), name.Replace(" ", "_") + ".cmd");

			try
			{
				// See if the file exists and if so, prompt the user to see if he wants 
				// to replace it.
				if (File.Exists(path))
				{
					if (MessageBox.Show(string.Format("The file {0} already exists. Overwrite?", path), "Cmd File Exists", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					File.Delete(path);
				}

				// Write the file - we need to change to the appropriate drive, and then 
				// to the appropriate folder before trying to execute this application.
				using (StreamWriter stream = new StreamWriter(path))
				{
					stream.WriteLine(string.Format("{0}", Application.ExecutablePath.Substring(0,2)));
					stream.WriteLine(string.Format("cd {0}", System.IO.Path.GetDirectoryName(Application.ExecutablePath)));
					stream.WriteLine(command);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("File could not be written.\n\n{0}\n\n{1}", ex.Message, ex.StackTrace), "Exception Encountered");
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Done button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonDone_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		#endregion Button handlers

	
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when a printer is selected in the list of installed printers.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkedListPrinters_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = this.listboxPrinters.SelectedIndex;
			if (index >= 0)
			{
				PopulateColorList((ProfileBitmap)this.listboxPrinters.SelectedItem);
				buttonPreview.Enabled			= true;
				buttonPreviewExtended.Enabled	= true;
				buttonMakeCmdFile.Enabled		= true;
				buttonPrint.Enabled				= true;
				buttonSaveColors.Enabled		= true;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the form is in the process of closing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.listboxPrinters.Items.Clear();
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when an item in the Colors To Print list is checked/unchecked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listOfColors_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			int index = this.listboxPrinters.SelectedIndex;
			if (index >= 0)
			{
				ColorBarFlags color;
				switch (e.Item.Index)
				{
					case 0 : color = ColorBarFlags.Black; break;
					case 1 : color = ColorBarFlags.LightBlack; break;
					case 2 : color = ColorBarFlags.Cyan; break;
					case 3 : color = ColorBarFlags.LightCyan; break;
					case 4 : color = ColorBarFlags.Yellow; break;
					case 5 : color = ColorBarFlags.Magenta; break;
					case 6 : color = ColorBarFlags.LightMagenta; break;
					case 7 : color = ColorBarFlags.Red; break;
					case 8 : color = ColorBarFlags.Green; break;
					case 9 : color = ColorBarFlags.Blue; break;
					default : color = ColorBarFlags.None; break;
				}
				if (color != ColorBarFlags.None)
				{
					ProfileBitmap profile = (ProfileBitmap)this.listboxPrinters.SelectedItem;
					if (e.Item.Checked)
					{
						profile.ColorBars |= color;
					}
					else
					{
						profile.ColorBars |= ~color;
					}
				}
			}			
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the side label needs to paint itself.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureboxSideLabel_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(this.pictureboxSideLabel.BackColor);
		    e.Graphics.TranslateTransform(0, m_vertLabel.Size.Width);
		    e.Graphics.RotateTransform(270);
		    int x = ((int)((this.pictureboxSideLabel.Size.Height - m_vertLabel.Size.Width) * 0.5) * -1);
		    e.Graphics.DrawImage(m_vertLabelBmp, new Point(x, 0));
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Enumerates installed printers, creates their profiles, and adds them to the 
		/// list of installed printers.
		/// </summary>
		private void DiscoverPrinters()
		{
			foreach (string printerName in PrinterSettings.InstalledPrinters)
			{
				if (!printerName.Contains("Microsoft") && !printerName.Contains("Fax"))
				{
					PrinterSettings printer = new PrinterSettings();
					printer.PrinterName = printerName;
					if (printer.IsValid)
					{
						ProfileBitmap profile = new ProfileBitmap(printerName, ColorBarFlags.Black);
						if (profile.SupportsColor)
						{
							profile.ColorBars |= (ColorBarFlags.Cyan|ColorBarFlags.Yellow|ColorBarFlags.Magenta);
						}
						this.listboxPrinters.Items.Add(profile);
					}
				}
			}
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Populates the Colors To Print list with the appropriate set of colors.
		/// </summary>
		/// <param name="profile">The selected printer profile</param>
		private void PopulateColorList(ProfileBitmap profile)
		{
			// Remove the ItemChecked handler for the color listbox so we don't fire 
			// that event during the population of the listbox. We do this because as 
			// we populate the listbox when a printer is selected, it will fire the 
			// ItemChecked event as we check colors that are selected in the 
			// ProfileBitmap object.
			this.listOfColors.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.listOfColors_ItemChecked);
			listOfColors.BeginUpdate();
			listOfColors.Clear();
			if (imageListSmall.Images.Count == 0)
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.Black.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.LightBlack.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.Cyan.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.LightCyan.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.Yellow.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.Magenta.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.LightMagenta.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.Red.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.Green.ico")));
				imageListSmall.Images.Add(new Icon(assembly.GetManifestResourceStream("AutoPrint.Resources.Blue.ico")));
			}
			listOfColors.Items.Add(new ListViewItem("Black", 0));
			if (profile.SupportsColor)
			{
				listOfColors.Items.Add(new ListViewItem("Light Black", 1));
				listOfColors.Items.Add(new ListViewItem("Cyan", 2));
				listOfColors.Items.Add(new ListViewItem("Light Cyan", 3));
				listOfColors.Items.Add(new ListViewItem("Yellow", 4));
				listOfColors.Items.Add(new ListViewItem("Magenta", 5));
				listOfColors.Items.Add(new ListViewItem("Light Magenta", 6));
				listOfColors.Items.Add(new ListViewItem("Red", 7));
				listOfColors.Items.Add(new ListViewItem("Green", 8));
				listOfColors.Items.Add(new ListViewItem("Blue", 9));
				listOfColors.Items[0].Checked = (profile.ColorIsSet(ColorBarFlags.Black));
				listOfColors.Items[1].Checked = (profile.ColorIsSet(ColorBarFlags.LightBlack));
				listOfColors.Items[2].Checked = (profile.ColorIsSet(ColorBarFlags.Cyan));
				listOfColors.Items[3].Checked = (profile.ColorIsSet(ColorBarFlags.LightCyan));
				listOfColors.Items[4].Checked = (profile.ColorIsSet(ColorBarFlags.Yellow));
				listOfColors.Items[5].Checked = (profile.ColorIsSet(ColorBarFlags.Magenta));
				listOfColors.Items[6].Checked = (profile.ColorIsSet(ColorBarFlags.LightMagenta));
				listOfColors.Items[7].Checked = (profile.ColorIsSet(ColorBarFlags.Red));
				listOfColors.Items[8].Checked = (profile.ColorIsSet(ColorBarFlags.Green));
				listOfColors.Items[9].Checked = (profile.ColorIsSet(ColorBarFlags.Blue));
				listOfColors.Enabled = true;

			}
			else
			{
				listOfColors.Items[0].Checked = true;
				listOfColors.Enabled = false;
			}
			listOfColors.EndUpdate();
			// re-add the ItemChecked handler for the color listbox so we get events 
			// when an item is checked in the listbox. 
			this.listOfColors.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listOfColors_ItemChecked);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Gets the selected (checked) colors from the Colors To Print list control.
		/// </summary>
		/// <returns>The selected colors</returns>
		private ColorBarFlags GetSelectedColors()
		{
			ColorBarFlags bars = ColorBarFlags.None;
			bars |= (listOfColors.Items[0].Checked) ? ColorBarFlags.Black		: 0;
			if (listOfColors.Items.Count > 1)
			{
				bars |= (listOfColors.Items[1].Checked) ? ColorBarFlags.LightBlack	: 0;
				bars |= (listOfColors.Items[2].Checked) ? ColorBarFlags.Cyan		: 0;
				bars |= (listOfColors.Items[3].Checked) ? ColorBarFlags.LightCyan	: 0;
				bars |= (listOfColors.Items[4].Checked) ? ColorBarFlags.Yellow		: 0;
				bars |= (listOfColors.Items[5].Checked) ? ColorBarFlags.Magenta		: 0;
				bars |= (listOfColors.Items[6].Checked) ? ColorBarFlags.LightMagenta: 0;
				bars |= (listOfColors.Items[7].Checked) ? ColorBarFlags.Red			: 0;
				bars |= (listOfColors.Items[8].Checked) ? ColorBarFlags.Green		: 0;
				bars |= (listOfColors.Items[9].Checked) ? ColorBarFlags.Blue		: 0;
			}
			return bars;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Common preview-related code
		/// </summary>
		/// <param name="extended"></param>
		private void PreviewCommon(bool extended)
		{
			if (this.listboxPrinters.SelectedIndex == -1)
			{
				MessageBox.Show("No printer selected. Print/preview action terminated.");
				return;
			}
			ColorBarFlags bars = GetSelectedColors();
			if (bars == ColorBarFlags.None)
			{
				MessageBox.Show("No colors have been selected. Print/preview action terminated.");
				return;
			}
			ProfileBitmap profile = (ProfileBitmap)this.listboxPrinters.SelectedItem;
			profile.ColorBars = bars;
			profile.IsExtendedPreview = extended;
			printPreviewDialog.Document = profile.PrintDocument;
			printPreviewDialog.ShowDialog();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the user clicks the Help menu item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ExtractHelpResources())
			{
				FormHelp form = new FormHelp();
				form.ShowDialog();
			}
			else
			{
				MessageBox.Show("Help Resources could not be extracted from the exe file. Help is not avaiilable.", "Help Not Available");
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the user clicks the About menu item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
            AboutBox1 form = new AboutBox1();
            form.ShowDialog();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Extracts the necessary help resource files to the ProgramData folder.
		/// </summary>
		/// <returns>True if all files were successfully extracted</returns>
		private bool ExtractHelpResources()
		{
			bool extracted = false;

			// Vista won't allow you to modify folder contents in the Program Files 
			// folder, so we have to extract these files to the ProgramData folder.
			string appPath  = System.IO.Path.GetDirectoryName(ProfileBitmap.GetDataFileName());

			// build the fully qualified filenames for our extracted resources
			string helpFileName = System.IO.Path.Combine(appPath, "AutoPrintHelp.html");
			string image1Name = System.IO.Path.Combine(appPath, "screenshot01.png");
			string image2Name = System.IO.Path.Combine(appPath, "screenshot02.png");

			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				ResourceManager rm = new ResourceManager("AutoPrint.Resources", assembly);

				string buffer = "";
				Bitmap bmpBuffer = null;
				// extract screenshot 1
				if (!File.Exists(image1Name))
				{
					bmpBuffer = (Bitmap)rm.GetObject("screenshot01");
					bmpBuffer.Save(image1Name);
					extracted = true;
				}
				else
				{
					extracted = true;
				}

				// extract screenshot 2
				if (!File.Exists(image2Name))
				{
					bmpBuffer = (Bitmap)rm.GetObject("screenshot02");
					bmpBuffer.Save(image2Name);
					extracted = true;
				}
				else
				{
					extracted = true;
				}

				// extract the html file
				if (!File.Exists(helpFileName))
				{
					buffer = (string)rm.GetObject("AutoPrintHelp");
					using (StreamWriter stream = new StreamWriter(helpFileName))
					{
						stream.WriteLine(buffer);
						extracted = true;
					}
				}
				else
				{
					extracted = true;
				}

			}
			catch (Exception ex)
			{
				if (ex != null) {}
				extracted = false;
			}

			return extracted;
		}

        private void pictureboxSideLabel_Click(object sender, EventArgs e)
        {

        }

        private void checkboxResetPosition_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string appdatafolder = System.IO.Path.GetDirectoryName(ProfileBitmap.GetDataFileName());
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = windir + @"\explorer.exe";
            prc.StartInfo.Arguments = appdatafolder;
            prc.Start();
        }

        private void tooltipButtonMakeCmdFile_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}

