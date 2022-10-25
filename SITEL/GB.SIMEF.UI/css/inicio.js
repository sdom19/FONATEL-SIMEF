jQuery(document).ready(function () {
    jQuery('.grid').html('<div class="row clearfix"><div class="col m5"><div class="big-frame"><figure class="effect-bubba pulse"><img class="img-responsive" src="/image/blue-dark.png"><figcaption><h2><span>Gráfico<br>de indicadores</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="/SUTEL/Graficos">Ver más</a></figcaption></figure></div><div class="small-frame"><figure class="effect-bubba pulse blue"><img class="img-responsive" src="/image/blue.png"><figcaption><h2><span>Publicaciones</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="https://sutel.go.cr/informes-indicadores" target="_blank">Ver más</a></figcaption></figure></div></div><div class="col m5"><div class="small-frame"><figure class="effect-bubba pulse blue"><img class="img-responsive" src="/image/blue.png"><figcaption><h2><span>Definiciones</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="/SUTEL/pruebadatasoft">Ver más</a></figcaption></figure></div><div class="big-frame"><figure class="effect-bubba pulse"><img class="img-responsive" src="/image/blue-dark.png"><figcaption><h2><span>Descarga<br>de indicadores</span></h2><p style="font-size: 63.5%;">Superintendencia de Telecomunicaciones.</p><a data-target="#largemodal" data-toggle="modal" href="/SUTEL/descargaIndicadores">Ver más</a></figcaption></figure></div></div></div>');
    jQuery('#zone-content').css('margin-left', '0px');
    jQuery('#section-content-sec').css('padding-top', '0');
    jQuery('.carousel').carousel({
        fullWidth: false,
        indicators: true,
    });

    jQuery('.modal').modal();
    jQuery(".button-collapse").sideNav();

    jQuery('.side-nav li').click(() => {
        jQuery('side-nav').sideNav('hide');
    })

    console.log('Test');

});