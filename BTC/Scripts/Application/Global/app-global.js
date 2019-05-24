toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}


function successAlert(message) {
    toastr["success"](message)
}
function dangerAlert(message) {
    toastr["warning"](message)
}

function dangerAlertList(messages) {
    $.each(messages, function (key, value) {
        toastr["danger"](value);
    });
}


function selectPage() {
    var s_p = $('#select-page').val();
    $('.' + s_p).addClass('active');
}

$(document).ready(function () {
    $('.phoneMask').usPhoneFormat({
        format: '(xxx) xxx-xxxx',
    });
    selectPage();
});

$(window).on('load', function () {
    $("#loading").delay(1000).fadeOut(800);
})
