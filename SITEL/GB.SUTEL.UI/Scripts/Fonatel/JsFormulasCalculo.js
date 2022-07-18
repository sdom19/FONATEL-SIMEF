JsFormulasCalculo= {
    "Controles": {
        "ddlDireccionFormula": "#ddlDireccionFormula",
        "divDireccionFonatel": "div[name='divDireccionFonatel']"
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

$(document).on("change", JsFormulasCalculo.Controles.ddlDireccionFormula, function () {
    alert("sdad");
});
