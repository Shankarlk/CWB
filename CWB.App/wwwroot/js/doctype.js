$(function () {

    $("#doctypename").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#DocumentTypeTable tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#BtnSaveDocumentType").click(function (event) {
        var formData = AppUtil.GetFormData("DocTypeForm");
        api.post("/documenttype/doctype", formData).then((data) => {
            console.log(data);
            document.getElementById("btn-doctypedetails-close").click();
            document.getElementById("DocTypeForm").reset();
            loadDocumentTypes();
        }).catch((error) => {
            AppUtil.HandleError("DocTypeForm", error);
        });
    });

    function loadDocumentTypes() {
        //DocumentTypeTable
        //DocumentTypeTemplate
        var tablebody = $("#DocumentTypeTable tbody");
        $(tablebody).html("");//empty tbody
        //PlantRowTemplate
        //PlantTable
        api.getbulk("/documenttype/getdoctypes").then((data) => {
            //console.log(data);
            for (i = 0; i < data.length; i++) {
                for (var key in data[i]) {
                    console.log(key);
                    console.log(data[i][key]);
                    console.log("*****");
                }
                console.log("================");
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("DocumentTypeTemplate", data[i], i));
            }
        }).catch((error) => {
        });
    };
    loadDocumentTypes();
    
});

