$(document).ready(function () {

    function onSuccessEliminar(data) {
        // debugger;

        //  onsuccessAjaxEliminarSolicitud(JSON.parse(data));
    }

    function onFailureEliminar(error) {
        // debugger;

        //onFailureAjaxEliminarSolicitud(JSON.parse(error));
    }

    function onSuccessNotificar(data) {
        onsuccessAjaxNotificarSolicitud(JSON.parse(data));
    }

    function onFailureNotificar(error) {

    }

    function onSuccessActualizarFecha(data) {
        onsuccessAjaxActualizarFecha(JSON.parse(data));
    }

    function onFailureActualizarFecha(error) {
        onFailureAjaxActualizarFecha(JSON.parse(error));
    }

    AjaxifyMyForm("formEliminar",
        // onSuccessEliminar,
        onFailureEliminar
    );

    AjaxifyMyForm("formEditarFormularioWeb",
        // onSuccessEliminar,
        //onFailureEliminar
    );

    AjaxifyMyForm("formConfirmaDescargaWeb",
        // onSuccessEliminar,
        //onFailureEliminar
    );

    AjaxifyMyForm("formNotificar",
        onSuccessNotificar,
        onFailureNotificar
    );

    AjaxifyMyForm("formActualizarFechas",
        onSuccessActualizarFecha,
        onFailureActualizarFecha
    );

});

//#####################################################
function onsuccessAjaxEliminarSolicitud(data) {

    if (data.ok === 'True') {
        //debugger;

        $("#modalEliminarSolicitud").modal('hide');

        //addSuccess({ msg: data.strMensaje });

        Swal.fire(
            '',
            data.strMensaje,
            'success'
        )

        $("#frmFiltrarSolicitudes")[0].reset();
        $("#frmFiltrarSolicitudes").submit();

    } else {
        $("#divMensajeErrorEliminarSolicitud").removeClass("hidden");
        $("#divMensajeErrorEliminarSolicitud").removeAttr('style');
        $("#errorMensajeEliminar").text(data.strMensaje);
    }


}

function onFailureAjaxEliminarSolicitud(data) {
    $("#divMensajeErrorEliminarSolicitud").removeClass("hidden");
    $("#divMensajeErrorEliminarSolicitud").removeAttr('style');
    $("#errorMensajeEliminar").text(data.strMensaje);

}


function onsuccessAjaxNotificarSolicitud(data) {
    if (data.ok == 'True') {
        $("#modalNotificarSolicitud").modal('hide');

        addSuccess({ msg: data.strMensaje });

    } else {
        $("#divMensajeErrorNotificarSolicitud").removeClass("hidden");
        $("#divMensajeErrorNotificarSolicitud").removeAttr('style');
        $("#errorMensajeNotificar").text(data.strMensaje);
    }


}

function onsuccessAjaxActualizarFecha(data) {
    if (data.ok === 'True') {

        $("#modalActualizarFecha").modal('hide');

        addSuccess({ msg: data.strMensaje });

        $("#frmFiltrarSolicitudes")[0].reset();
        $("#frmFiltrarSolicitudes").submit();

    } else {
        $("#divMensajeErrorActulizarFecha").removeClass("hidden");
        $("#divMensajeErrorActulizarFecha").removeAttr('style');
        $("#errorMensajeActualizarFecha").text(data.strMensaje);
    }


}

function onFailureAjaxActualizarFecha(data) {
    $("#divMensajeErrorActulizarFecha").removeClass("hidden");
    $("#divMensajeErrorActulizarFecha").removeAttr('style');
    $("#errorMensajeActualizarFecha").text(data.strMensaje);

}
function cargaOperadoresxSolicitud(valorSeleccionado) {
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);
    $.ajax({
        type: "GET",
        url: 'Solicitud/listarOperadoresXSolicitud',
        data: { IdSolicitud: valorSeleccionado },
        contentType: "application/json",
        success: function (data) {
            $('#ddlOperadorDescarga').empty();
            for (var i = 0; i < data.length; i++) {
                $('#ddlOperadorDescarga').append('<option value="' + data[i].id + '">' + data[i].valor + '</option>');
            }
            $('#ddlOperadorDescarga').select2();
            $('#Solicitud').val = valorSeleccionado;
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
        },
        error: function () {

            $('#ddlOperadorDescarga').eq(0).html();
        }
    });
}
function cargaOperadoresxSolicitudTipoFormulario(valorSeleccionado) {
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);
    $.ajax({
        type: "GET",
        url: 'Solicitud/listarOperadoresXSolicitud',
        data: { IdSolicitud: valorSeleccionado },
        contentType: "application/json",
        success: function (data) {
            $('#ddlOperadorSolicitud').empty();
            for (var i = 0; i < data.length; i++) {
                $('#ddlOperadorSolicitud').append('<option value="' + data[i].id + '">' + data[i].valor + '</option>');
            }
            $('#ddlOperadorSolicitud').select2();
            $('#Solicitud').val = valorSeleccionado;
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
        },
        error: function () {

            $('#ddlOperadorDescarga').eq(0).html();
        }
    });
}
function cargaFormularioWebxSolicitudTipoFormulario(valorSeleccionado) {
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);
    $.ajax({
        type: "GET",
        url: 'Solicitud/FormularioWebXSolicitud',
        data: { IdSolicitud: valorSeleccionado },
        contentType: "application/json",
        success: function (data) {
            if (data == 0) {
                document.getElementById("radioFormularioExcel").checked = true;
            }
            else {
                document.getElementById("radioFormularioWeb").checked = true;
            }

            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
        },
        error: function () {

            $('#ddlOperadorDescarga').eq(0).html();
        }
    });
}
function replacer(key, value) {
    if (typeof value === 'function') {
        return value.toString()
    }
    return value
}

