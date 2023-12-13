var OperationListFormUtil = {
    UpdateFormIDs: (operationId) => {
        $("#OperationId").val(operationId);
        $("#OperationListIdForDocType").val(operationId);
    },
    UpdateOperationForm: (data) => {
        $("#OperationId").val(data.operationId);
        $("#OperationListIdForDocType").val(data.operationId);
        $("#Operation").val(data.operation);
        $("#IsMultiplePartsOfBOMUsed").prop('checked', data.isMultiplePartsOfBOMUsed).change();
    },
    ClearOperationForm: () => {
        $("#OperationId").val("0");
        $("#Operation").val("");
        $("#IsMultiplePartsOfBOMUsed").prop('checked', false).change();
    },
    ClearOperationListDocForm: () => {
        $("#OperationDocumentTypeId").val("");
        $("#OperationListIdForDocType").val("0");
        $("#IsOperationDocumentMandatory").prop('checked', false).change();
    },
    ResetOperationListDocForm: () => {
        $("#OperationDocumentTypeId").val("");
        $("#IsOperationDocumentMandatory").prop('checked', false).change();
    },
    ShowOperationListDocContainer: () => {

        $("#containerOperationListDocType").show();
    },
    HideOperationListDocContainer: () => {
        $("#containerOperationListDocType").hide();
    },
    LoadOperation: (operationId) => {
        api.get("/operationlist/Operation/" + operationId).then((data) => {
            OperationListFormUtil.UpdateOperationForm(data);
            OperationListFormUtil.LoadOperationListDocs(data.operationId);
        }).catch((error) => {

        });
    },
    LoadOperationListDocs: (operationId) => {
        api.get("/operationlist/GetOperationalDocuments/" + operationId).then((data) => {
            var tablebody = $("#tbl-operation-list-doc tbody");
            $(tablebody).html("");//empty tbody
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("operationlist-doc-template", data[i]));
            }
            OperationListFormUtil.ShowOperationListDocContainer();
        }).catch((error) => {

        });

    },
    LoadOperationDocTypes: (operationId) => {
        api.get("/operationlist/GetDocTypes/" + operationId).then((data) => {
            var docTypeSelect = $("#OperationDocumentTypeId");
            $(docTypeSelect).html("");
            $(docTypeSelect).append('<option value="">--Select Document Type--</option>');
            for (i = 0; i < data.length; i++) {
                $(docTypeSelect).append('<option value="' + data[i].documentTypeId + '">' + data[i].name + '</option>');
            }

        }).catch((error) => {

        });
    },
    ProcessOperationList: (operationId, IsLoadOperation) => {
        OperationListFormUtil.UpdateFormIDs(operationId);
        if (IsLoadOperation) {
            OperationListFormUtil.LoadOperation(operationId);
        }
        OperationListFormUtil.LoadOperationDocTypes(operationId);
        OperationListFormUtil.ShowOperationListDocContainer();
    },
    ProcessOperationDocumentList: (operationId) => {
        OperationListFormUtil.ResetOperationListDocForm();
        OperationListFormUtil.LoadOperationListDocs(operationId);
        OperationListFormUtil.LoadOperationDocTypes(operationId);
    }

};
$(function () {
    //Hide operation document mapping initially
    OperationListFormUtil.HideOperationListDocContainer();
    $('#dialog-operation').on('show.bs.modal', function (event) {
        OperationListFormUtil.ClearOperationForm();
        OperationListFormUtil.ClearOperationListDocForm();
        $("#frmOperationlist").resetValidation();
        $("#frmOperationlistdoc").resetValidation();
        var relatedTarget = $(event.relatedTarget)
        var operationId = relatedTarget.data("id");
        if (operationId == "0") {
            var tablebody = $("#tbl-operation-list-doc tbody");
            $(tablebody).html("");//empty tbody
            OperationListFormUtil.HideOperationListDocContainer();
            return;
        }
        OperationListFormUtil.ProcessOperationList(operationId, true);

    });
    $('#dialog-operation').on('hide.bs.modal', function (event) {
        OperationListFormUtil.ClearOperationForm();
        OperationListFormUtil.ClearOperationListDocForm();
        api.get("/operationlist/Operations").then((data) => {
            var tablebody = $("#tbl-operation-list tbody");
            $(tablebody).html("");//empty tbody
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("operationlist-template", data[i]));
            }
            //filter once loaded
            OperationListUtil.FilterOperationList();
        }).catch((error) => {

        });
    });
    $("#btnOperationSubmit").click(function () {
        if ($("#frmOperationlist").valid()) {
            var formData = AppUtil.GetFormData("frmOperationlist");
            api.post("/operationlist/Operation", formData).then((data) => {
                OperationListFormUtil.ProcessOperationList(data.operationId, false);
            }).catch((error) => {
                AppUtil.HandleError("frmOperationlist", error);
            });
        }
    });
    $("#btnOperationDocTypeSubmit").click(function () {
        if ($("#frmOperationlistdoc").valid()) {
            var formData = AppUtil.GetFormData("frmOperationlistdoc");
            api.post("/operationlist/OperationalDocuments", formData).then((data) => {
                OperationListFormUtil.ProcessOperationDocumentList(data.operationListIdForDocType);
            }).catch((error) => {
                AppUtil.HandleError("frmOperationlist", error);
            });
        }
    });
    $("#tbl-operation-list-doc").on("click", "button.edit", function () {
        var docid = $(this).data("id");
        var doctype = $(this).data("doctype");
        var doctypeId = $(this).data("doctypeid");
        var docMandatory = $(this).data("mandatory");
        var docTypeSelect = $("#OperationDocumentTypeId");
        var docTypeoption = $(docTypeSelect).find("option[value='" + doctypeId + "'");
        if (docTypeoption.length === 0) {
            $(docTypeSelect).append('<option value="' + doctypeId + '">' + doctype + '</option>');
        }       
        $(docTypeSelect).val(doctypeId);
        $("#IsOperationDocumentMandatory").prop('checked', docMandatory).change();
        $("#OperationListDocumentId").val(docid);
    });
});