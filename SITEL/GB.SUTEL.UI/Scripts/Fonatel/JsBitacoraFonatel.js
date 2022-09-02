JsBitacora= {
    "Controles": {
        "btnCancelar": "#btnCancelarBitacora",
        "btnBuscarBitacora": "#btnBuscarBitacora",
        "txtFechaHasta": "#txtFechaHasta",
        "txtFechaDesde": "#txtFechaDesde",
        "TablaBitacora":"#TableBitacoraFonatel tbody"
    },
    "Variables":{
        "ListaBitacora":[]
    },

    "Metodos": {
        "CargarTablaBitacora": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsBitacora.Variables.ListaBitacora.length; i++) {
                let Bitacora = JsBitacora.Variables.ListaBitacora[i];

                html=html + "<tr>"
                html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                html = html + "<th>" + Bitacora.Codigo + "</th>";
                html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                html = html + "<th>" + Bitacora.Usuario + "</th>";             
                html=html + "</tr>"
            }
            $(JsBitacora.Controles.TablaBitacora).html(html);
            CargarDatasource();
        },
    },
    "Consulta": {
        "ConsultarBitacora": function () {
            let bitacora = new Object();
            bitacora.FechaDesde = $(JsBitacora.Controles.txtFechaDesde).val();
            bitacora.FechaHasta = $(JsBitacora.Controles.txtFechaHasta).val();
            $("#loading").fadeIn();
            execAjaxCall("/BitacoraFonatel/ObtenerListaBitacora", "POST", bitacora)
                .then((obj) => {
                    JsBitacora.Variables.ListaBitacora = obj.objetoRespuesta;
                    JsBitacora.Metodos.CargarTablaBitacora();
                }).catch((data) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        }
    }

}

$(document).on("click", JsBitacora.Controles.btnBuscarBitacora, function (e) {
    e.preventDefault();
    JsBitacora.Consulta.ConsultarBitacora();
});


$(document).on("click", JsBitacora.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Home";
        });
});


$(function () {
    $(JsBitacora.Controles.txtFechaDesde).val("") ;
    $(JsBitacora.Controles.txtFechaHasta).val("");
    JsBitacora.Consulta.ConsultarBitacora();
})













