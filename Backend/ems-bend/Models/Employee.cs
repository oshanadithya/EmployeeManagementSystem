public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }  // Adjusted to match your DB schema
    public DateTime DOB { get; set; }
    public int Age { get; set; }  // This will be fetched directly from the DB
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
