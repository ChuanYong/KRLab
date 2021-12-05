using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.StudentModule
{
    public class Performance
    {
        public static readonly string Excellent= "优";
        public static readonly string VeryGood = "良好";
        public static readonly string Good = "中";
        public static readonly string Average = "及格";
        public static readonly string Poor = "不及格";

        protected readonly string _name;
        public string Name
        { get { return _name; } }

        public Performance(string name)
        {
            _name = name;
        }
    }
}
