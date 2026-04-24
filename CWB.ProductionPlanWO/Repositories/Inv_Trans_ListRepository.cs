using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inv_Trans_ListRepository: Repository<Inv_Trans_List>, IInv_Trans_ListRepository
    {
        public Inv_Trans_ListRepository(WODbContext context) : base(context)
        {

        }
    }
}
