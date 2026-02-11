let archive = 0;
let accpet = 0;
function loadPO() {
    api.getbulk("/WorkOrder/GetAllInw_Recpt_HeaderInsp").then((data) => {
        data = data.filter(item => item.status >= 3);
        var tablebody = $("#inspGrid tbody");
        $(tablebody).html("");
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("POGrid1Row", data[i]));
        }
    }).catch((error) => {
    });
}

function loadSuppliers() {
    var selElem = $('#searchSupp');
    selElem.html('');
    api.getbulk("/masters/Suppliers").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].companyId + "'>" + data[i].companyName + "</option>";
            selElem.append(div_data);
        }
    });
}
function calculateTotals() {
    let ourCountTotal = 0;
    let dcCountTotal = 0;

    // Loop through table rows and sum up the values
    document.querySelectorAll("#P6InwardGrid tbody tr").forEach(row => {
        let ourCount = parseFloat(row.children[3].innerText) || 0;  // Assuming "Our Count" is in the 2nd column
        let dcCount = parseFloat(row.children[3].innerText) || 0;   // Assuming "Supplier DC Count" is in the 3rd column

        ourCountTotal += ourCount;
        dcCountTotal += dcCount;
    });

    // Display totals in footer
    document.getElementById("P6TbTotalOff").value = ourCountTotal;
    var P6TbInspTotal = $("#P6TbTotalNc").val();
    if (isNaN(P6TbInspTotal)) {
        P6TbInspTotal = 0;
    }
    document.getElementById("P6TotalInsp").value = 0;
    //document.getElementById("P6TotalInsp").value = ourCountTotal - P6TbInspTotal;
    //document.getElementById("unitsTbVM").innerText = "Nos";
    //document.getElementById("P6TbInspTotalUnit").innerText = "Nos";
    //document.getElementById("P6TbTotalNcUnit").innerText = "Nos";
}
function calculateTotalInsp() {
    let inspTotal = parseFloat($("#P6TbInspTotal").val()) || 0;
    let totalNc = parseFloat($("#P6TbTotalNc").val()) || 0;
    let unpro = parseFloat($("#P6TbUnpro").val()) || 0;

    let total = inspTotal + totalNc + unpro;
    $("#P6TotalInsp").val(total);
}
async function showPopup() {
    const popup = document.querySelector('#popupInspect6');

    if (popup) {
        // Execute the code right before displaying the popup
        popup.style.overflow = 'auto';
        popup.style.display = 'block';
        popup.style.opacity = '1';

        const data = {};
        const popupStyles = window.getComputedStyle(popup);
        data.popupStyles = {
            overflow: popupStyles.overflow,
            height: popupStyles.height,
            display: popupStyles.display,
            position: popupStyles.position,
            overflowY: popupStyles.overflowY,
            overflowX: popupStyles.overflowX,
        };
        data.scrollHeight = popup.scrollHeight;
        data.clientHeight = popup.clientHeight;
    }
}

