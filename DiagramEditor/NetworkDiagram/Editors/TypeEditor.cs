 

using System;
using System.Drawing;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public abstract class TypeEditor : FloatingEditor
	{
		internal sealed override void Relocate(DiagramElement element)
		{
			Relocate((SimpleNodeShape) element);
		}

		internal void Relocate(SimpleNodeShape shape)
		{
			Diagram diagram = shape.Diagram;
			if (diagram != null)
			{
				Point absolute = new Point(shape.Right, shape.Top);
				Size relative = new Size(
					(int) (absolute.X * diagram.Zoom) - diagram.Offset.X + MarginSize,
					(int) (absolute.Y * diagram.Zoom) - diagram.Offset.Y);

				this.Location = ParentLocation + relative;
			}
		}
	}
}
