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
        "btnFinalizarDetalleRelacion": "#btnFinalizarDetalleRelacion",
        "btnCargarDetalle": "#TablaRelacionCategoria tbody tr td .btn-upload",
        "inputFileCargarDetalle": "#inputFileCargarDetalle",
        "btnDescargarDetalle": "#TablaRelacionCategoria tbody tr td .btn-download",
        "TablaRelacionCategoria": "#TablaRelacionCategoria tbody",
        "TablaRelacionCategoriaElemento": "#TablaRelacionCategoria",
        "TablaDetalleRelacion": "#TablaDetalleRelacionCategoria tbody",
        "TablaDetalleRelacionElemento": "#TablaDetalleRelacionCategoria",
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
        "id": "#txtidRelacion",
        "detalleid": "#txtidDetalle"
    },

    "Variables": {
        "ListadoRelaciones": [],
        "ListadoDetalleRelaciones": [],
        "ModoEditarAtributo": false,
        "esModoEdicion": false,
        "objEditarDetalleAtributo": null,
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

                html = html + "<td scope='row'>" + detalle.CategoriaDesagracion.NombreCategoria + "</td>";

                html = html + "<td scope='row'>" + detalle.CategoriaAtributoValor + "</td>";

                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' data-index=" + i + " title='Editar' value=" + detalle.id + " class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' data-index=" + i + " title='Eliminar' value=" + detalle.id + " class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>"
            }
            $(JsRelacion.Controles.TablaDetalleRelacion).html(html);
            CargarDatasource();
            JsRelacion.Consultas.DetalleCompletos();

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

        "ValidarFormularioDetalle": function () {

            let validar = true;

            $(JsRelacion.Controles.CategoriaDetalleHelp).addClass("hidden");
            $(JsRelacion.Controles.DetalleDesagregacionAtributoHelp).addClass("hidden");

            if ($(JsRelacion.Controles.ddlCategoriaDetalle).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.CategoriaDetalleHelp).removeClass("hidden");
            }

            if ($(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).val().length == 0) {
                validar = false;
                $(JsRelacion.Controles.DetalleDesagregacionAtributoHelp).removeClass("hidden");
            }


            return validar;
        },

        "CerrarFormulario": function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Salir del Formulario?", jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {

                });
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
                    console.log(obj);
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
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
        },

        "ConsultarDetalleDesagregacionId": function (selected) {

            execAjaxCall("/RelacionCategoria/CargarListaDetalleDesagregacion?selected=" + selected, "GET")

                .then((obj) => {

                    let html = "";

                    for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                        html = html + "<option value='" + obj.objetoRespuesta[i].toUpperCase() + "'>" + obj.objetoRespuesta[i] + "</option>"
                    }

                    $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).html(html);

                    if (JsRelacion.Variables.esModoEdicion) {
                        $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).val(JsRelacion.Variables.objEditarDetalleAtributo.CategoriaAtributoValor.toUpperCase());
                        $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).trigger("change");
                        JsRelacion.Variables.esModoEdicion = false;
                    }
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
                    jsMensajes.Metodos.OkAlertModal("La Relación ha sido creada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/RelacionCategoria/Index";
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
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
                    jsMensajes.Metodos.OkAlertModal("La Relación ha sido editada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/RelacionCategoria/Index";
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {

                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {

                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarRelacionCategoria": function (idRelacionCategoria) {

            $("#loading").fadeIn();

            execAjaxCall("/RelacionCategoria/EliminarRelacionCategoria", "POST", { idRelacionCategoria: idRelacionCategoria })
                .then((obj) => {

                    jsMensajes.Metodos.ConfirmYesOrNoModal("La Relación ya está en uso en el/los Indicadores:<br><br> Indicadores Asociados<br> <br> ¿Desea eliminarla?", jsMensajes.Variables.actionType.eliminar)
                        .set('onok', function (closeEvent) {

                            JsRelacion.Metodos.RemoverItemDataTable(JsRelacion.Controles.TablaRelacionCategoriaElemento, `button[value='${idRelacionCategoria}']`)

                            jsMensajes.Metodos.OkAlertModal("La Relación ha sido eliminada")
                                .set('onok', function (closeEvent) {

                                    JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;
                                });

                        });

                    //JsRelacion.Metodos.RemoverItemDataTable(JsRelacion.Controles.TablaRelacionCategoriaElemento, `button[value='${idRelacionCategoria}']`)

                    //    jsMensajes.Metodos.OkAlertModal("La Relación ha sido eliminada")
                    //        .set('onok', function (closeEvent) {

                    //            JsRelacion.Variables.ListadoRelaciones = obj.objetoRespuesta;
                    //});

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

        "InsertarDetalleRelacion": function () {


            let RelacionDetalle = new Object();
            RelacionDetalle.id = $(JsRelacion.Controles.id).val();
            RelacionDetalle.idCategoriaAtributo = $(JsRelacion.Controles.ddlCategoriaDetalle).val();
            RelacionDetalle.CategoriaAtributoValor = $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).val();

            execAjaxCall("/RelacionCategoria/InsertarDetalleRelacion", "POST", RelacionDetalle)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Detalle ha sido creado")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });
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

        "ModificarDetalleRelacion": function () {
            $("#loading").fadeIn();

            let RelacionDetalle = new Object();
            RelacionDetalle.idDetalleRelacionCategoria = $(JsRelacion.Controles.detalleid).val();
            RelacionDetalle.id = $(JsRelacion.Controles.id).val();
            RelacionDetalle.idCategoriaAtributo = $(JsRelacion.Controles.ddlCategoriaDetalle).val();
            RelacionDetalle.CategoriaAtributoValor = $(JsRelacion.Controles.ddlDetalleDesagregacionAtributo).val();

            execAjaxCall("/RelacionCategoria/ModificarDetalleRelacion", "POST", RelacionDetalle)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Detalle ha sido editado")
                        .set('onok', function (closeEvent) {
                            location.reload();
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) {

                            });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) {

                            });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "CargarDetalleDesagregacion": function (index) {


            if (JsRelacion.Variables.ListadoDetalleRelaciones.length > index) {
                JsRelacion.Variables.esModoEdicion = true;
                JsRelacion.Consultas.DetalleCompletos();
                JsRelacion.Variables.objEditarDetalleAtributo = JsRelacion.Variables.ListadoDetalleRelaciones[index];
                $(JsRelacion.Controles.ddlCategoriaDetalle).val(JsRelacion.Variables.objEditarDetalleAtributo.CategoriaDesagracion.idCategoria);
                $(JsRelacion.Controles.ddlCategoriaDetalle).trigger("change");

                $(JsRelacion.Controles.detalleid).val(JsRelacion.Variables.objEditarDetalleAtributo.idDetalleRelacionCategoria);
            }
        },

        "EliminarDetalleRelacion": function (idDetalleRelacionCategoria) {

            $("#loading").fadeIn();

            execAjaxCall("/RelacionCategoria/EliminarDetalleRelacion", "POST", { idDetalleRelacionCategoria: idDetalleRelacionCategoria })

                .then((obj) => {

                    JsRelacion.Metodos.RemoverItemDataTable(JsRelacion.Controles.TablaDetalleRelacionElemento, `button[value='${idDetalleRelacionCategoria}']`)

                    jsMensajes.Metodos.OkAlertModal("La Detalle ha sido eliminado")
                        .set('onok', function (closeEvent) {

                            JsRelacion.Variables.ListadoDetalleRelaciones = obj.objetoRespuesta;
                            JsRelacion.Consultas.DetalleCompletos();
                            location.reload();

                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ImportarExcel": function () {
            var data;
            data = new FormData();
            data.append('file', $(JsRelacion.Controles.inputFileCargarDetalle)[0].files[0]);
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/RelacionCategoria/CargarExcel',
                type: 'post',
                datatype: 'json',
                contentType: false,
                processData: false,
                async: false,
                data: data,
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    $("#loading").fadeOut();
                    jsMensajes.Metodos.OkAlertModal("El archivo ha sido importado")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });

                }
            }).fail(function (obj) {
                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();

            })
        },

        "DetalleCompletos": function () {

            let formularioCompleto = JsRelacion.Variables.ListadoDetalleRelaciones.length == 0 ? false : JsRelacion.Variables.ListadoDetalleRelaciones[0].Completo;

            if (JsRelacion.Variables.ModoEditarAtributo) {

                if (formularioCompleto) {

                    $(JsRelacion.Controles.btnGuardarDetalle).prop("disabled", false);
                    $(JsRelacion.Controles.btnFinalizarDetalleRelacion).prop("disabled", true);

                }
                
            } else {

                if (formularioCompleto) {

                    $(JsRelacion.Controles.btnGuardarDetalle).prop("disabled", true);
                    $(JsRelacion.Controles.btnFinalizarDetalleRelacion).prop("disabled", false);

                } else {
                    $(JsRelacion.Controles.btnGuardarDetalle).prop("disabled", false);
                    $(JsRelacion.Controles.btnFinalizarDetalleRelacion).prop("disabled", true);
                }
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
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar la Relación entre Categoría?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsRelacion.Consultas.EditarRelacion();
            });
    }
    else {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Relación entre Categoría?", jsMensajes.Variables.actionType.agregar)
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

//EVENTO PARA ELIMINAR RELACION ENTRE CATEGORIAS 
$(document).on("click", JsRelacion.Controles.btnDeleteRelacion, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Relación entre Categoría?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {

            JsRelacion.Consultas.EliminarRelacionCategoria(id);

        });

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


    if (!JsRelacion.Variables.ModoEditarAtributo) {
        if (JsRelacion.Metodos.ValidarFormularioDetalle()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el detalle a la Categoría?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsRelacion.Consultas.InsertarDetalleRelacion();
                });
        }
    }
    else {

        if (JsRelacion.Metodos.ValidarFormularioDetalle()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea modificar el detalle a la Categoría?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsRelacion.Consultas.ModificarDetalleRelacion();
                });
        }
    }

});

