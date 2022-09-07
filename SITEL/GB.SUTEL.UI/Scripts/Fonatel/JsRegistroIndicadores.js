jsRegistroIndicadorFonatel= {
    "Controles": {
        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-edit-form",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "tabActivoRegistroIndicador": "div.tab-pane.active",
        "tablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table",
        "columnasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr",
        "btnDescargarPlantillaRegistro": "#btnDescargarPlantillaRegistro",
        "fileCargaRegistro":"#fileCargaRegistro",
        "btnCancelar": "#btnCancelarRegistroIndicador",
        "btnGuardar": "#btnGuardarRegistroIndicador",
        "btnValidar": "#btnValidarRegistroIndicador",
        "btnCarga": "#btnCargaRegistroIndicador",
        "btnCargarPlantillaRegistro": "#btnCargarPlantillaRegistro",
        "btnCargaRegistroIndicador":"#btnCargaRegistroIndicador",
        "InputSelect2": (id, option) => `<div class="select2-wrapper">
                                                    <select class="listasDesplegables" id="${id}" >
                                                    <option></option>${option}</select ></div >`,
        "InputDate": id => `<input type="date" class="form-control form-control-fonatel" id="${id}">`,
        "InputText": (id, placeholder) => `<input type="text" aria-label="${placeholder}" class="form-control form-control-fonatel alfa_numerico" id="${id}" placeholder="${placeholder}" style="min-width:150px;">`,

        "inputFileCargarPlantilla": "#inputFileCargarPlantilla"

    },
    "Variables": {
        "VariableIndicador": 1,
        "paginasActualizadasConSelect2_tablaIndicador": {}
    },

    "Metodos": {
        "GuardarEncabezadFormulario": function(){

        },

        "CargarExcel": function () {

            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
        },
    }

}


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index";
        });
});




$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnGuardar, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido guardado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index";});
        });
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCarga, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar la carga de la información?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La carga de información ha sido completada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index"; });
        });
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnllenadoweb, function () {
    let id = 1;
    window.location.href = "/Fonatel/RegistroIndicadorFonatel/Create?id=" + id;
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            window.open(jsUtilidades.Variables.urlOrigen + "/RegistroIndicadorFonatel/DescargarExcel");
            jsRegistroIndicadorFonatel.Metodos.CargarExcel();
        });
});


$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCargaPlantillaRegistro, function () {

   
});

$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnValidar, function () {

    //jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario", null, "Descargar Registro")
    //    .set('onok', function (closeEvent) {
    //        jsMensajes.Metodos.OkAlertModal("La información ingresada cumple con los criterios de validación")

    //    });

    jsMensajes.Metodos.OkAlertModal("La información ingresada cumple con los criterios de validación.", null, "Error en Registro");


});

/*
 Evento para cada input Cantidad de Registros de cada tab o indicador.
 */
$(document).on("keypress", jsRegistroIndicadorFonatel.Controles.txtCantidadRegistroIndicador, function () {


    if (event.keyCode == 13) {
        var table = $(jsRegistroIndicadorFonatel.Controles.tablaIndicador).DataTable();
        table.clear().draw();

        let tabActual = getTabActivoRegistroIndicador();

        jsRegistroIndicadorFonatel.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual] = [];
        
        if ($(this).val() != 0 || $(this).val().trim() != "") {
            for (let x = 0; x < $(this).val(); x++) {
                let listaColumnasVariablesDato = [];

                $(jsRegistroIndicadorFonatel.Controles.columnasTablaIndicador).children().each(function (index) {
                    if ($(this).attr('class').includes("highlighted")) {
                        listaColumnasVariablesDato.push(1);
                    }
                    else if ($(this).attr('class').includes("name-col")) {
                        listaColumnasVariablesDato.push(
                            jsRegistroIndicadorFonatel.Controles.InputText(`inputText-${tabActual}-${x}-${index}`, "Nombre"));
                    }
                    else if ($(this).attr('class').includes("date-col")) {
                        listaColumnasVariablesDato.push(
                            jsRegistroIndicadorFonatel.Controles.InputDate(`inputDate-${tabActual}-${x}-${index}`));
                    }
                    else {
                        listaColumnasVariablesDato.push(
                            jsRegistroIndicadorFonatel.Controles.InputSelect2(
                                `inputSelect-${tabActual}-${x}-${index}`,
                                '<option value="1">Opción 1</option><option value="2">Opción 2</option>'));
                    }
                });
                table.row.add(listaColumnasVariablesDato).draw(false);

                $(jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro).prop("disabled", false);
                $(jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro).prop("disabled", false);
            }

            jsRegistroIndicadorFonatel.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual].push(0);

            setSelect2();

            eventNextPrevDatatable();
        }
    }
});

/*
 Evento que captura los eventos de siguiente y atras de los datatables.
 Se maneja una variable que almacena las paginas visitadas de cada tab o indicador, 
 para así refrescar los select2.
 */
function eventNextPrevDatatable() {
    $(jsRegistroIndicadorFonatel.Controles.tablaIndicador).on('page.dt', function () {
        var nextPage = $(this).DataTable().page.info().page; 
        let listaPages = jsRegistroIndicadorFonatel.Variables.paginasActualizadasConSelect2_tablaIndicador[getTabActivoRegistroIndicador()];

        if (!listaPages.includes(nextPage)) {
            setTimeout(() => {
                setSelect2()
            }, 0);

            listaPages.push(nextPage);
        }
    });
}

function getTabActivoRegistroIndicador() {
    return $(jsRegistroIndicadorFonatel.Controles.tabActivoRegistroIndicador).attr("id");
}

function setSelect2() {
    $('.listasDesplegables').select2({
        placeholder: "Seleccione",
        width: 'resolve'
    });
}

$(document).on("click", jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro, function (e) {
    $(jsRegistroIndicadorFonatel.Controles.inputFileCargarPlantilla).click();
    jsMensajes.Metodos.OkAlertModal("El Formulario ha sido cargado")
});

$(function () {
    $(document).ready(function () {

        var t = $('#TableRegistroIndicadorFonatel').DataTable({
            'scrollY': '400px',
    

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

            columnDefs: [
                {
                    searchable: false,
                    orderable: false,
                    targets: 0,
                },
            ],
            order: [[1, 'asc']],
        });

        t.on('order.dt search.dt', function () {
            let i = 1;

            t.cells(null, 0, { search: 'applied', order: 'applied' }).every(function (cell) {
                this.data(i++);
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#tablaIndicador').DataTable({
            'scrollY': '400px',

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

            columnDefs: [
                {
                    searchable: false,
                    orderable: false,
                    targets: 0,
                },
            ],
            order: [[1, 'asc']],
        });

        t.on('order.dt search.dt', function () {
            let i = 1;

            t.cells(null, 0, { search: 'applied', order: 'applied' }).every(function (cell) {
                this.data(i++);
            });
        }).draw();
    });




    //$(jsRegistroIndicadorFonatel.Controles.btnCargaRegistroIndicador).prop("disabled", false);
    //$(jsRegistroIndicadorFonatel.Controles.btnGuardar).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnDescargarPlantillaRegistro).prop("disabled", true);
    //$(jsRegistroIndicadorFonatel.Controles.btnCargarPlantillaRegistro).prop("disabled", true);




})