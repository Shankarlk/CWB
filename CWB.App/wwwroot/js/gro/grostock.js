
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
    
}
$("#txtQtyToAdd").on("input change", function () {

    

    var qty = parseInt($(this).val()) || 0;

    if (qty > 0) {
        $("#btnPrintLabels").show();
    } else {
        $("#btnPrintLabels").hide();
    }
});
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

               
                $("#pendingLabelsModal").modal('show');
                return;
            }
            $("#qrContainer").empty();

            $.each(response.labels, function (index, item) {

                var card = `
            <div class="col-md-6">

                <div class="border p-2 text-center">

                 

                    <div>${item.serialNo}</div>

                    <img
                        src="data:image/png;base64,${item.qrCode}"
                        width="180"
                        height="180" />

                </div>

            </div>
        `;

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

                body{
                    margin:10px;
                    font-family:Arial;
                }

                .row{
                    display:flex;
                    flex-wrap:wrap;
                }

                .col-md-6{
                    width:50%;
                    box-sizing:border-box;
                    padding:10px;
                }

                .border{
                    border:1px solid #000;
                }

                .text-center{
                    text-align:center;
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

    $("#btnScanLabels").show();

    $(this).hide();     // optional

    toastr.success("Now scan the labels.");

});
$("#btnScanLabels").click(function () {

    var groPartListId = $("#hdnGroPartListId").val();

    if (!groPartListId) {
        alert("Gro Part not selected.");
        return;
    }

    api.getbulk("/Gro/ScanLabels?groPartListId=" + encodeURIComponent(groPartListId))
        .then(function (data) {

            if (data.success) {

                $("#txtBoxesScanned").val(data.scannedCount);

                toastr.success("Labels scanned successfully.");
            }
            else {

                toastr.error(data.message);
            }
        })
        .catch(function (err) {

            console.log(err);
            toastr.error("Error while scanning labels.");
        });
});

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

                $("#lblQtyAdded").text(data.qtyAdded);
                $("#lblUpdatedStock").text(data.updatedStock);

                toastr.success("Stock updated successfully.");
                loadGroPartWithStock();
                loadGroPartWithoutStock();
                $("#updateStockModal").modal('hide');
 
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







