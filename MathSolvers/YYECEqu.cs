using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

using Symbolism;
using Symbolism.AlgebraicExpand;
using Symbolism.SimplifyEquation;
using Symbolism.IsolateVariable;

namespace ITS.MathSolvers
{
    /// <summary>
    /// 一元二次方程
    /// </summary>
    public class YYECEqu:BaseEquation,ISolving
    {
        private Symbol x = new Symbol("x");
        private MathObject _x1;
        private MathObject _x2;

        public string X1
        {
            get { return _x1.StandardForm(); }
        }
        public string X2
        {
            get { return _x2.StandardForm(); }
        }

        public YYECEqu()
        {

        }

        public YYECEqu(Symbolism.Equation equ)
        {
            _equ = equ;
        }

        public List<string> SolvingSteps()
        {
            List<string> steps = new List<string>();
            return steps;
        }

        /// <summary>
        /// 根据输入的参数，产生不同的一元二次方程
        /// </summary>
        /// <param name="i"></param>
        public override void CreateInstance(int i)
        {
            MathObject left = null;
            MathObject right = null;

            if (i==0)
            {
                left = x*x + 2*x ;
                right = 1;
            }
            else if(i==1)
            {
                left = x*x + (4 * x) + 3;
                right = 5;
            }
            else if(i==2)
            {
                left = (x + 1) * (x + 3) + 2;
                right = 3 * x;
            }
            else if(i==3)
            {
                left = (x + 4) * (4*x + 2) + 3;
                right = 2*(x^2);
            }
            else
            {
                throw new Exception();
            }

            _equ = new Symbolism.Equation(left, right);
        } 

        /// <summary>
        /// 配方法求解一元二次方程
        /// </summary> 
        /// <param name="callback">反馈信息</param>
        public void MatchingMethodSolve(Action<string> callback)
        { 
            MathObject coef2 = Utilities.Get2OrderCoef(_equ.a, x);
            MathObject constItem = null;
            MathObject coef1 = null;
            if(coef2.Equals(new Integer(0)))
            {
                constItem = Utilities.GetConstItem(_equ.a, x);
                coef1 = Utilities.Get1OrderCoef(_equ.a, x);
                _equ.SubstractBothSides(constItem);
                _equ.DividedBothSideBy(coef1);

                _feedbackInfo += _equ.StandardForm() + "\n";
                callback(_feedbackInfo);
                return;
            }

            ///(1)检查是否需要移项
            if (!_equ.b.Equals(new Integer(0)))
            {
                _feedbackInfo += $"方程的右边是{_equ.b}，不为0，将其移到等式的左边。\n";
                _equ.a -= _equ.b;
                _equ.b = 0;
                _feedbackInfo += _equ.StandardForm() + "\n";
            }

            //检查是否需要展开
            List<MathObject> expands = Utilities.GetExpands(_equ.a);
            foreach (var e in expands)
            {
                _feedbackInfo += $"需要展开等式左边的{ e}，然后合并同类项，得到，\n";
                _equ = (Symbolism.Equation)Symbolism.AlgebraicExpand.Extensions.AlgebraicExpand(_equ);
                _feedbackInfo += _equ.StandardForm() + "\n";
            }

            if (coef2!=null && !coef2.Equals(new Integer(1)))
            {
                _feedbackInfo += $"二次项系数的系数为{coef2.StandardForm()},等式两边除以{coef2.StandardForm()},将二次项系数变成1\n";
                _equ.DividedBothSideBy(coef2);
                _equ= (Symbolism.Equation)Symbolism.AlgebraicExpand.Extensions.AlgebraicExpand(_equ);
                _feedbackInfo += _equ.StandardForm() + "\n";
            } 

            //配方
            constItem = Utilities.GetConstItem(_equ.a,x);
            coef1 = Utilities.Get1OrderCoef(_equ.a, x);
            MathObject coef12 = coef1 / 2;
            if(!constItem.Equals(new Integer(0)))
            {
                _feedbackInfo += $"配方，将常数项{constItem.StandardForm()}移到右边，\n";
                _equ.SubstractBothSides(constItem);
                _feedbackInfo += _equ.StandardForm() + "\n";
            }

            if(!coef1.Equals(new Integer(0)))
            {
                MathObject coef = coef12 ^ 2;
                _feedbackInfo += $"一次项系数b={coef1.StandardForm()}，按公式{new Symbol("(b/2)^2")}={coef.StandardForm()}，等式两边加{coef.StandardForm()}，得到\n";
                _equ.AddToBothSides(coef);
                _feedbackInfo += _equ.StandardForm() + "\n";

                _feedbackInfo += "等式左边利用完全平方公式，得到，\n";
                _equ.a = (x + coef12) ^ 2;
                _feedbackInfo += _equ.StandardForm() + "\n";
            }


            //检查等式右边是否小于0
            if(_equ.b<0)
            {
                _feedbackInfo += $"等式右边等于{_equ.b},小于0，无解。\n";
                callback(_feedbackInfo);
                return;
            }

            _feedbackInfo += "左右两边开平方，得到，\n";
            if(_equ.b.Equals(new Integer(0)))
            {
                Symbolism.Equation equ = new Symbolism.Equation(x + coef12, 0, _equ.Operator);
            }
            Symbolism.Equation equ1 = new Symbolism.Equation(x + coef12, Constructors.sqrt(_equ.b), _equ.Operator);
            Symbolism.Equation equ2 = new Symbolism.Equation(x + coef12, -Constructors.sqrt(_equ.b), _equ.Operator);
            _feedbackInfo += equ1.StandardForm() + "\n" + equ2.StandardForm() + "\n";

            _feedbackInfo += $"将上面等式左边的{coef12.StandardForm()}移到右边，得到，\n";
            
            equ1.SubstractBothSides(coef12);
            equ2.SubstractBothSides(coef12);

            _x1 = equ1.b;
            _x2 = equ2.b;

            _feedbackInfo+=equ1.StandardForm() + "\n" + equ2.StandardForm() + "\n\n";

            _feedbackInfo += "方程求解完毕。\n";

            callback(_feedbackInfo);
        }

