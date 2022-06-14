
var hayCopiado;
var nodoT = null;// este nodo para mantener la copia en memoria

var valorMinimoReglaEstadistica =0;
var valorMaximoReglaEstadistica = 0;

var ListaReglasProvincia = [];
var ListaReglasGenero = [];
var ListaReglasCanton = [];

var AlgunDetalleNivelSeleccionado = false;
var ReglaAutomaticaDisponible = false;

var Editando = false;



//Crea los tab en la pantalla
$('#constructorTab a').click(function (e) {
    e.preventDefault()
    $(this).tab('show')
})


$(document).ready(function () {
    //evento de cambio de dirección
    $('#ddlDireccion').change(function () {
        $("#IdDireccion").val(this.value);
        cargarIndicador(this.value);
        //cargarCriterio(this.value);
        limpiarIndicador();
        limpiarCriterio();
        limpiarDetallesAgrupacionAsociados();
        limpiarDetallesAgrupacionAgregar();
        limpiarDetalleAgrupacionOperador();
        limpiarCriterios();

      

    });

    
    $('#ddlTipoValor').change(function () {
        cambioTipoValor(this.value);
    });
    //evento de cambio en el ck de último nivel
    $('#ckUltimoNivel').change(function () {
        if ($(this).is(':checked')) {

            //debugger;
            $('#divRegla').removeClass('hidden');
            
            $('#ddlTipoValor').val('');            

            $("#divMensajeReglasMultiples").addClass('hidden');

           // $("#contenedorRadioButtonsRegla").addClass('hidden');
       
            $("#divMsjNoReglaAutomaticaDisponible").addClass('hidden');
        } else {

            $('#divRegla').addClass('hidden');
        }


    });

    $('#ckOperador').change(function () {
        if ($(this).is(':checked')) {
            console.log("Seleccionado");
            alert(this.value);
            this.value;
        } else {
            console.log("Des");
            this.value;
        }


    });

    $('#jstreeDetalleAgrupacion').on("changed.jstree", function (e, data) {


    });

    

});


