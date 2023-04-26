
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
        "ErrorNumericoEncategoria": (min, max) => `El valor debe encontrarse dentro del rango ${min} al ${max}`
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
                    //JsEditarFormularioWeb.Metodos.CargarTablaRegistroIndicador();
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
$(document).on("click", JsEditarFormularioWeb.Controles.btnCancela, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
                .set('onok', function (closeEvent) { window.location.href = "/" });
        })
});

$(document).on("click", JsEditarFormularioWeb.Controles.btndescarga, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario?", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
                .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/Index" });
        });
});
$(document).on("blur", ".solo_numeros", function () {
    /*    $(this).removeClass("has-error");*/
    let valorminimo = $(this).attr("min") == undefined ? 0 : parseFloat($(this).attr("min"));
    let valormaximo = $(this).attr("max") == undefined ? 1000000 : parseFloat($(this).attr("max"));
    let valorActual = $(this).val();
    if (valorActual < valorminimo || valorActual > valormaximo) {

        $(this).val('');
        /*      $(this).addClass("has-error");*/

        jsMensajes.Metodos.OkAlertErrorModal(JsEditarFormularioWeb.Mensajes.ErrorNumericoEncategoria(valorminimo, valormaximo));
    }

});



$(function () {

    let modo = $.urlParam("modo");

    if ($(JsEditarFormularioWeb.Controles.TablaEditarRegistroIndicador).length > 0) {
        JsEditarFormularioWeb.Consultas.ConsultaListaRegistroIndicador();
    }

});









