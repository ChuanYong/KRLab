

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;
using KRLab.DiagramEditor;

namespace KRLab.GUI
{
	[ToolStripItemDesignerAvailability(
		ToolStripItemDesignerAvailability.ToolStrip |
		ToolStripItemDesignerAvailability.StatusStrip)]
	[DefaultEvent("ZoomValueChanged")]
	[DefaultProperty("ZoomValue")]
	public class ZoomingToolStrip : ToolStripItem
	{
		const float MaxValue = Canvas.MaxZoom;
		const float MinValue = Canvas.MinZoom;
		const float DefaultValue = 1.0F;

		const int MinWidth = 100;
		const int PrecisionSize = 4;
		static readonly int SliderWidth = Properties.Resources.Slider.Width;
		static readonly int SliderHeight = Properties.Resources.Slider.Height;
		static readonly Pen linePen = SystemPens.ControlDarkDark;
		static readonly Pen disabledPen = SystemPens.ControlDark;

		float zoomValue = DefaultValue;

		public event EventHandler ZoomValueChanged;

		[DefaultValue(DefaultValue)]
		public float ZoomValue
		{
			get
			{
				return zoomValue;
			}
			set
			{
				if (value > MaxValue) value = MaxValue;
				if (value < MinValue) value = MinValue;

				if (zoomValue != value)
				{
					zoomValue = value;
					OnZoomValueChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		protected override Size DefaultSize
		{
			get
			{
				return new Size(MinWidth, base.DefaultSize.Height);
			}
		}

		public override Size GetPreferredSize(Size constrainingSize)
		{
			return new Size(
				Math.Max(MinWidth, Size.Width), Size.Height);
		}

		private void MoveSlider(int location, bool snapToCenter)
		{
			int center = Width / 2;

			// Mono hack for a ToolStripItem layout problem
			if (MonoHelper.IsRunningOnMono)
				location -= this.Bounds.Left;

			if (snapToCenter && Math.Abs(location - center) <= PrecisionSize)
				location = center;

			if (location < center)
			{
				int left = SliderWidth / 2;
				float scale = (DefaultValue - MinValue) / (center - left);
				ZoomValue = (location - left) * scale + MinValue;
			}
			else // location >= center
			{
				int right = Width - SliderWidth / 2;
				float scale = (MaxValue - DefaultValue) / (right - center);
				ZoomValue = (location - center) * scale + DefaultValue;
			}
		}

		protected virtual void OnZoomValueChanged(EventArgs e)
		{
			if (ZoomValueChanged != null)
				ZoomValueChanged(this, e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (Enabled && e.Button == MouseButtons.Left)
			{
				bool snapToCenter = (Control.ModifierKeys == Keys.None);
				MoveSlider(e.X, snapToCenter);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (Enabled && e.Button == MouseButtons.Left)
			{
				bool snapToCenter = (Control.ModifierKeys == Keys.None);
				MoveSlider(e.X, snapToCenter);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			Point center = new Point(Width / 2, Height / 2);
			int left = SliderWidth / 2;
			int right = Width - SliderWidth / 2;
			int top = center.Y - 3;
			int bottom = center.Y + 3;

			// Draw horizontal and vertical lines
			Pen pen = (Enabled) ? linePen : disabledPen;
			g.DrawLine(pen, left, center.Y, right, center.Y);
			g.DrawLine(pen, center.X, top, center.X, bottom);

			if (Enabled)
				DrawSlider(g, TrackBarThumbState.Normal);
			else
				DrawSlider(g, TrackBarThumbState.Disabled);
		}

		private void DrawSlider(Graphics g, TrackBarThumbState state)
		{
			int sliderLocation;

			if (ZoomValue < DefaultValue)
			{
				int regionWidth = Width / 2 - SliderWidth / 2;
				float scale = (float) regionWidth / (DefaultValue - MinValue);
				sliderLocation = (int) Math.Round(scale * (ZoomValue - MinValue)) + SliderWidth / 2;
			}
			else
			{
				int regionWidth = Width / 2 - SliderWidth / 2;
				float scale = (float) regionWidth / (MaxValue - DefaultValue);
				sliderLocation = (int) Math.Round(scale * (ZoomValue - DefaultValue)) + Width / 2;
			}

			Image slider;
			if (state == TrackBarThumbState.Disabled)
				slider = Properties.Resources.DisabledSlider;
			else
				slider = Properties.Resources.Slider;
			int top = Height / 2 - SliderHeight / 2;
			int left = sliderLocation - SliderWidth / 2;

			g.DrawImage(slider, left, top, SliderWidth, SliderHeight);	
		}
	}
}