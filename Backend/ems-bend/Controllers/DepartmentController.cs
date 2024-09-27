using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly DepartmentService _departmentService;

    public DepartmentController(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public IActionResult GetDepartments()
    {
        var departments = _departmentService.GetDepartments();
        return Ok(departments);
    }

    [HttpPost]
    public IActionResult AddDepartment([FromBody] Department department)
    {
        _departmentService.AddDepartment(department);
        return CreatedAtAction(nameof(GetDepartments), new { id = department.DepartmentId }, department);
    }

    [HttpPut("{id}")]
    public IActionResult EditDepartment(int id, [FromBody] Department department)
    {
        _departmentService.EditDepartment(id, department);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDepartment(int id)
    {
        _departmentService.DeleteDepartment(id);
        return NoContent();
    }
}
