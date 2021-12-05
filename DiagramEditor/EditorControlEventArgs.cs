 

using System;
using System.Windows.Forms;

namespace KRLab.DiagramEditor
{
	public delegate void PopupWindowEventHandler(object sender, PopupWindowEventArgs e);

	public class PopupWindowEventArgs
	{
		PopupWindow window;

		public PopupWindowEventArgs(PopupWindow window)
		{
			this.window = window;
		}

		public PopupWindow Window
		{
			get { return window; }
		}
	}
}
