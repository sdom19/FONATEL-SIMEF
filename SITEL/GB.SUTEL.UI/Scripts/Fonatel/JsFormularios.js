JsFormulario= {
    "Controles": {
        "FormularioIndex": "#FormularioIndex",
        "FormFormulario": "#FormFormulario",
        "tablaFormulario": "#TablaFormulario tbody",
        "TableIndicadorFormulario": "#TableIndicadorFormulario",
        "TableIndicadorFormularioBody": "#TableIndicadorFormulario tbody",
        "txtCodigoFormulario": "#txtCodigoFormulario",
        "txtNombreFormulario": "#txtNombreFormulario",
        "txtDescripcionFormulario": "#txtDescripcionFormulario",
        "txtCantidadIndicadoresFormulario": "#txtCantidadIndicadoresFormulario",
        "ddlFrecuanciaEnvio": "#ddlFrecuanciaEnvio",
        "txtCodigoFormularioHelp": "#txtCodigoFormularioHelp",
        "txtNombreFormularioHelp": "#txtNombreFormularioHelp",
        "txtDescripcionFormularioHelp": "#txtDescripcionFormularioHelp",
        "txtCantidadIndicadoresFormularioHelp": "#txtCantidadIndicadoresFormularioHelp",
        "ddlFrecuenciaHelp": "#ddlFrecuenciaHelp",
        "btnAgregarFormulario": "#TablaFormulario tbody tr td .btn-add",
        "btnEditarFormulario": "#TablaFormulario tbody tr td .btn-edit",
        "btnDeleteFormulario": "#TablaFormulario tbody tr td .btn-delete",
        "btnVisualizarFormulario": "#TablaFormulario tbody tr td .btn-view",
        "btnDeleteIndicador": "#TableIndicadorFormulario tbody tr td .btn-delete",
        "btnEditarIndicadores":"#TableIndicadorFormulario tbody tr td .btn-edit",
        
        "btnSiguienteFormulario":"#btnSiguienteFormulario",
        "btnCloneFormulario": "#TablaFormulario tbody tr td .btn-clone",
        "btnDesactivadoFormulario": "#TablaFormulario tbody tr td .btn-power-off",
        "btnActivadoFormulario": "#TablaFormulario tbody tr td .btn-power-on",
        "btnGuardar": "#btnGuardarFormulario",
        "btnCancelar": "#btnCancelarFormulario",
        "btnGuardarIndicador":"#btnGuardarIndicadorFormulario",
        "divContenedor": ".contenedor_formulario",
        "btnAtrasFormularioRegla": "#btnAtrasFormularioRegla",
        "btnGuardarFormularioCompleto": "#btnGuardarFormularioCompleto",
        "txtTituloHoja": "#txtTituloHoja",
        "txtNotasEncargadoFormulario": "#txtNotasEncargadoFormulario",
        "ddlIndicador": "#ddlIndicador",
        "ddlIndicadorHelp": "#ddlIndicadorHelp",
        "txtTituloHojaHelp": "#txtTituloHojaHelp",
        "txtNotasEncargadoFormularioHelp":"#txtNotasEncargadoFormularioHelp"

    },
    "Variables": {
        "ListadoFormulario":[],
        "ListadoDetalleIndicadores": [],

    },

    "Metodos": {

        "ValidarFormularioWebParcial": function () {
            let validar = true;

            $(JsFormulario.Controles.txtCodigoFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtNombreFormularioHelp).addClass("hidden");

            let codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            let nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();

            if (codigo == 0) {
                $(JsFormulario.Controles.txtCodigoFormularioHelp).removeClass("hidden");
                validar = false;
            }

            if (nombre == 0) {
                $(JsFormulario.Controles.txtNombreFormularioHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },

        "ValidarFormularioWebTotal": function () {
            let validar = true;

            $(JsFormulario.Controles.txtCodigoFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtNombreFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtDescripcionFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtCantidadIndicadoresFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.ddlFrecuenciaHelp).addClass("hidden");

            let codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            let nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();
            let descripcion = $(JsFormulario.Controles.txtDescripcionFormulario).val().trim();
            let cantidadIndicadores = $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val().trim();
            let idFrecuencia = $(JsFormulario.Controles.ddlFrecuanciaEnvio).val();

            if (codigo == 0) {
                $(JsFormulario.Controles.txtCodigoFormularioHelp).removeClass("hidden");
                validar = false;
            }

            if (nombre == 0) {
                $(JsFormulario.Controles.txtNombreFormularioHelp).removeClass("hidden");
                validar = false;
            }

            if (descripcion == 0) {
                $(JsFormulario.Controles.txtDescripcionFormularioHelp).removeClass("hidden");
                validar = false;
            }

            if (cantidadIndicadores == 0) {
                $(JsFormulario.Controles.txtCantidadIndicadoresFormularioHelp).removeClass("hidden");
                validar = false;
            }

            if (idFrecuencia == 0) {
                $(JsFormulario.Controles.ddlFrecuenciaHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },

        "ValidarFormularioIndicador": function () {
            let validar = true;

            $(JsFormulario.Controles.txtTituloHojaHelp).addClass("hidden");
            $(JsFormulario.Controles.txtNotasEncargadoFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.ddlIndicadorHelp).addClass("hidden");
            let notas = $(JsFormulario.Controles.txtNotasEncargadoFormulario).val().trim();
            let indicador = $(JsFormulario.Controles.ddlIndicador).val();
            let hoja = $(JsFormulario.Controles.txtTituloHoja).val().trim();

            //if (hoja.length == 0) {
            //    $(JsFormulario.Controles.txtTituloHojaHelp).removeClass("hidden");

            //    validar = false;
            //}
            //if (notas.length == 0) {
            //    $(JsFormulario.Controles.txtNotasEncargadoFormularioHelp).removeClass("hidden");
            //    validar = false;
            //}

            if (indicador == "") {
                $(JsFormulario.Controles.ddlIndicadorHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },

        "ValidarEstadoParcialFormulario": function (obj) {
            if (obj.Descripcion === '' || obj.CantidadIndicadores == 0 || obj.idFrecuencia == '')
                return jsUtilidades.Variables.EstadoRegistros.EnProceso;
            return jsUtilidades.Variables.EstadoRegistros.Activo;
        },

        "CargarTablasFormulario": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsFormulario.Variables.ListadoFormulario.length; i++) {
                let formulario = JsFormulario.Variables.ListadoFormulario[i];
                html = html + "<tr>"
                html = html + "<td scope='row'>" + formulario.Codigo + "</td>";
                html = html + "<td>" + formulario.Nombre + "</td>";
                if (formulario.ListaIndicadores == null) {
                    html = html + "<td>N/A</td>";
                }
                else {
                    html = html + "<td>" + formulario.ListaIndicadores + "</td>";
                }
                html = html + "<td>" + formulario.FrecuenciaEnvio.Nombre + "</td>";
                html = html + "<td>" + formulario.EstadoRegistro.Nombre + "</td>";
                html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + formulario.id + " data-original-title='Editar' title='Editar' class='btn-icon-base btn-edit'></button>";
                html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' data-original-title='Clonar' value=" + formulario.id + " class='btn-icon-base btn-clone' ></button>";
                if (formulario.idEstado == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + formulario.id + " class='btn-icon-base btn-power-off'></button>";
                } else {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + formulario.id + " class='btn-icon-base btn-power-on'></button>";
                }
                html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Visualizar' data-original-title='Visualizar' value=" + formulario.id + " class='btn-icon-base btn-view' ></button>";
                html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Eliminar' data-original-title='Eliminar' value=" + formulario.id + " class='btn-icon-base btn-delete' ></button></td >";


                html = html + "</tr>"
            }
            $(JsFormulario.Controles.tablaFormulario).html(html);
            CargarDatasource();
            JsFormulario.Variables.ListadoFormulario = [];
        },

        "CargarTablasIndicadores": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsFormulario.Variables.ListadoDetalleIndicadores.length; i++) {
                let indicadores = JsFormulario.Variables.ListadoDetalleIndicadores[i];
                html = html + "<tr>"
                html = html + "<td class='dt-center'>" + indicadores.Codigo + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.Nombre + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.GrupoIndicadores.Nombre + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.TipoIndicadores.Nombre + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.EstadoRegistro.Nombre + " </td>";
                html = html + "<td class='dt-center'><button type='button' data-toggle='tooltip' data-placement='top' title='Editar' value=" + indicadores.idIndicador + " class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar'  value=" + indicadores.idIndicador + " class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>"
            }
            $(JsFormulario.Controles.TableIndicadorFormularioBody).html(html);
            CargarDatasource();
            JsFormulario.Variables.ListadoDetalleIndicadores = [];
        },

        "CrearTablaFormulario": function (formulario, indicador) {
            let html = "";
            html = html + "<tr>"
            html = html + "<td scope='row'>" + formulario.Codigo + "</td>";
            html = html + "<td>" + formulario.Nombre + "</td>";
            html = html + "<td>" + indicador + "</td>";
            html = html + "<td>" + formulario.FrecuenciaEnvio.Nombre + "</td>";
            html = html + "<td>" + formulario.EstadoRegistro.Nombre + "</td>";
            html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + formulario.id + " data-original-title='Editar' title='Editar' class='btn-icon-base btn-edit'></button>";
            html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' data-original-title='Clonar' value=" + formulario.id + " class='btn-icon-base btn-clone' ></button>";
            if (formulario.idEstado == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + formulario.id + " class='btn-icon-base btn-power-off'></button>";
            } else {
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + formulario.id + " class='btn-icon-base btn-power-on'></button>";
            }
            html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Visualizar' data-original-title='Visualizar' value=" + formulario.id + " class='btn-icon-base btn-view' ></button>";
            html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Eliminar' data-original-title='Eliminar' value=" + formulario.id + " class='btn-icon-base btn-delete' ></button></td >";


            html = html + "</tr>"
            return html;
        },

        "CargarIndicadores": function (obj) {
            $(JsFormulario.Controles.txtTituloHoja).val(obj.TituloHojas);
            $(JsFormulario.Controles.txtNotasEncargadoFormulario).val(obj.NotasEncargado);
            $(JsFormulario.Controles.ddlIndicador).val(obj.idIndicador).change();
        }
    },

    "Consultas": {

        "InsertarFormularioWeb": function () {
            $("#loading").fadeIn();
            let formulario = new Object();
            formulario.Codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            formulario.Nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();
            formulario.Descripcion = $(JsFormulario.Controles.txtDescripcionFormulario).val().trim();
            formulario.CantidadIndicadores = $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val().trim();
            formulario.idFrecuencia = $(JsFormulario.Controles.ddlFrecuanciaEnvio).val();
            formulario.IdEstado = JsFormulario.Metodos.ValidarEstadoParcialFormulario(formulario);
            execAjaxCall("/FormularioWeb/InsertarFormularioWeb", "POST", formulario)
                .then((obj) => {
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);
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
                    $("a[href='#step-2']").trigger('click');
                });
        },

        "InsertarIndicadores": function () {
            $("#loading").fadeIn();
            let detalleFormulario = new Object();
            let formularioweb = new Object();
            detalleFormulario.TituloHojas = $(JsFormulario.Controles.txtTituloHoja).val().trim();
            detalleFormulario.NotasEncargado = $(JsFormulario.Controles.txtNotasEncargadoFormulario).val().trim();
            detalleFormulario.idIndicador = $(JsFormulario.Controles.ddlIndicador).val();
            formularioweb.id = ObtenerValorParametroUrl("id");
            detalleFormulario.formularioweb = formularioweb;
            execAjaxCall("/FormularioWeb/InsertarIndicadoresFormulario", "POST", detalleFormulario)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Indicador ha sido agregado")
                        .set('onok', function (closeEvent) {
                            JsFormulario.Consultas.ConsultaListaIndicadoresFormulario();
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

        "EliminarFormulario": function (idFormulario)
        {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        },

        "DesactivarFormulario": function (idFormulario) {
            let objFormulario = new Object()
            objFormulario.id = idFormulario;
            execAjaxCall("/FormularioWeb/ValidarFormulario", "POST", objFormulario)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Formulario ha sido desactivado")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
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

        "ValidarExistenciaFormulario": function (idFormulario, eliminado = false) {
            $("#loading").fadeIn();
            let objFormulario= new Object()
            objFormulario.id = idFormulario;
            execAjaxCall("/FormularioWeb/ValidarFormulario", "POST", objFormulario)
                .then((obj) => {
                    if (obj.objetoRespuesta.length == 0) {

                        if (eliminado) {
                            JsFormulario.Consultas.EliminarFormulario(idFormulario);

                        } else {
                            JsFormulario.Consultas.DesactivarFormulario(idFormulario);
                        }
                    } else {
                        let dependencias = obj.objetoRespuesta[0] + "<br>"
                        if (eliminado) {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Formulario ya está en uso en las<br>" + dependencias + "<br>¿Desea Eliminar?", jsMensajes.Variables.actionType.eliminar)
                                .set('onok', function (closeEvent) {
                                    JsFormulario.Consultas.EliminarFormulario(idFormulario);
                                })
                        }
                        else {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Formulario ya está en uso en las<br>" + dependencias + "<br>¿Desea desactivarla?", jsMensajes.Variables.actionType.estado)
                                .set('onok', function (closeEvent) {
                                    JsFormulario.Consultas.DesactivarFormulario(idFormulario);
                                })
                        }        
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

        "ConsultaListaFormularioWeb": function () {
            $("#loading").fadeIn();
            execAjaxCall("/FormularioWeb/ObtenerFormulariosWeb", "GET")
                .then((obj) => {
                    JsFormulario.Variables.ListadoFormulario = obj.objetoRespuesta;
                    JsFormulario.Metodos.CargarTablasFormulario();
                }).catch((obj) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaListaIndicadoresFormulario": function () {
            $("#loading").fadeIn();
            let id = ObtenerValorParametroUrl("id");
            execAjaxCall("/FormularioWeb/ObtenerIndicadoresFormulario?idFormulario="+id, "GET")
                .then((obj) => {
                    JsFormulario.Variables.ListadoDetalleIndicadores = obj.objetoRespuesta;
                    JsFormulario.Metodos.CargarTablasIndicadores();
                }).catch((obj) => {
                    jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                        .set('onok', function (closeEvent) {
                        });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaDetalleFormularioWebAjax": function (idIndicador, idFormulario) {
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/FormularioWeb/ObtenerDetalleFormularioWeb',
                type: "GET",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { idIndicador, idFormulario },
                success: function (obj) {
                    $("#loading").fadeOut();
                    JsFormulario.Metodos.CargarIndicadores(obj);
                }
            }).fail(function (obj) {
                $("#loading").fadeOut();
            })
        },
        // NO ME ESTA FUNCIONANDO
        "ConsultaDetalleFormularioWeb": function (idIndicador, idFormulario) {
            $("#loading").fadeIn();
            execAjaxCall("/FormularioWeb/ObtenerDetalleFormularioWeb", "GET", { idIndicador, idFormulario })
                .then((obj) => {
                    $("#loading").fadeOut();
                    JsFormulario.Metodos.CargarIndicadores(obj);
                }).catch((obj) => {
                    $("#loading").fadeOut();
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        }
    }

}

// CANCELAR
$(document).on("click", JsFormulario.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/FormularioWeb/Index";
        });
});

// GUARDAR INDICADOR
$(document).on("click", JsFormulario.Controles.btnGuardarIndicador, function (e) {
    e.preventDefault();
    if (JsFormulario.Metodos.ValidarFormularioIndicador()) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Indicador?", jsMensajes.Variables.actionType.cancelar)
            .set('onok', function (closeEvent) {
                JsFormulario.Consultas.InsertarIndicadores();
            });
    }     
});

// EDITAR INDICADORES
$(document).on("click", JsFormulario.Controles.btnEditarIndicadores, function () {
    let idIndicador = $(this).val();
    let idFormulario = $.urlParam('id');
    JsFormulario.Consultas.ConsultaDetalleFormularioWebAjax(idIndicador, idFormulario);
});

// EDITAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnEditarFormulario, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/FormularioWeb/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

// CLONAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnCloneFormulario, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/FormularioWeb/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
});

// GUARDAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnGuardar, function (e) {
    e.preventDefault();
    if (JsFormulario.Metodos.ValidarFormularioWebParcial()) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsFormulario.Consultas.InsertarFormularioWeb();
                jsMensajes.Metodos.OkAlertModal("El Formulario ha sido creado")
                    .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
            });
    }
});

// VISUALIZAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnVisualizarFormulario, function (e) {
    // Esta quemado por el tema del tiempo, para no generar conflictos con los posibles cambios de otros compañeros
    //let id = $(this).val();
    // valor quemado
    let id = 1;
    window.location.href = "/Fonatel/RegistroIndicadorFonatel/Create?id=" + id + "&modo=" + 6;
    //window.location.href = "/Fonatel/RegistroIndicadorFonatel/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Visualizar;
});

