
function LoadHolidays(plantId) {
    var tablebody = $("#HolidaysTable tbody");
    $(tablebody).html("");//empty tbody
    api.get("/plant/getholidays?plantId=" + plantId).then((data) => {
        //console.log(data);
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
           $(tablebody).append(AppUtil.ProcessTemplateDataNew("HolidayRow", data[i], i));
        }
    }).catch((error) => {
        //console.log(error);
    });
}
/*
"holidayId": 0,
    "plantId": 0,
    "name": "string",
    "holidayDate": "2024-04-22T19:27:49.041Z",
*/

function EditHoliday(holidayId, name, holiDayDateStr, day) {
    //console.log("holidayId : "+holidayId);
    $("#HolidayId").val(holidayId);
    $("#HName").val(name);
    document.getElementById("HolidayDate").value = holiDayDateStr.split("-").reverse().join("-");;
}

function DeleteHoliday(holidayId,name,plantId) {
    var result = confirm("Are you sure you want to delete "+name+" from holiday list?");
    if (result) {
        api.get("/plant/delholiday?holidayId=" + holidayId).then((data) => {
            //console.log(data);
            var plantId = $("#HolidayPlantId").val();
            LoadHolidays(plantId);
        }).catch((error) => {
        });
    }
}
function AddHolidayToList() {
    var formData = AppUtil.GetFormData("HolidayForm");
    return api.post("/plant/plantholiday", formData).then((data) => {
        var plantId = $("#HolidayPlantId").val();
        LoadHolidays(plantId);
        $("#HolidayId").val("0");
        $("#HName").val("");
        $("#HolidayDate").val("");
        alert("Holiday for the Plant Saved!");  
    }).catch((error) => {
        AppUtil.HandleError("HolidayForm", error);
    });
}

function GetPlantWD(plantId) {
    api.get("/plant/getplantwd?plantId=" + plantId).then((data) => {
        //console.log(data);
        $("#WDId").val(data.wdId);
        $("#WeeklyOff1").val(data.weeklyOff1);
        $("#WeeklyOff2").val(data.weeklyOff2);
        if (data.weeklyOff1 == null) {
            $("#WeeklyOff1").val("Sunday");
        }
        if (data.weeklyOff2 == null) {
            $("#WeeklyOff2").val("Sunday");
        }
        $("#NoOfShifts").val(data.noOfShifts);
        $("#NoOfWeeklyOff").val(data.noOfWeeklyOff);
        if (data.noOfWeeklyOff == 1) {
            $('#lblWkOf2').hide();
            $('#WeeklyOff2').hide();
        } else if(data.noOfWeeklyOff == 2) {
            $('#lblWkOf2').show();
            $('#WeeklyOff2').show();
        }
        for (let i = 1; i <= 3; i++) {
            if (i <= data.noOfShifts) {
                $('#shift' + i).show();
                $('#shiftdur' + i).show();
                $('#shiftbreakd' + i).show();
                $('#shiftbreakdur' + i).show();
            } else {
                $('#shift' + i).hide();
                $('#shiftdur' + i).hide();
                $('#shiftbreakd' + i).hide();
                $('#shiftbreakdur' + i).hide();
            }
        }
        $("#FirstShiftStartTime").val(data.firstShiftStartTime);
        $("#SecondShiftStartTime").val(data.secondShiftStartTime);
        $("#ThirdShiftStartTime").val(data.thirdShiftStartTime);
        $("#FirstShiftDuration").val(data.firstShiftDuration.replace(/:\d{2}$/, ""));
        $("#SecondShiftDuration").val(data.secondShiftDuration.replace(/:\d{2}$/, ""));
        $("#ThirdShiftDuration").val(data.thirdShiftDuration.replace(/:\d{2}$/, ""));
        $("#No_of_span_days").val(data.no_of_span_days);
        $("#Timeslot_duration").val(data.timeslot_duration);
        $("#Retention_Days").val(data.retention_Days);
        $("#First_Shift_Break_duration").val(data.first_Shift_Break_duration);
        $("#First_Shift_Break_start_time").val(data.first_Shift_Break_start_time);
        $("#Sec_Shift_Break_duration").val(data.sec_Shift_Break_duration);
        $("#Sec_Shift_Break_start_time").val(data.sec_Shift_Break_start_time);
        $("#Third_Shift_Break_duration").val(data.third_Shift_Break_duration);
        $("#Third_Shift_Break_start_time").val(data.third_Shift_Break_start_time);
    }).catch((error) => {
        //console.log(error);
    });
}

function AddWorkingDetails() {
    var formData = AppUtil.GetFormData("WDForm");
    return api.post("/plant/plantwd", formData).then((data) => {
        //console.log(data);
        $("#WDId").val(data.wdId);
        $("#WDPlantId").val(data.plantId);
        alert("Plant Working Details Saved!");
        $.ajax({
            type: "POST",
            url: '/WorkOrder/PostTimeslot_List',
            contentType: "application/json; charset=utf-8",
            headers: { 'Content-Type': 'application/json' },
            success: function (result) {
                console.log("Success:", result);
            }
        });
        //$("#NoOfShifts").val(1);
        //for (let i = 1; i <= 3; i++) {
        //    if (i <= 1) {
        //        $('#shift' + i).show();
        //        $('#shiftdur' + i).show();
        //        $('#shiftbreakd' + i).show();
        //        $('#shiftbreakdur' + i).show();
        //    } else {
        //        $('#shift' + i).hide();
        //        $('#shiftdur' + i).hide();
        //        $('#shiftbreakd' + i).hide();
        //        $('#shiftbreakdur' + i).hide();
        //    }
        //}
        //document.getElementById("WDForm").reset();
    }).catch((error) => {
        AppUtil.HandleError("WDForm", error);
    });
}

