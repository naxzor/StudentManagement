using Microsoft.EntityFrameworkCore;
using StudentManagementChange.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Json(new { status = "ok" }));

app.Run();