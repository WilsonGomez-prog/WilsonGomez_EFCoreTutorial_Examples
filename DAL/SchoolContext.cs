using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WilsonGomez_EFCoreTutorials_AP2_PT2.Entidades;

namespace WilsonGomez_EFCoreTutorials_AP2_PT2.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StudentAddress> StudentAddresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=SchoolDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Properties Configurations.
            modelBuilder.Entity<Student>()
                    .Property(s => s.StudentId)
                    .UseIdentityColumn()
                    .IsRequired();

            modelBuilder.Entity<Grade>()
                    .Property(g => g.Id)
                    .UseIdentityColumn()
                    .IsRequired();

            modelBuilder.Entity<StudentAddress>()
                    .Property(sa => sa.StudentAddressId)
                    .UseIdentityColumn()
                    .IsRequired();

            modelBuilder.Entity<Course>()
                    .Property(c => c.CourseId)
                    .UseIdentityColumn()
                    .IsRequired();


            modelBuilder.Entity<Student>()
                .Property(s => s.GradeId)
                .HasDefaultValue(0);

            //Entity Configuration
            //One-to-Many Relationship
            modelBuilder.Entity<Student>()
                .HasOne<Grade>(s => s.Grade)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            //One-to-One Relationship
            modelBuilder.Entity<Student>()
                .HasOne<StudentAddress>(s => s.Address)
                .WithOne(a => a.Student)
                .HasForeignKey<StudentAddress>(a => a.StudentId);

            //Many-to-Many Relationship

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });
            
            modelBuilder.Entity<StudentCourse>()
                .HasOne<Student>(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);


            modelBuilder.Entity<StudentCourse>()
                .HasOne<Course>(sc => sc.Course)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            //Configuring Shadow Propierties in all the entities

            var allEntities = modelBuilder.Model.GetEntityTypes();

            foreach (var entity in allEntities)
            {
                entity.AddProperty("CreatedDate", typeof(DateTime));
                entity.AddProperty("UpdatedDate", typeof(DateTime));
            }
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Property("UpdatedDate").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
