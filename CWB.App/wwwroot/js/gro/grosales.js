var dispatchAgeing = -1;
var dispatchStatus = "All";
var deliveryAgeing = -1;
var modalStack = [];
$(document).on("show.bs.modal", ".modal", function () {

    var currentModal = $(this);

    if (modalStack.length > 0) {

        modalStack[modalStack.length - 1]
            .find(".modal-content")
            .addClass("modal-stack-blur");

    }

    modalStack.push(currentModal);

});

$(document).on("hidden.bs.modal", ".modal", function () {

    modalStack.pop();

    if (modalStack.length > 0) {

        modalStack[modalStack.length - 1]
            .find(".modal-content")
            .removeClass("modal-stack-blur");

    }

});



$(document).ready(function () {
    $("#preloaderblurred").show();
    loadGroUploadSummary();
    $("#preloaderblurred").hide();

    $("#preloaderblurred").show();
    loadDispatchAgeingSummary();
    $("#preloaderblurred").hide();

    $("#preloaderblurred").show();
    loadDeliveryAgeingSummary();
    $("#preloaderblurred").hide();

    $("#preloaderblurred").show();
    loadPendingCourierCount();
    $("#preloaderblurred").hide();

});
function loadDispatchAgeingSummary() {

    api.getbulk("/Gro/GetDispatchAgeingSummary")

        .then(function (data) {

            bindDispatchAgeingSummary(data);

        })

        .catch(function (err) {

            console.log(err);

        });

}


function makeHyperLink(control, value, status, ageing) {

    if (value == 0) {

        control.text("0");

        return;
    }

    control.html(
        '<a href="javascript:void(0);" ' +
        'onclick="openDispatchAgeing(\'' + status + '\',' + ageing + ')">' +
        value +
        '</a>'
    );
}
function bindDispatchAgeingSummary(data) {

    // Full Qty to be Dispatched
    makeHyperLink($("#full0"), data.full.day0, "No Dispatch", 0);
    makeHyperLink($("#full1"), data.full.day1, "No Dispatch", 1);
    makeHyperLink($("#full2"), data.full.day2, "No Dispatch", 2);
    makeHyperLink($("#full3"), data.full.day3, "No Dispatch", 3);
    makeHyperLink($("#fullgt3"), data.full.gt3, "No Dispatch", 4);

    $("#fulltotal").text(data.full.total);

    // Partial Qty Dispatched
    makeHyperLink($("#partial0"), data.partial.day0, "Partial Dispatch", 0);
    makeHyperLink($("#partial1"), data.partial.day1, "Partial Dispatch", 1);
    makeHyperLink($("#partial2"), data.partial.day2, "Partial Dispatch", 2);
    makeHyperLink($("#partial3"), data.partial.day3, "Partial Dispatch", 3);
    makeHyperLink($("#partialgt3"), data.partial.gt3, "Partial Dispatch", 4);

    $("#partialtotal").text(data.partial.total);

    // Totals (Ageing only, no status filter)
    makeHyperLink($("#total0"),
        data.full.day0 + data.partial.day0,
        "All",
        0);

    makeHyperLink($("#total1"),
        data.full.day1 + data.partial.day1,
        "All",
        1);

    makeHyperLink($("#total2"),
        data.full.day2 + data.partial.day2,
        "All",
        2);

    makeHyperLink($("#total3"),
        data.full.day3 + data.partial.day3,
        "All",
        3);

    makeHyperLink($("#totalgt3"),
        data.full.gt3 + data.partial.gt3,
        "All",
        4);

    $("#grandtotal").text(
        data.full.total +
        data.partial.total
    );
}
function openDispatchAgeing(status, ageing) {

    dispatchStatus = status;
    dispatchAgeing = ageing;

    $("#dispatchPopup").modal("show");

}
function loadPendingCourierCount() {

    api.getbulk("/Gro/GetPendingCourierCount")
        .then(function (data) {

            $("#pendingCourierCount").text(data);

        })
        .catch(function (err) {

            console.log(err);

        });

}
function loadDeliveryAgeingSummary() {

    api.getbulk("/Gro/GetDeliveryAgeingSummary")
        .then(function (data) {

            bindDeliveryAgeingSummary(data);

        })
        .catch(function (err) {

            console.log(err);

        });

}
function bindDeliveryAgeingSummary(data) {

    $("#pendingDeliveryCount").text(data.total);

    makeDeliveryHyperLink($("#delivery0to2"), data.day0to2, 0);
    makeDeliveryHyperLink($("#delivery3to5"), data.day3to5, 1);
    makeDeliveryHyperLink($("#delivery6to7"), data.day6to7, 2);
    makeDeliveryHyperLink($("#delivery8to10"), data.day8to10, 3);
    makeDeliveryHyperLink($("#deliverygt10"), data.daygt10, 4);

    $("#deliveryTotal").text(data.total);

}
function makeDeliveryHyperLink(control, value, ageing) {

    if (value == 0) {

        control.text("0");

        return;
    }

    control.html(
        '<a href="javascript:void(0);" ' +
        'onclick="openDeliveryAgeing(' + ageing + ')">' +
        value +
        '</a>'
    );

}
function openDeliveryAgeing(ageing) {

    deliveryAgeing = ageing;

    $("#deliverydataupdatemodal").modal("show");
}

$('#groUploadModal').on('hidden.bs.modal', function () {

    // Clear selected file
    $('#groExcelFile').val('');

    // Clear displayed file name
    $('#selectedFileName').val('');
});
function displayFileName() {

    let fileInput =
        document.getElementById("groExcelFile");

    if (fileInput.files.length > 0) {

        document.getElementById("selectedFileName")
            .value =
            fileInput.files[0].name;
    }
}

function startGroUpload() {
    $("#preloaderblurred").show();
    let fileInput =
        document.getElementById("groExcelFile");

    if (fileInput.files.length === 0) {

        alert("Please Select Excel File");

        return;
    }

    let formData =  new FormData();

    formData.append("uploadedFile", fileInput.files[0]);
    
    $.ajax({

        url: "/Gro/UploadGroData",

        type: "POST",

        data: formData,

        processData: false,

        contentType: false,

        success: function (response) {

            if (response.success) {

                $("#preloaderblurred").hide();
                $("#IndentCount").text(response.indentCount);
                $("#ProductCount").text(response.productCount);

                alert("Upload Completed");
            }
            else {
                $("#preloaderblurred").hide();
                $("#ErrorPreview")
                    .html(response.message);

                $("#UploadErrroModal")
                    .modal('show');
            }
        }
    });
}
function loadGroUploadSummary() {

    $.get("/Gro/GetGroUploadSummary", function (response) {

        $("#LastUpdatedDate")
            .text(response.lastUpdatedDate);

        //$("#IndentCount")
        //    .text(response.indentCount);

        //$("#ProductCount")
        //    .text(response.productCount);
    });
}

