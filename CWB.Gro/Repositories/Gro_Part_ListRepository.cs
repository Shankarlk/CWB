using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Gro_Part_ListRepository:Repository<Gro_Part_List>,IGro_Part_ListRepository
    {
        public Gro_Part_ListRepository(GroDbContext context)
    : base(context)
        { }
    }
}
