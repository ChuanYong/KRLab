using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.MaterialModule
{

    /// <summary>
    /// 提问的答案是文本
    /// </summary>
    public class TextAnswer : Answer
    {
        protected string[] _answer;

        public TextAnswer(params string[] strs)
        {
            _answer = strs;
        }

        public override object Content
        {
            get { return _answer; }
        }
    }
}
