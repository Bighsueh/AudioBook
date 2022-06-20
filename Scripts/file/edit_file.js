//編輯書籍script
$(document).ready(function () {
    $(".btn-edit-file").on('click', function () {
        let content_id = $(this).siblings('.content_id').text();
        get_edit_file_data(content_id);

        //上傳btn
        $("#btn_submit_edit").on('click', function () {
            store_edit_file_data();
        })
    })
})

//取得欄位資料
function store_edit_file_data() {
    let content_id = $("#edit_content_id").text().trim(); //content_id
    let content_name = $("#edit_content_name").val();     //檔案名稱
    let file_type = $("#edit_file_type").val(); //檔案類型
    let check_file_length = 1;
    let url;
    
    
    //音檔上傳
    if (file_type === '音檔') {
        url = `/File/StoreAudioFile?content_id=${content_id}&content_name=${content_name}`;

        //get input:file
        let slow_audio = $("#edit_slow_audio")[0].files;
        let english_audio = $("#edit_english_audio")[0].files;
        let bilingual_audio = $("#edit_bilingual_audio")[0].files;

        //未選取檔案
        if (slow_audio.length <= 0) {
            window.alert('請先上傳檔案');
            check_file_length = 0;
        }
        if (english_audio.length <= 0) {
            window.alert('請先上傳檔案');
            check_file_length = 0;
        }
        if (bilingual_audio.length <= 0) {
            window.alert('請先上傳檔案');
            check_file_length = 0;
        }

        let form_data = new FormData()
        if (check_file_length == 1) {
            //先建立formData
             form_data.append('slow_audio', slow_audio[0]);
            form_data.append('english_audio', english_audio[0]);
            form_data.append('bilingual_audio', bilingual_audio[0]);
        }

        $.ajax({
            url: url,
            method: "post",
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
            },
        })
    }

    //圖片上傳
    if (file_type === '圖檔') {
        alert(content_id);
        url = `/File/StoreImageFile?content_id=${content_id}&content_name=${content_name}`;

        let upload_file = $('#edit_upload_image')[0].files;
        //未選取檔案
        if (upload_file.length <= 0) {
            window.alert('請先上傳檔案');
            check_file_length = 0;
        }

        let form_data = new FormData()
        if (check_file_length == 1) {
            //先建立formData
            form_data.append('upload_file', upload_file[0]);

            $.ajax({
                url: url,
                method: "post",
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
                },
            })
        }
    }
}

//取得編輯資料
function get_edit_file_data(content_id) {
    let url = "/File/GetEditFile";

    $.ajax({
        url: url,
        method: 'post',
        data: {
            content_id: content_id,
        }, success: function (res) {
            $("#edit_content_id").text(res['content_id']);
            //檔案名稱
            $("#edit_content_name").val(res['content_name']);


            //檔案類型
            let file_type = res['content_type'];
            if (file_type === 'audio') {
                //音檔
                $(".type_image").addClass('d-none');
                $(".type_audio").removeClass('d-none');
                $("#edit_file_type").val('音檔');
            } else if (file_type === 'image') {
                console.log(res);
                //圖檔
                $(".type_image").removeClass('d-none');
                $(".type_audio").addClass('d-none');
                $("#edit_file_type").val('圖檔');

                $("#preview_img").attr("src", res['img_path']);
            } else {
                $(".type_image").addClass('d-none');
                $(".type_audio").addClass('d-none');
                $("#edit_file_type").val('');
            }


        }
    })
}