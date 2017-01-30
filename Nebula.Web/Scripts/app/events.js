APP.events = {};
function IsBotPage() {
    var res = $('#bot-result').find('> div > div').length > 0;
    console.log('IsBotPage', res);
    return res;
}


function CarShare(type) {
    if (type == 4)
        FB.ui({
            method: 'feed',
            name: 'ნუ იქნები ეგოისტი. გახდი ეკოისტი!',
            link: 'http://ecoist.ge/',
            picture: 'http://ecoist.ge/content/img/car-4-share.png',
            caption: ' ',
            description: 'ავტომობილის დაზღვევით ვრგავ ხეს და ხის დარგვით ვაზღვევ მომავალს.'
        }); else if(type == 3)
        FB.ui({
            method: 'feed',
            name: 'ნუ იქნები ეგოისტი. გახდი ეკოისტი!',
            link: 'http://ecoist.ge/',
            picture: 'http://ecoist.ge/content/img/car-3.png',
            caption: ' ',
            description: 'ავტომობილის დაზღვევით ვრგავ ხეს და ხის დარგვით ვაზღვევ მომავალს.'
        });
}

APP.events.Scrolling = false;
APP.events.wHeight = null;

APP.events.onPageChange = function (pageId) {
    if (APP.events.Scrolling)
        return false;

    APP.events.Scrolling = true;

    $('#page-nav').find('div').hide();
    switch (pageId) {
        case 1:
            $('#pages').animate({ marginTop: 0 }, function () {
                $('#page-nav > li:eq(0)').find('div').fadeIn();
                APP.events.Scrolling = false;
            });
            break;
        case 2:
            $('#pages').animate({ marginTop: -APP.events.wHeight }, function () {
                $('#page-nav > li:eq(1)').find('div').fadeIn();
                APP.events.Scrolling = false;
            });
            break;
        default:
            alert(pageId);
    }
};

APP.events.onPageNavClick = function () {
    console.log('onPageNavClick', $(this).index() + 1);
    APP.events.onPageChange($(this).index() + 1);
};

APP.events.onNavClick = function (e) {
    e.preventDefault();
    $('#nav').animate({ width: '50%' });
};

APP.events.onNavCloseClick = function () {
    $('#nav').animate({ width: '0' })
};

APP.events.onScrollUP = function () {
    //console.log('onScrollUP', arguments);
    APP.events.onPageChange(1);
};

APP.events.onScrollDOWN = function () {
    //console.log('onScrollDOWN', arguments);
    APP.events.onPageChange(2);
};

APP.events.onMousewheel = function (e) {
    //console.log(e.deltaX, e.deltaY, e.deltaFactor

    if (e.deltaY > 0)
        APP.events.onScrollUP();
    else
        APP.events.onScrollDOWN();
};



APP.events.onLoad = function (e) {
    //console.log('onLoad', e);
    //APP.init();
    //return false;
    $loaderCount = $('#loader-count');

    if (location.hash == '#go') {
        top.location.href = U + 'home/registration';
        return;

   

      //  APP.init();
    } else {
        //$('#loader').show();
        //$({ x: 0 }).animate({ x: 916 }, {
        //    duration: 4000,
        //    step: function (now) {
        //        $loaderCount.html(parseInt(now) + ' %');
        //    },
        //    complete: function () {
        //        $('#step-1, #flash-wrapper').fadeOut(function () {
        //            $('.step-2').fadeIn(500, function () {
        //                setTimeout(APP.init, 2000);
        //            });
        //        });
        //    }
        //});
     
        $('#loader-img-color-wrapper').show();
        $('.before-load').show();
        var percent = 0;
        var timer = setInterval(function () {


            if (percent == 71) {
     
                clearInterval(timer);
                setTimeout(function () {
                    $('#after-loader-text').text('შენ რა შეგიძლია გააკეთო?').fadeIn();
                    setTimeout(function () {
                        APP.init();
                    }, 2000)               
                }, 2000);
             
             
                         
           
                return false;
            }
                

            percent++;

            $('#loader-img-color').css('top', '-' + percent + '%');
            $('#percent-number').text(percent);

            //  $('#loader-img-fg-color').css('top','-'+ percent + '%')
        }, 35);
 
        //$({ x: 0 }).animate({ x: 916 }, {
        //    duration: 4/*000*/,
        //    step: function (now) {
        //        $loaderCount.html(parseInt(now) + ' %');
        //    },
        //    complete: function () {
        //        $('#step-1, #flash-wrapper').fadeOut(function () {
        //            $('.step-2').fadeIn(5/*00*/, function () {
        //                setTimeout(APP.init, 2/*000*/);
        //            });
        //        });
        //    }
        //});
    }

    //setTimeout(APP.init, 5000);
};

