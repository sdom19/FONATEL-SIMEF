jsRegistroIndicadorFonatelEdit= {
    "Controles": {
        "btnllenadoweb": "#TableRegistroIndicadorFonatel tbody tr td .btn-edit-form",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "txtCantidadRegistroIndicador": "#txtCantidadRegistroIndicador",
        "tabActivoRegistroIndicador": "div.tab-pane.active",
        "tablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table",
        "columnasTablaIndicador": "div.tab-pane.active .table-wrapper-fonatel table thead tr",

        "btnDescargarPlantillaRegistro": "#btnDescargarPlantillaRegistro",
        "btnCargarPlantillaRegistro": "#btnCargarPlantillaRegistro",

        "fileCargaRegistro":"#fileCargaRegistro",
        "btnCancelar": "#btnCancelarRegistroIndicador",
        "btnGuardar": "#btnGuardarRegistroIndicador",
        "btnGuardarRegistroIndicadorEdicion":"#btnGuardarRegistroIndicadorEdicion",
        "btnValidar": "#btnValidarRegistroIndicador",
        "IndicadorCorrecto": "#Indicador1",
        "IndicadorErroneo": "#Indicador2",
        "btnCarga": "#btnCargaRegistroIndicador",
        "btnCargaRegistroIndicador": "#btnCargaRegistroIndicador",
        "btnCargaRegistroIndicadorEdicion": "#btnCargaRegistroIndicadorEdicion",
        "InputSelect2": (id, option) => `<div class="select2-wrapper">
                                                    <select class="listasDesplegables" id="${id}" >
                                                    <option></option>${option}</select ></div >`,
        "InputDate": id => `<input type="date" class="form-control form-control-fonatel" id="${id}">`,
        "InputText": (id, placeholder) => `<input type="text" aria-label="${placeholder}" class="form-control form-control-fonatel alfa_numerico" id="${id}" placeholder="${placeholder}" style="min-width:150px;">`,

        "inputFileCargarPlantilla": "#inputFileCargarPlantilla"

    },
    "Variables": {
        "VariableIndicador": 1,
        "Validacion": false,
        "paginasActualizadasConSelect2_tablaIndicador": {}
    },

    "Metodos": {

        "CargarExcel": function () {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido descargado")
        },
    }

}


$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnCancelar, function (e) {
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index";
        });
});

//GUARDAR MODO INFORMARTE
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnGuardar, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido guardado")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/RegistroIndicadorFonatel/Index";});
        });
});

//GUARDAR MODO ENCARGADO
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnGuardarRegistroIndicadorEdicion, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("Existen campos vacíos. ¿Desea realizar un guardado parcial para el Formulario?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El Formulario ha sido guardado")
                .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/index"; });
        });
});

//CARGAR MODO INFORMARTE
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnCarga, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar la carga de la información?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La carga de información ha sido completada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/EditarFormulario/Index"; });
        });
});

//CARGAR MODO ENCARGADO
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnCargaRegistroIndicadorEdicion, function (e) {
    e.preventDefault();
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea realizar la carga de la información?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La carga de información ha sido completada")
                .set('onok', function (closeEvent) { window.location.href = "/EditarFormulario/index"; });
        });
});
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnllenadoweb, function () {
    let id = 1;
    window.location.href = "/Fonatel/RegistroIndicadorFonatel/Create?id=" + id;
});
//DESCARGAR
$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnDescargarPlantillaRegistro, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea descargar el Formulario", null, "Descargar Registro")
        .set('onok', function (closeEvent) {
            window.open(jsUtilidades.Variables.urlOrigen + "/EditarFormulario/DescargarExcel");
            jsRegistroIndicadorFonatelEdit.Metodos.CargarExcel();

       });
});

$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnCargarPlantillaRegistro, function () {
    $(jsRegistroIndicadorFonatelEdit.Controles.inputFileCargarPlantilla).click();
});

$(document).on("change", jsRegistroIndicadorFonatelEdit.Controles.inputFileCargarPlantilla, function (e) {
    jsMensajes.Metodos.OkAlertErrorModal("El Formulario ha sido cargado")
});

$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.IndicadorCorrecto, function () {


    jsRegistroIndicadorFonatelEdit.Variables.Validacion = false;

});

