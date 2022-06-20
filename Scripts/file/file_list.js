$(document).ready(function () {
    //編輯書籍按鈕
    $("#btn-edit-file").on('click', function () {
        $('.delete-columns').toggleClass('d-none');
        $('.detail-columns').toggleClass('d-none');
    })

    //刪除書籍
    $(".btn-delete-file").on('click', function () {
        let url = "/File/DeleteFile";
        let content_id = $(this).siblings('.content_id').text().trim();
        alert(content_id);
        $.ajax({
            url: url,
            method: 'post',
            data: {
                content_id: content_id,
            }, success: function (res) {
                console.log(res);
                window.location.reload();
            }, error: function (res) {
                console.log(res);
            }
        })
    })
})
