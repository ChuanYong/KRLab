using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents; 
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions; 

namespace FVCClient
{
	public static class DocumentHelper
    {

		/// <summary>
		/// Parses a full command line and returns a Command object
		/// containing the command name as well as the different arguments.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static Answer ParseAnswerLine(string line) {
			string str = "";
			List<string> args = new List<string>();

			Match m = Regex.Match(line.Trim() + " ", @"^(.+?)(?:\s+|$)(.*)");
			if (m.Success) {
				str = m.Groups[1].Value.Trim();
				string argsLine = m.Groups[2].Value.Trim();
				Match m2 = Regex.Match(argsLine + " ", @"(?<!\\)"".*?(?<!\\)""|[\S]+");
				while (m2.Success) {
					string arg = Regex.Replace(m2.Value.Trim(), @"^""(.*?)""$", "$1");
					args.Add(arg);
					m2 = m2.NextMatch();
				}
			}

			return new Answer(line, str, args.ToArray());
		}


        private static MethodInfo findMethod = null;
        [Flags]
        public enum FindFlags
        {
            None = 0,
            MatchCase = 1,
            FindInReverse = 2,
            FindWholeWordsOnly = 4,
            MatchDiacritics = 8,
            MatchKashida = 16,
            MatchAlefHamza = 32,
        }

        public static TextRange FindText(TextPointer startPosition, TextPointer endPosition, String input, FindFlags flags, CultureInfo cultureInfo)
        {
            TextRange textRange = null;
            if (startPosition.CompareTo(endPosition) < 0)
            {
                try
                {
                    if (findMethod == null)
                    {
                        findMethod = typeof(FrameworkElement).Assembly.GetType("System.Windows.Documents.TextFindEngine").
                               GetMethod("Find", BindingFlags.Static | BindingFlags.Public);
                    }

                    textRange = findMethod.Invoke(null,
                       new Object[] { startPosition,
                                    endPosition,
                                    input,
                                    flags,
                                    CultureInfo.CurrentCulture
                       }) as TextRange;
                }
                catch (ApplicationException)
                {
                    textRange = null;
                }
            }

            return textRange;
        }

        public static IEnumerable<TextRange> FindText(FlowDocument document, String input, FindFlags flags, CultureInfo cultureInfo)
        {
            TextPointer start = document.ContentStart;
            TextPointer end = document.ContentEnd;
            TextRange last = null;
            var textRange = new List<TextRange>();

            try
            {
                if (findMethod == null)
                {
                    findMethod = typeof(FrameworkElement).Assembly.GetType("System.Windows.Documents.TextFindEngine").
                           GetMethod("Find", BindingFlags.Static | BindingFlags.Public);
                }
            }
            catch (ApplicationException)
            {
                last = null;
            }

            while (findMethod != null && start.CompareTo(end) < 0)
            {
                try
                {
                    last = findMethod.Invoke(null,
                                            new Object[] { start,
                                                      end,
                                                      input,
                                                      flags,
                                                      CultureInfo.CurrentCulture
                                         }) as TextRange;
                }
                catch (ApplicationException)
                {
                    last = null;
                }

                if (last == null)
                    yield break;
                else
                    yield return last;
                start = last.End;
            }
        }



        public static void AddImageToTextBox(Image img, RichTextBox textBox)
        {
            Paragraph parag = new Paragraph();
            parag.Inlines.Add(img);
            textBox.Document.Blocks.Add(parag);
        }


        public static void InsertATable(RichTextBox textBox)
        {
            var trg = new TableRowGroup();

            for (int r = 0; r < 5; r++)
            {
                var tr = new TableRow();

                for (int c = 0; c < 5; c++)
                {
                    var sf = string.Format("{0}x{1}", r, c);
                    var run = new Run(sf);
                    var par = new Paragraph(run);

                    var td = new TableCell();
                    td.Blocks.Add(par);

                    tr.Cells.Add(td);
                }
                trg.Rows.Add(tr);
            }

            var t = new Table();

            t.RowGroups.Add(trg);

            textBox.Selection.Text = "";
            //var pt = _textBox.Selection.Start.InsertParagraphBreak();
            //TextBox.Document.Blocks.InsertBefore(pt.Paragraph, t);

            var tp = textBox.CaretPosition.InsertParagraphBreak();
            tp.Paragraph.SiblingBlocks.InsertBefore(tp.Paragraph, t);
        }


        public static Image ExpressionToImage(byte[] pngBytes)
        {
            MemoryStream strm = new MemoryStream();
            strm.Write(pngBytes, 0, pngBytes.Length);
            strm.Position = 0;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = strm;
            bi.EndInit();

            Image img = new Image();
            img.Width = bi.Width;
            img.Height = bi.Height;
            img.Source = bi;

            return img;
        }
 

        // --------------------------------------------------------------------
        // GENERAL UTILITIES
        // --------------------------------------------------------------------

        public static string GetShortestString(string[] strs)
        {
            string ret = strs[0];
            foreach (string str in strs)
                ret = str.Length < ret.Length ? str : ret;
            return ret;
        }

        public static string GetCommonPrefix(string[] strs)
        {
            string shortestStr = GetShortestString(strs);
            for (int i = 0; i < shortestStr.Length; i++)
                foreach (string str in strs)
                    if (char.ToLower(str[i]) != char.ToLower(shortestStr[i]))
                        return shortestStr.Substring(0, i);
            return shortestStr;
        }

        public static string[] GetFileList(string dir, bool useAntislash)
        {
            if (!Directory.Exists(dir))
                return new string[0];
            string[] dirs = Directory.GetDirectories(dir);
            string[] files = Directory.GetFiles(dir);

            for (int i = 0; i < dirs.Length; i++)
                dirs[i] = System.IO.Path.GetFileName(dirs[i]) + (useAntislash ? "\\" : "/");
            for (int i = 0; i < files.Length; i++)
                files[i] = System.IO.Path.GetFileName(files[i]);

            List<string> ret = new List<string>();
            ret.AddRange(dirs);
            ret.AddRange(files);
            return ret.ToArray();
        }

        private static readonly Regex _CHUNKER = new Regex("[^ \"']+|([\"'])[^\\1]*?\\1[^ \"']*|([\"'])[^\\1]*$| $", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        public static string GetLastWord(this string cmdline, bool trimQuotes = true)
        {
            if (string.IsNullOrWhiteSpace(cmdline))
            {
                return string.Empty;
            }

            string lastWord = null;
            MatchCollection words = _CHUNKER.Matches(cmdline);
            if (words.Count >= 1)
            {
                Match lw = words[words.Count - 1];
                if (trimQuotes)
                {
                    lastWord = lw.Value.Trim();
                    lastWord = lastWord.Replace("\"", "");
                    //if (lastWord.StartsWith("\"", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //   lastWord = lastWord.Substring(1);
                    //}
                    //if (lastWord.EndsWith("\"", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //   lastWord = lastWord.Substring(0, lastWord.Length - 1);
                    //}
                }
                else
                {
                    lastWord = lw.Value.TrimEnd('\r', '\n');
                }
            }
            return lastWord;
        }

        public static int LineCount(this string text)
        {
            var lineends = new char[] { '\r', '\n' };
            int index = 0, count = 0;
            while ((index = 1 + text.IndexOfAny(lineends, index)) > 0)
            {
                count++;
                index += (text[index] == lineends[1]) ? 1 : 0;
            }
            return count;
        }

    }
}
