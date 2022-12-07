JsRelacion = {
    "Controles": {
        "btnCancelar": "#btnCancelar",
        "btnAgregarRelacion": "#TablaRelacionCategoria tbody tr td .btn-add",
        "btnEditarRelacion": "#TablaRelacionCategoria tbody tr td .btn-edit",
        "btnAtrasRelacionCategorias":"#btnAtrasRelacionCategorias",
        "TablaRelacionCategoria": "#TablaRelacionCategoria tbody",
        "btnGuardarCategoria":"#btnGuardarCategoria",
        "btnFinalizar": "#btnFinalizar",
        "btnDescargarDetalle": "#TablaRelacionCategoria tbody tr td .btn-download",
        "btnDeleteRelacion": "#TablaRelacionCategoria tbody tr td .btn-delete",
        "btnEliminarDetalleRelacion": "#TablaDetalleRelacionCategoria tbody tr td .btn-delete",
        "stepRelacionCategoria1": "a[href='#step-1']",
        "stepRelacionCategoria2": "a[href='#step-2']",
        "txtCantidadFilas": "#txtCantidadFilas",
        "txtNombreRelacion": "#txtNombreRelacion",
        "txtCodigoRelacion": "#txtCodigoRelacion",
        "TxtCantidadCategoria": "#TxtCantidadCategoria",
        "ddlCategoriaId":"#ddlCategoriaId",
        "btnGuardarRelacion": "#btnGuardarRelacion",
        "btnSiguienteRelacionCategoria": "#btnSiguienteRelacionCategoria",
        "ddlCategoriaAtributo":"#ddlCategoriaAtributo",
        "nombreHelp": "#nombreHelp",
        "CodigoHelp": "#CodigoHelp",
        "CantidadHelp": "#CantidadHelp",
        "CantidadRegistrosHelp": "#CantidadRegistrosHelp",
        "TipoCategoriaHelp": "#TipoCategoriaHelp",
        "DetalleDesagregacionIDHelp": "#DetalleDesagregacionIDHelp",
        "CategoriaDetalleHelp": "#CategoriaDetalleHelp",
        "DetalleDesagregacionAtributoHelp": "#DetalleDesagregacionAtributoHelp",
        "TableCategoriaAtributo": "#TableCategoriaAtributo tbody",
        "TableCategoriaAtributoEliminar": "#TableCategoriaAtributo tbody tr td .btn-delete",
        "btnAgregarRelacion": "#TablaRelacionCategoria tbody tr td .btn-add",
        "btnCargarDetalle": "#TablaRelacionCategoria tbody tr td .btn-upload",
        "inputFileCargarDetalle": "#inputFileCargarDetalle",
        "btnGuardarDetalle": "#btnGuardarDetalle"

    },

    "Variables": {
        "ListadoRelaciones": [],
        "ListadoDetalleCategoria":[]
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
                if (detalle.DetalleRelacionCategoria.length == 0) {
                    html = html + "<td><button type ='button' data-toggle='tooltip' data-placement='top' disabled  data-original-title='Cargar Detalle'  title='Cargar Detalle' class='btn-icon-base btn-upload' ></button >" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' disabled data-original-title='Descargar Plantilla' title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' disabled data-original-title='Agregar Detalle' title='Agregar atributos' class='btn-icon-base btn-add'></button></td>";
                } else {
                    html = html + "<td><button type ='button' data-toggle='tooltip' data-placement='top' value=" + detalle.id + "  data-original-title='Cargar Detalle'  title='Cargar Detalle' class='btn-icon-base btn-upload' ></button >" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' value=" + detalle.id + " data-original-title='Descargar Plantilla' title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' value=" + detalle.id + " data-original-title='Agregar Detalle' title='Agregar atributos' class='btn-icon-base btn-add'></button></td>";
                }
                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value='" + detalle.id + "' title='Editar' class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value='" + detalle.id + "' title='Eliminar' class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>";
            }
            $(JsRelacion.Controles.TablaRelacionCategoria).html(html);
            CargarDatasource();
            JsRelacion.Variables.ListadoRelaciones = [];
        },

        "CargarTablaDetalleRelacion": function (relacion) {
            $(JsRelacion.Controles.btnGuardarCategoria).prop("disabled", relacion.DetalleRelacionCategoria.length >= relacion.CantidadCategoria);
            $(JsRelacion.Controles.btnFinalizar).prop("disabled", relacion.DetalleRelacionCategoria.length != relacion.CantidadCategoria);
            JsRelacion.Variables.ListadoDetalleCategoria = relacion.DetalleRelacionCategoria;
            EliminarDatasource();
            let html = "";

            for (var i = 0; i < JsRelacion.Variables.ListadoDetalleCategoria.length; i++) {

                let detalle = JsRelacion.Variables.ListadoDetalleCategoria[i];

                html = html + "<tr>"

                html = html + "<td scope='row'>" + detalle.CategoriaAtributo.NombreCategoria  + "</td>";

                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' data-index=" + i + " title='Eliminar' value=" + detalle.id + " class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>"
            }
            $(JsRelacion.Controles.TableCategoriaAtributo).html(html);
            CargarDatasource();

        },

        "HabilitarBotonSiguiente": function () {
            let validar = true;
            if ($(JsRelacion.Controles.txtCodigoRelacion).val().length == 0) {
                validar = false;
            }
            if ($(JsRelacion.Controles.txtNombreRelacion).val().length == 0) {
                validar = false;
            }

            if ($(JsRelacion.Controles.TxtCantidadCategoria).val().length == 0 || $(JsRelacion.Controles.TxtCantidadCategoria).val() == 0) {
                validar = false;
            }

            if ($(JsRelacion.Controles.ddlCategoriaId).val().length == 0) {
                validar = false;
            }
            if ($(JsRelacion.Controles.txtCantidadFilas).val().length == 0 || $(JsRelacion.Controles.txtCantidadFilas).val()==0) {
                validar = false;
            }
            return validar;
        },

        "ValidarFormularioRelacion": function () {
            let validar = true;
            $(JsRelacion.Controles.nombreHelp).addClass("hidden");
            $(JsRelacion.Controles.CodigoHelp).addClass("hidden");
            $(JsRelacion.Controles.CantidadHelp).addClass("hidden");
            $(JsRelacion.Controles.TipoCategoriaHelp).addClass("hidden");
            $(JsRelacion.Controles.CantidadRegistrosHelp).addClass("hidden");

            if ($(JsRelacion.Controles.txtCodigoRelacion).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.CodigoHelp).removeClass("hidden");
            }
            if ($(JsRelacion.Controles.txtNombreRelacion).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.nombreHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.TxtCantidadCategoria).val().length == 0 || $(JsRelacion.Controles.TxtCantidadCategoria).val() == 0) {
                validar = false;
                $(JsRelacion.Controles.CantidadHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.ddlCategoriaId).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.TipoCategoriaHelp).removeClass("hidden");
            }
            if ($(JsRelacion.Controles.txtCantidadFilas).val().length == 0 || $(JsRelacion.Controles.txtCantidadFilas).val()==0) {
                validar = false;
                $(JsRelacion.Controles.CantidadRegistrosHelp).removeClass("hidden");
            }

            return validar;
        },

        "MetodoDescarga": function (idRelacion) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar la Plantilla?", jsMensajes.Variables.actionType.descargar)
                .set('onok', function (closeEvent) {
                    window.open(jsUtilidades.Variables.urlOrigen + "/RelacionCategoria/DescargarExcel?id=" + idRelacion);
                    jsMensajes.Metodos.OkAlertModal("La Plantilla ha sido descargada");
                });
        },





      

       
       

        "ValidarFormularioDetalle": function () {

            let validar = true;

            $(JsRelacion.Controles.CategoriaDetalleHelp).addClass("hidden");
            $(JsRelacion.Controles.DetalleDesagregacionAtributoHelp).addClass("hidden");

            if ($(JsRelacion.Controles.ddlCategoriaDetalle).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.CategoriaDetalleHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.DetalleDesagregacionAtributoHelp).removeClass("hidden");
            }


            return validar;
        },
    },

    "Consultas": {

        "ConsultaListaRelaciones": function () {
            $("#loading").fadeIn();
            execAjaxCall("/RelacionCategoria/ObtenerListaRelacionCategoria", "GET")
            .then((obj) => {
               JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;
               JsRelacion.Metodos.CargarTablaRelacion();                              
             }).catch((obj) => {
                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { location.reload(); })           
             })
             .finally(() => {
                $("#loading").fadeOut();
             });
         },

        "InsertarRelacion": function (Siguiente = false) {

            $("#loading").fadeIn();

            let RelacionCategoria = new Object();
            RelacionCategoria.Codigo = $(JsRelacion.Controles.txtCodigoRelacion).val().trim();
            RelacionCategoria.Nombre = $(JsRelacion.Controles.txtNombreRelacion).val().trim();
            RelacionCategoria.CantidadCategoria = $(JsRelacion.Controles.TxtCantidadCategoria).val();
            RelacionCategoria.idCategoria = $(JsRelacion.Controles.ddlCategoriaId).val();
            RelacionCategoria.CantidadFilas = $(JsRelacion.Controles.txtCantidadFilas).val();
            execAjaxCall("/RelacionCategoria/InsertarRelacionCategoria", "POST", RelacionCategoria)
                .then((obj) => {
                    let relacion = obj.objetoRespuesta[0];
                    JsRelacion.Metodos.CargarTablaDetalleRelacion(relacion);
                            if (Siguiente) {
                                InsertarParametroUrl("id", relacion.id);
                                $(JsRelacion.Controles.stepRelacionCategoria2).click();
                            } else {
                                window.location.href = "/Fonatel/RelacionCategoria/Index";
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

        "EditarRelacion": function (Siguiente=false) {

            $("#loading").fadeIn();

            let RelacionCategoria = new Object();
            RelacionCategoria.id = ObtenerValorParametroUrl("id");
            RelacionCategoria.Codigo = $(JsRelacion.Controles.txtCodigoRelacion).val().trim();
            RelacionCategoria.Nombre = $(JsRelacion.Controles.txtNombreRelacion).val().trim();
            RelacionCategoria.CantidadCategoria = $(JsRelacion.Controles.TxtCantidadCategoria).val();
            RelacionCategoria.idCategoria = $(JsRelacion.Controles.ddlCategoriaId).val();
            RelacionCategoria.CantidadFilas = $(JsRelacion.Controles.txtCantidadFilas).val();
            execAjaxCall("/RelacionCategoria/EditarRelacionCategoria", "POST", RelacionCategoria)
                .then((obj) => {
                    let relacion = obj.objetoRespuesta[0];




                    JsRelacion.Metodos.CargarTablaDetalleRelacion(relacion);
   
                            if (Siguiente) {
                                $(JsRelacion.Controles.stepRelacionCategoria2).click();
                            } else {
                                jsMensajes.Metodos.OkAlertModal("La Relación ha sido editada")
                                    .set('onok', function (closeEvent) {
                                        window.location.href = "/Fonatel/RelacionCategoria/Index";
                                    });
                            }
                     
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {

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

        "EliminarRelacionCategoria": function (idRelacionCategoria) {

            $("#loading").fadeIn();


            let relacionCategoria = new Object();
            relacionCategoria.id = idRelacionCategoria


            execAjaxCall("/RelacionCategoria/EliminarRelacionCategoria", "POST", relacionCategoria)
                .then((obj) => {
                    let relacion = obj.objetoRespuesta[0];
                    JsRelacion.Metodos.CargarTablaDetalleRelacion(relacion);
                    jsMensajes.Metodos.OkAlertModal("La Relación ha sido eliminada")
                        .set('onok', function (closeEvent) {

                            window.location.href = "/Fonatel/RelacionCategoria/Index";
                        });

                }).catch((obj) => {

                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarDetalleCategoria": function () {
            $("#loading").fadeIn();
            let DetalleRelacionCategoria = new Object();
            DetalleRelacionCategoria.relacionid = ObtenerValorParametroUrl("id");
            DetalleRelacionCategoria.idCategoriaAtributo = $(JsRelacion.Controles.ddlCategoriaAtributo).val();

            execAjaxCall("/RelacionCategoria/InsertarDetalleRelacion", "POST", DetalleRelacionCategoria)
                .then((obj) => {
                    let relacion = obj.objetoRespuesta[0];
                    JsRelacion.Metodos.CargarTablaDetalleRelacion(relacion);

                    jsMensajes.Metodos.OkAlertModal("El Detalle ha sido agregado")
                        .set('onok', function (closeEvent) {
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {

                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $(JsRelacion.Controles.ddlCategoriaAtributo).val(0);
                    $(JsRelacion.Controles.ddlCategoriaAtributo).trigger("change");
                    $("#loading").fadeOut();
                });



        },

        "EliminarRelacionCategoriaDetalle": function (idRelacionCategoriaDetalle) {

            $("#loading").fadeIn();
            let DetalleRelacionCategoria = new Object();
            DetalleRelacionCategoria.relacionid = ObtenerValorParametroUrl("id");
            DetalleRelacionCategoria.id = idRelacionCategoriaDetalle


            execAjaxCall("/RelacionCategoria/EliminarDetalleRelacion", "POST", DetalleRelacionCategoria)
                .then((obj) => {
                    let relacion = obj.objetoRespuesta[0];
                    JsRelacion.Metodos.CargarTablaDetalleRelacion(relacion);
                    jsMensajes.Metodos.OkAlertModal("La Categoría ha sido eliminada")
                        .set('onok', function (closeEvent) {

                            JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;
                        });

                }).catch((obj) => {

                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },


    

        "ImportarExcel": function () {
            var data;
            data = new FormData();
            data.append('file', $(JsRelacion.Controles.inputFileCargarDetalle)[0].files[0]);
            execAjaxCallFile("/RelacionCategoria/CargarExcel", data)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Detalle ha sido agregado")
                        .set('onok', function (closeEvent) { location.reload() });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ValidarExistenciaRelacion": function (idRelacionCategoria) {

            $("#loading").fadeIn();
            let relacion = new Object()
            relacion.id = idRelacionCategoria;
            execAjaxCall("/RelacionCategoria/ValidarRelacion", "POST", relacion)
                .then((obj) => {
                    if (obj.objetoRespuesta.length == 0) {
                        JsRelacion.Consultas.EliminarRelacionCategoria(idRelacionCategoria);
                    }
                    else {

                        let dependencias = '';

                        for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                            dependencias = obj.objetoRespuesta[i] + "<br>"
                        }

                        jsMensajes.Metodos.ConfirmYesOrNoModal("La Relación está en uso en el/los<br>" + dependencias + "<br>¿Desea eliminarla?", jsMensajes.Variables.actionType.estado)
                            .set('onok', function (closeEvent) {

                                JsRelacion.Consultas.EliminarRelacionCategoria(idRelacionCategoria);

                            });
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

        "EliminarDetalleRelacionId": function (idRelacionid) {

            $("#loading").fadeIn();


            let relacionCategoriaId = new Object();
            relacionCategoriaId.RelacionId= ObtenerValorParametroUrl("idRelacionCategoria");
            relacionCategoriaId.idCategoriaId = idRelacionid;

            execAjaxCall("/RelacionCategoria/EliminarRegistroRelacionId", "POST", relacionCategoriaId)
                .then((obj) => {
                    let relacion = obj.objetoRespuesta[0];
                    JsRelacion.Metodos.CargarTablaDetalleRelacion(relacion);
                    jsMensajes.Metodos.OkAlertModal("El Detalle ha sido eliminado")
                        .set('onok', function (closeEvent) {

                            location.reload();
                        });

                }).catch((obj) => {

                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
        "InsertarDetalleRelacionId": function () {
            $("#loading").fadeIn();

            let relacionCategoriaId = new Object();
            relacionCategoriaId.RelacionId = ObtenerValorParametroUrl("idRelacionCategoria");
            relacionCategoriaId.listaCategoriaAtributo = [];
            let categoriaId = true;

            let validacion = true;

            $(".listasDesplegables").each(function () {
                if (categoriaId) {
                    relacionCategoriaId.idCategoriaId = $(this).val();
                    categoriaId = false;

                    if (relacionCategoriaId.idCategoriaId.length == 0) {
                        $("#Categoriaid").removeClass("hidden");
                        validacion = false;
                    }
                    else {
                        $("#Categoriaid").addClass("hidden");
                    }

                } else {
                    let categoriaAtributo = new Object();
                    categoriaAtributo.IdCategoriaId = relacionCategoriaId.idCategoriaId;
                    categoriaAtributo.IdcategoriaAtributo = $(this).attr('id').replace("dd_", "");
                    categoriaAtributo.IdcategoriaAtributoDetalle = $(this).val();
                    if (categoriaAtributo.IdcategoriaAtributoDetalle.length == 0) {
                        $("#help_" + categoriaAtributo.IdcategoriaAtributo).removeClass("hidden");
                        validacion = false;
                    } else {
                        $("#help_" + categoriaAtributo.IdcategoriaAtributo).addClass("hidden");
                        relacionCategoriaId.listaCategoriaAtributo.push(categoriaAtributo);
                    }


                    
                }



            });

            if (validacion) {

                execAjaxCall("/RelacionCategoria/InsertarRelacionCategoriaId", "POST", relacionCategoriaId)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("El Detalle ha sido agregado")
                            .set('onok', function (closeEvent) {
                                location.reload();
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
            }
            $("#loading").fadeOut();
        },
    }
}

$(document).on("click", JsRelacion.Controles.btnEditarRelacion, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/RelacionCategoria/Create?id=" + id;
});

$(document).on("change", JsRelacion.Controles.ddlCategoriaId, function () {
    let validacion = !JsRelacion.Metodos.HabilitarBotonSiguiente();
    $(JsRelacion.Controles.btnSiguienteRelacionCategoria).prop("disabled", validacion);
});
$(document).on("keyup", "input", function () {
    let validacion = !JsRelacion.Metodos.HabilitarBotonSiguiente();
    $(JsRelacion.Controles.btnSiguienteRelacionCategoria).prop("disabled",  validacion);
});

$(document).on("click", JsRelacion.Controles.btnGuardarDetalle, function (e) {
    e.preventDefault();

    


    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Detalle?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.InsertarDetalleRelacionId();
        });
});



$(document).on("click", JsRelacion.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RelacionCategoria/Index";
        });
});
$(document).on("click", JsRelacion.Controles.btnSiguienteRelacionCategoria, function (e) {
    let id = ObtenerValorParametroUrl("id");
    if (id == null) {
          JsRelacion.Consultas.InsertarRelacion(true);     
    }
    else {
        JsRelacion.Consultas.EditarRelacion(true);
    }
});

$(document).on("click", JsRelacion.Controles.btnGuardarRelacion, function (e) {

    e.preventDefault();

    let id = ObtenerValorParametroUrl("id");
    let validar = true;

    if ($(JsRelacion.Controles.txtCodigoRelacion).val().length == 0) {
        $(JsRelacion.Controles.CodigoHelp).removeClass("hidden");
        validar = false;
    }
    if ($(JsRelacion.Controles.txtNombreRelacion).val().length == 0) {
        $(JsRelacion.Controles.nombreHelp).removeClass("hidden");
      
        validar = false;
    }
    if (validar) {
        $(JsRelacion.Controles.nombreHelp).addClass("hidden");
        $(JsRelacion.Controles.CodigoHelp).addClass("hidden");



        if (id == null) {
            if (JsRelacion.Metodos.HabilitarBotonSiguiente()) {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea guardar la Relación?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        JsRelacion.Consultas.InsertarRelacion();
                    });
            }
            else {
                jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial de la Relación?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        JsRelacion.Consultas.InsertarRelacion();
                    })
                    .set('oncancel', function (closeEvent) {
                        JsRelacion.Metodos.ValidarFormularioRelacion();
                    });
            }
        }
        else {
            if (JsRelacion.Metodos.HabilitarBotonSiguiente()) {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Relación?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        JsRelacion.Consultas.EditarRelacion();
                    });
            }
            else {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar un guardado parcial de la Relación?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        JsRelacion.Consultas.EditarRelacion();
                    })
                    .set('oncancel', function (closeEvent) {
                        JsRelacion.Metodos.ValidarFormularioRelacion();
                    });
            }
        }




    }


  

});


$(document).on("click", JsRelacion.Controles.btnEliminarDetalleRelacion, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Detalle?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.EliminarDetalleRelacionId(id);
        });
});


$(document).on("click", JsRelacion.Controles.btnCargarDetalle, function (e) {

    $(JsRelacion.Controles.inputFileCargarDetalle).click();
});

$(document).on("change", JsRelacion.Controles.inputFileCargarDetalle, function (e) {
    JsRelacion.Consultas.ImportarExcel();
});

$(document).on("click", JsRelacion.Controles.btnAtrasRelacionCategorias, function (e) {

    e.preventDefault();
    $(JsRelacion.Controles.stepRelacionCategoria1).click();

});

$(document).on("click", JsRelacion.Controles.btnFinalizar, function (e) {

    e.preventDefault();
    window.location.href = "/Fonatel/RelacionCategoria/index";

});


$(document).on("click", JsRelacion.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.InsertarDetalleCategoria();
        });
    
});

$(document).on("click", JsRelacion.Controles.TableCategoriaAtributoEliminar, function (e) {
    e.preventDefault();
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Categoría?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.EliminarRelacionCategoriaDetalle(id);
     });
  
});

