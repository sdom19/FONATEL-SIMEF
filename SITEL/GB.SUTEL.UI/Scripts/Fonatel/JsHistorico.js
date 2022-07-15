    JsHistorico= {
        "Controles": {
            "btnCancelarHistorico":"#btnCancelarHistorico",
            "btnDescargarHistorico":"#btnDescargarHistorico"
    },
    "Variables":{

    },

    "Metodos": {

    }

}


$(document).on("click", JsHistorico.Controles.btnCancelarHistorico, function (e) {
    e.preventDefault();
    window.location.href = "/"

});


$(document).on("click", JsHistorico.Controles.btnDescargarHistorico, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Generar el Reporte?", "", "Generar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Reporte ha Sido Generado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        });

});