function LoadPlants() {
    var tablebody = $("#PlantTable tbody");
    $(tablebody).html("");//empty tbody
    //PlantRowTemplate
    //PlantTable
    api.getbulk("/plant/getplants").then((data) => {
        //console.log(data);
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
            for (var key in data[i]) {
                //        console.log(key);
                //      console.log(data[i][key]);
                //    console.log("*****");
            }
            //console.log("================");
           $(tablebody).append(AppUtil.ProcessTemplateDataNew("PlantRowTemplate", data[i], i));

            //var plantID = data[i].plantId;
            //api.get("/plant/getplantwd?plantId=" + plantID).then((data) => {
            //    console.log(data);
            //    WorkDetails.push(data);
            //}).catch((error) => {
            //    console.log('Error occurred:', error.message);
            //});
        }
       
    }).catch((error) => {
    });
};

function DelPlant(name, plantId) {
    let confirmval = confirm("Are your sure you want to delete this plant? : " + name, "Yes", "No");
    if (confirmval) {
        api.get("/plant/delplant?plantId=" + plantId).then((data) => {
            //console.log(data);
            LoadPlants();
        }).catch((error) => {
            //console.log(error);
        });
    }
};
$(function () {

    //address: "test"
    //isMainPlant: true
    //isProductDesigned: true
    //name: "test"
    //notes: "test"
    //plantId: 1
    //tenantId: 1
    //shop-details
    $("#GEN").on("click", function(){
        $("#tab-001").show();
        $("#tab-002").hide();
        $("#tab-003").hide();
    });
    $("#PWD").on("click", function (e) {
        if ($(this).hasClass("disabled-tab")) {
            e.preventDefault();
            return; // stop execution if disabled
        }
        $("#tab-001").hide();
        $("#tab-002").show();
        $("#tab-003").hide();
    });
    $("#HLI").on("click", function (e) {
        if ($(this).hasClass("disabled-tab")) {
            e.preventDefault();
            return; // stop execution if disabled
        }
        $("#tab-001").hide();
        $("#tab-002").hide();
        $("#tab-003").show();
    });
    $('#shop-details').on('shown.bs.modal', function (event) {

        if (IsAddOpCalled()) {
            document.getElementById("PlantForm").reset();
            document.getElementById("WDForm").reset();
            document.getElementById("HolidayForm").reset();
            $("#WDPlantId").val("0");
            $("#HolidayPlantId").val("0");
            $("#HolidayId").val("0");
            $("#WDId").val("0");
            var tablebody = $("#HolidaysTable tbody");
            $(tablebody).html("");
            loadCity();
            loadCountrys();
            $("#PWD").addClass("disabled-tab");
            $("#HLI").addClass("disabled-tab");
            $("#PWD a").addClass("disabled");
            $("#HLI a").addClass("disabled");
            $("#NoOfShifts").val(1);
            $("#NoOfWeeklyOff").val(1);
            $('#lblWkOf2').hide();
            $('#WeeklyOff2').hide();
            for (let i = 1; i <= 3; i++) {
                if (i <= 1) {
                    $('#shift' + i).show();
                    $('#shiftdur' + i).show();
                    $('#shiftbreakd' + i).show();
                    $('#shiftbreakdur' + i).show();
                } else {
                    $('#shift' + i).hide();
                    $('#shiftdur' + i).hide();
                    $('#shiftbreakd' + i).hide();
                    $('#shiftbreakdur' + i).hide();
                }
            }
            return;
        } else {
            $("#PWD").removeClass("disabled-tab");
            $("#HLI").removeClass("disabled-tab");
            $("#PWD a").removeClass("disabled");
            $("#HLI a").removeClass("disabled");
        }
        $("#GEN").show();
        const navItem = document.getElementById("GEN");
        navItem.classList.add("active");
        const navLink = navItem.querySelector("a");
        if (navLink) {
            navLink.classList.add("active");
        }
        const navItemP = document.getElementById("PWD");
        navItemP.classList.remove("active");
        const navLinkp = navItemP.querySelector("a");
        if (navLinkp) {
            navLinkp.classList.remove("active");
        }
        const navItemH = document.getElementById("HLI");
        navItemH.classList.remove("active");
        const navLinkH = navItemH.querySelector("a");
        if (navLinkH) {
            navLinkH.classList.remove("active");
        }
        $("#tab-001").show();
        $("#tab-002").hide();
        $("#tab-003").hide();
        var relatedTarget = $(event.relatedTarget);
        var address = relatedTarget.data("address");
        $("#Address").val(address);
        var isMainPlant = relatedTarget.data("ismainplant");
        var isProductDesigned = relatedTarget.data("isproductdesigned");
        //IsProductDesigned
        //IsMainPlant
        $("#IsMainPlant").prop('checked', false);
        if (isMainPlant) {
            $("#IsMainPlant").prop('checked',true);
        }
        $("#IsProductDesigned").prop('checked', false);
        if (isProductDesigned) {
            $("#IsProductDesigned").prop('checked', true);
        }

        var name = relatedTarget.data("name");
        $("#Name").val(name);
        var city = relatedTarget.data("city");
        $("#City").val(city);
        var pin = relatedTarget.data("pin");
        $("#Pincode").val(pin);
        var gst = relatedTarget.data("gst");
        $("#GstNo").val(gst);
        var pan = relatedTarget.data("pan");
        $("#PanNo").val(pan);
        var country = relatedTarget.data("country");
        $("#Country").val(country);
        var CitySelect = $('#CitySelect');
        CitySelect.html('');
        api.getbulk("/Plant/GetCitys").then((data) => {
            var sv = "";
            div_data = "<option value='" + sv + "'>" + "--Select--" + "</option>";
            CitySelect.append(div_data);
            for (i = 0; i < data.length; i++) {
                div_data = "<option value='" + data[i].name + "'>" + data[i].name + "</option>";
                CitySelect.append(div_data);
            }
            var city = relatedTarget.data("city");
            var CitySe = $("#CitySelect");
            CitySe.find("option[value='" + city + "']").prop('selected', true);
        });
        var selElem = $('#CountrySelect');
        selElem.html('');
        api.getbulk("/Plant/GetCountrys").then((data) => {

            for (i = 0; i < data.length; i++) {
                div_data = "<option value='" + data[i].name + "'>" + data[i].name + "</option>";
                selElem.append(div_data);
            }
            var country = relatedTarget.data("country");
            var CountrSe = $("#CountrySelect");
            CountrSe.find("option[value='" + country + "']").prop('selected', true);
        });
        var plantId = relatedTarget.data("plantid");
        //console.log("PlantId " + plantId);
        $("#PlantId").val(plantId);
        $("#WDPlantId").val(plantId);
        $("#HolidayPlantId").val(plantId);
        var elm = document.getElementById("plantname");
        elm.innerText = name;
        LoadHolidays(plantId);
        GetPlantWD(plantId);
        
    });

    $('#shop-details').on('hide.bs.modal', function (event) {
        document.getElementById("PlantForm").reset();
        document.getElementById("WDForm").reset();
        document.getElementById("HolidayForm").reset();
        document.getElementById("plantname").innerHTML = '';
        var newNamevalidate = $('#CitySelect').next('.select2-container');
        newNamevalidate.css('border', '');
        $("#Plant-error").text('');
        var CountrySelect = $('#CountrySelect').next('.select2-container');
        CountrySelect.css('border', '');
        var Name = document.getElementById('Name');
        Name.style.border = '';
        var noofshitfs = document.getElementById('NoOfShifts');
        noofshitfs.style.border = '';
        var NoOfWeeklyOff = document.getElementById('NoOfWeeklyOff');
        NoOfWeeklyOff.style.border = '';
        var FirstShiftStartTime = document.getElementById('FirstShiftStartTime');
        FirstShiftStartTime.style.border = '';
        var SecondShiftStartTime = document.getElementById('SecondShiftStartTime');
        SecondShiftStartTime.style.border = '';
        var ThirdShiftStartTime = document.getElementById('ThirdShiftStartTime');
        ThirdShiftStartTime.style.border = '';
        var FirstShiftDuration = document.getElementById('FirstShiftDuration');
        FirstShiftDuration.style.border = '';
        var SecondShiftDuration = document.getElementById('SecondShiftDuration');
        SecondShiftDuration.style.border = '';
        var ThirdShiftDuration = document.getElementById('ThirdShiftDuration');
        ThirdShiftDuration.style.border = '';
        var First_Shift_Break_start_time = document.getElementById('First_Shift_Break_start_time');
        First_Shift_Break_start_time.style.border = '';
        var Sec_Shift_Break_start_time = document.getElementById('Sec_Shift_Break_start_time');
        Sec_Shift_Break_start_time.style.border = '';
        var Third_Shift_Break_start_time = document.getElementById('Third_Shift_Break_start_time');
        Third_Shift_Break_start_time.style.border = '';
        var First_Shift_Break_duration = document.getElementById('First_Shift_Break_duration');
        First_Shift_Break_duration.style.border = '';
        var Sec_Shift_Break_duration = document.getElementById('Sec_Shift_Break_duration');
        Sec_Shift_Break_duration.style.border = '';
        var Third_Shift_Break_duration = document.getElementById('Third_Shift_Break_duration');
        Third_Shift_Break_duration.style.border = '';
        var Timeslot_duration = document.getElementById('Timeslot_duration');
        Timeslot_duration.style.border = '';
        var No_of_span_days = document.getElementById('No_of_span_days');
        No_of_span_days.style.border = '';
        var Retention_Days = document.getElementById('Retention_Days');
        Retention_Days.style.border = '';
        var WeeklyOff1 = document.getElementById('WeeklyOff1');
        WeeklyOff1.style.border = '';
        var WeeklyOff2 = document.getElementById('WeeklyOff2');
        WeeklyOff2.style.border = '';
        var HolidayDate = document.getElementById('HolidayDate');
        HolidayDate.style.border = '';
        var HName = document.getElementById('HName');
        HName.style.border = '';
        $("#GEN").show();
        const navItem = document.getElementById("GEN");
        navItem.classList.add("active");
        const navLink = navItem.querySelector("a");
        if (navLink) {
            navLink.classList.add("active");
        }
        const navItemP = document.getElementById("PWD");
        navItemP.classList.remove("active");
        const navLinkp = navItemP.querySelector("a");
        if (navLinkp) {
            navLinkp.classList.remove("active");
        }
        const navItemH = document.getElementById("HLI");
        navItemH.classList.remove("active");
        const navLinkH = navItemH.querySelector("a");
        if (navLinkH) {
            navLinkH.classList.remove("active");
        }
        $("#tab-001").show();
        $("#tab-002").hide();
        $("#tab-003").hide();

    });

    $("#docname").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#PlantTable tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#PlantTable tbody");
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

    //SaveWorkDetails
    //AddHoliday
    $("#SaveWorkDetails").secureClick( function (event) {
        var WeeklyOff2 = document.getElementById('WeeklyOff2');
        var WeeklyOff1 = document.getElementById('WeeklyOff1');
        var NoOfWeeklyOff = document.getElementById('NoOfWeeklyOff');
        if (WeeklyOff2.value === WeeklyOff1.value && parseInt(NoOfWeeklyOff.value) == 2 ) {
            WeeklyOff1.style.border = '2px solid red';
            WeeklyOff2.style.border = '2px solid red';
            alert("Weekly Off 1 and Weekly Off 2 should be different.");
            return false;
        } else {
            WeeklyOff1.style.border = '';
            WeeklyOff2.style.border = '';
        }
        var noofshitfs = document.getElementById('NoOfShifts');
        if (!noofshitfs.value || parseInt(noofshitfs.value) <=0) {
            noofshitfs.style.border = '2px solid red';
            //alert(" No Of Shifts Field is Empty");
            return false;
        } else {
            noofshitfs.style.border = '';
        }
        if (parseInt(noofshitfs.value) > 3) {
            noofshitfs.style.border = '2px solid red';
            alert("No of Shifts / day can not be Greater Than 3");
            return false;
        } else {
            noofshitfs.style.border = '';
        }
        if (!NoOfWeeklyOff.value || parseInt(NoOfWeeklyOff.value) <= 0) {
            NoOfWeeklyOff.style.border = '2px solid red';
            return false;
        } else {
            NoOfWeeklyOff.style.border = '';
        }
        if (parseInt(NoOfWeeklyOff.value) > 2) {
            NoOfWeeklyOff.style.border = '2px solid red';
            alert("No Of Weekly Offs can not be Greater Than 2");
            return false;
        } else {
            NoOfWeeklyOff.style.border = '';
        }
        var FirstShiftStartTime = document.getElementById('FirstShiftStartTime');
        if (!FirstShiftStartTime.value) {
            FirstShiftStartTime.style.border = '2px solid red';
            return false;
        } else {
            FirstShiftStartTime.style.border = '';
        }
        var SecondShiftStartTime = document.getElementById('SecondShiftStartTime');
        if (!SecondShiftStartTime.value && parseInt(noofshitfs.value) == 2) {
            SecondShiftStartTime.style.border = '2px solid red';
            return false;
        } else {
            SecondShiftStartTime.style.border = '';
        }
        var ThirdShiftStartTime = document.getElementById('ThirdShiftStartTime');
        if (!ThirdShiftStartTime.value && parseInt(noofshitfs.value) == 3) {
            ThirdShiftStartTime.style.border = '2px solid red';
            return false;
        } else {
            ThirdShiftStartTime.style.border = '';
        }
        var FirstShiftDuration = document.getElementById('FirstShiftDuration');
        var SecondShiftDuration = document.getElementById('SecondShiftDuration');
        var ThirdShiftDuration = document.getElementById('ThirdShiftDuration');

        var timePatternShift = /^(?:[01]\d|2[0-3]):[0-5]\d$/; // full HH:MM
        var hourPattern = /^(?:[01]?\d|2[0-3])$/; // only HH

        // ---- First Shift ----
        if (!FirstShiftDuration.value) {
            FirstShiftDuration.style.border = '2px solid red';
            return false;
        } else if (hourPattern.test(FirstShiftDuration.value.trim())) {
            FirstShiftDuration.value = FirstShiftDuration.value.padStart(2, "0") + ":00";
            FirstShiftDuration.style.border = '';
        } else if (timePatternShift.test(FirstShiftDuration.value.trim())) {
            FirstShiftDuration.style.border = '';
        } else {
            FirstShiftDuration.style.border = '2px solid red';
            return false;
        }

        // ---- Second Shift ----
        if (parseInt(noofshitfs.value) >= 2) {
            if (!SecondShiftDuration.value) {
                SecondShiftDuration.style.border = '2px solid red';
                return false;
            } else if (hourPattern.test(SecondShiftDuration.value.trim())) {
                SecondShiftDuration.value = SecondShiftDuration.value.padStart(2, "0") + ":00";
                SecondShiftDuration.style.border = '';
            } else if (timePatternShift.test(SecondShiftDuration.value.trim())) {
                SecondShiftDuration.style.border = '';
            } else {
                SecondShiftDuration.style.border = '2px solid red';
                return false;
            }
        }

        // ---- Third Shift ----
        if (parseInt(noofshitfs.value) == 3) {
            if (!ThirdShiftDuration.value) {
                ThirdShiftDuration.style.border = '2px solid red';
                return false;
            } else if (hourPattern.test(ThirdShiftDuration.value.trim())) {
                ThirdShiftDuration.value = ThirdShiftDuration.value.padStart(2, "0") + ":00";
                ThirdShiftDuration.style.border = '';
            } else if (timePatternShift.test(ThirdShiftDuration.value.trim())) {
                ThirdShiftDuration.style.border = '';
            } else {
                ThirdShiftDuration.style.border = '2px solid red';
                return false;
            }
        }

        var Timeslot_duration = document.getElementById('Timeslot_duration');
        if (!Timeslot_duration.value || parseInt(Timeslot_duration.value) <= 0) {
            Timeslot_duration.style.border = '2px solid red';
            return false;
        } else {
            Timeslot_duration.style.border = '';
        }
        var No_of_span_days = document.getElementById('No_of_span_days');
        if (!No_of_span_days.value || parseInt(No_of_span_days.value) <= 0) {
            No_of_span_days.style.border = '2px solid red';
            return false;
        } else {
            No_of_span_days.style.border = '';
        }
        var Retention_Days = document.getElementById('Retention_Days');
        if (!Retention_Days.value || parseInt(Retention_Days.value) <= 0) {
            Retention_Days.style.border = '2px solid red';
            return false;
        } else {
            Retention_Days.style.border = '';
        }
        var First_Shift_Break_start_time = document.getElementById('First_Shift_Break_start_time');
        if (!First_Shift_Break_start_time.value && parseInt(noofshitfs.value) >= 1) {
            First_Shift_Break_start_time.style.border = '2px solid red';
            return false;
        } else {
            First_Shift_Break_start_time.style.border = '';
        }
        var Sec_Shift_Break_start_time = document.getElementById('Sec_Shift_Break_start_time');
        if (!Sec_Shift_Break_start_time.value && parseInt(noofshitfs.value) >= 2) {
            Sec_Shift_Break_start_time.style.border = '2px solid red';
            return false;
        } else {
            Sec_Shift_Break_start_time.style.border = '';
        }
        var Third_Shift_Break_start_time = document.getElementById('Third_Shift_Break_start_time');
        if (!Third_Shift_Break_start_time.value && parseInt(noofshitfs.value) == 3) {
            Third_Shift_Break_start_time.style.border = '2px solid red';
            return false;
        } else {
            Third_Shift_Break_start_time.style.border = '';
        }
        var First_Shift_Break_duration = document.getElementById('First_Shift_Break_duration');
        var Sec_Shift_Break_duration = document.getElementById('Sec_Shift_Break_duration');
        var Third_Shift_Break_duration = document.getElementById('Third_Shift_Break_duration');

        // Regex for full HH:MM
        var timePatternHHMM = /^([0-5]?\d):([0-5]\d)$/;
        // Regex for only HH
        var hourPatternHH = /^([0-5]?\d)$/;

        // ---- First Shift Break ----
        if (parseInt(noofshitfs.value) >= 1) {
            if (!First_Shift_Break_duration.value) {
                First_Shift_Break_duration.style.border = '2px solid red';
                return false;
            } else if (hourPatternHH.test(First_Shift_Break_duration.value.trim())) {
                First_Shift_Break_duration.value = First_Shift_Break_duration.value.padStart(2, "0") + ":00";
                First_Shift_Break_duration.style.border = '';
            } else if (timePatternHHMM.test(First_Shift_Break_duration.value.trim())) {
                First_Shift_Break_duration.style.border = '';
            } else {
                First_Shift_Break_duration.style.border = '2px solid red';
                return false;
            }
        }

        // ---- Second Shift Break ----
        if (parseInt(noofshitfs.value) >= 2) {
            if (!Sec_Shift_Break_duration.value) {
                Sec_Shift_Break_duration.style.border = '2px solid red';
                return false;
            } else if (hourPatternHH.test(Sec_Shift_Break_duration.value.trim())) {
                Sec_Shift_Break_duration.value = Sec_Shift_Break_duration.value.padStart(2, "0") + ":00";
                Sec_Shift_Break_duration.style.border = '';
            } else if (timePatternHHMM.test(Sec_Shift_Break_duration.value.trim())) {
                Sec_Shift_Break_duration.style.border = '';
            } else {
                Sec_Shift_Break_duration.style.border = '2px solid red';
                return false;
            }
        }

        // ---- Third Shift Break ----
        if (parseInt(noofshitfs.value) == 3) {
            if (!Third_Shift_Break_duration.value) {
                Third_Shift_Break_duration.style.border = '2px solid red';
                return false;
            } else if (hourPatternHH.test(Third_Shift_Break_duration.value.trim())) {
                Third_Shift_Break_duration.value = Third_Shift_Break_duration.value.padStart(2, "0") + ":00";
                Third_Shift_Break_duration.style.border = '';
            } else if (timePatternHHMM.test(Third_Shift_Break_duration.value.trim())) {
                Third_Shift_Break_duration.style.border = '';
            } else {
                Third_Shift_Break_duration.style.border = '2px solid red';
                return false;
            }
        }
        var isvalid = validateShifts();
        if (isvalid) {
            //AddWorkingDetails();
        } else {
            return false;
        }
        var isvalidBreak = validateBreakStartWithinShift();
        if (isvalidBreak) {
            //AddWorkingDetails();
        } else {
            return false;
        }
        return AddWorkingDetails();
        $("#btn-shopdetails-close").prop('disabled', false);
    });
    $("#AddHoliday").secureClick( function (event) {
        var nameInput = document.getElementById('HName');
        var dateInput = document.getElementById('HolidayDate');
        var PlantId = document.getElementById('PlantId');
        

        if (PlantId.value == 0) {
            //PlantId.style.border = '2px solid red';
            alert("Please Save The Plant Details.");
            return false;
        } else {
        }
        if (!nameInput.value) {
            nameInput.style.border = '2px solid red';
            return false;
        } else {
            nameInput.style.border = '';
        }

        if (!dateInput.value) {
            dateInput.style.border = '2px solid red';
            return false;
        } else {
            dateInput.style.border = '';
        }
        return AddHolidayToList();
        
    });

    $("#btn-shopdetails-close").on('click', function (event) {
             LoadPlants();
    });

    $("#EditCityPop").click(function (event) {
        var selectedValue = $("#CitySelect").val();  // Get the value of the selected option
        var selectedText = $("#CitySelect").find("option:selected").text();
        if (selectedText == "--Select--") {
            $("#CityPop").modal("hide");
            return false;
        }
        document.forms["frmAddCity"]["Name"].value = selectedText;
        api.getbulk("/Plant/GetCitys").then((data) => {
            data = data.filter(item => item.name == selectedText);
            var cid = data[0].cityId;
            document.forms["frmAddCity"]["CityId"].value = cid;
        });
    });
    $("#EditCountryPop").click(function (event) {
        var selectedValue = $("#CountrySelect").val();  // Get the value of the selected option
        var selectedText = $("#CountrySelect").find("option:selected").text();
        document.forms["frmAddCountry"]["Name"].value = selectedText;
        api.getbulk("/Plant/GetCountrys").then((data) => {
            data = data.filter(item => item.name == selectedText);
            var cid = data[0].countryId;
            document.forms["frmAddCountry"]["CountryId"].value = cid;
        });
    });
    $("#CitySelect").select2({
        dropdownParent: $("#shop-details")
    });
    $("#CountrySelect").select2({
        dropdownParent: $("#shop-details")
    });
    $('#CityPop').on('hidden.bs.modal', function (event) {
        document.getElementById('shop-details').style.filter = 'none';
        $("#CName").val("");
        $("#PCityId").val("");
    });
    $('#CountryPop').on('hidden.bs.modal', function (event) {
        document.getElementById('shop-details').style.filter = 'none';
        $("#CoName").val("");
        $("#PCountryId").val("");
    });
    $('#CityPop').on('show.bs.modal', function (event) {
        document.getElementById('shop-details').style.filter = 'blur(5px)';
    });
    $('#CountryPop').on('show.bs.modal', function (event) {
        document.getElementById('shop-details').style.filter = 'blur(5px)';
    });

    $("#SaveCity").on('click', function () {
        var name = $("#CName").val();
        if (name.length == 0) {
            var newNamevalidate = document.getElementById('CName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('CName');
            newNamevalidate.style.border = '';
        }
        var formData = AppUtil.GetFormData("frmAddCity");
        // if (valid) {

        api.getbulk("/plant/CheckCity?city=" + name).then((data) => {
            if (!data) {
                api.post("/plant/PostCity", formData).then((data) => {
                    var newopt = {
                        id: data.cityId,
                        text: data.name
                    };
                    loadCity();
                    $("#CityPop").modal("hide");
                }).catch((error) => {
                });
            } else {
                var newNamevalidate = document.getElementById('CName');
                newNamevalidate.style.border = '2px solid red';
            }
        }).catch((error) => {
        });
        // }
    });
    $('#NoOfShifts').on('input', function () {
        let val = parseInt($(this).val().trim());

        if (isNaN(val)) val = 1;
        if (val > 3) {
            alert("No of Shifts / day can not be Greater Than 3");
            //$('#NoOfShifts').val(3);
        }

        // Loop to show/hide shift inputs
        for (let i = 1; i <= 3; i++) {
            if (i <= val) {
                $('#shift' + i).show();
                $('#shiftdur' + i).show();
                $('#shiftbreakd' + i).show();
                $('#shiftbreakdur' + i).show();
            } else {
                $('#shift' + i).hide();
                $('#shiftdur' + i).hide();
                $('#shiftbreakd' + i).hide();
                $('#shiftbreakdur' + i).hide();
            }
        }
    }).trigger('input');
    $('#NoOfWeeklyOff').on('input', function () {
        let val = parseInt($(this).val().trim());

        if (isNaN(val)) val = 1;
        if (val > 2) {
            alert("No Of Weekly Offs can not be Greater Than 2");
        }
        if (1 === val) {
            $('#lblWkOf2').hide();
            $('#WeeklyOff2').hide();
        } else {
            $('#lblWkOf2').show();
            $('#WeeklyOff2').show();
        }
    }).trigger('input');
    $("#SaveCountry").on('click', function () {
        var name = $("#CoName").val();
        if (name.length == 0) {
            var newNamevalidate = document.getElementById('CoName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('CoName');
            newNamevalidate.style.border = '';
        }
        var formData = AppUtil.GetFormData("frmAddCountry");
        // if (valid) {

        api.getbulk("/plant/CheckCountry?city=" + name).then((data) => {
            if (!data) {
                api.post("/plant/PostCountry", formData).then((data) => {
                    var newopt = {
                        id: data.cityId,
                        text: data.name
                    };
                    loadCountrys();
                    $("#CountryPop").modal("hide");
                }).catch((error) => {
                });
            } else {
                var newNamevalidate = document.getElementById('CoName');
                newNamevalidate.style.border = '2px solid red';
            }
        }).catch((error) => {
        });
        // }
    });
    $("#BtnSavePlant").secureClick(function (event) {
        var Name = document.getElementById('Name');
        if (!Name.value) {
            Name.style.border = '2px solid red';
            return false;
        } else {
            Name.style.border = '';
        }
        var Address = document.getElementById('Address');
        if (!Address.value) {
            $("#Address").val("-");
        } else {
            Address.style.border = '';
        }
        var City = document.getElementById('CitySelect');
        if (!City.value) {
            var newNamevalidate = $('#CitySelect').next('.select2-container');
            newNamevalidate.css('border', '2px solid red');
            return false;
        } else {
            var newNamevalidate = $('#CitySelect').next('.select2-container');
            newNamevalidate.css('border', '');
        }
        var Pincode = document.getElementById('Pincode');
        if (!Pincode.value) {
            $("#Pincode").val("");
        } else {
            Pincode.style.border = '';
        }
        var Country = document.getElementById('CountrySelect');
        if (!Country.value) {
            var newNamevalidate = $('#CountrySelect').next('.select2-container');
            newNamevalidate.css('border', '2px solid red');
            return false;
        } else {
            var newNamevalidate = $('#CountrySelect').next('.select2-container');
            newNamevalidate.css('border', '');
        }
        var GstNo = document.getElementById('GstNo');
        if (!GstNo.value) {
            $("#GstNo").val("");
        } else {
            GstNo.style.border = '';
        }
        var PanNo = document.getElementById('PanNo');
        if (!PanNo.value) {
            $("#PanNo").val("");
        } else {
            PanNo.style.border = '';
        }
        var formData = AppUtil.GetFormData("PlantForm");
        return api.getbulk("/Plant/getplants").then((data) => {
            data = data.filter(item => item.name == Name.value);
            if (data.length === 0 || formData.PlantId > 0) {

                api.post("/plant/plant", formData).then((data) => {
                    // console.log(data);
                    //document.getElementById("btn-shopdetails-close").click();
                    //document.getElementById("PlantForm").reset();
                    var wd = $("#WDPlantId").val();
                    //if (wd === "0") {
                    //    $("#btn-shopdetails-close").prop('disabled', true);
                    //} else {
                    //    $("#btn-shopdetails-close").prop('disabled', false);
                    //}
                    var plantID = data.plantId;
                    $("#PWD").removeClass("disabled-tab");
                    $("#HLI").removeClass("disabled-tab");
                    $("#PWD a").removeClass("disabled");
                    $("#HLI a").removeClass("disabled");
                    $("#WDPlantId").val(plantID);
                    $("#HolidayPlantId").val(plantID);
                    alert("Plant Details Genaral Saved!");
                    $("#tab-002").show();
                    $("#tab-001").hide();
                    $("#GEN").show();

                    const navItem = document.getElementById("PWD");
                    navItem.classList.add("active");
                    const navLink = navItem.querySelector("a");
                    if (navLink) {
                        navLink.classList.add("active");
                    }
                    const navItemP = document.getElementById("GEN");
                    navItemP.classList.remove("active");
                    const navLinkp = navItemP.querySelector("a");
                    if (navLinkp) {
                        navLinkp.classList.remove("active");
                    }
                    const navItemH = document.getElementById("HLI");
                    navItemH.classList.remove("active");
                    const navLinkH = navItemH.querySelector("a");
                    if (navLinkH) {
                        navLinkH.classList.remove("active");
                    }
                }).catch((error) => {
                    AppUtil.HandleError("PlantForm", error);
                });
            } else {
                //alert("The Plant Name Already Exists.");  
                $("#Plant-error").text('The Plant Name Already Exists.').css('color', 'red');;
            }
        });
    });
   
    LoadPlants();
    loadTimeSlotDuration();
    loadNoOfDaysTimeSlot();
    // Initialize all timepickers
    $('#FirstShiftStartTime, #SecondShiftStartTime, #ThirdShiftStartTime, #First_Shift_Break_start_time, #Sec_Shift_Break_start_time, #Third_Shift_Break_start_time').timepicker({
        timeFormat: 'h:i A',
        step: 5,
        disableTextInput: true
    });
    $('#FirstShiftStartTime').on('input', function () {
        let val = $(this).val().toLowerCase().replace(/\s+/g, '');
        $('.ui-timepicker-list li').each(function () {
            let text = $(this).text().toLowerCase().replace(/\s+/g, '');
            $(this).toggle(text.startsWith(val));
        });
    });

    $('#FirstShiftStartTime, #FirstShiftDuration').on('change input', updateSecondShiftMinTime);
    $('#SecondShiftStartTime, #SecondShiftDuration').on('change input', updateThirdShiftMinTime);
    $('#ThirdShiftStartTime, #ThirdShiftDuration').on('change input', validateThirdShiftWithin24Hours);
});

function loadCity() {
    var selElem = $('#CitySelect');
    selElem.html('');
    api.getbulk("/Plant/GetCitys").then((data) => {
        var sv = "";
        div_data = "<option value='" + sv + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].name + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });

}
function loadNoOfDaysTimeSlot() {
    var selElem = $('#Timeslot_duration');
    selElem.html('');
    api.getbulk("/Plant/GetTimeSlotDurations").then((data) => {

        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].duration + "'>" + data[i].duration + "</option>";
            selElem.append(div_data);
        }
    });

}
function loadTimeSlotDuration() {
    var selElem = $('#No_of_span_days');
    selElem.html('');
    api.getbulk("/Plant/GetNoOfDaysTimeSlots").then((data) => {

        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].days + "'>" + data[i].days + "</option>";
            selElem.append(div_data);
        }
    });

}
function loadCountrys() {
    var selElem = $('#CountrySelect');
    selElem.html('');
    api.getbulk("/Plant/GetCountrys").then((data) => {

        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].name + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });

}
function parseTimeToMinutes(timeStr) {
    const date = new Date("1970-01-01T" + timeStr);
    return date.getHours() * 60 + date.getMinutes();
}

