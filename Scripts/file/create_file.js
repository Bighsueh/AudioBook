$("#btn_submit_create").on('click', function () {
    let file_title = $("#create_file_title").val();     //檔案名稱

    let file_type = $("#select_file_type").val();
    let check_file_length = 1;
    let url;
    let form_data;

    //音檔上傳
    if (file_type === 'audio') {
        url = `/File/CreateAudioFile?file_title=${file_title}`;

        //get input:file
        let slow_audio = $("#create_slow_audio")[0].files;
        let english_audio = $("#create_english_audio")[0].files;
        let bilingual_audio = $("#create_bilingual_audio")[0].files;


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
            },
            error: function (res) {
                console.log(res);
            },
        })
    }

    //圖片上傳
    if (file_type === 'image') {
        url = `/File/CreateImageFile?file_title=${file_title}`;

        let upload_file = $('#create_upload_image')[0].files;

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
                },
                error: function (res) {
                    console.log(res);
                },
            })
        }
    }


})

//檔案類型:select
$("#select_file_type").on('change', function () {
    let value = $(this).val();
    if (value === 'audio') {
        $(".type_image").addClass('d-none');
        $(".type_audio").removeClass('d-none');
    }
    if (value === 'image') {
        $(".type_image").removeClass('d-none');
        $(".type_audio").addClass('d-none');
    } 
})