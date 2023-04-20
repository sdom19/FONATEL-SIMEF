JsGestionTextoSIGITEL = {
    "Controles": {
        "btnEditarTexto": "#TablaPantallas tbody tr td .btn-edit",
        "btnEliminarDetalleTexto": "#TablaDetalleTexto tbody tr td .btn-delete",
        "btnEditarDetalleTexto": "#TablaDetalleTexto tbody tr td .btn-edit",
        "btnFinalizarDetalle": "#btnFinalizarDetalle",
        "btnGuardarDetalle": "#btnGuardarDetalle",
        "btnCancelarDetalle": "#btnCancelarDetalle",

        "SeccionImagen": "#seccion-imagen",
        "SeccionDescripcion": "#seccion-descripcion",
        "imgPreview": "#imgPreview",
        "TablaDetalleTexto":"#TablaDetalleTexto tbody",

        "ddlTipoContenidoDetalle": "#ddlTipoContenidoDetalle",
        "txtDescripcion": "#txtDescripcion",
        "txtOrden": "#txtOrden",
        "imageFile": "#imageFile",
        "txtIdContenidoPantallaSIGITEL":"#txtIdContenidoPantallaSIGITEL",

        "tipoDetalleHelp": "#tipoDetalleHelp",
        "descripcionHelp": "#descripcionHelp",
        "imagenHelp": "#imagenHelp",
        "ordenHelp": "#ordenHelp"
    },
    "Variables": {
        "TipoContenidoDetalle": {
            "TituloPrincipal": "1",
            "Subtitulo": "2",
            "Descripcion": "3",
            "Imagen": "4"
        },
        "DatosTablaDetalle": [],
        "ImagenCargada": false,
    },
    "Metodos": {
        "PreviewImage": function (event) {
            let input = event.target;
            let file = input.files[0];
            let objectURL = URL.createObjectURL(file);
            $(JsGestionTextoSIGITEL.Controles.imgPreview)[0].src = objectURL;
        },

        "ValidarCampos": function () {
            let resultado = true;
            let tipoContenido = $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).val();
            let descripcion = $(JsGestionTextoSIGITEL.Controles.txtDescripcion).val();
            let imagen = $(JsGestionTextoSIGITEL.Controles.imageFile).val();
            let orden = $(JsGestionTextoSIGITEL.Controles.txtOrden).val();

            $(JsGestionTextoSIGITEL.Controles.tipoDetalleHelp).addClass("hidden");
            $(JsGestionTextoSIGITEL.Controles.descripcionHelp).addClass("hidden");
            $(JsGestionTextoSIGITEL.Controles.ordenHelp).addClass("hidden");
            $(JsGestionTextoSIGITEL.Controles.imagenHelp).addClass("hidden");

            $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).parent().removeClass("has-error");
            $(JsGestionTextoSIGITEL.Controles.txtDescripcion).parent().removeClass("has-error");
            $(JsGestionTextoSIGITEL.Controles.txtOrden).parent().removeClass("has-error");
            $(JsGestionTextoSIGITEL.Controles.imageFile).parent().removeClass("has-error");

            switch (tipoContenido) {
                case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.TituloPrincipal:
                case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Subtitulo:
                case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Descripcion:
                    if (descripcion == "") {
                        $(JsGestionTextoSIGITEL.Controles.descripcionHelp).removeClass("hidden");
                        $(JsGestionTextoSIGITEL.Controles.txtDescripcion).parent().addClass("has-error");
                        resultado = false;
                    }
                    if (orden == "") {
                        $(JsGestionTextoSIGITEL.Controles.ordenHelp).removeClass("hidden");
                        $(JsGestionTextoSIGITEL.Controles.txtOrden).parent().addClass("has-error");
                        resultado = false;
                    }
                    break;

                case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Imagen:
                    if (!JsGestionTextoSIGITEL.Variables.ImagenCargada) {
                        $(JsGestionTextoSIGITEL.Controles.imagenHelp).removeClass("hidden");
                        $(JsGestionTextoSIGITEL.Controles.imageFile).parent().addClass("has-error");
                        resultado = false;
                    }
                    if (orden == "") {
                        $(JsGestionTextoSIGITEL.Controles.ordenHelp).removeClass("hidden");
                        $(JsGestionTextoSIGITEL.Controles.txtOrden).parent().addClass("has-error");
                        resultado = false;
                    }
                    break;

                default:
                    $(JsGestionTextoSIGITEL.Controles.tipoDetalleHelp).removeClass("hidden");
                    $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).parent().addClass("has-error");
                    resultado = false;
                    break;
            }
            return resultado;
        },

        "MostrarDatosDetalleTabla": function (data) {
            JsGestionTextoSIGITEL.Variables.DatosTablaDetalle = data;
            EliminarDatasource();
            $(JsGestionTextoSIGITEL.Controles.TablaDetalleTexto).html("");
            let html = "";
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                html = html + "<tr><th scope='row'>" + item.Orden + "</th><td>" + item.TipoContenidoTextoSIGITEL.NombreTipoContenido + "</td>";

                if (item.IdTipoContenidoTextoSIGITEL == JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Imagen) {
                    html = html + "<td><img height='50' src='"+item.RutaImagen+"'/></td>";
                } else {
                    html = html + "<td>"+item.Texto.substring(0, 150)+"</td>";
                }

                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + item.IdContenidoPantallaSIGITEL + " title='Editar' class='btn-icon-base btn-edit'></button>";
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' value=" + item.IdContenidoPantallaSIGITEL + " title='Eliminar' class='btn-icon-base btn-delete '></button></td></tr>";
                //html = html + "</tr>"
            }
            $(JsGestionTextoSIGITEL.Controles.TablaDetalleTexto).html(html);
            CargarDatasource();
        },

        "LimpiarCampos": function () {
            $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).val(1).trigger("change");
            $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).val(null).trigger("change");

            $(JsGestionTextoSIGITEL.Controles.txtDescripcion).val("");
            $(JsGestionTextoSIGITEL.Controles.imageFile).val("");
            $(JsGestionTextoSIGITEL.Controles.txtOrden).val("");
            $(JsGestionTextoSIGITEL.Controles.imgPreview)[0].src = "";
            $(JsGestionTextoSIGITEL.Controles.txtIdContenidoPantallaSIGITEL).val("0");
            $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).attr("disabled", false);
            JsGestionTextoSIGITEL.Variables.ImagenCargada = false;
        }
    },
    "Consultas": {
        "ObtenerDetalle": function () {
            let data = new Object();
            data.IdCatalogoPantallaSIGITELString = ObtenerValorParametroUrl("id");
            if (data.IdCatalogoPantallaSIGITELString == null) {
                return;
            }

            $("#loading").fadeIn();
            execAjaxCall("/GestionTextoSIGITEL/ObtenerDetallePantalla", "POST", data)
                .then((data) => {
                    if (data.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsGestionTextoSIGITEL.Metodos.MostrarDatosDetalleTabla(data.objetoRespuesta);
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(data.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "GuardarDetalle": function () {
            var data = new FormData();
            let objData = new Object();
            let tipoContenido = $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).val();

            if (tipoContenido == JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Imagen) {
                if ($(JsGestionTextoSIGITEL.Controles.imageFile)[0].files.length > 0) {
                    data.append('file', $(JsGestionTextoSIGITEL.Controles.imageFile)[0].files[0]);
                } else {
                    objData.RutaImagen = $(JsGestionTextoSIGITEL.Controles.imgPreview)[0].src;
                }
            }

            objData.IdCatalogoPantallaSIGITELString = ObtenerValorParametroUrl("id");
            objData.IdTipoContenidoTextoSIGITEL = $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).val();
            objData.Orden = $(JsGestionTextoSIGITEL.Controles.txtOrden).val();
            objData.IdContenidoPantallaSIGITEL = $(JsGestionTextoSIGITEL.Controles.txtIdContenidoPantallaSIGITEL).val();
            objData.Texto = $(JsGestionTextoSIGITEL.Controles.txtDescripcion).val();
            objData.Estado = true;

            if (objData.IdContenidoPantallaSIGITEL != "0") {
                let itemEdit = JsGestionTextoSIGITEL.Variables.DatosTablaDetalle.find(item => item.IdContenidoPantallaSIGITEL == parseInt(objData.IdContenidoPantallaSIGITEL));
                objData.RutaImagen = itemEdit.RutaImagen;
            }

            data.append('datos', JSON.stringify({ datos: objData}));
            
            $.ajax({
                url: 'GuardarDetalle',
                type: 'post',
                datatype: 'json',
                contentType: false,
                processData: false,
                data: data,
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    var respuesta = JSON.parse(obj);
                    if (respuesta.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsGestionTextoSIGITEL.Metodos.MostrarDatosDetalleTabla(respuesta.objetoRespuesta);
                        JsGestionTextoSIGITEL.Metodos.LimpiarCampos();
                        jsMensajes.Metodos.OkAlertModal("El detalle de gestión de texto SIGITEL ha sido guardado")
                            .set('onok', function (closeEvent) { });
                    } else{
                        jsMensajes.Metodos.OkAlertErrorModal(respuesta.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }
            }).fail(function (obj) {
                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })

            }).always(function () {
                $("#loading").fadeOut();
            })
        },

        "EliminarDetalle": function (id) {
            var data = new FormData();
            let objData = new Object();
            let itemEdit = JsGestionTextoSIGITEL.Variables.DatosTablaDetalle.find(item => item.IdContenidoPantallaSIGITEL == parseInt(id));

            objData.IdCatalogoPantallaSIGITELString = ObtenerValorParametroUrl("id");
            objData.IdTipoContenidoTextoSIGITEL = itemEdit.IdTipoContenidoTextoSIGITEL;
            objData.IdContenidoPantallaSIGITEL = id;
            objData.RutaImagen = itemEdit.RutaImagen;
            objData.Estado = false;

            data.append('datos', JSON.stringify({ datos: objData }));

            $.ajax({
                url: 'GuardarDetalle',
                type: 'post',
                datatype: 'json',
                contentType: false,
                processData: false,
                data: data,
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    var respuesta = JSON.parse(obj);
                    if (respuesta.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsGestionTextoSIGITEL.Metodos.MostrarDatosDetalleTabla(respuesta.objetoRespuesta);
                        JsGestionTextoSIGITEL.Metodos.LimpiarCampos();

                        jsMensajes.Metodos.OkAlertModal("El detalle de gestión de texto SIGITEL ha sido eliminado")
                            .set('onok', function (closeEvent) { });

                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(respuesta.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }
            }).fail(function (obj) {
                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })

            }).always(function () {
                $("#loading").fadeOut();
            })
        }
    }
}

