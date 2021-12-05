namespace KRLab.DiagramEditor.NetworkDiagram
{
	partial class DiagramDynamicMenu
	{
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.mnuDiagram = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFormat = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAlign = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuMakeSameSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSameWidth = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSameHeight = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSameSize = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAutoWidth = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoHeight = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddNewElement = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.elementsToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolSepEntities = new System.Windows.Forms.ToolStripSeparator();
			this.toolSepRelationships = new System.Windows.Forms.ToolStripSeparator();
			this.toolSepNodeUpDown = new System.Windows.Forms.ToolStripSeparator();

            ///菜单下拉单中新建结点的按钮
            this.mnuNewSNNode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewCMNode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewBNNode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewComment = new System.Windows.Forms.ToolStripMenuItem();

            this.toolNewSNNode = new System.Windows.Forms.ToolStripButton();
            this.toolNewCMNode = new System.Windows.Forms.ToolStripButton();
            this.toolNewBNNode = new System.Windows.Forms.ToolStripButton();

            //菜单下拉单中网络结点之间的关系按钮
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuNewSNRelationship = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewCommentRelationship = new System.Windows.Forms.ToolStripMenuItem();
            this.toolNewSNRelationship = new System.Windows.Forms.ToolStripButton();
            this.toolNewCommentRelationship = new System.Windows.Forms.ToolStripButton();

			this.mnuMembersFormat = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowType = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowParameters = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowParameterNames = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowInitialValue = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGenerateCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAsImage = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAlignTop = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAlignLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAlignBottom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAlignRight = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAlignHorizontal = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAlignVertical = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExpandAll = new System.Windows.Forms.ToolStripMenuItem(); 

			this.toolDelete = new System.Windows.Forms.ToolStripButton();
			this.mnuAutoSize = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip.SuspendLayout();
			this.elementsToolStrip.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDiagram,
            this.mnuFormat});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(530, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// mnuDiagram
			// 
			this.mnuDiagram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddNewElement,
            this.mnuMembersFormat,
            this.toolStripSeparator1,
            this.mnuGenerateCode,
            this.mnuSaveAsImage});
			this.mnuDiagram.Name = "mnuDiagram";
			this.mnuDiagram.Size = new System.Drawing.Size(58, 20);
			this.mnuDiagram.Text = "&Diagram";
			this.mnuDiagram.DropDownOpening += new System.EventHandler(this.mnuDiagram_DropDownOpening);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
			// 
			// mnuFormat
			// 
			this.mnuFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAlign,
            this.mnuMakeSameSize,
            this.toolStripSeparator4,
            this.mnuAutoSize,
            this.mnuAutoWidth,
            this.mnuAutoHeight,
            this.mnuAutoLayout,
            this.toolStripSeparator5,
            this.mnuCollapseAll,
            this.mnuExpandAll});
			this.mnuFormat.Name = "mnuFormat";
			this.mnuFormat.Size = new System.Drawing.Size(53, 20);
			this.mnuFormat.Text = "F&ormat";
			this.mnuFormat.DropDownOpening += new System.EventHandler(this.mnuFormat_DropDownOpening);
			// 
			// mnuAlign
			// 
			this.mnuAlign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAlignTop,
            this.mnuAlignLeft,
            this.mnuAlignBottom,
            this.mnuAlignRight,
            this.toolStripSeparator3,
            this.mnuAlignHorizontal,
            this.mnuAlignVertical});
			this.mnuAlign.Name = "mnuAlign";
			this.mnuAlign.Size = new System.Drawing.Size(161, 22);
			this.mnuAlign.Text = "&Align";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(192, 6);
			// 
			// mnuMakeSameSize
			// 
			this.mnuMakeSameSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSameWidth,
            this.mnuSameHeight,
            this.mnuSameSize});
			this.mnuMakeSameSize.Name = "mnuMakeSameSize";
			this.mnuMakeSameSize.Size = new System.Drawing.Size(161, 22);
			this.mnuMakeSameSize.Text = "&Make Same Size";
			// 
			// mnuSameWidth
			// 
			this.mnuSameWidth.Name = "mnuSameWidth";
			this.mnuSameWidth.Size = new System.Drawing.Size(197, 22);
			this.mnuSameWidth.Text = "Same &Width";
			this.mnuSameWidth.Click += new System.EventHandler(this.mnuSameWidth_Click);
			// 
			// mnuSameHeight
			// 
			this.mnuSameHeight.Name = "mnuSameHeight";
			this.mnuSameHeight.Size = new System.Drawing.Size(197, 22);
			this.mnuSameHeight.Text = "Same &Height";
			this.mnuSameHeight.Click += new System.EventHandler(this.mnuSameHeight_Click);
			// 
			// mnuSameSize
			// 
			this.mnuSameSize.Name = "mnuSameSize";
			this.mnuSameSize.Size = new System.Drawing.Size(197, 22);
			this.mnuSameSize.Text = "&Same Width and Height";
			this.mnuSameSize.Click += new System.EventHandler(this.mnuSameSize_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(158, 6);
			// 
			// mnuAutoWidth
			// 
			this.mnuAutoWidth.Name = "mnuAutoWidth";
			this.mnuAutoWidth.Size = new System.Drawing.Size(161, 22);
			this.mnuAutoWidth.Text = "Auto &Width";
			this.mnuAutoWidth.Click += new System.EventHandler(this.mnuAutoWidth_Click);
			// 
			// mnuAutoHeight
			// 
			this.mnuAutoHeight.Name = "mnuAutoHeight";
			this.mnuAutoHeight.Size = new System.Drawing.Size(161, 22);
			this.mnuAutoHeight.Text = "Auto &Height";
			this.mnuAutoHeight.Click += new System.EventHandler(this.mnuAutoHeight_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(158, 6);
			// 
			// elementsToolStrip
			// 
			this.elementsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.elementsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.elementsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
            this.toolNewSNNode,
            this.toolNewCMNode,
            this.toolNewBNNode,
            this.toolSepEntities,
            this.toolNewSNRelationship,
            this.toolNewCommentRelationship, 
            this.toolSepRelationships,
            this.toolDelete,
			this.toolSepNodeUpDown});

			this.elementsToolStrip.Location = new System.Drawing.Point(0, 0);
			this.elementsToolStrip.Name = "elementsToolStrip";
			this.elementsToolStrip.Size = new System.Drawing.Size(369, 25);
			this.elementsToolStrip.TabIndex = 8;
			// 
			// toolSepEntities
			// 
			this.toolSepEntities.Name = "toolSepEntities";
			this.toolSepEntities.Size = new System.Drawing.Size(6, 25);
			// 
			// toolSepRelationships
			// 
			this.toolSepRelationships.Name = "toolSepRelationships";
			this.toolSepRelationships.Size = new System.Drawing.Size(6, 25);

			this.toolSepNodeUpDown.Name = "toolSepNodeUpDown";
			this.toolSepNodeUpDown.Size = new System.Drawing.Size(6, 25);
			// 
			// mnuAddNewElement
			// 
			this.mnuAddNewElement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewSNNode,
            this.mnuNewCMNode,
            this.mnuNewBNNode,
            this.mnuNewComment, 
            this.toolStripSeparator2, 
            this.mnuNewSNRelationship,
            this.mnuNewCommentRelationship, 
            });

			this.mnuAddNewElement.Image = global::KRLab.DiagramEditor.Properties.Resources.NewEntity;
			this.mnuAddNewElement.Name = "mnuAddNewElement";
			this.mnuAddNewElement.Size = new System.Drawing.Size(170, 22);
			this.mnuAddNewElement.Text = "&Add New";

			// 
			// mnuNewSNNode
			// 
            this.mnuNewSNNode.Image = global::KRLab.DiagramEditor.Properties.Resources.SNNode;
            this.mnuNewSNNode.Name = "mnuNewSNNode";
            this.mnuNewSNNode.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.mnuNewSNNode.Size = new System.Drawing.Size(191, 22);
            this.mnuNewSNNode.Text = "&SNNode";
            this.mnuNewSNNode.Click += new System.EventHandler(this.mnuNewNode_Click);

            // 
            // mnuNewCMNode
            // 
            this.mnuNewCMNode.Image = global::KRLab.DiagramEditor.Properties.Resources.CMNode;
            this.mnuNewCMNode.Name = "mnuNewCMNode";
            this.mnuNewCMNode.Size = new System.Drawing.Size(191, 22);
            this.mnuNewCMNode.Text = "&CMNode";
            this.mnuNewCMNode.Click += new System.EventHandler(this.mnuNewNode_Click);

            // 
            // mnuNewSNNode
            // 
            this.mnuNewBNNode.Image = global::KRLab.DiagramEditor.Properties.Resources.BNNode;
            this.mnuNewBNNode.Name = "mnuNewBNNode";
            this.mnuNewBNNode.Size = new System.Drawing.Size(191, 22);
            this.mnuNewBNNode.Text = "&BNNode";
            this.mnuNewBNNode.Click += new System.EventHandler(this.mnuNewNode_Click);    

			// 
			// mnuNewComment
			// 
			this.mnuNewComment.Image = global::KRLab.DiagramEditor.Properties.Resources.Comment;
			this.mnuNewComment.Name = "mnuNewComment";
			this.mnuNewComment.Size = new System.Drawing.Size(191, 22);
			this.mnuNewComment.Text = "Commen&t";
			this.mnuNewComment.Click += new System.EventHandler(this.mnuNewComment_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(188, 6);

            // 
            // mnuNewSNISARelationship
            // 
            this.mnuNewSNRelationship.Image = global::KRLab.DiagramEditor.Properties.Resources.SNDefaultRelationship;
            this.mnuNewSNRelationship.Name = "mnuNewSNRelationship";
            this.mnuNewSNRelationship.ShortcutKeys= ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.mnuNewSNRelationship.Size = new System.Drawing.Size(191, 22);
            this.mnuNewSNRelationship.Text = "SNIS&A";
            this.mnuNewSNRelationship.Click += new System.EventHandler(this.mnuNewSNRelationship_Click);
  
            // 
            // mnuNewCommentRelationship
            // 
            this.mnuNewCommentRelationship.Image = global::KRLab.DiagramEditor.Properties.Resources.CommentRel;
            this.mnuNewCommentRelationship.Name = "mnuNewCommentRelationship";
            this.mnuNewCommentRelationship.Size = new System.Drawing.Size(191, 22);
            this.mnuNewCommentRelationship.Text = "Co&mment Relationship";
            this.mnuNewCommentRelationship.Click += new System.EventHandler(this.mnuNewCommentRelationship_Click); 

			// 
			// mnuShowType
			// 
			this.mnuShowType.CheckOnClick = true;
			this.mnuShowType.Name = "mnuShowType";
			this.mnuShowType.Size = new System.Drawing.Size(170, 22);
			this.mnuShowType.Text = "&Type";
			this.mnuShowType.Click += new System.EventHandler(this.mnuShowType_Click);
			// 
			// mnuShowParameters
			// 
			this.mnuShowParameters.CheckOnClick = true;
			this.mnuShowParameters.Name = "mnuShowParameters";
			this.mnuShowParameters.Size = new System.Drawing.Size(170, 22);
			this.mnuShowParameters.Text = "&Parameters";
			this.mnuShowParameters.Click += new System.EventHandler(this.mnuShowParameters_Click);
			// 
			// mnuShowParameterNames
			// 
			this.mnuShowParameterNames.CheckOnClick = true;
			this.mnuShowParameterNames.Name = "mnuShowParameterNames";
			this.mnuShowParameterNames.Size = new System.Drawing.Size(170, 22);
			this.mnuShowParameterNames.Text = "Parameter &Names";
			this.mnuShowParameterNames.Click += new System.EventHandler(this.mnuShowParameterNames_Click);
			// 
			// mnuShowInitialValue
			// 
			this.mnuShowInitialValue.CheckOnClick = true;
			this.mnuShowInitialValue.Name = "mnuShowInitialValue";
			this.mnuShowInitialValue.Size = new System.Drawing.Size(170, 22);
			this.mnuShowInitialValue.Text = "&Initial Value";
			this.mnuShowInitialValue.Click += new System.EventHandler(this.mnuShowInitialValue_Click);
			// 
			// mnuGenerateCode
			// 
			this.mnuGenerateCode.Image = global::KRLab.DiagramEditor.Properties.Resources.CodeGenerator;
			this.mnuGenerateCode.Name = "mnuGenerateCode";
			this.mnuGenerateCode.Size = new System.Drawing.Size(170, 22);
			this.mnuGenerateCode.Text = "&Generate Code...";
			this.mnuGenerateCode.Click += new System.EventHandler(this.mnuGenerateCode_Click);
			// 
			// mnuSaveAsImage
			// 
			this.mnuSaveAsImage.Image = global::KRLab.DiagramEditor.Properties.Resources.Image;
			this.mnuSaveAsImage.Name = "mnuSaveAsImage";
			this.mnuSaveAsImage.Size = new System.Drawing.Size(170, 22);
			this.mnuSaveAsImage.Text = "&Save As Image...";
			this.mnuSaveAsImage.Click += new System.EventHandler(this.mnuSaveAsImage_Click);
			// 
			// mnuAlignTop
			// 
			this.mnuAlignTop.Image = global::KRLab.DiagramEditor.Properties.Resources.AlignTop;
			this.mnuAlignTop.Name = "mnuAlignTop";
			this.mnuAlignTop.Size = new System.Drawing.Size(195, 22);
			this.mnuAlignTop.Text = "Align &Top";
			this.mnuAlignTop.Click += new System.EventHandler(this.mnuAlignTop_Click);
			// 
			// mnuAlignLeft
			// 
			this.mnuAlignLeft.Image = global::KRLab.DiagramEditor.Properties.Resources.AlignLeft;
			this.mnuAlignLeft.Name = "mnuAlignLeft";
			this.mnuAlignLeft.Size = new System.Drawing.Size(195, 22);
			this.mnuAlignLeft.Text = "Align &Left";
			this.mnuAlignLeft.Click += new System.EventHandler(this.mnuAlignLeft_Click);
			// 
			// mnuAlignBottom
			// 
			this.mnuAlignBottom.Image = global::KRLab.DiagramEditor.Properties.Resources.AlignBottom;
			this.mnuAlignBottom.Name = "mnuAlignBottom";
			this.mnuAlignBottom.Size = new System.Drawing.Size(195, 22);
			this.mnuAlignBottom.Text = "Align &Bottom";
			this.mnuAlignBottom.Click += new System.EventHandler(this.mnuAlignBottom_Click);
			// 
			// mnuAlignRight
			// 
			this.mnuAlignRight.Image = global::KRLab.DiagramEditor.Properties.Resources.AlignRight;
			this.mnuAlignRight.Name = "mnuAlignRight";
			this.mnuAlignRight.Size = new System.Drawing.Size(195, 22);
			this.mnuAlignRight.Text = "Align &Right";
			this.mnuAlignRight.Click += new System.EventHandler(this.mnuAlignRight_Click);
			// 
			// mnuAlignHorizontal
			// 
			this.mnuAlignHorizontal.Image = global::KRLab.DiagramEditor.Properties.Resources.AlignHorizontal;
			this.mnuAlignHorizontal.Name = "mnuAlignHorizontal";
			this.mnuAlignHorizontal.Size = new System.Drawing.Size(195, 22);
			this.mnuAlignHorizontal.Text = "Align &Horizontal Center";
			this.mnuAlignHorizontal.Click += new System.EventHandler(this.mnuAlignHorizontal_Click);
			// 
			// mnuAlignVertical
			// 
			this.mnuAlignVertical.Image = global::KRLab.DiagramEditor.Properties.Resources.AlignVertical;
			this.mnuAlignVertical.Name = "mnuAlignVertical";
			this.mnuAlignVertical.Size = new System.Drawing.Size(195, 22);
			this.mnuAlignVertical.Text = "Align &Vertical Center";
			this.mnuAlignVertical.Click += new System.EventHandler(this.mnuAlignVertical_Click);
			// 
			// mnuAutoLayout
			// 
			this.mnuAutoLayout.Image = global::KRLab.DiagramEditor.Properties.Resources.AutoLayout;
			this.mnuAutoLayout.Name = "mnuAutoLayout";
			this.mnuAutoLayout.Size = new System.Drawing.Size(161, 22);
			this.mnuAutoLayout.Text = "Auto &Layout";
			this.mnuAutoLayout.Visible = false;
			// 
			// mnuCollapseAll
			// 
			this.mnuCollapseAll.Image = global::KRLab.DiagramEditor.Properties.Resources.CollapseAll;
			this.mnuCollapseAll.Name = "mnuCollapseAll";
			this.mnuCollapseAll.Size = new System.Drawing.Size(161, 22);
			this.mnuCollapseAll.Text = "&Collapse All";
			this.mnuCollapseAll.Click += new System.EventHandler(this.mnuCollapseAll_Click);
			// 
			// mnuExpandAll
			// 
			this.mnuExpandAll.Image = global::KRLab.DiagramEditor.Properties.Resources.ExpandAll;
			this.mnuExpandAll.Name = "mnuExpandAll";
			this.mnuExpandAll.Size = new System.Drawing.Size(161, 22);
			this.mnuExpandAll.Text = "&Expand All";
			this.mnuExpandAll.Click += new System.EventHandler(this.mnuExpandAll_Click);

			// 
			// toolNewSNNode
			// 
            this.toolNewSNNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNewSNNode.Image = global::KRLab.DiagramEditor.Properties.Resources.SNNode;
            this.toolNewSNNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewSNNode.Name = "toolNewSNNode";
            this.toolNewSNNode.Size = new System.Drawing.Size(23, 22);
            this.toolNewSNNode.Click += new System.EventHandler(this.mnuNewNode_Click);

            // 
            // toolNewCMNode
            // 
            this.toolNewCMNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNewCMNode.Image = global::KRLab.DiagramEditor.Properties.Resources.CMNode;
            this.toolNewCMNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewCMNode.Name = "toolNewCMNode";
            this.toolNewCMNode.Size = new System.Drawing.Size(23, 22);
            this.toolNewCMNode.Click += new System.EventHandler(this.mnuNewNode_Click);


            // 
            // toolNewBNNode
            // 
            this.toolNewBNNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNewBNNode.Image = global::KRLab.DiagramEditor.Properties.Resources.BNNode;
            this.toolNewBNNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewBNNode.Name = "toolNewBNNode";
            this.toolNewBNNode.Size = new System.Drawing.Size(23, 22);
            this.toolNewBNNode.Click += new System.EventHandler(this.mnuNewNode_Click); 

            // 
            // toolNewSNRelationship

            this.toolNewSNRelationship.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNewSNRelationship.Image = global::KRLab.DiagramEditor.Properties.Resources.SNDefaultRelationship;
            this.toolNewSNRelationship.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewSNRelationship.Name = "toolNewSNRelationship";
            this.toolNewSNRelationship.Size = new System.Drawing.Size(23, 22);
            this.toolNewSNRelationship.Click += new System.EventHandler(this.mnuNewSNRelationship_Click);
 
            // 
            // toolNewCommentRelationship
            // 
            this.toolNewCommentRelationship.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNewCommentRelationship.Image = global::KRLab.DiagramEditor.Properties.Resources.CommentRel;
            this.toolNewCommentRelationship.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewCommentRelationship.Name = "toolNewCommentRelationship";
            this.toolNewCommentRelationship.Size = new System.Drawing.Size(23, 22);
            this.toolNewCommentRelationship.Click += new System.EventHandler(this.mnuNewCommentRelationship_Click);

			// 
			// toolDelete
			// 
			this.toolDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolDelete.Enabled = false;
			this.toolDelete.Image = global::KRLab.DiagramEditor.Properties.Resources.Delete;
			this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolDelete.Name = "toolDelete";
			this.toolDelete.Size = new System.Drawing.Size(23, 22);
			this.toolDelete.Click += new System.EventHandler(this.toolDelete_Click);

			// 
			// mnuAutoSize
			// 
			this.mnuAutoSize.Name = "mnuAutoSize";
			this.mnuAutoSize.Size = new System.Drawing.Size(161, 22);
			this.mnuAutoSize.Text = "Auto &Size";
			this.mnuAutoSize.Click += new System.EventHandler(this.mnuAutoSize_Click);
			// 
			// DiagramDynamicMenu
			// 
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.elementsToolStrip.ResumeLayout(false);
			this.elementsToolStrip.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuAddNewElement;

        #region nodes
        private System.Windows.Forms.ToolStripMenuItem mnuNewSNNode;
        private System.Windows.Forms.ToolStripMenuItem mnuNewCMNode;
        private System.Windows.Forms.ToolStripMenuItem mnuNewBNNode;
		private System.Windows.Forms.ToolStripMenuItem mnuNewComment;

        private System.Windows.Forms.ToolStripButton toolNewSNNode;
        private System.Windows.Forms.ToolStripButton toolNewCMNode;
        private System.Windows.Forms.ToolStripButton toolNewBNNode; 
        #endregion 

        #region relationships
        ///结点连接关系
        private System.Windows.Forms.ToolStripMenuItem mnuNewSNRelationship; 
		private System.Windows.Forms.ToolStripMenuItem mnuNewCommentRelationship;

        private System.Windows.Forms.ToolStripButton toolNewSNRelationship; 
        private System.Windows.Forms.ToolStripButton toolNewCommentRelationship;
        #endregion

        private System.Windows.Forms.ToolStripSeparator toolSepEntities;
        private System.Windows.Forms.ToolStripSeparator toolSepRelationships;
		private System.Windows.Forms.ToolStripSeparator toolSepNodeUpDown;

        private System.Windows.Forms.ToolStripButton toolDelete;

		private System.Windows.Forms.ToolStripMenuItem mnuMembersFormat;
		private System.Windows.Forms.ToolStripMenuItem mnuShowType;
		private System.Windows.Forms.ToolStripMenuItem mnuShowParameters;
		private System.Windows.Forms.ToolStripMenuItem mnuShowParameterNames;
		private System.Windows.Forms.ToolStripMenuItem mnuShowInitialValue;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAsImage;
		private System.Windows.Forms.ToolStripMenuItem mnuAlign;
		private System.Windows.Forms.ToolStripMenuItem mnuAlignTop;
		private System.Windows.Forms.ToolStripMenuItem mnuAlignLeft;
		private System.Windows.Forms.ToolStripMenuItem mnuAlignBottom;
		private System.Windows.Forms.ToolStripMenuItem mnuAlignRight;
		private System.Windows.Forms.ToolStripMenuItem mnuAlignHorizontal;
		private System.Windows.Forms.ToolStripMenuItem mnuAlignVertical;
		private System.Windows.Forms.ToolStripMenuItem mnuMakeSameSize;
		private System.Windows.Forms.ToolStripMenuItem mnuSameWidth;
		private System.Windows.Forms.ToolStripMenuItem mnuSameHeight;
		private System.Windows.Forms.ToolStripMenuItem mnuSameSize;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoWidth;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoHeight;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoLayout;
		private System.Windows.Forms.ToolStripMenuItem mnuCollapseAll;
		private System.Windows.Forms.ToolStripMenuItem mnuExpandAll;
		private System.Windows.Forms.ToolStripMenuItem mnuDiagram;
		private System.Windows.Forms.ToolStripMenuItem mnuFormat;

		private System.Windows.Forms.ToolStripMenuItem mnuGenerateCode;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStrip elementsToolStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoSize;
	}
}