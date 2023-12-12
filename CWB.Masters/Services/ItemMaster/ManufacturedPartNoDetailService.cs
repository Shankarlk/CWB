using AutoMapper;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.ItemMaster;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.ItemMaster
{
    public class ManufacturedPartNoDetailService : IManufacturedPartNoDetailService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManufacturedPartNoDetailRepository _manufacturedPartNoDetailRepository;
        private readonly IMPMakeFromRepository _mpMakeFromRepository;
        private readonly IMPBOMRepository _mpBOMRepository;
        private readonly IUOMRepository _uOMRepository;

        public ManufacturedPartNoDetailService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IManufacturedPartNoDetailRepository manufacturedPartNoDetailRepository, IMPMakeFromRepository mpMakeFromRepository, IMPBOMRepository mpBOMRepository,IUOMRepository uomRepository
            )
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _manufacturedPartNoDetailRepository = manufacturedPartNoDetailRepository;
            _mpMakeFromRepository = mpMakeFromRepository;
            _mpBOMRepository = mpBOMRepository;
            _uOMRepository = uomRepository;
        }

        public IEnumerable<ManufacturedPartNoDetailListVM> GetManufacturedPartNoDetailsByTypeTenant(long ManufPartType, string companyName)
        {
            var manufacturedpartnodetails = _manufacturedPartNoDetailRepository.GetRangeAsync(m => m.ManufacturedPartType == ManufPartType && m.CompanyName.Equals(companyName));
            //_manufacturedPartNoDetailRepository.GetAllManuFByPartTypeTenantID(manPartTypeId, tenantID);
            //(m => m.TenantId == tenantID && m.ManufacturedPartType == manPartTypeId);
            return _mapper.Map<IEnumerable<ManufacturedPartNoDetailListVM>>(manufacturedpartnodetails);
        }
        
        public IEnumerable<UOMVM> GetUOMsByTenantId(long tenantID)
        {
            var uoms = _uOMRepository.GetRangeAsync(m => m.Id > -1);
            Console.Write(uoms.ToString());
            //_manufacturedPartNoDetailRepository.GetAllManuFByPartTypeTenantID(manPartTypeId, tenantID);
            //(m => m.TenantId == tenantID && m.ManufacturedPartType == manPartTypeId);
            return _mapper.Map<IEnumerable<UOMVM>>(uoms);
        }



        public async Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var manufacturedpartnodetail = _mapper.Map<Domain.ManufacturedPartNoDetail>(manufacturedPartNoDetailVM);
            if (manufacturedpartnodetail.Id == 0)
            {
                await _manufacturedPartNoDetailRepository.AddAsync(manufacturedpartnodetail);
            }
            else
            {
                manufacturedpartnodetail = await _manufacturedPartNoDetailRepository.UpdateAsync(manufacturedpartnodetail.Id, manufacturedpartnodetail);
            }
            await _unitOfWork.CommitAsync();
            manufacturedPartNoDetailVM.ManufacturedPartNoDetailId = manufacturedpartnodetail.Id;
            return manufacturedPartNoDetailVM;
        }

        public async Task<ManufacturedPartNoDetailVM> MPMakeFrom(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var mpMakeFrom = _mapper.Map<Domain.MPMakeFrom>(manufacturedPartNoDetailVM);
            mpMakeFrom.InputPartNo = manufacturedPartNoDetailVM.PartNumber;
            if (mpMakeFrom.Id == 0)
            {
                await _mpMakeFromRepository.AddAsync(mpMakeFrom);
            }
            else
            {
                mpMakeFrom = await _mpMakeFromRepository.UpdateAsync(mpMakeFrom.Id, mpMakeFrom);
            }
            await _unitOfWork.CommitAsync();
            manufacturedPartNoDetailVM.MPMakeFromId = mpMakeFrom.Id;
            return manufacturedPartNoDetailVM;
        }
        public IEnumerable<MPMakeFromListVM> GetMPMakeFromListByPartNumberNTenant(string partNumber, long tenantID)
        {
            var mpmakefromlist = _mpMakeFromRepository.GetRangeAsync(m => m.TenantId == tenantID && m.InputPartNo == partNumber);
            return _mapper.Map<IEnumerable<MPMakeFromListVM>>(mpmakefromlist);
        }
        public async Task<ManufacturedPartNoDetailVM> MPBOM(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var mpBOM = _mapper.Map<Domain.MPBOM>(manufacturedPartNoDetailVM);
            mpBOM.PartNumber = manufacturedPartNoDetailVM.PartNumber;
            if (mpBOM.Id == 0)
            {
                await _mpBOMRepository.AddAsync(mpBOM);
            }
            else
            {
                mpBOM = await _mpBOMRepository.UpdateAsync(mpBOM.Id, mpBOM);
            }
            try
            {
                await _unitOfWork.CommitAsync();
            }catch(Exception ex)
            {
                string str = ex.ToString();
            }
            manufacturedPartNoDetailVM.MPMakeFromId = mpBOM.Id;
            return manufacturedPartNoDetailVM;
        }
    }
}
