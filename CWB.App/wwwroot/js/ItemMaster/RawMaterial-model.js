'use strict';
//
class AssociativeArray extends Array {
    constructor() {
        super();
        this.data = {};
    }
    push(key, value) {
        super.push(key);
        this.data[key] = value
    }
    *[Symbol.iterator]() {
        let nextIndex = 0;
        while (nextIndex < this.length) {
            yield this[nextIndex];
            nextIndex++;
        }
    }
}

let addedId = "";
let addedValue = "";


var RawMaterialDetailFormUtil = {

    UpdateFormIDs: (data) => {
        $("#RawMaterialDetailId").val(data.rawMaterialDetailId);
        $("#PartId").val(data.partId);
    },
    ClearMakefromTab: () => {
        $("#Supplier").val("");
        $("#SupplierId").val("");
        $("#SupplierId").trigger('change');
        $("#PartNo").val("");
        $("#PartDescription").val("");
        $("#RawMaterialWeight").val("");
        $("#RawMaterialNotes").val("");
        $("#RevDate").val("");
        $("#RevNo").val("");

        $("#BaseRawMaterialId").val("");
        $("#RawMaterialTypeId").val("");
        $("#BaseRawMaterialId").trigger("change");
        $("#RawMaterialTypeId").trigger("change");


        //$("#OPRM").prop("checked", true).trigger("click");
        $("#MPRM").prop("checked", true).trigger("click");
        $("#Status").val("1");
        $("#Status").trigger("change");
        $("#StatusChangeReason").val("");
        $("#Standard").val("");
        $("#Standard").trigger("change");
        $("#MaterialSpecId").val("");
        $("#MaterialSpecId").trigger("change");
        $("#RawMaterialDetailId").val("0");
        $("#PartId").val("0");
    },
    ConfirmDialog: (id, message) => {
        var result = confirm(message);
        if (result) {
            $("#TabPurchaseDetails").hide();
            if (id == "1") {
                RawMaterialDetailFormUtil.ClearMakefromTab();
                $("#TabPurchaseDetails").show();
            } else {
                RawMaterialDetailFormUtil.ClearMakefromTab();
            }
        }
        else {
            $("#TabPurchaseDetails").hide();
            if (id == "1") {
                $("#OPRM").prop("checked", false);
                $("#CSRM").prop("checked", true);
            } else {
                $("#OPRM").prop("checked", true);
                $("#CSRM").prop("checked", false);
                $("#TabPurchaseDetails").show();
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
        if ($("#RevNo").val().length == 0) {
            return false;
        }
        if ($("#RawMaterialWeight").val().length == 0) {
            return false;
        }
        if ($("#RawMaterialDetailId").val() == "0") {
            return false;
        }
       return true;
    },
    ValidateMainTabForRadio: () => {
        if ($("#PartNo").val().length) {
            return true;
        }
        if ($("#PartDescription").val().length) {
            return true;
        }
        if ($("#RevNo").val().length) {
            return true;
        }
        if ($("#RawMaterialWeight").val().length) {
            return true;
        }
        if ($("#RawMaterialNotes").val().length) {
            return true;
        }
        return false;
    },
    ValidateRawMaterialDetails: () => {
        var Message = "";
        var RawMaterialDetail = true;
        //////debugger;
        /*if ($("#RawMaterialMadeType").val().length == "") {
            RawMaterialDetail = false;
            Message += "RawMaterial Made Type\n"
        }
        if ($("#RawMaterialTypeId").val() == 0) {
            RawMaterialDetail = false;
            Message += "Raw Material Type\n"
        }*/
        
        if ($("#PartDescription").val().length == "") {
            RawMaterialDetail = false;
            Message += "Part Description\n"
        }
        /*if ($("#BaseRawMaterialId").val() == 0) {
            RawMaterialDetail = false;
            Message += "Base Raw Material\n"
        }*/
        if (RawMaterialDetail == false) {
            alert("Following field(s) cannot be left blank.\n" + Message);
        }
        return RawMaterialDetail;
    },
    ClearTable: () => {
        var tablebody = $("#TablePurchaseDetails tbody");
        $(tablebody).empty();
    },
    UpdatePurchaseDetailsTable: (keyVals) => {
        //////debugger;
        var tablebody = $("#TablePurchaseDetails tbody");
        var templateElement = $("#PurchaseDetailTemplate").html();
        for (const pair of keyVals) {
            let key = pair[0];
            let val = pair[1];

            templateElement = templateElement.replaceAll("{" + key + "}", val);
        }
        $(tablebody).append(templateElement);
    },
    UpdatePurchaseDetailsTableFromPostData: (keyVals) => {
        //////debugger;
        var tablebody = $("#TablePurchaseDetails tbody");
        var templateElement = $("#PurchaseDetailTemplate").html();
        for (const key of keyVals) {
            let val = keyVals.data[key];
            templateElement = templateElement.replaceAll("{" + key + "}", val);
        }
        $(tablebody).append(templateElement);
    }
};
$(function () {
    var RawMaterialMadeType = 0;
    //////debugger;
    // Document is ready
    $("#RawMaterialTypeId").select2();
  //  loadRMTypes("RawMaterialTypeId");

    $("#BaseRawMaterialId").select2();
 //   loadBaseRMs("BaseRawMaterialId");

    $("#Standard").select2();
 //   loadRMStandards("Standard");

    $("#MaterialSpecId").select2();
  //  loadRMSpecs("MaterialSpecId");

    $("#SupplierId").select2();
 //   loadSuppliers("SupplierId");

    CURRENT_TAB = "TabRawMaterial";

    $("#SupplierId").change(function () {
        $("#Supplier").val($("#SupplierId option:selected").text());
    });


    $("#TabRawMaterial").click(function (event) {
        if ($('.nav-tabs .active').text().trim() == "Part Info") {
            event.preventDefault();
            return;
        }
        else {
            $('.nav-pills a[href="#tab-1"]').tab('show');
            CURRENT_TAB = "TabRawMaterial";
            document.getElementById("RawMetform").reset();
            $("#Supplier").val("");
            $("#PartId").val("0");
            $("#RawMaterialDetailId").val("0");
            //alert("After Reset..." + $('#PartId').val());
            $("#RawMaterialTypeId").val("").trigger('change');
            $("#SupplierId").val("").trigger('change');
            $("#BaseRawMaterialId").val("").trigger('change');
            $("#Standard").val("").trigger('change');
            $("#MaterialSpecId").val("").trigger('change');
        }
    });

    $("#TabPurchaseDetails").click(function (event) {
        if ($('.nav-tabs .active').text().trim() == "Purchase Details") {
            event.preventDefault();
            return;
        }
        else {
            if (!RawMaterialDetailFormUtil.ValidateMainTabForTabChange()) {
                alert("Field(s) cannot be left blank.");
                event.preventDefault();
            }
            else {
                document.getElementById("FormPurchaseDetails").reset();
                let spanPartNo = document.getElementById("Span_PartNo");
                let spanPartDesc = document.getElementById("Span_PartDescription");
                spanPartNo.innerText = $("#PartNo").val();
                spanPartDesc.innerText = $("#PartDescription").val();
                masterPartType = "3";//Raw Material
                $("#PPartId").val($("#PartId").val());
                $("#PPartNo").val($("#PartNo").val());
            
                
                $('.nav-pills a[href="#tab-2"]').tab('show');
                CURRENT_TAB = "TabPurchaseDetails";
                var tablebody = $("#TablePurchaseDetails tbody");
                tablebody.html("");
            }
        }
        //
        // loadParts($("#PartNo").val());
    });

    $('input[type=radio][name=RawMaterialMadeType]').change(function () {
        var ShowMessage = "Please Save The Detail(s) or All The Data Will Erase";
        var val = this.value;
        if (RawMaterialDetailFormUtil.ValidateMainTabForRadio()) {
            // alert("calling clearMainTab...")
            RawMaterialDetailFormUtil.ConfirmDialog(this.value, ShowMessage);
        }
        else {
            if (val === "1") {
                // document.getElementById("RawMetform").reset();
                $("#TabPurchaseDetails").show();
                RawMaterialDetailFormUtil.ClearMakefromTab();
            }
            else if (val === "2") {
                //document.getElementById("RawMetform").reset();
                $("#TabPurchaseDetails").hide();
                RawMaterialDetailFormUtil.ClearMakefromTab();
            }
            else {

            }
        }

        //  alert(preVal + " " + this.value);

    });

    $("#btnRawMaterialDetailSubmit").click(function (event) {
        ////debugger;
        if (!modelObj.Edit) {
            if ($("#RawMaterialDetailId").val() != "0") {
                alert("Älready saved... RM");
                event.preventDefault();
                return;
            }
        }
        
        if (RawMaterialDetailFormUtil.ValidateRawMaterialDetails()) {
            if ($("#RawMetform").valid()) {
                //       ////debugger;
                var formData = AppUtil.GetFormData("RawMetform");
                api.post("/masters/rawmaterialdetail", formData).then((data) => {
                    RawMaterialDetailFormUtil.UpdateFormIDs(data);
                    //  document.getElementById("RawMetform").reset();
                }).catch((error) => {
                    AppUtil.HandleError("RawMetform", error);
                });
            } else {
                alert("Invalid form...")
            }
        }
        event.preventDefault();
    });

    $('#dialog-AddUOM').on('show.bs.modal', function (e) {
        alert("Show...");
    });

    $('#dialog-AddUOM').on('shown.bs.collapse', function (e) {
        alert("Collapse...")
    });



    $('#dialog-AddRMType').on('show.bs.modal', function (e) {
        CURDLG = $(this);
        let txt = $("#RawMaterialTypeId option:selected").text();
        let id = $("#RawMaterialTypeId option:selected").val();
        $('#TypeName').val(txt);
        $('#DRawMaterialTypeId').val(id);
        //alert("Show...RMType "+id+":"+id);

    });

    $('#dialog-AddRMStandard').on('show.bs.modal', function (e) {
        CURDLG = $(this);
        let txt = $("#Standard option:selected").text();
        let id = $("#Standard option:selected").val();
        $('#StandardName').val(txt);
        $('#DStandard').val(id);
    });

    $('#dialog-AddBaseRM').on('show.bs.modal', function (e) {
        let txt = $("#BaseRawMaterialId option:selected").text();
        let id = $("#BaseRawMaterialId option:selected").val();
        $('#BaseRMName').val(txt);
        $('#DBaseRawMaterialId').val(id);
    });

    $('#dialog-AddRMSpec').on('show.bs.modal', function (e) {
        CURDLG = $(this);
        let txt = $("#MaterialSpecId option:selected").text();
        let id = $("#MaterialSpecId option:selected").val();
        $('#SpecName').val(txt);
        $('#DMaterialSpecId').val(id);
    });

    $('#dialog-AddRMType').on('hide.bs.modal', function (e) {
        //if ((addedId.length) > 0)
        {
            if ((addedValue.length) > 0) {
                var data = {
                    id: addedId,
                    text: addedValue
                };
               var newOption = new Option(data.text, data.id, true, true);
               $('#RawMaterialTypeId').append(newOption).trigger('change');
            }
        }
        addedId = "";
        addedValue = "";
    });
    $('#dialog-AddRMSpec').on('hide.bs.modal', function (e) {
        //if ((addedId.length) > 0)
        {
            if ((addedValue.length) > 0) {
                var data = {
                    id: addedId,
                    text: addedValue
                };
                var newOption = new Option(data.text, data.id, true, true);
                $('#MaterialSpecId').append(newOption).trigger('change');
            }
        }
        addedId = "";
        addedValue = "";
    });

    $('#dialog-AddRMStandard').on('hide.bs.modal', function (e) {
     
        //if ((addedId.length) > 0)
        {
            if ((addedValue.length) > 0) {
                var data = {
                    id: addedId,
                    text: addedValue
                };
                var newOption = new Option(data.text, data.id, true, true);
                $('#Standard').append(newOption).trigger('change');

            }
        }
        addedId = "";
        addedValue = "";
    });

    $('#dialog-AddBaseRM').on('hide.bs.modal', function (e) {
        //if ((addedId.length) > 0)
        {
            if ((addedValue.length) > 0) {
                var data = {
                    id: addedId,
                    text: addedValue
                };
                var newOption = new Option(data.text, data.id, true, true);
                $('#BaseRawMaterialId').append(newOption).trigger('change');
             //   $('#BaseRawMaterialId').trigger('change');
            }
        }
        addedId = "";
        addedValue = "";
    });

});

function AddRMType() {
    if ($("#TypeForm").valid()) {
        ////debugger;
        var formData = AppUtil.GetFormData("TypeForm");
        api.post("/masters/rmtype", formData).then((data) => {
            // RawMaterialDetailFormUtil.UpdateFormIDs(data);
            //  document.getElementById("RawMetform").reset();
            addedId = "" + data.rawMaterialTypeId;
            addedValue = data.name;
            
           // CURDLG.modal('hide');
           // $('#RawMaterialTypeId').val(addedId);
            // $('#RawMaterialTypeId').trigger('change');
            document.getElementById("btn-close-AddRMType").click();
            //btn - close - AddRMType
        //    CURDLG.modal('hide');
            ////debugger;
        }).catch((error) => {
            AppUtil.HandleError("RawMetform", error);
        });
    } else {
        alert("Invalid form...")
    }
    //event.preventDefault();
}

function AddRMStandard() {
    if ($("#StandardForm").valid()) {
        //       ////debugger;
        var formData = AppUtil.GetFormData("StandardForm");
        api.post("/masters/rmstandard", formData).then((data) => {
            // RawMaterialDetailFormUtil.UpdateFormIDs(data);
            //  document.getElementById("RawMetform").reset();
            addedId = data.standard;
            addedValue = data.name;
            document.getElementById("btn-close-AddStandard").click();
           
        }).catch((error) => {
            AppUtil.HandleError("RawMetform", error);
        });
    } else {
        alert("Invalid form...")
    }
}

function AddRMSpec() {
    if ($("#SpecForm").valid()) {
        //       ////debugger;
        var formData = AppUtil.GetFormData("SpecForm");
        api.post("/masters/rmspec", formData).then((data) => {
            // RawMaterialDetailFormUtil.UpdateFormIDs(data);
            //  document.getElementById("RawMetform").reset();
            addedId = data.materialSpecId;
            addedValue = data.name;
            document.getElementById("btn-close-AddSpec").click();
        }).catch((error) => {
            AppUtil.HandleError("RawMetform", error);
        });
    } else {
        alert("Invalid form...")
    }
}

function AddBaseRM() {
    if ($("#BaseForm").valid()) {
        //       ////debugger;
        var formData = AppUtil.GetFormData("BaseForm");
        api.post("/masters/baserm", formData).then((data) => {
            // RawMaterialDetailFormUtil.UpdateFormIDs(data);
            //  document.getElementById("RawMetform").reset();
            addedId = data.baseRawMaterialId;
            addedValue = data.name;
            document.getElementById("btn-close-AddBaseRM").click();
        }).catch((error) => {
            AppUtil.HandleError("RawMetform", error);
        });
    } else {
        alert("Invalid form...")
    }
}

function loadParts(partNo) {//pass the element name
    api.get("/masters/partpurchases").then((data) => {
        //var div_data = "<option value=""></option>";
        for (i = 0; i < data.length; i++) {
            var arr = new AssociativeArray();
            arr.push("ID", data[i].partPurchaseId);
            arr.push("SupplierId", data[i].supplier);
            arr.push("LeadTimeInDays", data[i].leadTimeInDays);
            arr.push("MinimumOrderQuantity", data[i].minimumOrderQuantity);
            arr.push("Price", data[i].price);
            //var keyVals = data[i].entries();
            //console.log(arr);
            RawMaterialDetailFormUtil.ClearTable();
            RawMaterialDetailFormUtil.UpdatePurchaseDetailsTableFromPostData(arr);
            //   //debugger;
            //compSelect.append(div_data);
            //RawMaterialDetailFormUtil.ClearTable();
            //RawMaterialDetailFormUtil.UpdatePurchaseDetailsTable();
        }
        //compSelect.html(div_data);
        //filter once loaded

    }).catch((error) => {
        //console.log(error);
    });
}