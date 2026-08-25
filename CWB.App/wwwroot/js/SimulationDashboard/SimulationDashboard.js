var currentView = "Part";
var currentDisplay = "Hour";
var simulationData = null;

var HOUR_BASE_WIDTH = 38; // width that op.left / op.width in your data assume

function getHourWidth() {
    if (currentDisplay === "Shift") return 19;
    if (currentDisplay === "Day") return 8;
    return 38; // Hour-wise
}

function getScaleFactor() {
    return getHourWidth() / HOUR_BASE_WIDTH;
}

$(document).ready(function () {

    $("#ddlDisplay").change(function () {
        currentDisplay = $(this).val();
        refreshDashboard();
    });

    loadSelectedView();

    $("#txtPartNo, #txtMachine").on("keyup", function () {
        if (currentView == "Machine") {
            filterMachineSimulation();
        } else {
            filterPartSimulation();
        }
    });

    var container = $("#timelineContainer");
    container.on("scroll", updateArrows);
    updateArrows();

});

function refreshDashboard() {
    if (currentView == "Machine") {
        loadMachineView();
    } else {
        loadPartView();
    }
}

function loadSelectedView() {

    currentView = $("#ddlView").val();

    $(".content").removeClass("part-view machine-view");

    if (currentView == "Machine") {
        $(".content").addClass("machine-view");
        loadMachineView();
    } else {
        $(".content").addClass("part-view");
        loadPartView();
    }
}

$("#ddlView").change(function () {
    loadSelectedView();
});

function loadPartView() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/SimulationData")
        .then((data) => {
            simulationData = data;
            bindPartSimulation(data);
            scrollToToday();
            $("#preloaderblurred").hide();
        })
        .catch((error) => {
            console.log(error);
            $("#preloaderblurred").hide();
        });
}

function loadMachineView() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/SimulationMachineData")
        .then((data) => {
            simulationData = data;
            bindMachineSimulation(data);
            scrollToToday();
            $("#preloaderblurred").hide();
        })
        .catch((error) => {
            console.log(error);
            $("#preloaderblurred").hide();
        });
}

function scrollToToday() {

    var today = new Date().toLocaleDateString("en-GB");

    $("#dateHeader th").each(function () {
        if ($(this).text().trim() === today) {
            var left = this.offsetLeft - 160; // Machine column width
            $("#timelineContainer").scrollLeft(left);
            return false;
        }
    });
}


// FILTERING


function filterMachineSimulation() {

    var machine = $("#txtMachine").val().toLowerCase().trim();
    var part = $("#txtPartNo").val().toLowerCase().trim();

    if (machine == "" && part == "") {
        bindMachineSimulation(simulationData);
        return;
    }

    var filteredMachines = simulationData.machines.map(function (mc) {

        var operations = mc.operations;

        if (part != "") {
            operations = operations.filter(function (op) {
                return op.partNum.toLowerCase().includes(part);
            });
        }

        var machineMatch = machine == "" || mc.machineNo.toLowerCase().includes(machine);

        if (!machineMatch) return null;
        if (part != "" && operations.length == 0) return null;

        return { ...mc, operations: operations };

    }).filter(function (x) { return x != null; });

    bindMachineSimulation({
        headers: simulationData.headers,
        machines: filteredMachines
    });
}

function filterPartSimulation() {

    var partNo = $("#txtPartNo").val().toLowerCase().trim();
    var machine = $("#txtMachine").val().toLowerCase().trim();

    if (partNo === "" && machine === "") {
        bindPartSimulation(simulationData);
        return;
    }

    var filteredParts = simulationData.parts.map(function (part) {

        var operations = part.operations;

        if (machine !== "") {
            operations = operations.filter(function (op) {
                return op.machineName.toLowerCase().includes(machine);
            });
        }

        var partMatch = partNo === "" || part.partNo.toLowerCase().includes(partNo);

        if (!partMatch) return null;
        if (machine !== "" && operations.length === 0) return null;

        return { ...part, operations: operations };

    }).filter(function (x) { return x != null; });

    bindPartSimulation({
        headers: simulationData.headers,
        parts: filteredParts
    });
}


