let allmaxShifts = 0;


function loadDocTypes() {
    var selElem = $('#inwardDocDocTypeName');
    var inspectDocDocTypeName = $('#inspectDocDocTypeName');
    var lineDocTypeName = $('#lineDocTypeName');
    var finalDocTypeName = $('#finalDocTypeName');
    var rcacaDocTypeName = $('#rcacaDocTypeName');
    selElem.html('');
    inspectDocDocTypeName.html('');
    finalDocTypeName.html('');
    lineDocTypeName.html('');
    rcacaDocTypeName.html('');
    api.getbulk("/masters/DocTypes").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        inspectDocDocTypeName.append(div_data);
        lineDocTypeName.append(div_data);
        finalDocTypeName.append(div_data);
        rcacaDocTypeName.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].documentTypeId + "'>" + data[i].documentName + "</option>";
            selElem.append(div_data);
            lineDocTypeName.append(div_data);
            finalDocTypeName.append(div_data);
            inspectDocDocTypeName.append(div_data);
            rcacaDocTypeName.append(div_data);
        }
    });
}
function loadSelectNCDispDecision() {
    var selElem = $('#NcPopup2Decision');
    var P5NcDispDec = $('#P5NcDispDec');
    var btnElem = $('#NcPopup2OpenBtn');
    var btnElemP5NcDispDec = $('#P4AddNewDesc');
    P5NcDispDec.html('');
    selElem.html('');
    var existingNames = new Set($("#NcPopup1Grid tbody tr").map((_, row) =>
        $(row).find("td.nc-description").text().trim() // Adjust selector based on actual column
    ).get());
    var P5NcDispDecexistingNames = new Set($("#NcPopup4Grid tbody tr").map((_, row) =>
        $(row).find("td.nc-description").text().trim() // Adjust selector based on actual column
    ).get());
    api.getbulk("/WorkOrder/GetAllNC_Disp_Decision_List").then((data) => {
        let hasOptions = false;
        let P5NcDispDechasOptions = false;

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        P5NcDispDec.append(div_data);
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            let optionName = data[i].nC_Disp_Decision_Desc.trim(); // Ensure consistency

            if (!existingNames.has(optionName)) { // Only add if NOT in table
                div_data = `<option value='${data[i].nC_Disp_Decision_ListId}'>${optionName}</option>`;
                selElem.append(div_data);
                hasOptions = true;
            }
            if (!P5NcDispDecexistingNames.has(optionName)) { // Only add if NOT in table
                div_data = `<option value='${data[i].nC_Disp_Decision_ListId}'>${optionName}</option>`;
                P5NcDispDec.append(div_data);
                P5NcDispDechasOptions = true;
            }
            if (!hasOptions) {
                btnElem.prop("disabled", true);
            } else {
                btnElem.prop("disabled", false);
            }
            if (!P5NcDispDechasOptions) {
                btnElemP5NcDispDec.prop("disabled", true);
            } else {
                btnElemP5NcDispDec.prop("disabled", false);
            }
            //div_data = "<option value='" + data[i].nC_Disp_Decision_ListId + "'>" + data[i].nC_Disp_Decision_Desc + "</option>";
            //selElem.append(div_data);
        }
    });
}
function loadUi() {
    var selElem = $('#P3Ui');
    selElem.html('');
    api.getbulk("/Employee/GetAllUilist").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].uiListId + "'>" + data[i].uI_Name_Label + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadCustomer() {
    var selElem = $('#P7RtoCust');
    selElem.html('');
    api.getbulk("/Employee/GetAllUilist").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        div_data = "<option value='" + 1 + "'>" + "Concessionally Accept" + "</option>";
        selElem.append(div_data);
            div_data = "<option value='" + 2 + "'>" + "Rework" + "</option>";
            selElem.append(div_data);
    });
}
function loadFeed() {
    var selElem = $('#P7Feed');
    selElem.html('');
    api.getbulk("/Employee/GetAllUilist").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        div_data = "<option value='" + 1 + "'>" + "Accepted as is" + "</option>";
        selElem.append(div_data);
        div_data = "<option value='" + 2 + "'>" + "Condionally Accepted" + "</option>";
        selElem.append(div_data);
        div_data = "<option value='" + 3 + "'>" + "Rejected" + "</option>";
        selElem.append(div_data);
    });
}

function loadDisplvl1() {
    var selElem = $('#P8lvl1');
    selElem.html('');
    api.getbulk("/Employee/GetAllOrgChart").then((data) => {
        //data = data.filter(item => item.level_No === 1);
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].level_No + "'>" + data[i].level_No + "</option>";
            selElem.append(div_data);
        }

    }).catch((error) => {
    });
}
function loadSelectNCDispDecision27() {
    var selElem = $('#NcPopup27Decision');
    var btnElem = $('#OpenPopup27');
    selElem.html('');
    var existingNames = new Set($("#P7Grid tbody tr").map((_, row) =>
        $(row).find("td.nc-description").text().trim()
    ).get());
    api.getbulk("/WorkOrder/GetAllNC_Disp_Decision_List").then((data) => {
        let hasOptions = false;

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            let optionName = data[i].nC_Disp_Decision_Desc.trim(); 

            if (!existingNames.has(optionName)) { 
                div_data = `<option value='${data[i].nC_Disp_Decision_ListId}'>${optionName}</option>`;
                selElem.append(div_data);
                hasOptions = true;
            }
            if (!hasOptions) {
                btnElem.prop("disabled", true);
            } else {
                btnElem.prop("disabled", false);
            }
        }
    });
}
function loadDepartment() {
    var selElem = $('#P3Resp');
    selElem.html('');
    api.getbulk("/Department/GetDepartments").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].departmentId + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });
}

