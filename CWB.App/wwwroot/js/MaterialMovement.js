
$(document).ready(function () {
    loadInvLog();
    document.getElementById("NcGridChk").addEventListener("change", function () {
        let isChecked = this.checked;
        document.querySelectorAll(".row-checkbox").forEach(checkbox => {
            checkbox.checked = isChecked;
        });
    });
    $("#searchFromSend").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#MoveGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchToSend").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#MoveGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#searchPartType").on("change", function () {
        var value = $(this).val().toLowerCase();
        var selectedText = $("#searchPartType option:selected").text().trim().toLowerCase();
        if (value != "0") {
            $("#MoveGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(selectedText) > -1)
            });
        } else {
            $("#MoveGrid tbody tr").show();
        }
    });

    $('#ErrorMessage10').on('show.bs.modal', function (event) {
        document.getElementById('popup17').style.filter = 'blur(5px)';
    });
    $('#ErrorMessage10').on('hidden.bs.modal', function (event) {
        document.getElementById('popup17').style.filter = 'none';
    });
    $('#popup17').on('show.bs.modal', function (event) {
        let selectedRows = [];
        document.querySelectorAll(".row-checkbox:checked").forEach(checkbox => {
            let row = checkbox.closest("tr");
            let rowData = {
                partNo: row.cells[1].innerText,
                routing: row.cells[2].innerText,
                from: row.cells[3].innerText,
                to: row.cells[4].innerText,
                status: row.cells[5].innerText,
                quantity: row.cells[6].innerText,
                logid: row.cells[10].innerText,
                misstatus: row.cells[9].innerText
            };
            selectedRows.push(rowData);
        });
        let tbody = document.querySelector("#P17Grid tbody");
        tbody.innerHTML = "";

        selectedRows.forEach(data => {
            let newRow = `<tr>
            <td>${data.partNo}</td>
            <td>${data.routing}</td>
            <td>${data.from}</td>
            <td>${data.to}</td>
            <td>${data.status}</td>
            <td>${data.quantity}</td>
            <td hidden>${data.logid}</td>
            <td hidden>${data.misstatus}</td>
            <td></td>
        </tr>`;
            tbody.innerHTML += newRow;
        });

    });
    $('#popup16').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var relatedTarget = $(event.relatedTarget);
        var fromsend = relatedTarget.data("fromsend");
        var tosend = relatedTarget.data("tosend");
        var rout = relatedTarget.data("rout");
        var opr = relatedTarget.data("opr");
        var partstatus = relatedTarget.data("partstatus");
        var partid = relatedTarget.data("partid");
        var partno = relatedTarget.data("partno");
        var qntymis = relatedTarget.data("qntymis");
        var qntycom = relatedTarget.data("qntycom");
        var qnty = relatedTarget.data("qnty");
        var id = relatedTarget.data("id");
        var newNamevalidate = document.getElementById('P16Comment2');
        newNamevalidate.style.border = '';
        $("#P16Comment2").val(qntycom);
        $("#P16MisQnty").val(qntymis);
        $("#P16PartNo").text(partno);
        $("#P16FromSend").text(fromsend);
        $("#P16ToSend").text(tosend);
        $("#P16SendDpt").text(fromsend);
        $("#P16RecDpt").text(tosend);
        $("#P16Routing").text(rout);
        $("#P16OprNo").text(opr);
        $("#P16PartStatus").text(partstatus);
        $("#P16SpanQnty").text(qnty);
        $("#P16id").val(id);
        var qntyad = qnty - qntymis;
        $("#P16QntyRec").val(qntyad);
        $("#CommentFieldDiv").hide();
        $("#CommentDiv").hide();
    });
    $("#P16QntyRec").keyup(function () {
        let value = $(this).val();
        if (!$.isNumeric(value)) {
            $(this).css("border", "2px solid red");
            $("#Unit2").text("");
        } else {
            var qntsent = parseInt($("#P16SpanQnty").text());
            var misqnty = qntsent - value;
            $("#P16MisQnty").val(misqnty);
            if (misqnty > 0) {
                $("#Unit2").text("Less");
            }else if (misqnty < 0) {
                $("#Unit2").text("Extra");
            }
            $(this).css("border", "");
        }
    });
    $("#P17Save").on("click", function () {
        let P17Data = [];

        $("#P17Grid tbody tr").each(function () {
            let row = $(this).find("td");
            var tet = row.eq(9).text().trim();
            if (row.length > 0) { // Skip empty header rows
                let rowData = {
                    partNo: row.eq(0).text().trim(),
                    routing: row.eq(1).text().trim(),
                    from: row.eq(2).text().trim(),
                    to: row.eq(3).text().trim(),
                    status: row.eq(4).text().trim(),
                    quantity: parseInt(row.eq(5).text().trim()) || 0,
                    inlogId: parseInt(row.eq(6).text().trim()) || 0
                };
                P17Data.push(rowData);
            }
        });
        if (P17Data.length === 0) {
            alert("Select the Row from the Material Movement / Receipt Confirmation table.");
            return;
        } else {
            $("#ErrorMessage10").modal("show");
            //console.log(P17Data);
        }
    });
    $("#ErP9Edit").on("click", function () {
        $("#ErrorMessage9").modal("hide");
    });
    $("#ErP10Return").on("click", function () {
        $("#ErrorMessage10").modal("hide");
    });
    $("#ErP10Edit").on("click", function () {
        let P17Data = [];

        $("#P17Grid tbody tr").each(function () {
            let row = $(this).find("td");
            var tet = row.eq(9).text().trim();
            if (row.length > 0) { // Skip empty header rows
                let rowData = {
                    partNo: row.eq(0).text().trim(),
                    routing: row.eq(1).text().trim(),
                    from: row.eq(2).text().trim(),
                    to: row.eq(3).text().trim(),
                    status: row.eq(4).text().trim(),
                    quantity: parseInt(row.eq(5).text().trim()) || 0,
                    inlogId: parseInt(row.eq(6).text().trim()) || 0,
                    misstatus: row.eq(7).text().trim() 
                };
                P17Data.push(rowData);
            }
        });
        if (P17Data.length === 0) {
            alert("Select the Row from the Material Movement / Receipt Confirmation table.");
            return;
        } else {
            $("#ErrorMessage10").modal("hide");
            $("#popup17").modal("hide");
            for (var i = 0; i < P17Data.length; i++) {
                //if (P17Data[i].misstatus === "None") {
                    var formdata = {
                        inv_Trans_LogId: parseInt(P17Data[i].inlogId),
                        qnty_Mismatch: parseInt(0),
                        qnty_mismatch_status: 1,
                        movement_Compl: "Y",
                        qnty_Mismatch_Comment: ""
                    };
                    api.post("/WorkOrder/UpdateInv_Trans_Log", formdata).then((data) => {
                    }).catch((error) => {
                        console.log(error);
                    });
                //}
                if (P17Data.length === i) {
                    loadInvLog();
                    $("#ErrorMessage10").modal("hide");
                    $("#popup17").modal("hide");
                }
            }
            //console.log(P17Data);
        }
    });
    $("#P16Save").on("click", function () {
        var P16id = $("#P16id").val();
        var P16MisQnty = $("#P16MisQnty").val();
        var P16Comment2 = $("#P16Comment2").val().trim();
        var move = "Y";
        var status = 0;
        if (parseInt(P16MisQnty)!==0) {
            if (P16Comment2.length === 0) {
                var newNamevalidate = document.getElementById('P16Comment2');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P16Comment2');
                newNamevalidate.style.border = '';
            }
            if (parseInt(P16MisQnty) >= 0) {
                status = 2;
            } else {
                status = 3;
            }
            $("#ErrorMessage9").modal("show");
        } else if (parseInt(P16MisQnty) === 0) {
            status = 1;
            move = "N";
            var formdata = {
                inv_Trans_LogId: parseInt(P16id),
                qnty_Mismatch: parseInt(P16MisQnty),
                qnty_mismatch_status: status,
                movement_Compl: move,
                qnty_Mismatch_Comment: P16Comment2
            };
            api.post("/WorkOrder/UpdateInv_Trans_Log", formdata).then((data) => {
                loadInvLog();
                $("#popup16").modal("hide");
                $("#ErrorMessage9").modal("hide");
            }).catch((error) => {
                console.log(error);
            });
        }

        //var formdata = {
        //    inv_Trans_LogId: parseInt(P16id),
        //    qnty_Mismatch: parseInt(P16MisQnty),
        //    qnty_mismatch_status: status,
        //    movement_Compl: move,
        //    qnty_Mismatch_Comment: P16Comment2
        //};
        //api.post("/WorkOrder/UpdateInv_Trans_Log", formdata).then((data) => {
        //    loadInvLog();
        //    $("#popup16").modal("hide");
        //}).catch((error) => {
        //    console.log(error);
        //});
    });
    $("#ErP9Return").on("click", function () {
        var P16id = $("#P16id").val();
        var P16MisQnty = $("#P16MisQnty").val();
        var P16Comment2 = $("#P16Comment2").val().trim();
        var move = "Y";
        var status = 0;
        if (parseInt(P16MisQnty)!==0) {
            if (P16Comment2.length === 0) {
                var newNamevalidate = document.getElementById('P16Comment2');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('P16Comment2');
                newNamevalidate.style.border = '';
            }
            if (parseInt(P16MisQnty) >= 0) {
                status = 2;
            } else {
                status = 3;
            }
            //$("#ErrorMessage9").modal("show");
        } else if (parseInt(P16MisQnty) === 0) {
            status = 1;
            move = "N";
        }

        var formdata = {
            inv_Trans_LogId: parseInt(P16id),
            qnty_Mismatch: parseInt(P16MisQnty),
            qnty_mismatch_status: status,
            movement_Compl: move,
            qnty_Mismatch_Comment: P16Comment2
        };
        api.post("/WorkOrder/UpdateInv_Trans_Log", formdata).then((data) => {
            loadInvLog();
            $("#popup16").modal("hide");
            $("#ErrorMessage9").modal("hide");
        }).catch((error) => {
            console.log(error);
        });
    });

});
function loadInvLog() {
    var tablebody = $("#MoveGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllInv_Trans_Log").then((data) => {
        data = data.filter(item => item.movement_Compl === "Y");
        for (i = 0; i < data.length; i++) {
            //var tBody = ProcessTemplateData("lineDocRow", data[i]);
            $(tablebody).append(AppUtil.ProcessTemplateData("MoveGridRow", data[i]));
            //$(tablebody).append(tBody);
            //console.log(tBody);
            const updateMultipleOption = document.querySelectorAll('.dropdown-item[data-bs-target="#popup17"]');

            if (data.length > 1) {
                updateMultipleOption.forEach(option => option.style.display = "block"); // Show
            } else {
                updateMultipleOption.forEach(option => option.style.display = "none"); // Hide
            }
        }
        loadSelectNCDispDecision();
    }).catch((error) => {
    });
}