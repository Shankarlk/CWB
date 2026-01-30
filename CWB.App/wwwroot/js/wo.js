var noofWOCreation = [];
var subcontotal = 0;
var ba_masterparts = {};
const WoOrdStatus = {
    1: "w/f Detailed Prodn Planning",
    2: "w/f WO Release",
    3: "w/f Input Matl",
    4: "w/f Prodn",
    5: "WIP",
    6: "Complete",
    7: "Short Closed",
    8: "Hold",
    9: "Deleted"
};

function loadSO() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/AllSalesOrders").then((data) => {
        data = data.filter(item => item.status !== 6 && item.hold != true);
        var tablebody = $("#SalesOrders1 tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            if (data[i].poNumber == null) {
                continue;
            }
            $(tablebody).append(AppUtil.ProcessTemplateData("SalesOrderRow1", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
//function GetAllSubCons() {
//    api.getbulk("/WorkOrder/GetAllSubCons").then((data) => {
//        var tablebody = $("#SalesOrders1 tbody");
//        $(tablebody).html("");//empty tbody
//        //console.log(data);
//        for (i = 0; i < data.length; i++) {
//            $(tablebody).append(AppUtil.ProcessTemplateData("SalesOrderRow1", data[i]));
//        }
//    }).catch((error) => {
//    });
//}

function loadWO() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/AllWorkOrders").then((data) => {
        //data = data.filter(item => item.active !== 2);
        var tablebody = $("#WorkOrder tbody");
        $(tablebody).html("");//empty tbody
                                           
        data = data.filter(item => item.status !== 8);
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            //data[i].strStatus = WoOrdStatus[data[i].status];
            $(tablebody).append(AppUtil.ProcessTemplateData("WorkOrderRow", data[i]));
            $("#initiateDetaileBtn").prop('disabled', false);
            $("#ConcludeProdn").prop('disabled', false);
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}

function reloadWO(reloadOption, partid) {
    api.getbulk("/WorkOrder/ReloadWo?reloadoption=" + reloadOption + "&partid=" + partid).then((data) => {
        var tablebody = $("#MulitpleWOs tbody");
        $(tablebody).html("");//empty tbody
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            //data[i].strStatus = WoOrdStatus[data[i].status];
            $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", data[i]));
        }
    }).catch((error) => {
    });
}
function loadmissingdetails(data) {
    $("#missing-details").modal("show");

    var tablebody = $("#tblmissingdetails tbody");
    tablebody.html(""); // empty tbody

    // 🔥 Normalize: if single object, convert to array
    if (!Array.isArray(data)) {
        data = [data];
    }

    for (var i = 0; i < data.length; i++) {
        tablebody.append(
            AppUtil.ProcessTemplateData("tblmissingdetailsrow", data[i])
        );
    }
}


