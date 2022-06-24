$(document).ready(function () {

    function onSuccess(data) {
        onsuccessAjaxCrearFrecuencia(JSON.parse(data));
    }

    function onFailure(error) {
        onFailAjaxCrearFrecuencia(JSON.parse(error));
    }

    function onSuccessEditar(data) {
        onsuccessAjaxEditarFrecuencia(JSON.parse(data));
    }

    function onFailureEditar(error) {
        onFailAjaxEditarFrecuencia(JSON.parse(error));
    }

    function onSuccessEliminar(data) {
        onsuccessAjaxEliminarFrecuencia(JSON.parse(data));
    }

    function onFailureEliminar(error) {
        onsuccessAjaxEliminarNivel(JSON.parse(error));
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

    //#####################################################
    function onsuccessAjaxCrearFrecuencia(data) {
        if (data.ok == 'True') {
            $('#divMensajeErrorNuevaFrecuencia').addClass('hidden');
            $('#itemFrecuencia_NOMBREFRECUENCIA').val('');
            $('#modalCrearFrecuencia').modal('hide');

            addSuccess({ msg: data.strMensaje });

            $('#frmFiltrarFrecuencia')[0].reset();
            $('#frmFiltrarFrecuencia').submit();
           

        } else {
            $('#divMensajeErrorNuevaFrecuencia').removeClass('hidden');
            $("#divMensajeErrorNuevaFrecuencia").removeAttr('style');
            $('#MyerrorMensaje').text(data.strMensaje);
        }
    }

    //#####################################################
    function onFailAjaxCrearFrecuencia(data) {
        $('#divMensajeErrorNuevaFrecuencia').removeClass('hidden');
        $("#divMensajeErrorNuevaFrecuencia").removeAttr('style');
        $('#MyerrorMensaje').text(data.strMensaje);
    }

    //#####################################################
    function onsuccessAjaxEditarFrecuencia(data) {

        if (data.strMensaje == "") {
            data.strMensaje = "Hubo un error al tratar de editar la frecuencia.";
        }

        if (data.ok == 'True') {
            $("#divMensajeErrorEditarFrecuencia").addClass("hidden");
         
            $('#modalEditarFrecuencia').modal('hide');

            addSuccess({ msg: data.strMensaje });

            $('#frmFiltrarFrecuencia')[0].reset();
            $('#frmFiltrarFrecuencia').submit();
            
        } else {
            $("#divMensajeErrorEditarFrecuencia").removeClass("hidden");
            $("#divMensajeErrorEditarFrecuencia").removeAttr('style');
            $("#MierrorMensaje").text(data.strMensaje);
        }

    }

    //#####################################################
    function onFailAjaxEditarFrecuencia(data) {
        $("#divMensajeErrorEditarFrecuencia").removeClass("hidden");
        $("#divMensajeErrorEditarFrecuencia").removeAttr('style');
        $("#MierrorMensaje").text(data.strMensaje);

    }

    //#####################################################
    function onsuccessAjaxEliminarFrecuencia(data) {
        if (data.ok === 'True') {
            $('#modalEliminarFrecuencia').modal('hide');

            addSuccess({ msg: data.strMensaje });

            $('#frmFiltrarFrecuencia')[0].reset();
            $('#frmFiltrarFrecuencia').submit();
           
        }
    }

    //#####################################################
    function onsuccessAjaxEliminarNivel(data) {

    }

    //#####################################################
    //EVENTOS
    //#####################################################

    //#####################################################
    $("input[name='CantidadMeses']").keyup(function (e) {
        
        var cantidadMeses =  $(this).val();        

        if (cantidadMeses <= 0) {
            $(this).val("");
        }
    });

    $("input[name='CantidadMeses']").val("");

    $('#modalCrearFrecuencia').on('shown.bs.modal', function () {
        $("#modalCrearFrecuencia input[name='NombreFrecuencia']").val("");
        $("#modalCrearFrecuencia input[name='CantidadMeses']").val("");
    });

    $('#modalCrearFrecuencia').on('hide.bs.modal', function () {
        $("#modalCrearFrecuencia input[name='NombreFrecuencia']").val("");
        $("#modalCrearFrecuencia input[name='CantidadMeses']").val("");
    });
   
});