jsRegistroIndicadorFonatelEdit= {
    "Controles": {

        //TABS
        "tabRegistroIndicador": (id) => `#Tab${id} a`,
        "tabRgistroIndicadorActive": "ul .active a",
        "tabRgistroIndicador": "div.tab-pane",
        "tabActivoRegistroIndicador": "div.tab-pane.active",

        //TABLA PRINCIPAL DE EDITAR REGISTRO
        "TablaEditarRegistroIndicador": "#TablaEditarRegistroIndicador tbody",
        "btnDescargaPrincipal": "#TablaEditarRegistroIndicador tbody tr td .btn-download",

        //TABLA DETALLES
        "tablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table",
        "columnasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr",
        "columnaTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr th",
        "filasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table tbody",
        "tablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table",
        "tablaIndicadorRecorrido": "div.tab-pane.active .table-wrapper-fonatel table  tbody  tr",

        //CONTROLES DE FORMULARIO
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "txtNotasInformante": "#txtNotasInformante",
        "txtNotasEncargado": "#txtNotasEncargado",

        //BOTONES DEL FORMULARIO
        "btnDescargarPlantillaRegistro": "#btnDescargarPlantillaRegistro",
        "btnCargarPlantillaRegistro": "#btnCargarPlantillaRegistro",
        "btnGuardarRegistroIndicador": "#btnGuardarRegistroIndicador",
        "btnValidar": "#btnValidarRegistroIndicador",
        "btnCancelar": "#btnCancelarRegistroIndicador",

        //DESCARGA DE EXCEL
        "IndicadorCorrecto": "#Indicador1",
        "IndicadorErroneo": "#Indicador2",
        "fileCargaRegistro":"#fileCargaRegistro",
        "inputFileCargarPlantilla": "#inputFileCargarPlantilla",

        //CREAR FILAR
        "InputSelect2": (id, option) => `<div class="select2-wrapper">
                                                    <select class="listasDesplegables" id="${id}" >
                                                    <option></option>${option}</select ></div >`,
        "InputDate": id => `<input type="date" class="form-control form-control-fonatel" id="${id}">`,
        "InputText": (id, placeholder) => `<input type="text" aria-label="${placeholder}" class="form-control form-control-fonatel alfa_numerico" id="${id}" placeholder="${placeholder}" style="min-width:150px;">`,


    },

    "Variables": {

        "VariableIndicador": 1,
        "Validacion": false,
        "paginasActualizadasConSelect2_tablaIndicador": {},
        "DetalleRegistroIndicador": [],
        "ListadoDetalleRegistroIndicador": [],
        "ModoColsuta": false
    },

    "Mensajes": {

    },

    "Metodos": {

        "CrearRegistroIndicador": function () {

            jsRegistroIndicadorFonatelEdit.Variables.ListadoDetalleRegistroIndicador = [];
            let NumeroFila = 0

            $(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicadorRecorrido).each(function (index) {
                NumeroFila++;
                $(this).children("td").each(function (td) {

                    let registroIndicador = new Object();

                    registroIndicador.SolicitudId = ObtenerValorParametroUrl("idSolicitud");
                    registroIndicador.FormularioId = ObtenerValorParametroUrl("idFormulario");
                    registroIndicador.IndicadorId = $(jsRegistroIndicadorFonatelEdit.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
                    registroIndicador.Valor = "";
                    registroIndicador.NumeroFila = NumeroFila;

                    if ($(this).children("input").length != 0) {
                        var input = $(this).children("input");
                        registroIndicador.IdCategoria = $(input).attr("name").replace("name_", "");
                        registroIndicador.Valor = input.val();
                    }
                    else if ($(this).children(".select2-wrapper").length != 0) {
                        var select = $(this).children(".select2-wrapper");
                        registroIndicador.IdCategoria = $(select.children(".listasDesplegables")).attr("name").replace("name_", "");
                        registroIndicador.Valor = select.children(".listasDesplegables").val();
                    }

                    if (registroIndicador.Valor.length != 0) {
                        jsRegistroIndicadorFonatelEdit.Variables.ListadoDetalleRegistroIndicador.push(registroIndicador);
                    }
                });
            });

        },

        "CargarColumnasTabla": function () {
            if ($(jsRegistroIndicadorFonatelEdit.Controles.columnaTablaIndicador).length == 0) {
                let html = "<th style='min-width:30PX'>  </th>";
                for (var i = 0; i < jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorVariableFonatel.length; i++) {
                    let variable = jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorVariableFonatel[i];
                    html = html + variable.html;
                }
                for (var i = 0; i < jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaFonatel.length; i++) {
                    let categoria = jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaFonatel[i];
                    html = html + "<th style='min-width:160PX' scope='col'>" + categoria.NombreCategoria + "</th>";
                }
                $(jsRegistroIndicadorFonatelEdit.Controles.columnasTablaIndicador).html(html);
            }
            else {
                EliminarDatasource(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicador);
            }
        },

        "CargarFilasTabla": function (cantidadFilas) {

            $(jsRegistroIndicadorFonatelEdit.Controles.filasTablaIndicador).html("");

            $(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatelEdit.Controles.btnDescargarPlantillaRegistro).prop('disabled', false);
            $(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatelEdit.Controles.btnCargarPlantillaRegistro).prop('disabled', false);

            let html = "<tr><td></td>";

            for (var i = 0; i < jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorVariableFonatel.length; i++) {
                html = html + "<td>1</td>";
            }

            for (var i = 0; i < jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaFonatel.length; i++) {
                let categoria = jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaFonatel[i];
                html = html + categoria.html;
            }

            html = html + "</tr>"

            for (var i = 0; i < cantidadFilas; i++) {
                $(jsRegistroIndicadorFonatelEdit.Controles.filasTablaIndicador).append(html.replace("[0]", i + 1));
            }

        },

        "CargarExcel": function () {

            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
                .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/Index"; });
        },

        "CargarDatosTablaIndicador": function (Listado) {

            jsRegistroIndicadorFonatelEdit.Variables.ListadoDetalleRegistroIndicador = [];
            let NumeroFila = 0
            $(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicadorRecorrido).each(function (index) {
                NumeroFila++;
                var elementotd = $(this).children("td");
                Listado.forEach(function (item) {
                    if (item.NumeroFila == NumeroFila) {
                        var elementoInput = elementotd.find("[name='name_" + item.idCategoria + "']");
                        elementoInput.val(item.Valor).change();
                    }
                });
            });

            if (!jsRegistroIndicadorFonatelEdit.Variables.ModoConsulta) {
                jsMensajes.Metodos.OkAlertModal("La Plantilla ha sido cargada")
                    .set('onok', function (closeEvent) { $("#loading").fadeOut(); });
            }
        },

        "DescargarExcel": function () {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
        },

    },

    "Consultas": {

        "ConsultaRegistroIndicadorDetalle": function () {

            $("#loading").fadeIn();

            let detalleIndicadorFonatel = new Object();

            detalleIndicadorFonatel.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
            detalleIndicadorFonatel.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
            detalleIndicadorFonatel.IdIndicadorString = $(jsRegistroIndicadorFonatelEdit.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
            detalleIndicadorFonatel.CantidadFilas = $(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatelEdit.Controles.txtCantidadRegistroIndicador).val();
            execAjaxCall("/EditarFormulario/ConsultaRegistroIndicadorDetalle", "POST", detalleIndicadorFonatel = detalleIndicadorFonatel)
                .then((obj) => {
                    $(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicador).removeClass("hidden");
                    jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador = obj.objetoRespuesta[0];
                    if (jsRegistroIndicadorFonatelEdit.Variables.DetalleRegistroIndicador.DetalleRegistroIndicadorVariableFonatel.length > 0) {
                        jsRegistroIndicadorFonatelEdit.Metodos.CargarColumnasTabla();
                        jsRegistroIndicadorFonatelEdit.Metodos.CargarFilasTabla(detalleIndicadorFonatel.CantidadFilas);
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal("No se posee datos a cargar")
                            .set('onok', function (closeEvent) { });
                    }

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    CargarDatasourceV2(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicador);
                    if (jsRegistroIndicadorFonatelEdit.Variables.ModoConsulta) {
                        jsRegistroIndicadorFonatelEdit.Consultas.ConsultaDetalleRegistroIndicadorCategoriaValorFonatel();
                    }
                    $("#loading").fadeOut();
                });

        },

        "ConsultaDetalleRegistroIndicadorCategoriaValorFonatel": function () {

            $("#loading").fadeIn();

            let detalle = new Object();

            detalle.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
            detalle.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
            detalle.IdIndicadorString = $(jsRegistroIndicadorFonatelEdit.Controles.tabRgistroIndicadorActive).attr('data-Indicador');

            execAjaxCall("/EditarFormulario/ObtenerListaDetalleRegistroIndicadorCategoriaValorFonatel", "POST", detalle)
                .then((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {                    
                        jsRegistroIndicadorFonatelEdit.Metodos.CargarDatosTablaIndicador(obj.objetoRespuesta);
                    }
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarRegistroIndicadorDetalleValor": function () {

            $("#loading").fadeIn();

            jsRegistroIndicadorFonatelEdit.Metodos.CrearRegistroIndicador();

            let detalleIndicadorValor = jsRegistroIndicadorFonatelEdit.Variables.ListadoDetalleRegistroIndicador;

            $.ajax({
                url: '/EditarFormulario/InsertarRegistroIndicadorVariable',
                type: 'post',
                contentType: 'application/json;charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify({ ListaDetalleIndicadorValor: detalleIndicadorValor }),
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertModal("El Formulario ha sido guardado")
                            .set('onok', function (closeEvent) { $("#loading").fadeOut(); });
                    }
                },
                error: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                },
                async: false
            });
        },

        "ActualizarDetalleRegistroIndicador": function () {

            $("#loading").fadeIn();

            let obj = new Object();

            obj.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
            obj.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
            obj.IdIndicadorString = $(jsRegistroIndicadorFonatelEdit.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
            obj.NotasEncargado = $(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatelEdit.Controles.txtNotasEncargado).val();

            execAjaxCall("/EditarFormulario/ActualizarDetalleRegistroIndicador", "POST", obj)
                .then((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsRegistroIndicadorFonatelEdit.Consultas.InsertarRegistroIndicadorDetalleValor();
                        //jsMensajes.Metodos.OkAlertModal("El Formulario ha sido guardado")
                        //    .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/Index";});
                    }

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                })
            //.finally(() => {
            //    $("#loading").fadeOut();
            //});
        },

        "ImportarExcel": function () {

            var data;
            data = new FormData();
            data.append('file', $(jsRegistroIndicadorFonatelEdit.Controles.inputFileCargarPlantilla)[0].files[0]);
            let registroIndicador = new Object();
            registroIndicador.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
            registroIndicador.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
            registroIndicador.IdIndicadorString = $(jsRegistroIndicadorFonatelEdit.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
            data.append('datos', JSON.stringify({ datos: registroIndicador }));
            data.append('cantidadFilas', $(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatelEdit.Controles.txtCantidadRegistroIndicador).val());
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/EditarFormulario/CargarExcel',
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
                    $(jsRegistroIndicadorFonatelEdit.Controles.inputFileCargarPlantilla).val('');
                    var respuesta = JSON.parse(obj);
                    if (respuesta.HayError == jsUtilidades.Variables.Error.NoError) {
                        jsRegistroIndicadorFonatelEdit.Metodos.CargarDatosTablaIndicador(respuesta.objetoRespuesta);

                    } else if (respuesta.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal(respuesta.MensajeError)
                            .set('onok', function (closeEvent) { location.reload(); });
                    }

                }
            }).fail(function (obj) {
                $(jsRegistroIndicadorFonatelEdit.Controles.inputFileCargarPlantilla).val('');
                jsMensajes.Metodos.OkAlertErrorModal("Error al cargar los Datos")
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();

            })
        },

    }

}

