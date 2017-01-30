
pageLoading(true);
function userTagViewModel(){
    var self = this; 
}

function QuesionViewModel() {
    var self = this;
    self.QuestionText = '';
    self.id = '';
    self.answers = [];
    self.answerd = ko.observableArray();
}
        
function PirobaViewModel() {
    var self = this;
    self.id = '';
    self.text = '';
    self.typeName = '';
    self.questions = [];
    self.userAnswers = []; 
}
 
function ExerciseViewModel() {
           
    var self = this;
    //var L = {};
    var progressBarLength = 10;
    self.questions = ko.observableArray();
    self.activeQuestion = ko.observable();
    self.activeIndex = ko.observable(0);
    self.progressBar = ko.observableArray();
    self.correctAnswer = ko.observable();
    self.isEmpty = ko.observable(false);
    self.nextButtonVisible = ko.observable(true);
    self.newTestVisible = ko.observable(false);
    self.nextBtnDisable = ko.observable(true);
    self.type = ko.observable('');
    self.exercises = ko.observableArray();
    self.ModuleId = ko.observable(window.location.hash.substr(1));
    self.userAnswers = ko.observableArray();
    self.currentQuestionBtnVisible = ko.observable(false);
    self.resetBtn = ko.observable(false);

    self.selectedAnswerId = ko.observable(0);
    self.incorrectAsnwerId = ko.observable();
    self.correctAnswerId = ko.observable();
    self.hasAnswered = ko.observable(false);
    self.historyNavigate = ko.observable(true);
    //piroba
    self.piroba = ko.observable();
    self.isPiroba = ko.observable(false);
    self.hasNextQuestion = ko.observable(true);
    self.hasPreviousQuestion = ko.observable(false);
    self.startNewPiroba = ko.observable(false);

    $.getJSON(U + 'Exercise/GeneretePirobaOrQuestions',
        { ModuleId: self.ModuleId() },
        function(response) {

            if (response.isPiroba) {
                self.isPiroba(true);
                $.post(U + 'Exercise/InitExercisePirobaQuestions',
                        { ModuleId: self.ModuleId() },
                        function(response) {
                            self.initExercises(response);
                            if (response.error) {

                                pageLoading(false);
                                errorsAlert({ title: '', content: response.error });
                                return false;
                            }
                            if (response.isEmpty) {
                                self.isEmpty(true);
                                pageLoading(false);
                                self.resetBtn(true);
                                //  errorsAlert({ title: '', content: 'კითხვების ლიმიტი ამოიწურა' });
                                $('#exercise-ld').fadeIn();
                                return false;
                            } else {
                                self.initPiroba(response);
                            }

                        })
                    .done(function() {
                        pageLoading(false);
                        $('#exercise-ld').fadeIn();
                        if ($('#math-jax-area').find('.math-tex').length) {
                            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
                        }
                    });


            } else {
                self.isPiroba(false);
                $.post(U + 'Exercise/IntiExerciesQuestions',
                        { ModuleId: self.ModuleId() },
                        function(response) {
                            console.log('res', response);
                            self.initExercises(response);
                            if (response.error) {
                                errorsAlert({ title: '', content: response.error });
                                pageLoading(false);
                                return false;
                            }

                            self.initQuestions(response);
                        })
                    .done(function() {
                        pageLoading(false);
                        if ($('#math-jax-area').find('.math-tex').length) {
                            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
                        }
                        $('#exercise-ld').fadeIn();
                    });
            }

        });
     
    //behaiviour
    self.setAnswer = function(item) {
        console.log('piroba', self.piroba());
        self.historyNavigate(false);
        self.hasAnswered(true);
        //   console.log(item);
        self.selectedAnswerId(item.answerId);
        var pirobaId = null;
        if (self.isPiroba()) {
            pirobaId = self.piroba().id;
        }

        $.post(U + 'Exercise/setanswer',
            {
                questionId: self.activeQuestion().id,
                answerId: item.answerId,
                ModuleId: self.ModuleId(),
                pirobaId: pirobaId
            },
            function(response) {
                var incorrectAnswerId = -1;
                var correctAnswer = $.grep(self.activeQuestion().answers,
                    function(answer) {
                        return answer.id == response.correctAnswerId;
                    });

                var exerciseGroup = $.grep(self.exercises(),
                    function(item) {
                        return item.ModuleId == self.ModuleId();
                    });

                exerciseGroup[0].userAnswersCount(exerciseGroup[0].userAnswersCount() + 1);
                self.correctAnswer(correctAnswer[0]);

                if (correctAnswer[0].id == item.answerId) {
                    self.correctAnswerId(correctAnswer[0].id);
                    self.progressBar()[self.activeIndex()]('true');
                } else {
                    self.correctAnswerId(correctAnswer[0].id);
                    self.incorrectAsnwerId(item.answerId);
                    incorrectAnswerId = item.answerId;
                    self.progressBar()[self.activeIndex()]('false');
                }
                if (!self.isPiroba()) {
                    self.userAnswers.push({
                        answerId: item.answerId,
                        questionId: self.activeQuestion().id,
                        isCorrect: incorrectAnswerId == -1 ? true : false,
                        correctAnswerId: response.correctAnswerId
                    });
                }

                if (self.isPiroba()) {
                    self.piroba()
                        .userAnswers.push({
                            questionId: self.activeQuestion().id,
                            selectedAnswerId: item.answerId,
                            correctAnswerId: correctAnswer[0].id,
                            incorrectAnswerId: incorrectAnswerId
                        });
                    if (self.progressBar().length == self.piroba().userAnswers.length) {
                        self.startNewPiroba(true);
                    }
                }

                if (!self.isPiroba()) {
                    if (self.questions().length < 10) {
                        if (self.questions().length == self.userAnswers().length) {

                            self.resetBtn(true);
                        }
                    }

                    if (10 == self.activeIndex() + 1) {
                        self.nextButtonVisible(false);
                        self.newTestVisible(true);
                    }
                    self.nextBtnDisable(false);
                    //  $('.current-answer').fadeIn();
                }
                self.historyNavigate(true);
            });
    }

    self.getNextQuestion = function() { 
        self.activeIndex(self.activeIndex() + 1);
        self.activeQuestion(self.questions()[self.activeIndex()]);

        self.hasAnswered(false);
        self.nextBtnDisable(true);

        if (self.activeQuestion() == null) {
            self.nextButtonVisible(false);
        }

        if ($('#math-jax-area').find('.math-tex').length) {

            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
        }
    };

    self.getPirobaQuestion = function(item) {
        self.activeQuestion(self.piroba().questions[item.index]);
        if ($('#math-jax-area').find('.math-tex').length) {
            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
        }
        var userAnswer = $.grep(self.piroba().userAnswers,
            function(v) {
                return v.questionId == self.activeQuestion().id;
            });

        if (userAnswer[0]) {
            self.hasAnswered(true);
            self.correctAnswerId(userAnswer[0].correctAnswerId);
            self.incorrectAsnwerId(userAnswer[0].incorrectAnswerId);
            self.selectedAnswerId(userAnswer[0].selectedAnswerId)
        } else {
            self.hasAnswered(false);
        }

        self.activeIndex(item.index);
        showQuestion();

        self.checkIfNextOrPrevExist();
    };

    self.getPirobaNextQuestion = function(item) {
        item.index++;
        self.activeQuestion(self.piroba().questions[item.index]);
        var userAnswer = $.grep(self.piroba().userAnswers,
            function(v) {
                return v.questionId == self.activeQuestion().id;
            });

        if (userAnswer[0]) {
            self.hasAnswered(true);
            self.correctAnswerId(userAnswer[0].correctAnswerId);
            self.incorrectAsnwerId(userAnswer[0].incorrectAnswerId);
            self.selectedAnswerId(userAnswer[0].selectedAnswerId)
        } else {
            self.hasAnswered(false);
        }
        self.activeIndex(item.index);
        showQuestion();

        self.checkIfNextOrPrevExist();
        checkIfMathFormulaExist();
    };

    self.getPirobaPrevQuestion = function(item) {
        item.index--;
        self.activeQuestion(self.piroba().questions[item.index]);
        var userAnswer = $.grep(self.piroba().userAnswers,
            function(v) {
                return v.questionId == self.activeQuestion().id;
            });

        if (userAnswer[0]) {
            self.hasAnswered(true);
            self.correctAnswerId(userAnswer[0].correctAnswerId);
            self.incorrectAsnwerId(userAnswer[0].incorrectAnswerId);
            self.selectedAnswerId(userAnswer[0].selectedAnswerId);
        } else {
            self.hasAnswered(false);
        }
        self.activeIndex(item.index);
        showQuestion();

        self.checkIfNextOrPrevExist();
        checkIfMathFormulaExist();
    };

    self.checkIfNextOrPrevExist = function() {
        if (self.activeIndex() == 0) {
            self.hasPreviousQuestion(false);
        } else {
            self.hasPreviousQuestion(true);
        }
        if (self.activeIndex() == self.piroba().questions.length - 1) {
            self.hasNextQuestion(false);
        } else {
            self.hasNextQuestion(true);
        }
    };

    self.initQuestions = function(response) { 
        showQuestion();
        self.nextButtonVisible(true);
        self.newTestVisible(false);
        self.nextBtnDisable(true);
        self.currentQuestionBtnVisible(false);
        self.progressBar.removeAll();
        self.questions.removeAll();
        self.userAnswers.removeAll();
        self.activeIndex(0);
        self.resetBtn(false);

        ko.utils.arrayForEach(response.questions,
            function(item) {
                var model = new QuesionViewModel();
                model.QuestionText = item.QuestionText;
                model.id = item.id;

                $.each(item.answers,
                    function(i, v) {
                        model.answers.push({ id: v.id, answerString: v.answerString, L: numletterHelper(i) });
                    });

                self.questions.push(model);
            });

        if (response.questions.length < 10) {
            if (response.questions.length == 0) {
                self.resetBtn(true);
            }
            if (response.userAnswers) {
                if (response.userAnswers.length == response.questions.length) {
                    self.resetBtn(true);
                }
            }
        }

        for (i = 0; i < progressBarLength; i++) {
            self.progressBar.push(ko.observable('default'));
        }

        self.activeQuestion(self.questions()[response.startIndex]);

        if (self.activeQuestion() == null) {
            self.nextButtonVisible(false);
        }
        console.log('comes here');

        if (response.userAnswers) {
            $.each(response.userAnswers,
                function(i, v) {
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
    };

    self.initPiroba = function(response) {
        showPiroba();

        self.piroba(null);
        self.startNewPiroba(false);
        self.resetBtn(false);
        self.progressBar.removeAll();

        var model = new PirobaViewModel();
        model.text = response.piroba.text;
        model.id = response.piroba.id;
        model.typeName = response.typeName;
        ko.utils.arrayForEach(response.piroba.questions,
            function(item) {
                console.log('item', item);
                var questionViewModel = new QuesionViewModel();
                questionViewModel.QuestionText = item.text;
                questionViewModel.id = item.id;

                $.each(item.answers,
                    function(i, v) {

                        questionViewModel.answers.push({ id: v.id, answerString: v.text, L: numletterHelper(i) });
                    });

                model.questions.push(questionViewModel);
            });
        self.piroba(model);

        for (i = 0; i < response.piroba.questions.length; i++) {
            self.progressBar.push(ko.observable('default'));
        }

        self.activeQuestion(self.piroba().questions[0]);
         
        if (response.piroba.userAnswers) {
            if (response.piroba.userAnswers.length == response.piroba.questions.length) {
                self.startNewPiroba(true);
            }
        }

        $.each(response.piroba.userAnswers,
            function(i, v) {
                self.piroba()
                    .userAnswers.push({
                        questionId: v.questionId,
                        selectedAnswerId: v.answerId,
                        correctAnswerId: v.correctAnswerId,
                        incorrectAnswerId: v.isCorrect ? -1 : v.answerId
                    });
                var quest = $.grep(self.piroba().questions,
                    function(item) {
                        return v.questionId == item.id;
                    });

                if (v.isCorrect) {
                    self.progressBar()[self.piroba().questions.indexOf(quest[0])]('true');
                } else {
                    self.progressBar()[self.piroba().questions.indexOf(quest[0])]('false');
                }

            });
        console.log('piroba answers', self.piroba().userAnswers);
    };

    self.goBackToPiroba = function () {
        showPiroba();
    }
  
    self.changeHash = function (item) {
        location.hash = item.ModuleId;
    }

    self.initExercises = function (response) {
        self.exercises.removeAll();
        self.type(response.type);
        ko.utils.arrayForEach(response.exercises, function (item) {
            self.exercises.push({ name: item.name, url: '#' + item.id, userAnswersCount: ko.observable(item.userAnswerCount), questionCount: item.questionCount, ModuleId: item.id })
        });
    }

    self.reset = function() {

        pageLoading(true);
        $.post(U + 'Exercise/Reset',
            { moduleId: self.ModuleId() },
            function(resp) {
                if (resp.success) {
                    self.newTestVisible(false);
                    $('.current-answer').hide();
                    self.hasAnswered(false);
                    $.getJSON(U + 'Exercise/GeneretePirobaOrQuestions',
                        { ModuleId: self.ModuleId() },
                        function(response) { 
                            if (response.isPiroba) {
                                self.isPiroba(true);
                                $.post(U + 'Exercise/InitExercisePirobaQuestions',
                                        { ModuleId: self.ModuleId() },
                                        function(response) {
                                            self.initExercises(response);
                                            if (response.error) {
                                                errorsAlert({ title: '', content: response.error });
                                                pageLoading(false);
                                                return false;
                                            }
                                            if (response.isEmpty) {
                                                pageLoading(false);
                                                self.isEmpty(true);
                                                self.activeQuestion(null);
                                                errorsAlert({ title: '', content: 'კითხვების ლიმიტი ამოიწურა' });
                                                $('#exercise-ld').fadeIn();
                                                return false;
                                            }
                                            self.initPiroba(response);
                                        })
                                    .done(function() {
                                        pageLoading(false);
                                    });
                            } else {
                                self.isPiroba(false);
                                $.post(U + 'Exercise/IntiExerciesQuestions',
                                        { ModuleId: self.ModuleId() },
                                        function(response) {
                                            self.initExercises(response);
                                            self.initQuestions(response);
                                        })
                                    .done(function() {
                                        pageLoading(false);
                                        if ($('#math-jax-area').find('.math-tex').length) {
                                            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
                                        };
                                    });
                            };
                        });
                };
            }); 
    };

    self.newTest = function () {
        $('.current-answer').hide();
        self.hasAnswered(false);
        pageLoading(true);
        $.post(U + 'Exercise/IntiExerciesQuestions', { ModuleId: self.ModuleId() }, function (response) {
            self.initQuestions(response);
        }).done(function () {
            if ($('#math-jax-area').find('.math-tex').length) {
                MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
            }
            pageLoading(false);
        });
    }

    self.newPiroba = function () {
        self.hasAnswered(false);
        pageLoading(true);
        $.post(U + 'Exercise/InitExercisePirobaQuestions', { ModuleId: self.ModuleId() }, function (response) {
            if (response.isEmpty) {
                pageLoading(false);
                self.isEmpty(true);
                self.activeQuestion(null);
                // errorsAlert({ title: '', content: 'კითხვების ლიმიტი ამოიწურა' });
                self.resetBtn(true);
                $('#exercise-ld').fadeIn();
                return false;
            }
            self.initPiroba(response);
        }).done(function () {
            if ($('#math-jax-area').find('.math-tex').length) {
                MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
            }
            pageLoading(false);
        });
    }

    self.getQuestionByNum = function(item) {
        self.activeIndex(item.index());

        if (!self.newTestVisible()) {
            self.currentQuestionBtnVisible(true);
        }
        self.nextButtonVisible(false);
        self.activeQuestion(self.questions()[item.index()]);

        var correctAnswer = $.grep(self.userAnswers(),
            function(answer) {
                return answer.questionId == self.activeQuestion().id;
            });

        if (correctAnswer[0].isCorrect) {
            self.correctAnswerId(correctAnswer[0].answerId);

        } else {
            self.correctAnswerId(correctAnswer[0].correctAnswerId);
            self.incorrectAsnwerId(correctAnswer[0].answerId);
        }

        self.hasAnswered(true);
        self.nextBtnDisable(false);
        checkIfMathFormulaExist();
    };

    self.goToCurrentQuestion = function () {
        self.nextButtonVisible(true);
        self.currentQuestionBtnVisible(false);

        self.activeIndex(self.userAnswers().length);
        self.activeQuestion(self.questions()[self.activeIndex()]); 

        self.hasAnswered(false); 

        self.nextBtnDisable(true);

        if (self.activeQuestion() == null) {
            self.nextButtonVisible(false);
        }

        if ($('#math-jax-area').find('.math-tex').length) { 
            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);
        }
    }
    //jquery funcs
    function showPiroba() {
        $('#piroba-nav').show();
        $('#question-nav').hide();
    }

    function showQuestion() {
        $('#piroba-nav').hide();
        $('#question-nav').show();
    }

    function checkIfMathFormulaExist() {
        if ($('#math-jax-area').find('.math-tex').length) {

            MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']);

        }
    }

    window.onhashchange = function () {

        self.newTestVisible(false);
        $('.current-answer').hide();
        self.hasAnswered(false);
        self.ModuleId(window.location.hash.substr(1));

        pageLoading(true);
        $.getJSON(U + 'Exercise/GeneretePirobaOrQuestions',
            { ModuleId: self.ModuleId() },
            function(response) {

                if (response.isPiroba) {
                    self.isPiroba(true);
                    $.post(U + 'Exercise/InitExercisePirobaQuestions',
                            { ModuleId: self.ModuleId() },
                            function(response) {
                                self.initExercises(response);
                                if (response.error) {
                                    errorsAlert({ title: '', content: response.error });
                                    pageLoading(false);
                                    return false;
                                }
                                if (response.isEmpty) {
                                    pageLoading(false);
                                    self.isEmpty(true);
                                    self.activeQuestion(null);
                                    // errorsAlert({ title: '', content: 'კითხვების ლიმიტი ამოიწურა' });
                                    self.resetBtn(true);
                                    $('#exercise-ld').fadeIn();
                                    return false;
                                }
                                self.initPiroba(response);
                            })
                        .done(function() {
                            pageLoading(false);
                        });


                } else {
                    self.isPiroba(false);
                    $.post(U + 'Exercise/IntiExerciesQuestions',
                            { ModuleId: self.ModuleId() },
                            function(response) {
                                self.initExercises(response);
                                self.initQuestions(response);
                            })
                        .done(function() {
                            pageLoading(false);

                            if ($('#math-jax-area').find('.math-tex').length) {

                                MathJax.Hub.Queue(["Typeset", MathJax.Hub, 'math-jax-area']); 
                            }
                        });
                }
            });
    } 
}


ko.applyBindings(new ExerciseViewModel());
