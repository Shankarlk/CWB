using CWB.CompanySettings.Domain;
using CWB.CompanySettings.Infrastructure;
using CWB.CommonUtils.Common.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Repositories.Location
{
    public class Stores_Dept_IDListRepository:Repository<Stores_Dept_IDList>, IStores_Dept_IDListRepository
    {
        public Stores_Dept_IDListRepository(CompanySettingsDbContext context)
        : base(context)
        { }
    }
}
