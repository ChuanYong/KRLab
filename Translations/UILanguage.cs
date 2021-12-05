
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace KRLab.Translations
{
	public class UILanguage
	{
		static List<UILanguage> availableCultures;

		static UILanguage()
		{
			// Load localization resources
			Assembly assembly = Assembly.GetExecutingAssembly();
			string resourceDir = Path.GetDirectoryName(assembly.Location);

			// Search for localized cultures
			try
			{
				DirectoryInfo resource = new DirectoryInfo(resourceDir);
				DirectoryInfo[] directories = resource.GetDirectories("*",
					SearchOption.TopDirectoryOnly);
				availableCultures = new List<UILanguage>(directories.Length + 2);

				foreach (DirectoryInfo directory in directories)
				{
					if (directory.Name != "Plugins" && directory.Name != "Templates")
					{
						string cultureName = directory.Name;
						UILanguage language = CreateUILanguage(cultureName);
						if (language != null)
							availableCultures.Add(language);
					}
				}
			}
			catch
			{
				availableCultures = new List<UILanguage>(2);
			}

			availableCultures.Add(CreateDefaultUILanguage());
			availableCultures.Add(CreateUILanguage("en"));
			availableCultures.Sort(delegate(UILanguage c1, UILanguage c2)
			{
				return c1.Name.CompareTo(c2.Name);
			});
		}

		CultureInfo culture;
		bool isDefault;

		private UILanguage()
		{
		}

		private UILanguage(CultureInfo culture)
		{
			this.culture = culture;
			this.isDefault = false;
		}

		public string Name
		{
			get
			{
				if (IsDefault)
					return "[Default]";
				else
					return culture.EnglishName;
			}
		}

		public string ShortName
		{
			get
			{
				if (IsDefault)
					return "default";
				else
					return culture.Name;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				if (IsDefault)
					return CultureInfo.CurrentUICulture;
				else
					return culture;
			}
		}

		public bool IsDefault
		{
			get { return isDefault; }
		}

		public static IEnumerable<UILanguage> AvalilableCultures
		{
			get { return availableCultures; }
		}

		public static UILanguage CreateDefaultUILanguage()
		{
			UILanguage language = new UILanguage();
			language.isDefault = true;

			return language;
		}

		public static UILanguage CreateUILanguage(string cultureName)
		{
			if (cultureName == "default")
				return CreateDefaultUILanguage();

			try
			{
				CultureInfo culture = new CultureInfo(cultureName);
				return new UILanguage(culture);
			}
			catch (ArgumentException)
			{
				return null;
			}
		}

		public override bool Equals(object obj)
		{
			UILanguage other = obj as UILanguage;

			if (other == null)
				return false;
			else if (this.IsDefault && other.IsDefault)
				return true;
			else if (!this.IsDefault && !other.IsDefault)
				return (this.culture.Equals(other.culture));
			else
				return false;
		}

		public override int GetHashCode()
		{
			if (IsDefault)
				return 0;
			else
				return culture.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}
	}
}