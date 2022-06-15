IndicadoresExternos = [];
$(document).ready(function () {
    $("body").append('<iframe hidden src="" load="iframeLoad(0)" id="iframe"></iframe>');

    $("[name='IdFuenteExterna']").on('change', function () {
        IndicadoresExternos = [];
        $("[role='alert']").hide().html("");
        if ($(this).val() == "")
            $('#_tableIndicadoresExternos').hide();
        else {
            $('#_tableIndicadoresExternos').show();
            ajaxFilter('_tableIndicadoresExternos', $(this).val());
        }
    });

});
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
            $('#' + DivId + ' table[data-table-grid="true"]').ready(initDataGrid);
            $('#' + DivId + ' table[data-table-grid="true"]').unbind('draw.dt');
            $('#' + DivId + ' table[data-table-grid="true"]').on('draw.dt', onDraw);
            $('#' + DivId + ' table[data-table-grid="true"]').trigger('draw.dt');
            initOnEnterSubmit(DivId);
            $('#' + DivId + ' form, #' + DivId + 'div').show();
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


function getFile() {   
    $("[role='alert']").hide().html("");
    var token = $('[name="__RequestVerificationToken"]').val();
    if(IndicadoresExternos.length==0){
        $("[name='warning']").show().html(closureMessage().UnselectedItems);
        return false;
    }

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);

    var url = document.URL.split("/");
    var allurl = url[0] + "//" + url[2] + "/" + url[3] + "/Download?INDICADORES=" + IndicadoresExternos;
    var frame = $("#iframe");
    frame.attr("src", allurl);
    frame.on("load", iframeLoad);

    setTimeout(function () {
        window.location = "/RegistroIndicadorExterno";
    }, 700);
    
    $("[name='btnBack']").trigger("click");
}
function iframeLoad(state) {
    $("[name='error']").show().html($("#iframe").contents().find("h3").eq(0).html());
}