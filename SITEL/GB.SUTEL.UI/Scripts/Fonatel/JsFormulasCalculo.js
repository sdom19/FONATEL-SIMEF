JsFormulasCalculo = {
    "Controles": {

        "ddlFuenteIndicador": "#ddlFuenteIndicador",
        "ddlGrupoFonatel": "#ddlGrupoFonatel",
        "dllServicio": "#dllServicio",
        "ddlTipoFonatel": "#ddlTipoFonatel",
        "ddlIndicador": "#ddlIndicador",
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
        "btnEliminar_modalFechaFormulaCalculo": "#btnEliminar_modalFechaFormulaCalculo",
        "chkValorTotal": "#chkValorTotal",

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
        "ListadoFormulas":[]
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
                html = html + "<td>"+
                 "<button type='button' data-toggle='tooltip' data-placement='top' title='Editar' class='btn-icon-base btn-edit'></button>"+
                 "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' class='btn-icon-base btn-clone' ></button >"+
                 "<button type='button' data-toggle='tooltip' data-placement='top' title='Visualizar' class='btn-icon-base btn-view'></button>";


               

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
    },

    "Consultas": {
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Fórmula?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido eliminada")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/FormulaCalculo/Index";
                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnDesactivarFormula, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Fórmula?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {

            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido activada")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/FormulaCalculo/Index";
                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnCloneFormula, function () {
    let id = 1
    window.location.href = "/Fonatel/FormulaCalculo/Clone?id=" + id;
});

$(document).on("click", JsFormulasCalculo.Controles.btnEditFormula, function () {
    let id = 4
    window.location.href = "/Fonatel/FormulaCalculo/Edit?id=" + id;
});

$(document).on("click", JsFormulasCalculo.Controles.btnVerFormula, function () {
    let id = 1
    window.location.href = "/Fonatel/FormulaCalculo/View?id=" + id;
});

$(document).on("click", JsFormulasCalculo.Controles.btnActivarFormula, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Fórmula?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {

            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido desactivada")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/FormulaCalculo/Index";
                });
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial de la Fórmula de Cálculo?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido creada")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/FormulaCalculo/Index";
                });
        });
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
    jsMensajes.Metodos.OkAlertModal("El Detalle ha sido creado")
        .set('onok', function (closeEvent) {
            $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('hide');
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnRemoverItemFormula, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Fórmula?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido eliminada")
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar el Argumento de Fecha?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Argumento de Fecha ha sido eliminado")
                .set('onok', function (closeEvent) {
                    $(JsFormulasCalculo.Controles.modalFechaFormulaCalculo).modal('hide');
                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnEliminar_modalDetalle, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar el Detalle?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Detalle ha sido eliminado")
                .set('onok', function (closeEvent) {
                    $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('hide');
                });
        });
});

