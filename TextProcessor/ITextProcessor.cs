using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSText
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITextProcessor
    {
        /// <summary>
        /// Processes an input text using the registered command processors.
        /// </summary>
        /// <param name="inputText">The user input text.</param>
        /// <param name="context">The request context information.</param>
        /// <param name="cancellationToken">The cancellation token for the execution task.</param>
        /// <returns></returns>       
        Task ProcessAsync(string inputText);
    }
}
