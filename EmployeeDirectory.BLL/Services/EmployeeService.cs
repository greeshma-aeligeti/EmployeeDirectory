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
        public async Task<EmployeeTreeNode> BuildFullEmployeeTree(int rootEmployeeId)
        {
            // Get the root employee (current employee)
            var _rootEmployee = await GetEmployeeByID(rootEmployeeId);
            if (_rootEmployee == null) return null;

            // Get the list of higher authorities
            var _higherAuthorities = await GetHigherAuthorities(rootEmployeeId);

            // Build the tree from the highest authority down to the root employee
            EmployeeTreeNode _topNode = null;
            EmployeeTreeNode _currentNode = null;

            foreach (var _authority in _higherAuthorities)
            {
                var _authorityNode = new EmployeeTreeNode
                {
                    Employee = _authority,
                    Subordinates = new List<EmployeeTreeNode>()
                };

                // If it's the first node, it's the top of the tree
                if (_topNode == null)
                {
                    _topNode = _authorityNode;
                }

                // Attach the previous node to the current one
                if (_currentNode != null)
                {
                    _currentNode.Subordinates.Add(_authorityNode);
                }

                // Move to the current node
                _currentNode = _authorityNode;
            }

            // Create the node for the root employee (who we passed in)
            var _rootEmployeeNode = new EmployeeTreeNode
            {
                Employee = _rootEmployee,
                Subordinates = new List<EmployeeTreeNode>()
            };

            // Attach the root employee node to the last higher authority
            if (_currentNode != null)
            {
                _currentNode.Subordinates.Add(_rootEmployeeNode);
            }
            else
            {
                // If there are no higher authorities, the root employee is the top node
                _topNode = _rootEmployeeNode;
            }

            // Recursively add all subordinates of the root employee
            await AddSubordinates(_rootEmployeeNode);

            return _topNode;
        }

        public async Task AddSubordinates(EmployeeTreeNode parentNode)
        {
            var _subordinates = await GetNextSubordinatesAsync(parentNode.Employee.Id);

            foreach (var _subordinate in _subordinates)
            {
                var _subordinateNode = new EmployeeTreeNode
                {
                    Employee = _subordinate,
                    Subordinates = new List<EmployeeTreeNode>()
                };

                // Recursively add subordinates of the current employee
                await AddSubordinates(_subordinateNode);

                // Add the subordinate node to the parent node
                parentNode.Subordinates.Add(_subordinateNode);
            }
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

        public async Task<List<int>> GetAllManagersId()
        {
            var _allManagerIds=await _repository.GetAllManagerIDs();
            return _allManagerIds.ToList();
        }

        public async Task<EmployeeDTO> GetEmployeeByID(int id)
        {
            var _employee = await _repository.GetEmployeeByID(id);
            return MapToEmployeeDTO(_employee);

            //throw new NotImplementedException();
        }

        public async Task<List<EmployeeDTO>> GetHigherAuthorities(int id)
        {
            var _higherEmployees=await _repository.GetHigherAuthorities(id);
            var _resp=_higherEmployees.Select(MapToEmployeeDTO).ToList();   
            return _resp;
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeDTO>> GetNextSubordinatesAsync(int managerId)
        {
            var _nextSubordinates = await _repository.GetNextSubordinatesAsync(managerId);
            var _resp=_nextSubordinates.Select(MapToEmployeeDTO).ToList(); return _resp;
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
