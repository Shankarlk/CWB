using AutoMapper;
using CWB.Logging;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.ItemMaster;
using CWB.Masters.ViewModels.ItemMaster;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.ItemMaster
{
    public class RawMaterialDetailService : IRawMaterialDetailService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRawMaterialDetailRepository _rawMaterialDetailRepository;

        public RawMaterialDetailService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IRawMaterialDetailRepository rawMaterialDetailRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _rawMaterialDetailRepository = rawMaterialDetailRepository;
        }

        public IEnumerable<RawMaterialDetailListVM> GetRawMaterialDetailsByTenant(long tenantID)
        {
            var rawmaterialdetails = _rawMaterialDetailRepository.GetRangeAsync(m => m.TenantId == tenantID);
            return _mapper.Map<IEnumerable<RawMaterialDetailListVM>>(rawmaterialdetails);
        }

        public async Task<RawMaterialDetailVM> RawMaterialDetail(RawMaterialDetailVM rawMaterialDetailVM)
        {
            var rawmaterialdetail = _mapper.Map<Domain.RawMaterialDetail>(rawMaterialDetailVM);
            if (rawmaterialdetail.Id == 0)
            {
                await _rawMaterialDetailRepository.AddAsync(rawmaterialdetail);
            }
            else
            {
                rawmaterialdetail = await _rawMaterialDetailRepository.UpdateAsync(rawmaterialdetail.Id, rawmaterialdetail);
            }
            await _unitOfWork.CommitAsync();
            rawMaterialDetailVM.RawMaterialDetailId = rawmaterialdetail.Id;
            return rawMaterialDetailVM;
        }
    }
}
