JsCategoria= {
    "Controles": {
        "divFechaMinima": "#divFechaMinimaCategoria",
        "divFechaMaxima": "#divFechaMaximaCategoria",
        "divCantidadDetalle": "#divCantidadDetalleCategoria",
        "divRangoMinimoCategoria": "#divRangoMinimaCategoria",
        "divRangoMaximaCategoria":"#divRangoMaximaCategoria",
        "ddlTipoDetalle": "#ddlTipoDetalleCategoria",
        "btnGuardar": "#btnGuardarCategoria",
        "btnEditar": "#TableCategoriaDesagregacion tbody tr td .btn-edit",
        "btnDesactivar": "#TableCategoriaDesagregacion tbody tr td .btn-power-off",
        "btnActivar": "#TableCategoriaDesagregacion tbody tr td .btn-power-on",
        "btnClonar": "#TableCategoriaDesagregacion tbody tr td .btn-clone"
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


$(document).on("click", JsCategoria.Controles.btnEditar, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id;
});


$(document).on("click", JsCategoria.Controles.btnDesactivar, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Activar la Categoría?")
        .then((result) => {
            if (result.isConfirmed) {
                jsMensajes.Metodos.ConfirmaRegistro("La Categoría ha sido Activada");
            }
        });
});

$(document).on("click", JsCategoria.Controles.btnActivar, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Desactivar la Categoría?")
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


$(document).on("click", JsCategoria.Controles.btnClonar, function () {
    jsMensajes.Metodos.CambiarEstadoRegistro("¿Desea Clonar la Categoría?")
        .then((result) => {
            if (result.isConfirmed) {
                let id = 1;
                window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id;
            }
        });
});