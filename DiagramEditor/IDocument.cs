 

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using KRLab.Core;

namespace KRLab.DiagramEditor
{
	public interface IDocument : IProjectItem, IEditable, IPrintable
	{
		event EventHandler OffsetChanged;
		event EventHandler SizeChanged;
		event EventHandler ZoomChanged;
		event EventHandler StatusChanged;
		event EventHandler NeedsRedraw;
		event PopupWindowEventHandler ShowingWindow;
		event PopupWindowEventHandler HidingWindow;


		Point Offset { get; set; }

		Size Size { get; }

		float Zoom { get; set; }

		Color BackColor { get; }

		bool HasSelectedElement { get; }


		void Display(Graphics g);

		void Redraw();

		void CloseWindows();

		DynamicMenu GetDynamicMenu();

		string GetStatus();

		string GetShortDescription();

		string GetSelectedElementName();

		void MouseDown(AbsoluteMouseEventArgs e);

		void MouseMove(AbsoluteMouseEventArgs e);

		void MouseUp(AbsoluteMouseEventArgs e);

		void DoubleClick(AbsoluteMouseEventArgs e);

		void KeyDown(KeyEventArgs e);

		ContextMenuStrip GetContextMenu(AbsoluteMouseEventArgs e);
	}
}
