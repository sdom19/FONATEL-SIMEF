IndexView = {
    Controles: {
        tablaFormulas: "#tablaFormulasDetalle tbody",

        btnEliminarFormula: "#tablaFormulasDetalle tbody tr td .btn-delete",
        btnDesactivarFormula: "#tablaFormulasDetalle tbody tr td .btn-power-off",
        btnActivarFormula: "#tablaFormulasDetalle tbody tr td .btn-power-on",
        btnEditFormula: "#tablaFormulasDetalle tbody tr td .btn-edit",
        btnVerFormula: "#tablaFormulasDetalle tbody tr td .btn-view",
        btnCloneFormula: "#tablaFormulasDetalle tbody tr td .btn-clone",

        IndexView: "#ansy7o9dc"
    },

    Variables: {
        indexViewURL: "/FormulaCalculo/Index",
        createViewURL: "/Fonatel/FormulaCalculo/Create?id=",
        editViewURL: "/Fonatel/FormulaCalculo/Edit?id=",
        cloneViewURL: "/Fonatel/FormulaCalculo/Clone?id=",
        visualizeViewURL: "/Fonatel/FormulaCalculo/Visualize?id=",
    },

    Mensajes: {
        preguntaEliminarFormula: "¿Desea eliminar la Fórmula?",
        exitoEliminarFormula: "La Fórmula ha sido eliminada",

        preguntaActivarFormula: "¿Desea activar la Fórmula?",
        exitoActivarFormula: "La Fórmula ha sido activada",

        preguntaDesactivarFormula: "¿Desea desactivar la Fórmula?",
        exitoDesactivarFormula: "La Fórmula ha sido desactivada"
    },

    Metodos: {
        EliminarFormulaCalculo: function (idFormula) {
            $("#loading").fadeIn();

            IndexView.Consultas.EliminarFormula({ id: idFormula })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoEliminarFormula)
                        .set('onok', function (closeEvent) {
                            window.location.href = IndexView.Variables.indexViewURL;
                        });
                })
                .catch((obj) => {  ManejoDeExcepciones(null); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        ActivarFormulaCalculo: function (idFormula) {
            $("#loading").fadeIn();

            IndexView.Consultas.ActivarFormula({ id: idFormula })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoActivarFormula)
                        .set('onok', function (closeEvent) {
                            window.location.href = IndexView.Variables.indexViewURL;
                        });
                })
                .catch((obj) => { ManejoDeExcepciones(null); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        DesactivarFormulaCalculo: function (idFormula) {
            $("#loading").fadeIn();

            IndexView.Consultas.DesactivarFormula({ id: idFormula })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoDesactivarFormula)
                        .set('onok', function (closeEvent) {
                            window.location.href = IndexView.Variables.indexViewURL;
                        });
                })
                .catch((obj) => { ManejoDeExcepciones(null); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CargarTablaFormulas: function () {
            $("#loading").fadeIn();

            IndexView.Consultas.ObtenerListaFormulas()
                .then(obj => {
                    this.InsertarDatosTablaFormulas(obj.objetoRespuesta);
                })
                .catch(error => { ManejoDeExcepciones(null); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        InsertarDatosTablaFormulas: function (pListado) {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < pListado.length; i++) {
                let formula = pListado[i];
                html += "<tr><td scope='row'>" + formula.Codigo + "</td>";
                html += "<td>" + formula.Nombre + "</td>";
                html += "<td>" + formula.Descripcion + "</td>";
                html += "<td>" + formula.EstadoRegistro.Nombre + "</td>";
                html += "<td>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Editar' value='" + formula.id + "' class='btn-icon-base btn-edit'></button>" +
                    "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' value='" + formula.id + "' class='btn-icon-base btn-clone' ></button >" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Visualizar detalle' value='" + formula.id + "' class='btn-icon-base btn-view'></button>";

                if (formula.IdEstado == jsUtilidades.Variables.EstadoRegistros.Desactivado) {
                    html += "<button type='button' data-toggle='tooltip' data-placement='top' title='Activar' data-original-title='Activar' value='" + formula.id + "' class='btn-icon-base btn-power-off'></button>";
                } else {
                    html += "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' data-original-title='Desactivar' value='" + formula.id + "' class='btn-icon-base btn-power-on'></button>";
                }

                html += "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' value='" + formula.id + "'  class='btn-icon-base btn-delete'></button>";

                html += "</td></tr>";
            }
            $(IndexView.Controles.tablaFormulas).html(html);
            CargarDatasource();
        },
    },

    Consultas: {
        ObtenerListaFormulas: function () {
            return execAjaxCall("/FormulaCalculo/ObtenerListaFormulas", "GET");
        },

        DesactivarFormula: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/DesactivarFormula", "POST", { pFormulaCalculo: pFormulaCalculo });
        },

        ActivarFormula: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/ActivarFormula", "POST", { pFormulaCalculo: pFormulaCalculo });
        },

        EliminarFormula: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/EliminarFormula", "POST", { pFormulaCalculo: pFormulaCalculo });
        }
    },

    Eventos: function () {
        $(document).on("click", IndexView.Controles.btnEliminarFormula, function (e) {
            let idFormula = $(this).val();
            jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaEliminarFormula, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    IndexView.Metodos.EliminarFormulaCalculo(idFormula);
                });
        });

        $(document).on("click", IndexView.Controles.btnDesactivarFormula, function (e) {
            let idFormula = $(this).val();
            jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaActivarFormula, jsMensajes.Variables.actionType.estado)
                .set('onok', function (closeEvent) {
                    IndexView.Metodos.ActivarFormulaCalculo(idFormula);
                });
        });

        $(document).on("click", IndexView.Controles.btnActivarFormula, function (e) {
            let idFormula = $(this).val();
            jsMensajes.Metodos.ConfirmYesOrNoModal(IndexView.Mensajes.preguntaDesactivarFormula, jsMensajes.Variables.actionType.estado)
                .set('onok', function (closeEvent) {
                    IndexView.Metodos.DesactivarFormulaCalculo(idFormula);
                });
        });

        $(document).on("click", IndexView.Controles.btnCloneFormula, function () {
            window.location.href = IndexView.Variables.cloneViewURL + $(this).val();
        });

        $(document).on("click", IndexView.Controles.btnEditFormula, function () {
            window.location.href = IndexView.Variables.editViewURL + $(this).val();
        });

        $(document).on("click", IndexView.Controles.btnVerFormula, function () {
            window.location.href = IndexView.Variables.visualizeViewURL + $(this).val();
        });
    },

    Init: function () {
        IndexView.Eventos();
        IndexView.Metodos.CargarTablaFormulas();
    }
}

