/// <reference path="C:\Users\pfloyd555\Documents\nebula\Nebula.Web\Scripts/knockout.js" />

function AnswersViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;

}

function QuestionViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;
    self.userAnswerid = ko.observable(data.userAnswerid);
    self.answerLetter = ko.observable('?');
    self.answers = [];

    self.initAnswers = function () {
        ko.utils.arrayForEach(data.answers, function (answer) {
            self.answers.push(new AnswersViewModel({
                id: answer.id,
                text: answer.text
            }))
        })
    }

    self.setAnswer = function (answerId) {
        console.log('answers', self.answers);
        var index = utils.arrayFirstIndexOf(self.answers, function (answer) {
            return answer.id == answerId;
        });
        self.answerLetter(utils.getLetterByIndex(index));
        self.userAnswerid(answerId);
    }
    self.initAnswers();
    if (data.userAnswerId) {

        self.setAnswer(data.userAnswerId);
    }




}


var ckTextEditing = CKEDITOR.replace('text-editing');
var ckEssay = CKEDITOR.replace('essay');

function FictionTextViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;
    self.userTheme = data.userTheme;
    self.questions = [];
    self.points = [];

    self.activeQuestion = ko.observable();
    self.showText = ko.observable(true);

    self.initQuestions = function () {

        ko.utils.arrayForEach(data.questions, function (question) {
            self.questions.push(new QuestionViewModel({
                id: question.id,
                text: question.text,
                answers: question.answers,
                userAnswerId: question.userAnswerId
            }));
        })

        console.log('questions', self.questions);
    }

    self.initQuestions();
}


function EssayViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;
    self.userText = data.userText;
}

function TexTEditingViewModel(data) {
    var self = this;
    self.id = data.id;
    self.text = data.text;
    self.userText = data.userText;
}



function ExamViewModel() {
    var self = this;
    self.examId = '';
    self.textEditing = ko.observable();
    self.essay = ko.observable();
    self.prose = ko.observable();
    self.poetry = ko.observable();


    self.activePart = ko.observable('textEditing');


    self.choosenTheme = ko.observable();

    self.remainingSeconds = 0;

    self.timer = function (seconds) {
        self.remainingSeconds = seconds;
        setInterval(function () {
            self.remainingSeconds--;
            hour = Math.floor(self.remainingSeconds / 3600);
            minute = Math.floor(self.remainingSeconds / 60) - (hour * 60);
            second = Math.floor(self.remainingSeconds) - (hour * 60 * 60) - (minute * 60);
            hour = hour < 10 ? "0" + hour : hour;
            minute = minute < 10 ? "0" + minute : minute;
            second = second < 10 ? "0" + second : second;
            var remain = utils.timeFormat(hour, 'საათი') + utils.timeFormat(minute, 'წუთი') + utils.timeFormat(second, 'წამი');
            if (self.remainingSeconds == 600) {
                errorsAlert({ title: 'გაფრთხილებთ!', content: '10 წუთი დაგრჩათ' });
            }
            if (self.remainingSeconds == 0) {
                self.gameOver();
            }
            $('.timer').html(remain);
        }, 1000)
    }

    self.init = function () {
        $.post(U + 'GeoExam/Init', function (json) {
            if (json.error) {
                alert(json.error);
                return;
            }
            console.log('json', json);

            self.timer(json.remainingSeconds);
            json = json.exam;
            self.examId = json.id;
            self.textEditing(new TexTEditingViewModel({
                id: json.textEditingId,
                text: json.textEditingText,
                userText: json.userTextEditingText
            }));
            ckTextEditing.insertHtml(json.userTextEditingText);

            self.essay(new EssayViewModel({
                id: json.essayId,
                text: json.essayText,
                userText: json.userEssayText
            }));
            ckEssay.insertHtml(json.userEssayText);


            var proseJson = json.prose;

            self.prose(new FictionTextViewModel({
                id: proseJson.id,
                text: proseJson.text,
                userTheme: proseJson.userTheme,
                questions: proseJson.questions
            }));

            var poetryJson = json.poetry;

            self.poetry(new FictionTextViewModel({
                id: poetryJson.id,
                text: poetryJson.text,
                userTheme: poetryJson.userTheme,
                questions: proseJson.questions
            }));

            self.activePart('textEditing');
        });
    }

    self.gameOver = function () {
        alert('game-i over');
    }

    self.goToTextEditing = function () {
        self.activePart('textEditing');
    }

    self.goToEssay = function () {
        self.activePart('essay');
    }

    self.goToProse = function () {
        self.activePart('prose');
        self.prose().showText(true);
    }

    self.goToPoetry = function () {
        self.activePart('poetry');
        self.poetry().showText(true);
    }

    self.goToPoetryQuestion = function (params) {
        self.activePart('poetry');
        self.poetry().showText(false);
        self.poetry().activeQuestion(self.poetry().questions[params.index]);

    }

    self.goToProseQuestion = function (params) {
        self.activePart('prose');
        self.prose().showText(false);
        self.prose().activeQuestion(self.prose().questions[params.index]);

    }

    self.confirmEnd = function () {

    }

    self.saveTextEditing = function () {

    }

    self.saveEssay = function () {

    }

    self.setAnswer = function (params) {
        $.post(U + 'GeoExam/SetAnswer', { answerId: params.id, examId: self.examId },
            function (response) {
                if (response.error) {
                    alert(response.error);
                    return;
                }

                if (response.success) {
                    if (self.activePart() == 'poetry') {
                        var question = ko.utils.arrayFirst(self.poetry().questions, function (item) {
                            return self.poetry().activeQuestion().id == item.id;
                        });
                        question.setAnswer(params.id);
                    } else if (self.activePart() == 'prose') {
                        var question = ko.utils.arrayFirst(self.prose().questions, function (item) {
                            return self.prose().activeQuestion().id == item.id;
                        });
                        question.setAnswer(params.id);
                    }
                }
            })
    }


    self.init();
}



ko.applyBindings(new ExamViewModel());