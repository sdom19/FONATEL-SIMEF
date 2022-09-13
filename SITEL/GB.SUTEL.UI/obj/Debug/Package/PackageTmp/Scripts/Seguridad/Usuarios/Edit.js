$(document).ready(function () {
    function onSuccess(data) {
        var jData = JSON.parse(data);
        if (parseInt(jData.state) == 200) {
            addSuccess({ msg: JSON.parse(jData.strMensaje).msg });
            var url = document.URL.split("/");
            var allurl = url[0] + "//" + url[2] + "/" + JSON.parse(jData.strMensaje).url;
            window.location.href = allurl;
        } else {            
            if (parseInt(jData.state) == 300)
                $('[data-valmsg-for="NameRepeated"]').html(jData.strMensaje);                
            else 
                $("[name='error']").show().html(jData.strMensaje);
        }
    }
    function onSuccessPass(data) {
        var jData = JSON.parse(data);
        if (parseInt(jData.state) == 200) {
            $("#modalCambiarClave").modal('hide');
            addSuccess({ msg: JSON.parse(jData.strMensaje).msg });
        } else {
            if (parseInt(jData.state) == 300)
                $('[data-valmsg-for="NameRepeated"]').show();
            else
                $("[name='error']").show().html(jData.strMensaje);
        }
    }
    function onFailure(error) {
        $("[name='error']").show().html(error.statusText);
    }
    function validateForm() {
        if ($('.tipoInterno:visible').length > 0) {
            if ($("[name='ACCESOAD']:eq(0) option:selected").val() == "" || $("[name='ACCESOAD']:eq(0) option:selected").val() == undefined) {                
                $("[name='error']").show().html("Seleccione un usuario.");
                return false
            }
        }
        else {
            $("[name='ACCESOAD']:eq(0)").val(null);
            $("[name='ACCESOAD']:eq(0) option:selected").removeAttr("selected");
            $("[name='NOMBREAD']:eq(0)").removeAttr("value");
        }
        return true;
    }
    //Init

    AjaxifyMyForm("formCreate", onSuccess, onFailure, null, validateForm);
    AjaxifyMyForm("formCambiarClave", onSuccessPass, onFailure, null, validarCambioClave);
    
    $('[data-valmsg-for="passRepeat"]').hide();
    $('[data-valmsg-for="passLength"]').hide();

    $("[name='AccesoUsuario']:eq(0)").on('blur', function () {
        $('[data-valmsg-for="NameRepeated"]').html("");
    });
    $("[name='passRepeat']:eq(0)").on('blur', function () {
        validarClave();
    });
    $("[name='ACCESOAD']").on('change', function () {
        $('[data-valmsg-for="NameRepeated"]').html("");
        var namee = $("[name='ACCESOAD']:eq(0) option:selected").attr('data-name') == undefined ? "" : $("[name='ACCESOAD']:eq(0) option:selected").attr('data-name');
        $("[name='NOMBREAD']:eq(0)").val(namee);
    });
    //$("[name='UsuarioInterno']").on('change', function () {
    //    if ($("[name='UsuarioInterno']").eq(0).find("option:selected").eq(0).val() == 0) {
    //        $('.tipoExterno').show();
    //        $('.tipoInterno').hide();
    //    }
    //    else {
    //        $('.tipoExterno').hide();
    //        $('.tipoInterno').show();
    //        //getUsersAD();
    //    }
    //});
    $(".btnCambioPass").on("click", function () {
        var id = String(JSON.parse($(this).attr("data-json-selected")).IdUsuario).trim();
        $("#formCambiarClave .hiddenId").val(id);
    });

    function validarClave() {
        if ($("[name='Contrasena']:eq(0)").val().length < 6) {
            $('[data-valmsg-for="passLength"]').show();
            return false;
        } else
            $('[data-valmsg-for="passLength"]').hide();
        if ($("[name='Contrasena']:eq(0)").val() != $("[name='passRepeat']:eq(0)").val()) {
            $('[data-valmsg-for="passRepeat"]').show();
            return false;
        } else
            $('[data-valmsg-for="passRepeat"]').hide();
        return true;
    }
    //Cambio de clave
    function validarCambioClave() {
        if (validarClave() == false) {
            return false;
        }
        return true;
    }//Init    
    if (tipoInterno == 1) {
        $("[name='ACCESOAD']:eq(0)").val(accesoUsuario);
    }
    else {
        $('.tipoInterno').hide();
    }
    
});