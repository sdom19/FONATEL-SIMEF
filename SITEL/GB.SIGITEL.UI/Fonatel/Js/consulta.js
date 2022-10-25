var el_ind = "";
var el_cri = "";
var Grupo = document.querySelector('#selectGrupo');
var arbol;
var urlAPI = 'http://20.127.147.228:44313/api/DescargaIndicadores/'; // Acá se cambia el url del api


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
    GetPrograma();
    ocultarSubs();
    //Servicio Change
    jQuery("#selectGrupo").change(function () {
        jQuery("#selectTipo").empty();
        jQuery("#selectOperador").empty();
        jQuery("#selectIndicador").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();

        var Tipo = document.querySelector('#selectTipo');
        var IdServicio = document.getElementById("selectGrupo").value;
        fetch(urlAPI + 'GetTipo')
            .then(res => res.json())
            .then(datos => {
                cargarSelect(datos, Tipo, 'idTipoIdicador', 'nombre')
            });
    });
    //Temática Change
    jQuery("#selectTipo").change(function () {
        jQuery("#selectIndicador").empty();
        jQuery("#selectvariable").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Indicador = document.querySelector('#selectIndicador');
        var IdGrupo = document.getElementById("selectGrupo").value;
        var IdTipo = document.getElementById("selectTipo").value;
        fetch(urlAPI + 'GetIndicadores?Grupo=' + IdGrupo +
            '&Tipo=' + IdTipo )
            .then(res => res.json())
            .then(datos => {
                cargarSelect(datos, Indicador, 'id', 'desIndicador')
            })
    });
    //Operador Change
    jQuery("#selectOperador").change(function () {
        jQuery("#selectvariable").empty();
        jQuery("#selectCriterio").empty();
        ocultarSubs();
        var Indicador = document.querySelector('#selectIndicador');
        var IdGrupo = document.getElementById("selectGrupo").value;
        var IdTipo = document.getElementById("selectTipo").value;
        var Operador = document.getElementById("selectOperador").value;
        fetch(urlAPI + 'GetIndicadores?Grupo=' + IdGrupo +
            '&Tipo=' + IdTipo )
            .then(res => res.json())
            .then(datos => {
                cargarSelect(datos, Indicador, 'id', 'desIndicador')
                /*Indicador.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>'
                for (let valor of datos) {
                    Indicador.innerHTML +=
                        `<option value="${valor.IdIndicador}" data-tienesub="${valor.tienesubIndicador}" data-raiz="${valor.NumIndicador}"> ${valor.DesIndicador} </option>`;
                }*/
            })
    });
    //Indicador Change
    jQuery("#selectIndicador").change(function () {
        jQuery("#selectvariable").empty();
        ocultarSubs();
        var variable = document.querySelector('#selectvariable');
        var IndicadorPadre = jQuery("#selectIndicador option:selected").val();
        var idInd = jQuery("#selectIndicador option:selected").val();
        var raizId = jQuery("#selectIndicador option:selected").data('raiz');
        
            fetch(urlAPI + 'GetVariableDato?IdIndicador=' + IndicadorPadre)
                .then(res => res.json())
                .then(datos => {
                    cargarSelect(datos, variable, 'idIndicador', 'nombreVariable')
                })
        
            /*pedirFecha(raizId, idInd);*/
        
    });
    //Criterio Change
    jQuery("#selectvariable").change(function () {
        ocultarSubs();
        var Categoria = document.querySelector('#selectSub1');
        var idOp = jQuery("#selectOperador option:selected").val();
        var idCrit = jQuery("#selectCriterio option:selected").val();
        var opts = -1;
       /* fetch(urlAPI + 'GetSubIndicadores?IdCriterio=' + idCrit + '&IdOperador=' + idOp)
            .then(res => res.json())
            .then(datos => {
                arbol = datos;
                CargaSubArbol(1, '');
            });*/
        
            Categoria.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>'
            Categoria.innerHTML += '<option value="1">CD-01 CODIGO-SITIO</option>';
            
            jQuery("#sub1").show();
        
    });
    //Sub1 Change
    jQuery("#selectSub1").change(function () { // ocultamos los demas campos
        ocultarSubs();
        jQuery("#sub1").show();
        var idSubInd1 = jQuery("#selectSub1 option:selected").val();
        pedirFecha();
        //CargaSubArbol(2, idSubInd1);
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
        mes0.innerHTML += '<option value="1">Enero</option>';
        mes0.innerHTML += '<option value="2">Febrero</option>';
        mes0.innerHTML += '<option value="3">Marzo</option>';
        mes0.innerHTML += '<option value="4">Abril</option>';
        mes0.innerHTML += '<option value="5">Mayo</option>';
        mes0.innerHTML += '<option value="6">Junio</option>';
        mes0.innerHTML += '<option value="7">Julio</option>';
        mes0.innerHTML += '<option value="8">Agosto</option>';
        mes0.innerHTML += '<option value="9">Septiembre</option>';
        mes0.innerHTML += '<option value="10">Octubre</option>';
        mes0.innerHTML += '<option value="11">Noviembre</option>';
        mes0.innerHTML += '<option value="12">Diciembre</option>';
       /* fetch(urlAPI + 'GetParametrosMeses?idOperador=' + idOp + '&idIndicador=' + el_ind + "&idUsuario=" + idU + "&ano=" + idA)
            .then(res => res.json())
            .then(datos => {
                for (let obj of datos) {
                    mes0.innerHTML += `<option value="${obj}">${mes(obj)}</option>`
                }
            })*/
    });
     //Año Final Change
    jQuery("#cbAno1").change(function () {
        var idU = ""; //jQuery("#txUsuario").val();
        var idA = jQuery("#cbAno1").val();
        var idOp = jQuery("#selectOperador option:selected").val();
        var mes1 = document.querySelector('#cbMes1');
        mes1.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
        mes1.innerHTML += '<option value="1">Enero</option>';
        mes1.innerHTML += '<option value="2">Febrero</option>';
        mes1.innerHTML += '<option value="3">Marzo</option>';
        mes1.innerHTML += '<option value="4">Abril</option>';
        mes1.innerHTML += '<option value="5">Mayo</option>';
        mes1.innerHTML += '<option value="6">Junio</option>';
        mes1.innerHTML += '<option value="7">Julio</option>';
        mes1.innerHTML += '<option value="8">Agosto</option>';
        mes1.innerHTML += '<option value="9">Septiembre</option>';
        mes1.innerHTML += '<option value="10">Octubre</option>';
        mes1.innerHTML += '<option value="11">Noviembre</option>';
        mes1.innerHTML += '<option value="12">Diciembre</option>';
        /*fetch(urlAPI + 'GetParametrosMeses?idOperador=' + idOp + '&idIndicador=' + el_ind + "&idUsuario=" + idU + "&ano=" + idA)
            .then(res => res.json())
            .then(datos => {
                for (let obj of datos) {
                    mes1.innerHTML += `<option value="${obj}">${mes(obj)}</option>`
                }
            })*/
    });
});
 //Get Servicios
