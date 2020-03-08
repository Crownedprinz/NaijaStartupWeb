function payWithPaystack() {
    var data = document.getElementById("edit-data-is-correct").checked;
    var email = document.getElementById("id_email").value;
    var amount = document.getElementById("id_amount").value;
    var phoneNumber = document.getElementById("id_phoneNumber").value;
    var key = "";
    $.ajax({
        type: "get",
        url: "/Dashboard/GetConfigurationValue",
        data: { "sectionName": "AppSettings", "paramName":"PayStackPublic" },
        success: function (parameterValue) {
            var json = JSON.stringify(parameterValue);
            var obj = JSON.parse(json);
            key = obj.parameter;
            if (key === "" || key === null)
                return swal({
                    title: "Invalid Details!",
                    text: "Invalid Paystack Key passed!",
                    icon: "error"
                });
            if (!data)
                return swal({
                    title: "Invalid Details!",
                    text: "Please make sure you check all boxes required!",
                    icon: "error"
                });
            var data1 =  document.getElementById("edit-term-and-condition").checked;
            if (!data1)
                return swal({
                    title: "Invalid Details!",
                    text: "Please make sure you check all boxes required!",
                    icon: "error"
                });
            var handler = PaystackPop.setup({
                key: key, //put your public key here
                email: email, //put your customer's email here
                amount: amount,
                currency: "NGN",
                metadata: {
                    custom_fields: [
                        {
                            display_name: "Mobile Number",
                            variable_name: "mobile_number",
                            value: phoneNumber//customer's mobile number
                        }
                    ]
                },
                callback: function (response) {
                    //after the transaction have been completed
                    //make post call  to the server with to verify payment
                    //using transaction reference as post data
                    $.ajax({
                        type: "post",
                        url: "/Dashboard/Verify_PayStack",
                        data: { "reference": response.reference },
                        success: function (data) {
                            if (data == "True") {
                                swal({
                                    title: "Successful Transaction!",
                                    text: "Transaction was successful",
                                    icon: "success"
                                });
                                window.location.href = "/Dashboard/all_companies";
                            }
                            else {
                                 swal({
                                    title: "Invalid Details!",
                                    text: response,
                                    icon: "error"
                                });

                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            swal({
                                title: "Invalid Details!",
                                text: xhr.status,
                                icon: "error"
                            });
                            swal({
                                title: "Invalid Details!",
                                text: thrownError,
                                icon: "error"
                            });
                        }
                    });
                },
                onClose: function () {
                    //when the user close the payment modal
                    swal({
                        title: "Invalid Details!",
                        text: "Transaction cancelled",
                        icon: "error"
                    });
                    alet('Transaction cancelled');
                }
            });
            handler.openIframe(); 
                //key = parameterValue;
        }
    });
    //open the paystack's payment modal
}