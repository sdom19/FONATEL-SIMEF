    JsCategoria= {
    "Controles": {
        "divFechaMinima": "#divFechaMinimaCategoria",
        "divFechaMaxima": "#divFechaMaximaCategoria",
        "divCantidadDetalle": "#divCantidadDetalleCategoria",
        "divRangoMinimoCategoria": "#divRangoMinimaCategoria",
        "divRangoMaximaCategoria":"#divRangoMaximaCategoria",
        "ddlTipoDetalle": "#ddlTipoDetalleCategoria",
        "btnGuardarCategoria": "#btnGuardarCategoria",
        "btnEditarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-edit",
        "btnDesactivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-off",
        "btnActivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-on",
        "btnClonarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-clone",
        "btnAddCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-add",
            "btnRemoveCategoriaDetalle": "#TableCategoriaDesagregacionDetalle tbody tr td .btn-delete",
            "btnGuardarDetalleCategoria":"#btnGuardarDetalleCategoria"
    },
    "Variables":{
        "TipoFecha": 4,
        "TipoNumerico":1
    },

    "Metodos": {
        "HabilitarControlesTipoCategoria": function (selected) {
            if (selected == JsCategoria.Variables.TipoFecha) {
                $(JsCategoria.Controles.divRangoMaximaCategoria).addClass("hidden");
                $(JsCategoria.Controles.divRangoMinimoCategoria).addClass("hidden");
                $(JsCategoria.Controles.divFechaMaxima).removeClass("hidden");
                $(JsCategoria.Controles.divFechaMinima).removeClass("hidden");
                $(JsCategoria.Controles.divCantidadDetalle).addClass("hidden");
            }
            else if (selected == JsCategoria.Variables.TipoNumerico) {
                $(JsCategoria.Controles.divFechaMaxima).addClass("hidden");
                $(JsCategoria.Controles.divFechaMinima).addClass("hidden");
                $(JsCategoria.Controles.divRangoMaximaCategoria).removeClass("hidden");
                $(JsCategoria.Controles.divRangoMinimoCategoria).removeClass("hidden");
                $(JsCategoria.Controles.divCantidadDetalle).addClass("hidden");
            }
            else {
                $(JsCategoria.Controles.divRangoMaximaCategoria).addClass("hidden");
                $(JsCategoria.Controles.divRangoMinimoCategoria).addClass("hidden");
                $(JsCategoria.Controles.divFechaMaxima).addClass("hidden");
                $(JsCategoria.Controles.divFechaMinima).addClass("hidden");
                $(JsCategoria.Controles.divCantidadDetalle).removeClass("hidden");
            }
        }
    }

}
$(document).on("change", JsCategoria.Controles.ddlTipoDetalle, function () {
    var selected = $(this).val();
    JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
});


$(document).on("click", JsCategoria.Controles.btnEditarCategoria, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id;
});


$(document).on("click", JsCategoria.Controles.btnAddCategoria, function () {
    let id = 1;
    window.location.href = "/Fonatel/CategoriasDesagregacion/Detalle?id=" + id;
});




$(document).on("click", JsCategoria.Controles.btnDesactivarCategoria, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Activar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido Activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});

$(document).on("click", JsCategoria.Controles.btnActivarCategoria, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Desactivar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido Desactivada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});



$(document).on("click", JsCategoria.Controles.btnRemoveCategoriaDetalle, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar el Detalle?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Detalle ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/Detalle?id="+id });
        });
});

$(document).on("click", JsCategoria.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido Creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});



$(document).on("click", JsCategoria.Controles.btnGuardarDetalleCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar el detalle a la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Detalle ha sido agregado")
                .set('onok', function (closeEvent) {  });
        });
});





$(document).on("click", JsCategoria.Controles.btnClonarCategoria, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Clonar la Categoría?", jsMensajes.Variables.actionType.clonar)
        .set('onok', function (closeEvent) {
             window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id="+id
        });
});