using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Gro_Disp_HeaderRepository : Repository<Gro_Disp_Header>,IGro_Disp_HeaderRepository
    {
        public Gro_Disp_HeaderRepository(GroDbContext context)
     : base(context)
        { }
    }
}
