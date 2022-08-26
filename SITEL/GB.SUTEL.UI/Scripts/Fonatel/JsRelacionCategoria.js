JsRelacion = {
    "Controles": {
        "FormularioCrearRelacion": "#FormularioCrearRelacion",
        "FormularioDetalle": "#FormularioDetalle",

        "btnEliminar": ".btnEliminarRelacion",
        "btnGuardar": "#btnGuardarRelacion",
        "btnCancelar": "#btnCancelarRelacion",
        "btnCancelarDetalle": "#btnCancelarDetalle",
        "btnAgregarRelacion": "#TablaRelacionCategoria tbody tr td .btn-add",
        "btnEditarRelacion": "#TablaRelacionCategoria tbody tr td .btn-edit",
        "btnDeleteRelacion": "#TablaRelacionCategoria tbody tr td .btn-delete",
        "btnEliminarDetalleRelacion": "#TablaDetalleRelacionCategoria tbody tr td .btn-delete",
        "btnGuardarDetalle": "#btnGuardarDetalle",
        "btnEditarDetalle": "#TablaDetalleRelacionCategoria tbody tr td .btn-edit",
        "btnAgregarDetalle": "#btnAgregarDetalle",
        "inputFileAgregarDetalle": "#inputFileAgregarDetalle",

        "TablaRelacionCategoria": "#TablaRelacionCategoria tbody",
        "TablaRelacionCategoriaElemento": "#TablaRelacionCategoria",
        "TablaDetalleRelacion": "#TablaDetalleRelacionCategoria tbody",

        "txtCodigo": "#txtCodigo",
        "txtNombre": "#txtNombre",
        "TxtCantidad": "#TxtCantidad",
        "ddlCategoriaId": "#ddlCategoriaId",
        "ddlDetalleDesagregacionId": "#ddlDetalleDesagregacionId",
        "ddlCategoriaDetalle": "#ddlCategoriaDetalle",
        "ddlDetalleDesagregacionAtributo": "#ddlDetalleDesagregacionAtributo",

        "nombreHelp": "#nombreHelp",
        "CodigoHelp": "#CodigoHelp",
        "CantidadHelp": "#CantidadHelp",
        "TipoCategoriaHelp": "#TipoCategoriaHelp",
        "DetalleDesagregacionIDHelp": "#DetalleDesagregacionIDHelp",

        "CategoriaDetalleHelp": "#CategoriaDetalleHelp",
        "DetalleDesagregacionAtributoHelp": "#DetalleDesagregacionAtributoHelp",

        "txtmodoRelacion": "#txtmodoRelacion",
        "id": "#txtidRelacion"

    },

    "Variables": {
        "ListadoRelaciones": [],
        "ListadoDetalleRelaciones": []
    },

    "Metodos": {

        "RemoverItemDataTable": function (pDataTable, pItem) {
            $(pDataTable).DataTable().row($(pItem).parents('tr')).remove().draw();

        },

        "CargarTablaRelacion": function () {
            EliminarDatasource();
            let html = "";

            for (var i = 0; i < JsRelacion.Variables.ListadoRelaciones.length; i++) {
                let detalle = JsRelacion.Variables.ListadoRelaciones[i];
                html = html + "<tr>"

                html = html + "<td scope='row'>" + detalle.Codigo + "</td>";
                html = html + "<td>" + detalle.Nombre + "</td>";
                html = html + "<td>" + detalle.CantidadCategoria + "/" + detalle.DetalleRelacionCategoria.length + "</td>";
                html = html + "<td>" + detalle.EstadoRegistro.Nombre + "</td>";

                html = html + "<td><button type ='button' data-toggle='tooltip' data-placement='top' value=" + detalle.id + "  data-original-title='Cargar Detalle'  title='Cargar Detalle' class='btn-icon-base btn-upload' ></button >" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value=" + detalle.id + " data-original-title='Descargar Plantilla' title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value=" + detalle.id + " data-original-title='Agregar Detalle' title='Agregar Detalle' class='btn-icon-base btn-add'></button></td>";

                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value='" + detalle.id + "' title='Editar' class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' value='" + detalle.id + "' title='Eliminar' class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>";
            }
            $(JsRelacion.Controles.TablaRelacionCategoria).html(html);
            CargarDatasource();
            JsRelacion.Variables.ListadoRelaciones = [];
        },

        "CargarTablaDetalleRelacion": function () {

            EliminarDatasource();

            let html = "";

            for (var i = 0; i < JsRelacion.Variables.ListadoDetalleRelaciones.length; i++) {
                let detalle = JsRelacion.Variables.ListadoDetalleRelaciones[i];

                html = html + "<tr>"

                html = html + "<td scope='row'>" + detalle.idCategoriaAtributo + "</td>";
                html = html + "<td scope='row'>" + detalle.CategoriaAtributoValor + "</td>";

                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' data-index=" + i +" title='Editar' value=" + detalle.id + " class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' data-index="+ i +" title='Eliminar' value=" + detalle.id + " class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>"
            }
            $(JsRelacion.Controles.TablaDetalleRelacion).html(html);
            CargarDatasource();
            JsRelacion.Variables.ListadoDetalleRelaciones = [];
        },

        "ValidarFormularioRelacion": function () {

            let validar = true;

            $(JsRelacion.Controles.nombreHelp).addClass("hidden");
            $(JsRelacion.Controles.CodigoHelp).addClass("hidden");
            $(JsRelacion.Controles.CantidadHelp).addClass("hidden");
            $(JsRelacion.Controles.TipoCategoriaHelp).addClass("hidden");
            $(JsRelacion.Controles.DetalleDesagregacionIDHelp).addClass("hidden");

            if ($(JsRelacion.Controles.txtCodigo).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.CodigoHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.txtNombre).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.nombreHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.TxtCantidad).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.CantidadHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.ddlCategoriaId).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.TipoCategoriaHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.ddlDetalleDesagregacionId).val() == 0) {
                validar = false;
                $(JsRelacion.Controles.DetalleDesagregacionIDHelp).removeClass("hidden");
            }

            return validar;
        },
    },

    "Consultas": {
        "ConsultaListaRelaciones": function () {
            $("#loading").fadeIn();
            execAjaxCall("/RelacionCategoria/ObtenerListaRelacionCategoria", "GET")
                .then((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;
                        JsRelacion.Metodos.CargarTablaRelacion();
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                    $("#loading").fadeOut();
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaListaRelacionDetalle": function () {

            $("#loading").fadeIn();
            let idRelacion = $(JsRelacion.Controles.id).val();
            execAjaxCall("/RelacionCategoria/ObtenerListaCategoriasDetalle?IdRelacionCategoria=" + idRelacion, "GET")
                .then((obj) => {
                    JsRelacion.Variables.ListadoDetalleRelaciones = obj.objetoRespuesta;
                    JsRelacion.Metodos.CargarTablaDetalleRelacion();
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultarDesagregacionId": function (selected) {

            execAjaxCall("/RelacionCategoria/ObtenerDetalleDesagregacionId?selected=" + selected, "GET")

                .then((obj) => {

                    let html = "";

                    for (var i = 0; i < obj.objetoRespuesta.length; i++) {

                        html = html + "<option value='" + obj.objetoRespuesta[i] + "'>" + obj.objetoRespuesta[i] + "</option>"
                    }

                    $(JsRelacion.Controles.ddlDetalleDesagregacionId).html(html);

                }).catch((obj) => {
                    console.log(obj);
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                }).finally(() => {
                    $("#loading").fadeOut();
                });

            //$.ajax({
            //    url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/ObtenerDetalleDesagregacionId?selected=' + selected,
            //    type: "GET",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "JSON",
            //    beforeSend: function () {
            //        $("#loading").fadeIn();
            //    },
            //    success: function (obj) {

            //        let html = "";

            //        for (var i = 0; i < obj.length; i++) {

            //            html = html + "<option value='" + obj[i] + "'>" + obj[i] + "</option>"
            //        }

            //        $(JsRelacion.Controles.ddlDetalleDesagregacionId).html(html);


            //        $("#loading").fadeOut();
            //    }
            //}).fail(function (obj) {

            //    jsMensajes.Metodos.OkAlertErrorModal()
            //        .set('onok', function (closeEvent) { })
            //    $("#loading").fadeOut();
            //});
        },

        "ConsultarDetalleDesagregacionId": function (selected) {

            execAjaxCall("/RelacionCategoria/CargarListaDetalleDesagregacion?selected=" + selected, "GET")

                .then((obj) => {

                    let html = "";

                    for (var i = 0; i < obj.objetoRespuesta.length; i++) {

                        html = html + "<option value='" + obj.objetoRespuesta[i] + "'>" + obj.objetoRespuesta[i] + "</option>"
                    }

                    $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).html(html);

                }).catch((obj) => {
                    console.log(obj);
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); })
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarRelacion": function () {

            $("#loading").fadeIn();

            let RelacionCategoria = new Object();
            RelacionCategoria.Codigo = $(JsRelacion.Controles.txtCodigo).val().trim();
            RelacionCategoria.Nombre = $(JsRelacion.Controles.txtNombre).val().trim();
            RelacionCategoria.CantidadCategoria = $(JsRelacion.Controles.TxtCantidad).val();
            RelacionCategoria.idCategoria = $(JsRelacion.Controles.ddlCategoriaId).val();
            RelacionCategoria.idCategoriaValor = $(JsRelacion.Controles.ddlDetalleDesagregacionId).val();

            execAjaxCall("/RelacionCategoria/InsertarRelacionCategoria", "POST", RelacionCategoria)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Relación entre Categoría ha sido creada")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EditarRelacion": function () {

            $("#loading").fadeIn();

            let RelacionCategoria = new Object();
            RelacionCategoria.id = $(JsRelacion.Controles.id).val();
            RelacionCategoria.Codigo = $(JsRelacion.Controles.txtCodigo).val().trim();
            RelacionCategoria.Nombre = $(JsRelacion.Controles.txtNombre).val().trim();
            RelacionCategoria.CantidadCategoria = $(JsRelacion.Controles.TxtCantidad).val();
            RelacionCategoria.idCategoria = $(JsRelacion.Controles.ddlCategoriaId).val();
            RelacionCategoria.idCategoriaValor = $(JsRelacion.Controles.ddlDetalleDesagregacionId).val();

            execAjaxCall("/RelacionCategoria/EditarRelacionCategoria", "POST", RelacionCategoria)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Relación entre Categoría ha sido editada")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
             });

        },

        "EliminarRelacionCategoria": function (idRelacionCategoria) {

            $("#loading").fadeIn();

            execAjaxCall("/RelacionCategoria/EliminarRelacionCategoria", "POST", { idRelacionCategoria : idRelacionCategoria})
                .then((obj) => {

                    JsRelacion.Metodos.RemoverItemDataTable(JsRelacion.Controles.TablaRelacionCategoriaElemento, `button[value='${idRelacionCategoria}']`)

                    jsMensajes.Metodos.OkAlertModal("La Relación entre Categoría ha sido eliminado")
                        .set('onok', function (closeEvent) {
                            
                            JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;

                        });
                }).catch((obj) => {
                    console.log(obj);
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });

            //$.ajax({
            //    url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/EliminarRelacionCategoria',
            //    type: "POST",
            //    dataType: "JSON",
            //    beforeSend: function () {
            //        $("#loading").fadeIn();
            //    },
            //    data: { idRelacionCategoria },
            //    success: function (obj) {
            //        $("#loading").fadeOut();
            //        if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
            //            jsMensajes.Metodos.OkAlertModal("La Relación entre Categoría ha sido eliminado")
            //                .set('onok', function (closeEvent) {
            //                    JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;
            //                    JsRelacion.Metodos.RemoverItemDataTable(JsRelacion.Controles.TablaRelacionCategoriaElemento, `button[value='${idRelacionCategoria}']`)
            //                });
            //        } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
            //            jsMensajes.Metodos.OkAlertErrorModal()
            //                .set('onok', function (closeEvent) { location.reload(); });
            //        }
            //        else {
            //            jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
            //                .set('onok', function (closeEvent) { location.reload(); });
            //        }
            //    }
            //}).fail(function (obj) {


            //    jsMensajes.Metodos.OkAlertErrorModal()
            //        .set('onok', function (closeEvent) { })
            //    $("#loading").fadeOut();

            //})
        },

        "InsertarDetalleRelacion": function () {


            let RelacionDetalle = new Object();
            RelacionDetalle.id = $(JsRelacion.Controles.id).val();
            RelacionDetalle.idCategoriaAtributo = $(JsRelacion.Controles.ddlCategoriaDetalle).val();
            RelacionDetalle.CategoriaAtributoValor = $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).val();

            execAjaxCall("/RelacionCategoria/InsertarDetalleRelacion", "POST", RelacionDetalle)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Relación entre Categoría ha sido creada")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "CargarDetalleDesagregacion": function (index) {


            if (JsRelacion.Variables.ListadoDetalleRelaciones.length > index) {

                let RelacionDetalle = JsRelacion.Variables.ListadoDetalleRelaciones[index];

                $(JsRelacion.Controles.ddlCategoriaDetalle).val(RelacionDetalle.idCategoriaAtributo);
                $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).val(RelacionDetalle.CategoriaAtributoValor);

                $(JsRelacion.Controles.ddlCategoriaDetalle).trigger("change");
                $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).trigger("change");

            }             
        },
    }
}

