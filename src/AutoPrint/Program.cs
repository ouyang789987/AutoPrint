using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace AutoPrint
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
			else
			{
#if DEBUG
//				Debugger.Launch();
#endif
				ConsoleProcess process = new ConsoleProcess();
				try
				{
					process.Run(args);
				}
				catch (Exception ex)
				{
					MessageBox.Show(string.Format("{0}\n\n{1}", ex.Message, ex.StackTrace));
				}
			}
		}
	}
}
