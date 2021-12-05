using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    //Expression can be used in various places for runtime
    //evaluation.values, variables and functions are expressions.
    public abstract class Expression
    {
        public enum ExpType
        {
            EXP_VALUE,//常量
            EXP_VARIABLE,//变量
            EXP_FUNCALL,//函数
            EXP_PREDICATE,
            EXP_UNDEFINED
        };

        public abstract Value eval(Binding b = null);
        public abstract string get_name();
        public abstract ExpType get_type();
        public abstract bool is_variable();

        public virtual Variable get_variable() { return null; }

        public static ExpList explist_eval(ExpList explist,Binding binding)
        {
            if (explist == null)
                return null;
            Value v;
            foreach(Expression ex in explist)
            {
                v = new Value(ex.eval(binding));
            }
            return null;
        }

    }

}
