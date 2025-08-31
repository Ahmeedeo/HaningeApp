using Microsoft.EntityFrameworkCore;
using HaningeServer.Models;

namespace HaningeServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }

}