$(document).ready(function () {

    $('#chkP20RoutPrice').change(function () {
        if ($(this).is(':checked')) {
            var qnt = $("#P20UnitRout").val();
            $("#P20Convprice").val(qnt);
        }
    });
    $('#select-all-checkbox').change(function () {
        if ($(this).is(":checked")) {
            $('#SalesOrders1 tbody').find('input[type="checkbox"]').prop('checked', true);
            $('#btnGW').prop('disabled', true);
            $('#btnAG').prop('disabled', false);
        } else {
            $('#SalesOrders1 tbody').find('input[type="checkbox"]').prop('checked', false);
            $('#btnAG').prop('disabled', true);
        }
    });

    function handleCheckboxChange() {
        var checkboxes = $("#SalesOrders1 tbody input[type='checkbox']:checked"); // Select checked checkboxes
        var partIdSet = new Set(); // To store unique part IDs of the selected rows

        // Iterate over each checked checkbox to gather unique part IDs
        checkboxes.each(function () {
            var row = $(this).closest('tr'); // Get the closest parent row of the checkbox
            var partId = $(row).find("td:eq(3)").text().trim(); // Extract and trim the partId from the row
            partIdSet.add(partId); // Add the partId to the set
        });

        // Determine the button state based on the number of selected checkboxes and unique part IDs
        if (checkboxes.length === 1 || partIdSet.size === 1) {
            $('#btnGW').prop('disabled', false); // Enable the button
            $('#btnAG').prop('disabled', false);
        }
        else if (checkboxes.length > 1) {
            $('#btnGW').prop('disabled', true); // Disable the btnGW button
            $('#btnAG').prop('disabled', false); // Enable the btnAG button
        }
        else {
            $('#btnGW').prop('disabled', true); // Disable the button
            $('#btnAG').prop('disabled', true);
        }
    }

    // Attach the handleCheckboxChange function to the change event of the checkboxes using event delegation
    $('#SalesOrders1 tbody').on('change', 'input[type="checkbox"]', handleCheckboxChange);

    // Optionally, trigger the event handler once to set the initial state of the button
    handleCheckboxChange();


    loadSO();
    loadWO();
    //$("#initiateDetailedbtn").on("click", function () {

    //});
    $("#btnAG").secureClick( function () {
        var checkboxes = $("#SalesOrders1 tbody input[type='checkbox']:checked"); // Select only checked checkboxes
        var selectedRowsData = {};
        var partIdMap = {};
        var SalesorderId = [];
        var WoSoRel = [];
        var WoSOMethod = {};

        checkboxes.each(function (index, checkbox) {
            var row = checkbox.parentNode.parentNode;
            var rowData = {
                salesOrderId: parseInt($(row).find("td:eq(1)").text()),
                wonumber: "",
                partId: parseInt($(row).find("td:eq(3)").text()),
                saleOrderNo: $(row).find("td:eq(4)").text(),
                partNo: $(row).find("td:eq(7)").text(),
                partType: 0,
                partlevel: ' ',
                calcWOQty: parseInt($(row).find("td:eq(8)").text()),
                planCompletionDate: $(row).find("td:eq(12)").text(),
                soComplDate: $(row).find("td:eq(12)").text()
            };
            var balanceSoQty = parseInt($(row).find("td:eq(9)").text())
            if (balanceSoQty > 0) {
                rowData.calcWOQty = balanceSoQty;
            }
            // Group by PartId
            if (partIdMap[rowData.partId]) {
                partIdMap[rowData.partId].calcWOQty += rowData.calcWOQty;
                SalesorderId.push([rowData.partId, rowData.salesOrderId]);
                let currentDate = new Date(rowData.planCompletionDate);
                let storedDate = new Date(partIdMap[rowData.partId].planCompletionDate);
                if (currentDate < storedDate) {
                    partIdMap[rowData.partId].planCompletionDate = rowData.planCompletionDate;
                    partIdMap[rowData.partId].salesOrderId = rowData.salesOrderId;
                }
            } else {
                partIdMap[rowData.partId] = rowData;
                SalesorderId.push([rowData.partId, rowData.salesOrderId]);
            }
        });

        selectedRowsData = Object.values(partIdMap);

        //console.log(selectedRowsData);

        if (selectedRowsData.length === 1) {
            // Single checkbox selected, post to WOpost
            $("#preloaderblurred").show();
            return api.post("/businessaquisition/WOpost", selectedRowsData[0]).then((data) => {
                // Handle success if needed
                loadmissingdetails(data);
                loadWO();
                SalesorderId.forEach(function (arr, outerIndex) {
                    arr.forEach(function (ele, i) {
                        if (ele === data.partId) {
                            //WoSoRel.push([arr[i+1], data.woid]);
                            WoSoRel.push({
                                workOrderId: data.woid,
                                salesOrderId: arr[i + 1]
                            });
                        }
                    });
                });
                //console.log(WoSoRel);
                WoSOMethod = Object.values(WoSoRel);
                $.ajax({
                    type: "POST",
                    url: '/BusinessAquisition/PostWoSoRel',
                    contentType: "application/json; charset=utf-8",
                    headers: { 'Content-Type': 'application/json' },
                    data: JSON.stringify(WoSOMethod),
                    dataType: "json",
                    success: function (result) {

                        window.locationre = result.url;
                    }
                });
                $("#preloaderblurred").hide();
                //console.log(WoSOMethod);
            }).catch((error) => {
                AppUtil.HandleError("WOForm", error);
                $("#preloaderblurred").hide();
            });

        } else if (selectedRowsData.length > 1) {
            // Multiple checkboxes selected, post to MultiWOpost
            //console.log("-- multiplepostwo");
            $("#preloaderblurred").show();
            return $.ajax({
                type: "POST",
                url: '/BusinessAquisition/MultipleWOPost',
                contentType: "application/json; charset=utf-8",
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(selectedRowsData),
                dataType: "json",
                success: function (result) {
                    //alert(result);
                    loadmissingdetails(result);
                    result.forEach(function (a,i) {
                        SalesorderId.forEach(function (arr, outerIndex) {
                            arr.forEach(function (ele, ind) {
                                if (ele === a.partId) {
                                    WoSoRel.push({
                                        workOrderId: a.woid,
                                        salesOrderId: arr[ind + 1]
                                    });
                                }
                            });
                        });
                    });
                    //console.log(WoSoRel);
                    WoSOMethod = Object.values(WoSoRel);
                    //--
                    $.ajax({
                        type: "POST",
                        url: '/BusinessAquisition/PostWoSoRel',
                        contentType: "application/json; charset=utf-8",
                        headers: { 'Content-Type': 'application/json' },
                        data: JSON.stringify(WoSOMethod),
                        dataType: "json",
                        success: function (result) {
                            window.locationre = result.url;
                        }
                    });
                    $("#preloaderblurred").hide();
                    //--
                    window.locationre = result.url;
                    loadWO();
                }, error: function (result) {
                    $("#preloaderblurred").hide();
                }
            });

            //api.post("/businessaquisition/MultipleWOPost", Js).then((data) => {
            //api.post("/businessaquisition/MultipleWOPost?listworkOrdersVM="+ JSON.stringify(selectedRowsData)).then((data) => {
            // Handle success if needed
            // }).catch((error) => {
            //   AppUtil.HandleError("WOForm", error);
            //});
        } else {
            alert("Please select at least one row by checking the checkbox.");
        }
    });

    $("#btnGW").secureClick( function () {
        var checkboxes = $("#SalesOrders1 tbody input[type='checkbox']:checked"); // Select only checked checkboxes
        var selectedRowsData = {};
        var partIdMap = {};
        var SalesorderId = [];
        var WoSoRel = [];
        var WoSOMethod = {};

        checkboxes.each(function (index, checkbox) {
            var row = checkbox.parentNode.parentNode;
            var rowData = {
                salesOrderId: parseInt($(row).find("td:eq(1)").text()),
                wonumber: "",
                partId: parseInt($(row).find("td:eq(3)").text()),
                saleOrderNo: $(row).find("td:eq(4)").text(),
                partNo: $(row).find("td:eq(7)").text(),
                partType: 0,
                partlevel: ' ',
                calcWOQty: parseInt($(row).find("td:eq(8)").text()),
                planCompletionDate: $(row).find("td:eq(12)").text(),
                soComplDate: $(row).find("td:eq(12)").text()
            };
            var balanceSoQty = parseInt($(row).find("td:eq(9)").text())
            if (balanceSoQty > 0) {
                rowData.calcWOQty = balanceSoQty;
            }
            // Group by PartId
            if (partIdMap[rowData.partId]) {
                partIdMap[rowData.partId].calcWOQty += rowData.calcWOQty;
                SalesorderId.push([rowData.partId, rowData.salesOrderId]);
                let currentDate = new Date(rowData.planCompletionDate);
                let storedDate = new Date(partIdMap[rowData.partId].planCompletionDate);
                if (currentDate < storedDate) {
                    partIdMap[rowData.partId].planCompletionDate = rowData.planCompletionDate;
                    partIdMap[rowData.partId].salesOrderId = rowData.salesOrderId;
                }
            } else {
                partIdMap[rowData.partId] = rowData;
                SalesorderId.push([rowData.partId, rowData.salesOrderId]);
            }
        });

        selectedRowsData = Object.values(partIdMap);

        //console.log(selectedRowsData);

        if (selectedRowsData.length === 1) {
            // Single checkbox selected, post to WOpost
            $("#preloaderblurred").show();
            return api.post("/businessaquisition/WOpost", selectedRowsData[0]).then((data) => {
                // Handle success if needed
                loadmissingdetails(data);
                loadWO();
                SalesorderId.forEach(function (arr, outerIndex) {
                    arr.forEach(function (ele, i) {
                        if (ele === data.partId) {
                            //WoSoRel.push([arr[i+1], data.woid]);
                            WoSoRel.push({
                                workOrderId: data.woid,
                                salesOrderId: arr[i + 1]
                            });
                        }
                    });
                });
                //console.log(WoSoRel);
                WoSOMethod = Object.values(WoSoRel);
                return $.ajax({
                    type: "POST",
                    url: '/BusinessAquisition/PostWoSoRel',
                    contentType: "application/json; charset=utf-8",
                    headers: { 'Content-Type': 'application/json' },
                    data: JSON.stringify(WoSOMethod),
                    dataType: "json",
                    success: function (result) {
                        window.locationre = result.url;

                    }
                });
                $("#preloaderblurred").hide();
                //console.log(WoSOMethod);
            }).catch((error) => {
                AppUtil.HandleError("WOForm", error);
                $("#preloaderblurred").hide();
            });

        }

    });

    $('#routing').on('change', (e) => {
                const routeId = $(e.target).val();
                // make an API call to get data for select2 based on the selected studentId
        api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
            //console.log(data);
            //const uniqueData = [...new Set(data.map(item => item.stepOperation))];
            //const uniqueData = data.filter((item, index, self) =>
            //    self.findIndex((t) => t.stepOperation === item.stepOperation) === index
            //);
            const selectElement = $('#StartingOpNo');
            const selectEndOpNo = $('#EndingOpNo');
            selectElement.html("");
            $.each(data, (index, item) => {
                selectElement.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
            const reversedData = data.slice().reverse();
            selectEndOpNo.html('');
            $.each(reversedData, (index, item) => {
                selectEndOpNo.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
        }).catch((error) => {
            console.error(error);
        });
    });       

    //---Filtering---
    $("#ConcludeProdn").on("click", function () {
        window.location.href = "/WorkOrder";
    });
    $("#btnClear").on("click", function () {
        $("#SearchSO").val('');
        $("#SearchCustomer").val('');
        $("#SearchPO").val('');
        $("#SearchSoDateFr").val('');
        $("#SearchSoDateTo").val('');
        $("#SearchPartNo").val('');
        // loadSO();
        $("#SalesOrders1 tbody tr").show();
    });
    $("#SearchSO").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#SalesOrders1 tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#SalesOrders1 tbody").append(noRecordsRow);
        } else {
            $("#SalesOrders1 tbody").find(".norecordsfound").remove();
        }
    });

    $("#SearchCustomer").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#SalesOrders1 tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#SalesOrders1 tbody").append(noRecordsRow);
        } else {
            $("#SalesOrders1 tbody").find(".norecordsfound").remove();
        }
    });

    $("#SearchPO").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[6]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#SalesOrders1 tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#SalesOrders1 tbody").append(noRecordsRow);
        } else {
            $("#SalesOrders1 tbody").find(".norecordsfound").remove();
        }
    });

    $("#SearchSoDateTo").on("change", function () {
        var fromDate = $("#SearchSoDateFr").val().split("/").reverse().join("-");
        var toDate = $("#SearchSoDateTo").val().split("/").reverse().join("-");
        var fromDateTimestamp = new Date(fromDate).getTime();
        var toDateTimestamp = new Date(toDate).getTime();

        if (fromDateTimestamp > toDateTimestamp) {
            alert("So Compl Dt From Is Greater Than So Compl Dt To");
            $("#SearchSoDateFr").val('');
            $("#SearchSoDateTo").val('');
            return false;
        }
        $("#SalesOrders1 tbody tr").filter(function () {
            var dateText = $(this.children[11]).text(); // assuming the date is in the 3rd column
            var tableDate = dateText.split("-").reverse().join("-");

            $(this).toggle(tableDate >= fromDate && tableDate <= toDate);
        });
        var $tableBody = $("#SalesOrders1 tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#SalesOrders1 tbody").append(noRecordsRow);
        } else {
            $("#SalesOrders1 tbody").find(".norecordsfound").remove();
        }
    });

    $("#SearchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#SalesOrders1 tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#SalesOrders1 tbody").append(noRecordsRow);
        } else {
            $("#SalesOrders1 tbody").find(".norecordsfound").remove();
        }
    });
    
    $("#updateWO").on("click", function () {
        var planwoqty = $('#PlanWoQty').val();
        var wonumber = $('#woNumber').text();
        var woid= $('#WOID').val();
        var soid = $('#SalesOrderId').val();
        var partid = $('#PartId').val();
        var WoComplDate = new Date(Date.parse($('#WoComplDate').val()));
        var formattedDate = WoComplDate.toISOString();
        var routingid = $("#routing").val();
        var startingOpNo = $("#StartingOpNo").val();
        var endingOpNo = $("#EndingOpNo").val();
        var soqty = $('#SoQty').val();
        var reqdate = $('#reqdate').val();
        var status = $('#woStatus').val();
        var selectedData = {};
        var rstDt = new Date(Date.parse($('#p1planComplDate').val()));
        const restrictDt = new Date(rstDt); // Get today's date

        // Check if WoComplDate is after today's date
        if (WoComplDate < restrictDt) {
            alert("Please Don't enter the previous completion date.");
            $('#WoComplDate').val(""); // Clear the invalid date
            return false; // Prevent further processing if date is invalid
        }
        

        if (planwoqty < soqty) {
            alert("Plan WO Qnty should be Greater than Or Equal To Sales Order Qnty");
            return false;
        }
        else {
            if (planwoqty > 0) {
                var rowData = {
                    parentWoId: parseInt(woid),
                    salesOrderId: parseInt(soid),
                    wonumber: " ",
                    partId: parseInt(partid),
                    partType: 0,
                    parentlevel: '',
                    calcWOQty: parseInt(planwoqty),
                    planCompletionDate: formattedDate,
                    routingId: parseInt(routingid),
                    startingOpNo: parseInt(startingOpNo),
                    endingOpNo: parseInt(endingOpNo),
                    status: parseInt(status)
                };

                selectedData = rowData;
                var resultData = [];
                
                api.post("/businessaquisition/WOpost", selectedData).then((data) => {
                    resultData.push(data);
                    var wosorel = [];
                    var wosomethod = {};
                    resultData.forEach(function (a, i) {
                        wosorel.push({
                            workOrderId: a.woid,
                            salesOrderId: a.salesOrderId,
                            active : 0
                        });
                    });
                    //requestInProgress = false;
                    api.getbulk("/WorkOrder/GetSoWo?workOrderId=" + woid).then((data) => {
                        var checkboxes = $('#multipleSO .rowMCheckbox:checked');
                        var salesOrderIds = [];
                        checkboxes.each(function () {
                            var row = $(this).closest('tr');
                            var salesOrderId = row.find('td:eq(0)').text(); // or row.find('td:eq(0)').text() if salesOrderId is in the first column
                            salesOrderIds.push(parseInt(salesOrderId));
                        });
                        const filteredData = data.filter((item) => salesOrderIds.includes(item.salesOrderId));
                        filteredData.forEach(function (a, i) {
                            wosorel.push({
                                wosoId: a.wosoId,
                                workOrderId: a.workOrderId,
                                salesOrderId: a.salesOrderId,
                                active: 2
                            });
                        });
                        wosomethod = Object.values(wosorel);
                        $.ajax({
                            type: "POST",
                            url: '/BusinessAquisition/PostWoSoRel',
                            contentType: "application/json; charset=utf-8",
                            headers: { 'Content-Type': 'application/json' },
                            data: JSON.stringify(wosomethod),
                            dataType: "json",
                            success: function (result) {
                            }
                        });

                        var bal = $('#BalQty').val();
                        var woinactive = {
                            woid: parseInt(woid),
                            salesOrderId: parseInt(soid),
                            wonumber: wonumber,
                            partId: parseInt(partid),
                            partType: 0,
                            parentlevel: '',
                            calcWOQty: parseInt(bal),
                            planCompletionDate: formattedDate,
                            routingId: parseInt(routingid),
                            startingOpNo: parseInt(startingOpNo),
                            endingOpNo: parseInt(endingOpNo),
                            status: parseInt(status),
                            active: 2
                        };
                        api.post("/businessaquisition/WOpost", woinactive).then((data) => {
                            loadWO();
                            loadWoSubCon();
                        }).catch((error) => {
                        });
                    });
                    //--
                   
                    //$("#btnPop1Close").click();
                }).catch((error) => {
                    AppUtil.HandleError("WOForm", error);
                    //console.log(error);
                });
            } else {
                alert("Please Select atleast one!");
            }
        }
       
    });

    $('#woc-partno').on('hidden.bs.modal', function (event) {
        loadWO();
        const selectElement = $('#StartingOpNo');
        const selectEndOpNo = $('#EndingOpNo');
        selectElement.html("");
        selectEndOpNo.html('');
        $('#routing').prop('readonly', false);
        $('#routing').css('pointer-events', '');
        $('#StartingOpNo').prop('readonly', false);
        $('#StartingOpNo').css('pointer-events', '');
        $('#EndingOpNo').prop('readonly', false);
        $('#EndingOpNo').css('pointer-events', '');
    });

    $('#woc-partno').on('shown.bs.modal', function (event) {
        // Select all checkboxes in the table with the class 'rowMCheckbox'
        var soqty = $('#SoQty').val();
        var qntyonhand = $('#QtyOnHand').val();
        var balqnty = soqty - qntyonhand;
        $('#BalQty').val(balqnty);
        const checkboxes = document.querySelectorAll('#multipleSO .rowMCheckbox');
       
        // Define the function to update the total quantity
        function updateTotalQty() {
            let total = 0; // Initialize the total variable
            let soid = 0;
            var reqdate = "";
            // Iterate over all checkboxes
            checkboxes.forEach(checkbox => {
                if (checkbox.checked) { // Check if the checkbox is checked
                    // Traverse to the row and find the quantity in the 4th cell (index 3)
                    const row = checkbox.closest('tr'); // Using closest() for better readability
                    const qty = parseInt(row.querySelector('td:nth-child(4)').textContent, 10); // Get the quantity value from the 4th cell
                    soid = parseInt(row.querySelector('td:nth-child(1)').textContent, 10);
                    reqdate = row.querySelector('td:nth-child(7)').textContent
                    if (!isNaN(qty)) { // Ensure that the qty is a valid number
                        total += qty; // Add the quantity to the total
                    }
                }
            });

            // Update the values of the input fields with the calculated total
            if (total > 0) {
                $('#SoQty').val(total);
            }
            $('#SalesOrderId').val(soid);
            $('#reqdate').val(reqdate);
            $('#PlanWoQty').val(total);
        }
        // Calculate the initial total when the modal is first shown
        updateTotalQty();

        checkboxes.forEach(checkbox => {
            // Remove any existing event listeners to avoid duplication
            checkbox.removeEventListener('change', updateTotalQty);
            checkbox.addEventListener('change', updateTotalQty);
        });
        
    });

    $('#popup2').on('hidden.bs.modal', function (event) {
        var tablebody = $("#MulitpleWOs tbody");
        $(tablebody).html("");
        var $modal = $(this);
        //setTimeout(function () {
        $modal.find('input[type=radio][name=radioWO]').unbind('change');
        $modal.find('input[type=radio][name=equalwo]').unbind('change');
        $modal.find('button[id=MultipleWo]').unbind('click');
        $('input[type=radio][name=radioWO]').prop('checked', false);
        $('input[type=radio][name=equalwo]').prop('checked', false);
        $("#dispatchDate").prop('disabled', false);
        $('#p2WOid').val('');
        $('#p2SalesOrderId').val('');
        $('#p2PartId').val('');
        $('#dispatchDate').val('');
        $('#equaldiv').hide();
        loadWO();
        noofWOCreation = [];
        //console.log(noofWOCreation);
        //}, 100);
    });

    $('#popup2').on('shown.bs.modal', function (event) {

        $("#MultipleWo").prop('disabled', false);
        var planwoqty = $('#p2totalSoQty').val();
        var wonumber = $('#p2WoNumber').text();
        var woid = $('#p2WOid').val();
        var soid = $('#p2SalesOrderId').val();
        var partid = $('#p2PartId').val();
        var parttype = $('#p2PartType').val();
        var wostatus = $('#p2Status').val();
        var reloadOption = $('#p2ReloadOption').val();
        var WoComplDate = new Date(Date.parse($('#p2PlanComplDate').text()));
        var Compldt = $('#p2PlanComplDate').text();
        var partNo = $('#p2PartNo').text();
        var partDesc = $('#P2partdesc').text();
        var formattedDate = WoComplDate.toISOString();
        var requestInProgress = false;
        let totalQuantity = 0;
        var balsoqnty = $('#p2BalQty').val();
        var qntyOnhand = $('#p2QtyOnHand').val();
        var balmanuf = planwoqty - qntyOnhand;
        $('#p2BaltoManuf').val(balmanuf);
        //document.querySelectorAll('#MulitpleWOs tbody tr').forEach(row => {
        //    const quantity = parseInt(row.cells[1].textContent);
        //    if (!isNaN(quantity)) {
        //        totalQuantity += quantity;
        //    }
        //});
        $('#popup2Sum').val(0);
        $('input[name="radioWO"]').prop('disabled', false);
        $('input[name="equalwo"]').prop('disabled', false);
        $("#MultipleWo").prop('disabled', false);
        $("#NewWoPopupBtn").prop('disabled', false);

        var sonumber = "";

        var parentchildwo = [];

        api.get("/WorkOrder/GetSoNumber?soid=" + soid).then(async (data) => {
            //console.log(data);
            sonumber = await data.soNumber;
            $('#p2soNumber').text(sonumber);
        });

        api.getbulk("/WorkOrder/AllParentChildWos?parentWoId=" + woid).then(async (data) => {
            //console.log(data);
            var rop = "";
            $.each(data, (index, item) => {
                parentchildwo.push(item);
                rop = item.reloadOption;
            });
            if (parentchildwo.length > 0) {
                if (rop == "Manual") {
                    $('input[name="equalwo"]').prop('disabled', true);
                    $('input[name="radioWO"]').prop('disabled', true);
                    $('input[type=radio][name="radioWO"]').eq(2).prop('checked', true);
                    $('#equaldiv').hide();
                    $("#MultipleWo").prop('disabled', true);
                    $("#NewWoPopupBtn").prop('disabled', true);
                    $("#dispatchDate").prop('disabled', true);
                    $('#popup2Sum').val(planwoqty);
                }
                else {

                    $('input[name="equalwo"]').prop('disabled', true);
                    $('input[name="radioWO"]').prop('disabled', true);
                    $('input[type=radio][name="radioWO"]').eq(1).prop('checked', true);
                    $('#equaldiv').show();
                    $("#MultipleWo").prop('disabled', true);
                    $("#NewWoPopupBtn").prop('disabled', true);
                    $("#dispatchDate").prop('disabled', true);
                    $('#popup2Sum').val(planwoqty);
                    if (rop == "EQD_D") {
                        $('input[type=radio][name="equalwo"]').eq(0).prop('checked', true);
                    } else if (rop == "EQD_W") {
                        $('input[type=radio][name="equalwo"]').eq(1).prop('checked', true);
                    } else if (rop == "EQD_M") {
                        $('input[type=radio][name="equalwo"]').eq(2).prop('checked', true);
                    }

                }
                let totalQuantity = parentchildwo.reduce((acc, current) => acc + current.calcWOQty, 0);
                $('#popup2Sum').val(totalQuantity);
                var tablebody = $("#MulitpleWOs tbody");
                $(tablebody).html("");//empty tbody
                if (parentchildwo.length === 0) {
                    // 2. Insert the "No Records Found" row
                    // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                    const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                    $(tablebody).append(noRecordsRow);
                }

                for (i = 0; i < parentchildwo.length; i++) {
                    $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", parentchildwo[i]));
                }

            }
        });
        


        $('input[type=radio][name=radioWO]').change(function () {
            if (this.value == "1") {
                $('#equaldiv').hide();
            } else if (this.value == "2") {
                $('#equaldiv').show();
            } else if (this.value == "3") {
                $('#equaldiv').hide();
                $('#popup3ManualMultiple').modal('show');
                $('#p3MsoNumber').text(sonumber);
                $("#ManualpartNo").text(partNo);
                $("#P3Mpartdesc").text(partDesc);
                $("#ManualwoCompletedBy").text(Compldt);
                $("#ManualTotalSoQty").val(planwoqty);
                $("#ManualPlanWoQty").val(planwoqty);
                $("#ManualWoComplDt").val(Compldt.split("-").join("-"));
                $("#Manualwoid").val(woid);
                $("#Manualsoid").val(soid);
                $("#ManualpartId").val(partid);
                $("#ManualpartType").val(parttype);
                $("#ManualWoNumber").val(wonumber);
                $("#ManualStatus").val(wostatus);
                $('#ManualBalSoQty').val(balsoqnty);
                $('#ManualQtyOnHand').val(qntyOnhand);

            } else {
                $('#equaldiv').hide();
            }
        });

        $('input[type=radio][name=equalwo]').change( function () {
            //event.stopImmediatePropagation();
            event.stopPropagation();
            if (this.value == "1") {
                var dispatchDateInput = $('#dispatchDate');
                var dispatchDate = new Date(dispatchDateInput.val()); // assuming dispatchDate is an input field
                var completedDateElement = $('#p2PlanComplDate').text();
                var completedDate = new Date(completedDateElement.replace(/-/g, '/')); // assuming completedDate is an input field
                var maxDate = new Date(completedDate); // Set the particular date here
                var today = new Date();

                if (dispatchDate > maxDate || dispatchDate < today) {
                    alert("Please select the Dispatch Date between Todays Date and Completed By Date");
                    return false;
                }
                var dateDiff = Math.abs(completedDate - dispatchDate); // calculate the absolute difference in milliseconds
                var daysDiff = Math.ceil(dateDiff / (1000 * 3600 * 24)); // convert to days
                if (isNaN(dispatchDate.getTime())) {
                    alert("Please Select Date.");
                    $(this).prop('checked', false);
                }
                if (daysDiff > 22) {
                    alert("The number of days between dispatch date and completed date must be lesser than 22 days.");
                    $(this).prop('checked', false); // uncheck the radio button
                }
                else {
                    var dispatchDateIso = dispatchDate.toISOString();
                    var completedDateIso = completedDate.toISOString();
                    var disoption = "Daily";
                    api.get("/WorkOrder/CalculateWOQuantity?dispatchStartDate=" + dispatchDateIso + "&soCompletionDate=" + completedDateIso + "&balanceToManufacture=" + planwoqty + "&dispatchOption=" + disoption).then((data) => {
                        //console.log(data);
                        noofWOCreation.push(...data);
                    });
                }
            } else if (this.value == "2") {
                var dispatchDateInput = $('#dispatchDate');
                var dispatchDate = new Date(dispatchDateInput.val()); // assuming dispatchDate is an input field
                var completedDateElement = $('#p2PlanComplDate').text();
                var completedDate = new Date(completedDateElement.replace(/-/g, '/')); // assuming completedDate is an input field
                var maxDate = new Date(completedDate); // Set the particular date here
                var today = new Date();

                if (dispatchDate > maxDate || dispatchDate < today) {
                    alert("Please select the Dispatch Date between Todays Date and Completed By Date");
                    return false;
                }
                if (isNaN(dispatchDate.getTime())) {
                    alert("Please Select Date.");
                    $(this).prop('checked', false);
                }
                var dateDiff = Math.abs(completedDate - dispatchDate); // calculate the absolute difference in milliseconds
                var daysDiff = Math.ceil(dateDiff / (1000 * 3600 * 24)); // convert to days
                if (daysDiff <= 22) {
                    alert("The number of days between dispatch date and completed date must be greater than 22 days.");
                    $(this).prop('checked', false); // uncheck the radio button
                }
                else {
                    var dispatchDateIso = dispatchDate.toISOString();
                    var completedDateIso = completedDate.toISOString();
                    var disoption = "Weekly";
                    api.get("/WorkOrder/CalculateWOQuantity?dispatchStartDate=" + dispatchDateIso + "&soCompletionDate=" + completedDateIso + "&balanceToManufacture=" +planwoqty + "&dispatchOption=" + disoption).then((data) => {
                        //console.log(data);
                        noofWOCreation.push(...data);
                        var tablebody = $("#MulitpleWOs tbody");
                        $(tablebody).html("");//empty tbody
                        if (noofWOCreation.length === 0) {
                            // 2. Insert the "No Records Found" row
                            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                            $(tablebody).append(noRecordsRow);
                        }

                        for (i = 0; i < noofWOCreation.length; i++) {
                            $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", noofWOCreation[i]));
                        }
                    });
                }
            } else if (this.value == "3") {
                var dispatchDateInput = $('#dispatchDate');
                var dispatchDate = new Date(dispatchDateInput.val()); // assuming dispatchDate is an input field
                var completedDateElement = $('#p2PlanComplDate').text();
                var completedDate = new Date(completedDateElement.replace(/-/g, '/')); // assuming completedDate is an input field
                var maxDate = new Date(completedDate); // Set the particular date here
                var today = new Date();

                if (dispatchDate > maxDate || dispatchDate < today) {
                    alert("Please select the Dispatch Date between Todays Date and Completed By Date");
                    return false;
                }
                if (isNaN(dispatchDate.getTime())) {
                    alert("Please Select Date.");
                    $(this).prop('checked', false);
                }
                var dateDiff = Math.abs(completedDate - dispatchDate); // calculate the absolute difference in milliseconds
                var daysDiff = Math.ceil(dateDiff / (1000 * 3600 * 24)); // convert to days
                if (daysDiff <= 95) {
                    alert("The number of days between dispatch date and completed date must be greater than 95days.");
                    $(this).prop('checked', false); // uncheck the radio button
                }
                else {
                    var dispatchDateIso = dispatchDate.toISOString();
                    var completedDateIso = completedDate.toISOString();
                    var disoption = "Monthly";
                    api.get("/WorkOrder/CalculateWOQuantity?dispatchStartDate=" + dispatchDateIso + "&soCompletionDate=" + completedDateIso + "&balanceToManufacture=" + planwoqty + "&dispatchOption=" + disoption).then((data) => {
                        //console.log(data);
                        noofWOCreation.push(...data);
                    });
                }

            } else {

            }
        });

        $("#MultipleWo").on("click", function () {
            const selectedValue = $('input[name="equalwo"]:checked').val();
            var eq = "";
            if (selectedValue == "1") {
                eq = "D";
            }
            else if (selectedValue == "2") {
                eq = "W";
            }
            else {
                eq = "M";
            }
            let result = confirm("Are You Sure You Want To Proceed?");
            if (result) {

            } else {
                return false;
            }
            //if (requestInProgress) return;
            //requestInProgress = true;
            noofWOCreation.forEach((wo) => {
                wo.partId = parseInt(partid);
                wo.salesOrderId = parseInt(soid);
                wo.parentWoId = parseInt(woid);
                wo.reloadOption = "EQD_" + eq;
            });
            var tdata = [];
            if (noofWOCreation.length > 1) {
                $.ajax({
                    type: "POST",
                    url: '/BusinessAquisition/MultipleWOPost',
                    contentType: "application/json; charset=utf-8",
                    headers: { 'Content-Type': 'application/json' },
                    data: JSON.stringify(noofWOCreation),
                    dataType: "json",
                    success: function (result) {
                        //console.log(result);
                        tdata.push(...result);
                        var tablebody = $("#MulitpleWOs tbody");
                        $(tablebody).html("");//empty tbody
                        if (tdata.length === 0) {
                            // 2. Insert the "No Records Found" row
                            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                            $(tablebody).append(noRecordsRow);
                        }

                        for (i = 0; i < tdata.length; i++) {
                            $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", tdata[i]));
                        }
                        var wosorel = [];
                        var wosomethod = {};
                        tdata.forEach(function (a, i) {
                            wosorel.push({
                                workOrderId: a.woid,
                                salesOrderId: a.salesOrderId
                            });
                        });
                        requestInProgress = false;
                        wosomethod = Object.values(wosorel);
                        //--
                        $.ajax({
                            type: "POST",
                            url: '/BusinessAquisition/PostWoSoRel',
                            contentType: "application/json; charset=utf-8",
                            headers: { 'Content-Type': 'application/json' },
                            data: JSON.stringify(wosomethod),
                            dataType: "json",
                            success: function (result) {
                                window.locationre = result.url;
                                noofWOCreation = [];
                                $("#MultipleWo").prop('disabled', true);
                                $('input[name="equalwo"]').prop('disabled', true);
                                $('input[name="radioWO"]').prop('disabled', true);
                                $('input[type=radio][name="radioWO"]').eq(1).prop('checked', true);
                                $('#equaldiv').show();
                                $("#MultipleWo").prop('disabled', true);
                                $("#NewWoPopupBtn").prop('disabled', true);
                                $("#dispatchDate").prop('disabled', true);
                                requestInProgress = false;
                                $('#popup2Sum').val(planwoqty);
                            }
                        });
                        var woinactive = {
                            woid: parseInt(woid),
                            salesOrderId: parseInt(soid),
                            wonumber: wonumber,
                            partId: parseInt(partid),
                            partType: parseInt(parttype),
                            parentlevel: '',
                            calcWOQty: parseInt(planwoqty),
                            planCompletionDate: formattedDate,
                            routingId: parseInt(0),
                            startingOpNo: parseInt(0),
                            endingOpNo: parseInt(0),
                            status: parseInt(1),
                            active: 2
                        };
                        api.post("/businessaquisition/WOpost", woinactive).then((data) => {
                            //console.log(data);
                            //$('#popup3').modal('hide');
                            //reloadWO(reloadOption, partid);
                            loadWO();
                        }).catch((error) => {
                        });

                    }
                });
            }


        });

        /*
        //var tdata = [];
        //var wodt = $('#p2PlanComplDate').text();
        //if (reloadOption == "EQD_D") {
        //    $("#MultipleWo").prop('disabled', true);
        //    $("#NewWoPopupBtn").prop('disabled', true);
        //    $('input[type="radio"]').prop('disabled', true);
        //    $('input[name="equalwo"]').prop('disabled', true);
        //    // Then show the 1st radio btn checked
        //    $('input[type=radio][name="equalwo"]').eq(0).prop('checked', true);
        //    var trowdata = {
        //        active: 0,
        //        buildToStock: "\u0000",
        //        calcWOQty: planwoqty,
        //        comment: null,
        //        endingOpNo: 0,
        //        for_Ref: "\u0000",
        //        parentlevel: "N",
        //        partDesc: "",
        //        partId: parseInt(partid),
        //        partType: parseInt(parttype),
        //        partNo: "",
        //        planCompletionDateStr: wodt.split("-").reverse().join("-").join("-"),
        //        reloadOption: reloadOption,
        //        routingId: 0,
        //        salesOrderId: parseInt(soid),
        //        startingOpNo: 0,
        //        status: parseInt(wostatus),
        //        tenantId: 0,
        //        testData: "Y",
        //        woDate: null,
        //        woDateStr: "",
        //        woNumber: wonumber,
        //        woid: parseInt(woid)
        //    };

        //    tdata.push(trowdata);
        //    var tablebody = $("#MulitpleWOs tbody");
        //    $(tablebody).html("");//empty tbody

        //    for (i = 0; i < tdata.length; i++) {
        //        $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", tdata[i]));
        //    }

        //}
        //else if (reloadOption == "EQD_W") {
        //    $("#MultipleWo").prop('disabled', true);
        //    $("#NewWoPopupBtn").prop('disabled', true);
        //    $('input[type="radio"]').prop('disabled', true);
        //    $('input[name="equalwo"]').prop('disabled', true);
        //    // Then show the 1st radio btn checked
        //    $('input[type=radio][name="equalwo"]').eq(1).prop('checked', true);

        //    var trowdata = {
        //        active: 0,
        //        buildToStock: "\u0000",
        //        calcWOQty: planwoqty,
        //        comment: null,
        //        endingOpNo: 0,
        //        for_Ref: "\u0000",
        //        parentlevel: "N",
        //        partDesc: "",
        //        partId: parseInt(partid),
        //        partType: parseInt(parttype),
        //        partNo: "",
        //        planCompletionDateStr: wodt.split("-").reverse().join("-"),
        //        reloadOption: reloadOption,
        //        routingId: 0,
        //        salesOrderId: parseInt(soid),
        //        startingOpNo: 0,
        //        status: parseInt(wostatus),
        //        tenantId: 0,
        //        testData: "Y",
        //        woDate: null,
        //        woDateStr: "",
        //        woNumber: wonumber,
        //        woid: parseInt(woid)
        //    };

        //    tdata.push(trowdata);
        //    var tablebody = $("#MulitpleWOs tbody");
        //    $(tablebody).html("");//empty tbody

        //    for (i = 0; i < tdata.length; i++) {
        //        $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", tdata[i]));
        //    }

        //}
        //else if (reloadOption == "EQD_M") {
        //    $("#MultipleWo").prop('disabled', true);
        //    $("#NewWoPopupBtn").prop('disabled', true);
        //    $('input[type="radio"]').prop('disabled', true);
        //    $('input[name="equalwo"]').prop('disabled', true);
        //    // Then show the 1st radio btn checked
        //    $('input[type=radio][name="equalwo"]').eq(2).prop('checked', true);

        //    var trowdata = {
        //        active: 0,
        //        buildToStock: "\u0000",
        //        calcWOQty: planwoqty,
        //        comment: null,
        //        endingOpNo: 0,
        //        for_Ref: "\u0000",
        //        parentlevel: "N",
        //        partDesc: "",
        //        partId: parseInt(partid),
        //        partType: parseInt(parttype),
        //        partNo: "",
        //        planCompletionDateStr: wodt.split("-").reverse().join("-"),
        //        reloadOption: reloadOption,
        //        routingId: 0,
        //        salesOrderId: parseInt(soid),
        //        startingOpNo: 0,
        //        status: parseInt(wostatus),
        //        tenantId: 0,
        //        testData: "Y",
        //        woDate: null,
        //        woDateStr: "",
        //        woNumber: wonumber,
        //        woid: parseInt(woid)
        //    };

        //    tdata.push(trowdata);
        //    var tablebody = $("#MulitpleWOs tbody");
        //    $(tablebody).html("");//empty tbody

        //    for (i = 0; i < tdata.length; i++) {
        //        $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", tdata[i]));
        //    }

        //}
        */

    });

    $('#popup3').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var workOrderId = relatedTarget.data("workorderid");
        var salesOrderId = relatedTarget.data("salesorderid");
        var woNumber = relatedTarget.data("wonumber");
        var partNo = relatedTarget.data("partno");
        var planCompletionDateStr = relatedTarget.data("plancompletiondatestr");
        var partId = relatedTarget.data("partid");
        var partType = relatedTarget.data("parttype");
        var planWOQty = relatedTarget.data("calwoqty");
        var wostatus = relatedTarget.data("ptstatus");
        var reloadopt = relatedTarget.data("nreloadoption");
        var partno = $("#p2PartNo").text();
        var partDesc = $("#p2PartNo").text();
        var sonumber = $("#p2soNumber").text();
        $("#p3soNumber").text(sonumber);
        $("#p3partNo").text(partno);
        $("#P3partdesc").text(partDesc);
        $("#woCompletedBy").text(planCompletionDateStr.split("-").reverse().join("-"));
        $("#singleTotalSoQty").val(planWOQty);
        $("#singlePlanWoQty").val(planWOQty);
        $("#woid").val(workOrderId);
        $("#soid").val(salesOrderId);
        $("#partId").val(partId);
        $("#partType").val(partType);
        $("#p3Status").val(wostatus);
        $("#singleWoNumber").val(woNumber);
        $("#p3ReloadOption").val(reloadopt);
        $("#singleWoComplDt").val(planCompletionDateStr.split("-").reverse().join("-"));
        $("#singleBalSoQty").val(0);
        $("#singleQtyOnHand").val(0);
        $("#singleNoofWoReleased").val(0);
        $("#singleSumWoQnty").val(0);
        $("#singleBalWoQty").val(0);

        var balmanufqnty = planWOQty - 0;

        $("#singleBalToManuf").val(balmanufqnty);

        if (partType === 1) {
            $("#popup3divRouting").show().addClass("row");
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partId)).then((data) => {
                //console.log(data);
                const selectElement = $('#singleRouting');
                selectElement.prop("disabled", false);
                selectElement.html("");
                if (data.length === 1) {
                    $('#singleRouting').prop('readonly', true);
                    $('#singleRouting').css('pointer-events', 'none');
                    $.each(data, (index, item) => {
                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                    });
                    var routeId = $('#singleRouting').val();
                    $('#singleEndOpNo').prop('readonly', true);
                    $('#singleEndOpNo').css('pointer-events', 'none');
                    $('#singleStartOpNo').prop('readonly', true);
                    $('#singleStartOpNo').css('pointer-events', 'none');
                    api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
                        //console.log(data);
                        const selectstartElement = $('#singleStartOpNo');
                        const selectEndOpNo = $('#singleEndOpNo');
                        selectstartElement.html("");
                        $.each(data, (index, item) => {
                            selectstartElement.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
                        });
                        const reversedData = data.slice().reverse();
                        selectEndOpNo.html('');
                        $.each(reversedData, (index, item) => {
                            selectEndOpNo.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
                        });
                        loadWoP3SubConSIngle();
                    }).catch((error) => {
                        console.error(error);
                    });
                } else {

                    selectElement.append(`<option value="0">--Select--</option>`);
                    $.each(data, (index, item) => {
                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                    });
                }

            }).catch((error) => {
            });
            $('#singleStartOpNo').prop("disabled", false);
            $('#singleEndOpNo').prop("disabled", false);
            $("#SubConGridDivP3").show();
        }
        else {
            $("#popup3divRouting").hide();
            $("#SubConGridDivP3").hide();
            const selectElement = $('#singleRouting');
            selectElement.html("");
            selectElement.prop("disabled", true);
            $('#singleStartOpNo').html("").prop("disabled", true);
            $('#singleEndOpNo').html("").prop("disabled", true);
        }
        


    });

    $('#popup3').on('hidden.bs.modal', function (event) {
        const selectElement = $('#singleStartOpNo');
        const selectEndOpNo = $('#singleEndOpNo');
        selectElement.html("");
        selectEndOpNo.html('');
        $('#singleRouting').prop('readonly', false);
        $('#singleRouting').css('pointer-events', '');
        $('#singleStartOpNo').prop('readonly', false);
        $('#singleStartOpNo').css('pointer-events', '');
        $('#singleEndOpNo').prop('readonly', false);
        $('#singleEndOpNo').css('pointer-events', '');
    });

    $('#singleRouting').on('change', (e) => {
        const routeId = $(e.target).val();
        // make an API call to get data for select2 based on the selected studentId
        //api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
        //    //console.log(data);
        //    const selectElement = $('#singleStartOpNo');
        //    const selectEndOpNo = $('#singleEndOpNo');
        //    $.each(data, (index, item) => {
        //        selectElement.html("");
        //        selectElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
        //    });
        //    const reversedData = data.slice().reverse();
        //    selectEndOpNo.html('');
        //    $.each(reversedData, (index, item) => {
        //        selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
        //    });
        //}).catch((error) => {
        //    //console.error(error);
        //});

        api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
            //console.log(data);
            //const uniqueData = [...new Set(data.map(item => item.stepOperation))];
            const uniqueData = data.filter((item, index, self) =>
                self.findIndex((t) => t.stepOperation === item.stepOperation) === index
            );
            const selectElement = $('#singleStartOpNo');
            const selectEndOpNo = $('#singleEndOpNo');
            selectElement.html("");
            $.each(uniqueData, (index, item) => {
                selectElement.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
            const reversedData = uniqueData.slice().reverse();
            selectEndOpNo.html('');
            $.each(reversedData, (index, item) => {
                selectEndOpNo.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
        }).catch((error) => {
            console.error(error);
        });
    });

    $("#singleSaveWo").on("click", function () {
        var woid = parseInt($("#woid").val());
        var soid = parseInt($("#soid").val());
        var partid = parseInt($("#partId").val());
        var soqty = parseInt($("#singleTotalSoQty").val());
        var parttype = parseInt($("#partType").val());
        var planWoQty = parseInt($("#singlePlanWoQty").val());
        var wonumber = $("#singleWoNumber").val();
        var WoComplDate = new Date(Date.parse($('#singleWoComplDt').val()));
        var formattedDate = WoComplDate.toISOString();
        var routingid = $("#singleRouting").val();
        var startingOpNo = $("#singleStartOpNo").val();
        var endingOpNo = $("#singleEndOpNo").val();
        var status = parseInt($("#p3Status").val());
        var reloadOption = $("#p3ReloadOption").val();
        const woCompletedByText = $('#woCompletedBy').text();
        const rstDtParts = woCompletedByText.split('-');
        const rstDt = new Date(rstDtParts[2], rstDtParts[1] - 1, rstDtParts[0]);
        const restrictDt = new Date(rstDt);
        if (isNaN(WoComplDate.getTime())) {
            alert("Please Enter the WO Compl Date.");
            return false;
            // or display an error message to the user
        } else if (WoComplDate < restrictDt) {
            alert("Wo Complition Date Should Be Greater Than Current Complition Date");
            return false;
            // or display an error message to the user
        }
        else {
            formattedDate = WoComplDate.toISOString();
        }

        if (planWoQty < soqty) {
            alert("Plan Wo Qnty Should be Greater or Equal to Total So Qnty.");
            return false;
        }

        var rowData = {
            woid: parseInt(woid),
            salesOrderId: parseInt(soid),
            wonumber: wonumber,
            partId: parseInt(partid),
            partType: parseInt(parttype),
            parentlevel: '',
            calcWOQty: parseInt(planWoQty),
            planCompletionDate: formattedDate,
            routingId: parseInt(routingid),
            startingOpNo: parseInt(startingOpNo),
            endingOpNo: parseInt(endingOpNo),
            reloadOption: reloadOption,
            status: parseInt(status)
        };

        api.post("/businessaquisition/WOpost", rowData).then((data) => {
            //console.log(data);
            //$('#popup3').modal('hide');
            loadWoP3SubConSIngle();
            reloadWO(reloadOption, partid);
        }).catch((error) => {
        });

    });

    $('#popup3ManualMultiple').on('shown.bs.modal', function (event) {
        var partType = $("#ManualpartType").val();
        var partId = $("#ManualpartId").val();
        var totalsoqnty = $('#ManualTotalSoQty').val();
        var balSoqnty = $('#ManualBalSoQty').val();
        var QntyOnhand = $('#ManualQtyOnHand').val();
        var balmanuf = totalsoqnty - QntyOnhand;
        $('#ManualBalToManuf').val(balmanuf);
        $('#ManualNoofWoReleased').val(0);
        $('#ManualSumWoQnty').val(0);
        $('#ManualBalWoQty').val(0);
        if (partType == "1") {
            $("#popup3ManualdivRouting").show().addClass("row");
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partId)).then((data) => {
                //console.log(data);
                const selectElement = $('#ManualRouting');
                selectElement.prop("disabled", false);
                selectElement.html("");
                if (data.length === 1) {
                    $('#ManualRouting').prop('readonly', true);
                    $('#ManualRouting').css('pointer-events', 'none');
                    $.each(data, (index, item) => {
                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                    });
                    var routeId = $('#ManualRouting').val();
                    $('#ManualStartOpNo').prop('readonly', true);
                    $('#ManualStartOpNo').css('pointer-events', 'none');
                    $('#ManualEndOpNo').prop('readonly', true);
                    $('#ManualEndOpNo').css('pointer-events', 'none');
                    api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
                        //console.log(data);
                        const selectstartElement = $('#ManualStartOpNo');
                        const selectEndOpNo = $('#ManualEndOpNo');
                        selectstartElement.html("");
                        $.each(data, (index, item) => {
                            selectstartElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
                        });
                        const reversedData = data.slice().reverse();
                        selectEndOpNo.html('');
                        $.each(reversedData, (index, item) => {
                            selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
                        });
                    }).catch((error) => {
                        console.error(error);
                    });
                } else {

                selectElement.append(`<option value="0">--Select--</option>`);
                $.each(data, (index, item) => {
                    selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);
                   
                });
                }
               
            }).catch((error) => {
            });
            $('#ManualStartOpNo').prop("disabled", false);
            $('#ManualEndOpNo').prop("disabled", false);
        }
        else {
            $("#popup3ManualdivRouting").hide();
            const selectElement = $('#routing');
            selectElement.html("");
            selectElement.prop("disabled", true);
            $('#ManualStartOpNo').html("").prop("disabled", true);
            $('#ManualEndOpNo').html("").prop("disabled", true);
        }
    });

    $('#ManualRouting').on('change', (e) => {
        const routeId = $(e.target).val();
        // make an API call to get data for select2 based on the selected studentId
        api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
            //console.log(data);
            //const uniqueData = [...new Set(data.map(item => item.stepOperation))];
            const uniqueData = data.filter((item, index, self) =>
                self.findIndex((t) => t.stepOperation === item.stepOperation) === index
            );
            const selectElement = $('#ManualStartOpNo');
            const selectEndOpNo = $('#ManualEndOpNo');
            selectElement.html("");
            $.each(uniqueData, (index, item) => {
                selectElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
            });
            const reversedData = uniqueData.slice().reverse();
            selectEndOpNo.html('');
            $.each(reversedData, (index, item) => {
                selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
            });
        }).catch((error) => {
            //console.error(error);
        });
    });

    $('#popup3ManualMultiple').on('hidden.bs.modal', function (event) {
        var $modal = $(this);
        //$modal.find('button[id=ManualSaveWo]').unbind('click');
        $('#ManualRouting').prop('readonly', false);
        $('#ManualRouting').css('pointer-events', '');
        $('#ManualStartOpNo').prop('readonly', false);
        $('#ManualStartOpNo').css('pointer-events', '');
        $('#ManualEndOpNo').prop('readonly', false);
        $('#ManualEndOpNo').css('pointer-events', '');
        const selectElement = $('#ManualStartOpNo');
        const selectEndOpNo = $('#ManualEndOpNo');
        selectElement.html("");
        selectEndOpNo.html('');
    });

    $("#ManualSaveWo").on("click", function () {
        var woid = parseInt($("#Manualwoid").val());
        var soid = parseInt($("#Manualsoid").val());
        var partid = parseInt($("#ManualpartId").val());
        var parttype = parseInt($("#ManualpartType").val());
        var wostatus = parseInt($("#ManualStatus").val());
        var soqty = parseInt($("#ManualTotalSoQty").val());
        var planWoQty = parseInt($("#ManualPlanWoQty").val());
        var wonumber = $("#ManualWoNumber").val();
        var WoComplDate = new Date(Date.parse($('#ManualWoComplDt').val()));
        var formattedDate;
        var routingid = $("#ManualRouting").val();
        var startingOpNo = $("#ManualStartOpNo").val();
        var endingOpNo = $("#ManualEndOpNo").val();
        var resultData = [];
        var rstDt = new Date(Date.parse($('#ManualwoCompletedBy').text()));
        const restrictDt = new Date(rstDt);
        if (isNaN(WoComplDate.getTime())) {
            alert("Please Enter the WO Compl Date.");
            return false;
            // or display an error message to the user
        }
        if (WoComplDate < restrictDt) {
            alert("Wo Complition Date Should Be Greater Than Current Complition Date");
            return false;
            // or display an error message to the user
        } else {
            formattedDate = WoComplDate.toISOString();
        }

        if (planWoQty < soqty) {
            alert("Plan Wo Qnty Should be Greater or Equal to So Total So Qnty.");
            return false;
        }

        var rowData = {
            parentWoId: parseInt(woid),
            salesOrderId: parseInt(soid),
            wonumber:"",
            partId: parseInt(partid),
            partType: parseInt(parttype),
            parentlevel: '',
            calcWOQty: parseInt(planWoQty),
            reloadOption: "Manual",
            planCompletionDate: formattedDate,
            routingId: parseInt(routingid),
            startingOpNo: parseInt(startingOpNo),
            endingOpNo: parseInt(endingOpNo),
            status: parseInt(wostatus)
        };

        api.post("/businessaquisition/WOpost", rowData).then((data) => {
            //console.log(data);
            resultData.push(data);
            $('#popup3ManualMultiple').modal('hide');
            //$('#popup2Sum').val(planwoqty);
            var tablebody = $("#MulitpleWOs tbody");
            $(tablebody).html("");//empty tbody
            if (resultData.length === 0) {
                // 2. Insert the "No Records Found" row
                // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $(tablebody).append(noRecordsRow);
            }

            for (i = 0; i < resultData.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", resultData[i]));
            }
            $('input[name="equalwo"]').prop('disabled', true);
            $('input[name="radioWO"]').prop('disabled', true);
            $('input[type=radio][name="radioWO"]').eq(2).prop('checked', true);
            $('#equaldiv').hide();
            $("#MultipleWo").prop('disabled', true);
            $("#NewWoPopupBtn").prop('disabled', true);
            $("#dispatchDate").prop('disabled', true);
            $('#popup2Sum').val(planwoqty);
            var wosorel = [];
            var wosomethod = {};
            resultData.forEach(function (a, i) {
                wosorel.push({
                    workOrderId: a.woid,
                    salesOrderId: a.salesOrderId
                });
            });
            //requestInProgress = false;
            wosomethod = Object.values(wosorel);
            //--
            $.ajax({
                type: "POST",
                url: '/BusinessAquisition/PostWoSoRel',
                contentType: "application/json; charset=utf-8",
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(wosomethod),
                dataType: "json",
                success: function (result) {
                    //window.locationre = result.url;
                    //noofWOCreation = [];
                    //$("#MultipleWo").prop('disabled', true);
                    //$('input[name="equalwo"]').prop('disabled', true);
                    //$('input[name="radioWO"]').prop('disabled', true);
                    //$('input[type=radio][name="radioWO"]').eq(1).prop('checked', true);
                    //$('#equaldiv').show();
                    //$("#MultipleWo").prop('disabled', true);
                    //$("#NewWoPopupBtn").prop('disabled', true);
                    //$("#dispatchDate").prop('disabled', true);
                    //requestInProgress = false;
                }
            });
            var woinactive = {
                woid: parseInt(woid),
                salesOrderId: parseInt(soid),
                wonumber: wonumber,
                partId: parseInt(partid),
                partType: parseInt(parttype),
                parentlevel: '',
                calcWOQty: parseInt(planwoqty),
                planCompletionDate: formattedDate,
                routingId: parseInt(0),
                startingOpNo: parseInt(0),
                endingOpNo: parseInt(0),
                status: parseInt(1),
                active: 2
            };
            api.post("/businessaquisition/WOpost", woinactive).then((data) => {
                //console.log(data);
                //$('#popup3').modal('hide');
                //reloadWO(reloadOption, partid);
                loadWO();
            }).catch((error) => {
            });
        }).catch((error) => {
        });

    });

    $("#NewWoPopupBtn").on("click", function () {
        var planwoqty = $('#p2totalSoQty').val();
        var wonumber = $('#p2WoNumber').text();
        var sonumber = $('#p2soNumber').text();
        var woid = $('#p2WOid').val();
        var soid = $('#p2SalesOrderId').val();
        var partid = $('#p2PartId').val();
        var parttype = $('#p2PartType').val();
        var wostatus = $('#p2Status').val();
        var WoComplDate = new Date(Date.parse($('#p2PlanComplDate').text()));
        var Compldt = $('#p2PlanComplDate').text();
        var partNo = $('#p2PartNo').text();
        var partDesc = $('#P2partdesc').text();
        var formattedDate = WoComplDate.toISOString();
        var balsoqnty = $('#p2BalQty').val();
        var qntyOnhand = $('#p2QtyOnHand').val();

        $('#popup3NewWo').modal('show');
        $("#NewpartNo").text(partNo);
        $("#P3Npartdesc").text(partDesc);
        $("#p3NsoNumber").text(sonumber);
        $("#NewwoCompletedBy").text(Compldt);
        $("#NewTotalSoQty").val(planwoqty);
        $("#NewPlanWoQty").val(planwoqty);
        $("#Newwoid").val(woid);
        $("#Newsoid").val(soid);
        $("#NewpartId").val(partid);
        $("#NewpartType").val(parttype);
        $("#NewWoStatus").val(wostatus);
        $("#NewWoNumber").val(wonumber);
        $("#NewBalSoQty").val(balsoqnty);
        $("#NewQtyOnHand").val(qntyOnhand);
        $("#NewWoComplDt").val(Compldt.split("-").join("-"));
        document.getElementById("NewSaveWo").textContent = "Generate WO";
    });

    $("#NewSaveWo").on("click", function () {
        var woid = parseInt($("#Newwoid").val());
        var soid = parseInt($("#Newsoid").val());
        var partid = parseInt($("#NewpartId").val());
        var parttype = parseInt($("#NewpartType").val());
        var wostatus = parseInt($("#NewWoStatus").val());
        var planWoQty = parseInt($("#NewPlanWoQty").val());
        var soqty = parseInt($("#NewTotalSoQty").val());
        var wonumber = $("#NewWoNumber").val();
        var WoComplDate = new Date(Date.parse($('#NewWoComplDt').val()));
        var formattedDate;
        var routingid = $("#NewRouting").val();
        var startingOpNo = $("#NewStartOpNo").val();
        var endingOpNo = $("#NewEndOpNo").val();
        var sreloadOption = $("#p3NReloadOption").val();
        var reloadOpt = "New";
        var resultData = [];
        var rstDt = new Date(Date.parse($('#NewwoCompletedBy').text()));
        const restrictDt = new Date(rstDt);
        if (planWoQty < soqty) {
            alert("Plan Wo Qnty Should be Greater or Equal to So Total So Qnty.");
            return false;
        }
        if (WoComplDate < restrictDt) {
            alert("Wo Complition Date Should Be Greater Than Current Complition Date");
            return false;
            // or display an error message to the user
        } else {
            formattedDate = WoComplDate.toISOString();
        }
        if (sreloadOption) {
            reloadOpt = sreloadOption;
        }
        var rowData = {
            woid: parseInt(woid),
            salesOrderId: parseInt(soid),
            wonumber: wonumber,
            partId: parseInt(partid),
            partType: parseInt(parttype),
            parentlevel: '',
            calcWOQty: parseInt(planWoQty),
            planCompletionDate: formattedDate,
            reloadOption: reloadOpt,
            routingId: parseInt(routingid),
            startingOpNo: parseInt(startingOpNo),
            endingOpNo: parseInt(endingOpNo),
            status: parseInt(wostatus)
        };

        api.post("/businessaquisition/WOpost", rowData).then((data) => {
            //console.log(data);
            resultData.push(data);
            //$('#popup3NewWo').modal('hide');
            var tablebody = $("#MulitpleWOs tbody");
            $(tablebody).html("");//empty tbody
            if (resultData.length === 0) {
                // 2. Insert the "No Records Found" row
                // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $(tablebody).append(noRecordsRow);
            }

            for (i = 0; i < resultData.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", resultData[i]));
            }
            loadWO();
            loadWoP3SubConNew();
        }).catch((error) => {
        });

    });

    $('#NewRouting').on('change', (e) => {
        const routeId = $(e.target).val();
        api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
            //console.log(data);
            //const uniqueData = [...new Set(data.map(item => item.stepOperation))];
            //const uniqueData = data.filter((item, index, self) =>
            //    self.findIndex((t) => t.stepOperation === item.stepOperation) === index
            //);
            const selectElement = $('#NewStartOpNo');
            const selectEndOpNo = $('#NewEndOpNo');
            selectElement.html("");
            $.each(data, (index, item) => {
                selectElement.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
            const reversedData = data.slice().reverse();
            selectEndOpNo.html('');
            $.each(reversedData, (index, item) => {
                selectEndOpNo.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
        }).catch((error) => {
            //console.error(error);
        });

    });
    $('#popup3NewWo').on('hidden.bs.modal', function (event) {
        var $modal = $(this);
        //$modal.find('button[id=ManualSaveWo]').unbind('click');
        $('#NewRouting').prop('readonly', false);
        $('#NewRouting').css('pointer-events', '');
        $('#NewStartOpNo').prop('readonly', false);
        $('#NewStartOpNo').css('pointer-events', '');
        $('#NewEndOpNo').prop('readonly', false);
        $('#NewEndOpNo').css('pointer-events', '');
        const selectElement = $('#NewStartOpNo');
        const selectEndOpNo = $('#NewEndOpNo');
        selectElement.html("");
        selectEndOpNo.html('');
    });
    $('#popup3NewWo').on('shown.bs.modal', function (event) {
        var totalsoqnty = $("#NewTotalSoQty").val();
        var balsoq = $("#NewBalSoQty").val();
        var qntonhand = $("#NewQtyOnHand").val();
        var balmanuf = totalsoqnty - balsoq;
        $('#NewBalToManuf').val(balmanuf);
        $('#NewNoofWoReleased').val(0);
        $('#NewSumWoQnty').val(0);
        $('#NewBalWoQty').val(0);
        var partType = $("#NewpartType").val();
        var partId = $("#NewpartId").val();

        if (partType == "1") {
            //api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partId)).then((data) => {
            //    //console.log(data);
            //    const selectElement = $('#NewRouting');
            //    selectElement.prop("disabled", false);
            //    $.each(data, (index, item) => {
            //        selectElement.html("");
            //        selectElement.append(`<option value="0">--Select--</option>`);
            //        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);
            //    });
            //}).catch((error) => {
            //});
            //$('#NewStartOpNo').prop("disabled", false);
            //$('#NewEndOpNo').prop("disabled", false);
            //-----new 
            $("#popup3NewdivRouting").show().addClass("row");
            $("#SubConGridDivP3").show();
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partId)).then((data) => {
                //console.log(data);
                const selectElement = $('#NewRouting');
                selectElement.prop("disabled", false);
                selectElement.html("");
                if (data.length === 1) {
                    $('#NewRouting').prop('readonly', true);
                    $('#NewRouting').css('pointer-events', 'none');
                    $.each(data, (index, item) => {
                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                    });
                    var routeId = $('#NewRouting').val();
                    $('#NewStartOpNo').prop('readonly', true);
                    $('#NewStartOpNo').css('pointer-events', 'none');
                    $('#NewEndOpNo').prop('readonly', true);
                    $('#NewEndOpNo').css('pointer-events', 'none');
                    api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
                        //console.log(data);
                        const selectstartElement = $('#NewStartOpNo');
                        const selectEndOpNo = $('#NewEndOpNo');
                        selectstartElement.html("");
                        $.each(data, (index, item) => {
                            selectstartElement.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
                        });
                        const reversedData = data.slice().reverse();
                        selectEndOpNo.html('');
                        $.each(reversedData, (index, item) => {
                            selectEndOpNo.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
                        });
                        loadWoP3SubConNew();
                    }).catch((error) => {
                        console.error(error);
                    });
                } else {

                    selectElement.append(`<option value="0">--Select--</option>`);
                    $.each(data, (index, item) => {
                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                    });
                }

            }).catch((error) => {
            });
            $('#NewStartOpNo').prop("disabled", false);
            $('#NewEndOpNo').prop("disabled", false);
        }
        else {
            $("#popup3NewdivRouting").hide();
            $("#SubConGridDivP3").hide();
            const selectElement = $('#NewRouting');
            selectElement.html("");
            selectElement.prop("disabled", true);
            $('#NewStartOpNo').html("").prop("disabled", true);
            $('#NewStartOpNo').html("").prop("disabled", true);
        }
    });
    $('#popup7').on('hidden.bs.modal', function (event) {
        var $modal = $(this);
        //$modal.find('button[id=ManualSaveWo]').unbind('click');
        $('#popup7Routing').prop('readonly', false);
        $('#popup7Routing').css('pointer-events', '');
        $('#popup7StartingOpNo').prop('readonly', false);
        $('#popup7StartingOpNo').css('pointer-events', '');
        $('#popup7EndingOpNo').prop('readonly', false);
        $('#popup7EndingOpNo').css('pointer-events', '');
        const selectElement = $('#popup7StartingOpNo');
        const selectEndOpNo = $('#popup7EndingOpNo');
        selectElement.html("");
        selectEndOpNo.html('');
    });
    $('#popup7').on('shown.bs.modal', function (event) {
        //var formattedDate = planCompletionDateStr.split("-").reverse().join("-");
        LoadPartsExist();
        var P20DelQnty = document.getElementById('popup7WoComplDt');
        P20DelQnty.style.border = '';
        var popup7BuildToStockQnty = document.getElementById('popup7BuildToStockQnty');
        popup7BuildToStockQnty.style.border = '';
    });

    $('#popup7Routing').on('change', (e) => {
        const routeId = $(e.target).val();
        // make an API call to get data for select2 based on the selected studentId
        //api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
        //    //console.log(data);
        //    const selectElement = $('#popup7StartingOpNo');
        //    const selectEndOpNo = $('#popup7EndingOpNo');
        //    $.each(data, (index, item) => {
        //        selectElement.html("");
        //        selectElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
        //    });
        //    const reversedData = data.slice().reverse();
        //    selectEndOpNo.html('');
        //    $.each(reversedData, (index, item) => {
        //        selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
        //    });
        //}).catch((error) => {
        //    //console.error(error);
        //});
        api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
            //console.log(data);
            //const uniqueData = [...new Set(data.map(item => item.stepOperation))];
            //const uniqueData = data.filter((item, index, self) =>
            //    self.findIndex((t) => t.stepOperation === item.stepOperation) === index
            //);
            const selectElement = $('#popup7StartingOpNo');
            const selectEndOpNo = $('#popup7EndingOpNo');
            selectElement.html("");
            $.each(data, (index, item) => {
                selectElement.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
            const reversedData = data.slice().reverse();
            selectEndOpNo.html('');
            $.each(reversedData, (index, item) => {
                selectEndOpNo.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
            });
        }).catch((error) => {
            //console.error(error);
        });
    });

    $("#popup7SaveWo").on("click", function () {
        var woid = parseInt(0);
        var soid = parseInt($("#Popup7soid").val());
        var partid = parseInt($("#Popup7partId").val());
        var parttype = parseInt($("#Popup7partType").val());
        var wostatus = parseInt($("#Popup7WoStatus").val());
        var planWoQty = parseInt($("#popup7BuildToStockQnty").val());
        //var soqty = parseInt($("#NewTotalSoQty").val());
        var wonumber = $("#popup7WoNumber").text();
        var WoComplDate = new Date(Date.parse($('#popup7WoComplDt').val()));
        var routingid = $("#popup7Routing").val();
        var startingOpNo = $("#popup7StartingOpNo").val();
        var endingOpNo = $("#popup7EndingOpNo").val();
        var WoSoRel = [];
        var WoSOMethod = {};

        if (planWoQty.length <= 0 || parseInt(planWoQty) == 0 ) {
            var newNamevalidate = document.getElementById('popup7BuildToStockQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('popup7BuildToStockQnty');
            P20DelQnty.style.border = '';
        }
        if (WoComplDate.length <= 0) {
            var newNamevalidate = document.getElementById('popup7WoComplDt');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('popup7WoComplDt');
            P20DelQnty.style.border = '';
            var formattedDate = WoComplDate.toISOString();
        }
        const currentDate = new Date();
        const userDate = new Date(WoComplDate);
        //const lessDate = new Date(P10DateReqd);
        if (userDate < currentDate ) {
            alert('Please Enter A Date Greater Than Today\'s Date.');
            $("#popup7WoComplDt").val('');
            return;
        }
        var rowData = {
            woid: parseInt(woid),
            salesOrderId: parseInt(soid),
            wonumber: wonumber,
            partId: parseInt(partid),
            partType: parseInt(parttype),
            parentlevel: '',
            calcWOQty: parseInt(planWoQty),
            planCompletionDate: formattedDate,
            routingId: parseInt(routingid),
            startingOpNo: parseInt(startingOpNo),
            endingOpNo: parseInt(endingOpNo),
            status: parseInt(wostatus),
            buildToStock:'Y'
        };

        api.post("/businessaquisition/WOpost", rowData).then((data) => {
            //console.log(data);
            WoSoRel.push({
                workOrderId: data.woid,
                salesOrderId: parseInt(soid)
            });
            WoSOMethod = Object.values(WoSoRel);
            $.ajax({
                type: "POST",
                url: '/BusinessAquisition/PostWoSoRel',
                contentType: "application/json; charset=utf-8",
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(WoSOMethod),
                dataType: "json",
                success: function (result) {
                    window.locationre = result.url;
                }
            });
            $("#popup7WoNumber").text(data.woNumber);
            $('#Popup7woid').val(data.woid);
            loadWO();
            loadWoP7SubCon();
        }).catch((error) => {
        });
    });


    $('#P20Supplier').on('change', (e) => {
        const routeId = $(e.target).val();
        var NewStartOpNo = $("#NewStartOpNo").val();
        if (NewStartOpNo == null) {
            StartingOpNo = $("#StartingOpNo").val();
        }
        if (NewStartOpNo == null) {
            NewStartOpNo = $("#popup7StartingOpNo").val();
        }
        if (NewStartOpNo == null) {
            NewStartOpNo = $("#singleStartOpNo").val();
        }
        api.get("/routings/subcons?stepId=" + NewStartOpNo).then((data) => {
            var edata = data.filter(item => item.supplierId == routeId);
            if (edata[0].strPreferredSubCon === "") {
                $("#P20PreferredSpan").hide();
            } else {
                $("#P20PreferredSpan").show();
            }
            var cost = edata[0].costPerPart;
            $("#P20UnitRout").val(cost);
            if (edata[0].strPreferredSubCon === "") {
                $("#P20PreferredSpan").hide();
            } else {
                $("#P20PreferredSpan").show();
            }
        }).catch((error) => {
            //console.error(error);
        });
    });
    $('#popup20').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var woid = relatedTarget.data("workorderid");
        $("#flexSwitchCheckDefault").prop("disabled", false);
        var salesOrderId = relatedTarget.data("salesorderid");
        var woNumber = relatedTarget.data("wonumber");
        var partNo = relatedTarget.data("partno");
        var stepid = relatedTarget.data("stepid");
        var planwoqty = relatedTarget.data("qntys");
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        $('#P20AgrDate').val('');
        $('#P20AddnInfo').val('');
        $("#flexSwitchCheckDefault").prop("checked", false);
        $("#P20AddNextDel").prop("disabled", true);
        var P20AddnInfo = document.getElementById('P20AddnInfo');
        P20AddnInfo.style.border = '';
        var P20AgrDate = document.getElementById('P20AgrDate');
        P20AgrDate.style.border = '';
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        supdate = {};
        subcontotal = 0;
        if (woid === 1) {
            //var planwoqty = $('#SoQty').val();
            var orgwoid = $('#Popup20Woid').val();
            var routingId = $('#routing').val();
            var WoComplDate = $('#WoComplDate').val();
            var partNo = $('#partNo').text();
            var P1partdesc = $('#P1partdesc').text();
            var NewStartOpNoText = $("#StartingOpNo option:selected").text();
            var NewStartOpNo = $("#StartingOpNo").val();
            GetAllSubCons(orgwoid);
            $("#P20DelQnty").val(planwoqty);
            $("#P20WoQnty").val(planwoqty);
            $("#P20DateReqd").val(WoComplDate);
            $("#popup20PartNo").text(partNo);
            $("#P20partdesc").text(P1partdesc);
            $("#P20OpnoSpan").text(NewStartOpNoText);
            //$("#Popup20Woid").text(orgwoid);
            api.get("/routings/subcons?stepId=" + NewStartOpNo).then((data) => {
                const selectElement = $('#P20Supplier');
                selectElement.html("");
                $.each(data, (index, item) => {
                    selectElement.append(`<option value="${item.supplierId}">${item.company}</option>`);
                });
                if (data.length == 1) {
                    $("#P20AddOtherSupp").prop("disabled", true);
                } else {
                    $("#P20AddOtherSupp").prop("disabled", false);
                }
                var edata = data;
                if (edata[0].strPreferredSubCon === "") {
                    $("#P20PreferredSpan").hide();
                } else {
                    $("#P20PreferredSpan").show();
                }
            });
            api.get("/masters/masterparts").then((data) => {
                var edata = data.filter(item => item.partNo == partNo);
                api.get("/WorkOrder/GetManufPart?routingId=" + edata[0].partId).then((pdata) => {
                    var epdata = pdata;
                    $("#P20Convprice").val(epdata.priceSettledwithCustomer_INR);
                    $("#P20UnitRout").val(epdata.priceSettledwithCustomer_INR);
                });
            });
        }else if (woid == "3N") {
            var subqnt = $("#NewPlanWoQty").val();
            var reqddate = $("#NewWoComplDt").val();
            var NewRouting = $("#NewRouting").val();
            var NewStartOpNo = $("#NewStartOpNo").val();
            var NewStartOpNoText = $("#NewStartOpNo option:selected").text();
            var NewpartNo = $("#NewpartNo").text();
            var P3Npartdesc = $("#P3Npartdesc").text();
            var Newwoid = $("#Newwoid").val();
            GetAllSubCons(Newwoid);
            $("#P20DelQnty").val(planwoqty);
            $("#P20WoQnty").val(planwoqty);
            $("#P20DateReqd").val(reqddate);
            $("#Popup20Woid").val(Newwoid);
            $("#popup20PartNo").text(NewpartNo);
            $("#P20partdesc").text(P3Npartdesc);
            $("#P20OpnoSpan").text(NewStartOpNoText);
            api.get("/routings/subcons?stepId=" + NewStartOpNo).then((data) => {
                const selectElement = $('#P20Supplier');
                selectElement.html("");
                $.each(data, (index, item) => {
                    selectElement.append(`<option value="${item.supplierId}">${item.company}</option>`);
                });
                if (data.length == 1) {
                    $("#P20AddOtherSupp").prop("disabled", true);
                } else {
                    $("#P20AddOtherSupp").prop("disabled", false);
                }
                var edata = data;
                if (edata[0].strPreferredSubCon === "") {
                    $("#P20PreferredSpan").hide();
                } else {
                    $("#P20PreferredSpan").show();
                }
            });
            api.get("/masters/masterparts").then((data) => {
                var edata = data.filter(item => item.partNo == NewpartNo);
                api.get("/WorkOrder/GetManufPart?routingId=" + edata[0].partId).then((pdata) => {
                    var epdata = pdata;
                    $("#P20Convprice").val(epdata.priceSettledwithCustomer_INR);
                    $("#P20UnitRout").val(epdata.priceSettledwithCustomer_INR);
                });
            });
        } else if (woid == "3S") {
            var subqnt = $("#singlePlanWoQty").val();
            var reqddate = $("#singleWoComplDt").val();
            var NewRouting = $("#singleRouting").val();
            var NewStartOpNo = $("#singleStartOpNo").val();
            var NewStartOpNoText = $("#singleStartOpNo option:selected").text();
            var NewpartNo = $("#p3partNo").text();
            var P3Npartdesc = $("#P3partdesc").text();
            var Newwoid = $("#woid").val();
            GetAllSubCons(Newwoid);
            $("#P20DelQnty").val(planwoqty);
            $("#P20WoQnty").val(planwoqty);
            $("#P20DateReqd").val(reqddate);
            $("#Popup20Woid").val(Newwoid);
            $("#popup20PartNo").text(NewpartNo);
            $("#P20partdesc").text(P3Npartdesc);
            $("#P20OpnoSpan").text(NewStartOpNoText);

            api.get("/routings/subcons?stepId=" + NewStartOpNo).then((data) => {
                const selectElement = $('#P20Supplier');
                selectElement.html("");
                $.each(data, (index, item) => {
                    selectElement.append(`<option value="${item.supplierId}">${item.company}</option>`);
                });
                if (data.length == 1) {
                    $("#P20AddOtherSupp").prop("disabled", true);
                } else {
                    $("#P20AddOtherSupp").prop("disabled", false);
                }
                var edata = data;
                if (edata[0].strPreferredSubCon === "") {
                    $("#P20PreferredSpan").hide();
                } else {
                    $("#P20PreferredSpan").show();
                }
            });
            api.get("/masters/masterparts").then((data) => {
                var edata = data.filter(item => item.partNo == NewpartNo);
                api.get("/WorkOrder/GetManufPart?routingId=" + edata[0].partId).then((pdata) => {
                    var epdata = pdata;
                    $("#P20Convprice").val(epdata.priceSettledwithCustomer_INR);
                    $("#P20UnitRout").val(epdata.priceSettledwithCustomer_INR);
                });
            });
        } else if (woid == 7) {
            var subqnt = $("#popup7WoQnty").val();
            var reqddate = $("#popup7WoComplDt").val();
            var NewRouting = $("#popup7Routing").val();
            var NewStartOpNo = $("#popup7StartingOpNo").val();
            var NewStartOpNoText = $("#popup7StartingOpNo option:selected").text();
            var partnos = $("#popup7PartNoField").val();
            var partarry = partnos.split('/');
            var NewpartNo = partarry[0];
            var P3Npartdesc = partarry[1];
            var Newwoid = $("#Popup7woid").val();
            if (Newwoid === "" || Newwoid === 0) {
                alert("Please Genearate The WO.");
                $("#popup20").modal("hide");
                return false;
            }
            GetAllSubCons(Newwoid);
            $("#P20DelQnty").val(planwoqty);
            $("#P20WoQnty").val(planwoqty);
            $("#P20DateReqd").val(reqddate);
            $("#Popup20Woid").val(Newwoid);
            $("#popup20PartNo").text(NewpartNo);
            $("#P20partdesc").text(P3Npartdesc);
            $("#P20OpnoSpan").text(NewStartOpNoText);

            api.get("/routings/subcons?stepId=" + NewStartOpNo).then((data) => {
                const selectElement = $('#P20Supplier');
                selectElement.html("");
                $.each(data, (index, item) => {
                    selectElement.append(`<option value="${item.supplierId}">${item.company}</option>`);
                });
                if (data.length == 1) {
                    $("#P20AddOtherSupp").prop("disabled", true);
                } else {
                    $("#P20AddOtherSupp").prop("disabled", false);
                }
                var edata = data;
                if (edata[0].strPreferredSubCon === "") {
                    $("#P20PreferredSpan").hide();
                } else {
                    $("#P20PreferredSpan").show();
                }
            });
            api.get("/masters/masterparts").then((data) => {
                var edata = data.filter(item => item.partNo == NewpartNo);
                $("#P20partdesc").text(edata[0].description);
                api.get("/WorkOrder/GetManufPart?routingId=" + edata[0].partId).then((pdata) => {
                    var epdata = pdata;
                    $("#P20Convprice").val(epdata.priceSettledwithCustomer_INR);
                    $("#P20UnitRout").val(epdata.priceSettledwithCustomer_INR);
                });
            });
        }
    });
    $("#P20AddNextDel").on("click", function () {
        $("#P20DelQnty").val('');
        $("#P20AgrDate").val('');
        $("#P20Convprice").val('');
        //$("#P10Supplier").val('');
        $("#Popup20WoSubConId").val('');
        $("#P20AddnInfo").val('');
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        var P20AddnInfo = document.getElementById('P20AddnInfo');
        P20AddnInfo.style.border = '';
        var P20AgrDate = document.getElementById('P20AgrDate');
        P20AgrDate.style.border = '';

    });
    $("#P20AddOtherSupp").on("click", function () {
        $("#P20DelQnty").val('');
        $("#P20AgrDate").val('');
        $("#P20Convprice").val('');
        $("#P20Supplier").val('');
        $("#Popup20WoSubConId").val('');
        $("#P20AddnInfo").val('');
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        var P20AgrDate = document.getElementById('P20AgrDate');
        P20AgrDate.style.border = '';
        var P20AddnInfo = document.getElementById('P20AddnInfo');
        P20AddnInfo.style.border = '';
    });
    var supdate = {};
    var totalwoplanqnty = 0;
    $("#P20SaveWo").on("click", function () {
        var qntys = $("#P20DelQnty").val();
        var P20AgrDate = $("#P20AgrDate").val();
        var P20AddnInfo = $("#P20AddnInfo").val();
        var P20Convprice = $("#P20Convprice").val();
        var P20Supplier = $("#P20Supplier").val();
        var Newwoid = $("#Popup20Woid").val();
        var Popup20WoSubConId = $("#Popup20WoSubConId").val();
        var P10DateReqd = $("#P20DateReqd").val();
        var P10balQnty = $("#P20WoQnty").val();
        var balQtyToProcure = parseInt(P10balQnty);
        var diff = parseInt(balQtyToProcure) - parseInt(subcontotal);
        if (Popup20WoSubConId.length === 0) {
            if (parseInt(qntys) > diff || subcontotal > balQtyToProcure) {
                alert("Delivery Qnty should be less than or equal to the Difference Quantity" + diff);
                return false;
            }
        }
        if (qntys.length <= 0 || parseInt(qntys) == 0) {
            var newNamevalidate = document.getElementById('P20DelQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P20DelQnty');
            P20DelQnty.style.border = '';
        }
        if (P20AgrDate.length <= 0 ) {
            var newNamevalidate = document.getElementById('P20AgrDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P20AgrDate');
            P20DelQnty.style.border = '';
        }
        if (P20AddnInfo.length <= 0 ) {
            var newNamevalidate = document.getElementById('P20AddnInfo');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P20AddnInfo');
            P20DelQnty.style.border = '';
        }
        const currentDate = new Date();
        const userDate = new Date(P20AgrDate);
        const lessDate = new Date(P10DateReqd);
        if (userDate < currentDate || userDate > lessDate) {
            alert('Please Enter A Date Greater Than Today\'s Date And Less Than Date Reqd.');
            $("#P20AgrDate").val('');
            return;
        }
        var delv = "";
        var chekboxes = document.getElementById('flexSwitchCheckDefault');
        if (chekboxes.checked) {
            $("#P20AddNextDel").prop('disabled', false);
            delv = "MD";
        } else {
            $("#P20AddNextDel").prop('disabled', true);
            delv = "SDD";
        }
        var rowData = {
            woSubConSupplierId: parseInt(Popup20WoSubConId),
            woId: parseInt(Newwoid),
            supplierId: parseInt(P20Supplier),
            deliveryDate: delv,
            qnty: qntys,
            procPrice: P20Convprice,
            addnInfo: P20AddnInfo,
            recieptDate: P20AgrDate
        };

        api.post("/WorkOrder/WoSubConSupplier", rowData).then((data) => {
            //loadWO();
            $("#Popup20WoSubConId").val('');
            GetAllSubCons(Newwoid);
        }).catch((error) => {
        });
    });

    $("#baeppn").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbl-ba-existingparts tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#tbl-ba-existingparts tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }

    });

    $("#btnExistBtn").on("click", function () {
        $("#ba_existing-part").modal("show");
    });
    $("#baeppd").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbl-ba-existingparts tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#tbl-ba-existingparts tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });

});
document.addEventListener('DOMContentLoaded', function () {
    tippy('#btnExistBtn', {
        content: 'Search Part No',
        placement: 'top',
    });
});

