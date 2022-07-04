$(document).ready(function () {

    function onSuccess(data) {
        onsuccessAjaxCrearCriterio(JSON.parse(data));
    }

    function onFailure(error) {
        onFailAjaxCrearCriterio(JSON.parse(data));
    }

    function onSuccessEditar(data) {
        onsuccessAjaxEditarCriterio(JSON.parse(data));
    }

    function onFailureEditar(error) {
        onFailAjaxEditarCriterio(JSON.parse(data));
    }

    function onSuccessEliminar(data) {
        onsuccessAjaxEliminarCriterio(JSON.parse(data));
    }

    function onFailureEliminar(error) {

    }

    $('#IdDireccion').change(function () {
        cargarIndicador(this.value);
    });

    $('#IdDireccionEditar').change(function () {
        cargarIndicadorEditar(this.value);
    });


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
function cargarIndicador(idDireccion) {

    $.ajax({
        type: "POST",
        url: 'Criterio/_listarIndicadores',
        data: JSON.stringify({ IdDireccion: idDireccion, Nombre: '' }),
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            var dataCriterios = jsonData.data;

            $("#IdIndicador").html("");
            $("#IdIndicador").append("<option value=''>Seleccione</option>");
            $.each(dataCriterios, function (k, v) {
                $("#IdIndicador").append("<option value=\"" + v.IdIndicador + "\">" + v.NombreIndicador + "</option>");
            });
        },
        error: function () {
            
            $("[name='IdIndicador']").eq(0).html();
        }
    });
}

//#####################################################
function cargarEditar(idDireccion, idIndicador) {
    $.ajax({
        type: "POST",
        url: 'Criterio/_listarIndicadores',
        data: JSON.stringify({ IdDireccion: idDireccion, Nombre: '' }),
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (data) {            
            var jsonData = JSON.parse(data);
            var dataCriterios = jsonData.data;            


            $("#IdIndicadorEditar").html("");
            $("#IdIndicadorEditar").append("<option value=''>Seleccione</option>");
            $.each(dataCriterios, function (k, v) {                
                $("#IdIndicadorEditar").append("<option value=\"" + v.IdIndicador + "\">" + v.NombreIndicador + "</option>");
            });
            $('#IdIndicadorEditar').val(idIndicador);
        },
        error: function () {

            $("[name='IdIndicadorEditar']").eq(0).html();
        }
    });

   
}

//#####################################################
function cargarIndicadorEditar(idDireccion) {

    $.ajax({
        type: "POST",
        url: 'Criterio/_listarIndicadores',
        data: JSON.stringify({ IdDireccion: idDireccion, Nombre: '' }),
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            var dataCriterios = jsonData.data;
           

            $("#IdIndicadorEditar").html("");
            $("#IdIndicadorEditar").append("<option value=''>Seleccione</option>");
            $.each(dataCriterios, function (k, v) {
                $("#IdIndicadorEditar").append("<option value=\"" + v.IdIndicador + "\">" + v.NombreIndicador + "</option>");
            });                      
        },
        error: function () {

            $("[name='IdIndicadorEditar']").eq(0).html();
        }
    });
}
//#####################################################
$('#modalCrearCriterio').on('hidden.bs.modal', function (e) {
    $("#IdDireccion option:eq(0)").attr("selected", "selected");
    $("#IdIndicador").empty();
    //$("[name='IdIndicador']").eq(0).html();
    $(this)
        .find("span")
        .val('');
});

//#####################################################
$('#modalEditarCriterio').on('hidden.bs.modal', function (e) {
    $("#IdDireccionEditar option:eq(0)").attr("selected", "selected");
    $("#IdIndicadorEditar").empty();
    //$("[name='IdIndicador']").eq(0).html();
    $(this)
        .find("span")
        .val('');
});

//#####################################################
$('#modalCrearCriterio').on('show.bs.modal', function (e) {
    $("#IdCriterio").val("");
    $("#DescCriterio").val("");
    $(".text-danger field-validation-error").hide();
});

//#####################################################
function onsuccessAjaxCrearCriterio(data) {
    if (data.ok == 'True') {
        $('#divMensajeErrorNuevoNivel').addClass('hidden');
        $('#IdCriterio').val('');
        $('#DescCriterio').val('');
        $('#IdDireccion').prop('selectedIndex',0);
        $('#modalCrearCriterio').modal('hide');

        addSuccess({ msg: data.strMensaje });

        $('#frmFiltrarCriterio')[0].reset();
        $('#frmFiltrarCriterio').submit();
    } else {
        $("#divMensajeErrorNuevoCriterio").removeClass("hidden");
        $("#divMensajeErrorNuevoCriterio").removeAttr('style');
        $("#MyerrorMensaje").text(data.strMensaje);
    }
}
//#####################################################
function onFailAjaxCrearCriterio(data) {
    $("#divMensajeErrorNuevoCriterio").removeClass("hidden");
    $("#MyerrorMensaje").text(data.strMensaje);
}

//#####################################################
function onsuccessAjaxEditarCriterio(data) {
    if (data.ok == 'True') {
        $("#divMensajeErrorEditarCriterio").addClass("hidden");
        $('#IdCriterio').val('');
        $('#DescCriterio').val('');
        $('#IdDireccion').prop('selectedIndex', 0);
        $('#modalEditarCriterio').modal('hide');

        addSuccess({ msg: data.strMensaje });

        $('#frmFiltrarCriterio')[0].reset();
        $('#frmFiltrarCriterio').submit();
    } else {
        $("#divMensajeErroEditarCriterio").removeClass("hidden");
        $("#divMensajeErroEditarCriterio").removeAttr('style');
        $("#errorMensajeEditar").text(data.strMensaje);
    }


}
//#####################################################
function onFailAjaxEditarCriterio(data) {
    $("#divMensajeErroEditarCriterio").removeClass("hidden");
    $("#divMensajeErroEditarCriterio").removeAttr('style');
    $("#errorMensajeEditar").text(data.strMensaje);
}
//#####################################################
function onsuccessAjaxEliminarCriterio(data) {

    if (data.ok == 'True') {
        $("#modalEliminarCriterio").modal('hide');

        addSuccess({ msg: data.strMensaje });

        $('#frmFiltrarCriterio')[0].reset();
        $('#frmFiltrarCriterio').submit();
    } else {
        $("#divMensajeErrorEliminarCriterio").removeClass("hidden");
        $("#divMensajeErrorEliminarCriterio").removeAttr('style');
        $("#errorMensajeEliminar").text(data.strMensaje);
    }

}