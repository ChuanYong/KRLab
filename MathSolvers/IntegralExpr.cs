using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Symbolism;
using Symbolism.Substitute;
using Symbolism.LeadingCoefficientGpe;
using Symbolism.CoefficientGpe;
using Symbolism.AlgebraicExpand;
using Symbolism.DegreeGpe;

namespace ITS.MathSolvers
{
    /// <summary>
    /// 整式：单项式和多项式，统称为整式
    /// </summary>
    public class IntegralExpr:ISolving
    {
        public class ParaMathObject
        {
            public Symbol[] Paras;
            public MathObject Expr;

            public ParaMathObject(MathObject expr,params Symbol[] paras)
            {
                Expr = expr;
                Paras = paras;
            }
        } 

        public static List<ParaMathObject> _exprs;
        ParaMathObject _paraExpr;

        protected string _feedbackInfo = string.Empty;

        static IntegralExpr()
        {
            Symbol x = new Symbol("x");
            Symbol y = new Symbol("y");
            Symbol a = new Symbol("a");
            Symbol b = new Symbol("b");
            Symbol c = new Symbol("c"); 

            _exprs = new List<ParaMathObject>()
            {
                new ParaMathObject(new Product(2,x^2),x),
                new ParaMathObject(new Sum(new Power(x,2),4*x,2*new Power(x,2),5*x,16),x),
                new ParaMathObject(new Sum(new Sum(new Product(x,y*y),-new Product(new Fraction(new Integer(1),new Integer(5)),x,y*y))),x,y),
            };
        }

        public List<string> SolvingSteps()
        {
            List<string> steps = new List<string>();
            return steps;
        }

        public void CreateInstance(int i)
        {
            if (i < _exprs.Count)
                _paraExpr = _exprs[i];
        } 

        /// <summary>
        /// 根据输入的参数计算某个整式
        /// </summary>
        /// <param name="i"></param>
        /// <param name="paras">参数的名称及</param>
        public void Calculate(int[] paras,Action<string> callback)
        {
            MathObject expr = _paraExpr.Expr;
            
            ///是单项式
            if(! (expr is Sum))
            {
                var result=Substitute(expr,_paraExpr.Paras, paras);
                string str = string.Empty;
                foreach(Symbol e in _paraExpr.Paras)
                {
                    str += e.name + ",";
                }
                str=str.Remove(str.Length - 1, 1);
                _feedbackInfo += $"单项式，将{str}的值直接代入，结果为{result}\n";
            }

            bool b = false;
            //是否需要展开
            if (expr is Sum)
            {
                Sum sexpr = (Sum)expr;
                List<MathObject> elts = new List<MathObject>();
                foreach(var e in sexpr.elts)
                {
                    if (Utilities.CanExpand(e))
                    {
                        b = true;
                        _feedbackInfo += $"展开{e},\n";
                        elts.Add(Symbolism.AlgebraicExpand.Extensions.AlgebraicExpand(e));
                    }
                    else
                        elts.Add(e);
                }

                expr = new Sum(elts.ToArray());
                if(b)
                    _feedbackInfo += $"得到，{expr.StandardForm()}\n";
            }

            b = false;
            //是否需要合并同类项
            int n = (int)expr.DegreeGpe(new List<MathObject>(_paraExpr.Paras));
            foreach(var x in _paraExpr.Paras)
            {
                for (int i = 0; i <=n; i++)
                {
                    List<MathObject> results;
                    if (Utilities.CanMerge(expr, x, i, out results))
                    {
                        b = true;
                        foreach (var e in results)
                            _feedbackInfo += $"{e}，";
                        Sum sum = new Sum(results.ToArray());
                        var sums=sum.Simplify();
                        _feedbackInfo += $"可以合并同类项为{sums.StandardForm()}，\n";
                    }
                }
            }
            if(expr is Sum && b)
            {
                Sum sexpr = (Sum)expr;
                expr=sexpr.Simplify();
                _feedbackInfo += $"{expr.StandardForm()}，\n";
            }

            List<Symbol> sparas = new List<Symbol>();
            foreach(var p in paras)
            {
                sparas.Add(new Symbol(p.ToString()));
            }

            string strs = string.Empty;
            for (int i=0;i<_paraExpr.Paras.Length;i++)
            {
                Symbol e = _paraExpr.Paras[i];
                strs += e.name + "=" + paras[i] + ",";
            }
            strs = strs.Remove(strs.Length - 1, 1);

            MathObject rs = Substitute(expr, _paraExpr.Paras, sparas.ToArray());
            MathObject rss = Substitute(expr, _paraExpr.Paras, paras);

            _feedbackInfo += $"将{strs}直接代入，得到，\n{rs} = {rss}\n";

            callback(_feedbackInfo);
        }

        protected MathObject Substitute(MathObject ob,Symbol[] ss,int[] vs)
        {
            for(int i=0;i<ss.Length;i++)
            {
                ob = ob.Substitute(ss[i], vs[i]);
            }
            return ob;
        }
         
        protected MathObject Substitute(MathObject ob,Symbol[] ss,Symbol[] vs)
        {
            for(int i=0;i<ss.Length;i++)
            {
                ob = ob.Substitute(ss[i], vs[i]);
            }
            return ob;
        }

        /// <summary>
        /// 对表达式进行简化，合并同类项
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        protected MathObject Simplify(MathObject expr)
        {
            if (expr is Sum)
            {
                Sum sa = (Sum)expr;
                List<MathObject> ls = new List<MathObject>();
                foreach (var e in sa.elts)
                {
                    if (e is Product)
                    {
                        Product pe = (Product)e;
                        ls.Add(pe.Simplify());
                    }
                    else
                        ls.Add(e);
                }
                Sum nsa = new Sum(ls.ToArray());
                var snsa = nsa.Simplify();

                return snsa;
            }
            else
                return expr;
        }

    }
}
