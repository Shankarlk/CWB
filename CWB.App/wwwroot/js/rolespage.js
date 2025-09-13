

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

$(function () {
    LoadRoleUiAll();
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

});