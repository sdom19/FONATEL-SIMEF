IndexView = {
    Controles: {
        tablaIndicador: "#tableIndicador tbody",
        btnEditarIndicador: "#tableIndicador tbody tr td .btn-edit",
        btnDesactivarIndicador: "#tableIndicador tbody tr td .btn-power-on",
        btnActivarIndicador: "#tableIndicador tbody tr td .btn-power-off",
        btnEliminarIndicador: "#tableIndicador tbody tr td .btn-delete",
        btnClonarIndicador: "#tableIndicador tbody tr td .btn-clone",

        IndexView: "#dad1f1ea"
    },

    Variables: {
        indexViewURL: "/Fonatel/IndicadorFonatel/index",
        editViewURL: "/Fonatel/IndicadorFonatel/edit?id=",
        createViewURL: "/Fonatel/IndicadorFonatel/create?id=",
        cloneViewURL: "/Fonatel/IndicadorFonatel/clone?id="
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
                html += "<td>"
                html += `<button class="btn-icon-base btn-edit" type="button" data-toggle="tooltip" data-placement="top" title="Editar" value=${item.id}></button>`
                html += `<button class="btn-icon-base btn-clone" type="button" data-toggle="tooltip" data-placement="top" title="Clonar" value=${item.id}></button>`

                if (item.EstadoRegistro.Nombre == "Activo") {
                    html += `<button class="btn-icon-base btn-power-on" type="button" data-toggle="tooltip" data-placement="top" title="Desactivar" value=${item.id}></button>`
                }
                else {
                    html += `<button class="btn-icon-base btn-power-off" type="button" data-toggle="tooltip" data-placement="top" title="Activar" value=${item.id}></button>`
                }

                html += `<button class="btn-icon-base btn-delete" type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" value=${ item.id }></button>`
                html += "</td>"
                html += "</tr>";
            });
            $(IndexView.Controles.tablaIndicador).html(html);
            CargarDatasource();
        },

        EliminarIndicador: async function (pIdIndicador) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Indicador?", jsMensajes.Variables.actionType.eliminar)
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
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Indicador ya está en uso en el/los<br>" + dependencias + "<br>¿Desea eliminarlo?", jsMensajes.Variables.actionType.eliminar)
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
                        jsMensajes.Metodos.OkAlertModal("El Indicador ha sido eliminado")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .then(data => {
                    window.location.href = IndexView.Variables.indexViewURL
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        DesactivarIndicador: async function (pIdIndicador) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar el Indicador?", jsMensajes.Variables.actionType.estado)
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
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Indicador ya está en uso en el/los<br>" + dependencias + "<br>¿Desea desactivarlo?", jsMensajes.Variables.actionType.estado)
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
                        jsMensajes.Metodos.OkAlertModal("El Indicador ha sido desactivado")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .then(data => {
                    window.location.href = IndexView.Variables.indexViewURL
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        ActivarIndicador: async function (pIdIndicador) {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar el Indicador?", jsMensajes.Variables.actionType.estado)
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
                        jsMensajes.Metodos.OkAlertModal("El Indicador ha sido activado")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .then(data => {
                    window.location.href = IndexView.Variables.indexViewURL
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
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
            window.location.href = IndexView.Variables.editViewURL + $(this).val();
        });

        $(document).on("click", IndexView.Controles.btnClonarIndicador, function () {
            new Promise((resolve, rejected) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar el Indicador?", jsMensajes.Variables.actionType.clonar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    window.location.href = IndexView.Variables.cloneViewURL + $(this).val();
                });
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

        // ============ Formularios ============
        formIndicador: {
            form: "#formCrearIndicador",
            btnSiguiente: "#btnSiguienteIndicador",
            inputs: "#formCrearIndicador input, #formCrearIndicador textarea, #formCrearIndicador select",
            selects2: "#formCrearIndicador select",
            btnGuardar: "#btnGuardarIndicador",

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
        },

        formVariable: {
            form: "#formCrearVariable",
            btnSiguiente: "#btnSiguienteVariable",
            btnAtras: "#btnAtrasVariable",
            inputs: "#formCrearVariable input, #formCrearVariable textarea, #formCrearVariable select",
            tablaDetallesVariable: "#tableDetallesVariable",
            tablaDetallesVariable_tbody: "#tableDetallesVariable tbody",
            btnEditarVariable: "#tableDetallesVariable tbody tr td .btn-edit",
            btnEliminarVariable: "#tableDetallesVariable tbody tr td .btn-delete",

            inputNombreVariable: "#inputNombreVariable",
            inputDescripcionVariable: "#inputDescripcionVariable",
        },

        formCategoria: {
            form: "#formCrearCategoria",
            btnSiguiente: "#btnSiguienteCategoria",
            btnAtras: "#btnAtrasCategoria",
            tablaDetallesCategoria: "#tableDetallesCategoria",
            tablaDetallesCategoria_tbody: "#tableDetallesCategoria tbody",
            btnEditarCategoria: "#tableDetallesCategoria tbody tr td .btn-edit",
            btnEliminarCategoria: "#tableDetallesCategoria tbody tr td .btn-delete",

            ddlCategoriaIndicador: "#ddlCategoriaIndicador",
            ddlCategoriaDetalleIndicador: "#ddlCategoriaDetalleIndicador"
        },

        //btnstep: ".step_navigation_indicador div",
        //divContenedor: ".stepwizard-content-container",
        //btnEliminarCategoria: "#TablaDetalleCategoriaIndicador tbody tr td .btn-delete",
        //btnAddIndicadorVariable: "#TableIndicador tbody tr td .variable",
        //btnAddIndicadorCategoria: "#TableIndicador tbody tr td .Categoría",
        
        btnFinalizar: "#btnFinalizarIndicador",
        btnGuardarVariable: "#btnGuardarVariable",
        btnGuardarCategoria: "#btnGuardarCategoria",
        btnCancelar: "#btnCancelarIndicador",
        step1Indicador: "a[href='#step-1']",
        step2Variable: "a[href='#step-2']",
        step3Categoria: "a[href='#step-3']",
        CreateView: "#dad1f550"
    },

    Variables: {
        indexViewURL: "/Fonatel/IndicadorFonatel/index",
        btnEdit: (pValue) => `<button class="btn-icon-base btn-edit" type="button" data-toggle="tooltip" data-placement="top" title="Editar" value=${pValue}></button>`,
        btnDelete: (pValue) => `<button class="btn-icon-base btn-delete" type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" value=${pValue}></button>`,

        hizoCargaDetallesVariables: false,
        hizoCargaDetallesCategorias: false,
        listaVariablesDato: {},
        objEditarDetallesVariableDato: null,
        objEditarDetallesCategoria: null
    },

    Metodos: {
        ValidarFormulario: function (pInputs) {
            let inputsPendientesCompletar = [...$(pInputs)].filter(i => { return $.trim(i.value) == "" || i.value == null; });
            return { puedeContinuar: inputsPendientesCompletar.length == 0 ? true : false, objetos: inputsPendientesCompletar };
        },

        // Formulario Indicador
        CrearObjFormularioIndicador: function (esGuardadoParcial) {
            let controles = CreateView.Controles.formIndicador;

            var formData = {
                Codigo: $(controles.inputCodigo).val(),
                Nombre: $(controles.inputNombre).val(),
                Descripcion: $(controles.inputDescripcion).val(),
                CantidadVariableDato: $(controles.inputCantidadVariableDatosIndicador).val(),
                CantidadCategoriasDesagregacion: $(controles.inputCantidadCategoriaIndicador).val(),
                Interno: $(controles.ddlUsoIndicador).val(),
                Solicitud: $(controles.ddlUsoSolicitud).val(),
                Fuente: $(controles.inputFuenteIndicador).val(),
                Notas: $(controles.inputNota).val(),
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
                    id: $(controles.ddlClasificacion).val()
                },
                esGuardadoParcial: esGuardadoParcial ? true : false
            };
            return formData;
        },

        CrearIndicador: function () {
            if (this.ValidarFormulario(CreateView.Controles.formIndicador.inputs).puedeContinuar) {
                $("#loading").fadeIn();
                CreateView.Consultas.CrearIndicador(this.CrearObjFormularioIndicador(false))
                    .then(data => {
                        InsertarParametroUrl("id", data.objetoRespuesta[0].id);
                        $(CreateView.Controles.step2Variable).trigger('click'); // cargar los respectivos datos

                        jsMensajes.Metodos.OkAlertModal("El Indicador ha sido creado").set('onok', function (closeEvent) { });
                    })
                    .catch(error => {
                        if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                            jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                        }
                    })
                    .finally(() => {
                        $("#loading").fadeOut();
                    });
            }
        },

        CrearIndicadorGuardadoParcial: function () {
            let mensaje = "";
            let prefijoHelp = CreateView.Controles.prefijoLabelsHelp;
            let validacionFormulario = this.ValidarFormulario(CreateView.Controles.formIndicador.inputs);
            let camposObligatoriosPendientes = false;

            $(CreateView.Controles.formIndicador.inputCodigo + prefijoHelp).css("display", "none");
            $(CreateView.Controles.formIndicador.inputNombre + prefijoHelp).css("display", "none");

            if (validacionFormulario.objetos.some(x => $(x).attr("id") === $(CreateView.Controles.formIndicador.inputCodigo).attr("id"))) {
                $(CreateView.Controles.formIndicador.inputCodigo + prefijoHelp).css("display", "block");
                camposObligatoriosPendientes = true;
            }
            if (validacionFormulario.objetos.some(x => $(x).attr("id") === $(CreateView.Controles.formIndicador.inputNombre).attr("id"))) {
                $(CreateView.Controles.formIndicador.inputNombre + prefijoHelp).css("display", "block");
                camposObligatoriosPendientes = true;
            }

            if (camposObligatoriosPendientes) {
                return;
            }

            if (!validacionFormulario.puedeContinuar) {
                mensaje = "Existen campos vacíos. ";
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje + "¿Desea realizar un guardado parcial del Indicador?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function () { resolve(true); })
                    .set("oncancel", function () { })
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.CrearIndicador(this.CrearObjFormularioIndicador(true));
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal("El Indicador ha sido creado")
                        .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL; });
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
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
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Tipo Indicador?", jsMensajes.Variables.actionType.eliminar)
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
                        jsMensajes.Metodos.OkAlertModal("El Tipo Indicador ha sido eliminado")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearTipoIndicador: function (pNombre) {
            if ($.trim(pNombre).length <= 0) {
                $(CreateView.Controles.inputModalTipoHelp).css("display", "block");
                return;
            }
            $(CreateView.Controles.inputModalTipoHelp).css("display", "none");
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

                    jsMensajes.Metodos.OkAlertModal("El tipo ha sido agregado")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
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
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Grupo Indicador?", jsMensajes.Variables.actionType.eliminar)
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
                        jsMensajes.Metodos.OkAlertModal("El Grupo Indicador ha sido eliminado")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearGrupoIndicador: function (pNombre) {
            if ($.trim(pNombre).length <= 0) {
                $(CreateView.Controles.inputModalGrupoHelp).css("display", "block");
                return;
            }
            $(CreateView.Controles.inputModalGrupoHelp).css("display", "none");
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

                    jsMensajes.Metodos.OkAlertModal("El grupo ha sido agregado")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
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
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Unidad de Estudio?", jsMensajes.Variables.actionType.eliminar)
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
                        jsMensajes.Metodos.OkAlertModal("La Unidad de Estudio ha sido eliminada")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearUnidadEstudio: function (pNombre) {
            if ($.trim(pNombre).length <= 0) {
                $(CreateView.Controles.inputModalUnidadEstudioHelp).css("display", "block");
                return;
            }
            $(CreateView.Controles.inputModalUnidadEstudioHelp).css("display", "none");
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

                    jsMensajes.Metodos.OkAlertModal("La unidad de estudio ha sido agregada")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
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
                        this.InsertarDatosTablaDetallesVariable(data.objetoRespuesta);
                        // crear un objeto donde cada item es un map que contiene los datos la variable-dato
                        CreateView.Variables.listaVariablesDato = data.objetoRespuesta.reduce((map, obj) => (map[obj.id] = obj, map), {});
                        CreateView.Variables.hizoCargaDetallesVariables = true;
                    })
                    .catch(error => {
                        if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                            jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                        }
                    })
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
            var formData = {
                id: pIdDetalle,
                idIndicadorString: pIndicador,
                NombreVariable: $(controles.inputNombreVariable).val(),
                Descripcion: $(controles.inputDescripcionVariable).val()
            };
            return formData;
        },

        ValidarFormularioDetallesVariable: function () {
            this.LimpiarMensajesValidacionFormularioDetallesVariable();

            let validacion = this.ValidarFormulario(CreateView.Controles.formVariable.inputs);

            for (let input of validacion.objetos) {
                $("#" + $(input).attr("id") + CreateView.Controles.prefijoLabelsHelp).css("display", "block");
                $("#" + $(input).attr("id")).parent().addClass("has-error");
            }
            return validacion.puedeContinuar;
        },

        LimpiarMensajesValidacionFormularioDetallesVariable: function () {
            $(CreateView.Controles.formVariable.inputNombreVariable + CreateView.Controles.prefijoLabelsHelp).css("display", "none");
            $(CreateView.Controles.formVariable.inputDescripcionVariable + CreateView.Controles.prefijoLabelsHelp).css("display", "none");
            $(CreateView.Controles.formVariable.inputNombreVariable).parent().removeClass("has-error");
            $(CreateView.Controles.formVariable.inputDescripcionVariable).parent().removeClass("has-error");
        },

        EditarDetallesVariable: function (pIdIndicador) {
            let formValido = this.ValidarFormularioDetallesVariable();

            if (!formValido) {
                return;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Variable?", jsMensajes.Variables.actionType.agregar)
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
                    $(CreateView.Controles.formVariable.inputNombreVariable).val(null);
                    $(CreateView.Controles.formVariable.inputDescripcionVariable).val(null);
                    CreateView.Variables.objEditarDetallesVariableDato = null;

                    CreateView.Variables.listaVariablesDato[data.objetoRespuesta[0].id] = data.objetoRespuesta[0]; // guardar el item localmente

                    let item = [ // crear el item que se inserta en la tabla
                        data.objetoRespuesta[0].NombreVariable,
                        data.objetoRespuesta[0].Descripcion,
                        CreateView.Variables.btnEdit(data.objetoRespuesta[0].id) + CreateView.Variables.btnDelete(data.objetoRespuesta[0].id)
                    ];

                    //InsertarItemDataTable(CreateView.Controles.formVariable.tablaDetallesVariable, item);

                    jsMensajes.Metodos.OkAlertModal("La Variable ha sido agregada")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CrearDetallesVariables: function (pIdIndicador) {
            let formValido = this.ValidarFormularioDetallesVariable();

            if (!formValido) {
                return;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Variable?", jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CreateView.Consultas.CrearDetalleVariableDato(this.CrearObjDetallesVariable(pIdIndicador));
                })
                .then(data => {
                    $(CreateView.Controles.formVariable.inputNombreVariable).val(null);
                    $(CreateView.Controles.formVariable.inputDescripcionVariable).val(null);

                    CreateView.Variables.listaVariablesDato[data.objetoRespuesta[0].id] = data.objetoRespuesta[0]; // guardar el item localmente

                    let item = [ // crear el item que se inserta en la tabla
                        data.objetoRespuesta[0].NombreVariable,
                        data.objetoRespuesta[0].Descripcion,
                        CreateView.Variables.btnEdit(data.objetoRespuesta[0].id) + CreateView.Variables.btnDelete(data.objetoRespuesta[0].id)
                    ];

                    InsertarItemDataTable(CreateView.Controles.formVariable.tablaDetallesVariable, item);

                    jsMensajes.Metodos.OkAlertModal("La Variable ha sido agregada")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        // Formulario Detalles Categorias
        CargarDetallesCategoria: function (pIndicador) { // combo categorias y tabla
            if (!CreateView.Variables.hizoCargaDetallesCategorias) {
                $("#loading").fadeIn();

                CreateView.Consultas.ConsultarCategoriasDesagregacionTipoAtributo() // combo
                    .then(data => {
                        let dataSet = []
                        data.objetoRespuesta?.forEach(item => {
                            dataSet.push({ value: item.id, text: item.NombreCategoria });
                        });
                        InsertarDataSetSelect2(CreateView.Controles.formCategoria.ddlCategoriaIndicador, dataSet);
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
                            this.InsertarDatosTablaDetallesCategoria(data.objetoRespuesta);
                        }
                        CreateView.Variables.hizoCargaDetallesCategorias = true;
                    })
                    .catch(error => {
                        if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                            jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                        }
                        else {
                            jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                        }
                    })
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
                        InsertarOpcionTodosSelect2Multiple(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador);
                    }

                    InsertarDataSetSelect2(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador, dataSet);
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
                        CreateView.Variables.objEditarDetallesCategoria = data.objetoRespuesta;

                        SeleccionarItemsSelect2Multiple(
                            CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador,
                            data.objetoRespuesta, "idCategoriaDetalleString");
                    }
                })
                .catch(error => {
                    console.log(error)
                    if (error?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError).set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CargarFormularioEditarDetallesCategoria: function (pIdCategoria, pIdIndicador) {
            SeleccionarItemSelect2(CreateView.Controles.formCategoria.ddlCategoriaIndicador, pIdCategoria);
            this.CargarDetallesDeLaCategoria(pIdCategoria, pIdIndicador);
        },

        EditarDetallesCategoria: function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Categoría?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal("La Categoría ha sido agregada")
                        .set('onok', function (closeEvent) {
                            $(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador).empty();
                            SeleccionarItemSelect2(CreateView.Controles.formCategoria.ddlCategoriaIndicador, "");
                        });
                });
        },

        CrearDetallesCategoria: function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Categoría?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal("La Categoría ha sido agregada")
                        .set('onok', function (closeEvent) {
                            $(CreateView.Controles.formCategoria.ddlCategoriaDetalleIndicador).empty();
                            SeleccionarItemSelect2(CreateView.Controles.formCategoria.ddlCategoriaIndicador, "");
                        });
                });
        }
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

        ConsultarDetallesVariable: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaDetallesVariable', 'GET', { pIdIndicador: pIdIndicador });
        },

        ConsultarCategoriasDesagregacionTipoAtributo: function () {
            return execAjaxCall('/IndicadorFonatel/ObtenerCategoriasDesagregacionTipoAtributo', 'GET');
        },

        ConsultarDetallesDeCategoriaDesagregacion: function (pIdCategoria) {
            return execAjaxCall('/IndicadorFonatel/ObtenerDetallesDeCategoriaDesagregacion', 'GET', { pIdCategoria: pIdCategoria });
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
        }
    },

    Eventos: function () {
        $(document).on("click", CreateView.Controles.formIndicador.btnSiguiente, function (e) {
            CreateView.Metodos.CrearIndicador();
        });

        $(document).on("click", CreateView.Controles.formVariable.btnSiguiente, function (e) {
            $(CreateView.Controles.step3Categoria).trigger('click');
        });

        $(document).on("click", CreateView.Controles.formVariable.btnAtras, function (e) {
            $(CreateView.Controles.step1Indicador).trigger('click');
        });

        $(document).on("click", CreateView.Controles.formCategoria.btnAtras, function (e) {
            $(CreateView.Controles.step2Variable).trigger('click');
        });

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

        $(CreateView.Controles.formIndicador.inputs).on("keyup", function (e) {
            let validacion = CreateView.Metodos.ValidarFormulario(CreateView.Controles.formIndicador.inputs);
            $(CreateView.Controles.formIndicador.btnSiguiente).prop('disabled', !validacion.puedeContinuar);
        });

        $(CreateView.Controles.formIndicador.selects2).on('select2:select', function (e) {
            let validacion = CreateView.Metodos.ValidarFormulario(CreateView.Controles.formIndicador.inputs);
            $(CreateView.Controles.formIndicador.btnSiguiente).prop('disabled', !validacion.puedeContinuar);
        });

        $(document).on("click", CreateView.Controles.formIndicador.btnGuardar, function (e) {
            CreateView.Metodos.CrearIndicadorGuardadoParcial();
        });

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
            else {
                CreateView.Metodos.CargarDetallesCategoria(null);
            }
        });

        $(document).on("click", CreateView.Controles.formVariable.btnEditarVariable, function (e) {
            CreateView.Metodos.CargarFormularioEditarDetallesVariable($(this).val());
        });

        $(document).on("click", CreateView.Controles.btnGuardarVariable, function (e) {
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

        $(document).on("click", CreateView.Controles.btnGuardarCategoria, function (e) {
            if (CreateView.Variables.objEditarDetallesCategoria != null) {
                CreateView.Metodos.EditarDetallesCategoria();
            }
            else {
                CreateView.Metodos.CrearDetallesCategoria();
            }
        });

        $(document).on("click", CreateView.Controles.btnCancelar, function (e) {
            e.preventDefault();
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    window.location.href = CreateView.Variables.indexViewURL;
                });
        });

        $(document).on("click", CreateView.Controles.btnFinalizar, function (e) {
            e.preventDefault();
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Indicador?", jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal("El Indicador ha sido agregado")
                        .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL });
                });
        });

        //$(document).on("click", CreateView.Controles.btnAddIndicadorCategoria, function () {
        //    let id = 1;
        //    window.location.href = "/Fonatel/IndicadorFonatel/DetalleCategoría?id=" + id;
        //});

        //$(document).on("click", CreateView.Controles.btnAddIndicadorVariable, function () {
        //    let id = 1;
        //    window.location.href = "/Fonatel/IndicadorFonatel/DetalleVariables?id=" + id;
        //});

        //$(document).on("click", CreateView.Controles.btnEliminarVariable, function () {
        //    let id = 1;
        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Variable?", jsMensajes.Variables.actionType.eliminar)
        //        .set('onok', function (closeEvent) {
        //            jsMensajes.Metodos.OkAlertModal("La Variable ha sido eliminada")
        //                .set('onok', function (closeEvent) { });
        //        });
        //});

        //$(document).on("click", CreateView.Controles.btnEliminarCategoria, function () {
        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Categoría?", jsMensajes.Variables.actionType.eliminar)
        //        .set('onok', function (closeEvent) {
        //            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido eliminada")
        //                .set('onok', function (closeEvent) { });
        //        });
        //});

        //$(document).on("click", CreateView.Controles.formCategoria.btnSiguiente, function (e) {
        //    e.preventDefault();
        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Indicador?", jsMensajes.Variables.actionType.estado)
        //        .set('onok', function (closeEvent) {
        //            jsMensajes.Metodos.OkAlertModal("La Indicador ha sido agregado")
        //                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        //        });
        //});

    },

    Init: function () {
        CreateView.Eventos();

        let modo = $(CreateView.Controles.modoFormulario).val();
        console.log(modo);

        if (jsUtilidades.Variables.Acciones.Editar == modo) {
            let validacion = CreateView.Metodos.ValidarFormulario(CreateView.Controles.formIndicador.inputs);
            $(CreateView.Controles.formIndicador.btnSiguiente).prop('disabled', !validacion.puedeContinuar);
        }
        else if (jsUtilidades.Variables.Acciones.Clonar == modo) {
            console.log("Clonando");
        }
    }
}

$(function () {
    if ($(IndexView.Controles.IndexView).length > 0) {
        IndexView.Init(); console.log("indexView");
    }

    if ($(CreateView.Controles.CreateView).length > 0) {
        CreateView.Init(); console.log("createView");
    }
});