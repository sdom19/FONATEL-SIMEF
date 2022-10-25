jQuery(document).ready(function () {
  /*  jQuery("#right-nav").html('<li style="height: 20px;width: 80%;margin-left: 10%;"><a href="./"><i class="bi bi-house-door"><img alt="Ir a inicio" class="home_icon" height="25" src="Sutel/image/home-icon.png" width="25"></i></a></li><li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadoresFonatel.html" style=" height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadoresFonatel.html" style="height: 80px;width: 80%;margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #006671;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadoresFonatel.html"  style="height: 80px;width: 80%; margin-left: 10%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');*/
  jQuery("#right-nav").html('<li style="text-align: center;padding-top: auto;"><a href="definiciones.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">CATÁLOGO<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="TabladescargaIndicadoresFonatel.html" style=" height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;">TABLAS<figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="graficosIndicadoresFonatel.html" style="height: 80px;width: 100%;margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 30px;background: #006671;font-weight: bold !important;"> GRÁFICOS <figcaption class="navbar-rigth"></figcaption></figure></a></li><li style="text-align: center;padding-top: auto;"><a href="descargaIndicadoresFonatel.html"  style="height: 80px;width: 100%; margin-left: 0%;"><figure class="effect-bubba" style="height: 80px;margin-top: 0px !important;padding-top: 20px;background: #449ca4;font-weight: bold !important;">INFORMES<br>INTERACTIVOS<figcaption class="navbar-rigth"></figcaption></figure></a></li>');
    jQuery("#cbMes0, #cbMes1").change(function () {
        var idServ = jQuery("#selectServicio option:selected").val();
        var idTem = jQuery("#selectTematica option:selected").val();
        var idOp = jQuery("#selectOperador option:selected").val();
        var idInd = jQuery("#selectIndicador option:selected").val();
        var idCri = jQuery("#selectCriterio option:selected").val();

        var idU = "";//jQuery("#txUsuario").val();
        var idA0 = jQuery("#cbAno0").val();
        var idMes0 = jQuery("#cbMes0").val();

        var idA1 = jQuery("#cbAno1").val();
        var idMes1 = jQuery("#cbMes1").val();

        if (idA0 == -1 || idA1 == -1 || idMes0 == -1 || idMes1 == -1 || idMes0 == null || idMes1 == null)
        jQuery("#add").attr("disabled", true);
    else {
            jQuery("#add").attr("disabled", false);
            jQuery("#add").click(function () {
                window.location.href = `http://localhost/ApiDescarga/API/Tabla?idCriterio=${idCri}&listaIndicadores=${el_cri}&idOperador=${idOp}&anoInic=${idA0}&anoFin=${idA1}&mesInic=${idMes0}&mesFin=${idMes1}`;
                ;
            });
        }
    });
});