JsDefiniciones= {
    "Controles": {
        "btnEditarDefiniciones": "#TablaDefiniciones tbody tr td .btn-edit",
        "btnviewDefiniciones": "#TablaDefiniciones tbody tr td .btn-view",
        "btnAddDefiniciones": "#TablaDefiniciones tbody tr td .btn-add",
        "btnDeleteDefiniciones": "#TablaDefiniciones tbody tr td .btn-delete",
        "btnCloneDefiniciones": "#TablaDefiniciones tbody tr td .btn-clone",
        "btnGuardar": "#btnGuardarDefiniciones",
        "btnCancelar": "#btnCancelarDefiniciones",
        "TablaDefiniciones": "#TablaDefiniciones tbody",
        "btnAtrasDefinicion": "#btnAtrasDefinicion",
        "txtDefinicion": "#txtDefinicion",
        "txtFuenteDefinicion": "#txtFuenteDefinicion",
        "txtNotasDefinicion": "#txtNotasDefinicion",
        "txtNotasDefinicionHelp":"#txtNotasDefinicionHelp",
        "txtFuenteDefinicionHelp": "#txtFuenteDefinicionHelp",
        "ddlIndicadorHelp": "#ddlIndicadorHelp",
        "txtDefinicionHelp": "#txtDefinicionHelp",
        "ddlindicador": "#ddlindicador",
        "txtCodigoIndicador": "#txtCodigoIndicador",
        "txtNombreIndicador": "#txtNombreIndicador",
        "txtTipoIndicador":"#txtTipoIndicador"
    },
    "Variables":{
        "ListadoDefiniciones": [],
        "DefinicionSeleccionada": new Object()
    },

    Mensajes: {
        tooltipTablaBtnAgregar: "Agregar",
        tooltipTablaBtnEditar: "Editar",
        tooltipTablaBtnClonar: "Clonar",
        tooltipTablaBtnVisualizar: "Visualizar definición",
        tooltipTablaBtnEliminar: "Eliminar Definición"
    },

    "Metodos": {
        "ValidarControles": function () {
            let validar = true;
            $(JsDefiniciones.Controles.txtNotasDefinicionHelp).addClass("hidden");
            $(JsDefiniciones.Controles.txtFuenteDefinicionHelp).addClass("hidden");
            $(JsDefiniciones.Controles.txtDefinicionHelp).addClass("hidden");
            $(JsDefiniciones.Controles.ddlIndicadorHelp).addClass("hidden");

            $(JsDefiniciones.Controles.txtDefinicion).parent().removeClass("has-error");
            $(JsDefiniciones.Controles.txtFuenteDefinicion).parent().removeClass("has-error");
            $(JsDefiniciones.Controles.txtNotasDefinicion).parent().removeClass("has-error");
            $(JsDefiniciones.Controles.ddlindicador).parent().removeClass("has-error");

            let definiciones = $(JsDefiniciones.Controles.txtDefinicion).val().trim();
            let fuentes = $(JsDefiniciones.Controles.txtFuenteDefinicion).val().trim();
            let notas = $(JsDefiniciones.Controles.txtNotasDefinicion).val().trim();

            let modo = ObtenerValorParametroUrl("modo");
            if (modo == jsUtilidades.Variables.Acciones.Clonar) {
                let idClonado = $(JsDefiniciones.Controles.ddlindicador).val().trim();

                if (idClonado == null || idClonado.length == 0) {
                    $(JsDefiniciones.Controles.ddlIndicadorHelp).removeClass("hidden");
                    $(JsDefiniciones.Controles.ddlindicador).parent().addClass("has-error");
                    validar = false;
                }
            }

            if (definiciones.length == 0) {
                $(JsDefiniciones.Controles.txtDefinicionHelp).removeClass("hidden");
                $(JsDefiniciones.Controles.txtDefinicion).parent().addClass("has-error");
                validar = false;
            }
            if (fuentes.length == 0) {
                $(JsDefiniciones.Controles.txtFuenteDefinicionHelp).removeClass("hidden");
                $(JsDefiniciones.Controles.txtFuenteDefinicion).parent().addClass("has-error");
                validar = false;
            }
            if (notas.length == 0) {
                $(JsDefiniciones.Controles.txtNotasDefinicionHelp).removeClass("hidden");
                $(JsDefiniciones.Controles.txtNotasDefinicion).parent().addClass("has-error");
                validar = false;
            }
            return validar;
        },


        "CargarTablaDefiniciones": function () {
            EliminarDatasource();
            let html = "";

            for (var i = 0; i < JsDefiniciones.Variables.ListadoDefiniciones.length; i++) {
                let objDefiniciones = JsDefiniciones.Variables.ListadoDefiniciones[i];

                let TieneDefinicion = objDefiniciones.Definicion == null ? "NO" : "SI";

                html += "<tr><th scope='row'>" + objDefiniciones.Indicador.Codigo + "</th>" +
                    "<td>" + objDefiniciones.Indicador.Nombre + "</td>" +
                    "<td>" + objDefiniciones.Indicador.GrupoIndicadores.Nombre + "</td>" +
                    "<td>" + objDefiniciones.Indicador.TipoIndicadores.Nombre + "</td>" +
                    "<td>" + TieneDefinicion + "</td>";

                if (TieneDefinicion == "NO") {
                    html +=
                        `<td><button type='button' data-toggle='tooltip' value="${objDefiniciones.id}"  data-placement='top' title='${JsDefiniciones.Mensajes.tooltipTablaBtnAgregar}' class='btn-icon-base btn-add'></button>` +
                        `<button type='button' data-toggle='tooltip' disabled data-placement='top' title='${JsDefiniciones.Mensajes.tooltipTablaBtnEditar}' class='btn-icon-base btn-edit'></button>` +
                        `<button type='button' data-toggle='tooltip' disabled data-placement='top' title='${JsDefiniciones.Mensajes.tooltipTablaBtnClonar}' class='btn-icon-base btn-clone'></button>` +
                        `<button type='button' data-toggle='tooltip' disabled data-placement='top' title='${JsDefiniciones.Mensajes.tooltipTablaBtnVisualizar}' class='btn-icon-base btn-view'></button>` +
                        `<button type='button' data-toggle='tooltip' disabled data-placement='top' title='${JsDefiniciones.Mensajes.tooltipTablaBtnEliminar}' class='btn-icon-base btn-delete'></button></td>`;
                }
                else {
                    html +=
                        `<td><button type='button' data-toggle='tooltip' disabled data-placement='top' title='${JsDefiniciones.Mensajes.tooltipTablaBtnAgregar}' class='btn-icon-base btn-add'></button>` +
                        `<button type='button' data-toggle='tooltip' data-placement='top' value="${objDefiniciones.id}" title='${JsDefiniciones.Mensajes.tooltipTablaBtnEditar}' class='btn-icon-base btn-edit'></button>` +
                        `<button type='button' data-toggle='tooltip' data-placement='top' value="${objDefiniciones.id}" title='${JsDefiniciones.Mensajes.tooltipTablaBtnClonar}' class='btn-icon-base btn-clone'></button>` +
                        `<button type='button' data-toggle='tooltip' data-placement='top' value="${objDefiniciones.id}" title='${JsDefiniciones.Mensajes.tooltipTablaBtnVisualizar}' class='btn-icon-base btn-view'></button>` +
                        `<button type='button' data-toggle='tooltip' data-placement='top' value="${objDefiniciones.id}" title='${JsDefiniciones.Mensajes.tooltipTablaBtnEliminar}' class='btn-icon-base btn-delete'></button></td>`;
                }
                html += "</tr>"
            }
            $(JsDefiniciones.Controles.TablaDefiniciones).html(html);
            CargarDatasource();
            JsDefiniciones.Variables.ListadoDefiniciones = [];
        },
    },

    "Consultas": {
        "ConsultaListaDefiniciones": function () {
            $("#loading").fadeIn();
            execAjaxCall("/DefinicionIndicadores/ObtenerListaDefiniciones", "GET")
                .then((data) => {
                    JsDefiniciones.Variables.ListadoDefiniciones = data.objetoRespuesta;
                    JsDefiniciones.Metodos.CargarTablaDefiniciones();
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarDefinicion": function (idDefinicion) {
            let definicion = new Object();
            definicion.id = idDefinicion;
            $("#loading").fadeIn();
            execAjaxCall("/DefinicionIndicadores/EliminarDefinicion", "POST", definicion)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Definición ha sido eliminada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },


        "AgregarDefinicion": function () {
            let objDefinicion = new Object();
            objDefinicion.id = ObtenerValorParametroUrl("id");
            objDefinicion.fuente = $(JsDefiniciones.Controles.txtFuenteDefinicion).val().trim();
            objDefinicion.nota = $(JsDefiniciones.Controles.txtNotasDefinicion).val().trim();
            objDefinicion.definicion = $(JsDefiniciones.Controles.txtDefinicion).val().trim();

            $("#loading").fadeIn();
            execAjaxCall("/DefinicionIndicadores/AgregarDefinicion", "POST", objDefinicion = objDefinicion)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Definición ha sido creada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EditarDefinicion": function () {
            let objDefinicion = new Object();
            objDefinicion.id = ObtenerValorParametroUrl("id");
            objDefinicion .fuente = $(JsDefiniciones.Controles.txtFuenteDefinicion).val().trim();
            objDefinicion .nota = $(JsDefiniciones.Controles.txtNotasDefinicion).val().trim();
            objDefinicion .definicion = $(JsDefiniciones.Controles.txtDefinicion).val().trim();
            $("#loading").fadeIn();
            execAjaxCall("/DefinicionIndicadores/ActualizarDefinicion", "POST", objDefinicion = objDefinicion)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Definición ha sido editada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ClonarDefinicion": function () {
            let objDefinicion = new Object();
            objDefinicion.id = ObtenerValorParametroUrl("id");
            objDefinicion.fuente = $(JsDefiniciones.Controles.txtFuenteDefinicion).val().trim();
            objDefinicion.nota = $(JsDefiniciones.Controles.txtNotasDefinicion).val().trim();
            objDefinicion.definicion = $(JsDefiniciones.Controles.txtDefinicion).val().trim();
            objDefinicion.idClonado = $(JsDefiniciones.Controles.ddlindicador).val().trim();          
            $("#loading").fadeIn();
            execAjaxCall("/DefinicionIndicadores/ClonarDefinicion", "POST", objDefinicion = objDefinicion)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Definición ha sido clonada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "SelecionarIndicador": function (id) {
            let indicador = new Object();
            indicador.id = id;
            $("#loading").fadeIn();
            execAjaxCall("/DefinicionIndicadores/SeleccionarIndicador", "POST", indicador)
                .then((data) => {
                    if (!data.HayError) {
                        if (data.objetoRespuesta.length!=0) {
                            let indicador = data.objetoRespuesta[0];
                            $(JsDefiniciones.Controles.txtCodigoIndicador).val(indicador.Codigo);
                            $(JsDefiniciones.Controles.txtNombreIndicador).val(indicador.Nombre);
                            $(JsDefiniciones.Controles.txtTipoIndicador).val(indicador.TipoIndicadores.Nombre);
                        }
                    }
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

    }

}

$(document).on("click", JsDefiniciones.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/DefinicionIndicadores/Index";
        });
});


$(document).on("change", JsDefiniciones.Controles.ddlindicador, function () {
    let id = $(this).val();
    JsDefiniciones.Consultas.SelecionarIndicador(id);
});



$(document).on("click", JsDefiniciones.Controles.btnAddDefiniciones, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/DefinicionIndicadores/Create?id="+id;
});


$(document).on("click", JsDefiniciones.Controles.btnEditarDefiniciones, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/DefinicionIndicadores/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

$(document).on("click", JsDefiniciones.Controles.btnCloneDefiniciones, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/DefinicionIndicadores/Clonar?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
});


$(document).on("click", JsDefiniciones.Controles.btnviewDefiniciones, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/DefinicionIndicadores/Detalle?id=" + id ;
});


$(document).on("click", JsDefiniciones.Controles.btnAtrasDefinicion, function () {
    window.location.href = "/Fonatel/DefinicionIndicadores/index";
});



$(document).on("click", JsDefiniciones.Controles.btnGuardar, function (e) {
    e.preventDefault();
    if (JsDefiniciones.Metodos.ValidarControles()) {
        let modo = ObtenerValorParametroUrl("modo");
        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar  la Definición?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsDefiniciones.Consultas.EditarDefinicion();
                });
        }
        else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Definición?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsDefiniciones.Consultas.ClonarDefinicion();
                });
        }
        else {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar la Definición?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsDefiniciones.Consultas.AgregarDefinicion();
                   
                });
        }
    }
});

$(document).on("click", JsDefiniciones.Controles.btnDeleteDefiniciones, function (e) {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Definición?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsDefiniciones.Consultas.EliminarDefinicion(id);
        });
});

$(function () {
    if ($(JsDefiniciones.Controles.TablaDefiniciones).length > 0) {
        JsDefiniciones.Consultas.ConsultaListaDefiniciones();
    }
})


