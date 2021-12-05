using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DomainModule
{
    public class Variable
    {
        //是否可变
        protected bool _variable;
        //符号表示，v0,v1等
        protected string _symbol;
        public string Symbol
        { get { return _symbol; } }
        public bool IsVariable
        { get { return _variable; } }

        public void Set(string name, string symbol)
        {
        }
    }
}
