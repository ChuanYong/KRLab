using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class RealValue:Value
    {
        private double _val;
        public RealValue(RealValue v):base()
        {
            _val = v._val;
        }
        public RealValue(double v):base()
        {
            _val = v;
        }
        public override void print(Binding b=null)
        {
            Console.Write(_val);
            Console.Write(" ");
        }
        public override ValType type() { return ValType.VAL_REAL; }
        public override bool is_true() { return _val != 0.0; }
        public override IntPtr get_void() { return new IntPtr();  }
        public override double get_real() { return _val; }
        public override int get_int() { return (int)_val;}
        public override string get_string() { return ""; }
        public static Value operator- (RealValue v) { return new Value(-v._val); }
        public static Value operator+(RealValue v1,RealValue v2)
        {
            return new RealValue(v1._val+v2._val);
        }
        public static Value operator-(RealValue v1,RealValue v2)
        {
            return new RealValue(v1._val - v2._val);
        }
        public static Value operator*(RealValue v1,RealValue v2)
        {
            return new RealValue(v1._val * v2._val);
        }
        public static Value operator/(RealValue v1,RealValue v2)
        {
            return new RealValue(v1._val / v2._val);
        }

    }
}
