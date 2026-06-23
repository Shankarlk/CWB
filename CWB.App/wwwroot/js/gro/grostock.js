$(document).ready(function () {

    loadGroPartWithStock();
    loadGroPartWithoutStock();

});
function loadGroPartWithStock() {

    api.getbulk("/Gro/GetGroPartWithStock").then((data) => {

        var tablebody = $("#tblWithStock tbody");
        $(tablebody).html("");

        if (data.length === 0) {

            $(tablebody).append(`
                <tr class="norecordsfound">
                    <td colspan="3" class="text-center text-muted">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`);

            return;
        }

        for (var i = 0; i < data.length; i++) {

            $(tablebody).append(AppUtil.ProcessTemplateData("groPartWithStockRow", data[i]));

        }

    }).catch((error) => {

    });

}
function loadGroPartWithoutStock() {

    api.getbulk("/Gro/GetGroPartWithoutStock").then((data) => {

        var tablebody = $("#tblZeroStock tbody");
        $(tablebody).html("");

        if (data.length === 0) {

            $(tablebody).append(`
                <tr class="norecordsfound">
                    <td colspan="2" class="text-center text-muted">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`);

            return;
        }

        for (var i = 0; i < data.length; i++) {

            $(tablebody).append(AppUtil.ProcessTemplateData("groPartWithoutStockRow", data[i]));

        }

    }).catch((error) => {

    });

}
function LoadUpdateStock(ctrl) {

    var tr = $(ctrl).closest("tr");

    $("#hdnGroPartListId").val(tr.find("td:eq(0)").text().trim());
    $("#txtGroPartNo").val(tr.find("td:eq(1)").text().trim());
    $("#txtCurrentStock").val(tr.find("td:eq(2)").text().trim());

}