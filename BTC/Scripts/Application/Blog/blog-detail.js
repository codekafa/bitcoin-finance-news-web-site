

function HandleOperation() {

    $('#comment-form').on('submit', function (e) {
        e.preventDefault();
        var dat = new FormData($('#contact-form').get(0));
        console.log(dat);
        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/Blog/addComment',
            success: function (d) {
                alertResponse(d);

                if (d.IsSuccess == true) {
                    clearForm();
                }

            }
        });
    });
}

function clearForm() {
    $('#contact-form .form-control').val("");
}

$(document).ready(function () {
    HandleOperation();
});