function updateStickyHeaderPositions() {

    var $dateRow = $("#dateHeader");
    var $shiftRow = $("#shiftHeader");
    var $hourRow = $("#hourHeader");

    var dateHeight = $dateRow.outerHeight() || 0;

    // Date row
    $dateRow.find("th").css("top", "0px");

    // Shift row
    if ($shiftRow.is(":visible")) {
        $shiftRow.find("th").css(
            "top",
            dateHeight + "px"
        );
    }

    // Hour row
    if ($hourRow.is(":visible")) {

        var shiftHeight = $shiftRow.is(":visible")
            ? ($shiftRow.outerHeight() || 0)
            : 0;

        $hourRow.find("th").css(
            "top",
            (dateHeight + shiftHeight) + "px"
        );
    }
}

// HEADERS


function bindMachineHeader(headers) {
    $("#timelineContainer table colgroup").remove();
    $("#timelineContainer table").prepend(buildColgroup(headers, [160]));
    $("#dateHeader").html("");
    $("#shiftHeader").html("");
    $("#hourHeader").html("");

    if (currentDisplay == "Day") {
        $("#shiftHeader").hide();
        $("#hourHeader").hide();
    } else if (currentDisplay == "Shift") {
        $("#shiftHeader").show();
        $("#hourHeader").hide();
    } else {
        $("#shiftHeader").show();
        $("#hourHeader").show();
    }

    var hw = getHourWidth();

    $("#dateHeader").append(`
        <th rowspan="3"
            style="width:160px;
                   min-width:160px;
                   max-width:160px;
                   position:sticky;
                   left:0;
                   background:#f8f9fa;
                   z-index:105;">
            Machine
        </th>
    `);

    for (var i = 0; i < headers.length; i++) {

        var header = headers[i];

        var totalHours = 0;
        for (var j = 0; j < header.shifts.length; j++) {
            totalHours += header.shifts[j].hours.length;
        }

        var isLastDay = (i === headers.length - 1);

        $("#dateHeader").append(`
            <th colspan="${totalHours}" style="width:${totalHours * hw}px; ${isLastDay ? '' : 'border-right:2.5px solid white !important;'}">
                ${new Date(header.date).toLocaleDateString('en-GB')}
            </th>
        `);

        for (var j = 0; j < header.shifts.length; j++) {

            var shift = header.shifts[j];

            if (shift.hours.length == 0) continue;

            var color = "bg-primary text-white";
            if (shift.shiftName == "2nd Shift") color = "bg-warning";
            if (shift.shiftName == "3rd Shift") color = "bg-success text-white";

            $("#shiftHeader").append(`
                <th colspan="${shift.hours.length}" class="${color}" style="width:${shift.hours.length * hw}px;">
                    ${shift.shiftName}
                </th>
            `);

            if (currentDisplay == "Hour") {
                for (var k = 0; k < shift.hours.length; k++) {

                    var hour = shift.hours[k];
                    if (hour == 0) hour = 12;
                    if (hour > 12) hour = hour - 12;

                    $("#hourHeader").append(`
                        <th style="width:${hw}px; height:40px; min-width:${hw}px; max-width:${hw}px; padding:0;">
                            ${hour}
                        </th>
                    `);
                }
            }

        }
    }
    requestAnimationFrame(function () {
        updateStickyHeaderPositions();
    });
}