$('#dispatchPopup').on('shown.bs.modal', function () {
    $("#preloaderblurred").show();
    loadDispatchSelection(function () {

        // Show / Hide Ageing
        if (dispatchAgeing > -1) {

            $("#lblAgeing").text(
                dispatchAgeing == 4 ? "> 3" : dispatchAgeing
            );

            $("#divAgeingInfo").show();

        }
        else {

            $("#divAgeingInfo").hide();

        }

        // Apply Dispatch Status selected from dashboard
        $("#ddlDispatchStatus").val(dispatchStatus);

        // Filter after grid is loaded
        applyFilters();

    });
    $("#preloaderblurred").hide();
    $('#txtIndentSearch').on('keyup', applyFilters);
    $('#ddlDispatchStatus').on('change', applyFilters);
    $('#txtFromDate,#txtToDate').on('change', applyFilters);
    function applyFilters() {

        var indent = $('#txtIndentSearch').val().toLowerCase().trim();

        var status = $('#ddlDispatchStatus').val();

        var from = $('#txtFromDate').val()
            ? new Date($('#txtFromDate').val())
            : null;

        var to = $('#txtToDate').val()
            ? new Date($('#txtToDate').val())
            : null;

        if (to)
            to.setHours(23, 59, 59, 999);

        $('#Dispatchselectiontable tbody tr').each(function () {

            var row = $(this);
            var rowAgeing = parseInt(row.find("td:eq(13)").text());

            var matchAgeing =
                dispatchAgeing == -1 || rowAgeing == dispatchAgeing;

            var rowIndent = row.find('td:eq(6)').text().toLowerCase().trim();

            var rowDate = parseDate(row.find('td:eq(5)').text().trim());

            var rowStatus = row.find('td:eq(12)').text().trim();

            var matchIndent = indent === "" || rowIndent.includes(indent);

            var matchFrom = !from || rowDate >= from;

            var matchTo = !to || rowDate <= to;

            var matchStatus = status === "All" || rowStatus === status;

            row.toggle(matchIndent && matchFrom && matchTo &&matchStatus &&  matchAgeing );

        });

    }
    $('#btnClearFilter').on('click', function () {

        $('#txtIndentSearch').val('');
        $('#txtFromDate').val('');
        $('#txtToDate').val('');
        $('#ddlDispatchStatus').val('All');
        applyFilters();   // All rows become visible because no filters are applied
    });
    function parseDate(dateStr) {

        if (!dateStr)
            return null;

        var parts = dateStr.split('-'); // dd-MM-yyyy

        return new Date(parts[2], parts[1] - 1, parts[0]);
    }

        // Update Select All and button after filtering
         

    
    $(document).on('change',
        '#Dispatchselectiontable tbody input[type=checkbox]',
        function () {

           // updateDispatchButton();
            updatePrintLineItemsButton();

        });
    function updatePrintLineItemsButton() {

        var checkedCount = $('#Dispatchselectiontable tbody input[type=checkbox]:checked').length;

        $('#btnPrintLineItems').prop('disabled', checkedCount === 0);

    }
    function updateDispatchButton() {

        var selectedIndentNos = [];

        $('#Dispatchselectiontable tbody input[type=checkbox]:checked').each(function () {

            // Change the td:eq(x) to the actual column index of Indent No
            var indentNo = $(this).closest('tr').find('td:eq(6)').text().trim();

            if ($.inArray(indentNo, selectedIndentNos) === -1) {
                selectedIndentNos.push(indentNo);
            }
        });

        // More than one distinct indent selected
        if (selectedIndentNos.length > 1) {

            alert("Select only one Unique Indent No.");

            $('#Dispatchselectiontable tbody input[type=checkbox]')
                .prop('checked', false);

            $('#updatedispatchqnty').prop('disabled', true);

            return;
        }

        // Enable button only if one or more rows are selected
        var checkedCount = $('#Dispatchselectiontable tbody input[type=checkbox]:checked').length;

        $('#updatedispatchqnty').prop('disabled', checkedCount === 0);
    }






});

$("#btnPrintLineItems").click(function () {

    var indentNos = [];

    $('#Dispatchselectiontable tbody input[type=checkbox]:checked').each(function () {

        var indentNo = $(this).closest("tr").find("td:eq(6)").text().trim();

        if ($.inArray(indentNo, indentNos) === -1) {
            indentNos.push(indentNo);
        }

    });

    api.post("/Gro/GetPrintLineItems", {

        IndentNos: indentNos

    }).then(function (response) {

        bindPrintLineItems(response);

        $("#printLineItemsModal").modal("show");

    });

});

function bindPrintLineItems(response) {
    var indents = response.selectedIndents.split("     ");

    var html = "";

    $.each(indents, function (i, indent) {

        html += indent;

        if ((i + 1) % 5 === 0) {
            html += "<br/>";
        }
        else if (i < indents.length - 1) {
            html += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }

    });

    $("#selectedIndents").html(html);
    // Selected Indents
   

    // Clear old rows
    $("#printLineItemsBody").empty();

    if (!response.items || response.items.length === 0) {
        return;
    }

    // Bind rows
    $.each(response.items, function (i, item) {

        var row = $("#printLineItemRow").html();

        row = row.replace("{partNo}", item.partNo);
        row = row.replace("{totalOrderQty}", item.totalOrderQty);
        row = row.replace("{qtyAvailable}", item.qtyAvailable);
        row = row.replace("{balanceToPack}", item.balanceToPack);

        $("#printLineItemsBody").append(row);

    });

}

$("#btnPrintLineItemsList").click(function () {

    var printContents = $("#printLineItemsContent").html();

    var printWindow = window.open("", "", "width=1000,height=700");

    printWindow.document.write(`
        <html>
        <head>
            <title>Line Items Required</title>

            <link rel="stylesheet"
                  href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css">

            <style>

                body{
                    padding:20px;
                    font-family:Arial;
                    font-size:14px;
                }

                table{
                    width:100%;
                    border-collapse:collapse;
                }

                table,th,td{
                    border:1px solid black;
                }

                th,td{
                    padding:6px;
                }

                th{
                    text-align:center;
                }

                h3{
                    text-align:center;
                    margin-bottom:20px;
                }

            </style>

        </head>

        <body>

            <h3>Line Items Required</h3>

            ${printContents}

        </body>

        </html>
    `);

    printWindow.document.close();

    printWindow.focus();

    printWindow.print();

    printWindow.close();

});


$('#printLineItemsModal').on('hidden.bs.modal', function () {

    // Uncheck all checkboxes
    $('#Dispatchselectiontable tbody input[type=checkbox]')
        .prop('checked', false);

    // Disable Print Line Items button
    $('#btnPrintLineItems')
        .prop('disabled', true);
    $('#selectedIndents').empty();

    $('#printLineItemsBody').empty();
});



