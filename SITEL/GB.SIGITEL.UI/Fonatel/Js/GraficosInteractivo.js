var ruta = jsconstantes.variables.direccionInformes;
jQuery(document).ready(function () {

  MostrarBlogGraficosInteractivos("#blogGraficosInteractivos", 3);

  /*  jQuery("#right-nav").html('<li style="height: 20px;width: 80%;margin-left: 10%;"><a href="./"><i class="bi bi-house-door"><img alt="Ir a inicio" class="home_icon" height="25" src="Sutel/image/home-icon.png" width="25"></i></a></li><li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadores.html" style=" height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadores.html" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadores.html"  style="height: 80px;width: 80%; margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');*/
  jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadores.html" style=" height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadores.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadores.html"  style="height: 80px;width: 100%; margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #449ca4;font-weight: bold !important;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');

});

function descargarPDF(){
    const BotonDescargarPDF = document.createElement('a');
    BotonDescargarPDF.href = ruta+'.pdf';
    BotonDescargarPDF.target = '_blank';
    BotonDescargarPDF.download = 'Informe';
    
    document.body.appendChild(BotonDescargarPDF);
    BotonDescargarPDF.click();
    document.body.removeChild(BotonDescargarPDF);
}

function descargarPPTX(){
    const BotonDescargarPPTX = document.createElement('a');
    BotonDescargarPPTX.href = ruta+'.pptx';
    BotonDescargarPPTX.target = '_blank';
    BotonDescargarPPTX.download = 'Informe';
    
    document.body.appendChild(BotonDescargarPPTX);
    BotonDescargarPPTX.click();
    document.body.removeChild(BotonDescargarPPTX);
}