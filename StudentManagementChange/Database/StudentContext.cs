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
    }
}