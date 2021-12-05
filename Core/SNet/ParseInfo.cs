using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    public abstract class ParseInfo
    {
        protected SemanticNet _net;

        public SemanticNet Net
        {
            get { return _net; }
        }

        public ParseInfo(SemanticNet net)
        {
            _net = net;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qas">item1是问题，item2是答案关键字</param>
        public abstract void ProduceQAs(out List<System.Tuple<string, string[]>> qas);
    }
}