function actualizarSemaforos(idSolicitudConstructor, idOperador, idSemaforo, tipoConstructor) {

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);
    var tipoConstructor = localStorage.getItem("tipoConstructor");
    var a = JSON.parse(localStorage.getItem('SolicitudesArray'));
     
    let listaRegistrosJSON = localStorage.getItem("listaRegistros");
    let IdConstructorCriterio = localStorage.getItem("IdConstructorCriterio");
    
    let listaRegistros = JSON.parse(listaRegistrosJSON);
    var IdconstructorCriteriozona = localStorage.getItem("idUltimoBontonClickZona");
    //provincia, canton distrito
    if (tipoConstructor == "zona") {
        listaRegistros = obtenerValoresInputsZona(listaRegistros);

    } else {
        for (item of listaRegistros) {
            
            var id = item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno;
            item.Valor = document.getElementById(item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno).value;
            
        }

    }

    var Observacion = $('#txtIndicadorSeleccionadoObservacion').val();
    let acordionListahijoJSON = localStorage.getItem("acordionListahijoJSON");
    let acordionLista = JSON.parse(acordionListahijoJSON);
    var contador = 0;
    var contadorindicador = "";
    for (item of acordionLista) {
        item = item.split(',');
        if (contadorindicador == item[1]) {
            contador++;
        }
        contadorindicador = item[1];

    }
    if (contador==1) {
        var IdSemaforopadre = 3;
    } else {
        var IdSemaforopadre = 2;
    }
    
    //por defaul va en 3 guardadototal mas adelante evaluo el estado del semaforo padre
    var idregistroindicador = "";
     $.ajax({
        type: "POST",
        url: 'Solicitud/actualizarSemaforos',
        data: JSON.stringify({ IdSolicitudConstructor: idSolicitudConstructor, IdOperador: idOperador, idSemaforoActualizar: idSemaforo, Valor: listaRegistros, Observacion: Observacion }),
        contentType: "application/json",
         success: function (data) {
             if (data.listaDetalleRegistroIndicador != null) {
                 if (data.listaDetalleRegistroIndicador.length != 0) {

                     for (let item6 of data.listaDetalleRegistroIndicador) {

                         if (item6.IdSemaforo != 3) {
                             IdSemaforopadre = 2;
                             break;
                         } else {
                             //if (contador > 1) { //&& tipoConstructor == "zona") {
                             //    IdSemaforopadre = 2;
                             //    break;
                             //} else {
                             IdSemaforopadre = 3;
                             //break;
                             // }

                         }
                     }
                 } else {
                     IdSemaforopadre = 1;
                 }
             } else {
                 IdSemaforopadre = 1;
             }
             //fin del cambio para controlar el semaforo padre
             if (IdSemaforopadre == 1) {//blanco, amarilo, verde
                $(`#${idSolicitudConstructor}-blanco`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-amarillo`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-verde`).css("background-color", "white")
                //$("#contenidoModalMensaje").empty();
                //$("#contenidoModalMensaje").append("Los datos fueron Eliminados  con éxito");
             } else if (IdSemaforopadre == 2) {
                $(`#${idSolicitudConstructor}-blanco`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-amarillo`).css("background-color", "yellow")
                $(`#${idSolicitudConstructor}-verde`).css("background-color", "white")
                //$("#contenidoModalMensaje").empty();
                //$("#contenidoModalMensaje").append("Los datos fueron guardados de forma Parcial con éxito");

             } else if (IdSemaforopadre == 3) {
                $(`#${idSolicitudConstructor}-blanco`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-amarillo`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-verde`).css("background-color", "green")
                //$("#contenidoModalMensaje").empty();
                //$("#contenidoModalMensaje").append("Los datos fueron guardados con éxito");

             } else if (IdSemaforopadre == 5) {//blanco, amarilo, verde
                $(`#${idSolicitudConstructor}-blanco`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-amarillo`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-verde`).css("background-color", "white")
               // $("#contenidoModalMensaje").empty();
                //$("#contenidoModalMensaje").append("Los datos fueron Eliminados  con éxito en la Agrupacion seleccionada");
            } else {
                $(`#${idSolicitudConstructor}-blanco`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-amarilo`).css("background-color", "white")
                $(`#${idSolicitudConstructor}-verde`).css("background-color", "white")
            }
            //jose aqui modificamos el semaforo hijo
             if (tipoConstructor == "zona") {
                 

                 if (idSemaforo == 1) {//blanco, amarilo, verde
                     $(`#${IdconstructorCriteriozona}-blanco`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-amarillo`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-verde`).css("background-color", "white")
                     $("#contenidoModalMensaje").empty();
                     $("#contenidoModalMensaje").append("Los datos fueron Eliminados  con éxito");
                     
                    //cargaConstructorPorIndicador(idSolicitud, operador);
                    // window.location.reload();
                     $('#modalConfirmaDescargaWeb').modal('hide');
                     IdServicio = localStorage.getItem("ServicioId");
                     cambiar(IdServicio); 
                 } else if (idSemaforo == 2) {
                     $(`#${IdconstructorCriteriozona}-blanco`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-amarillo`).css("background-color", "yellow")
                     $(`#${IdconstructorCriteriozona}-verde`).css("background-color", "white")
                     $("#contenidoModalMensaje").empty();
                     $("#contenidoModalMensaje").append("Los datos fueron guardados de forma Parcial con éxito");

                 } else if (idSemaforo == 3) {
                     $(`#${IdconstructorCriteriozona}-blanco`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-amarillo`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-verde`).css("background-color", "green")
                     $("#contenidoModalMensaje").empty();
                     $("#contenidoModalMensaje").append("Los datos fueron guardados con éxito");

                 } else if (idSemaforo == 5) {//blanco, amarilo, verde
                     $(`#${IdconstructorCriteriozona}-blanco`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-amarillo`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-verde`).css("background-color", "white")
                     $("#contenidoModalMensaje").empty();
                     $("#contenidoModalMensaje").append("Los datos fueron Eliminados  con éxito en la Agrupacion seleccionada");
                 } else {
                     $(`#${IdconstructorCriteriozona}-blanco`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-amarilo`).css("background-color", "white")
                     $(`#${IdconstructorCriteriozona}-verde`).css("background-color", "white")
                 }

             } else {  // semaforo hijo
            if (idSemaforo == 1) {//blanco, amarilo, verde
                $(`#${IdConstructorCriterio}-blanco`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-amarillo`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-verde`).css("background-color", "white")
                $("#contenidoModalMensaje").empty();
                $("#contenidoModalMensaje").append("Los datos fueron Eliminados  con éxito");
                $('#modalConfirmaDescargaWeb').modal('hide');
                IdServicio = localStorage.getItem("ServicioId");
                cambiar(IdServicio); 
            } else if (idSemaforo == 2) {
                $(`#${IdConstructorCriterio}-blanco`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-amarillo`).css("background-color", "yellow")
                $(`#${IdConstructorCriterio}-verde`).css("background-color", "white")
                $("#contenidoModalMensaje").empty();
                $("#contenidoModalMensaje").append("Los datos fueron guardados de forma Parcial con éxito");

            } else if (idSemaforo == 3) {
                $(`#${IdConstructorCriterio}-blanco`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-amarillo`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-verde`).css("background-color", "green")
                $("#contenidoModalMensaje").empty();
                $("#contenidoModalMensaje").append("Los datos fueron guardados con éxito");

            } else if (idSemaforo == 5) {//blanco, amarilo, verde
                $(`#${IdConstructorCriterio}-blanco`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-amarillo`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-verde`).css("background-color", "white")
                $("#contenidoModalMensaje").empty();
                $("#contenidoModalMensaje").append("Los datos fueron Eliminados  con éxito en la Agrupacion seleccionada");
            } else {
                $(`#${IdConstructorCriterio}-blanco`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-amarilo`).css("background-color", "white")
                $(`#${IdConstructorCriterio}-verde`).css("background-color", "white")
                }
             }
            //$("#contenidoModalMensaje").empty();
            //$("#contenidoModalMensaje").append("Los datos fueron guardados con éxito");
            $('#modalMensaje').modal('show');

            if (data.listaDetalleRegistroIndicador != null) {
                for (let item of data.listaDetalleRegistroIndicador) {
                    var id = item.IdConstructorCriterio + '_' + item.NumeroDesglose + '_' + item.Anno;
                    var input = document.getElementById(id);
                    if (input != null) {
                        document.getElementById(id).value = item.Valor;
                    }
                }
            }

            $("#" + IdConstructorCriterio + '_').addClass("btn-dark");
            $(`#${localStorage.getItem("idUltimoBontonClickZona")}`).addClass("btn-dark");
            $(`#${localStorage.getItem("idUltimoBontonClickZona")}`).removeClass("btn-primary");
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
        },
        error: function () {

            $('#ddlOperadorDescarga').eq(0).html();
        }
    });
   
     


}
$("#formEliminar").submit(function (e) {
    var idsolicitud = $('#ItemEliminar').val();
    var Operador = $("#ddlOperadorDescarga").val() == null ? "" : $("#ddlOperadorDescarga").val();
    var SolicitudCompleta = document.getElementById('Completa').checked;
    var json = {
        "ItemEliminar": idsolicitud,
        "Operadores": Operador,
        "Completa": SolicitudCompleta
    };

    var options = {
        type: "post",
        dataType: "json",
        url: "/Solicitud/EliminarOpcional",
        data: { "data": json },
        success: function (data) {
            if (data.ok === "True") {

                $("#modalEliminarSolicitud").modal('hide');

                Swal.fire(
                    '',
                    data.strMensaje,
                    'success'
                )
                $("#frmFiltrarSolicitudes")[0].reset();
                $("#frmFiltrarSolicitudes").submit();
            } else {
                $("#divMensajeErrorEliminarSolicitud").removeClass("hidden");
                $("#divMensajeErrorEliminarSolicitud").removeAttr('style');
                $("#errorMensajeEliminar").text(data.strMensaje);
            }
        },
        error: function (error) {
            // window.location = "/Solicitud?em=true";
        }
    };
    $.ajax(options);

});


function cargaConstructorPorIndicador(valorSeleccionado, idoperadorSeleccionado) {

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);

    $('#divIndicadorSeleccionado').empty();
    $('#divListIndicadores').empty();
    //$('#idConstructorOperador').empty();
    $('#idConstructorDireccion').empty();
    $('#idConstructorServicio').empty();
    $('#idConstructorDesglose').empty();
    $('#idConstructorFrecuencia').empty();

    localStorage.removeItem("rutaAcordeon");
    $("#tituloDetalleIndicadores").html("Detalle de agrupación");
    localStorage.setItem("IdSolicitud", valorSeleccionado);

    var ventanaActual = location.pathname.replace("/", " ").trim();
    $('#txtIndicadorSeleccionadoObservacion').val('');
    $("#divIndicadorSeleccionadoObservacion").addClass("hide");
    $.ajax({
        type: "GET",
        url: 'Solicitud/listaConstructorPorIndicador',
        data: { IdSolicitud: valorSeleccionado, IdOperadorSeleccionado: idoperadorSeleccionado, ventana: ventanaActual },
        contentType: "application/json",
        success: function (data) {

            var acordion = '';
            var acordionLista = [];
            var acordionListahijo = [];
            var acordionListaSolicitud = [];
            var Lista = [];
            var ExistList = [];
            var contador = 0;
            var Nombre_CriterioAnterior = "";


            for (let item of (data.detalleAgrupacionesPorOperador.map(x => x.ID_Indicador)).filter(onlyUnique)) {
                var hijo = '';
                var hijos = '';
                var estado = 0;
                contador = 0;
                var existeIndocadorPorZona = data.detalleAgrupacionesPorOperador.filter(x => x.ID_Indicador == item && x.Tabla_Tipo_Nivel_Detalle != '').length;

                if (existeIndocadorPorZona == 0) {

                    var nivelmax = data.detalleAgrupacionesPorOperador.filter(x => x.ID_Indicador == item).filter(onlyUnique);

                    if (nivelmax[0].NivelMaximo == 1 && nivelmax[0].NivelDetalle) {
                        var ListaIndicador = data.detalleAgrupacionesPorOperador.filter(x => x.ID_Indicador == item && x.UltimoNivel == 1);
                        var nivelmaximo = ListaIndicador.filter(x => x.UltimoNivel == 1)[0].NivelMaximo;
                        var nivelminimo = ListaIndicador.filter(x => x.UltimoNivel == 1)[0].NivelMinimo;
                    } else {
                        var ListaIndicador = data.detalleAgrupacionesPorOperador.filter(x => x.ID_Indicador == item && x.UltimoNivel == 0);
                        var nivelmaximo = ListaIndicador.filter(x => x.UltimoNivel == 0)[0].NivelMaximo;
                        var nivelminimo = ListaIndicador.filter(x => x.UltimoNivel == 0)[0].NivelMinimo;
                    }
                    //Cambio para sacar cual es el primer padre con mas de un hijo
                    var NivelmaximoAcordion = 0;
                    for (var k = nivelminimo; k <= nivelmaximo; k++) {
                        for (let Lista1 of ListaIndicador.filter(x => x.IdNivel == k)) {
                            var cantidadhijos = 0;
                            for (let Lista2 of ListaIndicador.filter(x => x.Id_Padre_ConstructorCriterio == Lista1.Id_ConstructorCriterio)) {
                                cantidadhijos++;
                                if (cantidadhijos > 1) {
                                    NivelmaximoAcordion = Lista1.IdNivel;
                                    k = nivelmaximo++;
                                }

                            }

                        }
                    }
                    //// Aqui termina el cambio para saber el valor maximo a dibujar

                    for (var i = nivelmaximo; i >= nivelminimo; i--) {

                        for (let item2 of ListaIndicador.filter(x => x.IdNivel == i)) {

                            var ListTemporal = [];

                            for (let item3 of ListaIndicador.filter(x => x.Id_Padre_ConstructorCriterio == item2.Id_Padre_ConstructorCriterio)) {

                                if ((ExistList.filter(x => x == item3.Id_ConstructorCriterio)).length == 0) {

                                    ListTemporal.push(item3);
                                    ExistList.push(item3.Id_ConstructorCriterio);
                                }
                            }

                            for (let item4 of ListTemporal) {

                                var html = '';
                                var code = '';


                                for (let item5 of Lista.filter(x => x.Padre == item4.Id_ConstructorCriterio)) {
                                    html = html + item5.HTML;
                                }
                                if (item4.IdSemaforo == 1) {
                                    colorBlanco = "white";
                                    colorAmarillo = "white";
                                    colorVerde = "white";
                                    btnColorsemaforo = 'white';

                                } else if (item4.IdSemaforo == 2) {
                                    colorBlanco = "white";
                                    colorAmarillo = "yellow";
                                    colorVerde = "white";
                                    btnColorsemaforo = 'yellow';

                                } else if (item4.IdSemaforo == 3) {
                                    colorBlanco = "white";
                                    colorAmarillo = "white";
                                    colorVerde = "green";
                                    btnColorsemaforo = 'green';

                                } else {
                                    colorBlanco = "white";
                                    colorAmarillo = "white";
                                    colorVerde = "white";

                                }
                                //fin del semaforo principal
                                // inicio de la revision de que si tiene valores guardados

                                var btnColor = '';
                                var btnColorsemaforo = '';

                                if (item4.Tiene_Hijos) {
                                    btnColor = 'btn-dark';
                                    btnColorsemaforo = 'btn-dark';
                                    if (item4.IdSemaforohijo == 1) {
                                        colorBlancose = "white";
                                        colorAmarillose = "white";
                                        colorVerdese = "white";
                                        btnColorsemaforo = 'white';

                                    } else if (item4.IdSemaforohijo == 2) {
                                        colorBlancose = "white";
                                        colorAmarillose = "yellow";
                                        colorVerdese = "white";
                                        btnColorsemaforo = 'yellow';

                                    } else if (item4.IdSemaforohijo == 3) {
                                        colorBlancose = "white";
                                        colorAmarillose = "white";
                                        colorVerdese = "green";
                                        btnColorsemaforo = 'green';

                                    } else {
                                        colorBlancose = "white";
                                        colorAmarillose = "white";
                                        colorVerdese = "white";

                                    }
                                } else {
                                    btnColor = '';
                                    colorBlancose = "white";
                                    colorAmarillose = "white";
                                    colorVerdese = "white";
                                }
                                if ((ListaIndicador.filter(x => x.Id_Padre_ConstructorCriterio == item4.Id_ConstructorCriterio)).length == 0 && nivelmaximo != 1) {
                                    //code = '<button type="button" class="btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item4.Id_ConstructorCriterio + '_" title="' + item4.Nombre_Padre_Detalle_Agrupacion + '_' + item4.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item4.Id_ConstructorCriterio + '\')">' + item4.Nombre_Detalle_Agrupacion + '</button>';                                                                                                                                                                                              //function seleccionarConstructor(IdConstructorCriterio, zona, idGnero, nombreBoton, nombreIndicador, nombreProvincia) {${item} ${item4.Nombre_Indicador}
                                    // code = '<button type="button" class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item4.Id_ConstructorCriterio + '_" title="'+ item4.Nombre_Padre_Detalle_Agrupacion + item4.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item4.Id_ConstructorCriterio + '\',   \' \',   \' \',    \'' + item4.Nombre_Detalle_Agrupacion + '\', \'' + item + ' ' + item4.Nombre_Indicador + '\', \'\')">' + item4.Nombre_Detalle_Agrupacion + '</button>';
                                    code = '<div class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColorsemaforo + '"><button type="button" class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item4.Id_ConstructorCriterio + '_" title="' + item4.Nombre_Padre_Detalle_Agrupacion + item4.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item4.Id_ConstructorCriterio + '\', ' + undefined + ', ' + undefined + ', \'' + item4.Nombre_Detalle_Agrupacion + '\', \'' + item + ' ' + item4.Nombre_Indicador + '\', ' + undefined + ', \'' + item4.Id_Solicitud_Constructor + '\')">' + item4.Nombre_Detalle_Agrupacion + '</button>';
                                    code += `<table>
                                        <tr>
                                         <td><div id="${item4.Id_ConstructorCriterio}-blanco" title="Blanco" style="width: 20px; height: 20px; background-color:${colorBlancose};border-radius: 50%;border-color:black; /*border:2px solid black;*/" class="blanco"> </div></td>
                                         <td><div id="${item4.Id_ConstructorCriterio}-amarillo" title="Amarillo" style="width: 20px; height: 20px; background-color:${colorAmarillose};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="amarillo"> </div></td>
                                         <td><div id="${item4.Id_ConstructorCriterio}-verde" title="Verde" style="width: 20px; height: 20px; background-color:${colorVerdese};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="verde"> </div></td>
                                       </tr>
                                       </table></div>`// ojo cambio de sutel para controlar semaforos
                                    acordionListahijo.push(item4.Id_ConstructorCriterio +','+ item4.ID_Indicador);
                                } else {
                                    if (NivelmaximoAcordion == 0) {
                                        NivelmaximoAcordion = (nivelmaximo - 1);// para eliminar los demas niveles si no tiene disyuntivas
                                    }
                                    if (i == 1 || i <= NivelmaximoAcordion) {

                                        if ((ListaIndicador.filter(x => x.Id_Padre_ConstructorCriterio == item4.Id_ConstructorCriterio)).length == 0 && nivelmaximo == 1) {
                                            // html =/*boton*/'<button type="button" class="btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item4.Id_ConstructorCriterio + '_" title="' + item4.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item4.Id_ConstructorCriterio + '\')">' + item4.Nombre_Detalle_Agrupacion + '</button>';                                      
                                            //Recorro los hijos
                                            if (Nombre_CriterioAnterior != item4.Nombre_Criterio) {
                                                estado = 0;
                                            }
                                            if (estado == 0) {
                                                for (let item6 of ListaIndicador) {
                                                    if (item6.Nombre_Criterio == item4.Nombre_Criterio) {
                                                        if (item6.Tiene_Hijos) {
                                                            btnColor = 'btn-dark';
                                                            if (item6.IdSemaforohijo == 1) {
                                                                colorBlancose = "white";
                                                                colorAmarillose = "white";
                                                                colorVerdese = "white";
                                                                btnColorsemaforo = 'white';

                                                            } else if (item6.IdSemaforohijo == 2) {
                                                                colorBlancose = "white";
                                                                colorAmarilloe = "yellow";
                                                                colorVerdese = "white";
                                                                btnColorsemaforo = 'yellow';

                                                            } else if (item6.IdSemaforohijo == 3) {
                                                                colorBlancose = "white";
                                                                colorAmarillose = "white";
                                                                colorVerdese = "green";
                                                                btnColorsemaforo = 'green';

                                                            } else {
                                                                colorBlancose = "white";
                                                                colorAmarillose = "white";
                                                                colorVerdese = "white";

                                                            }
                                                            /// fin de la codificacion del semaforo hijo                                                           
                                                            ;
                                                        } else {
                                                            btnColor = '';
                                                            colorBlancose = "white";
                                                            colorAmarillose = "white";
                                                            colorVerdese = "white";
                                                        }

                                                        html1 =/*boton*/'<div class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColorsemaforo + '"><button type="button" class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item6.Id_ConstructorCriterio + '_" title="' + item4.Nombre_Padre_Detalle_Agrupacion + item6.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item6.Id_ConstructorCriterio + '\', ' + undefined + ', ' + undefined + ', \'' + item4.Nombre_Detalle_Agrupacion + '\', \'' + item + ' ' + item4.Nombre_Indicador + '\', ' + undefined + ', \'' + item4.Id_Solicitud_Constructor + '\' )">' + item6.Nombre_Detalle_Agrupacion + '</button>';
                                                        html1 += `<table>
                                                                        <tr>
                                                                         <td><div id="${item4.Id_ConstructorCriterio}-blanco" title="Blanco" style="width: 20px; height: 20px; background-color:${colorBlancose};border-radius: 50%;border-color:black; /*border:2px solid black;*/" class="blanco"> </div></td>
                                                                         <td><div id="${item4.Id_ConstructorCriterio}-amarillo" title="Amarillo" style="width: 20px; height: 20px; background-color:${colorAmarillose};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="amarillo"> </div></td>
                                                                         <td><div id="${item4.Id_ConstructorCriterio}-verde" title="Verde" style="width: 20px; height: 20px; background-color:${colorVerdese};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="verde"> </div></td>
                                                                       </tr>
                                                                       </table></div>`// ojo cambio de sutel para controlar semaforos
                                                        //html1 =/*boton*/'<button type="button" class="btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item6.Id_ConstructorCriterio + '_" title="' + item6.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item6.Id_ConstructorCriterio + '\')">' + item6.Nombre_Detalle_Agrupacion + '</button>';
                                                        //html1 =/*boton*/'<button type="button" class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item6.Id_ConstructorCriterio + '_" title="' + item4.Nombre_Padre_Detalle_Agrupacion + item6.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item6.Id_ConstructorCriterio + '\',   \' \',   \' \',    \'' + item4.Nombre_Detalle_Agrupacion + '\',   \'' + item + ' ' + item4.Nombre_Indicador + '\',   \'\' )">' + item6.Nombre_Detalle_Agrupacion + '</button>';
                                                        hijos += html1;
                                                        acordionListahijo.push(item4.Id_ConstructorCriterio + ',' + item4.ID_Indicador);
                                                    }
                                                }
                                            }

                                            //*********************************** Correccion de niveles hijos******************************
                                            ////Prueba de cambio Funcional
                                            if (hijos != " " && estado == 0) {
                                                hijo +=
                                                    //'<div class="accordion-container accordion-container-shadow">' +
                                                    //'<a style="text-decoration: none; color: inherit;" class="accordion-titulo ccordion-titulo" data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + ' " role="button" aria-expanded="false" aria-controls="collapseExample">' +
                                                    //'<span hidden>' + item4.Id_Solicitud_Constructor + '</span>' +
                                                    //'<span data-toggle="collapse"  data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + ' " style="float: right;" class="iconoAcordeon glyphicon glyphicon-plus"></span>' +
                                                    //'<p class="iconoAcordeon" data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + ' " id="' + item4.Id_ConstructorCriterio + '">' + item4.Nombre_Criterio + '</p> ' +
                                                    //'</a>' +
                                                    //'<div class="collapse p-1" id="collapseExample' + item4.Id_ConstructorCriterio + '" style="height: auto;">' +
                                                    //'<div class="card card-body">' +
                                                    hijos +
                                                    //'</div>' +
                                                    //'</div>' +
                                                    '</div>'
                                                estado = 1;
                                                contador = contador++;
                                                hijos = " ";
                                            }

                                            //+++++++++++++++++++++++++++++++++++++++++++++++Cambio ***************************

                                        } else {
                                            hijo +=
                                                //'<div class="accordion-container accordion-container-shadow">' +
                                                //'<a style="text-decoration: none; color: inherit;"class="accordion-titulo ccordion-titulo" role="button" aria-expanded="false" aria-controls="collapseExample">' +
                                                //'<span hidden>' + item4.Id_Solicitud_Constructor + '</span>' +
                                                //'<span data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + '-primero " style="float: right;" class="iconoAcordeon glyphicon glyphicon-plus"></span>' +
                                                //'<p class="iconoAcordeon" data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + '-primero " id="' + item4.Id_ConstructorCriterio + '">' + item4.Nombre_Criterio + '</p> ' +
                                                //'</a>' +
                                                //'<div class="collapse p-1" id="collapseExample' + item4.Id_ConstructorCriterio + '-primero" style="height: auto;">' +


                                                //'<div class="accordion-container accordion-container-shadow">' +
                                                //'<a style="text-decoration: none; color: inherit;" class="accordion-titulo ccordion-titulo" role="button" aria-expanded="false" aria-controls="collapseExample">' +
                                                //'<span hidden>' + item4.Id_Solicitud_Constructor + '</span>' +
                                                //'<span data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + ' " style="float: right;" class="iconoAcordeon glyphicon glyphicon-plus"></span>' +
                                                //'<p class="iconoAcordeon" data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + ' " id="' + item4.Id_ConstructorCriterio + '">' + item4.Nombre_Detalle_Agrupacion + '</p> ' +
                                                //'</a>' +
                                                //'<div class="collapse p-1" id="collapseExample' + item4.Id_ConstructorCriterio + '" style="height: auto;">' +
                                                //'<div class="card card-body">' +

                                                html +
                                                //'</div>' +
                                                //'</div>' +
                                                '</div>'

                                            //'<div class="card card-body">' +

                                            //    html +
                                            //    '</div>' +
                                            //    '</div>' +
                                            //    '</div>'
                                        }
                                        acordionLista.push(item4.Id_ConstructorCriterio);
                                        
                                        acordionListaSolicitud.push(item4.Id_Solicitud_Constructor + '-verde');

                                        if (i == 1) {
                                            code = `
                                            <div class="accordion-container accordion-container-shadow">
											 																											
                                                <a style="text-decoration: none; color: inherit;" class="accordion-titulo ccordion-titulo " role="button" aria-expanded="false" aria-controls="collapseexample">
                                                    <button style="border-radius: 50%;" data-book-id="${item4.Constructor_Criterio_Ayuda}" type="button" class="btn btn-warning" data-target="#modalAyuda" data-select="true" data-json-selected="{&quot;ItemNotificar&quot;:&quot; 806f36c4-3ceb-411c-9f60-e10f248b713b &quot;}" data-tooltip="true" title="Descripción del indicador" data-toggle="modal" data-placement="left">
                                                        <span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span>
                                                    </button>
                                                    <span hidden>${item4.Id_Solicitud_Constructor}</span>
                                                    <span data-toggle="collapse" href = "#collapseexample${item4.Id_ConstructorCriterio}-Padre" onclick="cerrarAcordeon()" style = "float: right;" class="iconoAcordeon glyphicon glyphicon-plus" ></span >
                                                    <p class="iconoAcordeon" data-toggle="collapse" href = "#collapseexample${item4.Id_ConstructorCriterio}-Padre">${item} ${item4.Nombre_Indicador}</p> 
                                                    <table>
                                                        <tr>
                                                            <td><div id="${item4.Id_Solicitud_Constructor}-blanco" title="Blanco" style="width: 20px; height: 20px; background-color:${colorBlanco};border-radius: 50%;border-color:black; /*border:2px solid black;*/" class="blanco"> </div></td>
                                                            <td><div id="${item4.Id_Solicitud_Constructor}-amarillo" title="Amarillo" style="width: 20px; height: 20px; background-color:${colorAmarillo};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="amarillo"> </div></td>
                                                            <td><div id="${item4.Id_Solicitud_Constructor}-verde" title="Verde" style="width: 20px; height: 20px; background-color:${colorVerde};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="verde"> </div></td>
                                                        </tr>
                                                    </table>
                                                </a>
                                                <div class="collapse p-1" id="collapseexample${item4.Id_ConstructorCriterio}-Padre" style="height: auto;">
                                                    <div class="card card-body">
                                                        ${hijo}
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                            `
                                        }
                                        /// jose revisar
                                    } else {
                                        code =
                                            '<div class="accordion-container accordion-container-shadow">' +
                                            '<a style="text-decoration: none; color: inherit;" class="accordion-titulo" >' +
                                            '<span hidden class="iconoAcordeon">' + item4.Id_Solicitud_Constructor + '</span>' +
                                            '<span data-toggle="collapse"  data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + ' " style="float: right;" class="iconoAcordeon glyphicon glyphicon-plus"></span>' +
                                            '<p  style="text-decoration: none" class="iconoAcordeon" data-toggle="collapse" href="#collapseExample' + item4.Id_ConstructorCriterio + ' " role="button" aria-expanded="false" aria-controls="collapseExample"  id="' + item4.Id_ConstructorCriterio + '">' + item4.Nombre_Detalle_Agrupacion + '</p> ' +
                                            '</a>' +
                                            '<div class="collapse p-1" id="collapseExample' + item4.Id_ConstructorCriterio + '" style="height: auto;">' +
                                            '<div class="card card-body">' +

                                            html +
                                            '</div>' +
                                            '</div>' +
                                            '</div>'
                                    }

                                }

                                Nombre_CriterioAnterior = item4.Nombre_Criterio;
                                Lista.push({ Padre: item4.Id_Padre_ConstructorCriterio, HTML: code });
                            }
                        }

                    }
                } else {//jose aqui 

                    var ListaIndicadorPorPronciaGenero = data.detalleAgrupacionesPorOperador.filter(x => x.ID_Indicador == item && x.UltimoNivel > 0).filter(onlyUnique);
                    if (ListaIndicadorPorPronciaGenero != undefined) {

                        var indicadorPadreZona = ``;
                        var provinciasHTML = ``;
                        var cantonesHTML = ``;

                        var indicadorZona = ``;
                        var iconoMas = ``;
                        var onclickProvincias = ``;

                        // for (let item2 of ListaIndicadorPorPronciaGenero.filter(x => x.IdNivel == i)) {
                        if (ListaIndicadorPorPronciaGenero[0].IdSemaforo == 1) {
                            colorBlanco = "white";
                            colorAmarillo = "white";
                            colorVerde = "white";

                        } else if (ListaIndicadorPorPronciaGenero[0].IdSemaforo == 2) {
                            colorBlanco = "white";
                            colorAmarillo = "yellow";
                            colorVerde = "white";

                        } else if (ListaIndicadorPorPronciaGenero[0].IdSemaforo == 3) {
                            colorBlanco = "white";
                            colorAmarillo = "white";
                            colorVerde = "green";

                        } else {
                            colorBlanco = "white";
                            colorAmarillo = "white";
                            colorVerde = "white";

                        }

                        ListaIndicadorPorPronciaGenero.forEach(function (item2, i) {

                            if (item2.Tabla_Tipo_Nivel_Detalle != null) {
                                var zona = item2.Tabla_Tipo_Nivel_Detalle;

                                if (zona == "PROVINCIA") {
                                    iconoMas = '<span onclick="seleccionarConstructor(\'' + item2.Id_ConstructorCriterio + '\', \'PROVINCIA\',\'' + item2.IDNIVELDETALLEGENERO + '\', \'\', \'' + item + ' ' + item2.Nombre_Indicador + '\')" style="float: right;" class="iconoAcordeon glyphicon glyphicon-plus"></span>'
                                    onclickProvincias = 'onclick="seleccionarConstructor(\'' + item2.Id_ConstructorCriterio + '\', \'PROVINCIA\',\'' + item2.IDNIVELDETALLEGENERO + '\', \'\', \'' + item + ' ' + item2.Nombre_Indicador + '\')"';


                                } else if (zona == "CANTON") {
                                    iconoMas = `<span data-toggle="collapse" href="#${item2.Id_ConstructorCriterio}-provincias" style="float: right;" class="iconoAcordeon glyphicon glyphicon-plus"></span>`
                                    provinciasHTML = ``
                                    //data.provincias.splice(7, 1) //Cambio para eliminar la provincia no especificado
                                    data.provincias.forEach(provincias => {
                                        btnColor = '';
                                        btnColorsemaforo = ''; 
                                        var SemaforoHijo='';
                                        if (item2.listaZonasTienenDatos != null) {
                                            
                                            item2.listaZonasTienenDatos.forEach(zonasTienenDatos => {
                                                if (zonasTienenDatos.idProvincia == provincias.IdProvincia && zonasTienenDatos.idConstructorCriterio == item2.Id_ConstructorCriterio) {
                                                    btnColor = 'btn-dark';
                                                     SemaforoHijo = data.listaDetalleRegistroIndicador.filter(x => x.IdProvincia == provincias.IdProvincia).filter(onlyUnique);
                                                    return false; 
                                                } 
                                            })
                                        }
                                        provinciaSinTilde = provincias.Nombre.replace(/[^-A-Za-z0-9]+/g, '-')
                                    //    // cambio para obtener el semaforo hijo
                                        
                                     if (SemaforoHijo != '') {

                                        if (SemaforoHijo[0].IdSemaforo == 1) {
                                            colorBlancose = "white";
                                            colorAmarillose = "white";
                                            colorVerdese = "white";
                                            btnColorsemaforo = 'white';

                                        } else if (SemaforoHijo[0].IdSemaforo == 2) {
                                            colorBlancose = "white";
                                            colorAmarillose = "yellow";
                                            colorVerdese = "white";
                                            btnColorsemaforo = 'yellow';

                                        } else if (SemaforoHijo[0].IdSemaforo == 3) {
                                            colorBlancose = "white";
                                            colorAmarillose = "white";
                                            colorVerdese = "green";
                                            btnColorsemaforo = 'green';

                                        }
                                     }else {
                                            colorBlancose = "white";
                                            colorAmarillose = "white";
                                            colorVerdese = "white";
                                        }
                                    //    // fin del cambio semaforo hijo por canton
    
                                        html =/*boton*/'<div class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColorsemaforo + '"><button id="' + item2.Id_ConstructorCriterio + '_CANTON_' + provincias.IdProvincia + '" type="button" class="bontonAcordeon btnZonas ' + btnColor + ' btn btn-primary btn-block mb-0-5" onclick="seleccionarConstructor(\'' + item2.Id_ConstructorCriterio + '\', \'CANTON-' + provincias.IdProvincia + '\', \'' + item2.IDNIVELDETALLEGENERO + '\', \'' + provincias.Nombre + '\', \'' + item + ' ' + item2.Nombre_Indicador + '\', ' + undefined + ', \'' + item2.Id_Solicitud_Constructor + '\' )">' + provincias.Nombre + '</button>';
                                        html += `<table>
                                        <tr>
                                         <td><div id="${item2.Id_ConstructorCriterio + '_CANTON_' + provincias.IdProvincia}-blanco" title="Blanco" style="width: 20px; height: 20px; background-color:${colorBlancose};border-radius: 50%;border-color:black; /*border:2px solid black;*/" class="blanco"> </div></td>
                                         <td><div id="${item2.Id_ConstructorCriterio + '_CANTON_' + provincias.IdProvincia}-amarillo" title="Amarillo" style="width: 20px; height: 20px; background-color:${colorAmarillose};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="amarillo"> </div></td>
                                         <td><div id="${item2.Id_ConstructorCriterio + '_CANTON_' + provincias.IdProvincia}-verde" title="Verde" style="width: 20px; height: 20px; background-color:${colorVerdese};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="verde"> </div></td>
                                       </tr>
                                       </table></div>`
                                        //html1 =/*boton*/'<button type="button" class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColor + '" id="' + item6.Id_ConstructorCriterio + '_" title="' + item4.Nombre_Padre_Detalle_Agrupacion + item6.Nombre_Detalle_Agrupacion + '" onclick="seleccionarConstructor(\'' + item6.Id_ConstructorCriterio + '\', ' + undefined + ', ' + undefined + ', \'' + item4.Nombre_Detalle_Agrupacion + '\', \'' + item + ' ' + item4.Nombre_Indicador + '\', ' + undefined + ', \'' + item4.Id_Solicitud_Constructor + '\' )">' + item6.Nombre_Detalle_Agrupacion + '</button>';
                                        provinciasHTML = provinciasHTML + `
                                                <div class="accordion-container accordion-container-shadow">
                                                    <a class="accordion-titulo ccordion-titulo" data-toggle="collapse" href="#collapseExample${item2.Id_ConstructorCriterio}-${provinciaSinTilde}" role="button" aria-expanded="false" aria-controls="collapseExample">
                                                       ${html}
                                                    </a>
    
                                                </div>`
                                        acordionListahijo.push(item2.Id_ConstructorCriterio + ',' + item2.ID_Indicador);
                                    });
                                } else if (zona == "DISTRITO") {
                                    provinciasHTML = ``;
                                    iconoMas = `<span data-toggle="collapse" href="#${item2.Id_ConstructorCriterio}-provincias" style="float: right;" class="iconoAcordeon glyphicon glyphicon-plus"></span>`

                                    data.provincias.forEach(provincias => {
                                        provinciaSinTilde = provincias.Nombre.replace(/[^-A-Za-z0-9]+/g, '-')

                                        data.cantones.forEach(canton => {
                                            btnColor = '';
                                            btnColorsemaforo = ''; 
                                            var SemaforoHijo = '';
                                            if (item2.listaZonasTienenDatos != null) {
                                                item2.listaZonasTienenDatos.forEach(zonasTienenDatos => {
                                                    if (zonasTienenDatos.idCanton == canton.IdCanton && zonasTienenDatos.idConstructorCriterio == item2.Id_ConstructorCriterio) {
                                                        btnColor = 'btn-dark';
                                                        SemaforoHijo = data.listaDetalleRegistroIndicador.filter(x => x.IdCanton == canton.IdCanton).filter(onlyUnique);
                                                        return false;
                                                    }
                                                })
                                            }
                                            cantonSinTilde = canton.Nombre.replace(/[\u0300-\u036f]/g, "")
                                            //    // cambio para obtener el semaforo hijo

                                            if (SemaforoHijo != '') {

                                                if (SemaforoHijo[0].IdSemaforo == 1) {
                                                    colorBlancose = "white";
                                                    colorAmarillose = "white";
                                                    colorVerdese = "white";
                                                    btnColorsemaforo = 'white';

                                                } else if (SemaforoHijo[0].IdSemaforo == 2) {
                                                    colorBlancose = "white";
                                                    colorAmarillose = "yellow";
                                                    colorVerdese = "white";
                                                    btnColorsemaforo = 'yellow';

                                                } else if (SemaforoHijo[0].IdSemaforo == 3) {
                                                    colorBlancose = "white";
                                                    colorAmarillose = "white";
                                                    colorVerdese = "green";
                                                    btnColorsemaforo = 'green';

                                                }
                                            } else {
                                                colorBlancose = "white";
                                                colorAmarillose = "white";
                                                colorVerdese = "white";
                                                btnColorsemaforo = 'white';
                                            }
                                        // fin del cambio semaforo hijo por Distrito

                                            //.filter(x => x.ID_Indicador == item).filter(onlyUnique);
                                            if (canton.IdProvincia == provincias.IdProvincia) {
                                                provincia = data.provincias.filter(x => x.IdProvincia == canton.IdProvincia).filter(onlyUnique);
                                                html =/*boton*/'<div class="bontonAcordeon btn btn-primary btn-block mb-0-5 ' + btnColorsemaforo + '"><button id="' + item2.Id_ConstructorCriterio + '_DISTRITO_' + canton.IdCanton + '" type="button" class="bontonAcordeon btnZonas ' + btnColor + ' btn btn-primary btn-block mb-0-5" onclick="seleccionarConstructor(\'' + item2.Id_ConstructorCriterio + '\', \'DISTRITO-' + canton.IdCanton + '\', \'' + item2.IDNIVELDETALLEGENERO + '\', \'' + canton.Nombre + '\', \'' + item + ' ' + item2.Nombre_Indicador + '\',\'' + provincia[0].Nombre + '\', ' + undefined + ', \'' + item2.Id_Solicitud_Constructor + '\')">' + canton.Nombre + '</button>';
                                                html += `<table>
                                        <tr>
                                         <td><div id="${item2.Id_ConstructorCriterio + '_DISTRITO_' + canton.IdCanton }-blanco" title="Blanco" style="width: 20px; height: 20px; background-color:${colorBlancose};border-radius: 50%;border-color:black; /*border:2px solid black;*/" class="blanco"> </div></td>
                                         <td><div id="${item2.Id_ConstructorCriterio + '_DISTRITO_' + canton.IdCanton}-amarillo" title="Amarillo" style="width: 20px; height: 20px; background-color:${colorAmarillose};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="amarillo"> </div></td>
                                         <td><div id="${item2.Id_ConstructorCriterio + '_DISTRITO_' + canton.IdCanton}-verde" title="Verde" style="width: 20px; height: 20px; background-color:${colorVerdese};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="verde"> </div></td>
                                       </tr>
                                       </table></div>`
                                                cantonesHTML +=
                                                    `
                                                            <div class="accordion-container accordion-container-shadow">
                                                                <a class="accordion-titulo ccordion-titulo" role="button" aria-expanded="false" aria-controls="collapseExample">
                                                                    ${html}
                                                                </a>
                                                            </div>
                                                            `
                                            }
                                        });
                                        provinciasHTML += `
                                                        <div class="accordion-container accordion-container-shadow">
                                                            <a style="text-decoration: none; color: inherit;" class="accordion-titulo ccordion-titulo" role="button" aria-expanded="false" aria-controls="collapseExample">
                                                                <span hidden>${item2.Id_Solicitud_Constructor}</span>
                                                                <span data-toggle="collapse" href = "#collapseExample${item2.Id_ConstructorCriterio}-${provinciaSinTilde}" style = "float: right;" class="iconoAcordeon glyphicon glyphicon-plus" ></span >
                                                                <p data-toggle="collapse" href = "#collapseExample${item2.Id_ConstructorCriterio}-${provinciaSinTilde}" id="Distrito" class="iconoAcordeon ${item2.Id_Solicitud_Constructor}">${provincias.Nombre}</p>
                                                            </a>
                                                            <div class="collapse p-1" id="collapseExample${item2.Id_ConstructorCriterio}-${provinciaSinTilde}" style="height: auto;">
                                                                <div class="card card-body">
                                                                    ${cantonesHTML}
                                                                </div>
                                                            </div>
                                                        </div>`

                                        cantonesHTML = ``;
                                        acordionListahijo.push(item2.Id_ConstructorCriterio + ',' + item2.ID_Indicador);
                                    });

                                }
                                nombreDetalleAgrupacion = `
                                                        <div class="accordion-container accordion-container-shadow">
                                                            <a style="text-decoration: none; color: inherit;" class="accordion-titulo ccordion-titulo" role="button" aria-expanded="false" aria-controls="collapseexample">
                                                                <span hidden>${item2.Id_Solicitud_Constructor}</span>
                                                                ${iconoMas}
                                                                <p class="iconoAcordeon" data-toggle="collapse" href="#${item2.Id_ConstructorCriterio}-provincias" ${onclickProvincias} id="${item2.Id_ConstructorCriterio}" class="${item2.Id_Solicitud_Constructor}">${item2.Nombre_Detalle_Agrupacion}</p>
                                                            </a>
                                                            <div id="${item2.Id_ConstructorCriterio}-provincias" class="collapse p-1" style="height: auto;">
                                                                <div class="card card-body">
                                                                    ${provinciasHTML}
                                                                </div>
                                                            </div>
                                                        </div>

                                                        `

                                indicadorZona += `
                                                <div class="accordion-container accordion-container-shadow">
                                                    <a style="text-decoration: none; color: inherit;" class="accordion-titulo ccordion-titulo" role="button" aria-expanded="false" aria-controls="collapseexample">
                                                        <span hidden>${item2.Id_Solicitud_Constructor}</span>
                                                        <span data-toggle="collapse" href = "#collapseexample${item2.Id_ConstructorCriterio}-hijo-${i}" style = "float: right;" class="iconoAcordeon glyphicon glyphicon-plus" ></span >
                                                        <p  data-toggle="collapse" href = "#collapseexample${item2.Id_ConstructorCriterio}-hijo-${i}" id="${item2.Id_ConstructorCriterio}"class="iconoAcordeon ${item2.Id_Solicitud_Constructor}">${item2.Nombre_Criterio}</p>
                                                    </a>
                                                    <div class="collapse p-1" id="collapseexample${item2.Id_ConstructorCriterio}-hijo-${i}" style="height: auto;">
                                                        <div class="card card-body">
                                                        ${nombreDetalleAgrupacion}
                                                        </div>
                                                    </div>
                                                </div>
                                                `
                            }
                            acordionListaSolicitud.push(item2.Id_Solicitud_Constructor + '-verde');
                        })

                        acordionLista.push(ListaIndicadorPorPronciaGenero[0].Id_ConstructorCriterio);
                        indicadorPadreZona = `
                                            <div class="accordion-container accordion-container-shadow">

                                                <a style="text-decoration: none; color: inherit;" class="accordion-titulo ccordion-titulo" role="button" aria-expanded="false" aria-controls="collapseexample">
                                                    <button style="border-radius: 50%;" data-book-id="${ListaIndicadorPorPronciaGenero[0].Constructor_Criterio_Ayuda}" type="button" class="btn btn-warning" data-target="#modalAyuda" data-select="true" data-json-selected="{&quot;ItemNotificar&quot;:&quot; 806f36c4-3ceb-411c-9f60-e10f248b713b &quot;}" data-tooltip="true" title="Descripción del indicador" data-toggle="modal" data-placement="left">
                                                        <span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span>
                                                    </button>
                                                    <span hidden>${ListaIndicadorPorPronciaGenero[0].Id_Solicitud_Constructor}</span>
                                                    <span data-toggle="collapse" onclick="cerrarAcordeon(\'' + ListaIndicadorPorPronciaGenero[0].Id_Solicitud_Constructor + '\')" href = "#collapseexample${ListaIndicadorPorPronciaGenero[0].Id_ConstructorCriterio}-Padre" style = "float: right;" class="iconoAcordeon glyphicon glyphicon-plus" ></span >
                                                    <p class="iconoAcordeon" data-toggle="collapse" " href = "#collapseexample${ListaIndicadorPorPronciaGenero[0].Id_ConstructorCriterio}-Padre" id="${ListaIndicadorPorPronciaGenero[0].Id_Solicitud_Constructor}">${item} ${ListaIndicadorPorPronciaGenero[0].Nombre_Indicador}</p> 
                                                    <table>
                                                        <tr>
                                                            <td><div id="${ListaIndicadorPorPronciaGenero[0].Id_Solicitud_Constructor}-blanco" title="Blanco" style="width: 20px; height: 20px; background-color:${colorBlanco};border-radius: 50%;border-color:black; /*border:2px solid black;*/" class="blanco"> </div></td>
                                                            <td><div id="${ListaIndicadorPorPronciaGenero[0].Id_Solicitud_Constructor}-amarillo" title="Amarillo" style="width: 20px; height: 20px; background-color:${colorAmarillo};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="amarillo"> </div></td>
                                                            <td><div id="${ListaIndicadorPorPronciaGenero[0].Id_Solicitud_Constructor}-verde" title="Verde" style="width: 20px; height: 20px; background-color:${colorVerde};border-radius: 50%;border-color:black; /*border:2px solid black;*/"class="verde"> </div></td>
                                                        </tr>
                                                    </table>
                                                </a>
                                                <div class="collapse p-1" id="collapseexample${ListaIndicadorPorPronciaGenero[0].Id_ConstructorCriterio}-Padre" style="height: auto;">
                                                    <div class="card card-body">
                                                        ${indicadorZona}
                                                    </div>
                                                </div>
                                            </div>
                                            `
                       // acordionListaSolicitud.push(item2.Id_Solicitud_Constructor + '-verde');
                    }

                }


                if (indicadorZona != `` && indicadorZona != undefined) {
                    acordion = ``;
                    acordion += indicadorPadreZona;
                    indicadorZona = ``

                } else {
                    acordion = Lista[Lista.length - 1].HTML;

                }
                $('#divListIndicadores').append(acordion);
                contador++
            }
            if (idoperadorSeleccionado == "" || idoperadorSeleccionado == undefined || idoperadorSeleccionado == null) {
                $('#idConstructorOperador').empty();

                for (var i = 0; i < data.listaOperador.length; i++) {
                    $('#idConstructorOperador').append(`<option value= "${data.listaIdOperador[i]}"> ${data.listaOperador[i]}</option>`);
                }
            }
            if (ventanaActual == "RegistroIndicador") {
                $("#idConstructorOperador").attr("disabled", true);
            } else {
                $("#btnLimpiar").addClass("hide");
                $("#btnGuardadoParcial").addClass("hide");
                $("#btnGuardadoTotal").addClass("hide");
                $("#btnCargar").addClass("hide");
            }

            $('#idConstructorDireccion').append(data.direccion);
            localStorage.setItem('idConstructorDireccion', data.IdDireccion);
            $('#idConstructorServicio').append(data.servicio);
            $('#idConstructorFrecuencia').append(data.frecuencia);
            $('#idConstructorDesglose').append(data.desglose);



            const acordionListaJSON = JSON.stringify(acordionLista);
            const acordionListahijosJSON = JSON.stringify(acordionListahijo);
            const acrodionlistasolicitudes = JSON.stringify(acordionListaSolicitud);
            localStorage.removeItem('acordionListaJSON');
            localStorage.removeItem('SolicitudesArray');
            localStorage.removeItem('acordionListahijoJSON');
            localStorage.setItem('acordionListaJSON', acordionListaJSON);
            localStorage.setItem('SolicitudesArray', acrodionlistasolicitudes);
            localStorage.setItem('acordionListahijoJSON', acordionListahijosJSON);
            

            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
        },
        error: function () {

            $('#divListIndicadores').eq(0).html();
            $('#idConstructorOperador').eq(0).html();
            $('#idConstructorDireccion').eq(0).html();
            $('#idConstructorServicio').eq(0).html();
            $('#idConstructorDesglose').eq(0).html();
            $('#idConstructorFrecuencia').eq(0).html();
        },
        complete: function () {
            $(".iconoAcordeon").click(function (e) {
                //if (ventanaActual == "RegistroIndicador") {
                if ($(this).is("span") && $(this).hasClass("glyphicon glyphicon-plus")) {
                    //cambia el icono mas por el menos
                    //alert("jose si entre (+)");
                    $(this).toggleClass('glyphicon glyphicon-plus glyphicon glyphicon-minus');
                    //********************************

                    var idConstructorSeleccionado = $(this).prev().text()
                    if (idConstructorSeleccionado != "") {
                        localStorage.setItem("idConstructorSeleccionado", idConstructorSeleccionado)
                    }

                    var nombreIndicador = $(this).next().text();
                    var idConstructor = $(this).prev().text();

                    //localStorage.setItem("idConstructorSeleccionado", idConstructor)
                    localStorage.setItem("ultimoCliqueado", nombreIndicador);

                    //$("#btnLimpiar").prop('disabled', false);
                    $("#btnGuardadoParcial").prop('disabled', false);
                    $("#btnGuardadoTotal").prop('disabled', false);
                    $('#divIndicadorSeleccionado').empty();
                    $("#divIndicadorSeleccionadoObservacion").removeClass("hide");
                    $("#divIndicadorSeleccionadoObservacion").addClass("hide");
                } else if ($(this).is("span") && !$(this).hasClass("glyphicon glyphicon-plus")) {
                    //cambia el icono menos por el mas
                    //alert("jose si entre (-)");
                    $(this).toggleClass('glyphicon glyphicon-minus glyphicon glyphicon-plus');
                    //********************************

                    var idConstructorSeleccionado = $(this).prev().text()
                    if (idConstructorSeleccionado != "") {
                        localStorage.setItem("idConstructorSeleccionado", idConstructorSeleccionado)
                    }

                    var nombreIndicador = $(this).next().text();
                    var idConstructor = $(this).prev().text();
                    //localStorage.setItem("idConstructorSeleccionado", idConstructor)
                    localStorage.setItem("ultimoCliqueado", nombreIndicador);

                    $("#tituloDetalleIndicadores").html("Detalle de agrupación: " + nombreIndicador)

                    //$("#btnLimpiar").prop('disabled', true);
                    $("#divIndicadorSeleccionadoObservacion").removeClass("hide");
                    $("#divIndicadorSeleccionadoObservacion").addClass("hide");
                    $('#divIndicadorSeleccionado').empty();
                    $("#btnGuardadoParcial").prop('disabled', true);
                    $("#btnGuardadoTotal").prop('disabled', true);
                    $("#tituloDetalleIndicadores").html("Detalle de agrupación");

                } else if ($(this).prev().is("span") && $(this).prev().hasClass("glyphicon glyphicon-plus")) {
                    //este else if es para cuando el usuario no cliquea el icono y si cliquea el texto del indicador o constructor
                    //alert("jose si entre (+)");
                    //cambia el icono mas por el menos
                    $(this).prev().toggleClass('glyphicon glyphicon-plus glyphicon glyphicon-minus');
                    //********************************

                    var idConstructorSeleccionado = $(this).prev().prev().text()
                    if (idConstructorSeleccionado != "") {
                        localStorage.setItem("idConstructorSeleccionado", idConstructorSeleccionado)
                    }

                    var nombreIndicador = $(this).text();
                    var idConstructor = $(this).prev().text();
                    //localStorage.setItem("idConstructorSeleccionado", idConstructor)
                    $("#tituloDetalleIndicadores").html("Detalle de agrupación: " + nombreIndicador)
                    localStorage.setItem("ultimoCliqueado", nombreIndicador);

                    //$("#btnLimpiar").prev('disabled', false);
                    //$("#btnGuardadoParcial").prop('disabled', false);
                    //$("#btnGuardadoTotal").prop('disabled', false);
                    $('#divIndicadorSeleccionado').empty();
                    $("#divIndicadorSeleccionadoObservacion").removeClass("hide");
                    $("#divIndicadorSeleccionadoObservacion").addClass("hide");
                } else if ($(this).prev().is("span") && !$(this).prev().hasClass("glyphicon glyphicon-plus")) {
                    //este else if es para cuando el usuario no cliquea el icono y si cliquea el texto del indicador o constructor
                    //alert("jose si entre (-)");
                    //cambia el icono menos por el mas
                    $(this).prev().toggleClass('glyphicon glyphicon-minus glyphicon glyphicon-plus');
                    var idConstructorSeleccionado = $(this).prev().prev().text()
                    //*******************************

                    if (idConstructorSeleccionado != "") {
                        localStorage.setItem("idConstructorSeleccionado", idConstructorSeleccionado)
                    }

                    var nombreIndicador = $(this).text();
                    var idConstructor = $(this).prev().text();
                    //localStorage.setItem("idConstructorSeleccionado", idConstructor)
                    $("#tituloDetalleIndicadores").html("Detalle de agrupación: " + nombreIndicador)
                    localStorage.setItem("ultimoCliqueado", nombreIndicador);

                    $("#btnLimpiar").prop('disabled', true);
                    $("#divIndicadorSeleccionadoObservacion").removeClass("hide");
                    $("#divIndicadorSeleccionadoObservacion").addClass("hide");
                    $('#divIndicadorSeleccionado').empty();
                    $("#btnGuardadoParcial").prop('disabled', true);
                    $("#btnGuardadoTotal").prop('disabled', true);
                    $("#tituloDetalleIndicadores").html("Detalle de agrupación");


                }
                //} else {
                //    $("#btnLimpiar").addClass("hide");
                //    $("#btnGuardadoParcial").addClass("hide");
                //    $("#btnGuardadoTotal").addClass("hide");
                //    $("#btnCargar").addClass("hide");

                //}
            });
            $(".btnZonas").click(function (e) {
                localStorage.setItem("idUltimoBontonClickZona", $(this).attr("id"))

            });
            $(".bontonAcordeon").click(function (e) {
                $(".bontonAcordeon").removeClass("btn-success");
                $(this).addClass("btn-success")

            });
        }
    });
}


function cerrarAcordeon() {
    let acordionListaJSON = localStorage.getItem("acordionListaJSON");
    let acordionLista = JSON.parse(acordionListaJSON);

    for (var item of acordionLista) {
        var id = "#collapseexample" + item + "-Padre";

        //console.log(id);
    }
}
function cargarDatos() {

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(200);
    var idsolicitud = $('#IDSolicitudIndicador').val();
    var poDireccion = localStorage.getItem('idConstructorDireccion');
    var Operador = $("#idConstructorOperador").val() == null ? "" : $("#idConstructorOperador").val();

    $.ajax({
        type: "GET",
        url: 'Solicitud/cargarDatos',
        data: { IdSolicitud: idsolicitud, idoperador: Operador, poDireccion: poDireccion },
        contentType: "application/json",
        success: function (data) {

            $("#contenidoModalMensaje").empty();
            $("#contenidoModalMensaje").append("Los datos fueron cargados con éxito");
            $('#modalMensaje').modal('show');
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
            },
        error: function () {

        }
    });
}


$(function () {
    //blanco ->1
    //amarillo -> 2
    //verde -> 3
    $("#btnLimpiar").click(function (e) {
        var r = confirm("¿Esta seguro de eliminar los valores guardados en el indicador?");
        if (r == true) {
            let listaRegistrosJSON = localStorage.getItem("listaRegistros");
            let idUltimoBontonClickZona = localStorage.getItem("idUltimoBontonClickZona");
            let IdConstructorCriterio = localStorage.getItem("IdConstructorCriterio");
            var tipoConstructor = localStorage.getItem("tipoConstructor");
            let listaRegistros = JSON.parse(listaRegistrosJSON);
            if (tipoConstructor == "zona") {
                listaRegistros = obtenerValoresInputsZona(listaRegistros);
            }

            for (item of listaRegistros) {
                if (tipoConstructor == "zona") {

                    if (item.zona == "CANTON") {
                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idCanton).value = "";

                    } else if (item.zona == "PROVINCIA") {
                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idProvincia).value = "";

                    } else if (item.zona == "DISTRITO") {
                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idDistrito).value = "";

                    }
                    $("#" + idUltimoBontonClickZona).removeClass("btn-dark");
                    $("#" + idUltimoBontonClickZona).addClass("btn-success");

                } else {
                    document.getElementById(item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno).value = "";
                }


            }

            document.getElementById('txtIndicadorSeleccionadoObservacion').value = "";
            var idConstructorSeleccionado = localStorage.getItem("idConstructorSeleccionado")
            var idOperadorSeleccionado = $("#idConstructorOperador").val();
            actualizarSemaforos(idConstructorSeleccionado, idOperadorSeleccionado, 1);
            
        }//else {
        //        var R = confirm("¿Esta seguro de eliminar los valores de la agrupacion seleccionada?");
        //        if (R == true) {
        //            let listaRegistrosJSON = localStorage.getItem("listaRegistros");
        //            let idUltimoBontonClickZona = localStorage.getItem("idUltimoBontonClickZona");
        //            let IdConstructorCriterio = localStorage.getItem("IdConstructorCriterio");
        //            var tipoConstructor = localStorage.getItem("tipoConstructor");
        //            let listaRegistros = JSON.parse(listaRegistrosJSON);
        //            if (tipoConstructor == "zona") {
        //                listaRegistros = obtenerValoresInputsZona(listaRegistros);
        //            }

        //            for (item of listaRegistros) {
        //                if (tipoConstructor == "zona") {

        //                    if (item.zona == "CANTON") {
        //                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idCanton).value = "";

        //                    } else if (item.zona == "PROVINCIA") {
        //                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idProvincia).value = "";

        //                    } else if (item.zona == "DISTRITO") {
        //                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idDistrito).value = "";

        //                    }
        //                    $("#" + idUltimoBontonClickZona).removeClass("btn-dark");
        //                    $("#" + idUltimoBontonClickZona).addClass("btn-success");

        //                } else {
        //                    document.getElementById(item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno).value = "";
        //                }
        //            }

        //            document.getElementById('txtIndicadorSeleccionadoObservacion').value = "";
        //            var idConstructorSeleccionado = localStorage.getItem("idConstructorSeleccionado")
        //            var idOperadorSeleccionado = $("#idConstructorOperador").val();
        //            actualizarSemaforos(idConstructorSeleccionado, idOperadorSeleccionado, 5);
        //        }

        //}
        
    });
    $("#btnLimpiarD").click(function (e) {
        var r = confirm("¿Esta seguro de eliminar los valores guardados en la Agrupacion Seleccionada?");
        if (r == true) {
            let listaRegistrosJSON = localStorage.getItem("listaRegistros");
            let idUltimoBontonClickZona = localStorage.getItem("idUltimoBontonClickZona");
            let IdConstructorCriterio = localStorage.getItem("IdConstructorCriterio");
            var tipoConstructor = localStorage.getItem("tipoConstructor");
            let listaRegistros = JSON.parse(listaRegistrosJSON);
            if (tipoConstructor == "zona") {
                listaRegistros = obtenerValoresInputsZona(listaRegistros);
            }

            for (item of listaRegistros) {
                if (tipoConstructor == "zona") {

                    if (item.zona == "CANTON") {
                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idCanton).value = "";

                    } else if (item.zona == "PROVINCIA") {
                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idProvincia).value = "";

                    } else if (item.zona == "DISTRITO") {
                        document.getElementById(item.numero_desglose + '_' + item.anno + '_' + item.idDistrito).value = "";

                    }
                    $("#" + idUltimoBontonClickZona).removeClass("btn-dark");
                    $("#" + idUltimoBontonClickZona).addClass("btn-success");

                } else {
                    document.getElementById(item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno).value = "";
                }


            }

            document.getElementById('txtIndicadorSeleccionadoObservacion').value = "";
            var idConstructorSeleccionado = localStorage.getItem("idConstructorSeleccionado")
            var idOperadorSeleccionado = $("#idConstructorOperador").val();
            actualizarSemaforos(idConstructorSeleccionado, idOperadorSeleccionado, 5);
        }
    });

     $("#btnGuardadoParcial").click(function (e) {
         var tipoConstructor = localStorage.getItem("tipoConstructor");
         let listaRegistrosJSON = localStorage.getItem("listaRegistros");
         let listaRegistros = JSON.parse(listaRegistrosJSON);

         //provincia, canton distrito
         if (tipoConstructor == "zona") {
             listaRegistros = obtenerValoresInputsZona(listaRegistros);

         } else {
             localStorage.setItem("Valida", true);
             for (item of listaRegistros) {
                 valida = localStorage.getItem("Valida");
                 if (valida == "true") {
                     var id = item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno;
                     item.Valor = document.getElementById(item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno).value;
                     Validar(id, item.nombreDetalleAgrupacion, item);
                 }

             }

         }
         valida = localStorage.getItem("Valida");
         if (valida == "true") {
         idConstructorSeleccionado = localStorage.getItem("idConstructorSeleccionado")
         idOperadorSeleccionado = $("#idConstructorOperador").val();
         actualizarSemaforos(idConstructorSeleccionado, idOperadorSeleccionado, 2);
         }
         else {
             alert("Existen datos incorrectos por favor revise los valores a guardar");
         }
           
    });

    $("#btnGuardadoTotal").click(function (e) {
        //valida = localStorage.getItem("Valida");
        //if (valida == "true") {
        //    idConstructorSeleccionado = localStorage.getItem("idConstructorSeleccionado")
        //    idOperadorSeleccionado = $("#idConstructorOperador").val();
        //    actualizarSemaforos(idConstructorSeleccionado, idOperadorSeleccionado, 3);
        //} else {
        //    alert("Existen datos incorrectos por favor revise los valores a guardar");
        //}
        var tipoConstructor = localStorage.getItem("tipoConstructor");
        let listaRegistrosJSON = localStorage.getItem("listaRegistros");
        let listaRegistros = JSON.parse(listaRegistrosJSON);

        //provincia, canton distrito
        if (tipoConstructor == "zona") {
            listaRegistros = obtenerValoresInputsZona(listaRegistros);

        } else {
            for (item of listaRegistros) {
                valida = localStorage.getItem("Valida");
                if (valida == "true") {
                    var id = item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno;
                    item.Valor = document.getElementById(item.Id_ConstructorCriterio + '_' + item.Numero_Desglose + '_' + item.Anno).value;
                    Validar(id, "", item);
                }

            }

        }
        valida = localStorage.getItem("Valida");
        if (valida == "true") {
            idConstructorSeleccionado = localStorage.getItem("idConstructorSeleccionado")
            idOperadorSeleccionado = $("#idConstructorOperador").val();
            actualizarSemaforos(idConstructorSeleccionado, idOperadorSeleccionado, 3);
        }
        else {
            alert("Existen datos incorrectos por favor revise los valores a guardar");
        }

        
    });
    $("#btnCargar").click(function (e) {
        var verificar = true;
        let acordionListaJSON = localStorage.getItem("SolicitudesArray");
        let acordionLista = JSON.parse(acordionListaJSON);

        for (var item of acordionLista) {
            //var id = "#" + item;
            let element = document.getElementById(item);
             
            if (element.style.backgroundColor != "green") {
                if (element.style.backgroundColor != "rgb(0, 128, 0)") {
                    verificar = false;
                }
                
            }
        }
        idConstructorSeleccionado = localStorage.getItem("idConstructorSeleccionado")
        if (verificar) {
            var r = confirm("¿Sus Datos seran enviados a la Sutel por favor Confirme?");
        } else {
            var r = confirm("“En esta solicitud de información existen indicadores con “Guardado parcial” o sin editar (marcados con semáforo amarillo o en blanco respectivamente), está seguro (a) de que la información a remitir es la oficial?”");
        }
        //var r = confirm("“En esta solicitud de información existen indicadores con “Guardado parcial” o sin editar (marcados con semáforo amarillo o en blanco respectivamente), está seguro (a) de que la información a remitir es la oficial?”");
       // var r = confirm("¿Sus Datos seran enviados a la Sutel por favor Confirme?");
        if (r == true) {
            
            cargarDatos();
            $('#modalConfirmaDescargaWeb').modal('hide');
        }
        
    });


    $('#modalAyuda').on('show.bs.modal', function (e) {
        var ayudaIndicador = $(e.relatedTarget).data('book-id');
        $("#contenidoModalAyuda").empty();
        $("#contenidoModalAyuda").append(ayudaIndicador);
    });

    $("#idConstructorOperador").change(function (e) {
        $("#divIndicadorSeleccionadoObservacion").removeClass("hide");
        $("#divIndicadorSeleccionadoObservacion").addClass("hide");
        //$("#btnLimpiar").prop('disabled', true);
        $("#btnGuardadoParcial").prop('disabled', true);
        $("#btnGuardadoTotal").prop('disabled', true);

        $("#tituloDetalleIndicadores").html("Detalle de agrupación");

        var idSolicitud = localStorage.getItem("IdSolicitud");
        var operador = $("#idConstructorOperador").val();

        cargaConstructorPorIndicador(idSolicitud, operador)
    });

    $(".accordion-titulo").click(function (e) {
        alert("jose si entre (mae es el titulo)");
        e.preventDefault();
        alert("jose si entre (mae es el titulo)");
        var contenido = $(this).next(".accordion-content");

        if (contenido.css("display") == "none") { //open		
            contenido.slideDown(250);
            $(this).addClass("open");
        }
        else { //close		
            contenido.slideUp(250);
            $(this).removeClass("open");
        }

    });
});
function CargaTotal(idsemaforo) {

    var r = confirm("“En esta solicitud de información existen indicadores con “Guardado parcial” o sin editar (marcados con semáforo amarillo o en blanco respectivamente), está seguro (a) de que la información a remitir es la oficial?”");
    // var r = confirm("¿Sus Datos seran enviados a la Sutel por favor Confirme?");
    if (r == true) {
        cargarDatos();
    }

}
function crearRutaAcordeon(idSolicitudConstructor, zona) {
    //input[id *= 'idSolicitudConstructor']
    var ruta = "";
    var rutaCompleta = "";
    var idSolicitudIndicador = "";
    var nombreSolicitudIndicador = "";

    $("p[id*=" + idSolicitudConstructor + "]").each(function (i, el) {

        idSolicitudIndicador = $(this).attr('class');;
        var valor = $(this).text();

        ruta += valor + "/";

    })
    if (zona != "" || zona != undefined) {
        //if (zona == "DISTRITO") {
        //    ruta += localStorage.getItem("ultimoCliqueado") + "/"
        //}


        //$("p[id*=" + idSolicitudIndicador + "]").each(function (i, el) {

        //    nombreSolicitudIndicador = $(this).text() + "/";

        //})


    }
    rutaCompleta = nombreSolicitudIndicador + ruta;

    return rutaCompleta

}

