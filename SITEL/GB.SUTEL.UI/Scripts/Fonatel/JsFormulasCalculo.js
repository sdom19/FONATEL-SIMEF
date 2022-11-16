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
        ManejoDeExcepciones: function (pError) {
            if (pError?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                jsMensajes.Metodos.OkAlertErrorModal(pError.MensajeError).set('onok', function (closeEvent) { });
            }
            else {
                jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
            }
        },

        EliminarFormulaCalculo: function (idFormula) {
            $("#loading").fadeIn();

            IndexView.Consultas.EliminarFormula({ id: idFormula })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoEliminarFormula)
                        .set('onok', function (closeEvent) {
                            window.location.href = IndexView.Variables.indexViewURL;
                        });
                }).catch((obj) => {
                    this.ManejoDeExcepciones(null);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        ActivarFormulaCalculo: function (idFormula) {
            $("#loading").fadeIn();

            IndexView.Consultas.ActivarFormula({ id: idFormula })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoActivarFormula)
                        .set('onok', function (closeEvent) {
                            window.location.href = IndexView.Variables.indexViewURL;
                        });
                }).catch((obj) => {
                    this.ManejoDeExcepciones(null);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        DesactivarFormulaCalculo: function (idFormula) {
            $("#loading").fadeIn();

            IndexView.Consultas.DesactivarFormula({ id: idFormula })
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal(IndexView.Mensajes.exitoDesactivarFormula)
                        .set('onok', function (closeEvent) {
                            window.location.href = IndexView.Variables.indexViewURL;
                        });
                }).catch((obj) => {
                    this.ManejoDeExcepciones(null);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        CargarTablaFormulas: function () {
            $("#loading").fadeIn();

            IndexView.Consultas.ObtenerListaFormulas()
                .then(obj => {
                    this.InsertarDatosTablaFormulas(obj.objetoRespuesta);
                }).catch(error => {
                    this.ManejoDeExcepciones(null);
                }).finally(() => {
                    $("#loading").fadeOut();
                });
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
            return execAjaxCall("/FormulaCalculo/DesactivarFormula", "POST", { formulaCalculo: pFormulaCalculo });
        },

        ActivarFormula: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/ActivarFormula", "POST", { formulaCalculo: pFormulaCalculo });
        },

        EliminarFormula: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/EliminarFormula", "POST", { formulaCalculo: pFormulaCalculo });
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

CreateView = {
    Controles: {
        modoFormulario: "#modoFormulario",

        ddlFuenteIndicador: "#ddlFuenteIndicador",
        ddlGrupoFonatel: "#ddlGrupoFonatel",
        dllServicio: "#dllServicio",
        ddlTipoFonatel: "#ddlTipoFonatel",
        ddlIndicador: "#ddlIndicador",
        btnAgregarDetalleAgregacion: "#btnAgregarDetalleAgregacion",
        btnEliminarDetalleAgregacion: "#btnEliminarDetalleAgregacion",
        //"divInputDetalleCategoríaDesagregacion": "#divInputDetalleCategoríaDesagregacion",
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
        //ELIMINADO: ControlesStep1: "#formCrearFormula input, #formCrearFormula textarea, #formCrearFormula select",
        step2: "a[href='#step-2']",
        divStep2: "#step-2 input, #step-2 select, #step-2 button",

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

            bnEditar: "#btnGuardarEditarFormulaCalculo",
            btnSiguienteEditar: "#btnSiguienteEditarFormulaCalculo",

            btnClonar: "#btnGuardarClonarFormulaCalculo",
            btnSiguienteClonar: "#btnSiguienteClonarFormulaCalculo",

            btnCancelar: "#btnCancelarFormula",
        },

        // Formulario Agregar - Paso 2
        formAgregarFormula: {

        },

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
        indexViewURL: "/Fonatel/FormulaCalculo/Index",

        Direccion: {
            FONATEL: 1,
            MERCADOS: 2,
            CALIDAD: 3
        },
        FECHAS: {
            ACTUAL: "3",
            Categoría: "2",
            FECHA: "1"
        },
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
        ManejoDeExcepciones: function (pError) {
            if (pError?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
                jsMensajes.Metodos.OkAlertErrorModal(pError.MensajeError).set('onok', function (closeEvent) { });
            }
            else {
                jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
            }
        },

        // Formulario crear fórmula
        CargarDatosDependientesDeIndicador: function (pIdIndicador, pCallback = null) { // variables dato y categorias de desagregación
            $("#loading").fadeIn();

            CreateView.Consultas.ConsultarVariablesDatoDeIndicador(pIdIndicador)
                .then(data => {
                    this.InsertarDatosEnComboVariablesDato(data);
                    return true;
                })
                .then(data => {
                    if ($(CreateView.Controles.formCrearFormula.radioCategoriaDesagregacion).is(':checked')) {
                        return CreateView.Consultas.ConsultarCategoriasDesagregacionDeIndicador(pIdIndicador)
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
                .catch(error => {
                    console.log(error);
                    this.ManejoDeExcepciones(error); })
                .finally(() => { $("#loading").fadeOut(); });
        },

        CargarCategoriasDesagregacionDeIndicador: function (pIdIndicador) {
            $("#loading").fadeIn();

            CreateView.Consultas.ConsultarCategoriasDesagregacionDeIndicador(pIdIndicador)
                .then(data => { this.InsertarDatosEnComboBoxCategorias(data); })
                .catch(error => { this.ManejoDeExcepciones(error); })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        InsertarDatosEnComboVariablesDato: function (pData) {
            $(CreateView.Controles.formCrearFormula.ddlVariableDatoFormula).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreVariable });
            });
            InsertarDataSetSelect2(CreateView.Controles.formCrearFormula.ddlVariableDatoFormula, dataSet);
            $(CreateView.Controles.formCrearFormula.ddlVariableDatoFormula).val("");
        },

        InsertarDatosEnComboBoxCategorias: function (pData) {
            $(CreateView.Controles.formCrearFormula.ddlCategoriaDesagregacion).empty();

            let dataSet = []
            pData.objetoRespuesta?.forEach(item => {
                dataSet.push({ value: item.id, text: item.NombreCategoria });
            });

            if (dataSet.length > 0) {
                InsertarOpcionTodosSelect2Multiple(CreateView.Controles.formCrearFormula.ddlCategoriaDesagregacion);
            }

            InsertarDataSetSelect2(CreateView.Controles.formCrearFormula.ddlCategoriaDesagregacion, dataSet);
        },

        CrearObjFormularioCrearFormula: function (pEsGuardadoParcial) {
            let controles = CreateView.Controles.formCrearFormula;

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

            if ($(CreateView.Controles.formCrearFormula.radioTotal).is(':checked')) {
                excepcionesForm.push(CreateView.Controles.formCrearFormula.ddlCategoriaDesagregacion.slice(1));
            }
            return excepcionesForm;
        },

        VerificarCamposIncompletosFormularioCrearFormula: function (pEsGuardadoParcial) {
            let prefijoHelp = CreateView.Controles.prefijoLabelsHelp;
            let camposObligatoriosGuardadoParcial = true;
            let controles = CreateView.Controles.formCrearFormula;

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
            $(CreateView.Controles.formCrearFormula.btnSiguienteCrear).prop('disabled', pDesactivar);
            $(CreateView.Controles.formCrearFormula.btnSiguienteEditar).prop('disabled', pDesactivar);
            $(CreateView.Controles.formCrearFormula.btnSiguienteClonar).prop('disabled', pDesactivar);
        },

        EventosEnInputsFormularioCrearFormulaCalculo: function () {
            let validacion = ValidarFormulario(
                CreateView.Controles.formCrearFormula.inputs,
                CreateView.Metodos.InputExcepcionesFormularioCrearFormula()
            );
            CreateView.Metodos.CambiarEstadoBtnSiguienteFormCrearFormula(!validacion.puedeContinuar);
        },

        CrearFormulaCalculo: function () {
            let validacionFormulario = ValidarFormulario(
                CreateView.Controles.formCrearFormula.inputs,
                this.InputExcepcionesFormularioCrearFormula()
            );

            if (validacionFormulario.puedeContinuar) {
                $("#loading").fadeIn();
                CreateView.Consultas.CrearFormulaCalculo(this.CrearObjFormularioCrearFormula(false))
                    .then(data => {
                        InsertarParametroUrl("id", data.objetoRespuesta[0].id);
                        $(CreateView.Controles.step2).trigger('click'); // cargar los respectivos datos
                    })
                    .catch(error => { this.ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
        },

        CrearFormulaGuardadoParcial: function () {
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioCrearFormula(true);

            if (!validacion.guardadoParcial) { return; }

            if (!validacion.guardadoCompleto) {
                mensaje = CreateView.Mensajes.existenCamposRequeridos;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje + CreateView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) {
                        resolve(true);
                    })
                    .set("oncancel", function () {
                        CreateView.Metodos.VerificarCamposIncompletosFormularioCrearFormula(false);
                    })
            })
            .then(data => {
                $("#loading").fadeIn();
                return CreateView.Consultas.CrearFormulaCalculo(this.CrearObjFormularioCrearFormula(true));
            })
            .then(data => {
                jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoFormulaCreada)
                    .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL; });
            })
            .catch(error => { this.ManejoDeExcepciones(error); })
            .finally(() => {  $("#loading").fadeOut(); });
        },

        EditarFormulaCalculo: function () {
            let validacionFormulario = ValidarFormulario(
                CreateView.Controles.formCrearFormula.inputs,
                this.InputExcepcionesFormularioCrearFormula()
            );

            if (validacionFormulario.puedeContinuar) {
                $("#loading").fadeIn();
                CreateView.Consultas.EditarFormulaCalculo(this.CrearObjFormularioCrearFormula(false))
                    .then(data => {
                        setTimeout(() => {
                            $(CreateView.Controles.step2).trigger('click'); // cargar los respectivos datos
                        }, 600);
                    })
                    .catch(error => { this.ManejoDeExcepciones(error); })
                    .finally(() => { $("#loading").fadeOut(); });
            }
        },

        EditarFormulaCalculoGuardadoParcial: function () {
            let mensaje = "";
            let validacion = this.VerificarCamposIncompletosFormularioCrearFormula(true);

            if (!validacion.guardadoParcial) { return; }

            if (!validacion.guardadoCompleto) {
                mensaje = CreateView.Mensajes.existenCamposRequeridos;
            }

            new Promise((resolve, reject) => {
                jsMensajes.Metodos.ConfirmYesOrNoModal(mensaje + CreateView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                    .set('onok', function (closeEvent) { resolve(true); })
                    .set("oncancel", function () {
                        CreateView.Metodos.VerificarCamposIncompletosFormularioCrearFormula(false);
                    })
            })
            .then(data => {
                $("#loading").fadeIn();
                return CreateView.Consultas.EditarFormulaCalculo(this.CrearObjFormularioCrearFormula(true));
            })
            .then(data => {
                jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoFormulaCreada)
                    .set('onok', function (closeEvent) { window.location.href = CreateView.Variables.indexViewURL; });
            })
            .catch(error => { this.ManejoDeExcepciones(error); })
            .finally(() => { $("#loading").fadeOut(); });
        }

        // ---------------
    },

    Consultas: {
        ConsultarVariablesDatoDeIndicador: function (pIdIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerVariablesDatoDeIndicador", "GET", { pIdIndicador });
        },

        ConsultarCategoriasDesagregacionDeIndicador: function (pIdIndicador) {
            return execAjaxCall("/FormulaCalculo/ObtenerCategoriasDesagregacionDeIndicador", "GET", { pIdIndicador });
        },

        CrearFormulaCalculo: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/CrearFormulaCalculo", "POST", { pFormulaCalculo });
        },

        EditarFormulaCalculo: function (pFormulaCalculo) {
            return execAjaxCall("/FormulaCalculo/EditarFormulaCalculo", "POST", { pFormulaCalculo });
        }
    },

    Eventos: function () {
        // Formulario Crear fórmula
        // -- Crear fórmula de cálculo
        $(document).on("click", CreateView.Controles.formCrearFormula.btnGuardar, function (e) {
            CreateView.Metodos.CrearFormulaGuardadoParcial();
        });

        $(document).on("click", CreateView.Controles.formCrearFormula.btnSiguienteCrear, function (e) {
            if (ObtenerValorParametroUrl("id") == null) {
                CreateView.Metodos.CrearFormulaCalculo();
            }
            else {
                CreateView.Metodos.EditarFormulaCalculo();
            }
        });

        // -- Editar fórmula de cálculo
        $(document).on("click", CreateView.Controles.formCrearFormula.bnEditar, function (e) {
            CreateView.Metodos.EditarFormulaCalculoGuardadoParcial();
        });

        $(document).on("click", CreateView.Controles.formCrearFormula.btnSiguienteEditar, function (e) {
            if (ObtenerValorParametroUrl("id") != null) {
                CreateView.Metodos.EditarFormulaCalculo();
            }
        });

        // -- Clonar fórmula de cálculo




        $(CreateView.Controles.formCrearFormula.ddlIndicadorFormulario).on('select2:select', function (event) {
            let idIndicador = $(this).val();
            if (idIndicador != null || $.trim(idIndicador) != "") {
                CreateView.Metodos.CargarDatosDependientesDeIndicador(
                    idIndicador,
                    CreateView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo
                );
            }
        });

        $(document).on("change", CreateView.Controles.formCrearFormula.radioCategoriaDesagregacion, function () {
            $(CreateView.Controles.formCrearFormula.divInputCategoriaDesagregacion).css("display", "block");

            let indicador = $(CreateView.Controles.formCrearFormula.ddlIndicadorFormulario).val();

            if (indicador != null && indicador != "") {
                CreateView.Metodos.CargarCategoriasDesagregacionDeIndicador(indicador);
            }
        });

        $(document).on("change", CreateView.Controles.formCrearFormula.radioTotal, function () {
            $(CreateView.Controles.formCrearFormula.ddlCategoriaDesagregacion).empty();
        });

        $(CreateView.Controles.formCrearFormula.inputs).on("keyup", function (e) {
            CreateView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CreateView.Controles.formCrearFormula.selects2).on('select2:select', function (e) {
            CreateView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CreateView.Controles.formCrearFormula.ddlCategoriaDesagregacion).on('select2:unselect', function (e) {
            RemoverOpcionesSelect2Multiple(e.params.data.text);
            
            CreateView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
        });

        $(CreateView.Controles.formCrearFormula.inputDates + "," +
            CreateView.Controles.formCrearFormula.inputRadios).on('change', function (e) {
                CreateView.Metodos.EventosEnInputsFormularioCrearFormulaCalculo();
            });


        // --------------------------------------

        $(document).on("click", CreateView.Controles.formCrearFormula.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.cancelar)
                .set('onok', function (closeEvent) {
                    window.location.href = CreateView.Variables.indexViewURL;
                });
        });


        $(document).on("change", CreateView.Controles.chkValorTotal, function () {
            if (chkValorTotal.checked) {
                $(CreateView.Controles.btnAgregarDetalleAgregacion).prop("disabled", true);
            }
            else {
                $(CreateView.Controles.btnAgregarDetalleAgregacion).prop("disabled", false);
            }
        });

        $(document).on("change", CreateView.Controles.modalFecha.ddlTipoFechaInicio, function () {
            $(CreateView.Controles.divCategoríaFechaInicioFormulaCalculo).addClass("hidden");
            $(CreateView.Controles.divFechaInicioFormulaCalculo).addClass("hidden");

            let option = $(this).val();

            switch (option) {

                case CreateView.Variables.FECHAS.FECHA:
                    $(CreateView.Controles.divFechaInicioFormulaCalculo).removeClass("hidden");
                    break
                case CreateView.Variables.FECHAS.Categoría:
                    $(CreateView.Controles.divCategoríaFechaInicioFormulaCalculo).removeClass("hidden");
                    break;
                default:
            }
        });

        $(document).on("change", CreateView.Controles.modalFecha.ddlTipoFechaFinal, function () {
            $(CreateView.Controles.divFechaFinalFormulaCalculo).addClass("hidden");
            $(CreateView.Controles.divCategoríaFechaFinalFormulaCalculo).addClass("hidden");
            let option = $(this).val();

            switch (option) {

                case CreateView.Variables.FECHAS.FECHA:
                    $(CreateView.Controles.divFechaFinalFormulaCalculo).removeClass("hidden");
                    break
                case CreateView.Variables.FECHAS.Categoría:
                    $(CreateView.Controles.divCategoríaFechaFinalFormulaCalculo).removeClass("hidden");
                    break;
                default:
            }
        });

        $(document).on("change", CreateView.Controles.ddlFuenteIndicador, function () {
            let optionSelected = $(this).select2('data')[0].id;

            if (optionSelected == "1") {
                $(CreateView.Controles.divGrupo).css("display", "block");
                $(CreateView.Controles.divClasificacion).css("display", "block");
                $(CreateView.Controles.divTipoIndicador).css("display", "block");
                $(CreateView.Controles.divIndicador).css("display", "block");
                $(CreateView.Controles.divServicio).css("display", "none");
                $(CreateView.Controles.divAcumulacion).css("display", "block");

                $(CreateView.Controles.columnaDetalleTabla).html(CreateView.Mensajes.labelDetalleDesagregacion);
                $(CreateView.Controles.modalDetalle.titulo).html(CreateView.Mensajes.labelDetalleDesagregacion);
                $(CreateView.Controles.modalDetalle.divCategoria).css("display", "block");
                $(CreateView.Controles.modalDetalle.divCriterio).css("display", "none");
            }
            else if (optionSelected != "1") {
                $(CreateView.Controles.divGrupo).css("display", "block");
                $(CreateView.Controles.divServicio).css("display", "block");
                $(CreateView.Controles.divClasificacion).css("display", "none");
                $(CreateView.Controles.divTipoIndicador).css("display", "block");
                $(CreateView.Controles.divIndicador).css("display", "block");
                $(CreateView.Controles.divAcumulacion).css("display", "none");

                $(CreateView.Controles.columnaDetalleTabla).html(CreateView.Mensajes.labelDetalleAgrupacion);
                $(CreateView.Controles.modalDetalle.titulo).html(CreateView.Mensajes.labelDetalleAgrupacion);
                $(CreateView.Controles.modalDetalle.divCategoria).css("display", "none");
                $(CreateView.Controles.modalDetalle.divCriterio).css("display", "block");

            }
        });

        $(document).on("click", CreateView.Controles.btnAgregarDetalleAgregacion, function () {
            $(CreateView.Controles.modalFormulaDetalleAgregacion).modal('show');
            $(CreateView.Controles.modalDetalle.btnGuardar).css("display", "initial");
            $(CreateView.Controles.modalDetalle.btnEliminar).css("display", "none");

            $(CreateView.Controles.modalDetalle.ddlCategoria).select2("enable", "true");
            $(CreateView.Controles.modalDetalle.ddlDetalle).select2("enable", "true");
            $(CreateView.Controles.modalDetalle.ddlCriterio).select2("enable", "true");
        });

        $(document).on("click", CreateView.Controles.btnEliminarDetalleAgregacion, function () {
            $(CreateView.Controles.modalFormulaDetalleAgregacion).modal('show');
            $(CreateView.Controles.modalDetalle.btnGuardar).css("display", "none");
            $(CreateView.Controles.modalDetalle.btnEliminar).css("display", "initial");

            $(CreateView.Controles.modalDetalle.ddlCategoria).select2("enable", false);
            $(CreateView.Controles.modalDetalle.ddlDetalle).select2("enable", false);
            $(CreateView.Controles.modalDetalle.ddlCriterio).select2("enable", false);
        });

        $(document).on("change", CreateView.Controles.formCrearFormula.radioTotal, function () {
            $(CreateView.Controles.formCrearFormula.divInputCategoriaDesagregacion).css("display", "none");
        });

        $(document).on("click", CreateView.Controles.btnAtrasGestionFormula, function (e) {
            $("a[href='#step-1']").trigger('click');
        });

        $(document).on("click", CreateView.Controles.btnFinalizarFormulaCalculo, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaAgregarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoFormulaAgregada)
                        .set('onok', function (closeEvent) {
                            window.location.href = CreateView.Variables.indexViewURL;
                        });
                });
        });

        $(document).on("click", CreateView.Controles.btnGuardarGestionFormulaCalculo, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaGuardadoParcial, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoFormulaCreada)
                        .set('onok', function (closeEvent) {
                            window.location.href = CreateView.Variables.indexViewURL;
                        });
                });
        });

        $(document).on("click", CreateView.Controles.btnCancelarGestionFormulaCalculo, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    window.location.href = CreateView.Variables.indexViewURL;
                });
        });

        $(document).on("click", CreateView.Controles.modalFecha.btnCancelar, function (e) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaCancelarAccion, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    $(CreateView.Controles.modalFechaFormulaCalculo).modal('hide');
                });
        });

        $(document).on("click", CreateView.Controles.btnCalendarFormula, function (e) {
            $(CreateView.Controles.modalFechaFormulaCalculo).modal('show');
        });

        $(document).on("click", CreateView.Controles.radioManual_modalFechaFormula, function () {
            $(CreateView.Controles.divTxtFechaInicio_modalFechaFormula).css("display", "block");
            $(CreateView.Controles.divTxtFechaFinal_modalFechaFormula).css("display", "block");
            $(CreateView.Controles.divDdlCategoríasTipoFecha_modalFechaFormula).css("display", "none");
        });

        $(document).on("click", CreateView.Controles.radioCategoríaDesagregacion_modalFechaFormula, function () {
            $(CreateView.Controles.divTxtFechaInicio_modalFechaFormula).css("display", "none");
            $(CreateView.Controles.divTxtFechaFinal_modalFechaFormula).css("display", "none");
            $(CreateView.Controles.divDdlCategoríasTipoFecha_modalFechaFormula).css("display", "block");
        });

        $(document).on("click", CreateView.Controles.modalDetalle.btnGuardar, function () {
            jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoDetalleAgregado)
                .set('onok', function (closeEvent) {
                    $(CreateView.Controles.modalFormulaDetalleAgregacion).modal('hide');
                    $(CreateView.Controles.chkValorTotal).prop("disabled", true);
                });
        });

        $(document).on("click", CreateView.Controles.btnRemoverItemFormula, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminaArgumento, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoArgumentoEliminado)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", CreateView.Controles.modalFecha.btnGuardar, function () {
            jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoArgumentoFechaCreado)
                .set('onok', function (closeEvent) {
                    $(CreateView.Controles.modalFechaFormulaCalculo).modal('hide');
                });
        });

        $(document).on("click", CreateView.Controles.modalFecha.btnEliminar, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminarArgumentoFecha, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEliminarArgumentoFecha)
                        .set('onok', function (closeEvent) {
                            $(CreateView.Controles.modalFechaFormulaCalculo).modal('hide');
                        });
                });
        });

        $(document).on("click", CreateView.Controles.btnAgregarArgumento, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaAgregarArgumento, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoArgumentoAgregado)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", CreateView.Controles.btnEjecutarFormula, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEjecutarFormula, jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoFormulaEjecutada)
                        .set('onok', function (closeEvent) {

                        });
                });
        });

        $(document).on("click", CreateView.Controles.modalDetalle.btnEliminar, function () {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CreateView.Mensajes.preguntaEliminarDetalle, jsMensajes.Variables.actionType.eliminar)
                .set('onok', function (closeEvent) {
                    jsMensajes.Metodos.OkAlertModal(CreateView.Mensajes.exitoEliminarDetalle)
                        .set('onok', function (closeEvent) {
                            $(CreateView.Controles.modalFormulaDetalleAgregacion).modal('hide');
                        });
                });
        });

        //$(document).on("keyup", CreateView.Controles.ControlesStep1, function (e) {
        //    CreateView.Metodos.ValidarFormularioCrear();
        //});

        //$(document).on("change", CreateView.Controles.ControlesStep1, function (e) {
        //    CreateView.Metodos.ValidarFormularioCrear();
        //});
    },

    Init: function () {
        CreateView.Eventos();

        let modo = $(CreateView.Controles.modoFormulario).val();

        CreateView.Metodos.CambiarEstadoBtnSiguienteFormCrearFormula(true);

        //CreateView.Metodos.ValidarFormularioCrear();

        if (modo == null) {
            //$(CreateView.Controles.formCrearFormula.radioCategoriaDesagregacion).prop("checked", false);
        }
        else if (modo == jsUtilidades.Variables.Acciones.Editar) {
            InsertarOpcionTodosSelect2Multiple(CreateView.Controles.formCrearFormula.ddlCategoriaDesagregacion);
            //$(CreateView.Controles.formCrearFormula.txtCodigoFormula).prop("disabled", true);
        }
        else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            //$(CreateView.Controles.formCrearFormula.txtCodigoFormula).val("");
            //$(CreateView.Controles.txtNombreFormula).val("");
        }
        else if (modo == jsUtilidades.Variables.Acciones.Consultar) {
            //$(CreateView.Controles.ControlesStep1).prop("disabled", true);
            //$(CreateView.Controles.divStep2).prop("disabled", true);
            //$(CreateView.Controles.formCrearFormula.btnGuardar).prop("disabled", true);
            //$(CreateView.Controles.btnGuardarGestionFormulaCalculo).prop("disabled", true);
            //$(CreateView.Controles.btnFinalizarFormulaCalculo).prop("disabled", true);
            //$(CreateView.Controles.btnCancelarGestionFormulaCalculo).prop("disabled", false);
            //$(CreateView.Controles.btnAtrasGestionFormula).prop("disabled", false);
        }
    }

};

$(function () {
    if ($(IndexView.Controles.IndexView).length > 0) {
        IndexView.Init();
    }

    if ($(CreateView.Controles.CreateView).length > 0) {
        CreateView.Init();
    }
});