function validateShifts() {
    const shiftCount = parseInt(document.getElementById('NoOfShifts').value);

    const start1 = parseTimeToMinutes(document.getElementById('FirstShiftStartTime').value);
    const dur1 = parseFloat(document.getElementById('FirstShiftDuration').value) * 60;
    const end1 = start1 + dur1;

    let start2 = 0, dur2 = 0, end2 = 0;
    if (shiftCount >= 2) {
        start2 = parseTimeToMinutes(document.getElementById('SecondShiftStartTime').value);
        dur2 = parseFloat(document.getElementById('SecondShiftDuration').value) * 60;
        if (start2 < start1) start2 += 1440; // next day
        end2 = start2 + dur2;

        if (start2 <= end1) {
            alert("Second shift must start after first shift ends.");
            return false;
        }
    }

    let start3 = 0, dur3 = 0, end3 = 0;
    if (shiftCount === 3) {
        start3 = parseTimeToMinutes(document.getElementById('ThirdShiftStartTime').value);
        dur3 = parseFloat(document.getElementById('ThirdShiftDuration').value) * 60;
        if (start3 < start2) start3 += 1440; // next day
        end3 = start3 + dur3;

        if (start3 <= end2) {
            alert("Third shift must start after second shift ends.");
            return false;
        }

        // Compare 3rd shift end with next day's 1st shift start
        const nextDayStart1 = start1 + 1440;
        if (end3 > nextDayStart1) {
            alert("Third shift ends after next day's first shift starts.");
            return false;
        }
    }

    return true;
}

