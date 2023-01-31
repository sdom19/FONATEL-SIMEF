jsRegistroIndicadorFonatel = {
    "Controles": {
        "tabRegistroIndicador": (id) => `#Tab${id} a`,
        "tabRgistroIndicadorActive": "ul .active a",
        "tabRgistroIndicador": "div.tab-pane",
        "columnasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr",
        "columnaTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr th",
        "filasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table tbody",
        "tablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table",
        "tablaIndicadorRecorrido": "div.tab-pane.active .table-wrapper-fonatel table  tbody  tr",
        "tablaIndicadorRecorridoMultiple": "#tablaIndicador_wrapper table  tbody  tr",
        "tablaIndicador_filter": "div.tab-pane.active #tablaIndicador_filter",



        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-edit-form",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "txtNotasInformante": "#txtNotasInformante",
        "tabActivoRegistroIndicador": "div.tab-pane.active",


        "table": "",
        "btnGuardarCategoría": "#btnGuardarCategoría",
        "btnDescargarPlantillaRegistro": "#btnDescargarPlantillaRegistro",
        "btnCargarPlantillaRegistro": "#btnCargarPlantillaRegistro",
        "inputFileCargarPlantilla": "#inputFileCargarPlantilla",

        "fileCargaRegistro": "#fileCargaRegistro",
        "btnCancelar": "#btnCancelarRegistroIndicador",

        "btnGuardar": "div.tab-pane.active #btnGuardarRegistroIndicador",
        //"btnValidar": "div.tab-pane.active #btnValidarRegistroIndicador",
        "btnValidar": "#btnValidarRegistroIndicador",

        "btnCargarPlantillaRegistro2": "#btnCargarPlantillaRegistro2",
        "btnGuardarRegistroIndicador2": "#btnGuardarRegistroIndicador2",
        "btnValidar2": "#btnValidarRegistroIndicador2",

        "IndicadorCorrecto": "#Indicador1",
        "IndicadorErroneo": "#Indicador2",
        "btnCarga": "#btnCargaRegistroIndicador",
        "btnCargaRegistroIndicador": "#btnCargaRegistroIndicador",
        "btnCargaRegistroIndicadorEdicion": "#btnCargaRegistroIndicadorEdicion",
        "btnGuardarRegistroIndicador": "#btnGuardarRegistroIndicador",
        "btnValidarRegistroIndicador": "#btnValidarRegistroIndicador",
        "btnCancelarRegistroIndicador": "#btnCancelarRegistroIndicador",
        "tabMenu": (id) => `#menu${id}`,
        "txtcantidadIndicadores": "#cantidadIndicadores",
        "tabRgistroIndicadorVerificado": "div.tab-pane.verificado",
        "tabRgistroIndicadoractivo": "div.tab-pane.active",
    },
    "Variables": {
        "VariableIndicador": 1,
        "Validacion": false,
        "paginasActualizadasConSelect2_tablaIndicador": {},
        "ListadoDetalleRegistroIndicador": new Object(),
        "ModoConsulta": false,

    },

    "Metodos": {
        "CrearRegistroIndicador": function () {

            jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador = new Object();
            jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaValorFonatel = [];
            jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorVariableValorFonatel = [];
            let NumeroFila = 0;
            $(jsRegistroIndicadorFonatel.Controles.tablaIndicadorRecorrido).each(function (index) {
                NumeroFila++;
                $(this).children("td").each(function (td) {

                    let registroIndicador = new Object();
                    registroIndicador.SolicitudId = ObtenerValorParametroUrl("idSolicitud");
                    registroIndicador.FormularioId = ObtenerValorParametroUrl("idFormulario");
                    registroIndicador.IndicadorId = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
                    registroIndicador.Valor = "";
                    registroIndicador.NumeroFila = NumeroFila;
                    if ($(this).children("input").length != 0) {
                        var input = $(this).children("input");
                        if (input.hasClass("VariableDato")) {
                            let registroVariable = new Object();
                            registroVariable.SolicitudId = ObtenerValorParametroUrl("idSolicitud");
                            registroVariable.FormularioId = ObtenerValorParametroUrl("idFormulario");
                            registroVariable.IndicadorId = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
                            registroVariable.Valor = "";
                            registroVariable.NumeroFila = NumeroFila;
                            registroVariable.IdVariable = $(input).attr("name").replace("name_", "");
                            registroVariable.Valor = input.val();
                            if (registroVariable.Valor.length != 0) {
                                jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorVariableValorFonatel.push(registroVariable);
                            }
                        } else {
                            registroIndicador.IdCategoria = $(input).attr("name").replace("name_", "");
                            registroIndicador.Valor = input.val();
                        }            
                    }
                    else if ($(this).children(".select2-wrapper").length != 0) {
                        var select = $(this).children(".select2-wrapper");
                        registroIndicador.IdCategoria = $(select.children(".listasDesplegables")).attr("name").replace("name_", "");
                        registroIndicador.Valor = select.children(".listasDesplegables").val();
                    }
                    if (registroIndicador.Valor.length != 0) {
                        jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaValorFonatel.push(registroIndicador);
                    }
                });
            });
        },
        "CrearRegistroIndicadorMultiple": function () {

            jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador = new Object();
            jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaValorFonatel = [];
            jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorVariableValorFonatel = [];
            let NumeroFila = 0;
            var cantIndicadores = parseInt($(jsRegistroIndicadorFonatel.Controles.txtcantidadIndicadores).val());
            for (var i = 1; i <= cantIndicadores; i++) {
                var cantFilas = $(jsRegistroIndicadorFonatel.Controles.tabMenu(i)).find(jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador).val();
                if (cantFilas > 0) {
                    NumeroFila = 0;
                    $(jsRegistroIndicadorFonatel.Controles.tabMenu(i)).find(jsRegistroIndicadorFonatel.Controles.tablaIndicadorRecorridoMultiple).each(function (index) {
                        NumeroFila++;
                        $(this).children("td").each(function (td) {

                            let registroIndicador = new Object();
                            registroIndicador.SolicitudId = ObtenerValorParametroUrl("idSolicitud");
                            registroIndicador.FormularioId = ObtenerValorParametroUrl("idFormulario");
                            registroIndicador.IndicadorId = $(jsRegistroIndicadorFonatel.Controles.tabRegistroIndicador(i)).attr('data-Indicador');
                            registroIndicador.Valor = "";
                            registroIndicador.NumeroFila = NumeroFila;
                            if ($(this).children("input").length != 0) {
                                var input = $(this).children("input");
                                if (input.hasClass("VariableDato")) {
                                    let registroVariable = new Object();
                                    registroVariable.SolicitudId = ObtenerValorParametroUrl("idSolicitud");
                                    registroVariable.FormularioId = ObtenerValorParametroUrl("idFormulario");
                                    registroVariable.IndicadorId = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
                                    registroVariable.Valor = "";
                                    registroVariable.NumeroFila = NumeroFila;
                                    registroVariable.IdVariable = $(input).attr("name").replace("name_", "");
                                    registroVariable.Valor = input.val();
                                    if (registroVariable.Valor.length != 0) {
                                        jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorVariableValorFonatel.push(registroVariable);
                                    }
                                } else {
                                    registroIndicador.IdCategoria = $(input).attr("name").replace("name_", "");
                                    registroIndicador.Valor = input.val();
                                }
                            }
                            else if ($(this).children(".select2-wrapper").length != 0) {
                                var select = $(this).children(".select2-wrapper");
                                registroIndicador.IdCategoria = $(select.children(".listasDesplegables")).attr("name").replace("name_", "");
                                registroIndicador.Valor = select.children(".listasDesplegables").val();
                            }
                            if (registroIndicador.Valor.length != 0) {
                                jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaValorFonatel.push(registroIndicador);
                            }
                        });
                    });
                }
            }
        },
        "CargarColumnasTabla": function () {
            if ($(jsRegistroIndicadorFonatel.Controles.columnaTablaIndicador).length == 0) {
                let html = "<th style='min-width:30PX'>  </th>";
                for (var i = 0; i < jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel.length; i++) {
                    let variable = jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel[i];
                    html = html + variable.html;
                }
                for (var i = 0; i < jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel.length; i++) {
                    let categoria = jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel[i];
                    html = html + "<th style='min-width:160PX' scope='col'>" + categoria.NombreCategoria + "</th>";
                }
                $(jsRegistroIndicadorFonatel.Controles.columnasTablaIndicador).html(html);
            }
            else {
                EliminarDatasource(jsRegistroIndicadorFonatel.Controles.tablaIndicador);
            }
        },

        "CargarFilasTabla": function (cantidadFilas) {
            $(jsRegistroIndicadorFonatel.Controles.filasTablaIndicador).html("");
            $(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro).prop('disabled', false);
            $(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro).prop('disabled', false);
            let html = "<tr><td></td>";

            for (var i = 0; i < jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel.length; i++) {
                let variable = jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel[i];
                html = html + "<td><input type='number' name='name_" + variable.idVariable + "' class='form-control form-control-fonatel solo_numeros VariableDato' id='[0]-" + variable.idVariable +"'></td>";
            }
            for (var i = 0; i < jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel.length; i++) {
                let categoria = jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel[i];
                html = html + categoria.html;
            }
            html = html + "</tr>"

            for (var i = 0; i < cantidadFilas; i++) {
                $(jsRegistroIndicadorFonatel.Controles.filasTablaIndicador).append(html.replaceAll("[0]", i + 1));
            }

        },


        "CargarDatosVisualizar": function () {
            $('.container button').prop('disabled', true);
            $('textarea').prop('disabled', true);
        },

        "CargarExcel": function () {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
        },

        "DescargarExcel": function () {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
        },

        "ImportarExcel": function () {
            var data;
            data = new FormData();
            data.append('file', $(jsRegistroIndicadorFonatel.Controles.inputFileCargarPlantilla)[0].files[0]);
            let registroIndicador = new Object();
            registroIndicador.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
            registroIndicador.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
            registroIndicador.IdIndicadorString = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
            data.append('datos', JSON.stringify({ datos: registroIndicador }));
            data.append('cantidadFilas', $(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador).val());
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/RegistroIndicadorFonatel/CargarExcel',
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
                    $(jsRegistroIndicadorFonatel.Controles.inputFileCargarPlantilla).val('');
                    var respuesta = JSON.parse(obj);
                    if (respuesta.HayError == jsUtilidades.Variables.Error.NoError) {
                        jsRegistroIndicadorFonatel.Metodos.CargarValores(respuesta.objetoRespuesta);
                        //jsMensajes.Metodos.OkAlertModal("Los Datos han sido cargados")
                        //.set('onok', function (closeEvent) { window.location.href = "/Fonatel/RelacionCategoria/index" });
                    } else if (respuesta.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal(respuesta.MensajeError)
                            .set('onok', function (closeEvent) { location.reload(); });
                    }


                }
            }).fail(function (obj) {
                $(jsRegistroIndicadorFonatel.Controles.inputFileCargarPlantilla).val('');
                jsMensajes.Metodos.OkAlertErrorModal("Error al cargar los Datos")
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();

            })
        },

        "CargarValores": function (listaDetalles) {

            jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador = [];
            let NumeroFila = 0;
            $(jsRegistroIndicadorFonatel.Controles.tablaIndicadorRecorrido).each(function (index) {
                NumeroFila++;
                var elementotd = $(this).children("td");
                listaDetalles.DetalleRegistroIndicadorCategoriaValorFonatel.forEach(function (item) {
                    if (item.NumeroFila == NumeroFila) {
                        var elementoInput = elementotd.find("[name='name_" + item.idCategoria + "']");
                        elementoInput.val(item.Valor).change();
                    }

                });
                $(jsRegistroIndicadorFonatel.Controles.tablaIndicador_filter).addClass("hidden");
            });
            NumeroFila = 0
            $(jsRegistroIndicadorFonatel.Controles.tablaIndicadorRecorrido).each(function (index) {
                NumeroFila++;
                var elementotd = $(this).children("td");
                listaDetalles.DetalleRegistroIndicadorVariableValorFonatel.forEach(function (item) {
                    if (item.NumeroFila == NumeroFila) {
                        var elementoInput = elementotd.find("[name='name_" + item.IdVariable + "']");
                        elementoInput.val(item.Valor).change();
                    }

                });
                $(jsRegistroIndicadorFonatel.Controles.tablaIndicador_filter).addClass("hidden");
            });
            if (!jsRegistroIndicadorFonatel.Variables.ModoConsulta) {
                jsMensajes.Metodos.OkAlertModal("La Plantilla ha sido cargada")
                    .set('onok', function (closeEvent) { $("#loading").fadeOut(); });
            }
        },
        "ValidarRegistroIndicadorDetalleValor": function () {
            $("#loading").fadeIn();
            jsRegistroIndicadorFonatel.Metodos.CrearRegistroIndicador();
            let detalleIndicadorValor = jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador;
            if (detalleIndicadorValor.length > 0) {
                $.ajax({
                    url: '/RegistroIndicadorFonatel/ValidarRegistroIndicadorDetalleValor',
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
                            jsMensajes.Metodos.OkAlertModal("Los Datos han sido guardados")
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
            } else {
                jsMensajes.Metodos.OkAlertErrorModal("No ha completado la ninguna información")
                    .set('onok', function (closeEvent) { $("#loading").fadeOut(); });
            }

        },
        "ValidarDetalle": function () {

            var valor = "";
            $(jsRegistroIndicadorFonatel.Controles.tablaIndicadorRecorrido).each(function (index) {
                $(this).children("td").each(function (td) {

                    if ($(this).children("input").length != 0) {
                        var input = $(this).children("input");
                        valor = input.val();
                    }
                    else if ($(this).children(".select2-wrapper").length != 0) {
                        var select = $(this).children(".select2-wrapper");
                        valor = select.children(".listasDesplegables").val();
                    }
                    //else if ($(this).children(".VariableDato").length != 0) {
                    //    var etiqueta = $(this).children(".VariableDato");
                    //    registroIndicador.IdCategoria = $(etiqueta).attr("name").replace("name_", "");
                    //    registroIndicador.Valor = $(etiqueta).text();
                    //}
                    if (valor.length == 0) {
                        return false;
                    }
                });
            });
            return true;
        },
        "VerificarCargado": function () {
            var cantIndicadores = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicador).length;
            var cantVerificados = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorVerificado).length;

            if (cantIndicadores == cantVerificados) {
                return true;
            } else {
                return false;
            }          
        },
    },
    "Consultas": {

        "InsertarRegistroIndicadorDetalleValor": function () {
            $("#loading").fadeIn();
            jsRegistroIndicadorFonatel.Metodos.CrearRegistroIndicadorMultiple();
            let detalleIndicadorValor = jsRegistroIndicadorFonatel.Variables.ListadoDetalleRegistroIndicador;
            if (detalleIndicadorValor.DetalleRegistroIndicadorCategoriaValorFonatel.length > 0 || detalleIndicadorValor.DetalleRegistroIndicadorVariableValorFonatel.length > 0) {
                $.ajax({
                    url: '/RegistroIndicadorFonatel/InsertarRegistroIndicadorVariable',
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
                            jsRegistroIndicadorFonatel.Consultas.ActualizarRegistroIndicador(1);
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
            } else {
                jsMensajes.Metodos.OkAlertErrorModal("No ha completado la ninguna información")
                    .set('onok', function (closeEvent) { $("#loading").fadeOut(); });
            }

        },
        "ActualizarDetalleRegistroIndicadorFonatel": function () {
            $("#loading").fadeIn();
            var listaActDetalleRegistroIndicador = [];
            var cantIndicadores = parseInt($(jsRegistroIndicadorFonatel.Controles.txtcantidadIndicadores).val());
            for (var i = 1; i <= cantIndicadores; i++) {
                let obj = new Object();
                obj.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
                obj.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
                obj.IdIndicadorString = $(jsRegistroIndicadorFonatel.Controles.tabRegistroIndicador(i)).attr('data-Indicador');
                obj.CantidadFilas = $(jsRegistroIndicadorFonatel.Controles.tabMenu(i)).find(jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador).val();
                obj.NotasInformante = $(jsRegistroIndicadorFonatel.Controles.tabMenu(i)).find(jsRegistroIndicadorFonatel.Controles.txtNotasInformante).val();
                if (obj.CantidadFilas > 0) {
                    listaActDetalleRegistroIndicador.push(obj);
                }
            }

            $.ajax({
                url: '/RegistroIndicadorFonatel/ActualizarDetalleRegistroIndicadorFonatel',
                type: 'post',
                contentType: 'application/json;charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify({ listaActDetalleRegistroIndicador: listaActDetalleRegistroIndicador }),
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsRegistroIndicadorFonatel.Consultas.InsertarRegistroIndicadorDetalleValor();
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

        "ConsultaRegistroIndicadorDetalle": function () {
            $("#loading").fadeIn();

            let detalleIndicadorFonatel = new Object();
            detalleIndicadorFonatel.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
            detalleIndicadorFonatel.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
            detalleIndicadorFonatel.IdIndicadorString = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
            detalleIndicadorFonatel.CantidadFilas = $(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador).val();
            execAjaxCall("/RegistroIndicadorFonatel/ConsultaRegistroIndicadorDetalle", "POST", detalleIndicadorFonatel = detalleIndicadorFonatel)
                .then((obj) => {
                    $(jsRegistroIndicadorFonatel.Controles.tablaIndicador).removeClass("hidden");
                    jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel = obj.objetoRespuesta[0];
                    if (jsRegistroIndicadorFonatel.Variables.detalleIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel.length > 0) {
                        jsRegistroIndicadorFonatel.Metodos.CargarColumnasTabla();
                        jsRegistroIndicadorFonatel.Metodos.CargarFilasTabla(detalleIndicadorFonatel.CantidadFilas);
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
                    CargarDatasourceV2(jsRegistroIndicadorFonatel.Controles.tablaIndicador);
                    if (jsRegistroIndicadorFonatel.Variables.ModoConsulta) {
                        jsRegistroIndicadorFonatel.Consultas.ConsultaDetalleRegistroIndicadorValoresFonatel();
                    }
                    $(jsRegistroIndicadorFonatel.Controles.tablaIndicador_filter).addClass("hidden");
                    $("#loading").fadeOut();
                });
        },

        "ConsultaDetalleRegistroIndicadorValoresFonatel": function () {
            $("#loading").fadeIn();

            let detalle = new Object();
            detalle.FormularioId = ObtenerValorParametroUrl("idFormulario");
            detalle.Solicitudid = ObtenerValorParametroUrl("idSolicitud");
            detalle.IndicadorId = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
            execAjaxCall("/RegistroIndicadorFonatel/ObtenerListaDetalleRegistroIndicadorValoresFonatel", "POST", detalle)
                .then((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsRegistroIndicadorFonatel.Metodos.CargarValores(obj.objetoRespuesta);
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
        "ActualizarRegistroIndicador": function (estado) {
            $("#loading").fadeIn();

            let registro = new Object();
            registro.FormularioId = ObtenerValorParametroUrl("idFormulario");
            registro.Solicitudid = ObtenerValorParametroUrl("idSolicitud");
            registro.IdEstado = estado;
            execAjaxCall("/RegistroIndicadorFonatel/ActualizarRegistroIndicador", "POST", registro)
                .then((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertModal("Los Datos han sido guardados")
                            .set('onok', function (closeEvent) { $("#loading").fadeOut(); });
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
        }

    }

}


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCancelarRegistroIndicador, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index";
        });
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnGuardarRegistroIndicador, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsRegistroIndicadorFonatel.Consultas.ActualizarDetalleRegistroIndicadorFonatel();
        });


});


//BTN GUARDAR REGISTRO SEGUNDO INDICADOR SOLO PARA ENTREGA DOCUMENTO SIMER REVISAR LINEA 293 - FRANCISCO VINDAS RUIZ
$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnGuardarRegistroIndicador2, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido guardado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index"; });
        });
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCarga, function (e) {
    e.preventDefault();


    jsRegistroIndicadorFonatel.Consultas.InsertarRegistroIndicadorDetalleValor();



    //jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar la carga de la información?", jsMensajes.Variables.actionType.agregar)
    //    .set('onok', function (closeEvent) {
    //        jsMensajes.Metodos.OkAlertModal("La carga de información ha sido completada")
    //            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index"; });
    //    });
});



//DESCARGAR EXCEL
$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro, function () {

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
        .set('onok', function (closeEvent) {

            var listaParametros = [];
            listaParametros.push(ObtenerValorParametroUrl("idSolicitud"));
            listaParametros.push(ObtenerValorParametroUrl("idFormulario"));
            listaParametros.push($(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador'));
            listaParametros.push($(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador).val());

            window.open(jsUtilidades.Variables.urlOrigen + "/RegistroIndicadorFonatel/DescargarExcel?listaParametros=" + listaParametros);
            jsRegistroIndicadorFonatel.Metodos.DescargarExcel();
        });
});

//DESCARGAR EXCEL
$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnGuardarCategoría, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            window.open(jsUtilidades.Variables.urlOrigen + "/RegistroIndicadorFonatel/DescargarExcel");
            jsRegistroIndicadorFonatel.Metodos.CargarExcel();
        });
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro, function () {
    $(jsRegistroIndicadorFonatel.Controles.inputFileCargarPlantilla).click();
});