function LoadPartsExist() {

    if (ba_masterparts.length > 0) {
        var tablebody = $("#tbl-ba-existingparts tbody");
        $(tablebody).html("");//empty tbody
        let i = 0;
        if (ba_masterparts.length > 0) {
            ba_masterparts = ba_masterparts.filter(item => item.finalPart === "Y");
            if (ba_masterparts.length === 0) {
                // 2. Insert the "No Records Found" row
                // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $(tablebody).append(noRecordsRow);
            }
            for (i = 0; i < ba_masterparts.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("BAParts", ba_masterparts[i], i));
            }
        }
    }
    else {
        api.get("/masters/masterparts").then((data) => {
            ba_masterparts = data;
            var tablebody = $("#tbl-ba-existingparts tbody");
            $(tablebody).html("");//empty tbody
            let i = 0;

            if (ba_masterparts.length === 0) {
                // 2. Insert the "No Records Found" row
                // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $(tablebody).append(noRecordsRow);
            }
            if (ba_masterparts.length > 0) {
                ba_masterparts = ba_masterparts.filter(item => item.finalPart === "Y");
                for (i = 0; i < ba_masterparts.length; i++) {
                    $(tablebody).append(AppUtil.ProcessTemplateDataNew("BAParts", ba_masterparts[i], i));
                }
            }
        }).catch((error) => {
        });
    }
}
function showLoadingScreen() {
    document.getElementById('preloader').style.display = 'block';
    document.getElementById('status').style.display = 'block';
    $.ajax({
        type: "POST",
        url: '/WorkOrder/ProcPlan',
        success: function (data) {
            document.getElementById('preloader').style.display = 'none';
            document.getElementById('status').style.display = 'none';
            window.location.href = '/D%231@122P%20I%255$3T%20I';
        }
    });
}
function copyPartData() {

    if (ba_masterparts.length == 0) {
        alert("Please load parts again...");
        return;
    }
    else {
        //alert("calling copyData");
    }
    var radiochkd = $('input[name=radiopartba]:checked');
    var selval = radiochkd.val();
    var data = ba_masterparts;
    //$('#SalesCustomerOrderId').val();
    var partId = data[selval].partId;
    $.ajax({
        type: "GET",
        url: "/masters/CheckPartNoInDocList",
        data: { partId: partId },
        success: function (response) {
            if (!response) {
                alert("This Part Doesnot Have Required Document.");
                return;
            }
            else {
                $("#popup7PartNoField").val(data[selval].partNo + "/" + data[selval].description);
                $("#Popup7partId").val(data[selval].partId);
                api.getbulk("/WorkOrder/AllSalesOrders").then((sodata) => {
                    sodata = sodata.filter(item => item.partId === partId && item.status !== 6);
                    $("#popup7SoQnty").val(sodata[0].requiredQuantity);
                    $("#Popup7soid").val(sodata[0].salesOrderId);
                    api.getbulk("/WorkOrder/AllWorkOrders").then((wodata) => {
                        wodata = wodata.filter(item => item.partId === partId);
                        $("#popup7WoQnty").val(wodata[0].calcWOQty);
                        if (data[selval].masterPartType == "Assembly") {
                            $("#Popup7partType").val(2);

                            $("#SubConGridDivP7").hide();
                            $("#popup7divRouting").hide();
                            const selectElement = $('#popup7Routing');
                            selectElement.html("");
                            selectElement.prop("disabled", true);
                            $('#popup7StartingOpNo').html("").prop("disabled", true);
                            $('#popup7EndingOpNo').html("").prop("disabled", true);
                        } else {
                            $("#Popup7partType").val(1);
                            $("#popup7divRouting").show().addClass("row");
                            $("#SubConGridDivP7").show();
                            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partId)).then((data) => {
                                //console.log(data);
                                const selectElement = $('#popup7Routing');
                                selectElement.prop("disabled", false);
                                selectElement.html("");
                                if (data.length === 1) {
                                    $('#popup7Routing').prop('readonly', true);
                                    $('#popup7Routing').css('pointer-events', 'none');
                                    $.each(data, (index, item) => {
                                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                                    });
                                    var routeId = $('#popup7Routing').val();
                                    $('#popup7StartingOpNo').prop('readonly', true);
                                    $('#popup7StartingOpNo').css('pointer-events', 'none');
                                    $('#popup7EndingOpNo').prop('readonly', true);
                                    $('#popup7EndingOpNo').css('pointer-events', 'none');
                                    api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
                                        //console.log(data);
                                        const selectstartElement = $('#popup7StartingOpNo');
                                        const selectEndOpNo = $('#popup7EndingOpNo');
                                        selectstartElement.html("");
                                        $.each(data, (index, item) => {
                                            selectstartElement.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
                                        });
                                        const reversedData = data.slice().reverse();
                                        selectEndOpNo.html('');
                                        $.each(reversedData, (index, item) => {
                                            selectEndOpNo.append(`<option value="${item.stepId}">${item.stepNumber}</option>`);
                                        });
                                        loadWoP7SubCon();
                                    }).catch((error) => {
                                        console.error(error);
                                    });
                                } else {

                                    selectElement.append(`<option value="0">--Select--</option>`);
                                    $.each(data, (index, item) => {
                                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                                    });
                                }

                            }).catch((error) => {
                            });
                            $('#popup7EndingOpNo').prop("disabled", false);
                            $('#popup7StartingOpNo').prop("disabled", false);
                            $('#ManualEndOpNo').prop("disabled", false);

                        }
                    }).catch((error) => {
                    });
                }).catch((error) => {
                });

                document.getElementById("btn-close-ba-ExistingParts").click();
            }
        }
    });



}
function EditWo(element) {
    //console.log("--Edit--");
    var relatedTarget = $(element);
    var workOrderId = relatedTarget.data("workorderid");
    var salesOrderId = relatedTarget.data("salesorderid");
    var woNumber = relatedTarget.data("wonumber");
    var partNo = relatedTarget.data("partno");
    var partDesc = relatedTarget.data("partdesc");
    var planCompletionDateStr = relatedTarget.data("plancompletiondatestr");
    var partId = relatedTarget.data("partid");
    var partType = relatedTarget.data("parttype");
    var planWOQty = relatedTarget.data("calwoqty");
    var woNumber = relatedTarget.data("wonumber");
    var wostatus = relatedTarget.data("wostatus");
    var reloadOption = relatedTarget.data("reloadoption");
    var daroutingid = relatedTarget.data("routingid");
    var startopno = relatedTarget.data("startopno");
    var endopno = relatedTarget.data("endopno");
    document.getElementById('WoComplDate').value = planCompletionDateStr.split("-").reverse().join("-");
    document.getElementById('p1planComplDate').value = planCompletionDateStr.split("-").reverse().join("-");
    $('#PlanWoQty').val(0);
    $('#SoQty').val(planWOQty);
    $('#woNumber').text(woNumber);
    $('#partNo').text(partNo);
    $('#P1partdesc').text(partDesc);
    $('#WOID').val(workOrderId);
    $('#Popup20Woid').val(workOrderId);
    $('#SalesOrderId').val(salesOrderId);
    $('#PartId').val(partId);
    $('#woStatus').val(wostatus);

    var WOSoTable = [];
    var temp = [];
    if (workOrderId > 0) {
        
        api.getbulk("/WorkOrder/GetSoWo?workOrderId=" + workOrderId).then((data) => {
            //console.log(data);
            $.each(data, (index, item) => {
                var rowdata = {
                    wosoId: parseInt(item.wosoId),
                    workOrderId: parseInt(item.workOrderId),
                    salesOrderId: parseInt(item.salesOrderId),
                    active: parseInt(item.active)

                };
                temp.push(rowdata);
            });
            

            $.ajax({
                type: "POST",
                url: '/WorkOrder/GetOneSO',
                contentType: "application/json; charset=utf-8",
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(temp),
                dataType: "json",
                success: function (result) {
                    window.locationre = result.url;
                    //console.log(result);
                    $.each(result, (index, item) => {
                        WOSoTable.push(item);
                    });

                    //CheckNumberOfSo(WOSoTable);     
                    if (WOSoTable.length >= 2) {
                        const salesOrderIdsToRemove = temp.filter((item) => item.active === 2).map((item) => item.salesOrderId);

                        WOSoTable = WOSoTable.filter((item) => !salesOrderIdsToRemove.includes(item.salesOrderId));
                        $('#woc-partno').modal('show');
                        $('#popup2').modal('hide');
                                               
                        var tablebody = $("#multipleSO tbody");
                        $(tablebody).html("");//empty tbody
                        if (WOSoTable.length === 0) {
                            // 2. Insert the "No Records Found" row
                            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                            $(tablebody).append(noRecordsRow);
                        }

                        for (i = 0; i < WOSoTable.length; i++) {
                            $(tablebody).append(AppUtil.ProcessTemplateData("MultipleSoRow", WOSoTable[i]));
                        }


                        if (partType === 1) {
                           
                            $("#popup1DivRouting").show().addClass("row");
                            $("#SubConGridDiv").show();
                            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partId)).then((data) => {
                                //console.log(data);
                                const selectElement = $('#routing');
                                selectElement.prop("disabled", false);
                                selectElement.html("");
                                if (data.length === 1) {
                                    $('#routing').prop('readonly', true);
                                    $('#routing').css('pointer-events', 'none');
                                    $.each(data, (index, item) => {
                                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                                    });
                                    var routeId = $('#routing').val();
                                    $('#StartingOpNo').prop('readonly', true);
                                    $('#StartingOpNo').css('pointer-events', 'none');
                                    $('#EndingOpNo').prop('readonly', true);
                                    $('#EndingOpNo').css('pointer-events', 'none');
                                    api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
                                        //console.log(data);
                                        const selectstartElement = $('#StartingOpNo');
                                        const selectEndOpNo = $('#EndingOpNo');
                                        selectstartElement.html("");
                                        $.each(data, (index, item) => {
                                            selectstartElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
                                        });
                                        const reversedData = data.slice().reverse();
                                        selectEndOpNo.html('');
                                        $.each(reversedData, (index, item) => {
                                            selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
                                        });
                                        loadWoSubCon();
                                    }).catch((error) => {
                                        console.error(error);
                                    });
                                } else {

                                    selectElement.append(`<option value="0">--Select--</option>`);
                                    $.each(data, (index, item) => {
                                        selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);

                                    });
                                    $('#routing').val(daroutingid);
                                    $('#StartingOpNo').val(startopno);
                                    $('#EndingOpNo').val(endopno);
                                }

                            }).catch((error) => {
                            });
                            $('#StartingOpNo').prop("disabled", false);
                            $('#EndingOpNo').prop("disabled", false);
                        }
                        else {
                            $("#popup1DivRouting").hide();
                            $("#SubConGridDiv").hide();
                            const selectElement = $('#routing');
                            selectElement.html("");
                            selectElement.prop("disabled", true);
                            $('#StartingOpNo').html("").prop("disabled", true);
                            $('#EndingOpNo').html("").prop("disabled", true);
                        }
                    }
                    else if (WOSoTable.length === 1) {
                        if (reloadOption) {
                            var sonumber = "";

                            api.get("/WorkOrder/GetSoNumber?soid=" + parseInt(salesOrderId)).then((data) => {
                                //console.log(data);
                                sonumber = data.soNumber;
                                $('#p3NsoNumber').text(sonumber);
                            });
                            $('#popup2').modal('hide');
                            $('#popup3NewWo').modal('show');
                            $("#NewpartNo").text(partNo);
                            $("#P3Npartdesc").text(partDesc);
                            $("#NewwoCompletedBy").text(planCompletionDateStr.split("-").reverse().join("-"));
                            $("#NewTotalSoQty").val(planWOQty);
                            $("#NewPlanWoQty").val(planWOQty);
                            $("#Newwoid").val(workOrderId);
                            $("#Newsoid").val(salesOrderId);
                            $("#NewpartId").val(partId);
                            $("#NewpartType").val(partType);
                            $("#NewWoStatus").val(wostatus);
                            $("#NewWoNumber").val(woNumber);
                            $("#p3NReloadOption").val(reloadOption);
                            $("#NewBalSoQty").val(0);
                            $("#NewQtyOnHand").val(0); //reloadOption
                            $("#NewWoComplDt").val(planCompletionDateStr.split("-").reverse().join("-"));
                            document.getElementById("NewSaveWo").textContent = "Save WO";
                        }
                        else {
                        $('#popup2').modal('show');
                        $('#p2totalSoQty').val(planWOQty);
                        $('#p2BalQty').val(0);
                        $('#p2WoNumber').text(woNumber);
                        $('#p2PartNo').text(partNo);
                        $('#P2partdesc').text(partDesc);
                        $('#p2WOid').val(workOrderId);
                        $('#p2SalesOrderId').val(salesOrderId);
                        $('#p2PartId').val(partId);
                        $('#p2PartType').val(partType);
                        $('#p2Status').val(wostatus);
                        $('#p2ReloadOption').val(reloadOption);
                        $('#p2PlanComplDate').text(planCompletionDateStr.split("-").reverse().join("-"));
                        //popup2show = true;
                        }
                    }
                    

                }
            });
          
            
        }).catch((error) => {
        });   
    }
    
}

