

using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.Translations;
using KRLab.DiagramEditor.NetworkDiagram.Dialogs;

namespace KRLab.DiagramEditor.NetworkDiagram.Editors
{
	public partial class CompositeNodeEditor : TypeEditor
	{
		CompositeNodeShape shape = null;
		bool needValidation = false;

		public CompositeNodeEditor()
		{
			InitializeComponent();
			toolStrip.Renderer = ToolStripSimplifiedRenderer.Default;
			UpdateTexts();

			if (MonoHelper.IsRunningOnMono)
				toolNewMember.Alignment = ToolStripItemAlignment.Left;
		}

		internal override void Init(DiagramElement element)
		{
            shape = (CompositeNodeShape)element;
			//RefreshToolAvailability();
			RefreshValues();
		}

        //private void RefreshToolAvailability()
        //{
        //    toolOverrideList.Visible = shape.Node is SingleInharitanceType;
			
        //    IInterfaceImplementer implementer = shape.Node as IInterfaceImplementer;
        //    if (implementer != null)
        //    {
        //        toolImplementList.Visible = true;
        //        toolImplementList.Enabled = implementer.ImplementsInterface;
        //    }
        //    else
        //    {
        //        toolImplementList.Visible = false;
        //    }
        //}

		private void UpdateTexts()
		{
			toolNewNameMember.Text = Strings.NewField;
			toolNewStateMember.Text = Strings.NewMethod; 
			toolNewCPMember.Text = Strings.NewProperty; 
			toolSortByKind.Text = Strings.SortByKind; 
			toolSortByName.Text = Strings.SortByName;
		}

		private void RefreshValues()
		{
			CompositeNode type = (CompositeNode)shape.Node; 
			SuspendLayout();

			int cursorPosition = txtName.SelectionStart;
			txtName.Text = type.Name;
			txtName.SelectionStart = cursorPosition;

			SetError(null);
			needValidation = false;

			bool hasMember = (type.MemberCount > 0); 
			toolSortByKind.Enabled = hasMember;
			toolSortByName.Enabled = hasMember;

			RefreshNewMembers();
			ResumeLayout();
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

		public override void ValidateData()
		{
			ValidateName();
			SetError(null);
		}

		private bool ValidateName()
		{
			if (needValidation)
			{
				try
				{
					shape.Node.Name = txtName.Text;
					RefreshValues();
				}
				catch (BadSyntaxException ex)
				{
					SetError(ex.Message);
					return false;
				}
			}
			return true;
		}

		private void SetError(string message)
		{
			if (MonoHelper.IsRunningOnMono && MonoHelper.IsOlderVersionThan("2.4"))
				return;

			errorProvider.SetError(this, message);
		}  

		private void txtName_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					if (e.Modifiers == Keys.Control || e.Modifiers == Keys.Shift)
						OpenNewMemberDropDown();
					else
						ValidateName();
					e.Handled = true;
					break;

				case Keys.Escape:
					needValidation = false;
					shape.HideEditor();
					e.Handled = true;
					break;

				case Keys.Down:
					shape.ActiveMemberIndex = 0;
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

                    case Keys.F:
                        AddNewMember(MemberType.Name);
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
			}
		}

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			needValidation = true;
		}

		private void txtName_Validating(object sender, CancelEventArgs e)
		{
			ValidateName();
		}

		private void AddNewMember()
		{
			AddNewMember(NewMemberType);
		}

		private void AddNewMember(MemberType type)
		{
			if (!ValidateName())
				return;

			NewMemberType = type;
			switch (type)
			{
				case MemberType.State:
                    //shape.Node.AddField();???
                    //shape.ActiveMemberIndex = shape.Node.StateCount - 1;
					break;

				case MemberType.CP:
                    //shape.Node.AddMethod();???
                    //shape.ActiveMemberIndex = shape.Node.MemberCount - 1;
					break;
			}
			txtName.SelectionStart = 0;
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

		private void toolSortByName_Click(object sender, EventArgs e)
		{
			//((CompositeNode)shape.Node).SortMembers(SortingMode.ByName);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			txtName.SelectionStart = 0;
		}
	}
}