$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index";
        });
});

//GUARDAR MODO ENCARGADO
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnGuardarRegistroIndicador, function (e) {

    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {

            jsRegistroIndicadorFonatelEdit.Consultas.ActualizarDetalleRegistroIndicador();

        });
});

//CARGAR MODO INFORMARTE
//$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnCarga, function (e) {
//    e.preventDefault();
//    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar la carga de la información?", jsMensajes.Variables.actionType.agregar)
//        .set('onok', function (closeEvent) {
//            jsMensajes.Metodos.OkAlertModal("La carga de información ha sido completada")
//                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/EditarFormulario/Index"; });
//        });
//});

$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnDescargaPrincipal, function () {
    //jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
    //    .set('onok', function (closeEvent) {
    //        jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
    //            .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/Index" });
    //    });

});

//DESCARGAR
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnDescargarPlantillaRegistro, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
        .set('onok', function (closeEvent) {

            var listaParametros = [];
            listaParametros.push(ObtenerValorParametroUrl("idSolicitud"));
            listaParametros.push(ObtenerValorParametroUrl("idFormulario"));
            listaParametros.push($(jsRegistroIndicadorFonatelEdit.Controles.tabRgistroIndicadorActive).attr('data-Indicador'));
            listaParametros.push($(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatelEdit.Controles.txtCantidadRegistroIndicador).val());
            window.open(jsUtilidades.Variables.urlOrigen + "/EditarFormulario/DescargarExcel?listaParametros=" + listaParametros);

            jsRegistroIndicadorFonatelEdit.Metodos.DescargarExcel();

       });
});

