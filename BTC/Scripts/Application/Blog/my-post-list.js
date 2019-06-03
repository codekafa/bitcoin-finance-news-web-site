

function getMyPosts() {
    $.ajax('/Blog/getMyPosts', {
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
                tr += "<a href='/makale-duzenle/"+item.ID+"' target='_blank' class='btn btn-xs btn-primary' > Düzenle </a>";

                if (item.IsPublish == true) {
                    tr += "<button type='button' class='btn btn-xs btn-danger' onclick='unPublishPost("+item.ID+")' >  Kaldır </button>";
                } else {
                    tr += "<button type='button' class='btn btn-xs btn-warning' onclick='publishPost(" + item.ID +")' > Yayınla </button>";
                }
                tr += "</td>";

                $('#postListTableBody').append(tr);
            });

        }
    });
}


function publishPost(id) {

    $.ajax('/Blog/updatePublishPost', {
        type: "GET",
        data: {post_id:id,p:true},
        success: function (result) {
            successAlert("Güncelleme başarılı!");
            getMyPosts();
        }
    });
    

}

function unPublishPost(id) {
    $.ajax('/Blog/updatePublishPost', {
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
