using Microsoft.EntityFrameworkCore;
using SmartTodoAPI.Models;

namespace SmartTodoAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        
    }
}
