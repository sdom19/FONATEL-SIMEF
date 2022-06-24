//$(document).ready(function () {
//    $("#btnGuardar").click(function (e) {
//        debugger;
//        URL: "/ConfiguracionServicios/Guardar"
//        $("#formGuardar").submit();
//    });
//});
function verificarServicio(id1)
{
    //debugger;
    $.ajax(
         {
             type: "POST",
             traditional: true,
             url: "/ConfiguracionServicios/Verificar",
             secureuri: false,
             dataType: 'json',
             contentType: 'application/json; charset=utf-8',
             enctype: 'multipart/form-data',
             data:
                 JSON.stringify({
                     id: id1
                 }),
             success: function (data, status) {
             },
             error: function (data, status, e) {
             }
         });
}