JsSolicitud = {
    "Controles": {
        "btnGuardarFormulario": "#btnGuardarFormulario",
        "btnAgregarSolicitud": "#TablaSolicitud tbody tr td .btn-add",
        "btnEditarSolicitud": "#TablaSolicitud tbody tr td .btn-edit",
        "btnDeleteSolicitud": "#TablaSolicitud tbody tr td .btn-delete",
        "btnCloneSolicitud": "#TablaSolicitud tbody tr td .btn-clone",
        "btnEliminarDetalleSolicitud": "#TablaSolicitud tbody tr td .btn-delete",
        "btnEnvioSolicitud": "#TablaSolicitud tbody tr td .btn-calendar",
        "btnEliminarProgramacion": "#TablaSolicitud tbody tr td .btn-calendar-disabled",
        "btnsent": "#TablaSolicitud tbody tr td .btn-sent",
        "btnGuardarSolicitud": "#btnGuardarSolicitud",
        "btnCancelar": "#btnCancelarSolicitud",
        "btnDesactivadoSolicitud": "#TablaSolicitud tbody tr td .btn-power-off",
        "btnActivadoSolicitud": "#TablaSolicitud tbody tr td .btn-power-on",
        "btnGuardarEnvio": "#btnGuardarSolicitudEnvio",
        "btnEliminarSolicituProgramardEnvio": "#btnEliminarSolicituProgramardEnvio",
        "btnCancelarEnvio": "#btnCancelarSolicitudEnvio",
        "modalEnvio": "#modalEnvio",
        "idSolicitudProgramacion": "#idSolicitudProgramacion",
        "ddldiaSolicitudModal": "#ddldiaSolicitudModal",
        "txtCantidadRepeticiones": "#txtCantidadRepeticiones",
        "txtFechaEnvio": "#txtFechaEnvio",
        "txtFechaInicioCiclo": "#txtFechaInicioCiclo",
        "ddlFormularios": "#ddlFormularios",
        "txtCampoRequerido": ".form-text-danger-fonatel",
        "TablaSolicitud": "#TablaSolicitud tbody",
        "txtmodoSolicitud": "#txtmodoSolicitud",
        "txtCodigo": "#txtCodigo",
        "btnGuardarSolicitud": "#btnGuardarSolicitud",
        "btnSiguienteSolicitud": "#btnSiguienteSolicitud",
        "step2": "a[href='#step-2']",
        "step1": "a[href='#step-1']",
        "btnAtrasSolicitud": "#btnAtrasSolicitud",
        "txtCodigo": "#txtCodigo",
        "txtNombre": "#txtNombre",
        "txtFechaInicio": "#txtFechaInicio",
        "txtFechaFin": "#txtFechaFin",
        "ddlFuentes": "#ddlFuentes",
        "TxtCantidadFormulario": "#TxtCantidadFormulario",
        "ddlMesSolicitud": "#ddlMesSolicitud",
        "ddlAnoSolicitud": "#ddlAnoSolicitud",
        "txtMensajeSolicitud": "#txtMensajeSolicitud",
        "ddlFormularioWeb": "#ddlFormularioWeb",
        "ddlVariableIndicadorHelp": "#ddlVariableIndicadorHelp",
        "btnCancelarFormulario": "#btnCancelarFormulario",
        "CodigoHelp": "#CodigoHelp",
        "nombreHelp": "#nombreHelp",
        "FechaInicioHelp": "#FechaInicioHelp",
        "FechaFinHelp": "#FechaFinHelp",
        "FuentesHelp": "#FuentesHelp",
        "FormulariosHelp": "#FormulariosHelp",
        "ddlMesSolicitudHelp": "#ddlMesSolicitudHelp",
        "ddlAnoSolicitudHelp": "#ddlAnoSolicitudHelp",
        "ControlesStep1": "#formCrearSolicitud input, #formCrearSolicitud textarea, #formCrearSolicitud select",
        "txtMensajeSolicitudHelp": "#txtMensajeSolicitudHelp"

    },
    "Variables": {
        "CantidadMaxDias": 28,
        "ListadoSolicitudes": []
    },

    "Metodos": {
        "CargarDiasMesCombo": function () {
            let html = "<option></option>";
            for (var i = 1; i <= JsSolicitud.Variables.CantidadMaxDias; i++) {
                html = html + "<option>" + i + "</option>";
            }
            $(JsSolicitud.Controles.ddldiaSolicitudModal).html(html);
        },

        "CargarTablaSolicitudes": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsSolicitud.Variables.ListadoSolicitudes.length; i++) {
                let solicitud = JsSolicitud.Variables.ListadoSolicitudes[i];
                let listaFormularios = solicitud.SolicitudFormulario.length == 0 ? "N/A" : solicitud.SolicitudFormulario.join(", ");
                let envioProgramado = solicitud.EnvioProgramado == null ? "NO" : "SI";

                html = html + "<tr>";

                html = html + "<td>" + solicitud.Codigo + "</td>";
                html = html + "<td>" + solicitud.Nombre + "</td>";
                html = html + "<td>" + solicitud.Fuente.Fuente + "</td>";
                html = html + "<td>" + listaFormularios + "</td>";
                html = html + "<td>" + envioProgramado + "</td>";
                html = html + "<td>" + solicitud.Estado.Nombre + "</td >";

                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Editar' class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Clonar' class='btn-icon-base btn-clone'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Desactivar' class='btn-icon-base btn-power-on'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Eliminar' class='btn-icon-base btn-delete'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Envío' class='btn-icon-base btn-sent'></button>";
                if (envioProgramado == "SI") {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar Programación' class='btn-icon-base btn-calendar'></button></td></tr>";
                }
                else {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar Programación' class='btn-icon-base btn-calendar-disabled'></button></td></tr>";
                }
            }
            $(JsSolicitud.Controles.TablaSolicitud).html(html);
            CargarDatasource();
            JsSolicitud.Variables.ListadoSolicitudes = [];
        },

        "ValidarControles": function () {
            let validar = true;
            $(JsSolicitud.Controles.CodigoHelp).addClass("hidden");
            $(JsSolicitud.Controles.nombreHelp).addClass("hidden");
            $(JsSolicitud.Controles.FechaInicioHelp).addClass("hidden");
            $(JsSolicitud.Controles.FechaFinHelp).addClass("hidden");
            $(JsSolicitud.Controles.FuentesHelp).addClass("hidden");
            $(JsSolicitud.Controles.FormulariosHelp).addClass("hidden");
            $(JsSolicitud.Controles.ddlMesSolicitudHelp).addClass("hidden");
            $(JsSolicitud.Controles.ddlAnoSolicitudHelp).addClass("hidden");
            $(JsSolicitud.Controles.txtMensajeSolicitudHelp).addClass("hidden");

            let codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            let nombre = $(JsSolicitud.Controles.txtNombre).val().trim();
            let fechainicio = $(JsSolicitud.Controles.txtFechaInicio).val().trim();
            let fechaFin = $(JsSolicitud.Controles.txtFechaFin).val().trim();
            let fuentes = $(JsSolicitud.Controles.ddlFuentes).val().trim();
            let CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val().trim();
            let mes = $(JsSolicitud.Controles.ddlMesSolicitud).val().trim();
            let anno = $(JsSolicitud.Controles.ddlAnoSolicitud).val().trim();
            let mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            if (codigo.length == 0) {
                $(JsSolicitud.Controles.CodigoHelp).removeClass("hidden");
                Validar = false;
            }
            if (nombre.length == 0) {
                $(JsSolicitud.Controles.nombreHelp).removeClass("hidden");
                validar = false;
            }
            if (fechainicio == "0001-01-01") {
                $(JsSolicitud.Controles.FechaInicioHelp).removeClass("hidden");
                validar = false;
            }
            if (fechaFin == "0001-01-01") {
                $(JsSolicitud.Controles.FechaFinHelp).removeClass("hidden");
                validar = false;
            }
            if (fuentes.length == 0) {
                $(JsSolicitud.Controles.FuentesHelp).removeClass("hidden");
                validar = false;
            }
            if (CantidadFormulario == 0) {
                $(JsSolicitud.Controles.FormulariosHelp).removeClass("hidden");
                validar = false;
            }
            if (mes.length == 0) {
                $(JsSolicitud.Controles.ddlMesSolicitudHelp).removeClass("hidden");
                validar = false;
            }
            if (anno.length == 0) {
                $(JsSolicitud.Controles.ddlAnoSolicitudHelp).removeClass("hidden");
                validar = false;
            }
            if (mensaje.length == 0) {
                $(JsSolicitud.Controles.txtMensajeSolicitudHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },

        "ValidarFormularioCrear": function () {
            if ($(JsSolicitud.Controles.txtCodigo).val().trim().length > 0 && $(JsSolicitud.Controles.txtNombre).val().trim().length > 0
                && $(JsSolicitud.Controles.txtFechaInicio).val().trim() != "0001-01-01" && $(JsSolicitud.Controles.txtFechaFin).val().trim() != "0001-01-01"
                && $(JsSolicitud.Controles.ddlFuentes).val().trim().length > 0 && $(JsSolicitud.Controles.TxtCantidadFormulario).val().trim() != 0
                && $(JsSolicitud.Controles.ddlMesSolicitud).val().trim().length > 0 && $(JsSolicitud.Controles.ddlAnoSolicitud).val().trim().length > 0
                && $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim().length > 0) {

                $(JsSolicitud.Controles.btnSiguienteSolicitud).prop("disabled", false);
                $(JsSolicitud.Controles.step2).prop("disabled", false);
            }
            else {
                $(JsSolicitud.Controles.btnSiguienteSolicitud).prop("disabled", true);
                $(JsSolicitud.Controles.step2).prop("disabled", true);
            }
        },
    },

    "Consultas": {
        "ConsultaListaSolicitudes": function () {
            $("#loading").fadeIn();
            execAjaxCall("/SolicitudFonatel/ObtenerListaSolicitudes", "GET")
                .then((obj) => {
                    JsSolicitud.Variables.ListadoSolicitudes = obj.objetoRespuesta;
                    JsSolicitud.Metodos.CargarTablaSolicitudes();
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        }
    }
}

$(document).on("click", JsSolicitud.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/SolicitudFonatel/Index";
        });
});

