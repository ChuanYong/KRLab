
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Dialogs;
using KRLab.DiagramEditor.NetworkDiagram.ContextMenus;
using KRLab.DiagramEditor.NetworkDiagram.Editors;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.Shapes
{
    public abstract class SimpleNodeShape:NodeShape
    {
        static SimpleNodeEditor editor = new SimpleNodeEditor();
		static Pen borderPen = new Pen(Color.Black);
		static SolidBrush backgroundBrush = new SolidBrush(Color.White);
		static SolidBrush textBrush = new SolidBrush(Color.Black);
		static StringFormat format = new StringFormat(StringFormat.GenericTypographic);
         
		bool editorShowed = false;

        public string Text
        {
            get { return Node.Name; }
            set { Node.Name = value; }
        } 

		static SimpleNodeShape()
		{
			format.Trimming = StringTrimming.EllipsisWord;
			format.FormatFlags = StringFormatFlags.LineLimit;
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="comment"/> is null.
		/// </exception>
        internal SimpleNodeShape(NodeBase node)
            : base(node)
		{ 
		}

		public override IEntity Entity
		{
			get { return Node; }
		}

		protected override Size DefaultSize
		{
			get
			{
				return defaultMinSize;
			}
		}

        protected override bool CloneEntity(Diagram diagram)
        {
            return diagram.InsertNode(Node.Clone());
        }

		protected override int GetBorderWidth(Style style)
		{
			return style.CommentBorderWidth;
		}

        protected override EditorWindow GetEditorWindow()
        {
            if (IsActive)
            {
                return editor;
            }
            else
            {
                return null;
            }
        }
        protected override int GetRoundingSize(Style style)
        {
            return style.ClassRoundingSize;
        }

		internal Rectangle GetTextRectangle()
		{
			return new Rectangle(Left, Top ,Width, Height);
		}

		protected override void OnMove(MoveEventArgs e)
		{
			base.OnMove(e);
			HideEditor();
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			if (editorShowed)
			{
				editor.Relocate(this);
				if (!editor.Focused)
					editor.Focus();
			}
		}

		protected override void OnMouseDown(AbsoluteMouseEventArgs e)
		{ 
			base.OnMouseDown(e);
		}

		protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
		{
			if (Contains(e.Location) && e.Button == MouseButtons.Left)
				ShowEditor();
		}

		protected internal override void ShowEditor()
		{ 
			if (!editorShowed)
			{
				editor.Relocate(this);
				editor.Init(this);
				ShowWindow(editor);
				editor.Focus();
				editorShowed = true;
			}
		}

		protected internal override void HideEditor()
		{
			if (editorShowed)
			{
				HideWindow(editor);
				editorShowed = false;
			}
		}

		protected internal override void MoveWindow()
		{
			HideEditor();
		}

		internal void EditText()
		{
			using (EditCommentDialog dialog = new EditCommentDialog(Text))
			{
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Text = dialog.InputText; 
                }

			}
		} 

		protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(Diagram diagram)
        {
            return TypeShapeContextMenu.Default.GetMenuItems(diagram); 
		}

		private void DrawText(IGraphics g, bool onScreen, Style style)
		{
			Rectangle textBounds = GetTextRectangle();

			if (string.IsNullOrEmpty(Text) && onScreen)
			{
				textBrush.Color = Color.FromArgb(128, style.CommentTextColor);
				g.DrawString(Strings.DoubleClickToEdit,style.CommentFont, textBrush, textBounds, format);
			}
			else
			{
				textBrush.Color = style.CommentTextColor;
				g.DrawString(Text, style.CommentFont, textBrush, textBounds, format); 
			}
		}

		public override void Draw(IGraphics g, bool onScreen, Style style)
		{
            DrawSurface(g, onScreen, style);
			DrawText(g, onScreen, style);
		}
        private void DrawSurface(IGraphics g, bool onScreen, Style style)
        {
            //GdiGraphics gdi = (GdiGraphics)g;
            //gdi.DrawEllipse(borderPen, Left, Top, Width, Height);

            // Update graphical objects
            backgroundBrush.Color = style.CommentBackColor;
            borderPen.Color = style.CommentBorderColor;
            borderPen.Width = style.CommentBorderWidth;
            if (style.IsCommentBorderDashed)
                borderPen.DashPattern = borderDashPattern;
            else
                borderPen.DashStyle = DashStyle.Solid;

            // Create shape pattern
            GraphicsPath path = new GraphicsPath();
            path.AddLine(Left, Top, Right, Top);
            path.AddLine(Right, Top, Right, Bottom);
            path.AddLine(Right, Bottom, Left, Bottom);
            path.CloseFigure();

            // Draw shadow first
            if ((!onScreen || !IsSelected) && !style.ShadowOffset.IsEmpty)
            {
                shadowBrush.Color = style.ShadowColor;
                g.TranslateTransform(style.ShadowOffset.Width, style.ShadowOffset.Height);
                g.FillPath(shadowBrush, path);
                g.TranslateTransform(-style.ShadowOffset.Width, -style.ShadowOffset.Height);
            }

            // Draw borders & background
            g.FillPath(backgroundBrush, path);
            g.DrawPath(borderPen, path);

            path.Dispose();
        }

		protected override float GetRequiredWidth(Graphics g, Style style)
		{
			return Width;
		}

		protected override int GetRequiredHeight()
		{
			return Height;
		}

        protected override GradientStyle GetGradientHeaderStyle(Style style)
        {
            return style.NodeGradientHeaderStyle;
        }

        protected override Color GetBorderColor(Style style)
        {
            return style.NodeBorderColor;
        }

        protected override bool IsBorderDashed(Style style)
        {
            return style.IsNodeBorderDashed;
        }

        protected override Color GetHeaderColor(Style style)
        {
            return style.NodeHeaderColor;
        }

        protected override Color GetBackgroundColor(Style style)
        {
            return style.NodeBackgroundColor;
        }

		public override string ToString()
		{
			return Strings.Comment;
		}
    }
}