function obtenerValoresInputsZona(listaRegistrosPredefinidos) {
    var listaRegistros = []

    var arrayZona = $("#idZona").val().split("-");



    var contador = 1;
    $(".inputProvincia").each(function () {

        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: listaRegistrosPredefinidos[0].Anno,
            zona: "PROVINCIA",
            idZona: arrayZona[1],
            idProvincia: arrayDatos[2],
            idCanton: "",
            idDistrito: ""

        });
        contador++;
    })

    if (listaRegistros.length != 0)
        return listaRegistros

    $(".inputCanton").each(function () {
        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: arrayDatos[1],//listaRegistrosPredefinidos[0].Anno,
            zona: "CANTON",
            idProvincia: arrayZona[1],
            idCanton: arrayDatos[2],
            idDistrito: ""
        });
        contador++;

    })

    if (listaRegistros.length != 0)
        return listaRegistros
    $(".inputDistrito").each(function () {
        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: arrayDatos[1],//listaRegistrosPredefinidos[0].Anno,
            zona: "DISTRITO",
            idZona: arrayZona[1],
            idProvincia: "",
            idCanton: arrayZona[1],
            idDistrito: arrayDatos[2]
        });
    })
    if (listaRegistros.length != 0)
        return listaRegistros


    /***************************************************************************************/
    //Obtiene todos los inputs que sean por sean provincia canton o distrito y genero Hombre y Mujer
    $(".inputProvinciaHombre").each(function () {

        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: listaRegistrosPredefinidos[0].Anno,
            zona: "PROVINCIA",
            idZona: arrayZona[1],
            idProvincia: arrayDatos[2],
            idCanton: "",
            idDistrito: "",
            idGenero: 1

        });

    })

    $(".inputProvinciaMujer").each(function () {

        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: listaRegistrosPredefinidos[0].Anno,
            zona: "PROVINCIA",
            idZona: arrayZona[1],
            idProvincia: arrayDatos[2],
            idCanton: "",
            idDistrito: "",
            idGenero: 2

        });

    });

    if (listaRegistros.length != 0)
        return listaRegistros

    $(".inputCantonHombre").each(function () {
        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: arrayDatos[1],//listaRegistrosPredefinidos[0].Anno,
            zona: "CANTON",
            idProvincia: arrayZona[1],
            idCanton: arrayDatos[2],
            idDistrito: "",
            idGenero: 1
        });


    })
    $(".inputCantonMujer").each(function () {
        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: arrayDatos[1],//listaRegistrosPredefinidos[0].Anno,
            zona: "CANTON",
            idProvincia: arrayZona[1],
            idCanton: arrayDatos[2],
            idDistrito: "",
            idGenero: 2
        });


    })

    if (listaRegistros.length != 0)
        return listaRegistros

    $(".inputDistritoHombre").each(function () {
        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: arrayDatos[1],//listaRegistrosPredefinidos[0].Anno,
            zona: "DISTRITO",
            idZona: arrayZona[1],
            idProvincia: "",
            idCanton: arrayZona[1],
            idDistrito: arrayDatos[2],
            idGenero: 1
        });
    })

    $(".inputDistritoMujer").each(function () {
        //mes_año_(id de la zona que corresponde el input)
        var arrayDatos = $(this).attr("id").split("_")

        listaRegistros.push({
            id_constructorcriterio: listaRegistrosPredefinidos[0].Id_ConstructorCriterio,
            id_tipo_valor: listaRegistrosPredefinidos[0].Id_Tipo_Valor,
            numero_desglose: arrayDatos[0],
            valor: $(this).val(),
            anno: arrayDatos[1],//listaRegistrosPredefinidos[0].Anno,
            zona: "DISTRITO",
            idZona: arrayZona[1],
            idProvincia: "",
            idCanton: arrayZona[1],
            idDistrito: arrayDatos[2],
            idGenero: 2
        });
    })

    if (listaRegistros.length != 0)
        return listaRegistros


    /**************************************************************************************/
}


