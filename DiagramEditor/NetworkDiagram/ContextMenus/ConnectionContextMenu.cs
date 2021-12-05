 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KRLab.DiagramEditor.Properties;
using KRLab.DiagramEditor.NetworkDiagram.Connections;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.ContextMenus
{
	internal sealed class ConnectionContextMenu : DiagramContextMenu
	{
		static ConnectionContextMenu _default = new ConnectionContextMenu();

		ToolStripMenuItem mnuAutoRouting;

		private ConnectionContextMenu()
		{
			InitMenuItems();
		}

		public static ConnectionContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuAutoRouting.Text = Strings.MenuAutoRouting;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			GeneralContextMenu.Default.ValidateMenuItems(diagram);
		}

		private void InitMenuItems()
		{
			mnuAutoRouting = new ToolStripMenuItem(Strings.MenuAutoRouting,
				null, mnuAutoRouting_Click);

			MenuList.AddRange(GeneralContextMenu.Default.MenuItems);
			MenuList.AddRange(new ToolStripItem[] {
				new ToolStripSeparator(),
				mnuAutoRouting,
			});
		}

		private void mnuAutoRouting_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				foreach (Connection connection in Diagram.GetSelectedConnections())
					connection.AutoRoute();
			}
		}
	}
}