$('#dispatchPopup').on('hidden.bs.modal', function () {

    // Reset global variables
    dispatchAgeing = -1;
    dispatchStatus = "All";

    // Hide Ageing Information
    $("#divAgeingInfo").hide();
    $("#lblAgeing").text("");

    // Reset Filters
    $("#txtIndentSearch").val("");
    $("#txtFromDate").val("");
    $("#txtToDate").val("");
    $("#ddlDispatchStatus").val("All");

    // Reset Selection
    $("#chkAll").prop("checked", false);
    $("#Dispatchselectiontable tbody input[type=checkbox]").prop("checked", false);

    // Disable Button
    $("#updatedispatchqnty").prop("disabled", true);

});
function ShowDispatchDetails(ele) {

    var item = $(ele);

    $('#lblIndentDate').text(item.data('indentdate'));
    $('#lblIndentNo').text(item.data('indentno'));
    $('#lblCustomer').text(item.data('customer'));
    $('#lblAddress').text(item.data('address'));
    $('#lblContactperson').text(item.data('contact'));
    $('#lblContactpersonCellPhone').text(item.data('cellphone'));
    $('#lblExecutive').text(item.data('executive'));
    $('#lblProduct').text(item.data('partno'));

    var reqQty = parseInt(item.data('requiredqty')) || 0;
    var balQty = parseInt(item.data('balqty')) || 0;

    var dispatchedQty = reqQty - balQty;
    $('#lblReqQty').text(reqQty + ' Nos');
    $('#lblTotalDispatch').text(dispatchedQty + ' Nos');
    $('#lblBalance').text(balQty + ' Nos');

   

    //Dispatch Details not implemented yet
    var groDataId = item.data("grodataid");

    $("#DispatchedDetailstable tbody").empty();

    api.getbulk("/Gro/GetLineItemDispatchHistory?groDataId=" + groDataId)
        .then(function (data) {

            if (data.length > 0) {

                $("#disp").show();

                for (var i = 0; i < data.length; i++) {

                    $("#DispatchedDetailstable tbody").append(
                        AppUtil.ProcessTemplateData(
                            "dispatchdetailrow",
                            data[i]
                        )
                    );
                }
            }
            else {

                $("#disp").hide();

            }
        });

    $('#dispatchDetailsPopup').modal('show');

}
function loadDispatchSelection(callback) {





    api.getbulk("/Gro/GetDispatchSelection").then((data) => {
        
        var tablebody = $("#Dispatchselectiontable tbody");
        $(tablebody).html("");//empty tbody
        
        //console.log(data);
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
        for (i = 0; i < data.length; i++) {
           
            //data[i].strStatus = WoOrdStatus[data[i].status];
            //data[i].woType = "";
            $(tablebody).append(AppUtil.ProcessTemplateData("dispatchSelectionRow", data[i]));
        }
        if (callback)
            callback();
    }).catch((error) => {
    });

}
$('#dispatchQtyModal').on('show.bs.modal', function () {

    var indentNo = "";

    $('#Dispatchselectiontable tbody input[type=checkbox]:checked').each(function () {

        indentNo = $(this).closest('tr').find('td:eq(6)').text().trim();

        return false; // first row is enough since all selected rows have same indent
    });
    
    loadDispatchData(indentNo);

});
 
function loadDispatchData(indentNo) {

    api.getbulk("/Gro/GetDispatchData?indentNo=" + encodeURIComponent(indentNo))
        .then((data) => {

            bindDispatchHeader(data.grodispheaderbyindent, data.result);

            bindDispatchGrid(data.result);

        })
        .catch((error) => {

            console.log(error);

           // AppUtil.MessageBox("Unable to load dispatch details.", 2);

        });

}
function bindDispatchHeader(header, result) {

    const firstRow = result.length > 0 ? result[0] : null;

    const date = new Date(header.sentDate);

    $("#IndentDate").text(
        String(date.getDate()).padStart(2, '0') + "-" +
        String(date.getMonth() + 1).padStart(2, '0') + "-" +
        date.getFullYear()
    );
    $("#hdnGroDispHeaderId").val(header.gro_Disp_HeaderId);
    $("#hdnIndentId").val(header.indent);
    $("#IndentNo").text(header.indent);

    $("#Customer").text(firstRow ? firstRow.company_Name : "");

    $("#Address").text(
        header.shipping_Address + ", " +
        header.shipping_City + " - " +
        header.shipping_PINCODE
    );

    $("#Contact").text(firstRow ? firstRow.contact_Person : "");

    $("#Mobile").text(firstRow ? firstRow.contact_Person_No : "");
    $("#Executive").text(firstRow ? firstRow.excutive_Name : "");
}
function bindDispatchGrid(data) {

    var tablebody = $("#Dispheadertable tbody");

    tablebody.html("");

    if (data.length === 0) {

        tablebody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < data.length; i++) {
        data[i].editDispatchStyle =
            Number(data[i].qntyAval) > 0 ? "" : "display:none;";
        tablebody.append(
            AppUtil.ProcessTemplateData("dispatchQtyRow", data[i])
        );
    }
    updateDispatchButtons();

}
function updateDispatchButtons() {

    var hasScannedQty = false;

    $("#Dispheadertable tbody tr").each(function () {

        var scannedQty = parseInt($(this).find("td:eq(6)").text()) || 0;

        if (scannedQty > 0) {
            hasScannedQty = true;
            return false;
        }

    });

    $("#btnReadyDispatch").prop("disabled", !hasScannedQty);

    $("#btnOpenDCPrint").prop("disabled", true);

    $("#btnPrintInvoice").prop("disabled", true);

}
$("#btnReadyDispatch").click(function () {

    $("#btnOpenDCPrint").prop("disabled", false);

    $("#btnPrintInvoice").prop("disabled", false);

    $(this).prop("disabled", true);

});
function loadDispatchAddress() {

    var headerId = $("#hdnGroDispHeaderId").val();
    var indentId = $("#hdnIndentId").val();

    api.getbulk("/Gro/GetDispatchAddress?headerId=" + encodeURIComponent(headerId)
        + "&indentId=" + encodeURIComponent(indentId))
        .then((data) => {

            // Bind here once controller returns data

            var header = data.header;
            var groData = data.result;
            $("#HeaderId").val(header.gro_Disp_HeaderId);
            $("#addrIndentDate").text(groData.indentDateStr);
            $("#addrIndentNo").text(groData.indent);
            $("#addrContact").text(groData.contact_Person);
            $("#addrCellPhone").text(groData.contact_Person_No);
            $("#addrExecutiveName").text(groData.excutive_Name);

            $("#txtCustomer").val(groData.company_Name);
            $("#txtAddress").val(header.shipping_Address);
            $("#txtCity").val(header.shipping_City);
            $("#txtPin").val(header.shipping_PINCODE);

        })
        .catch((error) => {

            console.log(error);

            // AppUtil.MessageBox("Unable to load dispatch address.", 2);

        });

}
$("#btnSaveDispatchAddress").click(function () {

    var model = {
        gro_Disp_HeaderId: $("#HeaderId").val(),
        shipping_Address: $("#txtAddress").val(),
        shipping_City: $("#txtCity").val(),
        shipping_Pincode: $("#txtPin").val()
    };
    var indentId = $("#hdnIndentId").val();
    api.post("/Gro/UpdateDispatchAddress", model)
        .then(function (response) {

            //AppUtil.MessageBox("Dispatch address updated successfully.", 1);

            $("#editDispatchAddressModal").modal("hide");
            loadDispatchData(indentId);

        })
        .catch(function (error) {

            console.log(error);

            //AppUtil.MessageBox("Unable to update dispatch address.", 2);

        });

});
$("#dispatchDetailsDataUpdatePopup").on("shown.bs.modal", function () {
    loadDispatchDetailsDataUpdate();

    $('#searchbyindent').on('keyup', function () {
        filterbyIndent();
    });
    function filterbyIndent() {

        var indent = $('#searchbyindent').val().toLowerCase().trim();

        $('#dispatchdataupdatetable tbody tr').each(function () {

            var row = $(this);

            var rowIndent = row.find('td:eq(3)').text().toLowerCase().trim();

            row.toggle(indent === "" || rowIndent.includes(indent));

        });

    }


});
function loadDispatchDetailsDataUpdate() {

    api.getbulk("/Gro/GetDispatchDetailsDataUpdate")
        .then((data) => {

            var tablebody = $("#dispatchdataupdatetable tbody");
            tablebody.html("");

            if (data.length === 0) {

                tablebody.append(`
                    <tr class="norecordsfound">
                        <td colspan="5" class="text-center text-muted">
                            <strong>No Records Found</strong>
                        </td>
                    </tr>`);

                return;
            }

            for (var i = 0; i < data.length; i++) {

                tablebody.append(
                    AppUtil.ProcessTemplateData("dispatchUpdateRow", data[i])
                );

            }

        })
        .catch((error) => {

            console.log(error);

        });

}



