    JsIndicador= {
        "Controles": {
            "btnstep": ".step_navigation_indicador div",
            "divContenedor": ".contenedor",
            "btnGuardarIndicador": "#btnGuardarIndicador",
            "btnGuardarVariable": "#btnGuardarVariable",
            "btnGuardarCategoria": "#btnGuardarCategoria",
            "btnEditarIndicador": "#TableIndicador tbody tr td .btn-edit",
            "btnDesactivarIndicador": "#TableIndicador tbody tr td .btn-power-off",
            "btnActivarIndicador": "#TableIndicador tbody tr td .btn-power-on",
            "btnEliminarIndicador": "#TableIndicador tbody tr td .btn-delete",
            "btnClonarIndicador": "#TableIndicador tbody tr td .btn-clone",
            "btnAddIndicadorVariable": "#TableIndicador tbody tr td .variable",
            "btnAddIndicadorCategoria": "#TableIndicador tbody tr td .categoria",
            "btnEliminarVariable":"#TableDetalleVariable tbody tr td .btn-delete",
            "btnSiguienteCategoria": "#btnSiguienteCategoria",
            "btnAtrasCategoria": "#btnAtrasCategoria",
            "btnSiguienteVariable": "#btnSiguienteVariable",
            "btnAtrasVariable": "#btnAtrasVariable"


    },
    "Variables":{

    },

    "Metodos": {

    }

}




$(document).on("click", JsIndicador.Controles.btnstep, function () {

    $(JsIndicador.Controles.btnstep).children("a").addClass('btn-default').removeClass('btn-info-fonatel');
    $(this).children("a").addClass('btn-info-fonatel').removeClass('btn-default');
    let div = $(this).children("a").attr("href");
    $(JsIndicador.Controles.divContenedor).addClass('hidden');
    $(div).removeClass('hidden');
});




$(document).on("click", JsIndicador.Controles.btnEditarIndicador, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/IndicadorFonatel/Create?id=" + id;
});



$(document).on("click", JsIndicador.Controles.btnAddIndicadorCategoria, function () {
    let id = 1;
    window.location.href = "/Fonatel/IndicadorFonatel/DetalleCategoria?id=" + id;
});

$(document).on("click", JsIndicador.Controles.btnAddIndicadorVariable, function () {
    let id = 1;
    window.location.href = "/Fonatel/IndicadorFonatel/DetalleVariables?id=" + id;
});




$(document).on("click", JsIndicador.Controles.btnDesactivarIndicador, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Activar el Indicador?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Indicador ha sido Activado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});


$(document).on("click", JsIndicador.Controles.btnEliminarIndicador, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Eliminar el Indicador?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Indicador ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});


$(document).on("click", JsIndicador.Controles.btnEliminarVariable, function () {
    let id = 1;
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Eliminar la Variable?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Variable ha sido Eliminada")
                .set('onok', function (closeEvent) {  });
        });
});




$(document).on("click", JsIndicador.Controles.btnActivarIndicador, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Desactivar el Indicadores?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Categoría ha sido Desactivada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});




$(document).on("click", JsIndicador.Controles.btnGuardarIndicador, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar el Indicador?")
        .set('onok', function (closeEvent) {
            $("a[href='#step-2']").trigger('click');
        });
});




$(document).on("click", JsIndicador.Controles.btnSiguienteVariable, function (e) {
    e.preventDefault();
   $("a[href='#step-3']").trigger('click');

});


$(document).on("click", JsIndicador.Controles.btnAtrasVariable, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');

});


$(document).on("click", JsIndicador.Controles.btnSiguienteCategoria, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Guardar el Indicador?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Indicador ha sido Agregado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });

});


$(document).on("click", JsIndicador.Controles.btnAtrasCategoria, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');

});








$(document).on("click", JsIndicador.Controles.btnGuardarVariable, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Variable?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Variable ha sido Creada")
                .set('onok', function (closeEvent) { });
        });
});

$(document).on("click", JsIndicador.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Categoría?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Categoría ha sido Creada")
                .set('onok', function (closeEvent) { });
        });
});


$(document).on("click", JsIndicador.Controles.btnClonarIndicador, function () {
    let id = 1;
    jsMensajes.Metodos.ClonarRegistro("¿Desea Clonar el Indicador?")
        .set('onok', function (closeEvent) {
             window.location.href = "/Fonatel/IndicadorFonatel/Create?id="+id
        });
});