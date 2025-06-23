let accp = 0;
let Popup10Open = 0;
let archive = 0;
async function showPopup14() {
    const popup = document.querySelector('#popup14');

    if (popup) {
        // Execute the code right before displaying the popup
        popup.style.overflow = 'auto';
        popup.style.display = 'block';
        popup.style.opacity = '1';

        const data = {};
        const popupStyles = window.getComputedStyle(popup);
        data.popupStyles = {
            overflow: popupStyles.overflow,
            height: popupStyles.height,
            display: popupStyles.display,
            position: popupStyles.position,
            overflowY: popupStyles.overflowY,
            overflowX: popupStyles.overflowX,
        };
        data.scrollHeight = popup.scrollHeight;
        data.clientHeight = popup.clientHeight;
    }
}
async function showPopup10() {
    const popup = document.querySelector('#popup10');

    if (popup) {
        // Execute the code right before displaying the popup
        popup.style.overflow = 'auto';
        popup.style.display = 'block';
        popup.style.opacity = '1';

        const data = {};
        const popupStyles = window.getComputedStyle(popup);
        data.popupStyles = {
            overflow: popupStyles.overflow,
            height: popupStyles.height,
            display: popupStyles.display,
            position: popupStyles.position,
            overflowY: popupStyles.overflowY,
            overflowX: popupStyles.overflowX,
        };
        data.scrollHeight = popup.scrollHeight;
        data.clientHeight = popup.clientHeight;
    }
}
function GetAllNC_Decision_Log(nclogId) {
    api.getbulk("/workOrder/GetAllNC_Decision_Log").then((data) => {
        Popup10Open++;
        for (i = 0; i < data.length; i++) {
            Popup10Open++;
            data = data.filter(item => item.ncLogId === nclogId);
            $("#P10FeedbackReceiptDateDiv").show();
            $("#P10FeedbackConditionalAcceptanceDiv").show();
            $("#P10RequestDecisionDiv").show();
            //$("#P10DivInsDesc").show();
            $("#P10DivRedo").show();
            //$("#P10DivDesc2").show();
            $("#P10DivDesc2VM").show();
            $("#P10DivDescVm").show();
            $("#P10NCInslvl2VM").show();
            $("#P10DivInssDesc2").show();
            $("#P10Save2").show();
            //$("#P10NClvl1VM").show();
            $("#P10RequestToCustomerDiv").show();
            $("#P10RequestedDateDiv").show();
            $("#P10FeedbackReceiptDateDiv").show();
            $("#P10RequestDecisionDiv").show();
            $("#P10RequestDecisionDiv").removeClass('d-none');
            $("#P10FeedbackReceiptDateDiv").removeClass('d-none');
            $("#P10FeedbackConditionalAcceptanceDiv").removeClass('d-none');
            if (data[data.length - 1].cust_Decision_Reqd === 'Y') {
                $("#P10ConsentRequired").prop("checked", true);
                if (data[data.length - 1].cust_Dec_Request === 1 && data[data.length - 1].cust_feedback_Id === 1) {
                    $("#P10NClvl1 option[value=" + 1 + "]").show();
                    $("#P10NClvl1 option[value=" + 2 + "]").show();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").hide();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl1 option[value=" + 1 + "]").show();
                    $("#P10NClvl1 option[value=" + 2 + "]").show();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").hide();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                } else if (data[data.length - 1].cust_Dec_Request === 1 && data[data.length - 1].cust_feedback_Id === 3) {
                    $("#P10NClvl1 option[value=" + 1 + "]").hide();
                    $("#P10NClvl1 option[value=" + 2 + "]").hide();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").show();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl2 option[value=" + 1 + "]").hide();
                    $("#P10NClvl2 option[value=" + 2 + "]").hide();
                    $("#P10NClvl2 option[value=" + 3 + "]").hide();
                    $("#P10NClvl2 option[value=" + 4 + "]").hide();
                    $("#P10NClvl2 option[value=" + 5 + "]").hide();
                    $("#P10NClvl2 option[value=" + 6 + "]").hide();
                    $("#P10NClvl2 option[value=" + 7 + "]").show();
                    $("#P10NClvl2 option[value=" + 8 + "]").hide();
                } else if (data[data.length - 1].cust_Dec_Request === 2 && data[data.length - 1].cust_feedback_Id === 3) {
                    $("#P10NClvl1 option[value=" + 1 + "]").hide();
                    $("#P10NClvl1 option[value=" + 2 + "]").hide();
                    $("#P10NClvl1 option[value=" + 3 + "]").hide();
                    $("#P10NClvl1 option[value=" + 4 + "]").hide();
                    $("#P10NClvl1 option[value=" + 5 + "]").hide();
                    $("#P10NClvl1 option[value=" + 6 + "]").hide();
                    $("#P10NClvl1 option[value=" + 7 + "]").show();
                    $("#P10NClvl1 option[value=" + 8 + "]").hide();
                    $("#P10NClvl2 option[value=" + 1 + "]").hide();
                    $("#P10NClvl2 option[value=" + 2 + "]").hide();
                    $("#P10NClvl2 option[value=" + 3 + "]").hide();
                    $("#P10NClvl2 option[value=" + 4 + "]").hide();
                    $("#P10NClvl2 option[value=" + 5 + "]").hide();
                    $("#P10NClvl2 option[value=" + 6 + "]").hide();
                    $("#P10NClvl2 option[value=" + 7 + "]").show();
                    $("#P10NClvl2 option[value=" + 8 + "]").hide();
                } else if (data[data.length - 1].cust_Dec_Request === 1 && data[data.length - 1].cust_feedback_Id === 2) {
                        $("#P10NClvl1 option[value=" + 1 + "]").hide();
                        $("#P10NClvl1 option[value=" + 2 + "]").show();
                        $("#P10NClvl1 option[value=" + 3 + "]").hide();
                        $("#P10NClvl1 option[value=" + 4 + "]").show();
                        $("#P10NClvl1 option[value=" + 5 + "]").hide();
                        $("#P10NClvl1 option[value=" + 6 + "]").hide();
                        $("#P10NClvl1 option[value=" + 7 + "]").hide();
                        $("#P10NClvl1 option[value=" + 8 + "]").hide();
                        $("#P10NClvl2 option[value=" + 1 + "]").hide();
                        $("#P10NClvl2 option[value=" + 2 + "]").show();
                        $("#P10NClvl2 option[value=" + 3 + "]").hide();
                        $("#P10NClvl2 option[value=" + 4 + "]").show();
                        $("#P10NClvl2 option[value=" + 5 + "]").hide();
                        $("#P10NClvl2 option[value=" + 6 + "]").hide();
                        $("#P10NClvl2 option[value=" + 7 + "]").hide();
                        $("#P10NClvl2 option[value=" + 8 + "]").hide();
                    }else if (data[data.length - 1].cust_Dec_Request === 2 && data[data.length - 1].cust_Dec_Request === 2) {
                        $("#P10NClvl1 option[value=" + 1 + "]").hide();
                        $("#P10NClvl1 option[value=" + 2 + "]").hide();
                        $("#P10NClvl1 option[value=" + 3 + "]").hide();
                        $("#P10NClvl1 option[value=" + 4 + "]").show();
                        $("#P10NClvl1 option[value=" + 5 + "]").hide();
                        $("#P10NClvl1 option[value=" + 6 + "]").hide();
                        $("#P10NClvl1 option[value=" + 7 + "]").hide();
                        $("#P10NClvl1 option[value=" + 8 + "]").hide();
                        $("#P10NClvl2 option[value=" + 1 + "]").hide();
                        $("#P10NClvl2 option[value=" + 2 + "]").hide();
                        $("#P10NClvl2 option[value=" + 3 + "]").hide();
                        $("#P10NClvl2 option[value=" + 4 + "]").show();
                        $("#P10NClvl2 option[value=" + 5 + "]").hide();
                        $("#P10NClvl2 option[value=" + 6 + "]").hide();
                        $("#P10NClvl2 option[value=" + 7 + "]").hide();
                        $("#P10NClvl2 option[value=" + 8 + "]").hide();
                    }
            } else {
                $("#P10ConsentRequired").prop("checked", false);
            }
            if (data[data.length - 1].redoCA === 'Y') {
                $("#P10RedoChkl1").prop("checked", true);
                $("#P10NCInslvl2VM").hide();
                $("#P10DivDesc2VM").hide();
                $("#P10DivDesc2").hide();
            } else {
                $("#P10RedoChkl1").prop("checked", false);
                $("#P10NCInslvl2VM").show();
                $("#P10DivDesc2VM").show();
                $("#P10DivDesc2").show();
            }
            let dateValue = data[data.length - 1].cust_Request_date.split("T")[0];
            let P10FeedbackReceiptDate = data[data.length - 1].cust_feedback_date.split("T")[0];
            $("#P10NcDecId").val(data[data.length - 1].nC_Decision_LogId);
            $("#P10RequestToCustomer").val(data[data.length - 1].cust_Dec_Request);
            $("#P10RequestedDate").val(dateValue);
            $("#P10FeedbackReceiptDate").val(P10FeedbackReceiptDate);
            $("#P10NClvl1").val(data[data.length - 1].nC_Disp_Deci_Lvl1_Id);
            $("#P10NClvl2").val(data[data.length - 1].nC_Disp_Deci_Lvl2_Id);
            $("#P10NCInslvl1").val(data[data.length - 1].nC_Disp_Inst_Lvl1);
            $("#P10NCInslvl2").val(data[data.length - 1].nC_Disp_Inst_Lvl2);
            $("#P10RequestDecision").val(data[data.length - 1].cust_feedback_Id);
            $("#P10FeedbackConditionalAcceptance").val(data[data.length - 1].cust_Feedback_Desc);

        }
        //loadSelectNCDispDecision();
    }).catch((error) => {
    });
}
$(document).ready(function () {
    $("#searchNcRefNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#NcGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchNcLoc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#NcGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#NcGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchPartType").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#searchPartType option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#NcGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[6]).text().toLowerCase().indexOf(selectedText) > -1)
            });
        } else {
            $("#NcGrid tbody tr").show();
        }
    });
    loadNCLog();
    $('#P10RedoChkl1').change(function () {
        if ($(this).is(':checked')) {
            $("#P10DivDesc2").hide();
        } else {
            $("#P10DivDesc2").show();
        }
    });
    $('#P10ConsentRequired').change(function () {
        if ($(this).is(':checked')) {
            if (Popup10Open === 1) {
                $("#P10RequestedDateDiv").show();
                $("#P10RequestToCustomerDiv").show();
                $("#P10FeedbackReceiptDateDiv").hide();
                $("#P10FeedbackConditionalAcceptanceDiv").hide();
                $("#P10RequestDecisionDiv").hide();
                $("#P10DivDesc").hide();
                $("#P10DivDescVm").hide();
                $("#P10DivInsDesc").hide();
                $("#P10DivRedo").hide();
                //$("#P10DivDesc2").hide();
                $("#P10DivDesc2VM").hide();
                $("#P10DivDescVm").hide();
                $("#P10NCInslvl2VM").hide();
                $("#P10DivInssDesc2").hide();
                $("#P10NClvl1VM").hide();
                $("#P10Save2").hide();
            } else if (Popup10Open => 1) {
                $("#P10RequestedDateDiv").hide();
                $("#P10RequestToCustomerDiv").hide();
                $("#P10FeedbackReceiptDateDiv").show();
                $("#P10FeedbackConditionalAcceptanceDiv").show();
                $("#P10RequestDecisionDiv").show();
                $("#P10DivDesc").show();
                $("#P10DivDescVm").show();
                $("#P10DivInsDesc").show();
                $("#P10DivRedo").show();
               // $("#P10DivDesc2").show();
                $("#P10DivDesc2VM").show();
                $("#P10DivDescVm").show();
                $("#P10NCInslvl2VM").show();
                $("#P10DivInssDesc2").show();
                $("#P10NClvl1VM").show();
                $("#P10Save2").show();
            } else {
                $("#P10RequestedDateDiv").hide();
                $("#P10RequestToCustomerDiv").hide();
                $("#P10FeedbackReceiptDateDiv").show();
                $("#P10FeedbackConditionalAcceptanceDiv").show();
                $("#P10RequestDecisionDiv").show();
                $("#P10DivDesc").show();
                $("#P10DivDescVm").show();
                $("#P10DivInsDesc").show();
                $("#P10DivRedo").show();
                //$("#P10DivDesc2").show();
                $("#P10DivDesc2VM").show();
                $("#P10DivDescVm").show();
                $("#P10NCInslvl2VM").show();
                $("#P10DivInssDesc2").show();
                $("#P10Save2").show();
                $("#P10NClvl1VM").show();
            }
        } else {
            $("#P10RequestedDateDiv").hide();
            $("#P10RequestToCustomerDiv").hide();
            $("#P10FeedbackReceiptDateDiv").show();
            $("#P10FeedbackConditionalAcceptanceDiv").show();
            $("#P10RequestDecisionDiv").show();
            $("#P10DivDesc").show();
            $("#P10DivDescVm").show();
            $("#P10DivInsDesc").show();
            $("#P10DivRedo").show();
            //$("#P10DivDesc2").show();
            $("#P10DivDesc2VM").show();
            $("#P10DivDescVm").show();
            $("#P10NCInslvl2VM").show();
            $("#P10DivInssDesc2").show();
            $("#P10Save2").show();
            $("#P10NClvl1VM").show();
        }
    });
    $("#P10Save2").on("click", function () {
        const restrictDt = new Date();
        var P10RequestedDate = new Date(Date.parse($("#P10RequestedDate").val()));
        var P10FeedbackReceiptDate = new Date(Date.parse($("#P10FeedbackReceiptDate").val()));
        var P10NClvl2 = parseInt($("#P10NClvl2").val());
        var P10NCInslvl2 = $("#P10NCInslvl2").val();
        var P10NClvl1 = $("#P10NClvl1").val();
        var P10NCInslvl1 = $("#P10NCInslvl1").val();
        var P10FeedbackConditionalAcceptance = $("#P10FeedbackConditionalAcceptance").val();
        var P10RequestDecision = $("#P10RequestDecision").val();
        var P10RequestToCustomer = $("#P10RequestToCustomer").val();
        var P10CustDesignChk = document.getElementById("P10ConsentRequired");
        var P10RedoChkl1 = document.getElementById("P10RedoChkl1");
        var P10NCLogId = $("#P10NCLogId").val();
        var P10CustDesignChkVal = 'N';
        var P10RedoChkl1val = 'N';
        var formattedDate2;
        var formattedDate;
        if (P10CustDesignChk.checked) {
            P10CustDesignChkVal = 'Y';
            if (P10RequestToCustomer === 0) {
                var newNamevalidate = document.getElementById('P10RequestToCustomer');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P10RequestToCustomer');
                newNamevalidate.style.border = '';
            }
            if (isNaN(P10RequestedDate.getTime())) {
                var newNamevalidate = document.getElementById('P10RequestedDate');
                newNamevalidate.style.border = '2px solid red';
                return false;

                // or display an error message to the user
            } else if (P10RequestedDate <= restrictDt) {
                var newNamevalidate = document.getElementById('P10RequestedDate');
                newNamevalidate.style.border = '2px solid red';
                alert("New Date Retained Should Be Greater Than Current Date Retained");
                return false;
                // or display an error message to the user
            }
            else {
                formattedDate = P10RequestedDate.toISOString();
                var newNamevalidate = document.getElementById('P10RequestedDate');
                newNamevalidate.style.border = '';
            }
        }
        if (Popup10Open > 1) {
            if (isNaN(P10FeedbackReceiptDate.getTime())) {
                var newNamevalidate = document.getElementById('P10FeedbackReceiptDate');
                newNamevalidate.style.border = '2px solid red';
                return false;

                // or display an error message to the user
            } else if (P10FeedbackReceiptDate <= restrictDt) {
                var newNamevalidate = document.getElementById('P10FeedbackReceiptDate');
                newNamevalidate.style.border = '2px solid red';
                alert("New Date Retained Should Be Greater Than Current Date Retained");
                return false;
                // or display an error message to the user
            }
            else {
                formattedDate2 = P10FeedbackReceiptDate.toISOString();
                var newNamevalidate = document.getElementById('P10FeedbackReceiptDate');
                newNamevalidate.style.border = '';
            }
            if (P10RequestDecision === 0) {
                var newNamevalidate = document.getElementById('P10RequestDecision');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P10RequestDecision');
                newNamevalidate.style.border = '';
            }
        }
        if (formattedDate2 === null) {
            formattedDate2 = formattedDate;
        }
        if (P10RedoChkl1.checked) {
            P10RedoChkl1val = 'Y';
            if (P10NClvl2 === 0) {
                var newNamevalidate = document.getElementById('P10NClvl2');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P10NClvl2');
                newNamevalidate.style.border = '';
            }
        }
        formattedDate = P10RequestedDate.toISOString();
        var formdata = {
            nC_Decision_LogId: 0,
            ncLogId: parseInt(P10NCLogId),
            cust_Decision_Reqd: P10CustDesignChkVal,
            redoCA: P10RedoChkl1val,
            cust_Dec_Request: P10RequestToCustomer,
            cust_Request_date: formattedDate,
            cust_feedback_date: formattedDate2,
            cust_feedback_Id: P10RequestDecision,
            cust_Feedback_Desc: P10FeedbackConditionalAcceptance,
            nC_Disp_Deci_Lvl1_Id: parseInt(P10NClvl1),
            nC_Disp_Deci_Lvl2_Id: P10NClvl2,
            nC_Disp_Inst_Lvl1: P10NCInslvl1,
            nC_Disp_Inst_Lvl2: P10NCInslvl2,
            nC_Disp_Decision_Id: 0,
            nC_Disp_Instruction: 0
        };
        api.post("/WorkOrder/PostNC_Decision_Log", formdata).then((data) => {
            //loadGetAllNC_Wk_List_Appl();
            if (P10RedoChkl1.checked) {
                var P14RCLogId = parseInt($("#P14RCLogId").val());
                var rowdata = {
                    cont_RCA_CA_LogId: P14RCLogId,
                    cont_RCA_CA_Status_Id: 4
                };
                api.post("/WorkOrder/UpdateCont_RCA_CA_log", rowdata).then((data) => {

                });
            } else {
                var P14RCLogId = parseInt($("#P14RCLogId").val());
                var rowdata = {
                    cont_RCA_CA_LogId: P14RCLogId,
                    cont_RCA_CA_Status_Id: 2
                };
                api.post("/WorkOrder/UpdateCont_RCA_CA_log", rowdata).then((data) => {

                });
            }
            loadNCLog();
            $("#popup10").modal("hide");
        }).catch((error) => {
            console.log(error);
        });
    });
    $("#P14UploadOther").on("click", function () {
        $("#doc-item").modal("show");
    });
    $("#P10Save").on("click", function () {
        //var P10RequestedDate = $("#P10RequestedDate").val();
        const restrictDt = new Date();
        var P10RequestedDate = new Date(Date.parse($("#P10RequestedDate").val()));
        var P10FeedbackReceiptDate = new Date(Date.parse($("#P10FeedbackReceiptDate").val()));
        var P10NClvl1 = $("#P10NClvl1").val();
        var P10NCInslvl1 = $("#P10NCInslvl1").val();
        var P10FeedbackConditionalAcceptance = $("#P10FeedbackConditionalAcceptance").val();
        var P10RequestDecision = $("#P10RequestDecision").val();
        var P10RequestToCustomer = $("#P10RequestToCustomer").val();
        var P10CustDesignChk = document.getElementById("P10ConsentRequired");
        var P10NcDecId = $("#P10NcDecId").val();
        var P10NCLogId = $("#P10NCLogId").val();
        var P10CustDesignChkVal = 'N';
        var formattedDate2;
        var formattedDate;
        if (P10CustDesignChk.checked) {
            P10CustDesignChkVal = 'Y';
        }
        if (P10RequestToCustomer === 0) {
            var newNamevalidate = document.getElementById('P10RequestToCustomer');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P10RequestToCustomer');
            newNamevalidate.style.border = '';
        }
        if (P10RequestDecision === "2") {
            if (P10FeedbackConditionalAcceptance.length === 0) {
                var newNamevalidate = document.getElementById('P10FeedbackConditionalAcceptance');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P10FeedbackConditionalAcceptance');
                newNamevalidate.style.border = '';
            }
        } 
        if (P10NClvl1 === "4" || P10NClvl1 === "2") {
            if (P10NCInslvl1.length === 0) {
                var newNamevalidate = document.getElementById('P10NCInslvl1');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P10NCInslvl1');
                newNamevalidate.style.border = '';
            }
        } 
        if (isNaN(P10RequestedDate.getTime())) {
            var newNamevalidate = document.getElementById('P10RequestedDate');
            newNamevalidate.style.border = '2px solid red';
            return false;

            // or display an error message to the user
        } else if (P10RequestedDate <= restrictDt) {
            var newNamevalidate = document.getElementById('P10RequestedDate');
            newNamevalidate.style.border = '2px solid red';
            alert("New Date Retained Should Be Greater Than Current Date Retained");
            return false;
            // or display an error message to the user
        }
        else {
            formattedDate = P10RequestedDate.toISOString();
            var newNamevalidate = document.getElementById('P10RequestedDate');
            newNamevalidate.style.border = '';
        }
        if (Popup10Open >= 2) {
            if (isNaN(P10FeedbackReceiptDate.getTime())) {
                var newNamevalidate = document.getElementById('P10FeedbackReceiptDate');
                newNamevalidate.style.border = '2px solid red';
                return false;

                // or display an error message to the user
            } else if (P10FeedbackReceiptDate <= restrictDt) {
                var newNamevalidate = document.getElementById('P10FeedbackReceiptDate');
                newNamevalidate.style.border = '2px solid red';
                alert("New Date Retained Should Be Greater Than Current Date Retained");
                return false;
                // or display an error message to the user
            }
            else {
                formattedDate2 = P10FeedbackReceiptDate.toISOString();
                var newNamevalidate = document.getElementById('P10FeedbackReceiptDate');
                newNamevalidate.style.border = '';
            }
            if (P10RequestDecision === 0) {
                var newNamevalidate = document.getElementById('P10RequestDecision');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P10RequestDecision');
                newNamevalidate.style.border = '';
            }
        }
        if (formattedDate2 === null) {
            formattedDate2 = formattedDate;
        }
        if (isNaN(P10NcDecId)) {
            P10NcDecId =0
        }

        var formdata = {
            nC_Decision_LogId: P10NcDecId,
            ncLogId: parseInt(P10NCLogId),
            cust_Decision_Reqd: P10CustDesignChkVal,
            redoCA: 'N',
            cust_Dec_Request: P10RequestToCustomer,
            cust_Request_date: formattedDate,
            cust_feedback_date: formattedDate2,
            cust_feedback_Id: P10RequestDecision,
            cust_Feedback_Desc: P10FeedbackConditionalAcceptance,
            nC_Disp_Deci_Lvl1_Id: parseInt(P10NClvl1),
            nC_Disp_Deci_Lvl2_Id: 0,
            nC_Disp_Inst_Lvl1: P10NCInslvl1,
            nC_Disp_Inst_Lvl2: 0,
            nC_Disp_Decision_Id: 0,
            nC_Disp_Instruction: 0
        };
        api.post("/WorkOrder/PostNC_Decision_Log", formdata).then((data) => {
            //loadGetAllNC_Wk_List_Appl();
            $("#popup10").modal("hide");
            loadNCLog();
        }).catch((error) => {
            console.log(error);
        });
    });

    $('#UploadDocumnet').on('hidden.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'none';
    });
    $('#UploadDocumnet').on('show.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'blur(5px)';
        var P10NCLogId = $("#P10NCLogId").val();
        loadDetailsDocUploadList(P10NCLogId);
    });
    $('#PartInspectionDocPop').on('hidden.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'none';
    });
    $('#PartInspectionDocPop').on('show.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'blur(5px)';
    });
    $('#InspectionDocPop').on('hidden.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'none';
    });
    $('#InspectionDocPop').on('show.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'blur(5px)';
    });
    $('#popup10').on('show.bs.modal', function (event) {
        showPopup10();
        $("#P10ConsentRequired").prop('checked', false);
        var P10FeedbackConditionalAcceptance = document.getElementById('P10FeedbackConditionalAcceptance');
        P10FeedbackConditionalAcceptance.style.border = '';
        var P10NCInslvl1 = document.getElementById('P10NCInslvl1');
        P10NCInslvl1.style.border = '';
        $("#P10RequestToCustomerDiv").hide();
        $("#P10RequestedDateDiv").hide();
        $("#P10FeedbackReceiptDateDiv").hide();
        $("#P10RequestDecisionDiv").hide();
        $("#P10RequestDecisionDiv").addClass('d-none');
        $("#P10FeedbackReceiptDateDiv").addClass('d-none');
        $("#P10FeedbackConditionalAcceptanceDiv").addClass('d-none');
        Popup10Open = 0;
        var relatedTarget = $(event.relatedTarget);
        var id = relatedTarget.data("id");
        var balno = relatedTarget.data("balno");
        var baldesc = relatedTarget.data("baldesc");
        var ncdes = relatedTarget.data("ncdes");
        var declsup = relatedTarget.data("declsup");
        var qnty = relatedTarget.data("qnty");
        var nctrack = relatedTarget.data("nctrack");
        var loc = relatedTarget.data("loc");
        var partno = relatedTarget.data("partno");
        var divhide = relatedTarget.data("divhide");
        var parttype = relatedTarget.data("parttype");
        var partid = relatedTarget.data("partid");
        var locnam = relatedTarget.data("locnam");
        var headid = relatedTarget.data("headid");
        var inspid = relatedTarget.data("inspid");
        loadPartDoc(headid);
        loadPartDocInsp(headid);
        GetAllNC_Decision_Log(id);
        $("#P10NcRef").val(nctrack);
        $("#P10BallNo").val(balno);
        $("#P10NcQnty").val(qnty);
        $("#P10NCLogId").val(id);
        $("#P10CurrentLoc").val(loc);
        $("#P10NcDesc").val(ncdes);
        $("#P10NcBaldesc").val(baldesc);
        $("#P10NcPartNo").val(locnam);
        $("#P10SpanHeading").text("PO Line Item Inward");
        $("#Span10Partno").text(partno);
        GetRcalog(id);
        if (declsup === 'Y') {
            $("#P10NCChkBySup").prop('checked', true);
        } else {
            $("#P10NCChkBySup").prop('checked', false);
        }
        if (divhide === 'Y') {
            $("#P10RequestToCustomer").prop('readonly', false);
            $("#P10RequestToCustomer").css({
                "pointer-events": "auto",
                "background-color": ""
            });
            $("#P10RequestedDate").prop('readonly', false);
            $("#P10FeedbackReceiptDate").prop('readonly', false);
            $("#P10RequestDecision").prop('readonly', false);
            $("#P10RequestDecision").css({
                "pointer-events": "auto",
                "background-color": ""
            });
            $("#P10FeedbackConditionalAcceptance").prop('readonly', false);
            $("#P10NClvl1").prop('readonly', false);
            $("#P10NClvl1").css({
                "pointer-events": "auto",
                "background-color": ""
            });
            $("#P10NCInslvl1").prop('readonly', false);
            $("#P10Save").show();
            $("#FullRedoDiv").hide();
        } else {
            $("#P10RequestToCustomer").prop('readonly', true);
            $("#P10RequestToCustomer").css({
                "pointer-events": "none",
                "background-color": "#e9ecef"
            });
            $("#P10RequestedDate").prop('readonly', true);
            $("#P10FeedbackReceiptDate").prop('readonly', true);
            $("#P10RequestDecision").prop('readonly', true);
            $("#P10RequestDecision").css({
                "pointer-events": "none",
                "background-color": "#e9ecef"
            });
            $("#P10FeedbackConditionalAcceptance").prop('readonly', true);
            $("#P10NClvl1").prop('readonly', true);
            $("#P10NClvl1").css({
                "pointer-events": "none",
                "background-color": "#e9ecef"
            });
            $("#P10NCInslvl1").prop('readonly', true);
            $("#P10Save").hide();
            $("#FullRedoDiv").show();
        }
        $("#Span10RoutingDiv").hide();
        if (parttype === "SubCon") {
            $("#P10SpanHeading").text("Shop");
            $("#SCmp").show();
            $("#SAssy").hide();
            $("#2SAssy").hide();
            $("#2SCmp").show();
            $("#2SendbehaRm").show();
            $("#2SendRm").show();
            $("#SendRm").show();
            $("#SendbehaRm").show();
            $("#Span10RoutingDiv").show();
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partid)).then((data) => {
                api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                    var routname = data[data.length - 1].routingName;
                    var opname = stepdata[stepdata.length - 1].stepNumber;
                    var stepId = stepdata[stepdata.length - 1].stepId;
                    $("#Span10Routing").text(routname);
                    $("#Span10OprNo").text(opname + " Input PartNo");

                    api.get("/routings/subcons?stepId=" + stepId).then((subcond) => {
                        data = data.filter(item => item.deleted != 1);
                        var ncval = qnty * parseInt(subcond[subcond.length - 1].costPerPart);
                        $("#P10NcValue").val(ncval);
                    }).catch((error) => {
                    });
                });
            });
        } else {
            $("#SCmp").hide();
            $("#SAssy").hide();
            $("#2SAssy").hide();
            $("#2SCmp").hide();
            $("#2SendbehaRm").show();
            $("#2SendRm").show();
            $("#SendRm").show();
            $("#SendbehaRm").show();
            api.get("/workOrder/GetPartPurchase?partId=" + partid).then((subcond) => {
                var ncval = qnty * parseInt(subcond[subcond.length - 1].price);
                $("#P10NcValue").val(ncval);
            }).catch((error) => {
            });
        }
        api.getbulk("/Employee/GetAllOrgChart").then((data) => {
            data = data.filter(item => item.level_No === 1);
            $("#P10NClvl1VM").val(data[data.length - 1].level_No);

        }).catch((error) => {
        });
    });
    $('#popup14').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        showPopup14();
        $("#P14Accept").removeClass("btn-primary");
        $("#P14Accept").addClass("btn-outline-primary");
        var id = relatedTarget.data("id");
        var balno = relatedTarget.data("balno");
        var baldesc = relatedTarget.data("baldesc");
        var ncdes = relatedTarget.data("ncdes");
        var declsup = relatedTarget.data("declsup");
        var qnty = relatedTarget.data("qnty");
        var nctrack = relatedTarget.data("nctrack");
        var loc = relatedTarget.data("loc");
        var partno = relatedTarget.data("partno");
        var divhide = relatedTarget.data("divhide");
        var parttype = relatedTarget.data("parttype");
        var partid = relatedTarget.data("partid");
        loadPartDocNC(id);
        $("#P14Containt").val('');
        $("#P14NcRef").val(nctrack);
        $("#P14PartId").val(partid);
        $("#P14BallNo").val(balno);
        if (declsup === 'Y') {
            $("#P14NCChkBySup").prop('checked', true);
        } else {
            $("#P14NCChkBySup").prop('checked', false);
        }
        if (divhide === 'Y') {
            $("#P14Containt").prop('readonly', false);
            $("#FeedLvl2Div").hide();
            $("#P14Save").prop('disabled', false);
        } else {
            $("#P14Containt").prop('readonly', true);
            $("#P14Save").prop('disabled', true);
            $("#P14Comment").prop('readonly', false);
            $("#FeedLvl2Div").show();
        }
        $("#P14NcQnty").val(qnty);
        $("#P14NCLogId").val(id);
        $("#P14CurrentLoc").val(loc);
        $("#P14NcDesc").val(ncdes);
        $("#P14NcBaldesc").val(baldesc);
        $("#Span14Partno").text(partno);
        $("#Span14RoutingDiv").hide();
        if (parttype === "SubCon") {
            $("#Span14RoutingDiv").show();
            api.getbulk("/WorkOrder/GetRoutings?manufPartId=" + parseInt(partid)).then((data) => {
                api.getbulk("/WorkOrder/RoutingSteps?routingId=" + data[data.length - 1].routingId).then((stepdata) => {
                    var routname = data[data.length - 1].routingName;
                    var opname = stepdata[stepdata.length - 1].stepNumber;
                    $("#Span14Routing").text(routname);
                    $("#Span14OprNo").text(opname + " Input PartNo");
                });
            });
        }
        GetRcalog(id);
        var newNamevalidate = document.getElementById('P14Containt');
        newNamevalidate.style.border = '';
        var P14Comment = document.getElementById('P14Comment');
        P14Comment.style.border = '';
    });
    $("#P14Accept").on("click", function () {
        if (accp === 1) {
            $("#P14Accept").removeClass("btn-primary");
            $("#P14Accept").addClass("btn-outline-primary");
            accp = 2;
        } else {
            accp = 1;
            var newNamevalidate = document.getElementById('P14Comment');
            newNamevalidate.style.border = '';
            alert("Accepted");
            $("#P14Accept").removeClass("btn-outline-primary");
            $("#P14Accept").addClass("btn-primary");
            $("#P14Comment").val('');
        }
    });
    $("#P14CorrectResubmit").on("click", function () {
        accp = 2;
        $("#P14Accept").removeClass("btn-primary");
        $("#P14Accept").addClass("btn-outline-primary");
        alert("Correct / Resubmit");
    });
    $("#ExitWarningBtn").on("click", function () {
        $("#popup10").modal("hide");
        $("#warning").modal("hide");
        $("#popup14").modal("hide");
    });
    $("#EP7Exit").on("click", function () {
        $("#popup10").modal("hide");
        $("#NotUploaded").modal("hide");
        $("#popup14").modal("hide");
    });
    $("#EP7Return").on("click", function () {
        $("#NotUploaded").modal("hide");
    });
    $('#warning').on('show.bs.modal', function (event) {
        document.getElementById('popup14').style.filter = 'blur(5px)';
        document.getElementById('popup10').style.filter = 'blur(5px)';
    });
    $('#warning').on('hidden.bs.modal', function (event) {
        document.getElementById('popup14').style.filter = 'none';
        document.getElementById('popup10').style.filter = 'none';
    });
    $("#P10BtnCLose").on("click", function () {
        var popupInwardHeaderId = parseInt($("#P10NcDecId").val());
        var P10RequestToCustomer = parseInt($("#P10RequestToCustomer").val());
        var P10RequestDecision = parseInt($("#P10RequestDecision").val());
        var P10NClvl1 = parseInt($("#P10NClvl1").val());
        var P10FeedbackConditionalAcceptance = $("#P10FeedbackConditionalAcceptance").val().trim();
        var P10NCInslvl1 = $("#P10NCInslvl1").val().trim();
        var P10RequestedDate = $("#P10RequestedDate").val();
        var P10FeedbackReceiptDate = $("#P10FeedbackReceiptDate").val();
        if (P10RequestToCustomer > 0 || isNaN(P10RequestToCustomer) || isNaN(P10RequestDecision) || P10RequestDecision > 0 || P10NClvl1 > 0 || P10RequestedDate.length > 0 || P10FeedbackConditionalAcceptance.length > 0 || P10NCInslvl1.length > 0) {
            if (isNaN(popupInwardHeaderId)) {
                $("#warning").modal("show");
            } else {
                $("#popup10").modal("hide");
            }
        } else {
            $("#popup10").modal("hide");
        }
    });
    $("#btnP14Close").on("click", function () {

        var popupInwardHeaderId = parseInt($("#P14RCLogId").val());
        var P14Containt = $("#P14Containt").val().trim();
        if (P14Containt.length > 0) {
            if (isNaN(popupInwardHeaderId)) {
                $("#warning").modal("show");
            } else {
                $("#popup14").modal("hide");
            }
        } else {
            $("#popup14").modal("hide");
        }
        //$("#P14Containt").val('');
    });
    $("#P14Save").on("click", function () {
        var P14Containt = $("#P14Containt").val().trim();
        var P14Comment = $("#P14Comment").val();
        var P14NCLogId = $("#P14NCLogId").val();
        var P14RCLogId = parseInt($("#P14RCLogId").val());
        if (P14Containt.length === 0) {
            var newNamevalidate = document.getElementById('P14Containt');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('P14Containt');
            newNamevalidate.style.border = '';
        }
        if (isNaN(P14RCLogId)) {
            P14RCLogId = 0;
        }

        var formdata = {
            cont_RCA_CA_LogId: P14RCLogId,
            ncLogId: parseInt(P14NCLogId),
            containment_Action: P14Containt,
            senior_Feedback: " ",
            cont_RCA_CA_Status_Id: 1
        };
        api.post("/WorkOrder/PostCont_RCA_CA_log", formdata).then((data) => {
            //loadGetAllNC_Wk_List_Appl();
            $("#P14RCLogId").val(data.cont_RCA_CA_LogId);
            loadNCLog();
        }).catch((error) => {
            console.log(error);
        });
    });
    $("#P14SaveRca").on("click", function () {
        var P14Containt = $("#P14Containt").val().trim();
        var P14Comment = $("#P14Comment").val();
        var P14NCLogId = $("#P14NCLogId").val();
        var P14RCLogId = parseInt($("#P14RCLogId").val());
        if (isNaN(P14RCLogId) || P14RCLogId == 0) {
            alert("Containment Action has taken");
            return false;
        }
        var status = 0;
        if (accp == 1) {
            status = 3;
        } else if (accp == 2){
            status = 4;
            if (P14Comment.length === 0) {
                var newNamevalidate = document.getElementById('P14Comment');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P14Comment');
                newNamevalidate.style.border = '';
            }
        }

        var formdata = {
            cont_RCA_CA_LogId: P14RCLogId,
            ncLogId: parseInt(P14NCLogId),
            containment_Action: P14Containt,
            senior_Feedback: P14Comment,
            cont_RCA_CA_Status_Id: status
        };
        api.post("/WorkOrder/PostCont_RCA_CA_log", formdata).then((data) => {
            alert("Feedback on Root Cause Analysis & Corrective Action Saved");
            $("#popup14").modal("hide");
            loadNCLog();
        }).catch((error) => {
            console.log(error);
        });
    });
    $('#doc-item').on('hidden.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'none';
        document.getElementById('popup14').style.filter = 'none';
        var InfoComments = document.getElementById('InfoComments');
        InfoComments.style.border = '';
        var newNamevalidate = document.getElementById('fileNameDisplay');
        newNamevalidate.style.border = '';
        $("#doclistidFile").val(0);
        $("#fileNameDisplay").val('');
        $("#InfoComments").val('');
        $("#DocTypeName").val('');
        $("#FileExtnName").val('');
        $("#docTypeIdFile").val(0);
        var fileInput = document.getElementById("fileUploadInput");
        fileInput.value = "";

    });
    $('#doc-item').on('show.bs.modal', function (event) {
        document.getElementById('popup10').style.filter = 'blur(5px)';
        document.getElementById('popup14').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var filename = relatedTarget.data("filename");
        var doctypename = relatedTarget.data("doctypename");
        var comments = relatedTarget.data("comments");
        var doclistid = relatedTarget.data("doclistid");
        var documenttypeid = relatedTarget.data("documenttypeid");
        var upload = relatedTarget.data("upload");
        var fileextnname = relatedTarget.data("fileextnname");
        var deletiondate = relatedTarget.data("deletiondate");
        if (doclistid == 0 && upload == 1) {
            $("#doclistidFile").val(0);
            $("#fileNameDisplay").val('');
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        } else if (doclistid > 0 && upload == 2) {
            $("#doclistidFile").val(doclistid);
            $("#fileNameDisplay").val(filename);
            $("#InfoComments").val(comments);
            $("#DocTypeName").val(doctypename);
            $("#FileExtnName").val(fileextnname);
            $("#docTypeIdFile").val(documenttypeid);
            var dateOnly = deletiondate.split('T')[0];

            // Set the value of the input field with the formatted date
            $("#deletiondate").val(dateOnly);
        }
        else {
            $("#DocTypeName").val("Others");
            $("#docTypeIdFile").val(3);
            $("#doclistidFile").val(0);
            $("#FileExtnName").val("Any Extn");
            var today = new Date();
            today.setDate(today.getDate() + 10);
            var deletionDate = today.toISOString().split('T')[0];
            $("#deletiondate").val(deletionDate);

        }

    });
    document.getElementById("UploadFileSave").addEventListener("click", function (event) {
        event.preventDefault();  // Prevent the default form submission
        var partids = parseInt($("#P14RCLogId").val());
        if (partids <= 0 || isNaN(partids)) {
            alert("Please Save The Containment Action taken & outcome.");
            return false;
        }
        var com = document.getElementById("InfoComments").value;
        if (com.trim() === "" || com.length === 1) {
            var newNamevalidate = document.getElementById('InfoComments');
            newNamevalidate.style.border = '2px solid red'; // Set border to red for invalid input
            return false; // Prevent form submission or further processing
        } else {
            var newNamevalidate = document.getElementById('InfoComments');
            newNamevalidate.style.border = ''; // Clear the border for valid input
        }
        // Create a FormData object to hold file and form data
        var formData = new FormData();

        // Add the file to the FormData object
        var fileInput = document.getElementById("fileUploadInput");
        var file = fileInput.files[0];
        var allowedExtensions = $("#FileExtnName").val().split(',').map(function (ext) {
            return ext.trim().toLowerCase(); // Create an array of allowed extensions (e.g., ['.pdf'])
        });
        if (!isNaN(file) || file) {
            var fileName = file.name;
            var fileExtension = '.' + fileName.split('.').pop().toLowerCase(); // Get the file extension and add a dot (e.g., '.pdf')

            // Validate that the file's extension is in the allowedExtensions array
            if (allowedExtensions.includes(fileExtension) || allowedExtensions.includes("any extn")) {
                formData.append("uploadedFile", file);
            } else {
                var newNamevalidate = document.getElementById('fileNameDisplay');
                newNamevalidate.style.border = '2px solid red';
                $("#fileNameDisplay").val("Invalid file type. Please upload a valid file."); // Show error message for invalid file type
                return false;
            }
        } else {
            var newNamevalidate = document.getElementById('fileNameDisplay');
            newNamevalidate.style.border = '2px solid red';
            return false;
        }

        // Add the other form inputs to the FormData object
        formData.append("DocumentTypeName", document.getElementById("DocTypeName").value);
        formData.append("FileExtnName", document.getElementById("FileExtnName").value);
        formData.append("FileName", document.getElementById("fileNameDisplay").value);
        formData.append("StorageLocation", "/Active");
        formData.append("Comments", document.getElementById("InfoComments").value);
        formData.append("DocListId", parseInt(document.getElementById("doclistidFile").value));
        formData.append("DocumentTypeId", parseInt(document.getElementById("docTypeIdFile").value));
        formData.append("PartId", parseInt(document.getElementById("P14PartId").value));
        formData.append("NcLogIdId", parseInt(document.getElementById("P14NCLogId").value));
        formData.append("UploadUiId", parseInt(1));
        //var today = new Date();
        //today.setDate(today.getDate() + 10);  // Add 10 days to the current date

        //// Format the date as YYYY-MM-DD (you can modify this to your required format)
        //var deletionDate = today.toISOString().split('T')[0];

        // Append the DeletionDate to the FormData
        var deletionDateValue = document.getElementById("deletiondate").value;
        var deletionDate = new Date(deletionDateValue);

        var formattedDate = deletionDate.toISOString().split('T')[0];

        formData.append("DeletionDate", formattedDate);
        // Post the form data to the server
        if (archive == 0) {
            $.ajax({
                type: "POST",
                url: "/masters/PostDocList",
                data: formData,
                contentType: false,  // Important: Let the browser set the Content-Type header automatically
                processData: false,  // Important: Don't process the form data, let it be as FormData
                success: function (response) {
                    // Handle the success response here
                    //alert("File uploaded and data saved successfully!");
                    $("#doc-item").modal("hide");
                    var rc = $("#P14NCLogId").val();
                    loadPartDocNC(rc);
                    archive = 0;
                },
                error: function (xhr, status, error) {
                    // Handle any errors here
                    console.error("An error occurred:", error);
                    alert("There was an error saving the file and data.");
                }
            });
        } else {
            $.ajax({
                type: "POST",
                url: "/masters/MoveFileToArchive",
                data: formData,
                contentType: false,  // Important: Let the browser set the Content-Type header automatically
                processData: false,  // Important: Don't process the form data, let it be as FormData
                success: function (response) {
                    // Handle the success response here
                    //alert("File uploaded and data saved successfully!");
                    $("#doc-item").modal("hide");
                    loadPartDocNC(partids);
                    archive = 0;
                },
                error: function (xhr, status, error) {
                    // Handle any errors here
                    console.error("An error occurred:", error);
                    alert("There was an error saving the file and data.");
                }
            });
        }
    });
});

