    JsCategoria= {
        "Controles": {
            "divFechaMinima": "#divFechaMinimaCategoria",
            "divFechaMaxima": "#divFechaMaximaCategoria",
            "divCantidadDetalle": "#divCantidadDetalleCategoria",
            "divRangoMinimoCategoria": "#divRangoMinimaCategoria",
            "divRangoMaximaCategoria": "#divRangoMaximaCategoria",
            "ddlTipoDetalle": "#ddlTipoDetalleCategoria",
            "btnGuardarCategoria": "#btnGuardarCategoria",
            "btnCancelar": "#btnCancelarCategoria",
            "btnCancelarDetalle": "#btnCancelarDetalleCategoria",
            "btnEditarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-edit",
            "btnDesactivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-off",
            "btnActivarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-power-on",
            "btnClonarCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-clone",
            "btnAddCategoria": "#TableCategoriaDesagregacion tbody tr td .btn-add",
            "btnGuardarDetalleCategoria": "#btnGuardarDetalleCategoria",
            "tablacategoria": "#TableCategoriaDesagregacion tbody",
            "TablaCategoriaDetalle": "#TableCategoriaDesagregacionDetalle tbody",
            "btnEliminarDetalle": "#TableCategoriaDesagregacionDetalle tbody .btn-delete",
            "btnCargarDetalle": "#TableCategoriaDesagregacion tbody tr td .btn-upload",
            "inputFileCargarDetalle": "#inputFileCargarDetalle"
        },
        "Variables": {
            "TipoFecha": 4,
            "TipoNumerico": 1,
            "OpcionSalir": true,
            "ListadoCategoria": [],
            "ListadoCategoriaDetalle":[]
        },
        "Metodos": {

            "CargarTablaCategoria": function () {
                EliminarDatasource();
                let html = "";
                for (var i = 0; i < JsCategoria.Variables.ListadoCategoria.length; i++) {
                    let categoria = JsCategoria.Variables.ListadoCategoria[i];

                    html = html + "<tr>"

                    html = html + "<td scope='row'>" + categoria.Codigo + "</td>";
                    html = html + "<td>" + categoria.NombreCategoria + "</td>";
                    if (!categoria.TieneDetalle) {
                        html = html + "<td><strong>N/A</strong></td>";
                        html = html + "<td>" + categoria.EstadoRegistro.Nombre + "</td>";
                        html = html + "<td><strong>N/A</strong></td>";
                    }
                    else {
                        html = html + "<td>" + categoria.CantidadDetalleDesagregacion + "/" + categoria.DetalleCategoriaTexto.length + "</td>";
                        html = html + "<td>" + categoria.EstadoRegistro.Nombre + "</td>";
                        html = html + "<td><input id='inputFileCargarDetalle' type='file' accept='.csv,.xlsx,.xls' style='display: none;' />" +
                            "<button type='button' data-toggle='tooltip' data-placement='top' title='Cargar Detalle' class='btn-icon-base btn-upload'></button>" +
                            "<button type='button' data-toggle='tooltip' data-placement='top' title='Descargar Plantilla' class='btn-icon-base btn-download'></button>" +
                            "<button type='button' data-toggle='tooltip' data-placement='top' title='Agregar Detalle' class='btn-icon-base btn-add'></button></td>";
                    }
                    html = html + "<td><button  type='button' data-toggle='tooltip' data-placement='top' value=" + categoria.idCategoria + " title='Editar' class='btn-icon-base btn-edit'></button>" +
                        "<button type = 'button' data - toggle='tooltip' data - placement='top' title = 'Clonar' class='btn-icon-base btn-clone' ></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' title='Desactivar' class='btn-icon-base btn-power-on'></button></td >";
                    html = html + "</tr>"
                }
                $(JsCategoria.Controles.tablacategoria).html(html);
                CargarDatasource();
            },

            "CargarTablaDetalleCategoria": function () {
                EliminarDatasource();
                let html = "";
                
                for (var i = 0; i < JsCategoria.Variables.ListadoCategoriaDetalle.length; i++) {
                    let detalle = JsCategoria.Variables.ListadoCategoriaDetalle[i];
                    html = html + "<tr>"

                    html = html + "<td scope='row'>" + detalle.Codigo + "</td>";
                    html = html + "<td scope='row'>" + detalle.Etiqueta + "</td>";

                    html = html + "<td><button type='button' data-toggle='tooltip' data-placement='top' title='Editar' class='btn-icon-base btn-edit'></button>" +
                        "<button type='button' data-toggle='tooltip' data-placement='top' title='Eliminar' value=" + detalle.idCategoriaDetalle + " class='btn-icon-base btn-delete'></button></td>";
                    html = html + "</tr>"
                }
                $(JsCategoria.Controles.TablaCategoriaDetalle).html(html);
                CargarDatasource();
            },

            "CerrarFormulario": function () {
                jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea Salir del Formulario?", jsMensajes.Variables.actionType.cancelar)
                    .set('onok', function (closeEvent) {
                        
                    }); 
            },
            "HabilitarControlesTipoCategoria": function (selected) {
                if (selected == JsCategoria.Variables.TipoFecha) {
                    $(JsCategoria.Controles.divRangoMaximaCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divRangoMinimoCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMaxima).removeClass("hidden");
                    $(JsCategoria.Controles.divFechaMinima).removeClass("hidden");
                    $(JsCategoria.Controles.divCantidadDetalle).addClass("hidden");
                }
                else if (selected == JsCategoria.Variables.TipoNumerico) {
                    $(JsCategoria.Controles.divFechaMaxima).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMinima).addClass("hidden");
                    $(JsCategoria.Controles.divRangoMaximaCategoria).removeClass("hidden");
                    $(JsCategoria.Controles.divRangoMinimoCategoria).removeClass("hidden");
                    $(JsCategoria.Controles.divCantidadDetalle).addClass("hidden");
                }
                else {
                    $(JsCategoria.Controles.divRangoMaximaCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divRangoMinimoCategoria).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMaxima).addClass("hidden");
                    $(JsCategoria.Controles.divFechaMinima).addClass("hidden");
                    $(JsCategoria.Controles.divCantidadDetalle).removeClass("hidden");
                }
            }
        },

        "Consultas": {
            "ConsultaDatosCategoria": function () {
                $.ajax({
                    url: urlOrigen + '/CategoriasDesagregacion/ObtenerCategorias',
                    type: "GET",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    success: function (obj) {
                        if (obj.objetoRespuesta !== undefined) {
                            JsCategoria.Variables.ListadoCategoria = obj.objetoRespuesta;
                            JsCategoria.Metodos.CargarTablaCategoria();
                        }
                        $("#loading").fadeOut();
                    }
                }).fail(function (obj) {
                    $("#loading").fadeOut();
                })
            },
            "ConsultaDatosCategoriaDetalle": function () {
                let idCategoria = 1;
                $.ajax({
                    url: urlOrigen + '/CategoriasDesagregacion/ObtenerCategoriasDetalle?idCategoria=' + idCategoria,
                    type: "GET",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    success: function (obj) {   
                        if (obj.objetoRespuesta !== undefined) {
                            JsCategoria.Variables.ListadoCategoriaDetalle = obj.objetoRespuesta;
                            JsCategoria.Metodos.CargarTablaDetalleCategoria();
                        }
                        $("#loading").fadeOut();
                    }
                }).fail(function (obj) {
                    $("#loading").fadeOut();
                })
            },
            "EliminarDetalleCategoria": function (idDetalleCategoria) {
                $.ajax({
                    url: urlOrigen + '/CategoriasDesagregacion/EliminarCategoriasDetalle',
                    type: "POST",
                    dataType: "JSON",
                    beforeSend: function () {
                        $("#loading").fadeIn();
                    },
                    data:{idDetalleCategoria},
                    success: function (obj) {
                        if (obj.objetoRespuesta !== undefined) {
                            $("#loading").fadeOut();
                            jsMensajes.Metodos.OkAlertModal("El Detalle ha sido Eliminado")
                                .set('onok', function (closeEvent)
                                {
                                    JsCategoria.Variables.ListadoCategoriaDetalle = obj.objetoRespuesta;
                                    JsCategoria.Metodos.CargarTablaDetalleCategoria();
                                });      
                            
                        }
                       
                    }
                }).fail(function (obj) {
                    console.log(obj);
                    $("#loading").fadeOut();

                })
            },
        }

}

