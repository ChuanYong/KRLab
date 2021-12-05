using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DomainModule
{
    public class CourseTypeNames
    {
        public const string Physics = "物理";
        public const string Math = "数学";
        public const string Chemistry = "化学";
        public const string Programming = "编程";  

        public static string GetCourseTypeName(string course)
        {
            if (course.Contains(Physics))
                return Physics;
            else if (course.Contains(Math))
                return Math;
            else if (course.Contains(Chemistry))
                return Chemistry;
            else if (course.Contains(Programming))
                return Programming;
            else
                throw new ArgumentException(course+"：没有相关的课程类型");
        }


    }
}
