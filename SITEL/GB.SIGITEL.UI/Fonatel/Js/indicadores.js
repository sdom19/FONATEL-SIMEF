jQuery(document).ready(function () {
    MostrarTituloPantalla("#tituloPantallFonatel", 2);
    jQuery('.nav-extended > br').remove();
    jQuery('.button-collapse').next(this).remove();    
    jQuery('.grid').html('<div class="row clearfix"><div class="col m5"><div class="big-frame"><figure class="effect-bubba pulse"><img class="img-responsive" src="image/blue-dark.png" alt="Fondo Azul"><figcaption><h2><span>Gráfico<br>de indicadores</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="graficosIndicadores.html">Ver más</a></figcaption></figure></div><div class="small-frame"><figure class="effect-bubba pulse blue"><img class="img-responsive" src="image/blue.png" alt="Fondo Azul"><figcaption><h2><span>INFORMES INTERACTIVOS</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="descargaIndicadores.html" target="_blank">Ver más</a></figcaption></figure></div></div><div class="col m5"><div class="small-frame"><figure class="effect-bubba pulse blue"><img alt="Azul" class="img-responsive" src="image/blue.png"><figcaption><h2><span>CATÁLOGO DE INDICADORES</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="definiciones.html">Ver más</a></figcaption></figure></div><div class="big-frame"><figure class="effect-bubba pulse"><img alt="Oscuro" class="img-responsive" src="image/blue-dark.png"><figcaption><h2><span>TABLA<br>de indicadores</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="TabladescargaIndicadores.html">Ver más</a></figcaption></figure></div></div></div>');
    jQuery('#zone-content').css('margin-left', '0px');
    jQuery('#section-content-sec').css('padding-top', '0');
    jQuery('.grid figure h2').css('word-spacing', '5px');
    
    jQuery('.carousel').carousel({
        fullWidth: false,
        indicators: true,
    });

    jQuery('.modal').modal();
    jQuery(".button-collapse").sideNav();

    jQuery('.side-nav li').click(() => {
        jQuery('side-nav').sideNav('hide');
    })

});