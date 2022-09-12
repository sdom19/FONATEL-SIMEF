JsBitacora= {
    "Controles": {
        "btnCancelar": "#btnCancelarBitacora",
        "btnBuscarBitacora": "#btnBuscarBitacora",
        "txtFechaHasta": "#txtFechaHasta",
        "txtFechaDesde": "#txtFechaDesde",
        "TablaBitacora": "#TableBitacoraFonatel tbody",
        "ddlPantalla": "#ddlPantalla",
        "ddlUsuario": "#ddlUsuario",
        "ddlAccion":"#ddlAccion"
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


                if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Editar) {
                    let json = JSON.parse(Bitacora.ValorDiferencial);

                    for (var objeto in json) {
                        if (objeto !="NoSerialize") {
                            if (Array.isArray(json[objeto])) {
                                let array = json[objeto];
                                html = html + "<tr>"
                                html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                                html = html + "<th>" + Bitacora.Codigo + "</th>";
                                html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                                html = html + "<th>" + Bitacora.Usuario + "</th>";
                                html = html + "<th>" + objeto + "</th>";
                                html = html + "<th>N/A</th>";
                                html = html + "<th>" + array[0] + "</th>";
                                html = html + "<th>" + array[1] + "</th>";
                            
                                html = html + "</tr>";
                            }
                        }
                    }
                }
                else if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Insertar)
                {
                    let json = JSON.parse(Bitacora.ValorInicial);
                    for (var objeto in json) {  
                        if (objeto != "NoSerialize") {
                                let array = json[objeto];
                                html = html + "<tr>"
                                html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                                html = html + "<th>" + Bitacora.Codigo + "</th>";
                                html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                                html = html + "<th>" + Bitacora.Usuario + "</th>";
                                html = html + "<th>" + objeto + "</th>";
                                html = html + "<th>" + array + "</th>";
                                html = html + "<th>N/A</th>";
                                html = html + "<th>N/A</th>";
                                html = html + "</tr>";
                        }
                    }
                }

                else if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Clonar) {
                    let json = JSON.parse(Bitacora.ValorInicial);
                    let jsonActual = JSON.parse(Bitacora.ValorActual);



                    for (var objeto in json) {
                        if (objeto != "NoSerialize") {
                            let array = json[objeto];
                            let array2 = jsonActual[objeto];
                            html = html + "<tr>"
                            html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                            html = html + "<th>" + Bitacora.Codigo + "</th>";
                            html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                            html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                            html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                            html = html + "<th>" + Bitacora.Usuario + "</th>";
                            html = html + "<th>" + objeto + "</th>";
                            html = html + "<th>" + array + "</th>";
                            html = html + "<th>N/A</th>";
                            html = html + "<th>" + array2+"</th>";
                            html = html + "</tr>";
                        }
                    }
                }
                else {
                  
                    html = html + "<tr>"
                    html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                    html = html + "<th>" + Bitacora.Codigo + "</th>";
                    html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                    html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                    html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                    html = html + "<th>" + Bitacora.Usuario + "</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "</tr>";
                }
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
            bitacora.Pantalla = $(JsBitacora.Controles.ddlPantalla).val()==null?"":  $(JsBitacora.Controles.ddlPantalla).val().join(',');
            bitacora.AccionNombre = $(JsBitacora.Controles.ddlAccion).val()==null?"": $(JsBitacora.Controles.ddlAccion).val().join(',');
            bitacora.Usuario = $(JsBitacora.Controles.ddlUsuario).val()==null?"":$(JsBitacora.Controles.ddlUsuario).val().join(',');


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
            location.reload()
        });
});


$(function () {
    $(JsBitacora.Controles.txtFechaDesde).val("");
    $(JsBitacora.Controles.txtFechaHasta).val("");
   
})













