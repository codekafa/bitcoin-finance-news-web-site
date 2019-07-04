


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



    $('#Title').blur(function () {
        $.ajax('/Page/generateUrlFormat', {
            type: "GET",
            data: { uri: $(this).val() },
            success: function (result) {
                $('#Uri').val(result);
            }
        });
    });

    $('#IsPublished').on('change', function () {
        if ($('#IsPublished').prop('checked') == true) {
            $('#IsPublish').val(true);
        } else {
            $('#IsPublish').val(false);
        }

    });




    $('#form-editPage').on('submit', function (e) {
        e.preventDefault();
        var page_value = $('.ql-editor').html();

        if ($('#IsPublished').prop('checked') == true) {
            $('#IsPublish').val(true);
        }

        $('#Body').val(page_value);
        var dat = new FormData($('#form-editPage').get(0));
        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/Page/addOrEditPage',
            success: function (d) {
                alertResponse(d);
            }
        });
    });

};


function GetPageByID() {
    var page_id = $('#Id').val();
    $.ajax('/Page/getPageById', {
        type: "GET",
        data: { page_id: page_id },
        async: false,
        success: function (result) {
            if (result != null && result.IsSuccess == true) {

                var post = result.ResultData;

                $('#Title').val(post.Title);
                $('.ql-editor').html(post.Body);

                $('#Uri').val(post.Uri);
                $('#MetaDescription').val(post.MetaDescription);
                $('#MetaKeywords').val(post.MetaKeywords);
                $('#MetaTitle').val(post.MetaTitle);
                $('#Tags').val(post.Tags);
                if (post.IsPublish == true) {
                    $('#IsPublished').prop('checked', true);
                    $('#IsPublish').val(true);
                } else {
                    $('#IsPublish').val(false);
                }

            } else {
                alertResponse(result);
            }
        }
    });
}

$(document).ready(function () {
    HandleOptions();
    GetPageByID();
});

