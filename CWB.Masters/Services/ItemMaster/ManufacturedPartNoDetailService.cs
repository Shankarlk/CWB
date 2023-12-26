using AutoMapper;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.ItemMaster;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CWB.Masters.Domain.ItemMaster;

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
        private readonly IMasterPartRepository _masterPartRepository;

        public ManufacturedPartNoDetailService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IManufacturedPartNoDetailRepository manufacturedPartNoDetailRepository
            , IMPMakeFromRepository mpMakeFromRepository
            , IMPBOMRepository mpBOMRepository
            ,IUOMRepository uomRepository
            , IMasterPartRepository masterPartRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _manufacturedPartNoDetailRepository = manufacturedPartNoDetailRepository;
            _mpMakeFromRepository = mpMakeFromRepository;
            _mpBOMRepository = mpBOMRepository;
            _uOMRepository = uomRepository;
            _masterPartRepository = masterPartRepository;
        }

        public IEnumerable<ManufacturedPartNoDetailVM> GetManufacturedPartNoDetailsByTypeTenant(long ManufPartType, string companyName, long tenantID)
        {
            var manufacturedpartnodetails = _manufacturedPartNoDetailRepository.GetRangeAsync(m => m.ManufacturedPartType == ManufPartType && Convert.ToString(m.CompanyId).Equals(companyName) && m.TenantId == tenantID);
            //_manufacturedPartNoDetailRepository.GetAllManuFByPartTypeTenantID(manPartTypeId, tenantID);
            //(m => m.TenantId == tenantID && m.ManufacturedPartType == manPartTypeId);
            return _mapper.Map<IEnumerable<ManufacturedPartNoDetailVM>>(manufacturedpartnodetails);
        }

        public IEnumerable<ManufacturedPartNoDetailVM> GetAllManufacturedPartNoDetailsByTypeTenant(long tenantID)
        {
            var manufacturedpartnodetails = _manufacturedPartNoDetailRepository.GetRangeAsync(m => m.TenantId == tenantID);
            //_manufacturedPartNoDetailRepository.GetAllManuFByPartTypeTenantID(manPartTypeId, tenantID);
            //(m => m.TenantId == tenantID && m.ManufacturedPartType == manPartTypeId);
            return _mapper.Map<IEnumerable<ManufacturedPartNoDetailVM>>(manufacturedpartnodetails);
        }

        public IEnumerable<UOMVM> GetUOMsByTenantId(long tenantID)
        {
            var uoms = _uOMRepository.GetRangeAsync(m => m.Id > -1);
            Console.Write(uoms.ToString());
            //_manufacturedPartNoDetailRepository.GetAllManuFByPartTypeTenantID(manPartTypeId, tenantID);
            //(m => m.TenantId == tenantID && m.ManufacturedPartType == manPartTypeId);
            return _mapper.Map<IEnumerable<UOMVM>>(uoms);
        }

        private int GetPartId(string partNo)
        {
            var rms = _masterPartRepository.GetRangeAsync(c => c.PartNo.Equals(partNo));
            if (!rms.Any())
            {
                return 0;
            }
            MasterPart part = _mapper.Map<Domain.ItemMaster.MasterPart>(rms.First());
            return (int)part.Id;
        }

        private async Task<string> GetPartNo(int partId)
        {
            var rms = await _masterPartRepository.SingleOrDefaultAsync(c => c.Id == partId);
            if (rms == null)
            {
                return "-NA-";
            }
            return rms.PartNo;
        }


        public async Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            try
            {
                var manufacturedpartnodetail = _mapper.Map<Domain.ManufacturedPartNoDetail>(manufacturedPartNoDetailVM);
                var masterPart = _mapper.Map<Domain.ItemMaster.MasterPart>(manufacturedPartNoDetailVM);
                int id = GetPartId(masterPart.PartNo);
                manufacturedpartnodetail.PartId = id;
                manufacturedPartNoDetailVM.PartId = id;
                if (id == 0)
                {
                    masterPart.Id = 0;
                    await _masterPartRepository.AddAsync(masterPart);
                    await _unitOfWork.CommitAsync();
                    manufacturedpartnodetail.PartId = (int)masterPart.Id;
                    manufacturedPartNoDetailVM.PartId = (int)masterPart.Id;
                }
                else
                {
                    if (id == manufacturedpartnodetail.PartId)
                    {
                        masterPart = await _masterPartRepository.UpdateAsync(masterPart.Id, masterPart);
                        await _unitOfWork.CommitAsync();
                    }
                }

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
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.Message;
                throw ex;
            }
            return manufacturedPartNoDetailVM;
        }

        public async Task<MPMakeFromVM> MPMakeFrom(MPMakeFromVM mpMakeFromVM)
        {
            var mpMakeFrom = _mapper.Map<Domain.MPMakeFrom>(mpMakeFromVM);
           // mpMakeFrom.InputPartNo = manufacturedPartNoDetailVM.PartNumber;
            if (mpMakeFrom.Id == 0)
            {
                try {
                    _mpMakeFromRepository.AddObj(mpMakeFrom);
                } catch(Exception ex)
                {
                    var msg = ex.InnerException.Message;
                    ex.GetBaseException();
                    ex.GetType();
                    

                }
            }
            else
            {
                mpMakeFrom = await _mpMakeFromRepository.UpdateAsync(mpMakeFrom.Id, mpMakeFrom);
            }
            await _unitOfWork.CommitAsync();
            mpMakeFromVM.MPMakeFromId = mpMakeFrom.Id;
            return mpMakeFromVM;
        }
        public IEnumerable<MPMakeFromVM> GetMPMakeFromList(string manufPartId, long tenantID)
        {
            var mpmakefromlist =  _mpMakeFromRepository.GetRangeAsync(m => m.PartId.ToString().Equals(manufPartId));
            return _mapper.Map<IEnumerable<MPMakeFromVM>>(mpmakefromlist);
        }


        public async Task<MPMakeFromVM> GetMPMakeFrom(long Id)
        {
            var mpmakefromlist = await _mpMakeFromRepository.SingleOrDefaultAsync(m => m.Id == Id);
            return _mapper.Map<MPMakeFromVM>(mpmakefromlist);
        }

     

       
        public async Task<MPBOMVM> GetMPBOM(long Id)
        {
            var mpmakefromlist = await _mpBOMRepository.SingleOrDefaultAsync(m => m.Id == Id);
            MPBOMVM mpbomvm = _mapper.Map<MPBOMVM>(mpmakefromlist);
            var mpart = await _masterPartRepository.SingleOrDefaultAsync(m => m.Id == mpbomvm.BOMPartId);
            MasterPartVM mpvm = _mapper.Map<MasterPartVM>(mpart);
            mpbomvm.BOMPartNo = mpvm.PartNo;
            return mpbomvm;
        }
        public IEnumerable<MPBOMVM> GetMPBOMList(string manufPartId,long tenantId)
        {
            
            var mpmakefromlist = _mpBOMRepository.GetRangeAsync(m => m.ManufPartId.ToString().Equals(manufPartId)).ToList();
            var idquery = from mpm in mpmakefromlist
                          select mpm.PartId;
            List<int> ids = idquery.ToList();
            var mps = _masterPartRepository.GetRangeAsync(m => ids.Contains((int)m.Id)).ToList();
            var query = from mpm in mpmakefromlist
                        join mpj in mps on mpm.PartId equals mpj.Id into rmjoin
                        from rmj in rmjoin
                        select new MPBOMVM
                        {
                            BOMManufPartId = mpm.ManufPartId,
                            BOMPartId = mpm.PartId,
                            MPBOMId = mpm.Id,
                            BOMPartDesc = mpm.PartDesc,
                            BOMPartNo = rmj.PartNo,
                            Quantity = mpm.Quantity
                        };
            return query.ToList();
        }
        public async Task<MPBOMVM> MPBOM(MPBOMVM mpBovm)
        {
            var mpBOM = _mapper.Map<Domain.MPBOM>(mpBovm);
        //    mpBOM.PartNumber = manufacturedPartNoDetailVM.PartNumber;
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
            mpBovm.MPBOMId = mpBOM.Id;
            return mpBovm;
        }

        public Task<UOMVM> UOM(UOMVM uOMVm)
        {
            if(uOMVm.UOMId == 0)
            {

            }
            else
            {

            }
            throw new NotImplementedException();
        }

        public bool CheckPartNo(long partId)
        {
            var manufPs = _manufacturedPartNoDetailRepository.GetRangeAsync(c => c.PartId == (partId));
            if (!manufPs.Any())
            {
                return false;
            }
            return true;
        }

        public async Task<MPMakeFromVM> RemMakeFrom(MPMakeFromVM mPMakeFromListVM)
        {
            var mpMakeFrom = _mapper.Map<Domain.MPMakeFrom>(mPMakeFromListVM);
            if (mpMakeFrom.Id > 0)
            {
                var dbc_mpMakeFrom = await _mpMakeFromRepository.SingleOrDefaultAsync(m => m.Id == mpMakeFrom.Id);
                if (dbc_mpMakeFrom != null)
                {
                    if (dbc_mpMakeFrom.Id > 0)
                    {
                        _mpMakeFromRepository.Remove(dbc_mpMakeFrom);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            return mPMakeFromListVM; 
        }

        public async Task<MPBOMVM> RemBOM(MPBOMVM mpBOMVM)
        {
            var mpBOM = _mapper.Map<Domain.MPBOM>(mpBOMVM);
            if (mpBOM.Id > 0)
            {
                
                var dbc_mpBOM = await _mpBOMRepository.SingleOrDefaultAsync(m => m.Id == mpBOM.Id);
                if (dbc_mpBOM != null)
                {
                    if (dbc_mpBOM.Id > 0)
                    {
                        _mpBOMRepository.Remove(dbc_mpBOM);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            return mpBOMVM;
        }

        public async Task<ManufacturedPartNoDetailVM> GetManuPart(int partId, long tenantId)
        {
            var part = await  _manufacturedPartNoDetailRepository.SingleOrDefaultAsync(m=>m.PartId == partId && m.TenantId == tenantId);
            if(part != null)
            {
                return _mapper.Map<ManufacturedPartNoDetailVM>(part);
            }
            return new ManufacturedPartNoDetailVM { ManufacturedPartNoDetailId = -1 };
        }
    }
}
