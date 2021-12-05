 

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.Core;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public abstract class EditorWindow : PopupWindow
	{
		internal abstract void Init(DiagramElement element);

		internal abstract void Relocate(DiagramElement element);

		public abstract void ValidateData();

		public override void Closing()
		{
			ValidateData();
		}
	}
}
