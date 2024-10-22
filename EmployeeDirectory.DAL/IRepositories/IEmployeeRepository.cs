using EmployeeDirectory.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.DAL.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeByID(int id);
        Task<List<Employee>> GetSubordinatesAsync(int managerId);

        Task<Employee> AddEmployee(Employee employee);
    }
}
