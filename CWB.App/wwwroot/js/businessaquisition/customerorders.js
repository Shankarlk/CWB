var ba_masterparts = {};
var schedules = new Array();
var holdsalesorder = false;
var deletesalesorder = false;
var salesCustOrderId = 0;
var editSalesOrder = false;

const OrdStatus = {
    1: "Not Planned",
    2: "Planned",
    3: "Matl Recd",
    4: "WIP",
    5: "Complete",
    6: "On Hold",
    7: "Deleted"
};

function LoadPOLines(customerOrderId) {
    api.get("/businessaquisition/getpolines?customerOrderId=" + customerOrderId).then((data) => {
        var tablebody = $("#POLinesTable tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            if (data[i].numSalesOrder > 1) {
                data[i].strStatus = " Multiple";
            } else {
                data[i].strStatus = OrdStatus[data[i].status];
            }
            if (data[i].hold) {
                data[i].strHold = "Y";
            }
            else
                data[i].strHold = "N";
            if (data[i].done) {
                data[i].strDone = "Y";
            }
            else
                data[i].strDone = "N";
            //data[i].customerName = GetNameForCustomer(data[i].customerId);
            //$(tablebody).append(AppUtil.ProcessTemplateData("POLineRow", data[i]));

            // Render row as jQuery object so we can mutate before appending
            var $row = $(AppUtil.ProcessTemplateData("POLineRow", data[i]));

            // If more than one sales order, remove the Hold/Resume link
            if (data[i].numSalesOrder > 1) {
                $row.find('.hold-resume').remove();
            }

            $(tablebody).append($row);
        }
        //  console.log($(tablebody).html());
    }).catch((error) => {
    });
}

function LoadCustomerOrders() {
    $("#preloaderblurred").show();
    
    api.get("/businessaquisition/getcustorders").then((data) => {
        var tablebody = $("#CustomerOrders tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            //data[i].strStatus = OrdStatus[data[i].status];
            //if (data[i].hold) {
            //    data[i].strHold = "Y";
            //}
            //else
            //    data[i].strHold = "N";
            if (data[i].done) {
                data[i].strDone = "Y";
            }
            else
                data[i].strDone = "N";
            //data[i].customerName = GetNameForCustomer(data[i].customerId);
            $(tablebody).append(AppUtil.ProcessTemplateData("CustomerOrdersRow", data[i]));
        }
        //  console.log($(tablebody).html());
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}

function LoadTempForm() {
    var totalQty = 0;
    //console.log(schedules);
    for (var i = 0; i < schedules.length; i++) {
        totalQty += schedules[i].requiredQuantity;
        var partId = schedules[i].dsPartId;
        var partNo = schedules[i].partNo;
        $("#PartNo").val(partNo);
        $("#PartId").val(partId);
        //$("#PartId").trigger('change');
        $('#DSPartId').val(partId);
        $('#DSPartNo').val(partNo);
        //console.log(partId + "/" + partNo);
        document.getElementById("LaunchDeliverySchedule").disabled = false;
    }
    $('#TotalQty').val(totalQty);
}

function showComments(comments, soNumber) {
    if (comments.length === 0 || comments === "null" || soNumber ==="Multiple") {
        $("#view-socomment").modal('hide');
    } else {
        $("#ViewSoCommentSpan").text(soNumber);
        $("#viewCommentField").text(comments);
        $("#view-socomment").modal('show');
    }
}
function showSalesOrdersForPart(partId,partNo) {
    if (ba_masterparts.length == 0) {
        alert("Please load parts again...");
        return;
    }
    else {
        //alert("calling copyData");
    }
    var customerorderid = $('#AggregateCustomerOrderId').val();
    //$('#SalesCustomerOrderId').val();
    document.getElementById("DeliveryScheduleForm").reset();
    document.getElementById("AggregateObjForm").reset();
    $('#PartId').val(partId);
    $('#PartNo').val(partNo);
    $('#DSPartId').val(partId);
    $('#DSPartNo').val(partNo);
    $('#LaunchDeliverySchedule').hide(partNo);
    $('#AggregateCustomerOrderId').val(customerorderid);
    $('#SalesCustomerOrderId').val(customerorderid);
    //console.log(partId + "/" + partNo);
    LoadSalesOrders(customerorderid);
    LoadDeliverySchedules($('#SalesCustomerOrderId').val());
    document.getElementById("LaunchDeliverySchedule").disabled = false;
    //document.getElementById("btnPONoDetails").click();
    $("#PONoDetailsPopup").modal('show');

}
function copyPartData() {

    if (ba_masterparts.length == 0) {
        alert("Please load parts again...");
        return;
    }
    else {
        //alert("calling copyData");
    }
    var radiochkd = $('input[name=radiopartba]:checked');
    var selval = radiochkd.val();
    var data = ba_masterparts;
    var customerorderid = $('#AggregateCustomerOrderId').val();
    //$('#SalesCustomerOrderId').val();
    
    document.getElementById("DeliveryScheduleForm").reset();
    document.getElementById("AggregateObjForm").reset();
    var partId = data[selval].partId;
     $.ajax({
            type: "GET",
            url: "/masters/CheckPartNoInDocList",
            data: { partId: partId },
            success: function (response) {
                //if (!response) {
                //    alert("This Part Doesnot Have Required Document.");
                //    return;
                //}
                //else {
                    $('#PartId').val(data[selval].partId);
                    $('#PartNo').val(data[selval].partNo);
                    $('#DSPartId').val(data[selval].partId);
                    $('#DSPartNo').val(data[selval].partNo);
                    document.getElementById("LaunchDeliverySchedule").disabled = false;
                    document.getElementById("btn-close-ba-ExistingParts").click();

                    $('#AggregateCustomerOrderId').val(customerorderid);
                    $('#SalesCustomerOrderId').val(customerorderid);
                    //console.log(data[selval].partId + "/" + data[selval].partNo);
                    LoadSalesOrders(customerorderid);
                    LoadDeliverySchedules($('#SalesCustomerOrderId').val());
                //}
        //PostDeliverySchedule();

               }
     });



}

function LoadSalesOrders(customerOrderId) {
    //
    GetMasterParts();
    var partId = $('#PartId').val();
    let totalReqdQty = 0;
    $("#TotalCountDiv").show();
    $("#SalesOrders").show();
    api.get("/businessaquisition/getsalesorders?customerOrderId=" + customerOrderId).then((data) => {
        var tablebody = $("#SalesOrders tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            if (data[i].partId == partId) { }
            else { continue; }

            totalReqdQty += data[i].requiredQuantity || 0;
            /*if (data[i].status == 40) {
                continue;
            }*/
            //data[i].partNo = GetPartNo(data[i].partId);
            //data[i].strStatus = OrdStatus[data[i].status];
            data[i].strHold = "N";
            if (data[i].hold) {
                data[i].strHold = "Y";
            }
            data[i].strDone = "N";
            if (data[i].done) {
                data[i].strDone = "Y";
            }
            //if (data[i].status == 35)
            {
                $(tablebody).append(AppUtil.ProcessTemplateData("SalesOrderRow", data[i]));
            }
        }
        $("#TotalPoQnty").text(totalReqdQty);
        $("#TotalPlanQnty").text(totalReqdQty);
        //LoadCustomerOrders();
    }).catch((error) => {
    });
}

function LoadPartsForSearching() {
    GetMasterParts();
    var tablebody = $("#tbl-ba-existingparts tbody");
    $(tablebody).html("");//empty tbody
    let i = 0;
    if (ba_masterparts.length > 0) {
        ba_masterparts = ba_masterparts.filter(item => item.finalPart === "Y");
        for (i = 0; i < ba_masterparts.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("BAParts", ba_masterparts[i], i));
        }
    }
}

function LoadPOLogs(customerOderId, salesOrderId) {
    //GetMasterParts();
    api.get("/businessaquisition/getpologs?customerOrderId=" + customerOderId).then((data) => {
        //console.log(data);
        var tablebody = $("#POLogTable tbody");
        $(tablebody).html("");//empty tbody
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            //data[i].partNo = GetPartNo(data[i].partId);
            if (data[i].newValue == "NOTPlanned") {
                data[i].newValue = "Not Planned";
            }
            if (data[i].newValue == "OnHold") {
                data[i].newValue = "On Hold";
            }
            if (data[i].oldValue == "NOTPlanned") {
                data[i].oldValue = "Not Planned";
            }
            if (data[i].oldValue == "OnHold") {
                data[i].oldValue = "On Hold";
            }
            if (salesOrderId > 0) {
                if (data[i].partId == 0) { continue; }
            }
            $(tablebody).append(AppUtil.ProcessTemplateData("PORow", data[i]));
        }
    }).catch((error) => {
    });
}

