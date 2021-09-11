using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Brainchild.HMS.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Brainchild.HMS.Data
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<BrainchildHMSDbContext>();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}
