 

using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public partial class MemberEditor : FloatingEditor
	{
		CompositeNodeShape shape = null;
		bool needValidation = false;

		public MemberEditor()
		{
			InitializeComponent();
			toolStrip.Renderer = ToolStripSimplifiedRenderer.Default;
			UpdateTexts();
			if (MonoHelper.IsRunningOnMono)
				toolNewMember.Alignment = ToolStripItemAlignment.Left;
		}

		private void UpdateTexts()
		{ 
			toolNewNameMember.Text = Strings.NewField;
			toolNewStateMember.Text = Strings.NewMethod; 
			toolNewCPMember.Text = Strings.NewProperty; 
			toolMoveUp.ToolTipText = Strings.MoveUp + " (Ctrl+Up)";
			toolMoveDown.ToolTipText = Strings.MoveDown + " (Ctrl+Down)";
			toolDelete.ToolTipText = Strings.Delete;
		}

		internal override void Init(DiagramElement element)
		{
			shape = (CompositeNodeShape) element;
			RefreshValues();
		}

		private void RefreshValues()
		{
			if (shape.ActiveMember != null)
			{
				Member member = shape.ActiveMember;
				SuspendLayout(); 

				int cursorPosition = txtDeclaration.SelectionStart;
				txtDeclaration.Text = member.ToString();
				txtDeclaration.SelectionStart = cursorPosition; 
				
				SetError(null);
				needValidation = false;       
				
				RefreshNewMembers();
				RefreshMoveUpDownTools();
				ResumeLayout();
			}
		} 

		private void RefreshNewMembers()
		{
			bool valid = false;
			switch (NewMemberType)
			{
				case MemberType.Name:		
                    toolNewMember.Image = Properties.Resources.NewField;
					toolNewMember.Text = Strings.NewField;
                    toolNewNameMember.Visible = true;
					valid = true;
					break; 

			}

			if (!valid)
			{
				NewMemberType = MemberType.Name;
				toolNewMember.Image = Properties.Resources.NewMethod;
				toolNewMember.Text = Strings.NewMethod;
			}
              
		}

		private void RefreshMoveUpDownTools()
		{
            //int index = shape.ActiveMemberIndex;
            //int fieldCount = shape.Node.FieldCount;
            //int memberCount = shape.Node.MemberCount;

            //toolMoveUp.Enabled = ((index < fieldCount && index > 0) || index > fieldCount);
            //toolMoveDown.Enabled = (index < fieldCount - 1 ||
            //    (index >= fieldCount && index < memberCount - 1));
		}

		internal override void Relocate(DiagramElement element)
		{
			Relocate((CompositeNodeShape) element);
		}

		internal void Relocate(CompositeNodeShape shape)
		{
			Diagram diagram = shape.Diagram;
			if (diagram != null)
			{
				Rectangle record = shape.GetMemberRectangle(shape.ActiveMemberIndex);

				Point absolute = new Point(shape.Right, record.Top);
				Size relative = new Size(
					(int) (absolute.X * diagram.Zoom) - diagram.Offset.X + MarginSize,
					(int) (absolute.Y * diagram.Zoom) - diagram.Offset.Y);
				relative.Height -= (Height - (int) (record.Height * diagram.Zoom)) / 2;

				this.Location = ParentLocation + relative;
			}
		}

		public override void ValidateData()
		{
			ValidateDeclarationLine();
			SetError(null);
		}

		private bool ValidateDeclarationLine()
		{
            //if (needValidation && shape.ActiveMember != null)
            //{
            //    try
            //    {
            //        shape.ActiveMember.InitFromString(txtDeclaration.Text);
            //        RefreshValues();
            //    }
            //    catch (BadSyntaxException ex)
            //    {
            //        SetError(ex.Message);
            //        return false;
            //    }
            //}
			return true;
		}

		private void SetError(string message)
		{
			if (MonoHelper.IsRunningOnMono && MonoHelper.IsOlderVersionThan("2.4"))
				return;

			errorProvider.SetError(this, message);
		}

		private void SelectPrevious()
		{
			if (ValidateDeclarationLine())
			{
				shape.SelectPrevious();
			}
		}

		private void SelectNext()
		{
			if (ValidateDeclarationLine())
			{
				shape.SelectNext();
			}
		}

		private void MoveUp()
		{
			if (ValidateDeclarationLine())
			{
				shape.MoveUp();
			}
		}

		private void MoveDown()
		{
			if (ValidateDeclarationLine())
			{
				shape.MoveDown();
			}
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
					if (e.Modifiers == Keys.Control || e.Modifiers == Keys.Shift)
						OpenNewMemberDropDown();
					else
						ValidateDeclarationLine();
					e.Handled = true;
					break;

				case Keys.Escape:
					needValidation = false;
					shape.HideEditor();
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

			if (e.Modifiers == (Keys.Control | Keys.Shift))
			{
				switch (e.KeyCode)
				{
					case Keys.A:
						AddNewMember();
						break;

					case Keys.S:
						AddNewMember(MemberType.State);
						break;

					case Keys.P:
						AddNewMember(MemberType.CP);
						break; 
                         
				}
			}
		}

		private void OpenNewMemberDropDown()
		{
			toolNewMember.ShowDropDown();
			
			switch (NewMemberType)
			{
				case MemberType.Name:
					toolNewNameMember.Select();
					break;

				case MemberType.State:
					toolNewStateMember.Select();
					break;

				case MemberType.CP:
					toolNewCPMember.Select();
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

		private void toolHider_CheckedChanged(object sender, EventArgs e)
		{
			bool checkedState = toolHider.Checked;

			if (shape.ActiveMember != null && ValidateDeclarationLine())
			{
				try
				{
					//shape.ActiveMember.IsHider = checkedState;
					RefreshValues();
				}
				catch (BadSyntaxException ex)
				{
					RefreshValues();
					SetError(ex.Message);
				}
			}
		}

		private void AddNewMember()
		{
			AddNewMember(NewMemberType);
		}

		private void AddNewMember(MemberType type)
		{
			if (!ValidateDeclarationLine())
				return;

			NewMemberType = type;
			//shape.InsertNewMember(type);??
			txtDeclaration.SelectionStart = 0;
		}

		private void toolNewMember_ButtonClick(object sender, EventArgs e)
		{
			AddNewMember();
		}

		private void toolNewNameMember_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Name);
		}

		private void toolNewStateMember_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.State);
		}


        private void toolNewCPMember_Click(object sender, EventArgs e)
        {
            AddNewMember(MemberType.CP);
        }


		private void toolMoveUp_Click(object sender, EventArgs e)
		{
			MoveUp();
		}

		private void toolMoveDown_Click(object sender, EventArgs e)
		{
			MoveDown();
		}
	}
}
