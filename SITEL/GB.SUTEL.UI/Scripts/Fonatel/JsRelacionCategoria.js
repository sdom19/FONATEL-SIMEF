JsRelacion = {
    "Controles": {
        "FormularioCrearRelacion": "#FormularioCrearRelacion",

        "btnEliminar": ".btnEliminarRelacion",
        "btnGuardar": "#btnGuardarRelacion",
        "btnCancelar": "#btnCancelarRelacion",
        "btnCancelarDetalle": "#btnCancelarDetalle",
        "btnAgregarRelacion": "#TablaRelacionCategoria tbody tr td .btn-add",
        "btnEditarRelacion": "#TablaRelacionCategoria tbody tr td .btn-edit",
        "btnDeleteRelacion": "#TablaRelacionCategoria tbody tr td .btn-delete",
        "btnEliminarDetalleRelacion": "#TablaDetalleRelacionCategoria tbody tr td .btn-delete",
        "btnGuardarDetalle": "#btnGuardarDetalle",
        "btnAgregarDetalle": "#btnAgregarDetalle",
        "inputFileAgregarDetalle": "#inputFileAgregarDetalle",
        "TablaRelacionCategoria": "#TablaRelacionCategoria tbody",

        "txtCodigo": "#txtCodigo",
        "txtNombre": "#txtNombre",
        "TxtCantidad": "#TxtCantidad",
        "ddlCategoriaId": "#ddlCategoriaId",
        "ddlDetalleDesagregacionId": "#ddlDetalleDesagregacionId",

        "nombreHelp": "#nombreHelp",
        "CodigoHelp": "#CodigoHelp",
        "CantidadHelp": "#CantidadHelp",
        "TipoCategoriaHelp": "#TipoCategoriaHelp",
        "DetalleDesagregacionIDHelp": "#DetalleDesagregacionIDHelp",

        "txtmodoRelacion": "#txtmodoRelacion",
        "id": "#txtidRelacion"

    },
    "Variables": {
        "ListadoRelaciones": []
    },

    "Metodos": {

        "CargarTablaRelacion": function () {
            EliminarDatasource();
            let html = "";

            for (var i = 0; i < JsRelacion.Variables.ListadoRelaciones.length; i++) {
                let detalle = JsRelacion.Variables.ListadoRelaciones[i];
                html = html + "<tr>"

                html = html + "<td scope='row'>" + detalle.Codigo + "</td>";
                html = html + "<td>" + detalle.Nombre + "</td>";
                html = html + "<td>" + detalle.CantidadCategoria + "/" + detalle.DetalleRelacionCategoria.length + "</td>";
                html = html + "<td>" + detalle.EstadoRegistro.Nombre + "</td>";
                
                html = html + "<td><button id = 'btnAgregarDetalle' type = 'button' data - toggle='tooltip' data - placement='top' title = 'Agregar Detalle' class='btn-icon-base btn-upload' ></button >" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Agregar Detalle' class='btn-icon-base btn-add'></button></td>";


                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value='" + detalle.id + "' title='Editar' class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value='" + detalle.id + "' title='Eliminar' class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>";
            }
            $(JsRelacion.Controles.TablaRelacionCategoria).html(html);
            CargarDatasource();
            JsRelacion.Variables.ListadoRelaciones = [];
        },

        "ValidarFormularioRelacion": function () {

            let validar = true;

            $(JsRelacion.Controles.nombreHelp).addClass("hidden");
            $(JsRelacion.Controles.CodigoHelp).addClass("hidden");
            $(JsRelacion.Controles.CantidadHelp).addClass("hidden");
            $(JsRelacion.Controles.TipoCategoriaHelp).addClass("hidden");
            $(JsRelacion.Controles.DetalleDesagregacionIDHelp).addClass("hidden");

            if ($(JsRelacion.Controles.txtCodigo).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.CodigoHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.txtNombre).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.nombreHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.TxtCantidad).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.CantidadHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.ddlCategoriaId).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.TipoCategoriaHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.ddlDetalleDesagregacionId).val() == 0) {
                validar = false;
                $(JsRelacion.Controles.DetalleDesagregacionIDHelp).removeClass("hidden");
            }

            return validar;
        },
    },

    "Consultas": {

        "ConsultaListaRelaciones": function () {
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/ObtenerListaRelacionCategoria',
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;
                        JsRelacion.Metodos.CargarTablaRelacion();
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },

        "ConsultarDesagregacionId": function (selected) {
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/ObtenerDetalleDesagregacionId?select=' + selected,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {

                    let html = "";

                    for (var i = 0; i < obj.length; i++) {

                        html = html + "<option value='" + obj[i] + "'>" + obj[i] + "</option>"
                    }

                    $(JsRelacion.Controles.ddlDetalleDesagregacionId).html(html);


                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },

        "InsertarRelacion": function () {
            let relacion = new Object();
            relacion.Codigo = $(JsRelacion.Controles.txtCodigo).val().trim();
            relacion.Nombre = $(JsRelacion.Controles.txtNombre).val().trim();
            relacion.CantidadCategoria = $(JsRelacion.Controles.TxtCantidad).val();
            relacion.idCategoria = $(JsRelacion.Controles.ddlCategoriaId).val();
            relacion.idCategoriaValor = $(JsRelacion.Controles.ddlDetalleDesagregacionId).val();
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/InsertarRelacionCategoria',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { relacion },
                success: function (obj) {
                    $("#loading").fadeOut();
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        jsMensajes.Metodos.OkAlertModal("La Relación entre Categoría ha sido creada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
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
                }
            }).fail(function (obj) {


                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();

            })
        },

        "EditarRelacion": function () {
            let relacion = new Object();
            relacion.id = $(JsRelacion.Controles.id).val();
            relacion.Codigo = $(JsRelacion.Controles.txtCodigo).val().trim();
            relacion.Nombre = $(JsRelacion.Controles.txtNombre).val().trim();
            relacion.CantidadCategoria = $(JsRelacion.Controles.TxtCantidad).val();
            relacion.idCategoria = $(JsRelacion.Controles.ddlCategoriaId).val();
            relacion.idCategoriaValor = $(JsRelacion.Controles.ddlDetalleDesagregacionId).val();

            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/EditarRelacionCategoria',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { relacion },
                success: function (obj) {
                    $("#loading").fadeOut();
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        jsMensajes.Metodos.OkAlertModal("La Relación entre Categoría ha sido editada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
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
                }
            }).fail(function (obj) {


                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();

            })

        },

    }

}

