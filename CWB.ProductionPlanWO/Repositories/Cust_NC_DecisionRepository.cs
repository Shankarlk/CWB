using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Cust_NC_DecisionRepository : Repository<Cust_NC_Decision>,ICust_NC_DecisionRepository
    {
        public Cust_NC_DecisionRepository(WODbContext context):base(context)
        {

        }
    }
}
