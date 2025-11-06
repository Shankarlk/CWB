let documenttype = "Part Drawing";
function loadCustomers(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    compSelect.empty();
    customers = {};
    ////debugger;
    var div_data = "<option value='0'>-Select-</option>";
    compSelect.append(div_data);
    api.get("/masters/Companies").then((data) => {
        customers = data;
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].companyName + "'>" +
                data[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}
function loadEditParts(loca) {
    $("#preloaderblurred").show();
    var tablebody = $("#grid1 tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/MaterialAvailability/MatlAvlOfParts?location=" + loca).then((data) => {
        data = data.filter(i => i.companyName == loca);
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
        let allrwkQntyEmpty = data.every(p => !p.rwkQnty || p.rwkQnty.toString().trim() === "");
        for (i = 0; i < data.length; i++) {
            if (data[i].routingName == null) {
                data[i].routingName = ""
            }
            if (data[i].oprNo == null) {
                data[i].oprNo = ""
            }
            //if (!(data[i]['status'] == strActive))
            //    continue;
            var tBody = AppUtil.ProcessTemplateData("grid1Row", data[i]);
            $(tablebody).append(tBody);
            if (allrwkQntyEmpty) {
                $("#G2WfReworkTh, .G2WfReworkTh").hide();
            } else {
                $("#G2WfReworkTh, .G2WfReworkTh").show();
            }
            //console.log(tBody);
        }
        $("#grid1 tbody tr").each(function () {
            let rowData = data[$(this).index()];
            if (rowData.ncAvl !== "Y") {
                $(this).find("td .dropdown").remove();
            }
        });
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}


$(function () {
    loadCustomers('searchLoc');
    loadEditParts();

    $("#ShowBomlbl").hide();
    $("#ShowBomChk").hide();
    $("#searchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#grid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#grid1 tbody");
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
    $("#searchPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#grid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#grid1 tbody");
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
    $("#searchLoc").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#grid1 tbody tr").show();
            return;
        }
        var selvallowc = selectedValue;
        loadEditParts(selvallowc);
    });
    $("#searchPartType").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#grid1 tbody tr").show();
            return;
        }
        var selvallowc = selectedValue.toLowerCase();
        $("#grid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(selvallowc) > -1)
        });
        var $tableBody = $("#grid1 tbody");
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
    $('#NcaawaitPopup').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        loadNCLog(partno + " / " + partdesc);
    });
});
function ShowGrid2(element) {
    var relatedTarget = $(element);
    var partid = relatedTarget.data("partid");
    var partno = relatedTarget.data("partno");
    var partdesc = relatedTarget.data("partdesc");
    var parttype = relatedTarget.data("parttype");
    $("#spanPartNo").text(partno);
    $("#spanPartDesc").text(partdesc);
    loadPartsGrid2(partid, parttype);
}
function loadPartsGrid2(partid, parttype) {
    var tablebody = $("#grid2 tbody");
    $(tablebody).html(""); // empty tbody

    api.getbulk("/MaterialAvailability/StockAvlOfParts?partid=" + partid + "&parttype=" + parttype).then((data) => {

        let allRoutingEmpty = data.every(p => !p.routingName || p.routingName.trim() === "");
        let allOprNoEmpty = data.every(p => !p.oprNo || p.oprNo.toString().trim() === "");
        let allrwkQntyEmpty = data.every(p => !p.rwkQnty || p.rwkQnty.toString().trim() === "");

        // Hide columns if needed
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
        for (let i = 0; i < data.length; i++) {
            let part = data[i];

            let tBody = AppUtil.ProcessTemplateData("grid2Row", part);

            $(tablebody).append(tBody);
            if (allRoutingEmpty) {
                $("#G2RoutingTh, .G2RoutingTh").hide();
            } else {
                $("#G2RoutingTh, .G2RoutingTh").show();
            }
            if (allOprNoEmpty) {
                $("#G2OpNoTh, .G2OpNoTh").hide();
            } else {
                $("#G2OpNoTh, .G2OpNoTh").show();
            }
            if (allrwkQntyEmpty) {
                $("#G2WfReworkTh, .G2WfReworkTh").hide();
            } else {
                $("#G2WfReworkTh, .G2WfReworkTh").show();
            }
        }
        $("#grid2 tbody tr").each(function () {
            let rowData = data[$(this).index()];
            if (rowData.ncAvl !== "Y") {
                $(this).find("td .dropdown").remove();
            }
        });



    }).catch((error) => {
        console.error(error);
    });
}

