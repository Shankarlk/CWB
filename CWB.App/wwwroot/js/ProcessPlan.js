function landingPage() {
    const today = new Date(); Promise.all([
        api.getbulk("/WorkOrder/AllSalesOrders"),
        api.getbulk("/WorkOrder/AllWorkOrders")
    ]).then(([salesOrders, workOrders]) => {
        const today = new Date();

        // Total SO count
        const totalSOCount = salesOrders.length;

        // SOs on hold
        const soOnHold = salesOrders.filter(so => so.status === 6);
        const soOnHoldCount = soOnHold.length;

        // Filter out SOs that are on hold
        const activeSOs = salesOrders.filter(so => so.status !== 6);

        // WO map by salesOrderId
        const woBySalesOrder = {};
        workOrders.forEach(wo => {
            if (!woBySalesOrder[wo.salesOrderId]) {
                woBySalesOrder[wo.salesOrderId] = [];
            }
            woBySalesOrder[wo.salesOrderId].push(wo);
        });

        // Find active SOs that do NOT have any WO in status 1
        const soNotInWoPending = activeSOs.filter(so => {
            const relatedWOs = woBySalesOrder[so.salesOrderId] || [];
            return !relatedWOs.some(wo => wo.status === 1); // no pending WO
        });

        // Find active SOs that DO have at least one WO in status 1
        const soWithPendingWO = activeSOs.filter(so => {
            const relatedWOs = woBySalesOrder[so.salesOrderId] || [];
            return relatedWOs.some(wo => wo.status === 1);
        });

        // UI updates
        $('#totalSO').text(totalSOCount);
        $('#noOfSoHold').text(soOnHoldCount);
        $('#SoWoPending').text(soNotInWoPending.length);  // Only active SOs with WO pending
        $('#1stSoWoPending').text(soNotInWoPending.length);
        $('#SoNotInWoPending').text(soNotInWoPending.length); // Active SOs with no pending WO
        $('#SoInPastDue').text(0);
        $('#SoOntrack').text(0);
    }).catch((error) => {
        console.error("Error loading SO or WO:", error);
    });


    api.getbulk("/WorkOrder/AllWorkOrders").then((data) => {
        const workOrdersWithStatus1 = data.filter((workOrder) => workOrder.status >= 1);
        const workOrdersWithStatusHold = data.filter((workOrder) => workOrder.status === 8);
        const count = workOrdersWithStatus1.length;
        const woonhold = workOrdersWithStatusHold.length;

        //const woInWipPastDueCount = data.filter((workOrder) => Date.parse(workOrder.planCompletionDate) < today.getTime()).length;
        //const woInWipOnTrackCount = data.filter((workOrder) => Date.parse(workOrder.planCompletionDate) >= today.getTime()).length;

        $('#totalWO').text(count);
        $('#WoOnHold').text(woonhold);
        $('#WoInPastDue').text(0);
        $('#WoOnTrack').text(0);
        $('#NoOfReorderItem').text(0);
    }).catch((error) => {
    });
}






$(document).ready(function () {

    landingPage();

    //$("#simulation").on("click", function () {
    //    $.ajax({
    //        type: "POST",
    //        url: '/WorkOrder/PostReadForProdWoWaitList',
    //        contentType: "application/json; charset=utf-8",
    //        headers: { 'Content-Type': 'application/json' },
    //        success: function (result) {
    //            console.log("Success:", result);
    //        }
    //    });
    //});
});
