 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.DiagramEditor.Properties;
using KRLab.Translations;
using KRLab.Core;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
	public sealed class BlankContextMenu : DiagramContextMenu
	{
		static BlankContextMenu _default = new BlankContextMenu();

		#region MenuItem fields

		ToolStripMenuItem mnuAddNewElement;
        ToolStripMenuItem mnuAddNewSNNode;
        ToolStripMenuItem mnuAddNewCMNode;
        ToolStripMenuItem mnuAddNewBNNode;
        ToolStripMenuItem mnuAddNewComment;

        ToolStripMenuItem mnuNewSNRelationship; 
		ToolStripMenuItem mnuNewCommentRelationship;

		ToolStripMenuItem mnuMembersFormat;
		ToolStripMenuItem mnuShowType;
		ToolStripMenuItem mnuShowParameters;
		ToolStripMenuItem mnuShowParameterNames;
		ToolStripMenuItem mnuShowInitialValue;

		ToolStripMenuItem mnuPaste;
		ToolStripMenuItem mnuSaveAsImage;
		ToolStripMenuItem mnuSelectAll;

		#endregion

		private BlankContextMenu()
		{
			InitMenuItems();
		}

		public static BlankContextMenu Default
		{
			get { return _default; }
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			mnuPaste.Enabled = diagram.CanPasteFromClipboard; 

			mnuShowType.Checked = DiagramEditor.Settings.Default.ShowType;
			mnuShowParameters.Checked = DiagramEditor.Settings.Default.ShowParameters;
			mnuShowParameterNames.Checked = DiagramEditor.Settings.Default.ShowParameterNames;
			mnuShowInitialValue.Checked = DiagramEditor.Settings.Default.ShowInitialValue;

			mnuSaveAsImage.Enabled = !diagram.IsEmpty;
		}

		private void InitMenuItems()
		{
            mnuAddNewElement = new ToolStripMenuItem(Strings.MenuNew, Resources.NewEntity);
            mnuAddNewCMNode = new ToolStripMenuItem(Strings.MenuCMNode, Resources.CMNode, mnuNewNode_Click);
            mnuAddNewSNNode = new ToolStripMenuItem(Strings.MenuSNNode, Resources.SNNode, mnuNewNode_Click);
            mnuAddNewBNNode = new ToolStripMenuItem(Strings.MenuBNNode, Resources.BNNode, mnuNewNode_Click);
            mnuAddNewComment=new ToolStripMenuItem(Strings.MenuComment,Resources.Comment,mnuNewComment_Click);

            mnuNewSNRelationship = new ToolStripMenuItem(Strings.SNRelationship, Resources.SNDefaultRelationship, mnuNewSNRelation_Click);
            mnuNewCommentRelationship = new ToolStripMenuItem(Strings.CommentRelationship,Resources.CommentRel,mnuNewCommentRelation_Click);

            mnuMembersFormat = new ToolStripMenuItem(Strings.MenuMembersFormat, null);
			mnuShowType = new ToolStripMenuItem(Strings.MenuType, null);
			mnuShowType.CheckedChanged += mnuShowType_CheckedChanged;
			mnuShowType.CheckOnClick = true;
			mnuShowParameters = new ToolStripMenuItem(Strings.MenuParameters, null);
			mnuShowParameters.CheckedChanged += mnuShowParameters_CheckedChanged;
			mnuShowParameters.CheckOnClick = true;
			mnuShowParameterNames = new ToolStripMenuItem(Strings.MenuParameterNames, null);
			mnuShowParameterNames.CheckedChanged += mnuShowParameterNames_CheckedChanged;
			mnuShowParameterNames.CheckOnClick = true;
			mnuShowInitialValue = new ToolStripMenuItem(Strings.MenuInitialValue, null);
			mnuShowInitialValue.CheckedChanged += mnuShowInitialValue_CheckedChanged;
			mnuShowInitialValue.CheckOnClick = true;

			mnuPaste = new ToolStripMenuItem(Strings.MenuPaste, Resources.Paste, mnuPaste_Click);
			mnuSaveAsImage = new ToolStripMenuItem(Strings.MenuSaveAsImage, Resources.Image, mnuSaveAsImage_Click);
			mnuSelectAll = new ToolStripMenuItem(Strings.MenuSelectAll, null, mnuSelectAll_Click);

			mnuAddNewElement.DropDownItems.AddRange(new ToolStripItem[] {                 
                mnuAddNewCMNode,
                mnuAddNewSNNode,
                mnuAddNewBNNode,
                mnuAddNewComment,
				new ToolStripSeparator(), 
                mnuNewSNRelationship, 
				mnuNewCommentRelationship
			});
			mnuMembersFormat.DropDownItems.AddRange(new ToolStripItem[] {
				mnuShowType,
				mnuShowParameters,
				mnuShowParameterNames,
				mnuShowInitialValue
			});
			MenuList.AddRange(new ToolStripItem[] {
				mnuAddNewElement,
				mnuMembersFormat,
				new ToolStripSeparator(),
				mnuPaste,
				mnuSaveAsImage,
				mnuSelectAll
			});
		}
        private void mnuNewNode_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
            {
                Diagram.CreateNodeShape();
            }
        }

		private void mnuNewComment_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.CreateShape(EntityType.Comment);
		}

        private void mnuNewSNRelation_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
                Diagram.CreateConnection(RelationshipType.SN_REL);
        }            

		private void mnuNewCommentRelation_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.CreateConnection(RelationshipType.Comment);
		}   

		private void mnuShowType_CheckedChanged(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowType = ((ToolStripMenuItem) sender).Checked;
			if (Diagram != null)
				Diagram.Redraw();
		}

		private void mnuShowParameters_CheckedChanged(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowParameters = ((ToolStripMenuItem) sender).Checked;
			if (Diagram != null)
				Diagram.Redraw();
		}

		private void mnuShowParameterNames_CheckedChanged(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowParameterNames = ((ToolStripMenuItem) sender).Checked;
			if (Diagram != null)
				Diagram.Redraw();
		}

		private void mnuShowInitialValue_CheckedChanged(object sender, EventArgs e)
		{
			DiagramEditor.Settings.Default.ShowInitialValue = ((ToolStripMenuItem) sender).Checked;
			if (Diagram != null)
				Diagram.Redraw();
		}

		private void mnuPaste_Click(object sender, EventArgs e)
		{ 
            if (Diagram != null)
            { 
                Diagram.Paste();
            }
		}

		private void mnuSaveAsImage_Click(object sender, EventArgs e)
		{
			if (Diagram != null && !Diagram.IsEmpty)
				Diagram.SaveAsImage();
		}

		private void mnuSelectAll_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
				Diagram.SelectAll();
		}
	}
}