// Paso 1
CrearFormulaView = {
    Controles: {
        modoFormulario: "#modoFormulario",
        step2: "a[href='#step-2']",
        CreateView: "#svh678an",
        prefijoLabelsHelp: "Help",

        // Formulario Crear fórmula de cálculo - Paso 1
        formCrearFormula: {
            form: "#formCrearFormula",
            inputs: "#formCrearFormula input, #formCrearFormula textarea, #formCrearFormula select",
            selects2: "#formCrearFormula select",
            inputDates: "#formCrearFormula input[type='date']",
            inputRadios: "#formCrearFormula input[type='radio']",

            txtCodigoFormula: "#txtCodigoFormula",
            txtNombreFormula: "#txtNombreFormula",
            txtFechaCalculo: "#txtFechaCalculo",
            txtDescripcionFormula: "#txtDescripcionFormula",
            ddlFrecuenciaFormulario: "#ddlFrecuenciaFormulario",
            ddlIndicadorFormulario: "#ddlIndicadorFormulario",
            ddlVariableDatoFormula: "#ddlVariableDatoFormula",
            ddlCategoriaDesagregacion: "#ddlCategoriaDesagregacion",
            radioTotal: "#radioTotal",
            radioCategoriaDesagregacion: "#radioCategoriaDesagregacion",
            radioNivelCalculoHelp: "#radioNivelCalculoHelp",

            divInputCategoriaDesagregacion: "#divInputCategoriaDesagregacion",

            btnGuardar: "#btnGuardarFormulaCalculo",
            btnSiguienteCrear: "#btnSiguienteFormulaCalculo",

            btnEditar: "#btnGuardarEditarFormulaCalculo",
            btnSiguienteEditar: "#btnSiguienteEditarFormulaCalculo",

            btnClonar: "#btnGuardarClonarFormulaCalculo",
            btnSiguienteClonar: "#btnSiguienteClonarFormulaCalculo",

            btnSiguienteVisualizar: "#btnSiguienteVisualizarFormulaCalculo",

            btnCancelar: "#btnCancelarFormula",
        },
    },

    Variables: {
        indexViewURL: "/Fonatel/FormulaCalculo/Index",
        laFormulaFueClonada: false
    },

    Mensajes: {
        preguntaCancelarAccion: "¿Desea cancelar la acción?",
        preguntaGuardadoParcial: "¿Desea realizar un guardado parcial de la Fórmula de Cálculo?",

        existenCamposRequeridos: "Existen campos vacíos. ",

        labelDetalleDesagregacion: "Detalle Desagregación",
        labelDetalleAgrupacion: "Detalle Agrupación",
        exitoDetalleAgregado: "El Detalle ha sido agregado",
        exitoEliminarDetalle: "El Detalle ha sido eliminado",
        preguntaEliminarDetalle: "¿Desea eliminar el Detalle?",

        exitoArgumentoEliminado: "El Argumento ha sido eliminado",
        exitoArgumentoAgregado: "El Argumento ha sido agregado",
        preguntaAgregarArgumento: "¿Desea agregar el Argumento?",
        preguntaEliminaArgumento: "¿Desea eliminar El Argumento?",

        exitoArgumentoFechaCreado: "El Argumento de Fecha ha sido creado",
        exitoEliminarArgumentoFecha: "El Argumento de Fecha ha sido eliminado",
        preguntaEliminarArgumentoFecha: "¿Desea eliminar el Argumento de Fecha?",

        exitoFormulaCreada: "La Fórmula ha sido creada",
        exitoFormulaAgregada: "La Fórmula ha sido agregada",
        exitoFormulaEjecutada: "La Fórmula ha sido ejecutada",
        preguntaAgregarFormula: "¿Desea agregar la Fórmula de Cálculo?",
        preguntaEjecutarFormula: "¿Desea ejecutar la Fórmula?",

    },

    Metodos: {
        CargarDatosDependientesDeIndicador: function (pIdIndicador, pCallback = null) { // variables dato y categorias de desagregación
            $("#loading").fadeIn();

            CrearFormulaView.Consultas.ConsultarVariablesDatoDeIndicador(pIdIndicador)
                .then(data => {
                    this.InsertarDatosEnComboVariablesDato(data);
                    return true;
                })
                .then(data => {
                    if ($(CrearFormulaView.Controles.formCrearFormula.radioCategoriaDesagregacion).is(':checked')) {
                        return CrearFormulaView.Consultas.ConsultarCategoriasDesagregacionDeIndicador(pIdIndicador)
                    }
                    return false;
                })
                .then(data => {
                    if (data) {
                        this.InsertarDatosEnComboBoxCategorias(data);
                    }
                    return pCallback != null;
                })
                .then(data => {
                    if (data) {
                        pCallback();
                    }
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CargarCategoriasDesagregacionDeIndicador: function (pIdIndicador) {
            $("#loading").fadeIn();

            CrearFormulaView.Consultas.ConsultarCategoriasDesagregacionDeIndicador(pIdIndicador)
                .then(data => { this.InsertarDatosEnComboBoxCategorias(data); })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CargarCategoriasDeFormulaNivelCalculo: function () {
            if ($(CrearFormulaView.Controles.formCrearFormula.radioCategoriaDesagregacion).is(':checked')) {
                let idParemeter = ObtenerValorParametroUrl("id");
                let indicador = $(CrearFormulaView.Controles.formCrearFormula.ddlIndicadorFormulario).val();

                if (idParemeter != null && idParemeter != "" && indicador != null && indicador != "") {
                    let formula = idParemeter;

                    $("#loading").fadeIn();

                    CrearFormulaView.Consultas.ConsultarCategoriasDesagregacionDeFormulaNivelCalculo(formula, indicador)
                        .then(data => {
                            SeleccionarItemsSelect2Multiple(
                                CrearFormulaView.Controles.formCrearFormula.ddlCategoriaDesagregacion,
                                data.objetoRespuesta,
                                "id",
                                true
                            );
                        })
                        .catch(error => { ManejoDeExcepciones(error); })
                        .finally(() => {
                            $("#loading").fadeOut();
                        });
                }
            }
        },

        InsertarDatosEnComboVariablesDato: function (pData) {
            $(CrearFormulaView.Controles.formCrearFormula.ddlVariableDatoFormula).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreVariable });
            });
            InsertarDataSetSelect2(CrearFormulaView.Controles.formCrearFormula.ddlVariableDatoFormula, dataSet);
            $(CrearFormulaView.Controles.formCrearFormula.ddlVariableDatoFormula).val("");
        },

        InsertarDatosEnComboBoxCategorias: function (pData) {
            $(CrearFormulaView.Controles.formCrearFormula.ddlCategoriaDesagregacion).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreCategoria });
            });

            if (dataSet.length > 0) {
                InsertarOpcionTodosSelect2Multiple(CrearFormulaView.Controles.formCrearFormula.ddlCategoriaDesagregacion);
            }

            InsertarDataSetSelect2(CrearFormulaView.Controles.formCrearFormula.ddlCategoriaDesagregacion, dataSet);
        },

        CrearObjFormularioCrearFormula: function (pEsGuardadoParcial) {
            let controles = CrearFormulaView.Controles.formCrearFormula;

            let listaInputCategoriasDesagracion = $(controles.ddlCategoriaDesagregacion).val();
            let listaCategorias = [];

            for (var i = 0; i < listaInputCategoriasDesagracion?.length; i++) {
                if (listaInputCategoriasDesagracion[i] !== "all")
                    listaCategorias.push({ IdCategoriaString: listaInputCategoriasDesagracion[i] })
            }

            var formData = {
                id: ObtenerValorParametroUrl("id"),
                IdIndicadorSalidaString: $(controles.ddlIndicadorFormulario).val(),
                IdVariableDatoString: $(controles.ddlVariableDatoFormula).val(),
                IdFrecuenciaString: $(controles.ddlFrecuenciaFormulario).val(),
                Codigo: $(controles.txtCodigoFormula).val(),
                Nombre: $(controles.txtNombreFormula).val(),
                Descripcion: $(controles.txtDescripcionFormula).val(),
                NivelCalculoTotal: $(controles.radioTotal).is(':checked'),
                FechaCalculo: $(controles.txtFechaCalculo).val(),
                ListaCategoriasNivelesCalculo: listaCategorias,
                EsGuardadoParcial: pEsGuardadoParcial ? true : false,
            };
            return formData;
        },

        InputExcepcionesFormularioCrearFormula: function () { // inputs a ignorar cuando se valida el formulario
            let excepcionesForm = ["select2-search__field"]; // el campo de busqueda de los select2 multiple los considera como un campo

            if ($(CrearFormulaView.Controles.formCrearFormula.radioTotal).is(':checked')) {
                excepcionesForm.push(CrearFormulaView.Controles.formCrearFormula.ddlCategoriaDesagregacion.slice(1));
            }
            return excepcionesForm;
        },

        VerificarCamposIncompletosFormularioCrearFormula: function (pEsGuardadoParcial) {
            let prefijoHelp = CrearFormulaView.Controles.prefijoLabelsHelp;
            let camposObligatoriosGuardadoParcial = true;
            let controles = CrearFormulaView.Controles.formCrearFormula;

            for (let input of $(controles.inputs)) { // limpiar mensajes de error
                $(input).parent().removeClass("has-error");
                $("#" + $(input).attr("id") + prefijoHelp).css("display", "none");
            }

            let validacionFormulario = ValidarFormulario(controles.inputs, this.InputExcepcionesFormularioCrearFormula()); // validar inputs del formulario

            if (validacionFormulario.objetos.some(x => $(x).attr("id") === $(controles.txtCodigoFormula).attr("id"))) { // campo obligatorio
                $(controles.txtCodigoFormula + prefijoHelp).css("display", "block");
                $(controles.txtCodigoFormula).parent().addClass("has-error");
                camposObligatoriosGuardadoParcial = false;
            }

            if (validacionFormulario.objetos.some(x => $(x).attr("id") === $(controles.txtNombreFormula).attr("id"))) { // campo obligatorio
                $(controles.txtNombreFormula + prefijoHelp).css("display", "block");
                $(controles.txtNombreFormula).parent().addClass("has-error");
                camposObligatoriosGuardadoParcial = false;
            }

            if (!pEsGuardadoParcial) {
                for (let input of validacionFormulario.objetos) {
                    $("#" + $(input).attr("id") + prefijoHelp).css("display", "block");
                    $("#" + $(input).attr("id")).parent().addClass("has-error");
                }
            }
            return { guardadoParcial: camposObligatoriosGuardadoParcial, guardadoCompleto: validacionFormulario.puedeContinuar };
        },

        CambiarEstadoBtnSiguienteFormCrearFormula: function (pDesactivar) {
            $(CrearFormulaView.Controles.formCrearFormula.btnSiguienteCrear).prop('disabled', pDesactivar);
            $(CrearFormulaView.Controles.formCrearFormula.btnSiguienteEditar).prop('disabled', pDesactivar);
            $(CrearFormulaView.Controles.formCrearFormula.btnSiguienteClonar).prop('disabled', pDesactivar);
        },

        EventosEnInputsFormularioCrearFormulaCalculo: function () {
            let validacion = ValidarFormulario(
                CrearFormulaView.Controles.formCrearFormula.inputs,
                CrearFormulaView.Metodos.InputExcepcionesFormularioCrearFormula()
            );
            CrearFormulaView.Metodos.CambiarEstadoBtnSiguienteFormCrearFormula(!validacion.puedeContinuar);
        },

        CrearFormulaCalculo: function () {
            let validacionFormulario = ValidarFormulario(
                CrearFormulaView.Controles.formCrearFormula.inputs,
                this.InputExcepcionesFormularioCrearFormula()
            );

            if (validacionFormulario.puedeContinuar) {
                $("#loading").fadeIn();
                CrearFormulaView.Consultas.CrearFormulaCalculo(this.CrearObjFormularioCrearFormula(false))
                    .then(data => {
                        InsertarParametroUrl("id", data.objetoRespuesta[0].id);
                        $(CrearFormulaView.Controles.step2).trigger('click'); // cargar los respectivos datos
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
        },

        CrearFormulaGuardadoParcial: function () {
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioCrearFormula(true);

            if (!validacion.guardadoParcial) { return; }

            if (!validacion.guardadoCompleto) {
                mensaje = CrearFormulaView.Mensajes.existenCamposRequeridos;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje + CrearFormulaView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    })
                    .set("oncancel", function () {
                        CrearFormulaView.Metodos.VerificarCamposIncompletosFormularioCrearFormula(false);
                    })
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CrearFormulaView.Consultas.CrearFormulaCalculo(this.CrearObjFormularioCrearFormula(true));
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal(CrearFormulaView.Mensajes.exitoFormulaCreada)
                        .set('onok', function (closeEvent) { window.location.href = CrearFormulaView.Variables.indexViewURL; });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        EditarFormulaCalculo: function () {
            let validacionFormulario = ValidarFormulario(
                CrearFormulaView.Controles.formCrearFormula.inputs,
                this.InputExcepcionesFormularioCrearFormula()
            );

            if (validacionFormulario.puedeContinuar) {
                $("#loading").fadeIn();
                CrearFormulaView.Consultas.EditarFormulaCalculo(this.CrearObjFormularioCrearFormula(false))
                    .then(data => {
                        setTimeout(() => {
                            $(CrearFormulaView.Controles.step2).trigger('click'); // cargar los respectivos datos
                        }, 600);
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
        },

        EditarFormulaCalculoGuardadoParcial: function () {
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioCrearFormula(true);

            if (!validacion.guardadoParcial) { return; }

            if (!validacion.guardadoCompleto) {
                mensaje = CrearFormulaView.Mensajes.existenCamposRequeridos;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje + CrearFormulaView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) { resolve(true); })
                    .set("oncancel", function () {
                        CrearFormulaView.Metodos.VerificarCamposIncompletosFormularioCrearFormula(false);
                    })
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CrearFormulaView.Consultas.EditarFormulaCalculo(this.CrearObjFormularioCrearFormula(true));
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal(CrearFormulaView.Mensajes.exitoFormulaCreada)
                        .set('onok', function (closeEvent) { window.location.href = CrearFormulaView.Variables.indexViewURL; });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        ClonarFormulaCalculo: function () {
            let validacionFormulario = ValidarFormulario(
                CrearFormulaView.Controles.formCrearFormula.inputs,
                this.InputExcepcionesFormularioCrearFormula()
            );

            if (validacionFormulario.puedeContinuar) {
                $("#loading").fadeIn();
                CrearFormulaView.Consultas.ClonarFormulaCalculo(this.CrearObjFormularioCrearFormula(false))
                    .then(data => {
                        CrearFormulaView.Variables.laFormulaFueClonada = true;
                        InsertarParametroUrl("id", data.objetoRespuesta.id); // actualizar el id del URL (previamente se tiene el id del indicador para clonar)

                        $(CrearFormulaView.Controles.step2Variable).trigger('click'); // cargar los respectivos datos
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => {
                        $("#loading").fadeOut();
                    });
            }
        },

        ClonarFormulaCalculoGuardadoParcial: function () {
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioCrearFormula(true);

            if (!validacion.guardadoParcial) { return; }

            if (!validacion.guardadoCompleto) {
                mensaje = CrearFormulaView.Mensajes.existenCamposRequeridos;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje + CrearFormulaView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) { resolve(true); })
                    .set("oncancel", function () {
                        CrearFormulaView.Metodos.VerificarCamposIncompletosFormularioCrearFormula(false);
                    })
            })
                .then(data => {
                    $("#loading").fadeIn();
                    return CrearFormulaView.Consultas.ClonarFormulaCalculo(this.CrearObjFormularioCrearFormula(true));
                })
                .then(data => {
                    jsMensajes.Metodos.OkAlertModal(CrearFormulaView.Mensajes.exitoFormulaCreada)
                        .set('onok', function (closeEvent) { window.location.href = CrearFormulaView.Variables.indexViewURL; });
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        }
    },

    Consultas: {
        ConsultarVariablesDatoDeIndicador: function (pIdIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerVariablesDatoDeIndicador", "GET", { pIdIndicador });
        },

        ConsultarCategoriasDesagregacionDeIndicador: function (pIdIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerCategoriasDesagregacionDeIndicador", "GET", { pIdIndicador });
        },

        ConsultarCategoriasDesagregacionDeFormulaNivelCalculo: function (pIdFormula, pIdIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerCategoriasDeFormulaNivelCalculo", "GET", { pIdFormula, pIdIndicador });
        },

        CrearFormulaCalculo: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/CrearFormulaCalculo", "POST", { pFormulaCalculo });
        },

        EditarFormulaCalculo: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/EditarFormulaCalculo", "POST", { pFormulaCalculo });
        },

        ClonarFormulaCalculo: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/ClonarFormulaCalculo", "POST", { pFormulaCalculo });
        }
    },

    Eventos: function () {
        // Formulario crear fórmula
        // -- Crear fórmula de cálculo
        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnGuardar, function (e) {
            CrearFormulaView.Metodos.CrearFormulaGuardadoParcial();
        });

        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CrearFormulaView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    window.location.href = CrearFormulaView.Variables.indexViewURL;
                });
        });

        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnSiguienteCrear, function (e) {
            if (ObtenerValorParametroUrl("id") == null) {
                CrearFormulaView.Metodos.CrearFormulaCalculo();
            }
            else {
                CrearFormulaView.Metodos.EditarFormulaCalculo();
            }
        });

        // -- Editar fórmula de cálculo
        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnEditar, function (e) {
            CrearFormulaView.Metodos.EditarFormulaCalculoGuardadoParcial();
        });

        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnSiguienteEditar, function (e) {
            if (ObtenerValorParametroUrl("id") != null) {
                CrearFormulaView.Metodos.EditarFormulaCalculo();
            }
        });

        // -- Clonar fórmula de cálculo
        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnClonar, function (e) {
            CrearFormulaView.Metodos.ClonarFormulaCalculoGuardadoParcial();
        });

        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnSiguienteClonar, function (e) {
            if (ObtenerValorParametroUrl("id") != null) {
                if (CrearFormulaView.Variables.laFormulaFueClonada) {
                    CrearFormulaView.Metodos.EditarFormulaCalculo();
                }
                else {
                    CrearFormulaView.Metodos.ClonarFormulaCalculo();
                }
            }
        });

        // -- Visualizar fórmula de cálculo
        $(document).on("click", CrearFormulaView.Controles.formCrearFormula.btnSiguienteVisualizar, function (e) {
            $(CrearFormulaView.Controles.step2).trigger('click');
        });

        $(CrearFormulaView.Controles.formCrearFormula.ddlIndicadorFormulario).on('select2:select', function (event) {
            let idIndicador = $(this).val();
            if (idIndicador != null || $.trim(idIndicador) != "") {
                CrearFormulaView.Metodos.CargarDatosDependientesDeIndicador(
                    idIndicador,
                    CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo
                );
            }
        });

        $(document).on("change", CrearFormulaView.Controles.formCrearFormula.radioCategoriaDesagregacion, function () {
            $(CrearFormulaView.Controles.formCrearFormula.divInputCategoriaDesagregacion).css("display", "block");

            let indicador = $(CrearFormulaView.Controles.formCrearFormula.ddlIndicadorFormulario).val();

            if (indicador != null && indicador != "") {
                CrearFormulaView.Metodos.CargarCategoriasDesagregacionDeIndicador(indicador);
            }
        });

        $(document).on("change", CrearFormulaView.Controles.formCrearFormula.radioTotal, function () {
            $(CrearFormulaView.Controles.formCrearFormula.ddlCategoriaDesagregacion).empty();
            $(CrearFormulaView.Controles.formCrearFormula.divInputCategoriaDesagregacion).css("display", "none");
        });

        $(CrearFormulaView.Controles.formCrearFormula.inputs).on("keyup", function (e) {
            CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CrearFormulaView.Controles.formCrearFormula.selects2).on('select2:select', function (e) {
            CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CrearFormulaView.Controles.formCrearFormula.ddlCategoriaDesagregacion).on('select2:unselect', function (e) {
            RemoverOpcionesSelect2Multiple(e.params.data.text);

            CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CrearFormulaView.Controles.formCrearFormula.inputDates + "," +
            CrearFormulaView.Controles.formCrearFormula.inputRadios).on('change', function (e) {
                CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
            });

        // --------------------------------------
    },

    Init: function () {
        CrearFormulaView.Eventos();
        CrearFormulaView.Metodos.CambiarEstadoBtnSiguienteFormCrearFormula(true);

        let modo = $(CrearFormulaView.Controles.modoFormulario).val();
        console.log(modo);
        if (modo == null) {

        }
        else if (modo == jsUtilidades.Variables.Acciones.Editar) {
            CrearFormulaView.Metodos.CargarCategoriasDeFormulaNivelCalculo();
        }
        else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            CrearFormulaView.Metodos.CargarCategoriasDeFormulaNivelCalculo();
        }
        else if (modo == jsUtilidades.Variables.Acciones.Visualizar) {
            CrearFormulaView.Metodos.CargarCategoriasDeFormulaNivelCalculo();
        }

        if (modo != null) {
            if ($(CrearFormulaView.Controles.formCrearFormula.radioTotal).is(':checked')) {
                CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
            }
        }
    }
};

