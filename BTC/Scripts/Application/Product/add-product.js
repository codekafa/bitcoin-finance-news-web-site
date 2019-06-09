

function HandleOptions() {

    if ($('#MainPhotoLarge').fileinput !== undefined) {
        $('#MainPhotoLarge').fileinput({

            allowedFileExtensions: ['jpg', 'png'],
            maxFileSize: 5242,
            maxFileCount: 1,
            language: "tr",

            fileActionSettings: {
                showUpload: false
            },

            slugCallback: function (fn) {
                return fn.replace(/ /g, '');
            }
        });
    }

    $('#Name').blur(function () {
        $.ajax('/Blog/generateUrlFormat', {
            type: "GET",
            data: { uri: $(this).val() },
            success: function (result) {
                $('#Uri').val(result);
            }
        });
    });

};


function AddProduct() {

    $('#MainImage').on('change', function () {
        $('#IsChangeMainImage').val(true);
    });

    $('#form-addProduct').on('submit', function (e) {
        e.preventDefault();
        if ($('#IsPublished').prop('checked') == true) {
            $('#IsPublish').val(true);
        }
        var dat = new FormData($('#form-addProduct').get(0));

        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/Product/addProduct',
            success: function (d) {
                alertResponse(d);
                if (d.IsSuccess === true) {
                    setTimeout(function () { location.reload(); }, 500);
                }
               
            }
        });
    });
};

$(document).ready(function () {
    HandleOptions();
    AddProduct();
});