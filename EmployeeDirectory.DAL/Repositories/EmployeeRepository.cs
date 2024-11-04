using EmployeeDirectory.DAL.Entities;
using EmployeeDirectory.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public EmployeeDBContext _dbContext;

        public EmployeeRepository(EmployeeDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
           
            try
            {
                employee.Id = 0;
                var _resp = _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync(); 

                if (employee.ManagerID != null)
                {
                    var _manager = await GetEmployeeByID(employee.ManagerID.Value);
                    if (_manager == null)
                    {
                        throw new Exception("Manager not found");
                    }
                    employee.Path = $"{_manager.Path}/{employee.Id}";

                }
                else
                {
                    employee.Path = employee.Id.ToString();
                }
                _dbContext.Employees.Update(employee);
                await _dbContext.SaveChangesAsync(); 
                return _resp.Entity;
            }
            catch (DbUpdateException dbEx)
            {
                // Log or inspect the specific error message
                throw new Exception($"Database Update Error: {dbEx.InnerException?.Message}", dbEx);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                throw new Exception($"An error occurred while saving the employee: {ex.Message}", ex);
            }

            //throw new NotImplementedException();
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var _allEmployees=await _dbContext.Employees.ToListAsync();
            return _allEmployees;
          //  throw new NotImplementedException();
        }

        public async Task<List<int>> GetAllManagerIDs()
        {
            var _allManagerIds = await _dbContext.Employees
                  .Where(e => e.ManagerID.HasValue)
                  .Select(e => e.ManagerID.Value)
                  .Distinct()
                  .ToListAsync();
            return _allManagerIds;
        }

        public async Task<Employee> GetEmployeeByID(int id)
        {

            var _employee= await _dbContext.Employees
                             .Include(e => e.Manager)
                             .FirstOrDefaultAsync(e => e.Id == id);
            return _employee;

            // throw new NotImplementedException();
        }

        public async Task<List<Employee>> GetHigherAuthorities(int id)
        {
            var _employee=await GetEmployeeByID(id);
            if(_employee==null || string.IsNullOrEmpty(_employee.Path))
            {
                return new List<Employee>();
            }
            var _pathIds=_employee.Path.Split('/').Select(int.Parse).ToList();
            _pathIds.Remove(id);

            List<Employee> _higherEmployees = new List<Employee>();
            foreach(var  _pathId in _pathIds) {
            var _emp=await GetEmployeeByID(_pathId);
                _higherEmployees.Add(_emp);
            }
            return _higherEmployees;
            //throw new NotImplementedException();

        }

        public async Task<List<Employee>> GetNextSubordinatesAsync(int managerId)
        {
            return await _dbContext.Employees.Where(e=>e.ManagerID == managerId).ToListAsync();

        }

        public async Task<List<Employee>> GetSubordinatesAsync(int managerId)
        {

            var _manager = await GetEmployeeByID(managerId);
            if (_manager == null) return new List<Employee>();

            // Return employees whose path starts with the manager's path
            return await _dbContext.Employees
                                 .Where(e => e.Path.StartsWith(_manager.Path + "/"))
                                 .ToListAsync();
        }

        public async Task<Employee> GetRootEmployee()
        {

            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.RoleID == 5);
            //throw new NotImplementedException();
        }
    }
}
