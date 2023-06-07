﻿
JsSolicitud = {

    "Controles": {
        "btnGuardarFormulario": "#btnGuardarFormulario",
        "btnAgregarSolicitud": "#TablaSolicitud tbody tr td .btn-add",
        "btnEditarSolicitud": "#TablaSolicitud tbody tr td .btn-edit",
        "btnDeleteSolicitud": "#TablaSolicitud tbody tr td .btn-delete",
        "btnCloneSolicitud": "#TablaSolicitud tbody tr td .btn-clone",
        "btnViewSolicitud": "#TablaSolicitud tbody tr td .btn-view",
        "btnEliminarDetalleSolicitud": "#TablaSolicitud tbody tr td .btn-delete",
        "btnEnvioSolicitud": "#TablaSolicitud tbody tr td .btn-calendar",
        "btnEliminarProgramacion": "#TablaSolicitud tbody tr td .btn-calendar-disabled",
        "btnsent": "#TablaSolicitud tbody tr td .btn-sent",
        "btnCancelar": "#btnCancelarSolicitud",
        "btnAtrasSolicitud":"#btnAtrasSolicitud",
        "btnDesactivadoSolicitud": "#TablaSolicitud tbody tr td .btn-power-off",
        "btnActivadoSolicitud": "#TablaSolicitud tbody tr td .btn-power-on",
        "btnGuardarEnvio": "#btnGuardarSolicitudEnvio",
        "btnEliminarSolicituProgramardEnvio": "#btnEliminarSolicituProgramardEnvio",
        "btnCancelarEnvio": "#btnCancelarSolicitudEnvio",
        "modalEnvio": "#modalEnvio",
        "idSolicitudProgramacion": "#idSolicitudProgramacion",
        "ddldiaSolicitudModal": "#ddldiaSolicitudModal",
        "txtCantidadRepeticion": "#txtCantidadRepeticiones",
        "txtFechaEnvio": "#txtFechaEnvio",
        "txtFechaInicioCiclo": "#txtFechaInicioCiclo",
        "ddlFormularios": "#ddlFormularios",
        "txtCampoRequerido": ".form-text-danger-fonatel",
        "TablaSolicitud": "#TablaSolicitud tbody",
        "TablaSolicitudElemento": "#TablaSolicitud",
        "txtmodoSolicitud": "#txtmodoSolicitud",
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
        "txtMensajeSolicitudHelp": "#txtMensajeSolicitudHelp",
        "TablaFormulario": "#TablaFormulario tbody",
        "TablaFormularioElemento": "#TablaFormulario",
        "btnDeleteFormulario": "#TablaFormulario tbody tr td .btn-delete",
        "btnFinalizarSolicitud": "#btnFinalizarSolicitud",
        "txtModo": "#txtmodo",
        "id": "#txtidsolicitud",
        "idEnvioProgramado": "txtSolicitudEnvio",
        "txtSolicitudModal": "#txtSolicitudModal",
        "txtSolicitudEnvio": "#txtSolicitudEnvio",
        "ddlFrecuencia": "#ddlFrecuencia",
        "ddlFrecuanciaEnvio": "#ddlFrecuanciaEnvio",
        "txtFechaCiclo": "#txtFechaCiclo",
        "ddlFrecuenciaHelp": "#ddlFrecuenciaHelp",
        "ddlFrecuanciaEnvioHelp":"#ddlFrecuanciaEnvioHelp",
        "txtRepeticionesSolicitudesHelp": "#txtRepeticionesSolicitudesHelp",
        "txtFechaEnvioSolicitudHelp": "#txtFechaEnvioSolicitudHelp",
        "txtEstado": "#IdEstado"

    },

    "Variables": {

        "CantidadMaxDias": 28,

        "DetallesCompletos": false,

        "SolicitudClonada": false,

        "EstadoRegistro": 0,

        "ListadoSolicitudes": [],

        "ListadoFormulario": []
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
                let listaFormularios = solicitud.FormulariosString;
                let envioProgramado = solicitud.EnvioProgramado == null ? "NO" : "SI";
                let EnProceso = solicitud.IdEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.EnProceso ? "SI" : "NO";
                let registroActivo = solicitud.IdEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.Activo;

                html = html + "<tr>";

                html = html + "<td>" + solicitud.Codigo + "</td>";
                html = html + "<td>" + solicitud.Nombre + "</td>";
                html = html + "<td>" + solicitud.Fuente.Fuente + "</td>";
                html = html + "<td>" + listaFormularios + "</td>";
                html = html + "<td>" + envioProgramado + "</td>";
                html = html + "<td>" + solicitud.Estado.Nombre + "</td >";
                //html = html + "<td>" + solicitud.IdFrecuenciaEnvio + "</td>";
           
                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Editar' class='btn-icon-base btn-edit'></button>";

                if (EnProceso == "SI") {

                    html = html + "<button type='button' data-toggle='tooltip' disabled data-placement='top' value=" + solicitud.id + " title='Clonar' class='btn-icon-base btn-clone'></button>";
                    html += "<button type='button' data-toggle='tooltip' disabled data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + solicitud.id + " class='btn-icon-base btn-power-on'></button>";
                } else {

                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Clonar' class='btn-icon-base btn-clone'></button>";

                    if (solicitud.IdEstadoRegistro== jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                        html += "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + solicitud.id + " class='btn-icon-base btn-power-off'></button>";
                    }
                    else {
                        html += "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + solicitud.id + " class='btn-icon-base btn-power-on'></button>";
                    }

                }

                html = html + " <button type='button' data-toggle='tooltip' data-placement='top' title='Visualizar' data-original-title='Visualizar' value=" + solicitud.id + " class='btn-icon-base btn-view'></button>"+
                              " <button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Eliminar' class='btn-icon-base btn-delete'></button > ";

                if (registroActivo) {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + "  title='Envío' class='btn-icon-base btn-sent'></button>";
                } else {
                    html = html + "<button type='button' data-toggle='tooltip' disabled data-placement='top' value=" + solicitud.id + "  title='Envío' class='btn-icon-base btn-sent'></button>";

                }

                if (envioProgramado == "NO" && !registroActivo) {
                    html = html + "<button type='button' data-toggle='tooltip'  disabled  data-placement='top' data-index=" + i + " value=" + solicitud.id + " title='Agregar Programación' class='btn-icon-base btn-calendar'></button></td></tr>";
                }
                else if (envioProgramado == "SI" && !registroActivo) {

                    html = html + "<button type='button' data-toggle='tooltip' disabled data-placement='top' data-index=" + i + " value=" + solicitud.id + " title='Eliminar Programación' class='btn-icon-base btn-calendar-disabled'></button></td></tr>";
                }
                else if (envioProgramado == "SI" && registroActivo) {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' data-index=" + i + " value=" + solicitud.id + " title='Eliminar Programación' class='btn-icon-base btn-calendar-disabled'></button></td></tr>";
                }

                else {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' data-index=" + i + " value=" + solicitud.id + " title='Agregar Programación' class='btn-icon-base btn-calendar'></button></td></tr>";

                }
            }

            $(JsSolicitud.Controles.TablaSolicitud).html(html);
            CargarDatasource();
            
        },

        "CargarTablaFormulario": function () {

            EliminarDatasource();
            let html = "";

            for (var i = 0; i < JsSolicitud.Variables.ListadoFormulario.length; i++) {
                let solicitud = JsSolicitud.Variables.ListadoFormulario[i];
                
                html = html + "<tr>";
              
       
                html = html + "<td>" + solicitud.Codigo + "</td>";
                html = html + "<td>" + solicitud.Nombre + "</td>";
                html = html + "<td>" + solicitud.EstadoRegistro.Nombre + "</td>";
                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + solicitud.id + " title='Eliminar' class='btn-icon-base btn-delete'></button></td></tr>";

            }

            $(JsSolicitud.Controles.TablaFormulario).html(html);
            CargarDatasource();

        },

        "RemoverItemDataTable": function (pDataTable, pItem) {

            $(pDataTable).DataTable().row($(pItem).parents('tr')).remove().draw();
        },

        "Detalles": function () {

            if (JsSolicitud.Variables.DetallesCompletos) {
                $(JsSolicitud.Controles.btnGuardarFormulario).prop("disabled", true);
                $(JsSolicitud.Controles.btnFinalizarSolicitud).prop("disabled", false);
            } else {
                $(JsSolicitud.Controles.btnGuardarFormulario).prop("disabled", false);
                $(JsSolicitud.Controles.btnFinalizarSolicitud).prop("disabled", true);
            }
        },

        "ValidarNombreyCodigo": function () {

            let validar = true;

            $(JsSolicitud.Controles.CodigoHelp).addClass("hidden");
            $(JsSolicitud.Controles.nombreHelp).addClass("hidden");
            $(JsSolicitud.Controles.txtCodigo).parent().addClass("has-error");
            $(JsSolicitud.Controles.txtNombre).parent().addClass("has-error");

            let codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            let nombre = $(JsSolicitud.Controles.txtNombre).val().trim();

            if (codigo.length == 0) {
                $(JsSolicitud.Controles.CodigoHelp).removeClass("hidden");
                $(JsSolicitud.Controles.txtCodigo).parent().addClass("has-error");
                validar = false;
            } else {
                $(JsSolicitud.Controles.txtCodigo).parent().removeClass("has-error");
            }

            if (nombre.length == 0) {
                $(JsSolicitud.Controles.nombreHelp).removeClass("hidden");
                $(JsSolicitud.Controles.txtNombre).parent().addClass("has-error");
                validar = false;
            } else {
                $(JsSolicitud.Controles.txtNombre).parent().removeClass("has-error");
            }

            return validar;
        },

        "ValidarControles": function () {
            let validar = true;

            $(JsSolicitud.Controles.FechaInicioHelp).addClass("hidden");
            $(JsSolicitud.Controles.txtFechaInicio).parent().removeClass("has-error");

            $(JsSolicitud.Controles.FechaFinHelp).addClass("hidden");
            $(JsSolicitud.Controles.txtFechaFin).parent().removeClass("has-error");

            $(JsSolicitud.Controles.FuentesHelp).addClass("hidden");
            $(JsSolicitud.Controles.ddlFuentes).parent().removeClass("has-error");

            $(JsSolicitud.Controles.FormulariosHelp).addClass("hidden");
            $(JsSolicitud.Controles.TxtCantidadFormulario).parent().removeClass("has-error");

            $(JsSolicitud.Controles.ddlMesSolicitudHelp).addClass("hidden");
            $(JsSolicitud.Controles.ddlMesSolicitud).parent().removeClass("has-error");

            $(JsSolicitud.Controles.ddlAnoSolicitudHelp).addClass("hidden");
            $(JsSolicitud.Controles.ddlAnoSolicitud).parent().removeClass("has-error");

            $(JsSolicitud.Controles.txtMensajeSolicitudHelp).addClass("hidden");
            $(JsSolicitud.Controles.txtMensajeSolicitud).parent().removeClass("has-error");

            let fechainicio = moment($(JsSolicitud.Controles.txtFechaInicio).val().trim());
            let fechaFin = moment($(JsSolicitud.Controles.txtFechaFin).val().trim());
            let fuentes = $(JsSolicitud.Controles.ddlFuentes).val().trim();
            let Frecuencia = $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val().trim();
            let CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val().trim();
            let mes = $(JsSolicitud.Controles.ddlMesSolicitud).val().trim();
            let anno = $(JsSolicitud.Controles.ddlAnoSolicitud).val().trim();
            let mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            if (!fechainicio.isValid()) {
                $(JsSolicitud.Controles.FechaInicioHelp).removeClass("hidden");
                $(JsSolicitud.Controles.txtFechaInicio).parent().addClass("has-error");
                validar = false;
            }
            if (!fechaFin.isValid()) {
                $(JsSolicitud.Controles.FechaFinHelp).removeClass("hidden");
                $(JsSolicitud.Controles.txtFechaFin).parent().addClass("has-error");
                validar = false;
            }
            if (fuentes.length == 0) {
                $(JsSolicitud.Controles.FuentesHelp).removeClass("hidden");
                $(JsSolicitud.Controles.ddlFuentes).parent().addClass("has-error");
                validar = false;
            }
            if (Frecuencia.length == 0) {
                $(JsSolicitud.Controles.ddlFrecuenciaHelp).removeClass("hidden");
                $(JsSolicitud.Controles.ddlFrecuenciaHelp).parent().addClass("has-error");
                validar = false;
            }
            if (CantidadFormulario.length == 0) {
                $(JsSolicitud.Controles.FormulariosHelp).removeClass("hidden");
                $(JsSolicitud.Controles.TxtCantidadFormulario).parent().addClass("has-error");
                validar = false;
            }
            if (mes.length == 0) {
                $(JsSolicitud.Controles.ddlMesSolicitudHelp).removeClass("hidden");
                $(JsSolicitud.Controles.ddlMesSolicitud).parent().addClass("has-error");
                validar = false;
            }
            if (anno.length == 0) {
                $(JsSolicitud.Controles.ddlAnoSolicitudHelp).removeClass("hidden");
                $(JsSolicitud.Controles.ddlAnoSolicitud).parent().addClass("has-error");
                validar = false;
            }
            if (mensaje.length == 0) {
                $(JsSolicitud.Controles.txtMensajeSolicitudHelp).removeClass("hidden");
                $(JsSolicitud.Controles.txtMensajeSolicitud).parent().addClass("has-error");
                validar = false;
            }
            return validar;
        },

        "ValidarFormularioWeb": function () {

            let validar = true;

            $(JsSolicitud.Controles.ddlVariableIndicadorHelp).addClass("hidden");
            $(JsSolicitud.Controles.ddlFormularioWeb).parent().removeClass("has-error");

            let Formulario = $(JsSolicitud.Controles.ddlFormularioWeb).val().trim();

            if (Formulario.length == 0) {
                $(JsSolicitud.Controles.ddlVariableIndicadorHelp).removeClass("hidden");
                $(JsSolicitud.Controles.ddlFormularioWeb).parent().addClass("has-error");
                validar = false;
            }

            return validar;
        },

        "ValidarEnvioProgramado": function () {

            let validar = true;

            let frecuencia = $(JsSolicitud.Controles.ddlFrecuencia).val().trim();
            let repeticiones = $(JsSolicitud.Controles.txtCantidadRepeticion).val().trim();
            let fecha = moment($(JsSolicitud.Controles.txtFechaCiclo).val().trim());

            if (frecuencia.length == 0) {
                $(JsSolicitud.Controles.ddlFrecuenciaHelp).removeClass("hidden");
                Validar = false;
            } else {
                $(JsSolicitud.Controles.ddlFrecuenciaHelp).addClass("hidden");
            }

            if (repeticiones.length == 0) {
                $(JsSolicitud.Controles.txtRepeticionesSolicitudesHelp).removeClass("hidden");
                Validar = false;
            }
            else {
                $(JsSolicitud.Controles.txtRepeticionesSolicitudesHelp).addClass("hidden");
            }

            if (!fecha.isValid()) {
                $(JsSolicitud.Controles.txtFechaEnvioSolicitudHelp).removeClass("hidden");
                validar = false;
            } else {
                $(JsSolicitud.Controles.txtFechaEnvioSolicitudHelp).addClass("hidden");
            }

            return validar;
        },

    },

    "Consultas": {

        "CargarCodigo": function (index) {

            if (JsSolicitud.Variables.esModoEliminar) {

                if (JsSolicitud.Variables.ListadoSolicitudes.length > index) {
                    JsSolicitud.Variables.ObjetoSolicitudes = JsSolicitud.Variables.ListadoSolicitudes[index];
                   /* '2022-10-11'*/
                    let dateTime = moment(JsSolicitud.Variables.ObjetoSolicitudes.EnvioProgramado.FechaCiclo);
                    $(JsSolicitud.Controles.txtSolicitudModal).val(JsSolicitud.Variables.ObjetoSolicitudes.Codigo);
                    $(JsSolicitud.Controles.ddlFrecuencia).val(JsSolicitud.Variables.ObjetoSolicitudes.EnvioProgramado.IdFrecuenciaEnvio);
                    $(JsSolicitud.Controles.ddlFrecuencia).trigger("change");
                    $(JsSolicitud.Controles.txtCantidadRepeticion).val(JsSolicitud.Variables.ObjetoSolicitudes.EnvioProgramado.CantidadRepeticion);
                    $(JsSolicitud.Controles.txtFechaCiclo).val(dateTime.format('YYYY-MM-DD'));
                }
            } else {
                if (JsSolicitud.Variables.ListadoSolicitudes.length > index) {
                    JsSolicitud.Variables.ObjetoSolicitudes = JsSolicitud.Variables.ListadoSolicitudes[index];
                    $(JsSolicitud.Controles.txtSolicitudModal).val(JsSolicitud.Variables.ObjetoSolicitudes.Codigo);
                    $(JsSolicitud.Controles.ddlFrecuencia).val(JsSolicitud.Variables.ObjetoSolicitudes.IdFrecuenciaEnvio).change();
                    $(JsSolicitud.Controles.ddlFrecuencia).prop("disabled", true);

                }
            }
        },

        "ValidarExistenciaSolicitud": function (idSolicitud, Eliminado = true) {

            $("#loading").fadeIn();

            let solicitud = new Object()

            solicitud.id = idSolicitud;

            execAjaxCall("/SolicitudFonatel/ValidarExistenciaSolicitud", "POST", solicitud)
                .then((obj) => {
                    if (obj.objetoRespuesta.length == 0) {

                        if (Eliminado) {

                            JsSolicitud.Consultas.EliminarSolicitud(idSolicitud);

                        } else {

                                    JsSolicitud.Consultas.CambiarEstadoDesactivado(idSolicitud);                        
                        }

                    } else {
                        let dependencias = '';
                        for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                            dependencias = dependencias + obj.objetoRespuesta[i] + "<br>"
                        }
                        if (Eliminado) {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("La Solicitud de Información ya ha sido enviada a la " + dependencias + "<br>¿Desea eliminarla?", jsMensajes.Variables.actionType.eliminar)
                                .set('onok', function (closeEvent) {
                                    JsSolicitud.Consultas.EliminarSolicitud(idSolicitud);
                                });
                        }
                        else {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("La Solicitud de Información ya ha sido enviada a la " + dependencias + "<br>¿Desea desactivarla?", jsMensajes.Variables.actionType.estado)
                                .set('onok', function (closeEvent) {
                                    JsSolicitud.Consultas.CambiarEstadoDesactivado(idSolicitud);
                                });
                        }


                    }
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EditarSolicitudParcial": function () {

            //ENCRIPTAR IDS

            $("#loading").fadeIn();

            let Solicitud = new Object();

            Solicitud.id = $(JsSolicitud.Controles.id).val();

            Solicitud.Codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            Solicitud.Nombre = $(JsSolicitud.Controles.txtNombre).val().trim();
            Solicitud.FechaInicio = $(JsSolicitud.Controles.txtFechaInicio).val();
            Solicitud.FechaFin = $(JsSolicitud.Controles.txtFechaFin).val();
            Solicitud.idFuenteRegistro = $(JsSolicitud.Controles.ddlFuentes).val();
            Solicitud.IdFrecuenciaEnvio = $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val();
            Solicitud.CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val();
            Solicitud.idMes = $(JsSolicitud.Controles.ddlMesSolicitud).val();
            Solicitud.idAnno = $(JsSolicitud.Controles.ddlAnoSolicitud).val();
            Solicitud.Mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            execAjaxCall("/SolicitudFonatel/EditarSolicitud", "POST", Solicitud)
                .then((obj) => {
                    if (obj.objetoRespuesta[0].CantidadFormulario == JsSolicitud.Variables.ListadoFormulario.length) {
                        JsSolicitud.Variables.DetallesCompletos = true;
                        JsSolicitud.Metodos.Detalles();
                    }
                    else {
                        JsSolicitud.Variables.DetallesCompletos = false;
                        JsSolicitud.Metodos.Detalles();
                    }
                    JsSolicitud.Consultas.CambiarFrecuencia(Solicitud.IdFrecuenciaEnvio);
                    $(JsSolicitud.Controles.step2).trigger('click');
                   
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
  
        "ClonarSolicitud": function () {

            $("#loading").fadeIn();

            let Solicitud = new Object();

            let id = ObtenerValorParametroUrl("id");

            Solicitud.id = id;
            Solicitud.Codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            Solicitud.Nombre = $(JsSolicitud.Controles.txtNombre).val().trim();

            Solicitud.FechaInicio = $(JsSolicitud.Controles.txtFechaInicio).val();
            Solicitud.FechaFin = $(JsSolicitud.Controles.txtFechaFin).val();

            Solicitud.idFuenteRegistro = $(JsSolicitud.Controles.ddlFuentes).val();
            Solicitud.CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val();
            Solicitud.IdFrecuenciaEnvio = $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val();
            Solicitud.idMes = $(JsSolicitud.Controles.ddlMesSolicitud).val();
            Solicitud.idAnno = $(JsSolicitud.Controles.ddlAnoSolicitud).val();
            Solicitud.Mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            execAjaxCall("/SolicitudFonatel/ClonarSolicitud", "POST", Solicitud)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido creada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/SolicitudFonatel/index";
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "CambiarEstadoDesactivado": function (idSolicitud) {

            $("#loading").fadeIn();
            let Solicitud = new Object()

            Solicitud.id = idSolicitud;

            execAjaxCall("/SolicitudFonatel/CambiarEstadoDesactivado", "POST", Solicitud)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido desactivada")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });

                }).catch((data) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {

                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "CambiarEstadoActivado": function (idSolicitud) {

            $("#loading").fadeIn();
            let Solicitud = new Object()
            Solicitud.id = idSolicitud;

            execAjaxCall("/SolicitudFonatel/CambiarEstadoActivado", "POST", Solicitud)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido activada")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });

                }).catch((data) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {

                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarDetalleSolicitud": function () {

            $("#loading").fadeIn();

            let Solicitud = new Object();

            Solicitud.id = ObtenerValorParametroUrl("id");
            Solicitud.Formularioid = $(JsSolicitud.Controles.ddlFormularioWeb).val();

            execAjaxCall("/SolicitudFonatel/InsertarDetalleSolicitud", "POST", Solicitud)
                .then((obj) => {


                    jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido agregado")
                        .set('onok', function (closeEvent) {

                            if (obj.objetoRespuesta[0].Completo) {
                                JsSolicitud.Variables.DetallesCompletos = true;
                                JsSolicitud.Metodos.Detalles();
                            }

                            $(JsSolicitud.Controles.ddlFormularioWeb).val("").trigger('change');

                            if ($(JsSolicitud.Controles.TablaFormulario).length > 0) {
                                JsSolicitud.Consultas.ConsultaListaFormulario();
                            }

                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarDetalleSolicitud": function (idSolicitud, idFormularioWeb) {

            $("#loading").fadeIn();

            execAjaxCall("/SolicitudFonatel/EliminarDetalleSolicitud", "POST", { idSolicitud: idSolicitud, idFormularioWeb: idFormularioWeb })
                .then((obj) => {
                    JsSolicitud.Variables.DetallesCompletos = false;
                    JsSolicitud.Metodos.Detalles();
                    JsSolicitud.Metodos.RemoverItemDataTable(JsSolicitud.Controles.TablaFormularioElemento, `button[value='${idFormularioWeb}']`)

                    jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido eliminado")
                        .set('onok', function (closeEvent) {
                            JsSolicitud.Variables.ListadoFormulario = obj.objetoRespuesta;
                        })

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarEnvioProgramado": function () {

                $("#loading").fadeIn();

                let objeto = new Object();

                objeto.id = $(JsSolicitud.Controles.txtSolicitudEnvio).val();
                objeto.IdFrecuenciaEnvio = $(JsSolicitud.Controles.ddlFrecuencia).val();
                objeto.CantidadRepeticion = $(JsSolicitud.Controles.txtCantidadRepeticion).val();
                objeto.FechaCiclo = $(JsSolicitud.Controles.txtFechaCiclo).val();
                objeto.CodigoSolicitud = $(JsSolicitud.Controles.txtSolicitudModal).val();

                execAjaxCall("/SolicitudFonatel/InsertarEnvioProgramado", "POST", objeto)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("La Programación ha sido creada")
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }).catch((obj) => {
                        if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) { });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                                .set('onok', function (closeEvent) { })
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
        },

        "EditarSolicitud": function () {

            //ENCRIPTAR IDS

            $("#loading").fadeIn();

            let Solicitud = new Object();

            Solicitud.id = $(JsSolicitud.Controles.id).val();
            Solicitud.Codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            Solicitud.Nombre = $(JsSolicitud.Controles.txtNombre).val().trim();
            Solicitud.FechaInicio = $(JsSolicitud.Controles.txtFechaInicio).val();
            Solicitud.FechaFin = $(JsSolicitud.Controles.txtFechaFin).val();
            Solicitud.idFuenteRegistro = $(JsSolicitud.Controles.ddlFuentes).val();
            Solicitud.IdFrecuenciaEnvio = $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val();
            Solicitud.CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val();
            Solicitud.idMes = $(JsSolicitud.Controles.ddlMesSolicitud).val();
            Solicitud.idAnno = $(JsSolicitud.Controles.ddlAnoSolicitud).val();
            Solicitud.Mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            execAjaxCall("/SolicitudFonatel/EditarSolicitud", "POST", Solicitud)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido editada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/SolicitudFonatel/index";
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ClonarSolicitudParcial": function () {

            $("#loading").fadeIn();

            let Solicitud = new Object();

            let id = ObtenerValorParametroUrl("id");

            Solicitud.id = id;
            Solicitud.Codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            Solicitud.Nombre = $(JsSolicitud.Controles.txtNombre).val().trim();

            Solicitud.FechaInicio = $(JsSolicitud.Controles.txtFechaInicio).val();
            Solicitud.FechaFin = $(JsSolicitud.Controles.txtFechaFin).val();

            Solicitud.idFuenteRegistro = $(JsSolicitud.Controles.ddlFuentes).val();
            Solicitud.IdFrecuenciaEnvio = $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val();
            Solicitud.CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val();
            Solicitud.idMes = $(JsSolicitud.Controles.ddlMesSolicitud).val();
            Solicitud.idAnno = $(JsSolicitud.Controles.ddlAnoSolicitud).val();
            Solicitud.Mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            execAjaxCall("/SolicitudFonatel/ClonarSolicitud", "POST", Solicitud)
                .then((obj) => {
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);

                    if (obj.objetoRespuesta[0].CantidadFormulario == JsSolicitud.Variables.ListadoFormulario.length) {
                        JsSolicitud.Variables.DetallesCompletos = true;
                        JsSolicitud.Metodos.Detalles();
                    }
                    else {
                        JsSolicitud.Variables.DetallesCompletos = false;
                        JsSolicitud.Metodos.Detalles();
                    }

                    $(JsSolicitud.Controles.step2).trigger('click');
                    
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarSolicitudParcial": function () {

            $("#loading").fadeIn();

            let Solicitud = new Object();
            Solicitud.id = ObtenerValorParametroUrl("id");
            Solicitud.Codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            Solicitud.Nombre = $(JsSolicitud.Controles.txtNombre).val().trim();

            Solicitud.FechaInicio = $(JsSolicitud.Controles.txtFechaInicio).val();
            Solicitud.FechaFin = $(JsSolicitud.Controles.txtFechaFin).val();

            Solicitud.idFuenteRegistro = $(JsSolicitud.Controles.ddlFuentes).val();
            Solicitud.IdFrecuenciaEnvio = $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val();
            Solicitud.CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val();
            Solicitud.idMes = $(JsSolicitud.Controles.ddlMesSolicitud).val();
            Solicitud.idAnno = $(JsSolicitud.Controles.ddlAnoSolicitud).val();
            Solicitud.Mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            execAjaxCall("/SolicitudFonatel/InsertarSolicitud", "POST", Solicitud)
                .then((obj) => {
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);

                    if (obj.objetoRespuesta[0].CantidadFormulario == JsSolicitud.Variables.ListadoFormulario.length) {
                        JsSolicitud.Variables.DetallesCompletos = true;
                        JsSolicitud.Metodos.Detalles();
                    }
                    else {
                        JsSolicitud.Variables.DetallesCompletos = false;
                        JsSolicitud.Metodos.Detalles();
                    }

                    if (JsSolicitud.Metodos.ValidarControles()) {
                        $(JsSolicitud.Controles.step2).trigger('click');
                    }

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "CambiarEstadoFinalizado": function (idSolicitud) {
            $("#loading").fadeIn();
            let Solicitud = new Object()
            Solicitud.id = idSolicitud;
            execAjaxCall("/SolicitudFonatel/CambiarEstadoActivado", "POST", Solicitud)
                .then((obj) => {

                    jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido creada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/Index"; })

                }).catch((data) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {

                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },


        "EliminarSolicitud": function (idSolicitud) {

            $("#loading").fadeIn();

            execAjaxCall("/SolicitudFonatel/EliminarSolicitud", "POST", { idSolicitud: idSolicitud })
                .then((obj) => {

                    JsSolicitud.Metodos.RemoverItemDataTable(JsSolicitud.Controles.TablaSolicitudElemento, `button[value='${idSolicitud}']`)

                    jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido eliminada")
                        .set('onok', function (closeEvent) {
                            JsSolicitud.Variables.ListadoSolicitudes = obj.objetoRespuesta;
                        });

                }).catch((obj) => {

                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarSolicitud": function () {

            $("#loading").fadeIn();

            let Solicitud = new Object();

            Solicitud.Codigo = $(JsSolicitud.Controles.txtCodigo).val().trim();
            Solicitud.Nombre = $(JsSolicitud.Controles.txtNombre).val().trim();

            Solicitud.FechaInicio = $(JsSolicitud.Controles.txtFechaInicio).val();
            Solicitud.FechaFin = $(JsSolicitud.Controles.txtFechaFin).val();

            Solicitud.idFuenteRegistro = $(JsSolicitud.Controles.ddlFuentes).val();
            Solicitud.IdFrecuenciaEnvio = $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val();
            Solicitud.CantidadFormulario = $(JsSolicitud.Controles.TxtCantidadFormulario).val();
            Solicitud.idMes = $(JsSolicitud.Controles.ddlMesSolicitud).val();
            Solicitud.idAnno = $(JsSolicitud.Controles.ddlAnoSolicitud).val();
            Solicitud.Mensaje = $(JsSolicitud.Controles.txtMensajeSolicitud).val().trim();

            execAjaxCall("/SolicitudFonatel/InsertarSolicitud", "POST", Solicitud)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido creada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/SolicitudFonatel/index";
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarEnvioProgramado": function () {

            $("#loading").fadeIn();

            let objeto = new Object();
            objeto.id = $(JsSolicitud.Controles.txtSolicitudEnvio).val();
            objeto.CodigoSolicitud = $(JsSolicitud.Controles.txtSolicitudModal).val();

            execAjaxCall("/SolicitudFonatel/EliminarEnvioProgramado", "POST", objeto)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Programación ha sido eliminada")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EnviarCorreo": function (id) {

            $("#loading").fadeIn();

            let objeto = new Object();
            objeto.IdSolicitudString = id;
            objeto.Enviado = false;
            objeto.EnvioProgramado = false,
            objeto.EjecutaJob = true
            execAjaxCall("/SolicitudFonatel/EnvioSolicitud", "POST", objeto = objeto)
                .then((obj) => {
                    if (obj.objetoRespuesta) {
                        jsMensajes.Metodos.OkAlertModal("La Solicitud de Información ha sido enviada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/SolicitudFonatel/index" });
                    } else {

                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                  
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ValidarFormularioCrear": function () {

            let codigo = $(JsSolicitud.Controles.txtCodigo).val() == undefined ? "" : $(JsSolicitud.Controles.txtCodigo).val().trim();
            let nombre = $(JsSolicitud.Controles.txtNombre).val() == undefined ? "" : $(JsSolicitud.Controles.txtNombre).val().trim();

            if (codigo.length > 0 && nombre.length > 0
                && $(JsSolicitud.Controles.txtFechaInicio).val().trim() != "0001-01-01" && $(JsSolicitud.Controles.txtFechaFin).val().trim() != "0001-01-01"
                && $(JsSolicitud.Controles.ddlFuentes).val().trim().length > 0 && $(JsSolicitud.Controles.ddlFrecuanciaEnvio).val().trim().length > 0 && $(JsSolicitud.Controles.TxtCantidadFormulario).val().trim() != 0
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
        },

        "ConsultaListaFormulario": function () {

            $("#loading").fadeIn();

            let Solicitud = new Object();
            Solicitud.id = ObtenerValorParametroUrl("id");


            execAjaxCall("/SolicitudFonatel/ObtenerListaFormulario", "GET", Solicitud)
                .then((obj) => {

                    JsSolicitud.Variables.ListadoFormulario = obj.objetoRespuesta;

                    CantidadDetalle = JsSolicitud.Variables.CantidadDetalles = JsSolicitud.Variables.ListadoFormulario.length;

                    JsSolicitud.Metodos.CargarTablaFormulario();

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
        },
        "CambiarFrecuencia": function (id) {
            $("#loading").fadeIn();
            let Solicitud = new Object();
            Solicitud.id = id;

          
            execAjaxCall("/SolicitudFonatel/ObtenerFormulariosxFrecuencia", "GET", Solicitud)
                .then((obj) => {
                    var comboFormularios = document.getElementById("ddlFormularioWeb");
                    comboFormularios.innerHTML = '';
                    // comboFormularios.empty();
                    for (var i = 1; i <= obj.objetoRespuesta.length; i++) {
                        comboFormularios.options[i] = new Option(obj.objetoRespuesta[i - 1].Text, obj.objetoRespuesta[i - 1].Value);
                    }
                    
                }).catch((data) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {

                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
    }
}
//CAMBIO DE FRECUENCIA
$(document).on("change", JsSolicitud.Controles.ddlFrecuanciaEnvio, function (e) {
    let id = $(this).val();
    // alert(id);
    JsSolicitud.Consultas.CambiarFrecuencia(id);
});


$(document).on("click", JsSolicitud.Controles.btnFinalizarSolicitud, function (e) {
    e.preventDefault();

    let id = ObtenerValorParametroUrl("id");

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea guardar la Solicitud de Información?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsSolicitud.Consultas.CambiarEstadoFinalizado(id);
        });
});

$(document).on("click", JsSolicitud.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/SolicitudFonatel/Index";
        });
});

$(document).on("click", JsSolicitud.Controles.btnDeleteFormulario, function (e) {

    let idSolicitud = ObtenerValorParametroUrl("id");

    let idFormularioWeb = $(this).val();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Formulario Web?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {

            JsSolicitud.Consultas.EliminarDetalleSolicitud(idSolicitud, idFormularioWeb);
        });
});

$(document).on("click", JsSolicitud.Controles.btnEditarSolicitud, function () {
    if (consultasFonatel) { return; }

    let id = encodeURIComponent($(this).val());
    window.location.href = "/Fonatel/SolicitudFonatel/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});




$(document).on("click", JsSolicitud.Controles.btnEliminarProgramacion, function () {
    JsSolicitud.Variables.esModoEliminar = true;
    let CargarCodigo = $(this).attr("data-index");
    JsSolicitud.Consultas.CargarCodigo(CargarCodigo);

    let id = $(this).val();
    $(JsSolicitud.Controles.txtSolicitudEnvio).val(id);

    $(JsSolicitud.Controles.txtSolicitudModal).prop("disabled", true);
    $(JsSolicitud.Controles.ddlFrecuencia).prop("disabled", true);
    $(JsSolicitud.Controles.txtCantidadRepeticion).prop("disabled", true);
    $(JsSolicitud.Controles.txtFechaCiclo).prop("disabled", true);

    $(JsSolicitud.Controles.ddlFrecuenciaHelp).addClass("hidden");
    $(JsSolicitud.Controles.txtRepeticionesSolicitudesHelp).addClass("hidden");
    $(JsSolicitud.Controles.txtFechaEnvioSolicitudHelp).addClass("hidden");


    $(JsSolicitud.Controles.btnEliminarSolicituProgramardEnvio).show();
    $(JsSolicitud.Controles.btnGuardarEnvio).hide();
    $(JsSolicitud.Controles.modalEnvio).modal('show');

});

$(document).on("click", JsSolicitud.Controles.btnCloneSolicitud, function () {
    if (consultasFonatel) { return; }
    let id = encodeURIComponent($(this).val());
    
            window.location.href = "/Fonatel/SolicitudFonatel/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
     
});

$(document).on("click", JsSolicitud.Controles.btnGuardarFormulario, function (e) {
    e.preventDefault();

    if (JsSolicitud.Metodos.ValidarFormularioWeb()) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Formulario Web a la Solicitud?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsSolicitud.Consultas.InsertarDetalleSolicitud();
            });
    }
});

$(document).on("click", JsSolicitud.Controles.btnCancelarFormulario, function (e) {
    e.preventDefault();
    $(JsSolicitud.Controles.ddlFormularioWeb).val("").trigger('change');
});

$(document).on("click", JsSolicitud.Controles.btnEliminarProgramacion, function () {

    let id = $(this).val();
    $(JsSolicitud.Controles.txtSolicitudEnvio).val(id);

    $(JsSolicitud.Controles.ddlFrecuenciaHelp).addClass("hidden");
    $(JsSolicitud.Controles.txtRepeticionesSolicitudesHelp).addClass("hidden");
    $(JsSolicitud.Controles.txtFechaEnvioSolicitudHelp).addClass("hidden");
    $(JsSolicitud.Controles.btnEliminarSolicituProgramardEnvio).show();
    $(JsSolicitud.Controles.btnGuardarEnvio).hide();
    $(JsSolicitud.Controles.modalEnvio).modal('show');

});

$(document).on("click", JsSolicitud.Controles.btnGuardarEnvio, function (e) {

    e.preventDefault();

    if (JsSolicitud.Metodos.ValidarEnvioProgramado()) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la programación a la Solicitud de Información?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                $(JsSolicitud.Controles.modalEnvio).modal('hide');
                JsSolicitud.Consultas.InsertarEnvioProgramado();
        });
    }

});

$(document).on("click", JsSolicitud.Controles.btnGuardarSolicitud, function (e) {
    e.preventDefault();

    let modo = $(JsSolicitud.Controles.txtModo).val();

    let ObtenerEstadoRegistro = $(JsSolicitud.Controles.txtEstado).val();
    JsSolicitud.Variables.EstadoRegistro = parseInt(ObtenerEstadoRegistro);

    let CamposVacios = "Existen campos vacíos. "

    if (JsSolicitud.Metodos.ValidarNombreyCodigo()) {

        if (modo == jsUtilidades.Variables.Acciones.Editar) {

            if (JsSolicitud.Variables.EstadoRegistro == jsUtilidades.Variables.EstadoRegistros.Activo) {

                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Solicitud de Información?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        JsSolicitud.Consultas.EditarSolicitud();
                    })
                    .set('oncancel', function (closeEvent) {
                        JsSolicitud.Metodos.ValidarControles();
                    });
            }
            else {

                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Solicitud de Información?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        JsSolicitud.Consultas.EditarSolicitud();
                    })
                    .set('oncancel', function (closeEvent) {
                        JsSolicitud.Metodos.ValidarControles();
                    });
            }


        } else if (modo == jsUtilidades.Variables.Acciones.Clonar){
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Solicitud de Información?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsSolicitud.Consultas.ClonarSolicitud();
                })
                .set('oncancel', function (closeEvent) {
                    JsSolicitud.Metodos.ValidarControles();
                });

        }
        else {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CamposVacios + "¿Desea realizar un guardado parcial de la Solicitud de Información?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsSolicitud.Consultas.InsertarSolicitud();
                })
                .set('oncancel', function (closeEvent) {
                    JsSolicitud.Metodos.ValidarControles();
                });
        }
    }
});

$(document).on("click", JsSolicitud.Controles.btnDeleteSolicitud, function (e) {
    if (consultasFonatel) { return; }
    let id = $(this).val();

    console.log(id);

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Solicitud de Información?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsSolicitud.Consultas.ValidarExistenciaSolicitud(id);
        });


});

$(document).on("click", JsSolicitud.Controles.btnsent, function (e) {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea enviar la Solicitud de Información?", null, "Enviar Registro")
        .set('onok', function (closeEvent) {
            
            JsSolicitud.Consultas.EnviarCorreo(id);
        });
});

$(document).on("click", JsSolicitud.Controles.btnDesactivadoSolicitud, function (e) {
    if (consultasFonatel) { return; }
    e.preventDefault();

    let id = $(this).val();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Solicitud de Información?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsSolicitud.Consultas.CambiarEstadoActivado(id);
        });
});

$(document).on("click", JsSolicitud.Controles.btnActivadoSolicitud, function (e) {
    if (consultasFonatel) { return; }
    e.preventDefault();

    let id = $(this).val();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Solicitud de Información?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsSolicitud.Consultas.CambiarEstadoDesactivado(id);

        });

});

$(document).on("click", JsSolicitud.Controles.btnSiguienteSolicitud, function (e) {

    e.preventDefault();

    let modo = $(JsSolicitud.Controles.txtModo).val();

    if (modo == jsUtilidades.Variables.Acciones.Editar) {

        if (JsSolicitud.Metodos.ValidarControles()) {
            JsSolicitud.Consultas.EditarSolicitudParcial();
        }

    } else if (modo == jsUtilidades.Variables.Acciones.Clonar) {

        if (JsSolicitud.Metodos.ValidarControles()) {
            
            JsSolicitud.Consultas.ClonarSolicitudParcial();
        }

    }
    else {
        JsSolicitud.Consultas.InsertarSolicitudParcial();
    }
});

$(document).on("click", JsSolicitud.Controles.btnEnvioSolicitud, function (e) {
    if (consultasFonatel) { return; }
    let CargarCodigo = $(this).attr("data-index");
    JsSolicitud.Consultas.CargarCodigo(CargarCodigo);
    let id = $(this).val();
    $(JsSolicitud.Controles.txtSolicitudEnvio).val(id);
    $(JsSolicitud.Controles.txtSolicitudModal).prop("disabled", false);
    $(JsSolicitud.Controles.ddlFrecuencia).prop("disabled", true);
    $(JsSolicitud.Controles.txtCantidadRepeticion).prop("disabled", false);
    $(JsSolicitud.Controles.txtFechaCiclo).prop("disabled", false);
    $(JsSolicitud.Controles.ddlFrecuenciaHelp).addClass("hidden");
    $(JsSolicitud.Controles.txtRepeticionesSolicitudesHelp).addClass("hidden");
    $(JsSolicitud.Controles.txtFechaEnvioSolicitudHelp).addClass("hidden");
    $(JsSolicitud.Controles.btnEliminarSolicituProgramardEnvio).hide();
    $(JsSolicitud.Controles.txtSolicitudModal).prop("disabled", true);
    $(JsSolicitud.Controles.btnGuardarEnvio).show();
    $(JsSolicitud.Controles.modalEnvio).modal('show');
});

$(document).on("click", JsSolicitud.Controles.btnEliminarSolicituProgramardEnvio, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Programación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            $(JsSolicitud.Controles.modalEnvio).modal('hide');
            JsSolicitud.Consultas.EliminarEnvioProgramado();
        });
});

