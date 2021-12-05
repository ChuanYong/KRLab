using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class BaseValue
    {
        public BaseValue(int i= 0) { }
    }
    public class Value:Expression
    {
        private Value _rep;
        private int _referenceCount;

        public static Value Undefined=new Value();
        public static Value True=new Value(1);
        public static Value False = new Value(0);

        public enum ValType
        {
            VAL_VOID,
            VAL_INT,
            VAL_REAL,
            VAL_STRING,
            VAL_UNDEF
        };

        public virtual int deReference()
        {
            return --_referenceCount;
        }
        public virtual int reference()
        {
            return ++_referenceCount;
        }

        public Value(BaseValue bv=null) 
        {
            _rep = null;
            _referenceCount = 1;
        }

        public Value(double f)
        {
            _rep = new RealValue(f); 
        }
        public Value(string s)
        {
            _rep = new StrValue(s);
        }

        public Value(Value x)
        {
            (_rep=x._rep).reference();
        }
        public void redefine(Value x)
        {
            if (_rep != null && _rep.deReference() <= 0)
                _rep = x;
        }
        public override Value eval(Binding b=null)
        {
            return this;
        }
        public virtual void print(Binding b=null)
        {
            _rep.print(b);
        }
        public override string get_name()
        {
            return "Value";
        }
        public override ExpType get_type()
        {
            return ExpType.EXP_VALUE;
        }
        public virtual ValType type()
        {
            return _rep.type();
        }
        public virtual bool is_true() { return _rep.is_true(); }
        public override bool is_variable()
        {
            return false;
        }
        public virtual bool is_defined()
        {
            return !(_rep.type() == ValType.VAL_VOID && _rep.get_void() == null);
        }

        public virtual IntPtr get_void() { return _rep.get_void(); }
        public virtual double get_real() { return _rep.get_real(); }
        public virtual int get_int() { return _rep.get_int(); }
        public virtual string get_string() { return _rep.get_string(); }

        public virtual bool is_equal(Value v)
        {
            return _rep.is_equal(v);
        }
 

        public virtual bool strEq(Value v)
        {
            return _rep.strEq(v);
        }
    }
}
