using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.DAL.Entities
{
    public class Employee //Its public as we need to access it outside of our project, as internal can accessed only inside own project

    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(RoleOfEmployee))]
        public int RoleID {  get; set; }
        public EmployeeType RoleOfEmployee { get; set; }

        public string Path {  get; set; }

        [ForeignKey(nameof(Manager))]
        public int? ManagerID { get; set; }
        public Employee Manager { get; set; }

        public ICollection<Employee> Subordinates { get; set; }



    }
}
