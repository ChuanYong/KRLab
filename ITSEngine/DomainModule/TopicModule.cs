using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRLab.Core;
using KRLab.Core.SNet;
using Utilities;

namespace ITS.DomainModule
{
    public class TopicModule
    {
        protected string _course;
        protected string _topic;
        protected KRModuleSNet _sNet; 

        public string Topic
        {
            get { return _topic; }
        }

        public KRModuleSNet KRModuleSNet
        {
            get { return _sNet; }
        }

        public TopicModule(string course,KRModuleSNet net)
        {
            _course = course;
            _sNet = net;
        }

        /// <summary>
        /// 在大多数情况下，topic中的学习课题名称与netName一致。
        /// 但有时，学习课题名称只是netName所在的语义网中的某个知识点
        /// </summary>
        /// <param name="krModule"></param> 
        /// <param name="topic">学习课题</param>
        public TopicModule(KRModule krModule,string topic)
        { 
            _course = krModule.Course;
            _topic = topic;

            ///_sNet有可能为null，因为topic有可能不在_course课程里面
            _sNet = krModule.GetKRModuleSNet(topic);
        }

        public  TopicModule()
        {

        }

        /// <summary>
        /// 用于学习结果的反馈，DomainTopicKRModule中的Parse()函数中调用
        /// </summary>
        /// <returns></returns>
        public virtual string Parse()
        {
            return string.Empty;
        }

        /// <summary>
        /// 用于学习结果的反馈，DomainTopicKRModule中的Parse()函数中调用
        /// </summary>
        /// <returns></returns>
        public virtual string Parse(string name)
        {
            return string.Empty;
        }

        /// <summary>
        /// 获取该知识点或学习课题的关联知识点
        /// </summary>
        public List<KnowledgeTopic> GetDeptTopics()
        {
            SNNode topicNode = _sNet.Net.FastGetNode(_topic);
            List<SNNode> deptNodes = _sNet.GetDeptTopicNodes(topicNode);

            List<KnowledgeTopic> topics = new List<KnowledgeTopic>();
            foreach (var node in deptNodes)
            {
                //获取node的知识类型结点
                List<SNNode> krNodes = _sNet.GetKCNodes(node);

                List<string> krTypes = new List<string>();
                foreach (var nd in krNodes)
                    krTypes.Add(nd.Name);
                topics.Add(new KnowledgeTopic(_course, node.Name, krTypes));
            }

            return topics;
        }
    }
}
