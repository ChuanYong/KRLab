namespace KRLab.DiagramEditor.NetworkDiagram.Dialogs
{
    partial class SNConnectionDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtStartRole = new System.Windows.Forms.TextBox();
			this.txtEndMultiplicity = new System.Windows.Forms.TextBox();
			this.txtEndRole = new System.Windows.Forms.TextBox();
			this.txtStartMultiplicity = new System.Windows.Forms.TextBox();
			this.picArrow = new System.Windows.Forms.PictureBox();
			this.labelName = new System.Windows.Forms.TextBox(); 
			((System.ComponentModel.ISupportInitialize) (this.picArrow)).BeginInit();
			this.SuspendLayout();
            this.treeRelationType = new System.Windows.Forms.TreeView();

            #region 标签编辑框



            // 
            // txtName
            // 
            this.labelName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelName.Location = new System.Drawing.Point(150, 12);
            this.labelName.Name = "relationName";
            this.labelName.Size = new System.Drawing.Size(100, 20);
            this.labelName.TabIndex = 0;
            this.labelName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            #endregion

            #region 箭头指向
            // 
            // picArrow
            // 
            this.picArrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picArrow.Location = new System.Drawing.Point(20, 39);
            this.picArrow.Name = "picArrow";
            this.picArrow.Size = new System.Drawing.Size(360, 15);
            this.picArrow.TabIndex = 4;
            this.picArrow.TabStop = false;
            this.picArrow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picArrow_MouseDown);
            this.picArrow.Paint += new System.Windows.Forms.PaintEventHandler(this.picArrow_Paint);
            #endregion

            #region 连接两端的端点编辑框

            // 
            // txtStartMultiplicity
            //
            this.txtStartRole.Location = new System.Drawing.Point(20, 12);
            this.txtStartRole.Name = "txtStartRole";
            this.txtStartRole.Size = new System.Drawing.Size(125, 20);
            this.txtStartRole.TabIndex = 6;

            // 
            // txtStartRole
            // 
            this.txtStartMultiplicity.Location = new System.Drawing.Point(20, 60);
            this.txtStartMultiplicity.Name = "txtStartMultiplicity";
            this.txtStartMultiplicity.Size = new System.Drawing.Size(125, 20);
            this.txtStartMultiplicity.TabIndex = 2;
            // 
            // txtEndMultiplicity
            // 
            this.txtEndRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEndRole.Location = new System.Drawing.Point(255, 12);
            this.txtEndRole.Name = "txtEndRole";
            this.txtEndRole.Size = new System.Drawing.Size(125, 20);
            this.txtEndRole.TabIndex = 1;
            this.txtEndRole.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtEndRole
            // 
            this.txtEndMultiplicity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEndMultiplicity.Location = new System.Drawing.Point(255, 60);
            this.txtEndMultiplicity.Name = "txtEndMultiplicity";
            this.txtEndMultiplicity.Size = new System.Drawing.Size(125, 20);
            this.txtEndMultiplicity.TabIndex = 3;
            this.txtEndMultiplicity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            #endregion

            #region 连接类型选择树
            //
            //this.treeRelationType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.treeRelationType.Location = new System.Drawing.Point(20, 105);
            this.treeRelationType.Size = new System.Drawing.Size(360, 380); 
            this.treeRelationType.Nodes.Add(new RelationTypeNode(treeRelationType,"连接类型")); 
            this.treeRelationType.Nodes[0].ExpandAll();
			this.treeRelationType.ShowNodeToolTips = true;

            this.treeRelationType.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(ChangeSNRelationType);
			this.treeRelationType.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(NodeMouseHover);


            #endregion

            #region 确认和取消按钮
            // 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(275, 550);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(50, 550);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            #endregion


            // 
			// AssociationDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(400,600); 
            this.Controls.Add(this.labelName);
			this.Controls.Add(this.txtStartMultiplicity);
			this.Controls.Add(this.txtEndRole);
			this.Controls.Add(this.picArrow);
			this.Controls.Add(this.txtEndMultiplicity);
			this.Controls.Add(this.txtStartRole);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.treeRelationType);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AssociationDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Association";
			((System.ComponentModel.ISupportInitialize) (this.picArrow)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox txtStartRole;
		private System.Windows.Forms.TextBox txtStartMultiplicity;
		private System.Windows.Forms.PictureBox picArrow;
		private System.Windows.Forms.TextBox txtEndRole;
		private System.Windows.Forms.TextBox txtEndMultiplicity;
		private System.Windows.Forms.TextBox labelName;
         
        private System.Windows.Forms.TreeView treeRelationType;
	}
}