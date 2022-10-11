JsFormulasCalculo = {
    "Controles": {

        "ddlFuenteIndicador": "#ddlFuenteIndicador",
        "ddlGrupoFonatel": "#ddlGrupoFonatel",
        "dllServicio": "#dllServicio",
        "ddlTipoFonatel": "#ddlTipoFonatel",
        "ddlIndicador": "#ddlIndicador",
        "ddlCategoríaDesagregacion": "#ddlCategoríaDesagregacion",
        "modalFormulaDetalleAgregacion": "#modalFormulaDetalleAgregacion",
        "modalFechaFormulaCalculo": "#modalFechaFormulaCalculo",
        "btnAgregarDetalleAgregacion": "#btnAgregarDetalleAgregacion",
        "btnEliminarDetalleAgregacion": "#btnEliminarDetalleAgregacion",
        "radioCategoríaDesagregacion": "#radioCategoríaDesagregacion",
        "radioTotal": "#radioTotal",
        "divInputCategoríaDesagregacion": "#divInputCategoríaDesagregacion",
        //"divInputDetalleCategoríaDesagregacion": "#divInputDetalleCategoríaDesagregacion",
        "divGrupo": "#divGrupo",
        "divClasificacion": "#divClasificacion",
        "divTipoIndicador": "#divTipoIndicador",
        "divIndicador": "#divIndicador",
        "divServicio": "#divServicio",
        "divAcumulacion": "#divAcumulacion",
        "btnAtrasGestionFormula": "#btnAtrasGestionFormula",
        "btnGuardarFormulaCalculo": "#btnGuardarFormulaCalculo",
        "btnFinalizarFormulaCalculo": "#btnFinalizarFormulaCalculo",
        "btnGuardarGestionFormulaCalculo": "#btnGuardarGestionFormulaCalculo",
        "btnCancelarGestionFormulaCalculo": "#btnCancelarGestionFormulaCalculo",
        "btnCancelar": "#btnCancelarFormula",
        "btnCalendarFormula": "#btnCalendarFormula",
        "txtFechaCalculo": "#txtFechaCalculo",
        "txtCodigoFormula": "#txtCodigoFormula",
        "txtNombreFormula": "#txtNombreFormula",
        "txtDescripcionFormula": "#txtDescripcionFormula",
        "txtFechaCalculoHelp": "#txtFechaCalculoHelp",
        "btnRemoverItemFormula": "#btnRemoverItemFormula",
        "btnGuardar_modalFechaFormulaCalculo": "#btnGuardar_modalFechaFormulaCalculo",
        "btnCancelar_modalFechaFormulaCalculo": "#btnCancelar_modalFechaFormulaCalculo",
        "btnEliminar_modalFechaFormulaCalculo": "#btnEliminar_modalFechaFormulaCalculo",
        "chkValorTotal": "#chkValorTotal",
        "txtCodigoFormulaHelp": "#txtCodigoFormulaHelp",
        "txtNombreFormulaHelp": "#txtNombreFormulaHelp",
        "ddlFrecuenciaFormularioHelp": "#ddlFrecuenciaFormularioHelp",
        "txtFechaCalculoHelp": "#txtFechaCalculoHelp",
        "txtDescripcionFormulaHelp": "#txtDescripcionFormulaHelp",
        "ddlIndicadorFormularioHelp": "#ddlIndicadorFormularioHelp",
        "ddlVariableFormularioHelp": "#ddlVariableFormularioHelp",
        "ddlNivelCalculoHelp": "#ddlNivelCalculoHelp",
        "ddlCategoríaDesagregacionHelp": "#ddlCategoríaDesagregacionHelp",
        "btnAgregarArgumento": "#TableaIndicadoresVariable tbody tr td .btn-add",
        "btnEjecutarFormula": "#btnEjecutarFormula",

        //Modal detalle
        "columnaDetalleTabla": "#columnaDetalleTabla",
        "titulo_modalDetalleFormulaCalculo": "#titulo_modalDetalleFormulaCalculo",
        "divCriterio_ModalDetalle": "#divCriterio_ModalDetalle",
        "divCategoría_ModalDetalle": "#divCategoría_ModalDetalle",
        "btnGuardar_modalDetalle": "#btnGuardar_modalDetalle",
        "btnEliminar_modalDetalle": "#btnEliminar_modalDetalle",
        "btnSiguienteFormulaCalculo": "#btnSiguienteFormulaCalculo",
        "ddlCategoría_ModalDetalle": "#ddlCategoría_ModalDetalle",
        "ddlDetalle_ModalDetalle": "#ddlDetalle_ModalDetalle",
        "ddlCriterio_ModalDetalle": "#ddlCriterio_ModalDetalle",
        "ddlFrecuenciaFormulario": "#ddlFrecuenciaFormulario",
        "ddlIndicadorFormulario": "#ddlIndicadorFormulario",
        "ddlVariableFormulario": "#ddlVariableFormulario",
        "btnEliminarFormula": "#Tablaformulasdetalle tbody tr td .btn-delete",
        "btnActivarFormula": "#Tablaformulasdetalle tbody tr td .btn-power-on",
        "btnEditFormula": "#Tablaformulasdetalle tbody tr td .btn-edit",
        "btnVerFormula": "#Tablaformulasdetalle tbody tr td .btn-view",
        "btnCloneFormula": "#Tablaformulasdetalle tbody tr td .btn-clone",
        "btnDesactivarFormula": "#Tablaformulasdetalle tbody tr td .btn-power-off",
        "tablaFormulas": "#Tablaformulasdetalle tbody",
        //Modal Fechas - Formula de cálculo
        "ddlTipoFechaFinalModalFechaFormula": "#ddlTipoFechaFinalModalFechaFormula",
        "ddlTipoFechaInicioModalFechaFormula": "#ddlTipoFechaInicioModalFechaFormula",
        "divFechaInicioFormulaCalculo": "#divFechaInicioFormulaCalculo",
        "divCategoríaFechaInicioFormulaCalculo": "#divCategoríaFechaInicioFormulaCalculo",
        "divFechaFinalFormulaCalculo": "#divFechaFinalFormulaCalculo",
        "divCategoríaFechaFinalFormulaCalculo": "#divCategoríaFechaFinalFormulaCalculo",
        "ControlesStep1": "#formCrearFormula input, #formCrearFormula textarea, #formCrearFormula select",
        "step2": "a[href='#step-2']",
        "divStep2": "#step-2 input, #step-2 select, #step-2 button",

        "modoFormulario": "#modoFormulario"
    },

    "Variables": {
        "Direccion": {
            "FONATEL": 1,
            "MERCADOS": 2,
            "CALIDAD": 3
        },
        "FECHAS": {
            "ACTUAL": "3",
            "Categoría": "2",
            "FECHA": "1"
        },
        "ListadoFormulas": []
    },

    "Metodos": {

        "CargarTablaFormulas": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsFormulasCalculo.Variables.ListadoFormulas.length; i++) {
                let Formula = JsFormulasCalculo.Variables.ListadoFormulas[i];
                html = html + "<tr><td scope='row'>" + Formula.Codigo + "</td>";
                html = html + "<td>" + Formula.Nombre + "</td>";
                html = html + "<td>" + Formula.Descripcion + "</td>";
                html = html + "<td>" + Formula.EstadoRegistro.Nombre + "</td>";
                html = html + "<td>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Editar' value='" + Formula.id + "' class='btn-icon-base btn-edit'></button>" +
                    "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' value='" + Formula.id + "' class='btn-icon-base btn-clone' ></button >" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Visualizar' value='" + Formula.id + "' class='btn-icon-base btn-view'></button>";




                if (Formula.IdEstado == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value='" + Formula.id + "' class='btn-icon-base btn-power-off'></button>";
                } else {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value='" + Formula.id + "' class='btn-icon-base btn-power-on'></button>";

                }

                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' value='" + Formula.id + "'  class='btn-icon-base btn-delete'></button>";

                html = html + "</td></tr>"
            }
            $(JsFormulasCalculo.Controles.tablaFormulas).html(html);
            CargarDatasource();
            JsFormulasCalculo.Variables.ListadoFormulas = [];
        },

        "ValidarNombreyCodigo": function () {
            let validar = true;
            $(JsFormulasCalculo.Controles.txtCodigoFormulaHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.txtNombreFormulaHelp).addClass("hidden");

            let codigo = $(JsFormulasCalculo.Controles.txtCodigoFormula).val().trim();
            let nombre = $(JsFormulasCalculo.Controles.txtNombreFormula).val().trim();

            if (codigo.length == 0) {
                $(JsFormulasCalculo.Controles.txtCodigoFormulaHelp).removeClass("hidden");
                Validar = false;
            }
            if (nombre.length == 0) {
                $(JsFormulasCalculo.Controles.txtNombreFormulaHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },

        "ValidarFormularioCrear": function () {
            $(JsFormulasCalculo.Controles.txtCodigoFormulaHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.txtNombreFormulaHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.ddlFrecuenciaFormularioHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.txtFechaCalculoHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.txtDescripcionFormulaHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.ddlIndicadorFormularioHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.ddlNivelCalculoHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.ddlVariableFormularioHelp).addClass("hidden");
            $(JsFormulasCalculo.Controles.ddlCategoríaDesagregacionHelp).addClass("hidden");
            if ($(JsFormulasCalculo.Controles.txtCodigoFormula).val() != null && $(JsFormulasCalculo.Controles.txtCodigoFormula).val().trim().length > 0
                && $(JsFormulasCalculo.Controles.txtNombreFormula).val() != null && $(JsFormulasCalculo.Controles.txtNombreFormula).val().trim().length > 0
                && $(JsFormulasCalculo.Controles.txtFechaCalculo).val().trim() != "0001-01-01" && $(JsFormulasCalculo.Controles.ddlFrecuenciaFormulario).val().trim().length > 0
                && $(JsFormulasCalculo.Controles.txtDescripcionFormula).val().trim().length > 0 && $(JsFormulasCalculo.Controles.ddlIndicadorFormulario).val().trim().length > 0
                && $(JsFormulasCalculo.Controles.ddlVariableFormulario).val().trim().length > 0) {

                if (radioTotal.checked) {
                    $(JsFormulasCalculo.Controles.btnSiguienteFormulaCalculo).prop("disabled", false);
                    $(JsFormulasCalculo.Controles.step2).prop("disabled", false);
                }
                else if ($(JsFormulasCalculo.Controles.ddlCategoríaDesagregacion).val() != null && $(JsFormulasCalculo.Controles.ddlCategoríaDesagregacion).val().length > 0) {

                    $(JsFormulasCalculo.Controles.btnSiguienteFormulaCalculo).prop("disabled", false);
                    $(JsFormulasCalculo.Controles.step2).prop("disabled", false);
                }
                else {
                    $(JsFormulasCalculo.Controles.btnSiguienteFormulaCalculo).prop("disabled", true);
                    $(JsFormulasCalculo.Controles.step2).prop("disabled", true);
                }
            }
            else {
                $(JsFormulasCalculo.Controles.btnSiguienteFormulaCalculo).prop("disabled", true);
                $(JsFormulasCalculo.Controles.step2).prop("disabled", true);
            }
        },
    },

    "Consultas": {

        "EliminarFormulaCalculo": function (idFormula) {

            let formulaCalculo = new Object();
            formulaCalculo.id = idFormula;


            $("#loading").fadeIn();
            execAjaxCall("/FormulaCalculo/EliminarFormula", "POST", { formulaCalculo: formulaCalculo })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido eliminada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/FormulaCalculo/Index";
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

        "ActivarFormulaCalculo": function (idFormula) {
            let formulaCalculo = new Object();
            formulaCalculo.id = idFormula;
            $("#loading").fadeIn();
            execAjaxCall("/FormulaCalculo/ActivarFormula", "POST", { formulaCalculo: formulaCalculo })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido activada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/FormulaCalculo/Index";
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

        "DesactivarFormulaCalculo": function (idFormula) {
            let formulaCalculo = new Object();
            formulaCalculo.id = idFormula;
            $("#loading").fadeIn();
            execAjaxCall("/FormulaCalculo/DesactivarFormula", "POST", { formulaCalculo: formulaCalculo })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido desactivada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/FormulaCalculo/Index";
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

        "ConsultaListaFormulas": function () {
            $("#loading").fadeIn();
            execAjaxCall("/FormulaCalculo/ObtenerListaFormulas", "GET")
                .then((obj) => {
                    JsFormulasCalculo.Variables.ListadoFormulas = obj.objetoRespuesta;
                    JsFormulasCalculo.Metodos.CargarTablaFormulas();
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
    }

};

$(document).on("click", JsFormulasCalculo.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/FormulaCalculo/Index";
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnEliminarFormula, function (e) {
    e.preventDefault();
    let idFormula = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Fórmula?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsFormulasCalculo.Consultas.EliminarFormulaCalculo(idFormula);
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnDesactivarFormula, function (e) {
    e.preventDefault();
    let idFormula = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Fórmula?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsFormulasCalculo.Consultas.ActivarFormulaCalculo(idFormula);
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnCloneFormula, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/FormulaCalculo/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
});

$(document).on("click", JsFormulasCalculo.Controles.btnEditFormula, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/FormulaCalculo/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

$(document).on("click", JsFormulasCalculo.Controles.btnVerFormula, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/FormulaCalculo/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Consultar;
});

$(document).on("click", JsFormulasCalculo.Controles.btnActivarFormula, function (e) {
    e.preventDefault();
    let idFormula = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Fórmula?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsFormulasCalculo.Consultas.DesactivarFormulaCalculo(idFormula);
        });
});

$(document).on("change", JsFormulasCalculo.Controles.radioCategoríaDesagregacion, function () {
    $(JsFormulasCalculo.Controles.divInputCategoríaDesagregacion).css("display", "block");
});

$(document).on("change", JsFormulasCalculo.Controles.chkValorTotal, function () {
    if (chkValorTotal.checked) {
        $(JsFormulasCalculo.Controles.btnAgregarDetalleAgregacion).prop("disabled", true);
    }
    else {
        $(JsFormulasCalculo.Controles.btnAgregarDetalleAgregacion).prop("disabled", false);
    }
});

$(document).on("change", JsFormulasCalculo.Controles.ddlTipoFechaInicioModalFechaFormula, function () {
    $(JsFormulasCalculo.Controles.divCategoríaFechaInicioFormulaCalculo).addClass("hidden");
    $(JsFormulasCalculo.Controles.divFechaInicioFormulaCalculo).addClass("hidden");

    let option = $(this).val();

    switch (option) {

        case JsFormulasCalculo.Variables.FECHAS.FECHA:
            $(JsFormulasCalculo.Controles.divFechaInicioFormulaCalculo).removeClass("hidden");
            break
        case JsFormulasCalculo.Variables.FECHAS.Categoría:
            $(JsFormulasCalculo.Controles.divCategoríaFechaInicioFormulaCalculo).removeClass("hidden");
            break;
        default:
    }
});

$(document).on("change", JsFormulasCalculo.Controles.ddlTipoFechaFinalModalFechaFormula, function () {
    $(JsFormulasCalculo.Controles.divFechaFinalFormulaCalculo).addClass("hidden");
    $(JsFormulasCalculo.Controles.divCategoríaFechaFinalFormulaCalculo).addClass("hidden");
    let option = $(this).val();

    switch (option) {

        case JsFormulasCalculo.Variables.FECHAS.FECHA:
            $(JsFormulasCalculo.Controles.divFechaFinalFormulaCalculo).removeClass("hidden");
            break
        case JsFormulasCalculo.Variables.FECHAS.Categoría:
            $(JsFormulasCalculo.Controles.divCategoríaFechaFinalFormulaCalculo).removeClass("hidden");
            break;
        default:
    }
});

$(document).on("change", JsFormulasCalculo.Controles.ddlFuenteIndicador, function () {
    let optionSelected = $(this).select2('data')[0].id;

    if (optionSelected == "1") {
        $(JsFormulasCalculo.Controles.divGrupo).css("display", "block");
        $(JsFormulasCalculo.Controles.divClasificacion).css("display", "block");
        $(JsFormulasCalculo.Controles.divTipoIndicador).css("display", "block");
        $(JsFormulasCalculo.Controles.divIndicador).css("display", "block");
        $(JsFormulasCalculo.Controles.divServicio).css("display", "none");
        $(JsFormulasCalculo.Controles.divAcumulacion).css("display", "block");

        $(JsFormulasCalculo.Controles.columnaDetalleTabla).html("Detalle Desagregación");
        $(JsFormulasCalculo.Controles.titulo_modalDetalleFormulaCalculo).html("Detalle Desagregación");
        $(JsFormulasCalculo.Controles.divCategoría_ModalDetalle).css("display", "block");
        $(JsFormulasCalculo.Controles.divCriterio_ModalDetalle).css("display", "none");
    }
    else if (optionSelected != "1") {
        $(JsFormulasCalculo.Controles.divGrupo).css("display", "block");
        $(JsFormulasCalculo.Controles.divServicio).css("display", "block");
        $(JsFormulasCalculo.Controles.divClasificacion).css("display", "none");
        $(JsFormulasCalculo.Controles.divTipoIndicador).css("display", "block");
        $(JsFormulasCalculo.Controles.divIndicador).css("display", "block");
        $(JsFormulasCalculo.Controles.divAcumulacion).css("display", "none");

        $(JsFormulasCalculo.Controles.columnaDetalleTabla).html("Detalle Agrupación");
        $(JsFormulasCalculo.Controles.titulo_modalDetalleFormulaCalculo).html("Detalle Agrupación");
        $(JsFormulasCalculo.Controles.divCategoría_ModalDetalle).css("display", "none");
        $(JsFormulasCalculo.Controles.divCriterio_ModalDetalle).css("display", "block");

    }
});

$(document).on("click", JsFormulasCalculo.Controles.btnAgregarDetalleAgregacion, function () {
    $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('show');
    $(JsFormulasCalculo.Controles.btnGuardar_modalDetalle).css("display", "initial");
    $(JsFormulasCalculo.Controles.btnEliminar_modalDetalle).css("display", "none");

    $(JsFormulasCalculo.Controles.ddlCategoría_ModalDetalle).select2("enable", "true");
    $(JsFormulasCalculo.Controles.ddlDetalle_ModalDetalle).select2("enable", "true");
    $(JsFormulasCalculo.Controles.ddlCriterio_ModalDetalle).select2("enable", "true");
});

$(document).on("click", JsFormulasCalculo.Controles.btnEliminarDetalleAgregacion, function () {
    $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('show');
    $(JsFormulasCalculo.Controles.btnGuardar_modalDetalle).css("display", "none");
    $(JsFormulasCalculo.Controles.btnEliminar_modalDetalle).css("display", "initial");

    $(JsFormulasCalculo.Controles.ddlCategoría_ModalDetalle).select2("enable", false);
    $(JsFormulasCalculo.Controles.ddlDetalle_ModalDetalle).select2("enable", false);
    $(JsFormulasCalculo.Controles.ddlCriterio_ModalDetalle).select2("enable", false);
});

$(document).on("change", JsFormulasCalculo.Controles.radioTotal, function () {
    $(JsFormulasCalculo.Controles.divInputCategoríaDesagregacion).css("display", "none");
});

$(document).on("click", JsFormulasCalculo.Controles.btnAtrasGestionFormula, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});

$(document).on("click", JsFormulasCalculo.Controles.btnGuardarFormulaCalculo, function (e) {
    e.preventDefault();
    if (JsFormulasCalculo.Metodos.ValidarNombreyCodigo()) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial de la Fórmula de Cálculo?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido creada")
                    .set('onok', function (closeEvent) {
                        window.location.href = "/Fonatel/FormulaCalculo/Index";
                    });
            });
    }
});

$(document).on("click", JsFormulasCalculo.Controles.btnSiguienteFormulaCalculo, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');
});