function LoadSOAggregate(customerOrderId) {
    api.get("/businessaquisition/getsoaggregate?customerOrderId=" + customerOrderId).then((data) => {
        //console.log(data);
        $('#TotalQty').val(data.totalQty);
        $('#PartId').val(data.partId);
        $('#PartId').trigger('change');
        $('#SOComment').val(data.comment);
        $('#SOAggregateId').val(data.soAggregateId);
        $('#AggregateCustomerOrderId').val(data.customerOrderId);
    }).catch((error) => {
    });
}


function LoadDeliverySchedules(customerOrderId) {
    api.get("/businessaquisition/getschedules?customerOrderId=" + customerOrderId).then((data) => {
        //console.log(data);
        schedules = new Array();
        var tablebody = $("#DeliverySchedules tbody");
        $(tablebody).html("");//empty tbody
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        //console.log(data);
        var partId = $('#DSPartId').val();
        for (i = 0; i < data.length; i++) {
            /*if (data[i].status == 40) {
                continue;
            }*/

            if (partId == data[i].dsPartId) { }
            else { continue; }

            schedules.push(data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("DeliverScheduleRow", data[i]));
        }

        LoadTempForm();
    }).catch((error) => {
    });
}

function RemoveCustomerOrder(customerOderId) {
    let confirmval = confirm("Are your sure you want to delete this Order?", "Yes", "No");
    if (confirmval) {
        api.get("/businessaquisition/removecustomerorder?cutomerOrderId=" + customerOderId).then((data) => {
            //console.log(data);
            LoadCustomerOrders();
        }).catch((error) => {
        });
    }
}

function RemoveSalesOrder(salesOrderId) {
    let confirmval = confirm("Are your sure you want to delete this Order?", "Yes", "No");
    if (confirmval) {
        api.get("/businessaquisition/removesalesorder?salesOrderId=" + salesOrderId).then((data) => {
            //console.log(data);
            LoadSalesOrders(data.customerOderId);
        }).catch((error) => {
        });
    }
}

function RemoveDeliverySchedule(scheduleId) {
    let confirmval = confirm("Are your sure you want to delete this Delivery Schedule item?", "Yes", "No");
    if (confirmval) {
        api.get("/businessaquisition/removeschedule?scheduleId=" + scheduleId).then((data) => {
            //console.log(data);
            LoadDeliverySchedules($('#SalesCustomerOrderId').val());
        }).catch((error) => {
        });
    }
}

