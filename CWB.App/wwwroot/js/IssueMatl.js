function loadMisCount() {
    $("#preloaderblurred").show();
    api.getbulk("/workOrder/GetAllMatl_Issue_List").then((data) => {
        let unique = [];
        let seen = new Set();

        for (let item of data) {
            // Build unique key including opNo to allow different operations
            let key = `${item.shop}|${item.woNumber}|${item.partNo}|${item.routingName}|${item.opNo}`;

            if (!seen.has(key)) {
                seen.add(key);
                unique.push(item);
            }
        }
        var datacount = unique.length;
        $("#PlanStore1").text('0');
        $("#ReadStore1").text('0');
        $("#PlanStore2").text(datacount);
        $("#ReadStore2").text('0');
        $("#PlanStore3").text('0');
        $("#ReadStore3").text('0');
        $("#PlanStore4").text('0');
        $("#ReadStore4").text('0');
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
    api.getbulk("/workOrder/GetAllIssueSubCon").then((data) => {
        var datacount = data.length;
        $("#PlanStore5").text(datacount);
        $("#ReadStore5").text('0');
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}




$(document).ready(function () {
    loadMisCount();
    loadSels();
    $('#Popup1').on('show.bs.modal', function (event) {
        $("#P1SearchShop").val(0);
        loadIssueShop();
    });
    $('#P1Select').on('change', function () {
        var isChecked = $(this).is(':checked');
        $('.P1gridChk').prop('checked', isChecked);
    });
    $("#P1SearchShop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P1Grid tbody tr").show();
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P1Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P1Grid tbody");
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
    $('#P1Issue').secureClick( function () {
        let selectedIds = [];

        $('#P1Grid tbody tr').each(function () {
            const checkbox = $(this).find('.P1gridChk');
            if (checkbox.is(':checked')) {
                // Get cell at index 10 (11th column)
                const idText = $(this).find('td').eq(10).text().trim();
                const id = parseInt(idText);
                if (!isNaN(id)) {
                    selectedIds.push(id);
                }
            }
        });

        if (selectedIds.length === 0) {
            alert("Please select at least one row.");
            return;
        }

        // AJAX POST
        document.getElementById('preloader').style.display = 'block';
        document.getElementById('status').style.display = 'block';
        return $.ajax({
            url: '/WorkOrder/PostIssueInv_Trans_Log', // Replace with your controller
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(selectedIds),
            success: function (res) {
                alert(res.message);
                console.log(res);
                loadIssueShop();
                loadMisCount();
                document.getElementById('preloader').style.display = 'none';
                document.getElementById('status').style.display = 'none';
            },
            error: function (err) {
                document.getElementById('preloader').style.display = 'none';
                document.getElementById('status').style.display = 'none';
                //alert("Error during submission.");
                console.error(err);
            }
        });
    });
    $('#Popup2').on('show.bs.modal', function (event) {
        $("#P2FromLoc").val(0);
        $("#P2ToLoc").val(0);
        var relatedTarget = $(event.relatedTarget);
        var btn = relatedTarget.data("btn");
        if (btn == "btn1") {
            $("#P2ToLoc").val('Stores');
            loadIssueShopToStores('Stores');
        } else {
            $("#P2ToLoc").val(0);
            loadIssueShopToStores();
        }
    });
    $("#P2FromLoc").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P2Grid tbody tr").show();
            var $tableBody = $("#P2Grid tbody");
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
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P2Grid tbody");
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
    $("#P2ToLoc").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P2Grid tbody tr").show();
            var $tableBody = $("#P2Grid tbody");
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
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P2Grid tbody");
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
    $('#P2SelectAll').on('change', function () {
        var isChecked = $(this).is(':checked');
        $('.P2gridChk').prop('checked', isChecked);
    });
    $('#P2Movt').secureClick( function () {
        let selectedIds = [];

        $('#P2Grid tbody tr').each(function () {
            const checkbox = $(this).find('.P2gridChk');
            if (checkbox.is(':checked')) {
                // Get cell at index 10 (11th column)
                const idText = $(this).find('td').eq(11).text().trim();
                const id = parseInt(idText);
                if (!isNaN(id)) {
                    selectedIds.push(id);
                }
            }
        });

        if (selectedIds.length === 0) {
            alert("Please select at least one row.");
            return;
        }

        // AJAX POST
        document.getElementById('preloader').style.display = 'block';
        document.getElementById('status').style.display = 'block';
        return $.ajax({
            url: '/WorkOrder/PostIssueInv_Trans_Log', // Replace with your controller
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(selectedIds),
            success: function (res) {
                alert(res.message);
                console.log(res);
                document.getElementById('preloader').style.display = 'none';
                document.getElementById('status').style.display = 'none';
            },
            error: function (err) {
                document.getElementById('preloader').style.display = 'none';
                document.getElementById('status').style.display = 'none';
                //alert("Error during submission.");
                console.error(err);
            }
        });
    });
    $('#Popup3').on('show.bs.modal', function (event) {
        loadIssueSubcon();
        $("#P3Subcon").val(0);
    });
    $('#P3SelectAll').on('change', function () {
        var isChecked = $(this).is(':checked');
        $('.P3gridChk').prop('checked', isChecked);
    });
    $("#P3Subcon").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P3Grid tbody tr").show();
            var $tableBody = $("#P3Grid tbody");
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
            $("#P3Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#P3Grid tbody");
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
    $('#P3Issue').secureClick( function () {
        let selectedIds = [];

        $('#P3Grid tbody tr').each(function () {
            const checkbox = $(this).find('.P3gridChk');
            if (checkbox.is(':checked')) {
                // Get cell at index 10 (11th column)
                const idText = $(this).find('td').eq(9).text().trim();
                const id = parseInt(idText);
                if (!isNaN(id)) {
                    selectedIds.push(id);
                }
            }
        });

        if (selectedIds.length === 0) {
            alert("Please select at least one row.");
            return;
        }

        // AJAX POST
        document.getElementById('preloader').style.display = 'block';
        document.getElementById('status').style.display = 'block';
        return $.ajax({
            url: '/WorkOrder/PostIssueInv_Trans_Log', // Replace with your controller
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(selectedIds),
            success: function (res) {
                alert(res.message);
                console.log(res);
                loadIssueSubcon();
                loadMisCount();
                document.getElementById('preloader').style.display = 'none';
                document.getElementById('status').style.display = 'none';
            },
            error: function (err) {
                document.getElementById('preloader').style.display = 'none';
                document.getElementById('status').style.display = 'none';
                //alert("Error during submission.");
                console.error(err);
            }
        });
    });
    $('#Popup4').on('show.bs.modal', function (event) {
        loadPartinShop();
        $("#P4Shop").val(0);
        $("#P4McName").val(0);
        $("#P4PartNo").val('');
    });
    $("#P4Shop").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P4Grid tbody tr").show();
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
    $('#Popup5').on('show.bs.modal', function (event) {
        loadPartinSubcon();
    });
    $("#P5Subcon").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P5Grid tbody tr").show();
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
            return;
        } else {
            var selvallow = selectedValue.toLowerCase();
            $("#P5Grid tbody tr").filter(function () {
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
    $("#P5PartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P5Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
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



function loadIssueShop() {
    var tablebody = $("#P1Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();

    api.getbulk("/workOrder/GetAllMatl_Issue_List").then((data) => {

        //let unique = [];
        //let seen = new Set();

        //for (let item of data) {
        //    // Build unique key including opNo to allow different operations
        //    let key = `${item.shop}|${item.woNumber}|${item.partNo}|${item.routingName}|${item.opNo}`;

        //    if (!seen.has(key)) {
        //        seen.add(key);
        //        unique.push(item);
        //    }
        //}
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P1GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
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
function loadPartinSubcon() {
    var tablebody = $("#P5Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();

    api.getbulk("/workOrder/GetPartsLoadedInSubCon").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P5GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function loadIssueSubcon() {
    var tablebody = $("#P3Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();

    api.getbulk("/workOrder/GetAllIssueSubCon").then((data) => {
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P3GridRow", data[i]));
        }
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}

function loadIssueShopToStores(filters) {
    var tablebody = $("#P2Grid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();

    api.getbulk("/workOrder/GetAllMatl_Issue_List").then((data) => {
        if (filters) {
            data = data.filter(item => item.from_LocationStr === filters);
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
            $(tablebody).append(AppUtil.ProcessTemplateData("P2GridRow", data[i]));
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