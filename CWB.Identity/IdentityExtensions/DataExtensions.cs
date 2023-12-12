using CWB.Identity.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CWB.Identity.IdentityExtensions
{
    public static class DataExtensions
    {
        public static void ConfigureAppDataEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CwbIdentityDbContext>(options =>
            {


                options.UseMySql(configuration.GetConnectionString("AppDB"), ServerVersion.AutoDetect(configuration.GetConnectionString("AppDB")), x => x.MigrationsAssembly("CWB.Identity"));
            });

        }
    }
}
