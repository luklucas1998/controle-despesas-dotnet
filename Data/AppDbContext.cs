using Microsoft.EntityFrameworkCore;
using ControleDespesas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace ControleDespesas.Data
{

    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Despesa> Despesas { get; set; }
    }
}