
JsEditarFormularioWeb = {

    "Controles": {

        //CONTROLES PANTALLA PRINCIPAL EDITAR FORMULARIO
        "TablaEditarRegistroIndicador": "#TablaEditarRegistroIndicador tbody",
        "btnEdit": "#TablaEditarRegistroIndicador tbody tr td .btn-edit",
        "btndescarga": "#TablaEditarRegistroIndicador tbody tr td .btn-download",
        "btnCancela": "#btnCancelaFormularioWeb",

        //CONTROLES PANTALLA EDITAR FORMULARIO
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",

        "tabActivoRegistroIndicador": "div.tab-pane.active",

    },
    "Variables": {

        "ListadoRegistrosIndicador": [],
        
    },
    "Mensajes": {
        Mensaje: "Area para mensajes",
    },

    "Metodos": {

        "CargarTablaRegistroIndicador": function () {

            EliminarDatasource();

            let html = "";

            for (var i = 0; i < JsEditarFormularioWeb.Variables.ListadoRegistrosIndicador.length; i++) {

                let RegistroIndicador = JsEditarFormularioWeb.Variables.ListadoRegistrosIndicador[i];

                html = html + "<tr>";
                html = html + "<td>" + RegistroIndicador.Fuente.Fuente + "</td>";
                html = html + "<td>" + RegistroIndicador.Solicitud.Nombre + "</td>";
                html = html + "<td>" + RegistroIndicador.Anno + "</td>";
                html = html + "<td>" + RegistroIndicador.Mes + "</td>";
                html = html + "<td>" + RegistroIndicador.Formulario + "</td>";
                html = html + "<td>" + RegistroIndicador.EstadoRegistro.Nombre + "</td>";
                html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' value=" + RegistroIndicador.FormularioId + " title='Editar' class='btn-icon-base btn-edit'></button>";
                html = html + "<button type='button' data-toggle='tooltip' data-placement='top' value=" + RegistroIndicador.FormularioId + " title='Descargar' class='btn-icon-base btn-download'></button></td>";
                html = html + "</tr>"
            }

            $(JsEditarFormularioWeb.Controles.TablaEditarRegistroIndicador).html(html);

            CargarDatasource();

        },
   
    },

    "Consultas": {

        "ConsultaListaRegistroIndicador": function () {

            $("#loading").fadeIn();
            execAjaxCall("/EditarFormulario/ObtenerListaRegistroIndicador", "GET")
                .then((obj) => {
                    JsEditarFormularioWeb.Variables.ListadoRegistrosIndicador = obj.objetoRespuesta;
                    JsEditarFormularioWeb.Metodos.CargarTablaRegistroIndicador();
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

    }

}

//METODO PARA CARGAR LA CANTIDAD DE FILAS DEL EDICADOR
$(document).on("keypress", JsEditarFormularioWeb.Controles.txtCantidadRegistroIndicador, function () {

    if (event.keyCode == 13) {
        var table = $(JsEditarFormularioWeb.Controles.tablaIndicador).DataTable();
        table.clear().draw();

        let tabActual = getTabActivoRegistroIndicador();

        JsEditarFormularioWeb.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual] = [];

        if ($(this).val() != 0 || $(this).val().trim() != "") {
            for (let x = 0; x < $(this).val(); x++) {
                let listaColumnasVariablesDato = [];

                $(JsEditarFormularioWeb.Controles.columnasTablaIndicador).children().each(function (index) {
                    if ($(this).attr('class').includes("highlighted")) {
                        listaColumnasVariablesDato.push(1);
                    }
                    else if ($(this).attr('class').includes("name-col")) {
                        listaColumnasVariablesDato.push(
                            JsEditarFormularioWeb.Controles.InputText(`inputText-${tabActual}-${x}-${index}`, "Nombre"));
                    }
                    else if ($(this).attr('class').includes("date-col")) {
                        listaColumnasVariablesDato.push(
                            JsEditarFormularioWeb.Controles.InputDate(`inputDate-${tabActual}-${x}-${index}`));
                    }
                    else {
                        listaColumnasVariablesDato.push(
                            JsEditarFormularioWeb.Controles.InputSelect2(
                                `inputSelect-${tabActual}-${x}-${index}`,
                                '<option value="1">Opción 1</option><option value="2">Opción 2</option>'));
                    }
                });
                table.row.add(listaColumnasVariablesDato).draw(false);

                $(JsEditarFormularioWeb.Controles.btnDescargarPlantillaRegistro).prop("disabled", false);
                $(JsEditarFormularioWeb.Controles.btnCargarPlantillaRegistro).prop("disabled", false);
            }

            JsEditarFormularioWeb.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual].push(0);

            setSelect2();

            eventNextPrevDatatable();
        }
    }
});

$(document).on("click", JsEditarFormularioWeb.Controles.btndescarga, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
                .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/Index" });
        });
});


$(document).on("click", JsEditarFormularioWeb.Controles.btnCancela, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        })
});


$(document).on("click", JsEditarFormularioWeb.Controles.btnEdit, function () {
    let id = 1;

    window.location.href = "/EditarFormulario/Edit?id="+id;       
});

$(function () {

    let modo = $.urlParam("modo");

    if ($(JsEditarFormularioWeb.Controles.TablaEditarRegistroIndicador).length > 0) {
        JsEditarFormularioWeb.Consultas.ConsultaListaRegistroIndicador();
    }

});









