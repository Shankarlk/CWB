let isP81Modified = false;
let isP11Modified = false;
let originalRoutingId = 0;
let currentRoutingId = 0;
let mcAdd = 0;
let Popup10Open = 0;
let closeP26 = 0;
var stoppingData = {
    woStoppingOprIds: {},     // { woId: oprListId }
    oprStoppingMcWaitIds: {}  // { oprListId: mcWaitListId }
};
function landingPage() {
    const today = new Date();
    api.getbulk("/WorkOrder/GetAllReadyforProductionWo").then((data) => {
        const count = data.length;
        const AssyCount = data.filter((salesOrder) => salesOrder.partType === 2 && salesOrder.parentWoId === 0 ).length;
        const CmpCount = data.filter((salesOrder) => salesOrder.partType === 1 && salesOrder.parentWoId === 0).length;
        const ChildAssyCount = data.filter((salesOrder) => salesOrder.partType === 2  ).length;
        const ChildCmpCount = data.filter((salesOrder) => salesOrder.partType === 1 ).length;
        $('#ReadAssy').text(AssyCount);
        $('#ReadCMp').text(CmpCount);
        $('#InMcQueRAssy').text('0');
        $('#InMcQueRCmp').text('0');
    }).catch((error) => {
    });
    api.getbulk("/WorkOrder/GetAllRwk_List").then((data) => {
        const AssyCount = data.filter((workOrder) => workOrder.partType === 1).length;
        const CmpCount = data.filter((workOrder) => workOrder.partType === 2).length;
        $('#RwkAssy').text(AssyCount);
        $('#RwkCmp').text(CmpCount);
    }).catch((error) => {
    });
    api.getbulk("/WorkOrder/GetAllNon_Plan_Wk_List").then((data) => {
        $('#NonAssy').text(data.length);
    }).catch((error) => {
    });
    api.getbulk("/WorkOrder/GetAllWO_Wait_List").then((data) => {
        //console.log(data);
        const AssyCount = data.filter(workOrder =>
            workOrder.planStartStr &&
            workOrder.planEndStr &&
            workOrder.plan_Start_Date &&
            workOrder.plan_End_Date &&
            workOrder.plan_Simul_Qnty
        ).length;
        if (AssyCount > 0) {
            $("#selectwoProdBtn").prop("disabled", false);
        } else {
            $("#selectwoProdBtn").prop("disabled", false);
        }
        //$('#NonAssy').text(data.length);
    }).catch((error) => {
    });
}

