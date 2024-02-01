﻿
$(document).ready(function () {
    $('.banner-slider').slick({
        autoplay: true,
        autoplaySpeed: 3000,
        infinite: true,
        dots: true,
        arrows: false
    });
    $('.category-slider').slick({
        dots: false,
        arrows: true,
        slidesToShow: 6,
        slidesToScroll: 1,
        prevArrow: '<span class="slick-prev slick-arrow" style=""><i class="fa-solid fa-chevron-left"></i></span>',
        nextArrow: '<span class="slick-next slick-arrow" style=""><i class="fa-solid fa-chevron-right"></i></span>',
    })
});
var urls = {
    urlQuickView: SiteConfig.gSiteAdrs + 'Customer/Home/_QuickView',
}
function openQuickViewModal(productID) {
    $.ajax({
        url: urls.urlQuickView,
        type: 'GET',
        data: {
            productID: productID
        },
        success: function (data) {
            $("#product-QuickView").html(data);
            $("#product-QuickView").dialog({
                width: 800,
                height: 600,
                modal: true,
                lgClass: true
            });
            $("#product-QuickView").dialog();
            $('#productModal').slick({
                infinite: true
            });
            $(".ui-dialog-titlebar").hide();
        }
    })
}

