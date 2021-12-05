// KRLab - Free class diagram editor
 

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.Core;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public abstract class FloatingEditor : EditorWindow
	{
		protected const int MarginSize = 20;
		static readonly Color beginColor = SystemColors.ControlLight;
		static readonly Color endColor = SystemColors.Control;

        static MemberType newMemberType = MemberType.Name;

        protected static MemberType NewMemberType
        {
            get { return newMemberType; }
            set { newMemberType = value; }
        }

		protected FloatingEditor()
		{
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Padding = new Padding(1);
		} 

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, Width - 1, Height - 1);
		}
	}
}
