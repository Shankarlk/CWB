var simulationData = null;

$(document).ready(function () {

    loadSimulation();

    $("#txtMachine, #txtPartNo").on("keyup", function () {

        filterSimulation();

    });

});


function loadSimulation() {

    api.getbulk("/WorkOrder/SimulationMachineData")

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

    if (data.machines.length == 0) {

        tableBody.append(`
            <tr>
                <td colspan="74" class="text-center">
                    No Records Found
                </td>
            </tr>`);

        return;
    }

    var totalHours = 0;

    for (var i = 0; i < data.headers.length; i++) {

        for (var j = 0; j < data.headers[i].shifts.length; j++) {

            totalHours += data.headers[i].shifts[j].hours.length;

        }

    }

    var totalWidth = totalHours * 37.5;

    for (var i = 0; i < data.machines.length; i++) {

        var operationsHtml = "";

        for (var j = 0; j < data.machines[i].operations.length; j++) {

            data.machines[i].operations[j].top = 6 + (j * 28);

            operationsHtml += AppUtil.ProcessTemplateData(

                "operationTemplate",

                data.machines[i].operations[j]

            );

        }

        var rowHeight = Math.max(70,

            data.machines[i].operations.length * 30);

        var row = {

            machineNo: data.machines[i].machineNo,

            operations: operationsHtml,

            rowHeight: rowHeight,

            totalHours: totalHours,

            totalWidth: totalWidth

        };

        tableBody.append(

            AppUtil.ProcessTemplateData(

                "simulationRow",

                row)

        );

    }

}

function filterSimulation() {

    var machine = $("#txtMachine").val().toLowerCase().trim();

    var part = $("#txtPartNo").val().toLowerCase().trim();

    if (machine == "" && part == "") {

        bindSimulation(simulationData);

        return;

    }

    var filteredMachines = simulationData.machines.map(function (mc) {

        var operations = mc.operations;

        if (part != "") {

            operations = operations.filter(function (op) {

                return op.partNum

                    .toLowerCase()

                    .includes(part);

            });

        }

        var machineMatch = machine == "" || mc.machineNo.toLowerCase().includes(machine);

        if (!machineMatch)

            return null;


        if (part != "" && operations.length == 0)

            return null;

        return { ...mc, operations: operations };

    })

        .filter(function (x) {

            return x != null;

        });

    bindSimulation
        ({

            headers: simulationData.headers,

            machines: filteredMachines

        });



}
function bindHeader(headers) {

    $("#dateHeader").html("");
    $("#shiftHeader").html("");
    $("#hourHeader").html("");

    $("#dateHeader").append('<th rowspan="3" style="min-width:160px;">Machine</th>');

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

            $("#shiftHeader").append(`
                <th colspan="${shift.hours.length}" class="${color}">
                    ${shift.shiftName}
                </th>
            `);

            for (var k = 0; k < shift.hours.length; k++) {

                var hour = shift.hours[k];

                if (hour == 0)
                    hour = 12;

                if (hour > 12)
                    hour = hour - 12;

                $("#hourHeader").append(`<th>${hour}</th>`);
            }
        }
    }
}