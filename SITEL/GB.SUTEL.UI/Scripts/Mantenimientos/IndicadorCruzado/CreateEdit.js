$(document).ready(function () {    
    try{
        Indicadores = Indicadores;
        IndicadoresInternos = IndicadoresInternos;
        IndicadoresExternos = IndicadoresExternos;
    }catch(error){
        Indicadores = [];
        IndicadoresInternos = [];
        IndicadoresExternos = [];
    }
    $("[name='submit']").click(function () {        
        $("#formCreate").trigger("submit");
    });
    $("#formCreate").submit(function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
        if (!$(this).valid()) return false;
        if (validateForm($(this).attr("id")) == false) return false;
        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(500);
        var data = {
            __RequestVerificationToken: $("#formCreate [name='__RequestVerificationToken']:eq(0)").val(),
            IdIndicadorCruzado: $("#formCreate [name='IdIndicadorCruzado']:eq(0)").val(),
            Nombre: $("#formCreate [name='Nombre']:eq(0)").val(),
            Descripcion: $("#formCreate [name='Descripcion']:eq(0)").val(),
            currentId: $("[name='currentId']:eq(0)").val(),
            myIndicadorInternoList: Indicadores,
            IndicadorInternoList: IndicadoresInternos,
            IndicadorExternoList: IndicadoresExternos
        };
        var options = {
            url: $("#formCreate").attr("action"),
            method: $("#formCreate").attr("method"),
            data: data,
            success: function (data) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                onSuccess(data);
            },
            error: function (data) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                onFailure(data, 'formCreate');
            }
        };
        $.ajax(options);
    }); 
    function onSuccess(data) {
        var jdata = JSON.parse(data);
        if (jdata.url != undefined) {
            var url = document.URL.split("/");
            var allurl = url[0] + "//" + url[2] + "/" + jdata.url;
            window.location.href = allurl;
        } else {
            Indicadores = [];
            IndicadoresInternos = [];
            IndicadoresExternos = [];
            $("[name='DIRECCIONINDICADOR']").val("").trigger("change");
            $("[name='TipoIndicador']").val("").trigger("change");
            $("form").trigger("reset");
            $("[name='submit']").val($("#formCreate").attr("data-ajax-message-default")).prop("disabled", false);            
        }

        addSuccess({ msg: jdata.msg });
        window.location = "/IndicadorCruzado/Index";
    }    
    function onFailure(error, formName) {            
        $("[name='error']").show().html(error.statusText);
        $("[name='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
    }
        
    /**/
    $("[name='DIRECCIONINDICADOR']").on('change', function () {
        Indicadores = [];
        if ($(this).val() == "")
            $('#_tableIndicadores form div').hide();
        else 
            ajaxFilter('_tableIndicadores',$(this).val());
    });
    $("[name='DIRECCIONINDICADORINTERNO']").on('change', function () {
        IndicadoresInternos = [];
        if ($(this).val() == "")             
            $('#_tableIndicadoresInternos form div').hide();
        else
            ajaxFilter('_tableIndicadoresInternos', $(this).val());
    });
    $("[name='FuenteExterna']").on('change', function () {
        IndicadoresExternos = [];        
        if ($(this).val() == "") 
            $('#_tableIndicadoresExternos form div').hide();        
        else
            ajaxFilter('_tableIndicadoresExternos', $(this).val());
    });
    $("[name='TipoIndicador']").on('change', function () {
        IndicadoresInternos = [];
        IndicadoresExternos = [];
        if ($(this).val() == "1") {
            $(".tipoInterno").show();
            $(".tipoExterno").hide();
        } else if ($(this).val() == "2") {
            $(".tipoInterno").hide();
            $(".tipoExterno").show();
        } else {
            $(".tipoInterno").hide();
            $(".tipoExterno").hide();
        }
    });
    $('_tableIndicadores form:eq(0), _tableIndicadoresInternos form:eq(0),_tableIndicadoresExternos form:eq(0)').submit(function (evt) {
        evt.preventDefault();
    });
    
    bindCheckBoxes('_tableIndicadores');
    initTable('_tableIndicadores');
    initOnEnterSubmit('_tableIndicadores');

    bindCheckBoxes('_tableIndicadoresInternos');
    initTable('_tableIndicadoresInternos');
    initOnEnterSubmit('_tableIndicadoresInternos');

    bindCheckBoxes('_tableIndicadoresExternos');
    initTable('_tableIndicadoresExternos');
    initOnEnterSubmit('_tableIndicadoresExternos');
});

