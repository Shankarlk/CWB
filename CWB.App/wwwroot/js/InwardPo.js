let archive = 0;
let poQnty = 0;
let poqntytodayrecd = 0;
let PPartType = "";
let partNo = "";
function loadPO() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllPodetails").then((data) => {
        data = data.filter(item => item.status >= 2);
        var tablebody = $("#PoGrid1 tbody");
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
        for (let i = 0; i < data.length; i++) {
            let rowData = data[i];

            // Determine the correct popup target based on partType
            let popupTarget = (rowData.partType === "SubCon") ? "#popupInward2" : "#popupInward";

            // Process the template
            let processedTemplate = AppUtil.ProcessTemplateData("POGrid1Row", rowData);

            // Convert it into a jQuery object to modify attributes
            let rowElement = $(processedTemplate);

            // Update the `data-bs-target` for Edit and Inward links
            rowElement.find(".dropdown-item[data-edit='{0}']").attr("data-bs-target", popupTarget);
            rowElement.find(".dropdown-item[data-edit='{1}']").attr("data-bs-target", popupTarget);

            // Append the modified row to the table body
            $(tablebody).append(rowElement);
        }

        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function calculateTotals() {
    let ourCountTotal = 0;
    let dcCountTotal = 0;

    // Loop through table rows and sum up the values
    document.querySelectorAll("#popupInwardSupplierGrid tbody tr").forEach(row => {
        const tds = row.querySelectorAll("td");
        if (tds.length >= 3) {
           
                let ourText = tds[1].innerText.trim();
            let dcText = tds[2].innerText.trim();

            let ourCount = isNaN(parseFloat(ourText)) ? 0 : parseFloat(ourText);// Assuming "Our Count" is in the 2nd column
            let dcCount = isNaN(parseFloat(dcText)) ? 0 : parseFloat(dcText);// Assuming "Supplier DC Count" is in the 3rd column
          
            ourCountTotal += ourCount;
            dcCountTotal += dcCount;
        }
    });

    // Display totals in footer
    document.getElementById("ourCountVM").innerText = ourCountTotal;
    document.getElementById("popupInwardPlanRecpt").value = ourCountTotal;
    document.getElementById("P4SubConPlanDate").value = ourCountTotal;
    document.getElementById("P4SubConActDate").value = ourCountTotal;
    document.getElementById("popupInwardActlDate").value = ourCountTotal;
    $("#baltorectd").text(ourCountTotal);
    poqntytodayrecd = ourCountTotal;
    document.getElementById("suppCountVM").innerText = dcCountTotal;
}
function loadInwardDetails(inwheaderid) {
    api.getbulk("/WorkOrder/GetAllInw_Recpt_Details").then((data) => {
        data = data.filter(item => item.inw_Recpt_Header_Id === parseInt(inwheaderid));
        var tablebody = $("#popupInwardSupplierGrid tbody");
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P4inwardGridRow", data[i]));
            calculateTotals();
        }
    }).catch((error) => {
    });
}
function loadSuppliers() {
    var selElem = $('#searchPoSupplier');
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
function loadCondition2() {
    var selElem = $('#P5Condition');
    selElem.html('');
    api.getbulk("/WorkOrder/GetAllInward_Condn_list").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        //data = data.filter(item => item.applicability == "All");
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].inward_Condn_listId + "'>" + data[i].inward_Condn_desc + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadCondition() {
    var selElem = $('#P5Condition');
    selElem.html('');
    api.getbulk("/WorkOrder/GetAllInward_Condn_list").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        data = data.filter(item => item.applicability == "All");
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].inward_Condn_listId + "'>" + data[i].inward_Condn_desc + "</option>";
            selElem.append(div_data);
        }
    });
}
async function showPopup() {
    const popup = document.querySelector('#popup4PoLineData');

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

$(document).ready(function () {
    loadSuppliers();
    loadPO();
    $("#searchPoNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#PoGrid1 tbody");
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
    $("#searchPoSupplier").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#searchPoSupplier option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selectedText) > -1)
            });
            var $tableBody = $("#PoGrid1 tbody");
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
            $("#PoGrid1 tbody tr").show();
            var $tableBody = $("#PoGrid1 tbody");
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
    $("#searchPartNo").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#searchPartNo option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(selectedText) > -1)
            });
            var $tableBody = $("#PoGrid1 tbody");
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
            $("#PoGrid1 tbody tr").show();
            var $tableBody = $("#PoGrid1 tbody");
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
    $("#popup4Capture").on("click", function () {
        $("#popup5").modal("show");
    });
    $("#ExitWarningBtn").on("click", function () {
        $("#popup5").modal("hide");
        $("#warning").modal("hide");
        $("#popupInward").modal("hide");
    });
    $("#EP7Exit").on("click", function () {
        $("#popup5").modal("hide");
        $("#NotUploaded").modal("hide");
        $("#popupInward").modal("hide");
    });
    $("#EP7Return").on("click", function () {
        $("#popup5").modal("hide");
        $("#NotUploaded").modal("hide");
    });
    $("#popupInwardClose").on("click", function () {
        var popupInwardHeaderId = parseInt($("#popupInwardHeaderId").val());
        var popupBalLine = parseInt($("#popupBalLine").val());
        var popupInwNoLine = parseInt($("#popupInwNoLine").val());
        var popupInwardDcDate = new Date(Date.parse($("#popupInwardDcDate").val()));
        var popupInwardDcref = $("#popupInwardDcref").val();
        var popupInwardInvRef = $("#popupInwardInvRef").val();
        if (popupBalLine.length > 0 || popupInwNoLine.length > 0 || popupInwardDcDate.length > 0 || popupInwardDcref.length > 0 || popupInwardInvRef.length > 0) {
            if (isNaN(popupInwardHeaderId)) {
                $("#warning").modal("show");
            } else {

                var row = $("#popupInwardDocGrid tbody tr");
                var mandatory = row.find("td:nth-child(2)").text().trim();
                var comment = row.find("td:nth-child(3)").text().trim();
                if (mandatory == "Y") {
                    if (comment.length <= 0) {
                        $("#NotUploaded").modal("show");
                    } else {
                        $("#popupInward").modal("hide");
                    }
                } else {
                    $("#popupInward").modal("hide");
                }
            }
        } else {
            $("#popupInward").modal("hide");
        }
    });
    $("#P5BtnClose").on("click", function () {
        var popupInwardHeaderId = parseInt($("#P5InwDetailsId").val());
        var P5Condition = parseInt($("#P5Condition").val());
        var P5OurCount = parseInt($("#P5OurCount").val());
        var P5SuppCount = parseInt($("#P5SuppCount").val());
        var P5Comment = $("#P5Comment").val();
        if (P5Condition > 0 || isNaN(P5OurCount) || P5OurCount > 0 || isNaN(P5OurCount) ||  P5SuppCount > 0 || P5Comment.length > 0 ) {
            if (isNaN(popupInwardHeaderId)) {
                $("#warning").modal("show");
            } else {
                $("#popup5").modal("hide");
            }
        } else {
            $("#popup5").modal("hide");
        }
    });
    $("#Error3Edit").on("click", function () {
        $("#ErrorMessage3").modal("hide");
    });
    $("#Error1Edit").on("click", function () {
        $("#ErrorMessage1").modal("hide");
    });
    $("#Error2WithOutExit").on("click", function () {
        $("#ErrorMessage2").modal("hide");
        $("#popup5").modal("hide");
        $("#popup4PoLineData").modal("hide");
        $("#popupInward").modal("hide");
    });
    $("#Error2Edit").on("click", function () {
        $("#ErrorMessage2").modal("hide");
    });
    $("#Error3WithOutExit").on("click", function () {
        $("#ErrorMessage3").modal("hide");
        $("#popup4PoLineData").modal("hide");
        $("#popup5").modal("hide");
        $("#popupInward").modal("hide");
    });
    $("#Error3Exit").on("click", function () {
        $("#ErrorMessage3").modal("hide");
        $("#popup4PoLineData").modal("hide");
        $("#popup5").modal("hide");
        $("#popupInward").modal("hide");
    });
    $("#P4BtnClose").on("click", function () {
        $("#popup4PoLineData").modal("hide");
    });
    $("#Error1WithOutExit").on("click", function () {
        $("#ErrorMessage1").modal("hide");
        $("#popup4PoLineData").modal("hide");
        $("#popup5").modal("hide");
        $("#popupInward").modal("hide");
    });
    $("#popup4PrintCapture").on("click", function () {
        $("#popup8").modal("show");
        var partno = $("#P4SpanPartNo").text();
        var supp = $("#P4SpanSupp").text();
        $("#P8SuppSpan").text(supp);
        var qnty = $("#ourCountVM").text();
        $("#P8With").val(qnty);
        $("#P8PartNoSpan").text(partno);
        $("#P8RoutingSpan").hide();
    });
    $("#SaveNEsca").on("click", function () {
        var E2Com = $("#E2Com").val();
        var baltorectd22B = $("#baltorectd22B").text();
        var alRecdC = $("#alRecdC").text();
        var baltorectd2D = $("#baltorectd2D").text();
        var cdb = parseInt(alRecdC) + parseInt(baltorectd2D) - parseInt(baltorectd22B);
        var popupPoPartId = parseInt($("#popupPoPartId").val());
        if (E2Com.length === 0) {
            var newNamevalidate = document.getElementById('E2Com');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('E2Com');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            inv_Mismatch_ListId: 0,
            calling_UI_ID: 90,
            pO_Ref: parseInt($("#popupPoHeaderId").val()),
            part_No: parseInt(popupPoPartId),
            mismatch_Qnty: parseInt(cdb),
            report_date: new Date().toISOString(),
            resolution_Comments: ".",
            resolved: 'N',
            mismatch_Comments: E2Com,
            mismatch_Status: "More",
        };
        $.ajax({
            type: "POST",
            url: '/workOrder/PostInv_Mismatch_List',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(rowData),
            dataType: "json",
            success: function (result) {
                $("#ErrorMessage2").modal("hide");
                $("#popup5").modal("hide");
                $("#popup4PoLineData").modal("hide");
                $("#popupInward").modal("hide");
            }
        });
    });
    $("#popup4LineComplete").secureClick( function () {
        var ourCountVM = $("#ourCountVM").text();
        var suppCountVM = $("#suppCountVM").text();
        var baltorectd22B = $("#baltorectd22B").text();
        var alRecdC = $("#alRecdC").text();
        var baltorectd2D = $("#baltorectd2D").text();
        var totalPOQnty = 0;
        var subtotalPOQnty = 0;
        var bCD = parseInt(baltorectd22B) - parseInt(alRecdC) - parseInt(baltorectd2D);
        $("#popupBalanceItemGrid tbody tr").each(function () {
            var poQnty = $(this).find("td:nth-child(2)").text().trim();
            var subpoQnty = $(this).find("td:nth-child(5)").text().trim();
            totalPOQnty += parseInt(poQnty) || 0;
            subtotalPOQnty += parseInt(subpoQnty) || 0;
        });
        if (parseInt(suppCountVM) != parseInt(ourCountVM)) {
            $("#ErrorMessage1").modal("show");
        } else if (parseInt(suppCountVM) > parseInt(baltorectd22B) - parseInt(alRecdC) - parseInt(baltorectd2D)) {
            if (PPartType == "SubCon") {
                $("#ErrorMessage2").modal("show");
                $("#ErSp2Sent").text(suppCountVM);
                $("#ErSp2To").text(subtotalPOQnty);
            } else {
                $("#ErSp3Sent").text(suppCountVM);
                $("#ErSp3To").text(totalPOQnty);
                $("#ErrorMessage3").modal("show");
            }
        } else {

            var selectedRowsData = {};
            var temprowdata = {};
                var rowData = {
                    poDetailsId: parseInt($("#popupPoHeaderId").val())
                };
                temprowdata[rowData.partId] = rowData;
            

            selectedRowsData = Object.values(temprowdata);
            if (selectedRowsData.length > 0) {
                return $.ajax({
                    type: "POST",
                    url: '/WorkOrder/UpdateInwardPOdetails',
                    contentType: "application/json; charset=utf-8",
                    headers: { 'Content-Type': 'application/json' },
                    data: JSON.stringify(selectedRowsData),
                    dataType: "json",
                    success: function (result) {
                        if (parseInt(suppCountVM) < parseInt(totalPOQnty)) {
                            $("#popupInwardPoStatusVM").val("Patically Recived");
                        } else {
                            $("#popupInwardPoStatusVM").val("Complete");
                        }
                        $("#P4MessageBox").text("Inwarding Complete");
                    }
                });
            } else {
                alert("Please select at least one material");
            }
        }
    });
    $('#ChkNcLabel').change(function () {
        if ($(this).is(':checked')) {
            $("#popupInwardExitBtn").prop("disabled", false);
        } else {
            $("#popupInwardExitBtn").prop("disabled", true);
        }
    });
    $("#P5Save").secureClick( function () {
        var P5Condition = parseInt($("#P5Condition").val());
        var P5OurCount = parseInt($("#P5OurCount").val());
        var P5SuppCount = parseInt($("#P5SuppCount").val());
        var P5Comment = $("#P5Comment").val();
        var P5InwDetailsId = parseInt($("#P5InwDetailsId").val());
        var popupPoPartId = parseInt($("#popupPoPartId").val());
        var popupInwardHeaderId = parseInt($("#popupInwardHeaderId").val());
        //const currentDate = $('#P5CurrentDate').val();
        //const [month, day, year] = currentDate.split('-');
        const rstDt = new Date();
        const restrictDt = new Date();
        var formattedDate;
        var formattedDate2;
        if (P5Condition === 0 || isNaN(P5Condition)) {
            var newNamevalidate = document.getElementById('P5Condition');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P5Condition');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P5OurCount) || P5OurCount === 0) {
            var newNamevalidate = document.getElementById('P5OurCount');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P5OurCount');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P5SuppCount) || P5SuppCount === 0) {
            var newNamevalidate = document.getElementById('P5SuppCount');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P5SuppCount');
            newNamevalidate.style.border = '';
        }
        if (P5Comment.length === 0) {
            var newNamevalidate = document.getElementById('P5Comment');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P5Comment');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P5InwDetailsId)) {
            P5InwDetailsId = 0;
        }
        var rowData = {
            inw_Recpt_DetailsId: parseInt(P5InwDetailsId),
            inw_Recpt_Header_Id: parseInt(popupInwardHeaderId),
            inw_Recpt_Part_No_Id: parseInt(popupPoPartId),
            our_count: parseInt(P5OurCount),
            vendor_DC_count: P5SuppCount,
            inward_Condition: P5Condition,
            comment: P5Comment
        };
        return $.ajax({
            type: "POST",
            url: '/workOrder/PostInw_Recpt_Details',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(rowData),
            dataType: "json",
            success: function (result) {
                var inw_Recpt_DetailsId = result.inw_Recpt_DetailsId;
                $("#P5InwDetailsId").val(inw_Recpt_DetailsId);
                alert("Inward Details Data Entered Successfully.");
                $("#popup5").modal("hide");
                loadInwardDetails(popupInwardHeaderId);
            }
        });
    });
    $("#popupInward2Save").on("click", function () {
        var popupInwardHeaderId = parseInt($("#popupInwardHeaderId").val());
        var popupPoHeaderId = parseInt($("#popupPoHeaderId").val());
        var popupBalLine = parseInt($("#popup2BalLine").val());
        var popupInwNoLine = parseInt($("#popupInw2NoLine").val());
        var popupInwardDcDate = new Date(Date.parse($("#popupInward2DcDate").val()));
        var popupInwardDcref = $("#popupInward2Dcref").val();
        var popupInwardInvRef = $("#popupInward2InvRef").val();
        var popupInvDate = new Date(Date.parse($("#popupInward2InvDate").val()));
        //const currentDate = $('#P5CurrentDate').val();
        //const [month, day, year] = currentDate.split('-');
        const rstDt = new Date();
        const restrictDt = new Date();
        restrictDt.setHours(0, 0, 0, 0);
        var formattedDate;
        var formattedDate2;
        if (popupInwardDcref.length === 0) {
            var newNamevalidate = document.getElementById('popupInward2Dcref');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('popupInward2Dcref');
            newNamevalidate.style.border = '';
        }
        if (isNaN(popupInwardDcDate.getTime())) {
            var newNamevalidate = document.getElementById('popupInward2DcDate');
            newNamevalidate.style.border = '2px solid red';
            return false;

            // or display an error message to the user
        } else if (popupInwardDcDate <= restrictDt) {
            var newNamevalidate = document.getElementById('popupInward2DcDate');
            newNamevalidate.style.border = '2px solid red';
            alert("Supplier DC Date should be less than Today's Date");
            return false;
            // or display an error message to the user
        }
        else {
            formattedDate = popupInwardDcDate.toISOString();
            var newNamevalidate = document.getElementById('popupInward2DcDate');
            newNamevalidate.style.border = '';
        }
        if (popupInwardInvRef.length === 0) {
            var newNamevalidate = document.getElementById('popupInward2InvRef');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('popupInward2InvRef');
            newNamevalidate.style.border = '';
        }   
        if (isNaN(popupInvDate.getTime())) {
            var newNamevalidate = document.getElementById('popupInvDate');
            newNamevalidate.style.border = '2px solid red';
            return false;

            // or display an error message to the user
        } else if (popupInvDate <= restrictDt) {
            var newNamevalidate = document.getElementById('popupInvDate');
            newNamevalidate.style.border = '2px solid red';
            alert("Supplier Inv Date should be less than or Equal to Today's Date");
            return false;
            // or display an error message to the user
        }
        else {
            formattedDate2 = popupInvDate.toISOString();
        }
        if (popupInwNoLine < popupBalLine) {
            alert("Please check the No of Line Items");
            return false;
        }
        if (isNaN(popupInwardHeaderId)) {
            popupInwardHeaderId = 0;
        }
        var rowData = {
            inw_Recpt_HeaderId: parseInt(popupInwardHeaderId),
            poHeaderId: parseInt(popupPoHeaderId),
            supplier_Dc_Ref: popupInwardDcref,
            Supplier_Dc_Date: formattedDate,
            Supplier_Inv_ref: popupInwardInvRef,
            Supplier_Inv_date: formattedDate2,
            Inw_Date_time: formattedDate
        };
        $.ajax({
            type: "POST",
            url: '/workOrder/PostInw_Recpt_Header',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(rowData),
            dataType: "json",
            success: function (result) {
                var inw_Recpt_HeaderId = result.inw_Recpt_HeaderId;
                $("#popupInwardHeaderId").val(inw_Recpt_HeaderId);
                //var newNamevalidate = document.getElementById('P5RetentionDate');
                //newNamevalidate.style.border = '';
                //loadDocList();
                //$('#RetentionDate').modal('hide');
                alert("Inward Header Data Entered Successfully.");
            }
        });
    });
    $("#popupInwardSaveWo").secureClick( function () {
        var popupInwardHeaderId = parseInt($("#popupInwardHeaderId").val());
        var popupPoHeaderId = parseInt($("#popupPoHeaderId").val());
        var popupBalLine = parseInt($("#popupBalLine").val());
        var popupInwNoLine = parseInt($("#popupInwNoLine").val());
        var popupInwardDcDate = new Date(Date.parse($("#popupInwardDcDate").val()));
        var popupInwardDcref = $("#popupInwardDcref").val();
        var popupInwardInvRef = $("#popupInwardInvRef").val();
        var popupInvDate = new Date(Date.parse($("#popupInvDate").val()));
        //const currentDate = $('#P5CurrentDate').val();
        //const [month, day, year] = currentDate.split('-');
        const rstDt = new Date();
        const restrictDt = new Date();
        restrictDt.setHours(0, 0, 0, 0);
        var formattedDate;
        var formattedDate2;
        if (popupInwardDcref.length === 0) {
            var newNamevalidate = document.getElementById('popupInwardDcref');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('popupInwardDcref');
            newNamevalidate.style.border = '';
        }
        if (isNaN(popupInwardDcDate.getTime())) {
            var newNamevalidate = document.getElementById('popupInwardDcDate');
            newNamevalidate.style.border = '2px solid red';
            return false;

            // or display an error message to the user
        } else if (popupInwardDcDate <= restrictDt) {
            var newNamevalidate = document.getElementById('popupInwardDcDate');
            newNamevalidate.style.border = '2px solid red';
            alert("Supplier DC Date should be less than or Equal to Today's Date");
            return false;
            // or display an error message to the user
        }
        else {
            formattedDate = popupInwardDcDate.toISOString();
            var newNamevalidate = document.getElementById('popupInwardDcDate');
            newNamevalidate.style.border = '';
        }
        if (popupInwardInvRef.length === 0) {
            var newNamevalidate = document.getElementById('popupInwardInvRef');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('popupInwardInvRef');
            newNamevalidate.style.border = '';
        }   
        if (isNaN(popupInvDate.getTime())) {
            var newNamevalidate = document.getElementById('popupInwardDcDate');
            newNamevalidate.style.border = '2px solid red';
            return false;

            // or display an error message to the user
        } else if (popupInvDate <= restrictDt) {
            var newNamevalidate = document.getElementById('popupInwardDcDate');
            newNamevalidate.style.border = '2px solid red';
            alert("Supplier Inv Date should be less than or Equal to Today's Date");
            return false;
            // or display an error message to the user
        }
        else {
            formattedDate2 = popupInvDate.toISOString();
        }
        if (popupInwNoLine > popupBalLine) {
            alert("Please check the No of Line Items");
            return false;
        }
        if (isNaN(popupInwardHeaderId)) {
            popupInwardHeaderId = 0;
        }
        var rowData = {
            inw_Recpt_HeaderId: parseInt(popupInwardHeaderId),
            poHeaderId: parseInt(popupPoHeaderId),
            supplier_Dc_Ref: popupInwardDcref,
            Supplier_Dc_Date: formattedDate,
            Supplier_Inv_ref: popupInwardInvRef,
            Supplier_Inv_date: formattedDate2,
            Inw_Date_time: formattedDate
        };
        return $.ajax({
            type: "POST",
            url: '/workOrder/PostInw_Recpt_Header',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(rowData),
            dataType: "json",
            success: function (result) {
                var inw_Recpt_HeaderId = result.inw_Recpt_HeaderId;
                $("#popupInwardHeaderId").val(inw_Recpt_HeaderId);
                //var newNamevalidate = document.getElementById('P5RetentionDate');
                //newNamevalidate.style.border = '';
                //loadDocList();
                //$('#RetentionDate').modal('hide');
                alert("Inward Header Data Entered Successfully.");
            }
        });
    });
    $('#popup5').on('hidden.bs.modal', function (event) {
        document.getElementById('popup4PoLineData').style.filter = 'none';
        $("#P5Condition").val(0);
        $("#P5SuppCount").val("");
        $("#P5Comment").val("");
        $("#P5InwDetailsId").val("");
        $("#P5OurCount").val("");
        var P5Condition = document.getElementById('P5Condition');
        P5Condition.style.border = '';
        var P5OurCount = document.getElementById('P5OurCount');
        P5OurCount.style.border = '';
        var P5Comment = document.getElementById('P5Comment');
        P5Comment.style.border = '';
        var P5SuppCount = document.getElementById('P5SuppCount');
        P5SuppCount.style.border = '';
    });
    $('#popup5').on('show.bs.modal', function (event) {
        document.getElementById('popup4PoLineData').style.filter = 'blur(5px)';
        var part = $("#P4SpanPartNo").text();
        var sup = $("#P4SpanSupp").text();
        $("#P5PartNoSpan").text(part);
        $("#P5SuppSpan").text(sup);

    });
    $('#popup4PoLineData').on('show.bs.modal', function (event) {
        showPopup();
    });
    $('#popupInward').on('hidden.bs.modal', function (event) {
        $("#popupInwardHeaderId").val('');
        $("#popupBalLine").val('');
        $("#popupInwNoLine").val('');
        $("#popupInwardDcDate").val('');
        $("#popupInwardDcref").val('');
        $("#popupInwardInvRef").val('');
    });
    $('#popupInward2').on('show.bs.modal', function (event) {
        poQnty = 0;
        $("#ourCountVM").text("0");
        $("#suppCountVM").text("0");
        poqntytodayrecd = 0;
        var relatedTarget = $(event.relatedTarget);
        var supp = relatedTarget.data("supp");
        var poref = relatedTarget.data("poref");
        var podate = relatedTarget.data("podate");
        var postatus = relatedTarget.data("postatus");
        var porcptdate = relatedTarget.data("porcptdate");
        var podetails = relatedTarget.data("podetails");
        var noofline = relatedTarget.data("noofline");
        var partid = relatedTarget.data("partid");
        var edit = relatedTarget.data("edit");
        var partno = relatedTarget.data("partno");
        var procid = relatedTarget.data("procid");
        var poqnty = relatedTarget.data("poqnty");
        var units = relatedTarget.data("units");
        var parttype = relatedTarget.data("parttype");
        var docavl = relatedTarget.data("docavl");
        var poqntyrecd = relatedTarget.data("poqntyrecd");
        var mismatch_Resolved = relatedTarget.data("mismatchresolved");
        if (mismatch_Resolved == undefined || mismatch_Resolved == null) {
            mismatch_Resolved = "-";
        }
        var inwardDate = new Date();
        calculateTotals();
        poQnty = poqnty;
        partNo = partno;
        var baltorec = 0;
        $("#RmVMFDiv").prop("hidden",true);
        $("#SubconVFDiv").prop("hidden",false);
        $("#popupInward2SpanSup").text(supp);
        $("#popupInward2SpanPo").text(poref);
        $("#popupInward2SpanDate").text(podate);
        $("#popupInward2SpanInwDate").text(podate);
        $("#popupInward2SpanInwBy").text(supp);
        $("#popupPoHeaderId").val(podetails);
        $("#popup2BalLine").val(noofline);
        $("#popupInward2BalVM").val(noofline);
        $("#popupPoPartId").val(partid);
        if (edit === "{1}") {
            api.getbulk("/WorkOrder/GetAllInw_Recpt_Header").then((data) => {
                data = data.filter(item => item.poHeaderId == podetails);
                if (data.length > 0) {

                    var popupInwardHeaderId = data[0].inw_Recpt_HeaderId || 0;
                    var popupPoHeaderId = data[0].poHeaderId;
                    var Supplier_Dc_Ref = data[0].supplier_Dc_Ref;
                    var Supplier_Dc_Date = data[0].supplier_Dc_Date;
                    let dateTimeSDC = new Date(Supplier_Dc_Date);
                    let Supplier_Dc_DateFD = dateTimeSDC.toISOString().split('T')[0];
                    var Supplier_Inv_ref = data[0].supplier_Inv_ref;
                    var Supplier_Inv_date = data[0].supplier_Inv_date;
                    let dateTimeSInD = new Date(Supplier_Inv_date);
                    let Supplier_Inv_dateFD = dateTimeSInD.toISOString().split('T')[0];
                    $("#popupInwardHeaderId").val(popupInwardHeaderId);
                    $("#popupPoHeaderId").val(popupPoHeaderId);
                    $("#popupInw2NoLine").val(noofline);
                    $("#popupInward2DcDate").val(Supplier_Dc_DateFD);
                    $("#popupInward2Dcref").val(Supplier_Dc_Ref);
                    $("#popupInward2InvRef").val(Supplier_Inv_ref);
                    $("#popupInward2InvDate").val(Supplier_Inv_dateFD);
                }
                loadInwardDetails(popupInwardHeaderId);
                loadDocUploadList();
                baltorec = Math.max(0, poQnty - poqntytodayrecd);
                var tablebody = $("#popup2BalanceItemGrid tbody");
                $(tablebody).html("");
                var poqntyrecdstr = "";
                if (poqntyrecd === 0) {
                    poqntyrecdstr = "0"
                } else {
                    poqntyrecdstr = poqntyrecd;
                }
                var poqntytodayrecdstr = "";
                if (poqntytodayrecd === 0) {
                    poqntytodayrecdstr = "0"
                } else {
                    poqntytodayrecdstr = poqntytodayrecd;
                }
                // for (i = 0; i < data.length; i++) {
                api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partid)).then((data) => {
                    api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                        var routname = data[data.length - 1].routingName;
                        var opname = stepdata[stepdata.length - 1].stepNumber;
                        baltorec = Math.max(0, poQnty - poqntytodayrecd);

                        let dropdownOptions = baltorec > 0
                            ? `<a href="javascript:void(0);" class="dropdown-item" 
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-postatus="${postatus || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-supp="${supp || ''}"
          data-poqnty="${poQnty || ''}" 
          data-noofline="${noofline || ''}"
          data-podetails="${podetails || ''}"
          data-parttype="${parttype || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
          data-balnqnty="${baltorec || ''}"
          data-poqntytodayrecd="${poqntytodayrecd || ''}"
          data-edit="{0}" onclick="OpenPopup4(this)">
          Inward
      </a>`
                            : `<a href="javascript:void(0);" class="dropdown-item" 
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-supp="${supp || ''}"
          data-poqnty="${poQnty || ''}" 
          data-postatus="${postatus || ''}"
          data-noofline="${noofline || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-parttype="${parttype || ''}"
          data-podetails="${podetails || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
          data-balnqnty="${baltorec || ''}"
          data-poqntytodayrecd="${poqntytodayrecd || ''}"
          data-edit="{1}"
          onclick="OpenPopup4(this)">
          Edit
      </a>`;
                        let rowHtml = $(`
                    <tr>
                        <td>${poref || ''}</td>
                        <td>${partNo || ''}</td>
                        <td>${routname || ''}</td>
                        <td>${opname || ''}</td>
                        <td>${poqnty || ''}</td>
                        <td>${poqntyrecdstr || ''}</td>
                        <td id="baltorectd22B">${poqnty - poqntyrecd }</td>
                        <td id="alRecdC">${poqntytodayrecdstr || ''}</td>
                        <td id="baltorectd2D">${poqntyrecdstr || ''}</td>
                        <td>${baltorec || '0'}</td>
                        <td>${units || ''}</td>
                        <td>${docavl || ''}</td>
                        <td>${postatus}</td>
                        <td>${mismatch_Resolved}</td>
                        <td>
                            ${mismatch_Resolved === 'N'
                                                        ? ''
                                                        : `
                                <div class="dropdown float-center">
                                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                        <i class="mdi mdi-dots-vertical"></i>
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        ${dropdownOptions}
                                        <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal"
                                           data-poref="${poref || ''}"
                                           data-podate="${podate || ''}"
                                           data-supp="${supp || ''}"
                                           data-poqnty="${poQnty || ''}" 
                                           data-noofline="${noofline || ''}"
                                           data-units="${units || ''}"
                                           data-docavl="${docavl || ''}"
                                           data-podetails="${podetails || ''}"
                                           data-postatus="${postatus || ''}"
                                           data-procid="${procid || ''}"
                                           data-partno="${partNo || ''}"
                                           data-partid="${partid || ''}"
                                           data-porcptdate="${porcptdate || ''}"
                                           data-balnqnty="${baltorec || ''}"
                                           data-edit="{2}"
                                           data-bs-target="#popup4PoLineData">
                                            View Details
                                        </a>
                                    </div>
                                </div>
                            `}
                        </td>
                    </tr>
                `);
                        if (baltorec === 0) {
                            rowHtml.find('.inward-link').remove();
                        } else {
                            rowHtml.find('.edit-link').remove();
                        }
                        $(tablebody).append(rowHtml);
                    });
                });
        //}
            });
        } else {
            loadInwardDetails(0);
            $("#popupInwardHeaderId").val();
            $("#popupPoHeaderId").val();
            $("#popupInwNoLine").val();
            $("#popupInwardDcDate").val();
            $("#popupInwardDcref").val();
            $("#popupInwardInvRef").val();
            $("#popupInvDate").val();
            loadDocUploadList();
            baltorec = Math.max(0, poQnty - poqntytodayrecd);
            var tablebody = $("#popup2BalanceItemGrid tbody");
            $(tablebody).html("");
            var poqntyrecdstr = "";
            if (poqntyrecd === 0) {
                poqntyrecdstr = "0"
            } else {
                poqntyrecdstr = poqntyrecd;
            }
            var poqntytodayrecdstr = "";
            if (poqntytodayrecd === 0) {
                poqntytodayrecdstr = "0"
            } else {
                poqntytodayrecdstr = poqntytodayrecd;
            }
            // for (i = 0; i < data.length; i++) {
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partid)).then((data) => {
                api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                    var routname = data[data.length - 1].routingName;
                    var opname = stepdata[data.length - 1].stepNumber;
                    //api.get("/routings/subcons?stepId=" + stepdata[data.length - 1].stepId).then((data) => {

                    //    for (i = 0; i < data.length; i++) {
                    //        if (data[i].deleted == 1)
                    //            continue;
                    //        if (data.length == 1) {

                    //        }
                    //    }
                    //    //console.log(data);
                    //    //console.log(tablebody.html());
                    //    //RouteSuppliersTable
                    //    //RouteSupplierTemplate
                    //}).catch((error) => {
                    //});
                    baltorec = Math.max(0, poQnty - poqntytodayrecd);
                    let dropdownOptions = baltorec > 0
                        ? `<a href="javascript:void(0);" class="dropdown-item" 
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-postatus="${postatus || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-supp="${supp || ''}"
          data-parttype="${parttype || ''}"
          data-poqnty="${poQnty || ''}" 
          data-noofline="${noofline || ''}"
          data-podetails="${podetails || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
                                       data-balnqnty="${baltorec || ''}"
          data-edit="{0}"
           onclick="OpenPopup4(this)">
          Inward
      </a>`
                        : `<a href="javascript:void(0);" class="dropdown-item"  onclick="OpenPopup4(this)"
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-supp="${supp || ''}"
          data-poqnty="${poQnty || ''}" 
          data-postatus="${postatus || ''}"
          data-noofline="${noofline || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-podetails="${podetails || ''}"
          data-parttype="${parttype || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
                                       data-balnqnty="${baltorec || ''}"
          data-edit="{1}"
           onclick="OpenPopup4(this)">
          Edit
      </a>`;
                    let rowHtml = $(`
                    <tr>
                        <td>${poref || ''}</td>
                        <td>${partNo || ''}</td>
                        <td>${routname || ''}</td>
                        <td>${opname || ''}</td>
                        <td>${poqnty || ''}</td>
                        <td>${poqntyrecdstr || ''}</td>
                        <td id="baltorectd22">0</td>
                        <td>${poqntyrecdstr || ''}</td>
                        <td id="baltorectd2">${poqntytodayrecdstr || ''}</td>
                        <td>${baltorec || '0'}</td>
                        <td>${units || ''}</td>
                        <td>${docavl || ''}</td>
                        <td>${postatus}</td>
                        <td>${mismatch_Resolved}</td>
                        <td>
                            ${mismatch_Resolved === 'N'
                                                    ? ''
                                                    : `
                                <div class="dropdown float-center">
                                    <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                        <i class="mdi mdi-dots-vertical"></i>
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        ${dropdownOptions}
                                        <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal"
                                           data-poref="${poref || ''}"
                                           data-podate="${podate || ''}"
                                           data-supp="${supp || ''}"
                                           data-poqnty="${poQnty || ''}" 
                                           data-noofline="${noofline || ''}"
                                           data-units="${units || ''}"
                                           data-docavl="${docavl || ''}"
                                           data-podetails="${podetails || ''}"
                                           data-postatus="${postatus || ''}"
                                           data-procid="${procid || ''}"
                                           data-partno="${partNo || ''}"
                                           data-partid="${partid || ''}"
                                           data-porcptdate="${porcptdate || ''}"
                                           data-balnqnty="${baltorec || ''}"
                                           data-edit="{2}"
                                           data-bs-target="#popup4PoLineData">
                                            View Details
                                        </a>
                                    </div>
                                </div>
                            `}
                        </td>
                    </tr>
                `);
                    if (baltorec === 0) {
                        rowHtml.find('.inward-link').remove();
                    } else {
                        rowHtml.find('.edit-link').remove();
                    }
                    $(tablebody).append(rowHtml);
                });
            });
        //}
        }
        var popupInward2DcDate = document.getElementById('popupInward2DcDate');
        popupInward2DcDate.style.border = '';
        var popupInvDate = document.getElementById('popupInvDate');
        popupInvDate.style.border = '';
        var popupInward2InvRef = document.getElementById('popupInward2InvRef');
        popupInward2InvRef.style.border = '';
        var popupInward2DcDate = document.getElementById('popupInward2DcDate');
        popupInward2DcDate.style.border = '';
        var popupInward2Dcref = document.getElementById('popupInward2Dcref');
        popupInward2Dcref.style.border = '';
    });
    $('#popupInward').on('show.bs.modal', function (event) {
        poqntytodayrecd = 0;
        poQnty = 0;
        $("#ourCountVM").text("0");
        $("#suppCountVM").text("0");
        var relatedTarget = $(event.relatedTarget);
        $("#RmVMFDiv").prop("hidden", false);
        $("#SubconVFDiv").prop("hidden", true);
        var supp = relatedTarget.data("supp");
        var poref = relatedTarget.data("poref");
        var podate = relatedTarget.data("podate");
        var postatus = relatedTarget.data("postatus");
        var porcptdate = relatedTarget.data("porcptdate");
        var podetails = relatedTarget.data("podetails");
        var noofline = relatedTarget.data("noofline");
        var partid = relatedTarget.data("partid");
        var edit = relatedTarget.data("edit");
        var partno = relatedTarget.data("partno");
        var procid = relatedTarget.data("procid");
        var poqnty = relatedTarget.data("poqnty");
        var units = relatedTarget.data("units");
        var docavl = relatedTarget.data("docavl");
        var poqntyrecd = relatedTarget.data("poqntyrecd");
        var inwardDate = new Date();
        //calculateTotals();
        poQnty = poqnty;
        partNo = partno;
        var baltorec = 0;
        $("#popupInwardSpanSup").text(supp);
        $("#popupInwardSpanPo").text(poref);
        $("#popupInwardSpanDate").text(podate);
        $("#popupInwardSpanInwDate").text(podate);
        $("#popupInwardSpanInwBy").text(supp);
        $("#popupPoHeaderId").val(podetails);
        $("#popupBalLine").val(noofline);
        $("#popupInwardBalVM").val(noofline);
        $("#popupPoPartId").val(partid);
        if (edit === "{1}") {
            api.getbulk("/WorkOrder/GetAllInw_Recpt_Header").then((data) => {
                data = data.filter(item => item.poHeaderId == podetails);
                var popupInwardHeaderId = data[0].inw_Recpt_HeaderId;
                var popupPoHeaderId = data[0].poHeaderId;
                var Supplier_Dc_Ref = data[0].supplier_Dc_Ref;
                var Supplier_Dc_Date = data[0].supplier_Dc_Date;
                let dateTimeSDC = new Date(Supplier_Dc_Date);
                let Supplier_Dc_DateFD = dateTimeSDC.toISOString().split('T')[0];
                var Supplier_Inv_ref = data[0].supplier_Inv_ref;
                var Supplier_Inv_date = data[0].supplier_Inv_date;
                let dateTimeSInD = new Date(Supplier_Inv_date);
                let Supplier_Inv_dateFD = dateTimeSInD.toISOString().split('T')[0];
                $("#popupInwardHeaderId").val(popupInwardHeaderId);
                $("#popupPoHeaderId").val(popupPoHeaderId);
                $("#popupInwNoLine").val(noofline);
                $("#popupInwardDcDate").val(Supplier_Dc_DateFD);
                $("#popupInwardDcref").val(Supplier_Dc_Ref);
                $("#popupInwardInvRef").val(Supplier_Inv_ref);
                $("#popupInvDate").val(Supplier_Inv_dateFD);
                loadInwardDetails(popupInwardHeaderId);
                loadDocUploadList();
                baltorec = Math.max(0, poQnty - poqntytodayrecd);
                var tablebody = $("#popupBalanceItemGrid tbody");
                $(tablebody).html("");
                var poqntyrecdstr = "";
                if (poqntyrecd === 0) {
                    poqntyrecdstr = "0"
                } else {
                    poqntyrecdstr = poqntyrecd;
                }
                var poqntytodayrecdstr = "";
                if (poqntytodayrecd === 0) {
                    poqntytodayrecdstr = "0"
                } else {
                    poqntytodayrecdstr = poqntytodayrecd;
                }

                let dropdownOptions = baltorec > 0
                    ? `<a href="javascript:void(0);" class="dropdown-item" 
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-postatus="${postatus || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-supp="${supp || ''}"
          data-poqnty="${poQnty || ''}" 
          data-noofline="${noofline || ''}"
          data-podetails="${podetails || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
                                       data-balnqnty="${baltorec || ''}"
          data-edit="{0}"
           onclick="OpenPopup4(this)">
          Inward
      </a>`
                    : `<a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal"
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-supp="${supp || ''}"
          data-poqnty="${poQnty || ''}" 
          data-postatus="${postatus || ''}"
          data-noofline="${noofline || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-podetails="${podetails || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
                                       data-balnqnty="${baltorec || ''}"
          data-edit="{1}"
           onclick="OpenPopup4(this)">
          Edit
      </a>`;
                // for (i = 0; i < data.length; i++) {
                let rowHtml = $(`
                    <tr>
                        <td>${partNo || ''}</td>
                        <td>${poqnty || ''}</td>
                        <td>${poqntyrecdstr || ''}</td>
                        <td id="baltorectd">${poqntytodayrecdstr || ''}</td>
                        <td>${baltorec || '0'}</td>
                        <td>${units || ''}</td>
                        <td>${docavl || ''}</td>
                        <td>${postatus}</td>
                        <td>
                            <div class="dropdown float-center">
                                <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                    <i class="mdi mdi-dots-vertical"></i>
                                </a>
                                <div class="dropdown-menu dropdown-menu-end">
                                    ${dropdownOptions}
                                    <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal"
                                       data-poref="${poref || ''}"
                                       data-podate="${podate || ''}"
                                       data-supp="${supp || ''}"
                                       data-poqnty="${poQnty || ''}" 
                                       data-noofline="${noofline || ''}"
                                       data-units="${units || ''}"
                                       data-docavl="${docavl || ''}"
                                       data-podetails="${podetails || ''}"
                                       data-postatus="${postatus || ''}"
                                       data-procid="${procid || ''}"
                                       data-partno="${partNo || ''}"
                                       data-partid="${partid || ''}"
                                       data-porcptdate="${porcptdate || ''}"
                                       data-balnqnty="${baltorec || ''}"
                                       data-edit="{2}"
                                       data-bs-target="#popup4PoLineData">
                                        View Details
                                    </a>
                                </div>
                            </div>
                        </td>
                    </tr>
                `);
                if (baltorec === 0) {
                    rowHtml.find('.inward-link').remove();
                } else {
                    rowHtml.find('.edit-link').remove();
                }
                $(tablebody).append(rowHtml);
        //}
            });
        } else {
            $("#popupInwardHeaderId").val();
            $("#popupPoHeaderId").val();
            $("#popupInwNoLine").val();
            $("#popupInwardDcDate").val();
            $("#popupInwardDcref").val();
            $("#popupInwardInvRef").val();
            $("#popupInvDate").val();
            loadDocUploadList();
            loadInwardDetails(0);
            baltorec = Math.max(0, poQnty - poqntytodayrecd);

            var tablebody = $("#popupBalanceItemGrid tbody");
            $(tablebody).html("");
            var poqntyrecdstr = "";
            if (poqntyrecd === 0) {
                poqntyrecdstr = "0"
            } else {
                poqntyrecdstr = poqntyrecd;
            }
            var poqntytodayrecdstr = "";
            if (poqntytodayrecd === 0) {
                poqntytodayrecdstr = "0"
            } else {
                poqntytodayrecdstr = poqntytodayrecd;
            }

            let dropdownOptions = baltorec > 0
                ? `<a href="javascript:void(0);" class="dropdown-item" 
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-postatus="${postatus || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-supp="${supp || ''}"
          data-poqnty="${poQnty || ''}" 
          data-noofline="${noofline || ''}"
          data-podetails="${podetails || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
          data-edit="{0}"
                                       data-balnqnty="${baltorec || ''}"
           onclick="OpenPopup4(this)">
          Inward
      </a>`
                : `<a href="javascript:void(0);" class="dropdown-item" 
          data-poref="${poref || ''}"
          data-podate="${podate || ''}"
          data-supp="${supp || ''}"
          data-poqnty="${poQnty || ''}" 
          data-postatus="${postatus || ''}"
          data-noofline="${noofline || ''}"
          data-units="${units || ''}"
          data-docavl="${docavl || ''}"
          data-podetails="${podetails || ''}"
          data-procid="${procid || ''}"
          data-partno="${partNo || ''}"
          data-partid="${partid || ''}"
          data-porcptdate="${porcptdate || ''}"
                                       data-balnqnty="${baltorec || ''}"
          data-edit="{1}"
           onclick="OpenPopup4(this)">
          Edit
      </a>`;
            // for (i = 0; i < data.length; i++) {
            let rowHtml = $(`
                    <tr>
                        <td>${partNo || ''}</td>
                        <td>${poqnty || ''}</td>
                        <td>${poqntyrecdstr || ''}</td>
                        <td>${poqntytodayrecdstr || ''}</td>
                        <td>${baltorec || '0'}</td>
                        <td>${units || ''}</td>
                        <td>${docavl || ''}</td>
                        <td>${postatus}</td>
                        <td>
                            <div class="dropdown float-center">
                                <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                    <i class="mdi mdi-dots-vertical"></i>
                                </a>
                                <div class="dropdown-menu dropdown-menu-end">
                                    ${dropdownOptions}
                                    <a href="javascript:void(0);" class="dropdown-item" data-bs-toggle="modal"
                                       data-poref="${poref || ''}"
                                       data-podate="${podate || ''}"
                                       data-units="${units || ''}"
                                       data-docavl="${docavl || ''}"
                                       data-supp="${supp || ''}"
                                       data-poqnty="${poQnty || ''}" 
                                       data-noofline="${noofline || ''}"
                                       data-postatus="${postatus || ''}"
                                       data-podetails="${podetails || ''}"
                                       data-procid="${procid || ''}"
                                       data-partno="${partNo || ''}"
                                       data-partid="${partid || ''}"
                                       data-porcptdate="${porcptdate || ''}"
                                       data-balnqnty="${baltorec || ''}"
                                       data-edit="{2}"
                                       data-bs-target="#popup4PoLineData">
                                        View Details
                                    </a>
                                </div>
                            </div>
                        </td>
                    </tr>
                `);
            if (baltorec === 0) {
                rowHtml.find('.inward-link').remove();
            }else{
                rowHtml.find('.edit-link').remove();
            }
            $(tablebody).append(rowHtml);
        //}
        }
    });
    $('#doc-item').on('hidden.bs.modal', function (event) {
        document.getElementById('popup4PoLineData').style.filter = 'none';
        document.getElementById('popupInward').style.filter = 'none';
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
        document.getElementById('popup4PoLineData').style.filter = 'blur(5px)';
        document.getElementById('popupInward').style.filter = 'blur(5px)';
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
        formData.append("Inw_Recpt_HeaderId", parseInt(document.getElementById("popupInwardHeaderId").value));
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

function displayFileName() {
    var docid = parseInt($("#doclistidFile").val());
    if (docid != 0) {
        var doctypeid = parseInt($("#docTypeIdFile").val());
        api.get("/DocumentManagement/GetAllDocumentType").then((data) => {
            const fdoc = data.find(item => item.documentTypeId === doctypeid);
            if (fdoc.docuCategory == 2) {
                var confrimval = confirm("Do You Want Move the Exsisting File To Archive.");
                if (confrimval) {
                    var fileInput = document.getElementById('fileUploadInput');
                    var fileName = fileInput.files[0].name; // Get the uploaded file name
                    document.getElementById('fileNameDisplay').value = fileName; // Display the file name in the text input
                    var docid = parseInt($("#doclistidFile").val());
                    archive = 1;
                    $("#Resonbtn").click();
                } else {
                    var fileInput = document.getElementById("fileUploadInput");
                    fileInput.value = "";
                }
            } else {
                var fileInput = document.getElementById('fileUploadInput');
                var fileName = fileInput.files[0].name; // Get the uploaded file name
                document.getElementById('fileNameDisplay').value = fileName;
            }
        }).catch((error) => {
        });
    } else {
        var fileInput = document.getElementById('fileUploadInput');
        var fileName = fileInput.files[0].name; // Get the uploaded file name
        document.getElementById('fileNameDisplay').value = fileName;
    }
}

function DeleteDocList(element) {
    var relatedTarget = $(element);
    var doclistid = relatedTarget.data("doclistid");
    if (doclistid != 0) {
        var confrimval = confirm("Do You Want This Document.");
        if (confrimval) {
            api.get("/masters/DeleteDocListAndFile?doclistid=" + doclistid).then((data) => {
                //console.log(data);
                loadDocUploadList();
            }).catch((error) => {
                //console.log(error);
            });
        }
    } else {
        alert("This Document Type Do Not Have Document.");
    }
}

function EditCaptureDetails(element) {
    var relatedTarget = $(element);
    var condition = relatedTarget.data("condition");
    var ourcount = relatedTarget.data("ourcount");
    var vendorcount = relatedTarget.data("vendorcount");
    var comment = relatedTarget.data("comment");
    var id = relatedTarget.data("id");
    var headerid = relatedTarget.data("headerid");
    $("#P5Condition").val(condition);
    $("#P5OurCount").val(ourcount);
    $("#P5SuppCount").val(vendorcount);
    $("#P5InwDetailsId").val(id);
    $("#P5Comment").val(comment);
    $("#popup5").modal("show");
}
function OpenPopup4(element) {
    var relatedTarget = $(element);

    $("#P4MessageBox").text("");
    var row = $("#popupInwardDocGrid tbody tr");
    var mandatory = row.find("td:nth-child(2)").text().trim();
    var comment = row.find("td:nth-child(3)").text().trim();
    if (mandatory == "Y") {
        if (comment.length <= 0) {
            alert("Mandatory Inward Document are not Uploaded");
            $("#P4BtnClose").click();
            $("#popup4PoLineData").modal("hide");
        } else {
            $("#popup4PoLineData").modal("show");
            var inwheaderid = parseInt($("#popupInwardHeaderId").val());
            loadInwardDetails(inwheaderid);
            var supp = relatedTarget.data("supp");
            var poref = relatedTarget.data("poref");
            var podate = relatedTarget.data("podate");
            var porcptdate = relatedTarget.data("porcptdate");
            var podetails = relatedTarget.data("podetails");
            var noofline = relatedTarget.data("noofline");
            var partid = relatedTarget.data("partid");
            var edit = relatedTarget.data("edit");
            var partno = relatedTarget.data("partno");
            var docavl = relatedTarget.data("docavl");
            var units = relatedTarget.data("units");
            var procid = relatedTarget.data("procid");
            var postatus = relatedTarget.data("postatus");
            var parttype = relatedTarget.data("parttype");
            var poqnty = relatedTarget.data("poqnty");
            var balnqnty = relatedTarget.data("balnqnty");
            PPartType = parttype;
            $("#P5RoutingSpan").prop("hidden", true);
            $("#P4RoutingSpan").prop("hidden", true);
            $("#P4SpanPartNo").text(partno);
            $("#popupInwardPoStatusVM").val(postatus);
            $("#P4SpanSupp").text(supp);
            let dateTimeSDC = new Date();
            let formattedDate = dateTimeSDC.toISOString().split('T')[0];
            //$("#popupInwardPlanRecpt").val(porcptdate.split('T')[0]);
            //$("#P4SubConPlanDate").val(porcptdate.split('T')[0]);
            //$("#popupInwardActlDate").val(formattedDate);
            $("#P4SubConActDate").val(0);
            var balitems = $("#popupInwardBalVM").val() || $("#popupInward2BalVM").val();
            $("#popupInwardBalToRecVM").val(balnqnty || 0);
            $("#P4SubConBalToRc").val(balnqnty || 0);
            $("#popupInwardActlDateUnit").text(units);
            $("#P4SubConBalToRcIUnit").text(units);
            $("#P4SubConActDateunit").text(units);
            $("#P4SubConPlanDateUnit").text(units);
            $("#P5SuppUnit").text(units);
            $("#P5QurUnit").text(units);
            $("#unitsTbVM").text(units);
            $("#popupInwardBalToRecVMUnit").text(units);
            $("#popupInwardPlanRecptUnit").text(units);
            if (parttype === "ManufacturedPart") {
                loadCondition2();
                $("#P5RoutingSpan").prop("hidden", false);
                $("#P4RoutingSpan").prop("hidden", false);

                api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partid)).then((data) => {
                    api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                        var routname = data[data.length - 1].routingName;
                        var opname = stepdata[stepdata.length - 1].stepNumber;
                        $("#P5SpanRouting").text(routname);
                        $("#P4SpanRouting").text(routname);
                        $("#P4SpanOprNo").text(opname);
                        $("#P5SpanOprNo").text(opname);
                    });
                });
            } else {
                loadCondition();
            }
            if (edit === "{1}") {
                //api.getbulk("/WorkOrder/GetAllInw_Recpt_Header").then((data) => {
                //    data = data.filter(item => item.poHeaderId == podetails);
                //    var popupInwardHeaderId = data[0].inw_Recpt_HeaderId;
                //    var popupPoHeaderId = data[0].poHeaderId;
                //    var Supplier_Dc_Ref = data[0].supplier_Dc_Ref;
                //    var Supplier_Dc_Date = data[0].supplier_Dc_Date;
                //    let dateTimeSDC = new Date(Supplier_Dc_Date);
                //    let Supplier_Dc_DateFD = dateTimeSDC.toISOString().split('T')[0];
                //    var Supplier_Inv_ref = data[0].supplier_Inv_ref;
                //    var Supplier_Inv_date = data[0].supplier_Inv_date;
                //    let dateTimeSInD = new Date(Supplier_Inv_date);
                //    let Supplier_Inv_dateFD = dateTimeSInD.toISOString().split('T')[0];
                //    $("#popupInwardHeaderId").val(popupInwardHeaderId);
                //    $("#popupPoHeaderId").val(popupPoHeaderId);
                //    $("#popupInwNoLine").val(noofline);
                //    $("#popupInwardDcDate").val(Supplier_Dc_DateFD);
                //    $("#popupInwardDcref").val(Supplier_Dc_Ref);
                //    $("#popupInwardInvRef").val(Supplier_Inv_ref);
                //    $("#popupInvDate").val(Supplier_Inv_dateFD);
                //    loadDocUploadList();
                //});
                loadDetailsDocUploadList();
            } else {
                loadDetailsDocUploadList();
            }
        }
    } else {

        $("#popup4PoLineData").modal("show");
        var inwheaderid = parseInt($("#popupInwardHeaderId").val());
        loadInwardDetails(inwheaderid);
        var supp = relatedTarget.data("supp");
        var poref = relatedTarget.data("poref");
        var podate = relatedTarget.data("podate");
        var porcptdate = relatedTarget.data("porcptdate");
        var podetails = relatedTarget.data("podetails");
        var noofline = relatedTarget.data("noofline");
        var partid = relatedTarget.data("partid");
        var edit = relatedTarget.data("edit");
        var partno = relatedTarget.data("partno");
        var docavl = relatedTarget.data("docavl");
        var units = relatedTarget.data("units");
        var procid = relatedTarget.data("procid");
        var postatus = relatedTarget.data("postatus");
        var parttype = relatedTarget.data("parttype");
        var poqnty = relatedTarget.data("poqnty");
        var balnqnty = relatedTarget.data("balnqnty");
        PPartType = parttype;
        $("#P5RoutingSpan").prop("hidden", true);
        $("#P4RoutingSpan").prop("hidden", true);
        $("#P4SpanPartNo").text(partno);
        $("#popupInwardPoStatusVM").val(postatus);
        $("#P4SpanSupp").text(supp);
        let dateTimeSDC = new Date();
        let formattedDate = dateTimeSDC.toISOString().split('T')[0];
        //$("#popupInwardPlanRecpt").val(porcptdate.split('T')[0]);
        //$("#P4SubConPlanDate").val(porcptdate.split('T')[0]);
        //$("#popupInwardActlDate").val(formattedDate);
        $("#P4SubConActDate").val(0);
        var balitems = $("#popupInwardBalVM").val() || $("#popupInward2BalVM").val();
        $("#popupInwardBalToRecVM").val(balnqnty || 0);
        $("#P4SubConBalToRc").val(balnqnty || 0);
        $("#popupInwardActlDateUnit").text(units);
        $("#P4SubConBalToRcIUnit").text(units);
        $("#P4SubConActDateunit").text(units);
        $("#P4SubConPlanDateUnit").text(units);
        $("#P5SuppUnit").text(units);
        $("#P5QurUnit").text(units);
        $("#unitsTbVM").text(units);
        $("#popupInwardBalToRecVMUnit").text(units);
        $("#popupInwardPlanRecptUnit").text(units);
        if (parttype === "ManufacturedPart") {
            loadCondition2();
            $("#P5RoutingSpan").prop("hidden", false);
            $("#P4RoutingSpan").prop("hidden", false);

            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partid)).then((data) => {
                api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                    var routname = data[data.length - 1].routingName;
                    var opname = stepdata[stepdata.length - 1].stepNumber;
                    $("#P5SpanRouting").text(routname);
                    $("#P4SpanRouting").text(routname);
                    $("#P4SpanOprNo").text(opname);
                    $("#P5SpanOprNo").text(opname);
                });
            });
        } else {
            loadCondition();
        }
        if (edit === "{1}") {
            //api.getbulk("/WorkOrder/GetAllInw_Recpt_Header").then((data) => {
            //    data = data.filter(item => item.poHeaderId == podetails);
            //    var popupInwardHeaderId = data[0].inw_Recpt_HeaderId;
            //    var popupPoHeaderId = data[0].poHeaderId;
            //    var Supplier_Dc_Ref = data[0].supplier_Dc_Ref;
            //    var Supplier_Dc_Date = data[0].supplier_Dc_Date;
            //    let dateTimeSDC = new Date(Supplier_Dc_Date);
            //    let Supplier_Dc_DateFD = dateTimeSDC.toISOString().split('T')[0];
            //    var Supplier_Inv_ref = data[0].supplier_Inv_ref;
            //    var Supplier_Inv_date = data[0].supplier_Inv_date;
            //    let dateTimeSInD = new Date(Supplier_Inv_date);
            //    let Supplier_Inv_dateFD = dateTimeSInD.toISOString().split('T')[0];
            //    $("#popupInwardHeaderId").val(popupInwardHeaderId);
            //    $("#popupPoHeaderId").val(popupPoHeaderId);
            //    $("#popupInwNoLine").val(noofline);
            //    $("#popupInwardDcDate").val(Supplier_Dc_DateFD);
            //    $("#popupInwardDcref").val(Supplier_Dc_Ref);
            //    $("#popupInwardInvRef").val(Supplier_Inv_ref);
            //    $("#popupInvDate").val(Supplier_Inv_dateFD);
            //    loadDocUploadList();
            //});
            loadDetailsDocUploadList();
        } else {
            loadDetailsDocUploadList();
        }
        //$("#popup4PoLineData").modal("hide");
    }
}
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

    var inw_Recpt_HeaderId = $("#popupInwardDetailsId").val();
    if (isNaN(inw_Recpt_HeaderId)) {
        api.getbulk("/workOrder/InwardDetailsDoclist?podetailsId=" + inw_Recpt_HeaderId ).then((data) => {
                    //data = data.filter(item => item.status == 1 || item.status==0);
               var tablebody = $("#popupInwardVendorDocGrid tbody");
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
        api.getbulk("/workOrder/InwardDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status == 0);
            var tablebody = $("#popupInwardVendorDocGrid tbody");
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

function loadDocUploadList() {

    //var content = parseInt($("#ManufacturedPartType").val());
    var inw_Recpt_HeaderId = $("#popupInwardHeaderId").val();
    if (isNaN(inw_Recpt_HeaderId)) {
        api.getbulk("/workOrder/InwardDoclist?poheaderid=" + inw_Recpt_HeaderId ).then((data) => {
                    //data = data.filter(item => item.status == 1 || item.status==0);
            var tablebody = $("#popupInwardDocGrid tbody");
            var tablebody2 = $("#popupInward2SupplierGrid tbody");
                    $(tablebody).html("");//empty tbody
                    $(tablebody2).html("");//empty tbody
                    //console.log(data);
                    for (i = 0; i < data.length; i++) {
                        let rowHtml2 = $(`
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
                            rowHtml2.find('.edit-link').remove(); // Remove Edit link
                            rowHtml2.find('.delete-link').remove(); // Remove Delete link
                            rowHtml.find('.delete-link').remove(); // Remove Delete link
                        }

                        if (data[i].docListId !== 0) {
                            rowHtml2.find('.upload-link').remove(); // Remove Upload link
                            rowHtml.find('.upload-link').remove(); // Remove Upload link
                        }

                        if (data[i].mandatory === 'Y') {
                            rowHtml2.find('.delete-link').remove(); // Remove Delete link for mandatory items
                            rowHtml.find('.delete-link').remove(); // Remove Delete link for mandatory items
                        }

                        // Append the processed row to the table body
                        $(tablebody).append(rowHtml2);
                        $(tablebody2).append(rowHtml);
                    }

                }).catch((error) => {
                    console.log(error);
                });
    } else {
        api.getbulk("/workOrder/InwardDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status == 0);
            var tablebody2 = $("#popupInward2SupplierGrid tbody");
            var tablebody = $("#popupInwardDocGrid tbody");
            $(tablebody).html("");//empty tbody
            $(tablebody2).html("");//empty tbody
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
                let rowHtml2 = $(`
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
                if (data[i].docListId === 0) {
                    rowHtml2.find('.edit-link').remove(); // Remove Edit link
                    rowHtml.find('.edit-link').remove(); // Remove Edit link
                    rowHtml2.find('.delete-link').remove(); // Remove Delete link
                    rowHtml.find('.delete-link').remove(); // Remove Delete link
                }

                if (data[i].docListId !== 0) {
                    rowHtml2.find('.upload-link').remove(); // Remove Upload link
                    rowHtml.find('.upload-link').remove(); // Remove Upload link
                }

                if (data[i].mandatory === 'Y') {
                    rowHtml2.find('.delete-link').remove(); // Remove Delete link for mandatory items
                    rowHtml.find('.delete-link').remove(); // Remove Delete link for mandatory items
                }

                // Append the processed row to the table body
                $(tablebody2).append(rowHtml2);
                $(tablebody).append(rowHtml);
            }

        }).catch((error) => {
            console.log(error);
        });

    }
}   