using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core;
using KRLab.Core.SNet;

namespace ITS.DomainModule
{

    /// <summary>
    /// 将知识分成各种类型，该类是各种类型知识表达的基类。
    /// 目前，主要分为如下几类：
    /// SingleEquation：表示公式
    /// Measure：表示测量
    /// Unit：单位
    /// PhysicsQuantity：物理量
    /// MathQuantity：数学量
    /// Concept:概念
    /// Principle：原理、定律
    /// Experiment：实验 
    /// KRModule对应语义项目，注意与TopicModule的不同，TopicModule对应
    /// 语义项目中的一个语义图。
    /// </summary>
    public abstract class KRModule
    {
        protected string _course;
        protected string _krType;

        protected object _project;

        public string KRType
        {
            get { return _krType; }
        }

        public string Course
        {
            get { return _course; }
        }  

        /// <summary>
        /// 给出某类知识的共同属性，比如测量知识点，有:误差处理、测量工具和测量
        /// 方法
        /// </summary>
        public abstract List<string> KRAttributes
        {
            get;
        }

        public abstract object Project
        {
            get;
        }

        public abstract List<SemanticNet> SNets
        {
            get;
        }

        public KRModule(string course,string krType)
        {
            _course = course;
            _krType = krType; 
        } 

        /// <summary>
        /// 获取语义项目中的一个语义网络SemanticNet
        /// </summary>
        /// <param name="netName">网络名称，一般就是学习课题名称</param>
        /// <returns></returns>
        public abstract KRModuleSNet GetKRModuleSNet(string netName);

        /// <summary>
        /// 创建一个学习课题模块实例，一个学习课题模块对应一个语义网络
        /// </summary>
        /// <param name="netName">语义网络名称=学习课题名称</param>
        /// <returns></returns>
        public abstract TopicModule CreateTopicModule(string netName);

        /// <summary>
        /// 获取该知识类型中的所有语义网名称
        /// </summary>
        public List<string> SNetNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (var net in SNets)
                {
                    names.Add(net.Topic);
                }
                return names;
            }
        } 
         
    }
}
