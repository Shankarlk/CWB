using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Cont_RCA_CA_Status_ListReposiotry : Repository<Cont_RCA_CA_Status_List>, ICont_RCA_CA_Status_ListRepository
    {
        public Cont_RCA_CA_Status_ListReposiotry(WODbContext context)
       : base(context)
        {

        }
    }
}
