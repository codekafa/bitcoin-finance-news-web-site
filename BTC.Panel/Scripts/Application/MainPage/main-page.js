

function getSliders() {

    $.ajax('/MainPage/_GetSliders', {
        type: "GET",
        success: function (result) {
            $('#sliders').val(result);
        }
    });

}


function addSlider() {
    $('#addSliderForm').on('submit', function (e) {
        e.preventDefault();

        var dat = new FormData($('#addSliderForm').get(0));
        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/MainPage/addSlider',
            success: function (d) {
                alertResponse(d);
                if (d.IsSuccess === true) {
                    getSliders();
                }
            }
        });
    });
};



function saveSettings() {

    $('#mainPageSettingForm').on('submit', function (e) {
        e.preventDefault();

        var dat = new FormData($('#mainPageSettingForm').get(0));
        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/MainPage/saveMainPageSettings',
            success: function (d) {
                alertResponse(d);
            }
        });
    });
}


function deleteSlider(id) {


    var txt;
    var r = confirm("Seçili slider bilgisi silinecektir.Onyalıyor musunuz?");
    if (r == true) {
        $.ajax('/MainPage/deleteSlider', {
            type: "GET",
            data: { slider_id: id },
            success: function (result) {
                alertResponse(result);
                if (result.IsSuccess === true) {
                    getSliders();
                }
            }
        });
    }
}
