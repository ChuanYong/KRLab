namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	partial class MemberEditor
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
			this.toolVisibility = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolDelete = new System.Windows.Forms.ToolStripButton();
			this.toolMoveDown = new System.Windows.Forms.ToolStripButton();
			this.toolMoveUp = new System.Windows.Forms.ToolStripButton();
			this.toolHider = new System.Windows.Forms.ToolStripButton();
			this.sepNewMember = new System.Windows.Forms.ToolStripSeparator();
			this.toolNewMember = new System.Windows.Forms.ToolStripSplitButton();
			this.toolNewNameMember = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewStateMember = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewCPMember = new System.Windows.Forms.ToolStripMenuItem(); 
			this.txtDeclaration = new KRLab.DiagramEditor.NetworkDiagram.Editors.BorderedTextBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
			this.SuspendLayout();

			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolVisibility, 
            this.toolDelete,
            this.toolMoveDown,
            this.toolMoveUp,
            this.toolHider, 
            this.sepNewMember,
            this.toolNewMember});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
			this.toolStrip.Size = new System.Drawing.Size(330, 25);
			this.toolStrip.TabIndex = 3;
			this.toolStrip.Text = "toolStrip1";

			// 
			// toolMoveDown
			// 
			this.toolMoveDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolMoveDown.Image = global::KRLab.DiagramEditor.Properties.Resources.MoveDown;
			this.toolMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolMoveDown.Name = "toolMoveDown";
			this.toolMoveDown.Size = new System.Drawing.Size(23, 22);
			this.toolMoveDown.Text = "toolStripButton3";
			this.toolMoveDown.ToolTipText = "Move Down";
			this.toolMoveDown.Click += new System.EventHandler(this.toolMoveDown_Click);
			// 
			// toolMoveUp
			// 
			this.toolMoveUp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolMoveUp.Image = global::KRLab.DiagramEditor.Properties.Resources.MoveUp;
			this.toolMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolMoveUp.Name = "toolMoveUp";
			this.toolMoveUp.Size = new System.Drawing.Size(23, 22);
			this.toolMoveUp.Text = "toolStripButton4";
			this.toolMoveUp.ToolTipText = "Move Up";
			this.toolMoveUp.Click += new System.EventHandler(this.toolMoveUp_Click);
			// 
			// toolHider
			// 
			this.toolHider.CheckOnClick = true;
			this.toolHider.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolHider.Image = global::KRLab.DiagramEditor.Properties.Resources.NewModifier;
			this.toolHider.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolHider.Name = "toolHider";
			this.toolHider.Size = new System.Drawing.Size(23, 22);
			this.toolHider.Text = "toolStripButton5";
			this.toolHider.ToolTipText = "New";
			this.toolHider.CheckedChanged += new System.EventHandler(this.toolHider_CheckedChanged); 
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
			this.toolNewMember.Text = "toolStripSplitButton1";
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
			this.toolNewStateMember.Text = "New Method";
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
			// txtDeclaration
			// 
			this.txtDeclaration.AcceptsTab = true;
			this.txtDeclaration.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDeclaration.Location = new System.Drawing.Point(4, 28);
			this.txtDeclaration.Name = "txtDeclaration";
			this.txtDeclaration.Padding = new System.Windows.Forms.Padding(1);
			this.txtDeclaration.ReadOnly = false;
			this.txtDeclaration.SelectionStart = 0;
			this.txtDeclaration.Size = new System.Drawing.Size(322, 20);
			this.txtDeclaration.TabIndex = 4;
			this.txtDeclaration.TextChanged += new System.EventHandler(this.txtDeclaration_TextChanged);
			this.txtDeclaration.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeclaration_Validating);
			this.txtDeclaration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDeclaration_KeyDown);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// MemberEditor
			// 
			this.Controls.Add(this.txtDeclaration);
			this.Controls.Add(this.toolStrip);
			this.Name = "MemberEditor";
			this.Size = new System.Drawing.Size(330, 52);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripDropDownButton toolVisibility;
		private System.Windows.Forms.ToolStripButton toolDelete;
		private System.Windows.Forms.ToolStripButton toolMoveDown;
		private System.Windows.Forms.ToolStripButton toolMoveUp;
		private System.Windows.Forms.ToolStripButton toolHider;
		private System.Windows.Forms.ToolStripSeparator sepNewMember;
		private System.Windows.Forms.ToolStripSplitButton toolNewMember;

		private System.Windows.Forms.ToolStripMenuItem toolNewNameMember;
		private System.Windows.Forms.ToolStripMenuItem toolNewStateMember;
		private System.Windows.Forms.ToolStripMenuItem toolNewCPMember;

		private BorderedTextBox txtDeclaration;
		private System.Windows.Forms.ToolStripMenuItem toolDefault;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}
