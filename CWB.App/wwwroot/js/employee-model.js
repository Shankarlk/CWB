var menusdata = {};
var menusdataEmpl = {};
var Departments = {};






$(function () {
    //Search Designation --
    LoadEmployee();
    $("#SearchEmplno").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchEmplName").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchLoc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchEmailId").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchDateOfJoin").on("change", function () {
        var selectedDate = $(this).val(); 
        var formattedSelectedDate = new Date(selectedDate);
        var month = (formattedSelectedDate.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-based
        var day = formattedSelectedDate.getDate().toString().padStart(2, '0');
        var year = formattedSelectedDate.getFullYear();
        var finalSelectedDate = month + '/' + day + '/' + year; 
        $("#EmployeeGrid tbody tr").filter(function () {
            var rowDate = $(this).children("td").eq(2).text();
            if (rowDate === finalSelectedDate || selectedDate === "") {
                $(this).show(); 
            } else {
                $(this).hide(); 
            }
        });
    });
    $("#UPPartOf").select2({
        dropdownParent: $("#addUi")
    });

   
    loadDesignation();
    loadDepartment();
    loadLoaction();
    $('#addEmployee').on('hidden.bs.modal', function (event) {
        $("#error-password").text(" ").css("color", "red");
        $("#error-username").text(" ").css("color", "red");
        $("#EPEmpId").val(''); $("#UiAccessEEmplid").val('');
        $("#EPEmpNo").val('');
        $("#EPEmpName").val('');
        $("#EPDateOfJoin").val('');
        $("#EPDateOfLeave").val('');
        $("#EPCell").val('');
        $("#EPEmail").val('');
        $("#EPAddress").val('');
        $("#EPContPer").val('');
        $("#EPContNo").val('');
        $("#EPEmpId").val('');
        $("#EPEmpRoles").val('');
        $("#EPEmpRoleid").val('');
       // $("#EPUserName").val('');
        $("#EPCfmPassword").val('');
        $("#EPPassword").val('');
        $("#EPDes").val(0).change(); // Set designation and trigger change
        $("#EPDept").val(0).change(); // Set department and trigger change
        $("#EPloc").val(0).change();
        $("#EPResignChk").prop("checked", false);
        $("#EPHeadOrg").prop("checked", false);
        var EPEmpNo = document.getElementById('EPEmpNo');
        EPEmpNo.style.border = '';
        var EPDateOfJoin = document.getElementById('EPDateOfJoin');
        EPDateOfJoin.style.border = '';
        var EPEmpName = document.getElementById('EPEmpName');
        EPEmpName.style.border = '';
        var EPCell = document.getElementById('EPCell');
        EPCell.style.border = '';
        var EPEmail = document.getElementById('EPEmail');
        EPEmail.style.border = '';
        var EPAddress = document.getElementById('EPAddress');
        EPAddress.style.border = '';
        var EPDes = document.getElementById('EPDes');
        EPDes.style.border = '';
        var EPDept = document.getElementById('EPDept');
        EPDept.style.border = '';
        var EPloc = document.getElementById('EPloc');
        EPloc.style.border = '';
        var EPDateOfLeave = document.getElementById('EPDateOfLeave');
        EPDateOfLeave.style.border = '';
        //var EPUserName = document.getElementById('EPUserName');
        //EPUserName.style.border = '';
        var EPPassword = document.getElementById('EPPassword');
        EPPassword.style.border = '';
        var EPCfmPassword = document.getElementById('EPCfmPassword');
        EPCfmPassword.style.border = '';
        var EPContPer = document.getElementById('EPContPer');
        EPContPer.style.border = '';
        var EPContNo = document.getElementById('EPContNo');
        EPContNo.style.border = '';
        var UiAccessRMenu1 = $('#EPDept');
        UiAccessRMenu1.html('');
        var EPRoleReportTo = document.getElementById('EPRoleReportTo');
        EPRoleReportTo.style.border = '';
    });
    $('#addEmployee').on('show.bs.modal', function (event) {
        $("#DateOfLeaveDIv").hide();
        $("#RoleReportDIv").show();
        $("#empno-error").text("").css("color", "red");
        $("#email-error").text("").css("color", "red");
        var relatedTarget = $(event.relatedTarget);
        var employee_ID = relatedTarget.data("employeeid");
        var employeeno = relatedTarget.data("employeeno");
        var emplname = relatedTarget.data("emplname");
        var dept = relatedTarget.data("dept");
        var desg = relatedTarget.data("desg");
        var cell = relatedTarget.data("cell");
        var email = relatedTarget.data("email");
        var uname = relatedTarget.data("uname");
        var rolereport = relatedTarget.data("rolereport");
        var headoforg = relatedTarget.data("headoforg");
        var eroleids = relatedTarget.data("eroleids");
        var passwd = relatedTarget.data("passwd");
        var dateofjoin = relatedTarget.data("dateofjoin");
        var chkresign = relatedTarget.data("chkresign");
        var date_Of_Resigning = relatedTarget.data("dateofresigning");
        var plant_Id = relatedTarget.data("plantid");
        var emerg_Contact_Name = relatedTarget.data("emergcontactname");
        var emerg_Contact_No = relatedTarget.data("emergcontactno");
        var address = relatedTarget.data("address");
        if (employee_ID > 0) {
            $("#EPEmpId").val(employee_ID);
            $("#EPEmpNo").val(employeeno);
            $("#EPEmpName").val(emplname);
            var formattedDate = dateofjoin.split("T")[0]; 
            $("#EPDateOfJoin").val(formattedDate);
            $("#EPCell").val(cell);
            $("#EPPassword").val(passwd);
           // $("#EPUserName").val(uname);
            $("#EPCfmPassword").val(passwd);
            $("#EPEmail").val(email);
            $("#EPAddress").val(address);
            $("#EPContPer").val(emerg_Contact_Name);
            $("#EPContNo").val(emerg_Contact_No);
            $("#EPDes").val(desg).change(); // Set designation and trigger change // Set department and trigger change
            $("#EPloc").val(plant_Id).change();
            if (chkresign === "Y") {
                $("#EPResignChk").prop("checked", true);
                $("#DateOfLeaveDIv").show();
                var formattedDateleave = date_Of_Resigning.split("T")[0];
                $("#EPDateOfLeave").val(formattedDateleave);
            } else {
                $("#EPResignChk").prop("checked", false);
            }
            if (headoforg === "Y") {
                $("#EPHeadOrg").prop("checked", true);
                $("#RoleReportDIv").hide();
            } else {
                $("#EPHeadOrg").prop("checked", false);
                $("#RoleReportDIv").show();
            }
            $("#UiAccessEEmplid").val(employee_ID);
            var UiAccessRMenu1 = $('#EPDept');
            UiAccessRMenu1.html('');
            const filteredData = Departments.filter(item => item.plantId === plant_Id);
            div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            UiAccessRMenu1.append(div_data);
            for (var i = 0; i < filteredData.length; i++) {
                div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
            }
            $("#EPDept").val(dept).change();
            var OrgEmpl = $('#EPRoleReportTo');
            OrgEmpl.html('');
            api.get("/Employee/GetAllEmployee").then((data) => {
                const filteredData = data.filter(item => item.headOfDepartment === "Y");
                div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
                OrgEmpl.append(div_data);
                OrgRoleReport.append(div_data);
                for (i = 0; i < filteredData.length; i++) {
                    div_data = "<option value='" + filteredData[i].employee_ID + "'>" + filteredData[i].employee_name + "</option>";
                    OrgEmpl.append(div_data);
                    OrgRoleReport.append(div_data);
                }
                $("#EPRoleReportTo").val(rolereport).change();
            }).catch((error) => {
            });
        } else {
            var OrgEmpl = $('#EPRoleReportTo');
            OrgEmpl.html('');
            api.get("/Employee/GetAllEmployee").then((data) => {
                const filteredData = data.filter(item => item.headOfDepartment === "Y");
                div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
                OrgEmpl.append(div_data);
                OrgRoleReport.append(div_data);
                for (i = 0; i < data.length; i++) {
                    div_data = "<option value='" + filteredData[i].employee_ID + "'>" + filteredData[i].employee_name + "</option>";
                    OrgEmpl.append(div_data);
                    OrgRoleReport.append(div_data);
                }
            }).catch((error) => {
            });
        }
        LoadEmplUiById();
        loadSelectMenusForEmpl();
        loadSelectEmplPermission();
    });

    $('#EPResignChk').on('click', function () {
        if ($(this).is(':checked')) {
            $("#DateOfLeaveDIv").show();
        } else {
            $("#DateOfLeaveDIv").hide();
        }
    });
    $('#EPDept').on('change', function () {
        var deptid = $("#EPDept").val();
        api.get("/Employee/GetAllOrgChart").then((data) => {
            const filteredData = data.filter(item => item.dept_ID === parseInt(deptid));
            $("#EPEmpRoleid").val(filteredData[0].roleIds);
            $("#EPEmpRoles").val(filteredData[0].roleName);
        });
    });
    $('#EPHeadOrg').on('click', function () {
        if ($(this).is(':checked')) {
            $("#RoleReportDIv").hide();
        } else {
            $("#RoleReportDIv").show();
        }
    });
    $('#EPEmail').on('keyup', function () {
        var email = $(this).val();
        var emailRegex = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        if (emailRegex.test(email)) {
            $("#email-error").text("").css("color", "red");
        } else {
            $("#email-error").text("Please enter a valid email address.").css("color", "red");
        }
    });
    $("#EmployeeSave").click(function () {
        //   //debugger;
        var EPEmpId = parseInt($("#EPEmpId").val());
        var EPEmpNo = $("#EPEmpNo").val();
        var EPEmpName = $("#EPEmpName").val();
        var EPPassword = $("#EPPassword").val();
       // var EPUserName = $("#EPUserName").val();
        var EPCfmPassword = $("#EPCfmPassword").val();
        var EPDateOfJoin = $("#EPDateOfJoin").val();
        var EPCell = $("#EPCell").val();
        var EPEmail = $("#EPEmail").val();
        var EPEmpRoleid = $("#EPEmpRoleid").val();
        var EPAddress = $("#EPAddress").val();
        var EPContPer = $("#EPContPer").val();
        var EPContNo = $("#EPContNo").val();
        var EPDateOfLeave = $("#EPDateOfLeave").val();
        var EPDes = parseInt($("#EPDes").val());
        var EPDept = parseInt($("#EPDept").val());
        var EPloc = parseInt($("#EPloc").val());
        var EPRoleReportTo = parseInt($("#EPRoleReportTo").val());
        var checkbox = document.getElementById("EPResignChk");
        var EPHeadOrg = document.getElementById("EPHeadOrg");
        var EPResignChk = 'N';
        var EPHeadOrgChk = 'N';
        if (EPEmpNo.length <= 0) {
            var newNamevalidate = document.getElementById('EPEmpNo');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPEmpNo');
            newNamevalidate.style.border = '';
        }
        if (EPDateOfJoin.length <= 0) {
            var newNamevalidate = document.getElementById('EPDateOfJoin');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPDateOfJoin');
            newNamevalidate.style.border = '';
        }
        if (EPEmpName.length <= 0) {
            var newNamevalidate = document.getElementById('EPEmpName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPEmpName');
            newNamevalidate.style.border = '';
        }
        if (EPDes === 0) {
            var newNamevalidate = document.getElementById('EPDes');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPDes');
            newNamevalidate.style.border = '';
        }
        if (EPloc === 0) {
            var newNamevalidate = document.getElementById('EPloc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPloc');
            newNamevalidate.style.border = '';
        }
        if (EPDept === 0) {
            var newNamevalidate = document.getElementById('EPDept');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPDept');
            newNamevalidate.style.border = '';
        }
        if (EPCell.length <= 0) {
            var newNamevalidate = document.getElementById('EPCell');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPCell');
            newNamevalidate.style.border = '';
        }
        if (EPEmail.length <= 0) {
            var newNamevalidate = document.getElementById('EPEmail');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPEmail');
            newNamevalidate.style.border = '';
        }
        //var regex = /^[a-zA-Z0-9]+$/;

        //if (!regex.test(EPUserName)) {
        //    $("#error-username").text("Username must contain only letters, digits and cannot contain spaces.").css("color", "red");
        //    return false;
        //} else {
        //    $("#error-username").text(" ").css("color", "red");
        //}
        var regexpassUpper = /[A-Z]/;
        var regexpassLower = /[a-z]/;
        if (!regexpassUpper.test(EPCfmPassword)) {
            $("#error-password").text("Password must contain at least one capital letter.").css("color", "red");
            return false;
        } else if (!regexpassLower.test(EPCfmPassword)) {
            $("#error-password").text("Password must contain at least one lowercase letter.").css("color", "red");
            return false;
        } else {
            $("#error-password").text(" ").css("color", "red");
        }
        if (EPCfmPassword != EPPassword) {
            var newNamevalidate = document.getElementById('EPCfmPassword');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPCfmPassword');
            newNamevalidate.style.border = '';
        }
        if (EPCfmPassword.length <= 6) {
            $("#error-password").text("Password must contain at least one capital letter.").css("color", "red");
            var newNamevalidate = document.getElementById('EPCfmPassword');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            $("#error-password").text(" ").css("color", "red");
            var newNamevalidate = document.getElementById('EPCfmPassword');
            newNamevalidate.style.border = '';
        }
        if (EPAddress.length <= 0) {
            var newNamevalidate = document.getElementById('EPAddress');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPAddress');
            newNamevalidate.style.border = '';
        }
        //if (EPContPer.length <= 0) {
        //    var newNamevalidate = document.getElementById('EPContPer');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('EPContPer');
        //    newNamevalidate.style.border = '';
        //}
        //if (EPContNo.length <= 0) {
        //    var newNamevalidate = document.getElementById('EPContNo');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('EPContNo');
        //    newNamevalidate.style.border = '';
        //}
        if (EPHeadOrg.checked) {
            EPHeadOrgChk = 'Y';
        } else {
            if (EPRoleReportTo == 0) {
                var newNamevalidate = document.getElementById('EPRoleReportTo');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('EPRoleReportTo');
                newNamevalidate.style.border = '';
            }
        }
        if (checkbox.checked) {
            EPResignChk = 'Y';
            if (EPDateOfLeave.length <= 0) {
                var newNamevalidate = document.getElementById('EPDateOfLeave');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var inputDate = new Date(document.getElementById('EPDateOfLeave').value);
                var today = new Date();

                today.setHours(0, 0, 0, 0);

                if (inputDate < today) {
                    var newNamevalidate = document.getElementById('EPDateOfLeave');
                    newNamevalidate.style.border = '2px solid red';
                    return false;
                } else {
                    var newNamevalidate = document.getElementById('EPDateOfLeave');
                    newNamevalidate.style.border = '';
                }
            }
        }
        var rowData = {
            employee_ID: EPEmpId,
            employee_name: EPEmpName,
            designation_Id: EPDes,
            employee_No: EPEmpNo,
            Date_Of_Joining: EPDateOfJoin,
            phone: EPCell,
            email: EPEmail,
            userName: EPEmail,
            roleIds: EPEmpRoleid,
            headOfDepartment: EPHeadOrgChk,
            roleReportTo: EPRoleReportTo,
            password: EPCfmPassword,
            residential_Address: EPAddress,
            emerg_Contact_Name: EPContPer,
            emerg_Contact_No: EPContNo,
            plant_Id: EPloc,
            home_Dept_Id: EPDept,
            employee_Resigned: EPResignChk,
            date_Of_Resigning: EPDateOfLeave
        };

        api.getbulk("/Employee/GetUnique?empNo="+EPEmpNo).then((data) => {
            //console.log(data);\
            if (data == true || EPEmpId > 0) {
                $("#empno-error").text("").css("color", "red");
                api.get("/Employee/GetAllEmployee").then((edata) => {
                    if (!edata.some(item => item.email === EPEmail)) {
                        api.post("/Employee/PostEmployee", rowData).then((data) => {
                            $("#EPEmpId").val(data.employee_ID);
                            $("#UiAccessEEmplid").val(data.employee_ID);
                            LoadEmployee();
                            LoadEmplUiById();
                            //data.employee_name = data.employee_name.replace(/\s+/g, '');
                            var userrowData = {
                                username: data.email,
                                email: data.email,
                                firstName: data.employee_name,
                                lastName: data.employee_name,
                                password: data.password,
                                phoneNumber: data.phone,
                                tenantId: "1"
                            };
                            const getIPAddress = () => {
                                const host = window.location.hostname; 
                                return host;
                            };

                            const ipAddress = getIPAddress();
                            console.log("Detected IP Address:", ipAddress);

                            api.post(`http://${ipAddress}:9003/account/Register`, userrowData, {
                                headers: {
                                    "Content-Type": "application/json",
                                    "Accept": "application/json",
                                },
                            })
                                .then((response) => {
                                    console.log("Success:", response);
                                })
                                .catch((error) => {
                                    console.error("Error:", error);
                                });

                            //api.post("http://172.23.0.1:9003/account/Register", userrowData, {
                            //    headers: {
                            //        "Content-Type": "application/json", // or application/x-www-form-urlencoded
                            //        "Accept": "application/json",
                            //    },
                            //})
                            //    .then((response) => {
                            //        console.log("Success:", response);
                            //    })
                            //    .catch((error) => {
                            //        console.error("Error:", error);
                            //    });

                        }).catch((error) => {
                            //AppUtil.HandleError("frmDesignation", error);
                        });
                    } else {
                        $("#email-error").text("This Email Id Already Exists.").css("color", "red");
                    }
                }).catch((error) => {

                });
            } else {
                $("#empno-error").text("This Employee No Already Exists.").css("color", "red");
            }
            //console.log(tablebody);
        }).catch((error) => { });
    });

    $('#orgChart').on('show.bs.modal', function (event) {
        LoadOrgChart();
        loadSelectRole();
    });

    $("#OrgRole").select2({
        dropdownParent: $("#addOrgChart")
    });

    $("#SearchOrgloc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchOrgDept").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchOrgRole").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchOrgEmp").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchOrgRRole").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchOrgLevel").on("change", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
        });
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#OrgChartGrid tbody tr").show();
        }
    });
    $("#SearchOrgREmp").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $('#addOrgChart').on('hidden.bs.modal', function (event) {
        $("#OrgLoc").val(0);
        $("#OrgDept").val(0);
        $("#OrgRole").val(0);
        $("#OrgEmpl").val(0);
        $("#POrgId").val('');
        $("#OrgRoleReport").val(0);
        var OrgLoc = document.getElementById('OrgLoc');
        OrgLoc.style.border = '';
        var OrgDept = document.getElementById('OrgDept');
        OrgDept.style.border = '';
        var OrgRole = document.getElementById('OrgRole');
        OrgRole.style.border = '';
        var OrgEmpl = document.getElementById('OrgEmpl');
        OrgEmpl.style.border = '';
        var newNamevalidate = document.getElementById('OrgRoleReport');
        newNamevalidate.style.border = '';
        var UiAccessRMenu1 = $('#OrgDept');
        UiAccessRMenu1.html('');
        LoadOrgChart();

    });
    $('#addOrgChart').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var orgid = relatedTarget.data("orgchartids");
        var toplevel = relatedTarget.data("toplevel");
        var roleid = relatedTarget.data("roleids");
        var dept = relatedTarget.data("dept");
        var reportto = relatedTarget.data("reportto");
        var employee = relatedTarget.data("employee");
        var plantid = relatedTarget.data("plantid");
        $("#OrgHead").prop("checked", false);
        //$("#OrgRole").prop("multiple", true);
        $("#DivRoleReportTo").show();
        if (orgid != undefined) {
            $("#OrgLoc").val(plantid).change(); // Set designation and trigger change
            //$("#OrgRole").prop("multiple", false);
            if (roleid.length > 1) {
                var roleidArray = roleid.split(",").map(value => value.trim());
                $("#OrgRole").val(roleidArray).change();
            } else {
                $("#OrgRole").val(roleid).change();
            }
            $("#OrgEmpl").val(employee).change();
            $("#OrgRoleReport").val(reportto).change();
            $("#POrgId").val(orgid);
            if (toplevel == "Y") {
                $("#OrgHead").prop("checked", true);
                $("#DivRoleReportTo").hide();
            } else {
                $("#OrgHead").prop("checked", false);
                $("#DivRoleReportTo").show();
            }
            var UiAccessRMenu1 = $('#OrgDept');
            UiAccessRMenu1.html('');
            const filteredData = Departments.filter(item => item.plantId === plantid);
            for (var i = 0; i < filteredData.length; i++) {
                div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
            }
            $("#OrgDept").val(dept).change(); // Set department and trigger change
        }
    });
    $('#OrgHead').on('click', function () {
        if ($(this).is(':checked')) {
            $("#DivRoleReportTo").hide();
        } else {
            $("#DivRoleReportTo").show();
        }
    });
    const getrolesLevel = {
        "Plant Head": 1,
        "Admin": 1,
        "Operations": 2,
        "Marketing": 2,
        "Finance Head": 2,
        "Opr Head": 2,
        "Prodn Design Engineer": 2,
        "Prodn Design Head": 2,
        "Product Design": 2,
        "Manf Engg": 2,
        "Quality": 2,
        "Quality Head": 2,
        "Sales Admin": 2,
        "Dispatch": 2,
        "Maintenance": 2,
        "HR": 2,
        "Finance": 2,
        "PPC": 3,
        "Maintenance": 3,
        "Dispatch": 3,
        "Tool Store": 3,
        "Shift QA": 3,
        "Shift Supervisor": 3,
        "Operator": 4,
        "PD Engineer": 3,
        "MFE Engineer": 3,
        "MFE Head": 3,
        "Line Quality": 3,
        "Inward Quality": 3,
        "Inward Qlty": 3,
        "Final Quality": 3,
        "Final Qlty": 3,
        "Purchase": 3,
        "Matl Stores": 3,
        "Stores": 3
    };
    $("#OrgSave").click(function () {
        //   //debugger;
        var POrgId = $("#POrgId").val();
        var OrgLoc = parseInt($("#OrgLoc").val());
        var OrgDept = parseInt($("#OrgDept").val());
        var OrgRole =  parseInt($("#OrgRole").val());
        var OrgEmpl =  parseInt($("#OrgEmpl").val());
        var OrgRoleReport =  parseInt($("#OrgRoleReport").val());
        var checkbox = document.getElementById("OrgHead");
        var EPResignChk = 'N';
        const selectElement = document.getElementById('OrgRole');
        const selectedValues = Array.from(selectElement.selectedOptions).map(option => Number(option.value));
        //var selectedText = $("#OrgRole option:selected").text();
        var roleidArray =[];
        if (POrgId.length > 1) {
            roleidArray = POrgId.split(",").map(value => value.trim());
        } else {
            roleidArray = POrgId;
        }
        if (checkbox.checked) {
            EPResignChk = 'Y';
        }
        if (OrgLoc=== 0) {
            var newNamevalidate = document.getElementById('OrgLoc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('OrgLoc');
            newNamevalidate.style.border = '';
        }
        if (OrgDept=== 0) {
            var newNamevalidate = document.getElementById('OrgDept');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('OrgDept');
            newNamevalidate.style.border = '';
        }
        if (OrgRole === 0 || isNaN(OrgRole)) {
            var newNamevalidate = $('#OrgRole').next('.select2-container');
            newNamevalidate.css('border', '2px solid red');
            return false;
        } else {
            var newNamevalidate = $('#OrgRole').next('.select2-container');
            newNamevalidate.css('border', '');
        }
        //if (OrgEmpl=== 0) {
        //    var newNamevalidate = document.getElementById('OrgEmpl');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('OrgEmpl');
        //    newNamevalidate.style.border = '';
        //}
        //if (EPResignChk === "N") {
        //    if (OrgRoleReport === 0) {
        //        var newNamevalidate = document.getElementById('OrgRoleReport');
        //        newNamevalidate.style.border = '2px solid red';
        //        return false;
        //    } else {
        //        var newNamevalidate = document.getElementById('OrgRoleReport');
        //        newNamevalidate.style.border = '';
        //    }
        //} else {
        //}
        api.get("/Employee/GetOrgChart").then((data) => {
            const filteredData = data.filter(item => item.first_node === "Y");
            if (!filteredData.some(item => item.first_node === EPResignChk)) {
                const selectElementText = document.getElementById('OrgRole');
                const selectedValuesText = Array.from(selectElementText.selectedOptions);
                for (var i = 0; i < selectedValues.length; i++) {
                    var selectedText = selectedValuesText[i].textContent;
                    var level = getrolesLevel[selectedText];
                    var orgids = roleidArray[i];
                    var rowData = {
                        org_ChartId: orgids,
                        first_node: EPResignChk,
                        role_NameId: selectedValues[i],
                        dept_ID: OrgDept,
                        location_id: OrgLoc,
                        reporting_to: OrgRoleReport,
                        employee_Id: OrgEmpl,
                        level_No: parseInt(level)
                    };
                    api.post("/Employee/PostOrgChart", rowData).then((data) => {
                        //LoadEmployee();
                        //$("#POrgId").val('');
                        //$("#OrgRoleReport").val(0);
                        //$("#OrgLoc").val(0);
                        //$("#OrgDept").val(0);
                        //$("#OrgRole").val(0);
                        //$("#OrgEmpl").val(0);
                        //$("#addOrgChart").modal("hide");
                        if (i === selectedValues.length - 1) {
                            // loadSelectRole();
                            $("#addOrgChart").modal("hide");
                        }
                    }).catch((error) => {
                    });
                }
                if (selectedValues.length < roleidArray.length) {
                    var j = roleidArray.length - 1;
                    var opid = roleidArray[j];
                    api.get("/Employee/DelOrgChart?designationId=" + parseInt(opid)).then((data) => {
                        //LoadOrgChart();
                    }).catch((error) => {

                    });
                }
            } else {
                alert("The Head Of Organization Is Already There In The List.");
            }
        }).catch((error) => {

        });

    });

    $('#uiList').on('show.bs.modal', function (event) {
        loadUiList();
        loadSelectMenus();
    });
    $('#UPTopLevel').on('click', function () {
        if ($(this).is(':checked')) {
            $("#DivUiPartOf").hide();
        } else {
            $("#DivUiPartOf").show();
        }
    });
    $('#addUi').on('hidden.bs.modal', function (event) {
        document.getElementById('uiList').style.filter = 'none';
        $("#DivUiPartOf").show();
        var newNamevalidate = document.getElementById('UPUiName');
        newNamevalidate.style.border = '';
        var UPPartOf = document.getElementById('UPPartOf');
        UPPartOf.style.border = '';
        $("#UPUiName").val('');
        $("#UPUiListId").val('');
        $("#UPTopLevel").prop("checked", false);
        $("#UPView").prop("checked", false);
        $("#UPAddEdit").prop("checked", false);
        $("#UPDelete").prop("checked", false);
        $("#UPApprove").prop("checked", false);
        $("#UPUiType").val("Landing Page").change();
    });
    $('#addUi').on('show.bs.modal', function (event) {
        document.getElementById('uiList').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var uilistid = relatedTarget.data("uilistid");
        var toplevel = relatedTarget.data("toplevel");
        var uitype = relatedTarget.data("uitype");
        var uiname = relatedTarget.data("uiname");
        var uipartto = relatedTarget.data("uipartto");
        var approveallow = relatedTarget.data("approveallow");
        var viewallow = relatedTarget.data("viewallow");
        var addedit = relatedTarget.data("addedit");
        var deleteallow = relatedTarget.data("deleteallow");
        if (uilistid > 0) {
            if (toplevel == "Y") {
                $("#UPTopLevel").prop("checked", true);
                $("#DivUiPartOf").hide();
            } else {
                $("#UPTopLevel").prop("checked", false);
                $("#UPPartOf").val(uipartto).change();
                $("#DivUiPartOf").show();
            }
            if (viewallow == "Y") {
                $("#UPView").prop("checked", true);
            } else {
                $("#UPView").prop("checked", false);
            }
            if (addedit == "Y") {
                $("#UPAddEdit").prop("checked", true);
            } else {
                $("#UPAddEdit").prop("checked", false);
            }
            if (deleteallow == "Y") {
                $("#UPDelete").prop("checked", true);
            } else {
                $("#UPDelete").prop("checked", false);
            }
            if (approveallow == "Y") {
                $("#UPApprove").prop("checked", true);
            } else {
                $("#UPApprove").prop("checked", false);
            }
            $("#UPUiType").val(uitype).change(); 
            $("#UPUiName").val(uiname);
            $("#UPUiListId").val(uilistid);
        }
    });
    $("#UPSaveUi").click(function () {
        var UPUiName = $("#UPUiName").val();
        var UPUiType = $("#UPUiType").val();
        var UPUiListId = $("#UPUiListId").val();
        var UPPartOf = parseInt($("#UPPartOf").val());
        var OrgRoleReport = parseInt($("#OrgRoleReport").val());
        var checkbox = document.getElementById("UPTopLevel");
        if (UPUiName.length <= 0) {
            var newNamevalidate = document.getElementById('UPUiName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('UPUiName');
            newNamevalidate.style.border = '';
        }
        var EPResignChk = 'N';
        if (checkbox.checked) {
            EPResignChk = 'Y';
        } else {
            if (UPPartOf == 0) {
                var newNamevalidate = document.getElementById('UPPartOf');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('UPPartOf');
                newNamevalidate.style.border = '';
            }}
        var UPView = document.getElementById("UPView");
        var UPViewChk = 'N';
        if (UPView.checked) {
            UPViewChk = 'Y';
        }
        var UPAddEdit = document.getElementById("UPAddEdit");
        var UPAddEditChk = 'N';
        if (UPAddEdit.checked) {
            UPAddEditChk = 'Y';
        }
        var UPDelete = document.getElementById("UPDelete");
        var UPDeleteChk = 'N';
        if (UPDelete.checked) {
            UPDeleteChk = 'Y';
        }
        var UPApprove = document.getElementById("UPApprove");
        var UPApproveChk = 'N';
        if (UPApprove.checked) {
            UPApproveChk = 'Y';
        }
        var rowData = {
            uiListId: UPUiListId,
            TopLevelId: EPResignChk,
            ui_Type: UPUiType,
            ui_Name_Label: UPUiName,
            ui_Part_linked_to: UPPartOf,
            approval_Allowed: UPApproveChk,
            view_Allowed: UPViewChk,
            add_Edit_Allowed: UPAddEditChk,
            delete_Allowed: UPDeleteChk
        };

        //api.getbulk("/Employee/GetUniqueUiName?uiName=" + UPUiName).then((data) => {
        //    if (data == true || UPUiListId >0) {
        //        $("#error-uiname").text("").css("color", "red");
                api.post("/Employee/PostUilist", rowData).then((data) => {
                    loadUiList();
                }).catch((error) => {
                    //AppUtil.HandleError("frmDesignation", error);
                });
        //    } else {
        //        $("#error-uiname").text("Please enter a different Ui Name.").css("color", "red");
        //    }
        //    //console.log(tablebody);
        //}).catch((error) => { });
    });

    $('#roleList').on('show.bs.modal', function (event) {
        LoadRoleUiAll();
    });
    $("#SearchRlRoleName").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#RoleGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchRlUiAccess").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#RoleGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#SearchUim1").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var UiAccessRMenu1 = $('#SearchUim2');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu2 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim2 = $('#SearchUim2');
            SearchUim2.html('');
            var SearchUim3 = $('#SearchUim3');
            SearchUim3.html('');
            var SearchUim4 = $('#SearchUim4');
            SearchUim4.html('');
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');

        }
    });
    $("#SearchUim2").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var UiAccessRMenu1 = $('#SearchUim3');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu3 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim3 = $('#SearchUim3');
            SearchUim3.html('');
            var SearchUim4 = $('#SearchUim4');
            SearchUim4.html('');
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');
        }
    });
    $("#SearchUim3").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var UiAccessRMenu1 = $('#SearchUim4');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu4 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim4 = $('#SearchUim4');
            SearchUim4.html('');
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');
        }
    });
    $("#SearchUim4").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var UiAccessRMenu1 = $('#SearchUim5');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu5 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');
        }
    });
    $("#SearchUim5").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
        }
    });
    $("#SearchUiType").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $('#addRole').on('hidden.bs.modal', function (event) {
        document.getElementById('roleList').style.filter = 'none';
        var newNamevalidate = document.getElementById('ARPWork');
        newNamevalidate.style.border = '';
        var ARPName = document.getElementById('ARPName');
        ARPName.style.border = '';
    });
    $('#addRole').on('show.bs.modal', function (event) {
        document.getElementById('roleList').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var roleid = relatedTarget.data("roleid");
        var rolename = relatedTarget.data("rolename");
        var uitype = relatedTarget.data("workdone");
        $("#ARPId").val(roleid);
        $("#ARPName").val(rolename);
        $("#ARPWork").val(uitype);
        loadSelectMenus();
        LoadRoleUiById(roleid);
    });
    $('#addUiAccessRole').on('hidden.bs.modal', function (event) {
        document.getElementById('addRole').style.filter = 'none';
        var UiAccessRMenu1 = document.getElementById('UiAccessRMenu1');
        UiAccessRMenu1.style.border = '';
        var UiAccessRMenu2 = document.getElementById('UiAccessRMenu2');
        UiAccessRMenu2.style.border = '';
        var UiAccessRMenu3 = document.getElementById('UiAccessRMenu3');
        UiAccessRMenu3.style.border = '';
        var UiAccessRMenu4 = document.getElementById('UiAccessRMenu4');
        UiAccessRMenu4.style.border = '';
        var UiAccessRMenu5 = document.getElementById('UiAccessRMenu5');
        UiAccessRMenu5.style.border = '';
        var UiAccessRPermission = document.getElementById('UiAccessRPermission');
        UiAccessRPermission.style.border = '';
        $("#UiAccessRMenu1").val(0);
        $("#UiAccessRMenu2").val(0);
        $("#UiAccessRMenu3").val(0);
        $("#UiAccessRMenu4").val(0);
        $("#UiAccessRMenu5").val(0);
        $("#UiAccessREmplid").val(0);
        $("#UiAccessRPermission").val(0);
    });
    $('#addUiAccessRole').on('show.bs.modal', function (event) {
        document.getElementById('addRole').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var uilistid = relatedTarget.data("uilistid");
        var roleid = relatedTarget.data("roleid");
        var uid = relatedTarget.data("uid");
        var permissionid = relatedTarget.data("permissionid");
        var uilevel = relatedTarget.data("uilevel");
        var menuo = relatedTarget.data("menuo");
        var menut = relatedTarget.data("menut");
        var menuth = relatedTarget.data("menuth");
        var menuf = relatedTarget.data("menuf");
        var menufi = relatedTarget.data("menufi");
        var roleidnew = $("#ARPId").val();
        $("#UiAccessRRoleid").val(roleidnew);
        if (uilistid > 0) {
            //const menuSelections = {
            //    1: { menu1: 1, menu2: 0, menu3: 0, menu4: 0, menu5: 0, permission: 0 },
            //    2: { menu1: 1, menu2: 2, menu3: 0, menu4: 0, menu5: 0, permission: 0 },
            //    3: { menu1: 1, menu2: 2, menu3: 3, menu4: 0, menu5: 0, permission: 0 },
            //    4: { menu1: 1, menu2: 2, menu3: 3, menu4: 4, menu5: 0, permission: 0 },
            //    5: { menu1: 1, menu2: 2, menu3: 3, menu4: 4, menu5: 6, permission: 0 },
            //    6: { menu1: 1, menu2: 2, menu3: 3, menu4: 4, menu5: 6, permission: 0 },
            //    // Add more mappings as needed
            //};

            //const selection = menuSelections[uid];
            $('#UiAccessRMenu1 option').each(function () {
                if ($(this).text() === menuo) {
                    $('#UiAccessRMenu1').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu2 option').each(function () {
                if ($(this).text() === menut) {
                    $('#UiAccessRMenu2').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu3 option').each(function () {
                if ($(this).text() === menuth) {
                    $('#UiAccessRMenu3').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu4 option').each(function () {
                if ($(this).text() === menuf) {
                    $('#UiAccessRMenu4').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu5 option').each(function () {
                if ($(this).text() === menufi) {
                    $('#UiAccessRMenu5').val($(this).val()).change();
                }
            });
            $("#UiAccessRPermission").val(permissionid).change();
            $("#UiAccessRRoleid").val(roleid);
            $("#UiAccessRUiId").val(uilistid);
        }
    });
    $("#UiAccessRSave").click(function () {
        var UiAccessRMenu1 = parseInt($("#UiAccessRMenu1").val());
        var UiAccessRMenu2 = parseInt($("#UiAccessRMenu2").val());
        var UiAccessRMenu3 = parseInt($("#UiAccessRMenu3").val());
        var UiAccessRMenu4 = parseInt($("#UiAccessRMenu4").val());
        var UiAccessRMenu5 = parseInt($("#UiAccessRMenu5").val());
        var UiAccessRRoleid = parseInt($("#UiAccessRRoleid").val());
        var UiAccessRUiId = parseInt($("#UiAccessRUiId").val());
        var UiAccessRPermission = parseInt($("#UiAccessRPermission").val());
        var ARPName = $("#ARPName").val();
        var ARPId = $("#ARPId").val();
        if (UiAccessRRoleid == 0 || isNaN(UiAccessRRoleid)) {
            alert("Please Save the Role Name");
            return false;
        }
        let uiId = 0;
        if (UiAccessRMenu5 != 0) {
            uiId = UiAccessRMenu5;
        } else if (UiAccessRMenu4 != 0) {
            uiId = UiAccessRMenu4;
        } else if (UiAccessRMenu3 != 0) {
            uiId = UiAccessRMenu3;
        } else if (UiAccessRMenu2 != 0) {
            uiId = UiAccessRMenu2;
        } else {
            uiId = UiAccessRMenu1;  // Default case when all above are zero
        }

        if (UiAccessRMenu1 == 0) {
            var newvalidate = document.getElementById('UiAccessRMenu1');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessRMenu1');
            newvalidate.style.border = '';
        }
        //if (UiAccessRMenu2 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu2');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu2');
        //    newvalidate.style.border = '';
        //}
        //if (UiAccessRMenu3 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu3');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu3');
        //    newvalidate.style.border = '';
        //}
        //if (UiAccessRMenu4 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu4');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu4');
        //    newvalidate.style.border = '';
        //}
        //if (UiAccessRMenu5 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu5');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu5');
        //    newvalidate.style.border = '';
        //}
        if (UiAccessRPermission == 0) {
            var newvalidate = document.getElementById('UiAccessRPermission');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessRPermission');
            newvalidate.style.border = '';
        }
        var rowData = {
            role_Ui_ListId: UiAccessRUiId,
            ui_Id: uiId,
            permissionId: UiAccessRPermission,
            roleId: UiAccessRRoleid
        };

        //api.getbulk("/Employee/GetUniqueRole?roleName=" + ARPName).then((data) => {
        //    if (data == true) {
        //        $("#error-rolename").text("").css("color", "red");
        api.post("/Employee/PostRoleUiList", rowData).then((data) => {
            LoadRoleUiById(UiAccessRRoleid);
            LoadRoleUiAll();
            $("#addUiAccessRole").modal("hide");
                    //LoadEmployee();
                    //loadSelectRole();
                }).catch((error) => {
                    //AppUtil.HandleError("frmDesignation", error);
                });
        //    } else {
        //        $("#error-rolename").text("Please enter a different Empl No.").css("color", "red");
        //    }
        //    //console.log(tablebody);
        //}).catch((error) => { });
    });

    $("#ARPSave").click(function () {
        var ARPWork = $("#ARPWork").val();
        var ARPName = $("#ARPName").val();
        var ARPId = $("#ARPId").val();
        if (ARPName.length <= 0) {
            var newNamevalidate = document.getElementById('ARPName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('ARPName');
            newNamevalidate.style.border = '';
        }
        if (ARPWork.length <= 0) {
            var newNamevalidate = document.getElementById('ARPWork');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('ARPWork');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            role_Desc: ARPName,
            work_Done: ARPWork,
            role_ListId: ARPId
        };

        api.getbulk("/Employee/GetUniqueRole?roleName=" + ARPName).then((data) => {
            if (data == true || ARPId > 0) {
                $("#error-rolename").text("").css("color", "red");
                api.post("/Employee/PostRolelist", rowData).then((data) => {
                    //LoadEmployee();
                    $("#ARPId").val(data.role_ListId);
                    $("#UiAccessRRoleid").val(data.role_ListId);
                    loadSelectRole();
                    LoadRoleUiAll();
                    LoadRoleUiById(data.role_ListId);
                }).catch((error) => {
                    //AppUtil.HandleError("frmDesignation", error);
                });
            } else {
                $("#error-rolename").text("This Role Name Already Exists.").css("color", "red");
            }
            //console.log(tablebody);
        }).catch((error) => { });
    });


    $('#addUiAccessEmpl').on('hidden.bs.modal', function (event) {
        document.getElementById('addEmployee').style.filter = 'none';
        var UiAccessRMenu1 = document.getElementById('UiAccessEMenu1');
        UiAccessRMenu1.style.border = '';
        var UiAccessRMenu2 = document.getElementById('UiAccessEMenu2');
        UiAccessRMenu2.style.border = '';
        var UiAccessRMenu3 = document.getElementById('UiAccessEMenu3');
        UiAccessRMenu3.style.border = '';
        var UiAccessRMenu4 = document.getElementById('UiAccessEMenu4');
        UiAccessRMenu4.style.border = '';
        var UiAccessRMenu5 = document.getElementById('UiAccessEMenu5');
        UiAccessRMenu5.style.border = '';
        var UiAccessRPermission = document.getElementById('UiAccessEPermission');
        UiAccessRPermission.style.border = '';
        $("#UiAccessEEmplid").val(0)
        $("#UiAccessEUiId").val(''); 
        $("#UiAccessEMenu1").val(0);
        $("#UiAccessEMenu2").val(0);
        $("#UiAccessEMenu3").val(0);
        $("#UiAccessEMenu4").val(0);
        $("#UiAccessEMenu5").val(0);
        $("#UiAccessEEmplid").val(0);
        $("#UiAccessEPermission").val(0);
    });
    $('#addUiAccessEmpl').on('show.bs.modal', function (event) {
        document.getElementById('addEmployee').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var uilistid = relatedTarget.data("uilistid");
        var roleid = relatedTarget.data("roleid");
        var uid = relatedTarget.data("uid");
        var permissionid = relatedTarget.data("permissionid");
        var uilevel = relatedTarget.data("uilevel");
        var menuo = relatedTarget.data("menuo");
        var menut = relatedTarget.data("menut");
        var menuth = relatedTarget.data("menuth");
        var menuf = relatedTarget.data("menuf");
        var menufi = relatedTarget.data("menufi");
        var empid = $("#EPEmpId").val();
        $("#UiAccessEEmplid").val(empid);
        if (uilistid > 0) {
            $('#UiAccessEMenu1 option').each(function () {
                if ($(this).text() === menuo) {
                    $('#UiAccessEMenu1').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu2 option').each(function () {
                if ($(this).text() === menut) {
                    $('#UiAccessEMenu2').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu3 option').each(function () {
                if ($(this).text() === menuth) {
                    $('#UiAccessEMenu3').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu4 option').each(function () {
                if ($(this).text() === menuf) {
                    $('#UiAccessEMenu4').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu5 option').each(function () {
                if ($(this).text() === menufi) {
                    $('#UiAccessEMenu5').val($(this).val()).change();
                }
            });
            $("#UiAccessEPermission").val(permissionid).change();
            $("#UiAccessERoleId").val(roleid);
            $("#UiAccessEUiId").val(uilistid);
        }
    });
    $("#UiAccessESave").click(function () {
        var UiAccessRMenu1 = parseInt($("#UiAccessEMenu1").val());
        var UiAccessRMenu2 = parseInt($("#UiAccessEMenu2").val());
        var UiAccessRMenu3 = parseInt($("#UiAccessEMenu3").val());
        var UiAccessRMenu4 = parseInt($("#UiAccessEMenu4").val());
        var UiAccessRMenu5 = parseInt($("#UiAccessEMenu5").val());
        var UiAccessRRoleid =$("#UiAccessEEmplid").val();
        var UiAccessRUiId = parseInt($("#UiAccessEUiId").val());
        var UiAccessERoleId = parseInt($("#UiAccessERoleId").val());
        var UiAccessRPermission = parseInt($("#UiAccessEPermission").val());
        var deptid = $("#EPDept").val();
        //var ARPId = $("#ARPId").val();
        if (UiAccessRRoleid == 0) {
            alert("Please Save the Employee Details");
            return false;
        }
        let uiId = 0;
        if (UiAccessRMenu5 != 0) {
            uiId = UiAccessRMenu5;
        } else if (UiAccessRMenu4 != 0) {
            uiId = UiAccessRMenu4;
        } else if (UiAccessRMenu3 != 0) {
            uiId = UiAccessRMenu3;
        } else if (UiAccessRMenu2 != 0) {
            uiId = UiAccessRMenu2;
        } else {
            uiId = UiAccessRMenu1; 
        }
        if (UiAccessRMenu1 == 0) {
            var newvalidate = document.getElementById('UiAccessEMenu1');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessEMenu1');
            newvalidate.style.border = '';
        }
        if (UiAccessRPermission == 0) {
            var newvalidate = document.getElementById('UiAccessEPermission');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessEPermission');
            newvalidate.style.border = '';
        }
        var rowData = {
            role_Ui_ListId: UiAccessRUiId,
            ui_Id: uiId,
            permissionId: UiAccessRPermission,
            employeeId: UiAccessRRoleid,
            departmentId: deptid,
            roleId: UiAccessERoleId
        };
        api.getbulk("/Employee/GetAllOrgChart").then((data) => {
            var deptid = $("#EPDept").val();
            const filteredData = data.filter(item => item.dept_ID === parseInt(deptid));
            if (filteredData.length > 0) {
                api.post("/Employee/PostRoleUiList", rowData).then((data) => {
                    LoadEmplUiById();
                }).catch((error) => {
                    //AppUtil.HandleError("frmDesignation", error);
                });
            } else {
                alert("Please Assign The Role For Department.");
            }
        });
    });

    //loadSelectRole();
    loadSelectPermission();


    $("#UiAccessRMenu1").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu2');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu2 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessRMenu2").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu3');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu3 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessRMenu3").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu4');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu4 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessRMenu4").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu5');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu5 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu1").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu2');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu2 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu2").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu3');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu3 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu3").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu4');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu4 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu4").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu5');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu5 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#EPloc").on("change", function () {
        var UiAccessRMenu1 = $('#EPDept');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);
        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        const filteredData = Departments.filter(item => item.plantId === formattedSelectedDate);
        for (var i = 0; i < filteredData.length; i++) {
            div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
        }
    });
    $("#OrgLoc").on("change", function () {
        var UiAccessRMenu1 = $('#OrgDept');
        UiAccessRMenu1.html('');
        //div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        //UiAccessRMenu1.append(div_data);
        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        const filteredData = Departments.filter(item => item.plantId === formattedSelectedDate);
        for (var i = 0; i < filteredData.length; i++) {
            div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
        }
    });
});

