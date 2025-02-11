
var subcontotal = 0;
function loadPO() {
    api.getbulk("/WorkOrder/GetAllPodetails").then((data) => {
        data = data.filter(item => item.status === 1);
        var tablebody = $("#PoGrid1 tbody");
        $(tablebody).html("");
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateData("POGrid2Row", data[i]));
            $('#TotalPoValue').val(0);
        }
    }).catch((error) => {
    });
}


$(document).ready(function () {

    $('#PoGridChk').change(function () {
        if ($(this).is(":checked")) {
            $('#PoGrid1 tbody').find('input[type="checkbox"]').prop('checked', true);
            $('#McListBtn').prop('disabled', false);
        } else {
            $('#PoGrid1 tbody').find('input[type="checkbox"]').prop('checked', false);
            $('#McListBtn').prop('disabled', true);
        }
    });

    $("#searchPoSupplier").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchPoPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchPoPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PoGrid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchPoFor3").on("change", function () {
        var value = $(this).val().toLowerCase();
        if (value == "1") {
            var d = "prodn";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(d) > -1)
            });
        }
        else if (value == "2") {
            var d = "stock";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[10]).text().toLowerCase().indexOf(d) > -1)
            });
        } else {
            $("#PoGrid1 tbody tr").show();
        }
    });
    $("#searchPoType").on("change", function () {
        var value = $(this).val().toLowerCase();
        if (value == "1") {
            var d = "prodn";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[10]).text().toLowerCase().indexOf(d) > -1)
            });
        }
        else if (value == "2") {
            var d = "stock";
            $("#PoGrid1 tbody tr").filter(function () {
                $(this).toggle($(this.children[10]).text().toLowerCase().indexOf(d) > -1)
            });
        } else {
            $("#PoGrid1 tbody tr").show();
        }
    });

    function handleCheckboxChange() {
        var checkboxes = $("#PoGrid1 tbody input[type='checkbox']:checked");
        var totalQuantity = 0;
        checkboxes.each(function () {
            var row = $(this).closest('tr');

            var row = $(this).closest('tr'); 
            var partId = $(row).find("td:eq(5)").text().trim();
            var price = $(row).find("td:eq(7)").text().trim();
            var quantity = parseFloat(partId) || 0;
            var floatprice = parseFloat(price) || 0;
            var povalue = quantity * floatprice;
            totalQuantity += povalue;
        });
        $('#TotalPoValue').val(totalQuantity);
        if (checkboxes.length === 1) {
            $('#McListBtn').prop('disabled', false);
        }
        else if (checkboxes.length > 1) {
            $('#McListBtn').prop('disabled', false); // Enable the btnAG button
        }
        else {
            $('#McListBtn').prop('disabled', true); // Disable the button
        }
    }

    $('#PoGrid1 tbody').on('change', 'input[type="checkbox"]', handleCheckboxChange);
    handleCheckboxChange();

    loadPO();

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
        var calcdate = relatedTarget.data("aggdate");
        var uom = relatedTarget.data("uom");
        var price = relatedTarget.data("price");
        var procid = relatedTarget.data("procid");
        $("#P10PartNo").val(partno);
        $("#popup10PartNo").text(partno);
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

    $("#P10AddNextDel").on("click", function () {
        $("#P10DelQnty").val('');
        $("#P10AgrDate").val('');
        $("#P10Convprice").val('');
        //$("#P10Supplier").val('');
        $("#Popup10WoSubConId").val('');
        $("#P10AddnInfo").val('');

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
    $("#P10AddOtherSupp").on("click", function () {
        $("#P10DelQnty").val('');
        $("#P10AgrDate").val('');
        $("#P10Convprice").val('');
        $("#P10Supplier").val('');
        $("#Popup10WoSubConId").val('');
        $("#P10AddnInfo").val('');

    });
    $("#P10SaveWo").on("click", function () {
        var qntys = $("#P10DelQnty").val();
        var P20AgrDate = $("#P10AgrDate").val();
        var P20AddnInfo = $("#P10AddnInfo").val();
        var P10DateReqd = $("#P10DateReqd").val();
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
        if (qntys.length <= 0 || parseInt(qntys) == 0) {
            var newNamevalidate = document.getElementById('P10DelQnty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P10DelQnty');
            P20DelQnty.style.border = '';
        }
        if (subconid.length === 0) {
            if (parseInt(qntys) < balQtyToProcure) {
                alert("Delivery Qnty should greater than or equal to the Bal Qnty to Procure.");
                return false;
            }
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
        const [day, month, year] = P10DateReqd.split("-");
        const lessDate = new Date(`${year}-${month}-${day}`);
        if (userDate < currentDate || userDate > lessDate) {
            alert('Please Enter A Date Greater Than Today\'s Date And Less Than Date Reqd.');
            $("#P10AgrDate").val('');
            return;
        }
        if (P20AddnInfo.length <= 0) {
            var newNamevalidate = document.getElementById('P10AddnInfo');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var P20DelQnty = document.getElementById('P10AddnInfo');
            P20DelQnty.style.border = '';
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


    $("#McListBtn").on("click", function () {
        var selectedRowsData = {};
        var temprowdata = {};
        var checkboxes = $("#PoGrid1 tbody input[type='checkbox']:checked");
        checkboxes.each(function (index, checkbox) {
            var row = checkbox.parentNode.parentNode;
            var rowData = {
                poDetailsId: parseInt($(row).find("td:eq(13)").text()),
                partId: parseInt($(row).find("td:eq(14)").text()),
            };
            temprowdata[rowData.partId] = rowData;
        });

        selectedRowsData = Object.values(temprowdata);
        if (selectedRowsData.length > 0) {
            $.ajax({
                type: "POST",
                url: '/WorkOrder/UpdatePOdetails',
                contentType: "application/json; charset=utf-8",
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify(selectedRowsData),
                dataType: "json",
                success: function (result) {
                    loadPO();
                    let userChoice = confirm("Select PO Approved successfully. Do you want to go to the PO List page?");

                    if (userChoice) {
                        window.location.href = "/WorkOrder/POLineList"; 
                    }
                }
            });
        } else {
            alert("Please select at least one material");
        }
    });
});
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