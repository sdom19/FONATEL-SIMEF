jsUtilidades= {
    "Variables": {
        "urlOrigen": location.origin,

        "TipoDetalleCategoria": {
            "Numerico":1,
            "Alfanumerico":2,
            "Texto":3,
            "Fecha":4
        },
        "Error": {
            "NoError": 0,
            "ErrorSistema": 1,
            "ErrorControlado": 2
        },

        "Acciones": {
            "Insertar": 1,
            "Consultar": 2,
            "Editar": 3,      
            "Eliminar": 4,
            "Clonar": 5,
            "Visualizar": 9
        },
        "EstadoRegistros": {
            "EnProceso": 1,
            "Activo" : 2,
            "Desactivado":3,
            "Eliminado": 4
        },
        "TipoReglasDetalle":
        {
            'NoRegistrado': 0,
            'FormulaCambioMensual': 1,
            'FormulaContraOtroIndicadorEntrada': 2,
            'FormulaContraConstante': 3,
            'FormulaContraAtributosValidos': 4,
            'FormulaActualizacionSecuencial': 5,
            'FormulaContraOtroIndicadorSalida': 6,
            'FormulaContraOtroIndicadorEntradaSalida': 7
        }
    }
}

$(document).ready(function () {

    $(".datatable_simef_modal").DataTable({
        'scrollY': '300px',
        'scrollCollapse': true,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "searchPlaceholder": "",
            "zeroRecords": "Sin resultados encontrados",

            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
       
    });

    CargarDatasource();
   

    $('.listasDesplegables').select2({
        placeholder: "Seleccione",
        width: 'resolve' 
    });


    $('.listasDesplegables_todos').select2({
        placeholder: "Todos",
        width: 'resolve'
    });


    $('.nav-tabs > li a[title]').tooltip();
});

$(document).on("select2:select", '.multiple-Select', function (e) {
    var data = e.params?.data?.text;
    if (data == 'Todos') {
        $(".multiple-Select > option").prop("selected", "selected");
        $(".multiple-Select").trigger("change");
    }
});

$(document).on("select2:unselect", '.multiple-Select', function (e) {
    RemoverOpcionesSelect2Multiple(e.params.data.text);
});

$('a[data-toggle="tab"]').on('show.bs.tab', function (e) {


    console.log($(this).val());

    var $target = $(e.target);

    if ($target.parent().hasClass('disabled')) {
        return false;
    }
});

$(document).on("keypress",'.solo_numeros', function (e) {
    var regex = new RegExp("^[0-9]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
});



$(document).on("paste", 'input', function (e) {
    e.preventDefault();
});


function EliminarDatasource(pDataTable = ".datatable_simef") {
    $(pDataTable).DataTable().destroy();
}

function CargarDatasourceV2 (table) {
  let t=  $(table).DataTable({
        pageLength: 5,
        lengthMenu: [[5,25, 50, 100], [5,25, 50, 100]],
        "dom": '<"top-position"<"subtop"Bl>f>r<"content-table"t><"bottom-position"ip><"clear">',
        buttons: [
            {
                extend: 'excel',
                text: '<i class="fa fa-file-excel-o" style="color:green;"></i>',
                titleAttr: 'Excel',
                autoPrint: false,
                exportOptions: {
                    columns: ':not(.noExport)'
                },
            },
            {
                extend: 'print',
                text: '<i class="fa fa-print" style="color:black;"></i>',
                titleAttr: 'Imprimir',
                autoPrint: false,
                exportOptions: {
                    columns: ':not(.noExport)'
                },

            },

        ],
        columnDefs: [
            {
                searchable: false,
                orderable: false,
                targets: 0,
            },
            { "className": "dt-center", "targets": "_all" }
        ],
        scrollY: 350,
        scrollX: true,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "searchPlaceholder": "",
            "zeroRecords": "Sin resultados encontrados",

            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
  });


    t.on('order.dt search.dt', function () {
        let i = 1;

        t.cells(null, 0, { search: 'applied', order: 'applied' }).every(function (cell) {
            this.data(i++);
        });
    }).draw();

};

function CargarDatasource(pDataTable = ".datatable_simef") {
   
      $(pDataTable).DataTable({
        pageLength: 5,
        lengthMenu: [[5, 25, 50, 100], [5, 25, 50, 100]],
        "dom": '<"top-position"<"subtop"Bl>f>r<"content-table"t><"bottom-position"ip><"clear">',
        buttons: [
            {
                extend: 'excel',
                text: '<i class="fa fa-file-excel-o" style="color:green;"></i>',
                titleAttr: 'Excel',
                autoPrint: false,
                exportOptions: {
                    columns: ':not(.noExport)'
                },
            },
            {
                extend: 'pdf',
                text: '<i class="fa fa-file-pdf-o" style="color:brown;"></i>',
                titleAttr: 'PDF',
                autoPrint: false,
                exportOptions: {
                    columns: ':not(.noExport)'
                },
            },
            {
                extend: 'print',
                text: '<i class="fa fa-print" style="color:black;"></i>',
                titleAttr: 'Imprimir',
                autoPrint: false,
                exportOptions: {
                    columns: ':not(.noExport)'
                },

            },

        ],

        columnDefs: [
            { "className": "dt-center", "targets": "_all" }
        ],
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "searchPlaceholder": "",
            "zeroRecords": "Sin resultados encontrados",

            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        initComplete: function () {
            this.api()
                .columns()
                .every(function () {
                    var column = this;

                    if ($(column.footer()).hasClass("select2-wrapper")) {
                        var select = $('<select><option value="">Todos</option></select>')
                            .appendTo($(column.footer()).empty())
                            .on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex($(this).val());
                                column.search(val ? '^' + val + '$' : '', true, false).draw();
                            });

                        column
                            .data()
                            .unique()
                            .sort()
                            .each(function (d, j) {
                                select.append('<option value="' + d + '">' + d + '</option>');
                            });
                    }
                });
        },

        
    });

    $('.table-wrapper-fonatel table tfoot th select').select2({
        width: 'resolve'
    });

    $('.datatable_simef > tbody tr td button').tooltip();
}

