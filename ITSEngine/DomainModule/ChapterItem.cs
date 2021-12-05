using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilities;

namespace ITS.DomainModule
{
    [Serializable]
    public class ChapterItem
    {
        public string Index
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public ChapterItem(string index, string name)
        {
            Index = index;
            Name = name;
        }

        public string Format
        {
            get { return "<" + Index + " " + Name + ">"; }
        }

        public override string ToString()
        {
            return Index + "-" + Name;
        }

        public int NumberIndex
        {
            get
            {
                string result = System.Text.RegularExpressions.Regex.Replace(Index, @"[^0-9]+", "");
                return int.Parse(result);
            }
        }
    }

    [Serializable]
    public class SectionItem
    {
        public string Index
        { get; set; }
        public string Name
        { get; set; }

        public SectionItem(string index,string name)
        {
            Index = index;
            Name = name;
        }

        public string Format
        {
            get { return "<" + Index + " " + Name + ">"; }
        }

        public override string ToString()
        {
            return Index + "-" + Name;
        }
        public int NumberIndex
        {
            get
            {
                string result = System.Text.RegularExpressions.Regex.Replace(Index, @"[^0-9]+", "");
                return int.Parse(result);
            }
        }
    }
}
