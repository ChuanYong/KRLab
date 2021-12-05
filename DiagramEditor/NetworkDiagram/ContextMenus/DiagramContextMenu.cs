

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
	public abstract class DiagramContextMenu : ContextMenu
	{
		Diagram diagram = null;

		protected sealed override IDocument Document
		{
			get { return diagram; }
		}

		protected Diagram Diagram
		{
			get { return diagram; }
		}

		public sealed override void ValidateMenuItems(IDocument document)
		{
			ValidateMenuItems(document as Diagram);
		}

		public virtual void ValidateMenuItems(Diagram diagram)
		{
			this.diagram = diagram;
		}
	}
}