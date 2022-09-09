$(document).ready(function () {
    function onSuccess(data) {
        $("[name='error']").hide().html("");
        var jData = JSON.parse(data);
        $("#formCreate").trigger('reset');
        $("[data-valmsg-for]").html("");
        addSuccess({ msg: jData.strMensaje });
    }
    function onFailure(error) {
        if (error.status == 409)
            $('[data-valmsg-for="NameRepeated"]').html(error.statusText);
        else
            $("[name='error']").show().html(error.statusText);
    }
    function validateForm() {
        if (validarClave() == false) {
            return false;
        }
        return true;
    }
    //Init

    $(".required").rules("add", {
        required: true,
        messages: {
            required: RequiredMessage
        }
    });

    AjaxifyMyForm("formCreate", onSuccess, onFailure, null, validateForm);
    $('.tipoInterno').hide();
    $('[data-valmsg-for="passRepeat"]').hide();

    $("[name='AccesoUsuario']:eq(0)").on('blur', function () {
        $('[data-valmsg-for="NameRepeated"]').html("");
    });
    $("[name='passRepeat']:eq(0)").on('blur', function () {
        validarClave();
    });
    
    $("[name='ACCESOAD']").on('change', function () {
        $('[data-valmsg-for="NameRepeated"]').html("");
        var namee = $("[name='ACCESOAD']:eq(0) option:selected").attr('data-name') == undefined ? "" : $("[name='ACCESOAD']:eq(0) option:selected").attr('data-name');
        $("[name='NOMBREAD']").val(namee);
    });

    $("[name='UsuarioInterno']").on('change', function () {
        $('[data-valmsg-for="NameRepeated"]').html("");
        if ($("[name='UsuarioInterno']").eq(0).find("option:selected").eq(0).val() == 0) {
            $('.tipoExterno').show();
            $('.tipoInterno').hide(); 
            $("[name='ACCESOAD']").val("");
            $("[name='NOMBREAD']").val("");           
        }
        else {
            $('.tipoExterno').hide();
            $('.tipoInterno').show();
            getUsersAD();
        }

    });

    function validarClave() {
        if ($("[name='Contrasena']:eq(0)").val() != $("[name='passRepeat']:eq(0)").val()) {
            $('[data-valmsg-for="passRepeat"]').show();
            return false;
        } else
            $('[data-valmsg-for="passRepeat"]').hide();
        return true;
    }
    function getUsersAD() {
        $("body").append("<div class='darkScreen' hidden><div class='img-waiter'></div></div>");
        $(".darkScreen").fadeIn(500);
        $.ajax({
            url: "getADUsers", type: "get",
            'error': function (data, textStatus) {
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                var jsonData = JSON.parse(data);
                $("[name='ACCESOAD']").eq(0).html(jsonData.data);
            },
            'success': function (data, text, jqXHR) {
                $("[name='error']").hide().html("");
                $(".darkScreen").fadeOut(100, function () {
                    $(this).remove();
                });
                var jsonData = JSON.parse(data);
                $("[name='ACCESOAD']").eq(0).html(jsonData.data);
                if (jsonData.state != 200)
                    $("[name='error']").show().html(jsonData.strMensaje);
            }
        });
    }
});