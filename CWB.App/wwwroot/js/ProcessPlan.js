let PendingSos = {};
let HoldSos = {};
let TotalSos = {};

function landingPage() {
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen
    const today = new Date();
    Promise.all([
        api.getbulk("/WorkOrder/AllSalesOrders"),
        api.getbulk("/WorkOrder/AllWorkOrders")
    ]).then(([salesOrders, workOrders]) => {
        const today = new Date();

        // Total SO count
        const totalSOCount = salesOrders.length;
        TotalSos = salesOrders;
        // SOs on hold
        const soOnHold = salesOrders.filter(so => so.status === 6);
        const soOnHoldCount = soOnHold.length;
        HoldSos = soOnHold;
        const opsos = totalSOCount - soOnHoldCount;
        $("#openSOs").text(opsos);

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
        PendingSos = soNotInWoPending;

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

        // --- Sequential API Calls within landingPage ---

        // Second API call in landingPage
        api.getbulk("/WorkOrder/AllWorkOrders").then((data) => {
            const workOrdersWithStatus1 = data.filter((workOrder) => workOrder.status >= 1);
            const workOrdersWithStatusHold = data.filter((workOrder) => workOrder.status === 8);
            const count = workOrdersWithStatus1.length;
            const woonhold = workOrdersWithStatusHold.length;

            //const woInWipPastDueCount = data.filter((workOrder) => Date.parse(workOrder.planCompletionDate) < today.getTime()).length;
            //const woInWipOnTrackCount = data.filter((workOrder) => Date.parse(workOrder.planCompletionDate) >= today.getTime()).length;
            const opwos = count - woonhold;
            $("#openWOs").text(opwos);
            $('#totalWO').text(count);
            $('#WoOnHold').text(woonhold);
            $('#WoInPastDue').text(0);
            $('#WoOnTrack').text(0);
            $('#NoOfReorderItem').text(0);
        }).catch((error) => {
            console.error("Error loading WOs in second call:", error);
        });

        // Third API call in landingPage
        api.getbulk("/workOrder/AllProductionWo").then((data) => {
            const workOrdersWithStatus1 = data.filter(item => item.readyForProd === "Y" && item.woRelease != "Y");  //
            const woproduction = data.filter(item => item.readyForProd === "Y" && item.woRelease === "Y");  //
            $('#woRelWf').text(workOrdersWithStatus1.length);
            $("#openWOPs").text(woproduction.length);
        }).catch((error) => {
            console.error("Error loading Production WOs:", error);
        });

        // Fourth API call in landingPage
        api.getbulk("/WorkOrder/GetAllPodetails").then((data) => {
            data = data.filter(item => item.status === 1);
            $("#openPOs").text(data.length);
        }).catch((error) => {
            console.error("Error loading PO details:", error);
        });
        api.getbulk("/WorkOrder/AllSODispatch").then((data) => {
            data = data.filter(item => item.status === 1 || item.status === 2);
            data = data.filter(item => item.qntyOnHand > 0);
            $("#openSODs").text(data.length);
            if (data.length === 0) {
                const anchorTag = document.getElementById('openSODsBtn');
                anchorTag.setAttribute('href', '#');
            }
        }).catch((error) => {
            console.error("Error loading PO details:", error);
        });

        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen after all primary data is fetched

    }).catch((error) => {
        console.error("Error loading SO or WO:", error);
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
    });
}

// --- SoWoPending function ---
function SoWoPending() {
    var tablebody = $("#SoPendingGrid tbody");
    $(tablebody).html(""); // clear tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen

    Promise.all([
        api.getbulk("/WorkOrder/AllSalesOrders"),
        api.getbulk("/WorkOrder/AllWorkOrders")
    ])
        .then(([salesOrders, workOrders]) => {
            // filter active SOs (not on hold)
            const activeSOs = salesOrders.filter(so => so.status !== 6);

            // group WOs by salesOrderId
            const woBySalesOrder = {};
            workOrders.forEach(wo => {
                if (!woBySalesOrder[wo.salesOrderId]) {
                    woBySalesOrder[wo.salesOrderId] = [];
                }
                woBySalesOrder[wo.salesOrderId].push(wo);
            });

            // pending SOs = active SOs with no WO in status 1
            const pendingSOs = activeSOs.filter(so => {
                const relatedWOs = woBySalesOrder[so.salesOrderId] || [];
                return !relatedWOs.some(wo => wo.status === 1);
            });

            PendingSos = pendingSOs; // keep global if needed

            if (pendingSOs.length > 0) {
                for (let i = 0; i < pendingSOs.length; i++) {
                    $(tablebody).append(
                        AppUtil.ProcessTemplateDataNew("SoWoRow", pendingSOs[i], i)
                    );
                }
            } else {
                $(tablebody).append("<tr><td colspan='8'>No Pending SOs</td></tr>");
            }
            $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
        })
        .catch((error) => {
            console.error("Error loading pending SOs:", error);
            $(tablebody).append("<tr><td colspan='8'>Error loading data</td></tr>");
            $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
        });
}