function LoadEmployee() {
    var tablebody = $("#EmployeeGrid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/Employee/GetAllEmployee").then((data) => {
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmployeeGridRow", data[i], i));
        }
        //console.log(tablebody);
    }).catch((error) => { });
    loadSelectEmployee();
}
function LoadOrgChart() {
    var tablebody = $("#OrgChartGrid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/Employee/GetAllOrgChart").then((data) => {
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("OrgChartGridRow", data[i], i));
        }
        //console.log(tablebody);
    }).catch((error) => { });
    loadSelectEmployee();
}
function LoadRoleUiById(roleid) {
    var tablebody = $("#RPGrid tbody");
    $(tablebody).html("");//empty tbody
    var ARPId;
    if (roleid == 0) {
        ARPId = $("#ARPId").val();
    } else {
        ARPId = roleid;
    }
    api.getbulk("/Employee/GetRoleUiList?roleId=" + parseInt(ARPId)).then((data) => {
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("RPGridRow", data[i], i));
        }
        //console.log(tablebody);
    }).catch((error) => { });
    //loadSelectEmployee();
}
function LoadRoleUiAll() {
    var tablebody = $("#RoleGrid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/Employee/GetAllRoleUiList").then((data) => {
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("RoleGridRow", data[i], i));
        }
        //console.log(tablebody);
    }).catch((error) => { });
    //loadSelectEmployee();
}
function loadUiList() {
    var tablebody = $("#UiGrid tbody");
    $(tablebody).html("");//empty tbody
    var selElem = $('#UPPartOf');
    selElem.html('');
    div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
    selElem.append(div_data);
    api.getbulk("/Employee/GetAllUilist").then((data) => {
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("UiGridRow", data[i], i));
            div_data = "<option value='" + data[i].uiListId + "'>" + data[i].uI_Name_Label + "</option>";
            selElem.append(div_data);
        }
        //console.log(tablebody);
    }).catch((error) => { });
}
function loadDesignation() {
    var selElem = $('#EPDes');
    selElem.html('');
    api.getbulk("/designation/designations").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].designationId + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadDepartment(locationid) {
    var selElem = $('#EPDept');
    var selElemOrgDept = $('#OrgDept');
    selElem.html('');
    selElemOrgDept.html('');
    api.getbulk("/Department/GetDepartments").then((data) => {
        Departments = data;
        const filteredData = data.filter(item => item.plantId === locationid);
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        selElemOrgDept.append(div_data);
        for (i = 0; i < filteredData.length; i++) {
            div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
            selElemOrgDept.append(div_data);
            selElem.append(div_data);
        }
    });
}
function loadSelectEmployee() {
    //var OrgEmpl = $('#EPRoleReportTo');
    var OrgRoleReport = $('#OrgRoleReport');
    //OrgEmpl.html('');
    OrgRoleReport.html('');

    api.get("/Employee/GetAllEmployee").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        //OrgEmpl.append(div_data);
        OrgRoleReport.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].employee_ID + "'>" + data[i].employee_name + "</option>";
            //OrgEmpl.append(div_data);
            OrgRoleReport.append(div_data);
        }
    }).catch((error) => {
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
function loadLoaction() {
    var selElem = $('#EPloc');
    var selElemOrgLoc = $('#OrgLoc');
    selElem.html('');
    selElemOrgLoc.html('');

    api.get("/plant/getplants").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        selElemOrgLoc.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].plantId + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
            selElemOrgLoc.append(div_data);
        }
    }).catch((error) => {
    });
}
function loadSelectMenus() {
    var UiAccessRMenu1 = $('#UiAccessRMenu1');
    UiAccessRMenu1.html('');
    var UiAccessRMenu2 = $('#UiAccessRMenu2');
    UiAccessRMenu2.html('');
    var UiAccessRMenu3 = $('#UiAccessRMenu3');
    UiAccessRMenu3.html('');
    var UiAccessRMenu4 = $('#UiAccessRMenu4');
    UiAccessRMenu4.html('');
    var UiAccessRMenu5 = $('#UiAccessRMenu5');
    UiAccessRMenu5.html('');
    var SearchUim1 = $('#SearchUim1');
    SearchUim1.html('');

    api.get("/Employee/GetAllUilist").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);
        UiAccessRMenu2.append(div_data);
        UiAccessRMenu3.append(div_data);
        UiAccessRMenu4.append(div_data);
        UiAccessRMenu5.append(div_data);
        SearchUim1.append(div_data);
        for (i = 0; i < data.length; i++) {
            menusdata = data;
            if (data[i].menuLevelId == 1) {
                div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu1 + "</option>";
                UiAccessRMenu1.append(div_data);
                SearchUim1.append(div_data);
            }
            //else if (data[i].menuLevelId == 2) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu2 + "</option>";
            //    UiAccessRMenu2.append(div_data);
            //}else if (data[i].menuLevelId == 3) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu3 + "</option>";
            //    UiAccessRMenu3.append(div_data);
            //}else if (data[i].menuLevelId == 4) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu4 + "</option>";
            //    UiAccessRMenu4.append(div_data);
            //}else if (data[i].menuLevelId == 5) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu5 + "</option>";
            //    UiAccessRMenu5.append(div_data);
            //}
        }
    }).catch((error) => {
    });
}
function loadSelectPermission() {
    var UiAccessRPermission = $('#UiAccessRPermission');
    UiAccessRPermission.html('');

    api.get("/Employee/GetAllPermission").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRPermission.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].permissionId + "'>" + data[i].permission + "</option>";
            UiAccessRPermission.append(div_data);
        }
    }).catch((error) => {
    });
}

