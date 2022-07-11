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
        "divFormulaActualizacionSecuencial": "#divFormulaActualizacionSecuencial",
         "divContenedor": ".contenedor_regla",
            "btnAtrasRegla": "#btnAtrasTipoRegla",
            "btnSiguienteTipoSiguiente": "#btnSiguienteTipoSiguiente",
            "btnGuardarReglaTipo": "#btnGuardarReglaTipo",
      
     },
    "Variables":{
        "FormulaCambioMensual":"1",
        "FormulaContraIndicador":"2",
        "FormulaContraConstante":"3",
        "FormulaContraAtributosValido":"4",
        "FormulaActualizacionSecuencial": "5",
    },

        "Metodos": {
            "SeleccionarStep": function (div) {
                $(JsReglas.Controles.divContenedor).addClass('hidden');
                $(div).removeClass('hidden');
            },
            "HabilitarControlesTipoRegla": function (selected) {
            $(JsReglas.Controles.divFormulaCambioMensual).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraIndicador).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraConstante).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraAtributosValido).addClass("hidden");
            $(JsReglas.Controles.divFormulaActualizacionSecuencial).addClass("hidden");
            switch (selected) {
                case JsReglas.Variables.FormulaContraIndicador:
                    $(JsReglas.Controles.divFormulaContraIndicador).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraConstante:
                    $(JsReglas.Controles.divFormulaContraConstante).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraAtributosValido:
                    $(JsReglas.Controles.divFormulaContraAtributosValido).removeClass("hidden");
                    break;
                default:
            }     
        }
    }
}



$(document).on("click", JsReglas.Controles.btnEditarRegla, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id;
});

$(document).on("click", JsReglas.Controles.btnGuardarReglaTipo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar el Tipo de Regla?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("Se ha Configurado una regla a la Variable")
                .set('onok', function (closeEvent) {
               
                });
        });
});



$(document).on("click", JsReglas.Controles.btnGuardarRegla, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Regla?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("Se ha Configurado una regla a la Variable")
                .set('onok', function (closeEvent) {
                    $("a[href='#step-2']").trigger('click');
                });
        });
});





$(document).on("click", JsReglas.Controles.btnClonarRegla, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Clonar la Regla?", jsMensajes.Variables.actionType.clonar )
        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id});
});



$(document).on("click", JsReglas.Controles.btnBorrarRegla, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Eliminar la Regla?",jsMensajes.Variables. actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Regla ha Sido Eliminada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/index" });
        });
});




$(document).on("change", JsReglas.Controles.ddlTipoRegla, function () {
    var selected = $(this).val();
    JsReglas.Metodos.HabilitarControlesTipoRegla(selected);
});

$(document).on("click", JsReglas.Controles.btnAtrasRegla, function (e) {
    e.preventDefault();
   $("a[href='#step-1']").trigger('click');
});



$(document).on("click", JsReglas.Controles.btnSiguienteTipoSiguiente, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Agregar la Regla de Validación?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Regla ha Sido Agregada de Manera Correcta")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/ReglasValidacion/index"
                });
        });
});

