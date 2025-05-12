using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Cust_NC_Decs_MatrixRepository : Repository<Cust_NC_Decs_Matrix>, ICust_NC_Decs_MatrixRepository
    {
        public Cust_NC_Decs_MatrixRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
