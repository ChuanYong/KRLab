using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.StudentModule
{
    /// <summary>
    /// 学生的知识水平
    /// </summary>
    public class Ability
    {  
        //comprehension ability
        protected double _ca;
        //problem-solving ability
        protected double _pa;

        public double CA
        {
            get { return _ca; }
            set { _ca = value; }
        }

        public double PA
        {
            get { return _pa; }
            set { _pa = value; }
        }
        

        //记录课本每章的得分
        protected Dictionary<string,double> _scoreDict; 
        public Dictionary<string,double> ScoreDict
        {
            get { return _scoreDict; }
            set { _scoreDict = value; }
        }
        public bool IsEmpty
        { get { return _scoreDict.Count == 0; } }


        public Ability( )
        {  
            _scoreDict = new Dictionary<string, double>();
        }

        public double GetScore(string topic)
        {
            if (!ScoreDict.Keys.Contains(topic))
                return 0.0;
            else return ScoreDict[topic];
        }

        public void UpdateScore(string chaptName,double score)
        {
            if(!_scoreDict.Keys.Contains(chaptName))
            {
                _scoreDict[chaptName]=score;
            }
            else
            {
                _scoreDict[chaptName] += score;
            }
        }

    }
}
