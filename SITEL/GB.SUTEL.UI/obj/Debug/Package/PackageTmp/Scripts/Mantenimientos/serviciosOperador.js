$(document).ready(function () {
    $('#modalContinuar').on('hidden.bs.modal', function (e) {
        ActivaCargaServicios();
    })
    $('#modalSeleccionarOperador').hide();

    function onSuccess(data) {
        onsuccessAjaxConsultarServicios(data);
    }

    function onFailure(error) {

    }

    function onSuccessGuardarServicios(data) {        
        onsuccessAjaxGuardarServiciosOperador(JSON.parse(data));
    }

    function onFailureGuardarServicios(data) {        
        onFailAjaxGuardarServiciosOperador(JSON.parse(data));
    }

    function onSuccessBusquedaOperador(data) {
        alert('exito busqueda');
    }

    function onFailureBusquedaOperador(data) {
        alert('fallo busqueda');
    }

    AjaxifyMyForm("ConsultarServicios",
        onSuccess,
        onFailure
        );
   
    AjaxifyMyForm("formGuardar",
        onSuccessGuardarServicios,
        onFailureGuardarServicios
        );

    AjaxifyMyForm("frmFilterOperador",
        onSuccessBusquedaOperador,
        onFailureBusquedaOperador
        );

    $('[type=checkbox]').click(function () {
        return;
        hayCambio = true;

        var checkbox = this;
        var $hidden = $('#ServiciosEliminar');
        //var $hiddenAdd = $('#ServiciosAgregar');


        var ValoresEliminar = $hidden.val();
        var ArregloValores = ValoresEliminar.split(',');

        //var ValoresAgregar = $hiddenAdd.val();
        //var ArregloValoresAgregar = ValoresEliminarAgregar.split(',');

        if ($(checkbox).prop('checked') == false) {
            if ($.inArray($(checkbox).attr('value'), ArregloValores) == -1) {
                if (ValoresEliminar == '') {
                    ValoresEliminar = $(checkbox).attr('value');
                } else {
                    ValoresEliminar = ValoresEliminar + "," + $(checkbox).attr('value');
                }

                //quitar de los cargados
                var inList = false;
                var indexArray = 0;
                for (var i = 0; i < ValoresServicios.length; i++) {
                    if (ValoresServicios[i] == ($(checkbox).attr('value'))) {
                        inList = true;
                        indexArray = i;
                        break;
                    }
                }
                if (inList) {
                    ValoresServicios.splice(indexArray, 1);
                }
                //quitar de los cargados
            }
        }
        //} else {
        //    if ($.inArray($(checkbox).attr('value'), ArregloValoresAgregar) == -1) {
        //        if (ValoresAgregar == '') {
        //            ValoresAgregar = $(checkbox).attr('value');
        //        } else {
        //            ValoresAgregar = ValoresAgregar + "," + $(checkbox).attr('value');
        //        }
        //    }
        //}

        //se cargan los nuevos valores
        $hidden.val(ValoresEliminar);
        //$hiddenAdd.val(ValoresAgregar);
    });
});


function onsuccessAjaxConsultarServicios(data) {
    data = JSON.parse(data);
   
    if (data.ok == 'True') {
        ValoresServicios = data.data;
        //var vOpe = data.data.split('|');
        //se quitan todos los check
        $('[type=checkbox]').each(function (index) {
            var checkbox = this;
            $(checkbox).prop('checked', false);
        });
        //for (x in vOpe) {


        //    //se borran primeramente
        //    var html = "<div id='CheckBoxHidden' style='display:none'>" + data + "</div>";
        //    var $form = $(html);
        //    $("#CheckBoxHidden").replaceWith($form);

        //    for (var i = 0; i < ValoresServicios.length; i++) {
        //        $("#" + ValoresServicios[i]).prop('checked', true);
        //    }
        //}
        for (var i = 0; i < data.data.length; i++) {

            //se borran primeramente
            var html = "<div id='CheckBoxHidden' style='display:none'>" + data + "</div>";
            var $form = $(html);
            $("#CheckBoxHidden").replaceWith($form);

            $("#" + data.data[i].IdServicio).prop('checked', true);
            if (data.data[i].Borrado == 1) {
                $("#Verificado_"+ data.data[i].IdServicio).addClass("glyphicon glyphicon-ok")

            }
        }


    }

}

function onsuccessAjaxGuardarServiciosOperador(data) {

    if (data.ok == 'True') {
        var $hidden = $('#ServiciosEliminar');

        $hidden.val('');
        hayCambio = false;
        addSuccess({ msg: "Asignación de Servicios realizada..." });

    }
    else if(data.ok == 'False') {
        var $hidden = $('#ServiciosEliminar');        
        $hidden.val('');
        hayCambio = false;
        //addSuccess({ msg: data.strMensaje });
        $('#modalSeleccionarOperador').modal();
        $('#modalError').html(data.strMensaje);

    }

}

function onFailAjaxGuardarServiciosOperador(data) {

    if (data.ok == 'False') {
        var $hidden = $('#ServiciosEliminar');

        $hidden.val('');
        hayCambio = false;
        addSuccess({ msg: data.strMensaje });

    }

}

function ActivaCargaServicios() {

    if ($('#hiddContinuar').val() == '1') {
        setTimeout(function () {
            hayCambio = false;
            $('#butCargaServicios').submit();
        }, 100);
    }
}

function Aviso() {

    $("#dataTable_1 td span").removeClass("glyphicon glyphicon-ok");

    varIdOperadorGuardar = $('#IdOperadorGuardar').val();
    if (ValoresServicios.length > 0 && hayCambio) {    
        $('#modalContinuar').modal();
    } else {
        setTimeout(function () {
            hayCambio = false;
            $('#butCargaServicios').submit();
        }, 100);
    }
}

$('#modalSi').click(function () {
    $('#hiddContinuar').val('1');
    hayCambio = false;
});

$('#modalNo').click(function () {
    $('#IdOperadorGuardar').val(varIdOperadorGuardar);
    $('#hiddContinuar').val('0');

});
$('#modalAceptar').click(function () {
    $('#hiddContinuar').val('1');
    hayCambio = false;
});
