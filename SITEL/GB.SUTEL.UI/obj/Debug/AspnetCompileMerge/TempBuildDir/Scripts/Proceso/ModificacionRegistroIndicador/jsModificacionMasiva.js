$(document).ready(function () {
    var valorSeleccionado = 0;
    cargaOperadoresMasivo(valorSeleccionado);
    cargaIndicadorMasivo(valorSeleccionado)
   
});

function cargaOperadoresMasivo(valorSeleccionado) {
    $.ajax({
        type: "GET",
        url: 'ModificionRegistroIndicadorInterno/listarOperadoresServicio',
        data: { IdServicio: valorSeleccionado },
        contentType: "application/json",
        success: function (data) {
            $('#ddlOperadorDescarga').empty();
            for (var i = 0; i < data.length; i++) {
                $('#ddlOperadorDescarga').append('<option value="' + data[i].id + '">'+ data[i].valor + '</option>');
            }
            $('#ddlOperadorDescarga').select2();
        },
        error: function () {

            $('#ddlOperadorDescarga').eq(0).html();
        }
    });
}



function cargaIndicadorMasivo(valorSeleccionado) {
    $.ajax({
        type: "GET",
        url: 'ModificionRegistroIndicadorInterno/listarIndicadores',
        data: { IdServicio: valorSeleccionado },
        contentType: "application/json",
        success: function (data) {
            $('#ddlIndicadorDescarga').empty();
            for (var i = 0; i < data.length; i++) {
                $('#ddlIndicadorDescarga').append('<option value="' + data[i].id + '">' + data[i].valor + '</option>');
            }
            $('#ddlIndicadorDescarga').select2();
        },
        error: function () {

            $('#ddlIndicadorDescarga').eq(0).html();
        }
    });
}



$("#ddlServicioMasivo").change(function () {
    var valorSeleccionado = $("#ddlServicioMasivo").val();
    cargaOperadoresMasivo(valorSeleccionado);
    cargaIndicadorMasivo(valorSeleccionado);
});



function CargarDatos() {
    $("#txtOperadorNombreDescarga").val($("#nombreOperador").val());
    var servicioSeleccionado = $("#ddlServicio").val();
    $("#txtDescripcionServicio").val(servicioSeleccionado == 0 ? "" : $("#ddlServicio option:selected").text());
    $("#txtano").val("");
    $("#ddlEstado").val("0");


};

function descargarExcelRegistroIndicadores() {

    var registroIndicadoresNombreExcel = $("#txtNombreFile").val();
    var Operador = $("#ddlOperadorDescarga").val() == null ? "": $("#ddlOperadorDescarga").val() ;
    var annos = $("#txtano").val();
    var servicio = $("#ddlServicioMasivo").val();
    var indicador = $("#ddlIndicadorDescarga").val() == null ? "":$("#ddlIndicadorDescarga").val();
    window.open("/ModificionRegistroIndicadorInterno/DescargarExcel?plantilla=" + registroIndicadoresNombreExcel + "&anno=" + annos + "&Operador=" + Operador + "&Servicio=" + servicio +  "&indicador=" + indicador);
};

$(document).on("submit", "#formularioCarga", function (e) {
    e.preventDefault();
    $("#mensaje").empty();
    var files = $('#fileImportar')[0].files[0];

    if (files == undefined) {
        $("#mensaje").append("<div class='alert alert-danger' role='alert'>No hay archivo a importar</div>")
        }
    else {
        var parametros = new FormData($(this)[0]);
        //parametros.append("Plantilla", files);

        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(200);
        //realizamos la petición ajax con la función de jquery
        $.ajax({
            type: "POST",
            url: '/ModificionRegistroIndicadorInterno/ImportarPlantillaExcel',
            data: parametros,
            contentType: false,
            processData: false,
            cache: false,
            success: function (response) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                           
                $('#fileImportar').val("");
                setTimeout(function () { $("#mensaje").append(response); }, 3000);
            },
            error: function (error) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                setTimeout(function () { $("#mensaje").append(error); }, 3000);
            },
        });
    }
});


$("#btnImportarPlantilla").click(function () {
  
    $("#modalRegistroIndicadoresImportar").modal({
        backdrop: 'static',
        keyboard: false,

        
    });
});

