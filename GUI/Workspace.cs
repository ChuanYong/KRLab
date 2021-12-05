

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.Core.SNet;
using KRLab.Translations;
using KRLab.DiagramEditor.NetworkDiagram;

using Utilities;
using System.Net;

namespace KRLab.GUI
{
	public class Workspace
	{
		static Workspace _default = new Workspace();

		private List<Project> _projects = new List<Project>();
		Project activeProject = null;

		public event EventHandler ActiveProjectChanged;
		public event EventHandler ActiveProjectStateChanged;
		public event ProjectEventHandler ProjectAdded;
		public event ProjectEventHandler ProjectRemoved;

		private Workspace()
		{
		}

		public static Workspace Default
		{
			get { return _default; }
		}

		public List<Project> Projects
		{
			get { return _projects; }
            //set { _projects = value; }
		}

		public int ProjectCount
		{
			get { return _projects.Count; }
		}

		public bool HasProject
		{
			get { return (ProjectCount > 0); }
		}

		public Project ActiveProject
		{
			get
			{
				return activeProject;
			}
			set
			{
				if (value == null)
				{
					if (activeProject != null)
					{
						activeProject = null;
						OnActiveProjectChanged(EventArgs.Empty);
					}
				}
				else if (activeProject != value && _projects.Contains(value))
				{
					activeProject = value;
					OnActiveProjectChanged(EventArgs.Empty);
				}
			}
		}

		public bool HasActiveProject
		{
			get { return (activeProject != null); }
		}

