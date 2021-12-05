using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class ConclusionTopicModule:TopicModule
    {
        public ConclusionKRModuleSNet ConclusionSNet
        {
            get { return (ConclusionKRModuleSNet)_sNet; }
        }

        public ConclusionTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public ConclusionTopicModule(ConclusionKRModule krModule,string topic):
            base(krModule, topic)
        {

        }

        public string Content
        {
            get 
            {
                if (ConclusionSNet.ConclusionNode == null)
                    return $"{Topic}相对应的语义网是空的";
                return ConclusionSNet.ConclusionNode.Name;
            }
        }
        public string RightOption
        {
            get
            {
                if (ConclusionSNet.RightOptionNode == null)
                    return $"{Topic}相对应的语义网是空的";
                return ConclusionSNet.RightOptionNode.Name;
            }
        }
        public string InterferOption
        {
            get
            {
                if (ConclusionSNet.InterfernceNode == null)
                    return $"{Topic}相对应的语义网是空的";
                return ConclusionSNet.InterfernceNode.Name;
            }
        }

        public List<string> KeyWords
        {
            get
            {
                List<string> strs = new List<string>();
                List<SNNode> nodes = ConclusionSNet.KeyWordNodes;
                foreach (var node in nodes)
                {
                    strs.Add(node.Name);
                }
                return strs;
            }

        }

        public List<string> ContentCharacts 
        {
            get
            {
                List<string> strs = new List<string>();
                List<SNNode> nodes = ConclusionSNet.ContentAssocedNodes;
                foreach(var node in nodes)
                {
                    strs.Add(node.Name);
                }
                return strs;
            }

        }
        //public List<string> Interferences//返回干扰项
        //{
        //    get
        //    {
        //        List<string> strs = new List<string>();
        //        List<SNNode> nodes = ConclusionSNet.OptionAssocedNodes;
        //        foreach(var node in nodes)
        //        {
        //            strs.Add(node.Name);
        //        }
        //        return strs;
        //    }
        //}
        //public string Options//返回正确选项
        //{
        //    get
        //    {
        //        SNNode node = ConclusionSNet.OptionNode;
        //        if(node == null)
        //        {
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            return node.Name;
        //        }
        //    }
        //}
       
        public List<string> ContKeyList
        {
            get
            {
                List<string> strs = new List<string>();
                List<SNNode> nodes = ConclusionSNet.ContKeyNodeList;
                foreach (var node in nodes)
                {
                    strs.Add(node.Name);
                }
                return strs;
            }

        }

        public override string Parse()
        {
            return base.Parse();
        }

        public override string Parse(string name)
        {
            return base.Parse(name);
        }
    }
}
