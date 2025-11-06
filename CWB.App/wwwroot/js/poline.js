var suppilerselecet = {};
var subcontotal = 0;
function loadPO() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllPodetails").then((data) => {
        //data = data.filter(item => item.active !== 2);
        var tablebody = $("#PoGrid1 tbody");
        var tablebody2 = $("#PoGrid2 tbody");
        $(tablebody).html("");//empty tbody
        $(tablebody2).html("");//empty tbody
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
            $(tablebody2).append(noRecordsRow);
        }


        //console.log(data);
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("POGrid1Row", data[i]));
            data[i].viewDeliveryClass = data[i].poQntyRecd > 0 ? "" : "disabled-link";
            data[i].deleteClass = data[i].poQntyRecd === 0 ? "" : "disabled-link";
            data[i].shortCloseClass = data[i].poQntyRecd > 0 ? "" : "disabled-link";
            $(tablebody2).append(AppUtil.ProcessTemplateData("POGrid2Row", data[i]));
        }
        const customerChildParts = data.filter((workOrder) => workOrder.partType === "RawMaterial" && workOrder.status == 1);
        const count = customerChildParts.length;
        $('#noOfPoApprv').text(count);
        const workOrdersWithStatus1 = data.filter((workOrder) => workOrder.partType === "BOF" && workOrder.status == 1);
        const totalcount = workOrdersWithStatus1.length;
        $('#noOfPoBof').text(totalcount);
        const totalspo = data.filter((workOrder) => workOrder.partType === "BOF");
        const totalbof = totalspo.length;
        $('#totalOpenBof').text(totalbof);
        const totalspor = data.filter((workOrder) => workOrder.partType === "RawMaterial");
        const totalbofr = totalspor.length;
        $('#totalOpenRm').text(totalbofr);
        const workOrdersWithStatus10 = data.filter((workOrder) => workOrder.partType === "BOF" && workOrder.status == 2);
        const totalcounto = workOrdersWithStatus10.length;
        $('#noOfOpenBof').text(totalcounto);
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}



