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

        preguntaActivarFormula: "¿Desea activar la Fórmula de Cálculo?",
        exitoActivarFormula: "La Fórmula de Cálculo ha sido activada",

        preguntaDesactivarFormula: "¿Desea desactivar la Fórmula de Cálculo?",
        exitoDesactivarFormula: "La Fórmula de Cálculo ha sido desactivada"
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
                .catch(error => { ManejoDeExcepciones(error); console.log(error) })
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
                        .catch(error => { ManejoDeExcepciones(error); console.log(error); })
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
                EsGuardadoParcial: pEsGuardadoParcial,
                Formula: ""
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
        step1: "a[href='#step-1']",
        step2: "a[href='#step-2']",
        prefijoLabelsHelp: "Help",

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

            // construcción de fórmula

            inputFormulaCalculo: ".divFormulaCalculo input.editable",

            btnCalendario: "#btnCalendario",
            btnSumar: "#btnSumar",
            btnRestar: "#btnRestar",
            btnMultiplicar: "#btnMultiplicar",
            btnDividir: "#btnDividir",
            btnMenorQue: "#btnMenorQue",
            btnMayorQue: "#btnMayorQue",
            btnMenorIgualQue: "#btnMenorIgualQue",
            btnMayorIgualQue: "#btnMayorIgualQue",
            btnIgualQue: "#btnIgualQue",
            btnParentesisAbrierto: "#btnParentesisAbrierto",
            btnParentesisCerrado: "#btnParentesisCerrado",

            btnRemoverItemFormula: "#btnRemoverItemFormula",
            btnEjecutarFormula: "#btnEjecutarFormula",

            // controles de navegación y guardado
            btnAtras: "#btnAtrasGestionFormula",
            btnFinalizar: "#btnFinalizarFormulaCalculo",
            btnGuardar: "#btnGuardarGestionFormulaCalculo",
            btnCancelar: "#btnCancelarGestionFormulaCalculo",
        },

        tablaDetallesIndicador: "#tablaDetallesIndicador tbody",

        // Modal detalle
        modalDetalleAgregacion: {
            modal: "#modalFormulaDetalleAgregacion",

            titulo: "#titulo_modalDetalleFormulaCalculo",

            divCriterio: "#divCriterio_ModalDetalle",
            divCategoria: "#divCategoria_ModalDetalle",

            ddlCategoria: "#ddlCategoria_ModalDetalle",
            ddlCriterio: "#ddlCriterio_ModalDetalle",
            ddlDetalle: "#ddlDetalle_ModalDetalle",

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

            radioDias: "#radioDias_modalFechaFormula",
            radioGroupName: "radiosUnidadDeMedidaModalFormula",
            ddlTipoFechaInicio: "#ddlTipoFechaInicioModalFechaFormula",
            ddlTipoFechaFinal: "#ddlTipoFechaFinalModalFechaFormula",
            ddlCategoriasFechaInicio: "#ddlCategoriasFechaInicio",
            ddlCategoriasFechaFinal: "#ddlCategoriasFechaFinal",
            txtFechaInicio: "#txtFechaInicioModalDefinicion",
            txtFechaFinal: "#txtFechaFinalModalDefinicion",

            btnGuardar: "#btnGuardar_modalFechaFormulaCalculo",
            btnCancelar: "#btnCancelar_modalFechaFormulaCalculo",
            btnEliminar: "#btnEliminar_modalFechaFormulaCalculo",
        }
    },

    Variables: {
        DefinionFecha: {
            fecha: 1,
            actual: 3,
            categoria: 2
        },

        //Define el comporamiento de un elemento de la fórmula cuando se quiere borrar o insertar un caracter dependiendo de la posicion del cursor
        //Numero permite insertar objetos en medio y lo divide para que sean dos numeros, de lo contrario el nuevo caracter se inserta fuera de la variable u operador
        TipoObjetoFormulaCalculo :{
            Numero: 1,
            Operador: 2,
            Variable: 3
        },

        FuenteIndicador: {
            IndicadorDGF: 1,
            IndicadorDGM: 2,
            IndicadorDGC: 3,
            IndicadorUIT: 4,
            IndicadorCruzado: 5,
            IndicadorFuenteExterna: 6
        },

        Operadores: {
            sumar: "+",
            restar: "-",
            multiplicar: "*",
            dividir: "/",
            menorQue: "<",
            mayorQue: ">",
            menorIgual: "<=",
            mayorIgual: ">=",
            igual: "=",
            abrirParentesis: "(",
            cerrarParentesis: ")",
            GetRegex: () => /^[\+\-*\/<>=().0-9 ]+$/
        },

        //Se usa para definir el tipo de objeto de un operador en la fórmula, si es fecha se puede modificar, si es criterio solo se puede borrar
        TipoArgumento: {
            criterio: 1,
            fecha: 2
        },

        attrCodigo: "data-codigo",
        attrIdentificador: "data-identificador",
        labelGrupo: "Grupo",
        labelAgrupacion: "Agrupación",
        labelHoy: "Hoy",
        tooltipBtnAgregarDetalleAgregacionAgregar: "Agregar detalle",
        tooltipBtnAgregarDetalleAgregacionEliminar: "Eliminar detalle",

        codigoIndicadorSeleccionado: "",
        cargoFuentesIndicador: false,
        cargoTiposFechasModalDeficionFechas: false,
        listaConfigDetallesIndicador: [],
        listaConfigDefinicionFechas: [],
        filaSeleccionadaTablaDetalles: null,
        FormulaCalculo: [],

        indexViewURL: "/Fonatel/FormulaCalculo/Index",
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

        preguntaGuardadoParcial: "¿Desea realizar un guardado parcial de la Fórmula de Cálculo?",
    },

    Metodos: {
        CargarDatosStep2: function () {
            $("#loading").fadeIn();

            GestionFormulaView.Consultas.ConsultarFuentesIndicador()
                .then(data => {
                    GestionFormulaView.Metodos.InsertarDatosEnComboBoxFuente(data);
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

            if (pFuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorFuenteExterna
                || pFuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorUIT
                || pFuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorCruzado) {
                $(GestionFormulaView.Controles.form.divServicio).css("display", "none");
                $(GestionFormulaView.Controles.form.divGrupo).css("display", "none");
                $(GestionFormulaView.Controles.form.divClasificacion).css("display", "none");
                $(GestionFormulaView.Controles.form.divAcumulacion).css("display", "none");
                $(GestionFormulaView.Controles.form.divTipoIndicador).css("display", "none");
                $(GestionFormulaView.Controles.form.divIndicador).css("display", "none");
                
            }
        },

        LimpiarComboxBoxDependientesDeddlFuenteIndicador() {
            $(GestionFormulaView.Controles.form.ddlGrupo).empty();
            $(GestionFormulaView.Controles.form.ddlClasificacion).empty();
            $(GestionFormulaView.Controles.form.ddlTipoIndicador).empty();
            $(GestionFormulaView.Controles.form.ddlIndicador).empty();
            $(GestionFormulaView.Controles.form.ddlServicio).empty();
        },

        InsertarDatosEnComboBoxFuente: function (pData) {
            $(GestionFormulaView.Controles.form.ddlFuenteIndicador).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push(
                    {
                        value: item.id,
                        text: item.Fuente,
                        extraParameters: [
                            { attr: GestionFormulaView.Variables.attrIdentificador, value: item.IdFuenteIndicador }
                        ]
                    });
            });
            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlFuenteIndicador, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.form.ddlFuenteIndicador, "");
        },

        InsertarDatosEnComboBoxGrupo: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlGrupo, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.form.ddlGrupo, "");
        },

        InsertarDatosEnComboBoxClasificacion: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlClasificacion, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.form.ddlClasificacion, "");
        },

        InsertarDatosEnComboBoxTipoIndicador: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlTipoIndicador, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.form.ddlTipoIndicador, "");
        },

        InsertarDatosEnComboBoxServicioSitel: function (pData) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Nombre });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlServicio, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.form.ddlServicio, "");
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
            SeleccionarItemSelect2(GestionFormulaView.Controles.form.ddlIndicador, "");
        },

        InsertarDatosEnComboBoxTipoAcumulacion: function (pData) {
            $(GestionFormulaView.Controles.form.ddlAcumulacion).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Acumulacion });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.form.ddlAcumulacion, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.form.ddlAcumulacion, "");
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
                .catch(error => { ManejoDeExcepciones(error); })
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
            let fuenteIndicador = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);
            let grupo = $(GestionFormulaView.Controles.form.ddlGrupo).val();
            let servicio = $(GestionFormulaView.Controles.form.ddlServicio).val();
            let clasificacion = $(GestionFormulaView.Controles.form.ddlClasificacion).val();
            let tipoIndicador = $(GestionFormulaView.Controles.form.ddlTipoIndicador).val();

            this.InsertarDatosTablaDetallesIndicador([]); // reutilizar el método para limpiar la tabla

            if (grupo != null && grupo != "" && tipoIndicador != null && tipoIndicador != "") {
                if ((clasificacion != null && clasificacion != "") || (servicio != null && servicio != "")) {
                    this.CargarDatosIndicador(fuenteIndicador);
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
                .then(_ => {
                    GestionFormulaView.Metodos.LimpiarComboxBoxModalDetallesIndicador();
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CargarTablaDetallesIndicador: function (pIdIndicador) {
            let fuenteIndicador = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            $("#loading").fadeIn();

            GestionFormulaView.Consultas.ConsultarVariablesDatoCriteriosIndicador(pIdIndicador, fuenteIndicador)
                .then(obj => {
                    this.InsertarDatosTablaDetallesIndicador(obj.objetoRespuesta);
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        InsertarDatosTablaDetallesIndicador: function (pListado) {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < pListado?.length; i++) {
                let codigo = GestionFormulaView.Variables.codigoIndicadorSeleccionado;
                if (codigo == "" || codigo == null) { codigo = "-"; }

                let fuenteSeleccionada = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);
                let disabled, checked = "";

                if (fuenteSeleccionada == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGC) {
                    disabled = "disabled"; checked = "checked";
                }

                let detalle = pListado[i];
                html += `<tr value="${detalle.id}">`;
                html += `<td scope='row'>${codigo}</td>`;
                html += `<td>${detalle.NombreVariable}</td>`;
                html += `<td><input type='checkbox' id='${GestionFormulaView.Controles.form.chkValorTotal.slice(1)}' ${disabled} ${checked}/></td>`;
                html += `<td><button type='submit' id='${GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion.slice(1)}'
                        class='btn-icon-base btn-touch' data-toggle='tooltip' data-placement='top' title='${GestionFormulaView.Variables.tooltipBtnAgregarDetalleAgregacionAgregar}' ${disabled}></button></td>`;
                html += `<td><button type='submit' id='' class='btn-icon-base btn-add' data-toggle='tooltip' data-placement='top' title='Agregar'></button></td>`;
                html += "</tr>";
            }

            $(GestionFormulaView.Controles.tablaDetallesIndicador).html(html);
            CargarDatasource();
        },

        ConstruirArgumento: function (pTipoArgumento) {
            let argumentoConstruido = null;

            if (pTipoArgumento == GestionFormulaView.Variables.TipoArgumento.criterio) {
                let variableCriterio = $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).attr("value");
                let objDetalle = GestionFormulaView.Variables.listaConfigDetallesIndicador[variableCriterio];

                if (objDetalle != null) {
                    argumentoConstruido = {
                        tipoObjeto: GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable,
                        etiqueta: this.ConstruirLabelArgumentoTipoVariableDatoCriterio(objDetalle.codigoIndicador, objDetalle.nombreVariable),
                        tipoArgumento: pTipoArgumento,
                        argumento: objDetalle
                    };
                }
            }
            else { // definición de fecha
                let indicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();
                let objFecha = GestionFormulaView.Variables.listaConfigDefinicionFechas[indicador];

                if (objFecha != null) {
                    argumentoConstruido = {
                        tipoObjeto: GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable,
                        etiqueta: this.ConstruirLabelArgumentoTipoFecha(objFecha),
                        tipoArgumento: pTipoArgumento,
                        argumento: objFecha,
                    };
                }
            }
            return argumentoConstruido;
        },

        ConstruirLabelArgumentoTipoVariableDatoCriterio: function (pArgIzquierda, pArgDerecha) {
            return `{${pArgIzquierda} - ${pArgDerecha}}`;
        },

        ConstruirLabelArgumentoTipoFecha: function (pConfigDefinicionFecha) {
            // tipo fecha final
            let parametro1, parametro2 = "";

            if (pConfigDefinicionFecha.idTipoFechaFinal == GestionFormulaView.Variables.DefinionFecha.categoria) {
                parametro1 = pConfigDefinicionFecha.nombreCategoriaFinal;
            }
            else if (pConfigDefinicionFecha.idTipoFechaFinal == GestionFormulaView.Variables.DefinionFecha.fecha) {
                parametro1 = pConfigDefinicionFecha.fechaFinal;
            }
            else {
                parametro1 = GestionFormulaView.Variables.labelHoy;
            }

            // tipo fecha inicio
            if (pConfigDefinicionFecha.idTipoFechaInicio == GestionFormulaView.Variables.DefinionFecha.categoria) {
                parametro2 = pConfigDefinicionFecha.nombreCategoriaInicio;
            }
            else if (pConfigDefinicionFecha.idTipoFechaInicio == GestionFormulaView.Variables.DefinionFecha.fecha) {
                parametro2 = pConfigDefinicionFecha.fechaInicio;
            }
            else {
                parametro2 = GestionFormulaView.Variables.labelHoy;
            }

            return this.ConstruirLabelArgumentoTipoVariableDatoCriterio(parametro1, parametro2);
        },

        RegistrarCriterio: function (pElementRoot) {
            GestionFormulaView.Variables.filaSeleccionadaTablaDetalles = $(pElementRoot).parent().parent();
            let argumento = GestionFormulaView.Metodos.ConstruirArgumento(GestionFormulaView.Variables.TipoArgumento.criterio);
            
            if (argumento) {
                new Promise((resolve, reject) => {
                    jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaAgregarArgumento, jsMensajes.Variables.actionType.agregar)
                        .set('onok', function (_) { resolve(true); });
                })
                    .then(_ => {
                        GestionFormulaView.Metodos.AgregarVariableAFormula(argumento);
                        jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoAgregado)
                            .set('onok', function (_) { });
                    });
            }
        },

        AniadirElementoAFormula: function (pOperador, pIndex) {
            let cantCaracteres = 0;

            if (GestionFormulaView.Variables.FormulaCalculo.length == 0) {
                GestionFormulaView.Variables.FormulaCalculo.push(pOperador)
                if (pOperador.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                    return pOperador.etiqueta.toString().length;
                }
                return pOperador.argumento.toString().length;
            }

            for (let i = 0; i < GestionFormulaView.Variables.FormulaCalculo.length; i++) {
                let item = GestionFormulaView.Variables.FormulaCalculo[i];
                if (item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                    cantCaracteres += item.etiqueta.toString().length;
                    if (cantCaracteres > (pIndex - 1)) {
                        GestionFormulaView.Variables.FormulaCalculo.splice((i), 0, pOperador)

                        if (pOperador.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                            return cantCaracteres;
                        } else {
                            return pIndex + pOperador.argumento.toString().length - 1;
                        }

                    } else if (cantCaracteres == (pIndex - 1)) {
                        GestionFormulaView.Variables.FormulaCalculo.splice((i + 1), 0, pOperador)
                        if (pOperador.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                            return cantCaracteres + pOperador.etiqueta.toString().length;
                        } else {
                            return cantCaracteres + pOperador.argumento.toString().length;
                        }
                    }
                } else {
                    let itemStr = item.argumento.toString();
                    cantCaracteres += itemStr.length;
                    if (cantCaracteres >= (pIndex - 1) && item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Numero && pOperador.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Numero) {
                        let inicioSimbolo = cantCaracteres - itemStr.length;
                        let charPos = pIndex - inicioSimbolo - 1;

                        let nuevo = itemStr.substring(0, (charPos))
                        let nuevo2 = itemStr.substring((charPos), itemStr.length)
                        GestionFormulaView.Variables.FormulaCalculo[i].argumento = nuevo + pOperador.argumento.toString() + nuevo2;
                        
                        return (inicioSimbolo + charPos + 1);
                    }
                    else if (cantCaracteres >= (pIndex - 1) && item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Numero && pOperador.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Operador) {
                        let inicioSimbolo = cantCaracteres - itemStr.length;
                        let charPos = pIndex - inicioSimbolo - 1;

                        let nuevo = itemStr.substring(0, (charPos))
                        let nuevo2 = itemStr.substring((charPos), itemStr.length)

                        GestionFormulaView.Variables.FormulaCalculo.splice(i, 1);

                        if (nuevo != "") GestionFormulaView.Variables.FormulaCalculo.splice(i, 0, { tipoObjeto: GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Numero, argumento: nuevo })
                        GestionFormulaView.Variables.FormulaCalculo.splice((i + (nuevo == "" ? 0 : 1)), 0, pOperador)
                        if (nuevo2 != "") GestionFormulaView.Variables.FormulaCalculo.splice((i + (nuevo == "" ? 1 : 2)), 0, { tipoObjeto: GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Numero, argumento: nuevo2 })

                        return (inicioSimbolo + charPos + pOperador.argumento.toString().length);
                    }
                    else if (cantCaracteres >= (pIndex - 1)) {

                        let complementoCursor = 0;

                        if (cantCaracteres > (pIndex - 1)) {
                            GestionFormulaView.Variables.FormulaCalculo.splice((i), 0, pOperador)
                            complementoCursor = itemStr.length;

                        } else if (cantCaracteres == (pIndex - 1)) {
                            GestionFormulaView.Variables.FormulaCalculo.splice((i + 1), 0, pOperador)
                        }

                        if (pOperador.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                            return pIndex + pOperador.etiqueta.toString().length - 1;
                        }

                        return cantCaracteres + pOperador.argumento.toString().length - complementoCursor
                    }
                }
            }
        },

        BorrarOperadorAFormula: function (pIndex) {
            let cantCaracteres = 0;
            let posicionCursor = 0;

            for (let i = 0; i < GestionFormulaView.Variables.FormulaCalculo.length; i++) {
                let item = GestionFormulaView.Variables.FormulaCalculo[i];

                if (item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                    cantCaracteres += item.etiqueta.toString().length;
                    if (cantCaracteres >= pIndex) {
                        let borrado = GestionFormulaView.Variables.FormulaCalculo.splice(i, 1);
                        posicionCursor = cantCaracteres - (borrado[0].etiqueta.toString().length);

                        //ELIMINAR DEL OBJETO DEFINICION DE FECHAS
                        let indicador = borrado[0].argumento.indicador;
                        if (borrado[0].tipoArgumento == GestionFormulaView.Variables.TipoArgumento.fecha && GestionFormulaView.Variables.listaConfigDefinicionFechas.hasOwnProperty(indicador)) {
                            delete GestionFormulaView.Variables.listaConfigDefinicionFechas[indicador];
                        }

                        break;
                    }
                } else {
                    item = item.argumento.toString();
                    cantCaracteres += item.length;

                    if (cantCaracteres >= pIndex) {
                        if (item.length == 1) {
                            GestionFormulaView.Variables.FormulaCalculo.splice(i, 1);
                            posicionCursor = cantCaracteres - 1;
                            break;
                        } else {
                            let inicioSimbolo = cantCaracteres - item.length;
                            let charPos = pIndex - inicioSimbolo;

                            let nuevo = item.substring(0, (charPos - 1))
                            let nuevo2 = item.substring((charPos), item.length)
                            GestionFormulaView.Variables.FormulaCalculo[i].argumento = nuevo + nuevo2;
                            //console.log({longitud:item.length, charPos, cantCaracteres, index, inicioSimbolo, nuevo, nuevo2})
                            posicionCursor = inicioSimbolo + charPos - 1;
                            break;
                        }
                    }
                }
            }

            //VERIFICAR INTEGRIDAD DE LA FORMULA, UNIFICAR DOS NUMEROS SEGUIDOS
            for (let i = 0; i < GestionFormulaView.Variables.FormulaCalculo.length; i++) {
                let item = GestionFormulaView.Variables.FormulaCalculo[i];

                if (i < (GestionFormulaView.Variables.FormulaCalculo.length - 1)) {
                    if (item.tipoObjeto == GestionFormulaView.Variables.FormulaCalculo[(i + 1)].tipoObjeto && item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Numero) {
                        let borrado = GestionFormulaView.Variables.FormulaCalculo.splice((i + 1), 1)[0];
                        GestionFormulaView.Variables.FormulaCalculo[i].argumento = item.argumento.toString() + borrado.argumento.toString();
                    }
                }
            }
            return posicionCursor;
        },

        ObtenerPosicionCursor: function () {
            let el = $(GestionFormulaView.Controles.form.inputFormulaCalculo)[0]
            let val = el.value;
            return val.slice(0, el.selectionStart).length + 1;
        },

        MostrarFormulaCalculo: function () {
            let txtFormula = ""
            for (let i = 0; i < GestionFormulaView.Variables.FormulaCalculo.length; i++) {
                let item = GestionFormulaView.Variables.FormulaCalculo[i];
                if (item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                    simbolo = item.etiqueta;
                } else {
                    simbolo = item.argumento.toString();
                }
                if (i == 0) {
                    txtFormula += simbolo;
                } else {
                    txtFormula += simbolo;
                }
            }
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).val(txtFormula);
        },

        BotonAgregarOperadorFormula: function (pOperador) {
            let index = GestionFormulaView.Metodos.ObtenerPosicionCursor();
            let newIndex = GestionFormulaView.Metodos.AniadirElementoAFormula({ tipoObjeto: GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Operador, argumento: pOperador }, (index))
            GestionFormulaView.Metodos.MostrarFormulaCalculo();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).focus();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).setCursorPosition(newIndex);
        },

        AgregarVariableAFormula: function (pVariable) {
            let index = GestionFormulaView.Metodos.ObtenerPosicionCursor();
            let newIndex = GestionFormulaView.Metodos.AniadirElementoAFormula(pVariable, (index))
            GestionFormulaView.Metodos.MostrarFormulaCalculo();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).focus();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).setCursorPosition(newIndex);
        },

        AgregarFechaAFormula: function (pVariable) {
            let index = GestionFormulaView.Metodos.ObtenerPosicionCursor();
            let nuevo = true;

            //VERIFICAR SI LA FECHA YA EXISTE, Modificarlo
            for (let i = 0; i < GestionFormulaView.Variables.FormulaCalculo.length; i++) {
                let item = GestionFormulaView.Variables.FormulaCalculo[i];
                if (item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable) {
                    if (item.argumento.indicador == pVariable.argumento.indicador && item.tipoArgumento == GestionFormulaView.Variables.TipoArgumento.fecha) {
                        GestionFormulaView.Variables.FormulaCalculo.splice(i, 1);
                        GestionFormulaView.Variables.FormulaCalculo.splice(i, 0, pVariable)
                        nuevo = false;
                        break;
                    }
                }
            }

            if (nuevo) {
                index = GestionFormulaView.Metodos.AniadirElementoAFormula(pVariable, (index))
            }

            GestionFormulaView.Metodos.MostrarFormulaCalculo();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).focus();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).setCursorPosition(index);
        },

        BotonRemoverOperadorFormula: function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminaArgumento, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    GestionFormulaView.Variables.FormulaCalculo.pop();
                    GestionFormulaView.Metodos.MostrarFormulaCalculo();

                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoEliminado).set('onok', function (_) { });
                });
        },

        CrearObjFormularioCrearFormula: function (pEsGuardadoParcial) {
            let formula = "";
            for (let i = 0; i < GestionFormulaView.Variables.FormulaCalculo.length; i++) {
                let argumento = GestionFormulaView.Variables.FormulaCalculo[i];
                if (argumento != ' ') { formula += argumento.etiqueta != null ? argumento.etiqueta : argumento.argumento }
            }

            let formData = {
                id: ObtenerValorParametroUrl("id"),
                EsGuardadoParcial: pEsGuardadoParcial,
                Formula: formula
            };
            return formData;
        },

        CrearFormulaGuardadoParcial: function () {
            let formulaConstruida = GestionFormulaView.Variables.FormulaCalculo;

            if (formulaConstruida != null && formulaConstruida.length > 0) {
                new Promise((resolve, reject) => {
                    jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                        .set('onok', function (closeEvent) { resolve(true); })
                })
                    .then(_ => {
                        return GestionFormulaView.Consultas.CrearDetallesFormulaCalculo(
                            GestionFormulaView.Metodos.CrearObjFormularioCrearFormula(true),
                            formulaConstruida
                        );
                    })
                    .then(_ => {
                        jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaCreada)
                            .set('onok', function (closeEvent) {
                                window.location.href = GestionFormulaView.Variables.indexViewURL;
                            });
                    })
                    .catch(error => { ManejoDeExcepciones(error); console.log(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
        },

        // Modal detalle desagregacion/agrupación
        LimpiarComboxBoxModalDetallesIndicador() {
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).empty();
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).empty();
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).empty();
        },

        AbrirModalDetallesIndicador: function (pEsModoCreacion) {
            $(GestionFormulaView.Controles.modalDetalleAgregacion.btnEliminar).css("display", pEsModoCreacion ? "none" : "initial");
            $(GestionFormulaView.Controles.modalDetalleAgregacion.modal).modal('show');
        },

        InsertarDatosEnComboBoxCategoriasModalDetalle: function (pData) {
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreCategoria });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria, "");
        },

        InsertarDatosEnComboBoxCriteriosModalDetalle: function (pData) {
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Acumulacion });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio, "");
        },

        InsertarDatosEnComboBoxDetalleModalDetalle: function (pData) {
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.Etiquetas });
            });

            InsertarDataSetSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle, dataSet, false);
            SeleccionarItemSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle, "");
        },

        CargarModalDetallesIndicador: function () {
            this.LimpiarMensajesValidacionModalDetalle();

            let fuenteIndicador = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);
            let pIdIndicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();
            let idDetalle = $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).attr("value");
            let objDetalle = GestionFormulaView.Variables.listaConfigDetallesIndicador[idDetalle];
            
            if (objDetalle != undefined && objDetalle != null) { // edición
                this.CargarComboxesModalDetallesIndicadorModoEdicion(objDetalle);
            }
            else { // creación
                if (fuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                    this.CargarComboBoxCategoriasModalDetalle(pIdIndicador);
                }
                else {
                    this.CargarComboBoxCriterioModalDetalle(); // TO DO
                }
            }
        },

        CargarComboBoxCategoriasModalDetalle: function (pIdIndicador) {
            $("#loading").fadeIn();
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).empty();

            GestionFormulaView.Consultas.ConsultarCategoriasDesagregacionDeIndicador(pIdIndicador)
                .then(data => {
                    GestionFormulaView.Metodos.InsertarDatosEnComboBoxCategoriasModalDetalle(data);
                })
                .then(_ => {
                    GestionFormulaView.Metodos.AbrirModalDetallesIndicador(true);
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CargarComboBoxCriterioModalDetalle: function () {
            //$("#loading").fadeIn();
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).empty();

            GestionFormulaView.Metodos.AbrirModalDetallesIndicador(true);
        },

        CargarComboBoxDetallesModalDetalle: function (pIdCategoria) {
            $("#loading").fadeIn();
            let fuenteIndicador = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);
            let idIndicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();

            if (fuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                GestionFormulaView.Consultas.ConsultarListaDetallesDeCategoria(idIndicador, pIdCategoria)
                    .then(data => {
                        GestionFormulaView.Metodos.InsertarDatosEnComboBoxDetalleModalDetalle(data);
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
            else {

            }
        },

        CargarComboxesModalDetallesIndicadorModoEdicion: function (pObjDetalle) {
            $("#loading").fadeIn();
            let idIndicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();

            if (pObjDetalle.fuente == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).empty();

                GestionFormulaView.Consultas.ConsultarCategoriasDesagregacionDeIndicador(idIndicador)
                    .then(data => {
                        GestionFormulaView.Metodos.InsertarDatosEnComboBoxCategoriasModalDetalle(data);
                    })
                    .then(_ => {
                        SeleccionarItemSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria, pObjDetalle.categoria);
                    })
                    .then(_ => {
                        return GestionFormulaView.Consultas.ConsultarListaDetallesDeCategoria(idIndicador, pObjDetalle.categoria);
                    })
                    .then(data => {
                        GestionFormulaView.Metodos.InsertarDatosEnComboBoxDetalleModalDetalle(data);
                    })
                    .then(_ => {
                        SeleccionarItemSelect2(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle, pObjDetalle.detalle);
                    })
                    .then(_ => { // en caso de que se haya eliminado la categoria o detalle ya estando el elemento creado
                        let categoria = $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).val();
                        let detalle = $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).val();

                        if (categoria == null || categoria == "" || detalle == null || detalle == "") {
                            this.EliminarObjDetalleModalDetalle();
                            this.ActivarDesactivarBtnAbrirModalDetalle(true);
                            return true; // excepción, se devuelve a modo creación
                        }
                        return false; // se continua con el modo edición
                    })
                    .then(modo => {
                        GestionFormulaView.Metodos.AbrirModalDetallesIndicador(modo);
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
            else {
                $("#loading").fadeOut();
            }
        },

        LimpiarMensajesValidacionModalDetalle: function () {
            let prefijoHelp = GestionFormulaView.Controles.prefijoLabelsHelp;
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).parent().removeClass("has-error");
            $("#" + $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).attr("id") + prefijoHelp).css("display", "none");
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).parent().removeClass("has-error");
            $("#" + $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).attr("id") + prefijoHelp).css("display", "none");
            $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).parent().removeClass("has-error");
            $("#" + $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).attr("id") + prefijoHelp).css("display", "none");
        },

        ValidarComboBoxesModalDetalle: function (pFuenteIndicador) {
            let detalle = $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).val();
            let categoria = $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).val();
            let criterio = $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).val();
            let prefijoHelp = GestionFormulaView.Controles.prefijoLabelsHelp;
            let puedeContinuar = true;

            this.LimpiarMensajesValidacionModalDetalle();

            // validar campos
            if (detalle == "" || detalle == null) {
                $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle + prefijoHelp).css("display", "block");
                $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).parent().addClass("has-error");
                puedeContinuar = false;
            }

            if (pFuenteIndicador == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                if (categoria == "" || categoria == null) {
                    $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria + prefijoHelp).css("display", "block");
                    $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).parent().addClass("has-error");
                    puedeContinuar = false;
                }
            }
            else {
                if (criterio == "" || criterio == null) {
                    $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio + prefijoHelp).css("display", "block");
                    $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCriterio).parent().addClass("has-error");
                    puedeContinuar = false;
                }
            }
            return puedeContinuar;
        },

        ActivarDesactivarBtnAbrirModalDetalle: function (pEsModoCreacion) {
            let tooltip = pEsModoCreacion ? GestionFormulaView.Variables.tooltipBtnAgregarDetalleAgregacionAgregar : GestionFormulaView.Variables.tooltipBtnAgregarDetalleAgregacionEliminar;
            let cssClassRemover = pEsModoCreacion ? "btn-touch-disabled" : "btn-touch";
            let cssClassAniadir = pEsModoCreacion ? "btn-touch": "btn-touch-disabled";

            $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).find(GestionFormulaView.Controles.form.chkValorTotal).prop("disabled", !pEsModoCreacion);

            $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).find(GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion)
                .attr("data-original-title", tooltip);

            $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).find(GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion)
                .removeClass(cssClassRemover)
                .addClass(cssClassAniadir);
        },

        AgregarObjDetalleModalDetalle: function (pEsValorTotal = false) {
            let variableCriterio = $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).attr("value");

            GestionFormulaView.Variables.listaConfigDetallesIndicador[variableCriterio] = {
                fuente: $(GestionFormulaView.Controles.form.ddlFuenteIndicador).val(),
                indicador: $(GestionFormulaView.Controles.form.ddlIndicador).val(),
                codigoIndicador: GestionFormulaView.Variables.codigoIndicadorSeleccionado,
                variableDatoCriterio: variableCriterio,
                nombreVariable: $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).children().eq(1).text(),
                categoria: $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).val(),
                detalle: $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlDetalle).val(),
                acumulacion: $(GestionFormulaView.Controles.form.ddlAcumulacion).val(),
                valorTotal: pEsValorTotal
            };
        },

        EliminarObjDetalleModalDetalle: function () {
            let variableCriterio = $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).attr("value");
            delete GestionFormulaView.Variables.listaConfigDetallesIndicador[variableCriterio];
        },

        GuardarDetalleModalDetalle: function () {
            let fuenteIndicador = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            if (this.ValidarComboBoxesModalDetalle(fuenteIndicador)) {
                this.AgregarObjDetalleModalDetalle();
                this.ActivarDesactivarBtnAbrirModalDetalle(false);

                jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoDetalleAgregado)
                    .set('onok', function (closeEvent) {
                        $(GestionFormulaView.Controles.modalDetalleAgregacion.modal).modal('hide');
                    });
            }
        },

        EliminarDetalleModalDetalle: function () {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminarDetalle, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(data => {
                    this.EliminarObjDetalleModalDetalle();
                    this.ActivarDesactivarBtnAbrirModalDetalle(true);
                })
                .then(_ => {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoEliminarDetalle)
                        .set('onok', function (closeEvent) {
                            $(GestionFormulaView.Controles.modalDetalleAgregacion.modal).modal('hide');
                        });
                });
        },

        AbrirModalDefinicionFechas: function (pEsModoCreacion) {
            $(GestionFormulaView.Controles.modalFechaCalculo.btnEliminar).css("display", pEsModoCreacion ? "none" : "initial");
            $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('show');
        },

        // Modal Definición de Fechas
        InsertarDatosEnComboBoxTipoFecha: function (pData, pCombobox) {
            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push(
                    {
                        value: item.id, text: item.Nombre,
                        extraParameters: [
                            { attr: GestionFormulaView.Variables.attrIdentificador, value: item.IdTipoFecha }
                        ]
                    });
            });

            InsertarDataSetSelect2(pCombobox, dataSet, false);
            $(pCombobox).val("");
        },

        InsertarDatosEnComboBoxCategoriasTipoFecha: function (pData, pComboCategorias) {
            $(pComboCategorias).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreCategoria });
            });

            InsertarDataSetSelect2(pComboCategorias, dataSet, false);
            SeleccionarItemSelect2(pComboCategorias, "");
        },

        CargarModalDefinicionFechas: function (pFuente) {
            if (pFuente == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                let pIdIndicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();
                let objFechas = GestionFormulaView.Variables.listaConfigDefinicionFechas[pIdIndicador];

                if (objFechas != undefined && objFechas != null) { // modo edición
                    this.CargarComboboxesModalFechasModoEdicion(objFechas);
                }
                else {
                    this.RestablecerCamposModalFecha();
                    GestionFormulaView.Metodos.MostrarComboboxesModalFecha("", "");
                    this.CargarComboboxesModalFechasModoCreacion();
                }   
            }
        },

        CargarComboboxesModalFechasModoEdicion: function (pObjFechas) {
            $("#loading").fadeIn();
            let controles = GestionFormulaView.Controles.modalFechaCalculo;
            let enumFechas = GestionFormulaView.Variables.DefinionFecha;
            let tieneCategorioInicio, tieneCategoriaFinal = false;

            // the radio buttons
            $(`input[name='${controles.radioGroupName}'][value='${pObjFechas.unidadMedida}']`).prop("checked", true);

            SeleccionarItemSelect2(controles.ddlTipoFechaInicio, pObjFechas.tipoFechaInicio);
            SeleccionarItemSelect2(controles.ddlTipoFechaFinal, pObjFechas.tipoFechaFinal);

            let tipoFechaInicio = pObjFechas.idTipoFechaInicio;
            let tipoFechaFinal = pObjFechas.idTipoFechaFinal;

            if (tipoFechaInicio == enumFechas.fecha) {
                $(controles.txtFechaInicio).val(pObjFechas.fechaInicio);
            }
            else if (tipoFechaInicio == enumFechas.categoria) {
                tieneCategorioInicio = true;
            }
            if (tipoFechaFinal == enumFechas.fecha) {
                $(controles.txtFechaFinal).val(pObjFechas.fechaFinal);
            }
            else if (tipoFechaFinal == enumFechas.categoria) {
                tieneCategoriaFinal = true;
            }

            if (tieneCategorioInicio || tieneCategoriaFinal) { // se necesitan cargar las categorias
                let idIndicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();

                GestionFormulaView.Consultas.ConsultarCategoriasDesagregacionTipoFechaDeIndicador(idIndicador)
                    .then(data => {
                        if (tieneCategorioInicio) {
                            GestionFormulaView.Metodos.InsertarDatosEnComboBoxCategoriasTipoFecha(data, controles.ddlCategoriasFechaInicio, pObjFechas.categoriaInicio);
                        }
                        if (tieneCategoriaFinal) {
                            GestionFormulaView.Metodos.InsertarDatosEnComboBoxCategoriasTipoFecha(data, controles.ddlCategoriasFechaFinal, pObjFechas.categoriaFinal);
                        }
                        return data;
                    })
                    .then(_ => {
                        if (tieneCategorioInicio) {
                            SeleccionarItemSelect2(controles.ddlCategoriasFechaInicio, pObjFechas.categoriaInicio);
                        }
                        if (tieneCategoriaFinal) {
                            SeleccionarItemSelect2(controles.ddlCategoriasFechaFinal, pObjFechas.categoriaFinal);
                        }
                    })
                    .then(_ => {
                        GestionFormulaView.Metodos.MostrarComboboxesModalFecha(pObjFechas.idTipoFechaInicio, pObjFechas.idTipoFechaFinal);
                    })
                    .then(_ => {
                        setTimeout(() => {
                            this.AbrirModalDefinicionFechas(false);
                        }, 100); // un pequeño delay para establecer los valores
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
            else {
                $("#loading").fadeOut();
                this.MostrarComboboxesModalFecha(pObjFechas.idTipoFechaInicio, pObjFechas.idTipoFechaFinal);
                this.AbrirModalDefinicionFechas(false);
            }
        },

        CargarComboboxesModalFechasModoCreacion: function () {
            if (!GestionFormulaView.Variables.cargoTiposFechasModalDeficionFechas) {
                $("#loading").fadeIn();

                GestionFormulaView.Consultas.ConsultarTiposFechas()
                    .then(data => {
                        GestionFormulaView.Metodos.InsertarDatosEnComboBoxTipoFecha(data, GestionFormulaView.Controles.modalFechaCalculo.ddlTipoFechaInicio);
                        GestionFormulaView.Metodos.InsertarDatosEnComboBoxTipoFecha(data, GestionFormulaView.Controles.modalFechaCalculo.ddlTipoFechaFinal);
                        GestionFormulaView.Variables.cargoTiposFechasModalDeficionFechas = true;
                    })
                    .then(_ => {
                        GestionFormulaView.Metodos.AbrirModalDefinicionFechas(true);
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
            else {
                GestionFormulaView.Metodos.AbrirModalDefinicionFechas(true);
            }
        },

        RestablecerCamposModalFecha: function () {
            let controles = GestionFormulaView.Controles.modalFechaCalculo;
            SeleccionarItemSelect2(controles.ddlTipoFechaInicio, "");
            SeleccionarItemSelect2(controles.ddlTipoFechaFinal, "");
            $(controles.txtFechaInicio).val("");
            $(controles.txtFechaFinal).val("");
            SeleccionarItemSelect2(controles.ddlCategoriasFechaInicio, "");
            SeleccionarItemSelect2(controles.ddlCategoriasFechaFinal, "");
            $(controles.radioDias).prop("checked", true);
        },

        MostrarComboboxesModalFecha: function (pTipoFechaInicio, pTipoFechaFinal) {
            this.ActivarDesactivarComboxesModalFecha(
                pTipoFechaInicio,
                GestionFormulaView.Controles.modalFechaCalculo.divFechaInicio,
                GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaInicio
            );

            this.ActivarDesactivarComboxesModalFecha(
                pTipoFechaFinal,
                GestionFormulaView.Controles.modalFechaCalculo.divFechaFinal,
                GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaFinal
            );
        },

        ActivarDesactivarComboxesModalFecha: function (pTipoFecha, pDivFecha, pDivCategoria) {
            if (pTipoFecha == GestionFormulaView.Variables.DefinionFecha.fecha) {
                $(pDivFecha).removeClass("hidden");
                $(pDivCategoria).addClass("hidden");
            }
            else if (pTipoFecha == GestionFormulaView.Variables.DefinionFecha.categoria) {
                $(pDivFecha).addClass("hidden");
                $(pDivCategoria).removeClass("hidden");
            }
            else {
                $(pDivFecha).addClass("hidden");
                $(pDivCategoria).addClass("hidden");
            }
        },

        CargarComboboxCategoriasTipoFecha: function (pComboboxCategorias) {
            let fuente = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            if (fuente == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                let idIndicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();

                $("#loading").fadeIn();

                GestionFormulaView.Consultas.ConsultarCategoriasDesagregacionTipoFechaDeIndicador(idIndicador)
                    .then(data => {
                        GestionFormulaView.Metodos.InsertarDatosEnComboBoxCategoriasTipoFecha(data, pComboboxCategorias);
                    })
                    .catch(error => { ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
        },

        LimpiarMensajesValidacionModalFecha: function () {
            let prefijoHelp = GestionFormulaView.Controles.prefijoLabelsHelp;
            let controles = GestionFormulaView.Controles.modalFechaCalculo;

            $(controles.ddlTipoFechaInicio).parent().removeClass("has-error");
            $("#" + $(controles.ddlTipoFechaInicio).attr("id") + prefijoHelp).css("display", "none");
            $(controles.txtFechaInicioModalDefinicion).parent().removeClass("has-error");
            $("#" + $(controles.txtFechaInicioModalDefinicion).attr("id") + prefijoHelp).css("display", "none");
            $(controles.ddlCategoriasFechaInicio).parent().removeClass("has-error");
            $("#" + $(controles.ddlCategoriasFechaInicio).attr("id") + prefijoHelp).css("display", "none");

            $(controles.ddlTipoFechaFinal).parent().removeClass("has-error");
            $("#" + $(controles.ddlTipoFechaFinal).attr("id") + prefijoHelp).css("display", "none");
            $(controles.txtFechaFinal).parent().removeClass("has-error");
            $("#" + $(controles.txtFechaFinal).attr("id") + prefijoHelp).css("display", "none");
            $(controles.ddlCategoriasFechaFinal).parent().removeClass("has-error");
            $("#" + $(controles.ddlCategoriasFechaFinal).attr("id") + prefijoHelp).css("display", "none");
        },

        AgregarObjConfigDefinicionFechas: function () {
            let controles = GestionFormulaView.Controles.modalFechaCalculo;
            let indicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();

            GestionFormulaView.Variables.listaConfigDefinicionFechas[indicador] = {
                indicador: indicador,
                unidadMedida: $(`input[name=${controles.radioGroupName}]:checked`).val(),
                idTipoFechaInicio: $(controles.ddlTipoFechaInicio).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador),
                tipoFechaInicio: $(controles.ddlTipoFechaInicio).val(),
                fechaInicio: $(controles.txtFechaInicio).val(),
                categoriaInicio: $(controles.ddlCategoriasFechaInicio).val(),
                nombreCategoriaInicio: $(controles.ddlCategoriasFechaInicio).text(),
                idTipoFechaFinal: $(controles.ddlTipoFechaFinal).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador),
                tipoFechaFinal: $(controles.ddlTipoFechaFinal).val(),
                fechaFinal: $(controles.txtFechaFinal).val(),
                categoriaFinal: $(controles.ddlCategoriasFechaFinal).val(),
                nombreCategoriaFinal: $(controles.ddlCategoriasFechaFinal).text()
            };
        },

        ValidarFormularioModalDefinicionFechas: function () {
            let controles = GestionFormulaView.Controles.modalFechaCalculo;
            let prefijoHelp = GestionFormulaView.Controles.prefijoLabelsHelp;
            let puedeContinuar = true;

            this.LimpiarMensajesValidacionModalFecha();

            let tipoFechaInicio = $(controles.ddlTipoFechaInicio).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);
            let fechaInicio = $(controles.txtFechaInicio).val();
            let categoriaInicio = $(controles.ddlCategoriasFechaInicio).val();

            let tipoFechaFinal = $(controles.ddlTipoFechaFinal).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);;
            let fechaFinal = $(controles.txtFechaFinal).val();
            let categoriaFinal = $(controles.ddlCategoriasFechaFinal).val();

            // tipo fecha inicio
            if (tipoFechaInicio == "" || tipoFechaInicio == null) {
                $(controles.ddlTipoFechaInicio + prefijoHelp).css("display", "block");
                $(controles.ddlTipoFechaInicio).parent().addClass("has-error");
                puedeContinuar = false;
            }

            if (tipoFechaInicio == GestionFormulaView.Variables.DefinionFecha.fecha) {
                if (fechaInicio == "" || fechaInicio == null) {
                    $(controles.txtFechaInicio + prefijoHelp).css("display", "block");
                    $(controles.txtFechaInicio).parent().addClass("has-error");
                    puedeContinuar = false;
                }
            }
            else if (tipoFechaInicio == GestionFormulaView.Variables.DefinionFecha.categoria) {
                if (categoriaInicio == "" || categoriaInicio == null) {
                    $(controles.ddlCategoriasFechaInicio + prefijoHelp).css("display", "block");
                    $(controles.ddlCategoriasFechaInicio).parent().addClass("has-error");
                    puedeContinuar = false;
                }
            }

            // tipo fecha final
            if (tipoFechaFinal == "" || tipoFechaFinal == null) {
                $(controles.ddlTipoFechaFinal + prefijoHelp).css("display", "block");
                $(controles.ddlTipoFechaFinal).parent().addClass("has-error");
                puedeContinuar = false;
            }

            if (tipoFechaFinal == GestionFormulaView.Variables.DefinionFecha.fecha) {
                if (fechaFinal == "" || fechaFinal == null) {
                    $(controles.txtFechaFinal + prefijoHelp).css("display", "block");
                    $(controles.txtFechaFinal).parent().addClass("has-error");
                    puedeContinuar = false;
                }
            }
            else if (tipoFechaFinal == GestionFormulaView.Variables.DefinionFecha.categoria) {
                if (categoriaFinal == "" || categoriaFinal == null) {
                    $(controles.ddlCategoriasFechaFinal + prefijoHelp).css("display", "block");
                    $(controles.ddlCategoriasFechaFinal).parent().addClass("has-error");
                    puedeContinuar = false;
                }
            }

            return puedeContinuar;
        },

        EliminarObjDefinicionModalFecha: function () {
            let index = GestionFormulaView.Metodos.ObtenerPosicionCursor();
            let indicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();
            delete GestionFormulaView.Variables.listaConfigDefinicionFechas[indicador];

            for (let i = 0; i < GestionFormulaView.Variables.FormulaCalculo.length; i++) {
                let item = GestionFormulaView.Variables.FormulaCalculo[i];
                if (item.tipoObjeto == GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Variable && item.tipoArgumento == GestionFormulaView.Variables.TipoArgumento.fecha) {
                    if (item.argumento.indicador == indicador) {
                        GestionFormulaView.Variables.FormulaCalculo.splice(i, 1);
                        break;
                    }
                }
            }

            GestionFormulaView.Metodos.MostrarFormulaCalculo();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).focus();
            $(GestionFormulaView.Controles.form.inputFormulaCalculo).setCursorPosition(index);
        },

        GuardarDefinicionDeFechas: function () {
            if (this.ValidarFormularioModalDefinicionFechas()) {
                this.AgregarObjConfigDefinicionFechas();
                let argumento = this.ConstruirArgumento(GestionFormulaView.Variables.TipoArgumento.fecha);

                if (argumento) {
                    this.AgregarFechaAFormula(argumento);

                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoArgumentoFechaCreado)
                        .set('onok', function (closeEvent) {
                            $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('hide');
                        });
                }

            }
        },

        EliminarDefinicionDeFechas: function () {
            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEliminarArgumentoFecha, jsMensajes.Variables.actionType.eliminar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    });
            })
                .then(_ => {
                    GestionFormulaView.Metodos.EliminarObjDefinicionModalFecha();
                })
                .then(_ => {
                    return new Promise((resolve, reject) => { // esperar la respuesta para proceder con el restablecimiento del formulario
                        jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoEliminarArgumentoFecha)
                            .set('onok', function (closeEvent) {
                                $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('hide');
                                resolve(true);
                            });
                    });
                })
                .then(_ => {
                    setTimeout(() => {
                        GestionFormulaView.Metodos.MostrarComboboxesModalFecha("", "");
                        GestionFormulaView.Metodos.LimpiarMensajesValidacionModalFecha();
                        GestionFormulaView.Metodos.RestablecerCamposModalFecha();
                    }, 150);
                })
                .catch(error => { ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
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

        ConsultarCategoriasDesagregacionDeIndicador: function (pIdIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerCategoriasDesagregacionDeIndicador", "GET", { pIdIndicador });
        },

        ConsultarCategoriasDesagregacionTipoFechaDeIndicador: function (pIdIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerCategoriasDesagregacionTipoFechaDeIndicador", "GET", { pIdIndicador });
        },

        ConsultarVariablesDatoCriteriosIndicador: function (pIdIndicador, pFuenteIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerVariablesDatoCriteriosIndicador", "GET", { pIdIndicador, pFuenteIndicador });
        },

        ConsultarListaDetallesDeCategoria: function (pIdIndicador, pIdCategoria) {
            return execAjaxCall("/FormulaCalculo/ObtenerListaDetallesDeCategoria", "GET", { pIdIndicador, pIdCategoria });
        },

        ConsultarTiposFechas: function () {
            return execAjaxCall("/FormulaCalculo/ObtenerTiposFechasDefinicion", "GET");
        },

        CrearDetallesFormulaCalculo: function (pFormulaCalculo, pListaArgumentos) {
            return execAjaxCall("/FormulaCalculo/CrearDetallesFormulaCalculo", "POST", { pFormulaCalculo, pListaArgumentos });
        },
    },

    Eventos: function () {
        $(document).on("click", GestionFormulaView.Controles.step2, function () {
            if (!GestionFormulaView.Variables.cargoFuentesIndicador) {
                GestionFormulaView.Metodos.CargarDatosStep2();
            }
        });

        $(GestionFormulaView.Controles.form.ddlFuenteIndicador).on('select2:select', function () {
            let fuenteSeleccionada = $(this).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            GestionFormulaView.Metodos.HabilitarComboboxddlFuenteIndicador(fuenteSeleccionada);
            GestionFormulaView.Metodos.InsertarDatosTablaDetallesIndicador([]); // reutilizar el método para limpiar la tabla

            if (fuenteSeleccionada == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                GestionFormulaView.Metodos.CargarCatalogosParaFuenteIndicadorFonatel();
            }
            else if (fuenteSeleccionada == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGM
                || fuenteSeleccionada == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGC) {
                GestionFormulaView.Metodos.CargarCatalogosParaFuenteIndicadorFueraDeFonatel(fuenteSeleccionada);
            }
            //else if (fuenteSeleccionada == GestionFormulaView.Variables.FuenteIndicador.IndicadorFuenteExterna) {
            //    GestionFormulaView.Metodos.CargarDatosIndicador(fuenteSeleccionada);
            //}
            
        });

        $(GestionFormulaView.Controles.form.ddlGrupo).on('select2:select', function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(GestionFormulaView.Controles.form.ddlServicio).on('select2:select', function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(GestionFormulaView.Controles.form.ddlTipoIndicador).on('select2:select', function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(GestionFormulaView.Controles.form.ddlClasificacion).on('select2:select', function () {
            GestionFormulaView.Metodos.SeleccionarCargaDeDatosIndicador();
        });

        $(GestionFormulaView.Controles.form.ddlIndicador).on('select2:select', function (e) {
            GestionFormulaView.Variables.codigoIndicadorSeleccionado = $(this).find(":selected").attr(GestionFormulaView.Variables.attrCodigo);
            GestionFormulaView.Metodos.CargarTablaDetallesIndicador($(this).val());
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion, function () {
            GestionFormulaView.Variables.filaSeleccionadaTablaDetalles = $(this).parent().parent();
            GestionFormulaView.Metodos.CargarModalDetallesIndicador();
        });

        $(document).on("change", GestionFormulaView.Controles.form.chkValorTotal, function () {
            GestionFormulaView.Variables.filaSeleccionadaTablaDetalles = $(this).parent().parent();
            let isChecked = $(this).is(':checked');
            $(GestionFormulaView.Variables.filaSeleccionadaTablaDetalles).find(GestionFormulaView.Controles.form.btnAgregarDetalleAgregacion).prop("disabled", isChecked);

            if (isChecked) {
                GestionFormulaView.Metodos.AgregarObjDetalleModalDetalle(true);
            }
            else {
                GestionFormulaView.Metodos.EliminarObjDetalleModalDetalle();
            }
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnAgregarArgumento, function () {
            let acumulacion = $(GestionFormulaView.Controles.form.ddlAcumulacion).val();
            let fuente = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            if (fuente == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGF) {
                if (acumulacion != "" && acumulacion != null) {
                    GestionFormulaView.Metodos.RegistrarCriterio(this);
                }
            }
            else if (fuente == GestionFormulaView.Variables.FuenteIndicador.IndicadorDGC) {
                // como no tenemos opcion de selecionar Valor Total o Detalle de Agrupación, por default debe ser Valor Total
                GestionFormulaView.Variables.filaSeleccionadaTablaDetalles = $(this).parent().parent();
                GestionFormulaView.Metodos.AgregarObjDetalleModalDetalle(true);
                GestionFormulaView.Metodos.RegistrarCriterio(this);
            }
            else {
                GestionFormulaView.Metodos.RegistrarCriterio(this);
            }

        });

        // Modal detalle desagregación/agrupación
        $(GestionFormulaView.Controles.modalDetalleAgregacion.ddlCategoria).on('select2:select', function () {
            GestionFormulaView.Metodos.CargarComboBoxDetallesModalDetalle($(this).val());
        });

        $(document).on("click", GestionFormulaView.Controles.modalDetalleAgregacion.btnGuardar, function () {
            GestionFormulaView.Metodos.GuardarDetalleModalDetalle();
        });

        $(document).on("click", GestionFormulaView.Controles.modalDetalleAgregacion.btnEliminar, function () {
            GestionFormulaView.Metodos.EliminarDetalleModalDetalle();
        });

        // Modal definición de fechas
        $(GestionFormulaView.Controles.modalFechaCalculo.ddlTipoFechaInicio).on('select2:select', function () {
            let tipoSeleccionado = $(this).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            GestionFormulaView.Metodos.ActivarDesactivarComboxesModalFecha(
                tipoSeleccionado,
                GestionFormulaView.Controles.modalFechaCalculo.divFechaInicio,
                GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaInicio
            );

            if (tipoSeleccionado == GestionFormulaView.Variables.DefinionFecha.categoria) {
                GestionFormulaView.Metodos.CargarComboboxCategoriasTipoFecha(GestionFormulaView.Controles.modalFechaCalculo.ddlCategoriasFechaInicio);
            }
        });

        $(GestionFormulaView.Controles.modalFechaCalculo.ddlTipoFechaFinal).on('select2:select', function () {
            let tipoSeleccionado = $(this).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            GestionFormulaView.Metodos.ActivarDesactivarComboxesModalFecha(
                tipoSeleccionado,
                GestionFormulaView.Controles.modalFechaCalculo.divFechaFinal,
                GestionFormulaView.Controles.modalFechaCalculo.divCategoriaFechaFinal
            );

            if (tipoSeleccionado == GestionFormulaView.Variables.DefinionFecha.categoria) {
                GestionFormulaView.Metodos.CargarComboboxCategoriasTipoFecha(GestionFormulaView.Controles.modalFechaCalculo.ddlCategoriasFechaFinal);
            }
        });

        $(document).on("click", GestionFormulaView.Controles.modalFechaCalculo.btnCancelar, function (e) {
            $(GestionFormulaView.Controles.modalFechaCalculo.modal).modal('hide');
        });

        $(document).on("click", GestionFormulaView.Controles.modalFechaCalculo.btnGuardar, function () {
            GestionFormulaView.Metodos.GuardarDefinicionDeFechas();
        });

        $(document).on("click", GestionFormulaView.Controles.modalFechaCalculo.btnEliminar, function () {
            GestionFormulaView.Metodos.EliminarDefinicionDeFechas();
        });

        // Tabla creación de fórmula
        $(document).on("keypress", GestionFormulaView.Controles.form.inputFormulaCalculo, function (event) {
            let keyCode = event.keyCode || event.which;
            let regex = GestionFormulaView.Variables.Operadores.GetRegex();
            let char = String.fromCharCode(keyCode);
            let index = event.target.selectionStart;
            
            if (keyCode >= 48 && keyCode <= 57) {
                let newIndex = GestionFormulaView.Metodos.AniadirElementoAFormula({ tipoObjeto: GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Numero, argumento: char }, (index + 1))
                $(GestionFormulaView.Controles.form.inputFormulaCalculo).setCursorPosition(newIndex);
                GestionFormulaView.Metodos.MostrarFormulaCalculo();
            }
            else if (regex.test(char)) {
                let newIndex = GestionFormulaView.Metodos.AniadirElementoAFormula({ tipoObjeto: GestionFormulaView.Variables.TipoObjetoFormulaCalculo.Operador, argumento: char }, (index + 1))
                $(GestionFormulaView.Controles.form.inputFormulaCalculo).setCursorPosition(newIndex);
                GestionFormulaView.Metodos.MostrarFormulaCalculo();
            }
            return false
        });

        $(document).on("keydown", GestionFormulaView.Controles.form.inputFormulaCalculo, function (event) {
            let index = event.target.selectionStart;
            let keyCode = event.keyCode || event.which;

            if (keyCode == 46) { // suppress key
                event.preventDefault();
            }

            if (keyCode == 8) { // backspace key
                let newIndex = GestionFormulaView.Metodos.BorrarOperadorAFormula(index);
                $(GestionFormulaView.Controles.form.inputFormulaCalculo).setCursorPosition(newIndex);
                GestionFormulaView.Metodos.MostrarFormulaCalculo();
                return false
            }
        })

        $(document).on("change", GestionFormulaView.Controles.form.inputFormulaCalculo, function () {
            $(this).val($(this).val().replace(/\s+/g, ' ').trim());
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnCalendario, function (e) {
            let idIndicador = $(GestionFormulaView.Controles.form.ddlIndicador).val();
            let fuente = $(GestionFormulaView.Controles.form.ddlFuenteIndicador).find(":selected").attr(GestionFormulaView.Variables.attrIdentificador);

            if (idIndicador != null && idIndicador != "" && fuente != null && fuente != "") {
                GestionFormulaView.Metodos.CargarModalDefinicionFechas(fuente);
            }
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnSumar, function (e) {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.sumar);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnRestar, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.restar);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnMultiplicar, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.multiplicar);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnDividir, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.dividir);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnMenorQue, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.menorQue);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnMayorQue, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.mayorQue);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnMenorIgualQue, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.menorIgual);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnMayorIgualQue, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.mayorIgual);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnIgualQue, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.igual);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnParentesisAbrierto, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.abrirParentesis);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnParentesisCerrado, function () {
            GestionFormulaView.Metodos.BotonAgregarOperadorFormula(GestionFormulaView.Variables.Operadores.cerrarParentesis);
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnRemoverItemFormula, function () {
            GestionFormulaView.Metodos.BotonRemoverOperadorFormula();
        });

        // Acciones de la pantalla

        $(document).on("click", GestionFormulaView.Controles.form.btnGuardar, function (e) {
            if (ObtenerValorParametroUrl("id") != null) {
                GestionFormulaView.Metodos.CrearFormulaGuardadoParcial();
            }
        });

        // | Eventos por probar y rehacer   |
        // |                                |
        // V                                V


        $(document).on("click", GestionFormulaView.Controles.form.btnFinalizar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaAgregarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaAgregada)
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

        $(document).on("click", GestionFormulaView.Controles.form.btnEjecutarFormula, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(GestionFormulaView.Mensajes.preguntaEjecutarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(GestionFormulaView.Mensajes.exitoFormulaEjecutada)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", GestionFormulaView.Controles.form.btnAtras, function (e) {
            $(GestionFormulaView.Controles.step1).trigger('click');
        });
    },

    CustomJQueryFunctions: function () {
        $.fn.setCursorPosition = function (pos) {
            this.each(function (index, elem) {
                if (elem.setSelectionRange) {
                    window.setTimeout(function () {
                        elem.setSelectionRange(pos, pos);
                    }, 0);
                } else if (elem.createTextRange) {
                    var range = elem.createTextRange();
                    range.collapse(true);
                    range.moveEnd('character', pos);
                    range.moveStart('character', pos);
                    range.select();
                }
            });
            return this;
        };
    },

    Init: function () {
        GestionFormulaView.Eventos();
        GestionFormulaView.CustomJQueryFunctions();
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