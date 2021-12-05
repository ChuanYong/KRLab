using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.TutorModule
{
    /// <summary>
    /// 描述Virtual Tutor的一个教案的教学过程
    /// </summary>
    public enum TeachingState
    {
        Pre,//没有确定学习的课程 
        Prepared,//课程准备好了，
        SchemePrepared,  //教案准备完毕 
        ProblemBegin,//表示已经提出了一个问题
        ProblemFinished,//表示提出的问题已经指导完成 
        SchemeFinished,
        CourseFinished,//一门课程结束
    }
}
