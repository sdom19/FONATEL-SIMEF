
function cargaOperadoresxSolicitud(valorSeleccionado) {
    $.ajax({
        type: "GET",
        url: 'Solicitud/OperadoresXSolicitud',
        data: { indicador: valorSeleccionado },
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
