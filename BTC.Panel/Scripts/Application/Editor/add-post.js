

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

};

function GetCategories() {
    $.ajax('/Blog/getCategories', {
        type: "GET",
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

function AddBlogPost() {

    $('#MainImage').on('change', function () {
        $('#IsChangeMainImage').val(true);
    });

    $('#form-addBlogPost').on('submit', function (e) {
        e.preventDefault();
        var post_value = $('.ql-editor').html();
        $('#Body').val(post_value);

        if ($('#IsPublished').prop('checked') == true) {
            $('#IsPublish').val(true);
        }
        var dat = new FormData($('#form-addBlogPost').get(0));
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
                if (d.IsSuccess === true) {
                    setTimeout(function () { location.reload(); }, 500);
                }
               
            }
        });
    });
};

$(document).ready(function () {
    HandleOptions();
    GetCategories();
    AddBlogPost();
});