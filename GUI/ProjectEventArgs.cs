

using System;
using KRLab.Core;

namespace KRLab.GUI
{
	public delegate void ProjectEventHandler(object sender, ProjectEventArgs e);

	public class ProjectEventArgs
	{
		Project project;

		public ProjectEventArgs(Project project)
		{
			this.project = project;
		}

		public Project Project
		{
			get { return project; }
		}
	}
}
