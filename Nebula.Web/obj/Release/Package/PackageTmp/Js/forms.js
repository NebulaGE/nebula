(function () {
    var FormsValidation = {
        required: function (val) {
            if (!val || !$.trim(val))
                return false;
            return true;
        },

        validLength: function (val, length) {
            if (val.length < length)
                return false;
            return true;
        },

        validateIdentificationNumber: function (privateNum) {
            if (privateNum.length != 11) {
                return false;
            }
            var re = new RegExp('^[0-9]+$');
            return re.test(privateNum);
        },

        validateEmail: function (email) {
            var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
            return re.test(email);
        },

        validateAlphabet: function (val) {
            if (!val.match(/^[a-zA-Z]+$/))
                return false;
            return true;
        },

        validateError: function (error) {
            '<li>' + error + '</li>'
        },

        IsdynamicValidationTrue: function (inputObj, bool) {
            bool ? $(inputObj).siblings('.glyphicon-ok').show().siblings('.glyphicon-remove').hide() : $(inputObj).siblings('.glyphicon-ok').hide().siblings('.glyphicon-remove').show()
        },
    }

    function ButtonLoader(bool, btnObj) {
        var btnLoader = $(btnObj).siblings('.btn-loader');
        bool ? btnLoader.show() : btnLoader.hide();
    }

    (function Register() {
        var $firstname = $('#r-firstname');
        var $lastname = $('#r-lastname');
        var $email = $('#r-email');
        var $password = $('#r-password');
        var $confirmPassword = $('#r-confirmpassword');

        $('#register-form').geoKeyboard();

        $('#registration').on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];


            ButtonLoader(true, $this);

            if (!FormsValidation.required($confirmPassword.val()) || !FormsValidation.validLength($confirmPassword.val(), 6) || $confirmPassword.val() != $password.val()) {
                valid = false;
                errors.push('გაიმეროეთ პაროლის ველი არ ემთხვევა პაროლის ველს');
            };

            if (!FormsValidation.required($firstname.val()) || !FormsValidation.validLength($firstname.val(), 2)) {
                valid = false;
                errors.push('სახელი  უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან');
            }

            if (!FormsValidation.required($lastname.val()) || !FormsValidation.validLength($lastname.val(), 4)) {
                valid = false;
                errors.push('გვარი უნდა შედგებოდეს მინიმუმ  4 სიმბოლოსგან');
            }

            if (!FormsValidation.required($password.val()) || !FormsValidation.validLength($password.val(), 6)) {
                valid = false;
                errors.push('პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან');
            }
            if (!FormsValidation.required($email.val()) || !FormsValidation.validateEmail($email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ.ფოსტა');
            }
            if (!$('input#register-checkbox').is(':checked')) {
                valid = false;
                errors.push('გთხოვთ დაეთანხმოთ წესებს');
            }
            if (valid) {
                $.post(U + 'Account/Register', $('#register-form').serialize(), function (response) {
                    if (response.Confirm) {
                        ButtonLoader(false, $this);
                        var successString = '<div style="text-align:center;">';

                        successString += '<p class="success-class">თქვენი ანგარიშის აქტივაციისთვის გთხოვთ შეამოწმოთ ელ.ფოსტა და გადახვიდეთ აქტივაციის ბმულზე</p>';

                        successString += '</div>';
                        successAlert(successString);
                        $this.prop('disabled', false);
                    } else {
                        if (response.Errors) {
                            errorString = '<div style="text-align:center;">';
                            $.each(response.Errors, function (i, v) {
                                errorString += '<p class="error-class">' + v + '</p>';
                            });
                            errorString += '</div>';
                            errorsAlert(errorString);
                        }
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);
                    }
                }).done(function () {

                });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });
                errorString += '</div>';

                errorsAlert(errorString);

                ButtonLoader(false, $this);

                $this.prop('disabled', false);
            }
        });

    })();

    (function Login() {
        var $email = $('#l-email');
        var $password = $('#l-password');

        $('#login').on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];

            ButtonLoader(true, $this);

            if (!FormsValidation.required($password.val()) || !FormsValidation.validLength($password.val(), 6)) {
                valid = false;
                errors.push('პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან');
            }
            if (!FormsValidation.required($email.val()) || !FormsValidation.validateEmail($email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ.ფოსტა');
            }

            if (valid) {
                $.post(U + 'Account/Login',
                    $('#login-form').serialize(),
                    function (response) {
                        if (response.Auth) {
                            if (response.showPdf == 'false') {

                                ButtonLoader(false, $this);
                                location.href = U + 'Home/Index?showPdf=true';
                                return false;
                            }

                            location.href = $('#return-url').val();
                        } else {
                            ButtonLoader(false, $this);
                            $this.prop('disabled', false);

                            if (response.Error) {
                                var errorString = '<div style="text-align:center;">';

                                errorString += '<p class="error-class">' + response.Error + '</p>';

                                errorString += '</div>';
                                errorsAlert(errorString);
                            }
                        }
                    });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });
                errorString += '</div>';
                errorsAlert(errorString);

                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });

    })();

    (function EmailConfirmation() {

        var $registerSuccess = $('#confirm-success');
        var $email = $('#rc-email');
        $('#email-confirm').on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];

            ButtonLoader(true, $this);

            if (!FormsValidation.required($email.val()) || !FormsValidation.validateEmail($email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ ფოსტა');
            }

            if (valid) {
                $.post(U + 'Account/ExternalLoginConfirmation', $('#email-confirm-form').serialize(), function (response) {
                    if (response.Confirm) {
                        ButtonLoader(false, $this);

                        $this.prop('disabled', false);

                        var successString = '<div style="text-align:center;">';
                        successString += '<p class="success-class">თქვენი ანგარიშის აქტივაციისთვის გთხოვთ შეამოწმოთ ელ.ფოსტა და გადახვიდეთ აქტივაციის ბმულზე</p>';
                        successString += '</div>';
                        successAlert(successString);
                    } else {
                        if (response.Errors) {
                            var errorString = '<div style="text-align:center;">';
                            $.each(response.Errors, function (i, v) {
                                errorString += '<p class="error-class">' + v + '</p>';
                            });
                            errorString += '</div>';
                            errorsAlert(errorString);
                        }
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);
                    }
                }).done(function () {

                });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });
                errorString += '</div>';
                errorsAlert(errorString);
                ButtonLoader(false, $this);

                $this.prop('disabled', false);
            }
        });
    })();

    (function UpdatePassword() {
        var currentPassword = $('#current-password');
        var newPassword = $('#new-password');
        var confirmPassword = $('#confirm-password');

        $('#update-pass')
            .on('click',
                function (e) {
                    var valid = true;
                    var $this = $(this);
                    e.preventDefault();
                    $this.prop('disabled', true);
                    ButtonLoader(true, $this);
                    var errors = [];

                    if (!FormsValidation.required(currentPassword.val()) ||
                        !FormsValidation.validLength(currentPassword.val(), 6)) {
                        valid = false;
                        errors.push('ძველი პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან');
                    }

                    if (!FormsValidation.required(newPassword
                            .val()) ||
                        !FormsValidation.validLength(newPassword.val(), 6)) {
                        valid = false;
                        errors.push('ახალი პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან');
                    }

                    if (!FormsValidation.required(confirmPassword.val()) ||
                        !FormsValidation.validLength(confirmPassword.val(), 6) ||
                        confirmPassword.val() != newPassword.val()) {
                        valid = false;
                        errors.push('გაიმეროეთ პაროლის ველი არ ემთხვევა პაროლის ველს');

                    };

                    if (valid) {
                        $.post(U + 'Account/ChangePassword',
                            $('#change-password-form').serialize(),
                            function (response) {
                                if (response.Confirm) {

                                    ButtonLoader(false, $this);
                                    $this.prop('disabled', false);

                                    var successString = '<div style="text-align:center;">';
                                    successString += '<p class="success-class">პაროლი წარმატებით შეიცვალა</p>';
                                    successString += '</div>';
                                    successAlert(successString);


                                } else {

                                    var errorString = '<div style="text-align:center;">';
                                    if (response.errors) {
                                        $.each(response.errors,
                                            function (i, v) {
                                                errorString += '<p class="error-class">' + v + '</p>';
                                            });
                                        errorString += '</div>';
                                        errorsAlert(errorString);
                                    }
                                    ButtonLoader(false, $this);
                                    $this.prop('disabled', false);
                                }
                            });

                    } else {
                        var errorString = '<div style="text-align:center;">';
                        $.each(errors,
                            function (i, v) {
                                errorString += '<p class="error-class">' + v + '</p>';
                            });
                        errorString += '</div>';

                        errorsAlert(errorString);
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);
                    }
                });
    })();
    
    (function UpdateUserInfo() {
        var telInput = $("#update-phone");
        $('#update-user-info-form').geoKeyboard();
        if (telInput.length != 0)
            telInput.intlTelInput({
                initialCountry: "auto",
                nationalMode: true,
                geoIpLookup: function (callback) {
                    $.get('http://ipinfo.io', function() {}, "jsonp")
                        .always(function(resp) {
                            var countryCode = (resp && resp.country) ? resp.country : "";
                            callback(countryCode);
                        });
                },
                utilsScript: U + 'Scripts/utils.js' // just for formatting/placeholders etc
            }).done(function () {
                telInput.val($('#user-number').val());
            });

        $('#update-info').on('click', function (e) { 
                    var identificationNumber = $('#update-identification-number');
                    var $firstname = $('#update-firstname');
                    var $lastname = $('#update-lastname');
                    var email = $('#update-email');

                    var valid = true;
                    var $this = $(this);
                    e.preventDefault();
                    $this.prop('disabled', true);
                    ButtonLoader(true, $this);
                    var errors = [];

                    if (!telInput.intlTelInput("isValidNumber") || !FormsValidation.required(telInput.val())) {
                        valid = false;
                        errors.push('გთხოვთ, სწორად შეიყვანოთ მობილურის ნომერი');
                    }

                    if (!FormsValidation.validateIdentificationNumber(identificationNumber.val())) {
                        valid = false;
                        errors.push('გთხოვთ, სწორად შეიყვანოთ პირადი ნომერი');
                    }

                    if (!FormsValidation.required($firstname
                        .val()) ||
                    !FormsValidation.validLength($firstname.val(), 2)) {
                        valid = false;
                        errors.push('სახელი  უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან');
                    }

                    if (!FormsValidation.required($lastname
                        .val()) ||
                    !FormsValidation.validLength($lastname.val(), 4)) {
                        valid = false;
                        errors.push('გვარი  უნდა შედგებოდეს მინიმუმ 4 სიმბოლოსგან');
                    }

                    if (!FormsValidation.required(email.val()) || !FormsValidation.validateEmail(email.val())) {
                        valid = false;
                        errors.push('გთხოვთ, სწორად შეიყვანოთ ელ ფოსტა');
                    }

                    if (valid) {
                        $.post(U + 'Account/UpdateUserInfo',
                            $('#update-user-info-form').serialize(),
                            function(response) {
                                if (response.Confirm) {
                                    ButtonLoader(false, $this);
                                    $this.prop('disabled', false);

                                    var successString = '<div style="text-align:center;">';
                                    successString += '<p class="success-class">მონაცემები წარმატებით განახლდა</p>';
                                    successString += '</div>';
                                    successAlert(successString);
                                } else {
                                    var errorString = '<div style="text-align:center;">';

                                    errorString += '<p class="error-class">' + response.errors + '</p>';
                                    errorString += '</div>';
                                    errorsAlert(errorString);

                                    ButtonLoader(false, $this);
                                    $this.prop('disabled', false);
                                }
                            });
                    } else {
                        var errorString = '<div style="text-align:center;">';
                        $.each(errors,
                            function(i, v) {
                                errorString += '<p class="error-class">' + v + '</p>';
                            });

                        errorString += '</div>';
                        errorsAlert(errorString);
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);
                    }
                });
    })();

    (function UpdateSocialInfo() {
        $('#social-form').geoKeyboard();

        $('#social-submit')
            .on('click',
                function(e) {
                    e.preventDefault();
                    var $firstname = $('#social-firstname');
                    var $lastname = $('#social-lastname');
                    var $phone = $('#social-phone');
                    var $identity = $('#social-identity');
                    var $socText = $('#social-text');
                    var $userFile = document.getElementById('social-file');

                    var valid = true;
                    var $this = $(this);

                    $this.prop('disabled', true);
                    ButtonLoader(true, $this);
                    var errors = [];

                    if (!FormsValidation.required($firstname
                        .val()) || !FormsValidation.validLength($firstname.val(), 2)) {
                        valid = false;
                        errors.push('სახელი  უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან');
                    }

                    if (!FormsValidation.required($lastname
                        .val()) || !FormsValidation.validLength($lastname.val(), 4)) {
                        valid = false;
                        errors.push('გვარი  უნდა შედგებოდეს მინიმუმ 4 სიმბოლოსგან');
                    }

                    if (!FormsValidation.required($phone.val())) {
                        valid = false;
                        errors.push('გთხოვთ  შეიყვანეთ ტელეფონის ნომერი');
                    }

                    if (!FormsValidation.required($identity
                        .val()) || !FormsValidation.validLength($identity.val(), 11)) {
                        valid = false;
                        errors.push('გთხოვთ სწორად შეიყვანეთ პირადი ნომერი');
                    }

                    if (!FormsValidation.required($socText.val())) {
                        valid = false;
                        errors.push('სოციალური ტექსტის შეყვანა სავალდებულოა');
                    }

                    if ($userFile.value == "") {
                        valid = false;
                        errors.push('გთხოვთ აირჩიოთ ფაილი');
                    }

                    if (!$('input#social-checkbox').is(':checked')) {
                        valid = false;
                        errors.push('გთხოვთ დაეთანხმოთ წესებს');
                    }

                    if (valid) {
                        document.getElementById("social-form").submit();
                    } else {
                        var errorString = '<div style="text-align:center;">';
                        $.each(errors,
                            function(i, v) {
                                errorString += '<p class="error-class">' + v + '</p>';
                            });

                        errorString += '</div>';
                        errorsAlert(errorString);
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);
                    }
                });
    })();

    (function TBCPay() {
        var telInput = $("#payment-phone-number");
        if (telInput.length != 0)
            telInput.intlTelInput({
                initialCountry: "auto",
                nationalMode: true,
                geoIpLookup: function (callback) {
                    $.get('http://ipinfo.io', function() {}, "jsonp")
                        .always(function(resp) {
                            var countryCode = (resp && resp.country) ? resp.country : "";
                            callback(countryCode);
                        });
                },
                utilsScript: U + 'Scripts/utils.js' // just for formatting/placeholders etc
            }).done(function () {
                telInput.val($('#user-number').val());
            });

        $('#payment-form').geoKeyboard();

        $('#tbc-pay-submit').on('click', function (e) {
            var identificationNumber = $('#payment-identification');
            var $firstname = $('#payment-firstname');
            var $lastname = $('#payment-lastname');
            var email = $('#payment-email');

            var valid = true;
            var $this = $(this);
            e.preventDefault();
            $this.prop('disabled', true);
            ButtonLoader(true, $this);
            var errors = [];

            if (!telInput.intlTelInput("isValidNumber") || !FormsValidation.required(telInput.val())) {
                valid = false;
                errors.push('შეიყვანეთ მობილურის ნომერი  სწორად');
            }

            if (!FormsValidation.validateIdentificationNumber(identificationNumber.val())) {
                valid = false;
                errors.push('შეიყვანეთ პირადი ნომერი  სწორად');
            }

            if (!FormsValidation.required($firstname.val()) || !FormsValidation.validLength($firstname.val(), 2)) {
                valid = false;
                errors.push('სახელი  უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან'); 
            }

            if (!FormsValidation.required($lastname.val()) || !FormsValidation.validLength($lastname.val(), 4)) {
                valid = false;
                errors.push('გვარი  უნდა შედგებოდეს მინიმუმ 4 სიმბოლოსგან'); 
            }

            if (!FormsValidation.required(email.val()) || !FormsValidation.validateEmail(email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ ფოსტა'); 
            }

            if (valid) {
                $.post(U + 'Payment/GenerateTransactionId', $('#payment-form').serialize(), function (response) {
                    if (response.transactionId) {
                        $('#trans_id').val(response.transactionId);

                        setTimeout(function () {
                            $('#tbcpay-btn').click();
                        }, 200);
                    } else {
                        var errorString = '<div style="text-align:center;">';

                        if (response.error) {
                            errorString += '<p class="error-class">' + response.error + '</p>';
                            errorString += '</div>';
                            errorsAlert(errorString);
                        }
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);
                    }
                });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });

                errorString += '</div>';
                errorsAlert(errorString);
                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });
    })();

    (function NOVAPay() {
        var telInput = $("#payment-phone-number");

        $('#nova-pay-submit').on('click', function (e) {
            var identificationNumber = $('#payment-identification');
            var $firstname = $('#payment-firstname');
            var $lastname = $('#payment-lastname');
            var email = $('#payment-email');

            var valid = true;
            var $this = $(this);
            e.preventDefault();
            $this.prop('disabled', true);
            ButtonLoader(true, $this);
            var errors = [];

            if (!telInput.intlTelInput("isValidNumber") || !FormsValidation.required(telInput.val())) {
                valid = false;
                errors.push('შეიყვანეთ მობილურის ნომერი  სწორად');
            }

            if (!FormsValidation.validateIdentificationNumber(identificationNumber.val())) {
                valid = false;
                errors.push('შეიყვანეთ პირადი ნომერი  სწორად');
            }

            if (!FormsValidation.required($firstname.val()) || !FormsValidation.validLength($firstname.val(), 2)) {
                valid = false;
                errors.push('სახელი  უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან'); 
            }

            if (!FormsValidation.required($lastname.val()) || !FormsValidation.validLength($lastname.val(), 4)) {
                valid = false;
                errors.push('გვარი  უნდა შედგებოდეს მინიმუმ 4 სიმბოლოსგან'); 
            }
            
            if (!FormsValidation.required(email.val()) || !FormsValidation.validateEmail(email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ ფოსტა'); 
            }

            if (valid) {
                $.post(U + 'Payment/UpdateUserInfo', $('#payment-form').serialize(), function (response) {
                    if (response.success) {
                        $this.prop('disabled', false);
                        ButtonLoader(false, $this);
                        $('#id-nmb').html(identificationNumber.val());
                        $('.nova-pay-popup').fadeIn();
                    } else {
                        var errorString = '<div style="text-align:center;">';

                        if (response.error) {
                            errorString += '<p class="error-class">' + response.error + '</p>';
                            errorString += '</div>';
                            errorsAlert(errorString);
                        }
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);
                    }
                });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });

                errorString += '</div>';
                errorsAlert(errorString);
                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });
    })();

    (function SendEmail() {
        $(document).on('click', '#contact-send', function (e) {
            e.preventDefault();
            var $this = $(this);

            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();

            var $email = $('#contact-email');
            var $text = $('#contact-text');

            var valid = true;
            $this.prop('disabled', true);
            var errors = []; 

            ButtonLoader(true, $this);

            if (!FormsValidation.required($email.val()) || !FormsValidation.validateEmail($email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ ფოსტა');
            }
            if (!FormsValidation.required($text.val()) || !FormsValidation.validLength($text.val(), 15)) {
                valid = false;
                errors.push('ტექსტი უნდა შედგებოდეს მინიმუმ 15 სიმბოლოსგან');
            }

            if (valid) {
                $.post(U + 'Home/SendEmail',
                    {
                        __RequestVerificationToken: token,
                        email: $email.val(),
                        text: $text.val()
                    },
                    function(response) {
                        if (response.send) {
                            successAlert('წერილი წარმატებით გაიგზავნა!');
                            $this.prop('disabled', false);
                        } else {
                            errorsAlert('სამწუხაროდ წერილი ვერ გაიგზავნა,გთხოვთ სცადეთ ხელახლა');
                            $this.prop('disabled', false);
                        }
                    });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });

                errorString += '</div>';
                errorsAlert(errorString);

                ButtonLoader(false, $this); 
                $this.prop('disabled', false);
            }
        });
    })();

    (function ForgotPasswordSendForm() {
        $('#fg-send-submit').on('click', function (e) {
            var $email = $('#fg-send-email');
            e.preventDefault();
            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];

            ButtonLoader(true, $this);
            
            if (!FormsValidation.required($email.val()) || !FormsValidation.validateEmail($email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ.ფოსტა');
            }

            if (valid) {
                $.post(U + 'Account/ForgotPassword',
                    $('#fg-send-form').serialize(),
                    function(response) {
                        ButtonLoader(false, $this);
                        $this.prop('disabled', false);

                        if (response.success) {
                            successAlert('გთხოვთ შეამოწმოთ ელ.ფოსტა');
                        } else if (response.error) {
                            errorsAlert(response.error);
                        } else {
                            errorsAlert('მოხდა სისტემური შეცდომა სცადეთ ხელახლა');
                        }
                    });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });
                errorString += '</div>';
                errorsAlert(errorString);

                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });
    })();

    (function ForgotPassword() {
        $('#forgot-password-submit').on('click', function (e) {

            var $email = $('#forgot-password-emal');
            var $password = $('#forgot-password-password');
            var $rePassword = $('#forgot-password-password-re');

            e.preventDefault();
            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];

            ButtonLoader(true, $this);

            if (!FormsValidation.required($email.val()) || !FormsValidation.validateEmail($email.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ელ.ფოსტა');
            }

            if (!FormsValidation.required($password.val()) || !FormsValidation.validLength($password.val(), 6)) {
                valid = false;
                errors.push('პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან');
            }

            if (!FormsValidation.required($rePassword.val()) || !FormsValidation.validLength($rePassword.val(), 6) || $rePassword.val() != $password.val()) {
                valid = false;
                errors.push('გაიმეროეთ პაროლის ველი არ ემთხვევა პაროლის ველს');
            };

            if (valid) {
                $.post(U + 'Account/ResetPassword',
                    $('#forgot-password-form').serialize(),
                    function(response) {
                        if (response.success) {
                            ButtonLoader(false, $this);
                            $this.prop('disabled', false);
                            successAlert(response.success);
                        } else {
                            ButtonLoader(false, $this);
                            $this.prop('disabled', false);
                            errorsAlert('სამწუხაროდ პაროლი ვერ შეიცვალა, სცადეთ ხელახლა');
                        }
                    });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });

                errorString += '</div>';
                errorsAlert(errorString);

                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });
    })();

    (function TeacherRegister() {
        $('#teacher-reg').geoKeyboard();

        $('#teacher-reg-submit').on('click', function (e) {
            e.preventDefault();

            var $firstname = $('#teacher-firstname');
            var $lastname = $('#teacher-lastname');
            var $email = $('#teacher-email');
            var $password = $('#teacher-password');
            var $confirmPassword = $('#teacher-re-password');
            var $identity = $('#teacher-identity');
            var $phone = $('#teacher-phone');

            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];

            ButtonLoader(true, $this);

            if (!FormsValidation.required($firstname.val()) || !FormsValidation.validLength($firstname.val(), 2)) {
                valid = false;
                errors.push('სახელი  უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან');
            }

            if (!FormsValidation.required($lastname.val()) || !FormsValidation.validLength($lastname.val(), 4)) {
                valid = false;
                errors.push('გვარი უნდა შედგებოდეს მინიმუმ  4 სიმბოლოსგან');
            }

            if ($email.val()) {
                if (!FormsValidation.validateEmail($email.val())) {
                    valid = false;
                    errors.push('გთხოვთ, სწორად შეიყვანოთ ელ.ფოსტა');
                }
            }

            if ($phone.val().length != 11) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ ტელეფონის ნომერი');
            }

            if (!FormsValidation.validateIdentificationNumber($identity.val())) {
                valid = false;
                errors.push('გთხოვთ, სწორად შეიყვანოთ პირადი ნომერი');
            }

            if (!FormsValidation.required($password.val()) || !FormsValidation.validLength($password.val(), 6)) {
                valid = false;
                errors.push('პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან');
            }

            if ($confirmPassword.val() != $password.val()) {
                valid = false;
                errors.push('გაიმეროეთ პაროლის ველი არ ემთხვევა პაროლის ველს');
            };

            if (valid) {
                $.post(U + 'Teacher/Register',
                    $('#teacher-reg').serialize(),
                    function(response) {
                        if (response.success) {
                            ButtonLoader(false, $this);
                            location.href = U + 'Teacher/Mystudents';
                        } else {
                            if (response.error) {
                                errorString = '<div style="text-align:center;">';
                                errorString += '<p class="error-class">' + response.error + '</p>';
                                errorString += '</div>';
                                errorsAlert(errorString);
                            }
                            ButtonLoader(false, $this);
                            $this.prop('disabled', false);
                        }
                    });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });

                errorString += '</div>';
                errorsAlert(errorString);
                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });
    })();

    (function TeacherLogin() {
        $('#teacher-log').geoKeyboard();

        $('#teacher-log-submit').on('click', function (e) {
            e.preventDefault();

            var $firstname = $('#teacher-log-firstname');
            var $lastname = $('#teacher-log-lastname');
            var $password = $('#teacher-log-password');

            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];

            ButtonLoader(true, $this);

            if (!FormsValidation.required($firstname.val()) || !FormsValidation.validLength($firstname.val(), 2)) {
                valid = false;
                errors.push('სახელი  უნდა შედგებოდეს მინიმუმ 2 სიმბოლოსგან');
            }

            if (!FormsValidation.required($lastname.val()) || !FormsValidation.validLength($lastname.val(), 4)) {
                valid = false;
                errors.push('გვარი უნდა შედგებოდეს მინიმუმ  4 სიმბოლოსგან');
            }

            if (!FormsValidation.required($password.val()) || !FormsValidation.validLength($password.val(), 6)) {
                valid = false;
                errors.push('პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან');
            }

            if (valid) {
                $.post(U + 'Teacher/Login',
                    $('#teacher-log').serialize(),
                    function(response) {
                        if (response.success) {
                            ButtonLoader(false, $this);
                            location.href = U + 'Teacher/Mystudents';
                        } else {
                            if (response.error) {
                                errorString = '<div style="text-align:center;">';
                                errorString += '<p class="error-class">' + response.error + '</p>';
                                errorString += '</div>';
                                errorsAlert(errorString);
                            }
                            ButtonLoader(false, $this);
                            $this.prop('disabled', false);
                        }
                    });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });

                errorString += '</div>';
                errorsAlert(errorString);
                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });
    })();

    (function CompetitionForm() {
        $('#competition-submit').on('click', function (e) {
            e.preventDefault();

            var $answer = $('#competition-answer');
            var $phone = $('#competition-phone');

            var $this = $(this);
            var valid = true;
            $this.prop('disabled', true);
            var errors = [];

            if (!FormsValidation.required($answer.val()) || !FormsValidation.validLength($answer.val(), 2)) {
                valid = false;
                errors.push('სწორი პასუხის შეყვანა სავალდებულოა');
            }

            if (!FormsValidation.required($phone.val()) || !FormsValidation.validLength($phone.val(), 11)) {
                valid = false;
                errors.push('გთხოვთ შეიყვანოთ ტელეფონის ნომერი სწორად');
            }

            if (valid) {
                $.post(U + 'Ajax/Competition',
                    {
                        phone: $phone.val(),
                        answer: $answer.val(),
                        competitionId: $('#competition-id').val()
                    },
                    function(response) {
                        if (response.success) {
                            ButtonLoader(false, $this);
                            var successString = '<div style="text-align:center;">';
                            successString += '<p class="success-class">' + response.success + '</p>';
                            successString += '</div>';
                            successAlert(successString);
                            $this.prop('disabled', false);
                        } else {
                            if (response.error) {
                                var errorString = '<div style="text-align:center;">';
                                errorString += '<p class="error-class">' + response.error + '</p>';
                                errorString += '</div>';
                                errorsAlert(errorString);
                            }
                            ButtonLoader(false, $this);
                            $this.prop('disabled', false);
                        }
                    });
            } else {
                var errorString = '<div style="text-align:center;">';
                $.each(errors, function (i, v) {
                    errorString += '<p class="error-class">' + v + '</p>';
                });

                errorString += '</div>';
                errorsAlert(errorString);
                ButtonLoader(false, $this);
                $this.prop('disabled', false);
            }
        });
    })();

    function errorsAlert(errorString) {
        $.alert({
            title: '',
            content: errorString,
            confirmButton: 'დახურვა',
            columnClass: 'col-md-4 col-md-offset-4'
        });
    }

    function successAlert(successString) {
        $.alert({
            title: '',
            content: successString,
            confirmButton: 'დახურვა',
            columnClass: 'col-md-4 col-md-offset-4'
        });
    }

    $(document).ready(function () { 
        if ($('#socSucces').val() === 'succes') { 
            var successString = '<div style="text-align:center;">';

            successString += '<p class="success-class">მონაცემები წარმატებით გაიგზავნა, დადებითი პასუხის შემთხვევაში დაგიკავშირდებით, თქვენი ანგარიშის აქტივაციისთვის გთხოვთ შეამოწმოთ ელ.ფოსტა და გადახვიდეთ აქტივაციის ბმულზე</p>';

            successString += '</div>';
            successAlert(successString);
        } 
        else if ($('#socSucces').val() === 'succesReg') { 
            var successStringr = '<div style="text-align:center;">';

            successStringr += '<p class="success-class">მონაცემები წარმატებით გაიგზავნა, დადებითი პასუხის შემთხვევაში დაგიკავშირდებით</p>';

            successStringr += '</div>';
            successAlert(successStringr);
        }
    }); 
})();