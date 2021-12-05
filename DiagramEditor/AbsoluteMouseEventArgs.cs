 

using System;
using System.Drawing;
using System.Windows.Forms;

namespace KRLab.DiagramEditor
{
	public delegate void AbsoluteMouseEventHandler(object sender, AbsoluteMouseEventArgs e);

	public class AbsoluteMouseEventArgs
	{
		float x;
		float y;
		MouseButtons button;
		bool handled = false;
		float zoom;

		public AbsoluteMouseEventArgs(MouseButtons button, float x, float y, float zoom)
		{
			this.button = button;
			this.x = x;
			this.y = y;
			this.zoom = zoom;
		}

		public AbsoluteMouseEventArgs(MouseButtons button, PointF location, float zoom)
		{
			this.button = button;
			this.x = location.X;
			this.y = location.Y;
			this.zoom = zoom;
		}

		public AbsoluteMouseEventArgs(MouseEventArgs e, Point offset, float zoom)
		{
			this.button = e.Button;
			this.x = (e.X + offset.X) / zoom;
			this.y = (e.Y + offset.Y) / zoom;
			this.zoom = zoom;
		}

		public AbsoluteMouseEventArgs(MouseEventArgs e, IDocument document)
			: this(e, document.Offset, document.Zoom)
		{
		}

		public MouseButtons Button
		{
			get { return button; }
		}

		public float X
		{
			get { return x; }
		}

		public float Y
		{
			get { return y; }
		}

		public PointF Location
		{
			get { return new PointF(x, y); }
		}

		public bool Handled
		{
			get { return handled; }
			set { handled = value; }
		}

		public float Zoom
		{
			get { return zoom; }
			set { zoom = value; }
		}
	}
}
