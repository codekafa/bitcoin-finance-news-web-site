

function getMyPosts() {
    $.ajax('/News/getMyNews', {
        type: "GET",
        success: function (result) {
            $('#postListTableBody').html("");
            $.each(result, function (i, item) {

                var tr = "<tr>";
                tr += "<td>" + "<image  src='/Images/_post/small/" + item.TopPhotoUrl + "' style='max-width:200px;' />" + "</td>";
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
                tr += "<a href='/haber-duzenle/" + item.ID +"' target='_blank' title='Düzenle' class='btn btn-xs btn-primary' > <i class='fa fa-edit'> </i>  </a>";

                if (item.IsPublish == true) {
                    tr += "<button type='button' class='btn btn-xs btn-danger'  title='Yayından Kaldır' onclick='unPublishPost(" + item.ID +")' >   <i class='fa fa-remove'> </button>";
                } else {
                    tr += "<button type='button' class='btn btn-xs btn-warning'  title='Yayınla' onclick='publishPost(" + item.ID +")' >  <i class='fa fa-check-circle'> </i> </button>";
                }
                tr += "</td>";

                $('#postListTableBody').append(tr);
            });

        }
    });
}


function publishPost(id) {

    $.ajax('/News/updatePublishNews', {
        type: "GET",
        data: {post_id:id,p:true},
        success: function (result) {
            successAlert("Güncelleme başarılı!");
            getMyPosts();
        }
    });
    

}

function unPublishPost(id) {
    $.ajax('/News/updatePublishNews', {
        type: "GET",
        data: { post_id: id, p: false },
        success: function (result) {
            successAlert("Güncelleme başarılı!");
            getMyPosts();
        }
    });
}

$(document).ready(function () {
    getMyPosts();
});
