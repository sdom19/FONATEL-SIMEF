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
        "txtmodoDefinicion": "#txtmodoDefinicion",
        "txtDefinicion": "#txtDefinicion",
        "txtFuenteDefinicion": "#txtFuenteDefinicion",
        "txtNotasDefinicion": "#txtNotasDefinicion",
        "txtNotasDefinicionHelp":"#txtNotasDefinicionHelp",
        "txtFuenteDefinicionHelp": "#txtFuenteDefinicionHelp",
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

    "Metodos": {

        "ValidarControles": function () {
            let validar = true;
            $(JsDefiniciones.Controles.txtNotasDefinicionHelp).addClass("hidden");
            $(JsDefiniciones.Controles.txtFuenteDefinicionHelp).addClass("hidden");
            $(JsDefiniciones.Controles.txtDefinicionHelp).addClass("hidden");

            let definiciones = $(JsDefiniciones.Controles.txtDefinicion).val().trim();
            let fuentes = $(JsDefiniciones.Controles.txtFuenteDefinicion).val().trim();
            let notas = $(JsDefiniciones.Controles.txtNotasDefinicion).val().trim();

            if (definiciones.length == 0) {
                $(JsDefiniciones.Controles.txtDefinicionHelp).removeClass("hidden");
                Validar = false;
            }
            if (fuentes.length == 0) {
                $(JsDefiniciones.Controles.txtFuenteDefinicionHelp).removeClass("hidden");
                validar = false;
            }
            if (notas.length == 0) {
                $(JsDefiniciones.Controles.txtNotasDefinicionHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },


        "CargarTablaDefiniciones": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsDefiniciones.Variables.ListadoDefiniciones.length; i++) {
                let Definiciones = JsDefiniciones.Variables.ListadoDefiniciones[i];

                let TieneDefinicion = Definiciones.idDefinicion == 0 ? "NO" : "SI";

                html = html + "<tr><th scope='row'>" + Definiciones.Indicador.Codigo + "</th>" +
                    "<td>" + Definiciones.Indicador.Nombre + "</td>" +
                    "<td>" + Definiciones.Indicador.GrupoIndicadores.Nombre + "</td>" +
                    "<td>" + Definiciones.Indicador.TipoIndicadores.Nombre + "</td>" +
                    "<td>" + TieneDefinicion + "</td>";
                if (TieneDefinicion=="NO") {
                    html = html + "<td><button type='button' data-toggle='tooltip' value=" + Definiciones.Indicador.id+"  data-placement='top' title='Agregar' class='btn-icon-base btn-add'></button>" +
                        "<button type='button' data-toggle='tooltip' disabled data-placement='top' title='Editar' class='btn-icon-base btn-edit'></button>" +
                        "<button type='button' data-toggle='tooltip' disabled data-placement='top' title='Clonar' class='btn-icon-base btn-clone'></button>" +
                        "<button type='button' data-toggle='tooltip' disabled  data-placement='top' title='Visualizar Detalle' class='btn-icon-base btn-view'></button>" +
                        "<button type='button' data-toggle='tooltip' disabled data-placement='top' title='Eliminar Definición' class='btn-icon-base btn-delete'></button></td>";
                }
                else {
                    html = html + "<td><button type='button' data-toggle='tooltip' disabled data-placement='top' title='Agregar' class='btn-icon-base btn-add'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' value=" + Definiciones.Indicador.id+" title='Editar' class='btn-icon-base btn-edit'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' value=" + Definiciones.id + " title='Clonar' class='btn-icon-base btn-clone'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' value=" + Definiciones.id + " title='Visualizar Detalle' class='btn-icon-base btn-view'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' value=" + Definiciones.id +" title='Eliminar Definición' class='btn-icon-base btn-delete'></button></td>";
                }
             


                html = html + "</tr>"
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

    window.location.href = "/Fonatel/DefinicionIndicadores/Clonar?id=" + id;
});


$(document).on("click", JsDefiniciones.Controles.btnviewDefiniciones, function () {
    let id = $(this).val();

    window.location.href = "/Fonatel/DefinicionIndicadores/Detalle?id=" + id;
});


$(document).on("click", JsDefiniciones.Controles.btnAtrasDefinicion, function () {


    window.location.href = "/Fonatel/DefinicionIndicadores/index";
});



$(document).on("click", JsDefiniciones.Controles.btnGuardar, function (e) {
    e.preventDefault();
    if (JsDefiniciones.Metodos.ValidarControles()) {
        let modo = $(JsDefiniciones.Controles.txtmodoDefinicion).val();
        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar  la Definicion?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal("La Definición ha sido editada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
                });
        }
        else {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Definicion?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal("La Definición ha sido creada")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/DefinicionIndicadores/index" });
                });
        }
    }
});

$(document).on("click", JsDefiniciones.Controles.btnDeleteDefiniciones, function (e) {




    let id = $(this).val();






    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea elimina la Definición?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {

            JsDefiniciones.Consultas.EliminarDefinicion(id);

        });
});






$(function () {
    if ($(JsDefiniciones.Controles.TablaDefiniciones).length > 0) {
        JsDefiniciones.Consultas.ConsultaListaDefiniciones();
    }

})


