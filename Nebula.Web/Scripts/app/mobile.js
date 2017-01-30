var wH = null, wW = null, $W = $(window);
$W.on('resize', function () {
    wH = $W.height();
    wW = $W.width();
    document.title = $W.width() + 'x' + wH;
    $('.hh').height(wH);
    $('.h').height(wH - 170);
    //$('#n-m-wrapper').width(wW);
    //$('#n-m-scroll').width(wW * 8).find('div').width(wW);
}).trigger('resize');

//var $loaderCount = $('#loader-count');
//$({ x: 0 }).animate({ x: 313 }, {
//    duration: 2000,
//    step: function (now) {
//        $loaderCount.html(parseInt(now) + ' %');
//    },
//    complete: function () {
//        $('#page-1').fadeOut(function () {
//            $('#page-2, #nav-wrapper').fadeIn();
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
                $('#page-1').fadeOut(function () {
                     $('#page-2, #nav-wrapper').fadeIn();
                     });
            }, 2000)
        }, 2000);




        return false;
    }


    percent++;

    $('#loader-img-color').css('top', '-' + percent + '%');
    $('#percent-number').text(percent);

    //  $('#loader-img-fg-color').css('top','-'+ percent + '%')
}, 35);


function START() {
    $('#nav-wrapper').removeClass('page-2-remove');
    $('#page-2').hide();
    $('#page-3').slideDown();
}

$('.autocomplete').each(function () {
    var $this = $(this);
    $this.autocomplete({
        minLength: parseInt($this.data('x')),
        source: function (request, response) {
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: U + 'ajax/search/' + $this.attr('name'),
                data: $('#form-1').serialize(),
                dataType: "json",
                success: response
            });
        }
    }).on("focus", function () {
        $(this).autocomplete("search", "");
    });;
});

function AddRev() {
    $('body, #nav-wrapper').addClass('rev');
}

