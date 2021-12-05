using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ITS.TutorModule;
using ITS.StudentModule;
using ITS.DomainModule;
using ITS.MaterialModule;

using Utilities;

namespace ITS 
{
    public delegate void OnLoadHistory(LearningHistory history,Ability ab);

    /// <summary>
    /// 从本地文件中读取相关数据初始化辅导模块、知识模块和学生模块，
    /// 系统完善之后从服务器读取数据进行相关初始化。
    /// 实现数据在各个模块(包括用户界面模块)之间的交换。
    /// </summary>
    public class ITSEngine 
    {
        protected  Student _students;
        protected VirtualTutor _tutor;

        protected static ITSEngine _default;  

        public static ITSEngine Default
        {
            get
            {
                if(_default==null)
                    _default=new ITSEngine();

                return _default;
            }
        }

        private ITSEngine()
        {
        }         
         
    }
}
