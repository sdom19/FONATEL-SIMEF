//#region Definicion Indicadores

//Función para buscar indicadores filtrados
function buscarIndicadores() {


    //Cargar loader
    $('#loader').show();
    $("#DivTable").hide();
    //Obtener filtros
    var direccionSelect = $('#direccionSelect').val();
    var servicioSelect = $('#servicioSelect').val();
    var nombreDireccion = $("#direccionSelect option:selected").text();
    nombreDireccion = nombreDireccion.substring(0, nombreDireccion.indexOf("("));
    //Consultar indicadores
    $.ajax({
        type: "get",
        url: "/Mantenimiento/GetIndicadores?direccion=" + direccionSelect + "&servicio=" + servicioSelect,
        contentType: "html",
        success: function (result) {
            //Limpiar tabla
            $('#IndicadoresTable').DataTable().destroy();
            $("#IndicadoresTable").empty();
            //Crear HTML de tabla
            var html = "<thead class='azul-sutel'> <tr> <th scope='col'>Id</th> <th  style='width:65%;' scope='col'>Nombre</th> <th  scope='col'>Tipo Indicador</th> <th scope='col'>Dirección</th> <th scope='col'>Acciones</th> </tr></thead>";
            result.forEach(function (item) {
                html += '<tr><th scope="row">' + item.IdIndicador + '</th> <td>' + item.NombreIndicador + '</td><td>' + item.DesTipoInd + '</td><td>' + nombreDireccion + '</td><td><i class="fa fa-pencil text-sutel" data-toggle="modal" data-target=".bd-example-modal-lg" aria-hidden="true" onclick = \"clickButton( \'' + item.IdIndicador + '\');\" ></i ></td ></tr>';
            });
            $("#IndicadoresTable").append(html);
            //Usar Datatable
            $('#IndicadoresTable').DataTable({
                dom: "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-12'p>>",
                responsive: true,
                'columnDefs': [{
                    "targets": [0, 3, 4],
                    "className": "text-center",
                },
                {
                    "targets": [3, 4],
                    "orderable": false,
                }],
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                }
            });
            //Quitar loader
            $('#loader').hide();
            $("#DivTable").show();
            $(".sorting").css("background", "none");
            $(".sorting_asc").css("background", "none");
        }
    });
}

//Función para guardar indicador
function guardarIndicador() {
    //Obtener valores
    var id = $('#codigoInput').val();
    var definicionInput = $('#definicionInput').val();
    var fuenteInput = $('#fuenteInput').val();
    var notaInput = $('#notaInput').val();
    //Validacíón vacios
    if (definicionInput == "" || fuenteInput == "") {
        $('#error').html('<div class="alert alert-danger" role="alert">No puede dejar en blanco la definción ni la fuente</div>');
    }
    else {
        //Validación tamaño
        if (definicionInput.length > 999 || fuenteInput.length > 999 || notaInput.length > 999)
            Swal.fire('Excedió el límite de caracteres', 'No puede pasar de 1000 caracteres', 'error');
        else {
            //Guardar indicador
            var urlNew = "/Mantenimiento/SaveIndicador?id=" + id + "&definicion=" + definicionInput + "&fuente=" + fuenteInput + "&nota=" + notaInput;
            $.ajax({
                type: "get",
                url: urlNew,
                contentType: "html",
                success: function (result) {
                    if (result) {
                        //Aviso positivo
                        Swal.fire('El indicador se guardó correctamente', '', 'success');
                        $('#ModalDefinicion').modal('toggle');
                    }
                    else 
                        Swal.fire('El indicador no pudo ser guardado', result, 'error');
                },
                error: function (result) {
                    Swal.fire('El indicador no se guardó correctamente', 'Error interno, contactar a soporte(500)', 'error');
                }
            });
        }
    }
}

