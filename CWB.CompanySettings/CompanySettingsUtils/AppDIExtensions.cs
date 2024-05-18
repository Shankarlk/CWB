﻿using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.Designations;
using CWB.CompanySettings.Repositories.DocType;
using CWB.CompanySettings.Repositories.Location;
using CWB.CompanySettings.Services.Designations;
using CWB.CompanySettings.Services.DocType;
using CWB.CompanySettings.Services.Location;
using CWB.CompanySettings.Repositories.BusinessAquisition;

using Microsoft.Extensions.DependencyInjection;
using CWB.CompanySettings.Services.BusinessAquisition;

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
			services.AddTransient<IDocumentTypeService, DocumentTypeService>();
            services.AddTransient<IPOLogRepository, POLogRepository>();
            services.AddTransient<ISalesOrderRepository, SalesOrderRepository>();
            services.AddTransient<ICustomerOrderRepository, CustomerOderRepository>();
            services.AddTransient<ISOAggregateRepository, SOAggregateRepository>();
            services.AddTransient<IDeliveryScheduleRepository, DeliveryScheduleRepository>();
            services.AddTransient<IBAService, BAService>();

        }
    }
}