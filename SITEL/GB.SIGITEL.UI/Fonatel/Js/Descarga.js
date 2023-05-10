var urlAPI = jsconstantes.variables.direccionApi+'DescargaIndicadores/'; // Acá se cambia el url del api

jQuery(document).ready(function () {
    jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadores.html" style=" height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #449ca4;font-weight: bold !important;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadores.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadores.html"  style="height: 80px;width: 100%; margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;font-weight: bold !important;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');
    CargarTabla();
});

function CargarTabla(){
    var queryString = window.location.search;
    var urlParams = new URLSearchParams(queryString);

    var idIndicador = urlParams.get('indicador');
    var variable = urlParams.get('variable');
    var AnnoInicio = urlParams.get('AnnoInicio');
    var MesInicio = urlParams.get('MesInicio');
    var AnnoFin = urlParams.get('AnnoFin');
    var MesFin = urlParams.get('MesFin');
    var idCategoria = urlParams.get('idCategoria');
    var DescCategoria = urlParams.get('DescCategoria');

    var idGrupo = urlParams.get('idGrupo');
    var idTipo = urlParams.get('idTipo');

    idCategoria = (idCategoria > 0 ? idCategoria : 0);

    var ultimoDiaFin = new Date(parseInt(AnnoFin), parseInt(MesFin), 0);

    var desde = AnnoInicio + '-' + MesInicio.padStart(2,'0') + '-01';
    var hasta = AnnoFin + '-' + MesFin.padStart(2,'0') + '-' + ultimoDiaFin.getDate();

   var htmlTabla = '';

   fetch(urlAPI + 'GetDefinicion?tipo=' + idTipo +
    '&grupo='+idGrupo+
    '&indicador='+idIndicador)
    .then(res => res.json())
        .then(definicion => {
            var hilera = "";
            if(idCategoria != 0){
                hilera = ' por '+DescCategoria;
            }

            jQuery("#IndicadorNombre").append('Cantidad de '+ variable+ ' de '+definicion.nombre + hilera);
            jQuery("#Fuente").append('Fuente: '+definicion.fuente);
            jQuery("#Nota").append('Nota al pie: '+definicion.nota);

            fetch(urlAPI + 'GetResultado?IdIndicador=' + idIndicador +
            '&variable='+variable+
            '&desde='+desde+
            '&hasta='+hasta+
            '&idCategoria='+idCategoria)
            .then(res => res.json())
                .then(resultado => {

                    htmlTabla = '<tbody><tr><th></th>';

                    resultado.encabezados.forEach(element => {
                        htmlTabla += '<th>' + element + '</th>'
                    });

                    htmlTabla += '</tr>';

                    resultado.datos.forEach( dato => {
                        var nombreGrupo = dato.categoria == null ? dato.variable : dato.categoria;
                        htmlTabla += '<tr><td>' + nombreGrupo + '</td>';
                        resultado.encabezados.forEach(encabezado => {
                            var valorCategoria = dato.valores[encabezado] == undefined ? '0' : dato.valores[encabezado];
                            htmlTabla += '<td align="rigth">'+Intl.NumberFormat('es-419',{minimumFractionDigits: 2,}).format(parseFloat(valorCategoria))+'</td>'
                        });
                        htmlTabla += '</tr>';
                    });

                    htmlTabla += '<tr><td>Totales</td>';
                    resultado.encabezados.forEach(encabezado => {
                        var totalEncabezado = resultado.totales[encabezado] == undefined ? '0' : resultado.totales[encabezado];
                        htmlTabla += '<td align="rigth">'+Intl.NumberFormat('es-419',{minimumFractionDigits: 2,}).format(parseFloat(totalEncabezado))+'</td>'
                    });

                    htmlTabla += '</tr></tbody>';
                    jQuery("#tablaSigitel").append(htmlTabla);
                });
        });               
}

function descargarExcel(){
    window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('#dvData').html()));
}

function ExportarPDF() {
    html2canvas($('#dvData')[0], {
        onrendered: function (canvas) {
            var data = canvas.toDataURL();
            var docDefinition = {
                content: [{
                    image: data,
                    width: 500
                }]
            };
            pdfMake.createPdf(docDefinition).download("Tabla.pdf");
        }      
    });
}