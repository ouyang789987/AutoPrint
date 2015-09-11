using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace AutoPrint
{
	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	public class RectPadding
	{
		private int m_top		= 0;
		private int m_bottom	= 0;
		private int m_left		= 0;
		private int m_right		= 0;

		//--------------------------------------------------------------------------------
		public int Top
		{
			get { return m_top; }
			set { m_top = value; }
		}
		//--------------------------------------------------------------------------------
		public int Bottom
		{
			get { return m_bottom; }
			set { m_bottom = value; }
		}
		//--------------------------------------------------------------------------------
		public int Left
		{
			get { return m_left; }
			set { m_left = value; }
		}
		//--------------------------------------------------------------------------------
		public int Right
		{
			get { return m_right; }
			set { m_right = value; }
		}
	}


	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	public class VerticalLabel : IDisposable
	{
		#region Data members
		private bool				m_disposed;
		private Color				m_foreColor		= System.Drawing.SystemColors.Control;
		private Color				m_backColor		= System.Drawing.SystemColors.ControlText;
		private Color				m_borderColor	= Color.Black;
		private Point				m_location		= new Point(0,0);
		private Size				m_size			= new Size(0,0);
		private Font				m_font			= new System.Drawing.Font("Arial", 8.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		private BorderStyle			m_borderStyle	= BorderStyle.None;
		private bool				m_visible		= true;
		private ContentAlignment	m_textAlign		= System.Drawing.ContentAlignment.TopLeft;
		private Bitmap				m_bitmap		= null;
		private string				m_text			= "";
		private RectPadding			m_padding		= new RectPadding();
		#endregion Data members

		#region Properties
		//--------------------------------------------------------------------------------
		public bool Disposed
		{
			get { return m_disposed; }
			set { m_disposed = value; }
		}
		//--------------------------------------------------------------------------------
		public Color ForeColor
		{
			get { return m_foreColor; }
			set { m_foreColor = value; }
		}
		//--------------------------------------------------------------------------------
		public Color BackColor
		{
			get { return m_backColor; }
			set { m_backColor = value; }
		}
		//--------------------------------------------------------------------------------
		public Color BorderColor
		{
			get { return m_borderColor; }
			set { m_borderColor = value; }
		}
		//--------------------------------------------------------------------------------
		public Point Location
		{
			get { return m_location; }
			set { m_location = value; }
		}
		//--------------------------------------------------------------------------------
		public Size Size
		{
			get { return m_size; }
			set { m_size = value; }
		}
		//--------------------------------------------------------------------------------
		public Font Font
		{
			get { return m_font; }
			set { m_font = value; }
		}
		//--------------------------------------------------------------------------------
		public BorderStyle BorderStyle
		{
			get { return m_borderStyle; }
			set { m_borderStyle = value; }
		}
		//--------------------------------------------------------------------------------
		public bool Visible 
		{
			get { return m_visible; }
			set { m_visible = value; }
		}
		//--------------------------------------------------------------------------------
		public ContentAlignment TextAlign
		{
			get { return m_textAlign; }
			set { m_textAlign = value; }
		}
		//--------------------------------------------------------------------------------
		public Bitmap Bitmap
		{
			get { return m_bitmap; }
		}
		//--------------------------------------------------------------------------------
		public string Text
		{
			get { return m_text; }
			set { m_text = value; }
		}
		//--------------------------------------------------------------------------------
		public RectPadding Padding
		{
			get { return m_padding; }
			set { m_padding = value; }
		}
		#endregion Properties


		//--------------------------------------------------------------------------------
		public VerticalLabel()
		{
		}

		//--------------------------------------------------------------------------------
		~VerticalLabel()
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
					if (m_bitmap != null)
					{
						m_bitmap.Dispose();
						m_bitmap = null;
					}
				}

				//free unmanaged resources

			}
			Disposed = true;
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		///  Create the bitma
		/// </summary>
		public void CreateLabelBitmap()
		{
			Rectangle	rect			= new Rectangle(0, 0, this.Size.Width, this.Size.Height);
			Bitmap		bitmap			= new Bitmap(rect.Width, rect.Height);
			Graphics	graphics		= Graphics.FromImage(bitmap);
			Pen			rectPen			= null;
			SizeF		textSize;

			if (this.BorderStyle == BorderStyle.FixedSingle)
			{
				rectPen = new Pen(Color.Black, 1);
			}
			else
			{
				rectPen = new Pen(this.BackColor, 0);
			}

			textSize = graphics.MeasureString(this.Text, this.Font);

			graphics.Clear(this.BackColor);
			graphics.DrawRectangle(rectPen, rect.Left, rect.Top, rect.Width-1, rect.Height-1);

			Point point = CalcOrigin(rect, textSize);

			graphics.SmoothingMode		= System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			graphics.TextRenderingHint	= System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			TextRenderer.DrawText(graphics, this.Text, this.Font, point, this.ForeColor, this.BackColor);
			m_bitmap = bitmap;
			graphics.Dispose();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		///  Determine the bitmap rectangle's origin
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="textSize"></param>
		/// <returns></returns>
		private Point CalcOrigin(Rectangle rect, SizeF textSize)
		{
			Point point = new Point(0, 0);
			switch (this.TextAlign)
			{
				case ContentAlignment.TopCenter:
					point.X = rect.Left + (int)((rect.Width - textSize.Width) * 0.5);
					point.Y = rect.Top  + this.Padding.Top;
					break;
				case ContentAlignment.MiddleCenter:
					point.X = rect.Left + (int)((rect.Width - textSize.Width) * 0.5);
					point.Y = rect.Top  + (int)((rect.Height - textSize.Height) * 0.5);
					break;
				case ContentAlignment.BottomCenter:
					point.X = rect.Left   + (int)((rect.Width - textSize.Width) * 0.5);
					point.Y = rect.Bottom - this.Padding.Bottom - (int)textSize.Height;
					break;
				case ContentAlignment.TopLeft:
					point.X = rect.Left + this.Padding.Left;
					point.Y = rect.Top + this.Padding.Top;
					break;
				case ContentAlignment.MiddleLeft:
					point.X = rect.Left + this.Padding.Left;
					point.Y = rect.Top  + (int)((rect.Height - (int)textSize.Height) * 0.5);
					break;
				case ContentAlignment.BottomLeft:
					point.X = rect.Left + this.Padding.Left;
					point.Y = rect.Bottom - this.Padding.Bottom - (int)textSize.Height;
					break;
				case ContentAlignment.TopRight:
					point.X = rect.Right - this.Padding.Right - (int)textSize.Width;
					point.Y = rect.Top + this.Padding.Top;
					break;
				case ContentAlignment.MiddleRight:
					point.X = rect.Right - this.Padding.Right - (int)textSize.Width;
					point.Y = rect.Top  + (int)((rect.Height - (int)textSize.Height) * 0.5);
					break;
				case ContentAlignment.BottomRight:
					point.X = rect.Right - this.Padding.Right - (int)textSize.Width;
					point.Y = rect.Bottom - this.Padding.Bottom - (int)textSize.Height;
					break;
			}
			point.X += 1;
			point.Y += 1;
			return point;
		}

	}
}

