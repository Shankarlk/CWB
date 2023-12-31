let customers = {};
let suppliers = {};
function OnCompanyCreated(data)//with companyId/companyName
{
    newCo = data.companyName;
    newCoId = data.companyId;
    document.getElementById("btnContactClose").click();
}

function OnCompDialogHidden() {

  //  alert("Tab "+CURRENT_TAB);
    if (CURRENT_TAB === "TabMPMain") {
        if ($("#CompanyId").length) {
            var data = {
                id: newCoId,
                text: newCo
            };
            var newOption = new Option(data.text, data.id, true, true);
            $('#CompanyId').append(newOption).trigger('change');
            $('#CompanyName').val(newCo);
        }
    }
    if (CURRENT_TAB === "TabRawMaterial") {
        if ($("#SupplierId").length) {
            var data = {
                id: newCoId,
                text: newCo
            };
            var newOption = new Option(data.text, data.id, true, true);
            $('#SupplierId').append(newOption).trigger('change');
            $('#Supplier').val(newCo);
        }
    }
    if (CURRENT_TAB === "TabPurchaseDetails") {
        if ($("#PSupplierId").length) {
            var data = {
                id: newCoId,
                text: newCo
            };
            var newOption = new Option(data.text, data.id, true, true);
            $('#PSupplierId').append(newOption).trigger('change');
            $('#PSupplier').val(newCo);
        }
    }
}
function loadCusomtersFromMem(CompanyOrSupplier) {
    if (customers.length > 0) {
        var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
        if (!compSelect.length)
            return;
        compSelect.empty();
        var div_data = "<option value=''></option>";
        compSelect.append(div_data);
        let i = 0;
        for (i = 0; i < customers.length; i++) {
            div_data = "<option value='" +
                customers[i].companyId + "'>" +
                customers[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }
}
function loadSuppliersFromMem(CustomerOrSupplier) {
    if (suppliers.length > 0) {
        var compSelect = $('#' + CustomerOrSupplier);//should be a select2 dropdown
        if (!compSelect.length)
            return;
        compSelect.empty();
        var div_data = "<option value=''></option>";
        compSelect.append(div_data);
        let i = 0;
        for (i = 0; i < suppliers.length; i++) {
            div_data = "<option value='" +
                suppliers[i].companyId + "'>" +
                suppliers[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }
}

function loadSuppliers(CustomerOrSupplier) {
    var compSelect = $('#' + CustomerOrSupplier);//should be a select2 dropdown
    if (!compSelect.length)
        return;
    compSelect.empty();
    var div_data = "<option value=''></option>";
    compSelect.append(div_data);
    api.get("/masters/suppliers").then((data) => {
        suppliers = data;
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].companyId + "'>" +
                data[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}
function loadCustomers(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    if (!compSelect.length)
        return;
    compSelect.empty();
    customers = {};
    ////debugger;
    var div_data = "<option value=''></option>";
    compSelect.append(div_data);
    api.get("/masters/customers").then((data) => {
        customers = data;
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].companyId + "'>" +
                data[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}

//RoutingSelectSupplierTable
//RoutingSelectSupplierRow
function loadSuppliersToTable(tableName,rowTemplate) {
    var tablebody = $("#"+tableName+" tbody");
    $(tablebody).html("");//empty tbody
    api.get("/masters/suppliers").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew(rowTemplate, data[i],i));
        }
        console.log($(tablebody).html());
    }).catch((error) => {
        //console.log(error);
    });
}    