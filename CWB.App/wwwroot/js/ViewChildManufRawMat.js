let documenttype = "Part Drawing";
function loadCustomers(CompanyOrSupplier) {//pass the element name
    var compSelect = $('#' + CompanyOrSupplier);//should be a select2 dropdown
    compSelect.empty();
    customers = {};
    ////debugger;
    var div_data = "<option value='0'>-Select-</option>";
    compSelect.append(div_data);
    api.get("/masters/Companies").then((data) => {
        customers = data;
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" +
                data[i].companyName + "'>" +
                data[i].companyName +
                "</option>";
            compSelect.append(div_data);
        }
    }).catch((error) => {
        //console.log(error);
    });
}
function loadEditParts() {
    var tablebody = $("#grid1 tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/ItemMasterInfo/masterparts").then((data) => {
        data = data.filter(item => item.masterDisplay === "Child Manufactured Part");
        for (i = 0; i < data.length; i++) {

            var tBody = AppUtil.ProcessTemplateData("grid1Row", data[i]);
            $(tablebody).append(tBody);

        }
    }).catch((error) => {
    });
}


$(function () {
    loadCustomers('searchCustomer');
    loadEditParts();
    $("#searchPartNo").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#grid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchPartDesc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#grid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchCustomer").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#grid1 tbody tr").show();
            return;
        }
        var selvallowc = selectedValue.toLowerCase();
        $("#grid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(selvallowc) > -1)
        });
    });
    $("#searchPartType").on("change", function () {
        var selectedValue = $(this).val();
        if (selectedValue == "0") {
            $("#grid1 tbody tr").show();
            return;
        }
        var selvallowc = selectedValue.toLowerCase();
        $("#grid1 tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(selvallowc) > -1)
        });
    });
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
        api.getbulk("/DocumentManagement/GetAllDocList").then((data) => {
            data = data.filter(item => item.partId === partid && item.documentTypeName === documenttype);
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
        $("#SelectDocType").hide();
        $("#BtnViewDoc").hide();
    });
    $("#BtnViewDoc").on("click", function () {
        var documenttype = $("#SelectDocType").val();
        if (parseInt(documenttype) == 0) {
            var newNamevalidate = document.getElementById('SelectDocType');
            newNamevalidate.style.border = '2px solid red';
            return false;
        }
    });
});
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
function ShowGrid2(element) {
    var relatedTarget = $(element);
    var partid = relatedTarget.data("partid");
    var partno = relatedTarget.data("partno");
    var partdesc = relatedTarget.data("partdesc");
    var parttype = relatedTarget.data("parttype");
    $("#spanPartNo").text(partno);
    $("#spanPartDesc").text(partdesc);
    loadPartsGrid2(partid, parttype);
}
function loadPartsGrid2(partid, parttype) {
    var tablebody = $("#inspGrid tbody");
    $(tablebody).html(""); // empty tbody

    api.getbulk("/ItemMasterInfo/RawMatlOfCmp?partid=" + partid).then((data) => {

        // First, get all document info so we can check for Part Drawing PDFs
        api.getbulk("/DocumentManagement/GetAllDocList").then((docs) => {

            for (let i = 0; i < data.length; i++) {
                let part = data[i];

                // Find if this part has a Part Drawing PDF
                let hasPartDrawingPDF = docs.some(doc =>
                    doc.partId === part.partId &&
                    doc.documentTypeName === "Part Drawing" &&
                    doc.fileName?.toLowerCase().endsWith(".pdf")
                );

                // Add a flag so template knows whether to show the eye icon
                part.showEye = hasPartDrawingPDF ? "yes" : "no";
                part.showMenu = (part.masterPartType !== "Child Manufactured Part") ? "yes" : "no";

                // Render row
                let tBody = AppUtil.ProcessTemplateData("grid2Row", part);
                $(tablebody).append(tBody);
            }

            // Remove eye buttons for rows where showEye = no
            $("#inspGrid tbody tr").each(function () {
                let rowData = data[$(this).index()];
                if (rowData.showEye === "no") {
                    $(this).find("td a[data-bs-target='#viewDoc']").remove();
                }
                if (rowData.showMenu === "no") {
                    $(this).find("td .dropdown").remove();
                }
            });

        });

    }).catch((error) => {
        console.error(error);
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
        if (partType == "Assembly") {
            data = data.filter(item => item.contentId === 2);
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
            data = data.filter(item => item.contentId === 4 || item.contentId == 5);
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