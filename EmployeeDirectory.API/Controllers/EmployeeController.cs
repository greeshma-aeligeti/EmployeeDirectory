using EmployeeDirectory.BLL.DTOs;
using EmployeeDirectory.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDirectory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        public IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<EmployeeDTO>> AddEmployee([FromBody]EmployeeDTO employeeDTO)
        {
            var _addEmployee=await _employeeService.AddEmployee(employeeDTO);
            return Ok(_addEmployee);
        }
    }
}
