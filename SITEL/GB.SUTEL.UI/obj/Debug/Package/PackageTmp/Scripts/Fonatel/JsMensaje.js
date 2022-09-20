jsMensajes = {
    "Variables": {
        "MensajeAgregar": "Agregar Registro",
        "MensajeCancelar": "Cancelar Registro",
        "MensajeEstado": "Cambio de Estado",
        "MensajeClonar": "Clonar Registro",
        "MensajeEliminar": "Eliminar Registro",
        "MensajeConfirmacion": "Proceso Exitoso",
        "ErrorTransaccion": "Error",
        "ContentDelete": (mensaje) => { return "<div class='text-center'><div class='icon warning-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "ContentError": (mensaje) => { return "<div class='text-center'><div class='icon warning-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "ContentSuccess": (mensaje) => { return "<div class='text-center'><div class='icon success-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "ContentQuestion": (mensaje) => { return "<div class='text-center'><div class='icon question-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "ContentQuestionStatus": (mensaje) => { return "<div class='text-center'><div class='icon warning-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "btnlisto": "ACEPTAR",
        "btnyes": "SI",
        "btnno": "NO",
        "actionType": {
            agregar: 0,
            clonar: 1,
            eliminar: 2,
            estado: 3,
            cancelar:4
        }

    },
    "Metodos": {
        "OkAlertErrorModal": function (mensaje = null) {
            if (mensaje == null) {
                let alertifyObject = alertify.alert(jsMensajes.Variables.ErrorTransaccion, "")
                    .set('label', jsMensajes.Variables.btnlisto)
                    .set({ 'modal': true, 'closable': false })
                alertifyObject.setContent(jsMensajes.Variables.ContentError("Favor comunicarse con soporte de aplicaciones"));
                return alertifyObject;
            }
            else {
                let alertifyObject = alertify.alert(jsMensajes.Variables.ErrorTransaccion, "")
                    .set('label', jsMensajes.Variables.btnlisto)
                    .set({ 'modal': true, 'closable': false })
                alertifyObject.setContent(jsMensajes.Variables.ContentError(mensaje));
                return alertifyObject;
            }
        },



        //"OkAlertModal": function (mensaje) {
        //    let alertifyObject = alertify.alert(jsMensajes.Variables.MensajeConfirmacion, "")
        //        .set('label', jsMensajes.Variables.btnlisto )
        //        .set({ 'modal': true, 'closable': false })
        //    alertifyObject.setContent(jsMensajes.Variables.ContentSuccess(mensaje));
        //    return alertifyObject;
        //},

        /**
         * Función que permite mostrar un modal de confirmación, incluye las acciones de Si y No, 
         * un título que puede ser customizado o bien utilizar uno genérico, y por último un mensaje a preguntar
         * @param {any} mensaje
         * @param {any} actionType
         * @param {any} customTitleMessage - OPTIONAL
         */
        "ConfirmYesOrNoModal": function (mensaje, actionType, customTitleMessage = null) {
            let _question = "Atención!";

            if (customTitleMessage == null) { // titulo de mensaje generico?
                if (actionType == jsMensajes.Variables.actionType.agregar) { // se busca por medio del actionType
                    _question = jsMensajes.Variables.MensajeAgregar;
                }
                else if (actionType == jsMensajes.Variables.actionType.clonar) {
                    _question = jsMensajes.Variables.MensajeClonar;
                }
                else if (actionType == jsMensajes.Variables.actionType.eliminar) {
                    _question = jsMensajes.Variables.MensajeEliminar;
                }
                else if (actionType == jsMensajes.Variables.actionType.estado) {
                    _question = jsMensajes.Variables.MensajeEstado;
                }
                else if (actionType == jsMensajes.Variables.actionType.cancelar) { // se busca por medio del actionType
                    _question = jsMensajes.Variables.MensajeCancelar;
                }
            }
            else {
                _question = customTitleMessage;
            }
            let alertifyObject = null;
            if (actionType == jsMensajes.Variables.actionType.eliminar) {
                alertifyObject = alertify.confirm(_question, 'Confirm Message', function () { }, function () { })
                    .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                    .set({ 'modal': true, 'closable': true, 'movable': false, transition: 'slide' })
                alertifyObject.setContent(jsMensajes.Variables.ContentDelete(mensaje));
            } else {
                alertifyObject = alertify.confirm(_question, 'Confirm Message', function () { }, function () { })
                    .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                    .set({ 'modal': true, 'closable': true, 'movable': false, transition: 'slide' })
                alertifyObject.setContent(jsMensajes.Variables.ContentQuestion(mensaje));
            }
           

            return alertifyObject;
        },

        "OkAlertModal": function (mensaje) {

            let alertifyObject = alertify.alert(jsMensajes.Variables.MensajeConfirmacion, "")
                .set('label', jsMensajes.Variables.btnlisto)
                .set({ 'modal': true, 'closable': true, 'movable': false, transition: 'slide' })
            alertifyObject.setContent(jsMensajes.Variables.ContentSuccess(mensaje));
            return alertifyObject;
        },

    }
}


$(function () {
    alertify.defaults.transition = "";
    alertify.defaults.theme.ok = "btn btn-fonatel btn-success-fonatel"; //"btn btn-success success-icon-btn btn-base-icon";
    alertify.defaults.theme.cancel = "btn btn-fonatel btn-error-fonatel";
    alertify.defaults.theme.input = "form-control";

})