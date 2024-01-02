var DesignationsConstants = {
    DivisionId: 0
};
var DesignationsFormUtil = {
    UpdateFormIDs: (data) => {
        //////debugger;
        $("#DesignationId").val(data.designationId);
    },
    PopulateForm: (data) => {
        //////debugger;
        $("#DesignationId").val(data.designationId);
        $("#DesignationName").val(data.designationName);
    },
    ClearForm: () => {
        $("#DesignationId").val("0");
        $("#DesignationName").val("");
    },
    HasFunction: (obj, methodName) => {
        return ((typeof obj[methodName]) == "function");
    }
};

$(function () {
    $('#dialog-designation').on('show.bs.modal', function (event) {
        DesignationsFormUtil.ClearForm();
        var relatedTarget = $(event.relatedTarget);
        var DesignationId = relatedTarget.data("id");
        //if (DesignationId == "0") {
        //    var tablebody = $("#tbl-division tbody");
        //    $(tablebody).html("");//empty tbody
        //    $("#btnAddDivision").hide();
        //    return;
        //}
    });
    $('#dialog-designation').on('hide.bs.modal', function (event) {
        DesignationsFormUtil.ClearForm();
    //    alert("On hide..");
       // $("#frmDesignation").resetValidation();
        api.get("/designation/designations").then((data) => {
            var tablebody = $("#tbl-designations tbody");
            if (tablebody.length) { //if there is a tablebody in the parent populate it
                $(tablebody).html("");//empty tbody
                for (i = 0; i < data.length; i++) {
                    $(tablebody).append(AppUtil.ProcessTemplateData("designation-template", data[i]));
                }
            }
            if (typeof OnDesigDialogHidden === 'function') {
                OnDesigDialogHidden();
            }
            //filter once loaded
            ContactsUtil.FilterContacts();
        }).catch((error) => {

        });
    });

    /*$("#btnContactClose").click(function () {
        $("#dialog-Designation").dialog("close");
    });*/

    $("#btnDesignationSubmit").click(function () {
     //   //debugger;
        if ($("#frmDesignation").valid()) {
            var formData = AppUtil.GetFormData("frmDesignation");
            api.post("/designation/designation", formData).then((data) => {
                DesignationsFormUtil.UpdateFormIDs(data);
                if (typeof OnDesignationCreated === 'function') {
                    OnDesignationCreated(data);
                }
            }).catch((error) => {
                AppUtil.HandleError("frmDesignation", error);
            });
        }
    });

});