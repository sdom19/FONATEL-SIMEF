JsFormulasCalculo = {
    "Controles": {


    

        "ddlFuenteIndicador": "#ddlFuenteIndicador",
        "modalFormulaDetalleAgregacion": "#modalFormulaDetalleAgregacion",
        "modalFechaFormulaCalculo": "#modalFechaFormulaCalculo",
        "btnAgregarDetalleAgregacion": "#btnAgregarDetalleAgregacion",
        "btnEliminarDetalleAgregacion": "#btnEliminarDetalleAgregacion",
        "radioCategoriaDesagregacion": "#radioCategoriaDesagregacion",
        "radioTotal": "#radioTotal",
        "divInputCategoriaDesagregacion": "#divInputCategoriaDesagregacion",
        //"divInputDetalleCategoriaDesagregacion": "#divInputDetalleCategoriaDesagregacion",
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

        //Modal detalle
        "columnaDetalleTabla": "#columnaDetalleTabla",
        "titulo_modalDetalleFormulaCalculo": "#titulo_modalDetalleFormulaCalculo",
        "divCriterio_ModalDetalle": "#divCriterio_ModalDetalle",
        "divCategoria_ModalDetalle": "#divCategoria_ModalDetalle",
        "btnGuardar_modalDetalle": "#btnGuardar_modalDetalle",
        "btnEliminar_modalDetalle": "#btnEliminar_modalDetalle",
        "btnSiguienteFormulaCalculo":"#btnSiguienteFormulaCalculo",
        "ddlCategoria_ModalDetalle": "#ddlCategoria_ModalDetalle",
        "ddlDetalle_ModalDetalle": "#ddlDetalle_ModalDetalle",
        "ddlCriterio_ModalDetalle": "#ddlCriterio_ModalDetalle",

        //Modal Fechas - Formula de cálculo
        "ddlTipoFechaFinalModalFechaFormula": "#ddlTipoFechaFinalModalFechaFormula",
        "ddlTipoFechaInicioModalFechaFormula": "#ddlTipoFechaInicioModalFechaFormula",
        "divFechaInicioFormulaCalculo":"#divFechaInicioFormulaCalculo",
        "divCategoriaFechaInicioFormulaCalculo": "#divCategoriaFechaInicioFormulaCalculo",
        "divFechaFinalFormulaCalculo": "#divFechaFinalFormulaCalculo",
        "divCategoriaFechaFinalFormulaCalculo": "#divCategoriaFechaFinalFormulaCalculo"
    },
    "Variables": {
        "Direccion": {
            "FONATEL": 1,
            "MERCADOS": 2,
            "CALIDAD":3
        },
        "FECHAS": {
            "ACTUAL": "3",
            "CATEGORIA":"2",
            "FECHA": "1"
        }
        
    },

    "Metodos": {
   
    }

}
$(document).on("click", JsFormulasCalculo.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/FormulaCalculo/Index";
        });
});
$(document).on("change", JsFormulasCalculo.Controles.radioCategoriaDesagregacion, function () {
    $(JsFormulasCalculo.Controles.divInputCategoriaDesagregacion).css("display", "block");
});


$(document).on("change", JsFormulasCalculo.Controles.ddlTipoFechaInicioModalFechaFormula, function () {
    $(JsFormulasCalculo.Controles.divCategoriaFechaInicioFormulaCalculo).addClass("hidden");
    $(JsFormulasCalculo.Controles.divFechaInicioFormulaCalculo).addClass("hidden");

    let option = $(this).val();

    switch (option) {

        case JsFormulasCalculo.Variables.FECHAS.FECHA:
            $(JsFormulasCalculo.Controles.divFechaInicioFormulaCalculo).removeClass("hidden");
            break
        case JsFormulasCalculo.Variables.FECHAS.CATEGORIA:
            $(JsFormulasCalculo.Controles.divCategoriaFechaInicioFormulaCalculo).removeClass("hidden");
            break;
        default:
    }
});


