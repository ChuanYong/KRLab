

using System;
using System.Drawing;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public sealed partial class CommentEditor : EditorWindow
	{
		CommentShape shape = null;

		public CommentEditor()
		{
			InitializeComponent();
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
		}

		internal override void Init(DiagramElement element)
		{
			shape = (CommentShape) element;
			
			txtComment.BackColor = Style.CurrentStyle.CommentBackColor;
			txtComment.ForeColor = Style.CurrentStyle.CommentTextColor;
			txtComment.Text = shape.Comment.Text;

			Font font = Style.CurrentStyle.CommentFont;
			txtComment.Font = new Font(font.FontFamily,
				font.SizeInPoints * shape.Diagram.Zoom, font.Style);
		}

		internal override void Relocate(DiagramElement element)
		{
			Relocate((CommentShape) element);
		}

		internal void Relocate(CommentShape shape)
		{
			Diagram diagram = shape.Diagram;
			if (diagram != null)
			{
				Rectangle absolute = shape.GetTextRectangle();
				// The following lines are required because of a .NET bug:
				// http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=380085
				if (!MonoHelper.IsRunningOnMono)
				{
					absolute.X -= 3;
					absolute.Width += 3;
				}
				
				this.SetBounds(
					(int) (absolute.X * diagram.Zoom) - diagram.Offset.X + ParentLocation.X,
					(int) (absolute.Y * diagram.Zoom) - diagram.Offset.Y + ParentLocation.Y,
					(int) (absolute.Width * diagram.Zoom),
					(int) (absolute.Height * diagram.Zoom));
			}
		}

		public override void ValidateData()
		{
			shape.Comment.Text = txtComment.Text;
		}

		private void txtComment_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.None ||
				e.KeyCode == Keys.Escape)
			{
				shape.HideEditor();
			}
		}
	}
}
