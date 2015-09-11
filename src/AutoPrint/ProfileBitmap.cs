using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing; 
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;

//using OLEPRNLib;

namespace AutoPrint
{
	[Flags]
	public enum ColorBarFlags { None=0, Black=1, Cyan=2, Magenta=4, Yellow=8, LightBlack=16, LightCyan=32, LightMagenta=64, Red=128, Green=256, Blue=512, All=1024}; 

	public class ProfileBitmap : IDisposable
	{
		private const float		m_barGap = 2.0f;
		#region Data members
		private bool			m_disposed;
		private string			m_printerName;
		private Font			m_font;
		private ColorBarFlags	m_colorBars;
		private int				m_colorsUsed;
		private PrintDocument	m_printDoc;
		private int				m_currentX;
		private int				m_currentY;
		private bool			m_firstPosition;
		private bool			m_isExtendedPreview;
		private bool			m_supportsColor;
		private string			m_ipAddress;
		#endregion Data members

		#region Properties
		//--------------------------------------------------------------------------------
		public bool Disposed
		{
			get { return m_disposed; }
			set { m_disposed = value; }
		}
		//--------------------------------------------------------------------------------
		public string PrinterName
		{
			get { return m_printerName; }
		}
		//--------------------------------------------------------------------------------
		public PrintDocument PrintDocument
		{
			get { return m_printDoc; }
		}
		//--------------------------------------------------------------------------------
		public bool IsExtendedPreview
		{
			get { return m_isExtendedPreview; }
			set { m_isExtendedPreview = value; }
		}
		//--------------------------------------------------------------------------------
		public ColorBarFlags ColorBars
		{
			get { return m_colorBars; }
			set { m_colorBars = value; }
		}
		//--------------------------------------------------------------------------------
		public bool SupportsColor
		{
			get { return m_supportsColor; }
		}
		//--------------------------------------------------------------------------------
		public string IPAddress
		{
			get { return m_ipAddress; }
			set { m_ipAddress = value; }
		}
		#endregion Properties

		//--------------------------------------------------------------------------------
		public ProfileBitmap(string configSection)
		{
			CommonInit();

			m_printDoc.PrinterSettings.PrinterName = configSection.Replace("_"," ");
			m_printerName = m_printDoc.PrinterSettings.PrinterName;
			m_supportsColor = m_printDoc.PrinterSettings.SupportsColor;

			bool isOnline = this.PrinterIsOnline();
			// Count the colors we'll be using
			CountColorsUsed();
			LoadSettings();
		}

		//--------------------------------------------------------------------------------
		public ProfileBitmap(string printerName, ColorBarFlags colorBars)
		{
			CommonInit();

			m_printerName		= printerName;
			m_colorBars			= colorBars;

			m_printDoc.PrinterSettings.PrinterName = printerName;
			m_supportsColor		= m_printDoc.PrinterSettings.SupportsColor;

			// Count the colors we'll be using
			CountColorsUsed();
			LoadSettings();
		}

		//--------------------------------------------------------------------------------
		~ProfileBitmap()
		{
			Dispose(false);
		}

		//--------------------------------------------------------------------------------
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Dispose the object.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			if (!this.Disposed)
			{
				if (disposing)
				{
					//free any managed resources
					if (m_font != null)
					{
						m_font.Dispose();
						m_font = null;
					}
					if (m_printDoc != null)
					{
						m_printDoc.Dispose();
						m_printDoc = null;
					}
				}

				//free unmanaged resources

			}
			Disposed = true;
		}

