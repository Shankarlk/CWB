
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

    loadGroPartWithStock();
    loadGroPartWithoutStock();
    $("#txtWithStockSearch").on("keyup", filterWithStock);

    $("#txtZeroStockSearch").on("keyup", filterZeroStock);


});
function filterWithStock() {

    var search = $("#txtWithStockSearch").val().toLowerCase().trim();

    $("#tblWithStock tbody tr").each(function () {

        var row = $(this);

        var part = row.find("td:eq(1)").text().toLowerCase().trim();

        row.toggle(search === "" || part.indexOf(search) !== -1);

    });

}
function filterZeroStock() {

    var search = $("#txtZeroStockSearch").val().toLowerCase().trim();

    $("#tblZeroStock tbody tr").each(function () {

        var row = $(this);

        var part = row.find("td:eq(1)").text().toLowerCase().trim();

        row.toggle(search === "" || part.indexOf(search) !== -1);

    });

}
function loadGroPartWithStock() {

    api.getbulk("/Gro/GetGroPartWithStock").then((data) => {

        var tablebody = $("#tblWithStock tbody");
        $(tablebody).html("");

        if (data.length === 0) {

            $(tablebody).append(`
                <tr class="norecordsfound">
                    <td colspan="3" class="text-center text-muted">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`);

            return;
        }

        for (var i = 0; i < data.length; i++) {

            $(tablebody).append(AppUtil.ProcessTemplateData("groPartWithStockRow", data[i]));

        }

    }).catch((error) => {

    });

}
function loadGroPartWithoutStock() {

    api.getbulk("/Gro/GetGroPartWithoutStock").then((data) => {

        var tablebody = $("#tblZeroStock tbody");
        $(tablebody).html("");

        if (data.length === 0) {

            $(tablebody).append(`
                <tr class="norecordsfound">
                    <td colspan="2" class="text-center text-muted">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`);

            return;
        }

        for (var i = 0; i < data.length; i++) {

            $(tablebody).append(AppUtil.ProcessTemplateData("groPartWithoutStockRow", data[i]));

        }

    }).catch((error) => {

    });

}
function LoadUpdateStock(ctrl) {

    var tr = $(ctrl).closest("tr");
    $("#btnPrintLabels").hide();
    $("#hdnGroPartListId").val(tr.find("td:eq(0)").text().trim());
    $("#txtGroPartNo").val(tr.find("td:eq(1)").text().trim());
    $("#txtCurrentStock").val(tr.find("td:eq(2)").text().trim());
    $("#btnPrintLabels").hide();
    $("#btnLabelsStuck").hide();
    $("#btnScanLabels").hide();
    $("#txtScannedQRCode").prop("disabled", true);
    $("#txtQtyToAdd").val(0);
    $("#txtBoxesScanned").val(0);
    $("#divSuccessBox").hide();
}
$("#txtQtyToAdd").on("input change", function () {

    

    var qty = parseInt($(this).val()) || 0;

    if (qty > 0) {
        $("#btnPrintLabels").show();
    } else {
        $("#btnPrintLabels").hide();
    }
    checkScanCompletion();
});
var printed = false;
$("#btnPrintLabels").click(function () {

    var partNo = $("#txtGroPartNo").val();
    var qty = $("#txtQtyToAdd").val();

    var serialNo = "SR000001";

    api.post("/Gro/GenerateQrCode", {
        GroPartNo: partNo,
        Qty: qty
    })
        .then((response) => {
            if (!response.success) {

                $("#pendingLabelsMessage")
                    .text(response.message);

                printed = true;
                $("#pendingLabelsModal").modal('show');
                return;
            }
            $("#qrContainer").empty();

            $.each(response.labels, function (index, item) {

        //        var card = `
        //    <div class="col-md-6">

        //        <div class="border p-2 text-center">

                 

        //            <div>${item.serialNo}</div>

        //            <img
        //                src="data:image/png;base64,${item.qrCode}"
        //                width="180"
        //                height="180" />

        //        </div>

        //    </div>
        //`;
                var card = `
                <div class="qr-label">

                    <div class="qr-left">
                        <img src="data:image/png;base64,${item.qrCode}" />
                    </div>

                    <div class="qr-right">
                        <div>Part No : ${item.groPartNo}</div>
                        <div>MRP : ${item.mrp}</div>
                        <div>SL No : ${item.serialNo}</div>
                    </div>

                </div>`;

                $("#qrContainer").append(card);

            });

           
            $("#printLabelModal").modal('show');
            $("#btnLabelsStuck").show();

        })
        .catch((error) => {

            console.log(error);

        });

});
 //  <h6>${item.groPartNo}</h6>
