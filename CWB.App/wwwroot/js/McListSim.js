let machineTypes = {};
let machineMasterList = {};
let shopList = {};
let originalRows = [];
function ViewFile() {
    document.getElementById('preloader').style.display = 'block';
    document.getElementById('status').style.display = 'block';
    var filename = "InfoImage.png";
    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/masters/ViewFile?fileName=' + filename, true);
    xhr.responseType = 'arraybuffer';
    xhr.onload = function (e) {
        if (this.status == 200) {
            var blob = new Blob([this.response], { type: "image/png" });

            const objectElement = document.getElementById('fileViewer');
            const url = URL.createObjectURL(blob);
            objectElement.src = url;
            document.getElementById('preloader').style.display = 'none';
            document.getElementById('status').style.display = 'none';
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


$(document).ready(function () {
    $('#InfoPopup').on('show.bs.modal', function (event) {
        ViewFile();
    });
    loadSels();
    loadMcLoads();
    $("#P2SearchMcType").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P21Grid tbody tr").show();
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P21Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallow) > -1)
            });
        } 
    });
    $("#P2SearchShop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P21Grid tbody tr").show();
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P21Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(selvallow) > -1)
            });
        } 
    });
    $("#P22SearchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P22Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#P22SearchPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P22Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $('#Popup22').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        closeP26 = 0;
        var mcname = relatedTarget.data("mcname");
        var shop = relatedTarget.data("shop");
        var mcid = relatedTarget.data("mcid");
        var datemcnot = relatedTarget.data("datemcnot");
        var freehr = relatedTarget.data("freehr");
        var simhr = relatedTarget.data("simhr");
        var hrbook = relatedTarget.data("hrbook");
        var mcnothr = relatedTarget.data("mcnothr");
        var nonhr = relatedTarget.data("nonhr");
        var rewkhr = relatedTarget.data("rewkhr");
        var findmc = machineMasterList.filter(mc => mc.machineId == mcid);
        $("#McNotPop").attr("data-mcid", mcid);
        $("#nonWorKRow").hide();
        $("#reWorKRow").hide();
        if (mcnothr === "00:00") {
            $("#notAvlMcRow").hide();
        } else {
            $("#notAvlMcRow").show();
            var notMin = timeToMinutes(mcnothr);
            var simMin = timeToMinutes(simhr);

            var notPercent = simMin > 0 ? ((notMin / simMin) * 100).toFixed(2) : 0;

            $("#McNotTimePer").text(notPercent + " %");
            $("#McNotTime").text(mcnothr);
        }
        if (freehr === "00:00") {
            $("#McWaitTimePer").text("0 %");
            $("#McWaitTime").text(freehr);
        } else {

            var freeMin = timeToMinutes(freehr);
            var simMin = timeToMinutes(simhr);

            var percent = simMin > 0 ? ((freeMin / simMin) * 100).toFixed(2) : 0;

            $("#McWaitTimePer").text(percent + " %");
            $("#McWaitTime").text(freehr);
        }
        $("#McBookTimePer").text("100 %");
        $("#McBookTime").text(hrbook);
        $("#SimDurTimePer").text("100 %");
        $("#SimDurTime").text(simhr);
        $("#P22McName").text(mcname);
        $("#P22SlNo").text(findmc[0].slNo);
        $("#P22ShopName").text(shop);
        $("#datemcnotloaded").text(datemcnot);
        loadMcSeq(mcid);
    });
    function timeToMinutes(timeStr) {
        var parts = timeStr.split(":");
        return parseInt(parts[0]) * 60 + parseInt(parts[1]);
    }
    $('#Popup13').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup22').style.filter = 'none';
    });
    $('#Popup13').on('show.bs.modal', function (event) {
        $("#P13SearchShop").prop("disabled", false);
        $("#P13SearchMc").prop("disabled", false);
        var relatedTarget = $(event.relatedTarget);
        var mcid = relatedTarget.data("mcid");
        loadNotAvlMcTimeslot(mcid);
        document.getElementById('Popup22').style.filter = 'blur(5px)';
    });
    originalRows = $("#P13Grid tbody tr").get();
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
            rows = originalRows;
        }

        $.each(rows, function (index, row) {
            $tbody.append(row); // Re-append in sorted order
        });
    });
});
function parseDateTime(dateStr) {
    let [datePart, timePart, meridian] = dateStr.split(" ");
    let [dd, mm, yyyy] = datePart.split("-");
    let fullTimeStr = `${yyyy}-${mm}-${dd} ${timePart} ${meridian}`;
    return new Date(fullTimeStr);
}

