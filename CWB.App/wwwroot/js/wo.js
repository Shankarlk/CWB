﻿var noofWOCreation = [];
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
    api.getbulk("/WorkOrder/AllSalesOrders").then((data) => {
        var tablebody = $("#SalesOrders1 tbody");
        $(tablebody).html("");//empty tbody
        console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("SalesOrderRow1", data[i]));
        }
    }).catch((error) => {
    });
}

function loadWO() {
    api.getbulk("/WorkOrder/AllWorkOrders").then((data) => {
        data = data.filter(item => item.active !== 2);
        var tablebody = $("#WorkOrder tbody");
        $(tablebody).html("");//empty tbody
                                           

        console.log(data);
        for (i = 0; i < data.length; i++) {
            data[i].strStatus = WoOrdStatus[data[i].status];
            $(tablebody).append(AppUtil.ProcessTemplateData("WorkOrderRow", data[i]));
        }
    }).catch((error) => {
    });
}

function reloadWO(reloadOption, partid) {
    api.getbulk("/WorkOrder/ReloadWo?reloadoption=" + reloadOption + "&partid=" + partid).then((data) => {
        var tablebody = $("#MulitpleWOs tbody");
        $(tablebody).html("");//empty tbody
        console.log(data);
        for (i = 0; i < data.length; i++) {
            data[i].strStatus = WoOrdStatus[data[i].status];
            $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", data[i]));
        }
    }).catch((error) => {
    });
}

