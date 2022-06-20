$(document).ready(function () {
    //編輯書籍按鈕
    $("#btn-edit-book").on('click', function () {
        $('.delete-columns').toggleClass('d-none');
        $('.detail-columns').toggleClass('d-none');
    })

    //刪除書籍
    $(".btn-delete-book").on('click', function () {
        let url = "/Book/DeleteBook";
        let book_id = $(this).siblings('.book_id').text();
        console.log(book_id);

        $.ajax({
            url: url,
            method: 'post',
            data: {
                book_id: book_id,
            }, success: function (res) {
                console.log(res);
                window.location.reload();
            }, error: function (res) {
                console.log(res);
            }
        })
    })

})
