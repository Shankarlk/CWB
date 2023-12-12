var RawMaterialDetailFormUtil = {

    UpdateFormIDs: (data) => {
        $("#RawMaterialDetailId").val(null);
    },
    ClearMakefromTab: () => {
        $("#InputPartNo").val("");
        $("#MFDescription").val("");
        $("#InputWeight").val(null);
        $("#Scrapgenerated").val("");
        $("#QuantityperInput").val("");
        $("#YieldNotes").val("");
        $('#PreferedRawMaterial').prop('checked', false);
        $("#ddlStatus").val(0);
        $("#txtStatusChangeReason").val("");
        $("#txtCustomer").val("");
        $("#txtPartNo").val("");
        $("#txtRevNo").val("");
        $("#txtRevDate").val(null);
        $("#PartDescription").val("");
        $("#ddlUOM").val(0);
        $("#txtFinishedWeight").val("");
        
    },
    ClearBOMTab: () => {
        $("#BOMPartNo").val("");
        $("#BOMPartDesc").val("");
        $("#BOMQuantity").val(null);
        $("#ddlStatus").val(0);
        $("#txtStatusChangeReason").val("");
        $("#txtCustomer").val("");
        $("#txtPartNo").val("");
        $("#txtRevNo").val("");
        $("#txtRevDate").val(null);
        $("#PartDescription").val("");
        $("#ddlUOM").val(0);
        $("#txtFinishedWeight").val(null);
    },
    ConfirmDialog: (id, message) => {
        debugger;

        var result = confirm(message);
        if (result) {
            if (id == 1) {
                RawMaterialDetailFormUtil.ClearBOMTab();
                $("#TabPurchaseDetailHeader").hide();
                $("#RawMaterialMadeType").val(id);
   
            }
            else if (id == 2) {
                RawMaterialDetailFormUtil.ClearMakefromTab();
                $("#TabPurchaseDetailHeader").show();
                $("#RawMaterialMadeType").val(id);

            }
        }
        else {
            if (id == 1) {
                $("#MadetoPrint").prop("checked", false);
                $("#StandardPurchasedRM").prop("checked", true);
            }
            else if (id == 2) {
                $("#MadetoPrint").prop("checked", true);
                $("#StandardPurchasedRM").prop("checked", false);
            }
        }
    },
    ValidateRawMaterialDetails: () => {
        var Message = "";
        var RawMaterialDetail = true;
        debugger;
        if ($("#RawMaterialMadeType").val().length == "") {
            RawMaterialDetail = false;
            Message += "RawMaterial Made Type\n"
        }
        if ($("#RawMaterialTypeId").val() == 0) {
            RawMaterialDetail = false;
            Message += "Raw Material Type\n"
        }
        if ($("#PartDescription").val().length == "") {
            RawMaterialDetail = false;
            Message += "Part Description\n"
        }
        if ($("#BaseRawMaterialId").val() == 0) {
            RawMaterialDetail = false;
            Message += "Base Raw Material\n"
        }
        if (RawMaterialDetail == false) {
            alert("Following field(s) cannot be left blank.\n" + Message);
        }
        return RawMaterialDetail;
    }
};
$(function () {
    var RawMaterialMadeType = 0;
    debugger;
    $("#TabPurchaseDetailHeader").hide();
    $("#TabHeadBalloon").hide();

    // Document is ready


    $('input[type=radio][name=RawMetMadeType]').change(function () {
        var ShowMessage = "Please Save The Detail(s) or All The Data Will Erase";
        RawMaterialDetailFormUtil.ConfirmDialog(this.value, ShowMessage)
    });

    $("#btnRawMaterialDetailSubmit").click(function (event) {
        debugger;
        if (RawMaterialDetailFormUtil.ValidateRawMaterialDetails()) {
            if ($("#RawMetform").valid()) {
                debugger;
                var formData = AppUtil.GetFormData("RawMetform");
                api.post("/masters/rawmaterialdetail", formData).then((data) => {
                    RawMaterialDetailFormUtil.UpdateFormIDs(data);
                }).catch((error) => {
                    AppUtil.HandleError("RawMetform", error);
                });
            }
        }
        else {
            event.preventDefault();
        }
       
    });

});