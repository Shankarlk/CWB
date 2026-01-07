let dataMPDList = "";
let partType = "ManufacturedPart";
let status = "Active";

const FILTER_KEY = "ItemMaster_SearchFilters";

function saveItemMasterFilters() {
    const filters = {
        MasterPart: $("#MasterPart").val(),
        Company: $("#master_co").val(),
        Status: $("#Status").val(),
        PartNo: $("#master_partno").val(),
        PartDesc: $("#master_description").val(),
        DocRefStatus: $("#DocRefStatus").val()
    };

    sessionStorage.setItem(FILTER_KEY, JSON.stringify(filters));
}

$(function () {

    $("#MasterPart, #master_co, #Status, #master_partno, #master_description, #DocRefStatus")
        .on("change keyup", saveItemMasterFilters);
    const savedFilters = sessionStorage.getItem("ItemMaster_SearchFilters");

    if (savedFilters) {
        const filters = JSON.parse(savedFilters);

        $("#MasterPart").val(filters.MasterPart);
        $("#master_co").val(filters.Company);
        $("#Status").val(filters.Status);
        $("#master_partno").val(filters.PartNo);
        $("#master_description").val(filters.PartDesc);
        $("#DocRefStatus").val(filters.DocRefStatus);
    }
    $("#MastersDetailClose").on("click", function () {
        sessionStorage.removeItem("ItemMaster_SearchFilters");
    });

    $("#master_co").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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

    $("#master_partno").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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

    $("#master_description").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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
    $("#manfWithOutRM").click(function (event) {
        var data = "No";
        var value = data.toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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
    $("#AssWithOuutBOM").click(function (event) {
        var data = "No";
        var value = data.toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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
    $("#PurcSupplier").click(function (event) {
        var data = "No";
        var value = data.toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[6]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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
    $("#MandatoryNotUp").click(function (event) {
        var data = "No";
        var value = data.toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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
    $("#finalepartSold").click(function (event) {
        var data = "Y";
        var value = data.toLowerCase();
        $("#mptable tbody tr").filter(function () {
            $(this).toggle($(this.children[9]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#mptable tbody");
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

    $("#Status").change(function () {
        loadMPDList();
    });
    $("#DocRefStatus").change(function () {
        var selval = $("#DocRefStatus").val();
        if (selval != "0") {
            var data = $("#DocRefStatus option:selected").text();
            var value = data.toLowerCase();
            $("#mptable tbody tr").filter(function () {
                $(this).toggle($(this.children[10]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#mptable tbody");
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
            $("#mptable tbody tr").show();
        }
    });

    var RawMaterialMadeType = 0;
    //////debugger;
    // Document is ready
    /** $("#RawMaterialTypeId").select2();
     loadRMTypes("RawMaterialTypeId");
 
     $("#BaseRawMaterialId").select2();
     loadBaseRMs("BaseRawMaterialId");
 
     $("#Standard").select2();
     loadRMStandards("Standard");
 
     $("#MaterialSpecId").select2();
     loadRMSpecs("MaterialSpecId");
 
     $("#SupplierId").select2();
     loadSuppliers("SupplierId");*/
    loadEditParts();

    $('#VMStatus').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue != "0") {
            var data = selectedValue;
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
        } else if (selectedValue == "0") {
            var $tableBody = $("#VMOneGrid tbody");
            $tableBody.find(".norecordsfound").remove();
            $("#VMOneGrid tbody tr").show();
            
        } else {
            $tableBody.find(".norecordsfound").remove();
            $("#VMOneGrid tbody tr").show();
        }
    });
    $('#P5Status').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue != "0") {
            var data = selectedValue;
            var value = data.toLowerCase();
            $("#P5Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P5Grid tbody");
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
        } else if (selectedValue == "0") {
            var $tableBody = $("#P5Grid tbody");
            $tableBody.find(".norecordsfound").remove();
            $("#P5Grid tbody tr").show();
        } else {
            $tableBody.find(".norecordsfound").remove();
            $("#P5Grid tbody tr").show();
        }
    });
    $("#P5Company").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P5Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P5Grid tbody");
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
    $("#P5Source").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P5Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P5Grid tbody");
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
    $("#P5MatlSpec").on("change", function () {
        filterP5Grid(6, $(this).val()); // column index 6
    });

    $("#P5BaseRm").on("change", function () {
        filterP5Grid(5, $(this).val()); // column index 5
    });

    $("#P5RmType").on("change", function () {
        filterP5Grid(4, $(this).val()); // column index 4
    });

    $("#P5PartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P5Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P5Grid tbody");
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
    $("#P3RmType").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P3Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
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
    });
    $("#P3BaseRm").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P3Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
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
    });
    $("#P9PartSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P9Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P9Grid tbody");
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
    $("#P9CompSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P9Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#P9Grid tbody");
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
    $('#P3StatusSr').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue != "0") {
            var data = selectedValue;
            var value = data.toLowerCase();
            $("#P3Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
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
        } else if (selectedValue == "0") {
            var $tableBody = $("#P3Grid tbody");
            $tableBody.find(".norecordsfound").remove();
            $("#P3Grid tbody tr").show();
        } else {
            $tableBody.find(".norecordsfound").remove();
            $("#P3Grid tbody tr").show();
        }
    });
    $('#P7Status').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue != "0") {
            var data = selectedValue;
            var value = data.toLowerCase();
            $("#P7Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P7Grid tbody");
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
        } else if (selectedValue == "0") {
            var $tableBody = $("#P5Grid tbody");
            $tableBody.find(".norecordsfound").remove();
            $("#P7Grid tbody tr").show();
        } else {
            $tableBody.find(".norecordsfound").remove();
            $("#P7Grid tbody tr").show();
        }
    });
    $('#P9StatusSearch').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue != "0") {
            var data = selectedValue;
            var value = data.toLowerCase();
            $("#P9Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P9Grid tbody");
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
        } else if (selectedValue == "0") {
            var $tableBody = $("#P9Grid tbody");
            $tableBody.find(".norecordsfound").remove();
            $("#P9Grid tbody tr").show();
        } else {
            $("#P9Grid tbody tr").show();
        }
    });
    $('#P7BofType').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue == "1") {
            var data = "Standard";
            var value = data.toLowerCase();
            $("#P7Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P7Grid tbody");
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
        } else if (selectedValue == "2") {
            var data = "Catalog";
            var value = data.toLowerCase();
            $("#P7Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P7Grid tbody");
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
        } else if (selectedValue == "3") {
            var data = "Made to Print";
            var value = data.toLowerCase();
            $("#P7Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P7Grid tbody");
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
            $("#P7Grid tbody tr").show();
        }
    });
    $('#P9BofType').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue == "1") {
            var data = "Standard";
            var value = data.toLowerCase();
            $("#P9Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P9Grid tbody");
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
        } else if (selectedValue == "2") {
            var data = "Catalog";
            var value = data.toLowerCase();
            $("#P9Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P9Grid tbody");
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
        } else if (selectedValue == "3") {
            var data = "Made to Print";
            var value = data.toLowerCase();
            $("#P9Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P9Grid tbody");
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
            $("#P9Grid tbody tr").show();
        }
    });
    $("#P9Supp1").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "1";
            var value = data.toLowerCase();
            $("#P9Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P9Grid tbody");
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
            $("#P9Grid tbody tr").show();
        }

    });
    $("#P9Supp2").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "2";
            var value = data.toLowerCase();
            $("#P9Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P9Grid tbody");
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
            $("#P9Grid tbody tr").show();
        }
    });
    $("#btnP5CancelFields").click(function (event) {
        $("#P5PartNo").val('');
        $("#P5Company").val('');
        $("#P5Source").val('');
        $("#P5RmType").val(0);
        $("#P5BaseRm").val(0);
        $("#P5MatlSpec").val(0);
        $("#P5Status").val(0);
        $("#P5Supplier2").prop('checked', false);
        $("#P5SuppWith1").prop('checked', false);
        $("#P5Grid tbody tr").show();
    });
    $("#P9BtnClearFields").click(function (event) {
        $("#P9PartSearch").val('');
        $("#P9CompSearch").val('');
        $("#P9SourceSearch").val('');
        $("#P9StatusSearch").val(0);
        $("#P9BofType").val(0);
        $("#P9Supp2").prop('checked', false);
        $("#P9Supp1").prop('checked', false);
        $("#P9Grid tbody tr").show();
    });
    $("#P5Supplier2").click(function (event) {
        var isChecked = $(this).prop("checked");
        var data = "2";
        var value = data.toLowerCase();
        if (isChecked) {
            $("#P5Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
            });
        } else {
            $("#P5Grid tbody tr").show(); // Show all rows if the checkbox is not checked
        }
    });
    $("#P5SuppWith1").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "1";
            var value = data.toLowerCase();
            $("#P5Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#P5Grid tbody");
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
            $("#P5Grid tbody tr").show();
        }

    });
    $('#VMSearchPartTypr').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue == "1") {
            var data = "ManufacturedPart";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
        } else if (selectedValue == "2") {
            var data = "Assembly";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
            $("#VMOneGrid tbody tr").show();    
        }
    });
    $("#BtnClearFlieds").click(function (event) {
        $("#VMOneGrid tbody tr").show();
    });
    $("#VMManfWithoutRouting").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "No";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
            $("#VMOneGrid tbody tr").show();
        }
    });
    $("#VMManfWithoutDoc").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "No";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
            $("#VMOneGrid tbody tr").show();
        }
    });
    $("#VMRoutingWithoutDoc").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "No";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[9]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
            $("#VMOneGrid tbody tr").show();
        }
    });
    $("#VMAssWithoutBOm").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "No";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[6]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
            $("#VMOneGrid tbody tr").show();
        }

    });
    $("#VMFinalPart").change(function (event) {
        var selectedValue = $(this).val();
        //var isChecked = $(this).prop("checked");
        if (selectedValue == "1") {
            var data = "Yes";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
        } else if (selectedValue == "1") {
            var data = "No";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
        else {
            var $tableBody = $("#VMOneGrid tbody");
            $tableBody.find(".norecordsfound").remove();
            $("#VMOneGrid tbody tr").show();
        }
    });
    $("#VMManfWithoutRM").click(function (event) {
        var isChecked = $(this).prop("checked");
        if (isChecked) {
            var data = "No";
            var value = data.toLowerCase();
            $("#VMOneGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#VMOneGrid tbody");
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
            $("#VMOneGrid tbody tr").show();
        }
    });
    $('select[name="MasterPart"]').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue == "1") {
            var data = "ManufacturedPart";
            var value = data.toLowerCase();
            $("#mptable tbody tr").filter(function () {
                $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#mptable tbody");
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
        } else if (selectedValue == "2") {
            partType = "Assembly";
            var data = "Assembly";
            var value = data.toLowerCase();
            $("#mptable tbody tr").filter(function () {
                $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#mptable tbody");
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
        } else if (selectedValue == "3") {
            partType = "BOF";
            var data = "BOF";
            var value = data.toLowerCase();
            $("#mptable tbody tr").filter(function () {
                $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#mptable tbody");
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
        } else if (selectedValue == "4") {
            partType = "RawMaterial";
            var data = "RM";
            var value = data.toLowerCase();
            $("#mptable tbody tr").filter(function () {
                $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#mptable tbody");
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
            $("#mptable tbody tr").show();
        }
    });
    $("#ClearSearchFields").on("click", function () {
        $("#mptable tbody tr").show();
        $("#master_co").val('');
        $("#cars").prop('checked', false);
        document.getElementById("manfWithOutRM").checked = false;
        document.getElementById("AssWithOuutBOM").checked = false;
        document.getElementById("PurcSupplier").checked = false;
        document.getElementById("MandatoryNotUp").checked = false;
        document.getElementById("finalepartSold").checked = false;

        var SearchFileExtn = $('#MasterPart');
        SearchFileExtn.val(0).trigger('change');
        $("#Status").val("Released").trigger('change');
    });

    $("#AddToMasterDocList").click(function (event) {
        var mkID = parseInt($("#SetPreferedMKId").val());
        var DocTypeName = parseInt($("#DocTypeName").val());
        var MasterContent = parseInt($("#MasterContent").val());
        var ItemDocId = parseInt($("#ItemDocId").val());
        var mandatory = 'N';
        if ($("#DocUploadMandatory").prop("checked") == true) {
            mandatory = 'Y';
        }
        if (MasterContent == 0) {
            var newNamevalidate = document.getElementById('MasterContent');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('MasterContent');
            newNamevalidate.style.border = '';
        }
        if (DocTypeName == 0) {
            var newNamevalidate = document.getElementById('DocTypeName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('DocTypeName');
            newNamevalidate.style.border = '';
        }
        var today = new Date();
        today.setDate(today.getDate());
        var deletionDate = today.toISOString().split('T')[0];
        var rowData = {
            ItemMasterDocListId: ItemDocId,
            ContentId: MasterContent,
            DocumentTypeId: DocTypeName,
            Mandatory: mandatory,
            UpdatedOn: deletionDate
        };
        api.getbulk("/masters/CheckDocumentTypeInItemMaster?documentTypeId=" + DocTypeName + "&contentId=" + MasterContent).then((data) => {
            if (data) {
                api.post("/masters/PostItemMasterDocList", rowData).then((data) => {
                    $("#Text-Error").text("");
                    var DocTypeName = $("#DocTypeName");
                    var MasterContent = $("#MasterContent");
                    $("#ItemDocId").val(0);
                    DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
                    MasterContent.find("option[value='" + 0 + "']").prop('selected', true);
                    $("#DocUploadMandatory").prop("checked", false);

                    loadItemMasterDocList();
                }).catch((error) => {
                    AppUtil.HandleError("FormEditMakeFrom", error);
                });

            } else {
                $("#Text-Error").text("This Entry Is Already in The List").css('color', 'red');
            }
        }).catch((error) => {
        });
    });
    $('#adtimc').on('hidden.bs.modal', function (event) {
        var DocTypeName = $("#DocTypeName");
        var MasterContent = $("#MasterContent");
        $("#ItemDocId").val(0);
        DocTypeName.find("option[value='" + 0 + "']").prop('selected', true);
        MasterContent.find("option[value='" + 0 + "']").prop('selected', true);
        $("#DocUploadMandatory").prop("checked", false);
        $("#Text-Error").text("");
    });
    $('#adtimc').on('show.bs.modal', function (event) {
        loadItemMasterDocList();
        loadDocTypes();
        loadMasterContent();
    });
    $('#popup6').on('show.bs.modal', function (event) {
        BofSupplier();
    });
    $('#popup5').on('show.bs.modal', function (event) {
        RMList();
    });
    $('#popup1').on('show.bs.modal', function (event) {
        loadManufAssem();
    });
    let vmpartid = 0;
    $('#viewDoc').on('show.bs.modal', function (event) {
        $('#fileViewer').attr('src', '');
        var newNamevalidate = document.getElementById('SelectDocType');
        newNamevalidate.style.border = '';
        var relatedTarget = $(event.relatedTarget);
        var customername = relatedTarget.data("customername");
        var partid = relatedTarget.data("partid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var parttype = relatedTarget.data("parttype");
        vmpartid = partid;
        $("#VPPartNo").text(partno);
        $("#VPPartDesc").text(partdesc);
        $("#VPCustomer").text(customername);
        loadViewDocumentType(parttype);
    });
    $("#BtnViewDoc").on("click", function () {
        var documenttype = $("#SelectDocType").val();
        if (parseInt(documenttype) == 0) {
            var newNamevalidate = document.getElementById('SelectDocType');
            newNamevalidate.style.border = '2px solid red';
            return false;
        }
        api.getbulk("/DocumentManagement/GetAllDocList").then((data) => {
            data = data.filter(item => item.partId === vmpartid && item.documentTypeId === parseInt(documenttype));
            if (data.length > 0) {
                $("#VPDocType").text(data[0].documentTypeName);
                $("#VPRetention").text(data[0].retentionDateStr);
                $("#VPUploadedOn").text(data[0].updatedOnStr);
                $("#VPUploadedBy").text(data[0].uploadedBy);
                ViewFile(data[0].fileName);
            }
        }).catch((error) => {
            console.log(error);
        });
    });
    $('#ViewManufPartPopup').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var customername = relatedTarget.data("customername");
        $("#VmPopComp").text(customername);
        loadManufAssemComp(customername);
    });
    $('#popup8').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var partid = relatedTarget.data("partid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var customername = relatedTarget.data("customername");
        $("#P8BofNo").text(partno);
        $("#P8BofDesc").text(partdesc);
        $("#P8Comp").text(customername);
        LoadPartByBof(partid);
    });
    $('#popup9').on('show.bs.modal', function (event) {
        BofList();
    });
    $('#popup2').on('show.bs.modal', function (event) {
        RmSupplier();
    });
    $('#popup3').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var customername = relatedTarget.data("customername");
        $("#P3CustSupp").text(" ");
        $("#P3Comp").text(customername);
        P2RMList(customername);
    });
    $('#popup7').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var customername = relatedTarget.data("customername");
        $("#P7Cust").text(" ");
        $("#P7Comp").text(customername);
        P7BofList(customername);
    });
    $('#popup4').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var partid = relatedTarget.data("partid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var customername = relatedTarget.data("customername");
        $("#P4RmPartNo").text(partno);
        $("#P4RmDesc").text(partdesc);
        $("#P4Comp").text(customername);
        LoadPartByRM(partid);
    });
    $('#status-info').on('show.bs.modal', function (event) {
        var newNamevalidate = document.getElementById('statusResasonopup');
        newNamevalidate.style.border = '';
        var statusPopup = document.getElementById('statusPopup');
        statusPopup.style.border = '';
        var relatedTarget = $(event.relatedTarget);
        var partid = relatedTarget.data("partid");
        var parttype = relatedTarget.data("parttype");
        var status = relatedTarget.data("status");
        var partno = relatedTarget.data("partno");
        $("#CurrentStatus").val(status);
        $("#PartIdSt").val(partid);
        $("#PartTypeSt").val(parttype);
    });
    $("#BtnstatusSave").click(function (event) {
        var streason = $("#statusResasonopup").val();
        var statusPopup = $("#statusPopup").val();
        var status = $("#CurrentStatus").val();
        var partid=  $("#PartIdSt").val();
        var parttype = $("#PartTypeSt").val();
        if (statusPopup == status) {
            var newNamevalidate = document.getElementById('statusPopup');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('statusPopup');
            newNamevalidate.style.border = '';
            //$("#status-info").modal("hide");
        }
        if (statusPopup == "Inactive") {
            if (streason.length == 0) {
                var newNamevalidate = document.getElementById('statusResasonopup');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('statusResasonopup');
                newNamevalidate.style.border = '';
                //$("#status-info").modal("hide");
            }
            if (parttype === "ManufacturedPart") {

                api.get("/Masters/CMPBomParents?partId=" + partid + "&partType=" + parttype)
                    .then((data) => {
                        if (data.length > 0) {
                            var partNos = data.map(x => x.partNo).join(", ");
                            var msg = "Part is linked to following BOM : " + partNos + "\nClick OK to proceed or Cancel to stop.";
                            let confirmval = confirm(msg, "Yes", "No");
                            if (confirmval) {
                                api.get("/Masters/ChangeStatusOfPart?partId=" + partid + "&partType=" + parttype + "&status=" + statusPopup + "&statusReason=" + streason)
                                    .then((response) => {
                                        $("#status-info").modal("hide");
                                        $("#statusResasonopup").val('');
                                        location.reload();
                                    })
                                    .catch((error) => {

                                    });
                            }
                        } else {
                            api.get("/Masters/ChangeStatusOfPart?partId=" + partid + "&partType=" + parttype + "&status=" + statusPopup + "&statusReason=" + streason)
                                .then((response) => {
                                    $("#status-info").modal("hide");
                                    $("#statusResasonopup").val('');
                                    location.reload();
                                })
                                .catch((error) => {

                                });
                        }
                    })
                    .catch((error) => {

                    });
            } else {
                api.get("/Masters/ChangeStatusOfPart?partId=" + partid + "&partType=" + parttype + "&status=" + statusPopup + "&statusReason=" + streason)
                    .then((response) => {
                        $("#status-info").modal("hide");
                        $("#statusResasonopup").val('');
                        location.reload();
                    })
                    .catch((error) => {

                    });
            }
        } else {
            api.get("/Masters/ChangeStatusOfPart?partId=" + partid + "&partType=" + parttype + "&status=" + statusPopup + "&statusReason=" + streason)
                .then((response) => {
                    $("#status-info").modal("hide");
                    $("#statusResasonopup").val('');
                    location.reload();
                })
                .catch((error) => {

                });
        }

    });
    loadrmtypes();
    loadbaserms();
    loadRMSpecs();
});
function filterP5Grid(columnIndex, value) {
    var $tableBody = $("#P5Grid tbody");

    // Remove old "No Records"
    $tableBody.find(".norecordsfound").remove();

    // If "0" → show all rows
    if (value === "0") {
        $tableBody.find("tr").show();
        return;
    }

    value = value.toLowerCase();

    $("#P5Grid tbody tr").each(function () {
        var cellText = $(this).children(columnIndex).text().toLowerCase();
        $(this).toggle(cellText.indexOf(value) > -1);
    });

    // Show "No Records Found" if nothing visible
    if ($tableBody.find("tr:visible").length === 0) {
        $tableBody.append(`
            <tr class="norecordsfound">
                <td colspan="20" style="text-align:center;color:#888;">
                    <strong>No Records Found</strong>
                </td>
            </tr>
        `);
    }
}

