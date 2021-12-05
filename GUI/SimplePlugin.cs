 

using System;
using System.Windows.Forms;
using KRLab.Translations;

namespace KRLab.GUI
{
	public abstract class SimplePlugin : Plugin
	{
		ToolStripMenuItem menuItem;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="environment"/> is null.
		/// </exception>
		protected SimplePlugin(KRLabEnvironment environment) : base(environment)
		{
			menuItem = new ToolStripMenuItem();
			menuItem.Text = MenuText;
			menuItem.ToolTipText = string.Format(Strings.PluginTooltip, Name, Author);
			menuItem.Click += new EventHandler(menuItem_Click);
		}

		public override ToolStripItem MenuItem
		{
			get { return menuItem; }
		}

		private void menuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Launch();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Strings.UnknownError,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#region Abstract members

		public abstract string Name
		{
			get;
		}

		public abstract string Author
		{
			get;
		}

		public abstract string MenuText
		{
			get;
		}

		protected abstract void Launch();

		#endregion
	}
}