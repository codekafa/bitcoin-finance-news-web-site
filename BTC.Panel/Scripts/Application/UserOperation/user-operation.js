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

function getCities() {
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Admin/getCities',
        success: function (d) {

            var city_val = $('#CompanyCityID').val();
            console.log(city_val);
            $(d).each(function (index,val) {
                var opt = "";
                if (city_val == val.ID) {
                    opt = "<option value='" + val.ID + "' selected>" + val.Name + "</option>";
                } else {
                    opt = "<option value='" + val.ID + "' >" + val.Name + "</option>";
                }
                $('#CompanyCity').append(opt);
               

            });


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

function photoChange() {
    $('#ProfilePhotoUrl').on('change', function () {
        $('#IsChangeProfilePhoto').val(true);
    });
}

