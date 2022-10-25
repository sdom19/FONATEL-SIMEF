var el_ind = "";
var el_cri = "";
var Servicios = document.querySelector('#selectServicio');
var arbol;
var urlAPI = 'http://localhost/ApiDescarga/API/'; // Acá se cambia el url del api


jQuery(document).ready(function () {
    //Menu
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
    //Servicio Change
    jQuery("#selectServicio").change(function () {
        jQuery("#selectTematica").empty();
        jQuery("#selectOperador").empty();
        jQuery("#selectIndicador").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();

        var Tematica = document.querySelector('#selectTematica');
        var IdServicio = document.getElementById("selectServicio").value;
        fetch(urlAPI + 'GetTematicas?idServicio=' + IdServicio)
            .then(res => res.json())
            .then(datos => {
                cargarSelect(datos, Tematica, 'idTipoInd', 'desTipoInd')
            });
    });
    //Temática Change
    jQuery("#selectTematica").change(function () {
        jQuery("#selectOperador").empty();
        jQuery("#selectIndicador").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Operador = document.querySelector('#selectOperador');
        var IdServicio = document.getElementById("selectServicio").value;
        var IdTematica = document.getElementById("selectTematica").value;
        fetch(urlAPI + 'GetOperadores?idServicio=' + IdServicio +
            '&idTematica=' + IdTematica)
            .then(res => res.json())
            .then(datos => {
                cargarSelect(datos, Operador, 'idOperador', 'nombreOperador')
            })
    });
    //Operador Change
    jQuery("#selectOperador").change(function () {
        jQuery("#selectIndicador").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Indicador = document.querySelector('#selectIndicador');
        var IdServicio = document.getElementById("selectServicio").value;
        var IdTematica = document.getElementById("selectTematica").value;
        var Operador = document.getElementById("selectOperador").value;
        fetch(urlAPI + 'GetIndicadores?idServicio=' + IdServicio +
            '&idTematica=' + IdTematica + '&idOperador=' + Operador)
            .then(res => res.json())
            .then(datos => {
                Indicador.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>'
                for (let valor of datos) {
                    Indicador.innerHTML +=
                        `<option value="${valor.idIndicador}" data-tienesub="${valor.tienesubIndicador}" data-raiz="${valor.numIndicador}"> ${valor.desIndicador} </option>`;
                }
            })
    });
    //Indicador Change
    jQuery("#selectIndicador").change(function () {
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Criterio = document.querySelector('#selectCriterio');
        var IndicadorPadre = jQuery("#selectIndicador option:selected").val();
        var idInd = jQuery("#selectIndicador option:selected").val();
        var tienesub = jQuery("#selectIndicador option:selected").data('tienesub');
        var raizId = jQuery("#selectIndicador option:selected").data('raiz');
        if (tienesub) {
            fetch(urlAPI + 'GetCriterios?IndicadorPadre=' + IndicadorPadre)
                .then(res => res.json())
                .then(datos => {
                    if (datos.length == 1) {
                        var valor = datos[0];
                        Criterio.innerHTML = `<option value="${valor.idCriterio}" data-tienesub="${valor.tienesubIndicador}" data-raiz="${valor.numIndicador}"> ${valor.desCriterio} </option>`;
                        jQuery("#selectCriterio").trigger("change");
                    } else {
                        Criterio.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
                        for (let valor of datos) {
                            Criterio.innerHTML += `<option value="${valor.idCriterio}" data-tienesub="${valor.tienesubIndicador}" data-raiz="${valor.numIndicador}"> ${valor.desCriterio} </option>`;
                        }
                    }
                })
        } else {
            pedirFecha(raizId, idInd);
        }
    });
    //Criterio Change
    jQuery("#selectCriterio").change(function () {
        ocultarSubs();
        var idOp = jQuery("#selectOperador option:selected").val();
        var idCrit = jQuery("#selectCriterio option:selected").val();
        var opts = -1;
        fetch(urlAPI + 'GetSubIndicadores?IdCriterio=' + idCrit + '&IdOperador=' + idOp)
            .then(res => res.json())
            .then(datos => {
                arbol = datos;
                CargaSubArbol(1, '');
            });
        if (opts > 0) {
            jQuery('#selectSub1').val(opts);
        }
    });
    //Sub1 Change
    jQuery("#selectSub1").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        var idSubInd1 = jQuery("#selectSub1 option:selected").val();
        CargaSubArbol(2, idSubInd1);
    });
     //Sub2 Change
    jQuery("#selectSub2").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        var idSubInd2 = jQuery("#selectSub2 option:selected").val();
        CargaSubArbol(3, idSubInd2);
    });
     //Sub3 Change
    jQuery("#selectSub3").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        jQuery("#sub3").show();
        var idSubInd3 = jQuery("#selectSub3 option:selected").val();
        CargaSubArbol(4, idSubInd3);
    });
     //Sub4 Change
    jQuery("#selectSub4").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        jQuery("#sub3").show();
        jQuery("#sub4").show();
        var idSubInd4 = jQuery("#selectSub4 option:selected").val();
        CargaSubArbol(5, idSubInd4);
    });
     //Sub5 Change
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
     //Sub6 Change
    jQuery("#selectSub6").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        jQuery("#sub2").show();
        jQuery("#sub3").show();
        jQuery("#sub4").show();
        jQuery("#sub5").show();
        jQuery("#sub6").show();
        var idSubInd6 = jQuery("#selectSub6 option:selected").val();
        CargaSubArbol(7, idSubInd6);
    });
     //Sub7 Change
    jQuery("#selectSub7").change(function () { // ocultamos los demas campos
        var idSubInd7 = jQuery("#selectSub7 option:selected").val();
        CargaSubArbol(8, idSubInd7);
    });
     //Año Inicial Change
    jQuery("#cbAno0").change(function () {
        var idU = ""; //jQuery("#txUsuario").val();
        var idA = jQuery("#cbAno0").val();
        var idOp = jQuery("#selectOperador option:selected").val();
        var mes0 = document.querySelector('#cbMes0');
        mes0.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
        fetch(urlAPI + 'GetParametrosMeses?idOperador=' + idOp + '&idIndicador=' + el_ind + "&idUsuario=" + idU + "&ano=" + idA)
            .then(res => res.json())
            .then(datos => {
                for (let obj of datos) {
                    mes0.innerHTML += `<option value="${obj}">${mes(obj)}</option>`
                }
            })
    });
     //Año Final Change
    jQuery("#cbAno1").change(function () {
        var idU = ""; //jQuery("#txUsuario").val();
        var idA = jQuery("#cbAno1").val();
        var idOp = jQuery("#selectOperador option:selected").val();
        var mes1 = document.querySelector('#cbMes1');
        mes1.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
        fetch(urlAPI + 'GetParametrosMeses?idOperador=' + idOp + '&idIndicador=' + el_ind + "&idUsuario=" + idU + "&ano=" + idA)
            .then(res => res.json())
            .then(datos => {
                for (let obj of datos) {
                    mes1.innerHTML += `<option value="${obj}">${mes(obj)}</option>`
                }
            })
    });
});
 //Get Servicios
