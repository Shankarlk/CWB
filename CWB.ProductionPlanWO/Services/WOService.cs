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
        private readonly IInward_Condn_listRepository _IInward_Condn_listRepository;
        private readonly IInsp_Outcome_DetailsRepository _iInsp_Outcome_DetailsRepository;
        private readonly IInsp_Outcome_ListRepository _IInsp_Outcome_ListRepository;
        private readonly IInventory_MasterRepository _IInventory_MasterRepository;
        private readonly IInv_Trans_LogRepository _IInv_Trans_LogRepository;
        private readonly IInw_Recpt_DetailsRepository _IInw_Recpt_DetailsRepository;
        private readonly IInw_Recpt_HeaderRepository _IInw_Recpt_HeaderRepository;
        private readonly IInw_Recpt_Part_NoRepository _IInw_Recpt_Part_NoRepository;
        private readonly IInwardDocTypeRepository _IInwardDocTypeRepository;
        private readonly IRcCaDocTypeRepository _IRcCaDocTypeRepository;
        private readonly ILineInspectDocTypeRepository _ILineInspectDocTypeRepository;
        private readonly IFinalInspectDocTypeRepository _IFinalInspectDocTypeRepository;
        private readonly IOperationSettingsRepository _operationSettingsRepository;
        private readonly ICont_RCA_CA_LogRepository _Cont_RCA_CA_LogRepository;
        private readonly INC_Decision_LogRepository _NC_Decision_LogRepository;
        private readonly INC_Wk_List_Tmpl_DetRepository _NC_Wk_List_Tmpl_DetRepository;
        private readonly INC_Disp_Decs_Appl_ListRepository _NC_Disp_Decs_Appl_ListRepository;
        private readonly INC_Wk_List_Tmpl_HeadRepository _NC_Wk_List_Tmpl_HeadRepository;
        private readonly INC_Work_ListRepository _NC_Work_ListRepository;
        private readonly INC_Wk_List_HeaderRepository _NC_Wk_List_HeaderRepository;
        private readonly ICust_NC_Decs_Matrix_OptRepositoy _Cust_NC_Decs_Matrix_OptRepository;
        private readonly ICust_NC_Decs_MatrixRepository _Cust_NC_Decs_MatrixRepository;
        private readonly ICont_RCA_CA_Status_ListRepository _Cont_RCA_CA_Status_ListRepository;
        private readonly INC_Disp_Decision_ListRepository _NC_Disp_Decision_ListRepository;
        private readonly INC_work_StatusRepository _NC_work_StatusRepository;
        private readonly IMc_Not_Avl_ReasonRepository _Mc_Not_Avl_ReasonRepository;
        private readonly IMode_ListRepository _Mode_ListRepository;
        private readonly INon_Plan_Wk_type_ListRepository _Non_Plan_Wk_type_ListRepository;
        private readonly ITime_Slot_AllocationRepository _Time_Slot_AllocationRepository;
        private readonly IMc_Timeslot_ListRepository _Mc_Timeslot_ListRepository;
        private readonly ITempMc_Timeslot_ListRepository _TempMc_Timeslot_ListRepository;
        private readonly INon_Plan_Wk_ListRepository _Non_Plan_Wk_ListRepository;
        private readonly IWO_Wait_ListRepository _WO_Wait_ListRepository;
        private readonly ITempWO_Wait_ListRepository _TempWO_Wait_ListRepository;
        private readonly IWO_Bookout_LogRepository _WO_Bookout_LogRepository;
        private readonly ITimeslot_SettingRepository _Timeslot_SettingRepository;
        private readonly ITimeslot_ListRepository _Timeslot_ListRepository;
        private readonly IRwk_ListRepository _Rwk_ListRepository;
        private readonly IOpr_ListRepository _Opr_ListRepository;
        private readonly ITempOpr_ListRepository _TempOpr_ListRepository;
        private readonly IShop_Insp_LogRepository _Shop_Insp_LogRepository;
        private readonly ISubCon_ListRepository _SubCon_ListRepository;
        private readonly ITempSubCon_ListRepository _TempSubCon_ListRepository;
        private readonly IMc_Wait_ListRepository _Mc_Wait_ListRepository;
        private readonly IMatl_Issue_SettingsRepository _Matl_Issue_SettingsRepository;
        private readonly IMatl_Issue_ListRepository _Matl_Issue_ListRepository;
        private readonly ITempMc_Wait_ListRepository _TempMc_Wait_ListRepository;
        private readonly IInspectDocTypeRepository _IInspectDocTypeRepository;
        private readonly INcLogStatusRepository _INcLogStatusRepository;

        public WOService(
            ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork
            , IWorkOrderRepository workOrderRepository , IPOLogRepository pOLogRepository
            , IProcPlanRepository procPlanRepository, IWOSORepository woso, IBOMTempRepository bOMTempRepository, IBOMListRepository bOMListRepository,
            IProductionPlan_WORepository productionPlan_WORepository, IWOStatusRepository wOStatus, IChildWoRelRepository childWoRelRepository
            , IMcTimeListRepository mcTimeListRepository, IPODetailsRepository pODetailsRepository,IPOHeaderRepository pOHeaderRepository,IPOStatusRepository pOStatusRepository,
            IWoSubConSupplierRepository woSubConSupplierRepository, IProcPlanPartPurChaseRelRepository purChaseRelRepository, IInward_Condn_listRepository IInward_Condn_listRepository,
            IInsp_Outcome_DetailsRepository iInsp_Outcome_DetailsRepository, 
            IInsp_Outcome_ListRepository insp_Outcome_ListRepository, IInventory_MasterRepository Inventory_Master
            ,IInv_Trans_LogRepository inv_Trans_LogRepository, IInw_Recpt_DetailsRepository inw_Recpt_DetailsRepository
            ,IInw_Recpt_HeaderRepository Inw_Recpt_Header, IInw_Recpt_Part_NoRepository Inw_Recpt_Part_No,
            IInwardDocTypeRepository InwardDocTypeRepository,IRcCaDocTypeRepository RcCaDocTypeRepository
            , ILineInspectDocTypeRepository LineInspectDocTypeRepository, IFinalInspectDocTypeRepository FinalInspectDocTypeRepository,
            IInspectDocTypeRepository InspectDocTypeRepository,INcLogStatusRepository ncLogStatusRepository,IOperationSettingsRepository operationSettingsRepository,ICont_RCA_CA_LogRepository Cont_RCA_CA_LogRepository
            ,INC_Decision_LogRepository NC_Decision_LogRepository,INC_Wk_List_Tmpl_DetRepository NC_Wk_List_Tmpl_DetRepository ,INC_Disp_Decs_Appl_ListRepository NC_Disp_Decs_Appl_ListRepository  ,INC_Wk_List_Tmpl_HeadRepository NC_Wk_List_Tmpl_HeadRepository  ,INC_Work_ListRepository NC_Work_ListRepository ,INC_Wk_List_HeaderRepository NC_Wk_List_HeaderRepository ,ICust_NC_Decs_Matrix_OptRepositoy Cust_NC_Decs_Matrix_OptRepository
            ,ICust_NC_Decs_MatrixRepository Cust_NC_Decs_MatrixRepository ,ICont_RCA_CA_Status_ListRepository Cont_RCA_CA_Status_ListRepository,INC_Disp_Decision_ListRepository NC_Disp_Decision_ListRepository
            ,INC_work_StatusRepository NC_work_StatusRepository, IMc_Not_Avl_ReasonRepository Mc_Not_Avl_ReasonRepository, IMode_ListRepository Mode_ListRepository,
            IMc_Timeslot_ListRepository Mc_Timeslot_ListRepository,ITempMc_Timeslot_ListRepository TempMc_Timeslot_ListRepository,
            INon_Plan_Wk_ListRepository Non_Plan_Wk_ListRepository,IWO_Wait_ListRepository WO_Wait_ListRepository,ITempWO_Wait_ListRepository TempWO_Wait_ListRepository,IWO_Bookout_LogRepository WO_Bookout_LogRepository,ITimeslot_SettingRepository Timeslot_SettingRepository,ITimeslot_ListRepository Timeslot_ListRepository,IRwk_ListRepository Rwk_ListRepository,IOpr_ListRepository Opr_ListRepository,ITempOpr_ListRepository TempOpr_ListRepository,IShop_Insp_LogRepository Shop_Insp_LogRepository,ISubCon_ListRepository SubCon_ListRepository,ITempSubCon_ListRepository TempSubCon_ListRepository, IMc_Wait_ListRepository Mc_Wait_ListRepository,IMatl_Issue_SettingsRepository Matl_Issue_SettingsRepository,IMatl_Issue_ListRepository Matl_Issue_ListRepository,
            ITempMc_Wait_ListRepository TempMc_Wait_ListRepository, INon_Plan_Wk_type_ListRepository Non_Plan_Wk_type_ListRepository, ITime_Slot_AllocationRepository Time_Slot_AllocationRepository)
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
            _IInward_Condn_listRepository = IInward_Condn_listRepository;
            _IInventory_MasterRepository = Inventory_Master;
            _IInsp_Outcome_ListRepository = insp_Outcome_ListRepository;
            _IInv_Trans_LogRepository = inv_Trans_LogRepository;
            _IInw_Recpt_DetailsRepository = inw_Recpt_DetailsRepository;
            _IInw_Recpt_HeaderRepository = Inw_Recpt_Header;
            _IInw_Recpt_Part_NoRepository = Inw_Recpt_Part_No;
            _iInsp_Outcome_DetailsRepository = iInsp_Outcome_DetailsRepository;
            _IInwardDocTypeRepository = InwardDocTypeRepository;
            _ILineInspectDocTypeRepository = LineInspectDocTypeRepository;
            _IFinalInspectDocTypeRepository = FinalInspectDocTypeRepository;
            _IRcCaDocTypeRepository = RcCaDocTypeRepository;
            _IInspectDocTypeRepository = InspectDocTypeRepository;
            _INcLogStatusRepository = ncLogStatusRepository;
            _operationSettingsRepository = operationSettingsRepository;
            _Cont_RCA_CA_LogRepository = Cont_RCA_CA_LogRepository;
            _NC_Decision_LogRepository = NC_Decision_LogRepository;
            _NC_Wk_List_Tmpl_DetRepository = NC_Wk_List_Tmpl_DetRepository;
            _NC_Disp_Decs_Appl_ListRepository = NC_Disp_Decs_Appl_ListRepository;
            _NC_Wk_List_Tmpl_HeadRepository = NC_Wk_List_Tmpl_HeadRepository;
            _NC_Work_ListRepository = NC_Work_ListRepository;
            _NC_Wk_List_HeaderRepository = NC_Wk_List_HeaderRepository;
            _Cust_NC_Decs_Matrix_OptRepository = Cust_NC_Decs_Matrix_OptRepository;
            _Cust_NC_Decs_MatrixRepository = Cust_NC_Decs_MatrixRepository;
            _Cont_RCA_CA_Status_ListRepository = Cont_RCA_CA_Status_ListRepository;
            _NC_work_StatusRepository = NC_work_StatusRepository;
            _NC_Disp_Decision_ListRepository = NC_Disp_Decision_ListRepository;
            _Mode_ListRepository = Mode_ListRepository;
            _Non_Plan_Wk_type_ListRepository = Non_Plan_Wk_type_ListRepository;
            _Time_Slot_AllocationRepository = Time_Slot_AllocationRepository;
            _Mc_Not_Avl_ReasonRepository = Mc_Not_Avl_ReasonRepository;
            _Mc_Timeslot_ListRepository = Mc_Timeslot_ListRepository;
            _TempMc_Timeslot_ListRepository = TempMc_Timeslot_ListRepository;
            _Non_Plan_Wk_ListRepository = Non_Plan_Wk_ListRepository;
            _WO_Wait_ListRepository = WO_Wait_ListRepository;
            _TempWO_Wait_ListRepository = TempWO_Wait_ListRepository;
            _WO_Bookout_LogRepository = WO_Bookout_LogRepository;
            _Timeslot_SettingRepository = Timeslot_SettingRepository;
            _Timeslot_ListRepository = Timeslot_ListRepository;
            _Rwk_ListRepository = Rwk_ListRepository;
            _Opr_ListRepository = Opr_ListRepository;
            _TempOpr_ListRepository = TempOpr_ListRepository;
            _Shop_Insp_LogRepository = Shop_Insp_LogRepository;
            _SubCon_ListRepository = SubCon_ListRepository;
            _TempSubCon_ListRepository = TempSubCon_ListRepository;
            _Mc_Wait_ListRepository = Mc_Wait_ListRepository;
            _Matl_Issue_SettingsRepository = Matl_Issue_SettingsRepository;
            _Matl_Issue_ListRepository = Matl_Issue_ListRepository;
            _TempMc_Wait_ListRepository = TempMc_Wait_ListRepository;
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
                        upp.Changed = 1;
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
        public async Task<ProductionPlan_WOVM> UpdateProductionPlan_Wo(ProductionPlan_WOVM productions)
        {
            var pp = _mapper.Map<ProductionPlan_WO>(productions);
            var upp = await _productionPlan_WORepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
            if (upp == null)
            {
                return productions;
            }
            upp.RoutingId = pp.RoutingId;
            upp.StartingOpNo = pp.StartingOpNo;
            upp.EndingOpNo = pp.EndingOpNo;
            upp.Changed = 1;
            pp = await _productionPlan_WORepository.UpdateAsync(pp.Id, upp);
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
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

        public async Task<IEnumerable<Insp_Outcome_DetailsVM>> GetAllInsp_Outcome_Details(long tenantId)
        {
            var allwo = _iInsp_Outcome_DetailsRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Insp_Outcome_DetailsVM>>(allwo);
        }
        public async Task<IEnumerable<Insp_Outcome_ListVM>> GetAllInsp_Outcome_List()
        {
            var allwo = await _IInsp_Outcome_ListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Insp_Outcome_ListVM>>(allwo);
        }
        public async Task<IEnumerable<Inventory_MasterVM>> GetAllInventory_Master(long tenantId)
        {
            var allwo = _IInventory_MasterRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inventory_MasterVM>>(allwo);
        }
        public async Task<IEnumerable<Inv_Trans_LogVM>> GetAllInvTransLog(long tenantId)
        {
            var allwo = _IInv_Trans_LogRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inv_Trans_LogVM>>(allwo);
        }
        public async Task<IEnumerable<Inw_Recpt_DetailsVM>> GetAllInw_Recpt_Details(long tenantId)
        {
            var allwo = _IInw_Recpt_DetailsRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inw_Recpt_DetailsVM>>(allwo);
        }
        public async Task<IEnumerable<Inw_Recpt_HeaderVM>> GetAlInw_Recpt_Header(long tenantId)
        {
            var allwo = _IInw_Recpt_HeaderRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inw_Recpt_HeaderVM>>(allwo);
        }
        public async Task<IEnumerable<Inw_Recpt_Part_NoVM>> GetAlInw_Recpt_Part_No(long tenantId)
        {
            var allwo = _IInw_Recpt_Part_NoRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inw_Recpt_Part_NoVM>>(allwo);
        }
        public async Task<IEnumerable<Inward_Condn_listVM>> GetAllInward_Condn_list()
        {
            var allwo = await _IInward_Condn_listRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Inward_Condn_listVM>>(allwo);
        }
        public async Task<Insp_Outcome_DetailsVM> PostInsp_Outcome_Details(Insp_Outcome_DetailsVM workOrdersVM)
        {
            var wo = _mapper.Map<Insp_Outcome_Details>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    var dt = DateTime.Now;
                    wo.NC_Tracking_No = "NC_" + dt.ToString("yyyyMMddHHmmssffff");
                    await _iInsp_Outcome_DetailsRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _iInsp_Outcome_DetailsRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                wo.NC_Tracking_No = wkord.NC_Tracking_No;
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _iInsp_Outcome_DetailsRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Insp_Outcome_Details_Id = wo.Id;
            return workOrdersVM;
        }
        public async Task<Insp_Outcome_ListVM> PostInsp_Outcome_List(Insp_Outcome_ListVM workOrdersVM)
        {
            var wo = _mapper.Map<Insp_Outcome_List>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    await _IInsp_Outcome_ListRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _IInsp_Outcome_ListRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _IInsp_Outcome_ListRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Insp_Outcome_ListId = wo.Id;
            return workOrdersVM;
        }
        public async Task<Inventory_MasterVM> PostInventory_Master(Inventory_MasterVM workOrdersVM)
        {
            var wo = _mapper.Map<Inventory_Master>(workOrdersVM);
            wo.Dt_time = DateTime.Now;
            if (wo.Id == 0)
            {
                try
                {
                    await _IInventory_MasterRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _IInventory_MasterRepository.SingleOrDefaultAsync(x => x.Id == wo.Inv_Trans_Log_Id);
                wo.Current_QntOnHand = wkord.Current_QntOnHand + wo.Current_QntOnHand;
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _IInventory_MasterRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Inventory_MasterId = wo.Id;
            return workOrdersVM;
        }
        public async Task<Inv_Trans_LogVM> PostInv_Trans_Log(Inv_Trans_LogVM workOrdersVM)
        {
            var wo = _mapper.Map<Inv_Trans_Log>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    wo.Dt_time = DateTime.Now;
                    await _IInv_Trans_LogRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                wo.Dt_time = DateTime.Now;
                var wkord = await _IInv_Trans_LogRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _IInv_Trans_LogRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Inv_Trans_LogId = wo.Id;
            return workOrdersVM;
        }
        public async Task<Inw_Recpt_DetailsVM> PostInw_Recpt_Details(Inw_Recpt_DetailsVM workOrdersVM)
        {
            var wo = _mapper.Map<Inw_Recpt_Details>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    await _IInw_Recpt_DetailsRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _IInw_Recpt_DetailsRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _IInw_Recpt_DetailsRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Inw_Recpt_DetailsId = wo.Id;
            return workOrdersVM;
        }
        public async Task<Inw_Recpt_HeaderVM> PostInw_Recpt_Header(Inw_Recpt_HeaderVM workOrdersVM)
        {
            var wo = _mapper.Map<Inw_Recpt_Header>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    await _IInw_Recpt_HeaderRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _IInw_Recpt_HeaderRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _IInw_Recpt_HeaderRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Inw_Recpt_HeaderId = wo.Id;
            return workOrdersVM;
        }
        public async Task<Inw_Recpt_Part_NoVM> PostInw_Recpt_Part_No(Inw_Recpt_Part_NoVM workOrdersVM)
        {
            var wo = _mapper.Map<Inw_Recpt_Part_No>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    await _IInw_Recpt_Part_NoRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _IInw_Recpt_Part_NoRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _IInw_Recpt_Part_NoRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Inw_Recpt_Part_No_Id = wo.Id;
            return workOrdersVM;
        }
        public async Task<Inward_Condn_listVM> PostInward_Condn_list(Inward_Condn_listVM workOrdersVM)
        {
            var wo = _mapper.Map<Inward_Condn_list>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    await _IInward_Condn_listRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _IInward_Condn_listRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _IInward_Condn_listRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Inward_Condn_listId = wo.Id;
            return workOrdersVM;
        }

        public async Task<bool> DeleteInsp_OutcomeDetails(long Id)
        {
            var co = await _iInsp_Outcome_DetailsRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _iInsp_Outcome_DetailsRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<bool> DeleteInventory_Master(long Id)
        {
            var co = await _IInventory_MasterRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _IInventory_MasterRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<bool> DeleteInv_Trans_Log(long Id)
        {
            var co = await _IInv_Trans_LogRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _IInv_Trans_LogRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<bool> DeleteInw_Recpt_Details(long Id)
        {
            var co = await _IInw_Recpt_DetailsRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _IInw_Recpt_DetailsRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<bool> DeleteInw_Recpt_Header(long Id)
        {
            var co = await _IInw_Recpt_HeaderRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _IInw_Recpt_HeaderRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<bool> DeleteInw_Recpt_Part_No(long Id)
        {
            var co = await _IInw_Recpt_Part_NoRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _IInw_Recpt_Part_NoRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<InwardDocTypeVM>> GetAllInwardDocList(long tenantId)
        {
            var allDocuType = _IInwardDocTypeRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<InwardDocTypeVM>>(allDocuType);
        }
        public async Task<InwardDocTypeVM> PostInwardDocList(InwardDocTypeVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<InwardDocType>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _IInwardDocTypeRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _IInwardDocTypeRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _IInwardDocTypeRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.InwardDocTypeId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteInwardDoc(long itemMasterDocListId, long tenantId)
        {
            var co = await _IInwardDocTypeRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _IInwardDocTypeRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<InspectDocTypeVM>> GetAllInspectDocList(long tenantId)
        {
            var allDocuType = _IInspectDocTypeRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<InspectDocTypeVM>>(allDocuType);
        }
        public async Task<InspectDocTypeVM> PostInspectDocList(InspectDocTypeVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<InspectDocType>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _IInspectDocTypeRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _IInspectDocTypeRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _IInspectDocTypeRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.InspectDocTypeId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteInspectDoc(long itemMasterDocListId, long tenantId)
        {
            var co = await _IInspectDocTypeRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _IInspectDocTypeRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }

        public async Task<IEnumerable<LineInspectDocTypeVM>> GetAllLineInspectDocList(long tenantId)
        {
            var allDocuType = _ILineInspectDocTypeRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<LineInspectDocTypeVM>>(allDocuType);
        }
        public async Task<LineInspectDocTypeVM> PostLineInspectDocList(LineInspectDocTypeVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<LineInspectDocType>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _ILineInspectDocTypeRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _ILineInspectDocTypeRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _ILineInspectDocTypeRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.LineInspectDocTypeId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteCont_RCA_CA_Log(long itemMasterDocListId, long tenantId)
        {
            var co = await _Cont_RCA_CA_LogRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Cont_RCA_CA_LogRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<bool> DeleteLineInspectDoc(long itemMasterDocListId, long tenantId)
        {
            var co = await _ILineInspectDocTypeRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _ILineInspectDocTypeRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<FinalInspectDocTypeVM>> GetAllFinalInspectDocList(long tenantId)
        {
            var allDocuType = _IFinalInspectDocTypeRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<FinalInspectDocTypeVM>>(allDocuType);
        }
        public async Task<FinalInspectDocTypeVM> PostFinalInspectDocList(FinalInspectDocTypeVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<FinalInspectDocType>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _IFinalInspectDocTypeRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _IFinalInspectDocTypeRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _IFinalInspectDocTypeRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.FinalInspectDocTypeId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteFinalInspectDoc(long itemMasterDocListId, long tenantId)
        {
            var co = await _IFinalInspectDocTypeRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _IFinalInspectDocTypeRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


        public async Task<IEnumerable<RcCaDocTypeVM>> GetAllRcCaDocList(long tenantId)
        {
            var allDocuType = _IRcCaDocTypeRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<RcCaDocTypeVM>>(allDocuType);
        }
        public async Task<RcCaDocTypeVM> PostRcCaDocList(RcCaDocTypeVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<RcCaDocType>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _IRcCaDocTypeRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _IRcCaDocTypeRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _IRcCaDocTypeRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.RcCaDocTypeId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteRcCaDoc(long itemMasterDocListId, long tenantId)
        {
            var co = await _IRcCaDocTypeRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _IRcCaDocTypeRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<NcLogStatusVM>> GetAllNcLogStatusList()
        {
            var allDocuType =await _INcLogStatusRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NcLogStatusVM>>(allDocuType);
        }
        public async Task<NcLogStatusVM> PostNcLogStatusList(NcLogStatusVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NcLogStatus>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _INcLogStatusRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _INcLogStatusRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _INcLogStatusRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Log_Status_List_Id = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNcLogStatus(long itemMasterDocListId)
        {
            var co = await _INcLogStatusRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId );
            if (co != null)
            {
                try
                {
                    _INcLogStatusRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }

        public async Task<OperationSettingsVM> PostOperationSettings(OperationSettingsVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<OperationSettings>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _operationSettingsRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _operationSettingsRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _operationSettingsRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.OperationSettingsId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<Cont_RCA_CA_LogVM> PostCont_RCA_CA_Log(Cont_RCA_CA_LogVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Cont_RCA_CA_Log>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Cont_RCA_CA_LogRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Cont_RCA_CA_LogRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Cont_RCA_CA_LogRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Cont_RCA_CA_LogId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<IEnumerable<OperationSettingsVM>> GetAllOperationSettings()
        {
            var allDocuType = await _operationSettingsRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OperationSettingsVM>>(allDocuType);
        }
        public async Task<IEnumerable<Cont_RCA_CA_LogVM>> GetAllCont_RCA_CA_Log(long tenantId)
        {
            var allDocuType = _Cont_RCA_CA_LogRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Cont_RCA_CA_LogVM>>(allDocuType);
        }
        public async Task<IEnumerable<NC_Decision_LogVM>> GetAllNC_Decision_Log(long tenantId)
        {
            var allDocuType = _NC_Decision_LogRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<NC_Decision_LogVM>>(allDocuType);
        }
        public async Task<NC_Decision_LogVM> PostNC_Decision_Log(NC_Decision_LogVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_Decision_Log>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_Decision_LogRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_Decision_LogRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_Decision_LogRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Decision_LogId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_Decision_Log(long itemMasterDocListId, long tenantId)
        {
            var co = await _NC_Decision_LogRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _NC_Decision_LogRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        
        public async Task<IEnumerable<NC_Wk_List_Tmpl_DetVM>> GetAllNC_Wk_List_Tmpl_Det(long tenantId)
        {
            var allDocuType = _NC_Wk_List_Tmpl_DetRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<NC_Wk_List_Tmpl_DetVM>>(allDocuType);
        }
        public async Task<NC_Wk_List_Tmpl_DetVM> PostNC_Wk_List_Tmpl_Det(NC_Wk_List_Tmpl_DetVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_Wk_List_Tmpl_Det>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_Wk_List_Tmpl_DetRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_Wk_List_Tmpl_DetRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_Wk_List_Tmpl_DetRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Wk_List_Tmpl_DetId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_Wk_List_Tmpl_Det(long itemMasterDocListId, long tenantId)
        {
            var co = await _NC_Wk_List_Tmpl_DetRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _NC_Wk_List_Tmpl_DetRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        
        public async Task<IEnumerable<NC_Disp_Decs_Appl_ListVM>> GetAllNC_Disp_Decs_Appl_List(long tenantId)
        {
            var allDocuType = _NC_Disp_Decs_Appl_ListRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<NC_Disp_Decs_Appl_ListVM>>(allDocuType);
        }
        public async Task<NC_Disp_Decs_Appl_ListVM> PostNC_Disp_Decs_Appl_List(NC_Disp_Decs_Appl_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_Disp_Decs_Appl_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_Disp_Decs_Appl_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_Disp_Decs_Appl_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_Disp_Decs_Appl_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Disp_Decs_Appl_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_Disp_Decs_Appl_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _NC_Disp_Decs_Appl_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _NC_Disp_Decs_Appl_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        
        public async Task<IEnumerable<NC_Wk_List_Tmpl_HeadVM>> GetAllNC_Wk_List_Tmpl_Head(long tenantId)
        {
            var allDocuType = _NC_Wk_List_Tmpl_HeadRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<NC_Wk_List_Tmpl_HeadVM>>(allDocuType);
        }
        public async Task<NC_Wk_List_Tmpl_HeadVM> PostNC_Wk_List_Tmpl_Head(NC_Wk_List_Tmpl_HeadVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_Wk_List_Tmpl_Head>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_Wk_List_Tmpl_HeadRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_Wk_List_Tmpl_HeadRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_Wk_List_Tmpl_HeadRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Wk_List_Tmpl_HeadId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_Wk_List_Tmpl_Head(long itemMasterDocListId, long tenantId)
        {
            var co = await _NC_Wk_List_Tmpl_HeadRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _NC_Wk_List_Tmpl_HeadRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<NC_Work_ListVM>> GetAllNC_Work_List(long tenantId)
        {
            var allDocuType = _NC_Work_ListRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<NC_Work_ListVM>>(allDocuType);
        }
        public async Task<NC_Work_ListVM> PostNC_Work_List(NC_Work_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_Work_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_Work_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_Work_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_Work_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Work_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_Work_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _NC_Work_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _NC_Work_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<NC_Wk_List_HeaderVM>> GetAllNC_Wk_List_Header(long tenantId)
        {
            var allDocuType = _NC_Wk_List_HeaderRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<NC_Wk_List_HeaderVM>>(allDocuType);
        }
        public async Task<NC_Wk_List_HeaderVM> PostNC_Wk_List_Header(NC_Wk_List_HeaderVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_Wk_List_Header>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_Wk_List_HeaderRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_Wk_List_HeaderRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_Wk_List_HeaderRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Wk_List_HeaderId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_Wk_List_Header(long itemMasterDocListId, long tenantId)
        {
            var co = await _NC_Wk_List_HeaderRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _NC_Wk_List_HeaderRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Cust_NC_Decs_Matrix_OptVM>> GetAllCust_NC_Decs_Matrix_Opt(long tenantId)
        {
            var allDocuType = _Cust_NC_Decs_Matrix_OptRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Cust_NC_Decs_Matrix_OptVM>>(allDocuType);
        }
        public async Task<Cust_NC_Decs_Matrix_OptVM> PostCust_NC_Decs_Matrix_Opt(Cust_NC_Decs_Matrix_OptVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Cust_NC_Decs_Matrix_Opt>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Cust_NC_Decs_Matrix_OptRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Cust_NC_Decs_Matrix_OptRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Cust_NC_Decs_Matrix_OptRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Cust_NC_Decs_Matrix_OptId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteCust_NC_Decs_Matrix_Opt(long itemMasterDocListId, long tenantId)
        {
            var co = await _Cust_NC_Decs_Matrix_OptRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Cust_NC_Decs_Matrix_OptRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Cust_NC_Decs_MatrixVM>> GetAllCust_NC_Decs_Matrix(long tenantId)
        {
            var allDocuType = _Cust_NC_Decs_MatrixRepository.GetRangeAsync(c=>c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Cust_NC_Decs_MatrixVM>>(allDocuType);
        }
        public async Task<Cust_NC_Decs_MatrixVM> PostCust_NC_Decs_Matrix(Cust_NC_Decs_MatrixVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Cust_NC_Decs_Matrix>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Cust_NC_Decs_MatrixRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Cust_NC_Decs_MatrixRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Cust_NC_Decs_MatrixRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Cust_NC_Decs_MatrixId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteCust_NC_Decs_Matrix(long itemMasterDocListId, long tenantId)
        {
            var co = await _Cust_NC_Decs_MatrixRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Cust_NC_Decs_MatrixRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Cont_RCA_CA_Status_ListVM>> GetAllCont_RCA_CA_Status_List()
        {
            var allDocuType = _Cont_RCA_CA_Status_ListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Cont_RCA_CA_Status_ListVM>>(allDocuType);
        }
        public async Task<Cont_RCA_CA_Status_ListVM> PostCont_RCA_CA_Status_List(Cont_RCA_CA_Status_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Cont_RCA_CA_Status_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Cont_RCA_CA_Status_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Cont_RCA_CA_Status_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Cont_RCA_CA_Status_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Cont_RCA_CA_Status_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteCont_RCA_CA_Status_List(long itemMasterDocListId)
        {
            var co = await _Cont_RCA_CA_Status_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId );
            if (co != null)
            {
                try
                {
                    _Cont_RCA_CA_Status_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<NC_Disp_Decision_ListVM>> GetAllNC_Disp_Decision_List()
        {
            var allDocuType =await _NC_Disp_Decision_ListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NC_Disp_Decision_ListVM>>(allDocuType);
        }
        public async Task<NC_Disp_Decision_ListVM> PostNC_Disp_Decision_List(NC_Disp_Decision_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_Disp_Decision_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_Disp_Decision_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_Disp_Decision_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_Disp_Decision_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_Disp_Decision_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_Disp_Decision_List(long itemMasterDocListId)
        {
            var co = await _NC_Disp_Decision_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId );
            if (co != null)
            {
                try
                {
                    _NC_Disp_Decision_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<NC_work_StatusVM>> GetAllNC_work_Status()
        {
            var allDocuType = _NC_work_StatusRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NC_work_StatusVM>>(allDocuType);
        }
        public async Task<NC_work_StatusVM> PostNC_work_Status(NC_work_StatusVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<NC_work_Status>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _NC_work_StatusRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _NC_work_StatusRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _NC_work_StatusRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.NC_work_StatusId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNC_work_Status(long itemMasterDocListId)
        {
            var co = await _NC_work_StatusRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId );
            if (co != null)
            {
                try
                {
                    _NC_work_StatusRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }

        public async Task<IEnumerable<Mc_Not_Avl_ReasonVM>> GetAllMc_Not_Avl_Reason(long tenantId)
        {
            var allDocuType = _Mc_Not_Avl_ReasonRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Mc_Not_Avl_ReasonVM>>(allDocuType);
        }
        public async Task<IEnumerable<Mode_ListVM>> GetAllMode_List()
        {
            var allDocuType = await _Mode_ListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Mode_ListVM>>(allDocuType);
        }
        public async Task<IEnumerable<Non_Plan_Wk_type_ListVM>> GetAllNon_Plan_Wk_type_List()
        {
            var allDocuType = await _Non_Plan_Wk_type_ListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Non_Plan_Wk_type_ListVM>>(allDocuType);
        }
        public async Task<IEnumerable<Time_Slot_AllocationVM>> GetAllTime_Slot_Allocation()
        {
            var allDocuType = await _Time_Slot_AllocationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Time_Slot_AllocationVM>>(allDocuType);
        }
        public async Task<Mc_Not_Avl_ReasonVM> PostMc_Not_Avl_Reason(Mc_Not_Avl_ReasonVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Mc_Not_Avl_Reason>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Mc_Not_Avl_ReasonRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Mc_Not_Avl_ReasonRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Mc_Not_Avl_ReasonRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Mc_Not_Avl_ReasonId = itemMaster.Id;
            return itemMasterDocList;
        }

        public async Task<IEnumerable<Mc_Timeslot_ListVM>> GetAllMc_Timeslot_List(long tenantId)
        {
            var allDocuType = _Mc_Timeslot_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Mc_Timeslot_ListVM>>(allDocuType);
        }
        public async Task<Mc_Timeslot_ListVM> PostMc_Timeslot_List(Mc_Timeslot_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Mc_Timeslot_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Mc_Timeslot_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Mc_Timeslot_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Mc_Timeslot_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Mc_Timeslot_List_Id = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<IEnumerable<Mc_Timeslot_ListVM>> GetAllMc_Timeslot_ListByMcWait(long mcWaitId, long tenantId)
        {
            var allDocuType = _Mc_Timeslot_ListRepository.GetRangeAsync(c => c.Mc_Wait_List_Id == mcWaitId && c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Mc_Timeslot_ListVM>>(allDocuType);
        }
        public async Task<bool> DeleteMc_Timeslot_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _Mc_Timeslot_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Mc_Timeslot_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<TempMc_Timeslot_ListVM>> GetAllTempMc_Timeslot_List(long tenantId)
        {
            var allDocuType = _TempMc_Timeslot_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TempMc_Timeslot_ListVM>>(allDocuType);
        }
        public async Task<TempMc_Timeslot_ListVM> PostTempMc_Timeslot_List(TempMc_Timeslot_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<TempMc_Timeslot_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _TempMc_Timeslot_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _TempMc_Timeslot_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _TempMc_Timeslot_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.TempMc_Timeslot_List_Id = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<IEnumerable<TempMc_Timeslot_ListVM>> GetAllTempMc_Timeslot_ListByMcWait(long mcWaitId, long tenantId)
        {
            var allDocuType = _TempMc_Timeslot_ListRepository.GetRangeAsync(c => c.Mc_Wait_List_Id == mcWaitId && c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TempMc_Timeslot_ListVM>>(allDocuType);
        }
        public async Task<bool> DeleteTempMc_Timeslot_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _TempMc_Timeslot_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _TempMc_Timeslot_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Non_Plan_Wk_ListVM>> GetAllNon_Plan_Wk_List(long tenantId)
        {
            var allDocuType = _Non_Plan_Wk_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Non_Plan_Wk_ListVM>>(allDocuType);
        }
        public async Task<Non_Plan_Wk_ListVM> PostNon_Plan_Wk_List(Non_Plan_Wk_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Non_Plan_Wk_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    itemMaster.Allocated = 'N';
                    await _Non_Plan_Wk_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Non_Plan_Wk_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Non_Plan_Wk_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Non_Plan_Wk_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteNon_Plan_Wk_List(long itemMasterDocListId,long tenantId)
        {
            var co = await _Non_Plan_Wk_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Non_Plan_Wk_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<WO_Wait_ListVM>> GetAllWO_Wait_List(long tenantId)
        {
            var allDocuType = _WO_Wait_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<WO_Wait_ListVM>>(allDocuType);
        }
        public async Task<WO_Wait_ListVM> PostWO_Wait_List(WO_Wait_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<WO_Wait_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _WO_Wait_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _WO_Wait_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _WO_Wait_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.WO_Wait_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteWO_Wait_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _WO_Wait_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _WO_Wait_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<TempWO_Wait_ListVM>> GetAllTempWO_Wait_List(long tenantId)
        {
            var allDocuType = _TempWO_Wait_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TempWO_Wait_ListVM>>(allDocuType);
        }
        public async Task<TempWO_Wait_ListVM> PostTempWO_Wait_List(TempWO_Wait_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<TempWO_Wait_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _TempWO_Wait_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _TempWO_Wait_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _TempWO_Wait_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.TempWO_Wait_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteTempWO_Wait_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _TempWO_Wait_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _TempWO_Wait_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<WO_Bookout_LogVM>> GetAllWO_Bookout_Log(long tenantId)
        {
            var allDocuType = _WO_Bookout_LogRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<WO_Bookout_LogVM>>(allDocuType);
        }
        public async Task<WO_Bookout_LogVM> PostWO_Bookout_Log(WO_Bookout_LogVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<WO_Bookout_Log>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _WO_Bookout_LogRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _WO_Bookout_LogRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _WO_Bookout_LogRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.WO_Bookout_LogId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteWO_Bookout_Log(long itemMasterDocListId)
        {
            var co = await _WO_Bookout_LogRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId);
            if (co != null)
            {
                try
                {
                    _WO_Bookout_LogRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Timeslot_SettingVM>> GetAllTimeslot_Setting(long tenantId)
        {
            var allDocuType = _Timeslot_SettingRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Timeslot_SettingVM>>(allDocuType);
        }
        public async Task<Timeslot_SettingVM> PostTimeslot_Setting(Timeslot_SettingVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Timeslot_Setting>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Timeslot_SettingRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Timeslot_SettingRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Timeslot_SettingRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Timeslot_SettingId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteTimeslot_Setting(long itemMasterDocListId)
        {
            var co = await _Timeslot_SettingRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId);
            if (co != null)
            {
                try
                {
                    _Timeslot_SettingRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Timeslot_ListVM>> GetAllTimeslot_List(long tenantId)
        {
            var allDocuType = _Timeslot_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Timeslot_ListVM>>(allDocuType);
        }
        public async Task<Timeslot_ListVM> PostTimeslot_List(Timeslot_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Timeslot_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Timeslot_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Timeslot_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Timeslot_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Timeslot_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteTimeslot_List(long itemMasterDocListId,long tenantId)
        {
            var co = await _Timeslot_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Timeslot_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Rwk_ListVM>> GetAllRwk_List(long tenantId)
        {
            var allDocuType = _Rwk_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Rwk_ListVM>>(allDocuType);
        }
        public async Task<Rwk_ListVM> PostRwk_List(Rwk_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Rwk_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Rwk_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Rwk_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Rwk_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Rwk_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteRwk_List(long itemMasterDocListId,long tenantId)
        {
            var co = await _Rwk_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Rwk_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Opr_ListVM>> GetAllOpr_List(long tenantId)
        {
            var allDocuType = _Opr_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Opr_ListVM>>(allDocuType);
        }
        public async Task<Opr_ListVM> PostOpr_List(Opr_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Opr_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Opr_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Opr_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Opr_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Opr_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteOpr_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _Opr_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Opr_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<TempOpr_ListVM>> GetAllTempOpr_List(long tenantId)
        {
            var allDocuType = _TempOpr_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TempOpr_ListVM>>(allDocuType);
        }
        public async Task<TempOpr_ListVM> PostTempOpr_List(TempOpr_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<TempOpr_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _TempOpr_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _TempOpr_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _TempOpr_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.TempOpr_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteTempOpr_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _TempOpr_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _TempOpr_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Shop_Insp_LogVM>> GetAllShop_Insp_Log(long tenantId)
        {
            var allDocuType = _Shop_Insp_LogRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Shop_Insp_LogVM>>(allDocuType);
        }
        public async Task<Shop_Insp_LogVM> PostShop_Insp_Log(Shop_Insp_LogVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Shop_Insp_Log>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Shop_Insp_LogRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Shop_Insp_LogRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Shop_Insp_LogRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Shop_Insp_LogId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteShop_Insp_Log(long itemMasterDocListId,long tenantId)
        {
            var co = await _Shop_Insp_LogRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId== tenantId);
            if (co != null)
            {
                try
                {
                    _Shop_Insp_LogRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        
        public async Task<IEnumerable<SubCon_ListVM>> GetAllSubCon_List(long tenantId)
        {
            var allDocuType = _SubCon_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<SubCon_ListVM>>(allDocuType);
        }
        public async Task<SubCon_ListVM> PostSubCon_List(SubCon_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<SubCon_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _SubCon_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _SubCon_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _SubCon_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.SubCon_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteSubCon_List(long itemMasterDocListId,long tenantId)
        {
            var co = await _SubCon_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId== tenantId);
            if (co != null)
            {
                try
                {
                    _SubCon_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
       
        public async Task<IEnumerable<TempSubCon_ListVM>> GetAllTempSubCon_List(long tenantId)
        {
            var allDocuType = _TempSubCon_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TempSubCon_ListVM>>(allDocuType);
        }
        public async Task<TempSubCon_ListVM> PostTempSubCon_List(TempSubCon_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<TempSubCon_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _TempSubCon_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _TempSubCon_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _TempSubCon_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.TempSubCon_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteTempSubCon_List(long itemMasterDocListId,long tenantId)
        {
            var co = await _TempSubCon_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId== tenantId);
            if (co != null)
            {
                try
                {
                    _TempSubCon_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Mc_Wait_ListVM>> GetAllMc_Wait_List(long tenantId)
        {
            var allDocuType = _Mc_Wait_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Mc_Wait_ListVM>>(allDocuType);
        }
        public async Task<Mc_Wait_ListVM> PostMc_Wait_List(Mc_Wait_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Mc_Wait_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Mc_Wait_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Mc_Wait_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Mc_Wait_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Mc_Wait_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<IEnumerable<Mc_Wait_ListVM>> GetAllMc_Wait_ListByMcWait(long mcWaitId, long tenantId)
        {
            var allDocuType = _Mc_Wait_ListRepository.GetRangeAsync(c => c.Wo_Id == mcWaitId && c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Mc_Wait_ListVM>>(allDocuType);
        }
        public async Task<bool> DeleteMc_Wait_List(long itemMasterDocListId,long tenantId)
        {
            var co = await _Mc_Wait_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId== tenantId);
            if (co != null)
            {
                try
                {
                    _Mc_Wait_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllTempMc_Wait_List(long tenantId)
        {
            var allDocuType = _TempMc_Wait_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TempMc_Wait_ListVM>>(allDocuType);
        }
        public async Task<TempMc_Wait_ListVM> PostTempMc_Wait_List(TempMc_Wait_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<TempMc_Wait_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _TempMc_Wait_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _TempMc_Wait_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _TempMc_Wait_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.TempMc_Wait_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllTempMc_Wait_ListByMcWait(long mcWaitId, long tenantId)
        {
            var allDocuType = _TempMc_Wait_ListRepository.GetRangeAsync(c => c.Wo_Id == mcWaitId && c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TempMc_Wait_ListVM>>(allDocuType);
        }
        public async Task<bool> DeleteTempMc_Wait_List(long itemMasterDocListId,long tenantId)
        {
            var co = await _TempMc_Wait_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId== tenantId);
            if (co != null)
            {
                try
                {
                    _TempMc_Wait_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }

        public async Task<IEnumerable<Matl_Issue_ListVM>> GetAllMatl_Issue_List(long tenantId)
        {
            var allDocuType = _Matl_Issue_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Matl_Issue_ListVM>>(allDocuType);
        }
        public async Task<Matl_Issue_ListVM> PostMatl_Issue_List(Matl_Issue_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Matl_Issue_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Matl_Issue_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Matl_Issue_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Matl_Issue_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Matl_Issue_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteMatl_Issue_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _Matl_Issue_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Matl_Issue_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        
        public async Task<IEnumerable<Matl_Issue_SettingsVM>> GetAllMatl_Issue_Settings(long tenantId)
        {
            var allDocuType = _Matl_Issue_SettingsRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Matl_Issue_SettingsVM>>(allDocuType);
        }
        public async Task<Matl_Issue_SettingsVM> PostMatl_Issue_Settings(Matl_Issue_SettingsVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Matl_Issue_Settings>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Matl_Issue_SettingsRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Matl_Issue_SettingsRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Matl_Issue_SettingsRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Matl_Issue_SettingsId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteMatl_Issue_Settings(long itemMasterDocListId, long tenantId)
        {
            var co = await _Matl_Issue_SettingsRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Matl_Issue_SettingsRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


    }
}
