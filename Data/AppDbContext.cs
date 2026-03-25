using Microsoft.EntityFrameworkCore;
using ControleDespesas.Models;

namespace ControleDespesas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Despesa> Despesas { get; set; }
    }
}