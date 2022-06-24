$(document).ready(
    function () {
        if (typeof initDataGrid !== 'undefined') {
            $('table[data-table-grid="true"]').ready(initDataGrid);
        }
        $("[name='ROLES']").on('click', function () {
            if ($(this).is(":checked")) {
                $(this).val($(this).attr("data-id"));
            } else {
                $(this).val("");
            }
        });
        AjaxifyMyForm("formRoles",
                onSuccessEditarRol,
                onFailureEditarRol);
        function onSuccessEditarRol(data) {
            $("[name='error']").hide().html("");
            var jsonData = JSON.parse(data);
            if (parseInt(jsonData.state) == 200) {
                addSuccess({ msg: jsonData.strMensaje });
            } else {
                $("#formRoles [name='error']").show().html(jsonData.strMensaje);
            }
            var roles = [];
            $("[name='ROLES']:checked").each(function (i, e) {
                roles.push(parseInt($(this).attr("data-id")));
            });
            var userUpdatedId = $("#IdUsuario").val();
            $("#tablaUsuarios tbody button").each(function (i, e) {
                json = JSON.parse($(this).attr('data-json-selected'));
                if (String(json.IdUsuario).trim() == String(userUpdatedId).trim()) {
                    json.Roles = roles;
                    $(this).attr('data-json-selected', JSON.stringify(json));
                }
            });
            $("#modalRol").modal('hide');
        }
        function onFailureEditarRol(data) {
            $("#formRoles [name='error']").show().html(data.statusText);
        }
        /*ELIMINAR*/
        AjaxifyMyForm("formEliminarUsuario",
        SuccessEliminarUsuario,
        FailureEliminarUsuario);        
        function SuccessEliminarUsuario(data) {                        
            jsonData = JSON.parse(data);
            $("[name='error']").hide().html("");
            if (parseInt(jsonData.state) == 200) {
                $("#modalEliminar").modal('hide');
                $("#frmFilterUsuario").submit();
                addSuccess({ msg: "La información se eliminó con éxito" });
            }
            else {
                $("#formEliminarUsuario [name='error']").show().html(jsonData.strMensaje);
            }
        }
        function FailureEliminarUsuario(error) {
            $("#formEliminarUsuario [name='error']").show().html(error.statusText);
        }
        /*end ELIMINAR*/

        function firstIniAjaxInit() {
            initRows();
            $('#frmFilterUsuario input').keyup(function (event) {
                if (event.keyCode === 13) {
                    $('#frmFilterUsuario').submit();
                }
            });
            AjaxifyMyForm("frmFilterUsuario",
            function (data) {
                data = "<div id='tablaUsuarios'>" + data + "</div>";
                var $form = $(data);
                $("#tablaUsuarios").replaceWith(data);
                $('table[data-table-grid="true"]').ready(initDataGrid);
                firstIniAjaxInit();                
            },
            function (error) {

            }, 10000);
            $('.dataTable').on('draw.dt', function () {                                                    
                initRows();
            }); 
        }
        function initRows() {            
            $('.modal').on('hidden.bs.modal', function () {
                $("[name='error']").hide().html("");
            });
            $("[data-target='#modalRol']").on('click', function () {
                var nombre = JSON.parse($(this).attr("data-json-selected")).NombreUsuario;                
                var rolIds = JSON.parse($(this).attr("data-json-selected")).Roles;
                var IdUsuario = JSON.parse($(this).attr("data-json-selected")).IdUsuario;
                $("[data-id='IdUsuario']").val(IdUsuario);
                $("[data-id='NombreUsuario']").html(nombre);
                $("#formRoles input[type='checkbox']").prop("checked", false);
                $("#formRoles input[type='checkbox']").val("");
                $("input[type='checkbox']").each(function (i, e) {
                    for (var i = 0; i < rolIds.length; i++) {
                        if ($(e).attr("data-id") == rolIds[i]) {
                            $(e).prop("checked", true);
                            $(e).val(rolIds[i]);
                        }
                    }
                });
            });
            $("button.btnEliminar").on("click", function () {
                var id = JSON.parse($(this).attr("data-json-selected")).IdUsuario.trim();
                $("#modalEliminar [name='IdUsuario']").val(id);
            });
        }
        firstIniAjaxInit();
    });
function consultaPermisosPorRol(idRol) {

    $("#divOpcionesAccionPermisos").removeClass("hidden");
    var idRol = idRol;

    $.ajax({
        "dataType": 'json',
        "type": "POST",
        "url": "Rol/ConsultaPermisosPorRol",
        "data": "idRol=" + idRol,
        "success": function (JSONdata) {
            console.log(JSONdata);
        }
    });
};
function editRedirect(actionUrl, id) {
    var url = document.URL.split("/");
    var allurl = url[0] + "//" + url[2] + "/" + actionUrl + "?id=" + id.trim();
    window.location.href = allurl;
}