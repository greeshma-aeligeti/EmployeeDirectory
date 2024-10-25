using Microsoft.AspNetCore.Identity;

namespace EmployeeDirectory.Web.UI.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
    }
}