async function showPopup11() {
    const popup = document.querySelector('#Popup11');

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
async function showPopup10() {
    const popup = document.querySelector('#PopupNC10');

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

    landingPage();
    $("#UpdateSimulaion").on("click", function () {
        isDataSaved = false;
        $('#preloadersim').show();
        $.ajax({
            type: "POST",
            url: '/WorkOrder/UpdateCurrentProductionStatus',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            success: function (result) {
                console.log("Success:", result);
                isDataSaved = true;
                $('#preloadersim').hide();
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                alert("Simulation failed ");
            },
            complete: function () {
                // This always runs after success or error
                $('#preloadersim').hide();
            }
        });
    });
    $("#P1Simulate").on("click", function () {
        let selectedWOs = [];

        $(".P1gridChk:checked").each(function () {
            let ppid = $(this).data("ppid");  // you can also use data("workorderid") etc.
            selectedWOs.push(ppid);
        });

        if (selectedWOs.length === 0) {
            alert("Please select at least one Work Order to simulate.");
            return;
        }
        alert('Simulation Has Started');
        isDataSaved = false;
        $('#preloadersim').show();
        $.ajax({
            type: "POST",
            url: '/WorkOrder/SimulateWOAllocation',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(selectedWOs),
            dataType: "json",
            success: function (result) {
                alert(result.message);
                isDataSaved = true;
                $("#Popup1").modal("hide");
                $('#preloadersim').hide();
                loadSimulationWos();
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                alert("Simulation failed ");
            },
            complete: function () {
                // This always runs after success or error
                $('#preloadersim').hide();
            }
        });
    });
    $("#PauseBtn").hide();
    $("#CountinueBtn").hide();
    $("#CancelBtn").hide();
    $('#SimulationCancelBtn').on('click', function () {
        if (confirm("Are you sure you want to cancel the simulation?")) {
            $.post("/WorkOrder/StopSimulation", function (res) {
                alert(res);
                $('#preloadersim').hide(); // hide spinner immediately
                isDataSaved = true;
                loadSimulationWos();
            });
        }
    });
    $('#CountinueBtn').on('click', function () {
        isDataSaved = false;
        $('#preloadersim').show();
        $.post("/WorkOrder/ResumeSimulation", function (res) {
            alert(res);
        });
    });
    $('#P1SelectAll').on('change', function () {
        var isChecked = $(this).is(':checked');
        $('#P1Grid').find('.P1gridChk').prop('checked', isChecked);
    });
    $('#P1Grid').on('change', '.P1gridChk', function () {
        if (!$(this).is(':checked')) {
            $('#P1SelectAll').prop('checked', false);
        } else if ($('.P1gridChk:checked').length === $('.P1gridChk').length) {
            $('#P1SelectAll').prop('checked', true);
        }
    });
    $('#SimulationPauseBtn').on('click', function () {
        if (confirm("Are you sure you want to pause the simulation?")) {
            $.post("/WorkOrder/PauseSimulation", function (res) {
                $('#preloadersim').hide();
                $("#CancelBtn").show(); // show Resume button
                $("#CountinueBtn").show(); // show Resume button
                $("#Popup1").modal("hide");
                loadSimulationWos();
                alert(res);
            });
        }
    });

    $('#Popup8').on('show.bs.modal', function (event) {
        loadNonWrkList();
        loadMachinceType();
        loadMachinceList();
    });
    $('#Popup81').on('hide.bs.modal', function (event) {
        document.getElementById('Popup8').style.filter = 'none';
        var P81MachinceSel2 = document.getElementById('P81MachinceSel2');
        P81MachinceSel2.style.border = '';
        var P81MachinceSel1 = document.getElementById('P81MachinceSel1');
        P81MachinceSel1.style.border = '';
        var P81WorkDesc = document.getElementById('P81WorkDesc');
        P81WorkDesc.style.border = '';
        var P81NonPlanType = document.getElementById('P81NonPlanType');
        P81NonPlanType.style.border = '';
        var P81ReqStartDate = document.getElementById('P81ReqStartDate');
        P81ReqStartDate.style.border = '';
        var P81ReqStartTime = document.getElementById('P81ReqStartTime');
        P81ReqStartTime.style.border = '';
        var P81ReqDur = document.getElementById('P81ReqDur');
        P81ReqDur.style.border = '';
        var P81ActDate1 = document.getElementById('P81ActDate1');
        P81ActDate1.style.border = '';
        var P81ActTime1 = document.getElementById('P81ActTime1');
        P81ActTime1.style.border = '';
        var P81CloseCom1 = document.getElementById('P81CloseCom1');
        P81CloseCom1.style.border = '';
        var P81ActEndDate1 = document.getElementById('P81ActEndDate1');
        P81ActEndDate1.style.border = '';
        var P81ActEndTime1 = document.getElementById('P81ActEndTime1');
        P81ActEndTime1.style.border = '';
        var P81ActDur = document.getElementById('P81ActDur');
        P81ActDur.style.border = '';
        if (isP81Modified) {
            event.preventDefault();
            $("#openfrom").val("P81");
            $("#ErrorMessage1").modal("show");
        }
    });
    $('#Popup81').on('show.bs.modal', function (event) {
        document.getElementById('Popup8').style.filter = 'blur(5px)';
        $("#Div2Act").hide();
        $("#Div3Close").hide();
        $("#Div2Acthr").hide();
        $("#Div3Closehr").hide();
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var mcid = relatedTarget.data("mcid");
        var mcname = relatedTarget.data("mcname");
        var shopname = relatedTarget.data("shopname");
        var nontype = relatedTarget.data("nontype");
        var reqddur = relatedTarget.data("reqddur");
        var reqddate = relatedTarget.data("reqddate");
        var workdesc = relatedTarget.data("workdesc");
        var actstartdate = relatedTarget.data("actstartdate"); 
        var actdur = relatedTarget.data("actdur");
        var actenddate = relatedTarget.data("actenddate");
        var closecom = relatedTarget.data("closecom");
        $("#P81ActDur").val(actdur).prop("readonly", true);

        $("#P81NonPlandId").val(id);
        $("#P81MachinceId").val(mcid);
        $("#P81MachinceSel2").val(mcname);
        $("#P81MachinceSel1").val(shopname);
        $("#P81WorkDesc").val(workdesc);
        $("#P81NonPlanType").val(nontype);
        if (reqddate) {
            var parts = reqddate.split("T");
            var datePart = parts[0];                // "2025-06-09"
            var timePart = parts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

            $("#P81ReqStartDate").val(datePart);
            $("#P81ReqStartTime").val(timePart);
        }
        $("#P81ReqDur").val(reqddur);
        if (closecom === null || !closecom) {
            $("#Div3Close").hide();
            $("#Div3Closehr").hide()
        } else {
            $("#Div3Close").show();
            $("#Div3Closehr").show();
            $("#P81CloseCom1").val(closecom);
            $("#P81ActDur").val(actdur).prop("readonly", true);
            if (actenddate) {
                var eparts = actenddate.split("T");
                var edatePart = eparts[0];                // "2025-06-09"
                var etimePart = eparts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

                $("#P81ActEndDate1").val(edatePart);
                $("#P81ActEndTime1").val(etimePart);
            }
        }
        if (!actstartdate) {
            $("#P81MachinceSel2").val(mcname).prop("readonly", true);
            $("#P81MachinceSel1").val(shopname).prop("readonly", true);
            $("#P81WorkDesc").val(workdesc).prop("readonly", false);
            $("#P81NonPlanType").val(0).prop("readonly", false);
            $("#P81NonPlanType").css("pointer-events", "").css("background-color", "");
            let now = new Date();
            var datePart = now.toISOString().split("T")[0];       
            var timePart = now.toTimeString().substring(0, 5);
            $("#P81ReqStartDate").val(datePart).prop("readonly", false);
            $("#P81ReqStartTime").val(timePart).prop("readonly", false);
            $("#P81ReqDur").val('').prop("readonly", false);
            $("#P81Save1").prop("disabled", false);
            $("#P81Select").prop("disabled", false);

        } else if (actstartdate === "0001-01-01T00:00:00") {
            $("#Div2Act").show();
            $("#Div2Acthr").show();

            $("#P81MachinceSel2").val(mcname).prop("readonly", true);
            $("#P81MachinceSel1").val(shopname).prop("readonly", true);
            $("#P81WorkDesc").val(workdesc).prop("readonly", true);
            $("#P81NonPlanType").val(nontype).prop("readonly", true);
            $("#P81NonPlanType").css("pointer-events", "none").css("background-color", "#e9ecef");
            let now = new Date();
            var datePart = now.toISOString().split("T")[0];        // today's date
            var timePart = now.toTimeString().substring(0, 5);      // current time

            $("#P81ReqStartDate").val(datePart).prop("readonly", true);
            $("#P81ReqStartTime").val(timePart).prop("readonly", true);
            $("#P81ReqDur").prop("readonly", true);   // Assuming duration = timePart?
            $("#P81Select").prop("disabled", true);
            $("#P81Save1").prop("disabled", true);

            $("#P81ActDate1").val(datePart).prop("readonly", false);
            $("#P81ActTime1").val(timePart).prop("readonly", false);
            $("#P81Save2").prop("disabled", false);
            if (reqddate) {
                var parts = reqddate.split("T");
                var rdatePart = parts[0];                // "2025-06-09"
                var rtimePart = parts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

                $("#P81ReqStartDate").val(rdatePart);
                $("#P81ReqStartTime").val(rtimePart);
            }
        } else {
            // actstartdate has a real value – use it to prefill fields
            $("#Div2Act").show();
            $("#Div2Acthr").show();
            $("#Div3Close").show();
            $("#Div3Closehr").show();
            var parts = actstartdate.split("T");
            var datePart = parts[0];                // e.g., "2025-06-09"
            var timePart = parts[1].substring(0, 5); // e.g., "12:00"

            $("#P81MachinceSel2").val(mcname).prop("readonly", true);
            $("#P81MachinceSel1").val(shopname).prop("readonly", true);
            $("#P81WorkDesc").val(workdesc).prop("readonly", true);
            $("#P81NonPlanType").val(nontype).prop("readonly", true);
            $("#P81NonPlanType").css("pointer-events", "none").css("background-color", "#e9ecef");
            if (reqddate) {
                var parts = reqddate.split("T");
                var rdatePart = parts[0];                // "2025-06-09"
                var rtimePart = parts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

                $("#P81ReqStartDate").val(rdatePart).prop("readonly", true);
                $("#P81ReqStartTime").val(rtimePart).prop("readonly", true);
            }
            $("#P81ReqDur").prop("readonly", true); // if duration is same as timePart?
            $("#P81Select").prop("disabled", true);
            $("#P81Save1").prop("disabled", true);
            $("#P81Save2").prop("disabled", true);

            $("#P81ActDate1").val(datePart).prop("readonly", true);
            $("#P81ActTime1").val(timePart).prop("readonly", true);
        }

        //loadMachinceType();
    });
    $('#Popup82').on('show.bs.modal', function (event) {
        document.getElementById('Popup81').style.filter = 'blur(5px)';
        //loadMachinceType();
    });
    $('#Popup82').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup81').style.filter = 'none';
        $("#P82McType").val('0');
        $("#P82Shop").val('0');
    });
    $("#P82McType").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P82Grid tbody tr").show();
        }
        else {
            selectedValue = selectedValue.toLowerCase();
            $("#P82Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selectedValue) > -1)
            });// show only the filtered rows
        }
    });
    $("#P82Shop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P82Grid tbody tr").show();
        }
        else {
            selectedValue = selectedValue.toLowerCase();
            $("#P82Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(selectedValue) > -1)
            });// show only the filtered rows
        }
    });
    $("#P82Select").on("click", function () {
        $("#Popup82").modal("hide");
    });
    $("#ErBtn1").on('click', function (event) {
        if ($("#openfrom").val() === "P81") {
            var P81ActDate1 = $("#P81ActDate1").val();
            var P81ActEndDate1 = $("#P81ActEndDate1").val();
            if (P81ActDate1.length === 0)  {
                $("#P81Save1").click();
            } else if (P81ActEndDate1.length === 0){
                $("#P81Save2").click();
            } else {
                $("#P81Save3").click();
            }
        } else if ($("#openfrom").val() === "P26") {
            $("#P26Save").click();
        }
        $("#ErrorMessage1").modal("hide");
    });
    $("#ErBtn2").on('click', function (event) {
        $("#ErrorMessage1").modal("hide");
    });
    $("#ErBtn3").on('click', function (event) {
        isP81Modified = false;
        closeP26 = 1;
        $("#Popup81").modal("hide");
        $("#Popup26").modal("hide");
        $("#ErrorMessage1").modal("hide");
    });
    $("#P81Save1").on('click', function (event) {
        var P81NonPlandId = parseInt($("#P81NonPlandId").val());
        var P81MachinceId = parseInt($("#P81MachinceId").val());
        var P81NonPlanType = parseInt($("#P81NonPlanType").val());
        var P81WorkDesc = $("#P81WorkDesc").val();
        var P81ReqStartDate = $("#P81ReqStartDate").val();
        var P81ReqStartTime = $("#P81ReqStartTime").val();
        var P81ReqDur = $("#P81ReqDur").val();
        if (isNaN(P81MachinceId) || P81MachinceId === 0) {
            var newNamevalidate = document.getElementById('P81MachinceSel1');
            newNamevalidate.style.border = '2px solid red';
            var newNamevalidate = document.getElementById('P81MachinceSel2');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81MachinceSel1');
            newNamevalidate.style.border = '';
            var newNamevalidate = document.getElementById('P81MachinceSel2');
            newNamevalidate.style.border = '';
        }
        if (P81WorkDesc.length === 0) {
            var newNamevalidate = document.getElementById('P81WorkDesc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81WorkDesc');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P81NonPlanType) || P81NonPlanType === 0) {
            var newNamevalidate = document.getElementById('P81NonPlanType');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81NonPlanType');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartDate.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqStartDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqStartDate');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartTime.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqStartTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqStartTime');
            newNamevalidate.style.border = '';
        }
        if (P81ReqDur.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            const durParts = P81ReqDur.split(":");
            let durValid = false;

            if (durParts.length === 2) {
                const hours = parseInt(durParts[0], 10);
                const minutes = parseInt(durParts[1], 10);

                // Check if valid numbers and not both zero
                if (!isNaN(hours) && !isNaN(minutes) && (hours > 0 || minutes > 0)) {
                    durValid = true;
                }
            }

            if (!durValid) {
                var durInput = document.getElementById('P81ReqDur');
                durInput.style.border = '2px solid red';
                alert("Please enter a valid Required Duration (not 00:00)");
                return false;
            } else {
                document.getElementById('P81ReqDur').style.border = '';
            }

            var newNamevalidate = document.getElementById('P81ReqDur');
            newNamevalidate.style.border = '';
        }
        var combinedDateTimeStr = P81ReqStartDate + "T" + P81ReqStartTime;
        var plannedDateTime = new Date(combinedDateTimeStr);
        var now = new Date();
        if (plannedDateTime <= now) {
            alert("Required Start Date / Time  must be greater than the Today's Date / Time.");
            return;
        }
        if (isNaN(P81NonPlandId)) {
            P81NonPlandId = 0;
        }
        var formdata = {
            non_Plan_Wk_ListId: P81NonPlandId,
            mc_Id: P81MachinceId,
            non_Plan_Wk_Type: P81NonPlanType,
            plan_start_time: combinedDateTimeStr,
            plan_Duration: P81ReqDur,
            work_Description: P81WorkDesc
        };
        api.post("/WorkOrder/PostNon_Plan_Wk_List", formdata).then((data) => {
            alert("Non Plan Work Saved");
            isP81Modified = false;
            $("#Popup81").modal("hide");
            loadNonWrkList();
        }).catch((error) => {
            console.log(error);
        });

    });
    $("#P81Save2").on('click', function (event) {
        var P81NonPlandId = parseInt($("#P81NonPlandId").val());
        var P81MachinceId = parseInt($("#P81MachinceId").val());
        var P81NonPlanType = parseInt($("#P81NonPlanType").val());
        var P81WorkDesc = $("#P81WorkDesc").val();
        var P81ReqStartDate = $("#P81ReqStartDate").val();
        var P81ReqStartTime = $("#P81ReqStartTime").val();
        var P81ReqDur = $("#P81ReqDur").val();
        var P81ActDate1 = $("#P81ActDate1").val();
        var P81ActTime1 = $("#P81ActTime1").val();
        if (isNaN(P81MachinceId) || P81MachinceId === 0) {
            var newNamevalidate = document.getElementById('P81MachinceSel1');
            newNamevalidate.style.border = '2px solid red';
            var newNamevalidate = document.getElementById('P81MachinceSel2');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81MachinceSel1');
            newNamevalidate.style.border = '';
            var newNamevalidate = document.getElementById('P81MachinceSel2');
            newNamevalidate.style.border = '';
        }
        if (P81WorkDesc.length === 0) {
            var newNamevalidate = document.getElementById('P81WorkDesc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81WorkDesc');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P81NonPlanType) || P81NonPlanType === 0) {
            var newNamevalidate = document.getElementById('P81NonPlanType');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81NonPlanType');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartDate.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqStartDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqStartDate');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartTime.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqStartTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqStartTime');
            newNamevalidate.style.border = '';
        }
        if (P81ReqDur.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqDur');
            newNamevalidate.style.border = '';
        }
        if (P81ActDate1.length === 0) {
            var newNamevalidate = document.getElementById('P81ActDate1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ActDate1');
            newNamevalidate.style.border = '';
        }
        if (P81ActTime1.length === 0) {
            var newNamevalidate = document.getElementById('P81ActTime1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ActTime1');
            newNamevalidate.style.border = '';
        }
        var combinedDateTimeStr = P81ReqStartDate + "T" + P81ReqStartTime;
        var actcombinedDateTimeStr = P81ActDate1 + "T" + P81ActTime1;
        var plannedDateTime = new Date(combinedDateTimeStr);
        var actualDateTime = new Date(actcombinedDateTimeStr);

        if (actualDateTime <= plannedDateTime) {
            alert("Actual Start Date / Time should be greater than Required Start Date / Time .");
            return false;
        }
        if (isNaN(P81NonPlandId)) {
            P81NonPlandId = 0;
        }
        var formdata = {
            non_Plan_Wk_ListId: P81NonPlandId,
            mc_Id: P81MachinceId,
            non_Plan_Wk_Type: P81NonPlanType,
            plan_start_time: combinedDateTimeStr,
            plan_Duration: P81ReqDur,
            work_Description: P81WorkDesc,
            actual_Start_Time: actcombinedDateTimeStr
        };
        api.post("/WorkOrder/PostNon_Plan_Wk_List", formdata).then((data) => {
            alert("Non Plan Work Saved");
            isP81Modified = false;
            $("#Popup81").modal("hide");
            loadNonWrkList();
        }).catch((error) => {
            console.log(error);
        });

    });
    $("#P81Save3").on('click', function (event) {
        var P81NonPlandId = parseInt($("#P81NonPlandId").val());
        var P81MachinceId = parseInt($("#P81MachinceId").val());
        var P81NonPlanType = parseInt($("#P81NonPlanType").val());
        var P81WorkDesc = $("#P81WorkDesc").val();
        var P81ReqStartDate = $("#P81ReqStartDate").val();
        var P81ReqStartTime = $("#P81ReqStartTime").val();
        var P81ReqDur = $("#P81ReqDur").val();
        var P81ActDate1 = $("#P81ActDate1").val();
        var P81ActTime1 = $("#P81ActTime1").val();
        var P81CloseCom1 = $("#P81CloseCom1").val();
        var P81ActEndDate1 = $("#P81ActEndDate1").val();
        var P81ActEndTime1 = $("#P81ActEndTime1").val();
        var P81ActDur = $("#P81ActDur").val();
        if (isNaN(P81MachinceId) || P81MachinceId === 0) {
            var newNamevalidate = document.getElementById('P81MachinceSel1');
            newNamevalidate.style.border = '2px solid red';
            var newNamevalidate = document.getElementById('P81MachinceSel2');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81MachinceSel1');
            newNamevalidate.style.border = '';
            var newNamevalidate = document.getElementById('P81MachinceSel2');
            newNamevalidate.style.border = '';
        }
        if (P81WorkDesc.length === 0) {
            var newNamevalidate = document.getElementById('P81WorkDesc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81WorkDesc');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P81NonPlanType) || P81NonPlanType === 0) {
            var newNamevalidate = document.getElementById('P81NonPlanType');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81NonPlanType');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartDate.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqStartDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqStartDate');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartTime.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqStartTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqStartTime');
            newNamevalidate.style.border = '';
        }
        if (P81ReqDur.length === 0) {
            var newNamevalidate = document.getElementById('P81ReqDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ReqDur');
            newNamevalidate.style.border = '';
        }
        if (P81ActDate1.length === 0) {
            var newNamevalidate = document.getElementById('P81ActDate1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ActDate1');
            newNamevalidate.style.border = '';
        }
        if (P81ActTime1.length === 0) {
            var newNamevalidate = document.getElementById('P81ActTime1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ActTime1');
            newNamevalidate.style.border = '';
        }
        if (P81CloseCom1.length === 0) {
            var newNamevalidate = document.getElementById('P81CloseCom1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81CloseCom1');
            newNamevalidate.style.border = '';
        }
        if (P81ActEndDate1.length === 0) {
            var newNamevalidate = document.getElementById('P81ActEndDate1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ActEndDate1');
            newNamevalidate.style.border = '';
        }
        if (P81ActEndTime1.length === 0) {
            var newNamevalidate = document.getElementById('P81ActEndTime1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ActEndTime1');
            newNamevalidate.style.border = '';
        }
        if (P81ActDur.length === 0) {
            var newNamevalidate = document.getElementById('P81ActDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P81ActDur');
            newNamevalidate.style.border = '';
        }
        var combinedDateTimeStr = P81ReqStartDate + "T" + P81ReqStartTime;
        var actcombinedDateTimeStr = P81ActDate1 + "T" + P81ActTime1;
        var actendcombinedDateTimeStr = P81ActEndDate1 + "T" + P81ActEndTime1;
        if (isNaN(P81NonPlandId)) {
            P81NonPlandId = 0;
        }
        var formdata = {
            non_Plan_Wk_ListId: P81NonPlandId,
            mc_Id: P81MachinceId,
            non_Plan_Wk_Type: P81NonPlanType,
            plan_start_time: combinedDateTimeStr,
            plan_Duration: P81ReqDur,
            work_Description: P81WorkDesc,
            actual_Start_Time: actcombinedDateTimeStr,
            actual_end_Time: actendcombinedDateTimeStr,
            closure_Comment: P81CloseCom1,
            actual_Duration: P81ActDur
        };
        api.post("/WorkOrder/PostNon_Plan_Wk_List", formdata).then((data) => {
            alert("Non Plan Work Saved");
            isP81Modified = false;
            loadNonWrkList();
            $("#Popup81").modal("hide");
        }).catch((error) => {
            console.log(error);
        });

    });
    $('#Popup81 input, #Popup81 textarea, #Popup81 select').on('change input', function () {
        isP81Modified = true;
    });

    // Reset flag on Save buttons
    $('#P81Save1, #P81Save2, #P81Save3').on('click', function () {
        isP81Modified = false;
    });
    $('#P11Save1, #P81Save2, #P11Save3').on('click', function () {
        isP11Modified = false;
    });
    $('#P81ActEndDate1, #P81ActEndTime1').on('change', function () {
        calculateActualDuration();
    });
    $('#P11ActEndDate1, #P11ActEndTime1').on('change', function () {
        calculateActualDurationP11();
    });

    $('#Popup10').on('show.bs.modal', function (event) {
        loadReworkList();
        loadMachinceType();
        loadMachinceList();
    });
    loadSimulationWos();
    $("#P81Select").on('click', function (event) {
        mcAdd = 0;
    });
    $("#P11Select").on('click', function (event) {
        mcAdd = 11;
    });
    $('#Popup11').on('hide.bs.modal', function (event) {
        document.getElementById('Popup10').style.filter = 'none';
        var P81MachinceSel2 = document.getElementById('P11MachinceSel2');
        P81MachinceSel2.style.border = '';
        var P81MachinceSel1 = document.getElementById('P11MachinceSel1');
        P81MachinceSel1.style.border = '';
        var P81ReqStartDate = document.getElementById('P11ReqDt');
        P81ReqStartDate.style.border = '';
        var P81ReqStartTime = document.getElementById('P11RqTime');
        P81ReqStartTime.style.border = '';
        var P81ReqDur = document.getElementById('P11ReqDur');
        P81ReqDur.style.border = '';
        var P81ActDate1 = document.getElementById('P11ActDate1');
        P81ActDate1.style.border = '';
        var P81ActTime1 = document.getElementById('P11ActTime1');
        P81ActTime1.style.border = '';
        var P81CloseCom1 = document.getElementById('P11CloseCom');
        P81CloseCom1.style.border = '';
        var P81ActEndDate1 = document.getElementById('P11ActEndDate1');
        P81ActEndDate1.style.border = '';
        var P81ActEndTime1 = document.getElementById('P11ActEndTime1');
        P81ActEndTime1.style.border = '';
        var P81ActDur = document.getElementById('P11ActDur');
        P81ActDur.style.border = '';
        if (isP11Modified) {
            event.preventDefault();
            $("#openfrom").val("P81");
            $("#ErrorMessage1").modal("show");
        }
    });
    $('#Popup11').on('show.bs.modal', function (event) {
        showPopup11();
        document.getElementById('Popup10').style.filter = 'blur(5px)';
        $("#Div11Act").hide();
        $("#Div11Close").hide();
        $("#Div11ActHr").hide();
        $("#Div11CloseHr").hide();
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var mcid = relatedTarget.data("mcid");
        var mcname = relatedTarget.data("mcname");
        var shopname = relatedTarget.data("shopname");
        var reqddur = relatedTarget.data("reqddur");
        var reqddate = relatedTarget.data("reqddate");
        var actstartdate = relatedTarget.data("actstartdate");
        var actdur = relatedTarget.data("actdur");
        var actenddate = relatedTarget.data("actenddate");
        var partno = relatedTarget.data("partno");
        var wonum = relatedTarget.data("wonum");
        var rwkqnty = relatedTarget.data("rwkqnty");
        var rout = relatedTarget.data("rout");
        var closecom = relatedTarget.data("closecom");
        var pov = relatedTarget.data("pov");
        var nclogid = relatedTarget.data("nclogid");
        $("#P11ActDur").val(actdur).prop("readonly", true);

        $("#P11PartNo").text(partno);
        $("#P11Rout").text(rout);
        $("#P11Wo").text(wonum);
        $("#P11RwkQnty").text(rwkqnty);
        $("#P11RwkPlanId").val(id);
        $("#NlogIdP11").val(nclogid);
        $("#P11MachinceId").val(mcid);
        $("#P11MachinceSel2").val(mcname);
        $("#P11MachinceSel1").val(shopname);
        if (reqddate === "2001-01-01T00:00:00" || reqddate === "0001-01-01T00:00:00") {
            actstartdate = null;
            reqddate = null;
        }
        if (reqddate) {
            var parts = reqddate.split("T");
            var datePart = parts[0];                // "2025-06-09"
            var timePart = parts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

            $("#P11ReqDt").val(datePart);
            $("#P11RqTime").val(timePart);
        }
        $("#P11ReqDur").val(reqddur);
        if (closecom === null || !closecom) {
            $("#Div11Close").hide();
            $("#Div11CloseHr").hide()
        } else {
            $("#Div11Close").show();
            $("#Div11CloseHr").show();
            $("#P11CloseCom").val(closecom);
            $("#P11ActDur").val(actdur).prop("readonly", true);
            if (actenddate) {
                var eparts = actenddate.split("T");
                var edatePart = eparts[0];                // "2025-06-09"
                var etimePart = eparts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

                $("#P11ActEndDate1").val(edatePart);
                $("#P11ActEndTime1").val(etimePart);
            }
        }
        if (!actstartdate) {
            $("#P11MachinceSel2").val(mcname).prop("readonly", true);
            $("#P11MachinceSel1").val(shopname).prop("readonly", true);
            let now = new Date();
            var datePart = now.toISOString().split("T")[0];
            var timePart = now.toTimeString().substring(0, 5);
            $("#P11ReqDt").val(datePart).prop("readonly", false);
            $("#P11RqTime").val(timePart).prop("readonly", false);
            $("#P11ReqDur").val('').prop("readonly", false);
            $("#P11Save1").prop("disabled", false);
            $("#P11Select").prop("disabled", false);

        } else if (actstartdate === "0001-01-01T00:00:00") {
            $("#Div11Act").show();
            $("#Div11ActHr").show();

            $("#P11MachinceSel2").val(mcname).prop("readonly", true);
            $("#P11MachinceSel1").val(shopname).prop("readonly", true);
            let now = new Date();
            var datePart = now.toISOString().split("T")[0];        // today's date
            var timePart = now.toTimeString().substring(0, 5);      // current time

            $("#P11ReqDt").val(datePart).prop("readonly", true);
            $("#P11RqTime").val(timePart).prop("readonly", true);
            $("#P11ReqDur").prop("readonly", true);   // Assuming duration = timePart?
            $("#P11Select").prop("disabled", true);
            $("#P11Save1").prop("disabled", true);

            $("#P11ActDate1").val(datePart).prop("readonly", false);
            $("#P11ActTime1").val(timePart).prop("readonly", false);
            $("#P11Save2").prop("disabled", false);
            if (reqddate) {
                var parts = reqddate.split("T");
                var rdatePart = parts[0];                // "2025-06-09"
                var rtimePart = parts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

                $("#P11ReqDt").val(rdatePart);
                $("#P11RqTime").val(rtimePart);
            }
        } else {
            // actstartdate has a real value – use it to prefill fields
            $("#Div11Act").show();
            $("#Div11ActHr").show();
            $("#Div11Close").show();
            $("#Div11CloseHr").show();
            var parts = actstartdate.split("T");
            var datePart = parts[0];                // e.g., "2025-06-09"
            var timePart = parts[1].substring(0, 5); // e.g., "12:00"

            $("#P11MachinceSel2").val(mcname).prop("readonly", true);
            $("#P11MachinceSel1").val(shopname).prop("readonly", true);
            if (reqddate) {
                var parts = reqddate.split("T");
                var rdatePart = parts[0];                // "2025-06-09"
                var rtimePart = parts[1].substring(0, 5); // "12:00" (first 5 chars of "12:00:00")

                $("#P11ReqDt").val(rdatePart).prop("readonly", true);
                $("#P11RqTime").val(rtimePart).prop("readonly", true);
            }
            $("#P11ReqDur").prop("readonly", true); // if duration is same as timePart?
            $("#P11Select").prop("disabled", true);
            $("#P11Save1").prop("disabled", true);
            $("#P11Save2").prop("disabled", true);

            $("#P11ActDate1").val(datePart).prop("readonly", true);
            $("#P11ActTime1").val(timePart).prop("readonly", true);
        }
        if (pov === "view") {
            // Make all input fields readonly or disabled
            $('#Popup11')
                .find(':input')
                .not('.btn-close, #PopupNC10Btn')
                .prop('readonly', true)
                .prop('disabled', true);

            // Also disable select fields but still exclude the close button
            $('#Popup11 select:not(.btn-close)').prop('disabled', true);


            // Hide all Save buttons
            $("#P11Save1").hide();
            $("#P11Save2").hide();
            $("#P11Select").hide();
        } else {
            // In non-view mode, ensure Save buttons are visible (if needed)
            $('#Popup11')
                .find(':input')
                .not('.btn-close, #PopupNC10Btn')
                .prop('readonly', false)
                .prop('disabled', false);
            $('#Popup11 select:not(.btn-close)').prop('disabled', false);
            $("#P11Save1").show();
            $("#P11Save2").show();
            $("#P11Select").show();
        }

        //loadMachinceType();
    });
    $("#P11Save1").on('click', function (event) {
        var P81NonPlandId = parseInt($("#P11RwkPlanId").val());
        var P11MachinceId = parseInt($("#P11MachinceId").val());
        var P81ReqStartDate = $("#P11ReqDt").val();
        var P81ReqStartTime = $("#P11RqTime").val();
        var P81ReqDur = $("#P11ReqDur").val();
        if (isNaN(P11MachinceId) || P11MachinceId === 0) {
            var newNamevalidate = document.getElementById('P11MachinceSel1');
            newNamevalidate.style.border = '2px solid red';
            var newNamevalidate = document.getElementById('P11MachinceSel2');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11MachinceSel1');
            newNamevalidate.style.border = '';
            var newNamevalidate = document.getElementById('P11MachinceSel2');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartDate.length === 0) {
            var newNamevalidate = document.getElementById('P11ReqDt');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ReqDt');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartTime.length === 0) {
            var newNamevalidate = document.getElementById('P11RqTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11RqTime');
            newNamevalidate.style.border = '';
        }
        if (P81ReqDur.length === 0) {
            var newNamevalidate = document.getElementById('P11ReqDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            const durParts = P81ReqDur.split(":");
            let durValid = false;

            if (durParts.length === 2) {
                const hours = parseInt(durParts[0], 10);
                const minutes = parseInt(durParts[1], 10);

                // Check if valid numbers and not both zero
                if (!isNaN(hours) && !isNaN(minutes) && (hours > 0 || minutes > 0)) {
                    durValid = true;
                }
            }

            if (!durValid) {
                var durInput = document.getElementById('P11ReqDur');
                durInput.style.border = '2px solid red';
                alert("Please enter a valid Required Duration (not 00:00)");
                return false;
            } else {
                document.getElementById('P11ReqDur').style.border = '';
            }

            var newNamevalidate = document.getElementById('P11ReqDur');
            newNamevalidate.style.border = '';
        }
        var combinedDateTimeStr = P81ReqStartDate + "T" + P81ReqStartTime;
        var plannedDateTime = new Date(combinedDateTimeStr);
        var now = new Date();
        if (plannedDateTime <= now) {
            alert("Required Start Date / Time  must be greater than the Today's Date / Time.");
            return;
        }
        if (isNaN(P81NonPlandId)) {
            P81NonPlandId = 0;
        }
        var formdata = {
            rwk_ListId: P81NonPlandId,
            mc_Id: P11MachinceId,
            requiredStartTime: combinedDateTimeStr,
            plan_Duration: P81ReqDur
        };
        api.post("/WorkOrder/PostRwk_List", formdata).then((data) => {
            alert("ReWork Saved");
            isP11Modified = false;
            $("#Popup11").modal("hide");
            loadReworkList();
        }).catch((error) => {
            console.log(error);
        });

    });
    $("#P11Save2").on('click', function (event) {
        var P81NonPlandId = parseInt($("#P11RwkPlanId").val());
        var P81MachinceId = parseInt($("#P11MachinceId").val());
        var P81ReqStartDate = $("#P11ReqDt").val();
        var P81ReqStartTime = $("#P11RqTime").val();
        var P81ReqDur = $("#P11ReqDur").val();
        var P81ActDate1 = $("#P11ActDate1").val();
        var P81ActTime1 = $("#P11ActTime1").val();
        if (isNaN(P81MachinceId) || P81MachinceId === 0) {
            var newNamevalidate = document.getElementById('P11MachinceSel1');
            newNamevalidate.style.border = '2px solid red';
            var newNamevalidate = document.getElementById('P11MachinceSel2');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11MachinceSel1');
            newNamevalidate.style.border = '';
            var newNamevalidate = document.getElementById('P11MachinceSel2');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartDate.length === 0) {
            var newNamevalidate = document.getElementById('P11ReqDt');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ReqDt');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartTime.length === 0) {
            var newNamevalidate = document.getElementById('P11RqTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11RqTime');
            newNamevalidate.style.border = '';
        }
        if (P81ReqDur.length === 0) {
            var newNamevalidate = document.getElementById('P11ReqDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ReqDur');
            newNamevalidate.style.border = '';
        }
        if (P81ActDate1.length === 0) {
            var newNamevalidate = document.getElementById('P11ActDate1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ActDate1');
            newNamevalidate.style.border = '';
        }
        if (P81ActTime1.length === 0) {
            var newNamevalidate = document.getElementById('P11ActTime1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ActTime1');
            newNamevalidate.style.border = '';
        }
        var combinedDateTimeStr = P81ReqStartDate + "T" + P81ReqStartTime;
        var actcombinedDateTimeStr = P81ActDate1 + "T" + P81ActTime1;
        var plannedDateTime = new Date(combinedDateTimeStr);
        var actualDateTime = new Date(actcombinedDateTimeStr);

        if (actualDateTime <= plannedDateTime) {
            alert("Actual Start Date / Time should be greater than Required Start Date / Time .");
            return false;
        }
        if (isNaN(P81NonPlandId)) {
            P81NonPlandId = 0;
        }
        var formdata = {
            rwk_ListId: P81NonPlandId,
            mc_Id: P81MachinceId,
            requiredStartTime: combinedDateTimeStr,
            plan_Duration: P81ReqDur,
            actual_Start_Time: actcombinedDateTimeStr
        };
        api.post("/WorkOrder/PostRwk_List", formdata).then((data) => {
            alert("Rework Saved");
            isP11Modified = false;
            $("#Popup11").modal("hide");
            loadReworkList();
        }).catch((error) => {
            console.log(error);
        });

    });
    $("#P11Save3").on('click', function (event) {
        var P81NonPlandId = parseInt($("#P11RwkPlanId").val());
        var P81MachinceId = parseInt($("#P11MachinceId").val());
        var P81ReqStartDate = $("#P11ReqDt").val();
        var P81ReqStartTime = $("#P11RqTime").val();
        var P81ReqDur = $("#P11ReqDur").val();
        var P81ActDate1 = $("#P11ActDate1").val();
        var P81ActTime1 = $("#P11ActTime1").val();
        var P81CloseCom1 = $("#P11CloseCom").val();
        var P81ActEndDate1 = $("#P11ActEndDate1").val();
        var P81ActEndTime1 = $("#P11ActEndTime1").val();
        var P81ActDur = $("#P11ActDur").val();
        if (isNaN(P81MachinceId) || P81MachinceId === 0) {
            var newNamevalidate = document.getElementById('P11MachinceSel1');
            newNamevalidate.style.border = '2px solid red';
            var newNamevalidate = document.getElementById('P11MachinceSel2');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11MachinceSel1');
            newNamevalidate.style.border = '';
            var newNamevalidate = document.getElementById('P11MachinceSel2');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartDate.length === 0) {
            var newNamevalidate = document.getElementById('P11ReqDt');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ReqDt');
            newNamevalidate.style.border = '';
        }
        if (P81ReqStartTime.length === 0) {
            var newNamevalidate = document.getElementById('P11RqTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11RqTime');
            newNamevalidate.style.border = '';
        }
        if (P81ReqDur.length === 0) {
            var newNamevalidate = document.getElementById('P11ReqDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ReqDur');
            newNamevalidate.style.border = '';
        }
        if (P81ActDate1.length === 0) {
            var newNamevalidate = document.getElementById('P11ActDate1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ActDate1');
            newNamevalidate.style.border = '';
        }
        if (P81ActTime1.length === 0) {
            var newNamevalidate = document.getElementById('P11ActTime1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ActTime1');
            newNamevalidate.style.border = '';
        }
        if (P81CloseCom1.length === 0) {
            var newNamevalidate = document.getElementById('P11CloseCom');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11CloseCom');
            newNamevalidate.style.border = '';
        }
        if (P81ActEndDate1.length === 0) {
            var newNamevalidate = document.getElementById('P11ActEndDate1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ActEndDate1');
            newNamevalidate.style.border = '';
        }
        if (P81ActEndTime1.length === 0) {
            var newNamevalidate = document.getElementById('P11ActEndTime1');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ActEndTime1');
            newNamevalidate.style.border = '';
        }
        if (P81ActDur.length === 0) {
            var newNamevalidate = document.getElementById('P11ActDur');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P11ActDur');
            newNamevalidate.style.border = '';
        }
        var combinedDateTimeStr = P81ReqStartDate + "T" + P81ReqStartTime;
        var actcombinedDateTimeStr = P81ActDate1 + "T" + P81ActTime1;
        var actendcombinedDateTimeStr = P81ActEndDate1 + "T" + P81ActEndTime1;
        if (isNaN(P81NonPlandId)) {
            P81NonPlandId = 0;
        }
        var formdata = {
            rwk_ListId: P81NonPlandId,
            mc_Id: P81MachinceId,
            requiredStartTime: combinedDateTimeStr,
            plan_Duration: P81ReqDur,
            actual_Start_Time: actcombinedDateTimeStr,
            actual_end_Time: actendcombinedDateTimeStr,
            closure_Comment: P81CloseCom1,
            actual_Duration: P81ActDur
        };
        api.post("/WorkOrder/PostRwk_List", formdata).then((data) => {
            alert("Rework Saved");
            isP11Modified = false;
            loadReworkList();
            $("#Popup11").modal("hide");
        }).catch((error) => {
            console.log(error);
        });

    });
    $("#CancelBtn").on('click', function (event) {
        let confirmval = confirm("Are your sure you want to Cancel Simulation ?", "Yes", "No");
        if (confirmval) {
            $("#preloader").show();
            document.getElementById('preloader').style.display = 'block';
            document.getElementById('status').style.display = 'block';
            api.get("/workOrder/ClearTemp").then((data) => {
                loadSimulationWos();
            }).catch((error) => {

            });
        }
    });
    $('#Popup26').on('hide.bs.modal', function (event) {
        if (originalRoutingId !== currentRoutingId && closeP26 === 0) {
            event.preventDefault();
            $("#openfrom").val("P26");
            $("#ErrorMessage1").modal("show");
        }
    });
    $('#Popup26').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        closeP26 = 0;
        var partid = relatedTarget.data("partid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var ppid = relatedTarget.data("ppid");
        var calwoqty = relatedTarget.data("calwoqty");
        var routingid = relatedTarget.data("routingid");
        currentRoutingId = routingid;
        $("#P26PartNo").text(partno + " / " + partdesc);
        $("#P26proQnty").text(calwoqty);
        $("#P26Woid").val(ppid);
        loadRouting(partid, parseInt(calwoqty), routingid);
    });
    $("#P26Save").on('click', function (event) {
        var selectedCheckboxes = $("#P26Grid .P26GridChk:checked");

        if (selectedCheckboxes.length !== 1) {
            alert("Please select exactly one routing option.");
            return;
        }

        var pwoid = parseInt($("#P26Woid").val());

        // Get the selected routingId from the same row as the checked checkbox
        var selectedRow = selectedCheckboxes.closest("tr");
        var routingId = selectedRow.find("[data-routingid]").data("routingid");
        originalRoutingId = parseInt(routingId);
        currentRoutingId = parseInt(routingId);
        // Call API
        api.get("/workOrder/UpdateProdWo?pwoid=" + pwoid + "&routingId=" + routingId).then((data) => {
            loadSimulationWos();
            $("#Popup26").modal("hide");
        }).catch((error) => {
            console.error("Update failed", error);
        });
    });
    $('#Popup27').on('show.bs.modal', function (event) {
        loadMoveMatl();
    });
    $('#Popup27').on('hidden.bs.modal', function (event) {
    });
    $('#Popup28').on('show.bs.modal', function (event) {
        loadMcWaitSetup();
    });
    $('#Popup28').on('hidden.bs.modal', function (event) {
    });
    $('#Popup3').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup26').style.filter = 'none';
    });
    $('#Popup3').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        document.getElementById('Popup26').style.filter = 'blur(5px)';
        var routingid = relatedTarget.data("routingid");
        var routingname = relatedTarget.data("routingname");
        $("#P3PartNo").text($("#P26PartNo").text());
        $("#P3proQnty").text($("#P26proQnty").text());
        $("#P3Routing").text(routingname);
        loadRoutingStep(routingid, parseInt($("#P26proQnty").text()));
    });
    $(document).on('change', '.P26GridChk', function () {
        if (this.checked) {
            $('.P26GridChk').not(this).prop('checked', false);
            currentRoutingId = $(this).data('routingid');
        }
    });
    $('#Popup1').on('show.bs.modal', function (event) {
        loadWoWaitlist();
    });
    $('#Popup13').on('show.bs.modal', function (event) {
        loadMcShopP13();
        loadNotAvlMcTimeslot();
    });
    $('#Popup14').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup13').style.filter = 'none';
        var P14Reason = document.getElementById('P14Reason');
        P14Reason.style.border = '';
        var P14ShopId = document.getElementById('P14ShopId');
        P14ShopId.style.border = '';
        var P14StartDate = document.getElementById('P14StartDate');
        P14StartDate.style.border = '';
        var P14StartTime = document.getElementById('P14StartTime');
        P14StartTime.style.border = '';
        var P14EndDate = document.getElementById('P14EndDate');
        P14EndDate.style.border = '';
        var P14EndTime = document.getElementById('P14EndTime');
        P14EndTime.style.border = '';
        var P14McId = document.getElementById('P14McId');
        P14McId.style.border = '';
    });
    $('#Popup14').on('show.bs.modal', function (event) {
        document.getElementById('Popup13').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var mcid = relatedTarget.data("mcid");
        var shopid = relatedTarget.data("shopid");
        var nonreasid = relatedTarget.data("nonreasid");
        var starttime = relatedTarget.data("starttime");
        var endtime = relatedTarget.data("endtime");
        if (starttime) {
            // Convert to JS Date object
            var parts = starttime.split(" ");
            var dateParts = parts[0].split("-"); // [dd, mm, yyyy]
            var timePart = parts[1] + " " + parts[2]; // "07:00 PM"

            var jsDate = new Date(`${dateParts[2]}-${dateParts[1]}-${dateParts[0]} ${timePart}`);

            // Format for input fields
            var dateValue = jsDate.toISOString().split("T")[0]; // yyyy-mm-dd
            var timeValue = jsDate.toTimeString().substring(0, 5); // hh:mm in 24hr

            // Set values to input fields
            $("#P14StartDate").val(dateValue);
            $("#P14StartTime").val(timeValue);
        }
        if (endtime) {
            // Convert to JS Date object
            var partse = endtime.split(" ");
            var datePartse = partse[0].split("-"); // [dd, mm, yyyy]
            var timeParte = partse[1] + " " + parts[2]; // "07:00 PM"

            var jsDatee = new Date(`${datePartse[2]}-${datePartse[1]}-${datePartse[0]} ${timeParte}`);

            // Format for input fields
            var dateValuee = jsDatee.toISOString().split("T")[0]; // yyyy-mm-dd
            var timeValuee = jsDatee.toTimeString().substring(0, 5); // hh:mm in 24hr

            // Set values to input fields
            $("#P14EndDate").val(dateValuee);
            $("#P14EndTime").val(timeValuee);
        } if (id > 0) {
            $("#P14McTmId").val(id);
            $("#P14ShopId").val(shopid);
            api.get("/machine/GetMachines").then((data) => {
                const filteredMachines = data.filter(m => m.shopId == shopid);

                const $mcDropdown = $("#P14McId");
                $mcDropdown.html(""); // clear first
                $mcDropdown.append('<option value="0">--Select Shop--</option>');

                filteredMachines.forEach(m => {
                    $mcDropdown.append(`<option value="${m.machineId}">${m.name}</option>`);
                });

                // Now set the mcid value after options are populated
                $mcDropdown.val(mcid);
            });
            $("#P14Reason").val(nonreasid);
        } else {
            $("#P14McTmId").val('');
            $("#P14ShopId").val(0);
            $("#P14McId").val(0);
            $("#P14Reason").val(0);
            $("#P14StartDate").val('');
            $("#P14StartTime").val('');
            $("#P14EndDate").val('');
            $("#P14EndTime").val('');
        }
    });
    loadCustomer();
    $("#P13SearchShop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P13Grid tbody tr").show();
            return;
        }
        var selvallowc = selectedValue.toLowerCase();
        $("#P13Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(selvallowc) > -1)
        });
    });
    $("#P13SearchMc").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P13Grid tbody tr").show();
            return;
        }
        var selvallowc = selectedValue.toLowerCase();
        $("#P13Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallowc) > -1)
        });
    });
    $("#P1SearchCustomer").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P1Grid tbody tr").show();
            return;
        }
        var selvallowc = selectedValue.toLowerCase();
        $("#P1Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(selvallowc) > -1)
        });
    });
    $("#P1SearchPartType").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P1Grid tbody tr").show();
            return;
        } else if (selectedValue == "1") {
            $("#P1Grid tbody tr").show();
            return;
        } else if (selectedValue == "2") {
            var selval = "Assembly";
            var selvallow = selval.toLowerCase();
            $("#P1Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallow) > -1)
            });
        } else if (selectedValue == "3") {
            var selvalc = "Child Part";
            var selvallowc = selvalc.toLowerCase();
            $("#P1Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallowc) > -1)
            });
        }
    });
    $("#P1SearchPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P1Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#P1SearchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P1Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#P14ShopId").on('change', function (event) {
        var selval = $("#P14ShopId").val();
        var P14McId = $("#P14McId");
        $(P14McId).html("");
        $(P14McId).append('<option value="0">--Select Shop--</option>');
        api.get("/machine/GetMachines").then((data) => {
            const filteredMachines = data.filter(m => m.shopId == selval);
            for (i = 0; i < filteredMachines.length; i++) {
                $(P14McId).append('<option value="' + data[i].machineId + '">' + data[i].name + '</option>');
            }
        }).catch((error) => {
            //console.log(error);
        });
    });
    $("#P14Save").on('click', async function (event) {
        var P14ShopId = parseInt($("#P14ShopId").val());
        var P14McId = parseInt($("#P14McId").val());
        var P14McTmId = parseInt($("#P14McTmId").val());
        var P14Reason = parseInt($("#P14Reason").val());
        var P14StartDate = $("#P14StartDate").val();
        var P14StartTime = $("#P14StartTime").val();
        var P14EndDate = $("#P14EndDate").val();
        var P14EndTime = $("#P14EndTime").val();
        if (P14ShopId === 0) {
            var newNamevalidate = document.getElementById('P14ShopId');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14ShopId');
            newNamevalidate.style.border = '';
        }
        if (P14McId === 0) {
            var newNamevalidate = document.getElementById('P14McId');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14McId');
            newNamevalidate.style.border = '';
        }
        if (P14StartDate.length === 0) {
            var newNamevalidate = document.getElementById('P14StartDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14StartDate');
            newNamevalidate.style.border = '';
        }
        if (P14StartTime.length === 0) {
            var newNamevalidate = document.getElementById('P14StartTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14StartTime');
            newNamevalidate.style.border = '';
        }
        if (P14EndDate.length === 0) {
            var newNamevalidate = document.getElementById('P14EndDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14EndDate');
            newNamevalidate.style.border = '';
        }
        if (P14EndTime.length === 0) {
            var newNamevalidate = document.getElementById('P14EndTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14EndTime');
            newNamevalidate.style.border = '';
        }
        if (P14Reason === 0) {
            var newNamevalidate = document.getElementById('P14Reason');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14Reason');
            newNamevalidate.style.border = '';
        }
        var combinedDateTimeStr = P14StartDate + "T" + P14StartTime;
        var combinedEndDateTimeStr = P14EndDate + "T" + P14EndTime;
        var plannedDateTime = new Date(combinedDateTimeStr);
        var plannedEndDateTime = new Date(combinedEndDateTimeStr);
        var now = new Date();
        if (plannedDateTime <= now) {
            alert("Start Date must be greater than the Today's Date.");
            return;
        }
        if (plannedEndDateTime <= plannedDateTime) {
            alert("End Date must be greater than the Start Date.");
            return;
        }
        if (isNaN(P14McTmId)) {
            P14McTmId = 0;
        }
        let existingList = [];
        try {
            existingList = await api.get("/WorkOrder/GetNotAvlMcTimeslot");
        } catch (err) {
            console.error("Failed to fetch existing list", err);
        }

        const duplicate = existingList.find(x => {
            // Convert API time "11-06-2025 07:30 PM" to Date object
            let [date, time, meridian] = x.startTime.split(" ");
            let [dd, mm, yyyy] = date.split("-");
            let formattedStr = `${yyyy}-${mm}-${dd} ${time} ${meridian}`;
            let apiStart = new Date(formattedStr);

            [date, time, meridian] = x.endTime.split(" ");
            [dd, mm, yyyy] = date.split("-");
            formattedStr = `${yyyy}-${mm}-${dd} ${time} ${meridian}`;
            let apiEnd = new Date(formattedStr);

            return x.mc_Id === P14McId &&
                apiStart.getTime() === plannedDateTime.getTime() &&
                apiEnd.getTime() === plannedEndDateTime.getTime();
        });

        if (duplicate) {
            alert("This Machine with same Start Date Time and End Date Time already exists.");
            return;
        }
        var formdata = {
            mc_Timeslot_List_Id: P14McTmId,
            mc_Id: P14McId,
            slot_Not_Avl: 'Y',
            GetStartTime: combinedDateTimeStr,
            GetEndTime: combinedEndDateTimeStr,
            not_Avl_reason: P14Reason
        };
        api.post("/WorkOrder/PostNotAvlMcTimeslot", formdata).then((data) => {
            alert("Machine Non Availability Slot Details Saved");
            $("#Popup14").modal("hide");
            loadNotAvlMcTimeslot();
        }).catch((error) => {
            console.log(error);
        });
    });
    $("#P13SwitchCheck").on('change', function () {
        let isGroupByDate = $(this).is(":checked"); // true if "Group by Date" is selected

        let $tbody = $("#P13Grid tbody");
        let rows = $tbody.find("tr").get();

        if (isGroupByDate) {
            rows.sort(function (a, b) {
                let aStart = $(a).find("td:eq(3)").text().trim(); // "Start Date / Time"
                let bStart = $(b).find("td:eq(3)").text().trim();

                let aDate = parseDateTime(aStart);
                let bDate = parseDateTime(bStart);

                return aDate - bDate;
            });
        } else {
            // Group by Shop then Machine
            rows.sort(function (a, b) {
                let aShop = $(a).find("td:eq(0)").text().trim().toLowerCase();
                let bShop = $(b).find("td:eq(0)").text().trim().toLowerCase();
                if (aShop < bShop) return -1;
                if (aShop > bShop) return 1;

                let aMc = $(a).find("td:eq(1)").text().trim().toLowerCase();
                let bMc = $(b).find("td:eq(1)").text().trim().toLowerCase();
                return aMc.localeCompare(bMc);
            });
        }

        $.each(rows, function (index, row) {
            $tbody.append(row); // Re-append in sorted order
        });
    });
    $("#ReSimulateBtn").hide();
    $('#ReSimulateBtn').on('click', function () {
        var selectedWOIds = [];

        $('#SimWoGrid tbody tr').each(function () {
            var isChanged = $(this).find('td:eq(15)').text().trim();  // 16th column: "Data Changed"
            var id = $(this).find('td:eq(16)').text().trim();         // 17th column: hidden WO ID

            if (isChanged === 'Y' && id && !isNaN(id)) {
                selectedWOIds.push(parseInt(id));
            }
        });

        if (selectedWOIds.length === 0) {
            alert("No Work Orders found to freeze.");
            return;
        }
        alert('Re-Simulation Has Started');
        isDataSaved = false;
        $('#preloadersim').show();
        $.ajax({
            type: "POST",
            url: '/WorkOrder/ReSimulateWOAllocation',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(selectedWOIds),
            dataType: "json",
            success: function (result) {
                if (result.message == "Re-Simulation completed.") {
                    alert("Re-Simulation completed.");
                } else {
                    alert("Re-Simulation stopped.");
                }
                isDataSaved = true;
                $("#Popup1").modal("hide");
                $('#preloadersim').hide();
                loadSimulationWos();
            }
        });
    });
    $('#FreezeBtn').on('click', function () {
        var selectedWOIds = [];

        $('#SimWoGrid tbody tr').each(function () {
            var id = $(this).find('td:eq(16)').text().trim(); // Hidden WO ID column
            if (id) {
                selectedWOIds.push(parseInt(id));
            }
        });

        if (selectedWOIds.length === 0) {
            alert("No Work Orders found to freeze.");
            return;
        }
        document.getElementById('preloader').style.display = 'block';
        document.getElementById('status').style.display = 'block';
        $.ajax({
            type: "POST",
            url: '/WorkOrder/FreezeSimulation',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(selectedWOIds),
            dataType: "json",
            success: function (result) {
                alert(result.message);
                document.getElementById('preloader').style.display = 'none';
                document.getElementById('status').style.display = 'none';
                $("#ReSimulateBtn").show();
                loadSimulationWos();
            },
            error: function () {
                alert("An error occurred while freezing.");
            }
        });
    });
    loadSels();
    $("#P27FromDt, #P27ToDt").on("change", function () {
        filterP27Grid();
    });
    $("#P28FromDt, #P28ToDt").on("change", function () {
        filterP28Grid();
    });

    $('#PopupNC10').on('show.bs.modal', function (event) {
        var nclogId = $("#NlogIdP11").val();
        api.getbulk("/workOrder/GetAllNcLog").then((ncdata) => {
            const selectedNcItem = ncdata.find(item => item.nC_Log_status_Id === 1 && item.insp_Outcome_Details_Id == parseInt(nclogId));

            if (selectedNcItem) {
                var id = selectedNcItem.insp_Outcome_Details_Id;
                var balno = selectedNcItem.balloon_No;
                var baldesc = selectedNcItem.balloon_No_Dir;
                var ncdes = selectedNcItem.nC_Descrip;
                var declsup = selectedNcItem.decl_by_Supplier;
                var qnty = selectedNcItem.nC_Qnty;
                var nctrack = selectedNcItem.nC_Tracking_No;
                var loc = selectedNcItem.storage_Location;
                var partno = selectedNcItem.inw_Recpt_Part_No_Name;
                var divhide = "Y"; // This is fixed as per your code
                var parttype = selectedNcItem.partType;
                var partid = selectedNcItem.inw_Recpt_Part_No_Id;
                var locnam = selectedNcItem.locationName;
                var headid = selectedNcItem.inw_Recpt_Header_Id;
                var inspid = selectedNcItem.inw_Insp_Log_Id;
        showPopup10();
        $("#P10ConsentRequired").prop('checked', false);
        var P10FeedbackConditionalAcceptance = document.getElementById('P10FeedbackConditionalAcceptance');
        P10FeedbackConditionalAcceptance.style.border = '';
        var P10NCInslvl1 = document.getElementById('P10NCInslvl1');
        P10NCInslvl1.style.border = '';
        $("#P10RequestToCustomerDiv").hide();
        $("#P10RequestedDateDiv").hide();
        $("#P10FeedbackReceiptDateDiv").hide();
        $("#P10RequestDecisionDiv").hide();
        $("#P10RequestDecisionDiv").addClass('d-none');
        $("#P10FeedbackReceiptDateDiv").addClass('d-none');
        $("#P10FeedbackConditionalAcceptanceDiv").addClass('d-none');
        //loadPartDoc(headid);
        //loadPartDocInsp(headid);
        GetAllNC_Decision_Log(id);
        $("#P10NcRef").val(nctrack);
        $("#P10BallNo").val(balno);
        $("#P10NcQnty").val(qnty);
        $("#P10NCLogId").val(id);
        $("#P10CurrentLoc").val(loc);
        $("#P10NcDesc").val(ncdes);
        $("#P10NcBaldesc").val(baldesc);
        $("#P10NcPartNo").val(locnam);
        $("#P10SpanHeading").text("PO Line Item Inward");
        $("#Span10Partno").text(partno);
        GetRcalog(id);
        if (declsup === 'Y') {
            $("#P10NCChkBySup").prop('checked', true);
        } else {
            $("#P10NCChkBySup").prop('checked', false);
        }
        if (divhide === 'Y') {
            $("#P10RequestToCustomer").prop('readonly', false);
            $("#P10RequestToCustomer").css({
                "pointer-events": "auto",
                "background-color": ""
            });
            $("#P10RequestedDate").prop('readonly', false);
            $("#P10FeedbackReceiptDate").prop('readonly', false);
            $("#P10RequestDecision").prop('readonly', false);
            $("#P10RequestDecision").css({
                "pointer-events": "auto",
                "background-color": ""
            });
            $("#P10FeedbackConditionalAcceptance").prop('readonly', false);
            $("#P10NClvl1").prop('readonly', false);
            $("#P10NClvl1").css({
                "pointer-events": "auto",
                "background-color": ""
            });
            $("#P10NCInslvl1").prop('readonly', false);
            $("#P10Save").show();
            $("#FullRedoDiv").hide();
        } else {
            $("#P10RequestToCustomer").prop('readonly', true);
            $("#P10RequestToCustomer").css({
                "pointer-events": "none",
                "background-color": "#e9ecef"
            });
            $("#P10RequestedDate").prop('readonly', true);
            $("#P10FeedbackReceiptDate").prop('readonly', true);
            $("#P10RequestDecision").prop('readonly', true);
            $("#P10RequestDecision").css({
                "pointer-events": "none",
                "background-color": "#e9ecef"
            });
            $("#P10FeedbackConditionalAcceptance").prop('readonly', true);
            $("#P10NClvl1").prop('readonly', true);
            $("#P10NClvl1").css({
                "pointer-events": "none",
                "background-color": "#e9ecef"
            });
            $("#P10NCInslvl1").prop('readonly', true);
            $("#P10Save").hide();
            $("#FullRedoDiv").show();
        }
        $("#Span10RoutingDiv").hide();
        if (parttype === "SubCon") {
            $("#P10SpanHeading").text("Shop");
            $("#SCmp").show();
            $("#SAssy").hide();
            $("#2SAssy").hide();
            $("#2SCmp").show();
            $("#2SendbehaRm").show();
            $("#2SendRm").show();
            $("#SendRm").show();
            $("#SendbehaRm").show();
            $("#Span10RoutingDiv").show();
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partid)).then((data) => {
                api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                    var routname = data[data.length - 1].routingName;
                    var opname = stepdata[stepdata.length - 1].stepNumber;
                    var stepId = stepdata[stepdata.length - 1].stepId;
                    $("#Span10Routing").text(routname);
                    $("#Span10OprNo").text(opname + " Input PartNo");

                    api.get("/routings/subcons?stepId=" + stepId).then((subcond) => {
                        data = data.filter(item => item.deleted != 1);
                        var ncval = qnty * parseInt(subcond[subcond.length - 1].costPerPart);
                        $("#P10NcValue").val(ncval);
                    }).catch((error) => {
                    });
                });
            });
        } else {
            $("#SCmp").hide();
            $("#SAssy").hide();
            $("#2SAssy").hide();
            $("#2SCmp").hide();
            $("#2SendbehaRm").show();
            $("#2SendRm").show();
            $("#SendRm").show();
            $("#SendbehaRm").show();
            api.get("/workOrder/GetPartPurchase?partId=" + partid).then((subcond) => {
                var ncval = qnty * parseInt(subcond[subcond.length - 1].price);
                $("#P10NcValue").val(ncval);
            }).catch((error) => {
            });
        }
                api.getbulk("/Employee/GetAllOrgChart").then((data) => {
                    data = data.filter(item => item.level_No === 1);
                    $("#P10NClvl1VM").val(data[data.length - 1].level_No);

                }).catch((error) => {
                });
            }
        });
    });
    $("#P10BtnCLose").on("click", function () {
        $("#PopupNC10").modal("hide");
    });

    $('#wohold').on('hidden.bs.modal', function (event) {

        var newNamevalidate = document.getElementById('WoComment');
        newNamevalidate.style.border = '';
        $("#WoComment").val('');
        //if (holdsalesorder) {
        //    LoadSalesOrders(salesCustOrderId);
        //}
    });

    $('#wohold').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var workOrderId = relatedTarget.data("workorderid");
        var salesOrderId = relatedTarget.data("salesorderid");
        var woNumber = relatedTarget.data("wonumber");
        var partNo = relatedTarget.data("partno");
        var partDesc = relatedTarget.data("partdesc");
        var planCompletionDateStr = relatedTarget.data("plancompletiondatestr");
        var partId = relatedTarget.data("partid");
        var partType = relatedTarget.data("parttype");
        var wostatus = relatedTarget.data("statusstr");
        var planWOQty = relatedTarget.data("calwoqty");
        var routingid = relatedTarget.data("routingid");
        var startingopno = relatedTarget.data("startingopno");
        var endingopno = relatedTarget.data("endingopno");
        var buildtostock = relatedTarget.data("buildtostock");
        var ppid = relatedTarget.data("ppid");
        var formattedDate = planCompletionDateStr.split("-").reverse().join("-");

        //$("#popup7PartNo").text(partNo);
        //$("#P7partdesc").text(partDesc);
        //$("#HoldPartNoField").val(partNo);
        $("#HolWoComlDt").val(formattedDate);
        $("#HoldPlanWoQnty").val(planWOQty);
        $("#HoldWorkOrderId").val(ppid);
        $("#HoldSalesOrderId").val(salesOrderId);
        $("#HoldPartId").val(partId);
        $("#HoldPartType").val(partType);
        $("#HoldWoNumber").val(woNumber);
        $("#HoldWoStatus").val(wostatus);
        $("#HoldRoutingId").val(routingid);
        $("#HoldStartingOpNo").val(startingopno);
        $("#HoldEndingOpNo").val(endingopno);
        $("#HoldBuildToStock").val(buildtostock);
        var statusstr = relatedTarget.data("holdstr");

        if (statusstr == "Y") {
            $("#SpanWoHold").text("Resume");
        } else {
            $("#SpanWoHold").text("Hold");
        }

    });


    $("#BtnWOHold").on("click", function () {
        var woid = parseInt($("#HoldWorkOrderId").val());
        var soid = parseInt($("#HoldSalesOrderId").val());
        var partid = parseInt($("#HoldPartId").val());
        var parttype = parseInt($("#HoldPartType").val());
        var wostatus = parseInt($("#HoldWoStatus").val());
        var planWoQty = parseInt($("#HoldPlanWoQnty").val());
        //var soqty = parseInt($("#NewTotalSoQty").val());
        var wonumber = $("#HoldWoNumber").val();
        var WoComplDate = new Date(Date.parse($('#HolWoComlDt').val()));
        var formattedDate = WoComplDate.toISOString();
        var routingid = $("#HoldRoutingId").val();
        var startingOpNo = $("#HoldStartingOpNo").val();
        var endingOpNo = $("#HoldEndingOpNo").val();
        var buildToStock = $("#HoldBuildToStock").val();
        var WoComment = $("#WoComment").val();

        if (WoComment.length == 0) {
            var newNamevalidate = document.getElementById('WoComment');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('WoComment');
            newNamevalidate.style.border = '';
        }
        if (buildToStock == 'Y') {

        } else {
            buildToStock = '';
        }
        if (wostatus === 8) {
            wostatus = 10;
        } else {
            wostatus = 8;
        }
        var rowData = {
            productionPlanId: parseInt(woid),
            woid: parseInt(woid),
            salesOrderId: parseInt(soid),
            wonumber: wonumber,
            partId: parseInt(partid),
            partType: parseInt(parttype),
            parentlevel: '',
            calcWOQty: parseInt(planWoQty),
            planCompletionDate: formattedDate,
            routingId: parseInt(routingid),
            startingOpNo: parseInt(startingOpNo),
            endingOpNo: parseInt(endingOpNo),
            status: parseInt(wostatus),
            buildToStock: buildToStock,
            comment: WoComment
        };

        api.post("/workorder/WoWaitlingpost", rowData).then((data) => {
            //console.log(data);
            $('#wohold').modal('hide');
            loadWoWaitlist();
        }).catch((error) => {
        });
    });
});
function GetRcalog(nclogId) {
    api.getbulk("/workOrder/GetAllCont_RCA_CA_log").then((data) => {
        data = data.filter(item => item.ncLogId === parseInt(nclogId));
        $("#P10Containt").val(data[0].containment_Action);
        $("#P14Containt").val(data[0].containment_Action);
        $("#P14Comment").val(data[0].senior_Feedback);
        $("#P14RCLogId").val(data[0].cont_RCA_CA_LogId);
        if (data[0].cont_RCA_CA_Status_Id === 4) {
            $("#P10DivDescVm").hide();
            $("#P10NClvl1VM").hide();
            $("#P10DivInsDesc").hide();
            $("#P10DivDesc").hide();
        } else {
            $("#P10DivDescVm").show();
            $("#P10NClvl1VM").show();
            $("#P10DivInsDesc").show();
            $("#P10DivDesc").show();
        }
    }).catch((error) => {
        console.log(error);
    });
}
function GetAllNC_Decision_Log(nclogId) {
    api.getbulk("/workOrder/GetAllNC_Decision_Log").then((data) => {
        Popup10Open++;
        $("#P10Save").hide();
        $("#P10Save2").hide();
        for (i = 0; i < data.length; i++) {
            Popup10Open++;
            data = data.filter(item => item.ncLogId === nclogId);
            $("#P10FeedbackReceiptDateDiv").show();
            $("#P10FeedbackConditionalAcceptanceDiv").show();
            $("#P10RequestDecisionDiv").show();
            //$("#P10DivInsDesc").show();
            $("#P10DivRedo").show();
            //$("#P10DivDesc2").show();
            $("#P10DivDesc2VM").show();
            $("#P10DivDescVm").show();
            $("#P10NCInslvl2VM").show();
            $("#P10DivInssDesc2").show();
            $("#P10Save2").show();
            //$("#P10NClvl1VM").show();
            $("#P10RequestToCustomerDiv").show();
            $("#P10RequestedDateDiv").show();
            $("#P10FeedbackReceiptDateDiv").show();
            $("#P10RequestDecisionDiv").show();
            $("#P10RequestDecisionDiv").removeClass('d-none');
            $("#P10FeedbackReceiptDateDiv").removeClass('d-none');
            $("#P10FeedbackConditionalAcceptanceDiv").removeClass('d-none');
            if (data[data.length - 1].cust_Decision_Reqd === 'Y') {
                $("#P10ConsentRequired").prop("checked", true);
                if (data[data.length - 1].cust_Dec_Request === 1 && data[data.length - 1].cust_feedback_Id === 1) {
                    $("#P10NClvl1 option[value=" + 1 + "]").show();
                    $("#P10NClvl1 option[value=" + 2 + "]").show();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").hide();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl1 option[value=" + 1 + "]").show();
                    $("#P10NClvl1 option[value=" + 2 + "]").show();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").hide();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                } else if (data[data.length - 1].cust_Dec_Request === 1 && data[data.length - 1].cust_feedback_Id === 3) {
                    $("#P10NClvl1 option[value=" + 1 + "]").hide();
                    $("#P10NClvl1 option[value=" + 2 + "]").hide();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").show();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl2 option[value=" + 1 + "]").hide();
                    $("#P10NClvl2 option[value=" + 2 + "]").hide();
                    $("#P10NClvl2 option[value=" + 3 + "]").hide();
                    $("#P10NClvl2 option[value=" + 4 + "]").hide();
                    $("#P10NClvl2 option[value=" + 5 + "]").hide();
                    $("#P10NClvl2 option[value=" + 6 + "]").hide();
                    $("#P10NClvl2 option[value=" + 7 + "]").show();
                    $("#P10NClvl2 option[value=" + 8 + "]").hide();
                } else if (data[data.length - 1].cust_Dec_Request === 2 && data[data.length - 1].cust_feedback_Id === 3) {
                    $("#P10NClvl1 option[value=" + 1 + "]").hide();
                    $("#P10NClvl1 option[value=" + 2 + "]").hide();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").show();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl2 option[value=" + 1 + "]").hide();
                    $("#P10NClvl2 option[value=" + 2 + "]").hide();
                    $("#P10NClvl2 option[value=" + 3 + "]").hide();
                    $("#P10NClvl2 option[value=" + 4 + "]").hide();
                    $("#P10NClvl2 option[value=" + 5 + "]").hide();
                    $("#P10NClvl2 option[value=" + 6 + "]").hide();
                    $("#P10NClvl2 option[value=" + 7 + "]").show();
                    $("#P10NClvl2 option[value=" + 8 + "]").hide();
                } else if (data[data.length - 1].cust_Dec_Request === 1 && data[data.length - 1].cust_feedback_Id === 2) {
                    $("#P10NClvl1 option[value=" + 1 + "]").hide();
                    $("#P10NClvl1 option[value=" + 2 + "]").show();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").show();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").hide();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl2 option[value=" + 1 + "]").hide();
                    $("#P10NClvl2 option[value=" + 2 + "]").show();
                    $("#P10NClvl2 option[value=" + 3 + "]").hide();
                    $("#P10NClvl2 option[value=" + 4 + "]").show();
                    $("#P10NClvl2 option[value=" + 5 + "]").hide();
                    $("#P10NClvl2 option[value=" + 6 + "]").hide();
                    $("#P10NClvl2 option[value=" + 7 + "]").hide();
                    $("#P10NClvl2 option[value=" + 8 + "]").hide();
                } else if (data[data.length - 1].cust_Dec_Request === 2 && data[data.length - 1].cust_Dec_Request === 2) {
                    $("#P10NClvl1 option[value=" + 1 + "]").hide();
                    $("#P10NClvl1 option[value=" + 2 + "]").hide();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").show();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").hide();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl2 option[value=" + 1 + "]").hide();
                    $("#P10NClvl2 option[value=" + 2 + "]").hide();
                    $("#P10NClvl2 option[value=" + 3 + "]").hide();
                    $("#P10NClvl2 option[value=" + 4 + "]").show();
                    $("#P10NClvl2 option[value=" + 5 + "]").hide();
                    $("#P10NClvl2 option[value=" + 6 + "]").hide();
                    $("#P10NClvl2 option[value=" + 7 + "]").hide();
                    $("#P10NClvl2 option[value=" + 8 + "]").hide();
                }
            } else {
                $("#P10ConsentRequired").prop("checked", false);
            }
            if (data[data.length - 1].redoCA === 'Y') {
                $("#P10RedoChkl1").prop("checked", true);
                $("#P10NCInslvl2VM").hide();
                $("#P10DivDesc2VM").hide();
                $("#P10DivDesc2").hide();
            } else {
                $("#P10RedoChkl1").prop("checked", false);
                $("#P10NCInslvl2VM").show();
                $("#P10DivDesc2VM").show();
                $("#P10DivDesc2").show();
            }
            let dateValue = data[data.length - 1].cust_Request_date.split("T")[0];
            let P10FeedbackReceiptDate = data[data.length - 1].cust_feedback_date.split("T")[0];
            $("#P10NcDecId").val(data[data.length - 1].nC_Decision_LogId);
            $("#P10RequestToCustomer").val(data[data.length - 1].cust_Dec_Request);
            $("#P10RequestedDate").val(dateValue);
            $("#P10FeedbackReceiptDate").val(P10FeedbackReceiptDate);
            $("#P10NClvl1").val(data[data.length - 1].nC_Disp_Deci_Lvl1_Id);
            $("#P10NClvl2").val(data[data.length - 1].NC_Disp_Deci_Lvl2_Id);
            $("#P10NCInslvl1").val(data[data.length - 1].nC_Disp_Inst_Lvl1);
            $("#P10NCInslvl2").val(data[data.length - 1].nC_Disp_Inst_Lvl2);
            $("#P10RequestDecision").val(data[data.length - 1].cust_feedback_Id);
            $("#P10FeedbackConditionalAcceptance").val(data[data.length - 1].cust_Feedback_Desc);

        }
        //loadSelectNCDispDecision();
        $("#PopupNC10 :input")
            .not('.btn-close, #PopupNC10Btn') // avoid disabling close or special buttons
            .prop("readonly", true)
            .prop("disabled", true);
    }).catch((error) => {
    });
}
function parseDateTime(dateStr) {
    let [datePart, timePart, meridian] = dateStr.split(" ");
    let [dd, mm, yyyy] = datePart.split("-");
    let fullTimeStr = `${yyyy}-${mm}-${dd} ${timePart} ${meridian}`;
    return new Date(fullTimeStr);
}
function filterP27Grid() {
    var fromDateStr = $("#P27FromDt").val();
    var toDateStr = $("#P27ToDt").val();

    // Convert to Date objects
    var fromDate = fromDateStr ? new Date(fromDateStr) : null;
    var toDate = toDateStr ? new Date(toDateStr) : null;

    // Validation: To Date can't be before From Date
    if (fromDate && toDate && toDate < fromDate) {
        alert("To Date cannot be earlier than From Date.");
        $("#P27ToDt").val(""); // Optional: clear the invalid date
        return;
    }

    $("#P27Grid tbody tr").each(function () {
        var issueDateStr = $(this).find("td:eq(9)").text().trim(); // 10th column (index 9)
        var issueDateParts = issueDateStr.split("-");
        if (issueDateParts.length === 3) {
            var issueDate = new Date(`${issueDateParts[2]}-${issueDateParts[1]}-${issueDateParts[0]}`);

            var show = true;
            if (fromDate && issueDate < fromDate) show = false;
            if (toDate && issueDate > toDate) show = false;

            $(this).toggle(show);
        } else {
            $(this).hide();
        }
    });
}
function filterP28Grid() {
    const fromDateStr = $("#P28FromDt").val(); // yyyy-mm-dd
    const toDateStr = $("#P28ToDt").val();

    const fromDate = fromDateStr ? new Date(fromDateStr) : null;
    const toDate = toDateStr ? new Date(toDateStr) : null;

    // Validation: To Date cannot be before From Date
    if (fromDate && toDate && toDate < fromDate) {
        alert("'To Date' cannot be earlier than 'From Date'.");
        $("#P28ToDt").val("");
        return;
    }

    $("#P28Grid tbody tr").each(function () {
        const cellText = $(this).find("td:eq(7)").text().trim(); // 8th column (index 7): "17-06-2025 01:30 AM"

        if (!cellText) {
            $(this).hide();
            return;
        }

        // Extract just the date part: "17-06-2025"
        const datePart = cellText.split(" ")[0];
        const [dd, mm, yyyy] = datePart.split("-");
        const cellDate = new Date(`${yyyy}-${mm}-${dd}`);

        let showRow = true;
        if (fromDate && cellDate < fromDate) showRow = false;
        if (toDate && cellDate > toDate) showRow = false;

        $(this).toggle(showRow);
    });
}
function calculateActualDurationP11() {
    const startDate = $('#P11ActDate1').val();   // format: YYYY-MM-DD
    const startTime = $('#P11ActTime1').val();   // format: HH:MM
    const endDate = $('#P11ActEndDate1').val();
    const endTime = $('#P11ActEndTime1').val();

    if (startDate && startTime && endDate && endTime) {
        const start = new Date(startDate + 'T' + startTime);
        const end = new Date(endDate + 'T' + endTime);

        if (end < start) {
            $('#P11ActDur').val('');
            alert('Actual End Date / Time must be greater than Actual Start Date / Time.');
            return;
        }

        const diffMs = end - start;
        const diffMins = Math.floor(diffMs / 60000);  // milliseconds to minutes
        const hours = Math.floor(diffMins / 60);
        const minutes = diffMins % 60;

        const formatted = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
        $('#P11ActDur').val(formatted);
    }
}
function calculateActualDuration() {
    const startDate = $('#P81ActDate1').val();   // format: YYYY-MM-DD
    const startTime = $('#P81ActTime1').val();   // format: HH:MM
    const endDate = $('#P81ActEndDate1').val();
    const endTime = $('#P81ActEndTime1').val();

    if (startDate && startTime && endDate && endTime) {
        const start = new Date(startDate + 'T' + startTime);
        const end = new Date(endDate + 'T' + endTime);

        if (end < start) {
            $('#P81ActDur').val('');
            alert('Actual End Date / Time must be greater than Actual Start Date / Time.');
            return;
        }

        const diffMs = end - start;
        const diffMins = Math.floor(diffMs / 60000);  // milliseconds to minutes
        const hours = Math.floor(diffMins / 60);
        const minutes = diffMins % 60;

        const formatted = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
        $('#P81ActDur').val(formatted);
    }
}
function loadMcWaitSetup() {

    api.getbulk("/workOrder/GetAllMcWaitSetupList").then((data) => {
        var tablebody = $("#P28Grid tbody");
        $(tablebody).html(""); // empty tbody
        for (let i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("P28GridRow", data[i]);
            let $row = $(row);

            $(tablebody).append($row);
        }
    }).catch((error) => {
        console.error("Failed to load WOs", error);
    });
}
function loadMoveMatl() {

    api.getbulk("/workOrder/GetAllSimulationMove").then((data) => {

        let unique = [];
        let seen = new Set();

        for (let item of data) {
            // Build unique key (exclude routingName/opNo condition)
            let key = `${item.issueMovDtStr}|${item.woNumber}|${item.partNo}|${item.routingName}|${item.opNo}`;

            // If same issueMovDtStr+woNumber+partNo+routingName but different opNo → allow
            // So we use full key including opNo to distinguish
            if (!seen.has(key)) {
                seen.add(key);
                unique.push(item);
            }
        }
        var tablebody = $("#P27Grid tbody");
        $(tablebody).html(""); // empty tbody
        for (let i = 0; i < unique.length; i++) {
            let row = AppUtil.ProcessTemplateData("P27GridRow", unique[i]);
            let $row = $(row);

            $(tablebody).append($row);
        }
    }).catch((error) => {
        console.error("Failed to load WOs", error);
    });
}
function loadWoWaitlist() {

    api.getbulk("/workOrder/AllProductionWo").then((data) => {
        var tablebody = $("#P1Grid tbody");
        $(tablebody).html(""); // empty tbody
        data = data.filter(item => item.readyForProd === "Y" && item.woRelease === "Y");  //
        for (let i = 0; i < data.length; i++) {
            if (data[i].status === 8) {
                data[i].holdstr = "Y";
            } else {
                data[i].holdstr = "N";
            }
            let rowHtml = AppUtil.ProcessTemplateData("P1GridRow", data[i]);
            let $row = $(rowHtml);
            if (data[i].status === 8) {
                $row.find('.P1gridChk').prop('disabled', true);
            }
            $(tablebody).append($row);
        }
    }).catch((error) => {
        console.error("Failed to load WOs", error);
    });
}
function copyMachineData(shop, name, machineId) {
    console.log("Shop:", shop);
    console.log("Name:", name);
    console.log("Machine ID:", machineId);
    if (mcAdd === 0) {
    $("#P81MachinceSel1").val(shop);
    $("#P81MachinceSel2").val(name);
    $("#P81MachinceId").val(machineId);
    }
    if (mcAdd === 11) {
        $("#P11MachinceSel1").val(shop);
        $("#P11MachinceSel2").val(name);
        $("#P11MachinceId").val(machineId);
    }
}
function loadReworkList() {
    var tablebody = $("#P10Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllRwk_List").then((data) => {
        for (i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("P10GridRow", data[i]);

            // Create jQuery object to manipulate the row
            let $row = $(row);

            // If inProcess is not "N", remove Edit and Delete options
            if (data[i].inProcess !== "N") {
                $row.find('.dropdown-menu a:contains("Edit")').remove();
                $row.find('.dropdown-menu a:contains("Delete")').remove();
            }

            $(tablebody).append($row);
        }
    }).catch((error) => {
    });
}
function loadNonWrkList() {
    var tablebody = $("#P8Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllNon_Plan_Wk_ListData").then((data) => {
        for (i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("P8GridRow", data[i]);

            // Create jQuery object to manipulate the row
            let $row = $(row);

            // If inProcess is not "N", remove Edit and Delete options
            if (data[i].inProcess !== "N") {
                $row.find('.dropdown-menu a:contains("Edit")').remove();
                $row.find('.dropdown-menu a:contains("Delete")').remove();
            }

            $(tablebody).append($row);
        }
    }).catch((error) => {
    });
}
function loadSimulationWos() {
    var tablebody = $("#SimWoGrid tbody");
    $(tablebody).html(""); // empty tbody
    document.getElementById('preloader').style.display = 'block';
    document.getElementById('status').style.display = 'block';
    api.getbulk("/workOrder/GetAllReadyforProductionWo").then((data) => {
        data.sort((a, b) => {
            const dateA = new Date(a.csStartDate.split("-").reverse().join("-")); // Convert to yyyy-MM-dd
            const dateB = new Date(b.csStartDate.split("-").reverse().join("-"));
            return dateA - dateB;
        });
        let showResimulate = data.some(d => d.dataChange === "Y");
        for (let i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("SimWoGridRow", data[i]);
            let $row = $(row);

            const partType = data[i].partType; // assume numeric or string like "Assy"
            const partTypeName = data[i].partTypeName?.toLowerCase() || "";
            const NoOfRoutes = data[i].noOfRoutes;
            const inProduction = "N"; // assume "Y" or "N"

            // === Part Wise Details ===
            if (partTypeName.toLowerCase() === "assembly") {
                $row.find(".dropdown-item:contains('Show Part Wise Details')")
                    .attr("data-filter", data[i].partNo)
                    .attr("data-show-child", "true")
                    .attr("data-disable-filter-bar", "true");
            } else {
                $row.find(".dropdown-item:contains('Show Part Wise Details')").remove();
              //  $row.find(".dropdown-item:contains('Change Routing')").remove(); // This part is unnecessary here; move to Change Routing section.
            }


            // === Opr Wise Details ===
            if (partTypeName.toLowerCase() === "parent cmp") {
                $row.find(".dropdown-item:contains('Show Opr Wise Details')")
                    .attr("data-filter", data[i].partNo);
            } else {
                $row.find(".dropdown-item:contains('Show Opr Wise Details')").remove();
            }


            // === Change Routing ===
            if (partTypeName.toLowerCase() === "parent cmp" && inProduction === "N" && parseInt(NoOfRoutes) > 1) {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('Change Routing')").remove();
            }

            $(tablebody).append($row);
            $("#SimWoGrid td").css("white-space", "nowrap");
        }
        if (showResimulate) {
            $("#ReSimulateBtn").show();
        } else {
            $("#ReSimulateBtn").hide();
        }
        document.getElementById('preloader').style.display = 'none';
        document.getElementById('status').style.display = 'none';
    }).catch((error) => {
        console.error("Failed to load WOs", error);
    });
}
let isDataSaved = true; // Set to true only after data is saved

