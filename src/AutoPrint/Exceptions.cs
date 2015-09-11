using System;

namespace AutoPrint
{
	[Serializable()]
	public class AutoPrintConsoleException : System.Exception
	{
		public AutoPrintConsoleException():base()
		{}
		public AutoPrintConsoleException(string message):base(message)
		{}
		public AutoPrintConsoleException(string message, Exception innerException):base(message, innerException)
		{}
		protected AutoPrintConsoleException(System.Runtime.Serialization.SerializationInfo info,
											System.Runtime.Serialization.StreamingContext context) 
			: base(info, context)
		{ }
	}

	[Serializable()]
	public class AutoPrintException : System.Exception
	{
		public AutoPrintException():base()
		{}
		public AutoPrintException(string message):base(message)
		{}
		public AutoPrintException(string message, Exception innerException):base(message, innerException)
		{}
		protected AutoPrintException(System.Runtime.Serialization.SerializationInfo info,
											System.Runtime.Serialization.StreamingContext context) 
			: base(info, context)
		{ }
	}
}