using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Text;

namespace AutoPrint
{
	#region Enumerations
	public enum DriveInterfaceType	{ Unknown=0, IDE=1, SCSI=2, HDC=3, USB=4, FireWire=5 };
	public enum MediaType			{ Unknown=0, Fixed=1, Removable=2, ExternalFixed=3  };
	public enum DriveType			{ Unknown=0, NoRootDir=1, RemovableDisk=2, LocalDisk=3, NetworkDisk=4, CompactDisc=5, RamDisk=6 };
	public enum DriveLocale			{ Unknown=0, Local=1, Remote=2 };
	public enum LogicalMediaType	{ Unknown=0, Fixed=1, Removable=2, Floppy=3 };
	public enum LogicalAccess		{ Unknown=0, Readable=1, Writable=2, ReadWriteSupported=3, WriteOnce=4 };
	#endregion Enumerations

	[Serializable()]
	public class SysInfoException : System.Exception
	{
		public SysInfoException() : base() 
		{}

		public SysInfoException(string message) : base(message) 
		{}

		public SysInfoException(string message, Exception innerException) 
			: base(message, innerException) 
		{}

		//--------------------------------------------------------------------------------
		// Constructor needed for serialization when exception propagates from a remoting 
		// server to the client.
		protected SysInfoException(System.Runtime.Serialization.SerializationInfo info,
								   System.Runtime.Serialization.StreamingContext context) 
			: base(info, context)
		{ }
	}

	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// This class contains information about the specified path (actually, 
	/// it's the drive part of the path that matters).  This class is 
	/// populated by calling the SystInfo.GetPathInfo() method. Unless 
	/// otherwise indicated, all properties are set/get properties.
	/// </summary>
	public class SIDriveInfo
	{
		#region Data members for physical drives
		private string m_physName;
		private string m_physDeviceID;
		private int    m_physIndex;
		private Int64  m_physSignature;
		private string m_physInterfaceString;
		private string m_physMediaString;
		private MediaType m_physDiskMediaType = MediaType.Unknown;
		private DriveInterfaceType m_physInterfaceType = DriveInterfaceType.Unknown;
		#endregion Data members for physical drives

		#region Properties for physical drives
		public string PhysicalInterfaceString
		{
			get { return m_physInterfaceString; }
			set { m_physInterfaceString = value; }
		}
		public string PhysicalMediaString
		{
			get { return m_physMediaString; }
			set { m_physMediaString = value; }
		}
		public DriveInterfaceType PhysicalInterfaceType
		{
			get { return m_physInterfaceType; }
			set { m_physInterfaceType = value; }
		}
		public MediaType PhysicalDiskMediaType
		{
			get { return m_physDiskMediaType; }
			set { m_physDiskMediaType = value; }
		}
		public string PhysicalDeviceID
		{
			get { return m_physDeviceID; }
			set { m_physDeviceID = value; }
		}
		public string PhysicalName
		{
			get { return m_physName; }
			set { m_physName = value; }
		}
		public Int64 PhysicalSignature
		{
			get { return m_physSignature; }
			set { m_physSignature = value; }
		}
		public int PhysicalIndex
		{
			get { return m_physIndex; }
			set { m_physIndex = value; }
		}
		#endregion Properties for physical drives
	
		#region Data members for logical drives
		string m_logName;
		string m_logDeviceID;
		int    m_logIndex;
		string m_logVolumeSN;
		string m_logVolumeName;
		string m_logSystemName;
		string m_logMediaString;
		DriveType m_logDriveType = DriveType.Unknown;
		LogicalMediaType m_logMediaType = LogicalMediaType.Unknown;
		string m_logFileSystem;
		bool m_logCompressed;
		UInt64 m_logFreeSpace;
		UInt64 m_logSize;
		string m_logPnpDeviceID;
		LogicalAccess m_logAccess = LogicalAccess.Unknown;
		string m_logProvider;
		#endregion Data members for logical drives