function ShowDetails(ele) {

    let tr = $(ele).closest("tr");

    let indent = tr.find("td:eq(3)").text().trim();

    let headerId = tr.find("td:eq(1)").text().trim();

    LoadDispatchDetails(indent, headerId, "SHOW");

}
function UpdateDispheader(ele) {

    let tr = $(ele).closest("tr");

    let indent = tr.find("td:eq(3)").text().trim();

    let headerId = tr.find("td:eq(1)").text().trim();

    LoadDispatchDetails(indent, headerId, "UPDATE");

}
function LoadDispatchDetails(indent, headerId, mode) {

    $.ajax({

        url: '/Gro/GetDispatchDetails',

        type: 'POST',

        data: {
            indent: indent,
            groDispHeaderId: headerId
        },

        success: function (response) {

            BindDispatchPopup(response, mode);

            $("#dispatchdataupdatebyindentmodal").modal("show");
        }

    });

}

function BindDispatchPopup(data, mode) {

    var header = data.header;
    var result = data.result;

    $("#updIndentDate").text(header.indentDateStr);

    $("#updIndentNo").text(header.indent);

    $("#updCustomer").text(header.company_Name);
    $("#updExecutive").text(header.excutive_Name);

    $("#updAddress").text(header.shipping_Address + ", " +
        header.shipping_City + " - " +
        header.shipping_PINCODE);

    $("#updContact").text(header.contact_Person);

    $("#updMobile").text(header.contact_Person_No);

    // Dispatch Details

    $("#ddlCourier").val(header.courier_Partner);

    $("#txtAwbReference").val(header.awb);

    $("#txtDispatchDate").val(header.dispatch_Date);
    $("#txtDispatchDate").attr(
        "max",
        new Date().toISOString().split('T')[0]
    );
    // Grid

    BindDispatchLines(result);

    // Show/Hide Update Section

    if (mode === "SHOW") {

        $("#dispatchUpdateSection").hide();

    }
    else {
        $("#updateHeaderId").val(header.gro_Disp_HeaderId);
        $("#dispatchUpdateSection").show();
        api.getbulk("/Gro/GetCouriers")
            .then((data) => {
                console.log(data);
                let ddl = $("#ddlCourier");

                ddl.empty();

              //  ddl.append('<option value="">Select Courier</option>');

                $.each(data, function (i, item) {

                    ddl.append(
                        $('<option></option>')
                            .val(item.courier_List_ID)
                            .text(item.courier_Name)
                    );

                });

            })
            .catch((error) => {

                console.log(error);

            });

    }

}
function BindDispatchLines(lines) {



    var tablebody = $("#dispatchupdateline tbody");

    tablebody.html("");

    if (lines.length === 0) {

        tablebody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < lines.length; i++) {

        tablebody.append(
            AppUtil.ProcessTemplateData("dispatchupdatelinerow", lines[i])
        );
    }
  
}
$("#btnUpdatedispatchdate").click(function () {

    var headerId = $("#updateHeaderId").val();

    var courierId = $("#ddlCourier").val();

    var awb = $("#txtAwbReference").val().trim();

    var dispatchDate = $("#txtDispatchDate").val();
    var model = {

        gro_Disp_HeaderId: $("#updateHeaderId").val(),

        awb: $("#txtAwbReference").val().trim(),
        courier_Partner: courierId,
        dispatch_Date: dispatchDate


    };
    api.post("/Gro/CheckAwbUnique", model)
        .then((result) => {

            if (!result.success) {

                alert("AWB No already exists. Please check and re-enter.");

                return;
            }
            api.post("/Gro/UpdateDispatchDetailByheader", model)
                .then((result) => {
                    loadDispatchDetailsDataUpdate();

                    $("#dispatchdataupdatebyindentmodal").modal("hide");
                    // Unique - proceed with Update API
                    loadDeliveryAgeingSummary();
                    loadPendingCourierCount();

                })
                .catch((err) => {

                    console.log(err);

                });

        })
        .catch((err) => {

            console.log(err);

        });

       

    // Call update API
});
$("#deliverydataupdatemodal").on("show.bs.modal", function () {

    LoadDeliveryPendingList();
    if (deliveryAgeing > -1) {

        $("#divDeliveryAgeing").show();

        switch (deliveryAgeing) {

            case 0:
                $("#lblDeliveryAgeing").text("0 - 2 Days");
                break;

            case 1:
                $("#lblDeliveryAgeing").text("3 - 5 Days");
                break;

            case 2:
                $("#lblDeliveryAgeing").text("6 - 7 Days");
                break;

            case 3:
                $("#lblDeliveryAgeing").text("8 - 10 Days");
                break;

            case 4:
                $("#lblDeliveryAgeing").text("> 10 Days");
                break;
        }

    }
    else {

        $("#divDeliveryAgeing").hide();

    }
    $("#txtDeliveryIndentSearch").on("keyup", FilterDeliveryGrid);

    $("#txtDeliveryCourierSearch").on("keyup", FilterDeliveryGrid);

    $("#txtDeliveryFromDate").on("change", FilterDeliveryGrid);

    $("#txtDeliveryToDate").on("change", FilterDeliveryGrid);
    $("#btnClearDeliveryFilter").click(function () {

        $("#txtDeliveryIndentSearch").val("");
        $("#txtDeliveryCourierSearch").val("");
        $("#txtDeliveryFromDate").val("");
        $("#txtDeliveryToDate").val("");

        FilterDeliveryGrid();

    });
  
});
function FilterDeliveryGrid() {

    var indent = $("#txtDeliveryIndentSearch").val().toLowerCase().trim();
    var courier = $("#txtDeliveryCourierSearch").val().toLowerCase().trim();

    var fromDate = $("#txtDeliveryFromDate").val();
    var toDate = $("#txtDeliveryToDate").val();

    $("#deliverydatatable tbody tr").each(function () {

        var row = $(this);
        var rowAgeing = parseInt(row.find("td:eq(5)").text());
        var dispatchDate = row.find("td:eq(1)").text().trim(); // dd-MM-yyyy
        var rowIndent = row.find("td:eq(2)").text().toLowerCase().trim();
        var rowCustomer = row.find("td:eq(3)").text().toLowerCase().trim();
        var rowCourier = row.find("td:eq(4)").text().toLowerCase().trim();

        var show = true;

        // Indent filter
        if (indent !== "" && rowIndent.indexOf(indent) === -1) {
            show = false;
        }
        var matchAgeing = false;

        if (deliveryAgeing == -1)
            matchAgeing = true;
        else if (deliveryAgeing == 0)
            matchAgeing = rowAgeing >= 0 && rowAgeing <= 2;
        else if (deliveryAgeing == 1)
            matchAgeing = rowAgeing >= 3 && rowAgeing <= 5;
        else if (deliveryAgeing == 2)
            matchAgeing = rowAgeing >= 6 && rowAgeing <= 7;
        else if (deliveryAgeing == 3)
            matchAgeing = rowAgeing >= 8 && rowAgeing <= 10;
        else if (deliveryAgeing == 4)
            matchAgeing = rowAgeing >= 11;
        // Courier filter
        if (courier !== "" && rowCourier.indexOf(courier) === -1) {
            show = false;
        }

        // Date filter
        if (show && (fromDate !== "" || toDate !== "")) {

            var parts = dispatchDate.split("-");
            var rowDate = new Date(parts[2], parts[1] - 1, parts[0]);

            if (fromDate !== "") {

                var from = new Date(fromDate);

                if (rowDate < from)
                    show = false;
            }

            if (toDate !== "") {

                var to = new Date(toDate);

                if (rowDate > to)
                    show = false;
            }
        }

        row.toggle(show && matchAgeing);

    });

}
function LoadDeliveryPendingList() {
    api.getbulk("/Gro/GetPendingDeliveryDetails")
        .then((data) => {

            let tbody = $("#deliverydatatable tbody");

            tbody.empty();
            tbody.html("");

            if (data.length === 0) {

                tbody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

                return;
            }

            for (var i = 0; i < data.length; i++) {

                tbody.append(
                    AppUtil.ProcessTemplateData("deliverydetailrow", data[i])
                );
            }

            FilterDeliveryGrid();
        })
        .catch((err) => {

            console.log(err);

        });
}


