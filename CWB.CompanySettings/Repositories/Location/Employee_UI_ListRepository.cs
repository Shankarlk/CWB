using CWB.CommonUtils.Common.Repositories;
using CWB.CompanySettings.Domain;
using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.Designations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CWB.CompanySettings.CompanySettingsUtils.ApiRoutes;

namespace CWB.CompanySettings.Repositories.Location
{
    public class Employee_UI_ListRepository : Repository<Employee_UI_List>, IEmployee_UI_ListRepository
    {
        public Employee_UI_ListRepository(CompanySettingsDbContext context)
         : base(context)
        { }
    }
}