function seleccionarConstructor(IdConstructorCriterio, zona, idGnero, nombreBoton, nombreIndicador, nombreProvincia,solicitudconstructor) {
   // seleccionarConstructor(\'' + item2.Id_ConstructorCriterio + '\', \'CANTON-' + provincias.IdProvincia + '\', \'' + item2.IDNIVELDETALLEGENERO + '\', \'' + provincias.Nombre + '\', \'' + item + ' ' + item2.Nombre_Indicador + '\', \'' + item4.Id_Solicitud_Constructor + '\' )">' + provincias.Nombre + '</button>';
                                         
    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
    $(".darkScreen").fadeIn(500);
    
   
    var IdSolicitudConstructor = localStorage.getItem("idConstructorSeleccionado");
    if (solicitudconstructor != undefined) {
        IdSolicitudConstructor = solicitudconstructor;
    }
    rutaIndicador = crearRutaAcordeon(IdConstructorCriterio);
    localStorage.setItem("IdConstructorCriterio", IdConstructorCriterio);

    $("#tituloDetalleIndicadores").html("Detalle de agrupación: " + rutaIndicador)
    var idsolicitud = $('#IDSolicitudIndicador').val();
    var Operador = $("#idConstructorOperador").val() == null ? "" : $("#idConstructorOperador").val();
    $('#divIndicadorSeleccionado').empty();
    $('#divIndicadorSeleccionadoError').empty();
    $('#txtIndicadorSeleccionadoObservacion').val('');
    $("#divIndicadorSeleccionadoObservacion").addClass("hide");
    $.ajax({
        type: "GET",
        url: 'Solicitud/seleccionarConstructor',
        data: { IdConstructorCriterio: IdConstructorCriterio, IdSolicitud: idsolicitud, IdSolicitudConstructor: IdSolicitudConstructor, idoperador: Operador, Zona: zona },
        contentType: "application/json",
        success: function (data) {
            $('#divIndicadorSeleccionado').empty();

            var registro = data.detalleAgrupacionesPorOperador[0];
            var fechaFinal = new Date(registro.FechaBaseParaCrearExcel);
            var fechaInicio = new Date(fechaFinal);
            var mesinicial = registro.MesInicial;
            //fechaInicio.setMonth(mesinicial);
            //fechaInicio.setMonth(fechaInicio.getMonth() - registro.Cantidad_Meses_Frecuencia);
            if (mesinicial == 0 && registro.Cantidad_Meses_Frecuencia == 12) {
                //fechaInicio.setMonth(0);// esto es por que es anual y tiene que iniciar en el primer mes 
                fechaInicio.setMonth(mesinicial - registro.Cantidad_Meses_Frecuencia);
            } else {
                fechaInicio.setMonth(mesinicial - registro.Cantidad_Meses_Frecuencia);
            }

            var content = '<table id="indicadores" class="table table-bordered table-hover">';
            content = content + '<thead><tr><th></th>';
            var header = [];

            var datosValidar = {
                Valor_Inferior: '',
                Valor_Superior: '',
                Id_Tipo_Valor: ''

            }

            if (data.detalleAgrupacionesPorOperador.length > 0 && (data.provincias == null && data.cantones == null && data.distritos == null)) {
                var J = fechaFinal.getMonth();
                localStorage.setItem("tipoConstructor", "")

                for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                    var fecha = new Date(fechaInicio);
                    //fecha.setMonth(i);
                    fecha.setMonth(fecha.getMonth() + i);
                    content = content + '<th scope="col">' + dateToString(fecha) + '</th>';
                    header.push(dateToString(fecha));
                }

                content = content + '</thead></tr>';
                var listaRegistros = [];
                var filaId = 0;
                for (let item of data.detalleAgrupacionesPorOperador) {
                    filaId = filaId + 1;
                    var columnaId = 0;
                    var validaciones = setValidaciones(item);
                    content = content + '<tr><th>' + item.Nombre_Detalle_Agrupacion + '</th>';


                    for (var k = 0; k < registro.Cantidad_Meses_Frecuencia; k++) {
                        var mes = new Date(fechaInicio);
                        //fecha.setMonth(i);
                        mes.setMonth(mes.getMonth() + k);
                        columnaId = columnaId + 1
                        var varId = item.Id_ConstructorCriterio + '_' + (mes.getMonth()+1)  + '_' + fechaInicio.getFullYear();
                       // content = content + '<th><input ' + validaciones + ' onkeydown="return event.keyCode !== 69 && event.keyCode !== 189 && event.keyCode !== 187" value="" class="form-control" id="' + varId + '" onblur="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[k] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" onkeyup="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[k] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" /><div id="' + varId + '_feedback" class="invalid-feedback hide"></div></th>';
                        content = content + '<th><textarea ' + validaciones + '  value="" class="form-control" id="' + varId + '" onblur="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[k] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" onkeyup="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[k] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" /></textarea><div id="' + varId + '_feedback" class="invalid-feedback hide"></div></th>';

                        listaRegistros.push({
                            Id_ConstructorCriterio: item.Id_ConstructorCriterio,
                            Id_Tipo_Valor: item.Id_Tipo_Valor,
                            Numero_Desglose: (mes.getMonth() + 1),
                            Valor: '',
                            Anno: fechaInicio.getFullYear(),
                            Valor_Inferior: item.Valor_Inferior,
                            Valor_Superior: item.Valor_Superior
                        });
                    }
                    content = content + '</tr>';
                }
                localStorage.setItem('listaRegistros', JSON.stringify(listaRegistros));
                localStorage.setItem("tipoConstructor", "SinNivel")
                content = content + '</tbody></table>';
                $('#divIndicadorSeleccionado').append(content);
            } else {
                localStorage.setItem("tipoConstructor", "zona")
                var listaRegistros = [];
                var filaId = 0;
                //por proovincias canton distrito y genero
                for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                    var fecha = new Date(fechaInicio);
                    fecha.setMonth(fecha.getMonth());
                    var validaciones = setValidaciones(data.detalleAgrupacionesPorOperador[0]);

                    if (idGnero == 0 || idGnero == undefined || idGnero == null) {
                        content = content + '<th scope="col">' + dateToString(fecha) + '</th>';
                    } else {

                        content = content + '<th colspan="2" class="text-center" scope="col">' + dateToString(fecha) + '</th>';
                    }
                    for (let item of data.detalleAgrupacionesPorOperador) {
                        listaRegistros.push({
                            Id_ConstructorCriterio: item.Id_ConstructorCriterio,
                            Id_Tipo_Valor: item.Id_Tipo_Valor,
                            Numero_Desglose: (i + 1),
                            Valor: '',
                            Anno: fechaInicio.getFullYear(),
                            Valor_Inferior: item.Valor_Inferior,
                            Valor_Superior: item.Valor_Superior
                        });
                    }

                    header.push(dateToString(fecha));
                }
                content += '</thead></tr>';

                //Por provincias
                if (data.provincias != null) {
                    content = content + '</thead></tr>';
                    if (idGnero == 0 || idGnero == undefined || idGnero == null) {

                        //solamente por provincias 
                        var filaId = 0;
                        for (let item of data.provincias) {

                            filaId = filaId + 1;
                            var columnaId = 0;
                            content = content + '<tr><th>' + item.Nombre + '</th>';

                            for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                                columnaId = columnaId + 1
                                var varId = (fechaInicio.getMonth() + 1) + '_' + fechaInicio.getFullYear() + '_' + item.IdProvincia;

                                datosValidar.Id_Tipo_Valor = data.detalleAgrupacionesPorOperador[0].Id_Tipo_Valor
                                datosValidar.Valor_Superior = data.detalleAgrupacionesPorOperador[0].Valor_Superior
                                datosValidar.Valor_Inferior = data.detalleAgrupacionesPorOperador[0].Valor_Inferior

                                //content += `<th><input ${validaciones} onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57"  value="" class="inputProvincia form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"><div class="invalid-feedback hide"></div></th>`
                                content += `<th><textarea ${validaciones}   value="" class="inputProvincia form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"></textarea><div class="invalid-feedback hide"></div></th>`
                            }
                            content = content + '</tr>';
                        }

                        //content = content + '</tbody></table>';
                        //$('#divIndicadorSeleccionado').append(content);
                    } else {

                        //por provincias y genero
                        for (let item of data.provincias) {
                            filaId = filaId + 1;
                            var columnaId = 0;
                            var validaciones = setValidaciones(item);

                            for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                                columnaId = columnaId + 1
                                var varId = (fechaInicio.getMonth() + 1) + '_' + fechaInicio.getFullYear() + '_' + item.IdProvincia;
                                var thInputHombre = `<th><input ${validaciones}  value="" class="inputProvinciaHombre form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"><div class="invalid-feedback hide"></div></th>`
                                var thInputMujer = `<th><input ${validaciones}   value="" class="inputProvinciaMujer form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"><div class="invalid-feedback hide"></div></th>`
                                //var thInput = '<th><input ' + validaciones + ' value="" class="form-control" id="' + varId + '" onblur="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" onkeyup="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" /><div id="' + varId + '_feedback" class="invalid-feedback hide"></div></th>';

                                content += crearFilasTablaPorGenero(item.Nombre, thInputHombre, thInputMujer)
                            }
                        }
                    }

                    //por cantones
                } else if (data.cantones != null) {

                    if (idGnero == 0 || idGnero == undefined || idGnero == null) {
                        //solamente por canton
                        content = content + '</thead></tr>';

                        //data.cantones.splice(81,10)
                        for (let item of data.cantones) {

                            filaId = filaId + 1;
                            var columnaId = 0;
                            var validaciones = setValidaciones(data.detalleAgrupacionesPorOperador[0]);
                            content = content + '<tr><th>' + item.Nombre + '</th>';

                            for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                                columnaId = columnaId + 1
                                var varId = (fechaInicio.getMonth() + 1) + '_' + fechaInicio.getFullYear() + '_' + item.IdCanton;

                                datosValidar.Id_Tipo_Valor = data.detalleAgrupacionesPorOperador[0].Id_Tipo_Valor
                                datosValidar.Valor_Superior = data.detalleAgrupacionesPorOperador[0].Valor_Superior
                                datosValidar.Valor_Inferior = data.detalleAgrupacionesPorOperador[0].Valor_Inferior

                                content += `<th><textarea ${validaciones}  value="" class="inputCanton form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"></textarea><div class="invalid-feedback hide"></div></th>`

                            }
                            content = content + '</tr>';
                        }

                    } else {
                        //por canton y genero
                        for (let item of data.cantones) {
                            filaId = filaId + 1;
                            var columnaId = 0;
                            var validaciones = setValidaciones(data.detalleAgrupacionesPorOperador[0]);

                            for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                                columnaId = columnaId + 1
                                var varId = (fechaInicio.getMonth() + 1) + '_' + fechaInicio.getFullYear() + '_' + item.IdCanton;
                                //var thInput = '<th><input ' + validaciones + ' value="" class="form-control" id="' + varId + '" onblur="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" onkeyup="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" /><div id="' + varId + '_feedback" class="invalid-feedback hide"></div></th>';
                                var thInputHombre = `<th><input ${validaciones}   value="" class="inputCantonHombre form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"><div class="invalid-feedback hide"></div></th>`
                                var thInputMujer = `<th><input ${validaciones}   value="" class="inputCantonMujer form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"><div class="invalid-feedback hide"></div></th>`

                                content += crearFilasTablaPorGenero(item.Nombre, thInputHombre, thInputMujer)
                            }
                        }

                    }

                    //por distritos
                } else if (data.distritos != null) {

                    if (idGnero == 0 || idGnero == undefined || idGnero == null) {
                        //solamente por canton

                        for (let item of data.distritos) {

                            filaId = filaId + 1;
                            var columnaId = 0;
                            var validaciones = setValidaciones(data.detalleAgrupacionesPorOperador[0]);
                            content = content + '<tr><th>' + item.Nombre + '</th>';

                            for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                                columnaId = columnaId + 1
                                var varId = (fechaInicio.getMonth() + 1) + '_' + fechaInicio.getFullYear() + '_' + item.IdDistrito;
                                datosValidar.Id_Tipo_Valor = data.detalleAgrupacionesPorOperador[0].Id_Tipo_Valor
                                datosValidar.Valor_Superior = data.detalleAgrupacionesPorOperador[0].Valor_Superior
                                datosValidar.Valor_Inferior = data.detalleAgrupacionesPorOperador[0].Valor_Inferior

                                content += `<th><textarea ${validaciones}   value="" class="inputDistrito form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"></textarea><div class="invalid-feedback hide"></div></th>`

                                //content = content + '<th><input ' + validaciones + ' value="" class="inputDistrito form-control" id="' + varId + '" onblur="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" onkeyup="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" /><div id="' + varId + '_feedback" class="invalid-feedback hide"></div></th>';
                            }
                            content = content + '</tr>';
                        }

                    } else {
                        //por canton y genero
                        for (let item of data.distritos) {
                            filaId = filaId + 1;
                            var columnaId = 0;
                            var validaciones = setValidaciones(data.detalleAgrupacionesPorOperador[0]);

                            for (var i = 0; i < registro.Cantidad_Meses_Frecuencia; i++) {
                                columnaId = columnaId + 1
                                var varId = (fechaInicio.getMonth() + 1) + '_' + fechaInicio.getFullYear() + '_' + item.IdDistrito;
                                var thInputHombre = `<th><input ${validaciones} value="" class="inputDistritoHombre form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"><div class="invalid-feedback hide"></div></th>`
                                var thInputMujer = `<th><input ${validaciones} value="" class="inputDistritoMujer form-control" id="${varId}" onblur="Validar('${varId}', '${item.Nombre}', '${JSON.stringify(datosValidar).replace(/\"/g, "&quot;")}')"><div class="invalid-feedback hide"></div></th>`

                                //var thInput = '<th><input ' + validaciones + ' value="" class="form-control" id="' + varId + '" onblur="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" onkeyup="Validar(\'' + varId + '\',\'' + item.Nombre_Detalle_Agrupacion + ' (' + header[i] + ')' + '\',' + JSON.stringify(item).replace(/\"/g, "&quot;") + ')" /><div id="' + varId + '_feedback" class="invalid-feedback hide"></div></th>';
                                content += crearFilasTablaPorGenero(item.Nombre, thInputHombre, thInputMujer)
                            }
                    }
                        }
                }
                localStorage.setItem('listaRegistros', JSON.stringify(listaRegistros));
                content = content + '</tbody></table>';
                content += `<input id="idZona" type="text" value="${zona}" hidden>`;
                $('#divIndicadorSeleccionado').append(content);

            }
            rutaindicador = crearRutaAcordeon(IdConstructorCriterio, zona != null ? zona.split('-')[0] : "");

            if (zona != undefined && zona != ' ') {
                if (zona.split("-")[0] == "DISTRITO" && nombreProvincia != undefined) {
                    rutaindicador += nombreProvincia + '/';
                }
            }
            rutaindicador += nombreBoton;
            rutaindicador = nombreIndicador + '/' + rutaindicador

            $("#tituloDetalleIndicadores").html(`Detalle de agrupación: ${rutaindicador}`);

            if (data.listaDetalleRegistroIndicador != null) {
                for (let item of data.listaDetalleRegistroIndicador) {
                    //author: Steven Arroyo Porras
                    if (item.IdProvincia != null) {
                        $(".inputProvincia").each(function () {
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdProvincia == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio) {
                                $(this).val(item.Valor);
                            }

                        })
                        $(".inputCanton").each(function () {
                            id = "' + item2.Id_ConstructorCriterio + '_CANTON_' + provincias.IdProvincia + '"
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdCanton == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio) {
                                $(this).val(item.Valor);
                            }

                        })
                        $(".inputDistrito").each(function () {
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdDistrito == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio) {
                                $(this).val(item.Valor);
                            }

                        })
                        /****************************************** */
                        /****************************************** */
                        $(".inputProvinciaHombre").each(function () {
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdProvincia == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio && item.IdGenero == 1) {
                                $(this).val(item.Valor);
                            }

                        })
                        $(".inputProvinciaMujer").each(function () {
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdProvincia == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio && item.IdGenero == 2) {
                                $(this).val(item.Valor);
                            }

                        })
                        $(".inputCantonHombre").each(function () {
                            id = "' + item2.Id_ConstructorCriterio + '_CANTON_' + provincias.IdProvincia + '"
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdCanton == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio && item.IdGenero == 1) {
                                $(this).val(item.Valor);
                            }

                        })
                        $(".inputCantonMujer").each(function () {
                            id = "' + item2.Id_ConstructorCriterio + '_CANTON_' + provincias.IdProvincia + '"
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdCanton == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio && item.IdGenero == 2) {
                                $(this).val(item.Valor);
                            }

                        })
                        $(".inputDistritoHombre").each(function () {
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdDistrito == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio && item.IdGenero == 1) {
                                $(this).val(item.Valor);
                            }

                        })
                        $(".inputDistritoMujer").each(function () {
                            idInput = $(this).attr("id")
                            arrayId = idInput.split("_")
                            if (item.IdDistrito == parseInt(arrayId[2]) && item.IdConstructorCriterio == IdConstructorCriterio && item.IdGenero == 2) {
                                $(this).val(item.Valor);
                            }

                        })
                        /**/
                    } else {

                        var id = item.IdConstructorCriterio + '_' + item.NumeroDesglose + '_' + item.Anno;


                    }

                    var input = document.getElementById(id);
                    if (input != null) {
                        document.getElementById(id).value = item.Valor;
                    }
                }
            }

            $("#divIndicadorSeleccionadoObservacion").removeClass("hide");
            $('#txtIndicadorSeleccionadoObservacion').val(data.observacion);
            $("#btnGuardadoParcial").prop('disabled', false);
            $("#btnGuardadoTotal").prop('disabled', false);
            $("#btnLimpiar").prop('disabled', false);
            if (solicitudconstructor == undefined) {
                localStorage.setItem("idConstructorSeleccionado", IdSolicitudConstructor)
            } else {
                localStorage.setItem("idConstructorSeleccionado", solicitudconstructor)
            }
            
            $(".darkScreen").fadeOut(100, function () {
                $(this).remove();
            });
        },
        error: function () {
            $('#divIndicadorSeleccionado').eq(0).html();
        }
    });
}

