using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
using MathNet.Symbolics;

using KRLab.Core;

using KRLab.Core.DataStructures.Graphs;
using KRLab.Core.DataStructures.Lists;
using KRLab.Core.DataStructures;
using KRLab.Core.SNet;

using ITS.DomainModule;

using Utilities;


namespace ITS.MaterialModule
{ 
    /// <summary>
    /// 给出一个问题的文本故事及其相关的图片、图形、动画和声音
    /// </summary>
    public class Problem 
    { 
        //问题
        protected string _story = string.Empty;
        public string Story
        {
            get { return _story; }
        }

        //问题相关的图片文件路径  
        protected DrawPicture _picturedrawing;
        protected LoadSound _soundLoading;


        /// <summary>
        /// 文本+图片构成的学习问题
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imagePaths"></param>
        public Problem(string story="")
        {  
            _story = story;  
            _picturedrawing = new DrawPicture();
            _soundLoading = new LoadSound();
        }
         
        public void PreparePictureDraws()
        {
            _picturedrawing = new DrawPicture();
        }
        public void PrepareSounds()
        {
            _soundLoading = new LoadSound();
        }
    }
 
}
