//新增書籍script

//上傳btn
$("#btn_submit_create").on('click', function () {
    get_create_book_data();
})

//取得欄位資料
function get_create_book_data() {
    let book_title = $("#create_book_title").val(); //書籍名稱
    let book_content = $("#create_book_content").val(); //書籍說明

    let url = '/Book/CreateBook'; //目標uri
    url = `${url}?book_title=${book_title}&book_content=${book_content}`;

    //input:file中的檔案
    let upload_file = $('#create_upload_image')[0].files;
    //檔案的副檔名
    let file_type = upload_file[0]['name'].split('.')[upload_file[0]['name'].split('.').length - 1]
    let file_type_check = false;

    $.each(['jpg', 'JPG', 'JPEG', 'png'], function (index, value) {
        if (file_type.includes(value)) file_type_check = true;
    })
    if (!file_type_check) {
        //不符合檔案格式要求則清空input:file
        upload_file = [];
        alert('上傳的檔案不符合系統要求格式\n請上傳jpg, JPG, JPEG, png');
    };

    //未選取檔案
    if (upload_file.length <= 0) {
        window.alert('請先上傳檔案');
    }

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