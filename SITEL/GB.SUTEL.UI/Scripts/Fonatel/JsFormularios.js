﻿JsFormulario = {
    "Controles": {
        "FormularioIndex": "#FormularioIndex",
        "FormFormulario": "#FormFormulario",
        "formCrearFormulario": "#formCrearFormulario",
        "tablaFormulario": "#TablaFormulario tbody",
        "TableIndicadorFormulario": "#TableIndicadorFormulario",
        "TableIndicadorFormularioBody": "#TableIndicadorFormulario tbody",

        "txtCodigoFormulario": "#txtCodigoFormulario",
        "txtNombreFormulario": "#txtNombreFormulario",
        "ddlFrecuanciaEnvio": "#ddlFrecuanciaEnvio",
        "txtDescripcionFormulario": "#txtDescripcionFormulario",

        "txtCantidadIndicadoresFormulario": "#txtCantidadIndicadoresFormulario",
        "txtCodigoFormularioHelp": "#txtCodigoFormularioHelp",
        "txtNombreFormularioHelp": "#txtNombreFormularioHelp",
        "txtCantidadIndicadoresFormularioHelp": "#txtCantidadIndicadoresFormularioHelp",
        "txtDescripcionFormularioHelp": "#txtDescripcionFormularioHelp",
        "ddlFrecuenciaHelp": "#ddlFrecuenciaHelp",

        "btnAgregarFormulario": "#TablaFormulario tbody tr td .btn-add",
        "btnEditarFormulario": "#TablaFormulario tbody tr td .btn-edit",
        "btnDeleteFormulario": "#TablaFormulario tbody tr td .btn-delete",
        "btnVizualizarFormulario": "#TablaFormulario tbody tr td .btn-view",
        "btnDeleteIndicador": "#TableIndicadorFormulario tbody tr td .btn-delete",
        "btnEditarIndicadores": "#TableIndicadorFormulario tbody tr td .btn-edit",

        "btnSiguienteFormulario": "#btnSiguienteFormulario",
        "btnCloneFormulario": "#TablaFormulario tbody tr td .btn-clone",
        "btnDesactivadoFormulario": "#TablaFormulario tbody tr td .btn-power-off",
        "btnActivadoFormulario": "#TablaFormulario tbody tr td .btn-power-on",
        "btnGuardar": "#btnGuardarFormulario",
        "btnCancelar": "#btnCancelarFormulario",
        "btnGuardarIndicador": "#btnGuardarIndicadorFormulario",
        "divContenedor": ".contenedor_formulario",
        "btnAtrasFormularioRegla": "#btnAtrasFormularioRegla",
        "btnAtrasFormularioVisualizar": "#btnAtrasFormularioVisualizar",
        "btnGuardarFormularioCompleto": "#btnGuardarFormularioCompleto",
        "txtTituloHoja": "#txtTituloHoja",
        "txtNotasEncargadoFormulario": "#txtNotasEncargadoFormulario",
        "ddlIndicador": "#ddlIndicador",
        "ddlIndicadorHelp": "#ddlIndicadorHelp",
        "txtTituloHojaHelp": "#txtTituloHojaHelp",
        "step2": "a[href='#step-2']",
        "txtNotasEncargadoFormularioHelp": "#txtNotasEncargadoFormularioHelp",

        "CantidadIndicadoresMax": "#CantidadIndicadoresMax",

        "txtCantidadIndicador": "#txtCantidadRegistroIndicador",
        "tablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table",
        "tabIndicadorActive": "ul .active a",
        "tabActivoIndicador": "div.tab-pane.active",
        "columnaTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr th",
        "columnasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr",
        "filasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table tbody",
        "btnDescargarPlantillaRegistro": "#btnDescargarPlantillaRegistro",
        "btnCargarPlantillaRegistro": "#btnCargarPlantillaRegistro",
        "tablaIndicador_filter": "div.tab-pane.active #tablaIndicador_filter",
    },
    "Variables": {
        "ListadoFormulario": [],
        "ListadoDetalleIndicadores": [],
        "HayError": false,
        "NuevoIndicador": true,
        "CantidadActual": 0
    },

    Mensajes: {
        preguntaGuardarIndicador: "¿Desea agregar el Indicador al Formulario Web?",
        preguntaEditarIndicador: "¿Desea editar el Indicador del Formulario?"
    },

    "Metodos": {

        // Validar los datos que son totalmente requeridos, que ni en guardado parcial deben ser omitidos
        "ValidarDatosMinimos": function () {
            let validar = true;
            $(JsFormulario.Controles.txtCodigoFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtNombreFormularioHelp).addClass("hidden");
            //$(JsFormulario.Controles.ddlFrecuenciaHelp).addClass("hidden");
            $(JsFormulario.Controles.txtCodigoFormulario).parent().removeClass("has-error");
            $(JsFormulario.Controles.txtNombreFormulario).parent().removeClass("has-error");

            let codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            let nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();
            //let idFrecuencia = $(JsFormulario.Controles.ddlFrecuanciaEnvio).val();

            if (codigo.length == 0) {
                $(JsFormulario.Controles.txtCodigoFormularioHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtCodigoFormulario).parent().addClass("has-error");
                validar = false;
            }
            if (nombre.length == 0) {
                $(JsFormulario.Controles.txtNombreFormularioHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtNombreFormulario).parent().addClass("has-error");
                validar = false;
            }
            //if (idFrecuencia == 0) {
            //    $(JsFormulario.Controles.ddlFrecuenciaHelp).removeClass("hidden");
            //    validar = false;
            //}

            return validar;
        },

        "ValidarFormularioWebCrear": function () {
            if ($(JsFormulario.Controles.txtCodigoFormulario).val()?.trim().length > 0 && $(JsFormulario.Controles.txtNombreFormulario).val().trim().length > 0
                && $(JsFormulario.Controles.ddlFrecuanciaEnvio).val().trim().length > 0 && $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val() > 0
                && $(JsFormulario.Controles.txtDescripcionFormulario).val().trim().length > 0) {

                    $(JsFormulario.Controles.btnSiguienteFormulario).prop("disabled", false);
                    $(JsFormulario.Controles.step2).prop("disabled", false);
            }
            else {
                $(JsFormulario.Controles.btnSiguienteFormulario).prop("disabled", true);
                $(JsFormulario.Controles.step2).prop("disabled", true);
            }
        },

        "ValidarFormularioWebTotal": function () {
            let validar = true;

            $(JsFormulario.Controles.txtCodigoFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtNombreFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtDescripcionFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.txtCantidadIndicadoresFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.ddlFrecuenciaHelp).addClass("hidden");

            $(JsFormulario.Controles.txtNombreFormulario).parent().removeClass("has-error");
            $(JsFormulario.Controles.txtCodigoFormulario).parent().removeClass("has-error");
            $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).parent().removeClass("has-error");
            $(JsFormulario.Controles.txtDescripcionFormulario).parent().removeClass("has-error");
            $(JsFormulario.Controles.ddlFrecuanciaEnvio).parent().removeClass("has-error");

            let codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            let nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();
            let descripcion = $(JsFormulario.Controles.txtDescripcionFormulario).val().trim();
            let cantidadIndicadores = $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val().trim();
            let idFrecuencia = $(JsFormulario.Controles.ddlFrecuanciaEnvio).val();

            if (codigo == 0) {
                $(JsFormulario.Controles.txtCodigoFormularioHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtCodigoFormulario).parent().addClass("has-error");
                validar = false;
            }

            if (nombre == 0) {
                $(JsFormulario.Controles.txtNombreFormularioHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtNombreFormulario).parent().addClass("has-error");
                validar = false;
            }

            if (descripcion == 0) {
                $(JsFormulario.Controles.txtDescripcionFormularioHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtDescripcionFormulario).parent().addClass("has-error");
                validar = false;
            }

            if (cantidadIndicadores == 0) {
                $(JsFormulario.Controles.txtCantidadIndicadoresFormularioHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).parent().addClass("has-error");
                validar = false;
            }

            if (idFrecuencia == 0) {
                $(JsFormulario.Controles.ddlFrecuenciaHelp).removeClass("hidden");
                $(JsFormulario.Controles.ddlFrecuanciaEnvio).parent().addClass("has-error");
                validar = false;
            }
            return validar;
        },

        "ValidarFormularioIndicador": function () {
            let validar = true;

            $(JsFormulario.Controles.txtTituloHojaHelp).addClass("hidden");
            $(JsFormulario.Controles.txtNotasEncargadoFormularioHelp).addClass("hidden");
            $(JsFormulario.Controles.ddlIndicadorHelp).addClass("hidden");
            $(JsFormulario.Controles.txtTituloHoja).parent().removeClass("has-error");
            $(JsFormulario.Controles.ddlIndicador).parent().removeClass("has-error");
            $(JsFormulario.Controles.txtNotasEncargadoFormulario).parent().removeClass("has-error");

            let notas = $(JsFormulario.Controles.txtNotasEncargadoFormulario).val().trim();
            let indicador = $(JsFormulario.Controles.ddlIndicador).val();
            let hoja = $(JsFormulario.Controles.txtTituloHoja).val().trim();

            if (hoja.length == 0) {
                $(JsFormulario.Controles.txtTituloHojaHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtTituloHoja).parent().addClass("has-error");
                validar = false;
            }
            if (notas.length == 0) {
                $(JsFormulario.Controles.txtNotasEncargadoFormularioHelp).removeClass("hidden");
                $(JsFormulario.Controles.txtNotasEncargadoFormulario).parent().addClass("has-error");
                validar = false;
            }

            if (indicador == "" || indicador == "-1") {
                $(JsFormulario.Controles.ddlIndicadorHelp).removeClass("hidden");
                $(JsFormulario.Controles.ddlIndicador).parent().addClass("has-error");
                validar = false;
            }
            return validar;
        },

        "ValidarButonFinalizar": function () {
            if (JsFormulario.Variables.CantidadActual != $(JsFormulario.Controles.CantidadIndicadoresMax).val())
                $(JsFormulario.Controles.btnGuardarFormularioCompleto).prop("disabled", true); // gris
            else
                $(JsFormulario.Controles.btnGuardarFormularioCompleto).removeAttr("disabled");// azul
        },

        "ValidarButonGuardarIndicador": function () {
            if (JsFormulario.Variables.CantidadActual != $(JsFormulario.Controles.CantidadIndicadoresMax).val())
                $(JsFormulario.Controles.btnGuardarIndicador).removeAttr("disabled"); // gris
            else
                $(JsFormulario.Controles.btnGuardarIndicador).prop("disabled", true);// azul
        },

        "CargarTablasFormulario": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsFormulario.Variables.ListadoFormulario.length; i++) {
                var ind = "";
                let formulario = JsFormulario.Variables.ListadoFormulario[i];
                html = html + "<tr>"
                html = html + "<td scope='row'>" + formulario.Codigo + "</td>";
                html = html + "<td>" + formulario.Nombre + "</td>";
                if (formulario.ListaIndicadores == null) {
                    html = html + "<td>N/A</td>";
                }
                else {
                    html = html + "<td>" + formulario.ListaIndicadores + "</td>";
                }
                html = html + "<td>" + formulario.FrecuenciaEnvio.Nombre + "</td>";
                html = html + "<td>" + formulario.EstadoRegistro.Nombre + "</td>";
                html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + formulario.id + " data-original-title='Editar' title='Editar' class='btn-icon-base btn-edit'></button>";
                if (formulario.idEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.EnProceso) {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Clonar' data-original-title='Clonar' disabled class='btn-icon-base btn-clone'></button>";
                }
                else {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Clonar' data-original-title='Clonar' value=" + formulario.id + " class='btn-icon-base btn-clone' ></button>";
                }
                if (formulario.idEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + formulario.id + " class='btn-icon-base btn-power-off'></button>";
                } else if (formulario.idEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.Activo) {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + formulario.id + " class='btn-icon-base btn-power-on'></button>";
                } else {
                    html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' disabled value=" + formulario.id + " class='btn-icon-base btn-power-off'></button>";
                }
                if (formulario.idEstadoRegistro != jsUtilidades.Variables.EstadoRegistros.Activo) {
                    ind = "disabled"
                }
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Visualizar' data-original-title='Visualizar' value=" + formulario.id + " class='btn-icon-base btn-view' " + ind + "></button>";
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' data-original-title='Eliminar' value=" + formulario.id + " class='btn-icon-base btn-delete' ></button></td >";


                html = html + "</tr>"
            }
            $(JsFormulario.Controles.tablaFormulario).html(html);
            CargarDatasource();
            JsFormulario.Variables.ListadoFormulario = [];
        },

        "CargarTablasIndicadores": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsFormulario.Variables.ListadoDetalleIndicadores.length; i++) {
                let indicadores = JsFormulario.Variables.ListadoDetalleIndicadores[i];
                html = html + "<tr>"
                html = html + "<td class='dt-center'>" + indicadores.Codigo + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.Nombre + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.GrupoIndicadores.Nombre + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.TipoIndicadores.Nombre + " </td>";
                html = html + "<td class='dt-center'>" + indicadores.EstadoRegistro.Nombre + " </td>";
                html = html + "<td class='dt-center'><button type='button' data-toggle='tooltip' data-placement='top' title='Editar' value=" + indicadores.IdIndicador + " class='btn-icon-base btn-edit'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar'  value=" + indicadores.IdIndicador + " class='btn-icon-base btn-delete'></button></td>";
                html = html + "</tr>"
            }
            $(JsFormulario.Controles.TableIndicadorFormularioBody).html(html);
            CargarDatasource();
            JsFormulario.Variables.ListadoDetalleIndicadores = [];
        },

        "CrearTablaFormulario": function (formulario, indicador) {
            let html = "";
            html = html + "<tr>"
            html = html + "<td scope='row'>" + formulario.Codigo + "</td>";
            html = html + "<td>" + formulario.Nombre + "</td>";
            html = html + "<td>" + indicador + "</td>";
            html = html + "<td>" + formulario.FrecuenciaEnvio.Nombre + "</td>";
            html = html + "<td>" + formulario.EstadoRegistro.Nombre + "</td>";
            html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + formulario.id + " data-original-title='Editar' title='Editar' class='btn-icon-base btn-edit'></button>";
            html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' data-original-title='Clonar' value=" + formulario.id + " class='btn-icon-base btn-clone' ></button>";
            if (formulario.idEstadoRegistro == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value=" + formulario.id + " class='btn-icon-base btn-power-off'></button>";
            } else {
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value=" + formulario.id + " class='btn-icon-base btn-power-on'></button>";
            }
            html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Visualizar' data-original-title='Visualizar' value=" + formulario.id + " class='btn-icon-base btn-view' ></button>";
            html = html + "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Eliminar' data-original-title='Eliminar' value=" + formulario.id + " class='btn-icon-base btn-delete' ></button></td >";


            html = html + "</tr>"
            return html;
        },

        "CargarIndicadores": function (obj) {
            $(JsFormulario.Controles.txtTituloHoja).val(obj.TituloHoja);
            $(JsFormulario.Controles.txtNotasEncargadoFormulario).val(obj.NotaEncargado);
            var comboIndicador = document.getElementById("ddlIndicador");
            comboIndicador.innerHTML = '';
           
            comboIndicador.options[0] = new Option(obj.Indicador.Nombre, obj.idIndicador);
            $(JsFormulario.Controles.ddlIndicador).val(obj.idIndicador).change();
            $(JsFormulario.Controles.ddlIndicador).prop("disabled", true);
            
        },

        "ReestablecerIndicadores": function (obj) {
            $(JsFormulario.Controles.txtTituloHoja).val("");
            $(JsFormulario.Controles.txtNotasEncargadoFormulario).val("");
            $(JsFormulario.Controles.ddlIndicador).val(0).change();
        },

        "MensajeError": function (obj) {
            if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) {
                        location.reload();
                    });
            }
            else {
                jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                    .set('onok', function (closeEvent) {
                    });
            }
        },
        "CargarColumnasTabla": function (pDetallesIndicadorFonatel) {
            if ($(JsFormulario.Controles.columnaTablaIndicador).length == 0) {
                let html = "<th style='min-width:30PX'>  </th>";
                for (var i = 0; i < pDetallesIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel.length; i++) {
                    let variable = pDetallesIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel[i];
                    html = html + variable.html;
                }
                for (var i = 0; i < pDetallesIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel.length; i++) {
                    let categoria = pDetallesIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel[i];
                    html = html + "<th style='min-width:160PX' scope='col'>" + categoria.NombreCategoria + "</th>";
                }
                $(JsFormulario.Controles.columnasTablaIndicador).html(html);
            }
            else {
                EliminarDatasource(JsFormulario.Controles.tablaIndicador);
            }
        },
        "CargarFilasTabla": function (pDetallesIndicadorFonatel) {
            $(JsFormulario.Controles.filasTablaIndicador).html("");
            let html = "<tr><td></td>";
            for (var i = 0; i < pDetallesIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel.length; i++) {
                let variable = pDetallesIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel[i];
                html = html + "<td><input type='number' name='name_" + variable.idVariable + "' class='form-control form-control-fonatel solo_numeros VariableDato' id='[0]-" + variable.idVariable + "'></td>";
                //html = html + "<td>1</td>";
            }
            for (var i = 0; i < pDetallesIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel.length; i++) {
                let categoria = pDetallesIndicadorFonatel.DetalleRegistroIndicadorCategoriaFonatel[i];
                html = html + categoria.html;
            }
            html = html + "</tr>"
            for (var i = 0; i < pDetallesIndicadorFonatel.CantidadFilas; i++) {
                $(JsFormulario.Controles.filasTablaIndicador).append(html.replace("[0]", i + 1));
            }
        },
    },

    "Consultas": {

        "InsertarFormularioWeb": async function () {
            $("#loading").fadeIn();
            let formulario = new Object();
            formulario.Codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            formulario.Nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();
            formulario.Descripcion = $(JsFormulario.Controles.txtDescripcionFormulario).val().trim();
            formulario.CantidadIndicador = $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val().trim();
            formulario.idFrecuenciaEnvio = $(JsFormulario.Controles.ddlFrecuanciaEnvio).val();
            formulario.idEstadoRegistro = jsUtilidades.Variables.EstadoRegistros.EnProceso;
            //formulario.idEstadoRegistro = JsFormulario.Metodos.ValidarEstadoParcialFormulario(formulario);
            await execAjaxCall("/FormularioWeb/InsertarFormularioWeb", "POST", formulario)
                .then((obj) => {
                    let cantidadMax = obj.objetoRespuesta[0].CantidadIndicador;
                    $(JsFormulario.Controles.CantidadIndicadoresMax).val(cantidadMax);
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);
                    JsFormulario.Variables.HayError = false;
                    JsFormulario.Metodos.ValidarButonFinalizar();
                    JsFormulario.Metodos.ValidarButonGuardarIndicador();
                }).catch((obj) => {
                    JsFormulario.Variables.HayError = true;
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "GuardadoCompleto": async function () {
            let formulario = new Object();
            formulario.id = ObtenerValorParametroUrl("id");
            await execAjaxCall("/FormularioWeb/GuardadoCompleto", "POST", formulario)
                .then((obj) => {
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);
                    JsFormulario.Variables.HayError = false;
                }).catch((obj) => {
                    JsFormulario.Variables.HayError = true;
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EditarFormularioWeb": async function () {
            $("#loading").fadeIn();
            let formulario = new Object();
            formulario.Codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            formulario.Nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();
            formulario.Descripcion = $(JsFormulario.Controles.txtDescripcionFormulario).val().trim();
            formulario.CantidadIndicador = $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val().trim();
            formulario.idFrecuenciaEnvio = $(JsFormulario.Controles.ddlFrecuanciaEnvio).val();
            formulario.id = $.urlParam("id");
            await execAjaxCall("/FormularioWeb/EditarFormularioWeb", "POST", formulario)
                .then((obj) => {
                    $(JsFormulario.Controles.CantidadIndicadoresMax).val(obj.objetoRespuesta[0].CantidadIndicador)
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);
                    JsFormulario.Variables.HayError = false;
                    JsFormulario.Metodos.ValidarButonFinalizar();
                    JsFormulario.Metodos.ValidarButonGuardarIndicador();
                }).catch((obj) => {
                    JsFormulario.Variables.HayError = true;
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ClonarFormularioWeb": async function () {
            $("#loading").fadeIn();
            let formulario = new Object();
            formulario.id = ObtenerValorParametroUrl("id");
            formulario.Codigo = $(JsFormulario.Controles.txtCodigoFormulario).val().trim();
            formulario.Nombre = $(JsFormulario.Controles.txtNombreFormulario).val().trim();
            formulario.Descripcion = $(JsFormulario.Controles.txtDescripcionFormulario).val().trim();
            formulario.CantidadIndicador = $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val().trim();
            formulario.idFrecuenciaEnvio = $(JsFormulario.Controles.ddlFrecuanciaEnvio).val();
            formulario.idEstadoRegistro = jsUtilidades.Variables.EstadoRegistros.EnProceso;
            await execAjaxCall("/FormularioWeb/ClonarFormulario", "POST", formulario)
                .then((obj) => {
                    $(JsFormulario.Controles.CantidadIndicadoresMax).val(obj.objetoRespuesta[0].CantidadIndicador)
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);
                    JsFormulario.Variables.HayError = false;
                }).catch((obj) => {
                    JsFormulario.Variables.HayError = true;
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarIndicadores": async function () {
            $("#loading").fadeIn();
            let detalleFormulario = new Object();
            let formularioweb = new Object();
            detalleFormulario.TituloHoja = $(JsFormulario.Controles.txtTituloHoja).val().trim();
            detalleFormulario.NotaEncargado = $(JsFormulario.Controles.txtNotasEncargadoFormulario).val().trim();
            detalleFormulario.idIndicador = $(JsFormulario.Controles.ddlIndicador).val();
            formularioweb.id = ObtenerValorParametroUrl("id");
            detalleFormulario.formularioweb = formularioweb;
            await execAjaxCall("/FormularioWeb/InsertarIndicadoresFormulario", "POST", detalleFormulario)
                .then((obj) => {
                    let cantidadMax = obj.objetoRespuesta[0].formularioweb.CantidadIndicador;
                    $(JsFormulario.Controles.CantidadIndicadoresMax).val(cantidadMax);
                    jsMensajes.Metodos.OkAlertModal("El Indicador ha sido agregado")
                        .set('onok', function (closeEvent) {
                            JsFormulario.Consultas.ConsultaListaIndicadoresFormulario();
                            JsFormulario.Metodos.ReestablecerIndicadores();
                            JsFormulario.Consultas.ConsultaListaIndicadoresFormularioConMsj();
                        });
                }).catch((obj) => {
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EditarIndicadores": function () {
            $("#loading").fadeIn();
            let detalleFormulario = new Object();
            let formularioweb = new Object();
            detalleFormulario.TituloHoja = $(JsFormulario.Controles.txtTituloHoja).val().trim();
            detalleFormulario.NotaEncargado = $(JsFormulario.Controles.txtNotasEncargadoFormulario).val().trim();
            detalleFormulario.idIndicador = $(JsFormulario.Controles.ddlIndicador).val();
            formularioweb.id = ObtenerValorParametroUrl("id");
            detalleFormulario.formularioweb = formularioweb;
            execAjaxCall("/FormularioWeb/EditarIndicadoresFormulario", "POST", detalleFormulario)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Indicador del Formulario ha sido editado")
                        .set('onok', function (closeEvent) {
                            $(JsFormulario.Controles.ddlIndicador).prop("disabled", false);
                            JsFormulario.Consultas.ConsultaListaIndicadoresFormulario();
                            JsFormulario.Variables.NuevoIndicador = true;
                            JsFormulario.Metodos.ReestablecerIndicadores();
                            JsFormulario.Metodos.ValidarButonGuardarIndicador();
                        });
                }).catch((obj) => {
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarIndicadores": async function (idIndicador, idFormulario) {
            $("#loading").fadeIn();
            let detalleFormulario = new Object();
            let formularioweb = new Object();
            detalleFormulario.idIndicador = idIndicador;
            formularioweb.id  = idFormulario;
            detalleFormulario.formularioweb = formularioweb;
            await execAjaxCall("/FormularioWeb/EliminarIndicadoresFormulario", "POST", detalleFormulario)
                .then((obj) => {
                    JsFormulario.Consultas.ConsultaListaIndicadoresFormularioCombo();
                    jsMensajes.Metodos.OkAlertModal("El Indicador ha sido eliminado")
                        .set('onok', function (closeEvent) {
                            JsFormulario.Consultas.ConsultaListaIndicadoresFormulario();                           
                        });
                }).catch((obj) => {
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarFormulario": async function (idFormulario) {
            let objFormulario = new Object()
            objFormulario.id = idFormulario;

            await JsFormulario.Consultas.EliminarDetalleIndicadoresFormulario(objFormulario);

            await execAjaxCall("/FormularioWeb/EliminarFormulario", "POST", objFormulario)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido eliminado")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
                }).catch((obj) => {
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarDetalleIndicadoresFormulario": async function (objFormulario) {
            await execAjaxCall("/FormularioWeb/EliminarDetalleIndicadoresFormulario", "POST", objFormulario)
                .then((obj) => {
                    
                }).catch((obj) => {
                    JsFormulario.Metodos.MensajeError(obj);
                })
        },

        "DesactivarFormulario": function (idFormulario) {
            let objFormulario = new Object()
            objFormulario.id = idFormulario;
            execAjaxCall("/FormularioWeb/DesactivarFormulario", "POST", objFormulario)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido desactivado")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ActivarFormulario": function (idFormulario) {
            let objFormulario = new Object()
            objFormulario.id = idFormulario;
            execAjaxCall("/FormularioWeb/ActivarFormulario", "POST", objFormulario)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido activado")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
                }).catch((obj) => {
                    JsFormulario.Metodos.MensajeError(obj);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ValidarExistenciaFormulario": function (idFormulario, eliminado = false) {
            $("#loading").fadeIn();
            let objFormulario = new Object()
            objFormulario.id = idFormulario;
            execAjaxCall("/FormularioWeb/ValidarFormulario", "POST", objFormulario)
                .then((obj) => {
                    if (obj.objetoRespuesta.length == 0) {

                        if (eliminado) {
                            JsFormulario.Consultas.EliminarFormulario(idFormulario);

                        } else {
                            JsFormulario.Consultas.DesactivarFormulario(idFormulario);
                        }
                    } else {
                        let dependencias = obj.objetoRespuesta[0] + "<br>"
                        if (eliminado) {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Formulario Web está en uso en la  " + dependencias + "<br>¿Desea eliminarlo?", jsMensajes.Variables.actionType.eliminar)
                                .set('onok', function (closeEvent) {
                                    JsFormulario.Consultas.EliminarFormulario(idFormulario);
                                })
                        }
                        else {
                            jsMensajes.Metodos.ConfirmYesOrNoModal("El Formulario Web está en uso en la " + dependencias + "<br>¿Desea desactivarlo?", jsMensajes.Variables.actionType.estado)
                                .set('onok', function (closeEvent) {
                                    JsFormulario.Consultas.DesactivarFormulario(idFormulario);
                                })
                        }
                    }
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { location.reload(); });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaListaFormularioWeb": function () {
            $("#loading").fadeIn();
            execAjaxCall("/FormularioWeb/ObtenerFormulariosWeb", "GET")
                .then((obj) => {
                    JsFormulario.Variables.ListadoFormulario = obj.objetoRespuesta;
                    JsFormulario.Metodos.CargarTablasFormulario();
                }).catch((obj) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaListaIndicadoresFormulario": function () {
            $("#loading").fadeIn();
            let id = ObtenerValorParametroUrl("id");
            execAjaxCall("/FormularioWeb/ObtenerIndicadoresFormulario?idFormulario=" + id, "GET")
                .then((obj) => {
                    JsFormulario.Variables.ListadoDetalleIndicadores = obj.objetoRespuesta;
                    JsFormulario.Variables.CantidadActual = JsFormulario.Variables.ListadoDetalleIndicadores.length
                    JsFormulario.Metodos.CargarTablasIndicadores();
                    JsFormulario.Metodos.ValidarButonFinalizar();
                    JsFormulario.Metodos.ValidarButonGuardarIndicador();
                }).catch((obj) => {
                    jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                        .set('onok', function (closeEvent) {
                        });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaListaIndicadoresFormularioCombo": function () {
            $("#loading").fadeIn();
            execAjaxCall("/FormularioWeb/ObtenerIndicadoresFormularioCombo", "GET")
                .then((obj) => {
                    var comboIndicador = document.getElementById("ddlIndicador");
                    comboIndicador.innerHTML = '';
                    comboIndicador.options[0] = new Option("", -1);
                    for (var i = 1; i <= obj.objetoRespuesta.length; i++) {
                        comboIndicador.options[i] = new Option(obj.objetoRespuesta[i-1].Text, obj.objetoRespuesta[i-1].Value);
                    }
                    
                }).catch((obj) => {
                    jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                        .set('onok', function (closeEvent) {
                        });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaListaIndicadoresFormularioConMsj": function () {
            $("#loading").fadeIn();
            let id = ObtenerValorParametroUrl("id");
            execAjaxCall("/FormularioWeb/ObtenerIndicadoresFormulario?idFormulario=" + id, "GET")
                .then((obj) => {
                    JsFormulario.Variables.ListadoDetalleIndicadores = obj.objetoRespuesta;
                    JsFormulario.Variables.CantidadActual = JsFormulario.Variables.ListadoDetalleIndicadores.length
                    JsFormulario.Metodos.CargarTablasIndicadores();
                    JsFormulario.Metodos.ValidarButonFinalizar();
                    
                    JsFormulario.Metodos.ValidarButonGuardarIndicador();
                    
                }).catch((obj) => {
                    jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                        .set('onok', function (closeEvent) {
                        });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaDetalleFormularioWebAjax": function (idIndicador, idFormulario) {
            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/FormularioWeb/ObtenerDetalleFormularioWeb',
                type: "GET",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                data: { idIndicador, idFormulario },
                success: function (obj) {
                    $("#loading").fadeOut();
                    JsFormulario.Metodos.CargarIndicadores(obj);
                }
            }).fail(function (obj) {
                $("#loading").fadeOut();
            })
        },

        "ConsultaVizualizarFormulario": function () {
            $("#loading").fadeIn();

            let detalleIndicadorFonatel = {
                IdIndicador: $(JsFormulario.Controles.tabIndicadorActive).attr('data-Indicador'),
                CantidadFila: $(JsFormulario.Controles.tabActivoIndicador).find(JsFormulario.Controles.txtCantidadIndicador).val()
            }

            execAjaxCall("/FormularioWeb/ConsultaVizualizarFormulario", "POST", detalleIndicadorFonatel)
                .then((obj) => {
                    $(JsFormulario.Controles.tablaIndicador).removeClass("hidden");

                    let detallesIndicadorFonatel = obj.objetoRespuesta[0];

                    if (detallesIndicadorFonatel.DetalleRegistroIndicadorVariableFonatel.length > 0) {
                        JsFormulario.Metodos.CargarColumnasTabla(detallesIndicadorFonatel);
                        JsFormulario.Metodos.CargarFilasTabla(detallesIndicadorFonatel);
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
                    CargarDatasourceV2(JsFormulario.Controles.tablaIndicador);
                    $(JsFormulario.Controles.tablaIndicador_filter).addClass("hidden");
                    $("#loading").fadeOut();
                });
        },
    }

}

$(document).on("keyup", JsFormulario.Controles.ControlesStep1, function (e) {
    JsFormulario.Metodos.ValidarFormularioWebCrear();
});

$(document).on("change", JsFormulario.Controles.ControlesStep1, function (e) {
    JsFormulario.Metodos.ValidarFormularioWebCrear();
});

// CANCELAR
$(document).on("click", JsFormulario.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/FormularioWeb/Index";
        });
});

// GUARDAR INDICADOR
$(document).on("click", JsFormulario.Controles.btnGuardarIndicador, function (e) {
    e.preventDefault();
    if (JsFormulario.Metodos.ValidarFormularioIndicador()) {
        if (JsFormulario.Variables.NuevoIndicador === true) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(JsFormulario.Mensajes.preguntaGuardarIndicador, jsMensajes.Variables.actionType.agregar)
                .set('onok', async function (closeEvent) {
                    await JsFormulario.Consultas.InsertarIndicadores();
                    JsFormulario.Consultas.ConsultaListaIndicadoresFormularioCombo();
                });
        }
        else {
            jsMensajes.Metodos.ConfirmYesOrNoModal(JsFormulario.Mensajes.preguntaEditarIndicador, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsFormulario.Consultas.EditarIndicadores();
                    JsFormulario.Consultas.ConsultaListaIndicadoresFormularioCombo();
                });
        }
    }
});

// EDITAR INDICADORES
$(document).on("click", JsFormulario.Controles.btnEditarIndicadores, function () {
    let idIndicador = $(this).val();
    let idFormulario =ObtenerValorParametroUrl('id');
    JsFormulario.Variables.NuevoIndicador = false;
    $(JsFormulario.Controles.btnGuardarIndicador).removeAttr("disabled");
    JsFormulario.Consultas.ConsultaDetalleFormularioWebAjax(idIndicador, idFormulario);
});

// EDITAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnEditarFormulario, function () {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    window.location.href = "/Fonatel/FormularioWeb/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

// CLONAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnCloneFormulario, function () {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    
            window.location.href = "/Fonatel/FormularioWeb/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar;
      
});
//CAMBIO DE FRECUENCIA
$(document).on("change", JsFormulario.Controles.ddlFrecuanciaEnvio, function (e) {
    let id = $(this).val();
   // alert(id);
    $.ajax({
        url: jsUtilidades.Variables.urlOrigen + '/FormularioWeb/ObtenerIndicadoresxFrecuencia',
        type: "GET",
        dataType: "JSON",
        beforeSend: function () {
            //$("#loading").fadeIn();
        },
        data: { id },
        success: function (obj) {
            $("#loading").fadeOut();
            //JsFormulario.Metodos.CargarIndicadores(obj);
            var comboIndicador = document.getElementById("ddlIndicador");
            comboIndicador.innerHTML = '';
            comboIndicador.options[0] = new Option("", -1);
            for (var i = 1; i <= obj.objetoRespuesta.length; i++) {
                comboIndicador.options[i] = new Option(obj.objetoRespuesta[i - 1].Text, obj.objetoRespuesta[i - 1].Value);
            }
        }
    }).fail(function (obj) {
        $("#loading").fadeOut();
    })
});
// GUARDAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnGuardar, function (e) {
    e.preventDefault();
    if (JsFormulario.Metodos.ValidarDatosMinimos()) {

        let modo = ObtenerValorParametroUrl('modo');

        if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar el Formulario Web? ", jsMensajes.Variables.actionType.agregar)
                .set('onok', async function (closeEvent) {
                    await JsFormulario.Consultas.ClonarFormularioWeb();
                    if (JsFormulario.Variables.HayError === false) {
                        jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido creado")
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
                    }
                })
                .set("oncancel", function () {
                    JsFormulario.Metodos.ValidarFormularioWebTotal();
                });

        } else if (modo == jsUtilidades.Variables.Acciones.Editar) {

            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar el Formulario Web?", jsMensajes.Variables.actionType.agregar)
                .set('onok', async function (closeEvent) {

                    if (modo == jsUtilidades.Variables.Acciones.Editar) {
                        await JsFormulario.Consultas.EditarFormularioWeb();
                        modoMsj = "editado";
                    }
                    if (JsFormulario.Variables.HayError === false) {
                        jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido " + modoMsj)
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
                    }
                })
                .set("oncancel", function () {
                    JsFormulario.Metodos.ValidarFormularioWebTotal();
                });
        }

        else {

            jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial del Formulario Web?", jsMensajes.Variables.actionType.agregar)
                .set('onok', async function (closeEvent) {

                    let modoMsj = "";

                    if (modo == undefined) { // Crear
                        await JsFormulario.Consultas.InsertarFormularioWeb();
                        modoMsj = "creado";
                    }
                    if (JsFormulario.Variables.HayError === false) {
                        jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido " + modoMsj)
                            .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
                    }
                })
                .set("oncancel", function () {
                    JsFormulario.Metodos.ValidarFormularioWebTotal();
                });
        }
    }
});

// VISUALIZAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnVizualizarFormulario, function (e) {
    let id = $(this).val();
    window.location.href = "/Fonatel/FormularioWeb/Visualizar?id=" + id + "&modo=" + 6;
});

// SIGUIENTE FORMULARIO
$(document).on("click", JsFormulario.Controles.btnSiguienteFormulario, async function (e) {
    e.preventDefault();
    let modo =ObtenerValorParametroUrl('modo');
    if (JsFormulario.Metodos.ValidarFormularioWebTotal()) {
        if (modo == undefined) { // Crear
            await JsFormulario.Consultas.InsertarFormularioWeb();
        }
        if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            await JsFormulario.Consultas.ClonarFormularioWeb();
            JsFormulario.Consultas.ConsultaListaIndicadoresFormulario();
        }
        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            await JsFormulario.Consultas.EditarFormularioWeb();
        }
        if (JsFormulario.Variables.HayError === false) {
            $("a[href='#step-2']").trigger('click');
        }
    }
});

// DELETE(BORRAR) FORMULARIO
$(document).on("click", JsFormulario.Controles.btnDeleteFormulario, function (e) {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Formulario Web?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsFormulario.Consultas.ValidarExistenciaFormulario(id, true);
        });
});

