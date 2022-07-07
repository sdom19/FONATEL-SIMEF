JsFuentes = {
    "Controles": {
        "btnstep": ".step_navigation_fuentes div",
        "btnGuardarFuente": "#btnGuardarFuente",
        "btnEditarFuente": "#TableFuentes tbody tr td .btn-edit",
        "btnBorrarFuente": "#TableFuentes tbody tr td .btn-delete",
        "btnAddFuente": "#TableFuentes tbody tr td .btn-add",
        "divContenedor": ".divContenedor_fuentes",
        "btnGuardarDestinatario": "#btnGuardarDestinatario",
        "btnGuardarFuentesCompleto":"#btnGuardarFuentesCompleto"
    },
    "Variables": {

    },

    "Metodos": {
      
    }

}

$(document).on("click", JsFuentes.Controles.btnGuardarFuente, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Fuente?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Fuente ha Sido Agregada de Manera Correcta")
                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click'); });
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarFuentesCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar la Fuente?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Fuente ha Sido Agregada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarDestinatario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.AgregarRegistro("¿Desea Agregar el Destinatario a la Fuente?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("El Destinatario ha Sido Agregado")
                .set('onok', function (closeEvent) { });
        });
});





$(document).on("click", JsFuentes.Controles.btnBorrarFuente, function () {
    jsMensajes.Metodos.EliminarRegistro("¿Desea Eliminar la Fuente?")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.ConfirmaRegistro("La Fuente ha Sido Eliminada de Manera Correcta")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});


$(document).on("click", JsFuentes.Controles.btnstep, function () {

    $(JsFuentes.Controles.btnstep).children("a").addClass('btn-default').removeClass('btn-info-fonatel');
    $(this).children("a").addClass('btn-info-fonatel').removeClass('btn-default');
    let div = $(this).children("a").attr("href");
    $(JsFuentes.Controles.divContenedor).addClass('hidden');
    $(div).removeClass('hidden');
});
