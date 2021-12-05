using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class StrValue:Value
    {
        private string _val;
        public StrValue(StrValue v):base(new BaseValue())
        {
            _val = new string(v._val.ToCharArray());
        }
        public StrValue(string s)
        {
            _val = s;
        }
        public StrValue(char[] s)
        {
            _val = new string(s);
        }
        public override void print(Binding b = null)
        {
            Console.Write(_val+' ');
        }
        public override string get_string() { return _val; }
        public override bool is_true() { return _val.Length != 0; }
        public override bool is_equal(Value v)
        {
            return v.strEq(this);
        }
        public override ValType type()  { return ValType.VAL_STRING;}
        public static Value operator+(StrValue v1,StrValue v2)
        {
            return new StrValue(v1._val + v2._val);
        }
        public static bool operator!(StrValue v) { return !v.is_true(); }


        public override bool strEq(Value v)
        {
            return ((StrValue)v)._val == this._val;
        }
    }
}
