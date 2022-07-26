JsEditarFormularioWeb= {
    "Controles": {
        "btnBuscar": "#TablaEditarRegistroIndicador tbody tr td .btn-edit",
        "btndescarga": "#TablaEditarRegistroIndicador tbody tr td .btn-download",
        "btnCancela": "#btnCancelaFormularioWeb"
    },
    "Variables": {
   
        
    },

    "Metodos": {
   
    }

}

$(document).on("click", JsEditarFormularioWeb.Controles.btndescarga, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido Descargado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        });
});


$(document).on("click", JsEditarFormularioWeb.Controles.btnCancela, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido Descargado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        })
});











