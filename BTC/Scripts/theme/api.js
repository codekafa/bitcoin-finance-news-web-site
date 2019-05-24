



function init() {

    var url = 'https://api.coinmarketcap.com/v1/ticker/?limit=8';

    postData();

    //Call API get Data

    function postData() {
        "use strict";

        fetch(url)
            .then(res => res.json())
            .then(function (response) {
                getDataRoot(response);
            })
            .catch(error => console.error('Error:', error));
    }

}

//Graph header crypto value

const app = document.getElementById('root');
var cryptoValue = 0;

function getDataRoot(data) {
    "use strict";

    cryptoValue = data;

    const container = document.createElement('div');
    container.setAttribute('id', 'container');
    container.setAttribute('class', 'container');

    
    const row = document.createElement('div');
    row.setAttribute('class', 'row');

    app.appendChild(container);
    container.appendChild(row);

    const content = document.createElement('div');
    content.setAttribute('class', 'owl-carousel owl-theme top-crypto-carousel');
    row.appendChild(content);


    data.forEach(resp => {

        const card = document.createElement('div');
        card.setAttribute('class', 'item box-crypto');

        // Create an h1 and set the text content to the price title
        const h1 = document.createElement('h1');

        const price = document.createElement('p');
        price.setAttribute('class', 'price');

        const change = document.createElement('span');

        if (resp.percent_change_24h < 0) {
            change.setAttribute('class', 'red');
        }
        else {
            change.setAttribute('class', 'green');
        }

        h1.textContent = resp.name + ' ' + '(' + resp.symbol + ')';
        price.textContent = '$ ' + Number(resp.price_usd).toFixed(2);
        change.textContent =  resp.percent_change_24h + '%';

        content.appendChild(card);
        card.appendChild(h1);
        price.appendChild(change);
        card.appendChild(price);
    });


    /*************************
     Carousel Header Crypto List
    *************************/

    //if ($('.top-crypto-carousel').length) {

  

        $('.owl-carousel.top-crypto-carousel').owlCarousel({
            loop: true,
            margin: 10,
            responsiveClass: true,
            navText: ["", ""],
            rewindNav: false,
            nav: false,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 3,
                },
                1100: {
                    items: 5,
                },
                1300: {
                    items: 6,
                },
                1400: {
                    items: 8,
                }
            }
        });
    //}

}


setInterval(function () {

    var myContainer = document.getElementById("container");

    app.removeChild(myContainer);

    init();

}, 300000);

clearInterval();

init();
