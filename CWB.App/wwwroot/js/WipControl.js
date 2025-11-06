
var ba_masterparts = {};
var allfilteredpodatas = {};

function loadPO() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllInw_Recpt_HeaderInsp").then((data) => {
        data = data.filter(item => item.status >= 3);
        let totalSubCon = 0;
        let totalRawMaterial = 0;
        let totalBOF = 0;

        for (let i = 0; i < data.length; i++) {
            if (data[i].partType === "SubCon") {
                totalSubCon++; // Adjust based on the actual property name
            } else if (data[i].partType === "RawMaterial") {
                totalRawMaterial++;
            } else if (data[i].partType === "BOF") {
                totalBOF++;
            }
        }

        console.log("Total SubCon:", totalSubCon);
        console.log("Total RawMaterial:", totalRawMaterial);
        console.log("Total BOF:", totalBOF);

        // Optionally, display these totals in the UI
        $("#subconInsp").text(totalSubCon);
        $("#rmInsp").text(totalRawMaterial);
        $("#bofInsp").text(totalBOF);
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
        console.error("Error fetching data:", error);
    });
}

function InwardPo() {
    $("#preloaderblurred").show();
    api.getbulk("/WorkOrder/GetAllPodetails").then((data) => {
        data = data.filter(item => item.status >= 2);
        let totalSubCon = 0;
        let totalRawMaterial = 0;
        let totalBOF = 0;

        for (let i = 0; i < data.length; i++) {
            if (data[i].partType === "ManufacturedPart") {
                totalSubCon++; // Adjust based on the actual property name
            } else if (data[i].partType === "RawMaterial") {
                totalRawMaterial++;
            } else if (data[i].partType === "BOF") {
                totalBOF++;
            }

        }
        $("#subconInw").text(totalSubCon);
        $("#rmInw").text(totalRawMaterial);
        $("#bofInw").text(totalBOF);
        $("#preloaderblurred").hide();

    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}

function loadNCLog() {
    var tablebody = $("#NcGrid tbody");
    $(tablebody).html("");//empty tbody

    $("#preloaderblurred").show();
    api.getbulk("/workOrder/GetAllNcLog").then((data) => {
        let totalInhouse = 0;
        let totalSubCon = 0;
        let totalRawMaterial = 0;
        let totalBOF = 0;

        for (let i = 0; i < data.length; i++) {
            if (data[i].partType === "SubCon") {
                totalSubCon++; 
            } else if (data[i].partType === "RawMaterial") {
                totalRawMaterial++;
            } else if (data[i].partType === "BOF") {
                totalBOF++;
            }
        }
        $("#ncInhouse").text(totalInhouse);
        $("#ncSubCon").text(totalSubCon);
        $("#ncRm").text(totalRawMaterial);
        $("#ncBof").text(totalBOF);
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
}
function LoadPartsExist() {
    var companyText = $("#CustomerSel").find('option:selected').text();

    if (ba_masterparts.length > 0) {
        var tablebody = $("#tbl-ba-existingparts tbody");
        $(tablebody).html("");//empty tbody
        let i = 0;
        if (ba_masterparts.length > 0) {
            ba_masterparts = ba_masterparts.filter(item => item.finalPart === "Y" && item.company === companyText);
            for (i = 0; i < ba_masterparts.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("BAParts", ba_masterparts[i], i));
            }
        }
    }
    else {
        $("#preloaderblurred").show();
        api.get("/masters/masterparts").then((data) => {
            ba_masterparts = data;
            var tablebody = $("#tbl-ba-existingparts tbody");
            $(tablebody).html("");//empty tbody
            let i = 0;
            if (ba_masterparts.length > 0) {
                ba_masterparts = ba_masterparts.filter(item => item.finalPart === "Y" &&  item.company === companyText);
                for (i = 0; i < ba_masterparts.length; i++) {
                    $(tablebody).append(AppUtil.ProcessTemplateDataNew("BAParts", ba_masterparts[i], i));
                }
            }
            $("#preloaderblurred").hide();
        }).catch((error) => {
            $("#preloaderblurred").hide();
        });
    }
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
    //$('#SalesCustomerOrderId').val();
    var partId = data[selval].partId;
    $.ajax({
        type: "GET",
        url: "/masters/CheckPartNoInDocList",
        data: { partId: partId },
        success: function (response) {
            if (!response) {
                alert("This Part Doesnot Have Required Document.");
                return;
            }
            else {
                $("#popup7PartNoField").val(data[selval].partNo + "/" + data[selval].description);
                $("#Popup7partId").val(data[selval].partId);
                document.getElementById("btn-close-ba-ExistingParts").click();
            }
        }
    });
}
function loadCustomers(CompanyOrSupplier) {//pass the element name
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
function LoadCustDesc(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    if (!compSelect.length)
        return;
    compSelect.empty();
    ////debugger;
    var div_data = "<option value=''>--Select--</option>";
    compSelect.append(div_data);
    api.get("/workorder/GetAllCust_NC_Decision").then((data) => {
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].cust_DecisionId + "'>" +
                data[i].cust_Decision +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}

function LoadPOLines(customerOrderId) {
    $("#btnExistBtn").prop("disabled", false);
    var compSelect = $('#NcPo');//should be a select2 dropdown
    compSelect.empty();;
    var div_data = "<option value=''>--Select--</option>";
    compSelect.append(div_data);
    allfilteredpodatas = {};
    $("#preloaderblurred").show();
    api.get("/businessaquisition/getcustorders").then((gcdata) => {
        var gcdata = gcdata.filter(item => item.customerId == customerOrderId);
        api.get("/businessaquisition/getsalesorders?customerOrderId=" + gcdata[0].customerOrderId).then((sodata) => {
            var custwoids = sodata.map(item => item.workOrderId);
            api.getbulk("/WorkOrder/GetAllProcPlan").then((procplan) => {
                var allfilteredprocdata = procplan.filter(item => custwoids.includes(item.partId));
                api.getbulk("/WorkOrder/GetAllPodetails").then((podata) => {
                    var allfilteredprocids = allfilteredprocdata.map(item => item.procPlanId);
                    var allfilteredpodata = podata.filter(item => allfilteredprocids.includes(item.partId));
                    //console.log(allfilteredpodata);
                    allfilteredpodatas = allfilteredpodata;
                    for (i = 0; i < allfilteredpodata.length; i++) {
                        div_data = "<option value='" +
                            allfilteredpodata[i].poDetailsId + "'>" +
                            allfilteredpodata[i].poReference +
                            "</option>";
                        compSelect.append(div_data);
                    }
                    if (allfilteredpodata.length > 0) {
                        $("#btnExistBtn").prop("disabled", true);
                    } else {
                        $("#btnExistBtn").prop("disabled", false);
                    }
                    $("#preloaderblurred").hide();
                }).catch((error) => {
                    $("#preloaderblurred").hide();
                });
            }).catch((error) => {
                $("#preloaderblurred").hide();
            });

        }).catch((error) => {
            $("#preloaderblurred").hide();
        });
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
                
}
$(document).ready(function () {
    InwardPo();
    loadNCLog();
    loadPO();
    $("#CustomerSel").on("change", function () {
        var selval = $(this).val().toLowerCase();
        LoadPOLines(selval);
    });
    $("#DecisionTaken").on("change", function () {
        var selval = $(this).val().toLowerCase();
        if (parseInt(selval) === 5) {
            $("#OtherDetailsDiv").show();
            $("#OtherDetails").show();
            $("#OthersLbl").show();
        } else {
            $("#OtherDetailsDiv").hide();
            $("#OthersLbl").hide();
            $("#OtherDetails").hide();
        }
    });
    $("#NcPo").on("change", function () {
        var selval = $(this).val().toLowerCase();
        var partid = allfilteredpodatas.filter(item => item.poDetailsId === parseInt(selval));
        $("#popup7PartNoField").val(partid[0].partNo);
        $("#Popup7partId").val(partid[0].partId);
        if (partid.length > 0) {
            $("#btnExistBtn").prop("disabled", true);
            $("#CustNcUnit").text("Nos")
        } else {
            $("#btnExistBtn").prop("disabled", false);
        }
    });
    $('#popup14').on('hidden.bs.modal', function (event) {
        $("#NcPo").val("");
        $("#popup7PartNoField").val("");
        $("#Popup7partId").val("");
        $("#CustNcRef").val("");
        $("#BallonNo").val("");
        $("#NcBallonFeature").val("");
        $("#NcDEscription").val("");
        $("#NcQunatity").val("");
        $("#AnyOtherDetails").val("");
        var NcBallonFeature = document.getElementById('NcBallonFeature');
        NcBallonFeature.style.border = '';
        var NcDEscription = document.getElementById('NcDEscription');
        NcDEscription.style.border = '';
        var NcQunatity = document.getElementById('NcQunatity');
        NcQunatity.style.border = '';
        var DecisionTaken = document.getElementById('DecisionTaken');
        DecisionTaken.style.border = '';
        var OtherDetails = document.getElementById('OtherDetails');
        OtherDetails.style.border = '';
        var CustomerSel = document.getElementById('CustomerSel');
        CustomerSel.style.border = '';
    });
    $('#popup14').on('show.bs.modal', function (event) {
        $("#OthersLbl").hide();
        $("#OtherDetails").hide();
        $("#OtherDetailsDiv").hide();
        loadCustomers("CustomerSel");
        LoadCustDesc("DecisionTaken");
        var firstText = $('.dropdown-menu a:first span').text().trim();
        $("#NcDataEntredBy").val(firstText);
        var today = new Date();

        // Format it as DD-MM-YYYY
        var formattedDate = String(today.getDate()).padStart(2, '0') + '-' +
            String(today.getMonth() + 1).padStart(2, '0') + '-' +
            today.getFullYear();

        // Set the value in the input field
        $("#NcDateEntry").val(formattedDate);
    });
    $("#btnExistBtn").on("click", function () {
        $("#ba_existing-part").modal("show");
        LoadPartsExist();
    });
    $('#ba_existing-part').on('show.bs.modal', function (event) {
        document.getElementById('popup14').style.filter = 'blur(5px)';
    });
    $('#ba_existing-part').on('hidden.bs.modal', function (event) {
        document.getElementById('popup14').style.filter = 'none';
    });

    $("#SaveCustNc").on("click", function () {
        var BallonNo = $("#BallonNo").val();
        var NcBallonFeature = $("#NcBallonFeature").val();
        var NcDEscription = $("#NcDEscription").val();
        var CustNcRef = $("#CustNcRef").val();
        var OtherDetails = $("#OtherDetails").val();
        var AnyOtherDetails = $("#AnyOtherDetails").val();
        var NcQunatity = parseInt($("#NcQunatity").val());
        var P7InwHeaderId = parseInt($("#P7InwHeaderId").val());
        var P7PartId = parseInt($("#Popup7partId").val());
        var P7NcId = parseInt($("#P7NcId").val());
        var DecisionTaken = parseInt($("#DecisionTaken").val());
        var CustomerSel = parseInt($("#CustomerSel").val());
        var NcPo = parseInt($("#NcPo").val());
        if (CustomerSel === 0 || isNaN(CustomerSel)) {
            var newNamevalidate = document.getElementById('CustomerSel');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('CustomerSel');
            newNamevalidate.style.border = '';
        }
        if (DecisionTaken === 0 || isNaN(DecisionTaken)) {
            var newNamevalidate = document.getElementById('DecisionTaken');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('DecisionTaken');
            newNamevalidate.style.border = '';
        }
        if (DecisionTaken == 5) {
            if (OtherDetails.length === 0) {
                var newNamevalidate = document.getElementById('OtherDetails');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('OtherDetails');
                newNamevalidate.style.border = '';
            }
        } else {
            OtherDetails = "-";
        }
        if (NcBallonFeature.length === 0) {
            var newNamevalidate = document.getElementById('NcBallonFeature');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('NcBallonFeature');
            newNamevalidate.style.border = '';
        }
        if (NcDEscription.length === 0) {
            var newNamevalidate = document.getElementById('NcDEscription');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('NcDEscription');
            newNamevalidate.style.border = '';
        }
        if (NcQunatity === 0 || isNaN(NcQunatity)) {
            var newNamevalidate = document.getElementById('NcQunatity');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('NcQunatity');
            newNamevalidate.style.border = '';
        }
        var decl = 'N';
        if (isNaN(P7NcId)) {
            P7NcId = 0;
        }
        var rowData = {
            insp_Outcome_Details_Id: parseInt(P7NcId),
            inw_Recpt_Header_Id: 0,
            inw_Recpt_Part_No_Id: P7PartId,
            balloon_No: BallonNo,
            balloon_No_Dir: NcBallonFeature,
            feature_Descrip: NcDEscription,
            nC_Descrip: NcDEscription,
            nC_Qnty: NcQunatity,
            decl_by_Supplier: decl,
            storage_Location: 3,
            nC_Log_status_Id: 1,
            nC_Entry_User: 1,
            cust_PO: NcPo,
            customer_NC_Ref: CustNcRef,
            nC_Cust_ID: DecisionTaken,
            other_Detail: OtherDetails,
            nC_Entry_date: new Date().toISOString().slice(0, 19),
            cust_comment: AnyOtherDetails,
            wo_Id: 0,
            so_Id: 0,
            cust_NC_date: 0,
            verified_Cutoff_Dt: 0,
            verification_done_by: 0,
            verified_Cutoff_Comment: 0
        };

        api.post("/WorkOrder/PostNcLog", rowData).then((data) => {
                alert("Customer Non Conformance Data Entry Saved");
                $("#popup14").modal("hide");
        }).catch((error) => {

        });
        //$.ajax({
        //    type: "POST",
        //    url: '/workOrder/PostNcLog',
        //    contentType: "application/json; charset=utf-8",
        //    headers: { 'Content-Type': 'application/json' },
        //    data: JSON.stringify(rowData),
        //    dataType: "json",
        //    success: function (result) {
        //        alert("Customer Non Conformance Data Entry Saved");
        //        $("#popup14").modal("hide");
        //    }
        //});
    });
});