JsDefiniciones= {
    "Controles": {
        
        "btnEditarDefiniciones": "#TablaDefiniciones tbody tr td .btn-edit",
        "btnDeleteDefiniciones": "#TablaDefiniciones tbody tr td .btn-delete",
        "btnCloneDefiniciones": "#TablaDefiniciones tbody tr td .btn-clone",
        "btnGuardar": "#btnGuardarDefiniciones",
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}




$(document).on("click", JsDefiniciones.Controles.btnEditarDefiniciones, function () {
    let id = 1;
    window.location.href = "/Fonatel/DefinicionIndicadores/Create?id=" + id;
});

$(document).on("click", JsDefiniciones.Controles.btnCloneDefiniciones, function () {
    let id = 1;
    window.location.href = "/Fonatel/DefinicionIndicadores/Create?id=" + id;
});






$(document).on("click", JsDefiniciones.Controles.btnGuardar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Definicion?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Definición ha Sido Creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
        });
});

$(document).on("click", JsDefiniciones.Controles.btnDeleteDefiniciones, function (e) {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Elimina la Definición? ")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Definición ha sido Eliminada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
        });
});









