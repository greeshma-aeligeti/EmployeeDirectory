using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.BLL.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public int RoleID {  get; set; }
        public int? ManagerID { get; set; }
        public string Path { get; set; }


    }
}
