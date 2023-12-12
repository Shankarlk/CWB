using CWB.Tenant.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CWB.Tenant.TenantExtensions
{
    public static class DataExtensions
    {
        public static void ConfigureAppDataEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TenantDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("AppDB"), ServerVersion.AutoDetect(configuration.GetConnectionString("AppDB")), x => x.MigrationsAssembly("CWB.Tenant"));
            });

        }
    }
}
