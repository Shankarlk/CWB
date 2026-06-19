using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;
namespace CWB.Gro.Repositories
{
    public class Gro_DataRepository:Repository<Gro_Data>,IGro_DataRepository
    {
        public Gro_DataRepository(GroDbContext context)
      : base(context)
        { }
    }
}
