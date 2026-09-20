using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Models;

namespace ABMTech.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<OrdemServico> OrdensServico { get; set; }
    }
}