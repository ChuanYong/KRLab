 

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace KRLab.DiagramEditor.NetworkDiagram.Connections
{
	internal static class Arrowhead
	{
		public const int ClosedArrowWidth = 12;
		public const int ClosedArrowHeight = 17;
		public static readonly Size ClosedArrowSize = new Size(ClosedArrowWidth, ClosedArrowHeight);
		static readonly GraphicsPath closedArrowPath = new GraphicsPath();

		public const int OpenArrowWidth = 10;
		public const int OpenArrowHeight = 16;
		public static readonly Size OpenArrowSize = new Size(OpenArrowWidth, OpenArrowHeight);
		static readonly Point[] openArrowPoints;

		static Arrowhead()
		{
			openArrowPoints = new Point[] {
				new Point(-OpenArrowWidth / 2, OpenArrowHeight),
				new Point(0, 0),
				new Point(OpenArrowWidth / 2, OpenArrowHeight)
			};

			closedArrowPath.AddLines(new Point[] {
				new Point(0, 0),
				new Point(ClosedArrowWidth / 2, ClosedArrowHeight),
				new Point(-ClosedArrowWidth / 2, ClosedArrowHeight)
			});
			closedArrowPath.CloseFigure();
		}

		public static GraphicsPath ClosedArrowPath
		{
			get { return closedArrowPath; }
		}

		public static Point[] OpenArrowPoints
		{
			get { return openArrowPoints; }
		}
	}
}
