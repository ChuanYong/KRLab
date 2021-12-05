using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.TutorModule
{
    public class Suitability
    {
        public static readonly int VeryHigh = 4;
        public static readonly int High = 3;
        public static readonly int Medium = 2;
        public static readonly int Low = 1;
        public static readonly int VeryLow = 0;

        protected readonly int _level;
        public int Level
        { get { return _level; } }

        public Suitability(int level)
        {
            _level = level;
        }
    }
}
