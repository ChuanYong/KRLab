 

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace KRLab.DiagramEditor
{
	public interface IGraphics
	{
		Region Clip { get; set; }
		RectangleF ClipBounds { get; }
		Matrix Transform { get; set; }

		void DrawEllipse(Pen pen, int x, int y, int width, int height);
		void DrawImage(Image image, int x, int y);
		void DrawImage(Image image, Point point);
		void DrawLine(Pen pen, int x1, int y1, int x2, int y2);
		void DrawLine(Pen pen, Point pt1, Point pt2);
		void DrawLines(Pen pen, Point[] points);
		void DrawPath(Pen pen, GraphicsPath path);
		void DrawPolygon(Pen pen, Point[] points);
		void DrawRectangle(Pen pen, Rectangle rect);
		void DrawString(string s, Font font, Brush brush, PointF point);
		void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle);
		void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format);
		void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format);
		void FillEllipse(Brush brush, Rectangle rect);
		void FillEllipse(Brush brush, int x, int y, int width, int height);
		void FillPath(Brush brush, GraphicsPath path);
		void FillPolygon(Brush brush, Point[] points);
		void FillRectangle(Brush brush, Rectangle rect);

		void RotateTransform(float angle);
		void ScaleTransform(float sx, float sy);
		void TranslateTransform(float dx, float dy);
		void ResetTransform();

		void SetClip(GraphicsPath path, CombineMode combineMode);
		void SetClip(Rectangle rect, CombineMode combineMode);
		void SetClip(RectangleF rect, CombineMode combineMode);
		void SetClip(Region region, CombineMode combineMode);
	}
}