//function CheckNumberOfSo(WOSoTable) {
//    if (WOSoTable.length >= 2) {
//        $('#woc-partno').modal('show');
//        $('#popup2').modal('hide');
//        var tablebody = $("#multipleSO tbody");
//        $(tablebody).html("");//empty tbody

//        for (i = 0; i < WOSoTable.length; i++) {
//            $(tablebody).append(AppUtil.ProcessTemplateData("MultipleSoRow", WOSoTable[i]));
//        }


//        if (partType === 1) {
//            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + partId).then((data) => {
//                console.log(data);
//                const selectElement = $('#routing');
//                selectElement.prop("disabled", false);
//                $.each(data, (index, item) => {
//                    selectElement.html("");
//                    selectElement.append(`<option value="0">--Select--</option>`);
//                    selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);
//                });
//            }).catch((error) => {
//            });
//            $('#StartingOpNo').prop("disabled", false);
//            $('#EndingOpNo').prop("disabled", false);
//        }
//        else {
//            const selectElement = $('#routing');
//            selectElement.html("");
//            selectElement.prop("disabled", true);
//            $('#StartingOpNo').html("").prop("disabled", true);
//            $('#EndingOpNo').html("").prop("disabled", true);
//        }
//    }
//    else if (WOSoTable.length === 1) {
//        $('#popup2').modal('show');
//        $('#p2totalSoQty').val(planWOQty);
//        $('#p2BalQty').val(0);
//        //$('#woNumber').text(woNumber);
//        $('#p2PartNo').text(partNo);
//        $('#p2WOid').val(workOrderId);
//        $('#p2SalesOrderId').val(salesOrderId);
//        $('#p2PartId').val(partId);
//        $('#p2PartType').val(partType);
//        $('#p2PlanComplDate').text(planCompletionDateStr.split("-").reverse().join("-"));
//        //popup2show = true;
//    }
//}

