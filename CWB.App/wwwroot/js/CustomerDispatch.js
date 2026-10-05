let selectedDispatchItems = [];
let selectedCustomer = null;

$(document).ready(function () {
    loadcustomerdispatch();
    LoadCustDesc("SearchCustomer");


    $("#SearchPO").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#Dispatchgrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#Dispatchgrid tbody");
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
    $("#SearchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#Dispatchgrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#Dispatchgrid tbody");
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
    $("#SearchCustomer").on("change", function () {
        var selectedValue = $(this).find('option:selected').text();
        if (selectedValue == "--Select--") {
            $("#Dispatchgrid tbody tr").show();
            var $tableBody = $("#Dispatchgrid tbody");
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
            $("#Dispatchgrid tbody tr").filter(function () {
                $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(selvallow) > -1)
            });
            var $tableBody = $("#Dispatchgrid tbody");
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
});


function loadcustomerdispatch() {
    $("#preloaderblurred").show();
    selectedDispatchItems = [];
    selectedCustomer = null;

    $("#InvoiceEntry").prop("disabled", true);
    $('#SelectAllMGrid').prop("checked", false);
    api.getbulk("/WorkOrder/CustomerDispatchData").then((data) => {
       
       
        var tablebody = $("#Dispatchgrid tbody");
        $(tablebody).html("");
        if (data.length === 0) {
            
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        console.log(data);
        for (i = 0; i < data.length; i++) {
            

            $(tablebody).append(AppUtil.ProcessTemplateData("DispatchgridRow", data[i]));
        }
        
        disableInvoiceEnteredCheckboxes();
        loadDispatchActionMenus();

        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function disableInvoiceEnteredCheckboxes() {

    $(".DispatchgridChks").each(function () {

        const checkbox = $(this);

        const invoiceEntered = $.trim(
            checkbox.data("invoice-entered")
        );

        const dataConfirmed = $.trim(
            checkbox.data("data-confirmed")
        );

        // Invoice already entered OR data confirmed
        if (invoiceEntered == 'Y' && dataConfirmed == 'N') {

            checkbox.prop("disabled", true);

        }
        else {

            checkbox.prop("disabled", false);

        }

    });

}
function loadDispatchActionMenus() {

    $(".DispatchgridChks").each(function () {

        const checkbox = $(this);

        const invoiceEntered = $.trim(
            checkbox.data("invoice-entered")
        );

        const dataConfirmed = $.trim(
            checkbox.data("data-confirmed")
        );

        const actionMenu = checkbox
            .closest("tr")
            .find(".DispatchActionMenu");

        actionMenu.html("");

        // Only allow Edit/Delete when Invoice Entered Y
        // and Data Confirmed N
        if (invoiceEntered === "Y" && dataConfirmed === "N") {

            const custDispListId = checkbox.data("custdispid");

            actionMenu.append(`

                <a class="dropdown-item EditInvoiceDetails"
                   href="javascript:void(0);"
                   data-custdispid="${custDispListId}">

                     
                    Edit Invoice Details

                </a>

                <a class="dropdown-item DeleteInvoiceDetails"
                   href="javascript:void(0);"
                   data-custdispid="${custDispListId}">

                   
                    Delete Invoice Details

                </a>

            `);

        }

    });

}
function LoadCustDesc(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    if (!compSelect.length)
        return;
    compSelect.empty();
    ////debugger;
    var div_data = "<option value=''>--Select--</option>";
    compSelect.append(div_data);
    api.get("/masters/customers").then((data) => {
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].companyId + "'>" +
                data[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}

$(document).on("change", ".DispatchgridChks", function () {

    const checkbox = $(this);
    if (checkbox.prop("disabled")) {
        checkbox.prop("checked", false);
        return;
    }
    const customer = $.trim(checkbox.data("customer"));

    if (checkbox.is(":checked")) {

        if (selectedCustomer !== null &&
            selectedCustomer !== customer) {

            alert("Please select items belonging to the same customer only.");

            checkbox.prop("checked", false);

            return;
        }

        if (selectedCustomer === null) {
            selectedCustomer = customer;
        }

    }

    updateInvoiceEntryButton();

});
function updateInvoiceEntryButton() {

    selectedDispatchItems = [];

    $(".DispatchgridChks:checked").each(function () {

        const item = $(this);

        selectedDispatchItems.push({

            customer: $.trim(item.data("customer")),
            poNumber: item.data("po"),
            partNo: item.data("partno"),
            partDesc: item.data("partdesc"),
            disp_Qnty: item.data("dispqnty"),
            soId: item.data("soid"),
            cust_Disp_List_ID: item.data("custdispid"),
            

        });

    });

    if (selectedDispatchItems.length === 0) {

        selectedCustomer = null;

        $("#InvoiceEntry")
            .prop("disabled", true)
            .text("Enter Invoice Details for Selected Items");

        return;
    }

    selectedCustomer = selectedDispatchItems[0].customer;

    $("#InvoiceEntry")
        .prop("disabled", false)
        .text(
            "Enter Invoice Details for Selected Items (" +
            selectedDispatchItems.length +
            ")"
        );
}
$(document).on("click", "#InvoiceEntry", function () {

    if (selectedDispatchItems.length === 0) {
        return;
    }

    // Set Customer
    $("#InvoiceCustomer").text(selectedCustomer);

    // Set count
    $("#InvoicePartCount").text(selectedDispatchItems.length);

    // Clear previous fields
    $("#InvoiceNo").val("");
    $("#InvoiceDate").val("");
    $("#DispatchDetails").val("");

    // Clear error
    $("#InvoiceErrorMessage")
        .addClass("d-none")
        .text("");

    // Disable Save initially
    $("#InvoiceSave").prop("disabled", true);

    // Clear selected grid
    $("#InvoiceSelectedGrid tbody").html("");

    // Load selected items
    selectedDispatchItems.forEach(function (item) {

        $("#InvoiceSelectedGrid tbody").append(

            AppUtil.ProcessTemplateData(
                "InvoiceSelectedGridRow",
                item
            )

        );

    });
    $("#InvoiceDetailsModal").modal("show");
    // Open Bootstrap 5 Modal
    

});
function validateInvoiceDetails() {

    const invoiceNo = $.trim($("#InvoiceNo").val());
    const invoiceDate = $("#InvoiceDate").val();

    // Clear previous error
    $("#InvoiceErrorMessage")
        .addClass("d-none")
        .text("");

    $("#InvoiceSave").prop("disabled", true);

    // Check Invoice No
    if (invoiceNo === "") {

        $("#InvoiceErrorMessage")
            .removeClass("d-none")
            .text("Please enter Invoice No.");

        return;
    }

    // Check Invoice Date
    if (invoiceDate === "") {

        $("#InvoiceErrorMessage")
            .removeClass("d-none")
            .text("Please select Invoice Date.");

        return;
    }

    // Both mandatory fields filled
    // Now check uniqueness
    checkInvoiceNoExists(invoiceNo);
}
function checkInvoiceNoExists(invoiceNo) {

    $("#InvoiceSave").prop("disabled", true);

    api.getbulk(
        "/WorkOrder/CheckInvoiceNo?invoiceNo=" +
        encodeURIComponent(invoiceNo)
    )
        .then(function (response) {

            console.log("Invoice Check Response:", response);

            if (response.exists === true) {

                $("#InvoiceErrorMessage")
                    .removeClass("d-none")
                    .text("Invoice No already exists. Please enter a unique Invoice No.");

                $("#InvoiceSave").prop("disabled", true);

                return;
            }

            // Invoice is unique
            $("#InvoiceErrorMessage")
                .addClass("d-none")
                .text("");

            $("#InvoiceSave").prop("disabled", false);

        })
        .catch(function (error) {

            console.error("Invoice validation error:", error);

            $("#InvoiceErrorMessage")
                .removeClass("d-none")
                .text("Unable to validate Invoice No. Please try again.");

            $("#InvoiceSave").prop("disabled", true);

        });
}
$(document).on("input change", "#InvoiceNo, #InvoiceDate", function () {

    validateInvoiceDetails();

});
$(document).on("click", "#InvoiceSave", async function () {

    const invoiceNo = $.trim($("#InvoiceNo").val());

    $("#InvoiceSave").prop("disabled", true);

    try {

        const checkResponse = await api.getbulk(
            "/WorkOrder/CheckInvoiceNo?invoiceNo=" +
            encodeURIComponent(invoiceNo)
        );

        if (checkResponse.exists === true) {

            $("#InvoiceErrorMessage")
                .removeClass("d-none")
                .text("Invoice No already exists. Please enter a unique Invoice No.");

            $("#InvoiceSave").prop("disabled", false);

            return;
        }

        await saveInvoiceDetails();

    }
    catch (error) {

        console.error(error);

        $("#InvoiceErrorMessage")
            .removeClass("d-none")
            .text("Unable to validate Invoice No.");

        $("#InvoiceSave").prop("disabled", false);

    }

});


// ==========================================
// Actual Save
// ==========================================

async function saveInvoiceDetails() {

    const invoiceNo = $.trim($("#InvoiceNo").val());
    const invoiceDate = $("#InvoiceDate").val();
    const dispatchDetails = $.trim($("#DispatchDetails").val());

    const requestData = {

        InvoiceNo: invoiceNo,

        InvoiceDate: invoiceDate,

        DispatchDetails: dispatchDetails,

        Customer: selectedCustomer,

        NoOfParts: selectedDispatchItems.length,

        CustDispListIds: selectedDispatchItems.map(function (item) {

            return item.cust_Disp_List_ID;

        })

    };

    console.log("Saving:", requestData);

    $("#preloaderblurred").show();

    try {

        const response = await api.post(
            "/WorkOrder/SaveCustomerDispatchInvoice",
            requestData
        );

        $("#preloaderblurred").hide();

        alert("Invoice details saved successfully.");

        
        $("#InvoiceDetailsModal").modal("hide");
        loadcustomerdispatch();

    }
    catch (error) {

        $("#preloaderblurred").hide();

        console.error(error);

        $("#InvoiceErrorMessage")
            .removeClass("d-none")
            .text("Error while saving Invoice Details.");

        $("#InvoiceSave").prop("disabled", false);

    }

}
$(document).on("click", ".EditInvoiceDetails", function () {

    const custDispListId = $(this).data("custdispid");

    openEditInvoicePopup(custDispListId);

});
async function openEditInvoicePopup(custDispListId) {

    console.log("Opening Edit Popup for:", custDispListId);

    $("#preloaderblurred").show();

    try {

        const data = await api.getbulk(
            "/WorkOrder/GetEditCustomerDispatchInvoice?custDispListId=" +
            encodeURIComponent(custDispListId)
        );

        console.log("Edit Invoice Data:", data);


        // ==========================================
        // 1. Bind Hidden Dispatch Details ID
        // ==========================================

        $("#EditDispatchDetailsId").val(data.dispatchDetailsId);


        // ==========================================
        // 2. Bind Customer
        // ==========================================

        $("#EditInvoiceCustomer").text(data.customer);


        // ==========================================
        // 3. Bind Number of Parts
        // ==========================================

        $("#EditInvoicePartCount").text(data.noOfParts);


        // ==========================================
        // 4. Bind Existing Invoice Details
        // ==========================================

        $("#EditInvoiceNo").val(data.invoiceNo);


        // Date Formatting
        if (data.invoiceDate) {

            const formattedDate = data.invoiceDate.split("T")[0];

            $("#EditInvoiceDate")
                .val(formattedDate);

        }


        $("#EditDispatchDetails")
            .val(data.dispatchDetails);


        // ==========================================
        // 5. Load Linked Line Items
        // ==========================================

        $("#EditInvoiceSelectedGrid tbody")
            .html("");

        data.selectedItems.forEach(function (item) {

            $("#EditInvoiceSelectedGrid tbody").append(

                AppUtil.ProcessTemplateData(
                    "EditInvoiceSelectedGridRow",
                    item
                )

            );

        });


        // ==========================================
        // 6. Initially Enable Update
        // ==========================================

        $("#EditInvoiceSave")
            .prop("disabled", false);


        $("#EditInvoiceErrorMessage")
            .addClass("d-none")
            .text("");


        // ==========================================
        // 7. Open Modal
        // ==========================================

        
        $("#EditInvoiceDetailsModal").modal("show");
        


    }
    catch (error) {

        console.error("Error loading Edit Invoice:", error);

        alert("Unable to load Invoice Details.");

    }
    finally {

        $("#preloaderblurred").hide();

    }

}
$(document).on("click", "#EditInvoiceSave", async function () {

    const button = $(this);

    const dispatchDetailsId =
        $("#EditDispatchDetailsId").val();

    const invoiceNo =
        $.trim($("#EditInvoiceNo").val());

    const invoiceDate =
        $("#EditInvoiceDate").val();

    const dispatchDetails =
        $.trim($("#EditDispatchDetails").val());


    // ==========================================
    // Clear Previous Error
    // ==========================================

    $("#EditInvoiceErrorMessage")
        .addClass("d-none")
        .text("");


    // ==========================================
    // Basic Validation
    // ==========================================

    if (!invoiceNo) {

        showEditInvoiceError("Invoice No is required.");

        return;
    }

    if (!invoiceDate) {

        showEditInvoiceError("Invoice Date is required.");

        return;
    }


    // ==========================================
    // Disable Button During Processing
    // ==========================================

    button.prop("disabled", true);

    $("#preloaderblurred").show();

    try {

        // ==========================================
        // STEP 1: Check Invoice No Uniqueness
        // ==========================================

        const checkUrl =
            "/WorkOrder/CheckEditInvoiceNo"
            + "?invoiceNo=" + encodeURIComponent(invoiceNo)
            + "&dispatchDetailsId=" + encodeURIComponent(dispatchDetailsId);

        const checkResult =
            await api.getbulk(checkUrl);

        if (checkResult.exists === true) {

            showEditInvoiceError(
                "Invoice No already exists."
            );

            button.prop("disabled", false);

            return;
        }


        // ==========================================
        // STEP 2: Prepare DispatchDetailsVM
        // ==========================================

        const model = {

            DispatchDetailsId: parseInt(dispatchDetailsId),

            InvoiceNo: invoiceNo,

            InvoiceDate: invoiceDate,

            DispatchDetail: dispatchDetails

        };


        // ==========================================
        // STEP 3: Update API
        // ==========================================

        const result =
            await api.post(
                "/WorkOrder/UpdateCustomerDispatchInvoice",
                model
            );


        if (result.success) {

            alert("Invoice details updated successfully.");
            $("#EditInvoiceDetailsModal").modal("hide");
            

            // Reload Grid
            loadcustomerdispatch();

        }
        else {

            showEditInvoiceError(
                result.message || "Unable to update Invoice Details."
            );

        }

    }
    catch (error) {

        console.error("Edit Invoice Save Error:", error);

       

    }
    finally {

        $("#preloaderblurred").hide();

        button.prop("disabled", false);

    }

});
function showEditInvoiceError(message) {

    $("#EditInvoiceErrorMessage")
        .removeClass("d-none")
        .text(message);

}
$(document).on("click", ".DeleteInvoiceDetails", function () {

    const custDispListId = $(this).data("custdispid");

    openDeleteInvoicePopup(custDispListId);

});
async function openDeleteInvoicePopup(custDispListId) {

    $("#preloaderblurred").show();

    try {

        const data = await api.getbulk(
            "/WorkOrder/GetEditCustomerDispatchInvoice?custDispListId="
            + encodeURIComponent(custDispListId)
        );

        console.log("Delete Invoice Data:", data);


        // ==========================================
        // 1. Bind Hidden Dispatch Details ID
        // ==========================================

        $("#DeleteDispatchDetailsId")
            .val(data.dispatchDetailsId);


        // ==========================================
        // 2. Bind Customer
        // ==========================================

        $("#DeleteInvoiceCustomer")
            .text(data.customer);


        // ==========================================
        // 3. Bind Number of Parts
        // ==========================================

        $("#DeleteInvoicePartCount")
            .text(data.noOfParts);


        // ==========================================
        // 4. Bind Invoice Details
        // ==========================================

        $("#DeleteInvoiceNo")
            .val(data.invoiceNo || "");


        // Avoid timezone conversion
        if (data.invoiceDate) {

            const formattedDate =
                String(data.invoiceDate).split("T")[0];

            $("#DeleteInvoiceDate")
                .val(formattedDate);

        }


        $("#DeleteDispatchDetails")
            .val(data.dispatchDetails || "");


        // ==========================================
        // 5. Load Linked Line Items
        // ==========================================

        $("#DeleteInvoiceSelectedGrid tbody")
            .html("");


        if (data.selectedItems && data.selectedItems.length > 0) {

            data.selectedItems.forEach(function (item) {

                $("#DeleteInvoiceSelectedGrid tbody").append(

                    AppUtil.ProcessTemplateData(
                        "DeleteInvoiceSelectedGridRow",
                        item
                    )

                );

            });

        }


        // ==========================================
        // 6. Clear Error Message
        // ==========================================

        $("#DeleteInvoiceErrorMessage")
            .addClass("d-none")
            .text("");


        // ==========================================
        // 7. Open Delete Modal
        // ==========================================
        $("#DeleteInvoiceDetailsModal").modal("show");
      

    }
    catch (error) {

        console.error("Error loading Delete Invoice:", error);

        alert("Unable to load Invoice Details.");

    }
    finally {

        $("#preloaderblurred").hide();

    }

}
$(document).on("click", "#ConfirmDeleteInvoice", async function () {

    const button = $(this);

    const dispatchDetailsId =
        $("#DeleteDispatchDetailsId").val();


    // ==========================================
    // Validate Hidden ID
    // ==========================================

    if (!dispatchDetailsId || dispatchDetailsId <= 0) {

        showDeleteInvoiceError(
            "Invalid Dispatch Details ID."
        );

        return;
    }


    // ==========================================
    // Confirm User Action
    // ==========================================

    const confirmDelete = confirm(
        "Are you sure you want to delete these Invoice Details?"
    );

    if (!confirmDelete) {
        return;
    }


    // ==========================================
    // Disable Button
    // ==========================================

    button.prop("disabled", true);

    $("#preloaderblurred").show();


    try {

        // ==========================================
        // Send Dispatch Details ID to Controller
        // ==========================================

        const model = {
            DispatchDetailsId: parseInt(dispatchDetailsId)
        };


        const result = await api.post(
            "/WorkOrder/DeleteCustomerDispatchInvoice",
            model
        );


        if (result.success) {

            alert("Invoice details deleted successfully.");


            $("#DeleteInvoiceDetailsModal").modal("hide");


            // Reload Grid

            loadcustomerdispatch();

        }
        else {

            showDeleteInvoiceError(
                result.message ||
                "Unable to delete Invoice Details."
            );

        }

    }
    catch (error) {

        console.error("Delete Invoice Error:", error);

        showDeleteInvoiceError(
            "An error occurred while deleting Invoice Details."
        );

    }
    finally {

        $("#preloaderblurred").hide();

        button.prop("disabled", false);

    }

});
function showDeleteInvoiceError(message) {

    $("#DeleteInvoiceErrorMessage")
        .removeClass("d-none")
        .text(message);

}
$(document).on("click", "#ConfirmEnteredData", function () {

    confirmEnteredCustomerDispatchData();

});
async function confirmEnteredCustomerDispatchData() {

    const confirmResult = confirm(
        "Are you sure you want to confirm all entered dispatch data?"
    );

    if (!confirmResult) {
        return;
    }


    $("#preloaderblurred").show();

    $("#ConfirmEnteredData")
        .prop("disabled", true);


    try {

        const result = await api.post(
            "/WorkOrder/ConfirmEnteredCustomerDispatchData",
            {}
        );


        if (result.success) {

            alert(result.message);

            loadcustomerdispatch();

        }
        else {

            alert(
                result.message ||
                "Unable to confirm entered data."
            );

        }

    }
    catch (error) {

        console.error(
            "Confirm Entered Data Error:",
            error
        );

        alert(
            "An error occurred while confirming entered data."
        );

    }
    finally {

        $("#preloaderblurred").hide();

    }

}