APP.events.onResize = function (e) {
    //console.log('onResize', e);
    APP.events.wHeight = $W.height();

    $('#man img').height(APP.events.wHeight - 80);

    $F.find('.col, #start-screen, #send-form').css('height', APP.events.wHeight);
};

APP.events.onResizeResult = function (e) {
    console.log('onResize', e);
    APP.events.wHeight = $W.height();
};
var CAR_GO = false;
function GO_CAR() {
    console.log('scrollTop', CAR_GO);

    $CAR = $('#car');
    $CAR_IMG = $CAR.find('#car-img');

    $CAR.animate({
        left: 560
    }, CAR_SPEED, function () {

        CarDrive(950, 1000, 728, 500, 0, 90, 800, function () {
            $CAR.animate({
                top: 1180
            }, CAR_SPEED, function () {
                CarDrive(1327, 500, 587, 1000, 90, 180, 800, function () {
                    $CAR.animate({ left: 306 }, CAR_SPEED, function () {
                        CarDrive(1480, 1000, 150, 500, 180, 90, 800, function () {
                            $CAR.animate({ top: 1980 }, CAR_SPEED + 200, function () {
                                CarDrive(2110, 500, 300, 1000, 90, 0, 800, function () {
                                    $CAR.animate({ left: 725 }, CAR_SPEED, function () {
                                        CarDrive(2251, 500, 830, 500, 0, 90, 500, function () {
                                            CarDrive(2385, 500, 725, 500, 90, 180, 500, function () {
                                                $CAR.animate({ left: 155 }, CAR_SPEED, function () {
                                                    CarDrive(2530, 1000, 15, 500, 180, 90, 500, function () {
                                                        $CAR.animate({ top: 2730 }, CAR_SPEED, function () {
                                                            CarDrive(2850, 500, 155, 1000, 90, 0, 500, function () {
                                                                $CAR.animate({ left: 300 }, CAR_SPEED, function () {
                                                                    CarDrive(2975, 1000, 420, 500, 0, 90, 500, function () {
                                                                        CarDrive(3130, 500, 590, 1000, 90, 0, 500, function () {
                                                                            $CAR.animate({ left: 800 }, CAR_SPEED, function () {
                                                                                CarDrive(3290, 1000, 955, 500, 0, 90, 500, function () {
                                                                                    $CAR.animate({ top: 3555 }, CAR_SPEED, function () {
                                                                                        CarDrive(3705, 500, 830, 1000, 90, 180, 500, function () {
                                                                                            $CAR.animate({ left: 545 }, CAR_SPEED, function () {
                                                                                                CarDrive(3867, 1000, 395, 500, 180, 90, 500, function () {
                                                                                                    $CAR.animate({ top: 4630 }, CAR_SPEED * 2, function () {
                                                                                                        CarDrive(4780, 500, 260, 1000, 90, 180, 500, function () {
                                                                                                            CarDrive(4500, 1000, 120, 500, 180, 270, 500, function () {
                                                                                                                $CAR.animate({ top: 4503 }, CAR_SPEED / 2, function () {
                                                                                                                    CarDrive(4365, 500, 240, 1000, 270, 360, 500, function () {
                                                                                                                        $CAR.animate({ left: 620 }, CAR_SPEED, function () {
                                                                                                                            CarDrive(4500, 1000, 780, 500, 0, 90, 500, function () {

                                                                                                                            });
                                                                                                                        });
                                                                                                                    });
                                                                                                                });
                                                                                                            });
                                                                                                        });
                                                                                                    });
                                                                                                });
                                                                                            });
                                                                                        });
                                                                                    });
                                                                                });
                                                                            });
                                                                        });
                                                                    });
                                                                });
                                                            });
                                                        });
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    });
}

APP.events.onSendFormDone = function (data, hide_pre) {
    console.log('onSendFormDone', data);
    $('#app').removeClass('new-bg');
   
    if (data.status == -1)
    {
        alert('აღნიშნული დასახელება ვერ მოიძებნა');
        return false;
    }
   

    $('#clouds-wrapper').remove();
    $('#home-btn').addClass('back').show().on('click', function (e) {
  
        e.preventDefault();
        $(this).hide();
        $('#car').stop(true, true);
        //   top.location.href = U;
        $('#pg2').hide();
        $('#pg1').show();
        $('#app').addClass('new-bg');
        $('#pg2').empty();
        $('#car').css({
            top: '690px',
            left: '380px'
        });
        var $F = $('#first-page');

        $W
          
            .on('mousewheel', APP.events.onMousewheel)
    });
    $('#pg1').hide();
    window.app = $('#pg2').empty().css('overflow', 'auto').show();
    var title = '';
    var imgType = '';
    var progLoader = '0 0 no-repeat url(' + U + 'content/img/co-loader2.png)';
    if (data.engine == 1) {
        title = 'გილოცავ!  შენი მანქანა ეკომეგობრულია. <br/> საკმარისია, თუკი დარგავ სამ-ოთხ ხეს.';
    } else if (data.engine == 2) {
        title = 'შენი მანქანა ნახევრად ეკომეგობრულია. <br/> შენ ხის დარგვა წესად უნდა დარგო.';
        progLoader = '0 0 no-repeat url(' + U + 'content/img/loader-2.png)';
    } else if (data.engine == 3) {
        title = 'შენი მანქანა სრულიად არაეკომეგობრულია. <br/> შენ უნდა გააშენო ტყე.';
        progLoader = '0 0 no-repeat url(' + U + 'content/img/loader-3.png)';
    }
    if (data.type == 1) {
        imgType = 4;
    } else {
        imgType = 3;
    }


        app.html(TEMP.PAGE_WRAPPER({
            footerClass: '',
            pre_html: TEMP.PRE({
                type: imgType,
                prog: 20,
                title: title,
                desc: 'შენი მანქანა გამართულ მდგომარეობაში დასაშვები ნორმის<br />გამონაბოლქვს გამოყოფს და გარემოს ნაკლებად აბინძურებს.',
                bot: '',
                progLoader : progLoader
            }),
            top_html: TEMP.PAGE_1({
                title: 'ასეთ ავტომობილებს, ჯი პი აი ჰოლდინგი წამახალისებელ პირობებს სთავაზობს:',
                user_data: LAST_DATA,
                ul: [
                    '20% ფასდაკლება ავტოდაზღვევაზე',
                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!',
                    'სიტიპარკის ერთ წლიანი ვაუჩერი'
                ],
                CO: data.co2
            })
        }));
    

    //switch (data.status) {
    //    case 1:
    //        app.html(TEMP.PAGE_WRAPPER({
    //            footerClass: '',
    //            pre_html: TEMP.PRE({
    //                type: 1,
    //                prog: 20,
    //                title: 'გილოცავთ, ავტომობილი ეკომეგობრულია',
    //                desc: 'შენი მანქანა გამართულ მდგომარეობაში დასაშვები ნორმის<br />გამონაბოლქვს გამოყოფს და გარემოს ნაკლებად აბინძურებს.',
    //                bot: ''
    //            }),
    //            top_html: TEMP.PAGE_1({
    //                title: 'ასეთ ავტომობილებს, ჯი პი აი ჰოლდინგი წამახალისებელ პირობებს სთავაზობს:',
    //                user_data: LAST_DATA,
    //                ul: [
    //                    '20% ფასდაკლება ავტოდაზღვევაზე',
    //                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
    //                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!',
    //                    'სიტიპარკის ერთ წლიანი ვაუჩერი'
    //                ],
    //                CO: data.co2
    //            })
    //        }));
    //        break;
    //    case 2:
    //        app.html(TEMP.PAGE_WRAPPER({
    //            footerClass: 'footer-rev',
    //            pre_html: TEMP.PRE({
    //                type: 1,
    //                prog: 15,
    //                title: 'ავტომობილი საშუალოდ ეკომეგობრულია',
    //                desc: '',
    //                bot: ''
    //            }),
    //            top_html: TEMP.PAGE_1({
    //                title: 'ასეთ ავტომობილებს, ჯიპიაი ჰოლდინგი წამახალისებელ პირობებს სთავაზობს:',
    //                user_data: LAST_DATA,
    //                ul: [
    //                    '15% ფასდაკლება ავტოდაზღვევაზე',
    //                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
    //                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!',
    //                    'სიტიპარკის ერთ წლიანი ვაუჩერი'
    //                ],
    //                CO: data.co2
    //            })
    //        }));
    //        break;
    //    case 3:
    //        app.html(TEMP.PAGE_WRAPPER({
    //            footerClass: '',
    //            pre_html: TEMP.PRE({
    //                type: 1,
    //                prog: 15,
    //                title: 'გილოცავთ, ავტომობილი ეკომეგობრულია!',
    //                desc: 'შენი მანქანა გამართულ მდგომარეობაში დასაშვები ნორმის<br />გამონაბოლქვს გამოყოფს და გარემოს ნაკლებად აბინძურებს.',
    //                bot: ''
    //            }),
    //            top_html: TEMP.PAGE_2({
    //                title: 'ასეთ ავტომობილებს, ჯიპიაი ჰოლინგი წამახალისებელ პირობებს სთავაზობს:',
    //                user_data: LAST_DATA,
    //                ul: [

    //                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
    //                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!'

    //                ],
    //                CO: data.co2
    //            })
    //        }));
    //        break;
    //    case 4:
    //        app.html(TEMP.PAGE_WRAPPER({
    //            footerClass: 'footer-rev',
    //            pre_html: TEMP.PRE({
    //                type: 2,
    //                prog: 80,
    //                title: 'ავტომობილი ეკომავნეა',
    //                desc: 'აღნიშნული ავტომანქანა გამართულ მდგომარეობაშიც ჭარბად გამოყოფს ტოქსიკურ ნივთიერებებს.',
    //                bot: 'სამწუხაროდ, თუ საბედნიეროდ ამის გამო არავინ დაგაჯარიმებს, თუმცა არსებობს გარკვეული წესები, რომელთა დაცვაც მნიშვნელოვნად შეამცირებს შენი მანქანის მიერ ჰაერის დაბინძურებას.'
    //            }),
    //            bot_html: TEMP.PAGE_2({
    //                title: 'გაეცანი ჯიპიაი ჰოლდინგის წამახალისებელ სპეციალურ პირობებს',
    //                user_data: LAST_DATA,
    //                ul: [
    //                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
    //                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!'
    //                ],
    //                CO: data.co2
    //            })
    //        }));
    //        break;
    //    case 5:
    //        app.html(TEMP.PAGE_WRAPPER({
    //            footerClass: '',
    //            pre_html: TEMP.PRE({
    //                type: 1,
    //                prog: 15,
    //                title: 'ავტომობილი საშუალოდ ეკომეგობრულია',
    //                desc: '',
    //                bot: ''
    //            }),
    //            top_html: TEMP.PAGE_2({
    //                title: 'ასეთ ავტომობილებს, ჯიპიაი ჰოლდინგი წამახალისებელ პირობებს სთავაზობს:',
    //                user_data: LAST_DATA,
    //                ul: [
    //                    'სპეციალური ტარიფი',
    //                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
    //                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!'
    //                ],
    //                CO: data.co2
    //            })
    //        }));
    //        break;
    //    case 6:
    //        app.html(TEMP.PAGE_WRAPPER({
    //            footerClass: '',
    //            pre_html: TEMP.PRE({
    //                type: 1,
    //                prog: 15,
    //                title: 'გილოცავთ, ავტომობილი ეკომეგობრულია',
    //                desc: '',
    //                bot: ''
    //            }),
    //            top_html: TEMP.PAGE_2({
    //                title: 'ასეთ ავტომობილებს, ჯიპიაი ჰოლდინგი წამახალისებელ პირობებს სთავაზობს:',
    //                user_data: LAST_DATA,
    //                ul: [
    //                    'სპეციალური ტარიფი',
    //                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
    //                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!'
    //                ],
    //                CO: data.co2
    //            })
    //        }));
    //        break;

    //    case 7:
    //        app.html(TEMP.PAGE_WRAPPER({
    //            footerClass: '',
    //            pre_html: TEMP.PRE({
    //                type: 1,
    //                prog: 15,
    //                title: 'ავტომობილი საშუალოდ ეკომეგობრულია',
    //                desc: '',
    //                bot: ''
    //            }),
    //            top_html: TEMP.PAGE_2({
    //                title: 'ასეთ ავტომობილებს, ჯიპიაი ჰოლდინგი წამახალისებელ პირობებს სთავაზობს:',
    //                user_data: LAST_DATA,
    //                ul: [

    //                    'ვიანორის 50% ფასდაკლების ვაუჩერი',
    //                    'გამონაბოლქვის შემოწმება დაბინძურების ხარისხზე - უფასოდ!'
    //                ],
    //                CO: data.co2
    //            })
    //        }));
    //        break;
    //    default: 
    //}


    if (hide_pre === true) {
       // $('#home-btn, #nav-btn, #social-share').hide();
	   $('#home-btn,  #social-share').hide();
	   $('#pre-result').remove();
	   $('#result').show();
	   $('#offer-page').hide();
	   app.scrollTop(0);
    }

    if (location.hash.length == 0)
        setTimeout(function () {
            return false;
            console.log('autoscroll');
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
            //APP.events.onMousewheelResult({ deltaY: -1 });
        }, 5000);
    $('.load-car').click(function () {
        $('#result').show();
        $('#offer-page').slideUp();
        app.scrollTop(0);
    });

    $('#btn-rules').click(function () {
        $('#right-side-info').fadeOut(function () {
            $('#right-side-rules').fadeIn(function () {
                $('.left-side').removeClass('brd-right');
                $('.right-side').addClass('brd-left');
            });
        })
    })

    var $form2 = $('#reg-form');
    
    var phone = $form2.find('input[name="phone"]');
 
    $('#circle-prct').html(data.percent);

    $('#registration-btn').click(function () {
        var name = $form2.find('input[name="name"]').val();
        var $form1 = $('#form-1');
        var type, price, engine;

        switch (data.type) {
            case 1:
                type = 'ჯიპი';
                break;
            case 2:
                type = 'კუპე';
                break;
            case 3:
                type = 'სედანი';
                break;
        }

        switch (data.engine) {
            case 1:
                engine = '1500-მდე';
                break;
            case 2:
                engine = '1500 - 2500-მდე';
                break;
            case 3:
                engine = '2500 და მეტი';
                break;
        }
        switch (data.price) {
            case 1:
                price = '5000$-მდე';
                break;
            case 2:
                price = '5000$ - 10000$-მდე';
                break;
            case 3:
                price = '10001$ - 20000$-მდე';
                break;
            case 4:
                price = '20001$ - 35000$-მდე';
                break;
            case 5:
                price = '35001$ - 50000$-მდე';
                break;
            case 6:
                price ='50001$ - 100000$-მდე';
                break;
        }

        var year = data.year;


     

        if (!name) {
            alert('გთხოვთ შეავსეთ სახელი / გვარის ველი');
            return false;
        }
        if (!(/^\d-?\d{2}-?\d{3}-?\d{3}/.test(phone.val())) || phone.val().length != 12) {
            alert('ტელეფონი მიუთითეთ სწორად');
            return false;
        }

        $.post(U + 'ajax/registration', { type: type, engine: engine, year: year, price: price, name: name, phone: phone.val() }, function (success) {
            if (success) {
                $('.left-side-down').hide()
                    .html('<h3 style="margin-top:0;">გმადლობთ,  განაცხადი მიღებულია. დეტალებისთვის, ჩვენი აგენტი დაგიკავშირდებათ. <br><br>გისურვებთ ბედნიერ დღეს!</h3>').fadeIn();
            } else {
                alert('გთხოვ, შეიყვანო მონაცემები');
            }
        });
    });
   
    phone
        .keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;
            $phone = $(this);

            // Auto-format- do not expose the mask as the user begins to type
            if (key !== 8 && key !== 9) {
                if ($phone.val().length === 1) {
                    $phone.val($phone.val() + '-');
                }
                if ($phone.val().length === 4) {
                    $phone.val($phone.val() + '-');
                }
                if ($phone.val().length === 4) {
                    $phone.val($phone.val() + '-');
                }
                if ($phone.val().length === 8) {
                    $phone.val($phone.val() + '-');
                }

            }

            // Allow numeric (and tab, backspace, delete) keys only
            return (key == 8 ||
                    key == 9 ||
                    key == 46 ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
        })

        .bind('focus click', function () {
            $phone = $(this);
            var val = $phone.val();
            $phone.val('').val(val); // Ensure cursor remains at the end

        });
    APP.init_result();
};
APP.events.onSendFormClick = function () {
    console.log('onSendFormClick');
    LAST_DATA = $('#form-1').serializeArray();
    console.log('fromData', LAST_DATA);

    if (//$('[name="make"]').val().length == 0 ||
        //$('[name="model"]').val().length == 0 ||
		$('[name="type"]').val().length == 0 ||
        $('[name="engine"]').val().length == 0 ||
        $('[name="year"]').val().length == 0) {
        alert('გთხოვ, შეიყვანო მონაცემები');

    } else {

        var val2 = $('[name="engine"]').val();
        if (!(/^\d{1,}$/.test(val2))) {
            alert('გთხოვთ მიუთითოთ ძრავის მოცულობა');
            return false;
        }
        
        $.post(U + 'ajax/sendForm', LAST_DATA, APP.events.onSendFormDone);
        LAST_DATA = JSON.stringify(LAST_DATA);
    }
};
APP.events.onRegistrationClick = function () {

    var val = $('input[name="phone"]').val();

    if (!(/^\d-?\d{2}-?\d{3}-?\d{3}/.test(val))) {
        alert('ტელეფონი მიუთითეთ სწორად');
        return false;
    }

    ga('send', 'event', { eventCategory: 'Form', eventAction: 'Submit', eventLabel: 'Submit' });


    $.post(U + 'ajax/registration', $('#reg-form').serialize(), function (success) {
        if (success) {

            window._fbq.push(['track', '6026315666788', { 'value': '0.00', 'currency': 'USD' }]);

            $('#reg-form-1').find('h3').html('გმადლობთ,<br>ეკოპოლისის განაცხადი მიღებულია. დეტალებისთვის, ჩვენი აგენტი დაგიკავშირდება. <br><br>გისურვებთ ბედნიერ დღეს!').end().find('div').empty();
            if ($('#reg-form-1').length == 0) {
                var $WIN = $('#win').html('<h3 style="margin-top: 41px">გმადლობთ,<br>ეკოპოლისის განაცხადი მიღებულია. დეტალებისთვის, ჩვენი აგენტი დაგიკავშირდება. <br><br>გისურვებთ ბედნიერ დღეს!</h3>');

            }
        } else {
            alert('გთხოვ, შეიყვანო მონაცემები');
        }
    }, 'json');
};

APP.events.onNewRegistrationClick = function (data) {



}
APP.events.onWinMoreClick = function () {
    $('#win').fadeOut(function () {
        $('#win-more').fadeIn();
    });
};
APP.events.onWinMoreCloseClick = function () {
    $('#win-more').fadeOut(function () {
        $('#win').fadeIn();
    });
};

APP.result = {
    offset: null,
    active: 0,
    page1: {
        maxStep: 6,
        step_1: function () {
            console.log('step_1');
            APP.events.Scrolling = true;
            $('#result-scroll').animate({ marginTop: 0 }, function () {
                APP.events.Scrolling = false;
                APP.result.active = 0;
            });
        },
        step_2: function () {
            console.log('step_2');
            APP.events.Scrolling = true;
            $('#result-scroll').animate({ marginTop: APP.result.offset - 170 }, 1000, function () {
                APP.events.Scrolling = false;
                APP.result.active = 1;

                $CAR_IMG.css('transform', 'rotate(0deg)');
                $CAR.css({ top: 690, left: 380 }).fadeIn();
            });
        },
        step_3: function () {
            console.log('step_3');
            APP.events.Scrolling = true;
            $('#result-scroll').animate({ marginTop: APP.result.offset - 1426 }, 1000, function () {
                APP.events.Scrolling = false;
                APP.result.active = 2;

                $CAR_IMG.css('transform', 'rotate(90deg)');
                $CAR.css({ top: 1480, left: 150 }).fadeIn();
            });
        },
        step_4: function () {
            console.log('step_4');
            APP.events.Scrolling = true;
            $('#result-scroll').animate({ marginTop: APP.result.offset - 2150 }, 1000, function () {
                APP.events.Scrolling = false;
                APP.result.active = 3;

                $CAR_IMG.css('transform', ' rotate(180deg)');
                $CAR.css({ top: 2385, left: 155 }).fadeIn();
            });
        },
        step_5: function () {
            console.log('step_5');
            APP.events.Scrolling = true;
            $('#result-scroll').animate({ marginTop: APP.result.offset - 3500 }, 1000, function () {
                APP.events.Scrolling = false;
                APP.result.active = 4;

                $CAR_IMG.css('transform', ' rotate(90deg)');
                $CAR.css({ top: 3867, left: 395 }).fadeIn();
            });
        },
        step_6: function () {
            console.log('step_6');
            APP.events.Scrolling = true;
            $('#result-scroll').animate({ marginTop: APP.result.offset - 4200 }, 1000, function () {
                APP.events.Scrolling = false;
                APP.result.active = 5;

                $CAR_IMG.css('transform', ' rotate(0deg)');
                $CAR.css({ top: 10, left: 10 }).fadeIn();
            });
        },
        step1: function () {
            console.log('step1');
            if (APP.result.offset == null) {
                APP.result.offset = -$('#top-result').height() + 340;
                console.log('#APP.result.offset', APP.result.offset);
            }

            //if (IsBotPage()) {
            //    APP.result.active = 1;
            //    APP.result.page1.step2();
            //}

            APP.events.Scrolling = true;

            $('#result-scroll').animate({ marginTop: APP.result.offset - 170 }, 1000, function () {
                APP.events.Scrolling = false;
                APP.result.active = 1;

                $('#text-0').fadeIn();
            });
        },
        step2: function () {
            console.log('step2');
            APP.events.Scrolling = true;

            $('#result-scroll').animate({ marginTop: APP.result.offset - 1426 }, 5000, function () {

                $('#text-1').fadeIn();
                setTimeout(function () { $('#text-2').fadeIn() }, 500);
            });


        },
        step3: function () {
            console.log('step3');
            APP.events.Scrolling = true;








        },
        step4: function () {
            console.log('step4');
            APP.events.Scrolling = true;

            $('#result-scroll').animate({ marginTop: APP.result.offset - 3500 }, PAGE_SCROLL_SPEED * 3, function () {

                $('#text-6').fadeIn();
                setTimeout(function () { $('#text-7').fadeIn() }, 500);
                setTimeout(function () { $('#text-8').fadeIn() }, 1000);
            });

        },
        step5: function () {
            console.log('step5');
            APP.events.Scrolling = true;

            $('#result-scroll').animate({ marginTop: APP.result.offset - 4200 },
                PAGE_SCROLL_SPEED, function () {
                    APP.events.Scrolling = false;
                    APP.result.active = 5;
                });


        },
        step6: function () {
            console.log('step6');
            APP.events.Scrolling = true;

            if (IsBotPage()) {
                $('#result-scroll').animate({
                    marginTop: -$('#result-scroll').height() + APP.events.wHeight
                }, PAGE_SCROLL_SPEED, function () {
                    APP.events.Scrolling = false;
                    APP.result.active = 6;
                });
            } else {
                $('#result-scroll').animate({ marginTop: APP.result.offset - 4720 }, PAGE_SCROLL_SPEED, function () {
                    APP.events.Scrolling = false;
                    APP.result.active = 6;
                });
            }
        }
    }
};