function crearFilasTablaPorGenero(nombre, thInputHombre, thInputMujer) {
    html = `
            <tr>
                <th rowspan="2">${nombre}</th>
                <th>Hombre</th>
                ${thInputHombre}
            </tr>
                <tr>
                <th>Mujer</th>
                ${thInputMujer}
            </tr>
            `
    return html
}
function ActualizarActivo() {
    IdServicio = localStorage.getItem("ServicioId");
    //IdServicioM = localStorage.getItem("ServicioIdMercado");
    //alert(IdServicio);
    $("#" + IdServicio).css("color", "#000000");
    //$("#" + IdServicioM).css("color", "#000000");
}
function ActualizarActivoMercados() {
    //IdServicio = localStorage.getItem("ServicioId");
    IdServicioM = localStorage.getItem("ServicioIdMercado");
    IdServicioM = "idServicio_" + IdServicioM; 
    //alert(IdServicioM);
    //$("#" + IdServicio).css("color", "#000000");
    $("#" + IdServicioM).css("color", "#000000");
}
function Validar(id, name, item) {
    var tipoConstructor = localStorage.getItem("tipoConstructor");
    
    if (tipoConstructor == "zona") {
        item = JSON.parse(item);
    }
    

    var Valor = document.getElementById(id).value
    var errorId = id + '_error'
    var feedbackId = id + '_feedback'
    var errorContent = '';

    $('#' + feedbackId).empty();
    $("#" + id).removeClass("isValid");
    $("#" + id).removeClass("isInvalid");
    $("#" + errorId).remove();

    if (Valor != '') {

        switch (Number(item.Id_Tipo_Valor)) {
            case 2: //Fecha: dd/mm/yyyy
                var Valor_Inferior = new Date(item.Valor_Inferior);
                var Valor_Superior = new Date(item.Valor_Superior);
                if (Valor_Inferior > Valor || Valor_Superior < Valor)
                    errorContent = 'Solamente se aceptan fechas. Entre ' + fullDateToString(Valor_Inferior) + ' a ' + fullDateToString(Valor_Superior);
                break;
            case 3: //Porcentaje: 0
                if (item.Valor_Inferior > Valor || item.Valor_Superior < Valor)
                    errorContent = 'Solamente números decimales. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
                break;
            case 4: //Monto: 0
                if (item.Valor_Inferior > Valor || item.Valor_Superior < Valor)
                    errorContent = 'Solamente números decimales. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
                break;
            case 5: //Cantidad sin decimales: 0
                var isNumber = validarDecimal(Valor);
                //var Valor_Inferior = parseInt(item.Valor_Inferior);
                //var Valor_Superior = parseInt(item.Valor_Superior);
                if (parseInt(item.Valor_Inferior) > Valor || parseInt(item.Valor_Superior) < Valor || !isNumber)
                    errorContent = 'Solamente números. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
                break;
            case 6: //Cantidad con decimales: 0,000000000000

              if (isNaN(Valor)) {
                    errorContent = "Ups... " + Valor + " no es un número.";
                    break; 
                } else {
                    if (Valor % 1 == 0) {
                        //errorContent="Es un numero entero ";
                        //break; 
                    } else {
                        if (parseFloat(item.Valor_Inferior) > Valor || parseFloat(item.Valor_Superior) < Valor)
                            errorContent = 'Solamente números decimales. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
                        break; 
                    
                    }
                }
               
            case 7: //Minutos: 0
                var isNumber = validarDecimal(Valor);
                if (parseInt(item.Valor_Inferior) > Valor || parseInt(item.Valor_Superior) < Valor || !isNumber)
                    errorContent = 'Solamente números. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
                break;
        }

        if (errorContent != '') {
            $("#" + id).addClass("isInvalid");

            //feedback
            var feedback = '<span title="' + errorContent + '">Formato inválido <span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span></span>';
            $('#' + feedbackId).append(feedback);
            $("#" + feedbackId).removeClass("hide");


            var divError = `<div id="${errorId}"><p><strong>${name}:</strong> ${errorContent}</p></div>`;
            $('#divIndicadorSeleccionadoError').append(divError);
            localStorage.setItem("Valida", false);
            //$("#btnGuardadoParcial").prop('disabled', true);
            //$("#btnGuardadoTotal").prop('disabled', true);
        } else {
            $("#" + id).addClass("isValid");
            localStorage.setItem("Valida", true);
            //$("#btnGuardadoParcial").prop('disabled', false);
            //$("#btnGuardadoTotal").prop('disabled', false);
        }
    }
};
function esEntero(numero) {
    if (isNaN(numero)) {
        alert("Ups... " + numero + " no es un número.");
        return false;
    } else {
        if (numero % 1 == 0) {
            alert("Es un numero entero");
            return true;
        } else {
            alert("Es un numero decimal");
            return false;
        }
    }
}
function validarDecimal(valor) {
    if (Number.isInteger(Number(valor)))
        return true;
    else
        return false;
}