function bindPartHeader(headers) {
    $("#timelineContainer table colgroup").remove();
    $("#timelineContainer table").prepend(buildColgroup(headers, [160, 80]));
    $("#dateHeader").html("");
    $("#shiftHeader").html("");
    $("#hourHeader").html("");

    if (currentDisplay == "Day") {
        $("#shiftHeader").hide();
        $("#hourHeader").hide();
    } else if (currentDisplay == "Shift") {
        $("#shiftHeader").show();
        $("#hourHeader").hide();
    } else {
        $("#shiftHeader").show();
        $("#hourHeader").show();
    }

    var hw = getHourWidth();

    $("#dateHeader").append(`
    <th rowspan="3"
        style="width:160px;
               min-width:160px;
               max-width:160px;
               position:sticky;
               left:0px;
               background:#f8f9fa;
               z-index:103;">
        Part No
    </th>
`);
    $("#dateHeader").append(`
    <th rowspan="3"
        style="width:80px;
               min-width:80px;
               max-width:80px;
               position:sticky;
               left:160px;
               background:#f8f9fa;
               z-index:104;">
        Urgent
    </th>
`);

    for (var i = 0; i < headers.length; i++) {

        var header = headers[i];

        var totalHours = 0;
        for (var j = 0; j < header.shifts.length; j++) {
            totalHours += header.shifts[j].hours.length;
        }

        var isLastDay = (i === headers.length - 1);

        $("#dateHeader").append(`
            <th colspan="${totalHours}" style="width:${totalHours * hw}px; ${isLastDay ? '' : 'border-right:2.5px solid white !important;'}">
                ${new Date(header.date).toLocaleDateString('en-GB')}
            </th>
        `);

        for (var j = 0; j < header.shifts.length; j++) {

            var shift = header.shifts[j];

            if (shift.hours.length == 0) continue;

            var color = "bg-primary text-white";
            if (shift.shiftName == "2nd Shift") color = "bg-warning";
            if (shift.shiftName == "3rd Shift") color = "bg-success text-white";

            $("#shiftHeader").append(`
                <th colspan="${shift.hours.length}" class="${color}" style="width:${shift.hours.length * hw}px;">
                    ${shift.shiftName}
                </th>
            `);

            if (currentDisplay == "Hour") {
                for (var k = 0; k < shift.hours.length; k++) {

                    var hour = shift.hours[k];
                    if (hour == 0) hour = 12;
                    if (hour > 12) hour = hour - 12;

                    $("#hourHeader").append(`
                        <th style="width:${hw}px; height:40px; min-width:${hw}px; max-width:${hw}px; padding:0;">
                            ${hour}
                        </th>
                    `);
                }
            }
        }
    }
    requestAnimationFrame(function () {
        updateStickyHeaderPositions();
    });
}


// DIVIDER LINES (day / shift / half-shift)


function getDayBoundaries(headers) {
    var boundaries = [];
    var cumulative = 0;

    for (var i = 0; i < headers.length; i++) {
        var dayHours = 0;
        for (var j = 0; j < headers[i].shifts.length; j++) {
            dayHours += headers[i].shifts[j].hours.length;
        }
        cumulative += dayHours;
        if (i < headers.length - 1) {
            boundaries.push(cumulative);
        }
    }
    return boundaries;
}

function getShiftBoundaries(headers) {
    var boundaries = [];
    var cumulative = 0;

    for (var i = 0; i < headers.length; i++) {
        var shifts = headers[i].shifts;
        for (var j = 0; j < shifts.length; j++) {
            var hrs = shifts[j].hours.length;
            if (hrs === 0) continue;

            cumulative += hrs;

            var isLastShiftOfLastDay =
                (i === headers.length - 1) && (j === shifts.length - 1);

            if (!isLastShiftOfLastDay) {
                boundaries.push(cumulative);
            }
        }
    }
    return boundaries;
}

function getShiftHalfBoundaries(headers) {
    var boundaries = [];
    var cumulative = 0;

    for (var i = 0; i < headers.length; i++) {
        var shifts = headers[i].shifts;
        for (var j = 0; j < shifts.length; j++) {
            var hrs = shifts[j].hours.length;
            if (hrs === 0) continue;

            var half = Math.floor(hrs / 2);
            if (half > 0) {
                boundaries.push(cumulative + half);
            }
            cumulative += hrs;
        }
    }
    return boundaries;
}

function buildDividerHtml(headers) {

    var hw = getHourWidth();
    var html = "";
    var dayBoundaries = getDayBoundaries(headers);

    // Day boundary - always shown, dark line
    for (var d = 0; d < dayBoundaries.length; d++) {
        var leftPx = dayBoundaries[d] * hw;
        html += `<div style="position:absolute;left:${leftPx}px;top:0;height:100%;width:2px;background:#6c757d;z-index:2;pointer-events:none;"></div>`;
    }

    // Shift boundary - Shift view AND Day view only 
    if (currentDisplay === "Shift" || currentDisplay === "Day") {

        var dayBoundarySet = {};
        dayBoundaries.forEach(function (b) { dayBoundarySet[b] = true; });

        var shiftBoundaries = getShiftBoundaries(headers);

        for (var s = 0; s < shiftBoundaries.length; s++) {

            if (dayBoundarySet[shiftBoundaries[s]]) continue;

            var leftPx2 = shiftBoundaries[s] * hw;
            html += `<div style="position:absolute;left:${leftPx2}px;top:0;height:100%;width:1px;background:#6c757d;z-index:1;pointer-events:none;"></div>`;
        }
    }

    // Shift half-boundary - Shift view ONLY
    if (currentDisplay === "Shift") {

        var halfBoundaries = getShiftHalfBoundaries(headers);

        for (var h = 0; h < halfBoundaries.length; h++) {
            var leftPx3 = halfBoundaries[h] * hw;
            html += `<div style="position:absolute;left:${leftPx3}px;top:0;height:100%;width:1px;background:#6c757d;z-index:1;pointer-events:none;"></div>`;
        }
    }

    return html;
}


