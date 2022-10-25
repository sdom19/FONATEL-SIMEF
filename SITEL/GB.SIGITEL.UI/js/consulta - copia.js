var el_ind = "";
var el_cri = "";
var Servicios = document.querySelector('#selectServicio');
var arbol;
jQuery(document).ready(function () {
    var trigger = jQuery('.hamburger'),
        overlay = jQuery('.overlay'),
        isClosed = false;

    trigger.click(function () {
        hamburger_cross();
    });
    overlay.click(function () {
        hamburger_cross();
    });

    function hamburger_cross() {
        if (isClosed == true) {
            overlay.hide();
            trigger.removeClass('is-open');
            trigger.addClass('is-closed');
            jQuery('#wrapper').removeClass('toggled');
            isClosed = false;
        } else {
            overlay.show();
            trigger.removeClass('is-closed');
            trigger.addClass('is-open');

            isClosed = true;
        }
    }
    jQuery('[data-toggle="offcanvas"]').click(function () {
        jQuery('#wrapper').toggleClass('toggled');
    });
    //Consulta
    GetServicios();
    ocultarSubs();
    jQuery("#selectServicio").change(function () {
        jQuery("#selectTematica").empty();
        jQuery("#selectOperador").empty();
        jQuery("#selectIndicador").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();

        var Tematica = document.querySelector('#selectTematica');
        var IdServicio = document.getElementById("selectServicio").value;
        fetch('https://apidescargas.sutel.go.cr/API/GetTematicas?idServicio=' + IdServicio)
            .then(res => res.json())
            .then(datos => {
                cargarSelect(datos, Tematica, 'IdTipoInd', 'DesTipoInd')
            });
    });
    jQuery("#selectTematica").change(function () {
        jQuery("#selectOperador").empty();
        jQuery("#selectIndicador").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Operador = document.querySelector('#selectOperador');
        var IdServicio = document.getElementById("selectServicio").value;
        var IdTematica = document.getElementById("selectTematica").value;
        fetch('https://apidescargas.sutel.go.cr/API/GetOperadores?idServicio=' + IdServicio +
                '&idTematica=' + IdTematica)
            .then(res => res.json())
            .then(datos => {
                cargarSelect(datos, Operador, 'IdOperador', 'NombreOperador')
            })
    });
    jQuery("#selectOperador").change(function () {
        jQuery("#selectIndicador").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Indicador = document.querySelector('#selectIndicador');
        var IdServicio = document.getElementById("selectServicio").value;
        var IdTematica = document.getElementById("selectTematica").value;
        var Operador = document.getElementById("selectOperador").value;
        fetch('https://apidescargas.sutel.go.cr/API/GetIndicadores?idServicio=' + IdServicio +
                '&idTematica=' + IdTematica + '&idOperador=' + Operador)
            .then(res => res.json())
            .then(datos => {
                Indicador.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>'
                for (let valor of datos) {
                    Indicador.innerHTML +=
                        `<option value="${valor.IdIndicador}" data-tienesub="${valor.tienesubIndicador}" data-raiz="${valor.NumIndicador}"> ${valor.DesIndicador} </option>`;
                }
            })
    });
    jQuery("#selectIndicador").change(function () {
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Criterio = document.querySelector('#selectCriterio');
        var IndicadorPadre = jQuery("#selectIndicador option:selected").val();
        var idInd = jQuery("#selectIndicador option:selected").val();
        var tienesub = jQuery("#selectIndicador option:selected").data('tienesub');
        var raizId = jQuery("#selectIndicador option:selected").data('raiz');
        if (tienesub) {
            fetch('https://apidescargas.sutel.go.cr/API/GetCriterios?IndicadorPadre=' + IndicadorPadre)
                .then(res => res.json())
                .then(datos => {
                    if (datos.length == 1) {
                        var valor = datos[0];
                        Criterio.innerHTML = `<option value="${valor.idCriterio}" data-tienesub="${valor.tienesubIndicador}" data-raiz="${valor.NumIndicador}"> ${valor.desCriterio} </option>`;
                        jQuery("#selectCriterio").trigger("change");
                    } else {
                        Criterio.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
                        for (let valor of datos) {
                            Criterio.innerHTML += `<option value="${valor.idCriterio}" data-tienesub="${valor.tienesubIndicador}" data-raiz="${valor.NumIndicador}"> ${valor.desCriterio} </option>`;
                        }
                    }
                })
        } else {
            pedirFecha(raizId, idInd);
        }
    });
    jQuery("#selectCriterio").change(function () {
        ocultarSubs();
        var idOp = jQuery("#selectOperador option:selected").val();
        var idCrit = jQuery("#selectCriterio option:selected").val();
        var opts = -1;
        fetch('https://apidescargas.sutel.go.cr/API/GetSubIndicadores?IdCriterio=' + idCrit + '&IdOperador=' + idOp)
            .then(res => res.json())
            .then(datos => {
                arbol = datos;
                CargaSubArbol(1, '');
            });
        if (opts > 0) {
            jQuery('#selectSub1').val(opts);
        }
    });
    jQuery("#selectSub1").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        var idSubInd1 = jQuery("#selectSub1 option:selected").val();
        CargaSubArbol(2, idSubInd1);
    });
    jQuery("#selectSub2").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        var idSubInd2 = jQuery("#selectSub2 option:selected").val();
        CargaSubArbol(3, idSubInd2);
    });
    jQuery("#selectSub3").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        jQuery("#sub3").show();
        var idSubInd3 = jQuery("#selectSub3 option:selected").val();
        CargaSubArbol(4, idSubInd3);
    });
    jQuery("#selectSub4").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        jQuery("#sub3").show();
        jQuery("#sub4").show();
        var idSubInd4 = jQuery("#selectSub4 option:selected").val();
        CargaSubArbol(5, idSubInd4);
    });
    jQuery("#selectSub5").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        jQuery("#sub3").show();
        jQuery("#sub4").show();
        jQuery("#sub5").show();
        var idSubInd5 = jQuery("#selectSub5 option:selected").val();
        CargaSubArbol(6, idSubInd5);
    });
    jQuery("#selectSub6").change(function () { // ocultamos los demas campos
        var idSubInd6 = jQuery("#selectSub6 option:selected").val();
        CargaSubArbol(7, idSubInd6);
    });
    jQuery("#cbAno0").change(function () {
        var idU = ""; //jQuery("#txUsuario").val();
        var idA = jQuery("#cbAno0").val();
        var idOp = jQuery("#selectOperador option:selected").val();
        var mes0 = document.querySelector('#cbMes0');
        mes0.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
        fetch('https://apidescargas.sutel.go.cr/API/GetParametrosMeses?idOperador=' + idOp + '&idIndicador=' + el_ind + "&idUsuario=" + idU + "&ano=" + idA)
            .then(res => res.json())
            .then(datos => {
                for (let obj of datos) {
                    mes0.innerHTML += `<option value="${obj}">${mes(obj)}</option>`
                }
            })
    });
    jQuery("#cbAno1").change(function () {
        var idU = ""; //jQuery("#txUsuario").val();
        var idA = jQuery("#cbAno1").val();
        var idOp = jQuery("#selectOperador option:selected").val();
        var mes1 = document.querySelector('#cbMes1');
        mes1.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
        fetch('https://apidescargas.sutel.go.cr/API/GetParametrosMeses?idOperador=' + idOp + '&idIndicador=' + el_ind + "&idUsuario=" + idU + "&ano=" + idA)
            .then(res => res.json())
            .then(datos => {
                for (let obj of datos) {
                    mes1.innerHTML += `<option value="${obj}">${mes(obj)}</option>`
                }
            })
    });
});

