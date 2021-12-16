using Fotoquest.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using static System.Environment;
using static System.Reflection.Assembly;

namespace Fotoquest.Api
{
    public class Program
    {
        static Program() =>
            CurrentDirectory = Path.GetDirectoryName(GetEntryAssembly().Location);

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            var configuration = BuildConfiguration(args);

            ConfigureWebHost(configuration).Build().Run();
        }

        private static IConfiguration BuildConfiguration(string[] args)
            => new ConfigurationBuilder()
                .SetBasePath(CurrentDirectory)
                .Build();

        private static IWebHostBuilder ConfigureWebHost(
            IConfiguration configuration)
            => new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(configuration)
                .UseContentRoot(CurrentDirectory)
                .UseSerilog()
                .UseKestrel();

        //public static void Main(string[] args)
        //{
        //    Log.Logger = new LoggerConfiguration()
        //        .WriteTo.Console()
        //        .MinimumLevel.Debug()
        //        .CreateLogger();


        //    var configuration = BuildConfiguration(args);

        //    var host = ConfigureWebHost(configuration).Build();
        //    using var scope = host.Services.CreateScope();
        //    var services = scope.ServiceProvider;

        //    try
        //    {
        //        var dbContext = services.GetRequiredService<FotoDbContext>();

        //        if (dbContext.Database.IsSqlServer())
        //        {
        //            dbContext.Database.Migrate();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        //        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

        //        throw;
        //    }

        //    host.Run();
        //}

        //private static IConfiguration BuildConfiguration(string[] args)
        //    => new ConfigurationBuilder()
        //        .SetBasePath(CurrentDirectory)
        //        .Build();

        //private static IWebHostBuilder ConfigureWebHost(
        //    IConfiguration configuration)
        //    => new WebHostBuilder()
        //        .UseStartup<Startup>()
        //        .UseConfiguration(configuration)
        //        .UseContentRoot(CurrentDirectory)
        //        .UseSerilog()
        //        .UseKestrel();

        //public async static Task Main(string[] args)
        //{
        //    var host = CreateHostBuilder(args).Build();

        //    using var scope = host.Services.CreateScope();
        //    var services = scope.ServiceProvider;

        //    try
        //    {
        //        var dbContext = services.GetRequiredService<FotoDbContext>();

        //        if (dbContext.Database.IsSqlServer())
        //        {
        //            dbContext.Database.Migrate();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        //        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

        //        throw;
        //    }

        //    await host.RunAsync();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }

}