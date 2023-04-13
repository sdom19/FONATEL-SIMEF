IndexView = {
    Controles: {
        tablaIndicador: "#tableIndicador tbody",
        btnEditarIndicador: "#tableIndicador tbody tr td .btn-edit",
        btnDesactivarIndicador: "#tableIndicador tbody tr td .btn-power-on",
        btnActivarIndicador: "#tableIndicador tbody tr td .btn-power-off",
        btnEliminarIndicador: "#tableIndicador tbody tr td .btn-delete",
        btnClonarIndicador: "#tableIndicador tbody tr td .btn-clone",
        btnViewIndicador: "#tableIndicador tbody tr td .btn-view",
        IndexView: "#dad1f1ea"
    },

    Variables: {
        indexViewURL: "/Fonatel/IndicadorFonatel/index",
        editViewURL: "/Fonatel/IndicadorFonatel/edit?id=",
        createViewURL: "/Fonatel/IndicadorFonatel/create?id=",
        cloneViewURL: "/Fonatel/IndicadorFonatel/clone?id=",
        indicadorViewurl:"/Fonatel/IndicadorFonatel/Visualiza?id="
    },

    Mensajes: {
        preguntaEliminarIndicador: "¿Desea eliminar el Indicador?",
        preguntaEliminarIndicadorDependencias: (pListado) => { return `El Indicador ya está en uso en el/los<br>${pListado}<br>¿Desea eliminarlo?` },
        exitoEliminarIndicador: "El Indicador ha sido eliminado",
        preguntaDesactivarIndicador: "¿Desea desactivar el Indicador?",
        preguntaDesactivarIndicadorDependencias: (pListado) => { return `El Indicador ya está en uso en el/los<br>${pListado}<br>¿Desea desactivarlo?`; },
        exitoDesactivarIndicador: "El Indicador ha sido desactivado",
        preguntaActivarIndicador: "¿Desea activar el Indicador?",
        exitoActivarIndicador: "El Indicador ha sido activado",
        preguntaClonarIndicador: "¿Desea clonar el Indicador?"
    },

    Metodos: {
        CargarTablaIndicadores: function () {
            $("#loading").fadeIn();

            IndexView.Consultas.ConsultaListaIndicadores()
                .then(data => {
                    IndexView.Metodos.InsertarDatosTablaIndicadores(data.objetoRespuesta);
                })
                .catch(error => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        InsertarDatosTablaIndicadores: function (listaIndicadores) {
            EliminarDatasource();
            let html = "";
            
            listaIndicadores?.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${ item.Codigo }</th>`;
                html += `<th scope='row'>${ item.Nombre }</th>`;
                html += `<th scope='row'>${ item.GrupoIndicadores.Nombre }</th>`;
                html += `<th scope='row'>${ item.TipoIndicadores.Nombre }</th>`;
                html += `<th scope='row'>${ item.EstadoRegistro.Nombre }</th>`;
                html += "<td>";
                html += `<button class="btn-icon-base btn-edit" type="button" data-toggle="tooltip" data-placement="top" title="Editar" value=${item.id}></button>`;

                if (item.IdEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.Activo) {
                    html += `<button class="btn-icon-base btn-clone" type="button" data-toggle="tooltip" data-placement="top" title="Clonar" value=${item.id}></button>`;
                    html += `<button class="btn-icon-base btn-power-on" type="button" data-toggle="tooltip" data-placement="top" title="Desactivar" value=${item.id}></button>`;
                }
                else if (item.IdEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.EnProceso) {
                    html += `<button class="btn-icon-base btn-clone" type="button" aria-hidden="true" disabled></button>`;
                    html += `<button class="btn-icon-base btn-power-on" type="button" aria-hidden="true" disabled></button>`;
                }
                else {
                    html += `<button class="btn-icon-base btn-clone" type="button" data-toggle="tooltip" data-placement="top" title="Clonar" value=${item.id}></button>`;
                    html += `<button class="btn-icon-base btn-power-off" type="button" data-toggle="tooltip" data-placement="top" title="Activar" value=${item.id}></button>`;
                }
                html += `<button class="btn-icon-base btn-view" type="button" data-toggle="tooltip" data-placement="top" title="Visualizar" value=${item.id}></button>`;
                html += `<button class="btn-icon-base btn-delete" type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" value=${item.id}></button>`;
                html += "</td>";
                html += "</tr>";
            });
            $(IndexView.Controles.tablaIndicador).html(html);
            CargarDatasource();
        },

        EliminarIndicador: function (pIdIndicador) {
            if (consultasFonatel) { return; }
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaEliminarIndicador, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return IndexView.Consultas.VerificarUsoIndicador(pIdIndicador);
                })
                .then(data => {
                    if (data.objetoRespuesta.length > 0) {
                        let dependencias = '';
                        for (var i = 0; i < data.objetoRespuesta.length; i++) {
                            dependencias += data.objetoRespuesta[i] + ".<br>";
                        }

                        $("#loading").fadeOut();
                        return new Promise((resolve, reject) => {
                            jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaEliminarIndicadorDependencias(dependencias), jsMensajes.Variables.actionType.eliminar)
                                .set('onok', function (closeEvent) {
                                    $("#loading").fadeIn();
                                    resolve(true);
                                });
                        })
                    }
                    else {
                        return true;
                    }
                })
                .then(data => {
                    return IndexView.Consultas.EliminarIndicador(pIdIndicador);
                })
                .then(data => {
                    $("#loading").fadeOut();
                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoEliminarIndicador)
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .then(data => {
                    window.location.href = IndexView.Variables.indexViewURL
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        DesactivarIndicador: function (pIdIndicador) {
            if (consultasFonatel) { return; }
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaDesactivarIndicador, jsMensajes.Variables.actionType.estado)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return IndexView.Consultas.VerificarUsoIndicador(pIdIndicador);
                })
                .then(data => {
                    if (data.objetoRespuesta.length > 0) {
                        let dependencias = '';
                        for (var i = 0; i < data.objetoRespuesta.length; i++) {
                            dependencias += data.objetoRespuesta[i] + ".<br>";
                        }

                        $("#loading").fadeOut();
                        return new Promise((resolve, reject) => {
                            jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaDesactivarIndicadorDependencias(dependencias), jsMensajes.Variables.actionType.estado)
                                .set('onok', function (closeEvent) {
                                    $("#loading").fadeIn();
                                    resolve(true);
                                });
                        })
                    }
                    else {
                        return true;
                    }
                })
                .then(data => {
                    return IndexView.Consultas.DesactivarIndicador(pIdIndicador);
                })
                .then(data => {
                    $("#loading").fadeOut();
                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoDesactivarIndicador)
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .then(data => {
                    window.location.href = IndexView.Variables.indexViewURL
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        ActivarIndicador: function (pIdIndicador) {
            if (consultasFonatel) { return; }
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaActivarIndicador, jsMensajes.Variables.actionType.estado)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return IndexView.Consultas.ActivarIndicador(pIdIndicador);
                })
                .then(data => {
                    $("#loading").fadeOut();
                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoActivarIndicador)
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .then(data => {
                    window.location.href = IndexView.Variables.indexViewURL
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        }
    },

    Consultas: {
        ConsultaListaIndicadores: function () {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaIndicadores', 'GET');
        },

        EliminarIndicador: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/EliminarIndicador', 'POST', { pIdIndicador: pIdIndicador });
        },

        DesactivarIndicador: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/DesactivarIndicador', 'POST', { pIdIndicador: pIdIndicador });
        },

        ActivarIndicador: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/ActivarIndicador', 'POST', { pIdIndicador: pIdIndicador });
        },

        VerificarUsoIndicador: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/VerificarUsoIndicador', 'GET', { pIdIndicador: pIdIndicador });
        }
    },

    Eventos: function () {
        $(document).on("click", IndexView.Controles.btnViewIndicador, function (e) {
            let id = $(this).val();
            window.location.href = IndexView.Variables.indicadorViewurl+ id;
        });

        $(document).on("click", IndexView.Controles.btnActivarIndicador, function () {
            IndexView.Metodos.ActivarIndicador($(this).val());
        });
        
        $(document).on("click", IndexView.Controles.btnDesactivarIndicador, function () {
            IndexView.Metodos.DesactivarIndicador($(this).val());
        });

        $(document).on("click", IndexView.Controles.btnEliminarIndicador, function () {
            IndexView.Metodos.EliminarIndicador($(this).val());
        });

        $(document).on("click", IndexView.Controles.btnEditarIndicador, function () {
            window.location.href = IndexView.Variables.editViewURL + encodeURIComponent($(this).val());
        });

        $(document).on("click", IndexView.Controles.btnClonarIndicador, function () {
            window.location.href = IndexView.Variables.cloneViewURL + encodeURIComponent($(this).val());
        });
    },

    Init: function () {
        IndexView.Eventos();
        IndexView.Metodos.CargarTablaIndicadores();
    }
}