$("#deliverydataupdatemodal").on("hidden.bs.modal", function () {

    deliveryAgeing = -1;

    $("#divDeliveryAgeing").hide();

    $("#txtDeliveryIndentSearch").val("");
    $("#txtDeliveryCourierSearch").val("");
    $("#txtDeliveryFromDate").val("");
    $("#txtDeliveryToDate").val("");

});


function LoadDeliveryDetails(ele, mode) {

    var tr = $(ele).closest("tr");

    var headerId = tr.find("td:eq(0)").text().trim();

    $.ajax({

        url: "/Gro/GetDeliveryDetails",

        type: "POST",

        data: {

            groDispHeaderId: headerId

        },

        success: function (response) {
            console.log(response);
            BindDeliveryPopup(response, mode);

            $("#deliverydataupdatebyindent").modal("show");

        }

    });

}
function BindDeliveryPopup(data, mode) {

    var header = data.header;

    $("#deliveryUpdateHeaderId").val(header.gro_Disp_HeaderId);

    $("#delIndentDate").text(header.indentDateStr);
    $("#delIndentNo").text(header.indent);
    $("#delCustomer").text(header.company_Name);
    $("#delAddress").text(header.shipping_Address + ", " +
        header.shipping_City + " - " +
        header.shipping_PINCODE);
    $("#delContact").text(header.contact_Person);
    $("#delMobile").text(header.contact_Person_No);
    $("#delExecutive").text(header.excutive_Name);

    $("#DispatchDate").text(header.dispatchDateStr);
    $("#courier").text(header.courier);
    $("#awbref").text(header.awb);

    BindDeliveryGrid(data.result);

    if (mode == "view") {

        $("#deliveryUpdateSection").hide();

        $("#deliverydataupdatebyindentLabel")
            .text("Gro - Delivery Details");

    }
    else {

        $("#deliveryUpdateSection").show();

        $("#deliverydataupdatebyindentLabel")
            .text("Gro - Delivery Data Update By Indent");

        // Default to today for update
        $("#txtActualDeliveryDate").val(new Date().toISOString().split('T')[0]);
        $("#txtActualDeliveryDate").attr(
            "max",
            new Date().toISOString().split('T')[0]
        );
        // Or if you prefer existing value when editing:
        // $("#txtActualDeliveryDate").val(header.delivery_Date_Str);

    }

}
function BindDeliveryGrid(lines) {



    var tablebody = $("#DeliveryDataUpdateGrid tbody");

    tablebody.html("");

    if (lines.length === 0) {

        tablebody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < lines.length; i++) {

        tablebody.append(
            AppUtil.ProcessTemplateData("deliveryupdatelinerow", lines[i])
        );
    }

}
$("#btnUpdateDeliveryData").click(function () {

    var headerId = $("#deliveryUpdateHeaderId").val();
    var deliveryDate = $("#txtActualDeliveryDate").val();

    if (!deliveryDate) {
        alert("Please select Delivery Date.");
        $("#txtActualDeliveryDate").focus();
        return;
    }

    var model = {
        gro_Disp_HeaderId: parseInt(headerId),
        delivered_Date: deliveryDate
    };

    $.ajax({

        url: "/Gro/UpdateDeliveryDateDispHeader",

        type: "POST",

        data: model,

        success: function (response) {

            if (response.success) {

                alert("Delivery date updated successfully.");

                $("#deliverydataupdatebyindent").modal("hide");

                // Reload the delivery list if required
                LoadDeliveryPendingList();
                loadDeliveryAgeingSummary();
            }
            else {

                alert(response.message);
            }

        },

        error: function () {

            alert("An error occurred while updating delivery date.");
        }

    });

});

$("#deliveryCompletePopup").on("show.bs.modal", function () {

    LoadDeliveryCompleteData();
    // Indent typing
    $("#txtSearchIndent").on("keyup", function () {
        FilterDeliveryCompleteGrid();
    });

    // Date category change
    $("#ddlDateCategory").on("change", function () {
        FilterDeliveryCompleteGrid();
    });

    // Date changes
    $("#txtdelFromDate, #txtdelToDate").on("change", function () {
        FilterDeliveryCompleteGrid();
    });
    $("#btnClearDeliveryComplete").click(function () {

        $("#txtSearchIndent").val("");
        $("#ddlDateCategory").val("");
        $("#txtdelFromDate").val("");
        $("#txtdelToDate").val("");

        // Show all rows
        $("#deliverycompletetable tbody tr").show();

    });
    function parseDate(dateStr) {

        if (!dateStr)
            return null;

        var parts = dateStr.split('-'); // dd-MM-yyyy

        return new Date(parts[2], parts[1] - 1, parts[0]);
    }
    function FilterDeliveryCompleteGrid() {

        var indent = $("#txtSearchIndent").val().toLowerCase().trim();
        var dateCategory = $("#ddlDateCategory").val();
        var fromDate = $("#txtdelFromDate").val();
        var toDate = $("#txtdelToDate").val();

        var from = fromDate ? new Date(fromDate) : null;
        var to = toDate ? new Date(toDate) : null;

        $("#deliverycompletetable tbody tr").each(function () {

            var row = $(this);

            var rowIndent = row.find("td:eq(1)").text().toLowerCase().trim();

            var indentDate = row.find("td:eq(0)").text().trim();
            var dispatchDate = row.find("td:eq(7)").text().trim();
            var deliveryDate = row.find("td:eq(8)").text().trim(); // hidden column

            var selectedDate = "";

            if (dateCategory === "Indent")
                selectedDate = indentDate;
            else if (dateCategory === "Dispatch")
                selectedDate = dispatchDate;
            else if (dateCategory === "Delivery")
                selectedDate = deliveryDate;

            var show = true;

            // Indent filter
            if (indent !== "" && !rowIndent.includes(indent))
                show = false;

            // Date filter
            if (selectedDate !== "" && (from || to)) {

                var rowDate = parseDate(selectedDate);

                if (from && rowDate < from)
                    show = false;

                if (to && rowDate > to)
                    show = false;
            }

            row.toggle(show);

        });

    }
});
function LoadDeliveryCompleteData() {

    api.getbulk("/Gro/GetDeliveryCompleteData")
        .then((data) => {

            var tbody = $("#deliverycompletetable tbody");

             

            tbody.empty();
            tbody.html("");

            if (data.length === 0) {

                tbody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

                return;
            }

            for (var i = 0; i < data.length; i++) {

                tbody.append(
                    AppUtil.ProcessTemplateData("deliverycompleterow", data[i])
                );
            }

        })
        .catch((error) => {

            console.log(error);

        });

}
//dispatch Quanity Update popup4 label scanning 
function EditDispatchQty(ctrl) {

    var model = {
        gro_Disp_DetId: 0,

        gro_Disp_Header_ID: $("#hdnGroDispHeaderId").val(),
        
        gro_data_ID: $(ctrl).data("grodataid"),

        gro_Part_No: $(ctrl).data("partid"),

        qnty_Dispatched: 0
    };

    api.post("/Gro/CreateDispatchDetail", model)

        .then(function (data) {

            $("#hdnDispDetailId").val(data.gro_Disp_DetId);
                var row = $(ctrl);

            $("#hdnGroDataId").val(row.data("grodataid"));
            $("#hdnPartId").val(row.data("partid"));

            $("#liIndentDate").text(row.data("indentdate"));
            $("#liIndentNo").text(row.data("indent"));
            $("#liPartNo").text(row.data("partno"));

            $("#liOrderQty").text(row.data("requiredqty"));
            $("#liDispTillDate").text(row.data("disptilldate"));
            $("#liQtyOnHand").text(row.data("qntyaval"));

            $("#txtBalanceDispatch").val(row.data("balqty"));

            var scanned = row.data("scannedqty");

            if (scanned == null || scanned === "")
                scanned = 0;

            $("#txtScannedQty").val(scanned);

            // Status Message

            if (Number(scanned) >= Number(row.data("balqty"))) {

                $("#txtDispatchStatus").val(
                    "Balance to Dispatch Quantity Scanned - Save & Exit"
                );

            }
            else {

                $("#txtDispatchStatus").val("");

            }
           // bindDispatchDetail(data);

        })

        .catch(function (error) {

            console.log(error);

           // AppUtil.MessageBox("Unable to load dispatch details.", 2);

        });

}