function ResetReglaEstadisticaControls()
{
    //debugger;

    
     valorMinimoReglaEstadistica = 0;
     valorMaximoReglaEstadistica = 0;
   

     ListaReglasProvincia = [];
     ListaReglasGenero = [];
     ListaReglasCanton = [];

     AlgunDetalleNivelSeleccionado = false;
     ReglaAutomaticaDisponible = false;    
     
     $('#ckUltimoNivel').prop('checked', false);



}


 //Carga los indicadores deacuerdo a la dirección
    function cargarIndicador(idDireccion) {

        $.ajax({
            type: "POST",
            url: '_tablaIndicador',
            data: JSON.stringify({ IdDireccion: idDireccion, Nombre: '' }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {
                data = "<div id='tablaIndicador'>" + data + "</div>";

                var $form = $(data);
                $("#tablaIndicador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }

    //Carga los criterios deacuedo a la dirección
    function cargarCriterio(idIndicador) {
        var idDireccion = $('#IdDireccion').val()
        $.ajax({
            type: "POST",
            url: '_tablaCriterio',
            data: JSON.stringify({ IdDireccion: idDireccion, IdIndicador: idIndicador }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {
                data = "<div id='tablaCriterio'>" + data + "</div>";

                var $form = $(data);
                $("#tablaCriterio").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }

    //Carga los detalles agrupacion deacuerdo al operador
    function cargarDetalleAgrupacion(idOperador) {

        $.ajax({
            type: "POST",
            url: '_tablaDetalleAgrupacion',
            data: JSON.stringify({ IdOperador: idOperador, NombreOperador: '' }),
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (data) {
                data = "<div id='tablaDetalleAgrupacion'>" + data + "</div>";

                var $form = $(data);
                $("#tablaDetalleAgrupacion").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }

$(document).ready(function () {
    cargaTipoValorFecha();



    //$("#descDetalleAgrupacion").live('input paste', function (e) {
    //    if (e.target.id == 'descDetalleAgrupacion') {
           
    //        alert("jojojo");
    //    }
    //    else

    //        alert("jojojoooooooo");
    //});

});

function cargaTipoValorFecha() {
    $(function () {
        $("[id$=txtReglaInferior2]").datepicker({
            dateFormat: 'dd/mm/yy',
            beforeShow: function (textbox, instance) {
                instance.dpDiv.css({
                    marginTop: (-textbox.offsetHeight) + 'px',
                    marginLeft: textbox.offsetWidth + 'px'
                });
            },
            onClose: function (selectedDate) {
                $("[id$=txtReglaSuperior2]").datepicker("option", "minDate", selectedDate);
            }
        });
    });
    $(function () {
        $("[id$=txtReglaSuperior2]").datepicker({
            dateFormat: 'dd/mm/yy',
            beforeShow: function (textbox, instance) {
                instance.dpDiv.css({
                    marginTop: (-textbox.offsetHeight) + 'px',
                    marginLeft: textbox.offsetWidth + 'px'
                });
            }
        });
    });

    jQuery(function (cash) {
        $("[id$=txtReglaInferior2]").mask("99/99/9999");
        $("[id$=txtReglaSuperior2]").mask("99/99/9999");


    });

}


//evento de cambio de operador
function functCambioOperador (idOperador,nombreOperador) {
    cargarDetalleAgrupacion(idOperador );
    crearArbol(idOperador, nombreOperador);
    limpiarDetallesAgrupacionAgregar();
}

//**********************************Manejo de Arbol*****************************************************************
//Inicializa el arbol de detalles agrupación
function crearArbol(idoperador, nombreOperador) {
   
    var to = false;

    if( dragFunction == true){
        var plugins = ["dnd", "unique", "state", "types", "contextmenu"];
    }else{
        var plugins = [ "unique", "state", "types", "contextmenu"];
    }
    
    var idPadre = "0|0|" + idoperador;
    $('#jstreeDetalleAgrupacion').jstree("destroy").empty();
    $('#demo_q').keyup(function () {
        if (to) { clearTimeout(to); }
        to = setTimeout(function () {
            var v = $('#demo_q').val();
            $('#jstreeDetalleAgrupacion').jstree(true).search(v);
        }, 250);
    });


    testData = [{ "id": idPadre, "text": nombreOperador, "data": { "ultimoNivel": "0", "idTipoValor": "0", "valorInferior": "0", "valorSuperior": "0", "idTipoNivelDetalle": "0", "idNivel": "0" } }];
    // testData = [{ "id": idPadre, "text": nombreOperador }];
 
    $('#jstreeDetalleAgrupacion')
        .jstree({
            "core": {
                "animation": 0,
                "check_callback": true,
                "themes": { "stripes": true },
                'data': testData
            },
            "types": {
                "#": { "max_children": 1, "valid_children": ["root"] },
                "root": { "icon": "/static/3.0.2/assets/images/tree_icon.png", "valid_children": ["default"] },
                "default": { "draggable":"false" , "valid_children": ["default", "file"] },
                "file": { "icon": "glyphicon glyphicon-file", "valid_children": [] }
            },

            "plugins": plugins 
                    
           ,
            "contextmenu": {
                
                "items": function ($node) {
                    return {
                        "Edit": {
                            "label": "Editar",
                            "action": function (obj) {
                                $("#jstreeDetalleAgrupacion").addClass("disabled");
                                this.funcBtnEditarNodo(obj);
                            }
                        },
                        "Copy": {
                            "label": "Copiar",
                            "action": function (obj) {
                               
                                this.funcBtnCopiarNodo();
                               
                            }
                        },

                      
                        "Paste": {
                            "label": "Pegar",
                            "action": function (obj) {
                               
                                this.funcBtnPaste();                              
                            }
                            
                        },
            

                        "Delete": {
                            "label": "Borrar",
                            "action": function (obj) {


                                $("#tituloMensajeB").html("Eliminar");
                                $("#contenidoMensajeB").html("¿Desea Eliminar el Detalle Agrupación?");
                                $("#BorrarRama").modal('show');
                                
                               
                            }
                        }
                    };
                }
}
        });

    //$("#jstreeDetalleAgrupacion").on('paste.jstree', function (e, data) {
    //    debugger;
    //    var folderId = data.parent;
    //    //var child = data.rslt.obj.parent().find('li');
    //   // getFolderFiles(folderId, userOrOrgProfId, filesDispDiv);
    //    console.log(data);

    //}).jstree();

    $("#jstreeDetalleAgrupacion").on('paste.jstree', function (e, data) {
        debugger;
        var ref = $('#jstreeDetalleAgrupacion').jstree(true);
        var folderId = data.parent;
        //var child = data.rslt.obj.parent().find('li');
        // getFolderFiles(folderId, userOrOrgProfId, filesDispDiv);

        console.log(data);
        var sel = ref.get_selected();
        var seldos = ref.get_node(sel[0]);
        console.log(seldos.children[0]);
    }).jstree();

};

//Agrega un nodo en el arbol de detalles agrupación
function funcBtnCrearNodo() {

    debugger;

    var ref = $('#jstreeDetalleAgrupacion').jstree(true),
        sel = ref.get_selected();
        sel = sel[0];
        var nodo = ref.get_node(sel);

        var detalleAgrupacion = $('#descDetalleAgrupacion').val();
        var agrupacion = $('#descAgrupacion').val();
        var descrip = agrupacion + "/" + detalleAgrupacion;

    if (!$('#descDetalleAgrupacion').val() == '') {
    

        //if (sel != undefined) {

        if (sel == undefined) {
                funcMostrarMensaje("Seleccione un detalle agrupación en el  árbol", "Informativo");
                return false;
        }


            if (!sel.length) {
                funcMostrarMensaje("Seleccione un detalle agrupación en el  árbol", "Informativo");
                return false;
            }
        //}
       
        //if (nodo)
        if (nodo.data.ultimoNivel == "1") {
            funcMostrarMensaje("El detalle agrupación seleccionado es último nivel. Por favor edite el detalle agrupación\n seleccionado o seleccione otro detalle agrupación.", "Alerta!");
            return false;
        }

        //if (nodo)
        if (nodo.text == descrip) {
            funcMostrarMensaje("No se puede agregar un detalle agrupación sobre otro con el mismo nombre.", "Alerta!");
            return false;
        }

       
        var nodoValidacion = ref.get_node(sel[0]);
    

        if (nodoValidacion.text == descrip) {
            funcMostrarMensaje("No se puede agregar un detalle agrupación sobre otro con el mismo nombre.", "Alerta!");
            return false;
        }
       
    
        var idAgrupacion = $('#IdAgrupacion').val();
        var idDetalleAgrupacion = $('#IdDetalleAgrupacion').val();
        var idOperador = $('#idDetalleOperadorAgrupacion').val();
        var id = idDetalleAgrupacion + '|' + idAgrupacion + '|' + idOperador;
        //if (nodo)
            var idNivel = nodo.parents.length;
        //if (nodo)
        id = id + "|" + nodo.id;        

        if ($("#ckUltimoNivel").is(':checked') ) {


            if (ReglaAutomaticaDisponible && document.getElementById("rbEstadistica").checked)
                var reglaEstadistica = true;
            else
                var reglaEstadistica = false

            if (!reglaEstadistica) {

                var esValido = validarRegla();
                if (esValido == 'false') {
                    return false;
                }
            }         
                        
            debugger;

            var idTipoValor = $('#ddlTipoValor').val();
          
            var intTipoNivelDetalle = "0";        

          
            if ($('input:radio[name=rdNivelDetalle]:checked').val() == 'undefined'
                || $('input:radio[name=rdNivelDetalle]:checked').val() == undefined
                || $('input:radio[name=rdNivelDetalle]:checked').val() == null) {

                intTipoNivelDetalle = "0";

                debugger;
                if (reglaEstadistica) {

                    var strValorInferior = valorMinimoReglaEstadistica;
                    var strValorSuperior = valorMaximoReglaEstadistica;

                } else {

                    var valores = obtenerValorTipoValor(idTipoValor);

                    var strValorInferior = valores[0];
                    var strValorSuperior = valores[1];

                }



            } else {

                intTipoNivelDetalle = $('input:radio[name=rdNivelDetalle]:checked').val();
                
                //Algún radio ha sido seleccionado!!!
                
                if (reglaEstadistica) {
                    
                    var arrayReglaEstadistica = [];

                    if(intTipoNivelDetalle == 1) //provincia
                        arrayReglaEstadistica = ListaReglasProvincia;

                    if(intTipoNivelDetalle == 2) //canton
                        arrayReglaEstadistica = ListaReglasCanton;

                    if (intTipoNivelDetalle == 3) //genero
                        arrayReglaEstadistica = ListaReglasGenero;

                } else {

                    var valores = obtenerValorTipoValor(idTipoValor);

                    var strValorInferior = valores[0];
                    var strValorSuperior = valores[1];
                }
                
              
            }

            if (reglaEstadistica)
            {
                if(AlgunDetalleNivelSeleccionado)

                    sel = ref.create_node(sel, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "1", "idTipoValor": idTipoValor, "valorInferior": strValorInferior, "valorSuperior": strValorSuperior, "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": true, "nivelDetalleReglaEstadistica": arrayReglaEstadistica } }, false, false);
              
                else

                    sel = ref.create_node(sel, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "1", "idTipoValor": idTipoValor, "valorInferior": strValorInferior, "valorSuperior": strValorSuperior, "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": true, "nivelDetalleReglaEstadistica": null } }, false, false);
        
            }
            else
            {
            
                sel = ref.create_node(sel, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "1", "idTipoValor": idTipoValor, "valorInferior": strValorInferior, "valorSuperior": strValorSuperior, "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": false, "nivelDetalleReglaEstadistica": null } }, false, false);

            
            }
           
           
        } else {
            sel = ref.create_node(sel, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "0", "idTipoValor": "0", "valorInferior": "0", "valorSuperior": "0", "idTipoNivelDetalle": "0", "idNivel": idNivel, "usaReglaEstadistica": false, "nivelDetalleReglaEstadistica": null } }, false, false);

        }

        $("#jstreeDetalleAgrupacion").jstree("open_all");
        $('#descDetalleAgrupacion').val('');
        $('#IdAgrupacion').val('');
        $('#IdDetalleAgrupacion').val('');
        $('#idDetalleOperadorAgrupacion').val('');
        limpiarDetallesAgrupacionAgregar();


    } else {
        funcMostrarMensaje("Seleccione un detalle agrupación", "Informativo");


    }

    ResetReglaEstadisticaControls();


};

/// <summary>
/// esta funcion crea el Nodo 
/// para pegarlo, recibe la el nodo que va
/// crear y la posicion del nuevo padre.
/// </summary>
/// <param name="indicadores"></param>
/// Autor:Diego Navarrete Alvarez
/// <returns></returns>
function funcBtnCrearNodoPegar(Nodo, sel) {
    debugger;

    var ref = $('#jstreeDetalleAgrupacion').jstree(true);

        if (!sel.length) {
            funcMostrarMensaje("Seleccione un detalle agrupación en el  árbol.", "Informativo");
            return false;
        }
        sel = sel[0];
        Nodo = Nodo[0];
        var textAgrupacion = Nodo.text.split("/");
        var separarID = Nodo.id.split("|");

        var detalleAgrupacion = textAgrupacion[1];
        var agrupacion = textAgrupacion[0];
        var idAgrupacion = separarID[1];
        var idDetalleAgrupacion = separarID[0];
        var idOperador = separarID[2];
        var id = idDetalleAgrupacion + '|' + idAgrupacion + '|' + idOperador;
        var nodo = ref.get_node(sel);
        var idNivel = nodo.parents.length;
        id = id + "|" + nodo.id;


        if (Nodo.data.ultimoNivel == "1") {


            var idTipoValor = Nodo.data.idTipoValor;
            var arrayReglaEstadistica = null;
            var strValorInferior = 0;
            var strValorSuperior = 0;
            
            if (Nodo.data.usaReglaEstadistica && Nodo.data.nivelDetalleReglaEstadistica!= null) //Si es cierto se usan los array
            {
                arrayReglaEstadistica = Nodo.data.nivelDetalleReglaEstadistica;

            } else {

                 strValorInferior = Nodo.data.valorInferior;
                 strValorSuperior = Nodo.data.valorSuperior;
            }
            
           // var valores = obtenerValorTipoValor(idTipoValor);
           
          
            var intTipoNivelDetalle = Nodo.data.idTipoNivelDetalle;
            var usaReglaEstadistica = Nodo.data.usaReglaEstadistica;

            sel = ref.create_node(sel, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "1", "idTipoValor": idTipoValor, "valorInferior": strValorInferior, "valorSuperior": strValorSuperior, "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": usaReglaEstadistica, "nivelDetalleReglaEstadistica": arrayReglaEstadistica } }, false, false);

        } else {
            sel = ref.create_node(sel, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "0", "idTipoValor": "0", "valorInferior": "0", "valorSuperior": "0", "idTipoNivelDetalle": "0", "idNivel": idNivel, "usaReglaEstadistica": usaReglaEstadistica, "nivelDetalleReglaEstadistica": arrayReglaEstadistica } }, false, false);

        }
   

        return id ;
};


//Borra el nodo
function funcBtnBorrarNodo() { 

    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
    var sel = ref.get_selected();
    var nodo = ref.get_node(sel[0]);    

    if (!sel.length) {
        funcMostrarMensaje("Seleccione el detalle agrupación en el árbol que desea borrar.", "Informativo");
        return false;
    }
    if (nodo.parent == "#") {
        funcMostrarMensaje("El operador (detalle raíz) no se puede borrar. Seleccione otro detalle agrupación.", "Informativo");
        return false;
    }

    ref.delete_node(sel);

    $("#slcDetalleAgrupacionPadre option[value='" + sel + "']").remove();
};


function functEliminarRama() {

    this.funcBtnBorrarNodo();
    limpiarDetallesAgrupacionAgregar();
    funcBtnCancelarEditarNodo();
    $("#BorrarRama").modal('hide');

}

/// <summary>
/// Funcion Pegar LLamada en el context menu
/// </summary>
/// <param name="indicadores"></param>
/// Autor:Diego Navarrete Alvarez
/// <returns></returns>
function funcBtnCopiarNodo() {
  
    debugger;
    this.nodoT = null;

    hayCopiado = true;


    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
    debugger;
    sel = ref.get_selected();
    console.log(sel);
    var select = ref.get_node(sel[0]);


    var clone = $.extend(true, {}, select);// crea un clone de select

    if (!sel.length) {
        funcMostrarMensaje("Seleccione el detalle agrupación en el árbol que desea Copiar.", "Informativo");
        return false;
    }

    
    var nodoSeleccionado = ref.get_node(sel[0]);//obtengo el nodo solo para preguntar por la raiz 

    if (nodoSeleccionado.parent == "#") {
        funcMostrarMensaje("El operador (detalle raíz) no se puede Copiar. Seleccione otro detalle agrupación.", "Informativo");
        return false;
    }
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(100);

    var nodo = [clone];//ingresamos el clone a un array 

    recorrerArbol(nodo);
 
    // var cloneArbol = $.extend(true, {}, ref);
    //var cloneNodo = $.extend(true, {}, nodo3);
   $(".darkScreen").fadeOut(100, function () {
            $(this).remove();
        });
    this.nodoT = nodo;
   console.log(nodoT);
  

}

/// <summary>
/// Funcion Pegar llamada en el context menu
/// </summary>
/// <param name="indicadores"></param>
/// Autor:Diego Navarrete Alvarez
/// <returns></returns>
function funcBtnPaste(){
  
 
    debugger;


    if (hayCopiado == false) {
        funcMostrarMensaje("No ha Copiado ninguna Rama.", "Alerta!!");
        return false;
    }

    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
    var sel = ref.get_selected();

    var nodo = ref.get_node(sel);
    var hijo = this.nodoT;//se obtiene el clon global
    var desc = hijo[0].text ;// se obtiene nombre del para comparalo con el padre

    if (nodo.data.ultimoNivel == "1") {
       funcMostrarMensaje("El detalle agrupación seleccionado es último nivel. Por favor  edite el detalle agrupación\n seleccionado o seleccione otro detalle agrupación.", "Alerta!!");
        return false;
    }
    
    if (nodo.text == desc) {
       funcMostrarMensaje("No se puede pegar un detalle agrupación sobre si mismo u otro con el mismo nombre.", "Alerta!!");
        return false;
    }

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(100);
    
    recorrerArbolPegar(hijo, sel);//recorre el arbol 

    $(".darkScreen").fadeOut(100, function () {
        $(this).remove();
    });

     ref.open_all();


    hayCopiado = false;


}

//************************Recursividad********************************************************//

/// <summary>
/// Recorrer el Arbol guardar la estructura completa
/// </summary>
/// <param name="indicadores"></param>
/// Autor:Diego Navarrete Alvarez
/// <returns></returns>
function recorrerArbol(json) {
    var type;

    var resultado;

    for (var i = 0; i < json.length; i++) {

        type = typeof json[i].children;

        if (type == "undefined" || type == "number" || json[i].children.length == 0) {//ingresa cuando no tiene hijos

            resultado = true;
           // console.log(json[i].id);
        }

        else {
             //console.log(json[i].id);
            var nodo = recursividadBuscar(json[i].children, true)// completa el nodo
            json[i].children = nodo;
            //var nuevoHijo = [json[i].children]
            resultado = recorrerArbol(json[i].children);//envia los hijos 
        }

    }

    return resultado;

}

/// <summary>
/// Recorrer el Arbol para pegarlo
/// </summary>
/// <param name="indicadores"></param>
/// Autor:Diego Navarrete Alvarez
/// <returns></returns>
function recorrerArbolPegar(json,sel) {
    
    var type;

    var resultado;

    for (var i = 0; i < json.length; i++) {

        type = typeof json[i].children;

        if (type == "undefined" || type == "number" || json[i].children.length == 0) {// el ultimo hijo
            var hijo = [json[i]];
            var newsel = funcBtnCrearNodoPegar(hijo, sel);//crea el nodo
            resultado = true;
         
        }

        else {
          //  console.log(json[i].id);

           
            var hijo = [json[i]];


            var newsel = funcBtnCrearNodoPegar(hijo, sel);// crea el nodo
            var rama = [newsel];//codigo del padre 
          
            resultado = recorrerArbolPegar(json[i].children, rama);// recibe el hijo y el id del padre
        }

    }

    return resultado;

}


/// <summary>
/// Busca la estructura completa y la retorna 
/// </summary>
/// <param name="indicadores"></param>
/// Autor:Diego Navarrete Alvarez
/// <returns></returns>
function recursividadBuscar(id, sinHijos) {

    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
    var nodo = null;
    var hijos = [];
    var cloneArbol = $.extend(true, {}, ref);//se crea un clon del Arbol para que original no se vea afectado.

    for (var i = 0; i < id.length; i++) {
        hijos.push(cloneArbol.get_node(id[i]));   
    }
    return hijos;

}


//*************************Fin de recursividad ********************************************//

function funcBtnEditarNodo() {
    
    Editando = true;
    limpiarDetallesAgrupacionAgregar();
                                                                         
    fncCambiarTextoControl('divPnDetalle', 'Editar Detalle Agrupación');
    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
   
    var  sel = ref.get_selected();
    if (!sel.length) {
        funcMostrarMensaje("Seleccione un detalle agrupación en el árbol.", "Informativo");
        return false;
    }
    
    nodo = ref.get_node(sel[0]);
    if (nodo.parent == "#") {
        $("#jstreeDetalleAgrupacion").removeClass("disabled");
        funcMostrarMensaje("El operador (detalle raíz) no se puede editarse. Seleccione otro detalle agrupación.", "Informativo");
        return false;
    }
    
    debugger;

    if (document.getElementById(nodo.id).getElementsByTagName('li').length == 0) {
        var idNodo = nodo.id.split("|");
        var textoNodo = nodo.text.split("/");
        var id = nodo.id;
        var detalleAgrupacion = textoNodo[1];
        var agrupacion = textoNodo[0];
        var idDetalleAgrupacion = idNodo[0];
        var idAgrupacion = idNodo[1];
        var idOperador = idNodo[2];
        var ultimoNivel = nodo.data.ultimoNivel;
        var idTipoValor = nodo.data.idTipoValor;
        var valorInferior = nodo.data.valorInferior;
        var valorSuperior = nodo.data.valorSuperior;
        var idTipoNivelDetalle = nodo.data.idTipoNivelDetalle;
        var idtiponivelDetalleGenero = nodo.data.idTipoNivelDetalleGenero;
        var idNivel = nodo.data.idNivel;
        var idPadreNodo = nodo.parent;
        debugger;

        var arrayRegla = nodo.data.nivelDetalleReglaEstadistica;

        if (arrayRegla != null) {

            AlgunDetalleNivelSeleccionado = true;

            $("#divReglaValorLimiteInferior").addClass('hidden');
            $("#divReglaValorLimiteSuperior").addClass('hidden');

            $("#divMensajeReglasMultiples").removeClass('hidden');

            $("#contenedorRadioButtonsRegla").removeClass('hidden');

            $("#divMsjNoReglaAutomaticaDisponible").addClass('hidden');

        } else {

            $("#divMensajeReglasMultiples").addClass('hidden');
        }

           

        //campos
        $('#descDetalleAgrupacion').val(detalleAgrupacion);

        ResetReglaEstadisticaControls();

        $('#IdAgrupacion').val(idAgrupacion);
        $('#descAgrupacion').val(agrupacion);
        $('#IdDetalleAgrupacion').val(idDetalleAgrupacion);
        $('#idDetalleOperadorAgrupacion').val(idOperador);
        $('#idPadreNodo').val(idPadreNodo);

        if (ultimoNivel == 1) {

            $('#divRegla').removeClass('hidden');
            $("#ckUltimoNivel").prop('checked', true);

            $('#ddlTipoValor').val(idTipoValor)
            //  $('#ddlTipoValor option[value="' + idTipoValor + '"]').attr('selected', 'selected');

            if (arrayRegla == null)
            cambioTipoValor(idTipoValor);
            setValorTipoValor(idTipoValor, valorInferior, valorSuperior);

            if (nodo.data.usaReglaEstadistica) {
                $("#rbEstadistica").prop('checked', true);

                valorMaximoReglaEstadistica = nodo.data.valorSuperior;
                valorMinimoReglaEstadistica = nodo.data.valorInferior;
                setStateTxtRegla(true);

                ReglaAutomaticaDisponible = true;

            }
            else {

                setStateTxtRegla(false);
                $("#contenedorRadioButtonsRegla").removeClass('hidden');
                $("#rbManual").prop('checked', true);
                ReglaAutomaticaDisponible = false;
            }



            if (idTipoNivelDetalle != 0) {

                $("input[name=rdNivelDetalle][value=" + idTipoNivelDetalle + "]").prop('checked', true);

                AlgunDetalleNivelSeleccionado = true;

            }
        } else {
            $('#divRegla').addClass('hidden');
        }
        $('#txtNivelDetalleGenero').val(idtiponivelDetalleGenero);

        if (idtiponivelDetalleGenero == 0 || idtiponivelDetalleGenero == null) {

            $("#ckNivelDetalleGenero").prop('checked', false);
           
        }
        else
        {
            $("#ckNivelDetalleGenero").prop('checked', true);
        }
       
        //botones
        $('#btnGuardarEditarNodo').removeClass('hidden');
        $('#btnCancelarEditarNodo').removeClass('hidden');
        $('#btnAgregarNodo').addClass('hidden');
        $('#btnEditarNodo').addClass('hidden');
        $('#btnBorrarNodo').addClass('hidden');
        $('#btnBuscarDetalleAgrupacion').addClass('hidden');
    } else {
        $("#jstreeDetalleAgrupacion").removeClass("disabled");
        funcMostrarMensaje("El detalle agrupación que se puede editar debe estar en el último nivel.", "Informativo");        
    }

    Editando = false;

};

function funcBtnGuardarEditarNodo() {
   
    debugger;
     if ($("#ckUltimoNivel").is(':checked')) {
         if (!this.validarCamposAlEditarDetalleAgrupacion()) {
             return false;
         }
     }

     var ref = $('#jstreeDetalleAgrupacion').jstree(true);
  

    $("#jstreeDetalleAgrupacion").removeClass("disabled");
    funcBtnBorrarNodo();//Elimina el nodo
    var idPadreNodo = $('#idPadreNodo').val();
   

    var detalleAgrupacion = $('#descDetalleAgrupacion').val();
    var agrupacion = $('#descAgrupacion').val();
    var idAgrupacion = $('#IdAgrupacion').val();
    var idDetalleAgrupacion = $('#IdDetalleAgrupacion').val();
    var idOperador = $('#idDetalleOperadorAgrupacion').val();
    var id = idDetalleAgrupacion + '|' + idAgrupacion + '|' + idOperador;
    var nodo = ref.get_node(idPadreNodo);
    var NivelDetalleGenero = 0;
    var idNivel = nodo.parents.length;
    id = id + "|" + nodo.id;

    
    var algunTipoNivelDetalle = $('input[name=rdNivelDetalle]:checked').val();

    if ($("#ckNivelDetalleGenero").is(':checked')) {
        NivelDetalleGenero = 1;
    }else{
        NivelDetalleGenero = 0;    
    }
    
           
    if ($("#ckUltimoNivel").is(':checked') ) {

        var idTipoValor = $('#ddlTipoValor').val();

        var reglaEstadisticaSeleccionada = document.getElementById("rbEstadistica").checked;

        if (ReglaAutomaticaDisponible) {

            var esValido = validarRegla();

            if (esValido == 'false') {

                return false;

            } else {

            }
        }
       
        var strValorInferior = "";
        var strValorSuperior = "";
       
        var intTipoNivelDetalle = 0;   

        if (algunTipoNivelDetalle == undefined
            || algunTipoNivelDetalle == null
            || algunTipoNivelDetalle == "undefined") {

            intTipoNivelDetalle = "0";


            if (reglaEstadisticaSeleccionada && ReglaAutomaticaDisponible) {

                 strValorInferior = valorMinimoReglaEstadistica;
                 strValorSuperior = valorMaximoReglaEstadistica;

            } else {

                var valores = obtenerValorTipoValor(idTipoValor);

                 strValorInferior = valores[0];
                 strValorSuperior = valores[1];

            }




        } else {
            intTipoNivelDetalle = $('input:radio[name=rdNivelDetalle]:checked').val();



            //Algún radio ha sido seleccionado!!!

            if (reglaEstadisticaSeleccionada && ReglaAutomaticaDisponible) {

                var arrayReglaEstadistica = [];

                if (intTipoNivelDetalle == 1) //provincia
                    arrayReglaEstadistica = ListaReglasProvincia;

                if (intTipoNivelDetalle == 2) //canton
                    arrayReglaEstadistica = ListaReglasCanton;

                if (intTipoNivelDetalle == 3) //genero
                    arrayReglaEstadistica = ListaReglasGenero;

            } else {

                var valores = obtenerValorTipoValor(idTipoValor);

                 strValorInferior = valores[0];
                 strValorSuperior = valores[1];
            }


        }

  

        if (reglaEstadisticaSeleccionada && ReglaAutomaticaDisponible) {
            if (AlgunDetalleNivelSeleccionado)

                sel = ref.create_node(idPadreNodo, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "1", "idTipoValor": idTipoValor, "valorInferior": strValorInferior, "valorSuperior": strValorSuperior, "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": true, "nivelDetalleReglaEstadistica": arrayReglaEstadistica } }, false, false);

            else

                sel = ref.create_node(idPadreNodo, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "1", "idTipoValor": idTipoValor, "valorInferior": strValorInferior, "valorSuperior": strValorSuperior, "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": true, "nivelDetalleReglaEstadistica": null } }, false, false);

        }
        else {

            sel = ref.create_node(idPadreNodo, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "1", "idTipoValor": idTipoValor, "valorInferior": strValorInferior, "valorSuperior": strValorSuperior, "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": false, "nivelDetalleReglaEstadistica": null, "idTipoNivelDetalleGenero": NivelDetalleGenero } }, false, false);


        }

    } else {
        sel = ref.create_node(idPadreNodo, { "text": agrupacion + "/" + detalleAgrupacion, "id": id, "data": { "ultimoNivel": "0", "idTipoValor": "0", "valorInferior": "0", "valorSuperior": "0", "idTipoNivelDetalle": "0", "idNivel": idNivel, "usaReglaEstadistica": false, "nivelDetalleReglaEstadistica": null } }, false, false);

    }



    $('#jstreeDetalleAgrupacion').jstree('select_node', id);
    $('#descDetalleAgrupacion').val('');
    $('#IdAgrupacion').val('');
    $('#IdDetalleAgrupacion').val('');
    $('#idDetalleOperadorAgrupacion').val('');
    $('#idPadreNodo').val('');
    funcBtnCancelarEditarNodo();
    


};

