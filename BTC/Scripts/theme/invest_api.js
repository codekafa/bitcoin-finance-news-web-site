// Invest pages

function investGraph() {
    "use strict";

    var url = 'https://api.coinmarketcap.com/v1/ticker/?limit=15';

    postData();

    function postData() {
        fetch(url)
            .then(res => res.json())

            .then(function (response) {
                boxInvest(response);
                chartInvest(response.slice(1, 15));
            })
            .catch(error => console.error('Error:', error));
    }

    function boxInvest(data) {

        var tablecontents = "";

        const app = document.getElementById('investBox');

        const container = document.createElement('div');
        container.setAttribute('id', 'investmentData');
        container.setAttribute('class', 'row');

        app.appendChild(container);

        data.forEach(resp => {
            tablecontents += "<div class='col-md-4 space-btn'>";
            tablecontents += "<div class='box-content'>";

            tablecontents += "<p class='name'>" + "<span>" + resp.name + "</span>" + ' - ' + "<span>" + resp.symbol + "</span>" + "</p>";


            if (resp.percent_change_24h < 0) {
                tablecontents += "<p class='price'>" + resp.price_usd + ' $' + "<span class='percent-change red'>" + resp.percent_change_24h + '%' + "</span>" + "</p>";
            }
            else {
                tablecontents += "<p class='price'>" + resp.price_usd + ' $' + "<span class='percent-change green'>" + resp.percent_change_24h + '%' + "</span>" + "</p>";
            }

            tablecontents += "</div>";
            tablecontents += "</div>";
        });

        container.innerHTML = tablecontents;
    }

    function chartInvest(data) {

        var chartId = document.getElementById('chartLine');
        var chartArrayPrice = [];
        var chartArrayName = [];

        data.forEach(resp => {
            chartArrayPrice.push(Number(resp.price_usd).toFixed(2));
            chartArrayName.push(resp.name);
        });

        if (chartId) {

            var ctx = chartId.getContext('2d');
            var chart = new Chart(ctx, {
                // The type of chart we want to create
                type: 'line',

                // The data for our dataset
                data: {
                    labels: chartArrayName,
                    datasets: [{
                        label: "Price line",
                        backgroundColor: ["rgba(255, 174, 17, 0)"],
                        borderColor: ["rgba(255, 174, 17, 1)"],
                        borderWidth: '3',
                        data: chartArrayPrice,
                    }]
                }
            });
        }


    }

}

setInterval(function () {

    var investData = document.getElementById("investBox");

    const dataChild = document.getElementById('investmentData');

    investData.removeChild(dataChild);

    investGraph();

}, 300000);

clearInterval();
