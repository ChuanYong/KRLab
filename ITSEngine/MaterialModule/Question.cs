using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using ITS.DomainModule;

namespace ITS.MaterialModule
{
    /// <summary>
    /// 学习问题Problem中的提问，包括提问所涉及的文本、图片、图形、动画和声音
    /// </summary>
    public class Question
    {
        protected string _question; 

        protected DrawPicture _picturedrawing;
        protected LoadSound _soundLoading;

        protected string[] _imageFilePaths;

        //问题的等级
        protected string _level;
        public string Level
        {
            get { return _level; } 
        }

        public string Content
        {
            get { return _question; }
        }

        public string[] ImageFilePaths
        {
            get { return _imageFilePaths; }
        }

        public Question(string q, string[] imagePaths=null,double[] icd =null)
        {
            _question = q;
            _imageFilePaths = imagePaths;

            LevelCalculator lc = null;
            if(icd==null)
            {
                lc= new LevelCalculator(0.1, 0.1, 0.1);
            }
            else
            {
                lc= new LevelCalculator(icd[0], icd[1], icd[2]);
            }
            _level = GetQuestionLevel(lc.LevelDictionary);
        } 

        public bool HasImageFiles()
        {
            return _imageFilePaths != null;
        } 

        public void PreparePictureDraws()
        {
            _picturedrawing = new DrawPicture();
        }

        public void PrepareSounds()
        {
            _soundLoading = new LoadSound();
        }


        /// <summary>
        /// 这个函数的相关算法将来要进行改进，目前获取所有等级中值为最高者。
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string GetQuestionLevel(Dictionary<string, double> levels)
        {
            string result = string.Empty;

            double v = -1;
            foreach (var d in levels)
            {
                if (d.Value > v)
                {
                    v = d.Value;
                    result = d.Key;
                }
            }


            return result;
        }
    }
}
