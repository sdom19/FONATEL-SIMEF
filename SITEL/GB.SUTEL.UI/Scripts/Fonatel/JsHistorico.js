    JsHistorico= {
        "Controles": {
            "btnCancelarHistorico":"#btnCancelarHistorico",
            "btnDescargarHistorico": "#btnDescargarHistorico",
            "ddlNombreHistorico": "#ddlNombreHistorico",
            "divHistorica": "#tablaHistorica",
            "tablaHistorica": "#tablaHistorica table",
            "btnVisualizarHistorico": "#btnVisualizarHistorico",
            "btnAtrasHistorico":"#btnAtrasHistorico"
    },
    "Variables":{
        "ListaDatosHistorico":[]
    },

        "Metodos": {
            "CargarTabla": function () {
                let html = " <ul class='nav nav-tabs nav-tabs-fonatel mt-4'>";
                for (var i = 0; i < JsHistorico.Variables.ListaDatosHistorico.length; i++) {
                    let datoHistorico = JsHistorico.Variables.ListaDatosHistorico[i];
                    if (i == 0) {
                        html = html + "<li class='active'><a data-toggle='tab' href='#tab" + i + "'>" + datoHistorico.NombrePrograma + "</a></li>"
                    }
                    else {
                        html = html + "<li><a data-toggle='tab' href='#tab" + i + "'>" + datoHistorico.NombrePrograma + "</a></li>"
                    }
                    
                }
                html = html + "</ul>";

                html = html +" <div class='tab-content'>"

                for (var x = 0; x < JsHistorico.Variables.ListaDatosHistorico.length; x++) {
                    let datoHistorico = JsHistorico.Variables.ListaDatosHistorico[x];

                    if (x == 0) {
                        html = html + "<div id='tab" + x + "' class='tab-pane fade in active'>";
                    }
                    else {
                        html = html + "<div id='tab" + x + "' class='tab-pane fade'>";
                    }
                    

                    html = html + "<table id='' class='dataTable table-bordered table-condensed table-hover'><thead><tr>";
                    for (var i = 0; i < datoHistorico.DetalleDatoHistoricoColumna.length; i++) {
                        html = html + " <th>" + datoHistorico.DetalleDatoHistoricoColumna[i].Nombre + "</th>"
                    }
                    html = html + "</tr></thead><tbody>";
                    for (var numfila = 1; numfila < 20; numfila++) {
                        let fila = datoHistorico.DetalleDatoHistoricoFila.filter(x => x.NumeroFila == numfila);
                        html = html + "<tr>";
                        for (var i = 0; i < fila.length; i++) {
                            html = html + " <th>" + fila[i].Atributo + "</th>"
                        }
                        html = html + "</tr>";
                    }
                    html = html + "</tbody>"
                    html = html + "</table>";

                    html = html + "</div>";
                }

                html = html + " </div>";
                $(JsHistorico.Controles.divHistorica).html(html);
                JsHistorico.Metodos.CargarDatasource();
            },
            "CargarDatasource": function () {
                $(JsHistorico.Controles.tablaHistorica).DataTable({
                    pageLength: 25,
                    lengthMenu: [[25, 50, 100], [25, 50, 100]],
                    "dom": '<"top-position"<"subtop"Bl>f>r<"content-table"t><"bottom-position"ip><"clear">',
                    buttons: [
                        {
                            extend: 'excel',
                            text: '<i class="fa fa-file-excel-o" style="color:green;"></i>',
                            titleAttr: 'Excel',
                            autoPrint: false,
                            exportOptions: {
                                columns: ':not(.noExport)'
                            },
                        },
                        {
                            extend: 'print',
                            text: '<i class="fa fa-print" style="color:black;"></i>',
                            titleAttr: 'Imprimir',
                            autoPrint: false,
                            exportOptions: {
                                columns: ':not(.noExport)'
                            },

                        },

                    ],
                    columnDefs: [
                        {
                            searchable: false,
                            orderable: false,
                            targets: 0,
                        },
                        { "className": "dt-center", "targets": "_all" }
                    ],
                    scrollY: 450,
                    scrollX: true,
                    language: {
                        "decimal": "",
                        "emptyTable": "No hay información",
                        "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
                        "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
                        "infoFiltered": "(Filtrado de _MAX_ total entradas)",
                        "infoPostFix": "",
                        "thousands": ",",
                        "lengthMenu": "Mostrar _MENU_ Entradas",
                        "loadingRecords": "Cargando...",
                        "processing": "Procesando...",
                        "search": "Buscar:",
                        "searchPlaceholder": "",
                        "zeroRecords": "Sin resultados encontrados",

                        "paginate": {
                            "first": "Primero",
                            "last": "Ultimo",
                            "next": "Siguiente",
                            "previous": "Anterior"
                        }
                    }
                });

            },
        },
    "Consulta": {
        "ConsultaListaHistorica": function (id) {

            let datoHistorico = new Object();
            datoHistorico.Id= id;
            $("#loading").fadeIn();
            execAjaxCall("/HistoricoFonatel/ObtenerListaHistorica", "POST", datoHistorico )
                .then((obj) => {
                    JsHistorico.Variables.ListaDatosHistorico = obj.objetoRespuesta;

                    JsHistorico.Metodos.CargarTabla();
                   
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


$(document).on("click", JsHistorico.Controles.btnCancelarHistorico, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/HistoricoFonatel/Index";
        });  

});


$(document).on("click", JsHistorico.Controles.btnAtrasHistorico, function (e) {
    e.preventDefault();
    window.location.href = "/HistoricoFonatel/Index";
});



$(document).on("change", JsHistorico.Controles.ddlNombreHistorico, function (e) {

    let id = $(JsHistorico.Controles.ddlNombreHistorico).val();

    if (id == null) {
        $(JsHistorico.Controles.btnDescargarHistorico).prop("disabled", true);
        $(JsHistorico.Controles.btnVisualizarHistorico).prop("disabled", true);
    }
    else {
        $(JsHistorico.Controles.btnDescargarHistorico).prop("disabled", false);
        $(JsHistorico.Controles.btnVisualizarHistorico).prop("disabled", false);
    }


   
});





$(document).on("click", JsHistorico.Controles.btnVisualizarHistorico, function (e) {
    e.preventDefault();
    let id = $(JsHistorico.Controles.ddlNombreHistorico).val();
    window.location.href = "/Fonatel/HistoricoFonatel/Detalle?id=" + id; 
});




$(document).on("click", JsHistorico.Controles.btnDescargarHistorico, function (e) {
    e.preventDefault();
    let id = $(JsHistorico.Controles.ddlNombreHistorico).val();
        jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea generar el Reporte?", "", "Generar Registro")
            .set('onok', function (closeEvent) {
                JsHistorico.Consulta.ConsultaListaHistorica(id);
                jsMensajes.Metodos.OkAlertModal("El Reporte ha sido generado")
                    .set('onok', function (closeEvent) { location.reload() });
            });
    
   
});


$(function () {
    if ($(JsHistorico.Controles.divHistorica).length > 0) {
        let id = $.urlParam('id');
        if (id != null) {
            JsHistorico.Consulta.ConsultaListaHistorica(id);
        }
        else {
            window.location.href = "/HistoricoFonatel/Index";
        }
    }
});