function formatHrs(totalHrs) {
    var hrs = Math.floor(totalHrs);
    var mins = Math.round((totalHrs - hrs) * 60);
    return `${hrs.toString().padStart(2, '0')}:${mins.toString().padStart(2, '0')}`;
}
function loadMcLoads() {
    var tablebody = $("#P21Grid tbody");
    $(tablebody).html(""); // empty tbody
    document.getElementById('preloader').style.display = 'block';
    document.getElementById('status').style.display = 'block';
    api.getbulk("/workOrder/GetAllMc_Wait_List").then((data) => {
        var waitList = data;
        for (let i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("P21GridRow", data[i]);
            let $row = $(row);
            if (data[i].mcNotAvlHrs != "00:00") {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('Show M/c Non Avl. Hrs')").remove();
            }
            $row.find(".dropdown-item:contains('Edit Non Plan Work')").remove();
            $row.find(".dropdown-item:contains('Edit Rework')").remove();
            $(tablebody).append($row);
            $("#P21Grid td").css("white-space", "nowrap");
        }
        document.getElementById('preloader').style.display = 'none';
        document.getElementById('status').style.display = 'none';
        var summaryMap = {};
        waitList.forEach(item => {
            let shop = item.shopName || "Unknown Shop";
            let type = item.mcTypeName || "Unknown Type";
            let mcName = item.mcName || "Unknown Machine";
            let mcId = item.mc_Id;

            if (!mcId) return; // skip invalid entry

            let key = shop + '|' + type;

            let hrs = timeFlotsParse(item.hrsBooked) || 0;

            if (!summaryMap[key]) {
                summaryMap[key] = {
                    shop: shop,
                    machineType: type,
                    totalHrs: 0,
                    usedMachines: {},
                    allMcIds: new Set()
                };
            }

            summaryMap[key].totalHrs += hrs;
            summaryMap[key].usedMachines[mcName] = (summaryMap[key].usedMachines[mcName] || 0) + hrs;
            summaryMap[key].allMcIds.add(mcId);
        });

        // calculate No of Mcs Available from machine master
        Object.values(summaryMap).forEach(group => {
            let shopId = shopList.find(s => s.name === group.shop)?.departmentId || null;
            let typeId = machineTypes.find(t => t.machineTypeName === group.machineType)?.machineTypeTypeId || null;

            let mcAvl = machineMasterList.filter(m =>
                (!shopId || m.shopId === shopId) &&
                (!typeId || m.machineTypeId === typeId)
            );

            group.mcAvlCount = mcAvl.length - Object.keys(group.usedMachines).length;
            group.mcUsedCount = Object.keys(group.usedMachines).length;

            let sorted = Object.entries(group.usedMachines).sort((a, b) => b[1] - a[1]);

            group.highest = sorted[0] ? `${sorted[0][0]} (${formatHrs(sorted[0][1])})` : '-';
            group.lowest = sorted[sorted.length - 1] ? `${sorted[sorted.length - 1][0]} (${formatHrs(sorted[sorted.length - 1][1])})` : '-';
            group.totalHrsFormatted = formatHrs(group.totalHrs);
        });

        // create table rows
        let tbody = $('#P212Grid tbody');
        tbody.empty();

        Object.values(summaryMap).forEach(group => {
            let tr = `
<tr>
    <td>${group.shop}</td>
    <td>${group.machineType}</td>
    <td>${group.totalHrsFormatted}</td>
    <td>${group.mcAvlCount}</td>
    <td>${group.mcUsedCount}</td>
    <td>${group.highest}</td>
    <td>${group.lowest}</td>
</tr>`;

            tbody.append(tr);
        });
    }).catch((error) => {
        console.error("Failed to load WOs", error);
    });
}
function timeFlotsParse(timestr) {
    var pt = timestr.split(":");
    var hrs = parseInt(pt[0], 10);
    var min = parseInt(pt[1], 10);
    return hrs + (min / 60);
}
function loadSels(){

    var compSelect = $("#P2SearchMcType");
    $(compSelect).html("");
    $(compSelect).append('<option value="0">--Select Machince Type--</option>');
    api.get("/machine/getmachinetypes").then((data) => {
        machineTypes = data;
        for (i = 0; i < data.length; i++) {
            $(compSelect).append('<option value="' + data[i].machineTypeName + '">' + data[i].machineTypeName + '</option>');
        }
    }).catch((error) => {
    });
    api.get("/machine/getmachines").then((data) => {
        machineMasterList = data;
        var P13SearchMc = $("#P13SearchMc");
        $(P13SearchMc).html("");
        $(P13SearchMc).append('<option value="0">--Select Machine--</option>');
        for (i = 0; i < data.length; i++) {
            $(P13SearchMc).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {
    });
    api.get("/department/getdepartments/" + 1).then((data) => {
        shopList = data;
        var departmentSelect = $("#P2SearchShop");
        var P13SearchShop = $("#P13SearchShop");
        $(P13SearchShop).html("");
        $(departmentSelect).html("");
        $(departmentSelect).append('<option value="0">--Select Shop--</option>');
        $(P13SearchShop).append('<option value="0">--Select Shop--</option>');
        for (i = 0; i < data.length; i++) {
            $(departmentSelect).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P13SearchShop).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {

    });
}
function loadNotAvlMcTimeslot(mcid) {

    var tablebody = $("#P13Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetNotAvlMcTimeslot").then((data) => {
        data = data.filter(item => item.mc_Id == parseInt(mcid));
        $("#P13SearchShop").val(data[0].shopName);
        $("#P13SearchMc").val(data[0].mcName);
        $("#P13SearchShop").prop("disabled", true);
        $("#P13SearchMc").prop("disabled", true);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P13GridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function loadMcSeq(mcid) {

    var tablebody = $("#P22Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllMcWos?mcId="+ mcid).then((data) => {
        for (i = 0; i < data.length; i++) {
            let row = AppUtil.ProcessTemplateData("P22GridRow", data[i]);
            let $row = $(row);
            if (data[i].non_Plan_Wk === "Y") {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('Edit Non Plan Work')").remove();
            }
            if (data[i].rework_Wo === "Y") {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('Edit Rework')").remove();
            }
            $(tablebody).append($row);
            $("#P22Grid td").css("white-space", "nowrap");
        }
    }).catch((error) => {
    });
}