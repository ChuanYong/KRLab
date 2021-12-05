 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.DiagramEditor.Properties;
using KRLab.DiagramEditor.NetworkDiagram.Connections;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
    internal sealed class SNISAContextMenu : DiagramContextMenu
    {
        static SNISAContextMenu _default = new SNISAContextMenu();

        ToolStripMenuItem mnuDirection, mnuUnidirectional;
        ToolStripMenuItem mnuType, mnuSNISARelation;
        ToolStripMenuItem mnuReverse;
        ToolStripMenuItem mnuEdit;

        private SNISAContextMenu()
        {
            InitMenuItems();
        }

        public static SNISAContextMenu Default
        {
            get { return _default; }
        }

        private void UpdateTexts()
        {
            mnuDirection.Text = Strings.MenuDirection;
            mnuUnidirectional.Text = Strings.MenuUnidirectional;
            mnuType.Text = Strings.MenuType;
            mnuSNISARelation.Text = Strings.SNRelationship; 
            mnuReverse.Text = Strings.MenuReverse;
            mnuEdit.Text = Strings.MenuProperties;
        }

        public override void ValidateMenuItems(Diagram diagram)
        {
            base.ValidateMenuItems(diagram);
            ConnectionContextMenu.Default.ValidateMenuItems(diagram);
            mnuEdit.Enabled = (diagram.SelectedElementCount == 1);
        }

        private void InitMenuItems()
        {
            mnuUnidirectional = new ToolStripMenuItem(Strings.MenuUnidirectional,
                Resources.Unidirectional, mnuUnidirectional_Click); 
            mnuDirection = new ToolStripMenuItem(Strings.MenuDirection, null,
                mnuUnidirectional 
            );

            mnuSNISARelation = new ToolStripMenuItem(Strings.SNRelationship,
                Resources.SNISARelationship, mnuSNISARelationship_Click); 
            mnuType = new ToolStripMenuItem(Strings.MenuType, null,
                mnuSNISARelation 
            );

            mnuReverse = new ToolStripMenuItem(Strings.MenuReverse, null, mnuReverse_Click);
            mnuEdit = new ToolStripMenuItem(Strings.MenuEditAssociation,
                Resources.Property, mnuEdit_Click);

            MenuList.AddRange(ConnectionContextMenu.Default.MenuItems);
            MenuList.InsertRange(7, new ToolStripItem[] {
				mnuDirection,
				mnuType,
				mnuReverse,
				new ToolStripSeparator(),
			});
            MenuList.Add(mnuEdit);
        }

        private void mnuUnidirectional_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
            {
                foreach (SNConnection connection in Diagram.GetSelectedConnections())
                    connection.SNRelationship.Direction = Direction.Unidirectional;
            }
        }

        private void mnuBidirectional_Click(object sender, EventArgs e)
        {
            //if (Diagram != null)
            //{
            //    foreach (Association association in Diagram.GetSelectedConnections())
            //        association.AssociationRelationship.Direction = Direction.Bidirectional;
            //}
        }

        private void mnuSNISARelationship_Click(object sender, EventArgs e)
        {
            if (Diagram != null)
            {
                //foreach (Association association in Diagram.GetSelectedConnections())
                //{ 
                //}
            }
        } 

        private void mnuReverse_Click(object sender, EventArgs e)
        {
            //if (Diagram != null)
            //{
            //    Association association = Diagram.TopSelectedElement as Association;
            //    if (association != null)
            //        association.AssociationRelationship.Reverse();
            //}
        }

        private void mnuEdit_Click(object sender, EventArgs e)
        {
            //if (Diagram != null)
            //{
            //    Association association = Diagram.TopSelectedElement as Association;
            //    //if (association != null)
            //    //    association.ShowEditDialog();
            //}
        }
    }
}