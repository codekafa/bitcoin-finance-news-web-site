


function HandleOptions() {

    var toolbarOptions = [
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        ['blockquote', 'code-block'],
        ['image'],
        [{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        [{ 'direction': 'rtl' }],                         // text direction

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        [{ 'font': [] }],
        [{ 'align': [] }],

        ['clean']                                         // remove formatting button
    ];

    var quill = new Quill('#BodyEditor', {
        modules: {
            toolbar: toolbarOptions
        },
        theme: 'snow'
    });

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

    $('#Title').blur(function () {
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


    $('#form-editBlogPost').on('submit', function (e) {
        e.preventDefault();
        var post_value = $('.ql-editor').html();

        if ($('#IsPublished').prop('checked') == true) {
            $('#IsPublish').val(true);
        }

        $('#Body').val(post_value);
        var dat = new FormData($('#form-editBlogPost').get(0));
        dat.append('MainImageLarge', $('#MainImage').val());

        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/Blog/addOrEditPost',
            success: function (d) {
                alertResponse(d);
            }
        });
    });

};

function GetCategories() {
    $.ajax('/Blog/getCategories', {
        type: "GET",
        async: false,
        success: function (result) {
            $('#CategoryID').html("");
            $('#CategoryID').append("<option>Kategori Seçiniz</option>");
            $.each(result, function (i, item) {

                if (item.IsActive == true) {
                    var opt = "<option value='" + item.ID + "'>" + item.Name + "</option>";
                    $('#CategoryID').append(opt);
                }

            });
        }
    });
};

function GetPostByID() {

    var post_id = $('#Id').val();
    $.ajax('/Blog/getPostById', {
        type: "GET",
        data: { post_id: post_id },
        async: false,
        success: function (result) {
            if (result != null && result.IsSuccess == true) {

                var post = result.ResultData;

                $('#Title').val(post.Title);
                $('.ql-editor').html(post.Body);
                //$('#CategoryID').attr('data-selected', post.CategoryID);

                console.log(post.CategoryID);

                $('#CategoryID').val(post.CategoryID);
                $('#Uri').val(post.Uri);
                $('#MetaDescription').val(post.MetaDescription);
                $('#MetaKeywords').val(post.MetaKeywords);
                $('#MetaTitle').val(post.MetaTitle);
                $('#Tags').val(post.Tags);
                $('#Summary').val(post.Summary);
                $('#TopPhotoUrl').val(post.TopPhotoUrl);

                $('#MainImage').attr('src', '/Images/_post/large/' + post.TopPhotoUrl);

                if (post.IsPublish == true) {
                    $('#IsPublished').prop('checked', true);
                    $('#IsPublish').val(true);
                }

            } else {
                alertResponse(result);
            }
        }
    });
}

$(document).ready(function () {
    GetCategories();
    HandleOptions();
    GetPostByID();
});

