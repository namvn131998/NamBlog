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
})