function MulitpleSoTo1Wo(rowData) {
    $("#multiplesaleO").click();
    var tablebody = $("#multipleSO tbody");
    $(tablebody).html("");//empty tbody
    if (rowData.length === 0) {
        // 2. Insert the "No Records Found" row
        // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
        const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
        $(tablebody).append(noRecordsRow);
    }
    
    for (i = 0; i < rowData.length; i++) {
        $(tablebody).append(AppUtil.ProcessTemplateData("MultipleSoRow", rowData[i]));
    }
}
function loadWoSubCon() {
    var routingId = parseInt($("#routing").val());
    var planqnty = parseInt($("#SoQty").val());
    const StartingOpNo = $('#StartingOpNo option:selected').text();
    const EndingOpNo = $('#EndingOpNo option:selected').text();
    api.getbulk("/WorkOrder/Subcon?routingId="+routingId).then((data) => {
        var tablebody = $("#subConGridP1 tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        let groupedData = {};
        data.forEach((item) => {
            if (!groupedData[item.routingStepId]) {
                // Keep the first occurrence as-is
                groupedData[item.routingStepId] = { ...item };
            } else {
                // Update only the noOfOperations field, keeping other properties of the first row intact
                groupedData[item.routingStepId].noOfOperations += item.noOfOperations;
            }
        });

        // Convert grouped data back to an array
        data = Object.values(groupedData);
        for (let i = 0; i < data.length; i++) {
            let splitqnty = Math.round(planqnty / data.length);
            data[i].qnty = splitqnty;
            data[i].startingOpNo = StartingOpNo;
            data[i].endingOpNo = EndingOpNo;
            data[i].woid = "1";
            if (data[i].noOfOperations == 1) {
                data[i].multipleSource = "N";
            } else {
                data[i].multipleSource = "Y";
            }
            // Dynamically add or exclude the dropdown div
            data[i].dropdownHTML = data[i].noOfOperations === 1
                ? ""
                : `
                <div class="dropdown float-center">
                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="mdi mdi-dots-vertical"></i>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end">
                        <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal"
                           data-workorderid="${data[i].woid}" data-salesorderid="${data[i].salesOrderId}" data-stepid="${data[i].routingStepId}"
                           data-wonumber="${data[i].woNumber}" data-qntys="${data[i].qnty}" data-partno="${data[i].partNo}"
                           data-bs-target="#popup20">Edit</a>
                    </div>
                </div>`;
        }

        // Append the rows to the table
        data.forEach((row) => {
            let rowHTML = `
                <tr>
                    <td>${row.company}</td>
                    <td>${row.qnty}</td>
                    <td>${row.startingOpNo}</td>
                    <td>${row.endingOpNo}</td>
                    <td>${row.multipleSource}</td>
                    <td>${row.dropdownHTML}</td>
                </tr>`;
            $(tablebody).append(rowHTML);
        });
    }).catch((error) => {
    });
}
function loadWoP3SubConSIngle() {
    var routingId = parseInt($("#singleRouting").val());
    var planqnty = parseInt($("#singlePlanWoQty").val());
    const StartingOpNo = $('#singleStartOpNo option:selected').text();
    const EndingOpNo = $('#singleEndOpNo option:selected').text();
    api.getbulk("/WorkOrder/Subcon?routingId=" + routingId).then((data) => {
        var tablebody = $("#subConGridP3 tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        let groupedData = {};
        data.forEach((item) => {
            if (!groupedData[item.routingStepId]) {
                groupedData[item.routingStepId] = { ...item };
            } else {
                // Update only the noOfOperations field, keeping other properties of the first row intact
                groupedData[item.routingStepId].noOfOperations += item.noOfOperations;
            }
        });
        data = Object.values(groupedData);
        for (let i = 0; i < data.length; i++) {
            let splitqnty = Math.round(planqnty / data.length);
            data[i].qnty = splitqnty;
            data[i].startingOpNo = StartingOpNo;
            data[i].endingOpNo = EndingOpNo;
            data[i].woid = "3S";
            if (data[i].noOfOperations == 1) {
                data[i].multipleSource = "N";
            } else {
                data[i].multipleSource = "Y";
            }
            // Dynamically add or exclude the dropdown div
            data[i].dropdownHTML = data[i].noOfOperations === 1
                ? ""
                : `
                <div class="dropdown float-center">
                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="mdi mdi-dots-vertical"></i>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end">
                        <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal" 
                           data-workorderid="${data[i].woid}" data-salesorderid="${data[i].salesOrderId}" data-stepid="${data[i].routingStepId}"
                           data-wonumber="${data[i].woNumber}" data-qntys="${data[i].qnty}" data-partno="${data[i].partNo}"
                           data-bs-target="#popup20">Edit</a>
                    </div>
                </div>`;
        }

        // Append the rows to the table
        data.forEach((row) => {
            let rowHTML = `
                <tr>
                    <td>${row.company}</td>
                    <td>${row.qnty}</td>
                    <td>${row.startingOpNo}</td>
                    <td>${row.endingOpNo}</td>
                    <td>${row.multipleSource}</td>
                    <td>${row.dropdownHTML}</td>
                </tr>`;
            $(tablebody).append(rowHTML);
        });
    }).catch((error) => {
    });
}

function loadWoP3SubConNew() {
    var routingId = parseInt($("#NewRouting").val());
    var planqnty = parseInt($("#NewPlanWoQty").val());
    const StartingOpNo = $('#NewStartOpNo option:selected').text();
    const EndingOpNo = $('#NewEndOpNo option:selected').text();
    api.getbulk("/WorkOrder/Subcon?routingId=" + routingId).then((data) => {
        var tablebody = $("#subConGridP3New tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);

        let groupedData = {};
        data.forEach((item) => {
            if (!groupedData[item.routingStepId]) {
                // Keep the first occurrence as-is
                groupedData[item.routingStepId] = { ...item };
            } else {
                // Update only the noOfOperations field, keeping other properties of the first row intact
                groupedData[item.routingStepId].noOfOperations += item.noOfOperations;
            }
        });
        data = Object.values(groupedData);
        for (let i = 0; i < data.length; i++) {
            let splitqnty = Math.round(planqnty / data.length);
            data[i].qnty = splitqnty;
            data[i].startingOpNo = StartingOpNo;
            data[i].endingOpNo = EndingOpNo;
            data[i].woid = "3N";
            if (data[i].noOfOperations == 1) {
                data[i].multipleSource = "N";
            } else {
                data[i].multipleSource = "Y";
            }
            // Dynamically add or exclude the dropdown div
            data[i].dropdownHTML = data[i].noOfOperations === 1
                ? ""
                : `
                <div class="dropdown float-center">
                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="mdi mdi-dots-vertical"></i>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end">
                        <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal" 
                           data-workorderid="${data[i].woid}" data-salesorderid="${data[i].salesOrderId}"  data-stepid="${data[i].routingStepId}"
                           data-wonumber="${data[i].woNumber}" data-qntys="${data[i].qnty}" data-partno="${data[i].partNo}"
                           data-bs-target="#popup20">Edit</a>
                    </div>
                </div>`;
        }

        // Append the rows to the table
        data.forEach((row) => {
            let rowHTML = `
                <tr>
                    <td>${row.company}</td>
                    <td>${row.qnty}</td>
                    <td>${row.startingOpNo}</td>
                    <td>${row.endingOpNo}</td>
                    <td>${row.multipleSource}</td>
                    <td>${row.dropdownHTML}</td>
                </tr>`;
            $(tablebody).append(rowHTML);
        });
    }).catch((error) => {
    });
}

function loadWoP7SubCon() {
    var routingId = parseInt($("#popup7Routing").val());
    var planqnty = parseInt($("#popup7WoQnty").val());
    const StartingOpNo = $('#popup7StartingOpNo option:selected').text();
    const EndingOpNo = $('#popup7EndingOpNo option:selected').text();
    api.getbulk("/WorkOrder/Subcon?routingId=" + routingId).then((data) => {
        var tablebody = $("#subConGridP7 tbody");
        $(tablebody).html("");
        let groupedData = {};
        data.forEach((item) => {
            if (!groupedData[item.routingStepId]) {
                // Keep the first occurrence as-is
                groupedData[item.routingStepId] = { ...item };
            } else {
                // Update only the noOfOperations field, keeping other properties of the first row intact
                groupedData[item.routingStepId].noOfOperations += item.noOfOperations;
            }
        });

        // Convert grouped data back to an array
        data = Object.values(groupedData);
        for (let i = 0; i < data.length; i++) {
            let splitqnty = Math.round(planqnty / data.length);
            data[i].qnty = splitqnty;
            data[i].startingOpNo = StartingOpNo;
            data[i].endingOpNo = EndingOpNo;
            data[i].woid = "7";
            if (data[i].noOfOperations == 1) {
                data[i].multipleSource = "N";
            } else {
                data[i].multipleSource = "Y";
            }
            // Dynamically add or exclude the dropdown div
            data[i].dropdownHTML = data[i].noOfOperations === 1
                ? ""
                : `
                <div class="dropdown float-center">
                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="mdi mdi-dots-vertical"></i>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end">
                        <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal" 
                           data-workorderid="${data[i].woid}" data-salesorderid="${data[i].salesOrderId}"  data-stepid="${data[i].routingStepId}"
                           data-wonumber="${data[i].woNumber}" data-qntys="${data[i].qnty}" data-partno="${data[i].partNo}"
                           data-bs-target="#popup20">Edit</a>
                    </div>
                </div>`;
        }

        // Append the rows to the table
        data.forEach((row) => {
            let rowHTML = `
                <tr>
                    <td>${row.company}</td>
                    <td>${row.qnty}</td>
                    <td>${row.startingOpNo}</td>
                    <td>${row.endingOpNo}</td>
                    <td>${row.multipleSource}</td>
                    <td>${row.dropdownHTML}</td>
                </tr>`;
            $(tablebody).append(rowHTML);
        });
    }).catch((error) => {
    });
}


function GetAllSubCons(woid) {
    api.getbulk("/WorkOrder/GetAllSubCons?woid=" + woid).then((data) => {
        //data = data.filter(item => item.woId === woid);
        var tablebody = $("#P20SupplierGrid tbody");
        $(tablebody).html("");//empty tbody
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        //console.log(data);
        let totalQuantity = 0;
        let totalprice = 0;
        subcontotal = 0;
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            totalQuantity += Number(data[i].qnty);
            totalprice += Number(data[i].procPrice);
            $(tablebody).append(AppUtil.ProcessTemplateData("P10SupplierGridRow", data[i]));
            subcontotal = totalQuantity;
        }
        const totalRow = `
            <tr>
                <td> </td>
                <td style="text-align: center; font-weight: bold;">Total Quantity : ${totalQuantity}</td>
                <td style="text-align: center; font-weight: bold;">Total Price : ${totalprice}</td>
                <td> </td>
            </tr>
        `;

        $(tablebody).append(totalRow);
    }).catch((error) => {
    });
}
function EditSubSupplier(element) {
    var relatedTarget = $(element);
    var calcdate = relatedTarget.data("calcdate");
    var price = relatedTarget.data("price");
    var qnty = relatedTarget.data("qnty");
    var subconid = relatedTarget.data("subconid");
    var workOrderId = relatedTarget.data("woid");
    var suppid = relatedTarget.data("suppid");
    var addninfo = relatedTarget.data("addninfo");
    var delv = relatedTarget.data("delv");
    let datePart = calcdate.split("T")[0];
    $("#P20Supplier").val(suppid);
    $("#Popup20Woid").val(workOrderId);
    $("#Popup20WoSubConId").val(subconid);
    $("#P20AgrDate").val(datePart);
    $("#P20DelQnty").val(qnty);
    $("#P20AddnInfo").val(addninfo);
    $("#P20Convprice").val(price);
    if (delv === "MD") {
        $("#flexSwitchCheckDefault").prop("checked", true);
    } else {
        $("#flexSwitchCheckDefault").prop("checked", false);
    }
    api.getbulk("/WorkOrder/GetAllSubCons?woid=" + workOrderId).then((data) => {
        data = data.filter(item => item.supplierId === suppid);
        if (data.length > 1) {
            $("#flexSwitchCheckDefault").prop("disabled", true);
            $("#flexSwitchCheckDefault").prop("checked", true);
        } else {
            $("#flexSwitchCheckDefault").prop("disabled", false);
        }
    }).catch((error) => {
    });
}
function DeleteSubSupplier(element) {
    var relatedTarget = $(element);
    var workOrderId = relatedTarget.data("woid");
    var subconid = relatedTarget.data("subconid");
    let result = confirm("Are You Sure You Want To Delete This?");
    if (result) {
        api.getbulk("/WorkOrder/DeleteSubCon?id=" + subconid).then((data) => {
            GetAllSubCons(workOrderId);
        });
    } else {
        return false;
    }
}
function DeleteWo(element) {
    var relatedTarget = $(element);
    var workOrderId = relatedTarget.data("workorderid");
    //var subconid = relatedTarget.data("subconid");
    let result = confirm("Are You Sure You Want To Delete This WO?");
    if (result) {
        api.getbulk("/WorkOrder/AllProductionWo").then((data) => {
            data = data.filter(item => item.status === 1 && item.woId === workOrderId);
            if (data.length === 0) {

                api.getbulk("/WorkOrder/DeleteWo?id=" + workOrderId + "&productionPlanId=" + 0).then((data) => {

                    if (data == false || data == true) {

                    } else {
                        alert(data);
                    }
                    loadWO();
                });
            }
           else if (data.length > 0) {
                api.getbulk("/WorkOrder/DeleteWo?id=" + workOrderId + "&productionPlanId=" + data[0].productionPlanId).then((data) => {

                    if (data == false || data == true) {

                    } else {
                        alert(data);
                    }
                    loadWO();
                });
            }
        });
    } else {
        return false;
    }
}