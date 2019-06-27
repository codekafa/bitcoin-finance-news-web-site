function loginUser() {
    var obj = new Object();
    obj.UserName = $('#UserName').val();
    obj.Password = $('#Password').val();
    $.ajax({
        url: "/Security/loginUser",
        dataType: "json",
        type: 'POST',
        data: {
            loginUser: obj
        },
        success: function (data) {
            if (data.IsSuccess == true) {
                location.reload();
            } else {
                dangerAlert(data.Message)
            }
        }
    });
}

function logoutUser() {
    $.ajax({
        url: "/Security/logoutUser",
        dataType: "json",
        type: 'POST',
        data: {
            loginUser: obj
        },
        success: function (data) {
            if (data.IsSuccess == true) {
                location.reload();
            } else {
                dangerAlert(data.Message)
            }
        }
    });
}

function registerUser() {
    var obj = new Object();
    obj.FirstName = $('#FirstName').val();
    obj.LastName = $('#LastName').val();
    obj.Phone = $('#Phone').val();
    obj.Email = $('#Email').val();
    obj.Password = $('#Password').val();
    obj.PasswordAgain = $('#PasswordAgain').val();
    obj.IsVip = $('#IsVip').prop('checked');
    $.ajax({
        url: "/Security/registerUser",
        dataType: "json",
        type: 'POST',
        data: {
            registerUser: obj
        },
        success: function (data) {
            if (data.IsSuccess == true) {
                $('#login').css('display', 'none');
                $('#' + data.Message).css('display', 'block');
            } else {
                dangerAlert(data.Message)
            }
        }
    });

}

function sendChangePasswordUri() {
    $.ajax({
        url: "/Security/sendChangePasswordMail",
        dataType: "json",
        type: 'GET',
        data: {
            email: $('#Email').val()
        },
        success: function (data) {
            if (data.IsSuccess == true) {
                successAlert(data.Message)
            } else {
                dangerAlert(data.Message)
            }
        }
    });
}

function changePasswordUrl() {

    var obj = new Object();
    obj.Password = $('#Password').val();
    obj.PasswordAgain = $('#PasswordAgain').val();
    obj.ProcessGuid = $('#ProcessGuid').val();
    $.ajax({
        url: "/Security/changePassword",
        dataType: "json",
        type: 'POST',
        data: {
            changeModel: obj
        },
        success: function (data) {
            if (data.IsSuccess == true) {
                successAlert(data.Message);
                setTimeout(function () {
                    window.location.href = "/";
                }, 5000);

            } else {
                dangerAlert(data.Message)
            }
        }
    });

}

function updateUser() {
    $('#profileForm').on('submit', function (e) {
        var dat = new FormData($('#profileForm').get(0));
        e.preventDefault();
        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/Profile/updateProfile',
            success: function (d) {
                alertResponse(d);
            }
        });
    });
}

function changePassword() {

    var obj = new Object();
    obj.OldPassword = $('#OldPassword').val();
    obj.Password = $('#Password').val();
    obj.PasswordAgain = $('#PasswordAgain').val();
    $.ajax({
        url: "/Profile/changePassword",
        dataType: "json",
        type: 'POST',
        data: {
            changeModel: obj
        },
        success: function (data) {
            alertResponse(data);
        }
    });

}


function photoChange() {
    $('#ProfilePhotoUrl').on('change', function () {
        $('#IsChangeProfilePhoto').val(true);
    });
}