// BODY BINDING (parts / machines) — scaled per currentDisplay


function bindPartSimulation(data) {

    bindPartHeader(data.headers);

    var tableBody = $("#simulationBody");
    tableBody.html("");

    if (data.parts.length == 0) {
        tableBody.append(`
            <tr>
                <td colspan="74" class="text-center">No Records Found</td>
            </tr>`);
        return;
    }

    var totalHours = 0;
    for (var i = 0; i < data.headers.length; i++) {
        for (var j = 0; j < data.headers[i].shifts.length; j++) {
            totalHours += data.headers[i].shifts[j].hours.length;
        }
    }

    var hw = getHourWidth();
    var scale = getScaleFactor();
    var totalWidth = totalHours * hw;

    for (var i = 0; i < data.parts.length; i++) {
        var rowHeight = 49;
        var barHeight = 24;
        var centeredTop = (rowHeight - barHeight) / 2;
        var operationsHtml = "";

        for (var j = 0; j < data.parts[i].operations.length; j++) {

            var op = data.parts[i].operations[j];

            var scaledOp = Object.assign({}, op, {
                left: op.left * scale,
                width: op.width * scale,
                top: centeredTop
            });

            operationsHtml += AppUtil.ProcessTemplateData("partOperationTemplate", scaledOp);

            if (j < data.parts[i].operations.length - 1) {
                var dividerLeft = (op.left + op.width) * scale;
                operationsHtml += `<div style="position:absolute;left:${dividerLeft}px;top:${centeredTop - 4}px;height:${barHeight + 8}px;width:1px;background:#fff;z-index:4;pointer-events:none;"></div>`;
            }
        }

        operationsHtml += buildDividerHtml(data.headers);

        var rowHeight = 49;


        var soMarker = "";

        if (data.parts[i].hasSoCompletionMarker) {

            var markerLeft = data.parts[i].soCompletionMarkerLeft * scale;
            var markerWidth = Math.max(36 * scale, 4);

            var soTooltip = `
        <b>Customer:</b> ${data.parts[i].soCustomer || ""}<br>
        <b>Part No / Desc:</b> ${data.parts[i].partNo || ""} / ${data.parts[i].description || ""}<br>
        <b>SO No:</b> ${data.parts[i].soNo || ""}<br>
        <b>Quantity:</b> ${data.parts[i].soQnty || ""}<br>
        <b>Completion Date:</b> ${data.parts[i].soCompletionDateDisplay || ""}
    `;

            soMarker = `
        <div class="so-completion-marker"
             data-bartype="SoMarker"
             data-tooltip="${encodeURIComponent(soTooltip)}"
             style="left:${markerLeft}px; width:${markerWidth}px;">
        </div>`;
        }
        var matlMarker = "";

        if (data.parts[i].hasMatlReceiptMarker) {

            var matlLeft = data.parts[i].matlReceiptMarkerLeft * scale;

            var typeLabel = data.parts[i].matlReceiptPartType === "RawMaterial"
                ? "Raw Material"
                : data.parts[i].matlReceiptPartType;

            var matlTooltip = `
        <b>${typeLabel || ""}</b><br>
        <b>Supplier:</b> ${data.parts[i].matlReceiptSupplier || ""}<br>
        <b>Part No / Desc:</b> ${data.parts[i].matlReceiptPartNoDesc || ""}<br>
        <b>PO No:</b> ${data.parts[i].matlReceiptPoNo || ""}<br>
        <b>Qnty:</b> ${data.parts[i].matlReceiptQnty || ""}<br>
        <b>Plan Receipt Date:</b> ${data.parts[i].matlReceiptDateDisplay || ""}
    `;

            matlMarker = `
        <div class="matl-receipt-marker"
             data-bartype="MatlMarker"
             data-tooltip="${encodeURIComponent(matlTooltip)}"
             style="left:${matlLeft}px; width:36px;">
        </div>`;
        }

        var row = {
            partNo: data.parts[i].partNo,
            partNoTitle: data.parts[i].partNo,
            urgent: data.parts[i].isUrgent ? "Y" : "N",
            partNoColor: data.parts[i].partNoColor,
            operations: operationsHtml + soMarker + matlMarker,
            soMarker: soMarker,
            rowHeight: rowHeight,
            totalHours: totalHours,
            totalWidth: totalWidth
        };
        tableBody.append(AppUtil.ProcessTemplateData("partSimulationRow", row));


    }

    $('[data-bs-toggle="popover"]').popover('dispose');
    $('[data-bs-toggle="popover"]').popover({ html: true, trigger: 'hover' });
}