// SIGUIENTE FORMULARIO
$(document).on("click", JsFormulario.Controles.btnSiguienteFormulario, function (e) {
    e.preventDefault();
    let modo = $.urlParam('modo');
    if (modo == undefined) {
        if (JsFormulario.Metodos.ValidarFormularioWebTotal())
            JsFormulario.Consultas.InsertarFormularioWeb();
    }
    else
        $("a[href='#step-2']").trigger('click');

});

// DELETE(BORRAR) FORMULARIO
$(document).on("click", JsFormulario.Controles.btnDeleteFormulario, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Formulario?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsFormulario.Consultas.ValidarExistenciaFormulario(id, true);
        });
});

// DELETE(BORRAR) INDICADOR
$(document).on("click", JsFormulario.Controles.btnDeleteIndicador, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina el Indicador?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido eliminado")
                .set('onok', function (closeEvent) { });
        });
});

// DESACTIVAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnDesactivadoFormulario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Formulario?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Formulario ha sido activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});

// ACTIVAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnActivadoFormulario, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar el Formulario?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsFormulario.Consultas.ValidarExistenciaFormulario(id);
        });
});

// ATRAS
$(document).on("click", JsFormulario.Controles.btnAtrasFormularioRegla, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});

// GUARDAR FORMULARIO COMPLETO
$(document).on("click", JsFormulario.Controles.btnGuardarFormularioCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Formulario ha sido creado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
        });
});

$(function () {
    if ($(JsFormulario.Controles.FormFormulario).length > 0) {
        let modo = $.urlParam('modo'); 
        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            $(JsFormulario.Controles.txtCodigoFormulario).prop("disabled", true);
        }
    }
    if ($(JsFormulario.Controles.FormularioIndex).length > 0) {
        JsFormulario.Consultas.ConsultaListaFormularioWeb();
    }
});