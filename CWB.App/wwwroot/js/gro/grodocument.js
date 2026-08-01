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


function OpenInvoiceDeletePopup() {

    $("#txtDeleteIndent").val("");

    api.getbulk("/Gro/GetInvoiceDeleteData")

        .then(function (data) {

            var tbody = $("#tblInvoiceDelete tbody");

            tbody.empty();

            if (data.length == 0) {

                tbody.append(`
                    <tr>
                        <td colspan="6" class="text-center">
                            No Records Found
                        </td>
                    </tr>`);

            } else {

                for (var i = 0; i < data.length; i++) {

                    tbody.append(
                        AppUtil.ProcessTemplateData(
                            "invoiceDeleteRow",
                            data[i]
                        )
                    );
                }
            }

            $("#groInvoiceDeleteModal").modal("show");

        })

        .catch(function (err) {

            console.log(err);

        });

}
$("#txtDeleteIndent").on("keyup", function () {

    var value = $(this).val().toLowerCase();

    $("#tblInvoiceDelete tbody tr").each(function () {

        var indent = $(this).find("td").eq(1).text().toLowerCase();

        $(this).toggle(indent.indexOf(value) > -1);

    });

});

function DeleteDispatchHeader(headerId) {

    api.getbulk("/Gro/GetDeleteDispatchHeaderDetails?headerId=" + headerId)

        .then(function (data) {

            $("#delIndentDate").text(data.header.indentDateStr);
            $("#delIndentNo").text(data.header.indent);
            $("#delCustomer").text(data.header.company_Name);
            $("#delExecutive").text(data.header.excutive_Name);

            $("#delAddress").text(
                data.header.shipping_Address + ", " +
                data.header.shipping_City + ", " +
                data.header.shipping_Pincode
            );

            $("#delContact").text(data.header.contact_Person);
            $("#delPhone").text(data.header.contact_Person_No);

            $("#delDCNo").text(data.header.dC_No + " / " + data.header.headerDatestr);
            $("#delInvNo").text(data.header.inv_No + " / " + data.header.headerDatestr);

            $("#tblDeleteInvoiceParts tbody").empty();

            var total = 0;

            if (data.details.length == 0) {

                $("#invoiceDeleteSection").hide();

            }
            else {

                $("#invoiceDeleteSection").show();

                var total = 0;

                for (var i = 0; i < data.details.length; i++) {

                    total += data.details[i].invoiceValue;

                    $("#tblDeleteInvoiceParts tbody").append(

                        AppUtil.ProcessTemplateData(
                            "deleteInvoiceRow",
                            data.details[i]
                        )

                    );
                }

                $("#lblInvoiceTotal").text(total);

                $("#chkConfirmDelete").prop("checked", false);

                $("#btnDeleteInvoice").prop("disabled", true);
            }

            $("#btnDeleteInvoice").data("headerid", headerId);

            $("#deleteDispatchPopup").modal("show");

        });

}
$("#chkConfirmDelete").change(function () {

    $("#btnDeleteInvoice")
        .prop("disabled", !$(this).is(":checked"));

});
$("#btnDeleteInvoice").click(function () {

    var headerId = $(this).data("headerid");

    api.post("/Gro/DeleteDispatchHeader", {
        gro_Disp_HeaderId: headerId
    })
        .then(function () {

            $("#deleteDispatchPopup").modal("hide");

            LoadInvoiceDeleteData();

        });

});
$("#btnStockCorrection").click(function () {

    $("#stockCorrectionModal").modal("show");
    $("#btnConfirmCorrection").hide();

});
$("#ddlSituation").change(function () {

    if ($(this).val() == "") {

        $("#btnScanCorrection").prop("disabled", true);

        return;

    }

    $("#btnScanCorrection").prop("disabled", false);

});
$("#btnScanCorrection").click(function () {

    $("#txtCorrectionQRCode")
        .prop("readonly", false)
        .focus();

});
$("#txtCorrectionQRCode").on("keypress", function (e) {

    if (e.which != 13)
        return;

    e.preventDefault();

    loadCorrectionLabel($(this).val().trim());

    $(this).val("");

});
function loadCorrectionLabel(qrCode) {

    api.post("/Gro/GetCorrectionLabel",
        {
            qrCode: qrCode
        })
        .then(function (data) {

            if (!data.success) {

                toastr.error(data.message);

                return;

            }

            $("#lblCorrPartNo").text(data.partNo);

            $("#lblCorrPartDesc").text(data.description);

            $("#lblCorrSerial").text(data.serialNo);
            $("#hdnCorrectionStockDetId")
                .val(data.stockDetId);

            $("#txtCorrectionQRCode")
                .prop("readonly", true);
            loadCorrectionParts();

            var situation = $("#ddlSituation").val();

            //----------------------------------------------------
            // Wrong Label
            //----------------------------------------------------

            if (situation == "WrongLabel") {

                if (data.statusId != 6) {

                    toastr.warning(
                        "Only Inventory labels can be corrected."
                    );

                    return;
                }
                $("#divCorrectionMessage")
                    .removeClass("d-none alert-info alert-success")
                    .addClass("alert-danger")
                    .text("Discard this incorrect label. Generate a new label from Unit Pack, stick it on the box, add it to inventory, then continue dispatch.");

                
                $("#btnConfirmCorrection")
                    .text("Discard Label")
                    .show();

                return;
            }

            //----------------------------------------------------
            // Empty Box
            //----------------------------------------------------

            if (situation == "EmptyBox") {

                if (data.statusId != 6) {

                    toastr.warning(
                        "Only Inventory labels can be corrected."
                    );

                    return;
                }

                $("#divCorrectionMessage")
                    .removeClass("d-none alert-danger alert-success")
                    .addClass("alert-info")
                    .text("Place the correct part inside this box and press Confirm.");

                $("#btnConfirmCorrection")
                    .text("Confirm Box Filled")
                    .show();

                return;
            }
           
            //loadCorrectionParts(data.partId);

        });

}