$(document).on("click", JsSolicitud.Controles.btnCancelarEnvio, function () {
    $(JsSolicitud.Controles.modalEnvio).modal('hide');
});

$(document).on("click", JsSolicitud.Controles.btnAtrasSolicitud, function (e) {
    e.preventDefault();
    InsertarParametroUrl("modo", jsUtilidades.Variables.Acciones.Editar);
    $(JsSolicitud.Controles.txtModo).val(jsUtilidades.Variables.Acciones.Editar);
    $(JsSolicitud.Controles.id).val(ObtenerValorParametroUrl("id"));
    $(JsSolicitud.Controles.step1).trigger('click');
});

$(document).on("keyup", JsSolicitud.Controles.ControlesStep1, function (e) {
    JsSolicitud.Consultas.ValidarFormularioCrear();
});

$(document).on("change", JsSolicitud.Controles.ControlesStep1, function (e) {
    JsSolicitud.Consultas.ValidarFormularioCrear();
});
$(document).on("click", JsSolicitud.Controles.btnViewSolicitud, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/SolicitudFonatel/Visualizacion?id=" + id;
});
////CAMBIO DE FRECUENCIA
//$(document).on("change", JsSolicitud.Controles.ddlFrecuanciaEnvio, function (e) {
//    let id = $(this).val();
//    // alert(id);
//    $.ajax({
//        url: jsUtilidades.Variables.urlOrigen + '/SolicitudFonatel/ObtenerFormulariosxFrecuencia',
//        type: "GET",
//        dataType: "JSON",
//        beforeSend: function () {
//            $("#loading").fadeIn();
//        },
//        data: { id },
//        success: function (obj) {
//            $("#loading").fadeOut();
//            var comboFormularios = document.getElementById("ddlFormularioWeb");
//                comboFormularios.innerHTML = '';
//               // comboFormularios.empty();
//                comboFormularios.options[0] = new Option("Seleccione", -1);
//            for (var i = 1; i <= obj.objetoRespuesta.length; i++) {
//                comboFormularios.options[i] = new Option(obj.objetoRespuesta[i - 1].Text, obj.objetoRespuesta[i - 1].Value);
//   s         }
            
//        }
//    }).fail(function (obj) {
//        $("#loading").fadeOut();
//    })
//});


$(function () {

    let modo = $.urlParam("modo");

    if ($(JsSolicitud.Controles.TablaSolicitud).length > 0) {
        JsSolicitud.Consultas.ConsultaListaSolicitudes();
    }
    else if (modo == jsUtilidades.Variables.Acciones.Editar) {
        $(JsSolicitud.Controles.txtCodigo).prop("disabled", true);
        JsSolicitud.Consultas.ValidarFormularioCrear();
    }
    else if ($("#TablaVisualizaFormulario").length==0) {
        JsSolicitud.Consultas.ValidarFormularioCrear();
    }
    if ($(JsSolicitud.Controles.TablaFormulario).length > 0) {
        JsSolicitud.Consultas.ConsultaListaFormulario();
    }
     if ($(JsSolicitud.Controles.TxtCantidadFormulario).val() == 0) {
        $(JsSolicitud.Controles.TxtCantidadFormulario).val("");
    }



});












