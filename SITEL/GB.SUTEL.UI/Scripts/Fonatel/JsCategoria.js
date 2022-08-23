    JsCategoria= {
        "Controles": {
            "FormularioCategorias": "#FormularioCategorias",
            "FormularioIndex":"#FormularioIndex",
            "FormularioDetalle": "#FormularioDetalle",
            "txtCodigo":"#txtCodigoCategoria",
            "EtiquetaDetalleHelp":"#txtEtiquetaDetalleHelp",
            "CodigoDetalleHelp": "#txtCodigoDetalleHelp",
            "ModalCargaExcel":"#modalImportarExcel",
            "txtmodoCategoria":"#txtmodoCategoria",
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

            "btnDescargarDetalle": "#TableCategoriaDesagregacion tbody tr td .btn-download",
            "inputFileCargarDetalle": "#inputFileCargarDetalle",
            "txtCodigoDetalle": "#txtCodigoDetalle",
            "txtEtiquetaDetalle": "#txtEtiquetaDetalle",
            "id":"#txtidCategoria"
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
                    html = html +     "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' data-original-title='Clonar' value=" + categoria.id + " class='btn-icon-base btn-clone' ></button>";
                    if (categoria.idEstado == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                        html = html +   "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + categoria.id + " class='btn-icon-base btn-power-off'></button></td >";
                    } else {
                        html = html +  "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + categoria.id + " class='btn-icon-base btn-power-on'></button></td >";
                    }       
                    html = html + "</tr>"
                }
                $(JsCategoria.Controles.tablacategoria).html(html);
                CargarDatasource();
                JsCategoria.Variables.ListadoCategoria = [];
            },
            "CargarTablaDetalleCategoria": function () {
                EliminarDatasource();
                let html = "";
                
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
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Salir del Formulario?", jsMensajes.Variables.actionType.cancelar)
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
            "ValidarFormularioCategoria": function () {
                let validar = true;

                $(JsCategoria.Controles.txtCodigoCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.txtnombreCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.ddlTipoCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMaximaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMaximaCategoriaHelp).addClass("hidden");



                if ($(JsCategoria.Controles.txtNombreCategoria).val().length == 0) {
                    validar = false;
                    $(JsCategoria.Controles.txtnombreCategoriaHelp).removeClass("hidden");
                }
                if ($(JsCategoria.Controles.txtCodigoCategoria).val().length == 0) {
                    validar = false;
                    $(JsCategoria.Controles.txtCodigoCategoriaHelp).removeClass("hidden");
                }
                if ($(JsCategoria.Controles.ddlTipoCategoria).val().length == 0) {
                    validar = false;
                    $(JsCategoria.Controles.ddlTipoCategoriaHelp).removeClass("hidden");
                }
                if ($(JsCategoria.Controles.ddlTipoDetalle).val() == 0) {
                    validar = false;
                    $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).removeClass("hidden");
                }
                else {

                    if ($(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Alfanumerico || $(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Texto) {
                        if ($(JsCategoria.Controles.txtCantidadDetalleCategoria).val()== 0) {
                            validar = false;

                            $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).removeClass("hidden");
                        }
                    }
                    else if ($(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Numerico) {
                        if ($(JsCategoria.Controles.txtRangoMinimaCategoria).val() == 0) {
                            validar = false;
                            $(JsCategoria.Controles.RangoMinimaCategoriaHelp).removeClass("hidden");
                        }
                        if ($(JsCategoria.Controles.txtRangoMaximaCategoria).val() == 0) {
                            validar = false;
                            $(JsCategoria.Controles.RangoMaximaCategoriaHelp).removeClass("hidden");
                        }
                    }
                    else {
                        if ($(JsCategoria.Controles.txtFechaMinimaCategoria).val() == "01/01/0001") {
                            validar = false;
                            $(JsCategoria.Controles.FechaMinimaCategoriaHelp).removeClass("hidden");
                        }
                        if ($(JsCategoria.Controles.txtFechaMaximaCategoria).val().length == "01/01/0001") {
                            validar = false;
                            $(JsCategoria.Controles.FechaMaximaCategoriaHelp).removeClass("hidden");
                        }
                    }
                }

                
               
             
                return validar;
            }
        },
        "Consultas": {
            "ConsultaListaCategoria": function () {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/ObtenerListaCategorias',
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            JsCategoria.Variables.ListadoCategoria = obj.objetoRespuesta;
                            JsCategoria.Metodos.CargarTablaCategoria();
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
            "ConsultaListaCategoriaDetalle": function () {
                let idCategoria = $(JsCategoria.Controles.id).val();
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/ObtenerListaCategoriasDetalle?idCategoria=' + idCategoria,
                    type: "GET",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            JsCategoria.Variables.ListadoCategoriaDetalle = obj.objetoRespuesta;
                            JsCategoria.Metodos.CargarTablaDetalleCategoria();
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
            "EliminarDetalleCategoria": function (idDetalleCategoria) {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/EliminarCategoriasDetalle',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { idDetalleCategoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            jsMensajes.Metodos.OkAlertModal("El Detalle ha sido eliminado")
                                .set('onok', function (closeEvent) {
                                    JsCategoria.Variables.ListadoCategoriaDetalle = obj.objetoRespuesta;
                                    JsCategoria.Metodos.CargarTablaDetalleCategoria();
                                });
                        } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) { location.reload(); });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                                .set('onok', function (closeEvent) { location.reload(); });
                        }
                    }
                }).fail(function (obj) {


                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { })
                    $("#loading").fadeOut();

                })
            },
            "ConsultaCategoriaDetalle": function (idDetalleCategoria) {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/ObtenerCategoriasDetalle?idCategoriaDetalle=' + idDetalleCategoria,
                    type: "GET",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            JsCategoria.Variables.ListadoCategoriaDetalle = obj.objetoRespuesta;
                            if (JsCategoria.Variables.ListadoCategoriaDetalle.length > 0) {
                                $(JsCategoria.Controles.txtCodigoDetalle).val(JsCategoria.Variables.ListadoCategoriaDetalle[0].Codigo);
                                $(JsCategoria.Controles.txtEtiquetaDetalle).val(JsCategoria.Variables.ListadoCategoriaDetalle[0].Etiqueta);
                                $(JsCategoria.Controles.txtCodigoDetalle).prop("disabled", true)

                            }
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
            "InsertarDetalleCategoria": function () {
                let detalleCategoria = new Object();
                detalleCategoria.categoriaid = $(JsCategoria.Controles.id).val();
                detalleCategoria.Codigo = $(JsCategoria.Controles.txtCodigoDetalle).val().trim();
                detalleCategoria.Etiqueta = $(JsCategoria.Controles.txtEtiquetaDetalle).val().trim();

                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/InsertarCategoriasDetalle',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { detalleCategoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            jsMensajes.Metodos.OkAlertModal("El detalle ha sido agregado")
                                .set('onok', function (closeEvent) {
                                    location.reload();
                                });
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
            "CambiarEstadoCategoria": function (idCategoria, estado) {
                let categoria = new Object()
                categoria.id = idCategoria;
                categoria.idEstado = estado;
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/CambiarEstadoCategoria',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { categoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            if (estado == 2) {
                                jsMensajes.Metodos.OkAlertModal("La Categoría ha sido activada")
                                    .set('onok', function (closeEvent) {
                                        location.reload();
                                    });
                            } else {
                                jsMensajes.Metodos.OkAlertModal("La Categoría ha sido desactivada")
                                    .set('onok', function (closeEvent) {
                                        location.reload();
                                    });
                            }
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
            "InsertarCategoria": function () {
                let categoria = new Object();
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
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/InsertarCategoria',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { categoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido creada")
                                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
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
            "EditarCategoria": function () {
                let categoria = new Object();
                categoria.Id = $(JsCategoria.Controles.id).val();
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
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/EditarCategoria',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { categoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido editada")
                                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
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
            "ClonarCategoria": function () {
                let categoria = new Object();
                categoria.Id = $(JsCategoria.Controles.id).val();
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
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/ClonarCategoria',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { categoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido creada")
                                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
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
            "ImportarExcel": function () {
                var data;
                data = new FormData();
                data.append('file', $(JsCategoria.Controles.inputFileCargarDetalle)[0].files[0]);
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/CargaExcel',
                    type: 'post',
                    datatype: 'json',
                    contentType: false,
                    processData: false,
                    async: false,
                    data: data,
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        jsMensajes.Metodos.OkAlertModal("El archivo ha sido importado")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });

                    }
                }).fail(function (obj) {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { })
                    $("#loading").fadeOut();

                })
            },
            "ModificarDetalleCategoria": function () {
                let detalleCategoria = new Object();
                detalleCategoria.categoriaid = $(JsCategoria.Controles.id).val();
                detalleCategoria.Codigo = $(JsCategoria.Controles.txtCodigoDetalle).val().trim();
                detalleCategoria.Etiqueta = $(JsCategoria.Controles.txtEtiquetaDetalle).val().trim();

                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/ModificaCategoriasDetalle',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { detalleCategoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            jsMensajes.Metodos.OkAlertModal("El detalle ha sido modificado")
                                .set('onok', function (closeEvent) {
                                    location.reload();
                                });
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
            "ValidarExistenciaCategoria": function (idCategoria, estado) {
                let categoria = new Object()
                categoria.id = idCategoria;


                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/CategoriasDesagregacion/ValidarCategoria',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data: { categoria },
                    success: function (obj) {
                        $("#loading").fadeOut();
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            if (obj.objetoRespuesta.length == 0) {
                                JsCategoria.Consultas.CambiarEstadoCategoria(idCategoria, estado);
                            } else {
                                let dependencias = '';
                                for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                                    dependencias = obj.objetoRespuesta[i]+"<br>"
                                }
                                jsMensajes.Metodos.ConfirmYesOrNoModal("La categoría ya está en uso en el/los<br>" + dependencias + "<br>¿Desea desactivarla?", jsMensajes.Variables.actionType.estado)
                                    .set('onok', function (closeEvent) {
                                        JsCategoria.Consultas.CambiarEstadoCategoria(idCategoria, estado);
                                    });
                            }
                        } else {
                            jsMensajes.Metodos.OkAlertErrorModal()
                                .set('onok', function (closeEvent) {
                                    location.reload();
                                });

                        }
                    }
                }).fail(function (obj) {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { })
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

$(document).on("click", JsCategoria.Controles.btnCancelarDetalle, function (e) {

    e.preventDefault();
    JsCategoria.Variables.ModoEditarAtributo = false;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            location.reload();
        });
});




