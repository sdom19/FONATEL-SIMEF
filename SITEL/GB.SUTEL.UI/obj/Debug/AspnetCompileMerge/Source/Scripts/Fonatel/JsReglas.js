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
        "btnAgregarTipoRegla": ".btnAgregarTipoRegla",
        "detalleTipoRegla":"#detalleTipoRegla"

      

    },
    "FomularioTipo": {
        "divtipo": (id) => {
            return `<div class="mt-5">
            <div class="row">
                <div class="col-md-4 col-sm-12">

                    <div class="form-group row">
                        <label for="ddlTipoRegla" class="col-sm-4 col-form-label ">Tipo Regla</label>
                        <div class="col-sm-8">
                            <select class="form-control required listasDesplegables" aria-label="Default select example" id="ddlTipoRegla">
                                <option></option>
                                <option value="1">Fórmula cambio mensual</option>
                                <option value="2">Fórmula contra otro indicador</option>
                                <option value="3">Fórmula contra constante</option>
                                <option value="4">Fórmula contra atributos válidos</option>
                                <option value="5">Fórmula actualización secuencial</option>
                            </select>
                            <br />
                            <small id="TipoReglaHelp" class="form-text text-danger">El tipo es requerido</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-12">
                    <div class="form-group row">
                        <label for="ddlVariableRegla" class="col-sm-4 col-form-label">Variable</label>
                        <div class="col-sm-8">
                            <select class="form-control required listasDesplegables" id="ddlVariableRegla">
                                <option></option>
                                <option value="1">Cantidad Torres</option>
                                <option value="2">Estado de Torres</option>
                            </select>
                            <br />
                            <small id="VariableHelp" class="form-text text-danger">El Indicador es requerido</small>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-12">
                    <div class="form-group row">
                        <label for="ddlOperadorRegla" class="col-sm-4 col-form-label">Operador</label>
                        <div class="col-sm-8">
                            <select class="form-control required listasDesplegables" aria-label="Default select example" id="ddlOperadorRegla">
                                <option></option>
                                <option value="1">igual</option>
                                <option value="2">menor que</option>
                                <option value="3">mayor que</option>
                                <option value="4">menor o igual que</option>
                                <option value="5">mayor o igual que</option>
                                <option value="6">diferente</option>
                            </select>
                            <br />
                            <small id="OperadorHelp" class="form-text text-danger">El Indicador es requerido</small>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="hidden" id="divFormulaCambioMensual">
                        Fórmula cambio mensual
                    </div>
                    <div class="hidden" id="divFormulaContraIndicador">
                        Fórmula contra otro indicador
                    </div>
                    <div class="hidden" id="divFormulaContraConstante">
                        Fórmula contra constante
                    </div>
                    <div class="hidden" id="divFormulaContraAtributosValido">
                        Fórmula contra atributos válidos
                    </div>
                    <div class="hidden" id="divFormulaActualizacionSecuencial">
                        Fórmula actualización secuencial
                    </div>
                </div>
                <div class="col-sm-12 text-center">
                    <button class="btn btn-fonatel  btn-info-fonatel btnAgregarTipoRegla">Nueva Regla</button>
                </div>
            </div>
        </div>`
        },
     },
    "Variables":{
        "FormulaCambioMensual":"1",
        "FormulaContraIndicador":"2",
        "FormulaContraConstante":"3",
        "FormulaContraAtributosValido":"4",
        "FormulaActualizacionSecuencial": "5",
        "CantidadVariable": 0,
        "detalleReglastipo":"#detalleReglastipo"
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
                    break;
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

$(document).on("click", JsReglas.Controles.btnAgregarTipoRegla, function (e) {
    e.preventDefault();
    $(JsReglas.Controles.detalleTipoRegla).append(JsReglas.FomularioTipo.divtipo($(this).val()));
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