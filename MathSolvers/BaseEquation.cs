using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Symbolism.CoefficientGpe;
using Symbolism;

namespace ITS.MathSolvers
{
    /// <summary>
    /// 所有数学表达式的基类
    /// </summary>
    public abstract class BaseEquation 
    { 
        protected string _feedbackInfo = string.Empty;
        protected Symbolism.Equation _equ;
         
        public BaseEquation()
        {
            MathObject.ToStringForm = MathObject.ToStringForms.Standard;
        }

        public abstract void CreateInstance(int i); 

        public override string ToString()
        {
            return $"{_equ.a.StandardForm()} = {_equ.b.StandardForm()}";
        }

    }
}