$(document).on("click", JsRelacion.Controles.btnDeleteRelacion, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Relación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.ValidarExistenciaRelacion(id);
        });
});


$(document).on("click", JsRelacion.Controles.btnDescargarDetalle, function (e) {
    let id = $(this).val();
    JsRelacion.Metodos.MetodoDescarga(id);
});

$(document).on("click", JsRelacion.Controles.btnAgregarRelacion, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/RelacionCategoria/Detalle?idRelacionCategoria=" + id;
});




$(function () {

    if ($(JsRelacion.Controles.TablaRelacionCategoria).length > 0) {
        JsRelacion.Consultas.ConsultaListaRelaciones();
    }
    else if ($(JsRelacion.Controles.txtCodigoRelacion).length>0) {
      let validacion = !JsRelacion.Metodos.HabilitarBotonSiguiente();
        $(JsRelacion.Controles.btnSiguienteRelacionCategoria).prop("disabled", validacion);
        $(JsRelacion.Controles.txtCodigoRelacion).prop("disabled", ObtenerValorParametroUrl("id") != null)
        if ($(JsRelacion.Controles.ddlCategoriaId).val()!=0) {
            $(JsRelacion.Controles.ddlCategoriaId).prop("disabled", ObtenerValorParametroUrl("id") != null)
        } 
       
    }
   

})