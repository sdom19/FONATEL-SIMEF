JsReglas = {

    "Controles": {
        "ddlVariableRegla": "#ddlVariableRegla",
        "ddlTipoRegla": "#ddlTipoRegla",
        "btnGuardarRegla": "#btnGuardarRegla",
        "btnCancelar": "#btnCancelarRegla",
        "btnSiguienteRegla": "#btnSiguienteRegla",
        "btnEditarRegla": "#TableReglaDesagregacion tbody tr td .btn-edit",
        "btnClonarRegla": "#TableReglaDesagregacion tbody tr td .btn-clone",
        "btnBorrarRegla": "#TableReglaDesagregacion tbody tr td .btn-delete",
        "btnEliminaTipoRegla": "#TableTipoRegla tbody tr td .btn-delete",
        "btnEditTipoRegla": "#TableTipoRegla tbody tr td .btn-edit",
        "btnAddRegla": "#TableReglaDesagregacion tbody tr td .btn-add",
        "btnRemoveReglaDetalle": "#TableReglaDesagregacionDetalle tbody tr td .btn-delete",
        "divFormulaCambioMensual": "#divFormulaCambioMensual",
        "divFormulaContraIndicador": "#divFormulaContraIndicador",
        "divFormulaContraIndicadorSalida": "#divFormulaContraIndicadorSalida",
        "divFormulaContraIndicadorEntradaSalida": "#divFormulaContraIndicadorEntradaSalida",
        "divFormulaContraConstante": "#divFormulaContraConstante",
        "divFormulaContraAtributosValido": "#divFormulaContraAtributosValido",
        "divFormulaActualizacionSecuencial": "#divFormulaActualizacionSecuencial",
        "divContenedor": ".contenedor_regla",
        "btnAtrasRegla": "#btnAtrasTipoRegla",
        "btnSiguienteTipoSiguiente": "#btnSiguienteTipoSiguiente",
        "btnGuardarReglaTipo": "#btnGuardarReglaTipo",
        "TablaReglas": "#TableReglaDesagregacion tbody",
        "TablaDetalleReglas": "#TableTipoRegla tbody",
        "CodigoHelp": "#CodigoHelp",
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
        "ddlIndicadorComparacionRegla": "#ddlIndicadorComparacionRegla",
        "ddlVariableComparacionRegla": "#ddlVariableComparacionRegla",
        "txtConstanteRegla": "#txtConstanteRegla",
        "ddlAtributosValidosCategoríaRegla": "#ddlAtributosValidosCategoríaRegla",
        "ddlAtributosValidosRegla": "#ddlAtributosValidosRegla",
        "ddlCategoríaActualizableRegla": "#ddlCategoríaActualizableRegla",
        "ddlIndicadorSalidaRegla": "#ddlIndicadorSalidaRegla",
        "formularioReglasInput": "#formularioReglas input, textarea",
        "formularioReglasSelect": "#formularioReglas select",
        "chkAtributosValidosRegla": "#chkAtributosValidosRegla",
        "FormularioDetalle": "#FormularioCrear",
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
        "ListaDetalleReglas": []
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
                    $(JsReglas.Controles.divFormulaContraIndicador).removeClass("hidden");
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

                html = html + "<td scope='row'>" + detalleReglas.detalleIndicadorVariables.NombreVariable + "</td>";
                html = html + "<td>" + detalleReglas.tipoReglaValidacion.Nombre + "</td>";
                html = html + "<td>" + detalleReglas.operadorArismetico.Nombre + "</td>";

                html = html + "<td><button type='button' data - toggle='tooltip' data - placement='top' value = '" + detalleReglas.id + "' title = 'Editar' class='btn-icon-base btn-edit' ></button>" +
                    "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' value = '" + detalleReglas.id + "' class='btn-icon-base btn-delete'></button></td>";

                html = html + "</tr>"
            }
            $(JsReglas.Controles.TablaDetalleReglas).html(html);
            CargarDatasource();
        },

        "CargarDatosSetp2": function (id) {
            if (id == 1) {
                $(JsReglas.Controles.ddlTipoRegla).val(6).change();
                $(JsReglas.Controles.ddlVariableRegla).val(1).change();
                $(JsReglas.Controles.ddlOperadorRegla).val(4).change();
            }
            else {
                $(JsReglas.Controles.ddlTipoRegla).val(5).change();
                $(JsReglas.Controles.ddlVariableRegla).val(2).change();
                $(JsReglas.Controles.ddlOperadorRegla).val(5).change();
            }
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
            $(JsReglas.Controles.ddlVariableComparacionRegla).val(null).trigger('change');
        },

        "RestablecerCamposStep2": function () {
            $(JsReglas.Controles.ddlTipoRegla).val(null).trigger('change');
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
            $(JsReglas.Controles.ddlVariableComparacionRegla).val(null).trigger('change');
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

            let Tipo = $(JsReglas.Controles.ddlTipoRegla).val().trim();
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
                if ($(JsReglas.Controles.ddlIndicadorComparacionRegla).val() == 0 || $(JsReglas.Controles.ddlIndicadorComparacionRegla).val() == null) {
                    $(JsReglas.Controles.ddlIndicadorComparacionHelp).removeClass("hidden");
                    validarTipo = false;
                }
                if ($(JsReglas.Controles.ddlVariableComparacionRegla).val() == 0 || $(JsReglas.Controles.ddlVariableComparacionRegla).val() == null) {
                    $(JsReglas.Controles.ddlVariableComparacionReglaHelp).removeClass("hidden");
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
            $(JsReglas.Controles.btnSiguienteRegla).prop("disabled", validar);
        },

        "ValidarNombreyCodigo": function () {
            let validar = true;
            $(JsReglas.Controles.TipoReglaHelp).addClass("hidden");
            $(JsReglas.Controles.OperadorHelp).addClass("hidden");

            let Codigo = $(JsReglas.Controles.txtCodigo).val().trim();
            let Nombre = $(JsReglas.Controles.txtNombre).val().trim();

            if (Codigo.length == 0) {
                $(JsReglas.Controles.CodigoHelp).removeClass("hidden");
                validar = false;
            }
            if (Nombre.length == 0) {
                $(JsReglas.Controles.nombreHelp).removeClass("hidden");
                validar = false;
            }
            return validar;
        }
    },

    "Consultas": {

        "EliminarRegla": function (IdRegla) {
            $("#loading").fadeIn();
            let objRegla = new Object()
            objRegla.id = IdRegla;
            execAjaxCall("/ReglasValidacion/EliminarRegla", "POST", objRegla)
                .then((data) => {
                    jsMensajes.Metodos.OkAlertModal("La Regla de Validación ha sido eliminada ")
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
                        JsReglas.Consultas.EliminarFormulario(IdRegla);
                    } else {
                        let dependencias = obj.objetoRespuesta[0] + "<br>"
                        jsMensajes.Metodos.ConfirmYesOrNoModal("La Regla de Validación ya está en uso en los<br>" + dependencias + "<br>¿Desea Eliminar?", jsMensajes.Variables.actionType.eliminado)
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
            objRegla.IdReglasValidacionTipoString = idDetalleRegla;
            execAjaxCall("/ReglasValidacion/EliminarDetalleRegla", "POST", objRegla)
                .then((data) => {
                    jsMensajes.Metodos.OkAlertModal("La Tipo de Regla ha sido eliminado")
                        .set('onok', function (closeEvent) { window.location.href = "/Fonatel/ReglasValidacion/Index" });
                }).catch((data) => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { location.reload(); });
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

            $.ajax({
                url: jsUtilidades.Variables.urlOrigen + '/ReglasValidacion/ObtenerListaReglasValidacion',
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                beforeSend: function () {
                    $("#loading").fadeIn();
                },
                success: function (obj) {
                    if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                        JsReglas.Variables.ListaReglas = obj.objetoRespuesta;
                        JsReglas.Metodos.CargarTablaReglas();
                    } else if (obj.HayError == jsUtilidades.Variables.Error.ErrorSistema) {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { });
                    }
                    else {
                        jsMensajes.Metodos.OkAlertErrorModal()
                            .set('onok', function (closeEvent) { })
                    }
                    $("#loading").fadeOut();
                }
            }).fail(function (obj) {

                jsMensajes.Metodos.OkAlertErrorModal()
                    .set('onok', function (closeEvent) { })
                $("#loading").fadeOut();
            })
        },

        "ConsultaListaDetalleReglas": function () {


            let IdRegla =ObtenerValorParametroUrl("id");
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

        "AgregarRegla": async function (parcial) {
            $("#loading").fadeIn();
            let objetoRegla = new Object()
            objetoRegla.Codigo = $(JsReglas.Controles.txtCodigo).val();
            objetoRegla.Nombre = $(JsReglas.Controles.txtNombre).val();
            objetoRegla.idIndicador = $(JsReglas.Controles.ddlIndicadorRegla).val();
            objetoRegla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val();

            if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Clonar) {
                objetoRegla.id = "";
            }
            else {
                objetoRegla.id =ObtenerValorParametroUrl("id");
            }
            execAjaxCall("/ReglasValidacion/AgregarRegla", "POST", objetoRegla)
                .then((obj) => {
                    if (parcial) {
                        let mensaje = "La Regla ha sido creada";
                        if (objetoRegla.id != null) {
                            mensaje = "La Regla ha sido editada";
                        }
                        jsMensajes.Metodos.OkAlertModal(mensaje)
                            .set('onok', function (closeEvent) {
                                window.location.href = "/Fonatel/ReglasValidacion/Index";
                            });
                    }
                    else {
                        $(JsReglas.Controles.IdRegla).val(obj.objetoRespuesta[0].id);
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

        "AgregarReglaSiguiente": async function (parcial) {
            $("#loading").fadeIn();

            let objetoRegla = {};
            objetoRegla.id = ObtenerValorParametroUrl("id");
            objetoRegla.Codigo = $(JsReglas.Controles.txtCodigo).val();
            objetoRegla.Nombre = $(JsReglas.Controles.txtNombre).val();
            objetoRegla.idIndicador = $(JsReglas.Controles.ddlIndicadorRegla).val();
            objetoRegla.Descripcion = $(JsReglas.Controles.txtDescripcionRegla).val();

            execAjaxCall("/ReglasValidacion/AgregarRegla", "POST", objetoRegla)
                .then((obj) => {
                    
                    InsertarParametroUrl("id", obj.objetoRespuesta[0].id);
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

        "AgregarDetalleRegla": async function (parcial) {
            $("#loading").fadeIn();
            let objetoTipoRegla = new Object()
            objetoTipoRegla.idOperador = $(JsReglas.Controles.ddlOperadorRegla).val();
            objetoTipoRegla.idIndicadorVariableString = $(JsReglas.Controles.ddlVariableRegla).val();
            objetoTipoRegla.IdTipo = $(JsReglas.Controles.ddlTipoRegla).val();

            if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Clonar) {
                objetoTipoRegla.id = "";
            }
            else {
                objetoTipoRegla.id =ObtenerValorParametroUrl("id");
            }
            execAjaxCall("/ReglasValidacion/AgregarDetalleRegla", "POST", objetoTipoRegla)
                .then((obj) => {
                    if (parcial) {
                        let mensaje = "El Detalle de la Regla de Validación ha sido creada";
                        if (objetoTipoRegla.id != null) {
                            mensaje = "El detalle de la Regla de Validación ha sido editada";
                        }
                        jsMensajes.Metodos.OkAlertModal(mensaje)
                            .set('onok', function (closeEvent) {

                            });
                    }
                    else {
                        $(JsReglas.Controles.IdRegla).val(obj.objetoRespuesta[0].id);
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
        }
    }
}

$(document).on("click", JsReglas.Controles.btnGuardarRegla, function (e) {
    e.preventDefault();
    let CamposVacios = "Existen campos vacíos. "
    if (JsReglas.Metodos.ValidarNombreyCodigo()) {
        if (JsReglas.Metodos.ValidarCampos()) {
            CamposVacios = ""
        }
        jsMensajes.Metodos.ConfirmYesOrNoModal(CamposVacios + "¿Desea realizar un guardado parcial de la Regla?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsReglas.Consultas.AgregarRegla(true);
            })
            .set('oncancel', function (closeEvent) {
                if (JsReglas.Metodos.ValidarControles()) {
                    JsReglas.Consultas.AgregarRegla(true);
                }
            });
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
    if (JsReglas.Metodos.ValidarControles()) {
        JsReglas.Consultas.AgregarReglaSiguiente();
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

//Step Tipos de Regla

$(document).on("click", JsReglas.Controles.btnGuardarReglaTipo, function (e) {
    e.preventDefault();
    if (JsReglas.Metodos.ValidarControlesTipo()) {
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar el Tipo de Regla?", jsMensajes.Variables.actionType.agregar)
            .set('onok', function (closeEvent) {
                JsReglas.Consultas.AgregarDetalleRegla(true);
            });
    };
});

$(document).on("click", JsReglas.Controles.btnSiguienteTipoSiguiente, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Regla de Validación?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Regla ha sido agregada ")
                .set('onok', function (closeEvent) {
                    window.location.href = "/Fonatel/ReglasValidacion/index"
                });
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
    let id = $(this).val();
    JsReglas.Metodos.CargarDatosSetp2(id);
});

$(document).on("change", JsReglas.Controles.ddlTipoRegla, function () {
    var selected = $(this).val();
    JsReglas.Metodos.RestablecerCampos();
    JsReglas.Metodos.HabilitarControlesTipoRegla(selected);
});

////////////////////

$(function () {
    if ($("#TableReglaDesagregacion").length > 0) {
        JsReglas.Consultas.ConsultaListaReglas();
    }
    else if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Editar) {
        $(JsReglas.Controles.txtCodigo).prop("disabled", true);
        JsReglas.Metodos.ValidarOpcionSiguiente();
    }
    else if (ObtenerValorParametroUrl("modo") == jsUtilidades.Variables.Acciones.Clonar) {
        JsReglas.Metodos.ValidarOpcionSiguiente();
    }

    if ($(JsReglas.Controles.FormularioDetalle).length > 0) {
        JsReglas.Consultas.ConsultaListaDetalleReglas();
        JsReglas.Metodos.ValidarOpcionSiguiente();
    }

});


