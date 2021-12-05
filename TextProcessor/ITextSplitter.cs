using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSText
{
    /// <summary>
    /// Defines a service for splitting a text input into multiple strings.
    /// </summary>
    public interface ITextSplitter
    {
        /// <summary>
        /// Splits an input text into multiple sentences for processing.
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        IEnumerable<string> Split(string inputText);
    }
}
