//modificado: 17/02/2015 5:08pm Dusting Campos
//Default: msg="" type="info" position ="stack-topright"
//type params: info     error   alert   progress
//position params: stack-topright   stack-topleft   stack-bottomright   stack-bottomleft
var showNotification = function (msg, type) {
    var position = "stack-topright";
    msg = msg == undefined || msg == null ? "" : msg;
    type = type == undefined || type == null || type == "" ? "info" : type;
    //descomentar si se quiere pasar 'position' por parametro
    //position = position == undefined || position == null || position == "" ? "": position;
    var title = "Atención";
    var icon = "glyphicon glyphicon-info-sign";
    switch (type) {
        case 'success':
            title = 'Éxito!';
            icon = 'glyphicon glyphicon-ok-sign';
            break;
        case 'error': 
            title = 'Error';
            icon = "glyphicon glyphicon-remove-sign";
        break;
        case 'alert': 
            title = 'Alerta';
            icon = "glyphicon glyphicon-warning-sign";
        break;
        case 'progress': 
            progressNotification = new PNotify({
                title: msg,
                text: "",
                type: type,
                icon: "img-waiter",
                hide: false,
                addclass: position,
                buttons: {
                    closer: false,
                    sticker: false
                }
            });
            
        break;
    }
    if (type != "progress") {
        var myNotify = new PNotify({
            title: title,
            text: msg,
            type: type,
            icon: icon,
            hide: false,
            addclass: position,
            buttons: {
                closer_hover: true,
                sticker: false
            }
        });
        myNotify.get().click(function () {
            myNotify.remove();
        });
        setTimeout(function () { myNotify.remove();},5000)
    }
};
$.fn.removeNotification = function () {
    progressNotification.remove();
};

var showNotificationSuccess = function (onSuccessMessage) {
    var type = "success";
    onSuccessMessage = (onSuccessMessage == null || onSuccessMessage == undefined || onSuccessMessage == "") ? "La operación fue exitosa" : onSuccessMessage;
    showNotification(onSuccessMessage, type);
}

var showNotificationFail = function (onFailMessage) {
    var type = "error";
    onFailMessage = (onFailMessage == null || onFailMessage == undefined || onFailMessage == "") ? "Exite un error" : onFailMessage;
    showNotification(onFailMessage, type);
}