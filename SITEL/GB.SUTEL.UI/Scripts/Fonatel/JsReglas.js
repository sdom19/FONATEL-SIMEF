    JsReglas= {
    "Controles": {
            "ddlVariableRegla":"#ddlVariableRegla",
            "ddlTipoRegla": "#ddlTipoRegla",
            "btnGuardarRegla": "#btnGuardarRegla",
            "btnCancelar":"#btnCancelarRegla",
            "btnSiguienteRegla":"#btnSiguienteRegla",
            "btnEditarRegla": "#TableReglaDesagregacion tbody tr td .btn-edit",
            "btnClonarRegla": "#TableReglaDesagregacion tbody tr td .btn-clone",
            "btnBorrarRegla": "#TableReglaDesagregacion tbody tr td .btn-delete",
            "btnEliminaTipoRegla":"#TableTipoRegla tbody tr td .btn-delete",
            "btnAddRegla": "#TableReglaDesagregacion tbody tr td .btn-add",
            "btnRemoveReglaDetalle": "#TableReglaDesagregacionDetalle tbody tr td .btn-delete",
            "divFormulaCambioMensual":"#divFormulaCambioMensual",
            "divFormulaContraIndicador": "#divFormulaContraIndicador",
            "divFormulaContraIndicadorSalida": "#divFormulaContraIndicadorSalida",
            "divFormulaContraIndicadorEntradaSalida": "#divFormulaContraIndicadorEntradaSalida",
            "divFormulaContraConstante":"#divFormulaContraConstante",
            "divFormulaContraAtributosValido": "#divFormulaContraAtributosValido",
            "divFormulaActualizacionSecuencial": "#divFormulaActualizacionSecuencial",
            "divContenedor": ".contenedor_regla",
            "btnAtrasRegla": "#btnAtrasTipoRegla",
            "btnSiguienteTipoSiguiente": "#btnSiguienteTipoSiguiente",
            "btnGuardarReglaTipo": "#btnGuardarReglaTipo",
            "ddlVariableRegla":"#ddlVariableRegla"
      
     },
    "Variables":{
        "FormulaCambioMensual":"1",
        "FormulaContraIndicador":"2",
        "FormulaContraConstante":"3",
        "FormulaContraAtributosValido":"4",
        "FormulaActualizacionSecuencial": "5",
        "FormulaContraIndicadorSalida": "6",
        "FormulaContraIndicadorEntradaSalida": "7",
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
                $(JsReglas.Controles.ddlVariableRegla).prop("disabled", false);
                $(JsReglas.Controles.divFormulaActualizacionSecuencial).addClass("hidden");
                $(JsReglas.Controles.divFormulaContraIndicadorSalida).addClass("hidden");
                $(JsReglas.Controles.divFormulaContraIndicadorEntradaSalida).addClass("hidden");

            switch (selected) {
                case JsReglas.Variables.FormulaContraIndicador:
                    $(JsReglas.Controles.divFormulaContraIndicador).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraConstante:
                    $(JsReglas.Controles.divFormulaContraConstante).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraAtributosValido:
                    $(JsReglas.Controles.divFormulaContraAtributosValido).removeClass("hidden");
                    $(JsReglas.Controles.ddlVariableRegla).val('...').change();
                    $(JsReglas.Controles.ddlVariableRegla).prop("disabled", true);
                    break;
                case JsReglas.Variables.FormulaActualizacionSecuencial:
                    $(JsReglas.Controles.divFormulaActualizacionSecuencial).removeClass("hidden");
                    break;

                case JsReglas.Variables.FormulaContraIndicadorSalida:
                    $(JsReglas.Controles.divFormulaContraIndicadorSalida).removeClass("hidden");
                    break;

                case JsReglas.Variables.FormulaContraIndicadorEntradaSalida:
                    $(JsReglas.Controles.divFormulaContraIndicador).removeClass("hidden");
                    break;

                default:
            }     
        }
    }
}

$(document).on("click", JsReglas.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/ReglasValidacion/Index";
        });
});


$(document).on("click", JsReglas.Controles.btnEditarRegla, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id;
});

$(document).on("click", JsReglas.Controles.btnGuardarReglaTipo, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el Tipo de Regla?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Tipo de Regla ha sido agregado")
                .set('onok', function (closeEvent) {
               
                });
        });
});


$(document).on("click", JsReglas.Controles.btnEliminaTipoRegla, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar  el Tipo de Regla?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Tipo de Regla ha sido eliminada")
                .set('onok', function (closeEvent) {

                });
        });
});



$(document).on("click", JsReglas.Controles.btnGuardarRegla, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial de la Regla?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("Se ha configurado una regla a la Variable")
                .set('onok', function (closeEvent) {
                    $("a[href='#step-2']").trigger('click');
                });
        });
});


$(document).on("click", JsReglas.Controles.btnSiguienteRegla, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');

});




$(document).on("click", JsReglas.Controles.btnClonarRegla, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Regla?", jsMensajes.Variables.actionType.clonar )
        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id});
});



$(document).on("click", JsReglas.Controles.btnBorrarRegla, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Regla?",jsMensajes.Variables. actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Regla ha sido eliminada ")
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
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Regla de Validación?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Regla ha sido agregada ")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/ReglasValidacion/index"
                });
        });
});