function loadCorrectionParts() {

    api.getbulk("/Gro/GetAllCorrectionParts")

        .then(function (list) {

            $("#tblCorrectionPartBody").empty();

            var template = $("#correctionPartRow").html();

            $.each(list, function (i, item) {

                var row = template;

                row = row
                    .replace("{gro_Part_List_ID}", item.partId)
                    .replace("{gro_Part_No}",
                        item.partNo + " - " + item.description)
                    .replace("{qnty_on_Hand}", item.qoh);

                $("#tblCorrectionPartBody").append(row);

            });

        });

}
$("#txtCorrectPartNo").on("keyup", function () {

    var value = $(this).val().toLowerCase();

    $("#tblCorrectionPartBody tr").each(function () {

        $(this).toggle(
            $(this).text().toLowerCase().indexOf(value) > -1
        );

    });

});
$("#stockCorrectionModal").on("hidden.bs.modal", function () {

    // Hidden fields
    $("#hdnCorrectionStockDetId").val("");

    // Situation
    $("#ddlSituation").val("");

    // Scan textbox
    $("#txtCorrectionQRCode")
        .val("")
        .prop("readonly", true);

    // Scanned label details
    $("#lblCorrPartNo").text("");
    $("#lblCorrPartDesc").text("");
    $("#lblCorrSerial").text("");

    // Search textbox
    $("#txtCorrectPartNo").val("");

    // Parts table
    $("#tblCorrectionPartBody").empty();

    // Instruction message
    $("#divCorrectionMessage")
        .addClass("d-none")
        .removeClass("alert-info alert-success alert-danger")
        .text("");

    // Buttons
    $("#btnScanCorrection")
        .prop("disabled", true);

    $("#btnConfirmCorrection")
        .text("Confirm Label")
        .hide();

});
$("#btnConfirmCorrection").click(function () {

    var situation = $("#ddlSituation").val();

    if (situation == "WrongLabel") {

        discardWrongLabel();

        return;
    }

    if (situation == "EmptyBox") {

        confirmEmptyBox();

        return;
    }

});
function confirmEmptyBox() {

    api.post("/Gro/ConfirmEmptyBox", {

        stockDetId:
            $("#hdnCorrectionStockDetId").val()

    })

        .then(function (data) {

            if (!data.success) {

                toastr.error(data.message);

                return;
            }

            toastr.success(data.message);

            $("#stockCorrectionModal").modal("hide");

        });

}
function discardWrongLabel() {

    api.post("/Gro/DiscardInventoryLabel", {

        stockDetId: $("#hdnCorrectionStockDetId").val()

    })

        .then(function (data) {

            if (!data.success) {

                toastr.error(data.message);

                return;
            }

            toastr.success(data.message);

            $("#stockCorrectionModal").modal("hide");

        })

        .catch(function (err) {

            console.log(err);

        });

}