$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.IndicadorErroneo, function () {
    $(jsRegistroIndicadorFonatelEdit.Controles.btnCargaRegistroIndicadorEdicion).prop("disabled", true);
    $(jsRegistroIndicadorFonatelEdit.Controles.btnCarga).prop("disabled", true);
    jsRegistroIndicadorFonatelEdit.Variables.Validacion = true;

});


$(document).on("click", jsRegistroIndicadorFonatelEdit.Controles.btnValidar, function () {


    if (jsRegistroIndicadorFonatelEdit.Variables.Validacion == false) {

        jsMensajes.Metodos.OkAlertModal("La información ingresada cumple con los criterios de validación.");
        $(jsRegistroIndicadorFonatelEdit.Controles.btnCargaRegistroIndicadorEdicion).prop("disabled", false);
        $(jsRegistroIndicadorFonatelEdit.Controles.btnCarga).prop("disabled", false);

    } else {
        jsMensajes.Metodos.OkAlertErrorModal("La información ingresada no es congruente con el registro del mes anterior");
    }

});




/*
 Evento para cada input Cantidad de Registros de cada tab o indicador.
 */
$(document).on("keypress", jsRegistroIndicadorFonatelEdit.Controles.txtCantidadRegistroIndicador, function () {


    if (event.keyCode == 13) {
        var table = $(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicador).DataTable();
        table.clear().draw();

        let tabActual = getTabActivoRegistroIndicador();

        jsRegistroIndicadorFonatelEdit.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual] = [];
        
        if ($(this).val() != 0 || $(this).val().trim() != "") {
            for (let x = 0; x < $(this).val(); x++) {
                let listaColumnasVariablesDato = [];

                $(jsRegistroIndicadorFonatelEdit.Controles.columnasTablaIndicador).children().each(function (index) {
                    if ($(this).attr('class').includes("highlighted")) {
                        listaColumnasVariablesDato.push(1);
                    }
                    else if ($(this).attr('class').includes("name-col")) {
                        listaColumnasVariablesDato.push(
                            jsRegistroIndicadorFonatelEdit.Controles.InputText(`inputText-${tabActual}-${x}-${index}`, "Nombre"));
                    }
                    else if ($(this).attr('class').includes("date-col")) {
                        listaColumnasVariablesDato.push(
                            jsRegistroIndicadorFonatelEdit.Controles.InputDate(`inputDate-${tabActual}-${x}-${index}`));
                    }
                    else {
                        listaColumnasVariablesDato.push(
                            jsRegistroIndicadorFonatelEdit.Controles.InputSelect2(
                                `inputSelect-${tabActual}-${x}-${index}`,
                                '<option value="1">Opción 1</option><option value="2">Opción 2</option>'));
                    }
                });
                table.row.add(listaColumnasVariablesDato).draw(false);

                $(jsRegistroIndicadorFonatelEdit.Controles.btnDescargarPlantillaRegistro).prop("disabled", false);
                $(jsRegistroIndicadorFonatelEdit.Controles.btnCargarPlantillaRegistro).prop("disabled", false);
            }

            jsRegistroIndicadorFonatelEdit.Variables.paginasActualizadasConSelect2_tablaIndicador[tabActual].push(0);

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
    $(jsRegistroIndicadorFonatelEdit.Controles.tablaIndicador).on('page.dt', function () {
        var nextPage = $(this).DataTable().page.info().page; 
        let listaPages = jsRegistroIndicadorFonatelEdit.Variables.paginasActualizadasConSelect2_tablaIndicador[getTabActivoRegistroIndicador()];

        if (!listaPages.includes(nextPage)) {
            setTimeout(() => {
                setSelect2()
            }, 0);

            listaPages.push(nextPage);
        }
    });
}

function getTabActivoRegistroIndicador() {
    return $(jsRegistroIndicadorFonatelEdit.Controles.tabActivoRegistroIndicador).attr("id");
}

function setSelect2() {
    $('.listasDesplegables').select2({
        placeholder: "Seleccione",
        width: 'resolve'
    });
}


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

    $(document).ready(function () {
        var t = $('#tablaEditarIndicador').DataTable({
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
        $(jsRegistroIndicadorFonatelEdit.Controles.btnCargaRegistroIndicador).prop("disabled", true);

        $(jsRegistroIndicadorFonatelEdit.Controles.btnDescargarPlantillaRegistro).prop("disabled", false);
        $(jsRegistroIndicadorFonatelEdit.Controles.btnCargarPlantillaRegistro).prop("disabled", false);

    });

})