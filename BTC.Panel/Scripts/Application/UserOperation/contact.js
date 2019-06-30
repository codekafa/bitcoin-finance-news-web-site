function HandleOptions() {
    $('#contact-form').on('submit', function (e) {

        var obj = new Object();
        obj.Name = $('#Name').val();
        obj.CompanyName = $('#CompanyName').val();
        obj.Phone = $('#Phone').val();
        obj.Email = $('#Email').val();
        obj.Message = $('#Message').val();

        $.ajax({
            data:obj,
            type: 'POST',
            dataType: 'json',
            url: '/Home/sendContactMessage',
            success: function (d) {
                alertResponse(d);
                if (d.IsSuccess === true) {
                    clearForm();
                }
            }
        });
    });
}


function clearForm() {
    $('.form-control').val("");
}

$(document).ready(function () {
    HandleOptions();
});