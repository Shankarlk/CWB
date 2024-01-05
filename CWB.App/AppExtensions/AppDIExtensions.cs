using CWB.App.Services.CompanySettings;
using CWB.App.Services.Masters;
using CWB.App.Services.Routings;
using Microsoft.Extensions.DependencyInjection;

namespace CWB.App.AppExtensions
{
    public static class AppDIExtensions
    {
        public static void ConfigureAppDI(this IServiceCollection services)
        {
            services.AddTransient<IMastersServices, MastersServices>();
            services.AddTransient<IRoutingService, RoutingService>();
            services.AddTransient<IOperationService, OperationService>();
            services.AddTransient<IMachineService, MachineService>();
            services.AddTransient<IPlantService, PlantService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IDocTypeService, DocTypeService>();
            services.AddTransient<IDesignationService, DesignationService>();
        }
    }
}
