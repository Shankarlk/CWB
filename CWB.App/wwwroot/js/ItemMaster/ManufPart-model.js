var ContactsConstants = {
    MPMakeFromId: 0
};
var ManufPartFormUtil = {

    UpdateFormIDs: (data) => {
        $("#ManufacturedPartNoDetailId").val(null);
    },
    PopulateForm: (data) => {
        $("#DivisionId").val(data.divisionId);
        $("#CompanyId").val(data.companyId);
        $("#CompanyType").val(data.companyType);
        $("#CompanyName").val(data.companyName);
        $("#DivisionName").val(data.divisionName);
        $("#Location").val(data.location);
        $("#Notes").val(data.notes);
    },
    UpdateMPMakeFromTable: (partNumber, MPMakeFromId) => {
        debugger;
        ContactsConstants.MPMakeFromId = MPMakeFromId;
        api.get("/masters/mpmakefromlist/" + partNumber).then((data) => {
            debugger;
            var tablebody = $("#tbl-MakeFromRM tbody");
            $(tablebody).html("");//empty tbody
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("MakeFrom-template", data[i]));
                if (ContactsConstants.MPRawMeterialId == data[i].mpMakeFromId) {
                   // ManufPartFormUtil.PopulateForm(data[i]);
                }
            };
        }).catch((error) => {

        });
    },
    UpdateBOMTable: (formData) => {
            debugger;
            var tablebody = $("#tbl-BOM tbody");
            //$(tablebody).html("");//empty tbody
            for (i = 0; i < formData.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("BOM-template", formData[i]));
                if (ContactsConstants.MPRawMeterialId == formData[i].mpMakeFromId) {
                    // ManufPartFormUtil.PopulateForm(data[i]);
                }
            };
    },
    UpdateBOMTableNew: (keyVals) => {
        debugger;
        var tablebody = $("#tbl-BOM tbody");
        var templateElement = $("#BOM-template").html();
        //$(tablebody).html("");//empty tbody
        for (const pair of keyVals) {
            let key = pair[0];
            let val = pair[1];
            templateElement = templateElement.replaceAll("{" + key + "}", val);
            //$(tablebody).append(AppUtil.ProcessTemplateData("BOM-template", pair);
            //if (ContactsConstants.MPRawMeterialId == formData[i].mpMakeFromId) {
                // ManufPartFormUtil.PopulateForm(data[i]);
            //}
        }
        $(tablebody).append(templateElement);
    },
    UpdateMakeFromTableNew: (keyVals) => {
        debugger;
        var tablebody = $("#tbl-MakeFromRM tbody");
        var templateElement = $("#MakeFrom-template").html();
        //$(tablebody).html("");//empty tbody
        for (const pair of keyVals) {
            let key = pair[0];
            let val = pair[1];
            templateElement = templateElement.replaceAll("{" + key + "}", val);
        }
        $(tablebody).append(templateElement);
    },
    ClearConstants: () => {
        ContactsConstants.MPRawMeterialId = 0;
    },
    ClearMakeFromAfterAddRM: () => {
        $("#InputWeight").val("");
        $("#Scrapgenerated").val("");
        $("#QuantityperInput").val("");
        $("#YieldNotes").val("");
        $('#PreferedRawMaterial').prop('checked', false);
    },
    ClearMainTab: () => {
        $("#Companies").val($("#Companies option:first").val()).trigger('change');
        $("#CompanyName").val("");
        $("#PartNumber").val("");
        $("#RevNo").val("")
        $("#RevDate").val(null);
        $("#PartDescription").val("");
        $("#UOMId").val("1");
        $("#FinishedWeight").val("");
    },
    ClearMakefromTab: () => {
       // $('input[type=radio][name=PartisMadefrom]').prop('checked', false);
        ManufPartFormUtil.ClearMakeFromAfterAddRM();        
        $("#InputPartNo").val("");
        $("#ddlStatus").val("0");
        $("#txtStatusChangeReason").val("");
    },
    ClearBOMTabAfterAddBOM: () => {
        $("#quantity").val("");
    },
    ClearBOMTab: () => {
        ManufPartFormUtil.ClearBOMTabAfterAddBOM();
        $("#ddlStatus").val("0");
        $("#txtStatusChangeReason").val("");
        $("#FinishedWeight").val("");
    },
    ConfirmDialog: (id, message) => {
        var result = confirm(message);
        if (result) {
            ManufPartFormUtil.ClearMainTab();
            if (id == 1) {
                ManufPartFormUtil.ClearBOMTab();
                $("#TabHeadBOM").hide();
                $("#TabHeadMakefrom").show();
                //$('.nav-pills a[href="#tab-2"]').tab('show')
              //  $("#InputPartNo").val($("#PartNumber").val);
             //   $("#MFDescription").val($("#PartDescription").val);
            }
            else if (id == 2) {
                ManufPartFormUtil.ClearMakefromTab();
                $("#TabHeadMakefrom").hide();
                $("#TabHeadBOM").show();
                //$('.nav-pills a[href="#tab-3"]').tab('show')
              //  $("#BOMPartNo").val($("#PartNumber").val);
              //  $("#BOMPartDesc").val($("#PartDescription").val);
            }
        }
        else {
            if (id == 1) {
                $("#ManufChildPart").prop("checked", false);
                $("#Assembly").prop("checked", true);
            }
            else //if (id == 2) 
            {
                $("#Assembly").prop("checked", false);
                $("#ManufChildPart").prop("checked", true);
            }
        }
    },
    ValidateMainTabForRadio: () => {
        if ($("#CompanyName").val().length != "") {
            return true;
        }
        if ($("#PartNumber").val().length != "") {
            return true;
        }
        if ($("#PartDescription").val().length != "") {
            return true;
        }
        if ($("#RevNo").val().length != "") {
            return true;
        }
        return false;
    },
    ValidateManufPartDetails: (Mode) => { 
        var Message = "";
        var ManufacturedPartDetail = true;
        if ($("#ManufacturedPartType").val().length == "") {
            ManufacturedPartDetail = false;
            Message += "ManufPartType\n"
        }
        if ($("#CompanyName").val().length == "") {
            ManufacturedPartDetail = false;
            Message += "Company\n"
        }
        if ($("#PartNumber").val().length == "") {
            ManufacturedPartDetail = false;
            Message += "Part No\n"
        }

        if ($("#PartDescription").val().length == "") {
            ManufacturedPartDetail = false;
            Message += "Part Desc\n"
        }
        if ($("#RevNo").val().length == "") {
            ManufacturedPartDetail = false;
            Message += "Rev No\n"
        }
        if (ManufacturedPartDetail == false && Mode == 1) {
            alert("Field(s) cannot be left blank.");
            //alert(Message);
        }
        return ManufacturedPartDetail;
        //return true;
    },
    ValidateMPMakeFrom: () => {
        var Message = "";
         var RawMeterial = true;
         if ($("#InputPartNo").val().length == "") {
             RawMeterial = false;
             Message += "Input PartNo\n"
        }
         if ($("#InputWeight").val().length == "") {
             RawMeterial = false;
             Message += "Input Weight\n"
        }
        if ($("#ScrapGenerated").val().length == "") {
             RawMeterial = false;
             Message += "Scrap Generated\n"
        }
        if ($("#QuantityPerInput").val().length == "") {
             RawMeterial = false;
            Message += "Part Desc\n"
        }
        if ($("#YieldNotes").val().length == "") {
             RawMeterial = false;
             Message += "Yield Notes\n"
         }
         if (RawMeterial == false) {
            alert("Field(s) cannot be left blank.");
        }
        return RawMeterial;
        //return true;
    },
    ValidateBOM: () => {
        var Message = "";
        var Bom = true;
        if ($("#BOMPartNo").val().length == "") {
            Bom = false;
            Message += "Part No\n"
        }
        if ($("#BOMPartDesc").val().length == "") {
            Bom = false;
            Message += "Part Description\n"
        }
        if ($("#quantity").val().length == "") {
            Bom = false;
            Message += "BOMQty\n"
        }
        
        if (Bom == false) {
            alert("Field(s) cannot be left blank.");
        }
        return Bom;
        //return true;
    }
};
$(function () {

    const radioButtons = document.querySelectorAll('input[name="PartisMadefrom"]');
    radioButtons.forEach(radio => {
        radio.addEventListener('click', handleRadioClick);
    });
    var manufacturedPartType = 0;
    $("#TabHeadMakefrom").show();
    $("#TabHeadBOM").hide();
    $("#TabHeadBalloon").hide();
    //$("#btnAddMPMakeFrom").hide();
    // Document is ready
    $('input[type=radio][name=ManufPartType]').change(function () {
        var ShowMessage = "Please Save The Detail(s) or All The Data Will Erase";
        $("#ManufacturedPartType").val(this.value);

        if (ManufPartFormUtil.ValidateMainTabForRadio()) {
           // alert("calling clearMainTab...")
            ManufPartFormUtil.ConfirmDialog(this.value, ShowMessage)
        }
        else {
                if (this.value == 1) {
                    ManufPartFormUtil.ClearBOMTab();
                    $("#TabHeadBOM").hide();
                    $("#TabHeadMakefrom").show();
                 //   $('.nav-pills a[href="#tab-2"]').tab('show');
                }
                else if (this.value == 2) {
                    ManufPartFormUtil.ClearMakefromTab();
                    $("#TabHeadMakefrom").hide();
                    $("#TabHeadBOM").show();
                   // $('.nav-pills a[href="#tab-3"]').tab('show');
                }
        }
    });

    $('input[type=radio][name=PartisMadefrom]').change(function () {
        $("#PartMadeFrom").val(this.value);
    });

    $("#CompanyNamebak").change(function () {
        if ($("#ManufacturedPartType").val() == 1) {
            $('#lblCompanyName').text($(this).val());
        }
        else {
            $('#lblBOMCompanyName').text($(this).val());
        }
    });
    $("#PartNumberbak").change(function () {
        if ($("#ManufacturedPartType").val() == 1) {
            $("#InputPartNo").val($(this).val());
            $("#lblPartNumber").text($(this).val());
        }
        else {
            $("#BOMPartNo").val($(this).val());
            $("#lblBOMPartNumber").text($(this).val());
        }
    });

    $("#PartDescriptionbak").change(function () {
        if ($("#ManufacturedPartType").val() == 1) {
            $("#MFDescription").val($(this).val());
            $("#lblPartDescription").text($(this).val());
        }
        else {
            $("#BOMPartDesc").val($(this).val());
            $("#lblBOMPartDescription").text($(this).val());
        }
    });

    $("#TabHeadMakefrom").click(function () {
        debugger;
        if (!ManufPartFormUtil.ValidateManufPartDetails(1)) {
        }
        else {
            $('.nav-pills a[href="#tab-2"]').tab('show');
            var coName = $("#CompanyName").val();
            var partDesc = $("#PartDescription").val();
            var partNo = $("#PartNumber").val();
            $('#lblCompanyName').text(coName);
            $("#InputPartNo").val(partNo);
            $("#lblPartNumber").text(partNo);
            $("#MFDescription").val(partDesc);
            $("#lblPartDescription").text(partDesc);
        }
    });

    $("#TabHeadBOM").click(function () {
        if (!ManufPartFormUtil.ValidateManufPartDetails(1)) {
        }
        else {
            $('.nav-pills a[href="#tab-3"]').tab('show');
            var coName = $("#CompanyName").val();
            var partDesc = $("#PartDescription").val();
            var partNo = $("#PartNumber").val();
            $('#lblBOMCompanyName').text(coName);
            $("#BOMPartNo").val(partNo);
            $("#lblBOMPartNumber").text(partNo);
            $("#BOMPartDesc").val(partDesc);
            $("#lblBOMPartDescription").text(partDesc);
        }
    });

    $("#TabMPMain").click(function () {
          $('.nav-pills a[href="#tab-1"]').tab('show');
    });

    $("#btnManufPartDetailSubmit").click(function (event) {
        debugger;
        if (ManufPartFormUtil.ValidateManufPartDetails(1)) {
            if ($("#ManufPartForm").valid()) {
                debugger;
                var formData = AppUtil.GetFormData("ManufPartForm");
                api.post("/Masters/ManufacturedPartNoDetail", formData).then((data) => {
                    ManufPartFormUtil.UpdateFormIDs(data);
                }).catch((error) => {
                    AppUtil.HandleError("ManufPartForm", error);
                });
            }
        }
        event.preventDefault();
       
    });

    $("#btnAddRawMeterial").click(function (event) {
            if ($("#PartNumber").val() != "") {
                $("#InputWeight").val(null);
                $("#Scrapgenerated").val("");
                $("#QuantityperInput").val("");
                $("#YieldNotes").val("");
                $('#PreferedRawMaterial').prop('checked', false);
            }    
    });

    $("#AddBOMBACK").click(function (event) {
        if (ManufPartFormUtil.ValidateBOM()) {
            if ($("#ManufPartForm").valid()) {
                var formData = AppUtil.GetFormData("ManufPartForm");
                ManufPartFormUtil.UpdateMPMakeFromTable(formData);
            }
            event.preventDefault();
        }
    });

    $("#btnSaveMPMakeFromBack").click(function (event) {
        debugger;
        if (ManufPartFormUtil.ValidateMPMakeFrom()) {
            if ($("#ManufPartForm").valid()) {
                debugger;
                var formData = AppUtil.GetFormData("ManufPartForm");
                api.post("/masters/mpmakefrom", formData).then((data) => {
                    debugger;
                    ManufPartFormUtil.UpdateFormIDs(data);
                    ManufPartFormUtil.UpdateMPMakeFromTable(data.inputPartNo, data.mpMakeFromId);
                    preventDefault();
                }).catch((error) => {
                    AppUtil.HandleError("ManufPartForm", error);
                });
            }
            event.preventDefault();
        }

    });

    $("#btnSaveBOM").click(function (event) {
        debugger;
        if (ManufPartFormUtil.ValidateMPMakeFrom()) {
            if ($("#ManufPartForm").valid()) {
                debugger;
                var formData = AppUtil.GetFormData("ManufPartForm");
                api.post("/masters/mpmakefrom", formData).then((data) => {
                    debugger;
                    ManufPartFormUtil.UpdateFormIDs(data);
                    ManufPartFormUtil.UpdateMPMakeFromTable(data.inputPartNo, data.mpMakeFromId);
                    preventDefault();
                }).catch((error) => {
                    AppUtil.HandleError("ManufPartForm", error);
                });
            }
            event.preventDefault();
        }

    });

    return;

});