function GetPrograma() {
    fetch(urlAPI + 'GetGrupo')
        .then(res => res.json())
        .then(datos => {
            cargarSelect(datos, Grupo, 'idGrupo', 'nombre')
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
    //jQuery("#add").attr("disabled", true);
    var idOp = jQuery("#selectOperador option:selected").val();
    var idInd = jQuery("#selectIndicador option:selected").val();
    el_ind = idInd;
    var ano0 = document.querySelector('#cbAno0');
    ano0.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    ano0.innerHTML += '<option value="1">2018</option>';
    ano0.innerHTML += '<option value="2">2019</option>';
    ano0.innerHTML += '<option value="3">2020</option>';
    ano0.innerHTML += '<option value="4">2021</option>';
    ano0.innerHTML += '<option value="5">2022</option>';
    /*fetch(urlAPI + 'GetParametrosAnos?idOperador=' + idOp + '&idIndicador=' + el_ind)
        .then(res => res.json())
        .then(datos => {
            for (let obj of datos) {
                ano0.innerHTML += `<option value="${obj}">${obj}</option>`
            }
        });*/
    var ano1 = document.querySelector('#cbAno1');
    ano1.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    //ano1.innerHTML = '<option value="-1" selected="true" disabled="disabled">(Selecciona)</option>';
    ano1.innerHTML += '<option value="1">2018</option>';
    ano1.innerHTML += '<option value="2">2019</option>';
    ano1.innerHTML += '<option value="3">2020</option>';
    ano1.innerHTML += '<option value="4">2021</option>';
    ano1.innerHTML += '<option value="5">2022</option>';
    /*fetch(urlAPI + 'GetParametrosAnos?idOperador=' + idOp + '&idIndicador=' + el_ind)
        .then(res => res.json())
        .then(datos => {
            for (let obj of datos) {
                ano1.innerHTML += `<option value="${obj}">${obj}</option>`
            }
        });*/
    jQuery("#fechas").show();
}

var sub_last=-1;
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
//Cargar arbol sub nivel
function CargaSubArbol(sub, cad) {
    debugger;
    var es_ultimo_nivel = false;
    var arbol_sub = [];
    var cs = subStrArr(arbol);
    
    var idOp = jQuery("#selectOperador option:selected").val();
    var IdServicio = document.getElementById("selectServicio").value;
    var kt = arbol[0].RutaJerarquiaHijo.split('|');
    var css = "";
    for (var i = 0; i < cs; i++)
        css += kt[i] + '|';
    if (cad.indexOf("Clasificación / Ingreso |") && idOp == "-1" && IdServicio == 4) {
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
        var k = obj.RutaJerarquiaHijo.split('|');
        if (k.length >= sub) {
            if (obj.RutaJerarquiaHijo.startsWith(cad) || sub == 1) {
                if (k.length > sub && es_ultimo_nivel == true && IdServicio == 4)
                    es_ultimo_nivel = false;
                if (k.length == sub && sub != 1)
                    es_ultimo_nivel = true;
                var iel = k[sub - 1];
                var pos_slash = iel.indexOf('/');
                rot = iel.substring(0, pos_slash);
                iel = iel.substring(pos_slash + 1, iel.length);
                var found = -1;
                jQuery.each(arbol_sub, function (j, obj1) {
                    if (obj1.des == iel)
                        found = j;
                });
                if (found == -1)
                    arbol_sub.push({ raiz: k[sub - 1], des: iel, id: obj.IdJerarquiaIndicadorUnico, rjp: obj.RutaJerarquiaPadre });
            }
        }
    });
    //if (es_ultimo_nivel || arbol_sub.length == 0) {
        if (arbol_sub.length == 0) {
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
            if (obj.des.indexOf("No Especificado") > 0) { }
            else {
                el.innerHTML += `<option value="${cad + obj.raiz + '|'}"> ${obj.des} </option>`;
            }
        });
    }
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