function loadrmtypes() {
    var selElem = $('#P5RmType');
    selElem.html('');
    api.getbulk("/masters/rmtypes").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "All RM Type" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].name + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadbaserms() {
    var selElem = $('#P5BaseRm');
    selElem.html('');
    api.getbulk("/masters/baserms").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "All Base RM" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].name + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadRMSpecs() {
    var selElem = $('#P5MatlSpec');
    selElem.html('');
    api.getbulk("/masters/RMSpecs").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "All MatlSpec" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].name + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });
}
function BofSupplier() {
    var tablebody = $("#P6Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/masters/BofByComp").then((data) => {
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
            if (data[i].company === "") {
                continue;
            }
            var tBody = ProcessTemplateDataNew("P6GridRow", data[i]);
            $(tablebody).append(tBody);
        }
    }).catch((error) => {
    });
}
function RmSupplier() {
    var tablebody = $("#P2Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/masters/RmByComp").then((data) => {
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
            var tBody = ProcessTemplateDataNew("P2GridRow", data[i]);
            $(tablebody).append(tBody);
        }
    }).catch((error) => {
    });
}
function LoadPartByBof(partid) {
    var tablebody = $("#P8Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/masters/GetAllAssemByBof?partid=" + partid).then((data) => {
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
            var tBody = ProcessTemplateDataNew("P8GridRow", data[i]);
            $(tablebody).append(tBody);
        }
    }).catch((error) => {
    });
}
function LoadPartByRM(partid) {
    var tablebody = $("#Popup4Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/masters/GetAllManufByRM?partid=" + partid).then((data) => {
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
            var tBody = ProcessTemplateDataNew("P4GridRow", data[i]);
            $(tablebody).append(tBody);
        }
    }).catch((error) => {
    });
}
function ViewFile(filename) {

    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/masters/ViewFile?fileName=' + filename, true);
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
function loadItemMasterDocList() {
    var tablebody = $("#MasterDocList tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/masters/GetAllItemMasterDocLists").then((data) => {
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
            var tBody = ProcessTemplateDataNew("MasterDocRow", data[i]);
            $(tablebody).append(tBody);
            //console.log(tBody);
        }
    }).catch((error) => {
    });
}
function loadMasterContent() {
    var selElem = $('#MasterContent');
    selElem.html('');
    api.getbulk("/masters/ItemMasterContent").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].itemMasterContentId + "'>" + data[i].contentDesc + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadDocTypes() {
    var selElem = $('#DocTypeName');
    selElem.html('');
    api.getbulk("/masters/DocTypes").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].documentTypeId + "'>" + data[i].documentName + "</option>";
            selElem.append(div_data);
        }
    });
}
function DeleteItemMasterDoc(element) {
    var relatedTarget = $(element);
    var masterdocid = relatedTarget.data("masterdocid");
    var doctypeId = relatedTarget.data("docname");
    api.get("/masters/CheckDocTypeInDocList?docTypeid=" + doctypeId).then((data) => {
        // loadDocType(); 
        if (data) {
            let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
            if (confirmval) {
                api.get("/masters/DeleteItemMasterDocList?itemMasterDocListId=" + masterdocid).then((data) => {
                    loadItemMasterDocList();
                }).catch((error) => {

                });
            }
        } else {
            alert("Deletion of This can be done after files with the Extn are deleted from the System");
        }
    }).catch((error) => {

    });
}
function EditItemMasterDoc(element) {
    var relatedTarget = $(element);
    var docname = relatedTarget.data("docname");
    var mandatory = relatedTarget.data("mandatory");
    var mastercontent = relatedTarget.data("mastercontent");
    var masterdocid = relatedTarget.data("masterdocid");
    var DocTypeName=$("#DocTypeName");
    var MasterContent = $("#MasterContent");
    $("#ItemDocId").val(masterdocid);
    DocTypeName.find("option[value='" + docname + "']").prop('selected', true);
    MasterContent.find("option[value='" + mastercontent + "']").prop('selected', true);
    if (mandatory == 'Y') {
        $("#DocUploadMandatory").prop("checked", true);
    }

}