function AddToBOM() {
    if (ManufPartFormUtil.ValidateBOM()) {
        if ($("#ManufPartForm").valid()) {
            debugger;
            var keyval = AppUtil.GetFormDataNew("ManufPartForm");
            var formData = AppUtil.GetFormData("ManufPartForm");
            api.post("/masters/mpbom", formData).then((data) => {
                debugger;
                ManufPartFormUtil.UpdateFormIDs(data);
                ManufPartFormUtil.UpdateBOMTableNew(keyval);
                ManufPartFormUtil.ClearBOMTabAfterAddBOM();
                //                ManufPartFormUtil.UpdateMPMakeFromTable(data.inputPartNo, data.mpMakeFromId);
                preventDefault();
            }).catch((error) => {
                AppUtil.HandleError("ManufPartForm", error);
            });
        }
    }
}

function AddAnotherRM() {
    if (ManufPartFormUtil.ValidateMPMakeFrom()) {
        if ($("#ManufPartForm").valid()) {
            debugger;
            var keyval = AppUtil.GetFormDataNew("ManufPartForm");
            var formData = AppUtil.GetFormData("ManufPartForm");
            api.post("/masters/mpmakefrom", formData).then((data) => {
                debugger;
                ManufPartFormUtil.UpdateFormIDs(data);
                ManufPartFormUtil.UpdateMakeFromTableNew(keyval);
                ManufPartFormUtil.ClearMakeFromAfterAddRM();
//                ManufPartFormUtil.UpdateMPMakeFromTable(data.inputPartNo, data.mpMakeFromId);
                preventDefault();
            }).catch((error) => {
                AppUtil.HandleError("ManufPartForm", error);
            });
        }
    }
}

//const box = document.getElementById('box');

function handleRadioClick() {
    const searchbtn = $("#searchbtn");
    searchbtn.empty();

    if (document.getElementById('csrm').checked) {
        var templateElement = $("#csrmsearch").html();
        searchbtn.append(templateElement);
    }
    if (document.getElementById('owrm').checked) { 
        var templateElement = $("#owrmsearch").html();
        searchbtn.append(templateElement);
    }
    if (document.getElementById('otmp').checked) { 
        var templateElement = $("#otmpsearch").html();
        searchbtn.append(templateElement);
    }
}

