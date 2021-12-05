 

using System;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using System.Windows.Forms;
using KRLab.Translations;

namespace KRLab.GUI
{
	internal static class Program
	{
		public static readonly Version CurrentVersion =
			Assembly.GetExecutingAssembly().GetName().Version;
		public static readonly string AppDataDirectory =
			Path.Combine(Environment.GetFolderPath(
			Environment.SpecialFolder.LocalApplicationData), "KRLab");

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			CrashHandler.CreateGlobalErrorHandler();
			UpdateSettings();

			// Set the user interface language
			UILanguage language = UILanguage.CreateUILanguage(Settings.Default.UILanguage);
			if (language != null)
				Strings.Culture = language.Culture;

			// Some GUI settings
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			ToolStripManager.VisualStylesEnabled = false;

			// Launch the application
			LoadFiles(args);
			Application.Run(new MainForm());

			// Save application settings
			DiagramEditor.Settings.Default.Save();
			Settings.Default.Save();
		}

		private static void UpdateSettings()
		{
			if (Settings.Default.CallUpgrade)
			{
				Settings.Default.Upgrade();
				Settings.Default.CallUpgrade = false;
			}

			if (Settings.Default.OpenedProjects == null)
				Settings.Default.OpenedProjects = new StringCollection();
			if (Settings.Default.RecentFiles == null)
				Settings.Default.RecentFiles = new StringCollection();
		}

		public static string GetVersionString()
		{
			if (CurrentVersion.Minor == 0)
			{
				return string.Format("KRLab {0}.0", CurrentVersion.Major);
			}
			else
			{
				return string.Format("KRLab {0}.{1:00}",
					CurrentVersion.Major, CurrentVersion.Minor);
			}
		}

		private static void LoadFiles(string[] args)
		{
			if (args.Length >= 1)
			{
				foreach (string filePath in args)
				{
					Workspace.Default.OpenProject(filePath);
				}
			}
			else if (Settings.Default.RememberOpenProjects)
			{ 
				Workspace.Default.Load();
			}
		}
	}
}