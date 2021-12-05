 
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.DiagramEditor.Properties;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
	internal sealed class TypeShapeContextMenu : DiagramContextMenu
	{
		static TypeShapeContextMenu _default = new TypeShapeContextMenu();

		ToolStripMenuItem mnuSize, mnuAutoSize, mnuAutoWidth, mnuAutoHeight;
		ToolStripMenuItem mnuCollapseAllSelected, mnuExpandAllSelected;
		ToolStripMenuItem mnuEditMembers;

		private TypeShapeContextMenu()
		{
			InitMenuItems();
		}

		public static TypeShapeContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuSize.Text = Strings.MenuSize;
			mnuAutoSize.Text = Strings.MenuAutoSize;
			mnuAutoWidth.Text = Strings.MenuAutoWidth;
			mnuAutoHeight.Text = Strings.MenuAutoHeight;
			mnuCollapseAllSelected.Text = Strings.MenuCollapseAllSelected;
			mnuExpandAllSelected.Text = Strings.MenuExpandAllSelected;
			mnuEditMembers.Text = Strings.MenuEditMembers;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			ShapeContextMenu.Default.ValidateMenuItems(diagram);
			mnuEditMembers.Enabled = (diagram.SelectedElementCount == 1);
		}

		private void InitMenuItems()
		{ 
			mnuEditMembers = new ToolStripMenuItem(Strings.MenuEditMembers,
				Resources.EditMembers, mnuEditMembers_Click);
			mnuAutoSize = new ToolStripMenuItem(Strings.MenuAutoSize,
				null, mnuAutoSize_Click);
			mnuAutoWidth = new ToolStripMenuItem(Strings.MenuAutoWidth,
				null, mnuAutoWidth_Click);
			mnuAutoHeight = new ToolStripMenuItem(Strings.MenuAutoHeight,
				null, mnuAutoHeight_Click);
			mnuCollapseAllSelected = new ToolStripMenuItem(
				Strings.MenuCollapseAllSelected,
				null, mnuCollapseAllSelected_Click);
			mnuExpandAllSelected = new ToolStripMenuItem(
				Strings.MenuExpandAllSelected,
				null, mnuExpandAllSelected_Click);
			mnuSize = new ToolStripMenuItem(Strings.MenuSize, null,
				mnuAutoSize,
				mnuAutoWidth,
				mnuAutoHeight,
				new ToolStripSeparator(),
				mnuCollapseAllSelected,
				mnuExpandAllSelected
			);

			MenuList.AddRange(ShapeContextMenu.Default.MenuItems);
			MenuList.AddRange(new ToolStripItem[] {
				mnuSize,
				new ToolStripSeparator(),
				mnuEditMembers,
			});
		}

		private void mnuAutoSize_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AutoSizeOfSelectedShapes();
		}

		private void mnuAutoWidth_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AutoWidthOfSelectedShapes();
		}

		private void mnuAutoHeight_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AutoHeightOfSelectedShapes();
		}

		private void mnuCollapseAllSelected_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.CollapseAll(true);
		}

		private void mnuExpandAllSelected_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.ExpandAll(true);
		}

		private void mnuEditMembers_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				SimpleNodeShape typeShape = Diagram.TopSelectedElement as SimpleNodeShape;
				if (typeShape != null)
				{
					typeShape.IsActive = false;
					typeShape.EditText();
				}
			}
		}
	}
}