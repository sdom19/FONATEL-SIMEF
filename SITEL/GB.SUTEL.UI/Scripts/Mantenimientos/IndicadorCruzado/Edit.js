$(document).ready(function () {    
    //console.log(Indicadores);
    //console.log(IndicadoresInternos);
    //console.log(IndicadoresExternos);
    //console.log(IdDireccion);
    //console.log(IdDireccionInterno);
    //console.log(IdFuenteExterna);    
    if (IdDireccion != 0) {
        $("[name='DIRECCIONINDICADOR']").val(IdDireccion);
        ajaxFilter('_tableIndicadores', IdDireccion);
    }
    var loadIndicadoresInEx = function () {
        if (IdDireccionInterno != 0) {
            $("[name='TipoIndicador']").val(1);
            $(".tipoInterno").show();
            $(".tipoExterno").hide();
            $("[name='DIRECCIONINDICADORINTERNO']").val(IdDireccionInterno);
            ajaxFilter('_tableIndicadoresInternos', IdDireccionInterno);
        } else
            if (IdFuenteExterna != 0) {
                $("[name='TipoIndicador']").val(2);
                $(".tipoInterno").hide();
                $(".tipoExterno").show();
                $("[name='FuenteExterna']").val(IdFuenteExterna);
                ajaxFilter('_tableIndicadoresExternos', IdFuenteExterna);
            }
    }
    loadIndicadoresInEx();
});