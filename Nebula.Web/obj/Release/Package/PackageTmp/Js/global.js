
var loaderInterval = '';

$("input").attr("autocomplete", "off");

function GoToPage(returnUrl, obj) {
    $('#return-url').val(returnUrl);
    $('.returnUrl').val(returnUrl);
    $.getJSON(U + 'Account/IsAuthenticated', function (response) {
        if (response.isAuthenticated) {
            location.href = returnUrl;
        } else {
           
            $('#enterBut').click();
            $(obj).css('cursor', 'pointer');
        }
    });
}

$('#uploadId').on("click",function(e) {
    $('#social-file').click();
});

$(document).on("click", function (e) {
    var container = $(".container-reg-enter");
    var $jconfirm = $('.jconfirm.jconfirm-white');

    if ($jconfirm.length !== 0)
        return;

    if (!container.is(e.target) // if the target of the click isn't the container...
        && container.has(e.target).length === 0) // ... nor a descendant of the container
    {
        $(".container-reg-enter").removeClass("active");
        $('body').css('overflow-y', 'auto');
    }
});

$(document).on('click', '.auth-require-page-click', function () {
    var $this = $(this);
    $this.css('cursor', 'wait');
    if ($this.data('val') == 'show-pdf-popup') {
        $('.show-pdf-inp').val(true);
    }
    GoToPage($this.data('val'), $this);
});

function pageLoading(bool) {
    if (bool) {
        $('.cs-loading').hide();
        $('body').css('overflow', 'hidden');
        loaderInt(true);
        $('#loader-cont').show();

    } else {
        loaderInt(false);
        $('body').css('overflow-y', 'scroll');
        $('#loader-cont').hide();
        $('.cs-loading').fadeIn();
    }
}

// პოპაპის გახსნა რეგისტაციაზე კლიკის დროს
$('#reg-form').on('click', function (e) {
    e.stopPropagation();
    $('#enterBut').click();
});
$('#enterBut').on('click', function (e) {
    e.stopPropagation();
    $('#return-url').val(U + 'CSMain');
    $('.returnUrl').val(U + 'CSMain');
    $('.container-reg-enter').addClass("active");
    $('body').css('overflow-y', 'hidden');
    $('html, body').animate({
        scrollTop: 0
    }, 1000);
});

// პოპაპის გახსნა რეგისტაციაზე კლიკის დროს
// პოპაპის დახურვა
$('.closse').on('click', function () {
    $('.container-reg-enter').removeClass("active");

    $('body').css('overflow-y', 'scroll');
});

// პოპაპის დახურვა
// რეგისტრაციდან ავტორიზაციაზე გადასვლა და პირიქით
$('#aut-form').on('click', function () {
    $('body').css('overflow-y', 'hidden');
});

//$('#reg-form').on('click', function () {
//    $('body').css('overflow-y', 'hidden');
//});

$('#external-login-btn, #external-login-btn-free, #external-login-btn-course')
    .on('click',
        function () {
            $('.show-pdf-inp').val(true);
            var $this = $(this);
            $this.css('cursor', 'wait');
            $('.returnUrl').val($this.data('val'));
            GoToPage($this.data('val'), $this);
        });

function numletterHelper(index) {
    var L = '';
    switch (index) {
        case 0:
            L = 'ა';
            break;
        case 1:
            L = 'ბ';
            break;
        case 2:
            L = 'გ';
            break;
        case 3:
            L = 'დ';
            break;
        case 4:
            L = 'ე';
            break;
        case 5:
            L = 'ვ';
            break;
    }
    return L;
}

function errorsAlert(item) {
    $.alert({
        title: item.title,
        content: item.content,
        confirmButton: 'დახურვა',
        columnClass: 'col-md-4 col-md-offset-4'
    });
}

function mathFormulaChecker(bool) {
    if (bool) {
        if ($('#math-jax-area').find('.math-tex').length) {
            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
        }
    } else {
        MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
    }
}

$('#not-popup')
    .on('click',
        function () {
            $.post(U + 'Ajax/NotificationOpen')
                .done(function () {
                    $('#notification-count').text(0);
                });
        });

function showPdfPopup(bool) {
    if (bool) {
        $('.container-reg-enter').fadeOut(function () {
            $('.pdf_vers').fadeIn();
        });
    } else {
        $('.pdf_vers').hide();
    }
}

$('#load-pdf').on('click', function () {
    var $this = $(this);
    $this.prop('disabled', true);
    $this.next('.pdf-loader').css('visibility', 'visible');
    var pdfRes = $('#pdf-result');

    pdfRes.append('<li>searching...</li>');
    $.post(U + 'Ajax/SearchPdfLinks', function (response) {
        if (response.error) {
            alert(response.error);
            return false;
        }
        if (response)
            pdfRes.empty();

        $.each(response, function (i, v) {
            setTimeout(function () {
                pdfRes.html('<p><a href="' + v + '" target="_blank">' + v + '<a/></p>');

            }, 200 * i);
        });

        setTimeout(function () {
            location = U + 'CSMAin';
        }, 200 * (response.length - 1));

    }).done(function () {
        $this.next('.pdf-loader').css('visibility', 'hidden');
        $this.prop('disabled', false);
    });

    $('.mask').on('click', function () {
        $('.upload-pdf-popup, .mask').hide();
    });
});


