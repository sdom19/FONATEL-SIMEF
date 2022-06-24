$(document).ready(function () {
    AjaxifyMyForm("formResetPassword", onSuccess, onFailure, null, validateForm);
    $('.required').rules('add', {
        required: true
    });
    $("[name='Contrasena']").on("blur", validateMatchingPass);
    $("[name='passRepeat']").on("blur", validateMatchingPass);
    $("[name='oldPassword']").on("keyup", function () {
        $("[name='oldPassword']").parent().removeClass("has-error");
    });


    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);            
            return this.optional(element) || re.test(value);
        },
        closureMessage().RegExpPassword
    );
    $('[name="Contrasena"]').rules("add", {
        regex: "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{5,15}$"
    })

    initCaps();

});
function onSuccess(data, formName) {
    var jData = JSON.parse(data);    
    $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
    if (jData.state == 200) {
        var url = document.URL.split("/");
        var allurl = url[0] + "//" + url[2] + "/" + JSON.parse(jData.strMensaje).url;
        addSuccess({ msg: JSON.parse(jData.strMensaje).msg });
        $("#" + formName + " input").prop("disabled", true);
        window.location.href = allurl;
    } else {
        $("[name='error']").show().html(jData.strMensaje);
        if(jData.state==405)
            if (!$("[name='oldPassword']").parent().hasClass("has-error"))
                $("[name='oldPassword']").parent().addClass("has-error");
    }

}
function onFailure(error, formName) {
    $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
    $("[name='error']").show().html(error.statusText);
}
function validateForm(formName) {
    $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message")).prop("disabled", true);
    return validateMatchingPass();
}
function validateMatchingPass() {    
    var newPass = $("[name='Contrasena']").val(); 
    var matchPass = $("[name='passRepeat']").val(); 
    if (newPass == matchPass) {
        $("[name='passRepeat']").parent().removeClass("has-error");
        $('[data-valmsg-for="passRepeat"]').html("");
        return true;
    }
    else {
        if(!$("[name='passRepeat']").parent().hasClass("has-error"))
            $("[name='passRepeat']").parent().addClass("has-error");
        $('[data-valmsg-for="passRepeat"]').html(closureMessage().DismatchPasswords);
        $("#formResetPassword [type='submit']").val($("#formResetPassword").attr("data-ajax-message-default")).prop("disabled", false);
        return false;
    }
}

function initCaps() {
    $(".hiddenPassword").on('keyup', function () {
        $(".alert-danger").hide();
    });

    $(window).bind("capsOn", function (event) {
        $(".alert-warning").show();
    });
    $(window).bind("capsOff capsUnknown", function (event) {
        $(".alert-warning").hide();
    });
    //$(".hiddenPassword").bind("blur", function (event) {
    //    $(".alert-warning").hide();
    //});
    $(".hiddenPassword").bind("focus", function (event) {
        if ($(window).capslockstate("state") === true) {
            $(".alert-warning").show();
        }
    });

    /* 
    * Initialize the capslockstate plugin.
    * Monitoring is happening at the window level.
    */
    $(window).capslockstate();
}
