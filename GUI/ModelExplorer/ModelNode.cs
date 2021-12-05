 

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KRLab.GUI.ModelExplorer
{
	public abstract class ModelNode : TreeNode
	{
		bool editingLabel = false;
		bool deleted = false;

		protected ModelNode()
		{
		}

		public ModelView ModelView
		{
			get { return this.TreeView as ModelView; }
		}

		public bool EditingLabel
		{
			get { return editingLabel; }
		}

		public virtual void BeforeDelete()
		{
			foreach (ModelNode node in Nodes)
			{
				node.BeforeDelete();
			}
		}

		public void Delete()
		{
			if (!deleted)
			{
				BeforeDelete();
				Remove();
				deleted = true;
			}
		}

		public void EditLabel()
		{
			if (!editingLabel)
			{
				editingLabel = true;
				this.BeginEdit(); 
			}
		}

		internal void LabelEdited()
		{
			editingLabel = false; 
		}

		public virtual void LabelModified(NodeLabelEditEventArgs e)
		{
		}

		public virtual void DoubleClick()
		{
		}

		public virtual void EnterPressed()
		{
		}

		protected internal virtual void AfterInitialized()
		{
			foreach (ModelNode node in Nodes)
				node.AfterInitialized();
		}
	}
}
