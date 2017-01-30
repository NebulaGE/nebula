/// <reference path="../common/exercise-navigation-common.js" />

function ExerciseViewModel() {
    var self = this;
   // location.hash = $('#sub-tag-id').val();
    self.navigationViewModel = new ExerciseNavigationViewModel();

    //init navigation  config
    self.navigationViewModel.url = U + 'GeoExercises/GetGrammarNavigation';
    self.navigationViewModel.questionsUrl = U + 'GeoExercises/GetGrammarQuestions';
    self.navigationViewModel.exerciseViewModel.setAnswerUrl = U + 'GeoExercises/SetAnswer';
    self.navigationViewModel.resetUrl = U + 'GeoExercises/ResetGrammar';
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
         $.post(self.navigationViewModel.resetUrl, 
               { Id: self.navigationViewModel.activeNavigationId}, function (response) {
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