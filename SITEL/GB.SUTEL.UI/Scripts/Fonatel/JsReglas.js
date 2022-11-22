JsReglas = {

    "Controles": {

        "btnGuardarRegla": "#btnGuardarRegla",
        "btnCancelar": "#btnCancelarRegla",
        "btnSiguienteRegla": "#btnSiguienteRegla",
        "btnEditarRegla": "#TableReglaDesagregacion tbody tr td .btn-edit",
        "btnClonarRegla": "#TableReglaDesagregacion tbody tr td .btn-clone",
        "btnBorrarRegla": "#TableReglaDesagregacion tbody tr td .btn-delete",
        "btnEditTipoRegla": "#TableTipoRegla tbody tr td .btn-edit",
        "btnEliminaTipoRegla": "#TableTipoRegla tbody tr td .btn-delete",
        "btnAddRegla": "#TableReglaDesagregacion tbody tr td .btn-add",
        "btnAtrasRegla": "#btnAtrasTipoRegla",
        "btnGuardarReglaTipo": "#btnGuardarReglaTipo",
        "divFormulaCambioMensual": "#divFormulaCambioMensual",
        "divFormulaContraIndicador": "#divFormulaContraIndicador",
        "divFormulaContraIndicadorSalida": "#divFormulaContraIndicadorSalida",
        "divFormulaContraIndicadorEntradaSalida": "#divFormulaContraIndicadorEntradaSalida",
        "divFormulaContraConstante": "#divFormulaContraConstante",
        "divFormulaContraAtributosValido": "#divFormulaContraAtributosValido",
        "divFormulaActualizacionSecuencial": "#divFormulaActualizacionSecuencial",
        "divContenedor": ".contenedor_regla",
        "TablaReglas": "#TableReglaDesagregacion tbody",
        "TablaDetalleReglas": "#TableTipoRegla tbody",
        "ddlVariableRegla": "#ddlVariableRegla",
        "ddlTipoRegla": "#ddlTipoRegla", "CodigoHelp": "#CodigoHelp",
        "nombreHelp": "#nombreHelp",
        "TipoIndicadorHelp": "#TipoIndicadorHelp",
        "DescripcionReglaHelp": "#DescripcionReglaHelp",
        "txtCodigo": "#txtCodigo",
        "txtNombre": "#txtNombre",
        "ddlIndicadorRegla": "#ddlIndicadorRegla",
        "txtDescripcionRegla": "#txtDescripcionRegla",
        "TipoReglaHelp": "#TipoReglaHelp",
        "ddlOperadorRegla": "#ddlOperadorRegla",
        "OperadorHelp": "#OperadorHelp",
        "VariableHelp": "#VariableHelp",
        "txtConstanteReglaHelp": "#txtConstanteReglaHelp",
        "ddlAtributosValidosCategoríaReglaHelp": "#ddlAtributosValidosCategoríaReglaHelp",
        "ddlAtributosValidosReglaHelp": "#ddlAtributosValidosReglaHelp",
        "ddlCategoríaActualizableReglaHelp": "#ddlCategoríaActualizableReglaHelp",
        "ddlIndicadorSalidaReglaHelp": "#ddlIndicadorSalidaReglaHelp",
        "ddlIndicadorComparacionHelp": "#ddlIndicadorComparacionHelp",
        "ddlVariableComparacionReglaHelp": "#ddlVariableComparacionReglaHelp",
        "ddlIndicadorComparacionEntradaSalidaHelp": "#ddlIndicadorComparacionEntradaSalidaHelp",
        "ddlVariableComparacionEntradaSalidaReglaHelp": "#ddlVariableComparacionEntradaSalidaReglaHelp",
        "ddlIndicadorComparacionRegla": "#ddlIndicadorComparacionRegla",
        "ddlVariableComparacionRegla": "#ddlVariableComparacionRegla",
        "txtConstanteRegla": "#txtConstanteRegla",
        "ddlAtributosValidosCategoríaRegla": "#ddlAtributosValidosCategoríaRegla",
        "ddlAtributosValidosRegla": "#ddlAtributosValidosRegla",
        "ddlCategoríaActualizableRegla": "#ddlCategoríaActualizableRegla",
        "ddlIndicadorSalidaRegla": "#ddlIndicadorSalidaRegla",
        "ddlIndicadorComparacionReglaEntradaSalida": "#ddlIndicadorComparacionReglaEntradaSalida",
        "ddlVariableComparacionReglaEntradaSalida": "#ddlVariableComparacionReglaEntradaSalida",
        "formularioReglasInput": "#formularioReglas input, textarea",
        "formularioReglasSelect": "#formularioReglas select",
        "chkAtributosValidosRegla": "#chkAtributosValidosRegla",
        "FormularioDetalle": "#FormularioCrear",
        "txtModo": "#txtmodo",
        "txtidIndicadorVariableString": "#txtidIndicadorVariableString",
        "txtidDetalleReglaValidacion": "#txtidDetalleReglaValidacion",
        "txtidCompara": "#txtidCompara",
        "step2": "#step2",
        "btnFinalizar": "#btnFinalizar"
    },

    "Variables": {
        "FormulaCambioMensual": "1",
        "FormulaContraIndicador": "2",
        "FormulaContraConstante": "3",
        "FormulaContraAtributosValido": "4",
        "FormulaActualizacionSecuencial": "5",
        "FormulaContraIndicadorSalida": "6",
        "FormulaContraIndicadorEntradaSalida": "7",
        "ListaReglas": [],
        "ListaDetalleReglas": [],
        "ListaVariablesDato": [],
        "esModoEdicion": false,
        "objetoTipoRegla": null,
    },

    "Mensajes": {
        MensajeDetalleAgregado: "El Tipo de Regla ha sido creado",
        MensajeDetalleEditado: "El Tipo de Regla ha sido editado",
        MensajeEliminarRegla: "La Regla ha sido eliminada",
        MensajeAgregarVariasReglas: "Recuerde que puede agregar más de una Regla de Validación para el Indicador seleccionado",
        MensajeReglaCreada: "La Regla ha sido creada"
    },

    "Metodos": {

        "SeleccionarStep": function (div) {
            $(JsReglas.Controles.divContenedor).addClass('hidden');
            $(div).removeClass('hidden');
        },

        "HabilitarControlesTipoRegla": function (selected) {
            $(JsReglas.Controles.divFormulaCambioMensual).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraIndicador).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraConstante).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraAtributosValido).addClass("hidden");
            $(JsReglas.Controles.ddlVariableRegla).prop("disabled", false);
            $(JsReglas.Controles.divFormulaActualizacionSecuencial).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraIndicadorSalida).addClass("hidden");
            $(JsReglas.Controles.divFormulaContraIndicadorEntradaSalida).addClass("hidden");

            switch (selected) {
                case JsReglas.Variables.FormulaContraIndicador:
                    $(JsReglas.Controles.divFormulaContraIndicador).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraConstante:
                    $(JsReglas.Controles.divFormulaContraConstante).removeClass("hidden");
                    break;
                case JsReglas.Variables.FormulaContraAtributosValido:
                    $(JsReglas.Controles.divFormulaContraAtributosValido).removeClass("hidden");
                    $(JsReglas.Controles.ddlVariableRegla).val('...').change();
                    $(JsReglas.Controles.ddlVariableRegla).prop("disabled", true);
                    break;
                case JsReglas.Variables.FormulaActualizacionSecuencial:
                    $(JsReglas.Controles.divFormulaActualizacionSecuencial).removeClass("hidden");
                    break;

                case JsReglas.Variables.FormulaContraIndicadorSalida:
                    $(JsReglas.Controles.divFormulaContraIndicadorSalida).removeClass("hidden");
                    break;

                case JsReglas.Variables.FormulaContraIndicadorEntradaSalida:
                    $(JsReglas.Controles.divFormulaContraIndicadorEntradaSalida).removeClass("hidden");
                    break;

                default:
            }
        },

        "CargarTablaReglas": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsReglas.Variables.ListaReglas.length; i++) {
                let reglas = JsReglas.Variables.ListaReglas[i];

                html = html + "<tr>"

                html = html + "<td scope='row'>" + reglas.Codigo + "</td>";
                html = html + "<td>" + reglas.Nombre + "</td>";
                html = html + "<td>" + reglas.ListadoTipoReglas + "</td>";


                html = html + "<td>" + reglas.EstadoRegistro.Nombre + "</td>";

                html = html + "<td><button type='button' data - toggle='tooltip' data - placement='top' value = '" + reglas.id + "' title = 'Editar' class='btn-icon-base btn-edit' ></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Clonar' value = '" + reglas.id + "' class='btn-icon-base btn-clone'></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' value = '" + reglas.id + "' class='btn-icon-base btn-delete'></button></td>";


                html = html + "</tr>"
            }
            $(JsReglas.Controles.TablaReglas).html(html);
            CargarDatasource();
        },

        "CargarTablaDetalleReglas": function () {
            EliminarDatasource();
            let html = "";
            for (var i = 0; i < JsReglas.Variables.ListaDetalleReglas.length; i++) {
                let detalleReglas = JsReglas.Variables.ListaDetalleReglas[i];
                html = html + "<tr>"
                html = html + "<td scope='row'>" + detalleReglas.NombreVariable + "</td >";
                html = html + "<td>" + detalleReglas.tipoReglaValidacion.Nombre + "</td>";
                html = html + "<td>" + detalleReglas.operadorArismetico.Nombre + "</td>";
                html = html + "<td><button type='button' data - toggle='tooltip' data - placement='top' data-index=" + i + " value = '" + detalleReglas.id + "' title = 'Editar' class='btn-icon-base btn-edit' ></button>";
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' data-index=" + i + " title='Eliminar' value = '" + detalleReglas.id + "' class='btn-icon-base btn-delete'></button></td></tr>";
            }
            $(JsReglas.Controles.TablaDetalleReglas).html(html);
            CargarDatasource();
        },

        "RestablecerCampos": function () {
            $(JsReglas.Controles.ddlOperadorRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlVariableRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlIndicadorComparacionRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlVariableComparacionRegla).val(null).trigger('change');
            $(JsReglas.Controles.txtConstanteRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlAtributosValidosCategoríaRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlAtributosValidosRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlCategoríaActualizableRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlIndicadorSalidaRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlIndicadorComparacionRegla).val(null).trigger('change');
            $(JsReglas.Controles.ddlVariableComparacionReglaEntradaSalida).val(null).trigger('change');

        },

        "ValidarControles": function () {
            let validar = true;
            $(JsReglas.Controles.CodigoHelp).addClass("hidden");
            $(JsReglas.Controles.nombreHelp).addClass("hidden");
            $(JsReglas.Controles.TipoIndicadorHelp).addClass("hidden");
            $(JsReglas.Controles.DescripcionReglaHelp).addClass("hidden");

            let codigo = $(JsReglas.Controles.txtCodigo).val().trim();
            let nombre = $(JsReglas.Controles.txtNombre).val().trim();
            let Indicador = $(JsReglas.Controles.ddlIndicadorRegla).val();
            let Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val().trim();

            if (codigo.length == 0) {
                $(JsReglas.Controles.CodigoHelp).removeClass("hidden");
                Validar = false;
            }
            if (nombre.length == 0) {
                $(JsReglas.Controles.nombreHelp).removeClass("hidden");
                validar = false;
            }
            if (Indicador.length == 0) {
                $(JsReglas.Controles.TipoIndicadorHelp).removeClass("hidden");
                validar = false;
            }
            if (Descripcion.length == 0) {
                $(JsReglas.Controles.DescripcionReglaHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        },

        "ValidarCampos": function () {
            let validar = true;
            $(JsReglas.Controles.CodigoHelp).addClass("hidden");
            $(JsReglas.Controles.nombreHelp).addClass("hidden");
            $(JsReglas.Controles.TipoIndicadorHelp).addClass("hidden");
            $(JsReglas.Controles.DescripcionReglaHelp).addClass("hidden");

            let codigo = $(JsReglas.Controles.txtCodigo).val().trim();
            let nombre = $(JsReglas.Controles.txtNombre).val().trim();
            let Indicador = $(JsReglas.Controles.ddlIndicadorRegla).val();
            let Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val().trim();

            if (codigo.length == 0) {
                Validar = false;
            }
            if (nombre.length == 0) {
                validar = false;
            }
            if (Indicador.length == 0) {
                validar = false;
            }
            if (Descripcion.length == 0) {
                validar = false;
            }
            return validar;
        },

        "ValidarControlesTipo": function () {
            let validarTipo = true;

            $(JsReglas.Controles.TipoReglaHelp).addClass("hidden");
            $(JsReglas.Controles.OperadorHelp).addClass("hidden");
            $(JsReglas.Controles.VariableHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorComparacionHelp).addClass("hidden");
            $(JsReglas.Controles.ddlVariableComparacionReglaHelp).addClass("hidden");
            $(JsReglas.Controles.txtConstanteReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlAtributosValidosCategoríaReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlAtributosValidosReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlCategoríaActualizableReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorSalidaReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorComparacionHelp).addClass("hidden");
            $(JsReglas.Controles.ddlVariableComparacionReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorComparacionEntradaSalidaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlVariableComparacionEntradaSalidaReglaHelp).addClass("hidden");

            let Tipo = $(JsReglas.Controles.ddlTipoRegla).val();
            let Operador = $(JsReglas.Controles.ddlOperadorRegla).val();
            let Variable = $(JsReglas.Controles.ddlVariableRegla).val();

            if (Operador == 0) {
                $(JsReglas.Controles.OperadorHelp).removeClass("hidden");
                validarTipo = false;
            }
            if (Variable == 0) {
                $(JsReglas.Controles.VariableHelp).removeClass("hidden");
                validarTipo = false;
            }
            if (Tipo == 0) {
                $(JsReglas.Controles.TipoReglaHelp).removeClass("hidden");
                validarTipo = false;
            }
            if (Tipo == 2) {
                if ($(JsReglas.Controles.ddlIndicadorComparacionRegla).val() == 0 || $(JsReglas.Controles.ddlIndicadorComparacionRegla).val() == null) {
                    $(JsReglas.Controles.ddlIndicadorComparacionHelp).removeClass("hidden");
                    validarTipo = false;
                }
                if ($(JsReglas.Controles.ddlVariableComparacionRegla).val() == 0 || $(JsReglas.Controles.ddlVariableComparacionRegla).val() == null) {
                    $(JsReglas.Controles.ddlVariableComparacionReglaHelp).removeClass("hidden");
                    validarTipo = false;
                }
            }
            if (Tipo == 3) {
                if ($(JsReglas.Controles.txtConstanteRegla).val().trim() == 0 || $(JsReglas.Controles.txtConstanteRegla).val().trim() == null) {
                    $(JsReglas.Controles.txtConstanteReglaHelp).removeClass("hidden");
                    validarTipo = false;
                }
            }
            if (Tipo == 4) {
                if ($(JsReglas.Controles.ddlAtributosValidosCategoríaRegla).val() == 0 || $(JsReglas.Controles.ddlAtributosValidosCategoríaRegla).val() == null) {
                    $(JsReglas.Controles.ddlAtributosValidosCategoríaReglaHelp).removeClass("hidden");
                    validarTipo = false;
                }
                if ($(JsReglas.Controles.ddlAtributosValidosRegla).val() == null || $(JsReglas.Controles.ddlAtributosValidosRegla).val() == 0) {
                    $(JsReglas.Controles.ddlAtributosValidosReglaHelp).removeClass("hidden");
                    validarTipo = false;
                }
            }
            if (Tipo == 5) {
                if ($(JsReglas.Controles.ddlCategoríaActualizableRegla).val() == 0 || $(JsReglas.Controles.ddlCategoríaActualizableRegla).val() == null) {
                    $(JsReglas.Controles.ddlCategoríaActualizableReglaHelp).removeClass("hidden");
                    validarTipo = false;
                }
            }
            if (Tipo == 6) {
                if ($(JsReglas.Controles.ddlIndicadorSalidaRegla).val() == 0 || $(JsReglas.Controles.ddlIndicadorSalidaRegla).val() == null) {
                    $(JsReglas.Controles.ddlIndicadorSalidaReglaHelp).removeClass("hidden");
                    validarTipo = false;
                }
            }
            if (Tipo == 7) {
                if ($(JsReglas.Controles.ddlIndicadorComparacionReglaEntradaSalida).val() == 0 || $(JsReglas.Controles.ddlIndicadorComparacionReglaEntradaSalida).val() == null) {
                    $(JsReglas.Controles.ddlIndicadorComparacionEntradaSalidaHelp).removeClass("hidden");
                    validarTipo = false;
                }
                if ($(JsReglas.Controles.ddlVariableComparacionReglaEntradaSalida).val() == 0 || $(JsReglas.Controles.ddlVariableComparacionReglaEntradaSalida).val() == null) {
                    $(JsReglas.Controles.ddlVariableComparacionEntradaSalidaReglaHelp).removeClass("hidden");
                    validarTipo = false;
                }
            }
            return validarTipo;
        },

        "ValidarOpcionSiguiente": function () {

            let codigo = $(JsReglas.Controles.txtCodigo).val().trim();
            let nombre = $(JsReglas.Controles.txtNombre).val().trim();
            let Indicador = $(JsReglas.Controles.ddlIndicadorRegla).val();
            let Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val().trim();
            let validar = false;

            if (codigo.length == 0) {
                validar = true;
            }
            if (nombre.length == 0) {
                validar = true;
            }
            if (Indicador.length == 0) {
                validar = true;
            }
            if (Descripcion.length == 0) {
                validar = true;
            }

            $(JsReglas.Controles.step2).prop("disabled", validar);
            $(JsReglas.Controles.btnSiguienteRegla).prop("disabled", validar);
        },

        "ValidarNombreyCodigo": function () {
            let validar = true;

            $(JsReglas.Controles.TipoReglaHelp).addClass("hidden");
            $(JsReglas.Controles.OperadorHelp).addClass("hidden");
            $(JsReglas.Controles.txtCodigo).parent().addClass("has-error");
            $(JsReglas.Controles.txtNombre).parent().addClass("has-error");

            let Codigo = $(JsReglas.Controles.txtCodigo).val().trim();
            let Nombre = $(JsReglas.Controles.txtNombre).val().trim();

            if (Codigo.length == 0) {
                $(JsReglas.Controles.CodigoHelp).removeClass("hidden");
                $(JsReglas.Controles.txtCodigo).parent().addClass("has-error");
                validar = false;
            } else {
                $(JsReglas.Controles.txtCodigo).parent().removeClass("has-error");
                $(JsReglas.Controles.CodigoHelp).addClass("hidden");
                Validar = false;
            }
            if (Nombre.length == 0) {
                $(JsReglas.Controles.nombreHelp).removeClass("hidden");
                $(JsReglas.Controles.txtNombre).parent().addClass("has-error");
                validar = false;
            } else {
                $(JsReglas.Controles.nombreHelp).addClass("hidden");
                $(JsReglas.Controles.txtNombre).parent().removeClass("has-error");
                Validar = false;
            }
            return validar;
        },

        "CargarDetallesRegla": function (index) {

            if (JsReglas.Variables.ListaDetalleReglas.length > index) {
                JsReglas.Variables.esModoEdicion = true;
                JsReglas.Variables.objetoTipoRegla = JsReglas.Variables.ListaDetalleReglas[index];

                $(JsReglas.Controles.txtidDetalleReglaValidacion).val(JsReglas.Variables.objetoTipoRegla.IdDetalleReglaValidacion);
                $(JsReglas.Controles.ddlTipoRegla).val(JsReglas.Variables.objetoTipoRegla.IdTipo).change();
                $(JsReglas.Controles.ddlOperadorRegla).val(JsReglas.Variables.objetoTipoRegla.IdOperador).change();
                $(JsReglas.Controles.ddlVariableRegla).val(JsReglas.Variables.objetoTipoRegla.idIndicadorVariableString).change();

                //REGLA CONTRA OTRO INDICADOR ENTRADA
                if (JsReglas.Variables.objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorEntrada) {
                    $(JsReglas.Controles.txtidCompara).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorEntrada.IdCompara);
                    $(JsReglas.Controles.ddlIndicadorComparacionRegla).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorEntrada.idIndicadorComparaString).change();
                }
                //REGLA CONTRA CONSTANTE
                if (JsReglas.Variables.objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraConstante) {
                    $(JsReglas.Controles.txtidCompara).val(JsReglas.Variables.objetoTipoRegla.reglaComparacionConstante.idCompara);
                    $(JsReglas.Controles.txtConstanteRegla).val(JsReglas.Variables.objetoTipoRegla.reglaComparacionConstante.Constante);
                }
                //REGLA CONTRA ATRIBUTOS VALIDOS
                if (JsReglas.Variables.objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraAtributosValidos) {
                    $(JsReglas.Controles.txtidCompara).val(JsReglas.Variables.objetoTipoRegla.reglaAtributosValidos.IdCompara);
                    $(JsReglas.Controles.ddlAtributosValidosCategoríaRegla).val(JsReglas.Variables.objetoTipoRegla.reglaAtributosValidos.IdCategoria).change();
                }
                //REGLA CONTRA ACTUALIZACION SECUENCIAL
                if (JsReglas.Variables.objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaActualizacionSecuencial) {
                    $(JsReglas.Controles.txtidCompara).val(JsReglas.Variables.objetoTipoRegla.reglaSecuencial.IdCompara);
                    $(JsReglas.Controles.ddlCategoríaActualizableRegla).val(JsReglas.Variables.objetoTipoRegla.reglaSecuencial.IdCategoria).change();
                }
                //REGLA CONTRA INDICADOR DE SALIDA
                if (JsReglas.Variables.objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorSalida) {
                    $(JsReglas.Controles.txtidCompara).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorSalida.IdCompara);
                    $(JsReglas.Controles.ddlIndicadorSalidaRegla).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorSalida.idIndicadorComparaString).change();
                }
                //REGLA CONTRA INDICADOR DE ENTRADA-SALIDA
                if (JsReglas.Variables.objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorEntradaSalida) {
                    $(JsReglas.Controles.txtidCompara).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorEntradaSalida.IdCompara);
                    $(JsReglas.Controles.ddlIndicadorComparacionReglaEntradaSalida).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorEntradaSalida.idIndicadorComparaString).change();
                }
            }
        },

        "LimpiarCamposDetalles": function () {

            $(JsReglas.Controles.ddlTipoRegla).val("").trigger('change');
            $(JsReglas.Controles.ddlOperadorRegla).val("").trigger('change');
            $(JsReglas.Controles.ddlVariableRegla).val("").trigger('change');

        },

        "LimpiarValidaciones": function () {

            $(JsReglas.Controles.TipoReglaHelp).addClass("hidden");
            $(JsReglas.Controles.OperadorHelp).addClass("hidden");
            $(JsReglas.Controles.VariableHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorComparacionHelp).addClass("hidden");
            $(JsReglas.Controles.ddlVariableComparacionReglaHelp).addClass("hidden");
            $(JsReglas.Controles.txtConstanteReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlAtributosValidosCategoríaReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlAtributosValidosReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlCategoríaActualizableReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorSalidaReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorComparacionHelp).addClass("hidden");
            $(JsReglas.Controles.ddlVariableComparacionReglaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlIndicadorComparacionEntradaSalidaHelp).addClass("hidden");
            $(JsReglas.Controles.ddlVariableComparacionEntradaSalidaReglaHelp).addClass("hidden");

        },

    },

    "Consultas": {

        "InsertarReglaValidacion": function () {

            $("#loading").fadeIn();
            let objetoRegla = new Object()
            objetoRegla.Codigo = $(JsReglas.Controles.txtCodigo).val();
            objetoRegla.Nombre = $(JsReglas.Controles.txtNombre).val();
            objetoRegla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();
            objetoRegla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val();

            execAjaxCall("/ReglasValidacion/InsertarReglaValidacion", "POST", objetoRegla)
                .then((obj) => {

                    jsMensajes.Metodos.OkAlertModal("La Regla ha sido creada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/ReglasValidacion/Index";
                        });

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

        "InsertarReglaValidacionParcial": function () {

            $("#loading").fadeIn();
            let objetoRegla = new Object()
            objetoRegla.Codigo = $(JsReglas.Controles.txtCodigo).val();
            objetoRegla.Nombre = $(JsReglas.Controles.txtNombre).val();
            objetoRegla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();
            objetoRegla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val();

            execAjaxCall("/ReglasValidacion/InsertarReglaValidacion", "POST", objetoRegla)
                .then((obj) => {
                    $("a[href='#step-2']").trigger('click');
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);
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

        "EditarReglaValidacion": function () {

            $("#loading").fadeIn();
            let objetoRegla = new Object()
            objetoRegla.id = ObtenerValorParametroUrl("id");
            objetoRegla.Codigo = $(JsReglas.Controles.txtCodigo).val().trim();
            objetoRegla.Nombre = $(JsReglas.Controles.txtNombre).val().trim();
            objetoRegla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();
            objetoRegla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val().trim();

            execAjaxCall("/ReglasValidacion/EditarReglaValidacion", "POST", objetoRegla)
                .then((obj) => {

                    jsMensajes.Metodos.OkAlertModal("La Regla ha sido editada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/ReglasValidacion/Index";
                        });

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

        "EditarReglaValidacionParcial": function () {

            $("#loading").fadeIn();
            let objetoRegla = new Object()
            objetoRegla.id = ObtenerValorParametroUrl("id");
            objetoRegla.Codigo = $(JsReglas.Controles.txtCodigo).val();
            objetoRegla.Nombre = $(JsReglas.Controles.txtNombre).val();
            objetoRegla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();
            objetoRegla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val();

            execAjaxCall("/ReglasValidacion/EditarReglaValidacion", "POST", objetoRegla)
                .then((obj) => {
                    $("a[href='#step-2']").trigger('click');
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

        "ClonarReglaValidacion": function () {

            $("#loading").fadeIn();

            let Regla = new Object();

            let id = ObtenerValorParametroUrl("id");
            Regla.id = id;

            Regla.Codigo = $(JsReglas.Controles.txtCodigo).val();
            Regla.Nombre = $(JsReglas.Controles.txtNombre).val();
            Regla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();
            Regla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val();


            execAjaxCall("/ReglasValidacion/ClonarRegla", "POST", Regla)
                .then((obj) => {
                    jsMensajes.Metodos.OkAlertModal("La Regla ha sido creada")
                        .set('onok', function (closeEvent) {
                            window.location.href = "/Fonatel/ReglasValidacion/Index";
                        });
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ClonarReglaValidacionParcial": function () {

            $("#loading").fadeIn();

            let Regla = new Object();

            let id = ObtenerValorParametroUrl("id");
            Regla.id = id;

            Regla.Codigo = $(JsReglas.Controles.txtCodigo).val();
            Regla.Nombre = $(JsReglas.Controles.txtNombre).val();
            Regla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();
            Regla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val();

            execAjaxCall("/ReglasValidacion/ClonarRegla", "POST", Regla)
                .then((obj) => {
                    InsertarParametroUrl("id", obj.objetoRespuesta.id);
                    if ($(JsReglas.Controles.TablaDetalleReglas).length > 0) {
                        JsReglas.Consultas.ConsultaListaDetalleReglas();
                    }
                    $("a[href='#step-2']").trigger('click');
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EliminarRegla": function (IdRegla) {
            $("#loading").fadeIn();
            let objRegla = new Object()
            objRegla.id = IdRegla;
            execAjaxCall("/ReglasValidacion/EliminarRegla", "POST", objRegla)
                .then((data) => {
                    jsMensajes.Metodos.OkAlertModal(JsReglas.Mensajes.MensajeEliminarRegla)
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/Index" });
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ValidarExistenciaReglas": function (IdRegla) {

            $("#loading").fadeIn();
            let objRegla = new Object()
            objRegla.id = IdRegla;
            execAjaxCall("/ReglasValidacion/ValidarRegla", "POST", objRegla)
                .then((obj) => {
                    if (obj.objetoRespuesta.length == 0) {
                        JsReglas.Consultas.EliminarRegla(IdRegla);
                    } else {
                        let dependencias = obj.objetoRespuesta[0] + "<br>"
                        jsMensajes.Metodos.ConfirmYesOrNoModal("La Regla ya está en uso en los<br>" + dependencias + "<br>¿Desea Eliminar?", jsMensajes.Variables.actionType.eliminar)
                            .set('onok', function (closeEvent) {
                                JsReglas.Consultas.EliminarRegla(IdRegla);
                            });
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

        "EliminarDetalleRegla": function (idDetalleRegla) {

            $("#loading").fadeIn();

            let objRegla = new Object()
            objRegla.idDetalleReglaString = idDetalleRegla;

            execAjaxCall("/ReglasValidacion/EliminarDetalleRegla", "POST", objRegla)
                .then((data) => {
                    jsMensajes.Metodos.OkAlertModal("El Tipo de Regla ha sido eliminado")
                        .set('onok', function (closeEvent) {

                            JsReglas.Metodos.LimpiarCamposDetalles();
                            $(JsReglas.Controles.ddlTipoRegla).prop("disabled", false);
                            if ($(JsReglas.Controles.TablaDetalleReglas).length > 0) {
                                JsReglas.Consultas.ConsultaListaDetalleReglas();
                            }
                        });
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ValidarExistenciaDetalleReglas": function (idDetalleRegla) {

            $("#loading").fadeIn();
            let objRegla = new Object()
            objRegla.id = idDetalleRegla;
            execAjaxCall("/ReglasValidacion/ValidarRegla", "POST", objRegla)
                .then((obj) => {
                    if (obj.objetoRespuesta.length == 0) {
                        JsReglas.Consultas.EliminarFormulario(idDetalleRegla);
                    } else {
                        let dependencias = obj.objetoRespuesta[0] + "<br>"
                        jsMensajes.Metodos.ConfirmYesOrNoModal("La Regla de Validación ya está en uso en los<br>" + dependencias + "<br>¿Desea Eliminar?", jsMensajes.Variables.actionType.eliminado)
                            .set('onok', function (closeEvent) {
                                JsReglas.Consultas.EliminarDetalleRegla(idDetalleRegla);
                            });
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

        "ConsultaListaReglas": function () {

            $("#loading").fadeIn();
            execAjaxCall("/ReglasValidacion/ObtenerListaReglasValidacion", "GET")
                .then((obj) => {
                    JsReglas.Variables.ListaReglas = obj.objetoRespuesta;
                    JsReglas.Metodos.CargarTablaReglas();
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });

        },

        "ConsultaListaDetalleReglas": function () {
            let IdRegla = ObtenerValorParametroUrl("id");
            if (IdRegla != null) {

                $("#loading").fadeIn();
                execAjaxCall("/ReglasValidacion/ObtenerListaDetalleReglasValidacion?IdReglasValidacionTipo=" + IdRegla, "GET")
                    .then((obj) => {
                        JsReglas.Variables.ListaDetalleReglas = obj.objetoRespuesta;
                        JsReglas.Metodos.CargarTablaDetalleReglas();
                    }).catch((obj) => {
                        console.log(obj);
                        if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                            jsMensajes.Metodos.OkAlertErrorModal()
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
        },

        "ConsultaVariablesDato": function (idIndicadorString) {

            $("#loading").fadeIn();

            execAjaxCall("/ReglasValidacion/ObtenerListaVariablesDato", "GET", { idIndicadorString })
                .then((obj) => {

                    let html = "<option value=''/>";
                    for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                        html = html + "<option value='" + obj.objetoRespuesta[i].id + "'>" + obj.objetoRespuesta[i].NombreVariable + "</option>"
                    }

                    $(JsReglas.Controles.ddlVariableRegla).html(html);

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaVariablesDatoEntrada": function (idIndicadorString) {

            $("#loading").fadeIn();

            execAjaxCall("/ReglasValidacion/ObtenerListaVariablesDato", "GET", { idIndicadorString })
                .then((obj) => {

                        let html = "<option value=''/>";
                        for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                            html = html + "<option value='" + obj.objetoRespuesta[i].id + "'>" + obj.objetoRespuesta[i].NombreVariable + "</option>"
                        }

                        $(JsReglas.Controles.ddlVariableComparacionRegla).html(html);

                    if (JsReglas.Variables.esModoEdicion) {
                        $(JsReglas.Controles.ddlVariableComparacionRegla).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorEntrada.idVariableComparaString).change();
                    }
                    
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaVariablesDatoEntradaSalida": function (idIndicadorString) {

            $("#loading").fadeIn();

            execAjaxCall("/ReglasValidacion/ObtenerListaVariablesDato", "GET", { idIndicadorString })
                .then((obj) => {

                    let html = "<option value=''/>";
                    for (var i = 0; i < obj.objetoRespuesta.length; i++) {
                        html = html + "<option value='" + obj.objetoRespuesta[i].id + "'>" + obj.objetoRespuesta[i].NombreVariable + "</option>"
                    }

                    $(JsReglas.Controles.ddlVariableComparacionReglaEntradaSalida).html(html);

                    if (JsReglas.Variables.esModoEdicion) {
                        $(JsReglas.Controles.ddlVariableComparacionReglaEntradaSalida).val(JsReglas.Variables.objetoTipoRegla.reglaIndicadorEntradaSalida.idVariableComparaString).change();
                    }

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "ConsultaDetallesCategoria": function (idCategoria) {

            $("#loading").fadeIn();


            let RelacionCategoria = new Object();
            RelacionCategoria.idCategoria = idCategoria;

            execAjaxCall("/ReglasValidacion/ObtenerListaDetallesCategoria", "POST", RelacionCategoria)
                .then((obj) => {
                    let respuestaRelacion = obj.objetoRespuesta[0].DetalleRelacionCategoria;

                    let html = "<option value='all'>Todos</option>";
                    for (var i = 0; i < respuestaRelacion.length; i++) {
                        html = html + "<option value=" + respuestaRelacion[i].CategoriaAtributo.idCategoria + ">" + respuestaRelacion[i].CategoriaAtributo.Codigo + " / " + respuestaRelacion[i].CategoriaAtributo.NombreCategoria + "</option>"
                    }

                    $(JsReglas.Controles.ddlAtributosValidosRegla).html(html);

                    if (JsReglas.Variables.esModoEdicion) {

                        let listaAtributos = JsReglas.Variables.objetoTipoRegla.reglaAtributosValidos.idAtributoString.split(',');

                        $(JsReglas.Controles.ddlAtributosValidosRegla).val(listaAtributos).change();
                    }

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "InsertarDetalleRegla": function () {

            $("#loading").fadeIn();

            let objetoTipoRegla = new Object()

            objetoTipoRegla.id = ObtenerValorParametroUrl("id");
            objetoTipoRegla.IdTipo = $(JsReglas.Controles.ddlTipoRegla).val();
            objetoTipoRegla.IdOperador = $(JsReglas.Controles.ddlOperadorRegla).val();
            objetoTipoRegla.idIndicadorVariableString = $(JsReglas.Controles.ddlVariableRegla).val();
            objetoTipoRegla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();

            //REGLA CONTRA OTRO INDICADOR ENTRADA
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorEntrada) {
                objetoTipoRegla.reglaIndicadorEntrada = {};
                objetoTipoRegla.reglaIndicadorEntrada.idIndicadorComparaString = $(JsReglas.Controles.ddlIndicadorComparacionRegla).val();
                objetoTipoRegla.reglaIndicadorEntrada.idVariableComparaString = $(JsReglas.Controles.ddlVariableComparacionRegla).val();
            }
            //REGLA CONTRA CONSTANTE
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraConstante) {
                objetoTipoRegla.reglaComparacionConstante = {};
                objetoTipoRegla.reglaComparacionConstante.Constante = $(JsReglas.Controles.txtConstanteRegla).val();
            }
            //REGLA CONTRA ATRIBUTOS VALIDOS
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraAtributosValidos) {
                objetoTipoRegla.reglaAtributosValidos = {};
                objetoTipoRegla.reglaAtributosValidos.IdCategoria = $(JsReglas.Controles.ddlAtributosValidosCategoríaRegla).val();
                objetoTipoRegla.reglaAtributosValidos.idAtributoString = $(JsReglas.Controles.ddlAtributosValidosRegla).val().toString();
            }
            //REGLA CONTRA ACTUALIZACION SECUENCIAL
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaActualizacionSecuencial) {
                objetoTipoRegla.reglaSecuencial = {};
                objetoTipoRegla.reglaSecuencial.idCategoria = $(JsReglas.Controles.ddlCategoríaActualizableRegla).val();
            }
            //REGLA CONTRA INDICADOR DE SALIDA
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorSalida) {
                objetoTipoRegla.reglaIndicadorSalida = {};
                objetoTipoRegla.reglaIndicadorSalida.idIndicadorComparaString = $(JsReglas.Controles.ddlIndicadorSalidaRegla).val();
            }
            //REGLA CONTRA INDICADOR DE ENTRADA-SALIDA
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorEntradaSalida) {
                objetoTipoRegla.reglaIndicadorEntradaSalida = {};
                objetoTipoRegla.reglaIndicadorEntradaSalida.idIndicadorComparaString = $(JsReglas.Controles.ddlIndicadorComparacionReglaEntradaSalida).val();
                objetoTipoRegla.reglaIndicadorEntradaSalida.idVariableComparaString = $(JsReglas.Controles.ddlVariableComparacionReglaEntradaSalida).val();
            }

            execAjaxCall("/ReglasValidacion/InsertarDetalleRegla", "POST", objetoTipoRegla)
                .then((obj) => {
                    new Promise((resolve) => {
                        JsReglas.Variables.esModoEdicion = false;
                        jsMensajes.Metodos.OkAlertModal(JsReglas.Mensajes.MensajeDetalleAgregado)
                            .set('onok', function () {
                                resolve(true)
                            })
                    })
                        .then((obj) => {
                            jsMensajes.Metodos.OkAlertModal(JsReglas.Mensajes.MensajeAgregarVariasReglas)
                                .set('onok', function (closeEvent) {
                                    JsReglas.Metodos.LimpiarCamposDetalles();
                                    if ($(JsReglas.Controles.TablaDetalleReglas).length > 0) {
                                        JsReglas.Consultas.ConsultaListaDetalleReglas();
                                    }
                                })
                        })
                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "EditarDetalleRegla": function () {

            $("#loading").fadeIn();

            let objetoTipoRegla = new Object()
            objetoTipoRegla.id = ObtenerValorParametroUrl("id");
            objetoTipoRegla.IdDetalleReglaValidacion = $(JsReglas.Controles.txtidDetalleReglaValidacion).val();
            objetoTipoRegla.IdTipo = $(JsReglas.Controles.ddlTipoRegla).val();
            objetoTipoRegla.IdOperador = $(JsReglas.Controles.ddlOperadorRegla).val();
            objetoTipoRegla.idIndicadorVariableString = $(JsReglas.Controles.ddlVariableRegla).val();
            objetoTipoRegla.idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();

            //REGLA CONTRA OTRO INDICADOR ENTRADA
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorEntrada) {
                objetoTipoRegla.reglaIndicadorEntrada = {};
                objetoTipoRegla.reglaIndicadorEntrada.idCompara = $(JsReglas.Controles.txtidCompara).val();
                objetoTipoRegla.reglaIndicadorEntrada.idIndicadorComparaString = $(JsReglas.Controles.ddlIndicadorComparacionRegla).val();
                objetoTipoRegla.reglaIndicadorEntrada.idVariableComparaString = $(JsReglas.Controles.ddlVariableComparacionRegla).val();
            }
            //REGLA CONTRA CONSTANTE
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraConstante) {
                objetoTipoRegla.reglaComparacionConstante = {};
                objetoTipoRegla.reglaComparacionConstante.idCompara = $(JsReglas.Controles.txtidCompara).val();
                objetoTipoRegla.reglaComparacionConstante.Constante = $(JsReglas.Controles.txtConstanteRegla).val();
            }
            //REGLA CONTRA ATRIBUTOS VALIDOS
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraAtributosValidos) {
                objetoTipoRegla.reglaAtributosValidos = {};
                objetoTipoRegla.reglaAtributosValidos.IdCategoria = $(JsReglas.Controles.ddlAtributosValidosCategoríaRegla).val();
                 let array= $(JsReglas.Controles.ddlAtributosValidosRegla).val();

                objetoTipoRegla.reglaAtributosValidos.idAtributoString = array.join(',');
            }
            // REGLA CONTRA ACTUALIZACION SECUENCIAL
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaActualizacionSecuencial) {
                objetoTipoRegla.reglaSecuencial = {};
                objetoTipoRegla.reglaSecuencial.idCompara = $(JsReglas.Controles.txtidCompara).val();
                objetoTipoRegla.reglaSecuencial.idCategoria = $(JsReglas.Controles.ddlCategoríaActualizableRegla).val();
            }
            //REGLA CONTRA INDICADOR DE SALIDA
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorSalida) {
                objetoTipoRegla.reglaIndicadorSalida = {};
                objetoTipoRegla.reglaIndicadorSalida.idCompara = $(JsReglas.Controles.txtidCompara).val();
                objetoTipoRegla.reglaIndicadorSalida.idIndicadorComparaString = $(JsReglas.Controles.ddlIndicadorSalidaRegla).val();
            }
            //REGLA CONTRA INDICADOR DE ENTRADA-SALIDA
            if (objetoTipoRegla.IdTipo == jsUtilidades.Variables.TipoReglasDetalle.FormulaContraOtroIndicadorEntradaSalida) {
                objetoTipoRegla.reglaIndicadorEntradaSalida = {};
                objetoTipoRegla.reglaIndicadorEntradaSalida.idCompara = $(JsReglas.Controles.txtidCompara).val();
                objetoTipoRegla.reglaIndicadorEntradaSalida.idIndicadorComparaString = $(JsReglas.Controles.ddlIndicadorComparacionReglaEntradaSalida).val();
                objetoTipoRegla.reglaIndicadorEntradaSalida.idVariableComparaString = $(JsReglas.Controles.ddlVariableComparacionReglaEntradaSalida).val();
            }

            execAjaxCall("/ReglasValidacion/EditarDetalleRegla", "POST", objetoTipoRegla)
                .then((obj) => {
                    new Promise((resolve) => {
                        JsReglas.Variables.esModoEdicion = false;
                        $(JsReglas.Controles.ddlTipoRegla).prop("disabled", false);
                        jsMensajes.Metodos.OkAlertModal(JsReglas.Mensajes.MensajeDetalleEditado)
                            .set('onok', function () {
                                resolve(true)
                            })
                    })
                        .then((obj) => {
                            jsMensajes.Metodos.OkAlertModal(JsReglas.Mensajes.MensajeAgregarVariasReglas)
                                .set('onok', function (closeEvent) {
                                    JsReglas.Metodos.LimpiarCamposDetalles();
                                    if ($(JsReglas.Controles.TablaDetalleReglas).length > 0) {
                                        JsReglas.Consultas.ConsultaListaDetalleReglas();
                                    }
                                })
                        })

                }).catch((obj) => {
                    if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    } else {
                        jsMensajes.Metodos.OkAlertErrorModal(obj.MensajeError)
                            .set('onok', function (closeEvent) { });
                    }
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

        "CambioEstado": function (IdRegla) {
            $("#loading").fadeIn();
            let objRegla = new Object()
            objRegla.id = IdRegla;
            execAjaxCall("/ReglasValidacion/CambioEstado", "POST", objRegla)
                .then((data) => {
                    jsMensajes.Metodos.OkAlertModal(JsReglas.Mensajes.MensajeReglaCreada)
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/Index" });
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
                }).finally(() => {
                    $("#loading").fadeOut();
                });
        },

    }
}

$(document).on("click", JsReglas.Controles.btnGuardarRegla, function (e) {
    e.preventDefault();

    let CamposVacios = "Existen campos vacíos. "

    let modo = $(JsReglas.Controles.txtModo).val();

    if (JsReglas.Metodos.ValidarNombreyCodigo()) {

        if (modo == jsUtilidades.Variables.Acciones.Editar) {

            jsMensajes.Metodos.ConfirmYesOrNoModal(CamposVacios + "¿Desea realizar un guardado parcial de la Regla?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsReglas.Consultas.EditarReglaValidacion();
                })
                .set('oncancel', function (closeEvent) {
                    JsReglas.Metodos.ValidarControles();
                });

        } else if (modo == jsUtilidades.Variables.Acciones.Clonar) {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CamposVacios + "¿Desea realizar un guardado parcial de la Regla?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsReglas.Consultas.ClonarReglaValidacion();
                })
                .set('oncancel', function (closeEvent) {
                    JsReglas.Metodos.ValidarControles();
                });

        }
        else {
            jsMensajes.Metodos.ConfirmYesOrNoModal(CamposVacios + "¿Desea realizar un guardado parcial de la Regla?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsReglas.Consultas.InsertarReglaValidacion();
                })
                .set('oncancel', function (closeEvent) {
                    JsReglas.Metodos.ValidarControles();
                });
        }
    }

});

$(document).on("click", JsReglas.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/ReglasValidacion/Index";
        });
});

$(document).on("click", JsReglas.Controles.btnSiguienteRegla, function (e) {

    e.preventDefault();

    let idIndicadorString = $(JsReglas.Controles.ddlIndicadorRegla).val();

    if (JsReglas.Metodos.ValidarControles()) {

        let modo = $(JsReglas.Controles.txtModo).val();

        if (modo == jsUtilidades.Variables.Acciones.Editar) {

            JsReglas.Consultas.ConsultaVariablesDato(idIndicadorString);
            JsReglas.Consultas.EditarReglaValidacionParcial();


        } else if (modo == jsUtilidades.Variables.Acciones.Clonar) {

            if (JsReglas.Metodos.ValidarControles()) {

                JsReglas.Consultas.ClonarReglaValidacionParcial();
            }

            JsReglas.Consultas.ConsultaVariablesDato(idIndicadorString);
        }
        else {
            JsReglas.Consultas.InsertarReglaValidacionParcial();
            JsReglas.Consultas.ConsultaVariablesDato(idIndicadorString);
        }
    }

});

$(document).on("click", JsReglas.Controles.btnEditarRegla, function () {
    let id = $(this).val();
    window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Editar;
});

$(document).on("click", JsReglas.Controles.btnClonarRegla, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Regla?", jsMensajes.Variables.actionType.clonar)
        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/Create?id=" + id + "&modo=" + jsUtilidades.Variables.Acciones.Clonar });
});

$(document).on("click", JsReglas.Controles.btnBorrarRegla, function () {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar la Regla?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsReglas.Consultas.ValidarExistenciaReglas(id);
        });
});

$(document).on("click", JsReglas.Controles.btnAtrasRegla, function (e) {
    e.preventDefault();
    $("a[href='#step-1']").trigger('click');
});

$(document).on("keyup", JsReglas.Controles.formularioReglasInput, function () {
    JsReglas.Metodos.ValidarOpcionSiguiente();
});

$(document).on("change", JsReglas.Controles.formularioReglasSelect, function () {
    JsReglas.Metodos.ValidarOpcionSiguiente();
});

$(document).on("click", JsReglas.Controles.chkAtributosValidosRegla, function () {
    if ($(this).is(':checked')) {
        $("#ddlAtributosValidosRegla > option").prop("selected", "selected");
    } else {
        $("#ddlAtributosValidosRegla > option").removeAttr("selected");
    }
});

$(document).on("click", JsReglas.Controles.btnGuardarReglaTipo, function (e) {
    e.preventDefault();

    if (JsReglas.Variables.esModoEdicion) {

        if (JsReglas.Metodos.ValidarControlesTipo()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea editar el Tipo de Regla?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsReglas.Consultas.EditarDetalleRegla(true);
                });
        };
    } else {

        if (JsReglas.Metodos.ValidarControlesTipo()) {
            jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Tipo de Regla?", jsMensajes.Variables.actionType.agregar)
                .set('onok', function (closeEvent) {
                    JsReglas.Consultas.InsertarDetalleRegla(true);
                });
        };
    }
});

$(document).on("click", JsReglas.Controles.btnFinalizar, function (e) {
    e.preventDefault();
    let id = ObtenerValorParametroUrl("id");
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Regla de Validación?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            JsReglas.Consultas.CambioEstado(id);
        });
});

$(document).on("click", JsReglas.Controles.btnEliminaTipoRegla, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Tipo de Regla?", jsMensajes.Variables.actionType.eliminar)
        .set('onok', function (closeEvent) {
            JsReglas.Consultas.EliminarDetalleRegla(id);
        });
});

$(document).on("click", JsReglas.Controles.btnEditTipoRegla, function (e) {

    $(JsReglas.Controles.ddlTipoRegla).prop("disabled", true);

    let id = $(this).attr("data-index");

    JsReglas.Metodos.CargarDetallesRegla(id);


});

$(document).on("change", JsReglas.Controles.ddlTipoRegla, function () {
    var selected = $(this).val();
    JsReglas.Metodos.RestablecerCampos();
    JsReglas.Metodos.LimpiarValidaciones();
    JsReglas.Metodos.HabilitarControlesTipoRegla(selected);
});

$(document).on("change", JsReglas.Controles.ddlIndicadorComparacionRegla, function () {

    let idIndicadorString = $(this).val();
    if (idIndicadorString != 0) {
        JsReglas.Consultas.ConsultaVariablesDatoEntrada(idIndicadorString);
    }
});

$(document).on("change", JsReglas.Controles.ddlIndicadorComparacionReglaEntradaSalida, function () {

    let idIndicadorString = $(this).val();

    if (idIndicadorString != 0) {
        JsReglas.Consultas.ConsultaVariablesDatoEntradaSalida(idIndicadorString);
    }

});

$(document).on("change", JsReglas.Controles.ddlAtributosValidosCategoríaRegla, function () {

    let idCategoria = $(this).val();
    if (idCategoria != 0) {
        JsReglas.Consultas.ConsultaDetallesCategoria(idCategoria);
    }

});

$(function () {


    let indicadorHabilitado = $(JsReglas.Controles.ddlIndicadorRegla).val();

    if ($("#TableReglaDesagregacion").length > 0) {
        JsReglas.Consultas.ConsultaListaReglas();
    }
    else if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Editar) {

        $(JsReglas.Controles.txtCodigo).prop("disabled", true);

        if (indicadorHabilitado == "") {
            $(JsReglas.Controles.ddlIndicadorRegla).prop("disabled", false);
        } else {
            $(JsReglas.Controles.ddlIndicadorRegla).prop("disabled", true);
        }

        JsReglas.Metodos.ValidarOpcionSiguiente();
    }
    else if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Clonar) {

        if (indicadorHabilitado == "") {
            $(JsReglas.Controles.ddlIndicadorRegla).prop("disabled", false);
        } else {
            $(JsReglas.Controles.ddlIndicadorRegla).prop("disabled", true);
        }

        JsReglas.Metodos.ValidarOpcionSiguiente();
    }
    if ($(JsReglas.Controles.FormularioDetalle).length > 0) {
        JsReglas.Consultas.ConsultaListaDetalleReglas();
        JsReglas.Metodos.ValidarOpcionSiguiente();
    }

});
