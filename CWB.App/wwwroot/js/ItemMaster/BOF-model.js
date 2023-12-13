var BOFFormUtil = {

    UpdateFormIDs: (data) => {
        $("#BoughtOutFinishDetailId").val(data.boughtOutFinishDetailId);
        $("#PartId").val(data.partId);
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
        $("#BoughtOutFinishDetailId").val("0");
        $("#PartId").val("0");

        
    },
    ConfirmDialog: (prevId,id, message) => {
        //////debugger;
        var result = confirm(message);
        if (result) {
            $("#BoughtOutFinishMadeType").val(id);
            if (Id === "1") {
                //$("#RadioStandard").prop("checked", true);
            }
            else if (Id === "2") {
                //$("#RadioCatalog").prop("checked", true);
            }
            else {
                //$("#RadioMadeToPrint").prop("checked", true);
            }
        }
        else {
            $("#BoughtOutFinishMadeType").val(prevId);
        //    alert(prevId)
            if (prevId === "1") {
                $("#RadioStandard").prop("checked", true);
            }
            else if (prevId === "2") {
                $("#RadioCatalog").prop("checked", true);
            }
            else {
                $("#RadioMadeToPrint").prop("checked", true);
            }
        }
    },
    ValidateMainTabForTabChange: () => {
        if ($("#PartNo").val().length == 0) {
            return false;
        }
        if ($("#PartDescription").val().length == 0) {
            return false;
        }
        if ($("#SupplierPartNo").val().length == 0) {
            return false;
        }
        if ($("#BoughtOutFinishDetailId").val() == "0") {
            return false;
        }
        return true;
    },
    ValidateBOFDetails: () => {
        var Message = "";
        var ManufacturedPartDetail = true;
        ////debugger;
       /* if ($("#BoughtOutFinishMadeType").val().length == "") {
            RawMaterialDetail = false;
            Message += "Bought Out FinishMade Type\n"
        }*/
        
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
    ////debugger;
    // Document is ready

    CURRENT_TAB = "BOFMain";
    $('input[type=radio][name=BoughtOutFinishMade]').change(function () {
        var ShowMessage = "Please Save The Detail(s) or All The Data Will Erase";
        //var preVal = $("#BoughtOutFinishMadeType").val();
        //var val = $('input[name=BoughtOutFinishMade]:checked').val();
        //alert(preVal + " v:" + val);
        var inputs = document.getElementsByName('BoughtOutFinishMade');
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                if (confirm(ShowMessage)) {
                    inputs[i].checked = true;
                } 
                break;
            }
        }
        //BOFFormUtil.ConfirmDialog(preVal, val, ShowMessage)
    });

    $("#TabPartInfo").click(function (event) {
        if ($('.nav-tabs .active').text().trim() == "Part Info") {
            event.preventDefault();
            return;
        }
        else {
            $('.nav-pills a[href="#tab-1"]').tab('show');
            CURRENT_TAB = "BOFMain";
            document.getElementById("BOFform").reset();
            $("#BoughtOutFinishDetailId").val("0");
            $("#PartId").val("0");

        }
        //BOFform
    });

    $("#TabPurchaseDetails").click(function (event) {
        if ($('.nav-tabs .active').text().trim() == "Purchase Details") {
            event.preventDefault();
            return;
        }
        else {
            if (!BOFFormUtil.ValidateMainTabForTabChange()) {
                alert("Field(s) cannot be left blank.");
                event.preventDefault();
            }
            else {
                document.getElementById("FormPurchaseDetails").reset();
                let spanPartNo = document.getElementById("Span_PartNo");
                let spanPartDesc = document.getElementById("Span_PartDescription");
                spanPartNo.innerText = $("#PartNo").val();
                spanPartDesc.innerText = $("#PartDescription").val();
                masterPartType = "2";//BOF
              


                $('.nav-pills a[href="#tab-2"]').tab('show');
                CURRENT_TAB = "TabPurchaseDetails";
                var tablebody = $("#TablePurchaseDetails tbody");
                tablebody.html("");
            }
        }
    });
    
   

    $('input[type=radio][name=PartisMadefrom]').change(function () {
        $("#PartMadeFrom").val(this.value);
    });

    $("#btnBOFDetailSubmit").click(function (event) {
        if (!modelObj.Edit) {
            if ($("#BoughtOutFinishDetailId").val() != "0") {
                alert("Älready saved... BOF");
                event.preventDefault();
                return;
            }
        }
        if (BOFFormUtil.ValidateBOFDetails()) {
            if ($("#BOFform").valid()) {
                var formData = AppUtil.GetFormData("BOFform");
                api.post("/masters/boughtoutfinishdetail", formData).then((data) => {
                    ////debugger;
                    BOFFormUtil.UpdateFormIDs(data);
                   // document.getElementById("BOFform").reset();

                }).catch((error) => {
                    AppUtil.HandleError("BOFform", error);
                });
            }
        }
        event.preventDefault();
    });

});