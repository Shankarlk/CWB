
let originalRoutingId = 0;
let currentRoutingId = 0;
function loadSimulationWos() {
    var tablebody = $("#P2Grid tbody");
    $(tablebody).html(""); // empty tbody
    document.getElementById('preloader').style.display = 'block';
    document.getElementById('status').style.display = 'block';
    api.getbulk("/workOrder/GetAllSimOutputWo").then((data) => {
        data.sort((a, b) => {
            const dateA = new Date(a.csStartDate.split("-").reverse().join("-")); // Convert to yyyy-MM-dd
            const dateB = new Date(b.csStartDate.split("-").reverse().join("-"));
            return dateA - dateB;
        });
        let showResimulate = data.some(d => d.dataChange === "Y");
        for (let i = 0; i < data.length; i++) {
            data[i].reworkWo = 'N';
            data[i].buildToStock = 'N';
            let row = AppUtil.ProcessTemplateData("P2GridRow", data[i]);
            let $row = $(row);
            const partType = data[i].partType; // assume numeric or string like "Assy"
            const partTypeName = data[i].partTypeName?.toLowerCase() || "";
            const NoOfRoutes = data[i].noOfRoutes;

            // === Change Routing ===
            if (parseInt(partType) === 2) {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('View Material Availability')").remove();
            }
            if (data[i].reworkWo === "Y") {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('Assign M/c for Rework')").remove();
            }
            if (data[i].for_Ref === "Y") {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('Allocate Common Material')").remove();
            }
            $row.find(".dropdown-item:contains('View Balance Oprs')").remove();
            if (parseInt(NoOfRoutes) > 1) {
                // leave it visible or set attributes if needed
            } else {
                $row.find(".dropdown-item:contains('Change Routing')").remove();
            }
            $(tablebody).append($row);
            $("#P2Grid td").css("white-space", "nowrap");
        }
        if (showResimulate) {
            $("#ReSimulate").show();
        } else {
            $("#ReSimulate").hide();
        }
        document.getElementById('preloader').style.display = 'none';
        document.getElementById('status').style.display = 'none';
    }).catch((error) => {
        console.error("Failed to load WOs", error);
    });
}

$(document).ready(function () {
    $("#P2SearchWo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P2Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#P2SearchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P2Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#P2SearchPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#P2Grid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $('#Popup26').on('hide.bs.modal', function (event) {
        if (originalRoutingId !== currentRoutingId && closeP26 === 0) {
            event.preventDefault();
            $("#openfrom").val("P26");
            $("#ErrorMessage1").modal("show");
        }
    });
    $('#P2SearchBuildStock').on('change', function () {
        const showOnlyCritical = $(this).is(':checked');

        $('#P2Grid tbody tr').each(function () {
            const criticalPart = $(this).find('td:nth-child(4)').text().trim(); // 20th column is "Critical Part"

            if (showOnlyCritical) {
                if (criticalPart === "Y") {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            } else {
                $(this).show();
            }
        });
    });
    $('#P2SearchRwkChk').on('change', function () {
        const showOnlyCritical = $(this).is(':checked');

        $('#P2Grid tbody tr').each(function () {
            const criticalPart = $(this).find('td:nth-child(3)').text().trim(); // 20th column is "Critical Part"

            if (showOnlyCritical) {
                if (criticalPart === "Y") {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            } else {
                $(this).show();
            }
        });
    });
    $('#P2SearchCritical').on('change', function () {
        const showOnlyCritical = $(this).is(':checked');

        $('#P2Grid tbody tr').each(function () {
            const criticalPart = $(this).find('td:nth-child(20)').text().trim(); // 20th column is "Critical Part"

            if (showOnlyCritical) {
                if (criticalPart === "Y") {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            } else {
                $(this).show();
            }
        });
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
        currentRoutingId = parseInt(routingId);
        originalRoutingId = parseInt(routingId);
        // Call API
        api.get("/workOrder/UpdateProdWo?pwoid=" + pwoid + "&routingId=" + routingId).then((data) => {
            loadSimulationWos();
            $("#Popup26").modal("hide");
        }).catch((error) => {
            console.error("Update failed", error);
        });
    });
    $('#Popup3').on('hidden.bs.modal', function (event) {
        document.getElementById('Popup26').style.filter = 'none';
    });
    $('#Popup3').on('show.bs.modal', function (event) {
        document.getElementById('Popup26').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
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
    $('#Popup7').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        closeP26 = 0;
        var partid = relatedTarget.data("partid");
        var workorderid = relatedTarget.data("workorderid");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var ppid = relatedTarget.data("ppid");
        var calwoqty = relatedTarget.data("calwoqty");
        var routingid = relatedTarget.data("routingid");
        currentRoutingId = routingid;
        $("#P7PartNo").text(partno + " / " + partdesc);
        $("#P7Wo").text(calwoqty);
        $("#P7WoQnty").text(calwoqty);
        $("#P7FinQnty").text('0');
        $("#P26Woid").val(ppid);
        loadMatAvl(workorderid);
    });
    $("#ShowChildChkDiv").hide();
    $("#P2SearchPartType").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#P2Grid tbody tr").show();
            $("#ShowChildChkDiv").hide();
            return;
        } else if (selectedValue == "1") {
            $("#ShowChildChkDiv").hide();
            $("#P2Grid tbody tr").show();
            return;
        } else if (selectedValue == "2") {
            var selval = "Parent Assembly";
            $("#ShowChildChkDiv").hide();
            var selvallow = selval.toLowerCase();
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallow) > -1)
            });
        } else if (selectedValue == "3") {
            var selvalc = "Assembly";
            $("#ShowChildChkDiv").show();
            var selvallowc = selvalc.toLowerCase();
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallowc) > -1)
            });
        }else if (selectedValue == "4") {
            var selvalc = "Parent Manf. Part";
            $("#ShowChildChkDiv").hide();
            var selvallowc = selvalc.toLowerCase();
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallowc) > -1)
            });
        }else if (selectedValue == "5") {
            var selvalc = "Child Manf. Part";
            $("#ShowChildChkDiv").hide();
            var selvallowc = selvalc.toLowerCase();
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallowc) > -1)
            });
        } else if (selectedValue == "6") {
            $("#ShowChildChkDiv").hide();
            var selvalc = "Child Manf Part (Comb. WO)";
            var selvallowc = selvalc.toLowerCase();
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallowc) > -1)
            });
        } else if (selectedValue == "7") {
            $("#ShowChildChkDiv").hide();
            var selvalc = "CMP with Common Input Matl";
            var selvallowc = selvalc.toLowerCase();
            $("#P2Grid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selvallowc) > -1)
            });
        }
    });
    loadSimulationWos();
});
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
function loadMatAvl(workorderid) {
    var tablebody = $("#P7Grid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllChildPartAssem?woId=" + parseInt(workorderid)).then((data) => {
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("P7GridRow", data[i]));
        }
    }).catch((error) => {
    });
}