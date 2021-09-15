
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brainchild.HMS.Data.Context;
namespace Brainchild.HMS.Data
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BrainchildHMSDbContext>
    {
        public BrainchildHMSDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine(Directory.GetCurrentDirectory());
            // Build config
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Brainchild.HMS.Data"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var builder = new DbContextOptionsBuilder<BrainchildHMSDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString, option =>
            {
                option.MigrationsAssembly("Brainchild.HMS.Data");
            });
            return new BrainchildHMSDbContext(builder.Options);

            //var optionsBuilder = new DbContextOptionsBuilder<PlanHubDataContext>();
            //optionsBuilder.UseSqlServer("Data Source=planhub.db");

            //return new PlanHubDataContext(optionsBuilder.Options);
        }
    }
}
