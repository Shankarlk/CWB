using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Shop_Insp_LogRepository : Repository<Shop_Insp_Log>, IShop_Insp_LogRepository
    {
        public Shop_Insp_LogRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
