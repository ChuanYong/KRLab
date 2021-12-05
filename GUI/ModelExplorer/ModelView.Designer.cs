using System.Windows.Forms;
using System.ComponentModel;

using KRLab.Translations;

namespace KRLab.GUI.ModelExplorer
{
	partial class ModelView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

        //By setting the ContextMenuStrip property, you can 
        //provide context-sensitive operations to the user when
        //they right-click the TreeView control.
		private ContextMenuStrip contextMenu;

        //新建工程
		private ToolStripMenuItem mnuNewProject;
        private ToolStripMenuItem mnuNewSNProject;
        private ToolStripMenuItem mnuNewBNProject;
        private ToolStripMenuItem mnuNewCMProject;

        //新建语义网工程
        private ToolStripMenuItem mnuNewGSNProject;//一般语义网
        private ToolStripMenuItem mnuNewCRSNProject;//课程语义网
        private ToolStripMenuItem mnuNewCNSNProject;//概念语义网
        private ToolStripMenuItem mnuNewEquSNProject;//公式语义网 
        private ToolStripMenuItem mnuNewUNTSNProject;//单位语义网 
        private ToolStripMenuItem mnuNewCONCSNProject;//结论语义网
        private ToolStripMenuItem mnuNewInstruProject;//器材语义网
        private ToolStripMenuItem mnuNewEXPSNProject;//实验语义网
        private ToolStripMenuItem mnuNewPHENSNProject;//现象语义网
        private ToolStripMenuItem mnuNewPROCSNProject;//流程语义网
        //

        //导入工程
		private ToolStripMenuItem mnuOpen;
		private ToolStripMenuItem mnuOpenFile;
		private ToolStripSeparator mnuSepOpenFile;
		private ToolStripMenuItem mnuRecentFile1;
		private ToolStripMenuItem mnuRecentFile2;
		private ToolStripMenuItem mnuRecentFile3;
		private ToolStripMenuItem mnuRecentFile4;
		private ToolStripMenuItem mnuRecentFile5;
        //保存或关闭工程
		private ToolStripMenuItem mnuSaveAll;
		private ToolStripMenuItem mnuCloseAll;
		private ImageList imageList;

