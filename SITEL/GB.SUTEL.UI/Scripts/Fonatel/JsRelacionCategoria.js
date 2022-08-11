JsRelacionCategoria = {
    "Controles": {

        "RelacionCategoriaIndex": "#divRelacionCategoriaIndex",
        "btnGuardar": "#btnGuardarRelacionCategoria",
        "btnEliminar": ".btnEliminarRelacionCategoria",
        "btnCancelar": "#btnCancelarRelacion",
        "btnCancelarDetalleRelacionCategoria": "#btnCancelarDetalle",
        "btnAgregarRelacion": "#TablaRelacionCategoria tbody tr td .btn-add",
        "btnEditarRelacionCategoria": "#TablaRelacionCategoria tbody tr td .btn-edit",
        "btnDeleteRelacionCategoria": "#TablaRelacionCategoria tbody tr td .btn-delete",
        "btnEliminarDetalleRelacionCategoria": "#TablaDetalleRelacionCategoria tbody tr td .btn-delete",
        "btnGuardarDetalle": "#btnGuardarDetalle",
        "btnAgregarDetalle": "#btnAgregarDetalle",
        "tablarelacion": "#TablaRelacionCategoria tbody",
        "inputFileAgregarDetalle": "#inputFileAgregarDetalle"
    },
    "Variables": {
        "ListadoRelacionCategoria": []
    },
    "Metodos": {

        "CargarTablaRelacionCategoria": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsRelacionCategoria.Variables.ListadoRelacionCategoria.length; i++) {
                let relacion = JsRelacionCategoria.Variables.ListadoRelacionCategoria[i];

                html = html + "<tr>"

                html = html + "<td scope='row'>" + relacion.Codigo + "</td>";
                html = html + "<td>" + relacion.Nombre + "</td>";

                if (!relacion.TieneDetalle) {
                    html = html + "<td><strong>N/A</strong></td>";
                    html = html + "<td>" + relacion.EstadoRegistro.Nombre + "</td>";
                    html = html + "<td><strong>N/A</strong></td>";
                } else {
                    html = html + "<td>" + relacion.CantidadCategoria + "/" + relacion.DetalleRelacionCategoria.length + "</td>";
                    html = html + "<td>" + relacion.EstadoRegistro.Nombre + "</td>";
                    html = html + "<td><input id='inputFileCargarDetalle' type='file' accept='.csv,.xlsx,.xls' style='display: none;' />" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' title='Cargar Detalle' class='btn-icon-base btn-upload'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' title='Agregar Detalle' class='btn-icon-base btn-add'></button></td>";
                }
                html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + relacion.idRelacionCategoria + " title='Editar' class='btn-icon-base btn-edit'></button>" +
                    "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' class='btn-icon-base btn-clone' ></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' class='btn-icon-base btn-power-on'></button></td >";
                html = html + "</tr>"
            }
            $(JsRelacionCategoria.Controles.tablarelacion).html(html);
            CargarDatasource();
        },

        "CerrarFormulario": function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Salir del Formulario?", jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {

                });

        },

        
    },
    "Consultas": {
        "ConsultaListaRelacionCategoria": function () {
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/ObtenerListaRelacionCategoria',
                type: "GET",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsRelacionCategoria.Variables.ListadoRelacionCategoria = obj.objetoRespuesta;
                        JsRelacionCategoria.Metodos.CargarTablaRelacionCategoria();
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
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




$(document).on("OnChange", JsRelacionCategoria. )

$(document).on("click", JsRelacionCategoria.Controles.btnAgregarRelacion, function () {
    let id = 1;
    window.location.href = "/Fonatel/RelacionCategoria/Detalle?id=" + id;
});

$(document).on("click", JsRelacionCategoria.Controles.btnEditarRelacionCategoria, function () {
        let id = 1;
        window.location.href = "/Fonatel/RelacionCategoria/Create?id=" + id;
    });

$(document).on("click", JsRelacionCategoria.Controles.btnCancelar, function (e) {
        e.preventDefault();
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
            .set('onok', function (closeEvent) {
                window.location.href = "/Fonatel/RelacionCategoria/Index";
            });
    });


$(document).on("click", JsRelacionCategoria.Controles.btnCancelarDetalle, function (e) {
        e.preventDefault();
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
            .set('onok', function (closeEvent) {
                window.location.href = "/Fonatel/RelacionCategoria/Index";
            });
    });

$(document).on("click", JsRelacionCategoria.Controles.btnGuardar, function (e) {
        e.preventDefault();
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Relación?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                jsMensajes.Metodos.OkAlertModal("La Relación ha sido creada")
                    .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
            });
    });

$(document).on("click", JsRelacionCategoria.Controles.btnGuardarDetalle, function (e) {
        e.preventDefault();
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea relacionar la Categoría?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                jsMensajes.Metodos.OkAlertModal("La Categoría ha sido relacionada")
                    .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
            });
    });

$(document).on("click", JsRelacionCategoria.Controles.btnEliminarDetalleRelacionCategoria, function (e) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina el Atributo?", jsMensajes.Variables.actionType.eliminar)
            .set('onok', function (closeEvent) {
                jsMensajes.Metodos.OkAlertModal("El Atributo ha sido eliminado")
                    .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
            });
    });

$(document).on("click", JsRelacionCategoria.Controles.btnDeleteRelacionCategoria, function (e) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina la Relación?", jsMensajes.Variables.actionType.eliminar)
            .set('onok', function (closeEvent) {
                jsMensajes.Metodos.OkAlertModal("La relación ha sido eliminado")
                    .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
            });
    });

$(document).on("click", JsRelacionCategoria.Controles.btnAgregarDetalle, function (e) {
        $(JsRelacionCategoria.Controles.inputFileAgregarDetalle).click();
    });

$(function () {
    if ($(JsRelacionCategoria.Controles.RelacionCategoriaIndex).length > 0) {
        JsRelacionCategoria.Consultas.ConsultaListaRelacionCategoria();
     }
});
