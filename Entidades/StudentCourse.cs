using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilsonGomez_EFCoreTutorials_AP2_PT2.Entidades
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
