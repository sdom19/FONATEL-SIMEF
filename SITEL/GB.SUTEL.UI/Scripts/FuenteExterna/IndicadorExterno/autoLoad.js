var files;
$(document).ready(function () {
    $("[name='btnPrev']").on("click", function () {
        $("#btnCargaAutomaticaCargaRegitros").attr("disabled", "disabled");
        if ($("[name='file']:eq(0)").val() == "") {
            $("[name='error']").show().html("Seleccione un archivo .xlsx");
        } else {
            SubmitForm($("#formUpload").attr("preview"), function (data) {
                $("#_tablePreview").html(data);                

                if (!($('#divTableFuentesExternasPreview').length > 0)) {
                    $("#btnCargaAutomaticaCargaRegitros").attr("disabled", "disabled");
                }
                else {
                    $("#btnCargaAutomaticaCargaRegitros").attr("disabled", false);
                }
            });
        }
    });
    $("#formUpload").submit(function (event) {
        event.preventDefault();
        $("[name='error']").hide().html("");
        $("[class='alert alert-danger']").hide().html("");
        if ($("[name='file']:eq(0)").val() == "") {
            $("[name='error']").show().html("Seleccione un archivo .xlsx");
        } else {
            SubmitForm($(this).attr("action"), function (data) {
                var jData = JSON.parse(data);
                if (jData.ok == "True") {
                    debugger;
                    addSuccess({ msg: jData.strMensaje });
                    window.location = "/RegistroIndicadorExterno/Index";
                }
                else {
                    $("[name='error']").show().html(jData.strMensaje);
                }
                $("#_tablePreview").html("");
                $("[name='file']").val(null);
                window.location.href = $("[name='btnBack']").attr("href");
            });
        }
        return false;
    });
    $("[name='file']").on("change", function () {
        $("#_tablePreview").html("");
        $("#btnCargaAutomaticaCargaRegitros").attr("disabled", "disabled");
    });

    function SubmitForm(action, ok) {
        debugger;
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
                debugger;
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
            console.log(Percentage);

            if (Percentage >= 100) {
                console.log("process completed!");
            }
        }
    }
});