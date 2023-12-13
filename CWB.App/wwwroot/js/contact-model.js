var ContactsConstants = {
    DivisionId: 0
};
var ContactsFormUtil = {
    UpdateFormIDs: (data) => {
        //////debugger;
        $("#DivisionId").val(data.divisionId);
        $("#CompanyId").val(data.companyId);
    },
    PopulateForm: (data) => {
        //////debugger;
        $("#DivisionId").val(data.divisionId);
        $("#CompanyId").val(data.companyId);
        $("#CompanyType").val(data.companyType);
        $("#CompanyName").val(data.companyName);
        $("#DivisionName").val(data.divisionName);
        $("#Location").val(data.location);
        $("#Notes").val(data.notes);
    },
    UpdateDivisonTable: (companyId, divisionId) => {
        //////debugger;
        ContactsConstants.DivisionId = divisionId;
        api.get("/contacts/divisions/" + companyId).then((data) => {
            var tablebody = $("#tbl-division tbody");
            $(tablebody).html("");//empty tbody
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("division-template", data[i]));
                if (ContactsConstants.DivisionId == data[i].divisionId) {
                    ContactsFormUtil.PopulateForm(data[i]);
                }
            };
        }).catch((error) => {

        });
    },
    ClearConstants: () => {
        ContactsConstants.DivisionId = 0;
    },
    ClearForm: () => {
        $("#DivisionId").val("0");
        $("#CompanyId").val("0");
        $("#CompanyType").val("");
        $("#CompanyName").val("");
        $("#DivisionName").val("");
        $("#Location").val("");
        $("#Notes").val("");
    },
    ClearDivision: () => {
        $("#DivisionId").val("0");
        $("#DivisionName").val("");
        $("#Location").val("");
        $("#Notes").val("");
    },
    HasFunction: (obj, methodName) => {
        return ((typeof obj[methodName]) == "function");
    }
};

$(function () {
    $("#btnAddDivision").hide();
    $('#dialog-company').on('show.bs.modal', function (event) {
        ContactsFormUtil.ClearForm();
        ContactsFormUtil.ClearConstants();
        var relatedTarget = $(event.relatedTarget);
        var companyId = relatedTarget.data("id");
        var divisionId = relatedTarget.data("divisionId");
        if (companyId == "0") {
            var tablebody = $("#tbl-division tbody");
            $(tablebody).html("");//empty tbody
            $("#btnAddDivision").hide();
            return;
        }
        $("#btnAddDivision").show();
        ContactsFormUtil.UpdateDivisonTable(companyId, divisionId);

    });
    $('#dialog-company').on('hide.bs.modal', function (event) {
        ContactsFormUtil.ClearForm();
        ContactsFormUtil.ClearConstants();
    //    alert("On hide..");
       // $("#frmCompany").resetValidation();
        api.get("/contacts/companies").then((data) => {
            var tablebody = $("#tbl-contacts tbody");
            if (tablebody.length) { //if there is a tablebody in the parent populate it
                $(tablebody).html("");//empty tbody
                for (i = 0; i < data.length; i++) {
                    $(tablebody).append(AppUtil.ProcessTemplateData("company-template", data[i]));
                }
            }
            if (typeof OnCompDialogHidden === 'function') {
                OnCompDialogHidden();
            }
            //filter once loaded
            ContactsUtil.FilterContacts();
        }).catch((error) => {

        });
    });

    /*$("#btnContactClose").click(function () {
        $("#dialog-company").dialog("close");
    });*/

    $("#btnContactSubmit").click(function () {
     //   //debugger;
        if ($("#frmCompany").valid()) {
            var formData = AppUtil.GetFormData("frmCompany");
            api.post("/contacts/company", formData).then((data) => {
                ContactsFormUtil.UpdateFormIDs(data);
                ContactsFormUtil.UpdateDivisonTable(data.companyId, data.divisionId);
                $("#btnAddDivision").show();
                if (typeof OnCompanyCreated === 'function') {
                    OnCompanyCreated(data);
                }
            }).catch((error) => {
                AppUtil.HandleError("frmCompany", error);
            });
        }
    });
    $("#btnAddDivision").click(function () {
        //////debugger;
        ContactsFormUtil.ClearDivision();
        ContactsFormUtil.ClearConstants();
    });
});