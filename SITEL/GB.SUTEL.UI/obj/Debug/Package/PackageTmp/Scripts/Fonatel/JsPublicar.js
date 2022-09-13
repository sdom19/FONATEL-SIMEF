JsPublicar= {
    "Controles": {

        "btnPublicar": "#TablaPublicar tbody tr td .btn-power-on",
        "btnEliminarPublicacion": "#TablaPublicar tbody tr td .btn-power-off",
        "TablaPublicar": "#TablaPublicar tbody"



    },
    "Variables":{
        "ListaIndicadores":[]
    },

    "Metodos": {
        "CargarTablaPublicaciones": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsPublicar.Variables.ListaIndicadores.length; i++) {
                let Publicacion = JsPublicar.Variables.ListaIndicadores[i];

                html = html + "<tr><th>" + Publicacion.Codigo + "</th><td>" + Publicacion.Nombre + "</td><td>" + Publicacion.GrupoIndicadores.Nombre + "</td><td>" + Publicacion.TipoIndicadores.Nombre + "</td>";
                if (Publicacion.VisualizaSigitel) {
                    html = html + "<td>NO</td><td><button type='button' data-toggle='tooltip' data-placement='top' value=" + Publicacion.id+" title='Publicar' class='btn-icon-base btn-power-off'></button></td>";
                }
                else {
                    html = html + "<td>SI</td><td><button type='button' data-toggle='tooltip' data-placement='top' value=" + Publicacion.id +" title='Eliminar Publicado' class='btn-icon-base btn-power-on'></button></td>";
                }
                html = html + "</tr>";
            }
            $(JsPublicar.Controles.TablaPublicar).html(html);
            CargarDatasource();
            JsPublicar.Variables.ListaIndicadores = [];
        }

      
    },
    "Consultas": {
        "ConsultaListaPublicaciones": function () {
            $("#loading").fadeIn();
            execAjaxCall("/PublicacionIndicadores/ObtenerListaIndicadores", "GET")
                .then((data) => {
                    JsPublicar.Variables.ListaIndicadores = data.objetoRespuesta;
                    JsPublicar.Metodos.CargarTablaPublicaciones();
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
        "CambiarEstadoSigitel": function (id, estado) {
            $("#loading").fadeIn();
            let indicador = new Object();
            indicador.id = id;
            indicador.VisualizaSigitel = estado;

            execAjaxCall("/PublicacionIndicadores/CambiarEstadoSigitel", "POST", indicador)
                .then((obj) => {

                    if (estado) {
                        jsMensajes.Metodos.OkAlertModal("El Indicador ha sido publicado")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertModal("La publicación ha sido desactivada")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/PublicacionIndicadores/index" });
                    }
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {
                                location.reload();
                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },
    }
    

}

$(document).on("click", JsPublicar.Controles.btnEliminarPublicacion, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea publicar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsPublicar.Consultas.CambiarEstadoSigitel(id, true);         
        });
});


$(document).on("click", JsPublicar.Controles.btnPublicar, function (e) {
    e.preventDefault();
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la publicación del Indicador?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsPublicar.Consultas.CambiarEstadoSigitel(id, false);
        });
});

$(function () {
    if ($(JsPublicar.Controles.TablaPublicar).length > 0) {
        JsPublicar.Consultas.ConsultaListaPublicaciones();
    }

})