using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementChange.Migrations
{
    /// <inheritdoc />
    public partial class RenameGradeToFinalGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Enrollments",
                newName: "FinalGrade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinalGrade",
                table: "Enrollments",
                newName: "Grade");
        }
    }
}