function funcBtnCancelarEditarNodo() {

    $("#jstreeDetalleAgrupacion").removeClass("disabled");

    limpiarDetallesAgrupacionAgregar();
    $('#btnGuardarEditarNodo').addClass('hidden');
    $('#btnCancelarEditarNodo').addClass('hidden');
    $('#btnAgregarNodo').removeClass('hidden');
    //$('#btnEditarNodo').removeClass('hidden');
    //$('#btnBorrarNodo').removeClass('hidden');
    $('#btnBuscarDetalleAgrupacion').removeClass('hidden');
}

function obtenerValorTipoValor(idTipoValor) {
    var valorInferior = "0";
    var valorSuperior = "0";
    switch (idTipoValor) {
        case '2'://fecha
            valorInferior = $("#txtReglaInferior2").val();
            valorSuperior = $("#txtReglaSuperior2").val();
            break;
        case '3'://porcentaje
        case '4'://Monto
        case '5'://Cantidad
        case '7':
            valorInferior = $("#txtReglaInferior3").val();
            valorSuperior = $("#txtReglaSuperior3").val();
            break;
        case '6':
            valorInferior = $("#txtReglaInferior4").val();
            valorSuperior = $("#txtReglaSuperior4").val();
            break;
        default:
            valorInferior = $("#txtReglaInferior").val();
            valorSuperior = $("#txtReglaSuperior").val();
            break;
    }


    return [valorInferior, valorSuperior];
}

function establecerValoresReglaEstadistica(idTipoValor, valorInferior, valorSuperior) {
   
    debugger;
    idTipoValor = String(idTipoValor);

    switch (idTipoValor) {

        case '2' ://fecha
            $("#txtReglaInferior2").val(valorInferior) ;
            $("#txtReglaSuperior2").val(valorSuperior) ;
            break;
        case '3'://porcentaje
        case '4'://Monto
        case '5'://Cantidad
        case '7':
            $("#txtReglaInferior3").val(valorInferior) ;
            $("#txtReglaSuperior3").val(valorSuperior) ;
            break;
        case '6':
            $("#txtReglaInferior4").val(valorInferior) ;
            $("#txtReglaSuperior4").val(valorSuperior)  ;
            break;
        default:
            $("#txtReglaInferior").val(valorInferior)  ;
            $("#txtReglaSuperior").val(valorSuperior);
            break;
    }
    
}

function setValorTipoValor(idTipoValor, valorInferior, valorSuperior) {

    

    switch (String(idTipoValor)) {
        case '2':
            $("#txtReglaInferior2").val(valorInferior);
            $("#txtReglaSuperior2").val(valorSuperior);
            break;
        case '3'://porcentaje
        case '4'://Monto
        case '5'://Cantidad
        case '7':
            $("#txtReglaInferior3").val(valorInferior);
            $("#txtReglaSuperior3").val(valorSuperior);
            break;
        case '6':
            $("#txtReglaInferior4").val(valorInferior);
            $("#txtReglaSuperior4").val(valorSuperior);
            break;
        default:
            $("#txtReglaInferior").val(valorInferior);
            $("#txtReglaSuperior").val(valorSuperior);
            break;
    }

}

//************************************Detalle Agrupacion*************************************************************