		#region Properties for logical drives
		public string LogicalName
		{
			get { return m_logName; }
			set { m_logName = value; }
		}
		public string LogicalDeviceID
		{
			get { return m_logDeviceID; }
			set { m_logDeviceID = value; }
		}
		public int LogicalIndex
		{
			get { return m_logIndex; }
			set { m_logIndex = value; }
		}
		public string LogicalVolumeSerialNumber
		{
			get { return m_logVolumeSN; }
			set { m_logVolumeSN = value; }
		}
		public string LogicalVolumeName
		{
			get { return m_logVolumeName; }
			set { m_logVolumeName = value; }
		}
		public string LogicalSystemName
		{
			get { return m_logSystemName; }
			set { m_logSystemName = value; }
		}
		public LogicalMediaType LogicalDiskMediaType
		{
			get { return m_logMediaType; }
			set { m_logMediaType = value; }
		}
		public string LogicalMediaString
		{
			get { return m_logMediaString; }
			set { m_logMediaString = value; }
		}
		public DriveType LogicalDriveType
		{
			get { return m_logDriveType; }
			set { m_logDriveType = value; }
		}
		public string LogicalFileSystem
		{
			get { return m_logFileSystem; }
			set { m_logFileSystem = value; }
		}
		public bool LogicalCompressed
		{
			get { return m_logCompressed; }
			set { m_logCompressed = value; }
		}
		public UInt64 LogicalFreeSpace
		{
			get { return m_logFreeSpace; }
			set { m_logFreeSpace = value; }
		}
		public string LogicalPnpDeviceID
		{
			get { return m_logPnpDeviceID; }
			set { m_logPnpDeviceID = value; }
		}
		public UInt64 LogicalSize
		{
			get { return m_logSize; }
			set { m_logSize = value; }
		}
		public LogicalAccess LogicalAccess
		{
			get { return m_logAccess; }
			set { m_logAccess = value; }
		}
		public string LogicalProvider
		{
			get { return m_logProvider; }
			set { m_logProvider = value; }
		}

		//Convenience properties
		public int PercentFree
		{
			get { return (Convert.ToInt32(((double)Math.Max(1, this.LogicalFreeSpace) / (double)Math.Max(1, this.LogicalSize)) * 100.0)); }
		}
		public int PercentUsed
		{
			get { return (100 - this.PercentFree); }
		}
		public UInt64 UsedSpace
		{
			get { return (this.LogicalSize - this.LogicalFreeSpace); }
		}
		#endregion Properties for physical drives


		#region Constructors
		public SIDriveInfo()
		{
		}
		#endregion Constructors

		#region Methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Casts the specified integer to an appropriate enum. If all else fails, 
		/// the enum will be returned as the specified default ordinal. Usage is:
		/// 
		/// MyEnum value = (MyEnum)IntToEnum(intValue, MyEnum.EnumValue);
		/// 
		/// </summary>
		/// <param name="value">The integer value representing an enumeration element</param>
		/// <param name="deafaultValue">The default enumertion to be used if the specified "value" does not exist in the enumeration definition</param>
		/// <returns>The enumeration of the specified value, or the default value if the enumartion was not found.</returns>
		public static T IntToEnum<T>(int value, T defaultValue)
		{
			T enumValue = (Enum.IsDefined(typeof(T), value)) ? (T)(object)value : defaultValue;
			return enumValue;
		}