function loadInwardDetails(inwheaderid) {
    api.getbulk("/WorkOrder/GetAllInw_Recpt_Details").then((data) => {
        data = data.filter(item => item.inw_Recpt_Header_Id === parseInt(inwheaderid));
        var tablebody = $("#P6InwardGrid tbody");
        $(tablebody).html("");
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P4inwardGridRow", data[i]));
            calculateTotals();
        }
        var inspcount = data.filter(item => item.inward_Condition === 1).length;
        //document.getElementById("P6TbUnpro").value = inspcount;
    }).catch((error) => {
    });
}
function loadNclog(inwheaderid) {
    api.getbulk("/WorkOrder/GetAllNcLogInsp").then((data) => {
        data = data.filter(item => item.inw_Recpt_Header_Id === parseInt(inwheaderid));
        var tablebody = $("#P6NcGrid tbody");
        $(tablebody).html("");
        let totalQuantity = 0;
        for (i = 0; i < data.length; i++) {
            totalQuantity += data[i].nC_Qnty;
            $(tablebody).append(AppUtil.ProcessTemplateData("P6BallonGridRow", data[i]));
            $("#P6TotalNc").val(totalQuantity);
            $("#P6TbTotalNc").val(totalQuantity);
            $("#P6TotalInsp").val(totalQuantity);
            $("#P6FinNcCount").val(totalQuantity);
            calculateTotals();
        }
        if (totalQuantity === 0) {
            $("#P6FinNcCount").hide();
            $("#P6FinNcUnit").hide();
        } else {
            $("#P6FinNcCount").show();
            $("#P6FinNcUnit").show();
        }
    }).catch((error) => {
    });
}
$(document).ready(function () {
    loadPO();
    loadSuppliers();

    $('#P6NcCnfChk').change(function () {
        if ($(this).is(':checked')) {
            $("#P6Exit").prop("disabled", false);
        } else {
            $("#P6Exit").prop("disabled", true);
        }
    });
    $('#P7CnfNcChk').change(function () {
        if ($(this).is(':checked')) {
            $("#P7Exit").prop("disabled", false);
        } else {
            $("#P7Exit").prop("disabled", true);
        }
    });
    $("#searchPoNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#inspGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#inspGrid tbody");
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
    $("#searchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#inspGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#inspGrid tbody");
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
    $("#searchSupp").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#searchSupp option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#inspGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(selectedText) > -1)
            });
            var $tableBody = $("#inspGrid tbody");
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
        } else {
            $("#inspGrid tbody tr").show();
            var $tableBody = $("#inspGrid tbody");
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
        }
    });
    $("#searchPartType").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#searchPartType option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#inspGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(selectedText) > -1)
            });
            var $tableBody = $("#inspGrid tbody");
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
        } else {
            $("#inspGrid tbody tr").show();
            var $tableBody = $("#inspGrid tbody");
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
        }
    });
    $('#InspectionDocPop').on('show.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'blur(5px)';
    });
    $('#InspectionDocPop').on('hidden.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'none';
    });
    $('#PartInspectionDocPop').on('show.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'blur(5px)';
    });
    $('#PartInspectionDocPop').on('hidden.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'none';
    });
    $('#UploadDocumnet').on('show.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'blur(5px)';
    });
    $('#UploadDocumnet').on('hidden.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'none';
    });
    $('#popup7').on('hidden.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'none';
    });
    $('#popup7').on('show.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var balno = relatedTarget.data("balno");
        var baldesc = relatedTarget.data("baldesc");
        var ncdes = relatedTarget.data("ncdes");
        var declsup = relatedTarget.data("declsup");
        var qnty = relatedTarget.data("qnty");
        var nctrack = relatedTarget.data("nctrack");
        var loc = relatedTarget.data("loc");
        var parttype = $("#P6Ncparttype").val();
        if (declsup === "Y") {
            $("#P7NcDesclaredBySup").prop("checked", true);
        } else {
            $("#P7NcDesclaredBySup").prop("checked", false);
        }
        if (parttype == "SubCon") {
            $("#P7NcLocation").val(1);
        } else {
            $("#P7NcLocation").val(2);
        }
        $("#P7BallonNo").val(balno);
        $("#P7NcBallonDesc").val(baldesc);
        $("#P7NcDesc").val(ncdes);
        $("#P7NcQnty").val(qnty);
        $("#P7NcTrackNo").val(nctrack);
        $("#P7NcId").val(id);
    });
    $('#ErrorMessage4').on('show.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'blur(5px)';
    });
    $('#ErrorMessage4').on('hidden.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'none';
    });
    $('#ErrorMessage5').on('show.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'blur(5px)';
    });
    $('#ErrorMessage5').on('hidden.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'none';
    });
    $("#ExitWarningBtn").on("click", function () {
        $("#popup7").modal("hide");
        $("#warning").modal("hide");
    });
    $("#EP7Exit").on("click", function () {
        $("#popup7").modal("hide");
        $("#NotUploaded").modal("hide");
    });
    $("#P7BtnClose").on("click", function () {
            $("#popup7").modal("hide");
    });
    $("#EP7Return").on("click", function () {
        $("#popup7").modal("hide");
        $("#NotUploaded").modal("hide");
    });
    $("#P6TbInspTotal, #P6TbTotalNc, #P6TbUnpro").on("input", calculateTotalInsp);
    document.getElementById("P6FinNcCount").addEventListener("input", function () {
        var totalGridNC = $("#P6TotalInsp").val();
        var P6FinNcCount = this.value;
        if (P6FinNcCount > totalGridNC) {
            $("#ErrorMessage12").modal("show");
        }
    });
    document.getElementById("P6InputNcCount").addEventListener("input", function () {
        var totalGridNC = $("#P6TotalNc").val();
        var P6FinNcCount = this.value;
        if (parseInt(P6FinNcCount) > parseInt(totalGridNC)) {
            $("#ErrorMessage12").modal("show");
        }
    });
    $('#ErrorMessage12').on('show.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'blur(5px)';
    });
    $('#ErrorMessage12').on('hidden.bs.modal', function (event) {
        document.getElementById('popupInspect6').style.filter = 'none';
    });

    $('#popupInspect6').on('show.bs.modal', function (event) {
        $("#P6TbInspTotal").val(0);
        $("#P6TotalInsp").val(0);
        $("#P6TbUnpro").val(0);
        $("#P6TbTotalNc").val(0);
        $("#P6TotalNc").val(0);
        showPopup();
        var relatedTarget = $(event.relatedTarget);
        var supp = relatedTarget.data("supp");
        var poref = relatedTarget.data("poref");
        var podate = relatedTarget.data("podate");
        var porcptdate = relatedTarget.data("porcptdate");
        var podetails = relatedTarget.data("podetails");
        var noofline = relatedTarget.data("noofline");
        var partid = relatedTarget.data("partid");
        var edit = relatedTarget.data("edit");
        var partno = relatedTarget.data("partno");
        var procid = relatedTarget.data("procid");
        var poqnty = relatedTarget.data("poqnty");
        var inwheaderid = relatedTarget.data("inwheaderid");
        var parttype = relatedTarget.data("parttype");
        var partId = relatedTarget.data("partid");
        var woid = relatedTarget.data("woid");
        var units = relatedTarget.data("units");
        var poid = relatedTarget.data("poid");
        var loc = 1;
        $("#P6TbTotalOffUnit").text(units);
        $("#unitsTbVM").text(units);
        $("#P6TbUnproUnit").text(units);
        $("#P6TbInspTotalUnit").text(units);
        $("#P6TbTotalNcUnit").text(units);
        $("#P6InputNcUnit").text(units);
        $("#P6FinNcUnit").text(units);
        $("#P7UnitVM").text(units);
        $("#P6TotalInspUnit").text(units);
        $("#P6SuppSpan").text(supp);
        $("#P6PartNoSpan").text(partno);
        $("#P7PartId").val(partid);
        $("#P6PartId").val(partId);
        $("#P6WoId").val(woid);
        $("#P6PoId").val(poid);
        $("#P7RoutingDiv").prop("hidden", true);
        $("#P6RoutingDiv").prop("hidden", true);
        $("#P6Ncparttype").val(parttype);
        $("#P7InwHeaderId").val(inwheaderid);
        loadInwardDetails(inwheaderid);
        loadNclog(inwheaderid);
        loadPartDoc(inwheaderid);
        loadVendorDoc(inwheaderid);
        loadDetailsDocUploadList(inwheaderid);
        $("#subcontabOkQntylablTd").hide();
        $("#P6MessageBox").text("");
        $("#P7NcLocation").prop("disabled", true);
        $("#P6FinNc").hide();
        $("#P6FinNcTr").hide();
        $("#P6PrintFin").hide();
        $("#P6InputNc").show();
        $("#P6InputNcTr").show();
        $("#P6PrintInput").show();
        if (parttype == "SubCon") {
            $("#P7RoutingDiv").prop("hidden",false);
            $("#P6RoutingDiv").prop("hidden",false);
            $("#subcontabOkQntylablTd").show();
            var qntyValue = 0;
            $("#P6InwardGrid tbody tr").each(function () {
                var inwardingData = $(this).find("td:nth-child(1)").text().trim();

                if (inwardingData === "Unprocessed Input Material returned by Supplier") {
                    qntyValue = $(this).find("td:nth-child(4)").text().trim();
                }
                $("#P6TbUnpro").val(qntyValue);
            });
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partId)).then((data) => {
                api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                    var routname = data[data.length - 1].routingName;
                    var opname = stepdata[stepdata.length - 1].stepNumber;
                    $("#P7RoutingSpan").text(routname);
                    $("#P6RoutingSpan").text(routname);
                    $("#P7OprNoSpan").text(opname);
                    $("#P6OprNoSpan").text(opname);
                });
            });
            $("#P6InputNc").hide();
            $("#P6InputNcTr").hide();
            $("#P6PrintInput").hide();
            $("#P6FinNc").show();
            $("#P6FinNcTr").show();
            $("#P6PrintFin").show();
        }
    });
    $("#P6FinNc").on("click", function () {
        $("#popup7").modal("show");
        var P7NcBallonDesc = document.getElementById('P7NcBallonDesc');
        P7NcBallonDesc.style.border = '';
        var P7NcDesc = document.getElementById('P7NcDesc');
        P7NcDesc.style.border = '';
        var P7NcQnty = document.getElementById('P7NcQnty');
        P7NcQnty.style.border = '';
        var partno = $("#P6PartNoSpan").text();
        var supp = $("#P6SuppSpan").text();
        $("#P7SuppSpan").text(supp);
        $("#P7PartNoSpan").text(partno);
        $("#P7FinOrInSpan").text("( Finish )");
    });
    $("#popupInwardClose").on("click", function () {
        $("#popupInspect6").modal("hide");
    });
    $("#P7Save").secureClick( function () {
        var P7BallonNo = $("#P7BallonNo").val();
        var P7NcBallonDesc = $("#P7NcBallonDesc").val();
        var P7NcDesc = $("#P7NcDesc").val();
        var P7NcQnty = parseInt($("#P7NcQnty").val());
        var P7InwHeaderId = parseInt($("#P7InwHeaderId").val());
        var P7PartId = parseInt($("#P7PartId").val());
        var P7NcId = parseInt($("#P7NcId").val());
        var P7NcLocation = $("#P7NcLocation").val();
        var checkbox = document.getElementById("P7NcDesclaredBySup");
        if (P7NcBallonDesc.length === 0) {
            var newNamevalidate = document.getElementById('P7NcBallonDesc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P7NcBallonDesc');
            newNamevalidate.style.border = '';
        }
        if (P7NcDesc.length === 0) {
            var newNamevalidate = document.getElementById('P7NcDesc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P7NcDesc');
            newNamevalidate.style.border = '';
        }
        if (P7NcQnty === 0 || isNaN(P7NcQnty)) {
            var newNamevalidate = document.getElementById('P7NcQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P7NcQnty');
            newNamevalidate.style.border = '';
        }
        var decl = 'N';
        if (checkbox.checked) {
            decl = 'Y';
        }
        if (isNaN(P7NcId)) {
            P7NcId = 0;
        }
        var rowData = {
            insp_Outcome_Details_Id: parseInt(P7NcId),
            inw_Recpt_Header_Id: P7InwHeaderId,
            inw_Recpt_Part_No_Id: P7PartId,
            balloon_No: P7BallonNo,
            balloon_No_Dir: P7NcBallonDesc,
            feature_Descrip: P7NcDesc,
            nC_Descrip: P7NcDesc,
            nC_Qnty: P7NcQnty,
            decl_by_Supplier: decl,
            storage_Location: P7NcLocation,
            nC_Log_status_Id: 1
        };
        return $.ajax({
            type: "POST",
            url: '/workOrder/PostInspNcLog',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(rowData),
            dataType: "json",
            success: function (result) {
                loadNclog(P7InwHeaderId);
                alert("Non Conformance Data Entered Successfully.");
            }
        });
    });
    $("#P6InputNc").on("click", function () {
        $("#popup7").modal("show");
        var P7NcBallonDesc = document.getElementById('P7NcBallonDesc');
        P7NcBallonDesc.style.border = '';
        var P7NcBallonDesc = document.getElementById('P7NcBallonDesc');
        P7NcBallonDesc.style.border = '';
        var P7NcDesc = document.getElementById('P7NcDesc');
        P7NcDesc.style.border = '';
        var P7NcQnty = document.getElementById('P7NcQnty');
        P7NcQnty.style.border = '';
        var partno = $("#P6PartNoSpan").text();
        var supp = $("#P6SuppSpan").text();
        $("#P7SuppSpan").text(supp);
        $("#P7PartNoSpan").text(partno);
        $("#P7FinOrInSpan").text("( Input )");
    });

    $("#Error4Edit").on("click", function () {
        $("#ErrorMessage4").modal("hide");
    });
    $("#Error5Edit").on("click", function () {
        $("#ErrorMessage5").modal("hide");
    });
    $("#Error5ExitAccpt").on("click", function () {
        $("#ErrorMessage5").modal("hide");
        accpet++;
    });
    $("#Error5ExitWith").on("click", function () {
        $("#ErrorMessage3").modal("hide");
        $("#popupInspect6").modal("hide");
    });
    $("#Error4Exit").on("click", function () {
        $("#ErrorMessage4").modal("hide");
        $("#popupInspect6").modal("hide");
    });
    $("#P6PrintFin").on("click", function () {
        $("#popup8").modal("show");
        var partno = $("#P6PartNoSpan").text();
        var supp = $("#P6SuppSpan").text();
        var qnty = $("#P6TotalNc").text();
        $("#P8With").val(qnty);
        $("#P8SuppSpan").text(supp);
        $("#P8PartNoSpan").text(partno);
        $("#P8RoutingSpan").hide();
    });
    $("#P7PrintLbl").on("click", function () {
        $("#popup8").modal("show");
        var partno = $("#P6PartNoSpan").text();
        var supp = $("#P6SuppSpan").text();
        $("#P8SuppSpan").text(supp);
        var qnty = $("#P6TotalNc").text();
        $("#P8With").val(qnty);
        $("#P8PartNoSpan").text(partno);
        $("#P8RoutingSpan").hide();
    });
    $("#P6PrintInput").on("click", function () {
        $("#popup8").modal("show");
        var qnty = $("#P6TotalNc").text();
        $("#P8With").val(qnty);
        var partno = $("#P6PartNoSpan").text();
        var supp = $("#P6SuppSpan").text();
        $("#P8SuppSpan").text(supp);
        $("#P8PartNoSpan").text(partno);
    });
    $("#P6LineInspupdate").secureClick(function () {
        var ourCountVM = $("#P6TbInspTotal").val();
        var P6TbUnpro = $("#P6TbUnpro").val();
        var suppCountVM = $("#P6TbTotalOff").val();
        var P6TbTotalNc = $("#P6TbTotalNc").val();
        if (P6TbTotalNc === "") {
            P6TbTotalNc = 0;
        }
        if (P6TbUnpro === "") {
            P6TbUnpro = 0;
        }
        var totalinps = parseInt(ourCountVM) + parseInt(P6TbTotalNc) + parseInt(P6TbUnpro);
        if (parseInt(suppCountVM) != parseInt(totalinps)) {
            $("#ErrorMessage4").modal("show");
        } else if (parseInt(suppCountVM) > parseInt(totalinps)) {
            //$("#ErrorMessage5").modal("show");
        } else {
            var partType = $("#P6Ncparttype").val();
            if (partType === "RawMaterial" || partType === "BOF") {
                var qnty = parseInt($("#P6TbInspTotal").val());
                var P6PartId = parseInt($("#P6PartId").val());
                var P6PoId = parseInt($("#P6PoId").val());

                return api.getbulk("/WorkOrder/GetAllInv_Trans_Log").then((logdata) => {
                    var heid = parseInt($("#P7InwHeaderId").val());
                    logdata = logdata.filter(item => item.pO_No_Id === heid);
                    var logId = 0;
                    if (Array.isArray(logdata) && logdata.length > 0) {
                        logId = logdata[logdata.length - 1].inv_Trans_LogId || 0;
                    } else {
                        logId = 0;
                    }
                    var firstColumnText = $('#P6InwardGrid tbody tr:first-child td:first-child').text();
                    var partstatus = 0;
                    if (firstColumnText != "OK Parts declared by Supplier" || firstColumnText != "NC Qnty declared by Supplier") {
                        partstatus = 2;
                    } else {
                        partstatus = 1;
                    }
                    var rowData = {
                        Inv_Trans_LogId: 0,
                        Input_Part_NoId: 0,
                        Input_Routing_Id: 0,
                        Input_Opr_No: 0,
                        Output_Part_No: P6PartId,
                        Output_Routing_Id: 0,
                        Output_Opr_No: 0,
                        Wo_Id: 0,
                        PO_No_Id: P6PoId,
                        Transaction_Id: 1,
                        Qnty: qnty,
                        From_Location_Id: 0,
                        To_Location_Id: 1,
                        Part_Status: partstatus,
                        Movement_Compl: 'N'
                    };
                    api.post("/WorkOrder/PostInv_Trans_Log", rowData).then((Insdata) => {
                        $("#P6MessageBox").text("Inspection Complete");
                        var InvMasterrowData = {
                            Part_NoId: P6PartId,
                            Routing_Id: 0,
                            Inv_Trans_Log_Id: Insdata.inv_Trans_LogId,
                            Opr_No_Id: 0,
                            Current_QntOnHand: qnty,
                            Location_Id: 1
                        };
                        api.post("/WorkOrder/PostInventory_Master", InvMasterrowData).then((data) => {


                        }).catch((error) => {
                            console.log(error);
                        });
                    }).catch((error) => {
                        console.log(error);
                    });

                }).catch((error) => {
                });
            } else {

                var qnty = parseInt($("#P6TotalInsp").val());
                var P6PartId = parseInt($("#P6PartId").val());
                var P6PoId = parseInt($("#P6PoId").val());
                var P6WoId = parseInt($("#P6WoId").val());

                var qntyValue = 0;
                $("#P6InwardGrid tbody tr").each(function () {
                    var inwardingData = $(this).find("td:nth-child(1)").text().trim();

                    if (inwardingData === "Unprocessed Input Material returned by Supplier") {
                        qntyValue = $(this).find("td:nth-child(4)").text().trim();
                    }
                    $("#P6TbUnpro").val(qntyValue);
                });
                if (parseInt(qntyValue) > suppCountVM && accpet === 0) {
                    $("#ErrorMessage5").modal("show");
                } else {
                    return api.getbulk("/WorkOrder/GetAllInv_Trans_Log").then((logdata) => {
                        var heid = parseInt($("#P7InwHeaderId").val());
                        logdata = logdata.filter(item => item.pO_No_Id === heid);
                        var logId = 0;
                        if (Array.isArray(logdata) && logdata.length > 0) {
                            logId = logdata[logdata.length - 1].inv_Trans_LogId || 0;
                        } else {
                            logId = 0;
                        }
                        api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(P6PartId)).then((data) => {
                            api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {

                                var firstColumnText = $('#P6InwardGrid tbody tr:first-child td:first-child').text();
                                var partstatus = 0;
                                if (firstColumnText != "OK Parts declared by Supplier" || firstColumnText != "NC Qnty declared by Supplier") {
                                    partstatus = 2;
                                } else {
                                    partstatus = 1;
                                }
                                var rowData = {
                                    Inv_Trans_LogId: 0,
                                    Input_Part_NoId: P6PartId,
                                    Input_Routing_Id: data[0].routingId,
                                    Input_Opr_No: stepdata[0].stepId,
                                    Output_Part_No: P6PartId,
                                    Output_Routing_Id: data[data.length - 1].routingId,
                                    Output_Opr_No: stepdata[data.length - 1].stepId,
                                    Wo_Id: P6WoId,
                                    PO_No_Id: P6PoId,
                                    Transaction_Id: 2,
                                    Qnty: qnty,
                                    From_Location_Id: stepdata[0].stepLocation,
                                    To_Location_Id: 1,
                                    Part_Status: partstatus,
                                    Movement_Compl: 'N'
                                };
                                api.post("/WorkOrder/PostInv_Trans_Log", rowData).then((Insdata) => {
                                    $("#P6MessageBox").text("Inspection Complete");
                                    var InvMasterrowData = {
                                        Part_NoId: P6PartId,
                                        Routing_Id: data[data.length - 1].routingId,
                                        Inv_Trans_Log_Id: Insdata.inv_Trans_LogId,
                                        Opr_No_Id: stepdata[data.length - 1].stepId,
                                        Current_QntOnHand: qnty,
                                        Location_Id: 1
                                    };
                                    api.post("/WorkOrder/PostInventory_Master", InvMasterrowData).then((data) => {
                                        accpet = 0;

                                    }).catch((error) => {
                                        console.log(error);
                                    });
                                }).catch((error) => {
                                    console.log(error);
                                });
                            });
                        });
                    });
                }
            }
        }
    });
    $('#doc-item').on('hidden.bs.modal', function (event) {
        var InfoComments = document.getElementById('InfoComments');
        InfoComments.style.border = '';
        var newNamevalidate = document.getElementById('fileNameDisplay');
        newNamevalidate.style.border = '';
        $("#doclistidFile").val(0);
        $("#fileNameDisplay").val('');
        $("#InfoComments").val('');
        $("#DocTypeName").val('');
        $("#FileExtnName").val('');
        $("#docTypeIdFile").val(0);
        var fileInput = document.getElementById("fileUploadInput");
        fileInput.value = "";

    });
    $('#doc-item').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var filename = relatedTarget.data("filename");
        var doctypename = relatedTarget.data("doctypename");
        var comments = relatedTarget.data("comments");
        var doclistid = relatedTarget.data("doclistid");
        var documenttypeid = relatedTarget.data("documenttypeid");
        var upload = relatedTarget.data("upload");
        var fileextnname = relatedTarget.data("fileextnname");
        var deletiondate = relatedTarget.data("deletiondate");
        if (doclistid == 0 && upload == 1) {
            $("#doclistidFile").val(0);
            $("#fileNameDisplay").val('');
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        } else if (doclistid > 0 && upload == 2) {
            $("#doclistidFile").val(doclistid);
            $("#fileNameDisplay").val(filename);
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        }
        else {
            $("#DocTypeName").val("Others");
            $("#docTypeIdFile").val(3);
            $("#doclistidFile").val(0);
            $("#FileExtnName").val("Any Extn");
            var today = new Date();
            today.setDate(today.getDate() + 10);
            var deletionDate = today.toISOString().split('T')[0];
            $("#deletiondate").val(deletionDate);

        }

    $('#doc-item').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var filename = relatedTarget.data("filename");
        var doctypename = relatedTarget.data("doctypename");
        var comments = relatedTarget.data("comments");
        var doclistid = relatedTarget.data("doclistid");
        var documenttypeid = relatedTarget.data("documenttypeid");
        var upload = relatedTarget.data("upload");
        var fileextnname = relatedTarget.data("fileextnname");
        var deletiondate = relatedTarget.data("deletiondate");
        if (doclistid == 0 && upload == 1) {
            $("#doclistidFile").val(0);
            $("#fileNameDisplay").val('');
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        } else if (doclistid > 0 && upload == 2) {
            $("#doclistidFile").val(doclistid);
            $("#fileNameDisplay").val(filename);
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        }
        else {
            $("#DocTypeName").val("Others");
            $("#docTypeIdFile").val(3);
            $("#doclistidFile").val(0);
            $("#FileExtnName").val("Any Extn");
            var today = new Date();
            today.setDate(today.getDate() + 10);
            var deletionDate = today.toISOString().split('T')[0];
            $("#deletiondate").val(deletionDate);

        }

    });
    $('#doc-item').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var filename = relatedTarget.data("filename");
        var doctypename = relatedTarget.data("doctypename");
        var comments = relatedTarget.data("comments");
        var doclistid = relatedTarget.data("doclistid");
        var documenttypeid = relatedTarget.data("documenttypeid");
        var upload = relatedTarget.data("upload");
        var fileextnname = relatedTarget.data("fileextnname");
        var deletiondate = relatedTarget.data("deletiondate");
        if (doclistid == 0 && upload == 1) {
            $("#doclistidFile").val(0);
            $("#fileNameDisplay").val('');
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        } else if (doclistid > 0 && upload == 2) {
            $("#doclistidFile").val(doclistid);
            $("#fileNameDisplay").val(filename);
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        }
        else {
            $("#DocTypeName").val("Others");
            $("#docTypeIdFile").val(3);
            $("#doclistidFile").val(0);
            $("#FileExtnName").val("Any Extn");
            var today = new Date();
            today.setDate(today.getDate() + 10);
            var deletionDate = today.toISOString().split('T')[0];
            $("#deletiondate").val(deletionDate);

        }

    });
    });
    document.getElementById("UploadFileSave").addEventListener("click", function (event) {
        event.preventDefault();  // Prevent the default form submission
        var partids = parseInt($("#popupInwardHeaderId").val());
        if (partids <= 0) {
            alert("Please Save The Header Info .");
            return false;
        }
        var com = document.getElementById("InfoComments").value;
        if (com.trim() === "" || com.length === 1) {
            var newNamevalidate = document.getElementById('InfoComments');
            newNamevalidate.style.border = '2px solid red'; // Set border to red for invalid input
            return false; // Prevent form submission or further processing
        } else {
            var newNamevalidate = document.getElementById('InfoComments');
            newNamevalidate.style.border = ''; // Clear the border for valid input
        }
        // Create a FormData object to hold file and form data
        var formData = new FormData();

        // Add the file to the FormData object
        var fileInput = document.getElementById("fileUploadInput");
        var file = fileInput.files[0];
        var allowedExtensions = $("#FileExtnName").val().split(',').map(function (ext) {
            return ext.trim().toLowerCase(); // Create an array of allowed extensions (e.g., ['.pdf'])
        });
        if (!isNaN(file) || file) {
            var fileName = file.name;
            var fileExtension = '.' + fileName.split('.').pop().toLowerCase(); // Get the file extension and add a dot (e.g., '.pdf')

            // Validate that the file's extension is in the allowedExtensions array
            if (allowedExtensions.includes(fileExtension) || allowedExtensions.includes("any extn")) {
                formData.append("uploadedFile", file);
            } else {
                var newNamevalidate = document.getElementById('fileNameDisplay');
                newNamevalidate.style.border = '2px solid red';
                $("#fileNameDisplay").val("Invalid file type. Please upload a valid file."); // Show error message for invalid file type
                return false;
            }
        } else {
            var newNamevalidate = document.getElementById('fileNameDisplay');
            newNamevalidate.style.border = '2px solid red';
            return false;
        }

        // Add the other form inputs to the FormData object
        formData.append("DocumentTypeName", document.getElementById("DocTypeName").value);
        formData.append("FileExtnName", document.getElementById("FileExtnName").value);
        formData.append("FileName", document.getElementById("fileNameDisplay").value);
        formData.append("StorageLocation", "/Active");
        formData.append("Comments", document.getElementById("InfoComments").value);
        formData.append("DocListId", parseInt(document.getElementById("doclistidFile").value));
        formData.append("DocumentTypeId", parseInt(document.getElementById("docTypeIdFile").value));
        formData.append("PartId", parseInt(document.getElementById("popupPoPartId").value));
        formData.append("Inw_Recpt_HeaderId", parseInt(document.getElementById("P7InwHeaderId").value));
        formData.append("UploadUiId", parseInt(1));
        //var today = new Date();
        //today.setDate(today.getDate() + 10);  // Add 10 days to the current date

        //// Format the date as YYYY-MM-DD (you can modify this to your required format)
        //var deletionDate = today.toISOString().split('T')[0];

        // Append the DeletionDate to the FormData
        var deletionDateValue = document.getElementById("deletiondate").value;
        var deletionDate = new Date(deletionDateValue);

        var formattedDate = deletionDate.toISOString().split('T')[0];

        formData.append("DeletionDate", formattedDate);
        // Post the form data to the server
        if (archive == 0) {
            $.ajax({
                type: "POST",
                url: "/masters/PostDocList",
                data: formData,
                contentType: false,  // Important: Let the browser set the Content-Type header automatically
                processData: false,  // Important: Don't process the form data, let it be as FormData
                success: function (response) {
                    // Handle the success response here
                    //alert("File uploaded and data saved successfully!");
                    $("#doc-item").modal("hide");
                    loadDocUploadList();
                    archive = 0;
                },
                error: function (xhr, status, error) {
                    // Handle any errors here
                    console.error("An error occurred:", error);
                    alert("There was an error saving the file and data.");
                }
            });
        } else {
            $.ajax({
                type: "POST",
                url: "/masters/MoveFileToArchive",
                data: formData,
                contentType: false,  // Important: Let the browser set the Content-Type header automatically
                processData: false,  // Important: Don't process the form data, let it be as FormData
                success: function (response) {
                    // Handle the success response here
                    //alert("File uploaded and data saved successfully!");
                    $("#doc-item").modal("hide");
                    loadDocUploadList();
                    archive = 0;
                },
                error: function (xhr, status, error) {
                    // Handle any errors here
                    console.error("An error occurred:", error);
                    alert("There was an error saving the file and data.");
                }
            });
        }
    });
});
function viewFile(element) {
    var relatedTarget = $(element);
    var file = relatedTarget.data("filename");
    var doctypename = relatedTarget.data("doctypename");
    var customername = $("#CompanyId option:selected").text();
    var partno = $("#PartNo").val();
    var partdesc = $("#PartDescription").val();
    var routingname = relatedTarget.data("routingname");
    var oprno = relatedTarget.data("oprno");
    var retdate = relatedTarget.data("retdate");
    if (file == null || file == "") {
        $('#viewDoc').modal('hide');
        return false;
    }
    if (doctypename == "Other") {
        $('#viewDoc').modal('hide');
        return false;
    } else {
        $('#viewDoc').modal('show');
        $("#DocTypenameText").text(doctypename);
        $("#PartNoText").text(partno);
        $("#PartDescText").text(partdesc);
        $("#CustomerText").text(customername);
        $("#RetentionDateText").text(retdate);
        $("#RoutingNameText").text(routingname);
        $("#OprNoText").text(oprno);


        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/masters/ViewFile?fileName=' + file, true);
        xhr.responseType = 'arraybuffer';
        xhr.onload = function (e) {
            if (this.status == 200) {
                var blob = new Blob([this.response], { type: "application/pdf" });

                const objectElement = document.getElementById('fileViewer');
                const url = URL.createObjectURL(blob);
                objectElement.src = url;
                //objectElement.width = '1000px';
                //objectElement.height = '1000px';
                //objectElement.type = 'text/plain';
                //var link = document.createElement('a');
                //link.href = window.URL.createObjectURL(blob);
                //link.download = "Report_" + new Date() + ".pdf";
                //link.click();
            }
        };
        xhr.send();
    }
}

