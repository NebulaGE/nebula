$(document).ready(function() {
    if ($('#Id').val() !== 0  && $('#CategoryId').val() === "1") { 
        getChoosenSelectDataForAuthorWork( $('#sAuthor').val(), $('#workPartId').val() );
    }
   
   console.log("cat", $('#CategoryId').val());
        console.log("id", $('#Id').val());
    if ($('#Id').val() !== 0 && $('#CategoryId').val() === "2") {
        
        getSelectDataForSubTags($('#sTagId').val(), $('#tag').val());
    }
});

function AnswerViewModel(data) {
    var self = this;
    self.Id = data.Id;
    self.IsCorrect = data.IsCorrect;
    self.Text = data.Text; 
}

function AdminViewModel() {
    var self = this;
    self.answers = ko.observableArray([]);

    self.numHelper =  function(index) {
        var L = '';
        switch (index) {
            case 0:
                L = 'ა';
                break;
            case 1:
                L = 'ბ';
                break;
            case 2:
                L = 'გ';
                break;
           case 3:
                L = 'დ';
                break;
            case 4:
                L = 'ე';
                break;
            case 5:
                L = 'ვ';
                break;
        }
        return L;
    }

    self.addAnswer = function () {

        self.answers.push(new AnswerViewModel({
            Id: 0,
            IsCorrect: false,
            Text: ''
        }));

        var index = self.answers().length - 1;       
                    CKEDITOR.replace("Answers[" + index + "].Text",
                    {
                        extraPlugins: 'mathjax',
                        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
                    }); 
    }
     
    self.removeAnswer = function(data) {
        self.answers.remove(self.answers()[data.index]);
    }

    $(document).on('click', '.answer-radio', function () {
                $.each($('.answer-radio'),
                    function() {
                        $(this).prop('checked', false);
                    });
                $(this).prop('checked', true);
            });

    self.initAnswers = function () {
        if ($('#json-data').data('answers-json')) {
            self.answers($('#json-data').data('answers-json'));
        }

       
        setTimeout(function () {
            $.each(self.answers(),
                function (i, v) {
                   
                    CKEDITOR.replace("Answers[" + i + "].Text",
                    {
                        extraPlugins: 'mathjax',
                        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
                    });
                });
            $('.ans-checked').prop('checked', true);
        }, 500);
    }

    self.initAnswers();

    self.generateRaodenobriviAnswers = function () {
         
        var ck1, ck2, ck3, ck4;

        for (i = 0; i < 4; i++) {

            self.answers.push(new AnswerViewModel({
                id: 0,
                isCorrect: false
            }));

            switch (i) {
                case 0:                                  
                    ck1 = CKEDITOR.replace("Answers[" + i + "].Text", {
                        extraPlugins: 'mathjax',
                        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML',
                    });
                    ck1.on("instanceReady", function (event) {
                        var text = '<p>A სვეტის უჯრაში მოცემული რაოდენობა მეტია B სვეტის უჯრაში მოცემულ რაოდენობაზე</p>';
                        ck1.config.allowedContent = true;
                        ck1.insertHtml(text);
                    });

                    break;
                case 1:                         
                    ck2 = CKEDITOR.replace("Answers[" + i + "].Text", {
                        extraPlugins: 'mathjax',
                        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                    });
                    ck2.on("instanceReady", function (event) {
                        var text = '<p>B სვეტის უჯრაში მოცემული რაოდენობა მეტია A სვეტის უჯრაში მოცემულ რაოდენობაზე</p>';
                        ck2.config.allowedContent = true;
                        ck2.insertHtml(text);
                    });
                    break;
                case 2:
                    ck3 = CKEDITOR.replace("Answers[" + i + "].Text", {
                        extraPlugins: 'mathjax',
                        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                    });
                    ck3.on("instanceReady", function (event) {
                       var text = '<p>A სვეტის უჯრაში მოცემული რაოდენობა B სვეტის უჯრაში მოცემული რაოდენობის ტოლია</p>';
                        ck3.config.allowedContent = true;
                        ck3.insertHtml(text);
                    });
                    break;
                case 3:
                    ck4 = CKEDITOR.replace("Answers[" + i + "].Text", {
                        extraPlugins: 'mathjax',
                        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                    });
                    ck4.on("instanceReady", function (event) {
                       var  text = '<p>მოცემული ინფორმაცია საკმარისი არაა იმის დასადგენად, რომელი რაოდენობაა მეტი</p>';
                        ck4.config.allowedContent = true;
                        ck4.insertHtml(text);
                    });
                    break;
            }
        } 
    }

    self.generateMonacemtaAnswers = function () {
        var ck1, ck2, ck3, ck4, ck5;
        for (i = 0; i < 5; i++) {
            self.answers.push(new AnswerViewModel({
                id: 0,
                isCorrect: false
            }));
            switch (i) {
            case 0:
                ck1 = CKEDITOR.replace("Answers[" + i + "].Text",
                {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML',
                });

                ck1.on("instanceReady",
                    function(event) {
                        var text =
                            '<p>I პირობა საკმარისია, II კი – არა</p>';
                        ck1.config.allowedContent = true;
                        ck1.insertHtml(text);
                    });

                break;
            case 1:
                ck2 = CKEDITOR.replace("Answers[" + i + "].Text",
                {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                });
                ck2.on("instanceReady",
                    function(event) {
                        var text =
                            '<p>II პირობა საკმარისია, I კი – არა</p>';
                        ck2.config.allowedContent = true;
                        ck2.insertHtml(text);
                    });
                break;
            case 2:
                ck3 = CKEDITOR.replace("Answers[" + i + "].Text",
                {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                });
                ck3.on("instanceReady",
                    function(event) {
                        var text =
                            '<p>I და II პირობა ერთად საკმარისია, ცალ-ცალკე არც ერთი არ არის საკმარისი</p>';
                        ck3.config.allowedContent = true;
                        ck3.insertHtml(text);
                    });
                break;
            case 3:
                ck4 = CKEDITOR.replace("Answers[" + i + "].Text",
                {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                });
                ck4.on("instanceReady",
                    function(event) {
                        var text = '<p>საკმარისია ცალ-ცალკე როგორც I, ასევე, II პირობა</p>';
                        ck4.config.allowedContent = true;
                        ck4.insertHtml(text);
                    });
                break;

                case 4:
                    ck5 = CKEDITOR.replace("Answers[" + i + "].Text",
                    {
                        extraPlugins: 'mathjax',
                        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
                    });
                    ck5.on("instanceReady", function (event) {
                        text = '<p>მოცემული პირობები არ არის საკმარისი</p>';
                        ck5.config.allowedContent = true;
                        ck5.insertHtml(text);
                    });
                    break;
            }
        }
    } 
}

