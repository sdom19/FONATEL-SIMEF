jsRegistroIndicadorFonatel= {
    "Controles": {
        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-home",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        //control para las categorías
        "ControlListaCategorias": (id, option) => `<select class="form-control form-control-fonatel" id="${id}" aria-label="Default select example">
                                <option>Seleccione</option>${option}</select>`
        // fin 
    },
    "Variables": {
        "VariableIndicador": 1
        
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

$(document).on("keypress", "#txtCantidadRegistroIndicador", function () {
    if (event.keyCode == 13) {
        var table = $('div.tab-pane.active .table-wrapper-fonatel table').DataTable();
        table.clear().draw();

        if ($(this).val() != 0 || $(this).val().trim() != "") {
            for (let x = 0; x < $(this).val(); x++) {
                let listaColumnasVariablesDato = [];

                $("div.tab-pane.active .table-wrapper-fonatel table thead tr").children().each(function (index) {
                    if ($(this).attr('class').includes("highlighted")) {
                        listaColumnasVariablesDato.push(1);
                    }
                    else {
                        listaColumnasVariablesDato.push(jsRegistroIndicadorFonatel.Controles.ControlListaCategorias(`ddlTabla-${x}-${index}`, '<option value="1">Opción 1</option><option value="2">Opción 2</option>'));
                    }
                });
                table.row.add(listaColumnasVariablesDato).draw(false);
            }
        }
    }
});