// --- SoHold function ---
function SoHold() {
    var tablebody = $("#SOHoldGrid tbody");
    $(tablebody).html("");//empty tbody
    // Data is loaded from global HoldSos, no API call needed here, so no loader logic.
    for (i = 0; i < HoldSos.length; i++) {
        $(tablebody).append(AppUtil.ProcessTemplateDataNew("SoHoldRow", HoldSos[i], i));
    }
}

// --- SoTotal function ---
function SoTotal() {
    var tablebody = $("#SoTotalGrid tbody");
    $(tablebody).html("");//empty tbody
    // Data is loaded from global TotalSos, no API call needed here, so no loader logic.
    for (i = 0; i < TotalSos.length; i++) {
        $(tablebody).append(AppUtil.ProcessTemplateDataNew("SoHoldRow", TotalSos[i], i));
    }
}

// --- WoHold function ---
function WoHold() {
    var tablebody = $("#WoHoldGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen

    api.getbulk("/WorkOrder/AllWorkOrders").then((data) => {
        const workOrdersWithStatusHold = data.filter((workOrder) => workOrder.status === 8);
        if (workOrdersWithStatusHold.length > 0) {
            for (i = 0; i < workOrdersWithStatusHold.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoHoldRow", workOrdersWithStatusHold[i], i));
            }
        } else {
            $(tablebody).append("<tr><td colspan='8'>No WO Hold</td></tr>");
        }
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
    }).catch((error) => {
        console.error("Error loading WO Hold:", error);
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
    });
}

// --- WoTotal function ---
function WoTotal() {
    var tablebody = $("#WoTotalGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen
    api.getbulk("/WorkOrder/AllWorkOrders").then((data) => {
        const workOrdersWithStatus1 = data.filter((workOrder) => workOrder.status >= 1);
        for (i = 0; i < workOrdersWithStatus1.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoHoldRow", workOrdersWithStatus1[i], i));
        }
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
    }).catch((error) => {
        console.error("Error loading WO Total:", error);
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
    });
}

// --- CustomerMisLoad function ---
function CustomerMisLoad() {
    var tablebody = $("#CustomerMis tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen
    api.getbulk("/WorkOrder/CustomerMis").then((data) => {
        let tbody = $("#CustomerMis tbody");
        tbody.empty();

        let totalOpenCount = 0, totalOpenValue = 0;
        let totalWipCount = 0, totalWipValue = 0;

        data.forEach(item => {
            // update totals
            totalOpenCount += item.soOpenCount;
            totalOpenValue += item.soOpenValue;
            totalWipCount += item.soWipCount;
            totalWipValue += item.soWipValue;

            // build row
            let row = `
                        <tr>
                            <td><a href="#" data-bs-toggle="modal" data-bs-target="#CustomerSoPop" data-customer="${item.customerName}">${item.customerName}</a></td>
                            <td>${item.soOpenCount} / ${item.soOpenValue} INR</td>
                            <td>${item.soWipCount} / ${item.soWipValue} INR</td>
                        </tr>
                    `;
            tbody.append(row);
        });

        // bind totals in 2nd table
        $("#CustomerMis").next("table").find("tbody").html(`
                    <tr>
                        <td width="32%" style="font-weight: bolder;">Total</td>
                        <td>${totalOpenCount} / ${totalOpenValue} INR</td>
                        <td>${totalWipCount} / ${totalWipValue} INR</td>
                    </tr>
                `);
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
    }).catch((error) => {
        console.error("Error loading sales orders", error);
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
    });
}

// --- CustomerSoLoad function ---
function CustomerSoLoad(customer) {
    var tablebody = $("#CusSoGrid tbody");
    $(tablebody).html("");//empty tbody
    // Data is loaded from global TotalSos, no API call needed here, so no loader logic.
    var customerSos = TotalSos.filter(so => so.status !== 6 && so.customer === customer);
    for (i = 0; i < customerSos.length; i++) {
        $(tablebody).append(AppUtil.ProcessTemplateDataNew("CusSoGridRow", customerSos[i], i));
    }
}

// --- woWait function ---
function woWait() {
    var tablebody = $("#ReleaseWaitGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen

    Promise.all([
        api.getbulk("/workOrder/AllProductionWo"),
        api.getbulk("/WorkOrder/AllWorkOrders")
    ])
        .then(([productionData, workOrdersData]) => {
            // 1️⃣ Filter production WO
            const filteredProd = productionData.filter(item => item.readyForProd === "Y" && item.woRelease != "Y");

            // 2️⃣ Create a Set of WO IDs from filtered production
            const prodWoIds = new Set(filteredProd.map(item => item.woId));

            // 3️⃣ Filter work orders that are status >=1 AND exist in production WO IDs
            const today = new Date();

            const filteredWorkOrders = workOrdersData.filter(wo => {
                // Check status and woId
                return wo.status >= 1 && prodWoIds.has(wo.woid);
            });
            // 4️⃣ Append filtered production WO
            //filteredProd.forEach((item, i) => {
            //    $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoHoldRow", item, i));
            //});

            // 5️⃣ Append filtered work orders
            $("#woRelWf").text(filteredWorkOrders.length);
            filteredWorkOrders.forEach((item, i) => {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoHoldRow", item, i));
            });
            $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
        })
        .catch((error) => {
            console.error("Error fetching work orders", error);
            $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
        });
}

// --- WoStartLoad function ---
function WoStartLoad() {
    var tablebody = $("#WoStartGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen
    Promise.all([
        api.getbulk("/workOrder/AllProductionWo"),
        api.getbulk("/WorkOrder/AllWorkOrders")
    ])
        .then(([productionData, workOrdersData]) => {
            // 1️⃣ Filter production WO
            const filteredProd = productionData.filter(item => item.readyForProd === "Y" && item.woRelease != "Y");

            // 2️⃣ Create a Set of WO IDs from filtered production
            const prodWoIds = new Set(filteredProd.map(item => item.woId));

            // 3️⃣ Filter work orders that are status >=1 AND exist in production WO IDs
            const today = new Date();

            const filteredWorkOrders = workOrdersData.filter(wo => {
                // Check status and woId
                if (wo.status < 1 || !prodWoIds.has(wo.woid)) return false;

                // Parse planCompletionDateStr (assume format "dd-MM-yyyy")
                const parts = wo.planCompletionDateStr.split("-");
                const planDate = new Date(parts[2], parts[1] - 1, parts[0]); // year, monthIndex, day

                // Include only if planDate < today
                return planDate > today;
            });
            // 4️⃣ Append filtered production WO
            //filteredProd.forEach((item, i) => {
            //    $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoHoldRow", item, i));
            //});

            // 5️⃣ Append filtered work orders
            $("#WoSt").text(filteredWorkOrders.length);
            filteredWorkOrders.forEach((item, i) => {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoHoldRow", item, i));
            });
            $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
        })
        .catch((error) => {
            console.error("Error fetching work orders", error);
            $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
        });
}

// --- WOAwait function ---
function WOAwait() {
    var tablebody = $("#WoMatlStartGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen
    api.getbulk("/WorkOrder/GetAllReadyforProductionWo").then((data) => {
        const today = new Date();
        const filteredWorkOrders = data.filter(wo => {
            // Check status and woId
            // Parse planCompletionDateStr (assume format "dd-MM-yyyy")
            const parts = wo.csStartDate.split("-");
            const planDate = new Date(parts[2], parts[1] - 1, parts[0]); // year, monthIndex, day

            // Include only if planDate < today
            return planDate > today;
        });
        $("#WoAwait").text(filteredWorkOrders.length);
        for (i = 0; i < filteredWorkOrders.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoSimRow", filteredWorkOrders[i], i));
        }
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
    }).catch((error) => {
        console.error("Error loading WO Await:", error);
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
    });
}

// --- WOMatlload function ---
function WOMatlload() {
    var tablebody = $("#WoPastDateGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show(); // ⬅️ SHOW loading screen
    api.getbulk("/WorkOrder/GetAllReadyforProductionWo").then((data) => {
        const today = new Date();
        const filteredWorkOrders = data.filter(wo => {
            // Check status and woId
            // Parse planCompletionDateStr (assume format "dd-MM-yyyy")
            const parts = wo.csStartDate.split("-");
            const planDate = new Date(parts[2], parts[1] - 1, parts[0]); // year, monthIndex, day

            // Include only if planDate < today
            return planDate < today;
        });
        $("#WoPast").text(filteredWorkOrders.length);
        for (i = 0; i < filteredWorkOrders.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("WoSimRow", filteredWorkOrders[i], i));
        }
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on success
    }).catch((error) => {
        console.error("Error loading WO Matl Load:", error);
        $("#preloaderblurred").hide(); // ⬅️ HIDE loading screen on error
    });
}

$(document).ready(function () {
    landingPage();
    WoStartLoad();
    WOAwait();
    WOMatlload();
    CustomerMisLoad();
    $('#SoWoPendingPop').on('shown.bs.modal', function (event) {
        SoWoPending();
    });
    $('#CustomerSoPop').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var customer = relatedTarget.data("customer");
        $("#SelCustomer").text(customer)
        CustomerSoLoad(customer);
    });
    $('#WOrelWait').on('shown.bs.modal', function (event) {
        woWait();
    });
    $('#WoPastDate').on('shown.bs.modal', function (event) {
        WOMatlload();
    });
    $('#WoMatlStart').on('shown.bs.modal', function (event) {
        WOAwait();
    });
    $('#WoStart').on('shown.bs.modal', function (event) {
        WoStartLoad();
    });
    $('#SoHoldPop').on('shown.bs.modal', function (event) {
        SoHold();
    });
    $('#SoTotalPop').on('shown.bs.modal', function (event) {
        SoTotal();
    });
    $('#WoHoldPop').on('shown.bs.modal', function (event) {
        WoHold();
    });
    $('#WOTotalPop').on('shown.bs.modal', function (event) {
        WoTotal();
    });
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