/// <reference path="~/Scripts/knockout.js" />

function AnswersViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;
    self.className = ko.observable(data.className);
    self.answerClassName = ko.observable('variant-char-gray');
}

function QuestionViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;
    self.expVideoLink = data.expVideo;
    self.hasAnswered = ko.observable(data.hasAnswered);
    self.userAnswerid = ko.observable(data.userAnswerid);
    self.answers = [];
    self.type = ko.observable(data.type);
}

function ProgressBarViewModel(data) {
    var self = this;
    self.questionId = data.questionId;
    self.status = data.status;
    self.className = ko.observable(data.className);
}

function UserAnswersViewModel(data) {
    var self = this;
    self.answerId = data.answerId;
    self.questionId = data.questionId;
    self.correctAnswerId = data.correctAnswerId;
}

function NavigationViewModel(data) {
    var self = this;
    self.id = data.id;
    self.name = data.name;
    self.url = data.url;
}

function TaskViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;
}


function ExerciseQuestionsViewModel() {
    var self = this;
    //var expVideoHrefValue = $('ExpVideo').attr('href');
    self.questions = ko.observableArray();
    self.userAnswers = ko.observableArray();
    self.activeQuestion = ko.observable();
    self.progressBar = ko.observableArray();
    self.navigation = ko.observableArray();
    self.correctAnswersCount = ko.observable(0);
    self.incorrectAnswersCount = ko.observable(0);

    self.activeIndex = 0;
    self.lastActiveIndex = 0;
    self.letActiveQuestion = ko.observable(false);
    self.letNext = ko.observable(false);
    self.letPrevious = ko.observable(false);
    self.hasNextQuestion = ko.observable(true);
    self.currentQuestionBtnVisible = ko.observable(false);
    self.quetionsResourceLimited = ko.observable(false);
    
    //task properties
    self.task = ko.observable();
    self.showTask = ko.observable(false);
    self.letTaskNext = ko.observable(true);
    self.letTaskPrev = ko.observable(true);
    self.finishedTask = ko.observable(false);

    self.activeNavigation = ko.observable().syncWith('activeNavigation');

    var letNewQuestions = false;

    self.deleteId=0;
    self.setAnswerUrl = '';
    self.setTaskAnswerUrl = '';
    self.resetUrl = '';

    self.init = function (data) {
        
        self.reset(); 
        if (!self.activeNavigation()) {
            self.quetionsResourceLimited(false);
            self.showTask(false);
            self.initQuestions(data.questions);
           
            self.initUserAnswers(data.userAnswers);
            self.initActiveQuestion();
            self.initProgressBar();
            self.checkIfNextOrPrevExist();
            self.initMathJax();

            self.checkMoveBtns();
            return;
        }
        switch (self.activeNavigation().type) {
            case 1: //questions 
                self.showTask(false);
            
                self.initQuestions(data.questions);
                self.initUserAnswers(data.userAnswers);
                self.initActiveQuestion();             
                break;
            case 2: //task
                self.finishedTask(false);
                self.showTask(true);
             
                self.task(new TaskViewModel({ id: data.id , text : data.text}));
                self.initQuestions(data.questions);               
                self.initUserAnswers(data.userAnswers); 
                break;
        }

            self.initProgressBar();
            self.checkIfNextOrPrevExist();
            self.initMathJax();

        self.checkMoveBtns();
    }


    self.checkMoveBtns = function() {
        if (self.questions().length == self.userAnswers().length) {
            self.quetionsResourceLimited(true);
            self.hasNextQuestion(false);
        
        } else { 
            self.hasNextQuestion(true);
            self.letNext(false);
            self.letActiveQuestion(false);
            self.quetionsResourceLimited(false);
        }
    }

    self.initQuestions = function (questions) {
        letNewQuestions = false;
        self.hasNextQuestion(true);
        self.questions.removeAll();
        ko.utils.arrayForEach(questions, function (question) {
            var questionViewModel = new QuestionViewModel
            ({
                id: question.id,
                text: question.text,
                expVideo:question.explanationVideo,
                hasAnswered: question.hasAnswered,
                type : self.activeNavigation() ? self.activeNavigation().type : 1
            });

            ko.utils.arrayForEach(question.answers, function (answer) {
                questionViewModel.answers.push(new AnswersViewModel({
                    id: answer.id,
                    text: answer.text,
                    className: 'variants'
                }));
            });
            self.questions.push(questionViewModel);       
        }); 
    }

    self.initUserAnswers = function (userAnswers) {
        ko.utils.arrayForEach(userAnswers, function (userAnswer) {
            self.userAnswers.push(new UserAnswersViewModel(
                {
                    answerId: userAnswer.answerId,
                    correctAnswerId: userAnswer.correctAnswerId,
                    questionId: userAnswer.questionId
                }
            ));
            console.log('user answers', self.userAnswers());
            self.setAnswerStatus(userAnswer.questionId, userAnswer.answerId, userAnswer.correctAnswerId);
        });
    }

    self.initProgressBar = function () {
        ko.utils.arrayForEach(self.questions(), function (question) {
            self.progressBar.push(new ProgressBarViewModel({ status: 'empty', className: 'default', questionId: question.id }));
        });

        ko.utils.arrayForEach(self.userAnswers(),
            function (userAnswer) {
                self.setProgressBarStatus(userAnswer.questionId, userAnswer.answerId, userAnswer.correctAnswerId);
            });
    }

    self.initActiveQuestion = function () {
        var question = ko.utils.arrayFirst(self.questions(),
            function (question) {
                return !question.hasAnswered();
            });
         
       
        if (question==null) {
            self.activeQuestion(self.questions()[self.questions().length - 1]); 
        } else {
            self.activeQuestion(question);
        }
        

        if (question !== null) {
            self.lastActiveIndex = self.activeIndex = arrayFirstIndexOf(self.questions(),
            function (item) {
                return item.id === self.activeQuestion().id;
            });
        } else {
            self.quetionsResourceLimited(true);
        }

        $('#ExpVideo').attr('href', self.activeQuestion().expVideoLink);
    }

    self.initMathJax = function () {
        if ($('.math-tex').length) {
            MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
        }
    }

    //behaviours
    self.getQuestionByIndex = function (params) {
        var temp = self.questions()[params.index];
        switch (temp.type()) {
            case 1: 
                if (self.lastActiveIndex >= params.index ) {
                    self.showTask(false);
                    self.activeQuestion(temp);
                    self.activeIndex = params.index;
                    self.checkIfNextOrPrevExist();
                    self.initMathJax();
                }
                break;
            case 2:
                self.getTaskQuestionByIndex(params);
                break;
           } 
    }  

    self.setAnswer = function (params) {
        if (!self.activeNavigation()) {
            self.setQestionTypeAnswer(params);
            return;
        }
        if (self.activeNavigation().type == 1) {
            self.setQestionTypeAnswer(params);
        } else if (self.activeNavigation().type == 2) {
            self.setTaskAnswer(params);
        } 
    }

    self.setQestionTypeAnswer = function (params) {
        var question = ko.utils.arrayFirst(self.questions(), function (item) {
            return item.id == params.questionId;
        });

        question.hasAnswered(true);
        question.userAnswerid(params.answerId);

        $.post(self.setAnswerUrl, { questionId: params.questionId, answerId: params.answerId },
            function (response) {
                self.setProgressBarStatus(params.questionId, params.answerId, response.correctAnswerId);
                self.setAnswerStatus(params.questionId, params.answerId, response.correctAnswerId);
            });

        self.letNext(true);
        letNewQuestions = true;
        self.checkIfNextOrPrevExist();
        if (self.activeNavigation()) {
            self.activeNavigation().userAnswersCount(self.activeNavigation().userAnswersCount() + 1);
        } 
    }
  

    self.getNextQuestion = function () { 
        if (self.letNext() === true && self.activeQuestion().hasAnswered()===true) {
            self.lastActiveIndex++;
            self.activeIndex++;
            self.activeQuestion(self.questions()[self.activeIndex]);
            $('#ExpVideo').attr('href', self.activeQuestion().expVideoLink);
            self.letNext(false);
            self.checkIfNextOrPrevExist();
            self.initMathJax(); 
        }
    }

    self.getPreQuestion = function () { 
        if (self.letPrevious() === true) {
            self.activeIndex--;
            self.activeQuestion(self.questions()[self.activeIndex]);
            $('#ExpVideo').attr('href', self.activeQuestion().expVideoLink);
            self.checkIfNextOrPrevExist();
            self.initMathJax();
        }
    }

    self.getCurrentQuestion = function () { 
        self.activeQuestion(self.questions()[self.lastActiveIndex]);
        $('#ExpVideo').attr('href', self.activeQuestion().expVideoLink);
        self.activeIndex = self.lastActiveIndex;
        self.checkIfNextOrPrevExist();
        self.initMathJax();
    }

    self.replaceItems = function () {
        var current = self.questions()[self.lastActiveIndex];
        self.questions()[self.lastActiveIndex] = self.questions()[self.lastActiveIndex + 1];
        var temp = self.questions()[self.questions().length - 1];
        self.questions()[self.questions().length - 1] = current;


        for (i = self.lastActiveIndex + 1; i < self.questions().length - 2; i++) {
            self.questions()[i] = self.questions()[i + 1];
        }

        self.questions()[self.questions().length - 2] = temp;
    }

    self.skipQuestion = function () {
        self.activeQuestion(self.questions()[self.lastActiveIndex + 1]);
        self.replaceItems();
        self.initMathJax();
    }

    //Task specific functions

    self.getTaskQuestionByIndex = function (params) {
        self.showTask(false);
        self.activeIndex = params.index;
        self.toggleTaskNextLeftBtnEnable(params.index);
        self.activeQuestion(self.questions()[params.index]);
        $('#ExpVideo').attr('href', self.activeQuestion().expVideoLink);
    }

    self.setTaskAnswer = function (params) {
        var question = ko.utils.arrayFirst(self.questions(), function (item) {
            return item.id == params.questionId;
        });

        question.hasAnswered(true);
        question.userAnswerid(params.answerId);

        $.post(self.setTaskAnswerUrl, {
            questionId: params.questionId,
            answerId: params.answerId,
            taskId: self.task().id

        }, function (response) {
            self.setProgressBarStatus(params.questionId, params.answerId, response.correctAnswerId);
            self.setAnswerStatus(params.questionId, params.answerId, response.correctAnswerId);
            self.activeNavigation().userAnswersCount(self.activeNavigation().userAnswersCount() + 1);
      
            if (self.questions().length == self.correctAnswersCount() + self.incorrectAnswersCount()) {
                self.finishedTask(true);
            } 
        });
    }

    self.getTaskNextQuestion = function () {
        self.activeIndex++;
        self.toggleTaskNextLeftBtnEnable(self.activeIndex);
        self.activeQuestion(self.questions()[self.activeIndex]);
        $('#ExpVideo').attr('href', self.activeQuestion().expVideoLink);
    }

    self.getTaskPrevQuestion = function () {
        self.activeIndex--;
        self.toggleTaskNextLeftBtnEnable( self.activeIndex);
        self.activeQuestion(self.questions()[ self.activeIndex]);
        $('#ExpVideo').attr('href', self.activeQuestion().expVideoLink);
    }

    self.toggleTaskNextLeftBtnEnable = function (index) {
        if (self.questions().length  == index + 1) {
            self.letTaskNext(false);
        }else{
            self.letTaskNext(true);
        }
        if(index == 0){
            self.letTaskPrev(false);
        } else{
            self.letTaskPrev(true);
        }
     
    }

    self.showTaskFunc = function () {
        self.showTask(true);
    }
    //End Task specific functions


    //private functions
    self.getProgressBarByQuestionId = function (questionId) {
        return ko.utils.arrayFirst(self.progressBar(),
                    function (item) {
                        return item.questionId == questionId;
                    });
    }

    self.setProgressBarStatus = function (questionId, answerId, correctAnswerId) {
        var progressBar = self.getProgressBarByQuestionId(questionId);
        var isCorrect = answerId == correctAnswerId ? true : false;
        progressBar.status = isCorrect ? 'correct' : 'incorrect';
        progressBar.className(isCorrect ? 'iscorrect' : 'isnotcorrect');
    }

    self.setAnswerStatus = function (questionId, answerId, correctAnswerId) {

        var question = ko.utils.arrayFirst(self.questions(), function (item) {
            return item.id == questionId;
        });
        console.log('question ID', questionId);
        question.hasAnswered(true);
        question.userAnswerid(answerId);

        var isCorrect = answerId == correctAnswerId ? true : false;
        if (isCorrect) {
            self.correctAnswersCount(self.correctAnswersCount() + 1);
        } else {
            self.incorrectAnswersCount(self.incorrectAnswersCount() + 1);
        }

        ko.utils.arrayForEach(question.answers, function (answer) {
            if (answerId == answer.id) {
                answer.className(isCorrect ? 'correct' : 'mistake');
                answer.answerClassName('variant-char-white');
            }
            if (correctAnswerId == answer.id) {
                answer.className('correct');
                answer.answerClassName('variant-char-white');
            }
        }); 
    }

    self.reset = function () {
        self.questions.removeAll();
        self.userAnswers.removeAll();
        self.progressBar.removeAll();
        self.correctAnswersCount(0);
        self.incorrectAnswersCount(0);
    }

    self.checkIfNextOrPrevExist = function () { 
        if (self.activeIndex !== 0) {
            self.letPrevious(true);
        }
        else {
            self.letPrevious(false);
        }
        if (self.activeIndex < self.lastActiveIndex) {
            self.letActiveQuestion(true);
        }
        else {
            self.letActiveQuestion(false);
        } 
        if (self.questions().length - 1 === self.activeIndex && self.letNext()) { 
            self.hasNextQuestion(false);
        } else { 
            letNewQuestions = false;
        }
    };

    function arrayFirstIndexOf(array, predicate, predicateOwner) {
        for (var i = 0, j = array.length; i < j; i++) {
            if (predicate.call(predicateOwner, array[i])) {
                return i;
            }
        }
        return -1;
    }

    return self;
}