// Trigger browser alert before unload
window.addEventListener('beforeunload', function (e) {
    if (!isDataSaved) {
        e.preventDefault(); // Required for some browsers
        e.returnValue = 'Simulation not completed'; 
    }
});

// Simulate a save action
function saveData() {
    // your save logic here
    isDataSaved = true;
    alert("Data saved successfully!");
}
function loadCustomer() {
    api.get("/masters/customers/").then((data) => {
        var departmentSelect = $("#P1SearchCustomer");
        $(departmentSelect).html("");
        $(departmentSelect).append('<option value="0">--Select Customer--</option>');
        for (i = 0; i < data.length; i++) {
            $(departmentSelect).append('<option value="' + data[i].companyName + '">' + data[i].companyName + '</option>');
        }
    }).catch((error) => {

    });
}
function loadRoutingStep(partId, qnty) {
    var tablebody = $("#P3Grid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/WorkOrder/RoutingStepsMcSub?routingId=" + partId + "&Qnty=" + qnty).then((data) => {
        for (i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("P3GridRow", data[i]);

            // Create jQuery object to manipulate the row
            let $row = $(row);

            $(tablebody).append($row);
        }
    }).catch((error) => {
    });
}
function loadRouting(partId, qnty, routingid) {
    originalRoutingId = parseInt(routingid);
    var tablebody = $("#P26Grid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/WorkOrder/GetSimRoutings?manufPartId=" + partId + "&Qnty=" + qnty).then((data) => {
        for (i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("P26GridRow", data[i]);
            let $row = $(row);
            if (parseInt(data[i].routingId) === parseInt(routingid)) {
                $row.find(".P26GridChk").prop("checked", true);
            }
            $(tablebody).append($row);
        }
    }).catch((error) => {
    });
}
function loadMachinceList() {

    var tablebody = $("#P82Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetMachines").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P82GridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function loadNotAvlMcTimeslot() {

    var tablebody = $("#P13Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetNotAvlMcTimeslot").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P13GridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function loadMachinceType() {
    var compSelect = $("#P82McType");
    $(compSelect).html("");
    $(compSelect).append('<option value="0">--Select Machince Type--</option>');
    api.get("/machine/getmachinetypes").then((data) => {
        for (i = 0; i < data.length; i++) {
            //div_data = "<option value='" +
            //    data[i].machineTypeTypeId + "'>" +
            //    data[i].machineTypeName +
            //    "</option>";
            $(compSelect).append('<option value="' + data[i].machineTypeName + '">' + data[i].machineTypeName + '</option>');
        }
    }).catch((error) => {
        //console.log(error);
    });
    api.get("/department/getdepartments/" + 1).then((data) => {
        var departmentSelect = $("#P82Shop");
        $(departmentSelect).html("");
        $(departmentSelect).append('<option value="0">--Select Shop--</option>');
        for (i = 0; i < data.length; i++) {
            $(departmentSelect).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {

    });
    api.get("/WorkOrder/GetAllNon_Plan_Wk_type_List/" + 1).then((data) => {
        var departmentSelect = $("#P81NonPlanType");
        $(departmentSelect).html("");
        $(departmentSelect).append('<option value="0">--Select Non Plan Type--</option>');
        for (i = 0; i < data.length; i++) {
            $(departmentSelect).append('<option value="' + data[i].non_Plan_Wk_type_ListId + '">' + data[i].non_Plan_Wk_type_Desc + '</option>');
        }
    }).catch((error) => {

    });
}
function loadMcShopP13() {

    var compSelect = $("#P13SearchMc");
    $(compSelect).html("");
    $(compSelect).append('<option value="0">--Select Machince--</option>');
    var P14Reason = $("#P14Reason");
    $(P14Reason).html("");
    $(P14Reason).append('<option value="0">--Select Reason--</option>');
    api.get("/machine/GetMachines").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(compSelect).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {
    });
    api.get("/workOrder/GetAllMc_not_avl_reason").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(P14Reason).append('<option value="' + data[i].mc_Not_Avl_ReasonId + '">' + data[i].reason_Desc + '</option>');
        }
    }).catch((error) => {
    });
    api.get("/department/getdepartments/" + 1).then((data) => {
        var departmentSelect = $("#P13SearchShop");
        var P14ShopId = $("#P14ShopId");
        $(P14ShopId).html("");
        $(departmentSelect).html("");
        $(P14ShopId).append('<option value="0">--Select Shop--</option>');
        $(departmentSelect).append('<option value="0">--Select Shop--</option>');
        for (i = 0; i < data.length; i++) {
            $(P14ShopId).append('<option value="' + data[i].departmentId + '">' + data[i].name + '</option>');
            $(departmentSelect).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {

    });
}
function DeleteMcNotAvl(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("id");
    let confirmval = confirm("Are your sure you want to delete this Machine Non Availability Detail ?", "Yes", "No");
    if (confirmval) {
        api.get("/workOrder/DeleteMc_Timeslot_List?itemMasterDocListId=" + masterdocid).then((data) => {
            loadNotAvlMcTimeslot();
        }).catch((error) => {

        });
    }
}
function DelteNonPlan(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("id");
    let confirmval = confirm("Are your sure you want to delete this Non-Plan Work ?", "Yes", "No");
    if (confirmval) {
        api.get("/workOrder/DeleteNon_Plan_Wk_List?itemMasterDocListId=" + masterdocid).then((data) => {
            loadNonWrkList();
        }).catch((error) => {

        });
    }
}
function loadSels() {
    api.get("/department/getdepartments/" + 1).then((data) => {
        var P27toLoc = $("#P27toLoc");
        $(P27toLoc).html("");
        var P27fromLoc = $("#P27fromLoc");
        $(P27fromLoc).html("");
        var P28Shop = $("#P28Shop");
        $(P28Shop).html("");
        $(P27toLoc).append('<option value="0">-Select Shop-</option>');
        $(P28Shop).append('<option value="0">-Select Shop-</option>');
        $(P27fromLoc).append('<option value="0">-Select Shop-</option>');
        $(P27fromLoc).append('<option value="Stores">Stores</option>');
        for (i = 0; i < data.length; i++) {
            $(P27toLoc).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P27fromLoc).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P28Shop).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {

    });
    var P4McName = $("#P28McName");
    $(P4McName).html("");
    $(P4McName).append('<option value="0">--Select Machine--</option>');
    api.get("/machine/getmachines").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(P4McName).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {
    });
}