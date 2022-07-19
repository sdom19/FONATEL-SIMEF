JsFormulasCalculo = {
    "Controles": {
        "ddlFuenteIndicador": "#ddlFuenteIndicador",
        "divDireccionFonatel": "div[name='divDireccionFonatel']",
        "modalFormulaDetalleAgregacion": "#modalFormulaDetalleAgregacion",
        "btnAgregarDetalleAgregacion": "#btnAgregarDetalleAgregacion",
        "radioCategoriaDesagregacion": "#radioCategoriaDesagregacion",
        "radioTotal": "#radioTotal",
        "divInputCategoriaDesagregacion": "#divInputCategoriaDesagregacion",
        "divInputDetalleCategoriaDesagregacion": "#divInputDetalleCategoriaDesagregacion"
    },
    "Variables": {
        "Direccion": {
            "FONATEL": 1,
            "MERCADOS": 2,
            "CALIDAD":3
        }
        
    },

    "Metodos": {
   
    }

}

$(document).on("change", JsFormulasCalculo.Controles.ddlFuenteIndicador, function () {
    if ($(this).select2('data')[0].id == "1") {
        $(JsFormulasCalculo.Controles.divDireccionFonatel).css("display", "block");
    }
    else {
        $(JsFormulasCalculo.Controles.divDireccionFonatel).css("display", "none");
    }
});

$(document).on("click", JsFormulasCalculo.Controles.btnAgregarDetalleAgregacion, function () {
    $(JsFormulasCalculo.Controles.modalFormulaDetalleAgregacion).modal('show');
});

$(document).on("change", JsFormulasCalculo.Controles.radioCategoriaDesagregacion, function () {
    $(JsFormulasCalculo.Controles.divInputCategoriaDesagregacion).css("display", "block");
    $(JsFormulasCalculo.Controles.divInputDetalleCategoriaDesagregacion).css("display", "block");
});

$(document).on("change", JsFormulasCalculo.Controles.radioTotal, function () {
    $(JsFormulasCalculo.Controles.divInputCategoriaDesagregacion).css("display", "none");
    $(JsFormulasCalculo.Controles.divInputDetalleCategoriaDesagregacion).css("display", "none");
});