$("#btnScanLabel").click(function () {

    var model = {

        gro_Disp_Det_Id: $("#hdnDispDetailId").val(),

        requiredQty: parseInt($("#txtBalanceDispatch").val())
    };

    api.post("/Gro/ScanDispatchLabels", model)

        .then(function (data) {

            $("#txtScannedQty").val(data.scannedQty);

            $("#txtBalanceDispatch").val(data.balanceQty);

            $("#txtDispatchStatus").val(data.message)

            //if (data.disableScanner) {

            //    $("#btnScanLabel").prop("disabled", true);

            //    $("#btnSaveExitDispatch")
            //        .removeClass("btn-secondary")
            //        .addClass("btn-primary");
            //}

        })

        .catch(function (error) {

            console.log(error);

           // AppUtil.MessageBox("Unable to assign stock.", 2);

        });

});
$("#btnSaveExitDispatch").click(function () {

    var model = {

        gro_Disp_Det_Id: $("#hdnDispDetailId").val()
    };

    api.post("/Gro/SaveDispatchQty", model)

        .then(function (data) {

            const indentId = $("#hdnIndentId").val();

            loadDispatchData(indentId);
            // Close current popup
            $("#lineItemDispatchQtyModal").modal("hide");

            // Refresh parent dispatch grid
           

            // Optional message
            // AppUtil.MessageBox("Dispatch quantity updated successfully.", 1);

        })

        .catch(function (error) {

            console.log(error);

            // AppUtil.MessageBox("Unable to save dispatch.", 2);

        });

});
//function LoadDispatchsummary(ctrl, mode) {

//    var row = $(ctrl).closest("tr");

//    var cells = row.find("td");

//    // Header Labels
//    $("#detIndentDate").text(cells.eq(0).text().trim());
//    $("#detIndentNo").text(cells.eq(1).text().trim());
//    $("#detCustomer").text(cells.eq(2).text().trim());
//    $("#detExecutive").text(cells.eq(9).text().trim());

//    // Summary Table
//    var html = $("#deliverycompleterowlineitem").html();

//    html = html.replace("{groPartNo}", cells.eq(3).text().trim());
//    html = html.replace("{reqd_Quantity}", cells.eq(4).text().trim());
//    html = html.replace("{qntyDispatched}", cells.eq(5).text().trim());
//    html = html.replace("{bal_to_Disp}", cells.eq(6).text().trim());
//    html = html.replace("{deliveredDateStr}", cells.eq(8).text().trim());

//    $("#dispatchSummaryTable tbody").html(html);

//    // Clear dispatch details section
//    $("#dispatchDetailsContainer").empty();

//    $("#lineItemDispatchDetailsModal").modal("show");
//    // Show popup
    
//}
function LoadDispatchsummary(ctrl, mode) {

    var row = $(ctrl).closest("tr");
    var cells = row.find("td");

    // Header Labels
    $("#detIndentDate").text(cells.eq(0).text().trim());
    $("#detIndentNo").text(cells.eq(1).text().trim());
    $("#detCustomer").text(cells.eq(2).text().trim());
    $("#detExecutive").text(cells.eq(9).text().trim());

    var indent = cells.eq(1).text().trim();

    api.getbulk("/Gro/GetDispatchSummary?indent=" + encodeURIComponent(indent))
        .then(function (data) {
            var showDispatchHistory = false;
            $("#dispatchSummaryTable tbody").empty();

            for (var i = 0; i < data.length; i++) {

                $("#dispatchSummaryTable tbody").append(
                    AppUtil.ProcessTemplateData(
                        "deliverycompleterowlineitem",
                        data[i]
                    )
                );
                if (parseFloat(data[i].balToReceive) > 0) {
                    showDispatchHistory = true;
                }
            }

            $("#dispatchDetailsContainer").empty();
            if (showDispatchHistory) {

                $("#dispatchDetailsContainer").show();

                api.getbulk("/Gro/GetDispatchHistory?indent=" + encodeURIComponent(indent))
                    .then(function (history) {

                        BindDispatchHistory(history);

                    })
                    .catch(function (err) {

                        console.log(err);

                    });
            }
            else {

                $("#dispatchDetailsContainer").hide();

            }

            $("#lineItemDispatchDetailsModal").modal("show");

            
        })
        .catch(function (err) {

            console.log(err);

        });
    
    function BindDispatchHistory(data) {

        $("#dispatchDetailsContainer").empty();

        for (var i = 0; i < data.length; i++) {

            var header = data[i].header;
            var details = data[i].details;

            var html = $("#dispatchhistorytemplate").html();

            html = html.replace(/{dispatchNo}/g, i + 1);
            html = html.replace("{address}", (header.shipping_Address ?? "") +
                ", " +
                (header.shipping_City ?? "") +
                ", " +
                (header.shipping_Pincode ?? ""));
            html = html.replace("{contact}", header.contact_Person);
            html = html.replace("{cellPhone}", header.contact_Person_No);
            html = html.replace("{ourDCNo}", header.dC_No);
            html = html.replace("{invoiceNo}", header.inv_No);
            html = html.replace("{courier}", header.courier);
            html = html.replace("{awb}", header.awb);
            html = html.replace("{dispatchDate}", header.dispatchDateStr);
            html = html.replace("{deliveryDate}", header.deliveredDateStr);

            $("#dispatchDetailsContainer").append(html);
           // console.log($("#dispatchDetailsContainer").html());
            var tbody = $("#dispatchTable_" + (i + 1));

            for (var j = 0; j < details.length; j++) {

                tbody.append(
                    "<tr>" +
                    "<td>" + details[j].partNo + "</td>" +
                    "<td>" + details[j].qnty_Dispatched + "</td>" +
                    "</tr>"
                );
            }
        }
    }
}
$("#btnOpenDCPrint").click(function () {

    var headerId = $("#hdnGroDispHeaderId").val();

    api.getbulk("/Gro/GetDCPrintData?headerId=" + headerId)

        .then(function (data) {

            bindDCPrint(data);

        })

        .catch(function (err) {

            console.log(err);

        });

});