function GetServicios() {
    fetch(urlAPI + 'GetServicios')
        .then(res => res.json())
        .then(datos => {
            cargarSelect(datos, Servicios, 'idServicio', 'desServicio')
        });
}
 //Cargar Select Dinámico
function cargarSelect(datos, select, id, desc) {
    var tema = select;
    tema.innerHTML = "";
    if (id == 'IdOperador') {
        tema.innerHTML += '<option value="-100" selected="true" disabled="disabled">(Selecciona)</option>';
        tema.innerHTML += '<option value="-1">TODOS LOS OPERADORES</option>';
    } else {
        tema.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    }
    for (let valor of datos) {
        tema.innerHTML += `<option value="${valor[id]}"> ${valor[desc]}</option>`
    }
}
 //Pedir fechas por año
function pedirFecha() {
    jQuery("#cbMes0").empty();
    jQuery("#cbMes1").empty();
    jQuery("#add").attr("disabled", true);
    var idOp = jQuery("#selectOperador option:selected").val();
    var idInd = jQuery("#selectIndicador option:selected").val();
    el_ind = idInd;
    var ano0 = document.querySelector('#cbAno0');
    ano0.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    fetch(urlAPI + 'GetParametrosAnos?idOperador=' + idOp + '&idIndicador=' + el_ind)
        .then(res => res.json())
        .then(datos => {
            for (let obj of datos) {
                ano0.innerHTML += `<option value="${obj}">${obj}</option>`
            }
        });
    var ano1 = document.querySelector('#cbAno1');
    ano1.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    fetch(urlAPI + 'GetParametrosAnos?idOperador=' + idOp + '&idIndicador=' + el_ind)
        .then(res => res.json())
        .then(datos => {
            for (let obj of datos) {
                ano1.innerHTML += `<option value="${obj}">${obj}</option>`
            }
        });
    jQuery("#fechas").show();
}

