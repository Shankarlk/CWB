var BOFFormUtil = {

    UpdateFormIDs: (data) => {
        $("#BoughtOutFinishDetailId").val(null);
    },
    ClearPurchaseDetailTab: () => {
        $('input[type=radio][name=PartisMadefrom]').prop('checked', false);
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
    ConfirmDialog: (id, message) => {
        debugger;

        var result = confirm(message);
        if (result) {
            if (id == 1) {
                ManufPartFormUtil.ClearBOMTab();
                $("#TabPurchaseDetailHeader").hide();
                $("#BoughtOutFinishMadeType").val(id);
            }
            else if (id == 2) {
                ManufPartFormUtil.ClearMakefromTab();
                $("#TabPurchaseDetailHeader").hide();
                $("#BoughtOutFinishMadeType").val(id);
            }
            else if (id == 3) {
                ManufPartFormUtil.ClearMakefromTab();
                $("#TabPurchaseDetailHeader").show();
                $("#BoughtOutFinishMadeType").val(id);
            }
        }
        else {
            if (id == 1) {
                $("#ManufChildPart").prop("checked", false);
                $("#Assembly").prop("checked", true);
            }
            else if (id == 2) {
                $("#Assembly").prop("checked", false);
                $("#ManufChildPart").prop("checked", true);
            }
            else if (id == 3) {
                $("#Assembly").prop("checked", false);
                $("#ManufChildPart").prop("checked", true);
            }
        }
    },
    ValidateBOFDetails: () => {
        var Message = "";
        var ManufacturedPartDetail = true;
        debugger;
        if ($("#BoughtOutFinishMadeType").val().length == "") {
            RawMaterialDetail = false;
            Message += "Bought Out FinishMade Type\n"
        }
        if ($("#PartDescription").val().length == "") {
            ManufacturedPartDetail = false;
            Message += "Part Description\n"
        }
        if (ManufacturedPartDetail == false) {
            alert("Following field(s) cannot be left blank.\n" + Message);
        }
        return ManufacturedPartDetail;
    }
};
$(function () {
    var manufacturedPartType = 0;
    debugger;
    $("#TabPurchaseDetailHeader").hide();
    $("#TabHeadBalloon").hide();

    // Document is ready


    $('input[type=radio][name=BOFMadeType]').change(function () {
        var ShowMessage = "Please Save The Detail(s) or All The Data Will Erase";
        BOFFormUtil.ConfirmDialog(this.value, ShowMessage)
    });

    $('input[type=radio][name=PartisMadefrom]').change(function () {
        $("#PartMadeFrom").val(this.value);
    });

    $("#btnBOFDetailSubmit").click(function (event) {
        debugger;
        if (BOFFormUtil.ValidateBOFDetails()) {
            if ($("#BOFform").valid()) {
                debugger;
                var formData = AppUtil.GetFormData("BOFform");
                api.post("/masters/boughtoutfinishdetail", formData).then((data) => {
                    BOFFormUtil.UpdateFormIDs(data);
                }).catch((error) => {
                    AppUtil.HandleError("BOFform", error);
                });
            }
        }
        else {
            event.preventDefault();
        }
       
    });

});