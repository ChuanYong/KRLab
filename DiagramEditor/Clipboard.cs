 

using System;
using System.Collections.Generic;

namespace KRLab.DiagramEditor
{
	public static class Clipboard
	{
		static IClipboardItem item = null;

		public static IClipboardItem Item
		{
			get { return Clipboard.item; }
			set { Clipboard.item = value; }
		}

		public static bool IsEmpty
		{
			get { return (item == null); }
		}

		public static void Clear()
		{
			item = null;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="document"/> is null.
		/// </exception>
		public static void Paste(IDocument document)
		{
			if (document == null)
				throw new ArgumentNullException("document");

			item.Paste(document);
		}
	}
}
