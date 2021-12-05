 

using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public class BorderedTextBox : UserControl
	{
		private class TabTextBox : TextBox
		{
			protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
			{
				if (keyData == Keys.Enter && AcceptsReturn)
				{
					OnKeyDown(new KeyEventArgs(keyData));
					return true;
				}
				else if (keyData == Keys.Tab && AcceptsTab)
				{
					OnKeyDown(new KeyEventArgs(keyData));
					return true;
				}
				else
				{
					return base.ProcessCmdKey(ref msg, keyData);
				}
			}
		}

		TabTextBox textBox = new TabTextBox();
		Panel panel = new Panel();

		public BorderedTextBox()
		{
			textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			textBox.BorderStyle = BorderStyle.FixedSingle;
			textBox.Location = new Point(-1, -1);
			textBox.AcceptsReturn = true;
			panel.Dock = DockStyle.Fill;
			panel.Size = textBox.Size - new Size(2, 0);
			panel.Controls.Add(textBox);

			textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
			textBox.TextChanged += new EventHandler(textBox_TextChanged);
			textBox.Validating += new CancelEventHandler(textBox_Validating);
			textBox.GotFocus += new EventHandler(textBox_GotFocus);
			textBox.LostFocus += new EventHandler(textBox_LostFocus);
			
			this.Padding = new Padding(1);
			this.BorderColor = SystemColors.ControlDark;
			this.Controls.Add(panel);
		}

		[DefaultValue(typeof(Color), "ControlDark")]
		public Color BorderColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		[DefaultValue(typeof(Color), "Window")]
		public new Color BackColor
		{
			get { return textBox.BackColor; }
			set { textBox.BackColor = value; }
		}

		public bool ReadOnly
		{
			get { return textBox.ReadOnly; }
			set { textBox.ReadOnly = value; }
		}

		public override string Text
		{
			get { return textBox.Text; }
			set { textBox.Text = value; }
		}

		[DefaultValue(true)]
		public bool AcceptsReturn
		{
			get { return textBox.AcceptsReturn; }
			set { textBox.AcceptsReturn = value; }
		}

		[DefaultValue(false)]
		public bool AcceptsTab
		{
			get { return textBox.AcceptsTab; }
			set { textBox.AcceptsTab = value; }
		}

		/// <exception cref="ArgumentOutOfRangeException">
		/// The assigned value is less than zero.
		/// </exception>
		public int SelectionStart
		{
			get { return textBox.SelectionStart; }
			set { textBox.SelectionStart = value; }
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		private void textBox_TextChanged(object sender, EventArgs e)
		{
			OnTextChanged(e);
		}

		private void textBox_GotFocus(object sender, EventArgs e)
		{
			OnGotFocus(e);
		}

		private void textBox_LostFocus(object sender, EventArgs e)
		{
			OnLostFocus(e);
		}

		private void textBox_Validating(object sender, CancelEventArgs e)
		{
			OnValidating(e);
		}

		protected override void SetBoundsCore(int x, int y,
			int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, textBox.PreferredHeight, specified);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				textBox.Dispose();
				panel.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
