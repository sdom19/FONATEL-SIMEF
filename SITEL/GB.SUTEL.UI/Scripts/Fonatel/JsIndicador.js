    JsIndicador= {
        "Controles": {



        "btnGuardarIndicador": "#btnGuardarIndicador",
        "btnEditarIndicador": "#TableIndicador tbody tr td .btn-edit",
        "btnDesactivarIndicador": "#TableIndicador tbody tr td .btn-power-off",
        "btnActivarIndicador": "#TableIndicador tbody tr td .btn-power-on",
        "btnEliminarIndicador": "#TableIndicador tbody tr td .btn-delete",
        "btnClonarIndicador": "#TableIndicador tbody tr td .btn-clone",
        "btnAddIndicadorVariable": "#TableIndicador tbody tr td .variable",
        "btnAddIndicadorCategoria": "#TableIndicador tbody tr td .categoria",


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
    let id = $(this).val();
    window.location.href = "/Fonatel/IndicadorFonatel/Create?id=" + id;
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
            jsMensajes.Metodos.ConfirmaRegistro("El Indicador ha sido Creado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});


$(document).on("click", JsIndicador.Controles.btnClonarIndicador, function () {
    let id = 1;
    jsMensajes.Metodos.ClonarRegistro("¿Desea Clonar el Indicador?")
        .set('onok', function (closeEvent) {
             window.location.href = "/Fonatel/IndicadorFonatel/Create?id="+id
        });
});