let dataPartsRoutings = {};
let partType = "ManufacturedPart";
let selectedManuPartId = "";
let selectedBomId = "";
let qtyAvailable = 0;
let hasRouting = true;
let stepModel = {};
let routingModel = {};




function LoadOperations() {
    var selElem = $('#StepOperation');//should be a select2 dropdown
    if (!selElem.length)
        return;
    selElem.empty();
    var div_data = "<option value=''></option>";
    selElem.append(div_data);
    
    api.get("/operationlist/operations").then((data) => {
        console.log(data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].operationId + "'>" +
                data[i].operation +
                "</option>";
            selElem.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}


function ProcessTemplateDataNew(templateId, dataObj) {
    var templateElement = $("#" + templateId).html();
    ////console.log(templateId);
    templateElement = templateElement.replaceAll("{partType}", partType)
    for (var key in dataObj) {
        ////console.log(key + " " + dataObj[key]);
        templateElement = templateElement.replaceAll("{" + key + "}", dataObj[key])
    }
    console.log(templateElement);
    return templateElement;
}

// JavaScript source code
function DowlonadPartsRoutings() {
    //RoutingListItems
    var tablebody = $("#PartsRoutingsTable tbody");
    $(tablebody).html("");//empty tbody
    //UpdatePurchaseDetailsTableFromPostData
    let i = 0;
    if (dataPartsRoutings.length > 2) {
        let data = dataPartsRoutings;
        for (i = 0; i < data.length; i++) {
            if (!(data[i]['masterPartType'] == partType))
                continue;
            //if ((data[i]['hasRouting']) != hasRouting)
              //  continue;
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("Parts-Routing-Template", data[i],i));
        }
    }
    else {
        api.getbulk("/routings/routinglistitems").then((data) => {
            console.log(data);
            dataPartsRoutings = data;
            for (i = 0; i < data.length; i++) {
                for (var key in data[i]) {
                    console.log(key);
                    console.log(data[i][key]);
                    console.log("*****");
                }
                console.log(partType);
                console.log("================");
                if (!(data[i]['masterPartType'] == partType))
                    continue;
             //   if ((data[i]['hasRouting']) != hasRouting)
               //     continue;
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("Parts-Routing-Template", data[i], i));
            }
        }).catch((error) => {
        });
    }
}

