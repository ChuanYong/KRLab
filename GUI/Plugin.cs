 

using System;
using System.Windows.Forms;
using KRLab.DiagramEditor;

namespace KRLab.GUI
{
	public abstract class Plugin
	{
		KRLabEnvironment environment;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="environment"/> is null.
		/// </exception>
		protected Plugin(KRLabEnvironment environment)
		{
			if (environment == null)
				throw new ArgumentNullException("environment");

			this.environment = environment;
		}

		protected KRLabEnvironment KRLabEnvironment
		{
			get { return environment; }
		}

		protected Workspace Workspace
		{
			get { return environment.Workspace; }
		}

		protected DocumentManager DocumentManager
		{
			get { return environment.DocumentManager; }
		}

		public abstract bool IsAvailable
		{
			get;
		}

		public abstract ToolStripItem MenuItem
		{
			get;
		}
	}
}