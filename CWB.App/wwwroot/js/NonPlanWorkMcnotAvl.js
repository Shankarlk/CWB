let isP81Modified = false;
let isP11Modified = false;
let originalRoutingId = 0;
let currentRoutingId = 0;
let mcAdd = 0;
let Popup10Open = 0;
let closeP26 = 0;
$(document).ready(function () {
    $("#txtCushionTime").val(10);
});
$("#txtCushionTime").on("input", function () {

    let value = $(this).val();

    // Allow empty while typing
    if (value === "")
        return;

    value = parseInt(value);

    if (isNaN(value) || value < 0 || value > 25) {
        alert("Cushion Time should be between 0 and 25%.");
        $(this).val(10);
    }
});
var stoppingData = {
    woStoppingOprIds: {},     // { woId: oprListId }
    oprStoppingMcWaitIds: {}  // { oprListId: mcWaitListId }
};



$(document).ready(function () {

    // landingPage();

    $("#simulate").secureClick(function () {
        var value = $("#txtCushionTime").val().trim();

        if (value === "") {
            cushionPercent = 10.0;
            $("#txtCushionTime").val(10);
        }
        else {
            cushionPercent = parseFloat(value);

            if (isNaN(cushionPercent) || cushionPercent < 0 || cushionPercent > 25) {
                alert("Cushion Time should be between 0 and 25%.");
                cushionPercent = 10.0;
                $("#txtCushionTime").val(10);
                return;
            }
        }
        alert("Simulation Has Started");

        isDataSaved = false;
        $('#preloadersim').show();

        $.ajax({
            type: "POST",
            url: "/WorkOrder/SimulateWO",
            data: {
                cushionPercent: cushionPercent
            },
            success: function (result) {
                alert(result.message);

                isDataSaved = true;



                loadSimulationWos();
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
                console.error(error);
                alert("Simulation Failed");
            },
            complete: function () {
                $('#preloadersim').hide();
            }
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
            if (P81ActDate1.length === 0) {
                $("#P81Save1").click();
            } else if (P81ActEndDate1.length === 0) {
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
    $("#P81Save1").secureClick(function (event) {
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
        return api.post("/WorkOrder/PostNon_Plan_Wk_List", formdata).then((data) => {
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

    loadCustomer();








});

function parseDateTime(dateStr) {
    let [datePart, timePart, meridian] = dateStr.split(" ");
    let [dd, mm, yyyy] = datePart.split("-");
    let fullTimeStr = `${yyyy}-${mm}-${dd} ${timePart} ${meridian}`;
    return new Date(fullTimeStr);
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

function loadNonWrkList() {
    var tablebody = $("#nonplanworktblGridbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllNon_Plan_Wk_ListData").then((data) => {
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


function loadMachinceList() {

    var tablebody = $("#P82Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetMachines").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P82GridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function loadNotAvlMcTimeslot() {

    var tablebody = $("#P13Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetNotAvlMcTimeslot").then((data) => {
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
    api.get("/WorkOrder/GetAllNon_Plan_Wk_type_List/").then((data) => {
        var departmentSelect = $("#P81NonPlanType");
        $(departmentSelect).html("");
        $(departmentSelect).append('<option value="0">--Select Non Plan Type--</option>');
        for (i = 0; i < data.length; i++) {
            $(departmentSelect).append('<option value="' + data[i].non_Plan_Wk_type_ListId + '">' + data[i].non_Plan_Wk_type_Desc + '</option>');
        }
    }).catch((error) => {

    });
}






function loadNonPlanType() {

    var select = $("#P81NonPlanType");

    select.html("");
    select.append('<option value="0">--Select Non Plan Type--</option>');

    api.get("/WorkOrder/GetAllNon_Plan_Wk_type_List/").then((data) => {

        for (let i = 0; i < data.length; i++) {

            select.append(
                '<option value="' +
                data[i].non_Plan_Wk_type_ListId +
                '">' +
                data[i].non_Plan_Wk_type_Desc +
                '</option>'
            );
        }

    }).catch((error) => {
        console.log(error);
    });
}

$(document).ready(function () {
    loadNonWrkList();
    loadNonPlanType();
});
