/// <reference path="common/exercise-common.js" />

pageLoading(true);
var id = $('#VideoId').val();
var ModuleId = $('#ModuleId').val();
var circles = [];
var color = ['#BEE3F7', '#45AEEA'];
var player = {};

function questionViewModel(item) {
    var self = this;
    self.question = item.question;
    self.id = item.id;
    self.answers = [];
    $.each(item.answers, function (i, v) {
        self.answers.push({ letter: numletterHelper(i), answer: v.answer, id: v.id });
    });
}

function videoViewModel() {
   
    var self = this;

    self.exercisePostQuestionsViewModel = new ExerciseQuestionsViewModel();
    self.exercisePostQuestionsViewModel.setAnswerUrl = U + 'CSVideo/SetAnswer';


    self.dataCountImg = ko.observable(5);
    self.exerciseType = ko.observable(false);
    self.hasLicense = ko.observable(false);
    self.videoSrc = ko.observable();
    self.activeQuestion = ko.observable();
    self.typeId = ko.observable();
    self.videoParts = ko.observableArray();
    self.tagsNavigation = ko.observableArray();
    self.markerPoints = ko.observableArray();
    self.questionNum = ko.observable(0);
    self.videoNavigation = ko.observableArray();
    self.questionPack = ko.observableArray();
    self.activeIndex = ko.observable(0);
    self.isNextBtnVisible = ko.observable(false);
    self.isPreviousBtnVisible = ko.observable(false);
    self.correctAnswer = ko.observable();
    self.correctAnswerId = ko.observable();
    self.incorrectAnswerId = ko.observable();
    self.selectedAnswerId = ko.observable();
    self.isFinishBtnVisible = ko.observable(false);
    self.markerClicked = ko.observable(false);
    self.correctAnswerCount = ko.observable(0);
    self.selectedVideoName = ko.observable();
    self.nextBtnDisable = ko.observable(false);
    self.alternateLink = ko.observable();

    //quiz variables
    self.historyNavigate = ko.observable(true);
    self.progressBar = ko.observableArray();
    self.questions = ko.observableArray();
    self.hasAnswered = ko.observable(false);
    self.isStartAgainBtnVisible = ko.observable(false);
    self.videoId = ko.observable();
    self.userAnswers = ko.observableArray();
    self.currentQuestionBtnVisible = ko.observable(false);

    // initialize video.js
    self.alternateVideo = function () {
        $("#video-container").hide();
        $('#alternate-video').show();
    }

    function initVideo(response) {
        self.videoId(response.id);
        self.alternateLink(response.alternateLink);


        if (response.isPayed) {
            $("#video-container").html(
            ' <video controls  data-setup=\'{ "aspectRatio":"640:267", "playbackRates": [1, 1.5, 2] }\'' +
                'class="video-js vjs-default-skin"  id="test_video"> ' +
              '<source src="' + U + 'Content/payed-videos/' + response.fileName + '" type="video/mp4"></source>' +
            '</video>');
        } else {
            $("#video-container").html(
                ' <video controls  data-setup=\'{ "aspectRatio":"640:267", "playbackRates": [1, 1.5, 2] }\'' +
                      'class="video-js vjs-default-skin"  id="test_video"> ' +
                    '<source src="' + U + 'Content/Videos/' + response.fileName + '" type="video/mp4"></source>' +
                '</video>');
        }

        //'</video>');
        self.videoSrc(U + 'Content/Videos/' + response.fileName);

        $('#test_video').bind('contextmenu', function () { return false; });

        //init videonavigation 
        player = videojs('test_video');
        var videoBreaks = [];

        //init video parts
        $.each(response.videoParts, function (i, v) {
            videoBreaks.push({ text: i + 1, time: v.second });
            self.questionPack.push(v.questions);
        });

        //load the marker plugin
        player.markers({
            markerTip: {
                display: false,
                text: function (marker) {
                    return "This is a break: " + marker.text;
                }
            },
            breakOverlay: {
                display: false,
                displayTime: 3,
                text: function (marker) {
                    return "This is an break overlay: " + marker.text;
                }
            },
            markerStyle: {
                'height': '8px',
                'top': '50%',
                'margin-top': '-4px',
                'border-radius': '0',
                'z-index': '999'
            },
            onMarkerReached: function (marker) {
                if (self.markerClicked())
                    return false;
                
                if (marker.time != Math.floor(player.currentTime()))
                    return false;

                self.correctAnswerCount(0);
                self.hasAnswered(false);
                //reset button visible
                self.isPreviousBtnVisible(false);
                self.isNextBtnVisible(false);
                self.incorrectAnswerId(null);
                self.selectedAnswerId(null);
                self.correctAnswer(null);
                self.correctAnswerId(null);
                self.progressBar.removeAll();

                $('.question-string').empty();
                $('.answers').empty();
                self.questionNum(1);
                var index = videoBreaks.indexOf(marker);
                self.activeIndex(index);
                var question = response.videoParts[index];

                //init question
                self.isFinishBtnVisible(false);
                self.activeQuestion(new questionViewModel(self.questionPack()[index][0]));
                if (self.typeId() == 2) {
                    mathFormulaChecker(true);
                }
                console.log(self.activeQuestion());

                for (i = 0; i < self.questionPack()[index].length; i++) {
                    self.progressBar.push(ko.observable('default'));
                }

                if (self.questionPack()[index].length > 1) {
                    // self.isNextBtnVisible(true);
                }

                $('.videoQuiz').fadeIn();

                player.pause();
            },
            onMarkerClick: function (marker) {
                //reset button visible
                self.correctAnswerCount(0);
                self.markerClicked(true);
                self.hasAnswered(false);
                self.isPreviousBtnVisible(false);
                self.isNextBtnVisible(false);
                self.incorrectAnswerId(null);
                self.selectedAnswerId(null);
                self.correctAnswer(null);
                self.correctAnswerId(null);
                self.progressBar.removeAll();

                $('.question-string').empty();
                $('.answers').empty();
                self.questionNum(1);
                var index = videoBreaks.indexOf(marker);
                self.activeIndex(index);
                var question = response.videoParts[index];

                //init question
                self.isFinishBtnVisible(false);
                self.activeQuestion(new questionViewModel(self.questionPack()[index][0]));
                if (self.typeId() == 2) {
                    mathFormulaChecker(true);
                }
                console.log(self.activeQuestion());

                for (i = 0; i < self.questionPack()[index].length; i++) {
                    self.progressBar.push(ko.observable('default'));
                }

                if (self.questionPack()[index].length > 1) {
                    //  self.isNextBtnVisible(true);
                }
                //  $('#movingSlider li').eq(index).addClass('active').siblings().removeClass('active');

                $('.videoQuiz').fadeIn();

                player.pause();
            },
            markers: videoBreaks
        });

        $(".next").click(function () {
            player.markers.next(0);
        });
        $(".prev").click(function () {
            player.markers.prev();
        });
        $(".remove").click(function () {
            player.markers.remove([1, 2]);
        });
        $(".add").click(function () {
            player.markers.add([{
                time: 40,
                text: "I'm NEW"
            }]);
        });

        $(".updateTime").click(function () {
            var markers = player.markers.getMarkers();
            for (var i = 0; i < markers.length; i++) {
                markers[i].time += 1;
            }
            player.markers.updateTime();
        });

        $(".reset").click(function () {
            player.markers.reset([{
                time: 40,
                text: "I'm NEW"
            },
               {
                   time: 20,
                   text: "Brank new"
               }]);
        });
        $(".destroy")
            .click(function () {
                player.markers.destroy();
            });

        document.onkeypress = function (e) {
            if ((e || window.event).keyCode === 32) {
                console.log('player', player);
                e.preventDefault();
                player.paused() ? player.play() : player.pause();
            }
        };
    }

    function initExercise(response) {
        //reset
        self.questions.removeAll();
        self.hasAnswered(false);
        self.currentQuestionBtnVisible(false);
        self.isNextBtnVisible(true);
        self.nextBtnDisable(true);
        self.progressBar.removeAll();
        self.userAnswers.removeAll();
        self.historyNavigate(true);
        self.incorrectAnswerId(null);
        self.selectedAnswerId(null);
        self.activeIndex(0);
        self.isStartAgainBtnVisible(false);
        self.activeQuestion(null);
        self.correctAnswer(null);

        console.log(response);
        self.exercisePostQuestionsViewModel.init({
            questions: response.exerciseQuestions,
            userAnswers: response.userAnswers
        });
        ko.utils.arrayForEach(response.exerciseQuestions, function (item) {
            console.log('item', item);
            var model = new questionViewModel(item);

            self.questions.push(model);
        });

        for (i = 0; i < self.questions().length; i++) {
            self.progressBar.push(ko.observable('default'));
        }
        self.activeIndex(response.userAnswers.length);
        self.activeQuestion(self.questions()[response.userAnswers.length]);
        if (self.questions().length == response.userAnswers.length) {
            self.isStartAgainBtnVisible(true);
        }

        $.each(response.userAnswers, function (i, v) {
            if (v.isCorrect) {
                self.progressBar()[i]('true');
            } else {
                self.progressBar()[i]('false');
            }

            self.userAnswers.push({
                answerId: v.answerId,
                questionId: v.questionId,
                isCorrect: v.isCorrect,
                correctAnswerId: v.correctAnswerId
            });

            self.activeIndex(i + 1);
        });
    }

    function initWrapper(response) {
        var videoUrl = '';
        var ind = 0;

        setTimeout(function () {
            ko.utils.arrayForEach(response.tagsNavigation, function (v) {
                if (response.hasLicense || !v.isPayed) {
                    videoUrl = U + 'video/index?ModuleId=' + v.id;
                } else {
                    videoUrl = U + 'payment';
                }

                self.tagsNavigation.push({
                    name: v.name,
                    id: v.id,
                    videoCount: v.videoCount,
                    selected: v.selected,
                    isPayed: v.isPayed,
                    url: videoUrl,
                    index: ind,
                    percent: Math.round(v.percent)
                });

                if ((!v.isPayed || response.hasLicense) && v.percent != -1) {
                    var child = document.getElementById('circles-' + ind);

                    console.log('name-id', v.name + '-' + child.id + 'color' + color + 'percent' + v.percent);
                    circles.push(Circles.create({
                        id: child.id,
                        value: v.percent,
                        radius: 30,
                        width: 5,
                        colors: color
                    }));
                }
                console.log('circles', circles);
                ind++;
            });
        }, 100);

        $('.verbal').text(response.type);
        self.typeId(response.typeId);
        self.hasLicense(response.hasLicense);

        var k = 1;
        var x = 1;
        if (response.videoNavigation) {
            if (response.videoNavigation.length <= 6) {
                // $('.goToPrev, .goToNext').hide();
            }
        }
        $.each(response.videoNavigation, function (i, v) {
            if (x < 10)
                k = '0' + x;
            else
                k = x;

            self.videoNavigation.push({ id: v.id, name: v.name, ModuleId: v.ModuleId, selected: v.selected, num: k });

            if (v.selected) {
                self.selectedVideoName(v.name);
                if (i > 4) {
                    self.dataCountImg(i);
                }
            }
            x++;
        });
    }

    if (id) {
        $.post(U + 'CSVideo/GetVideo', { videoId: id }, function (response) {
            initWrapper(response);
            if (response.video) {
                if (response.video.isExerciseType) {
                    self.exerciseType(true);
                    initExercise(response.video);
                }
                else {
                    self.exerciseType(false);
                    initVideo(response.video);
                }
            }
        }).done(function () {
            initPlayButtonsColor();
            if (self.typeId() == 2) {
                mathFormulaChecker(true);
            }
            pageLoading(false);
            setTimeout(function () {
                videoSlider({ dataCount: self.dataCountImg() });
                //   videoSlideNext();
            }, 100);
            $('[data-toggle="tooltip"]').tooltip();
        });
    } else if (ModuleId) {
        $.post(U + 'CSVideo/GetVideo', { ModuleId: ModuleId }, function (response) {
            initWrapper(response);
            if (response.video) {
                if (response.video.isExerciseType) {
                    self.exerciseType(true);
                    initExercise(response.video);
                } else {
                    self.exerciseType(false);
                    initVideo(response.video);
                }
            }
        }).done(function () {
            initPlayButtonsColor();
            if (self.typeId() == 2) {
                mathFormulaChecker(true);
            }
            pageLoading(false);
            setTimeout(function () {
                videoSlider({ dataCount: self.dataCountImg() });
                //   videoSlideNext();
            }, 100);

            $('[data-toggle="tooltip"]').tooltip();
        });
    }

    //helper functions
    self.setAnswer = function (item) {
        console.log(item);
        self.historyNavigate(false);
        self.selectedAnswerId(item.answerId);
        self.hasAnswered(true);

        $.post(U + 'CSVideo/SetAnswer',
                { answerId: item.answerId, questionId: self.activeQuestion().id },
                function (response) {
                    var incorrectAnswerId = -1;
                    var correctAnswer = $.grep(self.activeQuestion().answers,
                        function (answer) {
                            return answer.id == response.correctAnswerId;
                        });

                    self.correctAnswer(correctAnswer[0]);

                    if (correctAnswer[0].id == item.answerId) {
                        self.correctAnswerId(correctAnswer[0].id);
                        self.correctAnswerCount(self.correctAnswerCount() + 1);
                        if (self.exerciseType())
                            self.progressBar()[self.activeIndex()]('true');
                    } else {
                        self.correctAnswerId(correctAnswer[0].id);
                        self.incorrectAnswerId(item.answerId);
                        incorrectAnswerId = item.answerId;
                        if (self.exerciseType())
                            self.progressBar()[self.activeIndex()]('false');
                    }

                    self.userAnswers.push({
                        answerId: item.answerId,
                        questionId: self.activeQuestion().id,
                        isCorrect: incorrectAnswerId == -1 ? true : false,
                        correctAnswerId: response.correctAnswerId
                    });

                    if (!self.exerciseType()) {
                        var questionPack = self.questionPack()[self.activeIndex()];
                        console.log('questionpack', questionPack);
                        var question = $.grep(questionPack,
                            function (el, ind) {
                                return el.id == self.activeQuestion().id;
                            });
                        console.log('quest', question[0]);
                        var index = questionPack.indexOf(question[0]);
                        if (correctAnswer[0].id == item.answerId) {
                            self.progressBar()[index]('true');
                        } else {
                            self.progressBar()[index]('false');
                        }
                        if (index + 1 >= questionPack.length) {
                            var markers = player.markers.getMarkers();
                            if (self.correctAnswerCount() == questionPack.length) {
                                // make green
                                $('div[data-marker-key="' + markers[self.activeIndex()].key + '"]')
                                    .css('background-color', 'green');
                            } else {
                                $('div[data-marker-key="' + markers[self.activeIndex()].key + '"]')
                                    .css('background-color', 'red');
                            }
                            self.isFinishBtnVisible(true);
                            self.isNextBtnVisible(false);
                            // $('.videoQuiz').fadeOut();
                        } else {
                            self.isNextBtnVisible(true);
                            self.isFinishBtnVisible(false);
                        }
                    }

                    if (self.exerciseType()) {
                        if (self.questions().length == self.activeIndex() + 1) {
                            self.isNextBtnVisible(false);
                            self.currentQuestionBtnVisible(false);
                            self.isStartAgainBtnVisible(true);
                        } else {
                            self.nextBtnDisable(false);
                            // self.isNextBtnVisible(true);
                        }
                    }

                    self.historyNavigate(true);
                })
            .done(function () {
            });
    }

    self.getNextQuestion = function (item) { 
        self.isNextBtnVisible(false);
        self.hasAnswered(false);
        var questionPack = self.questionPack()[self.activeIndex()];

        var question = $.grep(questionPack, function (el, ind) {
            return el.id == item.question.id;
        });

        var index = questionPack.indexOf(question[0]);
        index++;

        self.activeQuestion(new questionViewModel(questionPack[index]));

        if (self.typeId() == 2) {

            mathFormulaChecker(true);
        }
    }

    self.getQuizNextQuestion = function (item) {
        self.nextBtnDisable(true);
        self.activeIndex(self.activeIndex() + 1);
        self.activeQuestion(self.questions()[self.activeIndex()]);
        self.nextBtnDisable(true);
        if (self.typeId() == 2) {
            mathFormulaChecker(true);
        }
        self.questionNum(self.activeIndex() + 1);
        self.hasAnswered(false);
    }

    self.getQuestionByNum = function (item) {
        self.activeIndex(item.index());

        if (!self.isStartAgainBtnVisible()) {
            self.currentQuestionBtnVisible(true);
        }

        self.isNextBtnVisible(false);
        self.activeQuestion(self.questions()[item.index()]);

        var correctAnswer = $.grep(self.userAnswers(), function (answer) {
            return answer.questionId == self.activeQuestion().id
        });

        if (correctAnswer[0].isCorrect) {
            self.correctAnswerId(correctAnswer[0].answerId);
        } else {
            self.correctAnswerId(correctAnswer[0].correctAnswerId);
            self.incorrectAnswerId(correctAnswer[0].answerId);
        }

        self.hasAnswered(true);

        if (self.typeId() == 2) {
            mathFormulaChecker(true);
        }
    }

    self.goToCurrentQuestion = function () {
        self.isNextBtnVisible(true);
        self.currentQuestionBtnVisible(false);
        self.activeIndex(self.userAnswers().length);
        self.activeQuestion(self.questions()[self.activeIndex()]);
        self.hasAnswered(false);
        self.nextBtnDisable(true);

        if (self.activeQuestion() == null) {
            self.nextButtonVisible(false);
        }

        if (self.typeId() == 2) {
            mathFormulaChecker(true);
        }
    }

    self.startAgain = function (item) {
        pageLoading(true);
        $.post(U + 'CSVideo/StartAgain?videoId=' + id,
                function (response) {

                })
            .done(function () {
                $.post(U + 'CSVideo/GetVideo',
                        { videoId: id },
                        function (response) {
                            initExercise(response.video);
                        })
                    .done(function () {
                        if (self.typeId() == 2) {
                            mathFormulaChecker(true);
                        }
                        pageLoading(false);
                    });
            });
    }

    self.moveToVideo = function (item) {
        location = item.url;
    }

    self.closePopup = function () {
        self.hasAnswered(false);
        self.markerClicked(false);
        $('.videoQuiz').hide();
        player.play();
    }
}

ko.applyBindings(new videoViewModel());

$(document).on('click', '.answers li', function (response) {
    $.post(U + 'Video/SetAnswer', { answerId: $(this).data('val') }, function (response) {
        if (response) {
            $('.vjs-marker').css('background', 'green !important')
        }
    }).done(function () {
        $('#mask, .question-container').hide();
        $('.answers').empty();
        player.play();
    });
});


//$(document).on('click', '#movingSlider li', function () {
//    var index = $('#movingSlider li').index($(this));
//    var markers = player.markers.getMarkers();
//    $('div[data-marker-key="' + markers[index].key + '"]').click();
//});