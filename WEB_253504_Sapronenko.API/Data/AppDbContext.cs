using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using WEB_253504_Sapronenko.Domain.Entites;

namespace WEB_253504_Sapronenko.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<DotaHero> Heroes { get; set; }
        public DbSet<WEB_253504_Sapronenko.Domain.Entites.Category> Category { get; set; } = default!;
    }
}
