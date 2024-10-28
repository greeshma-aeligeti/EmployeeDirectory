using EmployeeDirectory.BLL.DTOs;
using EmployeeDirectory.DAL.Entities;

namespace EmployeeDirectory.BLL
{
    public class EmployeeTreeNode
    {
        public EmployeeDTO Employee { get; set; }
        public List<EmployeeTreeNode> Subordinates { get; set; }
    }

}
