var TEMP = {}, APP = {}, $W = $(window);
var PAGE_SCROLL_SPEED = 2500;
var CAR_SPEED = 500;
var LAST_DATA = null;
var $CAR, $CAR_IMG;

function CarDrive(top, topTime, left, leftTime, oldDef, deg, degTime, callback) {
    console.log('CarDrive', arguments)
    var position = $CAR.position();
    var called = 0;

    $({ top: position.top }).animate({ top: top }, {
        duration: topTime,
        step: function (now) {
            //console.log('top', now);
            $CAR.css({
                top: now
            });
        },
        complete: function () {
            called++;
            if ($.isFunction(callback) && called == 3) {
                callback();
            }
        }
    });

    $({ left: position.left }).animate({ left: left }, {
        duration: leftTime,
        step: function (now) {
            //console.log('left', now);
            $CAR.css({
                left: now
            });
        },
        complete: function () {
            called++;
            if ($.isFunction(callback) && called == 3) {
                called = true;
                callback();
            }
        }
    });

    $({ deg: oldDef }).animate({ deg: deg }, {
        duration: degTime,
        step: function (now) {
            //console.log('rotate', now);
            $CAR_IMG.css({
                transform: "rotate(" + now + "deg)"
            });
        },
        complete: function () {
            called++;
            if ($.isFunction(callback) && called == 3) {
                called = true;
                callback();
            }
        }
    });

}

$('script[type="text/x-handlebars-template"]').each(function () {
    var $this = $(this);
    TEMP[$this.data('template-name')] = Handlebars.compile($this.html().trim());
    $this.remove();
});