
//function verbalConfirm(showMathPopup) {
//    $("#verbalConfirm").confirm({
//        title: "შედეგების დადასტურება",
//        text: "ვერბალურ ნაწილში 15-ზე მეტ კითხვაზე პასუხი გადაუტანელი გაქვთ, გსურთ თუ არა შედეგების ასახვა?",
//        confirm: function (button) {

//            $.post(U + 'Exam/ConfirmResult', { quizId: quizId, typeId: 1, confirmed : true }, function (response) {
//                if (response.done) {
//                    if (showMathPopup) {
//                        setTimeout(function () {
//                            mathConfirm();
//                        }, 500);
//                    }
//                } else if (response.error) {
//                    alert(response.error);
//                } else {
//                    alert('network error');
//                }

//            })

//        },
//        cancel: function (button) {

//            $.post(U + 'Exam/ConfirmResult', { quizId: quizId, typeId: 1, confirmed: false }, function (response) {
//                if (response.error) {
//                    alert(response.error);
//                } else if (response.done) {

//                }
//                else {
//                    alert('network error');
//                }
//            }).done(function () {
//                if (showMathPopup) {
//                    setTimeout(function () {
//                        mathConfirm();
//                    }, 500)
//                }
//            })

//        },
//        confirmButton: "დადასტურება",
//        cancelButton: "გაუქმება"
//    });
//    $('#verbalConfirm').click();
//}


////math 
//function mathConfirm() {
//    $("#mathConfirm").confirm({
//        title: "შედეგების დადასტურება",
//        text: "მათემატიკურ ნაწილში 15-ზე მეტ კითხვაზე პასუხი გადაუტანელი გაქვთ, გსურთ თუ არა შედეგების ასახვა?",
//        confirm: function (button) {
//            $.post(U + 'Exam/ConfirmResult', { quizId: quizId, typeId: 2, confirmed : true }, function (response) {
//                if (response.error) {
//                    alert(response.error);
//                } else if (response.done) {

//                }
//                else {
//                    alert('network error');
//                }
//            })

//        },
//        cancel : function(button){
//            $.post(U + 'Exam/ConfirmResult', { quizId: quizId, typeId: 2, confirmed: false }, function (response) {
//                if (response.error) {
//                    alert(response.error);
//                } else if (response.done) {

//                }
//                else {
//                    alert('network error');
//                }
//            })
//        },
//        confirmButton: "დადასტურება",
//        cancelButton: "გაუქმება"
//    });
//    $('#mathConfirm').click();
//}