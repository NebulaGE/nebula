swfobject.embedSWF(U + 'content/loader.swf', "flash", "188", "120", "10.0.0", false, {
}, {
    play: "true",
    loop: "false",
    menu: "true",
    quality: "best",
    wmode: "transparent",
    allowscriptaccess: "sameDomain"
});


FB.init({
    appId: '905547436172604',
    xfbml: true,
    version: 'v2.3'
});



var callInit = false;
APP.init = function () {
    if (callInit)
        return;

    callInit = true;

    console.log('init');

    $('#app').addClass('new-bg');

    $('#home-btn, #social-share, #nav-btn, #ls-btn').fadeIn();
    $('#clouds').show();

    $('#loader-wrapper').fadeOut(function () {
        $('#clouds > li').fadeIn(1000, function () {
            var clouds = document.getElementById('clouds');
            var parallax = new Parallax(clouds);
        })
        var mrgnLeft = 0;
        $('#app').fadeIn(function () {
            var imageWidth = 1920;
            var imageHeight = 1080;
            var hotspots = [{
                x: 140,
                y: 150,
                height: 120,
                width: 90,
                src: U + 'content/img/90x120.png'
            },
            {
                x: 165,
                y: 425,
                height: 115,
                width: 85,
                src: U + 'content/img/85x115a.png'
            },
             {
                 x: 5,
                 y: 460,
                 height: 100,
                 width: 65,
                 src: U + 'content/img/65x100.png'
             },
              {
                  x: 70,
                  y: 610,
                  height: 90,
                  width: 70,
                  src: U + 'content/img/70x90.png'
              },
             {
                 x: 2,
                 y: 118,
                 height: 75,
                 width: 50,
                 src: U + 'content/img/50x75.png'
             },
            {
                x: 670,
                y: 448,
                height: 150,
                width: 110,
                src: U + 'content/img/110x150.png'
            },
            {
                x: 850,
                y: 130,
                height: 75,
                width: 50,
                src: U + 'content/img/50x75.png'
            },
            {
                x: 1155,
                y: 180,
                height: 90,
                width: 55,
                src: U + 'content/img/55x90.png'
            },
            {
                   x: 960,
                   y: 390,
                   height: 90,
                   width: 70,
                   src: U + 'content/img/70x90.png'
            },
            {
                    x: 825,
                    y: 305,
                    height: 100,
                    width: 60,
                    src: U + 'content/img/60x100.png'
            },
            {
                x: 845,
                y: 500,
                height: 140,
                width: 90,
                src: U + 'content/img/90x140.png'
            },
            {
                x: 1060,
                y: 801,
                height: 160,
                width: 105,
                src: U + 'content/img/105x160.png'
            },
            {
                x: 1700,
                y: 662,
                height: 120,
                width: 90,
                src: U + 'content/img/90x120.png'
            },

            {
                x: 1785,
                y: 895,
                height: 100,
                width: 75,
                src: U + 'content/img/75x100.png'
            },
            {
                x: 1850,
                y: 200,
                height: 75,
                width: 50,
                src: U + 'content/img/50x75.png'
            },
            {
                x: 1580,
                y: 153,
                height: 90,
                width: 55,
                src: U + 'content/img/55x90.png'
            },
            {
                x: 968,
                y: 462,
                height: 28,
                width: 103,
                src: U + 'content/img/buchq2.png',
                fixed: true
              
            },
               {
                   x: 830,
                   y: 620,
                   height: 37,
                   width: 139,
                   src: U + 'content/img/buchq1.png',
                   fixed: true

               },
            {
                x: 205,
                y: 135,
                height: 735,
                width: 609,
                src: U + 'content/img/car-wheel.png',
                fixed: true,
                car: true,
                mrgn : true
            },
            {
                x: 310,
                y: 765.5,
                height: 90,
                width: 91,
                src: U + 'content/img/wheel.png',
                fixed: true,
                car: true,
                wheels : true
            },
                  {
                      x: 657,
                      y: 765.5,
                      height: 90,
                      width: 91,
                      src: U + 'content/img/wheel.png',
                      fixed: true,
                      car: true,
                      wheels: true
                  }];
            var aspectRatio = imageWidth / imageHeight;

            $(window).resize(function () {
                positionHotSpots(true);
            });
            var positionHotSpots = function (notAppend) {
                $('.hotspot').remove();
                var wi = 0,
                  hi = 0;
                var r = $('#app').width() / $('#app').height();
                if (aspectRatio <= r) {
                    wi = $('#app').width();
                    hi = $('#app').width() / aspectRatio;
                } else {
                    wi = $('#app').height() * aspectRatio;
                    hi = $('#app').height();
                }
                var offsetTop = (hi - $('#app').height()) / 2;
                var offsetLeft = (wi - $('#app').width()) / 2;
           
                $.each(hotspots, function (i, h) {

                    var x = (wi * h.x) / imageWidth;
                    var y = (hi * h.y) / imageHeight;

                    var ww = (wi * (h.width)) / imageWidth;
                    var hh = (hi * (h.height)) / imageHeight;
                    var randNumb = (Math.floor(Math.random() * 1500) + 1);
                    if (h.car) {
                        if (h.mrgn) {
                            mrgnLeft = x - offsetLeft + ww;
                        }
                        var hotspot = $('<img src="' + h.src + '">').addClass('hotspot');
                        hotspot.css({
                            top: y - offsetTop,
                            marginLeft: -1 * mrgnLeft,
                            left: x - offsetLeft,
                            height: hh,
                            width: ww,
                            zIndex: 2,
                            display: 'none'

                        });
                        setTimeout(function () {

                      



                          
                            //setTimeout(function () {
                            //    $('.wheels-animation').removeClass('wheels-animation').addClass('wheels-animation-slow');
                            //}, 1500);
                            //setTimeout(function () {
                            //    $('.wheels-animation-slow').removeClass('wheels-animation-slow').addClass('wheels-animation-slower');
                            //}, 2500);
                            setTimeout(function () {
                                $('.wheels-animation').removeClass('wheels-animation');
                            }, 2800);
                            setTimeout(function () {
                               // hotspot.css('margin-left', 0);
                                if (h.wheels) {
                                    hotspot.addClass('wheels-animation');
                                }
                            }, 100)


                            $('#car-wheel').append(hotspot).show();
                       
                            hotspot.show().animate({ marginLeft: 0 }, 3000);
                         
                           
                        }, 1500);
                    }
                   else if (h.fixed) {
                        var hotspot = $('<img src="' + h.src + '">').addClass('hotspot').css({
                            top: y - offsetTop,
                            left: x - offsetLeft,
                            height: hh,
                            width: ww,
                            zIndex : 2
                        });
                      
                        $('#trees').append(hotspot);

                    } else {
                        setTimeout(function () {
                            var hotspot = $('<img src="' + h.src + '">').addClass('hotspot animated zoomIn').css({
                                top: y - offsetTop,
                                left: x - offsetLeft,
                                height: hh,
                                width: ww
                            });
                            
                            $('#trees').append(hotspot);
                        }, randNumb);
                    }
               

                });
            };
            positionHotSpots();
        });

    });

    setInterval(function () {
        $('#go').fadeTo(800, .5, function () {
            $('#go').fadeTo(800, 1);
        });
    }, 2000);

    if (location.hash == '#go') {
        isFirstScroll = false;
        APP.events.onSendFormDone({ status: 1, co2: 160 }, true);
        return;
    }

    if (location.hash == '#go4') {
        isFirstScroll = false;
        APP.events.onSendFormDone({ status: 4, co2: 160 }, true);
        return;
    }

    $('#nav-btn').on('click', APP.events.onNavClick);
    $('#nav-close').on('click', APP.events.onNavCloseClick);
    $('#page-nav').find('li').on('click', APP.events.onPageNavClick);
    $('#send-form-btn').on('click', APP.events.onSendFormClick);
    $('#con-form > button').on('click', function () {
        $.post(U + 'ajax/contact', $('#con-form').serialize(), function (data) {
            if (data == 1)
                $('#con-form').empty().html('<h2>გმადლობთ, თქვენი წერილი მიღებულია.</h2>');
            else
                $('#con-form').empty().html('<h2>დაფიქსირდა შეცდომა, სცადეთ მოგვიანებით</h2>');
        });
    });

    $('#abc-share').on('click', function () {
        FB.ui({
            method: 'feed',
            name: 'გაიგე რამდენად აბინძურებს შენი მანქანა ჰაერს!',
            link: 'http://ecoist.ge/',
            picture: 'http://ecoist.ge/content/img/ekoist-share.png',
            caption: ' ',
            description: 'მე ვარ ეკოისტი. ვაზღვევ ავტომობილს “ჯიპიაი ჰოლდინგში“ და ვრგავ ჩემს ხეს.'
        });
    });

    $('.autocomplete').each(function () {
        var $this = $(this);
        $this.autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    url: U + 'ajax/search/' + $this.attr('name'),
                    data: $('#form-1').serialize(),
                    dataType: "json",
                    success: response
                });
            },
            select: function (e, data) {
                $(this).val(data.item.value);
                $.ajax({
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    url: U + 'ajax/search/model',
                    data: $('#form-1').serialize(),
                    dataType: "json",
                    success: function (data) {

                        var html = '<option value="">მოდელი</option>';
                        $.each(data, function () {
                            html += '<option value="' + this.value + '">' + this.value + '</option>';
                        });
                        $('[name="model"]').html(html).off('change').on('change', function () {
                            /*$.ajax({
                                type: "GET",
                                contentType: "application/json; charset=utf-8",
                                url: U + 'ajax/search/engine',
                                data: $('#form-1').serialize(),
                                dataType: "json",
                                success: function (data) {
                                    var html = '<option value="">ძრავის მოცულობა</option>';
                                    $.each(data, function () {
                                        html += '<option value="' + this.value + '">' + this.value + '</option>';
                                    });
                                    $('[name="engine"]').html(html).off('change').on('change', function () {
                                        $.ajax({
                                            type: "GET",
                                            contentType: "application/json; charset=utf-8",
                                            url: U + 'ajax/search/year',
                                            data: $('#form-1').serialize(),
                                            dataType: "json",
                                            success: function (data) {
                                                var html = '<option value="">წელი</option>';
                                                $.each(data, function () {
                                                    html += '<option value="' + this.value + '">' + this.value + '</option>';
                                                });
                                                $('[name="year"]').html(html);
                                            }
                                        });
                                    });
                                }
                            });*/
                        });
                    }
                });
                return false;
            }
        }).on("focus", function () {
            $(this).autocomplete("search", "");
        });
    });



    var $navLi = $('.nav-li').on('click', function () {
        //alert();
        $navLi.removeClass('active');
        $(this).addClass('active');
        var id = parseInt($(this).data('id'));
        console.log(id);
        if (id == '44') {
            APP.events.onSendFormDone({ status: 1, co2: 160 }, true);
            $('#nav-close').click();
            return;
        }

        if (id == '2') {
            $('#abc-share').show();
        } else {
            $('#abc-share').hide();
        }
        $('#li-0, #li-1, #li-2, #li-3, #li-4, #li-5').hide();
        $('#li-' + id).fadeIn();
    });
    setTimeout(function () {
        APP.events.onPageChange(2);
    }, 7000);
   
};


