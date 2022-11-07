jsRegistroIndicadorFonatel = {
    "Controles": {
        "tabRegistroIndicador": (id) =>  `#Tab${id} a`,
        "tabRgistroIndicadorActive":"ul .active a",




        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-edit-form",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "tabActivoRegistroIndicador": "div.tab-pane.active",
        "tablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table",
        "columnasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr",
        "table": "",
        "btnGuardarCategoría": "#btnGuardarCategoría",
        "btnDescargarPlantillaRegistro": "div.tab-pane.active #btnDescargarPlantillaRegistro",
        "btnCargarPlantillaRegistro": "div.tab-pane.active #btnCargarPlantillaRegistro",
        "inputFileCargarPlantilla": "#inputFileCargarPlantilla",

        "fileCargaRegistro": "#fileCargaRegistro",
        "btnCancelar": "#btnCancelarRegistroIndicador",

        "btnGuardar": "div.tab-pane.active #btnGuardarRegistroIndicador",
        "btnValidar": "div.tab-pane.active #btnValidarRegistroIndicador",

        "btnCargarPlantillaRegistro2": "#btnCargarPlantillaRegistro2",
        "btnGuardarRegistroIndicador2": "#btnGuardarRegistroIndicador2",
        "btnValidar2":"#btnValidarRegistroIndicador2",

        "IndicadorCorrecto": "#Indicador1",
        "IndicadorErroneo": "#Indicador2",
        "btnCarga": "#btnCargaRegistroIndicador",
        "btnCargaRegistroIndicador": "#btnCargaRegistroIndicador",
        "btnCargaRegistroIndicadorEdicion": "#btnCargaRegistroIndicadorEdicion",
        "InputSelect2": (id, option) => `<div class="select2-wrapper">
                                                    <select class="listasDesplegables" id="${id}" >
                                                    <option></option>${option}</select ></div >`,
        "InputDate": id => `<input type="date" class="form-control form-control-fonatel" id="${id}">`,
        "InputText": (id, placeholder) => `<input type="text" aria-label="${placeholder}" class="form-control form-control-fonatel alfa_numerico" id="${id}" placeholder="${placeholder}" style="min-width:150px;">`,

    },
    "Variables": {
        "VariableIndicador": 1,
        "Validacion": false,
        "paginasActualizadasConSelect2_tablaIndicador": {}
    },

    "Metodos": {

        "CargarDatosVisualizar": function () {
            $('.container button').prop('disabled', true);
            $('textarea').prop('disabled', true);
        },

        "CargarExcel": function () {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
        },

    },
    "Consultas": {
        "ConsultaRegistroIndicadorDetalle": function () {
            $("#loading").fadeIn();

            let detalleIndicadorFonatel = new Object();
            detalleIndicadorFonatel.IdSolicitudString = ObtenerValorParametroUrl("idSolicitud");
            detalleIndicadorFonatel.IdFormularioString = ObtenerValorParametroUrl("idFormulario");
            detalleIndicadorFonatel.IdIndicadorString = $(jsRegistroIndicadorFonatel.Controles.tabRgistroIndicadorActive).attr('data-Indicador');
            detalleIndicadorFonatel.CantidadFilas = $(jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador).val();






            execAjaxCall("/RegistroIndicadorFonatel/ConsultaRegistroIndicadorDetalle", "POST", detalleIndicadorFonatel = detalleIndicadorFonatel)
                .then((obj) => {

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


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index";
        });
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnGuardar, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido guardado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index"; });
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
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar la carga de la información?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La carga de información ha sido completada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index"; });
        });
});



