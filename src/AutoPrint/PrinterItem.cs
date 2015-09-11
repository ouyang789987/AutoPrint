using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AutoPrint
{
	public class PrinterItem
	{
		private string m_longName;
		private string m_shortName;
		private string m_colorBars;

		//--------------------------------------------------------------------------------
		public string LongName
		{
			get { return m_longName; }
			set { m_longName = value; }
		}
		//--------------------------------------------------------------------------------
		public string ShortName
		{
			get { return m_shortName; }
			set { m_shortName = value; }
		}
		//--------------------------------------------------------------------------------
		public string Colors
		{
			get { return m_colorBars; }
			set { m_colorBars = value; }
		}
		//--------------------------------------------------------------------------------
		public ColorBarFlags ColorBars
		{
			get { return Utility.MakeColorBars(m_colorBars); }
			set { m_colorBars = Utility.MakeColorString(value); }
		}
		//--------------------------------------------------------------------------------
		public XElement XElement
		{
			get 
			{ 
				return new XElement("PRINTER",
									new XElement("LongName", m_longName),
									new XElement("ShortName", m_shortName),
									new XElement("Colors", m_colorBars));
			}
		}

		//--------------------------------------------------------------------------------
		public PrinterItem() {}

		//--------------------------------------------------------------------------------
		public PrinterItem(string longName, string colorBars)
		{
			m_longName = longName;
			m_colorBars = colorBars;
		}

		//--------------------------------------------------------------------------------
		public PrinterItem(string longName, ColorBarFlags colorBars)
		{
			m_longName = longName;
			m_colorBars = Utility.MakeColorString(colorBars);
		}

		//--------------------------------------------------------------------------------
		public PrinterItem(XElement value)
		{
			SetXElement(value);
		}

		//--------------------------------------------------------------------------------
		public void SetXElement(XElement value)
		{
			if (value == null)
			{
				throw new AutoPrintException("XElement cannot be null.");
			}
			m_longName = value.Element("LongName").Value;
			m_shortName = value.Element("ShortName").Value;
			m_colorBars = value.Element("Colors").Value;
		}
	}
}
