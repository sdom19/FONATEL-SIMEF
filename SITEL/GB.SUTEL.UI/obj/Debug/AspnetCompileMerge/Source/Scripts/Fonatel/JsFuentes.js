JsFuentes = {
    "Controles": {

        "btnGuardarFuente": "#btnGuardarFuente",
        "btnEditarFuente": "#TableFuentes tbody tr td .btn-edit",
        "btnBorrarFuente": "#TableFuentes tbody tr td .btn-delete",
        "btnAddFuente": "#TableFuentes tbody tr td .btn-add"
    },
    "Variables": {

    },

    "Metodos": {
        "Cerrar": function() {
            jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Fuente?")
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.ConfirmaRegistro("La Fuente ha Sido Agregada de Manera Correcta")
                        .set('onok', event.returnValue = "Te estás saliendo del sitio…");
                });

        } 
    }

}

$(document).on("click", JsFuentes.Controles.btnGuardarFuente, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Fuente?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Fuente ha Sido Agregada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});




$(document).on("click", JsFuentes.Controles.btnBorrarFuente, function () {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Eliminar la Fuente?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Fuente ha Sido Eliminada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});


$(document).on("click", JsFuentes.Controles.btnAddFuente, function () {

    let id = 1;
    window.location.href = "/Fonatel/Fuentes/Detalle?id="+id
});
