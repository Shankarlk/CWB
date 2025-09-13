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
    public class Dept_Role_ListRepository : Repository<Dept_Role_List>, IDept_Role_ListRepository
    {
        public Dept_Role_ListRepository(CompanySettingsDbContext context)
         : base(context)
        { }
    }
}
