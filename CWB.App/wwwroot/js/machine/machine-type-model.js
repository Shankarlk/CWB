var MachineTypeFormUtil = {
    ClearForm: () => {
        $("#MachineTypeName").val("");
        $("#MachineTypeTypeId").val("0");
        $("#frmMachineType").resetValidation();
    },
    UpdateMachineType: (data) => {
        var machineTypeSelect = $("#MachineMachineTypeId");
        var machineTypeOptions = $(machineTypeSelect).find("option[value='" + data.machineTypeTypeId + "']");
        if (machineTypeOptions.length == 0) {
            $(machineTypeSelect).append('<option value="' + data.machineTypeTypeId + '">' + data.machineTypeName + '</option>');
        } else {
            $(machineTypeOptions).text(data.machineTypeName);
        }
        $("#MachineMachineTypeId").val(data.machineTypeTypeId);
        MachineTypeFormUtil.ClearForm();
        $('#machine-type-dialog').modal('hide');
    },
    LoadMachineTypeForm: (isNew) => {
        var machineTypeSelect = $("#MachineMachineTypeId");
        if ($(machineTypeSelect).val() == "" || isNew) {
            MachineTypeFormUtil.ClearForm();
            return;
        }
        $("#MachineTypeName").val($(machineTypeSelect).find("option:selected").text());
        $("#MachineTypeTypeId").val($(machineTypeSelect).val());
    }
};
$(function () {
    $("#btnMachineTypeSubmit").click(function () {
        if ($("#frmMachineType").valid()) {
            var formData = AppUtil.GetFormData("frmMachineType");
            api.post("/Machine/MachineType", formData).then((data) => {
                MachineTypeFormUtil.UpdateMachineType(data);
            }).catch((error) => {
                AppUtil.HandleError("frmMachineType", error);
            });
        }
    });

    $('#machine-type-dialog').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        MachineTypeFormUtil.LoadMachineTypeForm($(relatedTarget).data("isnew"));
    });

    $('#machine-type-dialog').on('hide.bs.modal', function (event) {
        MachineTypeFormUtil.ClearForm();
    });
});