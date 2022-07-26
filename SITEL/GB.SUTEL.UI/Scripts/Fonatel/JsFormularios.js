JsFormulario= {
    "Controles": {
        "btnAgregarFormulario": "#TablaFormulario tbody tr td .btn-add",
        "btnEditarFormulario": "#TablaFormulario tbody tr td .btn-edit",
        "btnDeleteFormulario": "#TablaFormulario tbody tr td .btn-delete",
        "btnSiguienteFormulario":"#btnSiguienteFormulario",
        "btnCloneFormulario": "#TablaFormulario tbody tr td .btn-clone",
        "btnDesactivadoFormulario": "#TablaFormulario tbody tr td .btn-power-off",
        "btnActivadoFormulario": "#TablaFormulario tbody tr td .btn-power-on",
        "btnGuardar": "#btnGuardarFormulario",
        "divContenedor": ".contenedor_formulario",
        "btnAtrasFormularioRegla": "#btnAtrasFormularioRegla",
        "btnGuardarFormularioCompleto":"#btnGuardarFormularioCompleto"
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}




$(document).on("click", JsFormulario.Controles.btnEditarFormulario, function () {
    let id = 1;
    window.location.href = "/Fonatel/FormularioWeb/Create?id=" + id;
});

$(document).on("click", JsFormulario.Controles.btnCloneFormulario, function () {
    let id = 1;
    window.location.href = "/Fonatel/FormularioWeb/Create?id=" + id;
});



$(document).on("click", JsFormulario.Controles.btnGuardar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El formulario ha sido Creado")
                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click'); });
        });
});



$(document).on("click", JsFormulario.Controles.btnSiguienteFormulario, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');
});


$(document).on("click", JsFormulario.Controles.btnDeleteFormulario, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Elimina el Formulario?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});






$(document).on("click", JsFormulario.Controles.btnDesactivadoFormulario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Activar la Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Formulario ha Sido Activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});
$(document).on("click", JsFormulario.Controles.btnActivadoFormulario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Desactivar el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Formulario ha Sido Activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});




$(document).on("click", JsFormulario.Controles.btnAtrasFormularioRegla, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});


$(document).on("click", JsFormulario.Controles.btnGuardarFormularioCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Guardar el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Formulario ha Sido Completado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});