function btnGuardarArbolDetalleAgrupacion() {

    var idOperador = $('#IdOperador').val();
    var json = [];
    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
     ref.open_all();
    if (validarDetallesAgrupacionPorOperador() == 'true') {
        var v = $("#jstreeDetalleAgrupacion").jstree(true).get_json('#', { 'flat': true });
        for (var i = 0; i < v.length; i++) {
            var text = v[i].text;
            var id = v[i].id;
            var parent = v[i].parent;
            var ckUltimoNivel = v[i].data.ultimoNivel;
            var idTipoValor = v[i].data.idTipoValor;
            var strValorInferior = v[i].data.valorInferior;
            var strValorSuperior = v[i].data.valorSuperior;
            var intTipoNivelDetalle = v[i].data.idTipoNivelDetalle;
            var idNivel = v[i].data.idNivel;
            var usaReglaEstadistica = v[i].data.usaReglaEstadistica;
         
            if (document.getElementById(id).getElementsByTagName('li').length >= 1) {
                ckUltimoNivel = 0;
            }
            else {

                //ES ÚLTIMO NIVEL

                ckUltimoNivel = 1;

                var arrayRegla = v[i].data.nivelDetalleReglaEstadistica;

               // var usaReglaEstadistica = v[i].data.usaReglaEstadistica;


            }
            if (ckUltimoNivel == 1 && idTipoValor == 0 && parent !="#") {
                funcMostrarMensaje("El detalle agrupación <strong>" + text + "</strong>, se encuentra en el último nivel. Por favor,  defínale  una regla.", "Informativo");
                return false;
            }

            debugger;

            json.push({
                "id": id, "text": text, "parent": parent, "ultimoNivel": ckUltimoNivel, "idTipoValor": idTipoValor,
                "valorInferior": strValorInferior, "valorSuperior": strValorSuperior,
                "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": usaReglaEstadistica, "nivelDetalleReglaEstadistica": arrayRegla
            });
        }

        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(100);

      
        $.ajax({

            type: 'POST',
            url: '_agregarDetalleAgrupacion',
            data: JSON.stringify(json),
            contentType: 'application/json; charset=utf-8',
            cache: false,
            success: function (data) {
                data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

                var $form = $(data);
                $("#divDetalleAgrupacionOperador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }
                $('#IdOperador').val('');
                $('#nombreOperador').val('');
                limpiarDetallesAgrupacionAsociados();
                limpiarDetallesAgrupacionAgregar();
                limpiarOperadoresCriterio();

                setTimeout(function () {
                    $(".darkScreen").fadeOut(100, function () {
                        $(this).remove();
                    });

                    addSuccess({ msg: "La información del detalle agrupación se ha agregado con éxito." });

                }, 500);
            },
            error: function () {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
    funcBtnCancelarEditarNodo();
}
///
///newM
//
function btnGuardarArbolDetalleAgrupacionEditar() {

    var idOperador = $('#IdOperador').val();
    var json = [];
    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
    ref.open_all();
    if (validarDetallesAgrupacionPorOperador() == 'true') {
        var v = $("#jstreeDetalleAgrupacion").jstree(true).get_json('#', { 'flat': true });
        for (var i = 0; i < v.length; i++) {
            var text = v[i].text;
            var id = v[i].id;
            var parent = v[i].parent;
            var ckUltimoNivel = v[i].data.ultimoNivel;
            var idTipoValor = v[i].data.idTipoValor;
            var strValorInferior = v[i].data.valorInferior;
            var strValorSuperior = v[i].data.valorSuperior;
            var intTipoNivelDetalle = v[i].data.idTipoNivelDetalle;
            var idNivel = v[i].data.idNivel;
            var usaReglaEstadistica = v[i].data.usaReglaEstadistica;

            if (document.getElementById(id).getElementsByTagName('li').length >= 1) {
                ckUltimoNivel = 0;
            }
            else {

                //ES ÚLTIMO NIVEL
                ckUltimoNivel = 1;
                var arrayRegla = v[i].data.nivelDetalleReglaEstadistica;
                // var usaReglaEstadistica = v[i].data.usaReglaEstadistica;
            }
            if (ckUltimoNivel == 1 && idTipoValor == 0 && parent != "#") {
                funcMostrarMensaje("El detalle agrupación <strong>" + text + "</strong>, se encuentra en el último nivel. Por favor,  defínale  una regla.", "Informativo");
                return false;
            }

            debugger;

            json.push({
                "id": id, "text": text, "parent": parent, "ultimoNivel": ckUltimoNivel, "idTipoValor": idTipoValor,
                "valorInferior": strValorInferior, "valorSuperior": strValorSuperior,
                "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": usaReglaEstadistica, "nivelDetalleReglaEstadistica": arrayRegla
            });
        }

        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(100);

        var urlO = document.URL;
        $.ajax({

            type: 'POST',
            url: '_agregarDetalleAgrupacionEditar',
            data: JSON.stringify({ Listadetalles: json, url: urlO }),
            contentType: 'application/json; charset=utf-8',
            cache: false,
            success: function (data) {
                data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

                var $form = $(data);
                $("#divDetalleAgrupacionOperador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }
                $('#IdOperador').val('');
                $('#nombreOperador').val('');
                limpiarDetallesAgrupacionAsociados();
                limpiarDetallesAgrupacionAgregar();
                limpiarOperadoresCriterio();

                setTimeout(function () {
                    $(".darkScreen").fadeOut(100, function () {
                        $(this).remove();
                    });

                    addSuccess({ msg: "La información del detalle agrupación se ha agregado con éxito." });
                    gVolverEditarOperador2();
                }, 500);
              
            },
            error: function () {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
                gVolverEditarOperador2();
            }

        });
    }
    funcBtnCancelarEditarNodo();
}
///
///newM
///
function functEditarArbolDetalleAgrupacionNuevo() {
    debugger;
    var url = document.URL.split("=");
    var variable = url[1].split("&");
    var variable2 = url[2].split("&");
    var Idconstructor = variable[0];
    var Idcriterio = variable2[0];
    var idOperador = url[3];
    var json = [];
  

    if (dragFunction == true) {
        var plugins = ["dnd", "unique", "state", "types", "contextmenu"];
    } else {
        var plugins = ["unique", "state", "types", "contextmenu"];
    }

    limpiarDetallesAgrupacionAsociados();
    $.ajax({
        type: "POST",
        url: '_newdetalleAgrupacionEditar',
        data: JSON.stringify({ idOperadorSeleccionado: idOperador, idOperadoreClonar: "", idConstructor: Idconstructor, idCriterio: Idcriterio }),
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            debugger;
            var nombreOperador = jsonData.data[0].text;
            var dataDetalles = jsonData.data;
            for (var i = 0; i < dataDetalles.length; i++) {

                var text = dataDetalles[i].text;
                var id = dataDetalles[i].id;
                var parent = dataDetalles[i].parent;
                var ckUltimoNivel = dataDetalles[i].ultimoNivel;
                var idTipoValor = dataDetalles[i].idTipoValor;
                var strValorInferior = dataDetalles[i].valorInferior;
                var strValorSuperior = dataDetalles[i].valorSuperior;
                var usaEstadistica = dataDetalles[i].UsaReglaEstadistica;
                var arrayReglaEstadistica = dataDetalles[i].nivelDetalleReglaEstadistica;
                var intTipoNivelDetalle = dataDetalles[i].idTipoNivelDetalle;
                var intTipoNivelDetalleGenero = dataDetalles[i].idTipoNivelDetalleGenero;

                var idNivel = dataDetalles[i].idNivel;
                
                json.push({
                    "id": id, "text": text, "parent": parent, "data": {
                        "ultimoNivel": ckUltimoNivel, "idTipoValor": idTipoValor
                        , "valorInferior": strValorInferior, "valorSuperior": strValorSuperior
                        , "usaReglaEstadistica": usaEstadistica
                        , "nivelDetalleReglaEstadistica": arrayReglaEstadistica
                        , "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel
                        , "idTipoNivelDetalleGenero": intTipoNivelDetalleGenero
                    }
                });
            }


            $('#jstreeDetalleAgrupacion')
     .jstree({
         "core": {
             "animation": 0,
             "check_callback": true,
             "themes": { "stripes": true },
             'data': json
         },
         "types": {
             "#": { "max_children": 1, "valid_children": ["root"] },
             "root": { "icon": "/static/3.0.2/assets/images/tree_icon.png", "valid_children": ["default"] },
             "default": { "draggable": "false", "valid_children": ["default", "file"] },
             "file": { "icon": "glyphicon glyphicon-file", "valid_children": [] }
         },
         // "dnd",
         "plugins": plugins,
         "contextmenu": {
             "items": function ($node) {
                 return {
                     "Edit": {
                         "label": "Editar",
                         "action": function (obj) {
                             $("#jstreeDetalleAgrupacion").addClass("disabled");
                             this.funcBtnEditarNodo(obj);
                         }
                     },
                     "Copy": {
                         "label": "Copiar",
                         "action": function (obj) {
                             debugger;
                             this.funcBtnCopiarNodo();

                         }
                     },
                     "Paste": {
                         "label": "Pegar",
                         "action": function (obj) {
                             debugger;
                             this.funcBtnPaste();
                         }

                     },
                     "Delete": {
                         "label": "Borrar",
                         "action": function (obj) {

                             $("#tituloMensajeB").html("Eliminar");
                             $("#contenidoMensajeB").html("¿Desea Eliminar el Detalle Agrupación?");
                             $("#BorrarRama").modal('show');
                         }
                     }
                 };
             }
         }
     });

            fncPosicionarScrooll('IdOperador');
            $('#btnGuardarEdicionArbolDetalleOperador').removeClass('hidden');
            $('#btnCancelarEdicionArbolDetalleOperador').removeClass('hidden');
           // $('#btnAgregarArbolDetalleOperador').addClass('hidden');
            $('#btnBuscarOperador').addClass('hidden');
            fncCambiarTextoControl('divPrincipalAgregarDetalleAgrupacion', '  <h3><small>Editar Detalle Agrupación</small> </h3>');
            agregarOperador(idOperador, nombreOperador);

            cargarDetalleAgrupacion(idOperador);
            var ref = $('#jstreeDetalleAgrupacion').jstree(true);
            ref.open_all();
        },
        error: function () {

            $("#jstreeDetalleAgrupacion").html("");
        }
    });


    funcBtnCancelarEditarNodo();


}

function functEditarArbolDetalleAgrupacion(idOperador, nombreOperador) {
    var json = [];
    debugger;
    if (dragFunction == true) {
        var plugins = ["dnd", "unique", "state", "types", "contextmenu"];
    } else {
        var plugins = ["unique", "state", "types", "contextmenu"];
    }

    limpiarDetallesAgrupacionAsociados();
    $.ajax({
        type: "POST",
        url: '_detalleAgrupacionEditar',
        data: JSON.stringify({ IdOperador: idOperador, NombreOperador: nombreOperador }),
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            debugger;
            var dataDetalles = jsonData.data;
            for (var i = 0; i < dataDetalles.length; i++) {

                var text = dataDetalles[i].text;
                var id = dataDetalles[i].id;
                var parent = dataDetalles[i].parent;
                var ckUltimoNivel = dataDetalles[i].ultimoNivel;
                var idTipoValor = dataDetalles[i].idTipoValor;
                var strValorInferior = dataDetalles[i].valorInferior;
                var strValorSuperior = dataDetalles[i].valorSuperior;
                var usaEstadistica = dataDetalles[i].UsaReglaEstadistica;
                var arrayReglaEstadistica = dataDetalles[i].nivelDetalleReglaEstadistica;
                var intTipoNivelDetalle = dataDetalles[i].idTipoNivelDetalle;
                var idNivel = dataDetalles[i].idNivel;
                
                json.push({
                    "id": id, "text": text, "parent": parent, "data": {
                        "ultimoNivel": ckUltimoNivel, "idTipoValor": idTipoValor
                        ,"valorInferior": strValorInferior, "valorSuperior": strValorSuperior
                        , "usaReglaEstadistica": usaEstadistica
                        , "nivelDetalleReglaEstadistica": arrayReglaEstadistica
                        ,"idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel
                    }
                });
            }


            $('#jstreeDetalleAgrupacion')
     .jstree({
         "core": {
             "animation": 0,
             "check_callback": true,
             "themes": { "stripes": true },
             'data': json
         },
         "types": {
             "#": { "max_children": 1, "valid_children": ["root"] },
             "root": { "icon": "/static/3.0.2/assets/images/tree_icon.png", "valid_children": ["default"] },
             "default": { "draggable": "false", "valid_children": ["default", "file"] },
             "file": { "icon": "glyphicon glyphicon-file", "valid_children": [] }
         },
         // "dnd",
         "plugins": plugins,
         "contextmenu": {
             "items": function ($node) {
                 return {
                     "Edit": {
                         "label": "Editar",
                         "action": function (obj) {
                             $("#jstreeDetalleAgrupacion").addClass("disabled");
                             this.funcBtnEditarNodo(obj);
                         }
                     },
                     "Copy": {
                         "label": "Copiar",
                         "action": function (obj) {
                             debugger;
                             this.funcBtnCopiarNodo();

                         }
                     },
                     "Paste": {
                         "label": "Pegar",
                         "action": function (obj) {
                             debugger;
                             this.funcBtnPaste();
                         }

                     },
                     "Delete": {
                         "label": "Borrar",
                         "action": function (obj) {

                             $("#tituloMensajeB").html("Eliminar");
                             $("#contenidoMensajeB").html("¿Desea Eliminar el Detalle Agrupación?");
                             $("#BorrarRama").modal('show');
                         }
                     }
                 };
             }
         }
     });

            fncPosicionarScrooll('IdOperador');
            $('#btnGuardarEdicionArbolDetalleOperador').removeClass('hidden');
            $('#btnCancelarEdicionArbolDetalleOperador').removeClass('hidden');
            $('#btnAgregarArbolDetalleOperador').addClass('hidden');
            $('#btnBuscarOperador').addClass('hidden');
            fncCambiarTextoControl('divPrincipalAgregarDetalleAgrupacion', 'Editar Detalle Agrupación');
            agregarOperador(idOperador, nombreOperador);
            
            cargarDetalleAgrupacion(idOperador);
            var ref = $('#jstreeDetalleAgrupacion').jstree(true);
            ref.open_all();
        },
        error: function () {

            $("#jstreeDetalleAgrupacion").html("");
        }
    });


    funcBtnCancelarEditarNodo();

  
}

function functGuardarEdicionArbolDetalleAgrupacion() {

    debugger;
    var json = [];
    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
    ref.open_all();
    if (validarDetallesAgrupacionPorOperador() == 'true') {
        var v = $("#jstreeDetalleAgrupacion").jstree(true).get_json('#', { 'flat': true });
        if (validarDetallesAgrupacionPorOperador() == 'true') {
            for (var i = 0; i < v.length; i++) {
                var text = v[i].text;
                var id = v[i].id;
                var parent = v[i].parent;
                var ckUltimoNivel = v[i].data.ultimoNivel;
                var idTipoValor = v[i].data.idTipoValor;
                var strValorInferior = v[i].data.valorInferior;
                var strValorSuperior = v[i].data.valorSuperior;
                var usaReglaEstadistica = v[i].data.usaReglaEstadistica;

                var intTipoNivelDetalle = v[i].data.idTipoNivelDetalle;
                var idNivel = v[i].data.idNivel;
                console.log(id);
                if (document.getElementById(id).getElementsByTagName('li').length >= 1) {
                    ckUltimoNivel = 0;
                } else {
                    ckUltimoNivel = 1;
                    var arrayRegla = v[i].data.nivelDetalleReglaEstadistica;
                }
                if (ckUltimoNivel == 1 && idTipoValor == 0 && parent != "#") {
                    funcMostrarMensaje("El detalle agrupación <strong>" + text + "</strong>, se encuentra en el último nivel. Por favor, defínale una regla.", "Informativo");
                    return false;
                }

                debugger;
                json.push({
                    "id": id, "text": text, "parent": parent, "ultimoNivel": ckUltimoNivel, "idTipoValor": idTipoValor,
                    "valorInferior": strValorInferior, "valorSuperior": strValorSuperior,
                    "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": usaReglaEstadistica, "nivelDetalleReglaEstadistica": arrayRegla
                });
            }

            $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
            $(".darkScreen").fadeIn(100);

            $.ajax({

                type: 'POST',
                url: '_guardarEdicionDetalleAgrupacion',
                data: JSON.stringify(json),
                contentType: 'application/json; charset=utf-8',
                cache: false,
                success: function (data) {
                    data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

                    var $form = $(data);
                    $("#divDetalleAgrupacionOperador").replaceWith(data);

                    if (typeof initDataGrid !== 'undefined') {
                        $('table[data-table-grid="true"]').ready(initDataGrid);
                    }
                    $('#btnGuardarEdicionArbolDetalleOperador').addClass('hidden');
                    $('#btnCancelarEdicionArbolDetalleOperador').addClass('hidden');
                    $('#btnAgregarArbolDetalleOperador').removeClass('hidden');
                    functCancelarEdicionArbolDetalleAgrupacion();

                    setTimeout(function () {
                        $(".darkScreen").fadeOut(100, function () {
                            $(this).remove();
                        });

                        addSuccess({ msg: "La información del detalle agrupación se ha agregado con éxito." });

                    }, 500);                    
                },
                error: function () {
                    $(".darkScreen").fadeOut(100, function () {
                        $(this).remove();
                    });
                    funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
                }
            });

        }
    }
}
//
//newM
//
function functGuardarEditarArbolDetalleAgrupacion() {

    debugger;
    var json = [];
    var ref = $('#jstreeDetalleAgrupacion').jstree(true);
    ref.open_all();
    if (validarDetallesAgrupacionPorOperador() == 'true') {
        var v = $("#jstreeDetalleAgrupacion").jstree(true).get_json('#', { 'flat': true });
        if (validarDetallesAgrupacionPorOperador() == 'true') {
            for (var i = 0; i < v.length; i++) {
                var text = v[i].text;
                var id = v[i].id;
                var parent = v[i].parent;
                var ckUltimoNivel = v[i].data.ultimoNivel;
                var idTipoValor = v[i].data.idTipoValor;
                var strValorInferior = v[i].data.valorInferior;
                var strValorSuperior = v[i].data.valorSuperior;
                var usaReglaEstadistica = v[i].data.usaReglaEstadistica;

                var intTipoNivelDetalle = v[i].data.idTipoNivelDetalle;
                var idNivel = v[i].data.idNivel;
                var intTipoNivelDetalleGenero = v[i].data.idTipoNivelDetalleGenero;
                console.log(id);
                if (document.getElementById(id).getElementsByTagName('li').length >= 1) {
                    ckUltimoNivel = 0;
                } else {
                    ckUltimoNivel = 1;
                    var arrayRegla = v[i].data.nivelDetalleReglaEstadistica;
                }
                if (ckUltimoNivel == 1 && idTipoValor == 0 && parent != "#") {
                    funcMostrarMensaje("El detalle agrupación <strong>" + text + "</strong>, se encuentra en el último nivel. Por favor, defínale una regla.", "Informativo");
                    return false;
                }

              
                json.push({
                    "id": id, "text": text, "parent": parent, "ultimoNivel": ckUltimoNivel, "idTipoValor": idTipoValor,
                    "valorInferior": strValorInferior, "valorSuperior": strValorSuperior,
                    "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel, "usaReglaEstadistica": usaReglaEstadistica, "nivelDetalleReglaEstadistica": arrayRegla,"idTipoNivelDetalleGenero": intTipoNivelDetalleGenero
                });
            }

            $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
            $(".darkScreen").fadeIn(100);
            var urlO = document.URL;
            $.ajax({

                type: 'POST',
                url: '_guardarEditarDetalleAgrupacion',
                data: JSON.stringify({ Listadetalles: json, url: urlO }),
                contentType: 'application/json; charset=utf-8',
                cache: false,
                success: function (data) {
                    data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

                    var $form = $(data);
                    $("#divDetalleAgrupacionOperador").replaceWith(data);

                    if (typeof initDataGrid !== 'undefined') {
                        $('table[data-table-grid="true"]').ready(initDataGrid);
                    }
                    $('#btnGuardarEdicionArbolDetalleOperador').addClass('hidden');
                    $('#btnCancelarEdicionArbolDetalleOperador').addClass('hidden');
                   // $('#btnAgregarArbolDetalleOperador').removeClass('hidden');
                    functCancelarEdicionArbolDetalleAgrupacion();

                    setTimeout(function () {
                        $(".darkScreen").fadeOut(100, function () {
                            $(this).remove();
                        });

                        addSuccess({ msg: "La información del detalle agrupación se ha agregado con éxito." });
                        
                    }, 500);
                    gVolverEditarOperador();
                },
                error: function () {
                    $(".darkScreen").fadeOut(100, function () {
                        $(this).remove();
                    });
                    funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
                    gVolverEditarOperador();
                }
            });

        }
    }
}

function functCancelarEditarArbolDetalleAgrupacion() {
    $('#btnGuardarEdicionArbolDetalleOperador').addClass('hidden');
    $('#btnCancelarEdicionArbolDetalleOperador').addClass('hidden');
    $('#btnAgregarArbolDetalleOperador').removeClass('hidden');
    $('#btnBuscarOperador').removeClass('hidden');
    $('#IdOperador').val('');
    $('#nombreOperador').val('');
    fncCambiarTextoControl('divPrincipalAgregarDetalleAgrupacion', 'Agregar Detalle Agrupación');
    limpiarDetallesAgrupacionAsociados();
    limpiarDetallesAgrupacionAgregar();
    funcBtnCancelarEditarNodo();
    gVolverEditarOperador();
}

function functCancelarCrearArbolDetalleAgrupacion() {  
    $('#IdOperador').val('');
    $('#nombreOperador').val('');
    fncCambiarTextoControl('divPrincipalAgregarDetalleAgrupacion', 'Agregar Detalle Agrupación');
    limpiarDetallesAgrupacionAsociados();
    limpiarDetallesAgrupacionAgregar();
    gVolverEditarOperador2();
}

function functCancelarEdicionArbolDetalleAgrupacion() {
    $('#btnGuardarEdicionArbolDetalleOperador').addClass('hidden');
    $('#btnCancelarEdicionArbolDetalleOperador').addClass('hidden');
    $('#btnAgregarArbolDetalleOperador').removeClass('hidden');
    $('#btnBuscarOperador').removeClass('hidden');
    $('#IdOperador').val('');
    $('#nombreOperador').val('');
    fncCambiarTextoControl('divPrincipalAgregarDetalleAgrupacion', 'Agregar Detalle Agrupación');
    limpiarDetallesAgrupacionAsociados();
    limpiarDetallesAgrupacionAgregar();
    funcBtnCancelarEditarNodo();
}


function functEliminarArbolDetalleAgrupacion() {
    var idOperador = $('#IDOperadorEliminar').val();
    var nombreOperador = $("#txtOperadorEliminar").val();

    $.ajax({

        type: 'POST',
        url: '_detalleAgrupacionEliminar',
        data: JSON.stringify({ IdOperador: idOperador, NombreOperador: nombreOperador }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

            var $form = $(data);
            $("#divDetalleAgrupacionOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }
            addSuccess({ msg: "La información se ha eliminado con éxito." })
            $("#modalDetalleAgrupacionEliminar").modal('hide');
            agregarOperador(idOperador, nombreOperador);
            functCancelarEdicionArbolDetalleAgrupacion()
        },
        error: function () {
            $("#divMensajeErroEliminarConstructor").show();
            $("#idMensajeErrorCuerpoEliminarConstructor").html(msg);
        }
    });
    funcBtnCancelarEditarNodo();
}
///
///NewM
///
function functEliminarArbolDetalleAgrupacionEditar() {
    var idOperador = $('#IDOperadorEliminar').val();
    var idConstructor = $("#IDConstructor").val();
    var idCriterio = $("#IDCriterio").val();

    $.ajax({

        type: 'POST',
        url: '_detalleAgrupacionEliminarEditar',
        data: JSON.stringify({ IdOperador: idOperador, IdConstructor: idConstructor, IdCriterio: idCriterio }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

            var $form = $(data);
            $("#divDetalleAgrupacionOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }
           //addSuccess({ msg: "La información se ha eliminado con éxito." })
          //  $("#modalDetalleAgrupacionEliminar").modal('hide');
           // agregarOperador(idOperador, nombreOperador);
            functCancelarEdicionArbolDetalleAgrupacion();
            window.location.reload();
        },
        error: function () {
            $("#divMensajeErroEliminarConstructor").show();
            $("#idMensajeErrorCuerpoEliminarConstructor").html(msg);
            window.location.reload();
        }
    });
    funcBtnCancelarEditarNodo();
}

/// metodo a remplazar por nuevo metedo al servidor
/// 
function functClonarDetalleAgrupacion() {
    debugger;
    $("#divMensajeErroOperadorClonar").hide()
    $("#divMensajeErrorClonar").hide();
    var url = window.location.href;
    var idConstructor = url.split('=')[1];
    var idOperadorSeleccionado = $('#idOperadorClonar').val();
    if (operadoresClonar.length <= 0) {
        $("#divMensajeErroOperadorClonar").show();
        $("#idMensajeErrorCuerpoOperadorClonar").html("Debe seleccionar al menos un operador", "Error");

        return false;
    }


    $.ajax({

        type: 'POST',
        url: '_clonarDetalleAgrupacion',
        data: JSON.stringify({ idOperadorSeleccionado: idOperadorSeleccionado, idOperadoreClonar: operadoresClonar.toString() }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

            var $form = $(data);
            $("#divDetalleAgrupacionOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }
            $("#modalSeleccionarClonarOperador").modal('hide');
            operadoresClonar = [];
            limpiarOperadoresCriterio();
            functMensajeClonar();
            limpiarOperadoresClonar();

        },
        error: function () {
            $("#divMensajeErroOperadorClonar").show();
            $("#idMensajeErrorCuerpoOperadorClonar").html("Ocurrió un error. Contacte al administrador del sistema");


        }
    });
}
//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//
//NewM
function functClonarDetalleAgrupacionEditar() {
    debugger;
    $("#divMensajeErroOperadorClonar").hide()
    $("#divMensajeErrorClonar").hide();
    var url = window.location.href;
    var Constructor = url.split('=')[1];
    var IdConstructor = Constructor.split('&')[0];
    var IdCriterio = url.split('=')[2];

    var idOperadorSeleccionado = $('#idOperadorClonar').val();
    if (operadoresClonar.length <= 0) {
        $("#divMensajeErroOperadorClonar").show();
        $("#idMensajeErrorCuerpoOperadorClonar").html("Debe seleccionar al menos un operador", "Error");
       
        return false;
    }
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(100);

    $.ajax({

        type: 'POST',
        url: '_clonarDetalleAgrupacionEditar',
        data: JSON.stringify({ idOperadorSeleccionado: idOperadorSeleccionado, idOperadoreClonar: operadoresClonar.toString(), idConstructor:IdConstructor, idCriterio:IdCriterio }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

            var $form = $(data);
            $("#divDetalleAgrupacionOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }
            $("#modalSeleccionarClonarOperador").modal('hide');
            setTimeout(function () {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
            }, 500);
         
            operadoresClonar = [];
            limpiarOperadoresCriterio();
            functMensajeClonarEditar();
            limpiarOperadoresClonar();
            //window.location.reload();
        },
        error: function () {
            $("#divMensajeErroOperadorClonar").show();
            $("#idMensajeErrorCuerpoOperadorClonar").html("Ocurrió un error. Contacte al administrador del sistema");
           
            
        }
    });
}



function functMensajeClonar() {
   
    $.ajax({

        type: 'POST',
        url: '_clonarMensajeError',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            var jData = JSON.parse(data);
            console.log(data);
            if (jData.ok == "False") {
                addSuccess({ msg: "Los operadores seleccionados han sido clonados." })
            }
            else {
                funcMostrarMensaje(jData.strMensaje, "Informativo");
            }
        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}
//NewM
function functMensajeClonarEditar() {

    $.ajax({

        type: 'POST',
        url: '_clonarMensajeError',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            var jData = JSON.parse(data);
            console.log(data);
            if (jData.ok == "False") {
                addSuccess({ msg: "Los operadores seleccionados han sido clonados." });
                funcMostrarMensaje(jData.strMensaje, "Informativo");
            }
            else {
                funcMostrarMensaje(jData.strMensaje, "Informativo");
            }
        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}


//************************************ funcionalidades Adicionales***************************************************//
function fncPosicionarScrooll(idControl) {
    var offsets = document.getElementById(idControl).getBoundingClientRect();
    var top = offsets.top;
    var left = offsets.left;

    //y,x
    window.scrollTo(0, top);
}

function fncCambiarTextoControl(idControl, mensaje) {
    $("#" + idControl + "").html(mensaje);
}

//****************************************Criterio**************************************************************************//
function btnAgregarCriterio() {
    var idCriterio = $('#IdCriterio').val();
    var nombreCriterio = $("#nombreCriterio").val();
    var ayuda = $('#txtAyuda').val();
    if (validarCriterio() == 'true') {

        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(100);

        $.ajax({

            type: 'POST',

            url: '_agregarCriterio',
            data: JSON.stringify({ IdCriterio: idCriterio, DescCriterio: nombreCriterio + '|' + ayuda }),
            contentType: 'application/json; charset=utf-8',
            cache: false,
            success: function (data) {

                data = "<div id='divCriteriosSeleccionados'>" + data + "</div>";

                var $form = $(data);
                $("#divCriteriosSeleccionados").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }
                
                filtrarDetallesAgrupacionOperador('');
                 $('#IdCriterio').val('');
                 $("#nombreCriterio").val('');
                 $('#txtAyuda').val('');
                 limpiarOperadores();

                 setTimeout(function () {
                     $(".darkScreen").fadeOut(100, function () {
                         $(this).remove();
                     });

                     addSuccess({ msg: "La información del criterio  se ha agregado con éxito." });

                 }, 1000);
                
                limpiarCriterioNoSeleccionados();


            },
            error: function () {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function funEliminarCriterio() {
    var idCriterio = $('#IDCriterioEliminar').val();
    var nombreCriterio = $("#txtCriterioEliminar").val();

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(100);

    $.ajax({

        type: 'POST',
        url: '_criterioEliminar',
        data: JSON.stringify({ IdCriterio: idCriterio, DescCriterio: nombreCriterio }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divCriteriosSeleccionados'>" + data + "</div>";

            var $form = $(data);
            $("#divCriteriosSeleccionados").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }
            
            setTimeout(function () {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });

                addSuccess({ msg: "La información del criterio se ha eliminado con éxito." });

            }, 500);
            
            $("#modalCriterioEliminar").modal('hide');
            limpiarCriterioNoSeleccionados();
        },
        error: function () {
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

/// Eliminar Criterio Editar 
/// <Method>newM</Method>
///
function funEliminarCriterioEditar() {
    debugger;
    var idCriterio = $('#IDCriterioEliminar').val();
    var txtAyuda = $("#txtAyuda").val();
    var idConstructor = $("#IDConstructor").val();

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(100);

    $.ajax({

        type: 'POST',
        url: '_criterioEliminarEditar',
        data: JSON.stringify({ IdCriterio: idCriterio, Ayuda: txtAyuda, IdConstructor: idConstructor }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divCriteriosSeleccionados'>" + data + "</div>";

            var $form = $(data);
            $("#divCriteriosSeleccionados").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }

            setTimeout(function () {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });

                addSuccess({ msg: "La información del criterio se ha eliminado con éxito." });

            }, 500);

            $("#modalCriterioEliminar").modal('hide');
            limpiarCriterioNoSeleccionados();
            window.location.reload();
        },
        error: function () {
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

function funEditarCriterioEditar(idCriterio, nombreCriterio) {
    debugger;
    $('#IdCriterio').val(idCriterio);
    $('#nombreCriterio').val(nombreCriterio);
}

//function funEditarCriterio(idCriterio, nombreCriterio) {
//    $('#IdCriterio').val(idCriterio);
//    $('#nombreCriterio').val(nombreCriterio);
   
//    $.ajax({

//        type: 'POST',
//        url: '_criterioEditar',
//        data: JSON.stringify({ IdCriterio: idCriterio, DescCriterio: nombreCriterio }),
//        contentType: 'application/json; charset=utf-8',
//        cache: false,
//        success: function (data) {
//            data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

//            var $form = $(data);
//            $("#divDetalleAgrupacionOperador").replaceWith(data);

//            if (typeof initDataGrid !== 'undefined') {
//                $('table[data-table-grid="true"]').ready(initDataGrid);
//            }
//            $('#divEditarCriterio').click();
//            fncPosicionarScrooll('IdOperador');
//            $('#btnGuardarEditarCriterioConstructor').removeClass('hidden');
//            $('#btnCancelarCriterioConstructor').removeClass('hidden');
//            $('#btnAgregarCriterioConstructor').addClass('hidden');
//            fncCambiarTextoControl('divEditarCriterio', 'Editar Criterio');
//            $('#btnBuscarCriterio').addClass('hidden');
//            limpiarOperadoresCriterio();
//            filtrarDetallesAgrupacionOperador('');
//            limpiarOperadoresClonar();

//        },
//        error: function () {
//            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
//        }
//    });
//}

function funGuardarEditarCriterio() {
    var idCriterio = $('#IdCriterio').val();
    var nombreCriterio = $("#nombreCriterio").val();
    var ayuda = $('#txtAyuda').val();
    if (validarCriterio() == 'true') {

        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(100);

        $.ajax({

            type: 'POST',
            url: '_criterioGuardarEditar',
            data: JSON.stringify({ IdCriterio: idCriterio, DescCriterio: nombreCriterio + '|' + ayuda }),
            contentType: 'application/json; charset=utf-8',
            cache: false,
            success: function (data) {
                data = "<div id='divCriteriosSeleccionados'>" + data + "</div>";

                var $form = $(data);
                $("#divCriteriosSeleccionados").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }
               
                funCancelarEditarCriterio();
                limpiarCriterio();
                filtrarDetallesAgrupacionOperador('');
                limpiarCriterioNoSeleccionados();

                setTimeout(function () {
                    $(".darkScreen").fadeOut(100, function () {
                        $(this).remove();
                    });

                    addSuccess({ msg: "La información del criterio se ha guardado con éxito." });

                }, 1000);
            },
            error: function () {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function funCancelarEditarCriterio() {
    $('#btnGuardarEditarCriterioConstructor').addClass('hidden');
    $('#btnCancelarCriterioConstructor').addClass('hidden');
    $('#btnAgregarCriterioConstructor').removeClass('hidden');
    $('#btnBuscarCriterio').removeClass('hidden');
    fncCambiarTextoControl('divEditarCriterio', 'Agregar Criterio');
    limpiarDetallesAgrupacionAsociados();
    limpiarDetallesAgrupacionAgregar();
    limpiarDetalleAgrupacionOperador();
    limpiarOperadores();
    $('#IdCriterio').val('');
    $("#nombreCriterio").val('');
   
    functCancelarEdicionArbolDetalleAgrupacion();

}

function rbManualChange() {

    $("#divReglaValorLimiteInferior").removeClass('hidden');
    $("#divReglaValorLimiteSuperior").removeClass('hidden');
    $("#divMensajeReglasMultiples").addClass('hidden');    
  
    debugger;
    var idTipoValorSeleccionado = $('#ddlTipoValor').val();
    establecerValoresReglaEstadistica(idTipoValorSeleccionado, "", "");
    setStateTxtRegla(false);
}

function rbCriterioChange() {
    debugger;
  
    Idcriterio = $("input[name=rbGroupCriterio]:checked").val();

}

function rbDetalleOperadorChange() {
    Idoperador = $("input[name=rbDetalle]:checked").val();
}

function gsiguienteCrearDetalle() {
    debugger;
    var url = document.URL.split("=");
    var variable = url[1].split("&");
    var idconstructor = variable[0];
    var idcriterio = url[2];
    if (idcriterio == "" || idcriterio == null) {
        funcMostrarMensaje("Seleccione el criterio que desea Modificar.", "Informativo");
        return false;
    }
    gLlamadaControllerOperador("/CrearArbol", idconstructor, idcriterio);
}

function gSiguineteEditarOperador() {
    debugger;
    var url = document.URL.split("=");
    var idconstructor = url[1];
    var idcriterio = Idcriterio;
    $("input[name=rbGroupCriterio]:checked").prop("checked",false);
    if (idcriterio == "" || idcriterio == null) {
        funcMostrarMensaje("Seleccione el criterio que desea Modificar.", "Informativo");
        return false;
    }
   
    gLlamadaControllerOperador("/EditarOperador", idconstructor, idcriterio);
}

function gSiguineteEditarDetalle() {
    var url = document.URL.split("=");
    var variable = url[1].split("&");
    var idconstructor = variable[0];
    var idcriterio = url[2];
    var idOperador = Idoperador;
    $("input[name=rbDetalle]:checked").prop("checked", false);
    if (idOperador == "" || idOperador == null) {
        funcMostrarMensaje("Seleccione el Operador que desea Modificar.", "Informativo");
        return false;
    }
    gLlamadaControllerArbol("/EditarArbol", idconstructor, idcriterio, idOperador);
}

function gVolverEditarOperador() {
    var url = document.URL.split("=");
    var variable = url[1].split("&");
    var idconstructor = variable[0];
    var variable2 = url[2].split("&");
    var idcriterio = variable2[0];

    gLlamadaControllerOperador("/EditarOperador", idconstructor, idcriterio);
}

function gVolverEditarOperador2() {
    var url = document.URL.split("=");
    var variable = url[1].split("&");
    var idconstructor = variable[0];
    var idcriterio = url[2];

    gLlamadaControllerOperador("/EditarOperador", idconstructor, idcriterio);
}
function rbEstadisticaChange(radioButtonEstadistica) {

    debugger;
    var idTipoValor = $('#ddlTipoValor').val();
    DeterminarSiEsPosibleAplicarReglaEstadística(idTipoValor);

    if (ReglaAutomaticaDisponible) {

        if (AlgunDetalleNivelSeleccionado) {

            //mostrar msj verde
            $("#divMensajeReglasMultiples").removeClass('hidden');

            $("#divReglaValorLimiteInferior").addClass('hidden');
            $("#divReglaValorLimiteSuperior").addClass('hidden');

        }
        else {

            $("#divReglaValorLimiteInferior").removeClass('hidden');
            $("#divReglaValorLimiteSuperior").removeClass('hidden');


            var idTipoValorSeleccionado = $('#ddlTipoValor').val();
            establecerValoresReglaEstadistica(idTipoValorSeleccionado, valorMinimoReglaEstadistica, valorMaximoReglaEstadistica);

            setStateTxtRegla(true);
        }


    }




}

function DeterminarSiEsPosibleAplicarReglaEstadística(idTipoValor) {

  

        $("#divMsjNoReglaAutomaticaDisponible").addClass('hidden');


        if (idTipoValor == 2 || idTipoValor == 1 || idTipoValor == "") {

            // Si entra aquí fue porque, en el combo tipo valor, seleccionaron texto, fecha o seleccione.

            // Entonces se ocultan los radioButtons de reglas  

            $("#divMensajeReglasMultiples").addClass('hidden');

 
            $("#contenedorRadioButtonsRegla").addClass('hidden');
            

        } else {                  

            $("#contenedorRadioButtonsRegla").removeClass('hidden');
            // Es posible aplicar regla estadística, sin embargo podría no estar disponible.

            //Por ende se accede al server para determinar si existen suficientes datos para calcular desviación estandar (regla estadística)

            var idOperador = $("#IdOperador").val();
            var idDescDetalleAgrupacion = $("#IdDetalleAgrupacion").val();

            var idNivelDetalle = -1;

            var value = $('input[name=rdNivelDetalle]:checked').val();

            if (value != undefined) {

                AlgunDetalleNivelSeleccionado = true;
                idNivelDetalle = $('input[name=rdNivelDetalle]:checked').val();
               
            }

            $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
            $(".darkScreen").fadeIn(100);

            //Se limpian los campos
            var idTipoValorSeleccionado = $('#ddlTipoValor').val();
           
            establecerValoresReglaEstadistica(idTipoValorSeleccionado, "", "");

            $.ajax({

                type: 'POST',
                url: '_esPosibleAplicarRegla',
                data: JSON.stringify({ idTipoValor: idTipoValor, idOperador: idOperador, idDescDetalleAgrupacion: idDescDetalleAgrupacion, idNivelDetalle: idNivelDetalle }),
                contentType: 'application/json; charset=utf-8',
                cache: false,
                success: function (data) {

                    debugger;
                    var JsonObject = JSON.parse(data);

                    if (JsonObject.ok == "True" || JsonObject.ok) {

                        if (JsonObject.data != null) {

                            debugger;

                            ReglaAutomaticaDisponible = true;

                            if (JsonObject.data[0].IdNivelDetalle == -1) //Si es cierto no se eligió ningún detalleNivel
                            {

                                debugger;

                               
                                valorMinimoReglaEstadistica = JsonObject.data[0].ValorLimiteInferior;
                                valorMaximoReglaEstadistica = JsonObject.data[0].ValorLimiteSuperior;

                                ////Usado para truncar a los cuatro decimales
                                //var indexValorMinimo = valorMinimoReglaEstadistica.indexOf(",");

                                //indexValorMinimo = indexValorMinimo + 5;

                                ////Usado para truncar a los cuatro decimales
                                //var indexValorMaximo = valorMaximoReglaEstadistica.indexOf(",");

                                //indexValorMaximo = indexValorMaximo + 5;

                                //valorMinimoReglaEstadistica = valorMinimoReglaEstadistica.substring(0, indexValorMinimo);

                                //valorMaximoReglaEstadistica = valorMaximoReglaEstadistica.substring(0, indexValorMaximo);

                                AlgunDetalleNivelSeleccionado = false;
                            }
                           
                       

                            if (JsonObject.data[0].IdNivelDetalle == 1) //Si es cierto se eligió Provincia
                            {
                                for (i = 0 ; i < JsonObject.data.length ; i++) {

                                    ListaReglasProvincia[i] = JsonObject.data[i];
                                }

                                    
                        
                           
                                ocultarTodosValorMinimoYMaximo();
                                AlgunDetalleNivelSeleccionado = true;

                            }

                            if (JsonObject.data[0].IdNivelDetalle == 2) //Si es cierto se eligió Canton
                            {

                                for (i = 0 ; i < JsonObject.data.length ; i++)

                                    ListaReglasCanton[i] = JsonObject.data[i];

                                ocultarTodosValorMinimoYMaximo();


                                AlgunDetalleNivelSeleccionado = true;
                         
                            }

                            if (JsonObject.data[0].IdNivelDetalle == 3) //Si es cierto se eligió Genero
                            {

                                for (i = 0 ; i < JsonObject.data.length ; i++)

                                    ListaReglasGenero[i] = JsonObject.data[i];

                                ocultarTodosValorMinimoYMaximo();


                                AlgunDetalleNivelSeleccionado = true;
                           
                            }


                            reglaEstadisticaDisponible(valorMinimoReglaEstadistica, valorMaximoReglaEstadistica);     
                            

                        } else {                         
                            //La consulta fue correcta pero no hay suficientes (12) datos
                            ReglaAutomaticaDisponible = false;

                            reglaEstadisticaNoDisponible();

                                                
                        }

                    } else {

                        //Algo no salió bien
                  

                   
                    }

                    $(".darkScreen").fadeOut(100, function () {
                        $(this).remove();
                    });

                },
                error: function () {

                    $(".darkScreen").fadeOut(100, function () {
                        $(this).remove();
                    });
                  //  funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
                }
            });

        }
        
}


function reglaEstadisticaNoDisponible() {

  //  $("#contenedorRadioButtonsRegla").addClass('hidden');

    $("#divMsjNoReglaAutomaticaDisponible").removeClass('hidden');

    $("#divMensajeReglasMultiples").addClass('hidden');

    $("#rbManual").prop('checked', true);

    $("#rbEstadistica").prop('checked', false);

    setStateTxtRegla(false);

    debugger;
    var idTipoValorSeleccionado = $('#ddlTipoValor').val();

    
    var valores = obtenerValorTipoValor(idTipoValorSeleccionado);

    var rbManual = document.getElementById("rbManual").checked;

    if (valores[0] != "" && valores[1] != "")
        establecerValoresReglaEstadistica(idTipoValorSeleccionado, "", "");

    mostrarTodosValorMinimoYMaximo();

}

function reglaEstadisticaDisponible(valorInferior, valorSuperior) {

    if (AlgunDetalleNivelSeleccionado)
    $("#divMensajeReglasMultiples").removeClass('hidden');

    $("#contenedorRadioButtonsRegla").removeClass('hidden');

    $("#divMsjNoReglaAutomaticaDisponible").addClass('hidden');

    setStateTxtRegla(true);

    var idTipoValorSeleccionado = $('#ddlTipoValor').val();

    establecerValoresReglaEstadistica(idTipoValorSeleccionado, valorInferior, valorSuperior);


    //Se chequea el RB de regla estadística
    $("#rbEstadistica").prop('checked', true);

}

function setStateTxtRegla(state){
   
    var idTipoValorSeleccionado = $('#ddlTipoValor').val();

    switch (String(idTipoValorSeleccionado)) {
        case '2'://fecha            
            $("#txtReglaInferior2").prop('disabled', state);
            $("#txtReglaSuperior2").prop('disabled', state);
            break;
        case '3'://porcentaje
        case '4'://Monto
        case '5'://Cantidad
        case '7':
            $("#txtReglaInferior3").prop('disabled', state);
            $("#txtReglaSuperior3").prop('disabled', state);
            break;
        case '6':
            $("#txtReglaInferior4").prop('disabled', state);
            $("#txtReglaSuperior4").prop('disabled', state);
            break;
        default:
            $("#txtReglaInferior").prop('disabled', state);
            $("#txtReglaSuperior").prop('disabled', state);
            break;
    }


}


function rdNivelDetalleChange(radioButton) {

    ////se metido el if grande para que respete los valores de regla manual  

  
    if (!document.getElementById("rbManual").checked) {
        //kevv
        var idTipoValorSeleccionado = $('#ddlTipoValor').val();

        if (idTipoValorSeleccionado != 2 && idTipoValorSeleccionado != 1 && idTipoValorSeleccionado != "") // Si no es ni texto, ni fecha, ni seleccione entra
        {
            //Se capturan los parámetros

            var idOperador = $("#IdOperador").val();
            var idDescDetalleAgrupacion = $("#IdDetalleAgrupacion").val();


            var operadorDetalleYTipoValorSeleccionado = false;

            if (idOperador != "") {

                if (idDescDetalleAgrupacion != "") {

                    if (idTipoValorSeleccionado) {

                        operadorDetalleYTipoValorSeleccionado = true; // operador DetalleAgrupacion Y TipoValor han sido Seleccionados

                    } else
                        funcMostrarMensaje("Seleccione un Tipo valor", "Informativo");


                } else
                    funcMostrarMensaje("Seleccione un detalle de agrupación", "Informativo");

            } else
                funcMostrarMensaje("Seleccione un operador", "Informativo");


            if (operadorDetalleYTipoValorSeleccionado) { // Si se tienen los tres parámetros suficientes entonces se calcula la regla



                //Se captura el id del detalle (genero, canton, o provincia



                var idNivelDetalle = radioButton.value;

                var idTipoValor = $('#ddlTipoValor').val();



                $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
                $(".darkScreen").fadeIn(100);
                debugger;
                $.ajax({

                    type: 'POST',
                    url: '_esPosibleAplicarRegla',
                    data: JSON.stringify({ idTipoValor: idTipoValor, idOperador: idOperador, idDescDetalleAgrupacion: idDescDetalleAgrupacion, idNivelDetalle: idNivelDetalle }),
                    contentType: 'application/json; charset=utf-8',
                    cache: false,
                    success: function (data) {
                        debugger;

                        var JsonObject = JSON.parse(data);

                        if (JsonObject.ok == "True" || JsonObject.ok) {

                            if (JsonObject.data != null) {

                                ReglaAutomaticaDisponible = true;

                                if (JsonObject.data[0].IdNivelDetalle == 1) //Si es cierto se eligió Provincia
                                {
                                    for (i = 0 ; i < JsonObject.data.length ; i++)

                                        ListaReglasProvincia[i] = JsonObject.data[i];

                                    ocultarTodosValorMinimoYMaximo();

                                    $("#divMensajeReglasMultiples").removeClass('hidden');
                                    AlgunDetalleNivelSeleccionado = true;

                                }

                                if (JsonObject.data[0].IdNivelDetalle == 2) //Si es cierto se eligió Canton
                                {


                                    for (i = 0 ; i < JsonObject.data.length ; i++)

                                        ListaReglasCanton[i] = JsonObject.data[i];

                                    ocultarTodosValorMinimoYMaximo();

                                    $("#divMensajeReglasMultiples").removeClass('hidden');
                                    AlgunDetalleNivelSeleccionado = true;
                                }

                                if (JsonObject.data[0].IdNivelDetalle == 3) //Si es cierto se eligió Genero
                                {

                                    for (i = 0 ; i < JsonObject.data.length ; i++)

                                        ListaReglasGenero[i] = JsonObject.data[i];

                                    ocultarTodosValorMinimoYMaximo();

                                    $("#divMensajeReglasMultiples").removeClass('hidden');
                                    AlgunDetalleNivelSeleccionado = true;
                                }


                                reglaEstadisticaDisponible(valorMinimoReglaEstadistica, valorMaximoReglaEstadistica);


                            } else {

                                
                                //La consulta fue correcta pero no hay suficientes (12) datos
                                ReglaAutomaticaDisponible = false

                                reglaEstadisticaNoDisponible();
                               
                            }

                        } else {

                            //Algo no salió bien


                        }

                        $(".darkScreen").fadeOut(100, function () {
                            $(this).remove();
                        });
                    },
                    error: function () {
                        //  funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
                        $(".darkScreen").fadeOut(100, function () {
                            $(this).remove();
                        });

                    }//fin del error

                });//fin del Ajax


            } // fin parametrosSuficiente TRUE



             }// fin del else 
        }
}

function ocultarTodosValorMinimoYMaximo() {

    $("#divReglaValorLimiteInferior").addClass('hidden');
    $("#divReglaValorLimiteSuperior").addClass('hidden'); 

}

function mostrarTodosValorMinimoYMaximo() {

    $("#divReglaValorLimiteInferior").removeClass('hidden');
    $("#divReglaValorLimiteSuperior").removeClass('hidden');

}

//**************************Cambios en controles*****************************

function cambioTipoValor(idTipoValor) {
   

    debugger;

    var detalleAgrupacion = $("#descDetalleAgrupacion").val();
    var nombreOperadordetalleValue = $("#nombreOperador").val();
    var operadorYDetalleSeeccionado = false;

   

    if (nombreOperadordetalleValue != "") {

        if (detalleAgrupacion != "") {            

            $("#txtReglaInferior").addClass('hidden');
            $("#txtReglaInferior2").addClass('hidden');
            $("#txtReglaInferior3").addClass('hidden');
            $("#txtReglaInferior4").addClass('hidden');

            $("#txtReglaSuperior").addClass('hidden');
            $("#txtReglaSuperior2").addClass('hidden');
            $("#txtReglaSuperior3").addClass('hidden');
            $("#txtReglaSuperior4").addClass('hidden');

            switch (String(idTipoValor)) {

                // Texto
                case '1':
                    $("#divReglaValorLimiteInferior").addClass('hidden');
                    $("#divReglaValorLimiteSuperior").addClass('hidden');
                    break;

                    // %
                case '2':
                    $("#divReglaValorLimiteInferior").removeClass('hidden');
                    $("#divReglaValorLimiteSuperior").removeClass('hidden');
                    $("#txtReglaInferior2").removeClass('hidden');
                    $("#txtReglaSuperior2").removeClass('hidden');
                    break;
                    // Monto
                case '3':
                    // Minutos
                case '4':
                    //Fecha
                case '5':
                    //Cant con decimales
                case '7':
                    $("#divReglaValorLimiteInferior").removeClass('hidden');
                    $("#divReglaValorLimiteSuperior").removeClass('hidden');
                    $("#txtReglaInferior3").removeClass('hidden');
                    $("#txtReglaSuperior3").removeClass('hidden');
                    break;
                    //Cant sin decimales
                case '6':
                    $("#divReglaValorLimiteInferior").removeClass('hidden');
                    $("#divReglaValorLimiteSuperior").removeClass('hidden');
                    $("#txtReglaInferior4").removeClass('hidden');
                    $("#txtReglaSuperior4").removeClass('hidden');
                    break;
                default:

                    $("#txtReglaInferior").removeClass('hidden');
                    $("#txtReglaSuperior").removeClass('hidden');
                    break;
            }

           
            if (!Editando)
            DeterminarSiEsPosibleAplicarReglaEstadística(idTipoValor);

        } else {

            funcMostrarMensaje("Seleccione un detalle de agrupación", "Informativo");
            $("#ddlTipoValor").val("");
        }
         

    } else {

        funcMostrarMensaje("Seleccione un operador", "Informativo");
        $("#ddlTipoValor").val("");
      
    }

    
}

function agregarOperador(idOperador, nombreOperador) {

    $("#IdOperador").val(idOperador);
    $("#nombreOperador").val(nombreOperador);
}

//*************************Metodos de limpieza**************************
function limpiarIndicador() {
    $("#nombreIndicadorCrear").val('');
    $("#IdIndicadorCrear").val('');
}

function limpiarCriterio() {
    $("#nombreCriterio").val('');
    $("#IdCriterio").val('');
    $("#txtAyuda").val('');
}

function limpiarCriterioNoSeleccionados() {
    var idDireccionSeleccionado = $("#IdDireccion").val();
    var idIndicadorSeleccionado = $("#IdIndicadorCrear").val();
    $.ajax({

        type: 'POST',
        url: 'gMostrarCriterioNoSeleccionados',
        data: JSON.stringify({ idDireccion: idDireccionSeleccionado, idIndicador: idIndicadorSeleccionado,codigoCriterio:'',nombreCriterio:'',ayuda:'' }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='tablaCriterio'>" + data + "</div>";

            var $form = $(data);
            $("#tablaCriterio").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }


        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

function limpiarDetallesAgrupacionAsociados() {
    $('#jstreeDetalleAgrupacion').jstree("destroy").empty();
}

function limpiarDetallesAgrupacionAgregar() {


    fncCambiarTextoControl('divPnDetalle', 'Agregar Detalle Agrupación');
    $("#descDetalleAgrupacion").val('');
    $("#IdAgrupacion").val('');
    $("#descAgrupacion").val('');
    $("#IdDetalleAgrupacion").val('');
    $("#idDetalleOperadorAgrupacion").val('');
    $('#ckUltimoNivel').prop('checked', false);
    $('#divRegla').addClass('hidden');
    $('#divReglaValorLimiteInferior').addClass('hidden');
    $('#divReglaValorLimiteSuperior').addClass('hidden');
    $('#slcDetalleAgrupacionPadre').empty();
    $("#ddlTipoValor option:eq(0)").attr("selected", "selected");
    $("#txtReglaInferior").val('');
    $("#txtReglaInferior2").val('');
    $("#txtReglaInferior3").val('');
    $("#txtReglaInferior4").val('');
    $("#txtReglaSuperior").val('');
    $("#txtReglaSuperior1").val('');
    $("#txtReglaSuperior2").val('');
    $("#txtReglaSuperior3").val('');
    $("#txtReglaSuperior4").val('');
    $('input[name=rdNivelDetalle]:checked').prop('checked', false);

}
function limpiarDetallesAgrupacionRadios() {


    //fncCambiarTextoControl('divPnDetalle', 'Agregar Detalle Agrupación');
    //$("#descDetalleAgrupacion").val('');
    //$("#IdAgrupacion").val('');
    //$("#descAgrupacion").val('');
    //$("#IdDetalleAgrupacion").val('');
    //$("#idDetalleOperadorAgrupacion").val('');
    //$('#ckUltimoNivel').prop('checked', false);
    //$('#divRegla').addClass('hidden');
    //$('#divReglaValorLimiteInferior').addClass('hidden');
    //$('#divReglaValorLimiteSuperior').addClass('hidden');
    //$('#slcDetalleAgrupacionPadre').empty();
    //$("#ddlTipoValor option:eq(0)").attr("selected", "selected");
    //$("#txtReglaInferior").val('');
    //$("#txtReglaInferior2").val('');
    //$("#txtReglaInferior3").val('');
    //$("#txtReglaInferior4").val('');
    //$("#txtReglaSuperior").val('');
    //$("#txtReglaSuperior1").val('');
    //$("#txtReglaSuperior2").val('');
    //$("#txtReglaSuperior3").val('');
    //$("#txtReglaSuperior4").val('');
    $('input[name=rdNivelDetalle]:checked').prop('checked', false);

}

function fncLimpiarradios() {
    $('input[name=rdNivelDetalle]:checked').prop('checked', false);
}
function limpiarDetalleAgrupacionOperador() {

    $.ajax({

        type: 'POST',
        url: '_limpiarDetalleAgrupacionConstructor',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

            var $form = $(data);
            $("#divDetalleAgrupacionOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }


        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

function limpiarCriterios() {

    $.ajax({

        type: 'POST',
        url: '_limpiarCriterio',

        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divCriteriosSeleccionados'>" + data + "</div>";

            var $form = $(data);
            $("#divCriteriosSeleccionados").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }

        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

function limpiarOperadores() {

    $.ajax({
        type: "POST",
        url: '_tablaOperador',
        contentType: "application/json; charset=utf-8",
       
        cache: false,
        success: function (data) {
            data = "<div id='tablaOperador'>" + data + "</div>";

            var $form = $(data);
            $("#tablaOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }
            
        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

function limpiarOperadoresCriterio() {

    $.ajax({
        type: "POST",
        url: '_tablaOperadorCriterio',
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        cache: false,
        success: function (data) {
            data = "<div id='tablaOperador'>" + data + "</div>";

            var $form = $(data);
            $("#tablaOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }
        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

function limpiarOperadoresClonar() {

    $.ajax({
        type: "POST",
        url: '_tablaOperadorClonar',
        contentType: "application/json; charset=utf-8",

        cache: false,
        success: function (data) {
            data = "<div id='tablaOperadorClonar'>" + data + "</div>";

            var $form = $(data);
            $("#tablaOperadorClonar").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }

        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}
function FunctionReload(){
    window.location.reload();
}
//++++++++++++++++++++++++++++++Mensaje+++++++++++++++++++++++++++++++++++++//
function funcMostrarMensaje(mensaje, titulo) {
    debugger;
    $('#divMensaje').modal({ backdrop: 'static', keyboard: false })
    $("#tituloMensaje").html(titulo);
    $("#contenidoMensaje").html(mensaje);
    $('#divMensaje').modal('show');
}
//**************************** validar **************************
function validarRegla() {

    var idTipoValor = $('#ddlTipoValor').val();
    var valores = obtenerValorTipoValor(idTipoValor);
    var strValorInferior = valores[0];
    var strValorSuperior = valores[1];

    if (idTipoValor == '' || idTipoValor < 0) {
        funcMostrarMensaje("Debe seleccionar un tipo de valor", "Informativo");
        return 'false';
    } else
        if (idTipoValor > 1 && strValorInferior == '') {
            funcMostrarMensaje("Debe seleccionar un valor Inferior", "Informativo");
            return 'false';
        }
        else
            if (idTipoValor > 1 && strValorSuperior == '') {
                funcMostrarMensaje("Debe seleccionar un valor Superior", "Informativo");
                return 'false';
            }

    if (idTipoValor > 1) {
       if (idTipoValor == 4 || idTipoValor == 5 || idTipoValor == 3) {
           if (parseFloat(strValorInferior) > parseFloat(strValorSuperior)) {
                funcMostrarMensaje("El valor inferior debe ser menor que el valor superior.", "Error");
                return 'false';
            }

        }
    }
    return 'true';
}

function validarDetallesAgrupacionPorOperador() {
    var v = document.getElementById("jstreeDetalleAgrupacion").getElementsByTagName('li').length;
  
    if (v == 1) {
        funcMostrarMensaje("Debe agregar detalles agrupación", "Informativo");
        return 'false';
    }
    return 'true';
}

function validarCriterio() {
    var idcriterio = $('#IdCriterio').val();
    var ayuda = $('#txtAyuda').val();

    var oTable = $('#dataTable_0').dataTable();
    if (idcriterio == null || idcriterio == '') {
        funcMostrarMensaje("Debe seleccionar un criterio", "Informativo");
        return 'false';
    }
    if (ayuda == '') {
        funcMostrarMensaje("Debe digitar la ayuda correspondiente al criterio", "Informativo");
        return 'false';
    }
    if (oTable.fnGetData().length < 1) {
        funcMostrarMensaje("Debe agregar al menos un detalle de agrupación al criterio", "Informativo");
        return 'false';
    }
    return 'true';
}

//**************************** filtrar *******************************************

function filtrarDetallesAgrupacionOperador(nombreOperador) {

    $.ajax({

        type: 'POST',
        url: '_tablaDetalleAgrupacionOperador',
        data: JSON.stringify({ IdOperador: '', NombreOperador: nombreOperador }),
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

            var $form = $(data);
            $("#divDetalleAgrupacionOperador").replaceWith(data);

            if (typeof initDataGrid !== 'undefined') {
                $('table[data-table-grid="true"]').ready(initDataGrid);
            }


        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });

}
//filtro de detalle agrupación
function filtrarDetalleAgrupacion(event) {

    if (event.keyCode === 13) {
        var filtroIdOperador = $('#IdOperador').val();
        var filtroNombreAgrupacion = $('#txtFiltroAgrupacion').val();
        var filtroNombreDetalleAgrupacion = $('#txtFiltroDetalleAgrupacion').val();
      
        $.ajax({
            type: "POST",
            url: '_filtrarDetalleAgrupacion',
            data: JSON.stringify({ idOperador: filtroIdOperador, nombreAgrupacion: filtroNombreAgrupacion, nombreDetalleAgrupacion: filtroNombreDetalleAgrupacion }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaDetalleAgrupacion'>" + data + "</div>";

                var $form = $(data);
                $("#tablaDetalleAgrupacion").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroAgrupacion').val(filtroNombreAgrupacion);
                $('#txtFiltroDetalleAgrupacion').val(filtroNombreDetalleAgrupacion);
               
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

//Filtrar indicador
function filtrarIndicador(event) {

    if (event.keyCode === 13) {
        var filtroIdDireccion = $('#ddlDireccion').val();
        var filtroCodigoIndicador = $('#txtFiltroCodigoIndicador').val();
        var filtroNombreTipoIndicador = $('#txtFiltroTipoIndicador').val();
        var filtroNombreIndicador = $('#txtFiltroIndicador').val();
        $.ajax({
            type: "POST",
            url: '_filtroIndicador',
            data: JSON.stringify({ idDireccion: filtroIdDireccion, codigoIndicador: filtroCodigoIndicador, nombreTipoIndicador: filtroNombreTipoIndicador, nombreIndicador: filtroNombreIndicador }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaIndicador'>" + data + "</div>";

                var $form = $(data);
                $("#tablaIndicador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCodigoIndicador').val(filtroCodigoIndicador);
                $('#txtFiltroTipoIndicador').val(filtroNombreTipoIndicador);
                $('#txtFiltroIndicador').val(filtroNombreIndicador);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

//Filtra los detalles Operador///

function filtrarDetalleOperador(event) {
   
    if (event.keyCode === 13) {
        
        var filtroOperador = $('#txtFiltroDetalleOperador').val();
        
        $.ajax({
            type: "POST",
            url: '_filtroDetalleAgrupacionOperador',
            data: JSON.stringify({ IdOperador: '', NombreOperador: filtroOperador }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

                var $form = $(data);
                $("#divDetalleAgrupacionOperador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroDetalleOperador').val(filtroOperador);
               
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}
//
//newM
//
function filtrarDetalleOperadorEditar(event) {

    if (event.keyCode === 13) {

        var filtroOperador = $('#txtFiltroDetalleOperador').val();
        var IDConstructor = $('#idConstructor').val();
        var IDCriterio = $('#idCriterio').val();
        $.ajax({
            type: "POST",
            url: '_filtroDetalleAgrupacionOperadorEditar',
            data: JSON.stringify({ NombreOperador: filtroOperador , idConstructor:IDConstructor , idCriterio:IDCriterio }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='divDetalleAgrupacionOperador'>" + data + "</div>";

                var $form = $(data);
                $("#divDetalleAgrupacionOperador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroDetalleOperador').val(filtroOperador);

            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarOperadorClonar(event) {

    if (event.keyCode === 13) {

        var filtroIdOperador = $('#txtFiltroCodigoOperadorClonado').val();
        var filtroNombreOperador = $('#txtFiltroNombreOperadorClonado').val();
        $.ajax({
            type: "POST",
            url: '_filtarTablaOperadorClonar',
            data: JSON.stringify({ IdOperador: filtroIdOperador, NombreOperador: filtroNombreOperador }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaOperadorClonar'>" + data + "</div>";

                var $form = $(data);
                $("#tablaOperadorClonar").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCodigoOperadorClonado').val(filtroIdOperador);
                $('#txtFiltroNombreOperadorClonado').val(filtroNombreOperador);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarOperadorClonarEditar(event) {

    if (event.keyCode === 13) {

        var filtroIdOperador = $('#txtFiltroCodigoOperadorClonado').val();
        var filtroNombreOperador = $('#txtFiltroNombreOperadorClonado').val();
        var IDConstructor = $('#idConstructor').val();
        var IDCriterio = $('#idCriterio').val();

        $.ajax({
            type: "POST",
            url: '_filtarTablaOperadorClonarEditar',
            data: JSON.stringify({ idOperador: filtroIdOperador, NombreOperador: filtroNombreOperador, idConstructor: IDConstructor, idCriterio: IDCriterio }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaOperadorClonarEditar'>" + data + "</div>";

                var $form = $(data);
                $("#tablaOperadorClonarEditar").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCodigoOperadorClonado').val(filtroIdOperador);
                $('#txtFiltroNombreOperadorClonado').val(filtroNombreOperador);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarCriterioSeleccionado(event) {
    debugger;
    if (event.keyCode === 13) {

        var filtroCodigoCriterio = $('#txtFiltroCodigoCriterio').val();
        var filtroCriterio = $('#txtFiltroCriterio').val();
        var filtroAyuda = $('#txtFiltroAyuda').val();
        $.ajax({
            type: "POST",
            url: '_filtrarCriterioConstructor',
            data: JSON.stringify({ codigoCriterio: filtroCodigoCriterio, nombreCriterio: filtroCriterio, ayuda: filtroAyuda }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='divCriteriosSeleccionados'>" + data + "</div>";

                var $form = $(data);
                $("#divCriteriosSeleccionados").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCodigoCriterio').val(filtroCodigoCriterio);
                $('#txtFiltroCriterio').val(filtroCriterio);
                $('#txtFiltroAyuda').val(filtroAyuda);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarCriterioSeleccionadoEditar(event) {
    debugger;
    if (event.keyCode === 13) {
        var idConstructor = $('#idConstructor').val();
        var filtroCodigoCriterio = $('#txtFiltroCodigoCriterio').val();
        var filtroCriterio = $('#txtFiltroCriterio').val();
        var filtroAyuda = $('#txtFiltroAyuda').val();
        var idconstructor = $('#idConstructor').val();
        $.ajax({
            type: "POST",
            url: '_filtrarCriterioConstructorEditar',
            data: JSON.stringify({ codigoCriterio: filtroCodigoCriterio, nombreCriterio: filtroCriterio, ayuda: filtroAyuda , idConstructor:idconstructor}),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='divCriteriosSeleccionados'>" + data + "</div>";

                var $form = $(data);
                $("#divCriteriosSeleccionados").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCodigoCriterio').val(filtroCodigoCriterio);
                $('#txtFiltroCriterio').val(filtroCriterio);
                $('#txtFiltroAyuda').val(filtroAyuda);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarCriterio(event) {

    if (event.keyCode === 13) {
        var filtroIdDireccion = $('#IdDireccion').val();
        var filtroCodigoCriterio = $('#txtFiltroCriterioCodigo').val();
        var filtroNombreCriterio = $('#txtFiltroCriterioNombre').val();
        var filtroIdIndicador = $('#IdIndicadorCrear').val();
        $.ajax({
            type: "POST",
            url: '_filtrarCriterio',
            data: JSON.stringify({ idDireccion: filtroIdDireccion, codigoCriterio: filtroCodigoCriterio, nombreCriterio: filtroNombreCriterio, ayuda: '', idIndicador: filtroIdIndicador }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaCriterio'>" + data + "</div>";

                var $form = $(data);
                $("#tablaCriterio").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCriterioCodigo').val(filtroCodigoCriterio);
                $('#txtFiltroCriterio').val(filtroNombreCriterio);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarCriterioEditar(event) {

    if (event.keyCode === 13) {
        var filtroIdDireccion = $('#IdDireccion').val();
        var filtroCodigoCriterio = $('#txtFiltroCriterioCodigo').val();
        var filtroNombreCriterio = $('#txtFiltroCriterioNombre').val();
        var filtroIdIndicador = $('#IdIndicadorCrear').val();
        var idconstructor = $('#idConstructor').val();
        $.ajax({
            type: "POST",
            url: '_filtrarCriterioEditar',
            data: JSON.stringify({ idDireccion: filtroIdDireccion, codigoCriterio: filtroCodigoCriterio, nombreCriterio: filtroNombreCriterio, ayuda: '', idIndicador: filtroIdIndicador, idConstructor:idconstructor }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaCriterio'>" + data + "</div>";

                var $form = $(data);
                $("#tablaCriterio").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCriterioCodigo').val(filtroCodigoCriterio);
                $('#txtFiltroCriterio').val(filtroNombreCriterio);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarOperador(event) {

    if (event.keyCode === 13) {

        var filtroCodigoOperador = $('#txtFiltroCodigoOperador').val();
        var filtroNombreOperador = $('#txtFiltroNombreOperador').val();
       
        $.ajax({
            type: "POST",
            url: '_filtroOperador',
            data: JSON.stringify({ IdOperador: filtroCodigoOperador, NombreOperador: filtroNombreOperador }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaOperador'>" + data + "</div>";

                var $form = $(data);
                $("#tablaOperador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCodigoCriterio').val(filtroCodigoOperador);
                $('#txtFiltroCriterio').val(filtroNombreOperador);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function filtrarOperadorEditar(event) {

    if (event.keyCode === 13) {

        var filtroCodigoOperador = $('#txtFiltroCodigoOperador').val();
        var filtroNombreOperador = $('#txtFiltroNombreOperador').val();
        var IdConstructor = $('#idConstructor').val();
        var IdCriterio = $('#idCriterio').val();

        $.ajax({
            type: "POST",
            url: '_filtroOperadorEditar',
            data: JSON.stringify({ idOperador: filtroCodigoOperador, NombreOperador: filtroNombreOperador, idConstructor: IdConstructor, idCriterio: IdCriterio }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {

                data = "<div id='tablaOperador'>" + data + "</div>";

                var $form = $(data);
                $("#tablaOperador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }

                $('#txtFiltroCodigoCriterio').val(filtroCodigoOperador);
                $('#txtFiltroCriterio').val(filtroNombreOperador);
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }
}

function functSeleccionarOperadorIDEditar(id) {
    debugger;
    var element = $("#CKOId_" + id);
    var valueChecked = element.prop('checked');

    var inList = false;
    var indexArray = null;
    for (var i = 0; i < operadoresClonar.length; i++) {
        if (operadoresClonar[i] == id) {
            inList = true;
            indexArray = i;
            break;
        }
    }
    if (!inList && valueChecked) {
      
        if (operadoresClonar.length < 10) {
            operadoresClonar.push(id);
        } else {
            $("#divMensajeWarningOperadorClonar").show();
            $("#idMensajeWarningCuerpoOperadorClonar").html("El número máximo de operadores a clonar es de 10, por favor clone los seleccionados \n y vuelva seleccionar los operadores deseados.");
            element.prop("checked", false);
        }
       
    }
    else {
        if (inList && !valueChecked) {
            operadoresClonar.splice(indexArray, 1);
        }
    }
};

function functSeleccionarOperadorID(id) {

    var element = $("#CKOId_" + id);
    var valueChecked = element.prop('checked');

    var inList = false;
    var indexArray = null;
    for (var i = 0; i < operadoresClonar.length; i++) {
        if (operadoresClonar[i] == id) {
            inList = true;
            indexArray = i;
            break;
        }
    }
    if (!inList && valueChecked) {
        operadoresClonar.push(id);
    }
    else {
        if (inList && !valueChecked) {
            operadoresClonar.splice(indexArray, 1);
        }
    }
};

function gLlamadaControles(actionUrl, id) {
    //debugger;
    var url = document.URL.split("/");
    if (id == '') {
        var allurl = url[0] + "//" + url[2] + "/" + actionUrl;
    } else {
    var allurl = url[0] + "//" + url[2] + "/" + actionUrl + "?id=" + id.trim();}
    window.location.href = allurl;
}
///
///NewM
///
function gLlamadaControllerOperador(actionUrl, id, idCriterio) {
    debugger;
    var url = document.URL.split("/");
    if (id == '') {
        var allurl = url[0] + "//" + url[2] + "/";
    } else {
        if (url.length == 5) {
            var allurl = url[0] + "//" + url[2] + "//" + url[3] + actionUrl + "?id=" + id + "&idcriterio=" + idCriterio;
        }
        if (url.length == 6) {
            var allurl = url[0] + "//" + url[2] + "//" + url[4] + actionUrl + "?id=" + id + "&idcriterio=" + idCriterio;
        }

    }
    window.location.href = allurl;
}
///
///NewM
///
function gLlamadaControllerArbol(actionUrl, id, idCriterio, idOperador) {
    debugger;
    var url = document.URL.split("/");
    if (id == '') {
        var allurl = url[0] + "//" + url[2] + "/";
    } else {
        if (url.length == 5) {
            var allurl = url[0] + "//" + url[2] + "//" + url[3] + actionUrl + "?id=" + id + "&idcriterio=" + idCriterio + "&idoperador=" + idOperador;
        }
        if (url.length == 6) {
            var allurl = url[0] + "//" + url[2] + "//" + url[4] + actionUrl + "?id=" + id + "&idcriterio=" + idCriterio + "&idoperador=" + idOperador;
        }
    }
    window.location.href = allurl;
}

function ValidarCaracteres(textareaControl, maxlength) {
    debugger;
    if (textareaControl.value.indexOf("\"") != -1 || textareaControl.value.indexOf("'") != -1) {
      
        var txt= textareaControl.value.replace(/["']/g, "");      
        $("#txtAyuda").val(txt);

        funcMostrarMensaje("Los caracteres comillas dobles y comillas simples no están permitidos.", "Informativo");
    }

    if (textareaControl.value.length > maxlength) {
        textareaControl.value = textareaControl.value.substring(0, maxlength);
    }
}

function validarCamposAlEditarDetalleAgrupacion() {

    var selectTipoValor = $("#ddlTipoValor").val();

    if (selectTipoValor != "") {

        var is_valid = true;
        var msj = "Debe completar el valor inferior y el superior";


        var reglaEstadistica = document.getElementById("rbEstadistica").checked;

        if (!reglaEstadistica && !ReglaAutomaticaDisponible) {
            //aqui preguntar
            switch (selectTipoValor) {
                //Fecha
                case "2":
                    if ($("#txtReglaInferior2").val() == "" || $("#txtReglaSuperior2").val() == "") {
                        is_valid = false;
                    }
                    break;

                    //Porcentaje
                case "3":
                    //Monto
                case "4":
                    //Cantidad sin decimales
                case "5":
                    //Minutos
                case "7":
                    if ($("#txtReglaInferior3").val() == "" || $("#txtReglaSuperior3").val() == "") {
                        is_valid = false;
                    }
                    else {
                        if (parseInt($("#txtReglaInferior3").val()) > parseInt($("#txtReglaSuperior3").val())) {
                            msj = "El valor inferior debe ser menor al valor superior";
                            is_valid = false;
                        }
                    }
                    break;

                    //Cantidad con decimales
                case "6":
                    if ($("#txtReglaInferior4").val() == "" || $("#txtReglaSuperior4").val() == "") {
                        is_valid = false;
                    } else {
                        if (parseFloat($("#txtReglaInferior4").val()) > parseFloat($("#txtReglaSuperior4").val())) {
                            msj = "El valor inferior debe ser menor al valor superior";
                            is_valid = false;
                        }
                    }
                    break;

            } // fin del switch

        }// fin del if

        if (!is_valid) {
            funcMostrarMensaje(msj, "Informativo");
            return is_valid;
        }
        return is_valid;
    }
    else {
        funcMostrarMensaje("Debe seleccionar un tipo de valor.", "Informativo");
        return false;
    }
}
//-------------mensajes----------------------------//
function funcMostrarMensajeIArbol() {
    funcMostrarMensaje("Para ingresar a esta Pestaña, debe ingresar a la Pestaña <strong>Criterio</strong> agregar \n o seleccionar un criterio, luego agregar o seleccionar un detalle de Agrupación por Operador.", "Informativo");
}
function funcMostrarMensajeIOperadores() {
    funcMostrarMensaje("Para ingresar a esta Pestaña, debe ingresar a la Pestaña <strong>Criterio</strong> agregar o seleccionar un criterio.", "Informativo");
}

function funcMostrarMensajeCArbol() {
    funcMostrarMensaje("Para ingresar a esta Pestaña, debe  agregar criterio o seleccionar un criterio, luego agregar o seleccionar un detalle de Agrupación por Operador.", "Informativo");
}
function funcMostrarMensajeCOperadores() {
    funcMostrarMensaje("Para ingresar a esta Pestaña, debe  agregar criterio o seleccionar un criterio.", "Informativo");
}
function funcMostrarMensajeArbol() {
    funcMostrarMensaje("Para ingresar a esta Pestaña, debe agregar o seleccionar un detalle de Agrupación por Operador.", "Informativo");
}