function setValidaciones(item) {
    var validaciones = '';
    var tooltip = '';

    switch (Number(item.Id_Tipo_Valor)) {
        case 1: // Texto: String
            validaciones = validaciones + 'type="text" ';
            tooltip = tooltip + 'Inserte un valor texto.';
            break;
        case 2: //Fecha: dd/mm/yyyy
            Valor_Inferior = new Date(item.Valor_Inferior);
            Valor_Superior = new Date(item.Valor_Superior);
            validaciones = validaciones + 'type="date" min="' + Valor_Inferior + '" max="' + Valor_Superior + '" ';
            tooltip = tooltip + 'Inserte una fecha. Formato: dd/mm/yyyy Entre ' + fullDateToString(Valor_Inferior) + ' a ' + fullDateToString(Valor_Superior);
            break;
        case 3: //Porcentaje: 0
            validaciones = validaciones + ' onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" type="number" min="' + item.Valor_Inferior + '" max="' + item.Valor_Superior + '" ';
            tooltip = tooltip + 'Inserte un valor númerico decimal. Solamente números. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
            break;
        case 4: //Monto: 0
            validaciones = validaciones + 'onkeydown="return event.keyCode !== 69 && event.keyCode !== 189 && event.keyCode !== 187"  type="number" step="0.01" min="' + item.Valor_Inferior + '" max="' + item.Valor_Superior + '" ';
            tooltip = tooltip + 'Inserte un valor númerico decimal. Solamente números. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
            break;
        case 5: //Cantidad sin decimales: 0
            validaciones = validaciones + ' onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" type="number" min="' + item.Valor_Inferior + '" max="' + item.Valor_Superior + '" ';
            tooltip = tooltip + 'Inserte un valor númerico. Solamente números. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
            break;
        case 6: //Cantidad con decimales: 0,000000000000
            validaciones = validaciones + ' onkeypress="return SoloNumeros(event)"  step="0.01" min=" ' + item.Valor_Inferior + '" max="' + item.Valor_Superior + '" ';
            tooltip = tooltip + 'Inserte un valor númerico decimal. Solamente números. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
            break;
        case 7: //Minutos: 0
            validaciones = validaciones + 'onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" type="number" min="' + item.Valor_Inferior + '" max="' + item.Valor_Superior + '" ';
            tooltip = tooltip + 'Inserte un valor númerico. Solamente números. Entre ' + item.Valor_Inferior + ' a ' + item.Valor_Superior;
            break;
    }

    validaciones = validaciones + 'title="' + tooltip + '"';
    return validaciones;
}
function SoloNumeros(e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    console.log(e.which || e.keyCode);

    if ((keynum == 8) || (keynum == 46) || (keynum == 32))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
}
function dateToString(fechaInicio) {
    let yy = new Intl.DateTimeFormat('es', { year: '2-digit' }).format(fechaInicio);
    let mm = new Intl.DateTimeFormat('es', { month: 'short' }).format(fechaInicio);
    //let dd = new Intl.DateTimeFormat('es', { day: '2-digit' }).format(fechaInicio);
    mm = mm.charAt(0).toUpperCase() + mm.slice(1);
    var result = `${mm}-${yy}`;
    return result;
}

