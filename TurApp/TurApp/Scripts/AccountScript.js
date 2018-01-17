$("#btnLogOff").click(function () {
    var base_url = window.location.origin;
    var token = $('input[name="__RequestVerificationToken"]').val();
    //debugger


    var options = [
        url = base_url + "/Account" + "/LogOff",
        __RequestVerificationToken= token
    ]


    $.ajax({
        url: url,
        type: 'POST',
        data: {
            __RequestVerificationToken: token
        },
        success: function (result) {
            document.write(result)
            //location.reload();
        }
    });
    //$.post(options);
});