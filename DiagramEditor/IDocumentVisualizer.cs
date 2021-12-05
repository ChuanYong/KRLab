
using System;
using System.Drawing;
using System.Windows.Forms;

namespace KRLab.DiagramEditor
{
	public interface IDocumentVisualizer
	{
		event EventHandler DocumentRedrawed;
		event EventHandler VisibleAreaChanged;


		bool HasDocument { get; }

		IDocument Document { get; }

		Point Offset { get; set; }

		Size DocumentSize { get; }

		Rectangle VisibleArea { get; }

		float Zoom { get; }


		void ChangeZoom(bool enlarge);
		
		void ChangeZoom(float zoom);

		void AutoZoom();

		void AutoZoom(bool selectedOnly);

		void DrawDocument(Graphics g);
	}
}
