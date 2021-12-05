using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DomainModule
{
    /// <summary>
    /// 用于描述知识点---对应学习课题下面的知识点，不会出现在目录语义网中。
    /// 因为每个知识点有可能对应多个知识类型，所以有多个语义网与之对应
    /// ，这些语义网在不同的知识类型语义项目中，相应的语义项目名称就是知识类型
    /// 名称，用字符串数组_krTypes表示。
    /// </summary>
    [Serializable]
    public class KnowledgeTopic
    {
        protected string _course;
        protected string _topic;

        //一个知识点有可能对应多个知识类型。
        //中文的知识类型，比如，概念、单位等，
        protected List<string> _krTypes;

        public string Topic
        {
            get { return _topic; }
        } 

        public List<string> KRTypes
        {
            get { return _krTypes; }
        }

        public KnowledgeTopic(string course,string topic,List<string> krTypes)
        {
            _course = course;
            _topic = topic;
            _krTypes = krTypes;
        }
    }
}
