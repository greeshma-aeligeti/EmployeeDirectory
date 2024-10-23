using EmployeeDirectory.BLL.DTOs;
using EmployeeDirectory.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.BLL.IServices
{
    public interface IEmployeeService
    {
       
            Task<Employee> AddEmployee(EmployeeDTO employeeDTO);
        Task<EmployeeDTO> GetEmployeeByID(int id);
        Task<List<EmployeeDTO>> GetSubordinatesAsync(int managerId);



    }
}