//EVENTO PARA EDITAR DETALLE RELACION ENTRE CATEGORIAS
$(document).on("click", JsRelacion.Controles.btnEditarDetalle, function () {
    JsRelacion.Variables.ModoEditarAtributo = true;
    let id = $(this).attr("data-index");
    JsRelacion.Consultas.CargarDetalleDesagregacion(id);
});

//EVENTO PARA ELIMINAR DETALLE RELACION ENTRE CATEGORIAS 
$(document).on("click", JsRelacion.Controles.btnEliminarDetalleRelacion, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el detalle?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsRelacion.Consultas.EliminarDetalleRelacion(id);
        });
});

//EVENTO PARA AGREGAR DETALLE POR EXCEL
$(document).on("click", JsRelacion.Controles.btnCargarDetalle, function (e) {

    $(JsRelacion.Controles.inputFileCargarDetalle).click();
});


$(document).on("change", JsRelacion.Controles.inputFileCargarDetalle, function (e) {
    JsRelacion.Consultas.ImportarExcel();
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

//EVENTO BOTON FINALIZAR DETALLE
$(document).on("click", JsRelacion.Controles.btnFinalizarDetalleRelacion, function (e) {
    e.preventDefault();
    window.location.href = "/Fonatel/RelacionCategoria/Index";
});

//EVENTO PARA DESCARGAR EXCEL
$(document).on("click", JsRelacion.Controles.btnDescargarDetalle, function () {
    let id = $(this).val();
    window.open(jsUtilidades.Variables.urlOrigen + "/RelacionCategoria/DescargarExcel?id=" + id);
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