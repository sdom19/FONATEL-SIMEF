JsRelacion= {
    "Controles": {

        "btnEliminar": ".btnEliminarRelacion",
        "btnGuardar": "#btnGuardarRelacion",
        "btnAgregarRelacion": "#TablaRelacionCategoria tbody tr td .btn-add",
        "btnEditarRelacion": "#TablaRelacionCategoria tbody tr td .btn-edit",
        "btnDeleteRelacion": "#TablaRelacionCategoria tbody tr td .btn-delete",
        "btnEliminarDetalleRelacion": "#TablaDetalleRelacionCategoria tbody tr td .btn-delete",
        "btnGuardarDetalle":"#btnGuardarDetalle"
    },
    "Variables":{

    },

    "Metodos": {
   
    }

}


$(document).on("click", JsRelacion.Controles.btnEliminar, function () {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Eliminar la Relación?");
});

$(document).on("click", JsRelacion.Controles.btnAgregarRelacion, function () {
    let id = 1;
    window.location.href = "/Fonatel/RelacionCategoria/Detalle?id=" + id;
});

$(document).on("click", JsRelacion.Controles.btnEditarRelacion, function () {
    let id = 1;
    window.location.href = "/Fonatel/RelacionCategoria/Create?id=" + id;
});



$(document).on("click", JsRelacion.Controles.btnGuardar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Relación?");
});

$(document).on("click", JsRelacion.Controles.btnGuardarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Relacionar la Categoría?");
});

$(document).on("click", JsRelacion.Controles.btnEliminarDetalleRelacion, function (e) {
    jsMensajes.Metodos.AgregarRegistro("¿Desea Elimina el Atributo?");
});

$(document).on("click", JsRelacion.Controles.btnDeleteRelacion, function (e) {
    jsMensajes.Metodos.AgregarRegistro("¿Desea Elimina la Relación?");
});