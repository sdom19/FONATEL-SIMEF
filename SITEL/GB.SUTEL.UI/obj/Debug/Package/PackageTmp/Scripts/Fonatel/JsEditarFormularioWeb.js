JsEditarFormularioWeb= {
    "Controles": {
        "btnBuscar": "#TablaEditarRegistroIndicador tbody tr td .btn-edit",
        "btndescarga": "#TablaEditarRegistroIndicador tbody tr td .btn-download",
        "btnEdit": "#TablaEditarRegistroIndicador tbody tr td .btn-edit",
        "btnCancela": "#btnCancelaFormularioWeb"
    },
    "Variables": {
   
        
    },

    "Metodos": {
   
    }

}

$(document).on("click", JsEditarFormularioWeb.Controles.btndescarga, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
                .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/Index" });
        });
});


$(document).on("click", JsEditarFormularioWeb.Controles.btnCancela, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        })
});




$(document).on("click", JsEditarFormularioWeb.Controles.btnEdit, function () {
    let id = 1;

    window.location.href = "/EditarFormulario/Edit?id="+id;       
});











