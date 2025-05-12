
function loadPO() {
    api.getbulk("/WorkOrder/GetAllInw_Recpt_HeaderInsp").then((data) => {
        data = data.filter(item => item.status >= 3);
        let totalSubCon = 0;
        let totalRawMaterial = 0;
        let totalBOF = 0;

        for (let i = 0; i < data.length; i++) {
            if (data[i].partType === "SubCon") {
                totalSubCon++; // Adjust based on the actual property name
            } else if (data[i].partType === "RawMaterial") {
                totalRawMaterial++;
            } else if (data[i].partType === "BOF") {
                totalBOF++;
            }
        }

        console.log("Total SubCon:", totalSubCon);
        console.log("Total RawMaterial:", totalRawMaterial);
        console.log("Total BOF:", totalBOF);

        // Optionally, display these totals in the UI
        $("#subconInsp").text(totalSubCon);
        $("#rmInsp").text(totalRawMaterial);
        $("#bofInsp").text(totalBOF);
    }).catch((error) => {
        console.error("Error fetching data:", error);
    });
}

function InwardPo() {
    api.getbulk("/WorkOrder/GetAllPodetails").then((data) => {
        data = data.filter(item => item.status >= 2);
        let totalSubCon = 0;
        let totalRawMaterial = 0;
        let totalBOF = 0;

        for (let i = 0; i < data.length; i++) {
            if (data[i].partType === "ManufacturedPart") {
                totalSubCon++; // Adjust based on the actual property name
            } else if (data[i].partType === "RawMaterial") {
                totalRawMaterial++;
            } else if (data[i].partType === "BOF") {
                totalBOF++;
            }

        }
        $("#subconInw").text(totalSubCon);
        $("#rmInw").text(totalRawMaterial);
        $("#bofInw").text(totalBOF);

    }).catch((error) => {
    });
}

function loadNCLog() {
    var tablebody = $("#NcGrid tbody");
    $(tablebody).html("");//empty tbody

    api.getbulk("/workOrder/GetAllNcLog").then((data) => {
        let totalInhouse = 0;
        let totalSubCon = 0;
        let totalRawMaterial = 0;
        let totalBOF = 0;

        for (let i = 0; i < data.length; i++) {
            if (data[i].partType === "SubCon") {
                totalSubCon++; 
            } else if (data[i].partType === "RawMaterial") {
                totalRawMaterial++;
            } else if (data[i].partType === "BOF") {
                totalBOF++;
            }
        }
        $("#ncInhouse").text(totalInhouse);
        $("#ncSubCon").text(totalSubCon);
        $("#ncRm").text(totalRawMaterial);
        $("#ncBof").text(totalBOF);
    }).catch((error) => {
    });
}

$(document).ready(function () {
    InwardPo();
    loadNCLog();
    loadPO();
});