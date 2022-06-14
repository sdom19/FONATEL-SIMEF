function validateFunction(formName) {
    var valid = true;
    $("[role='alert']").hide();
    $("[data-valmsg-for='Servicio']").html("");
    if ($("[name='Servicio']").val() == "") {
        $("[data-valmsg-for='Servicio']").html(closureMessage().RequiredMessage);
        $("[name='submit']:eq(0)").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        valid = false;
    }
    if (Indicadores.length == 0) {
        $("[name='warning']").show().html(closureMessage().UnselectedItems);
        valid = false;
    }
    return valid;
}