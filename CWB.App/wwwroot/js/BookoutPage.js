let P11Save = false;
async function showPopup() {
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
function getLinkOrText(count, popupId, shop) {
    if (count > 0) {
        return `<a href="#" onclick="openPopup('${popupId}', '${shop}')" style="text-decoration: underline; color: blue;">${count}</a>`;
    } else {
        return count;
    }
}
function openPopup(popupId, shopName) {
    // You can use `shopName` for filtering popup content if needed
    console.log(`Open ${popupId} for Shop: ${shopName}`);

    // Assuming you have modal divs with these IDs
    $("#P6Shop").val(shopName);
    $("#P8Shop").val(shopName);
    $("#P10Shop").val(shopName);
    $(`#${popupId}`).modal('show'); // If using Bootstrap
}
function loadMis() {
    $("#preloaderblurred").show();
    api.getbulk('/WorkOrder/GetSetupSummaryByShop').then((data) => {
        $("#MisTBody").html('');
        let html = '';
        if (data.length === 0) {
            // Data is empty, create a row with '0' in the metric columns
            html += `<tr class="text-center">
                        <td>0</td>
                        <td>0</td>
                        <td>0</td>
                        <td>0</td>
                     </tr>`;
        } else {
            data.forEach(item => {
                html += `<tr class="text-center">
                        <td>${item.shop}</td>
                        <td>${getLinkOrText(item.waitingForSetupStart, 'Popup6', item.shop)}</td>
                        <td>${getLinkOrText(item.readyForSetupApproval, 'Popup8', item.shop)}</td>
                        <td>${getLinkOrText(item.bookout, 'Popup10', item.shop)}</td>
                     </tr>`;
            });
        }
        $("#MisTBody").html(html);
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
$(document).ready(function () {
    loadMis();
    loadSels();
    $("#P6Shop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P6Grid tbody tr").show();
            var $tableBody = $("#P6Grid tbody");
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
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P6Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P6Grid tbody");
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
    $("#P6McName").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P6Grid tbody tr").show();
            var $tableBody = $("#P6Grid tbody");
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
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P6Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P6Grid tbody");
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
    $("#P6PartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P6Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P6Grid tbody");
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
    $('#Popup6').on('show.bs.modal', function (event) {
        loadSetUpConfList();
    });
    $('#Popup7').on('hidden.bs.modal', function (event) {
        $("#P7SetUpTime").val('');
        var newNamevalidate = document.getElementById('P7SetUpTime');
        newNamevalidate.style.border = '';
        document.getElementById('Popup6').style.filter = 'none';
    });
    $('#Popup7').on('show.bs.modal', function (event) {
        document.getElementById('Popup6').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var partno = relatedTarget.data("partno");
        var route = relatedTarget.data("route");
        var opname = relatedTarget.data("opname");
        var shopname = relatedTarget.data("shopname");
        var mcname = relatedTarget.data("mcname");
        var matlrcpt = relatedTarget.data("matlrcpt");
        var plantime = relatedTarget.data("plantime");
        var id = relatedTarget.data("id");
        $("#P7McWaitId").val(id);
        $("#P45PartName").text(partno);
        $("#P45Rout").text(route);
        $("#P45Op").text(opname);
        $("#P45ShopName").text(shopname);
        $("#P45McName").text(mcname);
        $("#P7RcptTime").text(matlrcpt);
        $("#P7PlanTime").text(plantime);
    });
    $("#P7Save").on('click', function (event) {
        var P7SetUpTime = $("#P7SetUpTime").val();
        var P7McWaitId = parseInt($("#P7McWaitId").val());
        if (P7SetUpTime.length === 0) {
            var newNamevalidate = document.getElementById('P7SetUpTime');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P7SetUpTime');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P7McWaitId)) {
            P7McWaitId = 0
            return false;
        }
        const today = new Date();
        const [setupHours, setupMinutes] = P7SetUpTime.split(":").map(Number);
        const setupStartDateTime = new Date(today.getFullYear(), today.getMonth(), today.getDate(), setupHours, setupMinutes);

        const rcptTimeStr = $("#P7RcptTime").text().trim(); // e.g., "06:40 AM"
        const rcpt24hr = convertTo24Hour(rcptTimeStr);      // e.g., "06:40"
        const [rcptHours, rcptMinutes] = rcpt24hr.split(":").map(Number);
        const rcptDateTime = new Date(today.getFullYear(), today.getMonth(), today.getDate(), rcptHours, rcptMinutes);
        if (setupStartDateTime < rcptDateTime) {
            alert("Setup Start Time cannot be earlier than Material Receipt Time.");
            return false;
        }
        const setupStartFormatted = formatDateTime(setupStartDateTime);
        var formdata = {
            mc_Wait_ListId: P7McWaitId,
            setup_Start_time: setupStartFormatted,
        };
        api.post("/WorkOrder/UpdateSetupMc_Wait_List", formdata).then((data) => {
            alert("Setup Start Confirmation Saved");
            $("#Popup7").modal("hide");
            loadSetUpConfList();
        }).catch((error) => {
            console.log(error);
        });
    });

    $("#P6Shop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P6Grid tbody tr").show();
            var $tableBody = $("#P6Grid tbody");
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
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P6Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P6Grid tbody");
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
    $("#P6McName").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P6Grid tbody tr").show();
            var $tableBody = $("#P6Grid tbody");
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
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P6Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P6Grid tbody");
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
    $("#P6PartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P6Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P6Grid tbody");
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
    $('#Popup8').on('show.bs.modal', function (event) {
        loadSetUpAplList();
    });
    $('#Popup9').on('hidden.bs.modal', function (event) {
        $("#P9SetDoc").val('-');
        $("#P9Com").val('');
        $("#P9SetChk").prop('checked',false);
        var P9SetDoc = document.getElementById('P9SetDoc');
        P9SetDoc.style.border = '';
        var P9AdSetTime = document.getElementById('P9AdSetTime');
        P9AdSetTime.style.border = '';
        var P9AchFtr = document.getElementById('P9AchFtr');
        P9AchFtr.style.border = '';
        document.getElementById('Popup8').style.filter = 'none';
    });
    $('#Popup9').on('show.bs.modal', function (event) {
        document.getElementById('Popup8').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var partno = relatedTarget.data("partno");
        var route = relatedTarget.data("route");
        var opname = relatedTarget.data("opname");
        var shopname = relatedTarget.data("shopname");
        var mcname = relatedTarget.data("mcname");
        var matlrcpt = relatedTarget.data("matlrcpt");
        var plantime = relatedTarget.data("plantime");
        var setuptime = relatedTarget.data("setuptime");
        var plannedsetuptime = relatedTarget.data("plannedsetuptime");
        var newPlannedTime = plannedsetuptime.slice(0, -3);
        var id = relatedTarget.data("id");
        loadEmployeeSel();
        loadReasonFtr("P9AchFtr");
        loadReasonSetup("P9AdSetTime");
        $("#P9McWaitId").val(id);
        $("#P9PartName").text(partno);
        $("#P9Rout").text(route);
        $("#P9Op").text(opname);
        $("#P9ShopName").text(shopname);
        $("#P9McName").text(mcname);
        $("#P9RcptTime").text(matlrcpt);
        $("#P9PlanTime").text(plantime);
        $("#P9ActulTime").text(setuptime);
        $("#P9PlanSetupTime").text(newPlannedTime);// 2. Get the current date and time
        const now = new Date();

        // 3. Create the Start Time Date Object (using today's date)

        // A standard way to parse time in a format like "HH:MM AM/PM" is to use
        // a temporary string that combines today's date with the time.
        const startDateTimeStr = now.toDateString() + " " + setuptime;
        const startTime = new Date(startDateTimeStr);

        // --- Calculation ---

        // Check if the start time is in the future. If so, it uses yesterday's date.
        // This handles cases where the start time is after midnight (e.g., 2 AM) 
        // but is still considered "today's" work start.
        if (startTime > now) {
            startTime.setDate(startTime.getDate() - 1);
        }

        // Get the difference in milliseconds
        const differenceMs = now.getTime() - startTime.getTime();

        // 4. Convert milliseconds to H:M:S format
        const totalSeconds = Math.floor(differenceMs / 1000);
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = totalSeconds % 60;

        // Format the time with leading zeros (e.g., 5 -> 05)
        const timeTaken =
            String(hours).padStart(2, '0') + ":" +
            String(minutes).padStart(2, '0');
        $("#P9SetTimeTaken").val(timeTaken);
        const timeTakenTotalMinutes = (hours * 60) + minutes;

        // 2. Convert Planned Time (string) to Total Minutes
        const [plannedHours, plannedMinutes] = newPlannedTime.split(':').map(Number);
        const plannedTimeTotalMinutes = (plannedHours * 60) + plannedMinutes;

        // 3. Perform the comparison
        if (timeTakenTotalMinutes > plannedTimeTotalMinutes) {

            // Time Taken is greater. Calculate the difference.
            const excessMinutes = timeTakenTotalMinutes - plannedTimeTotalMinutes;

            // Convert the difference (total minutes) back into HH:MM format
            const diffHours = Math.floor(excessMinutes / 60);
            const diffMinutes = excessMinutes % 60;

            const excessTimeFormatted =
                String(diffHours).padStart(2, '0') + ":" +
                String(diffMinutes).padStart(2, '0');

            // Console the comparison result
            console.log("Greater setup time taken");

            // Console the difference (Time Taken - Planned Time)
            console.log(`Excess Time (HH:MM): ${excessTimeFormatted}`);
            $("#P9Adntime").show();
            $("#lblAdnTime").show();
            $("#P9AdSetTime").show();
            $("#lblAdSetTime").show();
            $("#P9Adntime").val(excessTimeFormatted);
        } else {
            $("#P9Adntime").hide();
            $("#lblAdnTime").hide();
            $("#P9AdSetTime").hide();
            $("#lblAdSetTime").hide();
        }

    });
    $('#P7SetChk').on('change', function () {

        const isChecked = $(this).is(':checked');
        const selectElement = $('#P9AdSetTime');

        if (isChecked) {
            $("#P9AchFtr").hide(); // Hides the select element (sets display: none)
            $("#lblReasFtr").hide(); // Hides the select element (sets display: none)
            $("#P9AchFtrDiv").hide(); // Hides the select element (sets display: none)
        } else {
            $("#P9AchFtr").show(); // Shows the select element (reverts display to its default, usually block/inline-block)
            $("#lblReasFtr").show(); // Shows the select element (reverts display to its default, usually block/inline-block)
            $("#P9AchFtrDiv").show(); // Shows the select element (reverts display to its default, usually block/inline-block)
        }
    });
    $("#P9Save").on('click', function (event) {
        var P9SetDoc = $("#P9SetDoc").val();
        var P9Com = $("#P9Com").val();
        var P9SetChk = $("#P7SetChk").val();
        var P9Adntime = $("#P9Adntime").val();
        var P9SetTimeTaken = $("#P9SetTimeTaken").val();
        var P9AdSetTime = parseInt($("#P9AdSetTime").val());
        var P9AchFtr = parseInt($("#P9AchFtr").val());
        var P9Operator = parseInt($("#P9Operator").val());
        var schk = $("#P7SetChk").is(":checked") ? 'Y' : 'N';
        var P9McWaitId = parseInt($("#P9McWaitId").val());
        if (P9SetDoc.length === 0) {
            var newNamevalidate = document.getElementById('P9SetDoc');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P9SetDoc');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P9McWaitId)) {
            P9McWaitId = 0
            return false;
        }
        if (P9Operator === 0) {
            var newNamevalidate = document.getElementById('P9Operator');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P9Operator');
            newNamevalidate.style.border = '';
        }
        if ($('#P9AdSetTime').is(':visible')) {
            if (P9AdSetTime === 0) {
                var newNamevalidate = document.getElementById('P9AdSetTime');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P9AdSetTime');
                newNamevalidate.style.border = '';
            }
        }
        if ($('#P9AchFtr').is(':visible')) {
            if (P9AchFtr === 0 || isNaN()) {
                var newNamevalidate = document.getElementById('P9AchFtr');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P9AchFtr');
                newNamevalidate.style.border = '';
            }
        }
        var formdata = {
            mc_Wait_ListId: P9McWaitId,
            setup_Appvl_Doc_Ref: P9SetDoc,
            setup_FTR: schk,
            setup_comments: P9Com,
            operatorId: P9Operator,
            reasonforAddnSetupTimeId: P9AdSetTime,
            reasonfornotachievingFTRId: P9AchFtr,
            reasonfornotachievingFTRId: P9AchFtr,
            setupTimeTaken: P9SetTimeTaken,
            addntimeforSetup: P9Adntime,

        };
        api.post("/WorkOrder/UpdateSetupAplMc_Wait_List", formdata).then((data) => {
            alert("Setup Approval Saved");
            $("#Popup9").modal("hide");
            loadSetUpAplList();
        }).catch((error) => {
            console.log(error);
        });
    });
    $('#Popup10').on('show.bs.modal', function (event) {
        loadBookoutList();
    });
    $('#UploadDocumnet').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'none';
    });
    $('#UploadDocumnet').on('show.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'blur(5px)';
    });
    $('#Popup11').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup10').style.filter = 'none';
    });
    $('#PartInspectionDocPop').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'none';
    });
    $('#PartInspectionDocPop').on('show.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'blur(5px)';
    });
    $('#Popup11').on('show.bs.modal', function (event) {
        $("#P6MessageBox").text("");
        document.getElementById('Popup10').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var partno = relatedTarget.data("partno");
        var route = relatedTarget.data("route");
        var opname = relatedTarget.data("opname");
        var shopname = relatedTarget.data("shopname");
        var mcname = relatedTarget.data("mcname");
        var uomname = relatedTarget.data("uomname");
        var planqnty = relatedTarget.data("planqnty");
        var issueqnty = relatedTarget.data("issueqnty");
        var qntoff = relatedTarget.data("qntoff");
        var accp = relatedTarget.data("accp");
        var ncqnty = relatedTarget.data("ncqnty");
        var id = relatedTarget.data("id");
        var partid = relatedTarget.data("partid");
        var woid = relatedTarget.data("woid");
        $("#P11PartNo").text(partno);
        $("#P11PerPart").text(partno);
        $("#P11Rout").text(route);
        $("#P11OpNo").text(opname);
        $("#P11PerOp").text(opname);
        $("#P11PerQnty").text('0');
        $("#P11PerBook").text('0');
        $("#P11PerBal").text(planqnty);
        $("#P11Shop").text(shopname);
        $("#P11McName").text(mcname);
        $("#P6TbTotalOffUnit").text(uomname);
        $("#P6TbInspTotalUnit").text(uomname);
        $("#P6TbTotalNcUnit").text(uomname);
        $("#P6TotalInspUnit").text(uomname);
        $("#P7UnitVM").text(uomname);
        $("#P7PartNoSpan").text(partno);
        $("#P7RoutingSpan").text(route);
        $("#P7OprNoSpan").text(opname);
        $("#P7PartId").val(partid);
        $("#P7InwHeaderId").val(id);
        $("#P7woid").val(woid);
        $("#P11TotalInsp").val(qntoff);
        $("#P11TbTotalOff").val(qntoff);
        $("#P11TbInspTotal").val(accp);
        $("#P11TbTotalNc").val(ncqnty);
        $("#P11InputBtn").hide();
        showPopup();
        loadDocUploadList();
        loadNclog(id);
    });
    $("#P11TbTotalOff, #P11TbInspTotal").on("input", calculateNc);
    $("#P11NonBtn").on('click', function (event) {
        $("#popup7").modal("show");
        var P7NcBallonDesc = document.getElementById('P7NcBallonDesc');
        P7NcBallonDesc.style.border = '';
        var P7NcBallonDesc = document.getElementById('P7NcBallonDesc');
        P7NcBallonDesc.style.border = '';
        var P7NcDesc = document.getElementById('P7NcDesc');
        P7NcDesc.style.border = '';
        var P7NcQnty = document.getElementById('P7NcQnty');
        P7NcQnty.style.border = '';
        $("#P7RoutingDiv").removeAttr("hidden");
    });
    $("#P7BtnClose").on("click", function () {
        $("#popup7").modal("hide");
        document.getElementById('Popup11').style.filter = 'none';
    });
    $('#ErrorMessage11').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'none';
    });
    $('#ErrorMessage11').on('show.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'blur(5px)';
    });
    $("#EP7Exit").on("click", function () {
        $("#popup5").modal("hide");
        $("#Popup11").modal("hide");
        $("#NotUploaded").modal("hide");
        $("#popupInward").modal("hide");
    });
    $("#EP7Return").on("click", function () {
        $("#popup5").modal("hide");
        $("#NotUploaded").modal("hide");
    });
    $("#popupBookClose").on("click", function () {
        var P11TbInspTotal = $("#P11TbInspTotal").val();
        var P11TbTotalOff = $("#P11TbTotalOff").val();
        var row = $("#P6DocUploadGrid tbody tr");
        var mandatory = row.find("td:nth-child(2)").text().trim();
        var comment = row.find("td:nth-child(3)").text().trim();
        if (P11Save ) {
                $("#warning").modal("show");
        } else if (mandatory == "Y") {
            if (comment.length <= 0) {
                $("#NotUploaded").modal("show");
            } else {
                $("#Popup11").modal("hide");
            }
        }
        else {
            $("#Popup11").modal("hide");
        }
    });
    $('#popup7').on('show.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'blur(5px)';
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
    $("#P7NCSave").on("click", function () {
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
            mcWaitId: P7InwHeaderId,
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
        $.ajax({
            type: "POST",
            url: '/workOrder/PostNcLog',
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
    $("#Error4Exit").on("click", function () {
        $("#ErrorMessage4").modal("hide");
        $("#Popup11").modal("hide");
    });
    $('#ErrorMessage4').on('show.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'blur(5px)';
    });
    $('#ErrorMessage4').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup11').style.filter = 'none';
    });
    $("#ExitWarningBtn").on("click", function () {
        $("#warning").modal("hide");
        $("#Popup11").modal("hide");
    });
    $("#P6LineInspupdate").on("click", function () {
        var ourCountVM = $("#P11TbTotalOff").val();
        var suppCountVM = $("#P11TotalInsp").val();
        var P6TotalNc = $("#P6TotalNc").val();
        var P11TbTotalNc = $("#P11TbTotalNc").val();

        var formdata = {
            mc_Wait_ListId: parseInt($("#P7InwHeaderId").val()),
            nonConQnty: parseInt($("#P11TbTotalNc").val()),
            accepted: parseInt($("#P11TbInspTotal").val()),
            qntyOffered: parseInt($("#P11TbTotalOff").val())
        };
        api.post("/WorkOrder/UpdateQntyMc_Wait_List", formdata).then((data) => {

        }).catch((error) => {
            console.log(error);
        });
        if ((parseInt(suppCountVM) != parseInt(ourCountVM)) || (parseInt(P6TotalNc) < parseInt(P11TbTotalNc))) {
            $("#ErrorMessage4").modal("show");
        } else {
            var qnty = parseInt($("#P11TbInspTotal").val());
            var P6PartId = parseInt($("#P7PartId").val());
            var P6PoId = parseInt($("#P6PoId").val());

            api.getbulk("/WorkOrder/GetAllInv_Trans_Log").then((logdata) => {
                logdata = logdata.filter(item => item.input_Part_NoId === P6PartId);
                var logId = 0;
                if (Array.isArray(logdata) && logdata.length > 0) {
                    logId = logdata[logdata.length - 1].inv_Trans_LogId || 0;
                } else {
                    logId = 0;
                }
                var rowData = {
                    Inv_Trans_LogId: logId,
                    Input_Part_NoId: P6PartId,
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
                    Part_Status: 1,
                    Movement_Compl: 'N'
                };
                api.post("/WorkOrder/PostInv_Trans_Log", rowData).then((Insdata) => {
                    $("#P6MessageBox").text("Inspection Complete");
                    P11Save = false;
                    var InvMasterrowData = {
                        Part_NoId: P6PartId,
                        Routing_Id: 0,
                        Inv_Trans_Log_Id: Insdata.inv_Trans_LogId,
                        Opr_No_Id: 0,
                        Current_QntOnHand: qnty,
                        Location_Id: 1
                    };
                    api.post("/WorkOrder/PostInventory_Master", InvMasterrowData).then((data) => {

                        var woId = parseInt($("#P7woid").val());             // Ensure these fields exist
                        var oprNo = parseInt($("#P7OprId").val());           // Operation number
                        var mcId = 0;             // Machine ID
                        var balBookoutQty = parseInt($("#P11TbInspTotal").val());  // Balance Bookout Quantity
                        var bookoutTime = new Date().toISOString();          // Current time in ISO format

                        var statusUpdateData = {
                            woId: woId,
                            oprNo: oprNo,
                            mcId: mcId,
                            balBookoutQty: balBookoutQty,
                            bookoutTime: bookoutTime
                        };

                        api.post("/WorkOrder/UpdateWOStatusOnBookout", statusUpdateData)
                            .then((res) => {
                                console.log("WO status updated", res);
                            })
                            .catch((error) => {
                                console.log("Error updating WO status", error);
                            });


                    }).catch((error) => {
                        console.log(error);
                    });
                }).catch((error) => {
                    console.log(error);
                });

            }).catch((error) => {
            });
        }
    });
    loadEmployeeSel();
});
function loadEmployeeSel() {
    var OrgEmpl = $('#P9Operator');
    OrgEmpl.html('');
    api.get("/Employee/GetAllEmployee").then((data) => {
        const filteredData = data.filter(item => item.designation_Id === 2);
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        OrgEmpl.append(div_data);
        for (i = 0; i < filteredData.length; i++) {
            div_data = "<option value='" + filteredData[i].employee_ID + "'>" + filteredData[i].employee_name + "</option>";
            OrgEmpl.append(div_data);
        }
    }).catch((error) => {
    });
}
function loadReasonFtr(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    if (!compSelect.length)
        return;
    compSelect.empty();
    ////debugger;
    var div_data = "<option value=''>--Select--</option>";
    compSelect.append(div_data);
    api.get("/workorder/GetAllSetupVariationReason").then((data) => {
        data = data.filter(item => item.setupType === "FTR not achieved");
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].setupVariationReasonId + "'>" +
                data[i].reason +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}
function loadReasonSetup(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    if (!compSelect.length)
        return;
    compSelect.empty();
    ////debugger;
    var div_data = "<option value=''>--Select--</option>";
    compSelect.append(div_data);
    api.get("/workorder/GetAllSetupVariationReason").then((data) => {
        data = data.filter(item => item.setupType === "Setup Time Variation");
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].setupVariationReasonId + "'>" +
                data[i].reason +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}
function loadNclog(mcWaitId) {
    api.getbulk("/WorkOrder/GetAllNcLogInsp").then((data) => {
        data = data.filter(item => item.mcWaitId === parseInt(mcWaitId));
        var tablebody = $("#P11NcGrid tbody");
        $(tablebody).html("");
        let totalQuantity = 0;
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
            totalQuantity += data[i].nC_Qnty;
            $(tablebody).append(AppUtil.ProcessTemplateData("P11GridRow", data[i]));
            $("#P6TotalNc").val(totalQuantity);
        }
    }).catch((error) => {
    });
}
function calculateNc() {
    let totalOff = parseFloat($("#P11TbTotalOff").val()) || 0;
    let inspTotal = parseFloat($("#P11TbInspTotal").val()) || 0;
    let balqny = parseFloat($("#P11PerBal").text()) || 0;
    if (totalOff > balqny) {
        $("#ErrorMessage11").modal("show");
    }
    let totalNc = totalOff - inspTotal;
    if (totalNc < 0) {
        totalNc = 0; // optional: prevent negative result
       // $("#P11NonBtn").prop('disabled', true);
    } else {
       // $("#P11NonBtn").prop('disabled', false);
    }
    P11Save = true;
    $("#P11TbTotalNc").val(totalNc);
    $("#P11TotalInsp").val(totalOff);
}
function formatDateTime(date) {
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const dd = String(date.getDate()).padStart(2, '0');
    const hh = String(date.getHours()).padStart(2, '0');
    const min = String(date.getMinutes()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd} ${hh}:${min}`;
}

function convertTo24Hour(time12h) {
    const [time, modifier] = time12h.split(" ");
    let [hours, minutes] = time.split(":");

    hours = parseInt(hours, 10);
    if (modifier === "PM" && hours < 12) hours += 12;
    if (modifier === "AM" && hours === 12) hours = 0;

    return `${hours.toString().padStart(2, '0')}:${minutes}`;
}
function loadSetUpConfList() {
    var tablebody = $("#P6Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();

    api.getbulk("/workOrder/GetAllSetUpCnfList").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P6GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadSetUpAplList() {
    var tablebody = $("#P8Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();

    api.getbulk("/workOrder/GetAllSetUpApprolList").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P8GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadBookoutList() {
    var tablebody = $("#P10Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();


    api.getbulk("/workOrder/GetAllBookOutList").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P10GridRow", data[i]));
        }
        $("#preloaderblurred").hide();

    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadSels() {
    api.get("/department/getdepartments/" + 1).then((data) => {
        var departmentSelect = $("#P6Shop");
        $(departmentSelect).html("");
        $(departmentSelect).append('<option value="0">-Select Shop-</option>');
        var P8Shop = $("#P8Shop");
        $(P8Shop).html("");
        $(P8Shop).append('<option value="0">-Select Shop-</option>');
        var P10Shop = $("#P10Shop");
        $(P10Shop).html("");
        $(P10Shop).append('<option value="0">-Select Shop-</option>');
        for (i = 0; i < data.length; i++) {
            $(departmentSelect).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P8Shop).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P10Shop).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {

    });

    var P6McName = $("#P6McName");
    $(P6McName).html("");
    $(P6McName).append('<option value="0">--Select Machine--</option>');
    var P8McName = $("#P8McName");
    $(P8McName).html("");
    $(P8McName).append('<option value="0">--Select Machine--</option>');
    var P10McName = $("#P10McName");
    $(P10McName).html("");
    $(P10McName).append('<option value="0">--Select Machine--</option>');
    api.get("/machine/getmachines").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(P6McName).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P8McName).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P10McName).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {
    });
}

function loadDocUploadList() {

    //var content = parseInt($("#ManufacturedPartType").val());
    var inw_Recpt_HeaderId = $("#P7InwHeaderId").val();
    if (isNaN(inw_Recpt_HeaderId)) {
        api.getbulk("/workOrder/InwardDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status==0);
            var tablebody = $("#P6DocUploadGrid tbody");
            var tablebody2 = $("#P6partInspectPlanGrid tbody");
            $(tablebody).html("");//empty tbody
            $(tablebody2).html("");//empty tbody
            //console.log(data);
            for (i = 0; i < data.length; i++) {
                let rowHtml2 = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
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
                $(tablebody).append(rowHtml);
                $(tablebody2).append(rowHtml2);
            }

        }).catch((error) => {
            console.log(error);
        });
    } else {
        api.getbulk("/workOrder/InwardDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status == 0);
            var tablebody2 = $("#P6partInspectPlanGrid tbody");
            var tablebody = $("#P6DocUploadGrid tbody");
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