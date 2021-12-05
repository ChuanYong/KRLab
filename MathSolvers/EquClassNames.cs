using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace ITS.MathSolvers
{
    public class EquNames
    {
        //方程或公式的名称
        public const string IntegralExpr = "整式表达式";
        public const string YYYCEqu = "一元一次方程";
        public const string YYECEqu = "一元二次方程";
        public const string EYYCEqus = "二元一次方程组";
    }

    public class EquClassNames
    {
        public static Dictionary<string, string> ClassNames = new Dictionary<string, string>()
        {
            {EquNames.YYYCEqu,"YYYCEqu" },
            {EquNames.YYECEqu,"YYECEqu" },
            {EquNames.IntegralExpr,"IntegralExpr" },
        };
    }
}
