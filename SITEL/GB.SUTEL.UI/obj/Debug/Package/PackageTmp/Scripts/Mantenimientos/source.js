$(document).ready(function () {
    if (typeof initDataGrid !== 'undefined') {
        $('table[data-table-grid="true"]').ready(initDataGrid);
    }
    AjaxifyMyForm("formCreate", onSuccess, onFailure, null, validateForm);
    AjaxifyMyForm("formEdit", onSuccess, onFailure, null, validateForm);
    AjaxifyMyForm("formEliminar", onSuccess, onFailure, null, validateForm);
    function onSuccess(data, formName) {
        var jData = JSON.parse(data);
        $(".modal").modal('hide');
        $("form").trigger("reset");
        $("#formFilter").submit();
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        addSuccess({ msg: jData.strMensaje });        
    }
    function onFailure(error, formName) {
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message-default")).prop("disabled", false);
        $("[name='error']").show().html(error.statusText);        
    }
    function validateForm(formName) {        
        $("#" + formName + " [type='submit']").val($("#" + formName).attr("data-ajax-message")).prop("disabled", true);
        return true;
    }
    function initTable() {
        initRows();
        $('#formFilter input').keyup(function (event) {
            if (event.keyCode === 13) {
                $('#formFilter').submit();
            }
        });
        AjaxifyMyForm("formFilter",
            function (data) {                
                data = "<div id='tabla'>" + data + "</div>";
                var $form = $(data);
                $("#tabla").replaceWith(data);
                $('table[data-table-grid="true"]').ready(initDataGrid);
                initTable();
            },
            function (error) {                
            });
        $('.dataTable').on('draw.dt', function () {
            initRows();
        });
    }
    function initRows() {
        $('.modal').on('hidden.bs.modal', function () {
            $("[name='error']").hide().html("");
            $("form").trigger("reset");
        });
        $("button.btnEliminar").on("click", function () {
            var id = JSON.parse($(this).attr("data-json-selected")).id.trim();
            $("#modalEliminar [name='id']").val(id);
        });
        $('form input').on('keyup', function () {
            $("[name='error']").hide().html("");
        });
        /*falta arreglar esto*/
        $('td[data-toggle="modal"]').on('click', function () {
            var nombre = JSON.parse($(this).attr("data-json-selected")).NOMBREEDITAR;            
            var id = JSON.parse($(this).attr("data-json-selected")).ID;
            $("[type='hidden']:not([name='__RequestVerificationToken'])").val(id);
            $("#NOMBREEDITAR").html(nombre.trim());            
        });
    }
    initTable();
});
function editRedirect(actionUrl, id) {
    var url = document.URL.split("/");
    var allurl = url[0] + "//" + url[2] + "/" + actionUrl + "?id=" + id.trim();
    window.location.href = allurl;
}