		//--------------------------------------------------------------------------------
		/// Converts certain strings into their associated enumerated types 
		/// regarding physical drives. Currently, the ihnterface type and 
		/// media type are enumerated.
		public void EnumeratePhysical()
		{
			switch (m_physInterfaceString.ToUpper())
			{
				case "IDE"	: m_physInterfaceType = DriveInterfaceType.IDE; break;
				case "USB"	: m_physInterfaceType = DriveInterfaceType.USB; break;
				case "SCSI"	: m_physInterfaceType = DriveInterfaceType.SCSI; break;
				case "HDC"	: m_physInterfaceType = DriveInterfaceType.HDC; break;
				case "1394"	: m_physInterfaceType = DriveInterfaceType.FireWire; break;
				default		: m_physInterfaceType = DriveInterfaceType.Unknown; break;
			}
			switch (m_physMediaString.ToLower())
			{
				// vista and later
				case "external hard disk media"			: m_physDiskMediaType = MediaType.ExternalFixed; break;
				case "removable media other than floppy": m_physDiskMediaType = MediaType.Removable; break;
				case "fixed\thard disk media"			: m_physDiskMediaType = MediaType.Fixed; break;
				case "format is unknown"				: m_physDiskMediaType = MediaType.Unknown; break;
				// before Vista
				case "removable media"					: m_physDiskMediaType = MediaType.Removable; break;
				case "Fixed hard disk"					: m_physDiskMediaType = MediaType.Fixed; break;
				case "Unknown"							: m_physDiskMediaType = MediaType.Unknown; break;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Converts certain strings into their associated enumerated types 
		/// regarding logical drives. Currently, only the media type is 
		/// enumerated.
		/// </summary>
		public void EnumerateLogical()
		{
			switch (m_logMediaString.Trim().ToLower())
			{
				case "unknown" :
				case "0 unknown" : m_logMediaType = LogicalMediaType.Unknown; break;
				case "11" : m_logMediaType = LogicalMediaType.Removable; break;
				case "12" : m_logMediaType = LogicalMediaType.Fixed; break;
				default : m_logMediaType = LogicalMediaType.Floppy; break;
			}
		}
		#endregion Methods

	}


	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	public class SysInfoBase
	{

		// When adding functionality to this class, make sure you use the appropriate     
		// GetObjectPropertyXXXXX functions. These functions ease the pain of retrieving  
		// property values from a WMI class. Some properties aren't always provided       
		// (despite what you might find in the WMI documentation provided by Microsoft.   
		//                                                                                
		// When you add new functions, make sure they're static so that we don't have to  
		// instantiate an object of this class just to get to your new functionality.     

		#region GetObjectPropertyXXXX methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named string property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static string GetObjectPropertyString(ManagementObject mo, string name, string defaultValue)
		{
			string value = defaultValue;
			try
			{
				value = mo[name].ToString();
			}
			catch (Exception ex)
			{
				if (ex != null) {}
				throw;
			}
			return value;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named Int32 property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static int GetObjectPropertyInt32(ManagementObject mo, string name, int defaultValue)
		{
			Int32 value = defaultValue;
			try
			{
				value = Convert.ToInt32(mo[name]);
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}
			return value;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named Int32 property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static UInt32 GetObjectPropertyUInt32(ManagementObject mo, string name, UInt32 defaultValue)
		{
			UInt32 value = defaultValue;
			try
			{
				value = Convert.ToUInt32(mo[name]);
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}
			return value;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named Int16 property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static Int16 GetObjectPropertyInt16(ManagementObject mo, string name, Int16 defaultValue)
		{
			Int16 value = defaultValue;
			try
			{
				value = Convert.ToInt16(mo[name]);
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}
			return value;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named UInt16 property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static UInt16 GetObjectPropertyUInt16(ManagementObject mo, string name, UInt16 defaultValue)
		{
			UInt16 value = defaultValue;
			try
			{
				value = Convert.ToUInt16(mo[name]);
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}
			return value;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named Int64 property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static Int64 GetObjectPropertyInt64(ManagementObject mo, string name, Int64 defaultValue)
		{
			Int64 value = defaultValue;
			try
			{
				value = Convert.ToInt64(mo[name]);
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}
			return value;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named UINT64 property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static UInt64 GetObjectPropertyUInt64(ManagementObject mo, string name, UInt64 defaultValue)
		{
			UInt64 value = defaultValue;
			try
			{
				value = Convert.ToUInt64(mo[name]);
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}
			return value;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the named boolean property and if necessary (if the 
		/// property is not present in the management object) assigns the 
		/// default value.
		/// </summary>
		/// <param name="mo">The management object that contains the property</param>
		/// <param name="name">The case-sensitive name of the property</param>
		/// <param name="defaultValue">The value to assign if the property does not exist</param>
		/// <returns>The value of the property</returns>
		protected static bool GetObjectPropertyBool(ManagementObject mo, string name, bool defaultValue)
		{
			bool value = defaultValue;
			try
			{
				value = (bool)(mo[name]);
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}
			return value;
		}
		
		#endregion GetObjectPropertyXXXX methods

		#region Other basic utility methods

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

		#endregion Other basic utility methods

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Validates the path passed to the GetPathInfo() method.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected static string ValidatePath(string path)
		{
			string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

			// if the path is empty, return
			if (path == "")
			{
				return "No path specified.";
			}

			// if the first character isn't a letter, return
			if (!letters.Contains(path[0].ToString()))
			{
				throw new SysInfoException(string.Format("Incoming path: '{0}'\n\nInvalid drive specification - you must use a leter from a-z.", path));
			}

			string pathDrive = "";
			
			// if the length of the drive name is 1, grab the whole string and add a colon
			if (pathDrive.Length == 1)
			{
				pathDrive = path + ":";
			}
			// otherwise, grab the first two characters
			else
			{
				pathDrive = path.Substring(0, 2);
			}

			// if the resulting string doesn't end with a :, return
			if (!pathDrive.EndsWith(":"))
			{
				throw new SysInfoException(string.Format("Incoming path: '{0}'\n\nInvalid path format. The drive letter must be followed by a colon.", path));
			}
			return pathDrive;
		}
	}


	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	public sealed class SysInfo : SysInfoBase
	{
		private SysInfo() {}

		#region GetXXXX methods for WMI classes
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the mac address of the one or more adapters found to be 
		/// "IPEnabled" on this machine.  If there are a number of adapters, 
		/// and if the firstOnly parameter is false, this method returns a 
		/// comma-delimited list of ALL "IPEnabled" adapters in this system.
		/// </summary>
		/// <param name="stripColons">If true, strips the colons from the return MAC address(es).</param>
		/// <param name="firstOnly">If true, only returns the first "IPEnabled" MAC address</param>
		/// <returns>A comma-delited list of "IPEnabled" MAC addresses.</returns>
		public static string GetMacAddress(bool stripColons, bool firstOnly)
		{
			ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection moc = mc.GetInstances();
			string MACAddress = "";
			foreach (ManagementObject mo in moc)
			{
				if (GetObjectPropertyBool(mo, "IPEnabled", false) == true) 
				{
					if (MACAddress != "")
					{
						MACAddress += ",";
					}
					MACAddress = GetObjectPropertyString(mo, "MacAddress", "Uknown").Trim();
				}
				mo.Dispose();
				if (MACAddress != "" && firstOnly)
				{
					break;
				}
			}
			if (stripColons)
			{
				MACAddress = MACAddress.Replace(":", "");
			}
			return MACAddress;
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Retrieves the adpater index associated with the specified MAC 
		/// address.
		/// </summary>
		/// <param name="thisMAC">The MAC address of the desired network adapter.</param>
		/// <returns>The index of the network adapter (-1 indicates failure).</returns>
		public static int GetAdapterIndex(string thisMAC)
		{
			bool stripColons = (!thisMAC.Contains(":"));
			int adapterIndex = -1;
			ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				if ((bool)mo["IPEnabled"] == true)
				{
					string MACAddress = GetObjectPropertyString(mo, "MacAddress", "Uknown").Trim();
					if (stripColons)
					{
						MACAddress = MACAddress.Replace(":", "");
					}
					if (MACAddress == thisMAC)
					{
						adapterIndex = GetObjectPropertyInt32(mo, "Index", -1);
					}
				}
				mo.Dispose();
				if (adapterIndex > -1)
				{
					break;
				}
			}
			return adapterIndex;
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified adapter is connected to a network. Just
		/// because the adapter is connected, doesn't mean that the desired 
		/// server is available, so make sure you comine the results of this 
		/// function with an appropiate test to verify that the server is 
		/// available.
		/// </summary>
		/// <param name="adapterIndex">The index of the desired network adapter</param>
		/// <returns>True if the adpater is connected to the newtork.</returns>
		public static bool IsAdapterConnected(int adapterIndex)
		{
			bool connected = false;

			if (adapterIndex >= 0)
			{
				ManagementClass mc = new ManagementClass("Win32_NetworkAdapter");
				ManagementObjectCollection moc = mc.GetInstances();
				foreach (ManagementObject mo in moc)
				{
					if (Convert.ToInt32(mo["Index"]) == adapterIndex)
					{
						connected = (GetObjectPropertyInt32(mo, "NetConnectionStatus", 0) == 2);
					}
					mo.Dispose();
					if (connected)
					{
						break;
					}
				}
			}
			return connected;
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Return processorId's for all CPUs in the machine
		/// </summary>
		/// <returns>ProcessorId</returns>
		public static string GetCpuID()
		{
			StringBuilder cpuID = new StringBuilder("");
			ManagementClass mc = new ManagementClass("Win32_Processor");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				if (cpuID.Length != 0)
				{
					cpuID.Append(",");
				}
				cpuID.Append(GetObjectPropertyString(mo, "ProcessorId", "Unknown").Trim());
				mo.Dispose();
			}
			return cpuID.ToString();
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Gets the (windows) operating system currently running on the system.
		/// </summary>
		/// <returns>The name of the operating system found, or "Unknown"</returns>
		public static string GetOperatingSystem()
		{
			string osName = "Unknown";
			ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				osName = GetObjectPropertyString(mo, "Name", "Unkown");
				string[] parts = osName.Split('|');
				if (parts.Length > 0)
				{
					osName = parts[0].Trim();
				}
				mo.Dispose();
			}
			return osName;
		}

		//--------------------------------------------------------------------------------
		public static string GetPhysicalMemory()
		{
			StringBuilder memoryInfo = new StringBuilder("");
			ManagementClass mc = new ManagementClass("Win32_PhysicalMemory");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				//if (memoryInfo.Length > 0)
				//{
				//    memoryInfo.Append("^");
				//}
				memoryInfo.AppendFormat("{0}|{1}", 
				                        GetObjectPropertyString(mo, "Manufacturer", "Unkown").Trim(),
				                        GetObjectPropertyString(mo, "PartNumber", "Unkown").Trim());
				mo.Dispose();
				break;
			}
			if (memoryInfo.Length == 0)
			{
				memoryInfo.Append("Unknown");
			}

			return memoryInfo.ToString();
		}

		//--------------------------------------------------------------------------------
		public static string GetComputerSystemProductUUID()
		{
			string uuid = "Unknown";
			ManagementClass mc = new ManagementClass("Win32_ComputerSystemProduct");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				uuid = GetObjectPropertyString(mo, "UUID", "Unkown").Trim();
				mo.Dispose();
			}
			return uuid;
		}

		#endregion GetXXXX methods for WMI classes

		#region Methods that use one or more GetXXXX methods to gather data
		//////////////////////////////////////////////////////////////////////////////////

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not this computer is connected to a network.
		/// </summary>
		/// <returns>True - connected to a network</returns>
		static public bool ConnectedToNetwork(string connectionURL)
		{
			// Do NOT try to use "NetEnabled" property in the NetworkAdapter. It doesn't 
			// appear to be supported.
			bool	connected		= false;
			string	mac				= "";
			int		adapterIndex	= -1;

			// get the mac address and then the adapter index (based on the mac address)
			mac = GetMacAddress(true, true);

			if (mac != "")
			{
				adapterIndex = GetAdapterIndex(mac);
			}

			connected = IsAdapterConnected(adapterIndex);

			// If we couldn't find an enabled network adapter, try to simply connect to 
			// the vpopcorn web site.
			if (!connected)
			{
				//string url = "http://www.vPopcorn.com";
				Uri uri = new Uri(connectionURL);
				WebClient webClient = new WebClient();
				string response = "";
				try
				{
					response = webClient.DownloadString(uri);
					connected = true;
				}
				// If we got an exception, we couldn't reach the web site, so we 
				// must not be connected...
				catch (Exception ex)
				{
					if (ex != null) { }
					throw;
				}
			}

			return connected;
		}



		//--------------------------------------------------------------------------------
		/// <summary>
		/// Gets the drive info for the specified path. If the return value is 
		/// null, check the errMsg property to see why.
		/// </summary>
		/// <param name="path">The path to be inspected (it only uses the drive letter and colon, but it's easier to simply provide the entire path in question).</param>
		/// <param name="errorMsg">A reference parameter that contains any error message that may have been generated</param>
		/// <returns>A SIDriveInfo object, or null if the method failed.</returns>
		public static SIDriveInfo GetPathInfo(string path, ref string errorMsg)
		{
#if DEBUG
			System.Diagnostics.Trace.WriteLine("BRCommon.SysInfo2.GetPathInfo() - entry");
#endif

			string validPath = ValidatePath(path);
			SIDriveInfo info = null;
			bool found = false;
			if (validPath.Length > 2)
			{
				errorMsg = validPath;
				return info;
			}

			// find out what physical drive on which the path is located
			ManagementClass mc = new ManagementClass("Win32_LogicalDiskToPartition");
			ManagementObjectCollection moc = mc.GetInstances();
			string tempID = "";
			string physicalID = "";
			foreach (ManagementObject mo in moc)
			{
				string dependent = GetObjectPropertyString(mo, "Dependent", "");
				if (dependent.Contains(string.Format("\"{0}\"", validPath)))
				{
					tempID		= GetObjectPropertyString(mo, "Antecedent", "");
					if (tempID != "")
					{
						int pos1	= tempID.LastIndexOf("\"Disk #") + 7;
						int pos2	= tempID.LastIndexOf(", Partition");
						physicalID	= tempID.Substring(pos1, pos2-pos1);
						found	= true;
					}
				}
				mo.Dispose();
				if (found)
				{
					break;
				}
			}
			if (!found)
			{
				throw new SysInfoException(string.Format("Incoming path: '{0}'\nCould not determine physical drive.", path));
			}
			if (physicalID == "")
			{
				throw new SysInfoException(string.Format("Incoming path: '{0}'\nPhysical drive was invalid/corrupted.", path));
			}

			found = false;
			mc = new ManagementClass("Win32_DiskDrive");
			moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				string index = GetObjectPropertyString(mo, "Index", "");
				if (index == physicalID)
				{
					info = new SIDriveInfo();
					info.PhysicalDeviceID			= GetObjectPropertyString(mo, "DeviceID",      "Unknown");
					info.PhysicalIndex				= GetObjectPropertyInt32 (mo, "Index",         -1);
					info.PhysicalInterfaceString	= GetObjectPropertyString(mo, "InterfaceType", "Uknown");
					info.PhysicalName				= GetObjectPropertyString(mo, "Name",          "Unkown");
					info.PhysicalSignature			= GetObjectPropertyInt64 (mo, "Signature",     -1);
					info.PhysicalMediaString        = GetObjectPropertyString(mo, "MediaType",     "Unwnown");
					info.EnumeratePhysical();
					found = true;
				}
				mo.Dispose();
				if (found)
				{
					break;
				}
			}
			if (!found)
			{
				throw new SysInfoException(string.Format("Incoming path: '{0}'\nCould not determine physical drive.", path));
			}

			// if we get here, we've found our physical drive, so let's get the logical drive info
			found = false;
			mc = new ManagementClass("Win32_LogicalDisk");
			moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				// the Name, Description, and DeviceID values are the same 0 - "C:" (the 
				// drive letter followed by a colon)
				string name = GetObjectPropertyString(mo, "Name", "Unkown");
				if (name == validPath)
				{
					info.LogicalAccess				= (LogicalAccess)IntToEnum(GetObjectPropertyInt32(mo, "Access", (int)LogicalAccess.Unknown), LogicalAccess.Unknown);
					info.LogicalDeviceID			= GetObjectPropertyString(mo, "DeviceID", "Unknown");
					info.LogicalName				= name;
					info.LogicalVolumeName			= GetObjectPropertyString(mo, "VolumeName", "No Volume Name");
					info.LogicalVolumeSerialNumber	= GetObjectPropertyString(mo, "VolumeSerialNumber", "Unknown");
					info.LogicalSystemName			= GetObjectPropertyString(mo, "SystemName", "Unknown");
					info.LogicalFileSystem			= GetObjectPropertyString(mo, "FileSystem", "Unknown");
					info.LogicalMediaString			= GetObjectPropertyString(mo, "MediaType", "Unknown");
					//info.LogicalProvider			= GetObjectPropertyString(mo, "Provider", "Unknown");
					info.LogicalDriveType			= (DriveType)IntToEnum(GetObjectPropertyInt32(mo, "DriveType", (int)DriveType.Unknown), DriveType.Unknown);
					info.LogicalCompressed			= GetObjectPropertyBool(mo, "Compressed", false);
					info.LogicalFreeSpace			= GetObjectPropertyUInt64(mo, "FreeSpace", 0);
					//info.LogicalPnpDeviceID			= GetObjectPropertyString(mo, "PNPDeviceID", "Unknown");
					info.LogicalSize				= GetObjectPropertyUInt64(mo, "Size", 0);

					info.EnumerateLogical();
					found = true;
				}
				mo.Dispose();
				if (found)
				{
					break;
				}
			}
			if (!found)
			{
				throw new SysInfoException(string.Format("Incoming path: '{0}'\nCould not determine logical drive.", path));
			}
			return info;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a systemID string from appropriate WMI data.
		/// </summary>
		/// <returns></returns>
		public static string GetSystemID()
		{
			string errMsg = "";
			string systemID = GetPathInfo("C:\\", ref errMsg).LogicalVolumeSerialNumber;
			// add more things here if necessary/desired

			return systemID;
		}

		//--------------------------------------------------------------------------------
		public static bool PrinterIsOnline(string name)
		{
			bool online = false;
			ManagementClass mc = new ManagementClass("Win32_Printer");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject mo in moc)
			{
				string printerName = GetObjectPropertyString(mo, "PrinterName", "");
				if (printerName == name)
				{
					// if true is returned, the printer is turned off - does NOT work for network printers
					online = !GetObjectPropertyBool(mo, "WorkOffline", false);
				}
			}
			return online;
		}

		#endregion Methods that use one or more GetXXXX methods to gather data
	}
}
