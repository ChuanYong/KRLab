 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram;
using KRLab.GUI.Properties;
using KRLab.Translations;
using KRLab.DiagramEditor; 

namespace KRLab.GUI.ModelExplorer
{
	public sealed class ProjectNode : ModelNode
	{
		Project project;
		static ContextMenuStrip contextMenu = new ContextMenuStrip();

        static ToolStripMenuItem newSBDiagramMenuItem = new ToolStripMenuItem(Strings.MenuSNDiagram, null, newSNDiagram_Click);
        static ToolStripMenuItem newBNDiagramMenuItem=new ToolStripMenuItem(Strings.MenuBNDiagram, null, newBNDiagram_Click);
        static ToolStripMenuItem newCMDiagramMenuItem = new ToolStripMenuItem(Strings.MenuCMDiagram,null,newCMDiagram_Click);

        static ProjectNode()
		{
            
			contextMenu.Items.AddRange(new ToolStripItem[] {
				new ToolStripMenuItem(Strings.MenuAddNew, Resources.NewDocument,
                    newSBDiagramMenuItem,
                    newBNDiagramMenuItem,
                    newCMDiagramMenuItem
                ),
				new ToolStripSeparator(),
				new ToolStripMenuItem(Strings.MenuSave, Resources.Save, save_Click),
				new ToolStripMenuItem(Strings.MenuSaveAs, null, saveAs_Click),
				new ToolStripMenuItem(Strings.MenuRename, null, rename_Click, Keys.F2),
				new ToolStripSeparator(),
				new ToolStripMenuItem(Strings.MenuCloseProject, null, close_Click)
			});
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="project"/> is null.
		/// </exception>
		public ProjectNode(Project project)
		{
			if (project == null)
				throw new ArgumentNullException("project");

			this.project = project;
			this.Text = project.Name;
			this.ImageKey = "project";
			this.SelectedImageKey = "project";

			AddProjectItemNodes(project);
			project.Renamed += new EventHandler(project_Renamed);
			project.ItemAdded += new ProjectItemEventHandler(project_ItemAdded);
			project.ItemRemoved += new ProjectItemEventHandler(project_ItemRemoved);

            if (project.Type != ProjectType.gbn)
                newBNDiagramMenuItem.Enabled = false;
            if (project.Type != ProjectType.gcm)
                newCMDiagramMenuItem.Enabled = false;
            if (project.Type == ProjectType.gbn || project.Type == ProjectType.gcm)
                newSBDiagramMenuItem.Enabled = false;
        }

		public Project Project
		{
			get { return project; }
		}

		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				contextMenu.Tag = this;
				return contextMenu;
			}
			set
			{
				base.ContextMenuStrip = value;
			}
		}

		private void AddProjectItemNodes(Project project)
		{
			if (project.IsEmpty)
			{
                ModelNode node = new EmptyProjectNode(project);
                Nodes.Add(node);
                if (TreeView != null)
                    node.AfterInitialized();
            }
			else
			{
				foreach (IProjectItem projectItem in project.Items)
				{
					AddProjectItemNode(projectItem);
				}
			}
		}

		private void AddProjectItemNode(IProjectItem projectItem)
		{
			ModelNode node = null;

			if (projectItem is Diagram)
			{
				Diagram diagram = (Diagram) projectItem;
				node = new DiagramNode(diagram);
				if (TreeView != null)
					ModelView.OnDocumentOpening(new DocumentEventArgs(diagram));
			}
			// More kind of items might be possible later...			
			if (node != null)
			{
				Nodes.Add(node);
				if (TreeView != null)
				{
					node.AfterInitialized();
					TreeView.SelectedNode = node;
				}
				if (projectItem.IsUntitled)
				{
					node.EditLabel();
				}
			}
		}

		private void RemoveProjectItemNode(IProjectItem projectItem)
		{
			foreach (ProjectItemNode node in Nodes)
			{
				if (node.ProjectItem == projectItem)
				{
					node.Delete();
					return;
				}
			}
		}

		public override void LabelModified(NodeLabelEditEventArgs e)
		{
			project.Name = e.Label;

			if (project.Name != e.Label)
				e.CancelEdit = true;
		}

		private void project_Renamed(object sender, EventArgs e)
		{
			Text = project.Name;
		}

		private void project_ItemAdded(object sender, ProjectItemEventArgs e)
		{
			AddProjectItemNode(e.ProjectItem);
		}

		private void project_ItemRemoved(object sender, ProjectItemEventArgs e)
		{
			RemoveProjectItemNode(e.ProjectItem);
			if (project.IsEmpty)
			{
				ModelNode node = new EmptyProjectNode(project);
				Nodes.Add(node);
				if (TreeView != null)
					node.AfterInitialized();
			}
		}

		public override void BeforeDelete()
		{
			project.Renamed -= new EventHandler(project_Renamed);
			project.ItemAdded -= new ProjectItemEventHandler(project_ItemAdded);
			project.ItemRemoved -= new ProjectItemEventHandler(project_ItemRemoved);
			base.BeforeDelete();
		}

        private static void newSNDiagram_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem) sender;
			Project project = ((ProjectNode) menuItem.OwnerItem.Owner.Tag).Project;
             
			Diagram diagram = new Diagram(SemanticNetTemplate.Instance);
			project.Add(diagram); 
		}

		private static void newBNDiagram_Click(object sender, EventArgs e)
		{
            ToolStripItem menuItem = (ToolStripItem)sender;
            Project project = ((ProjectNode)menuItem.OwnerItem.Owner.Tag).Project;
             
            Diagram diagram = new Diagram(BayesianNetTemplate.Instance); 
            project.Add(diagram);
		}

        private static void newCMDiagram_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = (ToolStripItem)sender;
            Project project = ((ProjectNode)menuItem.OwnerItem.Owner.Tag).Project;
             
            Diagram diagram = new Diagram(ConceptMapTemplate.Instance); 
            project.Add(diagram);
        }

		private static void rename_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem) sender;
			ProjectNode node = (ProjectNode) menuItem.Owner.Tag;

			node.EditLabel();
		}

		private static void save_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem) sender;
			Project project = ((ProjectNode) menuItem.Owner.Tag).Project;

			Workspace.Default.SaveProject(project);
		}

		private static void saveAs_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem) sender;
			Project project = ((ProjectNode) menuItem.Owner.Tag).Project;

			Workspace.Default.SaveProjectAs(project);
		}

		private static void close_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem) sender;
			Project project = ((ProjectNode) menuItem.Owner.Tag).Project;

			Workspace.Default.RemoveProject(project);
		}
	}
}
