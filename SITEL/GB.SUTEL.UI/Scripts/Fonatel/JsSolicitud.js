JsSolicitud= {
    "Controles": {

        "btnAgregarSolicitud": "#TablaSolicitud tbody tr td .btn-add",
        "btnEditarSolicitud": "#TablaSolicitud tbody tr td .btn-edit",
        "btnDeleteSolicitud": "#TablaSolicitud tbody tr td .btn-delete",
        "btnCloneSolicitud": "#TablaSolicitud tbody tr td .btn-clone",
        "btnEliminarDetalleSolicitud": "#TablaSolicitud tbody tr td .btn-delete",
        "btnEnvioSolicitud": "#TablaSolicitud tbody tr td .btn-calendar",
        "btnEliminarProgramacion": "#TablaSolicitud tbody tr td .btn-calendar-disabled",
        "btnsent": "#TablaSolicitud tbody tr td .btn-sent",
        "btnGuardarSolicitud":"#btnGuardarSolicitud",
        "btnDesactivadoSolicitud": "#TablaSolicitud tbody tr td .btn-power-off",
        "btnActivadoSolicitud": "#TablaSolicitud tbody tr td .btn-power-on",
        "btnGuardarEnvio": "#btnGuardarSolicitudEnvio",
        "btnEliminarSolicituProgramardEnvio": "#btnEliminarSolicituProgramardEnvio",
        "btnCancelarEnvio": "#btnCancelarSolicitudEnvio",
        "modalEnvio": "#modalEnvio",
        "idSolicitudProgramacion": "#idSolicitudProgramacion",
        "ddldiaSolicitudModal":"#ddldiaSolicitudModal",
        "txtCantidadRepeticiones": "#txtCantidadRepeticiones",
        "txtFechaEnvio": "#txtFechaEnvio",
        "txtFechaInicioCiclo": "#txtFechaInicioCiclo",
        "ddlFormularios": "#ddlFormularios",
        "txtCampoRequerido": ".form-text-danger-fonatel"
    },
    "Variables":{
        "CantidadMaxDias": 28
    },

    "Metodos": {
        "CargarDiasMesCombo": function () {
            let html = "<option></option>";
            for (var i = 1; i <= JsSolicitud.Variables.CantidadMaxDias; i++) {
                html = html+"<option>" + i + "</option>";
            }
            $(JsSolicitud.Controles.ddldiaSolicitudModal).html(html);
        }
    }

}



$(document).on("click", JsSolicitud.Controles.btnEditarSolicitud, function () {
    let id = 1;
    window.location.href = "/Fonatel/SolicitudFonatel/Create?id=" + id;
});

$(document).on("click", JsSolicitud.Controles.btnCloneSolicitud, function () {
    let id = 1;
    window.location.href = "/Fonatel/SolicitudFonatel/Create?id=" + id;
});

$(document).on("click", JsSolicitud.Controles.btnEliminarProgramacion, function () {
    $(JsSolicitud.Controles.idSolicitudProgramacion).val("aqui va algun ID");

    $(JsSolicitud.Controles.txtCantidadRepeticiones).addClass("disabled");
    $(JsSolicitud.Controles.txtFechaEnvio).addClass("disabled");
    $(JsSolicitud.Controles.txtFechaInicioCiclo).addClass("disabled");
    $(JsSolicitud.Controles.ddlFormularios).select2("enable", false);
    $(JsSolicitud.Controles.txtCampoRequerido).addClass("hidden");

    $(JsSolicitud.Controles.btnEliminarSolicituProgramardEnvio).show();
    $(JsSolicitud.Controles.btnGuardarEnvio).hide();

    $(JsSolicitud.Controles.modalEnvio).modal('show');
});


$(document).on("click", JsSolicitud.Controles.btnGuardarEnvio, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Programación a las Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Programación a Sido Creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnGuardarSolicitud, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud a Sido Creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnDeleteSolicitud, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Elimina la Relación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Relación ha sido Eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnsent, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Enviar la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud ha sido enviada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnDesactivadoSolicitud, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Activar la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud ha Sido Activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnActivadoSolicitud, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Desactivar la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud ha Sido Desactivado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnEnvioSolicitud, function () {
    $(JsSolicitud.Controles.idSolicitudProgramacion).val(null);

    $(JsSolicitud.Controles.txtCantidadRepeticiones).removeClass("disabled");
    $(JsSolicitud.Controles.txtFechaEnvio).removeClass("disabled");
    $(JsSolicitud.Controles.txtFechaInicioCiclo).removeClass("disabled");
    $(JsSolicitud.Controles.ddlFormularios).select2("enable", "true");
    $(JsSolicitud.Controles.txtCampoRequerido).removeClass("hidden");

    $(JsSolicitud.Controles.btnEliminarSolicituProgramardEnvio).hide();
    $(JsSolicitud.Controles.btnGuardarEnvio).show();

    $(JsSolicitud.Controles.modalEnvio).modal('show');
});

$(document).on("click", JsSolicitud.Controles.btnEliminarSolicituProgramardEnvio, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar la Programación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Programación ha Sido Eliminada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnCancelarEnvio, function () {
    $(JsSolicitud.Controles.modalEnvio).modal('hide');
});