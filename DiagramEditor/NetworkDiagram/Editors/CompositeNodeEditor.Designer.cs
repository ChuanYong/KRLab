namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	partial class CompositeNodeEditor:TypeEditor
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolStrip = new System.Windows.Forms.ToolStrip(); 
			this.toolSortByName = new System.Windows.Forms.ToolStripButton(); 
			this.toolSortByKind = new System.Windows.Forms.ToolStripButton();
			this.sepNewMember = new System.Windows.Forms.ToolStripSeparator(); 
			this.toolNewMember = new System.Windows.Forms.ToolStripSplitButton();
			this.toolNewNameMember = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewStateMember = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewCPMember = new System.Windows.Forms.ToolStripMenuItem(); 
			this.txtName = new KRLab.DiagramEditor.NetworkDiagram.Editors.BorderedTextBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 
            this.toolSortByName, 
            this.toolSortByKind,
            this.sepNewMember, 
            this.toolNewMember});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
			this.toolStrip.Size = new System.Drawing.Size(330, 25);
			this.toolStrip.TabIndex = 4;
			this.toolStrip.Text = "toolStrip1";    
			// 
			// toolSortByName
			// 
			this.toolSortByName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolSortByName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolSortByName.Image = global::KRLab.DiagramEditor.Properties.Resources.SortByName;
			this.toolSortByName.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolSortByName.Name = "toolSortByName";
			this.toolSortByName.Size = new System.Drawing.Size(23, 22);
			this.toolSortByName.Text = "Sort by Name";
			this.toolSortByName.Click += new System.EventHandler(this.toolSortByName_Click);  
			// 
			// sepNewMember
			// 
			this.sepNewMember.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.sepNewMember.Name = "sepNewMember";
			this.sepNewMember.Size = new System.Drawing.Size(6, 25);  
			// 
			// toolNewMember
			// 
			this.toolNewMember.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolNewMember.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewMember.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNewNameMember,
            this.toolNewStateMember,
            this.toolNewCPMember, });
			this.toolNewMember.Image = global::KRLab.DiagramEditor.Properties.Resources.PublicMethod;
			this.toolNewMember.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewMember.Name = "toolNewMember";
			this.toolNewMember.Size = new System.Drawing.Size(32, 22);
			this.toolNewMember.Text = "New Member";
			this.toolNewMember.ButtonClick += new System.EventHandler(this.toolNewMember_ButtonClick);
			// 
			// toolNewNameMember
			// 
			this.toolNewNameMember.Image = global::KRLab.DiagramEditor.Properties.Resources.Field;
			this.toolNewNameMember.Name = "toolNewNameMember";
			this.toolNewNameMember.Size = new System.Drawing.Size(166, 22);
			this.toolNewNameMember.Text = "New Field";
			this.toolNewNameMember.Click += new System.EventHandler(this.toolNewNameMember_Click);
			// 
			// toolNewStateMember
			// 
			this.toolNewStateMember.Image = global::KRLab.DiagramEditor.Properties.Resources.Method;
			this.toolNewStateMember.Name = "toolNewStateMember";
			this.toolNewStateMember.Size = new System.Drawing.Size(166, 22);
			this.toolNewStateMember.Text = "New State";
			this.toolNewStateMember.Click += new System.EventHandler(this.toolNewStateMember_Click);
			// 
			// toolNewCPMember
			// 
			this.toolNewCPMember.Image = global::KRLab.DiagramEditor.Properties.Resources.Property;
			this.toolNewCPMember.Name = "toolNewCPMember";
			this.toolNewCPMember.Size = new System.Drawing.Size(166, 22);
			this.toolNewCPMember.Text = "New Property";
			this.toolNewCPMember.Click += new System.EventHandler(this.toolNewCPMember_Click); 
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(4, 28);
			this.txtName.Name = "txtName";
			this.txtName.Padding = new System.Windows.Forms.Padding(1);
			this.txtName.ReadOnly = false;
			this.txtName.SelectionStart = 0;
			this.txtName.Size = new System.Drawing.Size(322, 20);
			this.txtName.TabIndex = 5;
			this.txtName.AcceptsTab = true;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
			this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// CompositeTypeEditor
			// 
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.toolStrip);
			this.Name = "CompositeTypeEditor";
			this.Size = new System.Drawing.Size(330, 52);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip; 
		private BorderedTextBox txtName; 
		private System.Windows.Forms.ToolStripSplitButton toolNewMember;
		private System.Windows.Forms.ToolStripMenuItem toolNewNameMember;
		private System.Windows.Forms.ToolStripMenuItem toolNewStateMember;
		private System.Windows.Forms.ToolStripMenuItem toolNewCPMember; 
		private System.Windows.Forms.ToolStripButton toolSortByName;
		private System.Windows.Forms.ToolStripSeparator sepNewMember; 
		private System.Windows.Forms.ToolStripButton toolSortByKind;  
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}
