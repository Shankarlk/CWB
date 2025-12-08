
function loadInvMis() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllInv_Mismatch_List").then((data) => {
        var tablebody = $("#InvMisGrid tbody");
        $(tablebody).html("");
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("InvMisGridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}

function loadInvMisFirst() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllInvMaster").then((data) => {
        var tablebody = $("#P5Grid tbody");
        $(tablebody).html("");
        data = data.filter(item => parseInt(item.qnty) == 0);
        if (data.length === 0) {
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
            $("#FirstInvMasterBtn").prop("disabled", true);
        } else {
            $("#FirstInvMasterBtn").prop("disabled", false);
        }
        $("#SFirstInvMasterCount").text(data.length);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P5GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}

function loadInvMasterLogs(invmasterid) {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllInv_Master_Log?invmasterid=" + parseInt(invmasterid)).then((data) => {
        var tablebody = $("#P7Grid tbody");
        $(tablebody).html("");
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            data[i].old_Value = parseInt(data[i].old_Value);
            $(tablebody).append(AppUtil.ProcessTemplateData("P7GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadInvTranLog(partno) {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllInvTranLog").then((data) => {
        var tablebody = $("#P2InvTranLogGrid tbody");
        $(tablebody).html("");
        if (partno !== undefined && partno !== null && partno !== "") {
            const search = partno.toLowerCase();   // convert input to lowercase

            data = data.filter(item =>
                (item.inputPartNo && item.inputPartNo.toLowerCase().startsWith(search)) ||
                (item.outPutPartNo && item.outPutPartNo.toLowerCase().startsWith(search))
            );
        }

        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            let item = data[i];

            // 🌟 NEW: Process the data before templating 🌟
            // Format Input Part Info
            item.inputPartInfo = formatPartInfo(
                item.inputPartNo,
                item.inPutRoutingName,
                item.inputOprNo
            );

            // Format Output Part Info
            item.outputPartInfo = formatPartInfo(
                item.outPutPartNo,
                item.outPutRoutingName,
                item.outputOprNo
            );

            // Append the row using the new processed properties
            $(tablebody).append(AppUtil.ProcessTemplateData("P2InvTranLogGridRow", item));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function formatPartInfo(partNo, routingName, oprNo) {
    // Collect all non-empty or non-null data points into an array
    const parts = [];

    if (partNo) {
        parts.push(partNo);
    }
    if (routingName) {
        parts.push(routingName);
    }
    if (oprNo) {
        parts.push(oprNo);
    }

    // 1. If there is data, join the parts with ' / '
    if (parts.length > 0) {
        return parts.join(' / ');
    }

    // 2. If there is no data at all, return a single hyphen '-'
    return '-';
}
function loadInvMaster() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllInvMaster").then((data) => {
        var tablebody = $("#P1InvMasterGrid tbody");
        $(tablebody).html("");
        data = data.filter(item => parseInt(item.qnty) > 0);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            let item = data[i];

            // 🌟 NEW LOGIC: Check for Routing/Operation data 🌟
            // A part is considered "without Routing/Opr No" if both fields are falsey (null, empty string, undefined).
            const isFinishedPart = (item.routName == "-" && item.opName == "");

            // Set conditional variables for the template
            if (isFinishedPart) {
                // If it's a finished part (no routing), render the history/cycle count links
                item.historyLink = AppUtil.ProcessTemplateData("P1HistoryLinkTemplate", item);
                item.cycleCountOption = AppUtil.ProcessTemplateData("P1CycleCountOptionTemplate", item);
            } else {
                // If it's a WIP part (has routing), render empty placeholder
                item.historyLink = '';
                item.cycleCountOption = '';
            }

            // Append the row using the new conditional variables
            $(tablebody).append(AppUtil.ProcessTemplateData("P1InvMasterGridRow", item));
        }
        // 1. Calculate the Total Qnty by Status
        const totalsByStatus = data.reduce((acc, item) => {
            const status = item.partStatus || 'Unknown Status'; // Use a default if Status is missing
            const qnty = parseInt(item.qnty) || 0;         // Ensure Qnty is a number

            if (acc[status]) {
                acc[status] += qnty;
            } else {
                acc[status] = qnty;
            }
            return acc;
        }, {});

        // 2. Convert the object into the desired array format for binding
        const TotalQntyByStatus = Object.keys(totalsByStatus).map(status => {
            return {
                'Status': status,
                'Qnty': totalsByStatus[status]
            };
        });

        // --- Start of your original code logic for the other table ---

        // Note: The table ID in the prompt is 'DeptRoleGrid', but you are targeting '#P1InvMasterGrid' in your code.
        // I will use '#DeptRoleGrid' to bind the new data, as it matches the HTML provided.
        var statusTableBody = $("#DeptRoleGrid tbody");
        $(statusTableBody).html(""); // Clear the table body

        if (TotalQntyByStatus.length === 0) {
            // Handle no records for the Status Table
            const noRecordsRow = `
            <tr>
                <td colspan="2" style="text-align: center; color: #888;">
                    <strong>No Status Totals Found</strong>
                </td>
            </tr>`;
            $(statusTableBody).append(noRecordsRow);
        } else {
            // 3. Bind the TotalQntyByStatus array to the 'DeptRoleGrid' table
            TotalQntyByStatus.forEach(item => {
                const row = `
                <tr>
                    <td>${item.Status}</td>
                    <td>${item.Qnty}</td>
                </tr>`;
                $(statusTableBody).append(row);
            });
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadCompanies() {
    var selElem = $('#P1SComp');
    selElem.html('');
    api.getbulk("/masters/companies").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].companyId + "'>" + data[i].companyName + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadPartStatus() {
    var selElem = $('#P1SPartStatuss');
    selElem.html('');
    var P1SPartType = $('#P1SPartType');
    P1SPartType.html('');
    api.getbulk("/masters/GetPartStatus").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        P1SPartType.append(div_data);
        div_data = "<option value='ManufacturedPart'>ManufacturedPart</option>";
        P1SPartType.append(div_data);
        div_data = "<option value='RawMaterial'>RawMaterial</option>";
        P1SPartType.append(div_data);
        div_data = "<option value='BOF'>BOF</option>";
        P1SPartType.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].status + "'>" + data[i].status + "</option>";
            selElem.append(div_data);
        }
    });
}
function calculateCount() {
    var P3CorrectedQnty = $("#P3CorrectedQnty").val();
    var P3Qnty = $("#P3Qnty").text();
    if (parseInt(P3Qnty) == parseInt(P3CorrectedQnty)) {
        alert("Message no change in values");
        $("#P3Save").prop("disabled", true);
    } else {
        $("#P3Save").prop("disabled", false);
    }
}