function fullDateToString(fechaInicio) {
    let yy = new Intl.DateTimeFormat('es', { year: '2-digit' }).format(fechaInicio);
    let mm = new Intl.DateTimeFormat('es', { month: 'short' }).format(fechaInicio);
    let dd = new Intl.DateTimeFormat('es', { day: '2-digit' }).format(fechaInicio);
    mm = mm.charAt(0).toUpperCase() + mm.slice(1);
    var result = `${dd}-${mm}-${yy}`;
    return result;
}

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

$(function () {
    $(".accordion-titulo").click(function (e) {
        alert("jose si entre (jjojojjjojjojojo)");
        e.preventDefault();
        
        var contenido = $(this).next(".accordion-content");

        if (contenido.css("display") == "none") { //open		
            contenido.slideDown(250);
            $(this).addClass("open");
        }
        else { //close		
            contenido.slideUp(250);
            $(this).removeClass("open");
        }

    });
});
//Cambio de Copiar y pegar

$(document).on('paste', '#indicadores tbody  tr th textarea', null, function (e) {

    $txt = $(this);
    setTimeout(function () {

        var array = $txt.val().split(/\n/);
        var values = [];
        var RowIndex = $txt.parent().parent().index();
        var ColIndex = $txt.parent().index();
        for (let i = 0; i < array.length; i++) {

            var value = array[i].split(/\t/);

            values.push(value)
        }

        for (let i = 0; i < values.length; i++) {
            let value = values[i];
            for (let j = 0; j < value.length; j++) {
                let h = value[j]
                var inp = $('#indicadores tbody tr').eq(i + RowIndex).find('th').eq(j + ColIndex).find('textarea');
                inp.val(h.replace(' ', ''));
            }

        }
    }, 0);
});

