JsFuentes = {
    "Controles": {
        "btnstep": ".step_navigation_fuentes div",
        "btnGuardarFuente": "#btnGuardarFuente",
        "btnSiguienteFuente":"#btnSiguienteFuente",
        "btnEditarFuente": "#TablaFuentes tbody tr td .btn-edit",
        "btnBorrarFuente": "#TablaFuentes tbody tr td .btn-delete",
        "btnBorrarDetalle": "#TableDetalleFuentes tbody tr td .btn-delete",
        "btnEditarDetalle": "#TableDetalleFuentes tbody tr td .btn-edit",
        "btnAddFuente": "#TablaFuentes tbody tr td .btn-add",
        "divContenedor": ".divContenedor_fuentes",
        "btnGuardarDestinatario": "#btnGuardarDestinatario",
        "btnGuardarFuentesCompleto": "#btnGuardarFuentesCompleto",
        "btnAtrasFuentes": "#btnAtrasFuentes",
        "btnCancelar": "#btnCancelarFuente",
        "TablaFuentes": "#TablaFuentes tbody",
        "ControlesStep1":"#step-1 div form div div div div input",
        "tabladetalle":"#TableDetalleFuentes tbody",   
        "CorreoHelp":"#CorreoHelp",
        "nombreHelp":"#nombreHelp",
        "txtNombre": "#txtNombre",
        "txtCorreo": "#txtCorreo",
        "txtidDetalleFuente":"#txtidDetalleFuente",
        "txtIdUsuarioDetalle": "#txtIdUsuarioDetalle",
        "FuenteHelp": "#FuenteHelp",
        "CantidadDetalleHelp": "#CantidadDetalleHelp",
        "txtFuente": "#txtFuente",
        "txtCantidad": "#txtCantidad",
        "step1": "a[href='#step-1']",
        "divstepwizard":".stepwizard-step",
        "step2": "a[href='#step-2']"
    },
    "Variables": {
        "ListadoFuentes": [],
        "ListaDestinatarios": [],
        "esModoEdicion": false,
    },

    "Metodos": {

        "HabilitarBotones": function () {

            let CantidadDestinatario = $(JsFuentes.Controles.txtCantidad).val();

            if (CantidadDestinatario == 0) {
                $(JsFuentes.Controles.btnGuardarDestinatario).prop("disabled", true);
                $(JsFuentes.Controles.btnGuardarFuentesCompleto).prop("disabled", true);
            }
            else if (CantidadDestinatario > JsFuentes.Variables.ListaDestinatarios.length) {
                $(JsFuentes.Controles.btnGuardarDestinatario).prop("disabled", false);
                $(JsFuentes.Controles.btnGuardarFuentesCompleto).prop("disabled", true);
            }
            else {
                $(JsFuentes.Controles.btnGuardarDestinatario).prop("disabled", true);
                $(JsFuentes.Controles.btnGuardarFuentesCompleto).prop("disabled", false);
            }
        },
        "CargarTablaFuentes": function () {
            let html = "";
            EliminarDatasource();
            for (var i = 0; i < JsFuentes.Variables.ListadoFuentes.length; i++) {

                let fuente = JsFuentes.Variables.ListadoFuentes[i];
                html = html + "<tr><th scope='row'>" + fuente.Fuente + "</th>" +
                    "<td>" + fuente.CantidadDestinatario + "/" + fuente.DetalleFuentesRegistro.length + "</td><td>" + fuente.EstadoRegistro.Nombre + "</td>";
                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value='"+fuente.id+"' title='Editar' class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value='" + fuente.id +"' title='Eliminar' class='btn-icon-base btn-delete'></button></td></tr>";
            }
            $(JsFuentes.Controles.TablaFuentes).html(html);
            CargarDatasource();

            JsFuentes.Variables.ListadoFuentes = [];
        },
        "CargarTablaDestinatarios": function () {
            EliminarDatasource();
            //if (JsFuentes.Variables.ListaDestinatarios.length > 0) {
            //    if (JsFuentes.Variables.ListaDestinatarios[0].CantidadDisponible >= 0) {
            //        $(JsFuentes.Controles.btnGuardarDestinatario).prop("disabled", true);
            //        $(JsFuentes.Controles.btnGuardarFuentesCompleto).prop("disabled", false);
            //    }
            //    else {
            //        $(JsFuentes.Controles.btnGuardarDestinatario).prop("disabled", false);
            //        $(JsFuentes.Controles.btnGuardarFuentesCompleto).prop("disabled", true);
            //    }
            //}
            let html = "";
            for (var i = 0; i < JsFuentes.Variables.ListaDestinatarios.length; i++) {
                let destinatario = JsFuentes.Variables.ListaDestinatarios[i];
                html = html + "<tr><th scope='row'>" + destinatario.NombreDestinatario + "</th><td>" + destinatario.CorreoElectronico + "</td>";
                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + destinatario.idDetalleFuente + " title='Editar' class='btn-icon-base btn-edit'></button>";
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' value=" + destinatario.idDetalleFuente + " title='Eliminar' class='btn-icon-base btn-delete '></button></td></tr>";
                html = html + "</tr>"
               
            }
            $(JsFuentes.Controles.tabladetalle).html(html);
            CargarDatasource();
            JsFuentes.Variables.ListaDestinatarios  = [];
        },
        "ValidarFuente": function () {
            $(JsFuentes.Controles.FuenteHelp).addClass("hidden");
            $(JsFuentes.Controles.CantidadDetalleHelp).addClass("hidden");
            let validar = true;
            if ($(JsFuentes.Controles.txtFuente).val().trim().length == 0) {
                $(JsFuentes.Controles.FuenteHelp).removeClass("hidden");
                validar = false;
            }
            if ($(JsFuentes.Controles.txtCantidad).val().trim().length == 0 || $(JsFuentes.Controles.txtCantidad).val()<1 ) {
                $(JsFuentes.Controles.CantidadDetalleHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },
        "ValidarFuenteParcial": function () {
            $(JsFuentes.Controles.FuenteHelp).addClass("hidden");
            $(JsFuentes.Controles.CantidadDetalleHelp).addClass("hidden");
            let validar = true;
            if ($(JsFuentes.Controles.txtFuente).val().trim().length == 0) {
                $(JsFuentes.Controles.FuenteHelp).removeClass("hidden");
                validar = false;
            }
            
            return validar;
        },
        "ValidarFormularioDetalle": function () {
            $(JsFuentes.Controles.nombreHelp).addClass("hidden");
            $(JsFuentes.Controles.CorreoHelp).addClass("hidden");
            $(JsFuentes.Controles.txtNombre).parent().removeClass("has-error");
            $(JsFuentes.Controles.txtCorreo).parent().removeClass("has-error");
            let validar = true;
            if ($(JsFuentes.Controles.txtNombre).val().trim().length == 0) {
                $(JsFuentes.Controles.nombreHelp).removeClass("hidden");
                $(JsFuentes.Controles.txtNombre).parent().addClass("has-error");
                validar = false;
            }
            if ($(JsFuentes.Controles.txtCorreo).val().trim().length == 0) {
                $(JsFuentes.Controles.CorreoHelp).removeClass("hidden");
                $(JsFuentes.Controles.txtCorreo).parent().addClass("has-error");
                validar = false;
            }
            return validar;
        }
    },

    "Consultas": {
        "ConsultaListaFuentes": function () {
            $("#loading").fadeIn();
            execAjaxCall("/Fuentes/ObtenerListaFuentes", "GET")
                .then((data) => {
                    JsFuentes.Variables.ListadoFuentes = data.objetoRespuesta;
                    JsFuentes.Metodos.CargarTablaFuentes();
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
        "EliminarFuente": function (id) {
            let fuente = new Object();
            fuente.id = id;
            $("#loading").fadeIn();
            execAjaxCall("/Fuentes/EliminarFuente", "POST", fuente)
                .then((data) => {
                    jsMensajes.Metodos.OkAlertModal("La Fuente ha sido eliminada ")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
             });
        },
        "AgregarDestinatario": function (id) {
            let destinatario = new Object()
            destinatario.NombreDestinatario = $(JsFuentes.Controles.txtNombre).val();
            destinatario.CorreoElectronico = $(JsFuentes.Controles.txtCorreo).val();
            destinatario.fuenteId =ObtenerValorParametroUrl('id');
            destinatario.idDetalleFuente = id;
            destinatario.idUsuario = $(JsFuentes.Controles.txtIdUsuarioDetalle).val();
            $("#loading").fadeIn();
            execAjaxCall("/Fuentes/AgregarDestinatario", "POST", destinatario)
                .then((obj) => {
                   
                    let mensaje = "El Destinatario ha sido agregado";
                    if (destinatario.idDetalleFuente!= "") {
                        mensaje = "El Destinatario ha sido editado"
                    }
                    jsMensajes.Metodos.OkAlertModal(mensaje)
                        .set('onok', function (closeEvent) {
                            JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta[0].DetalleFuentesRegistro;
                           

                            $(JsFuentes.Controles.txtNombre).val("");
                            $(JsFuentes.Controles.txtCorreo).val("");
                            $(JsFuentes.Controles.txtidDetalleFuente).val("");
                            $(JsFuentes.Controles.txtIdUsuarioDetalle).val("");
                            JsFuentes.Metodos.HabilitarBotones();
                            JsFuentes.Metodos.CargarTablaDestinatarios();
                     });
                }).catch((data) => {
                    if (data.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(data.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
             });
        },
        "AgregarFuente": async function (parcial) {
            $("#loading").fadeIn();
            let objetoFuente = new Object()
            objetoFuente.Fuente = $(JsFuentes.Controles.txtFuente).val();
            objetoFuente.CantidadDestinatario = $(JsFuentes.Controles.txtCantidad).val();
            objetoFuente.id =ObtenerValorParametroUrl("id");
            execAjaxCall("/Fuentes/AgregarFuente", "POST", objetoFuente)
                .then((obj) => {
                    if (parcial) {
                        let mensaje = "La Fuente de Registro ha sido creada";
                        if (objetoFuente.id != null) {
                            mensaje = "La Fuente de Registro ha sido editada";
                        }
                        jsMensajes.Metodos.OkAlertModal(mensaje)
                            .set('onok', function (closeEvent) {
                                window.location.href = "/Fonatel/Fuentes/Index";
                            });
                    }
                    else {
                        //$(JsFuentes.Controles.txtidFuente).val(obj.objetoRespuesta[0].id);
                        if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Editar) {
                            JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta[0].DetalleFuentesRegistro;
                            JsFuentes.Metodos.HabilitarBotones();
                            JsFuentes.Metodos.CargarTablaDestinatarios();
                        } else {
                            InsertarParametroUrl("id", obj.objetoRespuesta[0].id)
                            JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta[0].DetalleFuentesRegistro;
                            JsFuentes.Metodos.HabilitarBotones();
                            JsFuentes.Metodos.CargarTablaDestinatarios();
                        }
                    }
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        $(JsFuentes.Controles.step1).trigger('click');
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
    
                    $("#loading").fadeOut();
                });
        
        },
        "EliminarDestinatario": function (id) {
         let destinatario = new Object();
            destinatario.idDetalleFuente = id;
            destinatario.fuenteId = ObtenerValorParametroUrl("id");
         execAjaxCall("/Fuentes/EliminarDestinatario", "POST", destinatario)
             .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Destinatario ha sido eliminado")
                        .set('onok', function (closeEvent) {
                            JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta[0].DetalleFuentesRegistro;
                            $(JsFuentes.Controles.txtNombre).val("");
                            $(JsFuentes.Controles.txtCorreo).val("");
                            $(JsFuentes.Controles.txtIdUsuarioDetalle).val("");
                            JsFuentes.Metodos.HabilitarBotones();
                            JsFuentes.Metodos.CargarTablaDestinatarios();
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
        "ActivarFuente": function () {
            $("#loading").fadeIn();
            let fuente = new Object()
            fuente.id = ObtenerValorParametroUrl("id");
            execAjaxCall("/Fuentes/ActivarFuente", "POST", fuente)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Fuente ha sido creada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
           },
        "ConsultarDestinatarios": function (id) {
            $("#loading").fadeIn();
            let destinatario = new Object()
            destinatario.idDetalleFuente = id;
            execAjaxCall("/Fuentes/ConsultarDestinatarios", "POST", destinatario)
                .then((obj) => {
                    JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta;

                    if (JsFuentes.Variables.ListaDestinatarios.length > 0) {
                        let destinatario = JsFuentes.Variables.ListaDestinatarios[0];
                        $(JsFuentes.Controles.txtidDetalleFuente).val(destinatario.idDetalleFuente);
                        $(JsFuentes.Controles.txtNombre).val(destinatario.NombreDestinatario);
                        $(JsFuentes.Controles.txtCorreo).val(destinatario.CorreoElectronico);
                        $(JsFuentes.Controles.btnGuardarDestinatario).prop("disabled", false);
                        $(JsFuentes.Controles.txtIdUsuarioDetalle).val(destinatario.idUsuario);
                    }
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
        "ConsultarListaDestinatarios": function (id) {
            $("#loading").fadeIn();
            let destinatario = new Object();
            destinatario.FuenteId = ObtenerValorParametroUrl("id");
            execAjaxCall("/Fuentes/ConsultarDestinatarios", "POST", destinatario = destinatario)
                .then((obj) => {
                    JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta;
                    JsFuentes.Metodos.CargarTablaDestinatarios();
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
        "ValidarExistenciaFuente": function (idfuente) {
            $("#loading").fadeIn();
            let fuente = new Object()
            fuente.id = idfuente;
            execAjaxCall("/Fuentes/ValidarFuente", "POST", fuente)
                .then((obj) => {
                    if (obj.objetoRespuesta.length == 0) {
                        JsFuentes.Consultas.EliminarFuente(idfuente);
                    } else {
                        let dependencias = obj.objetoRespuesta[0] + "<br>"

                        jsMensajes.Metodos.ConfirmYesOrNoModal("La Fuente ya está en uso en las<br>" + dependencias + "<br>¿Desea eliminarla?", jsMensajes.Variables.actionType.eliminar)
                            .set('onok', function (closeEvent) {
                                JsFuentes.Consultas.EliminarFuente(idfuente);
                            })
                    }
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });

        }

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
    let validar = JsFuentes.Metodos.ValidarFuente();
    if (validar) {
        if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Editar) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Fuente de Registro?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsFuentes.Consultas.AgregarFuente(true);
                });
        } else {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Fuente de Registro?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsFuentes.Consultas.AgregarFuente(true);
                });
        }
    } else {
        let validarParcial = JsFuentes.Metodos.ValidarFuenteParcial();
        if (validarParcial) {
            if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Editar) {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Fuente de Registro?", jsMensajes.Variables.actionType.agregar)
                    .set('onok',async function (closeEvent) {
                        JsFuentes.Consultas.AgregarFuente(true);
                    })
                    .set('oncancel', function () {
                        JsFuentes.Metodos.ValidarFuente();
                    });

            } else {
                jsMensajes.Metodos.ConfirmYesOrNoModal("Existen Campos vacíos.¿Desea realizar un guardado parcial de la Fuente de Registro?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        JsFuentes.Consultas.AgregarFuente(true);
                    })
                    .set('oncancel', function () {
                        JsFuentes.Metodos.ValidarFuente();
                    });
            }
        }
    }

});

$(document).on("click", JsFuentes.Controles.btnSiguienteFuente, function (e) {
    e.preventDefault();
    let validar = JsFuentes.Metodos.ValidarFuente();
    if (validar) {
        $(JsFuentes.Controles.step2).trigger('click');
    }
});

$(document).on("click", JsFuentes.Controles.step2, function (e) {
    let validar = JsFuentes.Metodos.ValidarFuente();
    if (validar) {
        JsFuentes.Consultas.AgregarFuente(false);
    }
    else {
        return false;
    }
});

$(document).on("click", JsFuentes.Controles.btnGuardarFuentesCompleto, function (e) {
    e.preventDefault();
 
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea crear la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsFuentes.Consultas.ActivarFuente();
        });
});

$(document).on("click", JsFuentes.Controles.btnGuardarDestinatario, function (e) {
    e.preventDefault();
    let validar = JsFuentes.Metodos.ValidarFormularioDetalle();
    if (validar) {
        let mensaje = "¿Desea agregar el Destinatario?";
        let id = $(JsFuentes.Controles.txtidDetalleFuente).val();
        if (id!="") {
            mensaje = "¿Desea editar el Destinatario?";
        }

        jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje, jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsFuentes.Consultas.AgregarDestinatario(id);
            });
    }
});

$(document).on("click", JsFuentes.Controles.btnBorrarFuente, function () {

    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Fuente?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsFuentes.Consultas.ValidarExistenciaFuente(id);
        });
});

$(document).on("click", JsFuentes.Controles.btnEditarFuente, function () {

    let id = $(this).val();
    window.location.href = "/Fonatel/Fuentes/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

$(document).on("click", JsFuentes.Controles.btnBorrarDetalle, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Destinatario?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {        
            JsFuentes.Consultas.EliminarDestinatario(id);
        });
});

$(document).on("click", JsFuentes.Controles.btnEditarDetalle, function () {
    let id = $(this).val();
    JsFuentes.Consultas.ConsultarDestinatarios(id);
});

$(document).on("click", JsFuentes.Controles.btnAtrasFuentes, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});

$(document).on("keyup", JsFuentes.Controles.ControlesStep1, function (e) {
   if ($(JsFuentes.Controles.txtFuente).val().trim().length > 0 && $(JsFuentes.Controles.txtCantidad).val() > 0) {
       $(JsFuentes.Controles.btnSiguienteFuente).prop("disabled", false);
       $(JsFuentes.Controles.step2).prop("disabled", false);
    }
    else {
       $(JsFuentes.Controles.btnSiguienteFuente).prop("disabled", true);
       $(JsFuentes.Controles.step2).prop("disabled", true);
    }
});

$(function () {
    if ($(JsFuentes.Controles.TablaFuentes).length > 0) {
        JsFuentes.Consultas.ConsultaListaFuentes();
    }
    if ($(JsFuentes.Controles.ControlesStep1).length > 0) {
        if ($(JsFuentes.Controles.txtFuente).val().trim().length > 0 && $(JsFuentes.Controles.txtCantidad).val() > 0) {
            $(JsFuentes.Controles.btnSiguienteFuente).prop("disabled", false);
            $(JsFuentes.Controles.step2).prop("disabled", false);
        }
        else {
            $(JsFuentes.Controles.btnSiguienteFuente).prop("disabled", true);
        }
    }
})
