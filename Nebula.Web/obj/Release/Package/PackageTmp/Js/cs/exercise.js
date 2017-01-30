/// <reference path="../common/exercise-navigation-common.js" />

function ExerciseViewModel() {
    var self = this;
    //if (!location.hash) {
    //    location.hash = $('#module-id').val();

    //}

    self.navigationViewModel = new ExerciseNavigationViewModel();

    //init navigation  config
    var type = $('#type-id').val();
    self.navigationViewModel.url = U + 'CSExercises/GetNavigation?type=' + $('#type-id').val();
    self.navigationViewModel.questionsUrl = U + 'CSExercises/GetQuestions'; 
    self.navigationViewModel.exerciseViewModel.setAnswerUrl = U + 'CSExercises/SetAnswer';
    self.navigationViewModel.exerciseViewModel.resetUrl = U + 'CSExercises/ResetCSExercise';
    self.navigationViewModel.taskUrl = U + 'CSExercises/GetTask';  
    self.navigationViewModel.exerciseViewModel.setTaskAnswerUrl = U + 'CSExercises/SetTaskAnswer'; 
    self.navigationViewModel.activeNavigationId = window.location.hash.substr(1);
    self.navigationViewModel.containerName(type == 1 ? 'ვერბალური სავარჯიშოები' : 'მათემატიკური სავარჯიშოები');
    self.navigationViewModel.exerciseViewModel.deleteId = window.location.hash.substr(1);
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
         $.post(self.navigationViewModel.exerciseViewModel.resetUrl, { Id: self.navigationViewModel.activeNavigationId },
             function (response) {
                 if (response.success) {
                     self.navigationViewModel.init();
                 } 
         }); 
    }
}

ko.applyBindings(new ExerciseViewModel());