//Función para cargar indicador en modal
function clickButton(id) {
    //Traer indicador por id
    $.ajax({
        type: "get",
        url: "/Mantenimiento/GetIndicador?id=" + id,
        contentType: "html",
        success: function (result) {
            //Cargar atributos
            $('#codigoInput').val(result.IdIndicador);
            $('#nombreInput').val(result.NombreIndicador);
            var nombreDireccion = $("#direccionSelect option:selected").text();
            nombreDireccion = nombreDireccion.substring(0, nombreDireccion.indexOf("("));
            $('#direccionInput').val(nombreDireccion);
            var servicioDireccion = $("#servicioSelect option:selected").text();
            servicioDireccion = servicioDireccion.substring(0, servicioDireccion.indexOf("("));
            $('#servicioInput').val(servicioDireccion);
            $('#definicionInput').val(result.Definicion);
            $('#fuenteInput').val(result.Fuente);
            $('#notaInput').val(result.Nota);
        }
    });
}

//Función para filtrar tabla por id
function filtroId(filtro) {
    //Filtrar tabla
    var table = $('#IndicadoresTable').DataTable();
    table.column(0).search(filtro).draw();
}

//Función para filtrar tabla por nombre
function filtroNombre(filtro) {
    //Filtrar tabla
    var table = $('#IndicadoresTable').DataTable();
    table.column(1).search(filtro).draw();
}

//Función para filtrar tabla por indicador
function filtroIndicador(filtro) {
    //Filtrar tabla
    var table = $('#IndicadoresTable').DataTable();
    table.column(2).search(filtro).draw();
}
//#endregion

//#region Parametros Indicadores

//Sets para parametros
var parametros = {};
var ids = new Set();
var idsIncompletos = new Set();
var idsSemiIncompletos = new Set();