ko.applyBindings(new AdminViewModel());


$(function () {
     
    $('#AuthorId').change(function () {

        $('#AuthorWorkId').empty();

        getSelectDataForAuthorWork( $(this).val() );
    });

    $('#sTagId')
        .change(function() {
            $('#SubTagId').empty();
            getSelectDataForSubTags($(this).val());
        });

    $('#sAuthor').change(function() {
            $('#AuthorWorkId').empty();
            $('#AuthorWorkPartId').empty();
            getSelectDataForAuthorWork( $(this).val() ); 
    });

    $('#AuthorWorkId').change(function () {
        $('#AuthorWorkPartId').empty();
        getSelectDataForAuthorWorkPart( $('#AuthorWorkId').val() );
    });
});

function getChoosenSelectDataForAuthorWork( authorId, partId ) { 
    var work = $('#AuthorWorkId');

    $.post(
        U + 'AdminGeoQuestion/GetChoosenAuthorWorksById',
        { id:authorId,partId:partId},
        function(data) { 
            $.each(data.works, function (index, value) { 
                if (value.Id === data.selectedWorkId)
                {
                    work.append('<option value="' + value.Id + '" selected="' + 'selected' + '">' + value.Title + '</option>');
                    getSelectDataForAuthorWorkPart( value.Id, partId );
                }
                else 
                    work.append('<option value="' + value.Id + '">' + value.Title + '</option>');   
            });
        } 
    ); 
}