$("#formEditarFormularioWeb").submit(function (e) {
    var idsolicitud = $('#ItemEditar').val();
    var Operador = $("#ddlOperadorSolicitud").val() == null ? "" : $("#ddlOperadorSolicitud").val();
    var FormularioWeb = document.getElementById('radioFormularioWeb').checked;
    var EditarOperadores = document.getElementById('checkEditarOperadores').checked;
    var json = {
        "ItemEditar": idsolicitud,
        "Operadores": Operador,
        "FormularioWeb": FormularioWeb,
        "EditarOperadores": EditarOperadores
    };

    var options = {
        type: "post",
        dataType: "json",
        url: "/Solicitud/EditarFormularioWeb",
        data: { "data": json },
        success: function (data) {
            if (data.ok === "True") {

                $("#modalEditarFormularioWeb").modal('hide');

                Swal.fire(
                    '',
                    data.strMensaje,
                    'success'
                )
                $("#frmFiltrarSolicitudes")[0].reset();
                $("#frmFiltrarSolicitudes").submit();
            } else {

                $("#divMensajeErrorEditarFormularioWeb").removeClass("hidden");
                $("#divMensajeErrorEditarFormularioWeb").removeAttr('style');
                $("#errorMensajeEditarFormularioWeb").html(data.strMensaje);


            }
        },
        error: function (error) {
            // window.location = "/Solicitud?em=true";
        }
    };
    $.ajax(options);

});