$(function () {
    console.log("Ready");

    function showElement(elem) {
        elem.style.display = "block";
    };
    function hideElem(elem) {
        elem.style.display = "none";
    };

    DowlonadPartsRoutings();
    //LoadBOMList(16);
    LoadOperations();
 
    $('#WithoutRouting').change(function () {
        if (this.checked) {
            hasRouting = false;
        }
        hasRouting = true;
     //   DowlonadPartsRoutings();
    });

    function hideMachinesSuppliersTable() {
        let machs = document.getElementById("Div_RouteMachines");
        let suplrs = document.getElementById("Div_RouteSuppliers");
        hideElem(machs);
        hideElem(suplrs);
    };

    $('#StepLocation').change(function () {
        //debugger;
        let selVal = $(this).val();
        let machs = document.getElementById("Div_RouteMachines");
        let suplrs = document.getElementById("Div_RouteSuppliers");
        if (selVal == "1")//InHouse
        {
            showElement(machs);
            hideElem(suplrs);
            //Div_RouteMachines show
        }
        else if (selVal == "2")//SubCon
        {
            //Div_RouteSuppliers show
            hideElem(machs);
            showElement(suplrs);
            loadSetSuppliers();
        }
        else {
            hideElem(machs);
            hideElem(suplrs);
        }
    });

    /*const checkbox = document.getElementById('WithoutRouting')

    checkbox.addEventListener('change', (event) => {
        if (event.currentTarget.checked) {
            hasRouting = false;
            DowlonadPartsRoutings();
        } else {
            hasRouting = true;
            DowlonadPartsRoutings();
        }
    });*/

    $('#BtnSelectBOMForRouting').click(function (event) {
        var bomId = $('input[name=BOMChk]:checked').val();
        var chkdelm = $('input[name=BOMChk]:checked');
        var currentrow = chkdelm.closest('tr');
        var partNo = currentrow.find("td:eq(1)").html();
        var partDesc = currentrow.find("td:eq(2)").html();
        var qty = currentrow.find("td:eq(4)").html();
        qtyAvailable = parseInt(qty);
        if (qtyAvailable <= 0) {
            alert("Quantity not available.");
            return;
        }
        $("#BOMId").val(bomId);
        $("#BOMPartNo").val(partNo);
        $("#BOMPartDescription").val(partDesc);
        $("#QuantityAssembly").val(qty);
        SetAssemblyIds();
        console.log("manufacturedPartId / BOMRoutingStepId / RoutingStepPartId: " + modelObj.manufacturedPartId + "/" + $("#BOMRoutingStepId").val() + "/" + $("#RoutingStepPartId").val());
        console.log(bomId + ":" + partNo + ":" + partDesc + ":"+qty);
    });

    function getRoutingInfoFromTable() {
        //stepsupplierselect
        var chkdelm = $('input[name=RoutingChk]:checked');
        var currentrow = chkdelm.closest('tr');
        var routingId = $('input[name=RoutingChk]:checked').val();
        var routingName = currentrow.find("td:eq(1)").html();
        modelObj["routingName"] = routingName;
        modelObj["routingId"] = routingId;
        modelObj["stepNumber"] = "";
    };

    $('input[type=radio][name=RoutingChk]').change(function () {
        getRoutingInfoFromTable();
        $("#DivRoutingName").html("<h5>Routing Name : " + modelObj.routingName + "</h5>");
        $("#DivRoutingName1").html("<h5>Routing Selected : " + modelObj.routingName + "</h5>");
    });
    $("#RoutingAvailable").click(function (event) {

    });

    $("#RoutingDetails").click(function (event) {
        getRoutingInfoFromTable();
        $("#DivRoutingName").html("<h5>Routing Name : " + modelObj.routingName + "</h5>");
        $("#DivRoutingName1").html("<h5>Routing Selected : " + modelObj.routingName + "</h5>");
        $('#StepRoutingId').val(modelObj.routingId);
        LoadRoutingSteps(modelObj.routingId);
    });

    $("#tab-step-info").click(function (event) {
        document.getElementById("FormRoutingStep").reset();
        var routingid = $('input[name=RoutingChk]:checked').val();
        $('#StepRoutingId').val(routingid);
        hideMachinesSuppliersTable();
    });

    $("#tab-step-parts").click(function (event) {
        
        if ($("#StepId").val() == "0") {
            alert("Please save step info to proceed further.");
            event.preventDefault();
            return;
        }
        else {
            LoadBOMList($("#StepId").val());
        }
    });

    function setDialogTitles() {
        //stepModel
        //modelObj
    };
    

    $("#RoutingStepDetails").click(function (event) {
        getRoutingInfoFromTable();
        hideMachinesSuppliersTable();
        $("#BOMManufacturedPartId").val(modelObj.manufacturedPartId);
    });
    
    
    $("#master_partno").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PartsRoutingsTable tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#master_description").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PartsRoutingsTable tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
    }); 
    
    $("#master_co").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PartsRoutingsTable tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    


    $('input[type=radio][name=MasterPartType]').change(function () {
        if (this.value == "1") {
            // alert("one clicked");
            partType = "ManufacturedPart";
            DowlonadPartsRoutings();
        } else if (this.value == "2") {
            partType = "Assembly";
            DowlonadPartsRoutings();
        }
        else if (this.value == "3") {
            //  alert("three clicked");
            //      loadBOFs();
            partType = "Routing";
            DowlonadPartsRoutings();
        } else {
            //  alert("four clicked");
            //     loadSupplierRMS();
            partType = "WithoutRouting";
            dataMPDList = "";
            DowlonadPartsRoutings();
        }

    });

    $('#alt-rout').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var routingid = relatedTarget.data("routingid");
        var manufacturedPartId = relatedTarget.data("manufid");
        selectedManuPartId = manufacturedPartId;
        $("#AltManufacturedPartId").val(manufacturedPartId);
        $("#AltOrigRoutingId").val(routingid);
    });
    $('#alt-rout').on('hide.bs.modal', function (event) {
        window.location.href = "/routings/routingdetails?manufPartId=" + selectedManuPartId;
    });
    
    $('#routing-new').on('hide.bs.modal', function (event) {
        window.location.href = "/routings/routingdetails?manufPartId=" + selectedManuPartId;
    });

    $('#routing-new').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var manufacturedPartId = relatedTarget.data("manufacturedpartid");
        $("#ManufacturedPartId").val(manufacturedPartId);
        selectedManuPartId = manufacturedPartId;
     /* var partNo = relatedTarget.data("partno");
        var coName = relatedTarget.data("companyname");
        var partDesc = var partNo = relatedTarget.data("partdescription");
        $("#NRPartNo").val(partNo);
        $("#NRCompanyName").val(coName);
        $("#NRPartDescription").val(partDesc);*/
        //<a href="javascript:void(0);" data-manufacturedPartId="{manufacturedPartId}" 
        //data - partno="{partNo}" data - companyname="{companyName}" data - partdescription="{partDescription}" data - toggle="modal" 
        //data - target="#routing-new" class="dropdown-item" > Create New Routing</a >
    });

    $("#BtnNewRouting").click(function (event) {
        //FormNewRoutingName
        //FormAltRoutingName
        //FormRoutingStep
        //FormStepPart
        var formData = AppUtil.GetFormData("FormNewRoutingName");
        api.post("/routings/addnewrouting", formData).then((data) => {
            console.log(data);
            document.getElementById("BtnNewRoutingClose").click();
        }).catch((error) => {
            AppUtil.HandleError("FormNewRoutingName", error);
        });
    });

    $("#BtnAltRoutingSave").click(function (event) {
        //routings/addnewrouting
        var formData = AppUtil.GetFormData("FormAltRoutingName");
        api.post("/routings/addnewrouting", formData).then((data) => {
            console.log(data);
            document.getElementById("BtnAltRoutingClose").click();
        }).catch((error) => {
            AppUtil.HandleError("FormAltRoutingName", error);
        });
    });

    $('#add-supplier').on('show.bs.modal', function (event) {
        var tablebody = $("#TitleTableSupplier tbody");
        tablebody.html("");
        $(tablebody).append(AppUtil.ProcessTemplateData("TitleRow", modelObj));
        console.log(modelObj);
        $("#SupplierRoutingStepId").val(modelObj["stepId"]);
        console.log("====2=====");
        console.log(modelObj["stepId"]);
        console.log("====2-End=====");
        //RoutingSelectSupplierTable
        //RoutingSelectSupplierRow
        loadSuppliersToTable("RoutingSelectSupplierTable", "RoutingSelectSupplierRow");
        if ($("#stepsupp_1").length) {
            $("#stepsupp_1").prop('checked',true);
        }
    });
    $('#add-machine').on('show.bs.modal', function (event) {
        var tablebody = $("#TitleTableMachine tbody");
        tablebody.html("");
        $(tablebody).append(AppUtil.ProcessTemplateData("TitleRowMachine", modelObj));
        console.log(modelObj);
    });

    $('input[type=radio][name=stepsupplierselect]').change(function () {
        var chkdelm = $('input[name=stepsupplierselect]:checked');
        var currentrow = chkdelm.closest('tr');
        var supplierId = $('input[name=stepsupplierselect]:checked').val();
        var supplierName = currentrow.find("td:eq(1)").html();
        $("#SupplierId").val(supplierId);
        $("#Supplier").val(supplierName);
    });
    $("#SaveRouteSupplier").click(function (event) {
        var chkdelm = $('input[name=stepsupplierselect]:checked');
        var currentrow = chkdelm.closest('tr');
        var supplierId = $('input[name=stepsupplierselect]:checked').val();
        var supplierName = currentrow.find("td:eq(1)").html();
        $("#SupplierId").val(supplierId);
        $("#Supplier").val(supplierName);
        //SupplierRoutingStepId
        //Supplier
        //SupplierId
        //stepsupplierselect
        //alert("Save route supplier");
        //routings/addnewrouting
        var formData = AppUtil.GetFormData("FormRoutingSupplier");
        api.post("/routings/savestepsupplier", formData).then((data) => {
            console.log(data);
            loadSetSuppliers();
        }).catch((error) => {
            AppUtil.HandleError("FormRoutingSupplier", error);
        });
        event.preventDefault();
    });

    function loadSetSuppliers() {
        var tablebody = $("#RouteSuppliersTable tbody");
        let stepId = modelObj["stepId"];
        api.get("/routings/stepsuppliers?stepId=" + stepId).then((data) => {
            $(tablebody).html("");//empty tbody
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("RouteSupplierTemplate", data[i]));
            }
            console.log(data);
            console.log(tablebody.html());
            //RouteSuppliersTable
            //RouteSupplierTemplate
        }).catch((error) => {
        });
    };

    $("#SaveRouteMachine").click(function (event) {
        alert("Save route Machine");
        /*var formData = AppUtil.GetFormData("FormRoutingMachine");
        api.post("/routings/savesupplier", formData).then((data) => {
            console.log(data);
        }).catch((error) => {
            AppUtil.HandleError("FormRoutingMachine", error);
        });
        event.preventDefault();*/
    });


    $("#BtnSaveStep").click(function (event) {
        //routings/addnewrouting
        var formData = AppUtil.GetFormData("FormRoutingStep");
        api.post("/routings/savestep", formData).then((data) => {
            stepModel = data;
            modelObj["stepNumber"] = data["stepNumber"];
            modelObj["stepId"] = data["stepId"];
            console.log("====1=====");
            console.log(data);
            console.log("====1-End=====");
            $("#StepId").val(data.stepId);
            $("#BOMRoutingStepId").val(data.stepId);
        }).catch((error) => {
            AppUtil.HandleError("FormRoutingStep", error);
        });
        event.preventDefault();
    });

    $("#BtnAddToStepList").click(function (event) {
        var qtyEntered = parseInt($("#QuantityAssembly").val());
        if (qtyEntered > qtyAvailable) {
            alert("Quantity not available.");
            return;
        }
        if (qtyEntered <= 0) {
            alert("Quantity not available.");
            return;
        }
        var formData = AppUtil.GetFormData("FormStepPart");
        api.post("/routings/addBomtoassembly", formData).then((data) => {
            console.log(data);
            LoadBOMList(data.routingStepId);
            //LoadStepParts(data.manufacturedPartId,data.routingStepId);
            document.getElementById("FormStepPart").reset();
            SetAssemblyIds();
        }).catch((error) => {
            AppUtil.HandleError("FormStepPart", error);
        });
        event.preventDefault();
    });


    $("#BtnAddNextStep").click(function (event) {
        //routings/addnewrouting
        alert("next step... todo...");
    });

});

