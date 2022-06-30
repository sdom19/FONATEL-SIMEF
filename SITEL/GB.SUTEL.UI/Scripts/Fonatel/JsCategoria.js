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
            "btnRemoveCategoriaDetalle": "#TableCategoriaDesagregacionDetalle tbody tr td .btn-delete"
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
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Activar la Categoría?")
        .then((result) => {
            if (result.isConfirmed) {
                jsMensajes.Metodos.ConfirmaRegistro("La Categoría ha sido Activada");
            }
        });
});

$(document).on("click", JsCategoria.Controles.btnActivarCategoria, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Desactivar la Categoría?");
});



$(document).on("click", JsCategoria.Controles.btnRemoveCategoriaDetalle, function () {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Eliminar el Detalle?");
});

$(document).on("click", JsCategoria.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Categoría?");
});


$(document).on("click", JsCategoria.Controles.btnClonarCategoria, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Clonar la Categoría?")
        .then((result) => {
            if (result.isConfirmed) {
                let id = 1;
                window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id;
            }
        });
});