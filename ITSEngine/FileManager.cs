using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using KRLab.Translations;
using ITS.DomainModule; 
using KRLab.Core;
using KRLab.Core.SNet;
using Utilities;
using System.Threading;



namespace ITS
{
    public class FileManager
    {
        public static string DataPath = @"F:\Work\KRLab17\FVCData";
        public static string KnowledgePath = DataPath + @"\知识库";
        public static string LearningHistoryPath = DataPath+@"\学习历史";

        protected static SNetProject _subjProject = null;

        public static SNetProject SubjectProject
        {
            get { return _subjProject; }
        }


        static FileManager()
        {
            string path = KnowledgePath + @"\课程排序.gsn";
            _subjProject = new SNetProject();
            _subjProject.LoadFromFile(path);
        }

        /// <summary>
        /// 在KnowledgePath文件夹下面是各个学科类型文件夹，比如
        /// 物理、数学、化学、生物、编程、信息工程等。
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCourseTypeNames()
        {
            DirectoryInfo di = new DirectoryInfo(KnowledgePath);
            List<string> dirs = new List<string>();
            DirectoryInfo[] arr = di.GetDirectories();
            if(arr==null)
            {
                return null;
            }
            foreach(var d in arr)
            {
                dirs.Add(d.ToString());
            }
            return dirs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <param name="type">是ProjectType中的类型，不是KCNames中的类型名</param>
        /// <returns></returns>
        public static string GetKRSNProjectPath(string course,string type)//获取传入的课程类型的路径
        {
            if (string.IsNullOrEmpty(course) || string.IsNullOrEmpty(type))
                return null;

            string fullName = course + "." + type;
            
            List<string> courseTypes = GetCourseTypeNames(); 
            foreach (var ct in courseTypes)
            {
                string path = KnowledgePath + @"\" + ct + @"\" + fullName;
                if (File.Exists(path))
                    return path;
            }
            return null;
        }

        public static string GetCourseTypeName(string course)
        {
            List<string> names = GetCourseTypeNames();
            foreach (var ct in names)
            {
                string path = KnowledgePath + @"\" + ct + @"\" + course+".topicsn";
                if (File.Exists(path))
                    return ct;
            }
            return null;
        }
         
        /// <summary>
        /// 获取某门课程下的图片文件夹路径
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public static string GetImageDirectory(string course)
        {
            string name = GetCourseTypeName(course);
            if (name != null)
                return KnowledgePath + @"\" + name + @"\图片";
            return null;
        }

        /// <summary>
        /// 获取某门课程中名称为fileName的图片路径
        /// </summary>
        /// <param name="course"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetImageFilePath(string course,string fileName)
        {
            string dir = GetImageDirectory(course);
            if (!Directory.Exists(dir))
                return null;

            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] fs = dirInfo.GetFiles();
            foreach (var f in fs)
            {
                if (f.Name.Contains(fileName))
                {
                    return f.FullName;
                } 
            }
            return null;
        }

        public static string GetLearningHistoryFilePath(string course)
        {
            string dir = LearningHistoryPath;
            if (!Directory.Exists(dir))
                return null;

            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] fs = dirInfo.GetFiles();
            foreach (var f in fs)
            {
                if (f.Name.Contains(course))
                {
                    return f.FullName;
                }
            }
            return null;
        } 

        /// <summary>
        /// 获取某个学科，比如数学，的课程排序
        /// </summary>
        /// <param name="project"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static List<string> GetCourseOrder(string subject)
        {
            SemanticNet net= _subjProject.GetSNet(subject);
            if (net == null)
                return null;

            List<string> names = new List<string>();
            SNNode startNode = net.FastGetNode("起始课程");
            SNNode endNode = net.FastGetNode("最终课程");

            startNode = net.GetIncomingSource(startNode, SNRational.ISA);
            endNode = net.GetIncomingSource(endNode, SNRational.ISA);

            List<SNNode> nodes= net.GetAPath(startNode, endNode);
            foreach(var node in nodes)
            {
                names.Add(node.Name);
            }
            return names;
        }

        /// <summary>
        /// 获取语义图
        /// </summary>
        /// <param name="subject">学科名称，比如数学、物理、化学，等</param>
        /// <param name="course">课程名称，比如七年级数学上、八年级物理上，等</param>
        /// <param name="type">语义网类型，比如topicsn,cnpsn,consn,等</param>
        /// <param name="topic">语义图的名称</param>
        /// <returns></returns>
        public static SemanticNet GetTopicNet(string subject,string course,string type,string topic)
        {
            string fullCourseName = course + "." + type;
            string path=Path.Combine(KnowledgePath, subject,fullCourseName);
            if(Directory.Exists(path))
            {
                SNetProject project = new SNetProject();
                project.LoadFromFile(path);
                return project.GetSNet(topic);
            }
            return null;
        }

        /// <summary>
        /// 从course的所有前序课程中查找类型为type，学习课题为topic的语义网
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="course"></param>
        /// <param name="type"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static SemanticNet SearchTopicNetPrior(string subject, string course, string type, string topic)
        {
            List<string> courses = GetCourseOrder(subject);
            List<string> preCourses = new List<string>();
            if(courses!=null)
            {
                foreach(var str in courses)
                {
                    if(str==course)
                    {
                        break;
                    }
                    preCourses.Add(str);
                }

                if(preCourses.Count>0)//找到了course的前序课程
                {
                    foreach(var pre in preCourses)
                    {
                        SemanticNet net = GetTopicNet(subject, pre, type, topic);
                        if (net != null)
                            return net;
                    }
                }
                else///没有找到在course的前序课程
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// 在除了course之外的所有subject学科中的其它课程中查找类型为type的学习课题topic的
        /// 语义网
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="course"></param>
        /// <param name="type"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static SemanticNet SearchTopicNet(string subject,string course,string type,string topic)
        {
            List<string> courses = GetCourseOrder(subject); 
            if (courses != null)
            {
                courses.Remove(course);//除了course之外的所有课程

                if (courses.Count > 0) 
                {
                    foreach (var pre in courses)
                    {
                        SemanticNet net = GetTopicNet(subject, pre, type, topic);
                        if (net != null)
                            return net;
                    }
                }
                else///没有找到在course的前序课程
                {
                    return null;
                }
            }

            return null;
        }
    }
}
