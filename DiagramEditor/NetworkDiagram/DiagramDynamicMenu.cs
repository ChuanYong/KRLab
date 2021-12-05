 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KRLab.Translations;
using KRLab.Core; 
using KRLab.BNet; 

namespace KRLab.DiagramEditor.NetworkDiagram
{
	public sealed partial class DiagramDynamicMenu : DynamicMenu
	{
		static DiagramDynamicMenu _default = new DiagramDynamicMenu();

		ToolStripMenuItem[] menuItems;
		Diagram diagram = null; 

		private DiagramDynamicMenu()
		{
			InitializeComponent();
			UpdateTexts();
			
			menuItems = new ToolStripMenuItem[2] { mnuDiagram, mnuFormat };
		}

		public static DiagramDynamicMenu Default
		{
			get { return _default; }
		}

		public override IEnumerable<ToolStripMenuItem> GetMenuItems()
		{
			return menuItems;
		}

		public override ToolStrip GetToolStrip()
		{
			return elementsToolStrip;
		}

		public override void SetReference(IDocument document)
		{
			if (diagram != null)
			{
				diagram.SelectionChanged -= new EventHandler(diagram_SelectionChanged);
			}

			if (document == null)
			{
				diagram = null;
			}
			else
			{
				diagram = document as Diagram;
				diagram.SelectionChanged += new EventHandler(diagram_SelectionChanged);                           
				toolDelete.Enabled = diagram.HasSelectedElement;          

			}
		}

		private void diagram_SelectionChanged(object sender, EventArgs e)
		{
			toolDelete.Enabled = (diagram != null && diagram.HasSelectedElement);
		}

		private void UpdateTexts()
		{
			// Diagram menu
			mnuDiagram.Text = Strings.MenuDiagram;
			mnuAddNewElement.Text = Strings.MenuNew;

            mnuNewSNNode.Text = Strings.MenuSNNode;
            mnuNewCMNode.Text = Strings.MenuCMNode;
            mnuNewBNNode.Text = Strings.MenuBNNode;
			mnuNewComment.Text = Strings.MenuComment;

            mnuNewSNRelationship.Text = Strings.SNRelationship; 
			mnuNewCommentRelationship.Text = Strings.MenuCommentRelationship; 

			mnuMembersFormat.Text = Strings.MenuMembersFormat;
			mnuShowType.Text = Strings.MenuType;
			mnuShowParameters.Text = Strings.MenuParameters;
			mnuShowParameterNames.Text = Strings.MenuParameterNames;
			mnuShowInitialValue.Text = Strings.MenuInitialValue;
			mnuGenerateCode.Text = Strings.MenuGenerateCode;
			mnuSaveAsImage.Text = Strings.MenuSaveAsImage;

			// Format menu
			mnuFormat.Text = Strings.MenuFormat;
			mnuAlign.Text = Strings.MenuAlign;
			mnuAlignTop.Text = Strings.MenuAlignTop;
			mnuAlignLeft.Text = Strings.MenuAlignLeft;
			mnuAlignBottom.Text = Strings.MenuAlignBottom;
			mnuAlignRight.Text = Strings.MenuAlignRight;
			mnuAlignHorizontal.Text = Strings.MenuAlignHorizontal;
			mnuAlignVertical.Text = Strings.MenuAlignVertical;
			mnuMakeSameSize.Text = Strings.MenuMakeSameSize;
			mnuSameWidth.Text = Strings.MenuSameWidth;
			mnuSameHeight.Text = Strings.MenuSameHeight;
			mnuSameSize.Text = Strings.MenuSameSize;
			mnuAutoSize.Text = Strings.MenuAutoSize;
			mnuAutoWidth.Text = Strings.MenuAutoWidth;
			mnuAutoHeight.Text = Strings.MenuAutoHeight;
			mnuCollapseAll.Text = Strings.MenuCollapseAll;
			mnuExpandAll.Text = Strings.MenuExpandAll;

			// Toolbar
            toolNewSNNode.Text = Strings.AddNewSNNode;
            toolNewBNNode.Text = Strings.AddNewBNNode;
            toolNewCMNode.Text = Strings.AddNewCMNode; 

            toolNewSNRelationship.Text = Strings.SNRelationship; 
			toolNewCommentRelationship.Text = Strings.CommentRelationship; 
			toolDelete.Text = Strings.DeleteSelectedItems;
		}

