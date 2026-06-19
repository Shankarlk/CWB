using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Field_TypeRepository : Repository<Field_Type>, IField_TypeRepository
    {
        public Field_TypeRepository(GroDbContext context) : base(context)
        {

        }
    }
}