        private ToolStripMenuItem mnuExport;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			if (disposing)
			{
				normalFont.Dispose();
				boldFont.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelView));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);

			this.mnuNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewSNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewBNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewCMProject = new System.Windows.Forms.ToolStripMenuItem();

            //语义网
            this.mnuNewGSNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewCRSNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewCNSNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewEquSNProject = new System.Windows.Forms.ToolStripMenuItem(); 
            this.mnuNewUNTSNProject = new System.Windows.Forms.ToolStripMenuItem(); 
            this.mnuNewCONCSNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewInstruProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewEXPSNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewPHENSNProject = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewPROCSNProject = new System.Windows.Forms.ToolStripMenuItem();


            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepOpenFile = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRecentFile1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile3 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile4 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile5 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExport = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.imageList = new System.Windows.Forms.ImageList(this.components);

            this.lblAddProjectHint = new System.Windows.Forms.Label();

			this.mnuSepOpen = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();

			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuNewProject,
                this.mnuOpen,
                this.mnuSepOpen,
                this.mnuSaveAll,
                this.mnuExport,
                this.mnuCloseAll
            });

			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(168, 98);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
           
            // 
            // mnuNewProject
            // 
            this.mnuNewProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]{
                this.mnuNewSNProject,
                this.mnuNewBNProject,
                this.mnuNewCMProject
            });
            this.mnuNewProject.Image = global::KRLab.GUI.Properties.Resources.Project;
			this.mnuNewProject.Name = "mnuNewProject";
			this.mnuNewProject.Size = new System.Drawing.Size(167, 22);
			this.mnuNewProject.Text = "&New Project";
            //this.mnuNewProject.Click += new System.EventHandler(this.mnuNewProject_Click);

            this.mnuNewSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewSNProject.Name = "mnuNewProject";
			this.mnuNewSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewSNProject.Text = "&语义网项目";

            this.mnuNewSNProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.mnuNewGSNProject,
                this.mnuNewCRSNProject,
                this.mnuNewCNSNProject,
                this.mnuNewEquSNProject, 
                this.mnuNewUNTSNProject, 
                this.mnuNewCONCSNProject,
                this.mnuNewInstruProject,
                this.mnuNewEXPSNProject,
                this.mnuNewPHENSNProject,
                this.mnuNewPROCSNProject,
            });

            this.mnuNewGSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewGSNProject.Name = "mnuNewGSNProject";
            this.mnuNewGSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewGSNProject.Text = "&一般语义网项目";
            this.mnuNewGSNProject.Click += new System.EventHandler(this.mnuNewGSNProject_Click);

            this.mnuNewCRSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewCRSNProject.Name = "mnuNewCNSNProject";
            this.mnuNewCRSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewCRSNProject.Text = "&课程语义网项目";
            this.mnuNewCRSNProject.Click += new System.EventHandler(this.mnuNewCRSNProject_Click);

            this.mnuNewCNSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewCNSNProject.Name = "mnuNewCNSNProject";
            this.mnuNewCNSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewCNSNProject.Text = "&概念语义网项目";
            this.mnuNewCNSNProject.Click += new System.EventHandler(this.mnuNewCNSNProject_Click);

            this.mnuNewEquSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewEquSNProject.Name = "mnuNewCNSNProject";
            this.mnuNewEquSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewEquSNProject.Text = "&公式语义网项目";
            this.mnuNewEquSNProject.Click += new System.EventHandler(this.mnuNewEQUSNProject_Click); 

            this.mnuNewUNTSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewUNTSNProject.Name = "mnuNewCNSNProject";
            this.mnuNewUNTSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewUNTSNProject.Text = "&单位语义网项目";
            this.mnuNewUNTSNProject.Click += new System.EventHandler(this.mnuNewUNTSNProject_Click);

            this.mnuNewInstruProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewInstruProject.Name = "mnuNewInstruProject";
            this.mnuNewInstruProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewInstruProject.Text = "&器材语义网项目";
            this.mnuNewInstruProject.Click += new System.EventHandler(this.mnuNewInstruSNProject_Click);

            this.mnuNewEXPSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewEXPSNProject.Name = "mnuNewEXPSNProject";
            this.mnuNewEXPSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewEXPSNProject.Text = "&实验语义网项目";
            this.mnuNewEXPSNProject.Click += new System.EventHandler(this.mnuNewEXPSNProject_Click); 

            this.mnuNewCONCSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewCONCSNProject.Name = "mnuNewCONCSNProject";
            this.mnuNewCONCSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewCONCSNProject.Text = "&结论语义网项目";
            this.mnuNewCONCSNProject.Click += new System.EventHandler(this.mnuNewCONCSNProject_Click);

            this.mnuNewPHENSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewPHENSNProject.Name = "mnuNewPHENSNProject";
            this.mnuNewPHENSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewPHENSNProject.Text = "&现象语义网项目";
            this.mnuNewPHENSNProject.Click += new System.EventHandler(this.mnuNewPHENOSNProject_Click);

            this.mnuNewPROCSNProject.Image = global::KRLab.GUI.Properties.Resources.Project;
            this.mnuNewPROCSNProject.Name = "mnuNewPROCSNProject";
            this.mnuNewPROCSNProject.Size = new System.Drawing.Size(167, 22);
            this.mnuNewPROCSNProject.Text = "&流程语义网项目";
            this.mnuNewPROCSNProject.Click += new System.EventHandler(this.mnuNewPROCSNProject_Click);

            // 
            // mnuOpen
            // 
            this.mnuOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenFile,
            this.mnuSepOpenFile,
            this.mnuRecentFile1,
            this.mnuRecentFile2,
            this.mnuRecentFile3,
            this.mnuRecentFile4,
            this.mnuRecentFile5});

			this.mnuOpen.Image = global::KRLab.GUI.Properties.Resources.Open;
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.Size = new System.Drawing.Size(167, 22);
			this.mnuOpen.Text = "&Open";
			this.mnuOpen.DropDownOpening += new System.EventHandler(this.mnuOpen_DropDownOpening);
			// 
			// mnuOpenFile
			// 
			this.mnuOpenFile.Name = "mnuOpenFile";
			this.mnuOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpenFile.Size = new System.Drawing.Size(177, 22);
			this.mnuOpenFile.Text = "&New File...";
			this.mnuOpenFile.Click += new System.EventHandler(this.mnuOpenFile_Click);
			// 
			// mnuSepOpenFile
			// 
			this.mnuSepOpenFile.Name = "mnuSepOpenFile";
			this.mnuSepOpenFile.Size = new System.Drawing.Size(174, 6);
			// 
			// mnuRecentFile1
			// 
			this.mnuRecentFile1.Name = "mnuRecentFile1";
			this.mnuRecentFile1.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile1.Tag = 0;
			this.mnuRecentFile1.Text = "Recent File 1";
			this.mnuRecentFile1.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile2
			// 
			this.mnuRecentFile2.Name = "mnuRecentFile2";
			this.mnuRecentFile2.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile2.Tag = 1;
			this.mnuRecentFile2.Text = "Recent File 2";
			this.mnuRecentFile2.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile3
			// 
			this.mnuRecentFile3.Name = "mnuRecentFile3";
			this.mnuRecentFile3.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile3.Tag = 2;
			this.mnuRecentFile3.Text = "Recent File 3";
			this.mnuRecentFile3.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile4
			// 
			this.mnuRecentFile4.Name = "mnuRecentFile4";
			this.mnuRecentFile4.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile4.Tag = 3;
			this.mnuRecentFile4.Text = "Recent File 4";
			this.mnuRecentFile4.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile5
			// 
			this.mnuRecentFile5.Name = "mnuRecentFile5";
			this.mnuRecentFile5.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile5.Tag = 4;
			this.mnuRecentFile5.Text = "Recent File 5";
			this.mnuRecentFile5.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuSaveAll
			// 
			this.mnuSaveAll.Image = global::KRLab.GUI.Properties.Resources.SaveAll;
			this.mnuSaveAll.Name = "mnuSaveAll";
			this.mnuSaveAll.Size = new System.Drawing.Size(167, 22);
			this.mnuSaveAll.Text = "Save A&ll Projects";
			this.mnuSaveAll.Click += new System.EventHandler(this.mnuSaveAll_Click);

            //mnuExport
            this.mnuExport.Image = global::KRLab.GUI.Properties.Resources.SaveAll;
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.Size = new System.Drawing.Size(167, 22);
            this.mnuExport.Text = "&Export Projects";
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);

            // 
            // mnuCloseAll
            // 
            this.mnuCloseAll.Name = "mnuCloseAll";
			this.mnuCloseAll.Size = new System.Drawing.Size(167, 22);
			this.mnuCloseAll.Text = "Close All Projects";
			this.mnuCloseAll.Click += new System.EventHandler(this.mnuCloseAll_Click);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "project");
			this.imageList.Images.SetKeyName(1, "diagram");

            // 
            // lblAddCMProject
            // 
            this.lblAddProjectHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddProjectHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblAddProjectHint.Location = new System.Drawing.Point(0, 0);
            this.lblAddProjectHint.Name = "lblAddProjectHint";
            this.lblAddProjectHint.Size = new System.Drawing.Size(100, 23);
            this.lblAddProjectHint.TabIndex = 0;
            this.lblAddProjectHint.Text = Strings.AddProjectHint;
            this.lblAddProjectHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAddProjectHint.Visible = false; 

            
            // 
            // mnuSepOpen
            // 
            this.mnuSepOpen.Name = "mnuSepOpen";
            this.mnuSepOpen.Size = new System.Drawing.Size(164, 6);

			// 
			// ModelView
			// 
			this.ContextMenuStrip = this.contextMenu;
			this.ImageIndex = 0;
			this.ImageList = this.imageList;
			this.LabelEdit = true;
			this.LineColor = System.Drawing.Color.Black;
			this.SelectedImageIndex = 0;
			this.ShowRootLines = false;
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private Label lblAddProjectHint;
		private ToolStripSeparator mnuSepOpen;
	}
}
