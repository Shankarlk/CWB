let dataMPDList = "";
let partType = "ManufacturedPart";

$(function () {

    $("#master_co").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#master_partno").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
    }); 
    $("#master_description").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
    }); 
    
    var RawMaterialMadeType = 0;
    //////debugger;
    // Document is ready
   /** $("#RawMaterialTypeId").select2();
    loadRMTypes("RawMaterialTypeId");

    $("#BaseRawMaterialId").select2();
    loadBaseRMs("BaseRawMaterialId");

    $("#Standard").select2();
    loadRMStandards("Standard");

    $("#MaterialSpecId").select2();
    loadRMSpecs("MaterialSpecId");

    $("#SupplierId").select2();
    loadSuppliers("SupplierId");*/
    loadMPDList();

    $('input[type=radio][name=MasterPart]').change(function () {
        if (this.value == "1") {
           // alert("one clicked");
            partType = "ManufacturedPart";
            loadMPDList();
        } else if (this.value == "2") {
            partType = "Assembly";
            loadMPDList();
        }
        else if (this.value == "3") {
          //  alert("three clicked");
      //      loadBOFs();
            partType = "BOF";
            loadMPDList();
        } else {
          //  alert("four clicked");
       //     loadSupplierRMS();
            partType = "RawMaterial";
            dataMPDList = "";
            loadMPDList();
        }

    });
});

function ProcessTemplateDataNew(templateId, dataObj) {
    //debugger;
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

function loadMPDList() {
    var tablebody = $("#mptable tbody");
    $(tablebody).html("");//empty tbody
    //UpdatePurchaseDetailsTableFromPostData
    let i = 0;
    if (dataMPDList.length > 2) {
        console.log(partType);
        console.log("================");
        let data = dataMPDList;
        for (i = 0; i < data.length; i++) {
            if (!(data[i]['masterPartType'] == partType))
                continue;
            var tBody = ProcessTemplateDataNew("MasterDetaiTemplate", data[i]);
            $(tablebody).append(tBody);
            console.log(tBody);
        }
    }
    else {
        api.getbulk("/masters/masterparts").then((data) => {
            //console.log(data);
            dataMPDList = data;
            for (i = 0; i < data.length; i++) {
                /*for (var key in data[i]) {
                    console.log(key);
                    console.log(data[i][key]);
                    console.log("*****");
                }*/
                console.log(partType);
                console.log("================");
                if (!(data[i]['masterPartType'] == partType))
                    continue;
                var tBody = ProcessTemplateDataNew("MasterDetaiTemplate", data[i]);
                $(tablebody).append(tBody);
                console.log(tBody);
            }
        }).catch((error) => {
        });
    }
}

