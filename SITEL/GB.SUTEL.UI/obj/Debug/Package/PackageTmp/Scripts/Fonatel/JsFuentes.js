JsFuentes = {
    "Controles": {
        "btnstep": ".step_navigation_fuentes div",
        "btnGuardarFuente": "#btnGuardarFuente",
        "btnEditarFuente": "#TableFuentes tbody tr td .btn-edit",
        "btnBorrarFuente": "#TableFuentes tbody tr td .btn-delete",
        "btnAddFuente": "#TableFuentes tbody tr td .btn-add",
        "divContenedor": ".divContenedor_fuentes",
        "btnGuardarDestinatario": "#btnGuardarDestinatario",
        "btnGuardarFuentesCompleto": "#btnGuardarFuentesCompleto",
        "btnAtrasFuentes":"#btnAtrasFuentes"
    },
    "Variables": {

    },

    "Metodos": {
      
    }

}

$(document).on("click", JsFuentes.Controles.btnGuardarFuente, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha Sido Agregada de Manera Correcta")
                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click'); });
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarFuentesCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha Sido Agregada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarDestinatario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar el Destinatario a la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Destinatario ha Sido Agregado")
                .set('onok', function (closeEvent) { });
        });
});

$(document).on("click", JsFuentes.Controles.btnBorrarFuente, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar la Fuente?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha Sido Eliminada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});

$(document).on("click", JsFuentes.Controles.btnAtrasFuentes, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});