// Paso 2
GestionFormulaView = {
    Controles: {
        ddlFuenteIndicador: "#ddlFuenteIndicador",
        ddlGrupoFonatel: "#ddlGrupoFonatel",
        dllServicio: "#dllServicio",
        ddlTipoFonatel: "#ddlTipoFonatel",
        ddlIndicador: "#ddlIndicador",
        btnAgregarDetalleAgregacion: "#btnAgregarDetalleAgregacion",
        btnEliminarDetalleAgregacion: "#btnEliminarDetalleAgregacion",

        divGrupo: "#divGrupo",
        divClasificacion: "#divClasificacion",
        divTipoIndicador: "#divTipoIndicador",
        divIndicador: "#divIndicador",
        divServicio: "#divServicio",
        divAcumulacion: "#divAcumulacion",
        btnAtrasGestionFormula: "#btnAtrasGestionFormula",
        btnFinalizarFormulaCalculo: "#btnFinalizarFormulaCalculo",
        btnGuardarGestionFormulaCalculo: "#btnGuardarGestionFormulaCalculo",
        btnCancelarGestionFormulaCalculo: "#btnCancelarGestionFormulaCalculo",
        btnCalendarFormula: "#btnCalendarFormula",

        btnRemoverItemFormula: "#btnRemoverItemFormula",

        chkValorTotal: "#chkValorTotal",
        btnAgregarArgumento: "#TableaIndicadoresVariable tbody tr td .btn-add",
        btnEjecutarFormula: "#btnEjecutarFormula",

        columnaDetalleTabla: "#columnaDetalleTabla",

        modalFormulaDetalleAgregacion: "#modalFormulaDetalleAgregacion",
        modalFechaFormulaCalculo: "#modalFechaFormulaCalculo",

        divFechaInicioFormulaCalculo: "#divFechaInicioFormulaCalculo",
        divCategoríaFechaInicioFormulaCalculo: "#divCategoríaFechaInicioFormulaCalculo",
        divFechaFinalFormulaCalculo: "#divFechaFinalFormulaCalculo",
        divCategoríaFechaFinalFormulaCalculo: "#divCategoríaFechaFinalFormulaCalculo",

        divStep2: "#step-2 input, #step-2 select, #step-2 button",

        // Modal detalle
        modalDetalle: {
            titulo: "#titulo_modalDetalleFormulaCalculo",

            divCriterio: "#divCriterio_ModalDetalle",
            divCategoria: "#divCategoria_ModalDetalle",

            ddlCategoria: "#ddlCategoria_ModalDetalle",
            ddlDetalle: "#ddlDetalle_ModalDetalle",
            ddlCriterio: "#ddlCriterio_ModalDetalle",

            btnGuardar: "#btnGuardar_modalDetalle",
            btnEliminar: "#btnEliminar_modalDetalle",
        },

        // Modal Fechas - Fórmula de cálculo
        modalFecha: {
            ddlTipoFechaFinal: "#ddlTipoFechaFinalModalFechaFormula",
            ddlTipoFechaInicio: "#ddlTipoFechaInicioModalFechaFormula",

            btnGuardar: "#btnGuardar_modalFechaFormulaCalculo",
            btnCancelar: "#btnCancelar_modalFechaFormulaCalculo",
            btnEliminar: "#btnGuardar",
        },
    },

    Variables: {
        Direccion: {
            FONATEL: 1,
            MERCADOS: 2,
            CALIDAD: 3
        },

        FECHAS: {
            ACTUAL: "3",
            Categoría: "2",
            FECHA: "1"
        }
    },

    Mensajes: {},

    Metodos: {},

    Consultas: {},

    Eventos: function () {
        $(document).on("change", GestionFormulaView.Controles.chkValorTotal, function () {
            if (chkValorTotal.checked) {
                $(GestionFormulaView.Controles.btnAgregarDetalleAgregacion).prop("disabled", true);
            }
            else {
                $(GestionFormulaView.Controles.btnAgregarDetalleAgregacion).prop("disabled", false);
            }
        });

        $(document).on("change", GestionFormulaView.Controles.modalFecha.ddlTipoFechaInicio, function () {
            $(GestionFormulaView.Controles.divCategoríaFechaInicioFormulaCalculo).addClass("hidden");
            $(GestionFormulaView.Controles.divFechaInicioFormulaCalculo).addClass("hidden");

            let option = $(this).val();

            switch (option) {

                case GestionFormulaView.Variables.FECHAS.FECHA:
                    $(GestionFormulaView.Controles.divFechaInicioFormulaCalculo).removeClass("hidden");
                    break
                case GestionFormulaView.Variables.FECHAS.Categoría:
                    $(GestionFormulaView.Controles.divCategoríaFechaInicioFormulaCalculo).removeClass("hidden");
                    break;
                default:
            }
        });

        $(document).on("change", GestionFormulaView.Controles.modalFecha.ddlTipoFechaFinal, function () {
            $(GestionFormulaView.Controles.divFechaFinalFormulaCalculo).addClass("hidden");
            $(GestionFormulaView.Controles.divCategoríaFechaFinalFormulaCalculo).addClass("hidden");
            let option = $(this).val();

            switch (option) {

                case GestionFormulaView.Variables.FECHAS.FECHA:
                    $(GestionFormulaView.Controles.divFechaFinalFormulaCalculo).removeClass("hidden");
                    break
                case GestionFormulaView.Variables.FECHAS.Categoría:
                    $(GestionFormulaView.Controles.divCategoríaFechaFinalFormulaCalculo).removeClass("hidden");
                    break;
                default:
            }
        });

        $(document).on("change", GestionFormulaView.Controles.ddlFuenteIndicador, function () {
            let optionSelected = $(this).select2('data')[0].id;

            if (optionSelected == "1") {
                $(GestionFormulaView.Controles.divGrupo).css("display", "block");
                $(GestionFormulaView.Controles.divClasificacion).css("display", "block");
                $(GestionFormulaView.Controles.divTipoIndicador).css("display", "block");
                $(GestionFormulaView.Controles.divIndicador).css("display", "block");
                $(GestionFormulaView.Controles.divServicio).css("display", "none");
                $(GestionFormulaView.Controles.divAcumulacion).css("display", "block");

                $(GestionFormulaView.Controles.columnaDetalleTabla).html(GestionFormulaView.Mensajes.labelDetalleDesagregacion);
                $(GestionFormulaView.Controles.modalDetalle.titulo).html(GestionFormulaView.Mensajes.labelDetalleDesagregacion);
                $(GestionFormulaView.Controles.modalDetalle.divCategoria).css("display", "block");
                $(GestionFormulaView.Controles.modalDetalle.divCriterio).css("display", "none");
            }
            else if (optionSelected != "1") {
                $(GestionFormulaView.Controles.divGrupo).css("display", "block");
                $(GestionFormulaView.Controles.divServicio).css("display", "block");
                $(GestionFormulaView.Controles.divClasificacion).css("display", "none");
                $(GestionFormulaView.Controles.divTipoIndicador).css("display", "block");
                $(GestionFormulaView.Controles.divIndicador).css("display", "block");
                $(GestionFormulaView.Controles.divAcumulacion).css("display", "none");

                $(GestionFormulaView.Controles.columnaDetalleTabla).html(GestionFormulaView.Mensajes.labelDetalleAgrupacion);
                $(GestionFormulaView.Controles.modalDetalle.titulo).html(GestionFormulaView.Mensajes.labelDetalleAgrupacion);
                $(GestionFormulaView.Controles.modalDetalle.divCategoria).css("display", "none");
                $(GestionFormulaView.Controles.modalDetalle.divCriterio).css("display", "block");

            }
        });

        $(document).on("click", GestionFormulaView.Controles.btnAgregarDetalleAgregacion, function () {
            $(GestionFormulaView.Controles.modalFormulaDetalleAgregacion).modal('show');
            $(GestionFormulaView.Controles.modalDetalle.btnGuardar).css("display", "initial");
            $(GestionFormulaView.Controles.modalDetalle.btnEliminar).css("display", "none");

            $(GestionFormulaView.Controles.modalDetalle.ddlCategoria).select2("enable", "true");
            $(GestionFormulaView.Controles.modalDetalle.ddlDetalle).select2("enable", "true");
            $(GestionFormulaView.Controles.modalDetalle.ddlCriterio).select2("enable", "true");
        });

        $(document).on("click", GestionFormulaView.Controles.btnEliminarDetalleAgregacion, function () {
            $(GestionFormulaView.Controles.modalFormulaDetalleAgregacion).modal('show');
            $(GestionFormulaView.Controles.modalDetalle.btnGuardar).css("display", "none");
            $(GestionFormulaView.Controles.modalDetalle.btnEliminar).css("display", "initial");

            $(GestionFormulaView.Controles.modalDetalle.ddlCategoria).select2("enable", false);
            $(GestionFormulaView.Controles.modalDetalle.ddlDetalle).select2("enable", false);
            $(GestionFormulaView.Controles.modalDetalle.ddlCriterio).select2("enable", false);
        });

        $(document).on("click", GestionFormulaView.Controles.btnAtrasGestionFormula, function (e) {
            $("a[href='#step-1']").trigger('click');
        });

        $(document).on("click", GestionFormulaView.Controles.btnFinalizarFormulaCalculo, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaAgregarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaAgregada)
                        .set('onok', function (closeEvent) {
                            window.location.href = GestionFormulaView.Variables.indexViewURL;
                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.btnGuardarGestionFormulaCalculo, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaCreada)
                        .set('onok', function (closeEvent) {
                            window.location.href = GestionFormulaView.Variables.indexViewURL;
                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.btnCancelarGestionFormulaCalculo, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    window.location.href = GestionFormulaView.Variables.indexViewURL;
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalFecha.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    $(GestionFormulaView.Controles.modalFechaFormulaCalculo).modal('hide');
                });
        });

        $(document).on("click", GestionFormulaView.Controles.btnCalendarFormula, function (e) {
            $(GestionFormulaView.Controles.modalFechaFormulaCalculo).modal('show');
        });

        $(document).on("click", GestionFormulaView.Controles.radioManual_modalFechaFormula, function () {
            $(GestionFormulaView.Controles.divTxtFechaInicio_modalFechaFormula).css("display", "block");
            $(GestionFormulaView.Controles.divTxtFechaFinal_modalFechaFormula).css("display", "block");
            $(GestionFormulaView.Controles.divDdlCategoríasTipoFecha_modalFechaFormula).css("display", "none");
        });

        $(document).on("click", GestionFormulaView.Controles.radioCategoríaDesagregacion_modalFechaFormula, function () {
            $(GestionFormulaView.Controles.divTxtFechaInicio_modalFechaFormula).css("display", "none");
            $(GestionFormulaView.Controles.divTxtFechaFinal_modalFechaFormula).css("display", "none");
            $(GestionFormulaView.Controles.divDdlCategoríasTipoFecha_modalFechaFormula).css("display", "block");
        });

        $(document).on("click", GestionFormulaView.Controles.modalDetalle.btnGuardar, function () {
            jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoDetalleAgregado)
                .set('onok', function (closeEvent) {
                    $(GestionFormulaView.Controles.modalFormulaDetalleAgregacion).modal('hide');
                    $(GestionFormulaView.Controles.chkValorTotal).prop("disabled", true);
                });
        });

        $(document).on("click", GestionFormulaView.Controles.btnRemoverItemFormula, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminaArgumento, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoEliminado)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalFecha.btnGuardar, function () {
            jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoFechaCreado)
                .set('onok', function (closeEvent) {
                    $(GestionFormulaView.Controles.modalFechaFormulaCalculo).modal('hide');
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalFecha.btnEliminar, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminarArgumentoFecha, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoEliminarArgumentoFecha)
                        .set('onok', function (closeEvent) {
                            $(GestionFormulaView.Controles.modalFechaFormulaCalculo).modal('hide');
                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.btnAgregarArgumento, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaAgregarArgumento, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoAgregado)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.btnEjecutarFormula, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEjecutarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaEjecutada)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalDetalle.btnEliminar, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminarDetalle, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoEliminarDetalle)
                        .set('onok', function (closeEvent) {
                            $(GestionFormulaView.Controles.modalFormulaDetalleAgregacion).modal('hide');
                        });
                });
        });
    },

    Init: function () {
        GestionFormulaView.Eventos();
    }
};

$(function () {
    if ($(IndexView.Controles.IndexView).length > 0) {
        IndexView.Init();
    }

    if ($(CrearFormulaView.Controles.CreateView).length > 0) {
        CrearFormulaView.Init();    // paso 1
        GestionFormulaView.Init();  // paso 2
    }
});