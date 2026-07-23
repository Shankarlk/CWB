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

            $("#txtCorrectionQRCode")
                .prop("readonly", true);

            loadCorrectionParts(data.partId);

        });

}
function loadCorrectionParts(partNo) {

    api.getbulk("/Gro/GetCorrectionPart?partNo="
        + encodeURIComponent(partNo))

        .then(function (data) {

            $("#tblCorrectionPartBody").empty();

            if (!data.success)
                return;

            var template =
                $("#correctionPartRow").html();

            template = template
                .replace("{gro_Part_List_ID}", data.partId)
                .replace("{gro_Part_No}",
                    data.partNo + " - " + data.description)
                .replace("{qnty_on_Hand}", data.qoh);

            $("#tblCorrectionPartBody").append(template);

        });

}
$("#stockCorrectionModal").on("hidden.bs.modal", function () {

    $("#ddlSituation").val("");

    $("#txtCorrectionQRCode")
        .val("")
        .prop("readonly", true);

    $("#lblCorrPartNo").text("");

    $("#lblCorrPartDesc").text("");

    $("#lblCorrSerial").text("");

    $("#txtCorrectPart").val("");

    $("#tblCorrectionPartBody").empty();

    $("#btnScanCorrection")
        .prop("disabled", true);

});
$("#btnConfirmCorrection").click(function () {

    // Call your controller here...

    $("#stockCorrectionModal").modal("hide");

});