$(document).on("change", JsFormulasCalculo.Controles.ddlTipoFechaFinalModalFechaFormula, function () {
    $(JsFormulasCalculo.Controles.divFechaFinalFormulaCalculo).addClass("hidden");
    $(JsFormulasCalculo.Controles.divCategoriaFechaFinalFormulaCalculo).addClass("hidden");
    let option = $(this).val();

    switch (option) {

        case JsFormulasCalculo.Variables.FECHAS.FECHA:
            $(JsFormulasCalculo.Controles.divFechaFinalFormulaCalculo).removeClass("hidden");
            break
        case JsFormulasCalculo.Variables.FECHAS.CATEGORIA:
            $(JsFormulasCalculo.Controles.divCategoriaFechaFinalFormulaCalculo).removeClass("hidden");
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
        $(JsFormulasCalculo.Controles.divCategoria_ModalDetalle).css("display", "block");
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
        $(JsFormulasCalculo.Controles.divCategoria_ModalDetalle).css("display", "none");
        $(JsFormulasCalculo.Controles.divCriterio_ModalDetalle).css("display", "block");

    }
});

$(document).on("click", JsFormulasCalculo.Controles.btnAgregarDetalleAgregacion, function () {
    $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('show');
    $(JsFormulasCalculo.Controles.btnGuardar_modalDetalle).css("display", "initial");
    $(JsFormulasCalculo.Controles.btnEliminar_modalDetalle).css("display", "none");

    $(JsFormulasCalculo.Controles.ddlCategoria_ModalDetalle).select2("enable", "true");
    $(JsFormulasCalculo.Controles.ddlDetalle_ModalDetalle).select2("enable", "true");
    $(JsFormulasCalculo.Controles.ddlCriterio_ModalDetalle).select2("enable", "true");
});

$(document).on("click", JsFormulasCalculo.Controles.btnEliminarDetalleAgregacion, function () {
    $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('show');
    $(JsFormulasCalculo.Controles.btnGuardar_modalDetalle).css("display", "none");
    $(JsFormulasCalculo.Controles.btnEliminar_modalDetalle).css("display", "initial");

    $(JsFormulasCalculo.Controles.ddlCategoria_ModalDetalle).select2("enable", false);
    $(JsFormulasCalculo.Controles.ddlDetalle_ModalDetalle).select2("enable", false);
    $(JsFormulasCalculo.Controles.ddlCriterio_ModalDetalle).select2("enable", false);
});







$(document).on("change", JsFormulasCalculo.Controles.radioTotal, function () {
   
});

$(document).on("click", JsFormulasCalculo.Controles.btnAtrasGestionFormula, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});

$(document).on("click", JsFormulasCalculo.Controles.btnGuardarFormulaCalculo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para la Fórmula de Cálculo?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fórmula ha sido creada")
                .set('onok', function (closeEvent) {
                    $("a[href='#step-2']").trigger('click');
                });
        });
});


$(document).on("click", JsFormulasCalculo.Controles.btnSiguienteFormulaCalculo, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');
});

$(document).on("click", JsFormulasCalculo.Controles.btnFinalizarFormulaCalculo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la fórmula de cálculo?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La fórmula ha sido agregada correctamente.")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/FormulaCalculo/Index"
                });
        });
});

$(document).on("click", JsFormulasCalculo.Controles.btnGuardarGestionFormulaCalculo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Guardar fórmula de cálculo?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La fórmula ha sido guardada correctamente.")
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
    $(JsFormulasCalculo.Controles.divDdlCategoriasTipoFecha_modalFechaFormula).css("display", "none");
});

$(document).on("click", JsFormulasCalculo.Controles.radioCategoriaDesagregacion_modalFechaFormula, function () {
    $(JsFormulasCalculo.Controles.divTxtFechaInicio_modalFechaFormula).css("display", "none");
    $(JsFormulasCalculo.Controles.divTxtFechaFinal_modalFechaFormula).css("display", "none");
    $(JsFormulasCalculo.Controles.divDdlCategoriasTipoFecha_modalFechaFormula).css("display", "block");
});