// JavaScript source code
$(function () {
    console.log("Ready");


    $('#routing-new').on('hide.bs.modal', function (event) {
        window.location.href = "/routings/routingdetails";
    });

    $('#routing-new').on('show.bs.modal', function (event) {
       // alert("New Routing...");
    });

    $("#BtnNewRouting").click(function (event) {
        //routings/addnewrouting
        api.get("/routings/addnewrouting").then((data) => {
            //console.log(data);
            document.getElementById("btnNewRoutingClose").click();
        }).catch((error) => {

        });
    });

});