function PostSalesOrder() {
    //SalesOrderForm
    //console.log("....PostSalesOrder....");
    var formData = AppUtil.GetFormData("SalesOrderForm");
    //  console.log(formData);
    api.post("/businessaquisition/salesorder", formData).then((data) => {
        //console.log("****SalesOrder****");
        //console.log(data);
        //console.log("****End-SalesOrder****");
        //document.getElementById("Btn").click();
    }).catch((error) => {
        AppUtil.HandleError("SalesOrderForm", error);
    });
}

function PostAggregateObj() {

    /*//SalesOrderForm
    console.log("....PostAggregateObj....");
    var formData = AppUtil.GetFormData("AggregateObjForm");
    //  console.log(formData);
    api.post("/businessaquisition/soaggregate", formData).then((data) => {
        console.log("****SOAggregate****");
        console.log(data);
        console.log("****End-SOAggregate****");
        //document.getElementById("Btn").click();
    }).catch((error) => {
        AppUtil.HandleError("AggregateObjForm", error);
    });*/
    var cuoid = $('#AggregateCustomerOrderId').val();
    api.get("/businessaquisition/addsalesorders?customerOrderId=" + cuoid).then((data) => {
        //console.log(data);
        LoadSalesOrders(cuoid);
    }).catch((error) => {
    });
}

function PostCustomerOder() {
    //CustomerOrderForm
    //console.log("....PostCustomerOrder....");
    var formData = AppUtil.GetFormData("CustomerOrderForm");
    //  console.log(formData);
    return api.post("/businessaquisition/customerorder", formData).then((data) => {
        //console.log("****CustomerOrder****");
        //console.log(data);
        //console.log("****End-CustomerOrder****");
        $('#AggregateCustomerOrderId').val(data.customerOrderId);
        $('#SalesCustomerOrderId').val(data.customerOrderId);
        //console.log(data.customerOrderId);
        alert("Customer Order Created");
        $('#CustomerOrderForm')[0].reset();
        $("#searchpart").prop("disabled", false);
        //$("#PONoDetailsPopup").modal('show');
        //  $('#').val("");
        //$('#').val("");
        //document.getElementById("Btn").click();
    }).catch((error) => {
        AppUtil.HandleError("CustomerOrderForm", error);
    });
}

function PostDeliverySchedule() {

    //console.log("....PostDeliverySchedule....");
    var formName = "DeliveryScheduleForm";
    if (editSalesOrder) {
        formName = "EditSOForm";
    }
    var formData = AppUtil.GetFormData(formName);
    return api.post("/businessaquisition/deliveryschedule", formData).then((data) => {

        if (editSalesOrder) {
            var cuoid = $('#SalesCustomerOrderId').val();
            LoadSalesOrders(cuoid);
            document.getElementById("BtnEditSalesOrderClose").click();
            LoadDeliverySchedules(cuoid);
        }
        else {
            //console.log("****DeliverySchedule****");
            //console.log(data);
            //console.log("****End-DeliverySchedule****");
            var cuoid = $('#SalesCustomerOrderId').val();
            var partId = $('#DSPartId').val();
            $('#RequiredQuantity').val("");
            $('#DSComment').val("");
            $('#RequiredByDate').val("");
            $('#ScheduleId').val("0");
            //console.log("/" + cuoid);
            LoadDeliverySchedules(cuoid);
            LoadSalesOrders(cuoid);
            LoadPOLines(cuoid);
        }
    }).catch((error) => {
        AppUtil.HandleError("DeliveryScheduleForm", error);
    });
}

function PostPODelete() {
    //DeliveryScheduleForm
    //CustomerOrderForm
    //console.log("....PostPODelete....");
    var formData = AppUtil.GetFormData("podeleteform");
    //  console.log(formData);
    var endpoint = "/businessaquisition/polog";
    if (deletesalesorder) {
        endpoint = "/businessaquisition/solog";
    }

    api.post(endpoint, formData).then((data) => {
        //console.log("****PostPODelete****");
        //console.log(data);
        //console.log("****End-PostPODelete****");
        LoadCustomerOrders();
        document.getElementById("btnpodeleteclose").click();
    }).catch((error) => {
        AppUtil.HandleError("podeleteform", error);
    });
    
}
function PostPOHold() {
    //DeliveryScheduleForm
    //CustomerOrderForm
    //console.log("....PostPOHold....");
    var formData = AppUtil.GetFormData("poholdform");
    //  console.log(formData);
    api.post("/businessaquisition/polog", formData).then((data) => {
        //console.log("****PostPOHold****");
        //console.log(data);
        //console.log("****End-PostPOHold****");
        document.getElementById("poholdform").reset();
        LoadCustomerOrders();
        document.getElementById("btnholdclose").click();
        $("po-hold").modal("hide");
    }).catch((error) => {
        AppUtil.HandleError("poholdform", error);
    });
}
function PostSOHold() {
    //DeliveryScheduleForm
    //CustomerOrderForm
    //console.log("....PostSOHold....");
    var formData = AppUtil.GetFormData("poholdform");
    //  console.log(formData);
    api.post("/businessaquisition/solog", formData).then((data) => {
        //console.log("****PostSOHold****");
        //console.log(data);
        //console.log("****End-PostSOHold****");
        var customerOrderId = $('#AggregateCustomerOrderId').val();
        //console.log(customerOrderId);
        LoadSalesOrders(customerOrderId);
        holdsalesorder = false;
        document.getElementById("btnholdclosePo").click();
        //$("po-hold").modal("hide");
    }).catch((error) => {
        AppUtil.HandleError("poholdform", error);
    });
}
function PostPOResume() {
    //DeliveryScheduleForm
    //CustomerOrderForm
    //console.log("....PostPOResume....");
    var formData = AppUtil.GetFormData("poholdform");
    //  console.log(formData);
    api.post("/businessaquisition/polog", formData).then((data) => {
        //console.log("****PostPOResume****");
        //console.log(data);
        //console.log("****End-PostPOResume****");
        LoadCustomerOrders();
        //document.getElementById("Btn").click();
    }).catch((error) => {
        AppUtil.HandleError("poholdform", error);
    });
}

