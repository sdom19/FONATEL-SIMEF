    JsCategoria= {
        "Controles": {
            "FormularioCategorias": "#FormularioCategorias",
            "FormularioIndex":"#FormularioIndex",
            "FormularioDetalle": "#FormularioDetalle",
            "txtCodigo":"#txtCodigoCategoria",
            "EtiquetaDetalleHelp":"#txtEtiquetaDetalleHelp",
            "CodigoDetalleHelp": "#txtCodigoDetalleHelp",
            "ModalCargaExcel":"#modalImportarExcel",
            "txtNombreCategoria": "#txtNombreCategoria",
            "txtCodigoCategoria":"#txtCodigoCategoria",
            "ddlTipoCategoria": "#ddlTipoCategoria",
            "txtCantidadDetalleCategoria": "#txtCantidadDetalleCategoria",
            "txtFechaMinimaCategoria": "#txtFechaMinimaCategoria",
            "txtFechaMaximaCategoria": "#txtFechaMaximaCategoria",
            "txtRangoMinimaCategoria": "#txtRangoMinimaCategoria",
            "txtRangoMaximaCategoria": "#txtRangoMaximaCategoria",
            "ddlTipoDetalle": "#ddlTipoDetalleCategoria",
            "txtCodigoCategoriaHelp": "#txtCodigoCategoriaHelp",
            "txtnombreCategoriaHelp": "#txtnombreCategoriaHelp",
            "ddlTipoCategoriaHelp": "#ddlTipoCategoriaHelp",
            "ddlTipoDetalleCategoriaHelp": "#ddlTipoDetalleCategoriaHelp",
            "CantidadDetalleCategoriaHelp": "#CantidadDetalleCategoriaHelp",
            "FechaMinimaCategoriaHelp": "#FechaMinimaCategoriaHelp",
            "FechaMaximaCategoriaHelp": "#FechaMaximaCategoriaHelp",
            "RangoMinimaCategoriaHelp": "#RangoMinimaCategoriaHelp",
            "RangoMaximaCategoriaHelp":"#RangoMaximaCategoriaHelp",
            "divFechaMinima": "#divFechaMinimaCategoria",
            "divFechaMaxima": "#divFechaMaximaCategoria",
            "divCantidadDetalle": "#divCantidadDetalleCategoria",
            "divRangoMinimoCategoria": "#divRangoMinimaCategoria",
            "divRangoMaximaCategoria": "#divRangoMaximaCategoria",
            "btnGuardarCategoria": "#btnGuardarCategoria",
            "btnCancelar": "#btnCancelarCategoria",
            "btnCancelarDetalle": "#btnCancelarDetalleCategoria",
            "btnEditarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-edit",
            "btnDesactivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-off",
            "btnActivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-on",
            "btnClonarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-clone",
            "btnAddCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-add",
            "btnGuardarDetalleCategoria": "#btnGuardarDetalleCategoria",
            "tablacategoria": "#TableCategoriaDesagregacion tbody",
            "TablaCategoriaDetalle": "#TableCategoriaDesagregacionDetalle tbody",
            "btnEliminarDetalle": "#TableCategoriaDesagregacionDetalle tbody .btn-delete",
            "btnEditarDetalle": "#TableCategoriaDesagregacionDetalle tbody .btn-edit",
            "btnCargarDetalle": "#TableCategoriaDesagregacion tbody tr td .btn-upload",
            "btnFinalizarDetalle":"#btnFinalizarDetalleCategoria",
            "btnDescargarDetalle": "#TableCategoriaDesagregacion tbody tr td .btn-download",
            "btnEliminarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-delete",
            "inputFileCargarDetalle": "#inputFileCargarDetalle",
            "txtCodigoDetalle": "#txtCodigoDetalle",
            "txtEtiquetaDetalle": "#txtEtiquetaDetalle"
        },
        "Variables": {
            "TipoFecha": 4,
            "TipoNumerico": 1,
            "OpcionSalir": true,
            "ListadoCategoria": [],
            "ListadoCategoriaDetalle": [],
            "ModoEditarAtributo": false
        },
        "Metodos": {
            "CargarTablaCategoria": function () {
                EliminarDatasource();
                let html = "";
                for (var i = 0; i < JsCategoria.Variables.ListadoCategoria.length; i++) {
                    let categoria = JsCategoria.Variables.ListadoCategoria[i];

                    html = html + "<tr>"

                    html = html + "<td scope='row'>" + categoria.Codigo + "</td>";
                    html = html + "<td>" + categoria.NombreCategoria + "</td>";
                    if (!categoria.TieneDetalle) {
                        html = html + "<td><strong>N/A</strong></td>";
                        html = html + "<td>" + categoria.EstadoRegistro.Nombre + "</td>";
                        html = html + "<td><strong>N/A</strong></td>";
                    }
                    else {
                        html = html + "<td>" + categoria.CantidadDetalleDesagregacion + "/" + categoria.DetalleCategoriaTexto.length + "</td>";
                        html = html + "<td>" + categoria.EstadoRegistro.Nombre + "</td>";
                        html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Cargar Detalle'  title='Cargar Detalle' class='btn-icon-base btn-upload'></button>" +
                            "<button type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Descargar Plantilla' title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                            "<button type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Agregar Detalle' title='Agregar Detalle' class='btn-icon-base btn-add'></button></td>";
                    }
                    html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Editar' title='Editar' class='btn-icon-base btn-edit'></button>";
                    if (categoria.idEstado != jsUtilidades.Variables.EstadoRegistros.EnProceso) {
                        html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' data-original-title='Clonar' value=" + categoria.id + " class='btn-icon-base btn-clone' ></button>";
                    }
                    else {
                        html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' disabled data-original-title='Clonar' value=" + categoria.id + " class='btn-icon-base btn-clone' ></button>";
                    }
                    if (categoria.idEstado == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                        html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + categoria.id + " class='btn-icon-base btn-power-off'></button>";
                    } else if (categoria.idEstado == jsUtilidades.Variables.EstadoRegistros.Activo) {
                        html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + categoria.id + " class='btn-icon-base btn-power-on'></button>";
                    }
                    else {
                        html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' disabled value=" + categoria.id + " class='btn-icon-base btn-power-off'></button>";
                    }
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' data-original-title='Eliminar' value=" + categoria.id + " class='btn-icon-base btn-delete'></button></td >";
                    html = html + "</tr>"
                }
                $(JsCategoria.Controles.tablacategoria).html(html);
                CargarDatasource();
                JsCategoria.Variables.ListadoCategoria = [];
            },
            "CargarTablaDetalleCategoria": function () {
                EliminarDatasource();
                let html = "";
                let formularioCompleto = JsCategoria.Variables.ListadoCategoriaDetalle.length == 0 ? false : JsCategoria.Variables.ListadoCategoriaDetalle[0].Completo;
                if (formularioCompleto) {
                    $(JsCategoria.Controles.btnGuardarDetalleCategoria).prop("disabled", true);
                    $(JsCategoria.Controles.btnFinalizarDetalle).prop("disabled", false);
                }
                else {
                    $(JsCategoria.Controles.btnGuardarDetalleCategoria).prop("disabled", false);
                    $(JsCategoria.Controles.btnFinalizarDetalle).prop("disabled", true);
                }
                for (var i = 0; i < JsCategoria.Variables.ListadoCategoriaDetalle.length; i++) {
                    let detalle = JsCategoria.Variables.ListadoCategoriaDetalle[i];
                    html = html + "<tr>"

                    html = html + "<td scope='row'>" + detalle.Codigo + "</td>";
                    html = html + "<td scope='row'>" + detalle.Etiqueta + "</td>";

                    html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' title='Editar' value=" + detalle.id + " class='btn-icon-base btn-edit'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' value=" + detalle.id + " class='btn-icon-base btn-delete'></button></td>";
                    html = html + "</tr>"
                }
                $(JsCategoria.Controles.TablaCategoriaDetalle).html(html);
                CargarDatasource();

       

                JsCategoria.Variables.ListadoCategoriaDetalle = [];
            },
            "CerrarFormulario": function () {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea salir del Formulario?", jsMensajes.Variables.actionType.cancelar)
                    .set('onok', function (closeEvent) {
                        
                    }); 
            },
            "HabilitarControlesTipoCategoria": function (selected) {
                if (selected == JsCategoria.Variables.TipoFecha) {
                    $(JsCategoria.Controles.divRangoMaximaCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divRangoMinimoCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMaxima).removeClass("hidden");
                    $(JsCategoria.Controles.divFechaMinima).removeClass("hidden");
                    $(JsCategoria.Controles.divCantidadDetalle).addClass("hidden");
                }
                else if (selected == JsCategoria.Variables.TipoNumerico) {
                    $(JsCategoria.Controles.divFechaMaxima).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMinima).addClass("hidden");
                    $(JsCategoria.Controles.divRangoMaximaCategoria).removeClass("hidden");
                    $(JsCategoria.Controles.divRangoMinimoCategoria).removeClass("hidden");
                    $(JsCategoria.Controles.divCantidadDetalle).addClass("hidden");
                }
                else {
                    $(JsCategoria.Controles.divRangoMaximaCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divRangoMinimoCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMaxima).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMinima).addClass("hidden");
                    $(JsCategoria.Controles.divCantidadDetalle).removeClass("hidden");
                }
            },
            "ValidarFormularioDetalle": function () {
                let validarFormulario = true;

                $(JsCategoria.Controles.CodigoDetalleHelp).addClass("hidden");
                $(JsCategoria.Controles.EtiquetaDetalleHelp).addClass("hidden");


                if ($(JsCategoria.Controles.txtEtiquetaDetalle).val().length == 0) {
                    $(JsCategoria.Controles.EtiquetaDetalleHelp).removeClass("hidden")
                    validarFormulario = false;
                }
                if ($(JsCategoria.Controles.txtCodigoDetalle).val().length == 0) {
                    $(JsCategoria.Controles.CodigoDetalleHelp).removeClass("hidden")
                    validarFormulario = false;
                }
                return validarFormulario;
            },
            "ValidarFormularioCategoria": function (opcion = true) {
                let validar = true;
                $(JsCategoria.Controles.ddlTipoCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMaximaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMaximaCategoriaHelp).addClass("hidden");
                if ($(JsCategoria.Controles.ddlTipoCategoria).val().length == 0) {
                    validar = false;
                    if (opcion) {
                        $(JsCategoria.Controles.ddlTipoCategoriaHelp).removeClass("hidden");
                    }             
                }
                if ($(JsCategoria.Controles.ddlTipoDetalle).val() == 0) {
                    validar = false
                    if (opcion) {
                        $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).removeClass("hidden");
                    }
              
                }
                if ($(JsCategoria.Controles.ddlTipoCategoria).val() != jsUtilidades.Variables.TipoCategoria.VariableDato)
                {
                    if ($(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Alfanumerico || $(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Texto) {
                        if ($(JsCategoria.Controles.txtCantidadDetalleCategoria).val() < 0) {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).removeClass("hidden");
                            }
                          
                        }
                    }
                    else if ($(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Numerico) {
                        if ($(JsCategoria.Controles.txtRangoMinimaCategoria).val() < 0) {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.RangoMinimaCategoriaHelp).removeClass("hidden");
                            }
                           
                        }
                        if ($(JsCategoria.Controles.txtRangoMaximaCategoria).val() == 0) {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.RangoMaximaCategoriaHelp).removeClass("hidden");
                            }
                         
                        }
                    }
                    else {
                        if (!moment($(JsCategoria.Controles.txtFechaMinimaCategoria).val(), 'YYYY-MM-DD').isValid()) {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.FechaMinimaCategoriaHelp).removeClass("hidden");
                            }
                          
                        }
                        if (!moment($(JsCategoria.Controles.txtFechaMaximaCategoria).val(), 'YYYY-MM-DD').isValid()) {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.FechaMaximaCategoriaHelp).removeClass("hidden");
                            }
                            
                        }
                    }
                }

                
               
             
                return validar;
            },
            "PrepararObjetoCategoria":function () {
                let categoria = new Object();
                categoria.Id =ObtenerValorParametroUrl("id");
                categoria.Codigo = $(JsCategoria.Controles.txtCodigoCategoria).val().trim();
                categoria.NombreCategoria = $(JsCategoria.Controles.txtNombreCategoria).val().trim();
                categoria.CantidadDetalleDesagregacion = $(JsCategoria.Controles.txtCantidadDetalleCategoria).val();
                categoria.idTipoDetalle = $(JsCategoria.Controles.ddlTipoDetalle).val();
                categoria.IdTipoCategoria = $(JsCategoria.Controles.ddlTipoCategoria).val();
                categoria.DetalleCategoriaNumerico = new Object();
                categoria.DetalleCategoriaNumerico.Minimo = $(JsCategoria.Controles.txtRangoMinimaCategoria).val();
                categoria.DetalleCategoriaNumerico.Maximo = $(JsCategoria.Controles.txtRangoMaximaCategoria).val();
                categoria.DetalleCategoriaFecha = new Object();
                categoria.DetalleCategoriaFecha.FechaMinima = $(JsCategoria.Controles.txtFechaMinimaCategoria).val();
                categoria.DetalleCategoriaFecha.FechaMaxima = $(JsCategoria.Controles.txtFechaMaximaCategoria).val();
                categoria.EsParcial = JsCategoria.Metodos.ValidarFormularioCategoria(false) == false ? true : false;
                return categoria;
            },
            "ValidacionTipoGuardado": function () {
                validar = JsCategoria.Metodos.ValidarFormularioCategoria(false);
                new Promise((resolve, reject) => {
                    if (!validar) {

                        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar un guardado parcial de la Categoría?", jsMensajes.Variables.actionType.agregar)
                            .set('onok', function () {
                                resolve(true);
                            })
                            .set('oncancel', function () {
                                resolve(false);
                            });
                    }
                    else {
                        resolve(true);
                    }

                }).then(result => {
                    if (result) {
                        let modo = ObtenerValorParametroUrl("modo");
                        if (modo == jsUtilidades.Variables.Acciones.Editar) {
                            JsCategoria.Consultas.ConsultaCategoriaPorId(result);
                            //if (obj.objetoRespuesta[0].idEstado == 1) {
                            //    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial de la Categoría?", jsMensajes.Variables.actionType.agregar)
                            //        .set('onok', function (closeEvent) {
                            //            JsCategoria.Consultas.EditarCategoria();
                            //        });
                            //} else {
                            //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Categoría?", jsMensajes.Variables.actionType.agregar)
                            //        .set('onok', function (closeEvent) {
                            //            JsCategoria.Consultas.EditarCategoria();
                            //        });
                            //}

                        }
                        else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Categoría?", jsMensajes.Variables.actionType.agregar)
                                .set('onok', function (closeEvent) {
                                    JsCategoria.Consultas.ClonarCategoria();
                                });
                        }
                        else {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos ¿Desea realizar un guardado parcial de la Categoría?", jsMensajes.Variables.actionType.agregar)
                                .set('onok', function (closeEvent) {
                                    JsCategoria.Consultas.InsertarCategoria();
                                });
                        }
                    }
                    else {
                        JsCategoria.Metodos.ValidarFormularioCategoria(true);
                    }
                });
            }
        },
        "Consultas": {
            "ConsultaListaCategoria": function () {
                $("#loading").fadeIn();
                execAjaxCall("/CategoriasDesagregacion/ObtenerListaCategorias", "GET")
                    .then((obj) => {
                        JsCategoria.Variables.ListadoCategoria = obj.objetoRespuesta;
                        JsCategoria.Metodos.CargarTablaCategoria();
                    }).catch((obj) => {
                        if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) { location.reload(); });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) {  })
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "ConsultaCategoriaPorId": function (result) {
                $("#loading").fadeIn();
                let idCategoria = ObtenerValorParametroUrl("id");
                execAjaxCall("/CategoriasDesagregacion/ObtenerCategoria?pid=" + idCategoria, "GET")
                    .then((obj) => {
                        if (result) {
                            let modo = ObtenerValorParametroUrl("modo");
                            if (modo == jsUtilidades.Variables.Acciones.Editar) {
                                if (obj.objetoRespuesta[0].idEstado == 1) {
                                    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial de la Categoría?", jsMensajes.Variables.actionType.agregar)
                                        .set('onok', function (closeEvent) {
                                            JsCategoria.Consultas.EditarCategoria();
                                        });
                                } else {
                                    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Categoría?", jsMensajes.Variables.actionType.agregar)
                                        .set('onok', function (closeEvent) {
                                            JsCategoria.Consultas.EditarCategoria();
                                        });
                                }

                            }
                            else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
                                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Categoría?", jsMensajes.Variables.actionType.agregar)
                                    .set('onok', function (closeEvent) {
                                        JsCategoria.Consultas.ClonarCategoria();
                                    });
                            }
                            else {
                                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Categoría?", jsMensajes.Variables.actionType.agregar)
                                    .set('onok', function (closeEvent) {
                                        JsCategoria.Consultas.InsertarCategoria();
                                    });
                            }
                        }
                        else {
                            JsCategoria.Metodos.ValidarFormularioCategoria(true);
                        }
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
            "ConsultaListaCategoriaDetalle": function () {
                $("#loading").fadeIn();
                let idCategoria =ObtenerValorParametroUrl("IdCategoria");
                execAjaxCall("/CategoriasDesagregacion/ObtenerListaCategoriasDetalle?idCategoria="+ idCategoria, "GET")
                    .then((obj) => {
                        JsCategoria.Variables.ListadoCategoriaDetalle = obj.objetoRespuesta;
                        JsCategoria.Metodos.CargarTablaDetalleCategoria();
                    }).catch((obj) => {
                        if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) { location.reload(); });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) {  })
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "FinalizarCategoria": function () {
                let categoria = new Object();
                categoria.id =ObtenerValorParametroUrl("IdCategoria");
                $("#loading").fadeIn();
                execAjaxCall("/CategoriasDesagregacion/CambiarEstadoFinalizado", "POST", categoria= categoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("La Categoria ha sido creada")
                            .set('onok', function (closeEvent) {
                                window.location.href = "/Fonatel/CategoriasDesagregacion/Index";
                            });         
                    }).catch((obj) => {
                        if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) { location.reload(); });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                                .set('onok', function (closeEvent) { });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "EliminarDetalleCategoria": function (categoriaDetalleid) {
                let DetalleCategoriaTexto = new Object();
                DetalleCategoriaTexto.id = categoriaDetalleid;
                DetalleCategoriaTexto.categoriaid =  ObtenerValorParametroUrl("IdCategoria");
                $("#loading").fadeIn();
                execAjaxCall("/CategoriasDesagregacion/EliminarCategoriasDetalle", "POST", DetalleCategoriaTexto)
                    .then((obj) => {
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
                            jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                                .set('onok', function (closeEvent) {  });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "ConsultaCategoriaDetalle": function () {
                let DetalleCategoriaTexto = new Object();
                DetalleCategoriaTexto.id = ObtenerValorParametroUrl("id");
               
                $("#loading").fadeIn();
                execAjaxCall('/CategoriasDesagregacion/ObtenerCategoriasDetalle', "POST", DetalleCategoriaTexto = DetalleCategoriaTexto )
                    .then((obj) => {
                        JsCategoria.Variables.ListadoCategoriaDetalle = obj.objetoRespuesta;
                        if (JsCategoria.Variables.ListadoCategoriaDetalle.length > 0) {
                            $(JsCategoria.Controles.txtCodigoDetalle).val(JsCategoria.Variables.ListadoCategoriaDetalle[0].Codigo);
                            $(JsCategoria.Controles.txtEtiquetaDetalle).val(JsCategoria.Variables.ListadoCategoriaDetalle[0].Etiqueta);
                            $(JsCategoria.Controles.txtCodigoDetalle).prop("disabled", true)
                            $(JsCategoria.Controles.btnGuardarDetalleCategoria).prop("disabled", false);
                        }
                    }).catch((obj) => {
                        if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) { location.reload(); });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) {  })
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "InsertarDetalleCategoria": function () {
                $("#loading").fadeIn();
                let DetalleCategoria = new Object();
                DetalleCategoria.categoriaid =ObtenerValorParametroUrl("IdCategoria");
                DetalleCategoria.Codigo = $(JsCategoria.Controles.txtCodigoDetalle).val().trim();
                DetalleCategoria.Etiqueta = $(JsCategoria.Controles.txtEtiquetaDetalle).val().trim();
                execAjaxCall("/CategoriasDesagregacion/InsertarCategoriasDetalle", "POST", DetalleCategoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("El Detalle ha sido agregado")
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
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
                                    
                                });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "CambiarEstadoCategoria": function (idCategoria, estado) {
                $("#loading").fadeIn();
                let Categoria = new Object()
                Categoria.id = idCategoria;
                Categoria.idEstado = estado;
                execAjaxCall("/CategoriasDesagregacion/CambiarEstadoCategoria", "POST", categoria=Categoria)
                    .then((obj) => {
                        let mensaje=""
                        if (estado == jsUtilidades.Variables.EstadoRegistros.Activo) {
                            mensaje = "La Categoría ha sido activada";
                        }
                        else if (estado == jsUtilidades.Variables.EstadoRegistros.Eliminado) {
                            mensaje = "La Categoría ha sido eliminada";
                        }
                        else {
                            mensaje="La Categoría ha sido desactivada";
                        }
                        jsMensajes.Metodos.OkAlertModal(mensaje)
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
            "InsertarCategoria": function () {
                $("#loading").fadeIn();
                let categoria = JsCategoria.Metodos.PrepararObjetoCategoria();
                execAjaxCall("/CategoriasDesagregacion/InsertarCategoria", "POST", categoria= categoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("La Categoría ha sido creada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
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
                          
                                });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "EditarCategoria": function () {
                $("#loading").fadeIn();

                let categoria = JsCategoria.Metodos.PrepararObjetoCategoria();

                execAjaxCall("/CategoriasDesagregacion/EditarCategoria", "POST", categoria= categoria)
                    .then((data) => {
                        jsMensajes.Metodos.OkAlertModal("La Categoría ha sido editada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
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
                                 
                                });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "ClonarCategoria": function () {
                $("#loading").fadeIn();
                let categoria = new Object();
                categoria.Id =ObtenerValorParametroUrl("id");
                categoria.Codigo = $(JsCategoria.Controles.txtCodigoCategoria).val().trim();
                categoria.NombreCategoria = $(JsCategoria.Controles.txtNombreCategoria).val().trim();
                categoria.CantidadDetalleDesagregacion = $(JsCategoria.Controles.txtCantidadDetalleCategoria).val();
                categoria.idTipoDetalle = $(JsCategoria.Controles.ddlTipoDetalle).val();
                categoria.IdTipoCategoria = $(JsCategoria.Controles.ddlTipoCategoria).val();
                categoria.DetalleCategoriaNumerico = new Object();
                categoria.DetalleCategoriaNumerico.Minimo = $(JsCategoria.Controles.txtRangoMinimaCategoria).val();
                categoria.DetalleCategoriaNumerico.Maximo = $(JsCategoria.Controles.txtRangoMaximaCategoria).val();
                categoria.DetalleCategoriaFecha = new Object();
                categoria.DetalleCategoriaFecha.FechaMinima = $(JsCategoria.Controles.txtFechaMinimaCategoria).val();
                categoria.DetalleCategoriaFecha.FechaMaxima = $(JsCategoria.Controles.txtFechaMaximaCategoria).val();
                execAjaxCall("/CategoriasDesagregacion/ClonarCategoria", "POST", categoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("La Categoría ha sido creada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
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
                            
                                });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "ImportarExcel": function () {
                $("#loading").fadeIn();
                var data;
                data = new FormData();
                data.append('file', $(JsCategoria.Controles.inputFileCargarDetalle)[0].files[0]);
                execAjaxCallFile("/CategoriasDesagregacion/CargaExcel", data)
                    .then((obj) => {
                        if (obj.HayError==0) {
                            jsMensajes.Metodos.OkAlertModal("Los Detalles han sido cargados")
                                .set('onok', function (closeEvent) { location.reload(); });
                        } else {
                            jsMensajes.Metodos.OkAlertErrorModal("Error al cargar los Detalles")
                                .set('onok', function (closeEvent) { location.reload(); })
                        }
                    }).catch((obj) => {
                        jsMensajes.Metodos.OkAlertErrorModal("Error al cargar los Detalles")
                            .set('onok', function (closeEvent) { location.reload(); })
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            "ModificarDetalleCategoria": function () {
                $("#loading").fadeIn();
                let detalleCategoria = new Object();
                detalleCategoria.id = ObtenerValorParametroUrl("id");
                detalleCategoria.categoriaid =ObtenerValorParametroUrl("IdCategoria");
                detalleCategoria.Codigo = $(JsCategoria.Controles.txtCodigoDetalle).val().trim();
                detalleCategoria.Etiqueta = $(JsCategoria.Controles.txtEtiquetaDetalle).val().trim();
                execAjaxCall("/CategoriasDesagregacion/ModificaCategoriasDetalle", "POST", detalleCategoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("El Detalle ha sido modificado")
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
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
            "ValidarExistenciaCategoria": function (idCategoria, estado) {
                $("#loading").fadeIn();
                let categoria = new Object()
                categoria.id = idCategoria;
                execAjaxCall("/CategoriasDesagregacion/ValidarCategoria", "POST", categoria)
                    .then((obj) => {
                        if (obj.objetoRespuesta.length == 0) {
                            JsCategoria.Consultas.CambiarEstadoCategoria(idCategoria, estado);
                        } else {
                            let dependencias = '';
                            for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                                dependencias = obj.objetoRespuesta[i] + "<br>"
                            }
                            if (estado == jsUtilidades.Variables.EstadoRegistros.Eliminado) {
                                jsMensajes.Metodos.ConfirmYesOrNoModal("La Categoría ya está en uso en el/los<br>" + dependencias + "<br>¿Desea eliminarla?", jsMensajes.Variables.actionType.eliminar)
                                    .set('onok', function (closeEvent) {
                                        JsCategoria.Consultas.CambiarEstadoCategoria(idCategoria, estado);
                                    });
                            }
                            else {
                                jsMensajes.Metodos.ConfirmYesOrNoModal("La Categoría ya está en uso en el/los<br>" + dependencias + "<br>¿Desea desactivarla?", jsMensajes.Variables.actionType.estado)
                                    .set('onok', function (closeEvent) {
                                        JsCategoria.Consultas.CambiarEstadoCategoria(idCategoria, estado);
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
            }   
        }
}

$(document).on("click", JsCategoria.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            preguntarAntesDeSalir = false;  window.location.href = "/Fonatel/CategoriasDesagregacion/Index"; 
        });   
});

$(document).on("click", JsCategoria.Controles.btnFinalizarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea guardar la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsCategoria.Consultas.FinalizarCategoria();
        });

    
});




$(document).on("click", JsCategoria.Controles.btnCancelarDetalle, function (e) {

    e.preventDefault();
    JsCategoria.Variables.ModoEditarAtributo = false;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/CategoriasDesagregacion/Index";
        });
});




$(document).on("change", JsCategoria.Controles.ddlTipoDetalle, function () {
    var selected = $(this).val();
    JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
});



$(document).on("change", JsCategoria.Controles.ddlTipoCategoria, function () {
    var selected = $(this).val();
    if (selected == jsUtilidades.Variables.TipoCategoria.VariableDato) {
        $(JsCategoria.Controles.ddlTipoDetalle).val(jsUtilidades.Variables.TipoDetalleCategoria.Numerico).trigger('change');
        $(JsCategoria.Controles.txtRangoMaximaCategoria).val(0);
        $(JsCategoria.Controles.txtRangoMinimaCategoria).val(0);
        $(JsCategoria.Controles.ddlTipoDetalle).prop("disabled", true);
        $(JsCategoria.Controles.txtRangoMinimaCategoria).addClass("disabled");
        $(JsCategoria.Controles.txtRangoMaximaCategoria).addClass("disabled");
    }
    else {
        $(JsCategoria.Controles.ddlTipoDetalle).prop("disabled", false);
        $(JsCategoria.Controles.txtRangoMinimaCategoria).removeClass("disabled");
        $(JsCategoria.Controles.txtRangoMaximaCategoria).removeClass("disabled");
    }
});




$(document).on("click", JsCategoria.Controles.btnEditarCategoria, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});


$(document).on("click", JsCategoria.Controles.btnDescargarDetalle, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar la Plantilla?", jsMensajes.Variables.actionType.descargar)
        .set('onok', function (closeEvent) {
            let win = window.open(jsUtilidades.Variables.urlOrigen + "/CategoriasDesagregacion/DescargarExcel?id=" + id);
            let timer = setInterval(function () {
                if (win.closed) {
                    clearInterval(timer);
                    jsMensajes.Metodos.OkAlertModal("La Plantilla ha sido descargada")
                        .set('onok', function (closeEvent) { });
                }
            }, 1000);
        });
});



$(document).on("click", JsCategoria.Controles.btnAddCategoria, function () {
    let idCategoria = $(this).val();
    window.location.href = "/Fonatel/CategoriasDesagregacion/Detalle?IdCategoria=" + idCategoria;
});




$(document).on("click", JsCategoria.Controles.btnDesactivarCategoria, function () {

    let id = $(this).val();
    let estado = jsUtilidades.Variables.EstadoRegistros.Activo;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
           
            JsCategoria.Consultas.CambiarEstadoCategoria(id, estado);
        });
});

$(document).on("click", JsCategoria.Controles.btnEliminarCategoria, function () {

    let id = $(this).val();
    let estado = jsUtilidades.Variables.EstadoRegistros.Eliminado;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Categoría?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {

            JsCategoria.Consultas.ValidarExistenciaCategoria(id, estado);;
        });
});




$(document).on("click", JsCategoria.Controles.btnActivarCategoria, function () {
    let id = $(this).val();
    let estado = jsUtilidades.Variables.EstadoRegistros.Desactivado;

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsCategoria.Consultas.ValidarExistenciaCategoria(id, estado);
            //JsCategoria.Consultas.CambiarEstadoCategoria(id, estado);
        });
});


$(document).on("click", JsCategoria.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();

    $(JsCategoria.Controles.txtCodigoCategoriaHelp).addClass("hidden");
    $(JsCategoria.Controles.txtnombreCategoriaHelp).addClass("hidden");
    $(JsCategoria.Controles.txtCodigoCategoria).parent().removeClass("has-error");
    $(JsCategoria.Controles.txtNombreCategoria).parent().removeClass("has-error");

    let validar = true;
    if ($(JsCategoria.Controles.txtNombreCategoria).val().length == 0) {

        $(JsCategoria.Controles.txtnombreCategoriaHelp).removeClass("hidden");
        $(JsCategoria.Controles.txtNombreCategoria).parent().addClass("has-error");
        validar = false;
    }
    if ($(JsCategoria.Controles.txtCodigoCategoria).val().length == 0) {

        $(JsCategoria.Controles.txtCodigoCategoriaHelp).removeClass("hidden");
        $(JsCategoria.Controles.txtCodigoCategoria).parent().addClass("has-error");
        validar = false;
    }
    if (!validar) {
        return;
    }
    JsCategoria.Metodos.ValidacionTipoGuardado();
});



$(document).on("click", JsCategoria.Controles.btnGuardarDetalleCategoria, function (e) {
    e.preventDefault();

    if (!JsCategoria.Variables.ModoEditarAtributo) {
        if (JsCategoria.Metodos.ValidarFormularioDetalle()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el detalle a la Categoría?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsCategoria.Consultas.InsertarDetalleCategoria();
                });
        }
    }
    else {

        if (JsCategoria.Metodos.ValidarFormularioDetalle()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea modificar el detalle a la Categoría?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsCategoria.Consultas.ModificarDetalleCategoria();
                  
                });
        }
    }

});





