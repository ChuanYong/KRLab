

using System.IO;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using KRLab.Core; 
using KRLab.Translations;

namespace KRLab.GUI
{
	public sealed partial class Settings
	{
		const int MaxRecentFileCount = 5; 

		public void AddRecentFile(string recentFile)
		{
			if (!File.Exists(recentFile))
				return;

			int index = RecentFiles.IndexOf(recentFile);

			if (index < 0)
			{
				if (RecentFiles.Count < MaxRecentFileCount)
					RecentFiles.Add(string.Empty);

				for (int i = RecentFiles.Count - 2; i >= 0; i--)
					RecentFiles[i + 1] = RecentFiles[i];
				RecentFiles[0] = recentFile;
			}
			else if (index > 0)
			{
				string temp = RecentFiles[index];
				for (int i = index; i > 0; i--)
					RecentFiles[i] = RecentFiles[i - 1];
				RecentFiles[0] = temp;
			}
		}
	}
}