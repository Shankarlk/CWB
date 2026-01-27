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
        private readonly IPartStatusChangeLogRepository _partStatusChangeLogRepository;

        public ManufacturedPartNoDetailService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IManufacturedPartNoDetailRepository manufacturedPartNoDetailRepository
            , IMPMakeFromRepository mpMakeFromRepository
            , IMPBOMRepository mpBOMRepository
            ,IUOMRepository uomRepository
            , IMasterPartRepository masterPartRepository, IPartStatusChangeLogRepository partStatusChangeLogRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _manufacturedPartNoDetailRepository = manufacturedPartNoDetailRepository;
            _mpMakeFromRepository = mpMakeFromRepository;
            _mpBOMRepository = mpBOMRepository;
            _uOMRepository = uomRepository;
            _masterPartRepository = masterPartRepository;
            _partStatusChangeLogRepository = partStatusChangeLogRepository;
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
            long? createdMasterPartId = null;
            long? createdStatusLogId = null;
            long? createdMPNDetailId = null;

            try
            {
                var manufacturedpartnodetail = _mapper.Map<ManufacturedPartNoDetail>(manufacturedPartNoDetailVM);
                var masterPart = _mapper.Map<Domain.ItemMaster.MasterPart>(manufacturedPartNoDetailVM);

                int id = GetPartId(masterPart.PartNo);
                manufacturedpartnodetail.PartId = id;
                manufacturedPartNoDetailVM.PartId = id;

                if (id == 0)
                {
                    // NEW MASTER PART
                    masterPart.Id = 0;
                    //masterPart.Status = "Not Released";
                    masterPart.Inv_Trans = 'N';
                    masterPart.Linked_to_BOM = 'N';

                    await _masterPartRepository.AddAsync(masterPart);
                    await _unitOfWork.CommitAsync(); // need ID from DB
                    createdMasterPartId = masterPart.Id;

                    manufacturedpartnodetail.PartId = (int)masterPart.Id;
                    manufacturedPartNoDetailVM.PartId = (int)masterPart.Id;

                    PartStatusChangeLog partStatus = new PartStatusChangeLog()
                    {
                        MasterPartId = masterPart.Id,
                        Status = masterPart.Status,
                        ChangeReason = masterPart.StatusChangeReason,
                        TenantId = masterPart.TenantId
                    };
                    await _partStatusChangeLogRepository.AddAsync(partStatus);
                    await _unitOfWork.CommitAsync();
                    createdStatusLogId = partStatus.Id;
                }
                else
                {
                    // EXISTING MASTER PART
                    if (id == manufacturedpartnodetail.PartId)
                    {
                        var findmp = await _masterPartRepository.SingleOrDefaultAsync(s => s.Id == masterPart.Id);

                        PartStatusChangeLog partStatus = new PartStatusChangeLog()
                        {
                            MasterPartId = masterPart.Id,
                            Status = masterPart.Status,
                            FromChangedStatus = findmp.Status.ToString(),
                            ChangeReason = masterPart.StatusChangeReason,
                            TenantId = masterPart.TenantId
                        };

                        await _partStatusChangeLogRepository.AddAsync(partStatus);
                        masterPart = await _masterPartRepository.UpdateAsync(masterPart.Id, masterPart);
                        await _unitOfWork.CommitAsync();

                        // if you also want to rollback this status log on failure:
                        createdStatusLogId = partStatus.Id;
                    }
                }

                //throw new Exception("TEST: Rollback Simulation");
                // MPN DETAIL
                if (manufacturedpartnodetail.Id == 0)
                {
                    await _manufacturedPartNoDetailRepository.AddAsync(manufacturedpartnodetail);
                    await _unitOfWork.CommitAsync();
                    createdMPNDetailId = manufacturedpartnodetail.Id;
                }
                else
                {
                    manufacturedpartnodetail = await _manufacturedPartNoDetailRepository
                        .UpdateAsync(manufacturedpartnodetail.Id, manufacturedpartnodetail);
                    await _unitOfWork.CommitAsync();
                }

                manufacturedPartNoDetailVM.ManufacturedPartNoDetailId = manufacturedpartnodetail.Id;
            }
            catch (Exception ex)
            {
                // avoid NullReferenceException here
                string msg = ex.InnerException?.Message ?? ex.Message;

                // ROLLBACK IN REVERSE ORDER OF CREATION

                // 1. Remove ManufacturedPartNoDetail if it was created
                if (createdMPNDetailId.HasValue)
                {
                    // ❌ you searched with createdMasterPartId earlier
                    var mpn = await _manufacturedPartNoDetailRepository
                        .SingleOrDefaultAsync(s => s.Id == createdMPNDetailId.Value);

                    if (mpn != null)
                    {
                        _manufacturedPartNoDetailRepository.Remove(mpn);
                        await _unitOfWork.CommitAsync();
                    }
                }

                // 2. Remove StatusLog if it was created
                if (createdStatusLogId.HasValue)
                {
                    var statusLog = await _partStatusChangeLogRepository
                        .SingleOrDefaultAsync(s => s.Id == createdStatusLogId.Value);

                    if (statusLog != null)
                    {
                        _partStatusChangeLogRepository.Remove(statusLog);
                        await _unitOfWork.CommitAsync();
                    }
                }

                // 3. Remove newly created MasterPart if it was created
                if (createdMasterPartId.HasValue)
                {
                    var mp = await _masterPartRepository
                        .SingleOrDefaultAsync(s => s.Id == createdMasterPartId.Value);

                    if (mp != null)
                    {
                        _masterPartRepository.Remove(mp);
                        await _unitOfWork.CommitAsync();
                    }
                }

                // VERY IMPORTANT: rethrow so caller knows it failed
                throw;
            }

            return manufacturedPartNoDetailVM;
        }

        public async Task<IEnumerable<PartStatusChangeLogVM>> GetPartStatusChangelog(long tenantId)
        {
            var allDocuType = _partStatusChangeLogRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<PartStatusChangeLogVM>>(allDocuType);
        }

        public async Task<MPMakeFromVM> MPMakeFrom(MPMakeFromVM mpMakeFromVM)
        {
            var mpMakeFrom = _mapper.Map<MPMakeFrom>(mpMakeFromVM);
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
            var mpmakefromlist =  _mpMakeFromRepository.GetRangeAsync(m => m.ManufPartId.ToString().Equals(manufPartId));
            return _mapper.Map<IEnumerable<MPMakeFromVM>>(mpmakefromlist);
        }
        public IEnumerable<MPMakeFromVM> GetAllMPMakeFromList(long tenantID)
        {
            var mpmakefromlist =  _mpMakeFromRepository.GetRangeAsync(m => m.TenantId.Equals(tenantID));
            return _mapper.Map<IEnumerable<MPMakeFromVM>>(mpmakefromlist);
        }


        public async Task<MPMakeFromVM> PreferredInputMatl(MPMakeFromVM mPMakeFromVM)
        {
            var findmk = _mpMakeFromRepository.SingleOrDefaultAsync(f=>f.Id ==mPMakeFromVM.MPMakeFromId);
            var mPMakeFrom = _mapper.Map<MPMakeFrom>(findmk.Result);
            if (mPMakeFrom.Id != 0)
            {
                List<MPMakeFromVM> lst = GetMPMakeFromList(mPMakeFrom.ManufPartId.ToString(), (int)mPMakeFrom.TenantId).ToList();
                foreach (MPMakeFromVM rv in lst)
                {
                    var lRouting = _mapper.Map<MPMakeFrom>(rv);
                    lRouting.PreferedRawMaterial = false;
                    await _mpMakeFromRepository.UpdateAsync(lRouting.Id, lRouting);
                }
                mPMakeFrom.PreferedRawMaterial = true;
                mPMakeFrom = await _mpMakeFromRepository.UpdateAsync(mPMakeFrom.Id, mPMakeFrom);
            }
            await _unitOfWork.CommitAsync();
            mPMakeFromVM.MPMakeFromId = (int)mPMakeFrom.Id;
            mPMakeFromVM.ManufPartId = mPMakeFrom.ManufPartId;
            return mPMakeFromVM;
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

        public IEnumerable<MPBOMVM> GetAllMPBOMList(long tenantID)
        {
            var mpmakefromlist = _mpBOMRepository.GetRangeAsync(m => m.TenantId.Equals(tenantID));
            return _mapper.Map<IEnumerable<MPBOMVM>>(mpmakefromlist);
        }
        public async Task<MPBOMVM> MPBOM(MPBOMVM mpBovm)
        {
            var mpBOM = _mapper.Map<MPBOM>(mpBovm);
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

        public async Task<UOMVM> UOM(UOMVM uOMVm)
        {
            var uom = _mapper.Map<UOM>(uOMVm);
            //    mpBOM.PartNumber = manufacturedPartNoDetailVM.PartNumber;
            if (uom.Id == 0)
            {
                await _uOMRepository.AddAsync(uom);
            }
            else
            {
                uom = await _uOMRepository.UpdateAsync(uom.Id, uom);
            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
            }
            uOMVm.UOMId = uom.Id;
            return uOMVm;
        }
        public async Task<bool> CheckUOM(string uomName)
        {
            try
            {
                var documentTypes = await _uOMRepository.SingleOrDefaultAsync(c => c.Name == (uomName));
                if (documentTypes != null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return true;
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
            var mpMakeFrom = _mapper.Map<MPMakeFrom>(mPMakeFromListVM);
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
            var mpBOM = _mapper.Map<MPBOM>(mpBOMVM);
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
