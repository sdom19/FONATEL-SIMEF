jQuery(document).ready(function () {
    jQuery('.nav-extended > br').remove();
    jQuery('.button-collapse').next(this).remove();    
    jQuery('.grid').html('<div><h2 style="font-weight: 550;color: #007788;padding-top: 0px;margin-top: 0px;margin-bottom: 0px;">SIGITEL</h2><h3 style="font-weight: 550;color: #007788;padding-top: 0px;margin-top: 0px;">Sistema General de Indicadores de Telecomunicaciones</h3><hr style="border-width: 1px!important;border: solid;padding-top: 0px;color:#007788 !important;"/></div><br></br><div class="row clearfix"><div class="col m4"><div class="small-frame"><figure class="effect-bubba pulse blue"><img class="img-responsive" src="/Sutel/image/blue.png" alt="Fondo Azul"><figcaption><h2><span>Mercados</span></h2><p style="font-size: 63.5%;">Indicadores de la Direccion de Mercados</p><a data-target="#largemodal" data-toggle="modal" href="https://www.sutel.go.cr/indicadores" target="_blank">Ver más</a></figcaption></figure></div></div><div class="col m4"><div class="small-frame"><figure class="effect-bubba pulse blue"><img class="img-responsive" src="/Sutel/image/blue-dark.png" alt="Fondo Azul"><figcaption><h2><span>Calidad</span></h2><p style="font-size: 63.5%;">Indicadores de la Direccion de Calidad</p><a data-target="#largemodal" data-toggle="modal" href="https://www.sutel.go.cr/informes-indicadores" target="_blank">Ver más</a></figcaption></figure></div></div><div class="col m4"><div class="small-frame"><figure class="effect-bubba pulse blue"><img alt="Azul" class="img-responsive" src="/Sutel/image/blue.png"><figcaption><h2><span>Fonatel</span></h2><p style="font-size: 63.5%;">Indicadores de la Direccion de FONATEL</p><a data-target="#largemodal" data-toggle="modal" href="Fonatel/Index.html">Ver más</a></figcaption></figure></div><br></br>');
    jQuery('#zone-content').css('margin-left', '0px');
    jQuery('#section-content-sec').css('padding-top', '0');
    jQuery('.grid figure h2').css('word-spacing', '5px');
    
    jQuery('.modal').modal();
    jQuery(".button-collapse").sideNav();
    
    jQuery('.side-nav li').click(() => {
        jQuery('side-nav').sideNav('hide');
    })


});