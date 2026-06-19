using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Upload_FormatRepository : Repository<Upload_Format>, IUpload_FormatRepository
    {
        public Upload_FormatRepository(GroDbContext context)
  : base(context)
        { }
    }
}