function GetRcalog(nclogId) {
    api.getbulk("/workOrder/GetAllCont_RCA_CA_log").then((data) => {
        data = data.filter(item => item.ncLogId === parseInt(nclogId));
        $("#P10Containt").val(data[0].containment_Action);
        $("#P14Containt").val(data[0].containment_Action);
        $("#P14Comment").val(data[0].senior_Feedback);
        $("#P14RCLogId").val(data[0].cont_RCA_CA_LogId);
        if (data[0].cont_RCA_CA_Status_Id === 4) {
            $("#P10DivDescVm").hide();
            $("#P10NClvl1VM").hide();
            $("#P10DivInsDesc").hide();
            $("#P10DivDesc").hide();
        } else {
            $("#P10DivDescVm").show();
            $("#P10NClvl1VM").show();
            $("#P10DivInsDesc").show();
            $("#P10DivDesc").show();
        }
    }).catch((error) => {
        console.log(error);
    });
}
function loadPartDocNC(nclogId) {
    api.getbulk("/workOrder/NcDoclist?nclogId=" + parseInt(nclogId)).then((data) => {
        //data = data.filter(item => item.status == 1 || item.status == 0);
        var tablebody = $("#P14DocUploadGrid tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            let rowHtml = $(`
                <tr>
                    <td>${data[i].documentTypeName || ''}</td>
                    <td>${data[i].mandatory || ''}</td>
                    <td>${data[i].comments || ''}</td>
                    <td>${data[i].uploadedBy || ''}</td>
                    <td>${data[i].updatedOnStr || ''}</td>
                    <td>
                        <a href="javascript:void(0);" 
                           data-filename="${data[i].fileName || ''}"
          data-doctypename="${data[i].documentTypeName || ''}" 
                           data-customername="${data[i].customerName || ''}" 
                           data-partno="${data[i].partNo || ''}" 
                           data-partdesc="${data[i].partDesc || ''}" 
                           data-routingname="${data[i].routingName || ''}" 
                           data-retdate="${data[i].retDate || ''}" 
                           data-oprno="${data[i].oprNo || ''}"
                           onclick="viewFile(this)">
                            <i class="fas fa-eye btn btn-sm"></i>
                        </a>
                    </td>
                    <td>
                        <a href="javascript:void(0);" 
                           data-filename="${data[i].fileName || ''}" 
          data-doctypename="${data[i].documentTypeName || ''}" 
                           data-customername="${data[i].customerName || ''}" 
                           data-partno="${data[i].partNo || ''}" 
                           data-partdesc="${data[i].partDesc || ''}" 
                           data-routingname="${data[i].routingName || ''}" 
                           data-retdate="${data[i].retDate || ''}" 
                           data-oprno="${data[i].oprNo || ''}"
                           onclick="downloadFile(this)">
                            <i class="fas fa-download btn btn-sm"></i>
                        </a>
                    </td>
                      <td>
                            <div class="dropdown float-center">
                                <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                    <i class="mdi mdi-dots-vertical"></i>
                                </a>
                                <div class="dropdown-menu dropdown-menu-end">
                                    <a href="javascript:void(0);" class="dropdown-item upload-link"
            data-bs-toggle="modal"
                                       data-bs-target="#doc-item"
                                       data-filename="${data[i].fileName || ''}" 
                                       data-doctypename="${data[i].documentTypeName || ''}" 
                                       data-comments="${data[i].comments || ''}" 
                                       data-fileextnname="${data[i].fileExtnName || ''}" 
                                       data-documenttypeid="${data[i].documentTypeId || ''}" 
                                       data-upload="1" 
                                       data-deletiondate="${data[i].deletionDate || ''}" 
                                       data-doclistid="${data[i].docListId || ''}" 
                                       data-retdate="${data[i].retDate || ''}">Upload</a>
                                    <a href="javascript:void(0);" class="dropdown-item edit-link"
             data-bs-toggle="modal"
                                       data-bs-target="#doc-item"
                                       data-filename="${data[i].fileName || ''}" 
                                       data-doctypename="${data[i].documentTypeName || ''}" 
                                       data-comments="${data[i].comments || ''}" 
                                       data-fileextnname="${data[i].fileExtnName || ''}" 
                                       data-documenttypeid="${data[i].documentTypeId || ''}" 
                                       data-upload="2" 
                                       data-deletiondate="${data[i].deletionDate || ''}" 
                                       data-doclistid="${data[i].docListId || ''}" 
                                       data-retdate="${data[i].retDate || ''}">Edit</a>
                                    <a href="javascript:void(0);" class="dropdown-item delete-link" data-doclistid="${data[i].docListId || ''}"
                                       onclick="DeleteDocList(this)">Delete</a>
                                </div>
                            </div>
                        </td>
                </tr>
            `);
            if (data[i].docListId === 0) {
                rowHtml.find('.edit-link').remove(); // Remove Edit link
                rowHtml.find('.delete-link').remove(); // Remove Delete link
            }

            if (data[i].docListId !== 0) {
                rowHtml.find('.upload-link').remove(); // Remove Upload link
            }

            if (data[i].mandatory === 'Y') {
                rowHtml.find('.delete-link').remove(); // Remove Delete link for mandatory items
            }

            // Append the processed row to the table body
            $(tablebody).append(rowHtml);
        }

    }).catch((error) => {
        console.log(error);
    });
}
function loadNCLog() {
    var tablebody = $("#NcGrid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/workOrder/GetAllNcLog").then((data) => {
        data = data.filter(item => item.nC_Log_status_Id === 1);

        data.forEach((ncItem) => {
            const nclogId = ncItem.insp_Outcome_Details_Id;

            Promise.all([
                api.getbulk("/workOrder/GetAllCont_RCA_CA_log"),
                api.getbulk("/workOrder/GetAllNC_Decision_Log")
            ]).then(([rcaData, decisionData]) => {
                let dropdownHtml = '';

                const rcdata = rcaData.filter(item => item.ncLogId === parseInt(nclogId));
                const containmentActionLength = rcdata[0]?.containment_Action?.length || 0;
                const statusId = rcdata[0]?.cont_RCA_CA_Status_Id;

                const decData = decisionData.filter(item => item.ncLogId === parseInt(nclogId));
                const lvl1Decision = decData[0]?.nC_Disp_Deci_Lvl1_Id;

                // Main conditional logic
                if (containmentActionLength > 0 && statusId === 3) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item descision-level"
                       data-bs-toggle="modal"
                       data-balno="${ncItem.balloon_No}"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-divhide="Y"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-headid="${ncItem.inw_Recpt_Header_Id}" data-inspid="${ncItem.inw_Insp_Log_Id}"
                       data-bs-target="#popup10">Decision (Level 1)</a>`;
                } else if (containmentActionLength > 0 && statusId === 4) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item"
                       data-bs-toggle="modal"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-balno="${ncItem.balloon_No}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-divhide="Y"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-bs-target="#popup14">Upload RCA / CA</a>`;
                } else if (containmentActionLength > 0) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item"
                       data-bs-toggle="modal"
                       data-divhide="N"
                       data-balno="${ncItem.balloon_No}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-baldesc="${ncItem.balloon_No_Dir}"
                       data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-bs-target="#popup14">Check RCA / CA</a>`;
                } else {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item"
                       data-bs-toggle="modal"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-balno="${ncItem.balloon_No}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-divhide="Y"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-bs-target="#popup14">Upload RCA / CA</a>`;
                }

                // ✅ Show Approval (Level 2) if Lvl1 decision is 3 or 4
                if (lvl1Decision === 3 || lvl1Decision === 4) {
                    dropdownHtml += `
                    <a href="javascript:void(0);" class="dropdown-item approval-level"
                       data-bs-toggle="modal"
                       data-balno="${ncItem.balloon_No}"
                       data-partno="${ncItem.inw_Recpt_Part_No_Name}"
                       data-partid="${ncItem.inw_Recpt_Part_No_Id}"
                       data-parttype="${ncItem.partType}"
                       data-locnam="${ncItem.locationName}"
                       data-baldesc="${ncItem.balloon_No_Dir}" data-ncdes="${ncItem.nC_Descrip}" data-declsup="${ncItem.decl_by_Supplier}"
                       data-nctrack="${ncItem.nC_Tracking_No}" data-qnty="${ncItem.nC_Qnty}"
                       data-loc="${ncItem.storage_Location}" data-id="${ncItem.insp_Outcome_Details_Id}"
                       data-headid="${ncItem.inw_Recpt_Header_Id}" data-inspid="${ncItem.inw_Insp_Log_Id}"
                       data-bs-target="#popup10">Approval (Level 2)</a>`;
                }

                const rowHtml = `
                <tr>
                    <td>${ncItem.nC_Tracking_No}</td>
                    <td>${ncItem.ncDateStr}</td>
                    <td>${ncItem.locationName}</td>
                    <td>${ncItem.inw_Recpt_Part_No_Name}</td>
                    <td>${ncItem.feature_Descrip}</td>
                    <td>${ncItem.nC_Descrip}</td>
                    <td>${ncItem.partType}</td>
                    <td>${ncItem.nC_Qnty}</td>
                    <td>${ncItem.unit}</td>
                    <td>${ncItem.rcaStatus}</td>
                    <td>${ncItem.wfCustFeedBack}</td>
                    <td>${ncItem.lvldesc}</td>
                    <td>${ncItem.lvlAppro}</td>
                    <td>${ncItem.noOfDays}</td>
                    <td>
                        <div class="dropdown float-center">
                            <a href="#" class="dropdown-toggle arrow-none card-drop" data-bs-toggle="dropdown" aria-expanded="false">
                                <i class="mdi mdi-dots-vertical"></i>
                            </a>
                            <div class="dropdown-menu dropdown-menu-end">
                                ${dropdownHtml}
                            </div>
                        </div>
                    </td>
                </tr>
            `;

                $(tablebody).append(rowHtml);

            }).catch((error) => {
                console.error("Bulk data fetch error:", error);
            });
        });

        //loadSelectNCDispDecision();
    }).catch((error) => {
        console.error("NC Log fetch error:", error);
    });



}
function loadDetailsDocUploadList(nclogId) {

    if (isNaN(nclogId)) {
        api.getbulk("/workOrder/NcDoclist?nclogId=" + parseInt(nclogId)).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status==0);
            var tablebody = $("#P6DocUploadGrid tbody");
            $(tablebody).html("");//empty tbody
            //console.log(data);
            for (i = 0; i < data.length; i++) {
                let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"   
                   data-doctypename="${data[i].documentTypeName || ''}"
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
        </tr>
    `);
                // Append the processed row to the table body
                $(tablebody).append(rowHtml);
            }

        }).catch((error) => {
            console.log(error);
        });
    } else {
        api.getbulk("/workOrder/NcDoclist?nclogId=" + parseInt(nclogId)).then((data) => {
            //data = data.filter(item => item.status == 1 || item.status == 0);
            var tablebody = $("#P6DocUploadGrid tbody");
            $(tablebody).html("");//empty tbody
            //console.log(data);
            for (i = 0; i < data.length; i++) {
                let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}" 
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
        </tr>
    `);


                // Append the processed row to the table body
                $(tablebody).append(rowHtml);
            }

        }).catch((error) => {
            console.log(error);
        });

    }
}

