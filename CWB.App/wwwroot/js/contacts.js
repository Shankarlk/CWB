var ContactsUtil = {
    FilterContacts: () => {
        var searchObject = {};
        $(".contact-search").each(function () {
            var val = $.trim($(this).val())
            if (val.length != 0) {
                searchObject[$(this).data("key")] = val.toUpperCase();
            }
        });
        AppUtil.TableFilter("tbl-contacts", searchObject);
    }
}
$(function () {
    $(".contact-search").change(function () {
        ContactsUtil.FilterContacts();
    });
    $(".contact-divi-search").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbl-contacts tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#tbl-contacts tbody");
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
    $(".contact-Location-search").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbl-contacts tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#tbl-contacts tbody");
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
    $(".contact-Name-search").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbl-contacts tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#tbl-contacts tbody");
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
});