function parseTime(timeStr) {
    const [time, modifier] = timeStr.split(' ');
    let [hours, minutes] = time.split(':').map(Number);

    if (modifier === 'PM' && hours !== 12) hours += 12;
    if (modifier === 'AM' && hours === 12) hours = 0;

    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return date;
}

function validateBreakStartWithinShift() {
    const shiftCount = parseInt(document.getElementById('NoOfShifts').value);

    if (shiftCount >= 1) {
        const shiftStart = parseTime(document.getElementById('FirstShiftStartTime').value);
        const duration = parseFloat(document.getElementById('FirstShiftDuration').value);
        const shiftEnd = new Date(shiftStart.getTime() + duration * 60 * 60 * 1000);
        const breakStartField = document.getElementById('First_Shift_Break_start_time');
        const breakStart = parseTime(breakStartField.value);

        if (!(breakStart >= shiftStart && breakStart < shiftEnd)) {
            breakStartField.style.border = '2px solid red';
            alert('1st Shift Break Start must be within 1st Shift time.');
            return false;
        } else {
            breakStartField.style.border = '';
        }
    }

    if (shiftCount >= 2) {
        const shiftStart = parseTime(document.getElementById('SecondShiftStartTime').value);
        const duration = parseFloat(document.getElementById('SecondShiftDuration').value);
        const shiftEnd = new Date(shiftStart.getTime() + duration * 60 * 60 * 1000);
        const breakStartField = document.getElementById('Sec_Shift_Break_start_time');
        const breakStart = parseTime(breakStartField.value);

        if (!(breakStart >= shiftStart && breakStart < shiftEnd)) {
            breakStartField.style.border = '2px solid red';
            alert('2nd Shift Break Start must be within 2nd Shift time.');
            return false;
        } else {
            breakStartField.style.border = '';
        }
    }

    if (shiftCount >= 3) {
        const shiftStart = parseTime(document.getElementById('ThirdShiftStartTime').value);
        const duration = parseFloat(document.getElementById('ThirdShiftDuration').value);
        const shiftEnd = new Date(shiftStart.getTime() + duration * 60 * 60 * 1000);
        const breakStartField = document.getElementById('Third_Shift_Break_start_time');
        const breakStart = parseTime(breakStartField.value);

        if (!(breakStart >= shiftStart && breakStart < shiftEnd)) {
            breakStartField.style.border = '2px solid red';
            alert('3rd Shift Break Start must be within 3rd Shift time.');
            return false;
        } else {
            breakStartField.style.border = '';
        }
    }

    return true;
}



