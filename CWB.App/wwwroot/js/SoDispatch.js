let g_selectedDispatchItems = [];
function loadSoDispatch() {
    $("#preloaderblurred").show();
    $('#SelectAllMGrid').prop("checked", false);
    api.getbulk("/WorkOrder/AllSODispatch").then((data) => {
        data = data.filter(item => item.status === 1 || item.status === 2 );
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
        for (i = 0; i < data.length; i++) {
            if (data[i].invoiceNo.length > 0 && data[i].qntyOnHand === 0 ) {
                continue;
            }
            $(tablebody).append(AppUtil.ProcessTemplateData("SOGridRow", data[i]));
        }
        if (g_selectedDispatchItems.length > 0) {

            // Create a temporary lookup map of the fresh server data, using the SO ID as the key
            const freshDataMap = new Map(data.map(item => [item.salesOrderId, item]));

            // Filter the global array and replace old data with fresh data
            g_selectedDispatchItems = g_selectedDispatchItems.map(oldItem => {
                const freshItem = freshDataMap.get(parseInt(oldItem.soId));
                oldItem.deispatchId = freshItem.dispatchId;
                oldItem.invoiceNo = freshItem.invoiceNo;
                oldItem.invoiceDate = freshItem.invoiceDate;
                oldItem.dispatchDetail = freshItem.dispatchDetail;
                // If the item still exists in the fresh server data, return the fresh data
                // Otherwise, return null to be filtered out later.
                return oldItem;

            }).filter(item => item !== null); // Remove items that no longer exist/qualify

            console.log("g_selectedDispatchItems synchronized with latest server data.");
            rebindP13Grid();
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
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
        //console.log(error);
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
    $('#SelectAllMGrid').on('change', function () {
        var isChecked = $(this).prop('checked');

        // 2. Select all row checkboxes within the same table body
        // and set their 'checked' property to match the header checkbox.
        // We use the table ID (#SOGrid) and the row checkbox class (.SOgridChks) 
        // to ensure we only target the correct checkboxes.
        $('#SOGrid tbody input.SOgridChks').prop('checked', isChecked);
    });

    // OPTIONAL: Add functionality to uncheck the header checkbox 
    // if a *single* row checkbox is manually unchecked.
    $('#SOGrid tbody input.SOgridChks').on('change', function () {
        var allChecked = true;

        // Check if ALL row checkboxes are currently checked
        $('#SOGrid tbody input.SOgridChks').each(function () {
            if (!$(this).prop('checked')) {
                allChecked = false;
                return false; // Exit the loop early
            }
        });

        // Update the state of the "Select All" checkbox
        $('#SelectAllMGrid').prop('checked', allChecked);
    });
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

        $("#P12PartNo").text(partn);
        $("#P12PartDesc").text(partdesc);
        $("#P12Po").text(pono);
        $("#P12Cust").text(customer);
        $("#P12BalQnty").text(baldispqnty);
        $("#P12QoH").text(baldispqnty);
        $("#P12SoId").val(soid);
        $("#P12FinQnty").val(findisqnty);

        $("#preloaderblurred").show();
        $('#SelectAllMGrid').prop("checked", false);
        api.getbulk("/WorkOrder/AllSODispatch").then((data) => {
            data = data.filter(item => item.status === 1 || item.status === 2);
            data = data.filter(item => item.qntyOnHand > 0);
            data = data.filter(item => item.partNo === partn);
            data.sort((a, b) => {
                // Convert the date strings to Date objects for comparison
                const dateA = new Date(a.requiredByDate);
                const dateB = new Date(b.requiredByDate);

                // Compare: dateA - dateB sorts ascending (earliest first)
                return dateA - dateB;
            });
            var tablebody = $("#P12Grid tbody");
            $(tablebody).html("");
            if (data.length <= 1) {
                $("#P12GridDiv").hide();
            } else {
                $("#P12GridDiv").show();
            }
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("P12GridRow", data[i]));
            }
            $("#preloaderblurred").hide();
        }).catch((error) => {
            $("#preloaderblurred").hide();
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
                                       class="dropdown-item" data-bs-toggle="modal" data-finaldispqnyt="${group.finalDispQnty}" data-pono="${group.poNo}" data-custid="${group.customerOrderId}" 
data-customer="${group.customer}" data-sono="${group.salesOrderNumber}" data-invoicedate="${group.invoiceDate}" data-invoiceno="${group.invoiceNo}" data-partid="${group.partId}"
data-partno="${group.partNoDesc}" data-soid="${group.soId}" data-dispatchfid="${group.deispatchId}" data-dispatchdetail="${group.dispatchDetail}"
                                       data-bs-target="#popup14">Edit</a>
                                </div>
                            </div>
                </td>
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
        var P12BalQnty= parseInt($("#P12BalQnty").text());
        var P12SoId = parseInt($("#P12SoId").val());
        if (P12FinQnty === 0 || isNaN(P12FinQnty)) {
            var newNamevalidate = document.getElementById('P12FinQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P12FinQnty');
            newNamevalidate.style.border = '';
        }
        if (P12FinQnty > P12BalQnty) {
            alert("Qnty to Dispatch is greater than Bal Disp.Qnty.");
            return false;
        }
        var formdata = {
            SalesOrderId: P12SoId,
            FinalDispQnty: P12FinQnty,

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

            // 2. Extract Suggested Quantity from the <th> with class 'suggested-qnty-value'
            var suggestedQntyText = row.find('.suggested-qnty-value').text().trim();
            var suggestedQnty = parseInt(suggestedQntyText);

            // Optional: Get the available quantity to ensure we don't dispatch more than what's on hand
            // Based on your template, {qntyOnHand} is the available quantity (assuming it's in the 7th <th> element)
            var qntyOnHandText = row.find('th:nth-child(7)').text().trim();
            var qntyOnHand = parseInt(qntyOnHandText);

            // Validation and data preparation
            if (soId && !isNaN(suggestedQnty) ) {

                // Check if suggested quantity exceeds quantity on hand (basic client-side check)
                if (suggestedQnty > qntyOnHand) {
                    console.warn(`Skipping SO ${soId}: Suggested Quantity (${suggestedQnty}) exceeds Qnty On Hand (${qntyOnHand}).`);
                    return true; // continue to the next iteration
                }

                bulkUpdateData.push({
                    SalesOrderId: parseInt(soId),
                    FinalDispQnty: suggestedQnty // Use the suggested quantity for the final dispatch
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

    $("#P12PartNo").text(partn);
    $("#P12PartDesc").text(partdesc);
    $("#P12Po").text(pono);
    $("#P12Cust").text(customer);
    $("#P12BalQnty").text(baldispqnty);
    $("#P12QoH").text(baldispqnty);
    $("#P12SoId").val(soid);
    $("#P12FinQnty").val(findisqnty);
}