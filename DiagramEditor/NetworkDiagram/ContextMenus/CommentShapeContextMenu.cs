 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.DiagramEditor.Properties;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
	internal sealed class CommentShapeContextMenu : DiagramContextMenu
	{
		static CommentShapeContextMenu _default = new CommentShapeContextMenu();

		ToolStripMenuItem mnuEditComment;

		private CommentShapeContextMenu()
		{
			InitMenuItems();
		}

		public static CommentShapeContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuEditComment.Text = Strings.MenuEditComment;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			ShapeContextMenu.Default.ValidateMenuItems(diagram);
			mnuEditComment.Enabled = (diagram.SelectedElementCount == 1);
		}

		private void InitMenuItems()
		{
			mnuEditComment = new ToolStripMenuItem(
				Strings.MenuEditComment,
				Resources.EditComment, mnuEditComment_Click);

			MenuList.AddRange(ShapeContextMenu.Default.MenuItems);
			MenuList.AddRange(new ToolStripItem[] {
				new ToolStripSeparator(),
				mnuEditComment,
			});
		}

		private void mnuEditComment_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				CommentShape commentShape = Diagram.TopSelectedElement as CommentShape;
				if (commentShape != null)
					commentShape.EditText();
			}
		}
	}
}