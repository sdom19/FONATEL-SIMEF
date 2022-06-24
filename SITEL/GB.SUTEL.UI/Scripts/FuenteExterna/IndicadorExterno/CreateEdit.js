$(document).ready(function () {
    AjaxifyMyForm("formCreate", onSuccess, onFailure, null, validateForm);
    AjaxifyMyForm("formEdit", onSuccessEdit, onFailure, null, validateForm);
    $("[name='Nombre']").on("keyup", function () {
        $('[data-valmsg-for="NameRepeated"]').hide().parent().removeClass("has-error");
    });
    function onSuccess(data, formName) {
        $("[name='error']").hide();
        var jData = JSON.parse(data);
        $(".modal").modal('hide');
        $("form").trigger("reset");
        $("#formFilter").submit();
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        addSuccess({ msg: jData.strMensaje });
    }
    function onSuccessEdit(data, formName) {
        $("[name='error']").hide();
        var jData = JSON.parse(data);
        $(".modal").modal('hide');
        $("#formFilter").submit();
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        addSuccess({ msg: JSON.parse(jData.strMensaje).msg });
        var url = document.URL.split("/");
        var allurl = url[0] + "//" + url[2] + "/" + JSON.parse(jData.strMensaje).url;
        window.location.href = allurl;
    }
    function onFailure(error, formName) {
        if (error.status == 505)
            $('[data-valmsg-for="NameRepeated"]').show().parent().addClass("has-error");
        else             
            $("[name='error']").show().html(error.statusText);
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
    }
    function validateForm(formName) {       
        //var reg = new RegExp("[^\d.]");
        //console.log(reg.test($('[name="ValorIndicador"]').val()));
        //if (reg.test($('[name="ValorIndicador"]').val() == false))
        //    return false;
        //return false;
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message")).prop("disabled", true);
        return true;
    }    
});