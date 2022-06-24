function functCrear() {
    $("#formCreate").submit();
}


$(document).ready(function () {
    function onSuccess(data) {

        data = JSON.parse(data);
        if (data.ok == "True") {

            // esperar y volver a ejecutar el refresh de la tabla por AJAX

           //
           // window.location = "/Constructor/Editar?id=" + data.data.IdConstructor;
           
            if (data.state == 1) {
                addSuccess({ msg: "La información se agregó con éxito" })
                $('#divTabCriterio').removeClass('hidden');
                $('#constructorTab a[href="#criterio"]').tab('show')
                $('#btnBuscarIndicador').addClass('hidden');
                $('#ddlDireccion').addClass('hidden');
                $('#txtDireccion').addClass('hidden');
               
                $("#IdConstructor").val(data.data.IdConstructor);
                $("#IdDireccion").val(data.data.IdDireccion);
                $("#divMensajeErroCrearConstructor").hide()
                funcMostrarMensaje("Proceda asociar los criterios al indicador", "Informativo");
            } else {
                addSuccess({ msg: "La información se modificó con éxito" })
                window.location = "/Constructor";
            }


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
    // pasa el id del form de crear
    // pasa la función a ejecutar si todo está bien
    // pasa la función a ejecutar si hay errores
    AjaxifyMyForm("formCreate",
        onSuccess,
        onFailure);
});

