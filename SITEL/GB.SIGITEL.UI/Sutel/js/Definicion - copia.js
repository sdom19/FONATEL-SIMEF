var tipoIndicador = null;
var servicioIndicador = '';
var indicador1 = null;
var indicador2 = null;
var indicador3 = null;
var indicador4 = null;
var expresion;
var url;

jQuery(document).ready(function () {
    if (jQuery(window).width() < 750)  
        jQuery("#selects").css("margin-top","30px");

    jQuery(".content").css("margin-left","280px");
    jQuery(".content").css("padding-right","330px");
    jQuery(".f1-step-icon").html('<i class="fa fa-check"></i>');
     
    jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="/graficosIndicadores" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> GRÁFICO <br>DE INDICADORES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="/definiciones" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;"> DEFINICIONES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="/descargaIndicadores" style=" height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> DESCARGA<br>DE INDICADORES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="https://sutel.go.cr/informes-indicadores" target="_blank" style="height: 80px;width: 80%; margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;"> PUBLICACIONES<figcaption class="navbar-rigth"></figcaption></figure></a></li>');
    jQuery('#tipos').html('<div class="row" style=""><div class="col-xs-6 col-sm-12"><ul class="nav" id="right-nav" style="list-style:none;list-style: none;"><li onclick="changeTipoIndicador(\'Suscripcion\');" style="text-align: center;padding-top: auto;"><a class="def-btn" style="height: 40px;margin-left: 10%;"><figure class="effect-bubba" style="height: 40px;margin-top: 0px !important;padding-top: 10px;background: #006671;"> Suscripción <figcaption class="navbar-rigth"></figcaption></figure></a></li><li onclick="changeTipoIndicador(\'Trafico\');" style="text-align: center;padding-top: auto;"><a class="def-btn" style="height: 40px; margin-left: 10%;"><figure class="effect-bubba" style="height: 40px;margin-top: 0px !important;padding-top: 10px;background: #006671;"> Tráfico<figcaption class="navbar-rigth"></figcaption></figure></a></li></ul></div><div class="col-xs-6 col-sm-12"><ul class="nav" id="right-nav" style="list-style:none;list-style: none;"><li onclick="changeTipoIndicador(\'Ingreso\');" style="text-align: center;padding-top: auto;"><a class="def-btn" style="height: 40px; margin-left: 10%"><figure class="effect-bubba" style="height: 40px;margin-top: 0px !important;padding-top: 10px;background: #006671;"> Ingreso<figcaption class="navbar-rigth"></figcaption></figure></a></li><li onclick="changeTipoIndicador(\'General\');" style="text-align: center;padding-top: auto;"><a class="def-btn" style="height: 40px; margin-left: 10%;"><figure class="effect-bubba" style="height: 40px;margin-top: 0px !important;padding-top: 10px;background: #006671;"> General<figcaption class="navbar-rigth"></figcaption></figure></a></li></ul></div></div>');
    jQuery('#servicio1').html('<figure class="effect-bubba" style="height: 130px;margin-top: 0px !important;padding-top: 20px;text-align: center;background: #355caa;"><i class="fa fa-users x5" style=" text-align: center; display: inline-block !important; font-size: 5rem !important; width: 100%;"></i><p class="p" style="margin-top: 5px;">Evolución general<br>del sector</p><figcaption class="iconos-servicios"></figcaption></figure>');
    jQuery('#servicio2').html('<figure class="effect-bubba" style="height: 130px;margin-top: 0px !important;padding-top: 20px;text-align: center;background: #7A3CAA;"><i class="fa fa-mobile-alt x5 x5" style=" text-align: center; display: inline-block !important; font-size: 5rem !important; width: 100%;"></i><p class="p" style="margin-top: 5px;">Telefonía móvil</p><figcaption class="iconos-servicios"></figcaption></figure>');
    jQuery('#servicio3').html('<figure class="effect-bubba" style="height: 130px;margin-top: 0px !important;padding-top: 20px;text-align: center;background: #F48A3B;"><i class="fa fa-phone x5" style=" text-align: center; display: inline-block !important; font-size: 5rem !important; width: 100%;"></i><p class="p" style="margin-top: 5px;">Telefonia fija</p><figcaption class="iconos-servicios"></figcaption></figure>');
    jQuery('#servicio4').html('<figure class="effect-bubba" style="height: 130px;margin-top: 0px !important;padding-top: 20px;text-align: center;background: #7CB342;"><i class="fa fa-tv x5" style=" text-align: center; display: inline-block !important; font-size: 5rem !important; width: 100%;"></i><p class="p" style="margin-top: 5px;">Televisión por suscripción</p><figcaption class="iconos-servicios"></figcaption></figure>');
    jQuery('#servicio5').html('<figure class="effect-bubba" style="height: 130px;margin-top: 0px !important;padding-top: 20px;text-align: center;background: #CA3062;"><i class="fa fa-feed x5" style=" text-align: center; display: inline-block !important; font-size: 5rem !important; width: 100%;"></i><p class="p" style="margin-top: 5px;">Internet</p><figcaption class="iconos-servicios"></figcaption></figure>');
    jQuery('#servicio6').html('<figure class="effect-bubba" style="height: 130px;margin-top: 0px !important;padding-top: 20px;text-align: center;background: #f66192;"><i class="fa fa-exchange-alt x5" style=" text-align: center; display: inline-block !important; font-size: 5rem !important; width: 100%;"></i><p class="p" style="margin-top: 5px;">Líneas dedicadas</p><figcaption class="iconos-servicios"></figcaption></figure>');
    var trigger = jQuery('.hamburger'), overlay = jQuery('.overlay'), isClosed = false;
    jQuery(".def-btn").click(function() {
        changeTipoIndicador(jQuery(this).text());
    });
    trigger.click(function () {
        hamburger_cross();
    });
    overlay.click(function () {
        hamburger_cross();
    });

    function hamburger_cross() {
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
});



function reiniciar() {
    location.reload();
}

function servicioChange(id) {
    servicioIndicador = id;
    changeTipoIndicador("Suscripcion");
}

function changeTipoIndicador(text) {
	
	
    jQuery("#tile-def").text(text);
    tipoIndicador = text.trim();
    var expresion = "#select" + tipoIndicador;
    jQuery(".selectIndicador").hide();
    jQuery(expresion).show();
    if (jQuery(expresion).val() == -1 || jQuery(expresion).val() == null || jQuery("#selectSuscripcion")[0].value == -1 ) {
        jQuery(expresion).empty();
        url = "https://apidefiniciones.sutel.go.cr/Indicadors/GetIndicadorFilter?" + `servicio=${servicioIndicador}&tipo=${tipoIndicador}`;

        fetch(url)
            .then(res => res.json())
            .then(datos => {
                var Servicios = document.querySelector(expresion);
                Servicios.innerHTML = '<option value="-1" selected="true">(Selecciona)</option>';
                for (let valor of datos) {
                    var nombre = valor.NombreIndicador;

                    //if (nombre.length > 50) {
                    //    nombre = nombre.substring(0, 50) + "...";
                    //}
                    Servicios.innerHTML += `<option class="optionIndicador" value="${valor.IdIndicador}"> ${valor.IdIndicador} - ${nombre}</option>`
                }
            })
    }
}

function buscarIndicadores() {
    var table = "<table id='tablaIndicadores' class='table table-bordered table-striped table-hover dataTable exportable ' width='100%'><thead><tr><th>Id</th><th>Nombre</th><th>Definición</th></tr></thead><tbody>";
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
            "search": "Buscar:",
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
            url = "https://apidefiniciones.sutel.go.cr/Indicadors/GetIndicador?" + `indicador=${jQuery(".optionIndicador")[i].value}`;

            fetch(url)
                .then(res => res.json())
                .then(datos => {
                    jQuery("#tablaIndicadores").DataTable().row.add([datos.IdIndicador, datos.NombreIndicador, datos.DefinicionIndicador]).draw(false);

                })

        }
    }
}
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

