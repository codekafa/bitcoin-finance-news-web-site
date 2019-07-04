

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

};

function AddPage() {


    $('#form-addPage').on('submit', function (e) {
        e.preventDefault();
        var post_value = $('.ql-editor').html();
        $('#Body').val(post_value);

        if ($('#IsPublished').prop('checked') == true) {
            $('#IsPublish').val(true);
        }
        var dat = new FormData($('#form-addPage').get(0));
        $.ajax({
            data: dat,
            type: 'post',
            dataType: 'json',
            contentType: false,
            processData: false,
            url: '/Page/addOrEditPage',
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
    AddPage();
});



