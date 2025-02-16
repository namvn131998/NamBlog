$(document).ready(function () {
    $(".c-name").html($("#hdcateName").val());
    $(".p-name").html($("#hdproName").val());


    // my slick slider options
    const options = {
        slidesToShow: 1,
        slidesToScroll: 1,
        dots: true,
        arrows: false,
        adaptiveHeight: true,
        autoplay: true,
        autoplaySpeed: 30000000
    };

    // my slick slider as constant object
    const mySlider = $('.p-slider-details').on('init', function (slick) {

        // constant var
        const slider = this;

        // slight delay so init completes render
        setTimeout(function () {

            // dot buttons
            let dots = $('.slick-dots > LI > BUTTON', slider);

            // each dot button function
            $.each(dots, function (i, e) {

                // slide id
                let slide_id = $(this).attr('aria-controls');

                // custom dot image
                let dot_img = $('#' + slide_id + ' .p-slider-details-item').data('dot-img');
                // assign dot image for slick dot default
                $(this).html('<img src="' + dot_img + '" alt="" />');

            });

        }, 100);


    }).slick(options);

    // custom button quantity
    let number = 1;

    document.getElementById("button-plus").addEventListener("click", () => {
        number++;
        document.getElementById("quantityProduct").value = number;
    });

    document.getElementById("button-minus").addEventListener("click", () => {
        number--;
        if (number >= 0)
            document.getElementById("quantityProduct").value = number;
        else {
            number = 0;
            document.getElementById("quantityProduct").value = number;
        }
    });
    //end

    // custom weight
    elements = document.querySelectorAll('.btn-weight');
    elements.forEach((element) => {
        element.addEventListener("click", function () {
            elements.forEach((element) => {
                element.classList.remove("active");
            });
            this.classList.add("active");
        });
    });
    //end
})
var urls = {
    urlQuickView: SiteConfig.gSiteAdrs + 'Customer/Cart/AddToCart',
}
//function AddToCart() {
//    // Url for the request 
//    let productID = document.getElementById("hdId").value;
//    let photo = document.getElementById("hdPhoto").value;
//    let weight = document.getElementsByClassName('btn-weight active')[0].textContent;
//    let quantity = document.getElementById("quantityProduct").value;
//    let url = urls.urlQuickView + `?Id=${productID}&Photo=${photo}&Weight=${weight}&Quantity=${quantity}`;

//    const docs = fetch(url, {
//        method: 'GET',
//        headers: {
//            Accept: 'application/json',
//            'Content-Type': 'application/json',
//        },
//        success: function (data) {
//            if (data.message == "OK") {
//                updateCartQuantity();
//            }
//        }
//    })
//}
function AddToCart() {
    let productID = document.getElementById("hdId").value;
    let photo = document.getElementById("hdPhoto").value;
    let weightElement = document.getElementsByClassName('btn-weight active')[0];
    let quantity = document.getElementById("quantityProduct").value;

    if (!weightElement) {
        console.error("Không tìm thấy trọng lượng sản phẩm.");
        return;
    }

    let weight = weightElement.textContent.trim();

    let url = `${urls.urlQuickView}?Id=${encodeURIComponent(productID)}&Photo=${encodeURIComponent(photo)}&Weight=${encodeURIComponent(weight)}&Quantity=${encodeURIComponent(quantity)}`;

    fetch(url, {
        method: 'GET',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.message === "OK") {
                updateCartQuantity();
            } else {
                console.error("Lỗi từ server:", data);
            }
        })
        .catch(error => {
            console.error("Lỗi khi gửi request:", error);
        });
}