function bindDCPrint(data) {

    //================ Header =================//

    $("#lblprintIndentDate").text(data.header.indentDateStr);
    $("#lblprintIndentNo").text(data.header.indent);

    $("#dcIndentDate").text(data.header.indentDateStr);
    $("#dcIndentNo").text(data.header.indent);

    $("#dcDate").text(data.header.headerDatestr);
    $("#dcNo").text(data.header.gro_Disp_HeaderId);

    $("#dcCustomer").text(data.header.company_Name);
    $("#dcAddress").text(data.header.shipping_Address);
   // $("#dcCity").text(data.header.city);
  //  $("#dcPin").text(data.header.pin);

    //================ Items =================//

    var tbody = $("#dcItemsBody tbody");

    tbody.empty();
    tbody.html("");

    if (data.items.length === 0) {

        tbody.append(`
            <tr>
                <td colspan="4" class="text-center">
                    No Items Found
                </td>
            </tr>`);

        return;
    }
    var totalQty = 0;
    for (var i = 0; i < data.items.length; i++) {

        data.items[i].slno = i + 1;
        data.items[i].remarks = "";
        totalQty += parseFloat(data.items[i].qntyDispatched || 0);
        tbody.append(
            AppUtil.ProcessTemplateData("dcItemRow", data.items[i])
        );
    }
    $("#dcTotalQty").text(totalQty);
}
$(document).on("click", "#btnPrintDCFinal", function () {

    var printContents = $("#DCPreview").prop("outerHTML");

    var printWindow = window.open("", "_blank");

    printWindow.document.write(`
<html>
<head>
    <title>Delivery Challan</title>

    <!-- Your site's Bootstrap -->
    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css">

    <!-- Your site CSS -->
    <link rel="stylesheet" href="/css/site.css">

    <!-- Any other CSS you already use -->
    <link rel="stylesheet" href="/css/custom.css">

    <style>

        @page{
            size:A4 portrait;
            margin:10mm;
        }

        html,body{
            width:210mm;
            min-height:297mm;
            margin:0;
            padding:0;
            font-size:13px;
            color:#000;
            background:#fff;
        }

        #DCPreview{
            width:100%;
            margin:0 auto;
            border:none !important;
            box-shadow:none !important;
        }

        table{
            width:100%;
            border-collapse:collapse;
        }

        table,th,td{
            border:1px solid #000 !important;
        }

        img{
            max-width:100%;
        }

        @media print{

            body{
                margin:0;
            }

            .table{
                --bs-table-border-color:#000;
            }

        }

    </style>

</head>

<body>

${printContents}

</body>

</html>
`);

    printWindow.document.close();

    setTimeout(function () {
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }, 500);

});

