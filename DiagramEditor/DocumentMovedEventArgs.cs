 

using System;

namespace KRLab.DiagramEditor
{
	public delegate void DocumentMovedEventHandler(object sender, DocumentMovedEventArgs e);

	public class DocumentMovedEventArgs : DocumentEventArgs
	{
		int oldPostion;
		int newPosition;

		public DocumentMovedEventArgs(IDocument document, int oldPostion, int newPosition)
			: base(document)
		{
			this.oldPostion = oldPostion;
			this.newPosition = newPosition;
		}

		public int OldPostion
		{
			get { return oldPostion; }
		}

		public int NewPosition
		{
			get { return newPosition; }
		}
	}
}