$(document).on("click", JsSolicitud.Controles.btnEditarSolicitud, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/SolicitudFonatel/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;;
});

$(document).on("click", JsSolicitud.Controles.btnCloneSolicitud, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/SolicitudFonatel/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
});

$(document).on("click", JsSolicitud.Controles.btnGuardarFormulario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el formulario a la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {

            if ($(JsSolicitud.Controles.ddlFormularioWeb).val().length > 0) {
                $(JsSolicitud.Controles.ddlVariableIndicadorHelp).addClass("hidden");
                jsMensajes.Metodos.OkAlertModal("El Formulario ha sido agregado")
                    .set('onok', function (closeEvent) { $(JsSolicitud.Controles.ddlFormularioWeb).val("").trigger('change'); });
            }
            else {
                $(JsSolicitud.Controles.ddlVariableIndicadorHelp).removeClass("hidden");
            }
        });
});
$(document).on("click", JsSolicitud.Controles.btnCancelarFormulario, function (e) {
    e.preventDefault();
    $(JsSolicitud.Controles.ddlFormularioWeb).val("").trigger('change');
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la programación a las Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Programación a sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnGuardarSolicitud, function (e) {
    e.preventDefault();
    let CamposVacios = "Existen campos vacíos. "
    if (JsSolicitud.Metodos.ValidarControles()) {
        CamposVacios = ""
    }
       jsMensajes.Metodos.ConfirmYesOrNoModal(CamposVacios +"¿Desea realizar un guardado parcial para la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud a sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnDeleteSolicitud, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Solicitud?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud ha sido eliminada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnsent, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea enviar la Solicitud?", null, "Enviar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud ha sido enviada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnDesactivadoSolicitud, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud ha sido activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnActivadoSolicitud, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Solicitud?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Solicitud ha sido desactivada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnSiguienteSolicitud, function (e) {
    e.preventDefault();
    if (JsSolicitud.Metodos.ValidarControles()) {
        $(JsSolicitud.Controles.step2).trigger('click');
    }
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Programación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Programación ha sido eliminada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
        });
});

$(document).on("click", JsSolicitud.Controles.btnCancelarEnvio, function () {
    $(JsSolicitud.Controles.modalEnvio).modal('hide');
});

$(document).on("click", JsSolicitud.Controles.btnAtrasSolicitud, function (e) {
    e.preventDefault();
    $(JsSolicitud.Controles.step1).trigger('click');
});

$(document).on("keyup", JsSolicitud.Controles.ControlesStep1, function (e) {
    JsSolicitud.Metodos.ValidarFormularioCrear();
});

$(document).on("change", JsSolicitud.Controles.ControlesStep1, function (e) {
    JsSolicitud.Metodos.ValidarFormularioCrear();
});

$(function () {
    JsSolicitud.Metodos.ValidarFormularioCrear();
    let modo = $(JsSolicitud.Controles.txtmodoSolicitud).val();
    if ($(JsSolicitud.Controles.TablaSolicitud).length > 0) {
        JsSolicitud.Consultas.ConsultaListaSolicitudes();
    }
    else if (modo == jsUtilidades.Variables.Acciones.Editar) {
        $(JsSolicitud.Controles.txtCodigo).prop("disabled", true);
    }
    else {

    }
});
