$(document).ready(function () {
    $("#preloaderblurred").show();
    loadGroUploadSummary();
    $("#preloaderblurred").hide();

});

$('#groUploadModal').on('shown.bs.modal', function () {

    loadGroUploadSummary();

});
$('#groUploadModal').on('hidden.bs.modal', function () {

    // Clear selected file
    $('#groExcelFile').val('');

    // Clear displayed file name
    $('#selectedFileName').val('');
});
function displayFileName() {

    let fileInput =
        document.getElementById("groExcelFile");

    if (fileInput.files.length > 0) {

        document.getElementById("selectedFileName")
            .value =
            fileInput.files[0].name;
    }
}

function startGroUpload() {
    $("#preloaderblurred").show();
    let fileInput =
        document.getElementById("groExcelFile");

    if (fileInput.files.length === 0) {

        alert("Please Select Excel File");

        return;
    }

    let formData =  new FormData();

    formData.append("uploadedFile", fileInput.files[0]);
    
    $.ajax({

        url: "/Gro/UploadGroData",

        type: "POST",

        data: formData,

        processData: false,

        contentType: false,

        success: function (response) {

            if (response.success) {

                $("#preloaderblurred").hide();
                loadGroUploadSummary();

                alert("Upload Completed");
            }
            else {
                $("#preloaderblurred").hide();
                $("#ErrorPreview")
                    .html(response.message);

                $("#UploadErrroModal")
                    .modal('show');
            }
        }
    });
}
function loadGroUploadSummary() {

    $.get("/Gro/GetGroUploadSummary", function (response) {

        $("#LastUpdatedDate")
            .text(response.lastUpdatedDate);

        $("#IndentCount")
            .text(response.indentCount);

        $("#ProductCount")
            .text(response.productCount);
    });
}
$('#dispatchPopup').on('shown.bs.modal', function () {
    $("#preloaderblurred").show();
    loadDispatchSelection();
    $("#preloaderblurred").hide();
    $('#txtIndentSearch').on('keyup', applyFilters);

    $('#txtFromDate,#txtToDate').on('change', applyFilters);
    function applyFilters() {

        var indent = $('#txtIndentSearch').val().toLowerCase().trim();

        var from = $('#txtFromDate').val()
            ? new Date($('#txtFromDate').val())
            : null;

        var to = $('#txtToDate').val()
            ? new Date($('#txtToDate').val())
            : null;

        if (to)
            to.setHours(23, 59, 59, 999);

        $('#Dispatchselectiontable tbody tr').each(function () {

            var row = $(this);

            var rowIndent = row.find('td:eq(6)').text().toLowerCase().trim();
            var rowDate = parseDate(row.find('td:eq(5)').text().trim());

            var matchIndent = indent === "" || rowIndent.includes(indent);
            var matchFrom = !from || rowDate >= from;
            var matchTo = !to || rowDate <= to;

            row.toggle(matchIndent || ( matchFrom && matchTo));

        });
    }
    $('#btnClearFilter').on('click', function () {

        $('#txtIndentSearch').val('');
        $('#txtFromDate').val('');
        $('#txtToDate').val('');

        applyFilters();   // All rows become visible because no filters are applied
    });
    function parseDate(dateStr) {

        if (!dateStr)
            return null;

        var parts = dateStr.split('-'); // dd-MM-yyyy

        return new Date(parts[2], parts[1] - 1, parts[0]);
    }

        // Update Select All and button after filtering
         

    
    $(document).on('change',
        '#Dispatchselectiontable tbody input[type=checkbox]',
        function () {

            updateDispatchButton();

        });

    function updateDispatchButton() {

        var selectedIndentNos = [];

        $('#Dispatchselectiontable tbody input[type=checkbox]:checked').each(function () {

            // Change the td:eq(x) to the actual column index of Indent No
            var indentNo = $(this).closest('tr').find('td:eq(6)').text().trim();

            if ($.inArray(indentNo, selectedIndentNos) === -1) {
                selectedIndentNos.push(indentNo);
            }
        });

        // More than one distinct indent selected
        if (selectedIndentNos.length > 1) {

            alert("Select only one Unique Indent No.");

            $('#Dispatchselectiontable tbody input[type=checkbox]')
                .prop('checked', false);

            $('#updatedispatchqnty').prop('disabled', true);

            return;
        }

        // Enable button only if one or more rows are selected
        var checkedCount = $('#Dispatchselectiontable tbody input[type=checkbox]:checked').length;

        $('#updatedispatchqnty').prop('disabled', checkedCount === 0);
    }






});
function ShowDispatchDetails(ele) {

    var item = $(ele);

    $('#lblIndentDate').text(item.data('indentdate'));
    $('#lblIndentNo').text(item.data('indentno'));
    $('#lblCustomer').text(item.data('customer'));
    $('#lblAddress').text(item.data('address'));
    $('#lblContactperson').text(item.data('contact'));
    $('#lblContactpersonCellPhone').text(item.data('cellphone'));
    $('#lblExecutive').text(item.data('executive'));
    $('#lblProduct').text(item.data('partno'));

    var reqQty = parseInt(item.data('requiredqty')) || 0;
    var balQty = parseInt(item.data('balqty')) || 0;

    var dispatchedQty = reqQty - balQty;
    $('#lblReqQty').text(reqQty + ' Nos');
    $('#lblTotalDispatch').text(dispatchedQty + ' Nos');
    $('#lblBalance').text(balQty + ' Nos');

   

    //Dispatch Details not implemented yet
    $('#DispatchedDetailstable tbody').empty();

    $('#dispatchDetailsPopup').modal('show');

}
function loadDispatchSelection() {





    api.getbulk("/Gro/GetDispatchSelection").then((data) => {
        
        var tablebody = $("#Dispatchselectiontable tbody");
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
           
            //data[i].strStatus = WoOrdStatus[data[i].status];
            //data[i].woType = "";
            $(tablebody).append(AppUtil.ProcessTemplateData("dispatchSelectionRow", data[i]));
        }
       
    }).catch((error) => {
    });

}
$('#dispatchQtyModal').on('show.bs.modal', function () {

    var indentNo = "";

    $('#Dispatchselectiontable tbody input[type=checkbox]:checked').each(function () {

        indentNo = $(this).closest('tr').find('td:eq(6)').text().trim();

        return false; // first row is enough since all selected rows have same indent
    });

    loadDispatchData(indentNo);

});
function loadDispatchData(indentNo) {

    api.getbulk("/Gro/GetDispatchData?indentNo=" + encodeURIComponent(indentNo))
        .then((data) => {

            bindDispatchHeader(data.grodispheaderbyindent, data.result);

            bindDispatchGrid(data.result);

        })
        .catch((error) => {

            console.log(error);

           // AppUtil.MessageBox("Unable to load dispatch details.", 2);

        });

}
function bindDispatchHeader(header, result) {

    const firstRow = result.length > 0 ? result[0] : null;

    const date = new Date(header.sentDate);

    $("#IndentDate").text(
        String(date.getDate()).padStart(2, '0') + "-" +
        String(date.getMonth() + 1).padStart(2, '0') + "-" +
        date.getFullYear()
    );
    $("#hdnGroDispHeaderId").val(header.gro_Disp_HeaderId);
    $("#hdnIndentId").val(header.indent);
    $("#IndentNo").text(header.indent);

    $("#Customer").text(firstRow ? firstRow.company_Name : "");

    $("#Address").text(
        header.shipping_Address + ", " +
        header.shipping_City + " - " +
        header.shipping_PINCODE
    );

    $("#Contact").text(firstRow ? firstRow.contact_Person : "");

    $("#Mobile").text(firstRow ? firstRow.contact_Person_No : "");
    $("#Executive").text(firstRow ? firstRow.excutive_Name : "");
}
function bindDispatchGrid(data) {

    var tablebody = $("#Dispheadertable tbody");

    tablebody.html("");

    if (data.length === 0) {

        tablebody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < data.length; i++) {

        tablebody.append(
            AppUtil.ProcessTemplateData("dispatchQtyRow", data[i])
        );
    }

}
function loadDispatchAddress() {

    var headerId = $("#hdnGroDispHeaderId").val();
    var indentId = $("#hdnIndentId").val();

    api.getbulk("/Gro/GetDispatchAddress?headerId=" + encodeURIComponent(headerId)
        + "&indentId=" + encodeURIComponent(indentId))
        .then((data) => {

            // Bind here once controller returns data

            var header = data.header;
            var groData = data.result;
            $("#HeaderId").val(header.gro_Disp_HeaderId);
            $("#addrIndentDate").text(groData.indentDateStr);
            $("#addrIndentNo").text(groData.indent);
            $("#addrContact").text(groData.contact_Person);
            $("#addrCellPhone").text(groData.contact_Person_No);
            $("#addrExecutiveName").text(groData.excutive_Name);

            $("#txtCustomer").val(groData.company_Name);
            $("#txtAddress").val(header.shipping_Address);
            $("#txtCity").val(header.shipping_City);
            $("#txtPin").val(header.shipping_PINCODE);

        })
        .catch((error) => {

            console.log(error);

            // AppUtil.MessageBox("Unable to load dispatch address.", 2);

        });

}
$("#btnSaveDispatchAddress").click(function () {

    var model = {
        gro_Disp_HeaderId: $("#HeaderId").val(),
        shipping_Address: $("#txtAddress").val(),
        shipping_City: $("#txtCity").val(),
        shipping_Pincode: $("#txtPin").val()
    };
    var indentId = $("#hdnIndentId").val();
    api.post("/Gro/UpdateDispatchAddress", model)
        .then(function (response) {

            //AppUtil.MessageBox("Dispatch address updated successfully.", 1);

            $("#editDispatchAddressModal").modal("hide");
            loadDispatchData(indentId);

        })
        .catch(function (error) {

            console.log(error);

            //AppUtil.MessageBox("Unable to update dispatch address.", 2);

        });

});
$("#dispatchDetailsDataUpdatePopup").on("shown.bs.modal", function () {
    loadDispatchDetailsDataUpdate();

    $('#searchbyindent').on('keyup', function () {
        filterbyIndent();
    });
    function filterbyIndent() {

        var indent = $('#searchbyindent').val().toLowerCase().trim();

        $('#dispatchdataupdatetable tbody tr').each(function () {

            var row = $(this);

            var rowIndent = row.find('td:eq(3)').text().toLowerCase().trim();

            row.toggle(indent === "" || rowIndent.includes(indent));

        });

    }


});
function loadDispatchDetailsDataUpdate() {

    api.getbulk("/Gro/GetDispatchDetailsDataUpdate")
        .then((data) => {

            var tablebody = $("#dispatchdataupdatetable tbody");
            tablebody.html("");

            if (data.length === 0) {

                tablebody.append(`
                    <tr class="norecordsfound">
                        <td colspan="5" class="text-center text-muted">
                            <strong>No Records Found</strong>
                        </td>
                    </tr>`);

                return;
            }

            for (var i = 0; i < data.length; i++) {

                tablebody.append(
                    AppUtil.ProcessTemplateData("dispatchUpdateRow", data[i])
                );

            }

        })
        .catch((error) => {

            console.log(error);

        });

}



