pageLoading(true);
function AllViewModel() {
    var self = this; 
    self.locked = ko.observable(false);
    self.verbalVideo = ko.observable(null);
    self.mathVideo = ko.observable(null);

    self.verbalExercise = ko.observable(null);
    self.mathExercise = ko.observable(null);

    self.currentQuiz = ko.observable(null);
    self.lastQuiz = ko.observable(null);
    self.analitic = ko.observable(null);
    self.leftDays = ko.observable();
    self.currentQuizText = ko.observable('მიმდინარე გამოცდა');

    self.resultViewhelper = ko.observable(3);
    self.scores = ko.observableArray();
    self.expectedScore = ko.observable(0);
    self.expectedPercent = ko.observable(0);
    self.isAuth = ko.observable();
    self.hasLicense = ko.observable(false);

    self.showPopup = function () {
        $('.exam_popup').fadeIn();
    }

    $.getJSON(U + 'All/GetAll', function (response) {
        self.locked(response.lockedQuiz);
        self.isAuth(response.isAuth);
        self.hasLicense(response.hasLicense);
     
        if (response.isAuth) {
            if (response.verbalVideos) {        
                self.verbalVideo(U + 'video/index?ModuleId=' + response.verbalVideos.id);
            }
        } else {
            self.verbalVideo(U + 'All');
        }

        if (response.isAuth) {
            if (response.mathVideos) {
          
                self.mathVideo(U + 'video/index?ModuleId=' + response.mathVideos.id);
            }
        } else{
            self.mathVideo(U + 'All');
        }

        if (response.isAuth) {
            if (response.verbalExercises) {
             
                self.verbalExercise(U + 'Exercise/Index#' + response.verbalExercises.id);
            }
        } else {
            self.verbalExercise( U + 'All')
        }

        if (response.isAuth) {
            if (response.mathExercises) {
                self.mathExercise(U + 'Exercise/Index#' + response.mathExercises.id);
            }
        } else {
            self.mathExercise(U + 'All');
        }

    
        if (response.isAuth) {
            self.analitic(U + 'User/Panel');
            if (response.lastQuiz) { 
                self.lastQuiz( U + 'Exam/Result?quizId=' + response.lastQuiz.id + '&returnUrl=' + U + 'All/Index'); 
            }
        } else{
            self.lastQuiz( U + 'All');
            self.analitic( U + 'All');
        }

        if (response.isAuth) {
            if (response.lockedQuiz) {
                self.currentQuizText('პაკეტის შეძენა');
                self.currentQuiz(U + 'Payment');
            }
            else if (response.currentQuiz) {
                self.currentQuizText('გაგრძელება');
                self.currentQuiz(U + 'Exam');
            } else {
                if (response.leftDays > 0) {
                    self.currentQuizText(response.leftDays + ' დღე');
                } else {
                    self.currentQuizText('გამოცდის დაწყება');
                }
                self.currentQuiz(U + 'Exam');
            }
               
        } else {
            self.currentQuiz(U + 'All');
        }

        ko.utils.arrayForEach(response.verbalResult, function (item) {
            self.scores.push({ module: item.module, percent: Math.round(item.percent) });
        });

        ko.utils.arrayForEach(response.mathResult, function (item) {
            self.scores.push({ module: item.module, percent: Math.round(item.percent) });
        });

        self.expectedScore(Math.round(response.expectedScore));

        self.expectedPercent(Math.round(self.expectedScore() / 80 * 100));

        self.leftDays(response.leftDays);              
    }).done(function () {
        pageLoading(false);
    });

    self.getResult = function (item) {
        location = U + 'Exam/Result?quizId=' + item.quizId+'&returnUrl='+U+'All/Index';
    }
}

ko.applyBindings(new AllViewModel());