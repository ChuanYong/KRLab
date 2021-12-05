using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core;

namespace ITS.DomainModule
{
    /// <summary>
    /// 该类可以用于目录语义网中列出的学习课题--对应课程中小节下一级的分类。
    /// </summary>
    [Serializable]
    public class LearningTopic:KnowledgeTopic
    { 
        protected ChapterItem _chaptItem;//章
        protected SectionItem _sectItem;

        //该学习课题在在章、小节、课题三级上的权重
        protected Tuple<int, int, int> _weights;

        public string Course
        {
            get { return _course; }
        }

        public ChapterItem ChaptItem
        {
            get { return _chaptItem; }
        } 

        public SectionItem SectItem
        {
            get { return _sectItem; }
        }   
        
        public string ChaptIndex
        {
            get { return _chaptItem.Index; }
        }
        public string SectIndex
        {
            get { return _sectItem.Index; }
        }

        public bool IsEmpty
        {
            get { return _topic == null; }
        } 

        public Tuple<int,int,int> Weights
        {
            get { return _weights; }
        }

        public double WeightInChapter
        {
            get
            {
                return _weights.Item2*_weights.Item3 * 0.01;
            }
        }

        public LearningTopic(string course,ChapterItem chapt,SectionItem section,string topic,
            List<string> krTypes,Tuple<int,int,int> weights):base(course,topic,krTypes)
        {
            _weights = weights;
            _course = course;
            _chaptItem = chapt;
            _sectItem = section;
            _topic = topic;
        }
    }
}
