var MachieListUtil = {
    LoadPlants: () => {
        api.get("/plant/getplants/").then((data) => {
            var plantSelect = $("#search-machine-plant");
            $(plantSelect).html("");
            $(plantSelect).append('<option value="">--Select Plant--</option>');
            for (i = 0; i < data.length; i++) {
                $(plantSelect).append('<option value="' + data[i].plantId + '">' + data[i].name + '</option>');
            }

        }).catch((error) => {

        });
    },
    LoadShop: (plantId) => {
        if (plantId == "") {
            var plantSelect = $("#search-machine-shop");
            $(plantSelect).html("");
            $(plantSelect).append('<option value="">--Select Shop--</option>');
            return;
        }
        api.get("/department/getdepartments/" + plantId).then((data) => {
            var plantSelect = $("#search-machine-shop");
            $(plantSelect).html("");
            $(plantSelect).append('<option value="">--Select Shop--</option>');
            for (i = 0; i < data.length; i++) {
                $(plantSelect).append('<option value="' + data[i].departmentId + '">' + data[i].name + '</option>');
            }

        }).catch((error) => {

        });
    },
    LoadMachineList: () => {
        api.get("/Machine/GetMachines").then((data) => {
            var tablebody = $("#tbl-machine-list tbody");
            $(tablebody).html("");//empty tbody
            if (data.length === 0) {
                // 2. Insert the "No Records Found" row
                // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $(tablebody).append(noRecordsRow);
            }
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateData("machinelist-template", data[i]));
            }
            //filter once loaded
            MachieListUtil.FilterMachineList();
        }).catch((error) => {

        });
    },
    FilterMachineList: () => {
        var searchObject = {};
        $(".machine-list-search").each(function () {
            var val = "";
            if ($(this).get(0).tagName.toLocaleLowerCase()!="select") {
                val = $.trim($(this).val());
            } else {
                if ($.trim($(this).val()).length != 0) {
                    val = $.trim($(this).find("option:selected").text());
                }                
            }
            
            if (val.length != 0) {
                searchObject[$(this).data("key")] = val.toUpperCase();
            }
        });
        AppUtil.TableFilter("tbl-machine-list", searchObject);
    }
};
$(function () {
   
    $(".machine-list-search").change(function () {
        MachieListUtil.FilterMachineList();
    });

    $("#search-machine-plant").change(function () {
        MachieListUtil.LoadShop($(this).val());
    });
});