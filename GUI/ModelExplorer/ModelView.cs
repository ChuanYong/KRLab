
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using KRLab.DiagramEditor;
using KRLab.Core;
using KRLab.GUI.Properties;
using KRLab.Translations;
using System.Collections.Generic;
using System.Linq;

namespace KRLab.GUI.ModelExplorer
{
	public partial class ModelView : TreeView
	{
		Workspace workspace = null;
		Font boldFont, normalFont;

		public event DocumentEventHandler DocumentOpening;

		public ModelView()
		{
			InitializeComponent();
            this.Controls.Add(lblAddProjectHint);
			UpdateTexts();
			normalFont = new Font(this.Font, FontStyle.Regular);
			boldFont = new Font(this.Font, FontStyle.Bold);
			this.AllowDrop = true;
			this.Dock = DockStyle.Fill;
		}

		private void UpdateTexts()
		{
			mnuNewProject.Text = Strings.MenuNewProject;
			mnuOpen.Text = Strings.MenuOpen;
			mnuOpenFile.Text = Strings.MenuOpenFile;
			mnuSaveAll.Text = Strings.MenuSaveAllProjects;
            mnuExport.Text = Strings.MenuExport;
			mnuCloseAll.Text = Strings.MenuCloseAllProjects;

            lblAddProjectHint.Text = Strings.AddProjectHint;
		}

		[Browsable(false)]
		public Workspace Workspace
		{
			get
			{
				return workspace;
			}
			set
			{
				if (workspace != value)
				{
					if (workspace != null)
					{
						workspace.ActiveProjectChanged -= workspace_ActiveProjectChanged;
						workspace.ProjectAdded -= workspace_ProjectAdded;
						workspace.ProjectRemoved -= workspace_ProjectRemoved;
						RemoveProjects();
					}
					workspace = value;
					if (workspace != null)
					{
						workspace.ActiveProjectChanged += workspace_ActiveProjectChanged;
						workspace.ProjectAdded += workspace_ProjectAdded;
						workspace.ProjectRemoved += workspace_ProjectRemoved;
						LoadProjects();
					}
                    lblAddProjectHint.Visible = (workspace != null && !workspace.HasProject);
				}
			}
		}

		private void AddProject(Project project)
		{
			ModelNode projectNode = new ProjectNode(project);
			Nodes.Add(projectNode);
			projectNode.AfterInitialized();

			SelectedNode = projectNode;
			//projectNode.Expand();
            lblAddProjectHint.Visible = false;
            Nodes[0].EnsureVisible();

			if (project.ItemCount == 1)
			{
				foreach (IProjectItem item in project.Items)
				{
					IDocument document = item as IDocument;
					if (document != null)
						OnDocumentOpening(new DocumentEventArgs(document));
				}
			}
			if (project.IsUntitled)
			{
				projectNode.EditLabel();
			}
		}

		private void RemoveProject(Project project)
		{
			foreach (ProjectNode projectNode in Nodes)
			{
				if (projectNode.Project == project)
				{
					projectNode.Delete();
					break;
				}
			}
            if (!workspace.HasProject)
            {
                lblAddProjectHint.Visible = true;
            }
		}

		private void RemoveProjects()
		{
			foreach (ModelNode node in Nodes)
			{
				node.BeforeDelete();
			}
			Nodes.Clear();
            lblAddProjectHint.Visible = true;
		}

		private void LoadProjects()
		{
			foreach (Project project in workspace.Projects)
			{
				AddProject(project);
			}
		}

		private void workspace_ActiveProjectChanged(object sender, EventArgs e)
		{
			foreach (ProjectNode node in Nodes)
			{
				if (node.Project == Workspace.ActiveProject)
					node.NodeFont = boldFont;
				else
					node.NodeFont = normalFont;
				node.Text = node.Text; // Little hack to update the text's clipping size
			}

			if (MonoHelper.IsRunningOnMono)
				this.Refresh();
		}

		private void workspace_ProjectAdded(object sender, ProjectEventArgs e)
		{
			AddProject(e.Project);
		}

		private void workspace_ProjectRemoved(object sender, ProjectEventArgs e)
		{
			RemoveProject(e.Project);
		}

		private void lblAddProject_RightClick(object sender, EventArgs e)
		{
			if (workspace != null && !workspace.HasProject)
			{
				workspace.AddEmptyProject(ProjectType.gsn);
			}
		}


		protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
		{
			base.OnNodeMouseDoubleClick(e);
			ModelNode node = (ModelNode) e.Node;
			node.DoubleClick();
		} 

        protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.Enter)
			{
				ModelNode selectedNode = SelectedNode as ModelNode;
				if (selectedNode != null)
					selectedNode.EnterPressed();
			}
			else if (e.KeyCode == Keys.F2)
			{
				ModelNode selectedNode = SelectedNode as ModelNode;
				if (selectedNode != null)
					selectedNode.EditLabel();
			}
		}

		protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
		{
			base.OnBeforeCollapse(e);

            // Prevent top level nodes to be collapsed
            //双击面板上的项目节点，阻止收缩
            //if (e.Node.Level == 0)
            //    e.Cancel = true;
            //双击面板上的项目节点，树节点收缩
            if (e.Node.Level == 0)
				e.Cancel = false;
		}

		protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
		{
			base.OnBeforeLabelEdit(e);

			ModelNode node = (ModelNode) e.Node;
			if (!node.EditingLabel)
				e.CancelEdit = true;
		}

		protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
		{
			base.OnAfterLabelEdit(e);
			
			ModelNode node = (ModelNode) e.Node;
			node.LabelEdited();	
			if (!e.CancelEdit && e.Label != null)
			{ 
				node.LabelModified(e); 
			}
		}
         
		protected internal virtual void OnDocumentOpening(DocumentEventArgs e)
		{
            if (DocumentOpening != null)
            {
                DocumentOpening(this, e);
            }
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			normalFont.Dispose();
			boldFont.Dispose();
			normalFont = new Font(this.Font, FontStyle.Regular);
			boldFont = new Font(this.Font, FontStyle.Bold);
		}
		 
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
			// Move the dragged node when the left mouse button is used.
			if (e.Button == MouseButtons.Left)
			{
				DoDragDrop(e.Item, DragDropEffects.Move);
			}

			// Copy the dragged node when the right mouse button is used.
			else if (e.Button == MouseButtons.Right)
			{
				DoDragDrop(e.Item, DragDropEffects.Copy);
			}
		}

		TreeNode draggedNode = null;
		TreeNode targetNode = null;
        protected override void OnDragEnter(DragEventArgs drgevent)
		{
			drgevent.Effect = drgevent.AllowedEffect;
			Point targetPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
			draggedNode = GetNodeAt(targetPoint);
		}

        protected override void OnDragOver(DragEventArgs drgevent)
        {		
			Point targetPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
			SelectedNode = GetNodeAt(targetPoint);
			targetNode = SelectedNode;
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
		{ 
			// Confirm that the node at the drop location is not 
			// the dragged node or a descendant of the dragged node.
			if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
			{
				// If it is a move operation, remove the node from its current 
				// location and add it to the node at the drop location.
				if (drgevent.Effect == DragDropEffects.Move)
				{
					draggedNode.Remove();
					TreeNode parentNode = targetNode.Parent;
					if(parentNode != null)
                    {
						parentNode.Nodes.Insert(targetNode.Index + 1, draggedNode);
						FreshDiagrams(parentNode.Text,draggedNode.Text, targetNode.Text);
                    }
					else
                    {
						Nodes.Insert(targetNode.Index + 1, draggedNode);
						FreshProjects(draggedNode.Text, targetNode.Text);
                    }
					//targetNode.Nodes.Add(draggedNode);
				}

				// If it is a copy operation, clone the dragged node 
				// and add it to the node at the drop location.
				else if (drgevent.Effect == DragDropEffects.Copy)
				{
					targetNode.Nodes.Add((TreeNode)draggedNode.Clone());
				}

				// Expand the node at the location 
				// to show the dropped node.
				//targetNode.Expand();
			}
		}

		private void FreshDiagrams(string projName,string draggedName,string targetName)
        {
			Project proj = workspace.Projects.Find(target => target.Name == projName);
	
			if(proj!=null)
			{
				List<IProjectItem> items = proj.Items.ToList();
				IProjectItem drag = items.Find(target => target.Name == draggedName);
				IProjectItem targ = items.Find(target => target.Name == targetName);
				items.Remove(drag);
				int index = items.IndexOf(targ);
				items.Insert(index + 1, drag);
				proj.Items = items;
				proj.Save();
			}
        }

		private void FreshProjects(string draggedName,string targetName)
        {
			Project drag=workspace.Projects.Find(target => target.Name == draggedName);
			Project targ = workspace.Projects.Find(target => target.Name == targetName);
			workspace.Projects.Remove(drag);
			int index = workspace.Projects.IndexOf(targ);
			workspace.Projects.Insert(index+1, drag);
        }


		// Determine whether one node is a parent 
		// or ancestor of a second node.
		private bool ContainsNode(TreeNode node1, TreeNode node2)
		{
			// Check the parent node of the second node.
			if (node2.Parent == null) return false;
			if (node2.Parent.Equals(node1)) return true;

			// If the parent node is not null or equal to the first node, 
			// call the ContainsNode method recursively using the parent of 
			// the second node.
			return ContainsNode(node1, node2.Parent);
		}

		#region Context menu event handlers

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			if (Workspace.Default.HasProject)
			{
				mnuSaveAll.Enabled = true;
				mnuCloseAll.Enabled = true;
                mnuExport.Enabled = true;
			}
			else
			{
				mnuSaveAll.Enabled = false;
				mnuCloseAll.Enabled = false;
                mnuExport.Enabled = false;
			}
		}

        /// <summary>
        /// 产生一般语义网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void mnuNewGSNProject_Click(object sender, EventArgs e)
		{
			Project project = Workspace.Default.AddEmptyProject(ProjectType.gsn);
            Workspace.Default.ActiveProject = project;
		}

        private void mnuNewCRSNProject_Click(object sender,EventArgs e)
        {
			Project project = Workspace.Default.AddEmptyProject(ProjectType.topicsn);
            Workspace.Default.ActiveProject = project;
        }

        private void mnuNewCNSNProject_Click(object sender,EventArgs e)
        {
			Project project = Workspace.Default.AddEmptyProject(ProjectType.conceptsn);
            Workspace.Default.ActiveProject = project;
        }

        private void mnuNewEQUSNProject_Click(object sender,EventArgs e)
        {
			Project project = Workspace.Default.AddEmptyProject(ProjectType.equsn);
            Workspace.Default.ActiveProject = project;

        } 

        private void mnuNewUNTSNProject_Click(object sender,EventArgs e)
        {
			Project project = Workspace.Default.AddEmptyProject(ProjectType.untsn);
            Workspace.Default.ActiveProject = project;
        } 

        private void mnuNewCONCSNProject_Click(object sender,EventArgs e)
        {
			Project project = Workspace.Default.AddEmptyProject(ProjectType.consn);
            Workspace.Default.ActiveProject = project;
        }

        private void mnuNewInstruSNProject_Click(object sender,EventArgs e)
        {
            Project project = Workspace.Default.AddEmptyProject(ProjectType.inssn);
            Workspace.Default.ActiveProject = project;
        }

        private void mnuNewEXPSNProject_Click(object sender, EventArgs e)
        {
			Project project = Workspace.Default.AddEmptyProject(ProjectType.expsn);
            Workspace.Default.ActiveProject = project;

        }
        private void mnuNewPHENOSNProject_Click(object sender, EventArgs e)
        {
            Project project = Workspace.Default.AddEmptyProject(ProjectType.phensn);
            Workspace.Default.ActiveProject = project;

        }
        private void mnuNewPROCSNProject_Click(object sender, EventArgs e)
        {
            Project project = Workspace.Default.AddEmptyProject(ProjectType.procesn);
            Workspace.Default.ActiveProject = project;

        }

        private void mnuOpen_DropDownOpening(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in mnuOpen.DropDownItems)
			{
				if (item.Tag is int)
				{
					int index = (int) item.Tag;

					if (index < Settings.Default.RecentFiles.Count)
					{
						item.Text = Settings.Default.RecentFiles[index];
						item.Visible = true;
					}
					else
					{
						item.Visible = false;
					}
				}
			}

			mnuSepOpenFile.Visible = (Settings.Default.RecentFiles.Count > 0);
		}

		private void mnuOpenFile_Click(object sender, EventArgs e)
		{
			Workspace.Default.OpenProject();
		}

		private void OpenRecentFile_Click(object sender, EventArgs e)
		{
			int index = (int) ((ToolStripItem) sender).Tag;
			if (index >= 0 && index < Settings.Default.RecentFiles.Count)
			{
				string fileName = Settings.Default.RecentFiles[index];
				Workspace.Default.OpenProject(fileName);
			}
		}

		private void mnuSaveAll_Click(object sender, EventArgs e)
		{
			Workspace.Default.SaveAllProjects();
		}

        private void mnuExport_Click(object sender,EventArgs e)
        {

        }

		private void mnuCloseAll_Click(object sender, EventArgs e)
		{
			Workspace.Default.RemoveAll();
		}

		#endregion
	}
}
