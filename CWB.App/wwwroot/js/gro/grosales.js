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
    $('#txtIndentSearch').on('keyup', function () {
        filterIndent();
    });

    $('#txtFromDate,#txtToDate').on('change', function () {
        filterDispatchTable();
    });

    function filterDispatchTable() {

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

            var rowDate = parseDate(row.find('td:eq(5)').text().trim());

            var matchFrom = !from || rowDate >= from;
            var matchTo = !to || rowDate <= to;

            row.toggle(matchFrom && matchTo);

        });

    }
    function filterIndent() {

        var indent = $('#txtIndentSearch').val().toLowerCase().trim();

        $('#Dispatchselectiontable tbody tr').each(function () {

            var row = $(this);

            var rowIndent = row.find('td:eq(6)').text().toLowerCase().trim();

            row.toggle(indent === "" || rowIndent.includes(indent));

        });

    }
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