$(document).on("click", JsGestionTextoSIGITEL.Controles.btnEliminarDetalleTexto, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el detalle de gestión de texto SIGITEL?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsGestionTextoSIGITEL.Consultas.EliminarDetalle(id);
        });
});

$(document).on("click", JsGestionTextoSIGITEL.Controles.btnEditarDetalleTexto, function () {
    $("#loading").fadeIn();

    let id = $(this).val();

    $(JsGestionTextoSIGITEL.Controles.txtIdContenidoPantallaSIGITEL).val(id);
    let itemEdit = JsGestionTextoSIGITEL.Variables.DatosTablaDetalle.find(item => item.IdContenidoPantallaSIGITEL == parseInt(id));

    $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).val(itemEdit.IdTipoContenidoTextoSIGITEL).trigger("change");
    $(JsGestionTextoSIGITEL.Controles.txtDescripcion).val(itemEdit.Texto);
    $(JsGestionTextoSIGITEL.Controles.txtOrden).val(itemEdit.Orden);
    $(JsGestionTextoSIGITEL.Controles.imgPreview)[0].src = itemEdit.RutaImagen;
    $(JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle).attr("disabled", true);
    JsGestionTextoSIGITEL.Variables.ImagenCargada = true;

    JsGestionTextoSIGITEL.Metodos.ValidarCampos();
    $("#loading").fadeOut();
});

