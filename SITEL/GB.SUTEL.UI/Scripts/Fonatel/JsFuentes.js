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
        "txtidFuente": "#txtidFuente",
        "txtidDetalleFuente":"#txtidDetalleFuente",
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
        "ListaDestinatarios":[]
    },

    "Metodos": {
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
        "ValidarFormularioDetalle": function () {
            $(JsFuentes.Controles.nombreHelp).addClass("hidden");
            $(JsFuentes.Controles.CorreoHelp).addClass("hidden");
            let validar = true;
            if ($(JsFuentes.Controles.txtNombre).val().trim().length == 0) {
                $(JsFuentes.Controles.nombreHelp).removeClass("hidden");
                validar = false;
            }
            if ($(JsFuentes.Controles.txtCorreo).val().trim().length == 0) {
                $(JsFuentes.Controles.CorreoHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        }
    },

    "Consultas": {
        "ConsultaListaFuentes": function () {
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/ObtenerListaFuentes',
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsFuentes.Variables.ListadoFuentes = obj.objetoRespuesta;
                        JsFuentes.Metodos.CargarTablaFuentes();
                    } else  {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                  
                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },
        "EliminarFuente": function (id) {
            let fuente = new Object();

            fuente.id = id;

            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/EliminarFuente',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { fuente },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {

                        jsMensajes.Metodos.OkAlertModal("La Fuente ha sido eliminada ")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });


                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }

                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },
        "AgregarDestinatario": function () {
            let destinatario = new Object()
            destinatario.NombreDestinatario = $(JsFuentes.Controles.txtNombre).val();
            destinatario.CorreoElectronico = $(JsFuentes.Controles.txtCorreo).val();
            destinatario.fuenteId = $(JsFuentes.Controles.txtidFuente).val();
            destinatario.idDetalleFuente = $(JsFuentes.Controles.txtidDetalleFuente).val();
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/AgregarDestinatario',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { destinatario },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {

                        let mensaje = "El destinatario ha sido creado";
                        if (destinatario.idDetalleFuente > 0) {
                            mensaje = "El destinatario ha sido editado"
                        }


                        jsMensajes.Metodos.OkAlertModal(mensaje) 
                           .set('onok', function (closeEvent) {
                               JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta;
                               JsFuentes.Metodos.CargarTablaDestinatarios();
                               $(JsFuentes.Controles.txtNombre).val("");
                               $(JsFuentes.Controles.txtCorreo).val("");
                               $(JsFuentes.Controles.txtidDetalleFuente).val("");
                           });
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {  });
                    }

                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })



        },
        "AgregarFuente": function (parcial) {
            let fuente = new Object()
             fuente.Fuente = $(JsFuentes.Controles.txtFuente).val();
            fuente.CantidadDestinatario = $(JsFuentes.Controles.txtCantidad).val();
            fuente.id = $(JsFuentes.Controles.txtidFuente).val();

            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/AgregarFuente',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { fuente },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {

                        if (parcial) {
                            jsMensajes.Metodos.OkAlertModal("La Fuente ha sido creada")
                                .set('onok', function (closeEvent) {
                                    window.location.href = "/Fonatel/Fuentes/Index";
                                });
                        }
                        else {
                            $(JsFuentes.Controles.txtidFuente).val(obj.objetoRespuesta[0].id);
                        }             
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {  });
                    }

                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })



        },
        "EliminarDestinatario": function (id) {
            let destinatario = new Object();

             destinatario.idDetalleFuente = id;
             destinatario.fuenteId = $(JsFuentes.Controles.txtidFuente).val();
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/EliminarDestinatario',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { destinatario },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        jsMensajes.Metodos.OkAlertModal("El Destinatario ha sido eliminado")
                            .set('onok', function (closeEvent) {
                                JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta;
                                JsFuentes.Metodos.CargarTablaDestinatarios();
                                $(JsFuentes.Controles.txtNombre).val("");
                                $(JsFuentes.Controles.txtCorreo).val("");
                            });
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { location.reload(); });
                    }

                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },
        "ActivarFuente": function () {
            let fuente = new Object()
            fuente.id = $(JsFuentes.Controles.txtidFuente).val();

            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/ActivarFuente',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { fuente },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        jsMensajes.Metodos.OkAlertModal("La Fuente ha sido creada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
                      
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {  });
                    }

                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },
        "ConsultarDestinatarios": function (id) {
            let destinatario = new Object()
            destinatario.idDetalleFuente = id;
            
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/ConsultarDestinatarios',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { destinatario },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsFuentes.Variables.ListaDestinatarios = obj.objetoRespuesta;
                        if (JsFuentes.Variables.ListaDestinatarios.length>0) {
                            let destinatario = JsFuentes.Variables.ListaDestinatarios[0];
                            $(JsFuentes.Controles.txtidDetalleFuente).val(destinatario.idDetalleFuente);
                            $(JsFuentes.Controles.txtNombre).val(destinatario.NombreDestinatario);
                            $(JsFuentes.Controles.txtCorreo).val(destinatario.CorreoElectronico)
                        }
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }

                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })



        },

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
    JsFuentes.Metodos.ValidarFuente();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsFuentes.Consultas.AgregarFuente(true);
        });
});


$(document).on("click", JsFuentes.Controles.btnSiguienteFuente, function (e) {
    e.preventDefault();
    $(JsFuentes.Controles.step2).trigger('click');
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


$(document).on("click", ".stepwizard-step[data-step='1']", function (e) {
    e.stopPropagation();
});






$(document).on("click", JsFuentes.Controles.btnGuardarFuentesCompleto, function (e) {
    e.preventDefault();
 
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsFuentes.Consultas.ActivarFuente();
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarDestinatario, function (e) {
    e.preventDefault();
    let validar = JsFuentes.Metodos.ValidarFormularioDetalle();



    if (validar) {
        let mensaje = "¿Desea agregar el destinatario a la Fuente?";
        if ($(JsFuentes.Controles.txtidDetalleFuente).val() > 0) {
            mensaje = "¿Desea editar el destinatario?";
        }

        jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje, jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsFuentes.Consultas.AgregarDestinatario();
            });
    }
});

$(document).on("click", JsFuentes.Controles.btnBorrarFuente, function () {

    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Fuente?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsFuentes.Consultas.EliminarFuente(id);
        });
});


$(document).on("click", JsFuentes.Controles.btnEditarFuente, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/Fuentes/Create?id=" + id;
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
