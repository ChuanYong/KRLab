 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.DiagramEditor.Properties;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
	public sealed class GeneralContextMenu : DiagramContextMenu
	{
		static GeneralContextMenu _default = new GeneralContextMenu();

		ToolStripMenuItem mnuCut;
		ToolStripMenuItem mnuCopy;
		ToolStripMenuItem mnuDelete;
		ToolStripMenuItem mnuCopyAsImage;
		ToolStripMenuItem mnuSaveAsImage;

		private GeneralContextMenu()
		{
			InitMenuItems();
		}

		public static GeneralContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuCut.Text = Strings.MenuCut;
			mnuCopy.Text = Strings.MenuCopy;
			mnuDelete.Text = Strings.MenuDelete;
			mnuCopyAsImage.Text = Strings.MenuCopyImageToClipboard;
			mnuSaveAsImage.Text = Strings.MenuSaveSelectionAsImage;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			mnuCut.Enabled = diagram.CanCutToClipboard;
			mnuCopy.Enabled = diagram.CanCopyToClipboard;
		}

		private void InitMenuItems()
		{
			mnuCut = new ToolStripMenuItem(Strings.MenuCut,
				Resources.Cut, mnuCut_Click);
			mnuCopy = new ToolStripMenuItem(Strings.MenuCopy,
				Resources.Copy, mnuCopy_Click);
			mnuDelete = new ToolStripMenuItem(Strings.MenuDelete,
				Resources.Delete, mnuDelete_Click);
			mnuCopyAsImage = new ToolStripMenuItem(
				Strings.MenuCopyImageToClipboard,
				Resources.CopyAsImage, mnuCopyAsImage_Click);
			mnuSaveAsImage = new ToolStripMenuItem(
				Strings.MenuSaveSelectionAsImage,
				Resources.Image, mnuSaveAsImage_Click);

			MenuList.AddRange(new ToolStripItem[] {
				mnuCut,
				mnuCopy,
				mnuDelete,
				new ToolStripSeparator(),
				mnuCopyAsImage,
				mnuSaveAsImage,
			});
		}

		private void mnuCut_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.Cut();
		}

		private void mnuCopy_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.Copy();
		}

		private void mnuDelete_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.DeleteSelectedElements();
		}

		private void mnuCopyAsImage_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.CopyAsImage();
		}

		private void mnuSaveAsImage_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.SaveAsImage(true);
		}
	}
}