$(document).on("click", JsRelacion.Controles.btnAgregarRelacion, function () {
    let idRelacion = 1;
    window.location.href = "/Fonatel/RelacionCategoria/Detalle?id=" + idRelacion;
});

$(document).on("click", JsRelacion.Controles.btnEditarRelacion, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/RelacionCategoria/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

$(document).on("click", JsRelacion.Controles.btnGuardar, function (e) {

    e.preventDefault();
    let modo = $(JsRelacion.Controles.txtmodoRelacion).val();
    let validar = JsRelacion.Metodos.ValidarFormularioRelacion();
    if (!validar) {
        return;
    }

    if (modo == jsUtilidades.Variables.Acciones.Editar) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Relación entre Categoria?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsRelacion.Consultas.EditarRelacion();             
            });
    }
    else {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Relación entre Categoria?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsRelacion.Consultas.InsertarRelacion();
            });
    }

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

$(document).on("change", JsRelacion.Controles.ddlCategoriaId, function () {

    let seleted = $(this).val();
    if (seleted != 0) {
        JsRelacion.Consultas.ConsultarDesagregacionId(seleted);
    }

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
            jsMensajes.Metodos.OkAlertModal("La relación ha sido eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});

$(document).on("click", JsRelacion.Controles.btnAgregarDetalle, function (e) {
    $(JsRelacion.Controles.inputFileAgregarDetalle).click();
});

$(function () {

    if ($(JsRelacion.Controles.FormularioCrearRelacion).length > 0) {

        let modo = $(JsRelacion.Controles.txtmodoRelacion).val();

        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            //DESACTIVE EL CAMPO CODIGO
            $(JsRelacion.Controles.txtCodigo).prop("disabled", true);
        }
    }

    if ($(JsRelacion.Controles.TablaRelacionCategoria).length > 0) {
        JsRelacion.Consultas.ConsultaListaRelaciones();
    }

})