function updateSiteSettings() {
    $.ajax({
        url: "/SiteSetting/updateSiteSettings",
        dataType: "json",
        type: 'POST',
        data: $('#siteSettingForm').serialize(),
        success: function (data) {
            alertResponse(data);
        }
    });
}

function updateMailSettings() {
    $.ajax({
        url: "/SiteSetting/updateMailSettings",
        dataType: "json",
        type: 'POST',
        data: $('#mailSettingForm').serialize(),
        success: function (data) {
            alertResponse(data);
        }
    });
}


function updateSmsSettings() {
    $.ajax({
        url: "/SiteSetting/updateSmsSettings",
        dataType: "json",
        type: 'POST',
        data: $('#smsSettingForm').serialize(),
        success: function (data) {
            alertResponse(data);
        }
    });
}