APP.init_result = function () {
    $W.off()
    .on('mousewheel', APP.events.onMousewheelResult)
    .on('resize', APP.events.onResizeResult)
    .trigger('resize');

    setInterval(function () {
        $('.cim-cim').fadeTo(400, .5, function () {
            $(this).fadeTo(400, 1);
        })
    }, 2000);

    setTimeout(function () {
        $('#ul-wrapper').fadeIn(500, function () {
            $('#fade-step-2').fadeIn(500);
        });
    }, 2000);

    FB.XFBML.parse(document.getElementById('n-like'));

    $('#n-share').on('click', function (e) {
        e.preventDefault();
        FB.ui({
            method: 'feed',
            name: 'გაიგე რამდენად აბინძურებს შენი მანქანა ჰაერს!',
            link: 'http://ecoist.ge/',
            picture: 'http://ecoist.ge/content/img/ekoist-share.png',
            caption: ' ',
            description: 'მე ვარ ეკოისტი. ვაზღვევ ავტომობილს “ჯიპიაი ჰოლდინგში“ და ვრგავ ჩემს ხეს.'
        });
    });

    var scrollTop;
    var scrollIndex = IsBotPage() ? 0 : 1;
    console.log('scrollIndex', scrollIndex);
    var scroll = [
        [200, 0, 1000, 1800, 2100, 3500, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000],
       // [800, 300, 1700, 2500, 2800, 4100, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000],
          [800, 300, 950, 1500, 2000, 3200, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000],
        [true, true, true, true, true, true, true, true, true, true, true, true, true, true]
    ];

    app.on('scroll', function (e) {

        var $t = $('#pre-result');
        if ($t.length > 0) {
            $t.slideUp(function () {
                $t.remove();
                $('#offer-bg').fadeIn();
                //$('body').css({
                //    background: 'url(' + U + 'content/img/offer-bg.png)',
                //    backgroundPosition: 'center',
                //    backgroundSize: '100% 100%'
                //});
            });
            app.scrollTop(0);
            return;
        }


        scrollTop = app.scrollTop();
        console.log(scrollTop);

        if (scrollTop >= scroll[scrollIndex][0] && scroll[2][0]) {
            GO_CAR();
            scroll[2][0] = false;
        }

        if (scrollTop >= scroll[scrollIndex][1] && scroll[2][1]) {
            scroll[2][1] = false;
            $('#text-0').css({
                top: '+=1000px',
                opacity: 1
            });
        }

        if (scrollTop >= scroll[scrollIndex][2] && scroll[2][2]) {
            scroll[2][2] = false;
            $('#text-1, #text-2').css({
                right: '+=1000px',
                opacity: 1
            });
        }

        if (scrollTop >= scroll[scrollIndex][3] && scroll[2][3]) {
            scroll[2][3] = false;
            $('#text-3').css({
                left: '+=1000px',
                top: '+=1000px',
                opacity: 1
            });
        }

        if (scrollTop >= scroll[scrollIndex][4] && scroll[2][4]) {
            scroll[2][4] = false;
            $('#text-4, #text-5').css({
                left: '+=1000px',
                top: '+=1000px',
                opacity: 1
            });
        }

        if (scrollTop >= scroll[scrollIndex][5] && scroll[2][5]) {
            scroll[2][5] = false;
            $('#text-6').css({
                left: '+=1000px',
                top: '+=1000px',
                opacity: 1
            });
            $('#text-7, #text-8').css({
                right: '+=1000px',
                top: '+=1000px',
                opacity: 1
            });
        }
    });
};


var $F = $('#first-page');

$W
    .on('load', APP.events.onLoad)
    .on('mousewheel', APP.events.onMousewheel)
    .on('resize', APP.events.onResize)
    .trigger('resize');