$(document).ready(function () {

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
        else {
            $('#btnGW').prop('disabled', true); // Disable the button
           // $('#btnAG').prop('disabled', false);
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
    $("#btnAG").on("click", function () {
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
                partType: 0,
                partlevel: ' ',
                calcWOQty: parseInt($(row).find("td:eq(8)").text()),
                planCompletionDate: $(row).find("td:eq(12)").text()
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

        console.log(selectedRowsData);

        if (selectedRowsData.length === 1) {
            // Single checkbox selected, post to WOpost
            api.post("/businessaquisition/WOpost", selectedRowsData[0]).then((data) => {
                // Handle success if needed
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
                console.log(WoSoRel);
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
                console.log(WoSOMethod);
            }).catch((error) => {
                AppUtil.HandleError("WOForm", error);
            });

        } else if (selectedRowsData.length > 1) {
            // Multiple checkboxes selected, post to MultiWOpost
            console.log("-- multiplepostwo");

            $.ajax({
                type: "POST",
                url: '/BusinessAquisition/MultipleWOPost',
                contentType: "application/json; charset=utf-8",
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(selectedRowsData),
                dataType: "json",
                success: function (result) {
                    //alert(result);
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
                    console.log(WoSoRel);
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
                    //--
                    window.locationre = result.url;
                    loadWO();
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

    $("#btnGW").on("click", function () {
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
                partType: 0,
                partlevel: ' ',
                calcWOQty: parseInt($(row).find("td:eq(8)").text()),
                planCompletionDate: $(row).find("td:eq(12)").text()
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

        console.log(selectedRowsData);

        if (selectedRowsData.length === 1) {
            // Single checkbox selected, post to WOpost
            api.post("/businessaquisition/WOpost", selectedRowsData[0]).then((data) => {
                // Handle success if needed
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
                console.log(WoSoRel);
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
                console.log(WoSOMethod);
            }).catch((error) => {
                AppUtil.HandleError("WOForm", error);
            });

        }

    });

    $('#routing').on('change', (e) => {
                const routeId = $(e.target).val();
                // make an API call to get data for select2 based on the selected studentId
                api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
                    console.log(data);
                    const selectElement = $('#StartingOpNo');
                    const selectEndOpNo = $('#EndingOpNo');
                    $.each(data, (index, item) => {
                        selectElement.html("");
                        selectElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
                    });
                    const reversedData = data.slice().reverse();
                    selectEndOpNo.html('');
                    $.each(reversedData, (index, item) => {
                        selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
                    });
                }).catch((error) => {
                    console.error(error);
                });
    });       

    //---Filtering---
    $("#SearchSO").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#SearchCustomer").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#SearchPO").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[6]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#SearchSoDateFr, #SearchSoDateTo").on("change", function () {
        var fromDate = $("#SearchSoDateFr").val().split("/").reverse().join("-");
        var toDate = $("#SearchSoDateTo").val().split("/").reverse().join("-");

        $("#SalesOrders1 tbody tr").filter(function () {
            var dateText = $(this.children[11]).text(); // assuming the date is in the 3rd column
            var tableDate = dateText.split("-").reverse().join("-");

            $(this).toggle(tableDate >= fromDate && tableDate <= toDate);
        });
    });

    $("#btnSearchPartNo").on("click", function () {
        var value = $("#SearchPartNo").val().toLowerCase();
        $("#SalesOrders1 tbody tr").filter(function () {
            $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    //gwo-
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
                    woid: parseInt(woid),
                    salesOrderId: parseInt(soid),
                    wonumber: wonumber,
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

                
                api.post("/businessaquisition/WOpost", selectedData).then((data) => {
                    // Handle success if needed
                    if (planwoqty < soqty) {
                        var balqty = soqty - planwoqty;
                        //var soupdate = {
                        //    salesOrderId: parseInt(soid),
                        //    requiredByDate: reqdate,
                        //    balanceSOQty: parseInt(balqty)
                        //};
                        var selectedRowsData = [];
                        var checkboxes = $("#multipleSO tbody input[type='checkbox']:checked");
                        checkboxes.each(function (index, checkbox) {
                            var row = checkbox.parentNode.parentNode;
                            var salesOrderId = parseInt($(row).find("td:eq(0)").text()); // get the text from the 3rd column (index 2)
                            //var balanceSOQty = parseInt($(row).find("td:eq(4)").text()); // get the text from the 5th column (index 4)
                            selectedRowsData.push({ salesOrderId });
                        });
                        console.log(selectedRowsData);
                        var norow = selectedRowsData.length;
                        for (var i = 0; i < selectedRowsData.length; i++) {
                            var edata = selectedRowsData[i];
                            if (norow > 1) {
                                var fBalqty = balqty / norow
                                edata = {
                                    ...selectedRowsData[i], 
                                    balanceSOQty: fBalqty
                                };
                            }
                            api.post("/businessaquisition/SalesOrder", edata)
                                .then((data) => {
                                    console.log("Data posted successfully!", data);
                                })
                                .catch((error) => {
                                    console.error("Error posting data:", error);
                                });
                        }
                        

                        
                    }
                    else {
                        var balqty = soqty - planwoqty
                        var soupdate = {
                            salesOrderId: parseInt(soid),
                            requiredByDate: reqdate,
                            balanceSOQty: parseInt(balqty)
                        }
                        api.post("/businessaquisition/SalesOrder", soupdate).then((data) => {

                        }).catch((error) => { });
                    }
                    $("#btnPop1Close").click();
                }).catch((error) => {
                    AppUtil.HandleError("WOForm", error);
                    console.log(error);
                });
            } else {
                alert("Please Select atleast one!");
            }
        }
       
    });
    $('#woc-partno').on('hidden.bs.modal', function (event) {
        loadWO();
    });
    // Event listener for when the modal is shown
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
        $('#p2WOid').val('');
        $('#p2SalesOrderId').val('');
        $('#p2PartId').val('');
        $('#dispatchDate').val('');
        $('#equaldiv').hide();
        loadWO();
        noofWOCreation = [];
        console.log(noofWOCreation);
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
        var WoComplDate = new Date(Date.parse($('#p2PlanComplDate').text()));
        var Compldt = $('#p2PlanComplDate').text();
        var partNo = $('#p2PartNo').text();
        var formattedDate = WoComplDate.toISOString();
        var requestInProgress = false;
        let totalQuantity = 0;
        var balsoqnty = $('#p2BalQty').val();
        var qntyOnhand = $('#p2QtyOnHand').val();
        var balmanuf = balsoqnty - qntyOnhand;
        $('#p2BaltoManuf').val(balmanuf);
        //document.querySelectorAll('#MulitpleWOs tbody tr').forEach(row => {
        //    const quantity = parseInt(row.cells[1].textContent);
        //    if (!isNaN(quantity)) {
        //        totalQuantity += quantity;
        //    }
        //});
        $('#popup2Sum').val(planwoqty);

        var sonumber = "";

        api.get("/WorkOrder/GetSoNumber?soid=" + soid).then(async (data) => {
            console.log(data);
            sonumber = await data.soNumber;
            $('#p2soNumber').text(sonumber);
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
                        console.log(data);
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
                        console.log(data);
                        noofWOCreation.push(...data);
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
                        console.log(data);
                        noofWOCreation.push(...data);
                    });
                }

            } else {

            }
        });

        $("#MultipleWo").on("click", function () {
            //if (requestInProgress) return;
            //requestInProgress = true;
            noofWOCreation.forEach((wo) => {
                wo.partId = parseInt(partid);
                wo.salesOrderId = parseInt(soid);
                wo.reloadOption = "EQD";
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
                        console.log(result);
                        tdata.push(...result);
                        var tablebody = $("#MulitpleWOs tbody");
                        $(tablebody).html("");//empty tbody

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
                                requestInProgress = false;
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
                            reloadOption: "inactive",
                            status: parseInt(1),
                            active: 2
                        };
                        api.post("/businessaquisition/WOpost", woinactive).then((data) => {
                            console.log(data);
                            //$('#popup3').modal('hide');
                            //reloadWO(reloadOption, partid);
                            loadWO();
                        }).catch((error) => {
                        });

                    }
                });
            }


        });

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
        var sonumber = $("#p2soNumber").text();
        $("#p3soNumber").text(sonumber);
        $("#p3partNo").text(partno);
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

        if (partType === 1) {
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + partId).then((data) => {
                console.log(data);
                const selectElement = $('#singleRouting');
                selectElement.prop("disabled", false);
                $.each(data, (index, item) => {
                    selectElement.html("");
                    selectElement.append(`<option value="0">--Select--</option>`);
                    selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);
                });
            }).catch((error) => {
            });
            $('#singleStartOpNo').prop("disabled", false);
            $('#singleEndOpNo').prop("disabled", false);
        }
        else {
            const selectElement = $('#singleRouting');
            selectElement.html("");
            selectElement.prop("disabled", true);
            $('#singleStartOpNo').html("").prop("disabled", true);
            $('#singleEndOpNo').html("").prop("disabled", true);
        }
        


    });

    $('#singleRouting').on('change', (e) => {
        const routeId = $(e.target).val();
        // make an API call to get data for select2 based on the selected studentId
        api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
            console.log(data);
            const selectElement = $('#singleStartOpNo');
            const selectEndOpNo = $('#singleEndOpNo');
            $.each(data, (index, item) => {
                selectElement.html("");
                selectElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
            });
            const reversedData = data.slice().reverse();
            selectEndOpNo.html('');
            $.each(reversedData, (index, item) => {
                selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
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
        var rstDt = new Date(Date.parse($('#woCompletedBy').text()));
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
            console.log(data);
            $('#popup3').modal('hide');
            reloadWO(reloadOption, partid);
        }).catch((error) => {
        });

    });


    $('#popup3ManualMultiple').on('shown.bs.modal', function (event) {
        var balSoqnty = $('#ManualBalSoQty').val();
        var QntyOnhand = $('#ManualQtyOnHand').val();
        var balmanuf = balSoqnty - QntyOnhand;
        $('#ManualBalToManuf').val(balmanuf);
    });

    $('#popup3ManualMultiple').on('hidden.bs.modal', function (event) {
        var $modal = $(this);
        //$modal.find('button[id=ManualSaveWo]').unbind('click');
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
            woid: parseInt(woid),
            salesOrderId: parseInt(soid),
            wonumber: wonumber,
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
            console.log(data);
            resultData.push(data);
            $('#popup3ManualMultiple').modal('hide');
            var tablebody = $("#MulitpleWOs tbody");
            $(tablebody).html("");//empty tbody

            for (i = 0; i < resultData.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", resultData[i]));
            }
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
        var formattedDate = WoComplDate.toISOString();
        var balsoqnty = $('#p2BalQty').val();
        var qntyOnhand = $('#p2QtyOnHand').val();

        $('#popup3NewWo').modal('show');
        $("#NewpartNo").text(partNo);
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
        var rowData = {
            woid: parseInt(woid),
            salesOrderId: parseInt(soid),
            wonumber: wonumber,
            partId: parseInt(partid),
            partType: parseInt(parttype),
            parentlevel: '',
            calcWOQty: parseInt(planWoQty),
            planCompletionDate: formattedDate,
            reloadOption: "New",
            routingId: parseInt(routingid),
            startingOpNo: parseInt(startingOpNo),
            endingOpNo: parseInt(endingOpNo),
            status: parseInt(wostatus)
        };

        api.post("/businessaquisition/WOpost", rowData).then((data) => {
            console.log(data);
            resultData.push(data);
            $('#popup3NewWo').modal('hide');
            var tablebody = $("#MulitpleWOs tbody");
            $(tablebody).html("");//empty tbody

            for (i = 0; i < resultData.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("MultipleWoRow", resultData[i]));
            }
        }).catch((error) => {
        });

    });

    $('#popup3NewWo').on('shown.bs.modal', function (event) {
        var balsoq = $("#NewBalSoQty").val();
        var qntonhand = $("#NewQtyOnHand").val();
        var balmanuf = balsoq - qntonhand;
        $("#NewBalToManuf").val(balmanuf);

    });
    $('#popup7').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var workOrderId = relatedTarget.data("workorderid");
        var salesOrderId = relatedTarget.data("salesorderid");
        var woNumber = relatedTarget.data("wonumber");
        var partNo = relatedTarget.data("partno");
        var planCompletionDateStr = relatedTarget.data("plancompletiondatestr");
        var partId = relatedTarget.data("partid");
        var partType = relatedTarget.data("parttype");
        var wostatus = relatedTarget.data("wostatus");
        var planWOQty = relatedTarget.data("calwoqty");
        var formattedDate = planCompletionDateStr.split("-").reverse().join("-");

        $("#popup7PartNo").text(partNo);
        $("#popup7PartNoField").val(partNo);
        $("#popup7WoComplDt").val(formattedDate);
        $("#popup7WoQnty").val(planWOQty);
        $("#popup7SoQnty").val(planWOQty);
        $("#Popup7woid").val(workOrderId);
        $("#Popup7soid").val(salesOrderId);
        $("#Popup7partId").val(partId);
        $("#Popup7partType").val(partType);
        $("#Popup7WoStatus").val(wostatus);
        $("#popup7WoNumber").text(woNumber);


        if (partType === 1) {
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + partId).then((data) => {
                console.log(data);
                const selectElement = $('#popup7Routing');
                selectElement.prop("disabled", false);
                $.each(data, (index, item) => {
                    selectElement.html("");
                    selectElement.append(`<option value="0">--Select--</option>`);
                    selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);
                });
            }).catch((error) => {
            });
            $('#popup7StartingOpNo').prop("disabled", false);
            $('#popup7EndingOpNo').prop("disabled", false);
        }
        else {
            const selectElement = $('#popup7Routing');
            selectElement.html("");
            selectElement.prop("disabled", true);
            $('#popup7StartingOpNo').html("").prop("disabled", true);
            $('#popup7EndingOpNo').html("").prop("disabled", true);
        }


    });

    $('#popup7Routing').on('change', (e) => {
        const routeId = $(e.target).val();
        // make an API call to get data for select2 based on the selected studentId
        api.getbulk("/WorkOrder/RoutingSteps?routingId=" + routeId).then((data) => {
            console.log(data);
            const selectElement = $('#popup7StartingOpNo');
            const selectEndOpNo = $('#popup7EndingOpNo');
            $.each(data, (index, item) => {
                selectElement.html("");
                selectElement.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
            });
            const reversedData = data.slice().reverse();
            selectEndOpNo.html('');
            $.each(reversedData, (index, item) => {
                selectEndOpNo.append(`<option value="${item.stepOperation}">${item.stepOperation}</option>`);
            });
        }).catch((error) => {
            console.error(error);
        });
    });

    $("#popup7SaveWo").on("click", function () {
        var woid = parseInt($("#Popup7woid").val());
        var soid = parseInt($("#Popup7soid").val());
        var partid = parseInt($("#Popup7partId").val());
        var parttype = parseInt($("#Popup7partType").val());
        var wostatus = parseInt($("#Popup7WoStatus").val());
        var planWoQty = parseInt($("#popup7WoQnty").val());
        //var soqty = parseInt($("#NewTotalSoQty").val());
        var wonumber = $("#popup7WoNumber").text();
        var WoComplDate = new Date(Date.parse($('#popup7WoComplDt').val()));
        var formattedDate = WoComplDate.toISOString();
        var routingid = $("#popup7Routing").val();
        var startingOpNo = $("#popup7StartingOpNo").val();
        var endingOpNo = $("#popup7EndingOpNo").val();
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
            console.log(data);
            $('#popup7').modal('hide');
            loadWO();
        }).catch((error) => {
        });
    });


});


