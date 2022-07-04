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

    jsMensajes.Metodos.EliminarRegistro("¿Desea Publicar el Indicador?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Indicador ha sido Publicado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
        });
});


$(document).on("click", JsPublicar.Controles.btnPublicar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea dejar de Publicar el Indicador?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Indicador ha sido Desactivado del modo Publicado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
        });
});
