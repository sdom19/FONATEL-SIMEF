﻿    JsCategoria = {
        Controles: {
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
            "btnAtrasCategoria":"#btnAtrasCategoria",
            "btnCancelarDetalle": "#btnCancelarDetalleCategoria",
            "btnEditarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-edit",
            "btnDesactivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-off",
            "btnActivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-on",
            "btnClonarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-clone",
            "btnAddCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-add",
            "btnViewCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-view",
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
        Variables: {
            "TipoFecha": 4,
            "TipoNumerico": 1,
            "OpcionSalir": true,
            "ListadoCategoria": [],
            "ListadoCategoriaDetalle": [],
            "ModoEditarAtributo": false
        },
        Mensajes: {
            preguntaAgregarDetalleACategoria: "¿Desea agregar el detalle a la Categoría de Desagregación?",
            preguntaEditarDetalleACategoria: "¿Desea editar el detalle de la Categoría de Desagregación?",
            MensajeErrorValorMinimoFecha: "La Fecha Final debe ser mayor a la Fecha Inicial",
            MensajeErrorValorMinimoNumerico: "El Valor Máximo debe ser mayor al Valor Mínimo",
            MensajeConfirmacionCategoriaCreada: "La Categoría de Desagregación ha sido creada",
            MensajeConfirmacionCategoriaEditada: "La Categoría de Desagregación ha sido editada",
            MensajeErrorCantidadDetalles: "La cantidad ingresada en Cantidad de Detalles no puede ser menor al valor actual",
            MensajeErrorCodigoYaExistente: "El Código ingresado ya se encuentra registrado",
            MensajeValorInferior: "La cantidad ingresada en Cantidad de Detalles no puede ser menor al valor actual"
        },

        Metodos: {
            CargarTablaCategoria: function () {
                EliminarDatasource();
                let html = "";
                for (var i = 0; i < JsCategoria.Variables.ListadoCategoria.length; i++) {
                    let categoria = JsCategoria.Variables.ListadoCategoria[i];

                    html = html + "<tr>"

                    html = html + "<td scope='row'>" + categoria.Codigo + "</td>";
                    html = html + "<td>" + categoria.NombreCategoria + "</td>";
                    if (!categoria.TieneDetalle) {
                        html = html + "<td><strong>N/A</strong></td>";
                        html = html + "<td>" + categoria.TipoCategoria.Nombre + "</td>";
                        html = html + "<td>" + categoria.EstadoRegistro.Nombre + "</td>";                     
                        html = html + "<td><strong>N/A</strong></td>";
                    }
                    else {
                        html = html + "<td>" + categoria.CantidadDetalleDesagregacion + "/" + categoria.DetalleCategoriaTexto.length + "</td>";
                        html = html + "<td>" + categoria.TipoCategoria.Nombre + "</td>";
                        html = html + "<td>" + categoria.EstadoRegistro.Nombre + "</td>";
                        html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Descargar Plantilla'  title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                            "<button type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Cargar plantilla con detalles' title='Cargar plantilla con detalles' class='btn-icon-base btn-upload'></button>" +
                            "<button type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Agregar/Editar Detalles' title='Agregar/Editar Detalles' class='btn-icon-base btn-add'></button></td>";
                    }
                    html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.id + " data-original-title='Editar' title='Editar' class='btn-icon-base btn-edit'></button>";
                    if (categoria.idEstadoRegistro != jsUtilidades.Variables.EstadoRegistros.EnProceso) {
                        html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' data-original-title='Clonar' value=" + categoria.id + " class='btn-icon-base btn-clone' ></button>";
                    }
                    else {
                        html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' disabled data-original-title='Clonar' value=" + categoria.id + " class='btn-icon-base btn-clone' ></button>";
                    }
                    if (categoria.idEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                        html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + categoria.id + " class='btn-icon-base btn-power-off'></button>";
                    } else if (categoria.idEstadoRegistro== jsUtilidades.Variables.EstadoRegistros.Activo) {
                        html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + categoria.id + " class='btn-icon-base btn-power-on'></button>";
                    }
                    else {
                        html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' disabled value=" + categoria.id + " class='btn-icon-base btn-power-off'></button>";
                    }
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Visualizar' data-original-title='Visualizar' value=" + categoria.id + " class='btn-icon-base btn-view'></button>";
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' data-original-title='Eliminar' value=" + categoria.id + " class='btn-icon-base btn-delete'></button></td >";
                    html = html + "</tr>"
                }
                $(JsCategoria.Controles.tablacategoria).html(html);
                CargarDatasource();
                JsCategoria.Variables.ListadoCategoria = [];
            },
            CargarTablaDetalleCategoria: function () {
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
            CerrarFormulario: function () {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea salir del Formulario?", jsMensajes.Variables.actionType.cancelar)
                    .set('onok', function (closeEvent) {
                        
                    }); 
            },
            HabilitarControlesTipoCategoria: function (selected) {
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
            ValidarFormularioDetalle: function () {
                let validarFormulario = true;

                $(JsCategoria.Controles.CodigoDetalleHelp).addClass("hidden");
                $(JsCategoria.Controles.EtiquetaDetalleHelp).addClass("hidden");
                $(JsCategoria.Controles.txtEtiquetaDetalle).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtCodigoDetalle).parent().removeClass("has-error");

                if ($(JsCategoria.Controles.txtEtiquetaDetalle).val().length == 0) {
                    $(JsCategoria.Controles.txtEtiquetaDetalle).parent().addClass("has-error");
                    $(JsCategoria.Controles.EtiquetaDetalleHelp).removeClass("hidden")
                    validarFormulario = false;
                }
                if ($(JsCategoria.Controles.txtCodigoDetalle).val().length == 0) {
                    $(JsCategoria.Controles.txtCodigoDetalle).parent().addClass("has-error");
                    $(JsCategoria.Controles.CodigoDetalleHelp).removeClass("hidden")
                    validarFormulario = false;
                }
            
                return validarFormulario;
            },
            ValidarFormularioCategoria: function (opcion = true) {
                let validar = true;
                $(JsCategoria.Controles.ddlTipoCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMaximaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMaximaCategoriaHelp).addClass("hidden");

                $(JsCategoria.Controles.ddlTipoCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.ddlTipoDetalle).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtRangoMaximaCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtFechaMinimaCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtFechaMaximaCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtCantidadDetalleCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtRangoMinimaCategoria).parent().removeClass("has-error");

                if ($(JsCategoria.Controles.ddlTipoCategoria).val().length == 0) {
                    validar = false;
                    if (opcion) {
                        $(JsCategoria.Controles.ddlTipoCategoriaHelp).removeClass("hidden");
                        $(JsCategoria.Controles.ddlTipoCategoria).parent().addClass("has-error");
                    }             
                }
                if ($(JsCategoria.Controles.ddlTipoDetalle).val() == 0) {
                    validar = false
                    if (opcion) {
                        $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).removeClass("hidden");
                        $(JsCategoria.Controles.ddlTipoDetalle).parent().addClass("has-error");
                    }
              
                }
                if ($(JsCategoria.Controles.ddlTipoCategoria).val() != jsUtilidades.Variables.TipoCategoria.VariableDato)
                {
                    if ($(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Alfanumerico || $(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Texto) {
                        if ($(JsCategoria.Controles.txtCantidadDetalleCategoria).val() < 0) {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).removeClass("hidden");
                                $(JsCategoria.Controles.txtCantidadDetalleCategoria).parent().addClass("has-error");
                            }
                          
                        }
                    }
                    else if ($(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Numerico) {
                        if ($(JsCategoria.Controles.txtRangoMinimaCategoria).val() == '') {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.RangoMinimaCategoriaHelp).removeClass("hidden");
                                $(JsCategoria.Controles.txtRangoMinimaCategoria).parent().addClass("has-error");
                            }
                           
                        }
                        if ($(JsCategoria.Controles.txtRangoMaximaCategoria).val() == '') {
                            validar = false;
                            if (opcion) {
                                $(JsCategoria.Controles.RangoMaximaCategoriaHelp).removeClass("hidden");
                                $(JsCategoria.Controles.txtRangoMaximaCategoria).parent().addClass("has-error");
                            }
                         
                        }
                    }
             
                }

                
               
             
                return validar;
            },
            PrepararObjetoCategoria:function () {
                let categoria = new Object();
                categoria.Id =ObtenerValorParametroUrl("id");
                categoria.Codigo = $(JsCategoria.Controles.txtCodigoCategoria).val().trim();
                categoria.NombreCategoria = $(JsCategoria.Controles.txtNombreCategoria).val().trim();
                categoria.CantidadDetalleDesagregacion = $(JsCategoria.Controles.txtCantidadDetalleCategoria).val();
                categoria.IdTipoDetalleCategoria= $(JsCategoria.Controles.ddlTipoDetalle).val();
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
            ValidarGuardadoCompletoSinDetalle: function () {
                let tipoDetalle = $(JsCategoria.Controles.ddlTipoDetalle).val();
                let tieneCategoriaMinima = $(JsCategoria.Controles.txtFechaMinimaCategoria).val().length > 0;
                let tieneCategoriaMaxima = $(JsCategoria.Controles.txtFechaMaximaCategoria).val().length > 0;
                let tieneRangoMinimoCategoria = $(JsCategoria.Controles.txtRangoMinimaCategoria).val().length > 0;
                let tieneRangoMaximoCategoria = $(JsCategoria.Controles.txtRangoMaximaCategoria).val().length > 0;
                let tieneDetalles = $(JsCategoria.Controles.txtCantidadDetalleCategoria).val() <= 0;

                if (tipoDetalle == jsUtilidades.Variables.TipoDetalleCategoria.Fecha && tieneCategoriaMinima && tieneCategoriaMaxima) {
                    return true;
                } else if (tipoDetalle == jsUtilidades.Variables.TipoDetalleCategoria.Numerico && tieneRangoMinimoCategoria && tieneRangoMaximoCategoria) {
                    return true;
                } else if (tipoDetalle == jsUtilidades.Variables.TipoDetalleCategoria.Alfanumerico && tieneDetalles) {
                    return true;
                } else if (tipoDetalle == jsUtilidades.Variables.TipoDetalleCategoria.Texto && tieneDetalles) {
                    return true;
                }
                return false;
            },            
            LimpiarErroresFormularioCategoria: function () {
                $(JsCategoria.Controles.ddlTipoCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.FechaMaximaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMinimaCategoriaHelp).addClass("hidden");
                $(JsCategoria.Controles.RangoMaximaCategoriaHelp).addClass("hidden");

                $(JsCategoria.Controles.ddlTipoCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.ddlTipoDetalle).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtRangoMaximaCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtFechaMinimaCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtFechaMaximaCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtCantidadDetalleCategoria).parent().removeClass("has-error");
                $(JsCategoria.Controles.txtRangoMinimaCategoria).parent().removeClass("has-error");
            },
            ValidacionCamposRequeridosCancelarGuardado: function () {
                JsCategoria.Metodos.LimpiarErroresFormularioCategoria();

                let tipoDetalleCategoria = $(JsCategoria.Controles.ddlTipoDetalle).val();

                if ($(JsCategoria.Controles.ddlTipoCategoria).val().length == 0) {

                    $(JsCategoria.Controles.ddlTipoCategoriaHelp).removeClass("hidden");
                    $(JsCategoria.Controles.ddlTipoCategoria).parent().addClass("has-error");
                }

                if (tipoDetalleCategoria.length == 0) {

                    $(JsCategoria.Controles.ddlTipoDetalleCategoriaHelp).removeClass("hidden");
                    $(JsCategoria.Controles.ddlTipoDetalle).parent().addClass("has-error");
                } else {

                    if (tipoDetalleCategoria == jsUtilidades.Variables.TipoDetalleCategoria.Numerico) {

                        if ($(JsCategoria.Controles.txtRangoMinimaCategoria).val().length == 0) {

                            $(JsCategoria.Controles.RangoMinimaCategoriaHelp).removeClass("hidden");
                            $(JsCategoria.Controles.txtRangoMinimaCategoria).parent().addClass("has-error");
                        }
                        if ($(JsCategoria.Controles.txtRangoMaximaCategoria).val().length == 0) {

                            $(JsCategoria.Controles.RangoMaximaCategoriaHelp).removeClass("hidden");
                            $(JsCategoria.Controles.txtRangoMaximaCategoria).parent().addClass("has-error");
                        }
                    }
                    if (tipoDetalleCategoria == jsUtilidades.Variables.TipoDetalleCategoria.Fecha) {

                        if (!moment($(JsCategoria.Controles.txtFechaMinimaCategoria).val(), 'YYYY-MM-DD').isValid()) {

                             $(JsCategoria.Controles.FechaMinimaCategoriaHelp).removeClass("hidden");
                             $(JsCategoria.Controles.txtFechaMinimaCategoria).parent().addClass("has-error");
                        }
                        if (!moment($(JsCategoria.Controles.txtFechaMaximaCategoria).val(), 'YYYY-MM-DD').isValid()) {
                             $(JsCategoria.Controles.FechaMaximaCategoriaHelp).removeClass("hidden");
                             $(JsCategoria.Controles.txtFechaMaximaCategoria).parent().addClass("has-error");
                        }
                    }
                    if (tipoDetalleCategoria == jsUtilidades.Variables.TipoDetalleCategoria.Alfanumerico || $(JsCategoria.Controles.ddlTipoDetalle).val() == jsUtilidades.Variables.TipoDetalleCategoria.Texto) {
                        if ($(JsCategoria.Controles.txtCantidadDetalleCategoria).val().length <= 0 || $(JsCategoria.Controles.txtCantidadDetalleCategoria).val() < 0) {
                            $(JsCategoria.Controles.CantidadDetalleCategoriaHelp).removeClass("hidden");
                            $(JsCategoria.Controles.txtCantidadDetalleCategoria).parent().addClass("has-error");
                        }
                    }
                }
            },
            ValidacionTipoGuardado: function () {
                validar = JsCategoria.Metodos.ValidarFormularioCategoria(true);

                new Promise((resolve, reject) => {
                    let modo = ObtenerValorParametroUrl("modo");
                    if (modo == jsUtilidades.Variables.Acciones.Editar) {
                        JsCategoria.Consultas.ConsultaCategoriaPorId(true);
                    }
                    else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
                        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Categoría de Desagregación?", jsMensajes.Variables.actionType.agregar)
                            .set('onok', function (closeEvent) {
                                JsCategoria.Consultas.ClonarCategoria();
                            });
                    }
                    else {
                        if (JsCategoria.Metodos.ValidarGuardadoCompletoSinDetalle()) {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea guardar la Categoría de Desagregación?", jsMensajes.Variables.actionType.agregar)
                                .set('onok', function (closeEvent) {
                                  //  ValidarFormularioCategoria();
                                    JsCategoria.Consultas.InsertarCategoria();
                                })
                                .set('oncancel', function (closeEvent) {
                                    JsCategoria.Metodos.ValidacionCamposRequeridosCancelarGuardado();
                                });
                        } else {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial de la Categoría de Desagregación?", jsMensajes.Variables.actionType.agregar)
                                .set('onok', function (closeEvent) {
                                    JsCategoria.Consultas.InsertarCategoria();
                                })
                                .set('oncancel', function (closeEvent) {
                                    JsCategoria.Metodos.ValidacionCamposRequeridosCancelarGuardado();
                                });
                        }
                    }
                });
            },

            AccionesEjecutarMensajesDeError: function (objError) {
                if (objError.MensajeError == JsCategoria.Mensajes.MensajeErrorValorMinimoFecha) {
                    $(JsCategoria.Controles.txtFechaMaximaCategoria).val("");
                }
                else if (objError.MensajeError == JsCategoria.Mensajes.MensajeErrorValorMinimoNumerico) {
                    $(JsCategoria.Controles.txtRangoMaximaCategoria).val("");
                } else if (objError.MensajeError == JsCategoria.Mensajes.MensajeErrorCantidadDetalles) {
                    $(JsCategoria.Controles.txtCantidadDetalleCategoria).val("");
                } else if (objError.MensajeError == JsCategoria.Mensajes.MensajeErrorCodigoYaExistente) {
                    $(JsCategoria.Controles.txtCodigoCategoria).val("");
                }else if (obj.MensajeError == JsCategoria.Mensajes.MensajeValorInferior) {
                     //location.reload();
                    $(JsCategoria.Controles.txtCantidadDetalleCategoria).val("");
                }
            }
        },
        Consultas: {
            ConsultaListaCategoria: function () {
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
            ConsultaCategoriaPorId: function (result) {
                $("#loading").fadeIn();
                let idCategoria = ObtenerValorParametroUrl("id");
                execAjaxCall("/CategoriasDesagregacion/ObtenerCategoria?pid=" + idCategoria, "GET")
                    .then((obj) => {
                        if (result) {
                            let modo = ObtenerValorParametroUrl("modo");
                            if (modo == jsUtilidades.Variables.Acciones.Editar) {
                                if (obj.objetoRespuesta[0].idEstadoRegistro == 1) {
                                    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Categoría de Desagregación?", jsMensajes.Variables.actionType.agregar)
                                        .set('onok', function (closeEvent) {
                                            JsCategoria.Consultas.EditarCategoria();
                                        })
                                        .set('oncancel', function (closeEvent) {
                                            JsCategoria.Metodos.ValidacionCamposRequeridosCancelarGuardado();
                                        });
                                } else {
                                    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Categoría de Desagregación?", jsMensajes.Variables.actionType.agregar)
                                        .set('onok', function (closeEvent) {
                                            JsCategoria.Consultas.EditarCategoria();
                                        })
                                        .set('oncancel', function (closeEvent) {
                                            JsCategoria.Metodos.ValidacionCamposRequeridosCancelarGuardado();
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
            ConsultaListaCategoriaDetalle: function () {
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
            FinalizarCategoria: function () {
                let categoria = new Object();
                categoria.id =ObtenerValorParametroUrl("IdCategoria");
                $("#loading").fadeIn();
                execAjaxCall("/CategoriasDesagregacion/CambiarEstadoFinalizado", "POST", categoria= categoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("La Categoría de Desagregación ha sido creada")
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
            EliminarDetalleCategoria: function (categoriaDetalleid) {
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
            ConsultaCategoriaDetalle: function () {
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
            InsertarDetalleCategoria: function () {
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
            CambiarEstadoCategoria: function (idCategoria, estado) {
                $("#loading").fadeIn();
                let Categoria = new Object()
                Categoria.id = idCategoria;
                Categoria.idEstadoRegistro = estado;
                execAjaxCall("/CategoriasDesagregacion/CambiarEstadoCategoria", "POST", categoria=Categoria)
                    .then((obj) => {
                        let mensaje=""
                        if (estado == jsUtilidades.Variables.EstadoRegistros.Activo) {
                            mensaje = "La Categoría de Desagregación ha sido activada";
                        }
                        else if (estado == jsUtilidades.Variables.EstadoRegistros.Eliminado) {
                            mensaje = "La Categoría de Desagregación ha sido eliminada";
                        }
                        else {
                            mensaje="La Categoría de Desagregación ha sido desactivada";
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
            InsertarCategoria: function () {
                $("#loading").fadeIn();
                let categoria = JsCategoria.Metodos.PrepararObjetoCategoria();
                execAjaxCall("/CategoriasDesagregacion/InsertarCategoria", "POST", categoria= categoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal(JsCategoria.Mensajes.MensajeConfirmacionCategoriaCreada)
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
                                    JsCategoria.Metodos.AccionesEjecutarMensajesDeError(obj);
                                });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            EditarCategoria: function () {
                $("#loading").fadeIn();

                let categoria = JsCategoria.Metodos.PrepararObjetoCategoria();

                execAjaxCall("/CategoriasDesagregacion/EditarCategoria", "POST", categoria= categoria)
                    .then((data) => {
                        jsMensajes.Metodos.OkAlertModal(JsCategoria.Mensajes.MensajeConfirmacionCategoriaEditada)
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

                                    JsCategoria.Metodos.AccionesEjecutarMensajesDeError(obj);
                                     
                                });
                        }
                    }).finally(() => {
                       $("#loading").fadeOut();
                    });
            },
            ClonarCategoria: function () {
                $("#loading").fadeIn();
                let categoria = new Object();
                categoria.Id =ObtenerValorParametroUrl("id");
                categoria.Codigo = $(JsCategoria.Controles.txtCodigoCategoria).val().trim();
                categoria.NombreCategoria = $(JsCategoria.Controles.txtNombreCategoria).val().trim();
                categoria.CantidadDetalleDesagregacion = $(JsCategoria.Controles.txtCantidadDetalleCategoria).val();
                categoria.IdTipoDetalleCategoria= $(JsCategoria.Controles.ddlTipoDetalle).val();
                categoria.IdTipoCategoria = $(JsCategoria.Controles.ddlTipoCategoria).val();
                categoria.DetalleCategoriaNumerico = new Object();
                categoria.DetalleCategoriaNumerico.Minimo = $(JsCategoria.Controles.txtRangoMinimaCategoria).val();
                categoria.DetalleCategoriaNumerico.Maximo = $(JsCategoria.Controles.txtRangoMaximaCategoria).val();
                categoria.DetalleCategoriaFecha = new Object();
                categoria.DetalleCategoriaFecha.FechaMinima = $(JsCategoria.Controles.txtFechaMinimaCategoria).val();
                categoria.DetalleCategoriaFecha.FechaMaxima = $(JsCategoria.Controles.txtFechaMaximaCategoria).val();
                execAjaxCall("/CategoriasDesagregacion/ClonarCategoria", "POST", categoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal(JsCategoria.Mensajes.MensajeConfirmacionCategoriaCreada)
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
                                    JsCategoria.Metodos.AccionesEjecutarMensajesDeError(obj);
                                    

                                });
                        }
                    }).finally(() => {
                        $("#loading").fadeOut();
                    });
            },
            ImportarExcel: function () {
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
            ModificarDetalleCategoria: function () {
                $("#loading").fadeIn();
                let detalleCategoria = new Object();
                detalleCategoria.id = ObtenerValorParametroUrl("id");
                detalleCategoria.categoriaid =ObtenerValorParametroUrl("IdCategoria");
                detalleCategoria.Codigo = $(JsCategoria.Controles.txtCodigoDetalle).val().trim();
                detalleCategoria.Etiqueta = $(JsCategoria.Controles.txtEtiquetaDetalle).val().trim();
                execAjaxCall("/CategoriasDesagregacion/ModificaCategoriasDetalle", "POST", detalleCategoria)
                    .then((obj) => {
                        jsMensajes.Metodos.OkAlertModal("El Detalle ha sido editado")
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
            ValidarExistenciaCategoria: function (idCategoria, estado) {
                $("#loading").fadeIn();
                let categoria = new Object()
                categoria.id = idCategoria;
                execAjaxCall("/CategoriasDesagregacion/ValidarCategoria", "POST", categoria)
                    .then((obj) => {

                        let dependencias = '';
                        for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                            if (obj.objetoRespuesta[i] != null) {
                                dependencias += obj.objetoRespuesta[i] + "<br>";
                            }
                        }

                        if (dependencias == '') {
                            JsCategoria.Consultas.CambiarEstadoCategoria(idCategoria, estado);
                        } else {

                            if (estado == jsUtilidades.Variables.EstadoRegistros.Eliminado) {
                                jsMensajes.Metodos.ConfirmYesOrNoModal("La Categoría de Desagregación está en uso en el/los<br>" + dependencias + "<br>¿Desea eliminarla?", jsMensajes.Variables.actionType.eliminar)
                                    .set('onok', function (closeEvent) {
                                        JsCategoria.Consultas.CambiarEstadoCategoria(idCategoria, estado);
                                    });
                            }
                            else {
                                jsMensajes.Metodos.ConfirmYesOrNoModal("La Categoría de Desagregación está en uso en el/los<br>" + dependencias + "<br>¿Desea desactivarla?", jsMensajes.Variables.actionType.estado)
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea guardar la Categoría de Desagregación?", jsMensajes.Variables.actionType.agregar)
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
    if (consultasFonatel) { return; }
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
    if (consultasFonatel) { return; }
    let id = $(this).val();
    let estado = jsUtilidades.Variables.EstadoRegistros.Activo;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Categoría de Desagregación?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
           
            JsCategoria.Consultas.CambiarEstadoCategoria(id, estado);
        });
});

$(document).on("click", JsCategoria.Controles.btnEliminarCategoria, function () {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    let estado = jsUtilidades.Variables.EstadoRegistros.Eliminado;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Categoría de Desagregación?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {

            JsCategoria.Consultas.ValidarExistenciaCategoria(id, estado);;
        });
});

$(document).on("click", JsCategoria.Controles.btnActivarCategoria, function () {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    let estado = jsUtilidades.Variables.EstadoRegistros.Desactivado;

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Categoría de Desagregación?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsCategoria.Consultas.ValidarExistenciaCategoria(id, estado);
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
            jsMensajes.Metodos.ConfirmYesOrNoModal(JsCategoria.Mensajes.preguntaAgregarDetalleACategoria, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsCategoria.Consultas.InsertarDetalleCategoria();
                });
        }
    }
    else {

        if (JsCategoria.Metodos.ValidarFormularioDetalle()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(JsCategoria.Mensajes.preguntaEditarDetalleACategoria, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsCategoria.Consultas.ModificarDetalleCategoria();
                  
                });
        }
    }

});

$(document).on("click", JsCategoria.Controles.btnClonarCategoria, function () {
    let id = $(this).val();
    
            window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
        
});

$(document).on("click", JsCategoria.Controles.btnCargarDetalle, function (e) {
    if (consultasFonatel) { return; }
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cargar los Detalles?", jsMensajes.Variables.actionType.cargar)
        .set('onok', function (closeEvent) {
            $(JsCategoria.Controles.inputFileCargarDetalle).click();
        });
});

$(document).on("change", JsCategoria.Controles.inputFileCargarDetalle, function (e) {
     JsCategoria.Consultas.ImportarExcel();
});

$(document).on("click", JsCategoria.Controles.btnEliminarDetalle, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el detalle de la Categoría de Desagregación?", jsMensajes.Variables.actionType.eliminar)
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

$(document).on("click", JsCategoria.Controles.btnViewCategoria, function (e) {
    let id = $(this).val();
    window.location.href = "/Fonatel/CategoriasDesagregacion/Visualizar?id=" + id;
});
$(document).on("click", JsCategoria.Controles.btnAtrasCategoria, function (e) {
    window.location.href = "/Fonatel/CategoriasDesagregacion/index";
});

$(JsCategoria.Controles.txtCantidadDetalleCategoria).on('input', function () {
    if (this.value.length > 30) {
        this.value = this.value.slice(0, 30);
    }
});

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