// DELETE(BORRAR) INDICADOR
$(document).on("click", JsFormulario.Controles.btnDeleteIndicador, function (e) {
    let idIndicador = $(this).val();
    let idFormulario = ObtenerValorParametroUrl("id");
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Indicador?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', async function (closeEvent) {
            await JsFormulario.Consultas.EliminarIndicadores(idIndicador, idFormulario);
            $(JsFormulario.Controles.ddlIndicador).prop("disabled", false);
            $(JsFormulario.Controles.txtNotasEncargadoFormulario).val("");
            $(JsFormulario.Controles.txtTituloHoja).val("");
            JsFormulario.Consultas.ConsultaListaIndicadoresFormularioCombo();           
            JsFormulario.Variables.NuevoIndicador = true;
        });
});

// ACTIVAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnDesactivadoFormulario, function (e) {
    if (consultasFonatel) { return; }
    e.preventDefault();
    let id = $(this).val();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar el Formulario Web?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsFormulario.Consultas.ActivarFormulario(id);
        });
});

// DESACTIVAR FORMULARIO
$(document).on("click", JsFormulario.Controles.btnActivadoFormulario, function (e) {
    if (consultasFonatel) { return; }
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar el Formulario Web?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            JsFormulario.Consultas.DesactivarFormulario(id);
        });
});

