
$(document).ready(function () {
    $('#ddlServicio').change(function () {
        lLimpiarIndicador();
        $("#ddlDireccion option:eq(0)").attr("selected", "selected");
        functionCargarIndicador();
        funcionCargarCriterios(0);
        
        $('#jstreeDetalleAgrupacion').jstree("destroy").empty();
        
    });
    $('#ddlDireccion').change(function () {
        lLimpiarIndicador();
        functionCargarIndicador();
        funcionCargarCriterios(0);
        $('#jstreeDetalleAgrupacion').jstree("destroy").empty();
    });

    $('#ddlNivelDetalle').change(function () {
        funcCargarValorConCombo(this.value);
    });
    
});





$(document).ready(function () {

    $(function () {
      

        $("[id$=txtFechaIncial]").datepicker({
            dateFormat: 'dd/mm/yy',
            defaultDate: new Date(),
            beforeShow: function (textbox, instance) {
                instance.dpDiv.css({
                    marginTop: (-textbox.offsetHeight) + 'px',
                    marginLeft: textbox.offsetWidth + 'px'
                });
            },
            onClose: function (selectedDate) {
                $("[id$=txtFechaFinal]").datepicker("option", "minDate", selectedDate);
            }
        });
    });
    $(function () {
        $("[id$=txtFechaFinal]").datepicker({
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
        $("[id$=txtFechaIncial]").mask("99/99/9999");
        $("[id$=txtFechaFinal]").mask("99/99/9999");


    });

    $(function () {
        $("[id$=txtValorNuevo1]").datepicker({
            dateFormat: 'dd/mm/yy',
            beforeShow: function (textbox, instance) {
                instance.dpDiv.css({
                    marginTop: (-textbox.offsetHeight) + 'px',
                    marginLeft: textbox.offsetWidth + 'px'
                });
            }
        });
    });
});

function checkSupportForInputTypeDate() {
    jQuery.validator.methods.date = function (value, element) {
        if (value) {
            try {
                value = value.replace(/\//g, "-");
                
                var date = $.datepicker.parseDate('dd-mm-yy', value);
                
            } catch (ex) {
                $(element).val('');

                return false;
            }
        }
        return true;
    };
}
$(document).ready(function () {
    checkSupportForInputTypeDate();
});


//#####################################################
//Definición de métodos
//#####################################################

//carga los servicios del operador
function functionCargarServicios(idOperador) {
    $("#ddlDireccion option:eq(0)").attr("selected", "selected");
    functionCargarIndicador();
    funcionCargarCriterios('0');
    $('#jstreeDetalleAgrupacion').jstree("destroy").empty();
    $.ajax({
        type: "POST",
        url: 'ModificionRegistroIndicadorInterno/_listarServicios',
        data: JSON.stringify({ IdOperador: idOperador, NombreOperador: '' }),
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            var dataServicio = jsonData.data;

            $("#ddlServicio").html("");
            $("#ddlServicio").append("<option value=''>Seleccione</option>");
            $.each(dataServicio, function (k, v) {
                $("#ddlServicio").append("<option value=\"" + v.IdServicio + "\">" + v.DesServicio + "</option>");
            });
        },
        error: function () {

            $("[name='ddlServicio']").eq(0).html();
        }
    });
}

//#####################################################
// carga  los indicadores deacuerdo a la dirección y al servicio
function functionCargarIndicador() {
    var idDireccion = $('#ddlDireccion').val();
    var idServicio = $('#ddlServicio').val();
    $.ajax({
        type: "POST",
        url: 'ModificionRegistroIndicadorInterno/_tablaIndicador',
        data: JSON.stringify({ IdDireccion: idDireccion, IdServicio: idServicio }),
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
           // funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}
//#####################################################
//carga los criterios del indicador
function funcionCargarCriterios(idIndicador) {
    $('#jstreeDetalleAgrupacion').jstree("destroy").empty();
    var idDireccion = $('#ddlDireccion').val();
    $.ajax({
        type: "POST",
        url: 'ModificionRegistroIndicadorInterno/_listarCriterio',
        data: JSON.stringify({ IdDireccion: idDireccion, IdIndicador: idIndicador }),
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            var dataCriterios = jsonData.data;

            $("#ddlCriterio").html("");
            $("#ddlCriterio").append("<option value=''>Seleccione</option>");
            $.each(dataCriterios, function (k, v) {
                $("#ddlCriterio").append("<option value=\"" + v.IdCriterio + "\">" + v.DescCriterio + "</option>");
            });
        },
        error: function () {

            $("[name='ddlCriterio']").eq(0).html();
        }
    });
}
//#####################################################
//carga las solicitudes que concuerden y valida
function functSolicitud() {
    
    if (!$("#formSolicitud").valid()) {
            console.log("no es valida");
            return false;
    }

    $("#formSolicitud").submit();
    this.funcionCargarSolicitudes();
   
}
//Carga las solicitudes
function funcionCargarSolicitudes() {
    $("#divMensajeSolicitudMensaje").hide();
    var valorIdOperador = $('#IdOperador').val();
    var valorIdServicio = $('#ddlServicio').val();
    var valorIdDireccion = $('#ddlDireccion').val();
    var valorIdIndicador = $('#IdIndicador').val();
    var valorIdCriterio = $('#ddlCriterio').val();
    var valorIdFrecuencia = $('#ddlFrecuencia').val();
    var valorIdDesglose = $('#ddlDesglose').val();
    var valorIdFechaInicial = $('#txtFechaIncial').val();
    var valorIdFechaFinal = $('#txtFechaFinal').val();
    lLimpiarValor();
    $('#jstreeDetalleAgrupacion').jstree("destroy").empty();
    if (valorIdOperador == '' || valorIdOperador < 0) {
        funcMostrarMensaje("Debe seleccionar un operador.", "Error");
        return  false;
    }
    if (valorIdServicio == '' || valorIdServicio < 0) {
        $("#spanErrorIdServicio").show();
        return false;
    }
    else {
        $("#spanErrorIdServicio").hide();
    }
    if (valorIdCriterio == '' || valorIdCriterio < 0) {
        $("#spanErrorIdCriterio").show();
        return false;
    }
    else {
        $("#spanErrorIdCriterio").hide();
    }

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);

    $.ajax({
        type: "POST",
        url: 'ModificionRegistroIndicadorInterno/BuscarSolicitud',
        data: JSON.stringify({ idOperador: valorIdOperador, idServicio: valorIdServicio, idDireccion: valorIdDireccion, idIndicador: valorIdIndicador, idCriterio: valorIdCriterio, idFrecuencia: valorIdFrecuencia, idDesglose: valorIdDesglose, fechaInicial: valorIdFechaInicial, fechaFinal: valorIdFechaFinal }),
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        cache: false,
        success: function (data) {
           
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
           
                data = "<div id='divSolicitudes'>" + data + "</div>";

                var $form = $(data);
                $("#divSolicitudes").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }
                functMensajeBuscarSolicitudes();
        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}
 //Carga los detalles agrupación segun la solicitud
function functCargarDetalleAgrupacion(valorIdSolicitudIndicador) {
    var json = [];
    var valorIdOperador = $('#IdOperador').val();
    var valorIdServicio = $('#ddlServicio').val();
    var valorIdDireccion = $('#ddlDireccion').val();
    var valorIdIndicador = $('#IdIndicador').val();
    var valorIdCriterio = $('#ddlCriterio').val();
    var valorIdFrecuencia = $('#ddlFrecuencia').val();
    var valorIdDesglose = $('#ddlDesglose').val();
    lLimpiarValor();
    $.ajax({
        type: "POST",
        url: 'ModificionRegistroIndicadorInterno/gMostrarDetalleAgrupacion',
        data: JSON.stringify({ idOperador: valorIdOperador, idServicio: valorIdServicio, idDireccion: valorIdDireccion, idIndicador: valorIdIndicador, idCriterio: valorIdCriterio, idFrecuencia: valorIdFrecuencia, idDesglose: valorIdDesglose, idSolicitudIndicador: valorIdSolicitudIndicador }),
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            var dataMeses = jsonData.data.meses;
            var dataDetalles = jsonData.data.arbol;
            $("#ddlMes").html("");
            $("#ddlMes").append("<option value=''>Seleccione</option>");
            $.each(dataMeses, function (k, v) {
                $("#ddlMes").append("<option value=\"" + v.idMes + "\">" + v.nombreMes + " - " + v.anno + "</option>");
            });

           
            for (var i = 0; i < dataDetalles.length; i++) {

                var text = dataDetalles[i].text;
                var id = dataDetalles[i].id;
                var parent = dataDetalles[i].parent;
                var ckUltimoNivel = dataDetalles[i].ultimoNivel;
                var idTipoValor = dataDetalles[i].idTipoValor;
                var strValorInferior = dataDetalles[i].valorInferior;
                var strValorSuperior = dataDetalles[i].valorSuperior;
                var intTipoNivelDetalle = dataDetalles[i].idTipoNivelDetalle;
                var idNivel = dataDetalles[i].idNivel;
                var idIdDetalleAgrupacionConstructor = dataDetalles[i].idConstructorCriterioDetalleAgrupacion;
                json.push({
                    "id": id, "text": text, "parent": parent, "data": {
                        "ultimoNivel": ckUltimoNivel, "idTipoValor": idTipoValor,
                        "valorInferior": strValorInferior, "valorSuperior": strValorSuperior,
                        "idTipoNivelDetalle": intTipoNivelDetalle, "idNivel": idNivel,
                        "idConstructorCriterioDetalleAgrupacion": idIdDetalleAgrupacionConstructor
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
             "default": { "valid_children": ["default", "file"] },
             "file": { "icon": "glyphicon glyphicon-file", "valid_children": [] }
         },
         "plugins": ["unique", "dnd", "state", "types", "contextmenu"],
         "contextmenu": {
             "items": function ($node) {
                 return {
                     "Edit": {
                         "label": "Editar",
                         "action": function (obj) {
                             this.funcBtnEditarNodo(obj);
                         }
                     }
                 };
             }
         }
     });
            

        },
        error: function () {
             funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}


//Carga para edición el detalle agrupación a modificar
function funcBtnEditarNodo() {
    $('#divNivelDetalle').addClass('hidden');
    lLimpiarValorDetalle();
    var idMes = $('#ddlMes').val();

    if (idMes == '' || idMes < 0) {
        funcMostrarMensaje("Debe seleccionar el mes", "Informativo");
        return 'false';
    } 
        var separar = idMes.split("|");
        var mes = separar[0];
        var anno = separar[1];
    var ref = $('#jstreeDetalleAgrupacion').jstree(true),
      sel = ref.get_selected()
    if (!sel.length) {
        funcMostrarMensaje("Seleccione un detalle agrupación en el árbol", "Informativo");
        return false;
    }

    nodo = ref.get_node(sel[0]);
    if (nodo.parent == "#") {
        funcMostrarMensaje("El operador (detalle raíz) no se puede editarse. Seleccione otro detalle agrupación.", "Informativo");
        return false;
    }

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
        var idNivel = nodo.data.idNivel;
        var idPadreNodo = nodo.parent;
        var idIdDetalleAgrupacionConstructor = nodo.data.idConstructorCriterioDetalleAgrupacion;
        $.ajax({
            type: "POST",
            url: 'ModificionRegistroIndicadorInterno/gMostrarValor',
            data: JSON.stringify({ IdConstructorCriterio: idIdDetalleAgrupacionConstructor, NumeroDesglose: mes, Anno: anno }),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            cache: false,
            success: function (data) {
                $('#divValor').removeClass('hidden');
                var jsonData = JSON.parse(data);
                cambioTipoValor(idTipoValor);
                $('#idTipoValor').val(idTipoValor);
                if(idTipoValor>2){
                    funcMostrarMensaje("El nuevo valor debe estar entre <strong>" + valorInferior + "</strong> y <strong>" + valorSuperior + "</strong>.", "Informativo");
                 }
                if (idTipoNivelDetalle > 0) {
                    $('#piNivelDetalle').val(idTipoNivelDetalle);
                    if (idTipoNivelDetalle == 1) {
                        $('#txtNivelDetalle').text('Provincia');
                    }
                    else if (idTipoNivelDetalle == 2) {
                        $('#txtNivelDetalle').text('Cantón');
                    }
                    else if (idTipoNivelDetalle == 3) {
                        $('#txtNivelDetalle').text('Genero');
                    }
                    $('#divNivelDetalle').removeClass('hidden');
                    var dataMeses = jsonData.data.items;
                    $("#ddlNivelDetalle").html("");
                    $("#ddlNivelDetalle").append("<option value=''>Seleccione</option>");
                    $.each(dataMeses, function (k, v) {
                        $("#ddlNivelDetalle").append("<option value=\"" + v.idItem + "\">" + v.nombreItem + "</option>");
                    });
                } else {

                    var dataRegistro = jsonData.data.detalle;
                    console.log(dataRegistro);
                    $('#txtValorAnterior').val(dataRegistro.Valor);
                    
                }
            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
       
    } else {
        funcMostrarMensaje("El detalle agrupación que se puede editar debe estar en el último nivel.", "Informativo");
    }

};
///Carga el combo cuando tiene un desglose por provincia,genero y cantón
function funcCargarValorConCombo(idItem) {
    var idValorMes = $('#ddlMes').val();
    if (idValorMes == '' || idValorMes < 0) {
        funcMostrarMensaje("Debe seleccionar el mes", "Informativo");
        return 'false';
    }
    var separar = idValorMes.split("|");
    var mes = separar[0];
    var anno = separar[1];
    var valorNivelDetalle = $('#piNivelDetalle').val();
    $.ajax({
        type: "POST",
        url: 'ModificionRegistroIndicadorInterno/gMostrarValorCombo',
        data: JSON.stringify({ idNivelDetalle: valorNivelDetalle, idMes: mes, idItemSeleccionado: idItem, idAnno: anno }),
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            
            var dataRegistro = jsonData.data.detalle;
            console.log(dataRegistro);
                $('#txtValorAnterior').val(dataRegistro.Valor);

            
        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}
///Cuarda el  valor
function functGuardarValor() {
    var valorIdTipoValor = $('#idTipoValor').val();
    var valorNuevo = obtenerValorTipoValor(valorIdTipoValor);
    var valorJustificacion = $('#txtJustificacion').val();
    $.ajax({
        type: "POST",
        url: 'ModificionRegistroIndicadorInterno/gGuardarNuevoValor',
        data: JSON.stringify({ Valor: valorNuevo, Comentario: valorJustificacion }),
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        cache: false,
        success: function (data) {
            var jsonData = JSON.parse(data);
            console.log(data);
            if (jsonData.ok == "True") {
                lLimpiarValor();
                addSuccess({ msg: "La información se actualizó con éxito" })
                $("#divMensajeError").hide();

            } else {

                $("#divMensajeError").show();
                $("#idMensajeErrorCuerpo").html(jsonData.strMensaje);
                
            }


        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}
///Control por el tipo de valor
function cambioTipoValor(idTipoValor) {
    $("#txtValorNuevo").addClass('hidden');
    $("#txtValorNuevo1").addClass('hidden');
    $("#txtValorNuevo3").addClass('hidden');
    $("#txtValorNuevo4").addClass('hidden');
    switch (String(idTipoValor)) {
        case '1':
            $("#txtValorNuevo").removeClass('hidden');
            break;
        case '2':
            $("#txtValorNuevo1").removeClass('hidden');
            break;
        case '3':
        case '4':
        case '5':
        case '7':
            $("#txtValorNuevo3").removeClass('hidden');
            break;
        case '6':
            $("#txtValorNuevo4").removeClass('hidden');
            break;
        default:

            $("#txtValorNuevo").removeClass('hidden');
            break;
    }
}
//obtiene el  nuevo valor
function obtenerValorTipoValor(idTipoValor) {
    var valor = "0";
   
    switch (idTipoValor) {
        case '1':
            valor = $("#txtValorNuevo").val();
            break;
        case '2'://fecha
            valor = $("#txtValorNuevo1").val();
           break;
        case '3'://porcentaje
        case '4'://Monto
        case '5'://Cantidad
        case '7':
            valor = $("#txtValorNuevo3").val();
            break;
        case '6':
            valor = $("#txtValorNuevo4").val();
            break;
        default:
            valor = $("#txtValorNuevo").val();
            break;
    }
    return valor;
}

//muestra el pop up de mensaje
function funcMostrarMensaje(mensaje, titulo) {
    $("#tituloMensaje").html(titulo);
    $("#contenidoMensaje").html(mensaje);
    $('#divMensaje').modal('show');
}
//muestra un mensaje deacuerdo a la busqueda de las solicitudes
function functMensajeBuscarSolicitudes() {

    $.ajax({

        type: 'POST',
        url: 'ModificionRegistroIndicadorInterno/_busquedaSolitudMensajeError',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            var jData = JSON.parse(data);
            console.log(data);
            if (jData.ok == "False") {
                addSuccess({ msg: "La busqueda a producido los siguientes resultados." })
            }
            else {
                funcMostrarMensaje(jData.strMensaje, "Error");
            }
        },
        error: function () {
            funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
        }
    });
}

//##################################################################################
//##############################Filtrar#############################################
//##################################################################################
// filtra el operador
function functionfiltrarOperador() {
    if (event.keyCode === 13) {
        var filtroIdOperador = $('#txtFiltroCodigoOperador').val();
        var filtroNombreAgrupacion = $('#txtFiltroNombreOperador').val();
       

        $.ajax({

            type: 'POST',
            url: 'ModificionRegistroIndicadorInterno/_tablaOperador',
            data: JSON.stringify({ IdOperador: filtroIdOperador, NombreOperador: filtroNombreAgrupacion }),
            contentType: 'application/json; charset=utf-8',
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
}



//filtra el indicador
function functionfiltrarIndicador() {
    if (event.keyCode === 13) {
        var filtroIdDireccion = $('#ddlDireccion').val();
        var filtroIdServicio = $('#ddlServicio').val();
        var filtroCodigoIndicador = $('#txtFiltroCodigoIndicador').val();
        var filtroTipoIndicador = $('#txtFiltroTipoIndicador').val();
        var filtroNombreIndicador = $('#txtFiltroIndicador').val();

        $.ajax({

            type: 'POST',
            url: 'ModificionRegistroIndicadorInterno/_tablaIndicador',
            data: JSON.stringify({ IdServicio: filtroIdServicio, IdDireccion: filtroIdDireccion, IdIndicador: filtroCodigoIndicador, TipoIndicador: filtroTipoIndicador, NombreIndicador: filtroNombreIndicador }),
            contentType: 'application/json; charset=utf-8',
            cache: false,
            success: function (data) {
                data = "<div id='tablaIndicador'>" + data + "</div>";

                var $form = $(data);
                $("#tablaIndicador").replaceWith(data);

                if (typeof initDataGrid !== 'undefined') {
                    $('table[data-table-grid="true"]').ready(initDataGrid);
                }
                $('#txtFiltroCodigoIndicador').val(filtroCodigoIndicador);
                $('#txtFiltroTipoIndicador').val(filtroTipoIndicador);
                $('#txtFiltroIndicador').val(filtroNombreIndicador);

            },
            error: function () {
                funcMostrarMensaje("Ocurrió un error. Contacte al administrador del sistema.", "Error");
            }
        });
    }

}


//##################################################################################
//##############################Limpiar#############################################
//##################################################################################
//limpia los valores para el nuevo valor
function lLimpiarValor() {
    $('#divValor').addClass('hidden');
    $('#txtValorAnterior').val('');
    $('#txtValorNuevo').val('');
    $('#txtValorNuevo1').val('');
    $('#txtValorNuevo3').val('');
    $('#txtValorNuevo4').val('');
    $('#txtJustificacion').val('');
    $("#ddlNivelDetalle option:eq(0)").attr("selected", "selected");
    $("#ddlMes option:eq(0)").attr("selected", "selected");
}

function lLimpiarValorDetalle() {
    $('#divValor').addClass('hidden');
    $('#txtValorAnterior').val('');
    $('#txtValorNuevo').val('');
    $('#txtValorNuevo1').val('');
    $('#txtValorNuevo3').val('');
    $('#txtValorNuevo4').val('');
    $('#txtJustificacion').val('');
    $("#ddlNivelDetalle option:eq(0)").attr("selected", "selected");
    
}
//limpia el indicador cuando hay algun cambio en el servicio o la dirección
function lLimpiarIndicador() {
    $('#txtIndicador').val('');
    $('#IdIndicador').val('');
}

$("#btnLimpiarDatos").click(function () {
    $("#btnBuscarModificarRegistro").attr("disabled", "disabled");
    $("#btnLimpiarDatos").attr("disabled", "disabled");    
    $("#btnLimpiarDatos").text("Limpiando...");
});

$("#ddlCriterio").change(function () {

    var valorIdCriterio = $("#ddlCriterio").val();
    if (valorIdCriterio == '' || valorIdCriterio < 0) {
        $("#spanErrorIdCriterio").show();        
    }
    else {
        $("#spanErrorIdCriterio").hide();
    }
});

$("#ddlServicio").change(function () {

    var valorIdServicio = $("#ddlServicio").val();
    if (valorIdServicio == '' || valorIdServicio < 0) {
        $("#spanErrorIdServicio").show();
    }
    else {
        $("#spanErrorIdServicio").hide();
    }
});





