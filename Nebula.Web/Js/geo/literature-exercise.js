/// <reference path="../common/exercise-navigation-common.js" />

function ExerciseViewModel() {
    var self = this;
  //  location.hash = $('#work-part-id').val();
    self.navigationViewModel = new ExerciseNavigationViewModel();

    //init navigation  config
    self.navigationViewModel.url = U + 'GeoExercises/GetLiteratureNavigation';
    self.navigationViewModel.questionsUrl = U + 'GeoExercises/GetLiteratureQuestions';
    self.navigationViewModel.exerciseViewModel.setAnswerUrl = U + 'GeoExercises/SetAnswer';
    self.navigationViewModel.exerciseViewModel.resetUrl = U + 'GeoExercises/ResetLiterature';
    self.navigationViewModel.activeNavigationId = window.location.hash.substr(1);
    self.navigationViewModel.deleteId = window.location.hash.substr(1);
    self.navigationViewModel.init();

    self.getNewSetOffQuestions = function () { 
        if (!window.IsAuth) {
            location.reload();
        }
        else {
            self.navigationViewModel.initQuestions();
        } 
    }

    self.resetQuestions = function () { 
        $.post(self.navigationViewModel.exerciseViewModel.resetUrl,
             { Id: self.navigationViewModel.activeNavigationId }, function (response) {
             if (response.success) {
                 self.navigationViewModel.init();
                 //self.navigationViewModel.exerciseViewModel.letNext(true);
                 self.navigationViewModel.exerciseViewModel.quetionsResourceLimited(false);
                 self.navigationViewModel.exerciseViewModel.letActiveQuestion(false);
             }
         });
    }
}

ko.applyBindings(new ExerciseViewModel());