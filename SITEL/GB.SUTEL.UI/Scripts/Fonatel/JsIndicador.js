    JsIndicador= {
        "Controles": {
            "btnstep": ".step_navigation_indicador div",
            "divContenedor": ".stepwizard-content-container",
            "btnGuardarIndicador": "#btnGuardarIndicador",
            "btnSiguienteIndicador":"#btnSiguienteIndicador",
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Activar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido Activado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});


$(document).on("click", JsIndicador.Controles.btnEliminarIndicador, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});


$(document).on("click", JsIndicador.Controles.btnEliminarVariable, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar la Variable?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Variable ha sido Eliminada")
                .set('onok', function (closeEvent) {  });
        });
});




$(document).on("click", JsIndicador.Controles.btnActivarIndicador, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Desactivar el Indicadores?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido Desactivada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});




$(document).on("click", JsIndicador.Controles.btnGuardarIndicador, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial del Indicador", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            $("a[href='#step-2']").trigger('click');
        });
});



$(document).on("click", JsIndicador.Controles.btnSiguienteIndicador, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Guardar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Indicador ha sido Agregado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });

});


$(document).on("click", JsIndicador.Controles.btnAtrasCategoria, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');

});








$(document).on("click", JsIndicador.Controles.btnGuardarVariable, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Variable?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Variable ha sido Creada")
                .set('onok', function (closeEvent) { });
        });
});

$(document).on("click", JsIndicador.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido Creada")
                .set('onok', function (closeEvent) { });
        });
});


$(document).on("click", JsIndicador.Controles.btnClonarIndicador, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Clonar el Indicador?", jsMensajes.Variables.actionType.clonar)
        .set('onok', function (closeEvent) {
             window.location.href = "/Fonatel/IndicadorFonatel/Create?id="+id
        });
});

