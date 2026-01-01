let plants = {};
let sectiondepart = {};

function LoadDepartments() {
    var tablebody = $("#DeptTable tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();
    api.get("/department/GetDepartmentsLevel").then((data) => {
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
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
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
function loadRole(deptid) {

    var tablebody = $("#DeptRoleGrid tbody");
    $(tablebody).html("");//empty tbody
    api.get("/department/GetDept_Role_List").then((data) => {
        //console.log(data);
        data = data.filter(i => i.dept_Struct_Id == parseInt(deptid));
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("DeptRoleRow", data[i], i));
        }
        //console.log($(tablebody).html());
    }).catch((error) => {
        //console.log(error);
    });
}

function loadSelectRole() {
    var OrgEmpl = $('#OrgRole');
    OrgEmpl.html('');

    api.get("/Employee/GetAllRoleList").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        OrgEmpl.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].role_ListId + "'>" + data[i].role_Desc + "</option>";
            OrgEmpl.append(div_data);
        }
    }).catch((error) => {
    });
}
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
        var tablebody = $("#DeptRoleGrid tbody");
        $(tablebody).html("");//empty tbody
        $("#lblNoOfShift").hide();
        $("#NoOfShiftsDiv").hide();
        //$("#lblActfun").hide();
        //$("#ActFunDiv").hide();
        loadSelectRole();
        $("#DepartmentId").val(0);
        document.getElementById("DepartmentForm").reset();
        LoadPlants();
        var relatedTarget = $(event.relatedTarget);
        var partOfId = relatedTarget.data("deptid");
        var partof = relatedTarget.data("partof");
        if (parseInt(partOfId) > 0 && typeof partof === "undefined") {
            $("#Part_Of").val(parseInt(partOfId));
            $("#DepartmentId").val(0);
        } else {
            if (isNaN(partof)) {
                $("#Part_Of").val(parseInt(0));
            } else {
                $("#Part_Of").val(parseInt(partof));
                $("#DepartmentId").val(partOfId);
            }
        }
        if (IsAddOpCalled())
            return;
        //var relatedTarget = $(event.relatedTarget);
        //console.log(relatedTarget);
        var strval = relatedTarget.data("name");
        //alert(strval);
        $("#Name").val(strval);
        strval = relatedTarget.data("deptid");
        ////alert(strval);
        //$("#DepartmentId").val(strval);
        loadRole(parseInt(strval));
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
            $("#NoOfShiftsDiv").show();
            $("#lblNoOfShift").show();
            //$("#lblActfun").show();
            //$("#ActFunDiv").show();
        } else {
            $("#NoOfShifts").val(1).change();
            $("#NoOfShiftsDiv").hide();
            $("#lblNoOfShift").hide();
            //$("#lblActfun").hide();
            //$("#ActFunDiv").hide();
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
    $('#addRole').on('hidden.bs.modal', function (event) {
        document.getElementById('add-dept').style.filter = 'none';
    });

    $('#addRole').on('shown.bs.modal', function (event) {
        document.getElementById('add-dept').style.filter = 'blur(5px)';
        $("#OrgRole").val(0);
        var newNamevalidate = document.getElementById('OrgRole');
        newNamevalidate.style.border = '';
    });
    $("#OrgSave").secureClick( function (event) {
        var id = $("#POrgId").val(); 
        var deptId = $("#DepartmentId").val();
        var roleId = parseInt($("#OrgRole").val());
        if (isNaN(parseInt(id))) {
            id = 0;
        }

        if (roleId === 0) {
            var newNamevalidate = document.getElementById('OrgRole');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('OrgRole');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            dept_Role_ListId: parseInt(id),
            dept_Struct_Id: parseInt(deptId),
            role_Access_Id: parseInt(roleId)
        };

        return api.get("/department/GetDept_Role_List").then((data) => {
            //console.log(data);
            var existingRoles = data.filter(i => i.dept_Struct_Id === parseInt(deptId));

            // check if the same role already exists
            var duplicate = existingRoles.some(i => i.role_Access_Id === parseInt(roleId));

            if (duplicate) {
                alert("This role is already assigned to the department!");
                return; // stop here
            }
            api.post("/department/PostDept_Role_List", rowData).then((data) => {
                loadRole(deptId);
                LoadDepartments();
                $("#addRole").modal("hide");
            }).catch((error) => {
            });
        }).catch((error) => {
            //console.log(error);
        });
    });
    $("#SaveDept").secureClick(function (event) {
        var formData = AppUtil.GetFormData("DepartmentForm");
        //console.log(formData);
        var form = document.getElementById("DepartmentForm");
        if (form.checkValidity())
        {
            return  api.post("/department/postdepartment", formData).then((data) => {
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
    $("#RoleUnassigned").change(function () {
        if ($(this).is(":checked")) {
            $("#DeptTable tbody tr").filter(function () {
                // check first column text
                let firstCol = $(this).children("td").eq(5).text().trim();
                return firstCol !== ""; // hide rows where column is NOT empty
            }).hide();

            $("#DeptTable tbody tr").filter(function () {
                // show rows where first column is empty
                let firstCol = $(this).children("td").eq(5).text().trim();
                return firstCol === "";
            }).show();
        } else {
            $("#DeptTable tbody tr").show();
        }

        var $tableBody = $("#DeptTable tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#ProdDept").change(function () {
        if ($(this).is(":checked")) {
            $("#NoOfShiftsDiv").show();
            $("#lblNoOfShift").show();
            //$("#lblActfun").show();
            //$("#ActFunDiv").show();
        } else {
            $("#NoOfShifts").val(1).change();
            $("#NoOfShiftsDiv").hide();
            $("#lblNoOfShift").hide();
            //$("#lblActfun").show();
            //$("#ActFunDiv").show();
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
            LoadDepartments();
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
function DeleteDeptRole(element) {
    var relatedTarget = $(element);
    var deptid = relatedTarget.data("deptroleid");
    let confirmval = confirm("Are your sure you want to delete this role?", "Yes", "No");
    if (confirmval) {
        api.get("/department/DelDept_Role_List?designationId=" + deptid).then((data) => {
            //console.log(data);
            var deptId = $("#DepartmentId").val();
            loadRole(deptId);
            LoadDepartments();
        }).catch((error) => {
            //console.log(error);
        });
    }
}

function DeleteSection(element) {
    var relatedTarget = $(element);
    var deptid = relatedTarget.data("deptid");
    var sectionsid = relatedTarget.data("sectionsid");
    var name = relatedTarget.data("name");
    let confirmval = confirm("Are your sure you want to delete this section?", "Yes", "No");
    if (confirmval) {
        api.get("/department/DelSections?designationId=" + sectionsid).then((data) => {
            //console.log(data);
            LoadDepartments();
            loadSection(deptid);
        }).catch((error) => {
            //console.log(error);
        });
    }
}

        /**
         * 
    
         */