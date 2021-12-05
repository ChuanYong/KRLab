using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using ITS;
using ITS.StudentModule; 
using ITS.MaterialModule;
using ITS.TutorModule;

using Utilities;

namespace ITS
{
    /// <summary>
    /// 在客户端生成该类的实例。
    /// </summary>
    public class ClientSystem
    {
        protected Student _student;
        protected VirtualTutor _tutor;

        public string Course
        {
            get
            {
                return _student.Course;
            }
        }

        public Student Student
        {
            get { return _student; }
        } 

        public ClientSystem()//构造器完成初始实例化过程
        {
            _student = new Student();//实例化一个学生
            _tutor = new VirtualTutor(_student);//针对这个学生的虚拟教师
        }

        public void OnStart(Action<bool, Question> callback)
        {
            _student.Start(callback); 
        }

        /// <summary>
        /// 选择学习科目
        /// </summary>
        /// <param name="course"></param>
        public void OnChooseCourse(string course,Action<Question> callback)
        {
            _student.ChangeLearningCourse(course, (isOk, info) =>
            {
                callback(info);
            });

        } 


        public void OnStartAQuestion(Action<bool,Question> callback)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            _student.PresentedAQuestion((isOk, quest) =>
            {
                callback.Invoke(isOk, quest);//即把参数传入委托封装的方法中
                ITSSpeech.Speak(quest.Content);
            });
            sw.Stop();

            callback.Invoke(true, new Question($"所用时间{sw.ElapsedMilliseconds}毫秒"));

        }

        /// <summary>
        /// 从客服端的GUI获取学生的回答，将学生回答与系统答案进行比对，
        /// 形成反馈，发回到GUI进行显示。
        /// </summary>
        /// <param name="inputStr">用户的输入/param>
        /// <param name="callback"></param>
        public void OnAnswerSubmitted(string inputStr,Action<string,double> callback)
        {
            _student.Feedbacked(inputStr, callback);
        } 

        /// <summary>
        /// 当用户输入字符串+?时，调用该函数，向用户反馈
        /// 正确答案。
        /// </summary>
        /// <param name="callback"></param>
        public void OnObtainCorrects(Action<string> callback)
        {
            _student.ObtainCorrects(callback);
        } 

        /// <summary>
        /// 当用户和输入字符串+？？时，调用该函数，向用户反馈
        /// 查询信息。
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="callback"></param>
        public void OnSearch(string inputStr,Action<bool, string> callback)
        {
            _student.Search(inputStr, callback);
        }

        public void Exit()
        {
            _student.Exit();
        }
    }
}
