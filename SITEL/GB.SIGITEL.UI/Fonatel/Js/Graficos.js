var indicador1 = null;
var indicador2 = null;
var indicador3 = null;
var indicador4 = null;
var contadorIndicadores = 0;
jQuery(document).ready(function () {

    MostrarTextoImagen("#contenidoPantallaGraficos", 5);
    
   /*  jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="/Sutel/definiciones" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;"> DEFINICIONES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="/descargaIndicadores" style=" height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> DESCARGA<br>DE INDICADORES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="/sutel/graficosIndicadores" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> GRÁFICO <br>DE INDICADORES<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="https://www.sutel.go.cr/informes-indicadores" target="_blank" style="height: 80px;width: 80%; margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;"> PUBLICACIONES<figcaption class="navbar-rigth"></figcaption></figure></a></li>');
   jQuery("#right-nav").html('<li style="height: 20px;width: 80%;margin-left: 10%;"><a href="./"><i class="bi bi-house-door"><img alt="Ir a inicio" class="home_icon" height="25" src="Sutel/image/home-icon.png" width="25"></i></a></li><li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadoresFonatel.html" style=" height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadoresFonatel.html" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #449ca4;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadoresFonatel.html"  style="height: 80px;width: 80%; margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');*/
   jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadores.html" style=" height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadores.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #449ca4;font-weight: bold !important;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadores.html"  style="height: 80px;width: 100%; margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top:20px;background: #006671;font-weight: bold !important;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');
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
});