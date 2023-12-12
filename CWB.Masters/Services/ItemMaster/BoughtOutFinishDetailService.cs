using AutoMapper;
using CWB.Logging;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.ItemMaster;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Services.ItemMaster
{
    public class BoughtOutFinishDetailService : IBoughtOutFinishDetailService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBoughtOutFinishDetailRepository _boughtOutFinishDetailRepository;


        public BoughtOutFinishDetailService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IBoughtOutFinishDetailRepository boughtOutFinishDetailRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _boughtOutFinishDetailRepository = boughtOutFinishDetailRepository;

        }
        public IEnumerable<BoughtOutFinishDetailListVM> GetBoughtOutFinishDetailsByTenant(long tenantID)
        {
            var boughtoutfinishdetails = _boughtOutFinishDetailRepository.GetRangeAsync(m => m.TenantId == tenantID);         
            return _mapper.Map<IEnumerable<BoughtOutFinishDetailListVM>>(boughtoutfinishdetails);
        }
        public async Task<BoughtOutFinishDetailVM> BoughtOutFinishDetail(BoughtOutFinishDetailVM boughtOutFinishDetailVM)
        {
            var boughtoutfinishdetail = _mapper.Map<Domain.BoughtOutFinishDetail>(boughtOutFinishDetailVM);
            if (boughtoutfinishdetail.Id == 0)
            {
                await _boughtOutFinishDetailRepository.AddAsync(boughtoutfinishdetail);
            }
            else
            {
                boughtoutfinishdetail = await _boughtOutFinishDetailRepository.UpdateAsync(boughtoutfinishdetail.Id, boughtoutfinishdetail);
            }
            await _unitOfWork.CommitAsync();
            boughtOutFinishDetailVM.BoughtOutFinishDetailId = boughtoutfinishdetail.Id;
            return boughtOutFinishDetailVM;
        }
    }
}
