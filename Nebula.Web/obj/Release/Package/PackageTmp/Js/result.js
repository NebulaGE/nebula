(function () {
    var quizId = $('#result-id').val();
    pageLoading(true);
    function resultViewModel() {
        var self = this;
        self.inactiveSeconds = 0;


        // verbal
        self.verbalResultByTagName = [];
        self.verbalGeneralResult = {};
        self.verbalQuizTime = {};

        //math
        self.mathResultByTagName = [];
        self.mathGeneralResult = {};
        self.mathQuizTime = {};

        //all
        self.allResult = [];
        self.wholeTime = ko.observable();
    }

    function questionsByTagViewModel() {
        var self = this;
        self.tagName = '';
        self.numArray = [];
        self.questions = [];
        self.questionAnswers = [];
        self.answersL = ko.observableArray();
    }

    function myViewModel() {
        var self = this;
        self.choosenTag = ko.observable(3);
        self.fullName = ko.observable('');
        self.gameResult = ko.observable();
        self.quizTimes = ko.observableArray();
        self.startDate = ko.observable();
        self.shewoniliTagScores  = ko.observableArray();
        self.sumTagScores = ko.observableArray();
        self.quizIndex = ko.observable();
        self.activeText = ko.observable('სრული ანალიზი');
        self.returnUrl = ko.observable($('#return-url').val());
        self.errors = ko.observableArray();
        self.errorsVisible = ko.observable(false);
     

        self.getVerbalResult = function () {

            self.choosenTag(1);
            self.activeText('ვერბალური');
            $('.verbal').addClass('active').siblings().removeClass('active');
            $('.verbal-bl').siblings().hide();
            $('.verbal-bl').fadeIn();
        }

        self.getMathResult = function () {
            self.choosenTag(2);
            self.activeText('მათემატიკური');
            $('.math').addClass('active').siblings().removeClass('active');
            $('.math-bl').siblings().hide();
            $('.math-bl').fadeIn();
        }

        self.getAllResult = function () {
            self.choosenTag(3);
            self.activeText('სრული ანალიზი');
            $('.all-V-M').addClass('active').siblings().removeClass('active');
            $('.all-bl').siblings().hide();
            $('.all-bl').fadeIn();
        }


        function getQuizTime(seconds) {
            var hour = Math.floor(seconds / 3600);
            var minute = Math.floor(seconds / 60) - (hour * 60);
            if (hour.toString().length < 2) {
                hour = '0' + hour;
            }
            if (minute.toString().length < 2) {
                minute = '0' + minute;
            }
            return hour + ':' + minute;
        }


        function getTime(seconds) {
            var hour = Math.floor(seconds / 3600);
            var minute = Math.floor(seconds / 60) - (hour * 60);
            var second = Math.floor(seconds) - (hour * 60 * 60) - (minute * 60);
            var remain = returnTImeFormat(hour, 'საათი') + returnTImeFormat(minute, 'წუთი') + returnTImeFormat(second, 'წამი');
            return remain;
        }

        function returnTImeFormat(time, word) {

            if (time.toString().length < 2) {
                return '<span class="timer-in"> 0' + time + '   <span class="time-word">' + word + '</span></span>';
            }
            return '<span class="timer-in"> ' + time + '   <span class="time-word">' + word + '</span></span>';
        }




  



        self.getResult = function(response) {
    
            var model = new resultViewModel();

            model.inactiveSeconds = getTime(response.inactiveSeconds);
            model.verbalGeneralResult = response.verbal;
            $.each(response.verbalResultByTagResult, function (i, v) {
                model.verbalResultByTagName.push(v);
            });

            model.mathGeneralResult = response.math;
            $.each(response.mathResultByTagResult, function (i, v) {
                model.mathResultByTagName.push(v);
            });

            if (response.Err.length) {
                self.errors.push('სტატისტიკა არ იქნება გათვალისწინებული რადგან :');
                self.errorsVisible(true);
            }
           
            ko.utils.arrayForEach(response.Err, function (item) {
                self.errors.push(item + ';');
            });

          
            
            model.verbalQuizTime = response.verbalQuizTime >= 0 ? getQuizTime(response.verbalQuizTime) : 'დაუსრულებელია';
            model.mathQuizTime = response.mathQuizTime >= 0 ? getQuizTime(response.mathQuizTime) : 'დაუსრულებელია';

            response.verbalQuizTime = response.verbalQuizTime > 0 ? response.verbalQuizTime : 0;
            response.mathQuizTime = response.mathQuizTime > 0 ? response.mathQuizTime : 0;
            model.wholeTime = getQuizTime(response.verbalQuizTime + response.mathQuizTime);

            self.gameResult(model);

            //self.quizTimes(response.times);
            //self.shewoniliTagScores(response.shewoniliTagScores);
         
            //self.sumTagScores(response.sumTagScores);

            self.startDate(response.startDate);
            self.quizIndex(response.quizIndex);
            self.fullName(response.fullName);
       
            if (response.showPopup) {     
                var errorString = '<div style="text-align:center;">';
                if (response.errors) {
                    $.each(response.errors, function (i, v) {
                        errorString += '<p class="error-class">' + v + '</p>';
                    });
                    errorString += '</div>';

                }
                errorsAlert({ title: 'სტატისტიკა არ იქნება გათვალისწინებული, რადგან:', content: errorString });
            }

          

         
        }




    }

    function reviewViewModel() {

        // config variables

        var self = this;
        var numeric = 1;
        var hour = 0;
        var minute = 0;
        var second = 0;
        var remain = 0;
        var index = 0;
        self.checkIt = ko.observable(1);

        self.activeIndex = ko.observable(0);
        self.answersHelperCounter = ko.observable(0);
        self.verbalFinished = ko.observable(false);

        self.userAnswersCount = ko.observable();
        self.questions = ko.observableArray();
        self.activeQuestion = ko.observable();
        self.hasPreviousQuestion = ko.observable();
        self.hasNextQuestion = ko.observable();
        self.questionsByTag = ko.observableArray();
        self.quizId = ko.observable();
        self.quizTypeId = ko.observable();
        self.activeAnswer = ko.observable();
        self.remain = ko.observable();
        self.activeQuestionNum = ko.observable();
        self.startIndex = ko.observable(1);
        self.quizTypeText = ko.observable();
        self.isGameOver = ko.observable(false);
        self.pirobebi = [];
        self.activePiroba = ko.observable();
        self.questionCount = ko.observable(80);
     
        // init









        self.initQuiz = function (response) {
            console.log('quiz', response);
            if (response.showOnlyVerbal) {

                self.questionCount(40);
            }
              
           
            self.activeIndex(0);
            self.questions.removeAll();
            self.pirobebi = [];

            self.activeQuestion(null);
            self.hasPreviousQuestion(false);
            self.hasNextQuestion(true);
            self.questionsByTag.removeAll();
            self.userAnswersCount(0);

            self.quizId(0);
            self.activeAnswer(-1);
            self.remain();
            self.startIndex(1);
            var numeric = 1;
            var index = 0;
            var startIndex = 1;

            var tagIndex = 0;
            ko.utils.arrayForEach(response.quiz, function (item) {
                var model = new questionsByTagViewModel();
                model.tagName = item.tagName;
                model.hasMultipleQuestions = item.hasMultipleQuestions;
                model.pirobaIndexer = 'p' + item.questions[0].id;
                if (item.hasMultipleQuestions) {
                    self.pirobebi[tagIndex] = { piroba: item.TaskText, questId: item.questions[0].id, pirobaIndexer: 'p' + item.questions[0].id, tagName: item.tagName }
                    //self.pirobebi.splice(tagIndex, 0, { });
                }

                ko.utils.arrayForEach(item.questions, function (questionItem) {
                    var plainText = $.parseHTML(questionItem.QuestionText);
                    var txt = $(plainText).text();

                    if ($(plainText).find('.math-tex').length) {
                        txt = model.tagName;
                    }
                    if (model.tagName == 'რაოდენობრივი შედარება') {
                        txt = model.tagName;
                    }


                    model.numArray.push({ num: index + startIndex, index: index });
                    var quest = {};
                    quest.id = questionItem.id;
                    quest.QuestionText = questionItem.QuestionText;
                    quest.hasMultipleQuestions = questionItem.hasMultipleQuestions;
                    quest.multipleQuestionsText = questionItem.multipleQuestionsText;
                    quest.plainText = txt;
                    quest.answers = [];
                    quest.answerL = ko.observable('');
                    quest.answerI = ko.observable(-1);
                    quest.moveAnswerI = ko.observable(-1);
                    quest.num = startIndex;
                    quest.index = index;
                    quest.isCorrect = ko.observable(false);
                    $.each(questionItem.answers, function (i, v) {
                        quest.answers.push({ id: v.id, answerString: v.answerString, L: numletterHelper(i), index: i, isCorrect: v.isCorrect })
                    });
                    model.questions.push(quest);
                    self.questions.push(quest);
                    startIndex++;
                    index++;
                });
                self.questionsByTag.push(model);

                // numeric = 1;
                tagIndex++;
            });

            //init user answers

            ko.utils.arrayForEach(response.userAnswers, function (item) {


                var quest = [];
                $.each(self.questionsByTag(), function (i, v) {
                    quest = $.grep(v.questions, function (question) {
                        return question.id == item.questionId;
                    });

                    if (quest.length) {
                        $.each(quest[0].answers, function (ansIndex, ansValue) {

                            if (item.answerId != null && item.answerId != 0) {

                                if (ansValue.id == item.answerId) {

                                    quest[0].answerL(numletterHelper(ansIndex));
                                    quest[0].answerI(ansIndex);
                                    quest[0].isCorrect(item.isCorrect);
                                  
                                    self.userAnswersCount(self.userAnswersCount() + 1);
                                    //     self.activeAnswer(quest[0].answerI());

                                }
                            }
                            if (item.moveAnswerId != null && item.moveAnswerId != 0) {

                                if (ansValue.id == item.moveAnswerId) {
                                    quest[0].moveAnswerI(ansIndex);
                                }
                            }

                        });

                    }

                });

            });



            //init first question and answer
            self.activeQuestion(self.questionsByTag()[0].questions[0]);
            checkIfMathFormulaExist();
            if (self.questionsByTag()[0].questions[0].answerI() != -1) {
                self.activeAnswer(self.questionsByTag()[0].questions[0].answerI());
            } else {
                self.activeAnswer(-1);
            }
            self.remain(Math.floor(response.remainingSeconds));
            var remainSecs = Math.floor(response.remainingSeconds);
            pageLoading(false);
        }

        function initActiveAnswer(questionId) {
            $.each(self.questionsByTag(), function (i, v) {
                quest = $.grep(v.questions, function (question) {
                    return question.id == questionId;
                });
                if (quest.length) {
                 
                    if (quest[0].answerI() != -1) {
                        self.activeAnswer(quest[0].answerI());
                        
                    } else {
                        self.activeAnswer(-1);
                    }
                    return false;
                } else {
                    self.activeAnswer(-1);
                }
            });
        }







        function showPiroba(bool) {

            if (bool) {
                $('.question-nav').hide();
                $('.piroba-nav').show();
            } else {
                $('.piroba-nav').hide();
                $('.question-nav').show();
            }
        }

        function animateNextAndPrevBtn() {
            var allAnswersArr = $('.testsAswers ol li'),
              indexOfActive = 0;

            for (var i = 0, length = allAnswersArr.length; i < length; i++) {
                if ($(allAnswersArr[i]).hasClass('active')) {
                    indexOfActive = i;
                    break;
                }
            }
            if (allAnswersArr[indexOfActive]) {
                $('.testLeftNav ol li.active').removeClass('active');
                $(allAnswersArr[indexOfActive]).addClass('active');
                $('.fullHeight').animate({ scrollTop: allAnswersArr[indexOfActive].offsetTop }, 500);
            }
        }

        checkIfMathFormulaExist = function () {
     
                if ($('#math-jax-area').find('.math-tex').length) {
                    MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
                }
            
        }



        //operations
        self.checkIfNextOrPrevExist = ko.computed(function () {
            if (self.activeIndex() == 0) {
                self.hasPreviousQuestion(false)
            } else {
                self.hasPreviousQuestion(true)
            }
            if (self.activeIndex() == 39) {
                self.hasNextQuestion(false);
            } else {
                self.hasNextQuestion(true);
            }
        });



        self.initUserAnswersCount = function () {
            userAnswers = $.grep(self.questions(), function (question) {
                return question.answerI() != -1;
            });
            self.userAnswersCount(userAnswers.length)
        }



        self.getQuestionByNum = function (item) {
            showPiroba(false);
            self.activeIndex(item.index);
            self.activeQuestion(self.questions()[item.index]);
            initActiveAnswer(item.questionId);

            checkIfMathFormulaExist();
        

        }

        self.getNextQuestion = function (item) {

            var comingPiroba = false;
            if (!self.activeQuestion().hasMultipleQuestions) {
                comingPiroba = true;

            }

            item.index++;

            if (self.questions()[item.index].hasMultipleQuestions && comingPiroba) {
                var piroba = $.grep(self.pirobebi, function (p) {
                    if (p) {
                        return p.questId == self.questions()[item.index].id;
                    }

                });

                self.activePiroba(piroba[0]);
                self.activeIndex('p' + self.questions()[item.index].id);
                animateNextAndPrevBtn();
                showPiroba(true);
                return false;
            }

            self.activeIndex(item.index);
            self.activeQuestion(self.questions()[item.index]);
            initActiveAnswer(self.questions()[item.index].id);
            animateNextAndPrevBtn();
            checkIfMathFormulaExist();
        }

        self.getPrevQuestion = function (item) {
            item.index--;
            self.activeIndex(item.index);
            self.activeQuestion(self.questions()[item.index]);
            initActiveAnswer(self.questions()[item.index].id);
            animateNextAndPrevBtn();
            checkIfMathFormulaExist();
        }

        self.getPiroba = function (item) {
           
            self.activePiroba(self.pirobebi[item.pirobaIndex]);
        
            showPiroba(true);
        }

        self.showPirobaPop = function () {
            $('.quisp').show();
        }


    }

    var reviewModel = new reviewViewModel();
    var resultModel = new myViewModel();
    $.post(U + 'Exam/GetResult?quizId=' + quizId, function (response) {    
        if (response.error) {
            var errorString = '<div style="text-align:center;">';                        
                errorString += '<p class="error-class">' + response.error + '</p>';             
                errorString += '</div>';
                errorsAlert({ title: 'სისტემური შეცდომა', content: errorString });
                return false;                  
        } else {
            resultModel.getResult(response);
            reviewModel.initQuiz(response.review);
        }
     

    }).done(function () {

        pageLoading(false);
     
         
     
    });

    ko.applyBindings(resultModel, document.getElementById("quiz-result"));
    ko.applyBindings(reviewModel, document.getElementById("quiz-review"));

    function showReview() {
        $('#quiz-result').hide();
        $('#quiz-review').show();
    }
    function showResult() {
        $('#quiz-review').hide();
        $('#quiz-result').show();
      
    }

    $('#show-result').on('click', function () {
    
        showResult();
    });
    $('.show-review').on('click', function () {
       
        showReview();
    });
    $('#show-times').on('click', function () {

        $.alert({
            title: 'თითოეულ კითხვაზე დახარჯული დრო',
            content: $('#quiz-time').html(),
            confirmButton: 'დახურვა',
            columnClass: 'col-md-4 col-md-offset-4'
        });
    });
    $('#show-algorithm').on('click', function () {

        $.alert({
            title: 'ალგორითმი',
            content: $('#quiz-algorithm').html(),
            confirmButton: 'დახურვა',
            columnClass: 'col-md-8 col-md-offset-2'
        });
    });

    $('.clossepop').on('click', function () {
        $('.quisp').hide();
        $('body').css('overflow', 'scroll');
    });
})();


