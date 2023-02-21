var indicador1 = null;
var indicador2 = null;
var indicador3 = null;
var indicador4 = null;
var contadorIndicadores = 0;
var urlAPI = 'https://localhost:44313/api/CatalogoIndicadores/'; // Acá se cambia el url del api
var urlAPIConsulta = 'https://localhost:44313/api/DescargaIndicadores/'; // Acá se cambia el url del api

jQuery(document).ready(function () {

   /*  jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="/Sutel/definiciones" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;"> DEFINICIONES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="/descargaIndicadores" style=" height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> DESCARGA<br>DE INDICADORES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="/sutel/graficosIndicadores" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> GRÁFICO <br>DE INDICADORES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="https://www.sutel.go.cr/informes-indicadores" target="_blank" style="height: 80px;width: 80%; margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;"> PUBLICACIONES<figcaption class="navbar-rigth"></figcaption></figure></a></li>');*/
   jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #449ca4;font-weight: bold !important;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadores.html" style=" height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadores.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadores.html"  style="height: 80px;width: 100%; margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;font-weight: bold !important;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');
    var trigger = jQuery('.hamburger'), overlay = jQuery('.overlay'), isClosed = false;
    jQuery(".def-btn").click(function () {
        changeTipoIndicador(jQuery(this).text());
    });
    trigger.click(function () {
        hamburger_cross();
    });
    overlay.click(function () {
        hamburger_cross();
    });


    function hamburger_cross() {
        jQuery('#wrapper').toggleClass('toggled');
        if (isClosed == true) {
            overlay.hide();
            trigger.removeClass('is-open');
            trigger.addClass('is-closed');
            jQuery('#wrapper').removeClass('toggled');
            isClosed = false;
        } else {
            overlay.show();
            trigger.removeClass('is-closed');
            trigger.addClass('is-open');

            isClosed = true;
        }
    }

    jQuery('[data-toggle="offcanvas"]').click(function () {
        jQuery('#wrapper').toggleClass('toggled');
    });
    CargarGrupos();
    CargarTipos();
});

//Reinciar pantalla
function reiniciar() {
    location.reload();
}

//Change Servicio
function servicioChange(id) {
    debugger;
    servicioIndicador = id;
    // document.querySelector("#selectGestion").innerHTML = "";
	// document.querySelector("#selectTrafico").innerHTML = "";
	// document.querySelector("#selectGeneral").innerHTML = "";
	// document.querySelector("#selectIngreso").innerHTML = "";
    changeTipoIndicador(2);
}

//Change Temática
function changeTipoIndicador(tipoIndicador) {
    //debugger;
    // jQuery("#tile-def").text(text);
    // tipoIndicador = text.trim();
    // var expresion = "#select" + tipoIndicador;
    // jQuery(".selectIndicador").hide();
    // jQuery(expresion).show();
    //     jQuery(expresion).empty();
        //Acá se cambia el url del API para temáticas
        //https://localhost:44313/api/CatalogoIndicadores/GetDefinicionXprograma?programa=P1&tipo=Gestion'
        //url= urlAPI +''
       url = urlAPI + "GetDefinicionXprograma?" + `programa=${servicioIndicador}&tipo=${tipoIndicador}`;
         fetch(url)
            .then(res => res.json())
            .then(datos => {
                var Servicios = document.querySelector("#selectGestion");
                Servicios.innerHTML = '<option value="-1" selected="true">(Selecciona)</option>';
                for (let valor of datos) {
                    var nombre = valor.nombreIndicador;
                    Servicios.innerHTML += `<option class="optionIndicador" value="${valor.idIndicador}"> ${valor.idIndicador} - ${nombre}</option>`
                }
            })
}

//Buscar indicadores
function buscarIndicadores() {
    debugger;
    var table = "<table id='tablaIndicadores' class='table table-bordered table-striped table-hover dataTable exportable ' width='100%'><thead><tr style='background-color: #0c6e70; color: #fff;'><th>Id</th><th>Nombre</th><th>Definición</th></tr></thead><tbody>";
    table += "</tbody></table>";
    jQuery('#DivIndicadores').html(table);
    jQuery('#cursosTable').DataTable().destroy();
    jQuery("#cursosTable").empty();
    jQuery('#tablaIndicadores').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        responsive: true,
        "pagingType": "full_numbers",
        "language": {
            "search": "Buscar: ",
            "paginate": {
                "first": "Primera",
                "last": "Última",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "aria": {
                "sortAscending": ": Ordenar ascendentemente",
                "sortDescending": ": Ordenar descendentemente"
            },
            "lengthMenu": "Mostrar _MENU_ indicadores por página",
            "zeroRecords": "No hay indicadores que mostrar",
            "info": "Mostrando página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay indicadores disponibles",
            "infoFiltered": "(filtrado de un total de _MAX_ indicadores)"
        },
    });
    for (var i = 0; i < jQuery(".optionIndicador").length; i++) {
        if (jQuery(".optionIndicador")[i].selected) {
             //Acá se cambia el url del API para buscar indicadores
            url = urlAPI + "GetIndicador?" + `indicador=${jQuery(".optionIndicador")[i].value}`;

            fetch(url)
                .then(res => res.json())
                .then(datos => {
                    jQuery("#tablaIndicadores").DataTable().row.add([datos.idIndicador, datos.nombreIndicador, datos.definicionIndicador]).draw(false);

                })

        }
    }
}
//Datatable
jQuery(document).ready(function () {
    jQuery('#datatable').dataTable();
    jQuery('#datatable-keytable').DataTable({
        keys: true
    });
    jQuery('#datatable-responsive').DataTable();
    jQuery('#datatable-scroller').DataTable({
        ajax: "assets/js/datatables/json/scroller-demo.json",
        deferRender: true,
        scrollY: 380,
        scrollCollapse: true,
        scroller: true
    });
    var table = jQuery('#datatable-fixed-header').DataTable({
        fixedHeader: true
    });
});

 //Carga los grupos en la pagina
 function CargarGrupos() {
    var html = "";
    fetch(urlAPI + 'GetGrupo')
        .then(res => res.json())
        .then(datos => {
            for (let valor of datos) {
                if(valor.detalleHtml != null){
                    html = html + valor.detalleHtml;
                }
            }
            document.querySelector('#iconos').innerHTML += html
        });
}

 //Carga los tipos en la pagina
 function CargarTipos() {
    var html = "";
    fetch(urlAPI + 'GetTipo')
        .then(res => res.json())
        .then(datos => {
            for (let valor of datos) {
                if(valor.estado){
                    html += '<li onclick="changeTipoIndicador('+valor.idTipoIdicador+');" style="text-align: center;padding-top: auto;"><a class="def-btn" style="height: 40px;margin-left: 10%;"><figure class="effect-bubba" style="height: 40px;margin-top: 0px !important;padding-top: 10px;background: #006671;">'+valor.nombre.substr(0,12)+'<figcaption class="navbar-rigth"></figcaption></figure></a></li>';
                }
            }
            document.querySelector('.botonesTipo').innerHTML += html
        });
}
