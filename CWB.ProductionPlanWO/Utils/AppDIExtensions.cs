using CWB.ProductionPlanWO.Infrastructure;
using CWB.ProductionPlanWO.Repositories;
using CWB.ProductionPlanWO.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Utils
{
    public static class AppDIExtensions
    {
        public static void ConfigureAppDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IWorkOrderRepository, WorkOrdersRepository>();
            services.AddTransient<IProcPlanRepository, ProcPlanRepository>();
            services.AddTransient<IWOSORepository, WOSORepository>();
            services.AddTransient<IWOStatusRepository, WOStatusRepository>();
            services.AddTransient<IBOMListRepository, BOMListRepository>();
            services.AddTransient<IBOMTempRepository, BOMTempRepository>();
            services.AddTransient<IProductionPlan_WORepository, ProductionPlan_WORepository>();
            services.AddTransient<IChildWoRelRepository, ChildWoRelRepository>();
            services.AddTransient<IMcTimeListRepository, McTimeListRepository>();
            services.AddTransient<IPODetailsRepository, PODetailsRepository>();
            services.AddTransient<IPOHeaderRepository, POHeaderRepository>();
            services.AddTransient<IPOStatusRepository, POStatusRepository>();
            services.AddTransient<IPOLogRepository, POLogRepository>();
            services.AddTransient<IWoSubConSupplierRepository, WoSubConRepository>();
            services.AddTransient<IProcPlanPartPurChaseRelRepository, ProcPlanPartPurChaseRelRepository>();
            services.AddTransient<IInward_Condn_listRepository, Inward_Condn_listRepository>();
            services.AddTransient<IInsp_Outcome_DetailsRepository, Insp_Outcome_DetailsRepository>();
            services.AddTransient<IInsp_Outcome_ListRepository, Insp_Outcome_ListRepository>();
            services.AddTransient<IInv_Trans_LogRepository, Inv_Trans_LogRepository>();
            services.AddTransient<IInw_Recpt_HeaderRepository, Inw_Recpt_HeaderRepository>();
            services.AddTransient<IInventory_MasterRepository, Inventory_MasterRepository>();
            services.AddTransient<IInw_Recpt_Part_NoRepository, Inw_Recpt_Part_NoRepository>();
            services.AddTransient<IInw_Recpt_DetailsRepository, Inw_Recpt_DetailsRepository>();
            services.AddTransient<IInwardDocTypeRepository, InwardDocTypeRepository>();
            services.AddTransient<ILineInspectDocTypeRepository, LineInspectDocTypeRepository>();
            services.AddTransient<IFinalInspectDocTypeRepository, FinalInspectDocTypeRepository>();
            services.AddTransient<IInspectDocTypeRepository, InspectDocTypeRepository>();
            services.AddTransient<IRcCaDocTypeRepository, RcCaDocTypeRepository>();
            services.AddTransient<INcLogStatusRepository, NcLogStatusRepository>();
            services.AddTransient<IOperationSettingsRepository, OperationSettingsRepository>();
            services.AddTransient<INC_Decision_LogRepository, NC_Decision_LogRepository>();
            services.AddTransient<INC_Disp_Decision_ListRepository, NC_Disp_Decision_ListRepository>();
            services.AddTransient<INC_work_StatusRepository, NC_work_StatusReposiotry>();
            services.AddTransient<INC_Disp_Decs_Appl_ListRepository, NC_Disp_Decs_Appl_ListRepository>();
            services.AddTransient<INC_Wk_List_Tmpl_HeadRepository, NC_Wk_List_Tmpl_HeadRepository>();
            services.AddTransient<INC_Wk_List_Tmpl_DetRepository, NC_Wk_List_Tmpl_DetRepository>();
            services.AddTransient<INC_Work_ListRepository, NC_Work_ListRepository>();
            services.AddTransient<INC_Wk_List_HeaderRepository, NC_Wk_List_HeaderRepository>();
            services.AddTransient<ICont_RCA_CA_Status_ListRepository, Cont_RCA_CA_Status_ListReposiotry>();
            services.AddTransient<ICont_RCA_CA_LogRepository, Cont_RCA_CA_LogRepository>();
            services.AddTransient<ICust_NC_Decs_MatrixRepository, Cust_NC_Decs_MatrixRepository>();
            services.AddTransient<ICust_NC_Decs_Matrix_OptRepositoy, Cust_NC_Decs_Matrix_OptRepository>();
            services.AddTransient<IWOService, WOService>();
        }
    }
}