$(document).on("click", JsCategoria.Controles.btnCancelar, function (e) {
    e.preventDefault();
    JsCategoria.Variables.OpcionSalir = false;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            preguntarAntesDeSalir = false;  window.location.href = "/Fonatel/CategoriasDesagregacion/Index"; 
        });   
});

$(document).on("click", JsCategoria.Controles.btnCancelarDetalle, function (e) {
    JsCategoria.Variables.OpcionSalir = false;
    e.preventDefault();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea cancelar la acción?", jsMensajes.Variables.actionType.cancelar)
        .set('onok', function (closeEvent) {
            window.location.href = "/Fonatel/CategoriasDesagregacion/Index";
        });
});


$(document).on("change", JsCategoria.Controles.ddlTipoDetalle, function () {
    var selected = $(this).val();
    JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
});

$(document).on("change", JsCategoria.Controles.ddlTipoDetalle, function () {
    var selected = $(this).val();
    JsCategoria.Metodos.HabilitarControlesTipoCategoria(selected);
});


$(document).on("click", JsCategoria.Controles.btnEditarCategoria, function () {
    JsCategoria.Variables.OpcionSalir = false;
    let id = $(this).val();
    window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id=" + id;
});


$(document).on("click", JsCategoria.Controles.btnAddCategoria, function () {
    JsCategoria.Variables.OpcionSalir = false;
    let id = 1;
    window.location.href = "/Fonatel/CategoriasDesagregacion/Detalle?id=" + id;
});