function EditWo(element) {
    console.log("--Edit--");
    var relatedTarget = $(element);
    var workOrderId = relatedTarget.data("workorderid");
    var salesOrderId = relatedTarget.data("salesorderid");
    var woNumber = relatedTarget.data("wonumber");
    var partNo = relatedTarget.data("partno");
    var planCompletionDateStr = relatedTarget.data("plancompletiondatestr");
    var partId = relatedTarget.data("partid");
    var partType = relatedTarget.data("parttype");
    var planWOQty = relatedTarget.data("calwoqty");
    var woNumber = relatedTarget.data("wonumber");
    var wostatus = relatedTarget.data("wostatus");
    document.getElementById('WoComplDate').value = planCompletionDateStr.split("-").reverse().join("-");
    document.getElementById('p1planComplDate').value = planCompletionDateStr.split("-").reverse().join("-");
    $('#PlanWoQty').val(0);
    $('#SoQty').val(planWOQty);
    $('#woNumber').text(woNumber);
    $('#partNo').text(partNo);
    $('#WOID').val(workOrderId);
    $('#SalesOrderId').val(salesOrderId);
    $('#PartId').val(partId);
    $('#woStatus').val(wostatus);

    var WOSoTable = [];
    var temp = [];
    if (workOrderId > 0) {
        
        api.getbulk("/WorkOrder/GetSoWo?workOrderId=" + workOrderId).then((data) => {
            console.log(data);
            $.each(data, (index, item) => {
                var rowdata = {
                    wosoId: parseInt(item.wosoId),
                    workOrderId: parseInt(item.workOrderId),
                    salesOrderId: parseInt(item.salesOrderId),

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
                    console.log(result);
                    $.each(result, (index, item) => {
                        WOSoTable.push(item);
                    });

                    //CheckNumberOfSo(WOSoTable);     
                    if (WOSoTable.length >= 2) {
                        $('#woc-partno').modal('show');
                        $('#popup2').modal('hide');
                        var tablebody = $("#multipleSO tbody");
                        $(tablebody).html("");//empty tbody

                        for (i = 0; i < WOSoTable.length; i++) {
                            $(tablebody).append(AppUtil.ProcessTemplateData("MultipleSoRow", WOSoTable[i]));
                        }


                        if (partType === 1) {
                            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + partId).then((data) => {
                                console.log(data);
                                const selectElement = $('#routing');
                                selectElement.prop("disabled", false);
                                $.each(data, (index, item) => {
                                    selectElement.html("");
                                    selectElement.append(`<option value="0">--Select--</option>`);
                                    selectElement.append(`<option value="${item.routingId}">${item.routingName}</option>`);
                                });
                            }).catch((error) => {
                            });
                            $('#StartingOpNo').prop("disabled", false);
                            $('#EndingOpNo').prop("disabled", false);
                        }
                        else {
                            const selectElement = $('#routing');
                            selectElement.html("");
                            selectElement.prop("disabled", true);
                            $('#StartingOpNo').html("").prop("disabled", true);
                            $('#EndingOpNo').html("").prop("disabled", true);
                        }
                    }
                    else if (WOSoTable.length === 1) {
                        $('#popup2').modal('show');
                        $('#p2totalSoQty').val(planWOQty);
                        $('#p2BalQty').val(0);
                        $('#p2WoNumber').text(woNumber);
                        $('#p2PartNo').text(partNo);
                        $('#p2WOid').val(workOrderId);
                        $('#p2SalesOrderId').val(salesOrderId);
                        $('#p2PartId').val(partId);
                        $('#p2PartType').val(partType);
                        $('#p2Status').val(wostatus);
                        $('#p2PlanComplDate').text(planCompletionDateStr.split("-").reverse().join("-"));
                        //popup2show = true;
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
    
    for (i = 0; i < rowData.length; i++) {
        $(tablebody).append(AppUtil.ProcessTemplateData("MultipleSoRow", rowData[i]));
    }
}