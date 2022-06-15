$(document).ready(function () {
    $("[role='alert']").hide();    
    $("#LoginForm input").on('keyup', function () {
        $("#LoginForm .alert-danger").hide();
    });
    function onSuccess(data) {
        try {
            var url = document.URL.split("/");
            var allurl = url[0] + "//" + url[2] + "/" + JSON.parse(data).url;            
            window.location.href = allurl;
            $("body").append("<div class='darkScreen'><div class='img-waiter'></div></div>");
        } catch (ex) { }
    }
    //Modificado por:     Kevin Hernández Arias
    //Fecha Modificación: 31/01/2018
    //Detalle:            Cambio de Captcha
    function onFailure(error) {

        $(".darkScreen").remove();
        $("#LoginForm .alert-danger").html(error.statusText).show();
        $("#AccesoUsuario").val('');
        $("#Contrasena").val('');
        //Método para recargar el captcha
        LoginCaptcha.ReloadImage();
    }
    function showLoader() {

        $("[role='alert']").hide();
        $("body").append("<div class='darkScreen'><div class='img-waiter'></div></div>");
        return true;
    }

    AjaxifyMyForm("LoginForm", onSuccess, onFailure,null,showLoader);


    $(window).bind("capsOn", function (event) {

        if ($("#Contrasena:focus").length > 0) {
            $("#LoginForm .alert-warning").show();
        }
    });
    $(window).bind("capsOff capsUnknown", function (event) {
       
        $("#LoginForm .alert-warning").hide();
    });
    $("#Contrasena").bind("blur", function (event) {
 
        $("#LoginForm .alert-warning").hide();
    });
    $("#Contrasena").bind("focus", function (event) {
     
        if ($(window).capslockstate("state") === true) {
            $("#LoginForm .alert-warning").show();
        }
    });

    /* 
    * Initialize the capslockstate plugin.
    * Monitoring is happening at the window level.
    */
    $(window).capslockstate();

    initializePassForgot();
});

function initializePassForgot() {
    $('.modal').on('hidden.bs.modal', function () {
        $(this).find("[role='alert']").hide().html("");
        $(this).find("form").trigger("reset");
    });
    AjaxifyMyForm("formCambiarClave", successResetPass, function (error, formName) {
        $("#" + formName + " [name='error']").show().html(error.statusText);        
    });
    function successResetPass(data, formName) {
        var jsonData = JSON.parse(data);
        if (jsonData.state == 200) {
            $('.modal').modal('hide');
            addSuccess({ msg: jsonData.strMensaje });
        } else {
            $("#"+formName+" [name='error']").show().html(jsonData.strMensaje);
        }
         
    }
}