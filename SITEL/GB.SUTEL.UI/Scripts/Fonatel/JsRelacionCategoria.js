JsRelacion= {
    "Controles": {

        "btnEliminar": ".btnEliminarRelacion",
        "btnGuardar": "#btnGuardarRelacion",
        "btnCancelar": "#btnCancelarRelacion",
        "btnCancelarDetalle":"#btnCancelarDetalle",
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

$(document).on("click", JsRelacion.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RelacionCategoria/Index";
        });
});


$(document).on("click", JsRelacion.Controles.btnCancelarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RelacionCategoria/Index";
        });
});



$(document).on("click", JsRelacion.Controles.btnGuardar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Relación?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Relación ha sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});

$(document).on("click", JsRelacion.Controles.btnGuardarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea relacionar la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido relacionada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});

$(document).on("click", JsRelacion.Controles.btnEliminarDetalleRelacion, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina el Atributo?", jsMensajes.Variables.actionType.eliminar)
           .set('onok', function (closeEvent) {
                jsMensajes.Metodos.OkAlertModal("El Atributo ha sido eliminado")
                 .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
         });
});

$(document).on("click", JsRelacion.Controles.btnDeleteRelacion, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina la Relación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Relación ha sido eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});