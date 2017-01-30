/// <reference path="C:\Users\Nik\Source\Nebula\Nebula.Web\Scripts/knockout.js" />
/// <reference path="~/Scripts/knockout.js" />
/// <reference path="common/exercise-common.js" />
/// <reference path="~/Js/common/exercise-common.js" />
function SubTagsViewModel(data) {
    var self = this;
    self.id = self.id;
    self.name = data.subTagName;
}

function NavTagsViewModel(data) {
    var self = this;
    self.id = data.id;
    self.isPayed = data.isPayed;
    self.name = data.name;
    self.subTags = data.subTags;
    self.needLicense = data.needLicense;
}

function GeoVideoViewModel() {
    var self = this;
    self.navTags = ko.observableArray();
    self.activeSubTag = ko.observable();
    self.activeSubTagId = ko.observable();
    self.video = ko.observable();
    self.isVideoSelected = ko.observable();

    self.exercisePostQuestionsViewModel = new ExerciseQuestionsViewModel();
    self.exercisePostQuestionsViewModel.setAnswerUrl = U + 'GeoVideo/SetAnswer';
    self.exercisePostQuestionsViewModel.resetUrl = U + 'GeoVideo/ResetGrammarVideoQuestions';
    self.videoType = true;
    var currentSubId;

    self.init = function () { 
        $.getJSON(U + 'GeoVideo/GetGrammarNavigation',
             function (response) {
                 ko.utils.arrayForEach(response.tags,
                     function (tag) {
                         self.navTags.push(new NavTagsViewModel(tag));
                     }); 
                 self.initSubTagDataById(self.navTags()[0].subTags[0].id);
             });
    }

    self.isVideoSelected(true);

    self.initSubTagDataById = function (subTagId) {
        self.exercisePostQuestionsViewModel.deleteId = currentSubId = subTagId;
        self.activeSubTagId(subTagId);
        $.post(U + 'GeoVideo/GetGrammarSubTagVideoAndPostQuestions',
            { subTagId: subTagId },
            function (response) {
                self.video(response.video); 

                self.exercisePostQuestionsViewModel.init({
                    questions: response.postQuestions,
                    userAnswers: response.userAnswers
                });
            });
    } 

    //behaviours
    self.getNewSetOffQuestions = function () {
        self.getSubTagDataById({ subTagId: currentSubId });
        self.isVideoSelected(false);
    }

    self.resetQuestions = function () {
        if (!window.IsAuth) {
            self.initSubTagDataById(currentSubId);
        } else {
            $.post(self.exercisePostQuestionsViewModel.resetUrl,
                { Id: currentSubId },
                function(response) {
                    if (response.success) {
                        self.getNewSetOffQuestions();
                    }
                });

            self.exercisePostQuestionsViewModel.hasNextQuestion(true);
            self.exercisePostQuestionsViewModel.quetionsResourceLimited(false);
            self.exercisePostQuestionsViewModel.letActiveQuestion(false);
        }
    }

    self.getSubTagDataById = function (params) {
        self.initSubTagDataById(params.subTagId);
        self.isVideoSelected(true);
    } 

    self.selectPost = function () {
        self.isVideoSelected(false);
    }

    self.selectVideo = function () {
        self.isVideoSelected(true);
    }

    self.init();
}

ko.applyBindings(new GeoVideoViewModel())