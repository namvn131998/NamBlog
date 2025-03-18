
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
        slidesToShow: 5,
        slidesToScroll: 1,
        prevArrow: '<span class="slick-prev slick-arrow" style=""><i class="fa-solid fa-chevron-left"></i></span>',
        nextArrow: '<span class="slick-next slick-arrow" style=""><i class="fa-solid fa-chevron-right"></i></span>',
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 5,
                    slidesToScroll: 3,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 800,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            }
        ]
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
                lgClass: true,
                show: {
                    effect: 'fade',
                    duration: 200 //at your convenience
                },
                hide: {
                    effect: 'fade',
                    duration: 200 //at your convenience
                }
            });
            $('#productModal').slick({
                infinite: true
            });
        }
    })
}
function updateCartQuantity() {
    let cartElements = document.getElementsByClassName('cart-quantity');

    if (!cartElements) {
        console.error("Không tìm thấy phần tử cart-quantity.");
        return;
    }

    let countNumber = parseInt(cartElements[0].innerHTML) || 0; // Nếu giá trị không hợp lệ, mặc định là 0
    for (var i = 0; i < cartElements.length; i++) {
        cartElements[i].innerHTML = countNumber + 1;
    };
}




