 

using System;

namespace KRLab.DiagramEditor
{
	public interface IClipboardItem
	{
		void Paste(IDocument document);
	}
}
