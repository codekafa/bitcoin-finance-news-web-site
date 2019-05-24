

function tradeGraph() {
    "use strict";

    var url = 'https://api.coinmarketcap.com/v1/ticker/';

    postData();

    function postData() {
        fetch(url)
            .then(res => res.json())
            
            .then(function (response) {
                chartBar(response.slice(1, 8));
                chartPie(response.slice(1, 8));
                createTable(response);
            })
            .catch(error => console.error('Error:', error));
    }


    function chartBar(data) {

        var chartId = document.getElementById('chartBar');
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
                type: 'bar',

                // The data for our dataset
                data: {
                    labels: chartArrayName,
                    datasets: [{
                        label: "Price Bar",
                        backgroundColor: ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                        borderColor: ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        borderWidth: '1',

                        data: chartArrayPrice,
                    }]
                },
                options: {

                    legend: {
                        display: true,
                        labels: {
                            fontColor: '#fff'
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                fontColor: "#fff", // this here
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                fontColor: "#fff", // this here
                            }
                        }]
                    }
                }
            });
        }

    } //End Bar


    function chartPie(data) {

        var chartId = document.getElementById('chartPie');
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
                type: 'pie',

                // The data for our dataset
                data: {
                    labels: chartArrayName,
                    datasets: [{
                        label: "Price pie",
                        backgroundColor: ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                        borderColor: ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        borderWidth: '1',
                        data: chartArrayPrice,
                    }]
                },
                options: {

                    legend: {
                        display: true,
                        labels: {
                            fontColor: '#fff'
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                fontColor: "#fff", // this here
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                fontColor: "#fff", // this here
                            }
                        }]
                    }
                }
            });
        }

    } //End Pie


    function createTable(data) {
        "use strict";

        var tablecontents = "";

        var data = data.slice(1, 15);

        const app = document.getElementById('priceTable');

        tablecontents = "<table cellpadding='0' cellspacing='0' border='0' id='table'>";

        tablecontents += "<thead class='top-table'>";

        tablecontents += "<tr>";

        tablecontents += "<td><h3>Name</h3></td>";
        tablecontents += "<td><h3>Price</h3></td>";
        tablecontents += "<td><h3>Change 24h</h3></td>";
        tablecontents += "<td class='total'><h3>Available Supply</h3></td>";
        tablecontents += "<td class='total'><h3>Total Supply</h3></td>";

        tablecontents += "</tr>";

        tablecontents += "</thead>";

        tablecontents += "<tbody class='body-table'>";



        data.forEach(resp => {
            tablecontents += "<tr>";
            tablecontents += "<td><p class='name'>" + resp.name + "</p></td>";
            tablecontents += "<td><p class='green-price'>" + Number(resp.price_usd).toFixed(2) + "</p></td>";

            if (resp.percent_change_24h < 0) {
                tablecontents += "<td><p class='red'>" + resp.percent_change_24h + "</p></td>";
            }
            else {
                tablecontents += "<td><p class='green'>" + resp.percent_change_24h + "</p></td>";
            }

            tablecontents += "<td class='total'><p>" + resp.available_supply + "</p></td>";
            tablecontents += "<td class='total'><p>" + resp.total_supply + "</p></td>";

            tablecontents += "</tr>";
        });



        tablecontents += "</tbody>";

        tablecontents += "</table>";

        app.innerHTML = tablecontents;

    }


} //End Trade


setInterval(function () {

    var priceTable = document.getElementById("priceTable");
    var table = document.getElementById("table");

    priceTable.removeChild(table);

    tradeGraph();

}, 300000);

clearInterval();