$(function () {
    let modo = $.urlParam("modo");

    if ($(JsFormulasCalculo.Controles.tablaFormulas).length > 0) {
        JsFormulasCalculo.Consultas.ConsultaListaFormulas();
    }
    



    if (modo == jsUtilidades.Variables.Acciones.Editar) {
        $(JsFormulasCalculo.Controles.txtCodigoFormula).prop("disabled", true);
        $(JsFormulasCalculo.Controles.txtCodigoFormula).val("F1P1TOR");        
        $(JsFormulasCalculo.Controles.txtNombreFormula).val("Torres según estado");
        $(JsFormulasCalculo.Controles.ddlFrecuenciaFormulario).val("2");
        $(JsFormulasCalculo.Controles.txtFechaCalculo).val("2022-01-01");
        $(JsFormulasCalculo.Controles.txtDescripcionFormula).val("Cálculo de del total de torres por estado constructivo");
        $(JsFormulasCalculo.Controles.ddlIndicadorFormulario).val("2");
        $(JsFormulasCalculo.Controles.ddlVariableFormulario).val("1");
        $(JsFormulasCalculo.Controles.radioTotal).prop("checked", true);
        $(JsFormulasCalculo.Controles.ddlFuenteIndicador).val("2");
        $(JsFormulasCalculo.Controles.ddlGrupoFonatel).val("2");
        $(JsFormulasCalculo.Controles.dllServicio).val("1");
        $(JsFormulasCalculo.Controles.ddlTipoFonatel).val("3");
        $(JsFormulasCalculo.Controles.ddlIndicador).val("1");
        $(JsFormulasCalculo.Controles.chkValorTotal).prop("checked", true);

        if (ddlFuenteIndicador != "1") {
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
    }
    else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
        $(JsFormulasCalculo.Controles.txtCodigoFormula).val("");
        $(JsFormulasCalculo.Controles.txtNombreFormula).val("");
        $(JsFormulasCalculo.Controles.ddlFrecuenciaFormulario).val("2");
        $(JsFormulasCalculo.Controles.txtFechaCalculo).val("2022-01-01");
        $(JsFormulasCalculo.Controles.txtDescripcionFormula).val("Cálculo de del total de torres por estado constructivo");
        $(JsFormulasCalculo.Controles.ddlIndicadorFormulario).val("2");
        $(JsFormulasCalculo.Controles.ddlVariableFormulario).val("1");
        $(JsFormulasCalculo.Controles.radioTotal).prop("checked", true);
        $(JsFormulasCalculo.Controles.ddlFuenteIndicador).val("2");
        $(JsFormulasCalculo.Controles.ddlGrupoFonatel).val("2");
        $(JsFormulasCalculo.Controles.dllServicio).val("1");
        $(JsFormulasCalculo.Controles.ddlTipoFonatel).val("3");
        $(JsFormulasCalculo.Controles.ddlIndicador).val("1");
        $(JsFormulasCalculo.Controles.chkValorTotal).prop("checked", true);

        if (ddlFuenteIndicador != "1") {
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
    }
    else if (modo == jsUtilidades.Variables.Acciones.Consultar) {
        $(JsFormulasCalculo.Controles.txtCodigoFormula).prop("disabled", true);
        $(JsFormulasCalculo.Controles.txtCodigoFormula).val("F1P1TOR");
        $(JsFormulasCalculo.Controles.txtNombreFormula).val("Torres según estado");
        $(JsFormulasCalculo.Controles.txtNombreFormula).prop("disabled", true);
        $(JsFormulasCalculo.Controles.ddlFrecuenciaFormulario).val("2");
        $(JsFormulasCalculo.Controles.ddlFrecuenciaFormulario).prop("disabled", true);
        $(JsFormulasCalculo.Controles.txtFechaCalculo).val("2022-01-01");
        $(JsFormulasCalculo.Controles.txtFechaCalculo).prop("disabled", true);
        $(JsFormulasCalculo.Controles.txtDescripcionFormula).val("Cálculo de del total de torres por estado constructivo");
        $(JsFormulasCalculo.Controles.txtDescripcionFormula).prop("disabled", true);
        $(JsFormulasCalculo.Controles.ddlIndicadorFormulario).val("2");
        $(JsFormulasCalculo.Controles.ddlIndicadorFormulario).prop("disabled", true);
        $(JsFormulasCalculo.Controles.ddlVariableFormulario).val("1");
        $(JsFormulasCalculo.Controles.ddlVariableFormulario).prop("disabled", true);
        $(JsFormulasCalculo.Controles.radioTotal).prop("checked", true);
        $(JsFormulasCalculo.Controles.radioTotal).prop("disabled", true);
        $(JsFormulasCalculo.Controles.radioCategoríaDesagregacion).prop("disabled", true);
        $(JsFormulasCalculo.Controles.ddlFuenteIndicador).val("2");
        $(JsFormulasCalculo.Controles.ddlFuenteIndicador).prop("disabled", true);
        $(JsFormulasCalculo.Controles.ddlGrupoFonatel).val("2");
        $(JsFormulasCalculo.Controles.ddlGrupoFonatel).prop("disabled", true);
        $(JsFormulasCalculo.Controles.dllServicio).val("1");
        $(JsFormulasCalculo.Controles.dllServicio).prop("disabled", true);
        $(JsFormulasCalculo.Controles.ddlTipoFonatel).val("3");
        $(JsFormulasCalculo.Controles.ddlTipoFonatel).prop("disabled", true);
        $(JsFormulasCalculo.Controles.ddlIndicador).val("1");
        $(JsFormulasCalculo.Controles.ddlIndicador).prop("disabled", true);
        $(JsFormulasCalculo.Controles.chkValorTotal).prop("checked", true);
        $(JsFormulasCalculo.Controles.chkValorTotal).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnAgregarDetalleAgregacion).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnEliminarDetalleAgregacion).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnGuardarGestionFormulaCalculo).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnFinalizarFormulaCalculo).prop("disabled", true);
        $(JsFormulasCalculo.Controles.btnGuardarFormulaCalculo).prop("disabled", true);

        if (ddlFuenteIndicador != "1") {
            $(JsFormulasCalculo.Controles.divGrupo).css("display", "block");
            $(JsFormulasCalculo.Controles.divGrupo).prop("disabled", true);
            $(JsFormulasCalculo.Controles.divServicio).css("display", "block");
            $(JsFormulasCalculo.Controles.divServicio).prop("disabled", true);
            $(JsFormulasCalculo.Controles.divClasificacion).css("display", "none");
            $(JsFormulasCalculo.Controles.divClasificacion).prop("disabled", true);
            $(JsFormulasCalculo.Controles.divTipoIndicador).css("display", "block");
            $(JsFormulasCalculo.Controles.divTipoIndicador).prop("disabled", true);
            $(JsFormulasCalculo.Controles.divIndicador).css("display", "block");
            $(JsFormulasCalculo.Controles.divIndicador).prop("disabled", true);
            $(JsFormulasCalculo.Controles.divAcumulacion).css("display", "none");
            $(JsFormulasCalculo.Controles.divAcumulacion).prop("disabled", true);

            $(JsFormulasCalculo.Controles.columnaDetalleTabla).html("Detalle Agrupación");
            $(JsFormulasCalculo.Controles.titulo_modalDetalleFormulaCalculo).html("Detalle Agrupación");
            $(JsFormulasCalculo.Controles.divCategoría_ModalDetalle).css("display", "none");
            $(JsFormulasCalculo.Controles.divCriterio_ModalDetalle).css("display", "block");
        }
    }
});