function ProcessTemplateDataNew(templateId, dataObj) {
    //debugger;
    var templateElement = $("#" + templateId).html();
    ////console.log(templateId);
    templateElement = templateElement.replaceAll("{partType}", partType)
    for (var key in dataObj) {
        ////console.log(key + " " + dataObj[key]);
        templateElement = templateElement.replaceAll("{" + key + "}", dataObj[key])
    }
    // console.log(templateElement);
    return templateElement;
}

function loadMPDList() {
    var tablebody = $("#mptable tbody");
    $(tablebody).html("");//empty tbody
    //UpdatePurchaseDetailsTableFromPostData
    let i = 0;
    var strActive = $("#Status").val();
    if (dataMPDList.length > 2) {
        //console.log(partType);
        //console.log("================");
        let data = dataMPDList;
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
            //if (!(data[i]['masterPartType'] == partType))
            //    continue;
            if (!(data[i]['status'] == strActive))
                continue;
            data[i].activeName = data[i]['status'] == "Active" ? "Make Inactive" : "Make Active";
            var tBody = ProcessTemplateDataNew("MasterDetaiTemplate", data[i]);
            $(tablebody).append(tBody);
            //console.log(tBody);
        }
    }
    else {
        api.getbulk("/masters/masterparts").then((data) => {
            dataMPDList = data;
            if (typeof fromobselete !== 'undefined' && fromobselete === 'ObsoletePart') {
                data = data.filter(item => item.status != "Active");
                dataMPDList = data;
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
                /*for (var key in data[i]) {
                    console.log(key);
                    console.log(data[i][key]);
                    console.log("*****");
                }*/
                //console.log(partType);
                //console.log("================");
                //if (!(data[i]['masterPartType'] == partType))
                //    continue;
                if (!(data[i]['status'] == strActive))
                    continue;
                data[i].activeName = data[i]['status'] == "Active" ? "Make Inactive" : "Make Active";
                var tBody = ProcessTemplateDataNew("MasterDetaiTemplate", data[i]);
                $(tablebody).append(tBody);
                //console.log(tBody);
            }
        }).catch((error) => {
        });
    }
}
function getItemMasterFilters() {
    return {
        MasterPart: $("#MasterPart option:selected").text(),          // 0,1,2,3,4
        Company: $("#master_co").val()?.trim(),
        Status: $("#Status").val(),
        PartNo: $("#master_partno").val()?.trim(),
        PartDesc: $("#master_description").val()?.trim(),
        DocRefStatus: $("#DocRefStatus").val()
    };
}
function applyItemMasterFilters(data, filters) {
    return data.filter(item => {

        // Part Type
        if (filters.MasterPart && filters.MasterPart !== "0" && $("#MasterPart").val() != "0") {
            var mp = filters.MasterPart.replace(/\s+/g, '');
            if (item.masterPartType != mp)
                return false;
        }

        // Company
        if (filters.Company) {
            if (!item.company?.toLowerCase().includes(filters.Company.toLowerCase()))
                return false;
        }

        // Part Status
        if (filters.Status) {
            if (item.status !== filters.Status)
                return false;
        }

        // Part No
        if (filters.PartNo) {
            if (!item.partNo?.toLowerCase().includes(filters.PartNo.toLowerCase()))
                return false;
        }

        // Part Description
        if (filters.PartDesc) {
            if (!item.description?.toLowerCase().includes(filters.PartDesc.toLowerCase()))
                return false;
        }

        // Document Status
        if (filters.DocRefStatus && filters.DocRefStatus !== "0") {
            if (item.docRefStatus != filters.DocRefStatus)
                return false;
        }

        return true;
    });
}

