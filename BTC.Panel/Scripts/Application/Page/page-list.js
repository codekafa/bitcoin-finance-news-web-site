

function getPages() {
    $.ajax('/Page/getPages', {
        type: "GET",
        success: function (result) {
            $('#pageListTableBody').html("");
            $.each(result, function (i, item) {

                var tr = "<tr>";
                tr += "<td> " + item.Title + "</td>";
                tr += "<td>";

                if (item.IsPublish == true) {
                    tr += "<h6><span class='badge badge-success'><i class='fa fa-check'></i></span></h6>";
                } else {
                    tr += "<h6><span class='badge badge-danger'><i class='fa fa-times'></i></span></h6>";
                }
                tr += "</td>";
                tr += "<td> " + item.ViewCount + "</td>";
                tr += "<td>";
                tr += "<a href='/sayfa-duzenle/" + item.ID +"' target='_blank' title='Düzenle' class='btn btn-xs btn-primary' >  <i class='fa fa-edit'> </i> </a>";

                if (item.IsPublish == true) {
                    tr += "<button type='button' class='btn btn-xs btn-danger' title='Yayından Kaldır' onclick='unPublishPage("+item.ID+")' >  <i class='fa fa-remove'> </i> </button>";
                } else {
                    tr += "<button type='button' class='btn btn-xs btn-warning'  title='Yayına Al' onclick='publishPage(" + item.ID +")' >  <i class='fa fa-check-circle'> </i> </button>";
                }
                tr += "</td>";

                $('#pageListTableBody').append(tr);
            });

        }
    });
}


function publishPage(id) {

    $.ajax('/Page/updatePublishPage', {
        type: "GET",
        data: {page_id:id,p:true},
        success: function (result) {
            successAlert("Güncelleme başarılı!");
            getPages();
        }
    });
    

}

function unPublishPage(id) {
    $.ajax('/Page/updatePublishPage', {
        type: "GET",
        data: { page_id: id, p: false },
        success: function (result) {
            successAlert("Güncelleme başarılı!");
            getPages();
        }
    });
}

$(document).ready(function () {
    getPages();
});