function SetAssemblyIds() {
    $("#BOMManufacturedPartId").val(modelObj.manufacturedPartId);
    var stepId = $("#StepId").val();
    $("#BOMRoutingStepId").val(stepId);
}
function LoadStepPartsFromData(rData) {
    var tablebody = $("#StepPart-BOMQTY-Table tbody");
    tablebody.html("");
    let i = 0;
    let lastSelectedBOMId = "";
    for (i = 0; i < rData.length; i++) {
        if (rData[i].quantityUsed != "0") {
            $(tablebody).append(AppUtil.ProcessTemplateData("RT_BOMQTY-template", rData[i]));
            if (lastSelectedBOMId == "") {
                lastSelectedBOMId = rData[i]['mpbomId'];
                $("#" + lastSelectedBOMId).attr('checked', true);
            }
        }
    }
    console.log(rData);
}

function LoadStepParts(partId,stepId) {
    var tablebody = $("#StepPart-BOMQTY-Table tbody");
    tablebody.html("");
    let rData = {};
    let i = 0;
    api.get("/routings/boms?partId=" + partId + "&stepId=" + stepId).then((rData) => {
        for (i = 0; i < rData.length; i++) {
            if (rData[i].quantityUsed != "0") {
                $(tablebody).append(AppUtil.ProcessTemplateData("RT_BOMQTY-template", rData[i]));
            }
        }
        console.log(rData);
      //  LoadStepPartsFromData(rData);
    }).catch((error) => {
    });
}

