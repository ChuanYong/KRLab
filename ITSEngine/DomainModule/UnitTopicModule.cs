using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class UnitTopicModule:TopicModule
    {
        public UnitKRModuleSNet SNet
        {
            get { return (UnitKRModuleSNet)_sNet; }
        } 

        public List<string> Units
        {
            get { return SNet.UnitNodeDict.Keys.ToList(); }
        }

        public UnitTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public UnitTopicModule(UnitKRModule krModule,string topic):
            base(krModule,topic)
        {

        }

        public override string Parse()
        {
            return base.Parse();
        }

        public override string Parse(string name)
        {
            return base.Parse(name);
        }


        public string GetSymbol(string unit)
        {
            if (SNet.UnitSymbolDict.Keys.Contains(unit))
                return SNet.UnitSymbolDict[unit];
            return null;
        }
        public string GetUnitSymbol(string symbol)
        {
            foreach (var x in SNet.UnitSymbolDict)
                if (x.Value == symbol)
                    return x.Key;

            return null;
        }

        /// <summary>
        /// 获取国际(IS)单位
        /// </summary>
        /// <returns></returns>
        public string GetISUnit()
        {
            SNNode isNode = SNet.GetISUnitNode();
            if (isNode != null)
                return isNode.Name;
            return null;
        } 

        /// <summary>
        /// 获取x个u0相当于多少个u1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="u0">u0是一个单位名称，不是符号</param>
        /// <param name="u1">u1是一个单位名称，不是符号</param>
        /// <returns></returns>
        public string GetResult(string x, string u0, string u1)
        { 
            string str = x + GetRate(SNet.Net, u0, u1);
            return str;
        }

        public string GetDefinition(string name)
        {
            if (!SNet.UnitNodeDict.Keys.Contains(name))
                return null;

            SNNode node = SNet.GetDefinitionNode(SNet.UnitNodeDict[name]);
            if (node != null)
                return node.Name;
            else
                return null;
        }


        /// <summary>
        /// 获取两个单位的比例关系的字符串，
        /// 需要将字符串转换为计算式子。
        /// </summary>
        /// <param name="u0"></param>
        /// <param name="u1"></param>
        /// <returns></returns>
        private string GetRate(SemanticNet net, string u0, string u1)
        {
            string str = string.Empty;
            List<SNNode> path = net.GetAConnection(SNet.UnitNodeDict[u0], SNet.UnitNodeDict[u1]);
            if (path.Count < 2)
                return str;

            for (int i = 0; i < path.Count - 1; i++)
            {
                string tmp = GetRate(net, path[i], path[i + 1]);
                if (tmp == string.Empty)
                    return string.Empty;

                str += "*(" + tmp + ")";
            }
            return str;
        }

        /// 获取紧邻的两个节点之间的公式 
        private string GetRate(SemanticNet net, SNNode n0, SNNode n1)
        {
            SNRational rational = net.ConnectionRational(n0, n1);

            //
            if (rational == null || rational.Rational != SNRational.ASSOC)
                return string.Empty;

            return rational.EndMulti + "/" + rational.StartMulti;
        }

    }
}