		//--------------------------------------------------------------------------------
		private void CommonInit()
		{
			// Set up our print document for the specified printer
			m_disposed			= false;
			m_firstPosition		= true;
			m_isExtendedPreview	= false;
			m_supportsColor		= false;
			m_currentX			= 0;
			m_currentY			= 0;
			m_ipAddress			= "127.0.0.1";

			FontStyle style		= FontStyle.Regular;
			FontFamily family	= new FontFamily("Arial");
			m_font				= new Font(family, 10.0f, style); 
			family.Dispose();

			m_printDoc			= new PrintDocument();
			// You have to implement a handler for the PrintPage event
			m_printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Resets the position at which the image is printed.
		/// </summary>
		public void ResetPosition()
		{
			m_currentX = 0;
			m_currentY = 0;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Counts the number of colors we'll be printing (usually so we can determine 
		/// the wisdth of the image to be printed).
		/// </summary>
		private void CountColorsUsed()
		{
			m_colorsUsed = 0;

			int start = (int)ColorBarFlags.None + 1;
			int end = (int)ColorBarFlags.All / 2;
			for (int i = start; i <= end; i*=2)
			{
				ColorBarFlags color = Utility.IntToEnum(i, ColorBarFlags.None);
				if (color != ColorBarFlags.None)
				{
					m_colorsUsed += ColorIsSet(color) ? 1 : 0;
				}
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// The function that fires off the printing of the bitmap.
		/// </summary>
		public void PrintBitmap()
		{
			if (m_printDoc != null && PrinterIsOnline())
			{
				m_printDoc.OriginAtMargins = false;
				m_printDoc.DocumentName = "Scheduled Nozzle Exercise";
				m_printDoc.Print();
			}
		}

		//--------------------------------------------------------------------------------
		public bool PrinterIsOnline()
		{
			bool online = true;

			// found this code at http://blog.crowe.co.nz/archive/2005/08/08/182.aspx

			//OLEPRNLib.SNMP snmp = new OLEPRNLib.SNMP();
			//try
			//{
			//    // Open the SNMP connect to the printer
			//    snmp.Open(this.IPAddress, "public", 1, 2000);
			//    uint statusResult = snmp.GetAsByte(String.Format("25.3.2.1.5.{0}", 1));
			//}
			//catch (Exception)
			//{
			//    online = false;
			//}
			return online;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Common point of access to the image printing function. Make sure you 
		/// establish whether or not this is print, or print preview, and whether 
		/// or not the print preview is inteded to display an entire page worth of 
		/// images ("extened" print preview) before calling this method.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void printDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			PrintDocument doc = sender as PrintDocument;
			if (doc.PrintController.IsPreview && this.m_isExtendedPreview)
			{
				// save the current X/Y position so we can restore it when we're done
				int oldCurrentX = m_currentX;
				int oldCurrentY = m_currentY;
				// this value allows us to reign in a runaway process. My printer only 
				// has room to print 70 of our nozzle-check images, so I figure a good 
				// value to force a stop would be 100.
				int repeats = 0;
				// since we're previewing in order to test the capacity of our page, 
				// let's start at the top-left corner of the page
				m_currentX = 0;
				m_currentY = 0;
				// print the image until we either run out of room, or we've printed 
				// 100 nozzle-check images on the page.
				do
				{
					PrintImage(e, doc);
					repeats++;
				} while (!m_firstPosition && repeats < 100);
				// reset our current X/Y coordinates.
				m_currentX = oldCurrentX;
				m_currentY = oldCurrentY;
			}
			else
			{
				// this is either a real print job, or a normal preview
				PrintImage(e, doc);
				// we only need to save the settings if we're actually printing
				SaveSettings();
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Prints the image (used by both print preview and print
		/// </summary>
		/// <param name="e"></param>
		private void PrintImage(PrintPageEventArgs e, PrintDocument doc)
		{
			CountColorsUsed();

			int imageWidth = 0;
			int imageHeight = 0;

			// to ease typing, we retrieve the printable area from the PrinterSettings 
			// object.
			int maxX = (int)e.PageSettings.PrintableArea.Width;
			int maxY = (int)e.PageSettings.PrintableArea.Height;

			// get the current date
			string		dateString	= DateTime.Now.ToString();
			// create our graphics object
			Graphics	graphics	= e.Graphics;
			// get the size of the string we'll be printing
			SizeF		textSize	= graphics.MeasureString(dateString, m_font);
			// set our pen width so we can draw our vertical lines
			float		penWidth	= textSize.Height;
			// keep track of the current position
			float		x			= m_currentX;
			float		y			= m_currentY;
			// specify how tall the lines will be
			float		barHeight	= 50;

			// initialize our brush and pen
			SolidBrush	brush		= new SolidBrush(Color.Black);
			Pen			pen			= new Pen(brush, penWidth);
			// draw the text (always in black)
			graphics.DrawString(dateString, m_font, brush, x, y);
			y += penWidth + m_barGap;
			x += (float)penWidth * 0.5f;

			// check to see what colors we need to print, and print the associated 
			// vertical bar(s)
			if ((m_colorBars & ColorBarFlags.Black) == ColorBarFlags.Black)
			{
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}

			if ((m_colorBars & ColorBarFlags.Cyan) == ColorBarFlags.Cyan)
			{
				brush.Color = Color.FromArgb(0, 255, 255);
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}

			if ((m_colorBars & ColorBarFlags.Magenta) == ColorBarFlags.Magenta)
			{
				brush.Color = Color.FromArgb(255, 0, 255);
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}

			if ((m_colorBars & ColorBarFlags.Yellow) == ColorBarFlags.Yellow)
			{
				brush.Color = Color.FromArgb(255, 255, 0);
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}
	
			if ((m_colorBars & ColorBarFlags.LightMagenta) == ColorBarFlags.LightMagenta)
			{
				brush.Color = Color.FromArgb(255, 128, 255);
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}

			if ((m_colorBars & ColorBarFlags.LightCyan) == ColorBarFlags.LightCyan)
			{
				brush.Color = Color.FromArgb(128, 255, 255);
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}

			if ((m_colorBars & ColorBarFlags.LightBlack) == ColorBarFlags.LightBlack)
			{
				brush.Color = Color.FromArgb(128, 128, 128);
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}

			if ((m_colorBars & ColorBarFlags.Red) == ColorBarFlags.Red)
			{
				brush.Color = Color.Red;
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}

			if ((m_colorBars & ColorBarFlags.Green) == ColorBarFlags.Green)
			{
				brush.Color = Color.Green;
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}
			if ((m_colorBars & ColorBarFlags.Blue) == ColorBarFlags.Blue)
			{
				brush.Color = Color.Blue;
				pen.Brush = brush;
				x += penWidth + m_barGap;
				graphics.DrawLine(pen, new PointF(x, y), new PointF(x, y+barHeight));
			}
			y += barHeight + 2;

			imageWidth = Math.Max((int)textSize.Width, (int)((float)m_colorsUsed * (penWidth + m_barGap)));

			x = m_currentX + imageWidth;
			pen.Width = 1;
			pen.Color = Color.Black;
			graphics.DrawLine(pen, new PointF(x-imageWidth, y), new PointF(x, y));

			// clean up
			brush.Dispose();
			pen.Dispose();

			// finally we adjust our current position so we can re-use the same sheet 
			// of paper for the test.
			y += 5;
			x += 5;
			imageHeight = (int)y - m_currentY;

			bool advancePosition = ((!doc.PrintController.IsPreview) || (doc.PrintController.IsPreview && this.IsExtendedPreview));
			if (advancePosition)
			m_currentX = (int)x;
			if (m_currentX + imageWidth > maxX)
			{
				m_currentX = 0;
				m_currentY += imageHeight;
				if (m_currentY + imageHeight > maxY)
				{
					m_currentY = 0;
				}
			}
			// set the value that indicates that we are/aren't at the first print position
			m_firstPosition = (m_currentX == 0 && m_currentY == 0);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Loads the setting sfrom the data file.
		/// </summary>
		public void LoadSettings()
		{
			// make the printer name a valid XML node name
			string printerName = m_printerName.Replace(" ", "_");;
			if (printerName.Contains("\\"))
			{
				printerName = printerName.Replace("\\", "_");
			}

			// esablish our data file name
			string path = GetDataFileName();

			// if the file exists, load it
			if (File.Exists(path))
			{
				try
				{
					XDocument doc = XDocument.Load(path);
					XElement element = doc.Element("ROOT").Element("SETTINGS").Element(printerName);
					// just because we didn't find the desired element doesn't mean it's an 
					// exception - it might just be a new printer.
					if (element != null)
					{
						XElement currentX  = element.Element("NextPositionX"); 
						XElement currentY  = element.Element("NextPositionY");
						XElement colorBars = element.Element("ColorsToPrint");
						if (currentX != null)
						{
							m_currentX = Convert.ToInt32(currentX.Value);
						}
						if (currentY != null)
						{
							m_currentY = Convert.ToInt32(currentY.Value);
						}
						if (colorBars != null)
						{
							m_colorBars = Utility.MakeColorBars(colorBars.Value);
						}
					}
				}
				catch (Exception ex)
				{
					if (ex != null) {}
					throw;
				}
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Saves the necessary settings to the data file
		/// </summary>
		public void SaveSettings()
		{
			string printerName = m_printerName.Replace(" ", "_");;
			if (printerName.Contains("\\"))
			{
				printerName = printerName.Replace("\\", "_");
			}
			string path = GetDataFileName();
			try
			{
				bool newFile = true;
				XDocument doc;

				// if the file exists, load it
				if (File.Exists(path))
				{
					doc = XDocument.Load(path);
					newFile = false;
				}
				// otherwise, create it, and add the root/settings nodes
				else
				{
					doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XComment("AutoPrint data file"));
					var root = new XElement("ROOT",
										   new XElement("SETTINGS"));
					doc.Add(root);
				}

				// establish our printer element
				var element = new XElement(printerName,
										   new XElement("NextPositionX", m_currentX.ToString()),
										   new XElement("NextPositionY", m_currentY.ToString()),
										   new XElement("ColorsToPrint", Utility.MakeColorString(m_colorBars)));

				// if the file is new, we'll add the printer element
				if (newFile)
				{
					doc.Element("ROOT").Element("SETTINGS").Add(element);
				}
				// otherwise
				else
				{
					// if the element exists, replavce it with our new one
					if (element != null)
					{
						XElement temp = doc.Element("ROOT").Element("SETTINGS").Element(printerName);
						if (temp == null)
						{
							doc.Element("ROOT").Element("SETTINGS").Add(element);
						}
						else
						{
							doc.Element("ROOT").Element("SETTINGS").Element(printerName).ReplaceWith(element);
						}
					}
					// otherwsie, add it
					else
					{
						doc.Element("ROOT").Element("SETTINGS").Add(element);
					}
				}

				// save the file to disk
				doc.Save(path);
			}
			catch (Exception ex)
			{
				if (ex != null) {}
				throw;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Provides a single point at which we build the data file name
		/// </summary>
		/// <returns>The completely qualified path to the data file</returns>
		public static string GetDataFileName()
		{
			return (System.IO.Path.Combine(Utility.CreateAppDataFolder("AutoPrint"), "autoprint.xml"));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determine if a SINGLE color is set
		/// </summary>
		/// <param name="color">The color to check</param>
		/// <returns>True if the specified color is set</returns>
		/// <exception cref="AutoPrintException"></exception>
		public bool ColorIsSet(ColorBarFlags color)
		{
			// see if the programmer specified more than one color
			if (Utility.IntToEnum((int)color, ColorBarFlags.None) == ColorBarFlags.None)
			{
				throw new AutoPrintException("Only one color at a time can be specified in the color parameter.");
			}
			bool isSet = ((m_colorBars & color) == color);
			return isSet;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Overriden so we can add the object to a listbox control.
		/// </summary>
		/// <returns>Returns the printer name</returns>
		public override string ToString()
		{
			return this.PrinterName;
		}

	}
}

