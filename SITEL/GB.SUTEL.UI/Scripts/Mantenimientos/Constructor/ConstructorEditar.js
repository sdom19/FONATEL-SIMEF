function functEditar() {
    $("#formEditar").submit();
}

$(document).ready(function () {
    //debugger;
    function onSuccess(data) {

        data = JSON.parse(data);
        if (data.ok == "True") {

            // esperar y volver a ejecutar el refresh de la tabla por AJAX

            window.location = "/Constructor";
            addSuccess({ msg: "La información se actualizó con éxito." })


        } else {

            $("#divMensajeErroCrearConstructor").show();
            $("#idMensajeErrorCuerpoConstructor").html(data.strMensaje);
            //showError(data.strMensaje);
        }
    }
    function onFailure(error) {
        $("#divMensajeErroCrearConstructor").show();
        $("#idMensajeErrorCuerpoConstructor").html("Ocurrió un error.");
    }

   
  
    functEditarArbolDetalleAgrupacionNuevo();
  
   
    // pasa el id del form de crear
    // pasa la función a ejecutar si todo está bien
    // pasa la función a ejecutar si hay errores
    AjaxifyMyForm("formEditar",
        onSuccess,
        onFailure);



});