function downloadFile(element) {
    var relatedTarget = $(element);
    var file = relatedTarget.data("filename");

    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/masters/ViewFile?fileName=' + file, true);
    xhr.responseType = 'arraybuffer';
    xhr.onload = function (e) {
        if (this.status == 200) {
            var blob = new Blob([this.response], { type: "application/pdf" });

            //const objectElement = document.getElementById('fileViewer');
            //const url = URL.createObjectURL(blob);
            //objectElement.src = url;
            //objectElement.width = '1000px';
            //objectElement.height = '1000px';
            //objectElement.type = 'text/plain';
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = file;
            link.click();
        }
    };
    xhr.send();
}

function viewFile(element) {
    var relatedTarget = $(element);
    var file = relatedTarget.data("filename");
    var doctypename = relatedTarget.data("doctypename");
    var customername = $("#CompanyId option:selected").text();
    var partno = $("#PartNo").val();
    var partdesc = $("#PartDescription").val();
    var routingname = relatedTarget.data("routingname");
    var oprno = relatedTarget.data("oprno");
    var retdate = relatedTarget.data("retdate");
    if (file == null || file == "") {
        $('#viewDoc').modal('hide');
        return false;
    }
    if (doctypename == "Other") {
        $('#viewDoc').modal('hide');
        return false;
    } else {
        $('#viewDoc').modal('show');
        $("#DocTypenameText").text(doctypename);
        $("#PartNoText").text(partno);
        $("#PartDescText").text(partdesc);
        $("#CustomerText").text(customername);
        $("#RetentionDateText").text(retdate);
        $("#RoutingNameText").text(routingname);
        $("#OprNoText").text(oprno);


        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/masters/ViewFile?fileName=' + file, true);
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
}
function loadPartDocInsp(inw_Recpt_HeaderId) {
    api.getbulk("/workOrder/InwardDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
        //data = data.filter(item => item.status == 1 || item.status == 0);
        var tablebody = $("#P6partInspectSuppGrid tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}" 
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
        </tr>
    `);

            // Append the processed row to the table body
            $(tablebody).append(rowHtml);
        }

    }).catch((error) => {
        console.log(error);
    });
}
function loadPartDoc(inw_Recpt_HeaderId) {
    api.getbulk("/workOrder/InwardDoclist?poheaderid=" + inw_Recpt_HeaderId).then((data) => {
        //data = data.filter(item => item.status == 1 || item.status == 0);
        var tablebody = $("#P6partInspectPlanGrid tbody");
        $(tablebody).html("");//empty tbody
        //console.log(data);
        for (i = 0; i < data.length; i++) {
            let rowHtml = $(`
        <tr>
            <td>${data[i].documentTypeName || ''}</td>
            <td>${data[i].mandatory || ''}</td>
            <td>${data[i].comments || ''}</td>
            <td>${data[i].uploadedBy || ''}</td>
            <td>${data[i].updatedOnStr || ''}</td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}"
  data-doctypename="${data[i].documentTypeName || ''}" 
                   data-customername="${data[i].customerName || ''}" 
                   data-partno="${data[i].partNo || ''}" 
                   data-partdesc="${data[i].partDesc || ''}" 
                   data-routingname="${data[i].routingName || ''}" 
                   data-retdate="${data[i].retDate || ''}" 
                   data-oprno="${data[i].oprNo || ''}"
                   onclick="viewFile(this)">
                    <i class="fas fa-eye btn btn-sm"></i>
                </a>
            </td>
            <td>
                <a href="javascript:void(0);" 
                   data-filename="${data[i].fileName || ''}" 
                   onclick="downloadFile(this)">
                    <i class="fas fa-download btn btn-sm"></i>
                </a>
            </td>
        </tr>
    `);

            // Append the processed row to the table body
            $(tablebody).append(rowHtml);
        }

    }).catch((error) => {
        console.log(error);
    });
}
function displayFileName() {
    var docid = parseInt($("#doclistidFile").val());
    if (docid != 0) {
        var doctypeid = parseInt($("#docTypeIdFile").val());
        api.get("/DocumentManagement/GetAllDocumentType").then((data) => {
            const fdoc = data.find(item => item.documentTypeId === doctypeid);
            if (fdoc.docuCategory == 2) {
                var confrimval = confirm("Do You Want Move the Exsisting File To Archive.");
                if (confrimval) {
                    var fileInput = document.getElementById('fileUploadInput');
                    var fileName = fileInput.files[0].name; // Get the uploaded file name
                    document.getElementById('fileNameDisplay').value = fileName; // Display the file name in the text input
                    var docid = parseInt($("#doclistidFile").val());
                    archive = 1;
                    $("#Resonbtn").click();
                } else {
                    var fileInput = document.getElementById("fileUploadInput");
                    fileInput.value = "";
                }
            } else {
                var fileInput = document.getElementById('fileUploadInput');
                var fileName = fileInput.files[0].name; // Get the uploaded file name
                document.getElementById('fileNameDisplay').value = fileName;
            }
        }).catch((error) => {
        });
    } else {
        var fileInput = document.getElementById('fileUploadInput');
        var fileName = fileInput.files[0].name; // Get the uploaded file name
        document.getElementById('fileNameDisplay').value = fileName;
    }
}