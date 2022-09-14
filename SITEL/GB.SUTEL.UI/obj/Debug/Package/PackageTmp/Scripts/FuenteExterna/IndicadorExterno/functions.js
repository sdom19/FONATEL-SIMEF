$(document).ready(function () {
    AjaxifyMyForm("formCreate", onSuccess, onFailure, null, validateForm);
    AjaxifyMyForm("formEdit", onSuccessEdit, onFailure, null, validateForm);
    function onSuccess(data, formName) {
        $("[name='error']").hide();
        var jData = JSON.parse(data);
        $("form").trigger("reset");
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        addSuccess({ msg: jData.strMensaje });
    }
    function onSuccessEdit(data, formName) {
        $("[name='error']").hide();
        var jData = JSON.parse(data);
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        addSuccess({ msg: JSON.parse(jData.strMensaje).msg });
        var url = document.URL.split("/");
        var allurl = url[0] + "//" + url[2] + "/" + JSON.parse(jData.strMensaje).url;
        window.location.href = allurl;
    }
    function onFailure(error, formName) {
        $("[name='error']").show().html(error.statusText);
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
    }
    $('#btnDus').on("click", validateForm);
    function validateForm(formName) {
        var decimalValidation = $('[name="ValorIndicador"]').val().match(/^\d{0,20}(\.\d{0,2}){0,1}$/);        
        if (!decimalValidation) {
            $('[data-valmsg-for="ValorIndicador"]').html("<span>" + InvalidDoubleFormat + "</span>");
            return false;
        }
        //return false;
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message")).prop("disabled", true);
        return true;
    }
    $('[name="ValorIndicador"]').on("keydown", function (evt) {
        var key = evt.keyCode;
        if (($.inArray(key, [9, 8, 46, 13, 37, 39]) == -1)&&
            (key < 96 || key > 105)&&
            (key < 48 || key > 57)&&
            (key!=110)&&
            (key!=190))
        //if (!((key > 47 && key < 58) || (key > 96 && key < 106) || (key == 110 || key == 190)) || ($.inArray(key, [9, 8, 46, 13, 37, 39]) == -1))
            evt.preventDefault();
        //var decimalValidation = $('[name="ValorIndicador"]').val().match(/^\d{0,20}(\.\d{0,1}){0,1}$/);
        //if (!decimalValidation && ($.inArray(evt.keyCode, [9, 8, 46, 13, 37, 39])==-1))
        //    evt.preventDefault();
    });
    $('[name="ValorIndicador"]').on("keyup", function (evt) {
        var decimalValidation = $('[name="ValorIndicador"]').val().match(/^\d{0,20}(\.\d{0,2}){0,1}$/);
        if (!decimalValidation)
            $('[name="ValorIndicador"]').val($('[name="ValorIndicador"]').val().substr(0, ($('[name="ValorIndicador"]').val().length-1)));
    });

    $('[name="ValorIndicador"]').val($('[name="ValorIndicador"]').val().toString().replace(",","."));
    $('.required').rules('add', {
        required: true,
        messages: {
            required: RequiredMessage
        }
    });
    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);            
            return this.optional(element) || re.test(value);
        },
        InvalidDoubleFormat
    );
    $('[name="ValorIndicador"]').rules("add", { regex: "[0-9]+(\.[0-9][0-9]?)?" })
    $('[name="ValorIndicador"]').rules('add', {
        messages: {
            number: InvalidDoubleFormat
        }
    });
    //$('[name="ValorIndicador"]').rules('add', {
    //    maxlength: 9,
    //    messages: {
    //        maxlength: IntLengthMessage
    //    }
    //});


    $("[name='IdFuenteExterna']").on('change', function (ev) {
        var selected = $(this).val();
        if (selected == "") {
            $("[name='IdIndicadorExterno'] option").hide();
            $("[name='IdIndicadorExterno'] option:eq(0)").show().prop("selected",true);
        } else {
            $("[name='IdIndicadorExterno'] option").each(function (i, e) {                
                var fuente = $(e).attr("data-fuente-externa");
                if (selected == fuente)
                    $(e).show();
                else
                    $(e).hide();
            });            
            if(!($("[name='IdIndicadorExterno'] option:selected").css("display")!="none"))
                $("[name='IdIndicadorExterno'] option:eq(0)").show().prop("selected", true);
        }
    });
    $("[name='IdFuenteExterna']").trigger("change");
});