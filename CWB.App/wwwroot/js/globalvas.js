//For loading Company / Supplier
let newCo = "";
let newCoId = "";
let CURRENT_TAB = "TabMPMain";
let CURDLG = "";

$(function () {
    $('.select2').each(function () {
        $(this).select2({ dropdownParent: $(this).parent() });
    })
});