CreateView = {
    Controles: {
        // ============ Modals ============
        modalTipoIndicador: "#modalTipoIndicador",
        tableModalTipoIndicador: "#tableModalTipoIndicador",
        tableModalTipoIndicador_tbody: "#tableModalTipoIndicador tbody",
        btnModalTipoIndicador: "#btnModalTipoIndicador",
        btnEliminarTipoIndicador: "#tableModalTipoIndicador tbody tr td .btn-delete",
        btnGuardarTipoIndicador: "#modalTipoIndicador #btnGuardarTipoIndicador",
        inputModalTipo: "#modalTipoIndicador #inputTipo",
        inputModalTipoHelp: "#modalTipoIndicador #inputTipoHelp",

        modalGrupoIndicador: "#modalGrupoIndicador",
        tableModalGrupoIndicador: "#tableModalGrupoIndicador",
        tableModalGrupoIndicador_tbody: "#tableModalGrupoIndicador tbody",
        btnModalGrupoIndicador: "#btnModalGrupoIndicador",
        btnEliminarGrupoIndicador: "#tableModalGrupoIndicador tbody tr td .btn-delete",
        btnGuardarGrupoIndicador: "#modalGrupoIndicador #btnGuardarGrupoIndicador",
        inputModalGrupo: "#modalGrupoIndicador #inputGrupo",
        inputModalGrupoHelp: "#modalGrupoIndicador #inputGrupoHelp",

        modalUnidadEstudio: "#modalUnidadEstudio",
        tableModalUnidadEstudio: "#tableModalUnidadEstudio",
        tableModalUnidadEstudio_tbody: "#tableModalUnidadEstudio tbody",
        btnModalUnidadEstudio: "#btnModalUnidadEstudio",
        btnEliminarUnidadEstudio: "#tableModalUnidadEstudio tbody tr td .btn-delete",
        btnGuardarUnidadEstudio: "#modalUnidadEstudio #btnGuardarUnidadEstudio",
        inputModalUnidadEstudio: "#modalUnidadEstudio #inputUnidadEstudio",
        inputModalUnidadEstudioHelp: "#modalUnidadEstudio #inputUnidadEstudioHelp",

        prefijoLabelsHelp: "Help",

        modoFormulario: "#modoFormulario",

        divTipoGrafico:"#divTipoGrafico",

        // ============ Formularios ============
        formIndicador: {
            form: "#formCrearIndicador",
            inputs: "#formCrearIndicador input, #formCrearIndicador .textareavalidar, #formCrearIndicador select",
            selects2: "#formCrearIndicador select",

            btnSiguienteCrearIndicador: "#btnSiguienteCrearIndicador",
            btnGuardarCrearIndicador: "#btnGuardarCrearIndicador",
            btnSiguienteEditarIndicador: "#btnSiguienteEditarIndicador",
            btnGuardarEditarIndicador: "#btnGuardarEditarIndicador",
            btnSiguienteClonarIndicador: "#btnSiguienteClonarIndicador",
            btnGuardarClonarIndicador: "#btnGuardarClonarIndicador",
            btnCancelar: "#btnCancelarIndicador",

            inputCodigo: "#inputCodigo",
            inputNombre: "#inputNombre",
            ddlTipo: "#ddlTipo",
            ddlFrecuencias: "#ddlFrecuencias",
            ddlClasificacion: "#ddlClasificacion",
            ddlTipoMedida: "#ddlTipoMedida",
            ddlGrupo: "#ddlGrupo",
            ddlUsoIndicador: "#ddlUsoIndicador",
            inputCantidadVariableDatosIndicador: "#inputCantidadVariableDatosIndicador",
            inputCantidadCategoriaIndicador: "#inputCantidadCategoriaIndicador",
            ddlUnidadEstudio: "#ddlUnidadEstudio",
            ddlUsoSolicitud: "#ddlUsoSolicitud",
            inputDescripcion: "#inputDescripcion",
            inputNota: "#inputNota",
            inputFuenteIndicador: "#inputFuenteIndicador",
            ddlTipoGrafico:"#ddlTipoGrafico",
        },

        formVariable: {
            btnSiguiente: "#btnSiguienteVariable",
            btnAtras: "#btnAtrasVariable",
            btnGuardar: "#btnGuardarVariable",
            btnEditarVariable: "#tableDetallesVariable tbody tr td .btn-edit",
            btnEliminarVariable: "#tableDetallesVariable tbody tr td .btn-delete",
            btnCancelar: "#btnCancelarVariable",

            form: "#formCrearVariable",
            inputs: "#formCrearVariable input, #formCrearVariable textarea, #formCrearVariable select",
            tablaDetallesVariable: "#tableDetallesVariable",
            tablaDetallesVariable_tbody: "#tableDetallesVariable tbody",
            inputNombreVariable: "#inputNombreVariable",
            inputDescripcionVariable: "#inputDescripcionVariable",
        },

        formCategoria: {
            btnSiguiente: "#btnSiguienteCategoria",
            btnAtras: "#btnAtrasCategoria",
            btnGuardar: "#btnGuardarCategoria",
            btnEditarCategoria: "#tableDetallesCategoria tbody tr td .btn-edit",
            btnEliminarCategoria: "#tableDetallesCategoria tbody tr td .btn-delete",
            btnCancelar: "#btnCancelarCategoria",

            form: "#formCrearCategoria",
            inputs: "#formCrearCategoria input, #formCrearCategoria textarea, #formCrearCategoria select",
            tablaDetallesCategoria: "#tableDetallesCategoria",
            tablaDetallesCategoria_tbody: "#tableDetallesCategoria tbody",
            ddlCategoriaIndicador: "#ddlCategoriaIndicador",
            ddlCategoriaDetalleIndicador: "#ddlCategoriaDetalleIndicador"
        },

        //btnstep: ".step_navigation_indicador div",
        //divContenedor: ".stepwizard-content-container",
        
        btnFinalizar: "#btnFinalizarIndicador",
        step1Indicador: "a[href='#step-1']",
        step2Variable: "a[href='#step-2']",
        step3Categoria: "a[href='#step-3']",
        CreateView: "#dad1f550",
        
    },

    Variables: {
        indexViewURL: "/Fonatel/IndicadorFonatel/index",
        btnEdit: (pValue) => `<button class="btn-icon-base btn-edit" type="button" data-toggle="tooltip" data-placement="top" title="Editar" value=${pValue}></button>`,
        btnDelete: (pValue) => `<button class="btn-icon-base btn-delete" type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" value=${pValue}></button>`,

        hizoCargaDetallesVariables: false,
        hizoCargaDetallesCategorias: false,
        listaVariablesDato: {},
        objEditarDetallesVariableDato: null,
        objEditarDetallesCategoria: null,
        cantidadDetallesCategoriaRegistrada: 0,
        cantidadDetallesVariablesRegistrada: 0,
        elIndicadorFueClonado: false,
        salida: "Salida",
        entradaSalida: "Entrada/salida",
        entradaSalidaM: "Entrada/Salida",
        entrada:"Entrada"
    },

    Mensajes: {
        preguntaCancelarAccion: "¿Desea cancelar la acción?",

        exitoCrearIndicador: "El Indicador ha sido creado",
        exitoEditarIndicador: "El Indicador ha sido editado",
        preguntaGuardadoParcialIndicador: "¿Desea realizar un guardado parcial del Indicador?",
        preguntaAgregarIndicador: "¿Desea agregar el Indicador?",
        preguntaFinalizarIndicador: "¿Desea guardar el Indicador?",
        preguntaGuardadoEditar: "¿Desea editar el Indicador?",
        preguntaGuardadoClonar: "¿Desea clonar el Indicador?",

        preguntaEliminarTipoIndicador: "¿Desea eliminar el Tipo Indicador?",
        exitoEliminarTipoIndicador: "El Tipo Indicador ha sido eliminado",
        exitoAgregarTipoIndicador: "El Tipo Indicador ha sido agregado",

        preguntaEliminarGrupo: "¿Desea eliminar el Grupo Indicador?",
        exitoEliminarGrupo: "El Grupo Indicador ha sido eliminado",
        exitoAgregarGrupo: "El Grupo Indicador ha sido agregado",

        preguntaEliminarUnidad: "¿Desea eliminar la Unidad de Estudio?",
        exitoEliminarUnidad: "La Unidad de Estudio ha sido eliminada",
        exitoAgregarUnidad: "La Unidad de Estudio ha sido agregada",

        preguntaEliminarVariable: "¿Desea eliminar la Variable?",
        exitoEliminarVariable: "La Variable ha sido eliminada",
        preguntaAgregarVariable: "¿Desea agregar la Variable?",
        preguntaEditarVariable: "¿Desea editar la Variable Dato?",
        exitoEditarVariable: "La Variable Dato ha sido editada",
        exitoAgregarVariable: "La Variable ha sido agregada",

        preguntaEliminarCategoria: "¿Desea eliminar la Categoría?",
        exitoEliminarCategoria: "La Categoría ha sido eliminada",
        preguntaAgregarCategoria: "¿Desea agregar la Categoría?",
        preguntaEditarCategoria: "¿Desea editar la Categoría?",
        exitoAgregarCategoria: "La Categoría ha sido agregada",
        exitoEditarCategoria: "La Categoría ha sido editada",
        preguntaClonarIndicador: "¿Desea clonar el Indicador?",

        existenCamposVacios: "Existen campos vacíos. "
    },

    Metodos: {
        // Formulario Indicador
        CrearObjFormularioIndicador: function (pEsGuardadoParcial) {
            let controles = CreateView.Controles.formIndicador;
            var formData = {
                id: ObtenerValorParametroUrl("id"),
                Codigo: $(controles.inputCodigo).val(),
                Nombre: $(controles.inputNombre).val(),
                Descripcion: $(controles.inputDescripcion).val(),
                CantidadVariableDato: $(controles.inputCantidadVariableDatosIndicador).val(),
                CantidadCategoriaDesagregacion: $(controles.inputCantidadCategoriaIndicador).val(),
                Interno: $(controles.ddlUsoIndicador).val(),
                Solicitud: $(controles.ddlUsoSolicitud).val(),
                Fuente: $(controles.inputFuenteIndicador).val(),
                Nota: $(controles.inputNota).val(),
                TipoIndicadores: {
                    id: $(controles.ddlTipo).val()
                },
                ClasificacionIndicadores: {
                    id: $(controles.ddlClasificacion).val()
                },
                GrupoIndicadores: {
                    id: $(controles.ddlGrupo).val()
                },
                UnidadEstudio: {
                    id: $(controles.ddlUnidadEstudio).val()
                },
                TipoMedida: {
                    id: $(controles.ddlTipoMedida).val()
                },
                FrecuenciaEnvio: {
                    id: $(controles.ddlFrecuencias).val()
                },
                GraficoInforme: {
                    id: $(controles.ddlTipoGrafico).val()
                },
                esGuardadoParcial: pEsGuardadoParcial ? true : false
            };
            return formData;
        },

        VerificarCamposIncompletosFormularioIndicador: function (pEsGuardadoParcial) {
            let prefijoHelp = CreateView.Controles.prefijoLabelsHelp;
            let camposObligatoriosGuardadoParcial = true;

            for (let input of $(CreateView.Controles.formIndicador.inputs)) {
                $(input).parent().removeClass("has-error");
                $("#" + $(input).attr("id") + prefijoHelp).css("display", "none");
            }

            let validacionFormulario = ValidarFormulario(CreateView.Controles.formIndicador.inputs);

            if (validacionFormulario.objetos.some(x => $(x).attr("id") === $(CreateView.Controles.formIndicador.inputCodigo).attr("id"))) {
                $(CreateView.Controles.formIndicador.inputCodigo + prefijoHelp).css("display", "block");
                $(CreateView.Controles.formIndicador.inputCodigo).parent().addClass("has-error");
                camposObligatoriosGuardadoParcial = false;
            }

            if (validacionFormulario.objetos.some(x => $(x).attr("id") === $(CreateView.Controles.formIndicador.inputNombre).attr("id"))) {
                $(CreateView.Controles.formIndicador.inputNombre + prefijoHelp).css("display", "block");
                $(CreateView.Controles.formIndicador.inputNombre).parent().addClass("has-error");
                camposObligatoriosGuardadoParcial = false;
            }

            if (!pEsGuardadoParcial) {
                for (let input of validacionFormulario.objetos) {
                    $("#" + $(input).attr("id") + prefijoHelp).css("display", "block");
                    $("#" + $(input).attr("id")).parent().addClass("has-error");
                }
            }
            return { guardadoParcial: camposObligatoriosGuardadoParcial, guardadoCompleto: validacionFormulario.puedeContinuar };
        },

        CambiarEstadoBtnSiguienteFormIndicador: function (pDesactivar) {
            $(CreateView.Controles.formIndicador.btnSiguienteCrearIndicador).prop('disabled', pDesactivar);
            $(CreateView.Controles.formIndicador.btnSiguienteEditarIndicador).prop('disabled', pDesactivar);
            $(CreateView.Controles.formIndicador.btnSiguienteClonarIndicador).prop('disabled', pDesactivar);
            $(CreateView.Controles.step2Variable).prop('disabled', pDesactivar);
        },

        CrearIndicador: function () {
            if (ValidarFormulario(CreateView.Controles.formIndicador.inputs).puedeContinuar) {
                $("#loading").fadeIn();
                CreateView.Consultas.CrearIndicador(this.CrearObjFormularioIndicador(false))
                    .then(data => {
                        InsertarParametroUrl("id", data.objetoRespuesta[0].id);
                        $(CreateView.Controles.step2Variable).trigger('click'); // cargar los respectivos datos
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => {
                        $("#loading").fadeOut();
                    });
            }
        },

        CrearIndicadorGuardadoParcial: function () {
            
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioIndicador(true);

            if (!validacion.guardadoParcial) {
                return;
            }

            if (!validacion.guardadoCompleto) {
                mensaje = CreateView.Mensajes.existenCamposVacios;
            }

            let rootObj = this;

            new Promise((resolve, reject) => {

                jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje + CreateView.Mensajes.preguntaGuardadoParcialIndicador, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function () { resolve(true); })
                    .set("oncancel", function () {
                        rootObj.VerificarCamposIncompletosFormularioIndicador(false);
                    })
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.CrearIndicador(this.CrearObjFormularioIndicador(true));
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoCrearIndicador)
                        .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL; });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        EditarIndicador: function () {
            if (ValidarFormulario(CreateView.Controles.formIndicador.inputs).puedeContinuar) {
                $("#loading").fadeIn();
                CreateView.Consultas.EditarIndicador(this.CrearObjFormularioIndicador(false))
                    .then(data => {
                        setTimeout(() => {
                            $(CreateView.Controles.step2Variable).trigger('click'); // cargar los respectivos datos
                        }, 600);
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => {
                        $("#loading").fadeOut();
                    });
            }
        },

        EditarIndicadorGuardadoParcial: function () {
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioIndicador(true);

            if (!validacion.guardadoParcial) {
                return;
            }

            if (!validacion.guardadoCompleto) {
                mensaje = CreateView.Mensajes.existenCamposVacios;
            }

            let rootObj = this;

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaGuardadoEditar, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function () { resolve(true); })
                    .set("oncancel", function () {
                        rootObj.VerificarCamposIncompletosFormularioIndicador(false);
                    })
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.EditarIndicador(this.CrearObjFormularioIndicador(true));
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEditarIndicador)
                        .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL; });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        ClonarIndicador: function () {
            if (ValidarFormulario(CreateView.Controles.formIndicador.inputs).puedeContinuar) {
                $("#loading").fadeIn();
                CreateView.Consultas.ClonarIndicador(this.CrearObjFormularioIndicador(false))
                    .then(data => {
                        CreateView.Variables.elIndicadorFueClonado = true;
                        InsertarParametroUrl("id", data.objetoRespuesta.id); // actualizar el id del URL (previamente se tiene el id del indicador para clonar)

                        $(CreateView.Controles.step2Variable).trigger('click'); // cargar los respectivos datos
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => {
                        $("#loading").fadeOut();
                    });
            }
        },

        ClonarIndicadorGuardadoParcial: function () {
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioIndicador(true);

            if (!validacion.guardadoParcial) {
                return;
            }

            if (!validacion.guardadoCompleto) {
                mensaje = CreateView.Mensajes.existenCamposVacios;
            }

            let rootObj = this;

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaClonarIndicador, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function () { resolve(true); })
                    .set("oncancel", function () {
                        rootObj.VerificarCamposIncompletosFormularioIndicador(false);
                    })
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.ClonarIndicador(this.CrearObjFormularioIndicador(true));
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoCrearIndicador)
                        .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL; });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        GuardadoDefinitivoIndicador: function (pIdIndicador) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaFinalizarIndicador, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) { resolve(true); });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.GuardadoDefinitivoIndicador(pIdIndicador);
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoCrearIndicador)
                        .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL; });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        // Modal Tipo Indicador
        AbrirModalTipoIndicador: function () {
            $("#loading").fadeIn();

            CreateView.Consultas.ConsultarListaTipoIndicador()
                .then(data => {
                    this.InsertarDatosTablaModalTipoIndicador(data.objetoRespuesta);
                })
                .then(_ => {
                    setTimeout(() => {
                        $(CreateView.Controles.modalTipoIndicador).modal('show');
                    }, 500);
                })
                .catch(error => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        InsertarDatosTablaModalTipoIndicador: function (pListaTipoIndicador) {
            EliminarDatasource(CreateView.Controles.tableModalTipoIndicador);
            let html = "";

            pListaTipoIndicador?.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${item.Nombre}</th>`;
                html += "<td>"
                html += CreateView.Variables.btnDelete(item.id)
                html += "</td>"
                html += "</tr>";
            });
            $(CreateView.Controles.tableModalTipoIndicador_tbody).html(html);
            CargarDatasource(CreateView.Controles.tableModalTipoIndicador);
        },

        EliminarTipoIndicador: function (pIdTipoIndicador) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminarTipoIndicador, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.EliminarTipoIndicador(pIdTipoIndicador);
                })
                .then(data => {
                    RemoverItemSelect2(CreateView.Controles.formIndicador.ddlTipo, pIdTipoIndicador);
                    RemoverItemDataTable(CreateView.Controles.tableModalTipoIndicador, `button[value='${pIdTipoIndicador}']`);

                    $("#loading").fadeOut();

                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEliminarTipoIndicador)
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearTipoIndicador: function (pNombre) {
            if ($.trim(pNombre).length <= 0) {
                $(CreateView.Controles.inputModalTipoHelp).css("display", "block");
                $(CreateView.Controles.inputModalTipo).parent().addClass("has-error");
                return;
            }
            $(CreateView.Controles.inputModalTipoHelp).css("display", "none");
            $(CreateView.Controles.inputModalTipo).parent().removeClass("has-error");
            $("#loading").fadeIn();

            CreateView.Consultas.CrearTipoIndicador(pNombre)
                .then(data => {
                    $(CreateView.Controles.inputModalTipo).val(null);

                    InsertarItemSelect2(
                        CreateView.Controles.formIndicador.ddlTipo,
                        data.objetoRespuesta[0].Nombre, data.objetoRespuesta[0].id);

                    InsertarItemDataTable(
                        CreateView.Controles.tableModalTipoIndicador,
                        [data.objetoRespuesta[0].Nombre, CreateView.Variables.btnDelete(data.objetoRespuesta[0].id)]);

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoAgregarTipoIndicador)
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        // Modal Grupo Indicador
        AbrirModalGrupoIndicador: function () {
            $("#loading").fadeIn();

            CreateView.Consultas.ConsultarListaGrupoIndicador()
                .then(data => {
                    this.InsertarDatosTablaModalGrupoIndicador(data.objetoRespuesta);
                })
                .then(_ => {
                    setTimeout(() => {
                        $(CreateView.Controles.modalGrupoIndicador).modal('show');
                    }, 500);
                })
                .catch(error => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        InsertarDatosTablaModalGrupoIndicador: function (pListaGrupoIndicador) {
            EliminarDatasource(CreateView.Controles.tableModalGrupoIndicador);
            let html = "";

            pListaGrupoIndicador?.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${item.Nombre}</th>`;
                html += "<td>"
                html += CreateView.Variables.btnDelete(item.id)
                html += "</td>"
                html += "</tr>";
            });
            $(CreateView.Controles.tableModalGrupoIndicador_tbody).html(html);
            CargarDatasource(CreateView.Controles.tableModalGrupoIndicador);
        },

        EliminarGrupoIndicador: function (pIdGrupoIndicador) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminarGrupo, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.EliminarGrupoIndicador(pIdGrupoIndicador);
                })
                .then(data => {
                    RemoverItemSelect2(CreateView.Controles.formIndicador.ddlGrupo, pIdGrupoIndicador);
                    RemoverItemDataTable(CreateView.Controles.tableModalGrupoIndicador, `button[value='${pIdGrupoIndicador}']`);

                    $("#loading").fadeOut();

                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEliminarGrupo)
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearGrupoIndicador: function (pNombre) {
            if ($.trim(pNombre).length <= 0) {
                $(CreateView.Controles.inputModalGrupoHelp).css("display", "block");
                $(CreateView.Controles.inputModalGrupo).parent().addClass("has-error");
                return;
            }
            $(CreateView.Controles.inputModalGrupoHelp).css("display", "none");
            $(CreateView.Controles.inputModalGrupo).parent().removeClass("has-error");
            $("#loading").fadeIn();

            CreateView.Consultas.CrearGrupoIndicador(pNombre)
                .then(data => {
                    $(CreateView.Controles.inputModalGrupo).val(null);

                    InsertarItemSelect2(
                        CreateView.Controles.formIndicador.ddlGrupo,
                        data.objetoRespuesta[0].Nombre, data.objetoRespuesta[0].id);

                    InsertarItemDataTable(
                        CreateView.Controles.tableModalGrupoIndicador,
                        [data.objetoRespuesta[0].Nombre, CreateView.Variables.btnDelete(data.objetoRespuesta[0].id)]);

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoAgregarGrupo)
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        // Modal Unidad Estudio
        AbrirModalUnidadEstudio: function () {
            $("#loading").fadeIn();

            CreateView.Consultas.ConsultarListaUnidadEstudio()
                .then(data => {
                    CreateView.Metodos.InsertarDatosTablaModalUnidadEstudio(data.objetoRespuesta);
                })
                .then(_ => {
                    setTimeout(() => {
                        $(CreateView.Controles.modalUnidadEstudio).modal('show');
                    }, 500);
                })
                .catch(error => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        InsertarDatosTablaModalUnidadEstudio: function (pListaUnidadEstudio) {
            EliminarDatasource(CreateView.Controles.tableModalUnidadEstudio);
            let html = "";

            pListaUnidadEstudio?.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${item.Nombre}</th>`;
                html += "<td>"
                html += CreateView.Variables.btnDelete(item.id)
                html += "</td>"
                html += "</tr>";
            });
            $(CreateView.Controles.tableModalUnidadEstudio_tbody).html(html);
            CargarDatasource(CreateView.Controles.tableModalUnidadEstudio);
        },

        EliminarUnidadEstudio: function (pIdUnidadEstudio) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminarUnidad, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.EliminarUnidadEstudio(pIdUnidadEstudio);
                })
                .then(data => {
                    RemoverItemSelect2(CreateView.Controles.formIndicador.ddlUnidadEstudio, pIdUnidadEstudio);
                    RemoverItemDataTable(CreateView.Controles.tableModalUnidadEstudio, `button[value='${pIdUnidadEstudio}']`);

                    $("#loading").fadeOut();

                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEliminarUnidad)
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearUnidadEstudio: function (pNombre) {
            if ($.trim(pNombre).length <= 0) {
                $(CreateView.Controles.inputModalUnidadEstudioHelp).css("display", "block");
                $(CreateView.Controles.inputModalUnidadEstudio).parent().addClass("has-error");
                return;
            }
            $(CreateView.Controles.inputModalUnidadEstudioHelp).css("display", "none");
            $(CreateView.Controles.inputModalUnidadEstudio).parent().removeClass("has-error");
            $("#loading").fadeIn();

            CreateView.Consultas.CrearUnidadEstudio(pNombre)
                .then(data => {
                    $(CreateView.Controles.inputModalUnidadEstudio).val(null);

                    InsertarItemSelect2(
                        CreateView.Controles.formIndicador.ddlUnidadEstudio,
                        data.objetoRespuesta[0].Nombre, data.objetoRespuesta[0].id);

                    InsertarItemDataTable(
                        CreateView.Controles.tableModalUnidadEstudio,
                        [data.objetoRespuesta[0].Nombre, CreateView.Variables.btnDelete(data.objetoRespuesta[0].id)]);

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoAgregarUnidad)
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        // Formulario Detalles Variable
        CargarDetallesVariable: function (pIndicador) {
            if (!CreateView.Variables.hizoCargaDetallesVariables) {
                $("#loading").fadeIn();
                CreateView.Consultas.ConsultarDetallesVariable(pIndicador) // tabla
                    .then(data => {
                        CreateView.Variables.cantidadDetallesVariablesRegistrada = data.objetoRespuesta.length;

                        this.ValidarCantidadDetallesVariablesDato();
                        this.InsertarDatosTablaDetallesVariable(data.objetoRespuesta);
                        // crear un objeto donde cada item es un map que contiene los datos la variable-dato
                        CreateView.Variables.listaVariablesDato = data.objetoRespuesta.reduce((map, obj) => (map[obj.id] = obj, map), {});
                        CreateView.Variables.hizoCargaDetallesVariables = true;
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => {
                        $("#loading").fadeOut();
                    });
            }
        },

        InsertarDatosTablaDetallesVariable: function (listaVariables) {
            EliminarDatasource(CreateView.Controles.formVariable.tablaDetallesVariable);
            let html = "";

            listaVariables?.forEach((item, index) => {
                html += "<tr>";
                html += `<th scope='row'>${item.NombreVariable}</th>`;
                html += `<th scope='row'>${item.Descripcion}</th>`;
                html += "<td>"
                html += `<button type="button" data-toggle="tooltip" data-placement="top" title="Editar" class="btn-icon-base btn-edit" value=${item.id}></button>` 
                html += `<button type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" class="btn-icon-base btn-delete" value=${item.id}></button>`
                html += "</td>"
                html += "</tr>";
            });
            $(CreateView.Controles.formVariable.tablaDetallesVariable_tbody).html(html);
            CargarDatasource(CreateView.Controles.formVariable.tablaDetallesVariable);
        },

        CargarFormularioEditarDetallesVariable: function (pId) {
            let variable = CreateView.Variables.listaVariablesDato[pId];
            CreateView.Variables.objEditarDetallesVariableDato = variable;

            $(CreateView.Controles.formVariable.inputNombreVariable).val(variable.NombreVariable);
            $(CreateView.Controles.formVariable.inputDescripcionVariable).val(variable.Descripcion);

            this.LimpiarMensajesValidacionFormularioDetallesVariable();
        },

        CrearObjDetallesVariable: function (pIndicador, pIdDetalle = null) {
            let controles = CreateView.Controles.formVariable;
            let formData = {
                id: pIdDetalle,
                idIndicadorString: pIndicador,
                NombreVariable: $(controles.inputNombreVariable).val(),
                Descripcion: $(controles.inputDescripcionVariable).val()
            };
            return formData;
        },

        ValidarFormularioDetallesVariable: function () {
            this.LimpiarMensajesValidacionFormularioDetallesVariable();

            let validacion = ValidarFormulario(CreateView.Controles.formVariable.inputs);

            for (let input of validacion.objetos) {
                $("#" + $(input).attr("id") + CreateView.Controles.prefijoLabelsHelp).css("display", "block");
                $(input).parent().addClass("has-error");
            }
            return validacion.puedeContinuar;
        },

        LimpiarMensajesValidacionFormularioDetallesVariable: function () {
            for (let input of $(CreateView.Controles.formVariable.inputs)) {
                $("#" + $(input).attr("id") + CreateView.Controles.prefijoLabelsHelp).css("display", "none");
                $(input).parent().removeClass("has-error");
            }
        },

        LimpiarValoresFormularioDetallesVariable: function () {
            $(CreateView.Controles.formVariable.inputNombreVariable).val(null);
            $(CreateView.Controles.formVariable.inputDescripcionVariable).val(null);
        },

        ValidarCantidadDetallesVariablesDato: function () {

            let cantidadIndicador = $(CreateView.Controles.formIndicador.inputCantidadVariableDatosIndicador).val();
            let cantidadEstablecida = cantidadIndicador == "" ? 0 : cantidadIndicador;

            if (cantidadEstablecida > CreateView.Variables.cantidadDetallesVariablesRegistrada) {
                $(CreateView.Controles.formVariable.btnSiguiente).prop("disabled", true);
                $(CreateView.Controles.formVariable.btnGuardar).prop("disabled", false);
            }
            else {
                $(CreateView.Controles.formVariable.btnSiguiente).prop("disabled", false);
                $(CreateView.Controles.formVariable.btnGuardar).prop("disabled", true);
            }
        },

        CancelarFormularioDetallesVariable: function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    window.location.href = CreateView.Variables.indexViewURL;
                });
        },

        EliminarDetallesVariable: function (pIdIndicador, pIdDetalle) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminarVariable, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) { resolve(true) });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.EliminarDetalleVariableDato(this.CrearObjDetallesVariable(pIdIndicador, pIdDetalle));
                })
                .then(data => {
                    delete CreateView.Variables.listaVariablesDato[pIdDetalle];

                    if (pIdDetalle === CreateView.Variables.objEditarDetallesVariableDato?.id) { // en caso de que se este editando, no actualicen y se presione el boton de eliminar
                        CreateView.Variables.objEditarDetallesVariableDato = null;
                        this.LimpiarValoresFormularioDetallesVariable();
                    }

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEliminarVariable).set('onok', function (closeEvent) {
                        CreateView.Variables.hizoCargaDetallesVariables = false;
                        CreateView.Metodos.CargarDetallesVariable(pIdIndicador);
                    });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        EditarDetallesVariable: function (pIdIndicador) {
            let formValido = this.ValidarFormularioDetallesVariable();

            if (!formValido) {
                return;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEditarVariable, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.EditarDetalleVariableDato(
                        this.CrearObjDetallesVariable(pIdIndicador, CreateView.Variables.objEditarDetallesVariableDato.id)
                    );
                })
                .then(data => {
                    this.LimpiarValoresFormularioDetallesVariable();
                    CreateView.Variables.objEditarDetallesVariableDato = null;

                    CreateView.Variables.listaVariablesDato[data.objetoRespuesta[0].id] = data.objetoRespuesta[0]; // actualizar el item localmente

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEditarVariable)
                        .set('onok', function (closeEvent) {
                            CreateView.Variables.hizoCargaDetallesVariables = false;
                            CreateView.Metodos.CargarDetallesVariable(pIdIndicador);
                        });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearDetallesVariables: function (pIdIndicador) {
            let formValido = this.ValidarFormularioDetallesVariable();

            if (!formValido) { return; }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaAgregarVariable, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.CrearDetalleVariableDato(this.CrearObjDetallesVariable(pIdIndicador));
                })
                .then(data => {
                    this.LimpiarValoresFormularioDetallesVariable();
                    CreateView.Variables.listaVariablesDato[data.objetoRespuesta[0].id] = data.objetoRespuesta[0]; // guardar el item localmente

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoAgregarVariable)
                        .set('onok', function (closeEvent) {
                            CreateView.Variables.hizoCargaDetallesVariables = false;
                            CreateView.Metodos.CargarDetallesVariable(pIdIndicador);
                        });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        // Formulario Detalles Categorias
        CargarDetallesCategoria: function (pIndicador) { // combo categorias y tabla
            if (!CreateView.Variables.hizoCargaDetallesCategorias) {
                $("#loading").fadeIn();

                CreateView.Consultas.ConsultarCategoriasDesagregacion() // combo
                    .then(data => {
                        let dataSet = []
                        data.objetoRespuesta?.forEach(item => {
                            dataSet.push({ value: item.id, text: item.NombreCategoria });
                        });

                        $(CreateView.Controles.formCategoria.ddlCategoriaIndicador).empty();

                        InsertarDataSetSelect2(CreateView.Controles.formCategoria.ddlCategoriaIndicador, dataSet);
                        CreateView.Metodos.LimpiarValoresFormularioDetallesCategoria();
                        return true;
                    }).
                    then(data => {
                        if (pIndicador) { // modo edición?
                            return CreateView.Consultas.ConsultarDetallesCategoria(pIndicador); // tabla
                        }
                        else {
                            return false;
                        }
                    }).
                    then(data => {
                        if (data) {
                            CreateView.Variables.cantidadDetallesCategoriaRegistrada = data.objetoRespuesta.length;
                            this.ValidarCantidadDetallesCategoria();
                            this.InsertarDatosTablaDetallesCategoria(data.objetoRespuesta);
                        }
                       
                        CreateView.Variables.hizoCargaDetallesCategorias = true;
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => {
                        $("#loading").fadeOut();
                    });
            }
        },

        InsertarDatosTablaDetallesCategoria: function (listaVariables) {
            EliminarDatasource(CreateView.Controles.formCategoria.tablaDetallesCategoria);
            let html = "";

            listaVariables?.forEach((item, index) => {
                html += "<tr>";
                html += `<th scope='row'>${item.Codigo}</th>`;
                html += `<th>${item.NombreCategoria}</th>`;
                html += `<th>${item.Etiquetas}</th>`;
                html += "<td>"
                html += `<button type="button" data-toggle="tooltip" data-placement="top" title="Editar" class="btn-icon-base btn-edit" value=${item.idCategoriaString}></button>`
                html += `<button type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" class="btn-icon-base btn-delete" value=${item.idCategoriaString}></button>`
                html += "</td>"
                html += "</tr>";
            });
            $(CreateView.Controles.formCategoria.tablaDetallesCategoria_tbody).html(html);
            CargarDatasource(CreateView.Controles.formCategoria.tablaDetallesCategoria);
        },

        CargarDetallesDeLaCategoria: function (pIdCategoria, pIdIndicador = null) { // combo detalles
            $("#loading").fadeIn();

            $(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador).empty();

            CreateView.Consultas.ConsultarDetallesDeCategoriaDesagregacion(pIdCategoria) // consultar los detalles de la categoria
                .then(data => {
                    let dataSet = [];
                    data.objetoRespuesta?.forEach(item => {
                        dataSet.push({ value: item.id, text: item.Etiqueta });
                    });

                    if (dataSet.length > 0) {
                        $(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador).prop("disabled", false);
                        InsertarOpcionTodosSelect2Multiple(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador);
                        InsertarDataSetSelect2(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador, dataSet);
                    }
                    else {
                        $(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador).prop("disabled", true);
                    }

                    return true;
                })
                .then(data => {
                    if (pIdIndicador) { // caso de editar, consultar los detalles seleccionados
                        return CreateView.Consultas.ConsultarListaDetallesDeCategoriaGuardada(pIdIndicador, pIdCategoria);
                    }
                    else {
                        return false;
                    }
                })
                .then(data => { // en caso de editar
                    if (data) {
                        CreateView.Variables.objEditarDetallesCategoria = {
                            idIndicador: pIdIndicador,
                            idCategoria: pIdCategoria,
                            detalles: data.objetoRespuesta
                        };
                        
                        SeleccionarItemsSelect2Multiple(
                            CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador,
                            data.objetoRespuesta, "idCategoriaDetalleString");
                    }
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CargarFormularioEditarDetallesCategoria: function (pIdCategoria, pIdIndicador) {
            $(CreateView.Controles.formCategoria.btnGuardar).prop("disabled", false);
            $(CreateView.Controles.formCategoria.ddlCategoriaIndicador).prop("disabled", true);
            SeleccionarItemSelect2(CreateView.Controles.formCategoria.ddlCategoriaIndicador, pIdCategoria);
            this.CargarDetallesDeLaCategoria(pIdCategoria, pIdIndicador);
            this.LimpiarMensajesValidacionFormularioDetallesCategoria();
        },

        CrearObjDetallesCategoria: function (pIndicador, pCategoria) {
            let listadoCategoriasDesagracion = $(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador).val();

            let formData = {
                id: CreateView.Variables.objEditarDetallesCategoria?.idCategoria,
                idIndicadorString: pIndicador,
                idCategoriaString: pCategoria,
                listaDetallesCategoriaString: [],
                NombreCategoria: $(`${CreateView.Controles.formCategoria.ddlCategoriaIndicador} option:selected`).text()
            };

            for (let i = 0; i < listadoCategoriasDesagracion?.length; i++) {
                if (listadoCategoriasDesagracion[i] !== "all") {
                    formData.listaDetallesCategoriaString.push(listadoCategoriasDesagracion[i]);
                }
            }

            return formData;
        },

        ValidarFormularioDetallesCategoria: function () {
            this.LimpiarMensajesValidacionFormularioDetallesCategoria();

            let listaExcepciones = ["select2-search__field"];

            if ($(`${CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador} > option`).length < 1) {
                listaExcepciones.push(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador.slice(1));
            }

            let validacion = ValidarFormulario(CreateView.Controles.formCategoria.inputs, listaExcepciones);

            for (let input of validacion.objetos) {
                $("#" + $(input).attr("id") + CreateView.Controles.prefijoLabelsHelp).css("display", "block");
                $(input).parent().addClass("has-error");
            }
            return validacion.puedeContinuar;
        },

        LimpiarMensajesValidacionFormularioDetallesCategoria: function () {
            for (let input of $(CreateView.Controles.formCategoria.inputs)) {
                $("#" + $(input).attr("id") + CreateView.Controles.prefijoLabelsHelp).css("display", "none");
                $(input).parent().removeClass("has-error");
            }
        },

        LimpiarValoresFormularioDetallesCategoria: function () {
            SeleccionarItemSelect2(CreateView.Controles.formCategoria.ddlCategoriaIndicador, "");
            SeleccionarItemSelect2(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador, "");
        },

        RestablecerCamposFormularioDetalleCategoria: function () {
            $(CreateView.Controles.formCategoria.ddlCategoriaIndicador).prop("disabled", false);
            $(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador).prop("disabled", false);
        },

        ValidarCantidadDetallesCategoria: function () {
            let cantidadEstablecida = $(CreateView.Controles.formIndicador.inputCantidadCategoriaIndicador).val() == "" ? 0 : $(CreateView.Controles.formIndicador.inputCantidadCategoriaIndicador).val();

            if (cantidadEstablecida > CreateView.Variables.cantidadDetallesCategoriaRegistrada) {
                $(CreateView.Controles.formCategoria.btnGuardar).prop("disabled", false);
                $(CreateView.Controles.btnFinalizar).prop("disabled", true);
            }
            else {
                $(CreateView.Controles.formCategoria.btnGuardar).prop("disabled", true);
                $(CreateView.Controles.btnFinalizar).prop("disabled", false);
            }
        },

        CancelarFormularioDetallesCategoria: function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    window.location.href = CreateView.Variables.indexViewURL;
                });
        },

        EliminarDetalleCategoria: function (pIdIndicador, pIdCategoria, pNombreCategoria) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminarCategoria, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) { resolve(true) });
            })
                .then(data => {
                    $("#loading").fadeIn();

                    let objDetalle = CreateView.Metodos.CrearObjDetallesCategoria(pIdIndicador, pIdCategoria);
                    objDetalle.NombreCategoria = pNombreCategoria;
                    return CreateView.Consultas.EliminarDetalleCategoria(objDetalle);
                })
                .then(data => {
                    if (pIdCategoria === CreateView.Variables.objEditarDetallesCategoria?.idCategoria) { // en caso de que se este editando, no actualicen y se presione el boton de eliminar
                        CreateView.Variables.objEditarDetallesCategoria = null;
                        this.LimpiarValoresFormularioDetallesCategoria();
                        this.RestablecerCamposFormularioDetalleCategoria();
                    }

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEliminarCategoria).set('onok', function (closeEvent) {
                        CreateView.Variables.hizoCargaDetallesCategorias = false;
                        CreateView.Metodos.CargarDetallesCategoria(pIdIndicador);
                    });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        EditarDetallesCategoria: function (pIdIndicador) {
            let formValido = this.ValidarFormularioDetallesCategoria();

            if (!formValido) { return; }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEditarCategoria, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.EditarDetalleCategoriasDesagregacion(
                        this.CrearObjDetallesCategoria(pIdIndicador, $(CreateView.Controles.formCategoria.ddlCategoriaIndicador).val())
                    );
                })
                .then(data => {
                    CreateView.Metodos.LimpiarValoresFormularioDetallesCategoria();
                    CreateView.Metodos.RestablecerCamposFormularioDetalleCategoria();
                    CreateView.Variables.objEditarDetallesCategoria = null;

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEditarCategoria)
                        .set('onok', function (closeEvent) {
                            CreateView.Variables.hizoCargaDetallesCategorias = false;
                            CreateView.Metodos.CargarDetallesCategoria(pIdIndicador);
                        });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearDetallesCategoria: function (pIdIndicador) {
            let formValido = this.ValidarFormularioDetallesCategoria();
            
            if (!formValido) { return; }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaAgregarCategoria, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.InsertarDetalleCategoriasDesagregacion(
                        this.CrearObjDetallesCategoria(pIdIndicador, $(CreateView.Controles.formCategoria.ddlCategoriaIndicador).val())
                    );
                })
                .then(data => {
                    this.LimpiarValoresFormularioDetallesCategoria();

                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoAgregarCategoria)
                        .set('onok', function (closeEvent) {
                            CreateView.Variables.hizoCargaDetallesCategorias = false;
                            CreateView.Metodos.CargarDetallesCategoria(pIdIndicador);
                        });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        HabilitarControlesTipoGrafico: function (selected) {
            if (selected == CreateView.Variables.salida || selected == CreateView.Variables.entradaSalida || selected == CreateView.Variables.entradaSalidaM) {
                $(CreateView.Controles.divTipoGrafico).removeClass("hidden");
            }
            else {
                
                $(CreateView.Controles.divTipoGrafico).addClass("hidden");
                SeleccionarItemSelect2(CreateView.Controles.formIndicador.ddlTipoGrafico, "");
                
            }
        }

        // ---
    },

    Consultas: {
        ConsultarListaTipoIndicador: function () {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaTipoIndicador', 'GET');
        },

        EliminarTipoIndicador: function (pIdTipoIndicador) {
            return execAjaxCall('/IndicadorFonatel/EliminarTipoIndicador', 'POST', { pIdTipoIndicador: pIdTipoIndicador });
        },

        ConsultarListaGrupoIndicador: function () {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaGrupoIndicador', 'GET');
        },

        EliminarGrupoIndicador: function (pIdGrupoIndicador) {
            return execAjaxCall('/IndicadorFonatel/EliminarGrupoIndicador', 'POST', { pIdGrupoIndicador: pIdGrupoIndicador });
        },

        ConsultarListaUnidadEstudio: function () {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaUnidadEstudio', 'GET');
        },

        EliminarUnidadEstudio: function (pIdUnidadEstudio) {
            return execAjaxCall('/IndicadorFonatel/EliminarUnidadEstudio', 'POST', { pIdUnidadEstudio: pIdUnidadEstudio });
        },

        CrearTipoIndicador: function (pNombre) {
            return execAjaxCall('/IndicadorFonatel/CrearTipoIndicador', 'POST', { pNombre: pNombre });
        },

        CrearGrupoIndicador: function (pNombre) {
            return execAjaxCall('/IndicadorFonatel/CrearGrupoIndicador', 'POST', { pNombre: pNombre });
        },

        CrearUnidadEstudio: function (pNombre) {
            return execAjaxCall('/IndicadorFonatel/CrearUnidadEstudio', 'POST', { pNombre: pNombre });
        },

        CrearIndicador: function (pIndicador) {
            return execAjaxCall('/IndicadorFonatel/CrearIndicador', 'POST', { pIndicador: pIndicador });
        },

        EditarIndicador: function (pIndicador) {
            return execAjaxCall('/IndicadorFonatel/EditarIndicador', 'POST', { pIndicador: pIndicador });
        },

        ClonarIndicador: function (pIndicador) {
            return execAjaxCall('/IndicadorFonatel/ClonarIndicador', 'POST', { pIndicador: pIndicador });
        },

        GuardadoDefinitivoIndicador: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/GuardadoDefinitivoIndicador', 'POST', { pIdIndicador: pIdIndicador });
        },

        ConsultarDetallesVariable: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaDetallesVariable', 'GET', { pIdIndicador: pIdIndicador });
        },

        ConsultarCategoriasDesagregacion: function () {
            return execAjaxCall('/IndicadorFonatel/ObtenerCategoriasDesagregacion', 'GET');
        },

        InsertarDetalleCategoriasDesagregacion: function (pDetalleIndicadorCategoria) {
            return execAjaxCall('/IndicadorFonatel/CrearDetalleCategoriaDesagregacion', 'POST', { pDetalleIndicadorCategoria: pDetalleIndicadorCategoria });
        },

        EditarDetalleCategoriasDesagregacion: function (pDetalleIndicadorCategoria) {
            return execAjaxCall('/IndicadorFonatel/EditarDetalleCategoriaDesagreagacion', 'POST', { pDetalleIndicadorCategoria: pDetalleIndicadorCategoria });
        },

        EliminarDetalleCategoria: function (pDetalleIndicadorCategoria) {
            return execAjaxCall('/IndicadorFonatel/EliminarDetalleCategoria', 'POST', { pDetalleIndicadorCategoria: pDetalleIndicadorCategoria });
        },

        ConsultarDetallesDeCategoriaDesagregacion: function (pIdCategoria) {
            return execAjaxCall('/IndicadorFonatel/ObtenerDetallesTipoTextoDeCategoriaDesagregacion', 'GET', { pIdCategoria: pIdCategoria });
        },

        ConsultarDetallesCategoria: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaDetallesCategoria', 'GET', { pIdIndicador: pIdIndicador });
        },

        ConsultarListaDetallesDeCategoriaGuardada: function (pIdIndicador, pIdCategoria) {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaDetallesDeCategoriaGuardada', 'GET', { pIdIndicador: pIdIndicador, pIdCategoria: pIdCategoria });
        },

        CrearDetalleVariableDato: function (pDetalleIndicadorVariables) {
            return execAjaxCall('/IndicadorFonatel/CrearDetalleVariableDato', 'POST', { pDetalleIndicadorVariables: pDetalleIndicadorVariables });
        },

        EditarDetalleVariableDato: function (pDetalleIndicadorVariables) {
            return execAjaxCall('/IndicadorFonatel/EditarDetalleVariableDato', 'POST', { pDetalleIndicadorVariables: pDetalleIndicadorVariables });
        },

        EliminarDetalleVariableDato: function (pDetalleIndicadorVariables) {
            return execAjaxCall('/IndicadorFonatel/EliminarDetalleVariableDato', 'POST', { pDetalleIndicadorVariables: pDetalleIndicadorVariables });
        },
    },

    Eventos: function () {
        // Formulario Indicador
        $(document).on("click", CreateView.Controles.formIndicador.btnSiguienteCrearIndicador, function (e) {

            CreateView.Variables.hizoCargaDetallesVariables = false;

            if (ObtenerValorParametroUrl("id") == null) {
                CreateView.Metodos.CrearIndicador();
            }
            else {
                CreateView.Metodos.EditarIndicador();
            }
        });

        //Habilitar el div de tipo grafico
        $(document).on("change", CreateView.Controles.formIndicador.ddlClasificacion, function () {
            
            var selected = $(this).find('option:selected').text();
           
            CreateView.Metodos.HabilitarControlesTipoGrafico(selected);
        });

        $(document).on("click", CreateView.Controles.formIndicador.btnGuardarCrearIndicador, function (e) {
            CreateView.Metodos.CrearIndicadorGuardadoParcial();
        });

        $(document).on("click", CreateView.Controles.formIndicador.btnSiguienteEditarIndicador, function (e) {

            CreateView.Variables.hizoCargaDetallesVariables = false;

            if (ObtenerValorParametroUrl("id") != null) {
                CreateView.Metodos.EditarIndicador();
            }
        });

        $(document).on("click", CreateView.Controles.formIndicador.btnGuardarEditarIndicador, function (e) {
            CreateView.Metodos.EditarIndicadorGuardadoParcial();
        });

        $(document).on("click", CreateView.Controles.formIndicador.btnSiguienteClonarIndicador, function (e) {

            CreateView.Variables.hizoCargaDetallesVariables = false;

            if (ObtenerValorParametroUrl("id") != null) {
                if (CreateView.Variables.elIndicadorFueClonado) {
                    CreateView.Metodos.EditarIndicador();
                }
                else {
                    CreateView.Metodos.ClonarIndicador();
                }
            }
        });

        $(document).on("click", CreateView.Controles.formIndicador.btnGuardarClonarIndicador, function (e) {
            CreateView.Metodos.ClonarIndicadorGuardadoParcial();
        });

        $(document).on("click", CreateView.Controles.formIndicador.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    window.location.href = CreateView.Variables.indexViewURL;
                });
        });

        $(CreateView.Controles.formIndicador.inputs).on("keyup", function (e) {
            let inputId = $(this).attr("id"); // evento adicional para los campos de cantidades
            if (inputId == $(CreateView.Controles.formIndicador.inputCantidadVariableDatosIndicador).attr("id") ||
                inputId == $(CreateView.Controles.formIndicador.inputCantidadCategoriaIndicador).attr("id")
            ) {
                if (parseInt($(this).val()) < 1)
                    $(this).val("");
            }

            let validacion = ValidarFormulario(CreateView.Controles.formIndicador.inputs);
            CreateView.Metodos.CambiarEstadoBtnSiguienteFormIndicador(!validacion.puedeContinuar);
        });

        $(CreateView.Controles.formIndicador.selects2).on('select2:select', function (e) {
            let validacion = ValidarFormulario(CreateView.Controles.formIndicador.inputs);
            CreateView.Metodos.CambiarEstadoBtnSiguienteFormIndicador(!validacion.puedeContinuar);
        });

        // Modals Formulario Indicador
        $(document).on("click", CreateView.Controles.btnModalTipoIndicador, function (e) {
            CreateView.Metodos.AbrirModalTipoIndicador();
        });

        $(document).on("click", CreateView.Controles.btnModalGrupoIndicador, function (e) {
            CreateView.Metodos.AbrirModalGrupoIndicador();
        });

        $(document).on("click", CreateView.Controles.btnModalUnidadEstudio, function (e) {
            CreateView.Metodos.AbrirModalUnidadEstudio();
        });

        $(document).on("click", CreateView.Controles.btnEliminarTipoIndicador, function (e) {
            CreateView.Metodos.EliminarTipoIndicador($(this).val());
        });

        $(document).on("click", CreateView.Controles.btnEliminarGrupoIndicador, function (e) {
            CreateView.Metodos.EliminarGrupoIndicador($(this).val());
        });

        $(document).on("click", CreateView.Controles.btnEliminarUnidadEstudio, function (e) {
            CreateView.Metodos.EliminarUnidadEstudio($(this).val());
        });

        $(document).on("click", CreateView.Controles.btnGuardarTipoIndicador, function (e) {
            CreateView.Metodos.CrearTipoIndicador($(CreateView.Controles.inputModalTipo).val());
        });

        $(document).on("click", CreateView.Controles.btnGuardarGrupoIndicador, function (e) {
            CreateView.Metodos.CrearGrupoIndicador($(CreateView.Controles.inputModalGrupo).val());
        });

        $(document).on("click", CreateView.Controles.btnGuardarUnidadEstudio, function (e) {
            CreateView.Metodos.CrearUnidadEstudio($(CreateView.Controles.inputModalUnidadEstudio).val());
        });

        // Formulario Detalles Variable
        $(document).on("click", CreateView.Controles.formVariable.btnSiguiente, function (e) {
            CreateView.Variables.hizoCargaDetallesCategorias = false;
            $(CreateView.Controles.step3Categoria).trigger('click');
        });

        $(document).on("click", CreateView.Controles.formVariable.btnCancelar, function (e) {
            CreateView.Metodos.CancelarFormularioDetallesVariable();
        });

        $(document).on("click", CreateView.Controles.formVariable.btnAtras, function (e) {
            $(CreateView.Controles.step1Indicador).trigger('click');
        });

        $(document).on("click", CreateView.Controles.formVariable.btnEditarVariable, function (e) {
            $(CreateView.Controles.formVariable.btnGuardar).prop("disabled", false);
            CreateView.Metodos.CargarFormularioEditarDetallesVariable($(this).val());
        });

        $(document).on("click", CreateView.Controles.formVariable.btnGuardar, function (e) {
            let idIndicador = ObtenerValorParametroUrl("id");

            if (idIndicador != null || $.trim(idIndicador) != "") {
                if (CreateView.Variables.objEditarDetallesVariableDato != null) {
                    CreateView.Metodos.EditarDetallesVariable(idIndicador);
                }
                else {
                    CreateView.Metodos.CrearDetallesVariables(idIndicador);
                }
            }
        });

        $(document).on("click", CreateView.Controles.formVariable.btnEliminarVariable, function () {
            let idIndicador = ObtenerValorParametroUrl("id");

            if (idIndicador != null || $.trim(idIndicador) != "") {
                CreateView.Metodos.EliminarDetallesVariable(idIndicador, $(this).val());
            }
        });

        // Formulario Detalles Categorias
        $(document).on("click", CreateView.Controles.formCategoria.btnCancelar, function (e) {
            CreateView.Metodos.CancelarFormularioDetallesCategoria();         
        });

        $(document).on("click", CreateView.Controles.formCategoria.btnAtras, function (e) {
            $(CreateView.Controles.step2Variable).trigger('click');
        });

        $(document).on("click", CreateView.Controles.formCategoria.btnEliminarCategoria, function () {
            let idIndicador = ObtenerValorParametroUrl("id");

            if (idIndicador != null || $.trim(idIndicador) != "") {
                let nombreCategoria = $(this).parent().parent().children().eq(1).html();
                CreateView.Metodos.EliminarDetalleCategoria(idIndicador, $(this).val(), nombreCategoria);
            }
        });

        $(CreateView.Controles.formCategoria.ddlCategoriaIndicador).on('select2:select', function (event) {
            let idCategoria = $(this).val();
            if (idCategoria != null || $.trim(idCategoria) != "") {
                CreateView.Metodos.CargarDetallesDeLaCategoria(idCategoria);
            }
        });

        $(document).on("click", CreateView.Controles.formCategoria.btnEditarCategoria, function (e) {
            let idCategoria = $(this).val();

            if (idCategoria != null || $.trim(idCategoria) != "") {
                let idIndicador = ObtenerValorParametroUrl("id");

                if (idIndicador != null || $.trim(idIndicador) != "") {
                    CreateView.Metodos.CargarFormularioEditarDetallesCategoria(idCategoria, idIndicador);
                }
            }
        });

        $(document).on("click", CreateView.Controles.formCategoria.btnGuardar, function (e) {
            let idIndicador = ObtenerValorParametroUrl("id");

            if (idIndicador != null || $.trim(idIndicador) != "") {
                if (CreateView.Variables.objEditarDetallesCategoria != null) {
                    CreateView.Metodos.EditarDetallesCategoria(idIndicador);
                }
                else {
                    CreateView.Metodos.CrearDetallesCategoria(idIndicador);
                }
            }
        });

        // ----
        
        $(document).on("click", CreateView.Controles.step2Variable, function (e) {
            let id = ObtenerValorParametroUrl("id");
            if (id != null || $.trim(id) != "") {
                CreateView.Metodos.CargarDetallesVariable(id);
            }
        });

        $(document).on("click", CreateView.Controles.step3Categoria, function (e) {
            let id = ObtenerValorParametroUrl("id");
            if (id != null || $.trim(id) != "") {
                CreateView.Metodos.CargarDetallesCategoria(id);
            }
        });

        $(document).on("click", CreateView.Controles.btnFinalizar, function (e) {
            let id = ObtenerValorParametroUrl("id");
            if (id != null || $.trim(id) != "") {
                CreateView.Metodos.GuardadoDefinitivoIndicador(id);
            }
        });
    },

    Init: function () {
        CreateView.Eventos();

        let modo = $(CreateView.Controles.modoFormulario).val();

        CreateView.Metodos.CambiarEstadoBtnSiguienteFormIndicador(true);

        if (jsUtilidades.Variables.Acciones.Editar == modo) {
            let validacion = ValidarFormulario(CreateView.Controles.formIndicador.inputs);
            CreateView.Metodos.CambiarEstadoBtnSiguienteFormIndicador(!validacion.puedeContinuar);
        }
    }
},


