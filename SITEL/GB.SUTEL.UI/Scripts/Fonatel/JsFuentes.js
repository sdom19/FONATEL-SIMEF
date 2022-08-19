JsFuentes = {
    "Controles": {
        "btnstep": ".step_navigation_fuentes div",
        "btnGuardarFuente": "#btnGuardarFuente",
        "btnSiguienteFuente":"#btnSiguienteFuente",
        "btnEditarFuente": "#TablaFuentes tbody tr td .btn-edit",
        "btnBorrarFuente": "#TablaFuentes tbody tr td .btn-delete",
        "btnBorrarDetalle": "#TableDetalleFuentes tbody tr td .btn-delete",
        "btnAddFuente": "#TablaFuentes tbody tr td .btn-add",
        "divContenedor": ".divContenedor_fuentes",
        "btnGuardarDestinatario": "#btnGuardarDestinatario",
        "btnGuardarFuentesCompleto": "#btnGuardarFuentesCompleto",
        "btnAtrasFuentes": "#btnAtrasFuentes",
        "btnCancelar": "#btnCancelarFuente",
        "TablaFuentes":"#TablaFuentes tbody"
    },
    "Variables": {
        "ListadoFuentes":[]
    },

    "Metodos": {
        "CargarTablaFuentes": function () {
            let html = "";
            EliminarDatasource();
            for (var i = 0; i < JsFuentes.Variables.ListadoFuentes.length; i++) {

                let fuente = JsFuentes.Variables.ListadoFuentes[i];
                html = html + "<tr><th scope='row'>" + fuente.Fuente + "</th>" +
                    "<td>" + fuente.CantidadDestinatario + "/"+fuente.DetalleFuentesRegistro.length + "</td><td>Activo</td>";
                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value='"+fuente.id+"' title='Editar' class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value='" + fuente.id +"' title='Eliminar' class='btn-icon-base btn-delete'></button></td></tr>";
            }
            $(JsFuentes.Controles.TablaFuentes).html(html);
            CargarDatasource();
            JsFuentes.Variables.ListadoFuentes = [];
        }



    },

    "Consultas": {
        "ConsultaListaFuentes": function () {
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/ObtenerListaFuentes',
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsFuentes.Variables.ListadoFuentes = obj.objetoRespuesta;
                        JsFuentes.Metodos.CargarTablaFuentes();
                    } else  {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                  
                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },




        "EliminarFuente": function (id) {
            let fuente = new Object();

            fuente.id = id;

            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/Fuentes/CambiarEstadoFuentes',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { fuente },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {

                        jsMensajes.Metodos.OkAlertModal("La Fuente ha sido eliminada ")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });


                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }

                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        }
    }

}

$(document).on("click", JsFuentes.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/Fuentes/Index";
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarFuente, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha sido creada")
                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click');  });
        });
});


$(document).on("click", JsFuentes.Controles.btnSiguienteFuente, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');
});



$(document).on("click", JsFuentes.Controles.btnGuardarFuentesCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Fuente ha sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/Fuentes/index" });
        });
});


$(document).on("click", JsFuentes.Controles.btnGuardarDestinatario, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el destinatario a la Fuente?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El destinatario ha sido creado")
                .set('onok', function (closeEvent) { });
        });
});

$(document).on("click", JsFuentes.Controles.btnBorrarFuente, function () {

    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Fuente?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsFuentes.Consultas.EliminarFuente(id);
        });
});


$(document).on("click", JsFuentes.Controles.btnEditarFuente, function () {

    let id = $(this).val();
    window.location.href = "/Fonatel/Fuentes/Create?id=" + id;
});




$(document).on("click", JsFuentes.Controles.btnBorrarDetalle, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Destinatario?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Destinatario ha sido eliminado")
                .set('onok', function (closeEvent) { });
        });
});


$(document).on("click", JsFuentes.Controles.btnAtrasFuentes, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});



$(function () {
    if ($(JsFuentes.Controles.TablaFuentes).length > 0) {
        JsFuentes.Consultas.ConsultaListaFuentes();
    }
})