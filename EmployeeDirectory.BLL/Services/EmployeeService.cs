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
            Employee _employee = MapToEmployee(employeeDTO);

            var _response=await _repository.AddEmployee(_employee);
            return _response;

          
            


            //throw new NotImplementedException();
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            var _allEmployees=await _repository.GetAllEmployees();
                return _allEmployees.Select(MapToEmployeeDTO).ToList();
           // throw new NotImplementedException();
        }

        public async Task<EmployeeDTO> GetEmployeeByID(int id)
        {
            var _employee = await _repository.GetEmployeeByID(id);
            return MapToEmployeeDTO(_employee);

            //throw new NotImplementedException();
        }

        public async Task<List<EmployeeDTO>> GetSubordinatesAsync(int managerId)
        {
            var _subordinates = await _repository.GetSubordinatesAsync(managerId);
            var _resp = _subordinates.Select(MapToEmployeeDTO
            ).ToList();
            return _resp;
            //throw new NotImplementedException();
        }

        private Employee MapToEmployee(EmployeeDTO employeeDTO)
        {
            return new Employee
            {
                Id = employeeDTO.Id,
                Name = employeeDTO.Name,
                PhoneNumber = employeeDTO.Phone,
                EmailAddress = employeeDTO.Email,
                RoleID = employeeDTO.RoleID,
                ManagerID = employeeDTO.ManagerID,
                Path = employeeDTO.Path
            };
        }

        private EmployeeDTO MapToEmployeeDTO(Employee employee)
        {
            return new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Phone = employee.PhoneNumber,
                Email = employee.EmailAddress,
                RoleID = employee.RoleID,
                ManagerID = employee.ManagerID,
                Path = employee.Path
            };
        }

    }
}
