using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using ITS.StudentModule;
using ITS.DomainModule;
using ITS.MaterialModule;
using KRLab.Core;
using KRLab.Core.SNet;
using Utilities;

namespace ITS.TutorModule
{
    public class CourseFactory
    {
        protected string _course;
        protected DomainTopicPQAFactory _domainSQAFactory;

        public DomainTopicKRModule DomainTopicKRModule
        {
            get { return (DomainTopicKRModule)_domainSQAFactory.KRModule ; }
        } 

        public string Course
        {
            get { return _course; }
        }

        public CourseFactory(string course)
        {
            _course = course;
            _domainSQAFactory = new DomainTopicPQAFactory(course);
        }


        public string GetChapterName(int index)
        {
            return DomainTopicKRModule.GetChapterName(index);
        }
        public string GetSectionName(int chapt,int sect)
        {
            return DomainTopicKRModule.GetSectionName(chapt, sect).Item2;
        }

        public void GetKRType(string chapter,string topic,Action<bool,string>callback)
        {
        } 


        /// <summary>
        /// 根据输入的字符串，解析该字符串的意义。
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="callback"></param>
        public void Parse(string inputStr, Action<bool, string> callback)
        {

        }

        /// <summary>
        /// 创建一个给定知识类型的问题工厂的实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual PQAFactory CreateSQAFactory(string type)
        {
            PQAFactory sqaFactory = null;

            if (type == ProjectType.untsn)
                sqaFactory = new UnitPQAFactory(_course);
            else if (type == ProjectType.conceptsn)
                sqaFactory = new ConceptPQAFactory(_course);
            else if (type == ProjectType.equsn)
                sqaFactory = new EquationPQAFactory(_course);
            else if (type == ProjectType.expsn)
                sqaFactory = new ExperimentPQAFactory(_course);
            else if (type == ProjectType.consn)
                sqaFactory = new ConclusionPQAFactory(_course);
            else if (type == ProjectType.phensn)
                sqaFactory = new PhenomenaPQAFactory(_course);
            else if (type == ProjectType.procesn)
                sqaFactory = new ProceduralPQAFactory(_course);
            else if (type == ProjectType.topicsn)
                sqaFactory = _domainSQAFactory;
            else if (type == ProjectType.inssn)
                sqaFactory = new InstrumentPQAFactory(_course); 

            return sqaFactory;
        }
    }
}
