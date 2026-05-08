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
        private readonly IDispatchDetailsRepository _DispatchDetailsRepository;
        private readonly IDispatchQntyRepository _DispatchQntyRepository;
        private readonly IProductionPlan_WORepository _productionPlan_WORepository;
        private readonly IWOStatusRepository _wOStatusrepository;
        private readonly ICust_NC_DecisionRepository _Cust_NC_Decisionrepository;
        private readonly IChildWoRelRepository _childWoRelRepository;
        private readonly IMcTimeListRepository _mcTimeListRepository;
        private readonly IPODetailsRepository _poDetailsRepository;
        private readonly IPOHeaderRepository _poHeaderRepository;
        private readonly IPOStatusRepository _poStatusRepository;
        private readonly IWoSubConSupplierRepository _woSubConSupplierRepository;
        private readonly IPOLogRepository _pOLogRepository;
        private readonly IInward_Condn_listRepository _IInward_Condn_listRepository;
        private readonly ISetupVariationReasonRepository _ISetupVariationReasonRepository;
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
        private readonly IInv_Mismatch_ListRepository _Inv_Mismatch_ListRepository;
        private readonly IInv_Master_LogRepository _Inv_Master_LogRepository;
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
        private readonly IConsolidatedWoMappingRepository _COnsolidatedWomappingRepository;
        private readonly IInv_Trans_ListRepository _Inv_Trans_ListRepository;

        public WOService(
            ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork
            , IWorkOrderRepository workOrderRepository , IPOLogRepository pOLogRepository
            , IProcPlanRepository procPlanRepository, IWOSORepository woso, IBOMTempRepository bOMTempRepository, IBOMListRepository bOMListRepository,IDispatchQntyRepository DispatchQntyRepository,IDispatchDetailsRepository DispatchDetailsRepository,
            IProductionPlan_WORepository productionPlan_WORepository, IWOStatusRepository wOStatus,ICust_NC_DecisionRepository Cust_NC_Decision, IChildWoRelRepository childWoRelRepository
            , IMcTimeListRepository mcTimeListRepository, IPODetailsRepository pODetailsRepository,IPOHeaderRepository pOHeaderRepository,IPOStatusRepository pOStatusRepository,
            IWoSubConSupplierRepository woSubConSupplierRepository, IProcPlanPartPurChaseRelRepository purChaseRelRepository, IInward_Condn_listRepository IInward_Condn_listRepository,ISetupVariationReasonRepository ISetupVariationReasonRepository,
            IInsp_Outcome_DetailsRepository iInsp_Outcome_DetailsRepository, 
            IInsp_Outcome_ListRepository insp_Outcome_ListRepository, IInventory_MasterRepository Inventory_Master
            ,IInv_Trans_LogRepository inv_Trans_LogRepository, IInw_Recpt_DetailsRepository inw_Recpt_DetailsRepository
            ,IInw_Recpt_HeaderRepository Inw_Recpt_Header, IInw_Recpt_Part_NoRepository Inw_Recpt_Part_No,
            IInwardDocTypeRepository InwardDocTypeRepository,IRcCaDocTypeRepository RcCaDocTypeRepository
            , ILineInspectDocTypeRepository LineInspectDocTypeRepository, IFinalInspectDocTypeRepository FinalInspectDocTypeRepository,
            IInspectDocTypeRepository InspectDocTypeRepository,INcLogStatusRepository ncLogStatusRepository,IOperationSettingsRepository operationSettingsRepository,ICont_RCA_CA_LogRepository Cont_RCA_CA_LogRepository,IInv_Mismatch_ListRepository Inv_Mismatch_ListRepository,IInv_Master_LogRepository Inv_Master_LogRepository
            ,INC_Decision_LogRepository NC_Decision_LogRepository,INC_Wk_List_Tmpl_DetRepository NC_Wk_List_Tmpl_DetRepository ,INC_Disp_Decs_Appl_ListRepository NC_Disp_Decs_Appl_ListRepository  ,INC_Wk_List_Tmpl_HeadRepository NC_Wk_List_Tmpl_HeadRepository  ,INC_Work_ListRepository NC_Work_ListRepository ,INC_Wk_List_HeaderRepository NC_Wk_List_HeaderRepository ,ICust_NC_Decs_Matrix_OptRepositoy Cust_NC_Decs_Matrix_OptRepository
            ,ICust_NC_Decs_MatrixRepository Cust_NC_Decs_MatrixRepository ,ICont_RCA_CA_Status_ListRepository Cont_RCA_CA_Status_ListRepository,INC_Disp_Decision_ListRepository NC_Disp_Decision_ListRepository
            ,INC_work_StatusRepository NC_work_StatusRepository, IMc_Not_Avl_ReasonRepository Mc_Not_Avl_ReasonRepository, IMode_ListRepository Mode_ListRepository,
            IMc_Timeslot_ListRepository Mc_Timeslot_ListRepository,ITempMc_Timeslot_ListRepository TempMc_Timeslot_ListRepository,
            INon_Plan_Wk_ListRepository Non_Plan_Wk_ListRepository,IWO_Wait_ListRepository WO_Wait_ListRepository,ITempWO_Wait_ListRepository TempWO_Wait_ListRepository,IWO_Bookout_LogRepository WO_Bookout_LogRepository,ITimeslot_SettingRepository Timeslot_SettingRepository,ITimeslot_ListRepository Timeslot_ListRepository,IRwk_ListRepository Rwk_ListRepository,IOpr_ListRepository Opr_ListRepository,ITempOpr_ListRepository TempOpr_ListRepository,IShop_Insp_LogRepository Shop_Insp_LogRepository,ISubCon_ListRepository SubCon_ListRepository,ITempSubCon_ListRepository TempSubCon_ListRepository, IMc_Wait_ListRepository Mc_Wait_ListRepository,IMatl_Issue_SettingsRepository Matl_Issue_SettingsRepository,IMatl_Issue_ListRepository Matl_Issue_ListRepository,
            ITempMc_Wait_ListRepository TempMc_Wait_ListRepository, INon_Plan_Wk_type_ListRepository Non_Plan_Wk_type_ListRepository, ITime_Slot_AllocationRepository Time_Slot_AllocationRepository,IConsolidatedWoMappingRepository consolidatedWoMappingRepository, IInv_Trans_ListRepository Inv_Trans_ListRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _workOrderRepository = workOrderRepository;
            _wosoRepository = woso;
            _bOMTempRepository = bOMTempRepository;
            _procPlanRepository = procPlanRepository;
            _bOMListRepository = bOMListRepository;
            _DispatchDetailsRepository = DispatchDetailsRepository;
            _DispatchQntyRepository = DispatchQntyRepository;
            _productionPlan_WORepository = productionPlan_WORepository;
            _wOStatusrepository = wOStatus;
            _Cust_NC_Decisionrepository = Cust_NC_Decision;
            _childWoRelRepository = childWoRelRepository;
            _mcTimeListRepository = mcTimeListRepository;
            _poDetailsRepository = pODetailsRepository;
            _poHeaderRepository = pOHeaderRepository;
            _poStatusRepository = pOStatusRepository;
            _woSubConSupplierRepository = woSubConSupplierRepository;
            _pOLogRepository = pOLogRepository;
            _IProcPlanPartPurChaseRelRepository = purChaseRelRepository;
            _IInward_Condn_listRepository = IInward_Condn_listRepository;
            _ISetupVariationReasonRepository = ISetupVariationReasonRepository;
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
            _Inv_Mismatch_ListRepository = Inv_Mismatch_ListRepository;
            _Inv_Master_LogRepository = Inv_Master_LogRepository;
            _TempMc_Wait_ListRepository = TempMc_Wait_ListRepository;
            _COnsolidatedWomappingRepository = consolidatedWoMappingRepository;
            _Inv_Trans_ListRepository = Inv_Trans_ListRepository;
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
                     if (wkord.Plan_Proc_Qnty != pp.Plan_Proc_Qnty)
                    {
                        wkord.Old_Plan_Proc_Qnty = wkord.Plan_Proc_Qnty;
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
        public async Task<List<ProcPlanVM>> PostProcPlanPOFlag(List<ProcPlanVM> proc)
        {
            foreach (ProcPlanVM item in proc)
            {
                var pp = _mapper.Map<ProcPlan>(item);
                if (pp.Id> 0)
                {

                    var wkord = await _procPlanRepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
                    if (wkord == null)
                    {
                        return proc;
                    }
                    wkord.PO_Flag = pp.PO_Flag;
                    pp = await _procPlanRepository.UpdateAsync(wkord.Id, wkord);
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
                   

                    var rootWoId = co.Id;
                    var allWos = await _productionPlan_WORepository.GetAllAsync();

                    List<long> woIds = new List<long>();

                    void Collect(long parent)
                    {
                        var children = allWos.Where(x => x.ParentWoId == parent).ToList();

                        foreach (var child in children)
                        {
                            woIds.Add(child.WoId);
                           // Collect(child.WoId);
                        }
                    }

                    // include root
                    woIds.Add(rootWoId);
                    Collect(rootWoId);


                    //var pwo = await _productionPlan_WORepository.AwaitGetRangeAsync(p=>p.WoId == co.Id);
                    //foreach (var item in pwo)
                    //{
                    //    _productionPlan_WORepository.Remove(item);
                    //    await _unitOfWork.CommitAsync();
                    //}
                    //var cworel =await _childWoRelRepository.AwaitGetRangeAsync(p => p.WoId == co.Id);
                    //foreach (var item in cworel)
                    //{
                    //    _childWoRelRepository.Remove(item);
                    //    await _unitOfWork.CommitAsync();
                    //}
                    //var mcTime =await _mcTimeListRepository.AwaitGetRangeAsync(p => p.WoId == co.Id);
                    //foreach (var item in mcTime)
                    //{
                    //    _mcTimeListRepository.Remove(item);
                    //    await _unitOfWork.CommitAsync();
                    //}
                    //var bom =await _bOMListRepository.AwaitGetRangeAsync(p => p.ParentWoId == co.Id);
                    //foreach (var item in bom)
                    //{
                    //    _bOMListRepository.Remove(item);
                    //    await _unitOfWork.CommitAsync();
                    //}
                    var bom = await _bOMListRepository.AwaitGetRangeAsync(p => woIds.Contains(p.ParentWoId));
                    foreach (var item in bom)
                    {
                        _bOMListRepository.Remove(item);
                    }
                       

                    var mcTime = await _mcTimeListRepository.AwaitGetRangeAsync(p => woIds.Contains(p.WoId));
                    foreach (var item in mcTime)
                    {
                        _mcTimeListRepository.Remove(item);
                    }
                       

                    var childRel = await _childWoRelRepository.AwaitGetRangeAsync(p => woIds.Contains(p.WoId));
                    foreach (var item in childRel)
                    {
                        _childWoRelRepository.Remove(item);
                    }
                        

                    var prodWos = await _productionPlan_WORepository.AwaitGetRangeAsync(p => woIds.Contains(p.WoId));
                    foreach (var item in prodWos)
                    {
                        _productionPlan_WORepository.Remove(item);
                    }
                       
                    var woso =await _wosoRepository.AwaitGetRangeAsync(p => p.WorkOrderId == co.Id);
                    foreach (var item in woso)
                    {
                        _wosoRepository.Remove(item);
                      
                    }
                    //var procplan =await _procPlanRepository.AwaitGetRangeAsync(p => p.WorkOrderId == co.Id);
                    //foreach (var item in procplan)
                    //{
                    //    _procPlanRepository.Remove(item);
                    //    var Podetails =await _poDetailsRepository.AwaitGetRangeAsync(p => p.ProcPlanId == item.Id);
                    //    foreach (var pod in Podetails)
                    //    {
                    //        var Pohead =await _poHeaderRepository.AwaitGetRangeAsync(p => p.PoDetailsId == pod.Id);
                    //        foreach (var pohead in Pohead)
                    //        {
                    //            _poHeaderRepository.Remove(pohead);
                    //            await _unitOfWork.CommitAsync();
                    //        }
                    //        _poDetailsRepository.Remove(pod);
                    //        await _unitOfWork.CommitAsync();
                    //    }
                    //    var purchaseRel = await _IProcPlanPartPurChaseRelRepository.AwaitGetRangeAsync(p => p.ProcPlanId == item.Id);
                    //    foreach (var pprel in purchaseRel)
                    //    {
                    //        _IProcPlanPartPurChaseRelRepository.Remove(pprel);
                    //        await _unitOfWork.CommitAsync();
                    //    }
                    //    await _unitOfWork.CommitAsync();
                    //}
                    var procPlans = await _procPlanRepository.AwaitGetRangeAsync(p => woIds.Contains(p.WorkOrderId));
                    var procPlanIds = procPlans.Select(p => p.Id).ToList();

                    var poDetails = await _poDetailsRepository.AwaitGetRangeAsync(p => procPlanIds.Contains(p.ProcPlanId));
                    var poDetailIds = poDetails.Select(p => p.Id).ToList();

                    var poHeaders = await _poHeaderRepository.AwaitGetRangeAsync(p => poDetailIds.Contains(p.PoDetailsId));

                    foreach (var item in poHeaders)
                    {
                        _poHeaderRepository.Remove(item);
                    }
                       

                    foreach (var item in poDetails)
                    {
                        _poDetailsRepository.Remove(item);
                    }
                        

                    var purchaseRel = await _IProcPlanPartPurChaseRelRepository.AwaitGetRangeAsync(p => procPlanIds.Contains(p.ProcPlanId));

                    foreach (var item in purchaseRel)
                    {
                        _IProcPlanPartPurChaseRelRepository.Remove(item);
                    }
                        

                    foreach (var item in procPlans)
                    {
                        _procPlanRepository.Remove(item);
                    }
                    var bomtemp =await _bOMTempRepository.AwaitGetRangeAsync(p => p.WorkOrderId == co.Id);
                    foreach(var item  in bomtemp)
                    {
                        _bOMTempRepository.Remove(item);
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
        public async Task<List<ProductionPlan_WOVM>> PostProductionPlan_WoConsolidation(List<ProductionPlan_WOVM> productions)
        {
            foreach (ProductionPlan_WOVM item in productions)
            {
                var pp = _mapper.Map<ProductionPlan_WO>(item);
                if (pp.SalesOrderId > 0)
                {
                    if (pp.Id > 0)
                    {
                        
                        var upp = await _productionPlan_WORepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
                        upp.Consolidation_Flag = 1;
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
        public async Task<List<ProductionPlan_WOVM>> UpdateProductionPlan_WoForReference(List<ProductionPlan_WOVM> productions)
        {
            foreach (ProductionPlan_WOVM item in productions)
            {
                var pp = _mapper.Map<ProductionPlan_WO>(item);
                if (pp.SalesOrderId > 0)
                {
                    if (pp.Id > 0)
                    {

                        var upp = await _productionPlan_WORepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
                        upp.For_Ref = 'Y';
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
        public async Task<List<ConsolidatedWoMappingVM>> PostConsolidationWo(List<ConsolidatedWoMappingVM> productions)
        {
            foreach (ConsolidatedWoMappingVM item in productions)
            {
                var pp = _mapper.Map<ConsolidatedWoMapping>(item);
                
                    if (pp.Id == 0)
                    {

                        try
                        {
                            await _COnsolidatedWomappingRepository.AddAsync(pp);
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
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
               
            
            return productions;
        }
        public async Task<IEnumerable<ConsolidatedWoMappingVM>> Getallconsolidationwo(long tenantId)
        {
            var allpp = _COnsolidatedWomappingRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<ConsolidatedWoMappingVM>>(allpp);
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
            upp.ActCompletionDate = pp.ActCompletionDate;
            upp.ActWOQty = pp.ActWOQty;
            upp.Status = pp.Status;
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
        public async Task<ProductionPlan_WOVM> UpdateProductionPlan_WoCriticalPart(ProductionPlan_WOVM productions)
        {
           var pp = _mapper.Map<ProductionPlan_WO>(productions);
            var upp = await _productionPlan_WORepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
            if (upp == null)
            {
                return productions;
            }
            upp.CriticalPart = 1;
           
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
        public async Task<ProductionPlan_WOVM> UpdateHoldProductionPlan_Wo(ProductionPlan_WOVM productions)
        {
            var pp = _mapper.Map<ProductionPlan_WO>(productions);
            var upp = await _productionPlan_WORepository.SingleOrDefaultAsync(x => x.Id == pp.Id);
            if (upp == null)
            {
                return productions;
            }
            upp.Comment = pp.Comment;
            upp.Status = pp.Status;
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
            var allpp =  _productionPlan_WORepository.GetRangeAsync(d => d.TenantId == tenantId);
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
        public async Task<Inv_Trans_ListVM> GetInvTransDescName(long Id)
        {
            var allpp = await _Inv_Trans_ListRepository.SingleOrDefaultAsync(d => d.Id == Id);
            if (allpp != null)
            {
                return _mapper.Map<Inv_Trans_ListVM>(allpp);
            }
            return new Inv_Trans_ListVM { TransactionId = -1 };
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
                    if(po.Status == 2)
                    {
                        poLog.OldValue = "Not Aprroved";
                        poLog.NewValue = "PO Aprroved";
                    }else if(po.Status == 3)
                    {
                        poLog.OldValue = "PO Aprroved";
                        poLog.NewValue = "Compeleted";
                    }
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
        public async Task<List<PODetailsVM>> UpdateInspection(List<PODetailsVM> pODetailsVM)
        {
            foreach (PODetailsVM item in pODetailsVM)
            {
                var po = _mapper.Map<PODetails>(item);
                if (po.Id >0 )
                {
                    
                    var upp = await _poDetailsRepository.SingleOrDefaultAsync(x => x.Id == po.Id);
                    upp.Inspection = 'Y';
                    await _poDetailsRepository.UpdateAsync(po.Id, upp);
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
            var allwo = await _IInventory_MasterRepository.AwaitGetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inventory_MasterVM>>(allwo);
        }
        public async Task<IEnumerable<Inventory_MasterVM>> GetAllInventory_MasterBypartid(long LocationId,long OprnoId, long RoutingId,long Partid,long tenantId )
        {
            var allwo = await _IInventory_MasterRepository.AwaitGetRangeAsync(d => d.TenantId == tenantId && d.Part_NoId==Partid && d.Location_Id==LocationId 
            && d.Opr_No_Id==OprnoId && d.Routing_Id==RoutingId );
            return _mapper.Map<IEnumerable<Inventory_MasterVM>>(allwo);
        }
        public async Task<IEnumerable<Inventory_MasterVM>> GetAllInventory_MasterBypartidWithFlag(string Flag,long LocationId, long OprnoId, long RoutingId, long Partid, long tenantId)
        {
            var allwo = await _IInventory_MasterRepository.AwaitGetRangeAsync(d => d.TenantId == tenantId && d.Part_NoId == Partid && d.Location_Id == LocationId
            && d.Opr_No_Id == OprnoId && d.Routing_Id == RoutingId && d.Loc_Flag==Flag);
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
        public async Task<IEnumerable<SetupVariationReasonVM>> GetAllSetupVariationReason()
        {
            var allwo = await _ISetupVariationReasonRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SetupVariationReasonVM>>(allwo);
        }
        public async Task<IEnumerable<Cust_NC_DecisionVM>> GetAllCust_NC_Decision()
        {
            var allwo = await _Cust_NC_Decisionrepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Cust_NC_DecisionVM>>(allwo);
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
            var invmasterlog = new Inv_Master_Log();
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
                try
                {
                    var list = await _IInventory_MasterRepository
    .AwaitGetRangeAsync(x => x.Id == wo.Id || x.Part_NoId == wo.Part_NoId);

                    var wkord = list.FirstOrDefault();   // SAFE — will never throw

                    if (wo.Routing_Id != wkord.Routing_Id)
                    {
                        invmasterlog.Field_Changed = "Routing";
                        invmasterlog.Old_Value = wkord.Routing_Id.ToString();
                        invmasterlog.New_Value = wo.Routing_Id.ToString();
                        invmasterlog.Reason_Desc = "-";
                    }
                    if (wo.Opr_No_Id != wkord.Opr_No_Id)
                    {
                        invmasterlog.Field_Changed = "Opr_No";
                        invmasterlog.Old_Value = wkord.Opr_No_Id.ToString();
                        invmasterlog.New_Value = wo.Opr_No_Id.ToString();
                        invmasterlog.Reason_Desc = "-";
                    }
                    if (wo.Current_QntOnHand != wkord.Current_QntOnHand)
                    {
                        invmasterlog.Field_Changed = "Current_QntOnHand";
                        invmasterlog.Old_Value = wkord.Current_QntOnHand.ToString();
                        invmasterlog.New_Value = wo.Current_QntOnHand.ToString();
                        invmasterlog.Reason_Desc = "-";
                    }
                    if (wo.Location_Id != wkord.Location_Id)
                    {
                        invmasterlog.Field_Changed = "Location";
                        invmasterlog.Old_Value = wkord.Location_Id.ToString();
                        invmasterlog.New_Value = wo.Location_Id.ToString();
                        invmasterlog.Reason_Desc = "-";
                    }
                    if (workOrdersVM.ReasonDesc == null)
                    {
                       // wo.Current_QntOnHand = wkord.Current_QntOnHand + wo.Current_QntOnHand;
                    }
                    if (wkord == null)
                    {
                        return workOrdersVM;
                    }
                }
                catch (Exception ex)
                {

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
            if(wo.Id == 0)
            {
                invmasterlog.Inv_mast_ID = wo.Id;
                invmasterlog.Dt_time = DateTime.Now;
                invmasterlog.Part_No = wo.Part_NoId;
                invmasterlog.Changed_by = wo.TenantId;
                invmasterlog.Field_Changed = "New Entry";
                invmasterlog.Old_Value = "-";
                invmasterlog.New_Value = wo.Current_QntOnHand.ToString();
                invmasterlog.Reason_Desc = "-";
                invmasterlog.Financial_Impact = 0;
                invmasterlog.TenantId = wo.TenantId;
                await _Inv_Master_LogRepository.AddAsync(invmasterlog);
            }
            else
            {
                invmasterlog.Inv_mast_ID = wo.Id;
                invmasterlog.Dt_time = DateTime.Now;
                invmasterlog.Part_No = wo.Part_NoId;
                invmasterlog.Changed_by = wo.TenantId;
                invmasterlog.Financial_Impact = 0;
                if(workOrdersVM.ReasonDesc != null)
                {
                    invmasterlog.Reason_Desc = workOrdersVM.ReasonDesc;
                }
                invmasterlog.TenantId = wo.TenantId;
                await _Inv_Master_LogRepository.AddAsync(invmasterlog);
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
       
        public async Task<SetupVariationReasonVM> PostSetupVariationReason(SetupVariationReasonVM workOrdersVM)
        {
            var wo = _mapper.Map<SetupVariationReason>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    await _ISetupVariationReasonRepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _ISetupVariationReasonRepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _ISetupVariationReasonRepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.SetupVariationReasonId = wo.Id;
            return workOrdersVM;
        }
        public async Task<Cust_NC_DecisionVM> PostCust_NC_Decision(Cust_NC_DecisionVM workOrdersVM)
        {
            var wo = _mapper.Map<Cust_NC_Decision>(workOrdersVM);
            if (wo.Id == 0)
            {
                try
                {
                    await _Cust_NC_Decisionrepository.AddAsync(wo);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var wkord = await _Cust_NC_Decisionrepository.SingleOrDefaultAsync(x => x.Id == wo.Id);
                if (wkord == null)
                {
                    return workOrdersVM;
                }
                wo = await _Cust_NC_Decisionrepository.UpdateAsync(wo.Id, wo);
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
            workOrdersVM.Cust_DecisionId = wo.Id;
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
            var allDocuType =await _Cont_RCA_CA_Status_ListRepository.GetAllAsync();
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
        
        public async Task<IEnumerable<DispatchDetailsVM>> GetAllDispatchDetails(long tenantId)
        {
            var allDocuType = _DispatchDetailsRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<DispatchDetailsVM>>(allDocuType);
        }
        public async Task<DispatchDetailsVM> PostDispatchDetails(DispatchDetailsVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<DispatchDetails>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _DispatchDetailsRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _DispatchDetailsRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _DispatchDetailsRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.DispatchDetailsId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteDispatchDetails(long itemMasterDocListId, long tenantId)
        {
            var co = await _DispatchDetailsRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _DispatchDetailsRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<DispatchQntyVM>> GetAllDispatchQnty(long tenantId)
        {
            var allDocuType = _DispatchQntyRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<DispatchQntyVM>>(allDocuType);
        }
        public async Task<DispatchQntyVM> PostDispatchQnty(DispatchQntyVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<TempDispatchQnty>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _DispatchQntyRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _DispatchQntyRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _DispatchQntyRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.DispatchQntyId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteDispatchQnty(long itemMasterDocListId, long tenantId)
        {
            var co = await _DispatchQntyRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _DispatchQntyRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


        public async Task<IEnumerable<Inv_Master_LogVM>> GetAllInv_Master_Log(long tenantId)
        {
            var allDocuType = _Inv_Master_LogRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inv_Master_LogVM>>(allDocuType);
        }
        public async Task<Inv_Master_LogVM> PostInv_Master_Log(Inv_Master_LogVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Inv_Master_Log>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Inv_Master_LogRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Inv_Master_LogRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Inv_Master_LogRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Inv_Master_LogId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteInv_Master_Log(long itemMasterDocListId, long tenantId)
        {
            var co = await _Inv_Master_LogRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Inv_Master_LogRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        
        public async Task<IEnumerable<Inv_Mismatch_ListVM>> GetAllInv_Mismatch_List(long tenantId)
        {
            var allDocuType = _Inv_Mismatch_ListRepository.GetRangeAsync(c => c.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Inv_Mismatch_ListVM>>(allDocuType);
        }
        public async Task<Inv_Mismatch_ListVM> PostInv_Mismatch_List(Inv_Mismatch_ListVM itemMasterDocList)
        {
            var itemMaster = _mapper.Map<Inv_Mismatch_List>(itemMasterDocList);
            if (itemMaster.Id == 0)
            {
                try
                {
                    await _Inv_Mismatch_ListRepository.AddAsync(itemMaster);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var itemMasterDoc = await _Inv_Mismatch_ListRepository.SingleOrDefaultAsync(x => x.Id == itemMaster.Id);
                if (itemMasterDoc == null)
                {
                    return itemMasterDocList;
                }
                itemMaster = await _Inv_Mismatch_ListRepository.UpdateAsync(itemMasterDoc.Id, itemMaster);
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
            itemMasterDocList.Inv_Mismatch_ListId = itemMaster.Id;
            return itemMasterDocList;
        }
        public async Task<bool> DeleteInv_Mismatch_List(long itemMasterDocListId, long tenantId)
        {
            var co = await _Inv_Mismatch_ListRepository.SingleOrDefaultAsync(m => m.Id == itemMasterDocListId && m.TenantId == tenantId);
            if (co != null)
            {
                try
                {
                    _Inv_Mismatch_ListRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllSetUpApprolList(long tenantId)
        {
            var result = new List<TempMc_Wait_ListVM>();

            // 1. Load all local master data in parallel
            var waitListTask = GetAllMc_Wait_List(tenantId);
            var timeslotsTask = GetAllTimeslot_List(tenantId);
            var allWOTask = AllProductionWo(tenantId);
            var translogsTask = GetAllInvTransLog(tenantId);

            await Task.WhenAll(waitListTask, timeslotsTask, allWOTask, translogsTask);

            // 2. Filter the Main List
            var rawWaitList = waitListTask.Result;
            var waitList = rawWaitList
                .Where(x => x.Setup_Start_time != null && x.Setup_Apprvl_time == null)
                .ToList();

            if (waitList.Count == 0) return result;

            // 3. Convert Lists to Dictionaries
            var allWO = allWOTask.Result
                .GroupBy(x => x.ProductionPlanId).ToDictionary(g => g.Key, g => g.First());

            var timeSlots = timeslotsTask.Result
                .GroupBy(x => x.Timeslot_ListId).ToDictionary(g => g.Key, g => g.First());

            // 4. Optimize Translogs (Assuming Input_Part_NoId and Output_Part_No are the link keys)
            var translogs = translogsTask.Result;

            var translogInputIndex = translogs
                .GroupBy(t => t.Input_Part_NoId)
                .ToDictionary(g => g.Key, g => g.First());

            var translogOutputIndex = translogs
                .GroupBy(t => t.Output_Part_No)
                .ToDictionary(g => g.Key, g => g.First());

            // 5. The Loop
            foreach (var mcWait in waitList)
            {
                // A. Fast Fail lookups
                if (!allWO.TryGetValue(mcWait.Wo_Id, out var wo)) continue;

                // Note: Machine lookup removed as it requires external DB call

                // B. Translog Logic
                Inv_Trans_LogVM translog = null;

                // Try Input Part ID
                if (!translogInputIndex.TryGetValue(wo.PartId, out translog))
                {
                    // Fallback: Try Output Part ID
                    translogOutputIndex.TryGetValue(wo.PartId, out translog);
                }

                if (translog == null) continue;

                // C. Data Mapping
                var currentStart = timeSlots.GetValueOrDefault(mcWait.Plan_start_time_Id)?.Start_time;

                result.Add(new TempMc_Wait_ListVM
                {
                    // External data fields left empty as per instructions to exclude other API calls
                    ShopName = "",
                    McName = "",
                    PartNo = "",
                    RoutingName = "",
                    OprNoName = "",
                    PlannedSetupTime = "",
                    Mc_Id = mcWait.Mc_Id,
                    Wo_Id = mcWait.Wo_Id,
                    Opr_No_Id = mcWait.Opr_No_Id,
                    // Local Data mapping
                    WoNumber = wo.WONumber ?? "",
                    WoQnty = wo.CalcWOQty.ToString(),
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,
                    MatlIssued = Convert.ToInt32(translog.Qnty).ToString(),
                    PlanStartStr = currentStart?.ToString("hh:mm tt") ?? "",
                    SetUpTimeStr = mcWait.Setup_Start_time?.ToString("hh:mm tt") ?? "",
                    MatlReceptTime = translog.Dt_time.ToString("hh:mm tt"),
                    Rework_Wo = 'N',
                    TenantId = tenantId
                });
            }

            return result;
        }
        // ... inside class WOService ...
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllSetUpCnfList(long tenantId)
        {
            var result = new List<TempMc_Wait_ListVM>();

            // 1. Load all local master data in parallel
            var waitListTask = GetAllMc_Wait_List(tenantId);
            var timeslotsTask = GetAllTimeslot_List(tenantId);
            var allWOTask = AllProductionWo(tenantId);
            var translogsTask = GetAllInvTransLog(tenantId);

            await Task.WhenAll(waitListTask, timeslotsTask, allWOTask, translogsTask);

            var waitList = waitListTask.Result;
            // Note: No filter applied to waitList as per provided requirement code

            if (!waitList.Any()) return result;

            // 2. Convert Lists to Dictionaries/Lookups
            var allWO = allWOTask.Result
                .GroupBy(x => x.ProductionPlanId)
                .ToDictionary(g => g.Key, g => g.First());

            var timeSlots = timeslotsTask.Result
                .GroupBy(x => x.Timeslot_ListId)
                .ToDictionary(g => g.Key, g => g.First());

            var translogs = translogsTask.Result;
            var translogInputLookup = translogs
                .GroupBy(x => x.Input_Part_NoId)
                .ToDictionary(g => g.Key, g => g.First());

            var translogOutputLookup = translogs
                .GroupBy(x => x.Output_Part_No)
                .ToDictionary(g => g.Key, g => g.First());

            // 3. The Loop
            foreach (var mcWait in waitList)
            {
                // A. Fast Fail lookups for WO
                if (!allWO.TryGetValue(mcWait.Wo_Id, out var wo)) continue;

                // B. Translog Logic
                Inv_Trans_LogVM translog = null;

                // Try Input Part first
                if (!translogInputLookup.TryGetValue(wo.PartId, out translog))
                {
                    // Fallback: Output Part search
                    translogOutputLookup.TryGetValue(wo.PartId, out translog);
                }

                if (translog == null) continue;

                // C. Data Mapping
                var currentStart = timeSlots.GetValueOrDefault(mcWait.Plan_start_time_Id)?.Start_time;

                result.Add(new TempMc_Wait_ListVM
                {
                    // CRITICAL: Map IDs for MVC Controller enrichment
                    Mc_Id = mcWait.Mc_Id,
                    Wo_Id = mcWait.Wo_Id,
                    Opr_No_Id = mcWait.Opr_No_Id,

                    // Local Data mapping
                    WoNumber = wo.WONumber ?? "",
                    WoQnty = wo.CalcWOQty.ToString(),
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,
                    MatlIssued = Convert.ToInt32(translog.Qnty).ToString(),
                    PlanStartStr = currentStart?.ToString("hh:mm tt") ?? "",
                    MatlReceptTime = translog.Dt_time.ToString("hh:mm tt"),
                    Rework_Wo = 'N',
                    TenantId = tenantId,

                    // Fields to be populated by MVC
                    ShopName = "",
                    McName = "",
                    PartNo = "",
                    RoutingName = "",
                    OprNoName = ""
                });
            }

            return result;
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllBookOutList(long tenantId)
        {
            var result = new List<TempMc_Wait_ListVM>();

            // 1. Load local master data in parallel
            var waitListTask = GetAllMc_Wait_List(tenantId);
            var timeslotsTask = GetAllTimeslot_List(tenantId);
            var allWOTask = AllProductionWo(tenantId);
            var translogsTask = GetAllInvTransLog(tenantId);

            await Task.WhenAll(waitListTask, timeslotsTask, allWOTask, translogsTask);

            // 2. Filter: Setup_Apprvl_time != null
            var rawWaitList = waitListTask.Result;
            var waitList = rawWaitList
                .Where(x => x.Setup_Apprvl_time != null)
                .ToList();

            if (!waitList.Any()) return result;

            // 3. Convert Lists to Dictionaries/Lookups
            var allWO = allWOTask.Result
                .GroupBy(x => x.ProductionPlanId)
                .ToDictionary(g => g.Key, g => g.First());

            var timeSlots = timeslotsTask.Result
                .GroupBy(x => x.Timeslot_ListId)
                .ToDictionary(g => g.Key, g => g.First());

            var translogs = translogsTask.Result;
            var translogLookup = translogs
                .GroupBy(x => x.Input_Part_NoId)
                .ToDictionary(g => g.Key, g => g.First());

            // 4. The Loop
            foreach (var mcWait in waitList)
            {
                // A. WO Lookup
                if (!allWO.TryGetValue(mcWait.Wo_Id, out var wo)) continue;

                // B. Translog Lookup
                Inv_Trans_LogVM translog = null;
                if (!translogLookup.TryGetValue(wo.PartId, out translog))
                {
                    // Fallback: Output Part search
                    translog = translogs.FirstOrDefault(t => t.Output_Part_No == wo.PartId);
                    if (translog == null) continue;
                }

                // C. Data Mapping
                var startTS = timeSlots.GetValueOrDefault(mcWait.Plan_start_time_Id);

                result.Add(new TempMc_Wait_ListVM
                {
                    // --- IDs for MVC Enrichment ---
                    Mc_Id = mcWait.Mc_Id,
                    Wo_Id = mcWait.Wo_Id,
                    Opr_No_Id = mcWait.Opr_No_Id,
                    // Assuming TempMc_Wait_ListVM has PartId. If not, MVC will retrieve it via WO_Id lookup
                    // PartId = wo.PartId, 

                    // --- Local Data ---
                    WoNumber = wo.WONumber ?? "",
                    WoQnty = wo.CalcWOQty.ToString(),
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,

                    // Specific BookOut Fields
                    QntyOffered = mcWait.QntyOffered,
                    Accepted = mcWait.Accepted,
                    NonConQnty = mcWait.NonConQnty,

                    MatlIssued = Convert.ToInt32(translog.Qnty).ToString(),
                    MatlReceptTime = translog.Dt_time.ToString("hh:mm tt"),

                    PlanStartStr = startTS?.Start_time.ToString("hh:mm tt") ?? "",
                    SetUpTimeStr = mcWait.Setup_Apprvl_time.Value.ToLocalTime().ToString("hh:mm tt"),

                    Rework_Wo = 'N',
                    TenantId = tenantId,

                    // --- Empty External Fields ---
                    ShopName = "",
                    McName = "",
                    PartNo = "",
                    RoutingName = "",
                    OprNoName = "",
                    UomName = ""
                });
            }

            return result;
        }
        public async Task<IEnumerable<ProductionPlan_WOVM>> AllProductionWoReadForProd(long tenantId)
        {
            // 1. Fetch Local Data in Parallel
            var woTask = AllProductionWo(tenantId);
            var procTask = AllProcPlan(tenantId);
            var transTask = GetAllInvTransLog(tenantId);
            var ncTask = GetAllInsp_Outcome_Details(tenantId);

            await Task.WhenAll(woTask, procTask, transTask, ncTask);

            var productions = woTask.Result.ToList();
            var procPlans = procTask.Result;
            var transLogs = transTask.Result;
            var ncLogs = ncTask.Result;

            // 2. Create Lookups (Optimization)
            // ProcPlan by WorkOrderId
            var procPlanDict = procPlans
                .GroupBy(p => p.WorkOrderId)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());

            // NC Logs count by PartId
            var ncLogCountDict = ncLogs
                .GroupBy(n => n.Inw_Recpt_Part_No_Id)
                .ToDictionary(g => g.Key, g => g.Count());

            // TransLogs (Input and Output)
            // We group by PartId to quickly check quantities
            var transLogInputDict = transLogs
                .GroupBy(t => t.Input_Part_NoId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var transLogOutputDict = transLogs
                .Where(t => t.Output_Part_No != null)
                .GroupBy(t => t.Output_Part_No)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 3. Process Logic
            foreach (var item in productions)
            {
                // A. PartType & PlanStartDateStr
                if (item.PartType == 2)
                {
                    item.PlanStartDateStr = item.PlanStartDate.ToString("dd-MM-yyyy");
                    item.PartTypeName = "Assembly";
                }
                else if(item.PartType == 1)
                {
                    item.PlanStartDateStr = item.PlanStartDate.ToString("dd-MM-yyyy");
                    item.PartTypeName = "Child Part";
                }
                else
                {
                    if (procPlanDict.TryGetValue(item.WoId, out var pp) && pp != null)
                    {
                        item.PlanStartDateStr = pp.CalcReceiptDate.ToString("dd-MM-yyyy");
                    }
                    item.PartTypeName = "Child Part";
                }

                // B. SO Completion Date
                //if (item.SoComplDate.HasValue)
                //{
                    item.SoComplDateStr = item.SoComplDate.ToString("dd-MM-yyyy");
                //}

                // C. No of Open NC
                item.NoOfOpenNc = ncLogCountDict.GetValueOrDefault(item.PartId, 0);

                // D. Ready For Prod
                // Logic: Check if sufficient quantity exists in TransLogs for the ProcPlan part
                item.ReadyForProd = "N";
                if (procPlanDict.TryGetValue(item.WoId, out var procplanwo) && procplanwo != null)
                {
                    bool hasStock = false;

                    // Check Input Parts
                    if (transLogInputDict.TryGetValue(procplanwo.PartId, out var inputs))
                    {
                        if (inputs.Any(t => t.Qnty >= procplanwo.Calc_Proc_Qnty)) hasStock = true;
                    }

                    // Check Output Parts (Fallback)
                    if (!hasStock && transLogOutputDict.TryGetValue(procplanwo.PartId, out var outputs))
                    {
                        if (outputs.Any(t => t.Qnty >= procplanwo.Calc_Proc_Qnty)) hasStock = true;
                    }

                    if (hasStock) item.ReadyForProd = "Y";
                }
            }

            return productions;
        }
        public async Task<IEnumerable<ProductionPlan_WOVM>> GetAllReadyforProductionWo(long tenantId)
        {
            // 1. Fetch Local Data in Parallel
            var woTask = AllProductionWo(tenantId);
            var procTask = AllProcPlan(tenantId);
            var tempWaitTask = GetAllTempWO_Wait_List(tenantId);
            var activeWaitTask = GetAllWO_Wait_List(tenantId);
            var oprTask = GetAllOpr_List(tenantId);
            var timeslotTask = GetAllTimeslot_List(tenantId);
            var mcTimeslotTask = GetAllMc_Timeslot_List(tenantId);

            await Task.WhenAll(woTask, procTask, tempWaitTask, activeWaitTask, oprTask, timeslotTask, mcTimeslotTask);

            var productions = woTask.Result.ToList();
            var procPlans = procTask.Result;
            var tempWaitList = tempWaitTask.Result;
            var activeWaitList = activeWaitTask.Result;
            var oprLists = oprTask.Result;
            var allTimeslots = timeslotTask.Result;
            var allMcTimeslots = mcTimeslotTask.Result;

            // 2. Create Efficient Lookups
            var procPlanDict = procPlans.GroupBy(p => p.WorkOrderId).ToDictionary(g => g.Key, g => g.First());
            var tempWaitDict = tempWaitList.GroupBy(w => w.Wo_Id).ToDictionary(g => g.Key, g => g.First());
            var activeWaitDict = activeWaitList.GroupBy(w => w.Wo_Id).ToDictionary(g => g.Key, g => g.First());
            var oprDict = oprLists.GroupBy(o => o.Wo_Id).ToDictionary(g => g.Key, g => g.ToList());
            var mcTimeDict = allMcTimeslots.ToDictionary(m => m.Mc_Timeslot_List_Id);
            var timeDict = allTimeslots.ToDictionary(t => t.Timeslot_ListId);

            // HashSet for fast Parent check
            var parentWoIds = new HashSet<long>(productions.Select(x => x.ParentWoId));

            var result = new List<ProductionPlan_WOVM>();

            // 3. Process Logic
            foreach (var item in productions)
            {
                // FILTER: Item MUST exist in TempWO_Wait_List to be included
                if (!tempWaitDict.TryGetValue(item.WoId, out var wO_Wait_List))
                    continue;

                // A. Plan Start Date Logic
                if (item.PartType == 2)
                {
                    item.PlanStartDateStr = item.PlanStartDate.ToString("dd-MM-yyyy");
                    item.PartTypeName = "Assembly"; // Default for Type 2
                }
                else
                {
                    if (procPlanDict.TryGetValue(item.WoId, out var pp))
                    {
                        item.PlanStartDateStr = pp.CalcReceiptDate.ToString("dd-MM-yyyy");
                    }

                    // Part Type Name Logic (Moved here as it depends on local WO structure)
                    bool isParentWo = parentWoIds.Contains(item.WoId);
                    if (item.PartType == 1 && item.ParentWoId == 0)
                    {
                        item.PartTypeName = isParentWo ? "Parent CMP" : "CMP";
                    }
                }

                // B. SO Completion Date (Base Format)
                //if (item.SoComplDate.HasValue)
                //{
                    item.SoComplDateStr = item.SoComplDate.ToString("dd-MM-yyyy");
                //}

                // C. Actual Start Date Calculation (Earliest Timeslot)
                DateTime? earliestStartTime = null;
                if (oprDict.TryGetValue(item.ProductionPlanId, out var operations))
                {
                    foreach (var opr in operations)
                    {
                        if (mcTimeDict.TryGetValue(opr.Shop_Plan_start_time, out var mcTimeslot))
                        {
                            if (timeDict.TryGetValue(mcTimeslot.Timeslot_List_Id, out var timeslot))
                            {
                                if (earliestStartTime == null || timeslot.Start_time < earliestStartTime)
                                {
                                    earliestStartTime = timeslot.Start_time;
                                }
                            }
                        }
                    }
                }

                if (earliestStartTime != null)
                {
                    item.ActStartDateStr = earliestStartTime.Value.ToString("dd-MM-yyyy");
                }

                // D. Data Change Flag
                item.DataChange = (item.Changed == 0) ? "N" : "Y";

                // E. Wait List Dates & Criticality
                item.CsStartDate = wO_Wait_List.Plan_Start_Date.ToString("dd-MM-yyyy");
                item.CsEndDate = wO_Wait_List.Plan_End_Date.ToString("dd-MM-yyyy");

                if (activeWaitDict.TryGetValue(item.ProductionPlanId, out var activeWait))
                {
                    item.PsStartDate = activeWait.Plan_Start_Date.ToString("dd-MM-yyyy");
                    item.PsEndDate = activeWait.Plan_End_Date.ToString("dd-MM-yyyy");
                }

                // Critical Parts Calculation
                item.CriticalParts = (wO_Wait_List.Plan_End_Date > item.PlanCompletionDate) ? "Y" : "N";

                result.Add(item);
            }

            return result;
        }
        public async Task<IEnumerable<Matl_Issue_ListVM>> GetAllMatlIssueListForShop(long tenantId)
        {
            // 1. Fetch Local Data
            var issueListTask = GetAllMatl_Issue_List(tenantId);
            var tempOprTask = GetAllTempOpr_List(tenantId);
            var woTask = AllProductionWo(tenantId);
            var transLogTask = GetAllInvTransLog(tenantId);

            await Task.WhenAll(issueListTask, tempOprTask, woTask, transLogTask);

            var resultList = issueListTask.Result;
            var tempOprList = tempOprTask.Result;
            var woList = woTask.Result;
            var transLogs = transLogTask.Result;

            // 2. Lookups
            var tempOprDict = tempOprList.ToDictionary(t => t.TempOpr_ListId);
            var woDict = woList.ToDictionary(w => w.ProductionPlanId);

            // --- CRITICAL FIX HERE ---
            // Only identify operations where Movement is NOT Complete ('N')
            var pendingOprIds = new HashSet<long>(
                transLogs
                .Where(t => t.Movement_Compl == 'N')
                .Select(t => t.Input_Opr_No)
            );
            // -------------------------

            var finalResult = new List<Matl_Issue_ListVM>();

            // 3. Filter & Map Local Fields
            foreach (var item in resultList)
            {
                if (!tempOprDict.TryGetValue(item.Part_Ref, out var tempopr)) continue;

                // FILTER: Skip if an incomplete transaction exists for this Operation
                if (pendingOprIds.Contains(tempopr.Opr_No)) continue;

                // FILTER: Skip if WO is missing or Status is 8 (Deleted/Closed)
                if (!woDict.TryGetValue(tempopr.Wo_Id, out var pp)) continue;
                if (pp.Status == 8) continue;

                // Map WO Data
                item.WoNumber = pp.WONumber ?? "";
                item.BalWoQnty = pp.CalcWOQty.ToString();
                item.QntyAvl = 0;
                item.BookOutQnty = 0;
                item.QntyRecdCnf = "Not Confirmed";
                item.IssueMovDtStr = item.Issue_Mov_date.ToString("dd-MM-yyyy");

                // Pass necessary IDs for Controller to use for enrichment
                item.PartId = (long)pp.PartId;
                item.RoutingId = (long)pp.RoutingId;

                finalResult.Add(item);
            }

            return finalResult;
        }
    }
}
