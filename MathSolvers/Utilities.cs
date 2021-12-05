using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Symbolism;
using Symbolism.CoefficientGpe;

namespace ITS.MathSolvers
{
    public static class Utilities
    {

        public static MathObject GetConstItem(MathObject ob, MathObject x)
        {
            return ob.CoefficientGpe(x, 0);
        }

        public static MathObject Get1OrderCoef(MathObject ob, MathObject x)
        {
            return ob.CoefficientGpe(x, 1);
        }
        public static MathObject Get2OrderCoef(MathObject ob, MathObject x)
        {
            return ob.CoefficientGpe(x, 2);
        }

        public static List<MathObject> GetExpands(MathObject ob)
        {
            List<MathObject> results = new List<MathObject>();
            if (ob is Sum)
            {
                Sum sum = (Sum)ob;
                foreach (var e in sum.elts)
                {
                    if (CanExpand(e))
                        results.Add(e);
                }
            }
            else
            {
                if (CanExpand(ob))
                    results.Add(ob);
            }
            return results;
        }

        public static bool CanMerge(MathObject ob,MathObject x,int i,out List<MathObject> results)
        {
            results = new List<MathObject>();

            if(ob is Sum)
            {
                Sum sob = (Sum)ob;
                foreach(var e in sob.elts)
                {
                    MathObject c = e.CoefficientGpe(x, i);
                    if(c!=0)
                    {
                        results.Add(e);
                    } 

                }

            }
            if(results.Count>1)
            {
                return true;
            }
            else 
                return false;
        }

        /// <summary>
        /// 需要完善
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static bool CanExpand(MathObject ob)
        {
            if (ob is Product)
            {
                Product p = (Product)ob;
                foreach (var e in p.elts)
                {
                    if (e is Sum)
                        return true;
                }
            }
            else if (ob is Power)
            {
                Power p = (Power)ob;
                if (p.bas is Sum)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 对sqrt开平方，并将开平方结果除以div
        /// </summary>
        /// <param name="sqrt"></param>
        /// <param name="div"></param>
        /// <returns></returns>
        public static MathObject SimpleSqrt(MathObject sqrt, MathObject div)
        {
            Power psqrt = (Power)sqrt;
            var v = psqrt.bas;
            var n = psqrt.exp;

            if (v is Integer && n is Fraction)
            {
                Fraction fn = (Fraction)n;
                Integer newV = new Integer(((Integer)v).val);

                if (fn.numerator == 1 && fn.denominator == 2)//平方根
                {
                    int i = 0;
                    MathObject r = null;
                    while (true)
                    {
                        r = newV / 4;
                        if (r is Integer && r > 0)
                        {
                            i++;
                            newV = (Integer)r;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (i >= 1)
                    {
                        Product result = new Product(i * 2 / div, Constructors.sqrt(newV));
                        return result.Simplify();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取expr中的所系数
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static List<MathObject> Get1OrderCoefs(MathObject expr, MathObject x)
        {
            if (expr == x)
            {
                return new List<MathObject>() { 1 };
            }

            if (expr is Product)
            {
                Product pe = (Product)expr;
                if (pe.elts.Contains(x))
                {
                    return new List<MathObject>() { pe / x };
                }
            }
            else if (expr is Sum)
            {
                List<MathObject> mos = new List<MathObject>();

                Sum se = (Sum)expr;
                foreach (var e in se.elts)
                {
                    if (e == x)
                    {
                        mos.Add(1);
                    }

                    if (e is Product)
                    {
                        Product pe = (Product)e;
                        if (pe.elts.Contains(x))
                        {
                            mos.Add(pe / x);
                        }
                    }
                }

                return mos;
            }

            return null;
        }
    }
}
