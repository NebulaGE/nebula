(function () {


    pageLoading(true);
    function questionsByTagViewModel() {
        var self = this;
        self.tagName = '';
        self.numArray = [];
        self.questions = [];
        self.questionAnswers = [];
        self.answersL = ko.observableArray();
    }



    function ExamViewModel() {

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
        self.quizIndex = ko.observable();
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
        self.showPiroba = ko.observable(false);

        //init data
        $.post(U + 'Exam/InitQuiz', function (response) {

            if (response.error) {
                if (response.error == 'timeout') {
                    location = U + 'Exam/Result?quizId=' + response.quizId + '&returnUrl=' + U + 'All/Index';;
                } else {
                    alert(response.error);
                }
            } else {
                initQuiz(response);
            }
           

        }).done(function () {
            if (self.quizTypeId() == 2) {
                //if ($('#math-jax-area').find('.math-tex').length) {
                //    MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
                //}

            }
			       $('.fullHeight').scrollTop(0);
        });

        window.onbeforeunload = confirmExit;
        function confirmExit() {
            $.post(U + 'Exam/SetUserLastActive', { quizId: self.quizId() }, function (r) {
            });
        }

        //helper functions
        function returnTImeFormat(time, word) {

            if (time.toString().length < 2) {
                return '<span class="timer-in"> 0' + time + '   <span class="time-word">' + word + '</span></span>';
            }
            return '<span class="timer-in"> ' + time + '   <span class="time-word">' + word + '</span></span>';
        }


        function initQuiz(response) {
            console.log('response', response);
     
            self.quizIndex(response.quizIndex);
            self.activeIndex(0);
            self.questions.removeAll();
            self.pirobebi = [];
            self.showPiroba(false);
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
            self.quizId(response.quizId);
            self.quizTypeId(response.quizTypeId);
            if (response.quizTypeId == 1) {
                startIndex = 1;
                self.startIndex(startIndex);
                self.quizTypeText('ვერბალური');
            } else if (response.quizTypeId == 2) {
                startIndex = 41;
                self.startIndex(startIndex);
                self.quizTypeText('მათემატიკური');
            }
            var tagIndex = 0;
            ko.utils.arrayForEach(response.quiz, function (item) {
              
                var model = new questionsByTagViewModel();
                model.tagName = item.tagName;
                model.hasMultipleQuestions = item.hasMultipleQuestions;
                model.pirobaIndexer = 'p' + item.questions[0].id;
                if (item.hasMultipleQuestions) {
                 
                    var pirobaQuestionNum = startIndex;
                    var pirobaQuestionIndex = index;
                    self.pirobebi[tagIndex] = {
                        pirobaId : item.questions[0].pirobaId,
                        piroba: item.TaskText,
                        questId: item.questions[0].id,
                        pirobaIndexer: 'p' + item.questions[0].id,
                        tagName: item.tagName,
                        questions : []
                    }

                    ko.utils.arrayForEach(item.questions, function (pirobaQuestion) {
                      
                        self.pirobebi[tagIndex].questions.push({
                            id: pirobaQuestion.id,
                            num: pirobaQuestionNum,
                            index : pirobaQuestionIndex
                        });

                        pirobaQuestionNum++;
                        pirobaQuestionIndex++;
                    });
                  
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
                    quest.pirobaId = questionItem.pirobaId;
                    quest.QuestionText = questionItem.QuestionText;
                    quest.hasMultipleQuestions = questionItem.hasMultipleQuestions;
                    quest.multipleQuestionsText = questionItem.multipleQuestionsText;
                 
                    quest.plainText = txt;
                   
                 
                    quest.answers = [];
                    quest.answerL = ko.observable('?');
                    quest.answerI = ko.observable(-1);
                    quest.moveAnswerI = ko.observable(-1);
                    quest.num = startIndex;
                    quest.index = index;
                    $.each(questionItem.answers, function (i, v) {
                        //isCorrect: v.isCorrect
                        quest.answers.push({ id: v.id, answerString: v.answerString, L: numletterHelper(i), index: i})
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
            console.log('questions', self.questions());
            //init user answers
            console.log('user answers', response.userAnswers);
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
            self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });

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
                    console.log('I', quest[0].answerI());
                    if (quest[0].answerI() != -1) {
                        self.activeAnswer(quest[0].answerI());
                        console.log('self', self.activeAnswer());
                    } else {
                        self.activeAnswer(-1);
                    }
                    return false;
                } else {
                    self.activeAnswer(-1);
                }
            });
        }
       

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

      

        function showPiroba(bool) {
   
        if (bool) {
          
                $('.question-nav').hide();
                $('.piroba-nav').show();
            } else {
                $('.piroba-nav').hide();
                $('.question-nav').show();           
            }
        }

        function animateNextAndPrevBtn (){
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
            if (self.quizTypeId() == 2) {
                if ($('#math-jax-area').find('.math-tex').length) {
                    MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
                }
            }
        }

        function getTime(seconds) {
            var hour = Math.floor(seconds / 3600);
            var minute = Math.floor(seconds / 60) - (hour * 60);
            var second = Math.floor(seconds) - (hour * 60 * 60) - (minute * 60);
            var remain = returnTImeFormat(hour, 'საათი') + returnTImeFormat(minute, 'წუთი') + returnTImeFormat(second, 'წამი');
            return remain;
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
         
            self.showPiroba(false);
            //self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId : self.quizId() });
            self.activeIndex(item.index);
            self.activeQuestion(self.questions()[item.index]);
            console.log('act', self.activeQuestion());
            animateNextAndPrevBtn();
            initActiveAnswer(item.questionId);
      
            checkIfMathFormulaExist();

            self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
        }

        self.getNextQuestion = function (item) {
            //self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
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
             self.showPiroba(true);
             self.activeQuestion(self.questions()[item.index]);
             console.log('quest', self.activeQuestion());
             self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
           
             return false;
            }

            self.activeIndex(item.index);
            self.activeQuestion(self.questions()[item.index]);
            initActiveAnswer(self.questions()[item.index].id);
            animateNextAndPrevBtn();
           
            checkIfMathFormulaExist();
            self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
        }

        self.getPrevQuestion = function (item) {
            //self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
            item.index--;
            self.activeIndex(item.index);
            self.activeQuestion(self.questions()[item.index]);
            initActiveAnswer(self.questions()[item.index].id);
            animateNextAndPrevBtn();
            checkIfMathFormulaExist();
            self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
        }

        self.getPiroba = function (item) {
         
            //self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
            self.activePiroba(self.pirobebi[item.pirobaIndex]);
            self.activeIndex(self.activePiroba().pirobaIndexer);
            
            self.showPiroba(true);
  
            animateNextAndPrevBtn();
           
            var quest = $.grep(self.questions(), function (q) {
                return q.id == self.activePiroba().questId;
            });
            self.activeQuestion(quest[0]);
            console.log('active quest', quest[0]);
            self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
      
        }

        self.getPirobaFromQuestion = function (item) {
           
            console.log('item', self.pirobebi);
           // self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
            var piroba = $.grep(self.pirobebi, function (v) {
                if (v) {
                    return item.pirobaId == v.pirobaId;
                }
               
            });
            
            self.activePiroba(piroba[0]);
            self.activeIndex(self.activePiroba().pirobaIndexer);

            self.showPiroba(true);
            animateNextAndPrevBtn();

        

            self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });

        }
        self.setAnswer = function (item) {
 

            $.post(U + 'Exam/SetAnswer', { answerId: item.answerId, quizId: item.quizId, questionId: item.questionId, quizTypeId: self.quizTypeId() }, function (response) {
                console.log(response);
                if (response.error) {                              
                    errorsAlert({ title: 'შეცდომა', content: response.error });
                    return false;
                } else if (response.success) {
                    var quest = '';
                    var L = '';
                    var I = item.index;
                    $.each(self.questionsByTag(), function (i, v) {
                        quest = $.grep(v.questions, function (question) {
                            return question.id == item.questionId;
                        });


                        if (quest.length) {
                            L = numletterHelper(item.index);
                            quest[0].answerL(L);
                            quest[0].answerI(I);
                            self.activeAnswer(quest[0].answerI());
                            return false;
                        }
                    });
                    self.initUserAnswersCount();
                }
            }).done(function () {

            }).error(function () {
                errorsAlert({ title: 'შეცდომა', content: 'გთხოვთ შეამოწმოთ ინტერნეტის კავშირი' });
            });
        }

        self.fillAnswers = function (item) {
            item.question.moveAnswerI(item.answer.index);
            $.post(U + 'Exam/MoveAnswer', { answerId: item.answer.id, quizId: self.quizId(), questionId: item.question.id, quizTypeId: self.quizTypeId() },
                function (response) {
                    if (response.error) {
                        item.question.moveAnswerI(-1);
                        errorsAlert({ title: 'შეცდომა', content: response.error });
                    }
                    console.log(response);
                }).done(function () {

                });
        }

        self.gameOver = function () {

           // self.upadateQuestionTime({ questionId: self.activeQuestion().id, quizTypeId: self.quizTypeId(), quizId: self.quizId() });
            self.isGameOver(true);
            pageLoading(true);

            $.post(U + 'Exam/GameOver', { quizId: self.quizId(), quizTypeId: self.quizTypeId() }, function (response) {
            }).done(function () {
                if (self.quizTypeId() == 2) {
                    location = U + 'Exam/Result?quizId=' + self.quizId() + '&returnUrl=' + U + 'All/Index';
              
                } else {
                    $.post(U + 'Exam/InitQuiz', function (response) {

                        initQuiz(response);
                    }).done(function(){
						setTimeout(function(){
							$('.fullHeight').scrollTop(0);
						},100)
						
					});
            
                }
            });

        }

        self.confirmGameOver = function () {
            var content = '';
            if (self.quizTypeId() == 1) {
                content = 'ნამდვილად გსურთ ვებრალურის დასრულება და მათემატიკური ნაწილის დაწყება?'
            } else if (self.quizTypeId() == 2) {
                content = 'ნამდვილად გსურთ ტესტირების დასრულება?'
            }
            $.confirm({
                title: 'ტესტის დასრულება',
                content: content,
                confirm: function () {
                    self.gameOver();
                },
                cancel: function () {
                  //  $.alert('Canceled!')
                },
                confirmButton: 'კი',
                cancelButton: 'არა'
            });
        }

        self.upadateQuestionTime = function (item) {
            $.post(U + 'Exam/UpdateQuestionTime', { questionId: item.questionId, quizTypeId: item.quizTypeId, quizId: item.quizId });
        }
       
        self.fromPirobaToQuestions = function () {
      
            self.activeIndex(self.questions.indexOf(self.activeQuestion()));
            showPiroba(false);
            animateNextAndPrevBtn();
          
        }


        self.showPirobaPop = function () {
            $('.quisp').show();
        }

        self.answerCounterHelper = function () { 
            return self.answersHelperCounter(self.answersHelperCounter() + 1);
        }
        //timer

        var timer = setInterval(function () {
            hour = Math.floor(self.remain() / 3600);
            minute = Math.floor(self.remain() / 60) - (hour * 60);
            second = Math.floor(self.remain()) - (hour * 60 * 60) - (minute * 60);
            hour = hour < 10 ? "0" + hour : hour;
            minute = minute < 10 ? "0" + minute : minute;
            second = second < 10 ? "0" + second : second;
            var remain = returnTImeFormat(hour, 'საათი') + returnTImeFormat(minute, 'წუთი') + returnTImeFormat(second, 'წამი');
            self.remain(self.remain() - 1);
            if (self.remain() == 600) {
                errorsAlert({ title: 'გაფრთხილებთ!', content: '10 წუთი დაგრჩათ' });
            }
             
            if (self.remain() == 0) {
                self.gameOver();

                //  remain = returnTImeFormat("00", 'საათი') + returnTImeFormat("00", 'წუთი') + returnTImeFormat("01", 'წამი');

                //errorsAlert({ title: 'გაფრთხილებთ!', content: 'დრო გავიდა, 1 წუთში ავტომატურად დასრულდება' });
                //self.remain(60);

        

                //if (self.quizTypeId() == 1) {
                //    setTimeout(function () {        
                //        if (self.quizTypeId() != 2) {                
                //            self.gameOver();
                //        }
                //    }, 60000);
                //}


                //if (self.quizTypeId() == 2) {
                //    setTimeout(function () {
                //        self.gameOver();
                //    }, 60000);
                //}

            }

            $('.timer').html(remain);
        }, 1000);

        


        //set user last active
        var userActiveInterval = setInterval(function () {
            $.post(U + 'Exam/SetUserLastActive', { quizId: self.quizId() }, function (r) {
            });
        }, 300000)

     

    }

    $('.clock').hover(function () {
        $('.timer').stop(true, true).fadeIn();
    }, function () {
        $('.timer').stop(true, true).fadeOut();
    });



    //$('#ansbtn').on('click', function () {
    //    if ($('.move-answers').is(':visible')) {
    //        $('.move-answers').hide();
    //        $('#ansbtn').text('პასუხების გადატანა');
    //    } else {
    //        $('.move-answers').css('margin-left', $('.fullHeight').width() + 'px').show();
    //        $('#ansbtn').text('ფურცლის დამალვა');
    //    }
    //});
    //confirm functions
    //verbal

    $('.clossepop').on('click', function () {
        $('.quisp').hide();
        $('body').css('overflow', 'scroll');
    });

    ko.applyBindings(new ExamViewModel());
})();
