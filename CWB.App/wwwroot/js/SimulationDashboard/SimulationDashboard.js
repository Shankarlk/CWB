$(document).ready(function () {

    loadSimulation();
    $("#txtPartNo, #txtMachine").on("keyup", function () {
        filterSimulation();
    });


});


var simulationData = null;


function loadSimulation() {

    api.getbulk("/WorkOrder/SimulationData")

        .then((data) => {

            simulationData = data;
            bindSimulation(data);

        })

        .catch((error) => {

            console.log(error);

        });

}
function bindSimulation(data) {
    bindHeader(data.headers);

    var tableBody = $("#simulationBody");

    tableBody.html("");

    if (data.parts.length == 0) {

        tableBody.append(`
            <tr>
                <td colspan="74" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    for (var i = 0; i < data.parts.length; i++) {

        // Build all operation bars for this part
        var operationsHtml = "";

        for (var j = 0; j < data.parts[i].operations.length; j++) {

            data.parts[i].operations[j].top = 6 + (j * 28);

            operationsHtml += AppUtil.ProcessTemplateData(
                "operationTemplate",
                data.parts[i].operations[j]
            );
        }
        var rowHeight =
            Math.max(70, data.parts[i].operations.length * 30);
        // Build the row
        var row = {

            partNo: data.parts[i].partNo,

            urgent: data.parts[i].isUrgent ? "Y" : "N",

            operations: operationsHtml,
            rowHeight: rowHeight
        };

        tableBody.append(

            AppUtil.ProcessTemplateData(
                "simulationRow",
                row
            )

        );

    }

}
function filterSimulation() {

    var partNo = $("#txtPartNo").val().toLowerCase().trim();

    var machine = $("#txtMachine").val().toLowerCase().trim();

    // both empty -> show everything
    if (partNo === "" && machine === "") {

        bindSimulation(simulationData);
        return;
    }

    var filteredParts = simulationData.parts.map(function (part) {

        var operations = part.operations;

        if (machine !== "") {

            operations = operations.filter(function (op) {

                return op.machineName.toLowerCase().includes(machine);

            });

        }

        var partMatch = partNo === "" || part.partNo.toLowerCase().includes(partNo);

        if (!partMatch)
            return null;

        if (machine !== "" && operations.length === 0)
            return null;

        return {

            ...part,
            operations: operations

        };

    })
        .filter(function (x) {
            return x != null;
        });

    bindSimulation({

        headers: simulationData.headers,
        parts: filteredParts

    });

}
function bindHeader(headers) {

    $("#dateHeader").html("");
    $("#shiftHeader").html("");
    $("#hourHeader").html("");

    // Fixed columns
    $("#dateHeader").append('<th rowspan="3" style="min-width:160px;">Part No</th>');
    $("#dateHeader").append('<th rowspan="3" style="min-width:80px;">Urgent</th>');

    for (var i = 0; i < headers.length; i++) {

        var header = headers[i];

        var totalHours = 0;

        for (var j = 0; j < header.shifts.length; j++) {
            totalHours += header.shifts[j].hours.length;
        }

        $("#dateHeader").append(`
            <th colspan="${totalHours}">
                ${new Date(header.date).toLocaleDateString('en-GB')}
            </th>
        `);

        for (var j = 0; j < header.shifts.length; j++) {

            var shift = header.shifts[j];

            if (shift.hours.length == 0)
                continue;

            var color = "bg-primary text-white";

            if (shift.shiftName == "2nd Shift")
                color = "bg-warning";

            if (shift.shiftName == "3rd Shift")
                color = "bg-success text-white";

            $("#shiftHeader").append(
                ` <th colspan="${shift.hours.length}" class="${color}">
                    ${shift.shiftName}
                </th>`
            );

            for (var k = 0; k < shift.hours.length; k++) {

                var hour = shift.hours[k];

                if (hour == 0)
                    hour = 12;

                if (hour > 12)
                    hour = hour - 12;

                $("#hourHeader").append(
                    `<th>${hour}</th>`
                );
            }
        }
    }

}