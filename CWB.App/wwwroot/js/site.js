var AppUtil = {
    NavMenuUpdate: () => {
        //remove active navbar..
        // Remove existing active navbar classes
        $("#side-menu").find("li.menuitem-active a").removeClass("active");
        $("#side-menu").find("li.menuitem-active").removeClass("menuitem-active");

        // Get the nav menu ID
        var navMenuID = $("#hdn-nav-menu").val();

        // Add 'active' class to the correct <a> tag
        var $activeLink = $("a[data-navid='" + navMenuID + "']");
        $activeLink.addClass("active");

        // Add 'menuitem-active' to the parent <li>
        $activeLink.parents("li").addClass("menuitem-active");

        // Expand the parent collapse div (if exists)
        var $collapseDiv = $activeLink.closest(".collapse");
        if ($collapseDiv.length) {
            $collapseDiv.addClass("show"); // Ensures it's expanded
            $collapseDiv.attr("aria-expanded", "true");
            $collapseDiv.prev("a[data-bs-toggle='collapse']").attr("aria-expanded", "true");
        }

    },
    TableFilter: (tableId, searchObj) => {
        var tableRows = $("#" + tableId).find("tbody tr");
        if (Object.keys(searchObj).length > 0) {
            $(tableRows).hide();
            for (i = 0; i < tableRows.length; i++) {
                var trow = tableRows[i];
                if (trow) {
                    for (var key in searchObj) {
                        var td = $(trow).find("td[data-key='" + key + "']").first();
                        txtValue = $(td).text();
                        if (txtValue.toUpperCase().indexOf(searchObj[key]) > -1) {
                            $(trow).show();
                        }
                    }
                }
            }
        } else {
            $(tableRows).show();
        }
    },
    ProcessTemplateDataNew: (templateId, dataObj,i) => {
        var templateElement = $("#" + templateId).html();
        templateElement = templateElement.replaceAll("{num}", i)
        ////console.log(templateId);
        for (var key in dataObj) {
      //      console.log(key + " " + dataObj[key]);
            templateElement = templateElement.replaceAll("{" + key + "}", dataObj[key])
        }
    //    console.log(templateElement);
 //       console.log("****************");
        return templateElement;
    },
    ProcessTemplateData: (templateId, dataObj) => {
        //debugger;
        var templateElement = $("#" + templateId).html();
        ////console.log(templateId);
        for (var key in dataObj) {
        //      console.log(key);
        //       console.log(dataObj[key]);
            templateElement = templateElement.replaceAll("{" + key + "}", dataObj[key])
            
        }
        //console.log(templateElement);
        //console.log("****************");
        return templateElement;
    },
    GetFormData: (formId) => {
        var formData = new FormData(document.getElementById(formId));
        var formDataObj = {};
        formData.forEach((value, key) => (formDataObj[key] = value));
        return formDataObj;
    },
    GetFormDataNew: (formId) => {
        var formData = new FormData(document.getElementById(formId));
        return formData.entries();
    },
    HandleError: (formId, error) => {
        if (error.status === 400) {
            $('#' + formId).validate().showErrors(error.responseJSON);
        }
    }
};

function ShowDoingNotifier() {
    let nLen = notifiers.length;
    for (let i = 0; i < nLen; i++) {
        AddDoingNotifier(notifiers[i]);
    }
};

let notifiers = ["op-notifier", "op-notifier-addbom", "op-notifier-addrm"];
function ShowSuccessNotifiers() {
    let nLen = notifiers.length;
    for (let i = 0; i < nLen; i++) {
        AddSuccessNotifier(notifiers[i]);
    }
};
function ShowFailureNotifiers() {
    let nLen = notifiers.length;
    for (let i = 0; i < nLen; i++) {
        AddFailureNotifier(notifiers[i]);
    }
};

//function AddDoingNotifier(elementId) {

//    if (document.getElementById(elementId) != null) {
//        $('#' + elementId).html(`
//					            <div id="op-alert" class="alert alert-success" role="success">
//		                		Saving...
//			                    </div>`);
//        setInterval(function () {
//            $('#op-alert').alert('close');
//        }, 5000);
//    }
//};
function AddDoingNotifier(elementId) {

    let $target = null;

    // If modal is open → use floating notifier
    if ($('.modal.show').length > 0) {
        $target = $('#global-notifier');
    }
    // Else → use normal top notifier
    else if ($('#op-notifier').length > 0) {
        $target = $('#op-notifier');
    }

    if (!$target || $target.length === 0) return;

    const alertHtml = `
        <div class="alert alert-success" role="success">
          Saving
        </div>`;

    $target.html(alertHtml);

    // Optional auto-close after 2 sec
    setTimeout(() => {
        $target.find('.alert').alert('close');
    }, 2000);
}


