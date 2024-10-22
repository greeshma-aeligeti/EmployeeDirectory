using EmployeeDirectory.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.DAL
{
    public class EmployeeDBContext:DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\mssqllocaldb; Initial Catalog=EmployeeDatabase; Integrated Security=true ").ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

            base.OnConfiguring(optionsBuilder);
 
            //base.OnConfiguring(optionsBuilder);
        }


        public DbSet<EmployeeType> EmployeeTypes { get; set; }  
        public DbSet<Employee> Employees { get; set; }  

    }
}
