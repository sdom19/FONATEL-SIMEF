JsDefiniciones= {
    "Controles": {
        
        "btnEditarDefiniciones": "#TablaDefiniciones tbody tr td .btn-edit",
        "btnAddDefiniciones": "#TablaDefiniciones tbody tr td .btn-add",
        "btnDeleteDefiniciones": "#TablaDefiniciones tbody tr td .btn-delete",
        "btnCloneDefiniciones": "#TablaDefiniciones tbody tr td .btn-clone",
        "btnGuardar": "#btnGuardarDefiniciones",
        "btnCancelar":"#btnCancelarDefiniciones"
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}

$(document).on("click", JsDefiniciones.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/DefinicionIndicadores/Index";
        });
});

$(document).on("click", JsDefiniciones.Controles.btnAddDefiniciones, function () {
    window.location.href = "/Fonatel/DefinicionIndicadores/Create";
});


$(document).on("click", JsDefiniciones.Controles.btnEditarDefiniciones, function () {
    let id = 1;
    window.location.href = "/Fonatel/DefinicionIndicadores/Create?id=" + id;
});

$(document).on("click", JsDefiniciones.Controles.btnCloneDefiniciones, function () {
    let id = 1;
    window.location.href = "/Fonatel/DefinicionIndicadores/Create?id=" + id;
});






$(document).on("click", JsDefiniciones.Controles.btnGuardar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Definicion?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Definición ha sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
        });
});

$(document).on("click", JsDefiniciones.Controles.btnDeleteDefiniciones, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina la Definición?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Definición ha sido eliminada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
        });
});









