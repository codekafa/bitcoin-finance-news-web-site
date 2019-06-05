
/*posts*/

function getCategories(element) {
    $.ajax('/Blog/getCategories', {
        type: "GET",
        async: false,
        success: function (result) {
            $('#' + element).html("");
            $('#' + element).append("<option value=''>Kategori Seçiniz</option>");
            $.each(result, function (i, item) {
                if (item.IsActive == true) {
                    var opt = "<option value='" + item.ID + "'>" + item.Name + "</option>";
                    $('#' + element).append(opt);
                }
            });
        }
    });
}

function getWriters(element) {

    $.ajax('/Admin/getWriters', {
        type: "GET",
        contentType: 'json',
        success: function (data) {

            console.log(data);
            $('#' + element).html("");
            $('#' + element).append("<option value=''>Yazar Seçiniz</option>");

            for (var i = 0; i < data.length; i++) {
                var opt = "<option value='" + data[i].ID + "'>" + data[i].FullName + "</option>";
                $('#' + element).append(opt);
            }
        }
    });
}

function getPostList() {

    var cate_id = $('#category_id').val();
    var wri_id = $('#user_id').val();

    var p = false;

    if ($('#publish').is(":checked") == true) {
        p = true;
    }

    if (cate_id == 0 || cate_id == "" || wri_id == 0 || wri_id == "") {
        dangerAlert("Kategori ve yazar seçimi zorunldur!");
        return;
    }

    $.ajax('/Admin/_GetPosts', {
        type: "GET",
        dataType: "html",
        data: { category_id: cate_id, user_id: wri_id, publish: p },
        success: function (result) {
            $('#userList').html("");
            $('#userList').append(result);
        }
    });
}

/*posts*/


/*comments*/

function getComments() {
    $.ajax('/Admin/getWaitingComments', {
        type: "GET",
        dataType: "json",
        success: function (result) {
            $('#commentListBody').html("");
            $.each(result, function (i, item) {
                var tr = "<tr id='comment_" + item.ID + "'>";
                tr += "<td>" + item.Description + "</td>";
                tr += "<td>" + item.Name + "</td>";
                tr += "<td>" + item.Email + "</td>";
                tr += "<td>" + item.CreateDateAsString + "</td>";
                tr += "<td>" + item.PostTitle + "</td>";
                tr += "<td><button type='button' class='btn btn-xs btn-success' onclick='publishComment(" + item.ID + ")'>Yayınla</button><button type='button' class='btn btn-xs btn-danger' onclick='deleteComment(" + item.ID + ")'>Sil</button></td>";
                tr += "</tr>"

                $('#commentListBody').append(tr);
            });
        }
    });
}
function publishComment(id) {

    $.ajax('/Admin/publishComment', {
        type: "GET",
        dataType: "json",
        data: { comment_id: id },
        success: function (result) {
            alertResponse(result);
            if (result.IsSuccess == true) {
                getComments();
            }
        }
    });

}
function deleteComment(id) {

    $.ajax('/Admin/deleteComment', {
        type: "GET",
        dataType: "json",
        data: { comment_id: id },
        success: function (result) {
            alertResponse(result);
            if (result.IsSuccess == true) {
                getComments();
            }
        }
    });
}


/*comments*/


/*categories*/


function getCategoryList() {
    $.ajax('/Blog/getCategories', {
        type: "GET",
        async: false,
        success: function (result) {

            $.each(result, function (i, item) {
                var tr = "<tr>";
                tr += "<td>" + item.Name + "<input type='hidden' id='category_name_" + item.ID + "' value=" + item.Name +"  /></td>";
                tr += "<td>" + item.Uri + "<input type='hidden' id='category_uri_" + item.ID + "' value=" + item.Uri +"  /></td>";
                if (item.IsActive == true) {
                    tr += "<td><span class='alert alert-success btn-xs' style='padding:0px;'>Aktif</span></td>";
                    tr += "<td><button type='button' onclick='updateCategoryState(" + item.ID + ",true)' class='btn btn-xs btn-danger'>Pasife Al</button><button type='button' onclick='editCategory(" + item.ID + ")' class='btn btn-xs btn-warning'>Düzenle</button></td>";
                } else {
                    tr += "<td><span class='alert alert-danger btn-xs' style='padding:0px;'>Pasif</span></td>";
                    tr += "<td><button type='button' onclick='updateCategoryState(" + item.ID + ",false)' class='btn btn-xs btn-success'>Aktife Al</button><button type='button' onclick='editCategory(" + item.ID + ")' class='btn btn-xs btn-warning'>Düzenle</button></td>";
                }

                tr += "</tr>";
                $('#categoryListBody').append(tr);

            });
        }
    });
}

function editCategory(id) {
    clearInputs();
    $('#CategoryID').val(id);
    $('#CategoryName').val($('#category_name_'+id).val());
    $('#CategoryUri').val($('#category_uri_' + id).val());
}

function clearInputs() {
    $('#CategoryID').val("");
    $('#CategoryName').val("");
    $('#CategoryUri').val("");
}

function addCategory() {
    var obj = new Object();
    obj.CategoryID = $('#CategoryID').val();
    obj.Name = $('#CategoryName').val();
    obj.Uri = $('#CategoryUri').val();

    $.ajax('/Admin/addOrEditCategory', {
        type: "Post",
        dataType: "json",
        data: obj,
        success: function (result) {
            alertResponse(result);
            if (result.IsSuccess == true) {
                getCategoryList();
                clearInputs();
            }
        }
    });

}

function updateCategoryState(id, state) {

    $.ajax('/Admin/updateCategoryState', {
        type: "GET",
        dataType: "json",
        data: { category_id: id, state: state },
        success: function (result) {
            alertResponse(result);
            if (result.IsSuccess == true) {
                getCategoryList();
            }
        }
    });

}


/*categories*/