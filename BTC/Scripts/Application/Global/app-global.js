toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}


function successAlert(message) {
    toastr["success"](message)
}
function dangerAlert(message) {
    toastr["warning"](message)
}

function dangerAlertList(messages) {
    $.each(messages, function (key, value) {
        toastr["danger"](value);
    });
}

function alertResponse(rm,r) {

    if (rm.IsSuccess == true) {
        successAlert(rm.Message);
    } else {
        dangerAlert(rm.Message);
    }
    if (r == true) {
        location.reload();
    }

}

function selectPage() {
    var s_p = $('#select-page').val();
    $('.' + s_p).addClass('active');
}

$(document).ready(function () {
    $('.phoneMask').mask('(000) 000-0000');
    selectPage();
});

//$(window).on('load', function () {
//    $("#loading").delay(100).fadeOut(100);
//})






//PRELOAD PAGES

$(window).on('load', function () {
    "use strict";

    $("#loading").delay(1000).fadeOut(800);
})

$(document).ready(function () {
    "use strict";

    /*************************
         SCROLL BAR
    *************************/

    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });


    /*************************
         NAVBAR MOBILE
    *************************/

    $('#sidebarCollapse').on('click', function () {
        // open or close navbar
        $('#sidebar').toggleClass('active');
    });

    $('.fa-times').on('click', function () {
        $('#sidebar').toggleClass('active');
    });


    /*************************
         COUNTER CRYPTO
    *************************/

    $.fn.counterSite = function () {

        var $items = $('.animate-number');

        if ($items.length) {
            $('.count').each(function () {
                $(this).prop('Counter', 0).animate({
                    Counter: $(this).text()
                }, {
                        duration: 4000,
                        easing: 'swing',
                        step: function (now) {
                            $(this).text(Math.ceil(now));
                        }
                    });
            });
        }
    }

    /**********************\
    //portfolio isotope init
    /************************/

    $('.portfolio-filter').on('click', 'li', function () {
        var filterValue = $(this).attr('data-filter');
        $('.portfolio-filter > li.active').removeClass('active');
        $(this).addClass('active')
        $('.portfolio').isotope({
            filter: filterValue
        });
    });

    var $portfolio = $('.portfolio');

    if ($portfolio.length) {
        var $portfolio = $('.portfolio');
        $portfolio.imagesLoaded(function () {
            $portfolio.isotope({
                itemSelector: '.portfolio-item',
                layoutMode: 'fitRows'
            });
        });
    }

    /*************************
         Back to top
    *************************/

    var $goToTop = $('#back-to-top');
    $goToTop.hide();
    $(window).scroll(function () {
        if ($(window).scrollTop() > 100) $goToTop.fadeIn();
        else $goToTop.fadeOut();
    });
    $goToTop.on("click", function () {
        $('body,html').animate({ scrollTop: 0 }, 1000);
        return false;
    });


    /*************************
         SlideShow
    *************************/

    if ($('.slideshow').length) {

        $('.owl-carousel.slideshow').owlCarousel({
            items: 1,
            smartSpeed: 700,
            loop: true,
            nav: true,
            navText: ["", ""],
            rewindNav: true,
            autoplay: false
        });
    }

    /*************************
         Carousel Services
    *************************/

    if ($('.multi-carousel').length) {

        $('.owl-carousel.multi-carousel').owlCarousel({
            loop: true,
            margin: 10,
            responsiveClass: true,
            navText: ["", ""],
            rewindNav: true,
            nav: false,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 4,
                }
            }
        });
    }

    /*************************
         Carousel Clients
    *************************/

    if ($('.our-clients').length) {

        $('.owl-carousel.our-clients').owlCarousel({
            loop: true,
            margin: 10,
            responsiveClass: true,
            navText: ["", ""],
            rewindNav: true,
            nav: false,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 2,
                }
            }
        })
    }

});




