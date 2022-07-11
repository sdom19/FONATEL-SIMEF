JsRelacion= {
    "Controles": {

        "btnEliminar": ".btnEliminarRelacion",
        "btnGuardar": "#btnGuardarRelacion",
        "btnAgregarRelacion": "#TablaRelacionCategoria tbody tr td .btn-add",
        "btnEditarRelacion": "#TablaRelacionCategoria tbody tr td .btn-edit",
        "btnDeleteRelacion": "#TablaRelacionCategoria tbody tr td .btn-delete",
        "btnEliminarDetalleRelacion": "#TablaDetalleRelacionCategoria tbody tr td .btn-delete",
        "btnGuardarDetalle":"#btnGuardarDetalle"
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}

$(document).on("click", JsRelacion.Controles.btnAgregarRelacion, function () {
    let id = 1;
    window.location.href = "/Fonatel/RelacionCategoria/Detalle?id=" + id;
});

$(document).on("click", JsRelacion.Controles.btnEditarRelacion, function () {
    let id = 1;
    window.location.href = "/Fonatel/RelacionCategoria/Create?id=" + id;
});



$(document).on("click", JsRelacion.Controles.btnGuardar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Relación?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Relación ha Sido Creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});

$(document).on("click", JsRelacion.Controles.btnGuardarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Relacionar la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido Relacionada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});

$(document).on("click", JsRelacion.Controles.btnEliminarDetalleRelacion, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Elimina el Atributo?", jsMensajes.Variables.actionType.eliminar)
           .set('onok', function (closeEvent) {
                jsMensajes.Metodos.OkAlertModal("El Atributo ha sido Eliminado")
                 .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
         });
});

$(document).on("click", JsRelacion.Controles.btnDeleteRelacion, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Elimina la Relación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Relación ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});