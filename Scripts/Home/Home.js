
// Bỏ cơ chế postback resubmit
document.onkeydown = fkey
function fkey(e) {
    if (e.keyCode == 116) {
        window.history.replaceState(null, null, window.location.href);
    }
}


// mặc định ban đầu ẩn tab group's content
//$('#content-group').hide();


$('#user-tab').click(function () {
    $('#content-group').hide();
    $('#content-user').show();
    $('.home-tab').toArray().forEach(
        e => {
            if (e.id == 'user-tab') {
                e.classList.add('active');
            } else {
                e.classList.remove('active');
            }
        });
});
$('#group-tab').click(function () {
    $('#content-group').show();
    $('#content-user').hide();
    $('.home-tab').toArray().forEach(
        e => {
            if (e.id == 'group-tab') {
                e.classList.add('active');
            } else {
                e.classList.remove('active');
            }
        });
});
$(function () {
    // Đổi in đậm phần navigation

    var nav_options = $('.nav-option').toArray();
    console.log("nav_opts: ", nav_options);
    nav_options.forEach(
        e => {
            console.log("e.datapage: ", e.dataset.page);
            if (e.dataset.page == 'Home') {
                e.classList = "nav-option active";

            } else {
                e.classList = "nav-option";
            }

        })
});

