$(document).ready(function () {
    $("#sidebar").click(function () {
        if ($(".sidebar-section").hasClass("width-20")) {
            $(".sidebar-section").removeClass("width-20");
            $(".sidebar-section").addClass("width-9");
            $("#sidebar").removeClass("fa-angle-left");
            $("#sidebar").addClass("fa-angle-right");
            $(".page-content").addClass("mgl-10");
        } else {
            $(".sidebar-section").removeClass("width-9");
            $(".sidebar-section").addClass("width-20");
            $("#sidebar").removeClass("fa-angle-right");
            $("#sidebar").addClass("fa-angle-left");
            $(".page-content").removeClass("mgl-10");
        }
    })

})
function Changepassword() {
    var url = $("#btn-changepassword").attr("data-ajax-url");
    var data = $("#changepassword").serialize();
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (data) {
            if (data.result == "OK")
                alert("OK");
        }
    })
}
function PopupUploadFile() {
    var url = $("#btn-uploadAvatar").attr("data-ajax-url");
    $.ajax({
        url: url,
        type: "GET",
        data: {

        },
        success: function (data) {
            $("#modalUploadFile").html(data);
            $("#modalUploadFile").dialog({
                modal: true,
                width: 500,
                lgClass: true
            });
            $("#modalUploadFile").dialog("open");
        }
    })
}
function UploadUserAvatar() {
    var data = new FormData($('#addMediaFileProduct')[0]);
    data.append("MediaTypeID", 1);
    var url = $("#btn-submitFile").attr("data-ajax-url");
    if ($("#addMediaFileProduct").valid()) {
        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            contentType: false,
            processData: false,
            success: function (data) {
                $("#modalUploadFile").dialog("close");
                $("#Photo").val(data.urlThumbnail);
                $(".img-thumbnail").attr("src",data.urlThumbnail);
            }
        });
    }
}