
//Permite solo numeros
function soloNumeros(evt) {
    //asignamos el valor de la tecla a keynum
    //llamar con onkeypress="return soloNumeros(event);"
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }
    //comprobamos si se encuentra en el rango
    if (keynum > 47 && keynum < 58) {
        return true;
    } else {
        if (keynum == 8 || keynum == 192 ||  keynum == 32 || keynum == 8
                                     || keynum == 13) {
            return true;
        } else {
            
                return false;
            
        }
    }
}

function soloNumerosDecimales(evt) {
    //asignamos el valor de la tecla a keynum
    //llamar con onkeypress="return soloNumeros(event);"
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }
    //comprobamos si se encuentra en el rango
    if (keynum > 47 && keynum < 58) {
        return true;
    } else {
        if (keynum == 8 || keynum == 192 || keynum == 32 || keynum == 8
                                     || keynum == 13) {
            return true;
        } else {
            if (keynum == 44) {
                return true;
            } else {
                return false;
            }
        }
    }
}


/*
    Previene el back cuando se le da al borrar
*/
$(document).unbind('keydown').bind('keydown', function (event) {
    var doPrevent = false;
    if (event.keyCode === 8) {
        var d = event.srcElement || event.target;
        if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD' || d.type.toUpperCase() === 'FILE' || d.type.toUpperCase() === 'EMAIL'))
             || d.tagName.toUpperCase() === 'TEXTAREA') {
            doPrevent = d.readOnly || d.disabled;
        }
        else {
            doPrevent = true;
        }
    }

    if (doPrevent) {
        event.preventDefault();
    }
});
