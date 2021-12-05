 

using System;
using System.Drawing;
using System.Windows.Forms;
using KRLab.Translations;
using System.ComponentModel;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public abstract partial class ItemEditor : FloatingEditor
	{
		bool needValidation = false;

		public ItemEditor()
		{
			InitializeComponent();
			UpdateTexts();
			toolStrip.Renderer = ToolStripSimplifiedRenderer.Default;
		}

		protected string DeclarationText
		{
			get { return txtDeclaration.Text; }
			set { txtDeclaration.Text = value; }
		}

		protected int SelectionStart
		{
			get { return txtDeclaration.SelectionStart; }
			set { txtDeclaration.SelectionStart = value; }
		}

		protected bool NeedValidation
		{
			get { return needValidation; }
			set { needValidation = value; }
		}

		internal override void Init(DiagramElement element)
		{
			RefreshValues();
		}

		protected virtual void UpdateTexts()
		{
			toolMoveUp.ToolTipText = Strings.MoveUp + " (Ctrl+Up)";
			toolMoveDown.ToolTipText = Strings.MoveDown + " (Ctrl+Down)";
			toolDelete.ToolTipText = Strings.Delete;
		}

		public override void ValidateData()
		{
			ValidateDeclarationLine();
			SetError(null);
		}

		protected void SetError(string message)
		{
			if (MonoHelper.IsRunningOnMono && MonoHelper.IsOlderVersionThan("2.4"))
				return;

			errorProvider.SetError(this, message);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			txtDeclaration.SelectionStart = 0;
		}

		private void txtDeclaration_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					ValidateDeclarationLine();
					e.Handled = true;
					break;

				case Keys.Escape:
					needValidation = false;
					HideEditor();
					e.Handled = true;
					break;

				case Keys.Up:
					if (e.Shift || e.Control)
						MoveUp();
					else
						SelectPrevious();
					e.Handled = true;
					break;

				case Keys.Down:
					if (e.Shift || e.Control)
						MoveDown();
					else
						SelectNext();
					e.Handled = true;
					break;
			}
		}

		private void txtDeclaration_TextChanged(object sender, EventArgs e)
		{
			needValidation = true;
		}

		private void txtDeclaration_Validating(object sender, CancelEventArgs e)
		{
			ValidateDeclarationLine();
		}

		private void toolMoveUp_Click(object sender, EventArgs e)
		{
			MoveUp();
		}

		private void toolMoveDown_Click(object sender, EventArgs e)
		{
			MoveDown();
		}

		private void toolDelete_Click(object sender, EventArgs e)
		{
			Delete();
		}

		internal abstract override void Relocate(DiagramElement element);

		protected abstract void HideEditor();

		protected abstract void RefreshValues();

		protected abstract bool ValidateDeclarationLine();

		protected abstract void SelectPrevious();

		protected abstract void SelectNext();

		protected abstract void MoveUp();

		protected abstract void MoveDown();

		protected abstract void Delete();
	}
}
