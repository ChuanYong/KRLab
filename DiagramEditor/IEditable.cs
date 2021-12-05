 

using System;

namespace KRLab.DiagramEditor
{
	public interface IEditable
	{
		bool IsEmpty { get; }

		bool CanCutToClipboard { get; }

		bool CanCopyToClipboard { get; }

		bool CanPasteFromClipboard { get; }

		
		event EventHandler ClipboardAvailabilityChanged;

		
		void Cut();

		void Copy();

		void Paste();

		void SelectAll();

		void DeleteSelectedElements();
	}
}
