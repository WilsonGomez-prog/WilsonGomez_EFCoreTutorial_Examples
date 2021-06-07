using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilsonGomez_EFCoreTutorials_AP2_PT2.Entidades
{
    public class Grade
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public string Section { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
