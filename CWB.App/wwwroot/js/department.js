$(function () {
    function LoadDepartments() {
        var tablebody = $("#DeptTable tbody");
        $(tablebody).html("");//empty tbody
        api.get("/department/getdepartments").then((data) => {
            console.log(data);
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("DeptRow", data[i], i));
            }
        }).catch((error) => {
            //console.log(error);
        });
    };
    function LoadPlants() {
        var selElem = $('#PlantId');//should be a select2 dropdown
        if (!selElem.length)
            return;
        selElem.empty();
        var div_data = "<option value=''></option>";
        selElem.append(div_data);

        api.get("/plant/getplants").then((data) => {
            console.log(data);
            operations = data;
            for (i = 0; i < data.length; i++) {
                div_data = "<option value='" +
                    data[i].plantId + "'>" +
                    data[i].name +
                    "</option>";
                selElem.append(div_data);
            }
        }).catch((error) => {
            //console.log(error);
        });
    };
    $("#SaveDept").click(function (event) {
        var formData = AppUtil.GetFormData("DepartmentForm");
        api.post("/department/postdepartment", formData).then((data) => {
            console.log(data);
            LoadDepartments();
            document.getElementById("AddDeptClose").click();
        }).catch((error) => {
            AppUtil.HandleError("FormRoutingSupplier", error);
        });
        event.preventDefault();
    });
    $('#add-dept').on('show.bs.modal', function (event) {
        LoadPlants();
        document.getElementById("DepartmentForm").reset();
    });

    LoadDepartments();
});