//function getChoosenSelectDataForSubTag(tagId, partId) {
//    $.post(
//        U + 'AdminGeoQuestion/GetSubTagsByTagId'
//        );
//}

function getSelectDataForAuthorWork(authorId) { 
    var work = $('#AuthorWorkId');

    $.ajax({
            type: "POST",
            url: U + "AdminGeoAuthorWorkParts/GetAuthorWorksById",
            datatype: "Json",
            data: { id: authorId },
            success: function (data) {
                work.append('<option selected="' + 'selected' + '">' + 'აირჩიე ნამუშევარი' + '</option>');
                $.each(data, function (index, value) {
                    work.append('<option value="' + value.Id + '">' + value.Title + '</option>');
                });
                getSelectDataForAuthorWorkPart( $('#AuthorWorkId').val());
            }
        });
}

function getSelectDataForAuthorWorkPart(authorWorkId, partId) {
    var workPart = $('#AuthorWorkPartId'); 

    $.post(
        U + 'AdminGeoQuestion/GetAuthorWorkPartsById',
        { id: authorWorkId },
        function (data) {

            if (partId == undefined) {
                workPart.append('<option selected="' + 'selected' + '">' + 'აირჩიე ნაწილი' + '</option>');
            }

            $.each(data,
                function(index, value) {
                    if (partId != undefined) { 
                        if (String(value.Id) === partId) {
                            workPart.append('<option value="' + value.Id + '" selected="' + 'selected' + '">' + value.PartName + '</option>');
                        } else
                            workPart.append('<option value="' + value.Id + '">' + value.PartName + '</option>');
                    }else
                        workPart.append('<option value="' + value.Id + '">' + value.PartName + '</option>');
                });
        }); 
}

function getSelectDataForSubTags(tagId,partId) {
    var subTag=$('#SubTagId');

    $.post(
        U + 'AdminGeoQuestion/GetSubTagsByTagId',
        { id: tagId },
        function (data) {
            subTag.append('<option selected="' + 'selected' + '">' + 'აირჩიე ქვეთეგი' + '</option>');
            $.each(data,
                    function (index, value) {
                        console.log("partId", partId);
                        console.log("val", value.Id);
                        if (partId !== undefined) 
                            if (String(value.Id) === partId)    
                                subTag.append('<option value="' + value.Id + '" selected="' + 'selected' + '">' + value.TagName + '</option>');
                            else
                                subTag.append('<option value="' + value.Id + '">' + value.TagName + '</option>');
                        else
                            subTag.append('<option value="' + value.Id + '">' + value.TagName + '</option>');
                    });
        });
}

$('#add-images-form').on('submit', function (e) {
    e.preventDefault();
    var fd = new FormData();
    var ins = document.getElementById('images').files.length;

    for (var x = 0; x < ins; x++) {
        fd.append("images[]", document.getElementById('images').files[x]);
    }
    var ajax = new XMLHttpRequest();

    ajax.open("POST", U + 'AdminGlobal/AddImages');
    ajax.send(fd);

    ajax.onreadystatechange = function () {
        if (ajax.status == 200 && ajax.readyState == 4) {
            var response = JSON.parse(ajax.responseText);

            $.each(response, function (i, v) {
                $('#response-img-url').append('<p>' + U + 'Content/Images/' + v + '</p>');
            });
        }
    } 
});

var i = 0;

if ($('#startIndex').length) {
    i = $('#startIndex').val();
}


