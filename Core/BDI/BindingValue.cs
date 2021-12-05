using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class BindingValue
    {
        //Binding value can be either a constant value
        public Value _value;
        //or a variable defined in other external binding
        public Variable _external_variable;
        public Binding _external_binding;

        public BindingValue()
        {
            _value=Value.Undefined;
            _external_binding=null;
            _external_variable = null;
        }

        public BindingValue(Variable var,Binding b)
        {
            _value=Value.Undefined;
            _external_binding = b;
            _external_variable = var;
        }

    }
}
