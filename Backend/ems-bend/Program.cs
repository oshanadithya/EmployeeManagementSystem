var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add your connection string here
builder.Services.AddSingleton<DepartmentService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new DepartmentService(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<EmployeeService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new EmployeeService(config.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Apply CORS policy before routing
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map Department Controller Endpoints
app.MapGet("/api/departments", async (DepartmentService departmentService) =>
{
    var departments = departmentService.GetDepartments();
    return Results.Ok(departments);
});

app.MapPost("/api/departments", async (DepartmentService departmentService, Department department) =>
{
    departmentService.AddDepartment(department);
    return Results.Created($"/api/departments/{department.DepartmentId}", department);
});

app.MapPut("/api/departments/{id}", async (DepartmentService departmentService, int id, Department department) =>
{
    departmentService.EditDepartment(id, department);
    return Results.NoContent();
});

app.MapDelete("/api/departments/{id}", async (DepartmentService departmentService, int id) =>
{
    departmentService.DeleteDepartment(id);
    return Results.NoContent();
});

// Map Employee Controller Endpoints
app.MapGet("/api/employees", async (EmployeeService employeeService) =>
{
    var employees = employeeService.GetEmployees();
    return Results.Ok(employees);
});

app.MapPost("/api/employees", async (EmployeeService employeeService, Employee employee) =>
{
    employeeService.AddEmployee(employee);
    return Results.Created($"/api/employees/{employee.EmployeeId}", employee);
});

app.MapPut("/api/employees/{id}", async (EmployeeService employeeService, int id, Employee employee) =>
{
    employeeService.EditEmployee(id, employee);
    return Results.NoContent();
});

app.MapDelete("/api/employees/{id}", async (EmployeeService employeeService, int id) =>
{
    employeeService.DeleteEmployee(id);
    return Results.NoContent();
});

// Ensure this comes after app.UseCors and other middleware
app.MapControllers();

app.Run();
