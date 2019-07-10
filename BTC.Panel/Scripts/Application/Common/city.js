

function updateOrEditCity() {

    var city = new Object();
    city.ID = $('#ID').val();
    city.Name = $('#Name').val();
    city.Uri = $('#Uri').val();


    if (city.Name == "" || city.Name == undefined) {
        dangerAlert("Şehir ismi zorunlu alandır!");
        return false;
    }

    if (city.Uri == "" || city.Uri == undefined) {
        dangerAlert("Şehir url alanı zorunludur!");
        return false;
    }

    $.ajax('/Admin/addOrEditCity', {
        type: "POST",
        dataType: "json",
        data: { city },
        success: function (result) {
            alertResponse(result);
            if (result.IsSuccess == true) {
                location.reload();
            }
        }
    });

}


function editCity(id) {

    $('#ID').val(id);
    $('#Name').val($('#name_' + id).val());
    $('#Uri').val($('#uri_' + id).val());

}


function clearInputs() {

    $('#ID').val("");
    $('#Name').val("");
    $('#Uri').val("");

}



function removeCity(id) {

    if (confirm("Seçili şehir silinecektir. Emin misiniz?") == true) {

        $.ajax('/Admin/deleteCity', {
            type: "GET",
            dataType: "json",
            data: { city_id: id },
            success: function (result) {
                alertResponse(result);
                if (result.IsSuccess == true) {
                    location.reload();
                }
            }
        });
    } 
}


function loadCities() {

    $('#Name').blur(function () {
        $.ajax('/Admin/generateUriFormat', {
            type: "GET",
            data: { title: $(this).val() },
            success: function (result) {
                $('#Uri').val(result);
            }
        });
    });

}