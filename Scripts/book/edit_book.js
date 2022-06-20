//編輯書籍script
$(document).ready(function () {
    $(".btn-edit-book").on('click', function () {
        let book_id = $(this).siblings('.book_id').text();
        get_edit_book_data(book_id);

        //上傳btn
        $("#btn_submit_edit").on('click', function () {
            store_edit_book_data();
        })
    })
})

//取得欄位資料
function store_edit_book_data() {
    let book_title = $("#edit_book_title").val(); //書籍名稱
    let book_content = $("#edit_book_content").val(); //書籍說明
    let book_id = $("#edit_book_id").text().trim(); //book_id

    let url = '/Book/StoreEditBook'; //目標uri
    url = `${url}?book_id=${book_id}&book_title=${book_title}&book_content=${book_content}`;

    let upload_file = $('#edit_upload_image')[0].files;

    let form_data = new FormData()
    if (upload_file.length > 0) {
        //先建立formData
        form_data.append('upload_image', upload_file[0])
    }

    //建立ajax連線，儲存資料
    $.ajax({
        url: url,
        method: 'post',
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        success: function (res) {
            console.log(res);
            window.location.reload();
        },
        error: function (res) {
            console.log(res);
        }
    })
}

//取得編輯資料
function get_edit_book_data(book_id) {
    console.log(book_id);
    let url = "/Book/GetEditBook";

    $.ajax({
        url: url,
        method: 'post',
        data: {
            book_id: book_id,
        }, success: function (res) {
            $("#edit_book_id").text(res['book_id']);
            $("#edit_book_title").val(res['book_title']);
            $("#edit_book_content").val(res['book_content']);
            $("#online_image").attr("src", res['front_cover']);
        }
    })
}