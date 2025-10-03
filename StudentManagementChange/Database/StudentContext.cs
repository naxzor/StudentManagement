using Microsoft.EntityFrameworkCore;
using StudentManagementChange.Models;

namespace StudentManagementChange.Data;

public class StudentContext : DbContext
{
    public StudentContext(DbContextOptions<StudentContext> options) : base(options) {}

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Instructor>()
            .HasIndex(s => s.Email)
            .IsUnique();
        
        b.Entity<Student>()
            .HasIndex(s => s.Email)
            .IsUnique();

        b.Entity<Student>()
            .Property(s => s.DateOfBirth)
            .HasColumnType("date");

        b.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany()
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.SetNull);
        
        b.Entity<Course>()
            .Property(c => c.Credits)
            .HasPrecision(5, 2);
        
        b.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany()
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Enrollment>()
            .Property(e => e.FinalGrade)
            .HasPrecision(3, 1);
        
        b.Entity<Department>()
            .Property(d => d.Budget)
            .HasPrecision(12, 2);

        b.Entity<Department>()
            .HasOne(d => d.DepartmentHead)
            .WithOne()
            .HasForeignKey<Department>(d => d.DepartmentHeadId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Course>()
            .HasOne(c => c.Department)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}