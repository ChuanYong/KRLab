using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Symbolism;

namespace ITS.MathSolvers
{
    /// <summary>
    /// 一元一次方程
    /// </summary>
    public class YYYCEqu:BaseEquation,ISolving
    {
        private static Symbol x = new Symbol("x");
        public static List<Symbolism.Equation> equs;

        private MathObject _x;

        public string X
        {
            get { return _x.StandardForm(); }
        }

        static YYYCEqu()
        {
            equs = new List<Symbolism.Equation>()
            {
                new Symbolism.Equation(new Product(2, x),1),//0

                new Symbolism.Equation(new Sum(4 * x,new Product(2,7),new Product(-1,3)),
                    new Sum(new Product(2, x), new Product(x, 3))),//1

                new Symbolism.Equation(new Sum(x,new Product(- 3, 2),new Product(- 2,x)),
                    new Product(new Fraction(new Integer(3),new Integer(2)),x)),//2

                new Symbolism.Equation(new Sum(new Product(2 , 15, x), new Product(70, x)),165+x),//3
                
                new Symbolism.Equation(new Sum(new Product(6,new Sum(x/2,new Product(-1,4))),2*x),new Sum(7,-new Sum(x/3,-1))),//4

                new Symbolism.Equation(new Sum(2,new Product(-3,x+1)),new Sum(1,new Product(-2,new Sum(1,0.5*x)))),//5
             
            };
        }

        public YYYCEqu() : base()
        { 
        }

        public List<string> SolvingSteps()
        {
            List<string> steps = new List<string>();
            return steps;            
        }

        public override void CreateInstance(int i)
        {
            if(i<equs.Count)
                _equ = equs[i];  
        }

        /// <summary>
        /// 需要添加去分母的方法
        /// </summary>
        /// <param name="callback"></param>
        public void Solve(Action<string> callback)
        { 
            bool ba = false;
            bool bb = false;
            if(_equ.a is Sum)
            {
                Sum sa = (Sum)_equ.a;
                foreach(var e in sa.elts)
                {
                    if(Utilities.CanExpand(e))
                    {
                        _feedbackInfo += $"展开的左边{e} \n";
                        ba = true;
                    }
                }
 
            }
            if (_equ.b is Sum)
            {
                Sum sa = (Sum)_equ.b;
                foreach (var e in sa.elts)
                {
                    if (Utilities.CanExpand(e))
                    {
                        _feedbackInfo += $"展开的右边{e} \n";
                        bb = true;
                    }
                }

            }

            if (ba || bb)
            {
                _equ = (Symbolism.Equation)Symbolism.AlgebraicExpand.Extensions.AlgebraicExpand(_equ);
                _feedbackInfo += $"{_equ}\n";
            }

            //判断等式左右两边的情况，
            List<MathObject> csa = Utilities.Get1OrderCoefs(_equ.a, x);
            List<MathObject> csb = Utilities.Get1OrderCoefs(_equ.b, x);
            //_equ.Simplify();

            if(csa!=null && csa.Count>1)
            {
                _feedbackInfo += $"等式的左边合并同类项，得到，\n";
                _equ.a = Simplify(_equ.a);

                _feedbackInfo += _equ.StandardForm()+"\n";
            }

            if(csb!=null && csb.Count>1)
            {
                _feedbackInfo += $"等式的右边合并同类项，得到，\n";
                _equ.b = Simplify(_equ.b);
                _feedbackInfo += _equ.StandardForm() + "\n";
            }

            var ca = Utilities.Get1OrderCoef(_equ.a, x);
            var cb = Utilities.Get1OrderCoef(_equ.b, x);

            if(ca!=0 && cb==0)//等式右边为常数
            {
                _equ.a = x;
                _equ.b = _equ.b / ca;
            }
            else if(ca==0 && cb!=0)//等式左边为常数
            {
                _equ.a = x;
                _equ.b = _equ.a / cb;
            }
            else if(ca!=0 && cb!=0)
            {
                _equ.a = Simplify(_equ.a);
                _equ.b = Simplify(_equ.b);
                MathObject consta = Utilities.GetConstItem(_equ.a, x);
                MathObject constb = Utilities.GetConstItem(_equ.b, x);
                var right = _equ.b - constb;
                _equ.SubstractBothSides(right); 
                _equ.SubstractBothSides(consta);

                _feedbackInfo += $"将{right}移到左边，";
                if (consta != 0)
                    _feedbackInfo += $"将左边的常数项{consta}移到右边，得到,\n";
                else
                    _feedbackInfo += "得到，\n";

                _feedbackInfo += $"{_equ}\n";

                ca = Utilities.Get1OrderCoef(_equ.a, x);
                if (ca != 1)
                {
                    _feedbackInfo += $"等式两边除以{ca}\n";
                    _equ.DividedBothSideBy(ca);
                }

            }
            
            _x = _equ.b;

            _feedbackInfo += $"方程的解为，{_equ}";
            callback(_feedbackInfo);
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
                    if(e is Product)
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
