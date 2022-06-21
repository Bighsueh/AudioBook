$(document).ready(function () {
    $('#tab_sound').removeClass('d-none');
    $('#tab_photo').addClass('d-none');
    $('#tab_video').addClass('d-none');

    $('#btn_sound').on('click', function () {
        $('#tab_sound').removeClass('d-none');
        $('#tab_photo').addClass('d-none');
        $('#tab_video').addClass('d-none');
    });

    $('#btn_photo').on('click', function () {
        $('#tab_sound').addClass('d-none');
        $('#tab_photo').removeClass('d-none');
        $('#tab_video').addClass('d-none');
    });

    $('#btn_video').on('click', function () {
        $('#tab_sound').addClass('d-none');
        $('#tab_photo').addClass('d-none');
        $('#tab_video').removeClass('d-none');
    });

})



