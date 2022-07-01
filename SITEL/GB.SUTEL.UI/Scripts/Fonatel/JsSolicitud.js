JsSolicitud= {
    "Controles": {

        "btnEliminar": ".btnEliminarSolicitud",
        "btnGuardar": "#btnGuardarSolicitud",
        "btnAgregarSolicitud": "#TablaSolicitudCategoria tbody tr td .btn-add",
        "btnEditarSolicitud": "#TablaSolicitudCategoria tbody tr td .btn-edit",
        "btnDeleteSolicitud": "#TablaSolicitudCategoria tbody tr td .btn-delete",
        "btnEliminarDetalleSolicitud": "#TablaDetalleSolicitudCategoria tbody tr td .btn-delete",
        "btnGuardarDetalle":"#btnGuardarDetalle"
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}

$(document).on("click", JsSolicitud.Controles.btnAgregarSolicitud, function () {
    let id = 1;
    window.location.href = "/Fonatel/SolicitudCategoria/Detalle?id=" + id;
});

$(document).on("click", JsSolicitud.Controles.btnEditarSolicitud, function () {
    let id = 1;
    window.location.href = "/Fonatel/SolicitudCategoria/Create?id=" + id;
});



$(document).on("click", JsSolicitud.Controles.btnGuardar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Relación?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Relación ha Sido Creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudCategoria/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnGuardarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Solicitudar la Categoría?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Categoría ha sido Solicitudada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudCategoria/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnEliminarDetalleSolicitud, function (e) {
     jsMensajes.Metodos.EliminarRegistro("¿Desea Elimina el Atributo?")
           .set('onok', function (closeEvent) {
                jsMensajes.Metodos.ConfirmaRegistro("El Atributo ha sido Eliminado")
                 .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudCategoria/index" });
         });
});

$(document).on("click", JsSolicitud.Controles.btnDeleteSolicitud, function (e) {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Elimina la Relación?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Relación ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudCategoria/index" });
        });
});