$(document).on("change", JsCategoria.Controles.ddlTipoDetalle, function () {
    var selected = $(this).val();
    JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
});


$(document).on("click", JsCategoria.Controles.btnEditarCategoria, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});


$(document).on("click", JsCategoria.Controles.btnDescargarDetalle, function () {
    let id = $(this).val();
    window.open(jsUtilidades.Variables.urlOrigen + "/CategoriasDesagregacion/DescargarExcel?id=" + id);

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
    let modo = $(JsCategoria.Controles.txtmodoCategoria).val();
    let validar = JsCategoria.Metodos.ValidarFormularioCategoria();
    if (!validar) {
        return;
    }


    if (modo == jsUtilidades.Variables.Acciones.Editar) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar  la Categoría?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsCategoria.Consultas.EditarCategoria();
            });
    }
    else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar  la Categoría?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsCategoria.Consultas.ClonarCategoria();
            });
    }
    else {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Categoría?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsCategoria.Consultas.InsertarCategoria();
            });
    }
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
    JsCategoria.Consultas.ConsultaCategoriaDetalle(id);
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
        let modo = $(JsCategoria.Controles.txtmodoCategoria).val();
        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            $(JsCategoria.Controles.txtCodigoCategoria).prop("disabled", true);
        }
        else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            $(JsCategoria.Controles.ddlTipoDetalle).prop("disabled", true);
            $(JsCategoria.Controles.ddlTipoCategoria).prop("disabled", true);

            $(JsCategoria.Controles.txtCantidadDetalleCategoria).prop("disabled", true);
            $(JsCategoria.Controles.txtRangoMaximaCategoria).prop("disabled", true);
            $(JsCategoria.Controles.txtRangoMinimaCategoria).prop("disabled", true);
            $(JsCategoria.Controles.txtFechaMaximaCategoria).prop("disabled", true);
            $(JsCategoria.Controles.txtFechaMinimaCategoria).prop("disabled", true);

        }
        var selected = $(JsCategoria.Controles.ddlTipoDetalle).val();
        if (selected > 0) {
            JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
        }
    }
    else if ($(JsCategoria.Controles.FormularioDetalle).length > 0) {
        JsCategoria.Consultas.ConsultaListaCategoriaDetalle();
        
    }
    else if ($(JsCategoria.Controles.FormularioIndex).length > 0) {
        JsCategoria.Consultas.ConsultaListaCategoria();
    }
});