function ShowLoadingNotifier() {
    let nLen = notifiers.length;
    for (let i = 0; i < nLen; i++) {
        AddLoadingNontifier(notifiers[i]);
    }
};

//function AddLoadingNontifier(elementId) {

//    if (document.getElementById(elementId) != null) {
//        $('#' + elementId).html(`
//					            <div id="op-alert" class="alert alert-success" role="success">
//		                		Loading...
//			                    </div>`);
//       /* setInterval(function () {
//            $('#op-alert').alert('close');
//        }, 8000);*/
//    }
//};
function AddLoadingNontifier(elementId) {

    let $target = null;

    // If modal is open → floating notifier
    if ($('.modal.show').length > 0) {
        $target = $('#global-notifier');
    }
    // Else → normal top notifier
    else if ($('#op-notifier').length > 0) {
        $target = $('#op-notifier');
    }

    if (!$target || $target.length === 0) return;

    const alertHtml = `
        <div class="alert alert-success" role="success">
            Loading...
        </div>`;

    $target.html(alertHtml);
    // Optional auto-close after 2 sec
    setTimeout(() => {
        $target.find('.alert').alert('close');
    }, 2000);
}


function CloseOpAlert() {
    if ($('#op-alert').length) {
        $('#op-alert').alert('close');
    }
}
//function AddSuccessNotifier(elementId) {
    
//    if (document.getElementById(elementId) != null) {
//        $('#' + elementId).html(`
//					            <div id="op-alert" class="alert alert-success" role="success">
//		                		Success
//			                    </div>`);
//        setInterval(function () {
//            $('#op-alert').alert('close');
//        }, 6000);
//    }
//};
function AddSuccessNotifier(elementId) {

    let $target = null;

    // If modal is open → use floating notifier
    if ($('.modal.show').length > 0) {
        $target = $('#global-notifier');
    }
    // Else → use normal top notifier
    else if ($('#op-notifier').length > 0) {
        $target = $('#op-notifier');
    }

    if (!$target || $target.length === 0) return;

    const alertHtml = `
        <div class="alert alert-success" role="success">
            Success
        </div>`;

    $target.html(alertHtml);

    setTimeout(() => {
        $target.find('.alert').alert('close');
    }, 6000);
}

//function AddFailureNotifier(elementId) {
//    if (document.getElementById(elementId) != null) {
//        $('#' + elementId).html(`
//					            <div id="op-alert" class="alert alert-success" role="success">
//		                		Failure
//			                    </div>`);
//        setInterval(function () {
//            $('#op-alert').alert('close');
//        }, 10000);
//    }
//};
function AddFailureNotifier(elementId) {

    let $target = null;

    // If modal is open → use floating notifier
    if ($('.modal.show').length > 0) {
        $target = $('#global-notifier');
    }
    // Else → use normal top notifier
    else if ($('#op-notifier').length > 0) {
        $target = $('#op-notifier');
    }

    if (!$target || $target.length === 0) return;

    const alertHtml = `
        <div class="alert alert-success" role="success">
            Failure
        </div>`;

    $target.html(alertHtml);

    setTimeout(() => {
        $target.find('.alert').alert('close');
    }, 10000);
}



