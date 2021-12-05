 

using System;
using System.Drawing;

namespace KRLab.DiagramEditor.NetworkDiagram
{
	public delegate void MoveEventHandler(object sender, MoveEventArgs e);

	public class MoveEventArgs : EventArgs
	{
		Size offset;

		public MoveEventArgs(Size offset)
		{
			this.offset = offset;
		}

		public Size Offset
		{
			get { return offset; }
		}
	}
}
