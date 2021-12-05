using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace KRLab.Core
{
    /// <summary>
    /// 利用这个项目类型名，生成文件的后缀名，
    /// 所以这个类型名称比较简单。名称后面带有sn
    /// 表示这是语义网semantic network，gsn表示
    /// 一般意义的项目，该项目中的各种图diagram的语法
    /// 不做特别检查。
    /// </summary>
    public class ProjectType
    {
        public const string gsn = "gsn";      //一般语义网

        ///按知识呈现形式，将语义项目分类，为一级语义网
        public const string conceptsn = "cnpsn";//概念语义网
        public const string equsn = "equsn";    //公式语义网
        public const string consn = "consn";   //结论语义网，表述公理、定理、定律等
        public const string phensn = "phensn";  //现象语义网
        public const string procesn = "procesn";//流程、步骤、算法语义网 

        /// <summary>
        /// 二级语义网，较为特殊。
        /// </summary>
        public const string topicsn = "topicsn";//课题语义网
        public const string untsn = "untsn";    //单位语义网
        public const string expsn = "expsn";    //实验语义网
        public const string inssn = "inssn";    //器材语义网

        public const string gbn = "gbs";//一般贝叶斯网
        public const string gcm = "gcm";//一般概念网

        public static Dictionary<string, string> TypeToName = new Dictionary<string, string>()
        {
            {topicsn,$"{KCNames.Catalogue}语义项目" },
            {conceptsn,$"{KCNames.Concept}语义项目" },
            {equsn,$"{KCNames.Equation}语义项目" }, 
            {untsn,$"{KCNames.Unit}语义项目" },
            {consn,$"{KCNames.Conclusion}语义项目" },
            {expsn,$"{KCNames.Experiment}语义项目" },
            {phensn,$"{KCNames.Phenomena}语义项目" },
            {procesn,$"{KCNames.Procedural}语义项目" },
            {inssn,$"{KCNames.Instrument}语义项目" },
            {gsn,"一般语义项目" },
            {gbn,"一般贝叶斯项目" },
            {gcm,"一般概念项目" }
        };

        /// <summary>
        /// 将KCNames中的名称转换为Project类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string KCNameToProjectType(string name)
        {
            foreach(var dict in TypeToName)
            {
                if (dict.Value.Contains(name))
                    return dict.Key;
            }

            return null;
        }
    } 
}
