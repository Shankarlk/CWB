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
    loadDispatchAgeingSummary();
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
 
 
function openDeliveryAgeing(ageing) {

    deliveryAgeing = ageing;

    $("#deliverydataupdatemodal").modal("show");
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

            row.toggle(matchIndent && matchFrom && matchTo && matchStatus && matchAgeing);

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

            updateDispatchButton();

        });

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
$('#dispatchQtyModal').on('hidden.bs.modal', function () {

    loadDispatchSelection();
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
    loadDispatchAgeingSummary();
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
            (Number(data[i].qntyAval) > 0 && Number(data[i].bal_to_Disp) > 0) ? "" : "display:none;";
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
            $("#btnSaveExitDispatch").hide();
        })

        .catch(function (error) {

            console.log(error);

            // AppUtil.MessageBox("Unable to load dispatch details.", 2);

        });

}

//$("#btnScanLabel").click(function () {

//    var model = {

//        gro_Disp_Det_Id: $("#hdnDispDetailId").val(),

//        requiredQty: parseInt($("#txtBalanceDispatch").val())
//    };

//    api.post("/Gro/ScanDispatchLabels", model)

//        .then(function (data) {

//            $("#txtScannedQty").val(data.scannedQty);

//            $("#txtBalanceDispatch").val(data.balanceQty);

//            $("#txtDispatchStatus").val(data.message)

//            //if (data.disableScanner) {

//            //    $("#btnScanLabel").prop("disabled", true);

//            //    $("#btnSaveExitDispatch")
//            //        .removeClass("btn-secondary")
//            //        .addClass("btn-primary");
//            //}

//        })

//        .catch(function (error) {

//            console.log(error);

//            // AppUtil.MessageBox("Unable to assign stock.", 2);

//        });

//});

$("#btnScanLabel").click(function () {

    $("#txtDispatchQRCode")
        .prop("readonly", false)
        .focus();
    if ($("#txtDispatchScannedQRCode").prop("readonly"))
        return;

    $("#txtDispatchScannedQRCode")
        .focus();
});
$("#txtDispatchScannedQRCode").on("keypress", function (e) {

    if (e.which == 13) {

        e.preventDefault();

        var qr = $(this).val().trim();

        if (qr == "")
            return;

        scanDispatchLabel(qr);

        $(this).val("");

    }

});
function scanDispatchLabel(qrCode) {

    var model = {

        gro_Disp_Det_Id: $("#hdnDispDetailId").val(),

        qrCode: qrCode

    };

    api.post("/Gro/ScanDispatchLabels", model)

        .then(function (data) {

            if (data.showPrintedPopup) {

                $("#lblPartNo").text($("#liPartNo").text());

                $("#lblSerialNo").text(data.serialNo);

                $("#hdnPrintedStockDetId").val(data.groStockDetId);

                $("#lineItemDispatchQtyModal").modal("hide");

                $("#labelNotInStockModal").modal("show");
                $("#btnForceAssign").hide();

                $("#btnConfirmSingle").prop("disabled", false);

                $("#btnSkipPart").prop("disabled", false);
                return;
            }

            if (!data.success) {

                toastr.warning(data.message);

                $("#txtDispatchScannedQRCode")
                    .val("")
                    .focus();

                return;
            }

            $("#txtScannedQty").val(data.scannedQty);

            $("#txtBalanceDispatch").val(data.balanceQty);

            $("#txtDispatchStatus").val(data.message);

            if (parseInt(data.scannedQty) > 0)
                $("#btnSaveExitDispatch").show();

            checkDispatchCompletion();

            $("#txtDispatchScannedQRCode")
                .val("")
                .focus();

        })

        .catch(function (err) {

            console.log(err);

        });

}
function checkDispatchCompletion() {

    var scanned = parseInt($("#txtScannedQty").val()) || 0;

    var balance = parseInt($("#txtBalanceDispatch").val()) || 0;

    var qtyOnHand = parseInt($("#liQtyOnHand").text()) || 0;

    if (balance == 0) {

        $("#txtDispatchStatus").val(
            "Balance to Dispatch Quantity Scanned - Save & Exit"
        );

        $("#txtDispatchScannedQRCode")
            .prop("readonly", true);

        $("#btnScanLabel")
            .prop("disabled", true);

        $("#btnSaveExitDispatch")
            .prop("disabled", false);

        return;
    }

    if (scanned >= qtyOnHand) {

        $("#txtDispatchStatus").val(
            "Available Stock is scanned - Save & Exit"
        );

        $("#txtDispatchScannedQRCode")
            .prop("readonly", true);

        $("#btnScanLabel")
            .prop("disabled", true);

        $("#btnSaveExitDispatch")
            .prop("disabled", false);

        return;
    }

    $("#txtDispatchStatus").val("Continue Scanning");

    $("#txtDispatchScannedQRCode")
        .prop("readonly", false);

    $("#btnScanLabel")
        .prop("disabled", false);
}
var dispatchSaved = false;
$("#btnConfirmSingle").click(function () {

    $("#btnForceAssign").show();

    $("#btnConfirmSingle").prop("disabled", true);

});