$('#editDispatchAddressUpdateModal').on('shown.bs.modal', function () {

    loadDispatchAddressUpdate();

});
function loadDispatchAddressUpdate() {

    var headerId = $("#updateHeaderId").val();
    var indentId = $("#updIndentNo").text();   // Hidden field inside Update popup

    api.getbulk("/Gro/GetDispatchAddress?headerId=" +
        encodeURIComponent(headerId) +
        "&indentId=" +
        encodeURIComponent(indentId))

        .then(function (data) {

            var header = data.header;
            var groData = data.result;

            $("#UpdateHeaderId2").val(header.gro_Disp_HeaderId);

            $("#updAddrIndentDate").text(groData.indentDateStr);
            $("#updAddrIndentNo").text(groData.indent);

            $("#updAddrContact").text(groData.contact_Person);
            $("#updAddrCellPhone").text(groData.contact_Person_No);
            $("#updExecutiveName").text(groData.excutive_Name);

            $("#txtCustomer2").val(groData.company_Name);
            $("#txtAddress2").val(header.shipping_Address);
            $("#txtCity2").val(header.shipping_City);
            $("#txtPin2").val(header.shipping_PINCODE);

        })
        .catch(function (err) {

            console.log(err);

        });

}
$("#btnSaveDispatchAddress2").click(function () {

    var model = {

        gro_Disp_HeaderId: $("#UpdateHeaderId2").val(),

        shipping_Address: $("#txtAddress2").val(),

        shipping_City: $("#txtCity2").val(),

        shipping_Pincode: $("#txtPin2").val()

    };

    api.post("/Gro/UpdateDispatchAddress", model)

        .then(function () {

            $("#editDispatchAddressUpdateModal").modal("hide");

            // Reload Update popup data
            loadDispatchDataByIndent();
            LoadDispatchDetails($("#updateHeaderId").val(), $("#updAddrIndentNo").val(), "UPDATE");

        })

        .catch(function (err) {

            console.log(err);

        });

});
$("#btnOpenDCPrintUpdate").click(function () {

    var headerId = $("#updateHeaderId").val();

    api.getbulk("/Gro/GetDCPrintData?headerId=" + headerId)

        .then(function (data) {

            bindDCPrintUpdate(data);

        })

        .catch(function (err) {

            console.log(err);

        });

});
function bindDCPrintUpdate(data) {

    $("#dcIndentDateUpd").text(data.header.indentDateStr);
    $("#dcIndentNoUpd").text(data.header.indent);

    $("#dcDateUpd").text(data.header.headerDatestr);
    $("#dcNoUpd").text(data.header.gro_Disp_HeaderId);
    
    $("#dcCustomerUpd").text(data.header.company_Name);
    $("#dcAddressUpd").text(data.header.shipping_Address);

    $("#dctransportUpd").text(data.header.courierName);
    $("#approx").text("₹ " + data.approxValue.toFixed(2));
    var tbody = $("#dcItemsBodyUpd tbody");

    tbody.empty();

    if (data.items.length == 0) {

        tbody.append(`
            <tr>
                <td colspan="4">No Items Found</td>
            </tr>`);

        return;
    }

    var totalQty = 0;

    for (var i = 0; i < data.items.length; i++) {

        data.items[i].slno = i + 1;
        data.items[i].remarks = "";

        totalQty += parseFloat(data.items[i].qntyDispatched || 0);

        tbody.append(

            AppUtil.ProcessTemplateData(
                "dcItemRowUpd",
                data.items[i]
            )

        );

    }

    $("#dcTotalQtyUpd").text(totalQty);

}
$(document).on("click", "#btnPrintDCFinalUpd", function () {

    var printContents = $("#DCPreviewUpd").prop("outerHTML");

    var printWindow = window.open("", "_blank");

    printWindow.document.write(`
<html>

<head>

<title>Delivery Challan</title>

<link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css">

<link rel="stylesheet" href="/css/site.css">

<link rel="stylesheet" href="/css/custom.css">

<style>

@page{
    size:A4 portrait;
    margin:10mm;
}

html,body{

    width:210mm;
    min-height:297mm;

    margin:0;
    padding:0;

    font-size:13px;

    color:#000;

    background:#fff;

}

#DCPreviewUpd{

    width:100%;
    border:none !important;

}

table{

    width:100%;
    border-collapse:collapse;

}

table,th,td{

    border:1px solid #000 !important;

}
.text-start{
    text-align:left !important;
}

img{

    max-width:100%;

}

</style>

</head>

<body>

${printContents}

</body>

</html>
`);

    printWindow.document.close();

    setTimeout(function () {

        printWindow.focus();

        printWindow.print();

        printWindow.close();

    }, 500);

});
$("#btnPrintInvoice").click(function () {

    var headerId = $("#hdnGroDispHeaderId").val();

    api.getbulk("/Gro/GetInvoicePrintData?headerId=" + headerId)

        .then(function (data) {

            bindInvoice(data);

        })

        .catch(function (err) {

            console.log(err);

        });

});
function bindInvoice(data) {

    //---------------- Company / Customer ----------------//

    $("#invCompanyName").text(data.header.company_Name);

    $("#invAddress").text(data.header.shipping_Address);

    $("#invCity").text(data.header.shipping_City);

    $("#invPin").text(data.header.shipping_PINCODE);

    //---------------- Invoice Header ----------------//

    $("#invInvoiceNo").text(data.header.gro_Disp_HeaderId);

    $("#invInvoiceDate").text(data.header.headerDatestr);

    $("#invOrderNo").text(data.header.indent);
    $("#invBuyerOrderNo").text(data.header.indent);

    $("#invOrderDate").text(data.header.indentDateStr);

    $("#invTransport").text(data.header.courier_Name);

    //---------------- Items ----------------//

    var tbody = $("#invoiceItemsBody");
    tbody.empty();

    var taxSummary = {};

    var subTotal = 0;
    var gstTotal = 0;
    var grandTotal = 0;

    $.each(data.details, function (i, item) {

        tbody.append(
            AppUtil.ProcessTemplateData("invoiceItemRow", item)
        );

        subTotal += parseFloat(item.taxableAmount);
        gstTotal += parseFloat(item.gstAmount || 0);
        grandTotal += parseFloat(item.totalAmount);

        // Group by HSN + GST Rate
        var key = item.hsnCode + "_" + item.gstRate;

        if (!taxSummary[key]) {

            taxSummary[key] = {

                hsnCode: item.hsnCode,
                gstRate: item.gstRate,
                taxableValue: 0,
                gstAmount: 0,
                totalTaxAmount: 0
            };
        }

        taxSummary[key].taxableValue += parseFloat(item.taxableAmount || 0);
        taxSummary[key].gstAmount += parseFloat(item.gstAmount || 0);
        taxSummary[key].totalTaxAmount += parseFloat(item.gstAmount || 0);

        
    });

    $("#invSubTotal").text(subTotal.toFixed(2));
    $("#invGSTAmount").text(gstTotal.toFixed(2));
    $("#invGrandTotal").text(grandTotal.toFixed(2));
    $("#invAmountWords").text(data.amountInWords);
    $("#invTaxAmountWords").text(data.taxAmountInWords);
    // Bind HSN Summary
    $("#invoiceTaxBody").empty();

    $.each(taxSummary, function (key, item) {

        item.taxableValue = item.taxableValue.toFixed(2);
        item.gstAmount = item.gstAmount.toFixed(2);
        item.totalTaxAmount = item.totalTaxAmount.toFixed(2);

        $("#invoiceTaxBody").append(
            AppUtil.ProcessTemplateData("invoiceTaxRow", item)
        );

    });

}
$(document).on("click", "#btnPrintInvoiceFinal", function () {

    var printContents = $("#InvoicePreview").prop("outerHTML");

    var printWindow = window.open("", "_blank");

    printWindow.document.write(`
<html>

<head>

    <title>Tax Invoice</title>

    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css">

    <link rel="stylesheet" href="/css/site.css">

    <link rel="stylesheet" href="/css/custom.css">

    <style>

       @page{
    size:A4 portrait;
    margin:8mm;
}

html,body{
    width:210mm;
    min-height:297mm;
    margin:0;
    padding:0;
    background:#fff;
    color:#000;
    font-family:Arial, Helvetica, sans-serif;
    font-size:12px;
    font-weight:600;     /* Make all printed text bold */
}

#InvoicePreview{
    width:100%;
    margin:0 auto;
    border:2px solid #000 !important;   /* Outer border */
    box-shadow:none !important;
    padding:8px !important;
}

table{
    width:100%;
    border-collapse:collapse !important;
}

table,
th,
td{
    border:1.5px solid #000 !important;   /* Dark borders */
}

th{
    font-weight:700 !important;
    text-align:center;
    background:#fff !important;
}

td{
    font-weight:600 !important;
    vertical-align:top;
    padding:4px 6px !important;
}

strong{
    font-weight:700 !important;
}

span{
    font-weight:600 !important;
}

p{
    margin-bottom:4px;
}

hr{
    border:1px solid #000 !important;
    opacity:1 !important;
}

h1,h2,h3,h4,h5,h6{
    font-weight:700 !important;
}

.text-end{
    text-align:right !important;
}

.text-center{
    text-align:center !important;
}

.table-borderless,
.table-borderless td,
.table-borderless th{
    border:none !important;
}

@media print{

    body{
        margin:0;
        -webkit-print-color-adjust:exact;
        print-color-adjust:exact;
    }

    .table{
        --bs-table-border-color:#000;
    }

    table{
        page-break-inside:auto;
    }

    tr{
        page-break-inside:avoid;
    }

    thead{
        display:table-header-group;
    }

    tfoot{
        display:table-footer-group;
    }
}

    </style>

</head>

<body>

${printContents}

</body>

</html>
`);

    printWindow.document.close();

    setTimeout(function () {

        printWindow.focus();

        printWindow.print();

        printWindow.close();

    }, 500);

});

$("#btnPrintInvoiceUpdate").click(function () {

    var headerId = $("#updateHeaderId").val();

    api.getbulk("/Gro/GetInvoicePrintData?headerId=" + encodeURIComponent(headerId))

        .then(function (data) {

            bindInvoice(data);

        })

        .catch(function (err) {

            console.log(err);

        });

});
$("#btnUploadInvoice").click(function () {

    api.getbulk("/Gro/GetPendingInvoicesForUpload")
        .then(function (data) {

            //console.log(data);

            //data.forEach(function (invoice) {

            //    console.log(invoice.header);

            //    console.log(invoice.details);

            //});

        })
        .catch(function (err) {

            console.log(err);

        });

});
$("#btnPrintAddressLabel").click(function () {

    $("#lblCustomerName").text($("#updCustomer").text());
    $("#lblCustomerAddress").html($("#updAddress").html());
    $("#lblCustomerMobile").text($("#updMobile").text());

});
$(document).on("click", "#btnPrintAddressLabelFinal", function () {

    var printContents = $("#AddressLabelPreview").prop("outerHTML");

    var printWindow = window.open("", "_blank");

    printWindow.document.write(`
<html>

<head>

    <title>Address Label</title>

    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css">

    <link rel="stylesheet" href="/css/site.css">

    <link rel="stylesheet" href="/css/custom.css">

    <style>

        @page{
            size:A4 portrait;
            margin:15mm;
        }

        html,body{
            margin:0;
            padding:0;
            background:#fff;
            font-family:Arial, Helvetica, sans-serif;
            font-size:20px;
            line-height:1.8;
        }

        #AddressLabelPreview{
            width:100%;
            padding:20px;
        }

        #lblCustomerName{
            font-size:24px;
            font-weight:bold;
            margin-bottom:20px;
        }

        #lblCustomerAddress{
            white-space:pre-line;
            margin-bottom:20px;
        }

        #lblCustomerMobile{
            font-weight:bold;
        }

    </style>

</head>

<body>

${printContents}

</body>

</html>
`);

    printWindow.document.close();

    setTimeout(function () {

        printWindow.focus();

        printWindow.print();

        printWindow.close();

    }, 500);

});