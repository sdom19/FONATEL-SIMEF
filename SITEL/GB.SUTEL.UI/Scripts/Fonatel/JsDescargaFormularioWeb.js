JsDescargaFormularioWeb= {
    "Controles": {
        "btndescarga": "#btnDescargaFormularioWeb",
        "btnCancela": "#btnCancelaFormularioWeb"
    },
    "Variables": {
   
        
    },

    "Metodos": {
   
    }

}



$(document).on("click", JsDescargaFormularioWeb.Controles.btndescarga, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Descargar el Formulario", null ,"Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido Descargado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        });
});


$(document).on("click", JsDescargaFormularioWeb.Controles.btnCancela, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido Descargado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        })
});

