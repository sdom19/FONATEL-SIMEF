JsCategoria= {
    "Controles": {
        "divFechaMinima": "#divFechaMinimaCategoria",
        "divFechaMaxima": "#divFechaMaximaCategoria",
        "divCantidadDetalle": "#divCantidadDetalleCategoria",
        "divRangoMinimoCategoria": "#divRangoMinimaCategoria",
        "divRangoMaximaCategoria":"#divRangoMaximaCategoria",
        "ddlTipoDetalle": "#ddlTipoDetalleCategoria",
        "btnEliminar": ".btnEliminar",
        "btnGuardar": "#btnGuardarCategoria"
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

$(document).on("click", JsCategoria.Controles.btnEliminar, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Cambiar el Estado de la Categoría?")
        .then((result) => {
            if (result.isConfirmed) {
                jsMensajes.Metodos.ConfirmaRegistro("La Categoría ha sido Desactivada");
            }
        });
});




$(document).on("click", JsCategoria.Controles.btnGuardar, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Categoría?")
        .then((result) => {
            if (result.isConfirmed) {
                jsMensajes.Metodos.ConfirmaRegistro("La Categoría ha sido Agregada");
            }
        });
});