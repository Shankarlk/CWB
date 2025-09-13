
$(function () {
    $("#error-password").text(" ").css("color", "red");
    $("#ChangePassBtn").on("click", function () {
        var EPPassword = $("#NewPassword").val();
        var CurrentPassword = $("#CurPassword").val();
        var EPCfmPassword = $("#CnfPassword").val();
        var regexpassUpper = /[A-Z]/;
        var regexpassLower = /[a-z]/;
        if (!regexpassUpper.test(EPCfmPassword)) {
            $("#error-password").text("Password must contain at least one capital letter.").css("color", "red");
            return false;
        } else if (!regexpassLower.test(EPCfmPassword)) {
            $("#error-password").text("Password must contain at least one lowercase letter.").css("color", "red");
            return false;
        } else {
            $("#error-password").text(" ").css("color", "red");
        }
        if (EPCfmPassword != EPPassword) {
            var newNamevalidate = document.getElementById('CnfPassword');
            newNamevalidate.style.border = '2px solid red';
            $("#error-password").text("Confirm Password must be same as New Password.").css("color", "red");
            return false;
        } else {
            var newNamevalidate = document.getElementById('CnfPassword');
            newNamevalidate.style.border = '';
        }
        if (EPCfmPassword.length <= 6) {
            $("#error-password").text("Password must contain at least 6 letters.").css("color", "red");
            var newNamevalidate = document.getElementById('CnfPassword');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            $("#error-password").text(" ").css("color", "red");
            var newNamevalidate = document.getElementById('CnfPassword');
            newNamevalidate.style.border = '';
        }
        api.get("/ChangePassword/ChangePassword?Password=" + EPPassword + "&CurrentPassword=" + CurrentPassword)
            .then((response) => {
                $("#error-password").text(" ").css("color", "red");

                var data = response;   // backend returns full object on success
                var userrowData = {
                    email: data.email,
                    newPassword: data.password,
                    confirmPassword: data.password,
                    token: "ersasdsada"
                };

                const ipAddress = window.location.hostname;
                alert("Password Reset Successfully!");

                api.post(`http://${ipAddress}:9003/account/ResetPassword`, userrowData, {
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json",
                    },
                }).then((response) => {
                    console.log("ResetPassword Success:", response);
                });
            })
            .catch((error) => {
                if (error.responseJSON.message && error.status === 400) {
                    $("#error-password").text(error.responseJSON.message).css("color", "red");
                } else {
                    console.error("Unexpected Error:", error);
                }
            });

    });
});