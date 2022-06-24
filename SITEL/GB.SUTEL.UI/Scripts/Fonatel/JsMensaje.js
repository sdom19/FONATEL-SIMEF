jsMensajes = {
    "Variables": {
        "MensajeAgregar":"Agregar Registro"
    },
    "Metodos": {
        "AgregarRegistro": function (mensaje) {
            return Swal.fire({
                icon: 'question',
                title: 'Agregar Registro',
                html: '<strong>'+ mensaje+ '</strong>',
                showDenyButton: true,
                confirmButtonText: 'SI',
                confirmButtonColor: '#5B9150',
                customClass: { popup: "swal2-border-radius" },

                denyButtonText: 'NO',
            });

        },
        "ConfirmaRegistro": function (mensaje) {
            return Swal.fire({
                icon: 'success',
                text: mensaje,
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#5B9150',
            });
        },
        "EliminarRegistro": function (mensaje) {
            return Swal.fire({
                icon: 'warning',
                title: 'Eliminar Registro',
                html: '<strong>' + mensaje + '</strong>',
                showDenyButton: true,
                confirmButtonText: 'SI',
                confirmButtonColor: '#5B9150',
                denyButtonText: 'NO',
            });
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