using CWB.Identity.DbContext;
using CWB.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CWB.Identity.Services
{
    public class UserService
    {
        private readonly CwbIdentityDbContext _cwbIdentityDbContext;
        private readonly DbSet<CwbUser> _dbSet;

        public UserService(CwbIdentityDbContext cwbIdentityDbContext)
        {
            _cwbIdentityDbContext = cwbIdentityDbContext;
            _dbSet = _cwbIdentityDbContext.Set<CwbUser>();
        }

        public async Task<CwbUser> GetUserbyPhoneEmail(string phoneEmail)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == phoneEmail || u.PhoneNumber == phoneEmail);
        }

        public async Task<CwbUser> GetUserbyUsernameEmail(string emailUserName)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == emailUserName || u.UserName == emailUserName);
        }
    }
}