function bindMachineSimulation(data) {

    bindMachineHeader(data.headers);

    var tableBody = $("#simulationBody");
    tableBody.html("");

    if (data.machines.length == 0) {
        tableBody.append(`
            <tr>
                <td colspan="74" class="text-center">No Records Found</td>
            </tr>`);
        return;
    }
    tableBody.find('.operation-bar[data-customer=""]').find('.customer-line').hide();
    var totalHours = 0;
    for (var i = 0; i < data.headers.length; i++) {
        for (var j = 0; j < data.headers[i].shifts.length; j++) {
            totalHours += data.headers[i].shifts[j].hours.length;
        }
    }

    var hw = getHourWidth();
    var scale = getScaleFactor();
    var totalWidth = totalHours * hw;

    for (var i = 0; i < data.machines.length; i++) {

        var rowHeight = 49;
        var barHeight = 24;
        var centeredTop = (rowHeight - barHeight) / 2;
        var operationsHtml = "";

        for (var j = 0; j < data.machines[i].operations.length; j++) {

            var op = data.machines[i].operations[j];

            var barHeight = 24; // matches the height in your template
            var rowHeight = 49;
            var centeredTop = (rowHeight - barHeight) / 2;

            var scaledOp = Object.assign({}, op, {
                left: op.left * scale,
                width: op.width * scale,
                top: centeredTop
            });

            operationsHtml += AppUtil.ProcessTemplateData("machineOperationTemplate", scaledOp);
            if (j < data.machines[i].operations.length - 1) {
                var dividerLeft = (op.left + op.width) * scale;
                operationsHtml += `<div style="position:absolute;left:${dividerLeft}px;top:6px;height:32px;width:1px;background:#fff;z-index:4;pointer-events:none;"></div>`;
            }
        }

        operationsHtml += buildDividerHtml(data.headers);

        var rowHeight = 48;

        var row = {
            machineNo: data.machines[i].machineNo,
            operations: operationsHtml,
            rowHeight: rowHeight,
            totalHours: totalHours,
            totalWidth: totalWidth
        };

        tableBody.append(AppUtil.ProcessTemplateData("machineSimulationRow", row));
    }
    tableBody.find('.operation-bar[data-supplier=""]').find('.supplier-line').hide();
    tableBody.find('.operation-bar[data-machine=""]').find('.machine-line').hide();

    $('[data-bs-toggle="popover"]').popover('dispose');
    $('[data-bs-toggle="popover"]').popover({ html: true, trigger: 'hover' });
}


function updateArrows() {

    var container = $("#timelineContainer");
    var left = container.scrollLeft();
    var max = container[0].scrollWidth - container.outerWidth();

    if (left > 30) $("#btnPrev").fadeIn();
    else $("#btnPrev").fadeOut();

    if (left < max - 30) $("#btnNext").fadeIn();
    else $("#btnNext").fadeOut();
}

$("#btnNext").click(function () {
    var container = $("#timelineContainer");
    container.animate({ scrollLeft: container.scrollLeft() + 900 }, 300);
});

$("#btnPrev").click(function () {
    var container = $("#timelineContainer");
    container.animate({ scrollLeft: container.scrollLeft() - 900 }, 300);
});




function loadSelectedView() {

    currentView = $("#ddlView").val();

    $(".content").removeClass("part-view machine-view");

    if (currentView == "Machine") {
        $(".content").addClass("machine-view");

        $("#legendPartView").removeClass('d-flex').addClass('d-none');
        $("#legendMachineView").removeClass('d-none').addClass('d-flex');

        loadMachineView();
    } else {
        $(".content").addClass("part-view");

        $("#legendMachineView").removeClass('d-flex').addClass('d-none');
        $("#legendPartView").removeClass('d-none').addClass('d-flex');

        loadPartView();
    }
}