function GetServicios() {
    fetch('https://apidescargas.sutel.go.cr/API/GetServicios')
        .then(res => res.json())
        .then(datos => {
            cargarSelect(datos, Servicios, 'IdServicio', 'DesServicio')
        });
}

function cargarSelect(datos, select, id, desc) {
    var tema = select;
    tema.innerHTML = "";
    if (id == 'IdOperador') {
        tema.innerHTML += '<option value="-100" selected="true" disabled="disabled">(Selecciona)</option>';
        tema.innerHTML += '<option value="-1">Todos los operadores</option>';
    } else {
        tema.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    }
    for (let valor of datos) {
        tema.innerHTML += `<option value="${valor[id]}"> ${valor[desc]}</option>`
    }
}

function pedirFecha() {
    jQuery("#cbMes0").empty();
    jQuery("#cbMes1").empty();
    jQuery("#add").attr("disabled", true);
    var idOp = jQuery("#selectOperador option:selected").val();
    var idInd = jQuery("#selectIndicador option:selected").val();
    el_ind = idInd;
    var ano0 = document.querySelector('#cbAno0');
    ano0.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    fetch('https://apidescargas.sutel.go.cr/API/GetParametrosAnos?idOperador=' + idOp + '&idIndicador=' + el_ind)
        .then(res => res.json())
        .then(datos => {
            for (let obj of datos) {
                ano0.innerHTML += `<option value="${obj}">${obj}</option>`
            }
        });
    var ano1 = document.querySelector('#cbAno1');
    ano1.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    fetch('https://apidescargas.sutel.go.cr/API/GetParametrosAnos?idOperador=' + idOp + '&idIndicador=' + el_ind)
        .then(res => res.json())
        .then(datos => {
            for (let obj of datos) {
                ano1.innerHTML += `<option value="${obj}">${obj}</option>`
            }
        });
    jQuery("#fechas").show();
}

