using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Cust_NC_Decs_Matrix_OptRepository : Repository<Cust_NC_Decs_Matrix_Opt>, ICust_NC_Decs_Matrix_OptRepositoy
    {
        public Cust_NC_Decs_Matrix_OptRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
