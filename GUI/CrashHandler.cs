

using System;
using System.IO;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.DiagramEditor;
using KRLab.Translations;

namespace KRLab.GUI
{
	internal static class CrashHandler
	{
		public static void CreateGlobalErrorHandler()
		{
#if !DEBUG
			AppDomain.CurrentDomain.UnhandledException += 
				new UnhandledExceptionEventHandler(AppDomain_UnhandledException);
#endif
		}

		private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
			{
				string crashDir = Path.Combine(Program.AppDataDirectory, "crash");
				Directory.CreateDirectory(crashDir);

				CreateBackups(crashDir);
				Exception ex = (Exception) e.ExceptionObject;
				CreateCrashLog(crashDir, ex);

				MessageBox.Show(
					Strings.ProgramTerminates, Strings.CriticalError,
					MessageBoxButtons.OK, MessageBoxIcon.Error);

				System.Diagnostics.Process.Start(crashDir);
				System.Diagnostics.Process.GetCurrentProcess().Kill();
				// Goodbye!
			}
		}

		private static void CreateBackups(string directory)
		{
			int untitledCount = 0;			
			foreach (Project project in Workspace.Default.Projects)
			{
				if (project.IsDirty){
				try
				{
					string fileName = project.FileName;
					if (project.IsUntitled)
					{
						untitledCount++;
						fileName = project.Name + untitledCount + ".fvc";
					}
					string filePath = Path.Combine(directory, fileName);
					
					project.Save(filePath);
				}
				catch
				{
				}
				}
			}
		}

		private static void CreateCrashLog(string directory, Exception exception)
		{
			StreamWriter writer = null;

			try
			{
				string filePath = Path.Combine(directory, "crash.log");
				writer = new StreamWriter(filePath);

				writer.WriteLine(string.Format(
					Strings.SendLogFile, Properties.Resources.MailAddress));
				writer.WriteLine();
				writer.WriteLine("Version: {0}", Program.GetVersionString());
				writer.WriteLine("Mono: {0}", MonoHelper.IsRunningOnMono ? "yes" : "no");
				if (MonoHelper.IsRunningOnMono)
					writer.WriteLine("Mono version: {0}", MonoHelper.Version);
				writer.WriteLine("OS: {0}", Environment.OSVersion.VersionString);

				writer.WriteLine();
				writer.WriteLine(exception.Message);
				Exception innerException = exception.InnerException;
				while (innerException != null)
				{
					writer.WriteLine(innerException.Message);
					innerException = innerException.InnerException;
				}

				writer.WriteLine();
				writer.WriteLine(exception.StackTrace);
			}
			catch
			{
				// Do nothing
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}
	}
}