$(document).ready(function () {
    $("#btn-create-page").on('click', function () {
        let url = "/Book/CreatePage";
        let book_id = $("#book_id").text().trim();
        $.ajax({
            url: url,
            method: "post",
            data: {
                book_id: book_id
            },
            success: function (res) {
                console.log(res);
            },
            error: function (res) {
                console.log(res);
            }
        })
    })
})
