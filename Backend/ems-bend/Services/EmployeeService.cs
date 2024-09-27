using System;
using System.Collections.Generic;
using System.Data;
// using Microsoft.Data.SqlClient; // Correct namespace for SqlConnection
using System.Data.SqlClient;



public class EmployeeService
{
    private readonly string _connectionString;

    public EmployeeService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Employee> GetEmployees()
    {
        var employees = new List<Employee>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT * FROM Employees", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            EmailAddress = reader.GetString(3),
                            DOB = reader.GetDateTime(4),
                            Age = reader.GetInt32(5),  // Directly fetching the computed Age from DB
                            Salary = reader.GetDecimal(6),
                            DepartmentId = reader.GetInt32(7),
                            CreatedDate = reader.GetDateTime(8),
                            ModifiedDate = reader.GetDateTime(9)
                        });
                    }
                }
            }
        }

        return employees;
    }

    public void AddEmployee(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("INSERT INTO Employees (FirstName, LastName, EmailAddress, DOB, Salary, DepartmentId, CreatedDate, ModifiedDate) VALUES (@firstName, @lastName, @Email, @dob, @salary, @departmentId, GETDATE(), GETDATE())", connection))
            {
                // Null or invalid DOB check (optional based on your use case)
                if (employee.DOB == null || employee.DOB < new DateTime(1753, 1, 1))
                {
                    throw new ArgumentException("Invalid Date of Birth. Must be between 1/1/1753 and 12/31/9999.");
                }

                // If you want to check other optional fields like Email:
                string email = string.IsNullOrEmpty(employee.EmailAddress) ? DBNull.Value.ToString() : employee.EmailAddress;

                // Add parameters with null handling
                command.Parameters.AddWithValue("@firstName", employee.FirstName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@lastName", employee.LastName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);  // Handle null or empty Email
                command.Parameters.AddWithValue("@dob", employee.DOB);
                Console.WriteLine($"DOB: {employee.DOB}");
                command.Parameters.AddWithValue("@salary", employee.Salary > 0 ? employee.Salary : (object)DBNull.Value);  // Handle zero or null salary
                command.Parameters.AddWithValue("@departmentId", employee.DepartmentId > 0 ? employee.DepartmentId : (object)DBNull.Value);  // Ensure valid departmentId

                command.ExecuteNonQuery();
            }
        }
    }


    public void EditEmployee(int employeeId, Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("UPDATE Employees SET FirstName = @firstName, LastName = @lastName, EmailAddress = @Email, DOB = @dob, Salary = @salary, DepartmentId = @departmentId, ModifiedDate = GETDATE() WHERE EmployeeId = @id", connection))
            {
                command.Parameters.AddWithValue("@id", employeeId);
                command.Parameters.AddWithValue("@firstName", employee.FirstName);
                command.Parameters.AddWithValue("@lastName", employee.LastName);
                command.Parameters.AddWithValue("@Email", employee.EmailAddress);
                command.Parameters.AddWithValue("@dob", employee.DOB);
                command.Parameters.AddWithValue("@salary", employee.Salary);
                command.Parameters.AddWithValue("@departmentId", employee.DepartmentId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteEmployee(int employeeId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @id", connection))
            {
                command.Parameters.AddWithValue("@id", employeeId);
                command.ExecuteNonQuery();
            }
        }
    }
}
