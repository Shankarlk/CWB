let golsupid = 0;
let isP81Modified = false;
$(document).ready(function () {
    loadSels();
    loadSubcon();
    $("#P23SearchSub").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P23Grid tbody tr").show();
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P23Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P23Grid tbody");
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
    $("#P24SearchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P24Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P24Grid tbody");
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
    $("#P24SearchPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P24Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P24Grid tbody");
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
    $('#Popup24').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var suppid = relatedTarget.data("suppid");
        var supp = relatedTarget.data("supp");
        $("#P24McName").text(supp);
        loadSubConWos(suppid);
        golsupid = suppid;
    });
    $('#Popup25').on('hide.bs.modal', function (event) {
        if (isP81Modified) {
            event.preventDefault();
            $("#openfrom").val("P81");
            $("#ErrorMessage1").modal("show");
        }
    });
    $("#ErBtn1").on('click', function (event) {
        $("#P25Save").click();
        $("#ErrorMessage1").modal("hide");
    });
    $("#ErBtn2").on('click', function (event) {
        $("#ErrorMessage1").modal("hide");
    });
    $("#ErBtn3").on('click', function (event) {
        isP81Modified = false;
        $("#Popup25").modal("hide");
        $("#ErrorMessage1").modal("hide");
    });
    $('#Popup25').on('show.bs.modal', function (event) {
        $("#P25DispNew").val('');
        $("#P25RecptNew").val('');
        $("#openfrom").val("P25")
        var P25DispNew = document.getElementById('P25DispNew');
        P25DispNew.style.border = '';
        var P25RecptNew = document.getElementById('P25RecptNew');
        P25RecptNew.style.border = '';
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var part = relatedTarget.data("part");
        var qnty = relatedTarget.data("qnty");
        var routi = relatedTarget.data("routi");
        var opno = relatedTarget.data("opno");
        var disp = relatedTarget.data("disp");
        var recp = relatedTarget.data("recp");
        var actdisp = relatedTarget.data("actdisp");
        var actrecp = relatedTarget.data("actrecp");
        var subc = $("#P24McName").text();
        $("#P25SubName").text(subc);
        $("#P25PartName").text(part);
        $("#P25RoutName").text(routi + " / " + opno);
        $("#P25QntyName").text(qnty);
        $("#P25SubconId").val(id);
        $("#P25DispPrev").val(formatDate(disp));
        $("#P25RecptPrev").val(formatDate(recp));
    });
    $("#P25Save").secureClick( function (event) {
        var P25DispNew = $("#P25DispNew").val();
        var P25RecptNew = $("#P25RecptNew").val();
        if (P25DispNew.length === 0) {
            var newNamevalidate = document.getElementById('P25DispNew');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P25DispNew');
            newNamevalidate.style.border = '';
        }
        if (P25RecptNew.length === 0) {
            var newNamevalidate = document.getElementById('P25RecptNew');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P25RecptNew');
            newNamevalidate.style.border = '';
        }
        const today = new Date();
        today.setHours(0, 0, 0, 0); // normalize to midnight

        const dispNewVal = $("#P25DispNew").val();
        const recptNewVal = $("#P25RecptNew").val();
        const dispPrevVal = $("#P25DispPrev").val();
        const recptPrevVal = $("#P25RecptPrev").val();
        var P25SubconId = parseInt($("#P25SubconId").val());
        let errorMsg = "";

        // Convert values to Date objects
        const dispNew = dispNewVal ? new Date(dispNewVal) : null;
        const recptNew = recptNewVal ? new Date(recptNewVal) : null;
        const dispPrev = dispPrevVal ? parseDateStr(dispPrevVal) : null;
        const recptPrev = recptPrevVal ? parseDateStr(recptPrevVal) : null;

        if (!dispNew || dispNew <= today) {
            errorMsg += " New Dispatch Date must be greater than today.";
        }

        if (errorMsg) {
            alert(errorMsg);
            return false;
        }
        const compareDisp = dispNew || dispPrev;

        if (!recptNew || (compareDisp && recptNew <= compareDisp)) {
            errorMsg += " New Receipt Date must be greater than the New Dispatch Date or Plan / Previous Dispatch Date.";
        }

        if (errorMsg) {
            alert(errorMsg);
            return false;
        }
        var formdata = {
            tempSubCon_ListId: parseInt($("#P25SubconId").val()),
            plan_Disp_date: $("#P25DispNew").val(),
            plan_Recpt_date: $("#P25RecptNew").val()
        };
        return api.post("/WorkOrder/UpdateTempSubCon_List", formdata).then((data) => {
            alert("Subcon Dispatch Date and Receipt Date Saved");
            isP81Modified = false;
            $("#Popup25").modal("hide");
            loadSubConWos(golsupid);
        }).catch((error) => {
            console.log(error);
        });

    });
    $('#Popup25 input').on('input', function () {
        isP81Modified = true;
    });
});
function parseDateStr(str) {
    if (!str) return null;
    const parts = str.split("-");
    if (parts.length !== 3) return null;
    const [day, month, year] = parts;
    return new Date(`${year}-${month}-${day}`);
}
function formatDate(input) {
    const date = new Date(input);
    if (isNaN(date) || input.startsWith("0001")) {
        return ""; // Blank if .NET default or invalid date
    }

    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear(); // <-- full 4-digit year

    return `${day}-${month}-${year}`; // Format: dd-MM-yyyy
}
function loadSels() {

    var compSelect = $("#P23SearchSub");
    $(compSelect).html("");
    $(compSelect).append('<option value="0">--Select SubCon--</option>');
    api.get("/masters/suppliers").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(compSelect).append('<option value="' + data[i].companyName + '">' + data[i].companyName + '</option>');
        }
    }).catch((error) => {
    });
}
function loadSubcon() {

    var tablebody = $("#P23Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllSubconWos").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P23GridRow", data[i]));
        }
    }).catch((error) => {
    });
    $.ajax({
        url: '/WorkOrder/GetSubConMachineHoursMatrix',
        method: 'GET',
        success: function (data) {
            const columns = data.columns;
            const rows = data.rows;

            // 1. Build table headers
            let headerHtml = '';
            columns.forEach(col => {
                headerHtml += `<th>${col}</th>`;
            });
            $('#P23MISHeaderRow').html(headerHtml);

            // 2. Build table rows
            let bodyHtml = '';
            rows.forEach(row => {
                bodyHtml += '<tr>';
                columns.forEach(col => {
                    bodyHtml += `<td>${row[col] !== undefined ? row[col] : ''}</td>`;
                });
                bodyHtml += '</tr>';
            });
            $('#P23MISBody').html(bodyHtml);
        },
        error: function (err) {
            console.error('Error loading SubCon Matrix:', err);
        }
    });
}
function loadSubConWos(Suppid) {

    var tablebody = $("#P24Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllSimWosBySuppiler?Suppid=" + Suppid).then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P24GridRow", data[i]));
        }
    }).catch((error) => {
    });
}