    JsIndicador = {
        "Controles": {
            "btnstep": ".step_navigation_indicador div",
            "divContenedor": ".stepwizard-content-container",
            "btnGuardarIndicador": "#btnGuardarIndicador",
            "btnFinalizar":"#btnFinalizarIndicador",
            "btnSiguienteIndicador":"#btnSiguienteIndicador",
            "btnGuardarVariable": "#btnGuardarVariable",
            "btnGuardarCategoría": "#btnGuardarCategoría",
            "tablaIndicador": "#TableIndicador tbody",
            "btnEditarIndicador": "#TableIndicador tbody tr td .btn-edit",
            "btnDesactivarIndicador": "#TableIndicador tbody tr td .btn-power-off",
            "btnActivarIndicador": "#TableIndicador tbody tr td .btn-power-on",
            "btnEliminarIndicador": "#TableIndicador tbody tr td .btn-delete",

            "btnEliminarCategoria":"#TablaDetalleCategoriaIndicador tbody tr td .btn-delete",
            "btnClonarIndicador": "#TableIndicador tbody tr td .btn-clone",
            "btnAddIndicadorVariable": "#TableIndicador tbody tr td .variable",
            "btnAddIndicadorCategoria": "#TableIndicador tbody tr td .Categoría",
            "btnEliminarVariable":"#TableDetalleVariable tbody tr td .btn-delete",
            "btnSiguienteCategoría": "#btnSiguienteCategoría",
            "btnAtrasCategoría": "#btnAtrasCategoría",
            "btnSiguienteVariable": "#btnSiguienteVariable",
            "btnAtrasVariable": "#btnAtrasVariable",
            "btnCancelar":"#btnCancelarIndicador",

            "indexView": "#dad1f1ea",
            "createView": "#dad1f550",

        },

    "Variables":{

    },

    "Metodos": {
        "CargarTablaIndicadores": function () {
            $("#loading").fadeIn();

            JsIndicador.Consultas.ConsultaListaIndicadores()
                .then((data) => {
                    JsIndicador.Metodos.InsertarDatosTablaIndicadores(data.objetoRespuesta);
                })
                .catch((error) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });

        },

        "InsertarDatosTablaIndicadores": function (listaIndicadores) {
            EliminarDatasource();
            let html = "";

            listaIndicadores.forEach(item => {
                html += "<tr>";

                html += `<th scope='row'>${ item.Codigo }</th>`;
                html += `<th scope='row'>${ item.Nombre }</th>`;
                html += `<th scope='row'>${ item.GrupoIndicadores.Nombre }</th>`;
                html += `<th scope='row'>${ item.TipoIndicadores.Nombre }</th>`;
                html += `<th scope='row'>${ item.EstadoRegistro.Nombre }</th>`;
                html += "<td>"
                html += `<button class="btn-icon-base btn-edit" type="button" data-toggle="tooltip" data-placement="top" title="Editar" value=${ item.idIndicador }></button>`
                html += `<button type="button" data-toggle="tooltip" data-placement="top" title="Clonar" class="btn-icon-base btn-clone" value=${ item.idIndicador }></button>`
                html += `<button type="button" data-toggle="tooltip" data-placement="top" title="Desactivar" class="btn-icon-base btn-power-off" value=${ item.idIndicador }></button>`
                html += `<button type="button" data-toggle="tooltip" data-placement="top" title="Eliminar" class="btn-icon-base btn-delete" value=${ item.idIndicador }></button>`
                html += "</td>"
                html += "</tr>";
            });

            $(JsIndicador.Controles.tablaIndicador).html(html);
            CargarDatasource();
        },

        "EliminarIndicador": function (identficador) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Indicador?", jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {

                    jsMensajes.Metodos.OkAlertModal("El Indicador ha sido eliminado")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
                });
        }
    },

    "Consultas": {
        "ConsultaListaIndicadores": function () {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: jsUtilidades.Variables.urlOrigen + '/IndicadorFonatel/ObtenerListaIndicadores',
                    type: "GET",
                    dataType: "JSON",
                    beforeSend: function () { },
                    success: function (obj) {
                        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                            resolve(obj);
                        }
                        else {
                            reject();
                        }
                    },
                    error: function () {
                        reject()
                    }
                })
            })
        },

        "EliminarDetalleCategoria": function (idIndicador) {
            var def = $.Deferred();
            return $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/IndicadorFonatel/EliminarCategoriasDetalle',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () { },
                data: { idIndicador },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        def.resolve(obj);
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }
            }).fail(function (obj) {
                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
            })
        },
    }
}