//Schedules

function SetScheduleEditVals(scheduleId,requiredByDate
    , requiredQuantity, customerOrderId, comment,dsPartId)
{
    //$('#RequiredByDate').val(requiredByDate);
    //console.log("Comment: " + comment);
    document.getElementById('RequiredByDate').value = requiredByDate.split("-").reverse().join("-");
    $('#RequiredQuantity').val(requiredQuantity);
    $('#DSComment').val(comment);
    $('#DSPartId').val(dsPartId);
    $('#ScheduleId').val(scheduleId);
    $('#SalesCustomerOrderId').val(customerOrderId);
    //RequiredByDate
    //RequiredQuantity
    //Comment
    //SalesPartId
    //ScheduleId
    //SalesCustomerOrderId
}
function DeleteSchedule(scheduleId, customerOrderId) {

}

//SalesOrders
function SetSalesOrderEditValues(customerOrderId, partId,comment)
{

}

//CustomerOrders
function AddParts() {
    //debugger;
    if (ba_masterparts.length > 0) {
        var compSelect = $('#PartId');//should be a select2 dropdown
        if (!compSelect.length)
            return;
        compSelect.empty();
        var div_data = "<option value=''></option>";
        compSelect.append(div_data);
        let i = 0;
        for (i = 0; i < ba_masterparts.length; i++) {
            div_data = "<option value='" +
                ba_masterparts[i].partId + "'>" +
                ba_masterparts[i].partNo +
                "</option>";
            compSelect.append(div_data);
        }
    }
}

function GetMasterParts() {
    if (ba_masterparts.length > 0) { }
    else {
        api.get("/masters/masterparts").then((data) => {
            ba_masterparts = data;
        }).catch((error) => {
        });
    }
}

function LoadMasterParts() {
    GetMasterParts();
    //console.log(ba_masterparts);
    if (ba_masterparts.length > 0) {
        //AddParts();
    }
}

function GetPartNo(partId) {
    for (i = 0; i < ba_masterparts.length; i++) {
        if (ba_masterparts[i].partId == partId) {
            return ba_masterparts[i].partNo;
        }
    }
    return "";
}


