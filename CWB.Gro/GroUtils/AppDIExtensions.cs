using CWB.Gro.Infrastructure;
using CWB.Gro.Repositories;
using CWB.Gro.Services;
using Microsoft.Extensions.DependencyInjection;
namespace CWB.Gro.GroUtils
{
    public static class AppDIExtensions
    {
        public static void ConfigureAppDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IGro_DataRepository, Gro_DataRepository>();
            services.AddTransient<IGro_Part_ListRepository, Gro_Part_ListRepository>();
            services.AddTransient<IGro_Disp_HeaderRepository, Gro_Disp_HeaderRepository>();
            services.AddTransient<ICust_Specific_DataRepository, Cust_Specific_DataRepository>();
            services.AddTransient<IGro_Disp_DetRepository, Gro_Disp_DetRepository>();
            services.AddTransient<IUpload_FormatRepository, Upload_FormatRepository>();
            services.AddTransient<IField_TypeRepository, Field_TypeRepository>();
            services.AddTransient<ICourier_ListRepository, Courier_ListRepository>();
            services.AddTransient<IPrintout_formatRepository, Printout_formatRepository>();
            services.AddTransient<IGro_Stock_ListRepository, Gro_Stock_ListRepository>();
            services.AddTransient<ITK_DC_Inv_ContrlRepository, TK_DC_Inv_ContrlRepository>();
            services.AddTransient<IGro_Stock_DetRepository, Gro_Stock_DetRepository>();
            services.AddTransient<ISl_No_Status_ListRepository, Sl_No_Status_ListRepository>();
            services.AddTransient<IIndent_Part_Sl_NoRepository, Indent_Part_Sl_NoRepository>();
            services.AddTransient<IGro_Indent_DispHeadRepository,Gro_Indent_DispHeadRepository>();
            services.AddTransient<IGroService, GroService>();
            
           
        }
    }
}
