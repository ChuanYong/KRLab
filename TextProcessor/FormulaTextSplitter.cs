using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSText
{ 
    public class FormulaTextSplitter : ITextSplitter
    {
        public static string[] SplitMarkers = new[]
        {
            "-",
            "+",
            "=",
        };

        public IEnumerable<string> Split(string inputText)
            => inputText?.Split(SplitMarkers, StringSplitOptions.RemoveEmptyEntries);
    }
}