$(function () {
    //console.log("CustomerOrders Ready");
    var tablebody = $("#CustomerOrders tbody");
    $("#CustomerId").select2();
    loadCustomers("CustomerId");

    $(tablebody).html("");//empty tbody
    //$("#PartId").select2();
    LoadMasterParts();
    LoadCustomerOrders();

    document.getElementById("LaunchDeliverySchedule").disabled = true;

    //  LoadPOLogs();

    /* $("input[type=date]").datepicker({
         dateFormat: 'dd-MM-yyyy',
         onSelect: function (dateText, inst) {
             $(inst).val(dateText); // Write the value in the input
         }
     });
 
     // Code below to avoid the classic date-picker
     $("input[type=date]").on('click', function () {
         return false;
     });*/

    /*$("#PartId").on('change', function () {
        console.log($("#PartId option:selected").val());
        $('#DSPartId').val($("#PartId option:selected").val());
        $('#DSPartNo').val($("#PartId option:selected").text());
        if ($('#DSPartId').val().trim().length > 0) {
            document.getElementById("LaunchDeliverySchedule").disabled = false;
        }
    });*/
    $("#CustomerId").on('change', function () {
        //console.log($("#CustomerId option:selected").val());
        //CustomerName
        $('#CustomerName').val($("#CustomerId option:selected").text());
        var custid = $("#CustomerId option:selected").val();
        api.get("/masters/customers").then((data) => {
            data = data.filter(item => item.companyId === parseInt(custid));
            $('#POAddress').val(data[0].location);
            $('#POCity').val(data[0].city);
            $('#POPIN').val(data[0].pincode);
            $('#POCountry').val(data[0].country);
        });
        // $('#SalesPartId').val($("#PartId option:selected").val());

    });
    $('#ba_existing-part').on('hidden.bs.modal', function (e) {
        document.getElementById('PONoDetailsPopup').style.filter = 'none';
    });
    $('#ba_existing-part').on('shown.bs.modal', function (event) {
        LoadPartsForSearching();
        document.getElementById('PONoDetailsPopup').style.filter = 'blur(5px)';
    });
    
    $('#po-Delete').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var customerorderid = relatedTarget.data("customerorderid");
        var salesorderid = relatedTarget.data("salesorderid");
        var salesorder = relatedTarget.data("salesorder");
        deletesalesorder = false;
        salesCustOrderId = 0;
        if (salesorder == "Y") {
            $('#PODSalesOrderId').val(salesorderid);
            deletesalesorder = true;
            salesCustOrderId = customerorderid;
        }
        else {
            $('#PODSalesOrderId').val("0");
        }
        //console.log("POHold salesorderid " + salesorderid);

        //console.log("POHold " + salesorderid);
        //PODCustomerOrderId
        $('#PODCustomerOrderId').val(customerorderid);
    });

    $('#po-Delete').on('hidden.bs.modal', function (event) {
        if (deletesalesorder) {
            LoadSalesOrders(salesCustOrderId);
        }
        $('#podeleteform')[0].reset();
    });

    $('#po-hold').on('hidden.bs.modal', function (event) {
        $("#POHComment").val('');

        var newNamevalidate = document.getElementById('POHComment');
        newNamevalidate.style.border = '';
        LoadPOLines(salesCustOrderId);
        if (holdsalesorder) {
            LoadSalesOrders(salesCustOrderId);
        }
    });

    
    //;
    $('#po-hold').on('shown.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var customerorderid = relatedTarget.data("customerorderid");
        var salesorderid = relatedTarget.data("salesorderid");
        var salesorder = relatedTarget.data("salesorder");
        var statusstr = relatedTarget.data("statusstr");
        holdsalesorder = false;
        salesCustOrderId = 0;
        if (statusstr == "On Hold") {
            $("#SpanPoHold").text("Resume");
        } else if (statusstr == "Hold") {
            $("#SpanPoHold").text("Resume");
        } else {
            $("#SpanPoHold").text("Hold");
        }
        if (salesorder == "Y") {
            $('#POHSalesOrderId').val(salesorderid);
            holdsalesorder = true;
            salesCustOrderId = customerorderid;
        }
        else {
            $('#POHSalesOrderId').val("0");
        }
        //console.log("POHold salesorderid " + salesorderid);

        //console.log("POHold "+salesorderid);
        //POHCustomerOrderId
        $('#POHCustomerOrderId').val(customerorderid);
    });

    $('#po-log').on('shown.bs.modal', function (event) {
        //LoadPOLogs
        var relatedTarget = $(event.relatedTarget);
        var customerorderid = relatedTarget.data("customerorderid");
        var salesorderid = relatedTarget.data("salesorderid");
        var salesorder = relatedTarget.data("salesorder");
        var podate = relatedTarget.data("podate");
        var ponumber = relatedTarget.data("ponumber");
        $("#SpPoNo").text(ponumber);
        $("#SpPoDate").text(podate);
        var relatedTarget = $(event.relatedTarget);
        var customerorderid = relatedTarget.data("customerorderid");
        //POHCustomerOrderId
        if (customerorderid == undefined) {
            customerorderid = $("#CustomerOrderId").val();
        }
        LoadPOLogs(customerorderid, salesorderid);
    });
    $('#Edit-SalesOrder').on('hidden.bs.modal', function (event) {
        editSalesOrder = false;
        document.getElementById('PONoDetailsPopup').style.filter = 'none';
    });
    $('#Edit-SalesOrder').on('shown.bs.modal', function (event) {
        document.getElementById('PONoDetailsPopup').style.filter = 'blur(5px)';
        editSalesOrder = false;
        document.getElementById("EditSOForm").reset();
        var relatedTarget = $(event.relatedTarget);
        var customerOrderId = relatedTarget.data("customerorderid");
        var requiredbydatestr = relatedTarget.data("requiredbydatestr");
        var requiredquantity = relatedTarget.data("requiredquantity");
        var comment = relatedTarget.data("comment");
        var partId = relatedTarget.data("partid");
        var salesorder = relatedTarget.data("salesorder");
        var scheduleid = relatedTarget.data("salesorderid");
        document.getElementById('EditSORequiredByDate').value = requiredbydatestr.split("-").reverse().join("-");
        $('#EditSORequiredQuantity').val(requiredquantity);
        $('#EditSOComment').val(comment);
        $('#EditSOPartId').val(partId);
        $('#EditSOScheduleId').val(scheduleid);
        $('#EditSOCustomerOrderId').val(customerOrderId);
        //console.log("EditSOScheduleId: " + scheduleid);
        //console.log("EditSOComment: " + comment);
        //console.log("EditSOCustomerOrderId: " + customerOrderId);
        //console.log("EditSOPartId: " + partId);
        if (salesorder == "Y") {
            editSalesOrder = true;
        }
    });
    
    $('#new-order-entry').on('shown.bs.modal', function (event) {
        loadCusomtersFromMem("CustomerId");
        LoadMasterParts();
        var tablebody = $("#SalesOrders tbody");
        $(tablebody).html("");//empty tbody
        var tablebody = $("#POLinesTable tbody");
        $(tablebody).html("");//empty tbody
        var newNamevalidate = document.getElementById('POPIN');
        newNamevalidate.style.border = '';

        $(LaunchDeliverySchedule).prop("disabled", true);
        $("#searchpart").prop("disabled", true);
        document.getElementById("CustomerOrderForm").reset();
        document.getElementById("AggregateObjForm").reset();
        if (IsAddOpCalled()) {
            $("#DirectEntryDetails").hide();
            $("#dentrylbl").hide();
            schedules = new Array();
            return;
        }
        else {
            
            var relatedTarget = $(event.relatedTarget);
            var customerorderid = relatedTarget.data("customerorderid");
            var customerId = relatedTarget.data("customerid");
            var customerName = relatedTarget.data("customername");
            var comment = relatedTarget.data("comment");
            var ordertype = relatedTarget.data("ordertype");
            var ponumber = relatedTarget.data("ponumber");
            var podate = relatedTarget.data("podate");
            var directentrydetails = relatedTarget.data("directentrydetails");
            var poaddress = relatedTarget.data("poaddress");
            var pocity = relatedTarget.data("pocity");
            var popin = relatedTarget.data("popin");
            var pocountry = relatedTarget.data("pocountry");
            $('#CustomerOrderId').val(customerorderid);
            
            $(LaunchDeliverySchedule).prop("disabled", false);
            $("#searchpart").prop("disabled", false);
            //console.log(customerorderid+"/"+customerId);
            //$("#CustomerId").select2();
            $('#CustomerId').val(customerId);
            //$('#CustomerId').trigger('change');
            //console.log("comment: " + comment);
            $('#Comment').val(comment);
            if (ordertype == 1) {
                $("#POEntry").prop('checked', true);
                $("#DirectEntryDetails").hide();
                $("#dentrylbl").hide();
                $("#polbl").show();
                $("#PONumber").show();
                $("#ponodiv").show();
            }
            else {
                $("#polbl").hide();
                $("#PONumber").hide();
                $("#ponodiv").hide();
                $("#DirectEntryDetails").show();
                $("#dentrylbl").show();
                $("#DirectEntry").prop('checked', true);
            }
            $('#PONumber').val(ponumber);
            document.getElementById('PODate').value = podate.split("-").reverse().join("-");
            $('#DirectEntryDetails').val(directentrydetails);
            $('#POAddress').val(poaddress);
            $('#POCity').val(pocity);
            $('#POPIN').val(popin);
            $('#POCountry').val(pocountry);
            $('#AggregateCustomerOrderId').val(customerorderid);
            $('#SalesCustomerOrderId').val(customerorderid);
            LoadDeliverySchedules(customerorderid);
            
            //LoadSOAggregate(customerorderid);
            $('#AggregateCustomerOrderId').val(customerorderid);
            $('#SalesCustomerOrderId').val(customerorderid);
            LoadSalesOrders(customerorderid);
            LoadPOLines(customerorderid);
        }
    });
    $('#DirectEntry').on('click', function () {
        $("#polbl").hide();
        $("#PONumber").hide();
        $("#ponodiv").hide();
        $("#DirectEntryDetails").show();
        $("#dentrylbl").show();
    });
    $('#POEntry').on('click', function () {
        $("#polbl").show();
        $("#ponodiv").show();
        $("#PONumber").show();
        $("#dentrylbl").hide();
        $("#DirectEntryDetails").hide();
    });
    $('#new-order-entry').on('hidden.bs.modal', function (e) {
        LoadCustomerOrders();
    });

    $('#Delivery-Schedule').on('shown.bs.modal', function (e) {
        document.getElementById('PONoDetailsPopup').style.filter = 'blur(5px)';
        var val = $('#DSPartId').val();
        if (val == "0" || val.trim()=="") {
            alert("Please select a part.");
            document.getElementById("BtnAddScheduleClose").click();
            return;
        }

       
        schedules = new Array();
        var dspartNo = $('#DSPartNo').val();
        
        var elm = document.getElementById("PartNoToSchedule");
        elm.innerText = dspartNo;

        var tablebody = $("#DeliverySchedules tbody");
        $(tablebody).html("");//empty tbody
        if (IsAddOpCalled()) {
            return;
        }
        LoadDeliverySchedules($('#SalesCustomerOrderId').val());
    });
    $('#Delivery-Schedule').on('hidden.bs.modal', function (e) {
        document.getElementById('PONoDetailsPopup').style.filter = 'none';
        //schedules = new Array();
        LoadTempForm();
        var customerOrderId = $('#AggregateCustomerOrderId').val();
        //console.log(customerOrderId);
        LoadSalesOrders(customerOrderId);
        LoadPOLines(customerOrderId);
    });
    $("#btnWo").on("click", function () {

    });
    $("#btnlistSO").on("click", function () {
        LoadSalesOrders(1);
    });

    $("#BtnAddCustomerOrder").secureClick( function () {
        // alert("Add CustomerOrder clicked");
        const POPIN = $("#POPIN").val();
        //const podate = $("#PODate").val();
        //const currentDate = new Date();
        //const userDate = new Date(podate);
        //if (userDate > currentDate) {
        //    alert('Please Enter A Date Greater Than Or Equal To Today\'s Date');
        //    $("#PODate").val('');
        //    return;
        //}
        //if (POPIN.length === 0) {
        //    var newNamevalidate = document.getElementById('POPIN');
        //    newNamevalidate.style.border = '2px solid red';
        //    return;
        //} else {
            //var newNamevalidate = document.getElementById('POPIN');
            //newNamevalidate.style.border = '';
            return PostCustomerOder();
        //}
    });

    $("#BtnPODelete").on("click", function () {
        // alert("Add CustomerOrder clicked");
        PostPODelete();
    });

    $("#BtnPOHold").on("click", function () {
        // alert("Add CustomerOrder clicked");

        var comt = $("#POHComment").val();
        if (comt.length == 0) {
            var newNamevalidate = document.getElementById('POHComment');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('POHComment');
            newNamevalidate.style.border = '';
        }
        if (holdsalesorder) {
            PostSOHold();
        }
        else {
            PostPOHold();
        }
        
    });

    $("#BtnAddSalesOrder").on("click", function () {
      //  alert("Add Sales Order clicked");
        if ($("#AggregateCustomerOrderId").val() == "0") {
            alert("Please create a customer oder first.");
            return;
        }
        if (schedules.length == 0) {
            alert("No schedules added.");
            return;
        }
        PostAggregateObj();
        var customerOrderId = $('#AggregateCustomerOrderId').val();
        //console.log(customerOrderId);
        LoadSalesOrders(customerOrderId);
    });
    $("#BtnAddSchedule").on("click", function () {
        //alert("Add Schedule clicked");
        if ($("#SalesCustomerOrderId").val() == "0") {
            alert("Please create a customer oder first.");
            return;
        }
        var partId = $("#DSPartId").val(); // assuming DSPartId is the input field for part number
        //$.ajax({
        //    type: "GET",
        //    url: "/BusinessAquisition/CheckPartNo",
        //    data: { partId: partId },
        //    success: function (response) {
        //        if (!response) {
        //            alert("This part number already exists.");
        //            return;
        //        }
                PostDeliverySchedule();
        //    }
        //});
    });
    $("#SavePoLineItem").secureClick( function () {
        //alert("Add Schedule clicked");
        if ($("#SalesCustomerOrderId").val() == "0") {
            alert("Please create a customer oder first.");
            return false;
        }
        var partId = $("#DSPartId").val();
        var qnty = $("#TotalQty").val();
        var redate = $("#POReqdDate").val();
        var PartNo = $("#PartNo").val();
        var POSoComment = $("#POSoComment").val();
        if (PartNo.length <= 0) {
            var newNamevalidate = document.getElementById('PartNo');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('PartNo');
            newNamevalidate.style.border = '';
        }
        if (parseInt(qnty) === 0 || isNaN(qnty) || qnty.length <= 0) {
            var newNamevalidate = document.getElementById('TotalQty');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('TotalQty');
            newNamevalidate.style.border = '';
        }
        if (redate.length <= 0) {
            var newNamevalidate = document.getElementById('POReqdDate');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('POReqdDate');
            newNamevalidate.style.border = '';
        }
        const currentDate = new Date();
        const userDate = new Date(redate);
        if (userDate < currentDate) {
            alert('Please Enter A Date Greater Than Or Equal To Today\'s Date');
            $("#POReqdDate").val('');
            return false;
        }
        if (POSoComment.length <= 0) {
            var newNamevalidate = document.getElementById('POSoComment');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('POSoComment');
            newNamevalidate.style.border = '';
        }
        $("#RequiredByDate").val(redate);
        $("#EditSORequiredByDate").val(redate);
        $("#RequiredQuantity").val(qnty);
        $("#EditSORequiredQuantity").val(qnty);
        $("#DSComment").val(POSoComment);
        $("#EditSOComment").val(POSoComment);
        if (editSalesOrder) {
            return PostDeliverySchedule();
        } else {
            return PostDeliverySchedule();
        }
        $("#LaunchDeliverySchedule").show();
    });

    $("#LaunchDeliverySchedule").on("click", function () {
        $("#TotalQty").val('');
        $("#POReqdDate").val('');
        $("#POSoComment").val('');
        editSalesOrder = false;
        //document.getElementById("DeliveryScheduleForm").reset();
        //document.getElementById("EditSOForm").reset();

    });
    $("#AddPoLineItem").on("click", function () {
        $("#SalesOrders tbody").html('');
        $("#PartNo").val('');
        $("#POSoComment").val('');
        $("#POReqdDate").val('');
        $("#TotalQty").val(0);
        $("#TotalPoQnty").text(0);
        $("#TotalPlanQnty").text(0);
    });
    $("#BtnEditSO").on("click", function () {
        //alert("Add Schedule clicked");
        if ($("#SalesCustomerOrderId").val() == "0") {
            alert("Please create a customer oder first.");
            return;
        }
        PostDeliverySchedule();
    });

    $("#PoLogToSr").on("change keyup", function () {
        var fromDate = $("#PoLogFromSr").val().trim();
        var toDate = $("#PoLogToSr").val().trim();

        $("#POLogTable tbody tr").each(function () {
            var rowDateTime = $(this).find("td:eq(0)").text().trim(); 
            var rowDate = rowDateTime.split(" ")[0]; 
            var formattedRowDate = convertToISODate(rowDate);
            var formattedFromDate = convertToISODate(fromDate);
            var formattedToDate = convertToISODate(toDate);
            var showRow = true;

            if (formattedFromDate && formattedRowDate < formattedFromDate) {
                showRow = false; // Hide if before FromDate
            }
            if (formattedToDate && formattedRowDate > formattedToDate) {
                showRow = false; // Hide if after ToDate
            }

            $(this).toggle(showRow);
        });
        var $tableBody = $("#POLogTable tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#POLogTable tbody").append(noRecordsRow);
        } else {
            $("#POLogTable tbody").find(".norecordsfound").remove();
        }
    });

    function convertToISODate(dateString) {
        if (!dateString) return "";

        var dateParts = dateString.split("-");
        if (dateParts.length === 3) {
            return `${dateParts[2]}-${dateParts[1].padStart(2, '0')}-${dateParts[0].padStart(2, '0')}`;
        }
        return "";
    }


    $("#PoLogPartNoSr").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#POLogTable tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#POLogTable tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#POLogTable tbody").append(noRecordsRow);
        } else {
            $("#POLogTable tbody").find(".norecordsfound").remove();
        }
    });
    $("#PoLogEventSr").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#POLogTable tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#POLogTable tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#POLogTable tbody").append(noRecordsRow);
        } else {
            $("#POLogTable tbody").find(".norecordsfound").remove();
        }
    });
    $("#PoLogComSr").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#POLogTable tbody tr").filter(function () {
            $(this).toggle($(this.children[8]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#POLogTable tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#POLogTable tbody").append(noRecordsRow);
        } else {
            $("#POLogTable tbody").find(".norecordsfound").remove();
        }
    });
    $("#baeppn").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbl-ba-existingparts tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#tbl-ba-existingparts tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#tbl-ba-existingparts tbody").append(noRecordsRow);
        } else {
            $("#tbl-ba-existingparts tbody").find(".norecordsfound").remove();
        }
    });

    $("#baeppd").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbl-ba-existingparts tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#tbl-ba-existingparts tbody");
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
    $("#Search-BA-Status").change(function () {
        var selectedValue = $(this).val();
        if (selectedValue != "0") {
            var data = selectedValue;
            var value = data.toLowerCase();
            $("#CustomerOrders tbody tr").filter(function () {
                $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#CustomerOrders tbody");
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
            var $tableBody = $("#CustomerOrders tbody");
            $tableBody.find(".norecordsfound").remove();
            $("#CustomerOrders tbody tr").show();
        } else {
            $("#CustomerOrders tbody tr").show();
        }
        
    });
    $("#Search-BA-PONumber").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#CustomerOrders tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#CustomerOrders tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#CustomerOrders tbody").append(noRecordsRow);
        } else {
            $("#CustomerOrders tbody").find(".norecordsfound").remove();
        }
    });

    $("#Search-BA-Customer").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#CustomerOrders tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#CustomerOrders tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#CustomerOrders tbody").append(noRecordsRow);
        } else {
            $("#CustomerOrders tbody").find(".norecordsfound").remove();
        }
    });

    $("#SearchPOdateTO").on("change", function () {
        var fromDate = $("#SearchPOdateFrom").val().split("/").reverse().join("-");
        var toDate = $("#SearchPOdateTO").val().split("/").reverse().join("-");
        var fromDateTimestamp = new Date(fromDate).getTime();
        var toDateTimestamp = new Date(toDate).getTime();

        if (fromDateTimestamp > toDateTimestamp) {
            alert("PO Date From Is Greater Than PO Date To");
            $("#SearchPOdateFrom").val('');
            $("#SearchPOdateTO").val('');
            return false;
        }
        $("#CustomerOrders tbody tr").filter(function () {
            var dateText = $(this.children[2]).text(); // assuming the date is in the 3rd column
            var tableDate = dateText.split("-").reverse().join("-");

            $(this).toggle(tableDate >= fromDate && tableDate <= toDate);
        });
        var $tableBody = $("#CustomerOrders tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $("#CustomerOrders tbody").append(noRecordsRow);
        } else {
            $("#CustomerOrders tbody").find(".norecordsfound").remove();
        }
    });
    
    //Search-BA-PONumber
    //Search-BA-Customer
    
    $('#PONoDetailsPopup').on('shown.bs.modal', () => {
        document.getElementById('new-order-entry').style.filter = 'blur(5px)'; // adjust the blur value as needed
        $("#LaunchDeliverySchedule").hide();
        //$("#SalesOrders").hide();
        //$("#TotalCountDiv").hide();
        var TotalQty = document.getElementById('TotalQty');
        TotalQty.style.border = '';
        var newNamevalidate = document.getElementById('POReqdDate');
        newNamevalidate.style.border = '';
        var POSoComment = document.getElementById('POSoComment');
        POSoComment.style.border = '';
        var PartNo = document.getElementById('PartNo');
        PartNo.style.border = '';
    });

    $('#PONoDetailsPopup').on('hidden.bs.modal', () => {
        document.getElementById('new-order-entry').style.filter = 'none';
    });
    loadBaStatus();
});
function loadBaStatus() {
    const selectElement = $('#Search-BA-Status');
    selectElement.html("");

    api.getbulk("/businessaquisition/GetBAAllStatus").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selectElement.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].status + "'>" + data[i].status + "</option>";
            selectElement.append(div_data);
        }
    });
}
function DeliverySechudeleLoad(element) {
    var relatedTarget = $(element);
    //document.getElementById('PONoDetailsPopup').style.filter = 'blur(5px)';
    editSalesOrder = false;
    document.getElementById("EditSOForm").reset();
    var customerOrderId = relatedTarget.data("customerorderid");
    var requiredbydatestr = relatedTarget.data("requiredbydatestr");
    var requiredquantity = relatedTarget.data("requiredquantity");
    var comment = relatedTarget.data("comment");
    var partId = relatedTarget.data("partid");
    var salesorder = relatedTarget.data("salesorder");
    var scheduleid = relatedTarget.data("salesorderid");
    document.getElementById('EditSORequiredByDate').value = requiredbydatestr.split("-").reverse().join("-");
    document.getElementById('POReqdDate').value = requiredbydatestr.split("-").reverse().join("-");
    $('#EditSORequiredQuantity').val(requiredquantity);
    $('#EditSOComment').val(comment);
    $('#EditSOPartId').val(partId);
    $('#EditSOScheduleId').val(scheduleid);
    $('#EditSOCustomerOrderId').val(customerOrderId);
    $("#TotalQty").val(requiredquantity);
    $('#POSoComment').val(comment);
    $("#LaunchDeliverySchedule").hide();
//    $("#POReqdDate").val();

    //console.log("EditSOScheduleId: " + scheduleid);
    //console.log("EditSOComment: " + comment);
    //console.log("EditSOCustomerOrderId: " + customerOrderId);
    //console.log("EditSOPartId: " + partId);
    if (salesorder == "Y") {
        editSalesOrder = true;
    }

}