//Función para obtener parametros
function buscarIndicadoresParametros() {
    //Inicialización
    parametros = {};
    ids = new Set();
    idsIncompletos = new Set();
    //Mostrar loader
    $('#loader').show();
    $("#DivTable").hide();
    //Obtener filtros
    var direccionSelect = $('#direccionSelect').val();
    var servicioSelect = $('#servicioSelect').val();
    //Consultar parametros filtrados
    $.ajax({
        type: "get",
        url: "/Mantenimiento/GetParametrosIndicadores?direccion=" + direccionSelect + "&servicio=" + servicioSelect,
        contentType: "html",
        success: function (result) {
            //Limpiar tabla
            $('#IndicadoresTable').DataTable().destroy();
            $("#IndicadoresTable").empty();
            //Crear HTML de tabla
            var head = "<thead class='azul-sutel'> <tr> <th> Id </th><th  style='vertical-align:middle; width:18%;'>Indicador</th><th style='vertical-align:middle;'>Visualiza</th> <th>  Visualizar desde </th> <th>  Visualizar por operador hasta </th> <th>  Visualizar a nivel total hasta</th> </tr> </thead>";
            $("#IndicadoresTable").append(head);
            result.forEach(function (item) {
                //Agregar filas por cada parametro
                var newRow = $("<tr>");
                var cols = '<th>' + item.IdIndicador + '</th>'
                cols += '<th style = "vertical-align:middle;" scope="row">' + item.DescripcionIndicador + '</th>';
                cols += '<td style = "vertical-align:middle;"> <input onChange="cambiarParametro(\'' + item.IdIndicador + '\')" id="Visualiza' + item.IdIndicador + '" type="checkbox">' + '</td>';
                cols += '<td> <div class="col-sm-6"> <select class="form-control" onChange="cambiarParametro(\'' + item.IdIndicador + '\')" id="selectMesDesde' + item.IdIndicador + '"><option value="">Mes</option><option value="1">Enero</option><option value="2">Febrero</option><option value="3">Marzo</option><option value="4">Abril</option><option value="5">Mayo</option><option value="6">Junio</option><option value="7">Julio</option><option value="8">Agosto</option><option value="9">Setiembre</option><option value="10">Octubre</option><option value="11">Noviembre</option><option value="12">Diciembre</option></select> </div><div class="col-sm-6"> <select class="form-control" onChange="cambiarParametro(\'' + item.IdIndicador + '\')" id="selectAnnoDesde' + item.IdIndicador + '"><option value="">Año</option><option value="2009">2009</option><option value="2010">2010</option><option value="2011">2011</option><option value="2012">2012</option><option value="2013">2013</option><option value="2014">2014</option><option value="2015">2015</option><option value="2016">2016</option><option value="2017">2017</option><option value="2018">2018</option><option value="2019">2019</option><option value="2020">2020</option><option value="2021">2021</option><option value="2022">2022</option><option value="2023">2023</option><option value="2024">2024</option><option value="2025">2025</option><option value="2026">2026</option><option value="2027">2027</option><option value="2028">2028</option><option value="2029">2029</option><option value="2030">2030</option><option value="2031">2031</option><option value="2032">2032</option><option value="2033">2033</option><option value="2034">2034</option><option value="2035">2035</option></select> </div>' + '</td>';
                cols += '<td> <div class="col-sm-6"> <select class="form-control" onChange="cambiarParametro(\'' + item.IdIndicador + '\')" id="selectMesOperador' + item.IdIndicador + '"><option value="">Mes</option><option value="1">Enero</option><option value="2">Febrero</option><option value="3">Marzo</option><option value="4">Abril</option><option value="5">Mayo</option><option value="6">Junio</option><option value="7">Julio</option><option value="8">Agosto</option><option value="9">Setiembre</option><option value="10">Octubre</option><option value="11">Noviembre</option><option value="12">Diciembre</option></select> </div><div class="col-sm-6"> <select class="form-control" onChange="cambiarParametro(\'' + item.IdIndicador + '\')" id="selectAnnoOperador' + item.IdIndicador + '"><option value="">Año</option><option value="2009">2009</option><option value="2010">2010</option><option value="2011">2011</option><option value="2012">2012</option><option value="2013">2013</option><option value="2014">2014</option><option value="2015">2015</option><option value="2016">2016</option><option value="2017">2017</option><option value="2018">2018</option><option value="2019">2019</option><option value="2020">2020</option><option value="2021">2021</option><option value="2022">2022</option><option value="2023">2023</option><option value="2024">2024</option><option value="2025">2025</option><option value="2026">2026</option><option value="2027">2027</option><option value="2028">2028</option><option value="2029">2029</option><option value="2030">2030</option><option value="2031">2031</option><option value="2032">2032</option><option value="2033">2033</option><option value="2034">2034</option><option value="2035">2035</option></select> </div>' + '</td>';
                cols += '<td> <div class="col-sm-6"> <select class="form-control" onChange="cambiarParametro(\'' + item.IdIndicador + '\')" id="selectMesTotal' + item.IdIndicador + '"><option value="">Mes</option><option value="1">Enero</option><option value="2">Febrero</option><option value="3">Marzo</option><option value="4">Abril</option><option value="5">Mayo</option><option value="6">Junio</option><option value="7">Julio</option><option value="8">Agosto</option><option value="9">Setiembre</option><option value="10">Octubre</option><option value="11">Noviembre</option><option value="12">Diciembre</option></select> </div><div class="col-sm-6"> <select class="form-control" onChange="cambiarParametro(\'' + item.IdIndicador + '\')" id="selectAnnoTotal' + item.IdIndicador + '"><option value="">Año</option><option value="2009">2009</option><option value="2010">2010</option><option value="2011">2011</option><option value="2012">2012</option><option value="2013">2013</option><option value="2014">2014</option><option value="2015">2015</option><option value="2016">2016</option><option value="2017">2017</option><option value="2018">2018</option><option value="2019">2019</option><option value="2020">2020</option><option value="2021">2021</option><option value="2022">2022</option><option value="2023">2023</option><option value="2024">2024</option><option value="2025">2025</option><option value="2026">2026</option><option value="2027">2027</option><option value="2028">2028</option><option value="2029">2029</option><option value="2030">2030</option><option value="2031">2031</option><option value="2032">2032</option><option value="2033">2033</option><option value="2034">2034</option><option value="2035">2035</option></select> </div>' + '</td>'; newRow.append(cols);
                $("#IndicadoresTable").append(newRow);
                //Poner valores seleccionados del parametro
                document.getElementById("Visualiza" + item.IdIndicador).checked = item.Visualiza;
                document.getElementById("selectMesDesde" + item.IdIndicador).value = item.MesDesde;
                document.getElementById("selectAnnoDesde" + item.IdIndicador).value = item.AnnoDesde;
                document.getElementById("selectMesOperador" + item.IdIndicador).value = item.MesPorOperador;
                document.getElementById("selectAnnoOperador" + item.IdIndicador).value = item.AnnoPorOperador;
                document.getElementById("selectMesTotal" + item.IdIndicador).value = item.MesPorTotal;
                document.getElementById("selectAnnoTotal" + item.IdIndicador).value = item.AnnoPorTotal;
                parametros[item.IdIndicador] = item;
            });
            //Usar Datatable
            $('#IndicadoresTable').DataTable({
                responsive: true,
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                },
                "columnDefs": [{
                    "targets": [2, 3, 4],
                    "orderable": false,
                    "className": "text-center"
                }]
            });
            //Mostrar componentes
            $('#loader').hide();
            $("#DivTable").show();
            $(".sorting").css("background", "none");
            $(".sorting_asc").css("background", "none");
        }
    });
}

