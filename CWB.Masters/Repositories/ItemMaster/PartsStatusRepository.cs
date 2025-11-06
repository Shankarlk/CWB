using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Repositories.ItemMaster
{
    public class PartsStatusRepository : Repository<Domain.ItemMaster.PartsStatus>, IPartsStatusRepository
    {
        public PartsStatusRepository(MastersDbContext context)
        : base(context)
        {
        }
    }
}