//EVENTO PARA GUARDAR RELACION ENTRE CATEGORIAS
$(document).on("click", JsRelacion.Controles.btnGuardar, function (e) {

    e.preventDefault();
    let modo = $(JsRelacion.Controles.txtmodoRelacion).val();
    let validar = JsRelacion.Metodos.ValidarFormularioRelacion();
    if (!validar) {
        return;
    }

    if (modo == jsUtilidades.Variables.Acciones.Editar) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Relación entre Categoria?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsRelacion.Consultas.EditarRelacion();
            });
    }
    else {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Relación entre Categoria?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsRelacion.Consultas.InsertarRelacion();
            });
    }

});

//EVENTO PARA EDITAR RELACION ENTRE CATEGORIAS
$(document).on("click", JsRelacion.Controles.btnEditarRelacion, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/RelacionCategoria/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

//EVENTO DE CAMBIO PARA CARGAR CATEGORIAS DESAGREGACION 
$(document).on("change", JsRelacion.Controles.ddlCategoriaId, function () {

    let selected = $(this).val();
    if (selected != 0) {
        JsRelacion.Consultas.ConsultarDesagregacionId(selected);
    }

});

//EVENTO PARA AGREGAR DETALLE 
$(document).on("click", JsRelacion.Controles.btnAgregarRelacion, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/RelacionCategoria/Detalle?idRelacionCategoria=" + id;
});

