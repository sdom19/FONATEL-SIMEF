$(document).ready(function () {

    function onSuccess(data) {
        onsuccessAjaxCrearNivel(JSON.parse(data));
    }

    function onFailure(error) {
        onFailAjaxCrearNivel(JSON.parse(error));
    }

    function onSuccessEditar(data) {
        onsuccessAjaxEditarNivel(JSON.parse(data));
    }

    function onFailureEditar(error) {
        onFailAjaxEditarNivel(JSON.parse(error));
    }

    function onSuccessEliminar(data) {
        onsuccessAjaxEliminarNivel(JSON.parse(data));
    }

    function onFailureEliminar(error) {
        onFailAjaxEditarNivel(JSON.parse(error));
    }

    AjaxifyMyForm("formCreate",
        onSuccess,
        onFailure
        );

    
    AjaxifyMyForm("formEditar",
          onSuccessEditar,
          onFailureEditar

 );

    AjaxifyMyForm("formEliminar",
         onSuccessEliminar,
         onFailureEliminar
   
    );

  


});
//#####################################################
function onsuccessAjaxCrearNivel(data) {

    if (data.ok == 'True') {
        $("#divMensajeErrorNuevoNivel").addClass("hidden");
        $("#itemNivel_DESCNIVEL").val("");
        $("#modalCrearNivel").modal('hide');

        addSuccess({ msg: data.strMensaje });

        $("#frmFiltrarNivel")[0].reset();
        $("#frmFiltrarNivel").submit();
    } else {
        $('#divMensajeErrorNuevoNivel').removeClass('hidden');
        $("#divMensajeErrorNuevoNivel").removeAttr('style');

        $('#MyerrorMensaje').text(data.strMensaje);
    }
}
//#####################################################
function onFailAjaxCrearNivel(data) {
    $("#divMensajeErrorNuevoNivel").removeClass("hidden");
    $("#MyerrorMensaje").text(data.strMensaje);
}

//#####################################################
function onsuccessAjaxEditarNivel(data) {

   
    if (data.ok == 'True') {
        $("#divMensajeErrorEditarNivel").addClass("hidden");
        $("#modalEditarNivel").modal('hide');

        addSuccess({ msg: data.strMensaje });
       
        $("#frmFiltrarNivel")[0].reset();
        $("#frmFiltrarNivel").submit();
    } else {
        $("#divMensajeErrorEditarNivel").removeClass("hidden");
        $("#divMensajeErrorEditarNivel").removeAttr('style');
        $("#MierrorMensaje").text(data.strMensaje);
    }
}
//#####################################################
function onFailAjaxEditarNivel(data) {
    $("#divMensajeErrorEditarNivel").removeClass("hidden");
    $("#MierrorMensaje").text(data.strMensaje);

}
//#####################################################
function onsuccessAjaxEliminarNivel(data) {
    if (data.ok === 'True') {
        $("#modalEliminarNivel").modal('hide');

        addSuccess({ msg: data.strMensaje });

        $("#frmFiltrarNivel")[0].reset();
        $("#frmFiltrarNivel").submit();
       
    } else {
        $("#divMensajeErrorEliminarNivel").removeClass("hidden");
        $("#divMensajeErrorEliminarNivel").removeAttr('style');
        $("#errorMensajeEliminar").text(data.strMensaje);
    }


}

