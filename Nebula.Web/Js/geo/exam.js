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
    self.plainText = data.plainText;
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
var ckUserTheme = CKEDITOR.replace('user-theme')

function FictionTextViewModel(data) {
    var self = this;
    self.id = data.id;
    self.title = data.title;
    self.text = data.text;
    self.userTheme = data.userTheme;
    self.point1 = data.point1;
    self.point2 = data.point2;
    self.point3 = data.point3;
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
                userAnswerId: question.userAnswerId,
                plainText: $($.parseHTML(question.text)).text()
            }));
        })


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
    self.testNumber = ko.observable();
    self.letPoetryNext = ko.observable();
    self.letProseNext = ko.observable();
    self.letPoetryPrev = ko.observable();
    self.letProsePrev = ko.observable();

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
            self.testNumber(json.testNumber);
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
                title: proseJson.title,
                text: proseJson.text,
                questions: proseJson.questions,
                point1: proseJson.point1,
                point2: proseJson.point2,
                point3: proseJson.point3
            }));

            var poetryJson = json.poetry;

            self.poetry(new FictionTextViewModel({
                id: poetryJson.id,
                title: poetryJson.title,
                text: poetryJson.text,
                questions: poetryJson.questions,
                point1: poetryJson.point1,
                point2: poetryJson.point2,
                point3: poetryJson.point3
            }));
            ckUserTheme.insertHtml(json.userTheme);
            self.activePart('textEditing');
        });
    }

    self.gameOver = function () {
        utils.ckUpdate();
        var textEditing = $('#text-editing').val();
        var essay = $('#essay').val();
        var userTheme = $('#user-theme').val();

        $.post(U + 'GeoExam/GameOver', {
            examId: self.examId,
            textEditing: textEditing,
            essay: essay,
            userTheme: userTheme
        }, function (response) {
            if (response.error) {
                alert(response.error)
                return;
            }
            if (response.success) {
                location.href = U + 'GeoExam/Stats?examId=' + self.examId;
            }
        });
    }

    self.confirmGameOver = function () {
        $.confirm({
            title: 'ტესტის დასრულება',
            content: 'ნამდვილად გსურთ ტესტირების დასრულება?',
            confirm: function () {
                self.gameOver();
            },
            cancel: function () {

            },
            confirmButton: 'კი',
            cancelButton: 'არა'
        });
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
        self.checkPoetryPrevNext(params.index);
    }

    self.goToProseQuestion = function (params) {
        self.activePart('prose');
        self.prose().showText(false);
        self.prose().activeQuestion(self.prose().questions[params.index]);
        self.checkProsePrevNext(params.index);
    }


    self.goToUserTheme = function () {
        self.activePart('userTheme');
    }





    self.getProseNextQuestion = function () {

        var index = utils.arrayFirstIndexOf(self.prose().questions, function (item) {
            return item.id == self.prose().activeQuestion().id;
        });
        index++;
        self.prose().activeQuestion(self.prose().questions[index]);
        self.checkProsePrevNext(index);
    }

    self.getProsePrevQuestion = function () {
        var index = utils.arrayFirstIndexOf(self.prose().questions, function (item) {
            return item.id == self.prose().activeQuestion().id;
        });
        index--;
        self.prose().activeQuestion(self.prose().questions[index]);
        self.checkProsePrevNext(index);
    }


    self.getPoetryNextQuestion = function () {
        var index = utils.arrayFirstIndexOf(self.poetry().questions, function (item) {
            return item.id == self.poetry().activeQuestion().id;
        });
        index++;
        self.poetry().activeQuestion(self.poetry().questions[index]);
        self.checkPoetryPrevNext(index);
    }
    self.getPoetryPrevQuestion = function () {
        var index = utils.arrayFirstIndexOf(self.poetry().questions, function (item) {
            return item.id == self.poetry().activeQuestion().id;
        });
        index--;
        self.poetry().activeQuestion(self.poetry().questions[index]);
        self.checkPoetryPrevNext(index);
    }

    self.showProseText = function () {
        self.prose().showText(true);
    }

    self.showPoetryText = function () {
        self.poetry().showText(true);
    }

    self.confirmEnd = function () {

    }

    self.saveTextEditing = function () {
        utils.ckUpdate();
        var text = $('#text-editing').val()
        $.post(U + 'GeoExam/saveTextEditing', { examId: self.examId, text: text }, function (response) {
            if (response.error) {
                alert(response.error);
                return;
            }

            if (response.success) {
                alert('შენახულია!')
            }
        });
    }

    self.saveEssay = function () {
        utils.ckUpdate();
        var text = $('#essay').val()
        $.post(U + 'GeoExam/saveEssay', { examId: self.examId, text: text }, function (response) {
            if (response.error) {
                alert(response.error);
                return;
            }

            if (response.success) {
                alert('შენახულია!')
            }
        });
    }



    self.saveUserTheme = function () {
        utils.ckUpdate();
        var text = $('#user-theme').val();
        $.post(U + 'GeoExam/SaveUserTheme', { examId: self.examId, text: text }, function (response) {
            if (response.error) {
                alert(response.error);
                return;
            }

            if (response.success) {
                alert('შენახულია!')
            }
        });
    }

    self.setAnswer = function (params) {
        var questionId;
        if (self.activePart() == 'poetry') {
            questionId = self.poetry().activeQuestion().id;
        } else if (self.activePart() == 'prose') {
            questionId = self.prose().activeQuestion().id;
        }
        $.post(U + 'GeoExam/SetAnswer', { answerId: params.id, examId: self.examId, questionId: questionId },
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

    self.checkProsePrevNext = function (index) {
        if (index == 0) {
            self.letProsePrev(false);
        } else {
            self.letProsePrev(true);
        }
        if (self.prose().questions.length == index + 1) {
            self.letProseNext(false);
        } else {
            self.letProseNext(true);
        }
    }


    self.checkPoetryPrevNext = function (index) {
        if (index == 0) {
            self.letPoetryPrev(false);
        } else {
            self.letPoetryPrev(true);
        }
        if (self.poetry().questions.length == index + 1) {
            self.letPoetryNext(false);
        } else {
            self.letPoetryNext(true);
        }
    }


    self.init();
}

$('#slide-toggle-poetry').click(function () {
    $('#poetry-content').slideToggle();
})
$('#slide-toggle-prose').click(function () {
    $('#prose-content').slideToggle();
})

ko.applyBindings(new ExamViewModel());