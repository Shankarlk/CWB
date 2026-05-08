using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.Designations;
using CWB.CompanySettings.Repositories.DocType;
using CWB.CompanySettings.Repositories.EmployeeMaster;
using CWB.CompanySettings.Repositories.Location;
using CWB.CompanySettings.Services.Designations;
using CWB.CompanySettings.Services.DocType;
using CWB.CompanySettings.Services.EmployeeMaster;
using CWB.CompanySettings.Services.Location;

using Microsoft.Extensions.DependencyInjection;

namespace CWB.CompanySettings.CompanySettingsUtils
{
    public static class AppDIExtensions
    {
        public static void ConfigureAppDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddTransient<IDocumentTypeService, DocumentTypeService>();
            services.AddTransient<IPlantRepository, PlantRepository>();
            services.AddTransient<IPlantWDRepository, PlantWDRepository>();
            services.AddTransient<IHolidayRepository, HolidayRepository>();
            services.AddTransient<IPlantService, PlantService>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IDesignationRepository, DesignationRepository>();
            services.AddTransient<IDesignationService, DesignationService>();
            services.AddTransient<IEmployeeSerivce, EmployeeService>();
			services.AddTransient<IDocumentTypeService, DocumentTypeService>();
			services.AddTransient<ISectionsRepository, SectionsRepository>();
			services.AddTransient<ICityRepository, CityRepository>();
			services.AddTransient<IDept_EmployeeRepository, Dept_EmployeeRepository>();
			services.AddTransient<IEmployee_UI_ListRepository, Employee_UI_ListRepository>();
			services.AddTransient<IDept_Role_ListRepository, Dept_Role_ListRepository>();
			services.AddTransient<IEmployee_PwdRepository, Employee_PwdRepository>();
			services.AddTransient<ITimeSlotDurationRepository, TimeSlotDurationRepository>();
			services.AddTransient<INoOfDaysTimeSlotRepository, NoOfDaysTimeSlotRepository>();
			services.AddTransient<ICountryRepository, CountryRepository>();
			services.AddTransient<IUi_ListRepository, UiListRepository>();
			services.AddTransient<IOrg_ChartRepository, Org_ChartRepository>();
			services.AddTransient<IRole_ListRepository, Role_ListRepository>();
			services.AddTransient<IRole_Ui_ListRepository, Role_Ui_ListRepository>();
			services.AddTransient<IEmpl_Role_ListRepository, Empl_Role_ListRepository>();
			services.AddTransient<IPermission_ListRepository, Permission_ListRepository>();
			services.AddTransient<IEmployeeMasterRepository, EmployeeMasterRepository>();
			services.AddTransient<IStores_Dept_IDListRepository, Stores_Dept_IDListRepository>();

        }
    }
}
