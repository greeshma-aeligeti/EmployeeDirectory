using EmployeeDirectory.BLL.DTOs;
using EmployeeDirectory.BLL.IServices;
using EmployeeDirectory.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.DAL.IRepositories;

namespace EmployeeDirectory.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        public IEmployeeRepository _repository;
        public EmployeeService(IEmployeeRepository repository) {
            _repository = repository;
        
        }

        public async Task<Employee> AddEmployee(EmployeeDTO employeeDTO)
        {
            Employee _employee = new Employee
            {

                Id = employeeDTO.Id,
                Name = employeeDTO.Name,
                PhoneNumber = employeeDTO.Phone,
                EmailAddress = employeeDTO.Email,
                RoleID = employeeDTO.RoleID,
                ManagerID = employeeDTO.ManagerID,
                Path = employeeDTO.Path,
            };

          var _response=await _repository.AddEmployee(_employee);
            return _response;

          
            


            //throw new NotImplementedException();
        }

        public async Task<EmployeeDTO> GetEmployeeByID(int id)
        {
            var _employee = await _repository.GetEmployeeByID(id);
            return new EmployeeDTO
            {
                Id = _employee.Id,
                Name = _employee.Name,
                Phone = _employee.PhoneNumber,
                Email = _employee.EmailAddress,
                RoleID = _employee.RoleID,
                ManagerID = _employee.ManagerID,
                Path = _employee.Path,
            };

            //throw new NotImplementedException();
        }

        public async Task<List<EmployeeDTO>> GetSubordinatesAsync(int managerId)
        {
            var _subordinates = await _repository.GetSubordinatesAsync(managerId);
            var _resp = _subordinates.Select(_s => new EmployeeDTO
            {
                Id = _s.Id,
                Name = _s.Name,
                Phone = _s.PhoneNumber,
                Email = _s.EmailAddress,
                RoleID = _s.RoleID,
                ManagerID = _s.ManagerID,
                Path = _s.Path,

            }).ToList();
            return _resp;
            //throw new NotImplementedException();
        }
    }
}
