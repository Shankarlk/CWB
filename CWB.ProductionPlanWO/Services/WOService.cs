using AutoMapper;
using CWB.Logging;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using CWB.ProductionPlanWO.Repositories;
using CWB.ProductionPlanWO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Services
{
    public class WOService :IWOService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkOrderRepository _workOrderRepository;
        private readonly IWOSORepository _wosoRepository;
        private readonly IProcPlanPartPurChaseRelRepository _IProcPlanPartPurChaseRelRepository;
        private readonly IBOMTempRepository _bOMTempRepository;
        private readonly IProcPlanRepository _procPlanRepository;
        private readonly IBOMListRepository _bOMListRepository;
        private readonly IProductionPlan_WORepository _productionPlan_WORepository;
        private readonly IWOStatusRepository _wOStatusrepository;
        private readonly IChildWoRelRepository _childWoRelRepository;
        private readonly IMcTimeListRepository _mcTimeListRepository;
        private readonly IPODetailsRepository _poDetailsRepository;
        private readonly IPOHeaderRepository _poHeaderRepository;
        private readonly IPOStatusRepository _poStatusRepository;
        private readonly IWoSubConSupplierRepository _woSubConSupplierRepository;
        private readonly IPOLogRepository _pOLogRepository;

        public WOService(
            ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork
            , IWorkOrderRepository workOrderRepository , IPOLogRepository pOLogRepository
            , IProcPlanRepository procPlanRepository, IWOSORepository woso, IBOMTempRepository bOMTempRepository, IBOMListRepository bOMListRepository,
            IProductionPlan_WORepository productionPlan_WORepository, IWOStatusRepository wOStatus, IChildWoRelRepository childWoRelRepository
            , IMcTimeListRepository mcTimeListRepository, IPODetailsRepository pODetailsRepository,IPOHeaderRepository pOHeaderRepository,IPOStatusRepository pOStatusRepository,
            IWoSubConSupplierRepository woSubConSupplierRepository, IProcPlanPartPurChaseRelRepository purChaseRelRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _workOrderRepository = workOrderRepository;
            _wosoRepository = woso;
            _bOMTempRepository = bOMTempRepository;
            _procPlanRepository = procPlanRepository;
            _bOMListRepository = bOMListRepository;
            _productionPlan_WORepository = productionPlan_WORepository;
            _wOStatusrepository = wOStatus;
            _childWoRelRepository = childWoRelRepository;
            _mcTimeListRepository = mcTimeListRepository;
            _poDetailsRepository = pODetailsRepository;
            _poHeaderRepository = pOHeaderRepository;
            _poStatusRepository = pOStatusRepository;
            _woSubConSupplierRepository = woSubConSupplierRepository;
            _pOLogRepository = pOLogRepository;
            _IProcPlanPartPurChaseRelRepository = purChaseRelRepository;
        }

        public string HelloWorld()
        {
            return "Hello World";
        }

        public async Task<IEnumerable<POLogVM>> GetWoPOLogs(long tenantId, long customerOrderId)
        {
            var poLogs = _pOLogRepository.GetAllAsync().Result.ToList(); //d => d.TenantId == tenantId && d.CustomerOrderId == customerOrderId);
            List<POLog> pvLogList = new List<POLog>();
            foreach (POLog polog in poLogs)
            {
                if (polog.CustomerOrderId != customerOrderId)
                    continue;
                pvLogList.Add(polog);
            }
            pvLogList.Reverse();
            return _mapper.Map<IEnumerable<POLogVM>>(pvLogList);
        }
        public async Task<IEnumerable<POLogVM>> GetPOLogs(long tenantId, long poid)
        {
            var poLogs = _pOLogRepository.GetAllAsync().Result.ToList(); //d => d.TenantId == tenantId && d.CustomerOrderId == customerOrderId);
            List<POLog> pvLogList = new List<POLog>();
            foreach (POLog polog in poLogs)
            {
                if (polog.SalesOrderId != poid)
                    continue;
                pvLogList.Add(polog);
            }
            pvLogList.Reverse();
            return _mapper.Map<IEnumerable<POLogVM>>(pvLogList);
        }

        public async Task<WorkOrdersVM> WorkOrder(WorkOrdersVM workOrdersVM)
        {
            var wo = _mapper.Map<WorkOrders>(workOrdersVM);
            if (wo.SalesOrderId > 0)
            {
                if (wo.Id == 0)
                {
                    wo.WODate = DateTime.Now;
                    wo.WONumber = "WO_" + wo.WODate.Value.ToString("yyyyMMddHHmmssffff");
                    wo.TestData = 'Y';
                    wo.Status = 1;
                    wo.Active = 1;
                    try
                    {
                        await _workOrderRepository.AddAsync(wo);
                        await _unitOfWork.CommitAsync();
                        POLogVM poLog = new POLogVM();
                        poLog.CustomerOrderId = wo.Id;
                        poLog.OldValue = " ";
                        poLog.NewValue = "Entry";
                        poLog.Event = "WOEntry";
                        poLog.User = "Kgk1 Admin";
                        poLog.Comment = wo.WONumber + "/" + wo.Comment + "/" + "/" + wo.SalesOrderId + "/" + wo.PlanCompletionDate;

                        var poLogvm = _mapper.Map<POLog>(poLog);
                        await _pOLogRepository.AddAsync(poLogvm);
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {
                    //wo.WODate = DateTime.Now;
                    var wkord = await _workOrderRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                    if(wkord == null)
                    {
                        return workOrdersVM;
                    }
                    POLogVM poLog = new POLogVM();
                    poLog.CustomerOrderId = wo.Id;
                    poLog.OldValue = wkord.CalcWOQty.ToString();
                    poLog.NewValue = wo.CalcWOQty.ToString();
                    poLog.Event = "WOEdit";
                    poLog.User = "Kgk1 Admin";
                    poLog.Comment = wo.WONumber + "/" + wo.Comment + "/" + "/" + wo.SalesOrderId + "/" + wo.PlanCompletionDate;
                    wkord.CalcWOQty = wo.CalcWOQty;
                    wkord.PlanCompletionDate = wo.PlanCompletionDate;
                    //wkord.BuildToStock = wo.BuildToStock;
                    wkord.Parentlevel = wo.Parentlevel;
                    wkord.Status = wo.Status;
                    wkord.RoutingId = wo.RoutingId;
                    wkord.StartingOpNo = wo.StartingOpNo;
                    wkord.EndingOpNo = wo.EndingOpNo;
                    wkord.ReloadOption = wo.ReloadOption;
                    wkord.Active = wo.Active;
                    wo = await _workOrderRepository.UpdateAsync(wo.Id, wkord);
                    var poLogvm = _mapper.Map<POLog>(poLog);
                    await _pOLogRepository.AddAsync(poLogvm);
                    await _unitOfWork.CommitAsync();
                }
                try
                {
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
                workOrdersVM.WOID = wo.Id;
                workOrdersVM.WONumber = wo.WONumber;
                workOrdersVM.TestData = wo.TestData;
                return workOrdersVM;
            }

            return workOrdersVM;
        }
        public async Task<WoSubConSupplierVM> PostWoSubCon(WoSubConSupplierVM workOrdersVM)
        {
            var wo = _mapper.Map<WoSubConSupplier>(workOrdersVM);
                if (wo.Id == 0)
                {
                    try
                    {
                        await _woSubConSupplierRepository.AddAsync(wo);
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {
                    //wo.WODate = DateTime.Now;
                    var wkord = await _woSubConSupplierRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                    if(wkord == null)
                    {
                        return workOrdersVM;
                    }
                    wo = await _woSubConSupplierRepository.UpdateAsync(wo.Id, wo);
                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
                workOrdersVM.WoSubConSupplierId = wo.Id;
                return workOrdersVM;
        }

        public async Task<List<WorkOrdersVM>> MultipleWorkOrder(List<WorkOrdersVM> workOrdersVM)
        {
            foreach (WorkOrdersVM item in workOrdersVM)
            {
                var wo = _mapper.Map<WorkOrders>(item);
                if (wo.SalesOrderId > 0)
                {
                    if (wo.Id == 0)
                    {
                        wo.WODate = DateTime.Now;
                        wo.WONumber = "WO_" + wo.WODate.Value.ToString("yyyyMMddHHmmssffff");
                        wo.Status = 1;
                        wo.TestData = 'Y';
                        try
                        {
                            await _workOrderRepository.AddAsync(wo);
                            await _unitOfWork.CommitAsync();
                            POLogVM poLog = new POLogVM();
                            poLog.CustomerOrderId = wo.Id;
                            poLog.OldValue = " ";
                            poLog.NewValue = "Entry";
                            poLog.Event = "WOEntry";
                            poLog.User = "Kgk1 Admin";
                            poLog.Comment = wo.WONumber + "/" + wo.Comment + "/" + "/" + wo.SalesOrderId + "/" + wo.PlanCompletionDate;

                            var poLogvm = _mapper.Map<POLog>(poLog);
                            await _pOLogRepository.AddAsync(poLogvm);
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                    }
                    else
                    {
                       
                    }
                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                item.WOID = wo.Id;
                item.WONumber = wo.WONumber;
                item.TestData = wo.TestData;
                item.Status = wo.Status;
            }
            return workOrdersVM;
        }

        public async Task<List<WorkOrdersVM>> UpdateMultipleWorkOrder(List<WorkOrdersVM> workOrdersVM)
        {
            foreach (WorkOrdersVM item in workOrdersVM)
            {
                var wo = _mapper.Map<WorkOrders>(item);
                if (wo.SalesOrderId > 0)
                {
                    if (wo.Id == 0)
                    {
                        //wo.WODate = DateTime.Now;
                        //wo.WONumber = "WO_" + wo.WODate.Value.ToString("yyyyMMddHHmmssffff");
                        //wo.Status = 1;
                        //wo.TestData = 'Y';
                        //try
                        //{
                        //    await _workOrderRepository.AddAsync(wo);
                        //}
                        //catch (Exception ex)
                        //{
                        //    Exception exa = ex.InnerException;
                        //    string msg = ex.Message;
                        //}
                    }
                    else
                    {
                        var wkord = await _workOrderRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                        if (wkord == null)
                        {
                            return workOrdersVM;
                        }
                        wkord.PPStatus = "PP";
                        wkord.Status = wo.Status;
                        wo = await _workOrderRepository.UpdateAsync(wo.Id, wkord);
                    }
                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
            }
            return workOrdersVM;
        }

        public async Task<List<WOSOVM>> PostWOSO(List<WOSOVM> woso)
        {
            foreach (WOSOVM item in woso)
            {
                var wosorel = _mapper.Map<WOSO>(item);
                if (wosorel.SalesOrderId > 0)
                {
                    if (wosorel.Id == 0)
                    {
                        try
                        {
                            await _wosoRepository.AddAsync(wosorel);
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                    }
                    else
                    {
                        var wkord = await _wosoRepository.SingleOrDefaultAsync(x => x.Id == wosorel.Id);
                        if (wkord == null)
                        {
                            return woso;
                        }
                        wkord.Active = wosorel.Active;
                        wosorel = await _wosoRepository.UpdateAsync(wosorel.Id, wkord);
                    }

                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
            }
            return woso;
        }
        public async Task<List<ProcPlanPartPurChaseRelVM>> PostProcPurchase(List<ProcPlanPartPurChaseRelVM> woso)
        {
            foreach (ProcPlanPartPurChaseRelVM item in woso)
            {
                var wosorel = _mapper.Map<ProcPlanPartPurChaseRel>(item);
                if (wosorel.ProcPlanId > 0)
                {
                    if (wosorel.Id == 0)
                    {
                        try
                        {
                            await _IProcPlanPartPurChaseRelRepository.AddAsync(wosorel);
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                    }
                    else
                    {
                        var wkord = await _IProcPlanPartPurChaseRelRepository.SingleOrDefaultAsync(x => x.Id == wosorel.Id);
                        if (wkord == null)
                        {
                            return woso;
                        }
                        wkord.Active = wosorel.Active;
                        wosorel = await _IProcPlanPartPurChaseRelRepository.UpdateAsync(wosorel.Id, wkord);
                    }

                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
            }
            return woso;
        }

        public async Task<List<BOMTempVM>> BOMTempPost(List<BOMTempVM> bomVm)
        {
            foreach (BOMTempVM item in bomVm)
            {
                var bom = _mapper.Map<BOMTemp>(item);
                if(bom.Id == 0)
                {
                    try
                    {
                        await _bOMTempRepository.AddAsync(bom);
                    }catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
            }
            return bomVm;
        }

        public async Task<List<ProcPlanVM>> PostProcPlan(List<ProcPlanVM> proc)
        {
            foreach (ProcPlanVM item in proc)
            {
                var pp = _mapper.Map<ProcPlan>(item);
                if(pp.Id == 0)
                {
                    DateTime dt = DateTime.Now;
                    pp.Reference = "PP_" + dt.ToString("yyyyMMddHHmmssffff");
                    pp.TestData = 'Y';
                    pp.Plan_Proc_Qnty = item.Calc_Proc_Qnty - item.OtyOnHand;
                    try
                    {
                        await _procPlanRepository.AddAsync(pp);
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                   
                }
                else
                {
                    var wkord = await _procPlanRepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
                    if (wkord == null)
                    {
                        return proc;
                    }
                    wkord.Plan_Proc_Qnty = pp.Plan_Proc_Qnty;
                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
                item.ProcPlanId = pp.Id;
            }
            return proc;
        }

        public async Task<List<BOMListVM>> PostBomList(List<BOMListVM> bomlist)
        {
            foreach (BOMListVM item in bomlist)
            {
                var bom = _mapper.Map<BOMList>(item);
                if (bom.Id == 0)
                {
                    //bom.TestData = 'Y';
                    try
                    {
                        await _bOMListRepository.AddAsync(bom);
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                item.BomListId = bom.Id;
            }
            return bomlist;
        }

        public async Task<IEnumerable<WorkOrdersVM>> AllWorkOrders(long tenantId)
        {
            var allwo =  _workOrderRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<WorkOrdersVM>>(allwo);
        }
        public async Task<IEnumerable<WoSubConSupplierVM>> GetAllWoSubCon(long tenantId)
        {
            var allwo = _woSubConSupplierRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<WoSubConSupplierVM>>(allwo);
        }
        public async Task<bool> DeleteSubCon(long Id)
        {
            var co = await _woSubConSupplierRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _woSubConSupplierRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<bool> DeleteWo(long Id)
        {
            var co = await _workOrderRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _workOrderRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    var pwo = await _productionPlan_WORepository.AwaitGetRangeAsync(p=>p.WoId == co.Id);
                    foreach (var item in pwo)
                    {
                        _productionPlan_WORepository.Remove(item);
                        await _unitOfWork.CommitAsync();
                    }
                    var cworel =await _childWoRelRepository.AwaitGetRangeAsync(p => p.WoId == co.Id);
                    foreach (var item in cworel)
                    {
                        _childWoRelRepository.Remove(item);
                        await _unitOfWork.CommitAsync();
                    }
                    var mcTime =await _mcTimeListRepository.AwaitGetRangeAsync(p => p.WoId == co.Id);
                    foreach (var item in mcTime)
                    {
                        _mcTimeListRepository.Remove(item);
                        await _unitOfWork.CommitAsync();
                    }
                    var bom =await _bOMListRepository.AwaitGetRangeAsync(p => p.ParentWoId == co.Id);
                    foreach (var item in bom)
                    {
                        _bOMListRepository.Remove(item);
                        await _unitOfWork.CommitAsync();
                    }
                    var woso =await _wosoRepository.AwaitGetRangeAsync(p => p.WorkOrderId == co.Id);
                    foreach (var item in woso)
                    {
                        _wosoRepository.Remove(item);
                        await _unitOfWork.CommitAsync();
                    }
                    var procplan =await _procPlanRepository.AwaitGetRangeAsync(p => p.WorkOrderId == co.Id);
                    foreach (var item in procplan)
                    {
                        _procPlanRepository.Remove(item);
                        var Podetails =await _poDetailsRepository.AwaitGetRangeAsync(p => p.ProcPlanId == item.Id);
                        foreach (var pod in Podetails)
                        {
                            var Pohead =await _poHeaderRepository.AwaitGetRangeAsync(p => p.PoDetailsId == pod.Id);
                            foreach (var pohead in Pohead)
                            {
                                _poHeaderRepository.Remove(pohead);
                                await _unitOfWork.CommitAsync();
                            }
                            _poDetailsRepository.Remove(pod);
                            await _unitOfWork.CommitAsync();
                        }
                        var purchaseRel = await _IProcPlanPartPurChaseRelRepository.AwaitGetRangeAsync(p => p.ProcPlanId == item.Id);
                        foreach (var pprel in purchaseRel)
                        {
                            _IProcPlanPartPurChaseRelRepository.Remove(pprel);
                            await _unitOfWork.CommitAsync();
                        }
                        await _unitOfWork.CommitAsync();
                    }
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }

        public async Task<IEnumerable<WorkOrdersVM>> AllParentChildWo(long parentWoId, long tenantId)
        {
            var allwo = _workOrderRepository.GetRangeAsync(d => d.ParentWoId == parentWoId && d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<WorkOrdersVM>>(allwo);
        }

        public async Task<WorkOrdersVM> GetSingleWO(long Id, long tenantId)
        {
            var singlewo =await _workOrderRepository.SingleOrDefaultAsync(d=>d.Id == Id);
            if (singlewo != null)
            {
              return _mapper.Map<WorkOrdersVM>(singlewo);
            }
            return new WorkOrdersVM { WOID = -1 };
        }

        public async Task<IEnumerable<WOSOVM>> GetSoWo(long workOrderId)
        {
            var so = _wosoRepository.GetRangeAsync(s => s.WorkOrderId == workOrderId).OrderBy(s => s.Id);
            try
            {
                return _mapper.Map<IEnumerable<WOSOVM>>(so);
            }
            catch (Exception ex)
            {
                return new List<WOSOVM>();                
            }
        }
        public async Task<IEnumerable<ProcPlanPartPurChaseRelVM>> GetProcPurchase(long procPlanId)
        {
            var so = _IProcPlanPartPurChaseRelRepository.GetRangeAsync(s => s.ProcPlanId == procPlanId).OrderBy(s => s.Id);
            try
            {
                return _mapper.Map<IEnumerable<ProcPlanPartPurChaseRelVM>>(so);
            }
            catch (Exception ex)
            {
                return new List<ProcPlanPartPurChaseRelVM>();                
            }
        }

        public async Task<IEnumerable<ProcPlanVM>> AllProcPlan(long tenantId)
        {
            var allprocplan =_procPlanRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<ProcPlanVM>>(allprocplan);
        }
        public async Task<IEnumerable<BOMListVM>> AllBomList(long tenantId)
        {
            var allbomlist = _bOMListRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<BOMListVM>>(allbomlist);
        }

        public async Task<List<ProductionPlan_WOVM>> PostProductionPlan_Wo(List<ProductionPlan_WOVM> productions)
        {
            foreach (ProductionPlan_WOVM item in productions)
            {
                var pp = _mapper.Map<ProductionPlan_WO>(item);
                if (pp.SalesOrderId > 0)
                {
                    if (pp.Id == 0)
                    {
                        pp.WODate = DateTime.Now;
                        pp.PPNumber = "PP_" + pp.WODate.Value.ToString("yyyyMMddHHmmssffff");
                        if(pp.WONumber == null)
                        {
                            pp.WONumber = "WO_" + pp.WODate.Value.ToString("yyyyMMddHHmmssffff");
                        }
                        pp.Status = 1;
                        pp.TestData = 'Y';
                        try
                        {
                            await _productionPlan_WORepository.AddAsync(pp);
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                    }
                    else
                    {
                        var upp = await _productionPlan_WORepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
                        if (upp == null)
                        {
                            return productions;
                        }
                        upp.CalcWOQty = pp.CalcWOQty;
                        upp.PlanCompletionDate = pp.PlanCompletionDate;
                        upp.BuildToStock = pp.BuildToStock;
                        upp.Parentlevel = pp.Parentlevel;
                        upp.Status = pp.Status;
                        upp.RoutingId = pp.RoutingId;
                        upp.StartingOpNo = pp.StartingOpNo;
                        upp.EndingOpNo = pp.EndingOpNo;
                        upp.ReloadOption = pp.ReloadOption;
                        upp.Active = pp.Active;
                        pp = await _productionPlan_WORepository.UpdateAsync(pp.Id, upp);
                    }
                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                item.ProductionPlanId = pp.Id;
                item.PPNumber = pp.PPNumber;
                item.WONumber = pp.WONumber;
                item.TestData = pp.TestData;
            }
            return productions;
        }

        public async Task<IEnumerable<ProductionPlan_WOVM>> AllProductionWo(long tenantId)
        {
            var allpp = _productionPlan_WORepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<ProductionPlan_WOVM>>(allpp);
        }

        public async Task<WOStatusVM> GetWOStatus(long Id)
        {
            var allpp =await _wOStatusrepository.SingleOrDefaultAsync(d => d.Id == Id);
            if (allpp != null)
            {
                return _mapper.Map<WOStatusVM>(allpp);
            }
            return new WOStatusVM { StatusId = -1 };
        }

        public async Task<List<ChildWoRelVM>> PostChildWoRel(List<ChildWoRelVM> childWos)
        {
            foreach (ChildWoRelVM item in childWos)
            {
                var cwo = _mapper.Map<ChildWoRel>(item);
                if (cwo.WoId > 0)
                {
                    if (cwo.Id == 0)
                    {
                        try
                        {
                            await _childWoRelRepository.AddAsync(cwo);
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                        try
                        {
                            await _unitOfWork.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                    }
                }
            }
            return childWos;
        }

        public async Task<List<McTimeListVM>> PostMcTimeList(List<McTimeListVM> mcTimeLists)
        {
            foreach (McTimeListVM item in mcTimeLists)
            {
                var cwo = _mapper.Map<McTimeList>(item);
                //if (cwo.WoId > 0)
                //{
                    if (cwo.Id == 0)
                    {
                        try
                        {
                            await _mcTimeListRepository.AddAsync(cwo);
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                        try
                        {
                            await _unitOfWork.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                    }
                //}
                item.McTimeListId = cwo.Id;
            }
            return mcTimeLists;
        }

        public async Task<IEnumerable<McTimeListVM>> GetAllMcTimeListVMs(long tenantId)
        {
            var allpp = _mcTimeListRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<McTimeListVM>>(allpp);
        }
        public async Task<IEnumerable<PODetailsVM>> GetAllPodetails(long tenantId)
        {
            var allpp = _poDetailsRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<PODetailsVM>>(allpp);
        }


        public async Task<POStatusVM> GetPOStatus(long Id)
        {
            var allpp = await _poStatusRepository.SingleOrDefaultAsync(d => d.Id == Id);
            if (allpp != null)
            {
                return _mapper.Map<POStatusVM>(allpp);
            }
            return new POStatusVM { StatusId = -1 };
        }

        public async Task<List<PODetailsVM>> MultiplePODetails(List<PODetailsVM> pODetailsVM)
        {
            foreach (PODetailsVM item in pODetailsVM)
            {
                var po = _mapper.Map<PODetails>(item);
                if (po.Id == 0)
                {
                    po.PoDate = DateTime.Now;
                    po.POReference = "PO_" + po.PoDate.ToString("yyyyMMddHHmmssffff");
                    po.Status = 1;
                    try
                    {
                        await _poDetailsRepository.AddAsync(po);
                        POLogVM poLog = new POLogVM();
                        poLog.SalesOrderId = po.Id;
                        poLog.OldValue = " ";
                        poLog.NewValue = "Entry";
                        poLog.Event = "POEntry";
                        poLog.User = "Kgk1 Admin";
                        poLog.Comment = po.POReference + "/"+ po.Id + "/" + po.PlanPoReceiptDate;

                        var poLogvm = _mapper.Map<POLog>(poLog);
                        await _pOLogRepository.AddAsync(poLogvm);
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {
                    var upp = await _poDetailsRepository.SingleOrDefaultAsync(x => x.Id == po.Id);
                    upp.Status = po.Status;
                    await _poDetailsRepository.UpdateAsync(po.Id, upp);

                    POLogVM poLog = new POLogVM();
                    poLog.SalesOrderId = po.Id;
                    poLog.OldValue = "Not Aprroved";
                    poLog.NewValue = "PO Aprroved";
                    poLog.Event = "Status Change";
                    poLog.User = "Kgk1 Admin";
                    poLog.Comment = po.POReference + "/" + po.Id + "/" + po.PlanPoReceiptDate;

                    var poLogvm = _mapper.Map<POLog>(poLog);
                    await _pOLogRepository.AddAsync(poLogvm);

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
                item.PoDetailsId = po.Id;
                item.POReference = po.POReference;
                item.Status = po.Status;
            }
            return pODetailsVM;
        }

        public async Task<List<POHeaderVM>> MultiplePOHeaders(List<POHeaderVM> pOHeaderVMs)
        {
            foreach (POHeaderVM item in pOHeaderVMs)
            {
                var po = _mapper.Map<POHeader>(item);
                if (po.Id == 0)
                {
                    try
                    {
                        await _poHeaderRepository.AddAsync(po);
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
                item.PoDetailsId = po.Id;
            }
            return pOHeaderVMs;
        }
    }
}
