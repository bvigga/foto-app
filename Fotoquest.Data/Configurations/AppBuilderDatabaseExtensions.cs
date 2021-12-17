using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fotoquest.Data.Configurations
{
    public static class AppBuilderDatabaseExtensions
    {
        public static IApplicationBuilder EnsureDatabase(this IApplicationBuilder builder)
        {
            EnsureContextIsMigrated(builder.ApplicationServices.GetService<FotoDbContext>());

            return builder;
        }

        private static void EnsureContextIsMigrated(DbContext context)
        {
            //context.Database.EnsureDeleted();

            if (!context.Database.EnsureCreated())
                context.Database.Migrate();
        }

        public static IServiceCollection AddSqlServerDbContext<T>(this IServiceCollection services,
            string connectionString) where T : DbContext
        {
            services.AddDbContext<T>(options => options.UseSqlServer(connectionString));
            return services;
        }

        //public static IServiceCollection AddPostgresDbContext<T>(this IServiceCollection services,
        //    string connectionString) where T : DbContext
        //{
        //    services.AddDbContext<T>(options => options.UseNpgsql(connectionString));
        //    return services;
        //}
    }
}