//design for QR code 
$("#btnPrint").click(function () {

    var printContents =
        $("#printHelper").html();

    var printWindow =
        window.open(
            "",
            "_blank",
            "width=900,height=700"
        );

    printWindow.document.write(`
        <html>
        <head>

            <title>Print Labels</title>

           <style>

    @page{
        size:75mm 25mm;
        margin:0;
    }

    html,body{
        margin:0;
        padding:0;
        font-family:Arial, Helvetica, sans-serif;
    }

    #qrContainer{
        margin:0;
        padding:0;
    }

    .qr-label{
        width:75mm;
        height:25mm;
        display:flex;
        border:1px solid #000;
        box-sizing:border-box;
        
    }
    .qr-label:not(:last-child){
    page-break-after: always;
}
    .qr-left{
        width:25mm;
        height:25mm;
        display:flex;
        justify-content:center;
        align-items:center;
        border-right:1px solid #000;
    }

    .qr-left img{
        width:22mm;
        height:22mm;
    }

    .qr-right{
        width:50mm;
        padding:2mm;
        box-sizing:border-box;
        display:flex;
        flex-direction:column;
        justify-content:center;
        font-size:10pt;
        line-height:1.2;
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

    printWindow.onload = function () {

        printWindow.focus();

        printWindow.print();

        printWindow.close();

    };

});
$("#btnLabelsStuck").click(function () {
    $("#txtScannedQRCode")
        .prop("disabled", false)
        .val("")
        .focus();
    $("#btnScanLabels").show();

    $(this).hide();     // optional

    toastr.success("Now scan the labels.");

});
$("#txtScannedQRCode").on("keypress", function (e) {

    if (e.which == 13) {

        e.preventDefault();

        var qrCode = $(this).val().trim();

        if (qrCode == "")
            return;

        var groPartListId = $("#hdnGroPartListId").val();

        $.ajax({
            url: "/Gro/ScanLabels",
            type: "GET",
            data: {
                groPartListId: groPartListId,
                qrCode: qrCode
            },
            success: function (data) {

                if (data.success) {

                    $("#txtBoxesScanned").val(data.scannedCount);
                    checkScanCompletion();
                    toastr.success(data.message);
                }
                else {

                    toastr.warning(data.message);
                }

                $("#txtScannedQRCode").val("").focus();
            },
            error: function () {

                toastr.error("Error while scanning label.");
                $("#txtScannedQRCode").focus();
            }
        });   // Temporary - we'll replace this with an AJAX call

       // $(this).val("");

    }

});
function checkScanCompletion() {

    var qtyToAdd = parseInt($("#txtQtyToAdd").val()) || 0;
    var scanned = parseInt($("#txtBoxesScanned").val()) || 0;

    if (qtyToAdd > 0 && qtyToAdd === scanned) {

        $("#divSuccessBox").show();

    }
    else {

        $("#divSuccessBox").hide();

    }
}
var dispatchSaved = false;
$("#btnSaveExitStock").click(function () {

    var groPartListId = $("#hdnGroPartListId").val();

    if (!groPartListId) {
        toastr.error("Part not selected.");
        return;
    }

    api.getbulk("/Gro/UpdateStockFromScannedLabels?groPartListId="
        + encodeURIComponent(groPartListId))
        .then(function (data) {

            if (data.success) {

                $("#lblQtyAdded").text(data.qtyAdded);
                $("#lblUpdatedStock").text(data.updatedStock);

                toastr.success("Stock updated successfully.");
                loadGroPartWithStock();
                loadGroPartWithoutStock();
                printed = false;
                $("#updateStockModal").modal('hide');
                dispatchSaved = true;
            }
            else {

                toastr.error(data.message);
            }

        })
        .catch(function (err) {

            console.log(err);
            toastr.error("Error while updating stock.");
        });

});

//start of popup2

//pending lables Rescan or Delete
function loadPendingLabels() {

    var groPartListId = $("#hdnGroPartListId").val();

    $.get("/Gro/GetPendingPrintedLabels",
        {
            groPartListId: groPartListId
        },
        function (res) {

            if (!res.success)
                return;

            $("#lblPendingLabels").text(res.pendingCount);

            $("#lblQtyEntered").text(res.qtyEntered);

            $("#lblStickerCount").text(res.stickerCount);

            $("#txtNotScanned").val(res.serialNos.join("\n"));
            if (res.pendingCount == 0) {

                $("#btnSaveExit").show();

                $("#btnRescan").hide();
                $("#btndeletelabel").hide();
                $("#btnConfirmDelete").hide();
                $("#btnConfirmDestroy").hide();

            }
            else {

                $("#btnSaveExit").hide();

            }

        });

}
$("#updateStockModal").on("hidden.bs.modal", function () {
    if (dispatchSaved) {


        dispatchSaved = false;

        return;
    }
    if (!printed)
        return;
    resetDeleteLabelsModal();
    loadPendingLabels();

    $("#deleteLabelsModal").modal("show");
    $("#btnConfirmDelete").hide();
    $("#btnConfirmDestroy").hide();
    $("#btnSaveExit").hide();

});

$("#btndeletelabel").click(function () {

    $("#btnConfirmDelete").show();

    $("#btndeletelabel").prop("disabled", true);
    $("#btnRescan").prop("disabled", true);

});
$("#btnConfirmDelete").click(function () {

    $("#btnConfirmDestroy").show();

    $("#btnConfirmDelete").prop("disabled", true);

});

$("#btnConfirmDestroy").click(function () {

    $.post("/Gro/DeletePendingLabels",
        {
            groPartListId: $("#hdnGroPartListId").val()
        },
        function (res) {

            if (!res.success) {

                toastr.error(res.message);
                return;
            }

            toastr.success("Pending labels deleted.");

            loadPendingLabels();

        });

});


//$("#btnScanLabels").click(function () {

//    var groPartListId = $("#hdnGroPartListId").val();

//    if (!groPartListId) {
//        alert("Gro Part not selected.");
//        return;
//    }

//    api.getbulk("/Gro/ScanLabels?groPartListId=" + encodeURIComponent(groPartListId))
//        .then(function (data) {

//            if (data.success) {

//                $("#txtBoxesScanned").val(data.scannedCount);

//                toastr.success("Label scanned successfully.");
//            }
//            else {

//                toastr.error(data.message);
//            }
//        })
//        .catch(function (err) {

//            console.log(err);
//            toastr.error("Error while scanning labels.");
//        });
//});



$("#btnRescan").click(function () {

    $("#txtReScannedQRCode")
        .prop("readonly", false)
        .focus();
    $("#btndeletelabel").prop("disabled", true);
});

$("#txtReScannedQRCode").on("keydown", function (e) {

    if (e.which == 13) {

        e.preventDefault();

        var qr = $(this).val().trim();

        rescanLabel(qr);

        $(this).val("");

    }

});
function rescanLabel(qrCode) {

    $.ajax({

        url: "/Gro/ReScanLabel",

        type: "POST",

        data: {
            groPartListId: $("#hdnGroPartListId").val(),
            qrCode: qrCode
        },

        success: function (res) {

            if (res.success) {

                loadPendingLabels();

                toastr.success(res.message);

            }
            else {

                toastr.warning(res.message);

            }

            $("#txtReScannedQRCode").focus();

        }

    });

}
$("#btnSaveExit").click(function () {

    var groPartListId = $("#hdnGroPartListId").val();

    if (!groPartListId) {
        toastr.error("Part not selected.");
        return;
    }

    api.getbulk("/Gro/UpdateStockFromScannedLabels?groPartListId="
        + encodeURIComponent(groPartListId))
        .then(function (data) {

            if (data.success) {

                //$("#lblQtyAdded").text(data.qtyAdded);
                //$("#lblUpdatedStock").text(data.updatedStock);

                toastr.success("Stock updated successfully.");
                loadGroPartWithStock();
                loadGroPartWithoutStock();
                $("#deleteLabelsModal").modal('hide');
                $("#updateStockModal").modal("hide");
            }
            else {

                toastr.error(data.message);
            }

        })
        .catch(function (err) {

            console.log(err);
            toastr.error("Error while updating stock.");
        });

});

function resetDeleteLabelsModal() {

    // Clear text
    $("#txtReScannedQRCode").val("").prop("readonly", true);
    $("#txtNotScanned").val("");

    $("#lblPendingLabels").text("0");
    $("#lblQtyEntered").text("0");
    $("#lblStickerCount").text("0");

    // Show default buttons
    $("#btnRescan").show().prop("disabled", false);
    $("#btndeletelabel").show().prop("disabled", false);

    // Hide confirmation buttons
    $("#btnConfirmDelete").hide().prop("disabled", false);
    $("#btnConfirmDestroy").hide();

    // Hide Save & Exit
    $("#btnSaveExit").hide();
}
$("#deleteLabelsModal").on("hidden.bs.modal", function () {

    resetDeleteLabelsModal();

});
