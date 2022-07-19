jsRegistroIndicadorFonatel= {
    "Controles": {
        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-home",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        //control para las categorías
        "ControlListaCategorias": (id, option) => `<div class="select2-wrapper">
        <select class="listasDesplegables" id = "${id}" >
        <option></option>${option}</select ></div >`
        // fin 
    },
    "Variables": {
        "VariableIndicador":1
        
    },

    "Metodos": {
        "GuardarEncabezadFormulario": function(){

        }
    }

}



$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnllenadoweb, function () {
    let id = 1;
    window.location.href = "/Fonatel/RegistroIndicadorFonatel/Create?id=" + id;
});
