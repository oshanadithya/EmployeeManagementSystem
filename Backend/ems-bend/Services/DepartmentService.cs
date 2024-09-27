using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
// using Microsoft.Data.SqlClient; // Correct namespace for SqlConnection

public class DepartmentService
{
    private readonly string _connectionString;

    public DepartmentService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Department> GetDepartments()
    {
        var departments = new List<Department>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT * FROM Departments", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentId = reader.GetInt32(0),
                            DepartmentCode = reader.GetString(1),
                            DepartmentName = reader.GetString(2),
                            CreatedDate = reader.GetDateTime(3),
                            ModifiedDate = reader.GetDateTime(4)
                        });
                    }
                }
            }
        }

        return departments;
    }

    public void AddDepartment(Department department)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("INSERT INTO Departments (DepartmentCode, DepartmentName, CreatedDate, ModifiedDate) VALUES (@code, @name, GETDATE(), GETDATE())", connection))
            {
                command.Parameters.AddWithValue("@code", department.DepartmentCode);
                command.Parameters.AddWithValue("@name", department.DepartmentName);
                command.ExecuteNonQuery();
            }
        }
    }

    public void EditDepartment(int departmentId, Department department)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("UPDATE Departments SET DepartmentCode = @code, DepartmentName = @name, ModifiedDate = GETDATE() WHERE DepartmentId = @id", connection))
            {
                command.Parameters.AddWithValue("@id", departmentId);
                command.Parameters.AddWithValue("@code", department.DepartmentCode);
                command.Parameters.AddWithValue("@name", department.DepartmentName);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteDepartment(int departmentId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("DELETE FROM Departments WHERE DepartmentId = @id", connection))
            {
                command.Parameters.AddWithValue("@id", departmentId);
                command.ExecuteNonQuery();
            }
        }
    }
}