function RemoveRev() {
    $('body, #nav-wrapper').removeClass('rev');
}
var LAST_DATA = null;
$('#send-form-btn').on('click', function () {
    LAST_DATA = $('#form-1').serializeArray();
    console.log('fromData', LAST_DATA);
    if (//$('[name="make"]').val().length == 0 ||
       // $('[name="model"]').val().length == 0 ||
	      $('[name="type"]').val().length == 0 ||
        $('[name="engine"]').val().length == 0 ||
        $('[name="year"]').val().length == 0) {
        alert('გთხოვ, შეიყვანო მონაცემები');

    } else {
        $.post(U + 'ajax/sendForm', LAST_DATA, function (data) {
            if (data.status == -1)
            {
                alert('აღნიშნული დასახელება ვერ მოიძებნა');
                return false;
            }
            var title = '';
            var imgType = '';
            if (data.engine == 1) {
                title = 'გილოცავ!  შენი მანქანა ეკომეგობრულია. <br/> საკმარისია, თუკი დარგავ სამ-ოთხ ხეს.';
            } else if (data.engine == 2) {
                title = 'შენი მანქანა ნახევრად ეკომეგობრულია. <br/> შენ ხის დარგვა წესად უნდა დარგო.';
            } else if (data.engine == 3) {
                title = 'შენი მანქანა სრულიად არაეკომეგობრულია. <br/> შენ უნდა გააშენო ტყე.';
            }
            if (data.type == 1) {
                imgType = 4;
            } else {
                imgType = 3;
            }
            $('#result-status').html(title);
            if (imgType == 3) {
                $('#result-img').html('<img src="' + U + 'content/img/car-' + imgType + '.png" style="width:300px;" />');
            } else {
                $('#result-img').html('<img src="' + U + 'content/img/car-' + imgType + '.png" style="width:350px;" />');
            }
   

            $('#page-3').slideUp();
            $('#circle-prct').html(data.percent);
            $('body').scrollTop(0);
            console.log('status', data.status);
            //if (data.status == 4) {
            //    $('#page-5').slideDown();
            //    setTimeout(function () {
            //        $('body').scrollTop(0);
            //        $('#page-5').slideUp();
            //        //$('#result-' + data.status).slideDown();
            //        $('#result-' + 1).slideDown();
            //    }, 3000);
            //} else {
                $('#page-4').slideDown();
                setTimeout(function () {
                    $('body').scrollTop(0);
                    $('#page-4').slideUp();
                    //$('#result-' + data.status).slideDown();
                    $('#result-' + 1).slideDown();
                }, 3000);
            
            var $form2 = $('#reg-form');

            var phone = $form2.find('input[name="phone"]');

            $('#circle-prct').html(data.percent);

            $('#registration-btn').click(function (e) {
                e.preventDefault();
                var name = $form2.find('input[name="name"]').val();
            
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
                        price = '50001$ - 100000$-მდე';
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
                    alert('here');
                    if (success) {
                        $('.left-side-down').hide()
                            .html('<h3 style="margin-top:0; font-size:30px;">გმადლობთ,<br> განაცხადი მიღებულია. დეტალებისთვის, ჩვენი აგენტი დაგიკავშირდებათ.<br><br>გისურვებთ ბედნიერ დღეს!</h3>').fadeIn();
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


        }, 'json');
        LAST_DATA = JSON.stringify(LAST_DATA);
    }
});

$('.more-btn').on('click', function () {
    $('#results').slideUp();
    $('#page-6').slideDown();
});

$('.reg-btn').on('click', function () {
    $('#results').slideUp();
    $('#page-7').slideDown().find('input[name="user_data"]').val(LAST_DATA);
});

$('#reg-form-btn').on('click', function () {
    var val = $('input[name="phone"]').val();

    if (!(/^\d-?\d{2}-?\d{3}-?\d{3}/.test(val))) {
        alert('ტელეფონი მიუთითეთ სწორად');
        return false;
    }


    $.post(U + 'ajax/registration', $('#reg-form').serialize(), function (success) {
        if (success) {

            $('body').scrollTop(0);
            $('#reg-form').html('<h3 style="margin-top: 41px">გმადლობთ,<br>ეკოპოლისის განაცხადი მიღებულია. დეტალებისთვის, ჩვენი აგენტი დაგიკავშირდება. <br><br>გისურვებთ ბედნიერ დღეს!</h3>');

            setTimeout(function () {

                $('body').scrollTop(0);
                $('#page-7').hide();
                $('#page-8').fadeIn();
            }, 1000);
        } else {
            alert('გთხოვ, შეიყვანო მონაცემები');
        }
    }, 'json');
});

$('#n-m-wrapper').on('swipeleft', function () {
    var index = ($('#n-m-nav > .active').index() + 1);
    if (index == 9)
    {
        $('#page-8').hide();
        AddRev();
        $('#page-9').slideDown();

    }
    $('#n-m-nav > li:eq(' + index + ')').click();
}).on('swiperight', function () {
    var index = ($('#n-m-nav > .active').index() -1) % 9;
    $('#n-m-nav > li:eq(' + index + ')').click();
});

$('.m-fb').on('click', function () {

    FB.ui({
        method: 'feed',
        name: 'გაიგე რამდენად აბინძურებს შენი მანქანა ჰაერს!',
        link: 'http://ecoist.ge/',
        picture: 'http://ecoist.ge/content/img/ekoist-share.png',
        caption: ' ',
        description: 'მე ვარ ეკოისტი. ვაზღვევ ავტომობილს “ჯიპიაი ჰოლდინგში“ და ვრგავ ჩემს ხეს.'
    });
});

$('#m-fb-2').on('click', function () {

    FB.ui({
        method: 'feed',
        name: 'გაიგე რამდენად აბინძურებს შენი მანქანა ჰაერს!',
        link: 'http://ecoist.ge/',
        picture: 'http://ecoist.ge/content/img/ekoist-share.png',
        caption: ' ',
        description: 'მე ვარ ეკოისტი. ვაზღვევ ავტომობილს “ჯიპიაი ჰოლდინგში“ და ვრგავ ჩემს ხეს.'
    });
});

$('#m-nav-share-2').on('click', function () {

    FB.ui({
        method: 'feed',
        name: 'გაიგე რამდენად აბინძურებს შენი მანქანა ჰაერს!',
        link: 'http://ecoist.ge/',
        picture: 'http://ecoist.ge/content/img/ekoist-share.png',
        caption: ' ',
        description: 'მე ვარ ეკოისტი. ვაზღვევ ავტომობილს “ჯიპიაი ჰოლდინგში“ და ვრგავ ჩემს ხეს.'
    });
});

$('#nav-btn').on('click', function () {
    $('#nav-content').fadeIn();
});

$('#nav-nav-close').on('click', function () {
    if ($('#nav-ul-wrapper').data('close') == 1) {
        $('#nav-ul-wrapper').slideDown().data('close', '0');
        $('#li-0, #li-1, #li-2, #li-3, #li-4, #li-5').hide();
    } else {
        $('#nav-content').slideToggle();
    }
});


$('.close-btn-1').on('click', function () {
    $(this).closest('.page').slideUp();
    $('#results').slideDown();
});

$('#con-form > button').on('click', function () {
    $.post(U + 'ajax/contact', $('#con-form').serialize(), function (data) {
        if (data == 1)
            $('#con-form').empty().html('<h2>გმადლობთ, თქვენი წერილი მიღებულია.</h2>');
        else
            $('#con-form').empty().html('<h2>დაფიქსირდა შეცდომა, სცადეთ მოგვიანებით</h2>');
    });
});

var $navLi = $('.nav-li').on('click', function () {
    $navLi.removeClass('active');
    $(this).addClass('active');
    var id = parseInt($(this).data('id'));

    if (id == '44') {
        $('#nav-nav-close').click();
        $('.page').hide();
        $('#page-8').fadeIn();
        return;
    }

    $('#nav-ul-wrapper').slideUp(function () {
        $(this).data('close', '1');
    });
    
    $('#li-0, #li-1, #li-2, #li-3, #li-4 ,#li-5').hide();
    console.log('#li-' + id);
    $('#li-' + id).fadeIn();
});

var nmThis,nmDivs;
var nmLi = $('#n-m-nav > li').on('click', function () {
    nmLi.removeClass('active');
    nmThis = $(this).addClass('active');
    
    if (nmThis.index() == 0)
    {
        $('#n-m').html('8');
        $('#n-m-top').hide();
        $('#n-m-bot').show();
    } else {
        $('#n-m-top').show();
        $('#n-m-bot').hide();
        $('#n-m').html(nmThis.index());
    }

    nmDivs = $('#n-m-scroll > div').hide();
    $(nmDivs).eq(nmThis.index()).fadeIn();
});


(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

ga('create', 'UA-62136744-1', 'auto');
ga('send', 'pageview');

window.fbAsyncInit = function () {
    FB.init({
        appId: '905547436172604',
        xfbml: true,
        version: 'v2.3'
    });
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));