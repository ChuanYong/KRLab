using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.DomainModule;

namespace ITS.StudentModule
{
    public class CourseDictionary
    {
        protected Dictionary<string, Chapter> _courseDict;
        public List<string> Courses
        {
            get { return _courseDict.Keys.ToList(); }
        }

        public CourseDictionary()
        {
            _courseDict = new Dictionary<string, Chapter>();
        }

        public bool IsContains(string course)
        {
            return _courseDict.Keys.Contains(course);
        }

        public CourseDictionary(string course,Chapter chapterDict):this()
        {
            _courseDict[course] = chapterDict;
        }         


        /// <summary>
        /// 获取某门课程所有的章节
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Chapter GetChapterDict(string course)
        {
            if (!_courseDict.Keys.Contains(course))
                return null;
            return _courseDict[course];
        }

        public void AddCourse(string course,Chapter chapter)
        {
            _courseDict[course] = chapter;
        }
    }
}
