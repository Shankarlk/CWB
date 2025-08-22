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
    public class SectionsRepository : Repository<Section>, ISectionsRepository
    {
        public SectionsRepository(CompanySettingsDbContext context)
         : base(context)
        { }
    }
}