/**
 * Permite remover un item de una tabla DataTable.
 * @param {any} pDataTable tabla DataTable.
 * @param {any} pItem item a eliminar.
 */
function RemoverItemDataTable (pDataTable, pItem) {
    $(pDataTable).DataTable().row($(pItem).parents('tr')).remove().draw(false);
}

/**
 * Permite remover un item de un combobox select2.
 * @param {any} pSelect2 combobox select2 de la vista.
 * @param {any} pValor valor del item a remover
 */
function RemoverItemSelect2 (pSelect2, pValor) {
    $(`${pSelect2} option[value='${pValor}']`).remove();
}

/**
 * Permite insertar un item en una tabla DataTable.
 * @param {any} pDataTable tabla DataTable.
 * @param {any} pItem item a insertar.
 */
function InsertarItemDataTable (pDataTable, pItem) {
    $(pDataTable).DataTable().row.add(pItem).draw(false);
}

/**
 * Permite insertar un item en un combobox select2.
 * @param {any} pSelect2 combobox select2 de la vista.
 * @param {any} pTexto texto que se muestra en las opciones.
 * @param {any} pValor valor del item dentro del combobox select2.
 * @param {any} pDefaultSelected indica si por defecto el valor se selecciona.
 * @param {any} pSelect indica si se debe seleccionar el item.
 */
function InsertarItemSelect2 (pSelect2, pTexto, pValor, pDefaultSelected = false, pSelect = false) {
    var newOption = new Option(pTexto, pValor, pDefaultSelected, pSelect);
    $(pSelect2).append(newOption).trigger('change');
}

/**
 * Permite insertar un conjunto de opciones a un combobox select2.
 * @param {any} pSelect2 combobox select2 de la vista.
 * @param {any} pDataSet listado de opciones a insertar. IMPORTANTE: la forma de la lista debe ser [{ text: <texto>, value: <valor> }, {..}, ..., {..}]
 */


function InsertarDataSetSelect2(pSelect2, pDataSet, pTrigerOnChange = true, pDefaultSelected = false, pSelected = false) {
    if (pDataSet.length > 0) {
        pDataSet.forEach(option => {
            var newOption = new Option(option.text, option.value, pDefaultSelected, pSelected);

            if (option.extraParameters) {
                for (let i = 0; i < option.extraParameters.length; i++) {
                    newOption.setAttribute(option.extraParameters[i].attr, option.extraParameters[i].value);
                }
            }

            $(pSelect2).append(newOption);
        });

        if (pTrigerOnChange) {
            $(pSelect2).trigger('change');
        }
    }
}

/**
 * Útil para cuando se edita un formulario y se deben seleccionar items de un combobox select2 multiple.
 * @param {any} pSelect2 combobox select2 de la vista.
 * @param {any} pDataSet las opciones a seleccionar en el combobox múltiple.
 * @param {any} pLlave valor mediante el cuál se obtiene el valor de 'pDataSet', comúnmente es 'value' o algún ID.
 * @param {any} pActivarEventoOnChange (opcional) - hace posible visualizar los cambios en el input.
 */
function SeleccionarItemsSelect2Multiple(pSelect2, pDataSet, pLlave, pActivarEventoOnChange = false) {
    if (pDataSet.length > 0) {
        let list = [];
        pDataSet.forEach(option => {
            list.push(option[pLlave]);
        });
        $(pSelect2).val(list);
        $(pSelect2).trigger('change'); // trigger a nivel interno

        if (pActivarEventoOnChange) {
            $(pSelect2).trigger({ // trigger a nivel visual
                type: 'select2:select'
            });
        }
    }
}

/**
 * Seleccionar una opción de un combobox select2.
 * @param {any} pSelect2 combobox select2 de la vista.
 * @param {any} pValue valor utilizado en el combobox como atributo 'value'.
 * @param {any} pActivarEventoOnChange (opcional) - hace posible visualizar los cambios en el input.
 */
function SeleccionarItemSelect2(pSelect2, pValue, pActivarEventoOnChange = false) {
    $(pSelect2).val(pValue);
    $(pSelect2).trigger('change');

    if (pActivarEventoOnChange) {
        $(pSelect2).trigger({
            type: 'select2:select'
        });
    }
}

