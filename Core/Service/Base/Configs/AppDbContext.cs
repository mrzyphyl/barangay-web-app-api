using Core.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Service.Base.Configs
{
    public class AppDbContext : DbContext {

        public DbSet<User> Users { get; set; }
    }
}
