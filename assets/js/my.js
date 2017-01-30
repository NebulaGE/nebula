function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown if the user clicks outside of it
window.onclick = function(event) {
  if (!event.target.matches('.dropbtn')) {

    var dropdowns = document.getElementsByClassName("dropdown-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (openDropdown.classList.contains('show')) {
        openDropdown.classList.remove('show');
      }
    }
  }
}
var chart;
$(function () {

    var  categories =  [
        '01',
        '02',
        '03',
        '04',
        '05',
        '06',
        '07',
        '08',
        '09',
        '10',
        '11',
        '12'
    ];

    var categoryNames = [
        'იანვარი',
        'თებერვალი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
        'იანვარი',
    ];


     chart = Highcharts.chart('chart-div', {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: ''
        },
        yAxis: {
            title: {
                text: ''
            },
            gridLineColor: '#dee8ee',
            stackLabels: {
                enabled: false,
                style: {
                    fontWeight: 'normal',
                    color: "#808080"
                }
            },
            max: 20,
            plotLines: [{
                    value: 0,
                    color: '#d4d4d4',
                    dashStyle: 'dash',
                    width: 2,
            }],
            labels: {
              style: {
                color: '#808080 ',
                font: "16px 'bpg_excelsior_caps_dejavu_2010';"
              },
              x: -13
            },                    
            gridLineColor: '#d4d4d4',
            gridLineDashStyle: 'longdash',
        },
        xAxis: {
          categories: categories,
          lineColor: '#ffffff',
          labels: {
               style: {
                   color: '#000000 ',
                  // fontSize: '16px',
                  // fontFamily: 'TBcdinMtavruliBold'
               }
          },
          startOnTick: false,
          endOnTick: false,  
          tickWidth: 0,
          title: {
              enabled: false,
              text: Highcharts.getOptions().lang.category,
          },        
          tickColor: '#dee8ee',
          labels: {
              useHTML: true,            
              formatter: function() {
                  return '<div class="xaxis-point">'+this.value+'</div>';    
              },
              style: {
                color: '#808080 ',
                font: "16px 'bpg_excelsior_caps_dejavu_2010';"
              }
          },
        },
        tooltip: {
          shared: true,
          useHTML: true,
          formatter: function() {
            var x = parseInt(this.x);
            var string = this.x + " " + categoryNames[x];
             return "<span class='datalabel'>" + string + "</span>" ;
          }
        },
        credits: {
            enabled: false
        },
        plotOptions: {           
               areaspline: {
                   color: 'rgba(139,128,255,1)',
                   fillOpacity: .5,
                   fillColor: {
                       linearGradient: {
                           x1: 0,
                           y1: 0,
                           x2: 0,
                           y2: 1
                       },
                       stops: [
                            [0, 'rgba(139,128,255,0.5)'],
                            [1, 'rgba(139,128,255,0)'],

                       ]
                   },
               },
           series: {
              dataLabels: {
                  enabled: false
              },
              marker: {
                  symbol: 'url(assets/img/chart-point.svg)',
                  width: 21,
                  height: 21,                  
                  states: {
                      hover: {
                          enabled: false,
                      }
                  }
              }
           }
       },

        legend: {
          enabled: false
        },
        series: [{
            data: [3, 4, 3, 5, 4, 10, 12, 4, 3, 5, 4, 10]
        }]
    });
});