$(document).on("click", JsIndicador.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/IndicadorFonatel/Index";
        });
});



$(document).on("click", JsIndicador.Controles.btnFinalizar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Indicador?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido agregado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});



$(document).on("click", JsIndicador.Controles.btnEditarIndicador, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/IndicadorFonatel/Create?id=" + id;
});



$(document).on("click", JsIndicador.Controles.btnAddIndicadorCategoria, function () {
    let id = 1;
    window.location.href = "/Fonatel/IndicadorFonatel/DetalleCategoría?id=" + id;
});

$(document).on("click", JsIndicador.Controles.btnAddIndicadorVariable, function () {
    let id = 1;
    window.location.href = "/Fonatel/IndicadorFonatel/DetalleVariables?id=" + id;
});




$(document).on("click", JsIndicador.Controles.btnDesactivarIndicador, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido desactivado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});


$(document).on("click", JsIndicador.Controles.btnEliminarIndicador, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Indicador?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {

            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});


$(document).on("click", JsIndicador.Controles.btnEliminarVariable, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Variable?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Variable ha sido eliminada")
                .set('onok', function (closeEvent) {  });
        });
});
$(document).on("click", JsIndicador.Controles.btnEliminarCategoria, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Categoría?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido eliminada")
                .set('onok', function (closeEvent) { });
        });
});



$(document).on("click", JsIndicador.Controles.btnActivarIndicador, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido activado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });
});




$(document).on("click", JsIndicador.Controles.btnGuardarIndicador, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial del Indicador?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {

            jsMensajes.Metodos.OkAlertModal("El Indicador ha sido creado")
                .set('onok', function (closeEvent) { $("a[href='#step-2']").trigger('click'); });
            
        });
});



$(document).on("click", JsIndicador.Controles.btnSiguienteIndicador, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');
});




$(document).on("click", JsIndicador.Controles.btnSiguienteVariable, function (e) {
    e.preventDefault();
   $("a[href='#step-3']").trigger('click');

});


$(document).on("click", JsIndicador.Controles.btnAtrasVariable, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');

});


$(document).on("click", JsIndicador.Controles.btnSiguienteCategoría, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Indicador?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Indicador ha sido agregado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/IndicadorFonatel/index" });
        });

});

$(document).on("click", JsIndicador.Controles.btnAtrasCategoría, function (e) {
    e.preventDefault();
    $("a[href='#step-2']").trigger('click');

});

$(document).on("click", JsIndicador.Controles.btnGuardarVariable, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Variable?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Variable ha sido agregada")
                .set('onok', function (closeEvent) { });
        });
});

$(document).on("click", JsIndicador.Controles.btnGuardarCategoría, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido agregada")
                .set('onok', function (closeEvent) { });
        });
});


$(document).on("click", JsIndicador.Controles.btnClonarIndicador, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar el Indicador?", jsMensajes.Variables.actionType.clonar)
        .set('onok', function (closeEvent) {
             window.location.href = "/Fonatel/IndicadorFonatel/Create?id="+id
        });
});


$(function () {
    if ($(JsIndicador.Controles.indexView).length > 0) {
        JsIndicador.Metodos.CargarTablaIndicadores();
    }

    if ($(JsIndicador.Controles.createView).length > 0) {
        console.log("create");
    }
});