function validateForm(formName) {
    $("[role='alert']").hide();
    $("[name='submit']:eq(0)").val($("#" + formName).attr("data-ajax-message")).prop("disabled", true);
        
    if (Indicadores.length == 0 && (IndicadoresInternos.length > 0 || IndicadoresExternos.length > 0)) {
        $("[name='warning']").show().html(closureMessage().UnselectedItemsLeft);
        $("[name='submit']:eq(0)").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        return false;
    }
    if (Indicadores.length > 0 && (IndicadoresInternos.length == 0 && IndicadoresExternos.length == 0)) {
        $("[name='warning']").show().html(closureMessage().UnselectedItemsRight);
        $("[name='submit']:eq(0)").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        return false;
    }
    if (Indicadores.length == 0) {
        $("[name='warning']").show().html(closureMessage().UnselectedItems);
        $("[name='submit']:eq(0)").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        return false;
    }
    return true;
}

/*Inicializa filtros y tablas*/
function initTable(DivId) {    
    $('#' + DivId + ' input').unbind("keyup");
    $('#' + DivId + ' form').submit(function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
    });
}

function bindCheckBoxes(DivId) {
    if (DivId != undefined){
        var arrayName = DivId.replace("_table", "");
        $("#" + DivId + " [name='INDICADORES']").unbind('click');
        $("#" + DivId + " [name='INDICADORES']").on('click', function () {
            $("[role='alert']").hide().html("");
            var dataId = $(this).attr("data-id").trim();
            switch (arrayName) {
                case "Indicadores":
                    if ($.inArray(dataId, Indicadores) != -1) {
                        Indicadores = $.grep(Indicadores, function (value) {
                            return value != dataId;
                        });
                    } else
                        Indicadores.push(dataId);
                    break;
                case "IndicadoresInternos":
                    if ($.inArray(dataId, IndicadoresInternos) != -1) {
                        IndicadoresInternos = $.grep(IndicadoresInternos, function (value) {
                            return value != dataId;
                        });
                    } else
                        IndicadoresInternos.push(dataId);
                    break;
                case "IndicadoresExternos":
                    if ($.inArray(dataId, IndicadoresExternos) != -1) {
                        IndicadoresExternos = $.grep(IndicadoresExternos, function (value) {
                            return value != dataId;
                        });
                    } else
                        IndicadoresExternos.push(dataId);
                    break;
                default:
                    break;
            }
            if(validateForm('formCreate')==true)
                $("[name='submit']").val($("#formCreate").attr("data-ajax-message-default")).prop("disabled", false);
                $("[name='submit']").val("Guardar Cambios");
        });
    }
}

function onDraw() {
    bindCheckBoxes($(this).parent().parent().parent().attr("id"));
    var arrayName = String($(this).parent().parent().parent().attr("id")).replace("_table", "");    
    if (arrayName != undefined) {
        $(this).find("input[type='checkbox']").each(function (i, e) {                        
            var active = false;
            switch (arrayName) {
                case "Indicadores":
                    active = $.inArray($(this).attr("data-id"), Indicadores) == -1 ? false : true;
                    break;
                case "IndicadoresInternos":
                    active = $.inArray($(this).attr("data-id"), IndicadoresInternos) == -1 ? false : true;
                    break;
                case "IndicadoresExternos":
                    active = $.inArray($(this).attr("data-id"), IndicadoresExternos) == -1 ? false : true;
                    break;
                default:
                    break;
            }
            if (active) {                
                $(e).prop("checked", active);                
            }
        });
    }
}

function ajaxFilter(DivId, searchid) {
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);
    $("[role='alert']").hide().html("");
    var options = {
        url: $("#" + DivId + " form").attr("action"),
        method: $("#" + DivId + " form").attr("method"),
        data: {
            __RequestVerificationToken: $("#" + DivId + " [name='__RequestVerificationToken']:eq(0)").val(),
            searchid: searchid == undefined ? $("#" + DivId + " [name='searchid']:eq(0)").val() : searchid,
            Nombre: searchid != undefined ? null : $("#" + DivId + " [name='Nombre']:eq(0)").val(),
        },
        success: function (data) {
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });            
            $("#" + DivId).html(data);
            initTable(DivId);
            setTimeout(function () {
                $('#' + DivId + ' table[data-table-grid="true"]').ready(initDataGrid).unbind('draw.dt').on('draw.dt', onDraw).trigger('draw.dt');
                initOnEnterSubmit(DivId);
                $('#' + DivId + ' form, #' + DivId + 'div').show();
            },500);                        
        },
        error: function (data) {
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
            $("[name='error']").show().html(data.statusText);
        }
    };    
    $.ajax(options);
}

function initOnEnterSubmit(DivId) {
    $('#' + DivId + ' input').keyup(function (event) {
        if (event.keyCode === 13)
            ajaxFilter(DivId);
    });
}