/// <reference path="exercise-common.js" />

function NavigationViewModel(data) {
    var self = this;
    self.id = data.id;
    self.name = data.name;
    self.questionsCount = data.questionsCount;
    self.userAnswersCount = ko.observable(data.userAnswersCount);
    self.type = data.type;
    self.activeClass = ko.observable('');
}

function ExerciseNavigationViewModel() {
    var self = this;
    self.exerciseViewModel = new ExerciseQuestionsViewModel();
    self.navigations = ko.observableArray();
    self.activeNavigation = ko.observable().syncWith('activeNavigation');
    self.containerName = ko.observable('');
    self.reset = ko.observable(false);

    // needs to be initialized from caller
    self.url = '';
    self.questionsUrl = '';
    self.taskUrl = '';
    self.activeNavigationId = '';

    self.title = ko.observable(); 

    self.init = function () {
        self.navigations.removeAll();
 
        $.getJSON(self.url,
            function(json) {
                ko.utils.arrayForEach(json,
                    function(item) {
                        self.navigations.push(new NavigationViewModel({
                            id: item.id,
                            name: item.name,
                            userAnswersCount: item.userAnswersCount,
                            questionsCount: item.questionsCount,
                            type: item.type
                        }));
                    });

                console.log('navigations', self.navigations());
                self.initActiveNavigation(); 
             
                self.initQuestions();
            });
    }

    self.initQuestions = function () { 
        if (self.activeNavigation().type == 1) {
            $.post(self.questionsUrl,
                { id: self.activeNavigationId },
                function(json) { 
                    self.exerciseViewModel.reset();

                    self.exerciseViewModel.init({
                        questions: json.questions,
                        userAnswers: json.userAnswers
                    }); 
                });
        } else if (self.activeNavigation().type == 2) { 
            $.post(
                self.taskUrl,
                { id: self.activeNavigationId },
                function(json) {
                    self.exerciseViewModel.reset();
                    self.exerciseViewModel.init({
                        id: json.id,
                        text: json.text,
                        questions: json.questions,
                        userAnswers: json.userAnswers
                    }); 
                });
        }
    }

    self.initActiveNavigation = function () {
   
        var nav = ko.utils.arrayFirst(self.navigations(),
            function(item) {
                return item.id == self.activeNavigationId;
            });

        nav.activeClass('test-blocks-active');

        ko.utils.arrayForEach(self.navigations(), function (item) {
            if (item.id != self.activeNavigationId) {
                item.activeClass('')
            }
        });
        self.activeNavigation(nav);
       
    }

    self.reset = function() {
        self.navigations.removeAll();
    }

    self.navChange = function(params) {
        location.hash = params.id;
    } 

    window.onhashchange = function() {
        self.activeNavigationId = window.location.hash.substr(1); 
        self.initActiveNavigation();
     
        self.initQuestions(); 
    }
}
    


