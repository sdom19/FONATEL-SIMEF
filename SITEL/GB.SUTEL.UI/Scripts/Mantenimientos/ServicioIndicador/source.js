$(document).ready(function () {
    try {
        $('#_tableIndicadores table[data-table-grid="true"]').on('draw.dt', onDraw);
        Indicadores = Indicadores;
        console.log('yes');
    } catch (error) {
        Indicadores = [];
        console.log('no');
    }
    console.log(Indicadores);
    $("#formCreate").submit(function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
        if (!validateFunction("formCreate")) return false;
        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(500);
        var data = {
            __RequestVerificationToken: $("#formCreate [name='__RequestVerificationToken']:eq(0)").val(),
            Servicio: $("[name='Servicio']:eq(0)").val(),
            INDICADORES: Indicadores
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
            error: function (error) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                $("[name='error']").show().html(error.statusText);
                $("[name='submit']").val($("#formCreate").attr("data-ajax-message-default")).prop("disabled", false);
            }
        };
        $("[name='submit']").val($("#formCreate").attr("data-ajax-message")).prop("disabled", true);
        $.ajax(options);
    });
    function onSuccess(data) {
        //debugger;
        var jdata = JSON.parse(data);
        if (jdata.url != undefined) {
            var url = document.URL.split("/");
            var allurl = url[0] + "//" + url[2] + "/" + jdata.url;
            
            //window.location.href = allurl;
        } else {
            Indicadores = [];
            $("select").val("");
            $("[type='checkbox']").prop("checked",true);
            $("[name='submit']").val($("#formCreate").attr("data-ajax-message-default")).prop("disabled", false);
        }
       
        addSuccess({ msg: jdata.strMensaje });
        window.location.href = '/ServicioIndicador/Index';
    }
    $("[name='submit']").click(function () {
        $("#formCreate").trigger("submit");
    });
    /**/
    $("[name='Servicio']").on('change', function () {
        $("[data-valmsg-for='Servicio']").html("");
        if ($(this).val()!="")
            ajaxFilter('_tableIndicadores', $(this).val());
    });
    $('_tableIndicadores form:eq(0)').submit(function (evt) {
        evt.preventDefault();
    });
    bindCheckBoxes('_tableIndicadores');
    initTable('_tableIndicadores');
    initOnEnterSubmit('_tableIndicadores');
    $('#_tableIndicadores table[data-table-grid="true"]').on('draw.dt', onDraw);
});

/*Inicializa filtros y tablas*/
function initTable(DivId) {
    $('#' + DivId + ' input').unbind("keyup");
    $('#' + DivId + ' form').submit(function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
    });
}

function bindCheckBoxes(DivId) {
    if (DivId != undefined) {
        var arrayName = DivId.replace("_table", "");
        $("#" + DivId + " [name='INDICADORES']").unbind('click');
        $("#" + DivId + " [name='INDICADORES']").on('click', function () {            
            $("[role='alert']").hide().html("");
            var dataId = $(this).attr("data-id").trim();
            if ($.inArray(dataId, Indicadores) != -1) {
                Indicadores = $.grep(Indicadores, function (value) {
                    return value != dataId;
                });
            } else
                Indicadores.push(dataId);            
        });
    }
}

function onDraw() {
    bindCheckBoxes($(this).parent().parent().parent().attr("id"));
    var arrayName = String($(this).parent().parent().parent().attr("id")).replace("_table", "");
    if (arrayName != undefined) {
        $(this).find("input[type='checkbox']").each(function (i, e) {
            active = $.inArray($(this).attr("data-id"), Indicadores) == -1 ? false : true;            
            if (active) {
                $(e).prop("checked", active);
            }
        });
    }
}

function ajaxFilter(DivId,searchid) {
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);
    $("[role='alert']").hide().html("");
    var options = {
        url: $("#" + DivId + " form").attr("action"),
        method: $("#" + DivId + " form").attr("method"),
        data: {
            __RequestVerificationToken: $("#" + DivId + " [name='__RequestVerificationToken']:eq(0)").val(),
            searchid: searchid == undefined ? null : searchid,
            Nombre: $("#" + DivId + " [name='Nombre']:eq(0)").val(),
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
                $('#' + DivId + ' form').fadeIn();
            }, 500);
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

function CargarIndicadores() {
    $("#_tableIndicadores").find("input[type='checkbox']").each(function (i, e) {
        active = $.inArray($(this).attr("data-id"), Indicadores) == -1 ? false : true;
        if (active) {
            $(e).prop("checked", active);
        }
    });
}