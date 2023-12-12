let existingpartdata = [] ;
let custRM = [];
let ownRM = [];
let ownRMSelected = false;
let tableHasOwnRMParts = false;
let dataWritten = false;

let makeFroms = new Array();
let boms = new Array();

var ContactsConstants = {
    MPMakeFromId: 0,
    DivisionId: 0
    
};

var MPRawMaterialUtil = {
    UpdateFormIDs: (data) => {
        $("#MPMakeFromId").val(data.mPMakeFromId);
    },
    UpdateMakeFromTableNew: (keyVals) => {
        //////debugger;
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
    ClearMakefromTab: () => {
        $("#InputWeight").val("");
        $("#ScrapGenerated").val("");
        $("#QuantityPerInput").val("");
        $("#YieldNotes").val("");
        $("#MPMakeFromId").val("0");
        $('#PreferedRawMaterial').prop('checked', false);
    }
};
var MPBOMUtil = {
    UpdateFormIDs: (data) => {
        $("#MPBOMId").val(data.mPBOMId);
    },
    UpdateBOMTableNew: (keyVals) => {
        //////debugger;
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
    ClearBOMTab: () => {
        $("#Quantity").val("");
        $("#MPBOMId").val("0");
    }
};

var ManufPartFormUtil = {

    UpdateFormIDs: (data) => {
        $("#ManufacturedPartNoDetailId").val(data.manufacturedPartNoDetailId);
        $("#PartId").val(data.partId);
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
    ClearConstants: () => {
        ContactsConstants.MPRawMeterialId = 0;
    },
    
    ClearMainTab: () => {
        $("#CompanyId").val($("#CompanyId option:first").val()).trigger('change');
        $("#CompanyName").val("");
        $("#PartNo").val("");
        $("#RevNo").val("")
        $("#RevDate").val(null);
        $("#PartDescription").val("");
        $("#UOMId").val("1");
        $("#FinishedWeight").val("");
        $("#StatusChangeReason").val("");
        $("ManufacturedPartNoDetailId").val("0");
        $("PartId").val("0");
    },
    ConfirmDialog: (id, message) => {
        var result = confirm(message);
        if (result) {
            ManufPartFormUtil.ClearMainTab();
            if (id == 1) {
                MPBOMUtil.ClearBOMTab();
                $("#TabHeadBOM").hide();
                $("#TabHeadMakefrom").show();
                //$('.nav-pills a[href="#tab-2"]').tab('show')
              //  $("#InputPartNo").val($("#PartNumber").val);
             //   $("#MFDescription").val($("#PartDescription").val);
            }
            else if (id == 2) {
                MPRawMaterialUtil.ClearMakefromTab();
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
        if ($("#PartNo").val().length != "") {
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
        if ($("#PartNo").val().length == "") {
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
        if ($("#Quantity").val().length == "") {
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

$(document).ready(function () {

    if (!document.body) {
        return setTimeout(jquery.ready,1);
    };
    // Initialize select2
    $("#CompanyId").select2();
    // Read selected option
  //  loadCustomers("CompanyId");
//    $("#CompanyId").trigger("change");

    $("#TabMPMain").click(function (event) {
        if ($('.nav-tabs .active').text().trim().indexOf("Basic") >= 0) {
            event.preventDefault();
            return;
        }
        else {
            $('.nav-pills a[href="#tab-1"]').tab('show');
            CURRENT_TAB = "TabMPMain";
            $("#TabHeadBOM").hide();
            $("#TabHeadMakefrom").show();
            document.getElementById("ManufPartForm").reset();
            //Reset hidden fields
            $("#CompanyName").val("");
            $("#PartId").val("0");
            $("#ManufacturedPartNoDetailId").val("0");
            ManufPartFormUtil.ClearMainTab();
            $("#CompanyId").val("").trigger('change');
        }

    });
    $("#TabHeadMakefrom").click(function (event) {

        //  //////debugger;
        if (!ManufPartFormUtil.ValidateManufPartDetails(1)) {
        }
        else {
            if ($("#ManufacturedPartNoDetailId").val() == "0") {
                alert("Save info to proceed further.");
                event.preventDefault();
                return;
            }
            CURRENT_TAB = "TabHeadMakefrom";
            $('.nav-pills a[href="#tab-2"]').tab('show');
            document.getElementById("MPRawMaterial").reset();
            var coName = $("#CompanyName").val();
            var partDesc = $("#PartDescription").val();
            var partNo = $("#PartNo").val();
            var partId = $("#PartId").val();
            $('#lblCompanyName').text(coName);
          //  $("#InputPartNo").val(partNo);
            $("#DelManufPartId").val($("#ManufacturedPartNoDetailId").val());
            $("#EditManufPartId").val($("#ManufacturedPartNoDetailId").val());
            $("#ManufPartId").val($("#ManufacturedPartNoDetailId").val());
            $("#lblPartNumber").text(partNo);
        //    $("#MFDescription").val(partDesc);
            $("#lblPartDescription").text(partDesc);
            var tablebody = $("#tbl-MakeFromRM tbody");
            tablebody.html("");
            makeFroms = new Array();
        }
    });

    $("#TabHeadBOM").click(function (event) {

        if (!ManufPartFormUtil.ValidateManufPartDetails(1)) {
        }
        else {
            if ($("#ManufacturedPartNoDetailId").val() == "0") {
                alert("Save info to proceed further.");
                event.preventDefault();
                return;
            }
            CURRENT_TAB = "TabHeadBOM";
            $('.nav-pills a[href="#tab-3"]').tab('show');
            document.getElementById("MPBOM").reset();
            var coName = $("#CompanyName").val();
            var partDesc = $("#PartDescription").val();
            var partNo = $("#PartNo").val();
            var partId = $("#PartId").val();
            $('#lblBOMCompanyName').text(coName);
            //$("#BOMPartNo").val(partNo);
            $("#DelBOMManufPartId").val($("#ManufacturedPartNoDetailId").val());
            $("#EditBOMManufPartId").val($("#ManufacturedPartNoDetailId").val());
            $("#BOMManufPartId").val($("#ManufacturedPartNoDetailId").val());
            $("#lblBOMPartNumber").text(partNo);
            //$("#BOMPartDesc").val(partDesc);
            $("#lblBOMPartDescription").text(partDesc);
            var tablebody = $("#tbl-BOM tbody");
            tablebody.html("");
            boms = new Array();
        }
    });

    $('#rm-select').on('show.bs.modal', function (event) {
        //  //debugger;
        ownRM = {};
        loadBaseRMs("OBaseRawMaterialId");
        loadRMTypes("ORawMaterialTypeId");
        loadSuppliers("OSupplierId");
        ownRM = "";
        //   $("OPartNo").val($("#InputPartNo").val());
        //  $("OPartDescription").val($("#MFDescription").val());
        //OPartNo
        //OPartDescription
        api.get("/masters/ownrms").then((data) => {
            //console.log(data);
            ownRM = data;
            var tablebody = $("#OwnRMTable tbody");
            $(tablebody).html("");//empty tbody
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("OwnRMSelectionTemplate", data[i],i));
            }

        }).catch((error) => {

        });

    });

    $('#rm-select-cust').on('show.bs.modal', function (event) {
        custRM = {};
        loadBaseRMs("CBaseRawMaterialId");
        loadRMTypes("CRawMaterialTypeId");
        let spansupplier = document.getElementById("RMSupplier");
        spansupplier.innerText = $("#CompanyName").val();
        //   $("CPartNo").val($("#InputPartNo").val());
        //      $("CPartDescription").val($("#MFDescription").val());
        //CPartNo
        //CPartDescription
        var tablebody = $("#CustRMTable tbody");
        var coId = $("#CompanyId option:selected").val();
        $(tablebody).html("");//empty tbody
        api.get("/masters/supplierrms?supplierId=" + coId).then((data) => {
            custRM = data;
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("CustomerSuppliedRMTemplate", data[i],i));
            }

        }).catch((error) => {
        });
    });

    //rm-select
    $("#EditMakeFrom").click(function (event) {
        ////debugger;
        EditMakeFrom(event);
    });

    $("#DelMakeFrom").click(function (event) {
        ////debugger;
        DeleteMakdeFrom(event);
    });

    $("#EditBOM").click(function (event) {
        ////debugger;
        EditBOM(event);
    });

    $("#DelBOM").click(function (event) {
        ////debugger;
        DeleteBOM(event);
       
    });

    $("#CancelDelMakeFrom").click(function (event) {
        ////debugger;
        document.getElementById("btnDelMakeFromClose").click();

    });

    $("#CancelDelBOM").click(function (event) {
        ////debugger;
        document.getElementById("btnDelBOMClose").click();

    });

    $('#dialog-DeleteBOM').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var bomid = relatedTarget.data("bomid");
        api.get("/masters/getbom?Id=" + bomid).then((eData) => {
           /* for (var key in eData) {
                console.log(key);
                console.log(eData[key]);
            }*/
            console.log("++++++++++++++++");
            key = "bomPartNo";
            $("#DelBOMPartNo").val(eData[key]);
            key = "bomPartDesc";
            $("#DelBOMPartDesc").val(eData[key]);
            key = "quantity";
            $("#DelQuantity").val(eData[key]);
            key = "mpbomId";
            $("#DelMPBOMId").val(eData[key]);
        }).catch((error) => {
        });
        //alert(bomid);
    });

    $('#dialog-DeleteMakeFrom').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var makefromid = relatedTarget.data("makefromid");
        api.get("/masters/getmakefrom?Id=" + makefromid).then((eData) => {
         /*   for (var key in eData) {
                console.log(key);
                console.log(eData[key]);
            }
            console.log("++++++++++++++++");*/
            key = "inputWeight";
            $("#DelInputWeight").val(eData[key]);
            key = "mfDescription";
            $("#DelMFDescription").val(eData[key]);
            key = "quantityPerInput";
            $("#DelQuantityPerInput").val(eData[key]);

            key = "yieldNotes";
            $("#DelYieldNotes").val(eData[key]);
            masterPartNo = eData[key];
            key = "preferedRawMaterial";
            $("#DelPreferedRawMaterial").val(eData[key]);
            key = "inputPartNo";
            $("#DelInputPartNo").val(eData[key]);

            key = "partMadeFrom";
            $("#DelPartMadeFrom").val(eData[key]);
            key = "scrapGenerated";
            $("#DelScrapgenerated").val(eData[key]);
            key = "mpMakeFromId";
            $("#DelMPMakeFromId").val(eData[key]);

            key = "mpPartId";
            $("#DelMPPartId").val(eData[key]);
            key = "manufPartId";
            $("#DelManufPartId").val(eData[key]);

        }).catch((error) => {
        });
        //alert(makefromid);
    });

    $('#dialog-EditBOM').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var bomid = relatedTarget.data("bomid");
        //bomPartNo
        //bomPartDesc
        //quantity
        api.get("/masters/getbom?Id=" + bomid).then((eData) => {
           /* for (var key in eData) {
                console.log(key);
                console.log(eData[key]);
            }
            console.log("++++++++++++++++");*/
            key = "bomPartNo";
            $("#EditBOMPartNo").val(eData[key]);
            key = "bomPartDesc";
            $("#EditBOMPartDesc").val(eData[key]);
            key = "quantity";
            $("#EditQuantity").val(eData[key]);
            key = "mpbomId";
            $("#EditMPBOMId").val(eData[key]);
            key = "bomPartId";
            $("#EditMPBOMPartId").val(eData[key]);

        }).catch((error) => {
        });
        //alert(bomid);
    });
    $('#dialog-EditMakeFrom').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var makefromid = relatedTarget.data("makefromid");
        api.get("/masters/getmakefrom?Id=" + makefromid).then((eData) => {
           /* for (var key in eData) {
                console.log(key);
                console.log(eData[key]);
            }
            console.log("++++++++++++++++");*/
            key = "inputPartNo";
            $("#EditInputPartNo").val(eData[key]);

            key = "inputWeight";
            $("#EditInputWeight").val(eData[key]);

            key = "mfDescription";
            $("#EditMFDescription").val(eData[key]);
            key = "quantityPerInput";
            $("#EditQuantityPerInput").val(eData[key]);

            key = "yieldNotes";
            $("#EditYieldNotes").val(eData[key]);
            masterPartNo = eData[key];
            key = "preferedRawMaterial";
            $("#EditPreferedRawMaterial").val(eData[key]);
            
            key = "mpPartMadeFrom";
            $("#EditPartMadeFrom").val(eData[key]);
            key = "scrapGenerated";
            $("#EditScrapgenerated").val(eData[key]);
            key = "mpMakeFromId";
            $("#EditMPMakeFromId").val(eData[key]);

            key = "mpPartId";
            $("#EditMPPartId").val(eData[key]);
            key = "manufPartId";
            $("#EditManufPartId").val(eData[key]);
            
        }).catch((error) => {
        });
     //   alert(makefromid);
    });


    const radioButtons = document.querySelectorAll('input[name="MPPartMadeFrom"]');
    radioButtons.forEach(radio => {
        radio.addEventListener('click', handleRadioClick);
    });
    //debugger;
    var manufacturedPartType = modelObj.manufacturedPartType;
    $("#TabHeadBalloon").hide();
    console.log(manufacturedPartType);
    if (manufacturedPartType == 1) {
        $("#TabHeadMakefrom").show();
        $("#TabHeadBOM").hide();
    }
    else {
        $("#TabHeadMakefrom").hide();
        $("#TabHeadBOM").show();
    }


    $("#CompanyId").change(function () {
        setCo();
    });

    
    //$("#btnAddMPMakeFrom").hide();
    // Document is ready
    $('input[type=radio][name=ManufacturedPartType]').change(function () {
        var ShowMessage = "Please Save The Detail(s) or All The Data Will Erase";
       // $("#ManufacturedPartType").val(this.value);

        if (ManufPartFormUtil.ValidateMainTabForRadio()) {
           // alert("calling clearMainTab...")
            ManufPartFormUtil.ConfirmDialog(this.value, ShowMessage)
        }
        else {
            if (this.value == 1) {
                $("#MasterPartType").val("0");
                MPBOMUtil.ClearBOMTab();
                    $("#TabHeadBOM").hide();
                    $("#TabHeadMakefrom").show();
                 //   $('.nav-pills a[href="#tab-2"]').tab('show');
                }
            else if (this.value == 2) {
                $("#MasterPartType").val("1");
                MPRawMaterialUtil.ClearMakefromTab();
                    $("#TabHeadMakefrom").hide();
                    $("#TabHeadBOM").show();
                   // $('.nav-pills a[href="#tab-3"]').tab('show');
                }
        }
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

    /*if ($("#ManufacturedPartNoDetailId").val() != "0") {
        return true;
    }*/
   

   

    $("#btnManufPartDetailSubmit").click(function (event) {
        if (!modelObj.Edit) {
            if ($("#ManufacturedPartNoDetailId").val() != "0") {
                event.preventDefault();
                alert("Älready saved...");
                return;
            }
        }
        if (ManufPartFormUtil.ValidateManufPartDetails(1)) {
            if ($("#ManufPartForm").valid()) {
                //////debugger;
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
    return;
});

function copyBOFData() {
    var data = bofParts;
    var radiochkd = $('input[name=radiopartsbof]:checked');
    var selval = radiochkd.val();
    //alert("selVal "+selval);
    let i = 0;
    var fndObj = false;
    for (i = 0; i < data.length; i++) {
        console.log("************");
        if (i == selval) {
            console.log(i);
            console.log(data[i]);
            $('#BOMPartId').val(data[i]['partId']);
            $('#BOMPartNo').val(data[i]['partNo']);
            $('#BOMPartDesc').val(data[i]['description']);
            fndObj = true;
            break;
        }
    }
    document.getElementById("btn-close-ExistingPartsBOF").click();
}


function AddToBOM() {
    if (ManufPartFormUtil.ValidateBOM()) {
        if ($("#MPBOM").valid()) {
            //////debugger;
            var keyval = AppUtil.GetFormDataNew("MPBOM");
            var formData = AppUtil.GetFormData("MPBOM");
            var tablebody = $("#tbl-BOM tbody");
            var templateElement = $("#BOM-template").html();
            api.post("/masters/mpbom", formData).then((data) => {
                ////debugger;
                data['deleted'] = false;
                boms.push(data);
                MPBOMUtil.UpdateFormIDs(data);
                //MPBOMUtil.UpdateBOMTableNew(keyval);
                $(tablebody).append(AppUtil.ProcessTemplateData("BOM-template", data));
                MPBOMUtil.ClearBOMTab();
                //$("#BOMManufPartId").val($("#ManufacturedPartNoDetailId").val());
                //document.getElementById("MPBOM").reset();
              //  ManufPartFormUtil.ClearBOMTabAfterAddBOM();
                //                ManufPartFormUtil.UpdateMPMakeFromTable(data.inputPartNo, data.mpMakeFromId);
                preventDefault();
            }).catch((error) => {
                AppUtil.HandleError("MPBOM", error);
            });
        }
    }
}

function EditMakeFrom(event) {
    var formData = AppUtil.GetFormData("FormEditMakeFrom");
    api.post("/masters/mpmakefrom", formData).then((data) => {
        //debugger;
        var partNo = data['inputPartNo'];
        //reloadMakeFroms(partNo);
        modifyMakeFromListForEdit(data);
        reloadMakeFroms(partNo);
        event.preventDefault();
        document.getElementById("btnEditMakeFromClose").click();
    }).catch((error) => {
        AppUtil.HandleError("FormEditMakeFrom", error);
    });
}
function DeleteMakdeFrom(event) {
    var formData = AppUtil.GetFormData("FormDelMakeFrom");
    var partNo = $("#DelInputPartNo").val();
    //debugger;
    api.post("/masters/remmakefrom", formData).then((data) => {
        modifyMakeFromListForDel(data);
        reloadMakeFroms(partNo);
        document.getElementById("btnDelMakeFromClose").click();
        if (dataWritten && ownRMSelected) {
            disableOtherCustRadios();
        }
        else {
            enableOtherCustRadios();
        }
        event.preventDefault();
    }).catch((error) => {
        AppUtil.HandleError("FormEditBOM", error);
    });
}

function EditBOM(event){
    var formData = AppUtil.GetFormData("FormEditBOM");
    api.post("/masters/mpbom", formData).then((data) => {
        var partNo = data['bomPartNo'];
        console.log("====1")
        console.log(data);
        console.log("====2")
        modifyBOMListForEdit(data);
        reloadBOMs(partNo);
        event.preventDefault();
        document.getElementById("btnEditBOMClose").click();
    }).catch((error) => {
        AppUtil.HandleError("FormEditBOM", error);
    });
}

function DeleteBOM(event){
    var formData = AppUtil.GetFormData("FormDeleteBOM");
    var partNo = $("#DelBOMPartNo").val();
    api.post("/masters/rembom", formData).then((data) => {
        console.log("====3")
        console.log(data);
        console.log("====4")
        modifyBOMListForDel(data);
        reloadBOMs(partNo);
        event.preventDefault();
        document.getElementById("btnDelBOMClose").click();
    }).catch((error) => {
        AppUtil.HandleError("FormEditBOM", error);
    });
}

function reloadMakeFroms(partNo) {
    var tablebody = $("#tbl-MakeFromRM tbody");
    tablebody.html("");
    let rData = {};
    let i = 0;
    dataWritten = false;
    /*api.get("/masters/mpmakefromlist?partNo=" + partNo).then((rData) => {
        for (i = 0; i < rData.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("MakeFrom-template", rData[i]));
            dataWritten = true;
        }
    }).catch((error) => {
    });*/
    for (i = 0; i < makeFroms.length; i++) {
        if (makeFroms[i]['deleted']) {
            continue;
        }
        console.log(makeFroms[i]);
        dataWritten = true;
        $(tablebody).append(AppUtil.ProcessTemplateData("MakeFrom-template", makeFroms[i]));
    }
}

function reloadBOMs(partNo) {
    var tablebody = $("#tbl-BOM tbody");
    tablebody.html("");
    let rData = {};
    let i = 0;
    /*api.get("/masters/boms?partNo=" + partNo).then((rData) => {
        for (i = 0; i < rData.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("BOM-template", rData[i]));
        }
    }).catch((error) => {
    });*/
    for (i = 0; i < boms.length; i++) {
        if (boms[i]['deleted']) {
            continue;
        }
        console.log(boms[i]);
        $(tablebody).append(AppUtil.ProcessTemplateData("BOM-template", boms[i]));
    }
}
let modOpN = "";
function modifyMakeFromListForEdit(newData) {
    modOpN = "Edit";
    modifyMakeFromList(newData);
}
function modifyMakeFromListForDel(newData) {
    modOpN = "Delete";
    modifyMakeFromList(newData);
}
function modifyMakeFromList(newData) {
    let foundObj = false;
    let i = 0;
    let newObj = {};
    let idxToDel = -1;
    for (i = 0; i < makeFroms.length; i++) {
        if (newData['mpMakeFromId'] == makeFroms[i]['mpMakeFromId']) {
            idxToDel = i;
            foundObj = true;
            break;
        }
    }
    if (foundObj) {
        if (idxToDel > -1) {
            makeFroms.splice(idxToDel, 1);
            console.log("Spliced one element " + idxToDel)
        }
        if (modOpN == "Edit") {
            newObj = JSON.parse(JSON.stringify(newData));
            newObj['deleted'] = false;
            console.log("Adding new Obj makefroms..");
            console.log(newObj);
            makeFroms.push(newObj);
        }
    }
}

function modifyBOMListForEdit(newData) {
    modOpN = "Edit";
    modifyBOMList(newData);
}
function modifyBOMListForDel(newData) {
    modOpN = "Delete";
    modifyBOMList(newData);
}
function modifyBOMList(newData) {
    let foundObj = false;
    let i = 0;
    let newObj = {};
    let idxToDel = -1;
    for (i = 0; i < boms.length; i++) {
        if (newData['mpbomId'] == boms[i]['mpbomId']) {
            idxToDel = i;
            foundObj = true;
            break;
        }
    }
    if (foundObj) {
        if (idxToDel > -1) {
            boms.splice(idxToDel, 1);
            console.log("Spliced one element " + idxToDel)
        }
        if (modOpN == "Edit") {
            newObj = JSON.parse(JSON.stringify(newData));
            newObj['deleted'] = false;
            console.log("Adding new Obj..");
            console.log(newObj);
            boms.push(newObj);
        }
    }
}

function AddAnotherRM() {
    if (ManufPartFormUtil.ValidateMPMakeFrom()) {
        if ($("#MPRawMaterial").valid()) {
            
            var keyval = AppUtil.GetFormDataNew("MPRawMaterial");
            var formData = AppUtil.GetFormData("MPRawMaterial");
            var tablebody = $("#tbl-MakeFromRM tbody");
            enableOtherCustRadios();
       //     debugger;
            api.post("/masters/mpmakefrom", formData).then((data) => {
             //   //debugger;
                data['deleted'] = false;
                makeFroms.push(data);
                MPRawMaterialUtil.UpdateFormIDs(data);
                $(tablebody).append(AppUtil.ProcessTemplateData("MakeFrom-template", data));
                //MPRawMaterialUtil.UpdateMakeFromTableNew(keyval);
                var coName = $("#CompanyName").val();
                var partDesc = $("#MFDescription").val();
                var partNo = $("#InputPartNo").val();
                var manufPartId = $("#ManufPartId").val();
                var mpPartId = $("#MPPartId").val();
                MPRawMaterialUtil.ClearMakefromTab();
                //document.getElementById("MPRawMaterial").reset();
                $("#InputPartNo").val(partNo);
                $("#MFDescription").val(partDesc);
                $("#lblPartDescription").text(partDesc);
                $("#ManufPartId").val(manufPartId);
                $("#MPPartId").val(mpPartId);
                if (ownRMSelected) {
                    tableHasOwnRMParts = true;
                    disableOtherCustRadios();
                }
            }).catch((error) => {
                AppUtil.HandleError("MPRawMaterial", error);
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





function downloadNLoadExistingParts() {
    var ManufPartType = $("input[name='ManufacturedPartType']:checked").val();
    var coName = $('#CompanyName').val();
    var coId = $("#CompanyId option:selected").val();

    $('#epco').val("");
    $('#eppn').val("");
    $('#eppd').val("");

    epco = "";
    eppn = "";
    eppd = "";
    existingpartdata = "";

    api.get("/masters/ManufacturedPartNoDetailList?ManufPartType=" + ManufPartType + "&CompanyName=" + coId).then((data) => {
        existingpartdata = data;
        loadExistingParts(existingpartdata)//,"child");
        //console.log(existingpartdata);
    }).catch((error) => {
        //console.log(error);
    });

}

function loadExistingParts(data) {
    var compSelect = $('#tbl-existingparts tbody');
    if (compSelect) {
        compSelect.empty();
        var strVal = $("#CompanyName").val();
        $('#epco').val(strVal);
    }
    var coId = $("#CompanyId option:selected").val();


    for (i = 0; i < data.length; i++) {
        var chld = "Child";
        if (data[i].manufacturedPartType == "2") //{ 
            chld = "Assembly";
        //    if(soption == "child")
        //      continue;
        //}
        //else
        //{
        //  if (soption == "assembly")
        //    continue;
        //}*
        if (epco.length > 0) {
            if (!(data[i].companyId.startsWith(coId))) {
                continue;
            }
        }
        if (eppn.length > 0) {
            if (!(data[i].partNo.startsWith(eppn))) {
                continue;
            }
        }
        if (eppd.length > 0) {
            if (!(data[i].partDescription.startsWith(eppd))) {
                continue;
            }
        }
        //todo:soption is a search string
        var val = "<tr><td><input type='radio' name='caselect' value='" + i + "'></td>" +
            "<td>" + data[i].partNo + "</td>" +
            "<td>" + data[i].partDescription + "</td>" +
            "<td>" + chld + "</td>" +
            "<td style='visibility:collapse'>" + data[i].manufacturedPartNoDetailId + "</td>";
        compSelect.append(val);
    }

}

function disableOtherCustRadios() {
    document.getElementById("otmp").disabled = true;
    document.getElementById("csrm").disabled = true;
}
function enableOtherCustRadios() {
    document.getElementById("otmp").disabled = false;
    document.getElementById("csrm").disabled = false;
}

function copyCustData() {
    var data = custRM;
    var radiochkd = $('input[name=CSRM]:checked');
    var selval = radiochkd.val();

    $('#InputPartNo').val(data[selval].partNo);
    $('#MFDescription').val(data[selval].partDescription);
    $('#MPPartId').val(data[selval].partId);
    document.getElementById("btn-close-CustRM").click();
    ownRMSelected = false;
}
function copyOwnData() {
    var data = ownRM;
    var radiochkd = $('input[name=OSRM]:checked');
    var selval = radiochkd.val();
    $('#InputPartNo').val(data[selval].partNo);
    $('#MFDescription').val(data[selval].partDescription);
    $('#MPPartId').val(data[selval].partId);
    document.getElementById("btn-close-RMSelect").click();
    ownRMSelected = true;
}


function copyData() {

    if (existingpartdata.length == 0) {
        alert("Please load parts again...");
        return;
    }
    else {
        //alert("calling copyData");
    }
    var data = existingpartdata;
    var radiochkd = $('input[name=caselect]:checked');
    var selval = radiochkd.val();
    if (CURRENT_TAB == "TabMPMain") {
        $('#PartId').val("0");
        $('#PartNo').val("");
        $('#ManufacturedPartNoDetailId').val("0");
        $('#RevNo').val(data[selval].revNo);
        $('#PartDescription').val(data[selval].partDescription);
        $('#StatusChangeReason').val(data[selval].statusChangeReason);
        $('#FinishedWeight').val(data[selval].finishedWeight);
        $('#RevDate').val(data[selval].revDate);
    }
    else if (CURRENT_TAB == "TabHeadMakefrom") {
        $('#InputPartNo').val(data[selval].partNo);
        $('#MPPartId').val(data[selval].partId);
        $('#MFDescription').val(data[selval].partDescription);
    }
    else {
        $('#BOMPartNo').val(data[selval].partNo);
        $('#MPPartId').val(data[selval].partId);
        $('#BOMPartDesc').val(data[selval].partDescription);
    }
    document.getElementById("btn-close-ExistingParts").click();
    ownRMSelected = false;
}

function doChldAssyRadioJob() {
    //var radiochkd = $('input[name=grchldassy]:checked');
    //var selval = radiochkd.val();
    loadExistingParts(existingpartdata)//, selval);
}

$(document).on('click', '#btnAddUOMClose', function () {
    var dlgComp = $("#dialog-AddUOM");
    document.getElementById("frmAddUOM").reset();
    dlgComp.dialog('close');
});

$(document).on('click', '#btnEditUOMClose', function () {
    var dlgComp = $("#dialog-EditUOM");
    document.getElementById("frmEditUOM").reset();
    dlgComp.dialog('close');
});


$(document).on('click', '#btnAddUOMSubmit', function () {
    var valid = $("#frmAddUOM").valid();
    if (valid) {
        alert("Todo: Add UOM...");
    }
});

$(document).on('click', '#btnEditUOMSubmit', function () {
    var valid = $("#frmEditUOM").valid();
    if (valid) {
        alert("Todo: Edit UOM...");
    }
});


function setUOMId() {
    var strVal = $("#UOMs").children(":selected").attr("id");
    if (strVal == null || strVal == "")
        strVal = "1";
    $('#UOMId').val(strVal);
    //alert(strVal + " UOMId");
}

function setCo() {
    //  var strVal = $("#CompanyId").text();
    $("#CompanyName").val($("#CompanyId option:selected").text());
    let strVal = $("#CompanyName").val();
    //     alert("Setting val" + strVal);
    if (strVal != "") {
        $('#createfep').prop('disabled', false);
    }
    else {
        $('#createfep').prop('disabled', true);
    }
    /*if ($("#ManufacturedPartType").val() == 1) {
        $('#lblCompanyName').text($(this).val());
    }
    else {
        $('#lblBOMCompanyName').text($(this).val());
    }*/
}


