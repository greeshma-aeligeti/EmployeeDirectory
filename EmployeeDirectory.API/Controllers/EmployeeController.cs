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
        public async Task<ActionResult<EmployeeDTO>> AddEmployee([FromBody] EmployeeDTO employeeDTO)
        {
          
            var _addEmployee=await _employeeService.AddEmployee(employeeDTO);
            return Ok(_addEmployee);
        }

        [HttpGet]
        [Route("GetSubordinates")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllSubordinates(int managerId)
        {
            var _subordinates=await _employeeService.GetSubordinatesAsync(managerId);
            return Ok(_subordinates);
        }
        [HttpGet]
        [Route("AllEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            var _allEmployees = await _employeeService.GetAllEmployees();
            return Ok(_allEmployees);
        }

        [HttpGet("Employee/{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            var _employee = await _employeeService.GetEmployeeByID(id);
            return Ok(_employee);
        }
       
    }
}
