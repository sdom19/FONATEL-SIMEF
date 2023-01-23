jsGeneradorURL = {
    Controles: {
        tablaIndicador: "#tableIndicador tbody",
        "chkDatos": ".chkDatos",
        "btnGenerarURL": "#btnGenerarURL",
        "txtURL": "#txtURL",
        "txtRuta": "#txtRuta",
    },
    Variables: {
        listadoIndicadores:[],
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
            return execAjaxCall('/IndicadorFonatel/ObtenerListaIndicadores', 'GET');
        },
    }
}

$(document).ready(function () {
    jsGeneradorURL.Metodos.CargarTablaIndicadores();
});

$(document).on("click", jsGeneradorURL.Controles.btnGenerarURL, function () {
    jsGeneradorURL.Metodos.CrearURL();
});

$(document).on("click", jsGeneradorURL.Controles.chkDatos, function () {
    if (this.checked) {
       jsGeneradorURL.Variables.listadoIndicadores.push(this.name);
    } else {
        jsGeneradorURL.Variables.listadoIndicadores = jsGeneradorURL.Variables.listadoIndicadores.filter(x => x != this.name);
    }
});