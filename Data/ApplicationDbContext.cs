using Microsoft.EntityFrameworkCore;
using EmployeeSystem.Models;

namespace EmployeeSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option)
        {
        }

        public DbSet<Employee> Employees { get; set;}
    }
}