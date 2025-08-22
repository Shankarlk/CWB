let plants = {};
let sectiondepart = {};

function LoadDepartments() {
    var tablebody = $("#DeptTable tbody");
    $(tablebody).html("");//empty tbody
    api.get("/department/getdepartments").then((data) => {
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            if (data[i].prodDept) {
                data[i].prodDept = "Y";
            }
            else
                data[i].prodDept = "";
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("DeptRow", data[i], i));
        }
        //console.log($(tablebody).html());
    }).catch((error) => {
        //console.log(error);
    });
};
function LoadPlantsInMem() {
    api.get("/plant/getplants").then((data) => {
        //console.log(data);
        plants = data;
    }).catch((error) => {
        //console.log(error);
    });
}
function LoadPlants() {
    var selElem = $('#PlantId');//should be a select2 dropdown
    if (!selElem.length)
        return;
    selElem.empty();
    var div_data = "<option value=''>Select</option>";
    selElem.append(div_data);
    var data = plants;
    for (i = 0; i < data.length; i++) {
        div_data = "<option value='" + data[i].plantId + "' data-shifts='" + data[i].noOfShifts + "'>" +
            data[i].name +
            "</option>";
        selElem.append(div_data);
    }
};
function DelDept(name, deptId) {
    let confirmval = confirm("Are your sure you want to delete this department? : "+name, "Yes", "No");
    if (confirmval) {
        api.get("/department/deldept?departmentId=" + deptId).then((data) => {
            //console.log(data);
            LoadDepartments();
        }).catch((error) => {
            //console.log(error);
        });
    }
};


$(function () {
    //activity: null    departmentId: 1    name: "Production"
    //noOfShifts: 3    plantId: 1    plantName: "test"
    LoadPlantsInMem();
    $('#add-dept').on('shown.bs.modal', function (event) {
        // gdalert("add dept...")
        document.getElementById("DepartmentForm").reset();
        LoadPlants();
        if (IsAddOpCalled())
            return;
        var relatedTarget = $(event.relatedTarget);
        //console.log(relatedTarget);
        var strval = relatedTarget.data("name");
        //alert(strval);
        $("#Name").val(strval);
        strval = relatedTarget.data("deptid");
        //alert(strval);
        $("#DepartmentId").val(strval);

        strval = relatedTarget.data("noofshifts");
        //alert(strval);
        $("#NoOfShifts").val(strval).change();
        $("#NoOfShifts").change();



        strval = relatedTarget.data("plantid");
        //alert(strval);
        $("#PlantId").val(strval).change();
        $("#PlantId").change();


        strval = relatedTarget.data("activity");
        $("#Activity").val(strval);
        //alert(strval);
        val = relatedTarget.data("section");
        if (val == "") {
            val = "-";
        }
        $("#Section").val(val);
        //alert(val);
        val = relatedTarget.data("proddept");
        //console.log(val);
        document.getElementById("ProdDept").checked = false;
        if (val == "Y") {
            document.getElementById("ProdDept").checked = true;
        }
    });
    $("#PlantId").on("change", function () {
        var selected = $(this).find("option:selected");
        var maxShifts = selected.data("shifts"); 
        var noOfShiftsDropdown = $("#NoOfShifts");

        noOfShiftsDropdown.empty();
        if (maxShifts) {
            for (var j = 1; j <= maxShifts; j++) {
                noOfShiftsDropdown.append("<option value='" + j + "'>" + j + "</option>");
            }
        }
    });
    $("#SaveDept").on("click", function (event) {
        var formData = AppUtil.GetFormData("DepartmentForm");
        //console.log(formData);
        var form = document.getElementById("DepartmentForm");
        if (form.checkValidity())
        {
            api.post("/department/postdepartment", formData).then((data) => {
                //console.log(data);
                LoadDepartments();
                document.getElementById("AddDeptClose").click();
            }).catch((error) => {
                AppUtil.HandleError("DepartmentForm", error);
            });
        }
        else {
            //alert("Invalid form");
        //    form.classList.add("was-validated");
        }
    });
    LoadDepartments();
    $('#Addsection').on('shown.bs.modal', function (event) {
        var newNamevalidate = document.getElementById('SectionName');
        newNamevalidate.style.border = '';
        var relatedTarget = $(event.relatedTarget);
        var deptid = relatedTarget.data("deptid");
        loadSection(deptid);
        $("#SectionName").val('');
        $("#secdeptId").val(deptid);
        $("#secId").val('0');
    });
    $("#btnAddSection").on("click", function (event) {
        var deptId = $("#secdeptId").val();
        var secId = $("#secId").val();
        var SectionName = $("#SectionName").val();

        if (SectionName.length <= 0) {
            var newNamevalidate = document.getElementById('SectionName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('SectionName');
            newNamevalidate.style.border = '';
        }
        if (isNaN(secId)) {
            secId = 0;
        }
        var rowData = {
            sectionsId: parseInt(secId),
            name: SectionName,
            shopDepartmentId: parseInt(deptId)
        };
        api.post("/department/PostSection", rowData).then((data) => {
            $("#SectionName").val('');
            $("#secId").val('0');
            alert("Section Saved Successfully!");
            loadSection(parseInt(deptId));
        }).catch((error) => {
        });
    });
});
function loadSection(deptId) {
    var tablebody = $("#tbl-section tbody");
    $(tablebody).html("");//empty tbody
    api.get("/department/GetSections").then((data) => {
        //console.log(data);
        data = data.filter(item => item.shopDepartmentId === deptId)
        sectiondepart = data;
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("tbl-sectionRow", data[i], i));
        }
        //console.log($(tablebody).html());
    }).catch((error) => {
        //console.log(error);
    });
};
function EditSection(element) {
    var relatedTarget = $(element);
    var deptid = relatedTarget.data("deptid");
    var sectionsid = relatedTarget.data("sectionsid");
    var name = relatedTarget.data("name");
    $("#SectionName").val(name);
    $("#secdeptId").val(deptid);
    $("#secId").val(sectionsid);
}

        /**
         * 
    
         */