$(document).on("click", JsFormulasCalculo.Controles.btnFinalizarFormulaCalculo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Fórmula de Cálculo?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido agregada")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/FormulaCalculo/Index"
                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnGuardarGestionFormulaCalculo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Existen campos vacíos. ¿Desea realizar un guardado parcial de la Fórmula de Cálculo?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido creada")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/FormulaCalculo/Index"
                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnCancelarGestionFormulaCalculo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/FormulaCalculo/Index";
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnCancelar_modalFechaFormulaCalculo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            $(JsFormulasCalculo.Controles.modalFechaFormulaCalculo).modal('hide');
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnCalendarFormula, function (e) {
    $(JsFormulasCalculo.Controles.modalFechaFormulaCalculo).modal('show');
});

$(document).on("click", JsFormulasCalculo.Controles.radioManual_modalFechaFormula, function () {
    $(JsFormulasCalculo.Controles.divTxtFechaInicio_modalFechaFormula).css("display", "block");
    $(JsFormulasCalculo.Controles.divTxtFechaFinal_modalFechaFormula).css("display", "block");
    $(JsFormulasCalculo.Controles.divDdlCategoríasTipoFecha_modalFechaFormula).css("display", "none");
});

$(document).on("click", JsFormulasCalculo.Controles.radioCategoríaDesagregacion_modalFechaFormula, function () {
    $(JsFormulasCalculo.Controles.divTxtFechaInicio_modalFechaFormula).css("display", "none");
    $(JsFormulasCalculo.Controles.divTxtFechaFinal_modalFechaFormula).css("display", "none");
    $(JsFormulasCalculo.Controles.divDdlCategoríasTipoFecha_modalFechaFormula).css("display", "block");
});

