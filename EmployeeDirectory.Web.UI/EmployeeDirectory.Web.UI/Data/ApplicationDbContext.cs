using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace EmployeeDirectory.Web.UI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Call the base class constructor
        {

        }

        // Add DbSet properties for your entities here
    }

    /* public class ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
         :IdentityDbContext<ApplicationUser>(options)
     {


     }*/
}
