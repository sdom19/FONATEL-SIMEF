JsBitacora= {
    Controles: {
        btnCancelar: "#btnCancelarBitacora",
        btnBuscarBitacora: "#btnBuscarBitacora",
        btnLimpiar: "#btnLimpiarBitacora",
        txtFechaHasta: "#txtFechaHasta",
        txtFechaDesde: "#txtFechaDesde",
        TablaBitacora: "#TableBitacoraFonatel tbody",
        EncabezadoTablaBitacora: "#TableBitacoraFonatel thead tr",
        FootTablaBitacora: "#TableBitacoraFonatel tfoot tr",
        ddlPantalla: "#ddlPantalla",
        ddlUsuario: "#ddlUsuario",
        ddlAccion: "#ddlAccion"
    },
    Variables: {
        "ListaBitacora":[]
    },

    Metodos: {
        "CargarTablaBitacora": function () {
            EliminarDatasource();

            const codigoRegistro = "Código";
            const camposUnicos = [codigoRegistro];
            const strNoAplica = "N/A";

            //Editar
            JsBitacora.Variables.ListaBitacora.filter(i => i.Accion == jsUtilidades.Variables.Acciones.Editar && i.ValorDiferencial != null && i.ValorDiferencial != "").forEach(objeto => {
                const valorDiferencial = JSON.parse(objeto.ValorDiferencial);
                const campos = Object.keys(valorDiferencial);

                campos.forEach(campo => {
                    if (!camposUnicos.includes(campo)) {
                        camposUnicos.push(campo);
                    }
                });
            });

            //Insertar
            JsBitacora.Variables.ListaBitacora.filter(i => i.Accion == jsUtilidades.Variables.Acciones.Insertar && i.ValorInicial != null && i.ValorInicial != "").forEach(objeto => {
                const valorInicial = JSON.parse(objeto.ValorInicial);
                const campos = Object.keys(valorInicial);

                campos.forEach(campo => {
                    if (!camposUnicos.includes(campo)) {
                        camposUnicos.push(campo);
                    }
                });
            });

            //Clonar
            JsBitacora.Variables.ListaBitacora.filter(i => i.Accion == jsUtilidades.Variables.Acciones.Clonar && i.ValorInicial != null && i.ValorInicial != "").forEach(objeto => {
                const valorInicial = JSON.parse(objeto.ValorInicial);
                const campos = Object.keys(valorInicial);

                campos.forEach(campo => {
                    if (!camposUnicos.includes(campo)) {
                        camposUnicos.push(campo);
                    }
                });
            });

            //Activar
            JsBitacora.Variables.ListaBitacora.filter(i => i.Accion == jsUtilidades.Variables.Acciones.Activar && i.ValorDiferencial != null && i.ValorDiferencial != "").forEach(objeto => {
                const valorDiferencial = JSON.parse(objeto.ValorDiferencial);
                const campos = Object.keys(valorDiferencial);

                campos.forEach(campo => {
                    if (!camposUnicos.includes(campo)) {
                        camposUnicos.push(campo);
                    }
                });
            });

            //Desactivar
            JsBitacora.Variables.ListaBitacora.filter(i => i.Accion == jsUtilidades.Variables.Acciones.Desactivar && i.ValorDiferencial != null && i.ValorDiferencial != "").forEach(objeto => {
                const valorDiferencial = JSON.parse(objeto.ValorDiferencial);
                const campos = Object.keys(valorDiferencial);

                campos.forEach(campo => {
                    if (!camposUnicos.includes(campo)) {
                        camposUnicos.push(campo);
                    }
                });
            });

            //Campos al inicio del array
            const camposGenerales = ["Pantalla", "Usuario", "Acción", "Fecha", "Hora"];
            const camposValores = ["Valor Anterior", "Valor Actual"];
            const columnasTabla = [...camposGenerales, ...camposUnicos, ...camposValores];
         

            let htmlEncabezado = "";
            let htmlFoot = "";
            for (let i = 0; i < columnasTabla.length; i++) {
                htmlEncabezado += '<th scope="col">' + columnasTabla[i] + '</th>';

                if (columnasTabla[i] == "Fecha" || columnasTabla[i] == "Hora") {
                    htmlFoot += '<th></th>';
                } else {
                    htmlFoot += '<th class="select2-wrapper"></th>';
                }
            }

            $(JsBitacora.Controles.EncabezadoTablaBitacora).html(htmlEncabezado);
            $(JsBitacora.Controles.FootTablaBitacora).html(htmlFoot);


            let html = "";
            for(var i = 0; i < JsBitacora.Variables.ListaBitacora.length; i++) {
                let Bitacora = JsBitacora.Variables.ListaBitacora[i];

                if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Insertar && Bitacora.ValorInicial != null && Bitacora.ValorInicial != "") {

                    html = html + "<tr>"
                    html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                    html = html + "<th>" + Bitacora.Usuario + "</th>";
                    html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                    html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                    html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                    html = html + "<th>" + Bitacora.Codigo + "</th>";

                    let json = JSON.parse(Bitacora.ValorInicial);
                    camposUnicos.forEach(col => {
                        if (col != codigoRegistro) {
                            html = html + "<th>" + (json[col] ?? strNoAplica) + "</th>";
                        }
                    })

                    html = html + "<th>" + strNoAplica + "</th>";
                    html = html + "<th>" + strNoAplica + "</th>";
                    html = html + "</tr>";

                }
                else if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Editar && Bitacora.ValorDiferencial != null && Bitacora.ValorDiferencial != "") {

                    let json = JSON.parse(Bitacora.ValorDiferencial);

                    for (var objeto in json) {
                        if (objeto != "NoSerialize") {
                            if (Array.isArray(json[objeto])) {
                                let array = json[objeto];
                                html = html + "<tr>"
                                html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                                html = html + "<th>" + Bitacora.Usuario + "</th>";
                                html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                                html = html + "<th>" + Bitacora.Codigo + "</th>";

                                camposUnicos.forEach(col => {
                                    if (col != codigoRegistro) {
                                        if (col == objeto) {
                                            html = html + "<th>" + (array[0] ?? strNoAplica) + "</th>";
                                        } else {
                                            html = html + "<th>" + strNoAplica + "</th>";
                                        }
                                    }
                                })

                                html = html + "<th>" + array[0] + "</th>";
                                html = html + "<th>" + array[1] + "</th>";
                                html = html + "</tr>";
                            }
                        }
                    }
                }
            }

            $(JsBitacora.Controles.TablaBitacora).html(html);
            CargarDatasourceBitacora();
            return;

            //--------------------------------

            for (var i = 0; i < JsBitacora.Variables.ListaBitacora.length; i++) {
                let Bitacora = JsBitacora.Variables.ListaBitacora[i];


                if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Editar && Bitacora.ValorDiferencial!='') {
                    let json = JSON.parse(Bitacora.ValorDiferencial);

                    for (var objeto in json) {
                        if (objeto !="NoSerialize") {
                            if (Array.isArray(json[objeto])) {
                                let array = json[objeto];
                                html = html + "<tr>"
                                html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                                html = html + "<th>" + Bitacora.Usuario + "</th>";
                                html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                                html = html + "<th>" + objeto + "</th>"; //Elemento Afectado
                                html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                                html = html + "<th>" + Bitacora.Codigo + "</th>";
                                html = html + "<th>N/A</th>"; //Valor Inicial
                                html = html + "<th>" + array[0] + "</th>"; // Valor Anterior 
                                html = html + "<th>" + array[1] + "</th>"; // Valor Actual
                                html = html + "</tr>";
                            }
                        }
                    }
                }
                else if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Insertar && Bitacora.ValorInicial!='')
                {
                    let json = JSON.parse(Bitacora.ValorInicial);
                    for (var objeto in json) {  
                        if (objeto != "NoSerialize") {
                                let array = json[objeto];
                                html = html + "<tr>"
                                html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                                html = html + "<th>" + Bitacora.Usuario + "</th>";
                                html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                                html = html + "<th>" + objeto + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                                html = html + "<th>" + Bitacora.Codigo + "</th>";
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
                            html = html + "<th>" + Bitacora.Usuario + "</th>";
                            html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                            html = html + "<th>" + objeto + "</th>";
                            html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                            html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                            html = html + "<th>" + Bitacora.Codigo + "</th>";
                            html = html + "<th>" + array + "</th>";
                            html = html + "<th>N/A</th>";
                            html = html + "<th>" + array2+"</th>";
                            html = html + "</tr>";
                        }
                    }
                }
                else if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Activar && Bitacora.ValorDiferencial != '') {
                    let json = JSON.parse(Bitacora.ValorDiferencial);

                        for (var objeto in json) {
                            if (objeto != "NoSerialize") {
                                if (Array.isArray(json[objeto])) {
                                    let array = json[objeto];
                                    html = html + "<tr>"
                                    html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                                    html = html + "<th>" + Bitacora.Usuario + "</th>";
                                    html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                                    html = html + "<th>" + objeto + "</th>";
                                    html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                                    html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                                    html = html + "<th>" + Bitacora.Codigo + "</th>";
                                    html = html + "<th>N/A</th>";
                                    html = html + "<th>" + array[0] + "</th>";
                                    html = html + "<th>" + array[1] + "</th>";

                                    html = html + "</tr>";
                                }
                            }
                        }
                }
                else if (Bitacora.Accion == jsUtilidades.Variables.Acciones.Desactivar && Bitacora.ValorDiferencial != '') {
                    let json = JSON.parse(Bitacora.ValorDiferencial);

                    for (var objeto in json) {
                        if (objeto != "NoSerialize") {
                            if (Array.isArray(json[objeto])) {
                                let array = json[objeto];
                                html = html + "<tr>"
                                html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                                html = html + "<th>" + Bitacora.Usuario + "</th>";
                                html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                                html = html + "<th>" + objeto + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                                html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                                html = html + "<th>" + Bitacora.Codigo + "</th>";
                                html = html + "<th>N/A</th>";
                                html = html + "<th>" + array[0] + "</th>";
                                html = html + "<th>" + array[1] + "</th>";

                                html = html + "</tr>";
                            }
                        }
                    }
                }
                else {
                  
                    html = html + "<tr>"
                    html = html + "<th scope='row'>" + Bitacora.Pantalla + "</th>";
                    html = html + "<th>" + Bitacora.Usuario + "</th>";
                    html = html + "<th>" + Bitacora.AccionNombre + "</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "<th>" + moment(Bitacora.Fecha).format('MM/DD/YYYY') + "</th>";
                    html = html + "<th>" + moment(Bitacora.Fecha).format('hh:mm a') + "</th>";
                    html = html + "<th>" + Bitacora.Codigo + "</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "<th>N/A</th>";
                    html = html + "</tr>";
                }
            }
            $(JsBitacora.Controles.TablaBitacora).html(html);
            CargarDatasourceBitacora();
        },

        LimpiarControles: function () {
            SeleccionarItemSelect2(JsBitacora.Controles.ddlPantalla, "");
            SeleccionarItemSelect2(JsBitacora.Controles.ddlUsuario, "");
            SeleccionarItemSelect2(JsBitacora.Controles.ddlAccion, "");

            $(JsBitacora.Controles.txtFechaDesde).val("");
            $(JsBitacora.Controles.txtFechaHasta).val("");

            EliminarDatasource();
            $(JsBitacora.Controles.TablaBitacora).html("");
            CargarDatasourceBitacora();
        }
    },
    Consulta: {
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
                    
                }).catch((data) => {
                    console.log(data);
                    if (data.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal(data.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    JsBitacora.Metodos.CargarTablaBitacora();
                    $("#loading").fadeOut();
                });
        }
    }

}

$(document).on("click", JsBitacora.Controles.btnBuscarBitacora, function (e) {
    e.preventDefault();
    JsBitacora.Consulta.ConsultarBitacora();
});


$(document).on("click", JsBitacora.Controles.btnLimpiar, function (e) {
    JsBitacora.Metodos.LimpiarControles();
});

$(document).on("click", JsBitacora.Controles.btnCancelar, function (e) {
    e.preventDefault();
    if (consultasFonatel) { return; }
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            location.reload()
        });
});


$(function () {
    $(JsBitacora.Controles.txtFechaDesde).val("");
    $(JsBitacora.Controles.txtFechaHasta).val("");
})















