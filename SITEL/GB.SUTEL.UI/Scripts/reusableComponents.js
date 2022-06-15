function AjaxifyMyForm(formName, ok, fail, timeOutMessage, especialValidation) {

    $("#" + formName).submit(function (ev) {
        ev.preventDefault();
        if (!$(this).valid()) {
            return false;
        }
        if (especialValidation != undefined) {
            if (!especialValidation(formName)) {
                return false;
            }
        }
        // obtiene el formulario
        var $form = $(this);
        // consulta donde poner los datos
        var target = $form.attr("data-ajax-target");
        // propiedad para mensaje de espera
        var loadingMessage = $form.attr("data-ajax-message");
        // arma el mensaje de cargando
        loadingMessage = loadingMessage == undefined || loadingMessage == "" ? "Cargando..." : loadingMessage;
        // coloca mensaje de cargando
        if(target != undefined && target != ""){
            //$(target).replaceWith("<div id='" + target.replace("#", "") + "'>" + loadingMessage + "</div>");
        }
        // mensaje de cargando en toast
        var message = $("#divLoading");
        //console.log($form.attr("data-ajax-message"));
        //message.html("<p>" + loadingMessage + "</p>");
        //message.show(500);
        
        //setTimeout(function () {
        //    message.hide(500);
        //}, timeOutMessage == undefined || timeOutMessage == null ? 3000 : timeOutMessage);
        //message.click(function () {
        //    $(this).hide(500);
        //});
        // extrae los atributos del form
        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(500);
        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize(),
            success: function (data) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                ok(data,formName);                
                //if (timeOutMessage == undefined)
                //    message.hide(500);
            },
            error: function (error) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                fail(error,formName);
                //if (timeOutMessage == undefined)
                //    message.hide(500);
            }
        };
        $.ajax(options);

        return false;
    });
    
}

addToast = function(options) {
    var message = options.msg;
    var title = options.title == undefined ? "ATENCIÓN" : options.title;
    var timeOut = options.timeOut;
    var dismiss = options.dismiss == undefined ? true : options.dismiss;

    var toast = $("<li class='toast'>")
        .append(
            $("<div class='title'>").append(title))
        .append(
            $("<div class='msg'>").append(message)
        );
    $("#toastStack").append(toast);
    toast.show();
    if (dismiss) {
        toast.click(function () {
            $(this).hide();
        });
        if (timeOut != undefined) {
            setTimeout(function () {
                toast.hide();
            }, timeOut);
        }
    }
    this.remove = function () {
        setTimeout(function () { $(toast).hide(500); }, 1000);
    };
}


$(document).ready(function () {

    $("#globalOk p span.close").click(function () {

        $("#globalOk").hide();

    })

});

function addSuccess(options) {

    var message = options.msg,

        title = options.title == undefined ? "Informativo" : options.title;

    var item = $("<li>").append("<p class='msg'>" +
                "<span class='icon'></span>" +
                "<span class='title'>" + title + "</span>" +
                "<span class='content'>" + message + "</span>" +
                "<span class='close'></span> " +
        "</p>"
        );
    item.click(function () { $(this).hide(); });
    $("#globalOk ul").append(item);

    setTimeout(function () {

        item.hide({
            complete: function () {
                item.remove();
            }});
    }, 6000);
        
        /*setTimeout(function () {

            $(this).hide();

            $("#globalOk p span.title").html('');
            $("#globalOk p span.content").html('');
        }, 6000);*/



}