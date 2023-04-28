jsMensajes = {
    Variables: {
        MensajeAgregar: "Agregar Registro",
        MensajeCancelar: "Cancelar Registro",
        MensajeEstado: "Cambio de Estado",
        MensajeClonar: "Clonar Registro",
        MensajeEliminar: "Eliminar Registro",
        MensajeConfirmacion: "Proceso Exitoso",
        MensajeDescargarRegistro: "Descargar Registro",
        MensajeCargarRegistro: "Cargar Registro",
        MensajeEjecutar: "Ejecutar Registro",
        ErrorTransaccion: "Error",
        ContentDelete: (mensaje) => { return "<div class='text-center'><div class='icon warning-icon'></div> <strong>" + mensaje + "</strong></div>" },
        ContentError: (mensaje) => { return "<div class='text-center'><div class='icon warning-icon'></div> <strong>" + mensaje + "</strong></div>" },
        ContentSuccess: (mensaje) => { return "<div class='text-center'><div class='icon success-icon'></div> <strong>" + mensaje + "</strong></div>" },
        ContentQuestion: (mensaje) => { return "<div class='text-center'><div class='icon question-icon'></div> <strong>" + mensaje + "</strong></div>" },
        ContentQuestionStatus: (mensaje) => { return "<div class='text-center'><div class='icon warning-icon'></div> <strong>" + mensaje + "</strong></div>" },
        btnlisto: "ACEPTAR",
        btnyes: "SI",
        btnno: "NO",
        actionType: {
            agregar: 0,
            clonar: 1,
            eliminar: 2,
            estado: 3,
            cancelar: 4,
            descargar: 5,
            cargar: 6,
            ejecutar: 7
        }

    },
    Metodos: {
        OkAlertErrorModal: function (mensaje = null) {
            if (mensaje == null) {
                let alertifyObject = alertify.alert(jsMensajes.Variables.ErrorTransaccion, "")
                    .set('label', jsMensajes.Variables.btnlisto)
                    .set({ 'modal': true, 'closable': false })
                alertifyObject.setContent(jsMensajes.Variables.ContentError("Favor comunicarse con soporte de aplicaciones"));
                let buttons = $(".btn.btn-fonatel.btn-success-fonatel.custom-tooltip.custom-tooltip-yes");
                buttons.last().removeClass("custom-tooltip-yes").addClass("custom-tooltip-aceptar");
                return alertifyObject;
            }
            else {
                let alertifyObject = alertify.alert(jsMensajes.Variables.ErrorTransaccion, "")
                    .set('label', jsMensajes.Variables.btnlisto)
                    .set({ 'modal': true, 'closable': false })
                alertifyObject.setContent(jsMensajes.Variables.ContentError(mensaje));
                let buttons = $(".btn.btn-fonatel.btn-success-fonatel.custom-tooltip.custom-tooltip-yes");
                buttons.last().removeClass("custom-tooltip-yes").addClass("custom-tooltip-aceptar");
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
        ConfirmYesOrNoModal: function (mensaje, actionType, customTitleMessage = null) {
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
                else if (actionType == jsMensajes.Variables.actionType.descargar) { // se busca por medio del actionType
                    _question = jsMensajes.Variables.MensajeDescargarRegistro;
                }
                else if (actionType == jsMensajes.Variables.actionType.cargar) {
                    _question = jsMensajes.Variables.MensajeCargarRegistro;
                }
                else if (actionType == jsMensajes.Variables.actionType.ejecutar) {
                    _question = jsMensajes.Variables.MensajeEjecutar;
                }
            }
            else {
                _question = customTitleMessage;
            }

            let alertifyObject = null;

            if (actionType == jsMensajes.Variables.actionType.eliminar) {
                alertifyObject = alertify.confirm(_question, 'Confirm Message', function () { }, function () { })
                    .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                    .set({ 'modal': true, 'closable': true, 'movable': false})
                alertifyObject.setContent(jsMensajes.Variables.ContentDelete(mensaje));
            } else if (actionType == jsMensajes.Variables.actionType.descargar) {

                alertifyObject = alertify.confirm(_question, 'Confirm Message', function () { }, function () { })
                    .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                    .set({ 'modal': true, 'closable': true, 'movable': false })
                alertifyObject.setContent(jsMensajes.Variables.ContentQuestion(mensaje));

            }
            else {
                alertifyObject = alertify.confirm(_question, 'Confirm Message', function () { }, function () { })
                    .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                    .set({ 'modal': true, 'closable': true, 'movable': false })
                alertifyObject.setContent(jsMensajes.Variables.ContentQuestion(mensaje));
            }

            let buttons = $(".btn.btn-fonatel.btn-success-fonatel.custom-tooltip");
            buttons.last().removeClass("custom-tooltip-aceptar").addClass("custom-tooltip-yes");

            $(".ajs-close").last().addClass("custom-tooltip custom-tooltip-close");
            return alertifyObject;
        },

        OkAlertModal: function (mensaje) {

            let alertifyObject = alertify.alert(jsMensajes.Variables.MensajeConfirmacion, "")
                .set('label', jsMensajes.Variables.btnlisto)
                .set({ 'modal': true, 'closable': true, 'movable': false })
            alertifyObject.setContent(jsMensajes.Variables.ContentSuccess(mensaje));

            let buttons = $(".btn.btn-fonatel.btn-success-fonatel.custom-tooltip.custom-tooltip-yes");
            buttons.last().removeClass("custom-tooltip-yes").addClass("custom-tooltip-aceptar");
            $(".ajs-close").last().addClass("custom-tooltip custom-tooltip-close");

            return alertifyObject;
        },
    }
}


$(function () {
    alertify.defaults.transitionOff = true;
    alertify.defaults.transition = "";
    alertify.defaults.theme.ok = "btn btn-fonatel btn-success-fonatel custom-tooltip custom-tooltip-yes"; //"btn btn-success success-icon-btn btn-base-icon";
    alertify.defaults.theme.cancel = "btn btn-fonatel btn-error-fonatel custom-tooltip custom-tooltip-no";
    alertify.defaults.theme.input = "form-control";
});