$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnCargarPlantillaRegistro, function () {
    $(jsRegistroIndicadorFonatelEdit.Controles.inputFileCargarPlantilla).click();
});

$(document).on("change", jsRegistroIndicadorFonatelEdit.Controles.inputFileCargarPlantilla, function (e) {
    jsRegistroIndicadorFonatelEdit.Consultas.ImportarExcel();
});

$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnValidar, function () {

});

/*
 Evento para cada input Cantidad de Registros de cada tab o indicador.
 */
$(document).on("keypress", jsRegistroIndicadorFonatelEdit.Controles.txtCantidadRegistroIndicador, function () {

    if (event.keyCode == 13) {

        jsRegistroIndicadorFonatelEdit.Consultas.ConsultaRegistroIndicadorDetalle();

    }
});

/*
 Evento que captura los eventos de siguiente y atras de los datatables.
 Se maneja una variable que almacena las paginas visitadas de cada tab o indicador, 
 para así refrescar los select2.
 */
function eventNextPrevDatatable() {
    $(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicador).on('page.dt', function () {
        var nextPage = $(this).DataTable().page.info().page; 
        let listaPages = jsRegistroIndicadorFonatelEdit.Variables.paginasActualizadasConSelect2_tablaIndicador[getTabActivoRegistroIndicador()];

        if (!listaPages.includes(nextPage)) {
            setTimeout(() => {
                setSelect2()
            }, 0);

            listaPages.push(nextPage);
        }
    });
}

function getTabActivoRegistroIndicador() {
    return $(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).attr("id");
}

function setSelect2() {

    $('.listasDesplegables').select2({
        placeholder: "Seleccione",
        width: 'resolve'
    });
}


$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.tabRegistroIndicador, function () {

    var cantidadFilas = parseInt($(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatelEdit.Controles.txtCantidadRegistroIndicador).val());
    var tabla = $(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicador);
    var ind = tabla.hasClass("revisado");
    if (cantidadFilas > 0 && !ind) {
        tabla.addClass("revisado");
        jsRegistroIndicadorFonatelEdit.Variables.ModoConsulta = true;
        jsRegistroIndicadorFonatelEdit.Consultas.ConsultaRegistroIndicadorDetalle();
    } else {
        jsRegistroIndicadorFonatelEdit.Variables.ModoConsulta = false;
    }

});
    
$(document).ready(function () {

    $(jsRegistroIndicadorFonatelEdit.Controles.tabRegistroIndicador(1)).click();

});