		public Project AddEmptyProject(string projectType)
		{
			Project project = new Project(projectType);
            _projects.Add(project);
			project.Modified += new EventHandler(project_StateChanged);
			project.FileStateChanged += new EventHandler(project_StateChanged);
			OnProjectAdded(new ProjectEventArgs(project));
			return project;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="project"/> is null.
		/// </exception>
		public void AddProject(Project project)
		{
			if (project == null)
				throw new ArgumentNullException("project");

			if (!_projects.Contains(project))
			{
                _projects.Add(project);
				project.Modified += new EventHandler(project_StateChanged);
				project.FileStateChanged += new EventHandler(project_StateChanged);
				if (project.FilePath != null)
					Settings.Default.AddRecentFile(project.FilePath);
				OnProjectAdded(new ProjectEventArgs(project));
			}
		}

		public bool RemoveProject(Project project)
		{
			return RemoveProject(project, true);
		}

		private bool RemoveProject(Project project, bool saveConfirmation)
		{
			if (saveConfirmation && project.IsDirty)
			{
				DialogResult result = MessageBox.Show(
					Strings.AskSaveChanges, Strings.Confirmation,
					MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
				{
					if (!SaveProject(project))
						return false;
				}
				else if (result == DialogResult.Cancel)
				{
					return false;
				}
			}

			if (_projects.Remove(project))
			{
				project.CloseItems();
				project.Modified -= new EventHandler(project_StateChanged);
				project.FileStateChanged -= new EventHandler(project_StateChanged);
				OnProjectRemoved(new ProjectEventArgs(project));
				if (ActiveProject == project)
					ActiveProject = null;
				return true;
			}
			return false;
		}

		public void RemoveActiveProject()
		{
			RemoveActiveProject(true);
		}

		private void RemoveActiveProject(bool saveConfirmation)
		{
			if (HasActiveProject)
				RemoveProject(ActiveProject, saveConfirmation);
		}

		public bool RemoveAll()
		{
			return RemoveAll(true);
		}

		private bool RemoveAll(bool saveConfirmation)
		{
			if (saveConfirmation)
			{
				ICollection<Project> unsavedProjects = _projects.FindAll(
					delegate(Project project) { return project.IsDirty; }
				);

				if (unsavedProjects.Count > 0)
				{
					string message = Strings.AskSaveChanges + "\n";
					foreach (Project project in unsavedProjects)
						message += "\n" + project.Name;

					DialogResult result = MessageBox.Show(message, Strings.Confirmation,
						MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

					if (result == DialogResult.Yes)
					{
						if (!SaveAllUnsavedProjects())
							return false;
					}
					else if (result == DialogResult.Cancel)
					{
						return false;
					}
				}
			}

			while (HasProject)
			{
				int lastIndex = _projects.Count - 1;
				Project project = _projects[lastIndex];
				project.CloseItems();
                _projects.RemoveAt(lastIndex);
				OnProjectRemoved(new ProjectEventArgs(project));
			}
			ActiveProject = null;
			return true;
		}

		public Project OpenProject()
		{
			using (OpenFileDialog dialog = new OpenFileDialog())
			{ 
                string filter = string.Empty;
                foreach (var x in ProjectType.TypeToName)
                {
                    filter+=x.Value + "(*." + x.Key + ")|*." + x.Key+"|";
                }

                int idx = filter.LastIndexOf("|");
                filter=filter.Substring(0, idx);
                dialog.Filter = filter;

                if (dialog.ShowDialog() == DialogResult.OK)
					return OpenProject(dialog.FileName);
				else 
					return null;
			}
		}

		public Project OpenProject(string fileName)
		{
			try
			{
				Project project = Project.Load(fileName);
				AddProject(project);
				return project;
			}
			catch (Exception ex)
			{
				MessageBox.Show(Strings.Error + ": " + ex.Message,
					Strings.Load, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="project"/> is null.
		/// </exception>
		public bool SaveProject(Project project)
		{
			if (project == null)
				throw new ArgumentNullException("project");

			if (project.FilePath == null || project.IsReadOnly)
			{
				return SaveProjectAs(project);
			}
			else
			{
				try
				{
					project.Save();
                    IEnumerable<IProjectItem> items = project.Items;
                    foreach(var item in items)
                    {
                        Model model = (Model)item;
                        List<IEntity> entities = new List<IEntity>(model.Entities);
                        List<Relationship> relations = new List<Relationship>(model.Relationships);
                        CheckNet(project.Type,model.Name,entities, relations); 
                        CheckSectionDeptNet(project,model.Name);
                    }
					//必须放在后面！！！！！
                    CheckProjectNames(project);
					CheckSubjectProject(project);
					CheckSectionNames(project);
                    return true;
				}
                catch(NetException ne)
                {
                    MessageBox.Show(Strings.Error + ":" + ne.Error);
                    return false;
                }
				catch (Exception ex)
				{
					MessageBox.Show(Strings.Error + ": " + ex.Message,
						Strings.SaveAs, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="project"/> is null.
		/// </exception>
		public bool SaveProjectAs(Project project)
		{
			if (project == null)
				throw new ArgumentNullException("project");

			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.FileName = project.Name; 
				dialog.InitialDirectory = project.GetProjectDirectory();
                dialog.Filter = ProjectType.TypeToName[project.Type] +
                    "(*." + project.Type + ")|*." + project.Type; 

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						project.Save(dialog.FileName);
						Settings.Default.AddRecentFile(project.FilePath);
						return true;
					}
					catch (Exception ex)
					{
						MessageBox.Show(Strings.Error + ": " + ex.Message,
							Strings.SaveAs, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				return false;
			}
		}

		public void SaveActiveProjectWithoutCheck()
		{
			if (HasActiveProject)
				ActiveProject.Save();
		}

		public bool SaveActiveProject()
		{
			if (HasActiveProject)
				return SaveProject(ActiveProject);
			else
				return false;
		}

		public bool SaveActiveProjectAs()
		{
			if (HasActiveProject)
				return SaveProjectAs(ActiveProject);
			else
				return false;
		}

		public bool SaveAllProjects()
		{
			bool allSaved = true;

			foreach (Project project in _projects)
			{
				allSaved &= SaveProject(project);
			}
			return allSaved;
		}

		public bool SaveAllUnsavedProjects()
		{
			bool allSaved = true;

			foreach (Project project in _projects)
			{
				if (project.IsDirty)
					allSaved &= SaveProject(project);
			}
			return allSaved;
		}

		public void Load()
		{
			if (HasProject)
				RemoveAll();

			foreach (string projectFile in Settings.Default.OpenedProjects)
			{
				if (!string.IsNullOrEmpty(projectFile))
				{
					OpenProject(projectFile);
				}
			}
		}

		public void Save()
		{
			Settings.Default.OpenedProjects.Clear();

			foreach (Project project in _projects)
			{
				if (project.FilePath != null)
					Settings.Default.OpenedProjects.Add(project.FilePath);
			}
		}

		public bool SaveAndClose()
		{
			Save();
			return RemoveAll();
		}

        private void CheckSectionNames(Project project)
        {
            if (project.Type != ProjectType.topicsn)
                return;
            int i = 0;
            List<string> sects = DomainTopicKRModuleSNet.GetSectionts();
            foreach(var item in project.Items)
            {
                if(item.Name!="目录" && !sects.Contains(item.Name))
                {
                    throw new NetException("<"+item.Name+">小节语义网的名称错误！");
                }
                i++;
            }
            if(i<sects.Count)
            {
                throw new NetException("目录语义网中的小节没有建立相应的小节语义网，请检查！");
            }

        }

        /// <summary>
        /// 检查语义网中的依赖语义网
        /// </summary>
        /// <param name="project"></param>
        /// <param name="netName"></param> 
        private void CheckSectionDeptNet(Project project,string netName)
        {
            if (project.Type != ProjectType.topicsn || netName=="目录")
                return;

            Dictionary<string, List<IEntity>> topicDict = DomainTopicKRModuleSNet.TopicDict; 

            foreach(var dict in topicDict)
            {
                Project proj = _projects.Find(target => target.Name == dict.Key);
                if (proj == null)
                {
                    throw new NetException("<" + netName + ">中有类型为<" + dict.Key + ">的结点，必须建立与之对应的语义项目。\n" +
                        "如果已经建立这种类型的语义网项目，请打开，否则新建立这种类型的语义项目");
                }
                foreach(var entity in dict.Value)
                {
                    List<IProjectItem> items = new List<IProjectItem>(proj.Items);
                    IProjectItem item = items.Find(target => target.Name == entity.Name);
                    if (item == null)
                    {
                        throw new NetException("<" + netName + ">中有<" + dict.Key + ">类型结点：" + entity.Name + 
                            "。但在<" + dict.Key + ">项目中没有<" + entity.Name + ">语义网!");
                    }
                }

            }
        }

        private Project  GetProject(string type)
        {
            foreach(var project in Projects)
            {
                if (project.Type == type)
                    return project;
            }
            return null;
        } 

		private void CheckSubjectProject(Project proj)
        {
			if(proj.Name=="课程排序")
            {
				IEnumerable<IProjectItem> items = proj.Items;
				foreach (var item in items)
				{
					Model model = (Model)item;
					List<IEntity> entities = new List<IEntity>(model.Entities);
					List<Relationship> relations = new List<Relationship>(model.Relationships);

					IEntity startNode = null;
					IEntity endNode = null;
					foreach (var node in entities)
					{
						if (node.Name == "起始课程")
						{
							startNode = node;
						}
						else if(node.Name=="最终课程")
                        {
							endNode = node;
                        }
					}

					if (startNode == null)
					{
						throw new NetException($"<{item.Name}>没有<起始课程>结点"); 
					}
					if(endNode==null)
                    {
						throw new NetException($"<{item.Name}>没有<最终课程>节点");
                    }
				}
			}
        }

        private void CheckProjectNames(Project project)
        {
            List<IProjectItem> items = new List<IProjectItem>(project.Items);
            for(int i=0;i<items.Count;i++)
            {
                for(int j=i+1;j<items.Count;j++)
                {
                    if(items[i].Name==items[j].Name)
                    {
                        throw new NetException("<" + project.Name + ">语义项目中有相同名称的语义网：" + items[i].Name);
                    }
                }
            }
        }

        /// <summary>
        /// 检查每个图是否符合语义要求
        /// </summary>
        /// <param name="projectItem">一个图diagram对应的xml element</param>
        private void CheckNet(string projectType,string modelName, List<IEntity> entities, List<Relationship> relations)
        {
			KRModuleSNet.Check(entities, relations, (isOk, info) =>
			 {
				 if (!isOk)
					 throw new NetException("'" + modelName + "'语义网：" + info);
			 }
			);
            ///如果是一般的语义网不做检查
            if (projectType == ProjectType.untsn)
            {
                UnitKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException("'" + modelName + "'语义网：" + info);
                });
            }
            else if (projectType == ProjectType.topicsn)
            {
                DomainTopicKRModuleSNet.Check(modelName, entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException(modelName + "：" + info);
                });
            } 
            else if (projectType == ProjectType.inssn)
            {
                InstrumentKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException(modelName + "：" + info);
                });
            }
            else if (projectType == ProjectType.conceptsn)
            {
                ConceptKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException(modelName + "：" + info);
                });
            }
            else if (projectType == ProjectType.equsn)
            {
                EquationKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException(modelName + "：" + info);
                });
            }
            else if (projectType == ProjectType.expsn)
            {
                ExperimentKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {

                    if (!isOk)
                        throw new NetException(modelName + "：" + info);
                });
            } 
            else if (projectType == ProjectType.consn)
            {
                ConclusionKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException(modelName + "：" + info);

                });
            }
            else if (projectType == ProjectType.phensn)
            {
                PhenomenaKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException(modelName + "：" + info);

                });
            }
            else if (projectType == ProjectType.procesn)
            {
                ProceduralKRModuleSNet.Check(entities, relations, (isOk, info) =>
                {
                    if (!isOk)
                        throw new NetException(modelName + "：" + info);

                });
            }

        }

        private void project_StateChanged(object sender, EventArgs e)
		{
			Project project = (Project) sender;
			if (project == ActiveProject)
				OnActiveProjectStateChanged(EventArgs.Empty);
		}

		protected virtual void OnActiveProjectChanged(EventArgs e)
		{
			if (ActiveProjectChanged != null)
				ActiveProjectChanged(this, EventArgs.Empty);
		}

		protected virtual void OnActiveProjectStateChanged(EventArgs e)
		{
			if (ActiveProjectStateChanged != null)
				ActiveProjectStateChanged(this, EventArgs.Empty);
		}

		protected virtual void OnProjectAdded(ProjectEventArgs e)
		{
			if (ProjectAdded != null)
				ProjectAdded(this, e);
		}

		protected virtual void OnProjectRemoved(ProjectEventArgs e)
		{
			if (ProjectRemoved != null)
				ProjectRemoved(this, e);
		}
	}
}
