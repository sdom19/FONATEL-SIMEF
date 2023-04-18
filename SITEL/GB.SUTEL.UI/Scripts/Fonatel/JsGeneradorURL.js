jsGeneradorURL = {
    Controles: {
        tablaIndicador: "#tableIndicador tbody",
        "chkDatos": ".chkDatos",
        "btnGenerarURL": "#btnGenerarURL",
        "txtURL": "#txtURL",
        "txtRuta": "#txtRuta",
        "btnCopiar": "#btnCopiar",
        "btnCancelar": "#btnCancelar",
    },
    Variables: {
        listadoIndicadores: [],
        rutaOriginalURL: "",
    },
    Mensajes: {
        preguntaCancelarAccion: "¿Desea cancelar la acción?",
    },
    Metodos: {
        CargarTablaIndicadores: function () {
            $("#loading").fadeIn();

            jsGeneradorURL.Consultas.ConsultaListaIndicadores()
                .then(data => {
                    jsGeneradorURL.Metodos.InsertarDatosTablaIndicadores(data.objetoRespuesta);
                })
                .catch(error => {
                    jsMensajes.Metodos.OkAlertErrorModal()
                        .set('onok', function (closeEvent) { });
                })
                .finally(() => {
                    $("#loading").fadeOut();
                });
        },

        InsertarDatosTablaIndicadores: function (listaIndicadores) {
            EliminarDatasource();
            let html = "";

            listaIndicadores?.forEach(item => {
                html += "<tr>";
                html += `<th><input type='checkbox' name='${item.Codigo}-${item.Nombre}' class='chkDatos' /></th>`
                html += `<th scope='row'>${item.Codigo}</th>`;
                html += `<th scope='row'>${item.Nombre}</th>`;
                html += `<th scope='row'>${item.GrupoIndicadores.Nombre}</th>`;
                html += `<th scope='row'>${item.TipoIndicadores.Nombre}</th>`;
                html += `<th scope='row'>${item.EstadoRegistro.Nombre}</th>`;
                html += "</tr>";
            });
            $(jsGeneradorURL.Controles.tablaIndicador).html(html);
            CargarDatasource();
        },
        CrearURL: function () {
            $(jsGeneradorURL.Controles.txtURL).val($(jsGeneradorURL.Controles.txtRuta).val()+jsGeneradorURL.Variables.listadoIndicadores.join(","));
        }
    },
    Consultas: {
        ConsultaListaIndicadores: function () {
            return execAjaxCall('/IndicadorFonatel/ObtenerListaIndicadoresparaGerarURl', 'GET');
        },
    }
}

$(document).ready(function () {
    jsGeneradorURL.Metodos.CargarTablaIndicadores();
    jsGeneradorURL.Variables.rutaOriginalURL = $(jsGeneradorURL.Controles.txtURL).val();
});

$(document).on("click", jsGeneradorURL.Controles.btnGenerarURL, function () {
    jsGeneradorURL.Metodos.CrearURL();
});

$(document).on("click", jsGeneradorURL.Controles.btnCopiar, function () {
    debugger;
    // Crea un campo de texto "oculto"
    var aux = document.createElement("input");

    // Asigna el contenido del elemento especificado al valor del campo
    aux.setAttribute("value", document.getElementById("txtURL").value);

    // Añade el campo a la página
    document.body.appendChild(aux);

    // Selecciona el contenido del campo
    aux.select();

    // Copia el texto seleccionado
    document.execCommand("copy");

    // Elimina el campo de la página
    document.body.removeChild(aux);
});

$(document).on("click", jsGeneradorURL.Controles.chkDatos, function () {
    if (this.checked) {
       jsGeneradorURL.Variables.listadoIndicadores.push(this.name);
    } else {
        jsGeneradorURL.Variables.listadoIndicadores = jsGeneradorURL.Variables.listadoIndicadores.filter(x => x != this.name);
    }
});


$(document).on("click", jsGeneradorURL.Controles.btnCancelar, function (e) {
    e.preventDefault();
    $(jsGeneradorURL.Controles.txtURL).val(jsGeneradorURL.Variables.rutaOriginalURL);
});