function CargaSubArbol(sub, cad) {
    var es_ultimo_nivel = false;
    var arbol_sub = [];
    var cs = subStrArr(arbol);
    var idOp = jQuery("#selectOperador option:selected").val();
    var kt = arbol[0].RutaJerarquiaHijo.split('|');
    var css = "";
    for (var i = 0; i < cs; i++) css += kt[i] + '|';
    if (idOp == "-1" && cad == css) {
        el_cri = cad + '%';
        pedirFecha();
        return;
    }
    var rot = "";
    jQuery.each(arbol, function (i, obj) {
        var k = obj.RutaJerarquiaHijo.split('|');
        if (k.length >= sub) {
            if (obj.RutaJerarquiaHijo.startsWith(cad) || sub == 1) {
                if (k.length == sub && sub != 1) es_ultimo_nivel = true;
                var iel = k[sub - 1];
                var pos_slash = iel.indexOf('/');
                rot = iel.substring(0, pos_slash);
                iel = iel.substring(pos_slash + 1, iel.length);
                var found = -1;
                jQuery.each(arbol_sub, function (j, obj1) {
                    if (obj1.des == iel) found = j;
                });
                if (found == -1) arbol_sub.push({
                    raiz: k[sub - 1],
                    des: iel,
                    id: obj.ID_JerarquiaIndicadorUnico,
                    rjp: obj.RutaJerarquiaPadre
                });
            }
        }
    });
    if (es_ultimo_nivel || arbol_sub.length == 0) {
        el_cri = '';
        if (arbol_sub.length == 0) {
            el_cri = cad + '%';
        } else {
            jQuery.each(arbol_sub, function (i, obj) {
                if (cad == obj.rjp + "|") {
                    el_cri += `${obj.id},`;
                }

            });
        }
        pedirFecha();
    } else {
        var el = document.querySelector('#selectSub' + sub);
        if (rot == "") {
            rot = "Subindicador nivel " + sub;
        }
        jQuery("#tSubInd" + sub).text(rot);
        jQuery("#sub" + sub).show();
        el.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
        jQuery.each(arbol_sub, function (i, obj) {
            el.innerHTML += `<option value="${cad + obj.raiz + '|'}"> ${obj.des} </option>`;
        });
    }
}

function subStrArr(arr) {
    var k = arr[0].RutaJerarquiaHijo.toLowerCase().replace(/\s+/g, '').split('|');
    for (let i = 0; i < 7; i++) {
        for (let j = 1; j < arr.length; j++) {
            var k1 = arr[j].RutaJerarquiaHijo.toLowerCase().replace(/\s+/g, '').split('|');
            if (i < k.length && i < k1.length) {
                if (k[i] != k1[i]) return i;
            }
        }
    }
    return -1;
}

function ocultarSubs() {
    jQuery("#sub1").hide();
    jQuery("#sub2").hide();
    jQuery("#sub3").hide();
    jQuery("#sub4").hide();
    jQuery("#sub5").hide();
    jQuery("#sub6").hide();
    jQuery("#fechas").hide();
}

function mes(numero) {
    switch (numero) {
        case "1":
            return "Enero";
        case "2":
            return "Febrero";
        case "3":
            return "Marzo";
        case "4":
            return "Abril";
        case "5":
            return "Mayo";
        case "6":
            return "Junio";
        case "7":
            return "Julio";
        case "8":
            return "Agosto";
        case "9":
            return "Septiembre";
        case "10":
            return "Octubre";
        case "11":
            return "Noviembre";
        case "12":
            return "Diciembre";
        default:
    }

}