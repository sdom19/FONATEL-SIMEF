JsPublicar= {
    "Controles": {

        "btnPublicar": "#TablaPublicar tbody tr td .btn-power-on",
        "btnEliminarPublicacion": "#TablaPublicar tbody tr td .btn-power-off",
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}

$(document).on("click", JsPublicar.Controles.btnEliminarPublicacion, function () {

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Publicar el Indicador?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido Publicado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
        });
});


$(document).on("click", JsPublicar.Controles.btnPublicar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea dejar de Publicar el Indicador?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido Desactivado del modo Publicado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
        });
});
