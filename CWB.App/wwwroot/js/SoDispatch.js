let g_selectedDispatchItems = [];
var P12Data = [];
var P12RedistributionRequired = false;
var P12SelectedSoAllocId = 0;
function loadSoDispatch() {
    $("#preloaderblurred").show();
    $('#SelectAllMGrid').prop("checked", false);
    api.getbulk("/WorkOrder/AllSODispatch").then((data) => {
      //  data = data.filter(item => item.status === 1 || item.status === 2 );
        data = data.filter(item => item.qntyOnHand > 0 );
        data.sort((a, b) => {
            // Convert the date strings to Date objects for comparison
            const dateA = new Date(a.requiredByDate);
            const dateB = new Date(b.requiredByDate);

            // Compare: dateA - dateB sorts ascending (earliest first)
            return dateA - dateB;
        });
        var tablebody = $("#SOGrid tbody");
        $(tablebody).html("");
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
        console.log(data);
        for (i = 0; i < data.length; i++) {
            //if (data[i].invoiceNo.length > 0 && data[i].qntyOnHand === 0 ) {
            //    continue;
            // }
           
            $(tablebody).append(AppUtil.ProcessTemplateData("SOGridRow", data[i]));
        }
        //if (g_selectedDispatchItems.length > 0) {

        //    // Create a temporary lookup map of the fresh server data, using the SO ID as the key
        //    const freshDataMap = new Map(data.map(item => [item.salesOrderId, item]));

        //    // Filter the global array and replace old data with fresh data
        //    g_selectedDispatchItems = g_selectedDispatchItems.map(oldItem => {
        //        const freshItem = freshDataMap.get(parseInt(oldItem.soId));
        //        oldItem.deispatchId = freshItem.dispatchId;
        //        oldItem.invoiceNo = freshItem.invoiceNo;
        //        oldItem.invoiceDate = freshItem.invoiceDate;
        //        oldItem.dispatchDetail = freshItem.dispatchDetail;
        //        // If the item still exists in the fresh server data, return the fresh data
        //        // Otherwise, return null to be filtered out later.
        //        return oldItem;

        //    }).filter(item => item !== null); // Remove items that no longer exist/qualify

        //    console.log("g_selectedDispatchItems synchronized with latest server data.");
        //    rebindP13Grid();
        //}
        updateSODispatchActions();


        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}

function updateSODispatchActions() {

    $("#SOGrid tbody tr").each(function () {

        var row = $(this);

        var finalDispQnty = parseFloat(
            row.find(".remove-suggested-qnty").attr("data-findisqnty")
        ) || 0;

        var checkbox = row.find(".SOgridChks");

        var removeSuggested = row.find(".remove-suggested-qnty");
        var removeFinal = row.find(".remove-final-disp-qnty");


        if (finalDispQnty === 0) {

            // Final Dispatch Quantity is 0
            // Disable SO checkbox
            checkbox.prop("disabled", true);

            // Show Remove Suggested Quantity
            removeSuggested.show();

            // Hide Remove Final Dispatch Quantity
            removeFinal.hide();

        } else {

            // Final Dispatch Quantity is greater than 0
            // Enable SO checkbox
            checkbox.prop("disabled", false);

            // Hide Remove Suggested Quantity
            removeSuggested.hide();

            // Show Remove Final Dispatch Quantity
            removeFinal.show();
        }
    });
}

















function LoadCustDesc(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    if (!compSelect.length)
        return;
    compSelect.empty();
    ////debugger;
    var div_data = "<option value=''>--Select--</option>";
    compSelect.append(div_data);
    api.get("/masters/customers").then((data) => {
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].companyId + "'>" +
                data[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);P14Save
    });
}
/**
 * Re-renders the P13Grid summary table using the data currently stored 
 * in the global g_selectedDispatchItems variable.
 */
function rebindP13Grid() {
    var tablebody = $("#P13Grid tbody");
    tablebody.html(""); // Clear existing rows

    if (g_selectedDispatchItems.length === 0) {
        // Optionally show a message if all items were removed/processed
        tablebody.append('<tr><td colspan="8">No parts remaining in dispatch selection.</td></tr>');
        $("#popup13").modal("hide"); // Hide the modal if empty
        return;
    }

    // Loop through the current global data and re-render the rows
    for (i = 0; i < g_selectedDispatchItems.length; i++) {
        if (g_selectedDispatchItems[i].invoiceNo.length > 0) {
            continue;
        }
        var group = g_selectedDispatchItems[i];
        const row = `
            <tr>
                <td>${group.salesOrderNumber}</td>
                <td>${group.customer}</td>
                <td>${group.poNo}</td>
                <td>${group.finalDispQnty}</td>
                <td>${group.invoiceNo}</td>
                <td>${group.invoiceDate}</td>
                <td>${group.dispatchDetail}</td>
                <td>
                    <div class="dropdown float-center">
                        <a href="#"
                            class="dropdown-toggle arrow-none card-drop"
                            data-bs-toggle="dropdown"
                            aria-expanded="false">
                            <i class="mdi mdi-dots-vertical"></i>
                        </a>
                        <div class="dropdown-menu dropdown-menu-end">
                            <a href="javascript:void(0);" 
                               class="dropdown-item" data-bs-toggle="modal" data-finaldispqnyt="${group.finalDispQnty}" data-pono="${group.poNo}" data-custid="${group.customerOrderId || ''}" 
                               data-customer="${group.customer}" data-sono="${group.salesOrderNumber}" data-invoicedate="${group.invoiceDate}" data-invoiceno="${group.invoiceNo}" data-partid="${group.partId}"
                               data-partno="${group.partNoDesc}" data-soid="${group.soId}" data-dispatchfid="${group.deispatchId}" data-dispatchdetail="${group.dispatchDetail}"
                               data-bs-target="#popup14">Edit</a>
                        </div>
                    </div>
                </td>
            </tr>
        `;
        tablebody.append(row);
    }
}

$(document).ready(function () {
    //$('#SelectAllMGrid').on('change', function () {
    //    var isChecked = $(this).prop('checked');

    //    // 2. Select all row checkboxes within the same table body
    //    // and set their 'checked' property to match the header checkbox.
    //    // We use the table ID (#SOGrid) and the row checkbox class (.SOgridChks) 
    //    // to ensure we only target the correct checkboxes.
    //    $('#SOGrid tbody input.SOgridChks').prop('checked', isChecked);
    //});

    //// OPTIONAL: Add functionality to uncheck the header checkbox 
    //// if a *single* row checkbox is manually unchecked.
    //$('#SOGrid tbody input.SOgridChks').on('change', function () {
    //    var allChecked = true;

    //    // Check if ALL row checkboxes are currently checked
    //    $('#SOGrid tbody input.SOgridChks').each(function () {
    //        if (!$(this).prop('checked')) {
    //            allChecked = false;
    //            return false; // Exit the loop early
    //        }
    //    });

    //    // Update the state of the "Select All" checkbox
    //    $('#SelectAllMGrid').prop('checked', allChecked);
    //});
    $('#SelectAllMGrid').on('change', function () {

        var isChecked = $(this).prop('checked');

        $('#SOGrid tbody input.SOgridChks:not(:disabled)')
            .prop('checked', isChecked);
    });


    $('#SOGrid tbody').on(
        'change',
        'input.SOgridChks:not(:disabled)',
        function () {

            var enabledCheckboxes =
                $('#SOGrid tbody input.SOgridChks:not(:disabled)');

            var allChecked =
                enabledCheckboxes.length > 0 &&
                enabledCheckboxes.filter(':checked').length ===
                enabledCheckboxes.length;

            $('#SelectAllMGrid').prop('checked', allChecked);
        }
    );
    loadSoDispatch();
    LoadCustDesc("SearchCustomer");
    LoadCustDesc("P13SearchCustomer");
    $('#popup14').on('hidden.bs.modal', function (event) {
        document.getElementById('popup13').style.filter = 'none';
        var P14InvNo = document.getElementById('P14InvNo');
        P14InvNo.style.border = '';

        var P14InvDate = document.getElementById('P14InvDate');
        P14InvDate.style.border = '';

    });
    $('#popup14').on('show.bs.modal', function (event) {
        document.getElementById('popup13').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var finaldispqnyt = relatedTarget.data("finaldispqnyt");
        var pono = relatedTarget.data("pono");
        var customer = relatedTarget.data("customer");
        var sono = relatedTarget.data("sono");
        var partno = relatedTarget.data("partno");
        var soid = relatedTarget.data("soid");
        var custid = relatedTarget.data("custid");
        var dispatchfid = relatedTarget.data("dispatchfid");
        var invoicedate = relatedTarget.data("invoicedate");
        var invoiceno = relatedTarget.data("invoiceno");
        var dispatchDetail = relatedTarget.data("dispatchdetail");
        var partId = relatedTarget.data("partid");
        $("#P14Cust").text(customer);
        $("#P14Po").text(pono);
        $("#P14NoOfParts").text(finaldispqnyt);
        $("#P14CustId").val(custid);
        $("#P14SoidId").val(soid);
        $("#P14DetailsId").val(dispatchfid);
        $("#P14InvNo").val(invoiceno);
        const originalDate = invoicedate; // e.g., "31-10-2025"

        if (originalDate) {
            // 2. Split the string by the hyphen (-)
            const parts = originalDate.split('-'); // parts will be ["31", "10", "2025"]

            // Check if the split operation resulted in the expected 3 parts
            if (parts.length === 3) {
                // 3. Reorder the parts to YYYY-MM-DD format
                const formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`; // "2025-10-31"

                // 4. Set the value using the correctly formatted string
                $("#P14InvDate").val(formattedDate);
            } else {
                console.error(`Invalid date format received: ${originalDate}. Expected DD-MM-YYYY.`);
                $("#P14InvDate").val(''); // Clear the field if the format is invalid
            }
        } else {
            // Handle cases where invoicedate might be null or empty
            $("#P14InvDate").val('');
        }
        $("#P14DisDetails").val(dispatchDetail);
        $("#P14partId").val(partId);
        var tablebody = $("#P14Grid tbody");
        $(tablebody).html("");
        const row = `
            <tr>
                <td>${partno}</td>
                <td>${finaldispqnyt}</td>
            </tr>
        `;
        $(tablebody).append(row);
    });
    $('#popup13').on('show.bs.modal', function (event) {

    });
    $('#popup12').on('show.bs.modal', function (event) {
       
        $("#P12FinQntyError").hide();
        $("#P12FinQnty").css("border", "");
        var P12FinQnty = document.getElementById('P12FinQnty');
        P12FinQnty.style.border = '';
        var relatedTarget = $(event.relatedTarget);
        var partn = relatedTarget.data("partn");
        var partdesc = relatedTarget.data("partdesc");
        var customer = relatedTarget.data("customer");
        var pono = relatedTarget.data("pono");
        var baldispqnty = relatedTarget.data("baldispqnty");
        var soid = relatedTarget.data("soid");
        var findisqnty = relatedTarget.data("findisqnty");
        var qoh = relatedTarget.data("qoh");
        var suggestedqty = relatedTarget.data("suggested");
        var soallocationId = relatedTarget.data("soallocid");
        $("#P12PartNo").text(partn);
        $("#P12PartDesc").text(partdesc);
        $("#P12Po").text(pono);
        $("#P12Cust").text(customer);
        $("#P12BalQnty").text(baldispqnty);
        $("#P12QoH").text(qoh);
        $("#P12SoId").val(soid);
        if (findisqnty > 0) {
            $("#P12FinQnty").val(findisqnty);
        }
        else {
            $("#P12FinQnty").val(suggestedqty);
        }
        
        $("#P12SuggestedQnty").text(suggestedqty);
        $("#P12SoAllocId").val(soallocationId);
        
        $("#preloaderblurred").show();
        $('#SelectAllMGrid').prop("checked", false);
        //api.getbulk("/WorkOrder/AllSODispatch").then((data) => {
        //   // data = data.filter(item => item.status === 1 || item.status === 2);
        //    data = data.filter(item => item.qntyOnHand > 0);
        //    data = data.filter(item => item.partNo === partn);
        //    data.sort((a, b) => {
        //        // Convert the date strings to Date objects for comparison
        //        const dateA = new Date(a.requiredByDate);
        //        const dateB = new Date(b.requiredByDate);

        //        // Compare: dateA - dateB sorts ascending (earliest first)
        //        return dateA - dateB;
        //    });
        //    var tablebody = $("#P12Grid tbody");
        //    $(tablebody).html("");
        //    if (data.length <= 1) {
        //        $("#P12GridDiv").hide();
        //    } else {
        //        $("#P12FinQnty").prop("disabled", true);
        //        $("#P12Save").prop("disabled", true);
        //        $("#P12GridDiv").show();
        //    }
        //    for (i = 0; i < data.length; i++) {
        //        $(tablebody).append(AppUtil.ProcessTemplateData("P12GridRow", data[i]));
        //    }
        //    $("#preloaderblurred").hide();
        //}).catch((error) => {
        //    $("#preloaderblurred").hide();
        //});
        api.getbulk("/WorkOrder/AllSODispatch").then((data) => {

            // 1. Filter records for this Part No
            data = data.filter(item => item.qntyOnHand > 0);
            data = data.filter(item => item.partNo === partn);

            // 2. Sort by Required By Date
            data.sort((a, b) => {
                const dateA = new Date(a.requiredByDate);
                const dateB = new Date(b.requiredByDate);
                return dateA - dateB;
            });

            // 3. Store fresh Popup 12 data
            P12Data = data;

            // 4. Reset redistribution flag
            P12RedistributionRequired = false;

            // 5. Determine whether redistribution is required
            if (P12Data.length > 1) {

                var allLinesHaveFullCoverage = P12Data.every(function (item) {

                    var balanceQty = parseInt(item.balanceSOQty) || 0;
                    var plannedQty = parseInt(item.suggestedDispQnty) || 0;

                    return balanceQty === plannedQty;
                });

                // If even one line does not have full coverage
                if (!allLinesHaveFullCoverage) {
                    P12RedistributionRequired = true;
                }
            }

            console.log("P12Data:", P12Data);
            console.log(
                "P12RedistributionRequired:",
                P12RedistributionRequired
            );

            // 6. Populate Popup 12 grid
            var tablebody = $("#P12Grid tbody");
            $(tablebody).html("");

            if (P12Data.length <= 1) {

                // Single SO/PO -> no redistribution
                $("#P12GridDiv").hide();

            } else {

                // Multiple SO/PO -> show grid
                $("#P12GridDiv").show();

                // Do not allow direct editing until Edit is clicked
                $("#P12FinQnty").prop("disabled", true);
                $("#P12Save").prop("disabled", true);
            }

            // 7. Bind grid
            for (var i = 0; i < P12Data.length; i++) {

                $(tablebody).append(
                    AppUtil.ProcessTemplateData(
                        "P12GridRow",
                        P12Data[i]
                    )
                );
            }
            calculateP12Redistribution();
            $("#preloaderblurred").hide();

        }).catch((error) => {

            $("#preloaderblurred").hide();

            console.log(error);
        });
    });
   
    $("#SearchSo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SOGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#SOGrid tbody");
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
    $("#SearchPO").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SOGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#SOGrid tbody");
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
    $("#P13SearchSo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P13Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P13Grid tbody");
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
    $("#P13SearchPO").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P13Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P13Grid tbody");
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
    $("#P13Partno").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P13Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P13Grid tbody");
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
    $("#P13SearchCustomer").on("change", function () {
        var selectedValue = $(this).find('option:selected').text();
        if (selectedValue == "--Select--") {
            $("#P13Grid tbody tr").show();
            var $tableBody = $("#P13Grid tbody");
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
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P13Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P13Grid tbody");
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
        }
    });
    $("#SearchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#SOGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#SOGrid tbody");
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
    $("#SearchCustomer").on("change", function () {
        var selectedValue = $(this).find('option:selected').text();
        if (selectedValue == "--Select--") {
            $("#SOGrid tbody tr").show();
            var $tableBody = $("#SOGrid tbody");
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
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#SOGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#SOGrid tbody");
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
        }
    });

    $("#DispatchSelectedParts").on('click', function (event) {
        const selectedCheckboxes = document.querySelectorAll('#SOGrid tbody .SOgridChks:checked');
        if (selectedCheckboxes.length === 0) {
            alert("Please select at least one Sales Order for dispatch.");
            console.warn("No parts selected for dispatch. Please check at least one row.");
            $("#popup13").modal("hide");
            return;
        }
        // 2. Process each selected row to extract all column data
        const dispatchData = Array.from(selectedCheckboxes).map(checkbox => {
            const row = checkbox.closest('tr');
            if (!row) return null;

            // Get all cells (th and td) in the row, excluding the checkbox column (index 0)
            const cells = row.querySelectorAll('th, td');


            // Extract the data from the cells
            return {
                soId: row.getAttribute('data-so-id'), // Primary identifier
                soAllocListId: row.getAttribute('data-soallocid'),
                salesOrderNumber: cells[1] ? cells[1].textContent.trim() : '',
                customer: cells[2] ? cells[2].textContent.trim() : '',
                poNo: cells[3] ? cells[3].textContent.trim() : '',
                partNoDesc: cells[4] ? cells[4].textContent.trim() : '',
                totalSoQnty: cells[5] ? cells[5].textContent.trim() : '',
                balSoDispQnty: cells[6] ? cells[6].textContent.trim() : '',
                qntyOnHand: cells[7] ? cells[7].textContent.trim() : '',
                suggestedDispQnty: cells[8] ? cells[8].textContent.trim() : '',
                finalDispQnty: cells[9] ? cells[9].textContent.trim() : '', // Assuming this will be the input value
                soPlanDt: cells[10] ? cells[10].textContent.trim() : '',
                deispatchId: cells[11] ? cells[11].textContent.trim() : '',
                invoiceNo: cells[12] ? cells[12].textContent.trim() : '',
                invoiceDate: cells[13] ? cells[13].textContent.trim() : '',
                dispatchDetail: cells[14] ? cells[14].textContent.trim() : '',
                partId: cells[15] ? cells[15].textContent.trim() : '',
            };
        }).filter(item => item !== null);
        // 3. Console log the collected full data
        console.log("Selected Parts Full Dispatch Data:", dispatchData);
        if (dispatchData.length > 0) {
            g_selectedDispatchItems = dispatchData;
            $("#popup13").modal("show");
            var tablebody = $("#P13Grid tbody");
            $(tablebody).html("");
            for (i = 0; i < dispatchData.length; i++) {
                var group = dispatchData[i];
                const row = `
            <tr data-soallocid="${group.soAllocListId}">
                <td>${group.salesOrderNumber}</td>
                <td>${group.customer}</td>
                <td>${group.poNo}</td>
                <td>${group.partNoDesc}</td>
                <td>${group.totalSoQnty}</td>
                <td>${group.balSoDispQnty}</td>
                <td>${group.finalDispQnty}</td>

            </tr>
        `;
                $(tablebody).append(row);
            }
        }
    });
    function getTodayISO() {
        const today = new Date();
        // Adjust for local timezone and get the ISO string (YYYY-MM-DDTHH:mm:ss.zzzZ)
        today.setMinutes(today.getMinutes() - today.getTimezoneOffset());
        // Slice to get only YYYY-MM-DD part
        return today.toISOString().slice(0, 10);
    }
    $("#P14Save").secureClick( function (event) {
        var P14InvNo=$("#P14InvNo").val();
        var P14DetailsId=$("#P14DetailsId").val();
        var P14SoidId = $("#P14SoidId").val();
        var P14partId = $("#P14partId").val();
        var P14CustId = $("#P14CustId").val();
        var P14InvDate = $("#P14InvDate").val();
        var P14DisDetails = $("#P14DisDetails").val();
        var P14NoOfParts = $("#P14NoOfParts").text();
        const todayISO = getTodayISO();
        if (P14InvNo.length === 0) {
            var newNamevalidate = document.getElementById('P14InvNo');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14InvNo');
            newNamevalidate.style.border = '';
        }
        if (P14InvDate.length === 0) {
            var newNamevalidate = document.getElementById('P14InvDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14InvDate');
            newNamevalidate.style.border = '';
        }
        if (P14InvDate > todayISO) {
            alert("Invoice Date cannot be a greater than today's date.");
            return false;
        }
        var formdata = {
            DispatchDetailsId: parseInt(P14DetailsId),
            SaleOrderId: parseInt(P14SoidId),
            CustomerId: parseInt(P14CustId),
            NoOfParts: P14NoOfParts,
            InvoiceNo: P14InvNo,
            InvoiceDate: P14InvDate,
            DispatchDetail: P14DisDetails,
            PartNoId: parseInt(P14partId),

        };
        return api.post("/WorkOrder/PostDispatchDetails", formdata).then((data) => {
            alert("Customer Dispatch Details Saved");
            $("#popup14").modal("hide");
            loadSoDispatch();
        }).catch((error) => {
            console.log(error);
        });
    });
    $("#P12Save").secureClick( function (event) {
        //var P12FinQnty = $("#P12FinQnty").val();
        var P12FinQnty = parseInt($("#P12FinQnty").val());
        //var P12BalQnty= parseInt($("#P12BalQnty").text());
        //var P12SoId = parseInt($("#P12SoId").val());
        var P12SoallocationId = parseInt($("#P12SoAllocId").val())
        //if (P12FinQnty === 0 || isNaN(P12FinQnty)) {
        //    var newNamevalidate = document.getElementById('P12FinQnty');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('P12FinQnty');
        //    newNamevalidate.style.border = '';
        //}
        //if (P12FinQnty > P12BalQnty) {
        //    alert("Qnty to Dispatch is greater than Bal Disp.Qnty.");
        //    return false;
        //}

        if (!validateP12Quantity()) {
            return false;
        }


        var formdata = {
            SO_Alloc_List_Id: P12SoallocationId,
            Final_Dispatch_Qnty: P12FinQnty,

        };
        return api.post("/WorkOrder/UpdateSOFinDisp", formdata).then((data) => {
            alert("Qnty to Dispatch Saved");
            $("#popup12").modal("hide");
            loadSoDispatch();
        }).catch((error) => {
            console.log(error);
        });
    });
    // --- Function to handle the bulk update for suggested quantities ---
    // --- Function to handle the bulk update for suggested quantities ---
    $("#AcptSuggested").secureClick( function (event) {
        // Show a preloader/loading indicator
        $("#preloaderblurred").show();

        // Prepare an array to hold the data for the bulk update
        var bulkUpdateData = [];

        // Iterate over all rows in the grid body
        $("#SOGrid tbody tr").each(function () {
            var row = $(this);

            // 1. Extract Sales Order ID from the 'data-so-id' attribute on the <tr>
            var soId = row.data('so-id');
            var soalloctionId = row.data('soallocid');
            // 2. Extract Suggested Quantity from the <th> with class 'suggested-qnty-value'
            var suggestedQntyText = row.find('.suggested-qnty-value').text().trim();
            var suggestedQnty = parseInt(suggestedQntyText);

            // Optional: Get the available quantity to ensure we don't dispatch more than what's on hand
            // Based on your template, {qntyOnHand} is the available quantity (assuming it's in the 7th <th> element)
            var qntyOnHandText = row.find('th:nth-child(7)').text().trim();
            var qntyOnHand = parseInt(qntyOnHandText);

            // Validation and data preparation
            if (soalloctionId && !isNaN(suggestedQnty) ) {

                // Check if suggested quantity exceeds quantity on hand (basic client-side check)
                if (suggestedQnty > qntyOnHand) {
                    console.warn(`Skipping SOallocationID ${soalloctionId}: Suggested Quantity (${suggestedQnty}) exceeds Qnty On Hand (${qntyOnHand}).`);
                    return true; // continue to the next iteration
                }

                bulkUpdateData.push({
                    SO_Alloc_List_Id: parseInt(soalloctionId),
                    Final_Dispatch_Qnty: suggestedQnty // Use the suggested quantity for the final dispatch
                });
            }
        });

        // 3. Check if there's any valid data to send
        if (bulkUpdateData.length === 0) {
            alert("No valid rows with suggested quantity found for dispatch. Please check quantities.");
            $("#preloaderblurred").hide();
            return;
        }

        // 4. Send the bulk data to the server
        // Using the NEW endpoint /WorkOrder/UpdateSOFinDispBulk as defined in the previous step
        return $.ajax({
            type: "POST",
            url: '/workOrder/UpdateSOFinDispBulk',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(bulkUpdateData),
            dataType: "json",
            success: function (result) {
                alert(`Successfully updated ${bulkUpdateData.length} sales orders with suggested quantity.`);
                loadSoDispatch();
                $("#preloaderblurred").hide();
            }
        });
        //api.post("/WorkOrder/UpdateSOFinDispBulk", JSON.stringify(bulkUpdateData)).then((data) => {
        //    alert(`Successfully updated ${bulkUpdateData.length} sales orders with suggested quantity.`);

        //    // 5. Refresh the grid and hide the preloader
        //    loadSoDispatch();
        //    // The preloader is hidden inside loadSoDispatch() on success/fail
        //}).catch((error) => {
        //    console.error("Bulk update failed:", error);
        //    alert("An error occurred during the bulk dispatch update. Check console for details.");
        //    $("#preloaderblurred").hide();
        //});
    });
});
function EditP12(element) {
    var relatedTarget = $(element);
    var partn = relatedTarget.data("partn");
    var partdesc = relatedTarget.data("partdesc");
    var customer = relatedTarget.data("customer");
    var pono = relatedTarget.data("pono");
    var baldispqnty = relatedTarget.data("baldispqnty");
    var soid = relatedTarget.data("soid");
    var findisqnty = relatedTarget.data("findisqnty");
    var qoh = relatedTarget.data("qoh");
    var suggestedqty = relatedTarget.data("suggested");
    var soallocationId = relatedTarget.data("soallocid");
    $("#P12PartNo").text(partn);
    $("#P12PartDesc").text(partdesc);
    $("#P12Po").text(pono);
    $("#P12Cust").text(customer);
    $("#P12BalQnty").text(baldispqnty);
    $("#P12QoH").text(qoh);
    $("#P12SoId").val(soid);
    if (findisqnty > 0) {
        $("#P12FinQnty").val(findisqnty);
    }
    else {
        $("#P12FinQnty").val(suggestedqty);
    }
    /*$("#P12SuggestedQnty").text(suggestedqty);*/
    $("#P12FinQnty").prop("disabled", false);
    $("#P12Save").prop("disabled", false);
    $("#P12SoAllocId").val(soallocationId);
    P12SelectedSoAllocId =  parseInt(soallocationId) || 0;
    validateP12Quantity();
}
$("#P13Save").on("click", function () {

    if (!g_selectedDispatchItems ||
        g_selectedDispatchItems.length === 0) {

        alert("No dispatch records selected.");
        return;
    }

    var selectedAllocations = g_selectedDispatchItems
        .map(function (item) {
            return {
                SO_Alloc_List_Id: parseInt(item.soAllocListId) || 0,
                SO_ID: parseInt(item.soId) || 0,
                PartId: parseInt(item.partId) || 0,
                Allocated_Qnty: parseInt(item.allocatedQnty) || 0,
                SuggestedDispQnty: parseInt(item.suggestedDispQnty) || 0,
                Final_Dispatch_Qnty: parseInt(item.finalDispatchQnty) || 0,
                Ready_for_Dispatch: item.readyForDispatch || 'N',
                Full_Qnty_Dispatched: item.fullQntyDispatched || 'N',
                Dispatch_Complete: item.dispatchComplete || 'N'
            };
        })
        .filter(function (item) {
            return item.SO_Alloc_List_Id > 0;
        });

    $("#P13Save").prop("disabled", true);
    $("#preloaderblurred").show();

    return $.ajax({
        type: "POST",
        url: "/WorkOrder/ConfirmDispatchList",
        contentType: "application/json; charset=utf-8",
        headers: {
            "Content-Type": "application/json"
        },
        data: JSON.stringify(selectedAllocations),
        dataType: "json",

        success: function (response) {

            console.log("Controller Response:", response);

            if (response.success) {
                alert(response.message);
                $("#popup13").modal("hide");
                loadSoDispatch();
            } else {
                alert(response.message || "Dispatch confirmation failed.");
            }
        },

        error: function (xhr) {

            console.error("P13 Save Error:", xhr);

            alert("An error occurred while confirming dispatch.");
        },

        complete: function () {
            $("#preloaderblurred").hide();
            $("#P13Save").prop("disabled", false);
        }
    });
});
var selectedSOAllocListId = 0;
var selectedSOAction = "";
$(document).on("click", ".remove-suggested-qnty", function () {

    var element = $(this);

    var soAllocListId =
        parseInt(element.attr("data-soallocid")) || 0;

    var partNo =
        element.attr("data-partn") || "";

    var soNumber =
        element.attr("data-sonumber") || "";

    var finalDispQnty =
        parseInt(element.attr("data-findisqnty")) || 0;


    // Validate SO Allocation ID
    if (soAllocListId <= 0) {

        alert("SO Allocation List ID is invalid.");

        return;
    }


    // Remove Suggested is allowed only when Final Disp Qnty = 0
    if (finalDispQnty !== 0) {

        alert(
            "Remove Suggested Qnty is allowed only when Final Disp Qnty is 0."
        );

        return;
    }


    // Store selected allocation
    selectedSOAllocListId = soAllocListId;

    // Store selected action
    selectedSOAction = "RemoveSuggested";


    // Set confirmation message
    $("#confirmSODispatchActionMessage").text(
        "Are you sure you want to remove the Suggested Dispatch Quantity?"
    );


    // Display information
    $("#confirmSODispatchPartNo").text(partNo);

    $("#confirmSODispatchSONumber").text(soNumber);

    $("#confirmSODispatchActionModal").modal("show");
    // Open Bootstrap 5 modal
   

});
$(document).on("click", ".remove-final-disp-qnty", function () {

    var element = $(this);

    var soAllocListId =
        parseInt(element.attr("data-soallocid")) || 0;

    var partNo =
        element.attr("data-partn") || "";

    var soNumber =
        element.attr("data-sonumber") || "";

    var finalDispQnty =
        parseInt(element.attr("data-findisqnty")) || 0;


    // Validate SO Allocation ID
    if (soAllocListId <= 0) {

        alert("SO Allocation List ID is invalid.");

        return;
    }


    // Remove Final is allowed only when Final Disp Qnty > 0
    if (finalDispQnty <= 0) {

        alert(
            "There is no Final Dispatch Quantity to remove."
        );

        return;
    }


    // Store selected allocation
    selectedSOAllocListId = soAllocListId;

    // Store selected action
    selectedSOAction = "RemoveFinal";


    // Set confirmation message
    $("#confirmSODispatchActionMessage").text(
        "Are you sure you want to reset the Final Dispatch Quantity to 0?"
    );


    // Display information
    $("#confirmSODispatchPartNo").text(partNo);

    $("#confirmSODispatchSONumber").text(soNumber);

    $("#confirmSODispatchActionModal").modal("show");
   

});
$(document).on("click", "#btnConfirmSODispatchAction", function () {

    // Validate allocation ID
    if (selectedSOAllocListId <= 0) {

        alert("Invalid SO Allocation List ID.");

        return;
    }


    // Validate action
    if (selectedSOAction === "") {

        alert("Invalid action.");

        return;
    }


    // Create your existing ViewModel
    var model = {
        SO_Alloc_List_Id: selectedSOAllocListId
    };


    // Decide which controller to call
    var url = "";

    if (selectedSOAction === "RemoveSuggested") {

        url = "/WorkOrder/RemoveSuggestedQnty";

    }
    else if (selectedSOAction === "RemoveFinal") {

        url = "/WorkOrder/RemoveFinalDispQnty";

    }
    else {

        alert("Invalid action.");

        return;
    }


    // Disable button to prevent double click
    var confirmButton =
        $("#btnConfirmSODispatchAction");

    confirmButton.prop("disabled", true);

    confirmButton.text("Processing...");


    $("#preloaderblurred").show();


    // POST to controller
    api.post(url, model)
        .then(function (response) {

            $("#preloaderblurred").hide();

            confirmButton.prop("disabled", false);

            confirmButton.text("Confirm");


            if (response.success) {

             
                $("#confirmSODispatchActionModal").modal("hide");

                // Clear selected values
                selectedSOAllocListId = 0;
                selectedSOAction = "";


                // Reload SO Dispatch grid
                loadSoDispatch();


                alert(response.message);

            }
            else {

                //alert(
                //    response.message ||
                //    "Operation failed."
                //);
            }

        })
        .catch(function (error) {

            $("#preloaderblurred").hide();

            confirmButton.prop("disabled", false);

            confirmButton.text("Confirm");

            console.error(error);

            alert(
                "An error occurred while processing the request."
            );
        });

});
$("#P12FinQnty").on("input", function () {
    if ($("#P12FinQnty").prop("disabled")) {
        return;
    }

    validateP12Quantity();
    //var value = $(this).val().trim();
    //var qtyToDispatch = parseInt(value);
    //var balDispQnty = parseInt($("#P12BalQnty").text()) || 0;
    //var qntyonhand = parseInt($("#P12QoH").text()) || 0;
    //// Empty / invalid / zero
    //if (value === "" || isNaN(qtyToDispatch) || qtyToDispatch <= 0) {

    //    $("#P12FinQntyError").hide();
    //    $(this).css("border", "");

    //    // Keep Save disabled until valid quantity is entered
    //    $("#P12Save").prop("disabled", true);

    //    return;
    //}

    //// Greater than balance
    //if (qtyToDispatch > balDispQnty || qtyToDispatch > qntyonhand) {

    //    $("#P12FinQntyError").show();
    //    $(this).css("border", "2px solid red");
    //    $("#P12Save").prop("disabled", true);

    //}
    //else {

    //    $("#P12FinQntyError").hide();
    //    $(this).css("border", "");
    //    $("#P12Save").prop("disabled", false);
    //}
});
function calculateP12Redistribution() {

    P12RedistributionRequired = false;

    // Single line item = no redistribution
    if (!P12Data || P12Data.length <= 1) {

        $("#P12RedistributionRow").hide();
        $("#P12QntyRedistribution").text(0);

        return;
    }

    // Check whether every line has full coverage
    var allCovered = P12Data.every(function (item) {

        var balQty =
            parseInt(item.balanceSOQty) || 0;

        var plannedQty =
            parseInt(item.suggestedDispQnty) || 0;

        return balQty === plannedQty;
    });

    // All line items covered
    if (allCovered) {

        P12RedistributionRequired = false;

        $("#P12RedistributionRow").hide();
        $("#P12QntyRedistribution").text(0);

        return;
    }

    // Redistribution required
    P12RedistributionRequired = true;

    var qoh =
        parseInt(P12Data[0].qntyOnHand) || 0;

    var totalFinalDispatchQty = P12Data.reduce(
        function (sum, item) {
            return sum +
                (parseInt(item.final_Dispatch_Qnty) || 0);
        },
        0
    );

    var redistributionQty =
        qoh - totalFinalDispatchQty;

    if (redistributionQty < 0) {
        redistributionQty = 0;
    }

    $("#P12QntyRedistribution").text(redistributionQty);
    $("#P12RedistributionRow").show();
}
function validateP12Quantity() {

    var P12FinQnty = parseInt($("#P12FinQnty").val()) || 0;
    var P12BalQnty = parseInt($("#P12BalQnty").text()) || 0;
    var P12SuggestedQnty = parseInt($("#P12SuggestedQnty").text()) || 0;
    var P12QntyRedistribution = parseInt($("#P12QntyRedistribution").text()) || 0;

    var errorLabel = $("#P12FinQntyError");
    var input = $("#P12FinQnty");
    var saveButton = $("#P12Save");

    // Reset
    errorLabel.hide();
    errorLabel.text("");
    input.css("border", "");

    // --------------------------------------------------
    // 1. Basic quantity validation
    // --------------------------------------------------

    if (isNaN(parseInt($("#P12FinQnty").val())) || P12FinQnty <= 0) {

        errorLabel.text(
            "Please enter a valid quantity to dispatch."
        );

        errorLabel.show();
        input.css("border", "2px solid red");
        saveButton.prop("disabled", true);

        return false;
    }

    // --------------------------------------------------
    // 2. Common Bal Disp Qty validation
    // --------------------------------------------------

    if (P12FinQnty > P12BalQnty) {

        errorLabel.text(
            "Quantity to Dispatch is greater than Bal Disp.Qnty. Please check and reenter."
        );

        errorLabel.show();
        input.css("border", "2px solid red");
        saveButton.prop("disabled", true);

        return false;
    }

    // ==================================================
    // FLOW 1 : NO REDISTRIBUTION REQUIRED
    // ==================================================

    if (!P12RedistributionRequired) {

        saveButton.prop("disabled", false);

        return true;
    }

    // ==================================================
    // FLOW 2 : REDISTRIBUTION REQUIRED
    // ==================================================

    // --------------------------------------------------
    // 3. If entered quantity is within Suggested Qty,
    //    no redistribution is consumed.
    // --------------------------------------------------

    if (P12FinQnty <= P12SuggestedQnty) {

        saveButton.prop("disabled", false);

        return true;
    }

    // --------------------------------------------------
    // 4. Calculate quantity required from redistribution
    // --------------------------------------------------

    var redistributionRequired =
        P12FinQnty - P12SuggestedQnty;

    // --------------------------------------------------
    // 5. Check available redistribution quantity
    // --------------------------------------------------

    if (P12QntyRedistribution <= 0) {

        errorLabel.text(
            "There is no quantity available for redistribution. Please reduce the quantity or redistribute quantity from another SO / PO."
        );

        errorLabel.show();
        input.css("border", "2px solid red");
        saveButton.prop("disabled", true);

        return false;
    }

    // --------------------------------------------------
    // 6. Requested redistribution exceeds availability
    // --------------------------------------------------

    if (redistributionRequired > P12QntyRedistribution) {

        errorLabel.text(
            "Quantity available for redistribution is not sufficient for this dispatch. Please reduce the quantity or redistribute quantity from another SO / PO."
        );

        errorLabel.show();
        input.css("border", "2px solid red");
        saveButton.prop("disabled", true);

        return false;
    }

    // --------------------------------------------------
    // 7. Valid redistribution
    // --------------------------------------------------

    saveButton.prop("disabled", false);

    return true;
}