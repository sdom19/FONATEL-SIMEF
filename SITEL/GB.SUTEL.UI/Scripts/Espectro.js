

$(document).on('change', '#selectedFile', function () {
    var fullPath = document.getElementById('selectedFile').value;
    if (fullPath) {
        var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
        var filename = fullPath.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
        $("#txtArchivoDelete").val(filename);
    }
})

$(function () {

    $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");

})


$(document).click("click", "#btnBuscarEliminar", function () {
    alert("Hola");
})



function mostrarModal() {

    $('#modalMsjEspere').modal('show');

}

function spiner() {

    $(".darkScreen").fadeIn(100);
}


$(document).ready(function () {


    $("#divdelete").hide();

    $("#Buttondiv").click(function () {
        $("#divdelete").slideToggle();
    });

    showModal = function () {

        $('#modalReemplazar').modal('show');
    }

    $(".darkScreen").fadeOut(100, function () {
        $(this).remove();
    })

    var file = document.getElementById("file");

    file.onchange = function (event) {


        if (file.files.length > 0) {

            fileList = file.files;

            if (fileList.length > 0) {


                var contadorDeBytes = 0;

                for (var i = 0; i < fileList.length; i++) {

                    contadorDeBytes += fileList[i].size;
                }

                if ((contadorDeBytes / 1024 / 1024) > 80) {

                    $('#alrtExcesoDeTamano').show();
                    $('#btnSubir').prop("disabled", true);

                } else {

                    $('#alrtExcesoDeTamano').hide();
                    $('#btnSubir').prop("disabled", false);

                }
            } else {

                $('#btnSubir').prop("disabled", true);

            }
        } else {

            for (var i = 0; i < fileList.length; i++) {

                file.files = fileList;
            }

        }
    }

    //--------------------confirma la existencia del Archivo que se va eliminar------------------------------------//
    $("#buttonEliminar").click(function (e) {
        var nombreArchivoEliminar = $("#txtArchivoDelete").val();

        debugger;
        if (nombreArchivoEliminar == "") {
            data = "<div id='Mmensaje'>Por favor digite un nombre.</div>";
            $("#Mmensaje").replaceWith(data);
            $('#modalMensajeD').modal('show');
            return null;
        }



        var json = {
            "NombredelArchivo": nombreArchivoEliminar,
        };

        var options = {
            type: "post",
            dataType: "json",
            data: { "data": json },
            url: "/Espectro/ExistenciaDelArchivo",
            success: function (data) {
                debugger;

                if (data.ok === "True") {
                    data = "<div class='col-sm-9' id='mensaje'>" + data.strMensaje + "</div>";

                    $("#mensaje").replaceWith(data);

                    $('#modalMsjDelete').modal('show');

                } else {

                    //$("#titulo").replaceWith("Advertencia");

                    data = "<div class='col-sm-9' id='Mmensaje'>" + data.strMensaje + "</div>";
                    $("#Mmensaje").replaceWith(data);
                    $('#modalMensajeD').modal('show');

                }
            },
            error: function (xhr, status, error) {
                debugger;
                if (xhr.readyState == 4) {
                    if (xhr.status == 200) {

                        //$("#titulo").replaceWith("Error!!");
                        var data = "<div class='col-sm-9' id='Mmensaje'>" + xhr.responseText + "</div>";
                        $("#Mmensaje").replaceWith(data);
                        $('#modalMensajeD').modal('show');

                    }
                }
            }
        };

        $.ajax(options);
        e.preventDefault();
    })

    //--------------------Eliminar el Archivo-----------------------------------------//
    $("#btnEliminar").click(function (e) {
        debugger;
        var nombreArchivoEliminar = $("#txtArchivoDelete").val();
        $('#modalMsjDelete').modal('hide');

        var json = {
            "NombredelArchivo": nombreArchivoEliminar,
        };
        spiner();
        var options = {
            type: "post",
            dataType: "json",
            data: { "data": json },
            url: "/Espectro/EliminarArchivo",
            success: function (data) {
                debugger;
                $(".darkScreen").fadeOut(100);
                if (data.ok === "True") {

                    data = "<div class='col-sm-9' id='Mmensaje'>" + data.strMensaje + "</div>";

                    //$("#titulo").replaceWith("Eliminado");

                    $("#Mmensaje").replaceWith(data);
                    var nombreArchivoEliminar = $("#txtArchivoDelete").val("");
                    $('#modalMensajeD').modal('show');

                } else {


                    data = "<div  class='col-sm-9' id='Mmensaje'>" + data.strMensaje + "</div>";
                    //$("#titulo").replaceWith("Error");
                    $("#Mmensaje").replaceWith(data);
                    var nombreArchivoEliminar = $("#txtArchivoDelete").val("");
                    $('#modalMensajeD').modal('show');

                }
            },
            error: function (xhr, status, error) {
                debugger;
                if (xhr.readyState == 4) {
                    if (xhr.status == 200) {
                        var data = "<div class='col-sm-9' id='Mmensaje'>" + xhr.responseText + "</div>";

                        //$("#titulo").replaceWith("Mensaje");
                        $("#Mmensaje").replaceWith(data);
                        var nombreArchivoEliminar = $("#txtArchivoDelete").val("");
                        $('#modalMensajeD').modal('show');

                    }
                }
            }
        }

        $.ajax(options);
        e.preventDefault();
    });

})



