function loginUser() {
    var obj = new Object();
    obj.UserName = $('#UserName').val();
    obj.Password = $('#Password').val();
    $.ajax({
        url: "/Security/LoginUser",
        dataType: "json",
        type: 'POST',
        data: {
            loginUser: obj
        },
        success: function (data) {
            alertResponse(data);
            if (data.IsSuccess == true) {
                location.reload();
            } 
        }
    });

}

function changePassword() {
    var obj = new Object();
    obj.OldPassword = $('#Password').val();
    obj.Password = $('#NewPassword').val();
    obj.PasswordAgain = $('#NewPasswordAgain').val();
    $.ajax({
        url: "/Profile/changePassword",
        dataType: "json",
        type: 'POST',
        data: {
            changeModel: obj
        },
        success: function (data) {

            alertResponse(data);
            if (data.IsSuccess == true) {
                setTimeout(function () {
                    window.location.href = "/";
                }, 5000);
            } 

        }
    });

}

function submitProfileForm () {
    $.ajax({
        url: "/Profile/updateProfile",
        dataType: "json",
        type: 'POST',
        data: $('#profileForm').serialize(),
        success: function (data) {
            alertResponse(data);
        }
    });
}

