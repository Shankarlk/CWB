$(function () {

    $("#docname").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PlantTable tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#BtnSavePlant").click(function (event) {
        var formData = AppUtil.GetFormData("PlantForm");
        api.post("/plant/plant", formData).then((data) => {
            console.log(data);
            document.getElementById("btn-shopdetails-close").click();
            document.getElementById("PlantForm").reset();
            loadPlants();
        }).catch((error) => {
            AppUtil.HandleError("PlantForm", error);
        });
    });

    function loadPlants() {
        var tablebody = $("#PlantTable tbody");
        $(tablebody).html("");//empty tbody
        //PlantRowTemplate
        //PlantTable
        api.getbulk("/plant/getplants").then((data) => {
            //console.log(data);
            for (i = 0; i < data.length; i++) {
                for (var key in data[i]) {
                    console.log(key);
                    console.log(data[i][key]);
                    console.log("*****");
                }
                console.log("================");
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("PlantRowTemplate", data[i], i));
            }
        }).catch((error) => {
        });
    };
    loadPlants();
});

function test() {
    var tablebody = $("#mptable tbody");
    $(tablebody).html("");//empty tbody
    //UpdatePurchaseDetailsTableFromPostData
    let i = 0;
    if (dataMPDList.length > 2) {
        let data = dataMPDList;
        for (i = 0; i < data.length; i++) {
            if (!(data[i]['masterPartType'] == partType))
                continue;
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("MasterDetaiTemplate", data[i], i));
        }
    }
    else {
        api.getbulk("/masters/masterparts").then((data) => {
            //console.log(data);
            dataMPDList = data;
            for (i = 0; i < data.length; i++) {
                for (var key in data[i]) {
                    console.log(key);
                    console.log(data[i][key]);
                    console.log("*****");
                }
                console.log(partType);
                console.log("================");
                if (!(data[i]['masterPartType'] == partType))
                    continue;
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("MasterDetaiTemplate", data[i], i));
            }
        }).catch((error) => {
        });
    }
}