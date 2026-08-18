using CWB.App.Models.BusinessProcesses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.BusinessProcesses
{
   
    public interface IBAService
    {

        Task<WorkOrdersVM> PostWO(WorkOrdersVM workOrdersVM);
        Task<List<WorkOrdersVM>> MultiplePostWO(IEnumerable<WorkOrdersVM> workOrdersVM);
        Task<List<WorkOrdersVM>> BOMTempPOst(IEnumerable<BOMTempVM> bOMTempVMs);
        Task<List<WOSOVM>> PostWoSoRel(IEnumerable<WOSOVM> wOSOVMs);
        Task<IEnumerable<WorkOrdersVM>> AllWorkOrders();
        Task<bool> RemoveSalesOrder(long salesOrderId);
        Task<bool> RemoveCustomerOder(long customerOrderId);
        Task<bool> RemoveDeliverySchedule(long scheduleId);
        //Task<IEnumerable<POLogVM>> RemoveSalesOrder(long tenantId, long salesOrderId);

        Task<IEnumerable<POLogVM>> GetPOLogs(long customerOrderId);
        Task<IEnumerable<POLogVM>> GetProcLogs(long poid);
        Task<IEnumerable<POLogVM>> GetWoPOLogs(long customerOrderId);
        Task<IEnumerable<BAStatusVM>> GetBAAllStatus();
        Task<IEnumerable<SalesOrderVM>> GetSalesOrders(long customerOrderId);
        Task<SalesOrderVM> GetOneSO(long salesOrderId);
        Task<IEnumerable<SalesOrderVM>> AllSalesOrders();
        Task<IEnumerable<CustomerOrderRowVM>> GetCustomerOrders();
        Task<IEnumerable<DeliveryScheduleVM>> GetSchedules(long customerOrderId);
        Task<IEnumerable<SO_Alloc_ListVM>> GetSOAllocationlistbyPartid(long partId);
        Task<IEnumerable<SO_Alloc_ListVM>> AllSoallocation();
        Task<bool> AddSalesOrders(long customerOrderId);
        Task<SO_Alloc_ListVM> PostSOAllocation(SO_Alloc_ListVM soallocvm);
        Task<SalesOrderVM> PostSalesOrder(SalesOrderVM salesOrderVM);
        Task<POLogVM> PostSOLog(POLogVM poLogVM);
        Task<POLogVM> PostPOLog(POLogVM poLogVM);
        
        Task<CustomerOrderVM> PostCustomerOrder(CustomerOrderVM customerOrderVM);
        Task<DeliveryScheduleVM> PostDeliverySchedule(DeliveryScheduleVM deliveryScheduleVM);
        Task<SOAggregateVM> PostSOAggregate(SOAggregateVM sOAggregateVM);
        Task<SOAggregateVM> GetSOAggregate(long customerOrderId);

        //Task<DeliveryScheduleVM> PostOrderStatus();
        Task<BAStatusVM> GetBAStatus(long Id);
        Task<bool> CheckPartNo(long partId);
        Task<string> HelloWorld();
    }
}
