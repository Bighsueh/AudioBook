function create_user() {
    let url = "/User/CreateGroup";
    $.ajax({
        url: url,
        method: 'post',
        data: {
            group_name: $("#create_group_name").val(),
            group_content: $("#create_group_content").val(),
        },
        success: function (res) {
            console.log(res);
            window.location.reload();
        },
        error: function (res) {
            console.log(res);
        }
    })
}

$(document).ready(function () {
    //管理員成員群組禁止刪除
    $(".delete-columns").eq(0).children().remove();

    $("#btn_submit_create").on('click', function () {
        create_user();
    })

    $("#btn-edit-book").on('click', function () {
        $('.delete-columns').toggleClass('d-none');
        $('.detail-columns').toggleClass('d-none');
    })
    $(".btn-edit-group").on('click', function () {
        let url = "/User/GetGroupInfo";
        let group_id = $(this).siblings('.group_id').text().trim();

        $.ajax({
            url: url,
            method: 'post',
            data: {
                group_id: group_id,
            },
            success: function (res) {
                $("#edit_group_id").text(res['group_id']);
                $("#edit_group_name").val(res['group_name']);
                $("#edit_group_content").val(res['group_content']);
            },
            error: function (res) {
                console.log(res);
            }
        })

    })
    $("#btn_submit_edit").on('click', function () {
        let url = "/User/StoreGroupInfo";
        let group_id = $("#edit_group_id").text().trim();
        let group_name = $("#edit_group_name").val();
        let group_content = $("#edit_group_content").val();

        $.ajax({
            url: url,
            method: 'post',
            data: {
                group_id: group_id,
                group_name: group_name,
                group_content: group_content,
            },
            success: function (res) {
                console.log(res);
                window.location.reload();
            },
            error: function (res) {
                console.log(res);
            }
        })
    })

    $(".btn-delete-group").on('click', function () {
        let url = "/User/DeleteGroup";
        let group_id = $(this).siblings('.group_id').text().trim();
        $.ajax({
            url: url,
            method: 'post',
            data: {
                group_id: group_id,
            },
            success: function (res) {
                console.log(res);
                window.location.reload();
            },
            error: function (res) {
                console.log(res);
            }
        })
    })
})

