 

using System;
using System.Reflection;

namespace KRLab.DiagramEditor
{
	public static class MonoHelper
	{
		static bool isMono;
		static string version;

		static MonoHelper()
		{
			Type monoRuntime = Type.GetType("Mono.Runtime");

			if (monoRuntime != null)
			{
				isMono = true;
				MethodInfo method = monoRuntime.GetMethod("GetDisplayName",
					BindingFlags.NonPublic | BindingFlags.Static);
				
				if (method != null)
					version = method.Invoke(null, null) as string;
				else
					version = "Unknown version";
			}
			else
			{
				isMono = false;
				version = string.Empty;
			}
		}

		public static bool IsRunningOnMono
		{
			get { return isMono; }
		}

		public static string Version
		{
			get { return version; }
		}

		public static bool IsOlderVersionThan(string version)
		{
			version = "Mono " + version;
			return (Version.CompareTo(version) < 0);
		}
	}
}
