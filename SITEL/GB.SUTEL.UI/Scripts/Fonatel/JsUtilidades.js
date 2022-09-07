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
        },
        "EstadoRegistros": {
            "EnProceso": 1,
            "Activo" : 2,
            "Desactivado":3,
            "Eliminado": 4
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
    $('.nav-tabs > li a[title]').tooltip();
});

$(document).on("select2:select", '.multiple-Select', function (e) {
    var data = e.params.data.text;
    if (data == 'Todos') {
        $(".multiple-Select > option").prop("selected", "selected");
        $(".multiple-Select").trigger("change");
    }
});


$(document).on("select2:unselect", '.multiple-Select', function (e) {
    var data = e.params.data.text;
    if (data == 'Todos') {
        $(".multiple-Select > option").prop("selected", false);
        $(".multiple-Select").trigger("change");
    }
    else {
        $(".multiple-Select > option[value='all']").prop("selected", false);
        $(".multiple-Select").trigger("change");
    }

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


function EliminarDatasource(pDataTable = ".datatable_simef") {
    $(pDataTable).DataTable().destroy();
}


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

function ConcatenarItems(lista, nombreObj) { // concatenar una serie de objectos de una lista, según el parámetro enviado
    let resultado = "";
    lista.forEach(item => {
        resultado += item[nombreObj].trim() + ", ";
    });
    return resultado.slice(0, resultado.length - 2) + ".";
}

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



function RemoverItemDataTable (pDataTable, pItem) {
    $(pDataTable).DataTable().row($(pItem).parents('tr')).remove().draw();
}

function RemoverItemSelect2 (pSelect2, pValor) {
    $(`${pSelect2} option[value='${pValor}']`).remove();
}

function InsertarItemDataTable (pDataTable, pListaItems) {
    $(pDataTable).DataTable().row.add(pListaItems).draw(false);
}

function InsertarItemSelect2 (pSelect2, pTexto, pValor, pDefaultSelected = false, pSelect = false) {
    var newOption = new Option(pTexto, pValor, pDefaultSelected, pSelect);
    $(pSelect2).append(newOption).trigger('change');
}

function InsertarParametroUrl (pParametro, pValor) {
    const url = new URL(window.location);
    url.searchParams.set(pParametro, pValor);
    window.history.pushState(null, '', url.toString());
}

function ObtenerValorParametroUrl (pParametro) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(pParametro);
}



function ConcatenarItems(lista, nombreObj) { // concatenar una serie de objectos de una lista, según el parámetro enviado
    let resultado = "";
    lista.forEach(item => {
        resultado += item[nombreObj].trim() + ", ";
    });
    return resultado.slice(0, resultado.length - 2) + ".";
}

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

$(document).on("keypress", '.solo_operacion', function (e) {
    var regex = new RegExp("^[0-9]|[-+*>=</]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
});


$(document).on("keypress", '.alfa_numerico', function (e) {
    var regex = new RegExp("^[0-9]|[a-z]|[\s]|[A-Z]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
});

$(document).on("keypress", '.alfa_numerico_v2', function (e) {
    var regex = new RegExp("^[0-9]|[A-Za-zÁÉÍÓÚáéíóúñÑ]|[\\s]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
});

$(document).on("keypress", '.solo_texto', function (e) {
    var regex = new RegExp("^[a-z]|[A-Z]|[\\s]+$");
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