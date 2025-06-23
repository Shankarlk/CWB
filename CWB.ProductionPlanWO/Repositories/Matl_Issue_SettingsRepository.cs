using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Matl_Issue_SettingsRepository : Repository<Matl_Issue_Settings>, IMatl_Issue_SettingsRepository
    {
        public Matl_Issue_SettingsRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