//DESCARGAR EXCEL
$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            window.open(jsUtilidades.Variables.urlOrigen + "/RegistroIndicadorFonatel/DescargarExcel");
            jsRegistroIndicadorFonatel.Metodos.CargarExcel();
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
    jsMensajes.Metodos.OkAlertModal("El Formulario ha sido cargado")
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
    if (jsRegistroIndicadorFonatel.Variables.Validacion == false) {

        jsMensajes.Metodos.OkAlertModal("Validación Exitosa <br><br> La información ingresada cumple con los criterios de validación.");
        $(jsRegistroIndicadorFonatel.Controles.btnCarga).prop("disabled", false);

    } else {
        jsMensajes.Metodos.OkAlertErrorModal("Fórmula de cambio mensual <br><br> La información ingresada no es congruente con el registro del mes anterior");
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



        jsRegistroIndicadorFonatel.Consultas.ConsultaRegistroIndicadorDetalle();







        //$(jsRegistroIndicadorFonatel.Controles.tablaIndicador).removeClass("hidden");

        //if (jsRegistroIndicadorFonatel.Variables.Validacion == false) {

        //    $(jsRegistroIndicadorFonatel.Controles.btnValidar).prop("disabled", false);
        //    $(jsRegistroIndicadorFonatel.Controles.btnGuardar).prop("disabled", false);


        //}else {

        //    $(jsRegistroIndicadorFonatel.Controles.btnGuardarCategoría).prop("disabled", false);
        //    $(jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro2).prop("disabled", false);
        //    $(jsRegistroIndicadorFonatel.Controles.btnGuardarRegistroIndicador2).prop("disabled", false);
        //    $(jsRegistroIndicadorFonatel.Controles.btnValidar2).prop("disabled", false);
       

        //}
        //EliminarDatasource(jsRegistroIndicadorFonatel.Controles.tablaIndicador);

        //CargarDatasourceV2(jsRegistroIndicadorFonatel.Controles.tablaIndicador);
        //var table = $(jsRegistroIndicadorFonatel.Controles.tablaIndicador).DataTable();
        //table.clear().draw();

        //let tabActual = getTabActivoRegistroIndicador();

        //jsRegistroIndicadorFonatel.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual] = [];

        //if ($(this).val() != 0 || $(this).val().trim() != "") {
        //    for (let x = 0; x < $(this).val(); x++) {
        //        let listaColumnasVariablesDato = [];

        //        $(jsRegistroIndicadorFonatel.Controles.columnasTablaIndicador).children().each(function (index) {
        //            if ($(this).attr('class').includes("highlighted")) {
        //                listaColumnasVariablesDato.push(1);
        //            }
        //            else if ($(this).attr('class').includes("name-col")) {
        //                listaColumnasVariablesDato.push(
        //                    jsRegistroIndicadorFonatel.Controles.InputText(`inputText-${tabActual}-${x}-${index}`, "Nombre"));
        //            }
        //            else if ($(this).attr('class').includes("date-col")) {
        //                listaColumnasVariablesDato.push(
        //                    jsRegistroIndicadorFonatel.Controles.InputDate(`inputDate-${tabActual}-${x}-${index}`));
        //            }
        //            else {
        //                listaColumnasVariablesDato.push(
        //                    jsRegistroIndicadorFonatel.Controles.InputSelect2(
        //                        `inputSelect-${tabActual}-${x}-${index}`,
        //                        '<option value="1">Opción 1</option><option value="2">Opción 2</option>'));
        //            }
        //        });
        //        table.row.add(listaColumnasVariablesDato).draw(false);

        //        // Este método es de visualizar en el modulo Formulario Web, es de la Etapa de diseño
        //        //SE ENCUENTRA QUEMADO DE MOMENTO POR CONTRATIEMPOS DE ENTRAGA SIMEF - ANDERSON
        //        let modo =ObtenerValorParametroUrl('modo');
        //        if (modo != '6') {
        //            $(jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro).prop("disabled", false);
        //            $(jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro).prop("disabled", false);
        //        }
        //        if (modo == '6') {
        //            jsRegistroIndicadorFonatel.Metodos.CargarDatosVisualizar();
        //        }
        //    }

        //    jsRegistroIndicadorFonatel.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual].push(0);

        //}
        
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



$(document).ready(function () {



    $(jsRegistroIndicadorFonatel.Controles.tabRegistroIndicador(1)).click();
    console.log($(jsRegistroIndicadorFonatel.Controles.tabRegistroIndicador(1)).attr('data-Indicador'));






    ////BLOQUEO DE BOTONES HAY QUE REVISAR PORQUE NO FUNCIONAN CON LA CLASE .ACTIVE APARTIR DEL SEGUNDO TAB - FRANCISCO VINDAS
    //$(jsRegistroIndicadorFonatel.Controles.btnValidar).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnGuardar).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnCarga).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnGuardarCategoría).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro2).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnGuardarRegistroIndicador2).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnValidar2).prop("disabled", true);

    //    let modo =ObtenerValorParametroUrl('modo');
    //    if (modo == '6') {
    //        $("#loading").fadeIn();
    //        jsRegistroIndicadorFonatel.Metodos.CargarDatosVisualizar();
    //        $("#loading").fadeOut();
    //    }
 });