//Función para cambiar parametro visualmente
function cambiarParametro(id) { 
    //Obtener valores
    var parametro = parametros[id];
    parametro.Visualiza = document.getElementById("Visualiza" + id).checked;
    parametro.MesDesde = document.getElementById("selectMesDesde" + id).value;
    parametro.AnnoDesde = document.getElementById("selectAnnoDesde" + id).value;
    parametro.MesPorOperador = document.getElementById("selectMesOperador" + id).value;
    parametro.AnnoPorOperador = document.getElementById("selectAnnoOperador" + id).value;
    parametro.MesPorTotal = document.getElementById("selectMesTotal" + id).value;
    parametro.AnnoPorTotal = document.getElementById("selectAnnoTotal" + id).value;

    //Seleccione primer Mes y Año
    document.getElementById("selectMesDesde" + id).value = parametro.MesDesde;
    document.getElementById("selectAnnoDesde" + id).value = parametro.AnnoDesde;

    //Validaciones fechas
   if (parametro.AnnoPorOperador == "2010") {
        document.getElementById("selectMesOperador" + id).innerHTML = '<option value="">Mes</option><option value="10">Octubre</option><option value="11">Noviembre</option><option value="12">Diciembre</option>';
        if (parseInt(parametro.MesPorOperador) < 10)
            document.getElementById("selectMesOperador" + id).selectedIndex = "0";
        else
            document.getElementById("selectMesOperador" + id).value = parametro.MesPorOperador;
    }
    else {
        document.getElementById("selectMesOperador" + id).innerHTML = '<option value="">Mes</option><option value="1">Enero</option><option value="2">Febrero</option><option value="3">Marzo</option><option value="4">Abril</option><option value="5">Mayo</option><option value="6">Junio</option><option value="7">Julio</option><option value="8">Agosto</option><option value="9">Setiembre</option><option value="10">Octubre</option><option value="11">Noviembre</option><option value="12">Diciembre</option>';
        document.getElementById("selectMesOperador" + id).value = parametro.MesPorOperador;
    }

    if (!(!parametro.Visualiza && parametro.AnnoDesde == "" && parametro.MesDesde == "" && parametro.AnnoPorOperador == "" && parametro.MesPorOperador == "" && parametro.AnnoPorTotal == "" && parametro.MesPorTotal == "")) {
        if ((parametro.MesDesde != "" && parametro.AnnoDesde != "") && (parametro.MesPorOperador != "" && parametro.AnnoPorOperador != "") && (parametro.MesPorTotal != "" && parametro.AnnoPorTotal != "")) {
            if ((parseInt(parametro.AnnoDesde) > parseInt(parametro.AnnoPorOperador)) ||
                (parseInt(parametro.AnnoDesde) > parseInt(parametro.AnnoPorTotal)) ||
                (parseInt(parametro.AnnoPorTotal) < parseInt(parametro.AnnoPorOperador)) ||
                (parseInt(parametro.AnnoPorTotal) == parseInt(parametro.AnnoPorOperador) && parseInt(parametro.MesPorTotal) < parseInt(parametro.MesPorOperador)) ||
                (parseInt(parametro.AnnoDesde) == parseInt(parametro.AnnoPorOperador) && parseInt(parametro.MesDesde) > parseInt(parametro.MesPorOperador)) ||
                (parseInt(parametro.AnnoDesde) == parseInt(parametro.AnnoPorTotal) && parseInt(parametro.MesDesde) > parseInt(parametro.MesPorTotal))) {
                idsIncompletos.add(id);
                Swal.fire("Rango de Fechas Incorrecto");
            }
            else {
                ids.add(id);
                idsIncompletos.delete(id);
                idsSemiIncompletos.delete(id);
            }
        }
        else if ((parametro.MesDesde == "" && parametro.AnnoDesde != "") || (parametro.MesPorOperador == "" && parametro.AnnoPorOperador != "") || (parametro.AnnoPorTotal == "" && parametro.AnnoPorTotal != "")) {
            idsIncompletos.add(id);
        }
        else if ((parametro.MesDesde != "" && parametro.AnnoDesde == "") || (parametro.MesPorOperador != "" && parametro.AnnoPorOperador == "") || (parametro.AnnoPorTotal != "" && parametro.AnnoPorTotal == "")) {
            idsIncompletos.add(id);
        }
        else if ((parametro.MesDesde == "" && parametro.AnnoDesde == "") && (parametro.MesPorOperador != "" && parametro.AnnoPorOperador != "") && (parametro.MesPorTotal != "" && parametro.AnnoPorTotal != "")) {
            idsSemiIncompletos.add(id);
            idsIncompletos.delete(id);
        }
        else if ((parametro.MesDesde != "" && parametro.AnnoDesde != "") && (parametro.MesPorOperador == "" && parametro.AnnoPorOperador == "") && (parametro.MesPorTotal != "" && parametro.AnnoPorTotal != "")) {
            idsSemiIncompletos.add(id);
            idsIncompletos.delete(id);
        }
        else if ((parametro.MesDesde != "" && parametro.AnnoDesde != "") && (parametro.MesPorOperador != "" && parametro.AnnoPorOperador != "") && (parametro.MesPorTotal == "" && parametro.AnnoPorTotal == "")) {
            idsSemiIncompletos.add(id);
            idsIncompletos.delete(id);
        }
        else {
            idsIncompletos.add(id);
        }
    } else {
        ids.delete(id);
        idsIncompletos.delete(id);
        idsSemiIncompletos.delete(id);
    }
}

