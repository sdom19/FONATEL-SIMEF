jsRegistroIndicadorFonatel= {
    "Controles": {
        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-home",

    },
    "Variables":{

    },

    "Metodos": {
   
    }

}



$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnllenadoweb, function () {
    let id = 1;
    window.location.href = "/Fonatel/RegistroIndicadorFonatel/Create?id=" + id;
});