function downloadFile(element) {
    var relatedTarget = $(element);
    var file = relatedTarget.data("filename");

    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/masters/ViewFile?fileName=' + file, true);
    xhr.responseType = 'arraybuffer';
    xhr.onload = function (e) {
        if (this.status == 200) {
            var blob = new Blob([this.response], { type: "application/pdf" });

            //const objectElement = document.getElementById('fileViewer');
            //const url = URL.createObjectURL(blob);
            //objectElement.src = url;
            //objectElement.width = '1000px';
            //objectElement.height = '1000px';
            //objectElement.type = 'text/plain';
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = file;
            link.click();
        }
    };
    xhr.send();
}

function loadDetailsDocUploadList() {

    var inw_Recpt_HeaderId = $("#P7InwHeaderId").val();
    if (isNaN(inw_Recpt_HeaderId)) {
        api.getbulk("/workOrder/InspectionDetailsDoclist?podetailsId=" + inw_Recpt_HeaderId).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status==0);
            var tablebody = $("#P6DocUploadGrid tbody");
            $(tablebody).html("");//empty tbody
            //console.log(data);
            for (i = 0; i < data.length; i++) {
                let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"   
                   data-doctypename="${data[i].documentTypeName || ''}"
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
            <td>
                <div class="dropdown float-center">
                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="mdi mdi-dots-vertical"></i>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end">
                        <a href="javascript:void(0);" class="dropdown-item upload-link"
data-bs-toggle="modal"
                           data-bs-target="#doc-item"
                           data-filename="${data[i].fileName || ''}" 
                           data-doctypename="${data[i].documentTypeName || ''}" 
                           data-comments="${data[i].comments || ''}" 
                           data-fileextnname="${data[i].fileExtnName || ''}" 
                           data-documenttypeid="${data[i].documentTypeId || ''}" 
                           data-upload="1" 
                           data-deletiondate="${data[i].deletionDate || ''}" 
                           data-doclistid="${data[i].docListId || ''}" 
                           data-retdate="${data[i].retDate || ''}">Upload</a>
                        <a href="javascript:void(0);" class="dropdown-item edit-link"
 data-bs-toggle="modal"
                           data-bs-target="#doc-item"
                           data-filename="${data[i].fileName || ''}" 
                           data-doctypename="${data[i].documentTypeName || ''}" 
                           data-comments="${data[i].comments || ''}" 
                           data-fileextnname="${data[i].fileExtnName || ''}" 
                           data-documenttypeid="${data[i].documentTypeId || ''}" 
                           data-upload="2" 
                           data-deletiondate="${data[i].deletionDate || ''}" 
                           data-doclistid="${data[i].docListId || ''}" 
                           data-retdate="${data[i].retDate || ''}">Edit</a>
                        <a href="javascript:void(0);" class="dropdown-item delete-link" data-doclistid="${data[i].docListId || ''}"
                           onclick="DeleteDocList(this)">Delete</a>
                    </div>
                </div>
            </td>
        </tr>
    `);

                //            if (data[i].docCat === 1) {
                //                const approveTag = $(`
                //<a href="javascript:void(0);" 
                //   class="dropdown-item open-approve-modal" 
                //   data-bs-toggle="modal" 
                //   data-bs-target="#ApprovPopup"
                //   data-docid="${data[i].docListId}"
                //   data-doctype="${data[i].documentTypeName}"
                //   data-uploadedby="${data[i].uploadedBy}"
                //   data-uploadedon="${data[i].updatedOnStr}">
                //    ${data[i].docStatus || ''}
                //</a>`);
                //                rowHtml.find('td').eq(3).html(approveTag); // Replace {docStatus} placeholder
                //            } else {
                //                rowHtml.find('td').eq(3).html(''); // Clear {docStatus} if not applicable
                //            }

                if (data[i].docListId === 0) {
                    rowHtml.find('.edit-link').remove(); // Remove Edit link
                    rowHtml.find('.delete-link').remove(); // Remove Delete link
                }

                if (data[i].docListId !== 0) {
                    rowHtml.find('.upload-link').remove(); // Remove Upload link
                }

                if (data[i].mandatory === 'Y') {
                    rowHtml.find('.delete-link').remove(); // Remove Delete link for mandatory items
                }

                // Append the processed row to the table body
                $(tablebody).append(rowHtml);
            }

        }).catch((error) => {
            console.log(error);
        });
    } else {
        api.getbulk("/workOrder/InspectionDetailsDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status == 0);
            var tablebody = $("#P6DocUploadGrid tbody");
            $(tablebody).html("");//empty tbody
            //console.log(data);
            for (i = 0; i < data.length; i++) {
                let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}" 
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
            <td>
                <div class="dropdown float-center">
                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="mdi mdi-dots-vertical"></i>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end">
                        <a href="javascript:void(0);" class="dropdown-item upload-link"
data-bs-toggle="modal"
                           data-bs-target="#doc-item"
                           data-filename="${data[i].fileName || ''}" 
                           data-doctypename="${data[i].documentTypeName || ''}" 
                           data-comments="${data[i].comments || ''}" 
                           data-fileextnname="${data[i].fileExtnName || ''}" 
                           data-documenttypeid="${data[i].documentTypeId || ''}" 
                           data-upload="1" 
                           data-deletiondate="${data[i].deletionDate || ''}" 
                           data-doclistid="${data[i].docListId || ''}" 
                           data-retdate="${data[i].retDate || ''}">Upload</a>
                        <a href="javascript:void(0);" class="dropdown-item edit-link"
 data-bs-toggle="modal"
                           data-bs-target="#doc-item"
                           data-filename="${data[i].fileName || ''}" 
                           data-doctypename="${data[i].documentTypeName || ''}" 
                           data-comments="${data[i].comments || ''}" 
                           data-fileextnname="${data[i].fileExtnName || ''}" 
                           data-documenttypeid="${data[i].documentTypeId || ''}" 
                           data-upload="2" 
                           data-deletiondate="${data[i].deletionDate || ''}" 
                           data-doclistid="${data[i].docListId || ''}" 
                           data-retdate="${data[i].retDate || ''}">Edit</a>
                        <a href="javascript:void(0);" class="dropdown-item delete-link" data-doclistid="${data[i].docListId || ''}"
                           onclick="DeleteDocList(this)">Delete</a>
                    </div>
                </div>
            </td>
        </tr>
    `);

                //    if (data[i].docCat === 1) {
                //        const approveTag = $(`
                //<a href="javascript:void(0);" 
                //   class="dropdown-item open-approve-modal" 
                //   data-bs-toggle="modal" 
                //   data-bs-target="#ApprovPopup"
                //   data-docid="${data[i].docListId}"
                //   data-doctype="${data[i].documentTypeName}"
                //   data-uploadedby="${data[i].uploadedBy}"
                //   data-uploadedon="${data[i].updatedOnStr}">
                //    ${data[i].docStatus || ''}
                //</a>`);
                //        rowHtml.find('td').eq(3).html(approveTag); // Replace {docStatus} placeholder
                //    } else {
                //        rowHtml.find('td').eq(3).html(''); // Clear {docStatus} if not applicable
                //    }

                if (data[i].docListId === 0) {
                    rowHtml.find('.edit-link').remove(); // Remove Edit link
                    rowHtml.find('.delete-link').remove(); // Remove Delete link
                }

                if (data[i].docListId !== 0) {
                    rowHtml.find('.upload-link').remove(); // Remove Upload link
                }

                if (data[i].mandatory === 'Y') {
                    rowHtml.find('.delete-link').remove(); // Remove Delete link for mandatory items
                }

                // Append the processed row to the table body
                $(tablebody).append(rowHtml);
            }

        }).catch((error) => {
            console.log(error);
        });

    }
}
function loadPartDoc(inw_Recpt_HeaderId) {
    api.getbulk("/workOrder/InspectionDetailsDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
        //data = data.filter(item => item.status == 1 || item.status == 0);
        var tablebody = $("#P6partInspectPlanGrid tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}" 
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
        </tr>
    `);

            // Append the processed row to the table body
            $(tablebody).append(rowHtml);
        }

    }).catch((error) => {
        console.log(error);
    });
}
function loadVendorDoc(inw_Recpt_HeaderId) {
    api.getbulk("/workOrder/InwardDoclist?podetailsId=" + inw_Recpt_HeaderId).then((data) => {
        //data = data.filter(item => item.status == 1 || item.status == 0);
        var tablebody = $("#P6partInspectSuppGrid tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}" 
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
        </tr>
    `);

            // Append the processed row to the table body
            $(tablebody).append(rowHtml);
        }

    }).catch((error) => {
        console.log(error);
    });
}