//$('#add-answer').on('click', function () {


//    var inputTemplate = '<div class="form-group">' +
//   '<label class ="control-label col-md-2"> პასუხი (' + numletterHelper(i) + ') </label>' +
//   '<div class="col-md-7">' +
//   '<span class="X">X</span>' +
     
//        '<textarea  type="text" class="form-control" id=Answers[' + i + '].Text"  name="Answers[' + i + '].Text"  required /></textarea>' +
//   '<span style="">' +

//                  '<label style="margin-top:5px;" >სწორია?</label>' +
//                  '<input  type="radio" value="true"   name="Answers[' + i + '].IsCorrect"  class="question-checkbox" />' +
      
//              '</span>' +
//   '</div></div>';

//    $('.answers').append(inputTemplate);
//    CKEDITOR.replace("Answers[" + i + "].Text", {
//        extraPlugins: 'mathjax',
//        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
//    });
//    i++;
//});


$('.show-content').on('click', function () {
    $(this).next('.quest').fancybox({ 
        helpers: {
            overlay: {
                locked: false
            }
        }
    });
    $(this).next('.quest').click(); 
}); 

$('#caption').change(function () {

    if ($('#caption').is(':checked')) {
        $('#caption-form').show();
    } else {
        $('#caption-form').hide();
    }
});

//ckeditors 
if ($('#ck-editor').length != 0) {
   
    var ckeditor = CKEDITOR.replace('ck-editor', {
        extraPlugins: 'mathjax',
        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
    });
    ckeditor.config.allowedContent = true;

    $('#raodenobrivi-shedareba').on('click', function () {
      
        ckeditor.insertHtml($('#raodenobrivi-shedarebis-porma').html());
    });

    $('#analogebi').on('click', function () {
        ckeditor.insertHtml($('#analogebis-porma').html());
    });
}


if ($('#ck-editor2').length != 0) {
    var ckeditor2 = CKEDITOR.replace('ck-editor2', {
        extraPlugins: 'mathjax',
        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
    });
    ckeditor2.config.allowedContent = true;

    $('#raodenobrivi-shedareba').on('click', function () {
        ckeditor2.insertHtml($('#raodenobrivi-shedarebis-porma').html());
    });

    $('#analogebi').on('click', function () {
        ckeditor2.insertHtml($('#analogebis-porma').html());
    });
}


if ($('#Caption').length != null) {
    var ckeditor3 = CKEDITOR.replace('Caption', {
        extraPlugins: 'mathjax',
        mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
    });
    ckeditor3.config.allowedContent = true;
}

$('#generate-raodenobrivi').on('click', function () {
    var text = '';
    var ck1, ck2, ck3, ck4;

    for (k = 0; k < 4; k++) {
        var inputTemplate = '<div class="form-group">' +
     '<label class ="control-label col-md-2"> პასუხი (' + numletterHelper(k) + ') </label>' +
     '<div class="col-md-7">' +
     '<span class="X">X</span><textarea  type="text" class="form-control" id ="Answer' + k + '"  name="Answer' + k + '"  required /></textarea>' +
     '<span style="">' +

        '<label style="margin-top:5px;" >სწორია?</label>' +
        '<input  type="radio" value="' + k + '" name="IsCorrect"  class="question-checkbox" />' + 
        '</span>' +
        '</div></div>';

        $('.answers').append(inputTemplate);
        switch (k) {
            case 0:
                ck1 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML',


                });
                ck1.on("instanceReady", function (event) {
                    text = '<p>A სვეტის უჯრაში მოცემული რაოდენობა მეტია B სვეტის უჯრაში მოცემულ რაოდენობაზე</p>';
                    ck1.config.allowedContent = true;
                    ck1.insertHtml(text);
                });

                break;
            case 1:
                ck2 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                });
                ck2.on("instanceReady", function (event) {
                    text = '<p>B სვეტის უჯრაში მოცემული რაოდენობა მეტია A სვეტის უჯრაში მოცემულ რაოდენობაზე</p>';
                    ck2.config.allowedContent = true;
                    ck2.insertHtml(text);
                });
                break;
            case 2:
                ck3 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                });
                ck3.on("instanceReady", function (event) {
                    text = '<p>A სვეტის უჯრაში მოცემული რაოდენობა B სვეტის უჯრაში მოცემული რაოდენობის ტოლია</p>';
                    ck3.config.allowedContent = true;
                    ck3.insertHtml(text);
                });
                break;
            case 3:
                ck4 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'

                });
                ck4.on("instanceReady", function (event) {
                    text = '<p>მოცემული ინფორმაცია საკმარისი არაა იმის დასადგენად, რომელი რაოდენობაა მეტი</p>';
                    ck4.config.allowedContent = true;
                    ck4.insertHtml(text);
                });
                break;
        } 
    }
});

