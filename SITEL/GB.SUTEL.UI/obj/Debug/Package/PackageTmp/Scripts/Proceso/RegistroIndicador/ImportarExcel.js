var files;
$(document).ready(function () {
    $("[name='btnPrev']").on("click", function () {
        if ($("[name='file']:eq(0)").val() == "") {
            $("[name='error']").show().html("Seleccione un archivo .xlsm");
        } 
    });
    $("#formUpload").submit(function (event) {
        event.preventDefault();
        $("[name='error']").hide().html("");
        $("[class='alert alert-danger']").hide().html("");
        if ($("[name='file']:eq(0)").val() == "") {
            $("[name='error']").show().html("Seleccione un archivo .xlsm");
        } else {

            carga = document.getElementById($("#formUpload")[0].IDSolicitudIndicadorImportar.value).innerText;

            if (carga == "SI")
            {
                Swal.fire({
                    title: "¿Está seguro de subir el archivo?",
                    text: "Los valores van a ser reemplazados",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#5cb85c',
                    cancelButtonColor: '#d9534f',
                    confirmButtonText: 'Aceptar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        SubmitForm($(this).attr("action"), function (data) {
                            var jData = JSON.parse(data);
                            if (jData.ok == "True") {
                                $("#modalRegistroIndicadoresImportar").modal('hide');
                                $("[name='file']").val('');
                                addSuccess({ msg: "El archivo excel se ha cargado correctamente." });
                               // location.reload();

                            } else {
                                $("[name='file']").val('');
                                $("[name='error']").show().html(jData.strMensaje);
                            }
                        });
                    }
                });c 
            } else
            {
                   SubmitForm($(this).attr("action"), function (data) {
                    var jData = JSON.parse(data);
                    if (jData.ok == "True") {
                        $("#modalRegistroIndicadoresImportar").modal('hide');
                        $("[name='file']").val('');
                        addSuccess({ msg: "El archivo excel se ha cargado correctamente." });
                        //location.reload();
                    } else {
                        $("[name='file']").val('');
                        $("[name='error']").show().html(jData.strMensaje);
                    }
                });
            }
        }
        return false;
    });
   
   
    function SubmitForm(action, ok) {
        $("[name='error']").hide().html("");
        var $form = $("#formUpload");
        var formData = new FormData($("#formUpload")[0]);
        $("#" + $form.attr("id") + " [type='submit']").val($("#" + $form.attr("id")).attr("data-ajax-message")).prop("disabled", true);
        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(500);
        var options = {
            url: action,
            type: $form.attr("method"),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                $("#" + $form.attr("id") + " [type='submit']").val($("#" + $form.attr("id")).attr("data-ajax-message-default")).prop("disabled", false);
                ok(data);
            },
            error: function (error) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                $("[name='error']").show().html(error.statusText);
                $("#" + $form.attr("id") + " [type='submit']").val($("#" + $form.attr("id")).attr("data-ajax-message-default")).prop("disabled", false);
            },
            xhr: function () {
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {
                    myXhr.upload.addEventListener('progress', progress, false);
                }
                return myXhr;
            }
        };
        $.ajax(options);
    }

    function progress(e) {
        if (e.lengthComputable) {
            var max = e.total;
            var current = e.loaded;

            var Percentage = (current * 100) / max;
            

            if (Percentage >= 100) {
                
            }
        }
    }
});