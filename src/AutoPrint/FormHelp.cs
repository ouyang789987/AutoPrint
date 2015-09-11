using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AutoPrint
{
	/// <summary>
	/// This form displays the help in HTML format. It's nothign fancy, but it gets the 
	/// job done, and even better, allows screenshots.
	/// </summary>
	public partial class FormHelp : Form
	{

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
		public FormHelp()
		{
			InitializeComponent();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Loads the html, makes adjustments to the img tags (fully qualifies the 
		/// filename - this is absol;utely necessary in the WebBrowser control), and 
		/// displays the help page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormHelp_Load(object sender, EventArgs e)
		{
			// get our data file path (the parent form saved the help files to the program 
			// data folder (definitely necessary for Vista, and maybe for XP).
			string appDataFolder = System.IO.Path.GetDirectoryName(ProfileBitmap.GetDataFileName());

			bool valid = false;
			string htmlFile = System.IO.Path.Combine(appDataFolder, "AutoPrintHelp.html");
			StringBuilder htmlText = new StringBuilder("");

			// make sure the file exists before trying to load/modify it.
			if (File.Exists(htmlFile))
			{
				try
				{
					// read the file
					string rawHtml = "";
					using (StreamReader reader = new StreamReader(htmlFile))
					{
						rawHtml = reader.ReadToEnd();
					}

					// Replace the filenames specified in the image tags with fully 
					// qualified paths. The .Net  WebBrowser control will NOT display an 
					// image unless the filename is fully qualified.
					int start = -1;
					int stop = -1;
					do
					{
						// find the start of the next tag
					    start = rawHtml.IndexOf("<img ");
					    if (start >= 0)
					    {
							// while we're here, move everything BEFORE the start position 
							// into the stringbuilder.
							htmlText.Append(rawHtml.Substring(0, start));
							// find the end of the img tag
					        stop = rawHtml.IndexOf("\">", start);
							// extract it
					        string imageTag = rawHtml.Substring(start, stop+2-start);
							// replace the original file name with the fully qualified 
							// path/name
							string temp = imageTag;
							temp = temp.Replace("<img src=\"", "");
							temp = temp.Replace("\">", "");
							temp = System.IO.Path.Combine(appDataFolder, temp);
							imageTag = string.Format("<img src=\"{0}\">", temp);
							// append the newly constructed img tag to the string builder 
							// object
							htmlText.Append(imageTag);
							// shorten our raw text string 
							rawHtml = rawHtml.Substring(stop+2);
					    }
					}
					while (start >= 0);

					// add any remaining text from the raw html string to our string 
					// builder object
					htmlText.Append(rawHtml);

					// load up the WebBrowser control
					this.webBrowser1.Navigate("about:blank");
					HtmlDocument doc = this.webBrowser1.Document;
					doc.Write(string.Empty);
					this.webBrowser1.DocumentText = htmlText.ToString();

					// and we're out
					valid = true;
				}
				catch (Exception ex)
				{
					if (ex != null) {}
				}
			}
			// if we experienced a problem of some kind, display an error message.
			if (!valid)
			{
				MessageBox.Show("Could not find (and/or read) the help page file.", "Help File Error");
			}
		}

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