function LoadRoutingSteps(routingId) {
    var tablebody = $("#StepTable tbody");
    tablebody.html("");
    let rData = {};
    let i = 0;
    api.get("/routings/routingsteps?routingId=" + routingId).then((rData) => {
       for (i = 0; i < rData.length; i++) {
           $(tablebody).append(AppUtil.ProcessTemplateData("RoutingStepTemplate", rData[i]));
        }
        console.log(rData);
    }).catch((error) => {
    });
}

function LoadBOMList(stepId) {
    var tablebody = $("#StepPart-BOM-Table tbody");
    tablebody.html("");
    let rData = {};
    let i = 0;
    let manufId = modelObj.manufacturedPartId;
    api.get("/routings/boms?manufId=" + manufId + "&stepId=" + stepId).then((rData) => {
        console.log(rData);
        for (i = 0; i < rData.length; i++) {
            if (rData[i].quantityUsed == "0") {
                $(tablebody).append(AppUtil.ProcessTemplateData("RT_BOM-template", rData[i]));
            }
        }
        LoadStepPartsFromData(rData);
    }).catch((error) => {
    });

}

function onlyNum() {
    let qa = document.getElementById("QuantityAssembly");
    let qaval = qa.value;

    if (qaval != '') {
        if (isNaN(qaval)) {
            // Set input value empty
            qa.value = "";
            return false;
        } else {
            let iQaVal = parseInt(qaval);
            if (iQaVal < 0) {
                qa.value = "";
                return false;
            }

            return true
        }
    }
}

        