



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

    $.fn.counterSite = function() {

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

currency([0, 10, 5, 2, 20, 30, 45]);

function currency(data) {
    "use strict";

    var chartId = document.getElementById('myChart');

    if (chartId) {

        var ctx = chartId.getContext('2d');
        var chart = new Chart(ctx, {
            // The type of chart we want to create
            type: 'line',

            // The data for our dataset
            data: {
                labels: ["January", "February", "March", "April", "May", "June", "July"],
                datasets: [{
                    label: "My First dataset",
                    backgroundColor: 'rgba(255, 174, 17, 0.7)',
                    borderColor: 'rgba(255, 174, 17, 0.9)',
                    data: data,
                }]
            },

            // Configuration options go here
            options: {}
        });
    }

}

/********************************************
    EMAIL CONTROL
********************************************/

$(function () {
    "use strict";
    $('#contact-form').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        var name = $("#name").val();
        var company = $("#company").val();
        var phone = $("#phone").val();
        var email = $("#email").val();
        var text = $("#message").val();
        var dataString = 'name=' + name + '&email=' + email + '&company=' + company + '&phone=' + phone + '&message=' + text;
        function isValidEmail(emailAddress) {
            var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
            return pattern.test(emailAddress);
        };
        if (isValidEmail(email) && (text.length > 20) && (name.length > 1)) {
            $.ajax({
                type: 'POST',
                url: "contact_form/contact_process.php",
                data: dataString,
                success: function () {
                    $('.success').fadeIn(1000);
                    $("#name").val("");
                    $("#email").val("");
                    $("#company").val("");
                    $("#phone").val("");
                    $("#message").val("");
                }
            });
        } else {
            $('.error').fadeIn(1000);
        }
    });
});