$(document).on("click", JsGestionTextoSIGITEL.Controles.btnEditarTexto, function () {
    let id = $(this).val();
    window.location.href = "/GestionTextoSIGITEL/Create?id=" + id;
});

$(document).on("change", JsGestionTextoSIGITEL.Controles.ddlTipoContenidoDetalle, function () {
    let id = $(this).val();
    switch (id) {
        case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.TituloPrincipal:
        case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Subtitulo:
        case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Descripcion:
            $(JsGestionTextoSIGITEL.Controles.SeccionImagen).addClass("hidden");
            $(JsGestionTextoSIGITEL.Controles.SeccionDescripcion).removeClass("hidden");
            $(JsGestionTextoSIGITEL.Controles.imgPreview).addClass("hidden");
            break;
        
        case JsGestionTextoSIGITEL.Variables.TipoContenidoDetalle.Imagen:
            $(JsGestionTextoSIGITEL.Controles.SeccionImagen).removeClass("hidden");
            $(JsGestionTextoSIGITEL.Controles.SeccionDescripcion).addClass("hidden");
            $(JsGestionTextoSIGITEL.Controles.imgPreview).removeClass("hidden");
            break;

        default:
            break;
    }
});

$(document).on("change", JsGestionTextoSIGITEL.Controles.imageFile, function (evt) {
    $(JsGestionTextoSIGITEL.Controles.imgPreview)[0].src = "";
    JsGestionTextoSIGITEL.Metodos.PreviewImage(evt);
    if ($(JsGestionTextoSIGITEL.Controles.imageFile)[0].files.length > 0) {
        JsGestionTextoSIGITEL.Variables.ImagenCargada = true;
    } else {
        JsGestionTextoSIGITEL.Variables.ImagenCargada = false;
    }
});

