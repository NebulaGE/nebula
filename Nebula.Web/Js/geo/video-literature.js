/// <reference path="C:\Users\Nik\Source\Nebula\Nebula.Web\Scripts/knockout.js" />
/// <reference path="~/Scripts/knockout.js" />
/// <reference path="common/exercise-common.js" />

function NavTagsViewModel(data) {
    var self = this;
    self.id = data.id;
    self.isPayed = data.isPayed;
    self.name = data.name;
    self.works = data.works;
    self.needLicense = data.needLicense;
}

function WorkPartViewModel(data) {
    var self = this;
    self.video = data.video;
    self.postQuestionModel = data.postQuestion;
    self.preQuestionModel = data.preQuestion;
}

function GeoVideoViewModel() {
    var self = this;
    self.workParts = ko.observableArray();
    self.navTags = ko.observableArray();
    self.video = ko.observable();

    self.activeWorkPartIndex = ko.observable(0);
    self.activeInsideIndex = ko.observable(0);
    self.activePre = ko.observable(0);
    self.activeVideo = ko.observable();
    self.activePost = ko.observable();

    self.videoType = true;
    var currentWorkId;

    self.type = ko.observable('pre');

    //self.exercisePostQuestionsViewModel = new ExerciseQuestionsViewModel();
    //self.exercisePostQuestionsViewModel.setAnswerUrl = U + 'GeoVideo/SetAnswer'; 

    self.init = function () {
        $.getJSON(U + 'GeoVideo/GetLiteratureNavigation',
            function (response) {
                console.log('response', response);
                ko.utils.arrayForEach(response.authorsWithWorks,
                    function (author) {
                        self.navTags.push(new NavTagsViewModel(author));
                    });
                self.initWorkPartsById(self.navTags()[0].works[0].id);
            });
    }

    self.initWorkPartsById = function (workId) {
        currentWorkId = workId;

        $.post(U + 'GeoVideo/GetLiteratureWorksWithParts',
            { workId: workId },
            function (response) {
                self.workParts.removeAll();
                
                ko.utils.arrayForEach(response.workParts,
                    function (part) {
                        self.post = new ExerciseQuestionsViewModel();
                        self.post.setAnswerUrl = U + 'GeoVideo/SetAnswer';
                        self.post.resetUrl = U + 'GeoVideo/ResetLiteraturePostVideoQuestions';

                        self.post.deleteId = part.deleteId;
                        self.post.init({
                            questions: part.postQuestions,
                            userAnswers: part.postQuestionAnswers
                        });

                        self.pre = new ExerciseQuestionsViewModel();
                        self.pre.setAnswerUrl = U + 'GeoVideo/SetAnswer';
                        self.pre.resetUrl = U + 'GeoVideo/ResetLiteraturePreVideoQuestions';

                        self.pre.deleteId = part.deleteId;
                        self.pre.init({
                            questions: part.preQuestions,
                            userAnswers: part.preQuestionAnswers
                        });

                        self.workParts.push(new WorkPartViewModel({
                            preQuestion: self.pre,
                            video: part.video,
                            postQuestion: self.post
                        }));

                        console.log('work parts', self.workParts());
                    });
                self.activeWorkPartIndex(0);
                self.activePre(self.workParts()[0].preQuestionModel);
                self.selectPre({ index: 0 });
            });
    }

    //behaviours
    self.getAuthorWorkDataById = function (params) {
        self.workParts([]);
        self.initWorkPartsById(params.workId);
    }

    self.getNewSetOffQuestions = function () {
        self.initWorkPartsById(currentWorkId);
    }

    self.resetQuestions = function () {
        if (!window.IsAuth) {
            self.initWorkPartsById(currentWorkId);
        }
        else {
            if (self.type() === "pre") {

                $.post(self.workParts()[self.activeWorkPartIndex()].preQuestionModel.resetUrl,
                    { Id: self.workParts()[self.activeWorkPartIndex()].preQuestionModel.deleteId }
                    , function (response) {
                        if (response.success) {
                            self.getNewSetOffQuestions();
                            self.type('pre');
                        }
                    });
            } else {
                $.post(self.workParts()[self.activeWorkPartIndex()].postQuestionModel.resetUrl,
                    { Id: self.workParts()[self.activeWorkPartIndex()].postQuestionModel.deleteId }, function (response) {
                        if (response.success) {
                            self.getNewSetOffQuestions();
                            self.type('post');
                        }
                    });
            }
        }
    }

    self.selectPost = function (params) {
        self.type('post');
        self.activeWorkPartIndex(params.index);
        self.activeInsideIndex(self.getCurrentIndex(params.index, 2));
        self.activePost(self.workParts()[params.index].postQuestionModel);
    }

    self.selectPre = function (params) {
        self.type('pre');
        self.activeWorkPartIndex(params.index);
        self.activeInsideIndex(self.getCurrentIndex(params.index, 0));
        self.activePre(self.workParts()[params.index].preQuestionModel);
    }

    self.selectVideo = function (params) {
        self.type('video');
        self.activeWorkPartIndex(params.index);
        self.activeInsideIndex(self.getCurrentIndex(params.index, 1));
        self.activeVideo(self.workParts()[params.index].video);
    }

    self.getCurrentIndex = function (index, num) {
        return index * 3 + num;
    }
    self.init();
}

ko.applyBindings(new GeoVideoViewModel())