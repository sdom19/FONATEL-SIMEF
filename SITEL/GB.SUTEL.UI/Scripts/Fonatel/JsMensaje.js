jsMensajes = {
    "Variables": {
        "MensajeAgregar": "Agregar Registro",
        "MensajeClonar": "Clonar Registro",
        "MensajeEliminar": "Eliminar Registro",
        "MensajeConfirmacion": "Proceso Exitoso",
        "ErrorTransaccion": "Ocurrio un Error",
        "ContentSuccess": (mensaje) => { return "<div class='text-center'><div class='icon success-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "ContentQuestion": (mensaje) => { return "<div class='text-center'><div class='icon question-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "ContentQuestionStatus": (mensaje) => { return "<div class='text-center'><div class='icon warning-icon'></div> <strong>" + mensaje + "</strong></div>" },
        "btnlisto": "<i class='glyphicon glyphicon-ok' aria-hidden='true' ></i > Listo",
        "btnyes": "<i class='glyphicon glyphicon-ok' aria-hidden='true' ></i > SI",
        "btnno": "<i class='glyphicon glyphicon-remove' aria-hidden='true' ></i > NO"

    },
    "Metodos": {
        "AgregarRegistro": function (mensaje) {
            let alertifyObject = alertify.confirm(jsMensajes.Variables.MensajeAgregar, 'Confirm Message', function () {}, function () {})
                .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                .set({ 'modal': true, 'closable': false })
            alertifyObject.setContent(jsMensajes.Variables.ContentQuestion(mensaje));
            return alertifyObject;
        },

        "ClonarRegistro": function (mensaje) {
            let alertifyObject = alertify.confirm(jsMensajes.Variables.MensajeClonar, 'Confirm Message', function () { }, function () { })
                .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                .set({ 'modal': true, 'closable': false })
            alertifyObject.setContent(jsMensajes.Variables.ContentQuestion(mensaje));
            return alertifyObject;
        },





        "ConfirmaRegistro": function (mensaje) {
            let alertifyObject = alertify.alert(jsMensajes.Variables.MensajeConfirmacion, "", function () { })
                .set('label', jsMensajes.Variables.btnlisto )
                .set({ 'modal': true, 'closable': false })
            alertifyObject.setContent(jsMensajes.Variables.ContentSuccess(mensaje));
            return alertifyObject;
        },
        "EliminarRegistro": function (mensaje) {
            let alertifyObject = alertify.confirm(jsMensajes.Variables.MensajeEliminar, 'Confirm Message', function () { }, function () { })
                .set('labels', { ok: jsMensajes.Variables.btnyes, cancel: jsMensajes.Variables.btnno })
                .set({ 'modal': true, 'closable': false })
            alertifyObject.setContent(jsMensajes.Variables.ContentQuestion(mensaje));

            return alertifyObject;
        },

        "CambiarEstadoRegistro": function (mensaje) {
            return Swal.fire({
                icon: 'warning',
                title: 'Cambio de Estado',
                html: '<strong>' + mensaje + '</strong>',
                showDenyButton: true,
                confirmButtonText: 'SI',
                confirmButtonColor: '#5B9150',
                denyButtonText: 'NO',
            });
        },

        "Error": function (mensaje) {
            return Swal.fire({
                icon: 'error',
                title: 'Se Produjo un Error',
                html: '<strong>' + mensaje + '</strong>',
                showDenyButton: true,
                confirmButtonText: 'SI',
                confirmButtonColor: '#5B9150',
                denyButtonText: 'NO',
            });
        },

    }
}


$(function () {
    alertify.defaults.transition = "slide";
    alertify.defaults.theme.ok = "btn btn-success";
    alertify.defaults.theme.cancel = "btn btn-danger";
    alertify.defaults.theme.input = "form-control";

})