using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult GetEmployees()
    {
        var employees = _employeeService.GetEmployees();
        return Ok(employees);
    }

    [HttpPost]
    public IActionResult AddEmployee([FromBody] Employee employee)
    {
        _employeeService.AddEmployee(employee);
        return CreatedAtAction(nameof(GetEmployees), new { id = employee.EmployeeId }, employee);
    }

    [HttpPut("{id}")]
    public IActionResult EditEmployee(int id, [FromBody] Employee employee)
    {
        _employeeService.EditEmployee(id, employee);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        _employeeService.DeleteEmployee(id);
        return NoContent();
    }
}