function loadNCLog(partname) {
    var tablebody = $("#NcGrid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/workOrder/GetAllNcLog").then((data) => {
        data = data.filter(item => item.nC_Log_status_Id === 1 && item.inw_Recpt_Part_No_Name === partname);

        data.forEach((ncItem) => {
            const nclogId = ncItem.insp_Outcome_Details_Id;

            Promise.all([
                api.getbulk("/workOrder/GetAllCont_RCA_CA_log"),
                api.getbulk("/workOrder/GetAllNC_Decision_Log")
            ]).then(([rcaData, decisionData]) => {
                let dropdownHtml = '';

                const rcdata = rcaData.filter(item => item.ncLogId === parseInt(nclogId));
                const containmentActionLength = rcdata[0]?.containment_Action?.length || 0;
                const statusId = rcdata[0]?.cont_RCA_CA_Status_Id;

                const decData = decisionData.filter(item => item.ncLogId === parseInt(nclogId));
                const lvl1Decision = decData[0]?.nC_Disp_Deci_Lvl1_Id;

                // Main conditional logic
                if (containmentActionLength > 0 && statusId === 3) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item descision-level"
                       data-bs-toggle="modal"
                       data-balno="${ncItem.balloon_No}"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-divhide="Y"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-headid="${ncItem.inw_Recpt_Header_Id}" data-inspid="${ncItem.inw_Insp_Log_Id}"
                       data-bs-target="#popup10">Decision (Level 1)</a>`;
                } else if (containmentActionLength > 0 && statusId === 4) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item"
                       data-bs-toggle="modal"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-balno="${ncItem.balloon_No}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-divhide="Y"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-bs-target="#popup14">Upload RCA / CA</a>`;
                } else if (containmentActionLength > 0) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item"
                       data-bs-toggle="modal"
                       data-divhide="N"
                       data-balno="${ncItem.balloon_No}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-baldesc="${ncItem.balloon_No_Dir}"
                       data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-bs-target="#popup14">Check RCA / CA</a>`;
                } else {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item"
                       data-bs-toggle="modal"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-balno="${ncItem.balloon_No}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-divhide="Y"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-bs-target="#popup14">Upload RCA / CA</a>`;
                }

                // ✅ Show Approval (Level 2) if Lvl1 decision is 3 or 4
                if (lvl1Decision === 3 || lvl1Decision === 4) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item approval-level"
                       data-bs-toggle="modal"
                       data-balno="${ncItem.balloon_No}"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-headid="${ncItem.inw_Recpt_Header_Id}" data-inspid="${ncItem.inw_Insp_Log_Id}"
                       data-bs-target="#popup10">Approval (Level 2)</a>`;
                }

                const rowHtml = `
                <tr>
                    <td>${ncItem.nC_Tracking_No}</td>
                    <td>${ncItem.ncDateStr}</td>
                    <td>${ncItem.locationName}</td>
                    <td>${ncItem.inw_Recpt_Part_No_Name}</td>
                    <td>${ncItem.feature_Descrip}</td>
                    <td>${ncItem.nC_Descrip}</td>
                    <td>${ncItem.partType}</td>
                    <td>${ncItem.nC_Qnty}</td>
                    <td>${ncItem.unit}</td>
                    <td>${ncItem.rcaStatus}</td>
                    <td>${ncItem.wfCustFeedBack}</td>
                    <td>${ncItem.lvldesc}</td>
                    <td>${ncItem.lvlAppro}</td>
                    <td>${ncItem.noOfDays}</td>
                    <td>
                    </td>
                </tr>
            `;

                $(tablebody).append(rowHtml);

            }).catch((error) => {
                console.error("Bulk data fetch error:", error);
            });
        });

        //loadSelectNCDispDecision();
    }).catch((error) => {
        console.error("NC Log fetch error:", error);
    });



}