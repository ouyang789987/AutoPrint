using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing; 
using System.Drawing.Printing; 
using System.Linq;
using System.Text;

namespace AutoPrint
{
	//class PrintEngine : PrintDocument
	//{
	//    private ArrayList	m_printObjects		= new ArrayList();
	//    private int			m_penWidth			= 6;
	//    private SolidBrush	m_brushBlack		= new SolidBrush(Color.Black);
	//    private SolidBrush	m_brushYellow		= new SolidBrush(Color.Yellow);
	//    private SolidBrush	m_brushCyan			= new SolidBrush(Color.Cyan);
	//    private SolidBrush	m_brushLightCyan	= new SolidBrush(Color.Cyan);
	//    private SolidBrush	m_brushDarkCyan		= new SolidBrush(Color.Cyan);
	//    private SolidBrush	m_brushMagenta		= new SolidBrush(Color.Magenta);
	//    private SolidBrush	m_brushLightMagenta	= new SolidBrush(Color.Magenta);
	//    private SolidBrush	m_brushDarkMagenta	= new SolidBrush(Color.Magenta);

	//    public int		  PenWidth		{ get { return m_penWidth; } }
	//    public SolidBrush Black			{ get { return m_brushBlack; } }
	//    public SolidBrush Yellow		{ get { return m_brushYellow; } }
	//    public SolidBrush Cyan			{ get { return m_brushCyan; } }
	//    public SolidBrush LightCyan		{ get { return m_brushLightCyan; } }
	//    public SolidBrush DarkCyan		{ get { return m_brushDarkCyan; } }
	//    public SolidBrush Magenta		{ get { return m_brushMagenta; } }
	//    public SolidBrush LightMagenta	{ get { return m_brushLightMagenta; } }
	//    public SolidBrush DarkMagenta	{ get { return m_brushDarkMagenta; } }

	//    public ~PrintEngine()
	//    {
	//        m_brushBlack.Dispose();
	//        m_brushYellow.Dispose();
	//        m_brushCyan.Dispose();
	//        m_brushLightCyan.Dispose();
	//        m_brushDarkCyan.Dispose();
	//        m_brushMagenta.Dispose();
	//        m_brushLightMagenta.Dispose();
	//        m_brushDarkMagenta.Dispose();
	//    }

	//    // AddPrintObject - add a print object the document...
	//    public void AddPrintObject(IPrintable printObject)
	//    {
	//      m_printObjects.Add(printObject);
	//    }
	//}

	//public interface IPrintable
	//{
	//    void Print(PrintElement element);
	//} 

	//public class NozzleExercise : IPrintable
	//{

	//    public void Print(PrintElement element)
	//    {
	//        string dateString = DateTime.Now.ToString();
	//        FontStyle style = FontStyle.Bold | FontStyle.Italic | FontStyle.Underline;
	//        FontFamily family = new FontFamily("Arial Black");
	//        Font font = new Font(family, 14.0f, style); 

	//    }
	//}

	//public interface IPrintPrimitive
	//{
	//    // CalculateHeight - work out how tall the primitive is...
	//    float CalculateHeight(PrintEngine engine, Graphics graphics);
	//    // Print - tell the primitive to physically draw itself...
	//    void Draw(PrintEngine engine, float yPos, Graphics graphics, Rectangle elementBounds);
	//} 

	//public class PrintPrimitiveRule : IPrintPrimitive 
	//{
	//    // CalculateHeight - work out how tall the primitive is...
	//    public float CalculateHeight(PrintEngine engine, Graphics graphics)
	//    {
	//        // we're always five units tall...
	//        return 5;
	//    } 

	//    // Print - draw the rule...
	//    public void Draw(PrintEngine engine, float yPos, Graphics graphics, Rectangle elementBounds)
	//    {
	//        // draw a line...
	//        Pen pen = new Pen(engine.PrintBrush);
	//        graphics.DrawLine(pen, elementBounds.Left, yPos + 2, elementBounds.Right, yPos + 2);
	//    } 
	//}
}
