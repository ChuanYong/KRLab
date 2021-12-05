 

using System;
using System.Windows.Forms;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.Dialogs
{
	public partial class EditCommentDialog : Form
	{
		public EditCommentDialog()
		{
			InitializeComponent();
		}

		public EditCommentDialog(string initText)
		{
			InitializeComponent();
			txtInput.Text = initText;
		}

		public string InputText
		{
			get { return txtInput.Text; }
		}

		private void UpdateTexts()
		{
			this.Text = Strings.EditComment;
			lblEdit.Text = Strings.EditText;
			btnOK.Text = Strings.ButtonOK;
			btnCancel.Text = Strings.ButtonCancel;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateTexts();
		}
	}
}