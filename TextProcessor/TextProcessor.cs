using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ITSText
{ 
    public class TextProcessor : ITextProcessor
    {
        static Hashtable ht = new Hashtable();
        static char[] s1 = new char[] { 'A', 'B', 'Γ', 'Δ', 'E', 'Z', 'H', 'Θ', 'Ι', 'K', 'Λ', 'M', 'N', 'Ξ', 'O', 'Π', 'P', 'Σ', 'T', 'Y', 'Φ', 'X', 'Ψ', 'Ω' };
        static char[] s2 = new char[] { 'α', 'β', 'γ', 'δ', 'ε', 'ζ', 'η', 'θ', 'ι', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο', 'π', 'ρ', 'σ', 'τ', 'υ', 'φ', 'χ', 'ψ', 'ω' };

        static TextProcessor()
        {
            for (int i = 0; i < s1.Length; i++)
            {
                ht.Add(s1[i], s2[i]);
            }
        }

        public TextProcessor()
        {

        }

        protected static ITextSplitter _textSplitter=new PunctuationTextSplitter();
        protected static ITextSplitter _formulaSplitter = new FormulaTextSplitter();
        protected static ITextSplitter TextSplitter
        {
            get { return _textSplitter; }
        } 

        public async Task ProcessAsync(string inputText)
        {
            if (inputText == null) throw new ArgumentNullException(nameof(inputText));
            if (string.IsNullOrWhiteSpace(inputText))
            {
                throw new ArgumentException("The input string must have a value", nameof(inputText));
            } 

            foreach (var splittedInputText in SplitInput(inputText))
            {
                await Task.Run(() =>
                {
                    ParseInput(splittedInputText);
                }); 
            }
        }

        private static IEnumerable<string> SplitInput(string inputText)
        {
            if (TextSplitter != null) return TextSplitter.Split(inputText);
            return new[] { inputText };
        }

        private static IEnumerable<string> SplitFormulaInput(string inputText)
        {
            if (_formulaSplitter != null) return _formulaSplitter.Split(inputText);
            return new[] { inputText };
        }

        public static List<string> Split(string str)
        {
            return SplitInput(str).ToList();
        }

        public static List<string> SplitFormula(string str)
        {
            return SplitFormulaInput(str).ToList();
        }

        public static string ToRome(string s)
        {
            char[] str = s.ToCharArray();
            string h = null;
            for (int i = 0; i < str.Length; i++)
            {
                if (ht.ContainsKey(str[i]))
                {
                    str[i] = Convert.ToChar(ht[str[i]]);
                }
                h += str[i];
            }
            return h;
        }

        private static void ParseInput(string inputStr)
        {

        }

        /// <summary>
        /// 将句子中包含words内容的替换为“_”
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="words"></param>
        public static string ReplaceWithUnderLine(string sentence, List<string> words)
        {
            foreach (var str in words)
            {
                if (sentence.Contains(str))
                {
                    sentence = sentence.Replace(str, "_______________");
                }
            }
            return sentence;
        }
        public static string ReplaceWithUnderLine(string sentence,string word)
        {
            if(sentence.Contains(word))
            {
                sentence = sentence.Replace(word, "_______________");
            }
            return sentence;
        }

        public static string ReplaceWords(string sentence,Dictionary<string,double> wordDict)
        {
            foreach (var str in wordDict.Keys)
            {
                if (sentence.Contains(str))
                {
                    sentence = sentence.Replace(str, wordDict[str].ToString());
                }
            }
            return sentence;
        }


        public static string StrBetween(string[] sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            foreach (var str in sourse)
            {
                startindex = str.IndexOf(startstr);
                if (startindex == -1)
                    continue;

                string tmpstr = str.Substring(startindex + startstr.Length);
                endindex = tmpstr.IndexOf(endstr);
                if (endindex == -1)
                    continue;
                result = tmpstr.Remove(endindex);
                return result;
            }


            return string.Empty;
        }


        /// <summary>
        /// 比较字符串，如果str1="str0&str1&str2"，则str2包含str1中所有&分割的，返回true
        /// 如果str1="str0|str1|str2"，则str2只要包含其中之一，返回true。
        /// 如果str1没有&或|分隔符，则str2必须完全与str1相同，返回true。
        /// </summary>
        /// <param name="str2"></param>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static bool Comp(string str2, string str1)
        {
            string[] strList;
            string signal;
            if (str1.Contains("&"))
            {
                strList = str1.Split(new char[] { '&' });
                signal = "&";
            }
            else if (str1.Contains("|"))
            {
                strList = str1.Split(new char[] { '|' });
                signal = "|";
            }
            else
            {
                strList = new string[] { str1 };
                signal = "";
            }

            if (signal == "&")
            {
                foreach (var x in strList)
                {
                    if (!str2.Contains(x))
                        return false;
                }
                return true;
            }
            else if (signal == "|")
            {
                foreach (var x in strList)
                {
                    if (str2.Contains(x))
                        return true;
                }
                return false;
            }
            else
            {
                if (str1.Contains(str2) || str2.Contains(str1))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 获取字符串中的数字，并转换为整型。注意，这种
        /// 方法只能处理字符串中只有一组数据，如果有多组则出错。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetNumber(string str)
        {
            string result = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");
            return int.Parse(result);
        }

    }
}
