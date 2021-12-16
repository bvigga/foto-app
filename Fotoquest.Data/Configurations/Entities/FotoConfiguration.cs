using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fotoquest.Data.Configurations.Entities
{
    public class FotoConfiguration : IEntityTypeConfiguration<Foto>
    {
        public void Configure(EntityTypeBuilder<Foto> builder)
        {
            builder.HasData(
                new Foto
                {
                    Id = Guid.NewGuid(),
                    GeoDirection = "North"
                }

            );
        }
    }
}