$('#generate-monacemta-sakmarisoba').on('click', function () {
    var text = '';
    var ck1, ck2, ck3, ck4, ck5;

    for (k = 0; k < 5; k++) {
        var inputTemplate = '<div class="form-group">' +
     '<label class ="control-label col-md-2"> პასუხი (' + numletterHelper(k) + ') </label>' +
     '<div class="col-md-7">' +
     '<span class="X">X</span><textarea  type="text" class="form-control" id ="Answer' + k + '"  name="Answer' + k + '"  required /></textarea>' +
     '<span style="">' +

        '<label style="margin-top:5px;" >სწორია?</label>' +
        '<input  type="radio" value="' + k + '" name="IsCorrect"  class="question-checkbox" />' +
        //  '<input type="hidden" value="false" name="IsCorrect'+i+'" />'+
        '</span>' +
        '</div></div>';

        $('.answers').append(inputTemplate);
        switch (k) {
            case 0:
                ck1 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML', 
                });
                ck1.on("instanceReady", function (event) {
                    text = '<p>I პირობა საკმარისია, II კი – არა</p>';
                    ck1.config.allowedContent = true;
                    ck1.insertHtml(text);
                });
                break;
            case 1:
                ck2 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
                });
                ck2.on("instanceReady", function (event) {
                    text = '<p>II პირობა საკმარისია, I კი – არა</p>';
                    ck2.config.allowedContent = true;
                    ck2.insertHtml(text);
                });
                break;
            case 2:
                ck3 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
                });
                ck3.on("instanceReady", function (event) {
                    text = '<p>I და II პირობა ერთად საკმარისია, ცალ-ცალკე არც ერთი არ არის საკმარისი</p>';
                    ck3.config.allowedContent = true;
                    ck3.insertHtml(text);
                });
                break;
            case 3:
                ck4 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
                });
                ck4.on("instanceReady", function (event) {
                    text = '<p>საკმარისია ცალ-ცალკე როგორც I, ასევე, II პირობა</p>';
                    ck4.config.allowedContent = true;
                    ck4.insertHtml(text);
                });
                break;
            case 4:
                ck5 = CKEDITOR.replace('Answer' + k, {
                    extraPlugins: 'mathjax',
                    mathJaxLib: 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML'
                });
                ck5.on("instanceReady", function (event) {
                    text = '<p>მოცემული პირობები არ არის საკმარისი</p>';
                    ck5.config.allowedContent = true;
                    ck5.insertHtml(text);
                });
                break;
        } 
    }
});

function numletterHelper(index) {
    var L = '';
    switch (index) {
        case 0:
            L = 'ა';
            break;
        case 1:
            L = 'ბ';
            break;
        case 2:
            L = 'გ';
            break;
        case 3:
            L = 'დ';
            break;
        case 4:
            L = 'ე';
            break;
        case 5:
            L = 'ვ';
            break;
    }
    return L;
} 