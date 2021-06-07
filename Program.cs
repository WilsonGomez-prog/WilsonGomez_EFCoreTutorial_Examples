using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using WilsonGomez_EFCoreTutorials_AP2_PT2.DAL;
using WilsonGomez_EFCoreTutorials_AP2_PT2.Entidades;

namespace WilsonGomez_EFCoreTutorials_AP2_PT2
{
    class Program
    {
        static void Main(string[] args)
        {
            #region First Application Chapter
            using (var context = new SchoolContext())
            {
                var grd = new Grade()
                {
                    GradeName = "Computer Science"
                };

                var std = new Student()
                {
                    FirstName = "Billy",
                    Grade = grd
                };
                context.Grades.Add(grd);
                context.Students.Add(std);
                Console.WriteLine(context.SaveChanges() > 0 ? "Guardado" : "No guardado");
            }
            #endregion First Application Chapter

            #region Querying Chapter
            var contexto = new SchoolContext();
                        var studentsWithSameName = contexto.Students
                                                          .Where(s => s.FirstName.Contains(GetName()))
                                                          .ToList();

                        foreach (Student student in studentsWithSameName)
                        {
                            Console.WriteLine("\t" + student.StudentId + " - " + student.FirstName);
                        }
            #endregion

            #region Saving Data Chapter
            //Inserting Data
            using (var context = new SchoolContext())
            {
                var std = new Student()
                {
                    FirstName = "Bill",
                    LastName = "Gates"
                };

                context.Add<Student>(std);
                Console.WriteLine(context.SaveChanges() > 0 ? "Saved" : "Couldn't save");
            }

            //Updating Data
            using (var context = new SchoolContext())
            {
                var std = context.Students.First<Student>();

                std.FirstName = "Steve";
                std.LastName = "Jobs";

                Console.WriteLine(context.SaveChanges() > 0 ? "Updated" : "Couldn't update");
            }

            //Deleting Data
            using (var context = new SchoolContext())
            {
                var std = context.Students.First<Student>();

                context.Remove <Student> (std);

                Console.WriteLine(context.SaveChanges() > 0 ? "Deleted" : "Couldn't delete");
            }
            #endregion

            #region Disconnected Scenario: Insert Data
            
            using (var context = new SchoolContext())
            {
                var stdAddress = new StudentAddress()
                {
                    City = "SFO",
                    State = "CA",
                    Country = "USA"
                };

                var std = new Student()
                {
                    FirstName = "Steve",
                    LastName = "Jobs",
                    Address = stdAddress
                };

                context.Add<Student>(std);

                Console.WriteLine(context.SaveChanges() > 0 ? "The record has been saved" : "Couldn't save");
            }

            //Insert multiples records of the same type
            using (var context = new SchoolContext())
            {
                var StudentList = new List<Student>()
                {
                    new Student() { FirstName = "Jane", LastName = "Doe"},
                    new Student() { FirstName = "John", LastName = "Doe"}
                };

                context.AddRange(StudentList);
                Console.WriteLine(context.SaveChanges() > 0 ? "Multiple records saved" : "Couldn't save");
            }

            //Insert multiples records of different type
            using (var context = new SchoolContext())
            {
                var EntitiesList = new List<Object>()
                {
                    new Student() { FirstName = "Mike", LastName = "Ross"},
                    new StudentAddress() { City = "MAN", State = "NY", Country = "USA"},
                    new Course() { CourseName = "Ethics in Law"}
                };

                context.AddRange(EntitiesList);
                Console.WriteLine(context.SaveChanges() > 0 ? "Multiple records saved" : "Couldn't save");
            }

            //Update a record of an entity
            using (var context = new SchoolContext())
            {
                var stud = new Student() { StudentId = 2, FirstName = "Bill" };

                stud.FirstName = "Steve";

                context.Update<Student>(stud);

                Console.WriteLine(context.SaveChanges() > 0 ? "The record has been updated" : "Couldn't update");
            }

            //Update multiples records of the same type
            using (var context = new SchoolContext())
            {
                IList<Student> modifiedStudents = new List<Student>()
                {
                    new Student() { StudentId = 3, FirstName = "Juan" },
                    new Student() { StudentId = 4, LastName = "Juan" },
                    new Student() { StudentId = 5, FirstName = "Lucas" },
                };
                context.UpdateRange(modifiedStudents);

                Console.WriteLine(context.SaveChanges() > 0 ? "The records has been updated" : "Couldn't update");
            }

            //Delete record of an entity
            using (var context = new SchoolContext())
            {
                var student = new Student() { StudentId = 2 };
                context.Remove<Student>(student);

                context.SaveChanges();
                Console.WriteLine(context.SaveChanges() > 0 ? "The record has been deleted" : "Couldn't delete");
            }

            //Delete record of multiples entities
            using (var context = new SchoolContext())
            {
                IList<Student> students = new List<Student>() 
                {                    
                    new Student(){ StudentId = 3 },
                    new Student(){ StudentId = 4 }
                };
                context.Students.RemoveRange(students);

                Console.WriteLine(context.SaveChanges() > 0 ? "The records has been deleted" : "Couldn't delete");
            }
            #endregion

            #region ChangeTracker

            //EntityState.Unchaged Demonstration
            using (var context = new SchoolContext())
            {
                var student = context.Students.First<Student>();
                DisplayStates(context.ChangeTracker.Entries());
            }

            //EntityState.Added Demonstration
            using (var context = new SchoolContext())
            {
                var student = new Student() 
                { 
                    FirstName = "Juan", 
                    LastName = "Perez" 
                };

                context.Add<Student>(student);
                DisplayStates(context.ChangeTracker.Entries());
            }

            //EntityState.Modified Demonstration
            using (var context = new SchoolContext())
            {
                var student = context.Students.First();
                student.FirstName = "DJ Khalid";
                student.LastName = "AnotherOne";

                DisplayStates(context.ChangeTracker.Entries());
            }

            //EntityState.Deleted Demonstration
            using (var context = new SchoolContext())
            {
                var student = context.Students.First();
                context.Students.Remove(student);

                DisplayStates(context.ChangeTracker.Entries());
            }

            //EntityState.Detached Demonstration
            using (var context = new SchoolContext())
            {
                var student = new Student() { FirstName = "Bruce", LastName = "Wayne"};

                Console.WriteLine($"Entity: {student.GetType().Name}, " +
                    $"State: {contexto.Entry(student).State} ");
            }
            #endregion

            #region Shadow Propierties

            using (var context = new SchoolContext())
            {
                var std = new Student() { FirstName = "Bill" };

                // Setting the shadow property value manually
                context.Entry(std).Property("CreatedDate").CurrentValue = DateTime.Now;

                // Getting the shadow property value
                var createdDate = context.Entry(std).Property("CreatedDate").CurrentValue;
            }

            #endregion

        }

        public static string GetName()
        {
            return "Billy";
        }

        private static void DisplayStates(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, " +
                    $"State: {entry.State} ");
            }
        }
    }
}