//BTN CARGAR SEGUNDO INDICADOR SOLO PARA ENTREGA DOCUMENTO SIMER REVISAR LINEA 293 - FRANCISCO VINDAS RUIZ
$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro2, function () {
    $(jsRegistroIndicadorFonatel.Controles.inputFileCargarPlantilla).click();
});

$(document).on("change", jsRegistroIndicadorFonatel.Controles.inputFileCargarPlantilla, function (e) {
    jsRegistroIndicadorFonatel.Metodos.ImportarExcel();
});

$(document).on("click", jsRegistroIndicadorFonatel.Controles.IndicadorCorrecto, function () {

    jsRegistroIndicadorFonatel.Variables.Validacion = false;
});

$(document).on("click", jsRegistroIndicadorFonatel.Controles.IndicadorErroneo, function () {

    $(jsRegistroIndicadorFonatel.Controles.btnCarga).prop("disabled", true);
    jsRegistroIndicadorFonatel.Variables.Validacion = true;

});



$(document).on('draw.dt', jsRegistroIndicadorFonatel.Controles.tablaIndicador, function (e) {
    setSelect2();
});




$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnValidar, function () {
    if (jsRegistroIndicadorFonatel.Metodos.ValidarDetalle()) {
        if (jsRegistroIndicadorFonatel.Variables.Validacion == false) {
            $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadoractivo).addClass("verificado");
            jsMensajes.Metodos.OkAlertModal("Validación Exitosa <br><br> La información ingresada cumple con los criterios de validación.");
            if (jsRegistroIndicadorFonatel.Metodos.VerificarCargado()) {
                $(jsRegistroIndicadorFonatel.Controles.btnCarga).prop("disabled", false);
            }
        } else {
            jsMensajes.Metodos.OkAlertErrorModal("Fórmula de cambio mensual <br><br> La información ingresada no es congruente con el registro del mes anterior");
        }
    } else {
        jsMensajes.Metodos.OkAlertErrorModal("");
    }


});




