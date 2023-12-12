var AppUtil = {
    NavMenuUpdate: () => {
        //remove active navbar..
        $("#side-menu").find("li.menuitem-active a").removeClass("active");
        $("#side-menu").find("li.menuitem-active").removeClass("menuitem-active");
        var navMenuID = $("#hdn-nav-menu").val();
        $("a[data-navid='" + navMenuID + "']").addClass("active");
        $("a[data-navid='" + navMenuID + "']").parents("li").addClass("menuitem-active");
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
    ProcessTemplateData: (templateId, dataObj) => {
        var templateElement = $("#" + templateId).html();
        for (var key in dataObj) {
            templateElement = templateElement.replaceAll("{" + key + "}", dataObj[key])
        }
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

function AddSuccessNotifier(elementId) {
    
    if (document.getElementById(elementId) != null) {
        $('#' + elementId).html(`
					            <div id="op-alert" class="alert alert-success" role="success">
		                		Success
			                    </div>`);
        setInterval(function () {
            $('#op-alert').alert('close');
        }, 4000);
    }
};
function AddFailureNotifier(elementId) {
    if (document.getElementById(elementId) != null) {
        $('#' + elementId).html(`
					            <div id="op-alert" class="alert alert-success" role="success">
		                		Failure
			                    </div>`);
        setInterval(function () {
            $('#op-alert').alert('close');
        }, 4000);
    }
};


var api = {
    post: (url, postData) => {
        return new Promise((reslove, reject) => {
            $.ajax({
                url: url,
                type: "POST",
                data: postData,
                success: function (data) {
                    reslove(data);
                    ShowSuccessNotifiers();
                },
                error: function (error) {
                    reject(error)
                    ShowFailureNotifiers();
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
                error: function (error) {
                    reject(error)
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
});
