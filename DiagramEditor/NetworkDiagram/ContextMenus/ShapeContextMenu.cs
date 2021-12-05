 
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.DiagramEditor.Properties;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
	internal sealed class ShapeContextMenu : DiagramContextMenu
	{
		static ShapeContextMenu _default = new ShapeContextMenu();

		ToolStripMenuItem mnuAlign;
		ToolStripMenuItem mnuAlignTop, mnuAlignLeft, mnuAlignBottom, mnuAlignRight;
		ToolStripMenuItem mnuAlignHorizontal, mnuAlignVertical;
		ToolStripMenuItem mnuMakeSameSize;
		ToolStripMenuItem mnuSameWidth, mnuSameHeight, mnuSameSize;

		private ShapeContextMenu()
		{
			InitMenuItems();
		}

		public static ShapeContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuAlign.Text = Strings.MenuAlign;
			mnuAlignTop.Text = Strings.MenuAlignTop;
			mnuAlignLeft.Text = Strings.MenuAlignLeft;
			mnuAlignBottom.Text = Strings.MenuAlignBottom;
			mnuAlignRight.Text = Strings.MenuAlignRight;
			mnuAlignHorizontal.Text = Strings.MenuAlignHorizontal;
			mnuAlignVertical.Text = Strings.MenuAlignVertical;
			mnuMakeSameSize.Text = Strings.MenuMakeSameSize;
			mnuSameWidth.Text = Strings.MenuSameWidth;
			mnuSameHeight.Text = Strings.MenuSameHeight;
			mnuSameSize.Text = Strings.MenuSameSize;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			GeneralContextMenu.Default.ValidateMenuItems(diagram);

			bool multiSelection = (diagram.SelectedElementCount >= 2);
			mnuAlign.Enabled = multiSelection;
			mnuAlignTop.Enabled = multiSelection;
			mnuAlignLeft.Enabled = multiSelection;
			mnuAlignBottom.Enabled = multiSelection;
			mnuAlignRight.Enabled = multiSelection;
			mnuAlignHorizontal.Enabled = multiSelection;
			mnuAlignVertical.Enabled = multiSelection;
			mnuMakeSameSize.Enabled = multiSelection;
			mnuSameWidth.Enabled = multiSelection;
			mnuSameHeight.Enabled = multiSelection;
			mnuSameSize.Enabled = multiSelection;
		}

		private void InitMenuItems()
		{
			mnuAlignTop = new ToolStripMenuItem(Strings.MenuAlignTop,
				Resources.AlignTop, mnuAlignTop_Click);
			mnuAlignLeft = new ToolStripMenuItem(Strings.MenuAlignLeft,
				Resources.AlignLeft, mnuAlignLeft_Click);
			mnuAlignBottom = new ToolStripMenuItem(Strings.MenuAlignBottom,
				Resources.AlignBottom, mnuAlignBottom_Click);
			mnuAlignRight = new ToolStripMenuItem(Strings.MenuAlignRight,
				Resources.AlignRight, mnuAlignRight_Click);
			mnuAlignHorizontal = new ToolStripMenuItem(Strings.MenuAlignHorizontal,
				Resources.AlignHorizontal, mnuAlignHorizontal_Click);
			mnuAlignVertical = new ToolStripMenuItem(Strings.MenuAlignVertical,
				Resources.AlignVertical, mnuAlignVertical_Click);
			mnuAlign = new ToolStripMenuItem(Strings.MenuAlign, null,
				mnuAlignTop,
				mnuAlignLeft,
				mnuAlignBottom,
				mnuAlignRight,
				new ToolStripSeparator(),
				mnuAlignHorizontal,
				mnuAlignVertical
			);

			mnuSameWidth = new ToolStripMenuItem(Strings.MenuSameWidth,
				null, mnuSameWidth_Click);
			mnuSameHeight = new ToolStripMenuItem(Strings.MenuSameHeight,
				null, mnuSameHeight_Click);
			mnuSameSize = new ToolStripMenuItem(Strings.MenuSameSize,
				null, mnuSameSize_Click);
			mnuMakeSameSize = new ToolStripMenuItem(Strings.MenuMakeSameSize, null,
				mnuSameWidth,
				mnuSameHeight,
				mnuSameSize
			);

			MenuList.AddRange(GeneralContextMenu.Default.MenuItems);
			MenuList.AddRange(new ToolStripItem[] {
				new ToolStripSeparator(),
				mnuAlign,
				mnuMakeSameSize,
			});
		}

		private void mnuAlignTop_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AlignTop();
		}

		private void mnuAlignLeft_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AlignLeft();
		}

		private void mnuAlignBottom_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AlignBottom();
		}

		private void mnuAlignRight_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AlignRight();
		}

		private void mnuAlignHorizontal_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AlignHorizontal();
		}

		private void mnuAlignVertical_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AlignVertical();
		}

		private void mnuSameWidth_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AdjustToSameWidth();
		}

		private void mnuSameHeight_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AdjustToSameHeight();
		}

		private void mnuSameSize_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.AdjustToSameSize();
		}
	}
}
