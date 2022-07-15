jsRegistroIndicadorFonatel= {
    "Controles": {
        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-home",
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



$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnllenadoweb, function () {
    let id = 1;
    window.location.href = "/Fonatel/RegistroIndicadorFonatel/Create?id=" + id;
});


$(document).on("change", jsRegistroIndicadorFonatel.Controles.ddlDireccionFormula, function () {
    alert("sdad");
});