var sub_last=-1;
 //Cargar arbol sub nivel
function CargaSubArbol(sub, cad) {
    var es_ultimo_nivel = false;
    var arbol_sub = [];
    var cs = subStrArr(arbol);

    var idOp = jQuery("#selectOperador option:selected").val();
    var IdServicio = document.getElementById("selectServicio").value;
    var kt = arbol[0].rutaJerarquiaHijo.split('|');
    var css = "";
    for (var i = 0; i < cs; i++) css += kt[i] + '|';
    if (cad.indexOf("Clasificación / Ingreso |") && idOp == "-1" && IdServicio==4) {
        if (sub_last == -1) {

        }
        sub_last = sub;
    }
    if (cad.indexOf("Tipo Mercado/") && idOp == "-1") {
        if (sub_last == -1) {

        }
        sub_last = sub;
    }
    if (idOp == "-1" && cad == css && sub_last == -1) {
        el_cri = cad + '%';
        pedirFecha();
        return;
    }
    var rot = "";
    jQuery.each(arbol, function (i, obj) {
        var k = obj.rutaJerarquiaHijo.split('|');
        if (k.length >= sub) {
            if (obj.rutaJerarquiaHijo.startsWith(cad) || sub == 1) {
                if(k.length>sub && es_ultimo_nivel==true && IdServicio==4) es_ultimo_nivel=false; 
                if (k.length == sub && sub != 1) es_ultimo_nivel = true;
                var iel = k[sub - 1];
                var pos_slash = iel.indexOf('/');
                rot = iel.substring(0, pos_slash);
                iel = iel.substring(pos_slash + 1, iel.length);
                var found = -1;
                jQuery.each(arbol_sub, function (j, obj1) {
                    if (obj1.des == iel) found = j;
                });
                if (found == -1) arbol_sub.push({ raiz: k[sub - 1], des: iel, id: obj.idJerarquiaIndicadorUnico, rjp: obj.rutaJerarquiaPadre });
            }
        }
    });
    if (es_ultimo_nivel || arbol_sub.length == 0) {
        el_cri = '';
       
        if (arbol_sub.length == 0 || sub_last > 0) {
            el_cri = cad + '%';
        } else {
            jQuery.each(arbol_sub, function (i, obj) {
                if (cad == obj.rjp + "|") {
                    el_cri += `${obj.id},`;

                }

            });
        }
        pedirFecha();
    }
    else {
        var el = document.querySelector('#selectSub' + sub);
        if (rot == "") {
            rot = "Subindicador nivel " + sub;
        }
        jQuery("#tSubInd" + sub).text(rot);
        jQuery("#sub" + sub).show();
        el.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
        jQuery.each(arbol_sub, function (i, obj) {
	    if(obj.des.indexOf("No Especificado")>0) { }
	    else {
               el.innerHTML += `<option value="${cad + obj.raiz + '|'}"> ${obj.des} </option>`;
	  }
        });
    }
}


function subStrArr(arr) {
    var k = arr[0].rutaJerarquiaHijo.toLowerCase().replace(/\s+/g, '').split('|');
    for (let i = 0; i < 7; i++) {
        for (let j = 1; j < arr.length; j++) {
            var k1 = arr[j].rutaJerarquiaHijo.toLowerCase().replace(/\s+/g, '').split('|');
            if (i < k.length && i < k1.length) {
                if (k[i] != k1[i]) return i;
            }
        }
    }
    return -1;
}
//Hide Selects Sub Nivel
function ocultarSubs() {
    jQuery("#sub1").hide();
    jQuery("#sub2").hide();
    jQuery("#sub3").hide();
    jQuery("#sub4").hide();
    jQuery("#sub5").hide();
    jQuery("#sub6").hide();
    jQuery("#sub7").hide();
    jQuery("#fechas").hide();
}

//Mes int -> string
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