$(document).on("click", JsGestionTextoSIGITEL.Controles.imageFile, function (evt) {
    JsGestionTextoSIGITEL.Variables.ImagenCargada = false;
});

$(document).on("click", JsGestionTextoSIGITEL.Controles.btnGuardarDetalle, function (e) {
    e.preventDefault();
    let id = $(JsGestionTextoSIGITEL.Controles.txtIdContenidoPantallaSIGITEL).val();

    if (id == "0") {
        if (JsGestionTextoSIGITEL.Metodos.ValidarCampos()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea guardar el detalle de gestión de texto SIGITEL?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsGestionTextoSIGITEL.Consultas.GuardarDetalle()
                });
        }
    } else {
        if (JsGestionTextoSIGITEL.Metodos.ValidarCampos()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar el detalle de gestión de texto SIGITEL?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsGestionTextoSIGITEL.Consultas.GuardarDetalle()
                });
        }
    }
    
});

$(document).on("click", JsGestionTextoSIGITEL.Controles.btnCancelarDetalle, function (e) {
    e.preventDefault();
    
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/GestionTextoSIGITEL/Index";
        });

});

$(document).on("click", JsGestionTextoSIGITEL.Controles.btnFinalizarDetalle, function (e) {
    e.preventDefault();

    /*jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/GestionTextoSIGITEL/Index";
        });
    */

    window.location.href = "/GestionTextoSIGITEL/Index";
});

$(function () {
    JsGestionTextoSIGITEL.Consultas.ObtenerDetalle();
})