$(document).on("click", JsFormulasCalculo.Controles.btnGuardar_modalDetalle, function () {
    jsMensajes.Metodos.OkAlertModal("El Detalle ha sido agregado")
        .set('onok', function (closeEvent) {
            $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('hide');
            $(JsFormulasCalculo.Controles.chkValorTotal).prop("disabled", true);
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnRemoverItemFormula, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar El Argumento?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Argumento ha sido eliminado")
                .set('onok', function (closeEvent) {

                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnGuardar_modalFechaFormulaCalculo, function () {
    jsMensajes.Metodos.OkAlertModal("El Argumento de Fecha ha sido creado")
        .set('onok', function (closeEvent) {
            $(JsFormulasCalculo.Controles.modalFechaFormulaCalculo).modal('hide');
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnEliminar_modalFechaFormulaCalculo, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Argumento de Fecha?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Argumento de Fecha ha sido eliminado")
                .set('onok', function (closeEvent) {
                    $(JsFormulasCalculo.Controles.modalFechaFormulaCalculo).modal('hide');
                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnAgregarArgumento, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Argumento?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Argumento ha sido agregado")
                .set('onok', function (closeEvent) {

                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnEjecutarFormula, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea ejecutar la Fórmula?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido ejecutada")
                .set('onok', function (closeEvent) {

                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnEliminar_modalDetalle, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Detalle?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Detalle ha sido eliminado")
                .set('onok', function (closeEvent) {
                    $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('hide');
                });
        });
});
$(document).on("keyup", JsFormulasCalculo.Controles.ControlesStep1, function (e) {
    JsFormulasCalculo.Metodos.ValidarFormularioCrear();
});

$(document).on("change", JsFormulasCalculo.Controles.ControlesStep1, function (e) {
    JsFormulasCalculo.Metodos.ValidarFormularioCrear();
});

$(function () {
    let modo =ObtenerValorParametroUrl("modo");

    if ($(JsFormulasCalculo.Controles.tablaFormulas).length > 0) {
        JsFormulasCalculo.Consultas.ConsultaListaFormulas();
    }

    if (modo == null) {
        JsFormulasCalculo.Metodos.ValidarFormularioCrear();
        $(JsFormulasCalculo.Controles.radioCategoríaDesagregacion).prop("checked", false);
    }

    if (modo == jsUtilidades.Variables.Acciones.Editar) {
        JsFormulasCalculo.Metodos.ValidarFormularioCrear();
        $(JsFormulasCalculo.Controles.txtCodigoFormula).prop("disabled", true);
    }

    if (modo == jsUtilidades.Variables.Acciones.Clonar) {
        JsFormulasCalculo.Metodos.ValidarFormularioCrear();
        $(JsFormulasCalculo.Controles.txtCodigoFormula).val("");
        $(JsFormulasCalculo.Controles.txtNombreFormula).val("");
    }

    if (modo == jsUtilidades.Variables.Acciones.Consultar) {
        JsFormulasCalculo.Metodos.ValidarFormularioCrear();
        $(JsFormulasCalculo.Controles.ControlesStep1).prop("disabled", true);
        $(JsFormulasCalculo.Controles.divStep2).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnGuardarFormulaCalculo).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnGuardarGestionFormulaCalculo).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnFinalizarFormulaCalculo).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnCancelarGestionFormulaCalculo).prop("disabled", false);
        $(JsFormulasCalculo.Controles.btnAtrasGestionFormula).prop("disabled", false);
    }

});