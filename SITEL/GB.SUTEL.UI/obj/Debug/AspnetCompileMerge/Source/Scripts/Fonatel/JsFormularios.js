JsFormulario= {
    "Controles": {
        "btnstep": ".step_navigation_Formulario div",
        "btnAgregarFormulario": "#TablaFormulario tbody tr td .btn-add",
        "btnEditarFormulario": "#TablaFormulario tbody tr td .btn-edit",
        "btnDeleteFormulario": "#TablaFormulario tbody tr td .btn-delete",
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

$(document).on("click", JsFormulario.Controles.btnstep, function () {
    $(JsFormulario.Controles.btnstep).children("a").addClass('btn-default').removeClass('btn-info-fonatel');
    $(this).children("a").addClass('btn-info-fonatel').removeClass('btn-default');
    let div = $(this).children("a").attr("href");
    $(JsFormulario.Controles.divContenedor).addClass('hidden');
    $(div).removeClass('hidden');
});



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
    jsMensajes.Metodos.EliminarRegistro("¿Desea Agregar el Formulario?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El formulario ha sido Creado")
                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click'); });
        });
});

$(document).on("click", JsFormulario.Controles.btnDeleteFormulario, function (e) {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Elimina el Formulario?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Formulario ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});






$(document).on("click", JsFormulario.Controles.btnDesactivadoFormulario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Activar la Formulario?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Formulario ha Sido Activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});
$(document).on("click", JsFormulario.Controles.btnActivadoFormulario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Desactivar el Formulario?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Formulario ha Sido Activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});




$(document).on("click", JsFormulario.Controles.btnAtrasFormularioRegla, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});


$(document).on("click", JsFormulario.Controles.btnGuardarFormularioCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Guardar el Formulario?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Formulario ha Sido Completado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});