		#region Event handlers

		private void mnuDiagram_DropDownOpening(object sender, EventArgs e)
		{
			bool hasContent = (diagram != null && !diagram.IsEmpty);
			mnuGenerateCode.Enabled = hasContent;
			mnuSaveAsImage.Enabled = hasContent;
		}

		private void mnuNewNode_Click(object sender, EventArgs e)
		{
            if (diagram != null)
            {
                diagram.CreateNodeShape();
            }
		}

        private void mnuNewComment_Click(object sender, EventArgs e)
        {
            if (diagram != null)
                diagram.CreateShape(EntityType.Comment);
        }

        private void mnuNewSNRelationship_Click(object sender, EventArgs e)
        {
			if(diagram!=null)
				diagram.CreateConnection(RelationshipType.SN_REL);
        }  
         

		private void mnuNewCommentRelationship_Click(object sender, EventArgs e)
		{
			diagram.CreateConnection(RelationshipType.Comment);
			//toolNewCommentRelationship.Checked = true;
		}  
		private void mnuShowType_Click(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowType = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
		}

		private void mnuShowParameters_Click(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowParameters = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
		}

		private void mnuShowParameterNames_Click(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowParameterNames = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
		}

		private void mnuShowInitialValue_Click(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowInitialValue = ((ToolStripMenuItem) sender).Checked;
			if (diagram != null)
				diagram.Redraw();
		}

		private void mnuGenerateCode_Click(object sender, EventArgs e)
		{
            //if (diagram != null && diagram.Project != null)
            //{
            //    using (CodeGenerator.Dialog dialog = new CodeGenerator.Dialog())
            //    {
            //        try
            //        {
            //            dialog.ShowDialog(diagram.Project);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message, Strings.UnknownError,
            //                MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}
		}

		private void mnuSaveAsImage_Click(object sender, EventArgs e)
		{
			if (diagram != null && !diagram.IsEmpty)
				diagram.SaveAsImage();
		}

		private void mnuFormat_DropDownOpening(object sender, EventArgs e)		
		{
			bool shapeSelected = (diagram != null && diagram.SelectedShapeCount >= 1);
			bool multiselection = (diagram != null && diagram.SelectedShapeCount >= 2);

			mnuAutoWidth.Enabled = shapeSelected;
			mnuAutoHeight.Enabled = shapeSelected;
			mnuAlign.Enabled = multiselection;
			mnuMakeSameSize.Enabled = multiselection;
		}

		private void mnuAlignTop_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignTop();
		}

		private void mnuAlignLeft_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignLeft();
		}

		private void mnuAlignBottom_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignBottom();
		}

		private void mnuAlignRight_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignRight();
		}

		private void mnuAlignHorizontal_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignHorizontal();
		}

		private void mnuAlignVertical_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AlignVertical();
		}

		private void mnuSameWidth_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AdjustToSameWidth();
		}

		private void mnuSameHeight_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AdjustToSameHeight();
		}

		private void mnuSameSize_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AdjustToSameSize();
		}

		private void mnuAutoSize_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AutoSizeOfSelectedShapes();
		}

		private void mnuAutoWidth_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AutoWidthOfSelectedShapes();
		}

		private void mnuAutoHeight_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.AutoHeightOfSelectedShapes();
		}

		private void mnuCollapseAll_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.CollapseAll();
		}

		private void mnuExpandAll_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.ExpandAll();
		}

		private void toolDelete_Click(object sender, EventArgs e)
		{
			if (diagram != null)
				diagram.DeleteSelectedElements();
		}

		#endregion
	}
}