function ShowDetails(ele) {

    let tr = $(ele).closest("tr");

    let indent = tr.find("td:eq(3)").text().trim();

    let headerId = tr.find("td:eq(1)").text().trim();

    LoadDispatchDetails(indent, headerId, "SHOW");

}
function UpdateDispheader(ele) {

    let tr = $(ele).closest("tr");

    let indent = tr.find("td:eq(3)").text().trim();

    let headerId = tr.find("td:eq(1)").text().trim();

    LoadDispatchDetails(indent, headerId, "UPDATE");

}
function LoadDispatchDetails(indent, headerId, mode) {

    $.ajax({

        url: '/Gro/GetDispatchDetails',

        type: 'POST',

        data: {
            indent: indent,
            groDispHeaderId: headerId
        },

        success: function (response) {

            BindDispatchPopup(response, mode);

            $("#dispatchdataupdatebyindentmodal").modal("show");
        }

    });

}

function BindDispatchPopup(data, mode) {

    var header = data.header;
    var result = data.result;

    $("#updIndentDate").text(header.indentDateStr);

    $("#updIndentNo").text(header.indent);

    $("#updCustomer").text(header.company_Name);

    $("#updAddress").text(header.shipping_Address + ", " +
        header.shipping_City + " - " +
        header.shipping_PINCODE);

    $("#updContact").text(header.contact_Person);

    $("#updMobile").text(header.contact_Person_No);

    // Dispatch Details

    $("#ddlCourier").val(header.courier_Partner);

    $("#txtAwbReference").val(header.awb);

    $("#txtDispatchDate").val(header.dispatch_Date);
    $("#txtDispatchDate").attr(
        "max",
        new Date().toISOString().split('T')[0]
    );
    // Grid

    BindDispatchLines(result);

    // Show/Hide Update Section

    if (mode === "SHOW") {

        $("#dispatchUpdateSection").hide();

    }
    else {
        $("#updateHeaderId").val(header.gro_Disp_HeaderId);
        $("#dispatchUpdateSection").show();
        api.getbulk("/Gro/GetCouriers")
            .then((data) => {
                console.log(data);
                let ddl = $("#ddlCourier");

                ddl.empty();

              //  ddl.append('<option value="">Select Courier</option>');

                $.each(data, function (i, item) {

                    ddl.append(
                        $('<option></option>')
                            .val(item.courier_List_ID)
                            .text(item.courier_Name)
                    );

                });

            })
            .catch((error) => {

                console.log(error);

            });

    }

}
function BindDispatchLines(lines) {



    var tablebody = $("#dispatchupdateline tbody");

    tablebody.html("");

    if (lines.length === 0) {

        tablebody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < lines.length; i++) {

        tablebody.append(
            AppUtil.ProcessTemplateData("dispatchupdatelinerow", lines[i])
        );
    }
  
}
$("#btnUpdatedispatchdate").click(function () {

    var headerId = $("#updateHeaderId").val();

    var courierId = $("#ddlCourier").val();

    var awb = $("#txtAwbReference").val().trim();

    var dispatchDate = $("#txtDispatchDate").val();
    var model = {

        gro_Disp_HeaderId: $("#updateHeaderId").val(),

        awb: $("#txtAwbReference").val().trim(),
        courier_Partner: courierId,
        dispatch_Date: dispatchDate


    };
    api.post("/Gro/CheckAwbUnique", model)
        .then((result) => {

            if (!result.success) {

                alert("AWB No already exists. Please check and re-enter.");

                return;
            }
            api.post("/Gro/UpdateDispatchDetailByheader", model)
                .then((result) => {


                    $("#dispatchdataupdatebyindentmodal").modal("hide");
                    // Unique - proceed with Update API

                })
                .catch((err) => {

                    console.log(err);

                });

        })
        .catch((err) => {

            console.log(err);

        });

       

    // Call update API
});
$("#deliverydataupdatemodal").on("show.bs.modal", function () {

    LoadDeliveryPendingList();
    $("#txtDeliveryIndentSearch").on("keyup", FilterDeliveryGrid);

    $("#txtDeliveryCourierSearch").on("keyup", FilterDeliveryGrid);

    $("#txtDeliveryFromDate").on("change", FilterDeliveryGrid);

    $("#txtDeliveryToDate").on("change", FilterDeliveryGrid);
    $("#btnClearDeliveryFilter").click(function () {

        $("#txtDeliveryIndentSearch").val("");
        $("#txtDeliveryCourierSearch").val("");
        $("#txtDeliveryFromDate").val("");
        $("#txtDeliveryToDate").val("");

        FilterDeliveryGrid();

    });
    function FilterDeliveryGrid() {

        var indent = $("#txtDeliveryIndentSearch").val().toLowerCase().trim();
        var courier = $("#txtDeliveryCourierSearch").val().toLowerCase().trim();

        var fromDate = $("#txtDeliveryFromDate").val();
        var toDate = $("#txtDeliveryToDate").val();

        $("#deliverydatatable tbody tr").each(function () {

            var row = $(this);

            var dispatchDate = row.find("td:eq(1)").text().trim(); // dd-MM-yyyy
            var rowIndent = row.find("td:eq(2)").text().toLowerCase().trim();
            var rowCustomer = row.find("td:eq(3)").text().toLowerCase().trim();
            var rowCourier = row.find("td:eq(4)").text().toLowerCase().trim();

            var show = true;

            // Indent filter
            if (indent !== "" && rowIndent.indexOf(indent) === -1) {
                show = false;
            }

            // Courier filter
            if (courier !== "" && rowCourier.indexOf(courier) === -1) {
                show = false;
            }

            // Date filter
            if (show && (fromDate !== "" || toDate !== "")) {

                var parts = dispatchDate.split("-");
                var rowDate = new Date(parts[2], parts[1] - 1, parts[0]);

                if (fromDate !== "") {

                    var from = new Date(fromDate);

                    if (rowDate < from)
                        show = false;
                }

                if (toDate !== "") {

                    var to = new Date(toDate);

                    if (rowDate > to)
                        show = false;
                }
            }

            row.toggle(show);

        });

    }
});
function LoadDeliveryPendingList() {
    api.getbulk("/Gro/GetPendingDeliveryDetails")
        .then((data) => {

            let tbody = $("#deliverydatatable tbody");

            tbody.empty();
            tbody.html("");

            if (data.length === 0) {

                tbody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

                return;
            }

            for (var i = 0; i < data.length; i++) {

                tbody.append(
                    AppUtil.ProcessTemplateData("deliverydetailrow", data[i])
                );
            }


        })
        .catch((err) => {

            console.log(err);

        });
}
function LoadDeliveryDetails(ele, mode) {

    var tr = $(ele).closest("tr");

    var headerId = tr.find("td:eq(0)").text().trim();

    $.ajax({

        url: "/Gro/GetDeliveryDetails",

        type: "POST",

        data: {

            groDispHeaderId: headerId

        },

        success: function (response) {
            console.log(response);
            BindDeliveryPopup(response, mode);

            $("#deliverydataupdatebyindent").modal("show");

        }

    });

}
function BindDeliveryPopup(data, mode) {

    var header = data.header;

    $("#deliveryUpdateHeaderId").val(header.gro_Disp_HeaderId);

    $("#delIndentDate").text(header.indentDateStr);
    $("#delIndentNo").text(header.indent);
    $("#delCustomer").text(header.company_Name);
    $("#delAddress").text(header.shipping_Address + ", " +
        header.shipping_City + " - " +
        header.shipping_PINCODE);
    $("#delContact").text(header.contact_Person);
    $("#delMobile").text(header.contact_Person_No);

    $("#DispatchDate").text(header.dispatchDateStr);
    $("#courier").text(header.courier);
    $("#awbref").text(header.awb);

    BindDeliveryGrid(data.result);

    if (mode == "view") {

        $("#deliveryUpdateSection").hide();

        $("#deliverydataupdatebyindentLabel")
            .text("Gro - Delivery Details");

    }
    else {

        $("#deliveryUpdateSection").show();

        $("#deliverydataupdatebyindentLabel")
            .text("Gro - Delivery Data Update By Indent");

        // Default to today for update
        $("#txtActualDeliveryDate").val(new Date().toISOString().split('T')[0]);
        $("#txtActualDeliveryDate").attr(
            "max",
            new Date().toISOString().split('T')[0]
        );
        // Or if you prefer existing value when editing:
        // $("#txtActualDeliveryDate").val(header.delivery_Date_Str);

    }

}
function BindDeliveryGrid(lines) {



    var tablebody = $("#DeliveryDataUpdateGrid tbody");

    tablebody.html("");

    if (lines.length === 0) {

        tablebody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < lines.length; i++) {

        tablebody.append(
            AppUtil.ProcessTemplateData("deliveryupdatelinerow", lines[i])
        );
    }

}
$("#btnUpdateDeliveryData").click(function () {

    var headerId = $("#deliveryUpdateHeaderId").val();
    var deliveryDate = $("#txtActualDeliveryDate").val();

    if (!deliveryDate) {
        alert("Please select Delivery Date.");
        $("#txtActualDeliveryDate").focus();
        return;
    }

    var model = {
        gro_Disp_HeaderId: parseInt(headerId),
        delivered_Date: deliveryDate
    };

    $.ajax({

        url: "/Gro/UpdateDeliveryDateDispHeader",

        type: "POST",

        data: model,

        success: function (response) {

            if (response.success) {

                alert("Delivery date updated successfully.");

                $("#deliverydataupdatebyindent").modal("hide");

                // Reload the delivery list if required
                LoadDeliveryPendingList();
            }
            else {

                alert(response.message);
            }

        },

        error: function () {

            alert("An error occurred while updating delivery date.");
        }

    });

});

