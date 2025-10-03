namespace StudentManagementChange.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Credits  { get; set; }
    public int? InstructorId { get; set; }
    public Instructor? Instructor { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
}