function loadEditParts() {
    var tablebody = $("#mptable tbody");
    $(tablebody).html("");//empty tbody
    $('#preloaderblurred').show();
    api.getbulk("/masters/masterparts").then((data) => {
        dataMPDList = data;
        if (typeof fromobselete !== 'undefined' && fromobselete === 'ObsoletePart') {
            data = data.filter(item => item.status === "Obsolete");
            dataMPDList = data;
            partType = " ";
            $("#Status").val("Obsolete");
            $("#Status").prop("disabled", true);
        }
        const filters = getItemMasterFilters();
        const savedFilters = sessionStorage.getItem("ItemMaster_SearchFilters");
        if (savedFilters) {
            data = applyItemMasterFilters(data, filters);
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
           
            if (!(data[i]['status'] == "Released"))
                continue;
            data[i].activeName = data[i]['status'] == "Active" ? "Make Inactive" : "Make Active";
            let canDelete = (data[i].inv_Trans === "N" && data[i].linked_to_BOM ==="N");
            let canalsoDelete = (data[i].inv_Trans === "N" && data[i].linked_to_BOM === "Y");
            data[i].showMenu = (canDelete || canalsoDelete) ? "block" : "none";
            var tBody = ProcessTemplateDataNew("MasterDetaiTemplate", data[i]);
            $(tablebody).append(tBody);
            //console.log(tBody);
        }
        $("#Status").val("Released");
        $('#preloaderblurred').hide();
    }).catch((error) => {
        $('#preloaderblurred').hide();
    });
}
function DeletePart(element) {
    var relatedTarget = $(element);
    var doclistid = relatedTarget.data("partid");
    var listassem = relatedTarget.data("listassem");
    var partno = relatedTarget.data("partno");
    if (doclistid != 0 && listassem != "-") {
        var msg = "Do you want to delete Part";
        if (listassem == null) {
            msg = "Do you want to delete Part";
        } else {
            msg = "This Part No is linked to following BOM: " + listassem + "  If deleted, the BOM will also get updated without the deleted Part Do you want to Delete.";
        }
        var confrimval = confirm(msg);
        if (confrimval) {
            api.get("/masters/DeleteItemMasterPart?itemMasterDocListId=" + doclistid+"&partno=" + partno).then((data) => {
                //console.log(data);
                if (data == false || data == true) {

                } else {
                    alert(data);
                }
                loadEditParts();
            }).catch((error) => {
                //console.log(error);
            });
        }
    } else if (doclistid != 0 && listassem === "-") {
        var confrimval = confirm("Do you want to Delete This Part.");
        if (confrimval) {
            api.get("/masters/DeleteItemMasterPart?itemMasterDocListId=" + doclistid + "&partno=" + partno).then((data) => {
                //console.log(data);
                loadEditParts();
                if (data == false || data == true) {

                } else {
                    alert(data);
                }
            }).catch((error) => {
                //console.log(error);
            });
        }
    } else { }
}

