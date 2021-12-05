 

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KRLab.DiagramEditor
{
	public abstract class DynamicMenu : IEnumerable<ToolStripMenuItem>
	{
		int preferredIndex;

		public DynamicMenu()
		{
			this.preferredIndex = -1;
		}

		public DynamicMenu(int preferredIndex)
		{
			this.preferredIndex = preferredIndex;
		}

		public int PreferredIndex
		{
			get { return preferredIndex; }
		}

		public IEnumerator<ToolStripMenuItem> GetEnumerator()
		{
			return GetMenuItems().GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public abstract IEnumerable<ToolStripMenuItem> GetMenuItems();

		public abstract ToolStrip GetToolStrip();

		public abstract void SetReference(IDocument document);
	}
}