// ATRAS
$(document).on("click", JsFormulario.Controles.btnAtrasFormularioRegla, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');

    $(JsFormulario.Controles.txtCodigoFormulario).prop("disabled", true);
    InsertarParametroUrl("modo", jsUtilidades.Variables.Acciones.Editar); 
});

//Atras visualizar
$(document).on("click", JsFormulario.Controles.btnAtrasFormularioVisualizar, function (e) {
    e.preventDefault();
    window.location.href = "/Fonatel/FormularioWeb/index";
});

// GUARDAR FORMULARIO COMPLETO
$(document).on("click", JsFormulario.Controles.btnGuardarFormularioCompleto, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea guardar el Formulario Web?", jsMensajes.Variables.actionType.agregar)
        .set('onok', async function (closeEvent) {
            await JsFormulario.Consultas.GuardadoCompleto();
            if (JsFormulario.Variables.HayError === false) {
                jsMensajes.Metodos.OkAlertModal("El Formulario Web ha sido creado")
                    .set('onok', function (closeEvent) { window.location.href = "/Fonatel/FormularioWeb/index" });
            }
        });
});

$(function () {
    $(JsFormulario.Controles.txtCodigoFormularioHelp).addClass("hidden");
    $(JsFormulario.Controles.txtNombreFormularioHelp).addClass("hidden");
    $(JsFormulario.Controles.ddlFrecuenciaHelp).addClass("hidden");
    $(JsFormulario.Controles.txtCantidadIndicadoresFormularioHelp).addClass("hidden");
    $(JsFormulario.Controles.txtDescripcionFormularioHelp).addClass("hidden");
    $(JsFormulario.Controles.btnGuardarFormularioCompleto).prop("disabled", true);
    JsFormulario.Metodos.ValidarButonFinalizar();
    JsFormulario.Metodos.ValidarButonGuardarIndicador();

    if ($(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val() == 0) {
        $(JsFormulario.Controles.txtCantidadIndicadoresFormulario).val("");
    }

    if ($(JsFormulario.Controles.FormFormulario).length > 0) {
        JsFormulario.Metodos.ValidarFormularioWebCrear();
        let modo =ObtenerValorParametroUrl('modo');
        if (modo == jsUtilidades.Variables.Acciones.Editar) {
            $(JsFormulario.Controles.txtCodigoFormulario).prop("disabled", true);
            JsFormulario.Consultas.ConsultaListaIndicadoresFormulario();
        }
        if (modo == undefined) {
            JsFormulario.Metodos.ValidarButonFinalizar();
            JsFormulario.Metodos.ValidarButonGuardarIndicador();
        }
    }

    if ($(JsFormulario.Controles.FormularioIndex).length > 0) {
        JsFormulario.Consultas.ConsultaListaFormularioWeb();
    }
});

$(document).on("keypress", JsFormulario.Controles.txtCantidadIndicador, function () {
    if (event.keyCode == 13) {     
        var tabla = $(JsFormulario.Controles.tablaIndicador);
        tabla.addClass("revisado");
        JsFormulario.Consultas.ConsultaVizualizarFormulario();
    }
});

function setSelect2() {
    $('.listasDesplegables').select2({
        placeholder: "Seleccione",
        width: 'resolve'
    });
}

$(document).on('draw.dt', JsFormulario.Controles.tablaIndicador, function (e) {
    setSelect2();
});