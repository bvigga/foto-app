using Fotoquest.Data.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fotoquest.Data
{
    public class FotoDbContext : IdentityDbContext<ApiUser>
    {
        public FotoDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Foto> Fotos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new FotoConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
