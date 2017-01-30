function PaymentViewModel() {
    var self = this;

     self.generateTransactionId = function(form) {
         var formData = $(form).serialize();

         $.post(U + 'Payment/GenerateTransactionId', formData, function (json) {
             if (json.error) {
                 alert(json.error);
                 return;
             }
             var transactionId = json.transactionId;
             $('#trans_id').val(transactionId);
             $('#tbcpay-btn').click();
         });
     }
}

ko.applyBindings(new PaymentViewModel())