function loadCmp() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetChildManfWIP").then((data) => {
        var tablebody = $("#P22CmpGrid tbody");
        $(tablebody).html("");
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            if (data[i].inputPartNo === "") {
                continue;
            }
            $(tablebody).append(AppUtil.ProcessTemplateData("P22CmpGridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadCmpProcess(woId) {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetWIPFlowReport?woId=" + woId).then((data) => {
        var tablebody = $("#P22GridWo tbody");
        $(tablebody).html("");
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            if (data[i].endingOprNo === "Stores") {
                continue;
            }
            $(tablebody).append(AppUtil.ProcessTemplateData("P22GridWoRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadCmpProcessInvLog(woId, oprNo) {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetInventoryTransactionLog?woId=" + woId + "&stepId=" + oprNo).then((data) => {
        var tablebody = $("#P225Grid tbody");
        $(tablebody).html("");
        if (data.gridData.length === 0) {
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        let totalInput = data.header.totalInput;
        let totalBookout = data.header.totalBookout;
        let totalWip = data.header.totalWIP;
        let totalScrap = data.header.totalScrap;
        let totalRework = data.header.totalRework;
        for (i = 0; i < data.gridData.length; i++) {

            let row = data.gridData[i];
            //totalInput += Number(row.inputQnty || 0);
            //totalBookout += Number(row.bookoutQnty || 0);
            //totalWip += Number(row.wipQnty || 0);
            //totalScrap += Number(row.scrapQnty || 0);
            //totalRework += Number(row.wfrQnty || 0);

            if (row.bookoutQnty) {
                row.bookoutQnty = parseFloat(row.bookoutQnty).toFixed(2);
            }
            tablebody.append(AppUtil.ProcessTemplateData("P225GridRow", row));
        }
        const totalRow = `
            <tr id="P225GridTotalTr">
                <td colspan="3" class="text-end"><strong>Total</strong></td>
                <td><strong> ${totalInput}</strong></td>
                <td><strong>${totalBookout}</strong></td>
                <td><strong> ${totalWip}</strong></td>
                <td><strong> ${totalScrap}</strong></td>
                <td><strong> ${totalRework}</strong></td>
            </tr>
        `;

        tablebody.append(totalRow);
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadAssy() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAssemblyWOInProgress").then((data) => {
        var tablebody = $("#P223Grid tbody");
        $(tablebody).html("");
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P223GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadAssemProcess(woId) {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAssemblyBOMCoverage?woId=" + woId).then((data) => {
        var tablebody = $("#P224Grid tbody");
        $(tablebody).html("");
        var headerdata = data.header;
        $("#P24WoN").text(headerdata.woNumber);
        $("#P24As").text(headerdata.assyPartNo + " / " + headerdata.assyDesc);
        $("#P24Rout").text(headerdata.routingNo);
        $("#P224PQnty").text(headerdata.plannedWOQnty);
        $("#P224StDt").text(headerdata.planStartDate);
        $("#P224EndDts").text(headerdata.planEndDate);
        if (data.gridData.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        var assembly = 0;
        for (i = 0; i < data.gridData.length; i++) {
            data.gridData[i].woId = headerdata.woId;
            if (data.gridData[i].partType == "Assembly") {
                data.gridData[i].partType = "Sub - Assembly";
                assembly++;
            }
            $("#P224TotalAssemCov").val(assembly);
            var rowHtml = AppUtil.ProcessTemplateData("P224GridRow", data.gridData[i]);
            var $row = $(rowHtml);

            if (data.gridData[i].partType === "Sub - Assembly") {
                $row.find(".inv-log").remove();
            } else {
                $row.find(".bom-cov").remove();
            }

            $(tablebody).append($row);
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadAssemSubProcess(woId, partid) {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetSubAssemblyBOMCoverage?woId=" + woId + "&BomPartId=" + partid).then((data) => {
        var tablebody = $("#P2241Grid tbody");
        $(tablebody).html("");
        var headerdata = data.header;
        $("#P241WoN").text(headerdata.woNumber);
        $("#P241Rout").text(headerdata.routingNo);
        $("#P2241PQnty").text(headerdata.plannedWOQnty);
        $("#P2241StDt").text(headerdata.planStartDate);
        $("#P2241EndDts").text(headerdata.planEndDate);
        if (data.gridData.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        var assembly = 0;
        for (i = 0; i < data.gridData.length; i++) {
            data.gridData[i].woId = headerdata.woId;
            if (data.gridData[i].partType == "Assembly") {
                data.gridData[i].partType = "Sub - Assembly";
                assembly++;
            }
            $("#P2241TotalAssemCov").val(assembly);
            var rowHtml = AppUtil.ProcessTemplateData("P2241GridRow", data.gridData[i]);
            var $row = $(rowHtml);

            if (data.gridData[i].partType === "Sub - Assembly") {
                $row.find(".inv-log").remove();
            } else {
                $row.find(".bom-cov").remove();
            }

            $(tablebody).append($row);
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadAssemInvLog(woId) {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAssemblyPartInventoryLog?woId=" + woId).then((data) => {
        var tablebody = $("#P225AsemGrid tbody");
        $(tablebody).html("");
        var headerdata = data.header;
        var wono = $("#P24WoN").text();
        $("#P225WoNo").text(wono);
        $("#P225AFromloc").text(headerdata.fromLocation);
        $("#P225AToloc").text(headerdata.toLocation);
        $("#P225ATotalQnty").text(headerdata.totalTransactionQnty);
        if (data.gridData.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        let totalQnty = 0;
        for (i = 0; i < data.gridData.length; i++) {
            if (data.gridData[i].partType == "Assembly") {
                data.gridData[i].partType = "Sub - Assembly";
            }
            let q = Number(data.gridData[i].transactionQnty || 0);
            totalQnty += q;

            $(tablebody).append(
                AppUtil.ProcessTemplateData("P225AsemGridRow", data.gridData[i])
            );
        }
        const totalRow = `
            <tr id="P225Tr">
                <td colspan="3" class="text-end"><strong>Total</strong></td>
                <td><strong>${totalQnty}</strong></td>
            </tr>`;

        $(tablebody).append(totalRow);
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
$(document).ready(function () {
    loadInvMis();
    loadInvMisFirst();
    $('#Popup1').on('show.bs.modal', function (event) {
        loadInvMaster();
        loadPartStatus();
        loadCompanies();
        $("#DeptRoleGrid").hide();
    });
    $('#Popup2').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var partno = relatedTarget.data("partno");
        loadInvTranLog(partno);
        loadPartStatus();
        loadCompanies();
    });
    $('#Popup21').on('show.bs.modal', function (event) {
        loadCmp();
    });
    $('#Popup23').on('show.bs.modal', function (event) {
        loadAssy();
    });
    $('#Popup241').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup24').style.filter = 'none';
    });
    $('#Popup241').on('show.bs.modal', function (event) {
        document.getElementById('Popup24').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var woId = relatedTarget.data("woid");
        var partid = relatedTarget.data("partid");
        loadAssemSubProcess(woId, partid);
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        $("#P241As").text(partno + " / " + partdesc);
    });
    $('#Popup24').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup23').style.filter = 'none';
    });
    $('#Popup24').on('show.bs.modal', function (event) {
        document.getElementById('Popup23').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var woId = relatedTarget.data("woid");
        loadAssemProcess(woId)
    });
    $('#Popup25').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup24').style.filter = 'none';
        document.getElementById('Popup241').style.filter = 'none';
    });
    $('#Popup25').on('show.bs.modal', function (event) {
        document.getElementById('Popup24').style.filter = 'blur(5px)';
        document.getElementById('Popup241').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var woId = relatedTarget.data("woid");
        var percov = relatedTarget.data("percov");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        loadAssemInvLog(woId);
        $("#P225PNo").text(partno + " / " + partdesc);
        $("#P225APCov").text(percov);
    });
    $('#Popup225').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup22').style.filter = 'none';
    });
    $('#Popup225').on('show.bs.modal', function (event) {
        document.getElementById('Popup22').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var woId = relatedTarget.data("woid");
        var opid = relatedTarget.data("opid");
        var stop = relatedTarget.data("stop");
        var endop = relatedTarget.data("endop");
        var fromloc = relatedTarget.data("fromloc");
        var toloc = relatedTarget.data("toloc");
        var wonum = $("#P22Wo").text();
        var cmpart = $("#P22CPND").text();
        var inpt = $("#P22CInPND").text();
        var rout = $("#P22Routing").text();
        var woqnty = $("#P22PQ").text();
        var planstdt = $("#P22PStDt").text();
        var planenddt = $("#P22PEndDt").text();
        var inpcov = $("#P22InCov").text();
        var bocov = $("#P22Book").text();
        $("#P225Wo").text(wonum);
        $("#P225CPND").text(cmpart);
        $("#P225CInPND").text(inpt);
        $("#P225Routing").text(rout);
        $("#P225PQ").text(woqnty);
        $("#P225PStDt").text(planstdt);
        $("#P225PEndDt").text(planenddt);
        $("#P225InvCov").text(inpcov);
        $("#P225BookCov").text(bocov);
        $("#P225StOp").text(stop);
        $("#P225EndOp").text(endop);
        $("#P225FromLoc").text(fromloc);
        $("#P225ToLoc").text(toloc);
        loadCmpProcessInvLog(woId, opid);

    });
    $('#Popup22').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup21').style.filter = 'none';
    });
    $('#Popup22').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var woId = relatedTarget.data("woid");
        var wonum = relatedTarget.data("wonum");
        var cmpart = relatedTarget.data("cmpart");
        var cmpartdesc = relatedTarget.data("cmpartdesc");
        var inpt = relatedTarget.data("inpt");
        var inptd = relatedTarget.data("inptd");
        var rout = relatedTarget.data("rout");
        var woqnty = relatedTarget.data("woqnty");
        var planstdt = relatedTarget.data("planstdt");
        var planenddt = relatedTarget.data("planenddt");
        var inpcov = relatedTarget.data("inpcov");
        var bocov = relatedTarget.data("bocov");
        $("#P22Wo").text(wonum);
        $("#P22CPND").text(cmpart + " / " + cmpartdesc);
        $("#P22CInPND").text(inpt + " / " + inptd);
        $("#P22Routing").text(rout);
        $("#P22PQ").text(woqnty);
        $("#P22PStDt").text(planstdt);
        $("#P22PEndDt").text(planenddt);
        $("#P22InCov").text(inpcov);
        $("#P22Book").text(bocov);
        document.getElementById('Popup21').style.filter = 'blur(5px)';
        loadCmpProcess(woId);
    });
    $('#Popup7').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup1').style.filter = 'none';
    });
    $('#Popup7').on('show.bs.modal', function (event) {
        document.getElementById('Popup1').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var invmasterid = relatedTarget.data("invmasterid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var compname = relatedTarget.data("compname");
        var locname = relatedTarget.data("locname");
        $("#P7PartName").text(partno);
        $("#P7PartDesc").text(partdesc);
        $("#P7Comp").text(compname);
        $("#P7Loc").text(locname);
        loadInvMasterLogs(invmasterid);
    });
    $('#Popup3').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup1').style.filter = 'none';
        $("#P3Save").prop("disabled", false);
        var P3CorrectedQnty = document.getElementById('P3CorrectedQnty');
        P3CorrectedQnty.style.border = '';
        var newNamevalidate = document.getElementById('P3CorrectedReason');
        newNamevalidate.style.border = '';
    });
    $('#Popup3').on('show.bs.modal', function (event) {
        document.getElementById('Popup1').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var invmasterid = relatedTarget.data("invmasterid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var compname = relatedTarget.data("compname");
        var parttype = relatedTarget.data("parttype");
        var qnty = relatedTarget.data("qnty");
        var locname = relatedTarget.data("locname");
        var datestr = relatedTarget.data("datestr");
        $("#P3Date").text(datestr);
        $("#P3PartName").text(partno);
        $("#P3PartDesc").text(partdesc);
        $("#P3Comp").text(compname);
        $("#P3Qnty").text(qnty);
        $("#P3PartType").text(parttype);
        $("#P3Loc").text(locname);
        $("#P3InvMasterId").val(invmasterid);

    });
    $("#P3CorrectedQnty").on("input", calculateCount);
    $("#P3Save").on("click", function () {
        var P3CorrectedQnty = $("#P3CorrectedQnty").val();
        var P3Qnty = $("#P3Qnty").text();
        var P3CorrectedReason = $("#P3CorrectedReason").val();
        var P3InvMasterId = parseInt($("#P3InvMasterId").val());
        if (P3CorrectedQnty.length === 0) {
            var newNamevalidate = document.getElementById('P3CorrectedQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P3CorrectedQnty');
            newNamevalidate.style.border = '';
        }
        if (parseInt(P3Qnty) == parseInt(P3CorrectedQnty)) {
            alert("Message no change in values");
            return false;
        }
        if (P3CorrectedReason.length === 0) {
            var newNamevalidate = document.getElementById('P3CorrectedReason');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P3CorrectedReason');
            newNamevalidate.style.border = '';
        }
        var InvMasterrowData = {
            inventory_MasterId: P3InvMasterId,
            Part_NoId: 0,
            Routing_Id: 0,
            Inv_Trans_Log_Id: 0,
            Opr_No_Id: 0,
            Current_QntOnHand: parseInt(P3CorrectedQnty),
            Location_Id: 1,
            ReasonDesc: P3CorrectedReason
        };
        api.post("/WorkOrder/UpdateInvMasterCon", InvMasterrowData).then((data) => {
            $("Updated Inventory Master Record Succesfully");
            $("#Popup3").modal("hide");
            loadInvMaster();
        }).catch((error) => {
            console.log(error);
        });
    });
    $("#SP2PartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        var SP2IO = $("#SP2IO").val();
        if (SP2IO == "2") {
            $("#P2InvTranLogGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
            });
        } else {
            $("#P2InvTranLogGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
            });
        }
        var $tableBody = $("#P2InvTranLogGrid tbody");
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
    $('#Popup6').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup5').style.filter = 'none';
        $("#P3Save").prop("disabled", false);
        var P6Q = document.getElementById('P6Q');
        P6Q.style.border = '';
        var P6Loc = document.getElementById('P6Loc');
        P6Loc.style.border = '';
    });
    $('#Popup6').on('show.bs.modal', function (event) {
        document.getElementById('Popup5').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var invmasterid = relatedTarget.data("invmasterid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var compname = relatedTarget.data("compname");
        var parttype = relatedTarget.data("parttype");
        $("#P6CPTPD").val(compname + " / " + partno + " / " + partdesc);
        $("#P6InvMastId").val(invmasterid);
    });
    $("#P6Save").on("click", function () {
        var P6Q = $("#P6Q").val();
        var P6Loc = $("#P6Loc").val();
        var P6InvMastId = parseInt($("#P6InvMastId").val());
        if (P6Q.length === 0 || isNaN(P6Q) || parseInt(P6Q) <= 0) {
            var newNamevalidate = document.getElementById('P6Q');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P6Q');
            newNamevalidate.style.border = '';
        }
        if (parseInt(P6Loc) === 0) {
            var newNamevalidate = document.getElementById('P6Loc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P6Loc');
            newNamevalidate.style.border = '';
        }
        var InvMasterrowData = {
            inventory_MasterId: P6InvMastId,
            Part_NoId: 0,
            Routing_Id: 0,
            Inv_Trans_Log_Id: 0,
            Opr_No_Id: 0,
            Current_QntOnHand: parseInt(P6Q),
            Location_Id: P6Loc,
            ReasonDesc: "First Time Inventory Master Update"
        };
        api.post("/WorkOrder/UpdateInvMasterFirst", InvMasterrowData).then((data) => {
            $("#Popup6").modal("hide");
            loadInvMisFirst();
        }).catch((error) => {
            console.log(error);
        });
    });
    $("#SPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#InvMisGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#InvMisGrid tbody");
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
    $("#SPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#InvMisGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#InvMisGrid tbody");
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
    $("#SPartLoc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#InvMisGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#InvMisGrid tbody");
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
    $("#P1SPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P1InvMasterGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P1InvMasterGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
            $("#DeptRoleGrid").hide();
        } else {
            $tableBody.find(".norecordsfound").remove();
            if (value == "") {
                $("#DeptRoleGrid").hide();
            } else {
                $("#DeptRoleGrid").show();
            }
            // 1. Read rows from P1InvMasterGrid and accumulate totals by Status
            const totalsByStatus = {};

            $("#P1InvMasterGrid tbody tr:visible").each(function () {
                const qnty = parseInt($(this).find("td:eq(4)").text().trim()) || 0;
                const status = $(this).find("td:eq(5)").text().trim() || "Unknown Status";

                if (totalsByStatus[status])
                    totalsByStatus[status] += qnty;
                else
                    totalsByStatus[status] = qnty;
            });

            // 2. Convert result to array
            const TotalQntyByStatus = Object.keys(totalsByStatus).map(s => ({
                Status: s,
                Qnty: totalsByStatus[s]
            }));

            // 3. Bind to DeptRoleGrid
            const statusTableBody = $("#DeptRoleGrid tbody");
            statusTableBody.html("");

            if (TotalQntyByStatus.length === 0) {
                statusTableBody.append(`
        <tr>
            <td colspan="2" style="text-align:center;color:#888;">
                <strong>No Status Totals Found</strong>
            </td>
        </tr>
    `);
            } else {
                TotalQntyByStatus.forEach(item => {
                    statusTableBody.append(`
            <tr>
                <td>${item.Status}</td>
                <td>${item.Qnty}</td>
            </tr>
        `);
                });
            }

        }
    });
    $("#P1SPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P1InvMasterGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P1InvMasterGrid tbody");
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
    $("#P1SLoc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P1InvMasterGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[6]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P1InvMasterGrid tbody");
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
    $("#P1SPartType").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#P1SPartType option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#P1InvMasterGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selectedText) > -1)
            });
            var $tableBody = $("#P1InvMasterGrid tbody");
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
            $("#P1InvMasterGrid tbody tr").show();
            var $tableBody = $("#P1InvMasterGrid tbody");
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
    $("#P1SComp").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#P1SComp option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#P1InvMasterGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(selectedText) > -1)
            });
            var $tableBody = $("#P1InvMasterGrid tbody");
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
            $("#P1InvMasterGrid tbody tr").show();
            var $tableBody = $("#P1InvMasterGrid tbody");
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
    $("#P1SPartStatus").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#P1SPartStatus option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#P1InvMasterGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selectedText) > -1)
            });
            var $tableBody = $("#P1InvMasterGrid tbody");
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
            $("#P1InvMasterGrid tbody tr").show();
            var $tableBody = $("#P1InvMasterGrid tbody");
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
    $("#P22PN").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P22CmpGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P22CmpGrid tbody");
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
    $("#P22PD").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P22CmpGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P22CmpGrid tbody");
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
    $("#P223APN").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P223Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P223Grid tbody");
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
    $("#P223APD").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P223Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P223Grid tbody");
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
});

function DelInvmist(companyID) {
    let confirmval = confirm("Are your sure you want to delete this?", "Yes", "No");
    if (confirmval) {
        api.get("/WorkOrder/DeleteInvMismatch?itemMasterDocListId=" + companyID).then((data) => {
            LoadCompanies();
        }).catch((error) => {

        });
    }
};