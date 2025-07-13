using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Infrastructure.Persistence;
using StudentCardAssignment.Infrastructure.Repositories;
using StudentCardAssignment.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(StudentCardAssignment.Application.Students.Commands.CreateStudent.CreateStudentCommand).Assembly);
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(StudentCardAssignment.Application.Students.Commands.CreateStudent.CreateStudentCommand).Assembly);

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add event store
builder.Services.AddScoped<StudentCardAssignment.Infrastructure.EventStore.IEventStore, StudentCardAssignment.Infrastructure.EventStore.SqlEventStore>();

// Add Infrastructure services (event-sourced repositories and projections)
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Card Assignment API V1");
    c.RoutePrefix = "swagger";
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
