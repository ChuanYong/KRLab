 

using System;
using System.Drawing;

namespace KRLab.DiagramEditor
{
	public interface IPrintable
	{
		/// <exception cref="ArgumentNullException">
		/// <paramref name="g"/> is null.-or-
		/// <paramref name="style"/> is null.
		/// </exception>
		void Print(IGraphics g, bool selectedOnly, Style style);

		RectangleF GetPrintingArea(bool selectedOnly);

		void ShowPrintDialog();
	}
}