//BTN VALIDAR SEGUNDO INDICADOR SOLO PARA ENTREGA DOCUMENTO SIMER REVISAR LINEA 293 - FRANCISCO VINDAS RUIZ
$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnValidar2, function () {


    if (jsRegistroIndicadorFonatel.Variables.Validacion == false) {

        jsMensajes.Metodos.OkAlertModal("La información ingresada cumple con los criterios de validación.");
        $(jsRegistroIndicadorFonatel.Controles.btnCarga).prop("disabled", false);

    } else {
        jsMensajes.Metodos.OkAlertErrorModal("Fórmula actualización secuencial <br><br>La información ingresada no cumple con la secuencia con respecto a los registros del periodo anterior");
    }

});

/*
 Evento para cada input Cantidad de Registros de cada tab o indicador.
 */
$(document).on("keypress", jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador, function () {


    if (event.keyCode == 13) {

        var tabla = $(jsRegistroIndicadorFonatel.Controles.tablaIndicador);
        tabla.addClass("revisado");
        jsRegistroIndicadorFonatel.Consultas.ConsultaRegistroIndicadorDetalle();

    }
});

/*
 Evento que captura los eventos de siguiente y atras de los datatables.
 Se maneja una variable que almacena las paginas visitadas de cada tab o indicador, 
 para así refrescar los select2.
 */

function getTabActivoRegistroIndicador() {
    return $(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).attr("id");
}

function setSelect2() {
    $('.listasDesplegables').select2({
        placeholder: "Seleccione",
        width: 'resolve'
    });
}

$(document).on("click", "#btnPruebaCombos", function () {
    setSelect2();
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.tabRegistroIndicador, function () {

    //jsRegistroIndicadorFonatel.Variables.ModoConsulta = false;

    var cantidadFilas = parseInt($(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).find(jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador).val());
    var tabla = $(jsRegistroIndicadorFonatel.Controles.tablaIndicador);
    var ind = tabla.hasClass("revisado");
    if (cantidadFilas > 0 && !ind) {
        tabla.addClass("revisado");
        jsRegistroIndicadorFonatel.Variables.ModoConsulta = true;
        jsRegistroIndicadorFonatel.Consultas.ConsultaRegistroIndicadorDetalle();
    } else {
        jsRegistroIndicadorFonatel.Variables.ModoConsulta = false;
    }

});


$(document).ready(function () {



    $(jsRegistroIndicadorFonatel.Controles.tabRegistroIndicador(1)).click();
});