$(document).ready(function () {
    loadPO();

    $("#searchPoSupplier").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#PoGrid1 tbody");
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
    $("#searchPoSupplier2").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid2 tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#PoGrid2 tbody");
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
    $("#searchPoStatus2").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid2 tbody tr").filter(function () {
            $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#PoGrid2 tbody");
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
    $("#searchPoPartNo2").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid2 tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#PoGrid2 tbody");
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
    $("#searchPoPartDesc2").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid2 tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#PoGrid2 tbody");
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
    $("#searchPoSuppType").on("change", function () {
        var value = $(this).val().toLowerCase();
        if (value == "1") {
            var d = "prodn";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(d) > -1)
            });
            var $tableBody = $("#PoGrid1 tbody");
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
        else if (value == "2") {
            var d = "stock";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(d) > -1)
            });
            var $tableBody = $("#PoGrid1 tbody");
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
            $("#PoGrid1 tbody tr").show();
            var $tableBody = $("#PoGrid1 tbody");
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
    $("#searchPoType").on("change", function () {
        var value = $(this).val().toLowerCase();
        if (value == "1") {
            var d = "prodn";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(d) > -1)
            });
            var $tableBody = $("#PoGrid1 tbody");
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
        else if (value == "2") {
            var d = "stock";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(d) > -1)
            });
            var $tableBody = $("#PoGrid1 tbody");
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
            $("#PoGrid1 tbody tr").show();
            var $tableBody = $("#PoGrid1 tbody");
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

    $('#Popup13').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var poref = relatedTarget.data("poref");
        var podate = relatedTarget.data("podate");
        var podetails = relatedTarget.data("podetails");
        var procid = relatedTarget.data("procid");
        var supp = relatedTarget.data("supp");
        $("#p13SPanPoRef").text(poref);
        $("#p13SpanPoDate").text(podate);
        $("#p13SpanSupp").text(supp);
        api.getbulk("/WorkOrder/GetAllPodetails").then((data) => {
            data = data.filter(item => item.poDetailsId === podetails);
            var tablebody = $("#PoGridP13 tbody");
            $(tablebody).html("");
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
            for (let i = 0; i < data.length; i++) {
                data[i].poDateStr = data[i].dateStr;
                data[i].viewDeliveryClass = data[i].poQntyRecd > 0 ? "" : "disabled-link";
                data[i].deleteClass = data[i].poQntyRecd === 0 ? "" : "disabled-link";
                data[i].shortCloseClass = data[i].poQntyRecd > 0 ? "" : "disabled-link";
                $(tablebody).append(AppUtil.ProcessTemplateData("P13GridRow", data[i]));
            }
        });

    });

    $('#popup10').on('shown.bs.modal', function (event) {
        $("#P10UnitSpan").text("Nos");
        var relatedTarget = $(event.relatedTarget);
        var partid = relatedTarget.data("partid");
        var poref = relatedTarget.data("poref");
        var customername = relatedTarget.data("customername");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var moq = relatedTarget.data("moq");
        var qnty = relatedTarget.data("qnty");
        var calcdate = relatedTarget.data("calcdate");
        var uom = relatedTarget.data("uom");
        var price = relatedTarget.data("price");
        var procid = relatedTarget.data("procid");
        $("#P10PartNo").val(partno);
        $("#P10Ref").text(poref);
        $("#P10UnitSpan").text(uom);
        $("#P10TotalPoQnty").val(qnty);
        $("#P10CalcReq").val(qnty);
        $("#P10balQnty").val(qnty);
        $("#P10DelQnty").val(qnty);
        $("#P10DateReqd").val(calcdate);
        $("#Popup10Woid").val(procid);
        $("#P10AddnInfo").val('');
        $("#P10AgrDate").val('');
        GetAllSubConsP10(procid);
        LoadSupplierRM(parseInt(partid));
    });


    $('#P10Supplier').on('change', (e) => {
        const routeId = $(e.target).val();
        var edata = suppilerselecet.filter(item => item.supplierId == routeId);
        if (edata[0].strPreferredSubCon === "") {
            $("#P10PreferredSpan").hide();
        } else {
            $("#P10PreferredSpan").show();
        }
        var cost = edata[0].costPerPart;
        $("#P10Convprice").val(cost);
        $("#P10UnitRout").val(cost);
        $("#P10Moq").val(data[0].minimumOrderQuantity);
    });
    $('#chkP10UpdateQnty').change(function () {
        if ($(this).is(':checked')) {
            var qnt = $("#P10Moq").val();
            $("#P10TotalPoQnty").val(qnt);
        }
    });
    $('#chkP10RoutPrice').change(function () {
        if ($(this).is(':checked')) {
            var qnt = $("#P10UnitRout").val();
            $("#P10Convprice").val(qnt);
        }
    });
    $('#P20Supplier').on('change', (e) => {
        const routeId = $(e.target).val();
        var NewStartOpNo = $("#NewStartOpNo").val();
        if (NewStartOpNo == null) {
            NewStartOpNo = $("#StartingOpNo").val();
        }
        if (NewStartOpNo == null) {
            NewStartOpNo = $("#Popup7woid").val();
        }
        if (NewStartOpNo == null) {
            NewStartOpNo = $("#woid").val();
        }
        api.get("/routings/subcons?stepId=" + NewStartOpNo).then((data) => {
            var edata = data.filter(item => item.supplierId == routeId);
            if (edata[0].strPreferredSubCon === "") {
                $("#P20PreferredSpan").hide();
            } else {
                $("#P20PreferredSpan").show();
            }
            var cost = edata[0].costPerPart;
            $("#P20UnitRout").val(cost);
            var workOrderId = $("#Popup20Woid").val();
            api.getbulk("/WorkOrder/GetAllSubCons?woid=" + workOrderId).then((data) => {
                data = data.filter(item => item.supplierId === parseInt(routeId));
                if (data.length > 1) {
                    $("#flexSwitchCheckDefault").prop("disabled", true);
                    $("#flexSwitchCheckDefault").prop("checked", true);
                } else {
                    $("#flexSwitchCheckDefault").prop("disabled", false);
                }
            }).catch((error) => {
            });
        }).catch((error) => {
            //console.error(error);
        });
    });
    $('#popup20').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var procid = relatedTarget.data("procid");
        $("#flexSwitchCheckDefault").prop("disabled", false);
        var supp = relatedTarget.data("supp");
        var podate = relatedTarget.data("podate");
        var podetails = relatedTarget.data("podetails");
        var partId = relatedTarget.data("partid");
        var partno = relatedTarget.data("partno");
        var qnty = relatedTarget.data("qnty");
        $("#popup20PartNo").text(partno);
        $("#P20DelQnty").val(qnty);
        $("#P20WoQnty").val(qnty);
        $("#P20DateReqd").val(podate);
        $("#Popup20Woid").val(procid);
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        $('#P20AgrDate').val('');
        $('#P20AddnInfo').val('');
        $("#flexSwitchCheckDefault").prop("checked", false);
        $("#P20AddNextDel").prop("disabled", true);
        var P20AddnInfo = document.getElementById('P20AddnInfo');
        P20AddnInfo.style.border = '';
        var P20AgrDate = document.getElementById('P20AgrDate');
        P20AgrDate.style.border = '';
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        supdate = {};
        subcontotal = 0;
        api.get("/masters/partpurchasesfor?partId=" + partId).then((rData) => {
            const selectElement = $('#P20Supplier');
            selectElement.html("");
            $.each(rData, (index, item) => {
                selectElement.append(`<option value="${item.pSupplierId}">${item.pSupplier}</option>`);
            });
            if (rData.length == 1) {
                $("#P20AddOtherSupp").prop("disabled", true);
            } else {
                $("#P20AddOtherSupp").prop("disabled", false);
            }
            
            $("#P20Convprice").val(rData[0].price);
            $("#P20UnitRout").val(rData[0].price);
        }).catch((error) => {
        });
        GetAllSubCons(procid);
    });
    $("#P20AddNextDel").on("click", function () {
        $("#P20DelQnty").val('');
        $("#P20AgrDate").val('');
        $("#P20Convprice").val('');
        //$("#P10Supplier").val('');
        $("#Popup20WoSubConId").val('');
        $("#P20AddnInfo").val('');
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        var P20AddnInfo = document.getElementById('P20AddnInfo');
        P20AddnInfo.style.border = '';
        var P20AgrDate = document.getElementById('P20AgrDate');
        P20AgrDate.style.border = '';

    });
    $("#P20AddOtherSupp").on("click", function () {
        $("#P20DelQnty").val('');
        $("#P20AgrDate").val('');
        $("#P20Convprice").val('');
        $("#P20Supplier").val('');
        $("#Popup20WoSubConId").val('');
        $("#P20AddnInfo").val('');
        var P20DelQnty = document.getElementById('P20DelQnty');
        P20DelQnty.style.border = '';
        var P20AgrDate = document.getElementById('P20AgrDate');
        P20AgrDate.style.border = '';
        var P20AddnInfo = document.getElementById('P20AddnInfo');
        P20AddnInfo.style.border = '';
    });
    var supdate = {};
    var totalwoplanqnty = 0;
    $("#P20SaveWo").on("click", function () {
        var qntys = $("#P20DelQnty").val();
        var P20AgrDate = $("#P20AgrDate").val();
        var P20AddnInfo = $("#P20AddnInfo").val();
        var P20Convprice = $("#P20Convprice").val();
        var P20Supplier = $("#P20Supplier").val();
        var Newwoid = $("#Popup20Woid").val();
        var Popup20WoSubConId = $("#Popup20WoSubConId").val();
        var P10balQnty = $("#P20WoQnty").val();
        var balQtyToProcure = parseInt(P10balQnty);
        var diff = parseInt(balQtyToProcure) - parseInt(subcontotal);
        if (Popup20WoSubConId.length === 0) {
            if (parseInt(qntys) > diff || subcontotal > balQtyToProcure) {
                alert("Delivery Qnty should be less than or equal to Difference Qnty." + diff);
                return false;
            }
        }
        if (qntys.length <= 0 || parseInt(qntys) == 0) {
            var newNamevalidate = document.getElementById('P20DelQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P20DelQnty');
            P20DelQnty.style.border = '';
        }
        if (P20AgrDate.length <= 0) {
            var newNamevalidate = document.getElementById('P20AgrDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P20AgrDate');
            P20DelQnty.style.border = '';
        }
        if (P20AddnInfo.length <= 0) {
            var newNamevalidate = document.getElementById('P20AddnInfo');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P20AddnInfo');
            P20DelQnty.style.border = '';
        }
        var delv = "";
        var chekboxes = document.getElementById('flexSwitchCheckDefault');
        if (chekboxes.checked) {
            $("#P20AddNextDel").prop('disabled', false);
            delv = "MD";
        } else {
            $("#P20AddNextDel").prop('disabled', true);
            delv = "SDD";
        }
        var rowData = {
            woSubConSupplierId: parseInt(Popup20WoSubConId),
            procPlanId: parseInt(Newwoid),
            supplierId: parseInt(P20Supplier),
            deliveryDate: delv,
            qnty: qntys,
            procPrice: P20Convprice,
            addnInfo: P20AddnInfo,
            recieptDate: P20AgrDate
        };

        api.post("/WorkOrder/WoSubConSupplier", rowData).then((data) => {
            //loadWO();
            $("#Popup20WoSubConId").val('');
            GetAllSubCons(Newwoid);
        }).catch((error) => {
        });
    });
    $("#P10SaveWo").on("click", function () {
        var qntys = $("#P10DelQnty").val();
        var P20AgrDate = $("#P10AgrDate").val();
        var P20AddnInfo = $("#P10AddnInfo").val();
        var P20Convprice = $("#P10Convprice").val();
        var P20Supplier = $("#P10Supplier").val();
        var Newwoid = $("#Popup10Woid").val();
        var P10CalcReq = $("#P10CalcReq").val();
        var subconid = $("#Popup10WoSubConId").val();
        var P10balQnty = $("#P10CalcReq").val();
        var balQtyToProcure = parseInt(P10balQnty);
        var rowData = {
            woSubConSupplierId: parseInt(subconid),
            procPlanId: parseInt(Newwoid),
            supplierId: parseInt(P20Supplier),
            qnty: qntys,
            procPrice: P20Convprice,
            addnInfo: P20AddnInfo,
            recieptDate: P20AgrDate
        };
        if (subconid.length === 0) {
            if (parseInt(qntys) < balQtyToProcure) {
                alert("Delivery Qnty should greater than or equal to the PO Qnty");
                return false;
            }
        }
        if (qntys.length <= 0 || parseInt(qntys) == 0) {
            var newNamevalidate = document.getElementById('P10DelQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P10DelQnty');
            P20DelQnty.style.border = '';
        }
        if (P20AgrDate.length <= 0) {
            var newNamevalidate = document.getElementById('P10AgrDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P10AgrDate');
            P20DelQnty.style.border = '';
        }
        const currentDate = new Date();
        const userDate = new Date(P20AgrDate);
        if (userDate < currentDate ) {
            alert('Please Enter A Date Greater Than Today\'s Date ');
            $("#P20AgrDate").val('');
            return;
        }
        var chekboxes = document.getElementById('flexSwitchCheckDefault');
        if (chekboxes.checked) {
            $("#P10AddNextDel").prop('disabled', false);
        } else {
            $("#P10AddNextDel").prop('disabled', true);

        }

        api.post("/WorkOrder/WoSubConSupplier", rowData).then((data) => {
            //loadWO();
            GetAllSubConsP10(Newwoid);
            $("#P10DelQnty").val('');
            $("#P10AgrDate").val('');
            $("#P10AddnInfo").val('');
            $("#P10Convprice").val('');
            $("#Popup10WoSubConId").val('');
        }).catch((error) => {
        });
    });

    $("#searchPoToDate2").on("change", function () {
        var fromDate = $("#searchPoFrDate2").val().split("/").reverse().join("-");
        var toDate = $("#searchPoToDate2").val().split("/").reverse().join("-");
        var fromDateTimestamp = new Date(fromDate).getTime();
        var toDateTimestamp = new Date(toDate).getTime();

        if (fromDateTimestamp > toDateTimestamp) {
            alert("From Date Is Greater Than To Date");
            $("#searchPoFrDate2").val('');
            $("#searchPoToDate2").val('');
            return false;
        }
        $("#PoGrid2 tbody tr").filter(function () {
            var dateText = $(this.children[1]).text(); // assuming the date is in the 3rd column
            var tableDate = dateText.split("-").reverse().join("-");

            $(this).toggle(tableDate >= fromDate && tableDate <= toDate);
        });
    });
    $("#SoLogcomplDtTo").on("change", function () {
        var fromDate = $("#SoLogcomplDtFrom").val().split("/").reverse().join("-");
        var toDate = $("#SoLogcomplDtTo").val().split("/").reverse().join("-");
        var fromDateTimestamp = new Date(fromDate).getTime();
        var toDateTimestamp = new Date(toDate).getTime();

        if (fromDateTimestamp > toDateTimestamp) {
            alert("PO Compl Dt From Is Greater Than PO Compl Dt To");
            $("#SoLogcomplDtFrom").val('');
            $("#SoLogcomplDtTo").val('');
            return false;
        }
        $("#WoLogGrid tbody tr").filter(function () {
            var dateText = $(this.children[0]).text(); // assuming the date is in the 3rd column
            var tableDate = dateText.split("-").reverse().join("-");

            $(this).toggle(tableDate >= fromDate && tableDate <= toDate);
        });
    });
    $("#searchLogPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#WoLogGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#WoLogGrid tbody");
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
    $("#searchEventLog").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#WoLogGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#WoLogGrid tbody");
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
    $("#searchLogComment").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#WoLogGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[7]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#WoLogGrid tbody");
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

    $('#view-socomment').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        $("#viewCommentField").val('');
        var woid = relatedTarget.data("procid");
        api.getbulk("/WorkOrder/GetAllPoSubCons?woid=" + woid).then((data) => {
            data = data.filter((po) => po.addnInfo);
            $("#viewCommentField").val(data[0].addnInfo);
        });
    });
    $('#ViewSoLog').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var workorderid = relatedTarget.data("podetails");
        var requiredbydatestr = relatedTarget.data("aggdate");
        var partno = relatedTarget.data("partno");
        var partdesc = relatedTarget.data("partdesc");
        var wonumber = relatedTarget.data("poref");
        var salesorderid = relatedTarget.data("salesorderid");
        $("#SpanSoNumber").text(wonumber);
        $("#SoDateSpan").text(requiredbydatestr);
        $("#SoPartNoSPan").text(partno);
        $("#SoPartDescSPan").text(partdesc);
        $("#SoLogcomplDtFrom").val('');
        $("#SoLogcomplDtTo").val('');
        $("#searchLogPartNo").val('');
        $("#searchEventLog").val('');
        $("#searchLogComment").val('');
        LoadPOLogs(workorderid, partno);
    });
});