ViewIndicador = {
        "Controles": {
        "btnSiguienteVisualiza": "#btnSiguiente",
        "btnSiguienteVisualizaVariable": "#btnSiguienteVariable",
            "step1Indicador": "a[href='#step-1']",
            "step2Variable": "a[href='#step-2']",
            "step3Categoria": "a[href='#step-3']",
            "ViewIndicador": "#dad1f560",
            "btnCancelar": "#btnCancelarIndicador",
        "btnAtrasVariable": "#btnAtrasVariable",
        "btnAtrasCategoria":"#btnAtrasCategoria"
    },
    "Mensajes": {
        "preguntaCancelarAccion": "¿Desea cancelar la acción?",
    },
        Eventos: function () {
            $(document).on("click", ViewIndicador.Controles.btnSiguienteVisualiza, function (e) {
                $(ViewIndicador.Controles.step2Variable).click();
            });
            $(document).on("click", ViewIndicador.Controles.btnSiguienteVisualizaVariable, function (e) {
                $(ViewIndicador.Controles.step3Categoria).click();
            });
            $(document).on("click", ViewIndicador.Controles.btnAtrasVariable, function (e) {
                $(ViewIndicador.Controles.step1Indicador).click();
            });
            $(document).on("click", ViewIndicador.Controles.btnAtrasCategoria, function (e) {
                $(ViewIndicador.Controles.step2Variable).click();
            });
            $(document).on("click", ViewIndicador.Controles.btnCancelar, function (e) {
                jsMensajes.Metodos.ConfirmYesOrNoModal(ViewIndicador.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.cancelar)
                    .set('onok', function (closeEvent) {
                        window.location.href = CreateView.Variables.indexViewURL;
                    });
            });
        },
        Init: function () {
            ViewIndicador.Eventos();
        }
    }

$(function () {
    if ($(IndexView.Controles.IndexView).length > 0) {
        IndexView.Init();
    }

    if ($(CreateView.Controles.CreateView).length > 0) {
        CreateView.Init();
    }
    if ($(ViewIndicador.Controles.ViewIndicador).length > 0) {
        ViewIndicador.Init();
    }
});

$(function () {
    var selected = $(CreateView.Controles.formIndicador.ddlClasificacion).find('option:selected').text();

    if (selected != "entrada" || selected !="Entrada") {
        CreateView.Metodos.HabilitarControlesTipoGrafico(selected);
    }
});