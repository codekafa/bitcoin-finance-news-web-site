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

