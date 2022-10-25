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
            if (obj.des.indexOf("No Especificado") > 0) { }
            else {
                el.innerHTML += `<option value="${cad + obj.raiz + '|'}"> ${obj.des} </option>`;
            }
        });
    }
}
