JsFuentes = {
    "Controles": {
        "btnstep": ".step_navigation_fuentes div",
        "btnGuardarFuente": "#btnGuardarFuente",
        "btnSiguienteFuente":"#btnSiguienteFuente",
        "btnEditarFuente": "#TableFuentes tbody tr td .btn-edit",
        "btnBorrarFuente": "#TableFuentes tbody tr td .btn-delete",
        "btnBorrarDetalle": "#TableDetalleFuentes tbody tr td .btn-delete",
        "btnAddFuente": "#TableFuentes tbody tr td .btn-add",
        "divContenedor": ".divContenedor_fuentes",
        "btnGuardarDestinatario": "#btnGuardarDestinatario",
        "btnGuardarFuentesCompleto": "#btnGuardarFuentesCompleto",
        "btnAtrasFuentes": "#btnAtrasFuentes",
        "btnCancelar": "#btnCancelarFuente"
    },
    "Variables": {

    },

    "Metodos": {
      
    }

}

$(document).on("click", JsFuentes.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/Fuentes/Index";
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarFuente, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha sido creada")
                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click');  });
        });
});


$(document).on("click", JsFuentes.Controles.btnSiguienteFuente, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');
});



$(document).on("click", JsFuentes.Controles.btnGuardarFuentesCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarDestinatario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el destinatario a la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El destinatario ha sido creado")
                .set('onok', function (closeEvent) { });
        });
});

$(document).on("click", JsFuentes.Controles.btnBorrarFuente, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Fuente?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha sido eliminada ")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});





$(document).on("click", JsFuentes.Controles.btnBorrarDetalle, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Destinatario?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Destinatario ha sido eliminado")
                .set('onok', function (closeEvent) { });
        });
});


$(document).on("click", JsFuentes.Controles.btnAtrasFuentes, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});