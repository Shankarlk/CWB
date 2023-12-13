using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.ItemMaster;
using CWB.Masters.Repositories.Machines;
using CWB.Masters.Repositories.OperationList;
using CWB.Masters.Repositories.Routing;
using CWB.Masters.Services.Company;
using CWB.Masters.Services.ItemMaster;
using CWB.Masters.Services.Machines;
using CWB.Masters.Services.OperationList;
using Microsoft.Extensions.DependencyInjection;

namespace CWB.Masters.MastersUtils
{
    public static class AppDIExtensions
    {
        public static void ConfigureAppDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDivisionRepository, DivisionRepository>();
            services.AddTransient<IOperationListRepository, OperationListRepository>();
            services.AddTransient<IOperationalDocumentRepository, OperationalDocumentRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IOperationListService, OperationListService>();
            services.AddTransient<IMachineTypeRepository, MachineTypeRepository>();
            services.AddTransient<IMachineRepository, MachineRepository>();
            services.AddTransient<IMachineProcessDocumentRepository, MachineProcessDocumentRepository>();
            services.AddTransient<IMachineService, MachineService>();
            services.AddTransient<IManufacturedPartNoDetailRepository, ManufacturedPartNoDetailRepository>();
            services.AddTransient<IManufacturedPartNoDetailService, ManufacturedPartNoDetailService>();
            services.AddTransient<IRawMaterialDetailRepository, RawMaterialDetailRepository>();
            services.AddTransient<IRawMaterialTypeRepository, RawMaterialTypeRespository>();
            services.AddTransient<IRawMaterialSpecRepository, RawMaterialSpecRespository>();
            services.AddTransient<IRawMaterialStandardRepository, RawMaterialStandardRespository>();
            services.AddTransient<IBaseRawMaterialRepository, BaseMaterialRespository>();
            services.AddTransient<IRawMaterialDetailService, RawMaterialDetailService>();
            services.AddTransient<IPartPurchaseDetailRepository, PartPurchaseDetailRepository>();
            services.AddTransient<IBoughtOutFinishDetailRepository, BoughtOutFinishDetailRepository>();
            services.AddTransient<IBoughtOutFinishDetailService, BoughtOutFinishDetailService>();
            services.AddTransient<IMPMakeFromRepository, MPMakeFromRepository>();
            services.AddTransient<IMPBOMRepository, MPBOMRepository>();
            services.AddTransient<IUOMRepository, UOMRepository>();
            services.AddTransient<IMasterPartService, MasterPartService>();
            services.AddTransient<IMasterPartRepository, MasterPartRepository>();
            services.AddTransient<IRoutingRepository, RoutingRepository>();
            services.AddTransient<IRoutingStepRepository, RoutingStepRepository>();
            services.AddTransient<IRoutingStepPartRepository, RoutingStepPartRepository>();
        }
    }
}
