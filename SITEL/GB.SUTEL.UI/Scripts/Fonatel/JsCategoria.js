    JsCategoria= {
        "Controles": {
            "divFechaMinima": "#divFechaMinimaCategoria",
            "divFechaMaxima": "#divFechaMaximaCategoria",
            "divCantidadDetalle": "#divCantidadDetalleCategoria",
            "divRangoMinimoCategoria": "#divRangoMinimaCategoria",
            "divRangoMaximaCategoria": "#divRangoMaximaCategoria",
            "ddlTipoDetalle": "#ddlTipoDetalleCategoria",
            "btnGuardarCategoria": "#btnGuardarCategoria",

            "btnCancelar": "#btnCancelarCategoria",
            "btnCancelarDetalle": "#btnCancelarDetalleCategoria",
            "btnEditarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-edit",
            "btnDesactivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-off",
            "btnActivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-on",
            "btnClonarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-clone",
            "btnAddCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-add",
            "btnRemoveCategoriaDetalle": "#TableCategoriaDesagregacionDetalle tbody tr td .btn-delete",
            "btnGuardarDetalleCategoria": "#btnGuardarDetalleCategoria",

            "btnCargarDetalle": "#btnCargarDetalle",
            "inputFileCargarDetalle": "#inputFileCargarDetalle"
        },

        "Variables": {
            "TipoFecha": 4,
            "TipoNumerico": 1
        },

        "Metodos": {
            "CerrarFormulario": function () {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Salir del Formulario?", jsMensajes.Variables.actionType.cancelar)
                    .set('onok', function (closeEvent) {
                        
                    }); 
            },
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

$(document).on("click", JsCategoria.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            preguntarAntesDeSalir = false;  window.location.href = "/Fonatel/CategoriasDesagregacion/Index"; 
        });   
});

$(document).on("click", JsCategoria.Controles.btnCancelarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/CategoriasDesagregacion/Index";
        });
});


$(document).on("change", JsCategoria.Controles.ddlTipoDetalle, function () {
    var selected = $(this).val();
    JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
});

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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});

$(document).on("click", JsCategoria.Controles.btnActivarCategoria, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido desactivada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});



$(document).on("click", JsCategoria.Controles.btnRemoveCategoriaDetalle, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Detalle?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Detalle ha sido eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/Detalle?id="+id });
        });
});

$(document).on("click", JsCategoria.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});



$(document).on("click", JsCategoria.Controles.btnGuardarDetalleCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el detalle a la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El detalle ha sido agregado")
                .set('onok', function (closeEvent) {  });
        });
});





$(document).on("click", JsCategoria.Controles.btnClonarCategoria, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Categoría?", jsMensajes.Variables.actionType.clonar)
        .set('onok', function (closeEvent) {
             window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id="+id
        });
});

$(document).on("click", JsCategoria.Controles.btnCargarDetalle, function (e) {
    $(JsCategoria.Controles.inputFileCargarDetalle).click();
});


//window.addEventListener('beforeunload', (event) => {
//    // Cancel the event as stated by the standard.
//    event.preventDefault();
//    // Chrome requires returnValue to be set.
//    event.returnValue = '';
//});