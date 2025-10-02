namespace StudentManagementChange.Models;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; } 
    public string LastName  { get; set; } = null!;
    public string Email     { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}