$(document).ready(function () {
    $('#inwardDoc').on('hidden.bs.modal', function (event) {
        var DocTypeName = $("#inwardDocDocTypeName");
        //var MasterContent = $("#MasterContent");
        $("#InwardDocId").val(0);
        DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
        //MasterContent.find("option[value='" + 0 + "']").prop('selected', true);
        $("#inwardDocUploadMandatory").prop("checked", false);
        $("#Text-Error-inwardDoc").text("");
        var newNamevalidate = document.getElementById('inwardDocDocTypeName');
        newNamevalidate.style.border = '';
    });
    $('#NcPopup3').on('hidden.bs.modal', function (event) {
        $("#P3Resp").val(0);
        $("#P3Ui").val(0);
        $("#P3WorkDesc").val('');
        document.getElementById('NcPopup1').style.filter = 'none';
        $("#P3SeqNo").val('');
    });
    $("#P8lvl1").on("change", function () {
        var value = $(this).val().toLowerCase();

        if (value === "1") {
            $("#P8lvl2").prop("disabled", true);
        } else {
            $("#P8lvl2").prop("disabled", false);
            var selElem = $('#P8lvl2');
            selElem.html('');
            api.getbulk("/Employee/GetAllOrgChart").then((data) => {
                data = data.filter(item => item.level_No === 1);
                div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
                selElem.append(div_data);
                for (i = 0; i < data.length; i++) {
                    if (data[data.length - 1].level_No === parseInt(value)) {
                        continue;
                    }
                    div_data = "<option value='" + data[i].level_No + "'>" + data[i].level_No + "</option>";
                    selElem.append(div_data);
                }

            }).catch((error) => {
            });
        }
    });
    $('#NcPopup8').on('show.bs.modal', function (event) {
        loadDisplvl1();
    });
    $('#NcPopup3').on('show.bs.modal', function (event) {
        document.getElementById('NcPopup1').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var nC_Wk_List_Tmpl_HeadId = relatedTarget.data("docname");
        var nC_Disp_Decision_Id = relatedTarget.data("mandatory");
        var no_of_steps = relatedTarget.data("mastercontent");
        var nC_Disp_Decision_Name = relatedTarget.data("masterdocid");
        $("#ItemDocId").val(nC_Wk_List_Tmpl_HeadId);
        $("#P3NcDisp").val(nC_Disp_Decision_Name);
        var newNamevalidate = document.getElementById('P3WorkDesc');
        newNamevalidate.style.border = '';
        var P3Ui = document.getElementById('P3Ui');
        P3Ui.style.border = '';
        var P3Resp = document.getElementById('P3Resp');
        P3Resp.style.border = '';
        loadNC_Wk_List_Tmpl_Det(nC_Wk_List_Tmpl_HeadId);
    });
    //$("#P3Ui").select2({
    //    dropdownParent: $("#NcPopup3")
    //});
    $('#NcPopup1').on('show.bs.modal', function (event) {
        loadGetAllNC_Wk_List_Tmpl_Head();
        loadDepartment();
        loadUi();
    });
    $('#NcPopup27').on('hidden.bs.modal', function (event) {
        document.getElementById('NcPopup7').style.filter = 'none';
    });
    $('#NcPopup27').on('show.bs.modal', function (event) {
        var newNamevalidate = document.getElementById('NcPopup27Decision');
        newNamevalidate.style.border = '';
        document.getElementById('NcPopup7').style.filter = 'blur(5px)';
    });
    $('#NcPopup2').on('hidden.bs.modal', function (event) {
        document.getElementById('NcPopup1').style.filter = 'none';
    });
    $('#NcPopup2').on('show.bs.modal', function (event) {
        var newNamevalidate = document.getElementById('NcPopup2Decision');
        newNamevalidate.style.border = '';
        document.getElementById('NcPopup1').style.filter = 'blur(5px)';
    });
    $('#inwardDoc').on('show.bs.modal', function (event) {
        loadInwardDocList();
        loadDocTypes();
    });

    $("#P3addNew").on('click', function (event) {
        $("#NcPopup8").modal("hide");
    });
    $("#P3addNew").on('click', function (event) {
        $("#P3Resp").val(0);
        $("#P3Ui").val(0);
        $("#P3WorkDesc").val('');
        $("#P3SeqNo").val('');
    });
    $('#NcPopup5').on('hidden.bs.modal', function (event) {
        document.getElementById('NcPopup4').style.filter = 'none';
        $("#P5AssyChk").prop('checked', false);
        $("#P5Lvl2Chk").prop('checked', false);
        $("#P5RMChk").prop('checked', false);
        $("#P5BofChk").prop('checked', false);
        $("#P5SubChk").prop('checked', false);
        $("#P5CmpChk").prop('checked', false);

    });
    $('#NcPopup5').on('show.bs.modal', function (event) {
        document.getElementById('NcPopup4').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var assy = relatedTarget.data("assy");
        var cmp = relatedTarget.data("cmp");
        var subCon = relatedTarget.data("subcon");
        var bof = relatedTarget.data("bof");
        var rm = relatedTarget.data("rm");
        var ncdispid = relatedTarget.data("ncdispid");
        var ncname = relatedTarget.data("ncname");
        var level = relatedTarget.data("level");
        if (id > 0) {
            var P5NcDispDec = $("#P5NcDispDec");
            div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            valdataa = "<option value='" + ncdispid + "'>" + ncname + "</option>";
            P5NcDispDec.append(div_data);
            P5NcDispDec.append(valdataa);
        }
        $("#P5NcDispAplId").val(id);
        if (assy === 'Y') {
            $("#P5AssyChk").prop('checked', true);
        }
        if (level === 'Y') {
            $("#P5Lvl2Chk").prop('checked', true);
        }
        if (rm === 'Y') {
            $("#P5RMChk").prop('checked', true);
        }
        if (bof === 'Y') {
            $("#P5BofChk").prop('checked', true);
        }
        if (subCon === 'Y') {
            $("#P5SubChk").prop('checked', true);
        }
        if (cmp === 'Y') {
            $("#P5CmpChk").prop('checked', true);
        }
    });
    $('#NcPopup7').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var rm = relatedTarget.data("rm");
        var bof = relatedTarget.data("bof");
        $("#P7MatrixId").val(id);
        $("#P7RtoCust").val(rm);
        $("#P7Feed").val(bof);
        var P7RtoCust = document.getElementById('P7RtoCust');
        P7RtoCust.style.border = '';
        var P7Feed = document.getElementById('P7Feed');
        P7Feed.style.border = '';
        LoadCustMatOpt(id);
        var NcPopup27Decision = document.getElementById('NcPopup27Decision');
        NcPopup27Decision.style.border = '';
        $("#P7Save2").hide();
    });
    $('#NcPopup6').on('show.bs.modal', function (event) {
        loadCustomer();
        loadFeed();
        LoadCustMatrix();
        var newNamevalidate = document.getElementById('P5NcDispDec');
        newNamevalidate.style.border = '';
    });
    $('#NcPopup4').on('show.bs.modal', function (event) {
        loadSelectNCDispDecision();
        loadGetAllNC_Wk_List_Appl();
        var newNamevalidate = document.getElementById('P5NcDispDec');
        newNamevalidate.style.border = '';
    });
    $("#NcP5Save").on('click', function (event) {
        var P5NcDispDec = parseInt($("#P5NcDispDec").val());
        var P5NcDispAplId = parseInt($("#P5NcDispAplId").val());
        var P5Lvl2Chk = document.getElementById("P5Lvl2Chk");
        var P5Lvl2Chkval = 'N';
        if (P5Lvl2Chk.checked) {
            P5Lvl2Chkval = 'Y';
        } 
        var P5RMChk = document.getElementById("P5RMChk");
        var P5RMChkval = 'N';
        if (P5RMChk.checked) {
            P5RMChkval = 'Y';
        } 
        var P5BofChk = document.getElementById("P5BofChk");
        var P5BofChkval = 'N';
        if (P5BofChk.checked) {
            P5BofChkval = 'Y';
        } 
        var P5SubChk = document.getElementById("P5SubChk");
        var P5SubChkval = 'N';
        if (P5SubChk.checked) {
            P5SubChkval = 'Y';
        } 
        var P5CmpChk = document.getElementById("P5CmpChk");
        var P5CmpChkval = 'N';
        if (P5CmpChk.checked) {
            P5CmpChkval = 'Y';
        } 
        var P5AssyChk = document.getElementById("P5AssyChk");
        var P5AssyChkval = 'N';
        if (P5AssyChk.checked) {
            P5AssyChkval = 'Y';
        }
        if (isNaN(P5NcDispAplId)) {
            P5NcDispAplId = 0;
        } 
        if (P5NcDispDec == 0) {
            var newNamevalidate = document.getElementById('P5NcDispDec');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P5NcDispDec');
            newNamevalidate.style.border = '';
        }
        var formdata = {
            nC_Disp_Decs_Appl_ListId: P5NcDispAplId,
            nC_Disp_Decision_Id: P5NcDispDec,
            level_2_Dec_Reqd: P5Lvl2Chkval,
            rm: P5RMChkval,
            bof: P5BofChkval,
            subCon: P5SubChkval,
            cmp: P5CmpChkval,
            assy: P5AssyChkval
        };
        api.post("/WorkOrder/PostNC_Disp_Decs_Appl_List", formdata).then((data) => {
            loadGetAllNC_Wk_List_Appl();
            $("#NcPopup5").modal("hide");
        }).catch((error) => {
            console.log(error);
        });

    });
    $("#P3Save").on('click',function (event) {
        var P3Resp = parseInt($("#P3Resp").val());
        var P3Ui = parseInt($("#P3Ui").val());
        var nC_Wk_List_Tmpl_HeadId = parseInt($("#ItemDocId").val());
        var nC_Wk_List_Tmpl_DetId = parseInt($("#DocTypeId").val());
        var P3WorkDesc = $("#P3WorkDesc").val();
        var P3SeqNo = getLastSeqNo();
        if (P3WorkDesc.length == 0) {
            var newNamevalidate = document.getElementById('P3WorkDesc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P3WorkDesc');
            newNamevalidate.style.border = '';
        }
        if (P3Ui == 0) {
            var newNamevalidate = document.getElementById('P3Ui');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P3Ui');
            newNamevalidate.style.border = '';
        }
        if (P3Resp == 0) {
            var newNamevalidate = document.getElementById('P3Resp');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P3Resp');
            newNamevalidate.style.border = '';
        }
        if (isNaN(nC_Wk_List_Tmpl_DetId)) {
            nC_Wk_List_Tmpl_DetId = 0;
        }
        if (nC_Wk_List_Tmpl_DetId > 0) {
            var sqpn = $("#P3SeqNo").val();
            var formdata = {
                nC_Wk_List_Tmpl_DetId: nC_Wk_List_Tmpl_DetId,
                nC_Wk_List_Tmpl_Appl_Id: nC_Wk_List_Tmpl_HeadId,
                nC_Wk_Step_Desc: P3WorkDesc,
                resp_Dept: P3Resp,
                uI_ID: P3Ui,
                seq_no: parseInt(sqpn)
            };
        } else {
            var formdata = {
                nC_Wk_List_Tmpl_DetId: nC_Wk_List_Tmpl_DetId,
                nC_Wk_List_Tmpl_Appl_Id: nC_Wk_List_Tmpl_HeadId,
                nC_Wk_Step_Desc: P3WorkDesc,
                resp_Dept: P3Resp,
                uI_ID: P3Ui,
                seq_no: parseInt(P3SeqNo) + 1
            };
        }
        api.post("/WorkOrder/PostNC_Wk_List_Tmpl_Det", formdata).then((data) => {
            loadNC_Wk_List_Tmpl_Det(nC_Wk_List_Tmpl_HeadId);
            loadGetAllNC_Wk_List_Tmpl_Head();
            $("#DocTypeId").val('');
            $("#P3Resp").val(0);
            $("#P3Ui").val(0);
            $("#P3WorkDesc").val('');
            $("#P3SeqNo").val('');
        }).catch((error) => {
            console.log(error);
        });
    });
    $("#NcPopup2Save").click(function (event) {
        var NcPopup2Decision = parseInt($("#NcPopup2Decision").val());
        if (NcPopup2Decision == 0) {
            var newNamevalidate = document.getElementById('NcPopup2Decision');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('NcPopup2Decision');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            NC_Wk_List_Tmpl_HeadId: 0,
            NC_Disp_Decision_Id: NcPopup2Decision,
            No_of_steps: 0
        };
        api.post("/WorkOrder/PostNC_Wk_List_Tmpl_Head", rowData).then((data) => {
            loadGetAllNC_Wk_List_Tmpl_Head();
        }).catch((error) => {
            AppUtil.HandleError("FormEditMakeFrom", error);
        });

    });
    $("#AddToinwardDocList").click(function (event) {
        var DocTypeName = parseInt($("#inwardDocDocTypeName").val());
        var ItemDocId = parseInt($("#InwardDocId").val());
        var mandatory = 'N';
        if ($("#inwardDocUploadMandatory").prop("checked") == true) {
            mandatory = 'Y';
        }
        if (DocTypeName == 0) {
            var newNamevalidate = document.getElementById('inwardDocDocTypeName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('inwardDocDocTypeName');
            newNamevalidate.style.border = '';
        }
        var today = new Date();
        today.setDate(today.getDate());
        var deletionDate = today.toISOString().split('T')[0];
        var rowData = {
            InwardDocTypeId: ItemDocId,
            DocumentTypeId: DocTypeName,
            Mandatory: mandatory,
            UpdatedOn: deletionDate
        };
        api.getbulk("/workOrder/GetAllInWardDocLists").then((getdata) => {
            getdata = getdata.filter(item => item.documentTypeId === DocTypeName);
            if (getdata.length===0) {
                api.post("/WorkOrder/PostInWardDocList", rowData).then((data) => {
                    $("#Text-Error-inwardDoc").text("");
                    var DocTypeName = $("#inwardDocDocTypeName");
                    $("#InwardDocId").val(0);
                    DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
                    $("#inwardDocUploadMandatory").prop("checked", false);
                    loadInwardDocList();
                }).catch((error) => {
                    AppUtil.HandleError("FormEditMakeFrom", error);
                });

            } else {
                $("#Text-Error-inwardDoc").text("This Entry Is Already in The List").css('color', 'red');
            }
        }).catch((error) => {
        });
    });
    $('#inspectDoc').on('hidden.bs.modal', function (event) {
        var DocTypeName = $("#inspectDocDocTypeName");
        //var MasterContent = $("#MasterContent");
        $("#inspectDocId").val(0);
        DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
        //MasterContent.find("option[value='" + 0 + "']").prop('selected', true);
        $("#inspectDocUploadMandatory").prop("checked", false);
        $("#Text-Error-inspectDoc").text("");
        var newNamevalidate = document.getElementById('inspectDocDocTypeName');
        newNamevalidate.style.border = '';
    });
    $('#inspectDoc').on('show.bs.modal', function (event) {
        loadinspectDocList();
        loadDocTypes();
    });

    $("#NcPopup27Save").click(function (event) {
        var NcPopup2Decision = parseInt($("#NcPopup27Decision").val());
        var P7MatrixId = parseInt($("#P7MatrixId").val());
        if (P7MatrixId === 0 || isNaN(P7MatrixId)) {
            alert("Please Save The Request To Customer And Feedback from Customer ");
            return false;
        } 
        if (NcPopup2Decision == 0) {
            var newNamevalidate = document.getElementById('NcPopup27Decision');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('NcPopup27Decision');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            Cust_NC_Decs_Matrix_OptId: 0,
            Cust_NC_Decs_Matrix_Id: P7MatrixId,
            NC_Disp_Decision_Id: NcPopup2Decision
        };
        api.post("/WorkOrder/PostCust_NC_Decs_Matrix_Opt", rowData).then((data) => {
            LoadCustMatrix();
            LoadCustMatOpt(P7MatrixId);
            $("#NcPopup27").modal("hide");
        }).catch((error) => {
            AppUtil.HandleError("FormEditMakeFrom", error);
        });

    });
    $("#AddToinspectDoc").click(function (event) {
        var DocTypeName = parseInt($("#inspectDocDocTypeName").val());
        var ItemDocId = parseInt($("#inspectDocId").val());
        var mandatory = 'N';
        if ($("#inspectDocUploadMandatory").prop("checked") == true) {
            mandatory = 'Y';
        }
        if (DocTypeName == 0) {
            var newNamevalidate = document.getElementById('inspectDocDocTypeName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('inspectDocDocTypeName');
            newNamevalidate.style.border = '';
        }
        var today = new Date();
        today.setDate(today.getDate());
        var deletionDate = today.toISOString().split('T')[0];
        var rowData = {
            inspectDocTypeId: ItemDocId,
            DocumentTypeId: DocTypeName,
            Mandatory: mandatory,
            UpdatedOn: deletionDate
        };
        api.getbulk("/workOrder/GetAllInspectDocLists").then((getdata) => {
            getdata = getdata.filter(item => item.documentTypeId === DocTypeName);
            if (getdata.length === 0) {
                api.post("/WorkOrder/PostinspectDocList", rowData).then((data) => {
                    $("#Text-Error-inspectDoc").text("");
                    var DocTypeName = $("#inspectDocDocTypeName");
                    $("#inspectDocId").val(0);
                    DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
                    $("#inspectDocUploadMandatory").prop("checked", false);
                    loadinspectDocList();
                }).catch((error) => {
                    AppUtil.HandleError("FormEditMakeFrom", error);
                });

            } else {
                $("#Text-Error-inspectDoc").text("This Entry Is Already in The List").css('color', 'red');
            }
        }).catch((error) => {
        });
    });
    $("#P7Save").click(function (event) {
        var P7RtoCust = parseInt($("#P7RtoCust").val());
        var P7Feed = parseInt($("#P7Feed").val());
        var P7MatrixId = parseInt($("#P7MatrixId").val());
        if (P7RtoCust == 0) {
            var newNamevalidate = document.getElementById('P7RtoCust');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P7RtoCust');
            newNamevalidate.style.border = '';
        }
        if (P7Feed == 0) {
            var newNamevalidate = document.getElementById('P7Feed');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P7Feed');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P7MatrixId)) {
            P7MatrixId = 0;
        }
        var rowData = {
            Cust_NC_Decs_MatrixId: P7MatrixId,
            Cust_Request_Id: P7RtoCust,
            Cust_Feedback_Id: P7Feed,
            TenantId: 0
        };
        api.post("/WorkOrder/PostCust_NC_Decs_Matrix", rowData).then((data) => {
            $("#P7MatrixId").val(data.cust_NC_Decs_MatrixId);
        }).catch((error) => {

        });
    });
    $('#lineDoc').on('hidden.bs.modal', function (event) {
        var DocTypeName = $("#lineDocTypeName");
        //var MasterContent = $("#MasterContent");
        $("#lineDocId").val(0);
        DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
        //MasterContent.find("option[value='" + 0 + "']").prop('selected', true);
        $("#lineDocUploadMandatory").prop("checked", false);
        $("#Text-Error-lineDoc").text("");
        var newNamevalidate = document.getElementById('lineDocDocTypeName');
        newNamevalidate.style.border = '';
    });
    $('#lineDoc').on('show.bs.modal', function (event) {
        loadlineDocList();
        loadDocTypes();
    });

    $("#AddTolineDocList").click(function (event) {
        var DocTypeName = parseInt($("#lineDocTypeName").val());
        var ItemDocId = parseInt($("#lineDocId").val());
        var mandatory = 'N';
        if ($("#lineDocUploadMandatory").prop("checked") == true) {
            mandatory = 'Y';
        }
        if (DocTypeName == 0) {
            var newNamevalidate = document.getElementById('lineDocTypeName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('lineDocTypeName');
            newNamevalidate.style.border = '';
        }
        var today = new Date();
        today.setDate(today.getDate());
        var deletionDate = today.toISOString().split('T')[0];
        var rowData = {
            LineInspectDocTypeId: ItemDocId,
            DocumentTypeId: DocTypeName,
            Mandatory: mandatory,
            UpdatedOn: deletionDate
        };
        api.getbulk("/workOrder/GetAlllineDocLists").then((getdata) => {
            getdata = getdata.filter(item => item.documentTypeId === DocTypeName);
            if (getdata.length === 0) {
                api.post("/WorkOrder/PostlineDocList", rowData).then((data) => {
                    $("#Text-Error-lineDoc").text("");
                    var DocTypeName = $("#lineDocTypeName");
                    $("#lineDocId").val(0);
                    DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
                    $("#lineDocUploadMandatory").prop("checked", false);
                    loadlineDocList();
                }).catch((error) => {
                    AppUtil.HandleError("FormEditMakeFrom", error);
                });

            } else {
                $("#Text-Error-lineDoc").text("This Entry Is Already in The List").css('color', 'red');
            }
        }).catch((error) => {
        });
    });
    $('#finalDoc').on('hidden.bs.modal', function (event) {
        var DocTypeName = $("#finalDocTypeName");
        //var MasterContent = $("#MasterContent");
        $("#finalDocId").val(0);
        DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
        //MasterContent.find("option[value='" + 0 + "']").prop('selected', true);
        $("#finalDocUploadMandatory").prop("checked", false);
        $("#Text-Error-finalDoc").text("");
        var newNamevalidate = document.getElementById('finalDocDocTypeName');
        newNamevalidate.style.border = '';
    });
    $('#finalDoc').on('show.bs.modal', function (event) {
        loadfinalDocList();
        loadDocTypes();
    });

    $("#AddTofinalDocList").click(function (event) {
        var DocTypeName = parseInt($("#finalDocTypeName").val());
        var ItemDocId = parseInt($("#finalDocId").val());
        var mandatory = 'N';
        if ($("#finalDocUploadMandatory").prop("checked") == true) {
            mandatory = 'Y';
        }
        if (DocTypeName == 0) {
            var newNamevalidate = document.getElementById('finalDocTypeName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('finalDocTypeName');
            newNamevalidate.style.border = '';
        }
        var today = new Date();
        today.setDate(today.getDate());
        var deletionDate = today.toISOString().split('T')[0];
        var rowData = {
            finalInspectDocTypeId: ItemDocId,
            DocumentTypeId: DocTypeName,
            Mandatory: mandatory,
            UpdatedOn: deletionDate
        };
        api.getbulk("/workOrder/GetAllfinalDocLists").then((getdata) => {
            getdata = getdata.filter(item => item.documentTypeId === DocTypeName);
            if (getdata.length === 0) {
                api.post("/WorkOrder/PostfinalDocList", rowData).then((data) => {
                    $("#Text-Error-finalDoc").text("");
                    var DocTypeName = $("#finalDocTypeName");
                    $("#finalDocId").val(0);
                    DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
                    $("#finalDocUploadMandatory").prop("checked", false);
                    loadfinalDocList();
                }).catch((error) => {
                    AppUtil.HandleError("FormEditMakeFrom", error);
                });

            } else {
                $("#Text-Error-finalDoc").text("This Entry Is Already in The List").css('color', 'red');
            }
        }).catch((error) => {
        });
    });
    $('#rcacaDoc').on('hidden.bs.modal', function (event) {
        var DocTypeName = $("#rcacaDocTypeName");
        //var MasterContent = $("#MasterContent");
        $("#rccaDocId").val(0);
        DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
        //MasterContent.find("option[value='" + 0 + "']").prop('selected', true);
        $("#rcacaDocUploadMandatory").prop("checked", false);
        $("#Text-Error-rcacaDoc").text("");
        var newNamevalidate = document.getElementById('rcacaDocDocTypeName');
        newNamevalidate.style.border = '';
    });
    $('#rcacaDoc').on('show.bs.modal', function (event) {
        loadrcacaDocList();
        loadDocTypes();
    });

    $("#UiEnableBtn").click(function (event) {
        let uncheckedItems = [];
        let operationSettingsMap = {};
        api.getbulk("/workOrder/GetAllOperationSettings")
            .then((data) => {
                if (data.length > 0) {
                    data.forEach(item => {
                        let checkboxId = item.uiName.replace(/\s+/g, "");
                        operationSettingsMap[checkboxId] = item.operationSettingsId;
                    });


                    document.querySelectorAll(".form-check-input").forEach(checkbox => {
                        if (!checkbox.checked || checkbox.checked) {
                            let checkboxId = checkbox.id;
                            let enabledisable="N"
                            if (checkbox.checked) {
                                enabledisable = "Y"
                            }
                            uncheckedItems.push({
                                operationSettingsId: operationSettingsMap[checkboxId] || null,
                                uiName: checkbox.id.replace(/([a-z])([A-Z])/g, "$1 $2"), // Convert back to UI name with spaces
                                enableDisable: enabledisable
                            });
                        }
                    });

                    if (uncheckedItems.length > 0) {
                        // Filter items where OperationSettingsId is > 0 and not null
                        let validItems = uncheckedItems.filter(item => item.operationSettingsId > 0 && item.OperationSettingsId !== null);

                        if (validItems.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: '/WorkOrder/PostOperationsSettings',
                                contentType: "application/json; charset=utf-8",
                                headers: { 'Content-Type': 'application/json' },
                                data: JSON.stringify(validItems),
                                dataType: "json",
                                success: function (result) {
                                    UiEnableDisable();
                                    alert("Changes saved successfully!");
                                }
                            });
                        } else {
                            alert("No valid changes to save.");
                        }
                    } else {
                        alert("No changes detected.");
                    }

                }
            })
            .catch((error) => {
                console.error("Error fetching operation settings:", error);
            });
    });
    $("#AddTorcacaDocList").click(function (event) {
        var DocTypeName = parseInt($("#rcacaDocTypeName").val());
        var ItemDocId = parseInt($("#rccaDocId").val());
        var mandatory = 'N';
        if ($("#rcacaDocUploadMandatory").prop("checked") == true) {
            mandatory = 'Y';
        }
        if (DocTypeName == 0) {
            var newNamevalidate = document.getElementById('rcacaDocTypeName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('rcacaDocTypeName');
            newNamevalidate.style.border = '';
        }
        var today = new Date();
        today.setDate(today.getDate());
        var deletionDate = today.toISOString().split('T')[0];
        var rowData = {
            rcCaDocTypeId: ItemDocId,
            DocumentTypeId: DocTypeName,
            Mandatory: mandatory,
            UpdatedOn: deletionDate
        };
        api.getbulk("/workOrder/GetAllrcacaDocLists").then((getdata) => {
            getdata = getdata.filter(item => item.documentTypeId === DocTypeName);
            if (getdata.length === 0) {
                api.post("/WorkOrder/PostrcacaDocList", rowData).then((data) => {
                    $("#Text-Error-rcacaDoc").text("");
                    var DocTypeName = $("#rcacaDocTypeName");
                    $("#rccaDocId").val(0);
                    DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
                    $("#rcacaDocUploadMandatory").prop("checked", false);
                    loadrcacaDocList();
                }).catch((error) => {
                    AppUtil.HandleError("FormEditMakeFrom", error);
                });

            } else {
                $("#Text-Error-rcacaDoc").text("This Entry Is Already in The List").css('color', 'red');
            }
        }).catch((error) => {
        });
    });
    UiEnableDisable();

    $('#McPopup11').on('show.bs.modal', function (event) {
        $("#McP11SuccesMsg").text('');
        loadTimeSettings();
        checkNoOfShiftsAndDisplayFields();
    });
    $('#McPopup12').on('show.bs.modal', function (event) {
        loadMcNotAvlReasonList();
    });
    $('#McPopup13').on('show.bs.modal', function (event) {
        document.getElementById('McPopup12').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("mcnotid");
        var reasondesc = relatedTarget.data("reasondesc");
        if (!isNaN(id)) {
            $("#Mc13ReasonId").val(id);
            $("#Mc13Reason").val(reasondesc);
        } else {
            $("#Mc13ReasonId").val('');
            $("#Mc13Reason").val('');
        }
    });
    $('#McPopup13').on('hidden.bs.modal', function (event) {
        document.getElementById('McPopup12').style.filter = 'none';
        var newNamevalidate = document.getElementById('Mc13Reason');
        newNamevalidate.style.border = '';
        $("#Text-Error-mcreason").text("");
    });
    $("#SaveMcPopup13").click(function (event) {
        var Mc13Reason = $("#Mc13Reason").val();
        var Mc13ReasonId = parseInt($("#Mc13ReasonId").val());
        if (Mc13Reason.length == 0) {
            var newNamevalidate = document.getElementById('Mc13Reason');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('Mc13Reason');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            mc_Not_Avl_ReasonId: Mc13ReasonId,
            reason_Desc: Mc13Reason
        };
        api.getbulk("/workOrder/GetAllMc_not_avl_reason").then((getdata) => {
            getdata = getdata.filter(item => item.reason_Desc === Mc13Reason);
            if (getdata.length === 0) {
                api.post("/WorkOrder/PostMc_not_avl_reason", rowData).then((data) => {
                    $("#Text-Error-mcreason").text("");
                    loadMcNotAvlReasonList();
                    $("#McPopup13").modal("hide");
                }).catch((error) => {
                    AppUtil.HandleError("FormEditMakeFrom", error);
                });

            } else {
                $("#Text-Error-mcreason").text("This Entry Is Already In The List").css('color', 'red');
            }
        }).catch((error) => {
        });
    });

    $("#SaveMcPopup14").click(function (event) {
        const currentVal = $("#Mc14Reason").val();
        if (currentVal.length == 0) {
            var newNamevalidate = document.getElementById('Mc14Reason');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('Mc14Reason');
            newNamevalidate.style.border = '';
        }
        if (parseInt(currentVal) < 0 || parseInt(currentVal) > 100) {
            var newNamevalidate = document.getElementById('Mc14Reason');
            alert("% of Production to be completed before Next Opr Parallel Opr can start should be 0 to 100% ");   
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('Mc14Reason');
            newNamevalidate.style.border = '';
        }
        setCookie("simulationpercent", currentVal);
    });
    $('#McPopup14').on('show.bs.modal', function (event) {
        var newNamevalidate = document.getElementById('Mc14Reason');
        newNamevalidate.style.border = '';
        const savedValue = getCookie("simulationpercent");
        $("#Mc14Reason").val(savedValue);
    });
    $("#McP11Save").click(function (event) {
        var McP11TimeSettingId = parseInt($("#McP11TimeSettingId").val());
        var McP11TimeDur = parseInt($("#McP11TimeDur").val());
        var McP11NoDay = parseInt($("#McP11NoDay").val());
        var McP11NoRetenDay = $("#McP11NoRetenDay").val();
        var McP11FirstStartTime = $("#McP11FirstStartTime").val();
        var McP11FirstDur = $("#McP11FirstDur").val();
        var McP11SecStart = $("#McP11SecStart").val();
        var McP11SecDur = $("#McP11SecDur").val();
        var McP11ThirdStart = $("#McP11ThirdStart").val();
        var McP11ThirdDur = $("#McP11ThirdDur").val();
        if (McP11TimeDur == 0) {
            var newNamevalidate = document.getElementById('McP11TimeDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('McP11TimeDur');
            newNamevalidate.style.border = '';
        }
        if (McP11NoDay == 0) {
            var newNamevalidate = document.getElementById('McP11NoDay');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('McP11NoDay');
            newNamevalidate.style.border = '';
        }
        if (McP11NoRetenDay.length == 0) {
            var newNamevalidate = document.getElementById('McP11NoRetenDay');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('McP11NoRetenDay');
            newNamevalidate.style.border = '';
        }
        if (allmaxShifts >= 1) {
            if (McP11FirstStartTime.length == 0) {
                var newNamevalidate = document.getElementById('McP11FirstStartTime');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('McP11FirstStartTime');
                newNamevalidate.style.border = '';
            }
            var inputElement = document.getElementById('McP11FirstDur');
            var timePattern = /^([0-5]?[0-9]):[0-5][0-9]$/; // matches 0–59 minutes and 0–59 seconds

            if (McP11FirstDur.length === 0 || !timePattern.test(McP11FirstDur)) {
                inputElement.style.border = '2px solid red';
                return false;
            } else {
                inputElement.style.border = '';
            }
        }
        if (allmaxShifts >= 2) {

            if (McP11SecStart.length == 0) {
                var newNamevalidate = document.getElementById('McP11SecStart');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('McP11SecStart');
                newNamevalidate.style.border = '';
            }
            var inputElementMcP11SecDur = document.getElementById('McP11SecDur');
            var timePatternMcP11SecDur = /^([0-5]?[0-9]):[0-5][0-9]$/; // matches 0–59 minutes and 0–59 seconds

            if (McP11SecDur.length === 0 || !timePatternMcP11SecDur.test(McP11SecDur)) {
                inputElementMcP11SecDur.style.border = '2px solid red';
                return false;
            } else {
                inputElementMcP11SecDur.style.border = '';
            }
        }

        if (allmaxShifts === 3) {
            if (McP11ThirdStart.length == 0) {
                var newNamevalidate = document.getElementById('McP11ThirdStart');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('McP11ThirdStart');
                newNamevalidate.style.border = '';
            }
            var inputElementMcP11ThirdDur = document.getElementById('McP11ThirdDur');
            var timePatternMcP11ThirdDur = /^([0-5]?[0-9]):[0-5][0-9]$/; // matches 0–59 minutes and 0–59 seconds
            if (McP11ThirdDur.length === 0 || !timePatternMcP11ThirdDur.test(McP11ThirdDur)) {
                inputElementMcP11ThirdDur.style.border = '2px solid red';
                return false;
            } else {
                inputElementMcP11ThirdDur.style.border = '';
            }
        }
        getShiftEndLimitsAndValidate(allmaxShifts).then(isValid => {
            if (!isValid) return; // stop if validation fails

            var rowData = {
                timeslot_SettingId: McP11TimeSettingId,
                timeslot_duration: McP11TimeDur,
                no_of_span_days: McP11NoDay,
                retention_Days: McP11NoRetenDay,
                first_Shift_Break_start_time: McP11FirstStartTime,
                first_Shift_Break_duration: McP11FirstDur,
                sec_Shift_Break_start_time: McP11SecStart,
                sec_Shift_Break_duration: McP11SecDur,
                third_Shift_Break_start_time: McP11ThirdStart,
                third_Shift_Break_duration: McP11ThirdDur,
                change_flag: 'N'
            };

            api.post("/WorkOrder/PostTimeslot_Setting", rowData).then((data) => {
                $("#Text-Error-mcreason").text("");
                loadMcNotAvlReasonList();
                $("#McPopup11").modal("hide");
            }).catch((error) => {
                AppUtil.HandleError("FormEditMakeFrom", error);
            });
        }).catch(error => {
            console.error("Error during shift validation:", error);
        });

    });
    $("#TimeSlotSettingsBtn").on("click", function () {
        $.ajax({
            type: "POST",
            url: '/WorkOrder/PostTimeslot_List',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            success: function (result) {
                console.log("Success:", result);
            }
        });
    });
    $('#woWaitList').on('show.bs.modal', function (event) {
        loadWoWaitList();
    });
    $('#oprList').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var mcnotid = relatedTarget.data("mcnotid");
        loadOprList(mcnotid);
    });
    $('#mcWaitList').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var mcnotid = relatedTarget.data("mcnotid");
        loadMcWaitList(mcnotid);
    });
    $('#McPopup9').on('show.bs.modal', function (event) {
        loadMcCoverage();
    });
    $('#McPopup10').on('hidden.bs.modal', function (event) {
        document.getElementById('McPopup9').style.filter = 'none';
    });
    $('#McPopup10').on('show.bs.modal', function (event) {
        document.getElementById('McPopup9').style.filter = 'blur(5px)';
        var newNamevalidate = document.getElementById('P10NoDays');
        newNamevalidate.style.border = '';
        var P10IssueDay = document.getElementById('P10IssueDay');
        P10IssueDay.style.border = '';
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var shop = relatedTarget.data("shop");
        var shopid = relatedTarget.data("shopid");
        var nodays = relatedTarget.data("nodays");
        var nodays = relatedTarget.data("nodays");
        var issueday = relatedTarget.data("issueday");
        $("#P9ShopName").text(shop);
        $("#P10NoDays").val(nodays);
        $("#P10IssueDay").val(issueday);
        $("#P10Id").val(id);
        $("#P10ShopId").val(shopid);
    });
    $("#SaveMcPopup10").click(function (event) {
        var P10NoDays = parseInt($("#P10NoDays").val());
        var P10IssueDay = parseInt($("#P10IssueDay").val());
        var P10ShopId = parseInt($("#P10ShopId").val());
        var P10Id = parseInt($("#P10Id").val());
        if (P10NoDays == 0) {
            var newNamevalidate = document.getElementById('P10NoDays');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P10NoDays');
            newNamevalidate.style.border = '';
        }
        if (P10NoDays < 0 || P10NoDays > 10) {
            var newNamevalidate = document.getElementById('P10NoDays');
            newNamevalidate.style.border = '2px solid red';
            alert(" No of Days of Coverage Should be between 1 to 10");
            return false;
        } else {
            var newNamevalidate = document.getElementById('P10NoDays');
            newNamevalidate.style.border = '';
        }
        if (P10IssueDay == 0) {
            var newNamevalidate = document.getElementById('P10IssueDay');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P10IssueDay');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            matl_Issue_SettingsId: P10Id,
            shop_Id: P10ShopId,
            no_days_coverage: P10NoDays,
            issueDay: P10IssueDay
        };

        api.post("/WorkOrder/PostMatl_Issue_Settings", rowData).then((data) => {
            $("#Text-Error-mcreason").text("");
            loadMcCoverage();
            $("#McPopup10").modal("hide");
        }).catch((error) => {
            AppUtil.HandleError("FormEditMakeFrom", error);
        });

    });
});
function loadMcCoverage() {
    var tablebody = $("#P9Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllMatl_Issue_Settings").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("inspectDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("P9GridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function loadWoWaitList() {
    var tablebody = $("#woWaitListGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllWO_Wait_List").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("finalDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("woWaitListGridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function loadOprList(woid) {
    var tablebody = $("#oprListGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllOpr_List").then((data) => {
        data = data.filter(item => item.wo_Id === woid);
        const seenOprNos = new Set();

        // Filter to keep only the first occurrence of each opr_No
        const uniqueData = data.filter(item => {
            if (seenOprNos.has(item.opr_No)) {
                return false;
            } else {
                seenOprNos.add(item.opr_No);
                return true;
            }
        });

        for (let i = 0; i < uniqueData.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("oprListGridRow", uniqueData[i]));
        }
    }).catch((error) => {
    });
}
function loadMcWaitList(oprid) {
    var tablebody = $("#mcWaitListGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllMc_Wait_List").then((data) => {
        data = data.filter(item => item.opr_No_Id === oprid);
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("finalDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("mcWaitListGridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
//document.getElementById("saveButton").addEventListener("click", function () {
//});
function getShiftEndLimitsAndValidate(allmaxShifts) {
    return new Promise((resolve, reject) => {
        api.getbulk("/plant/getplants").then((plants) => {
            let shiftPromises = plants.map(plant =>
                api.getbulk("/plant/getplantwd?plantId=" + plant.plantId)
            );

            Promise.all(shiftPromises).then(results => {
                let maxShiftEndTimes = {
                    1: "00:00",
                    2: "00:00",
                    3: "00:00"
                };

                results.forEach(plantData => {
                    for (let shift = 1; shift <= plantData.noOfShifts; shift++) {
                        let startKey, durKey;

                        if (shift === 1) {
                            startKey = 'firstShiftStartTime';
                            durKey = 'firstShiftDuration';
                        } else if (shift === 2) {
                            startKey = 'secondShiftStartTime';
                            durKey = 'secondShiftDuration';
                        } else if (shift === 3) {
                            startKey = 'thirdShiftStartTime';
                            durKey = 'thirdShiftDuration';
                        }

                        if (plantData[startKey] && plantData[durKey]) {
                            const start = plantData[startKey]; // "HH:mm"
                            const duration = parseHHMMtoMinutes(plantData[durKey]); // minutes

                            const endTime = addMinutesToTime(start, duration);

                            if (endTime > maxShiftEndTimes[shift]) {
                                maxShiftEndTimes[shift] = endTime;
                            }
                        }
                    }
                });

                const isValid = validateUserTimes(allmaxShifts, maxShiftEndTimes);
                resolve(isValid);
            }).catch(err => {
                console.error("Error getting plant data", err);
                reject(err);
            });
        }).catch(err => {
            console.error("Error getting plants", err);
            reject(err);
        });
    });
}

function parseHHMMtoMinutes(timeStr) {
    const [hours, minutes] = timeStr.split(":").map(Number);
    return (hours * 60) + minutes;
}


// Helper to add minutes to HH:mm time string
function addMinutesToTime(timeStr, minutesToAdd) {
    const [h, m] = timeStr.split(':').map(Number);
    const date = new Date();
    date.setHours(h, m + minutesToAdd);
    const hh = String(date.getHours()).padStart(2, '0');
    const mm = String(date.getMinutes()).padStart(2, '0');
    return `${hh}:${mm}`;
}

function validateUserTimes(allmaxShifts, shiftLimits) {
    if (allmaxShifts >= 1) {
        const user1 = document.getElementById('McP11FirstStartTime').value;
        if (user1 > shiftLimits[1]) {
            alert(`1st Shift Break Start Time is out of range. Max allowed is ${to12HourFormat(shiftLimits[1])}`);
            document.getElementById('McP11FirstStartTime').style.border = '2px solid red';
            return false;
        }
    }

    if (allmaxShifts >= 2) {
        const user2 = document.getElementById('McP11SecStart').value;
        if (user2 > shiftLimits[2]) {
            alert(`2nd Shift Break Start Time is out of range. Max allowed is ${to12HourFormat(shiftLimits[2])}`);
            document.getElementById('McP11SecStart').style.border = '2px solid red';
            return false;
        }
    }

    if (allmaxShifts >= 3) {
        const user3 = document.getElementById('McP11ThirdStart').value;
        if (user3 > shiftLimits[3]) {
            alert(`3rd Shift Break Start Time is out of range. Max allowed is ${to12HourFormat(shiftLimits[3])}`);
            document.getElementById('McP11ThirdStart').style.border = '2px solid red';
            return false;
        }
    }

    return true;
}

function to12HourFormat(timeStr) {
    const [hourStr, minuteStr] = timeStr.split(":");
    let hour = parseInt(hourStr);
    const minute = minuteStr.padStart(2, '0');

    const ampm = hour >= 12 ? 'PM' : 'AM';
    hour = hour % 12 || 12;

    return `${hour}:${minute} ${ampm}`;
}

function checkNoOfShiftsAndDisplayFields() {
    let maxShifts = 0;

    api.getbulk("/plant/getplants").then((plants) => {
        let shiftPromises = plants.map(plant =>
            api.getbulk("/plant/getplantwd?plantId=" + plant.plantId)
        );

        Promise.all(shiftPromises).then(results => {
            results.forEach(plantData => {
                if (plantData && plantData.noOfShifts > maxShifts) {
                    maxShifts = plantData.noOfShifts;
                }
            });

            toggleShiftFields(maxShifts);
        }).catch(error => {
            console.error("Error fetching plant shift data:", error);
        });
    }).catch(error => {
        console.error("Error fetching plants:", error);
    });
}
function toggleShiftFields(maxShifts) {
    allmaxShifts = maxShifts;
    if (maxShifts >= 1) {
        $('#McP11FirstStartTimelbl').show();
        $('#McP11FirstStartTime').show();
        $('#McP11FirstDurlbl').show();
        $('#McP11FirstDur').show();
    } else {
        $('#McP11FirstStartTimelbl').hide();
        $('#McP11FirstStartTime').hide();
        $('#McP11FirstDurlbl').hide();
        $('#McP11FirstDur').hide();
    }
    if (maxShifts >= 2) {
        $('#McP11SecStartTimelbl').show();
        $('#McP11SecStart').show();
        $('#McP11SecDurlbl').show();
        $('#McP11SecDur').show();
    } else {
        $('#McP11SecStartTimelbl').hide();
        $('#McP11SecStart').hide();
        $('#McP11SecDurlbl').hide();
        $('#McP11SecDur').hide();
    }
    if (maxShifts >= 3) {
        $('#McP11ThirdStartTimelbl').show();
        $('#McP11ThirdStart').show();
        $('#McP11ThirdDurlbl').show();
        $('#McP11ThirdDur').show();
    } else {
        $('#McP11ThirdStartTimelbl').hide();
        $('#McP11ThirdStart').hide();
        $('#McP11ThirdDurlbl').hide();
        $('#McP11ThirdDur').hide();
    }
}
function loadTimeSettings() {
    api.getbulk("/workOrder/GetAllTimeslot_Setting").then((data) => {
        if (data && data.length > 0) {
            const lastItem = data[data.length - 1];
            $("#McP11TimeDur").val(lastItem.timeslot_duration);
            $("#McP11NoDay").val(lastItem.no_of_span_days);
            $("#McP11NoRetenDay").val(lastItem.retention_Days);
            $("#McP11FirstStartTime").val(lastItem.first_Shift_Break_start_time);
            $("#McP11FirstDur").val(lastItem.first_Shift_Break_duration);
            $("#McP11SecStart").val(lastItem.sec_Shift_Break_start_time);
            $("#McP11SecDur").val(lastItem.sec_Shift_Break_duration);
            $("#McP11ThirdStart").val(lastItem.third_Shift_Break_start_time);
            $("#McP11ThirdDur").val(lastItem.third_Shift_Break_duration);
            $("#McP11TimeSettingId").val(lastItem.timeslot_SettingId);
            var changeflag = lastItem.Change_flag;
        } else {
            //console.log("No data received.");
        }
    }).catch((error) => {
    });
}
function setCookie(name, value) {
    const days = 365 * 20;
    const expires = new Date(Date.now() + days * 864e5).toUTCString();
    document.cookie = name + "=" + encodeURIComponent(value) + "; expires=" + expires + "; path=/";
    $("#McPopup14").modal("hide");
}

function getCookie(name) {
    return document.cookie.split('; ').reduce((r, v) => {
        const parts = v.split('=');
        return parts[0] === name ? decodeURIComponent(parts[1]) : r;
    }, '');
}
function loadMcNotAvlReasonList() {
    var tablebody = $("#McP12Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllMc_not_avl_reason").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("rcacaDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("McPopup12GridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function getLastSeqNo() {
    var lastSeqNo = 0; // Default if no rows exist

    // Check if there are any rows in the table body
    var lastRow = $("#NcPopup3Grid tbody tr:last");

    if (lastRow.length > 0) {
        lastSeqNo = parseInt(lastRow.find("td:first").text().trim()) || 0;
    }

    return lastSeqNo;
}
function UiEnableDisable() {
    var tablebody = $("#inwardDocGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllOperationSettings").then((data) => {
        if (0 != data.length) {
            data.forEach(item => {
                let checkboxId = item.uiName.replace(/\s+/g, ""); // Remove spaces
                let checkbox = document.getElementById(checkboxId);
                if (checkbox) {
                    checkbox.checked = item.enableDisable === 'Y';
                }
            });
        }
    }).catch((error) => {
    });
}
function loadInwardDocList() {
    var tablebody = $("#inwardDocGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllInWardDocLists").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("inwardDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("inwardDocRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function loadinspectDocList() {
    var tablebody = $("#inspectDocGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllInspectDocLists").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("inspectDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("inspectDocRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function loadNC_Wk_List_Tmpl_Det(nC_Wk_List_Tmpl_Appl_Id) {
    var tablebody = $("#NcPopup3Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllNC_Wk_List_Tmpl_Det").then((data) => {
        data = data.filter(item => item.nC_Wk_List_Tmpl_Appl_Id === nC_Wk_List_Tmpl_Appl_Id);
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("lineDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("NcPopup3GridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
        loadSelectNCDispDecision();
    }).catch((error) => {
    });
}
function loadGetAllNC_Wk_List_Appl() {
    var tablebody = $("#NcPopup4Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllNC_Disp_Decs_Appl_List").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("lineDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("NcPopup4GridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
        loadSelectNCDispDecision();
    }).catch((error) => {
    });
}
function LoadCustMatrix() {
    var tablebody = $("#NcP6Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllCust_NC_Decs_Matrix").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("lineDocRow", data[i]);
            if (data[i].cust_Request_Id === 1) {
                data[i].request = "Concessionally Accept";
            } else {
                data[i].request = "Rework";
            }
            if (data[i].cust_Feedback_Id === "1") {
                data[i].feed = "Accepted as is";
            } else if (data[i].cust_Feedback_Id === "2") {
                data[i].feed = "Condionally Accepted";
            } else {
                data[i].feed = "Rejected";
            }
            $(tablebody).append(AppUtil.ProcessTemplateData("NcPopup6GridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function LoadCustMatOpt(matid) {
    var tablebody = $("#P7Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllCust_NC_Decs_Matrix_Opt").then((data) => {
        data = data.filter(item => item.cust_NC_Decs_Matrix_Id === matid);
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("lineDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("NcPopup7GridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
        loadSelectNCDispDecision27();
    }).catch((error) => {
    });
}
function DeleteNcPopup1(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeleteNC_Wk_List_Tmpl_Det?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadGetAllNC_Wk_List_Tmpl_Head();
                }).catch((error) => {

                });
            }
}
function DeleteNcPopup3(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
    var headid = relatedTarget.data("headid");
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeleteNC_Wk_List_Tmpl_Det?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadGetAllNC_Wk_List_Tmpl_Head();
                    loadNC_Wk_List_Tmpl_Det(headid);
                }).catch((error) => {

                });
            }
}
function DeleteP4NcDelete(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeleteNC_Disp_Decs_Appl_List?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadGetAllNC_Wk_List_Appl();
                }).catch((error) => {

                });
            }
}
function DeleteP6NcDelete(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeleteCust_NC_Decs_Matrix?itemMasterDocListId=" + masterdocid).then((data) => {
                    LoadCustMatrix();
                }).catch((error) => {

                });
            }
}
function DeleteP7(element) {
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeleteCust_NC_Decs_Matrix_Opt?itemMasterDocListId=" + element).then((data) => {
                    var matid= $("#P7MatrixId").val();
                    LoadCustMatOpt(parseInt(matid));
                }).catch((error) => {

                });
            }
}
function loadGetAllNC_Wk_List_Tmpl_Head() {
    var tablebody = $("#NcPopup1Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllNC_Wk_List_Tmpl_Head").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("lineDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("NcPopup1GridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
        loadSelectNCDispDecision();
    }).catch((error) => {
    });
}
function loadlineDocList() {
    var tablebody = $("#lineDocGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAlllineDocLists").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("lineDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("lineDocRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function loadfinalDocList() {
    var tablebody = $("#finalDocGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllfinalDocLists").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("finalDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("finalDocRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function loadrcacaDocList() {
    var tablebody = $("#rcacaDocGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllrcacaDocLists").then((data) => {
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("rcacaDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("rcacaDocRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function DeleteinwardDoc(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
    var doctypeId = relatedTarget.data("docname");
    api.get("/masters/CheckDocTypeInDocList?docTypeid=" + doctypeId).then((data) => {
        // loadDocType(); 
        if (data) {
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeleteInWardDocList?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadInwardDocList();
                }).catch((error) => {

                });
            }
        } else {
            alert("Deletion of This can be done after files with the Extn are deleted from the System");
        }
    }).catch((error) => {

    });
}
function EditNcPopup3(element) {
    var relatedTarget = $(element);
    var headid = relatedTarget.data("headid");
    var id = relatedTarget.data("id");
    var seqno = relatedTarget.data("seqno");
    var respdept = relatedTarget.data("respdept");
    var uiid = relatedTarget.data("uiid");
    var ncwkdesc = relatedTarget.data("ncwkdesc");
    var DocTypeName = $("#P3Ui");
    DocTypeName.find("option[value='" + uiid + "']").prop('selected', true);
    var P3Resp = $("#P3Resp");
    P3Resp.find("option[value='" + respdept + "']").prop('selected', true);
    $("#ItemDocId").val(headid);
    $("#DocTypeId").val(id);
    $("#P3SeqNo").val(seqno);
    $("#P3WorkDesc").val(ncwkdesc);
}
function EditinwardDoc(element) {
    var relatedTarget = $(element);
    var docname = relatedTarget.data("docname");
    var mandatory = relatedTarget.data("mandatory");
    var masterdocid = relatedTarget.data("masterdocid");
    var DocTypeName = $("#inwardDocDocTypeName");
    var MasterContent = $("#MasterContent");
    $("#InwardDocId").val(masterdocid);
    DocTypeName.find("option[value='" + docname + "']").prop('selected', true);
    //MasterContent.find("option[value='" + mastercontent + "']").prop('selected', true);
    if (mandatory == 'Y') {
        $("#inwardDocUploadMandatory").prop("checked", true);
    }

}
function DeleteinspectDoc(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
    var doctypeId = relatedTarget.data("docname");
    api.get("/masters/CheckDocTypeInDocList?docTypeid=" + doctypeId).then((data) => {
        // loadDocType(); 
        if (data) {
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeleteinspectDocList?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadinspectDocList();
                }).catch((error) => {

                });
            }
        } else {
            alert("Deletion of This can be done after files with the Extn are deleted from the System");
        }
    }).catch((error) => {

    });
}
function EditinspectDoc(element) {
    var relatedTarget = $(element);
    var docname = relatedTarget.data("docname");
    var mandatory = relatedTarget.data("mandatory");
    var masterdocid = relatedTarget.data("masterdocid");
    var DocTypeName = $("#inspectDocDocTypeName");
    var MasterContent = $("#MasterContent");
    $("#inspectDocId").val(masterdocid);
    DocTypeName.find("option[value='" + docname + "']").prop('selected', true);
    //MasterContent.find("option[value='" + mastercontent + "']").prop('selected', true);
    if (mandatory == 'Y') {
        $("#inspectDocUploadMandatory").prop("checked", true);
    }

}
function DeletelineDoc(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
    var doctypeId = relatedTarget.data("docname");
    api.get("/masters/CheckDocTypeInDocList?docTypeid=" + doctypeId).then((data) => {
        // loadDocType(); 
        if (data) {
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeletelineDocList?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadlineDocList();
                }).catch((error) => {

                });
            }
        } else {
            alert("Deletion of This can be done after files with the Extn are deleted from the System");
        }
    }).catch((error) => {

    });
}
function EditlineDoc(element) {
    var relatedTarget = $(element);
    var docname = relatedTarget.data("docname");
    var mandatory = relatedTarget.data("mandatory");
    var masterdocid = relatedTarget.data("masterdocid");
    var DocTypeName = $("#lineDocTypeName");
    $("#lineDocId").val(masterdocid);
    DocTypeName.find("option[value='" + docname + "']").prop('selected', true);
    //MasterContent.find("option[value='" + mastercontent + "']").prop('selected', true);
    if (mandatory == 'Y') {
        $("#lineDocUploadMandatory").prop("checked", true);
    }

}
function DeletefinalDoc(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
    var doctypeId = relatedTarget.data("docname");
    api.get("/masters/CheckDocTypeInDocList?docTypeid=" + doctypeId).then((data) => {
        // loadDocType(); 
        if (data) {
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeletefinalDocList?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadfinalDocList();
                }).catch((error) => {

                });
            }
        } else {
            alert("Deletion of This can be done after files with the Extn are deleted from the System");
        }
    }).catch((error) => {

    });
}
function EditfinalDoc(element) {
    var relatedTarget = $(element);
    var docname = relatedTarget.data("docname");
    var mandatory = relatedTarget.data("mandatory");
    var masterdocid = relatedTarget.data("masterdocid");
    var DocTypeName = $("#finalDocTypeName");
    $("#finalDocId").val(masterdocid);
    DocTypeName.find("option[value='" + docname + "']").prop('selected', true);
    //MasterContent.find("option[value='" + mastercontent + "']").prop('selected', true);
    if (mandatory == 'Y') {
        $("#finalDocUploadMandatory").prop("checked", true);
    }

}
function DeleteRcaCaDoc(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
    var doctypeId = relatedTarget.data("docname");
    api.get("/masters/CheckDocTypeInDocList?docTypeid=" + doctypeId).then((data) => {
        // loadDocType(); 
        if (data) {
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/workOrder/DeletercacaDocList?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadrcacaDocList();
                }).catch((error) => {

                });
            }
        } else {
            alert("Deletion of This can be done after files with the Extn are deleted from the System");
        }
    }).catch((error) => {

    });
}
function EditRcaCaDoc(element) {
    var relatedTarget = $(element);
    var docname = relatedTarget.data("docname");
    var mandatory = relatedTarget.data("mandatory");
    var masterdocid = relatedTarget.data("masterdocid");
    var DocTypeName = $("#rcacaDocTypeName");
    $("#rccaDocId").val(masterdocid);
    DocTypeName.find("option[value='" + docname + "']").prop('selected', true);
    //MasterContent.find("option[value='" + mastercontent + "']").prop('selected', true);
    if (mandatory == 'Y') {
        $("#rcacaDocUploadMandatory").prop("checked", true);
    }

}