function LoadPOLogs(customerOderId,partno) {
    //GetMasterParts();
    api.get("/workorder/GetPOLogs?customerOrderId=" + customerOderId).then((data) => {
        //console.log(data);
        var tablebody = $("#WoLogGrid tbody");
        $(tablebody).html("");//empty tbody
        for (i = 0; i < data.length; i++) {
            let fullDateTime = data[i].poDateStr;
            let onlyDate = fullDateTime.split(' ')[0];
            data[i].poDateStr = onlyDate;
            data[i].partNo = partno;
            $(tablebody).append(AppUtil.ProcessTemplateData("PORow", data[i]));
        }
    }).catch((error) => {
    });
}
function GetAllSubCons(woid) {
    api.getbulk("/WorkOrder/GetAllPoSubCons?woid=" + woid).then((data) => {
        //data = data.filter(item => item.woId === woid);
        var tablebody = $("#P20SupplierGrid tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        let totalQuantity = 0;
        let totalprice = 0;
        subcontotal = 0;
        for (i = 0; i < data.length; i++) {
            totalQuantity += Number(data[i].qnty);
            totalprice += Number(data[i].procPrice);
            $(tablebody).append(AppUtil.ProcessTemplateData("P10SupplierGridRow", data[i]));
            subcontotal = totalQuantity;
        }
        const totalRow = `
            <tr>
                <td> </td>
                <td style="text-align: center; font-weight: bold;">Total Quantity : ${totalQuantity}</td>
                <td style="text-align: center; font-weight: bold;">Total Price : ${totalprice}</td>
                <td> </td>
            </tr>
        `;

        $(tablebody).append(totalRow);
    }).catch((error) => {
    });
}
function GetAllSubConsP10(woid) {
    api.getbulk("/WorkOrder/GetAllPoSubCons?woid=" + woid).then((data) => {
        //data = data.filter(item => item.woId === woid);
        var tablebody = $("#P10SupplierGrid tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        let totalQuantity = 0;
        let totalprice = 0;
        subcontotal = 0;
        for (i = 0; i < data.length; i++) {
            totalQuantity += Number(data[i].qnty);
            totalprice += Number(data[i].procPrice);
            $(tablebody).append(AppUtil.ProcessTemplateData("P10SupplierGridRow", data[i]));
            subcontotal = totalQuantity;
        }
        const totalRow = `
            <tr>
                <td> </td>
                <td style="text-align: center; font-weight: bold;">Total Quantity : ${totalQuantity}</td>
                <td style="text-align: center; font-weight: bold;">Total Price : ${totalprice}</td>
                <td> </td>
            </tr>
        `;

        $(tablebody).append(totalRow);
    }).catch((error) => {
    });
}

function EditSubSupplier(element) {
    var relatedTarget = $(element);
    var calcdate = relatedTarget.data("calcdate");
    var price = relatedTarget.data("price");
    var qnty = relatedTarget.data("qnty");
    var subconid = relatedTarget.data("subconid");
    var workOrderId = relatedTarget.data("procpalnid");
    var suppid = relatedTarget.data("suppid");
    var addninfo = relatedTarget.data("addninfo");
    var delv = relatedTarget.data("delv");
    let datePart = calcdate.split("T")[0];
    $("#P20Supplier").val(suppid);
    $("#Popup20Woid").val(workOrderId);
    $("#Popup20WoSubConId").val(subconid);
    $("#Popup20WoSubConId").val(subconid);
    $("#P20AgrDate").val(datePart);
    $("#P20DelQnty").val(qnty);
    $("#P20AddnInfo").val(addninfo);
    $("#P20Convprice").val(price);
    $("#P10Supplier").val(suppid);
    $("#Popup10Woid").val(workOrderId);
    $("#Popup10WoSubConId").val(subconid);
    $("#Popup10WoSubConId").val(subconid);
    $("#P10AgrDate").val(datePart);
    $("#P10DelQnty").val(qnty);
    $("#P10AddnInfo").val(addninfo);
    $("#P10Convprice").val(price);
    if (delv === "MD") {
        $("#flexSwitchCheckDefault20").prop("checked", true);
        $("#flexSwitchCheckDefault").prop("checked", true);
    } else {
        $("#flexSwitchCheckDefault20").prop("checked", false);
        $("#flexSwitchCheckDefault").prop("checked", false);
    }
    api.getbulk("/WorkOrder/GetAllSubCons?woid=" + workOrderId).then((data) => {
        data = data.filter(item => item.supplierId === suppid);
        if (data.length > 1) {
            $("#flexSwitchCheckDefault20").prop("disabled", true);
            $("#flexSwitchCheckDefault").prop("disabled", true);
            $("#flexSwitchCheckDefault").prop("checked", true);
            $("#flexSwitchCheckDefault20").prop("checked", true);
        } else {
            $("#flexSwitchCheckDefault").prop("disabled", false);
            $("#flexSwitchCheckDefault20").prop("disabled", false);
        }
    }).catch((error) => {
    });
}
function DeleteSubSupplier(element) {
    var relatedTarget = $(element);
    var workOrderId = relatedTarget.data("procpalnid");
    var subconid = relatedTarget.data("subconid");
    api.getbulk("/WorkOrder/DeleteSubCon?id=" + subconid).then((data) => {
        GetAllSubConsP10(workOrderId);
    });
}
function LoadSupplierRM(partid) {
    api.getbulk("/masters/partpurchasesfor?partId=" + parseInt(partid)).then((data) => {
        const selectElement = $('#P10Supplier');
        selectElement.html("");
        suppilerselecet = data;
        $.each(data, (index, item) => {
            selectElement.append(`<option value="${item.pSupplierId}">${item.pSupplier}</option>`);
        });
        if (data[0].preferredSupplier == 1) {
            $("#P10PreferredSpan").show();
        } else {
            $("#P10PreferredSpan").hide();
        }
        if (data.length == 1) {
            $("#P10AddOtherSupp").prop("disabled", true);
        } else {
            $("#P10AddOtherSupp").prop("disabled", false);
        }

        $("#P10Moq").val(data[0].minimumOrderQuantity);
        $("#P10Convprice").val(data[0].price);
        $("#P10UnitRout").val(data[0].price);
    }).catch((error) => {
    });
}