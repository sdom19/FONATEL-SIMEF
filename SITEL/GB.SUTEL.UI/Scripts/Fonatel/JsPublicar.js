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

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea publicar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido Publicado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
        });
});


$(document).on("click", JsPublicar.Controles.btnPublicar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la publicación del Indicador?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido desactivado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
        });
});