//Función para guardar parámetro
function guardarParametroIndicador() {
    //Validar incompletos
    var cantidad = idsIncompletos.size;
    switch (cantidad) {
        case 0:
            //Sin incompletos
            Swal.fire({
                title: '¿Está seguro de guardar los cambios de ' + ids.size + ' indicador(es) y ' + idsSemiIncompletos.size + ' indicador(es) incompletos',
                text: "",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Guardar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.value) {
                    var correcto = true;
                    idsSemiIncompletos.forEach(index => {
                        ids.add(index);
                    });
                    //Agregar parametros
                    ids.forEach(index => {
                        $.ajax({
                            data: parametros[index],
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            url: "/Mantenimiento/SaveParametroIndicador",
                            contentType: "html",
                            success: function (result) {
                                correcto = correcto && result;
                            },
                            error: function (result) {
                                Swal.fire('Error interno', 'Contactar a soporte(500)', 'error');
                            }
                        });
                    });
                    if (correcto) {
                        //Resultado de operación
                        Swal.fire({
                            title: 'Los indicadores se guardaron correctamente',
                            text: "",
                            type: 'success',
                        }).then((result) => {
                            if (result.value) {
                                window.location.href = "/Mantenimiento/ParametrosIndicadores";
                            }
                        })
                    }
                    else {
                        Swal.fire("Los indicadores no puedieron ser guardados", "Solicitud de actulización de parámetro rechazada", "error");

                    }
                }
            })
            break;
        //Avisar errores
        case 1:
            Swal.fire("Hay " + cantidad + " indicador con parametros errores", Array.from(idsIncompletos).join(', '), "warning");
            break;
        default:
            Swal.fire("Hay " + cantidad + " indicadores con parametros errores", Array.from(idsIncompletos).join(', '), "warning");
    }
}
//#endregion

//#region Reportes

