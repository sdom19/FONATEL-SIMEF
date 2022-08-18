IndexView = {
    Controles: {
        tablaIndicador: "#TableIndicador tbody",
        btnEditarIndicador: "#TableIndicador tbody tr td .btn-edit",
        btnDesactivarIndicador: "#TableIndicador tbody tr td .btn-power-off",
        btnActivarIndicador: "#TableIndicador tbody tr td .btn-power-on",
        btnEliminarIndicador: "#TableIndicador tbody tr td .btn-delete",
        btnClonarIndicador: "#TableIndicador tbody tr td .btn-clone",

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
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El indicador ya está en uso en el/los formularios: " + formularios + " ¿Desea eliminarlo?", jsMensajes.Variables.actionType.eliminar)
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
                        jsMensajes.Metodos.OkAlertModal("El indicador ha sido eliminado", jsMensajes.Variables.actionType.eliminar)
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
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El indicador ya está en uso en el/los formularios: " + formularios + " ¿Desea desactivarlo?", jsMensajes.Variables.actionType.estado)
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
                        jsMensajes.Metodos.OkAlertModal("El indicador ha sido desactivado", jsMensajes.Variables.actionType.eliminar)
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
                        jsMensajes.Metodos.OkAlertModal("El indicador ha sido activado", jsMensajes.Variables.actionType.estado)
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
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/IndicadorFonatel/ObtenerListaIndicadores',
                    type: "GET",
                    dataType: "JSON",
                    beforeSend: function () { },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            resolve(obj);
                        }
                        else {
                            reject();
                        }
                    },
                    error: function () {
                        reject()
                    }
                })
            })
        },

        EliminarIndicador: function (pIdIndicador) {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/IndicadorFonatel/EliminarIndicador',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () { },
                    data: { pIdIndicador },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            resolve(obj);
                        }
                        else {
                            reject(obj);
                        }
                    },
                    error: function () {
                        reject()
                    }
                })
            })
        },

        DesactivarIndicador: function (pIdIndicador) {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/IndicadorFonatel/DesactivarIndicador',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () { },
                    data: { pIdIndicador },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            resolve(obj);
                        }
                        else {
                            reject(obj);
                        }
                    },
                    error: function () {
                        reject()
                    }
                })
            })
        },

        ActivarIndicador: function (pIdIndicador) {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/IndicadorFonatel/ActivarIndicador',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () { },
                    data: { pIdIndicador },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            resolve(obj);
                        }
                        else {
                            reject(obj);
                        }
                    },
                    error: function () {
                        reject()
                    }
                })
            })
        },

        VerificarIndicadorEnFormularioWeb: function (pIdIndicador) {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/IndicadorFonatel/VerificarIndicadorEnFormularioWeb?pIdIndicador=' + pIdIndicador,
                    type: "GET",
                    dataType: "JSON",
                    beforeSend: function () { },
                    data: { pIdIndicador },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            resolve(obj);
                        }
                        else {
                            reject(obj);
                        }
                    },
                    error: function () {
                        reject()
                    }
                })
            })
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

    },

    Metodos: {

    },

    Consultas: {

    },

    Eventos: function () {
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

        $(document).on("click", CreateView.Controles.btnSiguienteIndicador, function (e) {
            console.log("Click on btnSiguienteIndicador button");
            e.preventDefault();
            $("a[href='#step-2']").trigger('click');
        });

        $(document).on("click", CreateView.Controles.btnSiguienteVariable, function (e) {
            console.log("Click on btnSiguienteVariable button");
            e.preventDefault();
            $("a[href='#step-3']").trigger('click');

        });

        $(document).on("click", CreateView.Controles.btnAtrasVariable, function (e) {
            console.log("Click on btnAtrasVariable button");
            e.preventDefault();
            $("a[href='#step-1']").trigger('click');

        });

        $(document).on("click", CreateView.Controles.btnAtrasCategoria, function (e) {
            e.preventDefault();
            $("a[href='#step-2']").trigger('click');

        });

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
        IndexView.Init();
    }

    if ($(CreateView.Controles.CreateView).length > 0) {
        CreateView.Init();
    }
});
