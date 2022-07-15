JsDescargaFormularioWeb= {
    "Controles": {
        "btndescarga": "#btnDescargaFormularioWeb",
        "btnCancela": "#btnCancelaFormularioWeb"
    },
    "Variables": {
   
        
    },

    "Metodos": {
   
    }

}



$(document).on("click", JsDescargaFormularioWeb.Controles.btndescarga, function () {
    debugger;
    $("#divLoading").fadeIn();
});


$(document).on("click", JsDescargaFormularioWeb.Controles.btnCancela, function () {
    window.location.href = "/DescargaFormulario/Create";
});

