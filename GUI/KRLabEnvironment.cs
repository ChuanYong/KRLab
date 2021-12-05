 

using System;
using KRLab.Core;
using KRLab.DiagramEditor;

namespace KRLab.GUI
{
	public sealed class KRLabEnvironment
	{
		Workspace workspace;
		DocumentManager docManager;

        internal KRLabEnvironment(Workspace workspace, DocumentManager docManager)
		{
			this.workspace = workspace;
			this.docManager = docManager;
		}

		public Workspace Workspace
		{
			get { return workspace; }
		}

		public DocumentManager DocumentManager
		{
			get { return docManager; }
		}
	}
}