//$(document).on('mouseenter', '.operation-bar', function () {
//    var $bar = $(this);
//    var barType = $bar.attr('data-bartype');

//    var $source;
//    if (barType === 'SoMarker') {
//        $source = $bar.find('.operation-tooltip-source');
//    } else if (barType === 'NonPlan') {
//        $source = $bar.find('.tooltip-nonplan');
//    } else if (barType === 'Subcon') {
//        $source = $bar.find('.tooltip-subcon');
//    } else {
//        $source = $bar.find('.tooltip-inhouse');
//    }

//    var sourceHtml = $source.html();
//    var $tooltip = $('#sharedTooltip');
//    $tooltip.html(sourceHtml);

//    var barRect = this.getBoundingClientRect();
//    $tooltip.css({
//        display: 'block',
//        left: (barRect.left + barRect.width / 2) + 'px',
//        top: (barRect.top - 10) + 'px',
//        transform: 'translate(-50%, -100%)'
//    });
//});

$(document).on('mouseenter', '.operation-bar, .so-completion-marker, .matl-receipt-marker', function () {

    var $element = $(this);
    var sourceHtml = "";

    // Operation bar
    if ($element.hasClass('operation-bar')) {

        var barType = $element.attr('data-bartype');

        if (barType === 'NonPlan') {
            sourceHtml = $element.find('.tooltip-nonplan').html();
        }
        else if (barType === 'Subcon') {
            sourceHtml = $element.find('.tooltip-subcon').html();
        }
        else {
            sourceHtml = $element.find('.tooltip-inhouse').html();
        }
    }

    // SO completion marker
    else if ($element.hasClass('so-completion-marker')) {

        sourceHtml = decodeURIComponent(
            $element.attr('data-tooltip') || ''
        );
    }

    // Material receipt marker
    else if ($element.hasClass('matl-receipt-marker')) {

        sourceHtml = decodeURIComponent(
            $element.attr('data-tooltip') || ''
        );
    }

    if (!sourceHtml) {
        $('#sharedTooltip').hide();
        return;
    }

    var $tooltip = $('#sharedTooltip');

    // Show temporarily so we can calculate its size
    $tooltip
        .html(sourceHtml)
        .css({
            display: 'block',
            visibility: 'hidden'
        });

    var elementRect = this.getBoundingClientRect();

    var tooltipWidth = $tooltip.outerWidth();
    var tooltipHeight = $tooltip.outerHeight();

    var left = elementRect.left + (elementRect.width / 2);
    var top;



    if (left - (tooltipWidth / 2) < 10) {
        left = (tooltipWidth / 2) + 10;
    }

    if (left + (tooltipWidth / 2) > window.innerWidth - 10) {
        left = window.innerWidth - (tooltipWidth / 2) - 10;
    }


    if (elementRect.top - tooltipHeight - 10 >= 0) {

        top = elementRect.top - 10;

        $tooltip.css({
            left: left + 'px',
            top: top + 'px',
            transform: 'translate(-50%, -100%)',
            visibility: 'visible'
        });

    }
    else {

        top = elementRect.bottom + 10;

        $tooltip.css({
            left: left + 'px',
            top: top + 'px',
            transform: 'translateX(-50%)',
            visibility: 'visible'
        });
    }
}
);


$(document).on('mouseleave', '.operation-bar, .so-completion-marker, .matl-receipt-marker', function () {

    $('#sharedTooltip').stop(true, true).hide().html('');
}
);

function buildColgroup(headers, firstColWidths) {
    // firstColWidths = [160] for machine view, [160, 80] for part view
    var hw = getHourWidth();
    var html = "<colgroup>";

    firstColWidths.forEach(function (w) {
        html += `<col style="width:${w}px;">`;
    });

    for (var i = 0; i < headers.length; i++) {
        var shifts = headers[i].shifts;
        for (var j = 0; j < shifts.length; j++) {
            var hrs = shifts[j].hours.length;
            for (var k = 0; k < hrs; k++) {
                html += `<col style="width:${hw}px;">`;
            }
        }
    }

    html += "</colgroup>";
    return html;
}