function DeleteOrgChart(element) {
    //var relatedTarget = $(element.relatedTarget);
    var orgid = $(element).data("orgid");
    var dept = $(element).data("dept");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.getbulk("/Employee/GetAllEmployee").then((data) => {
            const filteredData = data.filter(item => item.home_Dept_Id === dept);
            if (filteredData.length == 0) {
                api.get("/Employee/DelOrgChart?designationId=" + parseInt(orgid)).then((data) => {
                    LoadOrgChart();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete The Employe With This Department.");
            }
        }).catch((error) => {

        });
    }
}
function DeleteEmployee(element) {
    //var relatedTarget = $(element.relatedTarget);
    var employeeid = $(element).data("employeeid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/GetAllOrgChart").then((data) => {
            if (!data.some(item => item.employee_Id === parseInt(employeeid))) {
                api.get("/Employee/DelEmployee?designationId=" + parseInt(employeeid)).then((data) => {
                    LoadEmployee();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete Org Chart Of This Role.");
            }
        }).catch((error) => {

        });
    }
}
function DeleteUiList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/CheckUiList?designationId=" + parseInt(uilistid)).then((data) => {
            if (data == true) {
                api.get("/Employee/DelUiList?designationId=" + parseInt(uilistid)).then((data) => {
                    loadUiList();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete Role Ui Access Of This.");
            }
        }).catch((error) => {

        });
    }
}
function DeleteRoleList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/GetAllOrgChart").then((data) => {
            if (!data.some(item => item.role_NameId === parseInt(uilistid))) {
                api.get("/Employee/DelRoleList?designationId=" + parseInt(uilistid)).then((data) => {
                    LoadRoleUiAll();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete Org Chart Of This Role.");
            }
        }).catch((error) => {

        });

    }
}
function DeleteRoleUiList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/DelRoleUiList?designationId=" + parseInt(uilistid)).then((data) => {
            LoadRoleUiAll();
            LoadRoleUiById();
        }).catch((error) => {

        });
    }
}