        public void EquationMethodSolve(Action<string> callback)
        {
            MathObject coef2 = Utilities.Get2OrderCoef(_equ.a, x);
            MathObject coef1 = Utilities.Get1OrderCoef(_equ.a, x);

            if (coef2==0 && coef1!=0)
            {    
                MathObject constItem = Utilities.GetConstItem(_equ.a, x);
                _equ.SubstractBothSides(constItem);
                _equ.DividedBothSideBy(coef1);

                _feedbackInfo += $"方程的二次项系数等于0，方程有一个根，\n";
                _feedbackInfo += $" {_equ}\n";
                callback(_feedbackInfo);
                return;
            }

            ///(1)检查是否需要移项
            if (_equ.b!=0)
            {
                _feedbackInfo += $"方程的右边是{_equ.b}，不为0，将其移到等式的左边。\n";
                _equ.a -= _equ.b;
                _equ.b = 0;
                _feedbackInfo += _equ.StandardForm() + "\n";
            }

            //检查是否需要展开
            List<MathObject> expands = Utilities.GetExpands(_equ.a);
            foreach (var e in expands)
            {
                _feedbackInfo += $"需要展开等式左边的{ e.StandardForm()}，然后合并同类项，得到，\n";
                _equ = (Symbolism.Equation)Symbolism.AlgebraicExpand.Extensions.AlgebraicExpand(_equ);
                _feedbackInfo += _equ.StandardForm() + "\n";
            }

            MathObject a = Utilities.Get2OrderCoef(_equ.a, x);
            MathObject b = Utilities.Get1OrderCoef(_equ.a, x);
            MathObject c = Utilities.GetConstItem(_equ.a, x);

            if(a==0 && b!=0)
            {
                MathObject root = c / b;
                _feedbackInfo += $"方程有一个根，x={root}\n";
                callback(_feedbackInfo);
                return;
            }

            MathObject delta = (b^2) - 4 * a * c;
            MathObject deltaSymb = new Sum(new Power(new Symbol("b"),2),-new Product(4,new Symbol("a")*new Symbol("c")));

            _feedbackInfo += $"a={a},b={b},c={c}\n";
            _feedbackInfo += $"计算δ，\n δ={deltaSymb.StandardForm()}={delta.StandardForm()}\n"; 
            
            if(delta<new Integer(0))
            {
                _feedbackInfo += $"δ<0，方程无解，\n";
                callback(_feedbackInfo);
                return;
            } 
            Symbolism.Equation equ1 = new Symbolism.Equation(new Symbol("x1"), new Sum(-new Symbol("b") + Constructors.sqrt(new Symbol("δ"))) / new Product(2 * new Symbol("a")));
            Symbolism.Equation equ2 = new Symbolism.Equation(new Symbol("x2"), new Sum(-new Symbol("b") - Constructors.sqrt(new Symbol("δ"))) / new Product(2 * new Symbol("a")));

            Power sqrtDelta = new Power(delta,new Fraction(new Integer(1),new Integer(2)));
            MathObject result = Utilities.SimpleSqrt(sqrtDelta,new Product(2, a)); 
            MathObject x1 = -b / (2 * a) + result; 
            MathObject x2 = -b / (2 * a) - result;

            _x1 = x1;
            _x2 = x2;

            _feedbackInfo += $"根据公式{equ1.StandardForm()}，得到x1={x1.StandardForm()}\n";
            _feedbackInfo += $"根据公式{equ2.StandardForm()}，得到x2={x2.StandardForm()}\n";

            callback(_feedbackInfo);

        } 

    }
}
