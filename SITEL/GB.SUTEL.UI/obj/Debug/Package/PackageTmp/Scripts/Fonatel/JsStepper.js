﻿



//$(document).on("click", JsReglas.Controles.btnGuardarRegla, function (e) {
//    e.preventDefault();
//    jsMensajes.Metodos.AgregarRegistro("¿Desea agregar  la Regla de Validación?")
//        .set('onok', function (closeEvent) {
//            jsMensajes.Metodos.OkAlertModal("La Regla ha sido agregada ")
//                .set('onok', function (closeEvent) {
//                    $("a[href='#step-2']").trigger('click');
//                });
//        });
//});






$(document).on("click", ".step_navigation_indicador div a", function (e) {

    let div = $(this).parent();
    //$(".step_navigation_indicador div").removeClass('active');
    $(div).addClass('active');
    $(div).siblings().each(function () {
     if ($(this).attr("data-step") <= $(div).attr("data-step")) {
        $(this).addClass('active');
     }
     else {
         $(this).removeClass('active');
      }
    });

    let div_content = $(this).attr("href");
    $(".stepwizard-content-container").addClass('hidden');
    $(div_content).removeClass('hidden');
});


$(document).ready(function () {
    $(".stepwizard-step[data-step='0'] a").trigger('click');
});