    JsReglas= {
    "Controles": {

        "ddlTipoRegla": "#ddlTipoRegla",
        "btnGuardarRegla": "#btnGuardarRegla",
        "btnEditarRegla": "#TableReglaDesagregacion tbody tr td .btn-edit",
        "btnClonarRegla": "#TableReglaDesagregacion tbody tr td .btn-clone",
        "btnBorrarRegla": "#TableReglaDesagregacion tbody tr td .btn-delete",
        "btnAddRegla": "#TableReglaDesagregacion tbody tr td .btn-add",
        "btnRemoveReglaDetalle": "#TableReglaDesagregacionDetalle tbody tr td .btn-delete",
        "divFormulaCambioMensual":"#divFormulaCambioMensual",
        "divFormulaContraIndicador":"#divFormulaContraIndicador",
        "divFormulaContraConstante":"#divFormulaContraConstante",
        "divFormulaContraAtributosValido": "#divFormulaContraAtributosValido",
        "divFormulaActualizacionSecuencial":"#divFormulaActualizacionSecuencial"



    },
    "Variables":{
        "FormulaCambioMensual":"1",
        "FormulaContraIndicador":"2",
        "FormulaContraConstante":"3",
        "FormulaContraAtributosValido":"4",
        "FormulaActualizacionSecuencial":"5"
    },

    "Metodos": {
        "HabilitarControlesTipoRegla": function (selected) {
            $(JsReglas.Controles.divFormulaCambioMensual).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraIndicador).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraConstante).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraAtributosValido).addClass("hidden");
            $(JsReglas.Controles.divFormulaActualizacionSecuencial).addClass("hidden");

            switch (selected) {
                case JsReglas.Variables.FormulaCambioMensual:
                    $(JsReglas.Controles.divFormulaCambioMensual).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraIndicador:
                    $(JsReglas.Controles.divFormulaContraIndicador).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraConstante:
                    $(JsReglas.Controles.divFormulaContraConstante).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraAtributosValido:
                    $(JsReglas.Controles.divFormulaContraAtributosValido).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaActualizacionSecuencial:
                    $(JsReglas.Controles.divFormulaActualizacionSecuencial).removeClass("hidden");

                default:
            }
          

            
        }
    }

}
$(document).on("change", JsReglas.Controles.ddlTipoRegla, function () {
    var selected = $(this).val();

 
    JsReglas.Metodos.HabilitarControlesTipoRegla(selected);
});


$(document).on("click", JsReglas.Controles.btnEditarRegla, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id;
});









$(document).on("click", JsReglas.Controles.btnGuardarRegla, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Regla de Validación?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Regla ha Sido Agregada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/index"});
        });
});


$(document).on("click", JsReglas.Controles.btnClonarRegla, function () {
    let id = 1;
    jsMensajes.Metodos.ClonarRegistro("¿Desea Clonar la Regla?")
        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id});
});



$(document).on("click", JsReglas.Controles.btnBorrarRegla, function () {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Eliminar la Regla?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Regla ha Sido Eliminada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/index" });
        });
});