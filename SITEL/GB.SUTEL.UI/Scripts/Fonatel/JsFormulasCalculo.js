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
                    "<button type = 'button' data - toggle='tooltip' data-placement='top' title = 'Clonar' value='" + formula.id + "' class='btn-icon-base btn-clone' ></button >" +
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

        // Formulario Crear fórmula de cálculo
        form: {
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

        exitoFormulaCreada: "La Fórmula ha sido creada",
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
                    if ($(CrearFormulaView.Controles.form.radioCategoriaDesagregacion).is(':checked')) {
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
            if ($(CrearFormulaView.Controles.form.radioCategoriaDesagregacion).is(':checked')) {
                let idParemeter = ObtenerValorParametroUrl("id");
                let indicador = $(CrearFormulaView.Controles.form.ddlIndicadorFormulario).val();

                if (idParemeter != null && idParemeter != "" && indicador != null && indicador != "") {
                    let formula = idParemeter;

                    $("#loading").fadeIn();

                    CrearFormulaView.Consultas.ConsultarCategoriasDesagregacionDeFormulaNivelCalculo(formula, indicador)
                        .then(data => {
                            SeleccionarItemsSelect2Multiple(
                                CrearFormulaView.Controles.form.ddlCategoriaDesagregacion,
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
            $(CrearFormulaView.Controles.form.ddlVariableDatoFormula).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreVariable });
            });
            InsertarDataSetSelect2(CrearFormulaView.Controles.form.ddlVariableDatoFormula, dataSet);
            $(CrearFormulaView.Controles.form.ddlVariableDatoFormula).val("");
        },

        InsertarDatosEnComboBoxCategorias: function (pData) {
            $(CrearFormulaView.Controles.form.ddlCategoriaDesagregacion).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreCategoria });
            });

            if (dataSet.length > 0) {
                InsertarOpcionTodosSelect2Multiple(CrearFormulaView.Controles.form.ddlCategoriaDesagregacion);
            }

            InsertarDataSetSelect2(CrearFormulaView.Controles.form.ddlCategoriaDesagregacion, dataSet);
        },

        CrearObjFormularioCrearFormula: function (pEsGuardadoParcial) {
            let controles = CrearFormulaView.Controles.form;

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

            if ($(CrearFormulaView.Controles.form.radioTotal).is(':checked')) {
                excepcionesForm.push(CrearFormulaView.Controles.form.ddlCategoriaDesagregacion.slice(1));
            }
            return excepcionesForm;
        },

        VerificarCamposIncompletosFormularioCrearFormula: function (pEsGuardadoParcial) {
            let prefijoHelp = CrearFormulaView.Controles.prefijoLabelsHelp;
            let camposObligatoriosGuardadoParcial = true;
            let controles = CrearFormulaView.Controles.form;

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
            $(CrearFormulaView.Controles.form.btnSiguienteCrear).prop('disabled', pDesactivar);
            $(CrearFormulaView.Controles.form.btnSiguienteEditar).prop('disabled', pDesactivar);
            $(CrearFormulaView.Controles.form.btnSiguienteClonar).prop('disabled', pDesactivar);
        },

        EventosEnInputsFormularioCrearFormulaCalculo: function () {
            let validacion = ValidarFormulario(
                CrearFormulaView.Controles.form.inputs,
                CrearFormulaView.Metodos.InputExcepcionesFormularioCrearFormula()
            );
            CrearFormulaView.Metodos.CambiarEstadoBtnSiguienteFormCrearFormula(!validacion.puedeContinuar);
        },

        CrearFormulaCalculo: function () {
            let validacionFormulario = ValidarFormulario(
                CrearFormulaView.Controles.form.inputs,
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
                CrearFormulaView.Controles.form.inputs,
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
                CrearFormulaView.Controles.form.inputs,
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
        $(document).on("click", CrearFormulaView.Controles.form.btnGuardar, function (e) {
            CrearFormulaView.Metodos.CrearFormulaGuardadoParcial();
        });

        $(document).on("click", CrearFormulaView.Controles.form.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CrearFormulaView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    window.location.href = CrearFormulaView.Variables.indexViewURL;
                });
        });

        $(document).on("click", CrearFormulaView.Controles.form.btnSiguienteCrear, function (e) {
            if (ObtenerValorParametroUrl("id") == null) {
                CrearFormulaView.Metodos.CrearFormulaCalculo();
            }
            else {
                CrearFormulaView.Metodos.EditarFormulaCalculo();
            }
        });

        // -- Editar fórmula de cálculo
        $(document).on("click", CrearFormulaView.Controles.form.btnEditar, function (e) {
            CrearFormulaView.Metodos.EditarFormulaCalculoGuardadoParcial();
        });

        $(document).on("click", CrearFormulaView.Controles.form.btnSiguienteEditar, function (e) {
            if (ObtenerValorParametroUrl("id") != null) {
                CrearFormulaView.Metodos.EditarFormulaCalculo();
            }
        });

        // -- Clonar fórmula de cálculo
        $(document).on("click", CrearFormulaView.Controles.form.btnClonar, function (e) {
            CrearFormulaView.Metodos.ClonarFormulaCalculoGuardadoParcial();
        });

        $(document).on("click", CrearFormulaView.Controles.form.btnSiguienteClonar, function (e) {
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
        $(document).on("click", CrearFormulaView.Controles.form.btnSiguienteVisualizar, function (e) {
            $(CrearFormulaView.Controles.step2).trigger('click');
        });

        $(CrearFormulaView.Controles.form.ddlIndicadorFormulario).on('select2:select', function (event) {
            let idIndicador = $(this).val();
            if (idIndicador != null || $.trim(idIndicador) != "") {
                CrearFormulaView.Metodos.CargarDatosDependientesDeIndicador(
                    idIndicador,
                    CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo
                );
            }
        });

        $(document).on("change", CrearFormulaView.Controles.form.radioCategoriaDesagregacion, function () {
            $(CrearFormulaView.Controles.form.divInputCategoriaDesagregacion).css("display", "block");

            let indicador = $(CrearFormulaView.Controles.form.ddlIndicadorFormulario).val();

            if (indicador != null && indicador != "") {
                CrearFormulaView.Metodos.CargarCategoriasDesagregacionDeIndicador(indicador);
            }
        });

        $(document).on("change", CrearFormulaView.Controles.form.radioTotal, function () {
            $(CrearFormulaView.Controles.form.ddlCategoriaDesagregacion).empty();
            $(CrearFormulaView.Controles.form.divInputCategoriaDesagregacion).css("display", "none");
        });

        $(CrearFormulaView.Controles.form.inputs).on("keyup", function (e) {
            CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CrearFormulaView.Controles.form.selects2).on('select2:select', function (e) {
            CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CrearFormulaView.Controles.form.ddlCategoriaDesagregacion).on('select2:unselect', function (e) {
            RemoverOpcionesSelect2Multiple(e.params.data.text);

            CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CrearFormulaView.Controles.form.inputDates + "," +
            CrearFormulaView.Controles.form.inputRadios).on('change', function (e) {
                CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
            });

        // --------------------------------------
    },

    Init: function () {
        CrearFormulaView.Eventos();
        CrearFormulaView.Metodos.CambiarEstadoBtnSiguienteFormCrearFormula(true);

        let modo = $(CrearFormulaView.Controles.modoFormulario).val();
        
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
            if ($(CrearFormulaView.Controles.form.radioTotal).is(':checked')) {
                CrearFormulaView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
            }
        }
    }
};

// Paso 2
GestionFormulaView = {
    Controles: {
        step2: "a[href='#step-2']",

        form: {
            ddlFuenteIndicador: "#ddlFuenteIndicador",
            ddlGrupo: "#ddlGrupo",
            ddlClasificacion: "#ddlClasificacion",
            ddlTipoIndicador: "#ddlTipoIndicador",
            ddlServicio: "#ddlServicio",
            ddlIndicador: "#ddlIndicador",
            ddlAcumulacion: "#ddlAcumulacion",

            labelddlGrupo: "#labelddlGrupo",

            divGrupo: "#divGrupo",
            divClasificacion: "#divClasificacion",
            divTipoIndicador: "#divTipoIndicador",
            divIndicador: "#divIndicador",
            divServicio: "#divServicio",
            divAcumulacion: "#divAcumulacion",

            columnaDetalleTabla: "#columnaDetalleTabla",

            chkValorTotal: "#chkValorTotal",
            btnAgregarDetalleAgregacion: "#btnAgregarDetalleAgregacion",
            btnEliminarDetalleAgregacion: "#btnEliminarDetalleAgregacion",
            btnAgregarArgumento: "#tablaDetallesIndicador tbody tr td .btn-add",

            btnCalendarFormula: "#btnCalendarFormula",
            btnRemoverItemFormula: "#btnRemoverItemFormula",

            btnEjecutarFormula: "#btnEjecutarFormula",

            btnAtras: "#btnAtrasGestionFormula",
            btnFinalizar: "#btnFinalizarFormulaCalculo",
            btnGuardar: "#btnGuardarGestionFormulaCalculo",
            btnCancelar: "#btnCancelarGestionFormulaCalculo",
        },

        tablaDetallesIndicador: "#tablaDetallesIndicador tbody",

        //divStep2: "#step-2 input, #step-2 select, #step-2 button",

        // Modal detalle
        modalDetalleAgregacion: {
            modal: "#modalFormulaDetalleAgregacion",

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
        modalFechaCalculo: {
            modal: "#modalFechaFormulaCalculo",

            divFechaInicio: "#divFechaInicioFormulaCalculo",
            divCategoriaFechaInicio: "#divCategoriaFechaInicioFormulaCalculo",
            divFechaFinal: "#divFechaFinalFormulaCalculo",
            divCategoriaFechaFinal: "#divCategoriaFechaFinalFormulaCalculo",

            ddlTipoFechaFinal: "#ddlTipoFechaFinalModalFechaFormula",
            ddlTipoFechaInicio: "#ddlTipoFechaInicioModalFechaFormula",

            btnGuardar: "#btnGuardar_modalFechaFormulaCalculo",
            btnCancelar: "#btnCancelar_modalFechaFormulaCalculo",
            btnEliminar: "#btnGuardar",
        },
    },

    Variables: {
        //Direccion: {
        //    FONATEL: 1,
        //    MERCADOS: 2,
        //    CALIDAD: 3
        //},

        FECHAS: {
            ACTUAL: "3",
            Categoría: "2",
            FECHA: "1"
        },

        FuenteIndicador: {
            IndicadorDGF: 1,
            IndicadorDGM: 2,
            IndicadorDGC: 3,
            IndicadorUIT: 4,
            IndicadorCruzado: 5,
            IndicadorFuenteExterna: 6
        },

        labelGrupo: "Grupo",
        labelAgrupacion: "Agrupación",

        cargoFuentesIndicador: false,
        tipoFuenteSeleccionada: false,
        attrCodigo: "data-codigo"
    },

    Mensajes: {
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
        CargarDatosFuentesIndicador: function () {
            $("#loading").fadeIn();

            GestionFormulaView.Consultas.ConsultarFuentesIndicador()
                .then(data => {
                    $(GestionFormulaView.Controles.form.ddlFuenteIndicador).empty();

                    let dataSet = []
                    data.objetoRespuesta?.forEach(item => {
                        dataSet.push(
                            {
                                value: item.IdFuenteIndicador,
                                text: item.Fuente,
                            });
                    });
                    InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlFuenteIndicador, dataSet, false);
                    $(GestionFormulaView.Controles.form.ddlFuenteIndicador).val("");

                    return data.objetoRespuesta?.length > 0;
                })
                .then(data => {
                    if (data)
                        GestionFormulaView.Variables.cargoFuentesIndicador = true;
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        HabilitarComboboxddlFuenteIndicador(pFuenteIndicador) {
            $(GestionFormulaView.Controles.form.divGrupo).css("display", "block");
            $(GestionFormulaView.Controles.form.divTipoIndicador).css("display", "block");
            $(GestionFormulaView.Controles.form.divIndicador).css("display", "block");

            if (pFuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                $(GestionFormulaView.Controles.form.divClasificacion).css("display", "block");
                $(GestionFormulaView.Controles.form.divServicio).css("display", "none");
                $(GestionFormulaView.Controles.form.divAcumulacion).css("display", "block");
                $(GestionFormulaView.Controles.form.divTipoIndicador).css("display", "block");

                $(GestionFormulaView.Controles.form.labelddlGrupo).text(GestionFormulaView.Variables.labelGrupo);
                $(GestionFormulaView.Controles.form.columnaDetalleTabla).html(GestionFormulaView.Mensajes.labelDetalleDesagregacion);
                $(GestionFormulaView.Controles.modalDetalleAgregacion.titulo).html(GestionFormulaView.Mensajes.labelDetalleDesagregacion);
                $(GestionFormulaView.Controles.modalDetalleAgregacion.divCategoria).css("display", "block");
                $(GestionFormulaView.Controles.modalDetalleAgregacion.divCriterio).css("display", "none");
            }
            else {
                $(GestionFormulaView.Controles.form.divServicio).css("display", "block");
                $(GestionFormulaView.Controles.form.divClasificacion).css("display", "none");
                $(GestionFormulaView.Controles.form.divAcumulacion).css("display", "none");

                $(GestionFormulaView.Controles.form.labelddlGrupo).text(GestionFormulaView.Variables.labelAgrupacion);
                $(GestionFormulaView.Controles.form.columnaDetalleTabla).html(GestionFormulaView.Mensajes.labelDetalleAgrupacion);
                $(GestionFormulaView.Controles.modalDetalleAgregacion.titulo).html(GestionFormulaView.Mensajes.labelDetalleAgrupacion);
                $(GestionFormulaView.Controles.modalDetalleAgregacion.divCategoria).css("display", "none");
                $(GestionFormulaView.Controles.modalDetalleAgregacion.divCriterio).css("display", "block");
            }

            if (pFuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorFuenteExterna) {
                $(GestionFormulaView.Controles.form.divServicio).css("display", "none");
                $(GestionFormulaView.Controles.form.divGrupo).css("display", "none");
                $(GestionFormulaView.Controles.form.divClasificacion).css("display", "none");
                $(GestionFormulaView.Controles.form.divAcumulacion).css("display", "none");
                $(GestionFormulaView.Controles.form.divTipoIndicador).css("display", "none");
                
            }
        },

        LimpiarComboxBoxDependientesDeddlFuenteIndicador() {
            $(GestionFormulaView.Controles.form.ddlGrupo).empty();
            $(GestionFormulaView.Controles.form.ddlClasificacion).empty();
            $(GestionFormulaView.Controles.form.ddlTipoIndicador).empty();
            $(GestionFormulaView.Controles.form.ddlIndicador).empty();
            $(GestionFormulaView.Controles.form.ddlServicio).empty();
        },

        InsertarDatosEnComboBoxGrupo: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlGrupo, dataSet, false);
            $(GestionFormulaView.Controles.form.ddlGrupo).val("");
        },

        InsertarDatosEnComboBoxClasificacion: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlClasificacion, dataSet, false);
            $(GestionFormulaView.Controles.form.ddlClasificacion).val("");
        },

        InsertarDatosEnComboBoxTipoIndicador: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlTipoIndicador, dataSet, false);
            $(GestionFormulaView.Controles.form.ddlTipoIndicador).val("");
        },

        InsertarDatosEnComboBoxServicioSitel: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlServicio, dataSet, false);
            $(GestionFormulaView.Controles.form.ddlServicio).val("");
        },

        InsertarDatosEnComboBoxIndicador: function (pData) {
            $(GestionFormulaView.Controles.form.ddlIndicador).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push(
                    {
                        value: item.id,
                        text: item.Nombre,
                        extraParameters: [
                            { attr: GestionFormulaView.Variables.attrCodigo, value: item.Codigo }
                        ]
                    });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlIndicador, dataSet, false);
            $(GestionFormulaView.Controles.form.ddlIndicador).val("");
        },

        InsertarDatosEnComboBoxTipoAcumulacion: function (pData) {
            $(GestionFormulaView.Controles.form.ddlAcumulacion).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Acumulacion });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlAcumulacion, dataSet, false);
            $(GestionFormulaView.Controles.form.ddlAcumulacion).val("");
        },

        CargarCatalogosParaFuenteIndicadorFonatel: function () {
            $("#loading").fadeIn();

            this.LimpiarComboxBoxDependientesDeddlFuenteIndicador();
            let fuenteIndicador = GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF;

            GestionFormulaView.Consultas.ConsultarGrupoIndicador(fuenteIndicador)
                .then(data => {
                    this.InsertarDatosEnComboBoxGrupo(data);
                })
                .then(() => {
                    return GestionFormulaView.Consultas.ConsultarClasificacionIndicador();
                })
                .then(data => {
                    this.InsertarDatosEnComboBoxClasificacion(data);
                })
                .then(() => {
                    return GestionFormulaView.Consultas.ConsultarTipoIndicador(fuenteIndicador);
                })
                .then(data => {
                    this.InsertarDatosEnComboBoxTipoIndicador(data);
                })
                .then(() => {
                    return GestionFormulaView.Consultas.ConsultarAcumulacionesFonatel();
                })
                .then(data => {
                    this.InsertarDatosEnComboBoxTipoAcumulacion(data);
                })
                .catch(error => { ManejoDeExcepciones(error); console.log(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CargarCatalogosParaFuenteIndicadorFueraDeFonatel: function (pFuenteIndicador) {
            $("#loading").fadeIn();

            this.LimpiarComboxBoxDependientesDeddlFuenteIndicador();

            GestionFormulaView.Consultas.ConsultarGrupoIndicador(pFuenteIndicador)
                .then(data => {
                    this.InsertarDatosEnComboBoxGrupo(data);
                })
                .then(() => {
                    return GestionFormulaView.Consultas.ConsultarServiciosSitel(pFuenteIndicador);
                })
                .then(data => {
                    this.InsertarDatosEnComboBoxServicioSitel(data);
                })
                .then(() => {
                    return GestionFormulaView.Consultas.ConsultarTipoIndicador(pFuenteIndicador);
                })
                .then(data => {
                    this.InsertarDatosEnComboBoxTipoIndicador(data);
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CrearObjIndicador: function () {
            let controles = GestionFormulaView.Controles.form;
            var formData = {
                GrupoIndicadores: {
                    id: $(controles.ddlGrupo).val()
                },
                ClasificacionIndicadores: {
                    id: $(controles.ddlClasificacion).val()
                },
                TipoIndicadores: {
                    id: $(controles.ddlTipoIndicador).val()
                }
            };
            return formData;
        },

        CrearObjServicio: function () {
            return {
                id: $(GestionFormulaView.Controles.form.ddlServicio).val()
            };
        },

        SeleccionarCargaDeDatosIndicador: function () {
            let tipoFuenteSeleccionada = GestionFormulaView.Variables.tipoFuenteSeleccionada;
            let grupo = $(GestionFormulaView.Controles.form.ddlGrupo).val();
            let servicio = $(GestionFormulaView.Controles.form.ddlServicio).val();
            let clasificacion = $(GestionFormulaView.Controles.form.ddlClasificacion).val();
            let tipoIndicador = $(GestionFormulaView.Controles.form.ddlTipoIndicador).val();

            if (grupo != null && grupo != "" && tipoIndicador != null && tipoIndicador != "") {
                if ((clasificacion != null && clasificacion != "") || (servicio != null && servicio != "")) {
                    this.CargarDatosIndicador(tipoFuenteSeleccionada);
                }
            }
        },

        CargarDatosIndicador: function (pTipoFuenteSeleccionada) {
            $("#loading").fadeIn();

            GestionFormulaView.Consultas.ConsultarIndicadores(
                this.CrearObjIndicador(), pTipoFuenteSeleccionada, this.CrearObjServicio()
            )
                .then(data => {
                    GestionFormulaView.Metodos.InsertarDatosEnComboBoxIndicador(data);
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CargarTablaDetallesIndicador: function (pIdIndicador) {
            $("#loading").fadeIn();

            GestionFormulaView.Consultas.ConsultarVariablesDatoCriteriosIndicador(pIdIndicador, GestionFormulaView.Variables.tipoFuenteSeleccionada)
                .then(obj => {
                    this.InsertarDatosTablaDetallesIndicador(obj.objetoRespuesta);
                })
                .catch(error => { ManejoDeExcepciones(error); console.log(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        InsertarDatosTablaDetallesIndicador: function (pListado) {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < pListado?.length; i++) {
                let detalle = pListado[i];
                html += `<tr><td scope='row'>${GestionFormulaView.Variables.CodigoIndicadorSeleccionado}</td>`;
                html += `<td>${detalle.NombreVariable}</td>`;
                html += "<td><input type='checkbox' id='chkValorTotal' /></td>";
                html += "<td>" + '<button type="submit" id="btnAgregarDetalleAgregacion" class="btn-icon-base btn-touch" data-toggle="tooltip" data-placement="top" title="Agregar detalle"></button>' + "</td>";
                html += "<td>" + '<button type="submit" id="" class="btn-icon-base btn-add" data-toggle="tooltip" data-placement="top" title="Agregar"></button>' + "</td>";
                
                html += "</tr>";
            }
            $(GestionFormulaView.Controles.tablaDetallesIndicador).html(html);
            CargarDatasource();
        },

        LimpiarTablaDetallesIndicador: function () {
            let table = $(GestionFormulaView.Controles.tablaDetallesIndicador).DataTable();
            table.clear().draw();
        }
    },

    Consultas: {
        ConsultarFuentesIndicador: function () {
            return execAjaxCall("/FormulaCalculo/ObtenerFuentesIndicador", "GET");
        },

        ConsultarGrupoIndicador: function (pFuenteIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerGrupoIndicador", "GET", { pFuenteIndicador });
        },

        ConsultarTipoIndicador: function (pFuenteIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerTipoIndicador", "GET", { pFuenteIndicador });
        },

        ConsultarClasificacionIndicador: function () {
            return execAjaxCall("/FormulaCalculo/ObtenerClasificacionIndicador", "GET");
        },

        ConsultarServiciosSitel: function (pFuenteIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerServicios", "GET", { pFuenteIndicador });
        },

        ConsultarIndicadores: function (pIndicador, pFuenteIndicador, pServicio) {
            return execAjaxCall("/FormulaCalculo/ObtenerIndicadores", "POST",
                { pIndicador, pFuenteIndicador, pServicio }
            );
        },

        ConsultarAcumulacionesFonatel: function () {
            return execAjaxCall("/FormulaCalculo/ObtenerAcumulacionesFonatel", "GET");
        },

        ConsultarVariablesDatoCriteriosIndicador: function (pIdIndicador, pFuenteIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerVariablesDatoCriteriosIndicador", "GET", { pIdIndicador, pFuenteIndicador });
        }
    },

    Eventos: function () {
        $(document).on("click", GestionFormulaView.Controles.step2, function () {
            if (!GestionFormulaView.Variables.cargoFuentesIndicador) {
                GestionFormulaView.Metodos.CargarDatosFuentesIndicador();
            }
        });

        $(document).on("change", GestionFormulaView.Controles.form.ddlFuenteIndicador, function () {
            let fuenteSeleccionada = $(this).val();
            GestionFormulaView.Variables.tipoFuenteSeleccionada = fuenteSeleccionada;

            GestionFormulaView.Metodos.HabilitarComboboxddlFuenteIndicador(fuenteSeleccionada);

            if (fuenteSeleccionada == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                GestionFormulaView.Metodos.CargarCatalogosParaFuenteIndicadorFonatel();
            }
            else if (fuenteSeleccionada == GestionFormulaView.Variables.FuenteIndicador.IndicadorFuenteExterna) {
                GestionFormulaView.Metodos.CargarDatosIndicador(fuenteSeleccionada);
            }
            else { // demas fuentes de indicadores
                GestionFormulaView.Metodos.CargarCatalogosParaFuenteIndicadorFueraDeFonatel(fuenteSeleccionada);
            }
        });

        $(document).on("change", GestionFormulaView.Controles.form.ddlGrupo, function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(document).on("change", GestionFormulaView.Controles.form.ddlServicio, function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(document).on("change", GestionFormulaView.Controles.form.ddlTipoIndicador, function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(document).on("change", GestionFormulaView.Controles.form.ddlClasificacion, function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(document).on("change", GestionFormulaView.Controles.form.ddlIndicador, function (e) {
            GestionFormulaView.Variables.CodigoIndicadorSeleccionado = $(this).find(":selected").attr(GestionFormulaView.Variables.attrCodigo);
            GestionFormulaView.Metodos.CargarTablaDetallesIndicador($(this).val());
            
        });

        // | Eventos por probar y rehacer   |
        // |                                |
        // V                                V

        $(document).on("change", GestionFormulaView.Controles.form.chkValorTotal, function () {
            if (GestionFormulaView.Controles.form.chkValorTotal.checked) {
                $(GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion).prop("disabled", true);
            }
            else {
                $(GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion).prop("disabled", false);
            }
        });

        $(document).on("change", GestionFormulaView.Controles.modalFechaCalculo.ddlTipoFechaInicio, function () {
            $(GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaInicio).addClass("hidden");
            $(GestionFormulaView.Controles.modalFechaCalculo.divFechaInicio).addClass("hidden");

            let option = $(this).val();

            switch (option) {

                case GestionFormulaView.Variables.FECHAS.FECHA:
                    $(GestionFormulaView.Controles.modalFechaCalculo.divFechaInicio).removeClass("hidden");
                    break
                case GestionFormulaView.Variables.FECHAS.Categoría:
                    $(GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaInicio).removeClass("hidden");
                    break;
                default:
            }
        });

        $(document).on("change", GestionFormulaView.Controles.modalFechaCalculo.ddlTipoFechaFinal, function () {
            $(GestionFormulaView.Controles.modalFechaCalculo.divFechaFinal).addClass("hidden");
            $(GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaFinal).addClass("hidden");
            let option = $(this).val();

            switch (option) {

                case GestionFormulaView.Variables.FECHAS.FECHA:
                    $(GestionFormulaView.Controles.modalFechaCalculo.divFechaFinal).removeClass("hidden");
                    break
                case GestionFormulaView.Variables.FECHAS.Categoría:
                    $(GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaFinal).removeClass("hidden");
                    break;
                default:
            }
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion, function () {
            $(GestionFormulaView.Controles.modalDetalleAgregacion.modal).modal('show');
            $(GestionFormulaView.Controles.modalDetalleAgregacion.btnGuardar).css("display", "initial");
            $(GestionFormulaView.Controles.modalDetalleAgregacion.btnEliminar).css("display", "none");

            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).select2("enable", "true");
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).select2("enable", "true");
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).select2("enable", "true");
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnEliminarDetalleAgregacion, function () {
            $(GestionFormulaView.Controles.modalDetalleAgregacion.modal).modal('show');
            $(GestionFormulaView.Controles.modalDetalleAgregacion.btnGuardar).css("display", "none");
            $(GestionFormulaView.Controles.modalDetalleAgregacion.btnEliminar).css("display", "initial");

            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).select2("enable", false);
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).select2("enable", false);
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).select2("enable", false);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnFinalizar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaAgregarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaAgregada)
                        .set('onok', function (closeEvent) {
                            window.location.href = GestionFormulaView.Variables.indexViewURL;
                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnGuardar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaCreada)
                        .set('onok', function (closeEvent) {
                            window.location.href = GestionFormulaView.Variables.indexViewURL;
                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    window.location.href = GestionFormulaView.Variables.indexViewURL;
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalFechaCalculo.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('hide');
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnCalendarFormula, function (e) {
            $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('show');
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

        $(document).on("click", GestionFormulaView.Controles.modalDetalleAgregacion.btnGuardar, function () {
            jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoDetalleAgregado)
                .set('onok', function (closeEvent) {
                    $(GestionFormulaView.Controles.modalDetalleAgregacion.modal).modal('hide');
                    $(GestionFormulaView.Controles.form.chkValorTotal).prop("disabled", true);
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnRemoverItemFormula, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminaArgumento, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoEliminado)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalFechaCalculo.btnGuardar, function () {
            jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoFechaCreado)
                .set('onok', function (closeEvent) {
                    $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('hide');
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalFechaCalculo.btnEliminar, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminarArgumentoFecha, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoEliminarArgumentoFecha)
                        .set('onok', function (closeEvent) {
                            $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('hide');
                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnAgregarArgumento, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaAgregarArgumento, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoAgregado)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnEjecutarFormula, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEjecutarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaEjecutada)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.modalDetalleAgregacion.btnEliminar, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminarDetalle, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoEliminarDetalle)
                        .set('onok', function (closeEvent) {
                            $(GestionFormulaView.Controles.modalDetalleAgregacion.modal).modal('hide');
                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnAtras, function (e) {
            $("a[href='#step-1']").trigger('click');
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