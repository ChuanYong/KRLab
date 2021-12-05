using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class NetException:System.ApplicationException
    {
        private string _error; 

        public string Error
        {
            get { return _error; }
        }

        public NetException(string msg):base(msg)
        {
            this._error = msg;
        }
    }
}
