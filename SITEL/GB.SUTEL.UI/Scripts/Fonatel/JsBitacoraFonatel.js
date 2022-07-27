JsBitacora= {
    "Controles": {
        "btnCancelar":"#btnCancelarBitacora"
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}

$(document).on("click", JsBitacora.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/BitacoraFonatel/Index";
        });
});














