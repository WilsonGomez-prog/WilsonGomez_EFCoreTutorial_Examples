using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilsonGomez_EFCoreTutorials_AP2_PT2.Entidades
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public IList<StudentCourse> StudentCourses { get; set; }
    }
}
