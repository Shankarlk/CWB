var MachineProcDocUtil = {
    LoadMachineProcDocList: (machineId) => {
        api.get("/Machine/GetMachineProcDocs/" + machineId).then((data) => {
            var tablebody = $("#tbl-machine-proc-doc-list tbody");
            $(tablebody).html("");//empty tbody
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("template-machine-proc-doc", data[i]));
            }
        }).catch((error) => {

        });
    },
    LoadMachineDocType: (machineId) => {
        api.get("/Machine/GetDocTypes/" + machineId).then((data) => {
            var docTypeSelect = $("#MachineProcDocumentTypeId");
            $(docTypeSelect).html("");
            $(docTypeSelect).append('<option value="">--Select Document Type--</option>');
            for (i = 0; i < data.length; i++) {
                $(docTypeSelect).append('<option value="' + data[i].documentTypeId + '">' + data[i].name + '</option>');
            }
        }).catch((error) => {

        });
    },
    ResetMachineProcDocForm: () => {
        $("#MachineProcDocumentId").val(0);
        $("#MachineProcDocumentTypeId").val("");
        $("#IsMachineProcDocumentMandatory").prop('checked', false).change();
        $("#frmMachineProcsDoc").resetValidation();

    },
    ResetMachineProcDocType: () => {
        //reset table
        var tablebody = $("#tbl-machine-proc-doc-list tbody");
        $(tablebody).html("");//empty tbody
        var docTypeSelect = $("#MachineProcDocumentTypeId");
        $(docTypeSelect).html("");
    }
};

$(function () {
    $("#btnMachineProDocSubmit").click(function () {
        if ($("#frmMachineProcsDoc").valid()) {
            var formData = AppUtil.GetFormData("frmMachineProcsDoc");
            api.post("/Machine/MachineProcDoc", formData).then((data) => {
                MachineProcDocUtil.ResetMachineProcDocForm();
                MachineProcDocUtil.LoadMachineDocType(data.machineProcDocumentMachineId);
                MachineProcDocUtil.LoadMachineProcDocList(data.machineProcDocumentMachineId);

            }).catch((error) => {
                AppUtil.HandleError("frmMachineProcsDoc", error);
            });
        }
    });
    $("#tbl-machine-proc-doc-list").on("click", "button.edit", function () {
        var docid = $(this).data("id");
        var doctype = $(this).data("doctype");
        var doctypeId = $(this).data("doctypeid");
        var docMandatory = $(this).data("mandatory");
        var docTypeSelect = $("#MachineProcDocumentTypeId");
        var docTypeoption = $(docTypeSelect).find("option[value='" + doctypeId + "'");
        if (docTypeoption.length === 0) {
            $(docTypeSelect).append('<option value="' + doctypeId + '">' + doctype + '</option>');
        }
        $(docTypeSelect).val(doctypeId);
        $("#IsMachineProcDocumentMandatory").prop('checked', docMandatory).change();
        $("#MachineProcDocumentId").val(docid);
    });
    
})