function DeleteEmplUiList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/DelRoleUiList?designationId=" + parseInt(uilistid)).then((data) => {
            LoadEmplUiById();
        }).catch((error) => {

        });
    }
}
function LoadEmplUiById() {
    var tablebody = $("#EmpUiGrid tbody");
    $(tablebody).html("");//empty tbody
    var ARPId = $("#EPDept").val();
    //if (isNaN(ARPId) || ARPId == 0) {
    //    ARPId = $("#EPEmpId").val();
    //} 
    api.getbulk("/Employee/GetEmplRoleUiList?employeeId=" + parseInt(ARPId)).then((data) => {
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpUiGridRow", data[i], i));
        }
        //console.log(tablebody);
    }).catch((error) => { });
    //loadSelectEmployee();
}
function loadSelectMenusForEmpl() {
    var UiAccessRMenu1 = $('#UiAccessEMenu1');
    UiAccessRMenu1.html('');
    var UiAccessRMenu2 = $('#UiAccessEMenu2');
    UiAccessRMenu2.html('');
    var UiAccessRMenu3 = $('#UiAccessEMenu3');
    UiAccessRMenu3.html('');
    var UiAccessRMenu4 = $('#UiAccessEMenu4');
    UiAccessRMenu4.html('');
    var UiAccessRMenu5 = $('#UiAccessEMenu5');
    UiAccessRMenu5.html('');

    api.get("/Employee/GetAllUilist").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);
        UiAccessRMenu2.append(div_data);
        UiAccessRMenu3.append(div_data);
        UiAccessRMenu4.append(div_data);
        UiAccessRMenu5.append(div_data);
        menusdataEmpl = data;
        for (i = 0; i < data.length; i++) {
            if (data[i].menuLevelId == 1) {
                div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu1 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
            //else if (data[i].menuLevelId == 2) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu2 + "</option>";
            //    UiAccessRMenu2.append(div_data);
            //} else if (data[i].menuLevelId == 3) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu3 + "</option>";
            //    UiAccessRMenu3.append(div_data);
            //} else if (data[i].menuLevelId == 4) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu4 + "</option>";
            //    UiAccessRMenu4.append(div_data);
            //} else if (data[i].menuLevelId == 5) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu5 + "</option>";
            //    UiAccessRMenu5.append(div_data);
            //}
        }
    }).catch((error) => {
    });
}
function loadSelectEmplPermission() {
    var UiAccessRPermission = $('#UiAccessEPermission');
    UiAccessRPermission.html('');

    api.get("/Employee/GetAllPermission").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRPermission.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].permissionId + "'>" + data[i].permission + "</option>";
            UiAccessRPermission.append(div_data);
        }
    }).catch((error) => {
    });
}