function loadManufAssem() {
    var tablebody = $("#Popup1Grid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/masters/ManufAssemCompLinq").then((data) => {
        let totalFinalPartsSold = 0;
        let totalManfActive = 0;
        let totalAssemActive = 0;
        let totalManfInactive = 0;
        let totalAssemInactive = 0;
        let totalManfWithoutRM = 0;
        let totalAssemWithoutBOM = 0;
        let totalMandatoryDocsNotUploaded = 0;
        let totalRoutingNotAvl = 0;
        let totalnoOfManufHold= 0;
        let totalnoOfAssemblyHold= 0;

        for (let i = 0; i < data.length; i++) {
            const item = data[i];

            // Accumulate totals (assuming these fields exist and are numbers)
            totalFinalPartsSold += parseInt(item.finalPart) || 0;
            totalManfActive += parseInt(item.noOfManufActive) || 0;
            totalAssemActive += parseInt(item.noOfAssemblyActive) || 0;
            totalManfInactive += parseInt(item.noOfManufInActive) || 0;
            totalAssemInactive += parseInt(item.noOfAssemblyInActive) || 0;
            totalManfWithoutRM += parseInt(item.rmAvl) || 0;
            totalAssemWithoutBOM += parseInt(item.bomAvl) || 0;
            totalMandatoryDocsNotUploaded += parseInt(item.mandocAvl) || 0;
            totalRoutingNotAvl += parseInt(item.routingNotAvl) || 0;
            totalnoOfManufHold += parseInt(item.noOfManufHold) || 0;
            totalnoOfAssemblyHold += parseInt(item.noOfAssemblyHold) || 0;


            // Append the row data for each company
            $(tablebody).append(AppUtil.ProcessTemplateData("Popup1GridRow", item));
        }

        // Create the totals row with accumulated values
        const totalsRow = `
        <tr>
            <td>Total</td>
            <td>${totalFinalPartsSold}</td>
            <td>${totalManfActive}</td>
            <td>${totalAssemActive}</td>
            <td>${totalManfInactive}</td>
            <td>${totalAssemInactive}</td>
            <td>${totalnoOfManufHold}</td>
            <td>${totalnoOfAssemblyHold}</td>
            <td>${totalManfWithoutRM}</td>
            <td>${totalAssemWithoutBOM}</td>
            <td>${totalMandatoryDocsNotUploaded}</td>
            <td>${totalRoutingNotAvl}</td>
            <td></td>
        </tr>
    `;

        // Append the totals row to the table body
        $(tablebody).append(totalsRow);
    }).catch((error) => {
    });
}
function loadManufAssemComp(company) {
    var tablebody = $("#VMOneGrid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/masters/ManufAssemlist?company=" + company).then((data) => {
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
            if (data[i].partNo == "") {
                continue;
            }
            $(tablebody).append(AppUtil.ProcessTemplateData("VMGridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function BofList() {
    var tablebody = $("#P9Grid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/masters/AllBofList").then((data) => {
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
            data[i].reorderLevel = data[i].reorderLevel === null ? " " : data[i].reorderLevel;
            data[i].reorderQnty = data[i].reorderQnty === null ? " " : data[i].reorderQnty;
            $(tablebody).append(AppUtil.ProcessTemplateData("P9GridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function P7BofList(company) {
    var tablebody = $("#P7Grid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/masters/BofSumByComp?company=" + company).then((data) => {
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
        data = data.filter(item => item.company === company);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P7GridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function P2RMList(company) {
    var tablebody = $("#P3Grid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/masters/RmSupplierList?company=" + company).then((data) => {
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
        data = data.filter(item => item.company === company);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P3GridRow", data[i]));
        }
    }).catch((error) => {
    });
}
function RMList() {
    var tablebody = $("#P5Grid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/masters/RMList").then((data) => {
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
    }).catch((error) => {
    });
}
function loadViewDocumentType(partType) {
    api.getbulk("/masters/GetAllItemMasterDocLists").then((data) => {
        if (partType == "ManufacturedPart") {
            data = data.filter(item => item.contentId === 1);
            var selElem = $('#SelectDocType');
            selElem.html('');
            var rdiv_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            selElem.append(rdiv_data);
            for (i = 0; i < data.length; i++) {
                rdiv_data = "<option value='" + data[i].documentTypeId + "'>" + data[i].documentTypeName + "</option>";
                selElem.append(rdiv_data);
            }
        }
        if (partType == "RawMaterial") {
            data = data.filter(item => item.contentId === 4);
            var selElem = $('#SelectDocType');
            selElem.html('');
            var rdiv_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            selElem.append(rdiv_data);
            for (i = 0; i < data.length; i++) {
                rdiv_data = "<option value='" + data[i].documentTypeId + "'>" + data[i].documentTypeName + "</option>";
                selElem.append(rdiv_data);
            }
        }
        if (partType == "Standard BOF") {
            data = data.filter(item => item.contentId === 6);
            var selElem = $('#SelectDocType');
            selElem.html('');
            var rdiv_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            selElem.append(rdiv_data);
            for (i = 0; i < data.length; i++) {
                rdiv_data = "<option value='" + data[i].documentTypeId + "'>" + data[i].documentTypeName + "</option>";
                selElem.append(rdiv_data);
            }
        }
        if (partType == "BOF") {
            data = data.filter(item => item.contentId === 7);
            var selElem = $('#SelectDocType');
            selElem.html('');
            var rdiv_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            selElem.append(rdiv_data);
            for (i = 0; i < data.length; i++) {
                rdiv_data = "<option value='" + data[i].documentTypeId + "'>" + data[i].documentTypeName + "</option>";
                selElem.append(rdiv_data);
            }
        }
        if (partType == "Own Purchased RM") {
            data = data.filter(item => item.contentId === 4 || item.contentId ==5);
            var selElem = $('#SelectDocType');
            selElem.html('');
            var rdiv_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            selElem.append(rdiv_data);
            for (i = 0; i < data.length; i++) {
                rdiv_data = "<option value='" + data[i].documentTypeId + "'>" + data[i].documentTypeName + "</option>";
                selElem.append(rdiv_data);
            }
        }
    }).catch((error) => {
    });
}