

using System;
using System.Windows.Forms;
using KRLab.Translations;

namespace KRLab.GUI.Dialogs
{
	public partial class AboutDialog : Form
	{
		public AboutDialog()
		{
			InitializeComponent();
		}

		private void UpdateTexts()
		{
			this.Text = Strings.AboutKRLab;
			lblTitle.Text = Program.GetVersionString();
			lblCopyright.Text = "Copyright (C) 2019-2020 " + "DFL";
			lblStatus.Text = string.Format(Strings.BetaVersion);
			lnkEmail.Text = Strings.SendEmail;
			lnkHomepage.Text = Strings.VisitHomepage;
			btnClose.Text = Strings.ButtonClose;

			lnkHomepage.Links.Clear();
			lnkEmail.Links.Clear();
			lnkHomepage.Links.Add(0, lnkHomepage.Text.Length, Properties.Resources.WebAddress);
			lnkEmail.Links.Add(0, lnkEmail.Text.Length,
				"mailto:" + Properties.Resources.MailAddress + "?subject=KRLab");
			//lblTranslator.Text = Strings.Translator;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateTexts();
		}

		private void lnkEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string target = e.Link.LinkData as string;

			if (target != null)
			{
				try
				{
					System.Diagnostics.Process.Start(target);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						string.Format("{0}\n{1}: {2}", Strings.CommandFailed,
							Strings.ErrorsReason, ex.Message),
						Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void lnkHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string target = e.Link.LinkData as string;

			if (target != null)
			{
				try
				{
					System.Diagnostics.Process.Start(target);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						string.Format("{0}\n{1}: {2}", Strings.CommandFailed,
							Strings.ErrorsReason, ex.Message),
						Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}