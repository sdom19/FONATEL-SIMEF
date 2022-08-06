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
                html = html + "<th>" + Bitacora.Accion + "</th>";
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
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/BitacoraFonatel/ObtenerListaBitacora',
                type: "POST",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { bitacora },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsBitacora.Variables.ListaBitacora = obj.objetoRespuesta;
                        JsBitacora.Metodos.CargarTablaBitacora();
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                    $("#loading").fadeOut();
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

$(document).on("click", JsBitacora.Controles.btnBuscarBitacora, function (e) {
    e.preventDefault();
    JsBitacora.Consulta.ConsultarBitacora();
});


$(document).on("click", JsBitacora.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/BitacoraFonatel/Index";
        });
});