$("#deliveryCompletePopup").on("show.bs.modal", function () {

    LoadDeliveryCompleteData();
    // Indent typing
    $("#txtSearchIndent").on("keyup", function () {
        FilterDeliveryCompleteGrid();
    });

    // Date category change
    $("#ddlDateCategory").on("change", function () {
        FilterDeliveryCompleteGrid();
    });

    // Date changes
    $("#txtdelFromDate, #txtdelToDate").on("change", function () {
        FilterDeliveryCompleteGrid();
    });
    $("#btnClearDeliveryComplete").click(function () {

        $("#txtSearchIndent").val("");
        $("#ddlDateCategory").val("");
        $("#txtdelFromDate").val("");
        $("#txtdelToDate").val("");

        // Show all rows
        $("#deliverycompletetable tbody tr").show();

    });
    function parseDate(dateStr) {

        if (!dateStr)
            return null;

        var parts = dateStr.split('-'); // dd-MM-yyyy

        return new Date(parts[2], parts[1] - 1, parts[0]);
    }
    function FilterDeliveryCompleteGrid() {

        var indent = $("#txtSearchIndent").val().toLowerCase().trim();
        var dateCategory = $("#ddlDateCategory").val();
        var fromDate = $("#txtdelFromDate").val();
        var toDate = $("#txtdelToDate").val();

        var from = fromDate ? new Date(fromDate) : null;
        var to = toDate ? new Date(toDate) : null;

        $("#deliverycompletetable tbody tr").each(function () {

            var row = $(this);

            var rowIndent = row.find("td:eq(1)").text().toLowerCase().trim();

            var indentDate = row.find("td:eq(0)").text().trim();
            var dispatchDate = row.find("td:eq(7)").text().trim();
            var deliveryDate = row.find("td:eq(8)").text().trim(); // hidden column

            var selectedDate = "";

            if (dateCategory === "Indent")
                selectedDate = indentDate;
            else if (dateCategory === "Dispatch")
                selectedDate = dispatchDate;
            else if (dateCategory === "Delivery")
                selectedDate = deliveryDate;

            var show = true;

            // Indent filter
            if (indent !== "" && !rowIndent.includes(indent))
                show = false;

            // Date filter
            if (selectedDate !== "" && (from || to)) {

                var rowDate = parseDate(selectedDate);

                if (from && rowDate < from)
                    show = false;

                if (to && rowDate > to)
                    show = false;
            }

            row.toggle(show);

        });

    }
});
function LoadDeliveryCompleteData() {

    api.getbulk("/Gro/GetDeliveryCompleteData")
        .then((data) => {

            var tbody = $("#deliverycompletetable tbody");

             

            tbody.empty();
            tbody.html("");

            if (data.length === 0) {

                tbody.append(`
            <tr>
                <td colspan="8" class="text-center">
                    No Records Found
                </td>
            </tr>`);

                return;
            }

            for (var i = 0; i < data.length; i++) {

                tbody.append(
                    AppUtil.ProcessTemplateData("deliverycompleterow", data[i])
                );
            }

        })
        .catch((error) => {

            console.log(error);

        });

}