

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.Core;

namespace KRLab.GUI.ModelExplorer
{
	public abstract class ProjectItemNode : ModelNode
	{
		protected ProjectItemNode()
		{
		}

		public abstract IProjectItem ProjectItem { get; }

		public abstract override void LabelModified(NodeLabelEditEventArgs e);
	}
}