$("#btnForceAssign").click(function () {

    var model = {

        groDispDetId: $("#hdnDispDetailId").val(),  

        groStockDetId: $("#hdnPrintedStockDetId").val()

    };

    api.post("/Gro/ForceAssignPrintedLabel", model)

        .then(function (data) {

            if (!data.success) {

                toastr.error(data.message);

                return;

            }

            $("#txtScannedQty").val(data.scannedQty);

            $("#txtBalanceDispatch").val(data.balanceQty);

            $("#txtDispatchStatus").val(data.message);

            if (parseInt(data.scannedQty) > 0)
                $("#btnSaveExitDispatch").show();
            $("#liQtyOnHand").text(data.qtyOnHand); 
            checkDispatchCompletion();

            $("#labelNotInStockModal").modal("hide");

            $("#lineItemDispatchQtyModal").modal("show");

            $("#txtDispatchScannedQRCode")
                .val("")
                .focus();

        });

});
$("#btnSkipPart").click(function () {

    $("#labelNotInStockModal").modal("hide");

    $("#lineItemDispatchQtyModal").modal("show");

    $("#txtDispatchScannedQRCode")
        .val("")
        .focus();

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
            dispatchSaved = true;
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
//popup 4.1 or Popup3
$("#lineItemDispatchQtyModal").on("hidden.bs.modal", function () {

    if (dispatchSaved) {


        dispatchSaved = false;

        return;
    }

    var scanned = parseInt($("#txtScannedQty").val()) || 0;

    if (scanned == 0)
        return;

    loadDispatchPendingSession();

    $("#scanNotSavedModal").modal("show");

});

function loadDispatchPendingSession() {

    $.get("/Gro/GetDispatchPendingSession",
        {
            groDispDetId: $("#hdnDispDetailId").val()
        },
        function (res) {

            if (!res.success)
                return;

            $("#unsavedPartNo").text($("#liPartNo").text());

            $("#unsavedBalance").text($("#txtBalanceDispatch").val());

            $("#unsavedQty").text(res.scannedQty);

            $("#txtUnsavedSlNos").val(res.serialNos.join("\n"));

        });

}
$("#btnReturnScanning").click(function () {

    $("#scanNotSavedModal").modal("hide");

    $("#lineItemDispatchQtyModal").modal("show");

    $("#txtDispatchScannedQRCode").focus();

});
$("#btnAcceptDispatch").click(function () {

    $("#scanNotSavedModal").modal("hide");

    $("#btnSaveExitDispatch").click();

});

$("#btnCancelScanning").click(function () {

    $.post("/Gro/CancelDispatchScan",
        {
            groDispDetId: $("#hdnDispDetailId").val()
        },
        function (res) {

            if (!res.success)
                return;

            $("#scanNotSavedModal").modal("hide");

            loadDispatchData($("#hdnIndentId").val());

        });

});










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

    $("#dcDate").text(data.header.dispatchDateStr);
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

    $("#dcDateUpd").text(data.header.dispatchDateStr);
    $("#dcNoUpd").text(data.header.gro_Disp_HeaderId);
    $("#approx").text("₹ " + data.approxValue.toFixed(2));
    $("#dcCustomerUpd").text(data.header.company_Name);
    $("#dcAddressUpd").text(data.header.shipping_Address);

    $("#dctransportUpd").text(data.header.courierName);

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

    $("#invInvoiceDate").text(data.header.dispatchDateStr);

    $("#invOrderNo").text(data.header.indent);

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
$("#btnPrintAddressLabel").click(function () {

    $("#lblCustomerName").text($("#Customer").text());
    $("#lblCustomerAddress").html($("#Address").html());
    $("#lblCustomerMobile").text($("#Mobile").text());

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
        size:100mm 80mm;
        margin:0;
    }

    html,body{
        width:100mm;
        height:80mm;
        margin:0;
        padding:0;
        background:#fff;
        font-family:Arial, Helvetica, sans-serif;
        overflow:hidden;
    }

    #AddressLabelPreview{
        width:100mm;
        height:80mm;
        box-sizing:border-box;
        padding:0mm;
    }

    #lblCustomerName{
        font-size:22px;
        font-weight:bold;
        margin-bottom:8px;
        line-height:1.2;
    }

    #lblCustomerAddress{
        font-size:18px;
        line-height:1.4;
        white-space:pre-line;
        margin-bottom:8px;
    }

    #lblCustomerMobile{
        font-size:18px;
        font-weight:bold;
        line-height:1.3;
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