$(document).on("click", JsCategoria.Controles.btnClonarCategoria, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Categoría?", jsMensajes.Variables.actionType.clonar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
        });
});

$(document).on("click", JsCategoria.Controles.btnCargarDetalle, function (e) {
 
    $(JsCategoria.Controles.inputFileCargarDetalle).click();
});


$(document).on("change", JsCategoria.Controles.inputFileCargarDetalle, function (e) {
     JsCategoria.Consultas.ImportarExcel();
});



$(document).on("click", JsCategoria.Controles.btnEliminarDetalle, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Detalle?", jsMensajes.Variables.actionType.eliminar)
       .set('onok', function (closeEvent) {
         
           JsCategoria.Consultas.EliminarDetalleCategoria(id);
        });
});
$(document).on("click", JsCategoria.Controles.btnEditarDetalle, function (e) {
    JsCategoria.Variables.ModoEditarAtributo = true;

    let id = $(this).val();
    InsertarParametroUrl("id", id);
    JsCategoria.Consultas.ConsultaCategoriaDetalle();
});



//window.addEventListener('beforeunload', (event) => {

//    if (JsCategoria.Variables.OpcionSalir) {
//        // Cancel the event as stated by the standard.
//        event.preventDefault();
//        // Chrome requires returnValue to be set.
//        event.returnValue = '';
//    }