var api = {
    post: (url, postData) => {
        ShowDoingNotifier();
        //logger.logIt("Saving...", "", null, null, true, "info", 'toast-top-center');
        return new Promise((reslove, reject) => {
            $.ajax({
                url: url,
                type: "POST",
                data: postData,
                success: function (data) {
                    reslove(data);
                    //console.log(data);
                   // logger.logIt("Saved...", "", null, null, true, "info", 'toast-top-center');
                    ShowSuccessNotifiers();
                },
                error: function (xhr) {
                    reject(xhr)
                    let errMsg = "Unknown error";

                    if (xhr.responseJSON?.message)
                        errMsg = xhr.responseJSON.message;
                    else if (xhr.responseText)
                        errMsg = xhr.responseText;

                    // Store failure
                    $.ajax({
                        url: "/masters/postfailures",
                        type: "POST",
                        data: {
                            message: errMsg,
                            tenantId: 0
                        }
                    });
                    //logger.logIt("Failure...", "", null, null, true, "info", 'toast-top-center');
                    ShowFailureNotifiers();
                }
            });
        });
    },
    getbulk: (url) => {
        ShowLoadingNotifier();
        return new Promise((reslove, reject) => {
            $.ajax({
                url: url,
                type: "GET",
                success: function (data) {
                    reslove(data);
                    CloseOpAlert();
                },
                error: function (xhr) {
                    ShowFailureNotifiers();

                    let errMsg = "Unknown error";

                    if (xhr.responseJSON?.message)
                        errMsg = xhr.responseJSON.message;
                    else if (xhr.responseText)
                        errMsg = xhr.responseText;

                    // Store failure
                    $.ajax({
                        url: "/masters/postfailures",
                        type: "POST",
                        data: {
                            message: errMsg,
                            tenantId: 0
                        }
                    });
                    reject(xhr)
                }
            });
        });
    },
    get: (url) => {
        return new Promise((reslove, reject) => {
            $.ajax({
                url: url,
                type: "GET",
                success: function (data) {
                    reslove(data);
                },
                error: function (xhr) {
                    reject(xhr)

                    let errMsg = "Unknown error";

                    if (xhr.responseJSON?.message)
                        errMsg = xhr.responseJSON.message;
                    else if (xhr.responseText)
                        errMsg = xhr.responseText;

                    // Store failure
                    $.ajax({
                        url: "/masters/postfailures",
                        type: "POST",
                        data: {
                            message: errMsg,
                            tenantId: 0
                        }
                    });
                }
            });
        });
    },
    delete: (url) => {
        return new Promise((reslove, reject) => {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    reslove(data);
                },
                error: function (error) {
                    reject(error)
                }
            });
        });
    }
};
(function ($) {

    //re-set all client validation given a jQuery selected form or child
    $.fn.resetValidation = function () {

        var $form = $(this);//$("#" + formId);

        //reset jQuery Validate's internals
        $form.validate().resetForm();

        //reset unobtrusive validation summary, if it exists
        $form.find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul").empty();

        //reset unobtrusive field level, if it exists
        $form.find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty();

        return $form;
    };
})(jQuery);
$(function () {
    AppUtil.NavMenuUpdate();

    //checkbox change 

    $('.form-check-input[type=checkbox]').change(function () {
        var val =$(this).is(':checked');
        var name = $(this).attr('name');
        $("input[name='" + name+"']").val(val);
    });

    let idleTime = 0;
    const maxIdleTime = 59; // Set the idle time limit in minutes

    // Check idle time every minute
    const idleInterval = setInterval(() => {
        idleTime++;
        if (idleTime >= maxIdleTime) {
            window.location.href = '/Home/Logout'; // Redirect to the logout URL
        }
    }, 60000); // 1 minute interval

    // Reset idle time on any user activity
    const resetIdleTime = () => {
        //console.log(idleTime);
        idleTime = 0;
    };

    // Listen to events that indicate user activity
    document.onmousemove = resetIdleTime;
    document.onkeypress = resetIdleTime;
    document.onscroll = resetIdleTime;
    document.onclick = resetIdleTime;
    checkTokenExpiry();
    const tokenExpiryTime = localStorage.getItem("tokenExpiryTime");
    checkTokenExpiry(tokenExpiryTime * 1000);
});
function checkTokenExpiry(expiryTime) {
    if (!expiryTime) return;

    const logoutUrl = "/Home/Logout"; 
    const interval = 1000;

    // Periodically check the current time against the expiry time
    const intervalId = setInterval(() => {
        const currentTime = Date.now(); // Get current time in milliseconds
        //console.log("Current time:", currentTime, "Token expiry time:", expiryTime);

        if (currentTime >= expiryTime) {
            clearInterval(intervalId); // Stop further checks
           // alert("Your session has expired. You will be logged out.");
            window.location.href = logoutUrl; // Redirect to logout URL
        }
    }, interval);
}

$.fn.secureClick = function (selector, handler) {
    // Check if the first argument is actually the handler (direct bind)
    if (typeof selector === 'function') {
        handler = selector;
        selector = null; // No delegation
    }

    // Use .on() to handle both direct and delegated events
    this.on('click', selector, function (event) {
        event.preventDefault();
        let $btn = $(this);

        if ($btn.prop("disabled") || $btn.hasClass("processing")) return;

        $btn.prop("disabled", true).addClass("processing");

        Promise.resolve(handler.call(this, event))
            .finally(() => {
                $btn.prop("disabled", false).removeClass("processing");
            });
    });
    return this;
};