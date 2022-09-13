$(document).ready(function () {

    function onSuccess(data) {
        onsuccessAjaxCrearDetalle(JSON.parse(data));
    }

    function onFailure(data) {
        onFailAjaxCrearDetalle(JSON.parse(data));
    }

    function onSuccessEditar(data) {
        onsuccessAjaxEditarDetalle(JSON.parse(data));
    }

    function onFailureEditar(error) {
        onFailAjaxEditarDetalle(JSON.parse(data));
    }

    function onSuccessEliminar(data) {
        onsuccessAjaxEliminarDetalle(JSON.parse(data));
    }

    function onFailureEliminar(error) {

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

    //Everytime we press delete in the table row
    ActivarEliminar();

    CargarPlantilla();

});

//#####################################################
function onsuccessAjaxCrearDetalle(data) {
    if (data.ok == 'True') {
        $("#divMensajeErrorNuevoDetalle").addClass("hidden");
        $("#itemDetalleAgrupacion_DESCDETALLEAGRUPACION").val("");
        $('#itemDetalleAgrupacion_IDAGRUPACION').prop('selectedIndex', 0);
        $('#itemDetalleAgrupacion_IDOPERADOR').prop('selectedIndex', 0);
        $("#modalCrearDetalle").modal('hide');

        addSuccess({ msg: data.strMensaje });

        $("#frmFiltrarDetalleAgrupacion")[0].reset();
        $("#frmFiltrarDetalleAgrupacion").submit();
    } else {
        $("#divMensajeErrorNuevoDetalle").removeClass("hidden");
        $("#divMensajeErrorNuevoDetalle").removeAttr('style');
        $("#MyerrorMensaje").text(data.strMensaje);
    }
}
//#####################################################
function onFailAjaxCrearDetalle(data) {
    $("#divMensajeErrorNuevoDetalle").removeClass("hidden");
    $("#divMensajeErrorNuevoDetalle").removeAttr('style');
    $("#MyerrorMensaje").text(data.strMensaje);
}

//#####################################################
function onsuccessAjaxEditarDetalle(data) {

    if (data.ok === 'True') {
        $("#divMensajeErrorEditarDetalle").addClass("hidden");
        $("#modalEditarDetalle").modal('hide');

        addSuccess({ msg: data.strMensaje });

        $("#frmFiltrarDetalleAgrupacion")[0].reset();
        $("#frmFiltrarDetalleAgrupacion").submit();
    } else {
        $("#divMensajeErrorEditarDetalle").removeClass("hidden");
        $("#divMensajeErrorEditarDetalle").removeAttr('style');
        $("#MierrorMensaje").text(data.strMensaje);
    }

}
//#####################################################
function onFailAjaxEditarDetalle(data) {
    $("#divMensajeErrorEditarDetalle").removeClass("hidden");
    $("#divMensajeErrorEditarDetalle").removeAttr('style');
    $("#MierrorMensaje").text(data.strMensaje);

}
//#####################################################
function onsuccessAjaxEliminarDetalle(data) {
    if (data.ok === 'True') {
        $("#modalEliminarDetalle").modal('hide');

        addSuccess({ msg: data.strMensaje });

        $("#frmFiltrarDetalleAgrupacion")[0].reset();
        $("#frmFiltrarDetalleAgrupacion").submit();

    } else {
        $("#divMensajeErrorEliminarDetalle").removeClass("hidden");
        $("#divMensajeErrorEliminarDetalle").removeAttr('style');
        $("#errorMensajeEliminar").text(data.strMensaje);
    }


}

//#####################################################
function ActivarEliminar() {

    //Everytime we press delete in the table row
    $('.myDelete').click(function (e) {


        //Update the item to delete id so our model knows which one to delete
        var id1 = $(this).data('id1');
        $('#ItemEliminarOperador').val(id1);
        var id2 = $(this).data('id2');
        $('#ItemEliminarAgrupacion').val(id2);
        var id0 = $(this).data('id0');
        $('#ItemEliminarIdDetalleAgrupacion').val(id0);
        e.preventDefault();

    });
}

//###################################################

$('#modalCargarExcel').on('show.bs.modal', function (event) {
    $("#modalCargarExcel input[type='text']").val("");
    $("#buttonSubir").prop('disabled', true);
    $("#divMensajePlantilla").addClass("hidden");
});

$(document).on('click', '#btnBuscarPlantilla', function (e) {
    $("#modalCargarExcel input[type='text']").val("");
    $("#buttonSubir").prop('disabled', true);
    $("#divMensajeErrorPlantilla").addClass("hidden");
    $("#divMensajePlantilla").addClass("hidden");
    $("#selectedFile").click();
    e.preventDefault();
    });

$(document).on('change', '#selectedFile', function (e) {
    var fullPath = $('#selectedFile').val();
    if (fullPath) {
        var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
        var filename = fullPath.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
        $("#txtPlantilla").val(filename);
        $("#buttonSubir").removeAttr("disabled");
    }
});

function CargarPlantilla() {

    //sobreescribimos el metodo submit para que envie la solicitud por ajax
    $("#frmFormulario").submit(function (e) {
        //esto evita que se haga la petición común, es decir evita que se refresque la pagina
        e.preventDefault();

        //FormData es necesario para el envio de archivo,
        //y de la siguiente manera capturamos todos los elementos del formulario
        var parametros = new FormData($(this)[0]);
        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(200);
        //realizamos la petición ajax con la función de jquery
        $.ajax({
            type: "POST",
            url: "/DetalleAgrupacion/CargarExcel",
            data: parametros,
            contentType: false,
            processData: false,
            success: function (data) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                $("#modalCargarExcel input[type='text']").val("");
                $("#buttonSubir").prop('disabled', true);
                
                if (JSON.parse(data).strMensaje != "") {
                    $("#divMensajeErrorPlantilla").removeClass("hidden");
                    $("#divMensajeErrorPlantilla").removeAttr('style');
                    $("#divMensajeErrorPlantilla").html(JSON.parse(data).strMensaje);
                }

                if (JSON.parse(data).data != null && JSON.parse(data).strMensaje == "") {
                    $("#modalCargarExcel").modal('hide');

                    addSuccess({ msg: "Se insertó " + JSON.parse(data).data + " registro(s) correctamente" });

                    $("#frmFiltrarDetalleAgrupacion")[0].reset();
                    $("#frmFiltrarDetalleAgrupacion").submit();
                } else if (JSON.parse(data).data != null)
                {
                    $("#divMensajePlantilla").removeClass("hidden");
                    $("#divMensajePlantilla").removeAttr('style');
                    $("#MensajeExitoPlantilla").text("Se insertó " + JSON.parse(data).data + " registro(s) correctamente");

                    $("#frmFiltrarDetalleAgrupacion")[0].reset();
                    $("#frmFiltrarDetalleAgrupacion").submit();
                }
            },
            error: function (r) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
            }
        });
    })
}