$('#pdf-lnk').on('click', function () {
    $('#pdfs').click();
});

if (document.getElementById("pdfs")) {
    document.getElementById("pdfs").onchange = function () {
        $("#upload-pdfs-btn").click();
    };
}

$('#upload-pdfs-btn').on('click', function (e) {
    $('.upload-pdf-loader').css('visibility', 'visible');
    e.preventDefault();
    var fd = new FormData();
    var ins = document.getElementById('pdfs').files.length;

    for (var x = 0; x < ins; x++) {
        fd.append("pdfs[]", document.getElementById('pdfs').files[x]);
    }
    var ajax = new XMLHttpRequest();

    ajax.open("POST", U + 'Ajax/UploadPdfs');
    ajax.send(fd);
    var error = false;
    ajax.onreadystatechange = function () {
        var i = 0;
        if (ajax.status == 200 && ajax.readyState == 4) {
            var response = JSON.parse(ajax.responseText);

            $('.upload-pdf-loader').css('visibility', 'hidden');
            alert(response);
        } else {
            error = true;
        }
    }
    setTimeout(function () {
        if (error) {
            alert("კონვეტაციისას მოხდა შეცდომა, გთხოვთ გამოიყენოთ ჩამოტვირთვის ღილაკი");
            $('.upload-pdf-loader').css('visibility', 'hidden');
        }
    }, 4000);
});

$('.mask').on('click', function () {
    showPdfPopup(false);
});

function showFgSendForm(bool) {
    if (bool) {
        $('.reg-header').hide();
        $('.forgot-pass-header').show();

    } else {
        $('.forgot-pass-header').hide();
        $('.reg-header').show();
    }
}

$('#forgot-password-al').on('click', function () {
    console.log('clicks');
    showFgSendForm(true);
});

function loaderInt(bool) {
    if (bool) {
        var IconsCount = $('.loader-icon').length;

        $('.loader-icon_' + (1)).fadeIn(100, function () {
            $('.loader-icon_' + (1)).fadeOut(300);
        });

        var imgCount = 2;
        loaderInterval = setInterval(function () {
            if (imgCount > IconsCount) {
                imgCount = 1;
            }

            $('.loader-icon_' + (imgCount)).fadeIn(100, function () {
                $('.loader-icon_' + (imgCount)).fadeOut(300);
                imgCount++;
            });

        }, 400);
    } else {
        clearInterval(loaderInterval);
    }
}

function GenerateErrorString(errorCode) {
    var error = '';
    switch (errorCode) {
        case 'pass-val':
            error = "პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან";
            break;
        case 'firstname-val':
            error = "სახელი უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან";
            break;
        case 'lastname-val':
            error = "გვარი უნდა შედგებოდეს მინიმუმ 4 სიმბოლოსგან";
            break;
        case 'phone-val':
            error = "გთხოვთ, სწორად შეიყვანოთ მობილურის ნომერი";
            break;
        case 'identification-val':
            error = "გთხოვთ, სწორად შეიყვანოთ პირადი ნომერი";
            break;
    }
}


var left = 0;
var prop = "";
var count = 0;

$(document).ready(function () {

    $('#logBut').click(function () {
        //alert("eeee");
        //$('#dd').click();
        $('#dd').trigger('click');
    });

    $(document).on("click", ".video-pre-blocks", function (e) {
        e.stopPropagation();
    });


    $(document).on("click", ".video-blocks-main", function () { 
        if ($(this).hasClass("active")) {
            $(this).removeClass("active");
            $(this).find(".video-pre-blocks").slideUp();
        }
        else {
            $(this).addClass("active");
            $(this).find(".video-pre-blocks").slideDown();
        }
    });


    $('.next-videos.next')
        .click(function () {

            if (count < 25) {
                $(".number-divs.prev").addClass('number-div-long-left');
                left -= 100;
                prop = left + "px";

                console.log(prop);
                $(".number-divs").css("margin-left", prop)

                count += 1;
            } 
        });


    $('.next-videos.prev')
    .click(function () {

        if (count != 0) {

            $(".number-divs.prev").addClass('number-div-long-left');

            if (left != 0) {
                left += 100;
                prop = left + "px";

                console.log(prop);


                $(".number-divs").css("margin-left", prop);
            }


            count -= 1;
        }


    });

    $(".video-blocks-main").click(function () {
        $(".video-blocks-main").addClass('.video-pre-blocks-active');
    });

    $(document).on('keydown', '.only-numbers', function (e) {
        var key = e.charCode || e.keyCode || 0;
        return (key == 8 ||
           key == 9 ||
           key == 46 ||
           (key >= 48 && key <= 57) ||
           (key >= 96 && key <= 105));
    });
});