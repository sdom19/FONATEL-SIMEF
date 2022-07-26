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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/HistoricoFonatel/Index";
        });  

});


$(document).on("click", JsHistorico.Controles.btnDescargarHistorico, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Generar el Reporte?", "", "Generar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El reporte ha sido generado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        });

});