//});

//window.addEventListener('navigateback', function () {
//    alert("sadas");
//}, false);
$(function () {
    if ($(JsCategoria.Controles.FormularioCategorias).length > 0) {
        let modo =ObtenerValorParametroUrl("modo");
        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            $(JsCategoria.Controles.txtCodigoCategoria).prop("disabled", true);
            if ($(JsCategoria.Controles.ddlTipoDetalle).val()!="") {
                $(JsCategoria.Controles.ddlTipoDetalle).prop("disabled", true);
            }
            if ($(JsCategoria.Controles.ddlTipoCategoria).val() != "") {
                $(JsCategoria.Controles.ddlTipoCategoria).prop("disabled", true);
            }           
        }
        else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            if ($(JsCategoria.Controles.ddlTipoDetalle).val() != 0) {
                $(JsCategoria.Controles.ddlTipoDetalle).prop("disabled", true);
            }
            if ($(JsCategoria.Controles.ddlTipoCategoria).val() != 0) {
                $(JsCategoria.Controles.ddlTipoCategoria).prop("disabled", true);
            }
        }
        var selected = $(JsCategoria.Controles.ddlTipoDetalle).val();
        if (selected > 0) {
            JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
        }




        if ($(JsCategoria.Controles.ddlTipoCategoria).val() == jsUtilidades.Variables.TipoCategoria.VariableDato) {
            $(JsCategoria.Controles.txtRangoMinimaCategoria).addClass("disabled");
            $(JsCategoria.Controles.txtRangoMaximaCategoria).addClass("disabled");
        }
    }
    else if ($(JsCategoria.Controles.FormularioDetalle).length > 0) {
        JsCategoria.Consultas.ConsultaListaCategoriaDetalle();
        
    }
    else if ($(JsCategoria.Controles.FormularioIndex).length > 0) {
        JsCategoria.Consultas.ConsultaListaCategoria();
    }
});

