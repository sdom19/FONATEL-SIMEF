JsRelacion= {
    "Controles": {

        "btnEliminar": ".btnEliminarRelacion",
        "btnGuardar": "#btnGuardarRelacion"
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}


$(document).on("click", JsRelacion.Controles.btnEliminar, function () {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Eliminar la Relación?")
        .then((result) => {
            if (result.isConfirmed) {
                jsMensajes.Metodos.ConfirmaRegistro("La Relación ha sido Eliminada");
            }
        });
});




$(document).on("click", JsRelacion.Controles.btnGuardar, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Relación?")
        .then((result) => {
            if (result.isConfirmed) {
                jsMensajes.Metodos.ConfirmaRegistro("La Relación ha sido Agregada");
            }
        });
});