using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 课程语义网名称，与ProjectType中的语义项目类型相对应。
    /// </summary>
    public static class KCNames
    {
        public const string Catalogue = "目录";
        public const string Concept = "概念"; 
        public const string Unit = "单位"; 
        public const string Equation = "公式";
        public const string Conclusion = "结论"; 
        public const string Experiment = "实验"; 
        public const string Phenomena = "现象知识";
        public const string Procedural = "流程";
        public const string Instrument = "器材";

        public static List<string> Names = new List<string>()
        {
            Catalogue,
            Concept, 
            Unit,
            Equation,
            Conclusion,
            Experiment, 
            Phenomena,
            Procedural,
            Instrument,
        };
    }
}
