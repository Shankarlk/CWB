using System.Threading.Tasks;

namespace CWB.Gro.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
        int Commit();
    }
}
