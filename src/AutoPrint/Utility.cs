using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoPrint
{
	public static class Utility
	{
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Create (if necessary) the specified application data folder. This method 
		/// only creates the root folder, and will throw an exception if more than one 
		/// folder is specified.  For instance, "\MyApp" is valid, but 
		/// "\MyApp\MySubFolder" is not.
		/// </summary>
		/// <param name="folderName">A single folder name (can have a bcakslash at either or both ends).</param>
		/// <returns>The fully qualified path that was created (or that already exists)</returns>
		public static string CreateAppDataFolder(string folderName)
		{
			return CreateAppDataFolder(Environment.SpecialFolder.CommonApplicationData, folderName);
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Create (if necessary) the specified application data folder. This method 
		/// only creates the root folder, and will throw an exception if more than one 
		/// folder is specified.  For instance, "\MyApp" is valid, but 
		/// "\MyApp\MySubFolder" is not.
		/// </summary>
		/// <param name="folderName">A single folder name (can have a bcakslash at either or both ends).</param>
		/// <returns>The fully qualified path that was created (or that already exists)</returns>
		public static string CreateAppDataFolder(Environment.SpecialFolder specialFolder,  string folderName)
		{
			string appDataPath = "";
			string dataFilePath = "";

			folderName = folderName.Trim();
			if (folderName != "")
			{
				try
				{
					// set the directory where the file will come from 
					// under XP, the folder name is "C:\Documents and Settings\All Users\Application Data\[folderName]"
					// under Vista, the folder name is "C:\Program Data\[folderName]"
					appDataPath = System.Environment.GetFolderPath(specialFolder);
				}
				catch (Exception ex)
				{
					throw ex;
				}

				if (folderName.Contains("\\"))
				{
					string[] path = folderName.Split('\\');
					int folderCount = 0;
					int folderIndex = -1;
					for (int i = 0; i < path.Length; i++)
					{
						string folder = path[i];
						if (folder != "")
						{
							if (folderIndex == -1)
							{
								folderIndex = i;
							}
							folderCount++;
						}
					}
					if (folderCount != 1)
					{
						throw new Exception("Invalid folder name specified (this function only creates the root app data folder for the application).");
					}
					folderName = path[folderIndex];
				}
			}
			if (folderName == "")
			{
				throw new Exception("Processed folder name resulted in an empty string.");
			}

			try
			{
				dataFilePath = System.IO.Path.Combine(appDataPath, folderName);
				if (!Directory.Exists(dataFilePath))
				{
					Directory.CreateDirectory(dataFilePath);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dataFilePath;
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Casts the specified integer to an appropriate enum. If all else fails, 
		/// the enum will be returned as the specified default ordinal.
		/// </summary>
		/// <param name="value">The integer value representing an enumeration element</param>
		/// <param name="deafaultValue">The default enumertion to be used if the specified "value" does not exist in the enumeration definition</param>
		/// <returns></returns>
		public static T IntToEnum<T>(int value, T defaultValue)
		{
			T enumValue = (Enum.IsDefined(typeof(T), value)) ? (T)(object)value : defaultValue;
			return enumValue;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Create a ColorBarsFlag object based on the contents of the string
		/// </summary>
		/// <param name="text">A delimited string in the format "Color|Color"</param>
		/// <returns>The appropriate ColorBarFlags value (ColorBarFlags.None indicates a problem)</returns>
		public static ColorBarFlags MakeColorBars(string text)
		{
			ColorBarFlags colorBars = ColorBarFlags.None;
			if (text.ToLower() == "all")
			{
				colorBars = ColorBarFlags.All;
			}
			else
			{
				string[] colors = text.Split('|');
				foreach (string color in colors)
				{
					switch (color.ToLower())
					{
						case "black"		: colorBars |= ColorBarFlags.Black;			break;
						case "blue"			: colorBars |= ColorBarFlags.Blue;			break;
						case "cyan"			: colorBars |= ColorBarFlags.Cyan;			break;
						case "green"		: colorBars |= ColorBarFlags.Green;			break;
						case "lightblack"	: colorBars |= ColorBarFlags.LightBlack;	break;
						case "lightcyan"	: colorBars |= ColorBarFlags.LightCyan;		break;
						case "lightmagenta"	: colorBars |= ColorBarFlags.LightMagenta;	break;
						case "magenta"		: colorBars |= ColorBarFlags.Magenta;		break;
						case "red"			: colorBars |= ColorBarFlags.Red;			break;
						case "yellow"		: colorBars |= ColorBarFlags.Yellow;		break;
					}
				}
			}
			return colorBars;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Builds a color string based on the value specified ColorBarFlags object.
		/// </summary>
		/// <param name="colors">The color flags to use</param>
		/// <returns>A string representing the specified flags</returns>
		public static string MakeColorString(ColorBarFlags colors)
		{
			StringBuilder text = new StringBuilder("");
			// if all flags are chosen, we can get out
			if ((int)colors == 767 | colors == ColorBarFlags.All)
			{
				return "All";
			}

			// cycle through the ColoBarFlags
			int start = (int)ColorBarFlags.None + 1;
			int end = (int)ColorBarFlags.All / 2;
			for (int i = start; i <= end; i*=2)
			{
				// get the ordinal value of the loop control variable "i"
				ColorBarFlags color = IntToEnum(i, ColorBarFlags.None);
				// make sure we have something to check
				if (color != ColorBarFlags.None)
				{
					// append the appropriate text to the StringBuilder object
					text.Append(((colors & color) == color) ? string.Format("{0}|", color.ToString()) : "");
				}
			}

			// get the string and strip the last '|' character
			string colorsText = text.ToString().Substring(0, text.Length - 1);

			return colorsText;
		}


	}
}
