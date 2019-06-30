


function HandleOptions() {

    $('#Name').blur(function () {
        $.ajax('/Blog/generateUrlFormat', {
            type: "GET",
            data: { uri: $(this).val() },
            success: function (result) {
                $('#Uri').val(result);
            }
        });
    });

    $('#MainImage').on('change', function () {
        $('#IsChangeMainImage').val(true);
    });


    $('#form-editProduct').on('submit', function (e) {
        e.preventDefault();

        if ($('#IsPublished').prop('checked') == true) {
            $('#IsPublish').val(true);
        }
        var dat = new FormData($('#form-editProduct').get(0));

        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/Product/editProduct',
            success: function (d) {
                alertResponse(d);
            }
        });
    });
};

function removePhoto(id) {

    if (confirm("Seçili fotoğraf silinecektir. Onaylıyor musunuz?") == true) {
        $.ajax('/Product/removePhoto', {
            type: "GET",
            data: { photo_id: id },
            success: function (result) {
                alertResponse(result);
                if (result.IsSuccess == true) {
                    $('#photo_' + id).remove();
                }

            }
        });
    }
}


function updateIsMain(id, p_id) {

    if (confirm("Seçili fotoğraf kapak fotoğrafı olarak güncellenecektir. Onaylıyor musunuz?") == true) {

        $.ajax('/Product/updateIsMain', {
            type: "GET",
            data: { photo_id: id, product_id: p_id },
            success: function (result) {
                alertResponse(result);
                if (result.IsSuccess == true) {
                    location.reload();
                }
            }
        });

    }
}


$(document).ready(function () {
    HandleOptions();
});


function addPhotos() {

    var dat = new FormData($('#addPhotoForm').get(0));


    var file_count = $('#PhotoList')[0].files.length;

    if (file_count <= 0) {
        dangerAlert("En az 1 adet fotoğraf seçmelisiniz!");
        return;
    }

    $.ajax({
        data: dat,
        type: 'post',
        dataType: 'json',
        contentType: false,
        processData: false,
        url: '/Product/addPhotos',
        success: function (d) {
            alertResponse(d);
            if (d.IsSuccess == true) {
                location.reload();
            }
        }
    });

}
