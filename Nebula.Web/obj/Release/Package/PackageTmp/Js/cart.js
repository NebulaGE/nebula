/// <reference path="~/Scripts/knockout.js" />

function CartViewModel() {
    var self = this;

    self.total=ko.observable(0);
    self.totalString = ko.computed(function() { return  (self.total() / 100).toFixed(2).toString() + "₾" });

    self.engPrice = ko.observable(0);
    self.geoPrice = ko.observable(0);
    self.csPrice = ko.observable(0);

    self.geoPriceString = ko.observable();
    self.csPriceString = ko.observable();

    self.csSelected = ko.observable(false);
    self.geoSelected = ko.observable(false);
    self.engSelected = ko.observable(false);

    self.csPriceSaled = ko.observable(false);
    self.engPriceSaled = ko.observable(false);
    self.geoPriceSaled = ko.observable(false);

    self.engPriceAfterSale = ko.observable(0);
    self.geoPriceAfterSale = ko.observable(0);
    self.csPriceAfterSale = ko.observable(0);

    self.geoPriceAfterSaleString = ko.observable(0);
    self.csPriceAfterSaleString = ko.observable(0);

    self.coupon = ko.observable();

    self.init = function () {
        $.post(U + "Payment/GetPrices",
            {},
            function (resp) {
                self.geoPrice(resp.geoPrice);
                self.engPrice(resp.engPrice);
                self.csPrice(resp.csPrice);
                self.geoPriceString((self.geoPrice() / 100).toFixed(2).toString() + "₾");
                self.csPriceString((self.csPrice() / 100).toFixed(2).toString() + "₾");
                self.geoPriceAfterSaleString(geoPriceString());
                self.csPriceAfterSaleString(csPriceString()); 
            });
    }

    self.addToCart = function (param) { 
        if (param.p === 1 && !self.geoSelected()) {
            self.geoSelected(true);
            if (self.geoPriceSaled() !== true)
                self.total(self.total() + self.geoPrice());
            else
                self.total(self.total() + self.geoPriceAfterSale());
        }
        else if (param.p === 2 && !self.engSelected()) {
            self.engSelected(true);
        }
        else if (param.p === 3 && !self.csSelected()) { 
            self.csSelected(true);
            if (self.csPriceSaled() !== true) 
                self.total(self.total() + self.csPrice()); 
            else 
                self.total(self.total() + self.csPriceAfterSale()); 
        }
    }

    self.removeFromCart = function (param) {
        if (param.p === 1) {
            self.geoSelected(false);
            if (self.geoPriceSaled() !== true) 
                self.total(self.total() - self.geoPrice()); 
            else  
                self.total(self.total() - self.geoPriceAfterSale()); 
        }
        else if (param.p === 2) {
            self.engSelected(false);
        }
        else if (param.p === 3) {
            self.csSelected(false);
            if (self.csPriceSaled() !== true) 
                self.total(self.total() - self.csPrice()); 
            else 
                self.total(self.total() - self.csPriceAfterSale()); 
        }
    }

    self.checkCoupon = function () {
        $.post(U + "Payment/CheckCoupon",
        {
            couponCode: self.coupon()
        }, function (resp) {
            if (resp.status === "fail") {
                errorsAlert("კუპონის კოდი არასწორია");
            } 
            if (resp.status === "success") {
                if (resp.type === 0) { 
                    self.geoPriceAfterSale(resp.price);
                    self.geoPriceAfterSaleString((self.geoPriceAfterSale() / 100).toFixed(2).toString() + "₾");
                    self.geoPriceSaled(true);
                    if (self.geoSelected()===true) {
                        self.total(self.total() - self.geoPrice() + self.geoPriceAfterSale());
                    } 
                } else if (resp.type === 1) {
                    self.engPriceAfterSale(resp.price);
                    self.engPriceSaled(true);
                } else if (resp.type === 2) {
                    self.csPriceAfterSale(resp.price);
                    self.csPriceAfterSaleString((self.csPriceAfterSale() / 100).toFixed(2).toString() + "₾");
                    self.csPriceSaled(true);
                    if (self.csSelected()===true) {
                        self.total(self.total() - self.csPrice() + self.csPriceAfterSale());
                    } 
                }
            }
        });
    }

    self.generateTransactionId = function (form) {
        
        var formData = $(form).serialize();

        if (formData==="price=") {
            errorsAlert("გთხოვთ, შეიყვანოთ სასურველი თანხა");
        }

        $.post(U + 'Payment/GenerateTransactionId',
            formData,
            function(json) {
                if (json.error) {
                    errorsAlert(json.error);
                    return;
                }
                var transactionId = json.transactionId;
                $('#trans_id').val(transactionId);
                $('#tbcpay-btn').click();
            });
    };

    self.activateCourses = function () { 
        if (self.csSelected() || self.geoSelected()) { 
            $.post(U + "Payment/ActivateCourses", {
                cs: self.csSelected(),
                eng: self.engSelected(),
                geo: self.geoSelected()
            }, function (resp) {
                if (resp.success === 'balanceError') {
                    errorsAlert("ბალანსზე არ არის საკმარისი თანხა");
                } if (resp.success === "hasCsLicense") {
                    errorsAlert("ზოგადი უნარების კურსი უკვე გააქტიურებული გაქვს.");
                }
                if (resp.success === "hasGeoLicense") {
                    errorsAlert("ქართულის კურსი უკვე გააქტიურებული გაქვს.");
                }
                if (resp.success === "hasEngLicense") {
                    errorsAlert("ინგლისურის კურსი უკვე გააქტიურებული გაქვს.");
                }if (resp.success === "true") {
                    location.href = U + "PrivateBalance/Congrats";
                }
        });
        }
    }

    self.init();

    function errorsAlert(errorString) {
        $.alert({
            title: '',
            content: errorString,
            confirmButton: 'დახურვა',
            columnClass: 'col-md-4 col-md-offset-4'
        });
    } 
}


ko.applyBindings(new CartViewModel())