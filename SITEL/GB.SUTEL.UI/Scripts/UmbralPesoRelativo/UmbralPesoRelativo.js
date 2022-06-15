var editor; // use a global for the submit and return data rendering in the examples

$(function () {

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");

});



function spiner() {
    console.log("spiner");
    $(".darkScreen").fadeIn(100);
}

$(document).ready(function () {

    //$(".form_datetime").datetimepicker({ format: 'yyyy-mm-dd', autoclose: true, });

    //$('#fechaInic').datepicker({
    //    format: 'yyyy-mm-dd'
    //});

    //$('#fechaFin').datepicker({
    //    format: 'yyyy-mm-dd'
    //});

    $("#TxtUmbral").numeric(".");
    $("#TxtPeso").numeric(".");
    $("#Confirmacion").hide();
    $("#MensajeError").hide();
    $("#CombServicios").attr("disabled", true);
    $("#InputUsuarios").attr("readonly", true)


    //   $('#TxtUmbral').keyup(function () {
    //      this.value = (this.value + '').replace(/[^0-9]/g, '');
    //});

    $(".darkScreen").fadeOut(100, function () {
        $(this).remove();
    });

    /// $('#TxtPeso').keyup(function () {
    /// this.value = (this.value + '').replace(/[^0-9]/g, '');
    /// });


    function LoadTabla(data) {
        document.getElementById("ResultLisInidicarServicio").innerHTML = "";
        var tabla = '<table class="table table-striped table-bordered table-hover dataTables" id="dexample">';
        tabla += '<thead>';
        tabla += '<tr>';
        tabla += '<th align="right">IdIndicador</th>';
        tabla += '<th align="right">Indicador</th>';
        tabla += '<th align="right">Umbral</th>';
        tabla += '<th align="right">Peso Relativo</th>';
        tabla += '<th align="right">Usuario Mod</th>';
        tabla += '<th align="right">Fecha Mod</th>';
        tabla += '<th align="right"></th>';
        tabla += '</tr>';
        tabla += '</thead>';
        tabla += '<tbody>';
        $.each(data, function (i, item) {
            tabla += '<tr class="" align="left">';
            tabla += '<td>' + item.IdIndicador + '</td>';
            tabla += '<td>' + item.NombreIndicador + '</td>';
            tabla += '<td><input type="text" value=' + item.Umbral + ' readonly></td>';
            tabla += '<td><input type="text" value=' + item.Peso + ' readonly></td>';
            if (item.Usuario == "") {
                item.FechaUltimaModificacion = "";
            }
            //else
            //{
            //    var splitfecha = item.FechaUltimaModificacion.split("-");
            //    item.FechaUltimaModificacion = splitfecha[1] + "-" + splitfecha[0] + "-" + splitfecha[2];
            //}
            console.log(item.FechaUltimaModificacion)
            tabla += '<td>' + item.Usuario + ' </td>';
            tabla += '<td>' + item.FechaUltimaModificacion + '</td>';
            tabla += '<td ><button type="button" class="BtnEditar btn btn-default btn-xs" id=' + item.IdIndicador + "-" + item.Umbral + "-" + item.Peso + '><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button></td>';
            tabla += ' </tr>';
        })
        tabla += '</tbody>';
        tabla += '</table>';
        $("#ResultLisInidicarServicio").append(tabla);
        $('#dexample').dataTable
            ({
                "processing": true,
            });

        $("#fechaInic").attr("readonly", false);
        $("#fechaFin").attr("readonly", false);
    }

    var IdIndicador;
    var Umbral;
    var Peso;
    $(document).on('click', '.BtnEditar', function () {
        $("#exampleModalCenter").modal();
        var data = $(this).attr("id");
        var splitData = data.split("-");
        IdIndicador = splitData[0];
        $("#TxtUmbral").val(splitData[1]);
        $("#TxtPeso").val(splitData[2]);

    });


    $(document).on('click', '#BtnAplicar', function () {
        if ($("#fechaInic").val() == "" || $("#fechaFin").val() == "") {
            var dInicN = new Date();
            var dFinN = new Date();
            AjaxIdicadoSericFecha($("#CombServicios").val(), dInicN, dFinN, $("#CobmDireccion").val())
        }
        else {

            AjaxIdicadoSericFecha($("#CombServicios").val(), $("#fechaInic").val(), $("#fechaFin").val(), $("#CobmDireccion").val());
        }

        //var fechaInic = new Date(cInic);
        //var fechaFin = new Date(cFin);
    });

    $(document).on('click', '#BtnGuardar', function () {
        var data =
        {
            "IdIndicador": IdIndicador,
            "Umbral": $("#TxtUmbral").val(),
            "Peso": $("#TxtPeso").val()
        };

        if ($("#TxtUmbral").val() == "" || $("#TxtPeso").val() == "") {
            document.getElementById("MensajeError").innerHTML = "";
            $("#MensajeError").show("slow");
            $('#MensajeError').fadeIn();
            setTimeout(function () { $("#MensajeError").fadeOut() }, 4000);
            $("#MensajeError").append("<strong >Mensaje de Norificación! </strong>valida que los campos del formulario no se encuentren vacios");
        } else {
            EnviarDatos(data);
        }

    });


    $("#CobmDireccion").on("change", function () {

        if ($("#CombServicios").val() != 0) {
            var IdServicios = $("#CombServicios").val();
            var IdDireccion = $("#CobmDireccion").val();
            AjaxIdicadoSeric(IdServicios, IdDireccion);
        }

        $("#CombServicios").attr("disabled", false)

    });

    $("#CombServicios").on("change", function () {
        var IdServicios = $("#CombServicios").val();
        var IdDireccion = $("#CobmDireccion").val();
        AjaxIdicadoSeric(IdServicios, IdDireccion);
        $("#InputUsuarios").attr("readonly", false)

    });


    $(document).on("click", "#BtnAplicarUsuario", function () {
        var IdServicios = $("#CombServicios").val();
        var Usuario = $("#InputUsuarios").val();
        var IdDireccion = $("#CobmDireccion").val();
        AjaxIdicadoSericUsuarios(IdServicios, Usuario, IdDireccion);

    });
    //setInterval(function () { $("#Confirmacion").show(); }, 3000);



    function AjaxIdicadoSericFecha(IdServicios, FechaInic, FechaFin, IdDireccion) {
        spiner();
        $.ajax({
            url: '/IndicadorUmbral/GetDatosIndicadorXServicioFechas',
            type: 'post',
            contentType: 'application/json;charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({ IdServicios: IdServicios, FechaInic: FechaInic, FechaFin: FechaFin, IdDireccion: IdDireccion }),
            success: function (entidad) {

                LoadTabla(entidad);
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            },
            async: false
        });

    }


    function AjaxIdicadoSeric(IdServicios, IdDireccion) {
        spiner();
        $.ajax({
            url: '/IndicadorUmbral/GetDatosIndicadorXServicio',
            type: 'post',
            contentType: 'application/json;charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({ IdServicios: IdServicios, IdDireccion: IdDireccion }),
            success: function (entidad) {
                ///var pr = jQuery.parseJSON(entidad
                //console.log(JSON.stringify(entidad));
                $(".darkScreen").fadeOut(100);
                LoadTabla(entidad);
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            },
            async: false
        });

    }


    function AjaxIdicadoSericUsuarios(IdServicios, Usuario, IdDireccion) {
        spiner();
        $.ajax({
            url: '/IndicadorUmbral/GetDatosIndicadorXServicioUsuarios',
            type: 'post',
            contentType: 'application/json;charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({ IdServicios: IdServicios, Usuario: Usuario, IdDireccion: IdDireccion }),
            success: function (entidad) {
                ///var pr = jQuery.parseJSON(entidad
                //console.log(JSON.stringify(entidad));
                LoadTabla(entidad);
            },
            error: function (errormessage) {
                //alert(errormessage.responseText);
            },
            async: false
        });

    }

    var modelPrueba;
    function EnviarDatos(Mimodel) {
        Mimodel.Peso = Mimodel.Peso.replace(".", ",");
        Mimodel.Umbral = Mimodel.Umbral.replace(".", ",");
        console.log("Mi model");
        console.log(Mimodel);
        modelPrueba = Mimodel;
        var mensaje;
        spiner();
        document.getElementById("Confirmacion").innerHTML = "";
        $.ajax({
            url: '/IndicadorUmbral/CrearImbralesIndicadores',
            type: 'post',
            contentType: 'application/json;charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({ Mimodel: Mimodel }),
            success: function (entidad) {
                ///var pr = jQuery.parseJSON(entidad)
                //$("#Confirmacion").show();
                $(".darkScreen").fadeOut(100);
                $("#exampleModalCenter").modal("hide");

                AjaxIdicadoSeric($("#CombServicios").val(), $("#CobmDireccion").val());
                $("#Confirmacion").show("slow");
                $('#Confirmacion').fadeIn();
                setTimeout(function () { $("#Confirmacion").fadeOut() }, 5000);
                if (entidad[1] == "1") {
                    mensaje = "<strong >Información </strong>" + entidad[0] + "";
                    $("#Confirmacion").removeClass().addClass("alert alert-info alert-dismissible fade in");

                }
                else if (entidad[1] == "2") {
                    mensaje = "<strong >Mensaje de Confirmación! </strong>" + entidad[0] + "";
                    $("#Confirmacion").removeClass().addClass("alert alert-success alert-dismissible fade in");
                }
                else if (entidad[1] == "3") {
                    mensaje = "<strong >Mensaje de Confirmación! </strong>" + entidad[0] + "";
                    $("#Confirmacion").removeClass().addClass("alert alert-danger alert-dismissible fade in");
                }
                $("#Confirmacion").append(mensaje);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            },
            async: false
        });
    }
});