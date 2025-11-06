
$(document).ready(function () {
    loadPartinShop();
    loadSels();
    $("#P4Shop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P4Grid tbody tr").show();
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P4Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P4Grid tbody");
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
    $("#P4McName").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P4Grid tbody tr").show();
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P4Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P4Grid tbody");
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
    $("#P4PartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P4Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
            var $tableBody = $("#P4Grid tbody");
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
function loadPartinShop() {
    var tablebody = $("#P4Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();

    api.getbulk("/workOrder/GetPartsLoadedInShop").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P4GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadSels() {
    api.get("/department/getdepartments/" + 1).then((data) => {
        var departmentSelect = $("#P1SearchShop");
        $(departmentSelect).html("");
        var P4Shop = $("#P4Shop");
        $(P4Shop).html("");
        var P2FromLoc = $("#P2FromLoc");
        $(P2FromLoc).html("");
        var P2ToLoc = $("#P2ToLoc");
        $(P2ToLoc).html("");
        $(departmentSelect).append('<option value="0">-Select Shop-</option>');
        $(P4Shop).append('<option value="0">-Select Shop-</option>');
        $(P2FromLoc).append('<option value="0">-Select Shop-</option>');
        $(P2ToLoc).append('<option value="0">-Select Shop-</option>');
        $(P2ToLoc).append('<option value="Stores">Stores</option>');
        for (i = 0; i < data.length; i++) {
            $(departmentSelect).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P4Shop).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P2FromLoc).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
            $(P2ToLoc).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {

    });

    var compSelect = $("#P3Subcon");
    var P5Subcon = $("#P5Subcon");
    $(compSelect).html("");
    $(P5Subcon).html("");
    $(compSelect).append('<option value="0">--Select SubCon--</option>');
    $(P5Subcon).append('<option value="0">--Select SubCon--</option>');
    api.get("/masters/suppliers").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(compSelect).append('<option value="' + data[i].companyName + '">' + data[i].companyName + '</option>');
            $(P5Subcon).append('<option value="' + data[i].companyName + '">' + data[i].companyName + '</option>');
        }
    }).catch((error) => {
    });
    var P4McName = $("#P4McName");
    $(P4McName).html("");
    $(P4McName).append('<option value="0">--Select Machine--</option>');
    api.get("/machine/getmachines").then((data) => {
        for (i = 0; i < data.length; i++) {
            $(P4McName).append('<option value="' + data[i].name + '">' + data[i].name + '</option>');
        }
    }).catch((error) => {
    });
}