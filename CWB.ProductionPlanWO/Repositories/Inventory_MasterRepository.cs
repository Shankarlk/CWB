using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inventory_MasterRepository : Repository<Inventory_Master>, IInventory_MasterRepository
    {
        public Inventory_MasterRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
