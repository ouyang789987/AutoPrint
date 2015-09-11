using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoPrint
{
	/// <summary>
	/// This class encapsulates the commandline functionality, and should only be 
	/// instantiated if there commandline arguments passed to the application.
	/// </summary>
	public class ConsoleProcess
	{
		private string			m_printerName		= "";
		private ColorBarFlags	m_colorBars			= ColorBarFlags.None;


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
		public ConsoleProcess()
		{
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Runs a print job based on the specified commandline parameter(s).
		/// </summary>
		/// <param name="args"></param>
		public void Run(string[] args)
		{
			// validate that the arguments are somewhat valid
			int length = 0;
			foreach (string arg in args)
			{
				length += arg.Trim().Length;
			}
			if (length == 0)
			{
				Console.WriteLine("Invalid console arguments");
			}

			try
			{
				bool usingConfigFile = false;
				string configSection = "";
				for (int i = 0; i < args.Length; i++)
				{
					// If the config parameter was specified, get the section name and 
					// turn on the usingConfigFile flag.
					if (args[i].ToLower().StartsWith("config="))
					{
						configSection = args[i].Substring(7);
						usingConfigFile = true;
						if (configSection == "")
						{
							return;
						}
						break;
					}
					else
					{
						// If we get here, the parameters indicate the printer name and 
						// color bars.
						string colorBars = "";
						if (args[i].ToLower().StartsWith("name=") && args[i].Length > 5)
						{
							m_printerName = args[i].Substring(5);
						}
						else if (args[i].ToLower().StartsWith("colors=") && args[i].Length > 7)
						{
							colorBars = args[i].Substring(7);
						}

						// If the resulting printer name or color bars are NOT null 
						// strings, Convert the colorbar string to the appropriate 
						// enum flag.
						if (m_printerName != "" && colorBars != "")
						{
							m_colorBars = Utility.MakeColorBars(args[i]);
						}
						else
						{
							return;
						}
					}
				}

				// If we get here, we're ready to create the appropriate profile object 
				// and print it.
				ProfileBitmap profile = null;

				// Call the appropriate constructor.
				if (usingConfigFile)
				{
					profile = new ProfileBitmap(configSection);
				}
				else
				{
					profile = new ProfileBitmap(m_printerName, m_colorBars);
				}

				// Print the test image.
				profile.PrintBitmap();

				// Clean up.
				profile.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Exception encountered: {0}", ex.Message));
			}
		}

	}
}