function durationToSeconds(duration) {
    const parts = duration.split(':').map(Number);
    return parts[0] * 3600 + parts[1] * 60 + (parts[2] || 0);
}

function getShiftEndTime(start, duration) {
    const [time, modifier] = start.split(' ');
    let [hours, minutes] = time.split(':').map(Number);

    if (modifier === 'PM' && hours < 12) hours += 12;
    if (modifier === 'AM' && hours === 12) hours = 0;

    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return new Date(date.getTime() + durationToSeconds(duration) * 1000);
}

function formatTime12hr(date) {
    let hours = date.getHours();
    const minutes = date.getMinutes();
    const ampm = hours >= 12 ? 'PM' : 'AM';

    hours = hours % 12;
    hours = hours ? hours : 12; // 0 becomes 12
    return `${hours}:${minutes.toString().padStart(2, '0')} ${ampm}`;
}


function updateSecondShiftMinTime() {
    const start = $('#FirstShiftStartTime').val();
    const duration = $('#FirstShiftDuration').val();
    if (!start || !duration) return;

    const endTime = getShiftEndTime(start, duration);
    const formattedMin = formatTime12hr(endTime);
    $('#SecondShiftStartTime').timepicker('option', 'minTime', formattedMin);

    const current = $('#SecondShiftStartTime').val();
    if (current && getShiftEndTime(current, '00:00:00') < endTime) {
        $('#SecondShiftStartTime').val('');
    }

    updateThirdShiftMinTime();
}

