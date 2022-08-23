IndexView = {
    Controles: {
        tablaIndicador: "#tableIndicador tbody",
        btnEditarIndicador: "#tableIndicador tbody tr td .btn-edit",
        btnDesactivarIndicador: "#tableIndicador tbody tr td .btn-power-off",
        btnActivarIndicador: "#tableIndicador tbody tr td .btn-power-on",
        btnEliminarIndicador: "#tableIndicador tbody tr td .btn-delete",
        btnClonarIndicador: "#tableIndicador tbody tr td .btn-clone",

        IndexView: "#dad1f1ea"
    },

    Variables: {

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

            listaIndicadores.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${ item.Codigo }</th>`;
                html += `<th scope='row'>${ item.Nombre }</th>`;
                html += `<th scope='row'>${ item.GrupoIndicadores.Nombre }</th>`;
                html += `<th scope='row'>${ item.TipoIndicadores.Nombre }</th>`;
                html += `<th scope='row'>${ item.EstadoRegistro.Nombre }</th>`;
                html += "<td>"
                html += `<button class="btn-icon-base btn-edit" type="button" data-toggle="tooltip" data-placement="top" title="Editar" value=${ item.id }></button>`
                html += `<button class="btn-icon-base btn-clone" type="button" data-toggle="tooltip" data-placement="top" title="Clonar" value=${item.id}></button>`

                if (item.EstadoRegistro.Nombre == "Activo") {
                    html += `<button class="btn-icon-base btn-power-off" type="button" data-toggle="tooltip" data-placement="top" title="Desactivar" value=${item.id}></button>`
                }
                else {
                    html += `<button class="btn-icon-base btn-power-on" type="button" data-toggle="tooltip" data-placement="top" title="Activar" value=${item.id}></button>`
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
                    return IndexView.Consultas.VerificarIndicadorEnFormularioWeb(pIdIndicador);
                })
                .then(data => {
                    if (data.CantidadRegistros > 0 && data.objetoRespuesta != null) {
                        let formularios = ConcatenarItems(data.objetoRespuesta, "Nombre");

                        $("#loading").fadeOut();
                        return new Promise((resolve, reject) => {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Indicador ya está en uso en el/los formularios: " + formularios + " ¿Desea eliminarlo?", jsMensajes.Variables.actionType.eliminar)
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
                    window.location.href = "/Fonatel/IndicadorFonatel/index"
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
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
                    return IndexView.Consultas.VerificarIndicadorEnFormularioWeb(pIdIndicador);
                })
                .then(data => {
                    if (data.CantidadRegistros > 0 && data.objetoRespuesta != null) {
                        let formularios = ConcatenarItems(data.objetoRespuesta, "Nombre");

                        $("#loading").fadeOut();
                        return new Promise((resolve, reject) => {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Indicador ya está en uso en el/los formularios: " + formularios + " ¿Desea desactivarlo?", jsMensajes.Variables.actionType.estado)
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
                    window.location.href = "/Fonatel/IndicadorFonatel/index"
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
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
                    window.location.href = "/Fonatel/IndicadorFonatel/index"
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
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

        VerificarIndicadorEnFormularioWeb: function (pIdIndicador) {
            return execAjaxCall('/IndicadorFonatel/VerificarIndicadorEnFormularioWeb?pIdIndicador=' + pIdIndicador, 'GET');
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
            let id = $(this).val();
            window.location.href = "/Fonatel/IndicadorFonatel/Create?id=" + id;
        });

        $(document).on("click", IndexView.Controles.btnClonarIndicador, function () {
            let id = 1;
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar el Indicador?", jsMensajes.Variables.actionType.clonar)
                .set('onok', function (closeEvent) {
                     window.location.href = "/Fonatel/IndicadorFonatel/Create?id="+id
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
        btnSiguienteIndicador: "#btnSiguienteIndicador",
        btnSiguienteCategoria: "#btnSiguienteCategoria",
        btnSiguienteVariable: "#btnSiguienteVariable",
        btnAtrasVariable: "#btnAtrasVariable",
        btnAtrasCategoria: "#btnAtrasCategoria",

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


        //btnstep: ".step_navigation_indicador div",
        //divContenedor: ".stepwizard-content-container",
        //btnGuardarIndicador: "#btnGuardarIndicador",
        //btnFinalizar: "#btnFinalizarIndicador",
        //btnGuardarVariable: "#btnGuardarVariable",
        //btnGuardarCategoría: "#btnGuardarCategoría",

        //btnEliminarCategoria: "#TablaDetalleCategoriaIndicador tbody tr td .btn-delete",
        //btnAddIndicadorVariable: "#TableIndicador tbody tr td .variable",
        //btnAddIndicadorCategoria: "#TableIndicador tbody tr td .Categoría",
        //btnEliminarVariable: "#TableDetalleVariable tbody tr td .btn-delete",
        //btnCancelar: "#btnCancelarIndicador",

        CreateView: "#dad1f550"
    },

    Variables: {
        btnDeleteModal: (pValue) => `<button class="btn-icon-base btn-delete" type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" value=${pValue}></button>`
    },

    Metodos: {
        AbrirModalTipoIndicador: function () {
            $("#loading").fadeIn();

            CreateView.Consultas.ConsultarListaTipoIndicador()
                .then(data => {
                    CreateView.Metodos.InsertarDatosTablaModalTipoIndicador(data.objetoRespuesta);
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

            pListaTipoIndicador.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${item.Nombre}</th>`;
                html += "<td>"
                html += CreateView.Variables.btnDeleteModal(item.id)
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
                    CreateView.Metodos.RemoverItemDataTable(CreateView.Controles.tableModalTipoIndicador, `button[value='${pIdTipoIndicador}']`);

                    $("#loading").fadeOut();

                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal("El Tipo Indicador ha sido eliminado")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        AbrirModalGrupoIndicador: function () {
            $("#loading").fadeIn();

            CreateView.Consultas.ConsultarListaGrupoIndicador()
                .then(data => {
                    CreateView.Metodos.InsertarDatosTablaModalGrupoIndicador(data.objetoRespuesta);
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

            pListaGrupoIndicador.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${item.Nombre}</th>`;
                html += "<td>"
                html += CreateView.Variables.btnDeleteModal(item.id)
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
                    CreateView.Metodos.RemoverItemDataTable(CreateView.Controles.tableModalGrupoIndicador, `button[value='${pIdGrupoIndicador}']`);

                    $("#loading").fadeOut();

                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal("El Grupo Indicador ha sido eliminado")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

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

            pListaUnidadEstudio.forEach(item => {
                html += "<tr>";
                html += `<th scope='row'>${item.Nombre}</th>`;
                html += "<td>"
                html += CreateView.Variables.btnDeleteModal(item.id)
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
                    CreateView.Metodos.RemoverItemDataTable(CreateView.Controles.tableModalUnidadEstudio, `button[value='${pIdUnidadEstudio}']`);

                    $("#loading").fadeOut();

                    return new Promise((resolve, reject) => {
                        jsMensajes.Metodos.OkAlertModal("La Unidad de Estudio ha sido eliminada")
                            .set('onok', function (closeEvent) {
                                resolve(true);
                            });
                    })
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        RemoverItemDataTable: function (pDataTable, pItem) {
            $(pDataTable).DataTable().row($(pItem).parents('tr')).remove().draw();
        },

        InsertarItemDataTable: function (pDataTable, pListaItems) {
            $(pDataTable).DataTable().row.add(pListaItems).draw(false);
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
                    $(inputModalTipo).val(null);

                    CreateView.Metodos.InsertarItemDataTable(
                        CreateView.Controles.tableModalTipoIndicador,
                        [data.objetoRespuesta[0].Nombre, CreateView.Variables.btnDeleteModal(data.objetoRespuesta[0].id)]);

                    jsMensajes.Metodos.OkAlertModal("El tipo ha sido agregado")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
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
                    $(inputModalGrupo).val(null);

                    CreateView.Metodos.InsertarItemDataTable(
                        CreateView.Controles.tableModalGrupoIndicador,
                        [data.objetoRespuesta[0].Nombre, CreateView.Variables.btnDeleteModal(data.objetoRespuesta[0].id)]);

                    jsMensajes.Metodos.OkAlertModal("El grupo ha sido agregado")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
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
                    $(inputModalUnidadEstudio).val(null);

                    CreateView.Metodos.InsertarItemDataTable(
                        CreateView.Controles.tableModalUnidadEstudio,
                        [data.objetoRespuesta[0].Nombre, CreateView.Variables.btnDeleteModal(data.objetoRespuesta[0].id)]);

                    jsMensajes.Metodos.OkAlertModal("La unidad de estudio ha sido agregada")
                        .set('onok', function (closeEvent) { });
                })
                .catch(error => {
                    if (error.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(error.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                })
                .finally(() => {
                    $("#loading").fadeOut();
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
        }
    },

    Eventos: function () {
        $(document).on("click", CreateView.Controles.btnSiguienteIndicador, function (e) {
            e.preventDefault();
            $("a[href='#step-2']").trigger('click');
        });

        $(document).on("click", CreateView.Controles.btnSiguienteVariable, function (e) {
            e.preventDefault();
            $("a[href='#step-3']").trigger('click');
        });

        $(document).on("click", CreateView.Controles.btnAtrasVariable, function (e) {
            e.preventDefault();
            $("a[href='#step-1']").trigger('click');
        });

        $(document).on("click", CreateView.Controles.btnAtrasCategoria, function (e) {
            e.preventDefault();
            $("a[href='#step-2']").trigger('click');
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

        //$(document).on("click", CreateView.Controles.btnCancelar, function (e) {
        //    e.preventDefault();
        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        //        .set('onok', function (closeEvent) {
        //            window.location.href = "/Fonatel/IndicadorFonatel/Index";
        //        });
        //});

        //$(document).on("click", CreateView.Controles.btnFinalizar, function (e) {
        //    e.preventDefault();
        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Indicador?", jsMensajes.Variables.actionType.cancelar)
        //        .set('onok', function (closeEvent) {
        //            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido agregado")
        //                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        //        });
        //});

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

        //$(document).on("click", CreateView.Controles.btnGuardarIndicador, function (e) {
        //    e.preventDefault();
        //    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial del Indicador?", jsMensajes.Variables.actionType.agregar)
        //        .set('onok', function (closeEvent) {

        //            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido creado")
        //                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click'); });

        //        });
        //});

        //$(document).on("click", CreateView.Controles.btnSiguienteCategoria, function (e) {
        //    e.preventDefault();
        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Indicador?", jsMensajes.Variables.actionType.estado)
        //        .set('onok', function (closeEvent) {
        //            jsMensajes.Metodos.OkAlertModal("La Indicador ha sido agregado")
        //                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        //        });
        //});

        

        //$(document).on("click", CreateView.Controles.btnGuardarVariable, function (e) {
        //    e.preventDefault();

        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Variable?", jsMensajes.Variables.actionType.agregar)
        //        .set('onok', function (closeEvent) {
        //            jsMensajes.Metodos.OkAlertModal("La Variable ha sido agregada")
        //                .set('onok', function (closeEvent) { });
        //        });
        //});

        //$(document).on("click", CreateView.Controles.btnGuardarCategoría, function (e) {
        //    e.preventDefault();

        //    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Categoría?", jsMensajes.Variables.actionType.agregar)
        //        .set('onok', function (closeEvent) {
        //            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido agregada")
        //                .set('onok', function (closeEvent) { });
        //        });
        //});
    },

    Init: function () {
        CreateView.Eventos();
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