/**
 * Inserta en un combobox select2 múltiple la opción 'Todos'.
 * @param {any} pSelect2 input select2 de la vista.
 */
function InsertarOpcionTodosSelect2Multiple(pSelect2) {
    var newOption = new Option("Todos", "all", true, false);
    $(pSelect2).append(newOption).trigger('change');
}

/**
 * Inserta un parámetro en la URL del navegador. Afecta al state del history. No recarga página.
 * Nota: se realiza un replace del state.
 * @param {any} pParametro parámetro valor llave.
 * @param {any} pValor valor del parámetro.
 */
function InsertarParametroUrl(pParametro, pValor) {
    const url = new URL(window.location);
    url.searchParams.set(pParametro, pValor);
    window.history.replaceState(null, '', url.toString());
}

function RemoverOpcionesSelect2Multiple (pOpcion) {
    if (pOpcion == 'Todos') {
        $(".multiple-Select > option").prop("selected", false);
        $(".multiple-Select").trigger("change");
    }
    else {
        $(".multiple-Select > option[value='all']").prop("selected", false);
        $(".multiple-Select").trigger("change");
    }
}

/**
 * Obtener el valor de un parámetro de la URL.
 * @param {any} pParametro parámetro del valor a obtener.
 */
function ObtenerValorParametroUrl (pParametro) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(pParametro);
}

/**
 * Permite realizar llamados ajax asincrónicos.
 * @param {any} pURL dirección URL.
 * @param {any} pHttpMethod método Http a ejecutar.
 * @param {any} pParams parámetros a enviar durante la consulta, comúnmente son objetos o valores individuales.
 */
function execAjaxCall(pURL, pHttpMethod, pParams = null) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: jsUtilidades.Variables.urlOrigen + pURL,
            type: pHttpMethod,
            dataType: "JSON",
            data: pParams,
            success: function (obj) {
                if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                    resolve(obj);
                }
                else {
                    reject(obj);
                }
            },
            error: function () {
                reject()
            }
        })
    })
}




function execAjaxCallArray(pURL, pHttpMethod, pParams = null) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: jsUtilidades.Variables.urlOrigen + pURL,
            type: pHttpMethod,
            dataType: "JSON",
            data: JSON.stringify({ pParams }),
            success: function (obj) {
                if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                    resolve(obj);
                }
                else {
                    reject(obj);
                }
            },
            error: function () {
                reject()
            }
        })
    })
}





function execAjaxCallFile(pURL, pParams = null) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: jsUtilidades.Variables.urlOrigen + pURL,
            dataType: "JSON",
            type: 'post',
            contentType: false,
            processData: false,
            data: pParams,
            success: function (obj) {
                if (obj.HayError == jsUtilidades.Variables.Error.NoError) {
                    resolve(obj);
                }
                else {
                    reject(obj);
                }
            },
            error: function () {
                reject()
            }
        })
    })
}



/**
 * Permite validar los inputs completados de un formulario.
 * Retorna un objeto con: 
 * - puedeContinuar: booleano que indica si todos los campos estan completos
 * - objetos: listado de inputs pendientes por completar
 * @param {any} pInputs colección de inputs a validar.
 * @param {any} pExcepciones colección de inputs a ignorar en la validación.
 */
function ValidarFormulario(pInputs, pExcepciones = []) {
    let inputList = [...$(pInputs)];

    if (pExcepciones.length > 0) {
        inputList = inputList.filter(input => {
            if (pExcepciones.includes($(input).attr("id"))) return false;

            $(input).attr("class").split(' ').forEach(class_ => {
                if (pExcepciones.includes(class_)) return false;
            })
        });
    }

    let inputsPendientesCompletar = inputList.filter(input => {
        return $.trim(input.value) == "" || input.value == null;
    });
    return { puedeContinuar: inputsPendientesCompletar.length == 0 ? true : false, objetos: inputsPendientesCompletar };
}

/**
 * Permite el manejo de excepciones según la respuesta del controlador.
 * Si no es un error contralado retorna un mensaje genérico.
 * @param {any} pError
 */
function ManejoDeExcepciones (pError) {
    if (pError?.HayError == jsUtilidades.Variables.Error.ErrorControlado) {
        jsMensajes.Metodos.OkAlertErrorModal(pError.MensajeError).set('onok', function (closeEvent) { });
    }
    else {
        jsMensajes.Metodos.OkAlertErrorModal().set('onok', function (closeEvent) { });
    }
}

$(document).on("keypress", '.solo_operacion', function (e) {
    var regex = new RegExp("^[0-9]|[-+*>=</]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
});


$(document).on("keypress", '.alfa_numerico', function (e) {
    var regex = new RegExp("^[0-9A-Za-zÁÉÍÓÚáéíóúñÑ.,; ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
});



$(document).on("keypress", '.solo_texto', function (e) {
    var regex = new RegExp("^[A-Za-zÁÉÍÓÚáéíóúñÑ.,; ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
});

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    return decodeURI(results[1]) || 0;
}
