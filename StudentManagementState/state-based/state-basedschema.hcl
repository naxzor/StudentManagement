schema "public" {}

table "Students" {
  schema = schema.public

  column "Id" {
    type = int
    null = false
  }
  column "FirstName" {
    type = varchar(255)
    null = false
  }
  column "MiddleName" {
    type = varchar(255)
    null = true
  }
  column "LastName" {
    type = varchar(255)
    null = false
  }
  column "Email" {
    type = varchar(255)
    null = false
  }
  column "EnrollmentDate" {
    type = timestamp
    null = false
  }
  column "DateOfBirth" {
    type = date
    null = true
  }

  primary_key {
    columns = [column.Id]
  }

  index "IX_Students_Email" {
    unique  = true
    columns = [column.Email]
  }
}

table "Courses" {
  schema = schema.public

  column "Id" {
    type = int
    null = false
  }
  column "Title" {
    type = varchar(255)
    null = false
  }
  column "Credits" {
    type = int
    null = false
  }

  column "InstructorId" {
    type = int
    null = true
  }

  primary_key {
    columns = [column.Id]
  }

  foreign_key "fk_courses_instructor" {
    columns     = [column.InstructorId]
    ref_columns = [table.Instructors.column.Id]
    on_delete   = SET_NULL
  }
}

table "Instructors" {
  schema = schema.public

  column "Id" {
    type = int
    null = false
  }
  column "FirstName" {
    type = varchar(255)
    null = false
  }
  column "LastName" {
    type = varchar(255)
    null = false
  }
  column "Email" {
    type = varchar(255)
    null = false
  }
  column "HireDate" {
    type = date
    null = false
  }

  primary_key {
    columns = [column.Id]
  }

  index "IX_Instructors_Email" {
    unique  = true
    columns = [column.Email]
  }
}

table "Enrollments" {
  schema = schema.public

  column "Id" {
    type = int
    null = false
  }
  column "StudentId" {
    type = int
    null = false
  }
  column "CourseId" {
    type = int
    null = false
  }
  column "Grade" {
    type = numeric(3,1)
    null = true
  }

  primary_key {
    columns = [column.Id]
  }

  foreign_key "fk_enrollments_student" {
    columns     = [column.StudentId]
    ref_columns = [table.Students.column.Id]
    on_delete   = CASCADE
  }

  foreign_key "fk_enrollments_course" {
    columns     = [column.CourseId]
    ref_columns = [table.Courses.column.Id]
    on_delete   = CASCADE
  }
}

table "Departments" {
  schema = schema.public

  column "Id" {
    type = int
    null = false
  }
  column "Name" {
    type = varchar(255)
    null = false
  }
  column "Budget" {
    type = numeric(12,2)
    null = false
  }
  column "StartDate" {
    type = date
    null = false
  }
  column "DepartmentHeadId" {
    type = int
    null = true
  }

  primary_key {
    columns = [column.Id]
  }

  index "UX_Departments_DepartmentHeadId" {
    unique  = true
    columns = [column.DepartmentHeadId]
  }

  foreign_key "fk_departments_head" {
    columns     = [column.DepartmentHeadId]
    ref_columns = [table.Instructors.column.Id]
    on_delete   = NO_ACTION
  }
}