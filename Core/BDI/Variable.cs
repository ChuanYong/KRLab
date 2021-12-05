using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class Variable:Expression
    {
        private SymbolTable _st;
        private int _id;

        private void _init(SymbolTable st,string s)
        {
            _st = st;
            if((_id=_st.get_id(s))<0)
            {
                _id = _st.add(new Symbol(s)).id;
            }
        }

        public Variable(SymbolTable st,string s)
        {
            _init(st, s);
        }
        public Variable(Binding b,string s)
        {
            _init(b.get_symtab(), s);
        }
        public int get_id()
        {
            return _id;
        }
        public override string get_name()
        {
            return _st.lookup(get_id()).name;
        }
        public override ExpType get_type()
        {
            return ExpType.EXP_VARIABLE;
        }
        public override bool is_variable()
        {
            return true;
        }
        public override Value eval(Binding b=null)
        {
            return b != null ? b.get_value(this) : Value.Undefined;
        }
        public override Variable get_variable()
        {
            return this;
        }


    }
}