//Función para ver reportes
function verReporte() {
    //Mostrar loader
    $('#loader').show();
    //Obtener filtros
    var direccionSelect = $('#direccionSelect').val();
    var servicioSelect = $('#servicioSelect').val();
    var desde = $('#desde').val();
    var hasta = $('#hasta').val();
    var nombreServicio = $("#servicioSelect option:selected").text();
    nombreServicio = nombreServicio.substring(0, nombreServicio.indexOf("(") - 1);
    //Obtener reportes filtrados
    $.ajax({
        type: "get",
        url: "/Mantenimiento/GetBitacoraParametrosIndicadores?direccion=" + direccionSelect + "&servicio=" + servicioSelect + "&desde=" + desde + "&hasta=" + hasta,
        contentType: "html",
        success: function (result) {
            //Limpiar tabla
            $('#ReporteTable').DataTable().destroy();
            $("#ReporteTable").empty();
            //Crear HTML de tabla
            var html = '<thead class="azul-sutel"> <tr> <th>Usuario</th> <th style="max-width:7%;">Fecha de publicación</th> <th>Servicio</th> <th style="max-width:30%;">Indicador</th> <th style="text-align:center;">Visualizar por operador hasta</th> <th style="text-align:center;">Visualizar por Total hasta</th> </tr></thead>';
            result.forEach(function (item) {

                html += '<tr><th scope="row">' + item.UsuarioPublicador + '</th> <td>' + moment(item.FechaPublicacion).format("DD-MM-YYYY") + " " + moment(item.HoraPublicacion).format('HH:mm:ss') + '</td><td>' + nombreServicio + '</td><td>' + item.IdIndicador + " - " + item.NombreIndicador + '</td><td>' + mes(item.MesPorOperador) + ' ' + item.AnnoPorOperador + '</td><td>' + mes(item.MesPorTotal) + ' ' + item.AnnoPorTotal + '</td> </tr>';
            });
            $("#ReporteTable").append(html);
            //Usar Datatable
            $('#ReporteTable').DataTable({
                paging: true,
                dom: "<'row'<'col-sm-1'B><'col-sm-5'l><'col-sm-6'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
                fixedHeader: {
                    header: true
                },
                buttons: {
                    buttons: [{
                        extend: 'print',
                        text: '<i class="fa fa-print"></i> Imprimir',
                        title: $('h1').text()
                    }],
                    dom: {
                        container: {
                            className: 'dt-buttons'
                        },
                        button: {
                            className: 'btn btn-default'
                        }
                    }
                },
                responsive: true,
                "pagingType": "full_numbers",
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                },
                "columnDefs": [{
                    "targets": [4, 5],
                    "className": "text-center"
                },
                {
                    "targets": [2],
                    "orderable": false,
                    "className": "text-center"
                }]
            });
            //Mostrar componentes
            $("#tabla-reporte").show();
            $('#loader').hide();
            $(".sorting").css("background", "none");
            $(".sorting_asc").css("background", "none");
        }
    });
}

//Función para nombre de mes
function mes(numero) {
    switch (numero) {
        case 1:
            return "Enero";
        case 2:
            return "Febrero";
        case 3:
            return "Marzo";
        case 4:
            return "Abril";
        case 5:
            return "Mayo";
        case 6:
            return "Junio";
        case 7:
            return "Julio";
        case 8:
            return "Agosto";
        case 9:
            return "Setiembre";
        case 10:
            return "Octubre";
        case 11:
            return "Noviembre";
        case 12:
            return "Diciembre";
    }
}

//#endregion

//Función para obtener servicios filtrados
function getServicios(IdDireccion) {
    //Obtener servicios
    $("#Search").attr("disabled", true);
    $.getJSON("/Mantenimiento/GetServicios?IdDireccion=" + IdDireccion, function (data) {
        $('#servicioSelect').empty();
        $.each(data, function (key, val) {
            //Añadir opciones para cada servicio
            $('#servicioSelect').append($('<option>', {
                value: val.IdServicio,
                text: val.DesServicio + " (" + val.Cantidad + ")"
            }));
        });
        $("#Search").attr("disabled", false);
    });
}

//Función para redirigir a inicio
function inicio() {
    window.location.href = "/"
}

//Función para recargar página de parametros
function recargarParametros() {
    window.location.href = "/Mantenimiento/ParametrosIndicadores"
}
