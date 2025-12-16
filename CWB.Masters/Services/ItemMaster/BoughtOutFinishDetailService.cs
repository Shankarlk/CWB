using AutoMapper;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.Domain.ItemMaster;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.ItemMaster;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using System;
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
        private readonly IMasterPartRepository  _masterPartRepository;
        private readonly IPartStatusChangeLogRepository _partStatusChangeLogRepository;


        public BoughtOutFinishDetailService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IBoughtOutFinishDetailRepository boughtOutFinishDetailRepository,
            IMasterPartRepository masterPartRepository, IPartStatusChangeLogRepository partStatusChangeLogRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _boughtOutFinishDetailRepository = boughtOutFinishDetailRepository;
            _masterPartRepository = masterPartRepository;
            _partStatusChangeLogRepository = partStatusChangeLogRepository;
        }
        public IEnumerable<BoughtOutFinishDetailVM> GetBoughtOutFinishDetailsByTenant(long tenantID)
        {
            var boughtoutfinishdetails = _boughtOutFinishDetailRepository.GetRangeAsync(m => m.TenantId == tenantID);         
            return _mapper.Map<IEnumerable<BoughtOutFinishDetailVM>>(boughtoutfinishdetails);
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

        public async Task<BoughtOutFinishDetailVM> BoughtOutFinishDetail(BoughtOutFinishDetailVM vm)
        {
            long? createdMasterPartId = null;
            long? createdStatusLogId = null;
            long? createdBOFDetailId = null;

            try
            {
                var masterPart = _mapper.Map<MasterPart>(vm);
                var boughtOutFinishDetail = _mapper.Map<BoughtOutFinishDetail>(vm);

                int id = GetPartId(masterPart.PartNo);

                boughtOutFinishDetail.PartId = id;
                vm.PartId = id;

                // --------------------------------------------------------------------
                // CASE 1: NEW MASTER PART
                // --------------------------------------------------------------------
                if (id == 0)
                {
                    masterPart.Id = 0;
                    masterPart.Status = "Not Released";
                    masterPart.Inv_Trans = 'N';
                    masterPart.Linked_to_BOM = 'N';

                    await _masterPartRepository.AddAsync(masterPart);
                    await _unitOfWork.CommitAsync();     // get DB identity Id

                    createdMasterPartId = masterPart.Id;

                    boughtOutFinishDetail.PartId = (int)masterPart.Id;
                    vm.PartId = (int)masterPart.Id;

                    var status = new PartStatusChangeLog()
                    {
                        MasterPartId = masterPart.Id,
                        Status = masterPart.Status,
                        ChangeReason = masterPart.StatusChangeReason,
                        TenantId = masterPart.TenantId
                    };

                    await _partStatusChangeLogRepository.AddAsync(status);
                    await _unitOfWork.CommitAsync();

                    createdStatusLogId = status.Id;
                }
                else
                {
                    // --------------------------------------------------------------------
                    // CASE 2: EXISTING PART → UPDATE STATUS + LOG
                    // --------------------------------------------------------------------
                    var existing = await _masterPartRepository.SingleOrDefaultAsync(s => s.Id == masterPart.Id);

                    var status = new PartStatusChangeLog()
                    {
                        MasterPartId = masterPart.Id,
                        Status = masterPart.Status,
                        FromChangedStatus = existing.Status,
                        ChangeReason = masterPart.StatusChangeReason,
                        TenantId = masterPart.TenantId
                    };

                    await _partStatusChangeLogRepository.AddAsync(status);
                    await _masterPartRepository.UpdateAsync(masterPart.Id, masterPart);
                    await _unitOfWork.CommitAsync();

                    createdStatusLogId = status.Id;   // rollback support
                    vm.PartId = (int)masterPart.Id; 
                }

                boughtOutFinishDetail.PartId = vm.PartId;

                // --------------------------------------------------------------------
                // INSERT / UPDATE BOUGHT OUT FINISH DETAIL
                // --------------------------------------------------------------------
                if (boughtOutFinishDetail.Id == 0)
                {
                    await _boughtOutFinishDetailRepository.AddAsync(boughtOutFinishDetail);
                    await _unitOfWork.CommitAsync();
                    createdBOFDetailId = boughtOutFinishDetail.Id;
                }
                else
                {
                    boughtOutFinishDetail = await _boughtOutFinishDetailRepository
                            .UpdateAsync(boughtOutFinishDetail.Id, boughtOutFinishDetail);
                    await _unitOfWork.CommitAsync();
                }

                vm.BoughtOutFinishDetailId = boughtOutFinishDetail.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;

                // --------------------------------------------------------------------
                // ROLLBACK SECTION (reverse order)
                // --------------------------------------------------------------------

                // 1️⃣ DELETE BOUGHT OUT FINISH DETAIL
                if (createdBOFDetailId.HasValue)
                {
                    var bof = await _boughtOutFinishDetailRepository
                        .SingleOrDefaultAsync(s => s.Id == createdBOFDetailId.Value);

                    if (bof != null)
                    {
                        _boughtOutFinishDetailRepository.Remove(bof);
                        await _unitOfWork.CommitAsync();
                    }
                }

                // 2️⃣ DELETE STATUS LOG
                if (createdStatusLogId.HasValue)
                {
                    var status = await _partStatusChangeLogRepository
                        .SingleOrDefaultAsync(s => s.Id == createdStatusLogId.Value);

                    if (status != null)
                    {
                        _partStatusChangeLogRepository.Remove(status);
                        await _unitOfWork.CommitAsync();
                    }
                }

                // 3️⃣ DELETE MASTER PART
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

                throw;  // Return original error
            }

            return vm;
        }


        public bool CheckPartNo(long partId)
        {
            var bofs = _boughtOutFinishDetailRepository.GetRangeAsync(c => c.PartId == (partId));
            if (!bofs.Any())
            {
                return false;
            }
            return true;
        }

        public async Task<BoughtOutFinishDetailVM> GetPart(int partId, long tenantId)
        {
            try
            {
                var part = _boughtOutFinishDetailRepository.GetRangeAsync(m => m.PartId == partId && m.TenantId == tenantId).OrderByDescending(m=>m.Id).FirstOrDefault();
                if (part != null)
                {
                    return _mapper.Map<BoughtOutFinishDetailVM>(part);
                }
            } catch(Exception ex)
            {
                var msg = ex.InnerException.Message;
                var src = ex.InnerException.Source;
            }
            return new BoughtOutFinishDetailVM { BoughtOutFinishDetailId = -1 };
        }
    }
}
