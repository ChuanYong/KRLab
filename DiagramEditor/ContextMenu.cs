 

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KRLab.DiagramEditor
{
	public abstract class ContextMenu
	{
		internal static readonly ContextMenuStrip MenuStrip = new ContextMenuStrip();

		List<ToolStripItem> menuItems = new List<ToolStripItem>();

		public IEnumerable<ToolStripItem> GetMenuItems(IDocument document)
		{
			ValidateMenuItems(document);
			return menuItems;
		}

		protected abstract IDocument Document
		{
			get;
		}

		internal IEnumerable<ToolStripItem> MenuItems
		{
			get { return menuItems; }
		}

		protected List<ToolStripItem> MenuList
		{
			get { return menuItems; }
		}

		public abstract void ValidateMenuItems(IDocument document);
	}
}