function updateThirdShiftMinTime() {
    const start = $('#SecondShiftStartTime').val();
    const duration = $('#SecondShiftDuration').val();
    if (!start || !duration) return;

    const endTime = getShiftEndTime(start, duration);
    const formattedMin = formatTime12hr(endTime);
    $('#ThirdShiftStartTime').timepicker('option', 'minTime', formattedMin);

    const current = $('#ThirdShiftStartTime').val();
    if (current && getShiftEndTime(current, '00:00:00') < endTime) {
        $('#ThirdShiftStartTime').val('');
    }
}
function validateThirdShiftWithin24Hours() {
    const firstStart = $('#FirstShiftStartTime').val();
    const firstDuration = $('#FirstShiftDuration').val();
    const secondStart = $('#SecondShiftStartTime').val();
    const secondDuration = $('#SecondShiftDuration').val();
    const thirdStart = $('#ThirdShiftStartTime').val();
    const thirdDuration = $('#ThirdShiftDuration').val();

    if (!firstStart || !firstDuration || !secondStart || !secondDuration || !thirdStart || !thirdDuration) {
        return; // Wait until all values are filled
    }

    const firstStartTime = getShiftEndTime(firstStart, '00:00:00');
    const totalAllowedEnd = new Date(firstStartTime.getTime() + 24 * 3600 * 1000);

    const thirdEndTime = getShiftEndTime(thirdStart, thirdDuration);

    if (thirdEndTime > totalAllowedEnd) {
        alert("The combined duration of all three shifts exceeds 24 hours. Please adjust shift start time and shift duration.");
        $('#ThirdShiftDuration').val('');
    }
}