//EVENTO DE CAMBIO PARA CARGAR DETALLE CATEGORIAS DESAGREGACION 
$(document).on("change", JsRelacion.Controles.ddlCategoriaDetalle, function () {
    let selected = $(this).val();
    if (selected != 0) {
        JsRelacion.Consultas.ConsultarDetalleDesagregacionId(selected);
    }
});

//EVENTO PARA GUARDAR DETALLE RELACION ENTRE CATEGORIAS 
$(document).on("click", JsRelacion.Controles.btnGuardarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea relacionar la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.InsertarDetalleRelacion();
        });
});

//EVENTO PARA EDITAR DETALLE RELACION ENTRE CATEGORIAS
$(document).on("click", JsRelacion.Controles.btnEditarDetalle, function () {
    let id = $(this).attr("data-index");
    JsRelacion.Consultas.CargarDetalleDesagregacion(id);
});

//EVENTO PARA ELIMINAR DETALLE RELACION ENTRE CATEGORIAS 
$(document).on("click", JsRelacion.Controles.btnEliminarDetalleRelacion, function (e) {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina el Atributo?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Atributo ha sido eliminado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
        });
});

//EVENTO PARA ELIMINAR RELACION ENTRE CATEGORIAS 
$(document).on("click", JsRelacion.Controles.btnDeleteRelacion, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Relación entre Categoría?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.EliminarRelacionCategoria(id);
        });
});

//EVENTO PARA AGREGAR DETALLE POR EXCEL
$(document).on("click", JsRelacion.Controles.btnAgregarDetalle, function () {
    $(JsRelacion.Controles.inputFileAgregarDetalle).click();
});

//EVENTO PARA CANCELAR DETALLE RELACION CATEGORIA
$(document).on("click", JsRelacion.Controles.btnCancelarDetalle, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RelacionCategoria/Index";
        });
});

//EVENTO PARA CANCELAR RELACION CATEGORIA
$(document).on("click", JsRelacion.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RelacionCategoria/Index";
        });
});

$(function () {

    if ($(JsRelacion.Controles.FormularioCrearRelacion).length > 0) {

        let modo = $(JsRelacion.Controles.txtmodoRelacion).val();

        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            //DESACTIVE EL CAMPO CODIGO
            $(JsRelacion.Controles.txtCodigo).prop("disabled", true);
        }
    }

    if ($(JsRelacion.Controles.TablaRelacionCategoria).length > 0) {
        JsRelacion.Consultas.ConsultaListaRelaciones();
    }

    if ($(JsRelacion.Controles.FormularioDetalle).length > 0) {
        JsRelacion.Consultas.ConsultaListaRelacionDetalle();
    }

})