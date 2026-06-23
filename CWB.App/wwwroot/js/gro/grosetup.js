$('#groPartListModal').on('shown.bs.modal', function () {
    $("#preloaderblurred").show();
    loadGroPartList();
    $("#preloaderblurred").hide();
});
function loadGroPartList() {

    api.getbulk("/Gro/GetGroPartList")
        .then((data) => {

            bindGroPartList(data);

        })
        .catch((error) => {

            console.log(error);

           // AppUtil.MessageBox("Unable to load Gro Part List.", 2);

        });

}
function bindGroPartList(data) {

    var tableBody = $("#GroPartListGrid tbody");

    tableBody.html("");

    if (data.length === 0) {

        tableBody.append(`
            <tr>
                <td colspan="6" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < data.length; i++) {
        data[i].hsnCode = data[i].hsnCode ?? "";
        data[i].statusText = data[i].part_Status == 1
            ? "Active"
            : "Obsolete";
        console.log(data[i]);
        tableBody.append(
            AppUtil.ProcessTemplateData("groPartListRow", data[i])
        );
    }

}
function editGroPart(obj) {

    $("#editGroPartPopupLabel").text("Edit Gro Part");
   // $("#btnSaveGroPart").text("Update");

    $("#hdnGroPartId").val($(obj).data("id"));

    $("#txtGroPartNo").val($(obj).data("gropart"));
    $("#txtMrp").val($(obj).data("mrp"));
    $("#txtOurPrice").val($(obj).data("ourprice"));
    $("#txtOurPartNo").val($(obj).data("ourpart"));
    $("#txtHSNCode").val($(obj).data("hsn"));
    $("#txtGSTRate").val($(obj).data("gstrate"));

    $("#ddlGroPartStatus").val($(obj).data("status").toString());
}
function newGroPart() {

    $("#editGroPartPopupLabel").text("New Gro Part");
   // $("#btnSaveGroPart").text("Save");

    $("#hdnGroPartId").val(0);

    $("#txtGroPartNo").val("");
    $("#txtMrp").val("");
    $("#txtOurPrice").val("");
    $("#txtOurPartNo").val("");
    $("#txtHSNCode").val("");
    $("#txtGSTRate").val("");
    $("#ddlGroPartStatus").val("1");
}
function isDuplicateGroPartNo() {

    var partNo = $("#txtGroPartNo").val().trim().toUpperCase();
    var currentId = $("#hdnGroPartId").val();

    var duplicate = false;

    $("#GroPartListGrid tbody tr").each(function () {

        var rowPartNo = $(this).find("td:eq(0)").text().trim().toUpperCase();

        var rowId = $(this).find(".dropdown-item").data("id");

        if (rowPartNo === partNo && rowId != currentId) {
            duplicate = true;
            return false; // break loop
        }
    });

    return duplicate;
}
function saveGroPart() {
    if (isDuplicateGroPartNo()) {

        alert("Gro Part No already exists.");

        $("#txtGroPartNo").focus();

        return;
    }
    var model = {

        gro_Part_ListId: parseInt($("#hdnGroPartId").val()),

        gro_Part_No: $("#txtGroPartNo").val(),
        part_No: 0,
        ourPrice: $("#txtOurPrice").val(),
        Update_By:1,
       // Our_Part_Description: $("#txtDescription").val(),
        mrp: $("#txtMrp").val(),
        gstRate: $("#txtGSTRate").val(),
        hsnCode: $("#txtHSNCode").val(),
        part_Status: $("#ddlGroPartStatus").val()
    };

    $.ajax({
        url: "/Gro/SaveGroPart",
        type: "POST",
        data: model,
        success: function (response) {

            $("#editGroPartPopup").modal("hide");
            loadGroPartList();
        }
    });
}

function loadCourierList() {

    api.getbulk("/Gro/GetCourierList")
        .then((data) => {

            var tableBody = $("#CourierGrid tbody");

            tableBody.html("");

            if (data.length === 0) {

                tableBody.append(`
            <tr>
                <td colspan="6" class="text-center">
                    No Records Found
                </td>
            </tr>`);

                return;
            }

            for (var i = 0; i < data.length; i++) {
                 
                tableBody.append(
                    AppUtil.ProcessTemplateData("courierRow", data[i])
                );
            }

        })
        .catch((error) => {

            console.log(error);

            // AppUtil.MessageBox("Unable to load Gro Part List.", 2);

        });
}
function newCourier() {

    $("#editCourierPopupLabel").text("New Courier");

   // $("#btnSaveCourier").text("Save");

    $("#hdnCourierId").val(0);

    $("#txtCourierName").val("");

    $("#txtCourierContactPerson").val("");

    $("#txtCourierContactNo").val("");

}
function editCourier(obj) {

    $("#editCourierPopupLabel").text("Edit Courier");

   // $("#btnSaveCourier").text("Update");

    $("#hdnCourierId").val($(obj).data("id"));

    $("#txtCourierName").val($(obj).data("name"));

    $("#txtCourierContactPerson").val($(obj).data("contactperson"));

    $("#txtCourierContactNo").val($(obj).data("contactno"));

}
function isDuplicateCourier() {

    var courier = $("#txtCourierName").val().trim().toUpperCase();

    var currentId = $("#hdnCourierId").val();

    var duplicate = false;

    $("#CourierGrid tr").each(function () {

        var rowCourier = $(this).find("td:eq(0)").text().trim().toUpperCase();

        var rowId = $(this).find(".dropdown-item").data("id");

        if (rowCourier == courier && rowId != currentId) {

            duplicate = true;

            return false;

        }

    });

    return duplicate;

}
function saveCourier() {

    if (isDuplicateCourier()) {

        alert("Courier already exists. Please check and re-enter.");

        $("#txtCourierName").focus();

        return;

    }

    var model = {

        courier_List_ID: $("#hdnCourierId").val(),

        courier_Name: $("#txtCourierName").val(),

        contact_Person: $("#txtCourierContactPerson").val(),

        contact_Phone: $("#txtCourierContactNo").val()

    };

    $.ajax({

        url: "/Gro/SaveCourier",

        type: "POST",

        data: model,

        success: function (response) {

            $("#editCourierPopup").modal("hide");

            loadCourierList();

        }

    });

}
function loadInvDcControl() {

    $.ajax({

        url: "/Gro/GetInvDcControl",
        type: "GET",

        success: function (data) {

            clearInvDcControl();

            if (data == null)
                return;

            $("#hdnInvDcControlId").val(data.tK_DC_Inv_ContrlId);

            $("#chkDCEnable").prop("checked", data.dC_Enable == "Y");

            $("#chkInvPrintEnable").prop("checked", data.inv_Print_Enable == "Y");

            $("#chkInvPushEnable").prop("checked", data.inv_Push_Enable == "Y");

            $("#txtCurrentDCNo").val(data.tK_DC_Last_No);
            $("#txtNewDCNo").val(data.tK_DC_Last_No);

            $("#txtCurrentInvNo").val(data.tK_Inv_Last_No);
            $("#txtNewInvNo").val(data.tK_Inv_Last_No);

        }

    });

}
function clearInvDcControl() {

    $("#hdnInvDcControlId").val(0);

    $("#chkDCEnable").prop("checked", false);
    $("#chkInvPrintEnable").prop("checked", false);
    $("#chkInvPushEnable").prop("checked", false);

    $("#txtCurrentDCNo").val("");
    $("#txtCurrentInvNo").val("");

    $("#txtNewDCNo").val("");
    $("#txtNewInvNo").val("");

}
function saveInvDcControl() {

    if ($("#txtNewDCNo").val() != "" &&
        parseInt($("#txtNewDCNo").val()) < parseInt($("#txtCurrentDCNo").val() || 0)) {

        alert("DC Serial No should be greater than or equal to Current Last DC Serial No.");

        $("#txtNewDCNo").focus();

        return;
    }

    if ($("#txtNewInvNo").val() != "" &&
        parseInt($("#txtNewInvNo").val()) < parseInt($("#txtCurrentInvNo").val() || 0)) {

        alert("Invoice Serial No should be greater than or equal to Current Last Invoice Serial No.");

        $("#txtNewInvNo").focus();

        return;
    }

    var model = {

        tK_Dc_Inv_ContrlId: $("#hdnInvDcControlId").val(),

        tK_Dc_Last_No: $("#txtNewDCNo").val(),

        tK_Inv_Last_No: $("#txtNewInvNo").val(),

        dC_Enable: $("#chkDCEnable").is(":checked") ? "Y" : "N",

        inv_Print_Enable: $("#chkInvPrintEnable").is(":checked") ? "Y" : "N",

        inv_Push_Enable: $("#chkInvPushEnable").is(":checked") ? "Y" : "N"

    };

    $.ajax({

        url: "/Gro/SaveInvDcControl",

        type: "POST",

        data: model,

        success: function () {

            $("#groInvDcControlModal").modal("hide");

        }

    });

}