$(document).on("click", JsCategoria.Controles.btnDesactivarCategoria, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea activar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido activada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});

$(document).on("click", JsCategoria.Controles.btnActivarCategoria, function () {
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea desactivar la Categoría?", jsMensajes.Variables.actionType.estado)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido desactivada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});


$(document).on("click", JsCategoria.Controles.btnGuardarCategoria, function (e) {
    e.preventDefault();
    JsCategoria.Variables.OpcionSalir = false;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("La Categoría ha sido creada")
                .set('onok', function (closeEvent) { window.location.href = "/Fonatel/CategoriasDesagregacion/index" });
        });
});



$(document).on("click", JsCategoria.Controles.btnGuardarDetalleCategoria, function (e) {
    e.preventDefault();

    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea agregar  el detalle a la Categoría?", jsMensajes.Variables.actionType.agregar)
        .set('onok', function (closeEvent) {
            jsMensajes.Metodos.OkAlertModal("El detalle ha sido agregado")
                .set('onok', function (closeEvent) {  });
        });
});





$(document).on("click", JsCategoria.Controles.btnClonarCategoria, function () {
    let id = 1;
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea clonar la Categoría?", jsMensajes.Variables.actionType.clonar)
        .set('onok', function (closeEvent) {
             window.location.href = "/Fonatel/CategoriasDesagregacion/Create?id="+id
        });
});

$(document).on("click", JsCategoria.Controles.btnCargarDetalle, function (e) {
    $(JsCategoria.Controles.inputFileCargarDetalle).click();
});


$(document).on("click", JsCategoria.Controles.btnEliminarDetalle, function (e) {
    let id = $(this).val();
    jsMensajes.Metodos.ConfirmYesOrNoModal("¿Desea eliminar el Detalle?", jsMensajes.Variables.actionType.eliminar)
       .set('onok', function (closeEvent) {
         
           JsCategoria.Consultas.EliminarDetalleCategoria(id);
        });


});



//window.addEventListener('beforeunload', (event) => {

//    if (JsCategoria.Variables.OpcionSalir) {
//        // Cancel the event as stated by the standard.
//        event.preventDefault();
//        // Chrome requires returnValue to be set.
//        event.returnValue = '';
//    }

//});

//window.addEventListener('navigateback', function () {
//    alert("sadas");
//}, false);
$(function () {
    if ($(JsCategoria.Controles.tablacategoria).length > 0) {
         JsCategoria.Consultas.ConsultaDatosCategoria();

      
    }
    if ($(JsCategoria.Controles.TablaCategoriaDetalle).length > 0) {
        JsCategoria.Consultas.ConsultaDatosCategoriaDetalle();
    }
});

