using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Timeslot_SettingRepository : Repository<Timeslot_Setting>, ITimeslot_SettingRepository
    {
        public Timeslot_SettingRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
